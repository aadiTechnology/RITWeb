/* File Name :- UploadParentPhotosUI.aspx.cs
 * Created Date :- 16-Aug-2018
 * Class Description :- This class is used to Capture and save Student Parent like Mother, father, relative photo details. 
 * Created By :- Dnyaneshwar Shinde.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using SchoolEntities;
using BusinessLogic;
using Utility;
using System.Data;
using System.IO;

public partial class UploadParentPhotosUI : SchoolBase
{

    #region DataMember

    private StudentBL moStudentBL;

    #endregion

    #region Constant(s)

    private const string S_SAVE_MESSAGE = "Recoreds saved successfully !!!";
    private const string S_SUBMIT_Message = "Details submeeted successfully !!!";
    private const string S_ATLIST_ONE_MESSAGE = "At least one photo should be selecetd.";
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
            moStudentBL = new StudentBL();
            if (!IsPostBack)
            {
                DisplayGuardianDetails();
                DisplayPhotos();
                btnSave.Attributes.Add("Onclick", "CheckForSiblingDetails()");
                btnSubmit.Attributes.Add("Onclick", "SubmitDataForSibling()");


                bool bIsTransportAssociated = DisplayTransportPickUpPersonDetails();
                if (Settings.AllowExternalTransport && bIsTransportAssociated)
                {   
                    DisplayTransportPickUpPersonPhoto();
                    btnSaveTransport.Attributes.Add("Onclick", "CheckForSiblingDetails()");
                    btnsubmitTransport.Attributes.Add("Onclick", "SubmitDataForSibling()");
                }
                else
                    trTransportPickUp.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used for Save the Details in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveParentPhotoDetails();
            DisplayGuardianDetails();
            DisplayPhotos();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for submit the saved data.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool bSubmitSiblingDetails = false;
            if (hidSubmitSiblingDetails.Value == Constants.S_ONE)
                bSubmitSiblingDetails = true;
            moStudentBL.SubmitStudentParentPhotoDetails(miUserId, miSchoolId, miAcademicYearId, bSubmitSiblingDetails);
            DisplayGuardianDetails();
            DisplayPhotos();
            base.DisplayMessage(S_SUBMIT_Message, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used for Save the Transport PickUp person Details in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveTransport_Click(object sender, EventArgs e)
    {
        try
        {
            SaveTransportPersonPhotoDetails();
            DisplayTransportPickUpPersonDetails();
            DisplayTransportPickUpPersonPhoto();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
        }
        catch (ApplicationException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Event is used for submit Transport PickUp person Details in database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnsubmitTransport_Click(object sender, EventArgs e)
    {
        try
        {
            bool bSubmitSiblingDetails = false;
            if (hidSubmitSiblingDetails.Value == Constants.S_ONE)
                bSubmitSiblingDetails = true;

            moStudentBL.SubmitTransportPickUpPersonPhotoDetails(miUserId, miSchoolId, miAcademicYearId, bSubmitSiblingDetails);
            DisplayTransportPickUpPersonDetails();
            DisplayTransportPickUpPersonPhoto();
            base.DisplayMessage(S_SUBMIT_Message, false, tdMessage);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This method is used to display Parent names on screen.
    /// </summary>
    private void DisplayGuardianDetails()
    {
        DataTable dtParentDetails = moStudentBL.GetStudentParentPhotoDetails(miSchoolId, miAcademicYearId, miUserId);

        if (dtParentDetails.Rows.Count > Constants.I_ZERO)
        {
            lblFatherName.Text = dtParentDetails.Rows[0]["FatherName"].ToString();
            lblMotherName.Text = dtParentDetails.Rows[0]["MotherName"].ToString();
            txtRelativeName.Text = dtParentDetails.Rows[0]["ParentName"].ToString();
            bool bIsPhotosSubmitted = dtParentDetails.Rows[0]["IsPhotosSubmitted"].ToBool();
            bool bIsAllPhotosSaved = dtParentDetails.Rows[0]["IsAllPhotoSaved"].ToBool();
            hidIsSibling.Value  = dtParentDetails.Rows[0]["IsSiblingPresent"].ToString();

            if (dtParentDetails.Rows[0]["IsFatherPhotoSaved"].ToBool())
                hidIsFatherPhotoSaved.Value = Constants.S_ONE;
            else
                hidIsFatherPhotoSaved.Value = Constants.S_ZERO;

            if (dtParentDetails.Rows[0]["IsMotherPhotoSaved"].ToBool())
                hidIsMotherPhotoSaved.Value = Constants.S_ONE;
            else
                hidIsMotherPhotoSaved.Value = Constants.S_ZERO;

            if (dtParentDetails.Rows[0]["IsParentPhotoSaved"].ToBool())
                hidIsParentPhotoSaved.Value = Constants.S_ONE;
            else
                hidIsParentPhotoSaved.Value = Constants.S_ZERO;          
            
            if (bIsAllPhotosSaved)
                btnSubmit.Enabled = true;
            else
                btnSubmit.Enabled = false;

            if (bIsPhotosSubmitted)
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
                fuFatherPhoto.Enabled = false;
                fuMotherPhoto.Enabled = false;
                fuParentPhoto.Enabled = false;
                txtRelativeName.Enabled = false;
            }
        }
    }

    private bool DisplayTransportPickUpPersonDetails()
    {
        DataTable dtParentDetails = moStudentBL.GetTransportPickUpPersonPhotoDetails(miSchoolId, miAcademicYearId, miUserId);

        bool bIsTransportAssociated = dtParentDetails.Rows[0]["IsTransportAssociated"].ToBool();

        if (bIsTransportAssociated && Settings.AllowExternalTransport)
        {
            trTransportPickUp.Visible = true;
            if (dtParentDetails.Rows.Count > Constants.I_ZERO)
            {
                bool bIsPhotosSubmitted = dtParentDetails.Rows[0]["IsPhotosSubmitted"].ToBool();
                bool bIsAllPhotosSaved = dtParentDetails.Rows[0]["IsAllPhotoSaved"].ToBool();
                hidIsSibling.Value = dtParentDetails.Rows[0]["IsSiblingPresent"].ToString();
                txtTransportPickUpPerson.Text = dtParentDetails.Rows[0]["TransportPickUpPersonName"].ToString();

                if (dtParentDetails.Rows[0]["IsTransportPersonPhotosaved"].ToBool())
                    hidIsTransportPickUpPersonPhotosaved.Value = Constants.S_ONE;
                else
                    hidIsTransportPickUpPersonPhotosaved.Value = Constants.S_ZERO;

                if (bIsAllPhotosSaved)
                    btnsubmitTransport.Enabled = true;
                else
                    btnsubmitTransport.Enabled = false;

                if (bIsPhotosSubmitted)
                {
                    btnSaveTransport.Enabled = false;
                    btnsubmitTransport.Enabled = false;
                    txtTransportPickUpPerson.Enabled = false;
                    fuTransportPersonPhoto.Enabled = false;
                }
            }
        }
        return bIsTransportAssociated;
    }

    private void SaveTransportPersonPhotoDetails()
    {
        StudentAdditionalDetails oStudentAdditionalDetails = new StudentAdditionalDetails();
        moStudentBL = new StudentBL();
        string sTransportPersonName = string.Empty;

        if (fuTransportPersonPhoto.HasFile)
        {
            SaveFileOnServer(fuTransportPersonPhoto.FileName, fuTransportPersonPhoto);
            oStudentAdditionalDetails.TransportPickUpPersonBinartPhoto = GetByteArrayFromFileField(fuTransportPersonPhoto);
            sTransportPersonName = CheckIsPhotosUploaded(fuTransportPersonPhoto);
        }
        else
        {
            oStudentAdditionalDetails.TransportPickUpPersonBinartPhoto = null;
            sTransportPersonName = string.Empty;
        }

        string sSaveforSibling = hidSaveSiblingDetails.Value.ToString();
        bool bSaveSibling = false;
        if (sSaveforSibling == Constants.S_ONE)
            bSaveSibling = true;

        oStudentAdditionalDetails.TransportPickUpPersonPhoto = sTransportPersonName;
        oStudentAdditionalDetails.TransportPickUpPersonName = txtTransportPickUpPerson.Text;
        moStudentBL.SaveTransportPickUpPersonPhotoDetails(oStudentAdditionalDetails, miSchoolId, miUserId, bSaveSibling, miAcademicYearId);
    }

    /// <summary>
    /// This method is used to Save Parent photo details in table.
    /// </summary>
    private void SaveParentPhotoDetails()
    {
        StudentAdditionalDetails oStudentAdditionalDetails = new StudentAdditionalDetails();
        moStudentBL = new StudentBL();
        string sFatherPhoto = string.Empty;
        string sMotherPhoto = string.Empty;
        string sRelativePhoto = string.Empty;   

        if (fuFatherPhoto.HasFile)
        {
            SaveFileOnServer(fuFatherPhoto.FileName, fuFatherPhoto);
            oStudentAdditionalDetails.FatherBinaryPhoto = GetByteArrayFromFileField(fuFatherPhoto);
            sFatherPhoto = CheckIsPhotosUploaded(fuFatherPhoto);
        }
        else
        {
            oStudentAdditionalDetails.FatherBinaryPhoto = null;
            sFatherPhoto = string.Empty;
        }
        if (fuMotherPhoto.HasFile)
        {
            SaveFileOnServer(fuMotherPhoto.FileName, fuMotherPhoto);
            oStudentAdditionalDetails.MotherBinaryPhoto = GetByteArrayFromFileField(fuMotherPhoto);
            sMotherPhoto = CheckIsPhotosUploaded(fuMotherPhoto);
        }
        else
        {
            oStudentAdditionalDetails.MotherBinaryPhoto = null;
            sMotherPhoto = string.Empty;
        }
        if (fuParentPhoto.HasFile)
        {
            SaveFileOnServer(fuParentPhoto.FileName, fuParentPhoto);
            oStudentAdditionalDetails.ParentBinaryPhoto = GetByteArrayFromFileField(fuParentPhoto);
            sRelativePhoto = CheckIsPhotosUploaded(fuParentPhoto);
        }
        else
        {
            oStudentAdditionalDetails.ParentBinaryPhoto = null;
            sRelativePhoto = string.Empty;
        }      

        oStudentAdditionalDetails.FatherPhoto = sFatherPhoto;
        oStudentAdditionalDetails.MotherPhoto = sMotherPhoto;
        oStudentAdditionalDetails.GuardianPhoto = sRelativePhoto;
        oStudentAdditionalDetails.RelativeName = txtRelativeName.Text;
        string sSaveforSibling = hidSaveSiblingDetails.Value.ToString();
        bool bSaveSibling = false;
        if (sSaveforSibling == Constants.S_ONE)
            bSaveSibling = true;
        
        oStudentAdditionalDetails.TransportPickUpPersonName = txtTransportPickUpPerson.Text;

        moStudentBL.SaveStudentParentPhotoDetails(oStudentAdditionalDetails, miSchoolId, miUserId, bSaveSibling, miAcademicYearId);

    }

    /// <summary>
    /// This method is used to set values to image controls.
    /// </summary>
    private void DisplayPhotos()
    {
        imgFatherPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + miUserId + "&PhotoTypeId=1";
        imgMotherPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + miUserId + "&PhotoTypeId=2";
        imgParentPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + miUserId + "&PhotoTypeId=3";        
    }

    /// <summary>
    /// This method is used to set values to image controls.
    /// </summary>
    private void DisplayTransportPickUpPersonPhoto()
    {
        imgTransportPerson.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value=" + miUserId + "&PhotoTypeId=4";
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
        string sErrMessage = ValidateFile(sServerFilePath);
        if (sErrMessage.Equals(""))
        {
            // delete exesting logo
            string sFileToDelete = Server.MapPath(".") + oFileUpload.FileName;
            if (File.Exists(sFileToDelete))
            {
                File.Delete(sFileToDelete);
            }
        }
        else
        {
            File.Delete(sServerFilePath);
            throw new ApplicationException(sErrMessage);
        }
        return sFileName;
    }

    /// <summary>
    /// This function is used to ValidateFile
    /// </summary>
    /// <param name="asServerFilePath"></param>
    /// <returns></returns>
    private string ValidateFile(string asServerFilePath)
    {
        string sReturnErrorMsg = "";
        bool bIsValid = true;
        if (File.Exists(asServerFilePath))
        {
            FileStream oFileStream = new FileStream(asServerFilePath, FileMode.Open);
            System.Drawing.Image oImg = System.Drawing.Image.FromStream(oFileStream);
            if (oImg.Height > I_HEIGHT_LIMIT && oImg.Width > I_WIDTH_LIMIT)
            {
                sReturnErrorMsg = Resources.LocalizedResources.PhotoHeightWidth + " " + I_HEIGHT_LIMIT + "px " + Resources.LocalizedResources.And + " " + I_WIDTH_LIMIT + "px " + Resources.LocalizedResources.respectively;
                bIsValid = false;
            }
            else
            {
                if (oImg.Height > I_HEIGHT_LIMIT)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoHeight + I_HEIGHT_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    bIsValid = false;
                }
                if (oImg.Width > I_WIDTH_LIMIT)
                {
                    sReturnErrorMsg = Resources.LocalizedResources.PhotoWidth + I_WIDTH_LIMIT + "px." + Resources.LocalizedResources.Greater;
                    bIsValid = false;
                }
            }
            oFileStream.Close();
            oImg = null;

        }
        FileInfo oFile = new FileInfo(asServerFilePath);
        if (oFile.Length > I_FILE_SIZE_LIMIT && bIsValid)
        {
            sReturnErrorMsg = Resources.LocalizedResources.SizePhotoVal;
            bIsValid = false;
        }
        oFile = null;
        return sReturnErrorMsg;
    }

    private string CheckIsPhotosUploaded(FileUpload oFileUploadControl)
    {
        if (oFileUploadControl.FileName != string.Empty)
        {
            string sLinkFamilyName = string.Empty;            
            string sServerPath = Server.MapPath("~");
            if (sServerPath.Substring(sServerPath.Length - 1) != "\\")
                sServerPath = sServerPath + "\\";
            sLinkFamilyName = CommonUtility.GetFileNameForRenaming(oFileUploadControl.FileName.ToString());
            if (oFileUploadControl.HasFile)
            {
                string sFamilyName = oFileUploadControl.PostedFile.FileName;
                string sLinkFamilyPath = sServerPath + S_Parent_Photo + sLinkFamilyName;
                oFileUploadControl.SaveAs(sLinkFamilyPath);
                return sLinkFamilyName;
            }
        }       

        return string.Empty;
    }

    #endregion    
}