/* File Name :- ConfigureLeaveTypesUI.aspx.cs
 * Modified By :- Deepak
 * Created Date :- 8-Feb-2010
 * Class Description :- This class is used to define staff leaves.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class ConfigureLeaveTypesUI : SchoolBase
{
    #region Constants 

    private const string S_DATAKEY_LEAVE_ID = "LeaveId";
    private const string S_DATAKEY_ORIGINAL_LEAVE_ID = "OriginalLeaveId"; 
    private const string S_DATAKEY_SCHOOL_ID = "SchoolId";
    private const string S_SELECT_CHECKBOX = "ChkSelect";
    private const string S_STAFF_LEAVE_TEXTBOX = "txtLeave";
    private const string S_SHORT_NAME_TEXTBOX = "txtShortName";
    private const string S_CAN_ACCUMULATE = "CanAccumulate";
    private const string S_CAN_ACCUMULATE_CHECKBOX = "ChkCanAccumulate";
    private const string S_CAN_APPLICABLE_STAFF_HOLIDAY = "ExcludeFromDeduction";
    private const string S_CAN_ALLOW_ZERO_BALANCE = "AllowZeroBalance";
    private const string S_CAN_APPLICABLE_STAFF_HOLIDAY_CHECKBOX = "chkApplicabletostaffholiday";
    private const string S_CAN_ALLOW_FOR_ZERO_BALANCE_CHECKBOX = "chkAllowZeroBalance";
    private const string S_IS_UNPAID_LEAVE = "IsUnpaidLeave";
    private const string S_CONSIDER_ON_DUTY = "IsODApplicable";
    private const string S_IS_UNPAID_LEAVE_CHECKBOX = "ChkIsUnpaidLeave";
    private const string S_CONSIDER_ON_DUTY_CHECKBOX = "ChkConsiderOnDuty";
    private const string S_ACCUMULATE_LEAVE_TEXTBOX = "txtAccumulateLeave";
    private const string S_MINIMUM_BALANCE_TEXTBOX = "txtMinimumBalance";
    private const string S_COLOR_CODE_DROPDOWNLIST = "cmbColorCode";
    private const string S_CURRENT_YEAR_ACCUMULATED_LEAVE_LABEL = "lblCurrentYearAccumulated";
    private const string S_DATAKEY_IS_UNPAID_LEAVE = "IsUnpaidLeave";
    private const string S_SAVE_MESSAGE = "Leave(s) saved successfully !!!";
    
    #endregion

    #region Data Member(s)

    private int miSaveCount = 0; 

    #endregion
    
    #region Events

    /// <summary>
    /// This event is used to fill leaves into listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
                FillLeavesGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set Select Leave and Can Accumulate checkbox according to previous saved value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStaffLeaves_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                DataRowView oDataRowView = (DataRowView)oCurrentItem.DataItem;

                CheckBox oChkSelect = ((CheckBox)oCurrentItem.FindControl(S_SELECT_CHECKBOX));                
                CheckBox oChkCanApplicableToStaff = (CheckBox)oCurrentItem.FindControl(S_CAN_APPLICABLE_STAFF_HOLIDAY_CHECKBOX);
                CheckBox oChkAllowZero = (CheckBox)oCurrentItem.FindControl(S_CAN_ALLOW_FOR_ZERO_BALANCE_CHECKBOX);
                CheckBox oChkUnPaidLeave = (CheckBox)oCurrentItem.FindControl(S_IS_UNPAID_LEAVE_CHECKBOX);
                CheckBox oChkConsiderOnDuty = (CheckBox)oCurrentItem.FindControl(S_CONSIDER_ON_DUTY_CHECKBOX);
                TextBox oTxtLeave = (TextBox)oCurrentItem.FindControl(S_STAFF_LEAVE_TEXTBOX);
                TextBox oTxtShortName = (TextBox)oCurrentItem.FindControl(S_SHORT_NAME_TEXTBOX);
                TextBox txtMinimumBalance = (TextBox)oCurrentItem.FindControl(S_MINIMUM_BALANCE_TEXTBOX);
                DropDownList cmbColorCode = (DropDownList)oCurrentItem.FindControl(S_COLOR_CODE_DROPDOWNLIST);

                FillColorCombo(cmbColorCode);
                cmbColorCode.SelectedValue = Convert.ToString(oDataRowView["ColorCode"]);
                cmbColorCode.BackColor = Color.FromName(cmbColorCode.SelectedValue);

                // If the school id is not the default id i.e. -9999 that means the staff leave is already assigned
                // to the school. Thus check the checkbox.
                if (lstvwStaffLeaves.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    oChkSelect.Checked = true;                    
                    if (Convert.ToInt32(lstvwStaffLeaves.DataKeys[iRowId][S_CAN_APPLICABLE_STAFF_HOLIDAY]) == 1)
                        oChkCanApplicableToStaff.Checked = true;

                    if (Convert.ToInt32(lstvwStaffLeaves.DataKeys[iRowId][S_CAN_ALLOW_ZERO_BALANCE]) == 1)
                        oChkAllowZero.Checked = true;

                    if (Convert.ToInt32(lstvwStaffLeaves.DataKeys[iRowId][S_IS_UNPAID_LEAVE]) == 1)
                        oChkUnPaidLeave.Checked = true;
                    else
                    {
                        oChkUnPaidLeave.Checked = false;
                    }

                    if (Convert.ToInt32(lstvwStaffLeaves.DataKeys[iRowId][S_CONSIDER_ON_DUTY]) == 1)
                        oChkConsiderOnDuty.Checked = true;
                    else
                    {
                        oChkConsiderOnDuty.Checked = false;
                    }
                    oChkSelect.Enabled = false;
                    hidSavedCount.Value = (++miSaveCount).ToString();
                }
                else
                {
                    oTxtLeave.Enabled = true;
                    oChkSelect.Enabled = true;
                }                

                bool bIsUnpaidLeave = Convert.ToBoolean(oDataRowView[S_DATAKEY_IS_UNPAID_LEAVE]);
                if (bIsUnpaidLeave)
                {
                    oChkSelect.Checked = true;
                    oChkSelect.Enabled = false;                    
                    oTxtLeave.Enabled = false;
                    txtMinimumBalance.Enabled = false;
                }
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save leaves and add configuration entry.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sMessage = ValidateShortName();
            if (sMessage == string.Empty)
            {
                Save();
                lblMessage.Visible = true;
				tblBasicLeaves.Visible = true;
                lblMessage.Text = S_SAVE_MESSAGE;
            }
            else
            {
                trErrorMessage.Visible = true;
                lblErrorMessage.Text = sMessage;
            }
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            trErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.Message;
            FillLeavesGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method
    
    /// <summary>
    /// This method is used to to check duplication of short names in payroll module. 
    /// </summary>
    /// <returns></returns>
    private string ValidateShortName()
    {
        StringBuilder oLeaveShortName = new StringBuilder();
        foreach (ListViewDataItem oCurrentItem in lstvwStaffLeaves.Items)        
        {   
            CheckBox oChkSelect = (CheckBox)oCurrentItem.FindControl(S_SELECT_CHECKBOX);
            TextBox oTxtShortName = (TextBox)oCurrentItem.FindControl(S_SHORT_NAME_TEXTBOX);
            if (oChkSelect.Checked)
                oLeaveShortName.Append("," + oTxtShortName.Text);
        }

        string sLeaveShortName = oLeaveShortName.ToString().Substring(1);
        string sMessage = string.Empty;
        if(!string.IsNullOrEmpty(sLeaveShortName))
            sMessage = EarningsDeductionsBL.ValidateShortName(miSchoolId, miAcademicYearId, sLeaveShortName, false);
        return sMessage;
    }

    /// <summary>
    /// This method is used to fill staff leaves into listview.
    /// </summary>
    private void FillLeavesGrid()
    {
        DataTable oDTStaffLeaves = StaffLeavesBL.GetAll(miSchoolId);
        lstvwStaffLeaves.DataSource = oDTStaffLeaves;
        lstvwStaffLeaves.DataBind();

        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwStaffLeaves.FindControl("trHeader");
        CheckBox oCheckBox = (CheckBox)oHtmlTableRow.FindControl("ChkSelectAll");
        oCheckBox.Focus();
    }

    /// <summary>
    /// This method is used to save configured leaves.
    /// </summary>
    private void Save()
    {   
        int iOriginalId  = Convert.ToInt32(Constants.SchoolConfigurations.StaffLeaves);
        StaffLeavesBL oStaffLeavesBL = new StaffLeavesBL();
        oStaffLeavesBL.ConfiguredLeave = PopulateBL();
        DataTable oDTConfiguredLeaves = oStaffLeavesBL.Save(iOriginalId, miAcademicYearId);
        lstvwStaffLeaves.DataSource = oDTConfiguredLeaves;
        lstvwStaffLeaves.DataBind();
    }

    /// <summary>
    /// This method is used to genrate XML of leave details.
    /// </summary>
    private string GenerateXML()
    {
        CheckBox oChkSelect;
        const string S_ELEMENT = "element";
        string sAttribute;

        XmlDocument oDoc = new XmlDocument();
        XmlElement oRoot = oDoc.CreateElement("StaffLeaves");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StaffLeaves", string.Empty);
        
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= lstvwStaffLeaves.Items.Count - 1; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstvwStaffLeaves.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            int iLeaveId = Convert.ToInt32(lstvwStaffLeaves.DataKeys[iRowId][S_DATAKEY_LEAVE_ID]);
            TextBox oTxtLeave = (TextBox)oCurrentItem.FindControl(S_STAFF_LEAVE_TEXTBOX);
            TextBox oTxtShortName = (TextBox)oCurrentItem.FindControl(S_SHORT_NAME_TEXTBOX);            
            CheckBox oChkCanApplicableToStaff = (CheckBox)oCurrentItem.FindControl(S_CAN_APPLICABLE_STAFF_HOLIDAY_CHECKBOX);
            CheckBox oChkAllowZeroBalance = (CheckBox)oCurrentItem.FindControl(S_CAN_ALLOW_FOR_ZERO_BALANCE_CHECKBOX);
            CheckBox oChkUnPaidLeave = (CheckBox)oCurrentItem.FindControl(S_IS_UNPAID_LEAVE_CHECKBOX);
            CheckBox oChkConsiderOnDuty = (CheckBox)oCurrentItem.FindControl(S_CONSIDER_ON_DUTY_CHECKBOX);
            TextBox txtMinimumBalance = (TextBox)oCurrentItem.FindControl(S_MINIMUM_BALANCE_TEXTBOX);
            DropDownList cmbColorCode = (DropDownList)oCurrentItem.FindControl(S_COLOR_CODE_DROPDOWNLIST);

            oChkSelect = (CheckBox)oCurrentItem.FindControl(S_SELECT_CHECKBOX);
            if (oChkSelect.Checked || lstvwStaffLeaves.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StaffLeaves", "");

                sAttribute = S_DATAKEY_LEAVE_ID;
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = iLeaveId.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "LeaveName";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oTxtLeave.Text.Trim();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = S_DATAKEY_ORIGINAL_LEAVE_ID;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Convert.ToString(lstvwStaffLeaves.DataKeys[iRowCount][S_DATAKEY_ORIGINAL_LEAVE_ID]);
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "ShortName";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = oTxtShortName.Text.Trim();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = S_CAN_ACCUMULATE;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = "False"; 
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "AccumulateLeave";
                oAttr = oDoc.CreateAttribute(sAttribute);                
                oAttr.Value = "0";
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "SchoolId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = lstvwStaffLeaves.DataKeys[iRowId][S_DATAKEY_SCHOOL_ID].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = S_DATAKEY_IS_UNPAID_LEAVE;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = lstvwStaffLeaves.DataKeys[iRowId][S_DATAKEY_IS_UNPAID_LEAVE].ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "MinimumBalance";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = txtMinimumBalance.Text;
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "ColorCode";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = cmbColorCode.SelectedValue;
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = S_CAN_APPLICABLE_STAFF_HOLIDAY;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Convert.ToString(oChkCanApplicableToStaff.Checked);
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = S_CAN_ALLOW_ZERO_BALANCE;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Convert.ToString(oChkAllowZeroBalance.Checked);
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = S_IS_UNPAID_LEAVE;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Convert.ToString(oChkUnPaidLeave.Checked);
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = S_CONSIDER_ON_DUTY;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = Convert.ToString(oChkConsiderOnDuty.Checked);
                oXmlNode.Attributes.Append(oAttr);

                
                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }
        // Add the root node to document element.         
        oRoot.AppendChild(oXmlRootNode);

        // return the string generated.
        return oRoot.InnerXml;
    }

    /// <summary>
    /// This method is used to set javascript attributes and postback url to cancel button.
    /// </summary>
    protected void SetJavascriptAttributes()
    {
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
        valSummLeaves.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        BtnSave.Attributes.Add("onclick", "if(!CheckSelectedLeaves(this)) return false;if(!ConfirmInsert()) return false;");
        lnkUserLeaves.Attributes.Add("onclick", "OpenPopup(); return false;");
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnCancel });

        tblBasicLeaves.Visible = QueryString["Is_Configured"] == Constants.S_YES;      
    }

    /// <summary>
    /// This method is used to populate object of StaffLeavesBL,
    /// which is used to save staff leaves configuration.
    /// </summary>
    /// <param name="asFieldValue"></param>
    /// <param name="asIsPrePrimary"></param>
    /// <param name="aiSectionId"></param>
    /// <param name="aiOriginalFieldId"></param>
    /// <returns></returns>
    private ConfiguredLeaves PopulateBL()
    {
        return new ConfiguredLeaves
        {
            SchoolId = miSchoolId,
            InsertedById = miUserId,
            UpdatedById = miUserId,
            LeaveXML = GenerateXML()
        };        
    }

    /// <summary>
    /// This method is used to fill color combobox;
    /// </summary>
    /// <param name="cmbColors"></param>
    private void FillColorCombo(DropDownList cmbColors)
    {
        cmbColors.Attributes.Add("onchange", "SetColorPayPeriod(this)");

        Type tColors = typeof(Color);
        PropertyInfo[] oPropInfoArr = tColors.GetProperties(BindingFlags.Static | BindingFlags.Public);
        foreach (PropertyInfo oProperty in oPropInfoArr)
        {
            if (oProperty.DeclaringType.Equals(typeof(Color)))
                cmbColors.Items.Add(new ListItem(oProperty.Name, oProperty.Name));
        }

        RemoveUnwantedColors(cmbColors);
    }

    /// <summary>
    /// This method is used to remove unwanted color(s).
    /// </summary>
    /// <param name="cmbColors"></param>
    private void RemoveUnwantedColors(DropDownList cmbColors)
    {
        ListItem oLst = new ListItem(Color.Transparent.Name, Color.Transparent.Name);
        cmbColors.Items.Remove(oLst);
    }

    #endregion
}
