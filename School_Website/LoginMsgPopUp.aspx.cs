using System;
using System.Configuration;
using System.Reflection;
using BusinessLogic.Exceptions;
using Resources;
using Utility;

public partial class LoginMsgPopUp : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
	        this.Page.Title = Constants.S_TITLE_FOR_PAGE;
			if (QueryString["IsErrorMsg"] != null && (QueryString["IsErrorMsg"].Equals("1") && ConfigurationManager.AppSettings["IsLoginAllowd"] == Constants.S_NO))
				lblMsg.Text = Settings.LoginNotAllowdMsg;
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}
