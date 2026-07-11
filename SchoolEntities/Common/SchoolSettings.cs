/**
 * Filename		: SchoolSettings.cs
 * Author		: Sunny Chavan
 * Date			: 1-Feb-2013
 * Description	: Contains entities which are used for School Settings.
 */

namespace SchoolEntities
{
	/// <summary>
	///		This is an entity class which represents the school settings of a particular academic year.
	/// </summary>
	public class SchoolSettings : SchoolEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
        public string PossibleValues { get; set; }
        public string Description { get; set; }
	}

	/// <summary>
	///		This is an entity class which will represent the SchoolSettings table. 
	/// </summary>
	public class YearwiseSchoolSettings : SchoolEntity
	{
		public bool ActivityLogging { get; set; }
		public bool AddExtGoogleAnalytics { get; set; }
		public bool AddIntGoogleAnalytics { get; set; }
		public decimal AdmissionFormFees { get; set; }
		public string AdmissionFormSubmerchantID { get; set; }
		public bool AllowPartialSubmit { get; set; }
		public bool AllowPublishUnpublishExam { get; set; }
		public int AssemblyLectNo { get; set; }
		public string AssemblyName { get; set; }
		public string AssemblyWeekday { get; set; }
		public bool BlockProgressReportIfFeesArePending { get; set; }
		public string DateFormat { get; set; }
		public bool EnableAccountsModule { get; set; }
		public bool EnableAdmissionFormFee { get; set; }
		public bool EnableAdvanceFeePayment { get; set; }
		public bool EnabledOnlineFee { get; set; }
		public bool EnableInventoryModule { get; set; }
		public bool EnableLibraryModule { get; set; }
		public bool EnablePayrollModule { get; set; }
		public bool EnableTaskManagementModule { get; set; }
		public bool EnableTransportModule { get; set; }
		public bool EnableHomeworkModule { get; set; }
		public string ExternalLibrarySite { get; set; }
		public string GridDateFormat { get; set; }
		public string GridDateTimeFormat { get; set; }
		public string GridTimeFormat { get; set; }
		public bool HideReceiptNumber { get; set; }
		public bool IsAssemblyApplicable { get; set; }
		public bool IsCautionMoneyApplicable { get; set; }
		public bool IsConcessionApplicable { get; set; }
		public bool IsExamScheForPrePrimaryStud { get; set; }
		public bool IsGraceApplicable { get; set; }
		public bool IsMaxFeeApplicable { get; set; }
		public bool IsMPTApplicable { get; set; }
		public bool IsPrePrimaryProgressSheetWithGrade { get; set; }
		public bool IsReportApplicableToStudent { get; set; }
		public bool IsRTEApplicable { get; set; }
		public bool IsStaybackApplicable { get; set; }
		public bool IsTimeTableForPrePrimaryClassTeacher { get; set; }
		public bool IsExamScheduleForPrePrimaryClassTeacher { get; set; }
		public bool IsTimeTableForPrePrimaryStud { get; set; }
		public bool IsXseedAvailable { get; set; }
		public int LeaveSeperaterDay { get; set; }
		public int LeaveTransferMonth { get; set; }
		public string Location { get; set; }
		public string LoginNotAllowdMsg { get; set; }
		public int MaxFee { get; set; }
		public string MaxFeeNote { get; set; }
		public int MaxTeacherForSubject { get; set; }
		public int MPTLectNo { get; set; }
		public string MPTName { get; set; }
		public string MPTWeekday { get; set; }
		public string ProgressSheetNote { get; set; }
		public int ReceiptMinimumDigits { get; set; }
		public int RemarkLength { get; set; }
		public bool RoundMarksAtSubjectLevel { get; set; }
		public bool ShowAdmissionForCurrentYear { get; set; }
		public bool ShowAds { get; set; }
		public bool ShowAnnualInProgressSheet { get; set; }
		public bool ShowEmailIcon { get; set; }
        public bool ShowThemes { get; set;  }
		public bool ShowNotes { get; set; }
		public bool ShowProgressSheetNote { get; set; }
		public bool ShowTopppers { get; set; }
		public bool ShowTotalAsPerOutOfMarks { get; set; }
		public string SiteName { get; set; }
		public string SoftwareCoordinatorEmailAddresses { get; set; }
		public int StandardCautionMoneyAmt { get; set; }
		public string StaybackName { get; set; }
		public string StudentFeesSubmerchantID { get; set; }
		public string SubDomainLoginUrl { get; set; }
		public string SupervisorRoleName { get; set; }
		public bool UseAvarageFinalResult { get; set; }
		public string UserGuideUrl { get; set; }
		public string VerifyNote1 { get; set; }
		public string VerifyNote2 { get; set; }
		public string UserGuideLocation { get; set; }
        public bool AllowStudentResultSelection { get; set; }
		public bool IsMiniSite { get; set; }
        public int MaxLeaveDays { get; set; }
		public bool AllowMarksEntryForLateJoin { get; set; }
        public bool EnableStaffPerformanceModule { get; set; }
        public bool IsTotalConsiderForProgressReport { get; set; }
        public string SoftwareFeedbackLink { get; set; }
        public string DefaultFeeType { get; set; }
        public bool ShowCautionMoneyClrDate { get; set; }
        public bool ShowFormNumber { get; set; }
        public bool EnableOtherStaffLogin { get; set; }
        public bool EnableLibraryLinkForStudentLogin { get; set; }
        public bool EnableTransportLinkForStudentLogin { get; set; }
        public bool EnableTimetableMenuForStudentLogin { get; set; }
        public bool EnableTransportCommitteeForStudentLogin { get; set; }
        public string FeeVideoLinkURL { get; set; }
        public bool AllowPhotoGallaryDownloadForExternalSite { get; set; }
        public bool EnableHomeworkMySubjectListView { get; set; }
        public bool EnableAssignExamMarksToAllSubjectOfClass { get; set; }
        public string GPSTrackingUrl { get; set; }
        public bool EnableAskMeFunctionality { get; set; }
        public bool ShowStaffAttendanceMenu { get; set; }
        public bool SendHomeworkSMSToParents { get; set; }
        public bool EnableHomeworkModuleForStudentLogin { get; set; }
        public bool EnableSurveyModule { get; set; }
        public string HomeworkSmsScheduleTime { get; set; }
        public bool EnableAdvanceFeePaymentForStudent { get; set; }
        public int LessonPlanConfigTypeId { get; set; }
        public bool EnableLessonPlanModule { get; set; }
        public bool ResetInternalFeeReceiptNo { get; set; }
        public bool EnableObservationSystem { get; set; }
        public string SupportURL { get; set; }
        public bool EnableSurveyModuleOfJPS { get; set; }
        public bool EnableSurveyModuleForAdmin { get; set; }
        public int FullWorkingHours { get; set; }
        public int HalfWorkingHours { get; set; }
        public string CompareAgeTillDate { get; set; }
        public bool ShowFeeStructureOfNextYear { get; set; }
        public bool DisplayShortNameOnTimeTableScreen { get; set; }
        public bool DisplayBalanceAmountInPaymentAcknowledgementSMS { get; set; }
        public bool EnablePTAModule { get; set; }
        public bool EnablePTAModuleforStudents { get; set; }
        public bool EnableAssemblyModule { get; set; }
        public string DefaultInternalFeeType { get; set; }
        public bool EnabledServiceActivityLogging { get; set; }
		public string ExternalRITeStoreURL { get; set; }
        public bool EnableRITeStoreModule { get; set; }
        public string AllowStudentUpdateForClassTeacher { get; set; }
        public bool SendSMSToAbsentStudent { get; set; }
        public int MaxDaysOfMissingAttendance { get; set; }
        public bool DisplayLateFeeNote { get; set; }
        public bool IsAdditionalFieldsApplicable { get; set; }
        public string SchoolNameForFeeXML { get; set; }
        public bool DisplayAccountHeaders { get; set; }
        public bool IsEnableHalfDayView { get; set; }
        public bool IsEnableMessageCenterToParent { get; set; }
        public bool IsBiometriceEnabled { get; set; }
        public string BiometricUtilityPath { get; set; }
        public bool IsEnableSubjecTeacherScreen { get; set; }
        public int ToppersCount { get; set; }
        public bool IsEnableEnquiry { get; set; }
        public bool AutoCalculateEnrolmentNo { get; set; }
        public string AllowEmptyRegNo { get; set; }
        public bool ShowBothTermProgressRemark { get; set; }
        public bool IsEnableExternalActivities { get; set; }
        public bool EnableDescriptiveIndicatorAssignment { get; set; }
        public bool AllowExamStatusForCoCurricullarSubjects { get; set; }
        public bool AllowNextYearInternalFeePayment { get; set; }
        public bool EnableFeedbackForm { get; set; }
        public bool ShowAadharCardForStudent { get; set; }
        public bool ReceiveAllMsgToDefaultUser { get; set; }
        public int StudentAbsentCount { get; set; }
        public string SMSProvider { get; set; }
        public bool EnableTransportStaffForActiveDeactive { get; set; }
        public bool EnableGuestManagement { get; set; }
        public bool GenerateCountBasedLottery { get; set; }
        public bool EnableStudentFeesModule { get; set; }
        public bool EnableProgressReport { get; set; }
        public bool DisplayWeeklyTimtableLink { get; set; }
        public bool EnableStudentRecordModule { get; set; }
        public string DefaultStudentState { get; set; }
        public bool ShowGradePayOnStaffProfileScreen { get; set; }
        public bool IsWeeklyTestApplicable { get; set; }
        public string WeeklyTestName { get; set; }
        public int WeeklyTestLectNo { get; set; }
        public bool MarkMonthwiseAttendance { get; set; }
        public string WeeklyTestWeekDay { get; set; }
        public bool ShowAllGalleries { get; set; }
        public int SetDefaultSMSCountForSNS { get; set; }
        public bool FilterWithSalutation { get; set; }
		public int SetDefaultSMSCount { get; set; }
        public bool AutoPopulateInternalFeeRemark { get; set; }
        public bool EnableOnlinePaymentForCautionMoney { get; set; }
        public bool EnableStudentHealthDetailsModule { get; set; }
        public int DisplayTopPhotoAlbumCount { get; set;}
        public bool ShowPhotoGalleryPerClasswise { get; set; }
        public bool SetParticularFeeRestriction { get; set; }
        public string SMSProviderForWebsite { get; set; }
        public bool EnableMessageCenterReadModeForStudent { get; set; }
        public bool EnableOnlinePaymentForInternalFee { get; set; }
        public int DueDateDayCount { get; set; }
        public bool ShowRefundOptionForAll { get; set; }
        public bool ShowOnlyCoOrdinators { get; set; }
        public bool ShowAllClassesForStdClassAssignment { get; set; }
        public bool ShowConfirmedByName { get; set; }
        public bool ShowDayBoardingOptionOnStudentsScreen { get; set; }
        public bool AllowNextYearInternalFeePaymentForStudent { get; set; }
        public bool AllowExternalTransport { get; set; }
        public bool EnableOnlinePaymentForLastYearFee { get; set; }
        public bool IsAaryanSchool { get; set; }
        public bool EnablePartialFeePaymentForStudentLogin { get; set; }
        public int MinimumPartialAmountForOnline { get; set; }
		public bool AllowDuplicateStudentsForAdmission { get; set; }
        public bool RestrictFeePaymentForSequence { get; set; }
        public bool RestrictCopyDataFromMessageCenter { get; set; }
        public bool RestrictNewPaymentIfOldPaymentIsPending { get; set; }
        public bool EnableOnlineExamModule { get; set; }
        public string DescriptiveIndicatorMarkType { get; set; }
        public bool AllowDOBinTextEdit { get; set; }
        public int VehiclePassingPeriod { get; set; }
        public int VehicleServicingPeriod { get; set; }
        public int VehiclePUCPeriod { get; set; }
		public string SACCode { get; set; }
        public string InvoiceNoPrefix { get; set; }
        public bool AllowCautionMoneyAdjustmentInRegularFee { get; set; }
        //public string PONoPrefix { get; set; }
		public bool AllowUnsubmitExamMarks { get; set; }
        public bool AllowHomewirkDailyLog { get; set; }
        public bool AllowStudentPhotoUploadFromStudentLogin { get; set; }
        public string EmployeeNoPrefix { get; set; }
        public bool AllowParentPhotoUploadFromStudentLogin { get; set; }
		public bool ShowDailyHomeworkLogs { get; set; }
        public bool ShowITRReportOnStudentLogin { get; set; }
        //public string WONoPrefix { get; set; }
        public bool ShowRequisitionExpiryDate { get; set; }
        public int RequisitionExpiryDaysCount { get; set; }
        public bool EnableStoreModule { get; set; }
        public bool EnableParentHealthDetailsAtStudentLogin { get; set; }
        public bool BlockExamPublish { get; set; }
		public string BetaVersionURL { get; set; }
        public bool ShowInternalFeeAtStudentLogin { get; set; }
        public int PasswordChangeMandatoryDays { get; set; }
        public bool EnableDeleteButtonforStudentRegistration { get; set; }
        public bool ForceStudentToSubmitMandatoryFields { get; set; }
   }

    /// <summary>
    /// To get DB log file size details.
    /// </summary>
    public class DBLogFileSizeDetails
    {
        public string DBName { get; set; }
        public string DbSizeGb { get; set; }
        public string DbMdfSizeGb { get; set; }
    }
}
