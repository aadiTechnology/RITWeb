/* File Name - QueryDetailsUI.aspx.cs
 * Created Date - 31 Jul 2014
 * Cready By - Sachin
 * Description - This class is used to manage query details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using BusinessLogic;
using Kendo.DynamicLinq;
using PayrollReportingUserEntities;
using SchoolEntities;
using Utility;
using BusinessLogic.Exceptions;
using System.Text;

public partial class QueryDetailsUI : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This event is used to fill status combo box and check login user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillStatusCombobox();
                FillCategoryList();
                CheckLoginUser();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    private void FillCategoryList()
    {
        AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL(miSchoolId, miAcademicYearId, miUserId);
        List<AskMeCategory> lstCategories = oAskMeQuestionMasterBL.GetAllCategories();
        ListSource.FillCheckBoxList(lstCategories, chkCategoryLst, "Name", "Id");
    } 

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to check whether login user is moderator.
    /// </summary>
    private void CheckLoginUser()
    {
        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
        List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();
        if (lstUsers.FindAll(ru => ru.ReportingPrameterId == Constants.ReportingParameters.Moderator.ToInt() && ru.UserId == miUserId).Any())
            hisIsModerator.Value = Constants.S_ONE;
        else
            hisIsModerator.Value = Constants.S_ZERO;

        if (moUserRole == Constants.UserRoles.Student)
            btnNewQery.Visible = true;
    }

    /// <summary>
    /// This method is used to fill status combo box.
    /// </summary>
    private void FillStatusCombobox()
    {
        AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL();
        List<AskMeStatusMaster> lstStatuses = oAskMeQuestionMasterBL.GetAllStatuses();
        ListSource.FillDropDownList(lstStatuses, cmbStatus, "Name", "Id", Constants.S_ALL);
        cmbStatus.SelectedValue = Constants.AskMeStatus.New.ToInt().ToString();
    } 

    #endregion

    #region Public Method(s)
    
    /// <summary>
    /// This method is used to return all available questions according to selected page.
    /// </summary>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <param name="sort"></param>
    /// <param name="filter"></param>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="aiLoginUserId"></param>
    /// <param name="aiStatusId"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetAllQuestions(int take, int skip, IEnumerable<Sort> sort, Filter filter, int aiSchoolId, int aiAcademicYearId, int aiLoginUserId, int aiStatusId, string asFilter, string asCategories)
    {        
        int iStartINdex = skip + 1;
        int iEndIndex = iStartINdex + take;
        string sSortExpression = "LastUpdatedDate";
        string sSortDirection = "Desc";
        if (sort != null && sort.Count() > 0)
        {
            sSortExpression = sort.FirstOrDefault().Field;
            sSortDirection = sort.FirstOrDefault().Dir;
        }

        List<AskMeQuestionMaster> lstQuestions = AskMeQuestionMasterBL.GetAllQuestions(aiSchoolId, aiAcademicYearId, aiStatusId, aiLoginUserId, sSortExpression, sSortDirection, iStartINdex, iEndIndex, false, asFilter, asCategories);
        
        int iRecordCount = 0;
        if (lstQuestions.Count > 0)
            iRecordCount = lstQuestions[0].TotalRowCount;

        var result = new DataSourceResult()
        {
            Data = lstQuestions,
            Total = iRecordCount
        };

        return result;
    }

    /// <summary>
    /// This method is used to return all available question communications according to selected question and page.
    /// </summary>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <param name="sort"></param>
    /// <param name="filter"></param>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="aiQuestionId"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetAllQuestionCommunications(int take, int skip, IEnumerable<Sort> sort, Filter filter, int aiSchoolId, int aiAcademicYearId, int aiQuestionId, int aiLoginUserId)
    {
        int iStartINdex = skip + 1;
        int iEndIndex = iStartINdex + take;
        string sSortExpression = "Date";
        string sSortDirection = "Desc";

        if (sort != null && sort.Count() > 0)
        {
            sSortExpression = sort.FirstOrDefault().Field;
            sSortDirection = sort.FirstOrDefault().Dir;
        }

        List<AskMeQuestionDetails> lstQuestions = AskMeQuestionMasterBL.GetAllQuestionCommunications(aiSchoolId, aiAcademicYearId, aiQuestionId, sSortExpression, sSortDirection, iStartINdex, iEndIndex, aiLoginUserId);

        int iRecordCount = AskMeQuestionMasterBL.GetCountOfQuestionCommunications(aiSchoolId, aiAcademicYearId, aiQuestionId, aiLoginUserId);

        var result = new DataSourceResult()
        {
            Data = lstQuestions,
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
    public static string SetQueryString(int aiQuestionId)
    {
        return CommonUtility.EncryptQuerystring("QuestionId=" + aiQuestionId );
    }

    /// <summary>
    /// This method is used to return query string.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <param name="aiQuestionDetailsId"></param>
    /// <param name="aiIsReply"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetQueryString(int aiQuestionId, int aiQuestionDetailsId, int aiIsReply)
    {
        return CommonUtility.EncryptQuerystring("QuestionId=" + aiQuestionId + "&QuestionDetailsId=" + aiQuestionDetailsId + "&IsReply=" + aiIsReply);
    }

    /// <summary>
    /// This event is used to delete question details.
    /// </summary>
    /// <param name="aiQuestionDetailsId"></param>
    /// <param name="aiUpdatedById"></param>
    [WebMethod]
    public static void DeleteQuestionDetails(int aiQuestionDetailsId, int aiUpdatedById)
    {
        AskMeQuestionMasterBL.DeleteQuestionDetails(aiQuestionDetailsId, aiUpdatedById);
    }

    /// <summary>
    /// This event is used to publish question details.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <param name="aiUpdatedById"></param>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="abIsPublished"></param>
    [WebMethod]
    public static void PublishQuestionDetails(int aiQuestionId, int aiUpdatedById, int aiSchoolId, int aiAcademicYearId, bool abIsPublished)
    {
        abIsPublished = !abIsPublished;
        AskMeQuestionMasterBL.PublishCommunication(aiSchoolId, aiAcademicYearId, aiQuestionId, aiUpdatedById, abIsPublished);
    }

    /// <summary>
    /// This method is used to submit communication.
    /// </summary>
    /// <param name="aiQuestionDetailsId"></param>
    /// <param name="aiUpdatedById"></param>
    /// <param name="abIsSubmitted"></param>
    [WebMethod]
    public static void SubmitCommunication(int aiSchoolId, int aiQuestionDetailsId, int aiUpdatedById, bool abIsSubmitted)
    {
        abIsSubmitted = !abIsSubmitted;
        AskMeQuestionMasterBL.SubmitCommunication(aiSchoolId, aiQuestionDetailsId, aiUpdatedById, abIsSubmitted);
    }

    [WebMethod]
    public static string AssignCommunication(int aiSchoolId, int aiQuestionId, int aiUpdatedById, bool abIsForward, int aiAcademicYearId)
    {
        try
        {
            AskMeQuestionMasterBL.AssignCommunication(aiSchoolId, aiQuestionId, aiUpdatedById, abIsForward, aiAcademicYearId);
            return string.Empty;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// This method is used to return owner assignment query string.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetOwnerAssignmentQueryString(int aiQuestionId)
    {
        return CommonUtility.EncryptQuerystring("QuestionId=" + aiQuestionId);
    }

    [WebMethod]
    public static void MarkAsInvalid(int aiQuestionId, int aiSchoolId, int aiAcademicYearId, int aiUserId, bool abIsInvalidAction)
    {
        AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL();
        oAskMeQuestionMasterBL.MarkValidityStatus(aiQuestionId, aiSchoolId, aiAcademicYearId, aiUserId, abIsInvalidAction);
    }

    /// <summary>
    /// This method is used to return the Read Receipt Details.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiQuestionId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetReadReceiptDetails(int aiSchoolId, int aiQuestionId, int aiAcademicYearId, int aiLoginUserId)
    {
        StringBuilder oReadReceiptValue = new StringBuilder();
        oReadReceiptValue.Append("<table style='padding-left:12px; width:100%;'><th align='left' style='width:200px;background-Color:pink;'>Owner</th><th align='left' style='background-Color:pink;'> Read Date / Time </th>");
        AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL();
        List<AskMeReadReceiptDetails> lstAskMeReadReceiptDetails = oAskMeQuestionMasterBL.GetReadReceiptDetails(aiSchoolId, aiQuestionId, aiAcademicYearId,aiLoginUserId);

        if (lstAskMeReadReceiptDetails.Count > 0)
        {
            foreach (var lst in lstAskMeReadReceiptDetails)
                oReadReceiptValue.Append(lst.Name);
        }
        else
        {
            oReadReceiptValue.Append("<b>No Record Found.</b>");
        }
        oReadReceiptValue.Append("</table>");
        return oReadReceiptValue.ToString();
    }

    #endregion
}