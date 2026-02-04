using System;
using System.Configuration;
using System.Reflection;
using BusinessLogic.Exceptions;
using Utility;

public partial class EnquiryFormThankYouPopup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
                if (iSchoolId != Constants.SchoolId.SNS.ToInt())
                    btnClose.Attributes.Add("onclick", "ClosePopup(); return false;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("EnquiryForm.aspx", false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}