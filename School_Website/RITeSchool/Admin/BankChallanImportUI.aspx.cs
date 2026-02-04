/* Class Name - BankChallanImportUI
 * Description - This class used to pay fees using challan numbers.
 * Author - Yogesh Karne
 * Date - 6 July 2016
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Data;

public partial class BankChallanImportUI : SchoolBase
{
    #region Data Member(s)

    string msServerFilePath;
    string msFileName;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event will fire while user will visit this page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillFeeTypes();
                lnkDownloadTemplate.Attributes.Add("onclick", "window.open('../downloads/BankChallanDetails.xls','_self'); return false;");
                lnkDownloadTemplate.CssClass = "CursorHand";
                valErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                btnImportChallans.Attributes["onclick"] = "javascript:DisableButtons(this)";
            }
            ApplyMouseHoverEffect(new List<Button> { btnImportChallans, imgbtnBack });
            lblError.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fired while user will click on Import button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImportChallans_Click(object sender, EventArgs e)
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
            Constants.UploadFileType eUploadFileType = Constants.UploadFileType.Challan;

            FileUploadUtilityBL oFileUploadUtility = new FileUploadUtilityBL(sSourceFileName, msServerFilePath, eUploadFileType);
            oFileUploadUtility.UploadChallanDetails(miSchoolId, miAcademicYearId, miUserId, cmbFeeType.SelectedValue.ToInt(), miFinancialYearId);

            if (sErrorMessage.Equals(""))
            {
                lblHead.CssClass = "ClsHilightTextB";
                lblHead.Text = "Fee Details updated successfully !!!";
                lblHead.Visible = true;
            }
        }
        catch (NoRecordFoundExceptions ex)
        {
            lblHead.Text = ex.Message;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
        }
        catch (InvalidChallanNoExceptions ex)
        {
            lblHead.Text = ex.Message;
            lblHead.CssClass = "ClsLabel";
            lblHead.Visible = true;
            lblHead.ForeColor = System.Drawing.Color.Red;
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

    private void FillFeeTypes()
    {
        StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        DataTable dtStdFeeType = oStudentFeeDetailsBL.GetStandardFeeType(miSchoolId, miAcademicYearId, 0);
        cmbFeeType.Bind(dtStdFeeType, "SchoolWise_Standard_FeeType_Id", "Fee_Type", Constants.S_SELECT);
    }

    ///// <summary>
    ///// This Method is used to fill Payable for Combobox.
    ///// </summary>
    //private void FillPayableForCombo()
    //{
    //    StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
    //    DataTable dtPayableFor = oStandardCollectionBL.GetAllPayableforChallan(miAcademicYearId, Constants.I_ZERO, cmbFeeType.SelectedValue.ToInt());
    //    ListSource.FillDropDownList(dtPayableFor, cmbPayableFor, "Display_Member", "Value_Member", Constants.S_SELECT);
    //}

    #endregion    
}