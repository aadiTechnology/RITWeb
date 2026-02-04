//// File Name  : SurveySchoolDetailsPopup.aspx.cs
//// Created By : Yogesh
//// Date       : 31/10/2015
//// Description :This class is used to maintain survey school record details functionality. 
////   

using System;
using System.Collections.Generic;
using System.Web.Services;
using BusinessLogic;
using Kendo.DynamicLinq;
using SchoolEntities;


public partial class SurveySchoolDetailsPopup : SchoolBase
{    

    #region Event(s)

    /// <summary>
    /// This event is fired at page is loading.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hidmiUserId.Value = miUserId.ToString();
            hidAcademicYearId.Value = miAcademicYearId.ToString();
            hidSchoolId.Value = miSchoolId.ToString();

        }
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// This method is used to get all school details.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <returns></returns>
    [WebMethod]
    public static DataSourceResult GetAll(int aiSchoolId, int aiAcademicYearId)
    {
        List<SurveySchool> lstSurveySchool = SurveySchoolDetailsBL.GetAll(aiSchoolId, aiAcademicYearId);
        var result = new DataSourceResult()
        {
            Data = lstSurveySchool,
            Total = lstSurveySchool.Count
        };

        return result;
    }

    /// <summary>
    /// This method is used to save school details.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="SurveySchoolId"></param>
    /// <param name="SurveySchoolName"></param>
    /// <param name="aiUserId"></param>
    /// <returns></returns>
    [WebMethod]
    public static string Save(int aiSchoolId, int aiAcademicYearId, int SurveySchoolId, string SurveySchoolName, int aiUserId)
    {
        var Name = SurveySchoolName.Trim();
        return SurveySchoolDetailsBL.Save(aiSchoolId, aiAcademicYearId, SurveySchoolId, Name, aiUserId);
    }

    /// <summary>
    /// Thid method is used to delete school details.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <param name="aiServaySchoolId"></param>
    /// <param name="aiUserId"></param>
    [WebMethod]
    public static void Delete(int aiSchoolId, int aiAcademicYearId, int aiServaySchoolId, int aiUserId)
    {
        SurveySchoolDetailsBL.Delete(aiSchoolId, aiAcademicYearId, aiServaySchoolId, aiUserId);
    }

    #endregion
    
}