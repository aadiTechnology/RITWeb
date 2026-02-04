using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;
using BusinessLogic;
using System.Web.UI;
using StudentEntities;
using Utility;
using BusinessLogic.Exceptions;
using System.Globalization;

public partial class StudentSanctionedLeaveDetailsUI : SchoolBase
{
    #region "Data Members"
    const int I_PAGE_SIZE = 20;
    UserDetails oUserDetails = new UserDetails();
    List<UserDetails> olstUserDetails = new List<UserDetails>();
    private StudentSanctionedLeavesBL oStudentSanctionedLeavesBL;
    #endregion "Data Members"

    #region "Constants"

    private const string S_DELETE_MESSAGE = "Sanctioned Leave deleted successfully!!!";

    #endregion

    #region "Events"
    /// <summary>
    /// This event is used to fill standard dropdown list, set default control properties
    /// and to set javascript attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            oStudentSanctionedLeavesBL = new StudentSanctionedLeavesBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                FillStandardCombo();
                SetDefaultControls();                
				ApplyMouseHoverEffect(new List<Button>() { btnBack, btnSave, btnSearch });
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                if(lstvwStudentSanctionedLeave.Items.Count > 0)
                  ControlUtility.FillListViewPagerFooterWithCulture(lstvwStudentSanctionedLeave, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
            }
            DtPgCount.Visible = true;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back to AllStudentsUI.aspx page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/Admin/AllStudentsUI.aspx");
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save sanctioned leave details of students.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentSanctionedLeave.Items.Count > 0)
            {
                SaveStudentSanctionedLeaveDetials();
                lblUpdateSucess.Text = Resources.LocalizedResources.LeaveIsSanctionedSuccessfully;
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to show canceled sanctioned leaves records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkShowCanceledRecords_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            hidShowCanceledRecords.Value = Convert.ToString(chkShowCanceledRecords.Checked);
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            FillListView();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to get standardwise sanctioned leave records 
    /// and to fill division dropdown list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlStandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
            int iStandardId = Convert.ToInt32(ddlStandard.SelectedValue);
            ddlDivision.Visible = true;

            if (ddlStandard.SelectedIndex != 0)
            {
                FillDivisionCombobox(iStandardId);
                hidStandardId.Value = ddlStandard.SelectedValue;
                hidDivisionId.Value = ddlDivision.SelectedValue;
                FillListView();                
            }
            else
            {
                hidStandardId.Value = "0";
                hidDivisionId.Value = "0";
                ddlDivision.Items.Clear();
                ListItem olstDivision = new ListItem();
                olstDivision.Text = "-- All --";
                ddlDivision.Items.Add(olstDivision);
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to get sanctioned leave records for a division.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DtPgCount.SetPageProperties(0, I_PAGE_SIZE, false);
            hidDivisionId.Value = ddlDivision.SelectedValue;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view page wise sanctioned leaves list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNo(lstvwStudentSanctionedLeave);

            if (chkShowCanceledRecords.Checked)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Delete StudentSanctionedleave.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentSanctionedLeave_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            int aiSanctionedLeaveDetailsId = Convert.ToInt32(lstvwStudentSanctionedLeave.DataKeys[e.Item.DisplayIndex]["SanctionedLeaveDetailsId"]);
            if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {
                DeleteSanctionLeaveDetails(aiSanctionedLeaveDetailsId);
                RefreshSanctionLeaveList();
                lblUpdateSucess.Text = lblUpdateSucess.Text = S_DELETE_MESSAGE;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Refresh sanction leave list.
    /// </summary>
    private void RefreshSanctionLeaveList()
    {
        lstvwStudentSanctionedLeave.DataSourceID = ObjDSStudentSanctionedLeaves.ID;
        lstvwStudentSanctionedLeave.DataBind();
    }
     
    /// <summary>
    /// This event calls the StudentSanctionedLeavesBL Delete method.
    /// </summary>
    /// <param name="aiSanctionedLeaveDetailsId"></param>
    private void DeleteSanctionLeaveDetails(int aiSanctionedLeaveDetailsId)
    {
        oStudentSanctionedLeavesBL.Delete(aiSanctionedLeaveDetailsId);
    }

    #endregion "Events"

    #region "Listview Events"

    /// <summary>
    /// This event is used to fill footer property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentSanctionedLeave_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentSanctionedLeave.Items.Count > 0)
            {
                lstvwStudentSanctionedLeave.Items.Clear();
                //ControlUtility.FillListViewPagerFooter(lstvwStudentSanctionedLeave, DtPgCount);
                ControlUtility.FillListViewPagerFooterWithCulture(lstvwStudentSanctionedLeave, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
                if (chkShowCanceledRecords.Checked)
                    btnSave.Enabled = false;
                else
                {
                    SetConfirmationMessage();
                    btnSave.Enabled = true;
                }

                HtmlTableRow otrDataPager = lstvwStudentSanctionedLeave.FindControl("trDataPager") as HtmlTableRow;
                if (otrDataPager.Visible == true)
                    trPagerStudentSanctionedLeaves.Visible = true;
                else
                    trPagerStudentSanctionedLeaves.Visible = false;
            }
            else
            {
                trPagerStudentSanctionedLeaves.Visible = false;
                btnSave.Enabled = false;
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set default controls of listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwStudentSanctionedLeave_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                TextBox otxtStartDate = e.Item.FindControl("txtStartDate") as TextBox;
                TextBox otxtEndDate = e.Item.FindControl("txtEndDate") as TextBox;
                TextBox otxtRemark = e.Item.FindControl("txtRemark") as TextBox;
                CheckBox ochkIsUsed = e.Item.FindControl("chkIsCanceled") as CheckBox;
              
                ImageButton imgDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                ochkIsUsed.Enabled = false;
                
                if (otxtStartDate.Text.Trim() == string.Empty || otxtStartDate.Text == Resources.LocalizedResources.Date1Jan0001 || otxtStartDate.Text == Resources.LocalizedResources.Date1Jan1900)
                    otxtStartDate.Text = string.Empty;
                else
                    otxtStartDate.Text = Convert.ToDateTime(otxtStartDate.Text).ToString("dd-MMM-yyyy", new CultureInfo("en"));

                if (otxtEndDate.Text.Trim() == string.Empty || otxtEndDate.Text == Resources.LocalizedResources.Date1Jan0001 || otxtEndDate.Text == Resources.LocalizedResources.Date1Jan1900)
                    otxtEndDate.Text = string.Empty;
                else
                    otxtEndDate.Text = Convert.ToDateTime(otxtEndDate.Text).ToString("dd-MMM-yyyy", new CultureInfo("en"));


                if (otxtEndDate.Text.Trim() == string.Empty || otxtEndDate.Text == Resources.LocalizedResources.Date1Jan0001 || otxtEndDate.Text == Resources.LocalizedResources.Date1Jan1900)
                    otxtEndDate.Text = string.Empty;
                else
                    otxtEndDate.Text = Convert.ToDateTime(otxtEndDate.Text).ToString("dd-MMM-yyyy", new CultureInfo("en"));

                if (otxtStartDate.Text.Trim() == string.Empty || otxtStartDate.Text == Resources.LocalizedResources.Date1Jan0001 || otxtStartDate.Text == Resources.LocalizedResources.Date1Jan1900)
                    otxtStartDate.Text = string.Empty;

               
                imgDelete.Visible = true;
                if (otxtStartDate.Text.Trim() != string.Empty)
                {
                    ochkIsUsed.Enabled = true;
          
                    imgDelete.Enabled = true;                    
                }
                else {
                    imgDelete.Enabled = false;
                }
                StudentSanctionedLeaves oSanctionedLeavesInfo = oCurrentItem.DataItem as StudentSanctionedLeaves;
                if (oSanctionedLeavesInfo.IsCanceled)
                    (e.Item.FindControl("chkIsCanceled") as CheckBox).Checked = true;
                if (oSanctionedLeavesInfo.ShowOnAbsectStudentPopUp)
                    (e.Item.FindControl("chkShowOnAbsectStudentPopUp") as CheckBox).Checked = true;

                if (otxtStartDate.Text != string.Empty && otxtEndDate.Text != string.Empty)
                {
                    DateTime dtStatDate = Convert.ToDateTime(otxtStartDate.Text);
                    DateTime dtEndDate = Convert.ToDateTime(otxtEndDate.Text);
                    double dtotalLeaveDays = dtEndDate.Subtract(dtStatDate).TotalDays;
                    int iMaxLeaveDays = Settings.MaxLeaveDays;                    
                    if (dtotalLeaveDays >= iMaxLeaveDays)
                    {
                        var tableRow2 = oCurrentItem.FindControl("Tr2") as System.Web.UI.HtmlControls.HtmlTableRow;
                        var tableRow3 = oCurrentItem.FindControl("Tr3") as System.Web.UI.HtmlControls.HtmlTableRow;
                        if (tableRow2 != null)
                            tableRow2.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FFCCCC !important;");
                        if (tableRow3 != null)
                            tableRow3.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FFCCCC !important;");
                    }
                }

                bool bISUsedLeave = (e.Item.FindControl("chkIsCanceled") as CheckBox).Checked;
             
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmSanctionLeaveDelete('" + bISUsedLeave + "')) {return false;}");
              
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Listview Events"

    #region "Private Methods"
    /// <summary>
    /// This method is used to fill standard's combo.
    /// </summary>
    private void FillStandardCombo()
    {

        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oStandardCollectionBL.GetAssociatedStandards();
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlStandard,
                                       Constants.S_STANDARD_ID_FIELD,
                                       Constants.S_STANDARD_NAME_FIELD,
                                       "-- All --");

        //Add item into division combobox.
        ddlDivision.Items.Add(new ListItem("-- All --", Constants.I_ZERO.ToString()));
    }    

    /// <summary>
    /// This method is used to fill division's combo.    
    /// </summary>
    /// <param name="aiStandardId"></param>
    private void FillDivisionCombobox(int aiStandardId)
    {
        DivisionCollectionBL oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDSStandardCollection = oDivisionCollectionBL.GetAllDivisionsForStandard(aiStandardId);
        ControlUtility.FillDropDownList(oDSStandardCollection, ref ddlDivision,
                                       Constants.S_DIVISION_ID_FIELD,
                                       Constants.S_DIVISION_NAME_FIELD,
                                       "-- All --");
    }

    /// <summary>
    /// This method is used to set default controls.
    /// </summary>
    private void SetDefaultControls()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError; 
        txtSearch.Text = string.Empty;
        lblUpdateSucess.Text = string.Empty;
        if (chkShowCanceledRecords.Checked)
            btnSave.Enabled = false;
        else
            btnSave.Enabled = true;
        ddlStandard.Focus();
    }

    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        DataPager oDataPager = lstvwStudentSanctionedLeave.FindControl("DtPgDropDown") as DataPager;
        DropDownList ddlCnt = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
        ddlCnt.Attributes.Add("onchange", "if(!MessageAboutDate('" + ddlCnt.ClientID + "')){return false;}");
    }

    /// <summary>
    /// This method is used to fill listview.
    /// </summary>
    private void FillListView()
    {
        if (chkShowCanceledRecords.Checked)
            btnSave.Enabled = false;
        else
            btnSave.Enabled = true;
        lstvwStudentSanctionedLeave.DataSourceID = ObjDSStudentSanctionedLeaves.ID;
        lstvwStudentSanctionedLeave.DataBind();        
    }   

    /// <summary>
    /// This method is used to save sanctioned leave details of students.
    /// </summary>
    private void SaveStudentSanctionedLeaveDetials()
    {
        StudentSanctionedLeavesBL oStudentSanctionedLeavesBL = new StudentSanctionedLeavesBL(miSchoolId,miAcademicYearId);
        oStudentSanctionedLeavesBL.SanctionedLeavesInfo = PopulateSanctionedLeavesInfo();
        string sXML = GenerateXml(oStudentSanctionedLeavesBL.SanctionedLeavesInfo);
        oStudentSanctionedLeavesBL.SaveOrUpadteStudentSanctionedLeaveDetailsBL(sXML,miUserId);        
        foreach (UserDetails oUsers in olstUserDetails)
        {
            if (oUsers.IsCanceled == false) 
            {
                SetDeactivationSmsDetails();
                string sDeactivationReason = hidSmsTemplate.Value.Replace("%REASON%", " " + oUsers.EndDate.ToShortDateString() + Resources.LocalizedResources.DueToYourLongLeaveIsStartingFromTheDate + oUsers.StartDate.ToShortDateString() + Resources.LocalizedResources.ToDate + oUsers.EndDate.ToShortDateString());
                SendSMS(sDeactivationReason, oUsers.UserId, oUsers.MobileNumbers, oUsers.UserName);                
            }
            else
            {
                SetActivationSMSDetails();
                string sActivationReason = hidSmsTemplate.Value;
                bool bUnlockStudent = UnLockUser(oUsers.UserId);
                if (bUnlockStudent)
                    SendSMS(sActivationReason, oUsers.UserId, oUsers.MobileNumbers, oUsers.UserName);
            }
           
        }
        FillListView();
    }
  
	/// <summary>
    /// This method is used to populate SanctionedLeavesInfo class.
    /// </summary>
    /// <returns></returns>
    private SanctionedLeavesInfo PopulateSanctionedLeavesInfo()
    {
        SanctionedLeavesInfo oSanctionedLeavesInfo = new SanctionedLeavesInfo();
        oSanctionedLeavesInfo.lstStudentSanctionedLeaves = FillStudentSanctionedLeavesList();
        return oSanctionedLeavesInfo;
    }

    /// <summary>
    /// This method is used to fill list of StudentSanctionedLeaves class.
    /// </summary>
    /// <returns></returns>
    private List<StudentSanctionedLeaves> FillStudentSanctionedLeavesList()
    {
        StudentSanctionedLeaves oStudentSanctionedLeaves = null;
        List<StudentSanctionedLeaves> olstStudentSanctionedLeaves = new List<StudentSanctionedLeaves>();
        string sUserId = string.Empty;
        string sMobileNumber = string.Empty;
        bool bDatechangedOrNot = false;
        for (int iRowId = 0; iRowId < lstvwStudentSanctionedLeave.Items.Count; iRowId++)
        {
            oStudentSanctionedLeaves = new StudentSanctionedLeaves();
            oStudentSanctionedLeaves.SanctionedLeaveDetailsId = Convert.ToInt32(lstvwStudentSanctionedLeave.DataKeys[iRowId]["SanctionedLeaveDetailsId"]);
            oStudentSanctionedLeaves.StudentId = Convert.ToInt32(lstvwStudentSanctionedLeave.DataKeys[iRowId]["StudentId"]);
            oStudentSanctionedLeaves.UserId = Convert.ToInt32(lstvwStudentSanctionedLeave.DataKeys[iRowId]["UserId"]);
            oStudentSanctionedLeaves.MobileNumber = ((Label)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("lblMobileNo")).Text;
            oStudentSanctionedLeaves.StudentName = ((Label)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("lblName")).Text;
            oStudentSanctionedLeaves.Remark = ((TextBox)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("txtRemark")).Text.Trim();

            if (((TextBox)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("txtStartDate")).Text != "")
                oStudentSanctionedLeaves.StartDate = Convert.ToDateTime(((TextBox)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("txtStartDate")).Text);
            else
                oStudentSanctionedLeaves.StartDate = Convert.ToDateTime(Constants.S_DEFAULT_DATE_2);
            if (((TextBox)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("txtEndDate")).Text != string.Empty)
                oStudentSanctionedLeaves.EndDate = Convert.ToDateTime(((TextBox)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("txtEndDate")).Text);
            else
                oStudentSanctionedLeaves.EndDate = Convert.ToDateTime(Constants.S_DEFAULT_DATE_2);
            oStudentSanctionedLeaves.IsCanceled = ((CheckBox)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("chkIsCanceled")).Checked;
            oStudentSanctionedLeaves.ShowOnAbsectStudentPopUp = ((CheckBox)lstvwStudentSanctionedLeave.Items[iRowId].FindControl("chkShowOnAbsectStudentPopUp")).Checked;
            if (oStudentSanctionedLeaves.SanctionedLeaveDetailsId != 0)
            {
                bDatechangedOrNot = GetDatechangedOrNot(oStudentSanctionedLeaves.SanctionedLeaveDetailsId, oStudentSanctionedLeaves.StartDate, oStudentSanctionedLeaves.EndDate);
            }
            else
                bDatechangedOrNot = true;

            sUserId = oStudentSanctionedLeaves.UserId.ToString();
            sMobileNumber = oStudentSanctionedLeaves.MobileNumber;

            // get Students Id's whose long leave going to sanctioned and Deactivate those Students
            if (oStudentSanctionedLeaves.StartDate != (Convert.ToDateTime(Constants.S_DEFAULT_DATE_2)) && oStudentSanctionedLeaves.EndDate != (Convert.ToDateTime(Constants.S_DEFAULT_DATE_2)))
            {
                if (bDatechangedOrNot == true)
                {
                    //DeActivate users login. his Long Leave in use.
                    if (!string.IsNullOrEmpty(sUserId) && !string.IsNullOrEmpty(sMobileNumber) && (oStudentSanctionedLeaves.IsCanceled == false))
                    {
                        oUserDetails.UserId = oStudentSanctionedLeaves.UserId;
                        oUserDetails.UserName = oStudentSanctionedLeaves.StudentName;
                        oUserDetails.MobileNumbers = oStudentSanctionedLeaves.MobileNumber;
                        oUserDetails.IsCanceled = oStudentSanctionedLeaves.IsCanceled;
                        oUserDetails.StartDate = oStudentSanctionedLeaves.StartDate;
                        oUserDetails.EndDate = oStudentSanctionedLeaves.EndDate;
                       olstUserDetails.Add(oUserDetails);
                    }
                }
                //Activate users login if his Long Leave is used
                if (oStudentSanctionedLeaves.IsCanceled == true)
                {
                    oUserDetails.UserId = oStudentSanctionedLeaves.UserId;
                    oUserDetails.UserName = oStudentSanctionedLeaves.StudentName;
                    oUserDetails.MobileNumbers = oStudentSanctionedLeaves.MobileNumber;
                    oUserDetails.IsCanceled = oStudentSanctionedLeaves.IsCanceled;
                   olstUserDetails.Add(oUserDetails);
                }
                olstStudentSanctionedLeaves.Add(oStudentSanctionedLeaves);
                  }
           
        }
        return olstStudentSanctionedLeaves;
    }

    #endregion "Private Methods"
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            trPagerStudentSanctionedLeaves.Visible = false;
            DtPgCount.SetPageProperties(0, Constants.I_GRID_PAGE_COUNT, false);
            FillListView();
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }       
    }
      /// <summary>
    /// Deactivate Users/Students who on Long Leave
    /// </summary>
    /// <param name="asUserId"></param>
    /// <param name="asDeactivatedReason"></param>
    /// <returns></returns>
    private bool LockUser(int aiUserId, string asDeactivatedReason)
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        oSchoolUserBL.LockParticularUser(aiUserId, miSchoolId, miUserId, asDeactivatedReason, Constants.I_ONE,Constants.I_ZERO,Constants.I_ZERO);
        return true;
    }

    /// <summary>
    /// Activate user login whose Long leave is used.
    /// </summary>
    /// <param name="asUserId"></param>
    /// <returns></returns>
    private bool UnLockUser(int aiUserId)
    {      
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        oSchoolUserBL.UnLockParticularUser(aiUserId, miSchoolId, miUserId, 1);
        return true;
    }

    /// <summary>
    /// Send sms to the students whose Login is Deactivated because of Long Leave
    /// </summary>
    /// <param name="sSmsText"></param>
    /// <param name="sUserId"></param>
    /// <param name="asMobileNumber"></param>
    /// <param name="asUserName"></param>
    private void SendSMS(string sSmsText, int aiUserId, string asMobileNumber, string asUserName)
    {
        Hashtable oHTUsersMobileNo = new Hashtable();
        string sTemplateRegistrationId = string.Empty; //
        string[] sArrMobileNumber;
        sArrMobileNumber = asMobileNumber.Split(',');
        oHTUsersMobileNo[aiUserId] = sArrMobileNumber[0].Trim();

        if (sArrMobileNumber.Length > Constants.I_ONE && !sArrMobileNumber[1].Trim().IsNullOrEmpty() && sArrMobileNumber[0].Trim() != sArrMobileNumber[1].Trim())
            oHTUsersMobileNo[aiUserId + "sm;"] = sArrMobileNumber[1].Trim();
        if (oHTUsersMobileNo["TemplateRegistrationId"] != DBNull.Value)   
            sTemplateRegistrationId = oHTUsersMobileNo["TemplateRegistrationId"].ToString();  
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        var oSMS = new SMS
        {
            Sender = oSchoolBL.SMSSenderName,
            SMSText = sSmsText,
            School_Name = oSchoolBL.SchoolName + "::" + HidSMSTemplateName.Value,
            DisplayText = asUserName,
            SchoolID = miSchoolId,
            AcademicYearID = miAcademicYearId,
            SenderID = miUserId,
            SenderRoleID = Constants.UserRoles.Admin.ToInt(),
            InsertedByID = miUserId,
            TemplateRegistrationId = sTemplateRegistrationId
        };

        oSMS.To = oHTUsersMobileNo;
        oSMS.Send();
        oHTUsersMobileNo.Clear();
    }


    /// <summary>
    /// This method set default activation sms in text box.
    /// </summary>
    private void SetActivationSMSDetails()
    {
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.UserActivationSMS);
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTTemplate.Rows.Count != 0)
        {
            if (oDTTemplate.Rows[0][2] != DBNull.Value)
            {
                hidSmsTemplate.Value = Convert.ToString(oDTTemplate.Rows[0][2]);
                HidSMSTemplateName.Value = Convert.ToString(oDTTemplate.Rows[0][1]);
            }
        }
    }

    /// <summary>
    ///  /// <summary>
    /// This method set default Deactivatio reason in text box.
    /// </summary>
    /// </summary>
    private void SetDeactivationSmsDetails()
    {
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.UserSanctionLeaveDeactivationSMS);
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTTemplate.Rows.Count != 0)
        {
            if (oDTTemplate.Rows[0][2] != DBNull.Value)
            {
                hidSmsTemplate.Value = oDTTemplate.Rows[0][2].ToString();
                HidSMSTemplateName.Value = Convert.ToString(oDTTemplate.Rows[0][1]);
            }
        }
    }

    /// <summary>
    /// This Method is used to find is Students Long Leave date is Updating or Not.
    /// </summary>
    /// <param name="SanctionedLeaveDetailsId"></param>
    /// <param name="dtStartDate"></param>
    /// <param name="dtEntDate"></param>
    /// <returns></returns>
    private bool GetDatechangedOrNot(int SanctionedLeaveDetailsId, DateTime dtStartDate, DateTime dtEntDate)
    {
        StudentSanctionedLeavesBL oStudentSanctionedLeavesBL = new StudentSanctionedLeavesBL();
        bool bFlag = oStudentSanctionedLeavesBL.GetDatechangedOrNot(miSchoolId, miAcademicYearId, SanctionedLeaveDetailsId, dtStartDate, dtEntDate);
        return bFlag;
    }

    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidEndDateShouldBeGreaterThanStartDateForRow.Value = Resources.LocalizedResources.EndDateShouldBeGreaterThanStartDateForRow;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidEndDateShouldNotBeBlankForRow.Value = Resources.LocalizedResources.EndDateShouldNotBeBlank;
        hidStartDateAndEndDateShouldBeWithinCurrentAcademicYearAtRow.Value = Resources.LocalizedResources.StartDateAndEndDateShouldBeWithinCurrentAcademicYearAtRow;
        hidIfYouChangeThePageThenEnteredDatesOnCurrentPageWillGetLost.Value = Resources.LocalizedResources.IfYouChangeThePageThenEnteredDatesOnCurrentPageWillGetLost;
        hidStartDateShouldNotBeBlankForRow.Value = Resources.LocalizedResources.StartDateShouldNotBeBlank;
        hidAreYouSureYouWantToDeleteThisRecords.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRecords;
        hidEndDateSHouldNotBeFuture.Value = Resources.LocalizedResources.EndDateShouldNotBeFuture;
       // if (lstvwStudentSanctionedLeave.Items.Count > 0)
           // ControlUtility.FillListViewPagerFooterWithCulture(lstvwStudentSanctionedLeave, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
    }
}