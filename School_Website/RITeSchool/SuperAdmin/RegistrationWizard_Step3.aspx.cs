// File Name     : RegistrationWizard_Step3.aspx.cs
// Modified By   : Amit 
// Modified Date : 25/09/2009
// Description   : This class is used to show succesful registration message.

using System;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class RegistrationWizard_Step3 : SchoolBase
{
    /// <summary>
    /// This event is used to set default properties of page controls for succesful registration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                LblThankyou.Text = "Thank you for registering with " + Constants.S_SITE_NAME + ".";
                lblMessage.Text = " Your school will be activated after your address verification.<BR/><BR/>For more details, contact us at <a href=mailto:" + Constants.S_EMAIL_ADDRESS_OF_SITE_ADMIN + ">" + Constants.S_EMAIL_ADDRESS_OF_SITE_ADMIN + "</a> <BR/><BR/> OR  Call us on <BR/><BR/> Phone:  &nbsp;" +
                                    Constants.S_SITE_PHONE_NO + " <BR/><BR/> Mobile: &nbsp;" + Constants.S_SITE_MOBILE_NO;
                ApplyMouseHoverEffect(new List<Button> { btnOk });
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move back on dashboard screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
            {
                SuperAdminMasterPage  oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; 
                oSuperAdminMasterPage.RedirectToNextPage("../SuperAdmin/ScreensUI.aspx");
            }
            else
            {
                SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; 
                oSuperAdminMasterPage.RedirectToNextPage("../Home.aspx");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
}


