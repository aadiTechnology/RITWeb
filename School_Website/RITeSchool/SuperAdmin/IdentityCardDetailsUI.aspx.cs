using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using BusinessLogic;
using System.IO;
using BusinessLogic.Exceptions;
using System.Reflection;



public partial class IdentityCardDetailsUI :SchoolBase
{
    #region "Constants"
    private const string S_UPLOAD_IMAGE_PATH_FOR_ID = "\\images\\IdentityCardImages\\";
    public const string S_VALIDATION_SUMMARY_HEADER = "Please fix following error(s):";
    public const string S_URL = "~/RITeSchool/superadmin/ScreensUI.aspx";
    #endregion

    #region "Events"
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                InitializeFields();
                SetJavaScriptAttribute();
                valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            }
 
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolBL oSchoolDetails = new SchoolBL();
            oSchoolDetails.SchoolId = miSchoolId;
            oSchoolDetails.Address = txtAddress.Text.Trim();

            if (UploadLogo.HasFile)
            {
                string sFileName = SaveFileOnServer(UploadLogo.FileName);
                oSchoolDetails.LogoPath = Constants.S_UPLOAD_IMAGE_FOLDER_PATH + sFileName;
            }
            else
                oSchoolDetails.LogoPath = hidFilePath.Value;
            if (UploadLogo.HasFile)
            {
                Byte[] ImageBinaryData = this.GetByteArrayFromFileFieldDetails(UploadLogo);
                oSchoolDetails.UpdateSchoolLogo(ImageBinaryData);
            }
            if (UploadSign.HasFile)
            {
                string sFileName = SaveFileOnServerForSign(UploadSign.FileName);
                oSchoolDetails.SignLogo = S_UPLOAD_IMAGE_PATH_FOR_ID + sFileName;
                Byte[] ImageBinaryData = this.GetByteArrayFromFileFieldDetails(UploadSign);
                oSchoolDetails.UpdatePrincipalSignatureLogo(ImageBinaryData);
            }
            else
                oSchoolDetails.SignLogo = hidSignPath.Value;

            if (UploadICard.HasFile)
            {
                string sFileName = SaveFileOnServerForICardLogo(UploadICard.FileName);
                oSchoolDetails.ICardLogo = S_UPLOAD_IMAGE_PATH_FOR_ID + sFileName;
                Byte[] ImageBinaryData = this.GetByteArrayFromFileFieldDetails(UploadICard);
                oSchoolDetails.UpdateICardLogo(ImageBinaryData);
            }
            else
                oSchoolDetails.ICardLogo = hidICardPath.Value;

            oSchoolDetails.UpdatePrincipalSignAndIcardDetails();
            SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master;
            oSuperAdminMasterPage.RedirectToNextPage(S_URL);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    
    #endregion
    #region Private Method
    /// <summary>
    /// This method is used to save principle sign on the server.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveFileOnServerForSign(string asFileName)
    {
        // Upload the file to the server.
        // DeleteFiles();
        string sFolderName = Server.MapPath("..") + S_UPLOAD_IMAGE_PATH_FOR_ID;
        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName =CommonUtility.GetFileNameForRenaming(asFileName, "Principle Sign");
            sServerFilePath = sFolderName + sFileName;
        }
        string sFileToDelete = Server.MapPath(".") + hidSignPath.Value;
        if (File.Exists(sFileToDelete))
            File.Delete(sFileToDelete);
        UploadSign.SaveAs(sServerFilePath);
        return sFileName;
    }
    
    /// <summary>
    /// This mthod is used to save ICard logo on the server.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveFileOnServerForICardLogo(string asFileName)
    {
        // Upload the file to the server.
        string sFolderName = Server.MapPath("..") + S_UPLOAD_IMAGE_PATH_FOR_ID;

        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName =CommonUtility.GetFileNameForRenaming(asFileName, "IcardLogo");
            sServerFilePath = sFolderName + sFileName;
        }
        UploadICard.SaveAs(sServerFilePath);

        string sFileToDelete = Server.MapPath(".") + hidICardPath.Value;
        if (File.Exists(sFileToDelete))
            File.Delete(sFileToDelete);
        return sFileName;
    }
    /// <summary>
    /// This method is used to initialize the controls.
    /// </summary>
    private void InitializeFields()
    {
        SchoolBL oSchoolBL = new SchoolBL();
        oSchoolBL.GetSignPath(miSchoolId);
        txtAddress.Text = oSchoolBL.Address;        
        imgPhoto.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + Constants.SchoolLogos.SchoolLogo.ToInt().ToString();
        imgLogoICard.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + Constants.SchoolLogos.ICardLogo.ToInt().ToString();
        imgSign.Src = Constants.S_IMAGE_GENERATOR_PATH + "Value="  + Constants.SchoolLogos.SignLogo.ToInt().ToString();
        hidSignPath.Value = oSchoolBL.SignLogo;
        hidICardPath.Value = oSchoolBL.ICardLogo;
        hidFilePath.Value = oSchoolBL.LogoPath;
        hidSchoolName.Value = oSchoolBL.SchoolName;
    }
    /// <summary>
    /// This method is used to get array of bytes.
    /// </summary>
    /// <param name="FileField"></param>
    /// <returns></returns>
    private Byte[] GetByteArrayFromFileFieldDetails(FileUpload FileField)
    {
        //' Returns a byte array from the passed 
        //' file field controls file
        int intFileLength;
        Byte[] byteData = new byte[0];
        System.IO.Stream objStream;
        if (FileField.PostedFile != null && FileField.PostedFile.ContentLength != 0)
        {
            intFileLength = FileField.PostedFile.ContentLength;
            byteData = new Byte[intFileLength];
            objStream = FileField.PostedFile.InputStream;
            objStream.Read(byteData, 0, intFileLength);
        }
        return byteData;
    }
    /// <summary>
    /// This method is used to set javacsript attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnSave });
        btnBack.PostBackUrl = S_URL;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private string SaveFileOnServer(string asFileName)
    {
        // Upload the file to the server.
        // DeleteFiles();
        string sFolderName = Server.MapPath("..") + Constants.S_UPLOAD_IMAGE_FOLDER_PATH;

        string sServerFilePath = sFolderName + asFileName;
        string sFileName = asFileName;
        if (File.Exists(sServerFilePath))
        {
            sFileName =CommonUtility.GetFileNameForRenaming(asFileName, hidSchoolName.Value);
            sServerFilePath = sFolderName + sFileName;
        }
        UploadLogo.SaveAs(sServerFilePath);
        return sFileName;

    }
    #endregion
}
