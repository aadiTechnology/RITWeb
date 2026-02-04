/*
 * File Name - UploadStudentPhotoUI.aspx.cs
 * Created By - Vishakha
 * Created Date - 01 dec 2022
 * Descrption - This class is used to upload Student Photo.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using PhotoUploadEntities;
using System.Linq;
using System.Web;

public partial class UploadStudentPhotoUI : SchoolBase
{
    #region Constants

    private const string S_DELETE_MSG = "Student photo deleted successfully.";
    private const string S_SAVED_MSG = "Student photo uploaded successfully.";
    private const string S_SUMBITTED_MSG = "Student photo Submitted successfully.";
    private const string S_DEFAULT_PHOTO = "~/RITeSchool/images/Student_BlankPh.jpg";

    #endregion

    #region Data Member(s)

    private UploadStudentPhotoBL moUploadStudentPhotoBL;
   
    #endregion
    
    #region Event(s)
    /// <summary>
    /// This event is used to set default values, fill documents in listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moUploadStudentPhotoBL = new UploadStudentPhotoBL(miSchoolId,miAcademicYearId,miUserId);
            if (!IsPostBack)
            {
                SetFields();
                SetDefaultValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Student uploaded photo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            SetFields();
            lblMessage.Text = S_SAVED_MSG;
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
        catch (ApplicationException ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    /// <summary>
    /// This event is used to Delete saving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {            
            Delete();
            SetFields();
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
         }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit student uploaded photo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {            
            Submit();
            SetFields();
            this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

   #endregion

    #region Method(s)

    /// <summary>
    /// This method used to get student name for label.
    /// </summary>
    private void SetFields()
    {
        if (Settings.AllowStudentPhotoUploadFromStudentLogin)
        {
            int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
            StudentPhotoUploadDetails oGetStudentNameForLabel = moUploadStudentPhotoBL.GetStudentPhotoUploadDetails(iStudentId);

            lblStudentName.Text = oGetStudentNameForLabel.StudentName;
            hidStudentId.Value = oGetStudentNameForLabel.SchoolwiseStudentId.ToString();

            if (oGetStudentNameForLabel.IsSaved)
            {
                if (oGetStudentNameForLabel.IsSubmitted)
                {
                    BtnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSubmit.Enabled = false;
                }
                else
                {
                    BtnSave.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSubmit.Enabled = true;
                }
            }
            else
            {
                BtnSave.Enabled = true;
                btnDelete.Enabled = false;
                btnSubmit.Enabled = false;
            }

            if (oGetStudentNameForLabel.IsOldPhotoExist)
                imgExistingPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + miUserId;
            else
                imgExistingPhoto.Src = S_DEFAULT_PHOTO;

            if (oGetStudentNameForLabel.PhotoImage != null)
                imgPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])oGetStudentNameForLabel.PhotoImage);
            else
                imgPhoto.ImageUrl = S_DEFAULT_PHOTO;
        }
        else
        {
            BtnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnSubmit.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to set default values to fields.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnDelete, btnSubmit });
        BtnSave.Attributes.Add("onclick", "ResetMessage();");        
        string sQueryString = "UserId=" + hidStudentId.Value;
        ImgWebCam.Attributes.Add("Onclick", "OpenWebcamPopup('" + CommonUtility.EncryptQuerystring(sQueryString) + "');return false;");
        btnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
        btnSubmit.Attributes.Add("onclick", "if(!ConfirmSubmit()) return false;");
        FileUploadLogo.Focus();
    }

    /// <summary>
    /// This method is used to delete student photo.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void Delete()
    {
        int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        moUploadStudentPhotoBL.Delete(iStudentId);
        lblMessage.Text = S_DELETE_MSG;
    }

    /// <summary>
    /// This method is used to submit student photo.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void Submit()
    {
        int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        moUploadStudentPhotoBL.Submit(iStudentId);
        lblMessage.Text = S_SUMBITTED_MSG;
    }
    
    /// <summary>
    /// This method is used to remove session.
    /// </summary>
    /// <param name="asSessionName"></param>
    public void RemoveSession(string asSessionName)
    {
        if (HttpContext.Current.Session[asSessionName] != null)
            HttpContext.Current.Session.Remove(asSessionName);
    }

    /// <summary>
    /// This method is used to save student photo.
    /// </summary>
    private void Save()
    {
        SavePhotoFile oSavePhotoFile = new SavePhotoFile();
        if (FileUploadLogo.HasFile)
        {
            string sMsg = ValidatePhotoFile();
            if (sMsg == string.Empty)
                oSavePhotoFile.PhotoFilePathInBinary = GetByteArrayFromFileField(FileUploadLogo);
            else
                throw new ApplicationException(sMsg);
        }
        else
        {
            if (Session[Constants.S_SESSION_USER_IMAGE_DATA] != null && hidIsPhotoCaptured.Value == Constants.S_YES)
            {
                List<ImageData> lstImageData = (List<ImageData>)Session[Constants.S_SESSION_USER_IMAGE_DATA];
                var oImage = lstImageData.Where(lst => lst.UserID == hidStudentId.Value.ToInt()).LastOrDefault();
                if (!oImage.IsNull())
                    oSavePhotoFile.PhotoFilePathInBinary = oImage.ImagesData;
            }
        }

        oSavePhotoFile.StudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();

        moUploadStudentPhotoBL.Save(oSavePhotoFile);
    }

    private string ValidatePhotoFile()
    {
        int I_HEIGHT_LIMIT = 151;
        int I_WIDTH_LIMIT = 112;
        string sReturnErrorMsg = "";

        if (FileUploadLogo.HasFile)
        {
            string sServerPath = Server.MapPath("~");
            sServerPath = sServerPath + "\\RITeSchool\\Downloads\\StudentPhotos\\" + FileUploadLogo.FileName;

            if (File.Exists(sServerPath))
                sServerPath = sServerPath + "\\Downloads\\StudentPhotos\\" + CommonUtility.GetFileNameForRenaming(FileUploadLogo.FileName);

            FileUploadLogo.SaveAs(sServerPath);

            if (File.Exists(sServerPath))
            {
                FileStream oFileStream = new FileStream(sServerPath, FileMode.Open);
                System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
                if (oImg.Height > I_HEIGHT_LIMIT && oImg.Width > I_WIDTH_LIMIT)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoHeightWidth + " " + I_HEIGHT_LIMIT + "px " + Resources.LocalizedResources.And + " " + I_WIDTH_LIMIT + "px " + Resources.LocalizedResources.respectively;
                }
                else
                {
                    if (oImg.Height > I_HEIGHT_LIMIT)
                    {
                        sReturnErrorMsg = Resources.LocalizedResources.PhotoHeight + I_HEIGHT_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    }
                    if (oImg.Width > I_WIDTH_LIMIT)
                    {
                        sReturnErrorMsg = Resources.LocalizedResources.PhotoWidth + I_WIDTH_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    }
                }
                oFileStream.Close();
                oImg = null;

            }

            if (File.Exists(sServerPath))
                File.Delete(sServerPath);
        }

        return sReturnErrorMsg;
    }

    #endregion
}
