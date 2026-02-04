using BusinessLogic;
using BusinessLogic.Exceptions;
using System;
using System.Reflection;
using Utility;

public partial class ImportTransportAllocationUI : SchoolBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            string sFileName = CommonUtility.GetFileNameForRenaming(fileUploadAllocation.FileName);
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\TransportReadingAllocationAndMaintenance\\";
            string sServerFilePath = sFolderName + sFileName;
            fileUploadAllocation.SaveAs(sServerFilePath);

            if (cmbAllocationType.SelectedValue == Constants.S_ONE)
            {   
                FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(fileUploadAllocation.PostedFile.FileName, sServerFilePath, Constants.UploadFileType.VehicleReadingAllocation);
                oFileUploadUtility.UploadTransportAllocationDetails(miSchoolId, miAcademicYearId, miUserId);
                lblMessage.Text = "Transport Allocation Details uploaded successfully !!!";                
            }
            else
            {
                FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(fileUploadAllocation.PostedFile.FileName, sServerFilePath, Constants.UploadFileType.VehicleMaintenance);
                oFileUploadUtility.UploadMaintenanceDetails(miSchoolId, miAcademicYearId, miUserId);
                lblMessage.Text = "Transport Maintenance Details uploaded successfully !!!";
            }

            lblMessage.ForeColor = System.Drawing.Color.Blue;
            lblMessage.Font.Bold = true;
            cmbAllocationType.ClearSelection();
        }
        catch(InvalidVehicleDataExceptions ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Font.Bold = false;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Failed to import.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Font.Bold = false;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    private void SetJavascriptAttributes()
    {
        valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;

        lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../downloads/Transport Reading Allocation.xlsx','_self'); return false;");
        lnkDownloadTemplate.CssClass = "CursorHand";

        lnkDownloadMaintenance.Attributes.Add("onclick", "window.open('../downloads/Vehicles Maintenance Expenses.xlsx','_self'); return false;");
        lnkDownloadMaintenance.CssClass = "CursorHand";
    }
}