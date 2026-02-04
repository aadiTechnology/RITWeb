using System;
using BusinessLogic;
using Utility;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;
using SchoolAutoSearchService.Client;

public partial class ImportTeacherUI : SchoolBase
{
    #region Constants

    string msServerFilePath;
    string msFileName;
    const int I_COLUMN_INDEX_DOB = 3;
    const Int32 I_COL_INDEX_ROLL_NO = 2;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../downloads/TeacherDetails.xls','_self'); return false;");
                lnkDownloadTemplate.CssClass = "CursorHand";
                valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                btnImportTeachers.Attributes["onclick"] = "javascript:DisableButtons(this)";
                imgbtnBack.Attributes["onclick"] = "javascript:DisableButtons(this)";
            }
            ApplyMouseHoverEffect(new List<Button> { btnImportTeachers, imgbtnBack });
            lblError.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnImportStudent_Click(object sender, EventArgs e)
    {
        try
        {
            msFileName = CommonUtility.GetFileNameForRenaming(fileUploadStudents.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
            msServerFilePath = sFolderName + msFileName;

            fileUploadStudents.SaveAs(msServerFilePath);

            string sErrorMessage = "";
            string sSourceFileName = fileUploadStudents.PostedFile.FileName;
            Constants.UploadFileType eUploadFileType = Constants.UploadFileType.Teacher;

            FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(sSourceFileName, msServerFilePath, eUploadFileType);
            oFileUploadUtility.UserId = miUserId;
            oFileUploadUtility.SchoolId =miSchoolId;
            oFileUploadUtility.AcademicYearId = miAcademicYearId;
            oFileUploadUtility.CanPublishUnpublishExam = Settings.AllowPublishUnpublishExam ;
            sErrorMessage = oFileUploadUtility.UploadFile();

            if (sErrorMessage.Equals(""))
            {
                lblHead.CssClass = "ClsHilightTextB";
                lblHead.Text = "File uploaded successfully !!!";
                lblHead.Visible = true;
                SaveConfigDetails(Constants.SchoolConfigurations.Teacher.ToInt());
                RefreshStaffCache(0);
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
        catch (ValidMobileNumberExceptions ex)
        {
            catchException(ex);
        }
       
        catch (ValidExceptions ex)
        {
            catchException(ex);
        }
        catch (ValidPincodeExceptions ex)
        {
            catchException(ex);
        }
        catch (DuplicateExceptions ex)
        {
            catchException(ex);
        }
        catch (NoRecordFoundExceptions ex)
        {
            catchException(ex);
        }
        catch (Exception ex)
        {
            if(ex.Message == string.Empty)
                lblHead.Text = "Data in uploaded file is not in correct format.";
            else
                lblHead.Text = ex.Message;
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
    /// catch error message
    /// </summary>
    /// <param name="ex"></param>
    private void catchException(Exception ex)
    {
        lblHead.Text = ex.Message;
        lblHead.CssClass = "ClsLabel";
        lblHead.Visible = true;
        lblHead.ForeColor = System.Drawing.Color.Red;        
    }
    /// <summary>
    /// display error message
    /// </summary>
    /// <param name="asError"></param>
    private void DisplayError(string asError)
    {
        lblHead.Text = asError;
    }
    /// <summary>
    /// This for image button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgbtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            SuperAdminMasterPage oMasterPage = (SuperAdminMasterPage)this.Master;
            oMasterPage.RedirectToNextPage("~/SuperAdmin/ScreensUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This method is used to refresh staff cache.
    /// </summary>
    /// <param name="aiUserId"></param>
    private void RefreshStaffCache(int aiUserId)
    {
        List<int> lstUserIds = new List<int>();
        if (aiUserId != 0)
            lstUserIds.Add(aiUserId);
        AutoSearchService oAutoSearchService = new AutoSearchService();
        oAutoSearchService.RefreshStaffCache(miSchoolId, miAcademicYearId, lstUserIds, Constants.Action.Update);
    }
}
