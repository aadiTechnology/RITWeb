using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Admin;
using Utility;

public partial class PANAttachmentPopup : SchoolBase
{
    #region Constants

    private const string S_PAN_DOCUMENT_FOLDER_LOCATION = "\\DOWNLOADS\\PAN Attachment\\";
    private const string S_UPLOAD_FILE_PATH_FOR_AADHAR = "\\DOWNLOADS\\Aadhar Cards\\";
    
    #endregion

    #region Property(s)

    private Constants.DocumentTypes DocumentType
    {
        get { return (Constants.DocumentTypes)Convert.ToInt32(QueryString["DocumentTypeId"]); }
    }

    private string FolderPath
    {
        get
        {
            if (((Constants.DocumentTypes)Convert.ToInt32(QueryString["DocumentTypeId"])) == Constants.DocumentTypes.PAN)
                return S_PAN_DOCUMENT_FOLDER_LOCATION;
            else
                return S_UPLOAD_FILE_PATH_FOR_AADHAR;
        }
    }

    #endregion

    private int iFileSize = 1048576; // nearly 1 mb

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
            if (!IsPostBack)
            {
                hidDocumentTypeId.Value = Convert.ToString(QueryString["DocumentTypeId"]);
                SetDefaultValues();
                SetUserDetails();
            }   
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save investment document.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string asFileName;
            if (SaveFileToServer(out asFileName))
            {
                Update(asFileName);
                ClosePopup();
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to close popup and pass data to parent screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            ClosePopup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to delete PAN / aadhar card file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteImage_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Update(string.Empty);
            string sServerFilePath = Server.MapPath("..") + FolderPath + hidFileName.Value;
            if (DocumentType == Constants.DocumentTypes.StudentDocuments)
                sServerFilePath = Server.MapPath("..") + FolderPath + hidFileName.Value;

            if (File.Exists(sServerFilePath))
                File.Delete(sServerFilePath);

            hidFileName.Value = string.Empty;
            btnDeleteImage.Visible = false;
            btnDownload.Visible = false;
            DisplayMessage("deleted");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to delete PAN / aadhar card details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int iDocumentId = Convert.ToInt32(QueryString["DocumentTypeId"]);
            int iUserId = Convert.ToInt32(QueryString["UserId"]);
            PanAttachmentBL oPanAttachmentBL = new PanAttachmentBL();
            oPanAttachmentBL.Delete(iDocumentId, iUserId, miUserId);
            ClosePopup();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to close popup.
    /// </summary>
    private void ClosePopup()
    {
        ScriptManager.RegisterStartupScript(BtnSave, this.GetType(), "CloseWin", "CloseWindow()", true);
    }

    /// <summary>
    /// This method is used to user and investment details.
    /// </summary>
    private void SetUserDetails()
    {
        if (DocumentType == Constants.DocumentTypes.PAN)
        {
            lblInvestmentMethod.Text = "PAN Card";
            spnHeader.InnerText = "PAN No.";
            spnTopHeader.InnerText = "PAN Card Details";
            spnFileType.InnerText = "(Attachment supports files of types - .BMP, .JPG, .JPEG, .PNG, .PDF upto 2 MB.)";
            iFileSize = 2097152;
        }
        else
        {
            lblInvestmentMethod.Text = "Aadhar Card";
            spnHeader.InnerText = "Aadhar Card No.";
            spnTopHeader.InnerText = "Aadhar Card Details";
            spnFileType.InnerText = "(Attachment supports files of types - .BMP, .JPG,.JPEG, .PNG, .PDF upto 3 MB.)";
            iFileSize = 3145728;
        }

        PanAttachmentBL oPanAttachmentBL = new PanAttachmentBL();
        PANAttachmentDetails oPANAttachmentDetails = oPanAttachmentBL.Get(QueryString["UserId"].ToInt(), QueryString["DocumentTypeId"].ToInt());
        if (oPANAttachmentDetails != null)
        {
            lblUserName.Text = oPANAttachmentDetails.Name;
            txtPANNo.Text = oPANAttachmentDetails.PanNo;
            txtNameonAadharCard.Text = oPANAttachmentDetails.NameonAadharCard;

            if (oPANAttachmentDetails.PanAttachment != string.Empty)
            {
                btnDownload.Visible = true;
                btnDeleteImage.Visible = true;
                hidFileName.Value = oPANAttachmentDetails.PanAttachment;
                if(DocumentType == Constants.DocumentTypes.PAN)
                    btnDownload.Attributes.Add("onclick", "window.open('..//downloads//PAN Attachment//" + oPANAttachmentDetails.PanAttachment + "','_blank'); return false;");
                else
                    btnDownload.Attributes.Add("onclick", "window.open('..//downloads//Aadhar Cards//" + oPANAttachmentDetails.PanAttachment + "','_blank'); return false;");

                btnDeleteImage.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }

            if (oPANAttachmentDetails.PanNo != string.Empty)
                btnDelete.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to set default values to fields.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnClose });
        lblName.Text = DocumentType == Constants.DocumentTypes.StudentDocuments ? "Student Name:" : "User Name:";
        btnDelete.Attributes.Add("onclick", "if(!ConfirmAllDelete()) {return false;}");
        txtPANNo.Focus();
        
        if (DocumentType == Constants.DocumentTypes.PAN)
            trAadharCard.Visible = false;

        if (miSchoolId != Constants.SchoolId.PPS.ToInt())
            RequiredFieldValidator1.Enabled = false;
    }

    /// <summary>
    /// This method is used to validate file size.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private bool SaveFileToServer(out string asFileName)
    {
        if (flDocument.HasFile)
        {
            if (flDocument.FileContent.Length > iFileSize)
            {
                asFileName = flDocument.FileName;
                string sMessage = "File size should not be greater than 2 MB.";
                if(DocumentType == Constants.DocumentTypes.AadharCard)
                    sMessage = "File size should not be greater than 3 MB.";

                DisplayMessage(sMessage, true, tdMessage);
                return false;
            }

            string sFileName = flDocument.FileName;
            string sRenamedFileName = sFileName;
            string sFolderName = Server.MapPath("..") + FolderPath;

            string sServerFilePath = sFolderName + sFileName;

            asFileName = sFileName;

            if (File.Exists(sServerFilePath))
            {
                sRenamedFileName = CommonUtility.GetFileNameForRenaming(sFileName);
                asFileName = sRenamedFileName;
            }

            sServerFilePath = sFolderName + sRenamedFileName;
            flDocument.SaveAs(sServerFilePath);
        }
        else
            asFileName = hidFileName.Value;
        return true;
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="asMessage"></param>
    private void DisplayMessage(string asMessage)
    {
        string sMessage = "Document " + asMessage + " successfully !!!";
        base.DisplayMessage(sMessage, false, tdMessage);
    }

    /// <summary>
    /// This method is used to update details.
    /// </summary>
    /// <param name="asFileName"></param>
    private void Update(string asFileName)
    {
        int iDocumentId = Convert.ToInt32(QueryString["DocumentTypeId"]);
        int iUserId = Convert.ToInt32(QueryString["UserId"]);
        PanAttachmentBL oPanAttachmentBL = new PanAttachmentBL();
        oPanAttachmentBL.Save(iDocumentId, iUserId, txtPANNo.Text.Trim(), txtNameonAadharCard.Text.Trim(), asFileName, miUserId);
    }

    #endregion
}