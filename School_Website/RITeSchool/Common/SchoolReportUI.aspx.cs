/* File Name :- SchoolReportUI.aspx
 * Modified By :- Sachin
 * Modified Date :- 2-Oct-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display all reports list in treeview and open selected report.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;                                                                      
using System.Text;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using System.Xml;
using ASP;
using BusinessLogic;
using BusinessLogic.Exceptions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using RJS.Web.WebControl;
using Utility;
using System.Configuration;
using XseedReportEntities;
using SchoolEntities;
using System.Web.UI;
using BusinessLogic.MusterRollDetails;
using SchoolEntities.MusterRollDetails;
using Excel = Microsoft.Office.Interop.Excel;
using BusinessLogic.StudentPaidFeeDetailsReport;
using SchoolEntities.StudentPaidFeeDetails;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using A = DocumentFormat.OpenXml.Drawing;
using dr = System.Drawing;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using BusinessLogic.TransportBL;
using SchoolEntities.ProgressReport;
using PayrollReportingUserEntities;
using SchoolEntities.Teacher;

///<summary>
///	This class is used to generate tree structure of different reports as well to display report's parameters with respective controls.
///</summary>
public partial class SchoolReportsUI : ExportToExcel
{
    #region Data Members

    ExportReportBL oExportReportBL;
    MusterRollDetailsBL moMusterRollDetailsBL;
    StudentPaidFeeDetailsReportBL moStudentPaidFeeDetailsBL;
    OldYearPendingFeeReport moPendingFee;  //

    List<StudentMarkDetails> mlStudentMarkDetails;
    List<SchoolEntities.MusterRollDetails.AttendanceDetails> mlstAttendanceDetails;
    List<SchoolEntities.StudentPaidFeeDetails.StudentDetails> mlstStudentDetails;
    AttendanceReportEntity.StudentAttendanceReport moStudentAttendanceReport;
    FeeEntities.PaidFeeDetails moPaidFeeDetails;
    SchoolEntities.InternalPaidFeeExamDetails moInternalPaidFeeExamDetails;
    SchoolEntities.StudentsAcademicYearwisePendingFeeCountDetails moStudentsYearwisePendingfeecount;
    List<DateTime> mlstSundays;
    TestwiseMark moTestwiseMark;

    private string msReportID;
    private string msReportPath;
    private string msReportViewName;
    private ReportDocument crReportDocument;
    private string msSchoolName;
    private string msAcademicYearName;
    private string msOrgnizationName;
    private string msEnrollmentNumber;
    private int miStudentId;
    private string msType;
    private string msView;
    private int miIsSearchGridConsidered;
    private int miFirstRowNo = 6,miFirstRowForHSP = 5, miFontSize = 12, miRowsBeforeSignSection = 3, miSchoolNameRowIndex=1, miMusterRollStartIndex=6, miMusterRollStartupRow = 6, miStudentPaidFeeDetailsStartIndex = 2, miStudentPaidFeeStartupRow = 2;
    private Dictionary<string, string> moDictFiledDatatype = new Dictionary<string, string>();
    private List<string> molstPayrollDateReports = new List<string> { S_SALARY_SLIP,S_TRANSFERED_STAFF_SALARY_SLIP, S_SALARY_LEDGER, S_EARNINGS_DEDUCTIONS, S_INSURANCE_DETAILS, S_SALARY_SHEET, S_NET_SALARY, S_FORM_NO_16, S_PROVIDENT_FUND_DETAILS, S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE, S_BANK_LETTER, S_STAFF_ATTENDANCE };
    private List<string> mlstPayrollReports = new List<string> { S_SALARY_SLIP, S_TRANSFERED_STAFF_SALARY_SLIP, S_SALARY_LEDGER, S_EARNINGS_DEDUCTIONS, S_INSURANCE_DETAILS, S_SALARY_SHEET, S_NET_SALARY, S_FORM_NO_16, S_INVESTMENT_DECLARATIONS, S_PROFESSIONAL_TAX_DETAILS, S_PROFESSIONAL_TAX_CHALLAN, S_STAFF_LEAVES, S_LEAVE_BALANCE, S_SERVICE_TYPE_DETAILS, S_BANK_LETTER, S_SALARY_BANK_STATEMENT, S_PROVIDENT_FUND_DETAILS, S_FORM_NO_27A, S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE, S_STAFF_ATTENDANCE, S_STAFF_LEAVE_DETAILS_EXPORT };
    private List<string> mlstUserAccessPayrollReports = new List<string> { S_SALARY_SLIP, S_TRANSFERED_STAFF_SALARY_SLIP, S_STAFF_LEAVES,S_LEAVE_BALANCE, S_INVESTMENT_DECLARATIONS, S_FORM_NO_16, S_BANK_LETTER, S_STAFF_ATTENDANCE };
    
    #endregion

    #region Constants

    private const string S_LC_REPORT_ID = "32";
    private const string S_BONAFIDE_CERTIFICATE_REPORT_ID = "83";
    private const string S_CHARACTER_CERTIFICATE_REPORT_ID = "195";
    private const string S_BONAFIDE_CERTIFICATE_REPORT_FOR_JOS_ID = "193";
    private const string S_BONAFIDE_CERTIFICATE_REPORT_FOR_SS_ID = "117";
    private const string S_BONAFIDE_CERTIFICATE_REPORT_FOR_PPSH_ID = "125";
    private const string S_STUD_DETAIL_REPORT_ID = "7";
    private const string S_DISPLAY_MEMBER = "Display_Member";
    private const string S_VALUE_MEMBER = "Value_Member";
    private const string S_CHECKBOXLIST = "checkboxlist";
    private const string S_DROPDOWNLIST = "dropdownlist";
    private const string S_TEXTBOX = "textbox";
    private const string S_DATETIME = "datetime";
    private const string S_TOP_ROW = "-- Select --";

    private const string S_BONAFIDE_REPORT = "Parameters For Bonafide Certificate Report";
    private const int I_VIEWNAME_INDEX = 1;
    private const int I_ISREQUIRED_INDEX = 6;
    private const int I_DATATYPE_INDEX = 0;
    private const int I_FIELD_NAME_INDEX = 3;
    private const int I_DISPLAY_NAME_INDEX = 2;
    private const int I_TYPE_ROW = 0;
    private const int I_EXAM_ROW = 3;

    private const string S_CLASSWISE_STUDENT_LIST = "1";
    private const string S_EXAM_PUBLISH_STATUS = "68";
    private const string S_LEFT_STUDENT_DETAIL = "66";
    private const string S_MISSING_ATTENDANCE_REPORT = "67";
    private const string S_MUSTER_REPORT = "8";
    private const string S_IT_RECONCILIATION_RPT_ID = "55";
    private const string S_ASSEMBLY_LECT_NO = "AssemblyLectureNo";
    private const string S_MPT_WEEKDAY = "MPTDay";
    private const string S_MPT_LECT_NO = "MPTLectureNo";
    private const string S_ASSEMBLY_WEEKDAY = "AssemblyDay";
    private const string S_MPT_NAME = "MPTName";

    private const string S_SATYBACK_NAME = "StayBackName";
    private const string S_ANNUAL_RESULT = "Final Result";
    private const string S_ANNUAL_RESULT_TYPE = "-1";
    private const string S_ASSEMBLY_NAME = "AssemblyName";
	private const string S_STUDENTS_ANNUAL_ATTENDANCE = "43";
    private const string S_CLASS_TT_REPORT_ID = "30";
    private const string S_TEACHER_TT_REPORT_ID = "38";
    private const string S_SCHOOL_TT_REPORT_ID = "44";
    private const string S_FREE_TEACHER_LIST_REPORT_ID = "48";
    private const string S_DAILY_TEACHER_LECTCNT_REPORT_ID = "37";
    private const string S_TEACHER_REPLACEMENT_LIST_REPORT_ID = "39";
    private const string S_DATEWISE_POSTDATED_CHEQUE_REPORT_ID = "62";
    private const string S_CLASSWISE_STUDENT_PENDING_FEE_REPORT_ID = "47";
    private const string S_ENROLLMENTWISE_STUDENT_I_CARDS = "148";
    private const string S_ENROLLMENTWISE_STUDENT_AUTHORITY_CARDS = "152";
    private const string S_SUBJECT_TOPPERS = "65";
    private const string S_EXAM_RESULT = "82";
    private const string S_STUD_FINAL_RESULT = "88";
    private const string S_STUD_FINAL_RESULT_PPSH = "158";
    private const string S_STUD_FINAL_RESULT_SNS_6TO8_Std = "194";
    private const string S_STUD_FINAL_RESULT_PPSH_Old = "191";
    private const string S_REQUISITION_DETAILS = "154";
    private const string S_STUD_FINAL_RESULT_PPSN = "137";
    private const string S_STUD_FINAL_RESULT_MCPS = "138";
    private const string S_STUD_FINAL_RESULT_SS = "115";
    private const string S_EXAM_RESULT_SS = "111";
    private const string S_EXAM_RESULT_STSS_9STD = "203";
    private const string S_EXAM_RESULT_STSS_10STD = "206";
    private const string S_EXAM_RESULT_FBS = "120";
    private const string S_EXAM_RESULT_PPSN = "132";
    private const string S_STUD_TERM2_RESULT = "90";
    private const string S_STUD_TERM1_RESULT = "99";
    private const string S_STUD_PRELIMINARY_RESULT = "122";
    private const string S_STUD_FEE_LEDGER = "89";
    private const int I_DIVISION_REPORT_FIELD_ID = 224;
    private const string S_DUE_DATE_PASSED_BOOK_RPT_ID = "58";
    private const string S_TEACHER_ASSIGNMENT_RPT_ID = "39";
    private const string S_DEACTIVATED_USERS_RPT_ID = "41";
    private const string S_EXAM_PERFORMANCE_REPORT_ID = "49";
    private const string S_PENDING_FEE_DETAILS = "70";
    private const string S_STUD_INTERNAL_ASSESSEMENT_DETAILS = "98";
    private const string S_PENDING_FEE_STUDENTLIST = "71";
    private const string S_NETBANKING_REPORT = "72";
    private const string S_SALARY_BANK_STATEMENT = "73";
    private const string S_SALARY_SLIP = "74";
    private const string S_TRANSFERED_STAFF_SALARY_SLIP = "198";
    private const string S_BANK_LETTER = "131";
    private const string S_NET_SALARY = "124";
    private const string S_FORM_NO_16 = "126";
    private const string S_INTERNAL_FEE = "80";
    private const string S_STAFF_LEAVES = "81";
    private const string S_EMPLOYEE_DETAILS = "86";
    private const string S_CATEGORYWISE_ITEM_DETAILS = "155";
    private const string S_SALARY_LEDGER = "84";
    private const string S_PROVIDENT_FUND_DETAILS = "75";
    private const string S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE = "139";
    private const string S_PROFESSIONAL_TAX_DETAILS = "76";
    private const string S_PROFESSIONAL_TAX_CHALLAN = "77";
    private const string S_EARNINGS_DEDUCTIONS = "92";
    private const string S_ANNUAL_CONSOLDATED_REPORT = "63";
    private const string S_ANNUAL_CONSOLDATED_REPORT_SPS9 = "210";
    private const string S_ANNUAL_CONSOLDATED_REPORT_SPS11 = "211";
    private const string S_ANNUAL_CONSOLDATED_REPORT_SS = "112";
    private const string S_ANNUAL_CONSOLDATED_REPORT_SNS = "189";
    private const string S_ANNUAL_CONSOLDATED_UNITTEST_REPORT_SVNP = "227";
    private const string S_CLASSWISE_STUDENT_BANK_STATEMENT_REPORT = "54";
    private const string S_STUDENT_PHOTOS = "96";
    private const string S_USER_ROLEWISE_IDENTITY_CARDS = "97";
    private const string S_STUDENT_IDENTITY_CARDS = "85";
    private const string S_STUDENT_AUTHORITY_CARDS = "150";
    private const string S_STUDENT_DOCUMENT_DETAILS = "95";
    private const string S_INSURANCE_DETAILS = "100";
    private const string S_STUDENT_NOT_SELECTED_IN_LOTTERY = "101";
    private const string S_DAYWISE_ABSTNT_STUDENT = "121";
    private const string S_TASK_DETAILS = "102";
    private const string S_XSEED_REPORT = "103";
    private const string S_STOPWISE_TRANSPORT_DETAILS = "105";
    private const string S_LEAVE_BALANCE = "106";
    private const string S_SALARY_SHEET = "116";
    private const string S_DATEWISE_ATTENDANCE_COUNT = "142";
    private const string S_STUDENT_DETAIL_INFORMATION = "143";
    private const string S_STUDENT_ANNUAL_ATTENDANCE = "43";
    private const string S_USERROLEWISE_TRAVELLER_DETAILS = "107";
    private const string S_SERVICE_TYPE_DETAILS = "108";
    private const string S_USERROLEWISE_BOOK_ISSUED_USERS = "109";
    private const string S_LOST_BOOK_DETAILS = "110";
    private const string S_CLAIM_BOOK_DETAILS = "113";
    private const string S_FEE_PAID_STUDENT_COUNT = "69";
    private const string S_INVESTMENT_DECLARATIONS = "123";
    private const string S_CAUTION_MONEY_DETTAILS = "127";
	private const string S_BONAFIEDREPORTLFS = "83";
    private const string S_FORM_NO_27A = "130";
    private const string S_DAILY_FEE_COLLECTION = "22";
	private const string S_ADDITIONAL_FEETYPE_PAYMENT_DETAILS = "133";
    private const string S_STUDENTS_HOUSE = "134";
    private const string S_RESULTSHEET = "136";
    private const string S_NEW_ADMISSION_COUNT = "140";
    private const string S_STAFF_BIRTHDAY = "141";
    private const string S_PERFORMANCE_EVALUATION = "145";
    private const string S_SURVEY_ANALYSIS = "147";
    private const string S_EXPORT_STUDENT_LIST = "149";
    private const string S_PENDING_INTERNAL_FEE = "151";
    private const string S_PRE_PRIMARY_REPORT = "153";
    private const string S_CCE_REPORT = "135";
    private const string S_SERVEY_ANALYSIS_COUNT_REPORT = "156";
    private const string S_STUDENT_FEE_REPORT = "157";
    private const string S_FINAL_REPORT_JOS = "159";
    private const string S_FINAL_REPORT_JPS = "160";
    private const string S_FINAL_REPORT_PKJC = "163";
    private const string S_FINAL_REPORT_GSS = "162";
    private const string LC_ISSUE_REGISTER = "166";
    private const string LC_ISSUE_LOG = "167";
    private const string S_AGE_CALCULATION = "169";
    private const string S_BONAFIDE_ISSUE_REGISTER = "168";
    private const string S_STUD_FINAL_RESULT_FOR_PPSN = "172";
    private const string S_STUD_FINAL_RESULT_FOR_9 = "205";
    private const string S_STUD_FINAL_RESULT_FOR_11 = "207";
    private const string S_STUD_EXAM_RESULT_PPSN = "171";
    private const string S_STUD_EXAM_RESULT_MVPS_9 = "226";
    private const string S_BANK_CHALLAN_REPORT = "170";
    private const string S_FEE_RECEIPT_DETAILS = "178";  
    private const string S_DATEWISE_Fee_COLLECTION = "179";    
    private const string S_AREAWISE_PENDINGFEE_DETAILS = "180";
    private const string S_STUDENT_PENDING_FFE_DETAILS = "181";
    private const string S_STAFF_ATTENDANCE = "182";
    private const string S_CLASSWISE_WORKING_HOURS = "184";
    private const string S_ASSEMBLY_REPORT = "185";
    private const string S_MATERIAL_ISSUE_DETAILS = "187";
    private const string S_ITEMWISE_STOCK_DETAILS = "188";
    private const string S_PRE_PRIMARY_REPORT_JOS = "192";
    private const string S_INTERNAL_FEE_RECEIPT_DETAILS = "196";
    private const string S_STUDENT_EXCESS_FEE_DETAILS = "197";
    private const string S_STUDENT_GENERAL_REGISTER_REPORT = "199";
    private const string S_STANDARDWISE_CONCESSION_REPORT = "46";
    private const string S_TERM_TOPPERS = "200";
    private const string S_MONTHWISE_STUDENT_ATTENDANCE = "201";
    private const string S_EMPLOYEE_INFORMATION_FOR_REPORT = "202";
    private const string S_STANDARDWISE_TEST_DETAILS = "204";
    private const string S_STAFF_SCREEN_ACCESS_DETAILS = "208";
    private const string S_NEXT_YEAR_PAID_FEE = "209";
    private const string S_STUDENT_ADDRESS_REPORT = "213";
    private const string S_STAFF_KID_FEE = "214";
    private const string S_NOMINAL_ROLL = "215";
    private const string S_TRANSFER_CERTIFICATE = "216";
    private const string S_CLASS_CATELOG = "217";
    private const string S_PARENT_IDENTITY_CARDS = "218";
    private const string S_TEACHER_UDISE_DETAILS = "219";
    private const string S_UDISE_DETAILS = "220";
    private const string S_MARK_ENTRY_STATUS = "221";
    private const string S_MARK_ENTRY_FORM_REPORT = "222";
    private const string S_TESTWISE_SUBJECT_MARKS = "223";
    private const string S_STUDENT_HEALTH_DETAILS = "225";
    private const string S_STOPWISE_STUDENT_PENDING_FEE = "228";
    private const string S_STUDENT_FINAL_PROGRESS_REPORT_PEMS = "229";
    private const string S_EXAM_CONFIG_DETAILS = "230";
    private const string S_STUDENT_TRANSFER_DETAILS = "231";
    private const string S_USER_LOGIN_DETAILS = "232";
    private const string S_PRELIM_RESULT_SHEET = "233";
    private const string S_RTE_STUDENT_LIST = "128";
    private const string S_GRADUTY_REPORT_DETAILS = "234";
    private const string S_STUDENT_REGISTRATION_DEATILS = "235";
    private const string S_EXTERNAL_STUDENT_FEE_DETAILS = "236";
    private const string S_USERWISE_LOGIN_DURATION_DETAILS = "237";
    private const string S_CCE_REPORT_GRADE = "238";
    private const string S_STUDENT_REFUND_FEE_DETAILS = "239";
    private const string S_STUDENT_COUNT_LEARNING_OUTCOME = "240";
    private const string S_PAY_SCALE_STATEMENT = "241";
    private const string S_LC_NOT_AVAILABLE_MSG = "Leaving Certificate details of this Student are not available.";
    private const string S_TC_NOT_AVAILABLE_MSG = "Transfer Certificate details of this Student are not available.";
    private const string S_ERR_MSG = "Student not found.";
    private const string S_STUDENT_PROGRESS_REPORT_CBSE = "242";
    private const string S_HOUSEWISE_STUDENT_DETAILS = "243";
    private const string STUDENT_DOCUMNET_STATUS_DETAILS = "244";
    private const string S_MONTHLY_FEE_COLLECTION_DETAILS = "245";
    private const string S_STUDENT_PAID_FEE_DETAILS = "246";
    private const string S_EXPORT_FEE_DETAILS = "247";
    private const string S_COSCHOLASTIC_SUBJECT_MARK_DETAILS = "248";
    private const string S_STAFF_BIRTHDAY_LIST = "249";
    private const string S_PERIODIC_TEST_MARK_DETAILS = "250";
    private const string S_EXAMWISE_REPORT_CARD = "251";
    private const string S_STUDENTS_FEE_DETAILS_REPORT = "252";
    private const string S_LAST_ACADEMICYEAR_FEE_DETAILS = "253";
    private const string S_STUDENT_SA_ONE_REPORT_1stTO4th = "254";
    private const string S_STUDENT_SA_ONE_REPORT_5thTO8th = "255";
    private const string S_CATEGORYWISE_ITEM_BARCODE = "256";
    private const string S_STUDENT_OBSERVATION_REPORT = "257";
    private const string S_STUDENT_NEWADMISSION_DETAILS_EXPORT = "258";
    private const string S_STUDENT_ALL_ACADEMICS_PENDING_FEE = "259";
    private const string S_EXPORT_ADMISSION_DETAILS = "260";
    private const string S_EMPLOYEE_INFORMATION_DETAILS = "261";
    private const string S_CLASSWISE_STUDNET_PAID_FEE_REPORT = "20";
    private const string S_STANDARDWISE_FEE_COLLETION = "19";
    private const string S_STANDARDWISE_LATE_FEE_COLLECTION = "45";
    private const string S_LECTUREWISE_STUDENT_ATTENDANCE = "262";
    private const string S_TEACHER_JOINING_DATE = "263";      //Teacher joining date related
    private const string S_BONAFIDE_CERTIFICATE_REPORT_FOR_TBS_ID = "264"; //
    private const int I_PARAMETER_FILTER = 4;
    private const int I_PARAM_ORDER_BY_FLD = 5;
    private const string S_LEAVING_CERTIFICATE_10TH_NPS_ID = "265";     // NPS 10th LC
    public const string S_OLD_YEAR_PENDING_FEES_STUDENTS_LIST = "266";
    private const string S_DYNAMIC_PENDING_FEE_REPORT = "267";    //dynamic pending fee report
    private const string S_USER_RETIREMENT_DETAILS_REPORT = "268" ; // user retirement details
    private const string S_STAFF_LEAVE_DETAILS_EXPORT = "269";
    private const string S_STUDENT_STREAM_DETAILS = "270";
    private const string S_GST_Invoice_Details = "271";
    private const string S_CLASSWISE_ATTENDANCE_AVERAGE_REPORT = "272";
    private const string S_STUD_TERMWISE_RESULT = "273";
	private const string S_CAUTION_MONEY_PAYMENT_DETAILS = "275";
    private const string S_TRACKED_UPDATED_STUDENT_DETAILS = "274";
    private const string S_FEE_RECONCILIATION_REPORT = "276";
    private const string S_TRANSPORT_READING_ALLOCATION = "176";    
    private const string S_STUDENT_TERM1_PROGRESS_REPORT="277";
    private const string S_MATERIAL_ISSUE_DETAILS_BY_USER = "278";
    private const string S_USER_SALARY_DETAILS = "279";
    private const string S_STUDENT_TERM1_PROGRESS_REPORT_PPSN = "280";
    private const string S_PRELIM_REPORT_PP = "281";
    private const string S_ANNUAL_CATEGORYWISE_ATTENDANCE_REPORT = "282";
    private const string S_ADMISSION_CANCELLATION_FORM = "283";
    private const string S_BUS_ATTENDANCE = "284";
    private const string S_TRANSPORT_NOTIFICATIONS = "285";
    private const string S_ANNUAL_INCREMENT_LETTER = "286";
    private const string S_CAUTION_MONEY_ADJUSTMENT_AMOUNT = "287";
	private const string S_INAUGURAL_CERTIFICATE = "288";
    private const string S_PENDING_FEE_STATEMENT_FOR_ALL_ACADEMICS_PPSN = "290";
    private const string S_EMPLOYMENT_CONFIRMATION_LETTER = "291";
    private const string S_EMPLOYMENT_CONFIRMATION_LETTER_IN_DETAILS = "292";
    private const string S_PARENT_OCCUPATION_DETAILS = "293";
    private const string S_USER_PAYROLL_DETAILS = "294";
    private const string S_USER_PAYROLL_SALARY_DETAILS = "298";
    private const string S_STUDENT_FINAL_PROGRESS_REPORT_MNS = "296";
    private const string S_STUDENT_BONAFIDE_CERTIFICATE_VPMCPS = "297";
    private const string S_STUDENT_FEE_DETAILS = "300";
    private const string S_STUDENT_FEE_CONSOLIDATED_DETAILS = "302";
    private const string S_TEST_CONSOLIDATED_REPORT = "303";
    private const string S_TEST_TYPE_EXAM_RESULT = "304";
	private const string S_CLASSWISE_EXAM_PERFORMANCE = "305";
    private const string S_ANNUAL_CONSOLDATED_SUB_TYPE_REPORT_SNS = "306";
    private const string S_FEE_RECONCILIATION_REPORT_PPSH = "308";
    private const string S_FINAL_PROGRESS_CARD_SNS_11_12 = "310";
    private const string S_HOLISTIC_FINAL_PROGRESS_CARD = "312";
    private const string S_EXAM = "Exam";
    private const string S_STANDARDWISE = "Standardwise";
    private const string S_CLASSWISE = "Classwise";
    private const string S_ALL = "-- All --";
    private const int I_TEST_TOPPER_ROW = 4;
    private const string S_TYPE = "Type";
    private const string S_TEST_ID = "Schoolwise_Test_Id";
    private const string S_TEST_NAME = "Schoolwise_Test_Name";
    private const string S_DIVISION = "Division";
    private const string S_STANDARD = "Standard";
    private const string FULL_ACCESS_REPORTS = "FullAccessReports";
    private const string S_EXPORT_FEE_DETAILS_SNS = "311";
    private const string S_TERM_PROGRESS_REPORT_PIONEER = "315";
    private string S_SHEET_NAME = "ResultSheet";
    private const string S_EXAMWISE_MARK_DETAILS = "313";
    private const string S_EXPORT_STUDENT_MONTHLY_STATUS = "316";
    private const string S_STUDENT_INTERNAL_FEE_PAID_EXAM_DETAILS= "317";
    private const string S_EXPORT_STUDENTS_RECEIPTS_DETAILS = "314";
    private const string S_CA_RECONSOLIDATION_DETAILS = "318";
    private const string S_STUDENT_PENDING_FEE_REMINDER = "320";
    private const string S_PREPRIMARY_STUDENT_TERM1 = "321";
    private const string S_HOLISTIC_REPORT_FOR1TO3_PPSH = "322";
    private const string S_TESTWISE_SUBJECT_TOPPERS = "323";
    private const string S_STUDENT_HALF_YEARLY_3TO9 = "324";
    private const string S_STUDENT_YEARWISE_PENDING_FEE_COUNT_DETAILS = "325";
    private const string S_VEHICLES_FUEL_MAINTENANCE_EXPENSES = "326";
    private const string S_Holistic_Progress_Report_6to7_SNS = "327";
    private const string S_STUDENT_NEW_IDENTITY_CARDS = "328";
    private const string S_USER_ROLEWISE_IDENTITY_CARDS_NEW = "329";
    private DataTable oDTUderDetails;
    private DataTable moDTStudentFinalProgressReports;

    private bool mbExportReportZip = false;
    private int iCallCount = 0;

    #endregion

    public bool IsAnnualConsoldatedReportOf9thSVP
    {
        get
        {
            var oDropDownList = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
            if (msReportID == S_ANNUAL_CONSOLDATED_REPORT && oDropDownList.SelectedItem.Text == "9")
                return true;
            else
                return false;
        }
    }

    #region Events

    /// <summary>
    /// 	This method is used to generate tree view as well to show parameter grid depends on querystring.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            txtName.Focus();
            if (!IsPostBack)
            {

                HideControls();
                GenerateReportTree();
                SetJavascriptAttributes();
                if (QueryString["ReportID"] != null && (QueryString["ReportID"] == S_INSURANCE_DETAILS || QueryString["ReportID"] == S_PROVIDENT_FUND_DETAILS || QueryString["ReportID"] == S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE || QueryString["ReportID"] == S_ANNUAL_CONSOLDATED_REPORT_SS || QueryString["ReportID"] == S_EARNINGS_DEDUCTIONS || QueryString["ReportId"] == S_ADDITIONAL_FEETYPE_PAYMENT_DETAILS || QueryString["ReportId"] == S_INTERNAL_FEE || QueryString["ReportId"] == S_PENDING_INTERNAL_FEE || QueryString["ReportId"] == S_CLASSWISE_STUDENT_LIST || QueryString["ReportId"] == S_EMPLOYEE_DETAILS || QueryString["ReportId"] == S_STAFF_ATTENDANCE || QueryString["ReportId"] == S_NET_SALARY || QueryString["ReportId"] == S_NETBANKING_REPORT || QueryString["ReportId"] == S_STANDARDWISE_CONCESSION_REPORT || (QueryString["ReportId"] == S_ANNUAL_CONSOLDATED_REPORT && miSchoolId != Constants.SchoolId.HSP.ToInt()) || (QueryString["ReportId"] == S_RESULTSHEET && miSchoolId == Constants.SchoolId.PPS.ToInt())) || QueryString["ReportId"] == S_FEE_RECONCILIATION_REPORT || QueryString["ReportID"] == S_USER_SALARY_DETAILS || QueryString["ReportId"] == S_EMPLOYMENT_CONFIRMATION_LETTER || QueryString["ReportId"] == S_EMPLOYMENT_CONFIRMATION_LETTER_IN_DETAILS || QueryString["ReportID"] == S_TRANSPORT_READING_ALLOCATION || QueryString["ReportID"] == S_USERROLEWISE_TRAVELLER_DETAILS || QueryString["ReportID"] == S_STOPWISE_TRANSPORT_DETAILS || QueryString["ReportID"] == S_CLASSWISE_EXAM_PERFORMANCE)
                {
                    if (QueryString["ReportID"] == S_PROVIDENT_FUND_DETAILS || QueryString["ReportID"] == S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE || QueryString["ReportID"] == S_ANNUAL_CONSOLDATED_REPORT_SS || QueryString["ReportID"] == S_EARNINGS_DEDUCTIONS || QueryString["ReportId"] == S_ADDITIONAL_FEETYPE_PAYMENT_DETAILS || QueryString["ReportId"] == S_INTERNAL_FEE || QueryString["ReportId"] == S_PENDING_INTERNAL_FEE || QueryString["ReportId"] == S_CLASSWISE_STUDENT_LIST || QueryString["ReportId"] == S_EMPLOYEE_DETAILS || QueryString["ReportId"] == S_STAFF_ATTENDANCE || QueryString["ReportId"] == S_NET_SALARY || QueryString["ReportId"] == S_NETBANKING_REPORT || QueryString["ReportId"] == S_STANDARDWISE_CONCESSION_REPORT || QueryString["ReportId"] == S_RESULTSHEET || QueryString["ReportId"] == S_ANNUAL_CONSOLDATED_REPORT || QueryString["ReportId"] == S_FEE_RECONCILIATION_REPORT || QueryString["ReportID"] == S_USER_SALARY_DETAILS || QueryString["ReportID"] == S_TRANSPORT_READING_ALLOCATION || QueryString["ReportID"] == S_USERROLEWISE_TRAVELLER_DETAILS || QueryString["ReportID"] == S_STOPWISE_TRANSPORT_DETAILS || QueryString["ReportID"] == S_CLASSWISE_EXAM_PERFORMANCE)
                        DDLFormatType.Items.Remove("MS Word");
                    lblSelectType.Visible = true;
                    DDLFormatType.Visible = true;
                }
            }

            if (QueryString["ReportID"] != null)
            {
                DisplayReportFilters();
                if (QueryString["IsSearchGridConsidered"] != null && QueryString["IsSearchGridConsidered"].ToInt() > 0)
                {
                    trITReport.Visible = true;
                    miIsSearchGridConsidered = QueryString["IsSearchGridConsidered"].ToInt();
                    if (miIsSearchGridConsidered > 1)
                    {
                        lblName.Text = "Staff Name :";
                        tdName.Width = "80px";
                    }
                    else if (miIsSearchGridConsidered == Constants.I_ONE && msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS)
                        SetLabelText();
                }
                else
                    trITReport.Visible = false;

                if (Constants.UserRoles.Student != moUserRole && trITReport.Visible && miIsSearchGridConsidered == 1)
                    trITReport.Visible = true;
                else if ((moUserRole == Constants.UserRoles.Admin || hidHasFullAccess.Value != "0") && miIsSearchGridConsidered > 1)
                    trITReport.Visible = true;
                else
                    trITReport.Visible = false;
            }
			
            SetDefaultProperties();
        }
        catch (Exception ex)
        {
            lblErrorMesg.Visible = true;
            lblErrorMesg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    ///		This event is used to close and dispose the report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
            if (crReportDocument != null)
            {
                crReportDocument.Close();
                crReportDocument.Dispose();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// 	This event is used to set visibility of edit image.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdStudentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS)
            {
                if (e.Row.RowIndex >= Constants.I_ZERO)
                {
                    if (optSearchByBook.Checked)
                        grdStudentDetails.Columns[3].Visible = true;
                    else
                    {
                        grdStudentDetails.Columns[3].Visible = true;
                        var ohidUserRoleId = e.Row.Cells[2].FindControl("hidUserRoleId") as HiddenField;
                        ohidUserRoleId.Value = oDTUderDetails.Rows[e.Row.RowIndex][9].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// 	This event is used to set grid view column name as per the filter selection.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdStudentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS && grdStudentDetails.HeaderRow != null)
            {
                if (optSearchByBook.Checked)
                {
                    grdStudentDetails.HeaderRow.Cells[Constants.I_ZERO].Text = "Accession No.";
                    grdStudentDetails.HeaderRow.Cells[Constants.I_ONE].Text = "Book Name";
                    grdStudentDetails.HeaderRow.Cells[Constants.I_TWO].Text = "Author Name";
                }
                else
                {
                    grdStudentDetails.HeaderRow.Cells[Constants.I_ZERO].Text = "Reg.No/Employee No.";
                    grdStudentDetails.HeaderRow.Cells[Constants.I_ONE].Text = "Class/Designation";
                    grdStudentDetails.HeaderRow.Cells[Constants.I_TWO].Text = "User Name";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// 	This event is used to display report.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void btnDisplayReport_Click(object sender, EventArgs e)
    {
        try
        {
            iCallCount++;
            lblNorecord.Visible = false;

            //This method returns string with report filter values.            
            string sFilterString = GetReportFilter();

            if (msReportID == S_XSEED_REPORT && Settings.IsAaryanSchool || miSchoolId == Constants.SchoolId.PPSH.ToInt())
                sFilterString = sFilterString.Replace("Xseed.", "");

            //This method is used to check report data availability.
            if (msReportID != S_STUD_TERMWISE_RESULT && msReportID != "115" && msReportID != S_BONAFIDE_CERTIFICATE_REPORT_ID && msReportID != S_BONAFIDE_CERTIFICATE_REPORT_FOR_TBS_ID && msReportID != S_DATEWISE_Fee_COLLECTION && msReportID != S_BONAFIDE_CERTIFICATE_REPORT_FOR_SS_ID && msReportID != S_BONAFIDE_CERTIFICATE_REPORT_FOR_PPSH_ID && msReportID != S_STUD_FINAL_RESULT && msReportID != S_STUD_FINAL_RESULT_PPSN && msReportID != S_STUD_FINAL_RESULT_MCPS && msReportID != S_EXAM_RESULT && msReportID != S_EXAM_RESULT_FBS && msReportID != S_EXAM_RESULT_PPSN && msReportID != S_STUD_TERM2_RESULT && msReportID != S_BONAFIDE_CERTIFICATE_REPORT_FOR_JOS_ID && msReportID != S_BANK_CHALLAN_REPORT && msReportID != S_STUD_EXAM_RESULT_PPSN && msReportID != S_MARK_ENTRY_FORM_REPORT && msReportID != S_STUD_EXAM_RESULT_MVPS_9 && msReportID != S_USER_LOGIN_DETAILS && msReportID != S_PRELIM_RESULT_SHEET && msReportID != S_STUD_FINAL_RESULT_PPSH_Old && msReportID != S_STUD_FINAL_RESULT_FOR_9 && msReportID != S_FINAL_PROGRESS_CARD_SNS_11_12 && !(msReportID == S_ANNUAL_CONSOLDATED_REPORT && miSchoolId == Constants.SchoolId.HSP.ToInt()) && msReportID != STUDENT_DOCUMNET_STATUS_DETAILS && msReportID != S_MONTHLY_FEE_COLLECTION_DETAILS && msReportID != S_STUDENT_FEE_REPORT && msReportID != S_STUDENT_NEWADMISSION_DETAILS_EXPORT && msReportID != S_LEAVING_CERTIFICATE_10TH_NPS_ID && msReportID != S_DYNAMIC_PENDING_FEE_REPORT && msReportID != S_PRELIM_REPORT_PP && msReportID != S_STUDENT_OBSERVATION_REPORT && msReportID != S_STUD_FINAL_RESULT_FOR_PPSN && msReportID != S_TRANSPORT_NOTIFICATIONS && msReportID != S_INAUGURAL_CERTIFICATE && msReportID != S_PENDING_FEE_STATEMENT_FOR_ALL_ACADEMICS_PPSN && msReportID != S_EMPLOYMENT_CONFIRMATION_LETTER && msReportID != S_EMPLOYMENT_CONFIRMATION_LETTER_IN_DETAILS && msReportID != S_PARENT_OCCUPATION_DETAILS && msReportID != S_USER_PAYROLL_DETAILS && msReportID != S_USER_PAYROLL_SALARY_DETAILS && msReportID != S_STUDENT_FINAL_PROGRESS_REPORT_MNS && msReportID != S_STUDENT_BONAFIDE_CERTIFICATE_VPMCPS && msReportID != S_STUDENT_FEE_DETAILS && msReportID != S_STUDENT_FEE_CONSOLIDATED_DETAILS && msReportID != S_TEST_CONSOLIDATED_REPORT && msReportID != S_TEST_TYPE_EXAM_RESULT && msReportID != S_STUDENT_TERM1_PROGRESS_REPORT && msReportID != S_FEE_RECONCILIATION_REPORT_PPSH && msReportID != S_EXPORT_FEE_DETAILS_SNS && msReportID != S_HOLISTIC_FINAL_PROGRESS_CARD && msReportID != S_TERM_PROGRESS_REPORT_PIONEER && msReportID != S_EXPORT_STUDENT_MONTHLY_STATUS && msReportID != S_EXPORT_STUDENTS_RECEIPTS_DETAILS && msReportID != S_CA_RECONSOLIDATION_DETAILS && msReportID != S_HOLISTIC_REPORT_FOR1TO3_PPSH && msReportID != S_STUDENT_HALF_YEARLY_3TO9 && msReportID != S_STUDENT_YEARWISE_PENDING_FEE_COUNT_DETAILS && msReportID != S_MUSTER_REPORT && msReportID!=S_VEHICLES_FUEL_MAINTENANCE_EXPENSES && msReportID != S_Holistic_Progress_Report_6to7_SNS)
                IsReportEmpty(sFilterString);

            //set export option for opening reports.
            string sFormatType = DDLFormatType.SelectedItem.Text;

            // Check if selected report is "Studentwise Progress Report" or "Final Progress Card" for PPSN and student filter is "All",
            // then consider this report to be exported as a zip file.
            //if ((msReportID == S_STUD_EXAM_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_PPSN))
            //{
            //    ComboRpt cmbStudents = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
            //    if (cmbStudents.SelectedValue == Constants.S_ZERO)
            //    {
            //        if (hidIsReportGenerated.Value == Constants.S_NO)
            //        {
            //            mbExportReportZip = true;
            //            string sDirName = msReportID + DateTime.Now.ToString(Constants.S_DATE_FORMAT_TIMESTAMP);
            //            string sDirPath = Server.MapPath(Path.Combine(Constants.S_DOWNLOADS_FOLDER_RELATIVE_PATH + "/Reports", sDirName));

            //            if (!Directory.Exists(sDirPath))
            //            {
            //                Directory.CreateDirectory(sDirPath);
            //            }

            //            String[] sFilters = sFilterString.Split('@');

            //            for (int cnt = 0; cnt < moDTStudentFinalProgressReports.Rows.Count; cnt++)
            //            {
            //                // Update student id for the in the filters string.
            //                sFilters[3] = "({USP_StudentFinalProgressReportPPSN;1.StudentId}=" + moDTStudentFinalProgressReports.Rows[cnt]["Student_Id"] + ")";
            //                string finalFilterString = string.Join("@", sFilters);
            //                string sFilePath = sDirPath + "//" + moDTStudentFinalProgressReports.Rows[cnt]["StudentName"].ToString() + ".pdf";

            //                //This method is used to display reports by creating report selection formula.               
            //                if (lblErrorMesg.Text == string.Empty && lblNorecord.Text == string.Empty)
            //                    DisplayReport(finalFilterString, sFormatType, sFilePath);
            //            }

            //            CreateZipFile(sDirName);
            //        }
            //        else
            //        {
            //            hidIsReportGenerated.Value = Constants.S_NO;
            //        }
            //    }
            //    else
            //    {
            //        if (lblErrorMesg.Text == string.Empty && lblNorecord.Text == string.Empty)
            //            DisplayReport(sFilterString, sFormatType);
            //    }
            //}
            //else
            //{
            if ((msReportID == S_RESULTSHEET || msReportID == S_PRELIM_RESULT_SHEET || (msReportID == S_ANNUAL_CONSOLDATED_REPORT && DDLFormatType.SelectedItem.Text.ToLower() == "excel")) && miSchoolId == Constants.SchoolId.SVP.ToInt())
            {
                if (iCallCount == 1)
                    ExportResultSheet(sFilterString);
            }
            else if (msReportID == S_ANNUAL_CONSOLDATED_REPORT && miSchoolId == Constants.SchoolId.HSP.ToInt())
            {
                if (iCallCount == 1)
                    ExportAnnualConsolidatedReportHSP(sFilterString);
            }
            else if (msReportID == S_MUSTER_REPORT && miSchoolId == Constants.SchoolId.SS.ToInt())
            {
                if (iCallCount == 1)
                    ExportMusterRollReport(sFilterString);
            }
            else if (msReportID == S_LECTUREWISE_STUDENT_ATTENDANCE)
            {
                if (iCallCount == 1)
                    ExportLecturewiseStudentAttendance(sFilterString);
            }
            else if (msReportID == S_STUDENT_PAID_FEE_DETAILS && miSchoolId == Constants.SchoolId.BMFS.ToInt())
            {
                if (iCallCount == 1)
                    ExportStudentPaidFeeDetailsReport(sFilterString);
            }
            else if (msReportID == S_DYNAMIC_PENDING_FEE_REPORT)  //
            {
                if (iCallCount == 1)
                    ExportStudentPendingFeeDetailsReport(sFilterString);
            }
            else if (msReportID == S_STUDENT_FEE_DETAILS)
            {
                if (iCallCount == 1)
                    StudentPaidFeeDetailsReportVP(sFilterString);
            }
            else if (msReportID == S_STUDENT_INTERNAL_FEE_PAID_EXAM_DETAILS)
            {
                if (iCallCount == 1)
                    StudentInternalPaidFeeExamDetailsReport(sFilterString);
            }

            else if (msReportID == S_STUDENT_YEARWISE_PENDING_FEE_COUNT_DETAILS)
            {
                if (iCallCount == 1)
                    StudentsYearwisePendingfeecountReport(sFilterString);
            }
            else if (msReportID == S_TEST_CONSOLIDATED_REPORT)
            {
                if (iCallCount == 1)
                    ExportMarkDetailsForTestwiseReport(sFilterString);
            }
            else
            {
                //This method is used to display reports by creating report selection formula.               
                if (lblErrorMesg.Text == string.Empty && lblNorecord.Text == string.Empty)
                    DisplayReport(sFilterString, sFormatType);
            }
            //}
        }
        catch (ThreadAbortException)
        {
        }
        catch (NoRecordFoundException oEx)
        {
            lblErrorMesg.Visible = !oEx.Message.IsNullOrEmpty();
            lblErrorMesg.Text = oEx.Message;
        }
        catch (ApplicationException ex)
        {
            lblSNSErrorMsg.Visible = true;
            lblSNSErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblSNSErrorMsg.Text = ex.Message;                        
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }        
    }



    /// <summary>
    /// 	This method is used to fill dependant control on parent control's change event.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    private void oDropDownList_ComboChangeEvent(object sender, EventArgs e)
    {
        try
        {
            for (int iGridCount = 0; iGridCount < grdDisplayParameter.Rows.Count; iGridCount++)
            {
                if (msReportID == S_STUD_FINAL_RESULT || msReportID == S_STUD_FINAL_RESULT_PPSH || msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std || msReportID == S_STUD_FINAL_RESULT_PPSH_Old || msReportID == S_STUD_FINAL_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_MCPS || msReportID == S_EXAM_RESULT || msReportID == S_STUD_TERM1_RESULT || msReportID == S_STUD_TERMWISE_RESULT || msReportID == S_EXAM_RESULT_FBS || msReportID == S_EXAM_RESULT_PPSN || msReportID == S_STUD_PRELIMINARY_RESULT || msReportID == S_STUD_TERM2_RESULT || msReportID == S_STUD_EXAM_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_9 || msReportID == S_STUD_FINAL_RESULT_FOR_11 || msReportID == S_STUD_EXAM_RESULT_MVPS_9 || msReportID == S_STUDENT_PROGRESS_REPORT_CBSE || msReportID == S_COSCHOLASTIC_SUBJECT_MARK_DETAILS || msReportID == S_PERIODIC_TEST_MARK_DETAILS || msReportID == S_STUDENTS_FEE_DETAILS_REPORT || msReportID == S_STUDENT_SA_ONE_REPORT_1stTO4th || msReportID == S_STUDENT_SA_ONE_REPORT_5thTO8th || msReportID == S_STUDENT_OBSERVATION_REPORT || msReportID == S_FINAL_PROGRESS_CARD_SNS_11_12)
                {
                    var oTextBox = grdDisplayParameter.Rows[iGridCount].FindControl("txtRptParameter") as TextBox;
                    oTextBox.Text = string.Empty;
                }
            }
            if (msReportID == S_TASK_DETAILS)
            {
                var cmbAssigenType = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                var cmbDesignation = grdDisplayParameter.Rows[Constants.I_ONE].FindControl("DDLRptParameter") as ComboRpt;
                var cmbUser = grdDisplayParameter.Rows[Constants.I_TWO].FindControl("DDLRptParameter") as ComboRpt;
                var cmbTaskType = grdDisplayParameter.Rows[Constants.I_THREE].FindControl("DDLRptParameter") as ComboRpt;
                var cmbTaskStatus = grdDisplayParameter.Rows[Constants.I_FOUR].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbAssigenType.SelectedValue == Constants.I_ZERO.ToString())
                {
                    cmbDesignation.SelectedValue = cmbUser.SelectedValue = cmbTaskType.SelectedValue = cmbTaskStatus.SelectedValue = Constants.I_ZERO.ToString();
                    cmbDesignation.Enabled = false;
                    cmbUser.Enabled = false;
                }
                else
                    cmbDesignation.Enabled = cmbUser.Enabled = true;
                if (cmbDesignation.SelectedValue == Constants.I_ZERO.ToString())
                    cmbUser.SelectedValue = Constants.I_ZERO.ToString();
            }
            if (msReportID == S_STOPWISE_TRANSPORT_DETAILS)
            {
                var cmbBusNo = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                var cmbShift = grdDisplayParameter.Rows[Constants.I_ONE].FindControl("DDLRptParameter") as ComboRpt;
                var cmbRoute = grdDisplayParameter.Rows[Constants.I_TWO].FindControl("DDLRptParameter") as ComboRpt;
                var cmbStop = grdDisplayParameter.Rows[Constants.I_THREE].FindControl("DDLRptParameter") as ComboRpt;

                if (cmbBusNo.SelectedValue == Constants.I_ZERO.ToString() || sender.Equals(cmbBusNo))
                {
                    cmbShift.SelectedValue = cmbRoute.SelectedValue = cmbStop.SelectedValue = Constants.I_ZERO.ToString();
                    cmbShift.Enabled = cmbRoute.Enabled = cmbStop.Enabled = false;
                }
                if (cmbShift.SelectedValue == Constants.I_ZERO.ToString() || sender.Equals(cmbShift))
                {
                    cmbRoute.SelectedValue = cmbStop.SelectedValue = Constants.I_ZERO.ToString();
                    cmbRoute.Enabled = cmbStop.Enabled = false;
                }
                if (cmbRoute.SelectedValue == Constants.I_ZERO.ToString() || sender.Equals(cmbShift))
                {
                    cmbStop.SelectedValue = Constants.I_ZERO.ToString();
                    cmbStop.Enabled = false;
                }
            }
            if (msReportID == S_USERROLEWISE_TRAVELLER_DETAILS || msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS)
            {
                var cmbUserRole = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                var cmbStd = grdDisplayParameter.Rows[Constants.I_ONE].FindControl("DDLRptParameter") as ComboRpt;
                var cmbDiv = grdDisplayParameter.Rows[Constants.I_TWO].FindControl("DDLRptParameter") as ComboRpt;
                //IF user role is student then only standard combo is enabled.
                if (((cmbUserRole.SelectedValue == Constants.I_ZERO.ToString() || cmbUserRole.SelectedValue != Constants.I_THREE.ToString()) || sender.Equals(cmbUserRole)) && cmbUserRole.SelectedValue != "9")
                {
                    cmbStd.SelectedValue = cmbDiv.SelectedValue = Constants.I_ZERO.ToString();
                    cmbStd.Enabled = cmbDiv.Enabled = false;
                }
            }

            if (msReportID == S_IT_RECONCILIATION_RPT_ID)
            {
                var cmbFinancialYear = grdDisplayParameter.Rows[Constants.I_THREE].FindControl("DDLRptParameter") as ComboRpt;
                var cmbAcademicYear = grdDisplayParameter.Rows[Constants.I_FOUR].FindControl("DDLRptParameter") as ComboRpt;
                if (sender.Equals(cmbFinancialYear))
                    cmbAcademicYear.SelectedValue = Constants.S_ZERO;
                else if (sender.Equals(cmbAcademicYear))
                    cmbFinancialYear.SelectedValue = Constants.S_ZERO;                                
            }
            
            // Refresh dependent combobox and get values of dependent commboboxes.
            int iDivisionId = 0;
            bool bIsDivisionRequired = true;
            var oDropDownListSender = sender as ComboRpt;
            int iParentRptFldId = oDropDownListSender.ReportFieldId;
            lblNorecord.Visible = false;
            lblErrorMesg.Visible = false;

            moDictFiledDatatype.Clear();
            //This loop is to read each control one by one from grid.
            var oHashFilterParameters = new Hashtable();
            for (int iGridRowCount = 0; iGridRowCount < grdDisplayParameter.Rows.Count; iGridRowCount++)
            {
                //If current control is dependent control then do this.
                string sIsDependentFlag = grdDisplayParameter.DataKeys[iGridRowCount]["Is_Dependent"].ToString();
                if (sIsDependentFlag == Constants.C_YES.ToString() && bIsDivisionRequired)
                {
                    int iParentFieldId = grdDisplayParameter.DataKeys[iGridRowCount]["Parent_Field_Id"].ToString().ToInt();
                    string sParentFieldIdFilterString = grdDisplayParameter.DataKeys[iGridRowCount]["Filter_Field_Name"].ToString();
                    //To preserve ParentFoeldId  
                    int iParentId = iParentFieldId;
                    string sFilterString = sParentFieldIdFilterString;

                    //If current control's parent_field_Id and parent control's report_field_id is same then fill 
                    //child control as per parent control.
                    bool bIsSubjectReport = false;

                    /**************Vinod****************/
                    string sAdditionalParentFieldsNames = Convert.ToString(grdDisplayParameter.DataKeys[iGridRowCount]["Additional_Filter_Field_Name"]);
                    string sAdditionalParent_Ids = Convert.ToString(grdDisplayParameter.DataKeys[iGridRowCount]["Additional_Parent_Field_Id"]);
                    string[] sArrayAdditionalParent_Ids = sAdditionalParent_Ids.Split(',');
                    string[] sArrayAdditionalParentFieldsNames = sAdditionalParentFieldsNames.Split(',');
                    int iIndex = 0;
                    //When Parentfielsid is in AddiionalParentField array.
                    if (sArrayAdditionalParent_Ids != null && sArrayAdditionalParent_Ids.Contains(iParentRptFldId.ToString()) && iParentFieldId != iParentRptFldId)
                    {
                        foreach (string sItem in sArrayAdditionalParent_Ids)
                        {
                            iIndex++;
                            //Set ParentFieldId from AddidionalFiels array to Parentfield
                            if (sItem != iParentRptFldId.ToString())
                                continue;
                            iParentFieldId = iParentRptFldId;
                            sParentFieldIdFilterString = sArrayAdditionalParentFieldsNames[iIndex - 1];
                            //Replace Id
                            sAdditionalParent_Ids = sAdditionalParent_Ids.Replace(iParentRptFldId.ToString(), iParentId.ToString());
                            sArrayAdditionalParent_Ids[iIndex - 1] = sArrayAdditionalParent_Ids[iIndex - 1].Replace(iParentRptFldId.ToString(), iParentId.ToString());
                            //Replace Name
                            sAdditionalParentFieldsNames = sAdditionalParentFieldsNames.Replace(sParentFieldIdFilterString, sFilterString);
                            sArrayAdditionalParentFieldsNames[iIndex - 1] = sArrayAdditionalParentFieldsNames[iIndex - 1].Replace(sParentFieldIdFilterString, sFilterString);
                        }
                    }

                    /*********************************/
                    if (iParentFieldId == iParentRptFldId || bIsSubjectReport)
                    {
                        var oDropDownList = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                        string sViewName = grdDisplayParameter.DataKeys[iGridRowCount][I_VIEWNAME_INDEX].ToString();
                        if (!string.IsNullOrEmpty(sViewName))
                        {
                            //Here filter field name string is formatted and we get parameter name.
                            string sFilterFieldName = grdDisplayParameter.DataKeys[iGridRowCount]["Filter_Field_Name"].ToString();
                            /***********************/
                            if (iParentId != iParentFieldId)
                                sFilterFieldName = sParentFieldIdFilterString;
                            /***********************/
                            sFilterFieldName = sFilterFieldName.Replace("{", "[");
                            sFilterFieldName = sFilterFieldName.Replace("}", "]");
                            if (!string.IsNullOrEmpty(hidSchemaName.Value))
                                sFilterFieldName = sFilterFieldName.Replace(hidSchemaName.Value, string.Empty);
                            sFilterFieldName = sFilterFieldName.Replace(sFilterFieldName.Substring(1, sFilterFieldName.IndexOf(".")), string.Empty);
                            if (msReportID == S_SUBJECT_TOPPERS || msReportID == S_TESTWISE_SUBJECT_TOPPERS)
                                oHashFilterParameters.Add(sFilterFieldName, hidStandardId.Value);
                            else
                            {
                                if (mlstPayrollReports.Contains(msReportID) || msReportID == S_STUDENT_NOT_SELECTED_IN_LOTTERY || (msReportID == S_IT_RECONCILIATION_RPT_ID && oDropDownListSender.ReportFieldId == 203) || msReportID == S_EMPLOYEE_DETAILS || msReportID == S_EMPLOYEE_INFORMATION_FOR_REPORT || msReportID == S_REQUISITION_DETAILS || msReportID == S_USER_ROLEWISE_IDENTITY_CARDS || msReportID == S_USER_ROLEWISE_IDENTITY_CARDS_NEW || msReportID == S_TASK_DETAILS || msReportID == S_STOPWISE_TRANSPORT_DETAILS || msReportID == S_USERROLEWISE_TRAVELLER_DETAILS || msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS || msReportID == S_FEE_PAID_STUDENT_COUNT || msReportID == S_MUSTER_REPORT || msReportID == S_LECTUREWISE_STUDENT_ATTENDANCE || msReportID == S_PERFORMANCE_EVALUATION || msReportID == S_STUDENT_IDENTITY_CARDS || msReportID == S_SERVEY_ANALYSIS_COUNT_REPORT ||
                                    msReportID == S_STUDENT_FEE_REPORT || msReportID == S_BANK_CHALLAN_REPORT || msReportID == S_DATEWISE_Fee_COLLECTION || msReportID == S_STUDENT_GENERAL_REGISTER_REPORT || msReportID == S_AREAWISE_PENDINGFEE_DETAILS || msReportID == S_STUDENT_PENDING_FFE_DETAILS || msReportID == S_CLASSWISE_WORKING_HOURS || msReportID == S_MATERIAL_ISSUE_DETAILS || msReportID == S_ITEMWISE_STOCK_DETAILS || msReportID == S_STANDARDWISE_TEST_DETAILS || msReportID == S_STAFF_SCREEN_ACCESS_DETAILS || msReportID == S_PARENT_IDENTITY_CARDS || msReportID == S_CLASS_CATELOG || msReportID == S_TEACHER_UDISE_DETAILS || msReportID == S_UDISE_DETAILS || msReportID == S_MARK_ENTRY_STATUS || msReportID == S_MARK_ENTRY_FORM_REPORT || (miSchoolId == Constants.SchoolId.SS.ToInt() && msReportID == S_DATEWISE_ATTENDANCE_COUNT) || msReportID == S_STUDENT_HEALTH_DETAILS || msReportID == S_GRADUTY_REPORT_DETAILS || msReportID == S_STUDENT_REGISTRATION_DEATILS || msReportID == S_EXTERNAL_STUDENT_FEE_DETAILS || msReportID == S_USERWISE_LOGIN_DURATION_DETAILS || msReportID == S_STUDENT_REFUND_FEE_DETAILS || msReportID == S_PAY_SCALE_STATEMENT || msReportID == S_HOUSEWISE_STUDENT_DETAILS || msReportID == STUDENT_DOCUMNET_STATUS_DETAILS || msReportID == S_MONTHLY_FEE_COLLECTION_DETAILS || msReportID == S_EXPORT_FEE_DETAILS || msReportID == S_CLASSWISE_STUDNET_PAID_FEE_REPORT || msReportID == S_CLASSWISE_STUDENT_PENDING_FEE_REPORT_ID || msReportID == S_STANDARDWISE_FEE_COLLETION || msReportID == S_STANDARDWISE_LATE_FEE_COLLECTION || msReportID == S_PENDING_FEE_STUDENTLIST || msReportID == S_EXAMWISE_REPORT_CARD || msReportID == S_LAST_ACADEMICYEAR_FEE_DETAILS || msReportID == S_CATEGORYWISE_ITEM_BARCODE || msReportID == S_STUDENT_NEWADMISSION_DETAILS_EXPORT || msReportID == S_EMPLOYEE_INFORMATION_DETAILS || (msReportID == S_TEACHER_JOINING_DATE) || (msReportID == S_USER_RETIREMENT_DETAILS_REPORT) || msReportID == S_USER_SALARY_DETAILS || msReportID == S_MATERIAL_ISSUE_DETAILS_BY_USER || msReportID == S_NEXT_YEAR_PAID_FEE || msReportID == S_ANNUAL_INCREMENT_LETTER || msReportID == S_CAUTION_MONEY_ADJUSTMENT_AMOUNT || msReportID == S_INAUGURAL_CERTIFICATE || msReportID == S_PENDING_FEE_STATEMENT_FOR_ALL_ACADEMICS_PPSN || msReportID == S_EMPLOYMENT_CONFIRMATION_LETTER || msReportID == S_EMPLOYMENT_CONFIRMATION_LETTER_IN_DETAILS || msReportID == S_PARENT_OCCUPATION_DETAILS || msReportID == S_USER_PAYROLL_DETAILS || msReportID == S_USER_PAYROLL_SALARY_DETAILS || msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_MNS || msReportID == S_STUDENT_BONAFIDE_CERTIFICATE_VPMCPS || msReportID == S_STUDENT_FEE_CONSOLIDATED_DETAILS || msReportID == S_TEST_TYPE_EXAM_RESULT || msReportID == S_ANNUAL_CONSOLDATED_SUB_TYPE_REPORT_SNS || msReportID == S_CLASSWISE_EXAM_PERFORMANCE || msReportID == S_TEST_CONSOLIDATED_REPORT || msReportID == S_FEE_RECONCILIATION_REPORT_PPSH || msReportID == S_FINAL_PROGRESS_CARD_SNS_11_12 || msReportID == S_EXPORT_FEE_DETAILS_SNS || msReportID == S_EXAMWISE_MARK_DETAILS || msReportID == S_EXPORT_STUDENT_MONTHLY_STATUS || msReportID == S_EXPORT_STUDENTS_RECEIPTS_DETAILS || msReportID == S_CA_RECONSOLIDATION_DETAILS || msReportID == S_VEHICLES_FUEL_MAINTENANCE_EXPENSES || msReportID == S_STUDENT_NEW_IDENTITY_CARDS)
                                    sFilterFieldName = sFilterFieldName.Replace("[", string.Empty).Replace("]", string.Empty);
                                oHashFilterParameters[sFilterFieldName] = oDropDownListSender.SelectedValue;
                                if ((msReportID == S_MUSTER_REPORT || msReportID == S_LECTUREWISE_STUDENT_ATTENDANCE) && sFilterFieldName == "Year")
                                    if (oDropDownListSender.SelectedItem.Text == Constants.S_SELECT)
                                        oHashFilterParameters[sFilterFieldName] = 0;
                                    else
                                        oHashFilterParameters[sFilterFieldName] = oDropDownListSender.SelectedItem.Text;

                            }

                            //This method gives dataset for filter parameter.

                            // **************************** To support multiple parent fields *******************************************************************

                            string sAdditionalParentFields = Convert.ToString(grdDisplayParameter.DataKeys[iGridRowCount]["Additional_Filter_Field_Name"]);
                            string sAdditionalParentIds = Convert.ToString(grdDisplayParameter.DataKeys[iGridRowCount]["Additional_Parent_Field_Id"]);
                            if (iParentId != iParentFieldId)
                            {
                                sAdditionalParentIds = sAdditionalParent_Ids;
                                sAdditionalParentFields = sAdditionalParentFieldsNames;
                            }
                            if (!string.IsNullOrEmpty(sAdditionalParentFields))
                            {
                                string[] sArrayAdditionalFilterFields = sAdditionalParentFields.Split(',');
                                string[] sArrayAdditionalParentIds = sAdditionalParentIds.Split(',');
                                if (iParentId != iParentFieldId)
                                {
                                    sArrayAdditionalParentIds = sArrayAdditionalParent_Ids;
                                    sArrayAdditionalFilterFields = sArrayAdditionalParentFieldsNames;
                                }
                                for (int iFieldIndex = 0; iFieldIndex < grdDisplayParameter.Rows.Count; iFieldIndex++)
                                {
                                    var oDropDown = grdDisplayParameter.Rows[iFieldIndex].FindControl("DDLRptParameter") as ComboRpt;
                                    string sDataType = grdDisplayParameter.DataKeys[iFieldIndex][I_DATATYPE_INDEX].ToString().ToLower();
                                    string sReportFieldName = Convert.ToString(grdDisplayParameter.DataKeys[iFieldIndex]["Field_name"]);
                                    if (!string.IsNullOrEmpty(hidSchemaName.Value))
                                        sReportFieldName = sReportFieldName.Replace(hidSchemaName.Value, string.Empty);
                                    if (!sArrayAdditionalFilterFields.Contains(sReportFieldName))
                                        continue;
                                    string sAdditionalFilterFieldName = sReportFieldName;
                                    sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace("{", "[");
                                    sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace("}", "]");
                                    if (!string.IsNullOrEmpty(hidSchemaName.Value))
                                        sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace(hidSchemaName.Value, string.Empty);
                                    sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace(sAdditionalFilterFieldName.Substring(1, sAdditionalFilterFieldName.IndexOf(".")), string.Empty);
                                    if (msReportID == S_SUBJECT_TOPPERS || msReportID== S_TESTWISE_SUBJECT_TOPPERS)
                                        oHashFilterParameters.Add(sAdditionalFilterFieldName, hidStandardId.Value);
                                    else
                                    {
                                        if ((msReportID == S_IT_RECONCILIATION_RPT_ID && oDropDownListSender.ReportFieldId == 203) || mlstPayrollReports.Contains(msReportID) || msReportID == S_EMPLOYEE_DETAILS || msReportID == S_EMPLOYEE_INFORMATION_FOR_REPORT || msReportID == S_STAFF_ATTENDANCE || msReportID == S_STAFF_LEAVE_DETAILS_EXPORT || msReportID == S_REQUISITION_DETAILS || msReportID == S_USER_ROLEWISE_IDENTITY_CARDS || msReportID == S_USER_ROLEWISE_IDENTITY_CARDS_NEW || msReportID == S_TASK_DETAILS || msReportID == S_STOPWISE_TRANSPORT_DETAILS || msReportID == S_USERROLEWISE_TRAVELLER_DETAILS || msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS || msReportID == S_FEE_PAID_STUDENT_COUNT || msReportID == S_PERFORMANCE_EVALUATION || msReportID == S_BANK_CHALLAN_REPORT || msReportID == S_EMPLOYEE_INFORMATION_DETAILS || msReportID == S_USER_PAYROLL_DETAILS || msReportID == S_USER_PAYROLL_SALARY_DETAILS || msReportID == S_STUDENT_BONAFIDE_CERTIFICATE_VPMCPS || msReportID == S_STUDENT_FEE_CONSOLIDATED_DETAILS || msReportID == S_TEST_TYPE_EXAM_RESULT || msReportID == S_ANNUAL_CONSOLDATED_SUB_TYPE_REPORT_SNS || msReportID == S_FEE_RECONCILIATION_REPORT_PPSH || msReportID == S_EXAMWISE_MARK_DETAILS || msReportID == S_CLASSWISE_EXAM_PERFORMANCE)
                                            sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace("[", string.Empty).Replace("]", string.Empty);
                                        if (sDataType != S_DATETIME)
                                            oHashFilterParameters[sAdditionalFilterFieldName] = oDropDown.SelectedValue;
                                        else
                                        {
                                            var oPopCalendar = grdDisplayParameter.Rows[iFieldIndex].FindControl("CalenderRptParameter") as PopCalendar;
                                            oHashFilterParameters[sAdditionalFilterFieldName] = oPopCalendar.SelectedDate;
                                        }
                                        moDictFiledDatatype[sAdditionalFilterFieldName] = sDataType;

                                        if (msReportID == S_STAFF_LEAVES && (hidHasFullAccess.Value == Constants.S_ZERO && moUserRole != Constants.UserRoles.Admin))
                                            oHashFilterParameters["UserId"] = miUserId;

                                        if (msReportID == S_LEAVE_BALANCE && (hidHasFullAccess.Value == Constants.S_ZERO && moUserRole != Constants.UserRoles.Admin))
                                            oHashFilterParameters["UserId"] = miUserId;
                                    }
                                }
                            }

                            if (mlstUserAccessPayrollReports.Contains(msReportID) && (hidHasFullAccess.Value == Constants.S_ZERO && moUserRole != Constants.UserRoles.Admin))
                            {
                                int I_STAFF_GROUP_ID = 2;
                                int I_USER_ID = 3;

                                if (msReportID == S_FORM_NO_16)
                                {
                                    I_STAFF_GROUP_ID = 1;
                                    I_USER_ID = 2;
                                }

                                if (iGridRowCount == I_STAFF_GROUP_ID || iGridRowCount == I_USER_ID)
                                    oHashFilterParameters["UserId"] = miUserId;
                            }

                            // ***********************************************************************************************************************************
                            FillFilterParametersCombo(oDropDownList, oHashFilterParameters, iGridRowCount);

                            //For selecting current Finicial year in Combo box
                            if (msReportID == S_IT_RECONCILIATION_RPT_ID && oDropDownListSender.ReportFieldId == 203)
                            {
                                int iFinancialYrId = ReportsBL.SetDefaultFinancialYear(DateTime.Now.ToString());
                                string sFinancialYrId = iFinancialYrId.ToString();
                                ListItem oListItem = oDropDownList.Items.FindByValue(sFinancialYrId);
                                oDropDownList.SelectedValue = oListItem != null ? iFinancialYrId.ToString() : Constants.I_ZERO.ToString();
                                var cmbAcademicYear = grdDisplayParameter.Rows[Constants.I_FOUR].FindControl("DDLRptParameter") as ComboRpt;
                                cmbAcademicYear.SelectedValue = Constants.S_ZERO;
                            }
                            else if (mlstUserAccessPayrollReports.Contains(msReportID) && (hidHasFullAccess.Value == Constants.S_ZERO && moUserRole != Constants.UserRoles.Admin))
                            {
                                int I_USER_ID = 3;

                                if (msReportID == S_FORM_NO_16)
                                    I_USER_ID = 2;

                                if (iGridRowCount == I_USER_ID)
                                {
                                    ListItem oListItem = oDropDownList.Items.FindByValue(miUserId.ToString());
                                    if (oListItem != null)
                                        oListItem.Selected = true;
                                    else
                                    {
                                        oDropDownList.Items[0].Text = Constants.S_SELECT;
                                        oDropDownList.Items[0].Value = "-1";
                                    }
                                    oDropDownList.Enabled = false;
                                }
                            }
                        }
                    }

                    if (msReportID == S_ADDITIONAL_FEETYPE_PAYMENT_DETAILS)
                    {
                        var cmbStandard = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                        var cmbDivision = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
                        var cmbFeeType = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
                        DataTable oDataTable = SchoolwiseStudentFeeMasterBL.GetFeeTypes(miSchoolId, miAcademicYearId, cmbStandard.SelectedValue.ToInt(), cmbDivision.SelectedValue.ToInt());
                        if (cmbFeeType != null)
                        {
                            cmbFeeType.Items.Clear();
                            DataRow oDataRow = oDataTable.NewRow();
                            oDataRow["Value_Member"] = 0;
                            oDataRow["Display_Member"] = Constants.S_SELECT;
                            oDataTable.Rows.InsertAt(oDataRow, 0);
                            cmbFeeType.DataTextField = "Display_Member";
                            cmbFeeType.DataValueField = "Value_Member";
                            cmbFeeType.DataSource = oDataTable;
                            cmbFeeType.DataBind();
                        }
                    }

                    if (msReportID == S_SUBJECT_TOPPERS || msReportID==S_TESTWISE_SUBJECT_TOPPERS)
                    {
                        // fill division combobox on change of standard.
                        var oDropDownList = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                        if (iParentRptFldId != I_DIVISION_REPORT_FIELD_ID)
                            bIsSubjectReport = true;
                        if (hidStandardwise.Value == "Y" || hidStandardId.Value == "0")
                        {
                            oDropDownList.Enabled = false;
                            oDropDownList.SelectedItem.Selected = false;
                            oDropDownList.Items[0].Selected = true;
                            bIsDivisionRequired = false;
                            iDivisionId = 0;
                        }
                        else
                        {
                            oDropDownList.Enabled = true;
                            iDivisionId = oDropDownList.SelectedValue.ToInt();
                        }
                    }

                    if (msReportID == S_LEAVE_BALANCE && moUserRole != Constants.UserRoles.Admin && hidHasFullAccess.Value != "1")
                    {
                        if (oDropDownListSender.ReportFieldId == 376)
                        {

                            int iYear = oHashFilterParameters["Year"].ToInt();

                            string sStartDate = Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE].ToString();
                            string sEndDate = Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE].ToString();


                            DataSet oDataSet = SalaryDetailsBL.GetUsersStaffGroupDetais(miSchoolId, miAcademicYearId, miUserId, sStartDate, sEndDate, 0);
                            if (oDataSet != null && oDataSet.Tables.Count > 0)
                          {
                                DataTable oDataTable = oDataSet.Tables[0];
                                if (oDataTable.IsNonEmpty())
                                {
                                    ComboRpt oDropDownList = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
                                    oDropDownList.DataSource = oDataTable;
                                    oDropDownList.DataTextField = "Display_Member";
                                    oDropDownList.DataValueField = "Value_Member";
                                    oDropDownList.DataBind();

                                    oDropDownList.SelectedValue = oDataTable.Rows[0]["Value_Member"].ToString();
                                    oDropDownList.Enabled = false;
                                }

                                DataTable oDTUser = oDataSet.Tables[1];
                                if (oDTUser.IsNonEmpty())
                                {
                                    int iUsersId = oDTUser.Rows[0]["Value_Member"].ToInt();
                                    ComboRpt oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;

                                    oDropDownList.DataSource = oDTUser;
                                    oDropDownList.DataTextField = "Display_Member";
                                    oDropDownList.DataValueField = "Value_Member";
                                    oDropDownList.DataBind();

                                    oDropDownList.SelectedValue = iUsersId.ToString();
                                    oDropDownList.Enabled = false;
                                }
                            }
                        }
                    }




                }
                else
                {
                    // For subject toppers report only.
                    switch (msReportID)
                    {
                        case S_SUBJECT_TOPPERS:
                            {
                                string sParameterName = grdDisplayParameter.DataKeys[iGridRowCount][I_DISPLAY_NAME_INDEX].ToString();
                                var oDropDownList = sender as ComboRpt;
                                switch (sParameterName)
                                {
                                    case S_TYPE:
                                        if (oDropDownList.SelectedItem.Text == S_STANDARDWISE)
                                            hidStandardwise.Value = "Y";
                                        if (oDropDownList.SelectedItem.Text == S_CLASSWISE)
                                            hidStandardwise.Value = "N";
                                        break;
                                    case S_STANDARD:
                                        var oDDLStandard = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                                        hidStandardId.Value = oDDLStandard.SelectedValue;
                                        break;
                                    case S_DIVISION:
                                        var oDDLDivision = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                                        iDivisionId = oDDLDivision.SelectedValue.ToInt();
                                        break;
                                    case S_EXAM:
                                        if (hidStandardId.Value != "0")
                                        {
                                            int iStandardId = hidStandardId.Value.ToInt();
                                            // get published exams of selected class.
                                            DataTable oDtAllTests = GetExams(iStandardId, iDivisionId);
                                            FillExamCombo(iGridRowCount, oDtAllTests, iStandardId, iDivisionId);
                                        }
                                        else
                                        {
                                            var oDDLExam = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                                            oDDLExam.Enabled = false;
                                        }
                                        break;
                                }
                            }
                            break;
                        case S_TESTWISE_SUBJECT_TOPPERS:
                            {
                                string sParameterName = grdDisplayParameter.DataKeys[iGridRowCount][I_DISPLAY_NAME_INDEX].ToString();
                                var oDropDownList = sender as ComboRpt;
                                switch (sParameterName)
                                {
                                    case S_TYPE:
                                        if (oDropDownList.SelectedItem.Text == S_STANDARDWISE)
                                            hidStandardwise.Value = "Y";
                                        if (oDropDownList.SelectedItem.Text == S_CLASSWISE)
                                            hidStandardwise.Value = "N";
                                        break;
                                    case S_STANDARD:
                                        var oDDLStandard = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                                        hidStandardId.Value = oDDLStandard.SelectedValue;
                                        break;
                                    case S_DIVISION:
                                        var oDDLDivision = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                                        iDivisionId = oDDLDivision.SelectedValue.ToInt();
                                        break;
                                    case S_EXAM:
                                        if (iDivisionId == 0)
                                        {
                                            var oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
                                            DataTable oDtAllTests = oTestCollectionBL.GetAllExamsForTestwiseTopperReport(hidStandardId.Value.ToInt());
                                            var oDDLExam = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;

                                            oDDLExam.DataSource = oDtAllTests;
                                            oDDLExam.DataTextField = "Display_Member";
                                            oDDLExam.DataValueField = "Value_Member";
                                            oDDLExam.DataBind();

                                            oDDLExam.Items.Insert(0, new ListItem { Text = "-- All --", Value = Constants.S_ZERO });
                                        }
                                        break;
                                }
                            }
                            break;
                        case S_STAFF_LEAVES:

                            const int I_MONTH_INDEX = 0;
                            const int I_YEAR_INDEX = 1;
                            int I_STAFF_GROUP_INDEX = 2;
                            const int I_USER_INDEX = 3;
                            const int I_STAFF_GROUP_REPORT_FIELD = 254;
                            const int I_STAFF_LEAVE_REPORT_FIELD = 276;

                            if (sIsDependentFlag == Constants.S_NO)
                            {
                                ComboRpt oDropDownList;

                                switch (iGridRowCount)
                                {
                                    case I_MONTH_INDEX:
                                        oDropDownList = grdDisplayParameter.Rows[I_MONTH_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                                        oHashFilterParameters["MonthId"] = oDropDownList.SelectedValue;
                                        break;
                                    case I_YEAR_INDEX:
                                        oDropDownList = grdDisplayParameter.Rows[I_YEAR_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                                        oHashFilterParameters["Year"] = oDropDownList.SelectedValue;
                                        break;
                                }

                                if (oDropDownListSender.ReportFieldId != I_STAFF_GROUP_REPORT_FIELD && oDropDownListSender.ReportFieldId != I_STAFF_LEAVE_REPORT_FIELD)
                                {
                                    if (moUserRole == Constants.UserRoles.Admin || hidHasFullAccess.Value != "0")
                                    {
                                        if (miIsSearchGridConsidered != 2 || e != null)
                                        {
                                            oDropDownList = grdDisplayParameter.Rows[I_STAFF_GROUP_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                                            oDropDownList.SelectedValue = "0";

                                            oDropDownList = grdDisplayParameter.Rows[I_USER_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                                            oDropDownList.SelectedValue = "0";
                                            oDropDownList.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        if (iGridRowCount == I_STAFF_GROUP_INDEX)
                                        {
                                            int iMonthId = oHashFilterParameters["MonthId"].ToInt();
                                            int iYear = oHashFilterParameters["Year"].ToInt();

                                            string sStartDate;
                                            string sEndDate;
                                            if (iMonthId == 0)
                                            {
                                                sStartDate = new DateTime(iYear, 1, 1).ToShortDateString();
                                                sEndDate = new DateTime(iYear, 12, 1).ToShortDateString();
                                            }
                                            else
                                            {
                                                sStartDate = new DateTime(iYear, iMonthId, 1).ToShortDateString();
                                                sEndDate = sStartDate;
                                            }

                                            DataSet oDataSet = SalaryDetailsBL.GetUsersStaffGroupDetais(miSchoolId, miAcademicYearId, miUserId, sStartDate, sEndDate, 0);
                                            if (oDataSet != null && oDataSet.Tables.Count > 0)
                                            {
                                                DataTable oDataTable = oDataSet.Tables[0];
                                                if (oDataTable.IsNonEmpty())
                                                {
                                                    oDropDownList = grdDisplayParameter.Rows[I_STAFF_GROUP_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                                                    oDropDownList.DataSource = oDataTable;
                                                    oDropDownList.DataBind();
                                                }

                                                DataTable oDTUser = oDataSet.Tables[1];
                                                if (oDTUser.IsNonEmpty())
                                                {
                                                    int iUsersId = oDTUser.Rows[0]["Value_Member"].ToInt();
                                                    oDropDownList = grdDisplayParameter.Rows[I_USER_INDEX].FindControl("DDLRptParameter") as ComboRpt;

                                                    oDropDownList.DataSource = oDTUser;
                                                    oDropDownList.DataTextField = "Display_Member";
                                                    oDropDownList.DataValueField = "Value_Member";
                                                    oDropDownList.DataBind();

                                                    oDropDownList.SelectedValue = iUsersId.ToString();
                                                    oDropDownList.Enabled = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// This event is used to search student according to criteria.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            string sSearchCriteria;
            if (miIsSearchGridConsidered == 1)
            {
                int iReportId = 0;
                DivStaffDetailsContainer.Visible = false;
                grdStudentDetails.SelectedRowStyle.Font.Bold = false;
                sSearchCriteria = txtName.Text.Trim();
                bool bIsOnlyPrimary = false;
                if (msReportID == S_EXAM_RESULT_SS || msReportID == S_EXAM_RESULT_STSS_9STD || msReportID == S_EXAM_RESULT_STSS_10STD || msReportID == S_STUD_FINAL_RESULT_SS || msReportID == S_EXAM_RESULT || msReportID == S_STUD_FINAL_RESULT || msReportID == S_STUD_FINAL_RESULT_PPSH || msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std || msReportID == S_STUD_FINAL_RESULT_PPSH_Old || msReportID == S_STUD_FINAL_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_MCPS || msReportID == S_STUD_TERM2_RESULT || msReportID == S_STUD_TERM1_RESULT || msReportID == S_STUD_TERMWISE_RESULT || msReportID == S_EXAM_RESULT_FBS || msReportID == S_EXAM_RESULT_PPSN || msReportID == S_STUD_PRELIMINARY_RESULT || msReportID == S_STUD_EXAM_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_9 || msReportID == S_STUD_FINAL_RESULT_FOR_11 || msReportID == S_STUD_EXAM_RESULT_MVPS_9 || msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_PEMS || msReportID == S_STUDENT_PROGRESS_REPORT_CBSE || msReportID == S_COSCHOLASTIC_SUBJECT_MARK_DETAILS || msReportID == S_PERIODIC_TEST_MARK_DETAILS || msReportID == S_STUDENTS_FEE_DETAILS_REPORT || msReportID == S_EXAMWISE_REPORT_CARD || msReportID == S_STUDENT_SA_ONE_REPORT_1stTO4th || msReportID == S_STUDENT_SA_ONE_REPORT_5thTO8th || msReportID == S_STUDENT_OBSERVATION_REPORT)
                {
                    bIsOnlyPrimary = true;
                    iReportId = (msReportID != S_STUD_FINAL_RESULT && msReportID != S_STUD_FINAL_RESULT_PPSH && msReportID != S_STUD_FINAL_RESULT_SNS_6TO8_Std && msReportID != S_STUD_FINAL_RESULT_PPSH_Old && msReportID != S_STUD_FINAL_RESULT_FOR_PPSN && msReportID != S_COSCHOLASTIC_SUBJECT_MARK_DETAILS && msReportID != S_PERIODIC_TEST_MARK_DETAILS && msReportID != S_STUD_FINAL_RESULT_FOR_9 && msReportID != S_STUD_FINAL_RESULT_FOR_11 && msReportID != S_STUD_EXAM_RESULT_PPSN && msReportID != S_STUD_FINAL_RESULT_PPSN && msReportID == S_STUD_FINAL_RESULT_MCPS && msReportID == S_STUD_EXAM_RESULT_MVPS_9 && msReportID == S_STUDENT_PROGRESS_REPORT_CBSE && msReportID == S_STUDENTS_FEE_DETAILS_REPORT && msReportID == S_STUDENT_SA_ONE_REPORT_1stTO4th && msReportID == S_STUDENT_SA_ONE_REPORT_5thTO8th && msReportID == S_STUDENT_OBSERVATION_REPORT) ? msReportID.ToInt() : 0;
                }

                if (sSearchCriteria != string.Empty)
                {
                    int iOptChkVal = optSearchByBook.Checked ? Constants.I_ZERO : Constants.I_ONE;
                    if (msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS)
                        oDTUderDetails = IssueReturnBookBL.GetAllIssueBookDetails(GenerateDictionary(miSchoolId, miAcademicYearId, sSearchCriteria), iOptChkVal);
                    else
                        oDTUderDetails = StudentBL.GetAllStudentsByName(miSchoolId, miAcademicYearId, sSearchCriteria, bIsOnlyPrimary, iReportId);
                    grdStudentDetails.DataSource = oDTUderDetails;
                    grdStudentDetails.DataBind();
                    DivStudentDetailsContainer.Visible = true;

                    DivStudentDetailsContainer.Style.Add("height", grdStudentDetails.Rows.Count == 0 ? "25" : "150");
                }
                else
                    DivStudentDetailsContainer.Visible = false;
            }
            if (miIsSearchGridConsidered == 2)
            {
                DivStudentDetailsContainer.Visible = false;
                grdStaff.SelectedRowStyle.Font.Bold = false;
                sSearchCriteria = txtName.Text.Trim();

                if (sSearchCriteria != string.Empty)
                {
                    int iMonthId = 0;
                    int iYear = 0;
                    DateTime dtStartDate = DateTime.MinValue;
                    DateTime dtEndDate = DateTime.MinValue;

                    DataTable oDataTable = null;
                    if (msReportID == S_STAFF_LEAVES || msReportID == S_SALARY_SHEET)
                    {
                        const int I_MONTH_INDEX = 0;
                        const int I_YEAR_INDEX = 1;

                        var oMonthDropDownList = grdDisplayParameter.Rows[I_MONTH_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                        iMonthId = oMonthDropDownList.SelectedValue.ToInt();

                        var oYearDropDownList = grdDisplayParameter.Rows[I_YEAR_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                        iYear = oYearDropDownList.SelectedValue.ToInt();
                        if (iYear == 0)
                            iYear = DateTime.Now.Year;
                    }
                    else if (msReportID == S_LEAVE_BALANCE)
                    {
                        const int I_YEAR_INDEX = 0;

                        var oYearDropDownList = grdDisplayParameter.Rows[I_YEAR_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                        iYear = oYearDropDownList.SelectedValue.ToInt();
                        if (iYear == 0)
                            iYear = DateTime.Now.Year;
                    }
                    else if (molstPayrollDateReports.Contains(msReportID))
                    {
                        var oDateControl = grdDisplayParameter.Rows[0].FindControl("CalenderRptParameter") as PopCalendar;
                        dtStartDate = oDateControl.DateValue;

                        oDateControl = grdDisplayParameter.Rows[1].FindControl("CalenderRptParameter") as PopCalendar;
                        dtEndDate = oDateControl.DateValue;
                    }

                    if (msReportID == S_FORM_NO_16 || msReportID == S_INVESTMENT_DECLARATIONS || msReportID == S_NET_SALARY)
                    {
                        var oDownList = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                        int iFinancialYearId = miFinancialYearId;

                        if (oDownList.SelectedValue != Constants.S_ZERO)
                            iFinancialYearId = Convert.ToInt32(oDownList.SelectedValue);

                        oDataTable = SchoolUserCollectionBL.GetAllStaffByName(miSchoolId, miAcademicYearId, sSearchCriteria, iMonthId, iYear, dtStartDate, dtEndDate, iFinancialYearId);
                    }
                    else if (msReportID != S_STOPWISE_TRANSPORT_DETAILS && msReportID != S_USERROLEWISE_TRAVELLER_DETAILS)
                        oDataTable = SchoolUserCollectionBL.GetAllStaffByName(miSchoolId, miAcademicYearId, sSearchCriteria, iMonthId, iYear, dtStartDate, dtEndDate, -1);
                    else if ((msReportID == S_STOPWISE_TRANSPORT_DETAILS))
                    {
                        string sFilter = txtName.Text.Trim();
                        var cmbVehicalId = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                        var cmbShiftId = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
                        var cmbRouteId = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
                        var cmbStopId = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;
                        oDataTable = SchoolUserCollectionBL.GetTransportTravellerDetails(miSchoolId, miAcademicYearId, sFilter, cmbVehicalId.SelectedValue.ToInt(), cmbShiftId.SelectedValue.ToInt(), cmbRouteId.SelectedValue.ToInt(), cmbStopId.SelectedValue.ToInt());
                    }
                    else if ((msReportID == S_USERROLEWISE_TRAVELLER_DETAILS))
                    {
                        string sFilter = txtName.Text.Trim();
                        var cmbUserRoleId = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                        var cmbStandardId = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
                        var cmbDivisionId = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
                        var cmbUserId = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;
                        oDataTable = SchoolUserCollectionBL.GetAllTravellerDetails(miSchoolId, miAcademicYearId, sFilter, cmbUserRoleId.SelectedValue.ToInt(), cmbStandardId.SelectedValue.ToInt(), cmbDivisionId.SelectedValue.ToInt(), cmbUserId.SelectedValue.ToInt());
                    }
                    oDTUderDetails = oDataTable;
                    grdStaff.DataSource = oDataTable;
                    grdStaff.DataBind();
                    DivStaffDetailsContainer.Visible = true;
                    DivStaffDetailsContainer.Style.Add("height", grdStaff.Rows.Count == 0 ? "25" : "150");
                }
            }
            if (miIsSearchGridConsidered == 3)
            {
                DivStudentDetailsContainer.Visible = false;
                grdStaff.SelectedRowStyle.Font.Bold = false;
                sSearchCriteria = txtName.Text.Trim();
                if (sSearchCriteria != string.Empty)
                {
                    DataTable oDataTable = SchoolWiseTeacherMasterCollectionBL.GetAllTeachersByName(miSchoolId, miAcademicYearId, sSearchCriteria);
                    grdStaff.DataSource = oDataTable;
                    grdStaff.DataBind();
                    DivStaffDetailsContainer.Visible = true;

                    DivStaffDetailsContainer.Style.Add("height", grdStaff.Rows.Count == 0 ? "25" : "150");
                }
            }
            if (miIsSearchGridConsidered == 4)
            {
                var oDropDownList = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                hidUserRolId.Value = oDropDownList.SelectedValue;
                sSearchCriteria = txtName.Text.Trim();
                if (sSearchCriteria != string.Empty)
                {
                    string sName = txtName.Text.Trim();
                    DataTable oDTUserDetail = SchoolUserCollectionBL.GetUserDetails(miSchoolId, sName, hidUserRolId.Value.ToInt(), miAcademicYearId);
                    oDTUderDetails = oDTUserDetail;
                    grdStaff.DataSource = oDTUserDetail;
                    grdStaff.DataBind();
                    DivStaffDetailsContainer.Visible = true;
                    grdStaff.SelectedRowStyle.Font.Bold = false;
                    DivStaffDetailsContainer.Style.Add("height", grdStaff.Rows.Count == 0 ? "25" : "150");
                }
            }
            if (miIsSearchGridConsidered == 5)
            {
                sSearchCriteria = txtName.Text.Trim();
                if (sSearchCriteria != string.Empty)
                {
                    string sName = txtName.Text.Trim();
                    int iFlg;
                    var cmbAssigned = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                    if (cmbAssigned.SelectedValue != "0")
                        iFlg = cmbAssigned.SelectedValue.ToInt();
                    else
                        iFlg = Constants.I_THREE;
                    var oTaskManagementBL = new TaskManagementBL(miSchoolId, miAcademicYearId);
                    DataTable oDTUserDetail = oTaskManagementBL.GetUserDetails(GenerateXML(), sName, iFlg);
                    oDTUderDetails = oDTUserDetail;
                    grdStaff.DataSource = oDTUserDetail;
                    grdStaff.DataBind();
                    DivStaffDetailsContainer.Visible = true;
                    grdStaff.SelectedRowStyle.Font.Bold = false;
                    DivStaffDetailsContainer.Style.Add("height", grdStaff.Rows.Count == 0 ? "25" : "150");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// 	This event is used to set values to controls accoeding to selection.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdStudentDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "SELECT")
            {
                int iRowIndex = e.CommandArgument.ToInt();

                if (msReportID == S_EXAM_PERFORMANCE_REPORT_ID)
                {
                    var oTextBox = grdDisplayParameter.Rows[0].FindControl("txtRptParameter") as TextBox;
                    oTextBox.Text = grdStudentDetails.DataKeys[iRowIndex]["Enrolment_Number"].ToString().Trim();
                }

                else if (msReportID != S_USERROLEWISE_BOOK_ISSUED_USERS)
                {
                    var oDropDownList = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["Standard_Id"].ToString();
                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;

                    if (msReportID == S_ADMISSION_CANCELLATION_FORM)
                        oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["Division_Id"].ToString();
                    else
                        oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["SchoolWise_Standard_Division_Id"].ToString();
                    
                    oDropDownList_ComboChangeEvent(oDropDownList, null);
                    if (msReportID == S_CLASSWISE_STUDENT_BANK_STATEMENT_REPORT)
                    {
                        oDropDownList = grdDisplayParameter.Rows[4].FindControl("DDLRptParameter") as ComboRpt;
                        oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["Student_Id"].ToString();
                        oDropDownList_ComboChangeEvent(oDropDownList, null);
                    }
                    else
                    {
                        oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
                        oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["Student_Id"].ToString();
                        oDropDownList_ComboChangeEvent(oDropDownList, null);
                    }
                }
                else
                {
                    if (optSearchByBook.Checked)
                    {
                        ((TextBox)grdDisplayParameter.Rows[4].FindControl("txtRptParameter")).Text = grdStudentDetails.DataKeys[iRowIndex]["Enrolment_Number"].ToString();
                    }
                    else
                    {
                        var ohidUserRoleId = grdStudentDetails.Rows[iRowIndex].Cells[2].FindControl("hidUserRoleId") as HiddenField;
                        int iUserId = grdStudentDetails.DataKeys[iRowIndex]["Student_Id"].ToString().ToInt();

                        var cmbUserRoleId = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                        cmbUserRoleId.SelectedValue = ohidUserRoleId.Value;
                        oDropDownList_ComboChangeEvent(cmbUserRoleId, null);


                        if (ohidUserRoleId.Value == "3")
                        {
                            int iStandard_Id = grdStudentDetails.DataKeys[iRowIndex]["Standard_Id"].ToString().ToInt();
                            int iDivisionId = grdStudentDetails.DataKeys[iRowIndex]["SchoolWise_Standard_Division_Id"].ToString().ToInt();

                            var cmbStandard = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
                            cmbStandard.SelectedValue = iStandard_Id.ToString();
                            oDropDownList_ComboChangeEvent(cmbStandard, null);

                            var cmbDivision = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
                            cmbDivision.SelectedValue = iDivisionId.ToString();
                            oDropDownList_ComboChangeEvent(cmbDivision, null);
                        }
                        var cmbUser = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;
                        cmbUser.SelectedValue = iUserId.ToString();
                    }
                }

                if (msReportID == S_DYNAMIC_PENDING_FEE_REPORT || msReportID == S_OLD_YEAR_PENDING_FEES_STUDENTS_LIST || msReportID == S_STUDENT_ALL_ACADEMICS_PENDING_FEE || msReportID == S_COSCHOLASTIC_SUBJECT_MARK_DETAILS || msReportID == S_TRACKED_UPDATED_STUDENT_DETAILS || msReportID == S_STUDENT_TRANSFER_DETAILS || msReportID == S_STUDENT_PENDING_FEE_REMINDER || msReportID == S_STUDENT_HALF_YEARLY_3TO9)
                {
                    var oDropDownList = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["Standard_Id"].ToString();
                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;

                    oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["SchoolWise_Standard_Division_Id"].ToString();

                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["Student_Id"].ToString();
                }

                else if (msReportID == S_BONAFIDE_ISSUE_REGISTER)
                {
                    var oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["Standard_Id"].ToString();
                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;

                    oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["SchoolWise_Standard_Division_Id"].ToString();

                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[4].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = grdStudentDetails.DataKeys[iRowIndex]["Student_Id"].ToString();
                }

                grdStudentDetails.SelectedRowStyle.Font.Bold = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// This grid row databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStaff_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (miIsSearchGridConsidered == 4)
            {
                int iRowindex = e.Row.RowIndex.ToInt();
                if (iRowindex >= 0)
                {
                    var ohidUserRoleId = e.Row.Cells[2].FindControl("hidUser_RoleId") as HiddenField;
                    ohidUserRoleId.Value = oDTUderDetails.Rows[iRowindex][7].ToString();
                }
            }
            if (miIsSearchGridConsidered == 5)
            {
                int iRowindex = e.Row.RowIndex.ToInt();
                if (iRowindex >= 0)
                {
                    var hidDesignationId = e.Row.Cells[2].FindControl("hidDesignationId") as HiddenField;
                    hidDesignationId.Value = oDTUderDetails.Rows[iRowindex][2].ToString();
                    var hidUserId = e.Row.Cells[2].FindControl("hidUserId") as HiddenField;
                    hidUserId.Value = oDTUderDetails.Rows[iRowindex][0].ToString();
                }
            }
            if (miIsSearchGridConsidered == 2)
            {
                if ((msReportID == S_STOPWISE_TRANSPORT_DETAILS) || (msReportID == S_USERROLEWISE_TRAVELLER_DETAILS))
                {
                    int iRowindex = e.Row.RowIndex.ToInt();
                    if (iRowindex >= 0)
                    {
                        grdStaff.HeaderRow.Cells[1].Text = "Designaton/Class";
                        var hidVehicleId = e.Row.Cells[2].FindControl("hidDDL1") as HiddenField;
                        var hidShiftId = e.Row.Cells[2].FindControl("hidDDL2") as HiddenField;
                        var hidRouteId = e.Row.Cells[2].FindControl("hidDDL3") as HiddenField;
                        var hidStopId = e.Row.Cells[2].FindControl("hidDDL4") as HiddenField;
                        hidVehicleId.Value = oDTUderDetails.Rows[iRowindex][8].ToString();
                        hidShiftId.Value = oDTUderDetails.Rows[iRowindex][9].ToString();
                        hidRouteId.Value = oDTUderDetails.Rows[iRowindex][10].ToString();
                        hidStopId.Value = oDTUderDetails.Rows[iRowindex][11].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// 	This event is used to set values to controls accoeding to selection.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void grdStaff_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int I_MONTH_INDEX = 0;
        int I_YEAR_INDEX = 1;
        int I_STAFF_GROUP_INDEX = 2;
        int I_USER_INDEX = 3;
        int I_FINANCIAL_YEAR_ID = 0;

        try
        {
            if (e.CommandName == "SELECT" && miIsSearchGridConsidered == 2)
            {
                if ((msReportID != S_STOPWISE_TRANSPORT_DETAILS) && (msReportID != S_USERROLEWISE_TRAVELLER_DETAILS))
                {
                    int iRowIndex = e.CommandArgument.ToInt();
                    ComboRpt oDropDownList;

                    if (msReportID == S_LEAVE_BALANCE || msReportID == S_FORM_NO_16 || msReportID == S_NET_SALARY)
                    {
                        I_STAFF_GROUP_INDEX = 1;
                        I_USER_INDEX = 2;
                    }

                    if (msReportID == S_FORM_NO_16 || msReportID == S_INVESTMENT_DECLARATIONS || msReportID == S_NET_SALARY)
                    {
                        oDropDownList = grdDisplayParameter.Rows[I_FINANCIAL_YEAR_ID].FindControl("DDLRptParameter") as ComboRpt;
                        if (oDropDownList.SelectedValue == Constants.S_ZERO)
                        {
                            oDropDownList.SelectedValue = miFinancialYearId.ToString();
                            oDropDownList_ComboChangeEvent(oDropDownList, null);
                        }
                    }

                    if (msReportID == S_STAFF_LEAVES || msReportID == S_LEAVE_BALANCE)
                    {
                        int I_YEAR_ID = 1;

                        if (msReportID == S_LEAVE_BALANCE)
                            I_YEAR_ID = 0;

                        oDropDownList = grdDisplayParameter.Rows[I_YEAR_ID].FindControl("DDLRptParameter") as ComboRpt;
                        if (oDropDownList.SelectedValue == Constants.S_ZERO)
                        {
                            oDropDownList.SelectedValue = DateTime.Now.Year.ToString();
                            oDropDownList_ComboChangeEvent(oDropDownList, null);
                        }
                    }

                    if (msReportID == S_SALARY_SHEET)
                    {
                        oDropDownList = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                        if (oDropDownList.SelectedValue == Constants.S_ZERO)
                            oDropDownList.SelectedValue = DateTime.Now.Month.ToString();

                        oDropDownList = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
                        if (oDropDownList.SelectedValue == Constants.S_ZERO)
                        {
                            oDropDownList.SelectedValue = DateTime.Now.Year.ToString();
                            oDropDownList_ComboChangeEvent(oDropDownList, null);
                        }
                    }

                    oDropDownList = grdDisplayParameter.Rows[I_STAFF_GROUP_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = grdStaff.DataKeys[iRowIndex]["StaffGroupId"].ToString();
                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[I_USER_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                    string sUserId = grdStaff.DataKeys[iRowIndex]["UserId"].ToString();
                    ListItem oListItem = oDropDownList.Items.FindByValue(sUserId);
                    if (oListItem != null)
                        oListItem.Selected = true;
                }
                else
                {
                    int iRowIndex = e.CommandArgument.ToInt();
                    var hidDDL1 = grdStaff.Rows[iRowIndex].FindControl("hidDDL1") as HiddenField;
                    var hidDDL2 = grdStaff.Rows[iRowIndex].FindControl("hidDDL2") as HiddenField;
                    var hidDDL3 = grdStaff.Rows[iRowIndex].FindControl("hidDDL3") as HiddenField;
                    var hidDDL4 = grdStaff.Rows[iRowIndex].FindControl("hidDDL4") as HiddenField;

                    var oDropDownList = grdDisplayParameter.Rows[I_MONTH_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = hidDDL1.Value;
                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[I_YEAR_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = hidDDL2.Value;
                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[I_STAFF_GROUP_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = hidDDL3.Value;
                    oDropDownList_ComboChangeEvent(oDropDownList, null);

                    oDropDownList = grdDisplayParameter.Rows[I_USER_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                    oDropDownList.SelectedValue = hidDDL4.Value;
                }
            }

            if (e.CommandName == "SELECT" && miIsSearchGridConsidered == 3)
            {
                int iRowIndex = e.CommandArgument.ToInt();
                var oDropDownList = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                oDropDownList.SelectedValue = grdStaff.DataKeys[iRowIndex]["Teacher_Id"].ToString();
                oDropDownList_ComboChangeEvent(oDropDownList, null);
            }
            if (e.CommandName == "SELECT" && miIsSearchGridConsidered == 4)
            {
                int iRowIndex = e.CommandArgument.ToInt();
                var ohidUserRoleId = grdStaff.Rows[iRowIndex].FindControl("hidUser_RoleId") as HiddenField;
                var oddlDropDownList = grdDisplayParameter.Rows[I_MONTH_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                oddlDropDownList.SelectedValue = ohidUserRoleId.Value;
                oDropDownList_ComboChangeEvent(oddlDropDownList, null);

                var oDropDownList = grdDisplayParameter.Rows[I_YEAR_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                oDropDownList = grdDisplayParameter.Rows[I_YEAR_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                oDropDownList.Enabled = true;
                oDropDownList_ComboChangeEvent(oDropDownList, null);
                oDropDownList.SelectedValue = grdStaff.DataKeys[iRowIndex]["UserId"].ToString();
            }
            if (e.CommandName == "SELECT" && miIsSearchGridConsidered == 2)
            {
                if (msReportID != S_STOPWISE_TRANSPORT_DETAILS)
                {
                    int iRowIndex = e.CommandArgument.ToInt();

                    var oDropDownList = grdDisplayParameter.Rows[I_USER_INDEX].FindControl("DDLRptParameter") as ComboRpt;
                    string sUserId = grdStaff.DataKeys[iRowIndex]["UserId"].ToString();
                    ListItem oListItem = oDropDownList.Items.FindByValue(sUserId);
                    if (oListItem != null)
                        oListItem.Selected = true;
                }
            }
            if (e.CommandName == "SELECT" && miIsSearchGridConsidered == 5)
            {
                int iRowIndex = e.CommandArgument.ToInt();
                var hidUserId = grdStaff.Rows[iRowIndex].FindControl("hidUserId") as HiddenField;
                var hidDesignationId = grdStaff.Rows[iRowIndex].FindControl("hidDesignationId") as HiddenField;
                var cmbAssign = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                var cmbDesignation = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
                var cmbUser = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;

                if (cmbAssign.SelectedValue == "0")
                {
                    //Set Combo value 'ALL'
                    cmbAssign.SelectedValue = "3";
                    oDropDownList_ComboChangeEvent(cmbAssign, null);
                }
                cmbDesignation.SelectedValue = hidDesignationId.Value;
                oDropDownList_ComboChangeEvent(cmbDesignation, null);
                cmbUser.SelectedValue = hidUserId.Value;
            }
            grdStaff.SelectedRowStyle.Font.Bold = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// 	This event is used to fill search grid view.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void optSearchByUser_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetLabelText();
            btnShow_Click(sender, e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// 	This event is used to fill search grid view.
    /// </summary>
    /// <param name="sender"> </param>
    /// <param name="e"> </param>
    protected void optSearchByBook_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetLabelText();
            btnShow_Click(sender, e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    /// <summary>
    /// This for calender selected index change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void oPopCalendar_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            Dictionary<int, string> dictDataType = new Dictionary<int, string>();
            var oDropDownListSender = sender as PopCalendar;
            int iParentRptFldId = 0;
            DateTime dtParentDateValue = DateTime.Now.Date;
            int iParentValue = 0;

            lblNorecord.Visible = false;
            moDictFiledDatatype.Clear();

            if (!oDropDownListSender.DateValue.ToShortDateString().IsValidDate())
                oDropDownListSender.DateValue = DateTime.Now.Date;

            //This loop is to read each control one by one from grid.
            var oHashFilterParameters = new Hashtable();
            for (int iGridRowCount = 0; iGridRowCount < grdDisplayParameter.Rows.Count; iGridRowCount++)
            {
                //If current control is dependent control then do this.
                string sIsDependentFlag = "N";
                if (msReportID != S_DAYWISE_ABSTNT_STUDENT)
                    sIsDependentFlag = grdDisplayParameter.DataKeys[iGridRowCount]["Is_Dependent"].ToString();

                string sDataType = grdDisplayParameter.DataKeys[iGridRowCount][I_DATATYPE_INDEX].ToString().ToLower();

                var ddlControl = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                var dtControl = grdDisplayParameter.Rows[iGridRowCount].FindControl("CalenderRptParameter") as PopCalendar;
                if (oDropDownListSender.Equals(dtControl))
                    iParentRptFldId = grdDisplayParameter.DataKeys[iGridRowCount]["Report_Field_Id"].ToInt();

                if (grdDisplayParameter.DataKeys[iGridRowCount]["Is_Parent"].ToString() == Constants.S_YES)
                {
                    dictDataType.Add(grdDisplayParameter.DataKeys[iGridRowCount]["Report_Field_Id"].ToInt(), sDataType);

                    if (sIsDependentFlag == Constants.S_NO && grdDisplayParameter.DataKeys[iGridRowCount]["Report_Field_Id"].ToInt() != iParentRptFldId)
                        dtParentDateValue = dtControl.DateValue;
                    else if (sIsDependentFlag == Constants.S_YES && grdDisplayParameter.DataKeys[iGridRowCount]["Report_Field_Id"].ToInt() != iParentRptFldId)
                        iParentValue = ddlControl.SelectedValue.ToInt();
                }

                if (sIsDependentFlag == Constants.C_YES.ToString())
                {
                    int iParentFieldId = grdDisplayParameter.DataKeys[iGridRowCount]["Parent_Field_Id"].ToString().ToInt();
                    string sParentFieldIdFilterString = grdDisplayParameter.DataKeys[iGridRowCount]["Filter_Field_Name"].ToString();

                    string sAdditionalParentIds = string.Empty;

                    if (grdDisplayParameter.DataKeys[iGridRowCount]["Additional_Parent_Field_Id"] != DBNull.Value)
                        sAdditionalParentIds = Convert.ToString(grdDisplayParameter.DataKeys[iGridRowCount]["Additional_Parent_Field_Id"]);

                    string[] sArrayAdditionalParentIds = null;
                    if (!string.IsNullOrEmpty(sAdditionalParentIds))
                        sArrayAdditionalParentIds = sAdditionalParentIds.Split(',');

                    string sAdditionalParentFields = Convert.ToString(grdDisplayParameter.DataKeys[iGridRowCount]["Additional_Filter_Field_Name"]);
                    string[] sArrayAdditionalFilterFields = sAdditionalParentFields.Split(',');

                    if (iParentRptFldId == iParentFieldId || sArrayAdditionalParentIds.Contains(iParentRptFldId.ToString()))
                    {
                        var oDropDownList = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                        var dtControlParent = grdDisplayParameter.Rows[iGridRowCount].FindControl("CalenderRptParameter") as PopCalendar;

                        string sViewName = grdDisplayParameter.DataKeys[iGridRowCount][I_VIEWNAME_INDEX].ToString();
                        if (!string.IsNullOrEmpty(sViewName))
                        {
                            //Here filter field name string is formatted and we get parameter name.
                            string sFilterFieldName = grdDisplayParameter.DataKeys[iGridRowCount]["Filter_Field_Name"].ToString();

                            sFilterFieldName = sFilterFieldName.Replace("{", "[");
                            sFilterFieldName = sFilterFieldName.Replace("}", "]");
                            if (!string.IsNullOrEmpty(hidSchemaName.Value))
                                sFilterFieldName = sFilterFieldName.Replace(hidSchemaName.Value, string.Empty);
                            sFilterFieldName = sFilterFieldName.Replace(sFilterFieldName.Substring(1, sFilterFieldName.IndexOf(".")), string.Empty);

                            if (molstPayrollDateReports.Contains(msReportID))
                                sFilterFieldName = sFilterFieldName.Replace("[", string.Empty).Replace("]", string.Empty);

                            if (dictDataType[iParentFieldId] == S_DATETIME)
                            {
                                if (iParentRptFldId == iParentFieldId)
                                    oHashFilterParameters[sFilterFieldName] = oDropDownListSender.DateValue;
                                else
                                    oHashFilterParameters[sFilterFieldName] = dtParentDateValue;
                            }
                            else
                            {
                                if (iParentRptFldId == iParentFieldId)
                                    oHashFilterParameters[sFilterFieldName] = oDropDownList.SelectedValue;
                                else
                                    oHashFilterParameters[sFilterFieldName] = iParentValue;
                            }

                            moDictFiledDatatype[sFilterFieldName] = dictDataType[iParentFieldId];

                            // **************************** To support multiple parent fields *******************************************************************

                            if (!string.IsNullOrEmpty(sAdditionalParentFields))
                            {
                                for (int iFieldIndex = 0; iFieldIndex < grdDisplayParameter.Rows.Count; iFieldIndex++)
                                {
                                    string sControlDatatype = grdDisplayParameter.DataKeys[iFieldIndex][I_DATATYPE_INDEX].ToString().ToLower();

                                    var oDateControl = grdDisplayParameter.Rows[iFieldIndex].FindControl("CalenderRptParameter") as PopCalendar;
                                    string sReportFieldName = Convert.ToString(grdDisplayParameter.DataKeys[iFieldIndex]["Field_name"]);

                                    if (!string.IsNullOrEmpty(hidSchemaName.Value))
                                        sReportFieldName = sReportFieldName.Replace(hidSchemaName.Value, string.Empty);
                                    if (!sArrayAdditionalFilterFields.Contains(sReportFieldName))
                                        continue;
                                    string sAdditionalFilterFieldName = sReportFieldName;
                                    sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace("{", "[");
                                    sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace("}", "]");
                                    if (!string.IsNullOrEmpty(hidSchemaName.Value))
                                        sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace(hidSchemaName.Value, string.Empty);
                                    sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace(sAdditionalFilterFieldName.Substring(1, sAdditionalFilterFieldName.IndexOf(".")), string.Empty);

                                    if (molstPayrollDateReports.Contains(msReportID))
                                        sAdditionalFilterFieldName = sAdditionalFilterFieldName.Replace("[", string.Empty).Replace("]", string.Empty);
                                    if (sControlDatatype == S_DATETIME)
                                        oHashFilterParameters[sAdditionalFilterFieldName] = oDateControl.DateValue;
                                    else
                                    {
                                        var oDropDown = grdDisplayParameter.Rows[iFieldIndex].FindControl("DDLRptParameter") as ComboRpt;
                                        oHashFilterParameters[sAdditionalFilterFieldName] = oDropDown.SelectedValue;
                                    }
                                    moDictFiledDatatype[sAdditionalFilterFieldName] = sControlDatatype;
                                }
                            }

                            if ((msReportID == S_SALARY_SLIP || msReportID == S_TRANSFERED_STAFF_SALARY_SLIP || msReportID == S_BANK_LETTER) && (hidHasFullAccess.Value == Constants.S_ZERO && moUserRole != Constants.UserRoles.Admin))
                            {
                                int I_STAFF_GROUP_ID = 2;
                                int I_USER_ID = 3;

                                if (iGridRowCount == I_STAFF_GROUP_ID || iGridRowCount == I_USER_ID)
                                    oHashFilterParameters["UserId"] = miUserId;
                            }

                            if (oHashFilterParameters.ContainsKey("StaffGroupsId"))
                                oHashFilterParameters["StaffGroupsId"] = 0;

                            // ***********************************************************************************************************************************
                            FillFilterParametersCombo(oDropDownList, oHashFilterParameters, iGridRowCount);

                            if ((msReportID == S_SALARY_SLIP || msReportID == S_TRANSFERED_STAFF_SALARY_SLIP || msReportID == S_BANK_LETTER) && (hidHasFullAccess.Value == Constants.S_ZERO && moUserRole != Constants.UserRoles.Admin))
                            {
                                int I_USER_ID = 3;
                                if (iGridRowCount == I_USER_ID)
                                {
                                    ListItem oListItem = oDropDownList.Items.FindByValue(miUserId.ToString());
                                    if (oListItem != null)
                                        oListItem.Selected = true;
                                    else
                                    {
                                        oDropDownList.Items[0].Text = Constants.S_SELECT;
                                        oDropDownList.Items[0].Value = "-1";
                                    }
                                    oDropDownList.Enabled = false;
                                }
                            }

                            if (msReportID == S_SALARY_SLIP || msReportID == S_SALARY_LEDGER || msReportID == S_EARNINGS_DEDUCTIONS || msReportID == S_INSURANCE_DETAILS || msReportID == S_STAFF_ATTENDANCE)
                            {
                                if (txtName.Text.Trim() != string.Empty)
                                    btnShow_Click(btnSearch, null);
                            }
                        }
                    }
                }
            }

            SetSalarySlipState();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
        }
    }

    #endregion Events

    #region Methods

    #region General

    /// <summary>
    /// 	This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSearch, btnDisplayReport });
        btnDisplayReport.Attributes.Add("onclick", "VisibleOrHideControls();");
        txtName.Attributes.Add("onkeypress", "return clickButton(event,'" + btnSearch.ClientID + "')");
    }

    /// <summary>
    /// 	This method is used to do search after pressing of enter key.
    /// </summary>
    private void SetDefaultProperties()
    {
        var oform = this.Master.FindControl("form1") as HtmlForm;
        oform.DefaultButton = btnSearch.UniqueID;
    }

    /// <summary>
    /// 	This method is used to generate tree structure for reports.
    /// </summary>
    private void GenerateReportTree()
    {
        string sReportsRootDirectory = Server.MapPath("~") + "\\RITeSchool\\Report";
        var sbldTreeString = new StringBuilder();
        if (!Directory.Exists(sReportsRootDirectory))
            lblreportsNav.Text = "Directory does not exists. " + sReportsRootDirectory;
        else
        {
            sbldTreeString.Append("<div class=\"dtree\">\r\n");
            sbldTreeString.Append("<p><a href=\"javascript: d.openAll();\">Expand All</a> / <a href=\"javascript: d.closeAll();\">Collapse All</a></p>\r\n");
            sbldTreeString.Append("<script type=\"text/javascript\">\r\n");
            sbldTreeString.Append("<!--\r\n");
            sbldTreeString.Append("d = new dTree('d');\r\n");
            sbldTreeString.Append("d.add(0,-1,' School Reports');\r\n");
            int igroupIndex = 0;
            int ireportIndex = 1;
            // if given report directory is not exists then it gives proper error message.
            if (!Directory.Exists(sReportsRootDirectory))
            {
                lblreportsNav.Text = "Directory does not exists. " + sReportsRootDirectory;
                return;
            }
            if (moUserRole != Constants.UserRoles.Supervisor && moUserRole != Constants.UserRoles.Teacher && moUserRole != Constants.UserRoles.OtherStaff)
            {
                //This method is used to read report directory and to create tree structure for reports.
                ReadReportDir(sReportsRootDirectory, ref sbldTreeString, ref igroupIndex, ref ireportIndex);
            }
            else
                CheckReportDirAccess(sReportsRootDirectory, ref sbldTreeString, ref igroupIndex, ref ireportIndex);
        }
        sbldTreeString.Append("document.write(d);\r\n");
        sbldTreeString.Append("//-->>\r\n");
        sbldTreeString.Append("</script>\r\n");
        lblreportsNav.Text = sbldTreeString.ToString();
    }

    /// <summary>
    /// 	This method is used to read directories one by one.
    /// </summary>
    /// <param name="asDirName"> </param>
    /// <param name="asTreeString"> </param>
    /// <param name="aiParentIndex"> </param>
    /// <param name="aiIndex"> </param>
    private void CheckReportDirAccess(string asDirName, ref StringBuilder asTreeString, ref int aiParentIndex, ref int aiIndex)
    {
        asTreeString.AppendFormat("d.add({0},{1},'{2}');\r\n", aiIndex, aiParentIndex, asDirName.Substring(asDirName.LastIndexOf("\\") + 1));
        aiParentIndex += 1;
        DataSet oReportAccess = GetScreenAccessDetails();

        //This method gives all reports details.
        DataTable oDSReportDetails = oReportAccess.Tables[2];
        string sReportId = "0";
        if (QueryString["ReportID"] != null)
            sReportId = QueryString["ReportID"];

        var hasFullAccess = from report in oDSReportDetails.AsEnumerable()
                            where report.Field<int>("Report_id").ToString() == sReportId && report.Field<bool>("HasFullAccess")
                            select report;

        hidHasFullAccess.Value = hasFullAccess.Count().ToString();
        //This method gives all report folder names.
        DataRow[] oDRReportsFolder = oReportAccess.Tables[1].Select("HasAccess='Y'");
        foreach (DataRow oDataRow in oDRReportsFolder)
        {
            string sDirName = oDataRow["Report_Folder_Name"].ToString();
            aiIndex += 1;
            asTreeString.AppendFormat("d.add({0},{1},'{2}');\r\n", aiIndex, aiParentIndex, sDirName.Substring(0));

            //This method is used to read all files under given subdirectory and returns index which 
            //will increased by one and treat as a parent index for next subdirectory..
            DataRow[] oDTReportFiles = oDSReportDetails.Select(string.Format("Report_Folder_Name='{0}' AND HasAccess='Y'", sDirName));

            //This method is used to read all files under given subdirectory and returns index which 
            //will increased by one and treat as a parent index for next subdirectory..
            aiIndex = ReadDirForReportFiles(asDirName, asTreeString, aiIndex, sDirName, oDTReportFiles);
        }
    }

    /// <summary>
    /// 	This method is used to check whether report's data is available or not.
    /// </summary>
    /// <param name="asFilterString"> </param>
    private void IsReportEmpty(string asFilterString)
    {
        int iReportDataCount;
        //This method is used to decide given viewname is stored procedure or view.        
        //This method splits string into parameter list and it's respective values.
        asFilterString = FormatFilterString(asFilterString);
        String[] sFilterParameter = asFilterString.Split('@');
        for (int iRowCnt = 0; iRowCnt < sFilterParameter.Length; iRowCnt++)
            sFilterParameter[iRowCnt] = sFilterParameter[iRowCnt].Replace("#", "'");
        if (msType == "View")
        {
            iReportDataCount = ReadFilterParametersForView(sFilterParameter, msView);
            SetNoRecordsErrMsg(iReportDataCount);
        }
        else
        {
            if (msView != null)
            {
                iReportDataCount = ReadFilterParametersForUSP(sFilterParameter, msView);
                SetNoRecordsErrMsg(iReportDataCount);
            }
        }
    }

    /// <summary>
    /// 	This method is used to check report is empty.
    /// </summary>
    /// <param name="aiReportDataCount"> </param>
    private void SetNoRecordsErrMsg(int aiReportDataCount)
    {
        if ((aiReportDataCount == Constants.I_ZERO))
            throw new NoRecordFoundException("No records found.");
    }

    /// <summary>
    /// 	This method is used to set view/USP name/type to msView/msType respectivelu.
    /// </summary>
    private void ReadViewOrUSPName()
    {
        if (msReportViewName.StartsWith("{vw"))
        {
            msView = msReportViewName.Substring(1);
            msType = "View";
        }
        else
        {
            msView = msReportViewName.Substring(1, (msReportViewName.Length) - 3);
            msType = "SP";
        }
    }

    /// <summary>
    /// 	This method is used to hide controls at page load.
    /// </summary>
    private void HideControls()
    {
        lblErrorMesg.Visible = false;
        lblNorecord.Visible = false;
        tblHeader.Visible = false;
        lblSelect.Visible = false;
    }

    /// <summary>
    /// 	This method is used to read directories one by one.
    /// </summary>
    /// <param name="asDirName"> </param>
    /// <param name="asTreeString"> </param>
    /// <param name="aiParentIndex"> </param>
    /// <param name="aiIndex"> </param>
    private void ReadReportDir(string asDirName, ref StringBuilder asTreeString, ref int aiParentIndex, ref int aiIndex)
    {
        asTreeString.AppendFormat("d.add({0},{1},'{2}');\r\n", aiIndex, aiParentIndex, asDirName.Substring(asDirName.LastIndexOf("\\") + 1));
        aiParentIndex += 1;

        //This method gives all reports details.
        DataTable oDSReportDetails = ReportsBL.GetAllReportDetails(moUserRole.ToInt());

        //This method gives all report folder names.
        DataTable oDTReportFolderName = ReportsBL.GetReportFolderName(moUserRole.ToInt());
        if (oDTReportFolderName.Rows.Count <= 0)
            return;
        //This loop is for reading one by one folders under report(i.e. Parent) directory.
        for (int iRowCnt = 0; iRowCnt < oDTReportFolderName.Rows.Count; iRowCnt++)
        {
            if (oDTReportFolderName.Rows[iRowCnt]["IsActive"].ToString() != Constants.I_ONE.ToString())
                continue;
            string sDirName = oDTReportFolderName.Rows[iRowCnt]["Report_Folder_Name"].ToString();
            if (!Settings.ExternalLibrarySite.IsNullOrEmpty() && sDirName != Constants.ReportFolders.Library.ToString() || Settings.ExternalLibrarySite.IsNullOrEmpty())
            {
                aiIndex += 1;
                asTreeString.AppendFormat("d.add({0},{1},'{2}');\r\n", aiIndex, aiParentIndex, sDirName.Substring(0));

                DataRow[] oDTReportFiles = oDSReportDetails.Select(string.Format("Report_Folder_Name='{0}'", sDirName));

                //This method is used to read all files under given subdirectory and returns index which 
                //will increased by one and treat as a parent index for next subdirectory..
                aiIndex = ReadDirForReportFiles(asDirName, asTreeString, aiIndex, sDirName, oDTReportFiles);
            }
        }
    }

    /// <summary>
    /// 	This method is used to read Id's of accessible screens.
    /// </summary>
    /// <returns> </returns>
    private DataSet GetScreenAccessDetails()
    {
        var oSchoolWiseSupervisorBL = new SchoolWiseSupervisorMasterBL();
        DataSet oDsScreenId = oSchoolWiseSupervisorBL.GetScreenAccessDetails(miUserId, miUserId, false);
        return oDsScreenId;
    }

    /// <summary>
    /// 	This method is used to read files from report directory.
    /// </summary>
    /// <param name="asDirName"> </param>
    /// <param name="asTreeString"> </param>
    /// <param name="aiIndex"> </param>
    /// <param name="asSubDirName"> </param>
    /// <param name="oDTReportFiles"> </param>
    /// <returns> int </returns>
    private int ReadDirForReportFiles(string asDirName, StringBuilder asTreeString, int aiIndex, string asSubDirName, DataRow[] oDTReportFiles)
    {
        if (oDTReportFiles.Length > 0)
        {
            //Here iSubgroupIndex will treat as a parent index where aiIndex treat as a child index.
            int iSubgroupIndex = aiIndex;
            string sReportID;
            string sReportDisplayName;
            string sIsSearchGridConsidered;
            aiIndex = aiIndex + 1;
            //This loop is for reading each file of .rpt extension taking from subdirectory 
            //and by using this file tree string is created.
            foreach (DataRow dtRow in oDTReportFiles)
            {
                //Here we take datarow of a particular report which gives Report_Id,Report_name & Report_Display_name.
                string sReportName = dtRow["Report_Name"].ToString();
                sReportID = dtRow["Report_Id"].ToString();
                sReportDisplayName = dtRow["Report_Display_Name"].ToString();
                sIsSearchGridConsidered = dtRow["IsSearchGridConsidered"].ToString();

                string sReportPath = asDirName + "\\" + asSubDirName;
                //Here string is created for building a tree and add to dtree node.
                //asTreeString.AppendFormat("d.add({0},{1},'{2}','SchoolReportUI.aspx?rpt={3}\\\\{4}&d={5}&ReportID={6}&IsSearchGridConsidered={7}&ReportName={2}','', 'Parameters');\r\n", aiIndex, iSubgroupIndex, sReportDisplayName, sReportPath.Replace("\\", "\\\\"), sReportName.Replace("\\", "\\\\"), asDirName, sReportID, sIsSearchGridConsidered);

                string sQueryStrig = CommonUtility.EncryptQuerystring("rpt=" + sReportPath.Replace("\\", "\\\\") + "\\\\" + sReportName.Replace("\\", "\\\\") + "&d=" + asDirName + "&ReportID=" + sReportID + "&IsSearchGridConsidered=" + sIsSearchGridConsidered + "&ReportName=" + sReportName.Replace("\\", "\\\\") + "&Report_Display_Name=" + dtRow["Report_Display_Name"]);
                asTreeString.AppendFormat("d.add(" + aiIndex + "," + iSubgroupIndex + ",'" + sReportDisplayName + "','SchoolReportUI.aspx?" + sQueryStrig + "','', 'Parameters');\r\n");
                aiIndex = aiIndex + 1;
            }
        }
        return aiIndex;
    }

    /// <summary>
    /// 	This method is used to visible controls as well to set report description.
    /// </summary>
    private void SetPropertiesToControls()
    {
        lblDesc.Visible = true;
        //This method get report description for particular report and set to label.
        if (IsPostBack == false || hidIsReportDescription.Value.IsNullOrEmpty())
            hidIsReportDescription.Value = ReportsBL.GetReportDescription(msReportID).Trim();
        lblDesc.Text = string.Format("<span style='color:#000;'>Description:</span></br> {0}", ReportsBL.GetReportDescription(msReportID).Trim());
        btnDisplayReport.Visible = true;
        trNote.Visible = true;
    }

    /// <summary>
    /// 	This method is used to set required field validator and label of mandatory field to appropriate control.
    /// </summary>
    private void SetReqdFieldValidators()
    {
        for (int iGridRowCount = 0; iGridRowCount < grdDisplayParameter.Rows.Count; iGridRowCount++)
        {
            string sIsReqiured = grdDisplayParameter.DataKeys[iGridRowCount][I_ISREQUIRED_INDEX].ToString();
            //Used to set error message for particular control.
            string sDisplayName = grdDisplayParameter.DataKeys[iGridRowCount][I_DISPLAY_NAME_INDEX].ToString();
            var oDropDownList = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
            var olblMandatorySymbol = grdDisplayParameter.Rows[iGridRowCount].FindControl("lblDDLMandatory") as Label;
            //When current control is mandatory then do this.
            if (sIsReqiured == "Y")
            {
                lblManFld.Visible = true;
                //In case of leaving certificate and student details report required field validator 
                //and mandatory field label is set to that control.
                switch (grdDisplayParameter.DataKeys[iGridRowCount][0].ToString())
                {
                    case S_DATETIME:
                        {
                            olblMandatorySymbol.Visible = true;
                            var oRequiredFieldValidator = grdDisplayParameter.Rows[iGridRowCount].FindControl("RFVDatetime") as RequiredFieldValidator;
                            oRequiredFieldValidator.Visible = true;
                            oRequiredFieldValidator.ErrorMessage = string.Format("{0} should not be blank.", sDisplayName);
                        }
                        break;
                    case S_TEXTBOX:
                        {
                            var oRequiredFieldValidator = grdDisplayParameter.Rows[iGridRowCount].FindControl("RFVTxtParamReport") as RequiredFieldValidator;
                            oRequiredFieldValidator.Visible = true;
                            oRequiredFieldValidator.ErrorMessage = string.Format("{0} should not be blank.", sDisplayName);
                            olblMandatorySymbol.Visible = true;
							
                        }
                        break;
                    default:
                        oDropDownList.IsRequired = true;
                        oDropDownList.IsRequiredLabel = true;
                        oDropDownList.ErrorMessage = string.Format("{0} should be selected.", sDisplayName);
                        olblMandatorySymbol.Visible = false;
                        break;
                }
            }
            //When current control is not mandatory then do this.
            else
            {
                oDropDownList.IsRequired = false;
                if ((grdDisplayParameter.DataKeys[iGridRowCount][0].ToString().ToLower() == S_DROPDOWNLIST && msReportID != S_SUBJECT_TOPPERS && msReportID!= S_TESTWISE_SUBJECT_TOPPERS && msReportID != S_SALARY_SLIP && msReportID != S_TRANSFERED_STAFF_SALARY_SLIP && msReportID != S_BANK_LETTER && msReportID != S_STAFF_LEAVES && msReportID != S_FORM_NO_16) || ((msReportID == S_SALARY_SLIP || msReportID == S_TRANSFERED_STAFF_SALARY_SLIP || msReportID == S_STAFF_LEAVES || msReportID == S_FORM_NO_16 || msReportID == S_STAFF_ATTENDANCE || msReportID == S_STAFF_LEAVE_DETAILS_EXPORT || msReportID == S_BANK_LETTER) && (moUserRole == Constants.UserRoles.Admin) || hidHasFullAccess.Value == "1" || msReportID==S_MATERIAL_ISSUE_DETAILS_BY_USER))
                {
                    if (oDropDownList.Items.Count > 0)
                        oDropDownList.Items[0].Text = string.Format("-- {0} --", Constants.S_ALL);
                    else
                        oDropDownList.Items.Add(string.Format("-- {0} --", Constants.S_ALL));
                }
                oDropDownList.IsRequiredLabel = false;
                olblMandatorySymbol.Visible = false;
            }
        }
    }

    /// <summary>
    /// 	This method is used to set event to user control.
    /// </summary>
    private void AddParentFilterEventHandler()
    {
        for (int iGridRowCount = 0; iGridRowCount < grdDisplayParameter.Rows.Count; iGridRowCount++)
        {
            //If current control is a parent control(for implementing filtering), then event is set to that control.
            if (grdDisplayParameter.DataKeys[iGridRowCount]["Is_Parent"].ToString() != "Y")
                continue;
             var oDropDownList = grdDisplayParameter.Rows[iGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
            oDropDownList.ComboChangeEvent += oDropDownList_ComboChangeEvent;
            oDropDownList.ReportFieldId = grdDisplayParameter.DataKeys[iGridRowCount]["Report_Field_Id"].ToString().ToInt();
        }
    }

    /// <summary>
    /// 	This method is used to add "--Select--" item into combobox.
    /// </summary>
    /// <param name="aoDataView"> </param>
    /// <param name="asDisplayMember"> </param>
    /// <param name="asValueMember"> </param>
    /// <returns> </returns>
    public DataView AddTopElementToDataView(DataView aoDataView, string asDisplayMember, string asValueMember)
    {
        // Returns: dataview with a new row inserted and sorted by asDisplayMember.        
        DataRow oDataRow = aoDataView.Table.NewRow();
        oDataRow[asDisplayMember] = S_TOP_ROW;
        oDataRow[asValueMember] = 0;
        aoDataView.Table.Rows.InsertAt(oDataRow, 0);

        // Check if the items in the DataView need to be Title Case.
        int iRowCount;
        for (iRowCount = 0; iRowCount <= aoDataView.Table.Rows.Count - 1; iRowCount++)
            aoDataView.Table.Rows[iRowCount][asDisplayMember] = Convert.ToString(aoDataView.Table.Rows[iRowCount][asDisplayMember]);

        return aoDataView;
    }

    /// <summary>
    /// 	This method is used to create selection formula for report.
    /// </summary>
    /// <param name="asReportSelectionString"> </param>
    /// <param name="asFormatType"> </param>
    private void DisplayReport(string asReportSelectionString, string asFormatType, string sFilePath = "")
    {
        if ((msReportID == S_LC_REPORT_ID || msReportID == S_LEAVING_CERTIFICATE_10TH_NPS_ID) && miSchoolId != Constants.SchoolId.PPSH.ToInt())
       {            
            LCDetailsBL oLCDetailsBL = new LCDetailsBL();
            oLCDetailsBL.AddLCPrintCount(miSchoolId, hidRegNo.Value, miUserId, HidPrintDate.Value.ToString());
       }
       else if (msReportID == S_TRANSFER_CERTIFICATE && miSchoolId == Constants.SchoolId.PPSH.ToInt())
       {
           LCDetailsBL oLCDetailsBL = new LCDetailsBL();
           oLCDetailsBL.AddLCPrintCount(miSchoolId, hidRegNo.Value, miUserId, HidPrintDate.Value.ToString());
       }
		msSchoolName = Session[Constants.S_SESSION_SCHOOL_NAME].ToString();
        var oSchoolAcademicYearBL = new SchoolWiseAcademicYearMasterBL();
        DataTable oDTSchoolInfo = oSchoolAcademicYearBL.GetSchoolInfo(miSchoolId, miAcademicYearId);
        msAcademicYearName = "Year " + oDTSchoolInfo.Rows[Constants.I_ZERO]["Year"];
        msOrgnizationName = oDTSchoolInfo.Rows[Constants.I_ZERO]["School_Orgn_Name"].ToString();

        bool bGenerateReport = true;
        if (msReportPath != null)
        {
            crReportDocument = new ReportDocument();
            var crConnectionInfo = new ConnectionInfo
            {
                ServerName = ConfigurationManager.AppSettings["ReportingDataSource"],
                DatabaseName = ConfigurationManager.AppSettings["ReportDataBaseName"],
                UserID = ConfigurationManager.AppSettings["ReportingUserId"],
                Password = ConfigurationManager.AppSettings["ReportingPassword"]
            };
            
            if (msReportID == S_SALARY_SHEET)
            {
                string[] keys = asReportSelectionString.Split('@');
                foreach (var key in keys)
                {
                    var values = key.Replace("(", string.Empty).Replace(")", string.Empty).Split('=');
                    if (values[0].Contains("PageSize"))
                        msReportPath = msReportPath.Substring(0, msReportPath.IndexOf(".")) + values[1] + ".rpt";
                }
            }
            else if (miSchoolId == Constants.SchoolId.MNS.ToInt() && msReportID == S_STUDENT_IDENTITY_CARDS)
            {
                string[] keys = asReportSelectionString.Split('@');
                foreach (var key in keys)
                {
                    var values = key.Replace("(", string.Empty).Replace(")", string.Empty).Split('=');
                    if (values[0].Contains("IdCardFor") && values[1] == Constants.S_TWO)
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\ParentIdentityCardMNS.rpt";                   
                }
            }
            else if (msReportID == S_STUD_EXAM_RESULT_PPSN && miSchoolId == Constants.SchoolId.SS.ToInt())
            {
                string[] keys = asReportSelectionString.Split('@');
                foreach (var key in keys)
                {
                    var values = key.Replace("(", string.Empty).Replace(")", string.Empty).Split('=');
                    if (values[0].Contains("Term_Id") && values[1] == Constants.S_TWO)
                    {
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentWiseFinalProgressReportCBSEForSS.rpt";
                        break;
                    }
                }
            }
            else if (msReportID == S_ADDITIONAL_FEETYPE_PAYMENT_DETAILS)
            {
                if (asFormatType == "Excel")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentPayableFeeTypeDetailsInExcel.rpt";
            }
            else if (msReportID == S_NETBANKING_REPORT)
            {
                if (asFormatType == "Excel")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\NetBankingCollection_Excel.rpt";
            }
            else if (msReportID == S_STANDARDWISE_CONCESSION_REPORT)
            {
                if (asFormatType == "Excel")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\Standardwise Fee Concession_Excel.rpt";
            }
            else if (msReportID == S_EMPLOYEE_DETAILS)
            {
                if (asFormatType == "Excel")
                {
                    if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\SchoolEmployeeDetailsInExcel - PPSN.rpt";
                    else
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\SchoolEmployeeDetailsInExcel.rpt";
                }
            }
            //else if (msReportID == S_STAFF_LEAVE_DETAILS_EXPORT)
            //{
            //    if (asFormatType == "Excel")
            //        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StaffLeaveReportInExcel.rpt";
            //}
            else if (msReportID == S_STAFF_ATTENDANCE)
            {
                if (asFormatType == "Excel")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StaffAttendanceReportInExcel.rpt";
            }
            else if (msReportID == S_INTERNAL_FEE || msReportID == S_PENDING_INTERNAL_FEE)
            {
                if (asFormatType == "Excel")
                {
                    if(miSchoolId == Constants.SchoolId.PPSN.ToInt())
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentInternalFeePaidInExcel_PPSN.rpt";
                    else
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentInternalFeePaidInExcel.rpt";
                }
            }
            else if (msReportID == S_CCE_REPORT)
            {
                if (miSchoolId == Constants.SchoolId.PPS.ToInt() && miAcademicYearId >= 53)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\CCEReportPP53.rpt";
            }
            else if (msReportID == S_CLASSWISE_STUDENT_LIST)
            {
                if (asFormatType == "Excel")
                {
                    if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\Classwise Students List -PPS- Excel.rpt";
                    else if (miSchoolId != Constants.SchoolId.SNS.ToInt() && miSchoolId != Constants.SchoolId.MVPS.ToInt())
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\Classwise Students List - Excel.rpt";
                    else
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\Classwise Students List - Excel - SNS.rpt";
                }
            }
            else if (msReportID == S_STUD_FINAL_RESULT && miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                if (miAcademicYearId >= 44)
                {
                    int iStandardId = 0;
                    string[] keys = asReportSelectionString.Split('@');
                    foreach (var key in keys)
                    {
                        var values = key.Replace("(", string.Empty).Replace(")", string.Empty).Split('=');
                        if (values[0].Contains("Standard_Id"))
                            iStandardId = values[1].ToInt();
                    }

                    bool bIsGradingstandard = StandardMasterBL.IsGradingStandard(miSchoolId, miAcademicYearId, iStandardId);

                    if (bIsGradingstandard)
                    {
                        if(miAcademicYearId <= 50)
                            msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportGrading.rpt";
                        else
                            msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportGrading51.rpt";
                    }
                    else
                    {
                        if (miAcademicYearId <= 47)
                            msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportMarking.rpt";
                        else
                        {
                            ComboRpt oComboRpt = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                            if (miAcademicYearId >= 51 && oComboRpt.SelectedItem.Text =="9")
                                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportMarking51.rpt";
                            else if (miAcademicYearId >= 51 && (oComboRpt.SelectedItem.Text == "6" || oComboRpt.SelectedItem.Text == "7" || oComboRpt.SelectedItem.Text == "8"))
                                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportMarking51_6to8.rpt";
                            else
                                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportMarking48.rpt";
                        }
                    }
                }
            }

            else if (msReportID == S_LC_REPORT_ID && miSchoolId == Constants.SchoolId.SSN.ToInt())
            {
                string[] keys = asReportSelectionString.Split('@');
                foreach (var key in keys)
                {
                    var values = key.Replace("(", string.Empty).Replace(")", string.Empty).Split('=');
                    if (values[0].Contains("DisplayInMarathi") && values[1].Trim() == Constants.S_ONE)
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\Leaving Certificate SSN_Marathi.rpt";
                }
            }

            else if (msReportID == S_STUD_EXAM_RESULT_PPSN && miSchoolId == Constants.SchoolId.HSP.ToInt())
            {
                string[] keys = asReportSelectionString.Split('@');                                
                int iTermId = Constants.I_ZERO;
                int iStandardId = Constants.I_ZERO;
                foreach (var key in keys)
                {
                    var values = key.Replace("(", string.Empty).Replace(")", string.Empty).Split('=');                    
                    if(values[0].Contains("Standard_Id"))
                        iStandardId = values[1].ToInt();
                    else if(values[0].Contains("Term_Id"))
                        iTermId = values[1].ToInt();
                }
                string sReportName = ReportsBL.GetReportName(miSchoolId, miAcademicYearId, iStandardId, msReportID.ToInt(), iTermId);

                if(sReportName != string.Empty)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\" + sReportName;
            }
            else if (msReportID == S_RESULTSHEET && miSchoolId == Constants.SchoolId.SVP.ToInt())
            {
                int iDivisionId = 0, iTestId = 0, iStandardId = 0;
                string[] keys = asReportSelectionString.Split('@');
                foreach (var key in keys)
                {
                    var values = key.Replace("(", string.Empty).Replace(")", string.Empty).Split('=');
                    if (values[0].Contains("Division_Id"))
                        iDivisionId = values[1].ToInt();
                    else if (values[0].Contains("Test_Id"))
                        iTestId = values[1].ToInt();
                    else if (values[0].Contains("Standard_Id"))
                        iStandardId = values[1].ToInt();
                }

                SubjectwiseStandardExamScheduleBL oSubjectwiseStandardExamScheduleBL = new SubjectwiseStandardExamScheduleBL();
                int iSubjectCount = oSubjectwiseStandardExamScheduleBL.GetSubjectCount(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iTestId);
                string sNewName = string.Empty;

                var cmbStandard = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text == "9" || cmbStandard.SelectedItem.Text == "10")
                    sNewName = msReportPath.Replace(".rpt", string.Empty) + (iSubjectCount > 0 ? iSubjectCount.ToString() : string.Empty) + "_1.rpt";
                else        
                    sNewName = msReportPath.Replace(".rpt", string.Empty) + (iSubjectCount > 0 ? iSubjectCount.ToString() : string.Empty) + ".rpt";
                
                if (File.Exists(sNewName))
                    msReportPath = sNewName;
            }
            else if (msReportID == S_TESTWISE_SUBJECT_MARKS && miSchoolId == Constants.SchoolId.SVNP.ToInt())
            { 
                var cmbStandard = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text == "9" || cmbStandard.SelectedItem.Text == "10")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\TestwiseSubjectMarksSVNP9.rpt";
            }

            else if (msReportID == S_STUD_EXAM_RESULT_PPSN && miSchoolId == Constants.SchoolId.SVNP.ToInt())
            {
                var cmbStandard = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text == "9" || cmbStandard.SelectedItem.Text == "10")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseProgressReportSVNP9.rpt";
            }

            else if (msReportID == S_ANNUAL_CONSOLDATED_UNITTEST_REPORT_SVNP && miSchoolId == Constants.SchoolId.SVNP.ToInt())
            {
                var cmbStandard = grdDisplayParameter.Rows[Constants.I_TWO].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text == "Unit Test")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\AnnualConsolidationReportSVNP_UnitTest.rpt";
            }
            else if (miSchoolId == Constants.SchoolId.SNS.ToInt() && (msReportID == S_STUD_FINAL_RESULT || msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std || msReportID == S_STUD_FINAL_RESULT_FOR_9))
            {
                string sReportName = ReportsBL.GetReportName(miSchoolId, miAcademicYearId, 0, msReportID.ToInt(), 0);
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\" + sReportName;
            }
            else if (miSchoolId == Constants.SchoolId.SNS.ToInt() && (msReportID == S_STUDENT_OBSERVATION_REPORT))
            {
                var cmbStandard = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text == "3" || cmbStandard.SelectedItem.Text == "4" || cmbStandard.SelectedItem.Text == "5")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseProgressReportSNS_3rdTO5th.rpt";
                else if (cmbStandard.SelectedItem.Text == "1" || cmbStandard.SelectedItem.Text == "2")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseProgressReportSNS_1st&2nd.rpt";
            }

            if (msReportID == S_XSEED_REPORT && miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                msReportPath = msReportPath.Replace("Pre-Primary", "XSEED");
            }          

            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                string sReportName = msReportPath.Substring(msReportPath.LastIndexOf("\\")+1);

                if (miAcademicYearId == 51 || miAcademicYearId == 52)               
                {
                    if (sReportName.Trim() == "StudentwiseProgressReport.rpt")
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\" + "StudentwiseProgressReport51.rpt";
                    else if(sReportName.Trim() == "StudentTerm1ProgressReport.rpt")
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\" + "StudentTerm1ProgressReport51.rpt";
                }
                else if (miAcademicYearId >= 53)
                {
                    if (sReportName.Trim() == "StudentwiseProgressReport.rpt")
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\" + "StudentwiseProgressReportPP53.rpt";
                }
            }

            if (miSchoolId == Constants.SchoolId.DPIS.ToInt() && msReportID == S_EXAM_RESULT)
            {
                string sReportName = msReportPath.Substring(msReportPath.LastIndexOf("\\") + 1);
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\" + "StudentwiseProgressReportDPIS.rpt";                
            }

            if (msReportID == S_XSEED_REPORT && Settings.IsAaryanSchool)
            {
                var cmbStandard = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text == "Nursery")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\XseedProgressReportForAaryanNursary.rpt";
                else if(cmbStandard.SelectedItem.Text == "Junior KG")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\XseedProgressReportForAaryanJr.Kg.rpt";
            }

            if (msReportID == S_FEE_RECONCILIATION_REPORT && DDLFormatType.SelectedItem.Text == "Excel")
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\FeeReconciliationReportInExcel.rpt";

            if (msReportID == S_TRANSPORT_READING_ALLOCATION && DDLFormatType.SelectedItem.Text == "Excel")
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\TransportReadingAllocationDetailsInExcel.rpt";

            if (msReportID == S_USERROLEWISE_TRAVELLER_DETAILS && DDLFormatType.SelectedItem.Text == "Excel")
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\UserRolewiseTransportDetailsInExcel.rpt";

            if (msReportID == S_STOPWISE_TRANSPORT_DETAILS && DDLFormatType.SelectedItem.Text == "Excel")
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StopwiseTransportDetailsInExcel.rpt";

            if (msReportID == S_STUD_FINAL_RESULT_PPSH && miAcademicYearId >= 11)
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReport9thStd_PPSH.rpt";

            if (msReportID == S_STUD_FINAL_RESULT_FOR_PPSN && miAcademicYearId >= 10)
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentWiseFinalProgressReportCBSEForPPSN2023.rpt";
            else if (moSchool == Constants.SchoolId.PPSN && msReportID == S_STUD_FINAL_RESULT_FOR_9 && miAcademicYearId >= 10)
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentWiseFinalProgressReport9thStdPPSN2023.rpt";

            if (msReportID == S_STUD_FINAL_RESULT && miAcademicYearId >= 53)
            {
                int iStandardId = 0;
                string[] keys = asReportSelectionString.Split('@');
                foreach (var key in keys)
                {
                    var values = key.Replace("(", string.Empty).Replace(")", string.Empty).Split('=');
                    if (values[0].Contains("Standard_Id"))
                        iStandardId = values[1].ToInt();
                }

                bool bIsGradingstandard = StandardMasterBL.IsGradingStandard(miSchoolId, miAcademicYearId, iStandardId);
                if (!bIsGradingstandard)
                {
                    if (miAcademicYearId <= 55)
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\FinalProgressReportPP.rpt";
                    else
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\FinalProgressReportPP2026.rpt";
                }
                else
                {
                    var cmbStandard = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                    if (miAcademicYearId >= 54 && cmbStandard.SelectedItem.Text == "5")
                    {
                        if (miAcademicYearId <= 55)
                            msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportGradingFor5th_2024.rpt";
                        else
                            msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportGradingFor5th_2026.rpt";
                    }
                    else if (miAcademicYearId <= 55)
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportGrading2023.rpt";
                    else
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportGrading2026.rpt";
                }
            }

            if (msReportID == S_STUDENT_OBSERVATION_REPORT && moSchool == Constants.SchoolId.SNS)
            {
                var cmbStandard = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text == "5")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseProgressReportSNS_5th2023.rpt";
                else
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseProgressReportSNS_3rdTO5th2023.rpt";
            }

            if (msReportID == S_TRANSPORT_NOTIFICATIONS)
            {
                TransportNotificationBL oTransportNotificationBL = new TransportNotificationBL();
                string DBName = ConfigurationManager.AppSettings["ReportDataBaseName"].ToString();
                oTransportNotificationBL.CopyTransportNotification(miSchoolId, DBName);
            }

            if (miSchoolId == Constants.SchoolId.PPS.ToInt() && msReportID == S_CCE_REPORT)
            {
                if (miAcademicYearId >= 53 && miAcademicYearId <= 54)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\CCEReportPP_53.rpt";
                else if (miAcademicYearId >= 55)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\CCEReportPP_55.rpt";
            }

            if (miSchoolId == Constants.SchoolId.VPMCPS.ToInt() && msReportID == S_LC_REPORT_ID)
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\LeavingCertificateVPMCPS.rpt";

            if (miSchoolId == Constants.SchoolId.SNS.ToInt() && msReportID == S_STUDENT_OBSERVATION_REPORT && miAcademicYearId >= 8)
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseProgressReportSNS_3rdTO5th2024.rpt";

            if (moSchool == Constants.SchoolId.VPMCPS && msReportID == S_STUD_FINAL_RESULT)
            {
                var cmbStandard = grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text != "9")
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\FinalProgressReportVPMCPS5To8.rpt";
            }

            if(msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_MNS && miAcademicYearId >= 8 && moSchool == Constants.SchoolId.MNS)
            {
                var cmbTerm = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbTerm.SelectedItem.Text == "Term-II")
                {
                    var cmbStandard = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                    if (cmbStandard.SelectedItem.Text == "Play Group" || cmbStandard.SelectedItem.Text == "Nursery" || cmbStandard.SelectedItem.Text == "Junior KG" || cmbStandard.SelectedItem.Text == "Senior KG")
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseProgressReportMNS_PP2024.rpt";
                    else
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseProgressReportMNS2024.rpt";
                }
            }
            else if (msReportID == S_STUD_FINAL_RESULT_PPSH_Old && moSchool == Constants.SchoolId.DPIS)
            {
                if(miAcademicYearId >= 5)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentWiseFinalProgressReportForDPIS2024.rpt";
            }
            else if (moSchool == Constants.SchoolId.BFS)
            {
                var cmbStandard = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                if (miAcademicYearId >= 10)
                {
                    if (msReportID == S_STUD_FINAL_RESULT)
                    {
                        if (cmbStandard.SelectedItem.Text == "9")
                            msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportBFS2024.rpt";
                        else
                            msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentFinalProgressReportBFS2024_5to8.rpt";
                    }
                    
                    if (msReportID == S_STUD_TERM1_RESULT)
                        msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentTerm1ProgressReportForBFS.rpt";
                }                
            }
            else if (moSchool == Constants.SchoolId.MVPS && msReportID == S_STUD_FINAL_RESULT_PPSH_Old)
            {
                var cmbStandard = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                if(miAcademicYearId >= 8)
                {
                    if (cmbStandard.SelectedItem.Text == "5" || cmbStandard.SelectedItem.Text == "8")
                            msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentWiseProgressReportMVPS_Final5and8.rpt";
                }
            }
            else if (msReportID == "15" && moSchool == Constants.SchoolId.SNS)
            {
                var cmbStandard = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text.Contains("11 ") || cmbStandard.SelectedItem.Text.Contains("12 "))
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\Exam Result SNS.rpt";
                else if (cmbStandard.SelectedItem.Text.Contains("9") || cmbStandard.SelectedItem.Text.Contains("10"))
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\ExamResult9thStd.rpt";
            }


            if (msReportID == S_CLASSWISE_EXAM_PERFORMANCE && DDLFormatType.SelectedItem.Text == "PDF")
            {
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\ClasswiseTestPerformanceGraph.rpt";
                asFormatType = "PDF";
            }

            if (msReportID == S_ANNUAL_CONSOLDATED_REPORT_SNS)
            {
                var cmbStandard = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                if (cmbStandard.SelectedItem.Text.Contains("11 ") || cmbStandard.SelectedItem.Text.Contains("12 "))
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\AnnualConsolidationReportSNS11To12.rpt";
            }

            if (msReportID == S_STUDENT_TERM1_PROGRESS_REPORT && moSchool == Constants.SchoolId.DPIS)
            {
                if(miAcademicYearId >= 6)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentWiseTermProgressReportForDPIS_2024.rpt";
            }

            if (msReportID == S_STUDENT_OBSERVATION_REPORT && moSchool == Constants.SchoolId.SNS)
            {
                var cmbTerm = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;
                if (miAcademicYearId >= 10)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseTermProgressReportSNS_1rdTO5th2024.rpt";
            }

            if (msReportID == S_PRELIM_REPORT_PP)
            {
                if(miAcademicYearId >= 55)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\PrelimProgressReportPP2025.rpt";
            }

            if (msReportID == S_STUDENT_IDENTITY_CARDS && moSchool == Constants.SchoolId.PIONEER)
            {
                if (miAcademicYearId >= 2)
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentIdentityCardPioneer.rpt";
            }
            if (msReportID == S_PREPRIMARY_STUDENT_TERM1 && moSchool == Constants.SchoolId.VPMCPS)
            {
                var cmbTerm = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;

                if (cmbTerm.SelectedItem.Text == "Term-II")
                {
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentTerm2PrePrimaryReport.rpt";
                }
            }
            if (msReportID == S_TERM_PROGRESS_REPORT_PIONEER && moSchool == Constants.SchoolId.PIONEER)
            {
                var cmbTerm = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;

                if (cmbTerm.SelectedItem.Text == "Yearly Exam")
                {
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentwiseTerm2ProgressReportPrimaryPioneer.rpt";
                }
            }
            if (msReportID == S_STUD_TERM2_RESULT  && moSchool == Constants.SchoolId.BFS )
            {
                 if (miAcademicYearId >= 12)
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentTerm1ProgressReport_BFS.rpt";
            }
            if (msReportID == S_STUD_FINAL_RESULT_PPSH_Old && moSchool == Constants.SchoolId.DPIS && miAcademicYearId >= 6)
                msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\StudentWiseFinalProgressReportForDPIS2025.rpt";
            else if (msReportID == S_EXAMWISE_MARK_DETAILS)
            {
                List<string> lstStd = new List<string> {"1","2","3","4","5"};
                var cmbStandard = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                if (!lstStd.Contains(cmbStandard.SelectedItem.Text))
                    msReportPath = msReportPath.Substring(0, msReportPath.LastIndexOf("\\")) + "\\ExamwiseMarkDetails_6To12.rpt";
            }
            
            crReportDocument.Load(msReportPath);

            if (msReportID == S_STUD_FINAL_RESULT || msReportID == S_STUD_FINAL_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_MCPS || msReportID == S_EXAM_RESULT || msReportID == S_STUD_TERM1_RESULT || msReportID == S_STUD_TERMWISE_RESULT || msReportID == S_EXAM_RESULT_FBS || msReportID == S_EXAM_RESULT_PPSN || msReportID == S_STUD_PRELIMINARY_RESULT || msReportID == S_STUD_TERM2_RESULT || msReportID == S_PRELIM_REPORT_PP || (msReportID == S_STUDENT_TERM1_PROGRESS_REPORT && moSchool == Constants.SchoolId.VPMCPS) || msReportID == S_HOLISTIC_FINAL_PROGRESS_CARD || msReportID == S_PREPRIMARY_STUDENT_TERM1 || msReportID == S_HOLISTIC_REPORT_FOR1TO3_PPSH || msReportID==S_Holistic_Progress_Report_6to7_SNS)
            {
                bGenerateReport = SetProgressReportDataSource(asReportSelectionString);
            }
            else
            {
                using (CrystalDecisions.CrystalReports.Engine.Tables crTables = crReportDocument.Database.Tables)
                {
                    foreach (CrystalDecisions.CrystalReports.Engine.Table ocrTable in crTables)
                    {
                        var crtableLogoninfo = new TableLogOnInfo();
                        crtableLogoninfo = ocrTable.LogOnInfo;
                        crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                        ocrTable.ApplyLogOnInfo(crtableLogoninfo);
                    }
                }
                //This method add the parameters to the report.
                ApplyParametersToCrystalReport(asReportSelectionString);
                switch (msReportID)
                {
                    case S_DUE_DATE_PASSED_BOOK_RPT_ID:
                        {
                            string dtToday = DateTime.Today.ToShortDateString();
                            asReportSelectionString = string.Format("{0}Date({{vw_IssuedBookDetails_Report.Return_Date}})<#{1}#     ", asReportSelectionString, dtToday);
                        }
                        break;
                    case S_SALARY_BANK_STATEMENT:
                    case S_EXPORT_STUDENT_LIST:
                    case S_STUDENT_FEE_REPORT:
                    case S_DATEWISE_Fee_COLLECTION:
                    case S_AREAWISE_PENDINGFEE_DETAILS:
                    case S_STUDENT_PENDING_FFE_DETAILS: 
                    case S_STUDENT_EXCESS_FEE_DETAILS:
                    case S_LEFT_STUDENT_DETAIL:
                    case S_EMPLOYEE_INFORMATION_FOR_REPORT:
                    case S_STAFF_SCREEN_ACCESS_DETAILS:
                    case S_NEXT_YEAR_PAID_FEE:
                    case S_STAFF_KID_FEE:
                    case S_EXAM_CONFIG_DETAILS:
                    case S_STUDENT_TRANSFER_DETAILS:
                    case S_USER_LOGIN_DETAILS:
                    case S_GRADUTY_REPORT_DETAILS:
                    case S_EXTERNAL_STUDENT_FEE_DETAILS:
                    case S_STUDENT_REFUND_FEE_DETAILS:
                    case STUDENT_DOCUMNET_STATUS_DETAILS:
                    case S_MONTHLY_FEE_COLLECTION_DETAILS:
                    case S_EXPORT_FEE_DETAILS:
                    case S_STAFF_BIRTHDAY_LIST :
                    case S_STUDENTS_FEE_DETAILS_REPORT:
                    case S_LAST_ACADEMICYEAR_FEE_DETAILS:
                    case S_STUDENT_NEWADMISSION_DETAILS_EXPORT:
                    case S_EXPORT_ADMISSION_DETAILS:
                    case S_STAFF_LEAVE_DETAILS_EXPORT:
                    case S_STUDENT_STREAM_DETAILS : 
                    case S_CLASSWISE_ATTENDANCE_AVERAGE_REPORT:
                    case S_CAUTION_MONEY_PAYMENT_DETAILS:
                    case S_MATERIAL_ISSUE_DETAILS_BY_USER:
                    case S_BUS_ATTENDANCE:
                    case S_TRANSPORT_NOTIFICATIONS:
                    case S_PARENT_OCCUPATION_DETAILS:
                    case S_STUDENT_FEE_CONSOLIDATED_DETAILS:
                    case S_FEE_RECONCILIATION_REPORT_PPSH:
					case S_EXPORT_FEE_DETAILS_SNS:
                    case S_EXPORT_STUDENT_MONTHLY_STATUS:
                    case S_CA_RECONSOLIDATION_DETAILS:
                    case S_VEHICLES_FUEL_MAINTENANCE_EXPENSES:
                        asFormatType = "Excel";
                        break;
                }

                if (msReportID == S_CAUTION_MONEY_DETTAILS && miSchoolId == Constants.SchoolId.SNS.ToInt() )
                    asFormatType = "Excel";

                string sRecordSelectionFormula = asReportSelectionString.Replace("@", " AND ");

                if (msReportID == S_STUDENT_DOCUMENT_DETAILS)
                    crReportDocument.DataDefinition.RecordSelectionFormula = "";

                if (sRecordSelectionFormula != string.Empty && sRecordSelectionFormula != null && msType == "View")
                    crReportDocument.DataDefinition.RecordSelectionFormula = sRecordSelectionFormula.Remove(sRecordSelectionFormula.Length - 5);
            }
        }

		//For Salary Sheet Report and Pending Fee Details Report set Logo formula field.
        if (miSchoolId != Constants.SchoolId.JPS.ToInt())
        {
            if (msReportID == S_SALARY_SHEET || msReportID == S_PENDING_FEE_DETAILS)
            {
                crReportDocument.DataDefinition.FormulaFields["Logo"].Text = "'" + Request.Url.Scheme + "://" + Request.Url.Authority + Constants.S_SCHOOL_LOGO_FILE_PATH + "'";
            }
        }

        if (bGenerateReport)
            GeneratReport(asFormatType, sFilePath);
        else
            SetNoRecordsErrMsg(Constants.I_ZERO);
    }

    /// <summary>
    /// 	Sets the datasource for Final Progress Report.
    /// </summary>
    /// <param name="asReportSelectionString"> </param>
    private bool SetProgressReportDataSource(string asReportSelectionString)
    {
        asReportSelectionString = FormatFilterString(asReportSelectionString);
        String[] sFilters = asReportSelectionString.Split('@');
        string sParameterValue;
        string sParameterField;
        int iStandardId = 0;
        int iDivisionId = 0;
        int iStudentId = 0;
        int iTestId = 0;
        string sNote = string.Empty;
        int iTermId = 0;
        int iIsFromReportScreen = 0;
        foreach (string filter in sFilters)
        {
            if (filter.Equals(string.Empty))
                continue;

            sParameterValue = filter.Substring(filter.LastIndexOf("=") + 1);
            
            if (!filter.Contains(hidSchemaName.Value) || hidSchemaName.Value.Trim() == string.Empty)
                sParameterField = filter.Substring(filter.IndexOf(".") + 1, filter.LastIndexOf("=") - filter.IndexOf(".") - 1).Trim();
            else
                sParameterField = filter.Substring(filter.LastIndexOf(".") + 1, filter.LastIndexOf("=") - filter.LastIndexOf(".") - 1).Trim();
            
            if (msReportID == S_PRELIM_REPORT_PP && sParameterField == "StudentId" && sParameterValue.Trim() == "null")
                sParameterValue = "0";

            switch (sParameterField)
            {
                case "Standard_Id":
                    iStandardId = sParameterValue.ToInt();
                    break;
                case "Division_Id":
                    iDivisionId = sParameterValue.ToInt();
                    break;
                case "StudentId":
                    iStudentId = (sParameterValue.Trim() == "null" ? 0 : sParameterValue.ToInt());
                    break;
                case "Note":
                    sNote = Convert.ToString(sParameterValue);
                    break;
                case "Term_Id":
                    iTermId = sParameterValue.ToInt();
                    break;
                case "IsFromReportScreen":
                    iIsFromReportScreen = sParameterValue.ToInt();
                    break;
                case "TestId":
                    iTestId = sParameterValue.ToInt();
                    break;
                case "Student_Id":
                    iStudentId = (sParameterValue.Trim() == "null" ? 0 : sParameterValue.ToInt());
                    break;
            }
        }

        DataSet dsProgressReportDetails = new DataSet();
        switch (msReportID)
        {
            case S_EXAM_RESULT:
                dsProgressReportDetails = ReportsBL.GetGradingProgressReportDataSet(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, iIsFromReportScreen);
                break;
            case S_STUD_FINAL_RESULT:
                dsProgressReportDetails = ReportsBL.GetProgressReportDataSet(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, true);
                break;
            case S_STUD_FINAL_RESULT_PPSN:
                dsProgressReportDetails = ReportsBL.GetProgressReportDataSetForPPSN(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote);
                break;
            case S_STUD_FINAL_RESULT_MCPS:
                dsProgressReportDetails = ReportsBL.GetProgressReportDataSetForMCPS(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote);
                break;
            case S_STUD_TERM1_RESULT:
                iTermId = 1;
                dsProgressReportDetails = ReportsBL.GetMarkingSystemProgressReportDataSet(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, iIsFromReportScreen);
                break;
            case S_STUD_TERMWISE_RESULT:
                dsProgressReportDetails = ReportsBL.GetTermwiseProgressReportDataSet(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, true);
                break;
            case S_STUD_TERM2_RESULT:
                iTermId = 2;
                dsProgressReportDetails = ReportsBL.GetMarkingSystemProgressReportDataSet(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, iIsFromReportScreen);
                break;
            case S_EXAM_RESULT_FBS:
                dsProgressReportDetails = ReportsBL.GetGradingProgressReportDataSetForFBS(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId);
                break;
            case S_STUD_PRELIMINARY_RESULT:
                dsProgressReportDetails = ReportsBL.GetPreliminaryExaminationProgressReportDataSet(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, true);
                break;
            case S_EXAM_RESULT_PPSN:
                dsProgressReportDetails = ReportsBL.GetGradingProgressReportDataSetForPPSN(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId);
                break;
            case S_PRELIM_REPORT_PP :
                dsProgressReportDetails = ReportsBL.GetPrelimProgressReportDataSetForPP(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, 2, true);
                break;
            case S_STUDENT_TERM1_PROGRESS_REPORT:
               iTermId = 1;
               dsProgressReportDetails = ReportsBL.GetTerm1ProgressReportDataSet(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, sNote, iTermId, iIsFromReportScreen);
                break;
            case S_HOLISTIC_FINAL_PROGRESS_CARD:
                dsProgressReportDetails = ReportsBL.GetDetailsForHolisticReport(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, iTestId);
                break;
            case S_PREPRIMARY_STUDENT_TERM1:
                dsProgressReportDetails = ReportsBL.GetDetailsForPrePrimaryTerm1Report(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId,iTermId);
                break;
            case S_HOLISTIC_REPORT_FOR1TO3_PPSH:
                dsProgressReportDetails = ReportsBL.GetDetailsForHolisticReportForPPSH(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId, iTermId, true);
                break;
            case S_Holistic_Progress_Report_6to7_SNS:
                dsProgressReportDetails = ReportsBL.GetDetailsForHolisticReportFor6to8SNS(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iStudentId);
                break;
        }

        crReportDocument.SetDataSource(dsProgressReportDetails);

        return dsProgressReportDetails.Tables.Count > Constants.I_ZERO && dsProgressReportDetails.Tables[1].Rows.Count > Constants.I_ZERO;
    }

    /// <summary>
    /// 	This method is for exporting the report in selected format.
    /// </summary>
    private void GeneratReport(string asFormatType, string sFilePath = "")
    {
        int iTestCount = SchoolwiseStandardTestMasterBL.GetTestCount(miSchoolId, miAcademicYearId);
        int iStandard_Division_Id = hidStandardDivisionId.Value.ToInt();
        int iGeneratedTestCount = SchoolwiseStandardTestMasterBL.GetGeneratedTestCount(miSchoolId, miAcademicYearId, iStandard_Division_Id);
        int iUnGeneratedTestCnt = iTestCount - iGeneratedTestCount;
        int iMaxWidth = 8020;
        if (iUnGeneratedTestCnt > 0)
        {
            for (int i = 0; i < crReportDocument.ReportDefinition.ReportObjects.Count; i++)
            {
                if (crReportDocument.ReportDefinition.ReportObjects[i].Name != "SubRptOfActivitySubjectsForProgReport")
                    continue;
                for (int iCount = 0; iCount < iUnGeneratedTestCnt; iCount++)
                {
                    iMaxWidth = iMaxWidth - 720;
                    crReportDocument.ReportDefinition.ReportObjects[i].Left = iMaxWidth;
                }
            }
        }

        switch (asFormatType)
        {
            case "Excel":
                crReportDocument.ExportToHttpResponse(ExportFormatType.Excel, Response, true, Guid.NewGuid().ToString());
                break;
            case "PDF":
                if (mbExportReportZip && !sFilePath.IsNullOrEmpty())
                {
                    crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, sFilePath);
                    crReportDocument.Close();
                    crReportDocument.Dispose();
                }
                else
                {
                    crReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Guid.NewGuid().ToString());
                }
                break;
            case "MS Word":
                crReportDocument.ExportToHttpResponse(ExportFormatType.RichText, Response, true, Guid.NewGuid().ToString());
                break;
            default:
                crReportDocument.ExportToHttpResponse(ExportFormatType.CrystalReport, Response, true, Guid.NewGuid().ToString());
                break;
        }
    }

    /// <summary>
    /// This method is used for create zip file.
    /// </summary>
    /// <param name="sDirectoryName"></param>
    private void CreateZipFile(string sDirectoryName)
    {
        string sDirectoryPath = Server.MapPath(Constants.S_DOWNLOADS_FOLDER_RELATIVE_PATH + "/Reports" + "//" + sDirectoryName);
        string sZipFileName = sDirectoryName + ".zip";
        string sZipFilePath = Server.MapPath(Constants.S_DOWNLOADS_FOLDER_RELATIVE_PATH + "/Reports" + "//" + sZipFileName);

        DeleteExistingZipFiles();

        if (File.Exists(sZipFilePath))
            File.Delete(sZipFilePath);
        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(sZipFilePath))
        {
            zip.AddDirectory(sDirectoryPath, sDirectoryName);
            zip.Save();
        }

        Response.Write("<Script language='javascript'> window.open('" + Constants.S_DOWNLOADS_FOLDER_RELATIVE_PATH.Substring(1) + "/Reports/" + sZipFileName + "')</Script>");
        //hidIsReportGenerated.Value = Constants.S_YES;
        Directory.Delete(sDirectoryPath, true);
    }

    /// <summary>
    /// This mrthod is used to delete existing zip files.
    /// </summary>
    private void DeleteExistingZipFiles()
    {
        string sReportsFolderPath = Server.MapPath(Constants.S_DOWNLOADS_FOLDER_RELATIVE_PATH + "/Reports");
        foreach (string sFilePath in Directory.GetFiles(sReportsFolderPath))
        {
            FileInfo oFile = new FileInfo(sFilePath);
            if (oFile.CreationTime < DateTime.Now.AddMinutes(-30))
                File.Delete(sFilePath);
        }
    }

    #endregion

    #region Grid Controls

    /// <summary>
    /// 	This method fills the grid with controls. Datatype 'checkboxlist'(S_NVARCHAR_CONTROL) is for the Checklistbox control. Datatype 'drodownlist'(S_NUMERIC_CONTROL) is for DropdownListBox control. Datatype 'textbox'(S_TEXTBOX_CONTROL) is for TextBox control. Datatype 'Datetime' (S_DATETIME_CONTROL) is for calendar control. If the DataType is null then no any control is filled to grid.
    /// </summary>
    private void FillGridWithControls()
    {
        DataSet oDSFilterParameters = null;
        for (int iGridRowCount = 0; iGridRowCount < grdDisplayParameter.Rows.Count; iGridRowCount++)
        {
            bool bIsPayrollReport = mlstPayrollReports.Contains(msReportID);
            // Retrive report parameters if any of the following condition is true.
            // 1) Filter is not dependant on any other filter.
            // 2) Report is a Payroll related report
            // 3) Report is Survey Analysis Count report
            if (string.IsNullOrEmpty(grdDisplayParameter.DataKeys[iGridRowCount]["Is_Dependent"].ToString()) || grdDisplayParameter.DataKeys[iGridRowCount]["Is_Dependent"].ToString() == "N" || bIsPayrollReport || msReportID == S_SERVEY_ANALYSIS_COUNT_REPORT)
                oDSFilterParameters = RetriveReportParameters(iGridRowCount);
            //Here we check the transport module is enable. If not then removed the role from rolecombo.
            if (!Settings.EnableTransportModule && (msReportID == S_USER_ROLEWISE_IDENTITY_CARDS || msReportID == S_USER_ROLEWISE_IDENTITY_CARDS_NEW || msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS || msReportID == S_EMPLOYEE_INFORMATION_DETAILS || msReportID == S_USERROLEWISE_TRAVELLER_DETAILS) && !oDSFilterParameters.IsNull() && oDSFilterParameters.Tables.Count > 0)
            {
                DataRow[] oDataRows = oDSFilterParameters.Tables[0].Select("Value_Member=" + Constants.UserRoles.TransportStaff.ToInt());
                if (oDataRows.Length > 0)
                {
                    oDataRows[0].Delete();
                    oDSFilterParameters.AcceptChanges();
                }
            }

            string sDataType = grdDisplayParameter.DataKeys[iGridRowCount][I_DATATYPE_INDEX].ToString().ToLower();
            //According to datatype, control is added to grid.
            switch (sDataType)
            {
                case S_CHECKBOXLIST:
                    AddCheckBoxListToGrid(oDSFilterParameters, iGridRowCount);
                    break;
                case S_DROPDOWNLIST:
                    AddDropDownListToGrid(oDSFilterParameters, iGridRowCount);
                    break;
                case S_TEXTBOX:
                    AddTextBoxToGrid(iGridRowCount);
                    break;
                case S_DATETIME:
                    AddDateTimeControlToGrid(iGridRowCount);
                    break;
                default:
                    grdDisplayParameter.Visible = false;
                    break;
            }
        }

        SetSalarySlipState();       
    }

    /// <summary>
    /// This method is used to set salary slip report status.
    /// </summary>
    private void SetSalarySlipState()
    {
        if (hidHasFullAccess.Value == "1" && msReportID == S_SALARY_SLIP)
        {
            var oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;

            if (oDropDownList.Items.Count == 2)
            {
                oDropDownList.Items[1].Selected = true;
                oDropDownList.Enabled = false;
                oDropDownList_ComboChangeEvent(oDropDownList, null);
            }
        }
    }

    /// <summary>
    /// 	This method is used to add datetime control to grid.
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    private void AddDateTimeControlToGrid(int aiGridRowCount)
    {
        var oTextBox1 = grdDisplayParameter.Rows[aiGridRowCount].FindControl("cRptParameter") as TextBox;
        oTextBox1.Visible = true;
        oTextBox1.ReadOnly = true;
        var oPopCalendar = grdDisplayParameter.Rows[aiGridRowCount].FindControl("CalenderRptParameter") as PopCalendar;
        oPopCalendar.Visible = true;
        oPopCalendar.DateValue = DateTime.Now.ToString("yyyy-MM-dd").ToDateTime();
        if (msReportID == S_BONAFIDE_ISSUE_REGISTER)
        {
            ReportsBL oReportBL= new ReportsBL();
            DataTable dt = oReportBL.GetAcademicYearDate(miSchoolId,miAcademicYearId);
            if(aiGridRowCount == 0)
                oPopCalendar.DateValue = Convert.ToDateTime(dt.Rows[0][0].ToDateTime().ToString("yyyy-MM-dd"));
            if(aiGridRowCount == 1)
                oPopCalendar.DateValue = Convert.ToDateTime(dt.Rows[0][1].ToDateTime().ToString("yyyy-MM-dd"));
        }

        //if (!molstPayrollDateReports.Contains(msReportID) || (hidHasFullAccess.Value != Constants.S_ONE && moUserRole != Constants.UserRoles.Admin))
        //    oPopCalendar.AutoPostBack = PopCalendar.AutoPostBackEnum.False;

        if (!molstPayrollDateReports.Contains(msReportID) && msReportID != S_DYNAMIC_PENDING_FEE_REPORT)
            oPopCalendar.AutoPostBack = PopCalendar.AutoPostBackEnum.False;

        var oChkAll = grdDisplayParameter.Rows[aiGridRowCount].FindControl("ChkAll") as HtmlInputCheckBox;
        oChkAll.Visible = false;

        if (msReportID == S_DAILY_FEE_COLLECTION)
        {
            oTextBox1.Text = string.Empty;
        }
    }

    /// <summary>
    /// 	This method is used to add textbox control to grid.
    /// </summary>
    /// <param> <name>iCount</name> </param>
    /// <param name="aiGridRowCount"> </param>
    private void AddTextBoxToGrid(int aiGridRowCount)
    {
        var oTextBox = grdDisplayParameter.Rows[aiGridRowCount].FindControl("txtRptParameter") as TextBox;
        oTextBox.Text = string.Empty;
        oTextBox.EnableViewState = true;
        oTextBox.Visible = true;
        var oChkAll = grdDisplayParameter.Rows[aiGridRowCount].FindControl("ChkAll") as HtmlInputCheckBox;
        oChkAll.Visible = false;
        if (moUserRole == Constants.UserRoles.Student)
        {
            string sRegNo = Session[Constants.S_SESSION_STUDENT_REGISTRATION_NUM].ToString();
            oTextBox.Text = sRegNo;
            oTextBox.Enabled = false;
        }

        if (msReportID == S_PROFESSIONAL_TAX_CHALLAN)
            oTextBox.MaxLength = 50;
        else if (msReportID == S_BONAFIDE_CERTIFICATE_REPORT_ID && miSchoolId == Constants.SchoolId.PPSH.ToInt())
            oTextBox.MaxLength = 200;
        if (msReportID == S_PENDING_FEE_DETAILS && aiGridRowCount != 5)
        {
            oTextBox.TextMode = TextBoxMode.MultiLine;
            oTextBox.Rows = 4;
            oTextBox.CssClass = "LrgTxtBox";
            oTextBox.Width = 300;
            oTextBox.Height = 60;
            var oRegularExpressionValidator = grdDisplayParameter.Rows[aiGridRowCount].FindControl("Reg_Expr_ValidContent") as RegularExpressionValidator;
            oRegularExpressionValidator.Visible = true;
        }
        if (msReportID == S_STUD_INTERNAL_ASSESSEMENT_DETAILS)
        {
            oTextBox.MaxLength = aiGridRowCount == 3 ? 1 : 2;
            oTextBox.Attributes.Add("onblur", "extractNumber(this,0,false);");
            oTextBox.Attributes.Add("onkeyup", "extractNumber(this,0,false);");
            oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, false, false);");
            oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
            oTextBox.Attributes.Add("ondrop", "event.returnValue=false");
            var oRegularExpressionValidator = grdDisplayParameter.Rows[aiGridRowCount].FindControl("Reg_Expr_ForProgressCardRemark") as RegularExpressionValidator;

            oRegularExpressionValidator.Visible = true;
        }
        if (msReportID == S_PENDING_FEE_DETAILS && aiGridRowCount == 5)
        {
            oTextBox.MaxLength = 10;
            oTextBox.Attributes.Add("onblur", "extractNumber(this,0,false);");
            oTextBox.Attributes.Add("onkeyup", "extractNumber(this,0,false);");
            oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, false, false);");
            oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
            oTextBox.Attributes.Add("ondrop", "event.returnValue=false");
            var oRegularExpressionValidator = grdDisplayParameter.Rows[aiGridRowCount].FindControl("Reg_Expr_ForProgressCardRemark") as RegularExpressionValidator;

            oRegularExpressionValidator.Visible = true;
        }
        if (msReportID == S_CAUTION_MONEY_DETTAILS)
        {
            oTextBox.MaxLength = 7;
            oTextBox.Attributes.Add("onblur", "extractNumber(this,0,false);");
            oTextBox.Attributes.Add("onkeyup", "extractNumber(this,0,false);");
            oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, false, false);");
            oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
            oTextBox.Attributes.Add("ondrop", "event.returnValue=false");
            var oRegularExpressionValidator = grdDisplayParameter.Rows[aiGridRowCount].FindControl("Reg_Expr_ForProgressCardRemark") as RegularExpressionValidator;
            oRegularExpressionValidator.Visible = true;
            if (aiGridRowCount == 0 && oTextBox !=null)
            {
                oTextBox.Focus();
                SetDefaultButton(btnDisplayReport);
            }
        }
        if (msReportID == S_STUDENTS_ANNUAL_ATTENDANCE || msReportID == S_MONTHWISE_STUDENT_ATTENDANCE)
        {
            oTextBox.MaxLength = 3;
            oTextBox.Attributes.Add("onblur", "extractNumber(this,0,false);");
            oTextBox.Attributes.Add("onkeyup", "extractNumber(this,0,false);");
            oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, false, false);");
            oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
            oTextBox.Attributes.Add("ondrop", "event.returnValue=false");
        }
        if (msReportID == S_EXAM_RESULT_SS || msReportID == S_EXAM_RESULT_STSS_9STD || msReportID == S_EXAM_RESULT_STSS_10STD || msReportID == S_STUD_FINAL_RESULT_SS || msReportID == S_EXAM_RESULT || msReportID == S_STUD_FINAL_RESULT || msReportID == S_STUD_FINAL_RESULT_PPSH || msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std || msReportID == S_STUD_FINAL_RESULT_PPSH_Old || msReportID == S_STUD_FINAL_RESULT_FOR_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_9 || msReportID == S_STUD_FINAL_RESULT_FOR_11 || msReportID == S_STUD_FINAL_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_MCPS || msReportID == S_STUD_TERM2_RESULT || msReportID == S_STUD_TERM1_RESULT || msReportID == S_STUD_TERMWISE_RESULT || msReportID == S_EXAM_RESULT_FBS || msReportID == S_EXAM_RESULT_PPSN || msReportID == S_STUD_PRELIMINARY_RESULT || msReportID == S_ENROLLMENTWISE_STUDENT_I_CARDS || msReportID == S_ENROLLMENTWISE_STUDENT_AUTHORITY_CARDS || msReportID == S_FINAL_REPORT_JPS || msReportID == S_FINAL_REPORT_GSS || msReportID == S_STUD_EXAM_RESULT_PPSN || msReportID == S_FEE_RECEIPT_DETAILS || msReportID == S_STUD_EXAM_RESULT_MVPS_9 || msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_PEMS || msReportID == S_STUDENT_PROGRESS_REPORT_CBSE || msReportID == S_COSCHOLASTIC_SUBJECT_MARK_DETAILS || msReportID == S_PERIODIC_TEST_MARK_DETAILS || msReportID == S_STUDENTS_FEE_DETAILS_REPORT)
        {
            oTextBox.TextMode = TextBoxMode.MultiLine;
            oTextBox.Rows = 4;
            oTextBox.CssClass = "LrgTxtBox";
            oTextBox.Width = 300;
            oTextBox.Height = 60;
            if (msReportID == S_EXAM_RESULT_SS || msReportID == S_EXAM_RESULT_STSS_9STD || msReportID == S_EXAM_RESULT_STSS_10STD || msReportID == S_STUD_FINAL_RESULT_SS || msReportID == S_STUD_EXAM_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_9 || msReportID == S_STUD_FINAL_RESULT_FOR_11 || msReportID == S_STUD_EXAM_RESULT_MVPS_9 || msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_PEMS || msReportID == S_COSCHOLASTIC_SUBJECT_MARK_DETAILS || msReportID == S_PERIODIC_TEST_MARK_DETAILS || msReportID == S_STUDENTS_FEE_DETAILS_REPORT)
            {
                oTextBox.Visible = false;
                grdDisplayParameter.Rows[aiGridRowCount].FindControl("lblRptParameter").Visible = false;
                grdDisplayParameter.Rows[aiGridRowCount].Visible = false;
            }
            var oRegularExpressionValidator = grdDisplayParameter.Rows[aiGridRowCount].FindControl("Reg_Expr_ValidContent") as RegularExpressionValidator;
            oRegularExpressionValidator.Visible = true;
        }
        else
            oTextBox.CssClass = "MidTxtBox";
    }

    /// <summary>
    /// 	This method is used to add dropdownlist to grid.
    /// </summary>
    /// <param name="aoDSFilterParameters"> </param>
    /// <param name="aiGridRowCount"> </param>
    private void AddDropDownListToGrid(DataSet aoDSFilterParameters, int aiGridRowCount)
    {
        if (aoDSFilterParameters != null)
        {
            int iStandardId, iDivisionId;

            grdDisplayParameter.Visible = true;
            var oChkAll = grdDisplayParameter.Rows[aiGridRowCount].FindControl("ChkAll") as HtmlInputCheckBox;
            oChkAll.Visible = false;
            var oDropDownList = grdDisplayParameter.Rows[aiGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
            oDropDownList.Items.Clear();
            oDropDownList.EnableViewState = true;            
            FillDropdownList(oDropDownList, aoDSFilterParameters, aiGridRowCount);
            oDropDownList.ReportFieldId = grdDisplayParameter.DataKeys[aiGridRowCount]["Report_Field_Id"].ToString().ToInt();
            oDropDownList.Visible = true;

            if (Constants.UserRoles.Teacher == moUserRole)
            {
                int iStandardDivId = Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToInt();
                var oStandardDivisionMasterBL = new StandardDivisionMasterBL(iStandardDivId);
                iStandardId = oStandardDivisionMasterBL.StandardId;
                iDivisionId = oStandardDivisionMasterBL.DivisionId;
                string sHasEditAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.FinalResult).ToString();

                if ((msReportID == S_MUSTER_REPORT || msReportID == S_LECTUREWISE_STUDENT_ATTENDANCE))
                {
                    if (aiGridRowCount == 0)
                    {
                        oDropDownList.SelectedValue = iStandardId.ToString();
                    }
                    else if (aiGridRowCount == 1)
                    {
                        var ddlStandard = grdDisplayParameter.Rows[aiGridRowCount - 1].FindControl("DDLRptParameter") as ComboRpt;
                        oDropDownList_ComboChangeEvent(ddlStandard, null);

                        if (hidHasFullAccess.Value == Constants.S_ZERO)
                        {
                            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
                            DataTable oDtTeachers = oMasterDataCollectionBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);
                            
                            ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
                            List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();

                            AttendanceDetailsBL oAttendanceDetailsBL = new AttendanceDetailsBL();
                            List<CoordinateDetails> lstCoordinatorDetails = oAttendanceDetailsBL.GetCoordinatorDetails(miSchoolId, miAcademicYearId);
                            List<int> lstStandardIds = lstCoordinatorDetails.Where(ct => ct.UserId == miUserId).Select(ct => ct.StandardId).ToList();

                            DataRow[] drArr;
                            if (lstUsers.Any(ru => ru.ReportingPrameterId == Constants.ReportingParameters.PrePrimaryCoordinator.ToInt() && ru.UserId == miUserId))
                                drArr = oDtTeachers.Select("Is_PrePrimary='Y'");
                            else if (lstStandardIds.Count > 0)
                            {
                                drArr = oDtTeachers.Select("Standard_Id IN (" + string.Join(",", lstStandardIds) + ")");
                            }
                            else
                                drArr = oDtTeachers.Select("Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID].ToInt());


                            if (drArr.Length > 0)
                            {
                                DataTable dtTeachers = new DataTable();
                                dtTeachers.Columns.Add("Value_Member");
                                dtTeachers.Columns.Add("Display_Member");

                                DataRow newDefaultRow = dtTeachers.NewRow();
                                newDefaultRow["Value_Member"] = Constants.S_ZERO;
                                newDefaultRow["Display_Member"] = Constants.S_SELECT;
                                dtTeachers.Rows.Add(newDefaultRow);

                                if (aiGridRowCount == 1)
                                {
                                    foreach (DataRow dr in drArr)
                                    {
                                        DataRow drNew = dtTeachers.NewRow();
                                        DataRow[] drNewArr = dtTeachers.Select("Value_Member=" + dr["Division_Id"].ToString());
                                        if (drNewArr.Length == 0)
                                        {
                                            drNew["Value_Member"] = dr["Division_Id"];
                                            drNew["Display_Member"] = dr["Division_Name"];
                                            dtTeachers.Rows.Add(drNew);
                                        }
                                    }
                                }

                                oDropDownList.DataSource = dtTeachers;
                                oDropDownList.DataBind();
                            }
                            else
                            {
                                oDropDownList.DataSource = null;
                                oDropDownList.DataBind();
                            }

                            if (oDropDownList.Items.Count == 2)
                            {
                                oDropDownList.SelectedValue = iDivisionId.ToString();
                                oDropDownList_ComboChangeEvent(oDropDownList, null);
                            }
                        }
                        else
                        {
                            oDropDownList.SelectedValue = iDivisionId.ToString();
                            oDropDownList_ComboChangeEvent(oDropDownList, null);
                        }
                    }
                }
                

                if (msReportID == S_XSEED_REPORT && miSchoolId == Constants.SchoolId.PPS.ToInt())
                {
                    char cHasEditAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.XseedResults);    
                    
                    if (moUserRole != Constants.UserRoles.Admin && cHasEditAccess == 'N')
                    {
                        if (aiGridRowCount == 0)
                        {
                            oDropDownList.SelectedValue = iStandardId.ToString();
                            oDropDownList.Enabled = false;

                            oDropDownList_ComboChangeEvent(oDropDownList, null);
                        }
                        else if (aiGridRowCount == 1)
                        {
                            var ddlStandard = grdDisplayParameter.Rows[aiGridRowCount - 1].FindControl("DDLRptParameter") as ComboRpt;
                            oDropDownList_ComboChangeEvent(ddlStandard, null);
                            
                            oDropDownList.SelectedValue = iStandardDivId.ToString();
                            oDropDownList.Enabled = false;

                            oDropDownList_ComboChangeEvent(oDropDownList, null);
                        }
                        else if (aiGridRowCount == 2)
                        {
                            var ddldivision = grdDisplayParameter.Rows[aiGridRowCount - 1].FindControl("DDLRptParameter") as ComboRpt;
                            oDropDownList_ComboChangeEvent(ddldivision, null);
                        }
                        else if (aiGridRowCount == 3)
                        {
                            var ddldivision = grdDisplayParameter.Rows[aiGridRowCount - 2].FindControl("DDLRptParameter") as ComboRpt;
                            oDropDownList_ComboChangeEvent(ddldivision, null);
                        }
                    }
                }    

                if (msReportID == S_STUD_FINAL_RESULT && hidHasFullAccess.Value == Constants.S_ZERO && sHasEditAccess == Constants.S_NO)
                {                    
                    MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
                    List<ClassTeacherDetails> lstTeachers = oMasterDataCollectionBL.GetClassTeachersForExamResult(miSchoolId, miAcademicYearId);
                    List<ClassTeacherDetails> lstClassTeachers = lstTeachers.Where(Teacher => Teacher.TeacherId == Session[Constants.S_SESSION_TEACHER_ID].ToInt()).ToList();

                    if (lstClassTeachers.Count == 1)
                    {
                        if (aiGridRowCount == 0)
                        {
                            oDropDownList.SelectedValue = iStandardId.ToString();
                            oDropDownList.Enabled = false;

                            oDropDownList_ComboChangeEvent(oDropDownList, null);
                        }
                        else if (aiGridRowCount == 1)
                        {
                            var ddlStandard = grdDisplayParameter.Rows[aiGridRowCount - 1].FindControl("DDLRptParameter") as ComboRpt;
                            oDropDownList_ComboChangeEvent(ddlStandard, null);


                            oDropDownList.SelectedValue = iStandardDivId.ToString();
                            oDropDownList.Enabled = false;

                            oDropDownList_ComboChangeEvent(oDropDownList, null);
                        }
                        else if (aiGridRowCount == 2)
                        {
                            var ddldivision = grdDisplayParameter.Rows[aiGridRowCount - 1].FindControl("DDLRptParameter") as ComboRpt;
                            oDropDownList_ComboChangeEvent(ddldivision, null);
                        }
                    }
                }
                
            }
            else
            {
                iStandardId = Session[Constants.S_SESSION_STUDENT_STANDERED_ID].ToInt();
                iDivisionId = Session[Constants.S_SESSION_STUDENT_DIVISION_ID].ToInt();
            }

            if (msReportID == S_MUSTER_REPORT || msReportID == S_LECTUREWISE_STUDENT_ATTENDANCE)
            {
                if (moUserRole != Constants.UserRoles.Admin && hidHasFullAccess.Value == Constants.S_ZERO && (aiGridRowCount <=1))
                {
                    if (oDropDownList.Items.Count == 2)
                    {
                        if (Session[Constants.S_SESSION_TEACHER_STDDIV_ID] != null && Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToString() != Constants.S_ZERO)
                            oDropDownList.Enabled = false;
                    }
                }
            }

            if (moUserRole == Constants.UserRoles.Student)
                FillStudentAndDivisionCombo(oDropDownList, aiGridRowCount, iDivisionId, iStandardId);
            else
            {
                //If current control is parent control then set its event and AutoPostBack property true. 
                if (grdDisplayParameter.DataKeys[aiGridRowCount]["Is_Parent"].ToString() == "Y")
                {
                    oDropDownList.ComboChangeEvent += oDropDownList_ComboChangeEvent;
                    oDropDownList.AutoPostBack = true;
                }
                if (msReportID == S_TEACHER_ASSIGNMENT_RPT_ID && oDropDownList.ReportFieldId == 155)
                {
                    DateTime dtCurrentDate = DateTime.Today;
                    string sWeekday = dtCurrentDate.Date.DayOfWeek.ToString();
                    if (oDropDownList.Items.FindByText(sWeekday) != null)
                        oDropDownList.Items.FindByText(sWeekday).Selected = true;
                }
            }
            if (msReportID == S_IT_RECONCILIATION_RPT_ID)
            {
                if (moUserRole == Constants.UserRoles.Student && (oDropDownList.ReportFieldId == 229 || oDropDownList.ReportFieldId == 503))
                {
                    if (grdDisplayParameter.DataKeys[aiGridRowCount]["Is_Parent"].ToString() == "Y")
                    {
                        oDropDownList.ComboChangeEvent += oDropDownList_ComboChangeEvent;
                        oDropDownList.AutoPostBack = true;
                    }
                }

                if (oDropDownList.ReportFieldId == 229)
                {
                    if (btnSearch.Visible)
                    {
                        var oMasterPage = this.Master as MasterPage;
                        var oForm1 = oMasterPage.FindControl("form1") as HtmlForm;
                        oForm1.DefaultButton = btnSearch.UniqueID;
                    }

                    if (moUserRole == Constants.UserRoles.Student)
                    {
                        int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
                        DataTable dtStudents = StudentBL.GetFinancial(miSchoolId, iStudentId);

                        if (dtStudents.Rows.Count > 0)
                        {
                            DataRow dr = dtStudents.NewRow();
                            dr["Display_Member"] = Constants.S_ALL;
                            dr["Value_Member"] = Constants.S_ZERO;
                            dtStudents.Rows.InsertAt(dr, 0);
                        }

                        oDropDownList.DataSource = dtStudents.DefaultView;
                        oDropDownList.DataTextField = "Display_Member";
                        oDropDownList.DataValueField = "Value_Member";
                        oDropDownList.DataBind();

                        oDropDownList.Enabled = true;
                    }

                    int iFinancialYrId = ReportsBL.SetDefaultFinancialYear(DateTime.Now.ToString("yyyy-MMM-dd",new System.Globalization.CultureInfo("en")));
                    string sFinancialYrId = iFinancialYrId.ToString();

                    ListItem oListItem = oDropDownList.Items.FindByValue(sFinancialYrId);
                    oDropDownList.SelectedValue = oListItem != null ? iFinancialYrId.ToString() : Constants.I_ZERO.ToString();
                }
                else if (oDropDownList.ReportFieldId == 503) // Academic year combo
                {
                    if (moUserRole == Constants.UserRoles.Student)
                    {
                        int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
                        DataTable dtStudents = StudentBL.GetAllAcademicYearsOfStudent(miSchoolId, iStudentId);

                        if (dtStudents.Rows.Count > 0)
                        {
                            DataRow dr = dtStudents.NewRow();
                            dr["AcademicYear"] = Constants.S_ALL;
                            dr["Academic_Year_Id"] = Constants.S_ZERO;
                            dtStudents.Rows.InsertAt(dr, 0);
                        }

                        oDropDownList.DataSource = dtStudents.DefaultView;
                        oDropDownList.DataTextField = "AcademicYear";
                        oDropDownList.DataValueField = "Academic_Year_Id";
                        oDropDownList.DataBind();

                        oDropDownList.Enabled = true;
                    }
                }
            }

            if (msReportID == S_EXAM_PUBLISH_STATUS)
            {
                // Check whether annual result is published or not.
                if (ReportsBL.IsAnnualResultPublished(miSchoolId, miAcademicYearId))
                {
                    oDropDownList.Items.Add(new ListItem(string.Format("- {0} -", S_ANNUAL_RESULT), S_ANNUAL_RESULT_TYPE));
                    oDropDownList.SelectedValue = S_ANNUAL_RESULT_TYPE;
                }
                else if (oDropDownList.Items.Count > 0)
                {
                    var oDTExam = oDropDownList.DataSource as DataTable;
                    oDTExam.DefaultView.Sort = "Value_Member DESC";
                    DataView oDataView = oDTExam.DefaultView;
                    oDropDownList.SelectedValue = oDataView[0][0].ToString();
                }
            }
            else if (mlstUserAccessPayrollReports.Contains(msReportID) && moUserRole != Constants.UserRoles.Admin)
            {
                const int I_STAFF_GROUP_REPORT_FIELD = 254;
                const int I_STAFF_LEAVE_REPORT_FIELD = 276;
                const int I_STAFF_GROUP_FOR_INVESTMENT_DECLARATION = 443;
                const int I_USER_FOR_INVESTMENT_DECLARATION = 444;

                const int I_IT_STAFF_GROUP_FIELD = 450;
                const int I_IT_USER_FIELD = 451;

                if (hidHasFullAccess.Value == "0")
                {
                    switch (oDropDownList.ReportFieldId)
                    {
                        case I_STAFF_LEAVE_REPORT_FIELD:
                        case I_STAFF_GROUP_REPORT_FIELD:
                            {
                                GetAllStaffGroups(oDropDownList);
                                oDropDownList_ComboChangeEvent(oDropDownList, null);
                            }
                            break;

                        case 277:
                        case 255:
                            GetAllUsers(oDropDownList);
                            break;

                        case 274:
                            oDropDownList.Items[0].Text = Constants.S_ALL;
                            break;

                        case I_STAFF_GROUP_FOR_INVESTMENT_DECLARATION:
                        case I_IT_STAFF_GROUP_FIELD:
                            GetAllStaffGroups(oDropDownList);
                            break;

                        case I_USER_FOR_INVESTMENT_DECLARATION:
                        case I_IT_USER_FIELD:
                            GetAllUsers(oDropDownList);
                            break;

                    }
                }                
            }
        }
        else
        {
            if (msReportID == S_SUBJECT_TOPPERS || msReportID==S_TESTWISE_SUBJECT_TOPPERS)
            {
                switch (aiGridRowCount)
                {
                    case I_TYPE_ROW:
                        FillTypeCombobox(aiGridRowCount);
                        break;
                    case I_EXAM_ROW:
                        {
                            var oDropDownList = grdDisplayParameter.Rows[aiGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                            oDropDownList.Items.Clear();
                            oDropDownList.EnableViewState = true;
                            FillDropdownList(oDropDownList, aoDSFilterParameters, aiGridRowCount);
                            oDropDownList.ReportFieldId = grdDisplayParameter.DataKeys[aiGridRowCount]["Report_Field_Id"].ToString().ToInt();
                            oDropDownList.Visible = true;
                            oDropDownList.Items.Add(new ListItem(S_ALL, "0"));
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// This is a common function used to get all Staff groups.
    /// </summary>
    /// <param name="oDropDownList"></param>
    private void GetAllStaffGroups(ComboRpt oDropDownList)
    {
        string sStartDate = null;
        string sEndDate = null;
        int iFinancialYearId = 0;
        if (msReportID == S_SALARY_SLIP || msReportID == S_TRANSFERED_STAFF_SALARY_SLIP || msReportID == S_BANK_LETTER || msReportID == S_STAFF_ATTENDANCE || msReportID == S_STAFF_LEAVE_DETAILS_EXPORT)
        {
            sStartDate = (grdDisplayParameter.Rows[0].FindControl("CalenderRptParameter") as PopCalendar).DateValue.ToShortDateString();
            sEndDate = (grdDisplayParameter.Rows[1].FindControl("CalenderRptParameter") as PopCalendar).DateValue.ToShortDateString();
        }

        if(msReportID == S_FORM_NO_16 || msReportID == S_INVESTMENT_DECLARATIONS)
            iFinancialYearId = miFinancialYearId;
        else if (msReportID == S_STAFF_LEAVES)
        {
            sStartDate = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
            sEndDate = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
        }

        DataSet oDataSet = SalaryDetailsBL.GetUsersStaffGroupDetais(miSchoolId, miAcademicYearId, miUserId, sStartDate, sEndDate, iFinancialYearId);
        if (oDataSet != null && oDataSet.Tables.Count > 0)
        {
            DataTable oDataTable = oDataSet.Tables[0];
            if (oDataTable.IsNonEmpty())
            {   
                oDropDownList.DataSource = oDataTable;
                oDropDownList.DataBind();
                oDropDownList.Items.Insert(0, new ListItem { Text = "-- All --", Value = Constants.S_ZERO });
                ViewState["UsersStaffGroupDetais"] = oDataSet.Tables[1];
                oDropDownList.Enabled = true;
            }
            else
                oDropDownList.Enabled = false;
        }
    }

    /// <summary>
    /// This function is used to get the users for selected staff group.
    /// </summary>
    /// <param name="oDropDownList"></param>
    private void GetAllUsers(ComboRpt oDropDownList)
    {
        var oDataTable = ViewState["UsersStaffGroupDetais"] as DataTable;

        if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
        {
            oDropDownList.DataSource = oDataTable;
            oDropDownList.DataTextField = "Display_Member";
            oDropDownList.DataValueField = "Value_Member";
            oDropDownList.DataBind();
            oDropDownList.Items.FindByValue(miUserId.ToString()).Selected = true;
            oDropDownList.Enabled = false;
        }
        else
            oDropDownList.Enabled = false;
    }


    /// <summary>
    /// 	This method is used to add checkboxlist control to grid.
    /// </summary>
    /// <param name="aoDSFilterParameters"> </param>
    /// <param name="aiGridRowCount"> </param>
    private void AddCheckBoxListToGrid(DataSet aoDSFilterParameters, int aiGridRowCount)
    {
        if (aoDSFilterParameters != null)
        {
            grdDisplayParameter.Columns[0].Visible = true;
            var oCheckBoxList = grdDisplayParameter.Rows[aiGridRowCount].FindControl("ChkRptParameter") as CheckBoxList;
            var oChkAll = grdDisplayParameter.Rows[aiGridRowCount].FindControl("ChkAll") as HtmlInputCheckBox;
            oChkAll.Visible = true;
            oCheckBoxList.Items.Clear();
            oCheckBoxList.EnableViewState = true;
            oCheckBoxList.DataSource = aoDSFilterParameters.Tables[Constants.I_ZERO];
            oCheckBoxList.DataTextField = S_DISPLAY_MEMBER;
            oCheckBoxList.DataValueField = S_VALUE_MEMBER;
            oCheckBoxList.RepeatDirection = RepeatDirection.Horizontal;
            oCheckBoxList.DataBind();
            oCheckBoxList.Visible = true;
            oChkAll.Attributes.Add("onclick", "CheckAllOrUncheckChkBox(" + aiGridRowCount + " );");
            if (moUserRole == Constants.UserRoles.Teacher && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.C_YES.ToString())
            {
                string sDisplayName = grdDisplayParameter.DataKeys[aiGridRowCount]["Display_name"].ToString().ToLower();
                int iStandardDivId = Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToInt();
                var oStandardDivisionMasterBL = new StandardDivisionMasterBL(iStandardDivId);
                int iStandardId = oStandardDivisionMasterBL.StandardId;
                int iDivisionId = oStandardDivisionMasterBL.DivisionId;

                if (sDisplayName == "standard")
                    oCheckBoxList.Items.FindByValue(Convert.ToString(iStandardId)).Selected = true;
                if (sDisplayName == "division")
                    oCheckBoxList.Items.FindByValue(Convert.ToString(iDivisionId)).Selected = true;
                oCheckBoxList.Enabled = false;
                oChkAll.Disabled = true;
            }
            if (msReportID == S_EARNINGS_DEDUCTIONS)
                oCheckBoxList.RepeatColumns = 5;
        }
        else
        {
            var oCheckBoxList = grdDisplayParameter.Rows[aiGridRowCount].FindControl("ChkRptParameter") as CheckBoxList;
            oCheckBoxList.Items.Clear();
            oCheckBoxList.EnableViewState = true;
            if (msReportID != S_PENDING_FEE_DETAILS && msReportID != S_PENDING_FEE_STUDENTLIST && msReportID != S_ANNUAL_CONSOLDATED_REPORT && msReportID != S_ANNUAL_CONSOLDATED_REPORT_SPS9 && msReportID != S_ANNUAL_CONSOLDATED_REPORT_SPS11 && msReportID != S_ANNUAL_CONSOLDATED_REPORT_SNS && msReportID != S_STAFF_LEAVES  && msReportID!= S_LEAVE_BALANCE && msReportID != S_STUDENT_IDENTITY_CARDS && msReportID != S_STUDENT_AUTHORITY_CARDS && msReportID != S_STUDENT_FEE_REPORT && msReportID != S_PROVIDENT_FUND_DETAILS && msReportID != S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE && msReportID != S_SERVICE_TYPE_DETAILS && msReportID != S_SALARY_SHEET && msReportID != S_DAILY_FEE_COLLECTION && msReportID != S_STUDENTS_HOUSE && msReportID != S_RESULTSHEET && msReportID != S_ANNUAL_CONSOLDATED_SUB_TYPE_REPORT_SNS &&
                msReportID != S_EXPORT_STUDENT_LIST && msReportID != S_AGE_CALCULATION && msReportID != S_INVESTMENT_DECLARATIONS && msReportID != S_STAFF_ATTENDANCE && msReportID != S_DATEWISE_Fee_COLLECTION && msReportID != S_STUDENT_GENERAL_REGISTER_REPORT && msReportID != S_EMPLOYEE_INFORMATION_FOR_REPORT && msReportID != S_LC_REPORT_ID && msReportID != S_CATEGORYWISE_ITEM_DETAILS && msReportID != S_STAFF_SCREEN_ACCESS_DETAILS && msReportID != S_STUDENT_ADDRESS_REPORT && msReportID != S_STAFF_KID_FEE && msReportID != S_NOMINAL_ROLL && msReportID != S_TRANSFER_CERTIFICATE && msReportID != S_PARENT_IDENTITY_CARDS && msReportID != S_ANNUAL_CONSOLDATED_UNITTEST_REPORT_SVNP && msReportID != S_STOPWISE_STUDENT_PENDING_FEE && msReportID != S_RTE_STUDENT_LIST && msReportID != S_STUDENT_REGISTRATION_DEATILS && msReportID != S_STUDENT_REFUND_FEE_DETAILS && msReportID != S_HOUSEWISE_STUDENT_DETAILS && msReportID != S_BONAFIDE_CERTIFICATE_REPORT_ID && msReportID != S_EMPLOYEE_INFORMATION_DETAILS && msReportID != S_LEAVING_CERTIFICATE_10TH_NPS_ID && msReportID != S_BANK_CHALLAN_REPORT && msReportID != S_NEXT_YEAR_PAID_FEE && msReportID != S_CA_RECONSOLIDATION_DETAILS)
                oCheckBoxList.Items.Add("Include");
            else
                oCheckBoxList.Items.Add("");
            oCheckBoxList.Items[0].Selected = msReportID != S_ANNUAL_CONSOLDATED_REPORT && msReportID != S_ANNUAL_CONSOLDATED_REPORT_SPS9 && msReportID != S_ANNUAL_CONSOLDATED_REPORT_SPS11 && msReportID != S_ANNUAL_CONSOLDATED_REPORT_SNS && msReportID != S_ANNUAL_CONSOLDATED_SUB_TYPE_REPORT_SNS && msReportID != S_SERVICE_TYPE_DETAILS && msReportID != S_STUDENT_IDENTITY_CARDS && msReportID != S_AGE_CALCULATION && msReportID != S_STUDENT_FEE_REPORT && msReportID != S_STUDENT_AUTHORITY_CARDS && msReportID != S_EXPORT_STUDENT_LIST && msReportID != S_CATEGORYWISE_ITEM_DETAILS && msReportID != S_DATEWISE_Fee_COLLECTION && msReportID != S_STUDENT_GENERAL_REGISTER_REPORT && msReportID != S_LC_REPORT_ID && msReportID != S_STAFF_SCREEN_ACCESS_DETAILS && msReportID != S_STUDENT_ADDRESS_REPORT && msReportID != S_NOMINAL_ROLL && msReportID != S_TRANSFER_CERTIFICATE && msReportID != S_PARENT_IDENTITY_CARDS && msReportID != S_ANNUAL_CONSOLDATED_UNITTEST_REPORT_SVNP && msReportID != S_STOPWISE_STUDENT_PENDING_FEE && msReportID != S_STUDENT_REGISTRATION_DEATILS && msReportID != S_STUDENT_REFUND_FEE_DETAILS && msReportID != S_HOUSEWISE_STUDENT_DETAILS && msReportID != S_BONAFIDE_CERTIFICATE_REPORT_ID && msReportID != S_LEAVING_CERTIFICATE_10TH_NPS_ID && msReportID != S_CA_RECONSOLIDATION_DETAILS;
            oCheckBoxList.Visible = true;

            var oChkAll = grdDisplayParameter.Rows[aiGridRowCount].FindControl("ChkAll") as HtmlInputCheckBox;
            if (oChkAll != null)
            {
                if (msReportID == S_EXPORT_STUDENT_LIST || msReportID == S_DATEWISE_Fee_COLLECTION || msReportID == S_STUDENT_GENERAL_REGISTER_REPORT || msReportID == S_STUDENT_REGISTRATION_DEATILS || msReportID == S_STUDENT_REFUND_FEE_DETAILS || msReportID == S_NEXT_YEAR_PAID_FEE || msReportID == S_CA_RECONSOLIDATION_DETAILS)
                    oChkAll.Visible = false;
            }
        }
    }

    #endregion

    #region Fill Combobox

    /// <summary>
    /// 	This method is used to fill student and division combobox.
    /// </summary>
    /// <param name="oDropDownList"> </param>
    /// <param name="aiGridRowCount"> </param>
    /// <param name="iDivisionId"> </param>
    /// <param name="iStandardId"> </param>
    private void FillStudentAndDivisionCombo(ComboRpt oDropDownList, int aiGridRowCount, int iDivisionId, int iStandardId)
    {
        string sDisplayName = grdDisplayParameter.DataKeys[aiGridRowCount]["Display_name"].ToString().ToLower();

        switch (sDisplayName)
        {
            case "standard":
                oDropDownList.Items.FindByValue(iStandardId.ToString()).Selected = true;
                oDropDownList.Enabled = false;
                break;
            case "division":
                {
                    var oDivisionCollectionBL = new DivisionCollectionBL(miSchoolId, miAcademicYearId);
                    DataTable dtDivisionInfo = oDivisionCollectionBL.GetAllDivisionsForStandard(iStandardId);
                    oDropDownList.DataSource = dtDivisionInfo.DefaultView;
                    oDropDownList.DataTextField = "division_name";
                    oDropDownList.DataValueField = "division_id";
                    oDropDownList.DataBind();
                    oDropDownList.Items.FindByValue(iDivisionId.ToString()).Selected = true;
                }
                break;
            case "student":
                {
                    string sStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToString();

                    bool bFilterLeft = true;
                    if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                        bFilterLeft = false;

                    DataTable dtStudents = StudentBL.GetAllStudentsWithLeftFilter(miSchoolId, iStandardId, iDivisionId, miAcademicYearId, bFilterLeft);
                    oDropDownList.DataSource = dtStudents.DefaultView;
                    oDropDownList.DataTextField = "Name";
                    oDropDownList.DataValueField = "YearWise_Student_Id";
                    oDropDownList.DataBind();

                    if (oDropDownList.Items.FindByValue(sStudentId) != null)
                        oDropDownList.Items.FindByValue(sStudentId).Selected = true;

                    oDropDownList.Enabled = false;
                }
                break;
        }
    }

    /// <summary>
    /// 	This method is used to fill dropdown list.
    /// </summary>
    /// <param name="oDropDownList"> </param>
    /// <param name="aoDSFilterParameters"> </param>
    /// <param name="aiGridRowCount"> </param>
    private void FillDropdownList(ComboRpt oDropDownList, DataSet aoDSFilterParameters, int aiGridRowCount)
    {
        List<string> lstPayrollReports = new List<string> { S_SALARY_SLIP, S_TRANSFERED_STAFF_SALARY_SLIP, S_INVESTMENT_DECLARATIONS, S_STAFF_LEAVES, S_FORM_NO_16, S_LEAVE_BALANCE, S_SALARY_LEDGER, S_EARNINGS_DEDUCTIONS, S_SALARY_SHEET, S_INSURANCE_DETAILS, S_NET_SALARY, S_BANK_LETTER , S_STAFF_ATTENDANCE,S_STAFF_LEAVE_DETAILS_EXPORT};
        List<string> lstOtherPayrollReports = new List<string> { S_PROFESSIONAL_TAX_CHALLAN, S_PROFESSIONAL_TAX_DETAILS, S_PROVIDENT_FUND_DETAILS, S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE, S_STAFF_LEAVE_DETAILS_EXPORT};

        if (aoDSFilterParameters == null || aoDSFilterParameters.Tables.Count == Constants.I_ZERO)
            return;
        string sIsParent = grdDisplayParameter.DataKeys[aiGridRowCount]["Is_Parent"].ToString();

        if (grdDisplayParameter.DataKeys[aiGridRowCount]["Is_Dependent"].ToString() == "N")
            oDropDownList.DataSource = aoDSFilterParameters.Tables[Constants.I_ZERO];
        //If current control is dependent then set it to enabled false.
        else
        {
            if (msReportID == S_SERVEY_ANALYSIS_COUNT_REPORT)
            {
            }
            else if ((!lstPayrollReports.Contains(msReportID) && !lstOtherPayrollReports.Contains(msReportID)) || (lstPayrollReports.Contains(msReportID) && sIsParent != Constants.S_YES))            
            {
                oDropDownList.Enabled = false;
                aoDSFilterParameters.Tables[Constants.I_ZERO].Rows.Clear();
            }
            oDropDownList.DataSource = aoDSFilterParameters.Tables[Constants.I_ZERO];
        }

        if (msReportID == S_MUSTER_REPORT && moUserRole == Constants.UserRoles.Teacher && aiGridRowCount == 0 && hidHasFullAccess.Value == Constants.S_ZERO)
        {
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            DataTable oDtTeachers = oMasterDataCollectionBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);

            ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();

            AttendanceDetailsBL oAttendanceDetailsBL = new AttendanceDetailsBL();
            List<CoordinateDetails> lstCoordinatorDetails = oAttendanceDetailsBL.GetCoordinatorDetails(miSchoolId, miAcademicYearId);
            List<int> lstStandardIds = lstCoordinatorDetails.Where(ct => ct.UserId == miUserId).Select(ct => ct.StandardId).ToList();

            DataRow[] drArr;
            if (lstUsers.Any(ru => ru.ReportingPrameterId == Constants.ReportingParameters.PrePrimaryCoordinator.ToInt() && ru.UserId == miUserId))
                drArr = oDtTeachers.Select("Is_PrePrimary='Y'");
            else if (lstStandardIds.Count > 0)
            {
                drArr = oDtTeachers.Select("Standard_Id IN (" + string.Join(",", lstStandardIds) + ")");
            }
            else
                drArr = oDtTeachers.Select("Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID].ToInt());

            if (drArr != null && drArr.Length > 0)
            {
                DataTable dtTeachers = new DataTable();
                dtTeachers.Columns.Add("Value_Member");
                dtTeachers.Columns.Add("Display_Member");

                DataRow newDefaultRow = dtTeachers.NewRow();
                newDefaultRow["Value_Member"] = Constants.S_ZERO;
                newDefaultRow["Display_Member"] = Constants.S_SELECT;
                dtTeachers.Rows.Add(newDefaultRow);

                foreach (DataRow dr in drArr)
                {
                    DataRow drNew = dtTeachers.NewRow();
                    DataRow[] drNewArr = dtTeachers.Select("Value_Member=" + dr["Standard_id"].ToString());
                    if (drNewArr.Length == 0)
                    {
                        drNew["Value_Member"] = dr["Standard_id"];
                        drNew["Display_Member"] = dr["Standard_Name"];
                        dtTeachers.Rows.Add(drNew);
                    }
                }
                oDropDownList.DataSource = dtTeachers;
            }
        }

        //This method is used to add top element(i.e.'--Select--') to dropdownlist.
        AddTopElementToDataView(aoDSFilterParameters.Tables[Constants.I_ZERO].DefaultView, S_DISPLAY_MEMBER, S_VALUE_MEMBER);
        oDropDownList.DataTextField = S_DISPLAY_MEMBER;
        oDropDownList.DataValueField = S_VALUE_MEMBER;
        oDropDownList.DataBind();
    }

    /// <summary>
    /// 	This method is used to fill filter parameters combobox.
    /// </summary>
    /// <param name="oDropDownList"> </param>
    /// <param name="oHashFilterParameters"> </param>
    /// <param name="iGridRowCount"> </param>
    private void FillFilterParametersCombo(ComboRpt oDropDownList, Hashtable oHashFilterParameters, int iGridRowCount)
    {
        DataSet oDSFilterParameter = RetriveReportParameters(iGridRowCount, oHashFilterParameters);
        if (oDSFilterParameter == null || oDSFilterParameter.Tables.Count <= 0)
            return;

        if (oDropDownList.SelectedValue != string.Empty && oDropDownList.SelectedValue != Constants.S_ZERO)
        {
            oDropDownList.DataSource = null;
            oDropDownList.DataBind();
        }

        oDropDownList.Enabled = oDSFilterParameter.Tables[0].Rows.Count > Constants.I_ZERO;

        AddTopElementToDataView(oDSFilterParameter.Tables[0].DefaultView, S_DISPLAY_MEMBER, S_VALUE_MEMBER);
        oDropDownList.DataSource = oDSFilterParameter;
        oDropDownList.DataTextField = S_DISPLAY_MEMBER;
        oDropDownList.DataValueField = S_VALUE_MEMBER;
        oDropDownList.DataBind();

        if (!oDropDownList.IsRequired)
            oDropDownList.Items[0].Text = string.Format("-- {0} --", Constants.S_ALL);
    }

    #endregion

    #region Filter String

    /// <summary>
    /// 	This method is used to fill parameters grid.
    /// </summary>
    private void DisplayReportFilters()
    {
        tblGridView.Visible = true;
        tblHeader.Visible = true;
        lblSelect.Visible = true;

        msReportPath = QueryString["rpt"];
        msReportID = QueryString["ReportID"];
        string sReportDisplayName = QueryString["Report_Display_Name"];
        if (QueryString["IsControlPnl"] != null)
            lblHeader.Text = S_BONAFIDE_REPORT;
        else
            lblHeader.Text = "Parameters For " + sReportDisplayName + " Report";
        btnDisplayReport.Click += btnDisplayReport_Click;
        if (!IsPostBack)
        {
            if (msReportID != null)
            {
                lblDesc.Visible = false;

                if ((msReportID == S_RESULTSHEET || msReportID == S_PRELIM_RESULT_SHEET) && miSchoolId == Constants.SchoolId.SVP.ToInt())
                    trFontNote.Visible = true;
                else
                    trFontNote.Visible = false;

                FillGridWithReportParameters();
            }
        }
        else
            //This method is used to se events to parent combo box.
            AddParentFilterEventHandler();
		//If current reqest is not a post back and display parameter are avaialble then set properties to control.
        if (grdDisplayParameter.Rows.Count > 0)
            SetPropertiesToControls();
        lblErrorMesg.Text = string.Empty;
    }

    /// <summary>
    /// 	This method is used to read filter parameters and to get recordcount after executing selection string.
    /// </summary>
    /// <param name="aArrFilterParameter"> </param>
    /// <param name="asViewName"> </param>
    /// <returns> int </returns>
    private int ReadFilterParametersForView(String[] aArrFilterParameter, String asViewName)
    {
        string sParameterValue, sParameterField;
        string sparameter = "Parameters";

        int iReportDataCount = 0;
        foreach (string filter in aArrFilterParameter)
        {
            if (filter.Equals(string.Empty))
                continue;
            sParameterValue = filter.Substring(filter.LastIndexOf("=") + 1);
            sParameterField = filter.Substring(filter.LastIndexOf(".") + 1, filter.LastIndexOf("=") - filter.LastIndexOf(".") - 1).Trim();

            //Special case is handled if parameter field name is 'Payment_date'.
            if (sParameterField == "Payment_date")
                sParameterValue = string.Format("'{0}' ", sParameterValue.Substring(1, (sParameterValue.Length) - 5));
            //If selection string contains more than one same parameter field,then it creates with 'OR'.
            if (sparameter.Contains(sParameterField))
            {
                sparameter = sparameter.Substring(0, (sparameter.Length) - 5) + " OR ";
                sparameter = sparameter + sParameterField + "=" + sParameterValue + " OR ";
            }
            else
            {
                sparameter = sparameter.Substring(0, (sparameter.Length) - 5) + " AND ";
                sparameter = sparameter + sParameterField + "=" + sParameterValue + " AND ";
            }
        }
        sparameter = sparameter.Substring(5, (sparameter.Length) - 5);
        sparameter = sparameter.Substring(4, (sparameter.Length) - 8);
        iReportDataCount = ReportsBL.IsReportEmpty(asViewName, sparameter);
        return iReportDataCount;
    }

    /// <summary>
    /// 	This method is used to read filter parameters for stored procedure and returns recordcount after executing stored procedure.
    /// </summary>
    /// <param name="aArrFilterParameter"> </param>
    /// <param name="asUSPName"> </param>
    /// <returns> int </returns>
    private int ReadFilterParametersForUSP(String[] aArrFilterParameter, string asUSPName)
    {
        int iReportDataCount;
        string sParameterValue, sParameterField;
        var oHashFilterParameter = new Hashtable();        
            foreach (string filter in aArrFilterParameter)
            {
                if (filter.Equals(string.Empty))
                    continue;
                sParameterValue = filter.Substring(filter.IndexOf("=") + 1);

                if (!filter.Contains(hidSchemaName.Value) || hidSchemaName.Value.Trim() == string.Empty)
                    sParameterField = filter.Substring(filter.IndexOf(".") + 1, filter.LastIndexOf("=") - filter.IndexOf(".") - 1).Trim();
                else
                    sParameterField = filter.Substring(filter.LastIndexOf(".") + 1, filter.IndexOf("=") - filter.LastIndexOf(".") - 1).Trim();

                // Execute Only when to check Entered Receipt Numbers are in correct formate or not only for SNS School.
                if (miSchoolId == Constants.SchoolId.SNS.ToInt() && msReportID == S_FEE_RECEIPT_DETAILS && sParameterField == "ReceiptNo" && sParameterValue != string.Empty)
                {
                    string[] sArray = sParameterValue.Split(',');
                    if (sArray.Length > 0)
                    {
                        foreach (var svalue in sArray)
                        {
                            try
                            {
                                if (!Convert.ToBoolean(int.Parse(svalue)))
                                {
                                    lblErrorMesg.Visible = true;
                                }                                
                            }
                            catch (Exception)
                            {
                                throw new System.ApplicationException("Receipt Number should be in correct formate. (,)Comma Seprated.");
                            }
                        }
                    }
                }                
                    //Parameter names and parameter values are added into hashtable.
                    oHashFilterParameter.Add(sParameterField, sParameterValue.Trim());
                    if (sParameterValue.Trim() == "null")
                       oHashFilterParameter.Remove(sParameterField);                
            }        
        if (msReportID == S_CLASS_TT_REPORT_ID || msReportID == S_TEACHER_TT_REPORT_ID || msReportID == S_SCHOOL_TT_REPORT_ID || msReportID == S_FREE_TEACHER_LIST_REPORT_ID || msReportID == S_TEACHER_REPLACEMENT_LIST_REPORT_ID || msReportID == S_DAILY_TEACHER_LECTCNT_REPORT_ID)
        {
            if (Settings.IsMPTApplicable)
            {
                string sMPTWeekday = Settings.MPTWeekday;
                string sMPTLectNo = Settings.MPTLectNo.ToString();
                string sMPTName = Settings.MPTName;

                oHashFilterParameter.Add(S_MPT_NAME, sMPTName);
                oHashFilterParameter.Add(S_MPT_WEEKDAY, sMPTWeekday);
                oHashFilterParameter.Add(S_MPT_LECT_NO, sMPTLectNo);
            }
            if (Settings.IsAssemblyApplicable)
            {
                string sAssemblyWeekday = Settings.AssemblyWeekday;
                string sAssemblyLectNo = Settings.AssemblyLectNo.ToString();
                string sAssemblyName = Settings.AssemblyName;

                oHashFilterParameter.Add(S_ASSEMBLY_LECT_NO, sAssemblyLectNo);
                oHashFilterParameter.Add(S_ASSEMBLY_NAME, sAssemblyName);
                oHashFilterParameter.Add(S_ASSEMBLY_WEEKDAY, sAssemblyWeekday);
            }
            if (msReportID == S_CLASS_TT_REPORT_ID || msReportID == S_TEACHER_TT_REPORT_ID || msReportID == S_SCHOOL_TT_REPORT_ID)
            {
                if (Settings.IsStaybackApplicable)
                {
                    string sStayBackName = Settings.StaybackName;
                    oHashFilterParameter.Add(S_SATYBACK_NAME, sStayBackName);
                }
            }
        }

        if (msReportID == S_XSEED_REPORT && Settings.IsAaryanSchool || miSchoolId == Constants.SchoolId.PPSH.ToInt())
            asUSPName = asUSPName.Replace("Xseed.","");

        DataTable oDTReportData = ReportsBL.IsReportEmpty(asUSPName, oHashFilterParameter);
        moDTStudentFinalProgressReports = oDTReportData;
        iReportDataCount = oDTReportData.Rows.Count;
        return iReportDataCount;
    }

    /// <summary>
    /// 	This method is used to convert filter string into required format.
    /// </summary>
    /// <param name="asFilterString"> </param>
    /// <returns> string </returns>
    private string FormatFilterString(string asFilterString)
    {
        asFilterString = asFilterString.Replace("AND", "@");
        asFilterString = asFilterString.Replace("OR", "@");
        asFilterString = asFilterString.Replace("(", string.Empty);
        asFilterString = asFilterString.Replace(")", string.Empty);
        asFilterString = asFilterString.Replace("{", string.Empty);
        asFilterString = asFilterString.Replace("}", string.Empty);
        asFilterString = asFilterString.Remove(asFilterString.Length - 1);

        if (msReportID == S_INTERNAL_FEE || msReportID == S_PENDING_INTERNAL_FEE)
            asFilterString = asFilterString.Replace("#", "(").Replace("^", ")");

        return asFilterString;
    }

    /// <summary>
    /// 	Special case is handled for the Muster roll report as student_id is always null.
    /// </summary>
    /// <param name="asFilterSting"> </param>
    /// <returns> </returns>
    private string GetMusterReportFilter(string asFilterSting)
    {
        if (msReportID == S_MUSTER_REPORT || msReportID == S_LECTUREWISE_STUDENT_ATTENDANCE)
            asFilterSting += msReportViewName + ".student_id = null AND ";
        return asFilterSting;
    }

    /// <summary>
    /// 	This method generates the report filter as per the controls. Datatype 'checkboxlist'(S_NVARCHAR_CONTROL) is for the Checklistbox control. Datatype 'dropdownlist'(S_NUMERIC_CONTROL) is for DropdownListBox control. Datatype 'textbox'(S_TEXTBOX_CONTROL) is for TextBox control. Datatype 'Datetime' (S_DATETIME_CONTROL) is for calendar control. If DataType is null then there is no any control assigned.
    /// </summary>
    /// <returns> </returns>
    private string GetReportFilter()
    {
        string sFilterSting = string.Empty;
        string sParameterName = grdDisplayParameter.DataKeys[Constants.I_ZERO][I_FIELD_NAME_INDEX].ToString();     
        msReportViewName = sParameterName.Substring(Constants.I_ZERO, sParameterName.Contains(hidSchemaName.Value) ? sParameterName.LastIndexOf(".") : sParameterName.IndexOf("."));

        int iStartIndex = 0;
        int iRowCount = grdDisplayParameter.Rows.Count;
        ReadViewOrUSPName();
        sFilterSting = GetSchoolAcdYearFilter();
        sFilterSting = GetMusterReportFilter(sFilterSting);
        for (int aiGridRowCount = iStartIndex; aiGridRowCount < iRowCount; aiGridRowCount++)
        {
            string sReport_Field_Id = grdDisplayParameter.DataKeys[aiGridRowCount]["Is_Report_Filter_Field"].ToString().Trim();
            if (sReport_Field_Id != Constants.C_YES.ToString())
                continue;
            string sDataType = grdDisplayParameter.DataKeys[aiGridRowCount][I_DATATYPE_INDEX].ToString().ToLower();
            switch (sDataType)
            {
                case S_CHECKBOXLIST:
                    sFilterSting += CreateChkBoxFilterString(aiGridRowCount);
                    break;
                case S_DROPDOWNLIST:
                    sFilterSting += CreateDropDownListFilterString(aiGridRowCount);
                    break;
                case S_TEXTBOX:
                    sFilterSting += CreateTextBoxFilterString(aiGridRowCount);
                    break;
                case S_DATETIME:
                    sFilterSting += CreateDateTimeFilterString(aiGridRowCount);
                    break;
            }
        }
        sFilterSting = UpdateReportFilter(sFilterSting);
        return sFilterSting.Trim();
    }

    /// <summary>
    /// This method is used to update report filter string.
    /// </summary>
    /// <param name="sFilterSting"></param>
    /// <returns></returns>
    private string UpdateReportFilter(string sFilterSting)
    {
        if (msReportID == S_TASK_DETAILS)
            sFilterSting += "({usp_GetDesignationwiseUserTaskDetails_Report;1.OwnerUserId} =" + miUserId + ")@";
        else if (msReportID == S_STUD_FINAL_RESULT_SS && miSchoolId == Constants.SchoolId.STSS.ToInt())
            sFilterSting += "({usp_StudentwiseFinalProgressReportForSTSS;1.Term_Id=null  AND {usp_StudentwiseFinalProgressReportForSTSS;1.IsFromReportScreen}=1  AND {usp_StudentwiseFinalProgressReportForSTSS;1.IsFinalResult}=1})@";
        else if ((msReportID == S_STUD_FINAL_RESULT_SS || msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_PEMS) && miSchoolId == Constants.SchoolId.PEMS.ToInt())
            sFilterSting += "({usp_StudentwiseFinalProgressReportForPEMS;1.Term_Id=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_SS && miAcademicYearId < 7 && miSchoolId != Constants.SchoolId.PKIS.ToInt())
            sFilterSting += "({USP_StudentwiseProgressReportForSS;1.Term_Id=null  AND {USP_StudentwiseProgressReportForSS;1.IsFinalResult}=1})@";
        else if (msReportID == S_STUD_FINAL_RESULT_SS && miAcademicYearId >= 7)
            sFilterSting += "({USP_StudentwiseProgressReportForSS;1.Term_Id=null)@";
        else if (msReportID == S_FINAL_REPORT_JOS)
            sFilterSting += "({USP_StudentwiseProgressReportForJOS;1.Term_Id=null})@";
        else if ((msReportID == S_STUD_FINAL_RESULT_PPSH || msReportID == S_STUD_FINAL_RESULT_PPSH_Old) && miSchoolId != Constants.SchoolId.SVP.ToInt() && miSchoolId != Constants.SchoolId.MVPS.ToInt() && miSchoolId != Constants.SchoolId.BMFS.ToInt() && miSchoolId != Constants.SchoolId.PIONEER.ToInt() && miSchoolId != Constants.SchoolId.PPSH.ToInt() && miSchoolId != Constants.SchoolId.DPIS.ToInt())
            sFilterSting += "({USP_StudentwiseProgressReportForBMFS;1.Term_Id}=null AND {usp_StudentwiseProgressReportForBMFS;1.Note}=null AND {usp_StudentwiseProgressReportForBMFS;1.IsFinalResult}=1 AND {usp_StudentwiseProgressReportForBMFS.IsFromReportScreen}=0)@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH_Old && miSchoolId == Constants.SchoolId.SVP.ToInt())
            sFilterSting += "({USP_StudentProgressReportSVP;1.Term_Id}=null AND {USP_StudentProgressReportSVP;1.IsFromReportScreen}=1 AND {USP_StudentProgressReportSVP;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH_Old && miSchoolId == Constants.SchoolId.SVP.ToInt())
            sFilterSting += "({USP_StudentProgressReportSVP;1.Term_Id}=null AND {USP_StudentProgressReportSVP;1.Note}=null)@";
        else if (msReportID == S_STUD_EXAM_RESULT_PPSN && miSchoolId == Constants.SchoolId.SVP.ToInt())
            sFilterSting += "({USP_StudentTermProgressReportSVP;1.IsFromReportScreen}=1 AND {USP_StudentTermProgressReportSVP;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std && miSchoolId != Constants.SchoolId.SPS.ToInt() && miSchoolId != Constants.SchoolId.PPSH.ToInt() && !Settings.IsAaryanSchool)
            sFilterSting += "({usp_StudentwiseProgressReportSNS;1.Term_Id}=null AND {usp_StudentwiseProgressReportSNS;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std && miSchoolId == Constants.SchoolId.SPS.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportCBSEForSPS;1.Term_Id}=null AND {USP_StudentFinalProgressReportCBSEForSPS;1.IsTopperReport}=0 AND {USP_StudentFinalProgressReportCBSEForSPS;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_FOR_PPSN && miSchoolId == Constants.SchoolId.PPSN.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportForPPSN;1.Term_Id=null} AND {usp_StudentwiseProgressReportForPPSH;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_FOR_9 && miSchoolId != Constants.SchoolId.SPS.ToInt() && (!Settings.IsAaryanSchool))
            sFilterSting += "({USP_StudentFinalProgressReportCBSE9thStd;1.Term_Id=null} AND {USP_StudentFinalProgressReportCBSE9thStd;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_FOR_9 && miSchoolId == Constants.SchoolId.SPS.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportCBSE9thStdForSPS;1.Term_Id=null} AND {USP_StudentFinalProgressReportCBSE9thStdForSPS;1.IsTopperReport=0} AND {USP_StudentFinalProgressReportCBSE9thStdForSPS;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_FOR_11)
            sFilterSting += "({USP_StudentFinalProgressReportCBSE11thStdForSPS;1.Term_Id=null} AND {USP_StudentFinalProgressReportCBSE11thStdForSPS;1.IsTopperReport=0} AND {USP_StudentFinalProgressReportCBSE11thStdForSPS;1.Note}=)@";
        else if (msReportID == S_FORM_NO_16)
            sFilterSting += "({usp_GetIncomeTaxDetailsForReort;1.HasFullAccess} =" + (moUserRole == Constants.UserRoles.Admin || hidHasFullAccess.Value == Constants.S_ONE ? 1 : 0) + ")@";
        //else if (msReportID == S_EXPORT_STUDENT_LIST)
        //    sFilterSting += "({usp_GetStudents_Export;1.IsExact}=0 AND {usp_GetStudents_Export;1.Standard_Id}=0 AND {usp_GetStudents_Export;1.Division_id}=0 AND {usp_GetStudents_Export;1.Name}=-9999 AND {usp_GetStudents_Export;1.RegNo}=-9999"+
        //" AND {usp_GetStudents_Export;1.Prefix}=null AND {usp_GetStudents_Export;1.Operator}=null)@";
        else if (msReportID == S_INTERNAL_FEE)
            sFilterSting += "({usp_StudentInternalFeeDetailsReport;1.Type}=1)@";
        else if (msReportID == S_PENDING_INTERNAL_FEE)
            sFilterSting += "({usp_StudentInternalFeeDetailsReport;1.FromDate}=null AND {usp_StudentInternalFeeDetailsReport;1.Type}=2)@";
        else if (msReportID == S_CCE_REPORT)
            sFilterSting += "({usp_StudentInternalFeeDetailsReport;1.Subject_Id}=null)@";
        else if (msReportID == S_CCE_REPORT_GRADE)
            sFilterSting += "({usp_StudentInternalFeeDetailsReport;1.Subject_Id}=null)@";
        else if (msReportID == S_NET_SALARY)
            sFilterSting += "({usp_GetNetSalaryDetails;1.Format}=" + (DDLFormatType.SelectedValue == "Excel" ? 2 : 1) + ")@";
        else if (msReportID == S_EXAM_RESULT_SS && miSchoolId != Constants.SchoolId.PPSH.ToInt() && miSchoolId != Constants.SchoolId.STSS.ToInt())
            sFilterSting += "({usp_StudentwiseProgressReportForJPS;1.IsFinalResult=0})@";
        else if (msReportID == S_EXAM_RESULT_SS && miSchoolId == Constants.SchoolId.STSS.ToInt())
            sFilterSting += "({usp_StudentwiseProgressReportForSTSS;1.IsFinalResult=0 AND usp_StudentwiseProgressReportForSTSS.IsFromReportScreen=1})@";
        else if (msReportID == S_EXAM_RESULT_STSS_9STD && miSchoolId == Constants.SchoolId.STSS.ToInt())
            sFilterSting += "({usp_StudentwiseProgressReportForSTSS_9Std;1.IsFinalResult=0 AND usp_StudentwiseProgressReportForSTSS_9Std.IsFromReportScreen=1})@";
        else if (msReportID == S_EXAM_RESULT_STSS_10STD && miSchoolId == Constants.SchoolId.STSS.ToInt())
            sFilterSting += "({usp_StudentwiseProgressReportForSTSS10;1.IsFinalResult=0 AND {usp_StudentwiseProgressReportForSTSS10;1.Term_Id=null AND {usp_StudentwiseProgressReportForSTSS10;1.Note= AND usp_StudentwiseProgressReportForSTSS10.IsFromReportScreen=1})@";
        else if (msReportID == S_EXAM_RESULT_SS && miSchoolId == Constants.SchoolId.PPSH.ToInt())
            sFilterSting += "({usp_StudentwiseProgressReportForPPSH;1.IsFinalResult=0 AND usp_StudentwiseProgressReportForPPSH.IsFromReportScreen=1})@";
        else if (msReportID == S_FINAL_REPORT_JPS)
            sFilterSting += "({usp_StudentwiseProgressReportForJPS;1.Term_Id=null AND {usp_StudentwiseProgressReportForJPS;1.Note=null AND {usp_StudentwiseProgressReportForJPS;1.IsFinalResult=1})@";
        else if (msReportID == S_FINAL_REPORT_GSS)
            sFilterSting += "({usp_StudentwiseProgressReportForGSS;1.Term_Id=null AND {usp_StudentwiseProgressReportForGSS;1.Note=null AND {usp_StudentwiseProgressReportForGSS;1.IsFinalResult=1})@";
        else if (msReportID == LC_ISSUE_REGISTER)
            sFilterSting += "({usp_GetLCIssueRegisterDetails:1.ReportId} =" + msReportID + ") AND ({usp_GetLCIssueRegisterDetails:1.AcademicYearId} = null)@";
        else if (msReportID == LC_ISSUE_LOG)
            sFilterSting += "({usp_GetLCIssueRegisterDetails:1.ReportId} =" + msReportID + ")@";
        else if (msReportID == S_ENROLLMENTWISE_STUDENT_I_CARDS && miSchoolId == Constants.SchoolId.BFS.ToInt())
            sFilterSting += "({usp_GetStudentIdentityCardDetails_Report_GSS;1.Student_Id}=0 AND {usp_GetStudentIdentityCardDetails_Report_GSS;1.Standard_Id}=0 AND {usp_GetStudentIdentityCardDetails_Report_GSS;1.Division_Id}=0 AND {usp_GetStudentIdentityCardDetails_Report_GSS;1.StudentsWithoutPhoto} = 1)@";
        else if (msReportID == S_ENROLLMENTWISE_STUDENT_I_CARDS && miSchoolId == Constants.SchoolId.BMFS.ToInt())
            sFilterSting += "({usp_GetStudentIdentityCardDetails_ReportBMFS;1.Student_Id}=0 AND {usp_GetStudentIdentityCardDetails_ReportBMFS;1.Standard_Id} = 0 AND {usp_GetStudentIdentityCardDetails_ReportBMFS;1.Division_Id} = 0)@";
        else if (msReportID == S_ENROLLMENTWISE_STUDENT_I_CARDS && miSchoolId == Constants.SchoolId.MNS.ToInt())
            sFilterSting += "({usp_GetStudentIdentityCardDetails_Report_MNS;1.Student_Id}=0 AND {usp_GetStudentIdentityCardDetails_Report_MNS;1.Standard_Id} = 0 AND {usp_GetStudentIdentityCardDetails_Report_MNS;1.Division_Id} = 0)@";
        else if (msReportID == S_ENROLLMENTWISE_STUDENT_I_CARDS && miSchoolId == Constants.SchoolId.SS.ToInt())
            sFilterSting += "({usp_GetStudentIdentityCardDetails_Report_SS;1.Student_Id} = 0 AND {usp_GetStudentIdentityCardDetails_Report_SS;1.Standard_Id} = 0 AND {usp_GetStudentIdentityCardDetails_Report_SS;1.Division_Id} = 0)@";
        else if (msReportID == S_FEE_RECEIPT_DETAILS && miSchoolId == Constants.SchoolId.GSS.ToInt())
            sFilterSting += "({usp_GetReceiptDetailsForReport;1.FeeTypeId} = 0 )@";
        else if (msReportID == S_FEE_RECEIPT_DETAILS && miSchoolId == Constants.SchoolId.SNS.ToInt())
            sFilterSting += "({usp_GetReceiptDetailsForReport;1.ToDate} =1-Jan-1900)@";
        else if (msReportID == S_STAFF_ATTENDANCE && miSchoolId != Constants.SchoolId.SNS.ToInt())
            sFilterSting += "({usp_GetStaffAttendanceForReport;1.IncludeCHB}=0)@";
        else if (miSchoolId == Constants.SchoolId.PPS.ToInt() && msReportID == S_BONAFIDE_CERTIFICATE_REPORT_ID)
            sFilterSting += "({usp_GetBonafideCertificateDetails_Report;1.StudentFMLName} =" + string.Empty + " AND {usp_GetBonafideCertificateDetails_Report;1.Purpose} =" + string.Empty + ")@";
        else if (miSchoolId == Constants.SchoolId.PKIS.ToInt() && msReportID == S_STUD_EXAM_RESULT_PPSN)
            sFilterSting += "({USP_StudentProgressReportPKSC;1.IsFromReportScreen} =" + Constants.I_ZERO + ")@";
        else if (msReportID == S_STANDARDWISE_TEST_DETAILS)
            sFilterSting += "({usp_StudentwiseTestReportSPS;1.IsAccessedFromScreen} =" + Constants.I_ZERO + ")@";
        else if (msReportID == S_RESULTSHEET && miSchoolId == Constants.SchoolId.SPS.ToInt())
            sFilterSting += "({usp_GetClasswiseExamDetailsSPS;1.IsBlankReport} =" + Constants.I_ZERO + ")@";
        else if (msReportID == S_RESULTSHEET && miSchoolId == Constants.SchoolId.SVP.ToInt())
            sFilterSting += "({usp_GetClasswiseExamDetailsSVP;1.IsBlankReport} =" + Constants.I_ZERO + ")@";
        else if (msReportID == S_STUD_FINAL_RESULT_SS && miSchoolId == Constants.SchoolId.PKIS.ToInt())
            sFilterSting += "({USP_StudentProgressReportPrePrimaryPKSC;1.Term_Id}=2 AND {USP_StudentProgressReportPrePrimaryPKSC;1.IsFinalResult}=1 AND {USP_StudentProgressReportPrePrimaryPKSC;1.IsFromReportScreen}=1)@";
        else if (msReportID == S_LC_REPORT_ID && (miSchoolId == Constants.SchoolId.CSNP.ToInt() || miSchoolId == Constants.SchoolId.CSNS.ToInt()))
            sFilterSting += "({usp_LeavingCertificate_SSN;1.DisplayInMarathi}=0)@";
        else if (msReportID == S_STUD_EXAM_RESULT_PPSN && miSchoolId == Constants.SchoolId.MVPS.ToInt())
            sFilterSting += "({USP_StudentProgressReportMVPS;1.IsFromReportScreen}=1)@";
        else if ((msReportID == S_ANNUAL_CONSOLDATED_REPORT || msReportID == S_ANNUAL_CONSOLDATED_UNITTEST_REPORT_SVNP) && miSchoolId == Constants.SchoolId.SVNP.ToInt())
            sFilterSting += "({usp_Annual_Consolidation_Report_SVNP;1.Showgrade}=N)@";
        else if (msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS && miSchoolId != Constants.SchoolId.SNS.ToInt())
            sFilterSting += "({usp_GetIssuedBookDetails_Report;1.AccessionNoPrefix}=1)@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH_Old && miSchoolId == Constants.SchoolId.MVPS.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportMVPS;1.Term_Id}=null AND {USP_StudentFinalProgressReportMVPS;1.Note}=null AND {USP_StudentFinalProgressReportMVPS.IsFromReportScreen}=0)@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH_Old && miSchoolId == Constants.SchoolId.SVNP.ToInt())
            sFilterSting += "({usp_StudentwiseProgressReportSVNP;1.Term_Id}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH && miSchoolId == Constants.SchoolId.BMFS.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportCBSEForBMFS;1.Term_Id}=null AND {USP_StudentFinalProgressReportCBSEForBMFS;1.Note}=null)@";
        else if (msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_PEMS && miSchoolId == Constants.SchoolId.BMFS.ToInt())
            sFilterSting += "({usp_StudentwiseProgressReportForBMFSFinal_PrePrimary;1.Term_Id}=null AND {usp_StudentwiseProgressReportForBMFSFinal_PrePrimary;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH && miSchoolId == Constants.SchoolId.SVP.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportSVP_9;1.Term_Id}=null AND {USP_StudentFinalProgressReportSVP_9;1.Note}=null)@";
        else if (msReportID == S_PRELIM_RESULT_SHEET)
            sFilterSting += "({usp_GetResultSheetDetails;1.Division_Id}=0)@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH_Old && miSchoolId == Constants.SchoolId.PIONEER.ToInt())
            sFilterSting += "({USP_StudentProgressReportCBSEForPioneer;1.Term_Id}=null AND {USP_StudentProgressReportCBSEForPioneer;1.Note}=null)@";
        else if (msReportID == S_SALARY_SLIP)
            sFilterSting += "({usp_GetSalarySlipDetails;1.LoginUserId}=" + miUserId + ")@";
        else if (msReportID == S_EXAM_RESULT_STSS_10STD && miSchoolId == Constants.SchoolId.PPSN.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportCBSE10thStd;1.Term_Id=null AND {USP_StudentFinalProgressReportCBSE10thStd;1.Note= )@";
        else if (msReportID == S_EXAM_RESULT && miSchoolId == Constants.SchoolId.PPS.ToInt())
            sFilterSting += "({USP_StudentwiseProgressReport;1.IsFromReportScreen=1})@";
        else if (msReportID == S_STUD_TERM1_RESULT && miSchoolId == Constants.SchoolId.PPS.ToInt())
            sFilterSting += "({USP_StudentTerm1ProgressReport;1.IsFromReportScreen=1})@";
        else if (msReportID == S_EXPORT_ADMISSION_DETAILS)
            sFilterSting += "({usp_GetNewAdmissionsDetailsExport;1.Division_Id=0})@";
        else if (miSchoolId == Constants.SchoolId.PPS.ToInt() && msReportID == S_XSEED_REPORT)
            sFilterSting += "({Xseed.usp_GetXseedProgressReport.IsFromReportScreen}=1})@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH_Old && miSchoolId == Constants.SchoolId.DPIS.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportForDPIS;1.Term_Id}=null AND {USP_StudentFinalProgressReportForDPIS;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std && Settings.IsAaryanSchool)
            sFilterSting += "({USP_StudentFinalProgressReportFor1stTO4th_Aaryan;1.Term_Id}=null AND {USP_StudentFinalProgressReportFor1stTO4th_Aaryan;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_FOR_PPSN && miSchoolId == Constants.SchoolId.NPS.ToInt())
            sFilterSting += "({usp_StudentwiseProgressReportForNPS;1.Term_Id}=null AND {usp_StudentwiseProgressReportForNPS;1.Note}=null)@";
        else if (msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std)
            sFilterSting += "({usp_GetStudentwiseProgressReportDetailsFor6to8_PPSH;1.IsFromStudnetLogin}=0)@";
        else if (msReportID == S_STUDENT_OBSERVATION_REPORT && miSchoolId == Constants.SchoolId.PPSH.ToInt())
            sFilterSting += "({usp_GetStudentObservationDetailsForReport_PPSH;1.IsFromStudentLogin}=0)@";
        else if (msReportID == S_STUD_FINAL_RESULT_PPSH)
            sFilterSting += "({usp_GetStudentwiseProgressReportDetailsFor9th_PPSH;1.IsFromStudentLogin}=0)@";
        else if (msReportID == S_XSEED_REPORT && miSchoolId == Constants.SchoolId.NPS.ToInt())
            sFilterSting += "({Xseed.usp_GetXseedProgressReport;1.IsFromReportScreen}=0)@";
        else if (msReportID == S_STUDENT_TERM1_PROGRESS_REPORT && miSchoolId == Constants.SchoolId.DPIS.ToInt())
            sFilterSting += "({USP_StudentFinalProgressReportForDPIS;1.Term_Id}=1 AND {USP_StudentFinalProgressReportForDPIS;1.Note}=null)@";
        else if (msReportID == S_STUDENT_TERM1_PROGRESS_REPORT_PPSN && miSchoolId == Constants.SchoolId.PPSN.ToInt())
            sFilterSting += "({USP_StudentTerm1ProgressReportCBSEForPPSN;1.Term_Id}=1 AND {USP_StudentTerm1ProgressReportCBSEForPPSN;1.Note}=null)@";
        else if (msReportID == S_STUDENT_TERM1_PROGRESS_REPORT_PPSN && miSchoolId == Constants.SchoolId.SNS.ToInt())
            sFilterSting += "({USP_StudentTerm1ProgressReportCBSEForSNS;1.Term_Id}=1 AND {USP_StudentTerm1ProgressReportCBSEForSNS;1.Note}=null)@";
        else if (msReportID == S_PRELIM_REPORT_PP)
            sFilterSting += "({usp_GetPrelimProgressReportForPP;1.Term_Id}=2 AND {usp_GetPrelimProgressReportForPP;1.IsFromReportScreen=1})@";
        else if (msReportID == S_NEXT_YEAR_PAID_FEE && moSchool == Constants.SchoolId.SNS)
            sFilterSting += "({usp_GetNextYearFeePaymentDetails;1.FeeCategoryId}=1)@";
        else if (msReportID == S_ANNUAL_INCREMENT_LETTER)
            sFilterSting += "({usp_GetAnnualIncrementLetterDetails;1.LoginUserId}="+miUserId+")@";
        else if (msReportID == S_EMPLOYMENT_CONFIRMATION_LETTER || msReportID == S_EMPLOYMENT_CONFIRMATION_LETTER_IN_DETAILS)
            sFilterSting += "({usp_GetDetailsForEmploymentConfirmationLetter;1.ReportId}=" + msReportID + ")@";
        else if (msReportID == S_DATEWISE_Fee_COLLECTION && moSchool == Constants.SchoolId.VPMCPS)
            sFilterSting += "({usp_GetDatewiseFeeDetailsReport_SNS;1.IncludeConcession}=0)@";
        else if (msReportID == S_INTERNAL_FEE_RECEIPT_DETAILS)
            sFilterSting += "({usp_GetInternalFeeReceiptForReport;1.ReceiptNumber}=0)@";
        else if (msReportID == S_STUDENT_TERM1_PROGRESS_REPORT && moSchool == Constants.SchoolId.VPMCPS)
            sFilterSting += "({usp_GetTerm1ProgressReportForVPMCPS;1.IsFromReportScreen}=1)@";
        else if (msReportID == S_STUDENT_OBSERVATION_REPORT && miAcademicYearId < 10)
            sFilterSting += "({usp_StudentTermProgressReportForSNS1TO5;1.IsOpenFromReportScreen}=1)@";
        else if (msReportID == S_EXPORT_STUDENT_MONTHLY_STATUS)
            sFilterSting += "({Usp_StudentsMonthlyStatusReport;1.StudentId}=0)@";
        else if (msReportID == S_EXPORT_STUDENTS_RECEIPTS_DETAILS)
            sFilterSting += "({usp_GetReceiptDetailsforStandards;1.ReceiptNo}=0)@";
        else if (msReportID == S_CA_RECONSOLIDATION_DETAILS)
            sFilterSting += "({usp_GetCAReconsolidationReport_SNS;1.FromDate}=)@";
        else if (msReportID == S_TERM_PROGRESS_REPORT_PIONEER)
            sFilterSting += "({usp_GetProgressReportDetailsForPrePrimaryPioneer;1.IsFromReportScreen}=1)@";
        else if (msReportID == S_STUDENT_HALF_YEARLY_3TO9)
            sFilterSting += "({usp_GetDetailsForHalfYearlyReport_Pioneer;1.Term_Id}=1 AND {usp_GetDetailsForHalfYearlyReport_Pioneer;1.IsFromReportScreen}=1)@";
        else if(msReportID == S_MISSING_ATTENDANCE_REPORT)
            sFilterSting += "({usp_GetMissingAttendanceDetails;1.UserId}=null)@";
        return sFilterSting;
    }

    /// <summary>
    /// 	This method is used to create filter string for school_Id and Academic_Year_Id.
    /// </summary>
    /// <returns> </returns>
    private string GetSchoolAcdYearFilter()
    {
        string sSchoolYearFilter;
        string sFilterSting = string.Empty;
        const int I_PARAMETER_FILTER = 4;
        string sSign = "@ ";
        string sViewNameSchID = msReportViewName + ".School_Id}";
        string sViewNameAcdYearId = msReportViewName + ".Academic_Year_Id}";
        string sViewNameFinanYearId = msReportViewName + ".FinancialYearId}";
        string sFiterParameter = grdDisplayParameter.DataKeys[Constants.I_ZERO][I_PARAMETER_FILTER].ToString().Trim();
        if (sFiterParameter != string.Empty)
        {
            var iParameterFilters = (Constants.ReportParameterFilters)Convert.ToInt32(grdDisplayParameter.DataKeys[Constants.I_ZERO][I_PARAMETER_FILTER]);

            if (msReportID == S_LEAVE_BALANCE)
                iParameterFilters = (Constants.ReportParameterFilters)Convert.ToInt32(grdDisplayParameter.DataKeys[Constants.I_TWO][I_PARAMETER_FILTER]);

            if (iParameterFilters == Constants.ReportParameterFilters.SchoolId)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + ")";
                sFilterSting += sSchoolYearFilter + sSign;
            }
            else if (iParameterFilters == Constants.ReportParameterFilters.WithoutSchoolAcademic && (msReportID == S_STAFF_BIRTHDAY || msReportID == S_CATEGORYWISE_ITEM_DETAILS ))
            {
            }
            else
            {
                switch (msReportID)
                {
                    case S_PROVIDENT_FUND_DETAILS:
                        {
                            string sOriginalEarningDeductionId = " AND (" + msReportViewName + ".OriginalEarningDeductionId}=15)";
                            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3}){4}", sViewNameSchID, miSchoolId, sViewNameAcdYearId, miAcademicYearId, sOriginalEarningDeductionId);
                        }
                        break;
                    case S_PROFESSIONAL_TAX_DETAILS:
                        {
                            string sOriginalEarningDeductionId = " AND (" + msReportViewName + ".OriginalEarningDeductionId}=16)";
                            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3}){4}", sViewNameSchID, miSchoolId, sViewNameAcdYearId, miAcademicYearId, sOriginalEarningDeductionId);
                        }
                        break;
                    case S_LEAVE_BALANCE:
                        sSchoolYearFilter = string.Format("({0}={1} AND {2}={3})" + " AND ({4}.MonthId}}=0)", sViewNameSchID, miSchoolId, sViewNameAcdYearId, miAcademicYearId, msReportViewName);
                        break;
                    case S_LC_REPORT_ID:
                        if (miSchoolId == Constants.SchoolId.LFS.ToInt())
                            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3})", sViewNameSchID, miSchoolId, sViewNameAcdYearId, miAcademicYearId);
                        else
                            sSchoolYearFilter = sViewNameSchID + "=" + miSchoolId;
                        break;
                    case S_TRANSFER_CERTIFICATE:
                        sSchoolYearFilter = sViewNameSchID + "=" + miSchoolId;
                        break;
                    case S_TEACHER_UDISE_DETAILS :                        
                        sSchoolYearFilter = sViewNameSchID + "=" + miSchoolId;
                        break;
                    case S_UDISE_DETAILS :
                        sSchoolYearFilter = sViewNameSchID + "=" + miSchoolId;
                        break;
                    case S_EXPORT_STUDENT_LIST:
                        sSchoolYearFilter = string.Format("({0}={1} AND {2}={3})", sViewNameSchID, miSchoolId, sViewNameAcdYearId, miAcademicYearId);
                        sSchoolYearFilter = sSchoolYearFilter.Replace("School_Id", "iSchoolId").Replace("Academic_Year_Id", "iAcademicYrId");
                        break;
                    case S_LEAVING_CERTIFICATE_10TH_NPS_ID :
                        if (miSchoolId == Constants.SchoolId.LFS.ToInt())
                            sSchoolYearFilter = string.Format("({0}={1} AND {2}={3})", sViewNameSchID, miSchoolId, sViewNameAcdYearId, miAcademicYearId);
                        else
                            sSchoolYearFilter = sViewNameSchID + "=" + miSchoolId;
                        break;
                    default:
                        sSchoolYearFilter = string.Format("({0}={1} AND {2}={3})", sViewNameSchID, miSchoolId, sViewNameAcdYearId, miAcademicYearId);
                        break;
                }
                sFilterSting += sSchoolYearFilter + sSign;
            }
        }
        else
        {
            if (miSchoolId == Constants.SchoolId.SNS.ToInt() && msReportID == S_FEE_RECEIPT_DETAILS)
            {
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + " AND " + sViewNameAcdYearId + "=" + miAcademicYearId + " AND " + sViewNameFinanYearId + "=" + miFinancialYearId + ")";
            }
            else
                sSchoolYearFilter = "(" + sViewNameSchID + "=" + miSchoolId + " AND " + sViewNameAcdYearId + "=" + miAcademicYearId + ")";

            sFilterSting += sSchoolYearFilter + sSign;
        }
        return sFilterSting;
    }

    /// <summary>
    /// 	This method returns the filter criteria of the calendar control.
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    /// <returns> </returns>
    private string CreateDateTimeFilterString(int aiGridRowCount)
    {
        string sParameterFilterString;
        HidPrintDate.Value = string.Empty;
        string sParameterName = grdDisplayParameter.DataKeys[aiGridRowCount][I_FIELD_NAME_INDEX].ToString();
        var oCalendar = grdDisplayParameter.Rows[aiGridRowCount].FindControl("CalenderRptParameter") as PopCalendar;
        string sFilterValue = oCalendar.DateValue.ToDateTime().ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"));
        if (msType == "SP")
            if (msReportID == S_CLASSWISE_STUDENT_PENDING_FEE_REPORT_ID || msReportID == S_REQUISITION_DETAILS || msReportID == S_PENDING_FEE_DETAILS || msReportID == S_PENDING_FEE_STUDENTLIST || msReportID == S_NETBANKING_REPORT || msReportID == S_TASK_DETAILS || msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS || msReportID == S_LOST_BOOK_DETAILS || msReportID == S_CLAIM_BOOK_DETAILS || msReportID == S_LC_REPORT_ID || msReportID == S_DAILY_FEE_COLLECTION || msReportID == S_LEAVING_CERTIFICATE_10TH_NPS_ID
                || msReportID == S_NEXT_YEAR_PAID_FEE || msReportID == S_TRANSFER_CERTIFICATE || msReportID == S_STUDENT_ALL_ACADEMICS_PENDING_FEE || msReportID == S_TEACHER_JOINING_DATE || msReportID == S_USER_RETIREMENT_DETAILS_REPORT || msReportID == S_STUDENT_PENDING_FEE_REMINDER || msReportID == S_STUDENT_HALF_YEARLY_3TO9)
                sParameterFilterString = sFilterValue == "1/1/0001" || sFilterValue == "01-Jan-0001" ? string.Format("{0}= null   @ ", sParameterName) : string.Format("{0}={1}   @ ", sParameterName, sFilterValue);
            else if (msReportID == S_STUDENT_FEE_DETAILS)
            {
                    sParameterFilterString = " AND "+sParameterName+"="+sFilterValue;
            }
            else
                sParameterFilterString = string.Format("Date({0})" + "={1}   OR ", sParameterName, sFilterValue);
        else if (msReportID == S_DATEWISE_POSTDATED_CHEQUE_REPORT_ID)
            //For DateWise PostDated Cheque Report the special case is handeld for the Start date and end date.                
            sParameterFilterString = string.Format(sParameterName == "{vw_PostDated_Cheque_Report.Cheque_Date}" ? "Date({0}) >= #{1}#   AND " : "Date({0}) <= #{1}#   OR ", sParameterName, sFilterValue);
        else
            sParameterFilterString = string.Format("Date({0})=#{1}#   OR ", sParameterName, sFilterValue);
        if (msReportID == S_LC_REPORT_ID || msReportID == S_LEAVING_CERTIFICATE_10TH_NPS_ID)
            HidPrintDate.Value = sFilterValue;
        if (msReportID == S_TRANSFER_CERTIFICATE)
            HidPrintDate.Value = sFilterValue;
        return sParameterFilterString;
    }

    /// <summary>
    /// 	This method returns the filter criteria of the CheckListBox control.
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    /// <returns> </returns>
    private string CreateChkBoxFilterString(int aiGridRowCount)
    {
        string sParameterFilterString = string.Empty;
        string sFilterValue = string.Empty;
        string sFilterString = string.Empty;
        if (msReportID != S_SUBJECT_TOPPERS && msReportID !=S_TESTWISE_SUBJECT_TOPPERS)
        {
            string sParameterName = grdDisplayParameter.DataKeys[aiGridRowCount][I_FIELD_NAME_INDEX].ToString();
            var oCheckBoxList = grdDisplayParameter.Rows[aiGridRowCount].FindControl("ChkRptParameter") as CheckBoxList;
            sFilterString += "(";

            switch (msReportID)
            {
                case S_PROVIDENT_FUND_DETAILS:
                case S_STAFF_ATTENDANCE:
                case S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE :
                case S_STAFF_LEAVES:
                case S_STUDENT_IDENTITY_CARDS:
                case S_AGE_CALCULATION:
                case S_STUDENT_FEE_REPORT:
                case S_STUDENT_AUTHORITY_CARDS:
                case S_DAILY_FEE_COLLECTION:
                case S_RESULTSHEET:
                case S_STUDENTS_HOUSE:
                case S_SERVICE_TYPE_DETAILS:
                case S_CATEGORYWISE_ITEM_DETAILS:
                case S_INVESTMENT_DECLARATIONS:
                case S_SALARY_SHEET:
                case S_EMPLOYEE_INFORMATION_FOR_REPORT:
                case S_EMPLOYEE_INFORMATION_DETAILS:
                case S_LC_REPORT_ID :
                case S_STUDENT_ADDRESS_REPORT:
                case S_STAFF_KID_FEE:
                case S_TRANSFER_CERTIFICATE:
                case S_PARENT_IDENTITY_CARDS:
                case S_TEACHER_UDISE_DETAILS:
                case S_UDISE_DETAILS:
                case S_MARK_ENTRY_STATUS:
                case S_MARK_ENTRY_FORM_REPORT:
                case S_RTE_STUDENT_LIST:
                case S_STUDENT_REGISTRATION_DEATILS:
                case S_USERWISE_LOGIN_DURATION_DETAILS: 
                case S_LEAVING_CERTIFICATE_10TH_NPS_ID:  
                case S_BANK_CHALLAN_REPORT:  //
                case S_NEXT_YEAR_PAID_FEE:
                case S_DYNAMIC_PENDING_FEE_REPORT:                
				case S_STUDENT_NEW_IDENTITY_CARDS :
                        sFilterValue = "0";
                        if (oCheckBoxList.Items[0].Selected)
                            sFilterValue = "1";
                        sFilterString += sParameterName + "=" + sFilterValue + " OR ";
                        break;
                case S_EXPORT_FEE_DETAILS:
                case S_STUDENT_FEE_DETAILS:
                        for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                        {
                            if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                                continue;
                            sFilterValue +=","+ oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        }

                        if (sFilterValue.Length > 0)
                            sFilterValue = sFilterValue.Substring(1);
                        sFilterString += string.Format("{0} = {1} OR ", sParameterName, sFilterValue);
                        break;
                case S_NOMINAL_ROLL: sFilterValue = "N";
                        if (oCheckBoxList.Items[0].Selected)
                            sFilterValue = "Y";
                        sFilterString += sParameterName + "=" + sFilterValue + " OR ";
                        break;            
                case S_EXPORT_STUDENT_LIST:
                        if (aiGridRowCount == 2 || aiGridRowCount == 3)
                    {
                        sFilterValue = "0";
                        if (oCheckBoxList.Items[0].Selected)
                            sFilterValue = "1";
                        sFilterString += sParameterName + "=" + sFilterValue + " OR ";
                    }
                    else
                    {
                        for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                        {
                            if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                                continue;
                            sFilterValue +=","+ oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        }

                        if (sFilterValue.Length > 0)
                            sFilterValue = sFilterValue.Substring(1);
                        sFilterString += string.Format("{0} = {1} OR ", sParameterName, sFilterValue);
                    }
                    break;
                case S_DATEWISE_Fee_COLLECTION:                
                    if (aiGridRowCount == 5)
                    {
                        sFilterValue = "0";
                        if (oCheckBoxList.Items[0].Selected)
                            sFilterValue = "1";
                        sFilterString += sParameterName + "=" + sFilterValue + " OR ";
                    }
                    else
                    {
                        for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                        {
                            if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                                continue;
                            sFilterValue += "," + oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        }

                        if (sFilterValue.Length > 0)
                            sFilterValue = sFilterValue.Substring(1);
                        sFilterString += string.Format("{0} = {1} OR ", sParameterName, sFilterValue);
                    }
                    break;
                case S_CA_RECONSOLIDATION_DETAILS:
                    if (aiGridRowCount == 4)
                    {
                        sFilterValue = "0";
                        if (oCheckBoxList.Items[0].Selected)
                            sFilterValue = "1";
                        sFilterString += sParameterName + "=" + sFilterValue + " OR ";
                    }
                    else
                    {
                        for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                        {
                            if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                                continue;
                            sFilterValue += "," + oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        }

                        if (sFilterValue.Length > 0)
                            sFilterValue = sFilterValue.Substring(1);
                        sFilterString += string.Format("{0} = {1} OR ", sParameterName, sFilterValue);
                    }
                    break;
                case S_STUDENT_GENERAL_REGISTER_REPORT:
                case S_STAFF_SCREEN_ACCESS_DETAILS:
                case S_EXTERNAL_STUDENT_FEE_DETAILS:
                case S_STUDENT_REFUND_FEE_DETAILS:
                    if (aiGridRowCount == 4)
                    {
                        sFilterValue = "0";
                        if (oCheckBoxList.Items[0].Selected)
                            sFilterValue = "1";
                        sFilterString += sParameterName + "=" + sFilterValue + " OR ";
                    }
                    else
                    {
                        for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                        {
                            if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                                continue;
                            sFilterValue +=","+ oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        }

                        if (sFilterValue.Length > 0)
                            sFilterValue = sFilterValue.Substring(1);
                        sFilterString += string.Format("{0} = {1} OR ", sParameterName, sFilterValue);
                    }
                    break;                               
                case S_PROFESSIONAL_TAX_CHALLAN:
                case S_EARNINGS_DEDUCTIONS:
                case S_STUDENT_DETAIL_INFORMATION:                
                case S_FEE_RECEIPT_DETAILS: 
                case S_STUDENT_PENDING_FFE_DETAILS:
                case S_AREAWISE_PENDINGFEE_DETAILS:
                case S_INTERNAL_FEE_RECEIPT_DETAILS:
                case S_STUDENT_EXCESS_FEE_DETAILS:
                    for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                    {
                        if (oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                            sFilterValue += "," + oCheckBoxList.Items[iChkBoxLstIndex].Value;
                    }
                    if (sFilterValue.StartsWith(","))
                        sFilterValue = sFilterValue.Substring(1);
                    sFilterString += sParameterName + " = " + sFilterValue + " OR ";
                    break;
                case S_PENDING_FEE_STUDENTLIST:
                case S_CLASSWISE_STUDENT_PENDING_FEE_REPORT_ID:
                    sFilterString += sParameterName + " = " + (oCheckBoxList.Items[0].Selected ? Constants.S_YES : Constants.S_NO) + " OR ";
                    break;
                case LC_ISSUE_REGISTER:
                    sFilterString = "("+sParameterName + "= (";
                    for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                    {
                        if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                            continue;
                        sFilterValue = oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        sFilterString += string.Format("{0} ,", sFilterValue);
                    }
                    sFilterString = sFilterString.Remove(sFilterString.Length-1);
                    sFilterString += ")....";
                    break;
                case LC_ISSUE_LOG:
                        sFilterString = "("+sParameterName + "= (";
                    for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                    {
                        if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                            continue;
                        sFilterValue = oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        sFilterString += string.Format("{0} ,", sFilterValue);
                    }
                    sFilterString = sFilterString.Remove(sFilterString.Length-1);
                    sFilterString += ")....";
                    break;
                case S_CLASSWISE_STUDENT_LIST:
                    if (miSchoolId != Constants.SchoolId.PPS.ToInt())
                    {
                        sFilterString = "(" + sParameterName + "= (";
                        for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                        {
                            if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                                continue;
                            sFilterValue = oCheckBoxList.Items[iChkBoxLstIndex].Value;
                            sFilterString += string.Format("{0} ,", sFilterValue);
                        }
                        sFilterString = sFilterString.Remove(sFilterString.Length - 1);
                        sFilterString += ")....";
                    }
                    else
                    {
                        sFilterString += "(";
                        string sData = string.Empty;

                        for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                        {
                            if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                                continue;
                            sFilterValue = oCheckBoxList.Items[iChkBoxLstIndex].Value;

                            sData += " OR " + sParameterName + "=" + sFilterValue;

                        }

                        if (sData.Length > 0)
                            sData = sData.Substring(4);

                        sData += ")....";

                        sFilterString += sData;
                    }
                    break;
                case S_HOUSEWISE_STUDENT_DETAILS:
                    sFilterString = "(" + sParameterName + "= (";
                    for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                    {
                        if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                            continue;
                        sFilterValue = oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        sFilterString += string.Format("{0} ,", sFilterValue);
                    }
                    sFilterString = sFilterString.Remove(sFilterString.Length - 1);
                    sFilterString += ")....";
                    break;
                case S_LEFT_STUDENT_DETAIL:
                    sFilterString = "(" + sParameterName + "= (";
                    for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                    {
                        if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                            continue;
                        sFilterValue = oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        sFilterString += string.Format("{0} ,", sFilterValue);
                    }
                    sFilterString = sFilterString.Remove(sFilterString.Length - 1);
                    sFilterString += ")....";
                    break;
                case S_BONAFIDE_CERTIFICATE_REPORT_ID:
                    sFilterString += sParameterName + " = " + (oCheckBoxList.Items[0].Selected ? Constants.S_ONE : Constants.S_ZERO) + " OR ";
                    break;
                default:
                    for (int iChkBoxLstIndex = 0; iChkBoxLstIndex < oCheckBoxList.Items.Count; iChkBoxLstIndex++)
                    {
                        if (!oCheckBoxList.Items[iChkBoxLstIndex].Selected)
                            continue;
                        sFilterValue = oCheckBoxList.Items[iChkBoxLstIndex].Value;
                        sFilterString += string.Format("{0} = {1} OR ", sParameterName, sFilterValue);
                    }
                    break;
            }
            if (sFilterString != "(" && sFilterString != string.Empty)
                sParameterFilterString += sFilterString.Remove(sFilterString.Length - 4) + ") @";
        }
        return sParameterFilterString;
    }

    /// <summary>
    /// 	This method returns the filter criteria of the DropDownListBox control.
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    /// <returns> </returns>
    private string CreateDropDownListFilterString(int aiGridRowCount)
    {
        string sParameterFilterString = string.Empty;
        string sParameterName = grdDisplayParameter.DataKeys[aiGridRowCount][I_FIELD_NAME_INDEX].ToString();
        if (sParameterName != "{usp_GetTestAndSubjectToppersForReport;1.Type}" && sParameterName != "{usp_GetTestwiseSubjectToppers;1.Type}")
        {
            var oDropDownList = grdDisplayParameter.Rows[aiGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
            //For Muster Report the special case is handeld for the Year Combobox.
            //For this Combo text will be selected.
            string sFilterValue = string.Empty;
            
            if(sParameterName == "{usp_MusterReport_Sanjeevan;1.Year}")
                sFilterValue = sParameterName == "{usp_MusterReport_Sanjeevan;1.Year}" ? oDropDownList.SelectedItem.Text : oDropDownList.SelectedValue;
            else if (miSchoolId == Constants.SchoolId.SVNP.ToInt() && sParameterName == "{usp_GetAttendanceDetailsForReport_SVNP;1.Year}")
                sFilterValue = sParameterName == "{usp_GetAttendanceDetailsForReport_SVNP;1.Year}" ? oDropDownList.SelectedItem.Text : oDropDownList.SelectedValue;
            else
                sFilterValue = sParameterName == "{usp_MusterReport;1.Year}" ? oDropDownList.SelectedItem.Text : oDropDownList.SelectedValue;
            
            if (sParameterName == "{USP_StudentFinalProgressReport;1.Division_Id}" || sParameterName == "{USP_StudentTerm2ProgressReport;1.Division_Id}")
                hidStandardDivisionId.Value = oDropDownList.SelectedValue;
            if (msReportID == S_IT_RECONCILIATION_RPT_ID && sParameterName == "{USP_ITReconciliation_Statement_Report;1.StudentId}")
                miStudentId = sFilterValue.ToInt();

            if (msReportID == S_INTERNAL_FEE || msReportID == S_PENDING_INTERNAL_FEE || msReportID == S_BANK_CHALLAN_REPORT)
                sFilterValue = sFilterValue.Replace("(", "#").Replace(")", "^");

            if (sFilterValue != "0")
                sParameterFilterString = string.Format("({0}={1})@", sParameterName, sFilterValue);
            //when division_id is null, in this special case is handled.
            if (sFilterValue == Constants.I_ZERO.ToString() && msType != "View")
            {
                if (msReportID == S_SUBJECT_TOPPERS || msReportID==S_TESTWISE_SUBJECT_TOPPERS)
                {
                    if (hidStandardwise.Value == "Y")
                    {
                        var oDDList2 = grdDisplayParameter.Rows[aiGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
                        string sParamName = grdDisplayParameter.DataKeys[aiGridRowCount][I_DISPLAY_NAME_INDEX].ToString();
                        if (sParamName == "Division" && oDDList2.SelectedItem.Text.Equals(S_ALL))
                            sParameterFilterString = string.Format("({0}=-1) @", sParameterName);
                        else if (sParamName == S_EXAM)
                            sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                    }
                    else
                        sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                }
                else if (msReportID == S_EXAM_RESULT_SS || msReportID == S_EXAM_RESULT_STSS_9STD || msReportID == S_EXAM_RESULT_STSS_10STD || msReportID == S_STUD_FINAL_RESULT_SS || msReportID == S_EXAM_RESULT || msReportID == S_STUD_FINAL_RESULT || msReportID == S_STUD_FINAL_RESULT_PPSH || msReportID == S_STUD_FINAL_RESULT_PPSH_Old || msReportID == S_STUD_FINAL_RESULT_FOR_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_9 || msReportID == S_STUD_FINAL_RESULT_FOR_11 || msReportID == S_STUD_FINAL_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_MCPS || msReportID == S_STUD_TERM2_RESULT || msReportID == S_STUD_TERM1_RESULT || msReportID == S_STUD_TERMWISE_RESULT || msReportID == S_EXAM_RESULT_FBS || msReportID == S_EXAM_RESULT_PPSN || msReportID == S_STUD_PRELIMINARY_RESULT || msReportID == S_STUD_EXAM_RESULT_PPSN || msReportID == S_PAY_SCALE_STATEMENT || msReportID == S_COSCHOLASTIC_SUBJECT_MARK_DETAILS)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if (mlstUserAccessPayrollReports.Contains(msReportID) && (hidHasFullAccess.Value != Constants.S_ONE && moUserRole != Constants.UserRoles.Admin) && sParameterName.Contains("StaffGroupsId"))
                    sParameterFilterString = string.Format("({0}= -1 ) @", sParameterName);
                else if (msReportID == S_STAFF_SCREEN_ACCESS_DETAILS && (sParameterName.Contains("ScreenId") || sParameterName.Contains("ReportId") || sParameterName.Contains("UserRoleId") || sParameterName.Contains("UserId")) && sFilterValue == Constants.S_ZERO)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if(msReportID == S_PARENT_IDENTITY_CARDS)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if (msReportID == S_MARK_ENTRY_STATUS || msReportID == S_STUDENT_TRANSFER_DETAILS || msReportID == S_EXAM_CONFIG_DETAILS || msReportID == S_GRADUTY_REPORT_DETAILS || msReportID == S_STUDENT_REGISTRATION_DEATILS || msReportID == S_USERWISE_LOGIN_DURATION_DETAILS)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if (msReportID == S_STOPWISE_STUDENT_PENDING_FEE)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if(msReportID == S_TESTWISE_SUBJECT_MARKS)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if(msReportID == S_EXTERNAL_STUDENT_FEE_DETAILS)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if(msReportID == S_STUDENT_REFUND_FEE_DETAILS)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if (msReportID == S_STUDENT_PROGRESS_REPORT_CBSE || msReportID == S_HOUSEWISE_STUDENT_DETAILS || msReportID == S_STUDENT_EXCESS_FEE_DETAILS || msReportID == STUDENT_DOCUMNET_STATUS_DETAILS || msReportID == S_MONTHLY_FEE_COLLECTION_DETAILS || msReportID == S_STUDENT_PAID_FEE_DETAILS || msReportID == S_EXPORT_FEE_DETAILS || msReportID == S_PERIODIC_TEST_MARK_DETAILS || msReportID == S_STUDENTS_FEE_DETAILS_REPORT || msReportID == S_STUDENT_SA_ONE_REPORT_1stTO4th || msReportID == S_STUDENT_SA_ONE_REPORT_5thTO8th || msReportID == S_STUDENT_OBSERVATION_REPORT)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else if(msReportID == S_STAFF_KID_FEE)
                    sParameterFilterString = string.Format("({0}=0) @", sParameterName);
                else
                    sParameterFilterString = string.Format("({0}= null ) @", sParameterName);
            }
        }
        return sParameterFilterString;
    }

    /// <summary>
    /// This method returns the filter criteria of the TextBox control.
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    /// <returns> </returns>
    private string CreateTextBoxFilterString(int aiGridRowCount)
    {
        string sParameterFilterString = string.Empty;
        string sParameterName = grdDisplayParameter.DataKeys[aiGridRowCount][I_FIELD_NAME_INDEX].ToString();
        var oTextBox = grdDisplayParameter.Rows[aiGridRowCount].FindControl("txtRptParameter") as TextBox;
        string sFilterValue = oTextBox.Text;
        hidRegNo.Value = sFilterValue;
        msEnrollmentNumber = sFilterValue;
        if (msReportID == S_LC_REPORT_ID || msReportID == S_EXAM_PERFORMANCE_REPORT_ID || msReportID == S_TRANSFER_CERTIFICATE || msReportID == S_LEAVING_CERTIFICATE_10TH_NPS_ID)
        {
            // Here we check  for given enrollment number is correct or not.
            if (StudentBL.CheckIsEnrollmentNumber(sFilterValue, miSchoolId))
            {
                // Here we check that entered enrollment number Of student is left the school or
                // his LC is created or not. If LC is created then only report is generated otherwise 
                // it gives proper message.
                if (StudentBL.CheckIsStudentLeaveSchool(sFilterValue, miSchoolId) || msReportID == S_STUD_DETAIL_REPORT_ID || msReportID == S_EXAM_PERFORMANCE_REPORT_ID)
                {
                    if (msReportID == S_LC_REPORT_ID || msReportID == S_STUD_DETAIL_REPORT_ID || msReportID == S_TRANSFER_CERTIFICATE || msReportID == S_LEAVING_CERTIFICATE_10TH_NPS_ID)
                        sParameterFilterString = string.Format("{0} ={1}@", sParameterName, sFilterValue);
                    else
                        sParameterFilterString = string.Format("{0} ='{1}'@", sParameterName, sFilterValue);

                    lblNorecord.Text = string.Empty;
                }
                else if (msReportID == S_LC_REPORT_ID || msReportID == S_LEAVING_CERTIFICATE_10TH_NPS_ID)
                {
                    lblNorecord.Visible = true;
                    lblNorecord.Text = S_LC_NOT_AVAILABLE_MSG;
                }
                else if (msReportID == S_TRANSFER_CERTIFICATE)
                {
                    lblNorecord.Visible = true;
                    lblNorecord.Text = S_TC_NOT_AVAILABLE_MSG;
                }
            }
            else
            {
                lblNorecord.Visible = true;
                lblNorecord.Text = S_ERR_MSG;
                if (msReportID == S_EXAM_PERFORMANCE_REPORT_ID)
                    sParameterFilterString = string.Format("{0} ='{1}'@", sParameterName, sFilterValue);
            }
        }
        else if (sFilterValue == string.Empty)
            sParameterFilterString = string.Format("{0}={1}@", sParameterName, string.Empty);
        else
        {
            sParameterFilterString = string.Format("{0}={1} @", sParameterName, sFilterValue);
            lblNorecord.Text = string.Empty;

            if (msReportID == S_ENROLLMENTWISE_STUDENT_I_CARDS)
            {
                if (miSchoolId != Constants.SchoolId.BFS.ToInt())
                {
                    sParameterFilterString = sParameterFilterString + "{usp_GetStudentIdentityCardDetails_Report;1.Student_Id}=null @{usp_GetStudentIdentityCardDetails_Report;1.Standard_Id}=null @{usp_GetStudentIdentityCardDetails_Report;1.Division_Id}=null @{usp_GetStudentIdentityCardDetails_Report;1.StudentsWithoutPhoto}=1 @";
                }   
            }

            if (msReportID == S_ENROLLMENTWISE_STUDENT_AUTHORITY_CARDS)
            {
                sParameterFilterString = sParameterFilterString + "{usp_GetStudentAuthorityCardDetails_Report;1.Student_Id}=null @{usp_GetStudentAuthorityCardDetails_Report;1.Standard_Id}=null @{usp_GetStudentAuthorityCardDetails_Report;1.Division_Id}=null @{usp_GetStudentAuthorityCardDetails_Report;1.StudentsWithoutPhoto}=1 @";
            }
            
        }

        return sParameterFilterString;
    }

    /// <summary>
    /// 	This method is used to generate xml of filters for deiplyaing UserTask report.
    /// </summary>
    /// <returns> </returns>
    private string GenerateXML()
    {
        var cmbAssigned = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
        var cmbDesignation = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
        var cmbUser = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
        var cmbTaskType = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;
        var cmbTaskStatus = grdDisplayParameter.Rows[4].FindControl("DDLRptParameter") as ComboRpt;

        const string S_ELEMENT = "element";
        var oDoc = new XmlDocument();
        XmlElement oRoot = oDoc.CreateElement("Tasks");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "Tasks", "");

        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "Tasks", "");

        XmlAttribute oAttr = oDoc.CreateAttribute("Flag");

        oAttr.Value = cmbAssigned.SelectedValue != Constants.I_ZERO.ToString() ? cmbAssigned.SelectedValue : Constants.I_THREE.ToString();
        oXmlNode.Attributes.Append(oAttr);

        if (cmbDesignation.SelectedValue != Constants.I_ZERO.ToString())
        {
            oAttr = oDoc.CreateAttribute("DesignationId");
            oAttr.Value = cmbDesignation.SelectedValue;
            oXmlNode.Attributes.Append(oAttr);
        }

        if (cmbUser.SelectedValue != Constants.I_ZERO.ToString())
        {
            oAttr = oDoc.CreateAttribute("UserId");
            oAttr.Value = cmbUser.SelectedValue;
            oXmlNode.Attributes.Append(oAttr);
        }

        if (cmbTaskType.SelectedValue != Constants.I_ZERO.ToString())
        {
            oAttr = oDoc.CreateAttribute("TaskTypeId");
            oAttr.Value = cmbTaskType.SelectedValue;
            oXmlNode.Attributes.Append(oAttr);
        }
        if (cmbTaskStatus.SelectedValue != Constants.I_ZERO.ToString())
        {
            oAttr = oDoc.CreateAttribute("TaskStatusId");
            oAttr.Value = cmbTaskStatus.SelectedValue;
            oXmlNode.Attributes.Append(oAttr);
        }

        oAttr = oDoc.CreateAttribute("OwnerUserId");
        oAttr.Value = miUserId.ToString();
        oXmlNode.Attributes.Append(oAttr);

        oXmlRootNode.AppendChild(oXmlNode);
        oRoot.AppendChild(oXmlRootNode);
        return oRoot.InnerXml;
    }

    /// <summary>
    /// 	This method is used to generate xml of filters for deiplyaing UserTask report.
    /// </summary>
    /// <returns> </returns>
    private Dictionary<string, string> GenerateDictionary(int aiSchoolId, int aiAcademicYearId, string asSearchFilter)
    {
        var dictIssueBookSearchFilters = new Dictionary<string, string>();

        var cmbUserRole = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt; //UserRole
        var cmbStandard = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt; //Standard
        var cmbDivision = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt; //Division
        var cmbUser = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt; //User

        dictIssueBookSearchFilters.Add("School_Id", aiSchoolId.ToString());
        dictIssueBookSearchFilters.Add("Academic_Year_Id", aiAcademicYearId.ToString());
        //UserRole
        if (cmbUserRole.SelectedValue != Constants.I_ZERO.ToString())
            dictIssueBookSearchFilters.Add("UserRoleId", cmbUserRole.SelectedValue);
        //Standard
        if (cmbStandard.SelectedValue != Constants.I_ZERO.ToString()) //Standard
            dictIssueBookSearchFilters.Add("Standard_Id", cmbStandard.SelectedValue);
        //Division
        if (cmbDivision.SelectedValue != Constants.I_ZERO.ToString()) //Division
            dictIssueBookSearchFilters.Add("Division_Id", cmbDivision.SelectedValue);
        //User
        if (cmbUser.SelectedValue != Constants.I_ZERO.ToString()) //User
            dictIssueBookSearchFilters.Add("User_Id", cmbUser.SelectedValue);
        //BookNo/Name
        if (txtName.Text != string.Empty) //BookNo Or Name
            dictIssueBookSearchFilters.Add("BookNoOrName", txtName.Text.Trim());

        return dictIssueBookSearchFilters;
    }

    /// <summary>
    /// 	This method is used to set label name as per the selection of radio button.
    /// </summary>
    private void SetLabelText()
    {
        trBookSearch.Visible = true;
        lblName.Text = optSearchByBook.Checked ? "Book Name :" : "User Name :";
        if (!optSearchByBook.Checked)
            ((TextBox)grdDisplayParameter.Rows[4].FindControl("txtRptParameter")).Text = string.Empty;
        ((TextBox)grdDisplayParameter.Rows[4].FindControl("txtRptParameter")).Enabled = optSearchByBook.Checked;
        tdName.Width = "80px";
    }

    #endregion

    #region Parameters

    /// <summary>
    /// 	This method fills grid with the controls to display parameter.
    /// </summary>
    private void FillGridWithReportParameters()
    {
        DataSet oDataSet = ReportsBL.LoadReportsDataset(msReportID);
        if (oDataSet.Tables[0].Rows.Count > 0)
        {
            grdDisplayParameter.DataSource = oDataSet;
            grdDisplayParameter.DataBind();
            hidSchemaName.Value = grdDisplayParameter.DataKeys[Constants.I_ZERO]["SchemaName"].ToString();
            if (!string.IsNullOrEmpty(hidSchemaName.Value))
                hidSchemaName.Value += ".";
            //In this method one by one control is added to grid.
            FillGridWithControls();
            //This method is for setting required field validator and label of mandatory field to appropriate control.
            SetReqdFieldValidators();
        }
    }

    /// <summary>
    /// 	This method is used to fill parameters name into arraylist for usp_MusterReport stored procedure.
    /// </summary>
    /// <param name="aoArrParametersvalue"> </param>
    /// <returns> Hashtable </returns>
    private Hashtable FillParameterName(ArrayList aoArrParametersvalue)
    {
        var oHashFilterparameters = new Hashtable();
        oHashFilterparameters.Add("Academic_Year_Id", aoArrParametersvalue[0].ToString());
        oHashFilterparameters.Add("School_Id", aoArrParametersvalue[1].ToString());
        oHashFilterparameters.Add("Standard_Id", aoArrParametersvalue[2].ToString());
        oHashFilterparameters.Add("Division_id", aoArrParametersvalue[3].ToString());
        oHashFilterparameters.Add("Month_id", aoArrParametersvalue[4].ToString());
        oHashFilterparameters.Add("Year", aoArrParametersvalue[5].ToString());
        return oHashFilterparameters;
    }

    /// <summary>
    /// 	This method returns dataset for filter parameters.
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    /// <param name="aoHashFilterParameters"> </param>
    /// <returns> DataSet </returns>
    private DataSet RetriveReportParameters(int aiGridRowCount, Hashtable aoHashFilterParameters)
    {
        DataSet oDSFilterParameters = null;
        string sViewName = grdDisplayParameter.DataKeys[aiGridRowCount][I_VIEWNAME_INDEX].ToString();
        if (sViewName != null && sViewName != string.Empty)
        {
            //Conditional execution of the View.
            if (sViewName.StartsWith("vw") || sViewName.ToLower().StartsWith(hidSchemaName.Value.ToLower() + "vw") || sViewName.Contains(".vw_"))
            {
                var iParameterFilters = (Constants.ReportParameterFilters)Convert.ToInt32(grdDisplayParameter.DataKeys[aiGridRowCount][I_PARAMETER_FILTER]);
                string sParameterOrderByField = grdDisplayParameter.DataKeys[aiGridRowCount][I_PARAM_ORDER_BY_FLD].ToString();
                switch (iParameterFilters)
                {
                    //Screen report filter with School ID,Academic Year ID filter.
                    case Constants.ReportParameterFilters.SchoolAcademicYrId:
                        aoHashFilterParameters.Add("School_Id", miSchoolId);
                        if (msReportID != S_BANK_CHALLAN_REPORT || msReportID != S_STUDENT_EXCESS_FEE_DETAILS || msReportID != S_TEACHER_UDISE_DETAILS || msReportID != S_UDISE_DETAILS)
                            aoHashFilterParameters.Add("academic_Year_Id", miAcademicYearId);
                        break;
                    //Screen report filter with School ID filter.
                    case Constants.ReportParameterFilters.SchoolId:
                        aoHashFilterParameters.Add("School_Id", miSchoolId);
                        break;
                }
                oDSFilterParameters = ReportsBL.RetrieveReportParameters(sViewName, sParameterOrderByField, aoHashFilterParameters);
            }
            //When object is a stored procedure.
            else
            {
                if (msReportID == S_PROFESSIONAL_TAX_DETAILS || msReportID == S_PROFESSIONAL_TAX_CHALLAN || msReportID == S_INVESTMENT_DECLARATIONS || msReportID == S_STUDENT_NOT_SELECTED_IN_LOTTERY || msReportID == S_IT_RECONCILIATION_RPT_ID || msReportID == S_STAFF_LEAVES || msReportID == S_EMPLOYEE_DETAILS || msReportID == S_EMPLOYEE_INFORMATION_FOR_REPORT || msReportID == S_REQUISITION_DETAILS || msReportID == S_USER_ROLEWISE_IDENTITY_CARDS || msReportID == S_USER_ROLEWISE_IDENTITY_CARDS_NEW || msReportID == S_TASK_DETAILS || msReportID == S_LEAVE_BALANCE || msReportID == S_STOPWISE_TRANSPORT_DETAILS || msReportID == S_USERROLEWISE_TRAVELLER_DETAILS || msReportID == S_SERVICE_TYPE_DETAILS || msReportID == S_USERROLEWISE_BOOK_ISSUED_USERS || msReportID == S_FEE_PAID_STUDENT_COUNT || msReportID == S_MUSTER_REPORT || msReportID == S_LECTUREWISE_STUDENT_ATTENDANCE || molstPayrollDateReports.Contains(msReportID)
                    || msReportID == S_PERFORMANCE_EVALUATION || msReportID == S_STUDENT_IDENTITY_CARDS || msReportID == S_AGE_CALCULATION  || msReportID == S_SERVEY_ANALYSIS_COUNT_REPORT || msReportID == S_STUDENT_FEE_REPORT
                    || msReportID == S_BANK_CHALLAN_REPORT || msReportID == S_DATEWISE_Fee_COLLECTION || msReportID == S_STUDENT_GENERAL_REGISTER_REPORT || msReportID == S_AREAWISE_PENDINGFEE_DETAILS || msReportID == S_STUDENT_PENDING_FFE_DETAILS || msReportID == S_MATERIAL_ISSUE_DETAILS || msReportID == S_ITEMWISE_STOCK_DETAILS || msReportID == S_STANDARDWISE_TEST_DETAILS || msReportID == S_STAFF_SCREEN_ACCESS_DETAILS || msReportID == S_PARENT_IDENTITY_CARDS || msReportID == S_TEACHER_UDISE_DETAILS || msReportID == S_UDISE_DETAILS || msReportID == S_MARK_ENTRY_STATUS || msReportID == S_MARK_ENTRY_FORM_REPORT || (miSchoolId == Constants.SchoolId.SS.ToInt() && msReportID == S_DATEWISE_ATTENDANCE_COUNT) || msReportID == S_STUDENT_HEALTH_DETAILS || msReportID == S_GRADUTY_REPORT_DETAILS || msReportID == S_STUDENT_REGISTRATION_DEATILS || msReportID == S_EXTERNAL_STUDENT_FEE_DETAILS || msReportID == S_USERWISE_LOGIN_DURATION_DETAILS || msReportID == S_STUDENT_REFUND_FEE_DETAILS || msReportID == S_PAY_SCALE_STATEMENT || msReportID == S_HOUSEWISE_STUDENT_DETAILS || msReportID == STUDENT_DOCUMNET_STATUS_DETAILS || msReportID == S_MONTHLY_FEE_COLLECTION_DETAILS || msReportID == S_EXPORT_FEE_DETAILS || msReportID == S_STAFF_BIRTHDAY_LIST || msReportID == S_LAST_ACADEMICYEAR_FEE_DETAILS || msReportID == S_CATEGORYWISE_ITEM_BARCODE || msReportID == S_EMPLOYEE_INFORMATION_DETAILS || (msReportID == S_TEACHER_JOINING_DATE) || (msReportID == S_USER_RETIREMENT_DETAILS_REPORT) || msReportID == S_STAFF_LEAVE_DETAILS_EXPORT || msReportID == S_USER_SALARY_DETAILS || msReportID == S_MATERIAL_ISSUE_DETAILS_BY_USER || msReportID == S_NEXT_YEAR_PAID_FEE
                    || msReportID == S_ANNUAL_INCREMENT_LETTER || msReportID == S_INAUGURAL_CERTIFICATE || msReportID == S_PENDING_FEE_STATEMENT_FOR_ALL_ACADEMICS_PPSN || msReportID == S_EMPLOYMENT_CONFIRMATION_LETTER || msReportID == S_EMPLOYMENT_CONFIRMATION_LETTER_IN_DETAILS || msReportID == S_PARENT_OCCUPATION_DETAILS || msReportID == S_USER_PAYROLL_DETAILS || msReportID == S_USER_PAYROLL_SALARY_DETAILS || msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_MNS || msReportID == S_STUDENT_BONAFIDE_CERTIFICATE_VPMCPS || msReportID == S_STUDENT_FEE_CONSOLIDATED_DETAILS || msReportID == S_ANNUAL_CONSOLDATED_SUB_TYPE_REPORT_SNS || msReportID == S_CLASSWISE_EXAM_PERFORMANCE || msReportID == S_TEST_CONSOLIDATED_REPORT || msReportID == S_EXAMWISE_MARK_DETAILS || msReportID == S_EXPORT_STUDENT_MONTHLY_STATUS || msReportID == S_EXPORT_STUDENTS_RECEIPTS_DETAILS || msReportID == S_CA_RECONSOLIDATION_DETAILS || msReportID==S_STUDENT_NEW_IDENTITY_CARDS)
                {
                    if (msReportID == S_TASK_DETAILS && aiGridRowCount == 1)
                        aoHashFilterParameters.Add("UserId", miUserId);

                    if (msReportID == S_SALARY_SLIP && aiGridRowCount == 2)
                        aoHashFilterParameters.Add("LoginUserId", miUserId);

                    var iParameterFilters = (Constants.ReportParameterFilters)Convert.ToInt32(grdDisplayParameter.DataKeys[aiGridRowCount][I_PARAMETER_FILTER]);
                    switch (iParameterFilters)
                    {
                        //Screen report filter with School ID,Academic Year ID filter.
                        case Constants.ReportParameterFilters.SchoolAcademicYrId:
                            aoHashFilterParameters.Add("School_Id", miSchoolId);
                            aoHashFilterParameters.Add("academic_Year_Id", miAcademicYearId);
                            break;
                        //Screen report filter with School ID filter.
                        case Constants.ReportParameterFilters.SchoolId:
                            aoHashFilterParameters.Add("School_Id", miSchoolId);
                            break;
                    }
                    if (moDictFiledDatatype.Count > 0)
                        oDSFilterParameters = ReportsBL.RetrieveReportParameters(sViewName, aoHashFilterParameters, moDictFiledDatatype);
                    else
                        oDSFilterParameters = ReportsBL.RetrieveReportParameters(sViewName, aoHashFilterParameters);
                }
                else
                    oDSFilterParameters = ReportsBL.RetrieveReportParameters(sViewName);
            }
            aoHashFilterParameters.Clear();
        }
        return oDSFilterParameters;
    }

    /// <summary>
    /// 	This method gives dataset for filter parameters.
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    /// <returns> DataSet </returns>
    private DataSet RetriveReportParameters(int aiGridRowCount)
    {
        DataSet oDSFilterParameters = null;
        string sViewName = grdDisplayParameter.DataKeys[aiGridRowCount][I_VIEWNAME_INDEX].ToString().Trim();
        if (sViewName != null && sViewName != string.Empty)
        {
            //Conditional execution of the View.
            var oHashFilterParameters = new Hashtable();
            if (sViewName.StartsWith("vw") || sViewName.ToLower().StartsWith(hidSchemaName.Value.ToLower() + "vw") || sViewName.Contains(".vw_"))
            {
                var iParameterFilters = (Constants.ReportParameterFilters)Convert.ToInt32(grdDisplayParameter.DataKeys[aiGridRowCount][I_PARAMETER_FILTER]);
                string sParameterOrderByField = grdDisplayParameter.DataKeys[aiGridRowCount][I_PARAM_ORDER_BY_FLD].ToString();
                switch (iParameterFilters)
                {
                    //Screen report filter with School ID,Academic Year ID filter.
                    case Constants.ReportParameterFilters.SchoolAcademicYrId:
                        oHashFilterParameters.Add("School_Id", miSchoolId);                       
                        oHashFilterParameters.Add("academic_Year_Id", miAcademicYearId);
                        break;
                    //Screen report filter with School ID filter.
                    case Constants.ReportParameterFilters.SchoolId:
                        oHashFilterParameters.Add("School_Id", miSchoolId);
                        break;
                }
                oDSFilterParameters = ReportsBL.RetrieveReportParameters(sViewName, sParameterOrderByField, oHashFilterParameters);
            }
            //When view is a stored procedure.
            else
            {
                var iParameterFilters = (Constants.ReportParameterFilters)Convert.ToInt32(grdDisplayParameter.DataKeys[aiGridRowCount][I_PARAMETER_FILTER]);
                switch (iParameterFilters)
                {
                    //Screen report filter with School ID,Academic Year ID filter.
                    case Constants.ReportParameterFilters.SchoolAcademicYrId:
                        oHashFilterParameters.Add("School_Id", miSchoolId);
                        oHashFilterParameters.Add("academic_Year_Id", miAcademicYearId);
                        break;
                    //Screen report filter with School ID filter.
                    case Constants.ReportParameterFilters.SchoolId:
                        oHashFilterParameters.Add("School_Id", miSchoolId);
                        break;
                }
                if (msReportID == S_EXAM_RESULT_SS || msReportID == S_EXAM_RESULT_STSS_9STD || msReportID == S_EXAM_RESULT_STSS_10STD || msReportID == S_STUD_FINAL_RESULT_SS || msReportID == S_STUD_FINAL_RESULT_PPSH || msReportID == S_STUD_FINAL_RESULT_SNS_6TO8_Std || msReportID == S_STUD_FINAL_RESULT_PPSH_Old || msReportID == S_STUD_FINAL_RESULT_FOR_PPSN || msReportID == S_STUD_FINAL_RESULT_FOR_9 || msReportID == S_STUD_FINAL_RESULT_FOR_11 || msReportID == S_EXAM_RESULT || msReportID == S_STUD_TERM1_RESULT || msReportID == S_STUD_TERMWISE_RESULT || msReportID == S_STUD_TERM2_RESULT || msReportID == S_EXAM_RESULT_FBS || msReportID == S_EXAM_RESULT_PPSN || msReportID == S_STUD_PRELIMINARY_RESULT
                    || msReportID == S_FINAL_REPORT_JPS || msReportID == S_FINAL_REPORT_GSS || msReportID == S_FINAL_REPORT_PKJC || msReportID == S_STUD_EXAM_RESULT_PPSN || msReportID == S_PRE_PRIMARY_REPORT_JOS || msReportID == S_STANDARDWISE_TEST_DETAILS || msReportID == S_ANNUAL_CONSOLDATED_REPORT_SPS9 || msReportID == S_ANNUAL_CONSOLDATED_REPORT_SPS11 || msReportID == S_STUD_EXAM_RESULT_MVPS_9 || msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_PEMS || msReportID == S_PRELIM_RESULT_SHEET || msReportID == S_STUDENT_PROGRESS_REPORT_CBSE || msReportID == S_COSCHOLASTIC_SUBJECT_MARK_DETAILS || msReportID == S_PERIODIC_TEST_MARK_DETAILS || msReportID == S_STUDENT_OBSERVATION_REPORT || msReportID == S_STUDENT_TERM1_PROGRESS_REPORT || msReportID == S_STUDENT_TERM1_PROGRESS_REPORT_PPSN || msReportID == S_PRELIM_REPORT_PP || msReportID == S_STUDENT_FINAL_PROGRESS_REPORT_MNS || msReportID == S_FINAL_PROGRESS_CARD_SNS_11_12 || msReportID == S_HOLISTIC_FINAL_PROGRESS_CARD || msReportID == S_TERM_PROGRESS_REPORT_PIONEER || msReportID == S_EXAMWISE_MARK_DETAILS || msReportID == S_PREPRIMARY_STUDENT_TERM1 || msReportID == S_HOLISTIC_REPORT_FOR1TO3_PPSH || msReportID == S_STUDENT_HALF_YEARLY_3TO9 || msReportID == S_Holistic_Progress_Report_6to7_SNS)
                    oHashFilterParameters.Add("ReportId", msReportID.ToInt());

                if (msReportID == S_SALARY_SLIP && aiGridRowCount == 2)
                    oHashFilterParameters.Add("LoginUserId", miUserId);

                oDSFilterParameters = ReportsBL.RetrieveReportParameters(sViewName, oHashFilterParameters);
            }
        }
        return oDSFilterParameters;
    }

    /// <summary>
    /// 	This method is used set parameters such as school name, organisation name and academic year to report.
    /// </summary>
    private void ApplyParametersToCrystalReport(string asReportSelectionString)
    {
        string sParameterValue;
        string sParameterField;
        string sSubReportName;
        int iSubreportCount = 0;

        var kvp = new Dictionary<string, string>();
        ParameterFieldDefinition oParameterFieldDefinition;
        ParameterFieldDefinitions ApplyParameterFieldDefinations = crReportDocument.DataDefinition.ParameterFields;
        var ApplyParameterDiscreteValue = new ParameterDiscreteValue();
        var ApplyParameterValue = new CrystalDecisions.Shared.ParameterValues();
        asReportSelectionString = FormatFilterString(asReportSelectionString);
        String[] sFilters = asReportSelectionString.Split('@');

        if (msReportID == S_SUBJECT_TOPPERS || msReportID == S_EXAM_RESULT || msReportID == S_EXAM_RESULT_FBS || msReportID == S_EXAM_RESULT_PPSN || msReportID==S_TESTWISE_SUBJECT_TOPPERS)
            iSubreportCount = 1;
        if (msReportID == S_STUD_TERM2_RESULT || msReportID == S_STUD_TERM1_RESULT || msReportID == S_STUD_TERMWISE_RESULT || msReportID == S_STUD_PRELIMINARY_RESULT)
            iSubreportCount = 2;
        for (int index = 0; index <= iSubreportCount; index++)
        {
            sSubReportName = GetSubReportName(index);
            if (msType != "View" && msType != hidSchemaName.Value + "view")
            {
                foreach (string filter in sFilters)
                {
                    if (filter.Equals(string.Empty))
                        continue;
                    sParameterValue = filter.Substring(filter.IndexOf("=") + 1);
                    if (!filter.Contains(hidSchemaName.Value) || hidSchemaName.Value.Trim() == string.Empty)
                        sParameterField = filter.Substring(filter.IndexOf(".") + 1, filter.LastIndexOf("=") - filter.IndexOf(".") - 1).Trim();
                    else
                        sParameterField = filter.Substring(filter.LastIndexOf(".") + 1, filter.IndexOf("=") - filter.LastIndexOf(".") - 1).Trim();

                    if (msReportID == S_PENDING_FEE_DETAILS)
                    {
                        if (sParameterField == "Amount" && sParameterValue == "")
                            sParameterValue = "0";
                    }
                    else if (msReportID == S_CA_RECONSOLIDATION_DETAILS)
                    {
                        if (sParameterValue == "")
                            sParameterValue = "null";
                    }

                    if (msReportID == "22" || msReportID == "21" || msReportID == "182" || msReportID=="269")
                        oParameterFieldDefinition = ApplyParameterFieldDefinations["@" + sParameterField];
                    else
						oParameterFieldDefinition = ApplyParameterFieldDefinations[sParameterField];

                    if (sParameterValue.Trim() == "null")
                    {
                        ApplyParameterDiscreteValue.Value = null;
                        if (msReportID == "22" || msReportID == "21" || msReportID == "182" || msReportID == "269")
                            crReportDocument.SetParameterValue("@" + sParameterField, null);
                        else if (msReportID == S_STAFF_KID_FEE || msReportID == "278" || msReportID=="280" )
                            crReportDocument.SetParameterValue(sParameterField, "0");
                        else
                            crReportDocument.SetParameterValue(sParameterField, null);
                    }
                    else
                    {
                        ApplyParameterDiscreteValue.Value = sParameterValue.Trim();
                        if (sSubReportName == string.Empty)
                        {
                            switch (msReportID)
                            {
                                case "21":
                                case "22":
                                case "182" : 
                                case "269":
                                    crReportDocument.SetParameterValue("@" + sParameterField, sParameterValue);
                                    break;
                                case S_STUDENT_ANNUAL_ATTENDANCE:
                                    if (string.IsNullOrEmpty(sParameterValue.Trim()))
                                    {
                                        sParameterValue = "101";
                                        ApplyParameterDiscreteValue.Value = "101";
                                    }
                                    crReportDocument.SetParameterValue(sParameterField, sParameterValue);
                                    break;                                    
                                default:
									crReportDocument.SetParameterValue(sParameterField, sParameterValue);
                                    break;
                            }
                        }
                        else
                            crReportDocument.SetParameterValue(sParameterField, sParameterValue, sSubReportName);
                    }
                    ApplyParameterValue.Add(ApplyParameterDiscreteValue);
                    oParameterFieldDefinition.ApplyCurrentValues(ApplyParameterValue);

                    if (msReportID == S_SALARY_SHEET)
                        kvp[sParameterField] = sParameterValue.Trim() == "null" ? "0" : sParameterValue;
                }
            }
            if (msReportID == S_CLASS_TT_REPORT_ID || msReportID == S_TEACHER_TT_REPORT_ID || msReportID == S_SCHOOL_TT_REPORT_ID || msReportID == S_FREE_TEACHER_LIST_REPORT_ID || msReportID == S_TEACHER_REPLACEMENT_LIST_REPORT_ID || msReportID == S_DAILY_TEACHER_LECTCNT_REPORT_ID)
            {
                ApplyParameterDiscreteValue = CheckIsMPTApplicable(ApplyParameterDiscreteValue);
                ApplyParameterDiscreteValue = CheckIsAssemblyApplicable(ApplyParameterDiscreteValue);
                if (msReportID == S_CLASS_TT_REPORT_ID || msReportID == S_TEACHER_TT_REPORT_ID || msReportID == S_SCHOOL_TT_REPORT_ID)
                    ApplyParameterDiscreteValue = CheckIsStayBackApplicable(ApplyParameterDiscreteValue);
            }

            if (kvp.Count <= 0)
                continue;
            var oSalaryDetailsBL = new SalaryDetailsBL(miSchoolId, miAcademicYearId);
            oSalaryDetailsBL.SetSalaryDetails(kvp);
        }
        oParameterFieldDefinition = AddReportParameters(ApplyParameterFieldDefinations, ApplyParameterDiscreteValue, ApplyParameterValue);
    }

    /// <summary>
    /// 	This method is used to apply parameters by checking flag of assembly.
    /// </summary>
    /// <param name="ApplyParameterDiscreteValue"> </param>
    /// <returns> </returns>
    private ParameterDiscreteValue CheckIsAssemblyApplicable(ParameterDiscreteValue ApplyParameterDiscreteValue)
    {
        if (Settings.IsAssemblyApplicable)
        {
            string sAssemblyWeekday = Settings.AssemblyWeekday;
            string sAssemblyLectNo = Settings.AssemblyLectNo.ToString();
            string sAssemblyName = Settings.AssemblyName;

            ApplyParameterDiscreteValue.Value = sAssemblyWeekday;
            crReportDocument.SetParameterValue(S_ASSEMBLY_WEEKDAY, sAssemblyWeekday);

            ApplyParameterDiscreteValue.Value = sAssemblyLectNo;
            crReportDocument.SetParameterValue(S_ASSEMBLY_LECT_NO, sAssemblyLectNo);

            ApplyParameterDiscreteValue.Value = sAssemblyName;
            crReportDocument.SetParameterValue(S_ASSEMBLY_NAME, sAssemblyName);
        }
        else
        {
            ApplyParameterDiscreteValue.Value = null;
            crReportDocument.SetParameterValue(S_ASSEMBLY_WEEKDAY, null);

            ApplyParameterDiscreteValue.Value = null;
            crReportDocument.SetParameterValue(S_ASSEMBLY_LECT_NO, null);

            ApplyParameterDiscreteValue.Value = null;
            crReportDocument.SetParameterValue(S_ASSEMBLY_NAME, null);
        }
        return ApplyParameterDiscreteValue;
    }

    /// <summary>
    /// 	This method is used to apply parameters by checking flag of MPT.
    /// </summary>
    /// <param name="ApplyParameterDiscreteValue"> </param>
    /// <returns> </returns>
    private ParameterDiscreteValue CheckIsMPTApplicable(ParameterDiscreteValue ApplyParameterDiscreteValue)
    {
        if (Settings.IsMPTApplicable)
        {
            string sMPTWeekday = Settings.MPTWeekday;
            string sMPTLectNo = Settings.MPTLectNo.ToString();
            string sMPTName = Settings.MPTName;

            ApplyParameterDiscreteValue.Value = sMPTWeekday;
            crReportDocument.SetParameterValue(S_MPT_WEEKDAY, sMPTWeekday);

            ApplyParameterDiscreteValue.Value = sMPTLectNo;
            crReportDocument.SetParameterValue(S_MPT_LECT_NO, sMPTLectNo);

            ApplyParameterDiscreteValue.Value = sMPTName;
            crReportDocument.SetParameterValue(S_MPT_NAME, sMPTName);
        }
        else
        {
            ApplyParameterDiscreteValue.Value = null;
            crReportDocument.SetParameterValue(S_MPT_WEEKDAY, null);

            ApplyParameterDiscreteValue.Value = null;
            crReportDocument.SetParameterValue(S_MPT_LECT_NO, null);

            ApplyParameterDiscreteValue.Value = null;
            crReportDocument.SetParameterValue(S_MPT_NAME, null);
        }
        return ApplyParameterDiscreteValue;
    }

    /// <summary>
    /// 	This method is used to apply parameters by checking flag of MPT.
    /// </summary>
    /// <param name="ApplyParameterDiscreteValue"> </param>
    /// <returns> </returns>
    private ParameterDiscreteValue CheckIsStayBackApplicable(ParameterDiscreteValue ApplyParameterDiscreteValue)
    {
        string sStaybackApplicable = Settings.IsStaybackApplicable ? Constants.S_YES : Constants.S_NO;

        if (sStaybackApplicable == Constants.C_YES.ToString())
        {
            string sStayBackName = Settings.StaybackName;
            crReportDocument.SetParameterValue(S_SATYBACK_NAME, sStayBackName);
        }
        else
            crReportDocument.SetParameterValue(S_SATYBACK_NAME, null);

        return ApplyParameterDiscreteValue;
    }

    /// <summary>
    /// 	This method adds parameters to each report(School Name , Organization Name, Academic Year).
    /// </summary>
    /// <param name="aParameterFieldDefinations"> </param>
    /// <param name="aApplyParameterDiscreteValue"> </param>
    /// <param name="aApplyParameterValue"> </param>
    /// <returns> ParameterFieldDefinition </returns>
    private ParameterFieldDefinition AddReportParameters(ParameterFieldDefinitions aParameterFieldDefinations, ParameterDiscreteValue aApplyParameterDiscreteValue, CrystalDecisions.Shared.ParameterValues aApplyParameterValue)
    {
        ParameterFieldDefinition oParameterFieldDefinition;
        
        if (msReportID == S_IT_RECONCILIATION_RPT_ID)
        {
            var oStudentFeeDetailsBL = new StudentFeeDetailsBL();
            var cmbFinYear = grdDisplayParameter.Rows[3].FindControl("DDLRptParameter") as ComboRpt;
            int iYear = cmbFinYear.SelectedValue.ToInt();

            var cmbAcademicYear = grdDisplayParameter.Rows[4].FindControl("DDLRptParameter") as ComboRpt;
            int iSelectedAcademicYearId = cmbAcademicYear.SelectedValue.ToInt();

            string sTotalAmt = oStudentFeeDetailsBL.GetTotalAmtForITConciliationRpt(miSchoolId, miAcademicYearId, miStudentId, iYear, iSelectedAcademicYearId);
            if (sTotalAmt.Trim() == string.Empty)
                sTotalAmt = Constants.S_ZERO;
            string strAmount = CommonUtility.GetNumberInWords(sTotalAmt);
            oParameterFieldDefinition = aParameterFieldDefinations["TotalAmount"];
            aApplyParameterDiscreteValue.Value = strAmount;
            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
            crReportDocument.SetParameterValue("TotalAmount", strAmount);
        }

        if (msReportID == S_STUDENT_PHOTOS)
        {
            int iParameter = 0;
            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
                iParameter = 1;
            oParameterFieldDefinition = aParameterFieldDefinations["ShowOnlyReport"];
            aApplyParameterDiscreteValue.Value = iParameter;
            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
            crReportDocument.SetParameterValue("ShowOnlyReport", iParameter);
        }

        switch (msReportID)
        {   
            case S_STAFF_LEAVE_DETAILS_EXPORT:
            case S_SALARY_SHEET:
            case S_LC_REPORT_ID:
            case S_STUD_TERM1_RESULT:
            case S_STUD_TERMWISE_RESULT:
            case S_STUD_PRELIMINARY_RESULT:
            case S_STUDENT_PHOTOS:
            case S_STUD_TERM2_RESULT:
            case S_STUD_FEE_LEDGER:
            case S_STUD_FINAL_RESULT:
          //  case S_STUD_FINAL_RESULT_PPSH:
            case S_STUD_FINAL_RESULT_FOR_PPSN:
            case S_STUD_FINAL_RESULT_FOR_9:
            case S_STUD_FINAL_RESULT_FOR_11:
            case S_STUD_FINAL_RESULT_SNS_6TO8_Std:
            case S_STUD_FINAL_RESULT_PPSN:
            case S_STUD_EXAM_RESULT_PPSN:
            case S_STUD_FINAL_RESULT_MCPS:
            case S_USER_ROLEWISE_IDENTITY_CARDS:
            case S_USER_ROLEWISE_IDENTITY_CARDS_NEW:
            case S_EMPLOYEE_INFORMATION_DETAILS:
            case S_XSEED_REPORT:
            case S_EMPLOYEE_DETAILS:

            case S_IT_RECONCILIATION_RPT_ID:
            case S_EARNINGS_DEDUCTIONS:
            case S_CAUTION_MONEY_DETTAILS:  
            case S_SALARY_LEDGER:
			case S_BONAFIEDREPORTLFS:
            case S_BANK_LETTER:
            case S_FORM_NO_27A:
            case S_NET_SALARY:
            case S_DATEWISE_ATTENDANCE_COUNT:
            case S_NEW_ADMISSION_COUNT:
            case S_PERFORMANCE_EVALUATION:
            case S_SURVEY_ANALYSIS:
            case S_EXPORT_STUDENT_LIST:
            case S_PRE_PRIMARY_REPORT:
            case S_STUDENT_FEE_REPORT:
            case S_FINAL_REPORT_PKJC:
            case S_FINAL_REPORT_JOS :
            case S_FEE_RECEIPT_DETAILS:
            case S_ASSEMBLY_REPORT:
            case S_DATEWISE_Fee_COLLECTION:
            case S_AREAWISE_PENDINGFEE_DETAILS:
            case S_STUDENT_PENDING_FFE_DETAILS:
            case S_STAFF_ATTENDANCE:
            case S_CLASSWISE_WORKING_HOURS:
            case S_PRE_PRIMARY_REPORT_JOS:
            case S_CHARACTER_CERTIFICATE_REPORT_ID:
            case S_INTERNAL_FEE_RECEIPT_DETAILS:
            case S_STUDENT_EXCESS_FEE_DETAILS:
            case S_STUDENT_GENERAL_REGISTER_REPORT:
            case S_TERM_TOPPERS:       
            case S_EMPLOYEE_INFORMATION_FOR_REPORT:
            case S_STUD_FINAL_RESULT_SS:
            case S_BONAFIDE_CERTIFICATE_REPORT_FOR_PPSH_ID:
            case S_STANDARDWISE_TEST_DETAILS:
            case S_STAFF_SCREEN_ACCESS_DETAILS:
            case S_NEXT_YEAR_PAID_FEE:
            case S_STUD_FINAL_RESULT_PPSH_Old:
            case S_STAFF_KID_FEE:
            case S_NOMINAL_ROLL:
            case S_TRANSFER_CERTIFICATE:
            case S_CLASS_CATELOG:
            case S_TEACHER_UDISE_DETAILS:
            case S_UDISE_DETAILS:
            case S_MARK_ENTRY_STATUS:
            case S_MARK_ENTRY_FORM_REPORT:
            case S_TESTWISE_SUBJECT_MARKS:
            case S_STUDENT_HEALTH_DETAILS:
            case S_STUD_EXAM_RESULT_MVPS_9:
            case S_STOPWISE_STUDENT_PENDING_FEE:
            case S_STUD_FINAL_RESULT_PPSH:
            case S_STUDENT_FINAL_PROGRESS_REPORT_PEMS:
            case S_EXAM_CONFIG_DETAILS:
            case S_STUDENT_TRANSFER_DETAILS:
            case S_USER_LOGIN_DETAILS:
            case S_GRADUTY_REPORT_DETAILS:            
            case S_EXTERNAL_STUDENT_FEE_DETAILS:
            case S_STUDENT_REFUND_FEE_DETAILS:
            case S_STUDENT_COUNT_LEARNING_OUTCOME:
            case S_PAY_SCALE_STATEMENT:
            case S_STUDENT_PROGRESS_REPORT_CBSE:
            case S_HOUSEWISE_STUDENT_DETAILS:
            case STUDENT_DOCUMNET_STATUS_DETAILS:
            case S_MONTHLY_FEE_COLLECTION_DETAILS:
            case S_EXPORT_FEE_DETAILS:
            case S_COSCHOLASTIC_SUBJECT_MARK_DETAILS:
            case S_EXAM_RESULT_STSS_10STD:
            case S_PERIODIC_TEST_MARK_DETAILS:
            case S_STAFF_BIRTHDAY_LIST :
            case S_STUDENTS_FEE_DETAILS_REPORT :
            case S_BANK_CHALLAN_REPORT :
            case S_EXAMWISE_REPORT_CARD :
            case S_LAST_ACADEMICYEAR_FEE_DETAILS :
            case S_STUDENT_SA_ONE_REPORT_1stTO4th :
            case S_STUDENT_SA_ONE_REPORT_5thTO8th :
            case S_CATEGORYWISE_ITEM_BARCODE:
            case S_STUDENT_OBSERVATION_REPORT:
            case S_STUDENT_NEWADMISSION_DETAILS_EXPORT:
            case S_STUDENT_ALL_ACADEMICS_PENDING_FEE :
            case S_EXPORT_ADMISSION_DETAILS:
            case S_LEAVING_CERTIFICATE_10TH_NPS_ID:    //
            case S_STUDENT_STREAM_DETAILS : 
            case S_ITEMWISE_STOCK_DETAILS:
            case S_GST_Invoice_Details:
            case S_CLASSWISE_ATTENDANCE_AVERAGE_REPORT:
            case S_CAUTION_MONEY_PAYMENT_DETAILS:
            case S_STUDENT_TERM1_PROGRESS_REPORT:
            case S_MATERIAL_ISSUE_DETAILS_BY_USER:
            case S_STUDENT_TERM1_PROGRESS_REPORT_PPSN:
            case S_ADMISSION_CANCELLATION_FORM:
            case S_BUS_ATTENDANCE:
            case S_TRANSPORT_NOTIFICATIONS:
            case S_ANNUAL_INCREMENT_LETTER:
            case S_CAUTION_MONEY_ADJUSTMENT_AMOUNT:
			case S_INAUGURAL_CERTIFICATE:
            case S_PENDING_FEE_STATEMENT_FOR_ALL_ACADEMICS_PPSN:
            case S_EMPLOYMENT_CONFIRMATION_LETTER:
            case S_EMPLOYMENT_CONFIRMATION_LETTER_IN_DETAILS:
            case S_PARENT_OCCUPATION_DETAILS:
            case S_STUDENT_FINAL_PROGRESS_REPORT_MNS:            
            case S_STUDENT_BONAFIDE_CERTIFICATE_VPMCPS:
            case S_STUDENT_FEE_CONSOLIDATED_DETAILS:
            case S_FEE_RECONCILIATION_REPORT_PPSH:
            case S_FINAL_PROGRESS_CARD_SNS_11_12:
            case S_EXPORT_FEE_DETAILS_SNS:
            case S_TERM_PROGRESS_REPORT_PIONEER:
            case S_EXAMWISE_MARK_DETAILS : 
            case S_EXPORT_STUDENT_MONTHLY_STATUS:
            case S_EXPORT_STUDENTS_RECEIPTS_DETAILS:
            case S_CA_RECONSOLIDATION_DETAILS:
            case S_STUDENT_PENDING_FEE_REMINDER:
            case S_STUDENT_HALF_YEARLY_3TO9:
            case S_VEHICLES_FUEL_MAINTENANCE_EXPENSES:
            case S_STUDENT_NEW_IDENTITY_CARDS:
                oParameterFieldDefinition = null;
                break;
            case S_PROVIDENT_FUND_OF_SALARY_DIFFERENCE:
                oParameterFieldDefinition = aParameterFieldDefinations["IsSubReport"];
                aApplyParameterDiscreteValue.Value = "N";
                aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                break;
            case S_FORM_NO_16:
                oParameterFieldDefinition = aParameterFieldDefinations["IsFormNo16"];
                aApplyParameterDiscreteValue.Value = "1";
                aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                break;
            case S_STUDENT_ADDRESS_REPORT:
                oParameterFieldDefinition = aParameterFieldDefinations["IncludeLeftStudents"];
                var chk = grdDisplayParameter.Rows[3].FindControl("ChkRptParameter") as CheckBoxList;
                aApplyParameterDiscreteValue.Value = (chk.Items[0].Selected ? Constants.S_ONE : Constants.S_ZERO);
                aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                break;
            case S_LEAVE_BALANCE:
                oParameterFieldDefinition = aParameterFieldDefinations["DisplayHeader"];
                aApplyParameterDiscreteValue.Value = "1";
                aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                crReportDocument.SetParameterValue("DisplayHeader", msSchoolName);
                break;
          
            default:
                oParameterFieldDefinition = aParameterFieldDefinations["SchoolName"];
                aApplyParameterDiscreteValue.Value = msSchoolName;
                aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                crReportDocument.SetParameterValue("SchoolName", msSchoolName);
                oParameterFieldDefinition = aParameterFieldDefinations["AcademicYear"];
                aApplyParameterDiscreteValue.Value = msAcademicYearName;
                aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                crReportDocument.SetParameterValue("AcademicYear", msAcademicYearName);
                oParameterFieldDefinition = aParameterFieldDefinations["Organisation Name"];
                aApplyParameterDiscreteValue.Value = msOrgnizationName;
                aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                crReportDocument.SetParameterValue("Organisation Name", msOrgnizationName);
                switch (msReportID)
                {
                    case S_SUBJECT_TOPPERS:
                        {
                            oParameterFieldDefinition = aParameterFieldDefinations["IsTestToppersRequired"];
                            var oCheckBoxList = grdDisplayParameter.Rows[I_TEST_TOPPER_ROW].FindControl("ChkRptParameter") as CheckBoxList;
                            string sSelected = oCheckBoxList.Items[0].Selected ? "Y" : "N";

                            aApplyParameterDiscreteValue.Value = sSelected;

                            aApplyParameterDiscreteValue.Value = "N";
                            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                            crReportDocument.SetParameterValue("IsTestToppersRequired", sSelected);
                        }
                        break;
                    case S_TESTWISE_SUBJECT_TOPPERS:
                        {
                            oParameterFieldDefinition = aParameterFieldDefinations["IsTestToppersRequired"];
                            var oCheckBoxList = grdDisplayParameter.Rows[I_TEST_TOPPER_ROW].FindControl("ChkRptParameter") as CheckBoxList;
                            string sSelected = oCheckBoxList.Items[0].Selected ? "Y" : "N";

                            aApplyParameterDiscreteValue.Value = sSelected;

                            aApplyParameterDiscreteValue.Value = "N";
                            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                            crReportDocument.SetParameterValue("IsTestToppersRequired", sSelected);
                        }
                        break;
                    case S_PENDING_FEE_STUDENTLIST:
                    case S_CLASSWISE_STUDENT_PENDING_FEE_REPORT_ID:
                        {
                            oParameterFieldDefinition = aParameterFieldDefinations["IsIgnoreLeftStudent"];
                            var oCheckBoxList = grdDisplayParameter.Rows[I_TEST_TOPPER_ROW].FindControl("ChkRptParameter") as CheckBoxList;
                            aApplyParameterDiscreteValue.Value = oCheckBoxList.Items[0].Selected ? "Y" : "N";

                            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                            crReportDocument.SetParameterValue("IsIgnoreLeftStudent", aApplyParameterDiscreteValue.Value);

                            oParameterFieldDefinition = aParameterFieldDefinations["IsIgnorePDCStudent"];
                            oCheckBoxList = grdDisplayParameter.Rows[I_TEST_TOPPER_ROW + 1].FindControl("ChkRptParameter") as CheckBoxList;
                            aApplyParameterDiscreteValue.Value = oCheckBoxList.Items[0].Selected ? "Y" : "N";

                            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                            crReportDocument.SetParameterValue("IsIgnorePDCStudent", aApplyParameterDiscreteValue.Value);
                        }
                        break;
                    case S_PENDING_FEE_DETAILS:
                        {
                            oParameterFieldDefinition = aParameterFieldDefinations["IsIgnorePDCStudent"];
                            var oCheckBoxList = grdDisplayParameter.Rows[I_TEST_TOPPER_ROW].FindControl("ChkRptParameter") as CheckBoxList;
                            aApplyParameterDiscreteValue.Value = oCheckBoxList.Items[0].Selected ? "Y" : "N";

                            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                            crReportDocument.SetParameterValue("IsIgnorePDCStudent", aApplyParameterDiscreteValue.Value);
                        }
                        break;
                    case S_ANNUAL_CONSOLDATED_REPORT:
                    case S_ANNUAL_CONSOLDATED_REPORT_SPS9:
                    case S_ANNUAL_CONSOLDATED_REPORT_SPS11:
                    case S_ANNUAL_CONSOLDATED_UNITTEST_REPORT_SVNP:
                        {
                            if (miSchoolId != Constants.SchoolId.SVP.ToInt() && miSchoolId != Constants.SchoolId.SVNP.ToInt())
                            {
                                oParameterFieldDefinition = aParameterFieldDefinations["Showgrade"];
                                CheckBoxList oCheckBoxList = grdDisplayParameter.Rows[2].FindControl("ChkRptParameter") as CheckBoxList;
                                aApplyParameterDiscreteValue.Value = oCheckBoxList.Items[0].Selected ? "Y" : "N";

                                aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                                oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                                crReportDocument.SetParameterValue("Showgrade", aApplyParameterDiscreteValue.Value);
                            }
                        }
                        break;
                    case S_ANNUAL_CONSOLDATED_REPORT_SNS:
                    case S_ANNUAL_CONSOLDATED_SUB_TYPE_REPORT_SNS:
                        {
                            oParameterFieldDefinition = aParameterFieldDefinations["Showgrade"];
                            var oCheckBoxList = grdDisplayParameter.Rows[2].FindControl("ChkRptParameter") as CheckBoxList;
                            aApplyParameterDiscreteValue.Value = Constants.S_NO;

                            aApplyParameterValue.Add(aApplyParameterDiscreteValue);
                            oParameterFieldDefinition.ApplyCurrentValues(aApplyParameterValue);
                            crReportDocument.SetParameterValue("Showgrade", aApplyParameterDiscreteValue.Value);
                        }
                        break;
                }
                break;
        }
        return oParameterFieldDefinition;
    }

    /// <summary>
    /// This method is used to export result sheet details in excel format.
    /// </summary>
    /// <param name="asFilterString"></param>
    private void ExportResultSheet(string asFilterString)
    {   
        int iStandardId = 0, iDivisionId = 0, iTestId = 0,iTermId = 0;
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_GetClasswiseExamDetailsSVP;1.", "").Replace("usp_Annual_Consolidation_Report_SVP;1.", "").Split('@');
        
        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim() == "Standard_Id")
                    iStandardId = oData[1].ToInt();
                else if (oData[0].Trim() == "Division_Id")
                    iDivisionId = oData[1].ToInt();
                else if (oData[0].Trim() == "Test_Id")
                    iTestId = oData[1].ToInt();
                else if (oData[0].Trim() == "Term_Id")
                    iTermId = oData[1].ToInt();                
            }
        }

        string sHost = Context.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host;
        if (HttpContext.Current.Request.Url.Port != 80)
            sHost = sHost + ":" + HttpContext.Current.Request.Url.Port;

        int iMethodId = 3;

        oExportReportBL = new ExportReportBL(miSchoolId, miAcademicYearId, miUserId);
        oExportReportBL.Host = sHost;

        if ((msReportID == S_RESULTSHEET || msReportID == S_ANNUAL_CONSOLDATED_REPORT) && miSchoolId == Constants.SchoolId.SVP.ToInt())
        {
            if (iMethodId == 3)
            {
                //for (int i = 0; i < 3; i++)
                //{
                    mlStudentMarkDetails = oExportReportBL.GetResultSheetDetailsForExcelInterop(iStandardId, iDivisionId, iTestId, iTermId);
                    if (oExportReportBL.BasicInfo.ShowGrades)
                    {
                        //miFontSize = 11;
                        miFirstRowNo = miFirstRowNo - 1;
                    }

                    string sFileName = string.Empty;

                    if (msReportID == S_RESULTSHEET)
                        sFileName = "ResultSheet_" + Guid.NewGuid() + ".xlsx";
                    else
                        sFileName = "AnnualConsolidatedReport_" + Guid.NewGuid() + ".xlsx";
                    //string filePath = Server.MapPath("..") + @"\UPLOADS\ResultSheet\" + sFileName;
                    string filePath = base.BasePath + @"\UPLOADS\ResultSheet\" + sFileName;

                    using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
                    {
                        WorkbookPart workbookPart = document.AddWorkbookPart();
                        CreateWorkbookPart(workbookPart);
                    }

                    Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
               // }                
            }
            else if (iMethodId == 2)
            {
                //mlStudentMarkDetails = oExportReportBL.GetResultSheetDetailsForExcelInterop(iStandardId, iDivisionId, iTestId, 0);

                //if (oExportReportBL.BasicInfo.ShowGrades)
                //    miFirstRowNo = miFirstRowNo - 1;

                //Excel.Application objXL = null;
                //Excel.Workbook objWB = null;

                //try
                //{
                //    objXL = new Excel.Application();
                //    objWB = objXL.Workbooks.Add(1);
                //    objXL.Visible = false;
                //    objXL.DisplayAlerts = false;

                //    Excel.Worksheet objSHT = (Excel.Worksheet)objWB.Sheets.Add();

                //    objSHT.Name = "ResultSheet";

                //    int iSubjectCount = oExportReportBL.Subjects.Count;
                //    int iGrpSubjectCount = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();
                //    int iTotalCount = 0, iNo = 0;

                //    if (oExportReportBL.BasicInfo.ShowGrades)
                //        miFontSize = 11;

                //    if (oExportReportBL.BasicInfo.ShowGrades)
                //    {
                //        iTotalCount = (iSubjectCount * 2) + 7;
                //        iNo = miFirstRowNo - 1;
                //        objSHT.PageSetup.PrintTitleRows = "$" + miFirstRowNo + ":$" + (miFirstRowNo + 1);
                //    }
                //    else
                //    {
                //        iTotalCount = iSubjectCount + iGrpSubjectCount + 6;
                //        iNo = miFirstRowNo - 2;
                //        objSHT.PageSetup.PrintTitleRows = "$" + (miFirstRowNo - 1) + ":$" + (miFirstRowNo + 1);
                //    }

                //    Excel.Range rngSchoolName = objSHT.get_Range(objSHT.Cells[1, 2], objSHT.Cells[1, iTotalCount]);
                //    rngSchoolName.Merge(Type.Missing);
                //    rngSchoolName.Value2 = oExportReportBL.BasicInfo.SchoolName + ", " + oExportReportBL.BasicInfo.Location;
                //    rngSchoolName.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    rngSchoolName.Font.Size = 14;
                //    rngSchoolName.Font.Name = "SHREE-ENG7-0252";
                //    rngSchoolName.Font.Bold = true;

                //    Excel.Range rngExamName = objSHT.get_Range(objSHT.Cells[2, 2], objSHT.Cells[2, iTotalCount]);
                //    rngExamName.Merge(Type.Missing);
                //    rngExamName.Value2 = "RESULT OF " + oExportReportBL.BasicInfo.TestName + " " + oExportReportBL.BasicInfo.AcademicYear;
                //    rngExamName.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    rngExamName.Font.Size = 14;
                //    rngExamName.Font.Underline = Excel.XlUnderlineStyle.xlUnderlineStyleSingle;
                //    rngExamName.Font.Name = "Calibri";
                //    rngExamName.Font.Bold = true;

                //    string sPath = Server.MapPath("..") + "\\images\\Logos\\School_Logo_Small.jpg";
                //    objSHT.Shapes.AddPicture(sPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 30, 0, 100, 65);

                //    Excel.Range rngClass = objSHT.get_Range(objSHT.Cells[iNo, 1], objSHT.Cells[iNo, iTotalCount]);
                //    rngClass.Merge(Type.Missing);
                //    rngClass.Value2 = "CLASS : " + oExportReportBL.BasicInfo.ClassName;
                //    rngClass.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //    rngClass.Font.Size = 11;
                //    rngClass.Font.Name = "Calibri";
                //    rngClass.Font.Bold = true;

                //    AddGroupSubjectRow(objSHT, miFirstRowNo - 1);
                //    AddBasicColumns(objSHT);
                //    AddSubjectRow(objSHT, miFirstRowNo, 4);
                //    AddOutOfMarksRow(objSHT);
                //    AddStudentMarkDetails(objSHT);

                //    AddFooter(objSHT);

                //    objXL.ActiveWindow.DisplayGridlines = false;

                //    string sFileName = "ResultSheet_" + Guid.NewGuid() + ".xlsx";
                //    string filePath = Server.MapPath("..") + @"\UPLOADS\ResultSheet\" + sFileName;
                //    objWB.SaveCopyAs(filePath);

                //    Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
                //}
                //catch (ThreadAbortException)
                //{
                //}
                //catch (Exception ex)
                //{
                //    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), String.Format("ReportId: {0}", msReportID));
                //}
                //finally
                //{
                //    objWB.Close();
                //    objXL.Quit();
                //}
            }
            else
            {
                SetBasicHTTPResponse();
                StringBuilder obj = oExportReportBL.GetResultSheetDetails(iStandardId, iDivisionId, iTestId);
                HttpContext.Current.Response.Write(obj.ToString());
                HttpContext.Current.Response.Write("</font>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }
        else
        {
            mlStudentMarkDetails = oExportReportBL.GetResultSheetDetailsForPrelimReport(iStandardId, iDivisionId, iTestId);
            if (oExportReportBL.BasicInfo.ShowGrades)
            {
                //miFontSize = 11;
                miFirstRowNo = miFirstRowNo - 1;
            }

            string sFileName = "PrelimResultSheet_" + Guid.NewGuid() + ".xlsx";
            //string filePath = Server.MapPath("..") + @"\UPLOADS\ResultSheet\" + sFileName;
            string filePath = base.BasePath + @"\UPLOADS\ResultSheet\" + sFileName;

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                CreateWorkbookPartForPrelimReport(workbookPart);
            }

            Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
        }
    }


    #region HSP Report

    /// <summary>
    /// This method is used to export result sheet details in excel format.
    /// </summary>
    /// <param name="asFilterString"></param>
    private void ExportAnnualConsolidatedReportHSP(string asFilterString)
    {
        int iStandardId = 0, iDivisionId = 0;
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_Annual_Consolidation_Report;1.", "").Split('@');

        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim() == "Standard_Id")
                    iStandardId = oData[1].ToInt();
                else if (oData[0].Trim() == "Division_Id")
                    iDivisionId = oData[1].ToInt();
            }
        }

        string sHost = Context.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host;
        if (HttpContext.Current.Request.Url.Port != 80)
            sHost = sHost + ":" + HttpContext.Current.Request.Url.Port;

        oExportReportBL = new ExportReportBL(miSchoolId, miAcademicYearId, miUserId);
        oExportReportBL.Host = sHost;

        mlStudentMarkDetails = oExportReportBL.GetAnnualConsolDetailsForHSP(iStandardId, iDivisionId);

        string sFileName = "AnnualConsolidatedReportHSP_" + Guid.NewGuid() + ".xlsx";

        //string filePath = Server.MapPath("..") + @"\UPLOADS\ResultSheet\" + sFileName;
        string filePath = base.BasePath + @"\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkbookPartForHSP(workbookPart);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
    }

    /// <summary>
    /// This method is used to set margin.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private static void SetPageMarginForHSP(Worksheet aoWorksheet, double dbLeftMargin)
    {
        DocumentFormat.OpenXml.Spreadsheet.PageMargins pageMargins1 = new DocumentFormat.OpenXml.Spreadsheet.PageMargins() { Left = dbLeftMargin, Right = 0.25D, Top = 0.25D, Bottom = 0.50D, Header = 0.25D, Footer = 0.25D };
        aoWorksheet.Append(pageMargins1);
    }

    /// <summary>
    /// This method is used to set style properties.
    /// </summary>
    /// <param name="aoWorkbookStylesPart1"></param>
    private void GenerateHSPReportStyles(WorkbookStylesPart aoWorkbookStylesPart1)
    {
        Stylesheet stylesheet1 = new Stylesheet();

        Fonts fonts1 = new Fonts(
            new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new FontName { Val = "Calibri" }
            ),
            new Font( // Index 1 - header
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new Bold { Val = true },
                new Color() { Rgb = "000000" },
                new FontName { Val = "Calibri" }
            ),
            new Font(new DocumentFormat.OpenXml.Spreadsheet.FontSize { Val = 14D },
                    new FontName { Val = "Calibri" },
                    new Bold { Val = true }),
            new Font( // Index 1 - header
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 14 },
                new Bold { Val = true },
                new Color() { Rgb = "000000" },
                new FontName { Val = "Calibri" },
                new Underline { Val = UnderlineValues.Single }
            ),
             new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new FontName { Val = "Calibri" },
                new Bold { Val = true }
                )
            );

        Fills fills1 = new Fills(
               new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
               new Fill(new PatternFill() { PatternType = PatternValues.LightGray }), // Index 1 - default
               new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "A9A9A9" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
           );

        Borders borders = new DocumentFormat.OpenXml.Spreadsheet.Borders(
                new DocumentFormat.OpenXml.Spreadsheet.Border(), // index 0 default
                new DocumentFormat.OpenXml.Spreadsheet.Border( // index 1 black border
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterHeader = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnLeftHeader = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnLeftData = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterData = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterDecimalData = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnSign = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnSignLeft = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnHeader = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnHeaderYear = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnHeaderRight = GetAlignment(HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterDataHeader = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnHeaderOrgName = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnDataRight = GetAlignment(HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center);

        CellFormats cellFormats1 = new CellFormats(
                new CellFormat(), // default
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true, ApplyBorder = true, Alignment = alnLeftHeader }, // header
                new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true, ApplyBorder = true, Alignment = alnCenterHeader }, // header
                new CellFormat { FontId = 2, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnHeader },
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = alnLeftData },
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = alnCenterData },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnSign },
                new CellFormat { FontId = 3, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnHeaderYear },
                new CellFormat { FontId = 4, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnHeaderRight },
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = alnCenterDecimalData, NumberFormatId = 2 },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnSignLeft },
                new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true, ApplyBorder = true, Alignment = alnCenterDataHeader },
                new CellFormat { FontId = 4, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnHeaderOrgName },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnDataRight }
            );

        aoWorkbookStylesPart1.Stylesheet = new Stylesheet(fonts1, fills1, borders, cellFormats1); ;
    }

    /// <summary>
    /// This method is used to set page setup.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private static void SetPageSetupForHSP(Worksheet aoWorksheet, OrientationValues aoOrientationValues)
    {
        PageSetup pageSetup1 = new PageSetup() { PaperSize = (UInt32Value)8U, Orientation = aoOrientationValues, Id = "rId1", FitToHeight = (UInt32Value)0U };
        aoWorksheet.Append(pageSetup1);
    }



    /// <summary>
    /// This method is used to add data row.
    /// </summary>
    /// <param name="aoSheetData"></param>
    private void AddDataRowsForHSP(SheetData aoSheetData)
    {
        decimal iSubjectTotalMarksTerm1 = 0, iSubjectTotalMarksTerm2 = 0, dcTotal = 0, iSubOutOfMarksTerm1 = 0, iSubOutOfMarksTerm2 = 0, iTotalOutofMarks = 0;

        bool bShowAvg = false;
        if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
            bShowAvg = true;

        int iOldGroupSortOrder = 0;
        oExportReportBL.StudentInfos.OrderBy(si => si.RollNo).ToList().ForEach(
            stud =>
            {
                dcTotal = 0;
                iTotalOutofMarks = 0;
                Row row = new Row();
                row.Append(ConstructCell(stud.RollNo.ToString(), CellValues.Number, CellAlignment.CenterData));
                row.Append(ConstructCell(stud.StudentName, CellValues.String, CellAlignment.LeftData));

                oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
                    (
                    sb =>
                    {
                        iSubjectTotalMarksTerm1 = 0;
                        iSubjectTotalMarksTerm2 = 0;
                        iOldGroupSortOrder = 0;
                        iSubOutOfMarksTerm1 = 0;
                        iSubOutOfMarksTerm2 = 0;
                        oExportReportBL.TestDetails.OrderBy(tst => tst.GroupSortOrder).ThenBy(tst=>tst.TestSortOrder).ToList().ForEach(
                            tst =>
                            {
                                var oData = mlStudentMarkDetails.Where(smd => smd.SubjectId == sb.SubjectId && smd.SchoolwiseTestId == tst.SchoolwiseTestId && smd.StudentId == stud.StudentId).FirstOrDefault();
                                if (oData != null)
                                {
                                    if (oData.ExamStatus == string.Empty || oData.ScoredMarks > 0)
                                    {
                                        if (bShowAvg)
                                        {
                                            if (iOldGroupSortOrder != tst.GroupSortOrder)
                                            {
                                                var OGrpData = mlStudentMarkDetails.Join(oExportReportBL.TestDetails, s1 => s1.SchoolwiseTestId, s2 => s2.SchoolwiseTestId, (s1, s2) => new { s1, s2.GroupSortOrder }).Where(ss => ss.GroupSortOrder == 0 && ss.s1.StudentId == stud.StudentId && ss.s1.SubjectId == sb.SubjectId).Select(ss => ss.s1).ToList();

                                                var oTopData = OGrpData.OrderByDescending(ss => ss.ScoredMarks).Take(2).Average(avg => avg.ScoredMarks);
                                                row.Append(ConstructCell(Math.Round(oTopData, 2).ToString(), CellValues.String, CellAlignment.CenterData));

                                                if (tst.TermId == 1)
                                                    iSubjectTotalMarksTerm1 += Math.Round(oTopData, 2);
                                                else
                                                    iSubjectTotalMarksTerm2 += Math.Round(oTopData, 2);

                                                // iTotalOutofMarks += 10;

                                                if (tst.TermId == 1)
                                                    iSubOutOfMarksTerm1 += 10;
                                                else
                                                    iSubOutOfMarksTerm2 += 10;
                                            }
                                        }
                                        row.Append(ConstructCell(oData.ScoredMarks.ToString(), CellValues.String, CellAlignment.CenterData));

                                        if (tst.GroupSortOrder == 1)
                                        {
                                            if (tst.TermId == 1)
                                                iSubjectTotalMarksTerm1 += oData.ScoredMarks;
                                            else
                                                iSubjectTotalMarksTerm2 += oData.ScoredMarks;


                                            //  iTotalOutofMarks += tst.OutOfMarks;

                                            if (tst.TermId == 1)
                                                iSubOutOfMarksTerm1 += tst.OutOfMarks;
                                            else
                                                iSubOutOfMarksTerm2 += tst.OutOfMarks;
                                        }
                                    }
                                    else
                                    {
                                        row.Append(ConstructCell(oData.ExamStatus, CellValues.String, CellAlignment.CenterData));
                                        if (oData.ExamStatus.ToLower() == "ab")
                                        {
                                            if (tst.TermId == 1)
                                                iSubOutOfMarksTerm1 += tst.OutOfMarks;
                                            else
                                                iSubOutOfMarksTerm2 += tst.OutOfMarks;
                                        }
                                    }
                                }

                                iOldGroupSortOrder = tst.GroupSortOrder;
                            }
                            );


                        var exDetails = mlStudentMarkDetails.Where(smd => smd.SubjectId == sb.SubjectId && smd.ExamStatus.ToLower() == "ex" && smd.StudentId == stud.StudentId).FirstOrDefault();
                        if (exDetails != null)
                        {
                            
                            var totalOutOfMarks = oExportReportBL.TestDetails.Where(tst => tst.SchoolwiseTestId == exDetails.SchoolwiseTestId).Sum(tst => tst.OutOfMarks);

                            if (iSubOutOfMarksTerm1 != 100 && iSubOutOfMarksTerm1 != 0)
                            {
                                iSubjectTotalMarksTerm1 = Math.Round((iSubjectTotalMarksTerm1 / iSubOutOfMarksTerm1) * 100, 2);
                                iSubOutOfMarksTerm1 = 100;
                            }

                            if (iSubOutOfMarksTerm2 != 100 && iSubOutOfMarksTerm2 != 0)
                            {
                                iSubjectTotalMarksTerm2 = Math.Round((iSubjectTotalMarksTerm2 / iSubOutOfMarksTerm2) * 100, 2);
                                iSubOutOfMarksTerm2 = 100;
                            }
                           
                          //  iSubjectTotalMarksTerm1 = Math.Round((iSubjectTotalMarksTerm1 / iSubOutOfMarksTerm1) * 200, 2);
                          // iTotalOutofMarks = iTotalOutofMarks + totalOutOfMarks;
                        }

                        iSubjectTotalMarksTerm1 = iSubjectTotalMarksTerm1 + iSubjectTotalMarksTerm2;
                        iSubOutOfMarksTerm1 = iSubOutOfMarksTerm1 + iSubOutOfMarksTerm2;
                        iTotalOutofMarks = iTotalOutofMarks + iSubOutOfMarksTerm1;
                                                
                        //var sss = mlStudentMarkDetails.Where(smd => smd.SubjectId == sb.SubjectId && smd.ExamStatus.ToLower() == "ex" && smd.StudentId == stud.StudentId).FirstOrDefault();
                        //if (sss != null)
                        //{
                        //    iSubjectTotalMarks = Math.Round((iSubjectTotalMarks / 20) * 100, 2);
                        //    iTotalOutofMarks = iTotalOutofMarks + 80;
                        //}

                        row.Append(ConstructCell(iSubjectTotalMarksTerm1.ToString(), CellValues.String, CellAlignment.CenterData));
                        dcTotal += iSubjectTotalMarksTerm1;
                    }
                    );

                //oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
                //    (
                //    sb =>
                //    {
                //        iSubjectTotalMarks = 0;
                //        iOldGroupSortOrder = 0;
                //        oExportReportBL.TestDetails.OrderBy(tst => tst.GroupSortOrder).ThenBy(tst => tst.TestSortOrder).ToList().ForEach(
                //            tst =>
                //            {
                //                var oData = mlStudentMarkDetails.Where(smd => smd.SubjectId == sb.SubjectId && smd.SchoolwiseTestId == tst.SchoolwiseTestId && smd.StudentId == stud.StudentId).FirstOrDefault();
                //                if (oData != null)
                //                {
                //                    if (oData.ExamStatus == string.Empty || oData.ScoredMarks > 0)
                //                    {
                //                        //if (bShowAvg)
                //                        //{
                //                        //    if (iOldGroupSortOrder != tst.GroupSortOrder)
                //                        //    {
                //                        //        var OGrpData = mlStudentMarkDetails.Join(oExportReportBL.TestDetails, s1 => s1.SchoolwiseTestId, s2 => s2.SchoolwiseTestId, (s1, s2) => new { s1, s2.GroupSortOrder }).Where(ss => ss.GroupSortOrder == 0 && ss.s1.StudentId == stud.StudentId && ss.s1.SubjectId == sb.SubjectId).Select(ss => ss.s1).ToList();

                //                        //        var oTopData = OGrpData.OrderByDescending(ss => ss.ScoredMarks).Take(2).Average(avg => avg.ScoredMarks);
                //                        //        row.Append(ConstructCell(Math.Round(oTopData, 1).ToString(), CellValues.String, CellAlignment.CenterData));
                //                        //        iSubjectTotalMarks += Math.Round(oTopData, 1);
                //                        //        iTotalOutofMarks += 10;
                //                        //    }
                //                        //}
                //                        if (sb.IsGradingSubject)
                //                            row.Append(ConstructCell(oData.Grade, CellValues.String, CellAlignment.CenterData));
                //                        else
                //                            row.Append(ConstructCell(oData.ScoredMarks.ToString(), CellValues.String, CellAlignment.CenterData));

                //                        if (tst.GroupSortOrder == 1)
                //                        {
                //                            iSubjectTotalMarks += oData.ScoredMarks;
                //                            iTotalOutofMarks += tst.OutOfMarks;
                //                        }
                //                    }
                //                    else
                //                        row.Append(ConstructCell(oData.ExamStatus, CellValues.String, CellAlignment.CenterData));
                //                }

                //                iOldGroupSortOrder = tst.GroupSortOrder;
                //            }
                //            );

                //        if (!sb.IsGradingSubject)
                //        {
                //            row.Append(ConstructCell(iSubjectTotalMarks.ToString(), CellValues.String, CellAlignment.CenterData));
                //            dcTotal += iSubjectTotalMarks;
                //        }
                //    }
                //    );


                if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
                {
                    dcTotal = Math.Round(dcTotal);
                    row.Append(ConstructCell(dcTotal.ToInt().ToString(), CellValues.String, CellAlignment.CenterData));
                }
                else
                    row.Append(ConstructCell(dcTotal.ToString(), CellValues.String, CellAlignment.CenterData));

                row.Append(ConstructCell(Math.Round((dcTotal/iTotalOutofMarks)*100,2).ToString(), CellValues.String, CellAlignment.CenterData));                
                var oTotal = oExportReportBL.StudentMarkSummary.Where(ss => ss.StudentId == stud.StudentId).FirstOrDefault();
                row.Append(ConstructCell(oTotal.Rank.ToString(), CellValues.String, CellAlignment.CenterData));

                aoSheetData.Append(row);
            }
        );
    }

    private MergeCells MergeCellsHSP()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        int iSubjectCount = oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject);
        int iTestCount = oExportReportBL.TestDetails.Count + 1;

        if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
            iTestCount++;

        string sLastCell;
        int iLastCellIndex = 2 + (iSubjectCount * iTestCount) + 3;

        if (iLastCellIndex >= 53 && iLastCellIndex < 79)
            sLastCell = "B" + ((char)(64 + (iLastCellIndex - 52))).ToString();
        else if (iLastCellIndex >= 27 && iLastCellIndex < 53)
            sLastCell = "A" + ((char)(64 + (iLastCellIndex - 26))).ToString();
        else if (iLastCellIndex >= 79 && iLastCellIndex < 105)
            sLastCell = "C" + ((char)(64 + (iLastCellIndex - 78))).ToString();
        else if (iLastCellIndex >= 105 && iLastCellIndex < 131)
            sLastCell = "D" + ((char)(64 + (iLastCellIndex - 104))).ToString();
        else if (iLastCellIndex >= 131 && iLastCellIndex < 157)
            sLastCell = "E" + ((char)(64 + (iLastCellIndex - 130))).ToString();
        else if (iLastCellIndex >= 157)
            sLastCell = "F" + ((char)(64 + (iLastCellIndex - 156))).ToString();
        else
            sLastCell = ((char)(65 + iLastCellIndex)).ToString();

        mergeCells1.Append(new MergeCell() { Reference = "A" + miSchoolNameRowIndex + ":" + sLastCell + miSchoolNameRowIndex });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (miSchoolNameRowIndex + 1) + ":" + sLastCell + (miSchoolNameRowIndex + 1) });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (miSchoolNameRowIndex + 2) + ":" + sLastCell + (miSchoolNameRowIndex + 2) });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (miFirstRowForHSP - 1) + ":" + sLastCell + (miFirstRowForHSP - 1) });

        mergeCells1.Append(new MergeCell() { Reference = "A" + miFirstRowForHSP + ":" + "A" + (miFirstRowForHSP + 2) });
        mergeCells1.Append(new MergeCell() { Reference = "B" + miFirstRowForHSP + ":" + "B" + (miFirstRowForHSP + 2) });

        string sStartCell = string.Empty, sEndCell = string.Empty;
        int iCellIndex = 2;

        for (int iIndex = 0; iIndex < iSubjectCount; iIndex++)
        {
            if (iCellIndex >= 52)
                sStartCell = "B" + ((char)(65 + (iCellIndex - 52))) + miFirstRowForHSP.ToString();
            else if (iCellIndex >= 26)
                sStartCell = "A" + ((char)(65 + (iCellIndex - 26))) + miFirstRowForHSP.ToString();
            else
                sStartCell = ((char)(65 + iCellIndex)) + miFirstRowForHSP.ToString();

            if ((iCellIndex + iTestCount - 1) >= 52)
                sEndCell = "B" + ((char)(65 + (iCellIndex + iTestCount - 1 - 52))) + miFirstRowForHSP.ToString();
            else if ((iCellIndex + iTestCount - 1) >= 26)
                sEndCell = "A" + ((char)(65 + (iCellIndex + iTestCount - 1 - 26))) + miFirstRowForHSP.ToString();
            else
                sEndCell = ((char)(65 + (iCellIndex + iTestCount - 1))) + miFirstRowForHSP.ToString();

            mergeCells1.Append(new MergeCell() { Reference = sStartCell + ":" + sEndCell });

            iCellIndex = iCellIndex + iTestCount;
        }

        int iCoCurrSubjectCount = oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject);
        int iCoTestCount = 2;

        //for (int iIndex = 0; iIndex < iCoCurrSubjectCount; iIndex++)
        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).ToList().ForEach
            (
            sb=>
                {
                    if (sb.IsGradingSubject)
                        iCoTestCount = 2;
                    else
                        iCoTestCount = 3;

                    if (iCellIndex >= 52)
                        sStartCell = "B" + ((char)(65 + (iCellIndex - 52))) + miFirstRowForHSP.ToString();
                    else if (iCellIndex >= 26)
                        sStartCell = "A" + ((char)(65 + (iCellIndex - 26))) + miFirstRowForHSP.ToString();                
                    else
                        sStartCell = ((char)(65 + iCellIndex)) + miFirstRowForHSP.ToString();

                    if ((iCellIndex + iCoTestCount - 1) >= 52 && (iCellIndex + iCoTestCount - 1) < 78)
                        sEndCell = "B" + ((char)(65 + (iCellIndex + iCoTestCount - 1 - 52))) + miFirstRowForHSP.ToString();
                    else if ((iCellIndex + iCoTestCount - 1) >= 26 && (iCellIndex + iCoTestCount - 1) < 52)
                        sEndCell = "A" + ((char)(65 + (iCellIndex + iCoTestCount - 1 - 26))) + miFirstRowForHSP.ToString();
                    else if ((iCellIndex + iCoTestCount - 1) >= 78)
                        sEndCell = "C" + ((char)(65 + (iCellIndex + iCoTestCount - 1 - 78))) + miFirstRowForHSP.ToString();
                    else
                        sEndCell = ((char)(65 + (iCellIndex + iCoTestCount - 1))) + miFirstRowForHSP.ToString();

                    mergeCells1.Append(new MergeCell() { Reference = sStartCell + ":" + sEndCell });

                    iCellIndex = iCellIndex + iCoTestCount;
                }
            );

        string sTotalCell, sPercentage, sRank;
        int iGrandTotalCellIndex;

        if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
            iGrandTotalCellIndex = 2 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * (oExportReportBL.TestDetails.Count + 2));
        else
            iGrandTotalCellIndex = 2 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * (oExportReportBL.TestDetails.Count + 1));

        int iPercentageIndex = iGrandTotalCellIndex + 1;
        int iRankIndex = iGrandTotalCellIndex + 2;

        if (iGrandTotalCellIndex >= 52)
            sTotalCell = "B" + (((char)(65 + (iGrandTotalCellIndex - 52)))).ToString();
        else if (iGrandTotalCellIndex >= 26)
            sTotalCell = "A" + (((char)(65 + (iGrandTotalCellIndex - 26)))).ToString();
        else
            sTotalCell = ((char)(65 + iGrandTotalCellIndex)).ToString();

        if (iPercentageIndex >= 52)
            sPercentage = "B" + ((char)(65 + (iPercentageIndex - 52))).ToString();
        else if (iPercentageIndex >= 26)
            sPercentage = "A" + ((char)(65 + (iPercentageIndex - 26))).ToString();
        else
            sPercentage = ((char)(65 + iPercentageIndex)).ToString();

        if (iRankIndex >= 52)
            sRank = "B" + ((char)(65 + (iRankIndex - 52))).ToString();
        else if (iRankIndex >= 26)
            sRank = "A" + ((char)(65 + (iRankIndex - 26))).ToString();
        else
            sRank = ((char)(65 + iRankIndex)).ToString();

        mergeCells1.Append(new MergeCell() { Reference = sTotalCell + miFirstRowForHSP + ":" + sTotalCell + (miFirstRowForHSP + 1) });
        mergeCells1.Append(new MergeCell() { Reference = sPercentage + miFirstRowForHSP + ":" + sPercentage + (miFirstRowForHSP + 1) });
        mergeCells1.Append(new MergeCell() { Reference = sRank + miFirstRowForHSP + ":" + sRank + (miFirstRowForHSP + 2) });

        int iSignRowIndex = miFirstRowForHSP + 3 + oExportReportBL.StudentInfos.Count();
        iSignRowIndex = iSignRowIndex + 2; // Two blank rows are added.
        int iTotalCellCount = 2 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * oExportReportBL.TestDetails.Count) + 2;
        int iCnt = iTotalCellCount / 2;

        string sMidCell = string.Empty;
        if (iCnt >= 52)
            sMidCell = "B" + ((char)(65 + (iCnt - 52))).ToString();
        else if (iCnt >= 26)
            sMidCell = "A" + ((char)(65 + (iCnt - 26))).ToString();
        else
            sMidCell = ((char)(65 + iCnt)).ToString();

        mergeCells1.Append(new MergeCell() { Reference = "A" + iSignRowIndex + ":" + "C" + iSignRowIndex });
        mergeCells1.Append(new MergeCell() { Reference = sMidCell + iSignRowIndex + ":" + sRank + iSignRowIndex });

        // For CoCurricular Subjects.

        int iCoCurGrandeTotalCellIndex;
        string sCoCurPercentage, sCoCurTotalCell, sCoCurRank;

        if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
            iCoCurGrandeTotalCellIndex = 2 + (oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject) * (iCoTestCount + 2));
        else
            iCoCurGrandeTotalCellIndex = 2 + (oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject) * (iCoTestCount + 1));

        int iCoCurPercentageIndex = iCoCurGrandeTotalCellIndex + 1;
        int iCoCurRankIndex = iCoCurGrandeTotalCellIndex + 2;

        if (iCoCurGrandeTotalCellIndex >= 52)
            sCoCurTotalCell = "B" + (((char)(65 + (iCoCurGrandeTotalCellIndex - 52)))).ToString();
        else if (iCoCurGrandeTotalCellIndex >= 26)
            sCoCurTotalCell = "A" + (((char)(65 + (iCoCurGrandeTotalCellIndex - 26)))).ToString();
        else
            sCoCurTotalCell = ((char)(65 + iCoCurGrandeTotalCellIndex)).ToString();

        if (iCoCurPercentageIndex >= 52)
            sCoCurPercentage = "B" + ((char)(65 + (iCoCurPercentageIndex - 52))).ToString();
        else if (iCoCurPercentageIndex >= 26)
            sCoCurPercentage = "A" + ((char)(65 + (iCoCurPercentageIndex - 26))).ToString();
        else
            sCoCurPercentage = ((char)(65 + iCoCurPercentageIndex)).ToString();

        if (iCoCurRankIndex >= 52)
            sCoCurRank = "B" + ((char)(65 + (iCoCurRankIndex - 52))).ToString();
        else if (iCoCurRankIndex >= 26)
            sCoCurRank = "A" + ((char)(65 + (iCoCurRankIndex - 26))).ToString();
        else
            sCoCurRank = ((char)(65 + iCoCurRankIndex)).ToString();

        mergeCells1.Append(new MergeCell() { Reference = sCoCurTotalCell + miFirstRowForHSP + ":" + sCoCurTotalCell + (miFirstRowForHSP + 1) });
        mergeCells1.Append(new MergeCell() { Reference = sCoCurPercentage + miFirstRowForHSP + ":" + sCoCurPercentage + (miFirstRowForHSP + 1) });
        mergeCells1.Append(new MergeCell() { Reference = sCoCurRank + miFirstRowForHSP + ":" + sCoCurRank + (miFirstRowForHSP + 2) });

        int iCoCurSignRowIndex = miFirstRowForHSP + 3 + oExportReportBL.StudentInfos.Count();
        iCoCurSignRowIndex = iCoCurSignRowIndex + 2; // Two blank rows are added.
        int iCoCurTotalCellCount = 2 + (oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject) * iCoTestCount) + 2;
        int iCoCurCnt = iCoCurTotalCellCount / 2;

        string sCoCurMidCell = string.Empty;
        if (iCoCurCnt >= 52)
            sCoCurMidCell = "B" + ((char)(65 + (iCoCurCnt - 52))).ToString();
        else if (iCoCurCnt >= 26)
            sCoCurMidCell = "A" + ((char)(65 + (iCoCurCnt - 26))).ToString();
        else
            sCoCurMidCell = ((char)(65 + iCoCurCnt)).ToString();

        mergeCells1.Append(new MergeCell() { Reference = "A" + iCoCurSignRowIndex + ":" + "C" + iCoCurSignRowIndex });
        mergeCells1.Append(new MergeCell() { Reference = sCoCurMidCell + iCoCurSignRowIndex + ":" + sCoCurRank + iCoCurSignRowIndex });

        return mergeCells1;
    }

    /// <summary>
    /// This method is used to generate part contents.
    /// </summary>
    /// <param name="aoPart"></param>
    private void GeneratePartContentForHSP(WorkbookPart aoPart, bool abIsPrelimReport)
    {
        Workbook workbook1 = new Workbook();
        workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        AddFileVersion(workbook1);

        AddWorkbookProperties(workbook1);

        AddBookViews(workbook1);

        AddSheets(workbook1);

        AddDefinedNamesHSP(workbook1, abIsPrelimReport);

        AddCalculationProperties(workbook1);

        aoPart.Workbook = workbook1;
    }


    /// <summary>
    /// This method is used to set defines names.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    private void AddDefinedNamesHSP(Workbook aoWorkbook, bool abIsPrelimReport)
    {
        DefinedNames definedNames1 = new DefinedNames();
        DefinedName definedName1 = new DefinedName() { Name = "_xlnm.Print_Titles", LocalSheetId = (UInt32Value)0U };

        definedName1.Text = "AnnualConsolidatedReport" + "!$" + (miFirstRowForHSP - 1) + ":$" + (miFirstRowForHSP + 1);

        definedNames1.Append(definedName1);
        aoWorkbook.Append(definedNames1);
    }


    /// <summary>
    /// This method is used to add title row.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <returns></returns>
    private int AddSubjectTitleRowForHSP(SheetData aoResultSheetData, int aiiSharedIndex)
    {
        //Row row = new Row { Height = 22D, CustomHeight = true };
        Row row = new Row();
        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataHeader));
        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataHeader));

        bool bShowAvg = false;
        if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
            bShowAvg = true;

        int iOldGroupSortOrder = 0;        
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                iOldGroupSortOrder = 0;
                oExportReportBL.TestDetails.OrderBy(td => td.GroupSortOrder).ThenBy(td=>td.TestSortOrder).ToList().ForEach
                    (
                        smd =>
                        {
                            if (bShowAvg)
                            {
                                if (iOldGroupSortOrder != smd.GroupSortOrder)
                                    row.Append(ConstructCell("AVG", CellValues.String, CellAlignment.CenterDataHeader));                                    
                                row.Append(ConstructCell(smd.TestName, CellValues.String, CellAlignment.CenterDataHeader));
                            }
                            else
                                row.Append(ConstructCell(smd.TestName, CellValues.String, CellAlignment.CenterDataHeader));

                            iOldGroupSortOrder = smd.GroupSortOrder;
                        }
                    );

                row.Append(ConstructCell("Total", CellValues.String, CellAlignment.CenterDataHeader));
            }
        );
        

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                iOldGroupSortOrder = 0;

                var oTestIds = mlStudentMarkDetails.Where(sm => sm.SubjectId == sb.SubjectId).Select(sm => sm.SchoolwiseTestId).Distinct();

                oExportReportBL.TestDetails.OrderBy(td => td.GroupSortOrder).ThenBy(td => td.TestSortOrder).ToList().ForEach
                    (
                        smd =>
                        {
                            if (oTestIds.Contains(smd.SchoolwiseTestId))
                            {
                                row.Append(ConstructCell(smd.TestName, CellValues.String, CellAlignment.CenterDataHeader));
                                iOldGroupSortOrder = smd.GroupSortOrder;
                            }
                        }
                    );

                if (!sb.IsGradingSubject)
                    row.Append(ConstructCell("Total", CellValues.String, CellAlignment.CenterDataHeader));
            }
        );

        row.Append(ConstructCell("G.T.", CellValues.String, CellAlignment.CenterDataHeader));
        row.Append(ConstructCell("%", CellValues.String, CellAlignment.CenterDataHeader));
        row.Append(ConstructCell("RANK", CellValues.String, CellAlignment.CenterDataHeader));

        aoResultSheetData.Append(row);

        return aiiSharedIndex;
    }


    /// <summary>
    /// This method is used to add title row.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <returns></returns>
    private int AddOutOfMarksRowForHSP(SheetData aoResultSheetData, int aiiSharedIndex)
    {
        Row row = new Row();
        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataHeader));
        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataHeader));

        bool bShowAvg = false;
        if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
            bShowAvg = true;

        int iTotal = 0, iSubjectTotalMarks = 0, iOldGroupSortOrder = 0;
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                iSubjectTotalMarks = 0;
                iOldGroupSortOrder = 0;
                oExportReportBL.TestDetails.OrderBy(td => td.GroupSortOrder).ThenBy(td=>td.TestSortOrder).ToList().ForEach
                    (
                        smd =>
                        {
                            if (bShowAvg)
                            {
                                if (iOldGroupSortOrder != smd.GroupSortOrder)
                                {
                                    row.Append(ConstructCell("10", CellValues.String, CellAlignment.CenterDataHeader));
                                    iSubjectTotalMarks += 10;
                                    iTotal = iTotal + 10;
                                }

                                row.Append(ConstructCell(smd.OutOfMarks.ToString(), CellValues.String, CellAlignment.CenterDataHeader));

                                if (smd.GroupSortOrder == 1)
                                {
                                    iSubjectTotalMarks += smd.OutOfMarks;
                                    iTotal = iTotal + smd.OutOfMarks;
                                }
                            }
                            else
                            {
                                row.Append(ConstructCell(smd.OutOfMarks.ToString(), CellValues.String, CellAlignment.CenterDataHeader));
                                iTotal = iTotal + smd.OutOfMarks;
                                iSubjectTotalMarks += smd.OutOfMarks;
                            }

                            iOldGroupSortOrder = smd.GroupSortOrder;
                        }
                    );

                row.Append(ConstructCell(iSubjectTotalMarks.ToString(), CellValues.String, CellAlignment.CenterDataHeader));
            }
        );

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
           (
           sb =>
           {
               iSubjectTotalMarks = 0;
               iOldGroupSortOrder = 0;
               var oTestIds = mlStudentMarkDetails.Where(sm => sm.SubjectId == sb.SubjectId).Select(sm => sm.SchoolwiseTestId).Distinct();

               oExportReportBL.TestDetails.OrderBy(td => td.GroupSortOrder).ThenBy(td => td.TestSortOrder).ToList().ForEach
                   (
                       smd =>
                       {
                           if (oTestIds.Contains(smd.SchoolwiseTestId))
                           {
                               if (bShowAvg)
                               {
                                   if (iOldGroupSortOrder != smd.GroupSortOrder)
                                   {
                                       row.Append(ConstructCell("10", CellValues.String, CellAlignment.CenterDataHeader));
                                       iSubjectTotalMarks += 10;
                                       iTotal = iTotal + 10;
                                   }

                                   row.Append(ConstructCell(smd.OutOfMarks.ToString(), CellValues.String, CellAlignment.CenterDataHeader));

                                   if (smd.GroupSortOrder == 1)
                                   {
                                       iSubjectTotalMarks += smd.OutOfMarks;
                                       iTotal = iTotal + smd.OutOfMarks;
                                   }
                               }
                               else
                               {
                                   row.Append(ConstructCell(smd.OutOfMarks.ToString(), CellValues.String, CellAlignment.CenterDataHeader));
                                   iTotal = iTotal + smd.OutOfMarks;
                                   iSubjectTotalMarks += smd.OutOfMarks;
                               }

                               iOldGroupSortOrder = smd.GroupSortOrder;
                           }
                       }
                   );

               if(!sb.IsGradingSubject)
                    row.Append(ConstructCell(iSubjectTotalMarks.ToString(), CellValues.String, CellAlignment.CenterDataHeader));
           }
       );

        row.Append(ConstructCell(iTotal.ToString(), CellValues.String, CellAlignment.CenterDataHeader));
        row.Append(ConstructCell("100", CellValues.String, CellAlignment.CenterDataHeader));
        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataHeader));

        aoResultSheetData.Append(row);

        return aiiSharedIndex;
    }

    /// <summary>
    /// This method is used to add title row.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <returns></returns>
    private int AddTitleRowForHSP(SheetData aoResultSheetData)
    {
        string sOldSubjectGroup = string.Empty;

        int iCellIndex = 3, iSharedIndex = 0;

        sOldSubjectGroup = string.Empty;
        Row row2 = new Row() { RowIndex = Convert.ToUInt32(miFirstRowForHSP), Spans = new ListValue<StringValue>() { InnerText = "1:2" }, Height = 15.75D, CustomHeight = true };
        row2.Append(AddTitleCell("A" + (miFirstRowForHSP), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iSharedIndex++;

        row2.Append(AddTitleCell("B" + (miFirstRowForHSP), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iSharedIndex++;

        string sCell;

        bool bShowAvg = false;
        if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
            bShowAvg = true;

        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);

                int iCnt = oExportReportBL.TestDetails.Count;

                if (bShowAvg)
                    iCnt++;

                for (int iIndex = 0; iIndex < iCnt - 1; iIndex++)
                    sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);

                sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);
            }
            );

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);

                int iCnt = oExportReportBL.TestDetails.Count;

                if (bShowAvg)
                    iCnt++;

                for (int iIndex = 0; iIndex < iCnt - 1; iIndex++)
                    sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);

                sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);
            }
            );

        sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);
        sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);
        sCell = AddSubjectCellHSP(ref iCellIndex, ref iSharedIndex, row2);

        aoResultSheetData.Append(row2);
        return iSharedIndex;
    }

    private string AddSubjectCellHSP(ref int iCellIndex, ref int iSharedIndex, Row row2)
    {
        string sCell;
        if (iCellIndex >= 53 && iCellIndex < 79)
            sCell = "B" + ((char)(64 + (iCellIndex - 52))).ToString();
        else if (iCellIndex >= 27 && iCellIndex < 53)
            sCell = "A" + ((char)(64 + (iCellIndex - 26))).ToString();
        else if (iCellIndex >= 79 && iCellIndex < 105)
            sCell = "C" + ((char)(64 + (iCellIndex - 78))).ToString();
        else if (iCellIndex >= 105 && iCellIndex < 131)
            sCell = "D" + ((char)(64 + (iCellIndex - 104))).ToString();
        else if (iCellIndex >= 131 && iCellIndex < 157)
            sCell = "E" + ((char)(64 + (iCellIndex - 130))).ToString();
        else if (iCellIndex >= 157)
            sCell = "F" + ((char)(64 + (iCellIndex - 156))).ToString();
        else
            sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowForHSP), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        iSharedIndex++;
        return sCell;
    }

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    private void SetColumnWidthForHSP(Worksheet aoResultSheetData)
    {

        Columns columns1 = new Columns();
        Column column1 = new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 9.43D, CustomWidth = true };
        Column column2 = new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 35.57D, CustomWidth = true };
        columns1.Append(column1);
        columns1.Append(column2);

        int iStartIndex = 3; ;
        int iCnt = oExportReportBL.TestDetails.Count;
        if (oExportReportBL.TestDetails.Any(sb => sb.GroupSortOrder == 0))
            iCnt++;

        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).ToList().ForEach
            (
            sb =>
            {
                columns1.Append(new Column() { Min = Convert.ToUInt32(iStartIndex), Max = Convert.ToUInt32(iStartIndex + iCnt - 1), Width = 5D, CustomWidth = true });
                columns1.Append(new Column() { Min = Convert.ToUInt32(iStartIndex + iCnt), Max = Convert.ToUInt32(iStartIndex + iCnt), Width = 6D, CustomWidth = true });
                iStartIndex = iStartIndex + iCnt + 1;
            }
            );

        int iTotalColumnIndex = 2 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * (iCnt + 1)) + 1;
        columns1.Append(new Column() { Min = Convert.ToUInt32(iTotalColumnIndex), Max = Convert.ToUInt32(iTotalColumnIndex), Width = 9D, CustomWidth = true });
        columns1.Append(new Column() { Min = Convert.ToUInt32(iTotalColumnIndex + 1), Max = Convert.ToUInt32(iTotalColumnIndex + 1), Width = 7D, CustomWidth = true });
        columns1.Append(new Column() { Min = Convert.ToUInt32(iTotalColumnIndex + 2), Max = Convert.ToUInt32(iTotalColumnIndex + 2), Width = 7D, CustomWidth = true });
        aoResultSheetData.Append(columns1);
    }


    /// <summary>
    /// This method is used to add report header.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    private void AddReportHeaderForHSP(SheetData aoResultSheetData, bool abIsPrelimReport)
    {
        int iNonGradingSubjectCount = oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject && !sb.IsGradingSubject);
        int iTotalCellCount = 2 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * oExportReportBL.TestDetails.Count) + ((oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject) * Constants.I_TWO) + iNonGradingSubjectCount) + 3;

        Row row3 = new Row { RowIndex = Convert.ToUInt32(miSchoolNameRowIndex), Height = 20D, CustomHeight = true };

        for (int iIndex = 0; iIndex < iTotalCellCount; iIndex++)
        {
            if (iIndex == 0)
                row3.Append(ConstructCell(oExportReportBL.BasicInfo.OrgName, CellValues.String, CellAlignment.ReportHeaderOrg));
            else
                row3.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.RightHeaderClass));
        }

        aoResultSheetData.AppendChild(row3);

        Row row0 = new Row { RowIndex = Convert.ToUInt32(miSchoolNameRowIndex + 1), Height = 20D, CustomHeight = true };

        for (int iIndex = 0; iIndex < iTotalCellCount; iIndex++)
        {
            if (iIndex == 0)
                row0.Append(ConstructCell(oExportReportBL.BasicInfo.SchoolName, CellValues.String, CellAlignment.ReportHeader));
            else
                row0.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.RightHeaderClass));
        }

        aoResultSheetData.AppendChild(row0);


        Row row1 = new Row { RowIndex = Convert.ToUInt32(miSchoolNameRowIndex + 2), Height = 20D, CustomHeight = true };

        for (int iIndex = 0; iIndex < iTotalCellCount; iIndex++)
        {
            if (iIndex == 0)
                row1.Append(ConstructCell("YEAR " + oExportReportBL.BasicInfo.AcademicYear, CellValues.String, CellAlignment.ReportHeaderOrg));
            else
                row1.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.RightHeaderClass));
        }

        aoResultSheetData.AppendChild(row1);


        Row row2 = new Row { RowIndex = Convert.ToUInt32(miFirstRowForHSP - 1), Height = 20D, CustomHeight = true };

        for (int iIndex = 0; iIndex < iTotalCellCount; iIndex++)
        {
            if (iIndex == 0)
                row2.Append(ConstructCell("CLASS :- " + oExportReportBL.BasicInfo.ClassName, CellValues.String, CellAlignment.LeftDataWithNoBorder));
            else
                row2.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.RightHeaderClass));
        }

        aoResultSheetData.AppendChild(row2);

    }

    /// <summary>
    /// This method is sued to add print settings.
    /// </summary>
    /// <param name="worksheet1"></param>
    private void AddPrintSettingsForHSP(Worksheet worksheet1, bool abIsPrelimReport)
    {
        AddPrintOptions(worksheet1);
        SetPageMarginForHSP(worksheet1, 0.2);
        SetPageSetupForHSP(worksheet1, OrientationValues.Landscape);
        GenerateHeaderFooter(worksheet1);
    }

    /// <summary>
    /// This method is used to generate worksheet part 1 content.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateHSPReportContent(WorksheetPart aoWorksheetPart1)
    {
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        AddSheetDetails(worksheet1);

        SetColumnWidthForHSP(worksheet1);

        SheetData sheetData1 = new SheetData();

        AddReportHeaderForHSP(sheetData1, false);

        int iSharedTitleIndex = AddTitleRowForHSP(sheetData1);
        iSharedTitleIndex = AddSubjectTitleRowForHSP(sheetData1, iSharedTitleIndex);
        iSharedTitleIndex = AddOutOfMarksRowForHSP(sheetData1, iSharedTitleIndex);

        AddDataRowsForHSP(sheetData1);

        AddSignsForHSP(sheetData1);

        worksheet1.Append(sheetData1);

        worksheet1.Append(MergeCellsHSP());

        AddPrintSettingsForHSP(worksheet1, false);

        aoWorksheetPart1.Worksheet = worksheet1;
    }

    private void AddSignsForHSP(SheetData sheetData1)
    {
        int iTotalCellCount = 2 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * oExportReportBL.TestDetails.Count) + (oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject) * Constants.I_TWO) + 3;        

        AddEmptyRow(sheetData1);
        AddEmptyRow(sheetData1);
        
        Row row0 = new Row { Height = 20D, CustomHeight = true };
        row0.Append(ConstructCell("Class Teacher : " + oExportReportBL.BasicInfo.ClassTeacherName, CellValues.String, CellAlignment.LeftDataWithNoBorder));

        int iMidCount = iTotalCellCount / 2;
        for (int iIndex = 1; iIndex <= iTotalCellCount; iIndex++)
        {
            if (iIndex == iMidCount)
                row0.Append(ConstructCell("Principal : " + oExportReportBL.BasicInfo.PrincipalName, CellValues.String, CellAlignment.RightDataWithNoBorder));
            else
                row0.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.RightHeaderClass));
        }

        sheetData1.AppendChild(row0);
    }

    private void AddEmptyRow(SheetData sheetData1)
    {
        Row row1 = new Row { Height = 20D, CustomHeight = true };
        row1.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.RightHeaderClass));
        sheetData1.AppendChild(row1);
    }

    /// <summary>
    /// This method is used to generated shred string table part content.
    /// </summary>
    /// <param name="aoSharedStringTablePart1"></param>
    private void GenerateSharedStringForHSP(SharedStringTablePart aoSharedStringTablePart1)
    {
        SharedStringTable sharedStringTable1 = new SharedStringTable() { Count = (UInt32Value)4U, UniqueCount = (UInt32Value)4U };

        sharedStringTable1.Append(GetSharedItem("ROLL NO."));
        sharedStringTable1.Append(GetSharedItem("STUDENT NAME"));

        bool bShowAvg = false;
        if (oExportReportBL.TestDetails.Any(td => td.GroupSortOrder == 0))
            bShowAvg = true;

        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                sharedStringTable1.Append(GetSharedItem(sb.SubjectName));

                int iCnt = oExportReportBL.TestDetails.Count;

                if (bShowAvg)
                    iCnt = iCnt + 1;

                for (int iIndex = 0; iIndex < iCnt - 1; iIndex++)
                    sharedStringTable1.Append(GetSharedItem(""));

                // Add a blank item to add subject total.
                sharedStringTable1.Append(GetSharedItem(string.Empty));
            }
            );

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
            (
            sb =>
            {
                sharedStringTable1.Append(GetSharedItem(sb.SubjectName));

                var oTestId = mlStudentMarkDetails.Where(sm => sm.SubjectId == sb.SubjectId).Select(sm => sm.SchoolwiseTestId).Distinct();

                int iCnt = oTestId.Count();               

                if (bShowAvg)
                    iCnt = iCnt + 1;

                for (int iIndex = 0; iIndex < iCnt - 1; iIndex++)
                    sharedStringTable1.Append(GetSharedItem(""));

                // Add a blank item to add subject total.
                if(!sb.IsGradingSubject)
                    sharedStringTable1.Append(GetSharedItem(string.Empty));
            }
            );

        sharedStringTable1.Append(GetSharedItem("GRAND TOTAL"));
        sharedStringTable1.Append(GetSharedItem("PER(%)"));
        sharedStringTable1.Append(GetSharedItem("RANK"));

        aoSharedStringTablePart1.SharedStringTable = sharedStringTable1;
    }

    /// <summary>
    /// This method is used to create workbook part.
    /// </summary>
    /// <param name="aoPart"></param>
    public void CreateWorkbookPartForHSP(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        GenerateHSPReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateHSPReportContent(worksheetPart1);

        SharedStringTablePart sharedStringTablePart1 = aoPart.AddNewPart<SharedStringTablePart>("rId4");
        GenerateSharedStringForHSP(sharedStringTablePart1);

        //string filePath = Server.MapPath("..") + @"\Images\Logos\School_Logo_Small.jpg";
        //AddImage(filePath, worksheetPart1);

        GeneratePartContentForHSP(aoPart, false);
    } 

    #endregion

    #region Prelim Report

    /// <summary>
    /// This method is used to create workbook part.
    /// </summary>
    /// <param name="aoPart"></param>
    public void CreateWorkbookPartForPrelimReport(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        GenerateWorkbookStylesPart1Content(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GeneratePartOrPrelimReport(worksheetPart1);

        SharedStringTablePart sharedStringTablePart1 = aoPart.AddNewPart<SharedStringTablePart>("rId4");
        GenerateSharedStringForPrelimReport(sharedStringTablePart1);

        string filePath = Server.MapPath("..") + @"\Images\Logos\School_Logo_Small.jpg";
        AddImage(filePath, worksheetPart1);

        GeneratePartContent(aoPart, true);
    }


    /// <summary>
    /// This method is used to generated shred string table part content.
    /// </summary>
    /// <param name="aoSharedStringTablePart1"></param>
    private void GenerateSharedStringForPrelimReport(SharedStringTablePart aoSharedStringTablePart1)
    {
        SharedStringTable sharedStringTable1 = new SharedStringTable() { Count = (UInt32Value)4U, UniqueCount = (UInt32Value)4U };

        List<string> lstGroupSubjects = new List<string>();
        if (!oExportReportBL.BasicInfo.ShowGrades)
        {
            sharedStringTable1.Append(GetSharedItem("ROLL NO."));
            sharedStringTable1.Append(GetSharedItem("STUDENT NAME"));

            oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == false).OrderBy(sb => sb.SortOrder).ToList().ForEach(
                sb =>
                {
                    if (oExportReportBL.Subjects.Any(sbj => sbj.SubjectId == sb.SubjectId && sbj.ParentSubject != string.Empty))
                    {
                        if (!lstGroupSubjects.Contains(sb.ParentSubject))
                        {
                            string sName = sb.ParentSubject;
                            if (sName.ToLower() == "science")
                                sName = "SCI";
                            sharedStringTable1.Append(GetSharedItem(sName));
                            lstGroupSubjects.Add(sb.ParentSubject);
                        }
                    }
                    else
                        sharedStringTable1.Append(GetSharedItem(sb.SubjectName));
                }
                );

            sharedStringTable1.Append(GetSharedItem("BEST OF 5"));
            sharedStringTable1.Append(GetSharedItem("%"));

            sharedStringTable1.Append(GetSharedItem("RANK"));
        }

        List<string> lstSubjects = new List<string>();
        sharedStringTable1.Append(GetSharedItem("ROLL NO."));
        sharedStringTable1.Append(GetSharedItem("STUDENT NAME"));

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == false).OrderBy(sb => sb.SortOrder).ToList().ForEach(
               sb =>
               {
                   if (oExportReportBL.Subjects.Any(sbj => sbj.SubjectId == sb.SubjectId && sbj.ParentSubject != string.Empty))
                   {
                       if (!lstSubjects.Contains(sb.ParentSubject))
                       {
                           sharedStringTable1.Append(GetSharedItem(sb.ParentSubject));
                           lstSubjects.Add(sb.ParentSubject);
                       }
                   }
                   else
                       sharedStringTable1.Append(GetSharedItem(sb.SubjectName));
               }
               );

        sharedStringTable1.Append(GetSharedItem("TOTAL"));
        sharedStringTable1.Append(GetSharedItem("%"));

        sharedStringTable1.Append(GetSharedItem("RANK"));

        GetSharedStringOfOutOfMarksForPrelimReport(sharedStringTable1, lstGroupSubjects);

        aoSharedStringTablePart1.SharedStringTable = sharedStringTable1;
    }

    /// <summary>
    /// This method is used to generate out of marks sgared string.
    /// </summary>
    /// <param name="aoSharedStringTable1"></param>
    private void GetSharedStringOfOutOfMarksForPrelimReport(SharedStringTable aoSharedStringTable1, List<string> alstGroupSubjects)
    {
        aoSharedStringTable1.Append(GetSharedItem(string.Empty));
        aoSharedStringTable1.Append(GetSharedItem(string.Empty));

        List<string> lstSubjects = new List<string>();

        string sOldSubjectGroup = string.Empty;
        var oMaximumMarks = mlStudentMarkDetails.GroupBy(sm => sm.SubjectId).Select(sm => new { SubjectId = sm.Key, OutOFMarks = sm.Max(smd => smd.OutOfMarks) });

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == false).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                if (sb.ParentSubject == string.Empty)
                {
                    var oMarks = oMaximumMarks.Where(mm => mm.SubjectId == sb.SubjectId).Select(mm => mm.OutOFMarks).FirstOrDefault();
                    if (oMarks != null)
                        aoSharedStringTable1.Append(GetSharedItem(oMarks.ToString()));
                }
                else
                {
                    if (!lstSubjects.Contains(sb.ParentSubject))
                    {
                        var lstSubj = oExportReportBL.Subjects.Where(sbj => sbj.ParentSubject == sb.ParentSubject).Select(sbj => new { sbj.SubjectId, sbj.ParentSubject }).ToList();
                        var oMarks = oMaximumMarks.Join(lstSubj, x => x.SubjectId, y => y.SubjectId, (x, y) => new { x.SubjectId, x.OutOFMarks, y.ParentSubject }).Where(sm => sm.ParentSubject == sb.ParentSubject).GroupBy(gb => gb.ParentSubject).Select(sm => new { SubjectId = sm.Key, OutOFMarks = sm.Sum(smn => smn.OutOFMarks) }).FirstOrDefault();
                        if (oMarks != null)
                            aoSharedStringTable1.Append(GetSharedItem(oMarks.OutOFMarks.ToString()));

                        lstSubjects.Add(sb.ParentSubject);
                    }
                }
            }
            );

        var iOutOFMarks = oExportReportBL.StudentMarkSummary.Max(sm => sm.OutOfMarks);

        aoSharedStringTable1.Append(GetSharedItem(iOutOFMarks.ToString()));

        aoSharedStringTable1.Append(GetSharedItem("100"));

        aoSharedStringTable1.Append(GetSharedItem(string.Empty));
    }

    /// <summary>
    /// This method is used to generate worksheet part 1 content.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GeneratePartOrPrelimReport(WorksheetPart aoWorksheetPart1)
    {
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        AddSheetDetails(worksheet1);

        SetColumnWidthForPrelimReport(worksheet1);

        SheetData sheetData1 = new SheetData();

        AddReportHeader(sheetData1, true);

        int iSharedTitleIndex = AddTitleRowForPrelimReport(sheetData1);
        iSharedTitleIndex = AddOutOfMarksForPrelimReport(sheetData1, iSharedTitleIndex);

        var lstCount = oExportReportBL.StudentInfos.Select(sb => sb.OriginalDivisionId).Distinct().OrderBy(sb => sb).ToList();

        AddDataRowsForPrelimReport(sheetData1);

        worksheet1.Append(sheetData1);

        worksheet1.Append(MergeCellsForPrelimReport());

        AddPrintSettings(worksheet1, true);

        aoWorksheetPart1.Worksheet = worksheet1;
    }

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    private void SetColumnWidthForPrelimReport(Worksheet aoResultSheetData)
    {
        int iSubjectIndex = 3;

        Columns columns1 = new Columns();
        Column column1 = new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 7.00D, CustomWidth = true };
        Column column2 = new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 35.57D, CustomWidth = true };
        columns1.Append(column1);
        columns1.Append(column2);

        int iSubCount = oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).Count();
        if (oExportReportBL.BasicInfo.ShowGrades)
            iSubCount = iSubCount * 2;

        int iGroupSubjectCount = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();
        iSubjectIndex = iSubjectIndex + iSubCount + iGroupSubjectCount;

        Column column4 = new Column() { Min = (UInt32Value)3U, Max = Convert.ToUInt32(iSubjectIndex), Width = 7.29D, CustomWidth = true };
        columns1.Append(column4);
        iSubjectIndex++;

        iSubjectIndex = AddColumnWidthForSummaryColumns(iSubjectIndex, columns1);

        columns1.Append(new Column() { Min = Convert.ToUInt32(iSubjectIndex), Max = Convert.ToUInt32(iSubjectIndex), Width = 5.86D, CustomWidth = true });
        iSubjectIndex++;

        aoResultSheetData.Append(columns1);
    }

    /// <summary>
    /// This method is used to add out of marks.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <param name="aiSharedTitleIndex"></param>
    /// <returns></returns>
    private int AddOutOfMarksForPrelimReport(SheetData aoResultSheetData, int aiSharedTitleIndex)
    {
        Row row2 = new Row() { RowIndex = Convert.ToUInt32(miFirstRowNo + 1), Spans = new ListValue<StringValue>() { InnerText = "1:2" }, Height = 15.75D, CustomHeight = true };

        row2.Append(AddTitleCell("A" + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
        aiSharedTitleIndex++;

        row2.Append(AddTitleCell("B" + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.LeftHeader));
        aiSharedTitleIndex++;

        int iCellIndex = 3;
        string sCell;
        List<string> lstSubjects = new List<string>();

        //Set data for scholastic subjects.
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                if (sb.ParentSubject == string.Empty)
                {
                    sCell = ((char)(64 + iCellIndex)).ToString();
                    row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
                    iCellIndex++;
                    aiSharedTitleIndex++;
                }
                else
                {
                    if (!lstSubjects.Contains(sb.ParentSubject))
                    {
                        sCell = ((char)(64 + iCellIndex)).ToString();
                        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
                        iCellIndex++;
                        aiSharedTitleIndex++;

                        lstSubjects.Add(sb.ParentSubject);
                    }
                }
            }
            );

        sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        aiSharedTitleIndex++;

        sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        aiSharedTitleIndex++;

        // Set data for rank column.
        if (iCellIndex >= 27)
            sCell = "A" + ((char)(64 + (iCellIndex - 26))).ToString();
        else
            sCell = ((char)(64 + iCellIndex)).ToString();

        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        aiSharedTitleIndex++;

        aoResultSheetData.Append(row2);
        return aiSharedTitleIndex;
    }

    /// <summary>
    /// This method is used to add title row.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <returns></returns>
    private int AddTitleRowForPrelimReport(SheetData aoResultSheetData)
    {
        string sOldSubjectGroup = string.Empty;

        int iCellIndex = 3, iSharedIndex = 0;
        if (!oExportReportBL.BasicInfo.ShowGrades)
        {
            iSharedIndex = AddGroupHeaderForPrelimReport(aoResultSheetData);
        }

        sOldSubjectGroup = string.Empty;
        Row row2 = new Row() { RowIndex = Convert.ToUInt32(miFirstRowNo), Spans = new ListValue<StringValue>() { InnerText = "1:2" }, Height = 15.75D, CustomHeight = true };
        row2.Append(AddTitleCell("A" + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iSharedIndex++;

        row2.Append(AddTitleCell("B" + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iSharedIndex++;

        string sCell;

        List<string> lstSubjects = new List<string>();

        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                if (sb.ParentSubject == string.Empty)
                {
                    sCell = ((char)(64 + iCellIndex)).ToString();
                    row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
                    iCellIndex++;
                    iSharedIndex++;
                }
                else
                {
                    if (!lstSubjects.Contains(sb.ParentSubject))
                    {
                        sCell = ((char)(64 + iCellIndex)).ToString();
                        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
                        iCellIndex++;
                        iSharedIndex++;

                        lstSubjects.Add(sb.ParentSubject);
                    }
                }
            }
        );

        sOldSubjectGroup = string.Empty;
        sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        iSharedIndex++;

        sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        iSharedIndex++;

        if (iCellIndex >= 27)
            sCell = "A" + ((char)(64 + (iCellIndex - 26))).ToString();
        else
            sCell = ((char)(64 + iCellIndex)).ToString();

        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        iSharedIndex++;

        aoResultSheetData.Append(row2);
        return iSharedIndex;
    }

    #endregion

    #region SVP Result Sheet using OpenXml

    /// <summary>
    /// This method is used to create workbook part.
    /// </summary>
    /// <param name="aoPart"></param>
    public void CreateWorkbookPart(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        GenerateWorkbookStylesPart1Content(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateWorksheetPart1Content(worksheetPart1);

        SharedStringTablePart sharedStringTablePart1 = aoPart.AddNewPart<SharedStringTablePart>("rId4");
        GenerateSharedStringTablePart1Content(sharedStringTablePart1);

        string filePath = Server.MapPath("..") + @"\Images\Logos\School_Logo_Small.jpg";
        AddImage(filePath, worksheetPart1);

        GeneratePartContent(aoPart, false);
    }

    

    /// <summary>
    /// This method is used to generated shred string table part content.
    /// </summary>
    /// <param name="aoSharedStringTablePart1"></param>
    private void GenerateSharedStringTablePart1Content(SharedStringTablePart aoSharedStringTablePart1)
    {
        SharedStringTable sharedStringTable1 = new SharedStringTable() { Count = (UInt32Value)4U, UniqueCount = (UInt32Value)4U };

        if (!oExportReportBL.BasicInfo.ShowGrades)
        {
            sharedStringTable1.Append(GetSharedItem("ROLL NO."));
            sharedStringTable1.Append(GetSharedItem("STUDENT NAME"));
            sharedStringTable1.Append(GetSharedItem("HOUSE NAME"));

            string sOldSubjectGroup = string.Empty;
            oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == false).OrderBy(sb => sb.SortOrder).ToList().ForEach(
                sb =>
                {
                    if (sOldSubjectGroup != sb.ParentSubject)
                        sharedStringTable1.Append(GetSharedItem(sb.ParentSubject));
                    else
                    {
                        sharedStringTable1.Append(GetSharedItem(sb.SubjectName));
                        if (sOldSubjectGroup != string.Empty)
                            sharedStringTable1.Append(GetSharedItem(string.Empty));
                    }
                    sOldSubjectGroup = sb.ParentSubject;
                }
                );

            if (IsAnnualConsoldatedReportOf9thSVP)
            {
                sOldSubjectGroup = string.Empty;
                oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == true).OrderBy(sb => sb.SortOrder).ToList().ForEach(
                   sb =>
                   {
                       sharedStringTable1.Append(GetSharedItem(sb.SubjectName));
                   }
                   );

                sharedStringTable1.Append(GetSharedItem("TOTAL"));
                sharedStringTable1.Append(GetSharedItem("PER (%)"));
            }
            else
            {
                sharedStringTable1.Append(GetSharedItem("TOTAL"));
                sharedStringTable1.Append(GetSharedItem("PER (%)"));

                sOldSubjectGroup = string.Empty;
                oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == true).OrderBy(sb => sb.SortOrder).ToList().ForEach(
                   sb =>
                   {
                       sharedStringTable1.Append(GetSharedItem(sb.SubjectName));
                   }
                   );
            }
            sharedStringTable1.Append(GetSharedItem("RANK"));
        }

        sharedStringTable1.Append(GetSharedItem("ROLL NO."));
        sharedStringTable1.Append(GetSharedItem("STUDENT NAME"));
        sharedStringTable1.Append(GetSharedItem("HOUSE NAME"));

        AddSubjectShareString(sharedStringTable1, false);

        // Clumn order is different for annual consoldiated report of 9th std.
        if (IsAnnualConsoldatedReportOf9thSVP)
        {
            AddSubjectShareString(sharedStringTable1, true);
            sharedStringTable1.Append(GetSharedItem("TOTAL"));
            sharedStringTable1.Append(GetSharedItem("PER (%)"));

            if (oExportReportBL.BasicInfo.ShowGrades)
                sharedStringTable1.Append(GetSharedItem("GRADE"));
        }
        else
        {
            sharedStringTable1.Append(GetSharedItem("TOTAL"));
            sharedStringTable1.Append(GetSharedItem("PER (%)"));

            if (oExportReportBL.BasicInfo.ShowGrades)
                sharedStringTable1.Append(GetSharedItem("GRADE"));

            AddSubjectShareString(sharedStringTable1, true);
        }

        sharedStringTable1.Append(GetSharedItem("RANK"));

        if (!IsAnnualConsoldatedReportOf9thSVP)
            GetSharedStringOfOutOfMarks(sharedStringTable1);

        aoSharedStringTablePart1.SharedStringTable = sharedStringTable1;
    }

    /// <summary>
    /// This method is used to generate subject shared string.
    /// </summary>
    /// <param name="aoSharedStringTable1"></param>
    /// <param name="abIsCoCurriSubject"></param>
    private void AddSubjectShareString(SharedStringTable aoSharedStringTable1, bool abIsCoCurriSubject)
    {
        string sOldSubjectGroup = string.Empty;
        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == abIsCoCurriSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                aoSharedStringTable1.Append(GetSharedItem(sb.SubjectName));

                if (msReportID != S_ANNUAL_CONSOLDATED_REPORT || (msReportID == S_ANNUAL_CONSOLDATED_REPORT && !abIsCoCurriSubject))
                {
                    if (oExportReportBL.BasicInfo.ShowGrades)
                        aoSharedStringTable1.Append(GetSharedItem("-"));
                    else
                    {
                        if (sOldSubjectGroup != string.Empty && sOldSubjectGroup == sb.ParentSubject)
                            aoSharedStringTable1.Append(GetSharedItem("Total"));
                        sOldSubjectGroup = sb.ParentSubject;
                    }
                }
            }
            );
    }


    
    /// <summary>
    /// This method is used to generate out of marks sgared string.
    /// </summary>
    /// <param name="aoSharedStringTable1"></param>
    private void GetSharedStringOfOutOfMarks(SharedStringTable aoSharedStringTable1)
    {
        aoSharedStringTable1.Append(GetSharedItem(string.Empty));
        aoSharedStringTable1.Append(GetSharedItem(string.Empty));
        aoSharedStringTable1.Append(GetSharedItem(string.Empty));

        GetSharedStringOfSubject(aoSharedStringTable1, false);

        var oMaxMarks = oExportReportBL.StudentMarkSummary.Max(SM => SM.OutOfMarks);
        aoSharedStringTable1.Append(GetSharedItem(oMaxMarks.ToString()));
      
        aoSharedStringTable1.Append(GetSharedItem("100"));

        if (oExportReportBL.BasicInfo.ShowGrades)
            aoSharedStringTable1.Append(GetSharedItem("G"));

        GetSharedStringOfSubject(aoSharedStringTable1, true);

        aoSharedStringTable1.Append(GetSharedItem(string.Empty)); 
    }

    /// <summary>
    /// This method is used to return shred string of subjects.
    /// </summary>
    /// <param name="aoSharedStringTable1"></param>
    /// <param name="abIsCoCurriSubject"></param>
    private void GetSharedStringOfSubject(SharedStringTable aoSharedStringTable1, bool abIsCoCurriSubject)
    {
        string sOldSubjectGroup = string.Empty;
        var oMaxMarks = mlStudentMarkDetails.GroupBy(sm => sm.SubjectId).Select(sm => new { SubjectId = sm.Key, OutOFMarks = sm.Max(smd => smd.OutOfMarks) });
        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == abIsCoCurriSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                var oMarks = oMaxMarks.Where(mm => mm.SubjectId == sb.SubjectId).Select(mm => mm.OutOFMarks).FirstOrDefault();

                if ((msReportID == S_ANNUAL_CONSOLDATED_REPORT && !abIsCoCurriSubject) || msReportID != S_ANNUAL_CONSOLDATED_REPORT)
                {
                    if (oMarks != null)
                        aoSharedStringTable1.Append(GetSharedItem(oMarks.ToString()));
                    else
                        aoSharedStringTable1.Append(GetSharedItem("M"));
                }

                if (oExportReportBL.BasicInfo.ShowGrades || (msReportID == S_ANNUAL_CONSOLDATED_REPORT && abIsCoCurriSubject))
                    aoSharedStringTable1.Append(GetSharedItem("G"));
                else
                {
                    if (sOldSubjectGroup != string.Empty && sOldSubjectGroup == sb.ParentSubject)
                    {
                        var oGroupSubjectId = oExportReportBL.Subjects.Where(sbj => sbj.ParentSubject == sb.ParentSubject && sbj.SubjectId != sb.SubjectId).Select(sbj => sbj.SubjectId).FirstOrDefault();
                        if (oGroupSubjectId != null)
                        {
                            var oMarks1 = oMaxMarks.Where(mm => mm.SubjectId == oGroupSubjectId).Select(mm => mm.OutOFMarks).FirstOrDefault();
                            if (oMarks1 != null)
                            {
                                var oDropDownList = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
                                if (msReportID != S_ANNUAL_CONSOLDATED_REPORT || (msReportID == S_ANNUAL_CONSOLDATED_REPORT && oDropDownList.SelectedItem.Text != "9") || ((msReportID == S_ANNUAL_CONSOLDATED_REPORT && oDropDownList.SelectedItem.Text != "9" && sb.ParentSubject != "S.S.T.")))
                                    aoSharedStringTable1.Append(GetSharedItem((oMarks + oMarks1).ToString()));
                                else
                                    aoSharedStringTable1.Append(GetSharedItem("100"));
                            }
                        }
                    }
                    sOldSubjectGroup = sb.ParentSubject;
                }
            }
            );
    }


    

    /// <summary>
    /// This method is used to generate worksheet part 1 content.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateWorksheetPart1Content(WorksheetPart aoWorksheetPart1)
    {
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        AddSheetDetails(worksheet1);

        SetColumnWidth(worksheet1);

        SheetData sheetData1 = new SheetData();

        AddReportHeader(sheetData1, false);

        int iSharedTitleIndex = AddTitleRow(sheetData1);

        if (!IsAnnualConsoldatedReportOf9thSVP)
            iSharedTitleIndex = AddOutOfMarksForOpenXml(sheetData1, iSharedTitleIndex);

        AddDataRows(sheetData1);

        AddSignatures(sheetData1,false);

        worksheet1.Append(sheetData1);

        worksheet1.Append(MergeCells());

        AddPrintSettings(worksheet1, false);

        aoWorksheetPart1.Worksheet = worksheet1;
    }

    /// <summary>
    /// This method is sued to add sheet details.
    /// </summary>
    /// <param name="worksheet1"></param>
    private void AddSheetDetails(Worksheet worksheet1)
    {
        AddSheetProperties(worksheet1);

        AddSheedDimension(worksheet1);

        AddSheetView(worksheet1);

        AddFormatProperties(worksheet1);
    }

    /// <summary>
    /// This method is sued to add print settings.
    /// </summary>
    /// <param name="worksheet1"></param>
    private void AddPrintSettings(Worksheet worksheet1, bool abIsPrelimReport)
    {
        AddPrintOptions(worksheet1);

        if (!abIsPrelimReport)
        {
            SetPageMargin(worksheet1,0.45);
            SetPageSetup(worksheet1, OrientationValues.Landscape);
        }
        else
        {
            SetPageMargin(worksheet1, 0.45);
            SetPageSetup(worksheet1, OrientationValues.Portrait);
        }

        GenerateHeaderFooter(worksheet1);

        if (abIsPrelimReport)
            AddPageBreak(worksheet1);
    }

    /// <summary>
    /// This method is used to add page break.
    /// </summary>
    /// <param name="worksheet1"></param>
    private void AddPageBreak(Worksheet worksheet1)
    {
        int StudentCount = oExportReportBL.StudentInfos.Count(st => st.OriginalDivisionId == 1);
        int iCnt = miFirstRowNo + 1 + StudentCount + miRowsBeforeSignSection + 1;
        RowBreaks rowBreaks1 = new RowBreaks() { Count = (UInt32Value)1U, ManualBreakCount = (UInt32Value)1U };
        Break break1 = new Break() { Id = Convert.ToUInt32(iCnt), Max = (UInt32Value)16383U, ManualPageBreak = true };
        rowBreaks1.Append(break1);
        worksheet1.Append(rowBreaks1);
    }

    /// <summary>
    /// This method is used to add report header.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    private void AddReportHeader(SheetData aoResultSheetData, bool abIsPrelimReport)
    {
        int iGroupSubjectCount = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();

        int iTotalCells = 0;

        if (!abIsPrelimReport)
            iTotalCells = 3 + oExportReportBL.Subjects.Count + 4 + iGroupSubjectCount;
        else
            iTotalCells = 2 + oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject && sb.ParentSubject == string.Empty) + 3 + iGroupSubjectCount;
        
        AddSchoolName(aoResultSheetData, iTotalCells);
        AddTestName(aoResultSheetData, iTotalCells, abIsPrelimReport);
        AddClass(aoResultSheetData, iTotalCells, abIsPrelimReport);
    }

    /// <summary>
    /// This method is used to add class.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <param name="aiTotalCells"></param>
    private void AddClass(SheetData aoResultSheetData, int aiTotalCells, bool abIsPrelimReport)
    {
        Row row0 = new Row { RowIndex = Convert.ToUInt32(miSchoolNameRowIndex + 2), Height = 6D, CustomHeight = true };
        aoResultSheetData.AppendChild(row0);

        Row row2 = new Row { RowIndex = Convert.ToUInt32(miSchoolNameRowIndex + 3), Height = 15D, CustomHeight = true };

        string sClass = (abIsPrelimReport ? "STD. " + oExportReportBL.BasicInfo.ClassName : "CLASS : " + oExportReportBL.BasicInfo.ClassName);

        for (int iIndex = 0; iIndex < aiTotalCells; iIndex++)
        {
            if (iIndex == 0)
                row2.Append(ConstructCell(sClass, CellValues.String, CellAlignment.RightHeaderClass));
            else
                row2.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.RightHeaderClass));
        }

        aoResultSheetData.AppendChild(row2);
    }

    /// <summary>
    /// This method is used to add test name and academic year.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <param name="aiTotalCells"></param>
    private void AddTestName(SheetData aoResultSheetData, int aiTotalCells, bool abIsPrelimReport)
    {
        Row row1 = new Row { RowIndex = Convert.ToUInt32(miSchoolNameRowIndex + 1), Height = 20D, CustomHeight = true };

        string sHeader = (abIsPrelimReport || msReportID == S_ANNUAL_CONSOLDATED_REPORT ? string.Empty : "RESULT OF " );
        for (int iIndex = 0; iIndex < aiTotalCells; iIndex++)
        {
            if (iIndex == 0)
                row1.Append(ConstructCell(sHeader + oExportReportBL.BasicInfo.TestName + "      " + oExportReportBL.BasicInfo.AcademicYear, CellValues.String, CellAlignment.CenterHeaderYear));
            else
                row1.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterHeaderYear));
        }
        aoResultSheetData.AppendChild(row1);
    }

    /// <summary>
    /// This method is used to add school name.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <param name="aiTotalCells"></param>
    /// <returns></returns>
    private int AddSchoolName(SheetData aoResultSheetData, int aiTotalCells)
    {
        Row row0 = new Row { RowIndex = Convert.ToUInt32(miSchoolNameRowIndex), Height = 20D, CustomHeight = true };

        for (int iIndex = 0; iIndex < aiTotalCells; iIndex++)
        {
            if (iIndex == 0)
                row0.Append(ConstructCell(oExportReportBL.BasicInfo.SchoolName + ", " + oExportReportBL.BasicInfo.Location, CellValues.String, CellAlignment.ReportHeader));
            else
                row0.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.ReportHeader));
        }

        aoResultSheetData.AppendChild(row0);
        return aiTotalCells;
    }

    /// <summary>
    /// This method is used to add signatures.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    private void AddSignatures(SheetData aoResultSheetData, bool abIsPrelimReport)
    {
        for (int iIndex = 0; iIndex < miRowsBeforeSignSection; iIndex++)
        {
            Row row0 = new Row();
            aoResultSheetData.Append(row0);
        }

        int iSignRowIndex = miFirstRowNo + 2 + oExportReportBL.StudentInfos.Count + miRowsBeforeSignSection;
        
        Row row = new Row() { RowIndex = Convert.ToUInt32(iSignRowIndex) };
        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataWithNoBorder));
        row.Append(ConstructCell("CLASS TEACHER", CellValues.String, CellAlignment.LeftDataWithNoBorder));        
        row.Append(ConstructCell("COORDINATOR", CellValues.String, CellAlignment.CenterDataWithNoBorder));

        int iSubCount = oExportReportBL.Subjects.Count;
        if (oExportReportBL.BasicInfo.ShowGrades)
        {
            if (msReportID != S_ANNUAL_CONSOLDATED_REPORT)
                iSubCount = iSubCount * 2;
            else
                iSubCount = oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * 2 + oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject) + 1;
        }
        else
        {
            if (!abIsPrelimReport)
                iSubCount = iSubCount + oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();
            else
                iSubCount = oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject && sb.ParentSubject == string.Empty) + oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count() - 1;
        }

        for (int iIndex = 0; iIndex < iSubCount; iIndex++)
            row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataWithNoBorder));

        row.Append(ConstructCell("PRINCIPAL", CellValues.String, CellAlignment.CenterDataWithNoBorder));

        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataWithNoBorder));
        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataWithNoBorder));

       if (!abIsPrelimReport)
        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterDataWithNoBorder));

        aoResultSheetData.Append(row);
    }

    

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    private void SetColumnWidth(Worksheet aoResultSheetData)
    {
        int iSubjectIndex = 3;
        Columns columns1 = SetWidthForBasicColumns();

        int iSubCount = oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).Count();
        if (oExportReportBL.BasicInfo.ShowGrades)
            iSubCount = iSubCount * 2;

        int iGroupSubjectCount = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();
        iSubjectIndex = iSubjectIndex + iSubCount + iGroupSubjectCount;

        Column column4 = new Column() { Min = (UInt32Value)4U, Max = Convert.ToUInt32(iSubjectIndex), Width = 7.29D, CustomWidth = true };
        columns1.Append(column4);
        iSubjectIndex++;

        iSubjectIndex = AddColumnWidthForSummaryColumns(iSubjectIndex, columns1);

        int iCoSubCount = oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).Count();
        if (oExportReportBL.BasicInfo.ShowGrades)
            iCoSubCount = iCoSubCount * 2;

        columns1.Append(new Column() { Min = Convert.ToUInt32(iSubjectIndex), Max = Convert.ToUInt32(iSubjectIndex + iCoSubCount), Width = 7.29D, CustomWidth = true });
        iSubjectIndex = iSubjectIndex + iCoSubCount;

        columns1.Append(new Column() { Min = Convert.ToUInt32(iSubjectIndex), Max = Convert.ToUInt32(iSubjectIndex), Width = 5.86D, CustomWidth = true });
        iSubjectIndex++;

        aoResultSheetData.Append(columns1);
    }

    /// <summary>
    /// This method is used to set column width for summary columns.
    /// </summary>
    /// <param name="aiSubjectIndex"></param>
    /// <param name="oColumns"></param>
    /// <returns></returns>
    private static int AddColumnWidthForSummaryColumns(int aiSubjectIndex, Columns oColumns)
    {
        oColumns.Append(new Column() { Min = Convert.ToUInt32(aiSubjectIndex), Max = Convert.ToUInt32(aiSubjectIndex), Width = 7.29D, CustomWidth = true });
        aiSubjectIndex++;

        oColumns.Append(new Column() { Min = Convert.ToUInt32(aiSubjectIndex), Max = Convert.ToUInt32(aiSubjectIndex), Width = 7.5D, CustomWidth = true });
        aiSubjectIndex++;

        oColumns.Append(new Column() { Min = Convert.ToUInt32(aiSubjectIndex), Max = Convert.ToUInt32(aiSubjectIndex), Width = 7D, CustomWidth = true });
        aiSubjectIndex++;
        return aiSubjectIndex;
    }

    /// <summary>
    /// This method is used to set column width for basic columns.
    /// </summary>
    /// <returns></returns>
    private static Columns SetWidthForBasicColumns()
    {
        Columns columns1 = new Columns();
        Column column1 = new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 9.43D, CustomWidth = true };
        Column column2 = new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 35.57D, CustomWidth = true };
        Column column3 = new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 13.86D, CustomWidth = true };
        columns1.Append(column1);
        columns1.Append(column2);
        columns1.Append(column3);
        return columns1;
    }


    

    /// <summary>
    /// This method is used to add out of marks.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <param name="aiSharedTitleIndex"></param>
    /// <returns></returns>
    private int AddOutOfMarksForOpenXml(SheetData aoResultSheetData, int aiSharedTitleIndex)
    {
        Row row2 = new Row() { RowIndex = Convert.ToUInt32(miFirstRowNo + 1), Spans = new ListValue<StringValue>() { InnerText = "1:2" }, Height = 15.75D, CustomHeight = true };

        // Set data for basic columns.
        row2.Append(AddTitleCell("A" + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
        aiSharedTitleIndex++;

        row2.Append(AddTitleCell("B" + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.LeftHeader));
        aiSharedTitleIndex++;

        row2.Append(AddTitleCell("C" + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.LeftHeader));
        aiSharedTitleIndex++;

        int iCellIndex = 4;
        string sCell;

        string sOldSubjectGroup = string.Empty;
        //Set data for scholastic subjects.
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                sCell = ((char)(64 + iCellIndex)).ToString();
                row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
                iCellIndex++;
                aiSharedTitleIndex++;

                if (oExportReportBL.BasicInfo.ShowGrades)
                {
                    sCell = ((char)(64 + iCellIndex)).ToString();
                    row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
                    iCellIndex++;
                    aiSharedTitleIndex++;
                }
                else
                {
                    if (sOldSubjectGroup != string.Empty && sOldSubjectGroup == sb.ParentSubject)
                    {
                        sCell = ((char)(64 + iCellIndex)).ToString();
                        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
                        iCellIndex++;
                        aiSharedTitleIndex++;
                    }
                    sOldSubjectGroup = sb.ParentSubject;
                }
            }
            );

        sOldSubjectGroup = string.Empty;
        // Set data for scholastic subjects.
        sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        aiSharedTitleIndex++;

        sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        aiSharedTitleIndex++;

        if (oExportReportBL.BasicInfo.ShowGrades)
        {
            sCell = ((char)(64 + iCellIndex)).ToString();
            row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
            iCellIndex++;
            aiSharedTitleIndex++;
        }

        //Set data for co-curricular subjects.
        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                sCell = ((char)(64 + iCellIndex)).ToString();
                row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
                iCellIndex++;
                aiSharedTitleIndex++;

                if (msReportID != S_ANNUAL_CONSOLDATED_REPORT)
                {
                    if (oExportReportBL.BasicInfo.ShowGrades)
                    {
                        sCell = ((char)(64 + iCellIndex)).ToString();
                        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
                        iCellIndex++;
                        aiSharedTitleIndex++;
                    }
                }
            }
            );

        // Set data for rank column.
        if (iCellIndex >= 27)
            sCell = "A" + ((char)(64 + (iCellIndex - 26))).ToString();
        else
            sCell = ((char)(64 + iCellIndex)).ToString();

        row2.Append(AddTitleCell(sCell + (miFirstRowNo + 1), aiSharedTitleIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        aiSharedTitleIndex++;

        aoResultSheetData.Append(row2);
        return aiSharedTitleIndex;
    }


    

    /// <summary>
    /// This method is used to add title row.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <returns></returns>
    private int AddTitleRow(SheetData aoResultSheetData)
    {
        string sOldSubjectGroup = string.Empty;

        int iCellIndex = 4, iSharedIndex = 0;
        if (!oExportReportBL.BasicInfo.ShowGrades)
        {
            iSharedIndex = AddGroupHeader(aoResultSheetData);
        }

        sOldSubjectGroup = string.Empty;
        Row row2 = new Row() { RowIndex = Convert.ToUInt32(miFirstRowNo), Spans = new ListValue<StringValue>() { InnerText = "1:2" }, Height = 15.75D, CustomHeight = true };
        row2.Append(AddTitleCell("A" + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iSharedIndex++;

        row2.Append(AddTitleCell("B" + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iSharedIndex++;

        row2.Append(AddTitleCell("C" + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iSharedIndex++;

        string sCell;
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                sCell = ((char)(64 + iCellIndex)).ToString();
                row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
                iCellIndex++;
                iSharedIndex++;

                if (oExportReportBL.BasicInfo.ShowGrades)
                {
                    sCell = ((char)(64 + iCellIndex)).ToString();
                    row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
                    iCellIndex++;
                    iSharedIndex++;
                }
                else
                {
                    if (sOldSubjectGroup != string.Empty && sOldSubjectGroup == sb.ParentSubject)
                    {
                        sCell = ((char)(64 + iCellIndex)).ToString();
                        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
                        iCellIndex++;
                        iSharedIndex++;
                    }
                    sOldSubjectGroup = sb.ParentSubject;
                }
            }
            );

        sOldSubjectGroup = string.Empty;
        sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        iSharedIndex++;

        sCell = ((char)(64 + iCellIndex)).ToString();
        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        iSharedIndex++;

        if (oExportReportBL.BasicInfo.ShowGrades)
        {
            sCell = ((char)(64 + iCellIndex)).ToString();
            row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
            iCellIndex++;
            iSharedIndex++;
        }

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                sCell = ((char)(64 + iCellIndex)).ToString();
                row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
                iCellIndex++;
                iSharedIndex++;

                if (msReportID != S_ANNUAL_CONSOLDATED_REPORT)
                {
                    if (oExportReportBL.BasicInfo.ShowGrades)
                    {
                        sCell = ((char)(64 + iCellIndex)).ToString();
                        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
                        iCellIndex++;
                        iSharedIndex++;
                    }
                    else
                    {
                        if (sOldSubjectGroup != string.Empty && sOldSubjectGroup == sb.ParentSubject)
                        {
                            sCell = ((char)(64 + iCellIndex)).ToString();
                            row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
                            iCellIndex++;
                            iSharedIndex++;
                        }
                        sOldSubjectGroup = sb.ParentSubject;
                    }
                }
            }
            );

        if (iCellIndex >= 27)
            sCell = "A" + ((char)(64 + (iCellIndex - 26))).ToString();
        else
            sCell = ((char)(64 + iCellIndex)).ToString();

        row2.Append(AddTitleCell(sCell + (miFirstRowNo), iSharedIndex.ToString(), CellAlignment.CenterHeader));
        iCellIndex++;
        iSharedIndex++;

        aoResultSheetData.Append(row2);
        return iSharedIndex;
    }

    /// <summary>
    /// This method is used to add group header.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <returns></returns>
    private int AddGroupHeaderForPrelimReport(SheetData aoResultSheetData)
    {
        int iCellIndex = 3, iIncrIndex = -1;
        string sOldSubjectGroup = string.Empty, sCell;
        Row row1 = new Row() { RowIndex = Convert.ToUInt32(miFirstRowNo - 1), Spans = new ListValue<StringValue>() { InnerText = "1:2" }, Height = 15.75D, CustomHeight = true };
        row1.Append(AddTitleCell("A" + (miFirstRowNo - 1), "0", CellAlignment.CenterHeader));
        row1.Append(AddTitleCell("B" + (miFirstRowNo - 1), "1", CellAlignment.CenterHeader));
        
        List<string> lstSubjects = new List<string>();
        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == false).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                if (sb.ParentSubject == string.Empty)
                {
                    sCell = ((char)(64 + iCellIndex)).ToString();
                    row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
                    iCellIndex++;
                }
                else
                {
                    if (!lstSubjects.Contains(sb.ParentSubject))
                    {
                        sCell = ((char)(64 + iCellIndex)).ToString();
                        row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
                        iCellIndex++;

                        lstSubjects.Add(sb.ParentSubject);
                    }
                }
            }
            );

        sCell = ((char)(64 + iCellIndex)).ToString();
        row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
        iCellIndex++;

        sCell = ((char)(64 + iCellIndex)).ToString();
        row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
        iCellIndex++;

        sCell = ((char)(64 + iCellIndex)).ToString();
        row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));

        aoResultSheetData.Append(row1);
        return iCellIndex;
    }

    /// <summary>
    /// This method is used to add group header.
    /// </summary>
    /// <param name="aoResultSheetData"></param>
    /// <returns></returns>
    private int AddGroupHeader(SheetData aoResultSheetData)
    {
        int iCellIndex = 4, iIncrIndex = -1;
        string sOldSubjectGroup = string.Empty, sCell;
        Row row1 = new Row() { RowIndex = Convert.ToUInt32(miFirstRowNo - 1), Spans = new ListValue<StringValue>() { InnerText = "1:2" }, Height = 15.75D, CustomHeight = true };
        row1.Append(AddTitleCell("A" + (miFirstRowNo - 1), "0", CellAlignment.CenterHeader));
        row1.Append(AddTitleCell("B" + (miFirstRowNo - 1), "1", CellAlignment.CenterHeader));
        row1.Append(AddTitleCell("C" + (miFirstRowNo - 1), "2", CellAlignment.CenterHeader));

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == false).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                sCell = ((char)(64 + iCellIndex)).ToString();
                row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
                iCellIndex++;

                if (sOldSubjectGroup == sb.ParentSubject)
                {
                    if (sOldSubjectGroup != string.Empty)
                    {
                        sCell = ((char)(64 + iCellIndex)).ToString();
                        row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
                        iCellIndex++;
                    }
                }

                sOldSubjectGroup = sb.ParentSubject;
            }
            );

        sCell = ((char)(64 + iCellIndex)).ToString();
        row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
        iCellIndex++;

        sCell = ((char)(64 + iCellIndex)).ToString();
        row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
        iCellIndex++;

        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == true).OrderBy(sb => sb.SortOrder).ToList().ForEach(
           sb =>
           {
               sCell = ((char)(64 + iCellIndex)).ToString();
               row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));
               iCellIndex++;
           }
           );

        sCell = ((char)(64 + iCellIndex)).ToString();
        row1.Append(AddTitleCell(sCell + (miFirstRowNo - 1), (iCellIndex + iIncrIndex).ToString(), CellAlignment.CenterHeader));

        aoResultSheetData.Append(row1);
        return iCellIndex;
    }

    /// <summary>
    /// This method is used to generate part contents.
    /// </summary>
    /// <param name="aoPart"></param>
    private void GeneratePartContent(WorkbookPart aoPart, bool abIsPrelimReport)
    {
        Workbook workbook1 = new Workbook();
        workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        AddFileVersion(workbook1);

        AddWorkbookProperties(workbook1);

        AddBookViews(workbook1);

        AddSheets(workbook1);

        AddDefinedNames(workbook1, abIsPrelimReport);

        AddCalculationProperties(workbook1);

        aoPart.Workbook = workbook1;
    }

    /// <summary>
    /// This method is used to set defines names.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    private void AddDefinedNames(Workbook aoWorkbook, bool abIsPrelimReport)
    {
        DefinedNames definedNames1 = new DefinedNames();
        DefinedName definedName1 = new DefinedName() { Name = "_xlnm.Print_Titles", LocalSheetId = (UInt32Value)0U };

        if (!abIsPrelimReport)
        {
            int iStartRow = 0, iEndRow = 0;
            if (oExportReportBL.BasicInfo.ShowGrades)
            {
                iStartRow = miFirstRowNo;
                iEndRow = miFirstRowNo + 1;
            }
            else
            {
                iStartRow = miFirstRowNo - 1;
                iEndRow = miFirstRowNo + 1;
            }

            definedName1.Text = S_SHEET_NAME + "!$" + iStartRow + ":$" + iEndRow;
        }
        else
        {
            definedName1.Text = S_SHEET_NAME + "!$" + 1 + ":$" + (miFirstRowNo + 1);
        }

        definedNames1.Append(definedName1);
        aoWorkbook.Append(definedNames1);
    }

    /// <summary>
    /// This method is used to merge cells.
    /// </summary>
    /// <returns></returns>
    private MergeCells MergeCellsForPrelimReport()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        MergeSignCellsForPrelim(mergeCells1, true);

        MergeHeaderCells(mergeCells1, true);

        MergeGroupCellsForPrelim(mergeCells1);

        return mergeCells1;
    }

    /// <summary>
    /// This method is used to merge cells.
    /// </summary>
    /// <returns></returns>
    private MergeCells MergeCells()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        if (oExportReportBL.BasicInfo.ShowGrades)
        {
            string sCell1, sCell2;
            int iFirstCellIndex = 4;
            // Merge scholastic subjects.
            oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
               sb =>
               {
                   sCell1 = ((char)(64 + iFirstCellIndex)).ToString();
                   sCell2 = ((char)(64 + (iFirstCellIndex + 1))).ToString();
                   MergeCell mergeCell0 = new MergeCell() { Reference = sCell1 + miFirstRowNo + ":" + sCell2 + miFirstRowNo };
                   mergeCells1.Append(mergeCell0);

                   iFirstCellIndex = iFirstCellIndex + 2;
               }
               );

            iFirstCellIndex = iFirstCellIndex + 3;

            if (msReportID != S_ANNUAL_CONSOLDATED_REPORT)
            {
                //Merge co-curricular subjects.
                oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
                   sb =>
                   {
                       if (iFirstCellIndex >= 27)
                           sCell1 = "A" + ((char)(64 + (iFirstCellIndex - 26))).ToString();
                       else
                           sCell1 = ((char)(64 + iFirstCellIndex)).ToString();

                       if (iFirstCellIndex >= 27)
                           sCell2 = "A" + ((char)(64 + (iFirstCellIndex - 27))).ToString();
                       else
                           sCell2 = ((char)(64 + (iFirstCellIndex + 1))).ToString();

                       MergeCell mergeCell0 = new MergeCell() { Reference = sCell1 + miFirstRowNo + ":" + sCell2 + miFirstRowNo };
                       mergeCells1.Append(mergeCell0);

                       iFirstCellIndex = iFirstCellIndex + 2;
                   }
                   );
            }
        }

        MergeSignCells(mergeCells1, false);

        MergeHeaderCells(mergeCells1, false);

        MergeGroupCells(mergeCells1);

        return mergeCells1;
    }

    /// <summary>
    /// This method is used to merge group cells.
    /// </summary>
    /// <param name="aoMergeCells"></param>
    private void MergeGroupCellsForPrelim(MergeCells aoMergeCells)
    {
        int iCellIndex = 3;

        string sCell1, sCell2;
        aoMergeCells.Append(new MergeCell() { Reference = "A" + (miFirstRowNo - 1) + ":" + "A" + (miFirstRowNo + 1) });
        aoMergeCells.Append(new MergeCell() { Reference = "B" + (miFirstRowNo - 1) + ":" + "B" + (miFirstRowNo + 1) });

        List<string> lstSubjects = new List<string>();
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
        (
            sb =>
            {
                if (sb.ParentSubject == string.Empty)
                {
                    sCell1 = ((char)(64 + iCellIndex)).ToString();
                    MergeCell mergecell0 = new MergeCell() { Reference = sCell1 + (miFirstRowNo - 1) + ":" + sCell1 + (miFirstRowNo) };
                    aoMergeCells.Append(mergecell0);
                    iCellIndex = iCellIndex + 1;
                }
                else
                {
                    if (!lstSubjects.Contains(sb.ParentSubject))
                    {
                        sCell1 = ((char)(64 + iCellIndex)).ToString();
                        MergeCell mergecell0 = new MergeCell() { Reference = sCell1 + (miFirstRowNo - 1) + ":" + sCell1 + (miFirstRowNo) };
                        aoMergeCells.Append(mergecell0);
                        iCellIndex = iCellIndex + 1;

                        lstSubjects.Add(sb.ParentSubject);
                    }
                }
            }
            );


        sCell1 = ((char)(64 + iCellIndex)).ToString();
        sCell2 = ((char)(64 + iCellIndex+2)).ToString();
        MergeCell mergecell1 = new MergeCell() { Reference = sCell1 + (miFirstRowNo - 1) + ":" + sCell2 + (miFirstRowNo - 1)};
        aoMergeCells.Append(mergecell1);

        sCell1 = ((char)(64 + iCellIndex + 1)).ToString();
        MergeCell percentage = new MergeCell() { Reference = sCell1 + (miFirstRowNo) + ":" + sCell1 + (miFirstRowNo + 1) };
        aoMergeCells.Append(percentage);

        sCell1 = ((char)(64 + iCellIndex + 2)).ToString();
        MergeCell rank = new MergeCell() { Reference = sCell1 + (miFirstRowNo) + ":" + sCell1 + (miFirstRowNo + 1) };
        aoMergeCells.Append(rank);
    }

    /// <summary>
    /// This method is used to merge group cells.
    /// </summary>
    /// <param name="aoMergeCells"></param>
    private void MergeGroupCells(MergeCells aoMergeCells)
    {
        int iCellIndex = 4;
        string sOldGroupName = string.Empty, sCell1, sCell2;
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach(
            sb =>
            {
                if (sOldGroupName != sb.ParentSubject)
                {
                    sCell1 = ((char)(64 + iCellIndex)).ToString();
                    sCell2 = ((char)(64 + iCellIndex + 2)).ToString();
                    MergeCell mergeCell0 = new MergeCell() { Reference = sCell1 + (miFirstRowNo - 1) + ":" + sCell2 + (miFirstRowNo - 1) };
                    aoMergeCells.Append(mergeCell0);
                    iCellIndex = iCellIndex + 1;
                }
                iCellIndex = iCellIndex + 1;
                sOldGroupName = sb.ParentSubject;
            }
            );

        int iMergeRowValue = 0;
        if (IsAnnualConsoldatedReportOf9thSVP)
            iMergeRowValue = miFirstRowNo;
        else
            iMergeRowValue = miFirstRowNo + 1;

        if (!oExportReportBL.BasicInfo.ShowGrades)
        {
            aoMergeCells.Append(new MergeCell() { Reference = "A" + (miFirstRowNo - 1) + ":" + "A" + iMergeRowValue });
            aoMergeCells.Append(new MergeCell() { Reference = "B" + (miFirstRowNo - 1) + ":" + "B" + iMergeRowValue });
            aoMergeCells.Append(new MergeCell() { Reference = "C" + (miFirstRowNo - 1) + ":" + "C" + iMergeRowValue });

            string sOldSubjectGroup = string.Empty, sCell3;
            int iIndex = 4;
            oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
                (
                    sb =>
                    {
                        sCell3 = ((char)(64 + iIndex)).ToString();
                        if (sb.ParentSubject == string.Empty)
                            aoMergeCells.Append(new MergeCell() { Reference = sCell3 + (miFirstRowNo - 1) + ":" + sCell3 + miFirstRowNo });
                        iIndex++;
                    }
                );

            iIndex = iIndex + 3;

            sCell3 = ((char)(64 + iIndex)).ToString();
            aoMergeCells.Append(new MergeCell() { Reference = sCell3 + (miFirstRowNo - 1) + ":" + sCell3 + miFirstRowNo });
            iIndex++;

            sCell3 = ((char)(64 + iIndex)).ToString();
            aoMergeCells.Append(new MergeCell() { Reference = sCell3 + (miFirstRowNo - 1) + ":" + sCell3 + miFirstRowNo });
            iIndex++;

            oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
                (
                    sb =>
                    {
                        sCell3 = ((char)(64 + iIndex)).ToString();
                        if (sb.ParentSubject == string.Empty)
                            aoMergeCells.Append(new MergeCell() { Reference = sCell3 + (miFirstRowNo - 1) + ":" + sCell3 + miFirstRowNo });
                        iIndex++;
                    }
                );

            sCell3 = ((char)(64 + iIndex)).ToString();
            aoMergeCells.Append(new MergeCell() { Reference = sCell3 + (miFirstRowNo - 1) + ":" + sCell3 + iMergeRowValue });
        }
        else
        {
            aoMergeCells.Append(new MergeCell() { Reference = "A" + miFirstRowNo + ":" + "A" + iMergeRowValue });
            aoMergeCells.Append(new MergeCell() { Reference = "B" + miFirstRowNo + ":" + "B" + iMergeRowValue });
            aoMergeCells.Append(new MergeCell() { Reference = "C" + miFirstRowNo + ":" + "C" + iMergeRowValue });

            int iFintCount = 0;
            if(msReportID != S_ANNUAL_CONSOLDATED_REPORT)
                iFintCount = 3 + (oExportReportBL.Subjects.Count * 2) + 4;
            else
                iFintCount = 3 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * 2) + oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject) + 4;

            string sCell3 = string.Empty;
            if (iFintCount >= 27)
                sCell3 = "A" + ((char)(64 + (iFintCount - 26))).ToString();
            else
                sCell3 = ((char)(64 + iFintCount)).ToString();

            aoMergeCells.Append(new MergeCell() { Reference = sCell3 + (miFirstRowNo) + ":" + sCell3 + iMergeRowValue });
        }
    }

    /// <summary>
    /// This method is used to merge header cells.
    /// </summary>
    /// <param name="aoMergeCells"></param>
    private void MergeHeaderCells(MergeCells aoMergeCells, bool abIsPrelimReport)
    {
        string sCell2;
        int iGroupSubjectCount = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();

        int iInd = 0;

        if (!abIsPrelimReport)
        {
            if(msReportID != S_ANNUAL_CONSOLDATED_REPORT)
                iInd = 3 + (oExportReportBL.Subjects.Count * (oExportReportBL.BasicInfo.ShowGrades ? 2 : 1)) + (oExportReportBL.BasicInfo.ShowGrades ? 4 : 3) + iGroupSubjectCount;
            else
                iInd = 3 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * (oExportReportBL.BasicInfo.ShowGrades ? 2 : 1)) + (oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject)) + (oExportReportBL.BasicInfo.ShowGrades ? 4 : 3) + iGroupSubjectCount;
        }
        else
            iInd = 2 + oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject && sb.ParentSubject == string.Empty) + 3 + iGroupSubjectCount;
                
        if (iInd >= 27)
            sCell2 = "A" + ((char)(64 + (iInd - 26))).ToString();
        else
            sCell2 = ((char)(64 + iInd)).ToString();

        MergeCell mergeCell3 = new MergeCell() { Reference = "A" + miSchoolNameRowIndex + ":" + sCell2 + miSchoolNameRowIndex };
        aoMergeCells.Append(mergeCell3);

        MergeCell mergeCell4 = new MergeCell() { Reference = "A" + (miSchoolNameRowIndex + 1) + ":" + sCell2 + (miSchoolNameRowIndex + 1) };
        aoMergeCells.Append(mergeCell4);

        MergeCell mergeCell5 = new MergeCell() { Reference = "A" + (miSchoolNameRowIndex + 3) + ":" + sCell2 + (miSchoolNameRowIndex + 3) };
        aoMergeCells.Append(mergeCell5);
    }

    /// <summary>
    /// This method is used to merge signature cells.
    /// </summary>
    /// <param name="aoMergeCells"></param>
    private void MergeSignCellsForPrelim(MergeCells aoMergeCells, bool abIsPrelimReport)
    {
        string sCell1, sCell2;
        int iPrincipalCellIndex = 0, iGroupSubjectCount;
        iGroupSubjectCount = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();

        iPrincipalCellIndex = 1 + oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject && sb.ParentSubject == string.Empty) + 2 + iGroupSubjectCount;
        
        int iStudCount = oExportReportBL.StudentInfos.Count(sb => sb.OriginalDivisionId == 1);
        int iSignRowIndex = miFirstRowNo + 2 + iStudCount + miRowsBeforeSignSection;

        sCell1 = ((char)(64 + 3)).ToString();
        sCell2 = ((char)(64 + (iPrincipalCellIndex - 1))).ToString();
        MergeCell mergeCell1 = new MergeCell() { Reference = sCell1 + iSignRowIndex + ":" + sCell2 + iSignRowIndex };
        aoMergeCells.Append(mergeCell1);


        sCell1 = ((char)(64 + iPrincipalCellIndex)).ToString();
        sCell2 = ((char)(64 + iPrincipalCellIndex + 2)).ToString();
        MergeCell mergeCell2 = new MergeCell() { Reference = sCell1 + iSignRowIndex + ":" + sCell2 + iSignRowIndex };
        aoMergeCells.Append(mergeCell2);


        iStudCount = oExportReportBL.StudentInfos.Count();
        iSignRowIndex = miFirstRowNo + 2 + iStudCount + miRowsBeforeSignSection + 4;

        sCell1 = ((char)(64 + 3)).ToString();
        sCell2 = ((char)(64 + (iPrincipalCellIndex - 1))).ToString();
        MergeCell mergeCell3 = new MergeCell() { Reference = sCell1 + iSignRowIndex + ":" + sCell2 + iSignRowIndex };
        aoMergeCells.Append(mergeCell3);


        sCell1 = ((char)(64 + iPrincipalCellIndex)).ToString();
        sCell2 = ((char)(64 + iPrincipalCellIndex + 2)).ToString();
        MergeCell mergeCell5 = new MergeCell() { Reference = sCell1 + iSignRowIndex + ":" + sCell2 + iSignRowIndex };
        aoMergeCells.Append(mergeCell5);
    }

    /// <summary>
    /// This method is used to merge signature cells.
    /// </summary>
    /// <param name="aoMergeCells"></param>
    private void MergeSignCells(MergeCells aoMergeCells, bool abIsPrelimReport)
    {
        string sCell1, sCell2;
        int iPrincipalCellIndex = 0, iGroupSubjectCount;
        iGroupSubjectCount = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.ParentSubject).Distinct().Count();

        if (oExportReportBL.BasicInfo.ShowGrades)
        {
            if(msReportID != S_ANNUAL_CONSOLDATED_REPORT)
                iPrincipalCellIndex = 1 + (oExportReportBL.Subjects.Count * 2) + 3;
            else
                iPrincipalCellIndex = 2 + (oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject) * 2) + oExportReportBL.Subjects.Count(sb => sb.IsCoCurricularSubject) + 3;
        }
        else
        {
            if (!abIsPrelimReport)
                iPrincipalCellIndex = 1 + oExportReportBL.Subjects.Count + 3 + iGroupSubjectCount;
            else
                iPrincipalCellIndex = 1 + oExportReportBL.Subjects.Count(sb => !sb.IsCoCurricularSubject && sb.ParentSubject == string.Empty) + 2 + iGroupSubjectCount;
        }

        int iSignRowIndex = miFirstRowNo + 2 + oExportReportBL.StudentInfos.Count + miRowsBeforeSignSection;

        sCell1 = ((char)(64 + 3)).ToString();
        sCell2 = ((char)(64 + (iPrincipalCellIndex - 1))).ToString();

        MergeCell mergeCell1 = new MergeCell() { Reference = sCell1 + iSignRowIndex + ":" + sCell2 + iSignRowIndex };
        aoMergeCells.Append(mergeCell1);


        sCell1 = ((char)(64 + iPrincipalCellIndex)).ToString();
        sCell2 = ((char)(64 + iPrincipalCellIndex + 2)).ToString();

        MergeCell mergeCell2 = new MergeCell() { Reference = sCell1 + iSignRowIndex + ":" + sCell2 + iSignRowIndex };
        aoMergeCells.Append(mergeCell2);
    }

    /// <summary>
    /// This method is used to add data row.
    /// </summary>
    /// <param name="aoSheetData"></param>
    private void AddDataRowsForPrelimReport(SheetData aoSheetData)
    {
        int iRowIndex;
        iRowIndex = miFirstRowNo + 2;

        var lstCount = oExportReportBL.StudentInfos.Select(st => st.OriginalDivisionId).Distinct().OrderBy(st => st).ToList();

        foreach (int iOrgDivId in lstCount)
        {
            int iColumnIndex = 1;
            oExportReportBL.StudentInfos.Where(st => st.OriginalDivisionId == iOrgDivId).OrderBy(si => si.RollNo).ToList().ForEach(
                stud =>
                {
                    Row row = new Row { Height = 22D, CustomHeight = true };

                    row.Append(ConstructCell(stud.RollNo.ToString(), CellValues.Number, CellAlignment.CenterData));
                    row.Append(ConstructCell(stud.StudentName, CellValues.String, CellAlignment.LeftData));

                    iColumnIndex = SetSubjectMarksForPrelimReport(row, stud.StudentId, iRowIndex, 3);
                    var oTotal = oExportReportBL.StudentMarkSummary.Where(ss => ss.StudentId == stud.StudentId).FirstOrDefault();
                    iColumnIndex = SetSummaryFieldsForOpenXml(row, oTotal, iRowIndex, iColumnIndex);

                    if (oTotal != null)
                        row.Append(ConstructCell(oTotal.Rank.ToString(), CellValues.Number, CellAlignment.CenterData));
                    else
                        row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterData));

                    aoSheetData.Append(row);
                }
                );

            AddSignatures(aoSheetData, true);
        }
    }

    /// <summary>
    /// This method is used to add data row.
    /// </summary>
    /// <param name="aoSheetData"></param>
    private void AddDataRows(SheetData aoSheetData)
    {
        int iRowIndex;
        iRowIndex = miFirstRowNo + 2;

        int iColumnIndex = 1;
        oExportReportBL.StudentInfos.OrderBy(si => si.RollNo).ToList().ForEach(
            stud =>
            {
                Row row = new Row { Height = 22D, CustomHeight = true };

                row.Append(ConstructCell(stud.RollNo.ToString(), CellValues.Number, CellAlignment.CenterData));
                row.Append(ConstructCell(stud.StudentName, CellValues.String, CellAlignment.LeftData));
                row.Append(ConstructCell(stud.HouseName, CellValues.String, CellAlignment.CenterData));

                iColumnIndex = SetSubjectMarksForOpenXml(row, stud.StudentId, iRowIndex, 3);
                var oTotal = oExportReportBL.StudentMarkSummary.Where(ss => ss.StudentId == stud.StudentId).FirstOrDefault();

                if (IsAnnualConsoldatedReportOf9thSVP)
                {
                    iColumnIndex = SetCoCurriSubjectMarksForOpenXml(row, stud.StudentId, iRowIndex, iColumnIndex - 1);
                    iColumnIndex = SetSummaryFieldsForOpenXml(row, oTotal, iRowIndex, iColumnIndex);                    
                }
                else
                {
                    iColumnIndex = SetSummaryFieldsForOpenXml(row, oTotal, iRowIndex, iColumnIndex);
                    iColumnIndex = SetCoCurriSubjectMarksForOpenXml(row, stud.StudentId, iRowIndex, iColumnIndex - 1);
                }

                if (oTotal != null)
                {
                    if(msReportID == S_ANNUAL_CONSOLDATED_REPORT)
                        row.Append(ConstructCell((oTotal.Rank > 3 ? "-" : oTotal.Rank.ToString()), CellValues.String, CellAlignment.CenterData));
                    else
                        row.Append(ConstructCell(oTotal.Rank.ToString(), CellValues.Number, CellAlignment.CenterData));
                }
                else
                    row.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterData));

                aoSheetData.Append(row);
            }
            );
    }

    /// <summary>
    /// This method is used to set co-curricular subject marks.
    /// </summary>
    /// <param name="aoRow"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiColumnIndex"></param>
    /// <returns></returns>
    private int SetCoCurriSubjectMarksForOpenXml(Row aoRow, int aiStudentId, int aiRowIndex, int aiColumnIndex)
    {
        string sOldSubjectGroup = string.Empty;
        oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
        (
        sb =>
        {
            aiColumnIndex++;
            var oMarks = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == sb.SubjectId).FirstOrDefault();
            if (oMarks != null)
            {
                if(msReportID != S_ANNUAL_CONSOLDATED_REPORT)
                    aoRow.Append(ConstructCell(oMarks.ScoredMarks.ToString(), CellValues.Number, CellAlignment.CenterData));
                if (oExportReportBL.BasicInfo.ShowGrades || msReportID == S_ANNUAL_CONSOLDATED_REPORT)
                {
                    aiColumnIndex++;
                    aoRow.Append(ConstructCell(oMarks.Grade, CellValues.String, CellAlignment.CenterData));
                }
                else
                {
                    if (sOldSubjectGroup != string.Empty && sOldSubjectGroup == sb.ParentSubject)
                    {
                        var oGroupSubjectId = oExportReportBL.Subjects.Where(sbj => sbj.ParentSubject == sb.ParentSubject && sbj.SubjectId != sb.SubjectId).Select(sbj => sbj.SubjectId).FirstOrDefault();
                        if (oGroupSubjectId != null)
                        {
                            var oMarks1 = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == oGroupSubjectId).FirstOrDefault();
                            if (oMarks1 != null)
                                aoRow.Append(ConstructCell((oMarks.ScoredMarks + oMarks1.ScoredMarks).ToString(), CellValues.Number, CellAlignment.CenterData));
                        }
                    }
                    sOldSubjectGroup = sb.ParentSubject;
                }
            }
            else
                aoRow.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterData));
        }
        );
        aiColumnIndex++;
        return aiColumnIndex;
    }

    /// <summary>
    /// This method is used to set summary fields.
    /// </summary>
    /// <param name="aoRow"></param>
    /// <param name="aoTotal"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiColumnIndex"></param>
    /// <returns></returns>
    private int SetSummaryFieldsForOpenXml(Row aoRow, StudentMarkSummary aoTotal, int aiRowIndex, int aiColumnIndex)
    {
        if (aoTotal != null)
        {
            aoRow.Append(ConstructCell(aoTotal.TotalScoredMarks.ToString(), CellValues.Number, CellAlignment.CenterData));
            aoRow.Append(ConstructCell(aoTotal.Percentage.ToString(), CellValues.Number, CellAlignment.CenterDecimalData));

            if (oExportReportBL.BasicInfo.ShowGrades)
                aoRow.Append(ConstructCell(aoTotal.Grade, CellValues.String, CellAlignment.CenterData));
        }
        else
        {
            aoRow.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterData));
            aoRow.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterData));

            if (oExportReportBL.BasicInfo.ShowGrades)
                aoRow.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterData));
        }
        return aiColumnIndex;
    }

    /// <summary>
    /// This method is used to set suject marks.
    /// </summary>
    /// <param name="aoRow"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiColumnIndex"></param>
    /// <returns></returns>
    private int SetSubjectMarksForOpenXml(Row aoRow, int aiStudentId, int aiRowIndex, int aiColumnIndex)
    {
        string sOldSubjectGroup = string.Empty;
        var oGroupSubjectIds = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.SubjectId).ToList();
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
        (
        sb =>
        {
            aiColumnIndex++;
            var oMarks = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == sb.SubjectId).FirstOrDefault();
            if (oMarks != null)
            {
                if (oMarks.ExamStatus == string.Empty || oMarks.ScoredMarks > 0)
                    aoRow.Append(ConstructCell(oMarks.ScoredMarks.ToString(), CellValues.Number, CellAlignment.CenterData));
                else
                    aoRow.Append(ConstructCell(oMarks.ExamStatus.ToString(), CellValues.String, CellAlignment.CenterData));


                if (oExportReportBL.BasicInfo.ShowGrades)
                {
                    aiColumnIndex++;
                    if (oMarks.ExamStatus == string.Empty || oMarks.ScoredMarks > 0)
                        aoRow.Append(ConstructCell(oMarks.Grade, CellValues.String, CellAlignment.CenterData));
                    else
                        aoRow.Append(ConstructCell(oMarks.ExamStatus.ToString(), CellValues.String, CellAlignment.CenterData));
                }
                else
                {
                    if (sOldSubjectGroup != string.Empty && sOldSubjectGroup == sb.ParentSubject)
                    {
                        var oGroupSubjectId = oExportReportBL.Subjects.Where(sbj => sbj.ParentSubject == sb.ParentSubject && sbj.SubjectId != sb.SubjectId).Select(sbj => sbj.SubjectId).FirstOrDefault();
                        if (oGroupSubjectId != null)
                        {
                            var oMarks1 = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == oGroupSubjectId).FirstOrDefault();
                            if (oMarks1 != null)
                            {
                                decimal iTotal = oMarks.ScoredMarks + oMarks1.ScoredMarks;
                                int iTotalMk = oMarks.OutOfMarks + oMarks1.OutOfMarks;
                                
                                if (IsAnnualConsoldatedReportOf9thSVP && sb.ParentSubject != "S.S.T.")
                                    iTotal = Math.Round(((iTotal / iTotalMk)*100) + ((decimal)0.01), 0);

                                aoRow.Append(ConstructCell(iTotal.ToString(), CellValues.Number, CellAlignment.CenterData));
                            }
                        }
                    }
                    sOldSubjectGroup = sb.ParentSubject;
                }
            }
            else
                aoRow.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterData));
        }
        );
        aiColumnIndex++;
        return aiColumnIndex;
    }


    /// <summary>
    /// This method is used to set suject marks.
    /// </summary>
    /// <param name="aoRow"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiColumnIndex"></param>
    /// <returns></returns>
    private int SetSubjectMarksForPrelimReport(Row aoRow, int aiStudentId, int aiRowIndex, int aiColumnIndex)
    {
        string sOldSubjectGroup = string.Empty;
        var oGroupSubjectIds = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.SubjectId).ToList();
        List<string> lstSubjects = new List<string>();
        oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
        (
        sb =>
        {
            aiColumnIndex++;
            var oMarks = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == sb.SubjectId).FirstOrDefault();
            if (oMarks != null)
            {
                if (sb.ParentSubject == string.Empty)
                {
                    if (oMarks.ExamStatus == string.Empty || oMarks.ScoredMarks > 0)
                        aoRow.Append(ConstructCell(oMarks.ScoredMarks.ToString(), CellValues.Number, CellAlignment.CenterData));
                    else
                        aoRow.Append(ConstructCell(oMarks.ExamStatus.ToString(), CellValues.String, CellAlignment.CenterData));
                }
                else
                {
                    if (!lstSubjects.Contains(sb.ParentSubject))
                    {
                        var oGroupSubjectId = oExportReportBL.Subjects.Where(sbj => sbj.ParentSubject == sb.ParentSubject && sbj.SubjectId != sb.SubjectId).Select(sbj => sbj.SubjectId).FirstOrDefault();
                        if (oGroupSubjectId != null)
                        {
                            var oMarks1 = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == oGroupSubjectId).FirstOrDefault();
                            if (oMarks1 != null)
                                aoRow.Append(ConstructCell((oMarks.ScoredMarks + oMarks1.ScoredMarks).ToString(), CellValues.Number, CellAlignment.CenterData));
                        }

                        lstSubjects.Add(sb.ParentSubject);
                    }
                }
            }
            else
                aoRow.Append(ConstructCell(string.Empty, CellValues.String, CellAlignment.CenterData));
        }
        );
        aiColumnIndex++;
        return aiColumnIndex;
    }

    #region Basic OpenXml Method(s)

    /// <summary>
    /// This method is used to return alignment.
    /// </summary>
    /// <param name="aoHorizontalAlignment"></param>
    /// <param name="aoVerticalAlignment"></param>
    /// <returns></returns>
    private static DocumentFormat.OpenXml.Spreadsheet.Alignment GetAlignment(HorizontalAlignmentValues aoHorizontalAlignment, VerticalAlignmentValues aoVerticalAlignment)
    {
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterHeader = new DocumentFormat.OpenXml.Spreadsheet.Alignment
        {
            Vertical = aoVerticalAlignment,
            WrapText = true,
            Horizontal = aoHorizontalAlignment
        };

        if (aoHorizontalAlignment == HorizontalAlignmentValues.Left)
            alnCenterHeader.Indent = (UInt32Value)1U;

        return alnCenterHeader;
    }

    /// <summary>
    /// This method is used to set style properties.
    /// </summary>
    /// <param name="aoWorkbookStylesPart1"></param>
    private void GenerateWorkbookStylesPart1Content(WorkbookStylesPart aoWorkbookStylesPart1)
    {
        Stylesheet stylesheet1 = new Stylesheet();

        Fonts fonts1 = new Fonts(
            new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = miFontSize },
                new FontName { Val = "Calibri" }
            ),
            new Font( // Index 1 - header
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = miFontSize },
                new Bold { Val = true },
                new Color() { Rgb = "000000" },
                new FontName { Val = "Calibri" }
            ),
            new Font(new DocumentFormat.OpenXml.Spreadsheet.FontSize { Val = 14D },
                    new FontName { Val = "SHREE-ENG7-0252" }),
            new Font( // Index 1 - header
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 14 },
                new Bold { Val = true },
                new Color() { Rgb = "000000" },
                new FontName { Val = "Calibri" },
                new Underline { Val = UnderlineValues.Single }
            ),
             new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = miFontSize },
                new FontName { Val = "Calibri" },
                new Bold { Val = true }
                )
            );

        Fills fills1 = new Fills(
               new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
               new Fill(new PatternFill() { PatternType = PatternValues.LightGray }), // Index 1 - default
               new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "A9A9A9" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
           );

        Borders borders = new DocumentFormat.OpenXml.Spreadsheet.Borders(
                new DocumentFormat.OpenXml.Spreadsheet.Border(), // index 0 default
                new DocumentFormat.OpenXml.Spreadsheet.Border( // index 1 black border
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterHeader = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnLeftHeader = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnLeftData = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterData = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterDecimalData = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnSign = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnSignLeft = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnHeader = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnHeaderYear = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center);
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnHeaderRight = GetAlignment(HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center);

        CellFormats cellFormats1 = new CellFormats(
                new CellFormat(), // default
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true, ApplyBorder = true, Alignment = alnLeftHeader }, // header
                new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true, ApplyBorder = true, Alignment = alnCenterHeader }, // header
                new CellFormat { FontId = 2, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnHeader },
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = alnLeftData },
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = alnCenterData },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnSign },
                new CellFormat { FontId = 3, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnHeaderYear },
                new CellFormat { FontId = 4, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnHeaderRight },
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = alnCenterDecimalData, NumberFormatId = 2 },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = alnSignLeft }
            );

        aoWorkbookStylesPart1.Stylesheet = new Stylesheet(fonts1, fills1, borders, cellFormats1); ;
    }

    /// <summary>
    /// This method is used to set page setup.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private static void SetPageSetup(Worksheet aoWorksheet, OrientationValues aoOrientationValues)
    {
        PageSetup pageSetup1 = new PageSetup() { PaperSize = (UInt32Value)9U, Orientation = aoOrientationValues, Id = "rId1", FitToHeight = (UInt32Value)0U };
        aoWorksheet.Append(pageSetup1);
    }

    /// <summary>
    /// This method is used to set margin.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private static void SetPageMargin(Worksheet aoWorksheet, double dbLeftMargin)
    {
        DocumentFormat.OpenXml.Spreadsheet.PageMargins pageMargins1 = new DocumentFormat.OpenXml.Spreadsheet.PageMargins() { Left = dbLeftMargin, Right = 0.25D, Top = 0.25D, Bottom = 0.50D, Header = 0.25D, Footer = 0.25D };
        aoWorksheet.Append(pageMargins1);
    }

    /// <summary>
    /// This method is used to set print options
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private static void AddPrintOptions(Worksheet aoWorksheet)
    {
        DocumentFormat.OpenXml.Spreadsheet.PrintOptions printOptions1 = new DocumentFormat.OpenXml.Spreadsheet.PrintOptions() { HorizontalCentered = true };
        aoWorksheet.Append(printOptions1);
    }

    /// <summary>
    /// This method is used to set format properties.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private static void AddFormatProperties(Worksheet aoWorksheet)
    {
        SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D };
        aoWorksheet.Append(sheetFormatProperties1);
    }

    /// <summary>
    /// This method is used to set sheet dimensions.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private static void AddSheedDimension(Worksheet aoWorksheet)
    {
        SheetDimension sheetDimension1 = new SheetDimension() { Reference = "A1:B55" };
        aoWorksheet.Append(sheetDimension1);
    }

    /// <summary>
    /// This method is used to set sheet properties.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private static void AddSheetProperties(Worksheet aoWorksheet)
    {
        SheetProperties sheetProperties1 = new SheetProperties();
        PageSetupProperties pageSetupProperties1 = new PageSetupProperties() { FitToPage = true };        
        sheetProperties1.Append(pageSetupProperties1);
        aoWorksheet.Append(sheetProperties1);
    }
    
    /// <summary>
    /// This method is used to set calculation properties.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    private static void AddCalculationProperties(Workbook aoWorkbook)
    {
        CalculationProperties calculationProperties1 = new CalculationProperties() { CalculationId = (UInt32Value)124519U };
        aoWorkbook.Append(calculationProperties1);
    }

    /// <summary>
    /// This method is used to add sheet.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    private void AddSheets(Workbook aoWorkbook)
    {
        Sheets sheets1 = new Sheets();
        Sheet sheet1 = new Sheet() { Name = S_SHEET_NAME, SheetId = (UInt32Value)1U, Id = "rId1" };
        sheets1.Append(sheet1);
        aoWorkbook.Append(sheets1);
    }

    /// <summary>
    /// This method is used to set book view.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    private static void AddBookViews(Workbook aoWorkbook)
    {
        BookViews bookViews1 = new BookViews();
        WorkbookView workbookView1 = new WorkbookView() { XWindow = 120, YWindow = 30, WindowWidth = (UInt32Value)20055U, WindowHeight = (UInt32Value)9990U };
        bookViews1.Append(workbookView1);
        aoWorkbook.Append(bookViews1);
    }

    /// <summary>
    /// This method is used to set workbook properties.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    private static void AddWorkbookProperties(Workbook aoWorkbook)
    {
        WorkbookProperties workbookProperties1 = new WorkbookProperties() { DefaultThemeVersion = (UInt32Value)124226U };
        aoWorkbook.Append(workbookProperties1);
    }

    /// <summary>
    /// This method is used to set file version.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    private static void AddFileVersion(Workbook aoWorkbook)
    {
        FileVersion fileVersion1 = new FileVersion() { ApplicationName = "xl", LastEdited = "4", LowestEdited = "4", BuildVersion = "4505" };
        aoWorkbook.Append(fileVersion1);
    }

    /// <summary>
    /// This method is used to set sheet view.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    private void AddSheetView(Worksheet aoWorksheet)
    {
        SheetViews sheetViews1 = new SheetViews();

        SheetView sheetView1 = new SheetView() { ShowGridLines = false, TabSelected = true, WorkbookViewId = (UInt32Value)0U };
        Selection selection1 = new Selection() { SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1:B1" } };

        sheetView1.Append(selection1);

        sheetViews1.Append(sheetView1);
        aoWorksheet.Append(sheetViews1);
    }
    
    /// <summary>
    /// This method is used to set footer.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    public void GenerateHeaderFooter(Worksheet aoWorksheet)
    {
        HeaderFooter headerFooter1 = new HeaderFooter();
        OddFooter oddFooter1 = new OddFooter() { Space = SpaceProcessingModeValues.Preserve };
        //oddFooter1.Text = "&R&\"Calibri\"&B&12&P/&N       ";
        oddFooter1.Text = "&R&\"Calibri\"&B&12&P/&N";
        headerFooter1.Append(oddFooter1);

        aoWorksheet.Append(headerFooter1);
    }

    /// <summary>
    /// This method is used to add title cell.
    /// </summary>
    /// <param name="asCellReference"></param>
    /// <param name="asCellValue"></param>
    /// <param name="aoCellAlign"></param>
    /// <returns></returns>
    private static Cell AddTitleCell(string asCellReference, string asCellValue, CellAlignment aoCellAlign)
    {
        Cell cell4 = new Cell() { CellReference = asCellReference, StyleIndex = (UInt32Value)2U, DataType = CellValues.SharedString };
        CellValue cellValue4 = new CellValue();
        cellValue4.Text = asCellValue;
        cell4.Append(cellValue4);
        cell4.StyleIndex = Convert.ToUInt32(aoCellAlign);
        return cell4;
    }

    /// <summary>
    /// This method is used to construct cell.
    /// </summary>
    /// <param name="asValue"></param>
    /// <param name="aoDataType"></param>
    /// <param name="aoCellAlign"></param>
    /// <returns></returns>
    private Cell ConstructCell(string asValue, CellValues aoDataType, CellAlignment aoCellAlign)
    {
        return new Cell()
        {
            CellValue = new CellValue(asValue),
            DataType = new EnumValue<CellValues>(aoDataType),
            StyleIndex = Convert.ToUInt32(aoCellAlign)
        };
    }

    /// <summary>
    /// This method is used to return shared item.
    /// </summary>
    /// <param name="asName"></param>
    /// <returns></returns>
    private static SharedStringItem GetSharedItem(string asName)
    {
        SharedStringItem sharedStringItem1 = new SharedStringItem();
        Text text1 = new Text();
        text1.Text = asName;
        sharedStringItem1.Append(text1);
        return sharedStringItem1;
    }

    /// <summary>
    /// This method is used to add image.
    /// </summary>
    /// <param name="asImageFileName"></param>
    /// <param name="aoWorksheetPart"></param>
    private static void AddImage(string asImageFileName, WorksheetPart aoWorksheetPart)
    {
        var drawingsPart = aoWorksheetPart.AddNewPart<DrawingsPart>();

        if (!aoWorksheetPart.Worksheet.ChildElements.OfType<Drawing>().Any())
            aoWorksheetPart.Worksheet.Append(new Drawing { Id = aoWorksheetPart.GetIdOfPart(drawingsPart) });

        if (drawingsPart.WorksheetDrawing == null)
            drawingsPart.WorksheetDrawing = new WorksheetDrawing();

        var worksheetDrawing = drawingsPart.WorksheetDrawing;

        var imagePart = drawingsPart.AddImagePart(ImagePartType.Jpeg);

        using (var stream = new FileStream(asImageFileName, FileMode.Open))
            imagePart.FeedData(stream);

        dr.Bitmap bm = new dr.Bitmap(asImageFileName);
        DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();
        var extentsCx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
        var extentsCy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
        bm.Dispose();

        var colOffset = 0;
        var rowOffset = 0;
        int colNumber = 2;
        int rowNumber = 1;

        var nvps = worksheetDrawing.Descendants<Xdr.NonVisualDrawingProperties>();
        var nvpId = nvps.Count() > 0 ?
            (UInt32Value)worksheetDrawing.Descendants<Xdr.NonVisualDrawingProperties>().Max(p => p.Id.Value) + 1 :
            1U;

        var oneCellAnchor = new Xdr.OneCellAnchor(
            new Xdr.FromMarker
            {
                ColumnId = new Xdr.ColumnId((colNumber - 1).ToString()),
                RowId = new Xdr.RowId((rowNumber - 1).ToString()),
                ColumnOffset = new Xdr.ColumnOffset(colOffset.ToString()),
                RowOffset = new Xdr.RowOffset(rowOffset.ToString())
            },
            new Xdr.Extent { Cx = extentsCx, Cy = extentsCy },
            new Xdr.Picture(
                new Xdr.NonVisualPictureProperties(
                    new Xdr.NonVisualDrawingProperties { Id = nvpId, Name = "Picture " + nvpId, Description = asImageFileName },
                    new Xdr.NonVisualPictureDrawingProperties(new A.PictureLocks { NoChangeAspect = true })
                ),
                new Xdr.BlipFill(
                    new A.Blip { Embed = drawingsPart.GetIdOfPart(imagePart), CompressionState = A.BlipCompressionValues.Print },
                    new A.Stretch(new A.FillRectangle())
                ),
                new Xdr.ShapeProperties(
                    new A.Transform2D(
                        new A.Offset { X = 0, Y = 0 },
                        new A.Extents { Cx = extentsCx, Cy = extentsCy }
                    ),
                    new A.PresetGeometry { Preset = A.ShapeTypeValues.Rectangle }
                )
            ),
            new Xdr.ClientData()
        );

        worksheetDrawing.Append(oneCellAnchor);
    }   

    #endregion

    #endregion

    //private void AddBasicColumns(Excel.Worksheet objSHT)
    //{
    //    Excel.Range rngBasicColumns = objSHT.get_Range("A" + miFirstRowNo, "C" + miFirstRowNo);

    //    if (oExportReportBL.BasicInfo.ShowGrades)
    //    {
    //        objSHT.Cells[miFirstRowNo, 1] = "ROLL NO.";
    //        objSHT.Cells[miFirstRowNo, 2] = "STUDENT NAME";
    //        objSHT.Cells[miFirstRowNo, 3] = "HOUSE NAME";

    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, 1], objSHT.Cells[miFirstRowNo + 1, 1]).Merge(Type.Missing);
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, 2], objSHT.Cells[miFirstRowNo + 1, 2]).Merge(Type.Missing);
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, 3], objSHT.Cells[miFirstRowNo + 1, 3]).Merge(Type.Missing);

    //        objSHT.get_Range(rngBasicColumns.Cells[miFirstRowNo, 2], rngBasicColumns.Cells[miFirstRowNo, 2]).EntireColumn.ColumnWidth = 25;
    //    }
    //    else
    //        objSHT.get_Range(rngBasicColumns.Cells[miFirstRowNo, 2], rngBasicColumns.Cells[miFirstRowNo, 2]).EntireColumn.ColumnWidth = 22;

    //    objSHT.get_Range(rngBasicColumns.Cells[1, 1], rngBasicColumns.Cells[1, 1]).WrapText = true;
    //    objSHT.get_Range(rngBasicColumns.Cells[1, 3], rngBasicColumns.Cells[1, 3]).WrapText = true;

    //    objSHT.get_Range(rngBasicColumns.Cells[miFirstRowNo, 1], rngBasicColumns.Cells[miFirstRowNo, 1]).EntireColumn.ColumnWidth = 5;
    //    objSHT.get_Range(rngBasicColumns.Cells[miFirstRowNo, 3], rngBasicColumns.Cells[miFirstRowNo, 3]).EntireColumn.ColumnWidth = 8;

    //    rngBasicColumns.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
    //    rngBasicColumns.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
    //    rngBasicColumns.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    //    rngBasicColumns.Font.Bold = true;

    //    rngBasicColumns.Font.Size = miFontSize;
    //    rngBasicColumns.Font.Name = "Calibri";
    //}

    //private static void AddFooter(Excel.Worksheet objSHT)
    //{
    //    objSHT.PageSetup.FitToPagesTall = 1;
    //    objSHT.PageSetup.FitToPagesWide = 1;
    //    objSHT.PageSetup.CenterHorizontally = true;
    //    objSHT.PageSetup.RightFooter = "&\"Calibri\"&B&12&P/&N       ";
    //    objSHT.PageSetup.LeftMargin = 1;
    //    objSHT.PageSetup.RightMargin = 1;
    //    objSHT.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
    //    objSHT.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
    //}

    //private void AddGroupSubjectRow(Excel.Worksheet objSHT, int aiRowIndex)
    //{
    //    if (oExportReportBL.Subjects.Any(sb => sb.ParentSubject != string.Empty))
    //    {
    //        int iColumnIndex = 1;
    //        if (!oExportReportBL.Subjects.Any(sb => sb.ParentSubject != string.Empty))
    //        {
    //            objSHT.Cells[aiRowIndex, 1] = string.Empty;
    //            objSHT.Cells[aiRowIndex, 2] = string.Empty;
    //            objSHT.Cells[aiRowIndex, 3] = string.Empty;

    //            iColumnIndex = AddGroupSubjects(objSHT, false, aiRowIndex, 4);

    //            objSHT.Cells[aiRowIndex, iColumnIndex++] = string.Empty;
    //            objSHT.Cells[aiRowIndex, iColumnIndex++] = string.Empty;
    //            objSHT.Cells[aiRowIndex, iColumnIndex++] = string.Empty;
    //        }
    //        else
    //        {
    //            objSHT.Cells[aiRowIndex, 1] = "ROLL NO.";
    //            objSHT.Cells[aiRowIndex, 2] = "STUDENT NAME";
    //            objSHT.Cells[aiRowIndex, 3] = "HOUSE NAME";

    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, 1], objSHT.Cells[aiRowIndex, 1]).WrapText = true;
    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, 1], objSHT.Cells[aiRowIndex + 2, 1]).Merge(Type.Missing);
    //            Excel.Range rngRollNo = objSHT.get_Range("A" + miFirstRowNo, "A" + (miFirstRowNo + 1));
    //            rngRollNo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //            rngRollNo.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //            rngRollNo.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    //            rngRollNo.Font.Size = miFontSize;
    //            rngRollNo.Font.Name = "Calibri";

    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, 2], objSHT.Cells[aiRowIndex + 2, 2]).Merge(Type.Missing);
    //            Excel.Range rngStudentName = objSHT.get_Range("B" + miFirstRowNo, "B" + (miFirstRowNo + 1));
    //            rngStudentName.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
    //            rngStudentName.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //            rngStudentName.Font.Size = miFontSize;
    //            rngStudentName.Font.Name = "Calibri";

    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, 3], objSHT.Cells[aiRowIndex, 3]).WrapText = true;
    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, 3], objSHT.Cells[aiRowIndex + 2, 3]).Merge(Type.Missing);
    //            Excel.Range rngHouseName = objSHT.get_Range("C" + miFirstRowNo, "C" + (miFirstRowNo + 1));
    //            rngHouseName.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //            rngHouseName.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //            rngHouseName.Font.Size = miFontSize;
    //            rngHouseName.Font.Name = "Calibri";

    //            iColumnIndex = AddGroupSubjects(objSHT, false, aiRowIndex, 4);

    //            objSHT.Cells[aiRowIndex, iColumnIndex] = "GRAND TOTAL";
    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex, iColumnIndex]).WrapText = true;
    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex + 1, iColumnIndex]).Merge(Type.Missing);
    //            Excel.Range rngGrandTotal = objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex, iColumnIndex]);
    //            rngGrandTotal.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //            rngGrandTotal.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //            rngGrandTotal.Font.Size = miFontSize;
    //            rngGrandTotal.Font.Name = "Calibri";
    //            iColumnIndex++;

    //            objSHT.Cells[aiRowIndex, iColumnIndex] = "PER (%)";
    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex, iColumnIndex]).WrapText = true;
    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex + 1, iColumnIndex]).Merge(Type.Missing);
    //            Excel.Range rngPercentage = objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex, iColumnIndex]);
    //            rngPercentage.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //            rngPercentage.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //            rngPercentage.Font.Size = miFontSize;
    //            rngPercentage.Font.Name = "Calibri";
    //            rngPercentage.NumberFormat = "#.00";

    //            iColumnIndex++;

    //            iColumnIndex = AddGroupSubjects(objSHT, true, aiRowIndex, iColumnIndex);

    //            objSHT.Cells[aiRowIndex, iColumnIndex] = "RANK";

    //            if (!oExportReportBL.BasicInfo.TestName.ToLower().Contains("first term internal") && (oExportReportBL.BasicInfo.TestName.ToLower().Contains("first term") || oExportReportBL.BasicInfo.TestName.ToLower().Contains("annual exam")))
    //                objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex + 2, iColumnIndex]).EntireColumn.ColumnWidth = 5;
    //            else
    //                objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex + 2, iColumnIndex]).EntireColumn.ColumnWidth = 6;

    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex, iColumnIndex]).WrapText = true;
    //            objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex + 2, iColumnIndex]).Merge(Type.Missing);
    //            Excel.Range rngRank = objSHT.get_Range(objSHT.Cells[aiRowIndex, iColumnIndex], objSHT.Cells[aiRowIndex, iColumnIndex]);
    //            rngRank.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //            rngRank.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //            rngRank.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    //            rngRank.Font.Size = miFontSize;
    //            rngRank.Font.Name = "Calibri";
    //        }
    //    }
    //}

    ///// <summary>
    ///// This method is used to add group subjects.
    ///// </summary>
    ///// <returns></returns>
    //private int AddGroupSubjects(Excel.Worksheet objSHT, bool abIsCoCurriSubject, int aiRowIndex, int aiColumnIndex)
    //{
    //    string sOldParentSubejct = string.Empty;
    //    oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == abIsCoCurriSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
    //        (
    //            sb =>
    //            {
    //                if (sb.ParentSubject != string.Empty)
    //                {
    //                    if (sOldParentSubejct != sb.ParentSubject)
    //                    {
    //                        Excel.Range rng5 = objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex, aiColumnIndex + 2]);
    //                        objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex, aiColumnIndex + 2]).Merge(Type.Missing);
    //                        rng5.Value2 = sb.ParentSubject;
    //                        rng5.Font.Bold = true;
    //                        rng5.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

    //                        aiColumnIndex = aiColumnIndex + 2;
    //                    }
    //                    else
    //                        aiColumnIndex++;
    //                    sOldParentSubejct = sb.ParentSubject;
    //                }
    //                else
    //                {
    //                    objSHT.Cells[aiRowIndex, aiColumnIndex] = sb.SubjectName;
    //                    objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex + 1, aiColumnIndex]).Merge(Type.Missing);
    //                    objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex + 1, aiColumnIndex]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //                    objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex + 1, aiColumnIndex]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter; ;

    //                    aiColumnIndex++;
    //                }
    //            }
    //    );

    //    Excel.Range rngHeader = objSHT.get_Range(objSHT.Cells[aiRowIndex, 1], objSHT.Cells[aiRowIndex, aiColumnIndex]);
    //    rngHeader.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //    rngHeader.Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter; ;
    //    rngHeader.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
    //    rngHeader.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
    //    rngHeader.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    //    rngHeader.Font.Bold = true;

    //    return aiColumnIndex;
    //}

    ///// <summary>
    ///// This method is used to add student mark details.
    ///// </summary>
    ///// <returns></returns>
    //private void AddStudentMarkDetails(Excel.Worksheet objSHT)
    //{
    //    int iRowIndex;
    //    iRowIndex = miFirstRowNo + 2;

    //    int iColumnIndex = 1;
    //    oExportReportBL.StudentInfos.OrderBy(si => si.RollNo).ToList().ForEach(
    //        stud =>
    //        {
    //            objSHT.Cells[iRowIndex, 1] = stud.RollNo.ToString();
    //            objSHT.Cells[iRowIndex, 2] = stud.StudentName.ToString();
    //            objSHT.Cells[iRowIndex, 3] = stud.HouseName.ToString();

    //            iColumnIndex = SetSubjectMarks(objSHT, stud.StudentId, iRowIndex, 3);
    //            var oTotal = oExportReportBL.StudentMarkSummary.Where(ss => ss.StudentId == stud.StudentId).FirstOrDefault();
    //            iColumnIndex = SetSummaryFields(objSHT, oTotal, iRowIndex, iColumnIndex);

    //            iColumnIndex = SetCoCurriSubjectMarks(objSHT, stud.StudentId, iRowIndex, iColumnIndex - 1);

    //            if (oTotal != null)
    //                objSHT.Cells[iRowIndex, iColumnIndex] = oTotal.Rank.ToString();
    //            else
    //                objSHT.Cells[iRowIndex, iColumnIndex] = string.Empty;

    //            string sEndInd = string.Empty;
    //            if (iColumnIndex > 26)
    //                sEndInd = "A" + ((char)(64 + (iColumnIndex - 26))).ToString();
    //            else
    //                sEndInd = ((char)(64 + iColumnIndex)).ToString();

    //            Excel.Range rngAll = objSHT.get_Range("A" + iRowIndex, sEndInd + iRowIndex);
    //            rngAll.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
    //            rngAll.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    //            rngAll.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //            rngAll.Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //            rngAll.Font.Size = miFontSize;
    //            rngAll.Font.Name = "Calibri";
    //            // rngAll.WrapText = true;

    //            Excel.Range rngStudentName = objSHT.get_Range("B" + iRowIndex, "B" + iRowIndex);
    //            rngStudentName.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
    //            rngStudentName.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    //            rngStudentName.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
    //            rngStudentName.Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //            rngStudentName.WrapText = true;
    //            rngStudentName.Font.Size = miFontSize;
    //            rngStudentName.Font.Name = "Calibri";

    //            iRowIndex++;
    //        }
    //        );

    //    AddSignature(objSHT, iRowIndex, iColumnIndex);
    //}

    //private static void AddSignature(Excel.Worksheet objSHT, int iRowIndex, int iColumnIndex)
    //{
    //    Excel.Range rngClassTeacher = objSHT.get_Range(objSHT.Cells[iRowIndex + 3, 1], objSHT.Cells[iRowIndex + 3, 2]);
    //    objSHT.get_Range(objSHT.Cells[iRowIndex + 3, 1], objSHT.Cells[iRowIndex + 3, 2]).Merge(Type.Missing);
    //    rngClassTeacher.Value2 = "CLASS TEACHER";
    //    rngClassTeacher.Font.Bold = true;
    //    rngClassTeacher.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //    rngClassTeacher.Font.Size = 12;
    //    rngClassTeacher.Font.Name = "Calibri";

    //    Excel.Range rngPrincipal = objSHT.get_Range(objSHT.Cells[iRowIndex + 3, iColumnIndex - 4], objSHT.Cells[iRowIndex + 3, iColumnIndex]);
    //    objSHT.get_Range(objSHT.Cells[iRowIndex + 3, iColumnIndex - 4], objSHT.Cells[iRowIndex + 3, iColumnIndex]).Merge(Type.Missing);
    //    rngPrincipal.Value2 = "PRINCIPAL";
    //    rngPrincipal.Font.Bold = true;
    //    rngPrincipal.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //    rngPrincipal.Font.Size = 12;
    //    rngPrincipal.Font.Name = "Calibri";

    //    int iIndex = (iColumnIndex / 2 - 3);
    //    Excel.Range rngCoordinator = objSHT.get_Range(objSHT.Cells[iRowIndex + 3, iIndex], objSHT.Cells[iRowIndex + 3, iIndex + 4]);
    //    objSHT.get_Range(objSHT.Cells[iRowIndex + 3, iIndex], objSHT.Cells[iRowIndex + 3, iIndex + 4]).Merge(Type.Missing);
    //    rngCoordinator.Value2 = "COORDINATOR";
    //    rngCoordinator.Font.Bold = true;
    //    rngCoordinator.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //    rngCoordinator.Font.Size = 12;
    //    rngCoordinator.Font.Name = "Calibri";
    //}

    ///// <summary>
    ///// This method is used to set co-curricular subject marks/grades.
    ///// </summary>
    ///// <param name="aiStudentId"></param>
    ///// <returns></returns>
    //private int SetCoCurriSubjectMarks(Excel.Worksheet objSHT, int aiStudentId, int aiRowIndex, int aiColumnIndex)
    //{
    //    oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
    //    (
    //    sb =>
    //    {
    //        aiColumnIndex++;
    //        var oMarks = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == sb.SubjectId).FirstOrDefault();
    //        if (oMarks != null)
    //        {
    //            objSHT.Cells[aiRowIndex, aiColumnIndex] = oMarks.ScoredMarks.ToString();
    //            if (oExportReportBL.BasicInfo.ShowGrades)
    //            {
    //                aiColumnIndex++;
    //                objSHT.Cells[aiRowIndex, aiColumnIndex] = oMarks.Grade;
    //            }
    //        }
    //        else
    //            objSHT.Cells[aiRowIndex, aiColumnIndex] = string.Empty;
    //    }
    //    );
    //    aiColumnIndex++;
    //    return aiColumnIndex;
    //}

    ///// <summary>
    ///// This method is used to set summary fields.
    ///// </summary>
    ///// <param name="aoTotal"></param>
    ///// <returns></returns>
    //private int SetSummaryFields(Excel.Worksheet objSHT, StudentMarkSummary aoTotal, int aiRowIndex, int aiColumnIndex)
    //{
    //    if (aoTotal != null)
    //    {
    //        objSHT.Cells[aiRowIndex, aiColumnIndex++] = aoTotal.TotalScoredMarks.ToString();

    //        objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex, aiColumnIndex]).NumberFormat = "#.00";
    //        objSHT.Cells[aiRowIndex, aiColumnIndex++] = aoTotal.Percentage;

    //        if (oExportReportBL.BasicInfo.ShowGrades)
    //            objSHT.Cells[aiRowIndex, aiColumnIndex++] = aoTotal.Grade;
    //    }
    //    else
    //    {
    //        objSHT.Cells[aiRowIndex, aiColumnIndex++] = string.Empty;
    //        objSHT.Cells[aiRowIndex, aiColumnIndex++] = string.Empty;

    //        if (oExportReportBL.BasicInfo.ShowGrades)
    //            objSHT.Cells[aiRowIndex, aiColumnIndex++] = string.Empty;
    //    }
    //    return aiColumnIndex;
    //}

    ///// <summary>
    ///// This method is used to set subject marks.
    ///// </summary>
    ///// <param name="aiStudentId"></param>
    ///// <returns></returns>
    //private int SetSubjectMarks(Excel.Worksheet objSHT, int aiStudentId, int aiRowIndex, int aiColumnIndex)
    //{
    //    int iCnt = 0;
    //    var oGroupSubjectIds = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.SubjectId).ToList();
    //    oExportReportBL.Subjects.Where(sb => !sb.IsCoCurricularSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
    //    (
    //    sb =>
    //    {
    //        aiColumnIndex++;
    //        var oMarks = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId && st.SubjectId == sb.SubjectId).FirstOrDefault();
    //        if (oMarks != null)
    //        {
    //            if (oMarks.ExamStatus == string.Empty || oMarks.ScoredMarks > 0)
    //                objSHT.Cells[aiRowIndex, aiColumnIndex] = oMarks.ScoredMarks.ToString();
    //            else
    //                objSHT.Cells[aiRowIndex, aiColumnIndex] = oMarks.ExamStatus;

    //            if (oExportReportBL.BasicInfo.ShowGrades)
    //            {
    //                aiColumnIndex++;
    //                if (oMarks.ExamStatus == string.Empty || oMarks.ScoredMarks > 0)
    //                    objSHT.Cells[aiRowIndex, aiColumnIndex] = oMarks.Grade;
    //                else
    //                    objSHT.Cells[aiRowIndex, aiColumnIndex] = oMarks.ExamStatus;
    //            }
    //        }
    //        else
    //            objSHT.Cells[aiRowIndex, aiColumnIndex] = string.Empty;

    //        if (oGroupSubjectIds.Contains(sb.SubjectId))
    //        {
    //            iCnt++;

    //            if (iCnt == 2)
    //            {
    //                var oGroupMarks = mlStudentMarkDetails.Where(st => st.StudentId == aiStudentId).ToList();
    //                string sParentSubejct = oExportReportBL.Subjects.Where(sbs => sbs.SubjectId == sb.SubjectId).Select(sbs => sbs.ParentSubject).FirstOrDefault();
    //                var oTotalMk = oExportReportBL.Subjects.Where(sbs => sbs.ParentSubject == sParentSubejct).Select(sbs => sbs.SubjectId).ToList();

    //                var s = (from mm in oGroupMarks
    //                         join tt in oTotalMk
    //                         on mm.SubjectId equals tt
    //                         select mm).ToList();
    //                var sTotalMarks = s.Sum(sbs => sbs.ScoredMarks);

    //                aiColumnIndex++;
    //                objSHT.Cells[aiRowIndex, aiColumnIndex] = sTotalMarks.ToString();

    //                iCnt = 0;
    //            }
    //        }
    //    }
    //    );
    //    aiColumnIndex++;
    //    return aiColumnIndex;
    //}

    //private void AddOutOfMarksRow(Excel.Worksheet objSHT)
    //{
    //    int iInd = 4;
    //    iInd = AddOutOfMarks(objSHT, false, iInd);

    //    if (oExportReportBL.StudentMarkSummary.Any())
    //    {
    //        iInd = iInd + 1;
    //        var iTotal = oExportReportBL.StudentMarkSummary.Max(sms => sms.OutOfMarks);
    //        objSHT.Cells[miFirstRowNo + 1, iInd++] = iTotal.ToString();
    //        objSHT.Cells[miFirstRowNo + 1, iInd++] = 100;

    //        if (oExportReportBL.BasicInfo.ShowGrades)
    //            objSHT.Cells[miFirstRowNo + 1, iInd++] = "G";

    //        iInd = AddOutOfMarks(objSHT, true, iInd);
    //    }

    //    iInd = iInd + 1;

    //    string sEndInd = string.Empty;
    //    if (iInd > 26)
    //        sEndInd = "A" + ((char)(64 + (iInd - 26))).ToString();
    //    else
    //        sEndInd = ((char)(64 + iInd)).ToString();

    //    Excel.Range rngOutOfMarks = objSHT.get_Range("A" + (miFirstRowNo + 1), sEndInd + "" + (miFirstRowNo + 1));
    //    rngOutOfMarks.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
    //    rngOutOfMarks.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
    //    rngOutOfMarks.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    //    rngOutOfMarks.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //    rngOutOfMarks.Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //    rngOutOfMarks.Font.Bold = true;
    //    rngOutOfMarks.WrapText = true;
    //    rngOutOfMarks.Font.Size = miFontSize;
    //    rngOutOfMarks.Font.Name = "Calibri";
    //}

    //private int AddOutOfMarks(Excel.Worksheet objSHT, bool abIsCoCurriSubject, int aiInd)
    //{
    //    var oGroupSubjectIds = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.SubjectId).ToList();
    //    var oMaxMarks = mlStudentMarkDetails.GroupBy(sm => sm.SubjectId).Select(sm => new { SubjectId = sm.Key, OutOFMarks = sm.Max(smd => smd.OutOfMarks) });
    //    int iCounter = 0;

    //    oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == abIsCoCurriSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
    //        (
    //            sb =>
    //            {
    //                var oMarks = oMaxMarks.Where(mx => mx.SubjectId == sb.SubjectId).FirstOrDefault();
    //                if (oMarks != null)
    //                {
    //                    objSHT.Cells[miFirstRowNo + 1, aiInd] = oMarks.OutOFMarks.ToString();
    //                    if (oExportReportBL.BasicInfo.ShowGrades)
    //                    {
    //                        objSHT.get_Range(objSHT.Cells[miFirstRowNo + 1, aiInd], objSHT.Cells[miFirstRowNo + 1, aiInd]).EntireColumn.ColumnWidth = 3;
    //                        aiInd++;

    //                        objSHT.Cells[miFirstRowNo + 1, aiInd] = "G";
    //                        objSHT.get_Range(objSHT.Cells[miFirstRowNo + 1, aiInd], objSHT.Cells[miFirstRowNo + 1, aiInd]).EntireColumn.ColumnWidth = 3;
    //                        aiInd++;
    //                    }
    //                    else
    //                    {
    //                        objSHT.get_Range(objSHT.Cells[miFirstRowNo + 1, aiInd], objSHT.Cells[miFirstRowNo + 1, aiInd]).EntireColumn.ColumnWidth = 5;
    //                        aiInd++;
    //                    }
    //                }
    //                else
    //                {
    //                    objSHT.Cells[miFirstRowNo + 1, aiInd] = string.Empty;
    //                    objSHT.get_Range(objSHT.Cells[miFirstRowNo + 1, aiInd], objSHT.Cells[miFirstRowNo + 1, aiInd]).EntireColumn.ColumnWidth = 5;
    //                    aiInd++;
    //                }

    //                if (oGroupSubjectIds.Contains(sb.SubjectId))
    //                {
    //                    iCounter++;

    //                    if (iCounter == 2)
    //                    {
    //                        string sParentSubejct = oExportReportBL.Subjects.Where(sbs => sbs.SubjectId == sb.SubjectId).Select(sbs => sbs.ParentSubject).FirstOrDefault();
    //                        var oTotal = oExportReportBL.Subjects.Where(sbs => sbs.ParentSubject == sParentSubejct).Select(sbs => sbs.SubjectId).ToList();

    //                        var s = (from mm in oMaxMarks
    //                                 join tt in oTotal
    //                                 on mm.SubjectId equals tt
    //                                 select mm).ToList();
    //                        var sTotalMarks = s.Sum(sbs => sbs.OutOFMarks);

    //                        objSHT.Cells[miFirstRowNo + 1, aiInd] = sTotalMarks.ToString();
    //                        objSHT.get_Range(objSHT.Cells[miFirstRowNo + 1, aiInd], objSHT.Cells[miFirstRowNo + 1, aiInd]).EntireColumn.ColumnWidth = 5;
    //                        aiInd++;
    //                        iCounter = 0;
    //                    }
    //                }
    //            }
    //        );

    //    aiInd = aiInd - 1;

    //    return aiInd;
    //}

    ///// <summary>
    ///// This method is used to add subject row.
    ///// </summary>
    ///// <returns></returns>
    //private void AddSubjectRow(Excel.Worksheet objSHT, int aiRowIndex, int aiColumnIndex)
    //{
    //    int iColIndex = 0;
    //    if (!oExportReportBL.Subjects.Any(sb => sb.ParentSubject != string.Empty))
    //    {
    //        iColIndex = AddSubjects(false, objSHT, miFirstRowNo, 4);

    //        objSHT.Cells[miFirstRowNo, iColIndex] = "GRAND TOTAL";
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).WrapText = true;
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).EntireColumn.ColumnWidth = 7;

    //        objSHT.Cells[miFirstRowNo, ++iColIndex] = "PER (%)";
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).EntireColumn.ColumnWidth = 5;

    //        if (oExportReportBL.BasicInfo.ShowGrades)
    //        {
    //            objSHT.Cells[miFirstRowNo, ++iColIndex] = "GRADE";

    //            if (oExportReportBL.BasicInfo.ShowGrades && oExportReportBL.BasicInfo.TestName.Contains("Tool"))
    //                objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).EntireColumn.ColumnWidth = 5;
    //            else
    //                objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).EntireColumn.ColumnWidth = 7;
    //        }

    //        iColIndex = AddSubjects(true, objSHT, miFirstRowNo, (iColIndex + 1));

    //        objSHT.Cells[miFirstRowNo, iColIndex] = "RANK";
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).EntireColumn.ColumnWidth = 5;
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo + 1, iColIndex]).Merge(Type.Missing);
    //    }
    //    else
    //    {
    //        iColIndex = AddSubjects(false, objSHT, miFirstRowNo, 4);

    //        objSHT.Cells[miFirstRowNo, iColIndex] = "GRAND TOTAL";
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).WrapText = true;

    //        if (!oExportReportBL.BasicInfo.TestName.ToLower().Contains("first term internal") && (oExportReportBL.BasicInfo.TestName.ToLower().Contains("first term") || oExportReportBL.BasicInfo.TestName.ToLower().Contains("annual exam")))
    //            objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).EntireColumn.ColumnWidth = 7;
    //        else
    //            objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).EntireColumn.ColumnWidth = 9;

    //        objSHT.Cells[miFirstRowNo, ++iColIndex] = "PER (%)";
    //        objSHT.get_Range(objSHT.Cells[miFirstRowNo, iColIndex], objSHT.Cells[miFirstRowNo, iColIndex]).EntireColumn.ColumnWidth = 6;

    //        if (oExportReportBL.BasicInfo.ShowGrades)
    //            objSHT.Cells[miFirstRowNo, ++iColIndex] = "GRADE";

    //        iColIndex = AddSubjects(true, objSHT, miFirstRowNo, (iColIndex + 1));
    //    }

    //    string sEndInd = string.Empty;
    //    if (iColIndex > 26)
    //        sEndInd = "A" + ((char)(64 + (iColIndex - 26))).ToString();
    //    else
    //        sEndInd = ((char)(64 + iColIndex)).ToString();

    //    Excel.Range rngSubject = objSHT.get_Range("A" + aiRowIndex, sEndInd + aiRowIndex);
    //    rngSubject.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
    //    rngSubject.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
    //    rngSubject.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
    //    rngSubject.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
    //    rngSubject.Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
    //    rngSubject.Font.Bold = true;
    //    rngSubject.WrapText = true;
    //    rngSubject.Font.Size = miFontSize;
    //    rngSubject.Font.Name = "Calibri";
    //}

    //private int AddSubjects(bool abShowCoCurriSubject, Excel.Worksheet objSHT, int aiRowIndex, int aiColumnIndex)
    //{
    //    int iCounter = 0;
    //    var oGroupSubjectIds = oExportReportBL.Subjects.Where(sb => sb.ParentSubject != string.Empty).Select(sb => sb.SubjectId).ToList();
    //    oExportReportBL.Subjects.Where(sb => sb.IsCoCurricularSubject == abShowCoCurriSubject).OrderBy(sb => sb.SortOrder).ToList().ForEach
    //        (
    //            sb =>
    //            {
    //                if (oGroupSubjectIds.Count == 0)
    //                {
    //                    if (oExportReportBL.BasicInfo.ShowGrades)
    //                    {
    //                        objSHT.Cells[aiRowIndex, aiColumnIndex] = sb.SubjectName;
    //                        objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex, aiColumnIndex + 1]).Merge(Type.Missing);
    //                    }
    //                    else
    //                        objSHT.Cells[aiRowIndex, aiColumnIndex] = sb.SubjectName;
    //                }
    //                else
    //                {
    //                    if (sb.ParentSubject != string.Empty)
    //                    {
    //                        if (oExportReportBL.BasicInfo.ShowGrades)
    //                        {
    //                            objSHT.Cells[aiRowIndex, aiColumnIndex] = sb.SubjectName;
    //                            objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex, aiColumnIndex + 1]).Merge(Type.Missing);
    //                        }
    //                        else
    //                            objSHT.Cells[aiRowIndex, aiColumnIndex] = sb.SubjectName;
    //                    }
    //                    else
    //                        objSHT.Cells[aiRowIndex, aiColumnIndex] = string.Empty;
    //                }

    //                if (oGroupSubjectIds.Contains(sb.SubjectId))
    //                {
    //                    iCounter++;

    //                    if (iCounter == 2)
    //                    {
    //                        aiColumnIndex++;
    //                        objSHT.Cells[aiRowIndex, aiColumnIndex] = "TOT";
    //                        objSHT.get_Range(objSHT.Cells[aiRowIndex, aiColumnIndex], objSHT.Cells[aiRowIndex, aiColumnIndex]).EntireColumn.ColumnWidth = 5;
    //                        iCounter = 0;
    //                    }
    //                }

    //                if (oExportReportBL.BasicInfo.ShowGrades)
    //                    aiColumnIndex = aiColumnIndex + 2;
    //                else
    //                    aiColumnIndex = aiColumnIndex + 1;
    //            }
    //        );

    //    return aiColumnIndex;
    //}

    /// <summary>
    /// This method is used to set basic http details.
    /// </summary>
    private void SetBasicHTTPResponse()
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=ResultSheet.xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");
    }

    #endregion

    #region Subject Toppers Report

    /// <summary>
    /// 	This method is used to get all published exams.
    /// </summary>
    /// <param name="aiStandardId"> </param>
    /// <param name="aiDivisionId"> </param>
    /// <returns> </returns>
    private DataTable GetExams(int aiStandardId, int aiDivisionId)
    {
        DataTable oDtAllTests;
        //Return all published test of selected standard.
        if (hidStandardwise.Value == "Y" || aiDivisionId == 0)
        {
            var oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
            oDtAllTests = oTestCollectionBL.GetAllpublishedTestsForStandard(aiStandardId,0);
        }
        else
        {
            //Return all published test of selected class.
            DataTable oDataTable = ReportsBL.GetStandardDivisionId(miSchoolId, miAcademicYearId, aiStandardId, aiDivisionId);
            int iStdDivId = 0;
            if (oDataTable != null)
                iStdDivId = oDataTable.Rows[0][0].ToInt();
            var oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId, iStdDivId);
            oDtAllTests = oTestCollectionBL.GetAllpublishedTestsForClass();
        }
        return oDtAllTests;
    }

    /// <summary>
    /// 	This method is used to bind datatable to dropdownlist.
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    /// <param name="oDtAllTests"> </param>
    /// <param name="aiStandardId"> </param>
    /// <param name="aiDivisionId"> </param>
    private void FillExamCombo(int aiGridRowCount, DataTable oDtAllTests, int aiStandardId, int aiDivisionId)
    {
        string sIsReqiured = grdDisplayParameter.DataKeys[I_EXAM_ROW][I_ISREQUIRED_INDEX].ToString();
        DataView oDataView = null;
        if (sIsReqiured == "N")
        {
            oDataView = oDtAllTests.DefaultView;
            DataRow oDataRow = oDataView.Table.NewRow();
            oDataRow[S_TEST_NAME] = S_ALL;
            oDataRow[S_TEST_ID] = 0;
            oDataView.Table.Rows.InsertAt(oDataRow, 0);
        }

        var oDDLExam = grdDisplayParameter.Rows[aiGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
        oDDLExam.Enabled = true;
        oDDLExam.DataSource = oDataView;
        oDDLExam.DataTextField = S_TEST_NAME;
        oDDLExam.DataValueField = S_TEST_ID;
        oDDLExam.DataBind();

        //Check, final result is published or not.
        if (CheckIsResultPublished(aiStandardId, aiDivisionId))
            oDDLExam.Items.Add(new ListItem(string.Format("- {0}  -", S_ANNUAL_RESULT), S_ANNUAL_RESULT_TYPE));
    }

    /// <summary>
    /// 	This method is used to check that whether Result is published or not
    /// </summary>
    private Boolean CheckIsResultPublished(int aiStandardId, int iDivisionId)
    {
        return SchoolWiseAnnualResultPublishBL.IsExamPublished(miSchoolId, miAcademicYearId, aiStandardId, iDivisionId);
    }

    /// <summary>
    /// 	This method is used to return sub report name.
    /// </summary>
    /// <param name="aiIndex"> </param>
    /// <returns> </returns>
    private string GetSubReportName(int aiIndex)
    {
        string sSubReportName = string.Empty;
        if (aiIndex == 1)
        {
            switch (msReportID)
            {
                case S_SUBJECT_TOPPERS:
                    sSubReportName = "Test Toppers Details.rpt";
                    break;
                case S_TESTWISE_SUBJECT_TOPPERS:
                    sSubReportName = "TestwiseToppers.rpt";
                    break;
                case S_EXAM_RESULT:
                case S_EXAM_RESULT_PPSN:
                    sSubReportName = "SubReportOfGradeDetails.rpt";
                    break;
            }
        }
        if (msReportID == S_STUD_FINAL_RESULT || msReportID == S_STUD_FINAL_RESULT_PPSN || msReportID == S_STUD_FINAL_RESULT_MCPS)
        {
            if (aiIndex == 1)
                sSubReportName = "SubReportOfFinalGradeDetails.rpt";
            if (aiIndex == 2)
                sSubReportName = "SubReportOfFinalResultGraph.rpt";
        }
        if (msReportID == S_STUD_TERM2_RESULT)
        {
            if (aiIndex == 1)
                sSubReportName = "SubReportOfFinalGradeDetails.rpt";
            if (aiIndex == 2)
                sSubReportName = "SubReportOfTerm2ResultGraph.rpt";
        }
        if (msReportID == S_STUD_TERM1_RESULT)
        {
            if (aiIndex == 1)
                sSubReportName = "SubReportOfFinalGradeDetails.rpt";
            if (aiIndex == 2)
                sSubReportName = "SubReportOfTerm1ResultGraph.rpt";
        }
        return sSubReportName;
    }

    /// <summary>
    /// 	This method is used to fill type. i.e. Classwise/Standardwise
    /// </summary>
    /// <param name="aiGridRowCount"> </param>
    private void FillTypeCombobox(int aiGridRowCount)
    {
        var oDropDownList = grdDisplayParameter.Rows[aiGridRowCount].FindControl("DDLRptParameter") as ComboRpt;
        string sParameterName = grdDisplayParameter.DataKeys[aiGridRowCount][I_DISPLAY_NAME_INDEX].ToString();
        oDropDownList.Items.Clear();
        oDropDownList.EnableViewState = true;
        if (sParameterName == S_TYPE)
        {
            oDropDownList.Items.Add(S_CLASSWISE);
            oDropDownList.Items.Add(S_STANDARDWISE);
        }
        if (grdDisplayParameter.DataKeys[aiGridRowCount]["Is_Parent"].ToString() == "Y")
        {
            oDropDownList.ComboChangeEvent += oDropDownList_ComboChangeEvent;
            oDropDownList.AutoPostBack = true;
        }
        oDropDownList.Visible = true;
    }

	#endregion

    #region Muster roll report

    /// <summary>
    /// This method is used to show muster roll rpeort.
    /// </summary>
    /// <param name="asFilterString"></param>
    private void ExportMusterRollReport(string asFilterString)
    {
        int iStandardId = 0, iDivisionId = 0, iYear = 0, iMonth = 0;
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_MusterReport;1.", "").Split('@');

        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim().ToUpper() == "STANDARD_ID")
                    iStandardId = oData[1].ToInt();
                else if (oData[0].Trim().ToUpper() == "DIVISION_ID")
                    iDivisionId = oData[1].ToInt();
                else if (oData[0].Trim().ToUpper() == "YEAR")
                    iYear = oData[1].ToInt();
                else if (oData[0].Trim().ToUpper() == "MONTH_ID")
                    iMonth = oData[1].ToInt();
            }
        }

        int iNoOfDays = DateTime.DaysInMonth(iYear, iMonth);
        S_SHEET_NAME = "MusterRollReport";

        moMusterRollDetailsBL = new MusterRollDetailsBL(miSchoolId, miAcademicYearId);
        mlstAttendanceDetails = moMusterRollDetailsBL.GetAttendanceDetailsForMusterRoll(iStandardId, iDivisionId, iYear, iMonth);

        string sFileName = "MusterRollReport_" + Guid.NewGuid() + ".xlsx";

        //string filePath = Server.MapPath("..") + @"\UPLOADS\ResultSheet\" + sFileName;
        string filePath = base.BasePath + @"\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkbookPartForMusterRollReport(workbookPart, iNoOfDays, iYear, iMonth);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
    }

    /// <summary>
    /// This method is used to create workbook part.
    /// </summary>
    /// <param name="aoPart"></param>
    public void CreateWorkbookPartForMusterRollReport(WorkbookPart aoPart, int aiNoOfDays, int aiYear, int aiMonth)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        GenerateMusterRollReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateMusterRollReportContent(worksheetPart1, aiNoOfDays, aiYear, aiMonth);

        GeneratePartContentForMusterRoll(aoPart, false);
    }

    /// <summary>
    /// This method is used to generate worksheet part 1 content.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateMusterRollReportContent(WorksheetPart aoWorksheetPart1, int aiNoOfDays, int aiYear, int aiMonth)
    {
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        AddSheetDetails(worksheet1);
        SheetData sheetData1 = new SheetData();
       
        SetMusterRollColumnWidth(worksheet1, aiNoOfDays);
        SetMusterRollHeader(sheetData1, aiNoOfDays, aiYear, aiMonth);
        AddMusterRollHeader(sheetData1, aiNoOfDays);
        AddMusterRollDataRows(sheetData1, aiNoOfDays, aiYear, aiMonth);
        AddMusterRollSummaryRows(sheetData1, aiNoOfDays, aiYear, aiMonth);

        worksheet1.Append(sheetData1);

        worksheet1.Append(MergeMusterRollCells(aiNoOfDays));

        AddPrintOptions(worksheet1);
        SetPageMarginForHSP(worksheet1, 0.2);
        SetPageSetupForHSP(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    /// <summary>
    /// This method is used to merge cells.
    /// </summary>
    /// <returns></returns>
    private MergeCells MergeMusterRollCells(int aiNoOfDays)
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };
        int iCellCount = 4 + aiNoOfDays + 4;
        string sLastCell;
        if (iCellCount >= 53)
            sLastCell = "B" + ((char)(64 + (iCellCount - 52))).ToString();
        else if (iCellCount >= 27)
            sLastCell = "A" + ((char)(64 + (iCellCount - 26))).ToString();
        else
            sLastCell = ((char)(65 + iCellCount)).ToString();

        mergeCells1.Append(new MergeCell() { Reference = "A" + (miMusterRollStartupRow - 5) + ":" + sLastCell + (miMusterRollStartupRow - 5) });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (miMusterRollStartupRow - 4) + ":" + sLastCell + (miMusterRollStartupRow - 4) });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (miMusterRollStartupRow - 3) + ":" + sLastCell + (miMusterRollStartupRow - 3) });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (miMusterRollStartupRow - 2) + ":" + sLastCell + (miMusterRollStartupRow - 2) });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (miMusterRollStartupRow - 1) + ":" + sLastCell + (miMusterRollStartupRow - 1) });


        int iLastRowIndex = miMusterRollStartupRow + moMusterRollDetailsBL.StudentDetails.Count() + 7 + 2;
        mergeCells1.Append(new MergeCell() { Reference = "A" + iLastRowIndex + ":" + sLastCell + iLastRowIndex });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (iLastRowIndex + 1) + ":" + sLastCell + (iLastRowIndex + 1) });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (iLastRowIndex + 2) + ":" + sLastCell + (iLastRowIndex + 2) });

        return mergeCells1;
    }

    /// <summary>
    /// This method is used to set muster roll header note.
    /// </summary>
    /// <param name="aoSheetData"></param>
    /// <param name="aiNoOfDays"></param>
    /// <param name="aiYear"></param>
    /// <param name="aiMonth"></param>
    private void SetMusterRollHeader(SheetData aoSheetData, int aiNoOfDays, int aiYear, int aiMonth)
    {
        int iCellCount = 4 + aiNoOfDays + 4;

        Row rowOrgName = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartupRow - 5), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                rowOrgName.Append(AddCell(moMusterRollDetailsBL.SchoolDetails.OrgName, CellValues.String, MusterRollEnum.NoBorderCenterHeader));
            else
                rowOrgName.Append(AddCell(string.Empty, CellValues.String, MusterRollEnum.NoBorderCenterHeader));
        }
        aoSheetData.Append(rowOrgName);

        Row rowSchoolName = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartupRow - 4), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                rowSchoolName.Append(AddCell(moMusterRollDetailsBL.SchoolDetails.SchoolName, CellValues.String, MusterRollEnum.SchoolName));
            else
                rowSchoolName.Append(AddCell(string.Empty, CellValues.String, MusterRollEnum.NoBorderCenterHeader));
        }
        aoSheetData.Append(rowSchoolName);

        Row rowAcademicYear = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartupRow - 3), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                rowAcademicYear.Append(AddCell("Year " + moMusterRollDetailsBL.SchoolDetails.AcademicYear, CellValues.String, MusterRollEnum.AcademicYear));
            else
                rowAcademicYear.Append(AddCell(string.Empty, CellValues.String, MusterRollEnum.NoBorderCenterHeader));
        }
        aoSheetData.Append(rowAcademicYear);

        var oTotalWorkingDays = mlstAttendanceDetails.GroupBy(ad => ad.AttendanceDate).Count();
        var oTotalStudents = mlstAttendanceDetails.GroupBy(ad => ad.StudentId).Count();

        var cmbStandard = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
        var cmbDivision = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;
        string sText = "School Muster Roll For The Month : " + new DateTime(aiYear, aiMonth, 1).ToString("MMMM") + "          Standard : " + cmbStandard.SelectedItem.Text + "          Division : " + cmbDivision.SelectedItem.Text +
            "          School Working Days : " + oTotalWorkingDays + "          Total Student : " + oTotalStudents;

        Row rowClass = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartupRow - 2), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                rowClass.Append(AddCell(sText, CellValues.String, MusterRollEnum.NoBorderLeftBoldHeader));
            else
                rowClass.Append(AddCell(string.Empty, CellValues.String, MusterRollEnum.NoBorderLeftBoldHeader));
        }
        aoSheetData.Append(rowClass);

        Row rowNote = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartupRow - 1), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                rowNote.Append(AddCell("* This report is generated for Muster Roll.", CellValues.String, MusterRollEnum.NoBorderLeftHeader));
            else
                rowNote.Append(AddCell(string.Empty, CellValues.String, MusterRollEnum.NoBorderLeftHeader));
        }
        aoSheetData.Append(rowNote);
    }

    /// <summary>
    /// This method is used to set column width for muster roll report.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    /// <param name="aiNoOfDays"></param>
    private void SetMusterRollColumnWidth(Worksheet aoWorksheet1, int aiNoOfDays)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 9D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 35.57D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 15D, CustomWidth = true });

        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = Convert.ToUInt32(aiNoOfDays + 4), Width = 3D, CustomWidth = true });

        columns1.Append(new Column() { Min = Convert.ToUInt32(aiNoOfDays + 5), Max = Convert.ToUInt32(aiNoOfDays + 7), Width = 7D, CustomWidth = true });
        columns1.Append(new Column() { Min = Convert.ToUInt32(aiNoOfDays + 8), Max = Convert.ToUInt32(aiNoOfDays + 8), Width = 12D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    /// <summary>
    /// This method is used to show muster roll report data.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="aiNoOfDays"></param>
    /// <param name="aiYear"></param>
    /// <param name="aiMonth"></param>
    private void AddMusterRollDataRows(SheetData aoSheetData1, int aiNoOfDays, int aiYear, int aiMonth)
    {
        miMusterRollStartIndex++;
        moMusterRollDetailsBL.StudentDetails.OrderBy(stud => stud.RollNo).ToList().ForEach
            (
            stud =>
            {
                Row row = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartIndex), CustomHeight = true, Height = 20 };
                row.Append(AddCell(stud.RollNo.ToString(), CellValues.Number, MusterRollEnum.CenterData));
                row.Append(AddCell(stud.StudentName.ToString(), CellValues.String, MusterRollEnum.LeftData));
                row.Append(AddCell(stud.EnrolmentNumber.ToString(), CellValues.String, MusterRollEnum.LeftData));
                row.Append(AddCell(stud.DOB.ToString(Constants.S_DATE_FORMAT), CellValues.String, MusterRollEnum.CenterData));

                for (int iIndex = 1; iIndex <= aiNoOfDays; iIndex++)
                {
                    string sStatus = string.Empty;
                    SchoolEntities.MusterRollDetails.AttendanceDetails oAttendanceDetails = mlstAttendanceDetails.Where(ad => ad.StudentId == stud.StudentId && ad.AttendanceDate.Day == iIndex).FirstOrDefault();
                    if (oAttendanceDetails != null)
                    {
                        if (oAttendanceDetails.IsPresent)
                            row.Append(AddCell("P", CellValues.String, MusterRollEnum.PresentData));
                        else
                            row.Append(AddCell("A", CellValues.String, MusterRollEnum.AbsentData));
                    }
                    else
                    {
                        DateTime dt = new DateTime(aiYear, aiMonth, iIndex);
                        if (moMusterRollDetailsBL.HolidayDetails.Any(hd => dt.IsBetween(hd.StartDate, hd.EndDate)))
                            row.Append(AddCell("H", CellValues.String, MusterRollEnum.Holiday));
                        else if (moMusterRollDetailsBL.Weekends.Any(wd => dt.DayOfWeek.ToInt() == wd || (dt.DayOfWeek.ToInt() == 0 && wd == 7)))
                            row.Append(AddCell("W", CellValues.String, MusterRollEnum.Weekend));
                        else if (dt < moMusterRollDetailsBL.SchoolDetails.StartDate || dt > moMusterRollDetailsBL.SchoolDetails.EndDate)
                            row.Append(AddCell("O", CellValues.String, MusterRollEnum.OutsideAcademicYear));
                        else if (dt < stud.JoiningDate)
                            row.Append(AddCell("L", CellValues.String, MusterRollEnum.LateJoinee));
                        else if (dt > stud.SchoolLeftDate && stud.SchoolLeftDate != DateTime.MinValue)
                            row.Append(AddCell("D", CellValues.String, MusterRollEnum.LeftStudent));
                        else
                            row.Append(AddCell("X", CellValues.String, MusterRollEnum.AttendanceNotAvailable));
                    }
                }

                AttendanceSummaryDetails oAttendanceSummaryDetails = moMusterRollDetailsBL.AttendanceSummaryDetails.Where(asd => asd.StudentId == stud.StudentId).FirstOrDefault();
                if (oAttendanceSummaryDetails != null)
                {
                    row.Append(AddCell(oAttendanceSummaryDetails.CurrentMonthCount.ToString(), CellValues.Number, MusterRollEnum.CenterDataBold));
                    row.Append(AddCell(oAttendanceSummaryDetails.LastMonthCount.ToString(), CellValues.Number, MusterRollEnum.CenterDataBold));
                    row.Append(AddCell(oAttendanceSummaryDetails.TotalCount.ToString(), CellValues.Number, MusterRollEnum.CenterDataBold));
                    row.Append(AddCell(oAttendanceSummaryDetails.TotalPercentage + "%", CellValues.String, MusterRollEnum.Percentage));
                }

                aoSheetData1.Append(row);
                miMusterRollStartIndex++;
            }
            );
    }

    /// <summary>
    /// This method is used to add muster roll summary rows.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="aiNoOfDays"></param>
    /// <param name="aiYear"></param>
    /// <param name="aiMonth"></param>
    private void AddMusterRollSummaryRows(SheetData aoSheetData1, int aiNoOfDays, int aiYear, int aiMonth)
    {
        AddGenderwiseSummryRow(aoSheetData1, aiNoOfDays, "Present Girl(s)", 'F', true);
        AddGenderwiseSummryRow(aoSheetData1, aiNoOfDays, "Present Boy(s)", 'M', true);
        AddGenderwiseSummryRow(aoSheetData1, aiNoOfDays, "Absent Girl(s)", 'F', false);
        AddGenderwiseSummryRow(aoSheetData1, aiNoOfDays, "Absent Boy(s)", 'M', false);

        AddGenderwiseSummryRow(aoSheetData1, aiNoOfDays, "Total Present", 'O', true);
        AddGenderwiseSummryRow(aoSheetData1, aiNoOfDays, "Total Absent", 'O', false);
        AddGenderwiseSummryRow(aoSheetData1, aiNoOfDays, "Total", 'O', true);

        int iRollAtStrart = moMusterRollDetailsBL.StudentDetails.Count(sd => sd.SchoolLeftDate == DateTime.MinValue || sd.SchoolLeftDate > new DateTime(aiYear, aiMonth, 1));
        int iRollAtEnd = moMusterRollDetailsBL.StudentDetails.Count(sd => sd.SchoolLeftDate == DateTime.MinValue || sd.SchoolLeftDate > new DateTime(aiYear, aiMonth, aiNoOfDays));
        decimal dcPresentCount = mlstAttendanceDetails.Count(ad => ad.IsPresent);
        decimal dcPresentBoys = mlstAttendanceDetails.Where(ad => ad.IsPresent).Join(moMusterRollDetailsBL.StudentDetails.Where(sd => sd.Sex == 'M'), ad => ad.StudentId, sd => sd.StudentId, (ad, sd) => new { ad }).Count();
        decimal dcPresentGirls = mlstAttendanceDetails.Where(ad => ad.IsPresent).Join(moMusterRollDetailsBL.StudentDetails.Where(sd => sd.Sex == 'F'), ad => ad.StudentId, sd => sd.StudentId, (ad, sd) => new { ad }).Count();
        decimal dcTotalCount = mlstAttendanceDetails.GroupBy(ad => ad.AttendanceDate).Count();


        Row rowBlank = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartIndex), CustomHeight = true, Height = 20 };
        aoSheetData1.Append(rowBlank);
        miMusterRollStartIndex++;

        decimal dcPercent = 0, dcBoysPercent = 0,dcGirlPercent = 0;
        if (dcTotalCount != 0)
        {
            dcPercent = Math.Round((dcPresentCount / dcTotalCount), 2);
            dcBoysPercent = Math.Round((dcPresentBoys / dcTotalCount), 2);
            dcGirlPercent = Math.Round((dcPresentGirls / dcTotalCount), 2);
        }

        string sText = "No. of Roll at the beginning of the month : " + iRollAtStrart + "    No. of Roll at the end of the month : " + iRollAtEnd + "    Average attendance of Boys for the month : " + dcBoysPercent + "    Average attendance of Girls for the month : " + dcGirlPercent + "    Average attendance for the month : " + dcPercent;
        int iCellCount = 4 + aiNoOfDays + 4;

        Row row = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartIndex), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                row.Append(AddCell(sText, CellValues.String, MusterRollEnum.NoBorderLeftHeader));
            else
                row.Append(AddCell(string.Empty, CellValues.String, MusterRollEnum.NoBorderLeftHeader));
        }
        aoSheetData1.Append(row);
        miMusterRollStartIndex++;

        Row rowLegend = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartIndex), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                rowLegend.Append(AddCell("C. M. T. = Current Month Total              P. M. T. = Till Previous Month Total              F. T. = Final Total", CellValues.String, MusterRollEnum.NoBorderLeftHeader));
            else
                rowLegend.Append(AddCell(string.Empty, CellValues.String, MusterRollEnum.NoBorderLeftHeader));
        }
        aoSheetData1.Append(rowLegend);
        miMusterRollStartIndex++;

        Row rowLegend1 = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartIndex), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                rowLegend1.Append(AddCell("A-Absent     P-Present     X-Attendance Not Available     L-Late Joining     D-School Left     W-Weekend     H-Holiday     O-Outside Academic Year", CellValues.String, MusterRollEnum.NoBorderLeftHeader));
            else
                rowLegend1.Append(AddCell(string.Empty, CellValues.String, MusterRollEnum.NoBorderLeftHeader));
        }
        aoSheetData1.Append(rowLegend1);
    }

    /// <summary>
    /// This method is used to add genderwise summary rows in muster roll report.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="aiNoOfDays"></param>
    /// <param name="asTitle"></param>
    /// <param name="acSex"></param>
    /// <param name="abIsPresent"></param>
    private void AddGenderwiseSummryRow(SheetData aoSheetData1, int aiNoOfDays, string asTitle, char acSex, bool abIsPresent)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartIndex), CustomHeight = true, Height = 20 };
        row.Append(AddCell("", CellValues.String, MusterRollEnum.CenterData));
        row.Append(AddCell(asTitle, CellValues.String, MusterRollEnum.LeftData));
        row.Append(AddCell("", CellValues.String, MusterRollEnum.LeftData));
        row.Append(AddCell("", CellValues.String, MusterRollEnum.CenterData));

        for (int iIndex = 1; iIndex <= aiNoOfDays; iIndex++)
        {
            int iPresentCount = mlstAttendanceDetails.Where(at => at.AttendanceDate.Day == iIndex && (at.IsPresent == abIsPresent || asTitle == "Total")).Join(moMusterRollDetailsBL.StudentDetails.Where(st => (st.Sex == acSex || acSex == 'O')), ad => ad.StudentId, sd => sd.StudentId, (ad, sd) => new { ad.IsPresent }).Count();
            row.Append(AddCell(iPresentCount.ToString(), CellValues.Number, MusterRollEnum.SummaryRow));
        }

        int iCurentMonthPresentCount = 0, iPreviousMonthPresentCount = 0, iTotalWorkingDays = 0;


        if (acSex != 'O')
        {
            iCurentMonthPresentCount = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.Sex == acSex && gac.IsPresent == abIsPresent && gac.CategoryId == 1).Select(gac => gac.TotalCount).FirstOrDefault();
            iPreviousMonthPresentCount = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.Sex == acSex && gac.IsPresent == abIsPresent && gac.CategoryId == 2).Select(gac => gac.TotalCount).FirstOrDefault();
            iTotalWorkingDays = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.Sex == acSex && gac.CategoryId == 1).Sum(gac => gac.TotalCount);
        }
        else
        {
            if (asTitle != "Total")
            {
                iCurentMonthPresentCount = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.IsPresent == abIsPresent && gac.CategoryId == 1).Sum(gac => gac.TotalCount);
                iPreviousMonthPresentCount = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.IsPresent == abIsPresent && gac.CategoryId == 2).Sum(gac => gac.TotalCount);
                iTotalWorkingDays = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.CategoryId == 1).Sum(gac => gac.TotalCount);
            }
            else
            {
                iCurentMonthPresentCount = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.CategoryId == 1).Sum(gac => gac.TotalCount);
                iPreviousMonthPresentCount = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.CategoryId == 2).Sum(gac => gac.TotalCount);
                iTotalWorkingDays = moMusterRollDetailsBL.GenderwiseAttendanceSummary.Where(gac => gac.CategoryId == 1).Sum(gac => gac.TotalCount);
            }
        }

        row.Append(AddCell(iCurentMonthPresentCount.ToString(), CellValues.Number, MusterRollEnum.SummaryRow));
        row.Append(AddCell(iPreviousMonthPresentCount.ToString(), CellValues.Number, MusterRollEnum.SummaryRow));
        row.Append(AddCell((iCurentMonthPresentCount + iPreviousMonthPresentCount).ToString(), CellValues.Number, MusterRollEnum.SummaryRow));
        if (iTotalWorkingDays != 0)
            row.Append(AddCell(Math.Round((iCurentMonthPresentCount.ToDecimal() / iTotalWorkingDays) * 100, 2) + "%", CellValues.String, MusterRollEnum.SummaryRow));
        else
            row.Append(AddCell("0%", CellValues.String, MusterRollEnum.SummaryRow));

        aoSheetData1.Append(row);
        miMusterRollStartIndex++;
    }

    /// <summary>
    /// This method is used to add muster roll reprot column header row.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="aiNoOfDays"></param>
    private void AddMusterRollHeader(SheetData aoSheetData1, int aiNoOfDays)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miMusterRollStartIndex), CustomHeight = true, Height = 20 };
        row.Append(AddCell("Roll No.", CellValues.String, MusterRollEnum.CenterHeader));
        row.Append(AddCell("Student Name", CellValues.String, MusterRollEnum.LeftHeader));
        row.Append(AddCell("Redg. No.", CellValues.String, MusterRollEnum.LeftHeader));
        row.Append(AddCell("Date Of Birth", CellValues.String, MusterRollEnum.CenterHeader));

        for (int iIndex = 1; iIndex <= aiNoOfDays; iIndex++)
            row.Append(AddCell(iIndex.ToString(), CellValues.Number, MusterRollEnum.CenterHeader));

        row.Append(AddCell("C.M.T.", CellValues.String, MusterRollEnum.CenterHeader));
        row.Append(AddCell("P.M.T.", CellValues.String, MusterRollEnum.CenterHeader));
        row.Append(AddCell("F.T.", CellValues.String, MusterRollEnum.CenterHeader));
        row.Append(AddCell("Percentage", CellValues.String, MusterRollEnum.CenterHeader));

        aoSheetData1.Append(row);
    }

    /// <summary>
    /// This method is sued to add cell.
    /// </summary>
    /// <param name="asVal"></param>
    /// <param name="aoCellValues"></param>
    /// <param name="aoStypeIndex"></param>
    /// <returns></returns>
    private Cell AddCell(string asVal, CellValues aoCellValues, MusterRollEnum aoStypeIndex)
    {
        return new Cell()
        {
            CellValue = new CellValue(asVal),
            DataType = new EnumValue<CellValues>(aoCellValues),
            StyleIndex = Convert.ToUInt16(aoStypeIndex)
        };

    }

    

    /// <summary>
    /// This method is used to generate part contents.
    /// </summary>
    /// <param name="aoPart"></param>
    private void GeneratePartContentForMusterRoll(WorkbookPart aoPart, bool abIsPrelimReport)
    {
        Workbook workbook1 = new Workbook();
        workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        AddFileVersion(workbook1);

        AddWorkbookProperties(workbook1);

        AddBookViews(workbook1);

        AddSheets(workbook1);

        AddCalculationProperties(workbook1);

        aoPart.Workbook = workbook1;
    }

    /// <summary>
    /// This method is used to set style properties.
    /// </summary>
    /// <param name="aoWorkbookStylesPart1"></param>
    private void GenerateMusterRollReportStyles(WorkbookStylesPart aoWorkbookStylesPart1)
    {
        Fonts fonts1 = new Fonts(
            new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new FontName { Val = "Arial" },
                new Bold { Val = true }
            ),
            new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new FontName { Val = "Arial" }
            ),
            new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new FontName { Val = "Arial" },
                new Color { Rgb = "FF000000" },
                new Bold { Val = true }
            ),
            new Font( // Index 0 - default
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FFFF0000" },
               new Bold { Val = true }
           ),
           new Font( // Index 0 - default
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 12 },
               new FontName { Val = "Arial" },
               new Bold { Val = true }
           ),
           new Font( // Weekend
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF008080" },
               new Bold { Val = true }
           ),
           new Font( // Holiday
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF99CC00" },
               new Bold { Val = true }
           ),
           new Font( // Percentage
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF3366FF" },
               new Bold { Val = true }
           ),
           new Font( // Summary row
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF993300" },
               new Bold { Val = true }
           ),
           new Font( // Outside Academic Year
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF00FFFF" },
               new Bold { Val = true }
           ),
           new Font( // Late Joinee
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FFFFCC99" },
               new Bold { Val = true }
           ),
           new Font( // Left Student
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF808000" },
               new Bold { Val = true }
           ),
           new Font( // Attendance Not Available
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF800080" },
               new Bold { Val = true }
           ),
           new Font( // School Name
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 12 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF000080" },
               new Bold { Val = true }
           ),
           new Font( // Academic Year
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF800000" },
               new Bold { Val = true }
           )
            );

        Fills fills1 = new Fills(
               new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
               new Fill(new PatternFill() { PatternType = PatternValues.LightGray }), // Index 1 - default
               new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "A9A9A9" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
           );

        Borders borders = new DocumentFormat.OpenXml.Spreadsheet.Borders(
                new DocumentFormat.OpenXml.Spreadsheet.Border(), // index 0 default
                new DocumentFormat.OpenXml.Spreadsheet.Border( // index 1 black border
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

        CellFormats cellFormats1 = new CellFormats(
                new CellFormat(), // default
                new CellFormat { FontId = 0, FillId = 2, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 0, FillId = 2, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 2, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 3, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 4, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 0, FillId = 1, BorderId = 0, ApplyBorder = false, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 5, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 6, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 7, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 8, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 9, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 10, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 11, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 12, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 13, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 14, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) }
            );

        aoWorkbookStylesPart1.Stylesheet = new Stylesheet(fonts1, fills1, borders, cellFormats1); ;
    }

    #endregion

    #region Student Paid Fee

    /// <summary>
    /// This method is used to export students paid fee details in excel.
    /// </summary>
    /// <param name="asFilterString"></param>
    private void ExportStudentPaidFeeDetailsReport(string asFilterString)
    {
        int iStandardId = 0, iDivisionId = 0, iFeeTypeId = 0;
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_GetStudentPaidFeeDetailsForReport;1.", "").Split('@');

        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim().ToUpper() == "STANDARD_ID")
                    iStandardId = oData[1].ToInt();
                else if (oData[0].Trim().ToUpper() == "DIVISION_ID")
                    iDivisionId = oData[1].ToInt();
                else if (oData[0].Trim().ToUpper() == "FEETYPEID")
                    iFeeTypeId = oData[1].ToInt();                
            }
        }

        S_SHEET_NAME = "StudentPaidFeeDetailsReport";

        moStudentPaidFeeDetailsBL = new StudentPaidFeeDetailsReportBL(miSchoolId, miAcademicYearId);
        mlstStudentDetails = moStudentPaidFeeDetailsBL.GetStudentPaidFeeDetailsForReport(iStandardId, iDivisionId, iFeeTypeId);

        string sFileName = "StudentPaidFeeDetailsReport_" + Guid.NewGuid() + ".xlsx";

        //string filePath = Server.MapPath("..") + @"\UPLOADS\ResultSheet\" + sFileName;
        string filePath = base.BasePath + @"\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkBookPartForStudentPaidFeeReport(workbookPart);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
    }

    /// <summary>
    /// This method is used to create work book part for student paid fee details report.
    /// </summary>
    /// <param name="aoPart"></param>
    private void CreateWorkBookPartForStudentPaidFeeReport(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        GeneratePaidFeeReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateStudentPaidFeeDetailsReportContent(worksheetPart1);

        GeneratePartContentForPaidFeeDetails(aoPart, false);
    }

    /// <summary>
    /// This method is used to generate worksheet part 1 content.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateStudentPaidFeeDetailsReportContent(WorksheetPart aoWorksheetPart1)
    {
        int iColCount = moStudentPaidFeeDetailsBL.PayableForDetails.Count;
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        AddSheetDetails(worksheet1);
        SheetData sheetData1 = new SheetData();

        SetStudentPaidFeeColumnWidth(worksheet1, iColCount);
        SetPaidFeeReportHeader(sheetData1, iColCount);
        AddPaidFeeHeader(sheetData1, iColCount);
        AddStudentPaidFeeDataRows(sheetData1, iColCount);        

        worksheet1.Append(sheetData1);

        worksheet1.Append(MergeStudentPaidFeeReportCells(iColCount));

        AddPrintOptions(worksheet1);
        SetPageMarginForHSP(worksheet1, 0.2);
        SetPageSetupForHSP(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    /// <summary>
    /// This method is used to generate part contents.
    /// </summary>
    /// <param name="aoPart"></param>
    private void GeneratePartContentForPaidFeeDetails(WorkbookPart aoPart, bool abIsPrelimReport)
    {
        Workbook workbook1 = new Workbook();
        workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        AddFileVersion(workbook1);

        AddWorkbookProperties(workbook1);

        AddBookViews(workbook1);

        AddSheets(workbook1);

        AddCalculationProperties(workbook1);

        aoPart.Workbook = workbook1;
    }

    /// <summary>
    /// This method is used to set header for student paid fee report.
    /// </summary>
    /// <param name="aoSheetData"></param>
    /// <param name="iColCount"></param>
    private void SetPaidFeeReportHeader(SheetData aoSheetData, int iColCount)
    {
        int iCellCount = 4 + iColCount + 2;

        Row rowNote = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow - 1), CustomHeight = true, Height = 20 };
        for (int iIndex = 1; iIndex <= iCellCount; iIndex++)
        {
            if (iIndex == 1)
                rowNote.Append(AddCell("Students paid fee details", CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
            else
                rowNote.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        }
        aoSheetData.Append(rowNote);
    }

    /// <summary>
    /// This method is used add column header to excel file.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddPaidFeeHeader(SheetData aoSheetData1, int iColCount)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Class", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Reg. No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));        

        for (int iIndex = 0; iIndex < iColCount; iIndex++)
        {
            string sInstallmentName = moStudentPaidFeeDetailsBL.PayableForDetails[iIndex].PayableFor.ToString();
            row.Append(AddCell(sInstallmentName, CellValues.String, StudentPaidFeeEnum.LeftHeader));
        }

        row.Append(AddCell("Total Paid", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Remaining", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row);
    }

    /// <summary>
    /// This method is used to add all students paid fee details data in excel file.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddStudentPaidFeeDataRows(SheetData aoSheetData1, int iColCount)
    {
        miStudentPaidFeeDetailsStartIndex++;

        List<string> lstPayableFor = moStudentPaidFeeDetailsBL.PayableForDetails.Select(pay => pay.PayableFor).ToList();

        mlstStudentDetails.OrderBy(stud => stud.RollNo).ToList().ForEach
            (
                stud =>
                {
                    Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeDetailsStartIndex), CustomHeight = true, Height = 14 };
                    row.Append(AddCell(stud.RollNo.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(stud.StudentName.ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(stud.ClassName.ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(stud.EnrolmentNumber.ToString(), CellValues.String, StudentPaidFeeEnum.LeftData));

                    foreach (var payablefor in lstPayableFor)
                    {
                        string sPayableFor = payablefor.ToString();

                        var PayableFor = moStudentPaidFeeDetailsBL.PaidFeeDetails.Where(ss => ss.StudentId == stud.YearwiseStudentId && ss.PayableFor == sPayableFor).FirstOrDefault();

                        if (PayableFor != null)
                        {
                            row.Append(AddCell(PayableFor.Amount + "\n" + " " + PayableFor.PaidDate.ToString(Constants.S_DATE_FORMAT) + "\n" + "Che No. : " + PayableFor.ChequeNumber, CellValues.String, StudentPaidFeeEnum.CenterData));
                            row.Height = 39D;
                        }
                        else
                            row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                    }

                    int PendingAmount = moStudentPaidFeeDetailsBL.StudentFeeDetails.Where(ss => ss.StudentId == stud.YearwiseStudentId).Select(amt => amt.PedingAmount).FirstOrDefault().ToInt();
                    int PaidAmount = moStudentPaidFeeDetailsBL.StudentFeeDetails.Where(ss => ss.StudentId == stud.YearwiseStudentId).Select(amt => amt.PaidAmount).FirstOrDefault().ToInt();

                    row.Append(AddCell(PaidAmount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(PendingAmount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));                    

                    aoSheetData1.Append(row);
                    miStudentPaidFeeDetailsStartIndex++;
                }
            );
    }

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    /// <param name="aiNoOfDays"></param>
    private void SetStudentPaidFeeColumnWidth(Worksheet aoWorksheet1, int aiNoOfDays)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 9D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 35.57D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 15D, CustomWidth = true });

        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = Convert.ToUInt32(aiNoOfDays + 4), Width = 18D, CustomWidth = true });

        columns1.Append(new Column() { Min = Convert.ToUInt32(aiNoOfDays + 5), Max = Convert.ToUInt32(aiNoOfDays + 7), Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = Convert.ToUInt32(aiNoOfDays + 8), Max = Convert.ToUInt32(aiNoOfDays + 8), Width = 15D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    // <summary>
    /// This method is used to set style properties.
    /// </summary>
    /// <param name="aoWorkbookStylesPart1"></param>
    private void GeneratePaidFeeReportStyles(WorkbookStylesPart aoWorkbookStylesPart1)
    {
        Fonts fonts1 = new Fonts(
            new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new FontName { Val = "Arial" },
                new Bold { Val = true }
            ),
            new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new FontName { Val = "Arial" }
            ),
            new Font( // Index 0 - default
                new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
                new FontName { Val = "Arial" },
                new Color { Rgb = "FF000000" },
                new Bold { Val = true }
            ),
            new Font( // Index 0 - default
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FFFF0000" },
               new Bold { Val = true }
           ),
           new Font( // Index 0 - default
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 12 },
               new FontName { Val = "Arial" },
               new Bold { Val = true }
           )
          );

        Fills fills1 = new Fills(
               new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
               new Fill(new PatternFill() { PatternType = PatternValues.LightGray }), // Index 1 - default
               new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "A9A9A9" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
           );

        Borders borders = new DocumentFormat.OpenXml.Spreadsheet.Borders(
                new DocumentFormat.OpenXml.Spreadsheet.Border(), // index 0 default
                new DocumentFormat.OpenXml.Spreadsheet.Border( // index 1 black border
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

        CellFormats cellFormats1 = new CellFormats(
                new CellFormat(), // default
                new CellFormat { FontId = 0, FillId = 2, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 0, FillId = 2, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) },
                new CellFormat { FontId = 2, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center) }                
            );

        aoWorkbookStylesPart1.Stylesheet = new Stylesheet(fonts1, fills1, borders, cellFormats1); ;
    }

    /// <summary>
    /// This method is sued to add cell.
    /// </summary>
    /// <param name="asVal"></param>
    /// <param name="aoCellValues"></param>
    /// <param name="aoStypeIndex"></param>
    /// <returns></returns>
    private Cell AddCell(string asVal, CellValues aoCellValues, StudentPaidFeeEnum aoStypeIndex)
    {
        return new Cell()
        {
            CellValue = new CellValue(asVal),
            DataType = new EnumValue<CellValues>(aoCellValues),
            StyleIndex = Convert.ToUInt16(aoStypeIndex)
        };

    }

    /// <summary>
    /// This method is used to merge cells.
    /// </summary>
    /// <returns></returns>
    private MergeCells MergeStudentPaidFeeReportCells(int aiColCount)
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };
        int iCellCount = 4 + aiColCount + 2;
        string sLastCell = ((char)(65 + iCellCount - 1)).ToString();

        mergeCells1.Append(new MergeCell() { Reference = "A" + (miStudentPaidFeeStartupRow - 1) + ":" + sLastCell + (miStudentPaidFeeStartupRow - 1) });      

        return mergeCells1;
    }

    #endregion

   

    #endregion
  

    #region Student Attendance Report

    private void ExportLecturewiseStudentAttendance(string asFilterString)
    {
        int iStandardId = 0, iDivisionId = 0, iYear = 0, iMonth = 0;
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_MusterReport;1.", "").Split('@');

        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim().ToUpper() == "STANDARD_ID")
                    iStandardId = oData[1].ToInt();
                else if (oData[0].Trim().ToUpper() == "DIVISION_ID")
                    iDivisionId = oData[1].ToInt();
                else if (oData[0].Trim().ToUpper() == "YEAR")
                    iYear = oData[1].ToInt();
                else if (oData[0].Trim().ToUpper() == "MONTH_ID")
                    iMonth = oData[1].ToInt();
            }
        }

        ReportsBL oReportBL = new ReportsBL();
        moStudentAttendanceReport = oReportBL.GetStudentAttendanceDetails(miSchoolId, miAcademicYearId, iStandardId, iDivisionId, iYear, iMonth);

        mlstSundays = new List<DateTime>();
        DayOfWeek day = DayOfWeek.Sunday;
        int iDaysOfMOnth = DateTime.DaysInMonth(iYear, iMonth);
        for (int k = 1; k <= iDaysOfMOnth; k++)
        {
            DateTime dt = new DateTime(iYear, iMonth, k);
            if (dt.DayOfWeek == day && k != 1)
            {
                mlstSundays.Add(dt);
            }
        }

        DateTime dtFirstDate = moStudentAttendanceReport.AttendanceDetails.Min(at => at.Date);
        mlstSundays.RemoveAll(dt => dt <= dtFirstDate);

        string sFileName = "StudentAttendanceDetails_" + Guid.NewGuid() + ".xlsx";
        string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkBookForStudentAttendance(workbookPart);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
    }

    /// <summary>
    /// This method is used to create work book part for student paid fee details report.
    /// </summary>
    /// <param name="aoPart"></param>
    private void CreateWorkBookForStudentAttendance(WorkbookPart aoPart)
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateStudentAttendanceDetails(worksheetPart1);

        string sStdName = (grdDisplayParameter.Rows[Constants.I_ZERO].FindControl("DDLRptParameter") as ComboRpt).SelectedItem.Text;
        string sDivName = (grdDisplayParameter.Rows[Constants.I_ONE].FindControl("DDLRptParameter") as ComboRpt).SelectedItem.Text;
        string sYear = (grdDisplayParameter.Rows[Constants.I_TWO].FindControl("DDLRptParameter") as ComboRpt).SelectedItem.Text;
        string sMonth = (grdDisplayParameter.Rows[Constants.I_THREE].FindControl("DDLRptParameter") as ComboRpt).SelectedItem.Text;

        GeneratePartContent(aoPart, "Class " + sStdName + "-" + sDivName + "     Month " + sMonth + "-" + sYear);
    }

    /// <summary>
    /// This method is used to geenerate fee details.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateStudentAttendanceDetails(WorksheetPart aoWorksheetPart1)
    {
        int iColCount = 0;

        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);
        SheetData sheetData1 = new SheetData();

        AddStudentAttendanceHeader(sheetData1, iColCount);

        AddStudentAttendaceRow(sheetData1, iColCount);

        SetColumnWidthStudentAttendanceReport(worksheet1, iColCount);

        worksheet1.Append(sheetData1);

        worksheet1.Append(MergeHeaderCells());
        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    private MergeCells MergeHeaderCells()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        int iTopRowIndex = 2;
        int iStartIndex = 3;
        int iEndIndex;
        string sCellStart, sCellEnd;

        MergeCell mergeCell3 = null;

        mergeCell3 = new MergeCell() { Reference = "A" + iTopRowIndex + ":" + "A" + (iTopRowIndex + 1) };
        mergeCells1.Append(mergeCell3);

        mergeCell3 = new MergeCell() { Reference = "B" + iTopRowIndex + ":" + "B" + (iTopRowIndex + 1) };
        mergeCells1.Append(mergeCell3);

        int iIndex = 0;
        DateTime dtSunday = mlstSundays[iIndex];
        moStudentAttendanceReport.AttendanceDetails.Select(ad => ad.Date).Distinct().OrderBy(ad => ad).ToList().ForEach(
           ad =>
           {
               
               if (dtSunday < ad.Date && mlstSundays.Count > iIndex)
               {

                   sCellStart = base.GetReferenceName(iStartIndex);
                   mergeCell3 = new MergeCell() { Reference = sCellStart + iTopRowIndex + ":" + sCellStart + (iTopRowIndex + 1) };
                   mergeCells1.Append(mergeCell3);


                   iStartIndex++;

                   iIndex++;

                   if (mlstSundays.Count > iIndex)
                        dtSunday = mlstSundays[iIndex];
               }

               //int iCount = moStudentAttendanceReport.AttendanceDetails.Where(atd => atd.Date == ad.Date).Select(ATD => ATD.LectureNo).Distinct().Count();
               int iCount = 5;

               iEndIndex = iStartIndex + iCount - 1;

               sCellStart = base.GetReferenceName(iStartIndex);
               sCellEnd = base.GetReferenceName(iEndIndex);

               mergeCell3 = new MergeCell() { Reference = sCellStart + iTopRowIndex + ":" + sCellEnd + iTopRowIndex };
               mergeCells1.Append(mergeCell3);

               iStartIndex = iEndIndex + 1;
           }
           );

        sCellStart = base.GetReferenceName(iStartIndex);
        if (moStudentAttendanceReport.AttendanceDetails.Max(atd => atd.Date).Date != mlstSundays[mlstSundays.Count - 1])
        {
            mergeCell3 = new MergeCell() { Reference = sCellStart + iTopRowIndex + ":" + sCellStart + (iTopRowIndex + 1) };
            mergeCells1.Append(mergeCell3);
        }

        iStartIndex = iStartIndex + iIndex;

        sCellStart = base.GetReferenceName(iStartIndex);
        mergeCell3 = new MergeCell() { Reference = sCellStart + iTopRowIndex + ":" + sCellStart + (iTopRowIndex + 1) };
        mergeCells1.Append(mergeCell3);

        iStartIndex++;
        sCellStart = base.GetReferenceName(iStartIndex);
        mergeCell3 = new MergeCell() { Reference = sCellStart + iTopRowIndex + ":" + sCellStart + (iTopRowIndex + 1) };
        mergeCells1.Append(mergeCell3);

        iStartIndex++;
        sCellStart = base.GetReferenceName(iStartIndex);
        mergeCell3 = new MergeCell() { Reference = sCellStart + iTopRowIndex + ":" + sCellStart + (iTopRowIndex + 1) };
        mergeCells1.Append(mergeCell3);

        return mergeCells1;
    }

    private void AddStudentAttendaceRow(SheetData aoSheetData1, int aiColCount)
    {
        miStudentPaidFeeStartupRow++;
        int iIndex = 0;
        DateTime dtSunday, dtLastSunday;
        int iWeekCount, iWeekTotalCount;

        moStudentAttendanceReport.StudentDetails.OrderBy(STUD => STUD.RollNo).ToList().ForEach(
            stud =>
            {

                dtLastSunday = mlstSundays[0].AddDays((mlstSundays[0].Day) * -1);

                Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

                row.Append(AddCell(stud.RollNo.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell(stud.StudentName, CellValues.String, StudentPaidFeeEnum.LeftData));

                iIndex = 0;
                dtSunday = mlstSundays[iIndex];
                moStudentAttendanceReport.AttendanceDetails.Select(ad => ad.Date).Distinct().OrderBy(ad => ad).ToList().ForEach(
                ad =>
                {
                    if (dtSunday < ad.Date && mlstSundays.Count > iIndex)
                    {
                        iWeekCount = moStudentAttendanceReport.AttendanceDetails.Where(ad1 => ad1.StudentId == stud.YearWiseStudentId && ad1.IsPresent == "Y" && ad1.Date <= dtSunday && ad1.Date > dtLastSunday).Select(ad1 => new { ad1.Date, ad1.LectureNo }).Distinct().Count();
                        iWeekTotalCount = moStudentAttendanceReport.AttendanceDetails.Where(ad1 => ad1.StudentId == stud.YearWiseStudentId && ad1.Date <= dtSunday && ad1.Date > dtLastSunday).Select(ad1 => new { ad1.Date, ad1.LectureNo }).Distinct().Count();

                        row.Append(AddCell(iWeekCount + "/" + iWeekTotalCount, CellValues.String, StudentPaidFeeEnum.CenterHeader));

                        dtLastSunday = dtSunday;

                        iIndex++;

                        if (mlstSundays.Count > iIndex)
                            dtSunday = mlstSundays[iIndex];
                    }

                    List<AttendanceReportEntity.AttendanceDetails> lstData = moStudentAttendanceReport.AttendanceDetails.Where(atd => atd.StudentId == stud.YearWiseStudentId && atd.Date == ad.Date).ToList();

                    List<int> lstLectureNos = moStudentAttendanceReport.AttendanceDetails.Where(atd => atd.Date == ad.Date).Select(ATD => ATD.LectureNo).Distinct().ToList();

                    for(int k=1;k<=5;k++)
                    {
                        var oStatus = lstData.Where(lt => lt.LectureNo == k).FirstOrDefault();
                        if (oStatus != null)
                        {
                            if (oStatus.IsPresent == "Y")
                                row.Append(AddReportCell(oStatus.IsPresent, CellValues.String, ExcelReportEnum.CenterData));
                            else
                                row.Append(AddReportCell(oStatus.IsPresent, CellValues.String, ExcelReportEnum.CenterDataWithLightRedColor));
                        }
                        else
                            row.Append(AddReportCell("X", CellValues.String, ExcelReportEnum.CenterDataWithLightBlueColor));
                    }

                    //List<int> lstLectureNos = moStudentAttendanceReport.AttendanceDetails.Where(atd => atd.Date == ad.Date).Select(ATD => ATD.LectureNo).Distinct().ToList();

                    //lstLectureNos.OrderBy(atd => atd).ToList()
                    //.ForEach(lct =>
                    //{
                    //    var oStatus = lstData.Where(lt => lt.LectureNo == lct).FirstOrDefault();
                    //    if (oStatus != null)
                    //    {
                    //        if (oStatus.IsPresent == "Y")
                    //            row.Append(AddReportCell(oStatus.IsPresent, CellValues.String, ExcelReportEnum.CenterData));
                    //        else
                    //            row.Append(AddReportCell(oStatus.IsPresent, CellValues.String, ExcelReportEnum.CenterDataWithLightRedColor));
                    //    }
                    //    else
                    //        row.Append(AddReportCell(string.Empty, CellValues.String, ExcelReportEnum.CenterDataWithLightRedColor));
                    //});
                }
                );

                if (moStudentAttendanceReport.AttendanceDetails.Max(atd => atd.Date).Date != mlstSundays[mlstSundays.Count - 1])
                {
                    //iWeekCount = moStudentAttendanceReport.AttendanceDetails.Where(ad1 => ad1.StudentId == stud.YearWiseStudentId && ad1.IsPresent == "Y" && ad1.Date <= dtSunday && ad1.Date > dtLastSunday).Select(ad1 => new { ad1.Date, ad1.LectureNo }).Distinct().Count();
                    //iWeekTotalCount = moStudentAttendanceReport.AttendanceDetails.Where(ad1 => ad1.StudentId == stud.YearWiseStudentId && ad1.Date <= dtSunday && ad1.Date > dtLastSunday).Select(ad1 => new { ad1.Date, ad1.LectureNo }).Distinct().Count();

                    iWeekCount = moStudentAttendanceReport.AttendanceDetails.Where(ad1 => ad1.StudentId == stud.YearWiseStudentId && ad1.IsPresent == "Y" && ad1.Date > dtLastSunday).Select(ad1 => new { ad1.Date, ad1.LectureNo }).Distinct().Count();
                    iWeekTotalCount = moStudentAttendanceReport.AttendanceDetails.Where(ad1 => ad1.StudentId == stud.YearWiseStudentId && ad1.Date > dtLastSunday).Select(ad1 => new { ad1.Date, ad1.LectureNo }).Distinct().Count();

                    row.Append(AddCell(iWeekCount + "/" + iWeekTotalCount, CellValues.String, StudentPaidFeeEnum.CenterHeader));
                }

                int iMonthCount = moStudentAttendanceReport.AttendanceDetails.Where(ad => ad.StudentId == stud.YearWiseStudentId && ad.IsPresent == "Y").Select(ad => new { ad.Date, ad.LectureNo }).Distinct().Count();
                int iTotalCount = moStudentAttendanceReport.AttendanceDetails.Where(ad => ad.StudentId == stud.YearWiseStudentId).Select(ad => new { ad.Date, ad.LectureNo }).Distinct().Count();
                row.Append(AddReportCell(iMonthCount + "/" + iTotalCount, CellValues.String, ExcelReportEnum.CenterData));

                row.Append(AddReportCell(stud.TermCount, CellValues.String, ExcelReportEnum.CenterData));
                row.Append(AddReportCell(stud.TermPercentage+"%", CellValues.String, ExcelReportEnum.CenterData));

                aoSheetData1.Append(row);

                miStudentPaidFeeStartupRow++;
            });
    }

    /// <summary>
    /// This method is used to add columns.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddStudentAttendanceHeader(SheetData aoSheetData1, int iColCount)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        int iIndex = 0;
                
        DateTime dtSunday = mlstSundays[iIndex];
        moStudentAttendanceReport.AttendanceDetails.Select(ad => ad.Date).Distinct().OrderBy(ad => ad).ToList().ForEach(
            ad =>
            {
                if (dtSunday < ad.Date && mlstSundays.Count > iIndex)
                {
                    row.Append(AddCell("Week Total", CellValues.String, StudentPaidFeeEnum.LeftHeader));

                    iIndex++;

                    if (mlstSundays.Count > iIndex)
                        dtSunday = mlstSundays[iIndex];
                }

                row.Append(AddCell(ad.Day.ToString(), CellValues.String, StudentPaidFeeEnum.CenterHeader));

                //int iTotalCouint = moStudentAttendanceReport.AttendanceDetails.Where(atd => atd.Date == ad.Date).Select(ATD => ATD.LectureNo).Distinct().Count();
                //for (int K = 0; K < iTotalCouint - 1; K++)
                //    row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));

                int iTotalCouint = 5;
                for (int K = 0; K < iTotalCouint - 1; K++)
                    row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
            }
            );

        if (moStudentAttendanceReport.AttendanceDetails.Max(atd => atd.Date).Date != mlstSundays[mlstSundays.Count - 1])
            row.Append(AddCell("Week Total", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        row.Append(AddCell("Month Total", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Term Total", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Term Percentage", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row);

        miStudentPaidFeeStartupRow++;
        row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        iIndex = 0;
        dtSunday = mlstSundays[iIndex];
        moStudentAttendanceReport.AttendanceDetails.Select(ad => ad.Date).Distinct().OrderBy(ad => ad).ToList().ForEach(
            ad =>
            {
                if (dtSunday < ad.Date && mlstSundays.Count > iIndex)
                {
                    row.Append(AddCell("W", CellValues.String, StudentPaidFeeEnum.LeftHeader));

                    iIndex++;

                    if (mlstSundays.Count > iIndex)
                        dtSunday = mlstSundays[iIndex];
                }

                for(int k=1; k<=5;k++)
                {
                    row.Append(AddCell(GetLectureName(k), CellValues.String, StudentPaidFeeEnum.CenterHeader));
                }

                //moStudentAttendanceReport.AttendanceDetails.Where(atd => atd.Date == ad.Date).Select(ATD => ATD.LectureNo).Distinct().OrderBy(atd => atd).ToList().ForEach(atd =>
                //{
                //    row.Append(AddCell(GetLectureName(atd), CellValues.String, StudentPaidFeeEnum.CenterHeader));
                //});
            }
            );

        if (moStudentAttendanceReport.AttendanceDetails.Max(atd => atd.Date).Date != mlstSundays[mlstSundays.Count - 1])
            row.Append(AddCell("W", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftHeader));

        aoSheetData1.Append(row);
    }

    private string GetLectureName(int aiNumber)
    {
        string sLectureNo = string.Empty;
        switch (aiNumber)
        {
            case 1: sLectureNo = "st"; break;
            case 2: sLectureNo = "nd"; break;
            case 3: sLectureNo = "rd"; break;
            default: sLectureNo = "th"; break;
        }

        sLectureNo = aiNumber + sLectureNo;
        return sLectureNo;
    }

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    /// <param name="aiNoOfDays"></param>
    private void SetColumnWidthStudentAttendanceReport(Worksheet aoWorksheet1, int aiNoOfDays)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 10D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 30D, CustomWidth = true });

        int iStartIndex = 3;
        int iIndex = 0;
        DateTime dtSunday = mlstSundays[iIndex];
        moStudentAttendanceReport.AttendanceDetails.Select(ad => ad.Date).Distinct().OrderBy(ad => ad).ToList().ForEach(
            ad =>
            {
                if (dtSunday < ad.Date && mlstSundays.Count > iIndex)
                {
                    iStartIndex++;
                    columns1.Append(new Column() { Min = Convert.ToUInt32(iStartIndex), Max = Convert.ToUInt32(iStartIndex), Width = 10D, CustomWidth = true });

                    iIndex++;

                    if (mlstSundays.Count > iIndex)
                        dtSunday = mlstSundays[iIndex];
                }

                //int iAttendanceCount = moStudentAttendanceReport.AttendanceDetails.Where(ad1 => ad1.Date == ad.Date).Select(ad1 => ad1.LectureNo).Distinct().Count();
                int iAttendanceCount = 5;
                columns1.Append(new Column() { Min = Convert.ToUInt32(iStartIndex), Max = Convert.ToUInt32((iStartIndex - 1) + iAttendanceCount), Width = 5D, CustomWidth = true });

                iStartIndex = iStartIndex + iAttendanceCount;

            }
            );

        iStartIndex = iStartIndex + 3;
        columns1.Append(new Column() { Min = Convert.ToUInt32(iStartIndex), Max = Convert.ToUInt32(iStartIndex), Width = 15D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    private Cell AddReportCell(string asVal, CellValues aoCellValues, ExcelReportEnum aoStypeIndex)
    {
        return new Cell()
        {
            CellValue = new CellValue(asVal),
            DataType = new EnumValue<CellValues>(aoCellValues),
            StyleIndex = Convert.ToUInt16(aoStypeIndex)
        };

    }

    #endregion

    #region Student Pending Fee
    private void ExportStudentPendingFeeDetailsReport(string asFilterString)  //
    {
        int iStandardId = 0, iDivisionId = 0, iStudentId = 0, iFromYear = 0, iToYear = 0, iIncludeLateFee = 0;
        string sStartDate = "", sEndDate = "" ,sPendingTillDate = "";
        //var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_GetStudentPaidFeeDetailsForReport;1.", "").Split('@');
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").Replace("OR", "@").TrimAll().Replace("usp_GetOldPendingFeeDetailsForAllYears;1.", "").Split('@');
        
        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim().ToUpper() == "STANDARD_ID")
                    iStandardId = (oData[1].Trim()=="null"?0:oData[1].ToInt());
                else if (oData[0].Trim().ToUpper() == "DIVISION_ID")
                    iDivisionId = (oData[1].Trim() == "null" ? 0 : oData[1].ToInt());
                else if (oData[0].Trim().ToUpper() == "STUDENT_ID")
                    iStudentId = (oData[1].Trim() == "null" ? 0 : oData[1].ToInt());
                else if (oData[0].Trim().ToUpper() == "FROMYEAR")
                    iFromYear = (oData[1].Trim() == "null" ? 0 : oData[1].ToInt());
                else if (oData[0].Trim().ToUpper() == "TOYEAR")
                    iToYear = (oData[1].Trim() == "null" ? 0 : oData[1].ToInt());
                else if (oData[0].Trim().ToUpper() == "INCLUDELATEFEE")
                    iIncludeLateFee = (oData[1].Trim() == "null" ? 0 : oData[1].Trim().ToInt());
                else if (oData[0].Trim().ToUpper() == "DATEPENDINGTILLDATE")
                    sPendingTillDate = (oData[1].Trim() == "null" ? string.Empty : oData[1].ToString().TrimAll());
               else if (oData[0].Trim().ToUpper() == "DATESTARTDATE")
                    sStartDate = (oData[1].Trim() == "null" ? string.Empty : oData[1].ToString().TrimAll());
                else if (oData[0].Trim().ToUpper() == "DATEENDDATE")
                    sEndDate = (oData[1].Trim() == "null" ? string.Empty : oData[1].ToString().TrimAll());
                
            }
        }

        string sPaidFeeHeaderTitle = string.Empty;
        if (sStartDate != string.Empty && sEndDate != string.Empty)
            sPaidFeeHeaderTitle = "Paid Fee Details from " + sStartDate + " to " + sEndDate;        
        else
            sPaidFeeHeaderTitle = string.Empty;

        S_SHEET_NAME = "StudentPaidFeeDetailsReport";
        OldYearPendingFeeStudentsBL moFeeReportBL = new OldYearPendingFeeStudentsBL(miSchoolId, miAcademicYearId);
        moPendingFee = moFeeReportBL.GetOldYearPendingFeeDetails(miSchoolId, miAcademicYearId, iStudentId, iStandardId, iDivisionId, iFromYear, iToYear, iIncludeLateFee, sPendingTillDate ,sStartDate, sEndDate);

        string sFileName = "StudentPendingFeeDetails_" + Guid.NewGuid() + ".xlsx";
        string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            // CreateWorkBookForAaryan(workbookPart);
            CreateWorkBookForPendingFeeReport(workbookPart, sPaidFeeHeaderTitle);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));

    }

    /// <summary>
    /// This method is used to create work book part for student pending fee details report.
    /// </summary>
    /// <param name="aoPart"></param>
    private void CreateWorkBookForPendingFeeReport(WorkbookPart aoPart, string asPaidFeeHeaderTitle)   //
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);

        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        // GenerateStudentFeeDetailsForAaryan(worksheetPart1);
        GenerateStudentPendingFeeDetails(worksheetPart1, asPaidFeeHeaderTitle);

        
        GeneratePartContent(aoPart, "Fee Details");
    }

    /// <summary>
    /// This method is used to geenerate pending fee details.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateStudentPendingFeeDetails(WorksheetPart aoWorksheetPart1, string asPaidFeeHeaderTitle)    //
    {
         
        int iColCount = moPendingFee.OldYearPendingFeeStudents.Count;
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);
        SheetData sheetData1 = new SheetData();
        SetStudentPendingFeeDetailsColumnWidth(worksheet1, iColCount);
        AddPendingFeeHeader(sheetData1, iColCount);
        AddStudentPendingFeeDataRows(sheetData1, iColCount);
        
        if (moSchool == Constants.SchoolId.PPSH)
            AddStudentPaidFee(sheetData1, iColCount, asPaidFeeHeaderTitle);

        worksheet1.Append(sheetData1);

        worksheet1.Append(MergeCellsfForPendingFeeReport());

        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    private MergeCells MergeCellsfForPendingFeeReport()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        var oAcademicYears = moPendingFee.PendingFees.Select(fee => new { fee.AcademicYearId, fee.AcademicYear }).Distinct().OrderByDescending(fee => fee.AcademicYearId).ToList();
        int iTotalCellCount = 4 + oAcademicYears.Count + 1;
        mergeCells1.Append(new MergeCell() { Reference = "A1" + ":" + ((char)(65 + iTotalCellCount)).ToString()+"1" });

        if (moSchool == Constants.SchoolId.PPSH && moPendingFee.PaidFees.Count > 0)
        {
            int iRowIndex = moPendingFee.OldYearPendingFeeStudents.Count + 5;

            string sStartCell = "A" + iRowIndex;
            string sLastCell = string.Empty;

            if (iTotalCellCount <= 26)
                sLastCell = ((char)(65 + iTotalCellCount)).ToString() + iRowIndex;

            mergeCells1.Append(new MergeCell() { Reference = sStartCell + ":" + sLastCell });
        }        
        
        return mergeCells1;
    }

    /// <summary>
    /// This method is used to fill fee details.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddStudentPendingFeeDataRows(SheetData aoSheetData1, int iColCount)    /////
    {
        miStudentPaidFeeStartupRow++;

        var oAcademicYears = moPendingFee.PendingFees.Select(fee => new { fee.AcademicYearId, fee.AcademicYear }).Distinct().OrderBy(fee => fee.AcademicYearId).ToList();

        moPendingFee.OldYearPendingFeeStudents.OrderBy(stud => stud.OriginalStandardId).ThenBy(stud => stud.OriginalDivisionId).ThenBy(stud => stud.RollNo).ToList().ForEach
            (
            stud =>
            {
                Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

                row.Append(AddCell(stud.RegNo, CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(stud.Class, CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(stud.RollNo.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell(stud.StudentName, CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(stud.MobileNo, CellValues.String, StudentPaidFeeEnum.LeftData));
               
                foreach (var year in oAcademicYears)
                {
                    var oData = moPendingFee.PendingFees.Where(fee => fee.StudentId == stud.YearWiseStudentId && fee.AcademicYearId == year.AcademicYearId).Select(fee => fee.Amount).FirstOrDefault();
                    if (oData != null)
                        row.Append(AddCell(oData.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                    else
                        row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
                }

                int oTotalPendingFee = moPendingFee.PendingFees.Where(fee => fee.StudentId == stud.YearWiseStudentId).Sum(fee => fee.Amount);
                row.Append(AddCell(oTotalPendingFee.ToString(), CellValues.String, StudentPaidFeeEnum.CenterDataBold));

                aoSheetData1.Append(row);
                miStudentPaidFeeStartupRow++;
            }
            );


        Row rowTotal = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
        rowTotal.Append(AddCell("Total", CellValues.String, StudentPaidFeeEnum.CenterDataBold));
        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));

        foreach (var year in oAcademicYears)
        {
            var oData = moPendingFee.PendingFees.Where(fee => fee.AcademicYearId == year.AcademicYearId).Sum(fee => fee.Amount);
            if (oData != null)
                rowTotal.Append(AddCell(oData.ToString(), CellValues.String, StudentPaidFeeEnum.CenterDataBold));
            else
                rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterDataBold));
        }

        int oOverallTotalPendingFee = moPendingFee.PendingFees.Sum(fee => fee.Amount);
        rowTotal.Append(AddCell(oOverallTotalPendingFee.ToString(), CellValues.String, StudentPaidFeeEnum.CenterDataBold));

        aoSheetData1.Append(rowTotal);
        miStudentPaidFeeStartupRow++;
    }

    /// <summary>
    /// This method is used to fill fee details.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddStudentPaidFee(SheetData aoSheetData1, int iColCount, string asPaidFeeHeaderTitle)
    {
        if (moPendingFee.PaidFees.Count > 0)
        {
            var oAcademicYears = moPendingFee.PendingFees.Select(fee => new { fee.AcademicYearId, fee.AcademicYear }).Distinct().OrderBy(fee => fee.AcademicYearId).ToList();

            miStudentPaidFeeStartupRow++;
            Row rowHeaderTitle = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
            rowHeaderTitle.Append(AddCell(asPaidFeeHeaderTitle, CellValues.String, StudentPaidFeeEnum.CenterHeader));
            int iTotalCellCount = 4 + oAcademicYears.Count + 1;
            for (int iIndex = 0; iIndex < iTotalCellCount; iIndex++)
            {
                rowHeaderTitle.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
            }
            aoSheetData1.Append(rowHeaderTitle);

            /////////////////////////////////////////////////
            miStudentPaidFeeStartupRow++;
            Row rowHeader = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
                        rowHeader.Append(AddCell("Reg. No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
            rowHeader.Append(AddCell("Class", CellValues.String, StudentPaidFeeEnum.CenterHeader));
            rowHeader.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
            rowHeader.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.CenterHeader));
            rowHeader.Append(AddCell("Mobile No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
            
            foreach (var year in oAcademicYears)
            {
                rowHeader.Append(AddCell(year.AcademicYear, CellValues.String, StudentPaidFeeEnum.CenterHeader));
            }

            rowHeader.Append(AddCell("Total Paid Fee", CellValues.String, StudentPaidFeeEnum.CenterHeader));

            aoSheetData1.Append(rowHeader);
            /////////////////////////////////////////////////
            miStudentPaidFeeStartupRow++;
            moPendingFee.OldYearPaidFeeStudents.OrderBy(stud => stud.OriginalStandardId).ThenBy(stud => stud.OriginalDivisionId).ThenBy(stud => stud.RollNo).ToList().ForEach
                (
                stud =>
                {
                    Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

                    row.Append(AddCell(stud.RegNo, CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(stud.Class, CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(stud.RollNo.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(stud.StudentName, CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(stud.MobileNo, CellValues.String, StudentPaidFeeEnum.LeftData));

                    foreach (var year in oAcademicYears)
                    {
                        var oData = moPendingFee.PaidFees.Where(fee => fee.StudentId == stud.YearWiseStudentId && fee.AcademicYearId == year.AcademicYearId).Select(fee => fee.Amount).FirstOrDefault();
                        if (oData != null)
                            row.Append(AddCell(oData.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                        else
                            row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
                    }

                    int oTotalPendingFee = moPendingFee.PaidFees.Where(fee => fee.StudentId == stud.YearWiseStudentId).Sum(fee => fee.Amount);
                    row.Append(AddCell(oTotalPendingFee.ToString(), CellValues.String, StudentPaidFeeEnum.CenterDataBold));

                    aoSheetData1.Append(row);
                    miStudentPaidFeeStartupRow++;
                }
                );

            /////////////////////////////////////////////////
            Row rowTotal = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
            rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
            rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
            rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
            rowTotal.Append(AddCell("Total", CellValues.String, StudentPaidFeeEnum.CenterDataBold));
            rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));

            foreach (var year in oAcademicYears)
            {
                var oData = moPendingFee.PaidFees.Where(fee => fee.AcademicYearId == year.AcademicYearId).Sum(fee => fee.Amount);
                if (oData != null)
                    rowTotal.Append(AddCell(oData.ToString(), CellValues.String, StudentPaidFeeEnum.CenterDataBold));
                else
                    rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterDataBold));
            }

            int oOverallTotalPendingFee = moPendingFee.PaidFees.Sum(fee => fee.Amount);
            rowTotal.Append(AddCell(oOverallTotalPendingFee.ToString(), CellValues.String, StudentPaidFeeEnum.CenterDataBold));

            aoSheetData1.Append(rowTotal);
            miStudentPaidFeeStartupRow++;
        }
    }

    /// <summary>
    /// This method is used to add columns.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddPendingFeeHeader(SheetData aoSheetData1, int iColCount)   //////
    {
        var oAcademicYears = moPendingFee.PendingFees.Select(fee => new { fee.AcademicYearId, fee.AcademicYear }).Distinct().OrderBy(fee => fee.AcademicYearId).ToList();

        Row rowHeaderTitle = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow-1), CustomHeight = true, Height = 15 };
        rowHeaderTitle.Append(AddCell("Pending Fee Details", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        int iTotalCellCount = 4 + oAcademicYears.Count + 1;
        for (int iIndex = 0; iIndex < iTotalCellCount; iIndex++)
        {
            rowHeaderTitle.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        }
        aoSheetData1.Append(rowHeaderTitle);

        /////////////////////////////////////////////////
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Reg. No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Class", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Mobile No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
       
        

        foreach (var year in oAcademicYears)
        {
            row.Append(AddCell(year.AcademicYear, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        }

        row.Append(AddCell("Total Pending Fee", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row);
    }

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    /// <param name="aiNoOfDays"></param>
    private void SetStudentPendingFeeDetailsColumnWidth(Worksheet aoWorksheet1, int aiNoOfDays)    ////
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 20D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 30D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 40D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 21D, CustomWidth = true });

        int iAcademicYearCount = moPendingFee.PendingFees.Select(fee => new { fee.AcademicYearId, fee.AcademicYear }).Distinct().Count();
        columns1.Append(new Column() { Min = (UInt32Value)6U, Max = Convert.ToUInt32(5 + iAcademicYearCount), Width = 21D, CustomWidth = true });

        int iPendingFeeTotalColumnIndex = 5 + iAcademicYearCount + 1;
        columns1.Append(new Column() { Min = Convert.ToUInt32(iPendingFeeTotalColumnIndex), Max = Convert.ToUInt32(iPendingFeeTotalColumnIndex), Width = 21D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }
    #endregion

    #region Paid fee report of VP

    private void StudentPaidFeeDetailsReportVP(string asFilterString)
    {
        DateTime adFromDate = DateTime.MinValue;
        DateTime adToDate = DateTime.MinValue;
        string sStandardId = string.Empty, sSchoolwiseStandardDivisionId = string.Empty;
        int iStudentId = 0;
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_GetFeeDetailsForVP;1.", "").Split('@');

        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim().ToUpper() == "STANDARD_ID")
                    sStandardId = (oData[1].Trim() == "null" ? "0" : oData[1].ToString());
                else if (oData[0].Trim().ToUpper() == "SCHOOLWISE_STANDARD_DIVISION_ID")
                    sSchoolwiseStandardDivisionId = (oData[1].Trim() == "null" ? "0" : oData[1].ToString());
                else if (oData[0].Trim().ToUpper() == "STUDENT_ID")
                    iStudentId = (oData[1].Trim() == "null" ? 0 : oData[1].ToInt());
                else if (oData[0].Trim().ToUpper() == "FROMDATE")
                    adFromDate = (oData[1].Trim() == "null" ? DateTime.MinValue : oData[1].ToDateTime());
                else if (oData[0].Trim().ToUpper() == "TODATE")
                    adToDate = (oData[1].Trim() == "null" ? DateTime.MinValue : oData[1].ToDateTime());
                
            }
        }

        S_SHEET_NAME = "StudentFeeDetailsReportVP";
        StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL();
        moPaidFeeDetails = moStudentFeeDetailsBL.GetAllFeeDetailsForVP(miSchoolId, miAcademicYearId, sStandardId, sSchoolwiseStandardDivisionId, iStudentId, adFromDate, adToDate);

        string sFileName = "StudentFeeDetailsVP_" + Guid.NewGuid() + ".xlsx";
        string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkBookForPaidFeeOfVP(workbookPart);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));

    }
    
    // <summary>
    /// This method is used to create work book part for student pending fee details report.
    /// </summary>
    /// <param name="aoPart"></param>
    private void CreateWorkBookForPaidFeeOfVP(WorkbookPart aoPart)   //
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);
        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateStudentPaidFeeDetailsForVP(worksheetPart1);
        base.GeneratePartContent(aoPart, "Fee Details");
    }

    /// <summary>
    /// This method is used to geenerate pending fee details.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateStudentPaidFeeDetailsForVP(WorksheetPart aoWorksheetPart1)
    {
        int iColCount = 0;
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);

        SheetData sheetData1 = new SheetData();
        SetStudentPaidFeeDetailsColumnWidthforVp(worksheet1, iColCount);
        AddPaidFeeHeaderForVP(sheetData1, iColCount);
        AddStudentPaidgFeeDataRowsVP(sheetData1, iColCount);
        worksheet1.Append(sheetData1);

        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    private void AddStudentPaidgFeeDataRowsVP(SheetData aoSheetData1, int iColCount)    /////
    {
        miStudentPaidFeeStartupRow++;

        moPaidFeeDetails.StudentFeeDetailsList.Select(fd => fd.PaidDate.Date).Distinct().OrderBy(dt => dt).ToList().ForEach(dt =>
        {
            var oStudentFee = moPaidFeeDetails.StudentFeeDetailsList.Where(fd => fd.PaidDate.Date == dt).ToList();
            oStudentFee.Select(fd => new { fd.StudentId, fd.ReceiptNumber, fd.PaymentMode, fd.TransactionNumber }).Distinct().OrderBy(fd => fd.ReceiptNumber.ToInt()).ToList().ForEach(rcpt =>
            {
                var oStudeDetails = moPaidFeeDetails.StudentDetailsList.Where(stud => stud.StudentId == rcpt.StudentId).FirstOrDefault();

                var iRefuncableAmount = moPaidFeeDetails.StudentCautionMoneyDetailsList.Where(cm => cm.SchoolwiseStudentId == oStudeDetails.SchoolwiseStudentId).Select(cm => cm.CautionMoneyAmount).FirstOrDefault();

                int iTotal = oStudentFee.Where(fd => fd.StudentId == rcpt.StudentId && fd.ReceiptNumber == rcpt.ReceiptNumber && fd.PaymentMode == rcpt.PaymentMode && fd.TransactionNumber == rcpt.TransactionNumber).Sum(fd => fd.Amount);
                iTotal = iTotal + iRefuncableAmount;

                if (iTotal > 0)
                {
                    Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

                    row.Append(AddCell(dt.ToString(Constants.S_DATE_FORMAT), CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(rcpt.ReceiptNumber, CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(oStudeDetails.Class, CellValues.String, StudentPaidFeeEnum.LeftData));
                    row.Append(AddCell(rcpt.PaymentMode, CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(rcpt.TransactionNumber, CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(oStudeDetails.StudentName, CellValues.String, StudentPaidFeeEnum.LeftData));

                    moPaidFeeDetails.StudentFeeTypeConfigurationDetailsList.OrderBy(fee => fee.FeeTypeId).ToList().ForEach(fee =>
                    {
                        var iAmount = oStudentFee.Where(fd => fd.StudentId == rcpt.StudentId && fd.ReceiptNumber == rcpt.ReceiptNumber && fd.FeeType == fee.FeeType && fd.PaymentMode == rcpt.PaymentMode && fd.TransactionNumber == rcpt.TransactionNumber).Select(fd => fd.Amount).FirstOrDefault();
                        if (iAmount != null)
                            row.Append(AddCell(iAmount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                        else
                            row.Append(AddCell("0", CellValues.String, StudentPaidFeeEnum.CenterData));
                    });

                    //var iRefuncableAmount = moPaidFeeDetails.StudentCautionMoneyDetailsList.Where(cm => cm.SchoolwiseStudentId == oStudeDetails.SchoolwiseStudentId).Select(cm => cm.CautionMoneyAmount).FirstOrDefault();
                    if (iRefuncableAmount != null)
                        row.Append(AddCell(iRefuncableAmount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                    else
                    {
                        row.Append(AddCell("0", CellValues.String, StudentPaidFeeEnum.CenterData));
                        iRefuncableAmount = 0;
                    }

                    //int iTotal = oStudentFee.Where(fd => fd.StudentId == rcpt.StudentId && fd.ReceiptNumber == rcpt.ReceiptNumber && fd.PaymentMode == rcpt.PaymentMode && fd.TransactionNumber == rcpt.TransactionNumber).Sum(fd => fd.Amount);
                    //iTotal = iTotal + iRefuncableAmount;
                    row.Append(AddCell(iTotal.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));

                    aoSheetData1.Append(row);
                    miStudentPaidFeeStartupRow++;
                }                
            });
        });
    }

    /// <summary>
    /// This method is used to add columns.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddPaidFeeHeaderForVP(SheetData aoSheetData1, int iColCount)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Date", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Receipt No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Class", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Payment Mode", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Cheque / Txn. No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        moPaidFeeDetails.StudentFeeTypeConfigurationDetailsList.OrderBy(fee => fee.FeeTypeId).ToList().ForEach(fee =>
        {
            row.Append(AddCell(fee.FeeType, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        });

        row.Append(AddCell("Refundable Deposit", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Total", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row);
    }

    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    /// <param name="aiNoOfDays"></param>
    private void SetStudentPaidFeeDetailsColumnWidthforVp(Worksheet aoWorksheet1, int aiNoOfDays)    ////
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 16D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 25D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 18D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 25D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)6U, Max = (UInt32Value)6U, Width = 40D, CustomWidth = true });

        columns1.Append(new Column() { Min = (UInt32Value)7U, Max = Convert.ToUInt32(6 + moPaidFeeDetails.StudentFeeTypeConfigurationDetailsList.Count), Width = 25D, CustomWidth = true });

        int iNextColumn = Convert.ToInt32(7 + moPaidFeeDetails.StudentFeeTypeConfigurationDetailsList.Count);
        columns1.Append(new Column() { Min = Convert.ToUInt32(iNextColumn), Max = Convert.ToUInt32(iNextColumn), Width = 25D, CustomWidth = true });
        columns1.Append(new Column() { Min = Convert.ToUInt32(iNextColumn + 1), Max = Convert.ToUInt32(iNextColumn + 1), Width = 25D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    } 

    #endregion


    #region Yearwise Pending fee Student count

    private void StudentsYearwisePendingfeecountReport(string asFilterString)
    {
        string sStandardId = string.Empty, sDivisionId = string.Empty;
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_GetStudentsYearwisePendingFeeCount;1.", "").Split('@');

        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim().ToUpper() == "STANDARD_ID")
                    sStandardId = (oData[1].Trim() == "null" ? "0" : oData[1].ToString());
                else if (oData[0].Trim().ToUpper() == "DIVISION_ID")
                    sDivisionId = (oData[1].Trim() == "null" ? "0" : oData[1].ToString());
            }
        }

        S_SHEET_NAME = "StudentsYearwisePendingFeeCountDetails";
        StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL();
        moStudentsYearwisePendingfeecount = moStudentFeeDetailsBL.GetYearwisePendingFeeStudent(miSchoolId, miAcademicYearId, sStandardId, sDivisionId);

        string sFileName = "StudentsYearwisePendingfeecount_" + Guid.NewGuid() + ".xlsx";
        string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkBookForStudentsYearwisePendingfeecount(workbookPart);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
    }

    private void CreateWorkBookForStudentsYearwisePendingfeecount(WorkbookPart aoPart)   //
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);
        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateStudentsYearwisePendingfeecountDetails(worksheetPart1);
        base.GeneratePartContent(aoPart, "Fee Details");
    }

    private void GenerateStudentsYearwisePendingfeecountDetails(WorksheetPart aoWorksheetPart1)
    {
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);

        SheetData sheetData1 = new SheetData();

        SetStudentStudentsYearwisePendingfeecountDetailsColumnWidth(worksheet1);
        AddStudentsYearwisePendingfeecountDetailsHeader(sheetData1);
        AddStudentStudentsYearwisePendingfeecountDetailsDataRows(sheetData1);

        worksheet1.Append(sheetData1);

        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    private void SetStudentStudentsYearwisePendingfeecountDetailsColumnWidth(Worksheet aoWorksheet1)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 20D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 25D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 20D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 35D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 35D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)6U, Max = (UInt32Value)6U, Width = 35D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    private void AddStudentsYearwisePendingfeecountDetailsHeader(SheetData aoSheetData1)
    {
        Row rowClass = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow - 1), CustomHeight = true, Height = 25 };

        var ddlStd = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
        var ddlDiv = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;

        string sClass;
        if (ddlStd.SelectedValue == Constants.S_ZERO)
            sClass = "All";
        else if (ddlStd.SelectedValue != Constants.S_ZERO && ddlDiv.SelectedValue == Constants.S_ZERO)
            sClass = ddlStd.SelectedItem.Text + "-All";
        else
            sClass = ddlStd.SelectedItem.Text + "-" + ddlDiv.SelectedItem.Text;

        rowClass.Append(AddCell("Class : " + sClass, CellValues.String, StudentPaidFeeEnum.LeftHeader));

        aoSheetData1.Append(rowClass);
        /////////////////
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 25 };

        row.Append(AddCell("Year", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Total Students Count ", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("RTE students", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Actual Fees Receivable students", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Pending Fees students count", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Percentage of pending fees", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row);
    }

    private void AddStudentStudentsYearwisePendingfeecountDetailsDataRows(SheetData aoSheetData1)
    {
        miStudentPaidFeeStartupRow++;

        moStudentsYearwisePendingfeecount.AcademicYears.OrderBy(year => year.AcademicYearId).ToList().ForEach(year =>
            {
                Row row = new Row
                  {
                      RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow),
                      CustomHeight = true,
                      Height = 15
                  };

                row.Append(AddCell(year.AcademicYearName, CellValues.String, StudentPaidFeeEnum.CenterData));

                int totalStudents = 0;
                int rteStudents = 0;
                int pendingStudents = 0;

                var StudentsCounts = moStudentsYearwisePendingfeecount.StudentCounts.Where(sc => sc.AcademicYearId == year.AcademicYearId).ToList();

                StudentsCounts.ForEach(sc =>
                {
                    if (sc.CategoryId == 1)
                        totalStudents = sc.Count;
                    else if (sc.CategoryId == 2)
                        rteStudents = sc.Count;
                    else if (sc.CategoryId == 3)
                        pendingStudents = sc.Count;
                });

                row.Append(AddCell(totalStudents.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell(rteStudents.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));

                int actualFeeReceivedStudents = totalStudents - rteStudents;
                row.Append(AddCell(actualFeeReceivedStudents.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));

                row.Append(AddCell(pendingStudents.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));

                decimal percentage = 0;
                if (actualFeeReceivedStudents != 0)
                    percentage = (Convert.ToDecimal(pendingStudents) / actualFeeReceivedStudents) * 100;
                row.Append(AddCell(percentage.ToString("0.00"), CellValues.Number, StudentPaidFeeEnum.CenterData));

                aoSheetData1.Append(row);
                miStudentPaidFeeStartupRow++;
            });
    }
    #endregion

    #region Internal Paid fee exam details

    private void StudentInternalPaidFeeExamDetailsReport(string asFilterString)
    {
         string sStandardId = string.Empty, sDivisionId = string.Empty;    
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_ExportCompetitiveExamwiseDetails;1.", "").Split('@');

        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim().ToUpper() == "STANDARD_ID")
                    sStandardId = (oData[1].Trim() == "null" ? "0" : oData[1].ToString());
                else if (oData[0].Trim().ToUpper() == "DIVISION_ID")
                    sDivisionId = (oData[1].Trim() == "null" ? "0" : oData[1].ToString());
            }
        }

        S_SHEET_NAME = "StudentInternalPaidFeeDetailsReport";
        StudentFeeDetailsBL moStudentFeeDetailsBL = new StudentFeeDetailsBL();
        moInternalPaidFeeExamDetails = moStudentFeeDetailsBL.GetCompetitiveExamwiseDetails(miSchoolId, miAcademicYearId, sStandardId, sDivisionId);

        string sFileName = "StudentInternalPaidFeeDetailsReport_" + Guid.NewGuid() + ".xlsx";
        string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkBookForInternalPaidfeeExamDetails(workbookPart);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
    }

    private void CreateWorkBookForInternalPaidfeeExamDetails(WorkbookPart aoPart)   //
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);
        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateStudentInternalPaidFeeExamDetails(worksheetPart1);
        base.GeneratePartContent(aoPart, "Fee Details");
    }


    private void GenerateStudentInternalPaidFeeExamDetails(WorksheetPart aoWorksheetPart1)
    {    
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);

        SheetData sheetData1 = new SheetData();
        
        SetStudentPaidInternalFeeExamDetailsColumnWidth(worksheet1);
        AddInternalPaidFeeExamHeader(sheetData1);
        AddStudentPaidInternalFeeExamDataRows(sheetData1);

        worksheet1.Append(sheetData1);

        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    private void SetStudentPaidInternalFeeExamDetailsColumnWidth(Worksheet aoWorksheet1)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 10D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 18D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 18D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 15D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)6U, Max = (UInt32Value)6U, Width = 15D, CustomWidth = true });

        int iColumnCount = moInternalPaidFeeExamDetails.DebitPayables.Count;
        columns1.Append(new Column() { Min = (UInt32Value)7U, Max = Convert.ToUInt32(7+iColumnCount), Width = 22D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    private void AddInternalPaidFeeExamHeader(SheetData aoSheetData1)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("First Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Middle Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Last Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Section", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Mobile No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        moInternalPaidFeeExamDetails.DebitPayables.OrderBy(fee => fee.PayableFor).ToList().ForEach(fee =>
        {
            row.Append(AddCell(fee.PayableFor, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        });

        aoSheetData1.Append(row);
    }

    private void AddStudentPaidInternalFeeExamDataRows(SheetData aoSheetData1)    /////
    {
        miStudentPaidFeeStartupRow++;
        moInternalPaidFeeExamDetails.StudentList.OrderBy(sl => sl.RollNo).ToList().ForEach(
            stud =>
            {
                Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

                row.Append(AddCell(stud.RollNo.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell(stud.FirstName, CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(stud.MiddleName, CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(stud.LastName, CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(stud.ClassName, CellValues.String, StudentPaidFeeEnum.LeftData));
                row.Append(AddCell(stud.MobileNumber, CellValues.String, StudentPaidFeeEnum.CenterData));

                moInternalPaidFeeExamDetails.DebitPayables.OrderBy(fee => fee.PayableFor).ToList().ForEach(fee =>
                {
                    if(moInternalPaidFeeExamDetails.CreditEntries.Any(cd => cd.SchoolwiseStudentId == stud.SchoolwiseStudentId && cd.PayableFor == fee.PayableFor))
                        row.Append(AddCell("Yes", CellValues.String, StudentPaidFeeEnum.CenterData));
                    else
                        row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
                });
                                
                aoSheetData1.Append(row);
                miStudentPaidFeeStartupRow++;
            }
            );

        /////////////////////////////////////////////////////////////

        Row rowTotal = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 20 };

        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
        rowTotal.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftData));
        rowTotal.Append(AddCell("Total", CellValues.String, StudentPaidFeeEnum.CenterData));

        moInternalPaidFeeExamDetails.DebitPayables.OrderBy(fee => fee.PayableFor).ToList().ForEach(fee =>
        {
            int iCount = moInternalPaidFeeExamDetails.CreditEntries.Count(cd => cd.PayableFor == fee.PayableFor);
            rowTotal.Append(AddCell(iCount.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));                    
        });

        aoSheetData1.Append(rowTotal);
        miStudentPaidFeeStartupRow++;                       
    }

   #endregion

    #region VP Test consolidated report

    private void ExportMarkDetailsForTestwiseReport(string asFilterString)
    {
        int iStandardId = 0, iStdDivId = 0, iTestId = 0;
        var oFilters = asFilterString.Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("AND", "@").TrimAll().Replace("usp_GetMarksForTestwiseConsolidatedReport;1.", "").Split('@');

        foreach (string sVal in oFilters)
        {
            var oData = sVal.Split('=');
            if (oData.Length > 0)
            {
                if (oData[0].Trim().ToUpper() == "STANDARD_ID")
                    iStandardId = (oData[1].Trim() == "null" ? 0 : oData[1].ToInt());
                else if (oData[0].Trim().ToUpper() == "DIVISION_ID")
                    iStdDivId = (oData[1].Trim() == "null" ? 0 : oData[1].ToInt());
                else if (oData[0].Trim().ToUpper() == "SCHOOLWISE_TEST_ID")
                    iTestId = (oData[1].Trim() == "null" ? 0 : oData[1].ToInt());  
            }
        }

        S_SHEET_NAME = "ConsolidatedReport";
        ExamReportBL oExamReportBL = new ExamReportBL();
        moTestwiseMark = oExamReportBL.GetMarkDetailsForTestwiseReport(miSchoolId, miAcademicYearId, iStandardId, iStdDivId, iTestId);
        var ddlStd = grdDisplayParameter.Rows[0].FindControl("DDLRptParameter") as ComboRpt;
        var ddlDiv = grdDisplayParameter.Rows[1].FindControl("DDLRptParameter") as ComboRpt;

        DateTime academicStart = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]);
        DateTime academicEnd = Convert.ToDateTime(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
        string academicYear = academicStart.Year + "-" + academicEnd.Year;

        string sFileName = "TestConsolidatedReportVP_" + ddlStd.SelectedItem.Text.Trim()+"-"+ ddlDiv.SelectedItem.Text.Trim() + "_" + academicYear + "_" + Guid.NewGuid() + ".xlsx";  
        //string sFileName = "TestConsolidatedReportVP_" + Guid.NewGuid() + ".xlsx";
        string filePath = base.BasePath + @"\RITeSchool\UPLOADS\ResultSheet\" + sFileName;

        using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            CreateWorkBookForTestwiseConsolidatedReportVP(workbookPart);
        }

        Response.Write(string.Format("<Script language='Javascript'>window.open('../UPLOADS/ResultSheet/" + sFileName + "')</Script>"));
    }

    // <summary>
    /// This method is used to create work book part for student pending fee details report.
    /// </summary>
    /// <param name="aoPart"></param>
    private void CreateWorkBookForTestwiseConsolidatedReportVP(WorkbookPart aoPart)   //
    {
        WorkbookStylesPart workbookStylesPart1 = aoPart.AddNewPart<WorkbookStylesPart>("rId3");
        base.GenerateReportStyles(workbookStylesPart1);
        WorksheetPart worksheetPart1 = aoPart.AddNewPart<WorksheetPart>("rId1");
        GenerateTestwiseConsolidatedReportForVP(worksheetPart1);
        base.GeneratePartContent(aoPart, "Consolidated Report");
    }

    /// <summary>
    /// This method is used to geenerate pending fee details.
    /// </summary>
    /// <param name="aoWorksheetPart1"></param>
    private void GenerateTestwiseConsolidatedReportForVP(WorksheetPart aoWorksheetPart1)
    {
        int iColCount = 0;
        Worksheet worksheet1 = new Worksheet();
        worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
        base.AddSheetDetails(worksheet1);

        SheetData sheetData1 = new SheetData();

        if (!moTestwiseMark.OtherDetails.DisplaySubTypes)
        {
            SetWidthforTestwiseConsolReportVP(worksheet1, iColCount);
            AddTestConsolReportHeaderRowsVP(sheetData1, iColCount);
            AddHeaderForTestwiseConsolReportVP(sheetData1, iColCount);
            AddTestConsolReportRowsVP(sheetData1, iColCount);
            AddTestConsolReportFooterRowsVP(sheetData1, iColCount);
        }
        else
        {
            AddTestConsolReportHeaderRowsVP3To9(sheetData1);
            SetWidthforTestwiseConsolReportVP3to9(worksheet1);
            AddHeaderForTestwiseConsolReportVP3to9(sheetData1);
            AddTestConsolReportRowsVP3To9(sheetData1);
            AddTestConsolReportFooterRowsVP3To9(sheetData1);
        }

        worksheet1.Append(sheetData1);

        if (!moTestwiseMark.OtherDetails.DisplaySubTypes)
        {
            worksheet1.Append(MergeCellsfForTestConsolreportVP());
        }
        else
        {
            worksheet1.Append(MergeCellsfForTestConsolreportVP3To9());
        }

        base.AddPrintOptions(worksheet1);
        base.SetPageMargin(worksheet1, 0.2);
        base.SetPageSetup(worksheet1, OrientationValues.Landscape);
        aoWorksheetPart1.Worksheet = worksheet1;
    }

    private void AddTestConsolReportHeaderRowsVP(SheetData aoSheetData1, int iColCount)
    {
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row.Append(AddCell(moTestwiseMark.OtherDetails.SchoolName, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        int iCount = 1 + moTestwiseMark.Subjects.Count() + moTestwiseMark.Subjects.Count(sb => sb.IsGradeApplicable) + 3;

        for (int iIndex = 0; iIndex < iCount; iIndex++)
            row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        aoSheetData1.Append(row);

        miStudentPaidFeeStartupRow++;

        var oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
        string sHeaderText = "Rollwise Result Sheet for " + moTestwiseMark.OtherDetails.TestName;
        if (oDropDownList.SelectedValue == "2")
            sHeaderText = "Rankwise Result Sheet for " + moTestwiseMark.OtherDetails.TestName;

        Row row1 = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row1.Append(AddCell(sHeaderText, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        for (int iIndex = 0; iIndex < iCount; iIndex++)
            row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        aoSheetData1.Append(row1);

        miStudentPaidFeeStartupRow++;
        //-------------------
        Row row2 = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row2.Append(AddCell("Class : " + moTestwiseMark.OtherDetails.ClassName, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        for (int iIndex = 0; iIndex < iCount; iIndex++)
            row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));

        aoSheetData1.Append(row2);
    }

    private void AddTestConsolReportFooterRowsVP(SheetData aoSheetData1, int iColCount)
    {
        miStudentPaidFeeStartupRow = miStudentPaidFeeStartupRow + 4;
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row.Append(AddCell(moTestwiseMark.OtherDetails.TeacherName, CellValues.String, StudentPaidFeeEnum.RightDataWithNoBorder));

        int iCount = 1 + moTestwiseMark.Subjects.Count() + moTestwiseMark.Subjects.Count(sb => sb.IsGradeApplicable) + 3;

        for (int iIndex = 0; iIndex < iCount; iIndex++)
            row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.RightDataWithNoBorder));

        aoSheetData1.Append(row);
        //-------------------
        miStudentPaidFeeStartupRow++;
        Row row1 = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row1.Append(AddCell("Class Teacher", CellValues.String, StudentPaidFeeEnum.RightDataWithNoBorder));

        for (int iIndex = 0; iIndex < iCount; iIndex++)
            row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.RightDataWithNoBorder));

        aoSheetData1.Append(row1);
    }

    private MergeCells MergeCellsfForTestConsolreportVP()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        int iCount = 1 + moTestwiseMark.Subjects.Count() + moTestwiseMark.Subjects.Count(sb => sb.IsGradeApplicable) + 3;

        string sLastCell = ((char)(65 + iCount)).ToString();

        mergeCells1.Append(new MergeCell() { Reference = "A2" + ":" + sLastCell + "2" });
        mergeCells1.Append(new MergeCell() { Reference = "A3" + ":" + sLastCell + "3" });
        mergeCells1.Append(new MergeCell() { Reference = "A4" + ":" + sLastCell + "4" });

        int iSeqNo = 2;
        List<int> lstSubjectsIds = new List<int>();
        moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(sub =>
        {
            if (sub.IsGradeApplicable)
            {
                string sStartCell = ((char)(65 + iSeqNo)).ToString();
                string sEndCell = ((char)(65 + iSeqNo + 1)).ToString();
                mergeCells1.Append(new MergeCell() { Reference = sStartCell + "5" + ":" + sEndCell + "5" });
                iSeqNo++;
            }

            iSeqNo++;
        });

        int iIndex = 7 + moTestwiseMark.StudentDetails.Count() + 3;

        mergeCells1.Append(new MergeCell() { Reference = "A" + iIndex + ":" + sLastCell + iIndex });
        mergeCells1.Append(new MergeCell() { Reference = "A" + (iIndex + 1) + ":" + sLastCell + (iIndex + 1) });

        return mergeCells1;
    }
        
    /// <summary>
    ///  This method is used to get cell name.
    /// </summary>
    /// <param name="aiCellIndex"></param>
    /// <returns></returns>
    public string GetCellCharacter(int aiCellIndex)
    {
        string sCell;
        if (aiCellIndex >= 52)
            sCell = "B" + ((char)(65 + (aiCellIndex - 52)));
        else if (aiCellIndex >= 26)
            sCell = "A" + ((char)(65 + (aiCellIndex - 26)));
        else
            sCell = ((char)(65 + aiCellIndex)).ToString(); 
        return sCell;
    }

    private void AddTestConsolReportRowsVP(SheetData aoSheetData1, int iColCount)
    {
        miStudentPaidFeeStartupRow++;

        List<StudentDetailsForTestReport> lstStudOrder = moTestwiseMark.StudentDetails.OrderBy(stud => stud.RollNo).ToList();

        var oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
        if (oDropDownList.SelectedValue == "2")
        {
            lstStudOrder = (from stud in moTestwiseMark.StudentDetails
                      join sm in moTestwiseMark.MarkSummary
                      on stud.YearWiseStudentId equals sm.StudentId
                      orderby sm.Rank
                      select stud).ToList();
        }

        lstStudOrder.ToList().ForEach(stud =>
        {
            Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

            row.Append(AddCell(stud.RollNo.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(stud.StudentName, CellValues.String, StudentPaidFeeEnum.LeftData));

            moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(sub =>
            {
                var oMarkDetails = moTestwiseMark.Marks.Where(mk => mk.StudentId == stud.YearWiseStudentId && mk.SubjectId == sub.SubjectId).FirstOrDefault();
                if (oMarkDetails != null)
                {
                    if (sub.IsGradeApplicable)
                    {                       
                        var sGradeName = moTestwiseMark.Grades.Where(gd => oMarkDetails.Percentage >= gd.StartingMarkRange && oMarkDetails.Percentage <= gd.EndingMarkRange).Select(gd => gd.GradeName).FirstOrDefault();

                        if (sGradeName != null)
                        {
                            string sGrade = (oMarkDetails.IsAbsent == "Y" ? "Ab" : sGradeName);
                            row.Append(AddCell(sGrade, CellValues.String, StudentPaidFeeEnum.CenterData));
                        }
                        else
                            row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
                    }

                    if (oMarkDetails.IsAbsent == "Y")
                        row.Append(AddCell("Ab", CellValues.String, StudentPaidFeeEnum.CenterData));
                    else
                        row.Append(AddCell(oMarkDetails.TotalMarksScored.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                }
            });

            var oTotalMarks = moTestwiseMark.MarkSummary.Where(sm => sm.StudentId == stud.YearWiseStudentId).FirstOrDefault();

            if (oTotalMarks != null && oTotalMarks.TotalMarks != null)
            {
                bool IsAbsentCase = moTestwiseMark.Marks.Any(mk => mk.StudentId == stud.YearWiseStudentId && mk.IsAbsent == "Y");

                if (IsAbsentCase)
                {
                    row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));                    
                    row.Append(AddCell(oTotalMarks.Rank.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                }
                else
                {
                    row.Append(AddCell(oTotalMarks.TotalMarks.ToInt().ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(oTotalMarks.Percentage.ToString("F2"), CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(oTotalMarks.Rank.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                }
            }
            else
            {
                row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
            }

            aoSheetData1.Append(row);
            miStudentPaidFeeStartupRow++;
        });
    }

    /// <summary>
    /// This method is used to add columns.
    /// </summary>
    /// <param name="aoSheetData1"></param>
    /// <param name="iColCount"></param>
    private void AddHeaderForTestwiseConsolReportVP(SheetData aoSheetData1, int iColCount)
    {
        miStudentPaidFeeStartupRow++;
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));

        moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(sub =>
        {

            row.Append(AddCell(sub.SubjectName, CellValues.String, StudentPaidFeeEnum.CenterHeader));
            if (sub.IsGradeApplicable)
                row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        });

        row.Append(AddCell("Total", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Percentage", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Rank", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row);
    }
        
    /// <summary>
    /// This method is used to set column width.
    /// </summary>
    /// <param name="aoWorksheet1"></param>
    /// <param name="aiNoOfDays"></param>
    private void SetWidthforTestwiseConsolReportVP(Worksheet aoWorksheet1, int aiNoOfDays)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 10D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 40D, CustomWidth = true });

        int iSubjectCount = moTestwiseMark.Subjects.Count();
        int iGradeSubjectCount = moTestwiseMark.Subjects.Count(sub => sub.IsGradeApplicable);

        iSubjectCount = 2 + iSubjectCount + iGradeSubjectCount + 3;

        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = Convert.ToUInt32(iSubjectCount), Width = 15D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    #region VP Consolidated report 3-10

    private void SetWidthforTestwiseConsolReportVP3to9(Worksheet aoWorksheet1)
    {
        Columns columns1 = new Columns();
        columns1.Append(new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 10D, CustomWidth = true });
        columns1.Append(new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 40D, CustomWidth = true });

        int iSubjectCount = moTestwiseMark.Subjects.Count();
        int iGradeSubjectCount = moTestwiseMark.Subjects.Count(sub => sub.IsGradeApplicable);
        int iGroupSubjectCount = moTestwiseMark.Subjects.Where(sb => sb.ParentSubjectId != 0).Select(sb => sb.ParentSubjectId).ToList().Distinct().Count();

        //iSubjectCount = 2 + iSubjectCount + iGradeSubjectCount + 4 + iGroupSubjectCount;
        iSubjectCount = 2 + iSubjectCount + iGradeSubjectCount + (iGroupSubjectCount * 2) + 3;

        columns1.Append(new Column() { Min = (UInt32Value)3U, Max = Convert.ToUInt32(iSubjectCount), Width = 15D, CustomWidth = true });

        aoWorksheet1.Append(columns1);
    }

    private void AddTestConsolReportHeaderRowsVP3To9(SheetData aoSheetData1)
    {
        // Add school name
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row.Append(AddCell(moTestwiseMark.OtherDetails.SchoolName, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        aoSheetData1.Append(row);

        //Add test name headers.
        miStudentPaidFeeStartupRow++;
        var oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
        string sHeaderText = "Rollwise Result Sheet for " + moTestwiseMark.OtherDetails.TestName;
        if (oDropDownList.SelectedValue == "2")
            sHeaderText = "Rankwise Result Sheet for " + moTestwiseMark.OtherDetails.TestName;

        Row row1 = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row1.Append(AddCell(sHeaderText, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        aoSheetData1.Append(row1);

        //Add class header.
        miStudentPaidFeeStartupRow++;
        Row row2 = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row2.Append(AddCell("Class : " + moTestwiseMark.OtherDetails.ClassName, CellValues.String, StudentPaidFeeEnum.NoBorderCenterHeader));
        aoSheetData1.Append(row2);
    }

    private void AddTestConsolReportFooterRowsVP3To9(SheetData aoSheetData1)
    {
        // Add teacher name
        miStudentPaidFeeStartupRow = miStudentPaidFeeStartupRow + 4;
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row.Append(AddCell(moTestwiseMark.OtherDetails.TeacherName, CellValues.String, StudentPaidFeeEnum.RightDataWithNoBorder));
        aoSheetData1.Append(row);

        // Add class teacher header.
        miStudentPaidFeeStartupRow++;
        Row row1 = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };
        row1.Append(AddCell("Class Teacher", CellValues.String, StudentPaidFeeEnum.RightDataWithNoBorder));
        aoSheetData1.Append(row1);
    }

    private MergeCells MergeCellsfForTestConsolreportVP3To9()
    {
        MergeCells mergeCells1 = new MergeCells() { Count = (UInt32Value)1U };

        int iCellIndex = 2;
        List<int> lstSubjectsIds = new List<int>();
        string sStartCell;
        string sEndCell;
        int iEndCellIndex;
        int iLastParentSubjectId = -1;

        // Merge grading subject cells
        moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(sub =>
        {
            if (iLastParentSubjectId != -1 && (iLastParentSubjectId != sub.ParentSubjectId || sub.ParentSubjectId == 0))
            {
                iCellIndex = iCellIndex + 2;
                iLastParentSubjectId = -1;
            }

            sStartCell = GetCellCharacter(iCellIndex);

            if (sub.IsGradeApplicable)
                iEndCellIndex = iCellIndex + 1;
            else
                iEndCellIndex = iCellIndex;

            sEndCell = GetCellCharacter(iEndCellIndex);

            mergeCells1.Append(new MergeCell() { Reference = sStartCell + "6" + ":" + sEndCell + "6" });
            iCellIndex = iEndCellIndex + 1;

            if (sub.ParentSubjectId != 0 && iLastParentSubjectId == -1)
                iLastParentSubjectId = sub.ParentSubjectId;
        }
        );

        int iStartIndex = 3;
        iLastParentSubjectId = -1;
        // MErge group subject cells.
        moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(
        sub =>
        {
            if (iLastParentSubjectId != -1 && (iLastParentSubjectId != sub.ParentSubjectId || sub.ParentSubjectId == 0))
            {
                iEndCellIndex = iStartIndex;
                iLastParentSubjectId = -1;

                sStartCell = GetCellCharacter(iCellIndex);
                sEndCell = GetCellCharacter(iEndCellIndex);

                mergeCells1.Append(new MergeCell() { Reference = sStartCell + "5" + ":" + sEndCell + "5" });

                iStartIndex = iEndCellIndex + 2;
            }

            if (sub.ParentSubjectId != 0 && iLastParentSubjectId == -1)
            {
                iLastParentSubjectId = sub.ParentSubjectId;
                iCellIndex = iStartIndex - 1;
            }

            if (sub.IsGradeApplicable)
                iStartIndex = iStartIndex + 2;
            else
                iStartIndex = iStartIndex + 1;
        });

        // Merge header and footer cells.
        int iGrpSubCount = moTestwiseMark.Subjects.Where(sb => sb.ParentSubjectId != 0).Select(sb => sb.ParentSubjectId).Distinct().Count();
        int iCount = 1 + (moTestwiseMark.Subjects.Count()) + moTestwiseMark.Subjects.Count(sb => sb.IsGradeApplicable) + (iGrpSubCount * 2) + 3;

        string sLastCell = GetCellCharacter(iCount);

        mergeCells1.Append(new MergeCell() { Reference = "A2" + ":" + sLastCell + "2" });
        mergeCells1.Append(new MergeCell() { Reference = "A3" + ":" + sLastCell + "3" });
        mergeCells1.Append(new MergeCell() { Reference = "A4" + ":" + sLastCell + "4" });

        mergeCells1.Append(new MergeCell() { Reference = "A" + (miStudentPaidFeeStartupRow - 1) + ":" + sLastCell + (miStudentPaidFeeStartupRow - 1) });
        mergeCells1.Append(new MergeCell() { Reference = "A" + miStudentPaidFeeStartupRow + ":" + sLastCell + miStudentPaidFeeStartupRow });

        return mergeCells1;
    }

    private void AddTestConsolReportRowsVP3To9(SheetData aoSheetData1)
    {
        miStudentPaidFeeStartupRow++;

        List<StudentDetailsForTestReport> lstStudOrder = moTestwiseMark.StudentDetails.OrderBy(stud => stud.RollNo).ToList();

        var oDropDownList = grdDisplayParameter.Rows[2].FindControl("DDLRptParameter") as ComboRpt;
        if (oDropDownList.SelectedValue == "2")
        {
            lstStudOrder = (from stud in moTestwiseMark.StudentDetails
                            join sm in moTestwiseMark.MarkSummary
                            on stud.YearWiseStudentId equals sm.StudentId
                            orderby sm.Rank
                            select stud).ToList();
        }

        int iLastParentSubjectId = -1;
        lstStudOrder.ToList().ForEach(stud =>
        {
            Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

            row.Append(AddCell(stud.RollNo.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
            row.Append(AddCell(stud.StudentName, CellValues.String, StudentPaidFeeEnum.LeftData));

            moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(sub =>
            {
                // This condition is show/handle group total and average.
                if (iLastParentSubjectId != -1 && (iLastParentSubjectId != sub.ParentSubjectId || sub.ParentSubjectId == 0))
                {
                    List<int> lstSubjectIds = moTestwiseMark.Subjects.Where(sb => sb.ParentSubjectId == iLastParentSubjectId).Select(sb => sb.SubjectId).ToList();

                    decimal dcTotal = moTestwiseMark.Marks.Where(mk => mk.StudentId == stud.YearWiseStudentId && lstSubjectIds.Contains(mk.SubjectId)).Sum(mk => mk.TotalMarksScored);

                    //decimal dcPercentage = Math.Ceiling(dcTotal / lstSubjectIds.Count);

                    decimal dcAnswer = dcTotal / lstSubjectIds.Count;
                    decimal dcDecimalPart = dcAnswer - Math.Truncate(dcAnswer);

                    if (dcDecimalPart == Convert.ToDecimal(0.5))
                        dcAnswer = dcAnswer + Convert.ToDecimal(0.01);

                    decimal dcPercentage = Math.Round(dcAnswer);

                    if (moTestwiseMark.Marks.Any(mk => mk.StudentId == stud.YearWiseStudentId && lstSubjectIds.Contains(mk.SubjectId) && mk.IsAbsent == "Y"))
                    {
                        row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                        row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                    }
                    else
                    {
                        row.Append(AddCell(dcTotal.ToInt().ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                        row.Append(AddCell(dcPercentage.ToString(), CellValues.String, StudentPaidFeeEnum.CenterData));
                    }
                    iLastParentSubjectId = -1;
                    lstSubjectIds.Clear();
                }

                var oMarkDetails = moTestwiseMark.Marks.Where(mk => mk.StudentId == stud.YearWiseStudentId && mk.SubjectId == sub.SubjectId).FirstOrDefault();
                if (oMarkDetails != null)
                {
                    if (sub.IsGradeApplicable)
                    {
                        var sGradeName = moTestwiseMark.Grades.Where(gd => oMarkDetails.Percentage >= gd.StartingMarkRange && oMarkDetails.Percentage <= gd.EndingMarkRange).Select(gd => gd.GradeName).FirstOrDefault();

                        if (sGradeName != null)
                        {
                            string sGrade = (oMarkDetails.IsAbsent == "Y" ? "Ab" : sGradeName);
                            row.Append(AddCell(sGrade, CellValues.String, StudentPaidFeeEnum.CenterData));
                        }
                        else
                            row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
                    }

                    if (oMarkDetails.IsAbsent == "Y")
                        row.Append(AddCell("Ab", CellValues.String, StudentPaidFeeEnum.CenterData));
                    else
                        row.Append(AddCell(oMarkDetails.TotalMarksScored.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                }
                else
                {
                    if (sub.IsGradeApplicable)
                        row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                }

                if (sub.ParentSubjectId != 0 && iLastParentSubjectId == -1)
                    iLastParentSubjectId = sub.ParentSubjectId;
            });

            var oTotalMarks = moTestwiseMark.MarkSummary.Where(sm => sm.StudentId == stud.YearWiseStudentId).FirstOrDefault();

            if (oTotalMarks != null && oTotalMarks.TotalMarks != null)
            {
                //List<int> lstSubjectIds = moTestwiseMark.Subjects.Where(sb => sb.ParentSubjectId != 0).Select(sb => sb.SubjectId).ToList();
                List<int> lstGradedSubjectIds = moTestwiseMark.Subjects.Where(sb => sb.IsGradeApplicable == true).Select(sb => sb.SubjectId).ToList();
                //bool IsAbsentCase = moTestwiseMark.Marks.Any(mk => mk.StudentId == stud.YearWiseStudentId && mk.IsAbsent == "Y" && !lstSubjectIds.Contains(mk.SubjectId) && !lstGradedSubjectIds.Contains(mk.SubjectId));

                bool IsAbsentCase = moTestwiseMark.Marks.Any(mk => mk.StudentId == stud.YearWiseStudentId && mk.IsAbsent == "Y" && !lstGradedSubjectIds.Contains(mk.SubjectId));

                //if (!IsAbsentCase)
                //{
                //    if (lstSubjectIds.Count > 0)
                //    {
                //        moTestwiseMark.Marks.Where(mk => mk.StudentId == stud.YearWiseStudentId && mk.IsAbsent == "Y" && lstSubjectIds.Contains(mk.SubjectId)).Select(mk => mk.SubjectId).ToList().ForEach(
                //            subId =>
                //            {
                //                int iprSubId = moTestwiseMark.Subjects.Where(sb => sb.SubjectId == subId).Select(sb => sb.ParentSubjectId).FirstOrDefault();
                //                List<int> lstGrpSubjects = moTestwiseMark.Subjects.Where(sb => sb.ParentSubjectId == iprSubId && sb.SubjectId != subId).Select(sb => sb.SubjectId).ToList();

                //                if (moTestwiseMark.Marks.Any(mk => mk.StudentId == stud.YearWiseStudentId && mk.IsAbsent == "Y" && mk.SubjectId != subId && lstGrpSubjects.Contains(mk.SubjectId) && !lstGradedSubjectIds.Contains(mk.SubjectId)))
                //                    IsAbsentCase = true;

                //                lstGrpSubjects.Clear();
                //            }
                //            );

                //        lstSubjectIds.Clear();
                //    }
                //}

                if (IsAbsentCase)
                {
                    row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell("-", CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(oTotalMarks.Rank.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                }
                else
                {
                    row.Append(AddCell(oTotalMarks.TotalMarks.ToInt().ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(oTotalMarks.Percentage.ToString("F2"), CellValues.String, StudentPaidFeeEnum.CenterData));
                    row.Append(AddCell(oTotalMarks.Rank.ToString(), CellValues.Number, StudentPaidFeeEnum.CenterData));
                }
            }
            else
            {
                row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
                row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterData));
            }

            aoSheetData1.Append(row);
            miStudentPaidFeeStartupRow++;
        });
    }

    private void AddHeaderForTestwiseConsolReportVP3to9(SheetData aoSheetData1)
    {
        miStudentPaidFeeStartupRow++;
        Row row2 = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftHeader));

        int iLastParentSubjectId = -1;

        moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(sub =>
        {
            if (iLastParentSubjectId != -1 && (iLastParentSubjectId != sub.ParentSubjectId || sub.ParentSubjectId == 0))
            {
                row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
                iLastParentSubjectId = -1;
            }

            if (sub.ParentSubjectId != 0 && iLastParentSubjectId == -1)
            {
                iLastParentSubjectId = sub.ParentSubjectId;
                row2.Append(AddCell(sub.ParentSubjectName, CellValues.String, StudentPaidFeeEnum.CenterHeader));
            }

            row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
            if (sub.IsGradeApplicable)
                row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        });

        row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row2.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row2);
        //---------------------------
        miStudentPaidFeeStartupRow++;
        Row row = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        row.Append(AddCell("Roll No.", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        row.Append(AddCell("Student Name", CellValues.String, StudentPaidFeeEnum.LeftHeader));
        iLastParentSubjectId = -1;

        int iSubjectTotalOutOfMarks = 0, iSubCont = 0;
        moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(sub =>
        {
            if (iLastParentSubjectId != -1 && (iLastParentSubjectId != sub.ParentSubjectId || sub.ParentSubjectId == 0))
            {
                row.Append(AddCell("Total (" + iSubjectTotalOutOfMarks + ")", CellValues.String, StudentPaidFeeEnum.CenterHeader));
                //decimal dcAvg = Math.Ceiling((decimal)(iSubjectTotalOutOfMarks / iSubCont));

                decimal dcAnswer = (decimal)(iSubjectTotalOutOfMarks / iSubCont);
                decimal dcDecimalPart = dcAnswer - Math.Truncate(dcAnswer);

                if (dcDecimalPart == Convert.ToDecimal(0.5))
                    dcAnswer = dcAnswer + Convert.ToDecimal(0.01);

                decimal dcAvg = Math.Round(dcAnswer);

                row.Append(AddCell("Avg (" + dcAvg + ")", CellValues.String, StudentPaidFeeEnum.CenterHeader));
                iLastParentSubjectId = -1;
                iSubjectTotalOutOfMarks = 0;
                iSubCont = 0;
            }

            row.Append(AddCell(sub.SubjectName + " (" + sub.SubjectTotalMarks + ")", CellValues.String, StudentPaidFeeEnum.CenterHeader));
            if (sub.IsGradeApplicable)
                row.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));

            if (sub.ParentSubjectId != 0 && iLastParentSubjectId == -1)
                iLastParentSubjectId = sub.ParentSubjectId;

            if (sub.ParentSubjectId != 0)
            {
                iSubjectTotalOutOfMarks += sub.SubjectTotalMarks;
                iSubCont++;
            }
        });

        row.Append(AddCell("G. Total", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Percentage", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        row.Append(AddCell("Rank", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        aoSheetData1.Append(row);

        //-----------------
        //    miStudentPaidFeeStartupRow++;
        //    Row row1 = new Row { RowIndex = Convert.ToUInt32(miStudentPaidFeeStartupRow), CustomHeight = true, Height = 15 };

        //    row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftHeader));
        //    row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.LeftHeader));

        //    iLastParentSubjectId = -1;
        //    moTestwiseMark.Subjects.OrderBy(sub => sub.SortOrder).ToList().ForEach(sub =>
        //    {
        //        if (iLastParentSubjectId != -1 && sub.ParentSubjectId == 0)
        //        {
        //            row1.Append(AddCell("TOT", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        //            row1.Append(AddCell("AVG", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        //            iLastParentSubjectId = -1;
        //        }

        //        row1.Append(AddCell("THR", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        //        row1.Append(AddCell("ORL", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        //        if (sub.IsGradeApplicable)
        //            row1.Append(AddCell("GRD", CellValues.String, StudentPaidFeeEnum.CenterHeader));
        //        row1.Append(AddCell("TOT", CellValues.String, StudentPaidFeeEnum.CenterHeader));

        //        if (sub.ParentSubjectId != 0 && iLastParentSubjectId == -1)
        //            iLastParentSubjectId = sub.ParentSubjectId;
        //    });

        //    row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        //    row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));
        //    row1.Append(AddCell(string.Empty, CellValues.String, StudentPaidFeeEnum.CenterHeader));

        //    aoSheetData1.Append(row1);
    }

    #endregion

    #endregion

    protected void btnViewReport_Click(object sender, EventArgs e)
    {

    }

    private enum MusterRollEnum
    {
        LeftHeader = 1,
        CenterHeader = 2,
        LeftData = 3,
        CenterData = 4,
        PresentData = 5,
        AbsentData = 6,
        NoBorderCenterHeader = 7,
        NoBorderLeftBoldHeader = 8,
        NoBorderLeftHeader = 9,
        Weekend = 10,
        Holiday = 11,
        Percentage = 12,
        SummaryRow = 13,
        CenterDataBold = 14,
        OutsideAcademicYear = 15,
        LateJoinee = 16,
        LeftStudent = 17,
        AttendanceNotAvailable =  18,
        SchoolName = 19,
        AcademicYear = 20
    }

    private enum StudentPaidFeeEnum
    {
        LeftHeader = 1,
        CenterHeader = 2,
        LeftData = 3,
        CenterData = 4,
        NoBorderCenterHeader = 5,
        RightDataWithNoBorder = 14,
        CenterDataBold = 17
    }

    private enum CellAlignment
    {
        LeftHeader = 2,
        CenterHeader = 3,
        ReportHeader = 4,
        LeftData = 5,
        CenterData = 6,
        CenterDataWithNoBorder = 7,
        CenterHeaderYear = 8,
        RightHeaderClass = 9,
        CenterDecimalData = 10,
        LeftDataWithNoBorder = 11,
        CenterDataHeader = 12,
        ReportHeaderOrg = 13,
        RightDataWithNoBorder = 14
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public int Salary { get; set; }
    }
}