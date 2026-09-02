/*
 * File Name         :- SchoolBL.cs
 * Purpose           :- This Class is used as an interface between the UILayer and DCLayer.
 *                      Actual business logic is present in this class.
 
 */

using System;
using System.Collections;
using DataCommunicator;
using System.Data;
using System.Collections.Generic;
using SchoolEntities;
using System.Reflection;
using System.Linq;
using MasterEntities;
namespace BusinessLogic
{

    public class SchoolBL : BusinessLogicBaseBL
    {
        #region DataMembers & Properties        

        #region DataMembers

        SchoolDC moSchoolDC;
        SchoolUserBL moSchoolUserBL;
        ConfigureMenuBL moDefaultConfigureMenuBL;

        SchoolDC.SchoolInfo moSchoolInfo;

        #endregion DataMembers

        #region Properties

        public Int32 SchoolId
        {
            get
            {
                return moSchoolInfo.SchoolId;
            }
            set
            {
                moSchoolInfo.SchoolId = value;
            }
        }

        public string Pincode
        {
            get
            {
                return moSchoolInfo.Pincode;
            }
            set
            {
                moSchoolInfo.Pincode = value;
            }
        }

        public string FeedbackEmail
        {
            get
            {
                return moSchoolInfo.sFeedbackEmail;
            }
            set
            {
                moSchoolInfo.sFeedbackEmail = value;
            }
        }

        public string CareerEmails
        {
            get
            {
                return moSchoolInfo.sCareerEmails;
            }
            set
            {
                moSchoolInfo.sCareerEmails = value;
            }
        }

        public string ForgotPasswordEmails
        {
            get { return moSchoolInfo.sForgotPasswordEmail; }
            set { moSchoolInfo.sForgotPasswordEmail = value; }
        }

        public string SchoolName
        {
            get
            {
                return moSchoolInfo.SchoolName;
            }
            set
            {
                moSchoolInfo.SchoolName = value;
            }
        }

        public string SMSSenderName
        {
            get
            {
                return moSchoolInfo.SMSSenderName;
            }
            set
            {
                moSchoolInfo.SMSSenderName = value;
            }
        }

        public string SMSSenderMobileNo
        {
            get
            {
                return moSchoolInfo.SMSSenderMobileNo;
            }
            set
            {
                moSchoolInfo.SMSSenderMobileNo = value;
            }
        }


        public DateTime SubscriptionDate
        {
            get
            {
                return moSchoolInfo.SubscriptionDate;
            }
            set
            {
                moSchoolInfo.SubscriptionDate = value;
            }
        }

        public int AllowedSMSCount
        {
            get
            {
                return moSchoolInfo.AllowedSMSCount;
            }
            set
            {
                moSchoolInfo.AllowedSMSCount = value;
            }
        }

        public string SchoolOrgnName
        {
            get
            {
                return moSchoolInfo.SchoolOrgnName;
            }
            set
            {
                moSchoolInfo.SchoolOrgnName = value;
            }
        }

        public string StateName
        {
            get
            {
                return moSchoolInfo.StateName;
            }
            set
            {
                moSchoolInfo.StateName = value;
            }
        }

        public string RegNo
        {
            get
            {
                return moSchoolInfo.RegNo;
            }
            set
            {
                moSchoolInfo.RegNo = value;
            }
        }

        public string AccountNo
        {
            get
            {
                return moSchoolInfo.sAccountNo;
            }
            set
            {
                moSchoolInfo.sAccountNo = value;
            }
        }

        public string Address
        {
            get
            {
                return moSchoolInfo.Address;
            }
            set
            {
                moSchoolInfo.Address = value;
            }
        }

        public string City
        {
            get
            {
                return moSchoolInfo.City;
            }
            set
            {
                moSchoolInfo.City = value;
            }
        }


        public string PhoneNumber
        {
            get
            {
                return moSchoolInfo.PhoneNo;
            }
            set
            {
                moSchoolInfo.PhoneNo = value;
            }
        }

        public string PhoneNumber2
        {
            get
            {
                return moSchoolInfo.PhoneNo2;
            }
            set
            {
                moSchoolInfo.PhoneNo2 = value;
            }
        }

        public DateTime SchoolSinceDate
        {
            get
            {
                return moSchoolInfo.SchoolSinceDate;
            }
            set
            {
                moSchoolInfo.SchoolSinceDate = value;
            }
        }

        public string InsertedBY
        {
            get
            {
                return moSchoolInfo.InsertedBy;
            }
            set
            {
                moSchoolInfo.InsertedBy = value;
            }
        }

        public string UpdatedBY
        {
            get
            {
                return moSchoolInfo.UpdatedBy;
            }
            set
            {
                moSchoolInfo.UpdatedBy = value;
            }
        }

        public System.DateTime InsertDate
        {
            get
            {
                return moSchoolInfo.mdtInsertDate;
            }
            set
            {
                moSchoolInfo.mdtInsertDate = value;
            }
        }

        public System.DateTime UpdateDate
        {
            get
            {
                return moSchoolInfo.mdtUpdateDate;
            }
            set
            {
                moSchoolInfo.mdtUpdateDate = value;
            }
        }

        public SchoolUserBL SchoolUserInfo
        {
            get
            {
                return moSchoolUserBL;
            }
            set
            {
                moSchoolUserBL = value;
            }
        }

        public ConfigureMenuBL DefaultMenuInfo
        {
            get
            {
                return moDefaultConfigureMenuBL;
            }
            set
            {
                moDefaultConfigureMenuBL = value;
            }
        }

        public string WebSite
        {
            get
            {
                return moSchoolInfo.sWebSite;
            }
            set
            {
                moSchoolInfo.sWebSite = value;
            }
        }

        public string FaxNumber
        {
            get
            {
                return moSchoolInfo.sFaxNumber;
            }
            set
            {
                moSchoolInfo.sFaxNumber = value;
            }
        }

        public string Address1
        {
            get
            {
                return moSchoolInfo.sAddress1;
            }
            set
            {
                moSchoolInfo.sAddress1 = value;
            }
        }

        public string Address2
        {
            get
            {
                return moSchoolInfo.sAddress2;
            }
            set
            {
                moSchoolInfo.sAddress2 = value;
            }
        }

        public string Email
        {
            get
            {
                return moSchoolInfo.sEmail;
            }
            set
            {
                moSchoolInfo.sEmail = value;
            }
        }

        public string LogoPath
        {
            get
            {
                return moSchoolInfo.sLogoPath;
            }
            set
            {
                moSchoolInfo.sLogoPath = value;
            }
        }

        public string SignLogo
        {
            get
            {
                return moSchoolInfo.sSignLogo;
            }
            set
            {
                moSchoolInfo.sSignLogo = value;
            }
        }

        public string ICardLogo
        {
            get
            {
                return moSchoolInfo.sICardLogo;
            }
            set
            {
                moSchoolInfo.sICardLogo = value;
            }
        }

        public string PTRegCertificateNo
        {
            get
            {
                return moSchoolInfo.msPTRegCertificateNo;
            }
            set
            {
                moSchoolInfo.msPTRegCertificateNo = value;
            }
        }

        public string SchoolRecNoPrimary
        {
            get { return moSchoolInfo.SchoolRecNoPrimary; }
            set { moSchoolInfo.SchoolRecNoPrimary = value; }
        }

        public string SchoolRecNoSecondary
        {
            get { return moSchoolInfo.SchoolRecNoSecondary; }
            set { moSchoolInfo.SchoolRecNoSecondary = value; }
        }

        public string IndexNo
        {
            get { return moSchoolInfo.IndexNo; }
            set { moSchoolInfo.IndexNo = value; }
        }

		public int AdminId
		{
			get { return moSchoolInfo.AdminId; }
			set { moSchoolInfo.AdminId = value; }
		}
		public int AdminUeserRoleId
		{
			get { return moSchoolInfo.AdminRoleId; }
			set { moSchoolInfo.AdminRoleId = value; }
		}
		public string PanNo
		{
			get { return moSchoolInfo.PanNo; }
			set { moSchoolInfo.PanNo = value; }
		}

		public string TanNo
		{
			get { return moSchoolInfo.TanNo; }
			set { moSchoolInfo.TanNo = value; }
		}
        public string GSTIN
        {
            get { return moSchoolInfo.GSTIN; }
            set { moSchoolInfo.GSTIN = value; }
        }
        public string UDISENumber
        {
            get { return moSchoolInfo.UDISENumber; }
            set { moSchoolInfo.UDISENumber = value; }
        }
        public string Lattitude 
        {
            get { return moSchoolInfo.Lattitude; }
            set { moSchoolInfo.Lattitude = value; }
        }
        public string Longitude
        {
            get { return moSchoolInfo.Longitude; }
            set { moSchoolInfo.Longitude = value; }
        }


        #endregion Properties

        #endregion

        #region Overloaded Constructor

        public SchoolBL()
        {
            //Default constructor
            moSchoolDC = new SchoolDC();
        }

        public SchoolBL(int aiSchoolId)
        {
            // This Overloaded Constructor is used to View / Edit the Item data.
            moSchoolDC = new SchoolDC(aiSchoolId);
            moSchoolInfo = moSchoolDC.SchoolDetails;
        }

        #endregion Overloaded Constructor

        #region Public Method

        public bool CheckIfSchoolNameExists()
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            return moSchoolDC.CheckIfSchoolNameExists();
        }

        /// <summary>
        /// This method is used to check for dupication of sms sender name 
        /// </summary>
        /// <returns></returns>
        public bool CheckIfSMSSenderNameExists()
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            return moSchoolDC.CheckIfSMSSenderNameExists();
        }

        public Int32 InsertSchoolDetails()
        {
            //This method Insert newly regiester company information alog with
            // Registration payment details and creates default admin user.
            {
                ArrayList oArrayListInsertStatement = new ArrayList();
                moSchoolDC.SchoolDetails = moSchoolInfo;

                oArrayListInsertStatement.Add(SchoolUserInfo.GetInsertSqlStatementForSchoolUser());

                oArrayListInsertStatement.Add(moDefaultConfigureMenuBL.GetInsetStatementForDefaultConfigureMenu());

                return moSchoolDC.InsertSchoolRegistrationDetails(oArrayListInsertStatement);
            }
        }

        /// <summary>
        /// Update School Logo.
        /// </summary>
        /// <param name="ImageBinaryData"></param>
        public void UpdateSchoolLogo(Byte[] ImageBinaryData)
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdateSchoolLogo(ImageBinaryData);
        }

        /// <summary>
        /// Update School Information.
        /// </summary>
        public Int32 UpdateSchoolInformation()
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            return moSchoolDC.UpdateSchoolInformation();
        }

        /// <summary>
        /// Update ICard Image path.
        /// </summary>
        public void UpdateIcardDetails()
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdatePrincipalSignAndIcardDetails();
        }
        /// <summary>
        /// Update Principal Sign Image path.
        /// </summary>
        public void UpdatePrincipalSignDetails()
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdatePrincipalSignAndIcardDetails();
        }

        /// <summary>
        /// Update Principal Sign and ICard Image path.
        /// </summary>
        public void UpdatePrincipalSignAndIcardDetails()
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdatePrincipalSignAndIcardDetails();
        }

        /// <summary>
        /// Update ICard Image.
        /// </summary>
        /// <param name="ImageBinaryData"></param>
        public void UpdateICardLogo(Byte[] ImageBinaryData)
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdateICardLogo(ImageBinaryData);
        }

        /// <summary>
        /// Update Principal Sign Image.
        /// </summary>
        /// <param name="ImageBinaryData"></param>
        public void UpdatePrincipalSignatureLogo(Byte[] ImageBinaryData)
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdatePrincipalSignatureLogo(ImageBinaryData);
        }

        public void DeleteCompanyByComopanyID(int aiSchoolId)
        {
            //This function is used to Delete the Company by Company ID.
            moSchoolDC.DeleteCompanyByComopanyID(aiSchoolId);
        }

        /// <summary>
        /// This method fetch all school details to activate or deactivate
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSchoolForActivation()
        {
            return moSchoolDC.GetAllSchoolForActivation();
        }

        /// <summary>
        /// This method is used to get the current financial Year id
        /// </summary>
        /// <returns></returns>
        public int GetCurrentFinancialYrId(int aiSchoolId)
        {
            return moSchoolDC.GetCurrentFinancialYrId(aiSchoolId);
        }

		public List<StatisticalData> GetAllStatisticalData()
		{
			return moSchoolDC.GetAllStatisticalData();
		}

        #region static methods

        public static DataTable GetSchoolDetailsForSchoolShortName(string asShortName)
        {
            return SchoolDC.GetSchoolDetailsForSchoolShortName(asShortName);
        }
        #endregion
        #endregion

        #region Methods for Active/ Deactivate School

        public void UpdateSchoolActivationFlag(int aiSchoolId)
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdateSchoolActivationFlag(aiSchoolId);
        }

        public void UpdateSchoolDeActivationFlag(int aiSchoolId)
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdateSchoolDeActivationFlag(aiSchoolId);
        }

        /// <summary>
        /// This method is used to set school SMS count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public void UpdateSchoolSMSCount(int iAcademicYrId)
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdateSchoolSMSCount(iAcademicYrId);
        }

        public void GetSignPath(int aiSchoolId)
        {
            moSchoolDC.GetSignPath(aiSchoolId);
            moSchoolInfo = moSchoolDC.SchoolDetails;
        }


        /// <summary>
        /// This method is used to set school SMS count.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public void UpdateSchoolSentSMSCount(int aiSmsSentCount, int iAcademicYrId)
        {
            moSchoolDC.SchoolDetails = moSchoolInfo;
            moSchoolDC.UpdateSchoolSentSMSCount(aiSmsSentCount, iAcademicYrId);
        }
        #endregion


        public static DataTable GetSchoolAcademicDetails(int aiSchoolId)
        {
            return SchoolDC.GetSchoolAcademicDetails(aiSchoolId);
        }

        public static DataSet GetTimeTableDetails(int aiSchoolId, int aiAcademicYearId)
        {
            return SchoolDC.GetTimeTableDetails(aiSchoolId, aiAcademicYearId);
        }

        public static List<PhotoMaster> GetUserBinaryPhoto(int aiUserId, int aiSchoolId, int aiAcademicYearId, int aiPhotoTypeId)
        {
            return SchoolDC.GetUserBinaryPhoto(aiUserId, aiSchoolId, aiAcademicYearId, aiPhotoTypeId);
        }

        public static List<PhotoMaster> GetGuestsBinaryPhoto(int aiGuestId, int aiSchoolId)
        {
            return SchoolDC.GetGuestsBinaryPhoto(aiGuestId, aiSchoolId);
        }

		/// <summary>
		///		Returns the total staff count in the school.
		/// </summary>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public int GetStaffCount(int aiAcademicYearId)
		{
			return moSchoolDC.GetStaffCount(aiAcademicYearId);
		}

        /// <summary>
        ///		Returns mobile user count details
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public string GetMobileUserDetails(int aiSchoolId)
        { 
            return moSchoolDC.GetMobileUserDetails(aiSchoolId);
        }

        /// <summary>
        /// Get Monthwise login details of mobile app as well as School website.
        /// </summary>
        /// <returns></returns>
        public DataTable GetLoginDetailsForFeatureUsage()
        {
            return moSchoolDC.GetLoginDetailsForFeatureUsage();
        }

        /// <summary>
        /// This method gives Mobile numbers for Admin person.
        /// </summary>
        /// <param name="miSchoolId"></param>
        /// <param name="p"></param>
        /// <returns></returns>

        public string GetAdminMobileNo(int miSchoolId, int iUserId)
        {
            string sReturn = moSchoolDC.GetMobileNo(miSchoolId, iUserId);
            return sReturn;
        }
		/// <summary>
		/// This method fetches the list of settings and returns all the settings as a dictionary.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <returns></returns>
		public Dictionary<int, YearwiseSchoolSettings> GetSchoolSettings(int aiSchoolId)
		{
			List<SchoolSettings> lstSettings = moSchoolDC.GetSchoolSettings(aiSchoolId);

			return lstSettings.GroupBy(sc => sc.AcademicYearId)
							  .ToDictionary(grp => grp.Key, grp => PopulateYearwiseSettings(grp.ToList()));
		}

        public List<SchoolSettings> GetSchoolSettings(int aiSchoolId, int aiAcademicYearId)
        {
            List<SchoolSettings> lstSettings = moSchoolDC.GetSchoolSettings(aiSchoolId);
            return lstSettings.Where(st => st.AcademicYearId == aiAcademicYearId).ToList();
        }

        /// <summary>
        ///This method returns all the School Modules.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<SchoolModule> GetAllModuleSetting()
        {
            List<SchoolModule> lstSchoolModule = moSchoolDC.GetAllModuleSetting();
            return lstSchoolModule;
        }
               
        /// <summary>
        /// THis method is used for get school setting value of current academic year according to key name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sKeyName"></param>
        /// <returns></returns>
        public string GetSchoolSettingByName(int aiSchoolId, string sKeyName)
        {
            return moSchoolDC.GetSchoolSettingByName(aiSchoolId, sKeyName);
        }

		/// <summary>
		/// This method returns the all settings for the respective academic year.
		/// </summary>
		/// <param name="alstSchoolSettings"></param>
		/// <returns></returns>
		private YearwiseSchoolSettings PopulateYearwiseSettings(List<SchoolSettings> alstSchoolSettings)
		{
			var oYearwiseSchoolSettings = new YearwiseSchoolSettings();

			PropertyInfo[] oPropertyInfos = oYearwiseSchoolSettings.GetType().GetProperties();
			foreach (PropertyInfo oInfo in oPropertyInfos)
			{
				object oValue = GetSettingValue(oInfo, alstSchoolSettings);
				if (oValue == null)
					continue;

				oYearwiseSchoolSettings.GetType()
				                       .GetProperty(oInfo.Name)
									   .SetValue(oYearwiseSchoolSettings, oValue, null);
			}

			return oYearwiseSchoolSettings;
		}

        /// <summary>
        /// This method is used to get Menu details.
        /// </summary>
        /// <param name="PageName"></param>
        /// <returns></returns>
        public List<CounsellorMenu> GetMenuDetails(string asPageName, int aiSchoolId)
        {
            return moSchoolDC.GetMenuDetails(asPageName,aiSchoolId);
        }

        /// <summary>
        /// This method is used to get Menu details.
        /// </summary>
        /// <param name="PageName"></param>
        /// <returns></returns>
        public List<StudentsCornerMenu> GetStudentsCornerMenuDetails(string asPageName, int aiSchoolId)
        {
            return moSchoolDC.GetStudentsCornerMenuDetails(asPageName, aiSchoolId);
        }

        /// <summary>
        /// This method is used to get NewsLetter NewsLetter Menu details.
        /// </summary>
        /// <param name="PageName"></param>
        /// <returns></returns>
        public List<NewsLetterDetails> GetNewsLetterDetails(int aiParentMenuId, int aiSchoolId,int aiDisplaymonth = 0)
        {
            return moSchoolDC.GetNewsLetterDetails(aiParentMenuId, aiSchoolId,aiDisplaymonth);
        }

		/// <summary>
		/// This method converts the value into appropriate type and return it.
		/// </summary>
		/// <param name="oPropertyInfo"></param>
		/// <param name="alstSchoolSettings"></param>
		/// <returns></returns>
		private object GetSettingValue(PropertyInfo aoPropertyInfo, List<SchoolSettings> alstSchoolSettings)
		{
			if (alstSchoolSettings.Any(s => s.Name == aoPropertyInfo.Name))
			{
				string sValue = alstSchoolSettings.Find(sc => sc.Name == aoPropertyInfo.Name).Value;
				switch (aoPropertyInfo.PropertyType.Name)
				{
					case "String":
						return Convert.ToString(sValue);
					case "Int32":
						return Convert.ToInt32(sValue);
					case "Decimal":
						return Convert.ToDecimal(sValue);
					case "Double":
						return Convert.ToDouble(sValue);
					case "Boolean":
						return Convert.ToBoolean(sValue);
					case "DateTime":
						return Convert.ToDateTime(sValue);
					case "Int16":
						return Convert.ToInt16(sValue);
					default:
						return Convert.ToString(sValue);
				}
			}

			return null;
		}

        /// <summary>
        /// This method is used to return photo details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static List<PhotoMaster> GetStudentsBinaryPhoto(int aiSchoolId, int aiAcademicYearId)
        {
            return SchoolDC.GetStudentsBinaryPhoto(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to save school setting.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiId"></param>
        /// <param name="asValue"></param>
        /// <param name="asName"></param>
        public void SaveSchoolSetting(int aiSchoolId, int aiAcademicYearId, int aiId, string asValue, string asName)
        {
            SchoolDC.SaveSchoolSetting(aiSchoolId, aiAcademicYearId, aiId, asValue, asName);
        }

        public void UpdateModuleDetails(string asModuleId)
        {
            SchoolDC.UpdateModuleDetails(asModuleId);
        }

        public List<MonthMaster> GetAllMonths()
        {
          return moSchoolDC.GetAllMonths();
        }
    }
    public class AcademicYearException : Exception
    {
        private string msMessage = "";

        public override string Message
        {
            get
            {
                return msMessage;
            }
        }

        public AcademicYearException(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }
    }
}
