using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

public partial class RITeSchool_SingleSignOnPage : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                SchoolBL oSchoolBL = new SchoolBL();
                // Decleared authentication key same as school settings key
                string sAuthenticationKeyForMobile = "MobileAuthenticationKey";

                int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
                string sUserLogin = QueryString["UserLogin"];
                string sAuthenticationValueFromMobileApp = QueryString["MobileAuthenticationKeyValue"];

                string sAuthenticationValueFromSettings = oSchoolBL.GetSchoolSettingByName(iSchoolId, sAuthenticationKeyForMobile);

                if (sUserLogin != null && sAuthenticationValueFromMobileApp.Equals(sAuthenticationValueFromSettings))
                {
                    UpdateSessionVariableAndRedirectToNextPage(iSchoolId, sUserLogin);
                    Response.Redirect(QueryString["URL"], false);
                    HttpContext.Current.Session[Constants.S_SESSION_IS_LOGIN_FROM_MOBILE] = true;
                    HttpContext.Current.Session[Constants.S_SESSION_MOBILE_PAY_FEE_POSTBACKURL] = QueryString["URL"];
                }
                else
                {
                    tblWarnning.Visible = true;
                }
            }
            else
            {
                tblWarnning.Visible = true;
            }
        }
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
    }
}