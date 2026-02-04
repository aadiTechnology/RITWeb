using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

/// <summary>
/// This class is used to assign values to Earnings Deductions according to pay scale as well as set additional formula to earning deduction.
/// </summary>
public partial class UsersEarningsAndDeductions : SchoolBase
{
    #region Constants

    private const string S_MESSAGE = "These values are not yet saved.";
    private const int I_FORMULA_RANGE_TABLE = 4;
    private const int I_MODIFICATION_REASON_TABLE_INDEX = 5;
    private const int I_DEFAULT_FORMULA_TABLE = 6;
    private const string S_FORMULA_RANGE_TABLE = "FromulaRangeTable";
    private const int I_FIRST = 0;
    
    #endregion

    #region Data Member(s)

    private UsersEarningsDeductionsBL moUsersEarningsDeductionsBL;
    private DataTable moDTDefaultFormulaOrRange = null; 

    #endregion

    #region Events

    /// <summary>
    /// This event is used to check precondition, decrypt query string, fill pay scale combo box  and 
    /// fill earnings deductions list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUsersEarningsDeductionsBL = new UsersEarningsDeductionsBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    ReadQuerystring();
                    FillPayScaleCombobox();                    
                    InitializeFields();
                    FillEarningsDeductions();
                }

                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save users earnings - deductions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moUsersEarningsDeductionsBL.UsersEarningsDeduction = Populate();
            moUsersEarningsDeductionsBL.Insert();
            ClosePopupWindow();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill earning-deduction into list view according to pay scale.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPayScale_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillEarningsDeductions();
            hidLastSelectedPayScaleId.Value = cmbPayScale.SelectedValue;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill formula/ranges according to respective earning-deduction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwFormulaAndRangeED_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
            DropDownList ddlFormula = oCurrentItem.FindControl("ddlFormula") as DropDownList;
            HiddenField hidFormula = oCurrentItem.FindControl("hidFormula") as HiddenField;
            int iEarningDeductionId = Convert.ToInt32(lstvwFormulaAndRangeED.DataKeys[oCurrentItem.DisplayIndex]["EarningsDeductionsId"]);
            DataTable oDataTable = null;

            if (ViewState[S_FORMULA_RANGE_TABLE] != null)
            {
                oDataTable = ViewState[S_FORMULA_RANGE_TABLE] as DataTable;
                if (oDataTable.Select("EarningsDeductionsId = " + iEarningDeductionId).Length > 0)
                {
                    DataTable oDTRequired = oDataTable.Select("EarningsDeductionsId = " + iEarningDeductionId).CopyToDataTable();
                    ControlUtility.FillDropDownList(oDTRequired, ref ddlFormula, "FormularangeId", "FormularangeName", Constants.S_SELECT);
                    string sFormulaRangeId = lstvwFormulaAndRangeED.DataKeys[oCurrentItem.DisplayIndex]["FormulaRangeId"].ToString();
                    ddlFormula.SelectedValue = sFormulaRangeId;
                    hidFormula.Value = sFormulaRangeId;
                }
                else
                {
                    ListItem oListItem = new ListItem(Constants.S_SELECT, Constants.S_ZERO);
                    ddlFormula.Items.Add(oListItem);
                }
            }

            if (moDTDefaultFormulaOrRange.IsNonEmpty())
            {
                Label lblDefaultFormula = oCurrentItem.FindControl("lblDefaultFormula") as Label;
                int iEDNo = Convert.ToInt32(lstvwFormulaAndRangeED.DataKeys[oCurrentItem.DisplayIndex]["EarningsDeductionsId"].ToString());
               
                var oFormulae = from sformula in moDTDefaultFormulaOrRange.AsEnumerable()
                               where sformula.Field<int>("EarningsDeductionsId") == iEDNo
                               select sformula.Field<string>("FormulaValue");

                string sDefaultFormulaOrRange = string.Empty;
                if (oFormulae != null)
                    sDefaultFormulaOrRange = oFormulae.FirstOrDefault();

                lblDefaultFormula.Text = sDefaultFormulaOrRange.Replace(",", "<BR />");
            }
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to fill pay scale combo box.
    /// </summary>
    private void FillPayScaleCombobox()
    {
        const int I_CONFIG_DETAILS = 0;
        const int I_CURRENT_CONFIG_ID = 1;

        int iUserId = Convert.ToInt32(hidUserId.Value);
        DataSet oDSPayScales = moUsersEarningsDeductionsBL.GetPayScaleSettings(iUserId);

        if (oDSPayScales != null)
        {
            DataTable oDTPayScales = oDSPayScales.Tables[I_CONFIG_DETAILS];
            ControlUtility.FillDropDownList(oDTPayScales, ref cmbPayScale, "PayScaleSettingsId", "PayScaleName", Constants.S_SELECT);
            if (cmbPayScale.Items.Count > 1)
            {
                int iCurrentPayId = 0;
                if (oDSPayScales.Tables[I_CURRENT_CONFIG_ID] != null)
                {
                    iCurrentPayId = Convert.ToInt32(oDSPayScales.Tables[I_CURRENT_CONFIG_ID].Rows[I_FIRST]["PayScaleSettingsId"]);
                    cmbPayScale.SelectedValue = iCurrentPayId.ToString();
                    hidActivePayScaleId.Value = iCurrentPayId.ToString();
                    hidLastSelectedPayScaleId.Value = iCurrentPayId.ToString();
                }
            }
        }
        FillPayMatrixCombobox(oDSPayScales);
    }

    /// <summary>
    /// This method is used to set user details.
    /// </summary>
    /// <param name="oDSPayScales"></param>
    private void SetUserDetails(DataSet aoDSPayScales)
    {
        const int I_USER_DETAILS = 2;
        DataTable oDTUserDetails = aoDSPayScales.Tables[I_USER_DETAILS];
        if (oDTUserDetails.IsNonEmpty())
        {
            hidUserRoleId.Value = oDTUserDetails.Rows[I_FIRST]["User_Role_Id"].ToString();
            hidStaffGroupId.Value = oDTUserDetails.Rows[I_FIRST]["StaffGroupsId"].ToString();
            hidStaffGroupName.Value = oDTUserDetails.Rows[I_FIRST]["StaffGroupsName"].ToString();
            hidUserName.Value = oDTUserDetails.Rows[I_FIRST]["Name"].ToString();
        }
    }

    /// <summary>
    /// This method is used to fill Earnings - Deductions.
    /// </summary>
    private void FillEarningsDeductions()
    {   
        int iUserId = Convert.ToInt32(hidUserId.Value);
        int iStaffGroupId = Convert.ToInt32(hidStaffGroupId.Value);
        int iPayScaleSettingsId = Convert.ToInt32(cmbPayScale.SelectedValue);
        DataSet oDSEarningsDeductions = moUsersEarningsDeductionsBL.GetAll(iUserId, iStaffGroupId, iPayScaleSettingsId);

        if (oDSEarningsDeductions != null && oDSEarningsDeductions.Tables.Count > 0)
        {
            FillEarningDeductionConfig(oDSEarningsDeductions);
            SetRecordCount(oDSEarningsDeductions);
            FillEDFormulaRangeGrid(oDSEarningsDeductions);
        }

        DisplayMessage();
    }

    /// <summary>
    /// This method is used to set record count.
    /// </summary>
    /// <param name="oDSEarningsDeductions"></param>
    private void SetRecordCount(DataSet aoDSEarningsDeductions)
    {
        const int I_RECORD_COUNT = 1;
        const int I_DISPLAY_MESSAGE_TABLE_INDEX = 2;
        DataTable oDTRecordCount = aoDSEarningsDeductions.Tables[I_RECORD_COUNT];
        if (oDTRecordCount.IsNonEmpty())
            hidRecordCount.Value = oDTRecordCount.Rows[I_FIRST][0].ToString();
        hidDisplayMessage.Value = aoDSEarningsDeductions.Tables[I_DISPLAY_MESSAGE_TABLE_INDEX].Rows[I_FIRST][0].ToString();

        DataTable oDTReason = aoDSEarningsDeductions.Tables[I_MODIFICATION_REASON_TABLE_INDEX];
        if (oDTReason.IsNonEmpty())
            txtReason.Text = oDTReason.Rows[0][0].ToString();
        else
            txtReason.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to fill earning-deduction configuration grid.
    /// </summary>
    /// <param name="oDSEarningsDeductions"></param>
    private void FillEarningDeductionConfig(DataSet aoDSEarningsDeductions)
    {
        const int I_EARNINGS_DEDUCTIONS_TABLE_INDEX = 0;
        if (aoDSEarningsDeductions.Tables[I_EARNINGS_DEDUCTIONS_TABLE_INDEX] != null && aoDSEarningsDeductions.Tables[I_EARNINGS_DEDUCTIONS_TABLE_INDEX].Rows.Count != 0)
        {   
            HideControls(false);
            DataTable oDataTable = aoDSEarningsDeductions.Tables[I_EARNINGS_DEDUCTIONS_TABLE_INDEX];
            DataRow[] datarows = oDataTable.Select("UsersEarningsDeductionsId <> 0");
            if (datarows.Length == Constants.I_ZERO)
                hidUsersEarningsDeductionsId.Value = Constants.S_ZERO;
            lstvwEarningsDeductions.DataSource = aoDSEarningsDeductions.Tables[I_EARNINGS_DEDUCTIONS_TABLE_INDEX];
            lstvwEarningsDeductions.DataBind();

            if (hidActivePayScaleId.Value == cmbPayScale.SelectedValue)
            {
                chkIsActive.Checked = true;
                chkIsActive.Enabled = false;
            }
            else
            {
                chkIsActive.Checked = false;
                chkIsActive.Enabled = true;
            }
        }
        else
            HideControls(true);         
    }

    /// <summary>
    /// This method is used to fill earning deduction formula/range list view.
    /// </summary>
    /// <param name="oDSEarningsDeductions"></param>
    private void FillEDFormulaRangeGrid(DataSet aoDSEarningsDeductions)
    {
        const int I_EARNING_DEDUCTION_FORMULAANDRANGE = 3;
        ViewState[S_FORMULA_RANGE_TABLE] = aoDSEarningsDeductions.Tables[I_FORMULA_RANGE_TABLE];
        DataTable oDTFormulaAndRangeED = aoDSEarningsDeductions.Tables[I_EARNING_DEDUCTION_FORMULAANDRANGE];

        if (oDTFormulaAndRangeED.IsNonEmpty())
        {
            moDTDefaultFormulaOrRange = aoDSEarningsDeductions.Tables[I_DEFAULT_FORMULA_TABLE];
            lstvwFormulaAndRangeED.DataSource = oDTFormulaAndRangeED;
            lstvwFormulaAndRangeED.DataBind();
        }        
    }

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.UsersEarningsDeductions);
        if (!sLinks.Equals(string.Empty))
        {
            divErr.InnerHtml = sLinks;
            divErr.Attributes.Add("onclick", "return false;");
            HideControls(true);
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to hide controls when either staff groups - earnings deductions association are not configured.
    /// </summary>
    private void HideControls(bool abAction)
    {
        trValSummary.Visible = !abAction;
        //trErrorMessage.Visible = !abAction;
        trRole.Visible = !abAction;
        trlistview.Visible = !abAction;
        BtnSave.Visible = !abAction;
        //chkNewFormulaToAll.Visible = !abAction;
        //chkApplyToAll.Visible = !abAction;
    }

    /// <summary>
    /// This method is used to display message
    /// </summary>
    private void DisplayMessage()
    {
        if (lstvwEarningsDeductions.Items.Count > 0)
        {
            lblWarningMessage.Text = string.Empty;
            if (hidDisplayMessage.Value == Constants.S_YES)
                lblWarningMessage.Text = S_MESSAGE;
        }
    }

    /// <summary>
    /// This method is used to initialize fields.
    /// </summary>
    private void InitializeFields()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        lblUserName.Text = hidUserName.Value;
        HidApplyToAllUsersOfStaffGroup.Value = Constants.S_NO;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {   
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnCancel });
        //BtnSave.Attributes.Add("onclick", "DisableButtons()");
        btnCancel.Attributes.Add("onclick", "window.close();");
        cmbPayScale.Attributes.Add("onchange","if(!ConfirmChange()) return false;");
    }

    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    /// <returns></returns>
    private void ReadQuerystring()
    {
        hidIsConfigured.Value = QueryString["Is_Configured"];
        hidUserId.Value = QueryString["UserId"];

        if (QueryString != null)
            hidUserRoleId.Value = QueryString["UserRoleId"];

        hidStaffGroupId.Value = QueryString["StaffGroupId"];
        hidStaffGroupName.Value = QueryString["StaffGroupsName"];
        hidUserName.Value = QueryString["UserName"];
        hidFilter.Value = QueryString["Filter"];
        if (QueryString["IsLocked"] == "Y")
            BtnSave.Enabled = false;

        chkIsActive.Checked = false;
    }

    /// <summary>
    /// This method is used to encrypt query string and close popup.
    /// </summary>
    private void ClosePopupWindow()
    {
        string sQueryString = "UserRoleId=" + hidUserRoleId.Value +
                              "&Is_Configured=" + hidIsConfigured.Value +
                              "&Filter=" + hidFilter.Value;

        sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
        sQueryString = "'?" + sQueryString + "'";
        Response.Write("<Script language='Javascript'>window.opener.location=window.opener.location.pathname+" + sQueryString + ";window.close();window.opener.focus(); </Script>");
    }

    /// <summary>
    /// This method is used to populate UsersEarningsDeductionsBL object.
    /// </summary>
    /// <returns></returns>
    private UsersEarningsDeduction Populate()
    {
        return new UsersEarningsDeduction
        {
            UserId = Convert.ToInt32(hidUserId.Value),
            StaffGroupId = Convert.ToInt32(hidStaffGroupId.Value),
            PayScaleSettingId = Convert.ToInt32(cmbPayScale.SelectedValue),  
            PayMatrixId = Convert.ToInt32(cmbPayMatrix.SelectedValue),
            Reason = txtReason.Text.Trim(),
            //ApplyFormulaToAllUsersOfStaffGroup = chkNewFormulaToAll.Checked ? 'Y' : 'N',
            ApplyFormulaToAllUsersOfStaffGroup = 'N',
            //ApplyToAllUsersOfStaffGroup = Convert.ToChar(HidApplyToAllUsersOfStaffGroup.Value),
            ApplyToAllUsersOfStaffGroup = 'N',
            EarningsDeductionsXml = GenerateXml(),
            IsActivePayScale = chkIsActive.Checked,
            FormulaAndRangeXml = GenerateFormulaAndRangexml()
        };        
    }

    /// <summary>
    /// This method is used to generate formula and range XML.
    /// </summary>
    /// <returns></returns>
    private string GenerateFormulaAndRangexml()
    {
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("FormulaAndRange");

        XmlNode oXmlRootNode = oDoc.CreateNode("element", "FormulaAndRange", string.Empty);

        foreach (ListViewDataItem oCurrentItem in lstvwFormulaAndRangeED.Items)
        {
            int iUsersFormulaRangeId = Convert.ToInt32(lstvwFormulaAndRangeED.DataKeys[oCurrentItem.DisplayIndex]["UsersFormulaRangeId"]);
            DropDownList ddlFormula = oCurrentItem.FindControl("ddlFormula") as DropDownList;
            HiddenField hidFormula = oCurrentItem.FindControl("hidFormula") as HiddenField;

            XmlNode oXmlNode = oDoc.CreateNode("element", "FormulaAndRange", string.Empty);
            XmlAttribute attr = oDoc.CreateAttribute("UsersFormulaRangeId");
            attr.Value = iUsersFormulaRangeId.ToString();
            oXmlNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("FormulaRangeId");
            attr.Value = ddlFormula.SelectedValue;
            oXmlNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("IsFormula");
            attr.Value = lstvwFormulaAndRangeED.DataKeys[oCurrentItem.DisplayIndex]["IsFormula"].ToString();
            oXmlNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("IsDeleted");
            attr.Value = Constants.S_NO;
            if (ddlFormula.SelectedValue == Constants.S_ZERO)
                attr.Value = Constants.S_YES;
            oXmlNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("IsNew");
            attr.Value = Constants.S_YES;
            if (iUsersFormulaRangeId != 0)
                attr.Value = Constants.S_NO;
            oXmlNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("EarningsDeductionsId");
            attr.Value = lstvwFormulaAndRangeED.DataKeys[oCurrentItem.DisplayIndex]["EarningsDeductionsId"].ToString();
            oXmlNode.Attributes.Append(attr);

            attr = oDoc.CreateAttribute("OldValue");
            attr.Value = hidFormula.Value;
            oXmlNode.Attributes.Append(attr);

            oXmlRootNode.AppendChild(oXmlNode);
        }

        root.AppendChild(oXmlRootNode);
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to return xml of Earnings Deductions.
    /// </summary>
    /// <returns></returns>
    private string GenerateXml()
    {
        const string S_ELEMENT = "element";
        string sAttribute;

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("EarningsDeductions");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "EarningsDeductions", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwEarningsDeductions.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwEarningsDeductions.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;

            int iUsersEarningsDeductionsId = Convert.ToInt32(lstvwEarningsDeductions.DataKeys[iRowCount]["UsersEarningsDeductionsId"]);
            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "EarningsDeductions", string.Empty);

            sAttribute = "UsersEarningsDeductionsId";
            XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
            attr.Value = iUsersEarningsDeductionsId.ToString();
            oXmlNode.Attributes.Append(attr);

            sAttribute = "EarningsDeductionsId";
            attr = oDoc.CreateAttribute(sAttribute);
            int iEarningsDeductionsId = Convert.ToInt32(lstvwEarningsDeductions.DataKeys[iRowCount]["EarningsDeductionsId"]);
            attr.Value = iEarningsDeductionsId.ToString();
            oXmlNode.Attributes.Append(attr);

            sAttribute = "EarningsDeductionsValue";
            attr = oDoc.CreateAttribute(sAttribute);
            TextBox txtValue = (TextBox)oCurrentItem.FindControl("txtValue");
            decimal iValue = Convert.ToDecimal(txtValue.Text.Trim());
            attr.Value = iValue.ToString();
            oXmlNode.Attributes.Append(attr);

            // Add the node to root node.
            oXmlRootNode.AppendChild(oXmlNode);
        }

        // Add the root node to document element.         
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to fill pay Matrix combobox.
    /// </summary>
    /// <param name="dtDetails"></param>
    private void FillPayMatrixCombobox(DataSet dtDetails)
    {
        const int I_PAY_MATRIX_DETAILS = 3;
        const int I_CURRENT_MATRIX_ID = 4;

        DataTable dtPayScaleMatrix = dtDetails.Tables[I_PAY_MATRIX_DETAILS];
        if (dtPayScaleMatrix != null)
        {
            ControlUtility.FillDropDownList(dtPayScaleMatrix, ref cmbPayMatrix, "Id", "Matrix", Constants.S_SELECT);

            if (dtDetails.Tables[I_CURRENT_MATRIX_ID] != null)
            {
                int iCurrentMatrixId = 0;
                iCurrentMatrixId = Convert.ToInt32(dtDetails.Tables[I_CURRENT_MATRIX_ID].Rows[I_FIRST]["PaymatrixId"]);

                cmbPayMatrix.SelectedValue = iCurrentMatrixId.ToString();
            }
        }
    }

    #endregion
}