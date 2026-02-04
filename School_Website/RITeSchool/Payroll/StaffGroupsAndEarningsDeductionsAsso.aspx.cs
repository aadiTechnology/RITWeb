/* File Name :- StaffGroupsAndEarningsDeductionsAsso.aspx.cs
 * Created By :- Sachin
 * Created Date :- 26-Oct-2009
 * Class Description :- This class is used to define staff groups - Earnings Deductions association.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class StaffGroupsAndEarningsDeductionsAsso : SchoolBase
{
    #region Constants

    private const int I_ORIGINAL_STAFF_GROUP_ID_COLUMN_INDEX = 2;
    private const int I_STAFF_GROUP_ID_COLUMN_INDEX = 1;
    private const int I_STAFF_GROUP_NAME_COLUMN_INDEX = 3;
    private const int I_START_COUNT = 4;
    private const int I_FIRST_COLUMN_INDEX = 0;
    private const int I_FORMULA_TABLE = 0;
    private const int I_BASICID_TABLE = 1;
    private const int I_STAFFGROUPS_TABLE = 0;
    private const int I_EARNINGSDEDUCTIONS_TABLE = 1;
    
    private const string S_FORMULA_TABLE = "Formula";
    private const string S_BASICID_TABLE = "BasicId";
    private const string S_STAFF_GROUP_NAME_FIELD = "StaffGroupsName";
    private const string S_STAFFGROUPS_ID = "StaffGroupsId";
    private const string S_EARNINGSDEDUCTIONS_ID = "EarningsDeductionsId";

    #endregion

    #region Members

    private DataSet moDSAssociation = null;
    private StaffGroupsAndEarningsDeductionsAssociationBL moStaffGroupsAndEarningsDeductionsAssociationBL;

    #endregion
       
    #region Events

    /// <summary>
    /// This event is used to fill association grid.
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            moStaffGroupsAndEarningsDeductionsAssociationBL = new StaffGroupsAndEarningsDeductionsAssociationBL();
            if (!IsPostBack && CheckPreCondition())
                FillAssociationGrid();
            else
                FillAssociationGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill grid with staff groups, earnings and deductions and their association.
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
                SetScreenWidth();
                chkAll.Focus();
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set CSS style and names to grid checkboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdAssociation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int iRowIndex = e.Row.RowIndex;
            e.Row.Cells[I_FIRST_COLUMN_INDEX].CssClass = "locked";
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[I_FIRST_COLUMN_INDEX].CssClass = "Llocked";
                e.Row.Cells[I_FIRST_COLUMN_INDEX].Style.Add("left", grdAssociation.Style["scrollLeft"]);
            }
            if (iRowIndex >= 0)
            {
                string sName = grdAssociation.DataKeys[iRowIndex][S_STAFF_GROUP_NAME_FIELD].ToString();
                int iStaffGroupId = Convert.ToInt32(grdAssociation.DataKeys[iRowIndex][S_STAFFGROUPS_ID]);
                CheckBox chkCheckRow = ((CheckBox)e.Row.Cells[1].FindControl("CheckAllForRow"));
                chkCheckRow.Text = sName;
                int iRowNo = e.Row.RowIndex;
                int iEarningDeductionId = 0;
                if (moDSAssociation.Tables[I_EARNINGSDEDUCTIONS_TABLE] != null &&
                    moDSAssociation.Tables[I_EARNINGSDEDUCTIONS_TABLE].Rows.Count > 0 &&
                    moDSAssociation.Tables[I_EARNINGSDEDUCTIONS_TABLE].Rows[0][2] != DBNull.Value)
                {
                    iEarningDeductionId = Convert.ToInt32(moDSAssociation.Tables[I_EARNINGSDEDUCTIONS_TABLE].Rows[0][2]);
                    chkCheckRow.Attributes.Add("onclick", "CheckUncheckAllInRow(this," + iRowNo + ",'" + grdAssociation.AllowPaging + "'," + iStaffGroupId + "," + iEarningDeductionId + ")");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save staff groups-earnings deductions association and add entry into configuration table if not exists.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            if (QueryString["Is_Configured"] != Constants.S_YES)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StaffGroupsAndEarningDeductionsAssociation));

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related)));
        }

        catch (SqlException ex)
        {
            lblErr.Text = ex.Message;
            FillAssociationGrid();
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = ex.Message;
            FillAssociationGrid();
        }
      
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hadnle checkedChanged event of checkbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void oCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (hidIsSaveClick.Value == Constants.S_NO)
            {
                CheckBox oCheckBox = (CheckBox)sender;
                Control ctrl = oCheckBox.Parent;
                TableCell cell = (TableCell)ctrl;
                List<int> iBasicIds = new List<int>();
                string sIds = string.Empty;
                int iEarningDeductionId = Convert.ToInt32(oCheckBox.ID.Substring(oCheckBox.ID.LastIndexOf("_") + 1));
                DataSet oDataSet = null;
                DataTable oDTFormula = null;
                DataTable oDTBasicId = null;

                if (ViewState[S_FORMULA_TABLE] == null)
                {
                    oDataSet = moStaffGroupsAndEarningsDeductionsAssociationBL.GetStaffGroupsAndEarningsDeductionsIds(miSchoolId, miAcademicYearId);
                    oDTFormula = oDataSet.Tables[S_FORMULA_TABLE];
                    oDTBasicId = oDataSet.Tables[S_BASICID_TABLE];
                    ViewState[S_FORMULA_TABLE] = oDTFormula;
                    ViewState[S_BASICID_TABLE] = oDTBasicId;
                }
                else
                {
                    oDTFormula = (DataTable)ViewState[S_FORMULA_TABLE];
                    oDTBasicId = (DataTable)ViewState[S_BASICID_TABLE];
                }

                DataTable oDataTable = null;
                if (oDTFormula != null)
                {
                    DataRow[] oDataRow = oDTFormula.Select("EarningsDeductionsId=" + iEarningDeductionId);
                    if (oDataRow.Length > 0)
                        oDataTable = oDataRow.CopyToDataTable();
                    sIds = oDTBasicId.Rows[0][0].ToString();
                }
                CheckControl(oDataTable, oDTFormula, iEarningDeductionId, oCheckBox, sIds);
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
    /// This method is used to save staff groups-earnings deductions assocaition.
    /// </summary>
    private void Save()
    {   
        moStaffGroupsAndEarningsDeductionsAssociationBL.StaffGroupsEarningDeductionAssociation = PopulateBL();
        moStaffGroupsAndEarningsDeductionsAssociationBL.Save();     
    }

    /// <summary>
    /// This method is used to show/hide columns of grid.
    /// </summary>
    /// <param name="abAction"></param>
    private void HideColumns(bool abAction)
    {
        grdAssociation.Columns[I_STAFF_GROUP_ID_COLUMN_INDEX].Visible = abAction;
        grdAssociation.Columns[I_ORIGINAL_STAFF_GROUP_ID_COLUMN_INDEX].Visible = abAction;
        grdAssociation.Columns[I_STAFF_GROUP_NAME_COLUMN_INDEX].Visible = abAction;
    }

    /// <summary>
    /// This method is used to add javascripts attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons(this)";
        btnSave.Attributes["onclick"] = "if(!SetValue()) return false;";
        btnSave.Attributes["onclick"] = "if(!ConfirmSave()) return false;";
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
        grdAssociation.Columns[0].HeaderText = String.Empty;
    }

    /// <summary>
    /// This method is used to generate grid columns.
    /// </summary>
    private void GenerateColumns()
    {
        const int I_EARNINGS_DEDUCTIONS_TABLE_INDEX = 1;
        const int I_ASSOCIATION_TABLE_INDEX = 2;
        const string S_EARNINGS_DEDUCTIONS_NAME_FIELD = "ShortName";
        const string S_STAFF_GROUPS_ID_FIELD = S_STAFFGROUPS_ID;

        DataSet oDSAssociation = (DataSet)grdAssociation.DataSource;
        DataTable oDTEarningsDeductions = oDSAssociation.Tables[I_EARNINGS_DEDUCTIONS_TABLE_INDEX];
        DataTable oDTAssociation = oDSAssociation.Tables[I_ASSOCIATION_TABLE_INDEX];

        int iEarningsDeductionsCount = oDTEarningsDeductions.Rows.Count;
        int iStaffGroupsCount = grdAssociation.Rows.Count;
        int iCurrentColumnIndex = 0;
        int headerCellNo = 0;

        for (int iStaffGroupsIndex = 0; iStaffGroupsIndex < iStaffGroupsCount; iStaffGroupsIndex++)
        {            
            int iStaffGroupId = Convert.ToInt32(grdAssociation.DataKeys[iStaffGroupsIndex][S_STAFF_GROUPS_ID_FIELD]);
            for (int iEarnDeductionIndex = 0; iEarnDeductionIndex < iEarningsDeductionsCount; iEarnDeductionIndex++)
            {
                TableCell oTableCell = new TableCell();
                CheckBox oCheckBox = new CheckBox();
                oTableCell.Width = 100;
                oTableCell.HorizontalAlign = HorizontalAlign.Center;
                oTableCell.Width = System.Web.UI.WebControls.Unit.Point(900);
                oTableCell.Attributes.Add("title", "Staff Groups. " + oDSAssociation.Tables[I_STAFFGROUPS_TABLE].Rows[iStaffGroupsIndex]["StaffGroupsName"].ToString() + " [" + oDSAssociation.Tables[I_EARNINGSDEDUCTIONS_TABLE].Rows[iEarnDeductionIndex][S_EARNINGS_DEDUCTIONS_NAME_FIELD].ToString() + "]");
                oTableCell.Text = oDTEarningsDeductions.Rows[iEarnDeductionIndex][S_EARNINGSDEDUCTIONS_ID].ToString();
                iCurrentColumnIndex = grdAssociation.Rows[iStaffGroupsIndex].Cells.Add(oTableCell);
                int iEarningsDeductionsId = Convert.ToInt32(grdAssociation.Rows[iStaffGroupsIndex].Cells[iCurrentColumnIndex].Text);

                CheckBox oChkHeader = new CheckBox();
                if (iStaffGroupsIndex == 0)
                {
                    DataControlFieldHeaderCell oTHeader = new DataControlFieldHeaderCell(null);
                    oTHeader.CssClass = "locked";
                    oTHeader.HorizontalAlign = HorizontalAlign.Center;
                    oTHeader.Wrap = false;
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3");
                    oTHeader.Style.Add(HtmlTextWriterStyle.PaddingRight, "3");
                    oTHeader.Text = oDTEarningsDeductions.Rows[iEarnDeductionIndex][S_EARNINGS_DEDUCTIONS_NAME_FIELD].ToString();

                    iCurrentColumnIndex = grdAssociation.HeaderRow.Cells.Add(oTHeader);
                    if (iCurrentColumnIndex > 0)
                    {   
                        oChkHeader.Text = oDTEarningsDeductions.Rows[iEarnDeductionIndex][S_EARNINGS_DEDUCTIONS_NAME_FIELD].ToString();
                        if (oChkHeader.Text == "Basic")
                        {
                            oChkHeader.Checked = true;
                            oChkHeader.Enabled = false;
                        }
                        grdAssociation.HeaderRow.Cells[iCurrentColumnIndex].Controls.Add(oChkHeader);
                        headerCellNo = iCurrentColumnIndex - 4;                        
                        oChkHeader.Attributes.Add("onclick", "CheckAll(this, " + headerCellNo + ", '" + grdAssociation.AllowPaging + "'," + iStaffGroupId + "," + iEarningsDeductionsId + ")");
                    }
                }

                string sIsBasic = oDTEarningsDeductions.Rows[iEarnDeductionIndex]["IsBasic"].ToString();
                if (sIsBasic == "True")
                {
                    oCheckBox.Enabled = false;
                    oCheckBox.Checked = true;
                }

                oCheckBox.ID = "chk_" + iStaffGroupId + "_" + iEarningsDeductionsId;

                if (hidColumnValues.Value == string.Empty)
                    hidColumnValues.Value = oCheckBox.ID;
                DataRow[] oDataRow = oDTAssociation.Select("EarningsDeductionsId = " + iEarningsDeductionsId.ToString() + " AND StaffGroupsId = " + iStaffGroupId.ToString());

                if (oDataRow.Length > 0)
                    oCheckBox.Checked = true;

                oCheckBox.AutoPostBack = true;
                oCheckBox.CausesValidation = false;
                oCheckBox.CheckedChanged += new EventHandler(oCheckBox_CheckedChanged);
                oCheckBox.Attributes.Add("onclick", "CheckUnCheckAll(this," + iCurrentColumnIndex + ",'" + oChkHeader + "')");
                grdAssociation.Rows[iStaffGroupsIndex].Cells[iCurrentColumnIndex].Controls.Add(oCheckBox);
            }
        }

        hidRowCount.Value = iCurrentColumnIndex.ToString();
    }

    /// <summary>
    /// This method is used to check all the child checkbox if selected checkbox is parent checkbox.
    /// </summary>
    /// <param name="aoNewDataTable"></param>
    /// <param name="aoOldTable"></param>
    /// <param name="aiEarningDeductionId"></param>
    /// <param name="oCheckbox"></param>
    /// <param name="asIds"></param>
    private void CheckControl(DataTable aoNewDataTable, DataTable aoOldTable, int aiEarningDeductionId, CheckBox oCheckbox, string asIds)
    {
        string sCheckboxID = oCheckbox.ID;
        string sNewId = sCheckboxID.Substring(0, sCheckboxID.LastIndexOf("_"));
        List<int> oIdList = new List<int>();
        if (aoNewDataTable != null && aoNewDataTable.Rows.Count > 0 && aoNewDataTable.Rows[0][0] != DBNull.Value)
        {
            string sFirmula = string.Empty;
            sFirmula = GetFormula(aoOldTable, aiEarningDeductionId, sFirmula);
            string[] sFields = sFirmula.Split(',');
            int iCheckboxId;

            foreach (string sId in sFields)
            {
                if (sId.Contains("'"))
                {
                    iCheckboxId = Convert.ToInt32(sId.Replace("'", string.Empty));
                    oIdList.Add(Convert.ToInt32(sId.Replace("'", string.Empty)));
                    string str = sNewId + "_" + sId.Replace("'", string.Empty);

                    if (!asIds.Contains(iCheckboxId + ",") && !asIds.Contains(iCheckboxId.ToString()))
                    {
                        for (int iRowIndex = 0; iRowIndex < grdAssociation.Rows.Count; iRowIndex++)
                        {
                            GridViewRow oGridViewRow = grdAssociation.Rows[iRowIndex];
                            Control oControl = oGridViewRow.FindControl(str);

                            if (oControl != null)
                            {
                                CheckBox chkNew = ((CheckBox)oControl);
                                if (oCheckbox.Checked)
                                    chkNew.Checked = true;                                    
                                else
                                {
                                    chkNew.Checked = false;
                                    chkNew.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method is used to return formula.
    /// </summary>
    /// <param name="aoDataTable"></param>
    /// <param name="aiEarningDeductionId"></param>
    /// <param name="asFormula"></param>
    /// <returns></returns>
    private string GetFormula(DataTable aoDataTable, int aiEarningDeductionId, string asFormula)
    {
        int iEarnDeductId;
        DataRow[] oDataRow = aoDataTable.Select("EarningsDeductionsId=" + aiEarningDeductionId);
        if (oDataRow.Length > 0)
        {
            asFormula = oDataRow[0].ItemArray[0].ToString();
            string[] sFields = asFormula.Split(',');
            foreach (string sId in sFields)
            {
                if (sId.Contains("'"))
                {
                    iEarnDeductId = Convert.ToInt32(sId.Replace("'", string.Empty));
                    asFormula = asFormula + ",(," + GetFormula(aoDataTable, iEarnDeductId, asFormula) + ",)";
                }
            }
            return asFormula;
        }
        else
            return string.Empty;
    }

    /// <summary>
    /// This method is used to check pre-condition to configure association.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.StaffGroupsAndEarningDeductionsAssociation);

        if (!sLinks.Equals(String.Empty))
        {
            divErr.InnerHtml = sLinks;
            HideControls();
			trNote.Visible = false;
        }
        else
        {
            divErr.Visible = false;
            bReturn = true;
			trNote.Visible = true;
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to hide controls when either staff groups or earnings and deductions are not configured.
    /// </summary>
    private void HideControls()
    {
        chkAll.Visible = false;
        btnSave.Visible = false;
        grdAssociation.Visible = false;
        btnCancel.Text = "Back";
        tdGrid.Visible = false;
    }

    /// <summary>
    /// This method is used to fill association grid. 
    /// </summary>
    private void FillAssociationGrid()
    {
        const int I_FORMULA_TBL = 3;
        const int I_BASICID_TBL = 4;

        HideColumns(true);

        moDSAssociation = moStaffGroupsAndEarningsDeductionsAssociationBL.GetAssociation(miSchoolId, miAcademicYearId);
        grdAssociation.DataSource = moDSAssociation;
        grdAssociation.DataBind();
        ViewState[S_FORMULA_TABLE] = moDSAssociation.Tables[I_FORMULA_TBL];
        ViewState[S_BASICID_TABLE] = moDSAssociation.Tables[I_BASICID_TBL];
        HideColumns(false);
        GenerateColumns();
    }

    /// <summary>
    /// This method is used to populate StaffGroupsAndEarningsDeductionsAssociationBL object.
    /// </summary>
    /// <param name="aiStaffGroupId"></param>
    /// <param name="aiEarningsDeductionsId"></param>
    /// <param name="asEarningsDeductionsName"></param>
    /// <returns></returns>
    private StaffGroupsEarningDeductionAssociation PopulateBL()
    {
        return new StaffGroupsEarningDeductionAssociation
        {
            SchoolId = miSchoolId,
            AcademicYearId = miAcademicYearId,
            InsertedById = miUserId,
            UpdatedById = miUserId,
            AssociationXML = GenrateXML()
        };
    }

    /// <summary>
    /// This method is used to generate xml to save assiciation.
    /// </summary>
    /// <returns></returns>
    private string GenrateXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;

        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("Association");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "Association", "");

        DataSet oDsSource = (DataSet)grdAssociation.DataSource;

        object[] objPrimaryKey = new object[2];

        // Loop through all the grid rows.
        int iStaffGroupCount = grdAssociation.Rows.Count;
        int iEarningDeductionCount = Convert.ToInt32(hidRowCount.Value);
        for (int iRowCount = 0; iRowCount <= iStaffGroupCount - 1; iRowCount++)
        {
            int iStaffGroupId = Convert.ToInt32(grdAssociation.DataKeys[iRowCount][S_STAFFGROUPS_ID]);
            for (int iColumnCount = I_START_COUNT; iColumnCount <= iEarningDeductionCount; iColumnCount++)
            {
                int iEarningDeductionsId = Convert.ToInt32(grdAssociation.Rows[iRowCount].Cells[iColumnCount].Text);
                objPrimaryKey[0] = iStaffGroupCount;
                objPrimaryKey[1] = iEarningDeductionCount;                
                DataRow[] oDr = oDsSource.Tables[2].Select("StaffGroupsId=" + iStaffGroupId + " AND EarningsDeductionsId=" + iEarningDeductionsId);
                
                CheckBox oChkAssociation = (CheckBox)(grdAssociation.Rows[iRowCount].Cells[iColumnCount].Controls[0]);

                DataRow oDrCurrentClass = null;
                if (oDr.Length > 0)
                    oDrCurrentClass = oDr[0];
                if ((oChkAssociation.Checked == true && oDrCurrentClass != null) ||
                    (oChkAssociation.Checked == false && oDrCurrentClass == null))
                    continue;

                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "Association", "");

                sAttribute = "StaffGroupsId";
                XmlAttribute attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = iStaffGroupId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "EarningDeductionsId";
                attr = oDoc.CreateAttribute(sAttribute);
                attr.Value = iEarningDeductionsId.ToString();
                oXmlNode.Attributes.Append(attr);

                sAttribute = "Is_Deleted";
                attr = oDoc.CreateAttribute(sAttribute);
                if (oChkAssociation.Checked == true )
                    attr.Value = "N";
                else if (oChkAssociation.Checked == false )
                    attr.Value = "Y";
                oXmlNode.Attributes.Append(attr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        // Add the root node to document element.         
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;

    }

    /// <summary>
    /// This method is used to set screen width.
    /// </summary>
    private void SetScreenWidth()
    {
        if (Session[Constants.S_SESSION_SCREEN_WIDTH] != null)
        {
            string str = Session[Constants.S_SESSION_SCREEN_WIDTH].ToString().Replace("px !important", string.Empty);
            int iWidth = Convert.ToInt32(str);
            iWidth = iWidth / 100 * 80;
            GridViewScrollContainer.Style.Add("width", iWidth.ToString() + "px !important");
        }
        else
            GridViewScrollContainer.Style.Add("width", Convert.ToString(1024) + "px !important");
    }

    #endregion
}