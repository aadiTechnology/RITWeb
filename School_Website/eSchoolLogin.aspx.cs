/* File Name :- eSchoolLogin.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 17-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to authenticate user details.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Resources;
using SchoolEntities;
using Utility;
using DataCommunicator;
using System.Net.NetworkInformation;
public partial class eSchoolLogin : SchoolBase
{
    private Activation moActivation;

    #region -- CONSTANT(s) --

    private const int I_MAX_EVENTS = 3;

    #endregion -- CONSTANT(s) --

    #region -- EVENT HANDLER(s) --

    /// <summary>
    /// This event is used to set default button property and check whether login is allowed or not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (SchoolBase.Settings.IsMiniSite)
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                string sMACAddress = nics[0].GetPhysicalAddress().ToString();

                moActivation = new Activation();

                string sActiationKey = moActivation.GetActivationKey();

                if (sActiationKey == string.Empty || CommonUtility.DecryptQuerystring(sActiationKey) != sMACAddress)
                    Response.Redirect("~/ValidateKey.aspx");

            }
            Login1.FailureText = "";
            Button oButton = this.FindControl("ctl00$ContentPlaceHolder1$Login1$LoginButton") as Button;
            valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
            if (oButton != null)
            {
                SetDefaultButton(oButton);
				Char cIsLoginAllowd = Convert.ToChar(ConfigurationManager.AppSettings["IsLoginAllowd"]);
                if (cIsLoginAllowd.Equals(Constants.C_NO))
                {
                    string sEncrypt = CommonUtility.EncryptQuerystring("IsErrorMsg=1");
                    oButton.Attributes.Add("onclick", "window.open('./LoginMsgPopUp.aspx?" + sEncrypt
                                  + "' , '_new','scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=200,left=300,width=400,height=60'); return false;");
                }
            }
            TextBox otxtUserName = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$Login1$UserName");
            otxtUserName.Focus();
            GetDataForUpcomingEvents();
            if (!IsPostBack)
            {
				string sUserGuidURL = SchoolBase.Settings.UserGuideUrl;
                features.Attributes.Add("onclick", "OpenWindow('" + sUserGuidURL + "')");
            }
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for authentication.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        try
        {
			Char cIsLoginAllowd = Convert.ToChar(ConfigurationManager.AppSettings["IsLoginAllowd"]);
            if (cIsLoginAllowd.Equals(Constants.C_YES))
            {
                string sLogin = Login1.UserName.Trim();
                string sPassword = Login1.Password;
				int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
                string sIPAddress = Request.UserHostAddress;

                UserAuthentication oUserAuthentication = new UserAuthentication(iSchoolId, sLogin, sPassword, sIPAddress);

                if (oUserAuthentication.ValidUser)
                {
                    if (!oUserAuthentication.Locked)
                    {
                        if (!oUserAuthentication.TermAccepted)
                        {
                            sLogin = sLogin.Replace("&", "%USN%");
                            sPassword = sPassword.Replace("&", "%PWD%");
                            string sQuerystring = "login=true&sLogin=" + sLogin + "&sPassword=" + sPassword + "&iSchoolId=" + iSchoolId;
                            string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
                            System.Web.HttpContext.Current.Response.Redirect("~/TermsOfUse.aspx?" + sEncrypt, false);
                        }
                        else
                        {
                            oUserAuthentication.UpdateSession();
                            SetScreenSizeInSession();                            
                            string  returnUrl = Request.QueryString["ReturnUrl"];
                            if (string.IsNullOrEmpty(returnUrl) || returnUrl.Equals("/") || !returnUrl.Contains(".aspx"))
							{
                                if (sLogin == sPassword)
                                    Response.Redirect("RITeSchool/Common/StudentChangePassword.aspx", false);
                                else
                                    Response.Redirect("RITeSchool/Common/ControlPanel.aspx", false);
                            }
                            else
                                Response.Redirect(returnUrl, false);
                        }
                    }
                    else if (oUserAuthentication.Locked)
                        Login1.FailureText = "Your account is locked. Please contact school administrator.";
                }
				else
				{
					// Check if the user is from the management user role group.
					DataTable oDTSuperAdminDetails = oUserAuthentication.GetSuperAdmin(false);

					if (oDTSuperAdminDetails != null && oDTSuperAdminDetails.Rows.Count > 0)
					{
						DataRow oDR = oDTSuperAdminDetails.Rows[0];
						Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] = oDR["User_Id"].ToInt();
						Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID] = oDR["UserRoleId"].ToInt();
						SetScreenSizeInSession();
						Response.Redirect("RITeSchool/Management/ManagementDashboardUI.aspx?" + CommonUtility.EncryptQuerystring("SuperAdminDetailsId=" + oDR["SuperAdminDetailsId"] + "&UserId=" + oDR["User_Id"]), false);
						UpdateSessionVariable(oDR["Name"].ToString());
					}
					else
						Login1.FailureText = "You are not authenticated user.";
				}
            }
        }
        catch (Exception ex)
        {
            Login1.FailureText = "Web site is under maintenance or upgrade in progress.";

            try
            {
	            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            }
            catch (Exception exception)
            {
                string asCallingFunctionName = MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + MethodBase.GetCurrentMethod().Name;
                string sReplaceSingleQuoteString = asCallingFunctionName + " : " + exception.Message
                                                            + Constants.S_TRACE + exception.StackTrace;
                string sSchoolId = ConfigurationManager.AppSettings["SchoolID"];
                int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);

                ExceptionHandler.NotifyErrorLog(sReplaceSingleQuoteString, iUserId, sSchoolId);
            }
        }
    }

    #endregion -- EVENT HANDLER(s) --

    #region -- PRIVATE METHOD(s) --

    /// <summary>
	///		Sets the client screen size in session.
	/// </summary>
	private void SetScreenSizeInSession()
	{
		int iWidth;
		bool bIsWidth = Int32.TryParse(hidScreenWidth.Value, out iWidth);
		Session.Add(Constants.S_SESSION_SCREEN_WIDTH, bIsWidth ? iWidth : 1024);
	}

	/// <summary>
	///		Updates the Session for management user.
	/// </summary>
	/// <param name="asUserName"></param>
	private void UpdateSessionVariable(string asUserName)
	{
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
		string sLoginName = SuperAdminBL.GetSchoolAdminLoginName(iSchoolId);
		var oUserAuthentication = new UserAuthentication(iSchoolId, sLoginName, string.Empty, String.Empty);
		oUserAuthentication.UpdateSession();
		Session[Constants.S_SESSION_USER_NAME] = asUserName;
	//	InitializeMemberVariables();
	}

   /// <summary>
    ///		Fetches the events that are to be displayed on the home page.
    /// </summary>
    private void GetDataForUpcomingEvents()
    {
        miSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();

        var oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
        var dtAcademicYears = oSchoolWiseAcademicYearMasterBL.GetAllSchoolwiseAcademicYearInfo(miSchoolId);

        DataRow[] dtarrRows = dtAcademicYears.Select("is_current_year = 'Y' and school_id = " + miSchoolId);
        miAcademicYearId = dtarrRows[0]["Academic_Year_ID"].ToInt();

        List<Event> lstEvents = SchoolEventBL.GetAllEvents(miSchoolId, miAcademicYearId, 0, 0);

        if (lstEvents != null && lstEvents.Count > 0)
        {
            lstEvents = lstEvents.OrderBy(e => e.StartDate)
                                 .Where(e => e.Display_On_Homepage && (e.StartDate.Date >= DateTime.Now.Date || e.EndDate.Date >= DateTime.Now.Date))
                                 .GroupBy(e => e.StartDate)
                                 .Select(e => new { e.Key, Events = e.ToList() })
                                 .Take(I_MAX_EVENTS)
                                 .SelectMany(e => e.Events)
                                 .ToList();

            AppendStandardsToEventName(lstEvents);

            SerializeEventData(lstEvents);
        }
        else
        {
            defaultUpcomingEvents.Visible = false;
            defaultupcomingevent.Visible = false;
        }
            
    }

    /// <summary>
    ///		Updates the event names and appends the standards to it.
    /// </summary>
    /// <param name="alstEvents"></param>
    private void AppendStandardsToEventName(List<Event> alstEvents)
    {
        var oStandardCollectionBL = new StandardCollectionBL(miSchoolId, miAcademicYearId);
        DataTable odtStandards = oStandardCollectionBL.GetAllStandards();

        List<string> lstStandards = odtStandards.Select("school_id <> -9999")
                                                .AsEnumerable()
                                                .Select(row => row["standard_name"].ToString())
                                                .ToList();

        alstEvents.ForEach(evt =>
        {
            string sEventStandards = evt.Standards.Replace(", ", ",");
            List<string> lstEventStandards = null;
            if (!sEventStandards.IsNullOrEmpty())
                lstEventStandards = sEventStandards.Split(',').ToList();
            // We check if the Standards applicable for the event are not equal to the total standards in the school.
            // If they are not equal, we append the standard range to the description.
            if (lstStandards.Count > 0 && lstEventStandards != null && lstEventStandards.Count != lstStandards.Count)
                evt.EventDescription += " (" + GetStandardRange(lstEventStandards) + ")";
        });
    }

    /// <summary>
    ///		
    /// </summary>
    /// <param name="lstEventStandards"></param>
    /// <returns></returns>
    private string GetStandardRange(List<string> lstEventStandards)
    {
        string sPrePrimayRange = String.Empty;
        var lstStandards = new List<string>();

        if (lstEventStandards.Contains("Play Group"))
            lstStandards.Add("Play Group");

        if (lstEventStandards.Contains("Nursery"))
            lstStandards.Add("Nursery");

        if (lstEventStandards.Contains("Junior KG"))
            lstStandards.Add("Jr. KG");

        if (lstEventStandards.Contains("Senior KG"))
            lstStandards.Add("Sr. KG");

        lstEventStandards.Remove("Play Group");
        lstEventStandards.Remove("Nursery");
        lstEventStandards.Remove("Junior KG");
        lstEventStandards.Remove("Senior KG");

        if (lstStandards.Count > 0)
            sPrePrimayRange = String.Join(", ", lstStandards.ToArray());

        string sPrimaryRange = Intersperse(NumListToPossiblyDegenerateRanges(lstEventStandards.ConvertAll(Convert.ToInt32)).Select(PrettyRange), ", ");
        

        return String.Format("{0}{1}",
                              sPrePrimayRange.IsNullOrEmpty() ? String.Empty : sPrePrimayRange,
                              sPrimaryRange.IsNullOrEmpty() ? String.Empty : sPrePrimayRange.IsNullOrEmpty() ? "Std. " + sPrimaryRange : ", Std. " + sPrimaryRange);
    }

    /// <summary>
    /// e.g. 1,3,5,6,7,8,9,10,12
    /// becomes
    /// (1,1),(3,3),(5,10),(12,12)
    /// </summary>
    private IEnumerable<Tuple<int, int>> NumListToPossiblyDegenerateRanges(IEnumerable<int> numList)
    {
        Tuple<int, int> currentRange = null;

        foreach (var num in numList)
        {
            if (currentRange == null)
                currentRange = Tuple.Create(num, num);
            else if (currentRange.Item2 == num - 1)
                currentRange = Tuple.Create(currentRange.Item1, num);
            else
            {
                yield return currentRange;
                currentRange = Tuple.Create(num, num);
            }
        }

        if (currentRange != null)
            yield return currentRange;
    }

    /// <summary>
    /// e.g. (1,1) becomes "1"
    /// (1,3) becomes "1-3"
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    private string PrettyRange(Tuple<int, int> range)
    {
        return range.Item1 == range.Item2 ? range.Item1.ToString() : String.Format("{0} - {1}", range.Item1, range.Item2);
    }

    public string Intersperse(IEnumerable<string> items, string interspersand)
    {
        var currentInterspersand = String.Empty;
        var result = new StringBuilder();

        foreach (var item in items)
        {
            result.Append(currentInterspersand);
            result.Append(item);
            currentInterspersand = interspersand;
        }

        return result.ToString();
    }

    /// <summary>
    ///		Replaces numbers in a string with their Roman value.
    /// </summary>
    /// <param name="asNumRange"></param>
    /// <returns></returns>
    private string ConvertNumToRomanDigit(string asNumRange)
    {
        asNumRange = asNumRange.Replace("10", "X");
        asNumRange = asNumRange.Replace("9", "IX");
        asNumRange = asNumRange.Replace("8", "VIII");
        asNumRange = asNumRange.Replace("7", "VII");
        asNumRange = asNumRange.Replace("6", "VI");
        asNumRange = asNumRange.Replace("5", "V");
        asNumRange = asNumRange.Replace("4", "IV");
        asNumRange = asNumRange.Replace("3", "III");
        asNumRange = asNumRange.Replace("2", "II");
        asNumRange = asNumRange.Replace("1", "I");

        return asNumRange;
    }

    /// <summary>
    ///		Serializes the event data in json format so it can be used client side.
    /// </summary>
    /// <param name="alstEvents"></param>
    private void SerializeEventData(List<Event> alstEvents)
    {
        if (alstEvents.Count > 0)
        {
            defaultUpcomingEvents.Visible = false;
            defaultupcomingevent.Visible = true;
        }
        var obj = from evt in alstEvents.AsParallel().AsOrdered()
                  select new
                  {
                      name = evt.EventDescription,
                      date = GetEventDate(evt.StartDate, evt.EndDate)
                  };

        var jsSerializer = new JavaScriptSerializer();
        hidEventData.Value = jsSerializer.Serialize(obj);        
    }

    /// <summary>
    ///		Returns the event date or date range.
    /// </summary>
    /// <param name="adtStartDate"></param>
    /// <param name="adtEndDate"></param>
    /// <returns></returns>
    private string GetEventDate(DateTime adtStartDate, DateTime adtEndDate)
    {
        // When the Event is a single date event.
        if (adtStartDate.Date == adtEndDate.Date)
            return String.Format("{0}{1} {2}", adtStartDate.Day.ToString(), GetDateOrdinal(adtStartDate.Day), adtStartDate.ToString("MMM"));

        // When the event spans more than 1 day(s).
        return String.Format("{0}{1} {2} - {3}{4} {5}",
                              adtStartDate.Day.ToString(),
                              GetDateOrdinal(adtStartDate.Day),
                              adtStartDate.ToString("MMM"),
                              adtEndDate.Day.ToString(),
                              GetDateOrdinal(adtEndDate.Day),
                              adtEndDate.ToString("MMM"));
    }

    /// <summary>
    ///		Returns the DateOrdinal for a pass value.
    /// </summary>
    /// <param name="aiDateValue"></param>
    /// <returns></returns>
    private string GetDateOrdinal(int aiDateValue)
    {
        string sOrdinal;

        switch (aiDateValue)
        {
            case 1:
            case 21:
            case 31:
                sOrdinal = "st";
                break;
            case 2:
            case 22:
                sOrdinal = "nd";
                break;
            case 3:
            case 23:
                sOrdinal = "rd";
                break;
            default:
                sOrdinal = "th";
                break;
        }

        return sOrdinal;
    }

    #endregion -- PRIVATE METHOD(s) --

}
