using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Exceptions;
using System.Data;
using BusinessLogic;
using Utility;
using CrystalDecisions.Shared;
using System.Reflection;
using System.Threading;

public partial class GenerateChallanPopUp : SchoolBase
{
    #region Events

    /// <summary>
    /// This event is used to set the page Load Events.
    /// </summary>

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ReadQueryString();
                FillAcademicYearCombo();                
            }
        }        
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the fee type combobox.
    /// </summary>
    protected void cmbAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillFeeTypeCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the payable for combobox.
    /// </summary>
    protected void cmbFeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillPayableForCombo();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Diasplay Challan Details.
    /// </summary>
    protected void btnDisplayChallan_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDisplay oReportDisplay = null;
            oReportDisplay = new ReportDisplay(Constants.ExportReports.ClasswiseBankChallan, GetFilterString(), ExportFormatType.PortableDocFormat);
            oReportDisplay.DisplayReport();
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method's

    /// <summary>
    /// This Method is used to Fill Academic Year Combo.
    /// </summary>
    private void FillAcademicYearCombo()
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDtYearInfo = oSchoolWiseAcademicYearMasterBL.GetAcademicYearsforStudentFeeChallan(miSchoolId, miAcademicYearId, hidStudentId.Value.ToInt());
        cmbAcademicYear.Bind(oDtYearInfo, "Value_Member", "Display_Member", Constants.S_SELECT);
    }

    /// <summary>
    /// This Method is used to fill Fee Types combobox.
    /// </summary>
    private void FillFeeTypeCombo()
    {
        SchoolWiseAcademicYearMasterBL oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL(miSchoolId, miAcademicYearId);
        DataTable dtStudentInfo = oSchoolWiseAcademicYearMasterBL.GetStudentIdAndStandardIdForChallan(miSchoolId, miAcademicYearId, cmbAcademicYear.SelectedValue.ToInt(), hidStudentId.Value.ToInt());
        hidStandardId.Value = Convert.ToString(dtStudentInfo.Rows[Constants.I_ZERO]["StandardId"]);
        hidSchoolwiseStudentId.Value = Convert.ToString(dtStudentInfo.Rows[Constants.I_ZERO]["SchoolwiseStudentId"]);
        hidStandardDivisionId.Value = Convert.ToString(dtStudentInfo.Rows[Constants.I_ZERO]["StandardDivisionId"]);
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId);
        DataTable dtFeeType = oStandardCollectionBL.GetAllFeeTypesForChallanImport(cmbAcademicYear.SelectedValue.ToInt(), hidStandardId.Value.ToInt(), hidStandardDivisionId.Value.ToInt());
        ListSource.FillDropDownList(dtFeeType, cmbFeeType, "Display_Member", "Value_Member", Constants.S_SELECT);        
    }

    /// <summary>
    /// This Method is used to fill Payable for Combobox.
    /// </summary>
    private void FillPayableForCombo()
    {
        StandardCollectionBL oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable dtPayableFor = oStandardCollectionBL.GetAllPayableforChallan(cmbAcademicYear.SelectedValue.ToInt(), hidStandardId.Value.ToInt(), cmbFeeType.SelectedValue.ToInt());
        ListSource.FillDropDownList(dtPayableFor, cmbPayableFor, "Display_Member", "Value_Member", Constants.S_SELECT);        
    }

    /// <summary>
    /// This Method is used to get filter string for report.
    /// </summary>
    private string GetFilterString()
    {
        string sFilterStr = string.Empty;

        sFilterStr = "(usp_GetBankChallanDetails.School_Id}=" + miSchoolId + "AND usp_GetBankChallanDetails.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetBankChallanDetails.Standard_Id}=" + hidStandardId.Value.ToInt() + "AND usp_GetBankChallanDetails.SchoolWise_Standard_Division_Id}=" + hidStandardDivisionId.Value.ToInt() + "AND usp_GetBankChallanDetails.Student_Id}=" + hidSchoolwiseStudentId.Value.ToInt() + "AND usp_GetBankChallanDetails.Original_Fee_Type_Id}=" + cmbFeeType.SelectedValue.ToInt() + " AND usp_GetBankChallanDetails.Payable_For}=" + cmbPayableFor.SelectedValue + "AND usp_GetBankChallanDetails.AcademicYearId}=" + cmbAcademicYear.SelectedValue.ToInt() + ") @";

        return sFilterStr;

    }

    /// <summary>
    /// This Method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString != null)
        {
            if (QueryString["StudentId"] != null)
                hidStudentId.Value = QueryString["StudentId"];
        }
    }

    /// <summary>
    /// This Method is used to set java script attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnDisplayChallan, btnClose });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
    }

    #endregion
}