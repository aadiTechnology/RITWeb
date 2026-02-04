/* File Name - PublishedQueriesUI.aspx.cs
 * Created Date - 31 Jul 2014
 * Cready By - Sachin
 * Description - This class is used to display query details.
 */
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using BusinessLogic;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;
using System;
using BusinessLogic.Exceptions;

public partial class PublishedQueriesUI : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This event is used to fill up category list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillCategoryList();
                if (moUserRole == Constants.UserRoles.Student)
                    divNewQuery.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// This method is used to return all available questions.
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
        List<AskMeQuestionMaster> lstQuestions = AskMeQuestionMasterBL.GetAllQuestions(aiSchoolId, aiAcademicYearId, aiStatusId, aiLoginUserId, sSortExpression, sSortDirection, iStartINdex, iEndIndex, true, asFilter, asCategories);
        
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
    /// This method is sued to return query string.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <param name="aiIsPublishedView"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetQueryString(int aiQuestionId, int aiIsPublishedView)
    {
        string sQueryString = CommonUtility.EncryptQuerystring("QuestionId=" + aiQuestionId + "&IsPublishedView=" + aiIsPublishedView);
        return sQueryString;
    }

    /// <summary>
    /// This method is used to return query string.
    /// </summary>
    /// <param name="aiQuestionId"></param>
    /// <param name="aiQuestionDetailsId"></param>
    /// <param name="aiIsReply"></param>
    /// <returns></returns>
    [WebMethod]
    public static string GetNewQueryString(int aiQuestionId, int aiQuestionDetailsId, int aiIsReply)
    {
        string sQueryString = CommonUtility.EncryptQuerystring("QuestionId=" + aiQuestionId + "&QuestionDetailsId=" + aiQuestionDetailsId + "&IsReply=" + aiIsReply);
        return sQueryString;
    }

    /// <summary>
    /// This method is used o fill up category list.
    /// </summary>
    private void FillCategoryList()
    {
        AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL(miSchoolId, miAcademicYearId, miUserId);
        List<AskMeCategory> lstCategories = oAskMeQuestionMasterBL.GetAllCategories();
        ListSource.FillCheckBoxList(lstCategories, chkCategoryLst, "Name", "Id");
    }

    #endregion
}