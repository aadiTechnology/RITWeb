/*    File Name   : ImportClearanceDetailsPopup.aspx.cs
 *    Created By - Sachin
 *    Created Date - 21 May 2026
 *    Description : Import MIS clearance details from excel file.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using Utility;

public partial class ImportClearanceDetailsPopup : SchoolBase
{
    #region Constants

    private const string S_SHEET_NAME = "Sheet1";

    #endregion Constants

    #region Member(s)

    private ImportClearanceDetailsBL moImportClearanceDetailsBL;

    #endregion Member(s)

    #region Property(s)

    private bool IsAccountsModuleEnabled
    {
        get { return Settings.EnableAccountsModule; }
    }

    private ImportClearanceDetailsBL ClearanceDetailsBL
    {
        get
        {
            if (moImportClearanceDetailsBL == null)
                moImportClearanceDetailsBL = new ImportClearanceDetailsBL(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId, IsAccountsModuleEnabled);
            return moImportClearanceDetailsBL;
        }
    }

    #endregion Property(s)

    #region Event(s)

    /// <summary>
    /// This event is used to initialize popup controls.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
                SetDefaultValues();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to import clearance details from uploaded excel file.
    /// </summary>
    protected void btnImport_Click(object sender, EventArgs e)
    {
        string sServerFilePath = string.Empty;
        try
        {
            lblErrorMsg.Visible = false;
            lblSuccessMsg.Visible = false;

            string sValidationMessage = ClearanceDetailsBL.ValidateUploadedFile(fileUploadClearance.HasFile, fileUploadClearance.FileName);
            if (!string.IsNullOrEmpty(sValidationMessage))
            {
                DisplayError(sValidationMessage);
                return;
            }

            string sFileName = CommonUtility.GetFileNameForRenaming(fileUploadClearance.FileName);
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\ImportFiles\\";
            sServerFilePath = sFolderName + sFileName;
            fileUploadClearance.SaveAs(sServerFilePath);

            DataSet oDSExcelData = CommonUtility.ReadExcelSheetAndFetchData(sServerFilePath, string.Empty, S_SHEET_NAME);
            sValidationMessage = ClearanceDetailsBL.ValidateExcelData(oDSExcelData);
            if (!string.IsNullOrEmpty(sValidationMessage))
            {
                DisplayError(sValidationMessage);
                return;
            }

            DataTable odtExcelData = oDSExcelData.Tables[0];
            string sTxnIdsXml = ClearanceDetailsBL.GenerateTransactionIdsXml(odtExcelData);

            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
            DataTable odtTransactionDetails = oStudentFeeDetailsBL.GetTransactionDetails(miSchoolId, miFinancialYearId, sTxnIdsXml);

            sValidationMessage = ClearanceDetailsBL.ValidateTransactionIds(odtExcelData, odtTransactionDetails);
            if (!string.IsNullOrEmpty(sValidationMessage))
            {
                DisplayError(sValidationMessage);
                return;
            }

            ImportClearanceSaveResult oSaveResult = ClearanceDetailsBL.SaveOnlineTrasactionPayments(odtExcelData, odtTransactionDetails, RecordPayment);
            if (!string.IsNullOrEmpty(oSaveResult.ErrorMessage))
            {
                DisplayError(oSaveResult.ErrorMessage);
                return;
            }

            lblErrorMsg.Visible = false;
            lblSuccessMsg.Visible = true;
            lblSuccessMsg.Text = oSaveResult.SuccessMessage;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            DisplayError("Data in uploaded file is not in correct format.");
        }
        finally
        {
            if (!string.IsNullOrEmpty(sServerFilePath) && File.Exists(sServerFilePath))
                File.Delete(sServerFilePath);
        }
    }

    #endregion Event(s)

    #region Private Method(s)

    /// <summary>
    /// This method is used to record payment in day book.
    /// </summary>
    private void RecordPayment(string asDayBookXml, bool abIsForAdmission)
    {
        AccountVoucherClient oVoucherClient = null;
        try
        {
            oVoucherClient = new AccountVoucherClient();
            oVoucherClient.Open();

            if (!abIsForAdmission)
                oVoucherClient.CreateFeeVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId, asDayBookXml, Constants.PaymentMode.Online);
            else
                oVoucherClient.CreateAdmissionFormVoucher(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId, asDayBookXml);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (oVoucherClient != null && oVoucherClient.State != CommunicationState.Faulted)
                oVoucherClient.Close();
        }
    }

    /// <summary>
    /// Displays validation / error message on screen.
    /// </summary>
    private void DisplayError(string asMessage)
    {
        lblErrorMsg.Text = asMessage;
        lblErrorMsg.Visible = true;
        lblSuccessMsg.Visible = false;
    }

    /// <summary>
    /// This method is used to set default values.
    /// </summary>
    private void SetDefaultValues()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        btnClose.Attributes.Add("onclick", "CloseWindow(); return false;");
        ApplyMouseHoverEffect(new List<Button> { btnImport, btnClose });
        lblErrorMsg.Visible = false;
        lblSuccessMsg.Visible = false;
    }

    #endregion Private Method(s)
}
