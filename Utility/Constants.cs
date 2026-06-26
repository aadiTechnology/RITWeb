using System;
using System.Data;

namespace Utility
{
	public class Constants
	{
		#region General Constants

      

		public const string S_TITLE_FOR_PAGE = "Welcome to RITeSchool";
		public const string S_SELECT = "-- Select --";
		public const string S_NOT_ASSIGN = "Remove Assignment";
		public const string S_SELECT_ALL = "-- All --";
		public const string S_ALL = "All";

		public const string S_NOT_SPECIFIED = "Not Specified";
		public const string S_NOT_APPLICATBLE = "Not Applicable";

		public const string S_DEFAULT_CITY = "Pune";
		public const string S_DEFAULT_STATE = "Maharashtra";
		public const string S_DEFAULT_NATIONALITY = "Indian";
		public const string S_TRACE = "Trace :";
        public const string S_UNITS = "Unit(s)";

		public const string S_DEFAULT_GRID_SORT = "DefaultSort";
		public const string S_ASCENDING = "asc";
		public const string S_DESCENDING = "desc";
		public const char C_NO = 'N';
		public const char C_YES = 'Y';
		public const int I_ZERO = 0;
		public const int I_ONE = 1;
		public const int I_TWO = 2;
		public const int I_THREE = 3;
		public const int I_FOUR = 4;
		public const int I_FIVE = 5;
		public const int I_SIX = 6;
		public const int I_SEVEN = 7;
		public const int I_EIGHT = 8;
		public const int I_GRID_PAGE_COUNT = 20;
		public const int I_DEFAULT_MAX_VALUE = 9999;
        public const int I_FILE_SIZE_LIMIT = 81920;////nearly 80 kb
		public const string S_BLANK_GRID_MESSAGE = " No Records Found.";
		public const string S_EMPTY_STRING = "";
		public const string S_ENTIRE_SCHOOL = "Entire School";

        public const string S_DEFAULT_DATE = "1/1/0001";
        public const string S_DEFAULT_DATE1 = "01/01/0001";
        public const string S_DEFAULT_DATE_2 = "01/01/1900";
        public const string S_DEFAULT_DATE_3 = "1/1/1900";
        public const string S_DEFAULT_DATE_4 = "1/1/1900 12:00:00 AM";
        public const string S_DEFAULT_DATE_5 = "01-Jan-1900";
        public const string S_DEFAULT_DATE_6 = "01-Jan-0001";


		public const string S_DEFAUL_SCHOOL_ID = "-9999";

		public const string S_SERVER_CURRENT_DATE_TIME = "SERVER_CURRENT_DATE_TIME";
		public const string S_LAST_INSERTED_P_KEY = "LAST_INSERTED_P_KEY";
		public const string S_LAST_INSERTED_P_KEY2 = "LAST_INSERTED_P__KEY2";
		public const string S_LAST_INSERTED_P_KEY3 = "LAST_INSERTED_P__KEY3";
		public const string S_LAST_INSERTED_P_KEY4 = "LAST_INSERTED_P__KEY4";
		public const string S_LAST_INSERTED_P_KEY5 = "LAST_INSERTED_P__KEY5";
		public const string S_LAST_INSERTED_P_KEY6 = "LAST_INSERTED_P__KEY6";
		public const string S_LAST_INSERTED_P_KEY7 = "LAST_INSERTED_P__KEY7";
		public const string S_LAST_INSERTED_P_KEY8 = "LAST_INSERTED_P__KEY8";
		public const string S_LAST_INSERTED_P_KEY9 = "LAST_INSERTED_P__KEY9";

		public const string S_FF_BROWSER = "Firefox";
		public const string S_TO = " to ";
		public const string S_OUT_OF = " out of ";
		public const string S_RECORDS = " records ";
		public const string S_DEFAULT_ISSUE_BOOK = "N";

		public const string S_NOTICE_BOARD_MESSAGE = "Welcome to ";

		public const string S_IS_CONFIGURED = "Is_Configured";
		public const string S_SESSION_SELECTED_ACADEMIC_YEAR_ID = "Selected_Academic_Year_Id";


		public const string S_COMMAND_UPDATE = "UpdateCommand";
		public const string S_COMMAND_REMOVE = "RemoveCommand";
        public const string S_COMMAND_SELECT = "SelectCommand";
		public const string S_COMMAND_PUBLISH = "PublishCommand";
		public const string S_COMMAND_SORT = "Sort";
		public const string S_EDIT_MODE = "Edit";
		public const string S_SUBMIT = "SUBMIT";
		public const string S_ADD_MODE = "Add";
		public const string S_NEW_MODE = "New";


		public const string S_ZERO = "0";
		public const string S_ONE = "1";
        public const string S_TWO = "2";
        public const string S_THREE = "3";

		public const string S_HEIGHT = "height";
		public const string S_NO = "N";
		public const string S_YES = "Y";

        public const string S_PDC = "PDC";
        public const string S_CHEQUE = "Cheque";
        public const string S_CARD = "Card";
        public const string S_DATE_FORMAT = "dd-MMM-yyyy";
        public const string S_DATE_FORMAT_DD_MMM = "dd-MMM";
        public const string S_DATE_FORMAT_MARATHI = "yyyy-MM-dd";
        public const string S_DATE_FORMAT_TIMESTAMP = "yyyyMMddHHmmssfff";
        public const int I_MAX_FILE_SIZE_LIMIT = 1048576;
		public const string S_HOMEWORK_FOLDER_LOCATION = "../DOWNLOADS/Homework/";
        public const string S_SUPPORT_FOLDER_LOCATION = "\\Uploads\\Support\\";
        public const string S_SUPPORT_FOLDER_LOCATION_URL = "../Uploads/Support/";
        public const string S_DOWNLOADS_FOLDER_RELATIVE_PATH = "~/RITeSchool/DOWNLOADS/";
        public const string S_MINISITE_DEFAULT_PWD = "minirit";

        public const string S_GATEWAY = "Gateway";
        public const string S_TRANSACTION_FROM = "TxnFrom";
        public const string S_TRANSACTION_PAYMENT_METHOD = "PaymentMethod";
        public const string S_TRANSACTION_UNKNOWN_BANK_ID = "-9999";
        public const string S_SUBJECT_REMARK = "SubjectsectionSortOrder";

        public const string S_PRINCIPAL_DESIGNATION_ID = "10";
        public const string S_SUPERVISOR_DESIGNATION_NAME = "Accounts Cum Admin Officer";
        public const string S_DIRECTOR_DESIGNATION_NAME = "Director";
        public const string S_SENIOR_ADMINISTRATIVE_OFFICER = "Senior Administrative Officer";

        public const string S_DEFAULT_PROFILE_PIC_PATH = "../images/empty-profile.jpg";
        public const string S_LEGEND_SCREEN_NAME = "/TeacherInfoUI.aspx,/AnnualEventPlanner.aspx, /StudentPayFeeUI.aspx, /StaffStatusPopUp.aspx,/SalaryDetailsUI.aspx, /SalaryDifferenceUI.aspx, /LecturesPerStandardWeekday.aspx, /AdminProfileUI.aspx, /SmsTemplateUI.aspx, /UserRolewisePhotoUploadUI.aspx";
        public const string S_PAYMENT_SCREENS = "/PayFeeOnline.aspx,/PaymentConfirmationUI.Aspx,/FeeThankYouUI.aspx";
        #endregion

		#region Common Messages

		public const string S_PASSWORD_RELATED_NOTE = "Capitalization Matters! Min 6 characters, Max 15 characters.";
		public const string S_LOGIN_TOOL_TIP = "Min 6 characters, Max 20 characters.";
		public const string S_UNSUCCESSFUL_ACCOUNT_CREATION = "System encounetered error " +
								 "while creating new account. Please try again.";
		public const string S_COMMON_ERROR_MESSAGE = "System encountered error while performing operation(s). Please try again. ";
		public const string S_VALIDATION_SUMMARY_HEADER = "Please fix following error(s):";
		public const string S_DUPLICATE_LOGIN_MSG = "Login name already exists. Please enter another login name.";
		public const string S_DUPLICATE_REG_NO = "Registration number already exists. Please enter another registration number.";
		public const string S_DUPLICATE_FORM_NO = "Form number already exists. Please enter another form number.";
		public const string S_DUPLICATE_ROLL_NO = "This roll number is already assigned to another student. Please enter another roll number.";
		public const string S_DEFAULUT_USER_ROLE = "ADMIN";
		public const string S_DATA_SAVED_SUCCESSFULLY = "Data saved successfully.";
		public const string S_MESSAGE_SENT_SUCCESSFULLY = "Message sent successfully !!!";
		public const string S_ERROR_MSG_FOR_ALL_CONFIGURATION = "Please configure following details for School :";
		public const string S_NONADMIN_PRECONDITION_MSG = "Required details not configured.";
        public const string S_FEES_PENDING_FOR_STUDENT_MSG = "Your school fees are pending. Please pay the dues to view progress report.";
        public const string S_ERROR_OCCURED_MESSAGE = "Error occurred, we are looking into it.";

		#endregion Common Messages

		#region School Page Names

		public const string S_PAGE_TEACHER_UI = "~/RITeSchool/Admin/TeacherUI.aspx";
		public const string S_PAGE_TEACHER_INFO = "~/Admin/TeacherInfoUI.aspx";
		public const string S_PAGE_CONTROL_PANEL = "~/RITeSchool/Common/ControlPanel.aspx";
		public const string S_PAGE_ALL_STUDENTS_LIST = "~/RITeSchool/Teacher/StudentsListUI.aspx";
		public const string S_PAGE_STUDENT_BASIC_DETAILS = "~/RITeSchool/Teacher/StudentUI.aspx";
		public const string S_PAGE_HOME = "Home.aspx";
		public const string S_PAGE_SCHOOL_CONFIG_CONTROL_PANEL = "~/RITeSchool/Admin/SchoolConfigurationControlPanel.aspx";
		public const string S_PAGE_SUBJECT_GROUPS = "~/RITeSchool/Admin/SubjectGroupUI.aspx";
		public const string S_PAGE_SUBJECT_MARK_ASSIGNMENT = "~/RITeSchool/Teacher/StudentMarksAssignment.aspx";
		public const string S_PAGE_SUBJECT_TEST_CONFIGURATION = "../Admin/SubjectTestConfigurationDetails.aspx";
		public const string S_PAGE_COPY_SUBJECT_TEST_CONFIGURATION = "~/RITeSchool/Admin/CopyTestConfigurationPopUp.aspx";
		public const string S_PAGE_SUBJECT_TEST_CONFIGURATION_DISPLAY = "~/Admin/SubjectTestConfigurationUI.aspx";
		public const string S_PAGE_ERROR = "~/Common/Error.aspx";
		public const string S_PAGE_LIBRARY_MANAGEMENT = "~/LibrarianManagement/LibraryManagementUI.aspx";
		public const string S_PAGE_SUPERADMIN = "../SuperAdmin/ScreensUI.aspx";
		public const string S_PAGE_NON_XSEED_GRADE_ASSIGNMENT = "../Xseed/NonXseedGradeAssignmentUI.aspx";
		public const string S_PAGE_XSEED_GRADE_ASSIGNMENT = "../Xseed/StudentXseedGradeAssignmentUI.aspx";
		public const string S_PAGE_SUPERADMIN_DASHBOARD = "~/RITeSchool/SuperAdmin/ScreensUI.aspx";
		public const string S_PAGE_MANAGEMENT_DASHBOARD = "~/RITeSchool/Management/ManagementDashboardUI.aspx";


		#endregion

		#region Constants for Sending Mail after registration

		public const string S_SITE_NAME = "RITeSchool";
		public const string S_EMAIL_ADDRESS_OF_SITE_ADMIN = "admin@riteschool.com";
		public const string S_SITE_PHONE_NO = "+91-20-26980573";
		public const string S_SITE_MOBILE_NO = "+91-9922109397";

		public static string S_FROM_EMAIL_ADDRESS_OF_SITE_ADMIN;
		public static string S_SCHOOLID;

		#endregion

		#region Folder/Image paths

		public const string S_UPLOAD_IMAGE_FOLDER_PATH = "\\images\\logos\\";
		public const string S_UPLOAD_IMAGE_TEMP_FOLDER_PATH = "\\images\\temp\\";
        public const string S_UPLOAD_FAMILY_PHOTO_IMAGE_PATH = "\\DOWNLOADS\\Family Photos\\";
		public const string S_IMAGE_FOLDER_PATH = "~/images";
		public const string S_BACKGROUND_HEADER_IMAGE_PATH = "../images/index_02.gif";
        public const string S_SCHOOL_LOGO_FILE_NAME = "School_Logo.bmp";
        public const string S_SCHOOL_LOGO_FILE_PATH = "/RITeSchool/images/Logos/School_Logo.bmp?version=1.5";
		public const string S_UPLOAD_IMAGE_STATUS_TRUE = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
		public const string S_UPLOAD_IMAGE_STATUS_BLANK_PHOTO = "~/RITeSchool/images/IconGridStudentBlankPh.gif";
        public const string S_IMAGE_GENERATOR_PATH = "~/RITeSchool/Common/ImageProcessor.aspx?";
		public const string S_UPLOAD_LEAVING_CERTIFICATE_FOLDER_PATH = "\\DOWNLOADS\\User Documents\\Leaving Certificate\\";

		public const string S_IMG_FOR_NONE_CONFIGURATION = "~/RITeSchool/images/icoGrid_MarkEntryNotStart.gif";
		public const string S_IMG_FOR_PARTIAL_CONFIGURATION = "~/RITeSchool/images/icoGrid_MarkEntryPartDone.gif";
		public const string S_IMG_FOR_COMPLETE_CONFIGURATION = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
		public const string S_IMG_FOR_SUBMIT_EXAM_MARKS = "~/RITeSchool/images/icoGrid_SubmitExamMarks.gif";
		public const string S_TOOLTIP_ALREADY_SUBMITTED = "Marks already submitted";
		public const string S_TOOLTIP_SUBMITE_DENIED = "Marks cannot be submitted.";
		public const string S_TOOLTIP_NOT_SUBMIT = "Submit Marks To Class Teacher";
		public const string S_TOOLTIP_COMPLETE = "Marks entry Completed";
		public const string S_TOOLTIP_PARTIAL = "Marks entry partially done";
		public const string S_TOOLTIP_NOT_STARTED = "Marks entry not started";
        public const string S_UPLOAD_FORM16_FOLDER_PATH = "\\DOWNLOADS\\User Documents\\FormNo16\\";

		#endregion

		#region Field Names

		public const string S_STATE_ID_FIELD = "State_Id";

		public const string S_ACADEMIC_YEAR_ID_FIELD = "SchoolWise_Academic_Year_Id";
		public const string S_ACADEMIC_YEAR_TITLE_FIELD = "academic_year";

		public const string S_STATE_NAME_FIELD = "State_Name";
		public const string S_SALUTATION_ID_FIELD = "Salutation_Id";
		public const string S_SALUTATION_NAME_FIELD = "Salutation_Name";
		public const string S_QUALIFICATION_ID = "Qualification_Id";
		public const string S_QUALIFICATION = "Qualification_Name";
		public const string S_STANDARD_ID_FIELD = "Standard_Id";
		public const string S_STANDARD_NAME_FIELD = "Standard_Name";
		public const string S_YEAR_ID_FIELD = "year_id";
		public const string S_YEAR_NAME_FIELD = "year_name";
		public const string S_CLASS_ID_FIELD = "class_id";
		public const string S_CLASS_NAME_FIELD = "class_name";
		public const string S_CASTE_ID_FIELD = "Caste_Id";
		public const string S_CASTE_NAME_FIELD = "Caste_Name";
		public const string S_SUB_CASTE_ID_FIELD = "Sub_Caste_Id";
		public const string S_SUB_CASTE_NAME_FIELD = "Sub_Caste_Name";

		public const string S_CATEGORY_ID_FIELD = "Category_Id";
		public const string S_CATEGORY_NAME_FIELD = "Category_Name";
		public const string S_RELIGION_ID_FIELD = "Religion_Id";
		public const string S_RELIGION_NAME_FIELD = "Religion_Name";
		public const string S_DESIGNATION_ID_FIELD = "Teacher_Designation_Id";
		public const string S_DESIGNATION_NAME_FIELD = "Teacher_Designation_Name";
		public const string S_USERROLE_ID_FIELD = "User_Role_Id";
		public const string S_USERROLE_NAME_FIELD = "User_Role_Name";
		public const string S_STANDARD_DIVISION_ID_FIELD = "SchoolWise_Standard_Division_Id";
		public const string S_STANDARD_DIVISION_NAME_FIELD = "StandardDivision";
		public const string S_SUBJECT_ID_FIELD = "Subject_Id";
		public const string S_SUBJECT_NAME_FIELD = "Subject_Name";

		public const string S_TEACHER_ID_FIELD = "Teacher_Id";
		public const string S_TEACHER_NAME_FIELD = "TeacherName";

		public const string S_TEST_ID_FIELD = "schoolwise_test_id";
		public const string S_TEST_NAME_FIELD = "schoolwise_test_name";

		public const string S_ORIGINAL_STANDARD_ID_FIELD = "original_standard_id";

		public const string S_OCUPATION_ID = "Ocupation_Id";
		public const string S_OCUPATION_NAME = "Ocupation_Name";

		public const string S_ORIGINAL_DIVISION_ID_FIELD = "original_division_id";
		public const string S_DIVISION_ID_FIELD = "division_id";
		public const string S_DIVISION_NAME_FIELD = "division_name";

        public const string S_TYPE_INTERNAL_FEE = "InternalFee";

		public const string S_USER_ROLE_ID_FIELD = "User_Role_Id";
		public const string S_USER_ROLE_NAME_FIELD = "User_Role_Name";

		public const string I_PROFESSIONL_TAX = "16";

		public const string S_YOUTUBE_URL = "http://www.youtube.com/";
		public const string S_PRINCIPAL_DESIGNATION = "Principal";
        public const string S_MD_DESIGNATION = "MD";

		#endregion Field Names

		#region Web Config Related

        public static int I_ACTIVITY_LOG_CACHE_COUNT = 0;

		public static string S_IP_ADDRESS_SMTP = "";
		public static string S_PORT_NUMBER_SMTP = "";
		public static string S_STANDARD_DATE_FORMAT = "";
		public static string S_STANDARD_GRID_DATE_FORMAT = "";
		public static string S_STANDARD_GRID_TIME_FORMAT = "";
		public static string S_STANDARD_GRID_DATE_TIME_FORMAT = "";
		public static string S_SEND_SMS = "";
		public static string S_CONNECTION_STRING = "";        
		public static string SENDMAIL = "";
		public static string S_SUPERVISOR_ROLE_NAME = "";
		public const string S_PAGE_REQUEST_SERVICE = "Service";

		public static bool B_ACTIVITY_LOGGING;
        public static bool B_SERVICE_LOGGING_ENABLED;
		public static string MANAGEMENT_TOKEN = String.Empty;

		#endregion Web Config Related

        #region Themes Related

        public static string S_SESSION_SELECTED_THEME = "SelectedThemes";

        #endregion

        #region School Session Constants

        public const string S_SESSION_SCHOOL_ID = "I_SCHOOL_ID";
		public const string S_SESSION_SCHOOL_NAME = "S_SCHOOL_NAME";
		public const string S_SESSION_USER_IMAGE_DATA = "UserImageData";
        public const string S_SESSION_IS_BUTTON_CLOSE = "ButtonClose";
		public const string S_SESSION_USERS_ID = "UserID";
        public static bool B_SESSION_IS_FROM_SIBLING_SCREEN;
		public const string S_SESSION_USER_NAME = "S_USER_NAME";
		public const string S_SESSION_USER_FULLNAME = "S_USER_FULLNAME";
		public const string S_SESSION_TEACHER_SUBJECT_DS = "TeacherSubjectDS";
		public const string S_SESSION_TEACHER_STDDIV_ID = "TeacherStdDivId";
		public const string S_SESSION_TEACHER_STANDARD_ID = "Standard_Id";
		public const string S_SESSION_TEACHER_DIVISION_ID = "Division_Id";
		public const string S_SESSION_LATEFEE_CONFIG = "lateFee_Config";
		public const string S_TEMP_SESSION_CLASS_DS = "classDs";
		public const string S_TEMP_SESSION_DS = "SessionDS";
		public const string S_TEMP_SESSION_TIMETABLE_DS = "TimeTableDs";
		public const string S_SESSION_ISACADEMICYRAPPLICABLE = "IsAcademicYrApplicable";
		public const string S_SESSION_USER_ID = "I_USER_ID";
		public const string S_SESSION_SUPER_ADMIN_USER_ID = "I_SUPER_ADMIN_USER_ID";
        //public const string S_SESSION_IS_SUPERADMIN = "IsSuperAdmin";
		public const string S_SESSION_USER_LOGIN_ROLE_ID = "S_USERLOGIN_ROLE_ID";
		public const string S_SESSION_TEACHER_ID = "S_TEACHER_ID";
		public const string S_SESSION_CURRENT_ACADEMIC_YEAR = "S_CURRENT_ACADEMIC_YEAR_ID";
		public const string S_SESSION_CURRENT_ACADEMIC_YEAR_ID = "S_CURRENT_ACADEMIC_YEAR_ID";
		public const string S_SESSION_ACADEMIC_YEAR_START_DATE = "S_ACADEMIC_YEAR_START_DATE";
		public const string S_SESSION_ACADEMIC_YEAR_END_DATE = "S_ACADEMIC_YEAR_END_DATE";
		public const string S_SESSION_ACADEMIC_YEAR_STATUS = "S_ACADEMIC_YEAR_STATUS";
		public const string S_SESSION_ACADEMIC_YEAR_IS_NEWLYCREATED = "Is_NewlyCreated";
		public const string S_SESSION_IS_FINALYEAR_GENERATED = "Is_FinalYear_Generated";
		public const string S_SESSION_ACADEMIC_YEAR_IS_CLOSED = "S_ACADEMIC_YEAR_CLOSED";
        public const string S_SESSION_IS_SENT_SMS_LIST = "IsSentSMSList";
        public const string S_SESSION_HAS_SIBLING = "HasSibling";
        public const string S_SESSION_HAS_PARENT_STAFF = "HasParentStaff";
        public const string S_SESSION_HAS_USER_RECORD_IN_TABLE = "HasUserRecordInTable";
        public const string S_SESSION_DEMO_COMPANY_NAME = "DemoCompanyName";

		// Financial Year Related
		public const string S_SESSION_FINANCIAL_YEAR = "S_FINANCIAL_YEAR";
		public const string S_SESSION_MIS_FINANCIAL_YEAR = "S_MIS_FINANCIAL_YEAR";
		public const string S_SESSION_FINANCIAL_YEAR_ID = "S_FINANCIAL_YEAR_ID";
		public const string S_SESSION_FINANCIAL_YEAR_START_DATE = "S_FINANCIAL_YEAR_START_DATE";
		public const string S_SESSION_FINANCIAL_YEAR_END_DATE = "S_FINANCIAL_YEAR_END_DATE";
		public const string S_SESSION_FINANCIAL_YEAR_IS_CURRENT = "S_FINANCIAL_YEAR_IS_CURRENT";
		public const string S_SESSION_FINANCIAL_YEAR_IS_CLOSED = "S_FINANCIAL_YEAR_IS_CLOSED";
		public const string S_SESSION_IS_FINANCIALYEAR_APPLICABLE = "S_IS_FINANCIALYEAR_APPLICABLE";
		public const string S_SESSION_CAN_EDIT_OLD_FINANCIAL_YEAR = "S_CAN_EDIT_OLD_FINANCIAL_YEAR";

		// Management Related
		public const string S_SESSION_MANAGEMENT_CLIENT = "S_MANAGEMENT_CLIENT";
		public const string S_SESSION_MANAGEMENT_MISREPORT = "S_MANAGEMENT_MISREPORT";

		public const string S_SESSION_STUDENT_ID = "S_STUDENT_ID";
		public const string S_SESSION_SCHOOLWISE_STUDENT_ID = "SCHOOLWISE_STUDENT_ID";
		public const string S_SESSION_STUDENT_REGISTRATION_NUM = "S_SESSION_STUDENT_REGISTRATION_NUM";
		public const string S_SESSION_STUDENT_DIVISION_ID = "S_STUDENT_DIVISION_ID";
		public const string S_SESSION_STUDENT_DIVISION_NAME = "S_STUDENT_DIVISION_NAME";
		public const string S_SESSION_STUDENT_STANDERED_ID = "S_STUDENT_STANDERED_ID";
		public const string S_SESSION_STUDENT_STANDERED_NAME = "S_STUDENT_STANDERED_NAME";
		public const string S_SESSION_STUDENT_STANDERED_DIVISION_ID = "S_STUDENT_STANDERED_DIVISION_ID";
		public const string S_SESSION_IS_CLASS_TEACHER = "S_IS_CLASS_TEACHER";
		public const string S_SESSION_IS_STD_PREPRIMARY = "Is_Std_Preprimary";
		public const string S_SESSION_IS_MPT_APPLICABLE = "Is_MPT_Applicable";
		public const string S_SESSION_IS_ASSEMBLY_APPLICABLE = "Is_Assembly_Applicable";
		public const string S_SESSION_IS_STAYBACK_APPLICABLE = "Is_Stayback_Applicable";
		public const string S_SESSION_USER_STD_SECTION = "S_USER_STD_SECTION";
		public const string S_SESSION_USER_TERMS_ACCEPTED = "S_USER_TERMS_ACCEPTED";
		public const string S_SESSION_STUDENT_CLASS_NAME = "S_SESSION_STUDENT_CLASS_NAME";
		public const string S_SESSION_IS_FIRST_LOGIN = "S_SESSION_IS_FIRST_LOGIN";
		public const string S_SESSION_SCREEN_WIDTH = "S_SCREEN_WIDTH";

		public const string S_SESSION_SUPERVISOR_ROLE_NAME_FIELD = "S_SESSION_SUPERVISOR_ROLE_NAME_FIELD";


		public const string S_SESSION_SCHOOL_MENUS = "S_SCHOOL_MENUS";
		public const string S_SESSION_USER_LAST_LOGIN = "S_SESSION_USER_LAST_LOGIN";
		public const string S_SESSION_IS_NEW_ADMISSION = "S_SESSION_IS_NEW_ADMISSION";
		public const string S_ORIGINAL = "Original";
		public const string S_UPDATED = "Updated";
		public const string S_DELETED = "Deleted";
		public const string S_ADDED = "Added";
		public const string S_UPDATEDEL = "UpdateDelete";

		public const string S_SESSION_SCREENACCESS_DATATABLE = "S_SESSION_SCREENACCESS_DATATABLE";

		public const string S_SESSION_STUDENT_ADMISSION_ID = "S_STUDENT_ADMISSION_ID";
		public const string S_SESSION_STUDENT_FORM_NUMBER = "S_STUDENT_FORM_NUMBER";
		public const string S_SESSION_SUPERADMIN_ROLE_ID = "SuperadminRoleId";

		public const string S_SESSION_PAGE_REQUEST = "PageRequestLog";
        public const string S_SESSION_IS_LOGIN_FROM_MOBILE = "IS_LOGIN_FROM_MOBILE";
        public const string S_SESSION_MOBILE_PAY_FEE_POSTBACKURL = "POSTBACKURL";

        public const string S_SESSION_IS_10TH_STD_STUDENT = "IS_10TH_STD_STUDENT";
        public const string S_SESSION_ENABLE_LOGIN_FOR_LEFT_STUDENTS = "ENABLE_LOGIN_FOR_LEFT_STUDENTS";

        public const string S_SESSION_PAYMENT_RECORD = "PAYMENT_RECORD";

        public const string S_SESSION_DO_REFRESH_PAGE = "DorefreshPage";
        public const string S_SESSION_SELECTED_YEAR = "SelectedAcademicYearId";

        public const string S_SESSION_ARE_MANDATORY_FIELD_SUBMITTED_BY_STUDENT = "AreMandatoryFieldsSubmittedByStudent";

        #endregion

        #region  Language Related

        public const string S_SESSION_LANGUAGE = "language";
		public static string S_MARATHI_LANGUAGE = "mr";        
        public const  string S_ENGLISH_lANGUAGE = "en";
        public const string S_HASHTABLE_MARATHI="htMarathi";
        public const string S_HASHTABLE_ENGLISH="htEnglish";
        
		#endregion

		#region School Application Constants

		public const string S_APP_SCHOOL_SETTINGS = "APP_SCHOOL_SETTINGS";

		#endregion

		#region Report USP

		public const string S_EXPORT_CHEQUECLEARANCE_USP = "usp_GetChequeClearanceDetails";
		public const string S_EXPORT_CAUTIONMONEY_USP = "usp_GetAllCautionMoneyDetailsForExport";
		public const string S_EXPORT_STUDENT_TERM_1_PROGRESS_REPORT_USP = "USP_StudentTerm1ProgressReport";
		public const string S_EXPORT_STUDENT_TERM_2_PROGRESS_REPORT_USP = "USP_StudentTerm2ProgressReport";
		public const string S_EXPORT_STUDENTPROGRESSREPORT_USP = "USP_StudentwiseProgressReport";
        public const string S_EXPORT_STUDENTPROGRESSREPORT_USPSS = "USP_StudentwiseProgressReportForSS";
        public const string S_EXPORT_STUDENTPROGRESSREPORT_USPFBS = "USP_StudentwiseProgressReportForFBS";
        public const string S_EXPORT_STUDENTPROGRESSREPORT_USPPPSN = "USP_StudentwiseProgressReportForPPSN";
		public const string S_EXPORT_ONLINETRANSACTIONCLEARANCE_USP = "USP_GetOnlineTransactionDetails";
		public const string S_EXPORT_CARDPAYMENTS_USP = "USP_ExportCardPayments";
		public const string S_EXPORTUSP_CLEAREDCASHPAYMENT_USP = "USP_ExportClearedCashPayment";
		public const string S_EXPORT_PENDING_FEE = "usp_PendingFeeStudentList";
        public const string S_EXPORT_ELECTRONICPAYMENTS_USP = "usp_GetElectronicPayments";

		#endregion

		#region enums and structures

        public enum LeaveStatuses
        {
            Submitted = 1,
            InProcess = 2,
            Approved = 3,
            Rejected = 4,
            Cancelled = 5
        }


		public enum BasicSchoolConfigurationType
		{
			Standard = 0,
			Subject = 1,
			Division = 2,
			SubjectGrade = 3,
			TestNames = 4
		}

		public enum BasicRelationTable
		{
			StandardDivision = 0,
			ClassSubject = 1,
			StandardSubject = 2,
			DivisionSubject = 3
		}

		public enum TaskStatus
		{
			TASK_NOT_STARTED = 1,
			TASK_IN_PROGRESS = 2,
			TASK_ON_HOLD = 3,
			TASK_COMPLETED = 4
		}

        public enum StudentSurveyRegistrationCategory
        { 
            Interested = 1,
            NotInterested = 2,
            All = 3
        }

		public enum MarkAssignmentStatus
		{
			NotAssigned = 1,
			PartiallyAssigned = 2,
			Assigned = 3
		}

        public enum ConfigureMenuTypes
        {
            NewsLetterParentMenuId = 89
        }


		public enum MarkSubmitStatus
		{
			SubmitDenied = 1,
			Submit = 2,
			Submitted = 3
		}

		public enum BasicFieldsToFillDDList
		{
			Year = 0,
			State = 1,
			PassClass = 2,
			Salutation = 3,
			Caste = 4,
			Ocupation = 5,
			UserRole = 6,
			Qualification = 7
		}

        public enum DefaultLCValues
        {
            ReasonOfLeavingSchool = 17,
            Conduct = 20,
            Progress = 13
        }

		public struct ParameterNameValuePair
		{
			public string Name;
			public DbType DbType;
			public string Value;
		}

        public enum StudentAdditionalStatus
        { 
            Enquiry = 1,
            Registration = 2,
            Admission = 3
        }

		public enum ViewMode
		{
			New,
			Edit,
			View
		}

		public enum SchoolId
		{
			PPS = 18,
            SS = 1,
            PPSH = 11,
            LFS=21,
            FBS=31,
            MCPS = 61,
            PPSN=71,
			DSK = 81,
            NEMS = 101,
            PKIS = 107,            
            JOS = 110,
            EPPS = 118,
            LORDDS = 113,
            BFS = 120,
            GSS = 112,
            BMFS = 121,
            SNS = 122,
            MNS = 123,
            PEMS = 124,
            PKSC=107,
            STSS = 125,
            SSN = 127,
            CSNP = 128,
            CSNS = 129,
            OWS = 131,
            SPS = 132,
            SVP = 133,
            HSP = 136,
            MVPS = 137,
            SVNP = 138,
            PIONEER = 165,
            RITeSchool = 25,
            JPS = 140,
            DPIS = 5,
            ZLSP = 142,
            DYPV = 141,
            AaryanBhilarewadi = 150,
            NPS = 162,
            VPMCPS = 166,
            DPISRAVET = 168,
            CKInstOfCulinaryArtAndHotelMgmt = 169
		}

        public enum SchoolConfigurations
        {
            Standard = 1,
            Division = 2,
            StandardwiseDivision = 3,
            Subjects = 4,
            SubjectGrade = 5,
            TestNames = 6,
            SubjectGroups = 7,
            StandardwiseSubjects = 8,
            DivisionwiseSubjects = 9,
            MarksGrade = 10,
            FailCriteria = 11,
            WeekDaysConfiguration = 13,
            HolidaysManagement = 14,
            StandardwiseTests = 15,
            FeeType = 16,
            StandardwiseFeeTypes = 17,
            FeeSubType = 18,
            StandardwiseExamScheduleConfig = 19,
            LateFeeSettings = 20,
            StandardwiseFeeConfiguration = 21,
            Menu = 22,
            WeeklyMaxLecturePerStandardSubject = 23,
            MaxLecturePerStandard = 24,
            AcademicYear = 25,
            SubjectExamConfig = 26,
            Teacher = 27,
            ClassTeacher = 28,
            AssignedTeacherToSub = 29,
            Student = 30,
            TeacherTimeTable = 34,
            AnnualResult = 35,
            ExamMarks = 36,
            WeekDayTimeTable = 37,
            ClassTeacherTestMarks = 39,
            CopyTestConfiguration = 40,
            //FinalResultConfiguration = 41,
            //StandardwiseExamScheduleList = 43,
            ChangeStudentDivision = 63,
            TeacherTransfer = 64,
            PrePrimaryProgrssSheetConf = 48,
            IssuePeriod = 49,
            RenewAttempts = 50,
            BookPerPerson = 51,
            LectureTiming = 52,
            AdminStaffConfig = 53,
            PhotoGallery = 54,
            Fees = 56,
            WeeklyTimetable = 59,
            AnnualEventPlanner = 62,
            BookManagement = 72,
            AssignExamMarks = 74,
            MessageCenter = 75,
            UserMangement = 76,
            Attendance = 77,
            ExamResults = 78,
            ProgressReportConfig = 79,
            FinalResult = 80,
            SMSCenter = 81,
            LibrarySettings = 97,
            CategoryManagement = 98,
            ReturnRenewBooks = 99,
            PendingFeeList = 101,
            TeacherScreenAccess = 102,
            ApprovalLevelConfig = 105,
            StaffGroups = 111,
            EarningsAndDeductions = 112,
            StaffGroupsAndEarningDeductionsAssociation = 113,
            StaffLeaves = 114,
            OtherStaff = 115,
            UsersStaffGroupsAssociation = 116,
            UsersEarningsDeductions = 117,
            AdmissionLottery = 119,
            StaffAttendance = 120,
            PTChallanDetails = 123,
            TransportStaff = 126,
            VehicleDetails = 127,
            StopConfiguration = 128,
            RouteDetails = 129,
            RouteShiftTimmingDetails = 131,
            ShiftConfiguration = 132,
            TransportManagment = 133,
            LateMarkConfiguration = 138,
            StaffHolidayAndLeaveConfiguration = 140,
            PrePrimarySubjectsConfiguration = 142,
            PrePrimarySubSubjectsConfiguration = 143,
            PrePrimaryMonthsConfiguration = 144,
            PrePrimaryProgressReportRemarkConfiguration = 145,
            LibraryVendors = 146,
            GenerateBarcode = 148,
            ParentTeacherAssociation = 147,
            SchoolwiseTermConfiguration = 149,
            Library = 150,
            StandardwiseDocument = 151,
            AssessmentConfiguration = 154,
            StandardwiseAssessmentConfiguration = 155,
            GradeConfiguration = 156,
            SubjectwiseSubjectSectionConfiguration = 157,
            LearningOutcomeConfiguration = 158,
            WorkFlowConfiguration = 166,
            AutoTimeTable = 160,
            ExternalLectureConfiguration = 161,
            AssignXseedGrades = 162,
            XseedResults = 163,
            XseedProgressReport = 164,
            MenuFiles = 169,
            Theme = 168,
            OptionalSubject = 170,
            ManagementFileSharing = 171,
            Groups = 174,
            Ledgers = 175,
            BankAccounts = 176,
            ApprovalConfig = 177,
            AddEditBook = 178,
            StudentWiseProgressReport = 179,
            StandardWiseExamConfiguration = 182,
            LibrariansDesk = 183,
            RemarksConfiguration = 184,
            TransferOptionalSubjectMarks = 186,
            RemarkTemplate = 188,
            AttendanceStatus = 191,
            AttendanceAlertConfiguration = 192,
            RemarksCategory = 193,
            SalaryDetails = 121,
            PaySalary = 122,
            BasicLeaveConfiguration = 194,
            InvestmentMethod = 195,
            InvestmentDeclaration = 196,
            TaxDeduction = 197,
            IncomeTaxDetails = 198,
			IncomeTaxSlabs = 199,
			BlockProgressReport=200,
			HomeworkAssignment = 201,
			RetirementNoticeConfiguration = 202,
            Designations = 204,
            StaffPerformanceRelated = 205,
            PerformanceGrade = 206,
            PerformanceSkill = 207,
            Houses = 208,
            HouseInformation = 209,
            PerformanceParameter = 210,
            ReportingUserConfiguration = 211,
            ReportingConfiguration = 214,
            ProgressRemarksLengthConfiguration = 217,
            SchoolwiseExamStatus=215,
            TransportLateFeeSettings = 219,
            PaymentGroups = 222,
            TransportCommittee = 227,
            LessonPlanReportingUserConfig = 236,
            LessonPlanScreenFullAccess = 233,
            ExamTypes = 234,
            LessonPlanStdSubjects = 239,
            LessonPlanParameter = 238,
            AssignGrades = 241,
            RegenerateReassignRollNos = 243,
            ProgressRemarks = 266,
            StudentRecords = 271,
            HealthComponent = 275,
            HealthParameter = 276,
            TermwiseHeightWeight = 247,
            AwardDetails = 301,
            StudentRecordParameter = 302,            
            OnlineExamQuestionConfiguration = 309,
            OnlineExamConfiguration = 310,
            OnlineExamResult = 311,
            OnlineExamProgressReport = 312,
            AssignGradesResult = 313,
            DescriptiveIndicators = 314,
            StockManagement = 106,
			LecturewiseAttendanceScreen = 317,
            ConfigurePeerDetails = 334,
            StudentListForSelfAssessment = 335,
            ResultDetails = 336,
            ObservationParameters = 340,
            ObservationSkill = 339,
            StudentMonthlyStatus=341,
            UploadStudentDocument=343,
            StudentListForActivityDetails = 344
        }

		public enum Action
		{
			Insert,
			Update,
			Delete
		}

		public enum UserRoles
		{
			None=0,
            Admin = 1,
			Teacher = 2,
			Student = 3,
			Supervisor = 6,
			OtherStaff = 7,
			TransportStaff = 8,
			Parent = 9,
			ParentTeacherAssociation = 99,
            ExAdmin = 10
		}

		public enum UploadFileType
		{
			Student,
			Teacher,
			CautionMoney,
			Fee,
			Supervisor,
            Challan,
            StudentHealth,
            VehicleReadingAllocation,
            VehicleMaintenance,
            RFID
        }

        public enum SMSTypes
        { 
        None = 0,
        ForgotPasswordDetailSMS = 1
        }

		public enum ReferenceId
		{
			Standard = 1,
			Division = 6,
			StandardwiseDivision = 8,
			Subjects = 11,
			StandardwiseSubjects = 14,
			DivisionwiseSubjects = 16,
			ExamConfiguration = 19,
			ExamName = 21,
			Teacher = 25,
			StandardExams = 30,
			FeeSubTypes = 33,
			FeeTypes = 35,
			StandardFees = 38,
			StandardFeeSubtypes = 41,
			TeacherClassSubject = 43,
			StandardLateFees = 45,
			MaxWeeklyLectures = 47,
			MaxWeekDayLectures = 50,
			WeeklyStdSubjectLectures = 52,
			WeekDays = 55,
			MarksGradeConfiguration = 57,
			PassFailCriteria = 60,
			TeacherSubjectAssignment = 65,
			TeacherStandardAssignment = 62,
			ClassTeacherAssignment = 67,
			ExamSchedule = 69,
			ExamSubjectSchedule = 77,
			CategoryId = 79,
			SubCategoryId = 81,
			PrePrimaryProgrssSheetConf = 83,
			Inventory = 85,
			StudentPaidFee = 87,
			StaffGroups = 89,
			Earnings = 90,
			StaffGroupsAndEarningsDeductionsAssociation = 93,
			Deductions = 94,
			StaffLeaves = 96,
			PrePrimaryProgressReportSubjectConfigList = 103,
			PrePrimaryProgressReportRemarks = 105,
			PrePrimaryProgressReportSubSubjects = 104,
			AssessmentConfiguration = 107,
			StandardwiseAssessmentConfiguration = 108,
			SubjectSectionConfiguration = 109,
			StandardwiseAssessmentConfig = 111,
			GradeConfiguration = 114,
			ClassWiseOptionalSubject = 117
		}

		public enum FeeTypes
		{
			Monthly = 3,
			Term = 4,
			Annual = 5,
		}

		public enum SchoolConfigMenuId
		{
			Basic_Configuration = 86,
			Teacher_Related = 87,
			Attendance_Related = 88,
			Other_User_Related = 89,
			Fee_Related = 90,
			Timetable_Related = 91,
			Exam_Related = 92,
			Library_Related = 100,
			Inventory = 104,
			Payroll_Related = 110,
			Transport_Releted = 125,
			Xseed_Report_Related = 153,
			Task_Related = 165,
            Accounts_Related = 173,
            StaffPerformanceRelated = 205,
            LessonPlanRelated = 235,
            Ask_Me_Related = 249,
            Health_Related = 276,
            Student_Record_related = 302,
            OnlineExamRelated = 308,
            ObservationRelated = 338
		}

		public enum ReportParameterFilters
		{
			WithoutSchoolAcademic = 0,
			SchoolId = 1,
			SchoolAcademicYrId = 2
		}

		public enum PageMode
		{
			Normal = 0,
			Print = 1,
			Edit = 2,
			ShowAll = 3
		}

		public enum PrimaryKeyRecord
		{
			First = 0,
			Last = 1
		}

		public enum ScreenLevel
		{
			DashBoard			= 1,
			SchoolConfiguration = 2,
			Configuration		= 3
		}

		public enum RequisitionStatus
		{
			Pending					= 1,
			Denied					= 2,
			Approved				= 3,
			Waiting_For_My_Approval = 4,
			My_Requisition			= 5,
			Actioned_By_Me			= 6
		}

		public enum SMSTemplate
		{
			AdmissionConfirmationSMS		= 1,
			ChequeBounceSMS					= 2,
			ForgotPasswordDetailSMS			= 3,
			StudentLoginDetailSMS			= 4,
			OnlineAdmissionLoginDetailsSMS	= 5,
			OnlineFeeDetailsSMS				= 6,
			SelectedInLotterySMS			= 7,
			SalarySMS						= 8,
			UserDeactivationSMS				= 9,
			UserActivationSMS				= 10,
			SalaryDetailsSMS				= 11,
			ExamPublishSMS					= 14,
            NewFeesSMS                      = 15,
            NewFeesUpdateSMS                = 16,
            FeesDeletedSMS                  = 17,
			FeeDefaulterDeactivationSMS		= 18,
            MobileWebsiteDetailsSMS         = 19,
            StudentBirthdaySMS              = 20,
            StaffBirthdaySMS                = 21,
            FeeDefaulterActivationSMS       =22,
            FormReceivedSMS = 23,
            FeePaymentAcknowledgementSMS = 24,
            UserSanctionLeaveDeactivationSMS = 25,
            HomeworkAssignmentSMS = 0,
            AdmissionProvisionalConfirmationSMS = 29,
            EnquirySubmitSMS = 30,
            ChequePaymentSMS = -1
          
		}

        public enum ExportReports
        {
            ChequeClearanceDetails              = 1,
            CautionMoneyDetails                 = 2,
            StudentDetails                      = 3,
            SalarySlipReport                    = 4,
            LeavingCertificate                  = 5,
            AdmissionLotteryDetails             = 6,
            AdmissionFormReport                 = 7,
            StudentwiseProgressReport           = 8,
            OnlineTransactionClearanceDetails   = 9,
            CardPaymentDetails                  = 10,
            ExportCashPayment                   = 11,
            StudentTerm1ProgressReport          = 12,
            StudentTerm2ProgressReport          = 13,
            OutofAcademicYearStudentList        = 14,
            LeavingCertificateLFS               = 15,
            StudentwiseProgressReportSS         = 16,
            PendingFeeReminder                  = 17,
            StudentwiseProgressReportFBS        = 18,
			FeeReciept							= 19,
            FormNo16Report                      = 20,
            LeavingCertificateSS                = 21,
            LeavingCertificatePP                = 22,
            ElectronicPaymentDetails            = 23,
            StudentwiseProgressReportPPSN       = 24,
            ConsolidatedStudentAdmissionList    = 25,
            ServiceContract                     = 26,
            AppointmentLetter                   = 27,
            LessonPlan                          = 28,
            LeavingCertificateJPS               = 29,
            LeavingCertificateDSK               = 30,
            LeavingCertificateSNS               = 31,
            StudentwiseProgressReportPPSH       = 32,
            LeavingCerificatePPSN               = 33,
            StudentCautionMoneySNS              = 34,
            SchoolGuestDetails                  = 35,
            SchoolGuestDetailsForExport         = 36,
            StudentwiseProgressReportPKIS       = 37,
            LeavingCertificateSSN               = 38,
            LeavingCertificateSSNMarathi        = 39,
            TestwiseReport                      = 40,
            PurchaseOrder                       = 41,
            LeavingCertificateSPS               = 42,
            LeavingCertificateOWS               = 43,
            StudentwiseProgressReportSVP        = 44,
            LeavingCertificateCSNP              = 45,
            LeavingCertificateSVP               = 46,
            ClasswiseBankChallan                = 47,
            TransferCertificatePPSH             = 48,
            LeavingCertificatePPSH              = 49,
            LeavingCertificateHSP               = 50,
            LeavingCertificateMVPS              = 51,
            OnlineAdmissionFormPEMS             = 52,
            LeavingCertificateMCPS              = 53,
            StudentRegistrationForm             = 54,
            StudentRegistrationFeeReceipt       = 55,
            StudentAdmissionFormSPS             = 56,
            StudentAdmissionConfirmation        = 57,
            StudentwiseTermProgressReportSVP    = 58,
            StudentwiseProgressReportSVP_9      = 59,
            StudentwiseProgressReportPPS        = 60,
            StudentwiseProgressReportPPS_Grading = 61,
            StudentFeeReceiptForZLSP            = 62,
            LeavingCertificateJOS               = 63,
            LeavingCertificateForZeal           = 64,
            ClasswiseBankChallan_Aaryan         = 65,
            StudentwiseProgressReport_Aaryan = 66,
            StudentwiseProgressReportAaryan_5to8 = 67,
            ItemDetailsReport=68,
            EmployeeDetailsReport=69,
            StudentwiseProgressReportPPSH_9th    = 70,
            StudentwiseProgressReportPPSH_1stTO5th = 71,
            StudentwiseProgressReportPPSH_Xseed = 72,
            XseedProgressReport_PPS             = 73,
            PrelimReport = 74,
            LeavingCertificateForAryan = 75,
            LeavingCertificateForBFS = 76 ,
            LeavingCertificateForDYPV = 77,
            LeavingCertificateNurseryTo9th_NPS = 78,
            StudentFinalProgressReport9thStd_PPSH_AY10=79,
            FinalProgressReportNPS = 80,
            LeavingCertificateDPIS = 81,
            VehicleBillingDetails = 82,
            MaterialwiseStockDetails = 83,
            GSTInvoiceDetails=84,
            PPSTermwiseReport = 85,
			PODetails=86,
            PrelimReportPP = 87,
            FinalReportPP = 88,
            IncomeTaxReconciliation = 89,
            CancellationFormDetails = 90,
            CautionMoneyReceipt = 91,
            InternalFeeReceipt = 92,
            ConfirmationLetter = 93,
			InauguralCertificate = 94,
            StudentwiseProgressReportMNS = 95,
            LeavingCerificateVPMCPS = 96,
            LeavingCerificatePioneer = 97,
            InternalFeeReceiptSNS = 98,
            BonafideRequestApplication = 99,
            VehicleDocumentDetails = 100,
            StudentwiseTermProgressReportSNS_1rdTO5th2024 = 101,
            ExportVehicleDetails = 102,
            ExportStudentMonthlyDetails = 103,
            StudentCautionMoneySNSForStudentLogin = 104,
            StudentFeeReceipt = 105,
            StudentwiseProgressReportPioneer_NurseryTO2nd=106,
			HolosticProgressReportPPSNFor3to5=107,
            HalfYearlyReportFor3To9Pioneer = 108,
            EnquiryFormReport=109
        }

		public enum BarcodeChar
		{
			Book		= 'B',
			Separator	= 'P',
			Admin		= 'M',
			Teacher		= 'T',
			Student		= 'S',
			AdminStaff	= 'A',
			OtherStaff	= 'O',
			School		= 'P'
		}

		public enum ExamStatus
		{
			Absent,
			Exempted
		}

		public enum SuperAdminRoles
		{
			SuperAdmin		= 1,
			ManagementUser	= 99
		}

		public enum TransactionType
		{
			Credit	= 0,
			Debit	= 1
		}

        public enum NoticeDisplayLocation
        {

            Both			= 'B',
            Control_Panel	= 'C',
            Home_Page		= 'H'
        }

		public enum AccountsGroups
		{
			BankAccounts = 16,
			CashInHand	 = 17
		}

		public enum VoucherType
		{
			Payment	= 1,
			Receipt	= 2,
			Journal	= 3,
			Contra	= 4
		}

		public enum Screen
		{
			StudentUI = 1
		}

		public enum PaymentMode
		{
			Cash	= 1,
			Cheque	= 2,
			Card	= 3,
			Online	= 4,
            Electronic = 5,
            JournalVoucher = 7
		}

		public enum GroupNature
		{
			Incomes		= 1,
			Expenses	= 2,
			Assets		= 3,
			Liabilities = 4
		}

        public enum OnlineFeeTypes
        {
            AdmissionFee = 1,
            StudentFee	 = 2,
            CautionMoney = 3,
            InternalFee  = 4
        }

        public enum TransactionStatus
        {
            Created = 'C',
            Completed = 'Y',
            Failed = 'N'    
        }

        public enum AttendanceStatus
        {
            AtteandanceNotTaken = -3,
            AtteandanceTaken = -2,
            OutsideAcademicYear = -4,
            Other=-5,
            NoClassAvailable = -1
        }

        public enum Salutation
        {
			Mr=1,
            Mrs=2,
            Ms=3,
            Dr=4,
            Master=5,
            Miss=6
        }

        public enum ReportFolders
        { 
           SchoolConfiguration = 1,
           Attendance           = 2,
           TimeTable           = 3,
           Fee                  = 4,
           Exam                 = 5,
           Holiday              = 6,
           Teacher              = 7,
           Student              = 8,
           Library              = 15,
           Payroll              = 20,
           Task                 = 21,
           Xseed                = 25,
           Transport            = 30
        }

        public enum ItemState
        {
            saved,
            updated,
            deleted            
        }

        public enum ButtonText
        {
            Save,
            Update,
            Show            
        }

        public enum ReportCellType
        {
            Name = 0,
            ExamType = 1,
            Grade = 2,
            ExamTypeTotal = 3,
            GroupTotal = 4,
            ExamTypeGroupTotal = 5,
            GradeExamTypeTotal = 6
        }

        public enum SectionGroups
        {
            GrossSalary = 1,
            Allowance = 2,
            Deduction = 3,
            OtherIncome = 4,
            DeductionUnderChapterVIA = 5,
            Group89 = 6
        }

        public enum SectionCategories
        {
            A = 1,
            B = 2
        }

        public enum SchoolLogos
        {
            SchoolLogo=-9997,
            SignLogo=-9998,
            ICardLogo=-9999
        }

        public enum DocumentTypes
        {
            InvestmentDocuments = 1,
            StudentDocuments = 2,
            TeacherDocuments = 3,
			EducationCertificate=4,
			ExperienceCertificate=5,
            PAN=6,
            PerformanceEvaluation= 8,
            AadharCard = 9
        }

		public enum FeePaymentType
        {
            Cash = 0,
            Cheque = 1,
            PDC = 2,
            SwapCard = 3,
            Electronic = 4,
            JournalVoucher = 5
        }

        public enum PaymentGateways
        { 
            TPSL = 1,
            AxisBank = 2,
            PayU = 3,
            Atom = 4,
            PayUMoney = 5,
            AxisBankForAll = 6,
            EaseBuzz = 7,
            Billdesk = 8,
			BilldeskDYP=9,
			CCAvenue=10,
			RazorPay = 11,
            CCAvenueVPMCPS = 12,
		}

        public enum PaymentMethod
        {
            BankPayment =1,
            CardPayment = 2
        }

        public enum TransportType
        {
            Pickup = 1,
            Drop = 2
        }

        public enum PhotoGallerySection
        {
            PrePrimary = 1,
            Primary = 2,
            Secondary = 3
        }

        public enum SchoolCommittees
        {
            PTA = 1,
            Transport = 2
        }

        public enum AskMeStatus
        {
            All = 0,
            New = 1,
            Waiting = 2,
            InProgress = 3,
            Closed = 4,
            InvalidQuery = 5
        }

        public enum ReportingParameters
        {
            RetirementNotice = 2,
            Moderator = 3,
            JobPeriodNotification = 7,
            AllowPartialFee = 9,
            AllowItemBalanceUpdation = 12,
            RestrictUsersForFeeUpdation = 13,
            AllowUsersonlyForPayFee = 14 ,
            HideTabsFromStudentDetailScreen = 15,
            AllowItemDeleteAccess=16,
            PrePrimaryCoordinator = 17,
            AllowPaymentClearanceNotification=20,
			LeaveApprovalRejection = 22,
            ExternalPOApprover = 23,
            ExternalPOStaff = 24,
            AllowUserToDeleteFee = 26,
			AllowExamPublishAction = 27
		}

        public enum AtomCategories
        {
            All = 1,
            PrePrimary = 2,
            Primary = 3
        }

        public enum AdmissionStatus
        {
            Open = 1,
            InProcess = 2,
            StatementCompletedEventArgs = 3
        }

        public enum AdminDesignations
        {
            ChiefAdministratorOfficer = 3,
            ExAdmin = 1005
        }

        public enum FeedbackInputTypes
        {
            Both = 1,
            Text = 2,
            Grade = 3
        }

		public enum LessonPlanConfigTypes
        {
            Day = 1,
            Week = 2,
            FortNight = 3,
            Month = 4
        }

        public enum ReportingUserScreen
        {
            StaffPerformanceEval = 1,
            LessonPlan = 2,
            UserLeaveApprovalConfiguration = 3
        }

        public enum InputControls
        {
            Checkbox = 1,
            Textbox = 2,
            RadioButton =3,
            CheckboxAndTextbox = 4,
            MultilineTextbox = 5,
            CheckBoxList = 6,
            FileUploadControl = 7
        }

        public enum PayUMoneyPaymentModes
        {
            NB,
            DC,
            CC
        }

        public enum LanguageMode
        {
            SecondLanguage = 1,
            ThirdLanguage = 2
        }

        public enum SMSProviders
        {
            BusinessSMS,
            SoftSMS
        }

        public enum DemoCompanyName
        {
            Marpha=1,
            GMore = 2
        }

        public enum TransportOptions
        {
            Servicing = 1,
            Passing = 2,
            PUC = 3
        }

        public enum VPMCPSProductInfo
        {
            VPMCPS,
            VPMCPS_PP
        }

        public enum DownloadFileType
        {
            MessageCenterAttachment = 1
        }

        #endregion


        #region "Mobile - Push notification parameters"

        public const string S_NOTIFICATION_PARAMETER_FIRSTNAME = "{FirstName}";
        public const string S_NOTIFICATION_PARAMETER_LASTNAME = "{LastName}";
        public const string S_NOTIFICATION_PARAMETER_EXAMNAME = "{ExamName}";
        public const string S_NOTIFICATION_PARAMETER_HOLIDAYNAME = "{HolidayName}";
        public const string S_NOTIFICATION_PARAMETER_SCHOOLNAME = "{SchoolName}";
        public const string S_NOTIFICATION_PARAMETER_STARTDATE = "{StartDate}";
        public const string S_NOTIFICATION_PARAMETER_ENDDATE = "{EndDate}";
        public const string S_NOTIFICATION_PARAMETER_STANDARD = "{Standard}";
        public const string S_NOTIFICATION_PARAMETER_DIVISION = "{Divison}";
        public const string S_NOTIFICATION_PARAMETER_CLASSNAME = "{ClassName}";
        public const string S_NOTIFICATION_PARAMETER_DAY = "{Day}";
        public const string S_NOTIFICATION_PARAMETER_DATE = "{Date}";
        public const string S_NOTIFICATION_PARAMETER_SUBJECT = "{Subject}";
        public const string S_NOTIFICATION_PARAMETER_GALLERYNAME = "{GalleryName}";
        public const string S_NOTIFICATION_PARAMETER_VIDEONAME = "{VideoName}";
        public const string S_NOTIFICATION_PARAMETER_HEADING = "{Heading}";
        public const string S_NOTIFICATION_PARAMETER_FEEAMOUNT = "{FeeAmount}";
        public const string S_NOTIFICATION_PARAMETER_FEETYPE = "{FeeType}";
        public const string S_NOTIFICATION_PARAMETER_DESIGNATION = "{Designation}";
        public const string S_NOTIFICATION_PARAMETER_FULLNAME = "{FullName}";
        public const string S_NOTIFICATION_PARAMETER_MESSAGE_SUBJECT = "{Subject}";
		public const string S_NOTIFICATION_PARAMETER_USERNAME = "{UserName}";
        public const string S_NOTIFICATION_PARAMETER_PASSWORD = "{Password}";        
        #endregion
    }

    public class PayrollConstants
    {
        public const string S_SALARY_DETAILS = "TBL_SALARY_DETAILS";
        public const string S_SALARY_ENTITY_LIST = "SalaryEntityList";
        public const string S_SALARY_DIFFERENCE = "Salary Difference";
        public const string S_GROSS_SALARY = "Gross Salary";
        public const string S_TOTAL = "Total";
        public const string S_TOTAL_DEDUCTION = "Total Deduction";
        public const string S_NET_SALARY = "Net Salary";
        public const string S_SHOW = "Show";
        public const string S_CHANGE_INPUT = "Change Input";
        public const string S_LATE_MARK_LEAVES = "Late Mark Leaves";
        public const string S_NET_DIFFERENCE = "Net Difference";
        public const string S_SAVED_DIFFERENCE = "Saved Difference";
        public const string S_PAID_DIFFERENCE = "Paid Difference";

        public const string ED = "ED";
        public const string LD = "LD";
        public const string GS = "GS";
        public const string TD = "TD";
        public const string NS = "NS";

        public const string S_SINGLE_SPACE = " ";
        public const string S_UNDERSCORE = "_";
    }

    public class FileExtensions
    {
        public const string JPG = ".JPG";
        public const string JPEG = ".JPEG";
        public const string BMP = ".BMP";
        public const string PDF = ".PDF";
        public const string DOC = ".DOC";
        public const string DOCX = ".DOCX";
        public const string XLS = ".XLS";
        public const string XLSX = ".XLSX";
    }


    public class AdmissionMasterData
    {
        public const int I_TABLE_ACADAMIC_YEARS = 0;
        public const int I_TABLE_NEW_ACADAMIC_YEAR_ID = 1;
        public const int I_TABLE_STANDARDS = 2;
        public const int I_TABLE_RELIGIONS = 3;
        public const int I_TABLE_OCCUPATIONS = 4;
        public const int I_TABLE_EVENTS = 5;
        public const int I_TABLE_CATAGORIES = 6;
        public const int I_TABLE_MOTHER_DATA = 7;
        public const int I_TABLE_STUDENT_DETAILS = 8;
        public const int I_TABLE_PARENT_DETAILS = 9;
        public const int I_TABLE_PARENT_IN_EVENTS  =10;
        public const int I_TABLE_LOCATION_AREA = 11;
        public const int I_TABLE_ADMISSION_ADDITONAL_DETAILS  =12;
        public const int I_TABLE_BLOOD_GROUPS = 13;
        public const int I_TABLE_RESIDENCE_TYPES = 14;
        public const int I_TABLE_SECOND_LANGUAGE = 15;
        public const int I_TABLE_STUDENT_HEALTH_DETAILS = 16;
        public const int I_TABLE_STUDENT_10th_STD_MARK_DETAILS = 17;
        public const int I_TABLE_BROTHER_DETAILS = 18;
        public const int I_TABLE_STREAMWISE_Subject_DETAILS = 19;
        public const int I_TABLE_THIRD_LANGUAGE = 20;
    }
}
