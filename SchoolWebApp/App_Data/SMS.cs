using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Linq;
using BusinessLogic;
using Utility;
using System.Configuration;
using SchoolEntities;

/// <summary>
/// This class is used to send SMS to selected user and sent alert massage to admin.
/// </summary>
public class SMS
{
    #region Class Members

    private string msSenderName;
    private Hashtable moHTUsersMobileNo;
    private Hashtable moHTCDMAMobileNo;
    private Hashtable moHTManualMobileNo;
    private string msSMSText;
    private string msSchoolName = string.Empty;
    private int miSchoolId;
    private int miAcademicYearId;
    private int miSenderId;
    private int miInsertedById;
    private int miSenderRoleID;
    private string msDisplayText = string.Empty;
    private const string RelianceNo = "92";
    private const string TataIndicomNo = "93";
    private const string WLLNo = "9";
    private int miSMSCount=1;
    private List<SMSReceiverDetailsBL> SMSReceiverDetailsBLList = new List<SMSReceiverDetailsBL>();
    private int miSMSType;
    private int miSMSTypeId;
    private bool mbEncodeSpaces;
	private bool mbIsScheduled;
    private bool mbIsUnicodeSMS;
    private DateTime modtScheduleDateTime;
    private bool mbBlockDBLog;
    private string msTemplateRegistrationId;

    #endregion Class Members

    #region Properties 

    /// <summary>
    /// Gets or sets the SMS sender name.
    /// </summary>
    public string Sender
    {
        get
        {
            return msSenderName;
        }
        set
        {
            msSenderName = value;
        }
    }

    public string TemplateRegistrationId
    {
        get { return msTemplateRegistrationId; }
        set { msTemplateRegistrationId = value; }
    }

    /// <summary>
    /// If spaces are to be encoded the Encode Spaces.
    /// </summary>
    public bool EnocodeSpaces
    {
        get
        {
            return mbEncodeSpaces;
        }
        set
        {
            mbEncodeSpaces = value;
        }
    }

    /// <summary>
    /// Gets the address collection that contains the recipients of this SMS.
    /// </summary>
    public Hashtable To
    {
        get
        {
            return moHTUsersMobileNo;
        }
        set
        {
            moHTUsersMobileNo = value;
        }
    }

    /// <summary>
    /// Gets the address collection that contains the recipients of this SMS.
    /// </summary>
    public Hashtable ToManualNumbers
    {
        get
        {
            return moHTManualMobileNo;
        }
        set
        {
            moHTManualMobileNo = value;
        }
    }

    /// <summary>
    /// Gets or sets the SMS text.
    /// </summary>
    public string SMSText
    {
        get
        {
            return msSMSText;
        }
        set
        {
            msSMSText = value;
        }
    }

    /// <summary>
    /// Gets or sets the DisplayText.
    /// </summary>
    public string DisplayText
    {
        get
        {
            return msDisplayText;
        }
        set
        {
            msDisplayText = value;
        }
    }

    /// <summary>
    /// Gets or sets the DisplayText.
    /// </summary>
    public bool BlockDBEntry
    {
        get
        {
            return mbBlockDBLog;
        }
        set
        {
            mbBlockDBLog = value;
        }
    }

    /// <summary>
    /// Gets or sets the SchoolID .
    /// </summary>
    public int SchoolID
    {
        get
        {
            return miSchoolId != 0 ? miSchoolId : Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID]);
        }
        set
        {
            miSchoolId = value;
        }
    }

    /// <summary>
    /// Gets or sets the SMSCount .
    /// </summary>
    public int SMSCount
    {
        get
        {
            return miSMSCount;
        }
        set
        {
            miSMSCount = value;
        }
    }

    /// <summary>
    /// Gets or sets the School Name .
    /// </summary>
    public string School_Name
    {
        get
        {
            return msSchoolName != string.Empty ? msSchoolName : Convert.ToString(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_NAME]);
        }
        set
        {
            msSchoolName = value;
        }
    }

    /// <summary>
    /// Gets or sets the AcademicYearID .
    /// </summary>
    public int AcademicYearID
    {
        get
        {
            return miAcademicYearId != 0 ? miAcademicYearId : Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
        }
        set
        {
            miAcademicYearId = value;
        }
    }

    /// <summary>
    /// Gets or sets the SenderID .
    /// </summary>
    public int SenderID
    {
        get
        {
            return miSenderId != 0 ? miSenderId : Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_ID]);
        }
        set
        {
            miSenderId = value;
        }
    }

    /// <summary>
    /// Gets or sets the SenderRoleID .
    /// </summary>
    public int SenderRoleID
    {
        get
        {
            return miSenderRoleID != 0 ? miSenderRoleID : Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
        }
        set
        {
            miSenderRoleID = value;
        }
    }

    /// <summary>
    /// Gets or sets the InsertedByID .
    /// </summary>
    public int InsertedByID
    {
        get
        {
            return miInsertedById != 0 ? miInsertedById : Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_ID]);
        }
        set
        {
            miInsertedById = value;
        }
    }

    public int SMSType
    {
        get
        {
            return miSMSType;
        }
        set
        {
            miSMSType = value;
        }
    }

    public int SMSTypeId
    {
        get
        {
            return miSMSTypeId;
        }
        set
        {
            miSMSTypeId = value;
        }
    }
	
	public bool IsScheduled
    {
        get { return mbIsScheduled; }
        set { mbIsScheduled = value; }
    }

    public bool IsUnicodeSMS
    {
        get { return mbIsUnicodeSMS; }
        set { mbIsUnicodeSMS = value; }
    }

    public DateTime ScheduledDate
    {
        get { return modtScheduleDateTime; }
        set { modtScheduleDateTime = value; }
    }
    #endregion Properties

    #region Cunstructor

    /// <summary>
    /// Initalise default receiver collection.
    /// </summary>
    public SMS()
    {
        moHTUsersMobileNo = new Hashtable();
        moHTCDMAMobileNo = new Hashtable();
        moHTManualMobileNo = new Hashtable();
    }

    /// <summary>
    /// constructor to initialise sender, receiver and sms text
    /// </summary>
    /// <param name="asSender">Sms sender</param>
    /// <param name="ahtSMSReceiver">sms receiver collection</param>
    /// <param name="asSMSText">Sms text</param>
    public SMS(string asSender, Hashtable ahtSMSReceiver, string asSMSText)
    {
        msSenderName = asSender;
        moHTUsersMobileNo = ahtSMSReceiver;
        moHTCDMAMobileNo = new Hashtable();
        msSMSText = asSMSText;
    }

    #endregion Cunstructor

    #region Private Methods

    /// <summary>
    /// This method is used to send SMS to the receptants
    /// </summary>
    /// <returns>int : number of sms sent.</returns>
    public int Send()
    {   
        BuildReceiverList();
        return SendSMS();
    }

    /// <summary>
    /// This method is used to send Login SMS to the receptants
    /// </summary>
    /// <returns>int : number of sms sent.</returns>
    public int SendLoginSMS(int aiSchoolId,int aiAcademicYearId,int aiUserId)
    {
        BuildReceiverListForLoginSMS(aiSchoolId,aiAcademicYearId,aiUserId);
        return SendSMS();
    }

    /// <summary>
    /// This method is used to send salary sms.
    /// </summary>
    /// <param name="abIsNewUser"></param>
    /// <returns></returns>
    public int Send(bool abIsNewUser)
    {        
        if (abIsNewUser)
            BuildReceiverList();
        return SendSMS();
    }

    /// <summary>
    /// This method is used to send SMS.
    /// </summary>
    /// <returns></returns>
    private int SendSMS()
    {
        SchoolBL oSchoolBL = new SchoolBL(SchoolID);
        BuildManualNosReceiverList();
        FilterCDMAMobileNos(moHTUsersMobileNo);
        FilterCDMAMobileNos(moHTManualMobileNo);
        if (Sender == null)
            msSenderName = oSchoolBL.SMSSenderName;

        string sSMSShootId = string.Empty;
        if (mbIsScheduled.IsNull() || !mbIsScheduled)
        {
            //Send Sms to GSM numbers
            String SmsSendingString = SendSMS(moHTUsersMobileNo, msSenderName, ref sSMSShootId);

            //Send Sms to CDMA numbers
            if (moHTCDMAMobileNo.Count > 0)
                SmsSendingString += "</BR>" + SendSMS(moHTCDMAMobileNo, msSenderName, ref sSMSShootId);

            //Send Sms to CDMA numbers
            if (moHTManualMobileNo.Count > 0)
                SmsSendingString += "</BR>" + SendSMS(moHTManualMobileNo, msSenderName, ref sSMSShootId);

            //Send alert of sms to admin
            //if (SmsSendingString != string.Empty)
            //    SendAlertMailToAdmin("</BR>" + SmsSendingString);
        }

        int iTotalSentSMS = moHTUsersMobileNo.Count + moHTCDMAMobileNo.Count + moHTManualMobileNo.Count;
        //int iTotalSentSMS = InsertSMSDetails();

        if(!this.BlockDBEntry)
            InsertSMSDetails(iTotalSentSMS, sSMSShootId);

        if ((mbIsScheduled.IsNull() || !mbIsScheduled) && !this.BlockDBEntry)
            oSchoolBL.UpdateSchoolSentSMSCount(iTotalSentSMS * SMSCount, AcademicYearID);

        return iTotalSentSMS;
    }

    /// <summary>
    /// This method is used to send SMS to the receptants and returns sms sending Url
    /// </summary>
    private String SendSMS(Hashtable ahtSmsReceiver, string asSender, ref string asSMSShootId)
    {
        SMSMasterBL oSMSForSoftSMSMasterBL = new SMSMasterBL();
        string sSMSProvider = oSMSForSoftSMSMasterBL.GetSMSProviderForWebsite(Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID]), Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]));
        
        if (sSMSProvider.ToLower() == Constants.SMSProviders.BusinessSMS.ToString().ToLower())
        {
            int iCount = 1;

            msSMSText = StringUtility.UpdateSMSText(msSMSText);

            String sSMSSenderString = String.Format("{0}/?ID={1}&Pwd={2}", ConfigurationManager.AppSettings["SMSSenderIP"], ConfigurationManager.AppSettings["SMSSenderUName"], ConfigurationManager.AppSettings["SMSSenderUPwd"]);
            String sSMSstr = sSMSSenderString + "&PhNo=";
            String SAllSMSUrl = string.Empty;
            Hashtable OHashtable = GetUniqueMobileNos(ahtSmsReceiver);
            foreach (DictionaryEntry oDE in OHashtable)
            {
                sSMSstr += "91" + oDE.Value + ",";
                if ((iCount % 50) == 0 || iCount == (OHashtable.Count))
                {
                    sSMSstr = sSMSstr.Substring(0, sSMSstr.Length - 1);
                    sSMSstr += String.Format("&Text={0}", StringUtility.DoHTMLEncoding(msSMSText, mbEncodeSpaces));
                    SAllSMSUrl += sSMSstr;

                    if (miSMSType != 0)
                        sSMSstr += "&SMSType=" + miSMSType;

                    if (!string.IsNullOrEmpty(msTemplateRegistrationId))
                        sSMSstr = sSMSstr + "&TemplateId=" + msTemplateRegistrationId;

                    if (ConfigurationManager.AppSettings["SendSMS"].Equals(Constants.C_YES.ToString()))
                        SendSMSRequest(sSMSstr,false);
                    sSMSstr = sSMSSenderString + "&PhNo=";
                }
                iCount++;
            }
            return SAllSMSUrl;
        }
        else if (sSMSProvider.ToLower() == Constants.SMSProviders.SoftSMS.ToString().ToLower())
        {
            int iCount = 1;

            string sSMSType = (mbIsUnicodeSMS ? "unicode" : "long");
            
            msSMSText = StringUtility.UpdateSMSText(msSMSText);            
            string sSMSSenderString = ConfigurationManager.AppSettings["SMSSenderIPForSoftSMS"].ToString() + "/smsapi/index.php?key=" + ConfigurationManager.AppSettings["SMSSenderUPwdForSoftSMS"].ToString() + "&type=" + sSMSType;

            string sSMSstr = sSMSSenderString + "&contacts=";
            string SAllSMSUrl = string.Empty;
            Hashtable OHashtable = GetUniqueMobileNos(ahtSmsReceiver);
            foreach (DictionaryEntry oDE in OHashtable)
            {   
                sSMSstr += oDE.Value + ",";
                if ((iCount % 50) == 0 || iCount == (OHashtable.Count))
                {
                    sSMSstr = sSMSstr.Substring(0, sSMSstr.Length - 1);                    
                    sSMSstr += String.Format("&senderid=" + ConfigurationManager.AppSettings["SMSSenderUNameForSoftSMS"].ToString() + "&msg={0}", StringUtility.DoHTMLEncoding(msSMSText, mbEncodeSpaces));
                    SAllSMSUrl += sSMSstr;

                    if (ConfigurationManager.AppSettings["SendSMS"].Equals(Constants.C_YES.ToString()))
                        asSMSShootId = SendSMSRequest(sSMSstr,true);
                    sSMSstr = sSMSSenderString + "&contacts=";
                }
                iCount++;
            }
            return SAllSMSUrl;
        }
        return string.Empty;
    }

    /// <summary>
    /// This method is used to insert sms Details into database by constructing a sms master objects.
    /// </summary>
    private void InsertSMSDetails(int aiNoOfUsers, string asSMSShootId)
    {
        //This function is used to insert the Message details.
        SMSMasterBL moSMSMasterBL = new SMSMasterBL();
        moSMSMasterBL.SMS_Text = HttpUtility.HtmlDecode(msSMSText);
        moSMSMasterBL.SMS_Text = moSMSMasterBL.SMS_Text.Replace("\\n", "\n");
        //int iNoOfUsers = moHTUsersMobileNo.Count + moHTCDMAMobileNo.Count + moHTManualMobileNo.Count;
        moSMSMasterBL.SMS_Count = aiNoOfUsers * SMSCount;
        moSMSMasterBL.Sender_Name = msSenderName;
        moSMSMasterBL.SchoolId = SchoolID;
        moSMSMasterBL.AcademicYearId = AcademicYearID;
        moSMSMasterBL.Display_Text = msDisplayText;
        moSMSMasterBL.Inserted_By_Id = InsertedByID;
        moSMSMasterBL.Is_Deleted = "N";
        moSMSMasterBL.Is_DeletedFromUser = "N";
        moSMSMasterBL.Sender_User_Id = SenderID;
		if (mbIsScheduled.IsNull() || !mbIsScheduled)
        {
            moSMSMasterBL.Insert_Date = DateTime.Now;
            moSMSMasterBL.IsScheduled = false;
        }
        else
        {
            moSMSMasterBL.Insert_Date = ScheduledDate;
            moSMSMasterBL.IsScheduled = true;
        }
        moSMSMasterBL.Sender_User_Role_Id = SenderRoleID;
        moSMSMasterBL.Updated_By_Id = InsertedByID;
        moSMSMasterBL.SMS_Type_Id = miSMSTypeId;
        moSMSMasterBL.SMSShootId = asSMSShootId;
        moSMSMasterBL.InsertSMSMaster(SMSReceiverDetailsBLList);
        //return iNoOfUsers;
    }

    /// <summary>
    /// Get unique mobile numbers from collection
    /// </summary>
    /// <param name="oHTUsersMobileNo"></param>
    /// <returns></returns>
    private Hashtable GetUniqueMobileNos(Hashtable oHTUsersMobileNo)
    {
        Hashtable oHTMobileNo = new Hashtable();
        foreach (DictionaryEntry oDE in oHTUsersMobileNo)
        {
            oHTMobileNo[oDE.Value] = oDE.Value;
        }
        return oHTMobileNo;
    }

    /// <summary>
    /// This method is used to send SMS to the receptants
    /// </summary>
    private string SendSMSRequest(String asURL, bool abIsSoftSMS)
    {
        string SSMSShootId = string.Empty;
        try
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(asURL);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            using (StreamReader reader = new StreamReader(dataStream))
            {
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.


                if (abIsSoftSMS)
                    SSMSShootId = responseFromServer;

                Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.
                reader.Close();
            }
            response.Close();
        }
        catch (WebException ex)
        {
            string message = ex.Message;
            HttpWebResponse response = (HttpWebResponse)ex.Response;
            if (null != response)
            {
                message = response.StatusDescription;
            }
        }
        return SSMSShootId;
    }

    /// <summary>
    /// This method is used to send mail to RIT for tracking SMS service.
    /// </summary>
    private void SendAlertMailToAdmin(string asSmsSendingString)
    {
        if (asSmsSendingString != string.Empty || !asSmsSendingString.Equals("</BR>"))
        {
			string asToMailAddress = ConfigurationManager.AppSettings["EmailAddress"];
			string asFromMailAddress = ConfigurationManager.AppSettings["FromMailAddress"];
            string asSubject = String.Format("SMS Notification :{0}", School_Name);
            string asBodyText = String.Format("This is to notify that sms have sent by school {0}</BR> as a Sender: {1}</BR> to the following numbers using this SMS Url: {2}</BR>", School_Name, msSenderName, asSmsSendingString);
            CommonUtility.SendE_Mail(asToMailAddress, asFromMailAddress, asSubject, asBodyText);
        }
    }

    /// <summary>
    /// This method is used to filter CDMA mobile no. and Fill them into another Hashtable
    /// </summary>
    private void FilterCDMAMobileNos(Hashtable aohtMobileNumbers)
    {
        int iSeries = 0;
        List<CDMANumberDetails> lstTelecomDetails = GetCDMANumbers();
        Hashtable oHashtable = (Hashtable)aohtMobileNumbers.Clone();
        foreach (DictionaryEntry oDr in oHashtable)
        {
            if (oDr.Value.ToString().Length > 4)
            {
                iSeries = Convert.ToInt32(oDr.Value.ToString().Substring(0, 4));
                List<CDMANumberDetails> cdmaNumbers = lstTelecomDetails.Where(range => range.SrartRange <= iSeries && range.EndRange >= iSeries).ToList();
                if (cdmaNumbers.Count > 0)
                {
                    moHTCDMAMobileNo[oDr.Key] = oHashtable[oDr.Key];
                    aohtMobileNumbers.Remove(oDr.Key);
                }
            }
        }
    }

    /// <summary>
    /// This method is used to create
    /// </summary>
    /// <returns></returns>
    private List<CDMANumberDetails> GetCDMANumbers()
    {
        List<CDMANumberDetails> lstTelecomDetails = new List<CDMANumberDetails>();
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9200,EndRange = 9399 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9133,EndRange = 9151 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9153,EndRange = 9153 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9180,EndRange = 9197 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9606,EndRange = 9607 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9682,EndRange = 9684 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7419,EndRange = 7419 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9653,EndRange = 9653 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9667,EndRange = 9667 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9875,EndRange = 9875 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 9877,EndRange = 9877 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 8453,EndRange = 8453 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 8595,EndRange = 8595 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 8766,EndRange = 8766 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 8925,EndRange = 8925 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7411,EndRange = 7411 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7415,EndRange = 7416 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7418,EndRange = 7418 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7428,EndRange = 7429 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7439,EndRange = 7439 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7483,EndRange = 7483 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7488,EndRange = 7489 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7498,EndRange = 7499 });
        lstTelecomDetails.Add(new CDMANumberDetails  { SrartRange = 7520,EndRange = 7520 });
        lstTelecomDetails.Add(new CDMANumberDetails { SrartRange = 7814, EndRange = 7814 });
        return lstTelecomDetails;
    }

    /// <summary>
    /// Build collection of sms receiver list for selectred used moile numbers.
    /// </summary>
    private void BuildReceiverList()
    {
        foreach (DictionaryEntry oDE in moHTUsersMobileNo)
        {
            int iUserid = Convert.ToInt32(oDE.Key.ToString().Replace("sm;", ""));
            SchoolUserBL oSchoolUserBL = new SchoolUserBL(iUserid);
            PrepareSMSReceiverDetailsBL(iUserid, oSchoolUserBL.UserRoleId, Convert.ToString(oDE.Value));
        }
    }

    /// <summary>
    /// Build collection of sms receiver list for selectred used mobile numbers for Login SMS.
    /// </summary>
    private void BuildReceiverListForLoginSMS(int aiSchoolId,int aiAcademicYearId, int aiUserId)
    {
        foreach (DictionaryEntry oDE in moHTUsersMobileNo)
        {
            int iUserid = Convert.ToInt32(oDE.Key.ToString().Replace("sm;", ""));
            SchoolUserBL oSchoolUserBL = new SchoolUserBL();
            UserDetailsForLoginSMS oUserDetailsForLoginSMS = oSchoolUserBL.GetUserDetailsForLogin(aiSchoolId, aiAcademicYearId, aiUserId);
            PrepareSMSReceiverDetailsBL(iUserid, oUserDetailsForLoginSMS.UserRoleId, Convert.ToString(oDE.Value));
        }
    }


    /// <summary>
    /// Build collection of sms receiver list for manual numbers added.
    /// </summary>
    private void BuildManualNosReceiverList()
    {
        foreach (DictionaryEntry oDE in moHTManualMobileNo)
            PrepareSMSReceiverDetailsBL(0, 0, Convert.ToString(oDE.Value));
    }

    /// <summary>
    /// Build Sms receiver class instance and add it to arraylist
    /// </summary>
    /// <param name="iReceiver_User_Id"></param>
    /// <param name="iReceiver_User_Role_Id"></param>
    /// <param name="sMobile_No"></param>
    private void PrepareSMSReceiverDetailsBL(int iReceiver_User_Id, int iReceiver_User_Role_Id, string sMobile_No)
    {
        SMSReceiverDetailsBL oSMSReceiverDetailsBL = new SMSReceiverDetailsBL();
        oSMSReceiverDetailsBL.Receiver_User_Id = Convert.ToInt32(iReceiver_User_Id);
        oSMSReceiverDetailsBL.Receiver_User_Role_Id = iReceiver_User_Role_Id;
        oSMSReceiverDetailsBL.Updated_By_Id = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_ID]);
        oSMSReceiverDetailsBL.Inserted_By_Id = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_ID]);
        oSMSReceiverDetailsBL.Mobile_No = Convert.ToString(sMobile_No);
        oSMSReceiverDetailsBL.Is_Deleted = "N";
        oSMSReceiverDetailsBL.SMS_Type_Id = miSMSTypeId;
        SMSReceiverDetailsBLList.Add(oSMSReceiverDetailsBL);
    }

    #endregion Private Methods
}

public class CDMANumberDetails
{
    public int SrartRange { get; set; }
    public int EndRange { get; set; }    
}