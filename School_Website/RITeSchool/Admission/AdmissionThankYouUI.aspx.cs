using System;
using System.Configuration;
using System.Reflection;
using BusinessLogic.Exceptions;
using BusinessLogic;
using Utility;
using SchoolEntities;
using CrystalDecisions.Shared;
using System.Threading;
using System.Text;

using System.Data;
using System.Linq;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class AdmissionThankYouUI : SchoolBase
{
    /// <summary>
    /// This method is used to handle a page load event and intitalize the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ReadQueryString();

            if(ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
            {
                HyperLink2.NavigateUrl = "~/RITeSchool/DOWNLOADS/AdmissionForms/Revised Medical History Sheet.pdf";
                trPPShUndertakingForm.Visible = true;
                trSibling.Visible = false;
                HyperLink3.NavigateUrl = "~/RITeSchool/DOWNLOADS/AdmissionForms/Parental Consent Form Rev1.pdf";
            }

            hlnkReceipt.NavigateUrl = hlnkReceipt.NavigateUrl + "?" + Request.QueryString;
            hlnkReceipt.Attributes.Add("onclick", "window.open('" + hlnkReceipt.NavigateUrl
                                                   + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600'); return false;");
            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt())
            {
                hlnkAdmissionForm.NavigateUrl = hlnkAdmissionForm.NavigateUrl + "?" + Request.QueryString;

                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
                {
                    hlnkAdmissionForm.Attributes.Add("onclick", "window.open('" + hlnkAdmissionForm.NavigateUrl
                                                            + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=300');return false;");
                }
                else
                {
                    hlnkAdmissionForm.Attributes.Add("onclick", "window.open('" + hlnkAdmissionForm.NavigateUrl
                                                            + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600');return false;");
                }
            }
            else
            {
                string sQueryStrings = "StudentAdmissionId=" + QueryString["iAdmissionId"];
                sQueryStrings = CommonUtility.EncryptQuerystring(sQueryStrings);
                hlnkAdmissionForm.NavigateUrl = hlnkAdmissionForm.NavigateUrl + "?" + sQueryStrings;
                hlnkAdmissionForm.Attributes.Add("onclick", "window.open('" + hlnkAdmissionForm.NavigateUrl
                                                        + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600');return false;");
            }

            string sQuery = "sIsSubling=" + Constants.S_YES;
            if (QueryString["AcademicYearId"] != null && QueryString["AcademicYearId"].ToString() != string.Empty)
                sQuery += "&AcademicYearId=" + QueryString["AcademicYearId"].ToString();

            string sQuerystring = CommonUtility.EncryptQuerystring(sQuery);

            HyperLink1.NavigateUrl = HyperLink1.NavigateUrl + "?" + sQuerystring;

            string sQueryString = "EnquiryId=" + hidEnquiryId.Value;
            sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
            hlnkSPSRegistration.NavigateUrl = hlnkSPSRegistration.NavigateUrl + "?" + sQueryString;
            hlnkSPSRegistration.Attributes.Add("onclick", "window.open('" + hlnkSPSRegistration.NavigateUrl
                                                    + "' , '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=500,height=500');return false;");

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt())
            {
                trPPSDocuments.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This method is used to decrypt query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["EnquiryId"] == null)
        {
            if (QueryString.Count > 0)
            {
                String UserName = string.Empty;
                String MobileNo = string.Empty;
                
                if (QueryString["Form_Number"] != null)
                    UserName = QueryString["Form_Number"];
                if (QueryString["Mobile_Number"] != null)
                    MobileNo = QueryString["Mobile_Number"];               
                int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
                SetControls();

                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.SPS.ToInt())
                {
                    if (Session["IsInternalAdmission"] == null || Session["IsInternalAdmission"].ToString() != Constants.S_YES)
                    {
                        UserAuthentication oUserAuthentication = new UserAuthentication(iSchoolId, UserName, MobileNo);
                        if (!oUserAuthentication.ValidUser)
                            Response.Redirect("OnlineAdmissionDashBoardUI.aspx", false);
                        else
                            oUserAuthentication.UpdateAdmissionLoginSession();
                    }
                }

                Session["IsInternalAdmission"] = Constants.S_NO;

                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt() ||
                    ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt() ||
                    ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt() ||
                    ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt() ||
                    ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt())
                    SendMail();
            }
        }
        else
        {
            hidEnquiryId.Value = QueryString["EnquiryId"];
            SubmissionWizardSteps.Visible = false;
            tblMainDetails.Visible = false;
            tblSPSRegistrationLink.Visible = true;
        }
    }

    private void SendMail()
    {   
        try
        {
            if (QueryString["iAdmissionId"] != null)
            {
                if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.DPIS.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.DPISRAVET.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.PPSN.ToInt() && 
                    ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.PPS.ToInt())
                {
                    trMedicalForm.Visible = true;
                    trParentalConsentForm.Visible = true;
                }
                
                string SendAdmissionFormEmail = ConfigurationManager.AppSettings["SendAdmissionFormEmail"];
                if (SendAdmissionFormEmail.Trim() == Constants.S_YES)
                {
                    string sEmailAddress = string.Empty;
                    string sFileName = string.Empty;
                    int iAdmissionId = QueryString["iAdmissionId"].ToInt();
                    string sAdmnFileName = string.Empty, sTeacherFileName = string.Empty;

                    StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
                    AdmissionDetails oAdmissionDetails = oStudentAdmissionsBL.GetSingleStudentAdmissionDetails(ConfigurationManager.AppSettings["SchoolID"].ToInt(), iAdmissionId);
                    if (oAdmissionDetails.StudentName != string.Empty)
                    {
                        sFileName = Server.MapPath("~") + @"\RITeSchool\DOWNLOADS\AdmissionForms\" + oAdmissionDetails.StudentName + "_" + oAdmissionDetails.FormNumber + "_" + oAdmissionDetails.DOB.Day.ToString() + oAdmissionDetails.DOB.Month.ToString() + oAdmissionDetails.DOB.Year.ToString() + "_" + oAdmissionDetails.AcademicYear + ".pdf";
                        sEmailAddress = oAdmissionDetails.EmailAddress;
                    }

                    if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
                    {
                        sAdmnFileName = sFileName.Replace(".pdf", "_AdministrationCopy.pdf");
                        string SFilterString = "(usp_GetAdmmissionFormReport.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND usp_GetAdmmissionFormReport.StudentAdmissionId}=" + iAdmissionId + " AND usp_GetAdmmissionFormReport.IsTeachersCopy}=" + "0" + " AND usp_GetAdmmissionFormReport.AdmissionForCurrentYear}=" + (Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO) + ") @";
                        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.AdmissionFormReport, SFilterString, ExportFormatType.PortableDocFormat, sAdmnFileName, false);
                        oReportDisplay.DisplayReport();

                        sTeacherFileName = sFileName.Replace(".pdf", "_TeacherCopy.pdf");
                        SFilterString = "(usp_GetAdmmissionFormReport.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND usp_GetAdmmissionFormReport.StudentAdmissionId}=" + iAdmissionId + " AND usp_GetAdmmissionFormReport.IsTeachersCopy}=" + "1" + " AND usp_GetAdmmissionFormReport.AdmissionForCurrentYear}=" + (Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO) + ") @";
                        oReportDisplay = new ReportDisplay(Constants.ExportReports.AdmissionFormReport, SFilterString, ExportFormatType.PortableDocFormat, sTeacherFileName, false);
                        oReportDisplay.DisplayReport();
                    }
                    else
                    {
                        string SFilterString = "(usp_GetAdmmissionFormReport.SchoolId}=" + ConfigurationManager.AppSettings["SchoolID"] + " AND usp_GetAdmmissionFormReport.StudentAdmissionId}=" + iAdmissionId + " AND usp_GetAdmmissionFormReport.AdmissionForCurrentYear}=" + (Settings.ShowAdmissionForCurrentYear ? Constants.S_YES : Constants.S_NO) + ") @";
                        ReportDisplay oReportDisplay = new ReportDisplay(Constants.ExportReports.AdmissionFormReport, SFilterString, ExportFormatType.PortableDocFormat, sFileName, false);
                        oReportDisplay.DisplayReport();
                    }

                    string sRcptFile = string.Empty;
                    if (oAdmissionDetails.ReceiptNumber != 0)
                    {
                        ExportAdmissionReceipt oExportAdmissionReceipt = new ExportAdmissionReceipt();
                        string sPath = Server.MapPath("~") + @"\RITeSchool\Report\Fee\FeeReciept.rpt";
                        sRcptFile = Server.MapPath("~") + @"\RITeSchool\DOWNLOADS\AdmissionForms\AdmissionReceipt_" + oAdmissionDetails.StudentName + "_" + oAdmissionDetails.FormNumber + "_" + oAdmissionDetails.DOB.Day.ToString() + oAdmissionDetails.DOB.Month.ToString() + oAdmissionDetails.DOB.Year.ToString() + "_" + oAdmissionDetails.AcademicYear + ".pdf";
                        oExportAdmissionReceipt.DisplayReport(oAdmissionDetails.AcademicYearId, oAdmissionDetails.ReceiptNumber, iAdmissionId, sPath, sRcptFile);
                        sRcptFile = sRcptFile + ",";
                    }

                    string sAdmissionSenderEmailAddress = ConfigurationManager.AppSettings["AdmissionSenderEmailAddress"];

                    if (sEmailAddress.Trim() != string.Empty)
                    {
                        string sSchoolEmailAddresses = ConfigurationManager.AppSettings["AdmissionSchoolEmailAddresses"];
                        if (sSchoolEmailAddresses.Trim() != string.Empty)
                            sEmailAddress = sEmailAddress + "," + sSchoolEmailAddresses;

                        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
                            sFileName = sAdmnFileName + "," + sTeacherFileName + "," + sRcptFile + Server.MapPath("~") + @"\RITeSchool\DOWNLOADS\AdmissionForms\Revised Medical History Sheet.pdf" + "," + Server.MapPath("~") + @"\RITeSchool\DOWNLOADS\AdmissionForms\Parental Consent Form Rev1.pdf" + "," + Server.MapPath("~") + @"\RITeSchool\DOWNLOADS\AdmissionForms\Undertaking Form.pdf";
                        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt())
                        {
                            if (sRcptFile.EndsWith(","))
                                sRcptFile = sRcptFile.Substring(0, sRcptFile.Length - 1);

                            sFileName = sFileName + "," + sRcptFile;
                        }
                        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt())
                        {
                            sFileName = sFileName + "," + sRcptFile + Server.MapPath("~") + @"\RITeSchool\DOWNLOADS\AdmissionForms\Consent_Undertaking_medical_and_apaar_form.pdf";
                        }
                        else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.DPIS.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.DPISRAVET.ToInt())
                            sFileName = sFileName + "," + sRcptFile + Server.MapPath("~") + @"\RITeSchool\DOWNLOADS\AdmissionForms\Revised Medical History Sheet.pdf" + "," + Server.MapPath("~") + @"\RITeSchool\DOWNLOADS\AdmissionForms\Parental Consent Form.pdf";

                        String sMessaegBody = GenerateMesageBody(oAdmissionDetails.StudentName, oAdmissionDetails.AcademicYear, oAdmissionDetails.SalutationId);
                        CommonUtility.SendMail(sEmailAddress, sAdmissionSenderEmailAddress, "Admission form "+oAdmissionDetails.FormNumber+" "+oAdmissionDetails.StudentName, sMessaegBody, sFileName);
                    }
                }
            }
        }
        catch (ThreadAbortException)
        {   
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private string GenerateMesageBody(string asStudentName, string asAcademicYear, int aiSalutationId)
    {
        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt() ||
           ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt())
        {
            StringBuilder obj = new StringBuilder();
            obj.Append("<table style='width:100%;font-family:Cambria;'>");
            string sStyle = "style='line-height:30px;'";
            string sTrStyle = "style='height:10px'";

            obj.Append("<tr><td>");
            obj.Append("<span " + sStyle + ">Dear Sir / Ma'am,</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");

            string sSchoolName = "Pawar Public School";
            string sSchoolWebsite = string.Empty;
            string sAdmissionSenderEmailAddress = ConfigurationManager.AppSettings["AdmissionSenderEmailAddress"];
            string sSalutationWord = "his";
            if (aiSalutationId == Constants.Salutation.Miss.ToInt())
                sSalutationWord = "her";

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt())
            {
                sSchoolName = "Pawar Public School, Nanded city";
                sSchoolWebsite = "www.ppsnandedcity.com";
            }
            else if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt())
            {
                sSchoolName = "Pawar Public School, Amanora, Hadapsar";
                sSchoolWebsite = "www.ppspune.com";
            }           
           
            obj.Append("<tr><td>");
            obj.Append("<span " + sStyle + ">Greetings from " + sSchoolName + "!</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span " + sStyle + ">We thank you for showing your interest in seeking admission for your child in our school for the academic year " + asAcademicYear + ".</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span " + sStyle + ">For more information, please refer to our Admission Notification Circular " + asAcademicYear + ", displayed on the Home page of our website <a href='" + sSchoolWebsite + "'>" + sSchoolWebsite + "<a>.</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span " + sStyle + ">We wish your child all the very best for " + sSalutationWord + " future studies.</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr style='height:20px;'><td>");
            obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span>Best regards,</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span>Principal</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span>" + sSchoolName + "</span>");
            obj.Append("</td></tr>");

            obj.Append("</table>");

            return obj.ToString();
        }
        else
        {
            StringBuilder obj = new StringBuilder();
            obj.Append("<table style='width:100%;font-family:Cambria;'>");
            string sStyle = "style='line-height:30px;'";
            string sTrStyle = "style='height:10px'";

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())
            {
                obj.Append("<tr><td>");
                obj.Append("<span " + sStyle + ">Dear Sir / Ma'am,</span>");
                obj.Append("</td></tr>");
            }
            else
            {
                obj.Append("<tr><td>");
                obj.Append("<span " + sStyle + ">Dear Ma'am / Sir,</span>");
                obj.Append("</td></tr>");
            }

            obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");
            string sSchoolName = string.Empty;

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())            
                sSchoolName = "Delhi Public International School, Pimple Saudagar";                
            else
                sSchoolName = "Pawar Public School, Hinjewadi";

            string sAdmissionSenderEmailAddress = ConfigurationManager.AppSettings["AdmissionSenderEmailAddress"];
            
            obj.Append("<tr><td>");
            obj.Append("<span " + sStyle + ">Greetings from " + sSchoolName + "!</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span " + sStyle + ">Thank you for showing interest in seeking admission for your child in our school, for the academic year " + asAcademicYear + ".</span>");
            obj.Append("</td></tr>");

            obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.DPIS.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.DPISRAVET.ToInt())
            {
                obj.Append("<tr><td>");

                if(ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
                    obj.Append("<span " + sStyle + "><b><i>Kindly take a print out of the documents attached with this mail and share the scanned signed copy of the admission form with the list of supporting documents mentioned in the admission form to complete the Admission process.</i></b></span>");
                else
                    obj.Append("<span " + sStyle + "><b><i>Kindly take a print out of the documents attached with this mail and share the scanned signed copy of the admission form with the necessary documents to complete the Admission process.</i></b></span>");

                obj.Append("</td></tr>");
            }
            else
            {
                obj.Append("<tr><td>");
                obj.Append("<span " + sStyle + ">Kindly visit the school office along with the necessary documents to complete the Admission process.</span>");
                obj.Append("</td></tr>");
            }

            obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span " + sStyle + ">Admission confirmation will be subject to payment of fees and submission of required documents.</span>");
            obj.Append("</td></tr>");

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.DPIS.ToInt() && ConfigurationManager.AppSettings["SchoolID"].ToInt() != Constants.SchoolId.DPISRAVET.ToInt())
            {
                obj.Append("<tr " + sTrStyle + "><td>"); obj.Append("</td></tr>");

                obj.Append("<tr><td>");
                obj.Append("<span " + sStyle + ">For more information, kindly contact us at 020-67703700 / 67703701 OR email us at admissions@ppshinjewadi.com</span>");
                obj.Append("</td></tr>");
            }

            obj.Append("<tr style='height:20px;'><td>");
            obj.Append("</td></tr>");

            obj.Append("<tr><td>");
            obj.Append("<span>Best regards,</span>");
            obj.Append("</td></tr>");

            if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
            {
                obj.Append("<tr><td>");
                obj.Append("<span>Principal</span>");
                obj.Append("</td></tr>");
            }

            obj.Append("<tr><td>");
            obj.Append("<span>" + sSchoolName + "</span>");
            obj.Append("</td></tr>");

            obj.Append("</table>");

            return obj.ToString();
        }
    }

    /// <summary>
    /// This is used to set Controls upon conditions.
    /// </summary>
    private void SetControls()
    {
        if (!Convert.ToBoolean(QueryString["EnableAdmissionFormFee"]))
        {
            SubmissionWizardSteps.EnableFormFee = false;
            trPaymentAdmissionForm.Visible = false;
            trPaymentNote.Visible = false;
            trPaymentReceipt.Visible = false;
            trPaymentThankYou.Visible = false;
            //trSMS.Visible = true;
            StudentAdmissionsBL oStudentAdmissionsBL = new StudentAdmissionsBL();
            oStudentAdmissionsBL.SetAdmission(Session[Constants.S_SESSION_STUDENT_ADMISSION_ID].ToInt());
        }
        else
        {
            trAdmissionForm.Visible = false;
            trNote.Visible = false;
            trThankYou.Visible = false;
            trSMS.Visible = false;

            int iSchoolId = ConfigurationManager.AppSettings["SchoolId"].ToInt();
            if (iSchoolId == Constants.SchoolId.DPIS.ToInt() || iSchoolId == Constants.SchoolId.DPISRAVET.ToInt())
            {
                if (QueryString["Amount"] != null && QueryString["Amount"].ToString() != string.Empty && QueryString["TxnId"] != null && QueryString["TxnId"].ToString() != string.Empty)
                    lblSuccessMessage.Text = "Your payment of admission form Rs. " + QueryString["Amount"].ToString() + " is successfully received. Transaction No. : " + QueryString["TxnId"].ToString();
                else
                    lblSuccessMessage.Text = "Your payment of admission form is successfully received.";
            }
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPIS.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.DPISRAVET.ToInt())
        {
            trNote.Visible = false;
            trSMS.Visible = false;
        }

        if (Session["IsInternalAdmission"] != null && Session["IsInternalAdmission"].ToString() == Constants.S_YES)
        {
            trSibling.Visible = false;
            trCloseButton.Visible = true;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.SPS.ToInt())
        {
            trSMS.Visible = false;
            trSibling.Visible = false;
            SubmissionWizardSteps.Visible = false;
            trCloseButton.Visible = true;
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt() || ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt())
            trSibling.Visible = false;
    }
}

public class ExportAdmissionReceipt 
{
    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// This method is used to display report
    /// </summary>
    /// <param name="msFilter"></param>
    public void DisplayReport(int aiAcademicYearId, int aiReceiptNumber, int aiAdmissionId, string asReportFilePath, string asServerFilePath)
    {
        ReportDocument crReportDocument = new ReportDocument();
        try
        {          
            var crConnectionInfo = new ConnectionInfo();
            var crtableLogoninfos = new TableLogOnInfos();
            var crtableLogoninfo = new TableLogOnInfo();

            crConnectionInfo.ServerName = ConfigurationManager.AppSettings["ReportingDataSource"];
            crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["ReportDataBaseName"];
            crConnectionInfo.UserID = ConfigurationManager.AppSettings["ReportingUserId"];
            crConnectionInfo.Password = ConfigurationManager.AppSettings["ReportingPassword"];

            crReportDocument.Load(asReportFilePath);

            var dsFeeRecieptReportDetails = new DataSet();

            dsFeeRecieptReportDetails = StudentFeeDetailsBL.GetAdmissionReceiptDetails(aiAdmissionId, aiAcademicYearId);

            dsFeeRecieptReportDetails.Tables[0].TableName = "StudentDetails";
            dsFeeRecieptReportDetails.Tables[1].TableName = "PaymentmodeDetails";
            if (dsFeeRecieptReportDetails.Tables.Count == 3)
                dsFeeRecieptReportDetails.Tables[2].TableName = "FeeDetails";
            crReportDocument.SetDataSource(dsFeeRecieptReportDetails);

            crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, asServerFilePath);

            crtableLogoninfo = null;
            crtableLogoninfos = null;
            crConnectionInfo = null;
        }
        catch (ThreadAbortException)
        {
        }
        finally
        {
            if (crReportDocument != null)
            {
                crReportDocument.Close();
                crReportDocument.Dispose();
            }	
        }
    }

    #endregion -- PRIVATE METHOD(s) --
}
