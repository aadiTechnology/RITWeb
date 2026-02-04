using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using Utility;

public partial class LoginFromBetaPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (!Request.QueryString.ToString().IsNullOrEmpty())
                {
                    if (Request.QueryString != null && Request.QueryString.ToString() != string.Empty)
                    {
                        string sDecryptedQueryString = CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString()));
                        NameValueCollection QueryString = HttpUtility.ParseQueryString(sDecryptedQueryString);

                        if (QueryString["Key"] != null && QueryString["Key"].ToString() == ConfigurationManager.AppSettings["BetaWebsiteKey"].ToString())
                        {
                            int iSchoolId = 0;
                            if (QueryString["SchoolId"] != null && QueryString["SchoolId"].ToString() != string.Empty)
                                iSchoolId = QueryString["SchoolId"].ToInt();

                            string sUserName = string.Empty;
                            if (QueryString["UserName"] != null)
                                sUserName = QueryString["UserName"].ToString();

                            string sPassword = string.Empty;
                            if (QueryString["UserPassword"] != null)
                                sPassword = QueryString["UserPassword"].ToString();

                            string sEncPassword = string.Empty;
                            string sKeyName = "UserEncPassword";
                            if (QueryString[sKeyName] != null)
                            {
                                string[] sData = sDecryptedQueryString.Split('&');
                                foreach (var part in sData)
                                {
                                    if (part.StartsWith(sKeyName+"="))
                                    {
                                        sEncPassword = part.Substring((sKeyName+"=").Length);
                                        break;
                                    }
                                }
        
                                sPassword = CommonUtility.GetDecryptedPassword(sUserName, sEncPassword);
                            }

                            var oUserAuthentication = new UserAuthentication(iSchoolId, sUserName, sPassword, string.Empty);
                            if (oUserAuthentication.ValidUser)
                            {
                                oUserAuthentication.UpdateSession();
                                int iWidth;
                                bool bIsWidth = Int32.TryParse(hidScreenWidth.Value, out iWidth);
                                Session.Add(Constants.S_SESSION_SCREEN_WIDTH, bIsWidth ? iWidth : 1024);
                                Response.Redirect("RITeSchool/Common/ControlPanel.aspx", false);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
}