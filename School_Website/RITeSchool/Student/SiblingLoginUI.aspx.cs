using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Kendo.DynamicLinq;
using StudentEntities;
using Utility;

public partial class SiblingLoginUI : SchoolBase
{
    #region Data Member(s)
    
    public int miStudentId; 

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            miStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);

            if (!IsPostBack)
            {
                base.SetDocType();
            }

            hidClass.Value = Resources.LocalizedResources.ClassName;
            hidStudentName.Value = Resources.LocalizedResources.StudentName;
            hidRegNo.Value = Resources.LocalizedResources.RegNo;
            hidLogin.Value = Resources.LocalizedResources.Login;

            hidUserId.Value = miUserId.ToString();
            if (QueryString["IsFromSiblingScreen"] != null && QueryString["IsFromSiblingScreen"].ToString() == Constants.S_YES)
                hidIsSiblingLogin.Value = Constants.S_YES;
            else
                hidIsSiblingLogin.Value = Constants.S_NO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #region Public Method(s)
   
    [WebMethod]
    public static DataSourceResult GetAllSiblingDetails(int aiSchoolId, int aiAcademicYearId, int aistudentId, int aiUserId, string asIsFromSiblingScreen)
    {
        List<StudentInfo> lstStudents = StudentSiblingDetailsBL.GetSiblingDetailsForLogin(aiSchoolId, aiAcademicYearId, aistudentId, aiUserId, asIsFromSiblingScreen == Constants.S_YES);

        //if (lstStudents.Count == 1)
        //{
        //    UserAuthentication oUserAuthentication = new UserAuthentication(18, lstStudents[0].UserName, string.Empty, string.Empty);
        //    oUserAuthentication.UpdateSession();
        //}

        var result = new DataSourceResult()
        {
            Data = lstStudents,
            Total = lstStudents.Count           
        };

        return result;
    }

    [WebMethod]
    public static string LoginToSelectedStudent(int aiSchoolId, int aiAcademicYearId, int aistudentId, string asUserName, int aiUserId, string asIsFromSiblingScreen)
    {
        UserAuthentication oUserAuthentication = new UserAuthentication(aiSchoolId, asUserName, string.Empty, string.Empty);
        string sUrl = "../Common/ControlPanel.aspx";
        if (oUserAuthentication.Locked)
        {
            sUrl = "1,1";
        }
        else
        {
            if (oUserAuthentication.TermAccepted)
            {
                oUserAuthentication.UpdateSession();
                sUrl = "0," + sUrl;
            }
            else
            {
                List<StudentInfo> lstStudents = StudentSiblingDetailsBL.GetSiblingDetailsForLogin(aiSchoolId, aiAcademicYearId, aistudentId, aiUserId, asIsFromSiblingScreen == Constants.S_YES);
                var oStudentInfo = lstStudents.Where(st => st.UserName == asUserName).FirstOrDefault();

                string sPassword = CommonUtility.GetDecryptedPassword(oStudentInfo.UserName.ToLower(), oStudentInfo.Password);

                string sQuerystring = "login=true&sLogin=" + oStudentInfo.UserName + "&sPassword=" + sPassword + "&iSchoolId=" + aiSchoolId;
                string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);                
                sUrl = "0," + "../../TermsOfUse.aspx?" + sEncrypt;
            }
        }

        return sUrl;
    } 

    #endregion
}