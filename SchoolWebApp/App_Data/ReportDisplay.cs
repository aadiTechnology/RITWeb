using System;
using System.Configuration;
using System.Data;
using System.Web;
using BusinessLogic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Utility;
using System.Threading;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Summary description for ReportDisplay
/// </summary>
public class ReportDisplay : SchoolBase
{
    #region -- MEMBER(s) --

    private string msFilter;
    private Constants.ExportReports meReportName;
    private ExportFormatType moExportFormatType;
    private ReportDocument crReportDocument;
    private string msFilePathAndName;
    private bool mbExportReport;
    private int miTermId;
    private string msBasePath;
    private bool mbAllowSecondTermFromTermReport;
    private Dictionary<string, string> moKVP;

    private int miSchoolId;
    private int miAcademicYearId;
    private string msSchoolName;
    private bool mbIsServiceCall;
    private string msFileName;
    private bool mbIsReportGennerated;

    #endregion -- MEMBER(s) --

    #region -- CONSTRUCTOR(s) --

    public ReportDisplay(Constants.ExportReports aeReportName, string asFilter)
    {
        meReportName = aeReportName;
        msFilter = asFilter;
        moExportFormatType = ExportFormatType.Excel;
    }

    public ReportDisplay(Constants.ExportReports aeReportName, string asFilter, ExportFormatType asExportFormatType)
    {
        meReportName = aeReportName;
        msFilter = asFilter;
        moExportFormatType = asExportFormatType;
    }

    public ReportDisplay(Constants.ExportReports aeReportName, string asFilter, ExportFormatType asExportFormatType, string asFilePathAndName, bool abExportReport)
    {
        meReportName = aeReportName;
        msFilter = asFilter;
        moExportFormatType = asExportFormatType;
        msFilePathAndName = asFilePathAndName;
        mbExportReport = abExportReport;
    }

    public ReportDisplay()
    {        
    }

    #endregion -- CONSTRUCTOR(s) --

    #region -- Properties --

    public int TermId
    {
        get { return miTermId; }
        set { miTermId = value; }
    }

    public bool IsServiceCall
    {
        get { return mbIsServiceCall; }
        set { mbIsServiceCall = value; }
    }

    public string BasePath
    {
        get { return msBasePath; }
        set { msBasePath = value; }
    }

    public int SchoolId
    {
        get { return miSchoolId; }
        set { miSchoolId = value; }
    }

    public int AcademicYearId
    {
        get { return miAcademicYearId; }
        set { miAcademicYearId = value; }
    }

    public string SchoolName
    {
        get { return msSchoolName; }
        set { msSchoolName = value; }
    }

    public string FileName
    {
        get { return msFileName; }
        set { msFileName = value; }
    }

    public bool IsReportGennerated
    {
        get { return mbIsReportGennerated; }
    }

    public bool AllowSecondTermFromTermReport
    {
        get { return mbAllowSecondTermFromTermReport; }
        set { mbAllowSecondTermFromTermReport = value; }
    }

    #endregion

    #region -- PUBLIC METHOD(s) --

    /// <summary>
    /// This method is used to create selection formula for reports as well to display report.
    /// </summary>
    public void DisplayReport()
    {
        ConnectionInfo crConnectionInfo;
        TableLogOnInfos crtableLogoninfos;
        TableLogOnInfo crtableLogoninfo;
        Tables crTables = null;

        if (!IsServiceCall)
        {
            SchoolId = HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID].ToInt();

            if(miAcademicYearId == 0)
                AcademicYearId = HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();

            if (Session[Constants.S_SESSION_SCHOOL_NAME] != null)
                SchoolName = Session[Constants.S_SESSION_SCHOOL_NAME].ToString();
            else
                SchoolName = string.Empty;
        }

        try
        {
            crReportDocument = new ReportDocument();
            crConnectionInfo = new ConnectionInfo();
            crtableLogoninfos = new TableLogOnInfos();
            crtableLogoninfo = new TableLogOnInfo();

            crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ReportingDataSource"];
            crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["ReportDataBaseName"];
            crConnectionInfo.UserID = ConfigurationManager.AppSettings["ReportingUserId"];
            crConnectionInfo.Password = ConfigurationManager.AppSettings["ReportingPassword"];
            string sPath = string.Empty;

            if (IsServiceCall)
                sPath = msBasePath + GerReportName();
            else
                sPath = HttpContext.Current.Server.MapPath("~") + GerReportName();

            crReportDocument.Load(sPath);

            if (meReportName == Constants.ExportReports.StudentwiseProgressReport || meReportName == Constants.ExportReports.PrelimReport || meReportName == Constants.ExportReports.StudentTerm1ProgressReport || meReportName == Constants.ExportReports.StudentTerm2ProgressReport || meReportName == Constants.ExportReports.StudentwiseProgressReportFBS || meReportName == Constants.ExportReports.StudentwiseProgressReportPPSN || meReportName == Constants.ExportReports.PPSTermwiseReport || meReportName == Constants.ExportReports.PrelimReportPP || meReportName == Constants.ExportReports.HolosticProgressReportPPSNFor3to5)
                SetFinalProgressReportDataSource(msFilter);
            else
            {
                crTables = crReportDocument.Database.Tables;

                foreach (Table ocrTable in crTables)
                {
                    crtableLogoninfo = ocrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    ocrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                //This method add the parameters to the report.

                ApplyParametersToCrystalReport(msFilter);
            }

            if (!String.IsNullOrEmpty(msFilePathAndName))
            {
                crReportDocument.ExportToDisk(moExportFormatType, msFilePathAndName);
                if (mbExportReport)
                    crReportDocument.ExportToHttpResponse(moExportFormatType, HttpContext.Current.Response, true, Guid.NewGuid().ToString());
                else
                    mbIsReportGennerated = true;
            }
            else
                crReportDocument.ExportToHttpResponse(moExportFormatType, HttpContext.Current.Response, true, Guid.NewGuid().ToString());
        }
        catch (ThreadAbortException)
        {
            mbIsReportGennerated = true;
        }
        catch (Exception ex)
        {
            mbIsReportGennerated = true;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (crTables != null)
                crTables.Dispose();

            crtableLogoninfo = null;
            crtableLogoninfos = null;
            crConnectionInfo = null;

            if (crReportDocument != null)
            {
                crReportDocument.Close();
                crReportDocument.Dispose();
            }

            crReportDocument = null;
        }
    }

    #endregion -- PUBLIC METHOD(s) --

    #region -- PRIVATE METHOD(s) --

    private void SetFinalProgressReportDataSource(string asReportSelectionString)
    {
        asReportSelectionString = FormatFilterString(asReportSelectionString);
        String[] sFilters = asReportSelectionString.Split('@');
        string sParameterValue;
        string sParameterField;
        int iStandardId = 0;
        int iDivisionId = 0;
        int iStudentId = 0;
        string sNote = string.Empty;
        int iTermId = 0;
        foreach (string filter in sFilters)
        {
            if (filter.Equals(string.Empty))
                continue;

            sParameterValue = filter.Substring(filter.LastIndexOf("=") + 1);

            sParameterField = filter.Substring(filter.LastIndexOf(".") + 1, filter.LastIndexOf("=") - filter.LastIndexOf(".") - 1).Trim();

            switch (sParameterField)
            {
                case "Standard_Id":
                    iStandardId = sParameterValue.ToInt();
                    break;
                case "Division_Id":
                    iDivisionId = sParameterValue.ToInt();
                    break;
                case "StudentId":
                    iStudentId = sParameterValue.ToInt();
                    break;
                case "Note":
                    sNote = Convert.ToString(sParameterValue);
                    break;
                case "Term_Id":
                    iTermId = sParameterValue.ToInt();
                    break;
                case "Student_Id":
                    iStudentId = sParameterValue.ToInt();
                    break;
            }
        }

        //int iSchoolId = HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID].ToInt();
        //int iAcademicYearId = HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();
        int iSchoolId = SchoolId;
        int iAcademicYearId = AcademicYearId;
        DataSet dsProgressReportDetails = new DataSet();
        switch (meReportName)
        {
            case Constants.ExportReports.StudentwiseProgressReport:
                dsProgressReportDetails = ReportsBL.GetGradingProgressReportDataSet(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, 0);
                break;
            case Constants.ExportReports.StudentTerm1ProgressReport:
                iTermId = 1;
                dsProgressReportDetails = ReportsBL.GetMarkingSystemProgressReportDataSet(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, 0);
                break;
            case Constants.ExportReports.StudentTerm2ProgressReport:
                iTermId = 2;
                dsProgressReportDetails = ReportsBL.GetMarkingSystemProgressReportDataSet(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, 0);
                break;
            case Constants.ExportReports.StudentwiseProgressReportFBS:
                dsProgressReportDetails = ReportsBL.GetGradingProgressReportDataSetForFBS(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId);
                break;
            case Constants.ExportReports.StudentwiseProgressReportPPSN:
                dsProgressReportDetails = ReportsBL.GetGradingProgressReportDataSetForPPSN(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId);
                break;
            case Constants.ExportReports.PrelimReport:
                dsProgressReportDetails = ReportsBL.GetPreliminaryExaminationProgressReportDataSet(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, false);
                break;
            case Constants.ExportReports.PPSTermwiseReport:
                dsProgressReportDetails = ReportsBL.GetTermwiseProgressReportDataSet(miSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, 1, false);
                break;
            case Constants.ExportReports.PrelimReportPP:
                dsProgressReportDetails = ReportsBL.GetPrelimProgressReportDataSetForPP(miSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, 2, false);
                break;
            case Constants.ExportReports.HolosticProgressReportPPSNFor3to5:
                dsProgressReportDetails = ReportsBL.GetDetailsForHolisticReportForPPSH(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, iTermId, false);
                break;
        }

        crReportDocument.SetDataSource(dsProgressReportDetails);
    }

    public ReportDocument GetReportDocument(Constants.ExportReports aoExportReports, Dictionary<string, string> aoKVP)
    {
        ReportDocument crReportDocument = new ReportDocument();
        var crConnectionInfo = new ConnectionInfo();
        var crtableLogoninfos = new TableLogOnInfos();
        var crtableLogoninfo = new TableLogOnInfo();
        Tables crTables = null;

        try
        {
            crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ReportingDataSource"];
            crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["ReportDataBaseName"];
            crConnectionInfo.UserID = ConfigurationManager.AppSettings["ReportingUserId"];
            crConnectionInfo.Password = ConfigurationManager.AppSettings["ReportingPassword"];

            meReportName = aoExportReports;
            string sPath = Server.MapPath("~") + GerReportName();

            crReportDocument.Load(sPath);

            crTables = crReportDocument.Database.Tables;

            foreach (Table ocrTable in crTables)
            {
                crtableLogoninfo = ocrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                ocrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            foreach (KeyValuePair<string, string> kvp in aoKVP)
                crReportDocument.SetParameterValue(kvp.Key, kvp.Value);

            return crReportDocument;
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            crtableLogoninfo = null;
            crtableLogoninfos = null;
            crConnectionInfo = null;
            // crReportDocument = null;
            crTables = null;
        }
        return null;
    }

    //public ReportDocument GetReportDocument()
    //{
    //    ReportDocument crReportDocument = new ReportDocument();
    //    var crConnectionInfo = new ConnectionInfo();
    //    var crtableLogoninfos = new TableLogOnInfos();
    //    var crtableLogoninfo = new TableLogOnInfo();
    //    Tables crTables = null;

    //    try
    //    {
    //        crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ReportingDataSource"];
    //        crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["ReportDataBaseName"];
    //        crConnectionInfo.UserID = ConfigurationManager.AppSettings["ReportingUserId"];
    //        crConnectionInfo.Password = ConfigurationManager.AppSettings["ReportingPassword"];

    //        string sPath = Server.MapPath("~") + GerReportName();

    //        crReportDocument.Load(sPath);

    //        crTables = crReportDocument.Database.Tables;

    //        foreach (Table ocrTable in crTables)
    //        {
    //            crtableLogoninfo = ocrTable.LogOnInfo;
    //            crtableLogoninfo.ConnectionInfo = crConnectionInfo;
    //            ocrTable.ApplyLogOnInfo(crtableLogoninfo);
    //        }

    //        foreach (KeyValuePair<string, string> kvp in moKVP)
    //            crReportDocument.SetParameterValue(kvp.Key, kvp.Value);

    //        return crReportDocument;
    //    }
    //    catch (ThreadAbortException)
    //    {
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
    //    }
    //    finally
    //    {
    //        crtableLogoninfo = null;
    //        crtableLogoninfos = null;
    //        crConnectionInfo = null;
    //       // crReportDocument = null;
    //        crTables = null;
    //    }
    //    return null;
    //}

    private string GerReportName()
    {
        switch (meReportName)
        {
            case Constants.ExportReports.ChequeClearanceDetails:
                return "\\RITeSchool\\Report\\Fee\\ExportChequeClearanceDetails.rpt";
            case Constants.ExportReports.CautionMoneyDetails:
                return "\\RITeSchool\\Report\\Fee\\ExportCautionMoneyDetails.rpt";
            case Constants.ExportReports.StudentDetails:
                if (ConfigurationManager.AppSettings["SchoolId"].ToInt() == Convert.ToInt64(Constants.SchoolId.PPSN.ToInt()))
                    return "\\RITeSchool\\Report\\Student\\Export_Students_PPSN.rpt";
                else
                    return "\\RITeSchool\\Report\\Student\\Export_Students.rpt";
            case Constants.ExportReports.SalarySlipReport:
                return "\\RITeSchool\\Report\\Payroll\\SalarySlip.rpt";
            case Constants.ExportReports.LeavingCertificate:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate.rpt";
            case Constants.ExportReports.LeavingCertificateLFS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate LFS.rpt";
            case Constants.ExportReports.LeavingCertificateMCPS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate MCPS.rpt";
            case Constants.ExportReports.LeavingCertificateSS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate SS.rpt";
            case Constants.ExportReports.LeavingCertificatePP:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate PP.rpt";
            //case Constants.ExportReports.LeavingCertificateJPS:
            //    return "\\RITeSchool\\Report\\Student\\Leaving Certificate JPS.rpt";
            case Constants.ExportReports.LeavingCertificateDSK:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate DSK.rpt";
            case Constants.ExportReports.LeavingCertificateSNS:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateSNS.rpt";
            case Constants.ExportReports.LeavingCertificateSSN:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate SSN.rpt";
            case Constants.ExportReports.LeavingCertificateSSNMarathi:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate SSN_Marathi.rpt";
            case Constants.ExportReports.AdmissionLotteryDetails:
                return "\\RITeSchool\\Report\\Student\\AdmissionLotteryDetails.rpt";
            case Constants.ExportReports.LeavingCerificatePPSN:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificatePPSN.rpt";
            case Constants.ExportReports.LeavingCerificatePioneer:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificatePioneer.rpt";
            case Constants.ExportReports.LeavingCerificateVPMCPS:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateVPMCPS.rpt";
            case Constants.ExportReports.AdmissionFormReport:
                if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.PPS.ToInt()))
                {
                    //if(miAcademicYearId >= 56)
                        return "\\RITeSchool\\Report\\Student\\OnlineAdmissionFormAdministrationCopyPPS_2024.rpt";
                    //else
                    //    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionFormAdministrationCopyPPS.rpt";
                }
                else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.DSK.ToInt()))
                    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionFormDSK.rpt";
                else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.CSNP.ToInt()) ||
                    ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.CSNS.ToInt()))
                    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionFormCSNP.rpt";
                else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.PEMS.ToInt()))
                    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionFormPEMS.rpt";
                else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.DPIS.ToInt()) || ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.DPISRAVET.ToInt()))
                    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionForm_DPIS.rpt";
                else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.SNS.ToInt()))
                    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionForm_SNS.rpt";
                //else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.PPSH.ToInt()))
                //    return "\\RITeSchool\\Report\\Payroll\\StaffLeaveReportInExcel.rpt";
                //else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.DPIS.ToInt()))
                //    return "\\RITeSchool\\Report\\Student\\Leaving Certificate DPIS.rpt";
                else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.PPSH.ToInt()))
                    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionFormForPPSH.rpt";
                else if (ConfigurationManager.AppSettings["SchoolId"].ToString() == Convert.ToString(Constants.SchoolId.PPSN.ToInt()))
                    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionForm_PPSN.rpt";
                else
                    return "\\RITeSchool\\Report\\Student\\OnlineAdmissionForm.rpt";
            case Constants.ExportReports.StudentwiseProgressReport:
                return "\\RITeSchool\\Report\\Exam\\StudentwiseProgressReport.rpt";
            case Constants.ExportReports.StudentwiseProgressReportSS:
                return "\\RITeSchool\\Report\\Exam\\StudentwiseProgressReportSS.rpt";
            case Constants.ExportReports.OnlineTransactionClearanceDetails:
                return "\\RITeSchool\\Report\\Fee\\ExportOnlineTransactionClearanceDetails.rpt";
            case Constants.ExportReports.CardPaymentDetails:
                return "\\RITeSchool\\Report\\Fee\\ExportCardPaymentDetails.rpt";
            case Constants.ExportReports.ExportCashPayment:
                return "\\RITeSchool\\Report\\Fee\\ExportCashPaymentDetails.rpt";
            case Constants.ExportReports.StudentTerm1ProgressReport:
                return "\\RITeSchool\\Report\\Exam\\StudentTerm1ProgressReport.rpt";
            case Constants.ExportReports.StudentTerm2ProgressReport:
                return "\\RITeSchool\\Report\\Exam\\StudentTerm1ProgressReport.rpt";
            case Constants.ExportReports.OutofAcademicYearStudentList:
                return "\\RITeSchool\\Report\\Student\\OutofAcademicYearStudentList.rpt";
            case Constants.ExportReports.PendingFeeReminder:
                return "\\RITeSchool\\Report\\Fee\\ExportStudentPendingFee.rpt";
            case Constants.ExportReports.StudentwiseProgressReportFBS:
                return "\\RITeSchool\\Report\\Exam\\FBS\\StudentwiseProgressReport.rpt";
            case Constants.ExportReports.StudentwiseProgressReportPPSN:
                return "\\RITeSchool\\Report\\Exam\\StudentwiseProgressReportForPPSN.rpt";
            case Constants.ExportReports.FormNo16Report:
                return "\\RITeSchool\\Report\\Payroll\\IncomeTaxDetails.rpt";
            case Constants.ExportReports.ElectronicPaymentDetails:
                return "\\RITeSchool\\Report\\Fee\\ExportElectronicPaymentDetails.rpt";
            case Constants.ExportReports.ConsolidatedStudentAdmissionList:
                return "\\RITeSchool\\Report\\Student\\ConsolidatedStudentAdmissionList.rpt";
            case Constants.ExportReports.ServiceContract:
                return "\\RITeSchool\\Report\\Payroll\\ServiceContract.rpt";
            case Constants.ExportReports.AppointmentLetter:
                return "\\RITeSchool\\Report\\Payroll\\AppointmentLetter.rpt";
            case Constants.ExportReports.LessonPlan:
                return "\\RITeSchool\\Report\\LessonPlan\\LessonPlan.rpt";
            //case Constants.ExportReports.StudentwiseProgressReportPPSH:
            //    return "\\RITeSchool\\Report\\Exam\\StudentWiseProgressReportPPSH.rpt";
            case Constants.ExportReports.StudentwiseProgressReportPPSH:
                return "\\RITeSchool\\Report\\Exam\\StudentFinalProgressReport6to8_PPSH.rpt";
            case Constants.ExportReports.StudentwiseProgressReportPPSH_9th:
                return "\\RITeSchool\\Report\\Exam\\StudentFinalProgressReport9thStd_PPSH.rpt";
            case Constants.ExportReports.StudentFinalProgressReport9thStd_PPSH_AY10:
                return "\\RITeSchool\\Report\\Exam\\StudentFinalProgressReport9thStd_PPSH_AY10.rpt";
            case Constants.ExportReports.StudentwiseProgressReportPPSH_1stTO5th:            
                return "\\RITeSchool\\Report\\Exam\\StudentsObsrvationDetailsReport_PPSH.rpt";
            case Constants.ExportReports.StudentwiseProgressReportPPSH_Xseed:
                return "\\RITeSchool\\Report\\Xseed\\XseedProgressReportForPPSH.rpt";
            case Constants.ExportReports.XseedProgressReport_PPS:
                return "\\RITeSchool\\Report\\Xseed\\XseedProgressReport.rpt";
            case Constants.ExportReports.StudentCautionMoneySNS:
                return "\\RITeSchool\\Report\\Fee\\CautionMoneyDetails_SNS.rpt";
            case Constants.ExportReports.SchoolGuestDetails:
                return "\\RITeSchool\\Report\\School Configuration\\GuestDetailsForGatePass.rpt";
            case Constants.ExportReports.SchoolGuestDetailsForExport:
                return "\\RITeSchool\\Report\\School Configuration\\SchoolGuestDetailsForExport.rpt";
            case Constants.ExportReports.StudentwiseProgressReportPKIS:
                return "\\RITeSchool\\Report\\Exam\\StudentWiseProgressReportPKSC.rpt";
            case Constants.ExportReports.TestwiseReport:
                return "\\RITeSchool\\Report\\Exam\\StudentWiseTestReportSPS.rpt";
            case Constants.ExportReports.PurchaseOrder:
                return "\\RITeSchool\\Report\\Inventory\\PurchaseOrder.rpt";
            case Constants.ExportReports.LeavingCertificateSPS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate SPS.rpt";
            case Constants.ExportReports.LeavingCertificateOWS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate OWS.rpt";
             case Constants.ExportReports.LeavingCertificateDPIS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate DPIS.rpt";
            case Constants.ExportReports.StudentwiseProgressReportSVP:
                return "\\RITeSchool\\Report\\Exam\\StudentWiseProgressReportSVP.rpt";
            case Constants.ExportReports.StudentwiseProgressReportSVP_9:
                return "\\RITeSchool\\Report\\Exam\\StudentWiseFinalProgressReportSVP9.rpt";
            case Constants.ExportReports.LeavingCertificateCSNP:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate CSNP.rpt";
            case Constants.ExportReports.LeavingCertificateSVP:
                return "\\RITeSchool\\Report\\Student\\Leaving CertificateForSVP.rpt";
            case Constants.ExportReports.ClasswiseBankChallan:
                return "\\RITeSchool\\Report\\Fee\\BankChallan.rpt";
            case Constants.ExportReports.TransferCertificatePPSH:
                return "\\RITeSchool\\Report\\Student\\TransferCertificatePPSH.rpt";
            case Constants.ExportReports.LeavingCertificatePPSH:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateForPPSH.rpt";
            case Constants.ExportReports.LeavingCertificateHSP:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateHSP.rpt";
            case Constants.ExportReports.LeavingCertificateMVPS:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateMVPS.rpt";
            case Constants.ExportReports.StudentRegistrationForm:
                return "\\RITeSchool\\Report\\Student\\StudentRegistrationForm_SPS.rpt";
            case Constants.ExportReports.StudentRegistrationFeeReceipt:
                return "\\RITeSchool\\Report\\Student\\StudentRegistrationReceipt_SPS.rpt";
            case Constants.ExportReports.StudentAdmissionFormSPS:
                return "\\RITeSchool\\Report\\Student\\StudentAdmissionForm_SPS.rpt";
            case Constants.ExportReports.StudentAdmissionConfirmation:
                return "\\RITeSchool\\Report\\Student\\StudentAdmissionConfirmationReport.rpt";
            case Constants.ExportReports.StudentwiseTermProgressReportSVP:
                return "\\RITeSchool\\Report\\Exam\\StudentWiseTermProgressReportSVP.rpt";
            case Constants.ExportReports.LeavingCertificateJPS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate_JPS.rpt";
            case Constants.ExportReports.StudentwiseProgressReportMNS:
                return "\\RITeSchool\\Report\\Exam\\StudentwiseProgressReportMNS.rpt";
            case Constants.ExportReports.StudentwiseTermProgressReportSNS_1rdTO5th2024:
                return "\\RITeSchool\\Report\\Exam\\StudentwiseTermProgressReportSNS_1rdTO5th2024.rpt";
            case Constants.ExportReports.StudentwiseProgressReportPPS:
                {
                    if (TermId == Constants.I_ONE)
                    {
                        if (string.IsNullOrEmpty(msFileName))
                            return "\\RITeSchool\\Report\\Exam\\StudentTerm1ProgressReport.rpt";
                        else
                            return "\\RITeSchool\\Report\\Exam\\"+FileName;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(msFileName))
                            return "\\RITeSchool\\Report\\Exam\\StudentFinalProgressReportMarking48.rpt";
                        else
                            return "\\RITeSchool\\Report\\Exam\\" + msFileName;
                    }
                }
            case Constants.ExportReports.StudentwiseProgressReportPPS_Grading:
                {
                    if (TermId == Constants.I_ONE)
                    {
                        if (string.IsNullOrEmpty(msFileName))
                            return "\\RITeSchool\\Report\\Exam\\StudentwiseProgressReport.rpt";
                        else
                            return "\\RITeSchool\\Report\\Exam\\" + FileName;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(msFileName))
                            return "\\RITeSchool\\Report\\Exam\\StudentFinalProgressReportGrading.rpt";
                        else
                            return "\\RITeSchool\\Report\\Exam\\" + msFileName;
                    }
                }
            case Constants.ExportReports.StudentFeeReceiptForZLSP :
                return "\\RITeSchool\\Report\\Fee\\FeeReceiptDetails_ZLSP.rpt";
            case Constants.ExportReports.LeavingCertificateJOS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate_JOS.rpt";
            case Constants.ExportReports.LeavingCertificateForZeal:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateForZeal.rpt";
            case Constants.ExportReports.ClasswiseBankChallan_Aaryan:
                return "\\RITeSchool\\Report\\Fee\\StudentsFeeChallan.rpt";
            case Constants.ExportReports.StudentwiseProgressReport_Aaryan:
                return "\\RITeSchool\\Report\\Exam\\TermwiseStudentMarkDetails_1to4.rpt";
            case Constants.ExportReports.StudentwiseProgressReportAaryan_5to8:
                return "\\RITeSchool\\Report\\Exam\\TermwiseStudentMarkDetails_5to8.rpt";
			case Constants.ExportReports.ItemDetailsReport:
                return "\\RITeSchool\\Report\\Inventory\\ItemDetailsReport.rpt";
			case Constants.ExportReports.EmployeeDetailsReport:
                return "\\RITeSchool\\Report\\School Configuration\\EmployeeDetailsReport.rpt";
            case Constants.ExportReports.PrelimReport:
                return "\\RITeSchool\\Report\\Exam\\PreliminaryExaminationReport.rpt";
            case Constants.ExportReports.LeavingCertificateForAryan:
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateForAryan.rpt";
            case Constants.ExportReports.LeavingCertificateForBFS:
                return "\\RITeSchool\\Report\\Student\\Leaving Certificate_BFS.rpt";
            case Constants.ExportReports.LeavingCertificateForDYPV:   // 
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateFor_DYPV.rpt";
            case Constants.ExportReports.LeavingCertificateNurseryTo9th_NPS:   // nps
                return "\\RITeSchool\\Report\\Student\\LeavingCertificateNurseryTo9th_NPS.rpt";
            case Constants.ExportReports.FinalProgressReportNPS:
                return "\\RITeSchool\\Report\\Exam\\StudentWiseProgressReportNPS.rpt";
            case Constants.ExportReports.VehicleBillingDetails:
                return "\\RITeSchool\\Report\\Transport\\MonthwiseVehicleBillingDetails.rpt";
            case Constants.ExportReports.MaterialwiseStockDetails:
                return "\\RITeSchool\\Report\\Inventory\\CategorywiseItemStockDetails.rpt";
            case Constants.ExportReports.GSTInvoiceDetails:
                return "\\RITeSchool\\Report\\Inventory\\GSTInvoice.rpt";
            case Constants.ExportReports.PPSTermwiseReport:
                return "\\riteschool\\report\\Exam\\TermwiseProgressReportPP.rpt";                       
			case Constants.ExportReports.PODetails:
                return "\\RITeSchool\\Report\\Inventory\\ExternalPODetails.rpt";
            case Constants.ExportReports.PrelimReportPP:
                return "\\RITeSchool\\Report\\Exam\\PrelimProgressReportPP.rpt";
            case Constants.ExportReports.FinalReportPP :
                return "\\riteschool\\report\\Exam\\FinalProgressReportPP.rpt";
            case Constants.ExportReports.IncomeTaxReconciliation:
                return "\\RITeSchool\\Report\\Fee\\IncomeTaxReconciliationStatement.rpt";
            case Constants.ExportReports.CancellationFormDetails:
                return "\\RITeSchool\\Report\\Student\\AdmissionCancellationForm.rpt";
            case Constants.ExportReports.InternalFeeReceipt:
                return "\\RITeSchool\\Report\\Fee\\InternalFeeReceipt.rpt";
            case Constants.ExportReports.CautionMoneyReceipt:
                return "\\RITeSchool\\Report\\Fee\\CautionMoneyFeeReceipt.rpt";
			case Constants.ExportReports.ConfirmationLetter:
                return "\\RITeSchool\\Report\\Payroll\\ConfirmationLetter.rpt";
			case Constants.ExportReports.InauguralCertificate:
                return "\\RITeSchool\\Report\\Student\\InauguralCertificate.rpt";
            case Constants.ExportReports.InternalFeeReceiptSNS:
                return "\\RITeSchool\\Report\\Fee\\InternalFeeReceiptForReport.rpt";
            case Constants.ExportReports.BonafideRequestApplication:
                return "\\RITeSchool\\Report\\Student\\RequestApplicationForBonafide.rpt";
            case Constants.ExportReports.VehicleDocumentDetails:
                return "\\RITeSchool\\Report\\Transport\\VehicleDocumentDetails.rpt";
            case Constants.ExportReports.ExportVehicleDetails:
                return "\\RITeSchool\\Report\\Transport\\ExportVehicleDetails.rpt";
            case Constants.ExportReports.ExportStudentMonthlyDetails:
                return "\\RITeSchool\\Report\\Student\\StudentMonthlyStatusReport.rpt";
            case Constants.ExportReports.StudentCautionMoneySNSForStudentLogin:
                return "RITeSchool\\Report\\Fee\\CautionMoneyDetailsForStudentLogin_SNS.rpt";
			case Constants.ExportReports.StudentFeeReceipt:
                return "\\RITeSchool\\Report\\Fee\\FeeReceiptVPMCPS.rpt";
            case Constants.ExportReports.StudentwiseProgressReportPioneer_NurseryTO2nd:
                return "\\RITeSchool\\Report\\Exam\\StudentwiseTermProgressReportPrimaryPioneer.rpt";
            case Constants.ExportReports.HalfYearlyReportFor3To9Pioneer:
                return "\\RITeSchool\\Report\\Exam\\StudentHalfYearlyReportFor3To9ForPioneer.rpt";
            case Constants.ExportReports.HolosticProgressReportPPSNFor3to5:
                return "RITeSchool\\Report\\Exam\\StudentHolisticReportForPPSH.rpt";
            case Constants.ExportReports.EnquiryFormReport:
                return "\\RITeSchool\\Report\\Student\\StudentEnquiryFormReport_SNS.rpt";
             default:
                return string.Empty;
        }
    }

    /// <summary>
    /// This method is called for applying parameters such as school name,
    /// organisation name and academic year to report.
    /// </summary>
    private void ApplyParametersToCrystalReport(string asReportSelectionString)
    {
        string sParameterValue;
        string sParameterField;
        string sSubReportName;
        int iSubreportCount = 0;

        ParameterFieldDefinition oParameterFieldDefinition;
        ParameterFieldDefinitions ApplyParameterFieldDefinations = crReportDocument.DataDefinition.ParameterFields;
        ParameterDiscreteValue ApplyParameterDiscreteValue = new ParameterDiscreteValue();
        ParameterValues ApplyParameterValue = new ParameterValues();
        asReportSelectionString = FormatFilterString(asReportSelectionString);
        String[] sFilter = asReportSelectionString.Split('@');

        if (meReportName == Constants.ExportReports.StudentwiseProgressReport || meReportName == Constants.ExportReports.StudentwiseProgressReportPPSN)
        {
            iSubreportCount = 1;
        }
        if (meReportName == Constants.ExportReports.StudentTerm1ProgressReport || meReportName == Constants.ExportReports.StudentTerm2ProgressReport)
        {
            iSubreportCount = 2;
        }
        //int aiSchoolId = HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID].ToInt();
        int aiSchoolId = SchoolId;
        if ((aiSchoolId != Constants.SchoolId.PPS.ToInt() || (aiSchoolId == Constants.SchoolId.PPS.ToInt() && TermId == Constants.I_ZERO)))
        {
            for (int index = 0; index <= iSubreportCount; index++)
            {
                sSubReportName = GetSubReportName(index);

                for (int iCount = 0; iCount < sFilter.Length; iCount++)
                {
                    if (!sFilter[iCount].Equals(string.Empty))
                    {
                        string sXMLString = sFilter[iCount].Substring(sFilter[iCount].IndexOf("=") + 1);
                        // if parameter is of "XML" type
                        if (sXMLString.Contains("xmlns:"))
                        {
                            sParameterValue = sXMLString;
                            sParameterField = (sFilter[iCount].Substring(sFilter[iCount].IndexOf(".") + 1, sFilter[iCount].IndexOf("=") - sFilter[iCount].IndexOf(".") - 1)).Trim();
                        }
                        else
                        {

                            sParameterValue = sFilter[iCount].Substring(sFilter[iCount].IndexOf("=") + 1);
                            if (meReportName == Constants.ExportReports.PendingFeeReminder)
                                sParameterField = sFilter[iCount].Substring(sFilter[iCount].IndexOf(".") + 1, sFilter[iCount].IndexOf("=") - sFilter[iCount].IndexOf(".") - 1);
                            else
                                sParameterField = sFilter[iCount].Substring(sFilter[iCount].LastIndexOf(".") + 1, sFilter[iCount].IndexOf("=") - sFilter[iCount].LastIndexOf(".") - 1).Trim();
                        }

                        oParameterFieldDefinition = (ParameterFieldDefinition)ApplyParameterFieldDefinations[sParameterField];
                        if (sParameterValue.Trim() == "null")
                        {
                            ApplyParameterDiscreteValue.Value = null;
                            crReportDocument.SetParameterValue(sParameterField, null);
                        }
                        else
                        {
                            sParameterValue = sParameterValue.Trim();
                            ApplyParameterDiscreteValue.Value = sParameterValue;
                            if (sSubReportName == string.Empty)
                                crReportDocument.SetParameterValue(sParameterField, sParameterValue);
                            else
                                crReportDocument.SetParameterValue(sParameterField, sParameterValue, sSubReportName);
                        }
                        ApplyParameterValue.Add(ApplyParameterDiscreteValue);
                        oParameterFieldDefinition.ApplyCurrentValues(ApplyParameterValue);
                    }
                }

            }

            if (meReportName == Constants.ExportReports.StudentwiseProgressReportSS || meReportName == Constants.ExportReports.FormNo16Report)
                oParameterFieldDefinition = AddReportParameters(ApplyParameterFieldDefinations, ApplyParameterDiscreteValue, ApplyParameterValue);
            else
                oParameterFieldDefinition = null;
            oParameterFieldDefinition = null;
            ApplyParameterFieldDefinations = null;
            ApplyParameterDiscreteValue = null;
            ApplyParameterValue = null;
        }
        else
        {
            var dsFeeRecieptReportDetails = new DataSet();

            int iSchoolId = Constants.I_ZERO;
            int iAcademicYearId = Constants.I_ZERO;
            int iStandardId = Constants.I_ZERO;
            int iDivisionId = Constants.I_ZERO;
            int iStudentId = Constants.I_ZERO;
            int iTermId = Constants.I_ZERO;
            string sNote = string.Empty;
			int iIsFromReportScreen = 0;

            foreach (String filter in sFilter)
            {
                sParameterValue = filter.Substring(filter.LastIndexOf("=") + 1);
                sParameterField = filter.Substring(filter.LastIndexOf(".") + 1, filter.LastIndexOf("=") - filter.LastIndexOf(".") - 1).Trim();

                switch (sParameterField)
                {
                    case "School_Id":
                        iSchoolId = sParameterValue.ToInt();
                        break;
                    case "Academic_Year_Id":
                        iAcademicYearId = sParameterValue.ToInt();
                        break;
                    case "Standard_Id":
                        iStandardId = sParameterValue.ToInt();
                        break;
                    case "Division_Id":
                        iDivisionId = sParameterValue.ToInt();
                        break;
                    case "StudentId":
                        iStudentId = sParameterValue.ToInt();
                        break;
                    case "Term_Id":
                        iTermId = sParameterValue.ToInt();
                        break;
                    case "Note":
                        sNote = sParameterValue;
                        break;
					case "IsFromReportScreen":
                        iIsFromReportScreen = sParameterValue.ToInt();
                        break;

                }
            }

            bool bIsGradingstandard = StandardMasterBL.IsGradingStandard(iSchoolId, iAcademicYearId, iStandardId);

            if (TermId == Constants.I_ONE)
            {
                if (bIsGradingstandard)
                    dsFeeRecieptReportDetails = ReportsBL.GetGradingProgressReportDataSet(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, iIsFromReportScreen);
                else
                    dsFeeRecieptReportDetails = ReportsBL.GetMarkingSystemProgressReportDataSet(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, iIsFromReportScreen);
            }
            else if (TermId == Constants.I_TWO && mbAllowSecondTermFromTermReport == true && bIsGradingstandard)
                dsFeeRecieptReportDetails = ReportsBL.GetGradingProgressReportDataSet(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, TermId, iIsFromReportScreen);
            else
                dsFeeRecieptReportDetails = ReportsBL.GetProgressReportDataSet(iSchoolId, iAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, false);


            crReportDocument.SetDataSource(dsFeeRecieptReportDetails);
        }
    }

    /// <summary>
    /// This method adds parameters to each report(School Name , Organization Name, Academic Year).
    /// </summary>
    /// <param name="aParameterFieldDefinations"> </param>
    /// <param name="aApplyParameterDiscreteValue"> </param>
    /// <param name="aApplyParameterValue"> </param>
    /// <returns> ParameterFieldDefinition </returns>
    private ParameterFieldDefinition AddReportParameters(ParameterFieldDefinitions aParameterFieldDefinations, ParameterDiscreteValue aApplyParameterDiscreteValue, ParameterValues aApplyParameterValue)
    {
        InitializeMemberVariables();

        if (meReportName == Constants.ExportReports.StudentwiseProgressReportSS)
        {
            //string sSchoolName = Session[Constants.S_SESSION_SCHOOL_NAME].ToString();
            string sSchoolName = SchoolName;
            var oSchoolAcademicYearBL = new SchoolWiseAcademicYearMasterBL();
            DataTable oDTSchoolInfo = oSchoolAcademicYearBL.GetSchoolInfo(miSchoolId, miAcademicYearId);
            string msAcademicYearName = "Year " + oDTSchoolInfo.Rows[Constants.I_ZERO]["Year"];
            string msOrgnizationName = oDTSchoolInfo.Rows[Constants.I_ZERO]["School_Orgn_Name"].ToString();
            ParameterFieldDefinition oParameterFieldDefinition;
            oParameterFieldDefinition = aParameterFieldDefinations["SchoolName"];
            aApplyParameterDiscreteValue.Value = sSchoolName;
            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
            crReportDocument.SetParameterValue("SchoolName", sSchoolName);
            oParameterFieldDefinition = aParameterFieldDefinations["AcademicYear"];
            aApplyParameterDiscreteValue.Value = msAcademicYearName;
            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
            crReportDocument.SetParameterValue("AcademicYear", msAcademicYearName);
            oParameterFieldDefinition = aParameterFieldDefinations["Organisation Name"];
            aApplyParameterDiscreteValue.Value = msOrgnizationName;
            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
            crReportDocument.SetParameterValue("Organisation Name", msOrgnizationName);
            return oParameterFieldDefinition;
        }
        else if (meReportName == Constants.ExportReports.FormNo16Report)
        {
            ParameterFieldDefinition oParameterFieldDefinition;
            oParameterFieldDefinition = aParameterFieldDefinations["IsFormNo16"];
            aApplyParameterDiscreteValue.Value = "0";
            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
            return oParameterFieldDefinition;
        }
        return null;
    }

    private string GetSubReportName(int aiIndex)
    {
        switch (aiIndex)
        {
            case 1:
                if (meReportName == Constants.ExportReports.StudentwiseProgressReport)
                    return "SubReportOfGradeDetails.rpt";
                else if (meReportName == Constants.ExportReports.StudentTerm1ProgressReport || meReportName == Constants.ExportReports.StudentTerm2ProgressReport)
                    return "SubReportOfFinalGradeDetails.rpt";
                break;
            case 2:
                if (meReportName == Constants.ExportReports.StudentTerm1ProgressReport)
                    return "SubReportOfTerm1ResultGraph.rpt";
                else if (meReportName == Constants.ExportReports.StudentTerm2ProgressReport)
                    return "SubReportOfTerm2ResultGraph.rpt";
                break;
        }
        return string.Empty;
    }

    /// <summary>
    /// This method is used to convert string into particular format.
    /// </summary>
    /// <param name="sFilter"></param>
    /// <returns>string</returns>
    private string FormatFilterString(string asFilterString)
    {
        asFilterString = asFilterString.Replace("AND", "@");
        asFilterString = asFilterString.Replace("OR", "@");
        asFilterString = asFilterString.Replace("(", "");
        asFilterString = asFilterString.Replace(")", "");

        asFilterString = asFilterString.Replace("{", "");
        asFilterString = asFilterString.Replace("}", "");
        asFilterString = asFilterString.Remove(asFilterString.Length - 1);
        return asFilterString;
    }

    #endregion -- PRIVATE METHOD(s) --
}
