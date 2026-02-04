using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;

/// <summary>
/// Summary description for Payroll
/// </summary>
public abstract class Payroll : System.Web.UI.Page
{
    public Payroll()
    {        
    }

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    /// <returns></returns>    

    protected string ReadQueryString(HttpRequest aoHttpRequest)
    {
        if (Request.QueryString.ToString() != Constants.S_EMPTY_STRING)
        {
            string sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());
            string sQueryString = Utility.CommonUtility.DecryptQuerystring(sTestDecrypt);
            HttpRequest oHttpRequest = new HttpRequest(Page.Request.FilePath.ToString(),
                                            Page.Request.Url.ToString(),
                                            sQueryString);
            return oHttpRequest.QueryString["Is_Configured"];
        }
        return string.Empty;
    }

    /// <summary>
    /// This method is used to save staff groups configuration entry into Configuration_School_Master table.
    /// </summary>
    protected void SaveConfigDetails(int aiOriginalConfigId)
    {
        int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL();        
        oConfiguration.OriginalConfigId = aiOriginalConfigId;
        oConfiguration.SchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        oConfiguration.AcademicYearId = iAcademicYearId;
        oConfiguration.IsConfigure = Constants.C_YES;
        oConfiguration.InsertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oConfiguration.UpdateById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
        oConfiguration.InsertConfigurationSchoolMaster();
    }

    abstract protected void SetJavascriptAttributes();
    abstract protected void Save();
    abstract protected object PopulateBL(object obj);
}
