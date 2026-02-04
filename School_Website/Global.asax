<%@ Application Language="C#" %>
<%@ Import Namespace="Utility" %>

<script RunAt="server">

	void Application_Start(object sender, EventArgs e)
	{
		// Code that runs on application startup
		Constants.S_CONNECTION_STRING = string.Format("Data Source={0};User Id={1};password={2};initial catalog={3};MultipleActiveResultSets=True", 
			                                           ConfigurationManager.AppSettings["ReportingDataSource"], 
			                                           ConfigurationManager.AppSettings["ReportingUserId"], 
			                                           ConfigurationManager.AppSettings["ReportingPassword"], 
			                                           ConfigurationManager.AppSettings["ReportDataBaseName"]);

		SchoolBase.InitializeSettings();

		Constants.SENDMAIL = ConfigurationManager.AppSettings["SendMail"];
		Constants.S_IP_ADDRESS_SMTP = ConfigurationManager.AppSettings["IPAddress"];
		Constants.S_PORT_NUMBER_SMTP = ConfigurationManager.AppSettings["PortNumber"];
		Constants.S_STANDARD_DATE_FORMAT = SchoolBase.Settings.DateFormat; 
		Constants.S_STANDARD_GRID_TIME_FORMAT = SchoolBase.Settings.GridTimeFormat; 
		Constants.S_STANDARD_GRID_DATE_FORMAT = SchoolBase.Settings.GridDateFormat; 
		Constants.S_STANDARD_GRID_DATE_TIME_FORMAT = SchoolBase.Settings.GridDateTimeFormat;
		Constants.S_SEND_SMS = ConfigurationManager.AppSettings["SendSMS"];
		Constants.S_FROM_EMAIL_ADDRESS_OF_SITE_ADMIN = ConfigurationManager.AppSettings["FromMailAddress"];
		Constants.B_ACTIVITY_LOGGING = SchoolBase.Settings.ActivityLogging;
        Constants.B_SERVICE_LOGGING_ENABLED = SchoolBase.Settings.EnabledServiceActivityLogging;
        Constants.I_ACTIVITY_LOG_CACHE_COUNT = ConfigurationManager.AppSettings["ActivityLogCacheCount"].ToInt();
	}

	void Application_End(object sender, EventArgs e)
	{
		//  Code that runs on application shutdown

	}

	void Application_Error(object sender, EventArgs e)
	{
		// Code that runs when an unhandled error occurs
		if (Server.GetLastError().InnerException != null && Server.GetLastError().InnerException.Message.Contains("Maximum request length exceeded."))
			HttpContext.Current.Response.Redirect("../Common/Error.aspx?" + CommonUtility.EncryptQuerystring("ErrorMessage=File size exceeded. Each file can have maximum size of 250 KB."));
	}

	void Session_Start(object sender, EventArgs e)
	{
		// Code that runs when a new session is started
        //Session.Add(Constants.S_SESSION_LANGUAGE, System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
        Session.Add(Constants.S_SESSION_LANGUAGE, ConfigurationManager.AppSettings["DefaultCulture"]);
	}

	void Session_End(object sender, EventArgs e)
	{
		// Code that runs when a session ends. 
		// Note: The Session_End event is raised only when the session state mode
		// is set to InProc in the Web.config file. If session mode is set to StateServer 
		// or SQLServer, the event is not raised.
		if (HttpContext.Current != null && HttpContext.Current.Response != null)
			HttpContext.Current.Response.Redirect("Common/Error.aspx");

	}
	public void Application_AuthenticateRequest(Object src, EventArgs e)
	{
		HttpApplication oApplication = (HttpApplication)src;
		HttpContext oContext = oApplication.Context;
		if (!(oContext.User == null))
		{
			if (oContext.User.Identity.AuthenticationType == "Forms" && oContext.Request.IsAuthenticated == true)
			{
				System.Web.Security.FormsIdentity id;
				id = (System.Web.Security.FormsIdentity)oContext.User.Identity;
				FormsAuthenticationTicket ticket = id.Ticket;
				string userData = ticket.UserData;
				string[] roles = userData.Split(',');
				HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
			}
		}
	}
    /// <summary>
    /// This PreInit Event raise befor PreInit event if any page. so we can chage the theme from here.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

	void Application_PreRequestHandlerExecute(object sender, EventArgs e)
	{

		HttpApplication app = sender as HttpApplication;
		if (app == null) return;
		Page pActivepage = app.Context.Handler as Page;
		if (pActivepage == null) return;
		pActivepage.PreInit += pActivepage_PreInit;
		if (Request.Cookies["UserTheme"] != null)
			pActivepage.Theme = Server.HtmlEncode(Request.Cookies["UserTheme"].Value);

	}

	public void pActivepage_PreInit(object sender, EventArgs e)
	{
		Page pActivepage = sender as Page;
		if (Request.Cookies["UserTheme"] != null)
			pActivepage.Theme = Server.HtmlEncode(Request.Cookies["UserTheme"].Value);
	}

</script>
