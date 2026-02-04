using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using Utility;

public partial class WebcamPopup : SchoolBase
{
#region Constants
 const string S_USER_PHOTO_UPLAD="/RITeSchool/Common/UserRolewisePhotoUploadUI.aspx";
 const string S_TEACHER_PHOTO_UPLOAD="/RITeSchool/Admin/TeacherPhotoUI.aspx";
 const string S_STUDENT_PHOTO_UPLOAD="/RITeSchool/Teacher/StudentPhotoUploadUI.aspx";

#endregion

#region Event

    protected void Page_Load(object sender, EventArgs e)
    {
			try
			{
				if (!IsPostBack)
				{
					Session[Constants.S_SESSION_USERS_ID] = QueryString["UserId"];
					ApplyMouseHoverEffect(new List<Button> { btnSubmit, btnClose });
					HidRowCount.Value = QueryString["RowNo"];
					HidPerentPage.Value = Page.Request.UrlReferrer.AbsolutePath;
					// this is to remove session 
					this.RemoveSession(Constants.S_SESSION_IS_BUTTON_CLOSE);
					this.RemoveSession("IsPhotoCaptured");

				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
    }
	
	/// <summary>
	/// This event is used to check session empty or not.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnSubmit_Click(object sender, EventArgs e)
	{
	  try
	  { 
	    if (HidPerentPage.Value == S_USER_PHOTO_UPLAD || HidPerentPage.Value == S_TEACHER_PHOTO_UPLOAD || HidPerentPage.Value == S_STUDENT_PHOTO_UPLOAD)
		   {
			   if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && Session["IsPhotoCaptured"]!=null)
			       Response.Write(string.Format("<Script language='Javascript'>window.opener.UpdateHiddenField({0});window.close();window.opener.focus(); </Script>",HidRowCount.Value));
				else
				   lblSessionEmptyCheck.Text = "Please capture photo and then submit.";
		  }
		 else
			{
				if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && Session["IsPhotoCaptured"] != null)
				{
				   //Created Session to identify wether window close or button close identification which will used in DC .
					Session[Constants.S_SESSION_IS_BUTTON_CLOSE] = "Submit";
					Response.Write(string.Format("<Script language='Javascript'>window.opener.UpdateHiddenField();window.close();window.opener.focus(); </Script>"));
					}
				else
					lblSessionEmptyCheck.Text = "Please capture photo and then submit.";
			}
	  }
	  catch (Exception ex)
	  {
		  ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
	  }

	}

	/// <summary>
	/// This event is to remove session
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnClose_Click(object sender, EventArgs e)
	{
	   try
	   {
		   if (HidPerentPage.Value == S_USER_PHOTO_UPLAD || HidPerentPage.Value == S_TEACHER_PHOTO_UPLOAD || HidPerentPage.Value == S_STUDENT_PHOTO_UPLOAD)
		     Response.Write(string.Format("<Script language='Javascript'>window.close();window.opener.focus(); </Script>"));
		   else
		   {
			   this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
			   Response.Write(string.Format("<Script language='Javascript'>window.close();window.opener.focus(); </Script>"));
		   }
	   }
	   catch (Exception ex)
	   {
		   ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
	   }

	}
#endregion
}