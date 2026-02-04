using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StudentEntities;
using Utility;
using PayrollReportingUserEntities;
using System.Linq;

public partial class StudentDetailsUI : SchoolBase
{
    #region "Constants"

    private const string S_DEFAULT_PHOTO = "~/RITeSchool/images/Student_BlankPh.jpg";
    private const string S_SEARCH = "Search";
    
    #endregion

    #region "Events"

    ///// <summary>
    ///// This method is used to change masterpage.
    ///// </summary>
    ///// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);         
        }
        catch (Exception ex)
        {
            this.AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to decrypt querystring,display student personal and LC details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {   
                ReadQueryString();
                SetJavascriptAttribute();
                SetLinkView();

                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                    SetCultureSettings();
            }

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
                SetCultureSettings();

            ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);  //// new line add
            List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();   ////new line add
            if ((lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.HideTabsFromStudentDetailScreen.ToInt() && ru.UserId == miUserId).Any()))
            {
                tdRollNosGeneration.Visible = false;  //// new add
                hyperlnk.Visible = false;  ////new add
            }
        }
        catch (Exception ex)
        {
            this.AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display details of selected student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwStudent_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == Constants.S_COMMAND_SELECT)
            {
                int iStudentId = Convert.ToInt32(e.CommandArgument);                
                tblStudentDetails.Visible = true;
                DisplayStudentDetails(iStudentId);                
            }
        }
        catch (Exception ex)
        {
            this.AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwStudent_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstVwStudent.Items.Count > Constants.I_ZERO)
                ControlUtility.FillListViewPagerFooterWithCulture(lstVwStudent, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
            else
                DtPgCount.Visible = false;
        }
        catch (Exception ex)
        {
            this.AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to read registration number.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstVwStudent_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                DataRowView oDataRowView = (System.Data.DataRowView)oCurrentItem.DataItem;
                Label oLabel = (Label)oCurrentItem.FindControl("lblReg_No");
                HiddenField oHiddenField = (HiddenField)oCurrentItem.FindControl("hidisLeft"); 
                if (oHiddenField.Value != Constants.S_EMPTY_STRING)
                {
                    HtmlTableRow oHtmlTableRow = oCurrentItem.FindControl("Tr2") as HtmlTableRow;
                    oHtmlTableRow.Style.Add(HtmlTextWriterStyle.Color, "Red");
                }                
            }
        }
        catch (Exception ex)
        {
            this.AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to display page count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCnt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNoAndCulture(lstVwStudent, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);            
        }
        catch (Exception ex)
        {
            this.AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to search student according to name/Reg. No.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSearch.Text == Resources.LocalizedResources.Search)
            {   
                SetVisibility(true);
                FillStudentListview();
                btnSearch.Text = Resources.LocalizedResources.ChangeInput;
                hidSearch.Value = "Change Input";                
                SetListView();
            }
            else
            {
                lstVwStudent.DataSourceID = null;
                SetVisibility(false);                
                tblStudentDetails.Visible = false;
                btnSearch.Text = Resources.LocalizedResources.Search;
                hidSearch.Value = "Seacrh";                
                DtPgCount.Visible = false;                
            }
        }
        catch (Exception ex)
        {
            this.AddExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Method"

    /// <summary>
    /// This method is used to set visibility of controls.
    /// </summary>
    /// <param name="abVisibility"></param>
    private void SetVisibility(bool abVisibility)
    {
        lstVwStudent.Visible = abVisibility;
        txtRegNo.Enabled = !abVisibility;
        lblErr.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to fill up student listview.
    /// </summary>
    private void FillStudentListview()
    {
        var oDtPgDropDown = lstVwStudent.FindControl("DtPgDropDown") as DataPager;
        if (oDtPgDropDown != null)
            oDtPgDropDown.SetPageProperties(0, oDtPgDropDown.PageSize, true);

        lstVwStudent.DataSourceID = lstDSobj.ID;
        lstVwStudent.DataBind();
        
        if (oDtPgDropDown != null)
        {
            DropDownList oddlCnt = oDtPgDropDown.Controls[0].FindControl("ddlCnt") as DropDownList;
            if (oddlCnt != null)
            {
                if (!oddlCnt.SelectedValue.IsNullOrEmpty())
                {
                    if (oddlCnt.SelectedIndex != 0 && oddlCnt.Items.Count >= 1)
                    {
                        oddlCnt.SelectedIndex = 0;
                        ddlCnt_SelectedIndexChanged(oddlCnt, null);
                    }
                }
            }
        }               
    }

    /// <summary>
    /// This method is used to set javascript attribute.
    /// </summary>
    private void SetJavascriptAttribute()
    {
        base.SetDefaultButton(btnSearch);
        ApplyMouseHoverEffect(new List<Button> { btnSearch });
        hidSearch.Value = S_SEARCH;
    }

    /// <summary>
    /// This method is used to set design according to selected language
    /// </summary>
    private void SetCultureSettings()
    {
        ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));
        btnSearch.Text = oResourceManager.GetString(hidSearch.Value.Replace(" ", string.Empty));
        ValidationSummary1.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
        hlnkStudentFeeDetails.Text = Resources.LocalizedResources.Fees;
        hlnkStudentAttendance.Text = Resources.LocalizedResources.Attendance;
        hlnkStudentRollNos.Text = Resources.LocalizedResources.StudentDetails;
    }

    /// <summary>
    /// This method is used to log an exception to the error log table in the database.
    /// </summary>
    /// <param name="aoException"></param>
    /// <param name="aoCurrentMethod"></param>
    private void AddExceptionToErrorLog(Exception aoException, MethodBase aoCurrentMethod)
    {
        int iUserid = miUserId;
        ExceptionHandler.WriteExceptionToErrorLog(string.Format("{0}. Trace: {1}", aoException.Message, aoException.StackTrace),
                                                  string.Format("{0}.{1}", aoCurrentMethod.DeclaringType.FullName, aoCurrentMethod.Name),
                                                  iUserid);
    }

    /// <summary>
    /// This method is used to set listview.
    /// </summary>
    private void SetListView()
    {
        if (lstVwStudent.Items.Count == Constants.I_ONE)
        {
            int iStudentId = lstVwStudent.DataKeys[0]["SchoolWise_Student_Id"].ToInt();           
            tblStudentDetails.Visible = true;
            DisplayStudentDetails(iStudentId);       
            lstVwStudent.Visible = false;
            DtPgCount.Visible = false;
        }
        else if (lstVwStudent.Items.Count == Constants.I_ZERO)
        {
            lstVwStudent.Visible = false;
            DtPgCount.Visible = false;
            lblErr.Text = Resources.LocalizedResources.StudentNotFound;
        }
        else
        {
            lstVwStudent.Visible = true;
            tblStudentDetails.Visible = false;
            DtPgCount.Visible = true;
        }
    }

    /// <summary>
    /// This method use to bind student information to all controls below listview
    /// </summary>
    private void DisplayStudentDetails(int aiStudentId)
    {
        StudentBL oStudentBL = new StudentBL();
        StudentDetails oStudentDetails = oStudentBL.GetStudentInfo(miSchoolId, miAcademicYearId, aiStudentId);
        if (oStudentDetails != null)
        {
            SetFieldValues(oStudentDetails);
            if (divStudentInfo.Visible)
                SetStudentScreenUrl(oStudentDetails);
            SetStudentFeeScreenUrl(oStudentDetails);
            SetStudentAttendanceScreenUrl(oStudentDetails);
            SetExamDetails(oStudentDetails);
       }
    }

    /// <summary>
    /// This method is used to set exam details URL.
    /// </summary>
    /// <param name="aoStudentDetails"></param>
    private void SetExamDetails(StudentDetails aoStudentDetails)
    {   
        string sQueryString = "../Student/StudentProgressSheet.aspx?" + CommonUtility.EncryptQuerystring("IsOldProgressReport=True&IsTeacherLogin=True&StudentId=" + aoStudentDetails.YearwiseStudentId + "&StandardId=" + aoStudentDetails.StandrdId + "&SdtDivId=" + aoStudentDetails.SchoolwiseStandardDivisionId + "&ShowCurrentYearData=1");
        hlnkExam.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650'); return false;", sQueryString));
    }

    /// <summary>
    /// This method use to set value to all label controls
    /// </summary>
    /// <param name="aoStudentDetails"></param>
    private void SetFieldValues(StudentDetails aoStudentDetails)
    {
        divStudentInfo.Visible = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Student) || moUserRole == Constants.UserRoles.Admin;
        lblStudentName.Text = aoStudentDetails.Name;
        lblRollNo.Text = Convert.ToString(aoStudentDetails.RollNo);
        lblMobileOne.Text = aoStudentDetails.MobileNo1;
        lblDOB.Text = aoStudentDetails.DOB.ToString(Constants.S_DATE_FORMAT);
        lblClass.Text = aoStudentDetails.StandrdDivision;     
        if (!aoStudentDetails.HasDebitEntries)
        {
            hlnkStudentFeeDetails.Enabled = false;
            hlnkStudentFeeDetails.ToolTip = "There are no fee entries for this student.";
        }
        else
        {
            hlnkStudentFeeDetails.Enabled = true;
            hlnkStudentFeeDetails.ToolTip = string.Empty;
        }

        if (!string.IsNullOrEmpty(aoStudentDetails.PhotoFilePath))
            imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + aoStudentDetails.UserId;
        else
            imgPhoto.Src = S_DEFAULT_PHOTO;
    }

    /// <summary>
    /// This Method used to set url to open student ui page
    /// </summary>
    private void SetStudentScreenUrl(StudentDetails aoStudentDetails)
    {
        string sUrl = string.Format("../Teacher/StudentUI.aspx?StudentId={0}&amp;StudentName={1}&amp;ClassName={2}&amp;RegNo={3}", aoStudentDetails.SchoolwiseStudentId, aoStudentDetails.Name, aoStudentDetails.StandrdDivision, aoStudentDetails.EnrollmentNo);
        string sQueryString = string.Empty;

        sQueryString = sUrl.Substring(sUrl.IndexOf("?") + 1) + "&StandardId=" + aoStudentDetails.StandrdId
                                                                + "&DivisionId=" + aoStudentDetails.DivisionId
                                                                + "&abIsExactMatch=" + "False"
                                                                + "&ClassName=" + aoStudentDetails.StandrdDivision
                                                                + "&SearchedNumber=" + aoStudentDetails.EnrollmentNo
                                                                + "&IsStudntDtailsScrn=" + Constants.S_YES;
        string sStudnentInfo = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
        hlnkStudentRollNos.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=650'); return false;", sStudnentInfo));
    }


       /// <summary>
    /// This method use to set url string for fee screen
    /// </summary>
    /// <param name="aoStudentDetails"></param>
    private void SetStudentFeeScreenUrl(StudentDetails aoStudentDetails)
    {
        string sUrl = string.Format("../Accountant/StudentPayFeeUI.aspx?StudentId={0}&IsStudntDtailsScrn={1}&IsNewStudent={2}", aoStudentDetails.YearwiseStudentId, Constants.S_YES, aoStudentDetails.IsNewStudent);
        string sQueryString = string.Empty;
        sQueryString = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sUrl.Substring(sUrl.IndexOf("?") + 1));       
        if (aoStudentDetails.HasDebitEntries)
            hlnkStudentFeeDetails.Attributes.Add("onclick", "OpenFeePopup(this,'" + sQueryString + "'); return false;");
        else
            hlnkStudentFeeDetails.Attributes.Remove("onclick");

    }

    /// <summary>
    /// This method is used to set show/hide links as per school.
    /// </summary>
    private void SetLinkView()
    {
        if (miSchoolId == Constants.SchoolId.SNS.ToInt() && moUserRole == Constants.UserRoles.Teacher)
        {
            tdRollNosGeneration.Visible = false;
            hyperlnk.Visible = false;
        }
        else
        {
            tdRollNosGeneration.Visible = true;
            hyperlnk.Visible = true;
        }
    }

    /// <summary>
    /// This method use to set url for student attendance
    /// </summary>
    /// <param name="oStudentDetails"></param>
    private void SetStudentAttendanceScreenUrl(StudentDetails aoStudentDetails)
    {
        string sUrl = string.Format("../Student/StudentAttendance.aspx?StudentId={0}&StandardId={1}&DivisionId={2}&IsFrom={3}", aoStudentDetails.YearwiseStudentId, aoStudentDetails.StandrdId, aoStudentDetails.DivisionId, "StudentDetailsUI.aspx");
        string sQueryString = string.Empty;
        sQueryString = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sUrl.Substring(sUrl.IndexOf("?") + 1));
        hlnkStudentAttendance.Attributes.Add("onclick", string.Format("window.open('{0}' , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650'); return false;", sQueryString));
    }
   
    /// <summary>
    /// This method is used to read querystring.
    /// </summary>
    private void ReadQueryString()
    {   
        string sTestDecrypt = Server.UrlDecode(Convert.ToString(Request.QueryString));
        HidBackUrl.Value = sTestDecrypt;
        if (QueryString["StudentId"] != null)
            hidStudentId.Value = QueryString["StudentId"];
    }

    #endregion "Private Method"
}