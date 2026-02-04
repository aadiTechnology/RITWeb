// Class Name       :- HolidayLeavesDeductionUI
// Purpose          :- This class is used to configuration Staff holiday for salary deduction details.
// Date Of creation :- 12/09/2010
// Author Name      :- Shobha Patil

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using SchoolEntities;
using Utility;

public partial class StaffHolidaysSalaryDeductionUI : SchoolBase
{
    #region Constant(s)

    private const string S_DATEWISE_LEAVES = "Datewise Leaves";
    private const string S_CONFIGURED_STAFF_LEAVES = "Staff Leaves";
    private const string S_USER_DETAILS = "User Details";
    private const string S_SAVE_MESSAGE = "Staff holiday leave configuration has been saved successfully !!!";

    #endregion

    #region "DATA MEMBERS"

    private StaffHolidaysSalaryDeductionBL moStaffHolidaysSalaryDeductionBL;
    
    #endregion

    #region "EVENTS"

    /// <summary>
    /// This event is used to initialise the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            moStaffHolidaysSalaryDeductionBL = new StaffHolidaysSalaryDeductionBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {   
                DisplayHoliday();
                SetJavascriptAttributes();
                FillHolidayConfigListView();
                FillTypeCombobox();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill type combobox.
    /// </summary>
    private void FillTypeCombobox()
    {
        ListSource.FillDropDownList(moStaffHolidaysSalaryDeductionBL.StaffHolidayLeavesConfigTypes, cmbLeaveType, "Type", "Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This event is used to set the ListView controls set the serial no for each row of ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstHolidays_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewItem oCurrentItem = (ListViewItem)e.Item;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);

                CheckBox oChkSelect = ((CheckBox)oCurrentItem.FindControl("chkSelect"));
                TextBox oTxtHolidayName = (TextBox)oCurrentItem.FindControl("txtHolidayName");
                TextBox oTxtStartDate = (TextBox)oCurrentItem.FindControl("txtStartDate");
                TextBox oTxtEndDate = (TextBox)oCurrentItem.FindControl("txtEndDate");
                TextBox oTxtPercentage = (TextBox)oCurrentItem.FindControl("txtPercentage");
                Label lblSrNo = (Label)oCurrentItem.FindControl("lblSrNo");
                lblSrNo.Text = (oCurrentItem.DisplayIndex + 1).ToString();

                StaffHolidaysSalaryDeduction oStaffHolidaysSalaryDeduction = oCurrentItem.DataItem as StaffHolidaysSalaryDeduction;

                DropDownList cmbType = e.Item.FindControl("cmbType") as DropDownList;
                ListSource.FillDropDownList(moStaffHolidaysSalaryDeductionBL.StaffHolidayLeavesConfigTypes, cmbType, "Type", "Id", Constants.S_SELECT);
                ListItem oListItem = cmbType.Items.FindByValue(oStaffHolidaysSalaryDeduction.Type.ToString());

                oTxtHolidayName.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "')){return false;}");
                oTxtStartDate.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "')){return false;}");
                oTxtEndDate.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "')){return false;}");
                cmbType.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "')){return false;}");
                oTxtPercentage.Attributes.Add("onchange", "if(!IsTextChange('" + iRowId + "')){return false;}");

                if (oListItem != null)
                    oListItem.Selected = true;

                if (Convert.ToInt32(lstHolidays.DataKeys[iRowId]["StaffHolidaysSalaryDeductionId"]) != 0)
                    oChkSelect.Checked = true;
                else
                    oChkSelect.Checked = false;

                oChkSelect.Attributes.Add("onclick", "SelectAllControls(this," + iRowId + ")");
            }
        }
        catch (Exception ex)
        {
             ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the staff holiday configuration details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            FillHolidayConfigListView();
            lblMessage.Text = S_SAVE_MESSAGE;
            hidStaffHolidaysLeaveDeductionId.Value = string.Empty;
        }
        catch (SqlException ex)
        {
            trErrorMessage.Visible = true;
            if (ex.Message.StartsWith("RI_CHECK:"))
            {
                lblErrorMessage.Text = ex.Message.Substring("RI_CHECK:".Length);
                FillHolidayConfigListView();
            }
            else
                lblErrorMessage.Text = ex.Message;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region METHODS

    /// <summary>
    /// This method is used to set the javascript attributes.
    /// </summary>
    protected void SetJavascriptAttributes()
    {
        valSumHolidayConfig.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;        
        btnSave.Attributes.Add("Onclick", "if(!(ConfirmToSave())){return false;}");
        ApplyMouseHoverEffect( new List<Button> { btnSave, btnCancel ,btnClosePopUp});
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Payroll_Related));
        hidAcademicYearStartDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToDateTime().ToString("dd-MMM-yyyy");
        hidAcademicYearEndDate.Value = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToDateTime().ToString("dd-MMM-yyyy");
    }

    /// <summary>
    /// This function is used to fill the holiday configuration details ListView. 
    /// </summary>
    private void FillHolidayConfigListView()
    {   
        moStaffHolidaysSalaryDeductionBL.GetAll();

        var lstStaffHolidaysSalaryDeduction = moStaffHolidaysSalaryDeductionBL.StaffHolidaysSalaryDeductions.Where(shld => !shld.IsWeekend);

        lstHolidays.DataSource = lstStaffHolidaysSalaryDeduction;
        lstHolidays.DataBind();

        var oWeekendSettings = moStaffHolidaysSalaryDeductionBL.StaffHolidaysSalaryDeductions.Where(shld => shld.IsWeekend).FirstOrDefault();
        if (oWeekendSettings != null)
        {
            cmbLeaveType.SelectedValue = oWeekendSettings.Type.ToString();
            txtPercentage.Text = oWeekendSettings.PercentageToDeduct.ToString();
            chkWeekend.Checked = true;
        }

        ViewState[S_DATEWISE_LEAVES] = moStaffHolidaysSalaryDeductionBL.DatewiseStaffLeaves;
        ViewState[S_CONFIGURED_STAFF_LEAVES] = moStaffHolidaysSalaryDeductionBL.ConfiguredLeaves;
        ViewState[S_USER_DETAILS] = moStaffHolidaysSalaryDeductionBL.StaffBaseDetails;        
    }

    private void DisplayHoliday()
    {
        HolidaysMasterBL oHolidaysMasterBL = new HolidaysMasterBL();
        List<Holiday> lstHoliday = oHolidaysMasterBL.GetHolidayDetails(miSchoolId, miAcademicYearId);
        lstvwHoliday.DataSource = lstHoliday;
        lstvwHoliday.DataBind();
    }
   
    /// <summary>
    /// This function is used to save holiday configuration details.
    /// </summary>
    protected void Save()
    { 
        string sHolidayConfigXML = GenerateHolidayConfigXML();
        moStaffHolidaysSalaryDeductionBL.Save(sHolidayConfigXML);

        StaffHolidaysSalaryDeduction oStaffHolidaysSalaryDeduction = new StaffHolidaysSalaryDeduction
                                                                         {
                                                                             IsWeekend = chkWeekend.Checked,
                                                                             Type = cmbLeaveType.SelectedValue.ToInt(),
                                                                             PercentageToDeduct = txtPercentage.Text.ToDecimal()
                                                                         };

        moStaffHolidaysSalaryDeductionBL.SaveWeekendConfiguration(oStaffHolidaysSalaryDeduction);

        if (QueryString["Is_Configured"] != Constants.S_YES)
            SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.StaffHolidayAndLeaveConfiguration));
    }

    /// <summary>
    /// This method is used to collect parametres and send to stored procedure to perform save, update and delete details.
    /// </summary>
    /// <returns></returns>
    private string GenerateHolidayConfigXML()
    {
        const string S_ELEMENT = "element";
        string sAttribute;
        string sIs_Deleted = string.Empty;

        if (hidStaffHolidaysLeaveDeductionId.Value.Trim().StartsWith(","))
            hidStaffHolidaysLeaveDeductionId.Value = hidStaffHolidaysLeaveDeductionId.Value.Substring(1);

        string[] sArrStaffLeaveDeductionIds = hidStaffHolidaysLeaveDeductionId.Value.Split(',');

        XmlDocument oDoc = new XmlDocument();
        XmlElement oElement = oDoc.CreateElement("StaffHolidayLeavesConfiguraton");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StaffHolidayLeavesConfiguraton", string.Empty);
        for (int iRowCount = 0; iRowCount < lstHolidays.Items.Count; iRowCount++)
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)lstHolidays.Items[iRowCount];
            int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);                     
            CheckBox chkHoliday = oCurrentItem.FindControl("chkSelect") as CheckBox;
            if (chkHoliday.Checked)
            {
                if (sArrStaffLeaveDeductionIds.ToList().FindAll(sl => sl == iRowId.ToString()).Count == 0)
                    continue;
            }

            TextBox oTxtHolidayName = (TextBox)oCurrentItem.FindControl("txtHolidayName");
            TextBox oTxtStartDate = (TextBox)oCurrentItem.FindControl("txtStartDate");
            TextBox oTxtEndDate = (TextBox)oCurrentItem.FindControl("txtEndDate");
            TextBox oTxtPercentage = (TextBox)oCurrentItem.FindControl("txtPercentage");
            DropDownList cmbType = (DropDownList)oCurrentItem.FindControl("cmbType");


            XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StaffHolidayLeavesConfiguraton", string.Empty);

            sIs_Deleted = chkHoliday.Checked ? Constants.S_NO : Constants.S_YES;
            
                sAttribute = "StaffHolidayLeavesConfiguratonId";
                XmlAttribute  oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = (lstHolidays.DataKeys[iRowId]["StaffHolidaysSalaryDeductionId"]).ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "HolidayName";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (oTxtHolidayName.Text.Trim() != "")
                    oAttr.Value = oTxtHolidayName.Text.Trim();
                else
                    oAttr.Value = DBNull.Value.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "HolidayStartDate";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (oTxtStartDate.Text.Trim() != "")
                    oAttr.Value = oTxtStartDate.Text.Trim();
                else
                    oAttr.Value = DBNull.Value.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "HolidayEndDate";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (oTxtEndDate.Text.Trim() != "")
                    oAttr.Value = oTxtEndDate.Text.Trim();
                else
                    oAttr.Value = DBNull.Value.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "PercentageToDeduct";
                oAttr = oDoc.CreateAttribute(sAttribute);
                if (oTxtPercentage.Text.Trim() != "")
                    oAttr.Value = oTxtPercentage.Text.Trim();
                else
                    oAttr.Value = DBNull.Value.ToString();
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Is_Deleted";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = sIs_Deleted;
                oXmlNode.Attributes.Append(oAttr);

                sAttribute = "Type";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = cmbType.SelectedValue;
                oXmlNode.Attributes.Append(oAttr);

            oXmlRootNode.AppendChild(oXmlNode);
        }
        oElement.AppendChild(oXmlRootNode);
        // return the string generated.
        return oElement.InnerXml;
    }

    #endregion
}
