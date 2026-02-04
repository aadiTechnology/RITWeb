using System;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class ImportFee :SchoolBase
{
    #region Constants

    string msServerFilePath;
    string msFileName;
    const int I_COLUMN_INDEX_DOB = 3;
    const Int32 I_COL_INDEX_ROLL_NO = 2;

    #endregion
    /// <summary>
    /// This for page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../downloads/Fee Template.xls','_self'); return false;");
                valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                btnImportFee.Attributes["onclick"] = "javascript:DisableButtons(this)";
                imgbtnBack.Attributes["onclick"] = "javascript:DisableButtons(this)";
            }
            ApplyMouseHoverEffect(new List<Button> { btnImportFee, imgbtnBack });
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This  button Import caution money
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
   protected void btnImportCautionMoney_Click(object sender, EventArgs e)
    {
        try
        {
            msFileName =CommonUtility.GetFileNameForRenaming(fileUploadStudents.FileName);
            //string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sFolderName = base.BasePath + "\\RITeSchool\\Uploads\\";
            msServerFilePath = sFolderName + msFileName;

            fileUploadStudents.SaveAs(msServerFilePath);

            string sErrorMessage = "";
            string sSourceFileName = fileUploadStudents.PostedFile.FileName;
            Constants.UploadFileType eUploadFileType = Constants.UploadFileType.Fee;
            FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(sSourceFileName, msServerFilePath, eUploadFileType);
            oFileUploadUtility.UserId = Convert.ToInt32(miUserId);
            oFileUploadUtility.SchoolId = Convert.ToInt32(miSchoolId);
            oFileUploadUtility.AcademicYearId = Convert.ToInt32(miAcademicYearId);
            sErrorMessage = oFileUploadUtility.UploadFile();
            
            if (sErrorMessage.Equals(""))
            {
                lblHead.CssClass = "ClsHilightTextB";
                lblHead.Text = "File uploaded successfully !!!";
                lblHead.Visible = true;
                AddTeacherConfigDetails();
            }
            else
            {
                DisplayError(sErrorMessage);
                lblHead.Visible = true;
                
            }
        }
        catch (BusinessLogic.Exceptions.DuplicateRegisterNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullRegisterNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentRollNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentFirstNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.DuplicateRollNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentMiddleNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentLastNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentMotherNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentDateofBirthExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentAdmissionDateExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentJoiningDateExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentSexExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentParentNameExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentParentOccupationExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentAddressExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentCityExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentStateExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentPincodeExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentMobileExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.ValidMobileNumberExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentCategoryExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullStudentCasteSubcasteExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.ValidPincodeExceptions ex)
        {
            catchException(ex);
        }
        catch (BusinessLogic.Exceptions.NullPhotoFileExceptions ex)
        {
            catchException(ex); 
        }
        catch (Exception ex)
        {
            lblHead.Text = "Data in uploaded file is not in correct format.";
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (System.IO.File.Exists(msServerFilePath))
                System.IO.File.Delete(msServerFilePath);
        }
    }
    /// <summary>
    /// This for catch error message
    /// </summary>
    /// <param name="ex"></param>
    private void catchException(Exception ex)
    {
        lblHead.Text = ex.Message;
        lblHead.CssClass = "ClsLabel";
        lblHead.Visible = true;
        lblHead.ForeColor = System.Drawing.Color.Red;
        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());

    }
    /// <summary>
    /// Display error message
    /// </summary>
    /// <param name="asError"></param>
    private void DisplayError(string asError)
    {
        lblHead.Text = asError;
    }
    /// <summary>
    /// This is for image button click
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
    /// This method is used to do entry into configurationSchoolMaster.
    /// </summary>
    private void AddTeacherConfigDetails()
    {
        int iAcademicYearId = Convert.ToInt32(miAcademicYearId);
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL();
        oConfiguration.OriginalConfigId = Convert.ToInt32(Constants.SchoolConfigurations.Teacher);
        oConfiguration.SchoolId = Convert.ToInt32(miSchoolId);
        oConfiguration.AcademicYearId = iAcademicYearId;
        oConfiguration.IsConfigure = Constants.C_YES;
        oConfiguration.InsertedById = Convert.ToInt32(miUserId);
        oConfiguration.UpdateById = Convert.ToInt32(miUserId);
        oConfiguration.InsertConfigurationSchoolMaster();
    }
}
