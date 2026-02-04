// File Name  : LocalGuardianDetialsUI.aspx.cs
// Created By : Sonali 
// Date       : 11-09-2019
//Description : This class is used to fill Local Guardian Detials admission of student.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Configuration;
using System.Data;

public partial class LocalGuardianDetials : SchoolBase
{
    private const string S_ELEMENT = "element";
    
    StudentAdmissionsBL moStudentAdmissionsBL;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moStudentAdmissionsBL = new StudentAdmissionsBL();
            if (!IsPostBack)
            {
                ReadQueryString();
                GetLocalGuardianDetails();               
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
            string sGuardinaDetailsxml = GetGuardianDetails();
            string sStudentEduDetailsxml = GetStudentEducationDetails();

            moStudentAdmissionsBL.SaveGuardinAndEducationDetails(iSchoolId, miAcademicYearId, hidStudentAdmissionId.Value.ToInt(), sGuardinaDetailsxml, sStudentEduDetailsxml, miUserId);

            if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null && ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Admin
               || (Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Supervisor
               || (Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Teacher))
            {
                //string sParams = SendSMS(oDTStudentDetails); 
                //sQryString = CommonUtility.EncryptQuerystring(sParams); It is removed because there is no need to send sms to manually added student.
                Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] = null;
                Response.Redirect("~/RITeSchool/Admission/NewStudentAdmisionsListUI.aspx", false);
            }
            else
            {
                string sQueryString = "Form_Number=" + Constants.S_ZERO + "&Mobile_Number=" + Constants.S_ZERO + "&iAdmissionId=" + Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] + "&EnableAdmissionFormFee" + Convert.ToBoolean(QueryString["EnableAdmissionFormFee"]);
                sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
                Response.Redirect("~/RITeSchool/Admission/AdmissionThankYouUI.aspx?" + sQueryString, false);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void ReadQueryString()
    {
        if (QueryString["StudentAdmissionId"] != null)
            hidStudentAdmissionId.Value = QueryString["StudentAdmissionId"];
    }

    private void GetLocalGuardianDetails()
    {
        const int I_TABLE_GUARDIAN_DETAILS = 0;
        const int I_TABLE_EDUCATION_DETAILS = 1;
        DataSet dtDetails = moStudentAdmissionsBL.GetStudentGuardinaDetails(miSchoolId, miAcademicYearId, hidStudentAdmissionId.Value.ToInt());
        DataTable dtGuardiandtls = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS] as DataTable;
        DataTable dtEduDtls = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS] as DataTable;

        if (dtGuardiandtls.Rows.Count > Constants.I_ZERO)
        {
            txtFName.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FFirstName"].ToString();
            txtFMName.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FMiddleName"].ToString();
            txtFLName.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FLastName"].ToString();
            txtFCalDobPopup.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FDOB"].ToDateTime().ToString(Constants.S_DEFAULT_DATE);
            txtFAadharCardNo.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FAadharCardNo"].ToString();
            txtFPANNo.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FPanNo"].ToString();
            txtFQualification.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FEducation"].ToString();
            txtFMobile.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FMobile"].ToString();
            txtFEmail.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FEmailAddress"].ToString();
            txtFStudentRelation.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["FRelation"].ToString();

            txtSName.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SFirstName"].ToString();
            txtSMName.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SMiddleName"].ToString();
            txtSLName.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SLastName"].ToString();
            txtSCalDobPopup.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SDOB"].ToDateTime().ToString(Constants.S_DEFAULT_DATE);
            txtSAadharCardNo.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SAadharCardNo"].ToString();
            txtSPANNo.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SPanNo"].ToString();
            txtSQualification.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SEducation"].ToString();
            txtSMobile.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SMobileNo"].ToString();
            txtSEmail.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SEmailAddress"].ToString();
            txtSStudentRelation.Text = dtDetails.Tables[I_TABLE_GUARDIAN_DETAILS].Rows[0]["SRelation"].ToString();        
        }

        if (dtEduDtls.Rows.Count > Constants.I_ZERO)
        {
            txtLastExamDetails.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["LastExamDetails"].ToString();
            txtSyllabusFollowed.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["SyllabusFollowed"].ToString();
            txtEnglish.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["English"].ToString();
            txtSecondLanguage.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["SecondLanguage"].ToString();
            txtMaths.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["Maths"].ToString();
            txtSceince.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["Sceince"].ToString();
            txtSST.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["SST"].ToString();
            txtOther.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["Other"].ToString();
            txtTotalMarks.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["TotalMarks"].ToString();
            txtMaximumMarks.Text = dtDetails.Tables[I_TABLE_EDUCATION_DETAILS].Rows[0]["MaximumMarks"].ToString();
        }
    }

    private string GetGuardianDetails()
    {   

        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("StudentAdmissionGuardianDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionGuardianDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionGuardianDetail", "");

        string sAtrrName = "StudentAdmissionId";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidStudentAdmissionId.Value;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "GuardianNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = Constants.S_ONE;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FirstName";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFName.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MiddleName";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFMName.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LastName";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFLName.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DateOfBirth";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFCalDobPopup.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AadharCardNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFAadharCardNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PanNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFPANNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Qualification";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFQualification.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MobileNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFMobile.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "EmailAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFEmail.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Relation";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtFStudentRelation.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentAdmissionGuardianDetail", "");

        sAtrrName = "StudentAdmissionId";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidStudentAdmissionId.Value;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "GuardianNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = Constants.S_TWO;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "FirstName";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSName.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MiddleName";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSMName.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LastName";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSLName.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "DateOfBirth";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSCalDobPopup.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "AadharCardNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSAadharCardNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "PanNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSPANNo.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Qualification";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSQualification.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MobileNo";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSMobile.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "EmailAddress";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSEmail.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Relation";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSStudentRelation.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }

    private string GetStudentEducationDetails()
    {
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("AcademicInformationDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "AcademicInformationDetails", "");
        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "AcademicInformationDetail", "");

        string sAtrrName = "StudentAdmissionId";
        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = hidStudentAdmissionId.Value;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "LastExam";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtLastExamDetails.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SyllabusFolloed";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSyllabusFollowed.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "English";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtEnglish.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SecondLanguage";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSecondLanguage.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Maths";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMaths.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Science";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSceince.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SST";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtSST.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Other";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtOther.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "TotalMarks";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtTotalMarks.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "MaximumMarks";
        attr = oDoc.CreateAttribute(sAtrrName);
        attr.Value = txtMaximumMarks.Text.Trim();
        oXmlNode.Attributes.Append(attr);

        // Add the node to root node.
        oXmlRootNode.AppendChild(oXmlNode);
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);
        // return the string generated.
        return root.InnerXml;
    }
}