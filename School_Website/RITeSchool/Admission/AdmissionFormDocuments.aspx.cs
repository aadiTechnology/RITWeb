// File Name  : AdmissionFormDocuments.aspx.cs
// Created By : Amit 
// Date       : 17/11/2009
//Description : This class is used to view required document while student admisssion process
//              and redirect to student admisssion form.

using System;
using System.Configuration;
using System.Reflection;
using BusinessLogic.Exceptions;
using Utility;

public partial class AdmissionFormDocuments : SchoolBase
{
    #region " Events "

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                btnSubmit.Focus();
                ReadQueryString();
                SetStandardvalues();
            }

			int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();

            hidSchoolId.Value = iSchoolId.ToString();

            if (!Convert.ToBoolean(QueryString["EnableAdmissionFormFee"]))
                SubmissionWizardSteps.EnableFormFee = false;

            
            string sInnerText = string.Empty;
            string sMailAddress = string.Empty;

            if (iSchoolId == Constants.SchoolId.PPSN.ToInt())
            {
                if (iSchoolId == Constants.SchoolId.PPSN.ToInt())
                {
                    aSChoolWebsite.InnerText = "https://pawarpublicschool.com/nandedcity";                    
                    aSChoolWebsite.Attributes.Add("onclick", "OpenPopup()");
                }
                else if (iSchoolId == Constants.SchoolId.PPS.ToInt())
                {
                    aSChoolWebsite.InnerText = "https://pawarpublicschool.com/hadapsar";                    
                    aSChoolWebsite.Attributes.Add("onclick", "OpenPopup()");
                }

                if (miAcademicYearId == 0 && QueryString["AcademicYearId"] != null)
                    Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]  = QueryString["AcademicYearId"].ToString();
                
                sMailAddress = sInnerText = ConfigurationManager.AppSettings["AdmissionSenderEmailAddress"];

                tblSupportingDocuments.Visible = false;
                tblSupporingDocumenrsPPSN.Visible = true;

                if (iSchoolId == Constants.SchoolId.PPSN.ToInt())
                    sMailAddress = "info@ppsnandedcity.com";

                aAdmissionMail.InnerText = sMailAddress;
                aAdmissionMail.HRef = "mailto:" + sMailAddress;
            }
                          
            if (miAcademicYearId == 0 && QueryString["AcademicYearId"] != null)
                Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]  = QueryString["AcademicYearId"].ToString();

            if (iSchoolId == Constants.SchoolId.PPSH.ToInt())
            {
                tblSupportingDocumentsPPSH.Visible = true;
                tblSupportingDocuments.Visible = false;                
                tblSupporingDocumenrsPPSN.Visible = false;
            }
            else if (iSchoolId == Constants.SchoolId.ZLSP.ToInt())
            {                
                trdocZLSP.Visible = true;
                trNotes.Visible = false;
                trNotesDPIS.Visible = false;
                trNotesDetails.Visible = false;
                trNoticeDPIS.Visible = false;
                tblSupportingDocumentsPPSH.Visible = false;
                tblSupportingDocuments.Visible = true;
                tblSupporingDocumenrsPPSN.Visible = false;
            }
            else
            {
                tblSupportingDocuments.Visible = true;
                tblSupportingDocumentsPPSH.Visible = false;
                //tblSupporingDocumenrsPPSN.Visible = false;
            }

            if (iSchoolId != Constants.SchoolId.PPSN.ToInt())
                tblSupporingDocumenrsPPSN.Visible = false;
            else
                tblSupportingDocuments.Visible = false;
            
			if (iSchoolId == Constants.SchoolId.PPS.ToInt())
				docsPPS.Visible = true;
			if (iSchoolId == Constants.SchoolId.SS.ToInt())
				docsSS.Visible = true;
            if (iSchoolId == Constants.SchoolId.PPSH.ToInt() || iSchoolId == Constants.SchoolId.PPSN.ToInt())
				docsPPSH.Visible = true;
			if (iSchoolId == Constants.SchoolId.FBS.ToInt())
				docsFBS.Visible = true;
            if (iSchoolId == Constants.SchoolId.MCPS.ToInt())
                docsMCPS.Visible = true;
            if (iSchoolId == Constants.SchoolId.DSK.ToInt())
            {
                docsDSK.Visible = true;
                trDSKHeading.Visible = true;
            }
            else
                trHeading.Visible = true;

            if (iSchoolId == Constants.SchoolId.DPIS.ToInt() || iSchoolId == Constants.SchoolId.DPISRAVET.ToInt())
            {
                trDocumentHeaderDPIS.Visible = true;
                trDocumnetHeader.Visible = false;
                trdocDPIS.Visible = true;
                trNotes.Visible = false;
                trNotesDPIS.Visible = true;
                trNoticeDPIS.Visible = true;
                trNotesDetails.Visible = false;
                liPrintoutDPIS.Visible = false;                
            }
            else
            {
                trDocumentHeaderDPIS.Visible = false;
                trDocumnetHeader.Visible = true;
                trdocDPIS.Visible = false;                
                trNotesDPIS.Visible = false;
                trNoticeDPIS.Visible = false;
                liPrintoutDPIS.Visible = true;
            }

            //if (iSchoolId == Constants.SchoolId.DPISRAVET.ToInt())
            //    trInst.Visible = false;

            if (iSchoolId != Constants.SchoolId.PPS.ToInt())
                spnAmanotaMessage.InnerText = string.Empty;
            if (iSchoolId == Constants.SchoolId.DYPV.ToInt())
            {
               DYPVFEESTRUCTURE.Visible = true;
               liTC.InnerText = "Kolhapur";
            }
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to create quetrystring 
    /// and redirect to student admission form.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int iWtListId = 0;
            if (QueryString["WtListId"] != null && QueryString["WtListId"].ToString() != string.Empty)
                iWtListId = QueryString["WtListId"].ToInt();

			//string sQuerystring = Request.QueryString.ToString();
            string sQuerystring = "StandardId=" + hidStandardId.Value + "&AcademicYearId=" + hidAcademicYearId.Value + "&IsOnlineAdmission=1&EnableAdmissionFormFee=" + QueryString["EnableAdmissionFormFee"].ToString() + "&WtListId=" + iWtListId;

            Response.Redirect("~/RITeSchool/Admission/AdmissionFormStudentDetails.Aspx?" + CommonUtility.EncryptQuerystring(sQuerystring), false);		
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region "Private Method (s)"

    /// <summary>
    /// This method is used to decrypt querystring.
    /// </summary>
    private void ReadQueryString()
    {

        if (QueryString.Count > 0 && QueryString["StandardId"] != null)        
            hidStandardId.Value =  QueryString["StandardId"];

        if (QueryString.Count > 0 && QueryString["AcademicYearId"] != null)
            hidAcademicYearId.Value = QueryString["AcademicYearId"];

       
    }

    private void SetStandardvalues()
    {
        string sStandardName = QueryString["StandardName"].ToString();
        string sGrade = "GRADE ";
        if (sStandardName == "Nursery" || sStandardName == "Junior KG" || sStandardName == "Senior KG")
            sGrade = string.Empty;

        lblAdmissionProcess.Text = "ADMISSION PROCESS FOR " + sGrade + sStandardName.ToUpper();

        int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
        if (sStandardName == "Nursery")
        {
            NursaryCriteria.Visible = true;
            liDueNote.Visible = false;

            if (iSchoolId == Constants.SchoolId.PPSN.ToInt() && hidAcademicYearId.Value == "14")
            {
                NursaryCriteria.InnerHtml = "Eligibility : 3 years complete as on 31<sup>st</sup>December, 2026.(All children born on or between 1<sup>st</sup> October, 2022 and 31<sup>st</sup> December, 2023).";
            }
        }
        else
        {
            NursaryCriteria.Visible = false;
            if (iSchoolId == Constants.SchoolId.PPS.ToInt())
                liLC.Visible = true;
        }

        if (sStandardName == "Nursery" || sStandardName == "Junior KG" || sStandardName == "Senior KG")
            StandardwiseLC.Visible = false;
        else
            StandardwiseLC.Visible = true;

        if (sStandardName == "Nursery" || sStandardName == "Junior KG" || sStandardName == "Senior KG" || sStandardName == "1")
        {
            liLottery.Visible = true;
            liActivity1.Visible = false;
            liActivity2.Visible = false;
            SecondOnward.Visible = false;
            liGrade2.Visible = false;
            liGrade2Merit.Visible = false;            
        }
        else
        {
            liLottery.Visible = false;
            liActivity1.Visible = true;
            liActivity2.Visible = true;
            SecondOnward.Visible = true;
            liGrade2.Visible = true;
            liGrade2Merit.Visible = true;
            liGradePreference.Visible = false;           
        }

        //if (iSchoolId == Constants.SchoolId.PPSN.ToInt() && sStandardName == "5")
        //    liGrade2.Visible = false;

        if (sStandardName == "Junior KG")
        {
            JrkG.Visible = true;
            JrkG.InnerHtml = "Eligibility : 4 years complete as on 31<sup>st</sup> December, 2026.(All children born on or between 1<sup>st</sup> October, 2021 to 31<sup>st</sup> December, 2022).";
        }
        else
            JrkG.Visible = false;
         
        if (sStandardName== "Senior KG")
            SrKg.Visible = true;
        else
            SrKg.Visible = false;

        if (sStandardName == "1")
        {
            First.Visible = true;
            liGrade1.Visible = true;
        }
        else
        {
            First.Visible = false;
            liGrade1.Visible = false;
        }


        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSH.ToInt())
        {            
            //if (sStandardName == "Nursery")
            //{
            //    trPPSHNurseryDocuments.Visible = true;
            //}
            //else if (sStandardName == "Junior KG" || sStandardName == "Senior KG" || sStandardName == "1")
            //{
            //    trPPSHJRKGto1.Visible = true;
            //}
            //else
            //{
            //    trPPSH2to10.Visible = true;
            //}

            if (sStandardName == "Nursery" || sStandardName == "Junior KG" || sStandardName == "Senior KG")
            {
                liGrade1PPSH.Visible = false;
                TrPPSHNurseryToGrade1AdmissionProcess.Visible = true;
               
            }
            else if (sStandardName == "1")
            {
                TrPPSHNurseryToGrade1AdmissionProcess.Visible = true;
                spnAdmissionDate.InnerText = " as on today.";
                spnGrade1.InnerText = " parents will receive a mail with further admission details.";
               
            }
            else if ( sStandardName == "2" || sStandardName == "3" || sStandardName == "4" || sStandardName == "5")
            {
                TrPPSHGrade2ToGrade9AdmissionProcess.Visible = true;
                spn1to5.InnerText = " as on today.";
             }
            else if (sStandardName == "6" || sStandardName == "7" || sStandardName == "8" || sStandardName == "9")
            {
                TrPPSHGrade2ToGrade9AdmissionProcess.Visible = true;
            }

            spnPPSHAdmissionProcess.InnerText = "ADMISSION PROCESS FOR GRADE " + sStandardName.ToUpper();
            spnPPSHAdmissionForNurTo1.InnerText = "ADMISSION PROCESS FOR " + (sStandardName == "1" ? "GRADE 1" : sStandardName.ToUpper());
        }

        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPSN.ToInt())
        {
            if (sStandardName == "Junior KG" || sStandardName == "1")
                liDueNote.Visible = false;

            if (!(sStandardName == "Nursery" || sStandardName == "Junior KG" || sStandardName == "Senior KG" || sStandardName == "1"))
            {
                //liRandomAdmission.Visible = false;
                liBonafideNote.InnerText = "For Std. II to Std.VIII. previous School's bonafide certificate along with U-DISE and SARAL ID of the student is mandatory.";
                liBonafideNote.Style.Add("font-weight", "bold");
                liLCTC.Visible = true;
                liMarkSheet.Visible = true;
            }
            else
                liBonafideNote.Visible = false;

            if (sStandardName == "Nursery" || sStandardName == "Junior KG" || sStandardName == "Senior KG")
            {
                Tr3.Visible = true;
                Tr1to10.Visible = false;
            }
            else
            {
                Tr3.Visible = false;
                Tr1to10.Visible = true;
            }

            if (sStandardName == "2" || sStandardName == "3" || sStandardName == "4" || sStandardName == "5" || sStandardName == "6" || sStandardName == "7" || sStandardName == "8")
            {
                liGrade2.InnerText = "An activity paper of 1 hour will be conducted on Saturday, 7th February 2026 at 10:00 a.m. The form numbers of selected students (merit list) will be declared on 12th  February 2026 at 3:00 p.m. on the school website and notice board.";
                liGrade2to8.Visible = true;
                bPPSN2to8.InnerText = "Friday, 24th April, 2026";
            }

        }
        
        if (ConfigurationManager.AppSettings["SchoolID"].ToInt() == Constants.SchoolId.PPS.ToInt())
        {
            if (!(sStandardName == "Nursery" || sStandardName == "Junior KG" || sStandardName == "Senior KG" || sStandardName == "1"))
            {
                li2to9PPSMarkSheet.Visible = true;
                li2to9PPSLC.Visible = true;
                liLC.Visible = false;
            }
        }
    }

    #endregion
}
