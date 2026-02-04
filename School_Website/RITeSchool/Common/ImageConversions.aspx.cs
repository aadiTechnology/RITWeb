using System;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic.Exceptions;
using PhotoUploadEntities;
using Utility;

public partial class ImageConversions : System.Web.UI.Page
{
    #region Event

    protected void Page_Load(object sender, EventArgs e)
    {
	try
        {
         SaveImageDatatoSession();
		}
		catch (Exception oEx)
		{
			ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
		}
    }

	#endregion

	#region Private Methods
	/// <summary>
	/// This method is used to create list of photo and used id captured by captured by webcam.
	/// </summary>
    private void SaveImageDatatoSession()
    {
			ImageData oImageData = new ImageData();
			oImageData.UserID = Convert.ToInt32(Session[Constants.S_SESSION_USERS_ID]) ;
			oImageData.ImagesData = Convert.FromBase64String(Request.Form["imageData"]);
			Session["IsPhotoCaptured"]="Captured";
			int iImagelegth=Request.Form["imageData"].Length;
			if(iImagelegth > 2576)
			{
			List<ImageData> lstUsers = null;
			if (Session[Constants.S_SESSION_USER_IMAGE_DATA] == null)
				lstUsers = new List<ImageData>();
			else
			{
				lstUsers = new List<ImageData>();
				lstUsers = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
			}
			lstUsers.Add(oImageData);
			Session[Constants.S_SESSION_USER_IMAGE_DATA] = lstUsers;
			}
			else
			{
			 Session.Remove(Constants.S_SESSION_USER_IMAGE_DATA);
			}
    }

	#endregion
}
