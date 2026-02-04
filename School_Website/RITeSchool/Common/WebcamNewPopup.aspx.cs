using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using PhotoUploadEntities;
using SchoolWebApp;
using Utility;

public partial class WebcamNewPopup : SchoolBase
{
    #region Constants

    const string S_USER_PHOTO_UPLAD = "/RITeSchool/Common/UserRolewisePhotoUploadUI.aspx";
    const string S_TEACHER_PHOTO_UPLOAD = "/RITeSchool/Admin/TeacherPhotoUI.aspx";
    const string S_STUDENT_PHOTO_UPLOAD = "/RITeSchool/Teacher/StudentPhotoUploadUI.aspx";

    #endregion

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

    [WebMethod()]
    public static bool SaveCapturedImage(string data)
    {
        int UserId = 0;
        string fileName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");

        //Convert Base64 Encoded string to Byte Array.
        byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);

        ImageData oImageData = new ImageData();
        oImageData.UserID = UserId;
        oImageData.ImagesData = imageBytes;

        ImageDetails oImageDetails = new ImageDetails();
        oImageDetails.SetData(oImageData);

        //Save the Byte Array as Image File.
        //string filePath = HttpContext.Current.Server.MapPath(string.Format("~/Captures/{0}.jpg", fileName));
        //File.WriteAllBytes(filePath, imageBytes);
        return true;
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
            ImageDetails oImageDetails = new ImageDetails();
            var oImageData = oImageDetails.GetData();

            if (oImageData != null)
            {

                oImageData.UserID = Session[Constants.S_SESSION_USERS_ID].ToInt();
                SaveImageDatatoSession(oImageData);

                oImageDetails.ClearImage();
                if (HidPerentPage.Value == S_USER_PHOTO_UPLAD || HidPerentPage.Value == S_TEACHER_PHOTO_UPLOAD || HidPerentPage.Value == S_STUDENT_PHOTO_UPLOAD)
                {
                    if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && Session["IsPhotoCaptured"] != null)
                        Response.Write(string.Format("<Script language='Javascript'>window.opener.UpdateHiddenField({0});window.close();window.opener.focus(); </Script>", HidRowCount.Value));
                }
                else
                {
                    if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && Session["IsPhotoCaptured"] != null)
                    {
                        //Created Session to identify wether window close or button close identification which will used in DC .
                        Session[Constants.S_SESSION_IS_BUTTON_CLOSE] = "Submit";
                        Response.Write(string.Format("<Script language='Javascript'>window.opener.UpdateHiddenField();window.close();window.opener.focus(); </Script>"));
                    }                    
                }
            }
            else
                lblMessage.Text = "Please capture photo and then submit.";
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    private void SaveImageDatatoSession(ImageData oImageData)
    {
        Session["IsPhotoCaptured"] = "Captured";
        
        if (oImageData.ImagesData.Length > 2576)
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
}