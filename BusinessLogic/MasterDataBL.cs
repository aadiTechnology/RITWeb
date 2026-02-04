/*
 *  File Name : -- MasterDataBL.cs
 *  Purpose   : -- This Class is used as an interface between the UILayer and DCLayer. This class calls method 
 *                 of MasterDataDC.cs
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DataCommunicator;
using Utility;
using MasterEntities;
using XseedReportEntities;
using SchoolEntities.Admin;
using PayrollReportingUserEntities;
using BookEntities;
using System.Linq;


namespace BusinessLogic
{

    public class MasterDataCollectionBL
    {
        MasterDataCollectionDC moMasterDataCollectionDC;

        public MasterDataCollectionBL()
        {
            moMasterDataCollectionDC = new MasterDataCollectionDC();
        }

        public string GetStandardName(int aiSchoolId, int aiStandardId)
        {
            return moMasterDataCollectionDC.GetStandardName(aiSchoolId, aiStandardId);
        }

        public string GetDivisionName(int aiSchoolId, int aiDivisionId)
        {
            return moMasterDataCollectionDC.GetDivisionName(aiSchoolId, aiDivisionId);
        }

        public string GetClassName(int aiSchoolId, int aiStandardId, int aiDivisionId)
        {
            return moMasterDataCollectionDC.GetClassName(aiSchoolId, aiStandardId, aiDivisionId);
        }
        
        public static DataTable GetAllSchools()
        {
            return MasterDataCollectionDC.GetAllSchools();
        }

        public void FillAcedemicStartYear(int aiSchoolId, ref DropDownList oDropDownList)
        {
            DataTable oDT = moMasterDataCollectionDC.GetListOfAcedimicYear(aiSchoolId);
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                        "SchoolWise_Academic_Year_Id",
                                        "AcademicYear",
                                      Constants.S_SELECT);

        }


        public void FillUserRoleComboBox(ref DropDownList oDropDownList)
        {
            DataTable oDT = moMasterDataCollectionDC.GetAllUserRoles();
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                                Constants.S_USERROLE_ID_FIELD,
                                                Constants.S_USERROLE_NAME_FIELD,
                                                Constants.S_SELECT);
        }

        public void FillSalutationComboBox(ref DropDownList oDropDownList)
        {
            DataTable oDT = moMasterDataCollectionDC.GetSalutationType();
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                                 Constants.S_SALUTATION_ID_FIELD,
                                                 Constants.S_SALUTATION_NAME_FIELD,
                                                 Constants.S_EMPTY_STRING);
        }

		public void FillTeacherNameComboBox(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, ref DropDownList oDropDownList, string sDisplayMember)
		{
			DataTable oDT = moMasterDataCollectionDC.GetAllTeachers(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId);
			ControlUtility.FillDropDownList(oDT, ref oDropDownList,
												 Constants.S_TEACHER_ID_FIELD,
												 Constants.S_TEACHER_NAME_FIELD,
												 sDisplayMember);
		}

		public List<ClassTeacher> FillTeacherNameComboBox(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
		{
			return moMasterDataCollectionDC.GetAllTeachersForClassTeacherAssignment(aiSchoolId,aiAcademicYearId,aiStandardId);
		}
        public void FillClassTeachersComboBox(int aiSchoolId, int aiAcademicYearId, ref DropDownList oDropDownList, string sDisplayMember)
        {
            DataTable oDt = moMasterDataCollectionDC.GetAllClassTeachers(aiSchoolId, aiAcademicYearId);
            ControlUtility.FillDropDownList(oDt, ref oDropDownList,
												 Constants.S_TEACHER_ID_FIELD,
                                                 Constants.S_TEACHER_NAME_FIELD,
                                                 sDisplayMember);
        }

		public DataTable GetAllClassTeachers(int aiSchoolId, int aiAcademicYearId)
		{
			return moMasterDataCollectionDC.GetAllClassTeachers(aiSchoolId, aiAcademicYearId);
		}

        public void FillNonPrePrimaryClassTeachers(int aiSchoolId, int aiAcademicYearId, ref DropDownList oDropDownList, string sDisplayMember)
        {
            DataTable oDt = moMasterDataCollectionDC.GetNonPrePrimaryClassTeachers(aiSchoolId, aiAcademicYearId);
            ControlUtility.FillDropDownList(oDt, ref oDropDownList,
												 Constants.S_TEACHER_ID_FIELD,
                                                 Constants.S_TEACHER_NAME_FIELD,
                                                 sDisplayMember);
        }

		/// <summary>
		/// This method is used to get all class teahcer of those classes for which normal exam configuration is done.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <returns></returns>
		public List<ClassTeacherDetails> GetClassTeachersForExamResult(int aiSchoolId, int aiAcademicYearId)
		{
			return moMasterDataCollectionDC.GetClassTeachers(aiSchoolId, aiAcademicYearId);
		}

	    public void FillPrePrimaryClassTeachersComboBox(int aiSchoolId, int aiAcademicYearId, ref DropDownList oDropDownList, string sDisplayMember)
        {
            DataTable oDt = moMasterDataCollectionDC.GetPrePrimaryClassTeachers(aiSchoolId, aiAcademicYearId);
            ControlUtility.FillDropDownList(oDt, ref oDropDownList,
                                                 Constants.S_TEACHER_ID_FIELD,
                                                 Constants.S_TEACHER_NAME_FIELD,
                                                 sDisplayMember);
        }

        public DataTable GetClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            return moMasterDataCollectionDC.GetAllClassTeachers(aiSchoolId, aiAcademicYearId);
        }

        public void FillTeacherNameComboBoxForAssignSubject(int aiSchoolId, int aiSubjectId, int aiStandardDivisionId, ref DropDownList oDropDownList, string sDisplayMember)
        {
            DataTable oDT = moMasterDataCollectionDC.GetAllTeachersToAssignSubjects(aiSchoolId, aiSubjectId, aiStandardDivisionId);
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                                 Constants.S_TEACHER_ID_FIELD,
                                                 Constants.S_TEACHER_NAME_FIELD,
                                                 sDisplayMember);


        }

        public DataTable GetAllTeachersToAssignSubjects(int aiSchoolId, int aiSubjectId, int aiStandardDivisionId)
        {
            return moMasterDataCollectionDC.GetAllTeachersToAssignSubjects(aiSchoolId, aiSubjectId, aiStandardDivisionId);
        }

        /// <summary>
        /// This function fills teachers combo box with all teachers in std-div-teachers table
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="oDropDownList"></param>
        /// <param name="sDisplayMember"></param>
        public void FillTeachersCombobox(int aiSchoolId, int aiAcademicYearId, ref DropDownList oDropDownList, string sDisplayMember)
        {
            DataTable oDT = moMasterDataCollectionDC.RetriveAllTeachers(aiSchoolId, aiAcademicYearId);
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                                 Constants.S_TEACHER_ID_FIELD,
                                                 Constants.S_TEACHER_NAME_FIELD,
                                                 sDisplayMember);


        }
        public void FillStandardDivisionComboBox(int aiSchoolId, int aiTeacherId, int aiAcademicYearId, ref DropDownList oDropDownList)
        {
            DataTable oDT = moMasterDataCollectionDC.GetStandardDivisionName(aiSchoolId, aiTeacherId, aiAcademicYearId);
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                            Constants.S_STANDARD_DIVISION_ID_FIELD,
                                            Constants.S_STANDARD_DIVISION_NAME_FIELD,
                                            Constants.S_SELECT);

        }

        public void FillStandardDivisionComboBoxOfStudents(int aiSchoolId, int aiTeacherId, int aiAcademicYearId, ref DropDownList oDropDownList, int aiLoginUserId)
        {
            DataTable oDT = moMasterDataCollectionDC.GetStandardDivisionNameOfStudents(aiSchoolId, aiTeacherId, aiAcademicYearId, aiLoginUserId);
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                            Constants.S_STANDARD_DIVISION_ID_FIELD,
                                            Constants.S_STANDARD_DIVISION_NAME_FIELD,
                                            Constants.S_SELECT);

        }
        public void FillSubjectsComboBox(int aiSchoolId, int aiTeacherId, int aiStandardDivisionId, int aiAcademicYearId, ref DropDownList oDropDownList)
        {
            DataTable oDT = moMasterDataCollectionDC.GetSubjectNameForTeacherStandardDivision(aiSchoolId, aiTeacherId, aiStandardDivisionId, aiAcademicYearId);
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                            Constants.S_SUBJECT_ID_FIELD,
                                            Constants.S_SUBJECT_NAME_FIELD,
                                            Constants.S_SELECT);
        }

        public void FillDesignationCombobox(ref DropDownList oDropDownList)
        {
            DataTable oDT = moMasterDataCollectionDC.GetDesignations();
            ControlUtility.FillDropDownList(oDT, ref oDropDownList,
                                            Constants.S_DESIGNATION_ID_FIELD,
                                            Constants.S_DESIGNATION_NAME_FIELD,
                                            Constants.S_SELECT);
        }

        public void FillDesignationCombobox(ref DropDownList oDropDownList, Constants.UserRoles aoUserRole)
        {
            DesignationMasterBL oDesignationMasterBL = new DesignationMasterBL();
            List<DesignationMaster> lstDesignations = oDesignationMasterBL.GetAll();
            if (aoUserRole != Constants.UserRoles.None)
                lstDesignations = lstDesignations.Where(dg => dg.UserRoleId == aoUserRole.ToInt()).ToList();
            lstDesignations = lstDesignations.Where(dg => dg.UserRoleId != Convert.ToInt32(Constants.UserRoles.ExAdmin)).ToList();
            ListSource.FillDropDownList(lstDesignations, oDropDownList, "Designation", "DesignationId", Constants.S_SELECT);
        }

        public DataTable GetAllDivisionForTeacher(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiTeacherId)
        {
            return moMasterDataCollectionDC.GetAllDivisionForStandard(aiSchoolId, aiAcademicYearId, aiStandardId, aiTeacherId);
        }

        public DataTable GetAllSubCastes(Int32 aiCasteId)
        {
            return moMasterDataCollectionDC.GetAllSubCastes(aiCasteId);
        }

        public DataSet GetAllConfiguration(int aiSchoolId, int aiAcademic_year_id)
        {
            return moMasterDataCollectionDC.GetAllConfiguration(aiSchoolId, aiAcademic_year_id);
        }

        public DataSet GetAllConfigurationsForAcademicData(int aiSchoolId, int aiAcademic_year_id, Boolean bIsOnlyInMidAcademic)
        {
            return moMasterDataCollectionDC.GetAllConfigurationsForAcademicData(aiSchoolId, aiAcademic_year_id, bIsOnlyInMidAcademic);
        }

        public DataSet GetConfigurationsForFinalAcademicYearGeneration()
        {
            return moMasterDataCollectionDC.GetConfigurationsForFinalAcademicYearGeneration();
        }

        public DataTable GetAllUserRoles()
        {
            return moMasterDataCollectionDC.GetAllUserRoles();
        }

        /// <summary>
        /// This method is used to get user role without Parent and transport staff.
        /// </summary>
        /// <returns></returns>
        public DataRow[] GetRolesWithoutParent()
        {
           return moMasterDataCollectionDC.GetAllUserRoles().Select("User_Role_Id <> 9");
            
        }
        /// <summary>
        /// This method is used to get school teacher Id
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asUserIDs"></param>
        /// <returns></returns>
        public DataTable GetAllMobileNosForGivenTeacherUserID(int aiSchoolId, String asUserIDs, int aiAcademicYear)
        {
            return moMasterDataCollectionDC.GetAllMobileNosForGivenTeacherUserID(aiSchoolId, asUserIDs, aiAcademicYear);
        }

        /// <summary>
        /// This method is used to get school's all teachers mobile no.
        /// </summary>
        /// <param name="aiSchoolId"></param>        
        /// <returns></returns>
        public DataTable GetAllMobileNosForTeachers(int aiSchoolId, int aiAcademicYear)
        {
            return moMasterDataCollectionDC.GetAllMobileNosForTeachers(aiSchoolId, aiAcademicYear);
        }

        public DataTable GetAllUserRolesExceptAdmin()
        {
            return moMasterDataCollectionDC.GetAllUserRolesExceptAdmin();
        }



        public DataTable GetStandardIdsOfOriginalStandradId(int aiSchoolId, int aiStandardId)
        {
            return moMasterDataCollectionDC.GetStandardIdsOfOriginalStandradId(aiSchoolId, aiStandardId);
        }


        public DataTable GetStandardDivisionSubjectName(int aiStandardDivisionId, int aiSubjectId)
        {
            return moMasterDataCollectionDC.GetStandardDivisionSubjectName(aiStandardDivisionId, aiSubjectId);
        }

        public DataTable GetGradeNameForFailCriteria(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            return moMasterDataCollectionDC.GetGradeNameForFailCriteria(aiSchoolId, aiAcademicYearId, aiStandardId);
        }

        public void FillGradeNameComboxForSelectedStandard(int aiSchoolId, int aiAcademicYearId, int aiStandardId, ref DropDownList oDropDownList)
        {
            DataTable oDt = moMasterDataCollectionDC.GetGradeNameForFailCriteria(aiSchoolId, aiAcademicYearId, aiStandardId);
            ControlUtility.FillDropDownList(oDt, ref oDropDownList,
                                            "Marks_Grades_Configuration_Detail_ID",
                                            "Grade_Name",
                                            Constants.S_SELECT);
        }
            
        public void FillGradeListComboxForSelectedClass(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, ref DropDownList oDropDownList)
        {
            DataTable oDt = moMasterDataCollectionDC.GetGradeListForFailCriteria(aiSchoolId, aiAcademicYearId, aiStandardDivisionId);
            ControlUtility.FillDropDownList(oDt, ref oDropDownList,
                                            "Marks_Grades_Configuration_Detail_ID",
                                            "Grade_Name",
                                            Constants.S_SELECT);
        }


        public void FillStandardDivisionComboForMessageDetails(int aiSchoolId, int aiAcademicYearID, ref DropDownList oDropDownList)
        {
            //This function is used to get the standard divition Name for Message.

            DataTable oDS = moMasterDataCollectionDC.GetStandardDivisionNameForMessageDetails(aiSchoolId, aiAcademicYearID, Constants.I_ZERO, 0);
            ControlUtility.FillDropDownList(oDS, ref oDropDownList,
                                                  "Id",
                                                  "Name",
                                                  Constants.S_SELECT);
        }

        public DataTable GetStandardDivisionDetailsForMessageDetails(int aiSchoolId, int aiAcademicYearID, int aiTypeId, ref DropDownList oDropDownList, int aiLoginUserId)
        {
            //This function is used to get the standard divition Name for Message.

            return moMasterDataCollectionDC.GetStandardDivisionNameForMessageDetails(aiSchoolId, aiAcademicYearID, aiTypeId, aiLoginUserId);

        }

        /// <summary>
        /// This method returns category id for that category name.
        /// </summary>
        /// <param name="asCategoryName"></param>
        /// <returns></returns>
        public int GetCategoryIdForCategory(string asCategoryName)
        {
            return moMasterDataCollectionDC.GetCategoryIdForCategory(asCategoryName);
        }        

        public DataSet GetAllFeeCategoriesForImport(int aiSchoolId, int aiAcademicYearId)
        {
            return moMasterDataCollectionDC.GetAllFeeCategoriesForImport(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to return fee area id for fee area name.
        /// </summary>
        /// <param name="asFeeAreaName"></param>
        /// <returns></returns>
        public List<string> GetFeeAreas()
        {
            return moMasterDataCollectionDC.GetFeeAreas();
        }

        /// <summary>
        /// This method is used to return User Type.
        /// </summary>
        public DataTable GetAllUserTypes()
        {
            return moMasterDataCollectionDC.GetAllUserTypes();
        }

        /// <summary>
        /// This method returns RTE category id for that category name.
        /// </summary>
        /// <param name="asCategoryName"></param>
        /// <returns></returns>
        public int GetRTECategoryIdForCategory(string asCategoryName)
        {
            return moMasterDataCollectionDC.GetRTECategoryIdForCategory(asCategoryName);
        }

        public int GetSalutationIdForSalutationName(string asSalutation)
        {
            return moMasterDataCollectionDC.GetSalutationIdForSalutationName(asSalutation);
        }

        public int GetDesignationIdForDesignationName(string asDesignation)
        {
            return moMasterDataCollectionDC.GetDesignationIdForDesignationName(asDesignation);
        }
        public int GetReligionIdForReligionName(string asReligion)
        {
            return moMasterDataCollectionDC.GetReligionIdForReligionName(asReligion);
        }
        public int GetQualiIdForQualiName(string asQuali)
        {
            return moMasterDataCollectionDC.GetQualiIdForQualiName(asQuali);
        }
        public int GetClassIdForClassName(string asClass)
        {
            return moMasterDataCollectionDC.GetClassIdForClassName(asClass);
        }
        public int GetLivingLocationIdForLivingLocationName(string asLivingLocation)
        {
            return moMasterDataCollectionDC.GetLivingLocationIdForLivingLocationName(asLivingLocation);
        }

        /// <summary>
        /// This method is used to get supportive data for import student admission details.
        /// </summary>
        /// <returns></returns>
        public DataSet GetResidanceTypeMasterDataForadmission()
        {
            return moMasterDataCollectionDC.GetResidanceTypeMasterDataForadmission();
        }
        /// <summary>
        /// This method returns parent occupation id. 
        /// </summary>
        /// <param name="asParentOccupation"></param>
        /// <returns></returns>

        public int GetParentOccupationIdForParentOccupationName(string asParentOccupation)
        {
            return moMasterDataCollectionDC.GetParentOccupationIdForParentOccupationName(asParentOccupation);
        }
        public int GetRuleIdForRule(string asRule,int iSchoolId,int aiAcademicYrId)
        {
            return moMasterDataCollectionDC.GetRuleIdForRule(asRule, iSchoolId, aiAcademicYrId);
        }
        public static DataSet GetAllMasterData()
        {
            return MasterDataCollectionDC.GetAllMasterData();
        }
        public static DataSet GetAllMasterDataForStudent(int iSchoolId, int iAcademicYear,int iStandardId, int aiDivisionId)
        {
            return MasterDataCollectionDC.GetAllMasterDataForStudent(iSchoolId, iAcademicYear, iStandardId, aiDivisionId);
        }
        /// <summary>
        /// This Method is used to get stream.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="acAdmissionForCurrentYear"></param>
        /// <returns></returns>
        public  DataTable GetAllStreams()  /////
        {
            return moMasterDataCollectionDC.GetAllStreams();
        }
        /// <summary>
        /// This Method is used to get streamwise groups.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="acAdmissionForCurrentYear"></param>
        /// <returns></returns>
        public DataTable GetAllGroupsOfStream(int aiStream)
        {
            return moMasterDataCollectionDC.GetAllGroupsOfStream(aiStream);
        }
        
        /// <summary>
        /// This Method is used to get group wise compulsary subjects.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="acAdmissionForCurrentYear"></param>
        /// <returns></returns>
        public DataSet GetAllCompulsarySubjects( int aigroup, int aiAcademicYearId)
        {
            return moMasterDataCollectionDC.GetAllCompulsarySubjects( aigroup, aiAcademicYearId);
        }

        public static DataSet GetAllMasterDataForStudentAdmission(int aiSchoolID, int aiStudentAdmissionId, string acAdmissionForCurrentYear, int aiAcademicYearId)
        {
            return MasterDataCollectionDC.GetAllMasterDataForStudentAdmission(aiSchoolID, aiStudentAdmissionId,acAdmissionForCurrentYear, aiAcademicYearId);
        }
        public static DataSet GetAllLectureLimings(int aiSchoolID, int aiAcademicYrId, int aiSection)
        {
            return MasterDataCollectionDC.GetAllLectureLimings(aiSchoolID, aiAcademicYrId, aiSection);
        }
        /// <summary>
        /// This Method is used to get master details for student registration.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="acAdmissionForCurrentYear"></param>
        /// <returns></returns>
        public static DataSet GetAllMasterDataForStudentRegistration(int aiSchoolId, int aiAcademicYearId, string acAdmissionForCurrentYear)
        {
            return MasterDataCollectionDC.GetAllMasterDataForStudentRegistration(aiSchoolId, aiAcademicYearId, acAdmissionForCurrentYear);
        }

        /// <summary>
        /// This method is used to get configurarion datails.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiParentId"></param>
        /// <param name="aiScreenLevel"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public DataTable GetConfigurationDetails(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiParentId, int aiScreenLevel,int aiUserId,int aiUserRoleId)
        {
            return MasterDataCollectionDC.GetConfigurationDetails(aiSchoolId, aiAcademicYearId, aiFinancialYearId, aiParentId, aiScreenLevel, aiUserId, aiUserRoleId);
        }
  
        /// <summary>
        /// This method is used to get menu item details to fill menu control.
        /// </summary>
        /// <param name="aiScreenLevel"></param>
        /// <returns></returns>
        public DataTable GetMenuItemDetails( int aiScreenLevel)
        {
            return MasterDataCollectionDC.GetMenuItemDetails( aiScreenLevel);
        }
		public static List<ClassTeacherDetails> GetClassTeacher(int aiSchoolId, int aiAcademicYearId)
        {
            return MasterDataCollectionDC.GetClassTeacher(aiSchoolId, aiAcademicYearId);
        }
        public static List<DesignationMaster> GetDesignationsDetails(int aiUserId, int aiUserRoleId, int aiFilter, int aiSchoolId, int aiAcademicYearId)
        {
            return MasterDataCollectionDC.GetDesignationsDetails(aiUserId,aiUserRoleId,aiFilter,aiSchoolId,aiAcademicYearId);
        }

        /// <summary>
        /// This method is used for getting the users for selected role.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiRoleId"></param>
        /// <returns></returns>
        public static List<AttendanceAlertConfigDetails> GetUsers(int aiRoleId, int aiSchoolId, int aiAcademicYearId)
        {
            return MasterDataCollectionDC.GetUsers(aiRoleId, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get User details.
        /// </summary>
        /// <param name="aiRoleId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static List<ReportingUserConfiguration> GetReportingUsers(int aiRoleId, int aiSchoolId, int aiAcademicYearId)
        {
            return MasterDataCollectionDC.GetReportingUsers(aiRoleId, aiSchoolId, aiAcademicYearId);
        }
        /// <summary>
        /// This is used to get all the roles.
        /// </summary>
        /// <returns></returns>
        public static List<UserRoles> GetUserRoles()
        {
            return MasterDataCollectionDC.GetUserRoles();
        }

		/// <summary>
		/// This method is used to get list of all qualifications.
		/// </summary>
		/// <returns></returns>
		public static List<Qualification> GetAllQualification()
		{
			return MasterDataCollectionDC.GetAllQualification();
		}

		/// <summary>
		/// This method returns the datatable containing collection of all types of classes.
		/// </summary>
		/// <returns></returns>
		public static DataTable GetListOfClassType()
		{
			return MasterDataCollectionDC.GetListOfClassType();
		}

        /// <summary>
        /// This method return the user roles that will be applicable to mailing group functionality.
        /// </summary>
        /// <returns></returns>
        public static List<UserRoles> GetUserRolesForMailingList()
        {
            return MasterDataCollectionDC.GetUserRolesForMailingList();
        }

        /// <summary>
        /// This method return all the user roles.
        /// </summary>
        /// <returns></returns>
        public static List<UserRoles> GetAllRoles()
        {
            return MasterDataCollectionDC.GetAllRoles();
        }

        /// <summary>
        /// This method return whether passing user id is class teacher or not.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static bool IsClassTeacher(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            return MasterDataCollectionDC.IsClassTeacher(aiSchoolId,aiAcademicYearId,aiUserId);
        }
    }
}
