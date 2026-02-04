using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Services;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using SchoolEntities;
using Utility;

public partial class SchoolSettingsUI : SchoolBase
{
    /// <summary>
    /// This method is sued to set school id and academic year id to hidden fields.
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
                hidAcademicYearId.Value = miAcademicYearId.ToString();
                base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnRefreshCache, btnBack,btnSaveModule });
                lblHeader.Text = "School Settings (" + Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToDateTime().Year + " - " + Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToDateTime().Year + ")";
                lblModule.Text = "School Module Settings (" + Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToDateTime().Year + " - " + Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToDateTime().Year + ")"; 
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to refresh cache.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshCache_Click(object sender, EventArgs e)
    {
        try
        {
            using (var swFile = new StreamWriter(Server.MapPath(@"~\Cache.txt"), true))
            {
                swFile.WriteLine("\n" + DateTime.Now);
                swFile.Flush();
                swFile.Close();
                lblMessage.Text = "Cache is refreshed !!!";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    [WebMethod]
    public static DataSourceResult GetAllSettings(int aiSchoolId, int aiAcademicYearId)
    {
        SchoolBL oSchoolBL = new SchoolBL();
        List<SchoolSettings> lstSettings = oSchoolBL.GetSchoolSettings(aiSchoolId, aiAcademicYearId);

        //lstSettings.ForEach(st =>
        //{
        //    st.Name = Regex.Replace(st.Name, "(?<!^)_?([A-Z])", " $1");
        //});

        var result = new DataSourceResult()
        {
            Data = lstSettings,
            Total = lstSettings.Count
        };

        return result;
    }

    [WebMethod]
    public static DataSourceResult GetAllModule()
    {
        SchoolBL oSchoolBL = new SchoolBL();
        List<SchoolModule> lstSchoolModule = oSchoolBL.GetAllModuleSetting();

        var result = new DataSourceResult()
        {
            Data = lstSchoolModule,
            Total = lstSchoolModule.Count
        };
        return result;
    }
    [WebMethod]
    public static void SaveSetting(int aiSchoolId, int aiAcademicYearId, int aiId, string asValue, string asName)
    {
        asName = asName.Trim();
        SchoolBL oSchoolBL = new SchoolBL();
        oSchoolBL.SaveSchoolSetting(aiSchoolId, aiAcademicYearId, aiId, asValue, asName);
    }

    [WebMethod]
    public static void SaveModuleDetails(string asModuleId)
    {
        SchoolBL oSchoolBL = new SchoolBL();
        oSchoolBL.UpdateModuleDetails(asModuleId);

    }
}