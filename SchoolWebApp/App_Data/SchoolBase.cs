using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Linq;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using System.Web.Script.Serialization;
using SchoolEntities.Common.Recaptcha;

/// <summary>
///		Base class for all pages in the School application.
///		Defines common methods that can be re-used in the page.
/// </summary>
public class SchoolBase : Page
{
	#region -- MEMBER(s) --

	private static Cache _cache = HttpContext.Current.Cache;
	private static YearwiseSchoolSettings moSettings;
	private static PageRequestLog moPageRequestLog;
        
	protected int miSchoolId;
	protected int miAcademicYearId;
	protected int miFinancialYearId;
	protected int miUserId;
    protected string asUpdatedById;
	protected Constants.UserRoles moUserRole;
    protected Constants.SchoolId moSchool;

	#endregion -- MEMBER(s) --

	#region -- PROPERTIES --

	/// <summary>
	///		Represents the QueryString for the Page in decrypted form.
	/// </summary>
	protected NameValueCollection QueryString { get; private set; }

	/// <summary>
	///		Returns settings of all the academic years in the School.
	/// </summary>
	public static Dictionary<int, YearwiseSchoolSettings> AllSettings
	{
		get
		{
			if (_cache[Constants.S_APP_SCHOOL_SETTINGS] == null)
				InitializeSettings();
			
			return _cache[Constants.S_APP_SCHOOL_SETTINGS] as Dictionary<int, YearwiseSchoolSettings>;
		}
	}

   
	/// <summary>
	///		Returns settings for the current academic year of the School.
	/// </summary>
	public static YearwiseSchoolSettings Settings
	{
		get
		{
			int iAcademicYearId = 0;
		    if(HttpContext.Current != null && HttpContext.Current.Session != null)
			    iAcademicYearId = HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();
		
			if (iAcademicYearId != 0 && iAcademicYearId != moSettings.AcademicYearId)
				moSettings = AllSettings[iAcademicYearId];
			
			return moSettings;
		}
	}

	/// <summary>
	///		Returns the PageRequestLog object from session.
	/// </summary>
	public static PageRequestLog PageRequestLog
	{
		get
		{
			if (moPageRequestLog != null)
				return moPageRequestLog;

			return moPageRequestLog = HttpContext.Current.Session[Constants.S_SESSION_PAGE_REQUEST] as PageRequestLog;
		}
	}

	#endregion -- PROPERTIES --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	///		Used to read the QueryString.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnPreInit(EventArgs e)
	{
		try
		{
			InitializeMemberVariables();
			if (QueryString.IsNull())
				DecryptQuerystring();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(),
													  String.Format("Page: {0}. QueryString: {1}", Request.AppRelativeCurrentExecutionFilePath, Request.QueryString));
		}
		finally
		{
			if (QueryString.IsNull())
				QueryString = new NameValueCollection();
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PUBLIC METHOD(s) --

	/// <summary>
	///		Initalizes School Settings and stored them in the cache.
	/// </summary>
	public static void InitializeSettings()
	{
		int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
		
		// Fetch school settings and store them in the cache.
		var oSchoolBL = new SchoolBL();

		Dictionary<int, YearwiseSchoolSettings> dictAllAcademicYearSettings = oSchoolBL.GetSchoolSettings(iSchoolId);

		_cache.Insert(Constants.S_APP_SCHOOL_SETTINGS,
					  dictAllAcademicYearSettings,
		              new CacheDependency(HttpContext.Current.Server.MapPath(@"~\Cache.txt")),
		              Cache.NoAbsoluteExpiration,
		              Cache.NoSlidingExpiration,
		              CacheItemPriority.NotRemovable, 
		              null);
		
		var oSchoolWiseAcademicYearMasterBL = new SchoolWiseAcademicYearMasterBL();
	    int	iAcademicYearID = oSchoolWiseAcademicYearMasterBL.GetCurrentAcademicYearId(iSchoolId);
		moSettings = dictAllAcademicYearSettings[iAcademicYearID];
	}

	/// <summary>
	///  Initializes session related member variables with values from the session.
	/// </summary>
	public void InitializeMemberVariables()
	{
		if (HttpContext.Current.Session == null)
			return;

        if (Session[Constants.S_SESSION_SCHOOL_ID] != null)
        {
            miSchoolId = Session[Constants.S_SESSION_SCHOOL_ID].ToInt();
            moSchool = ((Constants.SchoolId)miSchoolId);
        }

		if (Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID] != null)
			miAcademicYearId = Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();

		if (Session[Constants.S_SESSION_FINANCIAL_YEAR_ID] != null)
			miFinancialYearId = Session[Constants.S_SESSION_FINANCIAL_YEAR_ID].ToInt();

		if (Session[Constants.S_SESSION_USER_ID] != null)
			miUserId = Session[Constants.S_SESSION_USER_ID].ToInt();

		if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null)
			moUserRole = (Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID];

        if (Session[Constants.S_SESSION_LANGUAGE] != null)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Session[Constants.S_SESSION_LANGUAGE].ToString());
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Session[Constants.S_SESSION_LANGUAGE].ToString());
        }                
	}

    /// <summary>
    /// This method is used to add cell into given row.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asCaption"></param>
    /// <param name="asClass"></param>
    /// <param name="asAlign"></param>
    /// <param name="aiColSpan"></param>
    /// <param name="asStyles"></param>
    /// <param name="aoControl"></param>
    public void AddCell(HtmlTableRow aoHtmlTableRow, string asCaption, string asClass, string asAlign = "Center", int aiColSpan = 1, string asStyles = "", Control aoControl = null, string asCellId="")
    {
        string[] stl;
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = asCaption, Align = asAlign, ColSpan = aiColSpan };
        
        if (asCellId != null)
            oHtmlTableCell.ID = asCellId;

        oHtmlTableCell.Attributes.Add("class", asClass);
        if (aoControl != null)
            oHtmlTableCell.Controls.Add(aoControl);

        oHtmlTableCell.Style.Add("Padding-Left", "5pt");

        if (asStyles != string.Empty)
        {
            string[] sArrStyles = asStyles.Split(';');
            sArrStyles.ToList().ForEach
                (
                    style =>
                    {
                        if (style.Trim() != string.Empty)
                        {
                            stl = style.Split(':');
                            if (stl[0] != string.Empty && stl[1] != string.Empty)
                                oHtmlTableCell.Style.Add(stl[0], stl[1]);
                            stl = null;
                        }
                    });
        }

        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
    }

    public void AddCells(HtmlTableRow aoHtmlTableRow, string asCaption, string asClass, string asAlign = "Center", int aiColSpan = 1, string asStyles = "", List<Control> alstControls = null)
    {
        string[] stl;
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = asCaption, Align = asAlign, ColSpan = aiColSpan };
        oHtmlTableCell.Attributes.Add("class", asClass);
        if (alstControls != null)
        {
            alstControls.ForEach(
                control =>
                {
                    oHtmlTableCell.Controls.Add(control);
                }
                );
        }

        oHtmlTableCell.Style.Add("Padding-Left", "5pt");

        if (asStyles != string.Empty)
        {
            string[] sArrStyles = asStyles.Split(';');
            sArrStyles.ToList().ForEach
                (
                    style =>
                    {
                        if (style.Trim() != string.Empty)
                        {
                            stl = style.Split(':');
                            if (stl[0] != string.Empty && stl[1] != string.Empty)
                                oHtmlTableCell.Style.Add(stl[0], stl[1]);
                            stl = null;
                        }
                    });
        }

        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
    }

    public void AddCellWithMandatoryMark(HtmlTableRow aoHtmlTableRow, string asCaption, string asClass, string asAlign = "Center", int aiColSpan = 1, string asStyles = "", Control aoControl = null, bool abIsMandatory = false)
    {
        string[] stl;
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = asCaption, Align = asAlign, ColSpan = aiColSpan };
        oHtmlTableCell.Attributes.Add("class", asClass);
        if (aoControl != null)
            oHtmlTableCell.Controls.Add(aoControl);

        if (abIsMandatory)
        {
            Label oLabel = new Label { Text = " *", ForeColor = System.Drawing.Color.Red };
            oHtmlTableCell.Controls.Add(oLabel);
        }

        oHtmlTableCell.Style.Add("Padding-Left", "5pt");

        if (asStyles != string.Empty)
        {
            string[] sArrStyles = asStyles.Split(';');
            sArrStyles.ToList().ForEach
                (
                    style =>
                    {
                        if (style.Trim() != string.Empty)
                        {
                            stl = style.Split(':');
                            if (stl[0] != string.Empty && stl[1] != string.Empty)
                                oHtmlTableCell.Style.Add(stl[0], stl[1]);
                            stl = null;
                        }
                    });
        }

        aoHtmlTableRow.Cells.Add(oHtmlTableCell);
    }

	/// <summary>
	///		Sets the mouseover and mouseout effect for specified Buttons.
	/// </summary>
	/// <param name="aolstButtons"></param>
	public void ApplyMouseHoverEffect(List<Button> aolstButtons)
	{
		aolstButtons.ForEach(btn =>
		{
			if (btn.IsNull())
				return;
			btn.Attributes["onmouseover"] = "javascript:fnover('" + btn.ClientID + "',this);";
			btn.Attributes["onmouseout"] = "javascript:fnout('" + btn.ClientID + "',this);";
		});
	}

	/// <summary>
	///		Saves Configuration details in the School Configuration Master table.
	/// </summary>
	public void SaveConfigDetails(int aiOriginalConfigId)
	{
		ConfigurationSchoolMasterBL oConfigurationSchoolMasterBL = PopulateSchoolDeatails(aiOriginalConfigId);
		oConfigurationSchoolMasterBL.InsertConfigurationSchoolMaster();
	}

	/// <summary>
	///		Deletes Configuration details from the School Configuration Master table.
	/// </summary>
	public void DeleteConfigDetails(int aiOriginalConfigId)
	{
		ConfigurationSchoolMasterBL oConfigurationSchoolMasterBL = PopulateSchoolDeatails(aiOriginalConfigId);
		oConfigurationSchoolMasterBL.DeleteConfigurationSchoolMaster();
	}

	/// <summary>
	///		Converts the given object into XML form.
	/// </summary>
	/// <param name="alstGenerateXML"></param>
	/// <returns></returns>
	public string GenerateXml(Object alstGenerateXML)
	{
		var oStrwrtr = new StringWriter();
		new XmlSerializer(alstGenerateXML.GetType()).Serialize(oStrwrtr, alstGenerateXML);
		string sXml = oStrwrtr.ToString();
		return sXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
	}

	/// <summary>
	///		Sets the specified Button as the default button of the Page.
	/// </summary>
	public void SetDefaultButton(Button aoButton)
	{
		var oform = this.Master.FindControl("form1") as HtmlForm;
        if (oform != null && aoButton != null)
            oform.DefaultButton = aoButton.UniqueID;
        else
            oform.DefaultButton = null;

	}

	/// <summary>
	///		Converts file uploaded into a byte array.
	/// </summary>
	/// <param name="aoFileField"></param>
	/// <returns></returns>
	public Byte[] GetByteArrayFromFileField(FileUpload aoFileField)
	{
		// Returns a byte array from the passed file field controls file
		var bytedata = new byte[0];
		if (aoFileField.PostedFile != null && aoFileField.PostedFile.ContentLength != 0)
		{
			int intFileLength = aoFileField.PostedFile.ContentLength;
			bytedata = new byte[intFileLength];
			Stream oStream = aoFileField.PostedFile.InputStream;
			oStream.Read(bytedata, 0, intFileLength);
		}

		return bytedata;
	}

    /// <summary>
    ///		Converts file uploaded into a byte array.
    /// </summary>
    /// <param name="aoFileField"></param>
    /// <returns></returns>
    public Byte[] GetByteArrayFromUploadedFileField(FileUpload aoFileField, int aiMaxWidth, int aiMaxHeight)
    {
        // Returns a byte array from the passed file field controls file
        var bytedata = new byte[0];
        if (aoFileField.PostedFile != null && aoFileField.PostedFile.ContentLength != 0)
        {
            string sFileName = CommonUtility.GetFileNameForRenaming(aoFileField.FileName);
            string sFolderName = Server.MapPath("~") + "\\RITeSchool\\Uploads\\";
            string sSourceFilePath = sFolderName + sFileName;
            string sTargetFilePath = sFolderName + CommonUtility.GetFileNameForRenaming("RIT"+aoFileField.FileName); ;
            aoFileField.SaveAs(sSourceFilePath);
            ShrinkImage(sSourceFilePath, aiMaxWidth, aiMaxHeight, sTargetFilePath);

            bytedata = File.ReadAllBytes(sTargetFilePath);
        }

        return bytedata;
    }

	/// <summary>
	///		Adds a sort image to the given list view as per the specified sort expresssion and sort direction.
	/// </summary>
	public void AddSortImage(ListView alstvwSections, string asSortExpression, string asSortDirection)
	{
		var oHtmlTableHeaderRow = alstvwSections.FindControl("trHeader") as HtmlTableRow;
		if (oHtmlTableHeaderRow != null)
			CommonUtility.AddSortImage(oHtmlTableHeaderRow, asSortExpression, asSortDirection);
	}

	/// <summary>
	///		This method is used to revert sort order.
	/// </summary>
	/// <param name="ahidSortDirection"></param>
	public void RevertSortOrder(HiddenField ahidSortDirection)
	{
		ahidSortDirection.Value = ahidSortDirection.Value == Constants.S_ASCENDING ? Constants.S_DESCENDING : Constants.S_ASCENDING;
	}

	/// <summary>
	/// This method is used to display message.
	/// </summary>
	/// <param name="asMessage"></param>
	/// <param name="abIsError"></param>
	/// <param name="aoHtmlTableCell"></param>
	public virtual void DisplayMessage(string asMessage, bool abIsError, HtmlTableCell aoHtmlTableCell)
	{
		var oLabel = aoHtmlTableCell.FindControl("lblMessage") as Label;
		if (oLabel == null)
			return;
		
		oLabel.Text = asMessage;
		if (abIsError)
		{
			oLabel.ForeColor = Color.Red;
			aoHtmlTableCell.Align = "Left";
			oLabel.Font.Bold = false;
			oLabel.Style.Add("padding-left", "0");
		}
		else
		{
			oLabel.ForeColor = Color.Blue;
			aoHtmlTableCell.Align = "Center";
			oLabel.Font.Bold = true;
		}
	}

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="asMessage"></param>
    /// <param name="abIsError"></param>
    /// <param name="aoHtmlTableCell"></param>
    public virtual void DisplayMessage(string asMessage, bool abIsError, HtmlTableCell aoHtmlTableCell, string asLabelName)
    {
        var oLabel = aoHtmlTableCell.FindControl(asLabelName) as Label;
        if (oLabel == null)
            return;

        oLabel.Text = asMessage;
        if (abIsError)
        {
            oLabel.ForeColor = Color.Red;
            aoHtmlTableCell.Align = "Left";
            oLabel.Font.Bold = false;
            oLabel.Style.Add("padding-left", "0");
        }
        else
        {
            oLabel.ForeColor = Color.Blue;
            aoHtmlTableCell.Align = "Center";
            oLabel.Font.Bold = true;
        }
    }

	/// <summary>
	///		Adds the given message to PageRequestLog, so it can be logged to the database.
	/// </summary>
	/// <param name="asKey">A key to identify the message.</param>
	/// <param name="asValue">The actual message.</param>
	public void AddMessageToRequest(string asKey, string asValue)
	{
		if (PageRequestLog == null)
			return;
	
		PageRequestLog.RequestData.Add(new KeyValuePair<string, string>(asKey, asValue));
	}

	/// <summary>
	///		Sets the DOCTYPE for the page.
	/// </summary>
	public void SetDocType()
	{
		var literal = Page.Master.FindControl("docType") as Literal;
		literal.Text = "<!DOCTYPE HTML>" + Environment.NewLine;
	}

	/// <summary>
	/// This method is used to Remove Image Data session.
	/// </summary>
	public void RemoveSession(string asSessionName)
	{
		if (HttpContext.Current.Session[asSessionName] != null)
			HttpContext.Current.Session.Remove(asSessionName);
	}

    public string DateCultureConversion(string sDate, string sPreviousCulture, string sCurrentCulture)
    {
        if (string.IsNullOrEmpty(sDate))
            return string.Empty;
        return Convert.ToDateTime(sDate, new CultureInfo(sPreviousCulture, false).DateTimeFormat).ToString((new CultureInfo(sCurrentCulture, false).DateTimeFormat)).ToDateTime().ToString("dd-MMM-yyyy");
    }

    // <summary>
    /// This method is used to send sms.
    /// </summary>
    /// <param name="asMessage"></param>
    public void SendSmsToUser(int aiUserId)
    {
        SchoolBL oSchoolBL = new SchoolBL(miSchoolId);
        SchoolUserBL oSchoolUserBL = new SchoolUserBL(aiUserId);
        string sLoginDetailsSmsText = string.Empty;
        string sTemplateRegistrationId = string.Empty;
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.ForgotPasswordDetailSMS);
        int iSMSType = 0;
        DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTSmsTemplate.Rows.Count != 0)
        {
            if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
            {
                sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);
                sLoginDetailsSmsText = sLoginDetailsSmsText.Replace("%LOGIN%", oSchoolUserBL.Login).Replace("%PASSWORD%", oSchoolUserBL.Password);

                if (oDTSmsTemplate.Rows[0]["TemplateRegistrationId"] != DBNull.Value)
                        sTemplateRegistrationId = oDTSmsTemplate.Rows[0]["TemplateRegistrationId"].ToString();
            }
            if (oDTSmsTemplate.Rows[0][3] != DBNull.Value)
                iSMSType = oDTSmsTemplate.Rows[0][3].ToInt();
        }

        DataTable oDataTable = SchoolUserCollectionBL.GetPasswordRecoveryDetails(oSchoolUserBL.UserId, miSchoolId);

        if (oDataTable.Rows.Count > 0)
        {
            SMS oSMS = new SMS();
            oSMS.SchoolID = oSchoolBL.SchoolId;
            oSMS.AcademicYearID = Convert.ToInt32(oDataTable.Rows[0]["Academic_Year_ID"]);
            oSMS.SenderID = Convert.ToInt32(oDataTable.Rows[0]["AdminUserId"]);
            oSMS.SenderRoleID = Convert.ToInt32(Constants.UserRoles.Admin);
            oSMS.InsertedByID = -9999;
            oSMS.Sender = oSchoolBL.SMSSenderName;
            oSMS.SMSText = sLoginDetailsSmsText;
            oSMS.TemplateRegistrationId = sTemplateRegistrationId;
            oSMS.School_Name = oSchoolBL.SchoolName + " :: Forgot Password";
            oSMS.DisplayText = Convert.ToString(oDataTable.Rows[0]["UserName"]);
            oSMS.SMSType = iSMSType;
            oSMS.SMSTypeId = Constants.SMSTypes.ForgotPasswordDetailSMS.ToInt();
            oSMS.To.Add(oSchoolUserBL.UserId, oSchoolUserBL.Mobile_Number);
            if (oSchoolUserBL.Mobile_Number2 != string.Empty)
                oSMS.To.Add(oSchoolUserBL.UserId + "sm;", oSchoolUserBL.Mobile_Number2);

            oSMS.Send();
            oSMS = null;
        }
    }

    /// <summary>
    /// This method is used to Send PushNotification to user here sUserId will be UserId or asStandardDivisionId, sParameterName will be like Notice name, ClassName, Amount  
    /// </summary>
    /// <param name="sId"></param>
    /// <param name="sName"></param>
    public virtual void SendPushNotification(string sUserId, object oParameter = null)
    {

    }

    /// <summary>
    /// THis method is used for update session for login name.
    /// </summary>
    /// <param name="iSchoolId"></param>
    /// <param name="sLoginName"></param>
    public void UpdateSessionVariableAndRedirectToNextPage(int iSchoolId, string sLoginName)
    {
        UserAuthentication oUserAuthentication = new UserAuthentication(iSchoolId, sLoginName, string.Empty, string.Empty);

        //Session cleared due to "Old Academic Record" Link on progress report.
        HttpContext.Current.Session.Remove(Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID);
        oUserAuthentication.UpdateSession();
    }

    public void ShrinkImage(string asSourceFilePath, int aiMaxWidth, int aiMaxHeight, string asTargetFilePath)
    {
        Bitmap image = (Bitmap)System.Drawing.Image.FromFile(asSourceFilePath, true);
        int quality = 1;

        //Get the image's original width and height
        int originalWidth = image.Width;
        int originalHeight = image.Height;

        // To preserve the aspect ratio
        float ratioX = (float)aiMaxWidth / (float)originalWidth;
        float ratioY = (float)aiMaxHeight / (float)originalHeight;
        float ratio = Math.Min(ratioX, ratioY);

        // New width and height based on aspect ratio
        int newWidth = (int)(originalWidth * ratio);
        int newHeight = (int)(originalHeight * ratio);

        // Convert other formats (including CMYK) to RGB.
        Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

        // Draws the image in the specified size with quality mode set to HighQuality
        using (Graphics graphics = Graphics.FromImage(newImage))
        {
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawImage(image, 0, 0, newWidth, newHeight);
        }

        // Get an ImageCodecInfo object that represents the JPEG codec.
        ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Jpeg);

        // Create an Encoder object for the Quality parameter.
        Encoder encoder = Encoder.Quality;

        // Create an EncoderParameters object. 
        EncoderParameters encoderParameters = new EncoderParameters(1);

        // Save the image as a JPEG file with quality level.
        EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
        encoderParameters.Param[0] = encoderParameter;
        newImage.Save(asTargetFilePath, imageCodecInfo, encoderParameters);
    }
    private ImageCodecInfo GetEncoderInfo(ImageFormat format)
    {
        return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
    }

    #region ReCaptcha related Method(s)

    protected string GetCaptcheHeaderData()
    {
        if (miSchoolId == 0)
        {
            string sCaptchsSiteKey = ConfigurationManager.AppSettings["CaptchaSiteKey"];
            if (!string.IsNullOrEmpty(sCaptchsSiteKey))
            {
                string sURL = "https://www.google.com/recaptcha/api.js?render=" + sCaptchsSiteKey;
                Literal scriptTag = new Literal();
                scriptTag.Text = "<script src='" + sURL + "'></script>";
                Page.Header.Controls.Add(scriptTag);
                return sCaptchsSiteKey;
            }
            else
                return string.Empty;
        }
        else
            return string.Empty;
    }

    protected string GetCaptcheHeaderData(PlaceHolder phScripts)
    {
        if (miSchoolId == 0)
        {
            string sCaptchsSiteKey = ConfigurationManager.AppSettings["CaptchaSiteKey"];
            if (!string.IsNullOrEmpty(sCaptchsSiteKey))
            {
                string sURL = "https://www.google.com/recaptcha/api.js?render=" + sCaptchsSiteKey;
                Literal scriptTag = new Literal();
                scriptTag.Text = "<script src='" + sURL + "'></script>";
                phScripts.Controls.Add(scriptTag);
                return sCaptchsSiteKey;
            }
            else
                return string.Empty;
        }
        else
            return string.Empty;
    }

    protected bool ValidateCaptcha()
    {
        bool bIsHuman;
        if (miSchoolId == 0)
        {
            string sToken = Request.Form["g-recaptcha-token"];
            bIsHuman = ValidateGoogleRecaptcha(sToken);
        }
        else
            bIsHuman = true;
        return bIsHuman;
    }

    private bool ValidateGoogleRecaptcha(string token)
    {
        try
        {
            string sCaptchsSecretKey = ConfigurationManager.AppSettings["CaptchaSecretKey"];

            if (!string.IsNullOrEmpty(sCaptchsSecretKey))
            {
                string postData = "secret=" + sCaptchsSecretKey + "&response=" + token;
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                string responseFromServer;
                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseFromServer = reader.ReadToEnd();
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                RecaptchaResponse jsonResponse = js.Deserialize<RecaptchaResponse>(responseFromServer);

                // Google recommends threshold 0.5 or higher
                return jsonResponse.success && jsonResponse.score >= 0.5;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        return false;
    } 

    #endregion

	#endregion -- PUBLIC METHOD(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	///		This method is used to decrypt query string.
	/// </summary>
	/// <returns></returns>
	private void DecryptQuerystring()
	{
		if (!Request.QueryString.ToString().IsNullOrEmpty())
		{
			string sDecryptedQueryString = CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString()));
			QueryString = HttpUtility.ParseQueryString(sDecryptedQueryString);
		}
	}

	/// <summary>
	///		This method is used to initailze configuration details.
	/// </summary>
	/// <param name="aiOriginalConfigId"></param>
	private ConfigurationSchoolMasterBL PopulateSchoolDeatails(int aiOriginalConfigId)
	{
		return new ConfigurationSchoolMasterBL
				{
					OriginalConfigId = aiOriginalConfigId,
					SchoolId		 = miSchoolId,
					AcademicYearId	 = miAcademicYearId,
					IsConfigure		 = Constants.C_YES,
					InsertedById	 = miUserId,
					UpdateById		 = miUserId,
					FinancialYearId  = miFinancialYearId
				};
	}

    protected string BasePath
    {
        get
        {
            if (ConfigurationManager.AppSettings["UploadFolderBasePath"] != null && ConfigurationManager.AppSettings["UploadFolderBasePath"].ToString() != string.Empty)
                return ConfigurationManager.AppSettings["UploadFolderBasePath"].ToString(); // + "\\RITeSchool\\Uploads\\";
            else
                return Server.MapPath("~"); // +"\\RITeSchool\\Uploads\\";
        }
    }

	#endregion -- PRIVATE METHOD(s) --
}