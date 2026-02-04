/* Class Name - SelectCommunicationPopup.cs
 * Author - Yogesh Karne
 * Description - This class is used to save communication details to display.
 */
using System;
using System.Collections.Generic;
using System.Web.Services;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;

public partial class SelectCommunicationPopup : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This event will fired when page will display.
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
                InitFields();
                SetQuestionDetails();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is sued to set question details.
    /// </summary>
    private void SetQuestionDetails()
    {
        AskMeQuestionMaster oAskMeQuestionMaster = AskMeQuestionMasterBL.GetQuestionDetails(miSchoolId, miAcademicYearId, 0, hidQuestionId.Value.ToInt(), miUserId);
        lblMainQuestion.Text = oAskMeQuestionMaster.Title;
        hidIsQueryPublished.Value = (oAskMeQuestionMaster.IsQueryPublished ? Constants.S_ONE : Constants.S_ZERO);
        hidEnablePublishButton.Value = (oAskMeQuestionMaster.IsPublishBtnEnabled?Constants.S_ONE:Constants.S_ZERO);
    }

    /// <summary>
    /// This method is used to initilize fileds.
    /// </summary>
    private void InitFields()
    {
        hidSchoolId.Value = miSchoolId.ToString();
        hidUserId.Value = miUserId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
    }

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (!QueryString["QuestionId"].IsNullOrEmpty())
            hidQuestionId.Value = QueryString["QuestionId"];
    } 

    #endregion

    #region Public Method(s)

    /// <summary>
    /// This method is used to get communication details.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiQuestionId"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult Get(int aiSchoolId, int aiAcademicYearId, int aiQuestionId)
    {
        List<AskMeCommunicationDetails> lstAskMeCommunicationDetails = AskMeQuestionMasterBL.GetAskMeCommunication(aiSchoolId, aiAcademicYearId, aiQuestionId);

        var result = new DataSourceResult()
        {
            Data = lstAskMeCommunicationDetails
        };
        return result;
    }

    /// <summary>
    /// This method is used to save communication details.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="asSelectedQuestionIds"></param>
    /// <param name="aiUserId"></param>
    [WebMethod]
    public static void SaveSelection(int aiSchoolId, string asSelectedQuestionIds, int aiMasterQuestionId, int aiUserId)
    {
        AskMeQuestionMasterBL.SaveSelection(aiSchoolId, asSelectedQuestionIds, aiMasterQuestionId, aiUserId);
    }

    [WebMethod]
    public static void PublishQuery(int aiSchoolId, int aiAcademicYearId,int aiQuestionId, int aiUserId, int aiIsPublish )
    {   
        bool bIsPublish = (aiIsPublish == 1 ? true : false);
        AskMeQuestionMasterBL oAskMeQuestionMasterBL = new AskMeQuestionMasterBL();
        oAskMeQuestionMasterBL.PublishCommunicationDetails(aiSchoolId, aiAcademicYearId, aiQuestionId, aiUserId, bIsPublish);
    } 

    #endregion
}