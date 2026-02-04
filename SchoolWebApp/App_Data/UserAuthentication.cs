/*
modified date:-5 Oct 2011
 */

using System;
using System.Data;
using System.Reflection;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using AccountsEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolBusinessService;
using SuperAdminEntities;
using Utility;
using Newtonsoft.Json;

/// <summary>
/// This class is used to authenticate user.
/// </summary>
public class UserAuthentication
{
    
	#region -- MEMBER(s) --
	
	private string msLogin;
    private string msPassword;
    private int miSchoolId = 0;
    private string msIPAddress;
    private DataSet moDataSet;
    private bool bIsValid;
    private Char cIsLocked = 'N';
    private Char cTermAccepted = 'N';
	private bool bChangePassword;							 
    public int iUserId;
    private Constants.UserRoles oLoginRole;
    private string sBetaURL;

    #endregion -- MEMBER(s) --
	
	#region -- CONSTRUCTOR(s) --
	
	/// <summary>
    /// Constructor to initialize authentication object and initialize session with them.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <param name="asLogin"></param>
    /// <param name="asPassword"></param>
    /// <param name="asIPAddress"></param>
    public UserAuthentication(int aiSchoolId, string asLogin, string asPassword, string asIPAddress)
    {
        msLogin = asLogin;
        msPassword = asPassword;
        miSchoolId = aiSchoolId;
        msIPAddress = asIPAddress;
        CheckIsUserValid();
    }

	/// <summary>
	/// Constructor to initialize authentication object and initialize session with them.
	/// </summary>
	/// <param name="aiSchoolId"></param>
	/// <param name="asLogin"></param>
	public UserAuthentication(int aiSchoolId, string asLogin)
    {
        msLogin = asLogin;
        miSchoolId = aiSchoolId;
    }

	/// <summary>
	/// Constructor to initialize authentication object and initialize session with them.
	/// </summary>
	/// <param name="aiSchoolId"></param>
	/// <param name="asLogin"></param>
	/// <param name="asPassword"></param>
	public UserAuthentication(int aiSchoolId, string asLogin, string asPassword)
    {
        msLogin = asLogin;
        msPassword = asPassword;
        miSchoolId = aiSchoolId;
        CheckIsAdmissionUserValid();
    }

	#endregion -- CONSTRUCTOR(s) --

    #region -- PROPERTIES --

    /// <summary>
    /// Current User Id of User begin authenticated.
    /// </summary>
    public int CurrentUserId
    {
        get { return iUserId; }
    }

    /// <summary>
    /// User begin authenticated have term accepted or not.
    /// </summary>
    public Boolean TermAccepted
    {
        get { return cTermAccepted == Constants.C_YES; }
    }

	public Boolean ChangePassword
    {
        get { return bChangePassword;  }
    }
	
    /// <summary>
    /// User begin authenticated is valid or not.
    /// </summary>
    public Boolean Locked
    {
        get { return cIsLocked == Constants.C_YES; }
    }

    /// <summary>
    /// User begin authenticated is valid or not.
    /// </summary>
    public Boolean ValidUser
    {
        get { return bIsValid; }
    }

    public Constants.UserRoles LoginRole
    {
        get { return oLoginRole; }
    }

    public string BetaURL
    {
        get { return sBetaURL; }
    }

	/// <summary>
	/// Determines if the Accounts module is enabled for the School.
	/// </summary>
	private bool IsAccountsModuleEnabled
	{
		get { return SchoolBase.Settings.EnableAccountsModule ; }
	}

    #endregion -- PROPERTIES --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// Get academic details of school.
    /// </summary>
    /// <param name="aiSchoolId"></param>
    /// <returns></returns>
    public DataTable GetSchoolAcademicDetails(int aiSchoolId)
    {
        return SchoolBL.GetSchoolAcademicDetails(aiSchoolId);
    }

    /// <summary>
    /// Get superadmin details.
    /// </summary>
    /// <returns></returns>
    public DataTable GetSuperAdmin(bool abIncludeSuperAdmin)
    {
        return SuperAdminBL.GetCompAdmin(msLogin, msPassword, abIncludeSuperAdmin);
    }

    /// <summary>
    /// Update session used throughout application depending upon user logged in.
    /// </summary>
    public void UpdateSessionForSuperAdmin()
    {
        SuperAdminDetails oSuperAdminDetails = SuperAdminBL.GetSuperAdminSessionDetails(miSchoolId, msLogin);
        HttpContext.Current.Session[Constants.S_SESSION_USER_ID] = Convert.ToInt32(oSuperAdminDetails.UserId);
        HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID] = Convert.ToInt32(miSchoolId);
        HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = Convert.ToInt32(oSuperAdminDetails.AcademicYearId);
        HttpContext.Current.Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] = Convert.ToInt32(oSuperAdminDetails.UserId);
        HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] = Convert.ToInt32(oSuperAdminDetails.UserRoleId);
        HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_ID] = Convert.ToInt32(oSuperAdminDetails.FinancialYearId);

		// Set Financial year related details in the session.
		SetFinancialYearDetailsInSession();
    }

    /// <summary>
    /// Update session used throughout application depending upon user logged in.
    /// </summary>
    public void UpdateSession()
    {
        DataRow oDR = moDataSet.Tables[0].Rows[0];
		if (moDataSet.Tables[2].Rows.Count > 0 && moDataSet.Tables[2].Rows[0]["Count"].ToInt() > 1)
		{
			string sLoginDate = moDataSet.Tables[2].Rows[0]["LoginDate"].ToString();
            //HttpContext.Current.Session[Constants.S_SESSION_USER_LAST_LOGIN] = "Last successful login on " + sLoginDate.Substring(0, 11) + " at " + sLoginDate.Substring(12, 8);
            HttpContext.Current.Session[Constants.S_SESSION_USER_LAST_LOGIN] = "Last login - " + sLoginDate;
        }
        else
            HttpContext.Current.Session[Constants.S_SESSION_USER_LAST_LOGIN] = "Welcome to RITeSchool! Thank you for logging into the system.";
        if (moDataSet.Tables[2].Rows.Count > 0)
            HttpContext.Current.Session["LoginCount"] = Convert.ToInt32(moDataSet.Tables[2].Rows[0]["Count"]);
        HttpContext.Current.Session[Constants.S_SESSION_USER_ID] = Convert.ToInt32(oDR["User_Id"].ToString());
        HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID] = Convert.ToInt32(oDR["School_Id"].ToString());
        HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_NAME] = oDR["School_Name"].ToString();
        HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] = (Constants.UserRoles)(oDR["User_Role_Id"]);

        int iAcademicYearId = Convert.ToInt32(oDR["year_id"].ToString());
		HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = iAcademicYearId;
        HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_ID] = Convert.ToInt32(oDR["FinancialYearId"].ToString());

        HttpContext.Current.Session[Constants.S_SESSION_IS_10TH_STD_STUDENT] = (oDR["Is10thStudent"].ToBool() ? Constants.S_ONE : Constants.S_ZERO);
        HttpContext.Current.Session[Constants.S_SESSION_ENABLE_LOGIN_FOR_LEFT_STUDENTS] = (oDR["EnableLoginForLeftStudent"].ToBool() ? Constants.S_ONE : Constants.S_ZERO);
        
		// Set Financial year related details in the session.
		SetFinancialYearDetailsInSession();

        if ((Constants.UserRoles)(oDR["User_Role_Id"]) != Constants.UserRoles.Student)
        {
            HttpContext.Current.Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = oDR["Start_date"].ToString();
            HttpContext.Current.Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = oDR["End_date"].ToString();
        }
        else
        {
            DataTable oDT = SchoolUserBL.GetAcademicYearForUser(Convert.ToInt32(oDR["User_Id"].ToString()),
                                                                Convert.ToInt32(oDR["School_Id"].ToString()),
                                                                iAcademicYearId);

           

            if (oDT.Rows.Count > 0)
            {
                HttpContext.Current.Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = oDT.Rows[0]["StartDate"].ToString();
                HttpContext.Current.Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = oDT.Rows[0]["EndDate"].ToString();

            }
            else
            {
                HttpContext.Current.Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE] = oDR["Start_date"].ToString();
                HttpContext.Current.Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE] = oDR["End_date"].ToString();
            }

        }
        HttpContext.Current.Session[Constants.S_SESSION_USER_TERMS_ACCEPTED] = oDR["TermsAccepted"].ToString();

        switch ((Constants.UserRoles)HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID])
        {
        	case Constants.UserRoles.Admin:
        		HttpContext.Current.Session[Constants.S_SESSION_USER_NAME] = oDR["User_First_Name"].ToString();

                if (oDR["User_login"].ToString() == "sadmin")
                    HttpContext.Current.Session[Constants.S_SESSION_USER_FULLNAME] = "Software Coordinator";

        		break;
        	case Constants.UserRoles.Teacher:
        		{
                    if (oDR["Teacher_Id"] == DBNull.Value)                    
                        throw new System.ApplicationException("You are not authenticated user.");                    
                    else
                    {
                        HttpContext.Current.Session[Constants.S_SESSION_TEACHER_ID] = Convert.ToInt32(oDR["Teacher_Id"].ToString());
                        HttpContext.Current.Session[Constants.S_SESSION_USER_NAME] = oDR["Teacher_First_Name"].ToString();

                        string sIsClassTeacher = "N";
                        int iStdDivId = 0;
                        int iStandard_Id = 0;
                        int iDivision_Id = 0;
                        if (oDR["Is_ClassTeacher"] != DBNull.Value)
                        {
                            sIsClassTeacher = oDR["Is_ClassTeacher"].ToString();
                            iStdDivId = Convert.ToInt32(oDR["TeacherStdDivId"].ToString());
                            iStandard_Id = Convert.ToInt32(oDR["Standard_Id"].ToString());
                            iDivision_Id = Convert.ToInt32(oDR["Division_Id"].ToString());
                        }
                        HttpContext.Current.Session[Constants.S_SESSION_IS_CLASS_TEACHER] = sIsClassTeacher;
                        HttpContext.Current.Session[Constants.S_SESSION_TEACHER_STDDIV_ID] = iStdDivId;
                        HttpContext.Current.Session[Constants.S_SESSION_TEACHER_STANDARD_ID] = iStandard_Id;
                        HttpContext.Current.Session[Constants.S_SESSION_TEACHER_DIVISION_ID] = iDivision_Id;
                        HttpContext.Current.Session[Constants.S_SESSION_IS_MPT_APPLICABLE] = oDR["MPT_Applicable"].ToString();
                        HttpContext.Current.Session[Constants.S_SESSION_IS_ASSEMBLY_APPLICABLE] = oDR["Assembly_Applicable"].ToString();
                        HttpContext.Current.Session[Constants.S_SESSION_IS_STAYBACK_APPLICABLE] = oDR["Stayback_Applicable"].ToString();
                        HttpContext.Current.Session[Constants.S_SESSION_USER_STD_SECTION] = oDR["Section"].ToString();
                        HttpContext.Current.Session[Constants.S_SESSION_ISACADEMICYRAPPLICABLE] = oDR[Constants.S_SESSION_ISACADEMICYRAPPLICABLE].ToString();
                        HttpContext.Current.Session[Constants.S_SESSION_IS_FINANCIALYEAR_APPLICABLE] = Convert.ToBoolean(oDR["IsFinancialYearApplicable"]);
                        HttpContext.Current.Session[Constants.S_SESSION_SCREENACCESS_DATATABLE] = moDataSet.Tables[3];
                    }
        		}
        		break;
        	case Constants.UserRoles.Student:
        		HttpContext.Current.Session[Constants.S_SESSION_STUDENT_ID] = Convert.ToInt32(oDR["Student_Id"].ToString());
        		HttpContext.Current.Session[Constants.S_SESSION_STUDENT_STANDERED_ID] = Convert.ToInt32(oDR["Standard_Id"].ToString());
        		HttpContext.Current.Session[Constants.S_SESSION_STUDENT_DIVISION_ID] = Convert.ToInt32(oDR["Division_Id"].ToString());
        		HttpContext.Current.Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID] = Convert.ToInt32(oDR["SchoolWise_Standard_Division_Id"].ToString());
        		HttpContext.Current.Session[Constants.S_SESSION_USER_NAME] = oDR["First_Name"].ToString();
        		HttpContext.Current.Session[Constants.S_SESSION_IS_STD_PREPRIMARY] = oDR["Is_Std_Preprimary"].ToString();
        		HttpContext.Current.Session[Constants.S_SESSION_USER_STD_SECTION] = oDR["Section"].ToString();
        		HttpContext.Current.Session[Constants.S_SESSION_STUDENT_CLASS_NAME] = oDR["studentClass"].ToString();
        		HttpContext.Current.Session[Constants.S_SESSION_STUDENT_REGISTRATION_NUM] = oDR["user_Login"].ToString();
        		HttpContext.Current.Session[Constants.S_SESSION_SCHOOLWISE_STUDENT_ID] = oDR["schoolwise_student_id"].ToString();

        		if (Convert.ToBoolean(oDR["IsNewStudent"]))
        			HttpContext.Current.Session[Constants.S_SESSION_IS_NEW_ADMISSION] = "True";
        		else
        			HttpContext.Current.Session[Constants.S_SESSION_IS_NEW_ADMISSION] = "False";

                if (Convert.ToBoolean(moDataSet.Tables[4].Rows[0]["HasSibling"]))
                    HttpContext.Current.Session[Constants.S_SESSION_HAS_SIBLING] = "True";
                else
                    HttpContext.Current.Session[Constants.S_SESSION_HAS_SIBLING] = "False";

        		break;
        	case Constants.UserRoles.Supervisor:
        		HttpContext.Current.Session[Constants.S_SESSION_USER_NAME] = oDR["Supervisor_FirstName"].ToString();
        		HttpContext.Current.Session[Constants.S_SESSION_ISACADEMICYRAPPLICABLE] = oDR[Constants.S_SESSION_ISACADEMICYRAPPLICABLE].ToString();
				HttpContext.Current.Session[Constants.S_SESSION_IS_FINANCIALYEAR_APPLICABLE] = Convert.ToBoolean(oDR["IsFinancialYearApplicable"]);
        		HttpContext.Current.Session[Constants.S_SESSION_SCREENACCESS_DATATABLE] = moDataSet.Tables[3];
        		break;
            case Constants.UserRoles.OtherStaff:
                HttpContext.Current.Session[Constants.S_SESSION_USER_NAME] = oDR["Supervisor_FirstName"].ToString();
                HttpContext.Current.Session[Constants.S_SESSION_ISACADEMICYRAPPLICABLE] = Constants.S_NO;
                HttpContext.Current.Session[Constants.S_SESSION_IS_FINANCIALYEAR_APPLICABLE] = false;
                HttpContext.Current.Session[Constants.S_SESSION_SCREENACCESS_DATATABLE] = moDataSet.Tables[3];
                break;
        }

        if ((Constants.UserRoles)HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != Constants.UserRoles.Student)
        {
            if (Convert.ToBoolean(moDataSet.Tables[4].Rows[0]["HasParentStaff"]))
                HttpContext.Current.Session[Constants.S_SESSION_HAS_PARENT_STAFF] = "True";
            else
                HttpContext.Current.Session[Constants.S_SESSION_HAS_PARENT_STAFF] = "False";
        }
        else
        {
            if (Convert.ToBoolean(moDataSet.Tables[5].Rows[0]["HasParentStaff"]))
                HttpContext.Current.Session[Constants.S_SESSION_HAS_PARENT_STAFF] = "True";
            else
                HttpContext.Current.Session[Constants.S_SESSION_HAS_PARENT_STAFF] = "False";
        }

        HttpContext.Current.Session[Constants.S_SESSION_IS_FIRST_LOGIN] = oDR["IsBirthday"] + "$" + oDR["BirthDate"];

        HttpContext.Current.Session["Location"] = SchoolBase.Settings.Location;

        //DataTable oDS;
        HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_MENUS] = moDataSet.Tables[1];
        string sUserRole = oDR["User_Role_Name"].ToString();
        HttpContext.Current.Session[Constants.S_SESSION_SUPERVISOR_ROLE_NAME_FIELD] = sUserRole;
		HttpContext.Current.Session["RITMiniSite"] = SchoolBase.Settings.IsMiniSite;
        FormsAuthenticationTicket(sUserRole);

    }

    /// <summary>
    /// This method is used to set authentication ticket for subsequent request.
    /// </summary>
    /// <param name="asUserRole"></param>
    public void FormsAuthenticationTicket(string asUserRole)
    {
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
        1, // Ticket version
        msLogin, // Username associated with ticket
        DateTime.Now, // Date/time issued
        DateTime.Now.AddMinutes(60), // Date/time to expire
        true, // "true" for a persistent user cookie
        asUserRole, // User-data, in this case the roles
        FormsAuthentication.FormsCookiePath);// Path cookie valid for

        // Encrypt the cookie using the machine key for secure transport
        string hash = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(
           FormsAuthentication.FormsCookieName, // Name of auth cookie
           hash); // Hashed ticket

        // Set the cookie's expiration time to the tickets expiration time
        if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

        // Add the cookie to the list for outgoing response
        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
    /// Check that if user begin authenticated is valid or not.
    /// </summary>
    /// <returns></returns>
    private Boolean CheckIsUserValid()
    {
        if (miSchoolId != 0)
        {
            moDataSet = SchoolUserBL.GetValidUser(miSchoolId, msLogin, msPassword, msIPAddress);
            if (moDataSet != null && moDataSet.Tables.Count > 0 && moDataSet.Tables[0].Rows.Count > 0)
            {
                cIsLocked = Convert.ToChar(moDataSet.Tables[0].Rows[0]["Is_Locked"]);
                cTermAccepted = Convert.ToChar(moDataSet.Tables[0].Rows[0]["TermsAccepted"]);
                iUserId = Convert.ToInt32(moDataSet.Tables[0].Rows[0]["User_Id"]);
                oLoginRole = (Constants.UserRoles)(moDataSet.Tables[0].Rows[0]["User_Role_Id"]);

                if (moDataSet.Tables[0].Columns.Contains("ChangePassword"))
				    bChangePassword = Convert.ToBoolean(moDataSet.Tables[0].Rows[0]["ChangePassword"]);																					

                if (moDataSet.Tables[4].Columns.Contains("BetaURL"))
                {
                    if (moDataSet.Tables[4].Rows[0]["BetaURL"] != DBNull.Value && moDataSet.Tables[4].Rows[0]["BetaURL"].ToString() != string.Empty)
                        sBetaURL = moDataSet.Tables[4].Rows[0]["BetaURL"].ToString() + CommonUtility.EncryptQuerystring("SchoolId=" + miSchoolId + "&UserId=" + moDataSet.Tables[0].Rows[0]["User_Id"]).Replace("+", "%20").Replace("/", "%2F");
                    else
                        sBetaURL = string.Empty;
                }
                else if (moDataSet.Tables[5].Columns.Contains("BetaURL"))
                {
                    if (moDataSet.Tables[5].Rows[0]["BetaURL"] != DBNull.Value && moDataSet.Tables[5].Rows[0]["BetaURL"].ToString() != string.Empty)
                        sBetaURL = moDataSet.Tables[5].Rows[0]["BetaURL"].ToString() + CommonUtility.EncryptQuerystring("SchoolId=" + miSchoolId + "&UserId=" + moDataSet.Tables[0].Rows[0]["User_Id"]).Replace("+", "%20").Replace("/", "%2F");
                    else
                        sBetaURL = string.Empty;
                }
                else
                    sBetaURL = string.Empty;

                bIsValid = true;
            }
            else
                bIsValid = false;
        }
        return bIsValid;
    }

    /// <summary>
    /// Check that if user begin authenticated is valid or not.
    /// This method is mainly defined to check for parent's login for online admission process.
    /// </summary>
    /// <returns></returns>
    private Boolean CheckIsAdmissionUserValid()
    {
        if (miSchoolId != 0)
        {
            moDataSet = new DataSet();
            DataTable oDataTable = StudentAdmissionsBL.CheckValidUser(miSchoolId, msLogin, msPassword);
            moDataSet.Tables.Add(oDataTable);
            if (oDataTable != null && oDataTable.Rows.Count > 0)
                bIsValid = true;
            else
                bIsValid = false;
        }
        return bIsValid;
    }

    /// <summary>
    /// Update session used throughout application depending upon user logged in.
    /// </summary>
    public void UpdateAdmissionLoginSession()
    {
        HttpContext.Current.Session[Constants.S_SESSION_STUDENT_ADMISSION_ID] = Convert.ToInt32(moDataSet.Tables[0].Rows[0]["Student_Admission_Id"].ToString());
        HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] = Convert.ToInt32(moDataSet.Tables[0].Rows[0]["Acedemic_Year_Id"].ToString());
        HttpContext.Current.Session[Constants.S_SESSION_STUDENT_FORM_NUMBER] = Convert.ToString(moDataSet.Tables[0].Rows[0]["Form_Number"]);
        //HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_ID] = Convert.ToInt32(moDataSet.Tables[0].Rows[0]["FinancialYearId"]);
    }

	/// <summary>
	/// Sets the FinancialYearId of the current financial year in session, if the Accounts module is enabled.
	/// </summary>
	private void SetFinancialYearDetailsInSession()
	{
		if (IsAccountsModuleEnabled)
		{
			AccountsBaseClient oAccountsBaseClient = null;
			try
			{
				oAccountsBaseClient = new AccountsBaseClient();
				oAccountsBaseClient.Open();
				
				FinancialYear oFinancialYear = oAccountsBaseClient.GetCurrentFinancialYear(miSchoolId);
				UserPermissions oUserPermissions = oAccountsBaseClient.GetUserPermissions(miSchoolId, HttpContext.Current.Session[Constants.S_SESSION_USER_ID].ToInt());
				
				bool bCanEditOldFinancialYear = !oUserPermissions.IsNull() && oUserPermissions.CanEditOldFinancialYear;

				HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_ID] = oFinancialYear.FinancialYearId;
				//HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_START_DATE] = oFinancialYearMaster.StartDate;
				//HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_END_DATE] = oFinancialYearMaster.EndDate;
				//HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_IS_CURRENT] = oFinancialYearMaster.IsCurrent;
				//HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR_IS_CLOSED] = oFinancialYearMaster.IsClosed;
				HttpContext.Current.Session[Constants.S_SESSION_FINANCIAL_YEAR] = oFinancialYear;
				HttpContext.Current.Session[Constants.S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR] = bCanEditOldFinancialYear;
			}
			catch (Exception ex)
			{
				ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
														  "Accounts Module : An exception occured while fetching current financial year.");
			}
			finally
			{
				if (oAccountsBaseClient != null && oAccountsBaseClient.State != CommunicationState.Faulted)
					oAccountsBaseClient.Close();
			} 
		}
	}

    #endregion -- PRIVATE METHOD(s) --

}

