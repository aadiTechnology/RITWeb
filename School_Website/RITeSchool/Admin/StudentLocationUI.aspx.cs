/* File Name :- SchoolLocationUI.aspx.cs
 * Created Date :- 23-DEC-2015
 * Class Description :- This class is used to manage Location Details. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.WebControls;


public partial class SchoolLocationUI : SchoolBase
{

    #region Events

    /// <summary>
    /// This Event is used to set school id and academic year id to hidden fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used to Save And Update Location.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="aiId"></param>
    /// <param name="aiUserId"></param>
    /// <param name="asLocation"></param>
    [WebMethod]
    public static string SaveLocation(int aiSchoolId, int aiId,int aiUserId, string asLocation)
    {
        string sMessage = string.Empty;
        try
        {            
            AdmissionProcessDetailsBL oAdmissionProcessDetailsBL = new AdmissionProcessDetailsBL(aiSchoolId, aiUserId, 0);
            oAdmissionProcessDetailsBL.SaveStudentLocation(aiId, asLocation);
        }
        catch (SqlException se)
        {
            sMessage = se.Message;
        }
        return sMessage;
    }


    /// <summary>
    /// This event is used to Get All Living Locations for fill KendoGrid.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    [WebMethod]
    public static DataSourceResult GetAllLivingLocation(int aiSchoolId,int aiUserId)
    {
        AdmissionProcessDetailsBL oAdmissionProcessDetailsBL = new AdmissionProcessDetailsBL(aiSchoolId, aiUserId, 0);
        List<StudentLivingLocation> lstLivingLocation = oAdmissionProcessDetailsBL.GetAllLivingLocation(aiSchoolId);

        var result = new DataSourceResult()
        {
            Data = lstLivingLocation,
            Total = lstLivingLocation.Count
        };

        return result;
    }

    /// <summary>
    /// This event is used to delete Living Location.
    /// </summary>
    /// <param name="aiQuestionDetailsId"></param>
    /// <param name="aiUpdatedById"></param>
    /// <param name="aiSchoolId"></param>
    [WebMethod]
    public static string DeleteStudentLocation(int aiLocationId, int aiSchoolId, int aiUpdatedById)
    {
        string sMessage = string.Empty;
        try
        {
            AdmissionProcessDetailsBL oAdmissionProcessDetailsBL = new AdmissionProcessDetailsBL(aiSchoolId, aiUpdatedById, 0);
            oAdmissionProcessDetailsBL.DeleteLocation(aiLocationId);
        }
        catch (SqlException se)
        {
            sMessage = se.Message;
        }
        return sMessage;
    }

    #endregion

    #region PrivateMethod

    private void SetJavascriptAttributes()
    {
        btnSave.Attributes.Add("onclick", "if(!ValidateLocation()) return false;");
        btnCancel.Attributes.Add("onclick", "CleareFields()");
        hidSchoolId.Value = miSchoolId.ToString();
        hidAcademicYearId.Value = miAcademicYearId.ToString();
        hidUserId.Value = miUserId.ToString();
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
    }

    #endregion

}