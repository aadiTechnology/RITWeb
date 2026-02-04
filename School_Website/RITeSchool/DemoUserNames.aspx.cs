/* File Name :- DemoUserNames.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 18-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display login demo.
*/
using System;
using Utility;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class DemoUserNames : SchoolBase
{
    #region Events

    /// <summary>
    /// This event is used to set attributes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnClose.Attributes.Add("onclick", "closewindow()");
            ApplyMouseHoverEffect(new List<Button> { btnClose });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close current window.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Write("<Script type='text/javascript'>window.close();</Script>");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion
}