// File Name    : WeekDaysConfiguration.aspx   
// Moodifies By : Amit
// Created Date : 24/9/2009
// Description  : This class is used to show session expire message.

using System;
using System.Collections.Generic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Web.UI.WebControls;

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
            bool bExpireSession = true;
            ApplyMouseHoverEffect(new List<Button> { btnLogin }); 
            if (!String.IsNullOrEmpty(Server.UrlDecode(Request.QueryString.ToString())))
            {
                if (!QueryString["Is_Session_Shared"].IsNull() && QueryString["Is_Session_Shared"] == Constants.C_YES.ToString())
                    lblPageHeader.Text = "Your session is shared with another user.";
                
                if (!QueryString["ErrorMessage"].IsNull())
                {
                    bExpireSession = false;
                    lblPageHeader.Text = "File size exceeded.";
                    lblLogin.Text = string.Empty;
                    lblNavigateToDashboard.Visible = true;
                }

                if (QueryString["AccessRestriction"] != null)
                {
                    bExpireSession = false;
                    lblPageHeader.Text = "Access Denied.";
                    lblLogin.Text = string.Empty;
                    lblNavigateToDashboard.Visible = true;
                }

                if (QueryString["IsFinancialYearShared"] != null && QueryString["IsFinancialYearShared"].ToString() == Constants.S_YES)
                {
                    bExpireSession = false;
                    
                    lblLogin.Text = string.Empty;
                    lblNavigateToDashboard.Visible = true;

                    if (QueryString["ShowLink"] != null && QueryString["ShowLink"].ToString() == Constants.S_YES)
                    {
                        lblPageHeader.Text = "Financial year session is shared.";
                        lblNavigateToDashboard.Visible = true;
                    }
                    else
                    {
                        lblNavigateToDashboard.Visible = false;
                        lblPageHeader.Text = "Financial year session is shared. Please close this window.";
                    }
                }                
            }

            if (bExpireSession)
            {
                Session.Abandon();
                lblNavigateToDashboard.Visible = false;
            }

            tdLogin.Visible = bExpireSession;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
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
            Response.Redirect("~/eSchoolLogin.aspx", false);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion " Events "

}
