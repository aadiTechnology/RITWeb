using System;
using BusinessLogic;
using Utility;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using SchoolAutoSearchService.Client;

public partial class PayFeeByChallanUI : SchoolBase
{
    string msServerFilePath;
    string msFileName;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if(!IsPostBack)
            {

                lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../downloads/BankChallanDetails.xls','_self'); return false;");
                lnkDownloadTemplate.CssClass = "CursorHand";
                valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnImportTeachers_Click(object sender, EventArgs e)
    {
        try
        {
            msFileName = CommonUtility.GetFileNameForRenaming(fileUploadStudents.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";\
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
            msServerFilePath = sFolderName + msFileName;

            fileUploadStudents.SaveAs(msServerFilePath);

            string sErrorMessage = "";
            string sSourceFileName = fileUploadStudents.PostedFile.FileName;
            Constants.UploadFileType eUploadFileType = Constants.UploadFileType.Challan;

            FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(sSourceFileName, msServerFilePath, eUploadFileType);
            oFileUploadUtility.UserId = miUserId;
            oFileUploadUtility.SchoolId = miSchoolId;
            oFileUploadUtility.AcademicYearId = miAcademicYearId;
            
            sErrorMessage = oFileUploadUtility.UploadFile();

            if (sErrorMessage.Equals(""))
            {
                lblHead.CssClass = "ClsHilightTextB";
                lblHead.Text = "File uploaded successfully !!!";
                lblHead.Visible = true;
            }
            else
            {
                DisplayError(sErrorMessage);
            }
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch
        {
            lblHead.Text = "Data in uploaded file is not in correct format.";
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            if (System.IO.File.Exists(msServerFilePath))
                System.IO.File.Delete(msServerFilePath);
        }
    }
    /// <summary>
    /// display error message
    /// </summary>
    /// <param name="asError"></param>
    private void DisplayError(string asError)
    {
        lblHead.Text = asError;
    }
}