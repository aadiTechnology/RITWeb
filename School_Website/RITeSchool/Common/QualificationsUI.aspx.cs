//// File Name  : QualificationsUI.aspx.cs
//// Created By : Yogesh
//// Date       : 12-Mar-2015
//// Description :This class is used to add, edit and delete qualification.
//// 

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Services;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;

public partial class QualificationsUI : SchoolBase
{
    #region Events

    /// <summary>
    /// This event is used to page load activity.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                hidmiUserId.Value = miUserId.ToString();
                hidAcademicYearId.Value = miAcademicYearId.ToString();
                hidSchoolId.Value = miSchoolId.ToString();
                btnSave.Attributes.Add("onclick", "if(!SaveQualification()) return false;");
                btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method is used to get all pan attachment details.
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
    public static DataSourceResult GetAll()
    {
        List<QualificatoinDetails> lstQualificatoinDetails = QualificationBL.GetAll();
        var result = new DataSourceResult()
        {
            Data = lstQualificatoinDetails,
            Total = lstQualificatoinDetails.Count
        };
        return result;
    }

    /// <summary>
    /// This method is used to save Qualification Details.
    /// </summary>
    /// <param name="asName"></param>
    /// <param name="aiId"></param>
    /// <param name="aiUserId"></param>
    /// <returns></returns>
    [WebMethod]
    public static string Save(string asName, string aiId, string aiUserId, string aiAcademicYearId, string aiSchoolId)
    {
        QualificatoinDetails oQualificatoinDetails = Populate(asName, aiId.ToInt());
        return QualificationBL.Save(oQualificatoinDetails, aiUserId.ToInt(), aiAcademicYearId.ToInt(), aiSchoolId.ToInt());
    }

    /// <summary>
    /// This method is used to Delete Qualification Details.
    /// </summary>
    /// <param name="aiQualifiationId"></param>
    [WebMethod]
    public static void Delete(int aiQualificationId, int aiAcademicYearId, int aiUserId)
    {
        QualificationBL.Delete(aiQualificationId, aiAcademicYearId, aiUserId);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// This method is used to populate values from controls.
    /// </summary>
    /// <param name="asName"></param>
    /// <param name="aiId"></param>
    /// <returns></returns>
    private static QualificatoinDetails Populate(string asName, int aiId)
    {
        QualificatoinDetails oQualificatoinDetails = new QualificatoinDetails
        {
            QualificationId = aiId,
            Qualification = asName
        };
        return oQualificatoinDetails;
    }

    #endregion

}