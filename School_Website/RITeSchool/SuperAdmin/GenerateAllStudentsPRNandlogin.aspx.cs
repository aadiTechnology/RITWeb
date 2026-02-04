/* Creater : Shankar Gurav
 * Created date: 2 July 2008
 * Last updated date: 3 July 2008
 * Purpose : This page class is gives the new academic generation wizard to the user.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using MasterEntities;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Configuration;

public partial class GenerateAllStudentsPRNandlogin : SchoolBase
{

    #region " Constants "

    private int iLoginId = 10000;
    const int I_SMS_TEMPLATE = 2;
    const int I_SMS_TEMPLATE_NAME = 1;
    const int I_USER_ROLE_TEACHER = 1;
    const int I_USER_ROLE_ADMINSTAFF = 2;
    const string S_SUCCESS_MSG = "SMS sent successfully!!!";
    const string S_LOGIN_ACTIVATE = "Login for all students has been activated successfully!!!";
    const string S_REPLACE_URL = "http://";
    const int I_SMS_TEMPLATE_TXT = Constants.I_TWO;
    const int I_SMS_SUBJECT_TXT = Constants.I_ONE;
    const int I_SMS_TYPE = Constants.I_THREE;
    const int I_STUDENTDETAILS = Constants.I_ZERO;
    const int I_LOGINID = Constants.I_ONE;

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to hadle page load event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            SetJavaScriptAttributes();
        }
        catch (Exception ex)
        {
          ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to generete login and password for all students in school. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        string sStudentXml = GenerateXml();
        StudentBL.UpdateAllStudentsLogins(miSchoolId, sStudentXml);
        lblUpdate.Text = " Login IDs and passwords are generated successfully";
        lblUpdate.ForeColor = System.Drawing.Color.Blue;
    }

    /// <summary>
    /// This event is used to move back on superadmin dashboard.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master;
            oSuperAdminMasterPage.RedirectToNextPage("~/SuperAdmin/ScreensUI.aspx");
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex,MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to send SMS of login and password to all students in school.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSMS_Click(object sender, EventArgs e)
    {
        try
        {
            int iSmsId = Convert.ToInt32(Constants.SMSTemplate.ForgotPasswordDetailSMS);
            int iSMSType = 0;
            string sLoginDetailsSmsText = string.Empty;
            string sTemplateRegistrationId = string.Empty;
            string sSmsSubject = string.Empty;
            DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
            if (oDTSmsTemplate.Rows.Count != 0)
            {
                if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
                {
                    sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);

                    if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                    sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][1]);
                }
                if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                    iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
            }
            DataTable odtStudents = null;
            if (chkSMSOldStudents.Checked && chkSMSNewStudents.Checked)
			{
                DataSet oDataSet = new DataSet();
                oDataSet=StudentBL.GetAllStudents(miSchoolId, miAcademicYearId);
                odtStudents = oDataSet.Tables[I_STUDENTDETAILS];
            }
            else if (chkSMSOldStudents.Checked)
                odtStudents = StudentBL.GetAllStudents(miSchoolId, Constants.C_NO, miAcademicYearId);
            else if (chkSMSNewStudents.Checked)
                odtStudents = StudentBL.GetAllStudents(miSchoolId, Constants.C_YES, miAcademicYearId);

            SchoolBL oSchoolBL = new SchoolBL(miSchoolId);

            foreach (DataRow drStudent in odtStudents.Rows)
            {
                string sUserLogin = drStudent["User_Login"].ToString();
                string sUserPass = CommonUtility.GetDecryptedPassword(sUserLogin, drStudent["User_Password"].ToString());                
                string sLoginDetails = sLoginDetailsSmsText;
                sLoginDetails = sLoginDetails.Replace("%LOGIN%", sUserLogin).Replace("%PASSWORD%", sUserPass);
                SMS oSMS = new SMS();
                oSMS.Sender = oSchoolBL.SMSSenderName;
                oSMS.SMSText = sLoginDetails;
                oSMS.SMSType = iSMSType;
                oSMS.TemplateRegistrationId = sTemplateRegistrationId;
                oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
                oSMS.DisplayText = drStudent["Name"].ToString();
                oSMS.SMSTypeId = Constants.SMSTypes.ForgotPasswordDetailSMS.ToInt();
                oSMS.To.Add(drStudent["ID"].ToString(), drStudent["Mobile_Number"].ToString());
				if (!string.IsNullOrEmpty(drStudent["Mobile_Number2"].ToString()))
					oSMS.To.Add(drStudent["ID"].ToString() + "sm;", drStudent["Mobile_Number2"].ToString());
                oSMS.Send();
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MobileUrl"]) && chkSendMobileSMS.Checked)
                    SendMobileDetailsSMS(oSchoolBL, drStudent);
            }

            ClearFields();
            lblUpdate.Text = S_SUCCESS_MSG;
            lblUpdate.ForeColor = System.Drawing.Color.Blue;
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());        
        }
    }

    /// <summary>
    ///  Button Send's sms to teacher
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSMSTeacher_Click(object sender, EventArgs e)
    {
        try
        {

            this.SendSMSToUsers(I_USER_ROLE_TEACHER);
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());        
        }
    }
    /// <summary>
    /// button sends sms to admin
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSMSAdminstaff_Click(object sender, EventArgs e)
    {
        try
        {
            this.SendSMSToUsers(I_USER_ROLE_ADMINSTAFF);
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());        
        }
    }

    /// <summary>
    /// This event is used to activate all student logins from super admin screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnActivateLogin_Click(object sender, EventArgs e)
    {
        try
        {
            StudentBL.ActivateStudentLogins(miSchoolId);
            lblUpdate.Text = S_LOGIN_ACTIVATE;
            lblUpdate.ForeColor = System.Drawing.Color.Blue;            
        }
        catch (Exception ex)
        {
           ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());        
        }        
    }

    #endregion " Events "

    #region " Public Methods "

    /// <summary>
    /// This method is used to clear the checkboxes after sending sms.
    /// </summary>
    private void ClearFields()
    {
        chkSendMobileSMS.Checked = false;
        chkSMSNewStudents.Checked = false;
        chkSMSOldStudents.Checked = false;
    }

    /// <summary>
    /// This method is used to send sms about mobile site details.
    /// </summary>
    /// <param name="oSchoolBL"></param>
    /// <param name="drStudent"></param>
    private void SendMobileDetailsSMS(SchoolBL aoSchoolBL, DataRow adrStudent)
    {
        string sMobileSmsTemplate = string.Empty;
        string sSmsSubject = string.Empty;
        int iTemplateId = Constants.SMSTemplate.MobileWebsiteDetailsSMS.ToInt();
        int iSMSType = 0;
        DataTable oDTMobileSMSTemplate = SmsTemplateBL.GetTemplate(iTemplateId, miSchoolId);

        if (oDTMobileSMSTemplate.IsNonEmpty())
        {
                if (oDTMobileSMSTemplate.Rows[Constants.I_ZERO][I_SMS_TEMPLATE_TXT] != DBNull.Value)
                {
                    sMobileSmsTemplate = Convert.ToString(oDTMobileSMSTemplate.Rows[0][I_SMS_TEMPLATE_TXT]);
                    sSmsSubject = Convert.ToString(oDTMobileSMSTemplate.Rows[0][I_SMS_SUBJECT_TXT]);
                }
                if (oDTMobileSMSTemplate.Rows[Constants.I_ZERO][I_SMS_TYPE] != DBNull.Value)
                    iSMSType = oDTMobileSMSTemplate.Rows[Constants.I_ZERO][I_SMS_TYPE].ToInt();

            SMS oSMS = new SMS();
            oSMS.Sender = aoSchoolBL.SMSSenderName;
			oSMS.SMSText = sMobileSmsTemplate.Replace("%WEBSITE%", ConfigurationManager.AppSettings["MobileUrl"].Replace(S_REPLACE_URL, string.Empty));
            oSMS.SMSType = iSMSType;
            oSMS.School_Name = aoSchoolBL.SchoolName + "::" + sSmsSubject;
            oSMS.DisplayText = adrStudent["Name"].ToString();
            oSMS.To.Add(adrStudent["ID"].ToString(), adrStudent["Mobile_Number"].ToString());
			if (!string.IsNullOrEmpty(adrStudent["Mobile_Number2"].ToString()))
				oSMS.To.Add(adrStudent["ID"].ToString() + "sm;", adrStudent["Mobile_Number2"].ToString());
            oSMS.Send();
        }
    }

    /// <summary>
    /// This mehod is used to create XML for all school Students. 
    /// </summary>
    /// <returns></returns>
    private string GenerateXml()
    {
        XmlDocument oDoc = new XmlDocument();
        // Create a root level element.
        XmlElement root = oDoc.CreateElement("Students");
        Random oRandom = new Random((int)DateTime.Now.Ticks);
        DataSet odsStudents = StudentBL.GetAllStudents(miSchoolId, miAcademicYearId);
        DataTable odtStudents = odsStudents.Tables[I_STUDENTDETAILS];
        iLoginId =Convert.ToInt32(odsStudents.Tables[I_LOGINID].Rows[0][0]);        
        foreach (DataRow oDataRow in odtStudents.Rows)
        {
            iLoginId = iLoginId + 1;
            XmlNode oXmlRootNode = oDoc.CreateNode("element", "Student", string.Empty);

            string sAtrrName = "Student_id";
            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = Convert.ToString(oDataRow["SchoolWise_Student_Id"]);
            oXmlRootNode.Attributes.Append(attr);

            sAtrrName = "User_id";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = Convert.ToString(oDataRow["ID"]);
            oXmlRootNode.Attributes.Append(attr);

            string sLogInID = Convert.ToString(iLoginId);
            sAtrrName = "Login_id";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = sLogInID;
            oXmlRootNode.Attributes.Append(attr);

            sAtrrName = "Password";
            attr = oDoc.CreateAttribute(sAtrrName);
            attr.Value = Utility.CommonUtility.GetEncryptedPassword(sLogInID, oRandom.Next(100000, 999999).ToString());
            oXmlRootNode.Attributes.Append(attr);

            root.AppendChild(oXmlRootNode);

        }
        return root.OuterXml;
    }
    /// <summary>
    /// this method for  for mouseover effect
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnGenerate, btnSMS, btnSMSAdminstaff, btnSMSTeacher,btnActivateLogin });
        if (ConfigurationManager.AppSettings["EnableStudentLogin"].IsNullOrEmpty() || ConfigurationManager.AppSettings["EnableStudentLogin"].Trim() == Constants.S_YES)
            btnActivateLogin.Enabled = false;
        btnGenerate.Attributes.Add("onclick", "if(!ConfirmGenerate()) {return false;}");
    }
    /// <summary>
    /// this  methood send sms to user
    /// </summary>
    /// <param name="aiUserRole"></param>
    private void SendSMSToUsers(int aiUserRole)
    {
        lblGenerate.Text = String.Empty;
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.ForgotPasswordDetailSMS);
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        string sSmsSubject = string.Empty;
        int iSMSType = 0;
        DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId,miSchoolId);
        if (oDTSmsTemplate.Rows.Count != Constants.I_ZERO)
        {
            if (oDTSmsTemplate.Rows[0][I_SMS_TEMPLATE] != DBNull.Value)
            {
                sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][I_SMS_TEMPLATE]);

                if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                    sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();

                sSmsSubject = Convert.ToString(oDTSmsTemplate.Rows[0][I_SMS_TEMPLATE_NAME]);
            }
            if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
        }
        List<UserSMS> lstUserLoginDetails = SchoolUserCollectionBL.GetUserLoginDetails(miSchoolId, miAcademicYearId, aiUserRole);
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        foreach (UserSMS user in lstUserLoginDetails)
        {
            string sUserLogin = user.UserLogin;
            string sUserPassword = CommonUtility.GetDecryptedPassword(sUserLogin, user.UserPassword);  
            string sLoginSmsText = sLoginDetailsSmsText.Replace("%LOGIN%", sUserLogin).Replace("%PASSWORD%", sUserPassword);
            SMS oSMS = new SMS();
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSText = sLoginSmsText;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + "::" + sSmsSubject;
            oSMS.DisplayText = user.Name.ToString();
            oSMS.SMSType = iSMSType;
            oSMS.SMSTypeId = Constants.SMSTypes.ForgotPasswordDetailSMS.ToInt();
            oSMS.To.Add(user.UserId.ToString(), user.MobileNo);
            oSMS.Send();
        }
        lblUpdate.Text = "SMS sent successfully!!!";
        lblUpdate.ForeColor = System.Drawing.Color.Blue;
    }
    #endregion " Public Methods "    
}
