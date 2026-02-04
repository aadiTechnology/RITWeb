//// File Name  : LeftStudentsDetailsUI.aspx.cs
//// Created By : Yogesh
//// Date       : 09/10/2015
//// Description :This class is used to maintain yearwise left student details functionality. 
////   

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities.AcademicYearwiseLeftStudentDetailsMaster;
using Utility;
using System.Collections;

public partial class LeftStudentsDetailsUI : SchoolBase
{
    #region Constants
    static string msFromUrl = string.Empty;
    const string S_SCREENS_URL = "ScreensUI.aspx";
   #endregion


    #region Event(s)





    /// <summary>
    /// This event is used to set masterpage according to login user.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            base.OnPreInit(e);

            if (!IsPostBack)
                msFromUrl = GetFromPageUrl();

            string sFromPage = string.Empty;

            if (Request.QueryString.ToString() != string.Empty)
            {
                if (QueryString["FromPage"] != null)
                    sFromPage = QueryString["FromPage"];
            }

            if (msFromUrl.Equals(S_SCREENS_URL) || sFromPage == S_SCREENS_URL)
                this.Page.MasterPageFile = "~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master";
            else
                this.Page.MasterPageFile = "~/RITeSchool/MasterPages/MasterPage.master";

            if (sFromPage == S_SCREENS_URL)
                msFromUrl = sFromPage;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }



    /// <summary>
    /// This event will fired while page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                hidSchoolId.Value = miSchoolId.ToString();
                ReadQueryString();
                this.SetJavascriptAttributes();
                this.FillAcademicYearCombo();
                this.FillStandardCombo();
                SetButtonValue();
                hidCurrentAcademicYearId.Value = miAcademicYearId.ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }




    #endregion

    #region Public Method(s)

    /// <summary>
    /// This method is used to fill academic year combo on page load.
    /// </summary>
    private void FillAcademicYearCombo()
    {
        DataTable oDtYearInfo = GetDataForAcademicYear();
        cmbAcademicYear.Bind(oDtYearInfo, "Academic_Year_ID", "YearValue", Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to provide data about all academic years of school.
    /// </summary>
    /// <returns></returns>
    private DataTable GetDataForAcademicYear()
    {
        var oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDtYearInfo = oSchoolWiseAcademicYearMasterBL.GetAllAcademicYearsForSchool(miSchoolId);
        return oDtYearInfo;
    }

    /// <summary>
    /// This method is used to fill Standard Combo cox.
    /// </summary>
    private void FillStandardCombo()
    {

        DataTable oDtStandard = GetDataForStandardCombo();
        cmbStandardId.Bind(oDtStandard, "Original_Standard_Id", "Standard_Name", Constants.S_SELECT_ALL);
    }
    /// <summary>
    /// This method is used for Reading the QueryString.
    /// </summary>
    private void ReadQueryString()
    {
              if (QueryString["Is_SuperAdmin"] != null)
            hidIsSuperAdmin.Value = QueryString["Is_SuperAdmin"];
    }
    /// <summary>
    /// This method is used to get data for filling standard combo box.
    /// </summary>
    /// <returns></returns>
    private DataTable GetDataForStandardCombo()
    {
        LeftStudentsDetailsBL oLeftStudentsDetailsBL = new LeftStudentsDetailsBL();
        DataTable oDataTable = oLeftStudentsDetailsBL.GetDataForStandardCombo(miSchoolId, miAcademicYearId);
        return oDataTable;
    }
    /// <summary>
    /// This method is used to get academic yearwise left student details as per filter.
    /// </summary>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <param name="sort"></param>
    /// <param name="aiUserRoleId"></param>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="asNameFilter"></param>
    /// <param name="abShowAllDetails"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult Get(int take, int skip, IEnumerable<Sort> sort, int aiSchoolId, int aiAcademicYearId, int aiStandardId, string asNameFilter)
    {
        int iStartINdex = skip + 1;
        int iEndIndex = iStartINdex + take;
        string sSortDirection = "DESC";

        if (sort != null && sort.Count() > 0)
            sSortDirection = sort.FirstOrDefault().Dir;

        int iRecordCount = Constants.I_ZERO;

        List<AcademicYearwiseLeftStudentDetails> lstLeftStudentDetails = LeftStudentsDetailsBL.Get(aiSchoolId, aiAcademicYearId, aiStandardId, asNameFilter, sSortDirection, iStartINdex, iEndIndex);
        if (lstLeftStudentDetails.Count > Constants.I_ZERO)
       iRecordCount = lstLeftStudentDetails[0].TotalRowCount;

        var result = new DataSourceResult()
        {
            Data = lstLeftStudentDetails,
            Total = iRecordCount
        };
        return result;
    }

    /// <summary>
    /// This method is used to return query string.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <param name="aiQuestionDetailsId"></param>
    /// <param name="aiIsReply"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetQueryString(int aiSchoolId, int aiAcademicYearId, int aiStudentId, string asStudentName, string asClassName, string asRegNo, int aiStandardId, int aiDivisionId, string asLeftDate)
    {
        DateTime dtLeftDate = Convert.ToDateTime(asLeftDate);
        return CommonUtility.EncryptQuerystring("StudentId=" + aiStudentId + "&StudentName=" + asStudentName + "&ClassName=" + asClassName + "&RegNo=" + asRegNo + "&StandardId=" + aiStandardId + "&DivisionId=" + aiDivisionId + "&standardName=" + string.Empty + "&DivisionName=" + string.Empty + "&NewMode=N&pIndex=0&pSortExp=Roll_No&pSortDirc=asc&Is_Configured=0&DivSelectedValue=0&StdSelectedValue=0&NameOrRegNo=&abIsExactMatch=False&IsSchoolLeft=" + dtLeftDate + "&AcademicYearId=" + aiAcademicYearId.ToString() + "&ClassId=0&asOperator=0&asPrefix=All&asPostfix=All&SearchedNumber=&Is_SuperAdmin=N&IsStudntDtailsScrn=N&FromLeftStudentScreen=Y");
    }

    /// <summary>
    /// This method is used to send SMS to the selected student.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="asStudentName"></param>
    /// <param name="SMSText"></param>
    [WebMethod]
    public static void SendSMS(int aiSchoolId, int aiAcademicYearId, string aiStudentId, string SMSText, int aiCurrentAcademicYearId)
    {
        int iIndex;
        LeftStudentsDetailsBL oLeftStudentsDetailsBL = new LeftStudentsDetailsBL();
        Hashtable oHTUsersMobileNo = new Hashtable();
        string sMobileNo1 = string.Empty, sMobileNo2 = string.Empty, sName;
        DataTable oDataTable = oLeftStudentsDetailsBL.GetMobileNumber(aiStudentId);
        for (iIndex = 0; iIndex < oDataTable.Rows.Count; iIndex++)
        {
            sMobileNo1 = (oDataTable.Rows[iIndex]["Mobile_Number"].ToString());
            sMobileNo2 = (oDataTable.Rows[iIndex]["Mobile_Number2"].ToString());
            sName = (oDataTable.Rows[iIndex]["Name"].ToString());
            if (sMobileNo1 != string.Empty)
                oHTUsersMobileNo[oDataTable.Rows[iIndex]["User_Id"]] = sMobileNo1;
            if (sMobileNo2 != string.Empty && sMobileNo2 != "0")
            {
                oHTUsersMobileNo[oDataTable.Rows[iIndex]["User_Id"] + "sm;"] = sMobileNo2;
            }
            SMS oSMS = new SMS();
            SchoolBL oSchoolBL = new SchoolBL(aiSchoolId);
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.SenderID = oSchoolBL.AdminId;
            oSMS.School_Name = oSchoolBL.SchoolName;
            oSMS.SMSText = SMSText;
            oSMS.AcademicYearID = aiCurrentAcademicYearId;
            oSMS.SchoolID = aiSchoolId;
            oSMS.DisplayText = sName;
            oSMS.To = oHTUsersMobileNo;
            oSMS.Send();
            oHTUsersMobileNo.Clear();
        }
    }

    /// <summary>
    /// This method is related to Student readmission process.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="aiCurrentAcademicYearId"></param>
    [WebMethod]
    public static void ReadmissionLeftStudent(int aiSchoolId, int aiAcademicYearId, string aiStudentId)
    {
        LeftStudentsDetailsBL oLeftStudentsDetailsBL = new LeftStudentsDetailsBL();
        oLeftStudentsDetailsBL.ReadmissionLeftStudent(aiSchoolId, aiAcademicYearId, aiStudentId);
    }


    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to set JavaScript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSearch });
        if ((moUserRole == Constants.UserRoles.Admin ||
                  ((moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
                      && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter).ToString() == "Y")) && System.Configuration.ConfigurationManager.AppSettings["SendSMS"] == "Y")
        {
            hidCanEdit.Value = "1";
        }
        else 
        {
            hidCanEdit.Value = "0";
        }
        if (hidIsSuperAdmin.Value ==Constants.S_YES)
            btnReadmission.Visible = true;
        else
            btnReadmission.Visible = false;
    }


    /// <summary>
    /// This method is used to get referrence page URL.
    /// </summary>
    /// <returns></returns>
    private string GetFromPageUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }
    /// <summary>
    /// This method is used for back button (PostBackUrl)
    /// </summary>
    private void SetButtonValue()
    {
        string sFromPage = string.Empty;
        if (msFromUrl.Equals(S_SCREENS_URL) || sFromPage == S_SCREENS_URL)
            btnBack.PostBackUrl = "../SuperAdmin/ScreensUI.aspx";
        else
            btnBack.PostBackUrl = "../Admin/AllStudentsUI.aspx?";
    }





    #endregion

   

}