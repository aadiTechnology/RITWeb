using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class UploadPhotosUI : SchoolBase
{
    #region DataMember

    private UserRolewisePhotoUploadBL moTeacherPhotoDetails;
    
    #endregion

    #region Constant(s)
        
    private const string S_SUBMIT_Message = "Photo submitted successfully !!!";
    private const int I_HEIGHT_LIMIT = 151;
    private const int I_WIDTH_LIMIT = 112;
    private const int I_FILE_SIZE_LIMIT = 81920;
    private const string S_Parent_Photo = "RITeSchool\\DOWNLOADS\\Parent Photos\\";
    private const string S_Parent_Path = @"../DOWNLOADS/Parent Photos/";

    #endregion

    #region Events

    /// <summary>
    /// This Event is used for Fill All controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavaScriptAttributes();
                DisplayPhotos();                
                IsUserPhotoSaved();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit photo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            IsUserPhotoSaved();
            DisplayPhotos();
            lblTeacherName.Text = Session[Constants.S_SESSION_USER_FULLNAME].ToString();
            base.DisplayMessage(S_SUBMIT_Message, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Thie method is used to save teacher photo details.
    /// </summary>
    private void Save()
    {
        UserRolewisePhotoUploadBL oUserRolewisePhotoUploadBL = new UserRolewisePhotoUploadBL(miSchoolId, miAcademicYearId);
        oUserRolewisePhotoUploadBL.UserRolewisePhotoDetails.UserId = miUserId;
        oUserRolewisePhotoUploadBL.UserRolewisePhotoDetails.BinaryPhotoImage = GetByteArrayFromFileField(FuTeacherPhoto);
        oUserRolewisePhotoUploadBL.UserRolewisePhotoDetails.UserRoleId = moUserRole.ToInt();
        oUserRolewisePhotoUploadBL.Save();
    }

    /// <summary>
    /// This method is used to set button state.
    /// </summary>
    private void IsUserPhotoSaved()
    {
        UserRolewisePhotoUploadBL oUserRolewisePhotoDetails = new UserRolewisePhotoUploadBL(miSchoolId, miAcademicYearId);
        DataTable dtTeacher = oUserRolewisePhotoDetails.GetSubmitStatus(miUserId);

        if (dtTeacher.Rows.Count == 0)
            btnSubmit.Enabled = true;
        else if (dtTeacher.Rows[0][0] != DBNull.Value)
        {
            if (dtTeacher.Rows[0]["IsSubmitted"].ToBool())
                btnSubmit.Enabled = false;
            else
                btnSubmit.Enabled = true;
        }
    }

    /// <summary>
    /// This method is used save file on server 
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveFileOnServer(string asFileName, FileUpload oFileUpload)
    {
        // Upload the file to the server.
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName = CommonUtility.GetFileNameForRenaming(asFileName);
            sServerFilePath = sFolderName + sFileName;
        }

        oFileUpload.SaveAs(sServerFilePath);
        //string sErrMessage = ValidateFile(sServerFilePath);
        //if (sErrMessage.Equals(""))
        //{
        //    // delete exesting logo
        //    string sFileToDelete = Server.MapPath(".") + oFileUpload.FileName;
        //    if (File.Exists(sFileToDelete))
        //    {
        //        File.Delete(sFileToDelete);
        //    }
        //}
        //else
        //{
        //    File.Delete(sServerFilePath);
        //    throw new ApplicationException(sErrMessage);
        //}
        return sFileName;
    }

    /// <summary>
    /// This function is used to ValidateFile
    /// </summary>
    /// <param name="asServerFilePath"></param>
    /// <returns></returns>
    //private string ValidateFile(string asServerFilePath)
    //{
    //    string sReturnErrorMsg = "";
    //    bool bIsValid = true;
    //    if (File.Exists(asServerFilePath))
    //    {
    //        FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
    //        System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
    //        if (oImg.Height > I_HEIGHT_LIMIT && oImg.Width > I_WIDTH_LIMIT)
    //        {
    //            sReturnErrorMsg = Resources.LocalizedResources.PhotoHeightWidth + " " + I_HEIGHT_LIMIT + "px " + Resources.LocalizedResources.And + " " + I_WIDTH_LIMIT + "px " + Resources.LocalizedResources.respectively;
    //            bIsValid = false;
    //        }
    //        else
    //        {
    //            if (oImg.Height > I_HEIGHT_LIMIT)
    //            {
    //                sReturnErrorMsg = Resources.LocalizedResources.PhotoHeight + I_HEIGHT_LIMIT + "px." + Resources.LocalizedResources.Greater;
    //                bIsValid = false;
    //            }
    //            if (oImg.Width > I_WIDTH_LIMIT)
    //            {
    //                sReturnErrorMsg = Resources.LocalizedResources.PhotoWidth + I_WIDTH_LIMIT + "px." + Resources.LocalizedResources.Greater;
    //                bIsValid = false;
    //            }
    //        }
    //        oFileStream.Close();
    //        oImg = null;

    //    }
    //    FileInfo oFile = new FileInfo(asServerFilePath);
    //    if (oFile.Length > I_FILE_SIZE_LIMIT && bIsValid)
    //    {
    //        sReturnErrorMsg = Resources.LocalizedResources.SizePhotoVal;
    //        bIsValid = false;
    //    }
    //    oFile = null;
    //    return sReturnErrorMsg;
    //}
    
    /// <summary>
    /// This method is used to display user photo.
    /// </summary>
    private void DisplayPhotos()
    {
        lblTeacherName.Text = Session[Constants.S_SESSION_USER_FULLNAME].ToString();
        imgTeacherPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + miUserId + "&PhotoTypeId=1";
    }

    /// <summary>
    /// This method is used to set button states.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        base.ApplyMouseHoverEffect(new List<Button> { btnSubmit });
        btnSubmit.Attributes.Add("onclick","if(!ConfirmSubmit()) return false;");
    }

    #endregion    
}
