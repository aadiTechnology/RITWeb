// File Name    : WeekDaysConfiguration.aspx   
// Moodifies By : Amit
// Created Date : 24/9/2009
// Description  : This class is used to show session expire message.

using System;
using Utility;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class Error : SchoolBase
{
    #region " Events "

    /// <summary>
    /// This event is used to destroy all session varaibles and set java script to control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ApplyMouseHoverEffect(new List<Button> { btnLogin, btnLogin });
            Session.Abandon();
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move login page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            //Server.Transfer("OnlineAdmissionlLoginUI.aspx", false);
            Response.Redirect("~/RITeSchool/Admission/OnlineAdmissionlLoginUI.aspx", false);
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion " Events "
}
