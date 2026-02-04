/*
 * File Name - InvestmentDocumentPopup.aspx.cs
 * Created By - sachin
 * Created Date - 5-April-2013
 * Descrption - This class is used to upload and delete investment documents.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollEntities;
using Utility;

public partial class InvestmentDocumentPopup : SchoolBase
{
    #region Constants

    private const string S_INVESTMENT_DOCUMENT_FOLDER_LOCATION = "\\DOWNLOADS\\Investment Declarations\\";
    private const string S_PAN_DOCUMENT_FOLDER_LOCATION = "\\DOWNLOADS\\PAN Attachment\\";
    private const string S_USER_DOCUMENT_FOLDER_LOCATION = "\\DOWNLOADS\\User Documents\\";
    private const string S_PERFORMANCE_EVALUATION_DOCUMENT_FOLDER_LOCATION = "\\DOWNLOADS\\Performance Evaluation\\";    
    private const string S_DOCUMENT = "Documents";
    private const int I_FILE_SIZE_LIMIT = 5242880; // nearly 5 mb
    private const string S_FILE_SIZE_ERROR_MESSAGE = "File size should not be greater than 5 MB.";
    
    #endregion

    #region Data Member(s)

    private InvestmentDeclarationBL moInvestmentDeclarationBL;
    private bool mbIsPublished;
    private string msHasFullAccess;

    #endregion

    #region Property(s)

    private bool AllowModification
    {
        get { return !mbIsPublished && (QueryString["IsSubmitted"] != Constants.S_YES || msHasFullAccess == Constants.S_YES); }
    }

    private Constants.DocumentTypes DocumentType
    {
        get { return (Constants.DocumentTypes)Convert.ToInt32(QueryString["DocumentTypeId"]); }
    }

    public string FolderPath
    {        
        get 
        {
            string sFolderPath = string.Empty;
            switch(DocumentType)
            {
                case Constants.DocumentTypes.InvestmentDocuments: sFolderPath =  S_INVESTMENT_DOCUMENT_FOLDER_LOCATION; break;
                case Constants.DocumentTypes.StudentDocuments: sFolderPath = S_USER_DOCUMENT_FOLDER_LOCATION; break;
                case Constants.DocumentTypes.PAN: sFolderPath = S_PAN_DOCUMENT_FOLDER_LOCATION; break;
                case Constants.DocumentTypes.PerformanceEvaluation: sFolderPath = S_PERFORMANCE_EVALUATION_DOCUMENT_FOLDER_LOCATION; break;
            }
            return sFolderPath;
        }
    }

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
            moInvestmentDeclarationBL = new InvestmentDeclarationBL(miSchoolId, miFinancialYearId, miUserId);
            if(DocumentType == Constants.DocumentTypes.InvestmentDocuments)
                CheckIsIncomeTaxPublished();
            if (!IsPostBack)
            {
                GetState();
                hidDocumentTypeId.Value = Convert.ToString(QueryString["DocumentId"]);
                SetDefaultValues();
                SetUserDetails();                
                FillDocumentsLisView();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
   
    /// <summary>
    /// This event is used to set attributes on listview child controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDocuments_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                InvestmentDocument oInvestmentDocument = oCurrentItem.DataItem as InvestmentDocument;
                Label lblFileName = oCurrentItem.FindControl("lblFileName") as Label;
                if (oInvestmentDocument.FileName.Length > 68)
                    lblFileName.Text = oInvestmentDocument.FileName.Substring(0, 68) + "..";
                ImageButton btnDownload = oCurrentItem.FindControl("btnDownload") as ImageButton;
                if (DocumentType == Constants.DocumentTypes.InvestmentDocuments)
                {
                    string sDestination = Server.MapPath("..") + FolderPath + oInvestmentDocument.FileName;
                    if (File.Exists(sDestination))
                        btnDownload.Attributes.Add("onclick", "window.open('..//downloads//Investment Declarations//" + oInvestmentDocument.FileName + "','_blank'); return false;");
                }
                else if (DocumentType == Constants.DocumentTypes.PAN)
                {
                    string sDestination = Server.MapPath("..") + FolderPath + oInvestmentDocument.FileName;
                    if (File.Exists(sDestination))
                        btnDownload.Attributes.Add("onclick", "window.open('..//downloads//PAN Attachment//" + oInvestmentDocument.FileName + "','_blank'); return false;");
                }
                else if (DocumentType == Constants.DocumentTypes.PerformanceEvaluation)
                {
                    string sDestination = Server.MapPath("..") + FolderPath + oInvestmentDocument.FileName;
                    if (File.Exists(sDestination))
                        btnDownload.Attributes.Add("onclick", "window.open('..//downloads//Performance Evaluation//" + oInvestmentDocument.FileName + "','_blank'); return false;");
                }
                else
                {
                    string sDestination = Server.MapPath("..") + FolderPath + oInvestmentDocument.FileName;
                    if (File.Exists(sDestination))
                        btnDownload.Attributes.Add("onclick", "window.open('..//downloads//User Documents//" + oInvestmentDocument.FileName + "','_blank'); return false;");
                }
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
                if (DocumentType == Constants.DocumentTypes.InvestmentDocuments && !AllowModification)
                    oimgbtnDelete.Enabled = false;
             
                if (hidBtnState.Value != string.Empty)
                    oimgbtnDelete.Enabled = hidBtnState.Value.ToBool();
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
        string asFileName;
        if (SaveFileToServer(out asFileName))
        {
            int iDocumentId = Convert.ToInt32(QueryString["DocumentId"]);
            int iUserId = Convert.ToInt32(QueryString["UserId"]);
            int iAcademicYear = QueryString["AcademicYear"].ToInt();
            int iReportingUserId = QueryString["ReportingUserId"].ToInt();                     
            moInvestmentDeclarationBL.SaveDocument(iDocumentId, asFileName, iUserId, DocumentType.ToInt(), iAcademicYear, iReportingUserId);            
            FillDocumentsLisView();
            DisplayMessage("uploaded");
        }
    }

    /// <summary>
    /// This event is used to remove documet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwDocuments_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    int iId = Convert.ToInt32(lstvwDocuments.DataKeys[e.Item.DisplayIndex]["Id"]);                    
                    moInvestmentDeclarationBL.DeleteDocument(iId, DocumentType.ToInt());
                    DisplayMessage("deleted");
                                        
                    string sFileName = (e.Item.FindControl("lblFileName") as Label).Text;
                    string sServerFilePath = Server.MapPath("..") + FolderPath + sFileName;
                    if (DocumentType == Constants.DocumentTypes.StudentDocuments)
                        sServerFilePath = Server.MapPath("..") + FolderPath + sFileName;

                    if (File.Exists(sServerFilePath))
                        File.Delete(sServerFilePath);

                    FillDocumentsLisView();
                }
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

            if (DocumentType != Constants.DocumentTypes.PerformanceEvaluation)
            {
                if (QueryString["ClientId"] != null && QueryString["ClientId"].ToString() != string.Empty)
                {
                    string sQueryString = lstvwDocuments.Items.Count + "$" + QueryString["ClientId"];
                    ScriptManager.RegisterStartupScript(BtnSave, this.GetType(), "CloseWin", "CloseAppWindow('" + sQueryString + "');", true);
                }
                else
                {
                    string sQueryString = lstvwDocuments.Items.Count + "$" + QueryString["DocumentId"] + "$" + QueryString["UserId"];
                    ScriptManager.RegisterStartupScript(BtnSave, this.GetType(), "CloseWin", "CloseNewWindow('" + sQueryString + "');", true);                    
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(BtnSave, this.GetType(), "CloseWin", "ClosePerformanceWindow(" + lstvwDocuments.Items.Count + ",'" + QueryString["ClientId"] + "')", true);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to user and investment details.
    /// </summary>
    private void SetUserDetails()
    {
        string sDocumentName;
        lblUserName.Text = moInvestmentDeclarationBL.GetUserInvestmentMethodDetails(QueryString["UserId"].ToInt(), QueryString["DocumentId"].ToInt(), out sDocumentName, DocumentType.ToInt());
        lblInvestmentMethod.Text = sDocumentName;
    }

    /// <summary>
    /// This method is used to set default values to fields.
    /// </summary>
    private void SetDefaultValues()
    {
        valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        ApplyMouseHoverEffect(new List<Button> { BtnSave, btnClose });
        lblName.Text = DocumentType == Constants.DocumentTypes.StudentDocuments ? "Student Name:" : "User Name:";

        BtnSave.Attributes.Add("onclick", "ResetMessage();");
        flDocument.Focus();
    }

    /// <summary>
    /// This method is used to validate file size.
    /// </summary>
    /// <param name="asFileName"></param>
    /// <returns></returns>
    private bool SaveFileToServer(out string asFileName)
    {      
        if (flDocument.FileContent.Length > I_FILE_SIZE_LIMIT)
        {
            asFileName = flDocument.FileName;
            DisplayMessage(S_FILE_SIZE_ERROR_MESSAGE, true, tdMessage);            
            return false;
        }

        string sFileName = flDocument.FileName;
        string sRenamedFileName = sFileName;
        string sFolderName = Server.MapPath("..") + FolderPath;
        //if (DocumentType == Constants.DocumentTypes.StudentDocuments)
        //    sFolderName = Server.MapPath("..") + S_USER_DOCUMENT_FOLDER_LOCATION;

        string sServerFilePath = sFolderName + sFileName;
       
        asFileName = sFileName;
        if (File.Exists(sServerFilePath))
        {
            sRenamedFileName = CommonUtility.GetFileNameForRenaming(sFileName);
            asFileName = sRenamedFileName;
        }

        sServerFilePath = sFolderName + sRenamedFileName;        
        flDocument.SaveAs(sServerFilePath);
        return true;
    }

    /// <summary>
    /// This method is used to fill documents in listview.
    /// </summary>
    private void FillDocumentsLisView()
    {
        int iDocumentId = Convert.ToInt32(QueryString["DocumentId"]);
        int iUserId = Convert.ToInt32(QueryString["UserId"]);
        int iAcademicYear = QueryString["AcademicYear"].ToInt();
        int iReportingUser = QueryString["ReportingUserId"].ToInt();

        List<InvestmentDocument> lstDocuments = moInvestmentDeclarationBL.GetDocuments(iDocumentId, iUserId, DocumentType.ToInt(), iAcademicYear, iReportingUser);
        lstvwDocuments.DataSource = lstDocuments;
        lstvwDocuments.DataBind();
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
    /// This method is used to check whether income tax details are published or not.
    /// </summary>
    private void CheckIsIncomeTaxPublished()
    {
        msHasFullAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.InvestmentDeclaration).ToString();

        if (moUserRole == Constants.UserRoles.Admin)
            msHasFullAccess = Constants.S_YES;

        IncomeTaxDetailsBL oIncomeTaxDetailsBL = new IncomeTaxDetailsBL(miSchoolId, miFinancialYearId, miUserId, miAcademicYearId);
        mbIsPublished = oIncomeTaxDetailsBL.CheckIsPublished(QueryString["UserId"].ToInt());
        if (!AllowModification)
            BtnSave.Enabled = false;
    }


    /// <summary>
    /// This method is used to check Document type is performance evaluation or not.
    /// </summary>
    private void GetState()
    {
        if (Constants.DocumentTypes.PerformanceEvaluation.ToInt() == Convert.ToInt32(QueryString["DocumentTypeId"]))
        {
                hidBtnState.Value = Convert.ToString(QueryString["SetContolState"].ToBool());
        }
        else
            hidBtnState.Value = string.Empty;

        if (hidBtnState.Value != string.Empty)
            BtnSave.Enabled = hidBtnState.Value.ToBool();
    }
    #endregion    
}