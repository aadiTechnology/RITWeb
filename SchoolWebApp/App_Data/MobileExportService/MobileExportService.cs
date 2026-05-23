using BusinessLogic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SchoolEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Hosting;
using Utility;


namespace MobileExportService.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MobileReceiptService" in code, svc and config file together.
    public class MobileExportService : IMobileExportService
    {
        #region Data Member(s)

        private ReportDocument crReportDocument;
        private int miSchoolId;
        private int miAcademicYearId;
        private int miStudentId;
        private string msStandardName;
        private int miStdDivId;
        private int miStandardId;
        private int miAssessmentId;
        private bool mbIsLateJoinee;
        List<string> mlstPioneerGradeReportStandards = new List<string> { "Nursery", "Junior KG", "Senior KG", "1", "2" };
        
        #endregion

        #region Constant(s)
        
        private const int I_PPS_2022_23 = 53;
        private const int I_PPS_2023_24 = 54;
        private const int I_PPS_2025_26 = 56;

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to return receipt file name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asReceiptNo"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiAccountHeaderId"></param>
        /// <param name="aiIsRefundFee"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asSerialNo"></param>
        /// <returns></returns>
        public string GetReceiptFileName(int aiSchoolId, string asReceiptNo, int aiAcademicYearId, int aiAccountHeaderId, string aiIsRefundFee, int aiStudentId, string asSerialNo)
        {
            return DisplayReport(aiSchoolId, asReceiptNo, aiAcademicYearId, aiAccountHeaderId, aiIsRefundFee, aiStudentId, asSerialNo, 0);
        }

        public string GetAdmissionReceiptFileName(int aiSchoolId, int aiAcademicYearId, int aiAdmissionId)
        {
            return DisplayReport(aiSchoolId, "0", aiAcademicYearId, 0, "0", 0, "0", aiAdmissionId);
        }

        /// <summary>
        /// This method is used to return challan file name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiStandardDivID"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="asPayableFor"></param>
        /// <returns></returns>
        public string GetChallanFileName(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStandardDivID, int aiStudentId, string asPayableFor)
        {
            string sFilterStr = "(usp_GetFeeChallanDetailsForAaryan.School_Id}=" + aiSchoolId + "AND usp_GetFeeChallanDetailsForAaryan.Academic_Year_Id}=" + aiAcademicYearId + "AND usp_GetFeeChallanDetailsForAaryan.Standard_Id}=" + aiStandardId + "AND usp_GetFeeChallanDetailsForAaryan.SchoolWise_Standard_Division_Id}=" + aiStandardDivID + "AND usp_GetFeeChallanDetailsForAaryan.Student_Id}=" + aiStudentId + " AND usp_GetFeeChallanDetailsForAaryan.PayableFor}=" + asPayableFor + ") @";

            string sFileName = @"\RITeSchool\OtherDownloads\ChallanDownloads\" + "Challan_" + GetDateFormat() + ".pdf";
            string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ClasswiseBankChallan_Aaryan, sFilterStr, ExportFormatType.PortableDocFormat, sDownloadPath, false);
            oReportDisplay.IsServiceCall = true;
            oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
            oReportDisplay.SchoolId = aiSchoolId;
            oReportDisplay.AcademicYearId = aiAcademicYearId;
            oReportDisplay.SchoolName = string.Empty;
            oReportDisplay.DisplayReport();

            return sFileName;
        }

        public string GetReportFileNameInFormat(int aiSchoolId, int aiAcademicYearId, int aiLoginUserId, int aiReportId, Constants.ExportReports aoExportReports, List<ParameterPair> aoParameterPairs, int aiExportFormatType)
        {
            if (aoParameterPairs.Count > 0)
            {
                string sRecordSelectionFormula = GetRecordSelectionFormula(aiSchoolId, aiAcademicYearId, aiLoginUserId, aiReportId, aoExportReports, aoParameterPairs);

                 ExportFormatType oExportFormatType = ExportFormatType.PortableDocFormat;
                if (aiExportFormatType != 0)
                    oExportFormatType = (ExportFormatType)aiExportFormatType;

                string sExtension = ".pdf";
                if (oExportFormatType == ExportFormatType.RichText)
                    sExtension = ".doc";
                else if (oExportFormatType == ExportFormatType.Excel)
                    sExtension = ".xls";

                string sFileName = @"\RITeSchool\OtherDownloads\AllDownloads\" + aoExportReports.ToString() + "_" + GetDateFormat() + sExtension;
                string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

                ReportDisplay oReportDisplay = new ReportDisplay(aoExportReports, sRecordSelectionFormula, oExportFormatType, sDownloadPath, false);
                oReportDisplay.IsServiceCall = true;
                oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
                oReportDisplay.SchoolId = aiSchoolId;
                oReportDisplay.AcademicYearId = aiAcademicYearId;
                oReportDisplay.SchoolName = string.Empty;
                oReportDisplay.DisplayReport();

                return "1:" + sFileName;
            }
            else
                return "0:Parameter list is blank.";
        }

        public string GetReportFileName(int aiSchoolId, int aiAcademicYearId, int aiLoginUserId, int aiReportId, Constants.ExportReports aoExportReports, List<ParameterPair> aoParameterPairs)        
        {
            if (aoParameterPairs.Count > 0)
            {                
                string sRecordSelectionFormula = GetRecordSelectionFormula(aiSchoolId, aiAcademicYearId, aiLoginUserId, aiReportId, aoExportReports, aoParameterPairs);
                ExportFormatType oExportFormatType = GetFileType(aoExportReports,aiSchoolId,aiReportId); 

                string sFileName = @"\RITeSchool\OtherDownloads\AllDownloads\" + aoExportReports.ToString() + "_" + GetDateFormat() + ".pdf";
                string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;
                ReportDisplay oReportDisplay = new ReportDisplay(aoExportReports, sRecordSelectionFormula, oExportFormatType, sDownloadPath, false);
                oReportDisplay.IsServiceCall = true;
                oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
                oReportDisplay.SchoolId = aiSchoolId;
                oReportDisplay.AcademicYearId = aiAcademicYearId;
                oReportDisplay.SchoolName = string.Empty;
                oReportDisplay.DisplayReport();

                return "1:" + sFileName;
            }
            else
                return "0:Parameter list is blank.";
        }

        private ExportFormatType GetFileType(Constants.ExportReports aoExportReports, int aiSchoolId, int aiReportId)
        {           
            return ExportFormatType.PortableDocFormat;
        }

        private string GetRecordSelectionFormula(int aiSchoolId, int aiAcademicYearId, int aiLoginUserId, int aiReportId, Constants.ExportReports aoExportReports, List<ParameterPair> aoParameterPairs)
        {
            string sRecordSelectionFormula = string.Empty;
            var aoDictParameters = aoParameterPairs != null
            ? aoParameterPairs.ToDictionary(p => p.Name, p => p.Value)
            : new Dictionary<string, string>();

            if (aoExportReports == Constants.ExportReports.LessonPlan)
            {
                int iSubjectId = 0, iStdDivId = 0;

                if (aoDictParameters["StartDate"] == null & aoDictParameters["EndDate"] == null)
                {
                    sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + aiSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + aiAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + aoDictParameters["UserId"] + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=" + iSubjectId + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=" + iStdDivId +
                            " AND usp_GetLessonPlanDetailsForReport.StartDate}=null" + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=null" + ")" + "@ ";
                }
                else if (aoDictParameters["StartDate"] != null && aoDictParameters["EndDate"] == null)
                {
                    sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + aiSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + aiAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + aoDictParameters["UserId"] + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=" + iSubjectId + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=" + iStdDivId +
                            " AND usp_GetLessonPlanDetailsForReport.StartDate}=" + aoDictParameters["StartDate"] + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=null" + ")" + "@ ";
                }
                else if (aoDictParameters["StartDate"] == null && aoDictParameters["EndDate"] != null)
                {
                    sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + aiSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + aiAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + aoDictParameters["UserId"] + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=" + iSubjectId + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=" + iStdDivId +
                            " AND usp_GetLessonPlanDetailsForReport.StartDate}=null" + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=" + aoDictParameters["EndDate"] + ")" + "@ ";
                }
                else
                {
                    sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + aiSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + aiAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + aoDictParameters["UserId"] + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=" + iSubjectId + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=" + iStdDivId +
                            " AND usp_GetLessonPlanDetailsForReport.StartDate}=" + aoDictParameters["StartDate"] + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=" + aoDictParameters["EndDate"] + ")" + "@ ";
                }
            }
            else if (aoExportReports == Constants.ExportReports.CancellationFormDetails)
            {
                sRecordSelectionFormula = "(usp_GetCancellationFormDetails.School_Id}=" + aiSchoolId + "AND usp_GetCancellationFormDetails.Academic_Year_Id}=" + aiAcademicYearId + "AND usp_GetCancellationFormDetails.Standard_Id}=" + aoDictParameters["Standard_Id"] + "AND usp_GetCancellationFormDetails.Division_Id}=" + aoDictParameters["Division_Id"] + "AND usp_GetCancellationFormDetails.Student_Id}=" + aoDictParameters["Student_Id"] + "AND usp_GetCancellationFormDetails.SubmittedBy}=" + aoDictParameters["SubmittedBy"] + aoDictParameters["CancFormId"] + ") @";
            }
            else if (aoExportReports == Constants.ExportReports.FormNo16Report)
            {
                sRecordSelectionFormula = "(usp_GetIncomeTaxDetailsForReort.School_Id}=" + aiSchoolId + " AND  usp_GetIncomeTaxDetailsForReort.Academic_Year_Id} =" + aiAcademicYearId + " AND usp_GetIncomeTaxDetailsForReort.FinancialYearId}=" + aoDictParameters["FinancialYearId"] +
               " AND  usp_GetIncomeTaxDetailsForReort.StaffGroupsId} = null AND  usp_GetIncomeTaxDetailsForReort.HasFullAccess} = 1 AND usp_GetIncomeTaxDetailsForReort.UserId}=" + aoDictParameters["UserId"] + ")" + "@ ";
            }
            else if (aoExportReports == Constants.ExportReports.AppointmentLetter)
            {
                sRecordSelectionFormula = "(usp_GetAppointmentDetailsForReport.School_Id}=" + aiSchoolId + " AND usp_GetAppointmentDetailsForReport.Academic_Year_Id}=" + aiAcademicYearId + " AND usp_GetAppointmentDetailsForReport.AppointmentId}=" + aoDictParameters["AppointmentId"] + ")" + "@ ";
            }
            else if (aoExportReports == Constants.ExportReports.ServiceContract)
            {
                sRecordSelectionFormula = "(usp_GetServiceContractDetails.School_Id}=" + aiSchoolId + " AND usp_GetServiceContractDetails.AppointmentId}=" + aoDictParameters["AppointmentId"] + ")" + "@ ";
            }
            else if (aoExportReports == Constants.ExportReports.SalarySlipReport)
            {
                sRecordSelectionFormula = "(usp_GetSalarySlipDetails.School_Id}=" + aiSchoolId + " AND  usp_GetSalarySlipDetails.Academic_Year_Id} =" + aiAcademicYearId + " AND usp_GetSalarySlipDetails.FromDate}=" + aoDictParameters["FromDate"] + " AND usp_GetSalarySlipDetails.StaffGroupsId} = null AND usp_GetSalarySlipDetails.UserId} = null AND usp_GetSalarySlipDetails.LoginUserId} = " + aoDictParameters["LoginUserId"] + " AND  usp_GetSalarySlipDetails.ToDate} =" + aoDictParameters["ToDate"] + ")" + "@ ";
            }
            else if (aoExportReports == Constants.ExportReports.MaterialwiseStockDetails)
            {
                sRecordSelectionFormula = "(usp_GetMaterialwiseStockDetails.School_Id}=" + aiSchoolId + "AND usp_GetMaterialwiseStockDetails.Academic_Year_Id}=" + aiAcademicYearId + " AND usp_GetMaterialwiseStockDetails.CategoryId}=" + aoDictParameters["CategoryId"] + " AND usp_GetMaterialwiseStockDetails.ItemIds}=" + aoDictParameters["ItemIds"] + ") @";
            }
            else if (aoExportReports == Constants.ExportReports.AdmissionFormReport)
            {
                if (aiSchoolId == Constants.SchoolId.PPSH.ToInt())
                    sRecordSelectionFormula = "(usp_GetAdmmissionFormReport.SchoolId}=" + aiSchoolId + " AND usp_GetAdmmissionFormReport.StudentAdmissionId}=" + aoDictParameters["StudentAdmissionId"] + " AND usp_GetAdmmissionFormReport.IsTeachersCopy}=" + aoDictParameters["IsTeachersCopy"] + " AND usp_GetAdmmissionFormReport.AdmissionForCurrentYear}=" + aoDictParameters["AdmissionForCurrentYear"] + ") @";
                else
                    sRecordSelectionFormula = "(usp_GetAdmmissionFormReport.SchoolId}=" + aiSchoolId + " AND usp_GetAdmmissionFormReport.StudentAdmissionId}=" + aoDictParameters["StudentAdmissionId"] + " AND usp_GetAdmmissionFormReport.AdmissionForCurrentYear}=" + aoDictParameters["AdmissionForCurrentYear"] + ") @";
            }
            else if (aoExportReports == Constants.ExportReports.ConsolidatedStudentAdmissionList)
            {
                sRecordSelectionFormula = "(usp_GetConsolidatedStudentAdmissionList.School_Id}=" + aiSchoolId + " AND  usp_GetConsolidatedStudentAdmissionList.Academic_Year_Id} =" + aiAcademicYearId +
             " AND usp_GetConsolidatedStudentAdmissionList.StandardId}=" + aoDictParameters["StandardId"] +
              " AND  usp_GetAllStudentOfAdmissionsLottery.SchoolName} =" + aoDictParameters["SchoolName"] +
              " AND  usp_GetAllStudentOfAdmissionsLottery.AcademicYear} =" + aoDictParameters["AcademicYear"] +
              " AND  usp_GetAllStudentOfAdmissionsLottery.OrganisationName} =" + aoDictParameters["OrganisationName"] + ")" + "@";
            }
            else if (aoExportReports == Constants.ExportReports.AdmissionLotteryDetails)
            {
                sRecordSelectionFormula = "(usp_GetAllStudentOfAdmissionsLottery.School_Id}=" + aiSchoolId + " AND  usp_GetAllStudentOfAdmissionsLottery.Academic_Year_Id} =" + aiAcademicYearId +
      " AND usp_GetAllStudentOfAdmissionsLottery.Standard_Id}=" + aoDictParameters["Standard_Id"] + " AND  usp_GetAllStudentOfAdmissionsLottery.cSelectedInLottery} =" + aoDictParameters["cSelectedInLottery"]
       + " AND  usp_GetAllStudentOfAdmissionsLottery.IsConfirmed} =" + aoDictParameters["IsConfirmed"] +
       " AND  usp_GetAllStudentOfAdmissionsLottery.SchoolName} =" + aoDictParameters["SchoolName"] +
       " AND  usp_GetAllStudentOfAdmissionsLottery.AcademicYear} =" + aoDictParameters["AcademicYear"] +
       " AND  usp_GetAllStudentOfAdmissionsLottery.OrganisationName} =" + aoDictParameters["OrganisationName"] +
       " AND  usp_GetAllStudentOfAdmissionsLottery.ListName} =" + aoDictParameters["ListName"] + ")" + "@";
            }
            else if (aoExportReports == Constants.ExportReports.LeavingCerificatePPSN
                || aoExportReports == Constants.ExportReports.LeavingCertificateSS
                || aoExportReports == Constants.ExportReports.LeavingCertificatePP
                || aoExportReports == Constants.ExportReports.LeavingCertificateSNS
                || aoExportReports == Constants.ExportReports.LeavingCertificateSSN
                || aoExportReports == Constants.ExportReports.LeavingCertificateSSNMarathi
                || aoExportReports == Constants.ExportReports.LeavingCertificateSPS
                || aoExportReports == Constants.ExportReports.LeavingCertificateOWS
                || aoExportReports == Constants.ExportReports.LeavingCertificatePPSH
                || aoExportReports == Constants.ExportReports.LeavingCertificateMVPS
                || aoExportReports == Constants.ExportReports.LeavingCertificateDPIS)
            {
                SchoolWiseAcademicYearMasterBL oSchoolAcademicYearBL = new SchoolWiseAcademicYearMasterBL();
                DataTable oDTSchoolInfo = oSchoolAcademicYearBL.GetSchoolInfo(aiSchoolId, aiAcademicYearId);
                string sAcademicYearName = "Year " + oDTSchoolInfo.Rows[Constants.I_ZERO]["Year"].ToString();
                string sOrgName = oDTSchoolInfo.Rows[Constants.I_ZERO]["School_Orgn_Name"].ToString();
                //string sSchoolName = Session[Constants.S_SESSION_SCHOOL_NAME].ToString();
                string sSchoolName = oDTSchoolInfo.Rows[Constants.I_ZERO]["School_Name"].ToString();


                if (aiSchoolId == Constants.SchoolId.LFS.ToInt())
                    sRecordSelectionFormula = "(usp_LeavingCertificateForLFS.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificateForLFS.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificateForLFS.Academic_Year_Id} =" + aiAcademicYearId + " AND  usp_LeavingCertificate.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                else if (aiSchoolId == Constants.SchoolId.SSN.ToInt())
                {
                  
                    sRecordSelectionFormula = "(usp_LeavingCertificate_SSN.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificate_SSN.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificate_SSN.PrintDate} = " + aoDictParameters["PrintDate"] + " AND usp_LeavingCertificate_SSN.DisplayInMarathi } =" + aoDictParameters["DisplayInMarathi"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.SPS.ToInt())
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificate_SPS.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificate_SPS.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificate_SPS.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.OWS.ToInt())
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificate_OWS.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificate_OWS.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificate_OWS.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if ((aiSchoolId == Constants.SchoolId.CSNP.ToInt() || aiSchoolId == Constants.SchoolId.CSNS.ToInt()))
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificate_SSN.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificate_SSN.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificate_SSN.PrintDate} = " + aoDictParameters["PrintDate"] + " AND usp_LeavingCertificate_SSN.DisplayInMarathi } =" + Constants.S_ZERO + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.SVP.ToInt())
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificate_SVP.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificate_SVP.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificate_SVP.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.PPSH.ToInt())
                {
                    sRecordSelectionFormula = "(usp_TransferCertificate_PPSH.School_Id}=)" + aiSchoolId + "AND usp_TransferCertificate_PPSH.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + "AND usp_TransferCertificate_PPSH.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.HSP.ToInt())
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificateHSP.School_Id}=)" + aiSchoolId + "AND usp_LeavingCertificateHSP.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + "AND usp_LeavingCertificateHSP.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.MVPS.ToInt())
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificateMVPS.School_Id}=)" + aiSchoolId + "AND usp_LeavingCertificateMVPS.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + "AND usp_LeavingCertificateMVPS.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.ZLSP.ToInt())
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificateDYP.School_Id}=)" + aiSchoolId + "AND usp_LeavingCertificateDYP.Enrolment_Number =" + aoDictParameters["Enrolment_Number"] + "AND usp_LeavingCertificateDYP;.PrintDate = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (SchoolBase.Settings.IsAaryanSchool)
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificateForAryan.School_Id}=)" + aiSchoolId + "AND usp_LeavingCertificateForAryan.Enrolment_Number =" + aoDictParameters["Enrolment_Number"] + "AND usp_LeavingCertificateForAryan;.PrintDate = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.DYPV.ToInt()) //
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificateForDYPV.School_Id}=)" + aiSchoolId + "AND usp_LeavingCertificateForDYPV.Enrolment_Number =" + aoDictParameters["Enrolment_Number"] + "AND usp_LeavingCertificateForDYPV;.PrintDate = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.NPS.ToInt()) //NPS
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificateNurseryTo9th_NPS.School_Id}=)" + aiSchoolId + "AND usp_LeavingCertificateNurseryTo9th_NPS.Enrolment_Number =" + aoDictParameters["Enrolment_Number"] + "AND usp_LeavingCertificateNurseryTo9th_NPS;.PrintDate = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.VPMCPS.ToInt())
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificate_VPMCPS.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificate_VPMCPS.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificate_VPMCPS.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                }
                else if (aiSchoolId == Constants.SchoolId.PIONEER.ToInt())
                {
                    sRecordSelectionFormula = "(usp_LeavingCertificate_Pioneer.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificate_Pioneer.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificate_Pioneer.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";
                }
                else
                    sRecordSelectionFormula = "(usp_LeavingCertificate.School_Id}=" + aiSchoolId + " AND  usp_LeavingCertificate.Enrolment_Number} =" + aoDictParameters["Enrolment_Number"] + " AND  usp_LeavingCertificate.PrintDate} = " + aoDictParameters["PrintDate"] + ") @";

            }
            return sRecordSelectionFormula;
            
        }

        public string GetLessonPlanFileName(int aiSchoolId, int aiAcademicYearId, int aiUserId, string asStartDate, string asEndDate)
        {
            int iSubjectId = 0, iStdDivId = 0;
            string sRecordSelectionFormula = string.Empty;
            if (asStartDate == null & asEndDate == null)
            {
                sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + aiSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + aiAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + aiUserId + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=" + iSubjectId + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=" + iStdDivId +
                        " AND usp_GetLessonPlanDetailsForReport.StartDate}=null" + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=null" + ")" + "@ ";
            }
            else if (asStartDate != null && asEndDate == null)
            {
                sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + aiSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + aiAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + aiUserId + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=" + iSubjectId + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=" + iStdDivId +
                        " AND usp_GetLessonPlanDetailsForReport.StartDate}=" + asStartDate + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=null" + ")" + "@ ";
            }
            else if (asStartDate == null && asEndDate != null)
            {
                sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + aiSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + aiAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + aiUserId + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=" + iSubjectId + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=" + iStdDivId +
                        " AND usp_GetLessonPlanDetailsForReport.StartDate}=null" + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=" + asEndDate + ")" + "@ ";
            }
            else
            {
                sRecordSelectionFormula = "(usp_GetLessonPlanDetailsForReport.SchoolId}=" + aiSchoolId + " AND  usp_GetLessonPlanDetailsForReport.AcademicYearId} =" + aiAcademicYearId + " AND usp_GetLessonPlanDetailsForReport.UserId}=" + aiUserId + " AND usp_GetLessonPlanDetailsForReport.SubjectId}=" + iSubjectId + " AND usp_GetLessonPlanDetailsForReport.StandardDivisionId}=" + iStdDivId +
                        " AND usp_GetLessonPlanDetailsForReport.StartDate}=" + asStartDate + "  AND  usp_GetLessonPlanDetailsForReport.EndDate}=" + asEndDate + ")" + "@ ";
            }

            string sFileName = @"\RITeSchool\OtherDownloads\LessonPlanDownloads\" + "LessonPlan_" + GetDateFormat() + ".pdf";
            string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.LessonPlan, sRecordSelectionFormula, ExportFormatType.PortableDocFormat, sDownloadPath, false);
            oReportDisplay.IsServiceCall = true;
            oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
            oReportDisplay.SchoolId = aiSchoolId;
            oReportDisplay.AcademicYearId = aiAcademicYearId;
            oReportDisplay.SchoolName = string.Empty;
            oReportDisplay.DisplayReport();

            return "0:" + sFileName;
        }

        public string GetITRFileName(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiValueMember, int aiSelectAcademicYearId, int aiCategoryId)
        {
            string sFilterStr = "(usp_ITReconciliation_Statement_Report.School_Id}=" + aiSchoolId + "AND usp_ITReconciliation_Statement_Report.Academic_Year_Id}=" + aiAcademicYearId + "AND usp_ITReconciliation_Statement_Report.StudentId}=" + aiStudentId + "AND usp_ITReconciliation_Statement_Report.Value_Member}=" + aiValueMember + "AND usp_ITReconciliation_Statement_Report.Select_AcademicYearId}=" + aiSelectAcademicYearId + "AND usp_ITReconciliation_Statement_Report.CategoryId}=" + aiCategoryId + ") @";

            string sFileName = @"\RITeSchool\OtherDownloads\IncomeTaxReconciliation\" + "IncomeTaxReconciliation_" + GetDateFormat() + ".pdf";
            string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.IncomeTaxReconciliation, sFilterStr, ExportFormatType.PortableDocFormat, sDownloadPath, false);
            oReportDisplay.IsServiceCall = true;
            oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
            oReportDisplay.SchoolId = aiSchoolId;
            oReportDisplay.AcademicYearId = aiAcademicYearId;
            oReportDisplay.SchoolName = string.Empty;
            oReportDisplay.DisplayReport();

            if (oReportDisplay.IsReportGennerated)
                return sFileName;
            else
                return string.Empty;
        }

        public string GetDateFormat()
        {
            return DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
        }

        /// <summary>
        /// This method is used to return progress report file name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiStandardDivID"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="abIsPrimaryReport"></param>
        /// <returns></returns>
        //public string GetProgressReportFileName(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStandardDivID, int aiStudentId, bool abIsPrimaryReport)
        //{
        //    ReportDisplay oReportDisplay = null;

        //    string sFilterStr = string.Empty;

        //    string sFileName = @"\RITeSchool\OtherDownloads\ProgressReportDownloads\" + "ProgressReport_" + GetDateFormat() + ".pdf";
        //    string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

        //    if (abIsPrimaryReport)
        //    {
        //        sFilterStr = "(usp_GetTermWiseStudentMarkDetails1_4.School_Id}=" + aiSchoolId + "AND usp_GetTermWiseStudentMarkDetails1_4.Academic_Year_Id}=" + aiAcademicYearId + "AND usp_GetTermWiseStudentMarkDetails1_4.Standard_Id}=" + aiStandardId + "AND usp_GetTermWiseStudentMarkDetails1_4.Division_Id}=" + aiStandardDivID + "AND usp_GetTermWiseStudentMarkDetails1_4.StudentId}=" + aiStudentId + ") @";
        //        oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReport_Aaryan, sFilterStr, ExportFormatType.PortableDocFormat, sDownloadPath, false);
        //    }
        //    else
        //    {
        //        sFilterStr = "(usp_GetTermWiseStudentMarkDetails5_8.School_Id}=" + aiSchoolId + "AND usp_GetTermWiseStudentMarkDetails5_8.Academic_Year_Id}=" + aiAcademicYearId + "AND usp_GetTermWiseStudentMarkDetails5_8.Standard_Id}=" + aiStandardId + "AND usp_GetTermWiseStudentMarkDetails5_8.Division_Id}=" + aiStandardDivID + "AND usp_GetTermWiseStudentMarkDetails5_8.StudentId}=" + aiStudentId + ") @";
        //        oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportAaryan_5to8, sFilterStr, ExportFormatType.PortableDocFormat, sDownloadPath, false);
        //    }

        //    oReportDisplay.IsServiceCall = true;
        //    oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
        //    oReportDisplay.SchoolId = aiSchoolId;
        //    oReportDisplay.AcademicYearId = aiAcademicYearId;
        //    oReportDisplay.SchoolName = string.Empty;
        //    oReportDisplay.DisplayReport();
        //    return sFileName;
        //}

        public string GetTermAndFinalProgressReportFileName(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStandardDivID, int aiStudentId, int aiTermId)
        {
            ReportDisplay oReportDisplay = null;

            string sFilterStr = string.Empty;

            string sFileName = @"\RITeSchool\OtherDownloads\ProgressReportDownloads\" + "MobileProgressReport_" + GetDateFormat() + ".pdf";
            string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

            DataTable dt = StudentBL.GetYearwiseStudentDetailsForService(aiSchoolId, aiAcademicYearId, aiStudentId);

            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miStudentId = dt.Rows[0]["YearWise_Student_Id"].ToInt();
            this.miStdDivId = dt.Rows[0]["SchoolWise_Standard_Division_Id"].ToInt();
            this.miStandardId = dt.Rows[0]["Standard_Id"].ToInt();
            this.msStandardName = dt.Rows[0]["Standard_Name"].ToString();
            //this.msAssessmentIds = dt.Rows[0]["AssessmentIds"].ToString();

            string sAssessmentIds = dt.Rows[0]["AssessmentIds"].ToString();

            this.mbIsLateJoinee = dt.Rows[0]["IsLateJoinee"].ToBool();

            bool bIsFinalExamPublished = false;

            if (msStandardName == "Nursery" || msStandardName == "Junior KG" || msStandardName == "Senior KG")
            {
                string[] arrAssessmentId = sAssessmentIds.Split(',');
                string sAssessmentId = string.Empty;
                if (arrAssessmentId.Length > 1)
                {
                    if (aiTermId == 1)
                        miAssessmentId = arrAssessmentId[0].ToInt();
                    else if (aiTermId == 2)
                        miAssessmentId = arrAssessmentId[1].ToInt();

                    XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
                    bool bIsResultPublished = oXseedProgressReportBL.IsXseedResultPublished(miSchoolId, miAcademicYearId, miStdDivId, miAssessmentId, miStudentId);
                    if (!bIsResultPublished)
                        return "1:Xseed exam is not yet published.";
                }
            }
            else
            {
                if (aiTermId == 1)
                {
                    string asStandardName;
                    SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
                    bool bistermexampublished = oSWStdDivTestMasterBL.IsTermExamPublished(miSchoolId, miAcademicYearId, miStdDivId, out asStandardName);
                    if (!bistermexampublished)
                        return "1:Term exam is not yet published.";
                }
                else if (aiTermId == 2)
                {
                    SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
                    bIsFinalExamPublished = oSWStdDivTestMasterBL.IsFinalResultPublished(miSchoolId, miAcademicYearId, miStdDivId);
                    if (!bIsFinalExamPublished)
                        return "1:Final exam is not yet published.";
                }
                else if (aiTermId == 3)
                {
                    SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
                    bIsFinalExamPublished = oSWStdDivTestMasterBL.IsPrelimExamPublished(miStdDivId, miSchoolId, miAcademicYearId);
                    if (!bIsFinalExamPublished)
                        return "1:Prelim exam is not yet published.";
                }
            }

            StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
            oStudentFeeDetailsBL.School_Id = miSchoolId;
            oStudentFeeDetailsBL.Academic_Year_Id = miAcademicYearId;
            oStudentFeeDetailsBL.Student_Id = miStudentId;

            SchoolBL oSchoolBL = new SchoolBL();
            List<SchoolSettings> lstSettings = oSchoolBL.GetSchoolSettings(aiSchoolId, aiAcademicYearId);
            string sBlockReport = lstSettings.Where(st => st.Name == "BlockProgressReportIfFeesArePending").Select(st => st.Value).FirstOrDefault();

            bool bIsFeePending = false;
            if (miSchoolId != Constants.SchoolId.PPSH.ToInt())
                bIsFeePending = sBlockReport.ToLower() == "true" && oStudentFeeDetailsBL.PendingFeesAvailableForStudent();
            else
                bIsFeePending = oStudentFeeDetailsBL.PendingFeesAvailableForStudent();

            if (bIsFeePending)
                return "1:Progress report is blocked due to pending fee.";

            ProgressReportBL oProgressReportBL = new ProgressReportBL(miSchoolId, miAcademicYearId, 0);
            string sBlockProgressReportReason = oProgressReportBL.GetBlockProgressReportReason(miStudentId);

            if (sBlockProgressReportReason.Trim() != string.Empty)
                return "1:Progress report is blocked.";

            oReportDisplay = GetReport(sDownloadPath, aiTermId, bIsFinalExamPublished);
            
            oReportDisplay.IsServiceCall = true;
            oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
            oReportDisplay.SchoolId = aiSchoolId;
            oReportDisplay.AcademicYearId = aiAcademicYearId;
            oReportDisplay.SchoolName = string.Empty;
            oReportDisplay.TermId = aiTermId;
            oReportDisplay.DisplayReport();
            return "0:"+sFileName;
        }

        public string GetFileNameForSNSChallan(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiStandardDivisionId, int aiSchoolwiseStudentId, int aiFeeTypeId, string asPayableFor, int aiSelectedAcademicYearId)
        {
            string sFilterStr = "(usp_GetBankChallanDetails.School_Id}=" + aiSchoolId + "AND usp_GetBankChallanDetails.Academic_Year_Id}=" + aiAcademicYearId + "AND usp_GetBankChallanDetails.Standard_Id}=" + aiStandardId + "AND usp_GetBankChallanDetails.SchoolWise_Standard_Division_Id}=" + aiStandardDivisionId + "AND usp_GetBankChallanDetails.Student_Id}=" + aiSchoolwiseStudentId + "AND usp_GetBankChallanDetails.Original_Fee_Type_Id}=" + aiFeeTypeId + " AND usp_GetBankChallanDetails.Payable_For}=" + asPayableFor + "AND usp_GetBankChallanDetails.AcademicYearId}=" + aiSelectedAcademicYearId + " AND usp_GetBankChallanDetails.Organisation Name=} AND usp_GetBankChallanDetails.SchoolName=} AND usp_GetBankChallanDetails.AcademicYear=} AND usp_GetBankChallanDetails.IncludeLateFee=1}) @";

            string sFileName = @"\RITeSchool\OtherDownloads\ChallanDownloads\" + "BankChallan_" + GetDateFormat() + ".pdf";
            string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.ClasswiseBankChallan, sFilterStr, ExportFormatType.PortableDocFormat, sDownloadPath, false);
            oReportDisplay.IsServiceCall = true;
            oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
            oReportDisplay.SchoolId = aiSchoolId;
            oReportDisplay.AcademicYearId = aiAcademicYearId;
            oReportDisplay.SchoolName = string.Empty;
            oReportDisplay.DisplayReport();

            return sFileName;
        }

        public string GetCautionMoneyReceiptFileName(int aiSchoolId,int aiAcademicYearId, int aiSchoolwiseStudentId)
        {
            string sFilterStr = "(usp_GetCautionMoneyReciept.SchoolId}=" + aiSchoolId + "AND usp_GetPODetailsForReport.StudentId}=" + aiSchoolwiseStudentId + "AND usp_GetPODetailsForReport.IsReturnMode}=" + 0 + ")@";

            string sFileName = @"\RITeSchool\OtherDownloads\ReceiptDownloads\" + "CautionMoneyReceipt_" + GetDateFormat() + ".pdf";
            string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.CautionMoneyReceipt, sFilterStr, ExportFormatType.PortableDocFormat, sDownloadPath, false);
            oReportDisplay.IsServiceCall = true;
            oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
            oReportDisplay.SchoolId = aiSchoolId;
            oReportDisplay.AcademicYearId = aiAcademicYearId;
            oReportDisplay.SchoolName = string.Empty;
            oReportDisplay.DisplayReport();

            return sFileName;
        }

        public string GetInternalFeeReceiptFileName(int aiSchoolId, int aiAcademicYearId, int aiSchoolwiseStudentId, string asReceiptNo, int aiInternalFeeDetailsId, bool abIsNextYearPayment, int aiSerialNumber)
        {
            string sFilterString = "(usp_GetInternalFeeReceiptDetails.SchoolId}=" + aiSchoolId + " AND usp_GetInternalFeeReceiptDetails.AcademicYearId}=" + aiAcademicYearId + " AND usp_GetInternalFeeReceiptDetails.CurrAcadYerId}=" + aiAcademicYearId +
                            " AND usp_GetInternalFeeReceiptDetails.ReceiptNo}=" + asReceiptNo + " AND usp_GetInternalFeeReceiptDetails.InternalFeeDetailsId}=" + aiInternalFeeDetailsId + " AND usp_GetInternalFeeReceiptDetails.Schoolwise_Student_Id}=" + aiSchoolwiseStudentId +
                            " AND usp_GetInternalFeeReceiptDetails.DuplicateInternalFeeDetailsId}=" + 0 + " AND usp_GetInternalFeeReceiptDetails.IsNextYearPayment}=" + abIsNextYearPayment + " AND usp_GetInternalFeeReceiptDetails.SerialNumber}=" + aiSerialNumber + ")@";

            string sFileName = @"\RITeSchool\OtherDownloads\ReceiptDownloads\" + "InternalFeeReceipt_" + GetDateFormat() + ".pdf";
            string sDownloadPath = HostingEnvironment.ApplicationPhysicalPath + sFileName;

            ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.InternalFeeReceipt, sFilterString, ExportFormatType.PortableDocFormat, sDownloadPath, false);
            oReportDisplay.IsServiceCall = true;
            oReportDisplay.BasePath = HostingEnvironment.ApplicationPhysicalPath;
            oReportDisplay.SchoolId = aiSchoolId;
            oReportDisplay.AcademicYearId = aiAcademicYearId;
            oReportDisplay.SchoolName = string.Empty;
            oReportDisplay.DisplayReport();

            return sFileName;
        }

        #endregion

        #region -- PRIVATE METHOD(s) --

        private ReportDisplay GetReport(string sDownloadPath, int aiTermId, bool abIsFinalExamPublished)
        {
            ReportDisplay oReportDisplay = null;
            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
            {              
                bool IsFinalExamPublished = true;

                if (aiTermId == 1)
                    IsFinalExamPublished = false;
                else
                    IsFinalExamPublished = abIsFinalExamPublished;

                if (msStandardName == "1" || msStandardName == "2" || msStandardName == "3" || msStandardName == "4" || msStandardName == "5")
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH_1stTO5th, GetFinalProgressReportFilterStringForPPSH(aiTermId, abIsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                else if (msStandardName == "6" || msStandardName == "7" || msStandardName == "8")
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH, GetFinalProgressReportFilterStringForPPSH(aiTermId, abIsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                else if (msStandardName == "9" && IsFinalExamPublished && this.miAcademicYearId >= 11)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH_9th, GetFinalProgressReportFilterStringForPPSH(aiTermId, abIsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                else if (msStandardName == "9" && IsFinalExamPublished)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentFinalProgressReport9thStd_PPSH_AY10, GetFinalProgressReportFilterStringForPPSH(aiTermId, abIsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                else if (msStandardName == "9" && !IsFinalExamPublished)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH_9th, GetFinalProgressReportFilterStringForPPSH(aiTermId, abIsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                else if (msStandardName == "Nursery" || msStandardName == "Junior KG" || msStandardName == "Senior KG")
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPSH_Xseed, GetXseedFilterString(aiTermId), ExportFormatType.PortableDocFormat, sDownloadPath, false);
            }
            else if (miSchoolId == Constants.SchoolId.PKIS.ToInt())
                oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPKIS, GetFinalProgressReportFilterString(false, false), ExportFormatType.PortableDocFormat, sDownloadPath, false);
            else if (miSchoolId == Constants.SchoolId.SVP.ToInt())
                oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportSVP, GetFinalProgressReportFilterString(false, false), ExportFormatType.PortableDocFormat, sDownloadPath, false);
            else if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                if (msStandardName == "Nursery" || msStandardName == "Junior KG" || msStandardName == "Senior KG")
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.XseedProgressReport_PPS, GetXseedFilterString(aiTermId), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                else
                {
                    if (aiTermId == 1)
                        oReportDisplay = DisplayPPSReport(false, sDownloadPath);
                    else if (aiTermId == 3)
                    {
                        if (miAcademicYearId <= 52)
                        {
                            string sFilterStr = "(usp_GetPreliminaryExaminationProgressReport.School_Id}=" + miSchoolId + "AND usp_GetPreliminaryExaminationProgressReport.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetPreliminaryExaminationProgressReport.Standard_Id}=" + miStandardId + " AND usp_GetPreliminaryExaminationProgressReport.Division_Id}=" + miStdDivId + "AND usp_GetPreliminaryExaminationProgressReport.StudentId}=" + miStudentId + "AND usp_GetPreliminaryExaminationProgressReport.IsFromReportScreen}=0 AND usp_GetPreliminaryExaminationProgressReport.Note}=" + string.Empty + ") @";
                            oReportDisplay = new ReportDisplay(Constants.ExportReports.PrelimReport, sFilterStr, ExportFormatType.PortableDocFormat, sDownloadPath, false);                            
                        }
                        else
                        {
                            string sFilterStr = "(usp_GetPrelimProgressReportForPP.School_Id}=" + miSchoolId + "AND usp_GetPrelimProgressReportForPP.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetPrelimProgressReportForPP.Standard_Id}=" + miStandardId + " AND usp_GetPrelimProgressReportForPP.Division_Id}=" + miStdDivId + "AND usp_GetPrelimProgressReportForPP.StudentId}=" + miStudentId + "AND usp_GetPrelimProgressReportForPP.IsFromReportScreen}=0 AND usp_GetPrelimProgressReportForPP.Note}=" + string.Empty + ") @";
                            oReportDisplay = new ReportDisplay(Constants.ExportReports.PrelimReportPP, sFilterStr, ExportFormatType.PortableDocFormat, sDownloadPath, false);                            
                        }
                        oReportDisplay.TermId = 2;
                    }
                    else
                    {
                        if (this.mbIsLateJoinee)
                            oReportDisplay = DisplayPPSReport(false, sDownloadPath);
                        else
                            oReportDisplay = DisplayPPSReport(true, sDownloadPath);
                    }
                }
            }
            else if (miSchoolId == Constants.SchoolId.NPS.ToInt())
                oReportDisplay = new ReportDisplay(Constants.ExportReports.FinalProgressReportNPS, GetFinalProgressReportFilterString(false, false), ExportFormatType.PortableDocFormat, sDownloadPath, false);
            else if (miSchoolId == Constants.SchoolId.PIONEER.ToInt())
            {
                if (mlstPioneerGradeReportStandards.Contains(msStandardName))
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPioneer_NurseryTO2nd, GetFinalProgressReportFilterString(false, false), ExportFormatType.PortableDocFormat, sDownloadPath, false);                 
                else
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.HalfYearlyReportFor3To9Pioneer, GetFinalProgressReportFilterString(false, false), ExportFormatType.PortableDocFormat, sDownloadPath, false);
            }
            return oReportDisplay;
        }

        private string GetXseedFilterString(int aiTermId)
        {
            string sFilterString = string.Empty;

            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                sFilterString = "(usp_GetXseedProgressReport_PPSH.School_Id}=" + miSchoolId + "AND usp_GetXseedProgressReport_PPSH.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetXseedProgressReport_PPSH.YearwiseStudentId}=" + miStudentId + "AND usp_GetXseedProgressReport_PPSH.Standard_Id}=" + miStandardId + "AND usp_GetXseedProgressReport_PPSH.AssessmentId}=" + miAssessmentId + "AND usp_GetXseedProgressReport_PPSH.SchoolWise_Standard_Division_Id}=" + miStdDivId + ") @";
            else if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                sFilterString = "(Xseed.usp_GetXseedProgressReport.School_Id}=" + miSchoolId + "AND Xseed.usp_GetXseedProgressReport.Academic_Year_Id}=" + miAcademicYearId + "AND Xseed.usp_GetXseedProgressReport.YearwiseStudentId}=" + miStudentId + "AND Xseed.usp_GetXseedProgressReport.Standard_Id}=" + miStandardId + "AND Xseed.usp_GetXseedProgressReport.SchoolWise_Standard_Division_Id}=" + miStdDivId + "AND Xseed.usp_GetXseedProgressReport.AssessmentId}=" + miAssessmentId + "AND Xseed.usp_GetXseedProgressReport.IsFromReportScreen}=0" + ") @";
           
            return sFilterString;
        }

        private ReportDisplay DisplayPPSReport(bool IsFinalExamPublished, string sDownloadPath)
        {
            ReportDisplay oReportDisplay;
          
            bool bIsGradingstandard = StandardMasterBL.IsGradingStandard(miSchoolId, miAcademicYearId, miStandardId);

            if (!bIsGradingstandard)
            {
                if (this.miAcademicYearId <= 52)
                    oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPS, GetFinalProgressReportFilterString(false, IsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                else
                {
                    if (IsFinalExamPublished)
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.FinalReportPP, GetFinalProgressReportFilterString(false, IsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                    else
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.PPSTermwiseReport, GetFinalProgressReportFilterString(false, IsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);
                }
            }
            else
                oReportDisplay = new ReportDisplay(Constants.ExportReports.StudentwiseProgressReportPPS_Grading, GetFinalProgressReportFilterString(true, IsFinalExamPublished), ExportFormatType.PortableDocFormat, sDownloadPath, false);

            if (IsFinalExamPublished)
            {
                oReportDisplay.TermId = Constants.I_TWO;

                if (bIsGradingstandard)
                {
                    if (this.miAcademicYearId >= I_PPS_2025_26)
                    {
                        if (msStandardName == "5")
                        {
                            oReportDisplay.FileName = "StudentFinalProgressReportGradingFor5th_2026.rpt";
                        }
                        else if (msStandardName == "1" || msStandardName == "2" || msStandardName == "3" || msStandardName == "4")
                        {
                            oReportDisplay.FileName = "StudentFinalProgressReportGrading2026.rpt";
                        }
                    }
                  else  if (this.miAcademicYearId >= I_PPS_2022_23)
                    {
                        if (this.miAcademicYearId >= I_PPS_2023_24 && this.msStandardName == "5")
                            oReportDisplay.FileName = "StudentFinalProgressReportGradingFor5th_2024.rpt";
                        else
                            oReportDisplay.FileName = "StudentFinalProgressReportGrading2023.rpt";
                    }
                    else
                        oReportDisplay.FileName = "StudentFinalProgressReportGrading51.rpt";
                }
                else
                {
                    if (this.miAcademicYearId >= I_PPS_2025_26)
                    {
                        if (msStandardName == "6" || msStandardName == "7" || msStandardName == "8" || msStandardName == "9" || msStandardName == "10")
                        {
                            oReportDisplay.FileName = "FinalProgressReportPP2026.rpt";
                        }
                    }
                    else if (this.miAcademicYearId >= I_PPS_2022_23)
                        oReportDisplay.FileName = "FinalProgressReportPP.rpt";
                    else
                    {
                        if (this.miAcademicYearId == 51 || this.miAcademicYearId == 52)
                        {
                            if (msStandardName == "6" || msStandardName == "7" || msStandardName == "8")
                                oReportDisplay.FileName = "StudentFinalProgressReportMarking51_6to8.rpt";
                            else
                                oReportDisplay.FileName = "StudentFinalProgressReportMarking51.rpt";
                        }
                        else if (this.miAcademicYearId >= I_PPS_2022_23)
                        {
                            if (!bIsGradingstandard)
                                oReportDisplay.FileName = "TermwiseProgressReportPP.rpt";
                            else
                            {
                                oReportDisplay.FileName = "StudentwiseProgressReportPP53.rpt";

                                if (this.mbIsLateJoinee)
                                {
                                    oReportDisplay.TermId = 2;
                                    oReportDisplay.AllowSecondTermFromTermReport = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                oReportDisplay.TermId = Constants.I_ONE;

                if (this.miAcademicYearId == 51 || this.miAcademicYearId == 52)
                {
                    if (!bIsGradingstandard)
                        oReportDisplay.FileName = "StudentTerm1ProgressReport51.rpt";
                    else
                        oReportDisplay.FileName = "StudentwiseProgressReport51.rpt";
                }
                else if (this.miAcademicYearId >= I_PPS_2022_23)
                {
                    if (!bIsGradingstandard)
                        oReportDisplay.FileName = "TermwiseProgressReportPP.rpt";
                    else
                    {
                        oReportDisplay.FileName = "StudentwiseProgressReportPP53.rpt";

                        if (this.mbIsLateJoinee)
                        {
                            oReportDisplay.TermId = 2;
                            oReportDisplay.AllowSecondTermFromTermReport = true;
                        }
                    }
                }
            }


            return oReportDisplay;
        }

        private string GetFinalProgressReportFilterStringForPPSH(int aiTermId, bool abIsFinalExamPublished)
        {
            string sFilterStr = string.Empty;

            if (aiTermId == 2 && abIsFinalExamPublished == false)
                aiTermId = 0;

            if (msStandardName == "6" || msStandardName == "7" || msStandardName == "8")
                sFilterStr = "(usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.StudentId}=" + miStudentId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Standard_Id}=" + miStandardId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Term_Id}=1AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.IsFromStudnetLogin}=1AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Division_Id}=" + miStdDivId + ") @";
            else if (msStandardName == "9")
                sFilterStr = "(usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.StudentId}=" + miStudentId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.IsFromStudentLogin}=1AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Standard_Id}=" + miStandardId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Division_Id}=" + miStdDivId + ") @";
            else if (msStandardName == "1" || msStandardName == "2" || msStandardName == "3" || msStandardName == "4" || msStandardName == "5")
                sFilterStr = "(usp_GetStudentObservationDetailsForReport_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentObservationDetailsForReport_PPSH.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetStudentObservationDetailsForReport_PPSH.StudentId}=" + miStudentId + "AND usp_GetStudentObservationDetailsForReport_PPSH.Standard_Id}=" + miStandardId + "AND usp_GetStudentObservationDetailsForReport_PPSH.Term_Id}=" + aiTermId + "AND usp_GetStudentObservationDetailsForReport_PPSH.IsFromStudentLogin}=1AND usp_GetStudentObservationDetailsForReport_PPSH.Division_Id}=" + miStdDivId + ") @";
                        
            return sFilterStr;
        }


        private string GetFinalProgressReportFilterString(bool bIsGradeingReport, bool bIsFinalExamPublished)
        {
            string sFilterStr = string.Empty;
          
            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
            {
                SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
                bool bIsFinalPublished = oSWStdDivTestMasterBL.IsFinalResultPublished(miSchoolId, miAcademicYearId, miStdDivId);
                int iTermId = bIsFinalPublished ? 2 : 1;

                if (msStandardName == "6" || msStandardName == "7" || msStandardName == "8")
                    sFilterStr = "(usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.StudentId}=" + miStudentId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Standard_Id}=" + miStandardId + "AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Term_Id}=1AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.IsFromStudnetLogin}=1AND usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH.Division_Id}=" + miStdDivId + ") @";
                else if (msStandardName == "9")
                    sFilterStr = "(usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.StudentId}=" + miStudentId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.IsFromStudentLogin}=1AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Standard_Id}=" + miStandardId + "AND usp_GetStudentwiseProgressReportDetailsFor9th_PPSH.Division_Id}=" + miStdDivId + ") @";
                else if (msStandardName == "1" || msStandardName == "2" || msStandardName == "3" || msStandardName == "4" || msStandardName == "5")
                    sFilterStr = "(usp_GetStudentObservationDetailsForReport_PPSH.School_Id}=" + miSchoolId + "AND usp_GetStudentObservationDetailsForReport_PPSH.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetStudentObservationDetailsForReport_PPSH.StudentId}=" + miStudentId + "AND usp_GetStudentObservationDetailsForReport_PPSH.Standard_Id}=" + miStandardId + "AND usp_GetStudentObservationDetailsForReport_PPSH.Term_Id}=" + iTermId + "AND usp_GetStudentObservationDetailsForReport_PPSH.IsFromStudentLogin}=1AND usp_GetStudentObservationDetailsForReport_PPSH.Division_Id}=" + miStdDivId + ") @";
            }           
            else if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {            
                if (!bIsGradeingReport)
                {
                    if (bIsFinalExamPublished)
                    {
                        if (this.miAcademicYearId >= I_PPS_2022_23)
                            sFilterStr = "(usp_GetFinalProgressReportDetailsForPP.School_Id}=" + miSchoolId + "AND usp_GetFinalProgressReportDetailsForPP.Academic_Year_Id}=" + this.miAcademicYearId + "AND usp_GetFinalProgressReportDetailsForPP.Standard_Id}=" + miStandardId + " AND usp_GetFinalProgressReportDetailsForPP.Division_Id}=" + miStdDivId + "AND usp_GetFinalProgressReportDetailsForPP.StudentId}=" + miStudentId + "AND usp_GetFinalProgressReportDetailsForPP.Note}=" + string.Empty + ") @";
                        else
                            sFilterStr = "(usp_GetFinalProgressReport.School_Id}=" + miSchoolId + "AND usp_GetFinalProgressReport.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetFinalProgressReport.Standard_Id}=" + miStandardId + " AND usp_GetFinalProgressReport.Division_Id}=" + miStdDivId + "AND usp_GetFinalProgressReport.StudentId}=" + miStudentId + "AND usp_GetFinalProgressReport.Note}=" + string.Empty + ") @";
                    }
                    else
                    {
                        if (this.miAcademicYearId <= 52)
                            sFilterStr = "(usp_GetMarkingSystemProgressReport.School_Id}=" + miSchoolId + "AND usp_GetMarkingSystemProgressReport.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetMarkingSystemProgressReport.Standard_Id}=" + miStandardId + " AND usp_GetMarkingSystemProgressReport.Division_Id}=" + miStdDivId + "AND usp_GetMarkingSystemProgressReport.StudentId}=" + miStudentId + "AND usp_GetMarkingSystemProgressReport.Term_Id}=" + Constants.I_ONE + "AND usp_GetMarkingSystemProgressReport.Note}=" + string.Empty + "AND usp_GetMarkingSystemProgressReport.IsFromReportScreen}=0" + ") @";
                        else
                            sFilterStr = "(usp_GetTerm1ProgressReportDetailsForPP.School_Id}=" + miSchoolId + "AND usp_GetTerm1ProgressReportDetailsForPP.Academic_Year_Id}=" + this.miAcademicYearId + "AND usp_GetTerm1ProgressReportDetailsForPP.Standard_Id}=" + miStandardId + " AND usp_GetTerm1ProgressReportDetailsForPP.Division_Id}=" + miStdDivId + "AND usp_GetTerm1ProgressReportDetailsForPP.StudentId}=" + miStudentId + "AND usp_GetTerm1ProgressReportDetailsForPP.Term_Id}=" + Constants.I_ONE + "AND usp_GetTerm1ProgressReportDetailsForPP.Note}=" + string.Empty + "AND usp_GetTerm1ProgressReportDetailsForPP.IsFromReportScreen}=0" + ") @";
                    }
                }
                else
                {
                    if (bIsFinalExamPublished)
                        sFilterStr = "(usp_GetFinalProgressReport.School_Id}=" + miSchoolId + "AND usp_GetFinalProgressReport.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetFinalProgressReport.Standard_Id}=" + miStandardId + " AND usp_GetFinalProgressReport.Division_Id}=" + miStdDivId + "AND usp_GetFinalProgressReport.StudentId}=" + miStudentId + "AND usp_GetFinalProgressReport.Note}=" + string.Empty + ") @";
                    else
                        sFilterStr = "(usp_GetGradingSystemProgressReport.School_Id}=" + miSchoolId + "AND usp_GetGradingSystemProgressReport.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetGradingSystemProgressReport.Standard_Id}=" + miStandardId + " AND usp_GetGradingSystemProgressReport.Division_Id}=" + miStdDivId + "AND usp_GetGradingSystemProgressReport.StudentId}=" + miStudentId + "AND usp_GetGradingSystemProgressReport.Term_Id}=" + Constants.I_ONE + "AND usp_GetGradingSystemProgressReport.Note}=" + string.Empty + "AND usp_GetGradingSystemProgressReport.IsFromReportScreen}=0" + ") @";
                }

            }
            else if (miSchoolId == Constants.SchoolId.NPS.ToInt())
                sFilterStr = "(usp_StudentwiseProgressReportForNPS.School_Id}=" + miSchoolId + "AND usp_StudentwiseProgressReportForNPS.Academic_Year_Id}=" + miAcademicYearId + "AND usp_StudentwiseProgressReportForNPS.StudentId}=" + miStudentId + "AND usp_StudentwiseProgressReportForNPS.Standard_Id}=" + miStandardId + " AND usp_StudentwiseProgressReportForNPS.Division_Id}=" + miStdDivId + "AND usp_StudentwiseProgressReportForNPS.Term_Id}=" + Constants.I_TWO + "AND usp_StudentwiseProgressReportForNPS.Note}=" + string.Empty + ") @";
            else if (miSchoolId == Constants.SchoolId.PIONEER.ToInt())
            {                
                DataTable oDatatable1 = StudentBL.GetYearwiseStudentDetails(miSchoolId, miAcademicYearId, miStudentId);

                if (oDatatable1 != null && oDatatable1.Rows.Count > 0)
                {
                    int iStdId = Convert.ToInt32(oDatatable1.Rows[0]["Standard_Id"]);

                    if (!mlstPioneerGradeReportStandards.Contains(msStandardName))
                        sFilterStr = "(usp_GetDetailsForHalfYearlyReport_Pioneer.School_Id}=" + miSchoolId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.StudentId}=" + miStudentId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.Standard_Id}=" + miStandardId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.Division_Id}=" + miStdDivId + "AND usp_GetDetailsForHalfYearlyReport_Pioneer.Term_Id}=" + Constants.I_ONE + " AND usp_GetDetailsForHalfYearlyReport_Pioneer.IsFromReportScreen}=0" + ") @";
                    else
                        sFilterStr = "(usp_GetProgressReportDetailsForPrePrimaryPioneer.School_Id}=" + miSchoolId + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.Academic_Year_Id}=" + miAcademicYearId + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.Standard_Id}=" + miStandardId + " AND usp_GetProgressReportDetailsForPrePrimaryPioneer.Division_Id}=" + miStdDivId + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.StudentId}=" + miStudentId + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.TestId}=" + 0 + "AND usp_GetProgressReportDetailsForPrePrimaryPioneer.IsFromReportScreen}=0" + ") @";
                }
            }
            return sFilterStr;
        }

       

        /// <summary>
        /// This method is used to display report
        /// </summary>
        /// <param name="msFilter"></param>
        private string DisplayReport(int aiSchoolId, string asReceiptNo, int aiAcademicYearId, int aiAccountHeaderId, string asIsRefundFee, int aiStudentId, string asSerialNo, int aiAdmissionId)
        {
            string sFilterString = GetFilterString(aiSchoolId, asReceiptNo, aiAcademicYearId, aiAccountHeaderId, asIsRefundFee);

            crReportDocument = new ReportDocument();
            var crConnectionInfo = new ConnectionInfo();
            var crtableLogoninfos = new TableLogOnInfos();
            var crtableLogoninfo = new TableLogOnInfo();

            crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ReportingDataSource"];
            crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["ReportDataBaseName"];
            crConnectionInfo.UserID = ConfigurationManager.AppSettings["ReportingUserId"];
            crConnectionInfo.Password = ConfigurationManager.AppSettings["ReportingPassword"];

            string sReportName = "FeeReciept.rpt";

            if (aiSchoolId == Constants.SchoolId.SS.ToInt())
                sReportName = "FeeRecieptSS.rpt";
            else if (aiSchoolId == Constants.SchoolId.PPS.ToInt())
                sReportName = "FeeRecieptForPP.rpt";
            else if (aiSchoolId == Constants.SchoolId.SNS.ToInt() && asIsRefundFee == Constants.S_ZERO)
                sReportName = "FeeRecieptForSNS.rpt";
            else if (aiSchoolId == Constants.SchoolId.SNS.ToInt() && asIsRefundFee == Constants.S_ONE)
                sReportName = "RefundFeeRecieptForSNS.rpt";
            else if (aiSchoolId == Constants.SchoolId.BMFS.ToInt())
            {
                bool bIsPreprimaryStudent = false;

                StudentFeeDetailsBL oStudentFeeDetailsBL = new StudentFeeDetailsBL();
                DataTable dt = oStudentFeeDetailsBL.CheckStudentsStandardDetails(asReceiptNo.ToInt(), aiSchoolId, aiAcademicYearId);
                if (dt.Rows.Count > Constants.I_ZERO)
                {
                    if (dt.Rows[0]["Is_PrePrimary"].ToString() == "Y")
                        bIsPreprimaryStudent = true;
                }

                if (bIsPreprimaryStudent)
                    sReportName = "FeeRecieptBMFS_PrePrimary.rpt";
                else
                    sReportName = "FeeRecieptBMFS.rpt";
            }

            string sBasePath = HostingEnvironment.ApplicationPhysicalPath;

            string sReportPath = sBasePath + @"\RITeSchool\Report\Fee\" + sReportName;
            string sFileName = @"\RITeSchool\OtherDownloads\ReceiptDownloads\" + "Receipt_" + GetDateFormat() + ".pdf";
            string sDownloadPath = sBasePath + sFileName;

            crReportDocument.Load(sReportPath);
            SetFeeRecieptDatascource(sFilterString, aiSchoolId, aiStudentId, asSerialNo, aiAdmissionId);

            crReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, sDownloadPath);

            crtableLogoninfo = null;
            crtableLogoninfos = null;
            crConnectionInfo = null;

            return sFileName;
        }

        /// <summary>
        /// This methd is used to get filter string.
        /// </summary>
        /// <returns></returns>
        private string GetFilterString(int aiSchoolId, string asReceiptNo, int aiAcademicYearId, int aiAccountHeaderId, string aiIsRefundFee)
        {
            if (aiSchoolId != Constants.SchoolId.SNS.ToInt())
            {
                string sReceiptNumber = Constants.S_EXPORT_PENDING_FEE + ".ReceiptNumber}";
                string sAcademicYearID = Constants.S_EXPORT_PENDING_FEE + ".AcademicYearID}";
                return String.Format("({0}={1} AND {2}={3}",
                                      sReceiptNumber,
                                      asReceiptNo,
                                      sAcademicYearID,
                                      aiAcademicYearId);
            }
            else
            {
                string sReceiptNumber = Constants.S_EXPORT_PENDING_FEE + ".ReceiptNumber}";
                string sAcademicYearID = Constants.S_EXPORT_PENDING_FEE + ".AcademicYearID}";
                string sAccountHeaderId = Constants.S_EXPORT_PENDING_FEE + ".AccountHeaderId}";
                string sIsRefundReport = Constants.S_EXPORT_PENDING_FEE + ".IsRefundReport}";
                return String.Format("({0}={1} AND {2}={3} AND {4}={5} AND {6}={7}",
                                      sReceiptNumber,
                                      asReceiptNo,
                                      sAcademicYearID,
                                      aiAcademicYearId,
                                      sAccountHeaderId,
                                      aiAccountHeaderId,
                                      sIsRefundReport,
                                      aiIsRefundFee);
            }
        }

        /// <summary>
        /// This method is used to set data source to report.
        /// </summary>
        /// <param name="asReportSelectionString"></param>
        private void SetFeeRecieptDatascource(string asReportSelectionString, int aiSchoolId, int aiStudentId, string aiSerialNo, int aiAdmissionId)
        {
            var dsFeeRecieptReportDetails = new DataSet();
            asReportSelectionString = FormatFilterString(asReportSelectionString);
            String[] sFilters = asReportSelectionString.Split('@');
            string sParameterValue;
            string sParameterField;
            string sReceiptNumber = Constants.S_ZERO;
            int sRecieptNo = 0;
            int iAcademicYearId = 0;
            int iAccountHeaderId = 0;
            int iIsRefundFee = 0;
            foreach (string filter in sFilters.Where(str => !str.IsNullOrEmpty()))
            {
                sParameterValue = filter.Substring(filter.LastIndexOf("=") + 1);
                sParameterField = filter.Substring(filter.LastIndexOf(".") + 1, filter.LastIndexOf("=") - filter.LastIndexOf(".") - 1).Trim();

                if (aiSchoolId == Constants.SchoolId.SNS.ToInt())
                {
                    switch (sParameterField)
                    {
                        case "ReceiptNumber":
                            sReceiptNumber = sParameterValue;
                            break;
                        case "AcademicYearID":
                            iAcademicYearId = sParameterValue.ToInt();
                            break;
                        case "AccountHeaderId":
                            iAccountHeaderId = sParameterValue.ToInt();
                            break;
                        case "IsRefundReport":
                            iIsRefundFee = sParameterValue.ToInt();
                            break;
                    }
                }
                else
                {
                    switch (sParameterField)
                    {
                        case "ReceiptNumber":
                            sRecieptNo = sParameterValue.ToInt();
                            break;
                        case "AcademicYearID":
                            iAcademicYearId = sParameterValue.ToInt();
                            break;
                        case "AccountHeaderId":
                            iAccountHeaderId = sParameterValue.ToInt();
                            break;
                    }
                }
            }

            if (aiAdmissionId == 0 && aiSerialNo == Constants.S_ZERO)
            {
                if (aiSchoolId == Constants.SchoolId.SNS.ToInt())
                {
                    bool bIsRefundFee = false;

                    if (iIsRefundFee == Constants.I_ONE)
                        bIsRefundFee = true;

                    dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetReceiptDetailsForSNS(sReceiptNumber, iAcademicYearId, iAccountHeaderId, aiStudentId, bIsRefundFee);
                }
                else
                    dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetReceiptDetails(sRecieptNo, iAcademicYearId);
            }
            else if (aiAdmissionId != 0)
                dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetAdmissionReceiptDetails(aiAdmissionId, iAcademicYearId);
            else if (aiSerialNo != Constants.S_ZERO)
                dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetReceiptDetails(aiSerialNo.ToInt());

            dsFeeRecieptReportDetails.Tables[0].TableName = "StudentDetails";
            dsFeeRecieptReportDetails.Tables[1].TableName = "PaymentmodeDetails";
            if (dsFeeRecieptReportDetails.Tables.Count == 3)
                dsFeeRecieptReportDetails.Tables[2].TableName = "FeeDetails";
            crReportDocument.SetDataSource(dsFeeRecieptReportDetails);
        }

        /// <summary>
        /// This method is used to convert string into particular format.
        /// </summary>
        /// <param name="asFilterString"> </param>
        /// <returns>string</returns>
        private string FormatFilterString(string asFilterString)
        {
            asFilterString = asFilterString.Replace("AND", "@");
            asFilterString = asFilterString.Replace("OR", "@");
            asFilterString = asFilterString.Replace("(", String.Empty);
            asFilterString = asFilterString.Replace(")", String.Empty);
            asFilterString = asFilterString.Replace("{", String.Empty);
            asFilterString = asFilterString.Replace("}", String.Empty);

            return asFilterString;
        }

        #endregion -- PRIVATE METHOD(s) --
    }
}