// File Name  : FeedbackDetailsUI.aspx.cs
// Created By : Milind
// Date       : 23/4/2009
//Description : This class is used to submit feedback.

using System;
using Utility;
using BusinessLogic.Exceptions;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;

public partial class FeedbackUI : SchoolBase
{   
    #region Events

    /// <summary>
    /// This event is used to fill all control related to user and feedback type.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FeedbackDetails1.bDisplay = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion
 }
