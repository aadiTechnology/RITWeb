/* File Name :- ControlPanel.aspx.cs
* Modified By :- Sachin
* Modified Date :- 2-Oct-2009
* Purpose :- Code Review.
* Class Description :- This class is used to display dashboard.
*/

/* -----------------------------------------------------------------------
 * MODIFICATION LOG
 * -----------------------------------------------------------------------
 * Author	: Vishal B. Shah
 * Date		: 12-March-2012
 * Purpose	: Added a drop down to select associated Financial Year.
 * -----------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using SchoolEntities.Admin;
using StudentEntities;
using Utility;
using System.Collections;
using System.Configuration;
using MasterEntities;
using PayrollReportingUserEntities;

public partial class ControlPanel : SchoolBase
{
    #region --ENUM --

    public enum BonafideReportID
    {
        PPSH = 125,
        SS = 117,
        PP = 83
    }

    #endregion

    #region -- CONSTANT(s) --
    const string S_SINGLE_BIRTHDAY = "Staff Birthday";
    const string S_MULTIPLE_BIRTHDAY = "Staff Birthdays";
    const string S_NEW_MESSAGE = "New Message";
    const string S_NEW_MESSAGES = "New Messages";
    const string S_UNREAD_MESSEGE = "Unread Message";
    const string S_UNREAD_MESSEGES = "Unread Messages";
    const string S_DEFAULT_PHOTO = "~/RITeSchool/images/Student_BlankPh.jpg";
    const string S_VIOLET_COLOR = "MediumVioletRed";
    const string S_DARK_GREEN_COLOR = "darkGreen";
    const string S_CLOSE_YEAR = "Is_Close_Year";
    const string S_NEWLYCREATED_YEAR = "Is_NewlyCreated";
    const string S_FINALYEAR_GENERATED = "Is_FinalYear_Generated";
    const string S_CURRENT_YEAR = "Is_Current_Year";
    const string S_CLOSED_WARNIG_MESSAGE = "You are viewing data of old academic year %d%. Please do not modify any data.";
    const string S_CLOSED_NEW_WARNIG_MESSAGE = "You are viewing data of old academic year %d% and new financial year %a%.";
    const string S_NEW_CLOSED_WARNIG_MESSAGE = "You are viewing data of new academic year %d% and old financial year %a%.";
    const string S_COMBINED_CLOSED_YEAR_MESSAGE = "You are viewing data of old academic year %d% and financial year %a%. Please do not modify any data.";
    const string S_COMBINED_NEW_YEAR_MESSAGE = "You are viewing data of new academic year %d% and financial year %a%.";
    const string S_ACCOUNTS_CLOSED_YEAR_MESSAGE = "You are viewing data of old financial year %a%. Please do not modify any data.";
    const string S_ACCOUNTS_NEW_YEAR_MESSAGE = "You are viewing data of new financial year %a%.";
    const string S_NEW_WARNIG_MESSAGE = "You are viewing new academic year";
    const string S_MESSAGE = "You are viewing current year.";
    const string S_MESSAGE_FEILD = "Message";
    const string S_BIRTHDAY_MESSAGE_FOR_STUDENT = "Wishing you another wonderful year of happiness, fun and success.<br>";
    const string S_BIRTHDAY_MESSAGE_FOR_OTHERS = "May this birthday be just the beginning of a year filled with <br>happy memories, wonderful moments and shining dreams.<br>";
    const string S_ACADEMIC_YEAR_ID = "Academic_Year_ID";
    const string S_YEAR_VALUE = "YearValue";
    const string S_NONPERMANANT_TEACHERS = "NonPermanent Teachers";
    protected string sMenuContent1 = string.Empty;
    protected string sMenuContent = string.Empty;
    const int S_REPORT_FOLDER_DETAILS = Constants.I_ONE;
    const int S_REPORT_DETAILS = Constants.I_TWO;
    const int S_REPORT_CONFIGURE_NAME = Constants.I_ZERO;
    const int S_HAS_ACCESS = Constants.I_TWO;
    string S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = "N";
    const int I_ZERO = 0;
    const int I_TWO = 2;
    const int I_ONE = 1;
    bool ISABSENT_STUDENT__LINK_VISIBEL;
    private DateTime DT_OPEN_DAY_NOTICE = new DateTime(2014, 11, 23);
    UserDetails oUserDetails = new UserDetails();
    List<UserDetails> olstUserDetails = new List<UserDetails>();
    DataTable moDtAcademicAndYearInfo;

    #endregion -- CONSTANT(s) --

    #region -- MEMBER(s) --


    private bool mbNewVoucherForApproval;
    private int miNewVouchersForApprovalCount;
    List<AbsentStudentDetails> mlstAbsentStudentDetails;

    // Fields for logging datetime conversion errors
    private string msDetails;
    private string msDate;
    public int miAdmissionCnt;
    private int miRequisitionCnt;
    public string msSupervisorDesignationName;

    #endregion -- MEMBER(s) --

    #region -- PROPERTIES --

    /// <summary>
    /// Determines if the Accounts module is enabled for the school.
    /// </summary>
    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

	private bool IsPayrollModuleEnabled
    {
        get { return Settings.EnablePayrollModule;}

    }
    #endregion -- PROPERTIES --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// This event is used for following purposes :-
    /// 1.To display control panel according to user role.
    /// 2.Set last login details.
    /// 3.Display birthday ppup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            InitializeMemberVariables();            
            S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR = Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO;
            miAdmissionCnt = StudentBL.GetNewAdmissionCount(miSchoolId, S_SHOW_ADMISSIONS_FOR_CURRENT_YEAR, miAcademicYearId);
            miRequisitionCnt = RequisitionBL.CountRowsOfRequisition(miSchoolId, 4, miUserId);
            trNonPermenantTeachers.Visible = false;            
            if (Settings.ExternalLibrarySite != string.Empty)
            {
                hlnkLibraryManagement.NavigateUrl = Settings.ExternalLibrarySite;
                hlnkLibraryManagement.Target = "_blank";
                hlnkIssueRenewReturn.NavigateUrl = Settings.ExternalLibrarySite;
                hlnkIssueRenewReturn.Target = "_blank";
                hlnkReturnRenew.NavigateUrl = Settings.ExternalLibrarySite;
                hlnkReturnRenew.Target = "_blank";

                //if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
                //{

                //    hlnkStudentLibrary.Visible = false;
                //    hlnkStudentLibraryPPSN.Visible = true;
                //}
                //else
                //{
                //    hlnkStudentLibrary.NavigateUrl = Settings.ExternalLibrarySite;
                //    hlnkStudentLibrary.Target = "_blank";
                //    hlnkStudentLibraryPPSN.Visible = false;
                //}

                if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                {
                    hlnkStudentLibrary.NavigateUrl = Settings.ExternalLibrarySite;
                    hlnkStudentLibrary.Target = "_blank";
                }
				
                hlnkTeacherLibrary.NavigateUrl = Settings.ExternalLibrarySite;
                hlnkTeacherLibrary.Target = "_blank";
                hlnkClassTeacherLibrary.NavigateUrl = Settings.ExternalLibrarySite;
                hlnkClassTeacherLibrary.Target = "_blank";
                
            }            
            if (!IsPostBack)
            {
               
                hidPhotoGalleryCount.Value = Settings.DisplayTopPhotoAlbumCount.ToString();
                if (miSchoolId == Constants.SchoolId.MVPS.ToInt())
                    hidIsMVPSSchool.Value = Constants.S_YES;
                else
                    hidIsMVPSSchool.Value = Constants.S_NO;

                AbsentStudentDetails();

                if (miSchoolId == Constants.SchoolId.PPSH.ToInt() && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO) && (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt() != Constants.UserRoles.Student.ToInt()))
                {
                    divMediclaimnotice.Visible = true;                    
                    HidMediclaimNoticeFirstTime.Value = Constants.S_YES;
                }
                else
                {
                    divMediclaimnotice.Visible = false;
                    HidMediclaimNoticeFirstTime.Value = Constants.S_NO;
                    
                }

                if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                    trExternalStudentsFeeDetails.Visible = true;
                else
                    trExternalStudentsFeeDetails.Visible = false;

                if (Settings.AllowStudentPhotoUploadFromStudentLogin)
                    trUploadStudentPhoto.Visible = true;
                else
                    trUploadStudentPhoto.Visible = false;

                if (moSchool == Constants.SchoolId.PPSH && moUserRole == Constants.UserRoles.Student)
                    trBonafideRequestApplication.Visible = true;
                else
                    trBonafideRequestApplication.Visible = false;

                if (moSchool == Constants.SchoolId.PIONEER && moUserRole == Constants.UserRoles.Student)
                    trStudentMonthlyDetails.Visible = true;
                else
                    trStudentMonthlyDetails.Visible = false;

				
                if (miSchoolId == Constants.SchoolId.PPSN.ToInt() && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO))
                {
                    //if (DateTime.Now > new DateTime(2022, 3, 11, 23, 59, 00))
                    //    divFeeNotice.Visible = true;

                    HidNoticeFirstTime.Value = Constants.S_YES;

                 //   divAppLetter2.Visible = true;///
                    divflashNotice.Visible = DateTime.Now < new DateTime(2020, 1, 20, 11, 00, 00);
                    //divPPSNResult.Visible = true;               
                }
                else
                {   
                    divFeeNotice.Visible = false;                    
                    HidNoticeFirstTime.Value = Constants.S_NO;
                  //  divAppLetter2.Visible = false;///
                    divflashNotice.Visible = false;
                    //divPPSNResult.Visible = false;
                }

                if (moSchool == Constants.SchoolId.PPSH && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO))
                {
                    if (DateTime.Now.Date == new DateTime(2022, 11, 14))
                        divPPSHChildrenDay.Visible = true;
                    else
                        divPPSHChildrenDay.Visible = false;
                }
                else
                    divPPSHChildrenDay.Visible = false;
                
                //if (miSchoolId == Constants.SchoolId.PPSH.ToInt() && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO))
                //{
                //    HidNoticeFirstTime.Value = Constants.S_YES;
                //    int iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
                //    string sStandardName = Convert.ToString(Session[Constants.S_SESSION_STUDENT_STANDERED_NAME]);              

                //    if (moUserRole == Constants.UserRoles.Student)
                //    {
                //        //Preprimary Block(Preprimary to 2nd std)
                //        if (iStandardId == 1005 || iStandardId == 1006 || iStandardId == 1007 || iStandardId == 1008 || iStandardId == 1009)
                //        {
                //            hidPPSHAnnualDayInvitation.Value = Constants.S_TWO;
                //            DivFlashInvitationPrePrimary.Visible = true;
                //            DivFlashInvitationPrimary.Visible = false;
                //        }
                //        //Primary Block(3rd to 9th std)
                //        else if (iStandardId == 1010 || iStandardId == 1011 || iStandardId == 1012 || iStandardId == 1013 || iStandardId == 1014 || iStandardId == 1015 || iStandardId == 1016)
                //        {
                //            hidPPSHAnnualDayInvitation.Value = "3";
                //            DivFlashInvitationPrimary.Visible = true;
                //            DivFlashInvitationPrePrimary.Visible = false;  
                //        }
                //    }
                //    else
                //    {
                //        hidPPSHAnnualDayInvitation.Value = Constants.S_ONE;
                //        DivFlashInvitationPrePrimary.Visible = true;
                //        DivFlashInvitationPrimary.Visible = true;
                //    }
                //}
                //else
                //{
                //    hidPPSHAnnualDayInvitation.Value = Constants.S_ZERO;
                //    HidNoticeFirstTime.Value = Constants.S_NO;
                //    DivFlashInvitationPrePrimary.Visible = false;
                //    DivFlashInvitationPrimary.Visible = false;
                //}

                if (miSchoolId == Constants.SchoolId.JPS.ToInt() && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO))
                    divInVitationVideo.Visible = true;
                else
                    divInVitationVideo.Visible = false;

                //if (miSchoolId == Constants.SchoolId.PPS.ToInt() && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO))
                //    divCeremonyVideo.Visible = true;
                //else
                //    divCeremonyVideo.Visible = false;

                //if (miSchoolId == Constants.SchoolId.PPSH.ToInt() && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO))
                //{
                //    divAnnualDayInvitationVideo.Visible = true;
                //    HidAnnualDayPPSH.Value = Constants.S_YES;
                //}
                //else
                //{
                //    divAnnualDayInvitationVideo.Visible = false;
                //    HidAnnualDayPPSH.Value = Constants.S_NO;
                //}
                
                //if (miSchoolId == Constants.SchoolId.PIONEER.ToInt() && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO) && moUserRole==Constants.UserRoles.Student)
                //{

                //    HidAdmissionNoticeFirstTime.Value = Constants.S_YES;
                //    divAdmission.Visible = true;
                //}
                //else
                //{

                //    HidAdmissionNoticeFirstTime.Value = Constants.S_NO;
                //    divAdmission.Visible = false;
                //}

                hidShowAllGalleries.Value = (Settings.ShowAllGalleries ? Constants.S_YES : Constants.S_NO);

                if (miSchoolId == Constants.SchoolId.PPS.ToInt() && (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO))
                {
                    divOpenDayNotice.Visible = DateTime.Now < DT_OPEN_DAY_NOTICE;
                    HidOpenDayNoticeFirstTime.Value = Constants.S_YES;
                }
                else
                {
                    HidOpenDayNoticeFirstTime.Value = Constants.S_NO;
                    divOpenDayNotice.Visible = false;
                }

                
                hidFeeVideolinkurl.Value = Settings.FeeVideoLinkURL;
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }

                if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO)
                    hidIsFirstTimeLogin.Value = Constants.S_YES;
                else
                    hidIsFirstTimeLogin.Value = Constants.S_NO;

                if (miSchoolId == Constants.SchoolId.LORDDS.ToInt())
                    divFeeDetails.Visible = false;

                HideStudentMenu();
                GetAcademicYears();
                FillYearComboBoxes();
                SetBonafideReportLink();
                DesignSettingAccordinglanguage();
                Constants.S_SCHOOLID = miSchoolId.ToString();
                string sFirstLogIn = Convert.ToString(Session[Constants.S_SESSION_IS_FIRST_LOGIN]);
                hidFirstLogIn.Value = sFirstLogIn;                
                DisplayControlPanelAccordingToRole();
                SetLastLoginDetails();
                DisplaySchoolNotice();
                DisplayBirthdayPopup();
                DisplayExpiredSanctionedLeavesPopup();
                ShowClasswiseStudentCount();
                PaymentClearanceNotification();
                SetJavascriptAttributes();                
                if (moUserRole == Constants.UserRoles.Admin && Settings.IsMiniSite && (Request.UrlReferrer.AbsolutePath.Contains("StudentChangePassword.aspx") || Request.UrlReferrer.AbsolutePath.Contains("ControlPanel.aspx")))
                    hidShowReadMe.Value = Constants.S_YES;
                hidSchoolId.Value = Constants.S_SCHOOLID;                
                //FillTestCombobox(cmbExam);
                FillStandardCombobox();
                ShowRetirementPopup();
                MissingAttendancelinkVissible();
                ReplaceXseedToPrePrimary();                
            }

          

            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                HyperLink134.Visible = true;
                lnkAchievement.Visible = true;
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordinglanguage();
            }

            DisplayNewAdmissionCount();
            DisplayRequisitionCount();
            cmbAcademicYearID.Focus();
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);

            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
                hlkAlumniDetails.Visible = true;            
            else
                hlkAlumniDetails.Visible = false;

            if (SchoolBase.Settings.EnableGuestManagement)
                trGuestManagement.Visible = true;
            else
                trGuestManagement.Visible = false;

            if (SchoolBase.Settings.EnableStudentHealthDetailsModule)
                trStudentHealthDetails.Visible = true;
            else
                trStudentHealthDetails.Visible = false;
             

            if (moUserRole == Constants.UserRoles.Student)
                tdAddStudentDetails.Visible = Settings.ShowAadharCardForStudent;

            if (moUserRole == Constants.UserRoles.Student && !(SchoolBase.Settings.IsAaryanSchool))
                tdUploadParentDetails.Visible = true;
            else
                tdUploadParentDetails.Visible = false;            

            if (miSchoolId == Constants.SchoolId.SPS.ToInt())
                trLcUpload.Visible = true;
            else
                trLcUpload.Visible = false;

            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                trUserDocument.Visible = true;
            else
                trUserDocument.Visible = false;
        }
        catch (Exception ex)
        {
            if (Request.UrlReferrer != null)
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
                                                          String.Format("{0}. msDetails - {1} msDate - {2}", Request.UrlReferrer.AbsoluteUri, msDetails, msDate));
            else
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
                                                          String.Format("msDetails - {0} msDate - {1}", msDetails, msDate));
        }
    }

    /// <summary>
    /// This event is used to change academic year.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbAcademicYearID_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
          

            Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR] = cmbAcademicYearID.SelectedValue;
            GetAcademicYears();
            int iSelectedAcademicYear = cmbAcademicYearID.SelectedValue.ToInt();
            DataRow[] oDataRow = moDtAcademicAndYearInfo.Select(S_ACADEMIC_YEAR_ID + " =" + iSelectedAcademicYear);
            Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = iSelectedAcademicYear;
            Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = oDataRow[0]["Start_date"];
            Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = oDataRow[0]["End_Date"];
            (this.Master.FindControl("hidAcademicYearId") as HiddenField).Value = cmbAcademicYearID.SelectedValue;
            miAcademicYearId = iSelectedAcademicYear;
            
            hidAcademicYearEndDate.Value = Convert.ToString(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToDateTime());

            CheckIfSelectedYearIsclosed(oDataRow);
            CheckIfNewVouchersForApproval();
            SetUserDetails();
            hidShowAdmissionPopup.Value = "N";
            ReplaceXseedToPrePrimary();
            //FillTestCombobox(cmbExam);
            FillStandardCombobox();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Updates session values based on the selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFinancialYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        AccountsBaseClient oAccountsBaseClient = null;
        try
        {
            oAccountsBaseClient = new AccountsBaseClient();
            oAccountsBaseClient.Open();

            if (moUserRole != Constants.UserRoles.Admin && Convert.ToChar(Session[Constants.S_SESSION_ISACADEMICYRAPPLICABLE]) == Constants.C_YES) 
                GetAcademicYears();

            FinancialYear oFinancialYear = oAccountsBaseClient.GetFinancialYear(miSchoolId, ddlFinancialYears.SelectedValue.ToInt());

            SetFinancialYearDetailsInSession(oFinancialYear);
            CheckIfFinancialYearIsClosed();
            CheckIfNewVouchersForApproval();
            SetUserDetails();
            ReplaceXseedToPrePrimary();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
                oAccountsBaseClient.Close();
        }
    }

    /// <summary>
    /// This event is used for implementing paging style.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                var pagerTable = e.Row.Cells[0].Controls[0] as Table;
                pagerTable.CssClass = "ClsNwGridPaging";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the ListView controls set the serial no for each row of ListView.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwRetirementDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewItem oCurrentItem = (ListViewItem)e.Item;
                Label lblRemainingDays = (Label)oCurrentItem.FindControl("lblDays");
                int iRemainingDays = lblRemainingDays.Text.ToInt();
                if (iRemainingDays < 0)
                {
                    lblRemainingDays.ForeColor = System.Drawing.Color.Red;
                    lblRemainingDays.Font.Bold = true;
                }
                Label lblSrNo = (Label)oCurrentItem.FindControl("lblSrNo");
                lblSrNo.Text = (oCurrentItem.DisplayIndex + 1).ToString();

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion  -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This event is used to open feedback / survey form page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private string GetSurveyFeedbackUrl()
    {

        string sQueryString = CommonUtility.EncryptQuerystring("SurveyId=2&UserId=" + miUserId);
        return "../Survey/SurveyFeedbackUI.aspx?" + sQueryString;
    }

    /// <summary>
    /// This method is used to check wherther current year is closed or not.
    /// </summary>
    private void CheckIfSelectedYearIsclosed(DataRow[] aoDataRow)
    {
        Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = string.Empty;
        string sClosedYearStatus = Convert.ToString(aoDataRow[0][S_CLOSE_YEAR]);
        String sNewYearStatus = Convert.ToString(aoDataRow[0][S_NEWLYCREATED_YEAR]);
        string sIsFinalYearGenerated = Convert.ToString(aoDataRow[0][S_FINALYEAR_GENERATED]);
        Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] = Constants.C_NO;
        var trYearStatus = this.Master.FindControl("trYearStatus") as HtmlTableRow;
        var lblYearStatus = this.Master.FindControl("lblYearStatus") as Label;
        Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_CLOSED] = Constants.C_NO;

        var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
        bool bIsFinancialYearClosed = oFinancialYear != null && oFinancialYear.IsClosed;
        bool bIsFinancialNewYear = oFinancialYear != null && !oFinancialYear.IsCurrent && oFinancialYear.StartDate > DateTime.Now;
        string sMessage = string.Empty;
        trYearStatus.Visible = false;
        if (sClosedYearStatus == Constants.S_YES)
        {
            string sMessasge = string.Empty;
            if (bIsFinancialYearClosed)
            {
                sMessasge = Resources.LocalizedResources.YouAreViewingDataOfOldAcademicYearAndFinancialYearPleaseDoNotModify;
                sMessasge = sMessasge.Replace("%d%", "(" + cmbAcademicYearID.SelectedItem.Text + ")");
                if (bIsFinancialYearClosed && ddlFinancialYears.Items.Count > 0)
                    sMessasge = sMessasge.Replace("%a%", "(" + ddlFinancialYears.SelectedItem.Text + ")");
            }
            else if (!bIsFinancialYearClosed && !bIsFinancialNewYear)
            {
                sMessasge = Resources.LocalizedResources.YouAreViewingDataOfOldAcademicYear;
                sMessasge = sMessasge.Replace("%d%", "(" + cmbAcademicYearID.SelectedItem.Text + ")");
                if (bIsFinancialYearClosed && ddlFinancialYears.Items.Count > 0)
                    sMessasge = sMessasge.Replace("%a%", "(" + ddlFinancialYears.SelectedItem.Text + ")");
            }
            else if (bIsFinancialNewYear)
            {
                sMessasge = Resources.LocalizedResources.YouAreViewingDataOfOldAcademicYearAndNewFinancialYear;
                sMessasge = sMessasge.Replace("%d%", String.Format("({0})", cmbAcademicYearID.SelectedItem.Text));
                sMessasge = sMessasge.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));

            }
            else if (bIsFinancialYearClosed)
            {
                sMessasge = Resources.LocalizedResources.YouAreViewingDataOfOldFinancialYearPleaseDoNotModifyAnyData.Replace("%a%", "(" + ddlFinancialYears.SelectedItem.Text + ")");
                Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = sMessasge;
                trYearStatus.Visible = true;
            }

            Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = sMessasge;
            trYearStatus.Visible = true;
            lblYearStatus.Text = sMessasge;
            Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_CLOSED] = Constants.C_YES;
        }
        else if (sNewYearStatus == Constants.S_YES)
        {
            if (bIsFinancialNewYear)
            {
                sMessage = Resources.LocalizedResources.YouAreviewingDataOfNewAcademicYearAndFinancialYear;
                sMessage = sMessage.Replace("%d%", String.Format("({0})", cmbAcademicYearID.SelectedItem.Text));
                sMessage = sMessage.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
            }
            else if (bIsFinancialYearClosed)
            {
                sMessage = S_NEW_CLOSED_WARNIG_MESSAGE;
                sMessage = sMessage.Replace("%d%", String.Format("({0})", cmbAcademicYearID.SelectedItem.Text));
                sMessage = sMessage.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
            }
            else if (!bIsFinancialNewYear)
            {
                sMessage = Resources.LocalizedResources.YouAreViewingNewAcademicYear + " (" + cmbAcademicYearID.SelectedItem.Text + ").";
                lblYearStatus.Text = Resources.LocalizedResources.YouAreViewingNewAcademicYear + " (" + cmbAcademicYearID.SelectedItem.Text + ").";
            }

            Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = sMessage;
            lblYearStatus.Text = sMessage;
            trYearStatus.Visible = true;
            Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] = Constants.C_YES;
        }
        else
        {
            if (bIsFinancialNewYear)
            {
                sMessage = Resources.LocalizedResources.YouAreViewingDataOfNewFinancialYear;
                sMessage = sMessage.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
                Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = sMessage;
                trYearStatus.Visible = true;
                lblYearStatus.Text = sMessage;
            }
            else if (bIsFinancialYearClosed)
            {
                sMessage = Resources.LocalizedResources.YouAreViewingDataOfOldFinancialYearPleaseDoNotModifyAnyData.Replace("%a%", " (" + cmbAcademicYearID.SelectedItem.Text + ")");
                Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = sMessage;
                Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = sMessage;
                trYearStatus.Visible = true;
                lblYearStatus.Text = sMessage;
            }
            else
                trYearStatus.Visible = false;

        }
        if (sIsFinalYearGenerated == "Y")
            Session[Constants.S_SESSION_IS_FINALYEAR_GENERATED] = Constants.C_YES;
        else
            Session[Constants.S_SESSION_IS_FINALYEAR_GENERATED] = Constants.C_NO;
    }

    /// <summary>
    /// This method is used to fill academic year combo on page load.
    /// </summary>
    private void FillAcademicYearCombo()
    {
            cmbAcademicYearID.Bind(moDtAcademicAndYearInfo, S_ACADEMIC_YEAR_ID, S_YEAR_VALUE, string.Empty);
            cmbAcademicYearID.SelectedValue = Convert.ToString(miAcademicYearId);

            cmbFeeAcademicYear.Bind(moDtAcademicAndYearInfo, S_ACADEMIC_YEAR_ID, S_YEAR_VALUE, string.Empty);
            cmbFeeAcademicYear.SelectedValue = Convert.ToString(miAcademicYearId);

            cmbAcademicYear.Bind(moDtAcademicAndYearInfo, S_ACADEMIC_YEAR_ID, S_YEAR_VALUE, string.Empty);
            cmbAcademicYear.SelectedValue = Convert.ToString(miAcademicYearId);

            if (cmbAcademicYearID.Items.Count <= 0)
                return;

            DataRow[] oDataRow = moDtAcademicAndYearInfo.Select(S_ACADEMIC_YEAR_ID + " =" + cmbAcademicYearID.SelectedValue);
            CheckIfSelectedYearIsclosed(oDataRow);

            FillFinancialYearCombo();

            
           
    }

    /// <summary>
    /// This method is used for to fill year dropdown used for photo gallery and payroll widgets
    /// </summary>
    private void FillYearComboBoxes() 
    {
        
        for (int iRecordCount = 0; iRecordCount < moDtAcademicAndYearInfo.Rows.Count; iRecordCount++)
        {
            string[] arrYears = moDtAcademicAndYearInfo.Rows[iRecordCount][S_YEAR_VALUE].ToString().Split('-');

            if (iRecordCount == 0 && Convert.ToInt32(arrYears[0]) <= DateTime.Now.Year)
            {
                cmbPhotoGalleryYear.Items.Add(new ListItem(arrYears[0], arrYears[0]));
                cmbPayrollYear.Items.Add(new ListItem(arrYears[0], arrYears[0]));
            }

            if (Convert.ToInt32(arrYears[1]) <= DateTime.Now.Year)
            {
                cmbPhotoGalleryYear.Items.Add(new ListItem(arrYears[1], arrYears[1]));
                cmbPayrollYear.Items.Add(new ListItem(arrYears[1], arrYears[1]));
            }
        }

        cmbPhotoGalleryYear.SelectedValue = DateTime.Now.Year.ToString();
        cmbPayrollYear.SelectedValue = DateTime.Now.Year.ToString();
    }

    /// <summary>
    /// Populates the Financial Year dropdown list on the page.
    /// </summary>
    private void FillFinancialYearCombo()
    {
        AccountsBaseClient oAccountsBaseClient = null;
        ddlFinancialYears.Items.Clear();
        try
        {
            oAccountsBaseClient = new AccountsBaseClient();
            oAccountsBaseClient.Open();
            List<FinancialYear> lstFinancialYears = oAccountsBaseClient.GetAllFinancialYears(miSchoolId);

            if (lstFinancialYears == null || lstFinancialYears.Count == 0)
            {
                trFinancialYearCombo.Visible = false;
                SetFinancialYearDetailsInSession(null);
            }


            if (IsAccountsModuleEnabled)
            {
                if (lstFinancialYears.Count > 0)
                {
                    lstFinancialYears.ForEach(fy =>
                    {
                        ddlFinancialYears.Items.Add(new ListItem(String.Format("{0}-{1}", fy.StartDate.Year, fy.EndDate.Year), fy.FinancialYearId.ToString()));
                        if (fy.FinancialYearId != miFinancialYearId)
                            return;
                        ddlFinancialYears.SelectedValue = miFinancialYearId.ToString();
                        SetFinancialYearDetailsInSession(fy);
                    });

                    // We only show the financial year dropdown list if it has more than 1 entry.
                    trFinancialYearCombo.Visible = lstFinancialYears.Count > 1;
                    if (lstFinancialYears.Count > 1)
                        divComboContainer.Visible = true;

                    if (lstFinancialYears.Count > 1)
                        divComboContainer.Visible = true;
                }

                CheckIfFinancialYearIsClosed();

                cmbAccountsFinancialYear.Items.Clear();
                lstFinancialYears.ForEach(fy =>
                {
                    cmbAccountsFinancialYear.Items.Add(new ListItem(String.Format("{0}-{1}", fy.StartDate.Year, fy.EndDate.Year), fy.FinancialYearId.ToString()));
                });

                cmbAccountsFinancialYear.SelectedValue = miFinancialYearId.ToString();
            }
            else
            {
                DateTime StarDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]);
                DateTime EndDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
                cmbAccountsFinancialYear.Items.Add(new ListItem(String.Format("{0}-{1}", StarDate.Year, EndDate.Year), "1"));
            }

            if (IsPayrollModuleEnabled)
            {
                cmbPayrollFinancialYear.Items.Clear();
                lstFinancialYears.ForEach(fy =>
                {
                    cmbPayrollFinancialYear.Items.Add(new ListItem(String.Format("{0}-{1}", fy.StartDate.Year, fy.EndDate.Year), fy.FinancialYearId.ToString()));
                });

                cmbPayrollFinancialYear.SelectedValue = miFinancialYearId.ToString();
            }
            else
            {
                DateTime StarDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]);
                DateTime EndDate = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
                cmbPayrollFinancialYear.Items.Add(new ListItem(String.Format("{0}-{1}", StarDate.Year, EndDate.Year), "1"));
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
                                                      "Accounts Module : An expcetion occurred while filling up the financial year drop down list.");
        }
        finally
        {
            if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
                oAccountsBaseClient.Close();
        }
    }

    /// <summary>
    /// Updates session values based on the selection.
    /// </summary>
    private void SetFinancialYearDetailsInSession(FinancialYear aoFinancialYear)
    {
        try
        {
            if (aoFinancialYear.IsNull())
            {
                miFinancialYearId = Constants.I_ZERO;
                Session[Constants.S_SESSION_FINANCIAL_YEAR_ID] = miFinancialYearId;
                Session[Constants.S_SESSION_FINANCIAL_YEAR] = null;
            }
            else
            {
                Session[Constants.S_SESSION_FINANCIAL_YEAR_ID] = aoFinancialYear.FinancialYearId;
                Session[Constants.S_SESSION_FINANCIAL_YEAR] = aoFinancialYear;
                miFinancialYearId = aoFinancialYear.FinancialYearId;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Checks if the selected Financial year is closed.
    /// Shows an appropriate message if it is closed.
    /// </summary>
    private void CheckIfFinancialYearIsClosed()
    {
        var trYearStatus = this.Master.FindControl("trYearStatus") as HtmlTableRow;
        var lblYearStatus = this.Master.FindControl("lblYearStatus") as Label;

        Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = String.Empty;
        trYearStatus.Visible = false;

        var oFinancialYear = Session[Constants.S_SESSION_FINANCIAL_YEAR] as FinancialYear;
        bool bIsNewFinancialYear = oFinancialYear != null && !oFinancialYear.IsCurrent && oFinancialYear.EndDate > DateTime.Now;
        bool bIsFinancialYearClosed = oFinancialYear != null && oFinancialYear.IsClosed && oFinancialYear.EndDate < DateTime.Now;
        bool bIsAcademicYearClosed = !Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_CLOSED].IsNull() && Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_CLOSED].ToString() == Constants.S_YES;

        bool bIsNewAcademicYear = false;
        if (Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED] != null)
            bIsNewAcademicYear = Session[Constants.S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED].ToString() == Constants.S_YES;

        string sMessage = string.Empty; ;
        trYearStatus.Visible = bIsFinancialYearClosed || bIsAcademicYearClosed || bIsNewFinancialYear || bIsNewAcademicYear;

        if (bIsNewFinancialYear && bIsNewAcademicYear)
        {
            sMessage = Resources.LocalizedResources.YouAreviewingDataOfNewAcademicYearAndFinancialYear;
            sMessage = sMessage.Replace("%d%", String.Format("({0})", cmbAcademicYearID.SelectedItem.Text));
            sMessage = sMessage.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
        }
        else if (bIsFinancialYearClosed && bIsAcademicYearClosed)
        {
            sMessage = Resources.LocalizedResources.YouAreViewingDataOfOldAcademicYearAndFinancialYearPleaseDoNotModify;
            sMessage = sMessage.Replace("%d%", String.Format("({0})", cmbAcademicYearID.SelectedItem.Text));
            sMessage = sMessage.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
        }
        else if (bIsNewFinancialYear && bIsAcademicYearClosed)
        {
            sMessage = Resources.LocalizedResources.YouAreViewingDataOfOldAcademicYearAndNewFinancialYear;
            sMessage = sMessage.Replace("%d%", String.Format("({0})", cmbAcademicYearID.SelectedItem.Text));
            sMessage = sMessage.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
        }
        else if (!bIsNewFinancialYear && bIsNewAcademicYear)
        {
            sMessage = Resources.LocalizedResources.YouAreViewingNewAcademicYear + " (" + cmbAcademicYearID.SelectedItem.Text + ").";
            sMessage = sMessage.Replace("%d%", String.Format("({0})", cmbAcademicYearID.SelectedItem.Text));
            sMessage = sMessage.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
        }
        else if (bIsNewFinancialYear && !bIsAcademicYearClosed)
            sMessage = Resources.LocalizedResources.YouAreViewingDataOfNewFinancialYear.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
        else if (!bIsNewFinancialYear && bIsAcademicYearClosed)
            sMessage = Resources.LocalizedResources.YouAreViewingDataOfOldAcademicYear.Replace("%d%", String.Format("({0})", cmbAcademicYearID.SelectedItem.Text));
        else if (bIsFinancialYearClosed && !bIsAcademicYearClosed)
        {
            sMessage = Resources.LocalizedResources.YouAreViewingDataOfOldFinancialYearPleaseDoNotModifyAnyData;
            sMessage = sMessage.Replace("%a%", String.Format("({0})", ddlFinancialYears.SelectedItem.Text));
        }
        Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = sMessage;
        lblYearStatus.Text = sMessage;
    }

    /// <summary>
    /// This function is used to provide data about all academic years of school.
    /// </summary>
    /// <returns></returns>
    private void GetAcademicYears()
    {
        InitializeMemberVariables();
        var oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDtYearInfo = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId, miUserId, moUserRole.ToInt());
        moDtAcademicAndYearInfo = oDtYearInfo;
    }

    /// <summary>
    /// Displays school notice
    /// </summary>
    private void DisplaySchoolNotice()
    {
        var oConfigureMenuBL = new ConfigureMenuBL();
        DataTable oDataTable = oConfigureMenuBL.FetchSchoolNoticess();

        if (oDataTable.Rows.Count > 1)
        {
            sMenuContent1 = HttpUtility.HtmlDecode(Convert.ToString(oDataTable.Rows[1]["ConfigureMenuContent"]));
            sMenuContent = HttpUtility.HtmlDecode(Convert.ToString(oDataTable.Rows[0]["ConfigureMenuContent"]));
        }
        else if (oDataTable.Rows.Count > 0)
            sMenuContent = HttpUtility.HtmlDecode(Convert.ToString(oDataTable.Rows[0]["ConfigureMenuContent"]));

        if (!string.IsNullOrEmpty(sMenuContent1))
        {
            if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != "N")
                hidSchoolNoticesPopUp.Value = "Y";
        }
        if (!string.IsNullOrEmpty(sMenuContent))
        {
            if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != "N")
                hidSchoolNoticesPopUp1.Value = "Y";
        }

        NoticeDivUC.Visible = Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != "N";
    }

    /// <summary>
    /// This method is used to set last login details.
    /// </summary>
    private void SetLastLoginDetails()
    {
        if (Session[Constants.S_SESSION_USER_LAST_LOGIN] != null)
        {
            lblLastLogin.Text = Session[Constants.S_SESSION_USER_LAST_LOGIN].ToString();
            lblLastLogin.Visible = true;
        }
        else
            lblLastLogin.Visible = false;
    }

    /// <summary>
    /// This method is used to access bonafide report id accoeding to school.
    /// </summary>
    private void SetBonafideReportLink()
    {
        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
            hidBonafideReportId.Value = BonafideReportID.PPSH.ToInt().ToString();
        else if (miSchoolId == Constants.SchoolId.SS.ToInt())
            hidBonafideReportId.Value = BonafideReportID.SS.ToInt().ToString();
        else if (miSchoolId == Constants.SchoolId.JPS.ToInt())
            hidBonafideReportId.Value = BonafideReportID.SS.ToInt().ToString();
        else
            hidBonafideReportId.Value = BonafideReportID.PP.ToInt().ToString();
    }

    /// <summary>
    /// This method is used to display panel according to login user role.
    /// </summary>
    private void DisplayControlPanelAccordingToRole()
    {
        if (!Settings.IsXseedAvailable)
        {
            trXseed.Visible = false;
            trXseedEmptyRow.Visible = false;
            trXseedScreens.Visible = false;
        }

        ReplaceXseedToPrePrimary();
        

        if (Settings.EnableAskMeFunctionality)
        {
            tdAskMeAdmin.Visible = true;
            trAskMeClassTeacher.Visible = true;
            trAskMeStudent.Visible = true;
            trAskMeTeacher.Visible = true;            
        }

        trStudentRecord.Visible = Settings.EnableStudentRecordModule;
        trOnlineExam.Visible = Settings.EnableOnlineExamModule;

        InitializeMemberVariables();
        var oSchoolUserBL = new SchoolUserBL();
        // Check if the user is 'Software Co-ordinator'. Returns 1 if it is.
        int iSuperAdminCount = oSchoolUserBL.GetSuperAdmin(miUserId, miSchoolId);
        hidServerDate.Value = DateTime.Today.ToString();
        Constants.S_SUPERVISOR_ROLE_NAME = Settings.SupervisorRoleName;
        string sUserRole;
        switch (moUserRole)
        {
            case Constants.UserRoles.Admin:
                CheckIfNewVouchersForApproval();
                if (iSuperAdminCount != 1)
                {
                    DisplayAdminRoleControlPanel();
                    divComboContainer.Visible = true;
                }
                else
                    DisplaySuperAdminRoleControlPanel();
                CheckLoginUser(true);

                treStore.Visible = Settings.EnableStoreModule;
                trUserLogin.Visible = Settings.IsAaryanSchool;
                trAcrossBranch.Visible = Settings.IsAaryanSchool;
                string sQuerystring = CommonUtility.EncryptQuerystring("From=1");
                HlinkHouseAssignment.NavigateUrl = "~/RITeSchool/Admin/StudentsHouseAssignmentUI.aspx?" + sQuerystring;
                break;
            case Constants.UserRoles.Student:
                {
                    sUserRole = Constants.UserRoles.Student.ToString();
                    hidStudentLogin.Value = "Y";
                    DisplayStudentRoleControlPanel();
                    var oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
                    bool bIsMonthConfig = oTeacherStandardDetailsCollectionBL.IsMonthConfiguration(Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt());
                    hlnkStudentPR.NavigateUrl = bIsMonthConfig ? "~/RITeSchool/Teacher/StudentProgressReportEntry.aspx" : "~/RITeSchool/Student/StudentProgressSheet.aspx";
                    GetUnreadQuestionCount(sUserRole);
                }
                break;
            case Constants.UserRoles.Supervisor:
                CheckIfNewVouchersForApproval();
                DisplaySupervisorRoleControlPanel();                
                break;
            case Constants.UserRoles.OtherStaff:
                DisplayOtherStaffRoleControlPanel();
                break;
            case Constants.UserRoles.Teacher:
                sUserRole = Constants.UserRoles.Teacher.ToString();
                CheckIfNewVouchersForApproval();
                DisplayTeacherRoleControlPanel();
                GetUnreadQuestionCount(sUserRole);
                CheckLoginUser(true);

                //if (moSchool == Constants.SchoolId.SNS)
                //{
                //    trMarkAssignment.Visible = false;
                //    trMarkAssignmentClassTeaqcher.Visible = false;
                //}
                break;
        }
        SetNavigationURLToLinks();
    }

    /// <summary>
    /// Set navigation URL to link
    /// </summary>
    private void SetNavigationURLToLinks()
    {
        string ptaUrl = GetCommitteeScreenURL(Constants.SchoolCommittees.PTA);
        string transportCommitteeUrl = GetCommitteeScreenURL(Constants.SchoolCommittees.Transport);
        lnkStudentPTA.NavigateUrl = ptaUrl;
        lnkTransportCommitteeForStudentLogin.NavigateUrl = transportCommitteeUrl;
        lnkAdminPTA.NavigateUrl = ptaUrl;
        lnkTransportCommittee.NavigateUrl = transportCommitteeUrl;
        lnkSurveyFeedback.NavigateUrl = GetSurveyFeedbackUrl();
    }

    /// <summary>
    /// This method is used to display supervisor role control panel.
    /// </summary>
    private void DisplaySupervisorRoleControlPanel()
    {
        //tdAdminNotice.Visible = false;
        SetUserDetails();        
        tblStudents.Visible = true;
        tblStudentMenu.Visible = false;
        tblTeacherDetails.Visible = false;
    }

    /// <summary>
    /// This method is used to display other staff role control panel.
    /// </summary>
    private void DisplayOtherStaffRoleControlPanel()
    {
        //tdAdminNotice.Visible = false;
        SetUserDetails();
        tblStudents.Visible = true;
        tblStudentMenu.Visible = false;
        tblTeacherDetails.Visible = false;
    }

    /// <summary>
    /// This method is used to display student role control panel.
    /// </summary>
    private void DisplayStudentRoleControlPanel()
    {
        //tdAdminNotice.Visible = false;
        tdAcademicCmblbl.Visible = false;
        tdAcademicCmb.Visible = false;
        divComboContainer.Visible = false;
        
        tblStudents.Visible = true;
        tblStudentMenu.Visible = true;

        trParentHealthDetails.Visible = Settings.EnableParentHealthDetailsAtStudentLogin;

        if (Settings.EnableOnlineExamModule)
        {
            tdOnlineExamResult.Visible = true;
            tdOnlineExamProgressReport.Visible = true;
        }
        else
        {
            tdOnlineExamResult.Visible = false;
            tdOnlineExamProgressReport.Visible = false;
        }

        if (!Settings.EnableTransportModule || !Settings.EnableTransportCommitteeForStudentLogin)
            trTransportCommitteeForStudentLogin.Visible = false;

        // Condition is used to hide message center for parent login.
        if (moUserRole == Constants.UserRoles.Student && !Settings.IsEnableMessageCenterToParent)
        {
            trStudentMessageCenter.Visible = false;
            divMessagecenter.Visible = false;
        } 

        //Codition is used to hide Subject teacher screen on student login
        if (moUserRole == Constants.UserRoles.Student && !Settings.IsEnableSubjecTeacherScreen)
        {
            trSubjectTeacher.Visible = false;
        }

        if (moSchool == Constants.SchoolId.SNS && moUserRole == Constants.UserRoles.Student)
            trStudentAssessment.Visible = true;
        else
            trStudentAssessment.Visible = false;

       if (moSchool == Constants.SchoolId.PPSN && moUserRole == Constants.UserRoles.Student)
        {
            StudentAssessmentBL oStudentAssessmentBL = new StudentAssessmentBL(miSchoolId, miAcademicYearId, miUserId);
            trStudentAssessment.Visible = oStudentAssessmentBL.AllowSelfAssessmentscreen();
			
			trStudentExamWiseSubjectMarkDetails.Visible = true;
        }
        
        trSurveyModule.Visible = Settings.EnableSurveyModule;

        hlnkExamSchedule.NavigateUrl = GetEncryptedStandardQueryString(Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt());
        tblStudentDetails.Visible = true;
        DataSet oDS = SetStudentDetails();
        DataTable oDT = oDS.Tables[0];

        SetNewMessageLink(imgBtnMsgAlertStud, oDS.Tables[2].Rows[0][0].ToInt());

        trStudentTT.Visible = Settings.EnableTimetableMenuForStudentLogin;

        // If the loggin student is preprimary then hide Timetable, Progress Report and exam Schedule menus.
        if (Session[Constants.S_SESSION_IS_STD_PREPRIMARY].ToString() == Constants.C_YES.ToString())
        {
            if (!Settings.IsTimeTableForPrePrimaryStud)
                trStudentTT.Visible = false;

            if (!Settings.IsExamScheForPrePrimaryStud)
                trStudentES.Visible = false;

            trSubjectTeacher.Visible = false;
        }

        trReports.Visible = Settings.IsReportApplicableToStudent;

        var oSchoolUserBL = new SchoolUserBL(miUserId);
        lblMobileOne.Text = oSchoolUserBL.Mobile_Number.Trim();
        if (oSchoolUserBL.Mobile_Number2 != string.Empty)
            lblMobileOne.Text += ", " + oSchoolUserBL.Mobile_Number2.Trim();
        trStudentLibrary.Visible = Settings.EnableLibraryModule && Settings.EnableLibraryLinkForStudentLogin && !Settings.IsMiniSite;
        trParentTeacherAssociationForStudent.Visible = Settings.EnablePTAModule && Settings.EnablePTAModuleforStudents && !Settings.IsMiniSite;
        trTransportDetails.Visible = Settings.EnableTransportModule && Settings.EnableTransportLinkForStudentLogin && !Settings.IsMiniSite;
        trStudentHomework.Visible = (Settings.EnableHomeworkModule && !Settings.IsMiniSite && Settings.EnableHomeworkModuleForStudentLogin);
        
        int iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
        int iNextStandardId = StudentAdmissionsBL.IsInternalAdmissionEnabled(miSchoolId, iStandardId);
        if (iNextStandardId != 0)
        {
            int iAdmissionId = StudentAdmissionsBL.IsAdmissionDone(miSchoolId, iNextStandardId, miUserId);

            if (iAdmissionId != 0)
                hlnkNextYearAdmission.Text = "Next Year Admission Form";

            trNextYearAdmission.Visible = true;
            string sQuerystring = CommonUtility.EncryptQuerystring("StandardId=" + iNextStandardId + "&EnableAdmissionFormFee=true");
            hidAdmissionQueryString.Value = sQuerystring;                                                                                                                                                                                                                                      
            hlnkNextYearAdmission.Attributes.Add("onclick","OpenAdmissionPopup()");
        }
        else

            trNextYearAdmission.Visible = false;

        trStudentFee.Visible = Settings.EnableStudentFeesModule;


        if (Session[Constants.S_SESSION_IS_10TH_STD_STUDENT].ToString() == "1")
        {
            trAnnualPlanner.Visible = false;
            trStudentAttendance.Visible = false;
            
            if (miSchoolId != Constants.SchoolId.PPS.ToInt())
                trReports.Visible = false;

            trParentTeacherAssociationForStudent.Visible = false;
            trStudentES.Visible = false;
            trHolidays.Visible = false;
            trStudentHomework.Visible = false;
            trSubjectTeacher.Visible = false;
            trStudentTT.Visible = false;
            trStudentLibrary.Visible = false;
            //trStudentFee.Visible = false;
            trChangePassword.Visible = false;
            trAddStudentDetails.Visible = false;
            trUploadParentDetails.Visible = false;

            if (miSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                trStudentFee.Visible = false;
                trStudentSMSCenter.Visible = false;
                trStudentPR.Visible = false;
            }
        }

        hidHideVidgets.Value = Constants.S_ZERO;
        if (Session[Constants.S_SESSION_ENABLE_LOGIN_FOR_LEFT_STUDENTS].ToString() == "1")
        {
            trAnnualPlanner.Visible = false;
            trStudentAttendance.Visible = false;
            trParentTeacherAssociationForStudent.Visible = false;
            trStudentES.Visible = false;
            trHolidays.Visible = false;
            trStudentHomework.Visible = false;
            trSubjectTeacher.Visible = false;
            trStudentTT.Visible = false;
            trStudentLibrary.Visible = false;
            trStudentFee.Visible = false;
            trChangePassword.Visible = false;
            trAddStudentDetails.Visible = false;
            trUploadParentDetails.Visible = false;
            trStudentMessageCenter.Visible = false;
            trStudentSMSCenter.Visible = false;
            trUploadStudentPhoto.Visible = false;

            trLeavingCertificate.Visible = true;
            hidHideVidgets.Value = Constants.S_ONE;
            divMessagecenter.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to display admin role control panel.
    /// </summary>
    private void DisplayAdminRoleControlPanel()
    {
        if (!Settings.IsCautionMoneyApplicable)
            hlnkCautionMoney.Visible = false;

        tblAdmin.Visible = true;
        tdSidebar.Visible = false;
        tblStudents.Visible = true;
        tdAssignGrades.Visible = Settings.EnableObservationSystem;

        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
            tdAdminPhoto.Visible = true;

        SetNewMessageLink(imgBtnMessageAlert);
        SetStaffBirthdayLink(imgBtnStaffBirthdayAlert);

        FillAcademicYearCombo();
        Session[Constants.S_SESSION_USER_FULLNAME] = Session[Constants.S_SESSION_USER_NAME];
        //tdAdminNotice.Visible = !Settings.IsMiniSite;
        GetAdminNoticeDetails();

        trLibraryModule.Visible = Settings.EnableLibraryModule && !Settings.IsMiniSite;
        plcHolderInventoy.Visible = Settings.EnableInventoryModule && !Settings.IsMiniSite;
        trPayroll.Visible = Settings.EnablePayrollModule && !Settings.IsMiniSite;
        trAccounts.Visible = Settings.EnableAccountsModule && !Settings.IsMiniSite;
        trTransport.Visible = Settings.EnableTransportModule && !Settings.IsMiniSite;
        trTaskManagement.Visible = Settings.EnableTaskManagementModule && !Settings.IsMiniSite;
        trAssembly.Visible = Settings.EnableAssemblyModule && !Settings.IsMiniSite;
        trHomework.Visible = Settings.EnableHomeworkModule && !Settings.IsMiniSite;
        //trLessonPlanRelated.Visible = Settings.EnableLessonPlanModule && !Settings.IsMiniSite;
        tdOnlineAdmission.Visible = !Settings.IsMiniSite;
        trPerformanceRelated.Visible = Settings.EnableStaffPerformanceModule && !Settings.IsMiniSite;
        trSurveyModuleOfJPS.Visible = Settings.EnableSurveyModuleOfJPS && !Settings.IsMiniSite;
        trSurveyModuleForAdmin.Visible = Settings.EnableSurveyModuleForAdmin && !Settings.IsMiniSite;
        trParentTeacherAssociation.Visible = Settings.EnablePTAModule && !Settings.IsMiniSite;
        if ( Settings.IsEnableExternalActivities)
        {
            trExternalActivities.Visible = true;
            trExternalActivities1.Visible = true;
        }
        else
        {
            trExternalActivities.Visible = false;
            trExternalActivities1.Visible = false;
        }

        if (Settings.EnableDescriptiveIndicatorAssignment)
            tdDescriptiveIndecators.Visible = true;
        else
            tdDescriptiveIndecators.Visible = false;

        hlnkBlackListedStudents.NavigateUrl = hlnkBlackListedStudents.NavigateUrl + "?" + CommonUtility.EncryptQuerystring("IsFromStudentScreen=Y");
    }

    /// <summary>
    /// This method is used to display teacher role control panel.
    /// </summary>
    private void DisplayTeacherRoleControlPanel()
    {
        tdAcademicCmblbl.Visible = false;
        //tdAdminNotice.Visible = false;
        tdAcademicCmb.Visible = false;
        divComboContainer.Visible = false;
        bool bEnableLibrary = Settings.EnableLibraryModule;
        trLessonPlanRelated.Visible = Settings.EnableLessonPlanModule && !Settings.IsMiniSite;

        // If loggedin user is class teacher or admin user then only display the attendance menu.
        if (Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.C_YES.ToString())
        {
            tblStudentMenu.Visible = false;
            tblStudents.Visible = true;
            tblTeacherDetails.Visible = true;
            tblClassTeacher.Visible = true;
            DataSet oDataSet = DisplayTeacherDetails();
            SetPrePrimaryTecherLinks(oDataSet.Tables[0]);
            SetNewMessageLink(imgBtnMsgAlertClsT, oDataSet.Tables[2].Rows[0][0].ToInt());
            SetStaffBirthdayLink(imgBtnBirthdayAlertClsT, oDataSet.Tables[4].Rows[0][0].ToInt());
            trClassTeacherInventory.Visible = Settings.EnableInventoryModule && !Settings.IsMiniSite;
            trClassTeacherLibrary.Visible = bEnableLibrary && !Settings.IsMiniSite;
            trClassTeacherHomework.Visible = Settings.EnableHomeworkModule && !Settings.IsMiniSite;
            trClassTeacherMeassagecenter.Visible = trClassTechersmscenter.Visible = !Settings.IsMiniSite;
            trAssignGradeClassTeacher.Visible = Settings.EnableObservationSystem;

            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                trTeacherPhoto1.Visible = true;

            if (!String.IsNullOrEmpty(Settings.BetaVersionURL))
            {
                trBetaVersionForClassTeacher.Visible = true;
                hlnkBetaVersionForClassTeacher.NavigateUrl = Settings.BetaVersionURL + CommonUtility.EncryptQuerystring("SchoolId=" + miSchoolId + "&UserId=" + miUserId).Replace("+", "%20").Replace("/", "%2F");
                hlnkBetaVersionForClassTeacher.Target = "_blank";
            }
            else
                trBetaVersionForClassTeacher.Visible = false;
        }
        else
        {
            trTeacherSmsCenter.Visible = trTecherMessageCenter.Visible = !Settings.IsMiniSite;
            DisplayTeacherDetails();
            tblTeacherDetails.Visible = true;
            tblTeacher.Visible = true;
            tblStudentMenu.Visible = false;
            tblStudents.Visible = true;
            SetNewMessageLink(imgBtnMsgAlertT);
            SetStaffBirthdayLink(imgBtnBirthdayAlertT);
            trSubTeacherInventory.Visible = Settings.EnableInventoryModule && !Settings.IsMiniSite;
            trTeacherLibrary.Visible = bEnableLibrary && !Settings.IsMiniSite;
            trTeacherHomework.Visible = Settings.EnableHomeworkModule && !Settings.IsMiniSite;
            trAssignGradesTeacher.Visible = Settings.EnableObservationSystem;

            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                trTeacherPhoto2.Visible = true;

            if (!String.IsNullOrEmpty(Settings.BetaVersionURL))
            {
                trBetaVersionForTeacher.Visible = true;
                hlnkBetaVersionForTeacher.NavigateUrl = Settings.BetaVersionURL + CommonUtility.EncryptQuerystring("SchoolId=" + miSchoolId + "&UserId=" + miUserId).Replace("+", "%20").Replace("/", "%2F");
                hlnkBetaVersionForTeacher.Target = "_blank";
            }
            else
                trBetaVersionForTeacher.Visible = false;
        }

        if (Convert.ToChar(Session[Constants.S_SESSION_ISACADEMICYRAPPLICABLE]) == Constants.C_YES)
        {
            trAcademicCmb.Visible = true;
            tdAcademicCmblbl.Visible = true;
            tdAcademicCmb.Visible = true;
            divComboContainer.Visible = true;




            FillAcademicYearCombo();
        }
        else
        {
            trAcademicCmb.Visible = true;

           
            lblLastLogin.Visible = true;
            Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS] = string.Empty;
        }

        DisplaySupervisorMenuDetails(tblTeacherLeft);
        ShowOnlyAdditionalAccessMenu();

        var oSchoolUserBL = new SchoolUserBL(miUserId, miSchoolId, miAcademicYearId, true);
        lblTeacherMobile.Text = oSchoolUserBL.Mobile_Number.Trim();
        if (oSchoolUserBL.Mobile_Number2 != string.Empty)
            lblTeacherMobile.Text += ", " + oSchoolUserBL.Mobile_Number2.Trim();

        if (!Settings.DisplayWeeklyTimtableLink)
        {
            trWeeklyTimetable.Visible = false;
            trWeeklyTimeTableTeacher.Visible = false;
        }
        else
        {
            trWeeklyTimetable.Visible = true;
            trWeeklyTimeTableTeacher.Visible = true;
        }

        char cHasTimetableEditAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.WeeklyTimetable);
        if (moSchool == Constants.SchoolId.VPMCPS && moUserRole == Constants.UserRoles.Teacher)
        {
            if ((Session[Constants.S_SESSION_IS_CLASS_TEACHER] != null && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == "Y") || cHasTimetableEditAccess == Constants.C_YES)
            {
                trWeeklyTimetable.Visible = true;
                trWeeklyTimeTableTeacher.Visible = true;
            }
            else
            {
                trWeeklyTimetable.Visible = false;
                trWeeklyTimeTableTeacher.Visible = false;
            }

            trStudentMenu.Visible = false;
        }

        if (moSchool == Constants.SchoolId.SNS && moUserRole == Constants.UserRoles.Teacher)
        {
            trPeerConfig.Visible = true;
            trStudentListForAssessment.Visible = true;
        }
        else
        {
            trPeerConfig.Visible = false;
            trStudentListForAssessment.Visible = false;
        }
    }

    /// <summary>
    /// This method is called when the logged in user is 'Software Co-ordinator'.
    /// Most of the menus are disabled for this user.
    /// </summary>
    private void DisplaySuperAdminRoleControlPanel()
    {
        trTransportCommitteeForStudentLogin.Visible = false;
        trParentTeacherAssociationForStudent.Visible = false;
        trStudentLibrary.Visible = false;
        tdAcademicCmblbl.Visible = false;
        tdAcademicCmb.Visible = false;
        divComboContainer.Visible = false;
        //tdAdminNotice.Visible = false;
        tblStudents.Visible = true;
        trStudentAttendance.Visible = false;
        trStudentPR.Visible = false;
        trStudentTT.Visible = false;
        HyperLink34.Visible = false;
        HyperLink53.Visible = false;
        HyperLink73.Visible = false;
        trTransportDetails.Visible = false;
        lnkHolidayList.Visible = false;
        trStudentES.Visible = false;
        trSubjectTeacher.Visible = false;
        HyperLink36.Visible = false;
        trStudentHomework.Visible = false;        
        SetNewMessageLink(imgBtnMsgAlertStud);
    }

    /// <summary>
    /// This method is used t0 display notice board.
    /// </summary>
    private void GetAdminNoticeDetails()
    {
        DataSet oDS = SuperAdminBL.GetAdminNoticeForControlPanel(miUserId, miSchoolId, miAcademicYearId);
        //lblAdminNotice.Text = ShowNoticeBoardMessage(oDS.Tables[0]);
        (this.Master.FindControl("LabelNoticeBoardMsg") as Label).Text = ShowNoticeBoardMessage(oDS.Tables[0]);
    }

    /// <summary>
    /// This method is used to retrive student information from database and return dataset.
    /// </summary>
    /// <returns></returns>
    private DataSet SetStudentDetails()
    {
        DataSet oDSStudent = StudentBL.GetStudentDetailsForControlPanel(Session[Constants.S_SESSION_STUDENT_ID].ToInt(), miSchoolId, miAcademicYearId);
        DataTable oDTStudent = oDSStudent.Tables[0];

        //lblNoticeBoardMsg.Text = ShowNoticeBoardMessage(oDSStudent.Tables[3]);
        (this.Master.FindControl("divSchoolNoticeBoard")).Visible = true;
        (this.Master.FindControl("LabelNoticeBoardMsg") as Label).Text = ShowNoticeBoardMessage(oDSStudent.Tables[3]);

        if ((moSchool == Constants.SchoolId.PPSH || moSchool == Constants.SchoolId.PPS) && moUserRole == Constants.UserRoles.Student)
            HyperLink46.Text = oDTStudent.Rows[0]["StudentFullName"].ToString() + " (GR. No. : " + oDTStudent.Rows[0]["Enrolment_Number"].ToString() + ")";
        else
            HyperLink46.Text = oDTStudent.Rows[0]["StudentFullName"].ToString();
		
        Session[Constants.S_SESSION_USER_FULLNAME] = oDTStudent.Rows[0]["StudentFullName"].ToString();
        Label1.Text = oDTStudent.Rows[0]["studentClass"].ToString();
        lblRollNo.Text = oDTStudent.Rows[0]["Roll_No"].ToString();
        DateTime oDtDob = oDTStudent.Rows[0]["DOB"].ToString().ToDateTime();
        lblDOB.Text = oDtDob.ToString(Constants.S_STANDARD_DATE_FORMAT);
        return oDSStudent;
    }

    /// <summary>
    /// This method is used to set user details.
    /// </summary>
    /// <returns></returns>
    private void SetUserDetails()
    {
        switch (moUserRole)
        {
            case Constants.UserRoles.Supervisor:
                {
                    //string sIsReportApplToSupervisor = SchoolSettings.ResourceManager.GetString("IsReportApplicableToSupervisor"); Dont delete this line 
                    if (Convert.ToChar(Session[Constants.S_SESSION_ISACADEMICYRAPPLICABLE]) == Constants.C_YES)
                    {
                        trAcademicCmb.Visible = true;
                        tdAcademicCmblbl.Visible = true;
                        tdAcademicCmb.Visible = true;
                        divComboContainer.Visible = true;
                        FillAcademicYearCombo();
                    }
                   else
                    {
                        trAcademicCmb.Visible = false;
                    }
                     
                    //tblSupervisor.Visible = true;
                    DisplaySupervisorMenuDetails(tblSuperLeft);
                    // Dont delete this code
                    //if (sIsReportApplToSupervisor == Constants.C_NO.ToString())
                    //    tdAdminReports.Visible = false;

                    tblSuperLeft.Visible = true;
                    tblSepervisorDetails.Visible = true;
                    //tdSidebar.Visible = false;
                    //tblSuperLeft.Width = Unit.Percentage(15);
                    //tblStudents.Width ="85%";

                    //FillSupervisorDetails();
                    DataSet oDSSupervisorDetails = DisplaySupervisorDetails();

                    var oSchoolUserBL = new SchoolUserBL(miUserId);
                    lblSuperwiserMob.Text = oSchoolUserBL.Mobile_Number.Trim();
                    if (oSchoolUserBL.Mobile_Number2 != string.Empty)
                        lblSuperwiserMob.Text += ", " + oSchoolUserBL.Mobile_Number2.Trim();
                }
                break;
            case Constants.UserRoles.OtherStaff:
                {
                    trAcademicCmb.Visible = false;
                    //tblSupervisor.Visible = true;
                    DisplayOtherStaffMenuDetails(tblSuperLeft);

                    tblSuperLeft.Visible = true;
                    tblSepervisorDetails.Visible = true;
                    //tdSidebar.Visible = false;
                    //tblSuperLeft.Width = Unit.Percentage(15);
                    //tblStudents.Width = "85%";
                    //FillSupervisorDetails();
                    DataSet oDSOtherStaffDetails = DisplayOtherStaffDetails();
                }
                break;
            case Constants.UserRoles.Teacher:
                {
                    InitializeMemberVariables();
                    int iCurrentTeacherId = Session[Constants.S_SESSION_TEACHER_ID].ToInt();
                    DataTable oDataTable = SchoolWiseTeacherMasterBL.GetTeacherDetails(miSchoolId, miAcademicYearId, iCurrentTeacherId);
                    if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
                    {
                        Session[Constants.S_SESSION_TEACHER_ID] = oDataTable.Rows[0]["Teacher_Id"];
                        string cIsClassTEacher = Convert.ToString(oDataTable.Rows[0]["Is_ClassTeacher"]);
                        Session[Constants.S_SESSION_IS_CLASS_TEACHER] = cIsClassTEacher;
                        Session[Constants.S_SESSION_IS_MPT_APPLICABLE] = oDataTable.Rows[0]["MPT_Applicable"].ToString();
                        Session[Constants.S_SESSION_IS_ASSEMBLY_APPLICABLE] = oDataTable.Rows[0]["Assembly_Applicable"].ToString();
                        Session[Constants.S_SESSION_IS_STAYBACK_APPLICABLE] = oDataTable.Rows[0]["Stayback_Applicable"].ToString();
                        if (oDataTable.Rows[0]["StandardDivisionId"].ToString() != "-")
                            Session[Constants.S_SESSION_TEACHER_STDDIV_ID] = oDataTable.Rows[0]["StandardDivisionId"].ToInt();
                        tblTeacher.Visible = false;
                        tblClassTeacher.Visible = false;                        
                    }
                    DisplayTeacherRoleControlPanel();
                }
                break;
            default:
                GetAdminNoticeDetails();
                break;
        }
    }

    /// <summary>
    /// This method is used to return notice board message.
    /// </summary>    
    private string ShowNoticeBoardMessage(DataTable aoDTNoticeMsg)
    {
        string sNoticeMsg = string.Empty;
        DataRow[] oDRMessages = aoDTNoticeMsg.Select("Is_Default_Msg=False");
        if (oDRMessages.Length > 0)
            sNoticeMsg = DisplayMessages(oDRMessages);
        if (sNoticeMsg == string.Empty)
        {
            oDRMessages = aoDTNoticeMsg.Select("Is_Default_Msg=True");
            sNoticeMsg = DisplayDefaultMessage(oDRMessages);
        }
        return sNoticeMsg;
    }

    /// <summary>
    /// This method is used to display notice board messages.
    /// </summary>
    /// <param name="aoDRMessages"></param>
    /// <returns></returns>
    private string DisplayMessages(DataRow[] aoDRMessages)
    {
        string sNoticeMsg = string.Empty;
        DateTime dtToday = DateTime.Today;
        DateTime dtStartDate;
        DateTime dtEndDate;
        string sMessageColor = S_VIOLET_COLOR;

        foreach (DataRow dtRow in aoDRMessages)
        {
            dtStartDate = dtRow["Start_Date"].ToDateTime();
            dtEndDate = dtRow["End_Date"].ToDateTime();
            if (dtToday < dtStartDate || dtToday > dtEndDate)
                continue;

            if (sNoticeMsg.Equals(string.Empty))
                sNoticeMsg = Convert.ToString(dtRow[S_MESSAGE_FEILD]);
            else
                sNoticeMsg = string.Format("{0} &nbsp;&nbsp;&nbsp;&nbsp;<font color='black'>&sect;</font><font color='{1}'>&nbsp;&nbsp;&nbsp;&nbsp;{2}</font>", sNoticeMsg, sMessageColor, Convert.ToString(dtRow[S_MESSAGE_FEILD]));

            sMessageColor = sMessageColor.Equals(S_DARK_GREEN_COLOR) ? S_VIOLET_COLOR : S_DARK_GREEN_COLOR;
        }
        return sNoticeMsg;
    }

    /// <summary>
    /// This method is used to display default message.
    /// </summary>
    /// <param name="aoDRMessages"></param>
    private string DisplayDefaultMessage(DataRow[] aoDRMessages)
    {
        string sNoticeMsg = string.Empty;
        foreach (DataRow dtRow in aoDRMessages)
            sNoticeMsg = Convert.ToString(dtRow[S_MESSAGE_FEILD]);
        return sNoticeMsg;
    }
    //This method use to give direct access of bonafide certificate and student link to superviser
    private DataTable GetAccessGivenOnControlPanel()
    {
        var oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        DataTable oDTSupAccessPages = oSchoolWiseSupervisorMasterBL.GetStudentBonafideCertificateToSuperwiser(miUserId);
        return oDTSupAccessPages;
    }

    /// <summary>
    /// This method is used to display supervisor access paged hyperlink.
    /// </summary>
    /// <param name="aoMenuTable"> </param>
    private void DisplaySupervisorMenuDetails(Table aoMenuTable)
    {
        int iDisplyedManu = 0;
        DataTable oDTSupAccessPages = GetSupervisorAccessPages();
        DataTable oDTDirectAccess = GetAccessGivenOnControlPanel();
        
        if (oDTSupAccessPages.Rows.Count > 0)
        {
            for (int iCount = 0; iCount < oDTSupAccessPages.Rows.Count; iCount++)
            {
                Boolean bAddMenu = true;
                if (moUserRole == Constants.UserRoles.Teacher)
                    bAddMenu = CheckIfMenuAlreadyAvailable(oDTSupAccessPages.Rows[iCount]["Configure_Name"].ToString(), oDTSupAccessPages.Rows[iCount]["NavigateURL"].ToString());

                if (!bAddMenu)
                    continue;

                iDisplyedManu++;

                if (oDTSupAccessPages.Rows[iCount]["Configure_Name"].ToString() == "Caution Money Details" && !Settings.IsCautionMoneyApplicable)
                    continue;

                //tdMid.ColSpan = oDTSupAccessPages.Rows.Count;
                FillMenuControls(oDTSupAccessPages.Rows[iCount], aoMenuTable);
            }

            if (HidAttendanceAlert.Value == Constants.S_YES && moUserRole == Constants.UserRoles.Supervisor)
                MissingAttendanceLink(aoMenuTable);

            if (moUserRole == Constants.UserRoles.Supervisor)
                ShowBitaURLLinkForAdminStaff(aoMenuTable);


            if (hidShowAttendanceDiv.Value == "YES" ||(ISABSENT_STUDENT__LINK_VISIBEL == true && moUserRole == Constants.UserRoles.Supervisor))
                AbsentStudentDetailsLink(aoMenuTable);


            //if (moUserRole == Constants.UserRoles.Supervisor)
            //{
            //        HtmlAnchor link = new HtmlAnchor();
            //        link.InnerText = "Payment Clearance Notification";
            //        link.Attributes.Add("class", "SubTitleMenu");
            //        link.Attributes.Add("onclick", "ShowClearanceNotification()");
            //        link.Style.Add("cursor", "pointer");

            //        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            //        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
            //        if (lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowPaymentClearanceNotification.ToInt() && ru.UserId == miUserId).Any())
            //        {
            //            link.Visible = true;
            //        }
            //        else
            //            link.Visible = false;

            //        var tRow = new TableRow();
            //        tRow.ID = "trPaymentClearances";
            //        tRow.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
            //        var tCell = new TableCell
            //        {
            //            CssClass = "ClsBorderlight",
            //            ColumnSpan = 1,
            //            HorizontalAlign = HorizontalAlign.Left
            //        };
            //        tCell.Controls.Add(link);
            //        tRow.Controls.Add(tCell);
            //        aoMenuTable.Controls.Add(tRow);
            //}

            if (moUserRole == Constants.UserRoles.Supervisor)
            {
                bool bVal = CheckLoginUser(false);

                if (bVal == true)
                {
                    HtmlAnchor link = new HtmlAnchor();
                    link.InnerText = S_NONPERMANANT_TEACHERS;
                    link.Attributes.Add("class", "SubTitleMenu");
                    link.Attributes.Add("onclick", "ShowTeacherAlertPopup()");
                    link.Style.Add("cursor", "pointer");
                    var tRow = new TableRow();
                    var tCell = new TableCell
                    {
                        CssClass = "ClsBorderlight",
                        ColumnSpan = 1,
                        HorizontalAlign = HorizontalAlign.Left
                    };
                    tCell.Controls.Add(link);
                    tRow.Controls.Add(tCell);
                    aoMenuTable.Controls.Add(tRow);
                }
            }

            if (oDTDirectAccess.Rows[0]["HasStudentAccess"].ToString() == Constants.S_YES || oDTDirectAccess.Rows[0]["HasReportAccess"].ToInt() == Constants.I_ONE)
            {
                var tRow = new TableRow();
                var tCell = new TableCell
                {
                    CssClass = "header-color-green-custom",
                    ColumnSpan = 2,
                    HorizontalAlign = HorizontalAlign.Left,
                    
                };

                var olblDirectAccess = new Label
                {
                    Text = "Direct Access",
                    EnableViewState = false
                };
                tCell.Controls.Add(olblDirectAccess);
                tRow.Controls.Add(tCell);
                aoMenuTable.Controls.Add(tRow);
            }            

            if (oDTDirectAccess.Rows[0]["HasReportAccess"].ToInt() == Constants.I_ONE)
                FillMenuControlsWithBonafideReportForAdminStaff(aoMenuTable);

            if (oDTDirectAccess.Rows[0]["HasStudentAccess"].ToString() == Constants.S_YES)
                FillMenuControlsWithStudentListForAdminStaff(aoMenuTable);            

        }       

        aoMenuTable.Visible = iDisplyedManu != 0;
    }

    /// <summary>
    /// This method is used to display supervisor access paged hyperlink.
    /// </summary>
    /// <param name="aoMenuTable"> </param>
    private void DisplayOtherStaffMenuDetails(Table aoMenuTable)
    {
        int iDisplyedManu = 0;
        DataTable oDTSupAccessPages = GetOtherStaffAccessPages();
        if (oDTSupAccessPages.Rows.Count > 0)
        {
            for (int iCount = 0; iCount < oDTSupAccessPages.Rows.Count; iCount++)
            {
                Boolean bAddMenu = true;
                if (moUserRole == Constants.UserRoles.Teacher)
                    bAddMenu = CheckIfMenuAlreadyAvailable(oDTSupAccessPages.Rows[iCount]["Configure_Name"].ToString(), oDTSupAccessPages.Rows[iCount]["NavigateURL"].ToString());

                if (!bAddMenu)
                    continue;

                iDisplyedManu++;

                if (oDTSupAccessPages.Rows[iCount]["Configure_Name"].ToString() == "Caution Money Details" && !Settings.IsCautionMoneyApplicable)
                    continue;

                //tdMid.ColSpan = oDTSupAccessPages.Rows.Count;
                FillMenuControls(oDTSupAccessPages.Rows[iCount], aoMenuTable);
            }
        }

        aoMenuTable.Visible = iDisplyedManu != 0;
    }

    private DataTable GetOtherStaffAccessPages()
    {
        var oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        DataTable oDTSupAccessPages = oSchoolWiseSupervisorMasterBL.GetSupervisorAccessPages(miUserId);
        return oDTSupAccessPages;
    }

    /// <summary>
    /// This method use to fill menu control with student list for AdminStaff only
    /// </summary>
    /// <param name="tblSupervisorMenu"></param>
    private void FillMenuControlsWithStudentListForAdminStaff(Table tblSupervisorMenu)
    {

        var tRow = new TableRow();
        var tCell = new TableCell
        {
            CssClass = "ClsBorderlight",
            ColumnSpan = 1,
            HorizontalAlign = HorizontalAlign.Left
        };

        var ohlinkPageName = new HyperLink
        {
            Text = "Students",
            NavigateUrl = "../Admin/AllStudentsUI.aspx",
            CssClass = "SubTitleMenu",
            EnableViewState = false
        };
        tCell.Controls.Add(ohlinkPageName);
        tRow.Controls.Add(tCell);
        tblSupervisorMenu.Controls.Add(tRow);
    }


    /// <summary>
    /// This method use to fill menu control with Bonafide certificate for AdminStaff only
    /// </summary>
    /// <param name="tblSupervisorMenu"></param>
    private void FillMenuControlsWithBonafideReportForAdminStaff(Table tblSupervisorMenu)
    {
        string sBonafideCertificateReportFileName = ReportsBL.GetBonafideReportFileName();
        string sBonafideReport =
          "rpt=" + Server.MapPath("~") + @"\RITeSchool\Report\Student\" + sBonafideCertificateReportFileName + "&d=" +
                   Server.MapPath("~") + @"\RITeSchoolReport&ReportID=" + hidBonafideReportId.Value + "&IsSearchGridConsidered=1&ReportName=Bonafide Certificate&IsControlPnl=1";
        string sEncryptedQueryString = CommonUtility.EncryptQuerystring(sBonafideReport);
        var tRow = new TableRow();
        var tCell = new TableCell
        {
            CssClass = "ClsBorderlight",
            ColumnSpan = 1,
            HorizontalAlign = HorizontalAlign.Left
        };

        var ohlinkPageName = new HyperLink
        {
            Text = "Bonafide Certificate",

            NavigateUrl = "../Common/SchoolReportUI.aspx?" + sEncryptedQueryString,
            CssClass = "SubTitleMenu",
            EnableViewState = false
        };
        tCell.Controls.Add(ohlinkPageName);
        tRow.Controls.Add(tCell);
        tblSupervisorMenu.Controls.Add(tRow);
    }

    /// <summary>
    /// This method is used to fill hyperlink in the supervisor table.
    /// </summary>
    /// <param name="aoDataRow"></param>
    /// <param name="tblSupervisorMenu"></param>
    private void FillMenuControls(DataRow aoDataRow, Table tblSupervisorMenu)
    {
        var tRow = new TableRow();
        var tCell = new TableCell
        {
            CssClass = "ClsBorderlight",
            ColumnSpan = 1,
            HorizontalAlign = HorizontalAlign.Left
        };

        var ohlinkPageName = new HyperLink
        {
            Text = aoDataRow["Configure_Name"].ToString(),
            // Replace ~ character to solve menu navigation incorrect path problem
            NavigateUrl = aoDataRow["NavigateURL"].ToString().Replace("~/RITeSchool/Common/", "/RITeSchool/Common/"),
            CssClass = "SubTitleMenu",
            EnableViewState = false
        };

        if ((aoDataRow["Screen_Id"].ToInt() == Constants.SchoolConfigurations.Library.ToInt() || aoDataRow["Screen_Id"].ToInt() == Constants.SchoolConfigurations.LibrariansDesk.ToInt() || aoDataRow["Screen_Id"].ToInt() == Constants.SchoolConfigurations.ReturnRenewBooks.ToInt() || aoDataRow["Screen_Id"].ToInt() == Constants.SchoolConfigurations.BookManagement.ToInt()) && !Settings.ExternalLibrarySite.IsNullOrEmpty())
        {
            ohlinkPageName.NavigateUrl = Settings.ExternalLibrarySite;
            ohlinkPageName.Target = "_blank";
        }
        if (aoDataRow["Configure_Name"].ToString() == "Library")
        {
            tCell.Visible = Settings.EnableLibraryModule;
        }
                
        if (aoDataRow["Configure_Name"].ToString() == "Reports")
        {
            bool bHasAccess = IsReportAvailable();
            ohlinkPageName.Visible = bHasAccess;
            tCell.Controls.Add(ohlinkPageName);
            tCell.Visible = bHasAccess;
        }
        if (aoDataRow["Configure_Name"].ToString() == "Message Center")
        {
            var oImgBtn = new ImageButton();
            oImgBtn.ImageUrl = "~/RITeSchool/images/NewMail_Blink.gif";
            SetNewMessageLink(oImgBtn);
            tCell.Controls.Add(ohlinkPageName);
            tCell.Controls.Add(oImgBtn);
        }
        else if (aoDataRow["Configure_Name"].ToString() == "Staff Birthdays")
        {
            var oImgBtn = new ImageButton();

            oImgBtn.ImageUrl = "~/RITeSchool/images/animated_gift_box3.gif";
            SetStaffBirthdayLink(oImgBtn);
            tCell.Controls.Add(ohlinkPageName);
            tCell.Controls.Add(oImgBtn);
        }
        else
            tCell.Controls.Add(ohlinkPageName);
        if (mbNewVoucherForApproval && aoDataRow["Configure_Name"].ToString() == "Vouchers")
        {
            var imgNewVoucherOther = new HtmlImage();
            imgNewVoucherOther.Src = "../images/document_pending.gif";            
            imgNewVoucherOther.Alt = "New Voucher(s)";
            imgNewVoucherOther.Attributes["title"] = String.Format("{0} New Voucher(s) for Approval", miNewVouchersForApprovalCount);
            imgNewVoucherOther.Attributes["onclick"] = "window.open('../Accounts/VoucherListUI.aspx','_self'); return false;";
            imgNewVoucherOther.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
            tCell.Controls.Add(ohlinkPageName);
            tCell.Controls.Add(imgNewVoucherOther);
        }

        if (aoDataRow["Configure_Name"].ToString() == "New Admissions")
        {
            var spnCount = new System.Web.UI.HtmlControls.HtmlGenericControl("label");
            spnCount.Attributes.Add("class", "clsCount");
            spnCount.Attributes.Add("runat", "server");
            spnCount.Attributes.Add("id", "spnCount1");
            spnCount.Attributes["title"] = "Admission Count";
            spnCount.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
            tCell.Attributes.Add("margin-bottom", "3px");
            tCell.Controls.Add(ohlinkPageName);
            tCell.Controls.Add(spnCount);
            if (miAdmissionCnt > 0)
            {
                spnCount.InnerHtml = miAdmissionCnt.ToString();
                spnCount.Attributes.Add("title", "Admission Count");
                spnCount.Visible = true;
            }
            else
                spnCount.Visible = false;
        }
        else if (aoDataRow["Configure_Name"].ToString() == "Parent Teacher Association")
        {
            string sEncryptedString = CommonUtility.EncryptQuerystring("SchoolCommitteeId=" + Constants.SchoolCommittees.PTA.ToInt());
            ohlinkPageName.NavigateUrl = "../Teacher/ParentTeacherAssociationUI.aspx?" + sEncryptedString;
        }
        else if (aoDataRow["Configure_Name"].ToString() == "Transport Committee")
        {
            string sEncryptedString = CommonUtility.EncryptQuerystring("SchoolCommitteeId=" + Constants.SchoolCommittees.Transport.ToInt());
            ohlinkPageName.NavigateUrl = "../Teacher/ParentTeacherAssociationUI.aspx?" + sEncryptedString;
        }

        if (aoDataRow["Configure_Name"].ToString() == "Requisition")
        {
            var spnCount = new System.Web.UI.HtmlControls.HtmlGenericControl("label");
            spnCount.Attributes.Add("class", "clsCount");
            spnCount.Attributes.Add("runat", "server");
            spnCount.Attributes.Add("id", "spnCount1");
            spnCount.Attributes["title"] = "Waiting Approval Count";
            spnCount.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
            tCell.Attributes.Add("margin-bottom", "3px");
            tCell.Controls.Add(ohlinkPageName);
            tCell.Controls.Add(spnCount);
            if (miRequisitionCnt > 0)
            {
                spnCount.InnerHtml = miRequisitionCnt.ToString();
                spnCount.Attributes.Add("title", "Waiting Approval Count");
                spnCount.Visible = true;
            }
            else
                spnCount.Visible = false;
        }

        tRow.Controls.Add(tCell);
        tblSupervisorMenu.Controls.Add(tRow);
    }

    /// <summary>
    /// This method is used to check whether reports are available to user.
    /// </summary>
    /// <returns></returns>
    private bool IsReportAvailable()
    {

        var oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        DataSet oDsSupervisorrDetail = oSchoolWiseSupervisorMasterBL.GetScreenAccessDetails(miUserId, miUserId, false);
        bool iHasAccess = false;
        for (int iRowindex = 0; iRowindex < oDsSupervisorrDetail.Tables[S_REPORT_FOLDER_DETAILS].Rows.Count; iRowindex++)
        {
            if (oDsSupervisorrDetail.Tables[S_REPORT_FOLDER_DETAILS].Rows[iRowindex][S_HAS_ACCESS].ToString() != Constants.S_YES)
                continue;

            iHasAccess = true;
            break;
        }
        return iHasAccess;
    }

    /// <summary>
    /// This method is used to check whether menu is already present or not.
    /// </summary>
    /// <param name="asMenuName"></param>
    /// <param name="asMenuUrl"></param>
    /// <returns></returns>
    private bool CheckIfMenuAlreadyAvailable(string asMenuName, string asMenuUrl)
    {
        HtmlTable oHtmlTable = null;
        oHtmlTable = Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.C_YES.ToString() ? tblClassTeacher : tblTeacher;
        foreach (HtmlTableRow oHtmlTableRow in oHtmlTable.Rows)
        {
            foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
            {
                foreach (Control oControl in oHtmlTableCell.Controls)
                {
                    if (oControl.IsNull() || !(oControl is HyperLink))
                        continue;

                    var oHyperLink = oControl as HyperLink;
                    if (!oHyperLink.Visible || oHyperLink.Text.ToLower().Trim() != asMenuName.ToLower().Trim())
                        continue;
                    if (oHyperLink.Text.Trim() == Constants.SchoolConfigurations.Library.ToString() && Settings.ExternalLibrarySite != string.Empty)
                        oHyperLink.NavigateUrl = Settings.ExternalLibrarySite;
                    else
                        oHyperLink.NavigateUrl = asMenuUrl.Replace("~/RITeSchool/Common/", "/RITeSchool/Common/"); // Replace ~ character to solve menu navigation incorrect path problem
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// This method is used to show only additional access screen menu.
    /// </summary>
    private void ShowOnlyAdditionalAccessMenu()
    {
        int iScreenLevel = Constants.ScreenLevel.Configuration.ToInt();
        var oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        DataTable oDTMenuDetails = oSchoolWiseSupervisorMasterBL.GetScreenAccessDetails(miUserId, iScreenLevel);
        for (int iCount = 0; iCount < oDTMenuDetails.Rows.Count; iCount++)
            CheckIfMenuAlreadyAvailable(oDTMenuDetails.Rows[iCount]["Configure_Name"].ToString(), oDTMenuDetails.Rows[iCount]["NavigateURL"].ToString());
    }

    /// <summary>
    /// This method is used to check birthday.
    /// </summary>
    private void DisplayBirthdayPopup()
    {
        hidShowPopup.Value = "N";
        hidShowAdmissionPopup.Value = "N";
        string sMessage = string.Empty;
        string sLateMessage = string.Empty;

        if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != "N")
        {
            msDetails = Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString();
            string sIsBirthday = msDetails.Substring(0, 1);
            msDate = msDetails.Substring(2);
            hidShowAdmissionPopup.Value = "Y";
            if (sIsBirthday == "Y")
            {
                hidShowPopup.Value = "Y";
                string sDate = string.Format("{0}-{1}-{2}", msDate.ToDateTime().Month, msDate.ToDateTime().Day, DateTime.Now.Year);
                if (sDate.ToDateTime() < DateTime.Now.Date)
                {
                    sMessage = string.Format("Birthday : {0}<br>", sDate.ToDateTime().ToString("dd MMM"));
                    sLateMessage = Resources.LocalizedResources.Belated;
                }
            }
        }

        if (moUserRole == Constants.UserRoles.Student)
            lblMessage.Text = string.Format("{0}{1}{2} Happy Birthday !!! - From all of us at {3}!!!", sMessage, Resources.LocalizedResources.WishingAnotherWonderfulYearOfHappinessFunAndSuccess, sLateMessage, Convert.ToString(Session[Constants.S_SESSION_SCHOOL_NAME]));
        else
            lblMessage.Text = string.Format("{0}{1}{2} Happy Birthday !!! - From all of us at {3}!!!", sMessage, Resources.LocalizedResources.WishingAnotherWonderfulYearOfHappinessFunAndSuccess, sLateMessage, Convert.ToString(Session[Constants.S_SESSION_SCHOOL_NAME]));
    }

    #region ExpiredSanctionedLeavesPopup

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
                ControlUtility.FillListViewPagerFooter(lstvwStudentSanctionedLeave, DtPgCount);
                SetConfirmationMessage();
                btnSave.Visible = true;
            }
            else
            {
                DtPgCount.Visible = false;
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
                var oCurrentItem = e.Item as ListViewDataItem;
                var otxtStartDate = e.Item.FindControl("txtStartDate") as TextBox;
                var otxtEndDate = e.Item.FindControl("txtEndDate") as TextBox;
                if (otxtStartDate.Text == "01-Jan-0001" || otxtStartDate.Text == "01-Jan-1900")
                    otxtStartDate.Text = string.Empty;
                if (otxtEndDate.Text == "01-Jan-0001" || otxtEndDate.Text == "01-Jan-1900")
                    otxtEndDate.Text = string.Empty;

                DateTime dtStatDate = Convert.ToDateTime(otxtStartDate.Text);
                DateTime dtEndDate = Convert.ToDateTime(otxtEndDate.Text);
                double dtotalLeaveDays = dtEndDate.Subtract(dtStatDate).TotalDays;

                int iMaxLeaveDays = Settings.MaxLeaveDays;
                Session["MaxLeaveDays"] = iMaxLeaveDays.ToString();
                if (dtotalLeaveDays >= iMaxLeaveDays)
                {
                    var tableRow2 = oCurrentItem.FindControl("Tr2") as System.Web.UI.HtmlControls.HtmlTableRow;
                    var tableRow3 = oCurrentItem.FindControl("Tr3") as System.Web.UI.HtmlControls.HtmlTableRow;
                    if (tableRow2 != null)
                        tableRow2.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FFCCCC");
                    if (tableRow3 != null)
                        tableRow3.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "#FFCCCC");
                }
                var oSanctionedLeavesInfo = oCurrentItem.DataItem as StudentSanctionedLeaves;
                if (oSanctionedLeavesInfo.IsCanceled)
                    (e.Item.FindControl("chkIsCanceled") as CheckBox).Checked = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Listview Events"

    #region "Events"

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
            var oDataPager = lstvwStudentSanctionedLeave.FindControl("DtPgDropDown") as DataPager;
            var ddlCnt = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
            hidPageNo.Value = (ddlCnt.SelectedIndex + 1).ToString();
            hidShowAdmissionPopup.Value = "N";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
                lblUpdateSucess.Text = "Leave is sanctioned successfully !!!";
                DisplayControlPanelAccordingToRole();
                hidShowAdmissionPopup.Value = "N";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to set confirmation messaege on change of page.
    /// </summary>
    private void SetConfirmationMessage()
    {
        var oDataPager = lstvwStudentSanctionedLeave.FindControl("DtPgDropDown") as DataPager;
        var ddlCnt = (oDataPager.Controls[0].FindControl("ddlCnt")) as DropDownList;
        ddlCnt.Attributes.Add("onchange", String.Format("if(!MessageAboutDate('{0}')){{return false;}}", ddlCnt.ClientID));
    }

    /// <summary>
    /// This method is used to save sanctioned leave details of students.
    /// </summary>
    private void SaveStudentSanctionedLeaveDetials()
    {
        var oStudentSanctionedLeavesBL = new StudentSanctionedLeavesBL(miSchoolId, miAcademicYearId)
        {
            SanctionedLeavesInfo = PopulateSanctionedLeavesInfo()
        };
        string sXML = GenerateXml(oStudentSanctionedLeavesBL.SanctionedLeavesInfo);
        oStudentSanctionedLeavesBL.SaveOrUpadteStudentSanctionedLeaveDetailsBL(sXML, miUserId);

        SetActivationSMSDetails();
        foreach (UserDetails oUsers in olstUserDetails)
        {
            if (oUsers.IsCanceled == true)
            {
                string sActivationReason = hidSmsTemplate.Value;
                bool bUnlockStudent = UnLockUser(oUsers.UserId);
                if (bUnlockStudent)
                    SendSMS(sActivationReason, oUsers.UserId, oUsers.MobileNumbers, oUsers.UserName);
            }
        }
        lstvwStudentSanctionedLeave.DataSourceID = ObjDSStudentSanctionedLeaves.ID;
        lstvwStudentSanctionedLeave.DataBind();
    }

    /// <summary>
    /// This method is used to populate SanctionedLeavesInfo class.
    /// </summary>
    /// <returns></returns>
    private SanctionedLeavesInfo PopulateSanctionedLeavesInfo()
    {
        var oSanctionedLeavesInfo = new SanctionedLeavesInfo
        {
            lstStudentSanctionedLeaves = FillStudentSanctionedLeavesList(),
            AcademicYearId = miAcademicYearId,
            SchoolId = miSchoolId,
            InsertedById = miUserId,
            UpdatedById = miUserId
        };
        return oSanctionedLeavesInfo;
    }

    /// <summary>
    /// This method is used to fill list of StudentSanctionedLeaves class.
    /// </summary>
    /// <returns></returns>
    private List<StudentSanctionedLeaves> FillStudentSanctionedLeavesList()
    {
        var olstStudentSanctionedLeaves = new List<StudentSanctionedLeaves>();
        for (int iRowId = 0; iRowId < lstvwStudentSanctionedLeave.Items.Count; iRowId++)
        {
            if ((lstvwStudentSanctionedLeave.Items[iRowId].FindControl("txtStartDate") as TextBox).Text == string.Empty)
                continue;

            olstStudentSanctionedLeaves.Add(new StudentSanctionedLeaves
            {
                SanctionedLeaveDetailsId = lstvwStudentSanctionedLeave.DataKeys[iRowId]["SanctionedLeaveDetailsId"].ToInt(),
                StudentId = lstvwStudentSanctionedLeave.DataKeys[iRowId]["StudentId"].ToInt(),
                UserId = lstvwStudentSanctionedLeave.DataKeys[iRowId]["UserId"].ToInt(),
                StartDate = (lstvwStudentSanctionedLeave.Items[iRowId].FindControl("txtStartDate") as TextBox).Text.ToDateTime(),
                EndDate = (lstvwStudentSanctionedLeave.Items[iRowId].FindControl("txtEndDate") as TextBox).Text.ToDateTime(),
                IsCanceled = (lstvwStudentSanctionedLeave.Items[iRowId].FindControl("chkIsCanceled") as CheckBox).Checked
            });

            if ((lstvwStudentSanctionedLeave.Items[iRowId].FindControl("chkIsCanceled") as CheckBox).Checked == true)
            {
                oUserDetails.UserId = (lstvwStudentSanctionedLeave.DataKeys[iRowId]["UserId"]).ToInt();
                oUserDetails.UserName = (lstvwStudentSanctionedLeave.Items[iRowId].FindControl("lblName") as Label).Text;
                oUserDetails.MobileNumbers = (lstvwStudentSanctionedLeave.Items[iRowId].FindControl("lblMobileNo") as Label).Text;
                oUserDetails.IsCanceled = (lstvwStudentSanctionedLeave.Items[iRowId].FindControl("chkIsCanceled") as CheckBox).Checked;
                olstUserDetails.Add(oUserDetails);
            }
        }
        return olstStudentSanctionedLeaves;
    }

    /// <summary>
    /// This method is used to display expired sanctioned leaves pop up.
    /// </summary>
    private void DisplayExpiredSanctionedLeavesPopup()
    {
        if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != "N")
        {
            valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            var oStudentSanctionedLeavesBL = new StudentSanctionedLeavesBL();
            if (oStudentSanctionedLeavesBL.CanSanctionLeave(miUserId) && oStudentSanctionedLeavesBL.CountTotalExpiredSanctionedLeaves(miSchoolId, miAcademicYearId, miUserId, 0, 0) > 0)
                hidShowExpiredSanctionedLeavesPopup.Value = "Y";
            else
                hidShowExpiredSanctionedLeavesPopup.Value = "N";
        }
    }

    /// <summary>
    /// This method set default activation sms in text box.
    /// </summary>
    private void SetActivationSMSDetails()
    {
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.UserActivationSMS);
        DataTable oDTTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTTemplate.Rows.Count != I_ZERO)
        {

            if (oDTTemplate.Rows[I_ZERO][I_TWO] != DBNull.Value)
            {
                hidSmsTemplate.Value = Convert.ToString(oDTTemplate.Rows[I_ZERO][I_TWO]);
                HidSMSTemplateName.Value = Convert.ToString(oDTTemplate.Rows[I_ZERO][I_ONE]);

            }

        }
    }

    /// <summary>
    /// To Unlock User while Sanction leave used.
    /// </summary>
    /// <param name="iUserId"></param>
    /// <returns></returns>
    private bool UnLockUser(int iUserId)
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        oSchoolUserBL.UnLockParticularUser(iUserId, miSchoolId, miUserId, I_ONE);
        return true;
    }


    //Send sms to the students whose Login is Deactivated because of Long Leave
    private void SendSMS(string sSmsText, int iUserId, string asMobileNumber, string asUserName)
    {
        Hashtable oHTUsersMobileNo = new Hashtable();

        string[] sArrMobileNumber;
        sArrMobileNumber = asMobileNumber.Split(',');
        oHTUsersMobileNo[iUserId] = sArrMobileNumber[0].Trim(); ;
        string sTemplateRegistrationId = string.Empty; 

        if (sArrMobileNumber.Length > Constants.I_ONE && !sArrMobileNumber[1].Trim().IsNullOrEmpty() && sArrMobileNumber[0].Trim() != sArrMobileNumber[1].Trim())
            oHTUsersMobileNo[iUserId + "sm;"] = sArrMobileNumber[1].Trim();

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
    /// This is used to handle the command.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAttendanceDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DETAILS")
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                if (oCurrentItem != null)
                {
                    int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                    HideOtherViews(iRowId);
                    int StandardDivisionId = Convert.ToInt32(lstvwAttendanceDetails.DataKeys[iRowId]["StandardDivisionId"]);
                    ListViewDataItem oItem = lstvwAttendanceDetails.Items[iRowId] as ListViewDataItem;
                    HtmlTableRow oHtmlTableRow = e.Item.FindControl("trDateDetails") as HtmlTableRow;
                    if (oHtmlTableRow != null)
                    {
                        HtmlTableCell oHtmlTableCell = oHtmlTableRow.FindControl("tdDateDetails") as HtmlTableCell;
                        ListView olstvwDateDetails = oHtmlTableCell.FindControl("lstvwDateDetails") as ListView;
                        AttendanceAlertConfigBL oAttendanceAlertConfigBL = new AttendanceAlertConfigBL(miSchoolId, miAcademicYearId);
                        List<DateTime> olstDates = oAttendanceAlertConfigBL.GetMissingAttendanceDates(StandardDivisionId, miUserId);
                        oHtmlTableRow.Visible = true;
                        olstvwDateDetails.DataSource = olstDates.Select(dt => dt.Date);

                        olstvwDateDetails.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is used to laod date details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDateDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                Label lblDate = e.Item.FindControl("lblDate") as Label;
                ListViewDataItem oListViewDataItem = e.Item as ListViewDataItem;
                lblDate.Text = oListViewDataItem.DataItem.ToDateTime().ToString("dd-MMM-yyyy");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This evetn is used to set attributes on link button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAttendanceDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                Button BtnCancelDates = ((Button)oCurrentItem.FindControl("BtnCancelDates"));
                ApplyMouseHoverEffect(new List<Button> { BtnCancelDates });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This evetn is used to set list view item data Bound event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwNonPermanantTeachers_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                NonPermanentTeacherDetails oNonPermanentTeacherDetails = e.Item.DataItem as NonPermanentTeacherDetails;
                Label lblJoiningDate = e.Item.FindControl("lblJoiningDate") as Label;

                lblJoiningDate.Text = oNonPermanentTeacherDetails.JoiningDate.ToString(Constants.S_DATE_FORMAT);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to hide all the visible listview except selected.
    /// </summary>
    /// <param name="aiRowId"></param>
    private void HideOtherViews(int aiRowId)
    {
        int iItemCount = lstvwAttendanceDetails.Items.Count;
        int iRowId;
        foreach (ListViewDataItem oCurrentItem in lstvwAttendanceDetails.Items)
        {
            iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
            if (iRowId != aiRowId)
            {
                System.Web.UI.HtmlControls.HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("trDateDetails") as System.Web.UI.HtmlControls.HtmlTableRow;
                oHtmlTableRow.Visible = false;
            }
        }
    }

    /// <summary>
    /// This event is used to close the users renew/return sub grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancelDates_Click(object sender, EventArgs e)
    {
        try
        {
            Button oButton = sender as Button;
            ListView olstvwDateDetails = oButton.Parent.FindControl("lstvwDateDetails") as ListView;
            oButton.Parent.Parent.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Private Methods"

    #endregion

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnClosePopUp, btnClose, btnSave, btnCloseAttendance, btnCloseClassDiv });
        btnClosePopUp.Attributes["OnClick"] = "javascript:HidePopup();return false;";
        btnClose.Attributes["OnClick"] = "javascript:HidePopup();return false;";
        btnSave.Attributes["onclick"] = "javascript:btnsaveonclick('" + btnSave.ClientID + "');";

        if (Settings.EnabledOnlineFee && moUserRole == Constants.UserRoles.Student)
        {
            lnkVideo.Visible = true;
            lnkVideo.CssClass = "class1 fee-video-img-spacing";
            lnkVideo.Attributes.Add("onclick", "ShowVideo('" + hidFeeVideolinkurl.Value + "'); return false;");
        }
    }

    /// <summary>
    /// This method is used to display supervisor details table into tblSuperRight table.
    /// </summary>
    private void FillSupervisorDetails()
    {
        var tRow = new TableRow();
        var tCell = new TableCell
        {
            ColumnSpan = 1,
            HorizontalAlign = HorizontalAlign.Left
        };
        //tCell.Controls.Add(tblNoticeMessage);
        //tRow.Controls.Add(tCell);
        //tblSuperRight.Controls.Add(tRow);

        tRow = new TableRow();
        tCell = new TableCell
        {
            ColumnSpan = 1,
            HorizontalAlign = HorizontalAlign.Left
        };
        tCell.Controls.Add(tblSepervisorDetails);
        tRow.Controls.Add(tCell);
        //tblSuperRight.Controls.Add(tRow);

    }

    /// <summary>
    /// This method is used to get supervisor access pages.
    /// </summary>
    /// <returns></returns>
    private DataTable GetSupervisorAccessPages()
    {
        var oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        DataTable oDTSupAccessPages = oSchoolWiseSupervisorMasterBL.GetSupervisorAccessPages(miUserId);
        return oDTSupAccessPages;
    }

    /// <summary>
    /// This method is used to set PrePrimary teacher link.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private void SetPrePrimaryTecherLinks(DataTable aoDataTable)
    {
        if ((aoDataTable == null) || aoDataTable.Rows.Count <= 0)
            return;

        if (aoDataTable.Rows[0]["Is_PrePrimary"] == DBNull.Value)
            return;

        if (Convert.ToChar(aoDataTable.Rows[0]["Is_PrePrimary"]) == Constants.C_YES)
        {
            if (Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.C_YES.ToString())
            {
                if (Settings.IsTimeTableForPrePrimaryClassTeacher)
                {
                    trTimetable.Visible = true;
                    trWeeklyTimetable.Visible = true;
                }
                else
                {
                    trTimetable.Visible = false;
                    trWeeklyTimetable.Visible = false;
                }
            }

            TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
            trFinalResults.Visible = trFinalResults.Visible = !oTeacherStandardDetailsBL.IsPreprimaryExamConfiguration(miSchoolId, miAcademicYearId, Session[Constants.S_SESSION_TEACHER_ID].ToInt(), ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]).ToString());
            Constants.SchoolId oSchoolId = (Constants.SchoolId)miSchoolId;
            trTeacherExamSchedule.Visible = Settings.IsExamScheduleForPrePrimaryClassTeacher;
        }
        else
        {
            trFinalResults.Visible = true;
            trExamResults.Visible = true;
            trTeacherExamSchedule.Visible = true;
        }              
    }

    /// <summary>
    /// This method is used to retrive Teachers information from database and set it to controls.
    /// </summary>
    /// <returns></returns>
    private DataSet DisplayTeacherDetails()
    {
        
        DataSet oDSTeacherDetails = SchoolWiseTeacherMasterBL.GetTeacherDetailsForControlPanel(Session[Constants.S_SESSION_TEACHER_ID].ToInt(), miSchoolId, miAcademicYearId);
        DataTable oDTTeacherDetails = oDSTeacherDetails.Tables[0];
        lblTeacherName.Text = oDTTeacherDetails.Rows[0]["TeacherName"].ToString();
        Session[Constants.S_SESSION_USER_FULLNAME] = oDTTeacherDetails.Rows[0]["TeacherName"].ToString();
        lblDesignation.Text = oDTTeacherDetails.Rows[0]["Teacher_Designation_Name"].ToString();
        lblClassDiv.Text = oDTTeacherDetails.Rows[0]["TeacherStdDiv"].ToString();

        (this.Master.FindControl("divSchoolNoticeBoard")).Visible = true;
        (this.Master.FindControl("LabelNoticeBoardMsg") as Label).Text = ShowNoticeBoardMessage(oDSTeacherDetails.Tables[3]);

        if (Convert.ToInt32(oDTTeacherDetails.Rows[0]["Teacher_Designation_Id"]) == Convert.ToInt32(Constants.S_PRINCIPAL_DESIGNATION_ID)) // principal designation id
        {
            //Table2.Visible = false;
            SchoolNoticeDiv.Visible = true;
            hidPrincipalDesignationId.Value = Constants.S_PRINCIPAL_DESIGNATION_ID;
            //Table2.Visible = false;
        }

        for (int iClassCount = 1; iClassCount < oDTTeacherDetails.Rows.Count; iClassCount++)
            lblClassDiv.Text = lblClassDiv.Text + ", " + oDTTeacherDetails.Rows[iClassCount]["TeacherStdDiv"].ToString();

        if (String.IsNullOrEmpty(lblClassDiv.Text))
            lblClassDiv.Text = "-";

        lblQualification.Text = oDTTeacherDetails.Rows[0]["qualification"].ToString();

        //lblNoticeBoardMsg.Text = ShowNoticeBoardMessage(oDSTeacherDetails.Tables[3]);
        (this.Master.FindControl("divSchoolNoticeBoard")).Visible = true;
        (this.Master.FindControl("LabelNoticeBoardMsg") as Label).Text = ShowNoticeBoardMessage(oDSTeacherDetails.Tables[3]);        

        return oDSTeacherDetails;        
    }

    /// <summary>
    /// This method is used to retrive Supervisor information from database and set it to controls.
    /// </summary>
    /// <returns></returns>
    private DataSet DisplaySupervisorDetails()
    {
        var oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        DataSet oDSSupervisorDetails = oSchoolWiseSupervisorMasterBL.GetSupervisorDetailsForControlPanel(miUserId, miSchoolId, miAcademicYearId);
        DataTable oDTSupervisorDetails = oDSSupervisorDetails.Tables[0];

        string sSupervisorRoleName = Session[Constants.S_SESSION_SUPERVISOR_ROLE_NAME_FIELD].ToString();

        lblSupervisorDetailsField.Text = sSupervisorRoleName + " Details";

        lblSupervisorName.Text = oDTSupervisorDetails.Rows[0]["SupervisorName"].ToString();
        Session[Constants.S_SESSION_USER_FULLNAME] = oDTSupervisorDetails.Rows[0]["SupervisorName"].ToString();
        lblSupervisorDesignation.Text = oDTSupervisorDetails.Rows[0]["Designation"].ToString();
        msSupervisorDesignationName = oDTSupervisorDetails.Rows[0]["Designation"].ToString();
        hidSupervisorDesignationName.Value = oDTSupervisorDetails.Rows[0]["Designation"].ToString();

        //lblNoticeBoardMsg.Text = ShowNoticeBoardMessage(oDSSupervisorDetails.Tables[3]);
        (this.Master.FindControl("divSchoolNoticeBoard")).Visible = true;
        (this.Master.FindControl("LabelNoticeBoardMsg") as Label).Text = ShowNoticeBoardMessage(oDSSupervisorDetails.Tables[3]);

        return oDSSupervisorDetails;
    }

    /// <summary>
    /// This method is used to retrive Supervisor information from database and set it to controls.
    /// </summary>
    /// <returns></returns>
    private DataSet DisplayOtherStaffDetails()
    {
        //var oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        OtherStaffBL oOtherStaffBL = new OtherStaffBL();
        DataSet oDSOtherStaffDetails = oOtherStaffBL.GetOtherStaffDetailsForControlPanel(miUserId, miSchoolId, miAcademicYearId);
        DataTable oDTOtherStaffDetails = oDSOtherStaffDetails.Tables[0];

        string sSupervisorRoleName = Session[Constants.S_SESSION_SUPERVISOR_ROLE_NAME_FIELD].ToString();

        lblSupervisorDetailsField.Text = sSupervisorRoleName + " Details";

        lblSupervisorName.Text = oDTOtherStaffDetails.Rows[0]["OtherStaffName"].ToString();
        Session[Constants.S_SESSION_USER_FULLNAME] = oDTOtherStaffDetails.Rows[0]["OtherStaffName"].ToString();
        lblSupervisorDesignation.Text = oDTOtherStaffDetails.Rows[0]["Designation"].ToString();
        lblSuperwiserMob.Text = oDTOtherStaffDetails.Rows[0]["MobileNo"].ToString();

        //lblNoticeBoardMsg.Text = ShowNoticeBoardMessage(oDSOtherStaffDetails.Tables[3]);
        (this.Master.FindControl("divSchoolNoticeBoard")).Visible = true;
        (this.Master.FindControl("LabelNoticeBoardMsg") as Label).Text = ShowNoticeBoardMessage(oDSOtherStaffDetails.Tables[3]);

        return oDSOtherStaffDetails;
    }

    /// <summary>
    /// This method is used to set New message link on control panel
    /// to display count of new messages in inbox.
    /// </summary>
    private void SetNewMessageLink(ImageButton aoImgBtn)
    {
        var oMessageReceiverDetailsCollectionBL = new MessageReceiverDetailsCollectionBL();
        int iNewMessageCount = oMessageReceiverDetailsCollectionBL.GetCountOfNewMessageForUser(miUserId, miAcademicYearId);
        SetNewMessageLink(aoImgBtn, iNewMessageCount);
    }

    /// <summary>
    /// This method is used to get satff birthday count.
    /// </summary>
    /// <param name="aoImgBtn"></param>
    private void SetStaffBirthdayLink(ImageButton aoImgBtn)
    {
        var oSchooluserBL = new SchoolUserBL();
        int iTotalBirthdayCount = oSchooluserBL.GetCountOfSchoolStaffBirthDay(miSchoolId, miAcademicYearId);
        SetStaffBirthdayLink(aoImgBtn, iTotalBirthdayCount);

    }
    /// <summary>
    /// This method is used to set New message link on control panel
    /// to display count of new messages in inbox.
    /// </summary>
    private void SetNewMessageLink(ImageButton aoImgBtn, int aiCount)
    {
        if (aoImgBtn == null)
            return;

        int iNewMessageCount = aiCount;
        if (iNewMessageCount > 0)
        {
            aoImgBtn.Visible = true;
            aoImgBtn.Attributes.Add("title", String.Format("{0} {1}", iNewMessageCount, iNewMessageCount == 1 ? Resources.LocalizedResources.NewMessage : Resources.LocalizedResources.NewMessages));
            aoImgBtn.Attributes.Add("onclick", "window.open('../Common/MessageInbox.aspx','_self');return false;");
            aoImgBtn.ID = "imgBtnMsgAlertAdminStaff";
        }
        else
            aoImgBtn.Visible = false;
    }

    /// <summary>
    /// This method is used to set New message link on control panel
    /// to display count of new messages in inbox.
    /// </summary>
    private void SetStaffBirthdayLink(ImageButton aoImgBtn, int aiCount)
    {
        if (aoImgBtn == null)
            return;

        if (aiCount > 0)
        {
            aoImgBtn.Visible = true;
            aoImgBtn.Attributes.Add("title", String.Format("{0} {1}", aiCount, aiCount == 1 ? Resources.LocalizedResources.StaffBirthday : Resources.LocalizedResources.StaffBirthdays));
            aoImgBtn.Attributes.Add("onclick", "window.open('../Common/StaffBirthDay.aspx','_self');return false;");
            aoImgBtn.ID = "imgBtnBirthdayAlert";
        }
        else
            aoImgBtn.Visible = false;
    }
    /// <summary>
    /// This method is used to encrypt queryString.
    /// </summary>
    /// <param name="aiStandardId"></param>
    /// <returns></returns>
    private string GetEncryptedStandardQueryString(int aiStandardId)
    {
        string sQuerystring = "StandardId=" + aiStandardId;
        string sEncryptedString = "~/RITeSchool/Student/StandardwiseExamScheduleList.aspx?" + CommonUtility.EncryptQuerystring(sQuerystring);
        return sEncryptedString;
    }

    /// <summary>
    /// Determines if there are new vouchers for approval for the current user, based on the DesignationId.
    /// </summary>
    private void CheckIfNewVouchersForApproval()
    {
        if (IsAccountsModuleEnabled)
        {
            AccountVoucherClient oAccountVoucherClient = null;
            try
            {
                oAccountVoucherClient = new AccountVoucherClient();
                oAccountVoucherClient.Open();

                miNewVouchersForApprovalCount = oAccountVoucherClient.GetVoucherCountForApproval(miSchoolId, miFinancialYearId, miUserId);
                mbNewVoucherForApproval = miNewVouchersForApprovalCount > 0;
                imgNewVoucherAdmin.Visible = mbNewVoucherForApproval;
                if (mbNewVoucherForApproval)
                {
                    imgNewVoucherAdmin.Attributes["title"] = String.Format("{0} New Voucher(s) for Approval", miNewVouchersForApprovalCount);
                    imgNewVoucherAdmin.Attributes["onclick"] = "window.open('../Accounts/VoucherListUI.aspx','_self'); return false;";
                    imgNewVoucherAdmin.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Accounts Module : There was an error checking for new vouchers for approval.");
            }
            finally
            {
                if (oAccountVoucherClient != null && oAccountVoucherClient.State != CommunicationState.Faulted)
                    oAccountVoucherClient.Close();
            }
        }
    }

    /// <summary>
    /// This method is used to get and display new admission count in mid  year.
    /// </summary>
    private void DisplayNewAdmissionCount()
    {
        if (miAdmissionCnt > 0)
        {
            spnCount.InnerHtml = miAdmissionCnt.ToString();
            spnCount.Attributes.Add("title", "Admission Count");
            spnCount.Visible = true;
        }
        else
            spnCount.Visible = false;
    }

    /// <summary>
    /// This method is used to get and display requisition count.
    /// </summary>
    private void DisplayRequisitionCount()
    {
        if (moUserRole == Constants.UserRoles.Teacher)
        {
            if (miRequisitionCnt > 0)
            {
                spnRequisitionCount.InnerHtml = miRequisitionCnt.ToString();
                spnRequisitionCount.Attributes.Add("title", "Waiting Approval Count");
                spnRequisitionCount.Visible = true;
            }
            else
            {
                spnRequisitionCount.Visible = false;
            }
        }
        if (moUserRole == Constants.UserRoles.Admin)
        {
            if (miRequisitionCnt > 0)
            {
                spnRequisitionCountForAdmin.InnerHtml = miRequisitionCnt.ToString();
                spnRequisitionCountForAdmin.Attributes.Add("title", "Waiting Approval Count");
                spnRequisitionCountForAdmin.Visible = true;
            }
            else
            {
                spnRequisitionCountForAdmin.Visible = false;
            }
        }
    }

    /// <summary>
    /// This used to dynamicall generate Missing Attendance Link
    /// </summary>
    private void MissingAttendanceLink(Table aoMenuTable)
    {
        HtmlAnchor link = new HtmlAnchor();
        link.InnerText = Resources.LocalizedResources.MissingAttendance;
        link.Attributes.Add("class", "SubTitleMenu");
        link.Attributes.Add("onclick", "ShowAttendanceAlertPopup()");
        link.Style.Add("cursor", "pointer");
        var tRow = new TableRow();
        var tCell = new TableCell
        {
            CssClass = "ClsBorderlight",
            ColumnSpan = 1,
            HorizontalAlign = HorizontalAlign.Left
        };
        tCell.Controls.Add(link);
        tRow.Controls.Add(tCell);
        aoMenuTable.Controls.Add(tRow);
    }


    /// <summary>
    /// this is used to generate bita URL.
    /// </summary>
    private void ShowBitaURLLinkForAdminStaff(Table aoMenuTable)
    {
        if (!String.IsNullOrEmpty(Settings.BetaVersionURL))
        {
            HtmlAnchor link = new HtmlAnchor();
            link.InnerText = "Beta Version";
            link.Attributes.Add("class", "SubTitleMenu");

            Label lblNew = new Label();
            lblNew.Text = " NEW";
            lblNew.CssClass = "menu-new-badge";

            link.HRef = Settings.BetaVersionURL + CommonUtility.EncryptQuerystring("SchoolId=" + miSchoolId + "&UserId=" + miUserId).Replace("+", "%20").Replace("/", "%2F");
            link.Target = "_blank";
            link.Style.Add("cursor", "pointer");

            Image imgNew = new Image();
            imgNew.ImageUrl = "~/images/newLink.gif";
            imgNew.AlternateText = "NEW";
            imgNew.Style.Add("margin-left", "5px");
            imgNew.Style.Add("vertical-align", "middle");

            var tRow = new TableRow();
            var tCell = new TableCell
            {
                CssClass = "ClsBorderlight",
                ColumnSpan = 1,
                HorizontalAlign = HorizontalAlign.Left
            };

            tCell.Controls.Add(link);
            tCell.Controls.Add(imgNew);

            tRow.Controls.Add(tCell);

            if (aoMenuTable.Controls.Count > 2)
                aoMenuTable.Controls.AddAt(2, tRow);
            else
                aoMenuTable.Controls.Add(tRow);
        }
    }

     /// <summary>
    /// This used to dynamicall generate Absent Student Details Link.
    /// </summary>
    private void AbsentStudentDetailsLink(Table aoMenuTable)
    {             
        HtmlAnchor link = new HtmlAnchor();        
        link.InnerText = "Absent Student Details";
        link.Attributes.Add("class", "SubTitleMenu");
        link.Attributes.Add("onclick", "ShowAbsentStudentPopup()");
        link.Style.Add("cursor", "pointer");
        
        var tRow = new TableRow();
        tRow.ID = "trAbsentNotification";
        tRow.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
        var tCell = new TableCell
        {
            CssClass = "ClsBorderlight",
            ColumnSpan = 1,
            HorizontalAlign = HorizontalAlign.Left
        };
        tCell.Controls.Add(link);
        tRow.Controls.Add(tCell);
        aoMenuTable.Controls.Add(tRow);
    }

    /// <summary>
    /// This method is used to make visible the Missinattendance link
    /// </summary>
    private void MissingAttendancelinkVissible()
    {        
        HidAttendanceAlert.Value = Constants.S_NO;

        if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] == null || Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO)
        {
            AttendanceAlertConfigBL oAttendanceAlertConfigBL = new AttendanceAlertConfigBL(miSchoolId, miAcademicYearId);
            List<AttendanceAlertDetails> olstAttendanceAlertDetails = oAttendanceAlertConfigBL.GetMissingAttendanceDetailsForUser(miUserId, Constants.I_ZERO);
            if (olstAttendanceAlertDetails.Count > Constants.I_ZERO)
            {
                HidAttendanceAlert.Value = Constants.S_YES;
                TrteacherMissingAttendsAlert.Visible = true;
                trAdminMissingAttendance.Visible = true;
                TrclassteacherMissingAttendsAlert.Visible = true;

                lstvwAttendanceDetails.DataSource = olstAttendanceAlertDetails;
                lstvwAttendanceDetails.DataBind();

                if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO)
                    HidAttendanceAlertFirstTime.Value = Constants.S_YES;
            }
            Session[Constants.S_SESSION_IS_FIRST_LOGIN] = Constants.S_NO;
        }
    }

    /// <summary>
    /// This method is used to make visible the Absent student details popup.
    /// </summary>
    private void AbsentStudentDetails()
    {
        hidShowAttendanceDiv.Value = Constants.S_NO;
        if (Settings.StudentAbsentCount > Constants.I_ZERO)
         {
             mlstAbsentStudentDetails = new List<SchoolEntities.Admin.AbsentStudentDetails>();             
             AttendanceAlertConfigBL oAttendanceAlertConfigBL = new AttendanceAlertConfigBL(miSchoolId, miAcademicYearId);
             mlstAbsentStudentDetails = oAttendanceAlertConfigBL.GetAbsentStudentDetailsForPopup(miUserId, out ISABSENT_STUDENT__LINK_VISIBEL);

             if (mlstAbsentStudentDetails.Count > Constants.I_ZERO )
             {
                 lblAbsentHeader.Text = "This is the absent students list who is absent from last " + Settings.StudentAbsentCount + " working days.";
                 trMissingAttendance.Visible = true;
                 TrAbsentStudentPopup.Visible = true;
                 TrClassTeacherAbsentStudents.Visible = true;
                 lstvwMissingAttendance.DataSource = mlstAbsentStudentDetails;
                 lstvwMissingAttendance.DataBind();

                 if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO)
                 {
                     hidShowAttendanceDiv.Value = Constants.S_YES;
                 }
             }
            
         }
         else
         {
             trMissingAttendance.Visible = false;
             divMissingAttendancePopup.Visible = false;
             TrAbsentStudentPopup.Visible = false;
             TrClassTeacherAbsentStudents.Visible = false;
             //trMissingAttendancePoppup.Visible = false;
         }
    }

    /// <summary>
    /// This method is used to set display NonPermenant Teacher Details.
    /// </summary>
    private void DisplayNonPermanantTeacherDetails()
    {        
        hlnkTeacherDetails.Attributes.Add("onclick", "ShowTeacherAlertPopup()");
        AttendanceAlertConfigBL oAttendanceAlertConfigBL = new AttendanceAlertConfigBL(miSchoolId, miAcademicYearId);
        List<NonPermanentTeacherDetails> olstNonPermantTeacherDetails = oAttendanceAlertConfigBL.GetNonPermanentTeacherDetails();

        if (olstNonPermantTeacherDetails.Count > Constants.I_ZERO)
        {
            lstvwNonPermanantTeachers.DataSource = olstNonPermantTeacherDetails;
            lstvwNonPermanantTeachers.DataBind();
        }
    }

    /// <summary>
    /// This method is used to set design according to the language selected.
    /// </summary>
    private void DesignSettingAccordinglanguage()
    {
        hidEndDateRequiredForRrow.Value = Resources.LocalizedResources.EndDateRequiredForRrow;
        hidStartDateRequiredForRow.Value = Resources.LocalizedResources.StartDateRequiredForRow;
        hidIfYouChangeThePageThenSelectedSanctioned.Value = Resources.LocalizedResources.IfYouChangeThePageThenSelectedSanctioned;
        hidAPopupBlockerIsDetected.Value = Resources.LocalizedResources.APopupBlockerIsDetected;
    }

    /// <summary>
    /// This method is used to open PTA screen.
    /// </summary>
    /// <param name="aoCommittee"></param>
    private string GetCommitteeScreenURL(Constants.SchoolCommittees aoCommittee)
    {
        string sEncryptedString = CommonUtility.EncryptQuerystring("SchoolCommitteeId=" + aoCommittee.ToInt());
        return "../Teacher/ParentTeacherAssociationUI.aspx?" + sEncryptedString;
    }

    /// <summary>
    /// This method is used to display class wise student count.
    /// </summary>
    private void ShowClasswiseStudentCount()
    {
        hidShowClassDiv.Value = Constants.S_NO;
        if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] != null && Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO)
        {
            StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId);
            List<StudentStrengthDetails> lstClasses = oStandardCollectionBL.GetClasseswiseStudentCountDetails(miSchoolId, miAcademicYearId, miUserId);

            lstvwClasses.DataSource = lstClasses;
            lstvwClasses.DataBind();

            if (lstClasses.Count > 0)
                hidShowClassDiv.Value = Constants.S_YES;
        }
    }

    protected void lstvwClasses_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {   
            Label lblClassName = e.Item.FindControl("lblClassName") as Label;
            Label lblStudentCount = e.Item.FindControl("lblStudentCount") as Label;
            Label lblStrength = e.Item.FindControl("lblStrength") as Label;
            if (Convert.ToBoolean(lstvwClasses.DataKeys[e.Item.DisplayIndex]["IsExceeded"]))
            {
                lblClassName.ForeColor = System.Drawing.Color.Red;
                lblStudentCount.ForeColor = System.Drawing.Color.Red;
                lblStrength.ForeColor = System.Drawing.Color.Red;
            }                
        }
    }

    /// <summary>
    /// This method is used to fill standard in combobox.
    /// </summary>
    private void FillStandardCombobox()
    {
        YearWIseStudentsBL oYearWiseSTudentInfoBL = new YearWIseStudentsBL();
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);

        string topStandardElement = string.Empty;

        List<StandardMaster> oDSStandardCollection = oStandardCollectionBL.GetExamConfiguredStandards();

         topStandardElement = oDSStandardCollection.Count > 0 ? string.Empty : Constants.S_SELECT;

         ListSource.FillDropDownList(oDSStandardCollection, cmbStandardName, "StandardName", "StandardId", topStandardElement);

        FillTestCombobox(cmbStandardWiseExam, Convert.ToInt32(cmbStandardName.SelectedItem.Value));
    }

    /// <summary>
    /// This method is used to fill exam dropdown for student performance widget.
    /// </summary>
    /// <param name="dropdown"></param>
    /// <param name="aiStandardId"></param>
    private void FillTestCombobox(DropDownList dropdown, int aiStandardId = 0)
    {
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
        /* if standard id will be available then return only specific exams  otherwise return all exams specific to school*/
        using (DataTable oDsAllTests = (aiStandardId != Constants.I_ZERO) ? oTestCollectionBL.GetAllTestsForStandard(aiStandardId) : oTestCollectionBL.GetAllTestsForSchool())
        {
            string topElement = string.Empty;
            if (aiStandardId == Constants.I_ZERO)
                topElement = oDsAllTests.Rows.Count > 0 ? string.Empty : Constants.S_SELECT;

            ControlUtility.FillDropDownList(
                oDsAllTests,
                ref dropdown,
                Constants.S_TEST_ID_FIELD,
                Constants.S_TEST_NAME_FIELD,
                topElement);

            //Select by default value as latest exam value.
            //if (aiStandardId == Constants.I_ZERO)
            //    cmbExam.SelectedValue = SchoolWiseTestMasterBL.GetLatestExamId(miSchoolId, miAcademicYearId, 0, 0).ToString();
        }
    }

    /// <summary>
    /// This function is used to set Unread Question Count of User.
    /// </summary>
    private void GetUnreadQuestionCount(string sUserRole)
    {
        AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL();
        int iCount = oAskMeQuestionMasterBL.GetCountOfUnreadQuestion(miUserId);
        
        if (sUserRole == Constants.UserRoles.Teacher.ToString())
        {
            if (iCount > 0)
            {
                if (trAskMeClassTeacher.Visible == true)
                {
                    lblClassTeacherQueCnt.Visible = true;
                    lblClassTeacherQueCnt.Text = Convert.ToString(iCount);
                    lblClassTeacherQueCnt.ToolTip = Convert.ToString(iCount) + " Unread Query(s)";
                }
                else
                {
                    lblUnreadCount.Visible = true;
                    lblUnreadCount.Text = Convert.ToString(iCount);
                    lblUnreadCount.ToolTip = Convert.ToString(iCount) + " Unread Query(s)";
                }
            }
            else
                lblUnreadCount.Visible = false;
        }
        else if (sUserRole == Constants.UserRoles.Student.ToString())
        {
            if (iCount > 0)
            {
                lblStudUnreadCount.Visible = true;
                lblStudUnreadCount.Text = Convert.ToString(iCount);
                lblStudUnreadCount.ToolTip = Convert.ToString(iCount) + " Unread Query(s)";
            }
            else
                lblStudUnreadCount.Visible = false;
        }
    }

   
    private void PaymentClearanceNotification()
    {
       // hlkPaymentNotification.Attributes.Add("onclick", "ShowClearanceNotification()");

        if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] == null || Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO)
        {
            ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();

            if ((moUserRole == Constants.UserRoles.Admin && lstUsers.Any(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowPaymentClearanceNotification.ToInt())) || lstUsers.Any(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowPaymentClearanceNotification.ToInt() && ru.UserId == miUserId))
            {
                //trPaymentNotificationClearance.Visible = true;

                StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                DataTable dt = oStudentFeeDetailsBL.GetPaymentClearanceNotification(miSchoolId, miAcademicYearId);
                lstpaymentNotification.DataSource = dt;
                lstpaymentNotification.DataBind();
                trPaymentClearance.Visible = dt.Rows.Count > 0;
            }
        }
    }

    /// <summary>
    /// This method is used to check the Login User is configured for getting the NonPermenant teacher details.
    /// </summary>
    private bool CheckLoginUser(bool abValue)
    {
        bool bValue = false;
        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
        if (moUserRole == Constants.UserRoles.Admin || lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.JobPeriodNotification.ToInt() && ru.UserId == miUserId).Any())
        {
            bValue = true;
            trNonPermenantTeachers.Visible = true;
            DisplayNonPermanantTeacherDetails();

            if (abValue == true)
            {
                trclassTeacherNonPermenant.Visible = true;
                trNonPermenantTeacherlink.Visible = true;
            }
        }
        else
            trNonPermenantTeachers.Visible = false;

        return bValue;
    }

    /// <summary>
    /// This Method is used to Replace The Xseed To Preprimary Details.
    /// </summary>
    private void ReplaceXseedToPrePrimary()
    {
        //if (miSchoolId == Constants.SchoolId.PPS.ToInt()
        //   || (miSchoolId == Constants.SchoolId.BMFS.ToInt()))
        //{

            lblXseed.Text = "Pre-Primary";
            hlkAssignXseedGrades.Text = "Assign Pre-Primary Grades";
            hlkXseedResult.Text = "Pre-Primary Result";
            hlkXseedProgressReport.Text = "Pre-Primary Progress Report";

        //}
        //else
        //{

        //    lblXseed.Text = "Xseed";
        //    hlkAssignXseedGrades.Text = "Assign Xseed Grades";
        //    hlkXseedResult.Text = "Xseed Result";
        //    hlkXseedProgressReport.Text = "Xseed Progress Report";
        //}
    }

    private void HideStudentMenu()
    {
        if (moSchool == Constants.SchoolId.PPS)
            trStudentMenu.Visible = false;
        else
            trStudentMenu.Visible = true;
    }

    private void ShowRetirementPopup()
    {
        hidShowRetirementPopup.Value = Constants.S_NO;
        if (Session[Constants.S_SESSION_IS_FIRST_LOGIN] == null || Session[Constants.S_SESSION_IS_FIRST_LOGIN].ToString() != Constants.S_NO)
        {
            ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
            if ((moUserRole == Constants.UserRoles.Admin || lstUsers.Any(ru => ru.UserId == miUserId && ru.ReportingPrameterId == Constants.ReportingParameters.RetirementNotice.ToInt())))
            {
                RetirementNoticeConfigBL oRetirementNoticeConfigBL = new RetirementNoticeConfigBL(miSchoolId, miFinancialYearId, miAcademicYearId, miUserId);
                List<PayrollEntities.StaffMemberRetirementNotice> lstStaffRetirementNotice = oRetirementNoticeConfigBL.GetAllStaffsRetirementNotices();
                lstvwRetirementDetails.DataSource = lstStaffRetirementNotice;
                lstvwRetirementDetails.DataBind();
                if (lstStaffRetirementNotice.Count > 0)
                    hidShowRetirementPopup.Value = Constants.S_YES;
            }
        }
    }

    #endregion -- PRIVATE METHOD(s) --    
}