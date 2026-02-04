using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using System.Configuration;
using BusinessLogic;
using CrystalDecisions.Shared;
using PushNotificationService;
using BusinessLogic.Exceptions;
using System.Data;

public partial class StudentLeavingCertificateUI : SchoolBase
{
    #region Event(s)

    /// <summary>
    /// This event is used to load the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GetTCFileName();
                SetJavascriptAttributes();
                CheckFeeStatus();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }       

    #endregion

    #region Private method(s)

    private void GetTCFileName()
    {
        UploadUserDocumentBL oUploadUserDocumentBL = new UploadUserDocumentBL();
        DataTable dtFileName = oUploadUserDocumentBL.GetStudentLCFileName(miSchoolId, miUserId);

        if (dtFileName.Rows.Count > Constants.I_ZERO)
            hidFileName.Value = dtFileName.Rows[0]["FileName"].ToString();
        else
        {
            tblLCDetails.Visible = false;
            hidFileName.Value = string.Empty;
        }
    }

    /// <summary>
    /// This method is used to check fee is pending or not.
    /// </summary>
    private void CheckFeeStatus()
    {
        StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
        oStudentFeeDetailsBL.School_Id = miSchoolId;
        oStudentFeeDetailsBL.Academic_Year_Id = miAcademicYearId;
        oStudentFeeDetailsBL.Student_Id = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        bool bIsFeePending = oStudentFeeDetailsBL.PendingFeesAvailableForStudent();
        

        if (bIsFeePending)
        {
            tblLCDetails.Visible = false;
            trPendingFee.Visible = true;
        }
        else
        {
            tblLCDetails.Visible = true;
            trPendingFee.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to display report.
    /// </summary>
    /// <param name="asCompanyId"></param>
    /// <param name="asReportPath"></param>
    private void DisplayReport()
    {
        StudentBL oStudentBL = new StudentBL(Session[Constants.S_SESSION_STUDENT_ID].ToInt());
        string sEnrolmentNo = oStudentBL.EnrolementNo;
        ReportDisplay oReportDisplay = null;  

        int aiGeneratedById = 0;
        if (rdoParent.Checked)
            aiGeneratedById = 1;
        else if (rdoSchool.Checked)
            aiGeneratedById = 2;

        LCDetailsBL oLCDetailsBL = new LCDetailsBL();
        oLCDetailsBL.AddLcGeneratedDetails(miSchoolId, sEnrolmentNo, aiGeneratedById);  

        string sRecordSelectionFormula = "(usp_TransferCertificate_PPSH.School_Id}=)" + miSchoolId + "AND usp_TransferCertificate_PPSH.Enrolment_Number} =" + sEnrolmentNo + "AND usp_TransferCertificate_PPSH.PrintDate} = " + DateTime.Now.ToDateTime() + ") @";

        oReportDisplay = new ReportDisplay(Constants.ExportReports.TransferCertificatePPSH, sRecordSelectionFormula, ExportFormatType.PortableDocFormat);
        oReportDisplay.DisplayReport();
    }

    /// <summary>
    /// This method is used to set javascript attributes to controls.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        rdoParent.Checked = true;
        base.ApplyMouseHoverEffect(new List<Button> { btnReport });
        btnReport.Attributes.Add("Onclick", "OpenDocument('" + hidFileName.Value + "');return false;");
    }

    #endregion
}