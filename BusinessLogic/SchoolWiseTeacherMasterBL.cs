using System;
using DataCommunicator;
using Utility;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;

namespace BusinessLogic
{
    public class SchoolWiseTeacherMasterBL
    {
        #region " Constants "

        private const string S_DUPLICATE_SCHOOL_NAME = "School Name already exists.";
        #endregion " Constants "

        #region " Data Members & Properties"

        #region " Data Members "

        // Object of the SchoolWiseTeacherMasterDC Class. 
        //Using this object call the methods of the SchoolWiseTeacherMasterDC Class.

        SchoolWiseTeacherMasterDC moSchoolWiseTeacherMasterDC;
        SchoolWiseTeacherMasterDC.TeacherInfoStruct moTeacherInfoStruct;
        public Collection<TeacherEducationDetailsBL> moTeacherEduDetails = new Collection<TeacherEducationDetailsBL>();
        public Collection<TeacherExperienceDetailsBL> moTeacherExperienceDetails = new Collection<TeacherExperienceDetailsBL>();
        public Collection<TeacherSubjectDetailsBL> moTeacherSubDetails = new Collection<TeacherSubjectDetailsBL>();
        public Collection<TeacherStandardDetailsBL> moTeacherStandardDetails = new Collection<TeacherStandardDetailsBL>();
        public Collection<UserShiftAssociationBL> moUserShiftAssociation = new Collection<UserShiftAssociationBL>();
        public Collection<UserWeekEndAssociationBL> moUserWeekendAssociation = new Collection<UserWeekEndAssociationBL>();

        
        ArrayList oArrMsgForStd = new ArrayList();
        ArrayList oArrMsgForSub = new ArrayList();

        #endregion

        #region " Properties "

        public int TeacherId
        {
            get
            {
                return moTeacherInfoStruct.miTeacherId;
            }
            set
            {
                moTeacherInfoStruct.miTeacherId = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moTeacherInfoStruct.miSchoolId;
            }
            set
            {
                moTeacherInfoStruct.miSchoolId = value;
            }
        }

        public int UserId
        {
            get
            {
                return moTeacherInfoStruct.miUserId;
            }
            set
            {
                moTeacherInfoStruct.miUserId = value;
            }
        }

        public string TeacherFirstName
        {

            get
            {
                return moTeacherInfoStruct.msTeacherFirstName;
            }
            set
            {
                moTeacherInfoStruct.msTeacherFirstName = value;
            }
        }

        public string TeacherMiddleName
        {

            get
            {
                return moTeacherInfoStruct.msTeacherMiddleName;
            }
            set
            {
                moTeacherInfoStruct.msTeacherMiddleName = value;
            }
        }

        public string TeacherLastName
        {

            get
            {
                return moTeacherInfoStruct.msTeacherLastName;
            }
            set
            {
                moTeacherInfoStruct.msTeacherLastName = value;
            }
        }

        public string Designation
        {

            get
            {
                return moTeacherInfoStruct.msDesignation;
            }
            set
            {
                moTeacherInfoStruct.msDesignation = value;
            }
        }

        public string CategoryName
        {

            get
            {
                return moTeacherInfoStruct.msCategoryName;
            }
            set
            {
                moTeacherInfoStruct.msCategoryName = value;
            }
        }

        public string ReligionName
        {
            get
            {
                return moTeacherInfoStruct.msReligionName;
            }
            set
            {
                moTeacherInfoStruct.msReligionName = value;
            }
        }

        public string LocalAddress
        {

            get
            {
                return moTeacherInfoStruct.msLocalAddress;
            }
            set
            {
                moTeacherInfoStruct.msLocalAddress = value;
            }
        }

        public string LocalCity
        {

            get
            {
                return moTeacherInfoStruct.msLocalCity;
            }
            set
            {
                moTeacherInfoStruct.msLocalCity = value;
            }
        }

        public int LocalPincode
        {
            get
            {
                return moTeacherInfoStruct.miLocalPincode;
            }
            set
            {
                moTeacherInfoStruct.miLocalPincode = value;
            }
        }

        public string LocalStateName
        {
            get
            {
                return moTeacherInfoStruct.msLocalStateName;
            }
            set
            {
                moTeacherInfoStruct.msLocalStateName = value;
            }
        }

        public string PermanentAddress
        {

            get
            {
                return moTeacherInfoStruct.msPermanentAddress;
            }
            set
            {
                moTeacherInfoStruct.msPermanentAddress = value;
            }
        }

        public string PermanentCity
        {

            get
            {
                return moTeacherInfoStruct.msPermanentCity;
            }
            set
            {
                moTeacherInfoStruct.msPermanentCity = value;
            }
        }

        public int PermanentPincode
        {
            get
            {
                return moTeacherInfoStruct.miPermanentPincode;
            }
            set
            {
                moTeacherInfoStruct.miPermanentPincode = value;
            }
        }



        public string PhoneNumber
        {
            get
            {
                return moTeacherInfoStruct.msPhoneNumber;
            }
            set
            {
                moTeacherInfoStruct.msPhoneNumber = value;
            }
        }

        public string MobileNumber
        {
            get
            {
                return moTeacherInfoStruct.msMobileNumber;
            }
            set
            {
                moTeacherInfoStruct.msMobileNumber = value;
            }
        }

        public DateTime DateofBirth
        {
            get
            {
                return moTeacherInfoStruct.mdtDateofBirth;
            }
            set
            {
                moTeacherInfoStruct.mdtDateofBirth = value;
            }
        }

        public string Nationality
        {
            get
            {
                return moTeacherInfoStruct.msNationality;
            }
            set
            {
                moTeacherInfoStruct.msNationality = value;
            }
        }

        public int SalutationId
        {
            get
            {
                return moTeacherInfoStruct.miSalutationId;
            }
            set
            {
                moTeacherInfoStruct.miSalutationId = value;
            }
        }

        public char IsLocalAddress
        {
            get
            {
                return moTeacherInfoStruct.mcIsLocalAddress;
            }
            set
            {
                moTeacherInfoStruct.mcIsLocalAddress = value;
            }
        }

        public int DesignationId
        {
            get
            {
                return moTeacherInfoStruct.miDesignationId;
            }
            set
            {
                moTeacherInfoStruct.miDesignationId = value;
            }
        }


        public int CategoryId
        {
            get
            {
                return moTeacherInfoStruct.miCategoryId;
            }
            set
            {
                moTeacherInfoStruct.miCategoryId = value;
            }
        }


        public int ReligionId
        {
            get
            {
                return moTeacherInfoStruct.miReligionId;
            }
            set
            {
                moTeacherInfoStruct.miReligionId = value;
            }
        }


        public string CasteSubCaste
        {
            get
            {
                return moTeacherInfoStruct.msCasteSubCaste;
            }
            set
            {
                moTeacherInfoStruct.msCasteSubCaste = value;
            }
        }

        public DateTime DateofRetirement
        {
            get
            {
                return moTeacherInfoStruct.mdtDateOfRetirement;
            }
            set
            {
                moTeacherInfoStruct.mdtDateOfRetirement = value;
            }
        }



        public char IsTemporary
        {
            get
            {
                return moTeacherInfoStruct.mcIsTemporary;
            }
            set
            {
                moTeacherInfoStruct.mcIsTemporary = value;
            }
        }

        public int ExpInYears
        {
            get
            {
                return moTeacherInfoStruct.miExpInYears;
            }
            set
            {
                moTeacherInfoStruct.miExpInYears = value;
            }
        }

        public int ExpInMonths
        {
            get
            {
                return moTeacherInfoStruct.miExpInMonths;
            }
            set
            {
                moTeacherInfoStruct.miExpInMonths = value;
            }
        }

        public DateTime DateOfJoining
        {
            get
            {
                return moTeacherInfoStruct.mdtDateOfJoining;
            }
            set
            {
                moTeacherInfoStruct.mdtDateOfJoining = value;
            }
        }       

        public string Achivements
        {

            get
            {
                return moTeacherInfoStruct.msAchivements;
            }
            set
            {
                moTeacherInfoStruct.msAchivements = value;
            }
        }

        public int InsertedByid
        {
            get
            {
                return moTeacherInfoStruct.miInsertedByid;
            }
            set
            {
                moTeacherInfoStruct.miInsertedByid = value;
            }
        }

        public int UpdatedById
        {
            get
            {
                return moTeacherInfoStruct.miUpdatedById;
            }
            set
            {
                moTeacherInfoStruct.miUpdatedById = value;
            }
        }

        public string Salutataion
        {

            get
            {
                return moTeacherInfoStruct.msSalutation;
            }
            set
            {
                moTeacherInfoStruct.msSalutation = value;
            }
        }

        public string LocalState
        {
            get
            {
                return moTeacherInfoStruct.msLocalState;
            }
            set
            {
                moTeacherInfoStruct.msLocalState = value;
            }
        }

        public string PermanentState
        {

            get
            {
                return moTeacherInfoStruct.msPermanentState;
            }
            set
            {
                moTeacherInfoStruct.msPermanentState = value;
            }
        }
        public int Academic_Year_Id
        {
            get
            {
                return moTeacherInfoStruct.miAcademicYearId;
            }
            set
            {
                moTeacherInfoStruct.miAcademicYearId = value;
            }
        }

        public char IsAcademicYrApplicable
        {

            get
            {
                return moTeacherInfoStruct.mcIsAcademicYrApplicable;
            }
            set
            {
                moTeacherInfoStruct.mcIsAcademicYrApplicable = value;
            }
        }

        public string PhotoFilePath
        {

            get
            {
                return moTeacherInfoStruct.msPhotoFilePath;
            }
            set
            {
                moTeacherInfoStruct.msPhotoFilePath = value;
            }
        }
        public byte[] BinaryPhotoImage
        {

            get
            {
                return moTeacherInfoStruct.msBinaryPhotoImage;
            }
            set
            {
                moTeacherInfoStruct.msBinaryPhotoImage = value;
            }
        }

        public int ExpDetailsId
        {
            get { return moTeacherInfoStruct.miExpDetailsId; }
            set { moTeacherInfoStruct.miExpDetailsId = value; }
        }

        public string SchoolName
        {
            get { return moTeacherInfoStruct.msSchoolName; }
            set { moTeacherInfoStruct.msSchoolName = value; }
        }
         
        public DateTime JoinDate
        {
            get { return moTeacherInfoStruct.mdtJoinDate; }
            set { moTeacherInfoStruct.mdtJoinDate = value; }
        }
        
        public DateTime LeftDate
        {
            get { return moTeacherInfoStruct.mdtLeftDate; }
            set { moTeacherInfoStruct.mdtLeftDate = value; }
        }

        public virtual System.DateTime InsertDate
        {
            get
            {
                return moTeacherInfoStruct.mdtInsertDate;
            }
            set
            {
                moTeacherInfoStruct.mdtInsertDate = value;
            }
        }
        public virtual System.DateTime UpdateDate
        {
            get
            {
                return moTeacherInfoStruct.mdtUpdateDate;
            }
            set
            {
                moTeacherInfoStruct.mdtUpdateDate = value;
            }
        }

        public  string PreviousDesignation
        {
            get
            {
                return moTeacherInfoStruct.msPreviousDesignation;
            }
            set
            {
                moTeacherInfoStruct.msPreviousDesignation = value;
            }
        }
        public  decimal Last_Salary
        {
            get
            {
                return moTeacherInfoStruct.msLast_Salary;
            }
            set
            {
                moTeacherInfoStruct.msLast_Salary = value;
            }
        }
        public  string Job_Description
        {
            get
            {
                return moTeacherInfoStruct.msJob_Description;
            }
            set
            {
                moTeacherInfoStruct.msJob_Description = value;
            }
        }
        public  string Reason_for_Leaving
        {
            get
            {
                return moTeacherInfoStruct.msReason_for_Leaving;
            }
            set
            {
                moTeacherInfoStruct.msReason_for_Leaving = value;
            }
        }

        public  string DurationDays
        {
            get
            {
                return moTeacherInfoStruct.msDurationDays;
            }
            set
            {
                moTeacherInfoStruct.msDurationDays = value;
            }
        }
		public bool IsFinancialYearApplicable
		{
			get { return moTeacherInfoStruct.IsFinancialYearApplicable; }
			set { moTeacherInfoStruct.IsFinancialYearApplicable = value; }
		}

        public int AssociatedStandardCategory
        {
            get { return moTeacherInfoStruct.AssociatedStandardCategory; }
            set { moTeacherInfoStruct.AssociatedStandardCategory = value; }
        }

        public int TeacherTypeId
        {
            get { return moTeacherInfoStruct.TeacherTypeId; }
            set { moTeacherInfoStruct.TeacherTypeId = value; }
        }

        #endregion

        #endregion

        #region " OverLoaded Constructors "

        public SchoolWiseTeacherMasterBL()
        {
            //Default constructor
            moSchoolWiseTeacherMasterDC = new SchoolWiseTeacherMasterDC();
        }

        public SchoolWiseTeacherMasterBL(int aiTeacherId)
        {
            //Default constructor
            moSchoolWiseTeacherMasterDC = new SchoolWiseTeacherMasterDC(aiTeacherId);
            moTeacherInfoStruct = moSchoolWiseTeacherMasterDC.TeacherInfoStructure;
        }
        public static DataSet FetchAllTeacherDetails(int aiTeacherId, int aiAcademicYrId, int aiSchoolId,int  aiUserId)
        {
            return SchoolWiseTeacherMasterDC.FetchAllTeacherDetails(aiTeacherId, aiAcademicYrId, aiSchoolId, aiUserId);
        }
       
        public static DataSet FetchTeacherStdSubjectDetails(int aiAcademicYrId, int aiSchoolId)
        {
            return SchoolWiseTeacherMasterDC.FetchTeacherStdSubjectDetails(aiAcademicYrId, aiSchoolId);
        }

        #endregion

        #region " Public Methods "

        /// <summary>
        /// This method is used to get all details from UI to insert in database.
        /// </summary>
        /// <returns></returns>

        public int InsertTeacherDetails()
        {
            // This Function is used to insert the record in to database.

            ArrayList oArrayListInsertStatements = new ArrayList();
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = moTeacherInfoStruct;
            oArrayListInsertStatements.Add(moSchoolWiseTeacherMasterDC.GetTeacherDetailsInsertStatement());
            oArrayListInsertStatements.Add(DataCommunicatorBaseDC.GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY));
            foreach (TeacherEducationDetailsBL oTeacherEducationDetailsBL in moTeacherEduDetails)
            {
                oArrayListInsertStatements.Add(oTeacherEducationDetailsBL.InsertTeacherEducationDetails());
            }
            foreach (TeacherExperienceDetailsBL oTeacherExperienceDetailsBL in moTeacherExperienceDetails)
            {
                oArrayListInsertStatements.Add(oTeacherExperienceDetailsBL.InsertExperienceDetails());
            }
            foreach (TeacherSubjectDetailsBL oTeacherSubjectDetailsBL in moTeacherSubDetails)
            {
                oArrayListInsertStatements.Add(oTeacherSubjectDetailsBL.InsertTeacherSubjectDetails());
            }
            foreach (TeacherStandardDetailsBL oTeacherStandardDetailsBL in moTeacherStandardDetails)
            {
                oArrayListInsertStatements.Add(oTeacherStandardDetailsBL.InsertTeacherStandardDetails());
            }
            foreach (UserShiftAssociationBL oUserShiftsAssociationBL in moUserShiftAssociation)
            {
                oArrayListInsertStatements.Add(oUserShiftsAssociationBL.InsertUserShiftAssociationDetails());
            }
            foreach (UserWeekEndAssociationBL oUserWeekendAssociationBL in moUserWeekendAssociation)
            {
                oArrayListInsertStatements.Add(oUserWeekendAssociationBL.InsertUserWeekendAssociationDetails());
            }
            return moSchoolWiseTeacherMasterDC.InsertTeacherDetails(oArrayListInsertStatements);
        }

        public static string CheckDependenciesAndGetErrorMessagesForSubject(TeacherSubjectDetailsBL oTeacherSubjectDetailsBL)
        {
            int iParentId = Convert.ToInt32(Constants.ReferenceId.TeacherSubjectAssignment);
            int iParentIdValue = oTeacherSubjectDetailsBL.TeacherSubjectId;
            string sName = oTeacherSubjectDetailsBL.SubjectName;
            int iAcademicYearId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]);

            return ReferenceDC.CheckDependenciesAndGetErrorMessages(iParentId, iParentIdValue, sName, iAcademicYearId);
        }

        public static string CheckDependenciesAndGetErrorMessagesForStandard(TeacherStandardDetailsBL oTeacherStandardDetailsBL)
        {
            int iParentId = Convert.ToInt32(Constants.ReferenceId.TeacherStandardAssignment);
            int iParentIdValue = oTeacherStandardDetailsBL.TeacherStandardId;
            string sName = oTeacherStandardDetailsBL.StandardName;
            int iAcademicYearId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]);

            return ReferenceDC.CheckDependenciesAndGetErrorMessages(iParentId, iParentIdValue, sName, iAcademicYearId);
        }

        public Int32 UpdateTeacherDetails(ArrayList oArrayListUpdateStatements)
        {
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = this.moTeacherInfoStruct;
            oArrayListUpdateStatements.Add(TeacherEducationDetailsCollectionBL.RemoveEducationalDetailsForTeacherId(TeacherId));
            oArrayListUpdateStatements.Add(TeacherExperienceCollectionBL.RemoveExperienceDetailsForTeacherId(TeacherId));
            oArrayListUpdateStatements.Add(TeacherSubjectDetailsCollectionBL.RemoveAllSubjectsForTeacherId(TeacherId));
            oArrayListUpdateStatements.Add(TeacherStandardDetailsCollectionBL.RemoveAllStandardForTeacherId(TeacherId)); 
            
            foreach (TeacherEducationDetailsBL oTeacherEducationDetailsBL in moTeacherEduDetails)
            {
                oArrayListUpdateStatements.Add(oTeacherEducationDetailsBL.InsertTeacherEducationDetails());
            }
            foreach (TeacherExperienceDetailsBL oTeacherExperienceDetailsBL in moTeacherExperienceDetails)
            {
                oArrayListUpdateStatements.Add(oTeacherExperienceDetailsBL.InsertExperienceDetails());
            }
            foreach (TeacherSubjectDetailsBL oTeacherSubjectDetailsBL in moTeacherSubDetails)
            { 
                oArrayListUpdateStatements.Add(oTeacherSubjectDetailsBL.InsertTeacherSubjectDetails());
            }
            foreach (TeacherStandardDetailsBL oTeacherStandardDetailsBL in moTeacherStandardDetails)
            {
                oArrayListUpdateStatements.Add(oTeacherStandardDetailsBL.InsertTeacherStandardDetails());
            }

            return moSchoolWiseTeacherMasterDC.UpdateTeacherDetails(oArrayListUpdateStatements);
        }

        public Int32 UpdateTeacherMobileNo()
        {
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = this.moTeacherInfoStruct;
            return moSchoolWiseTeacherMasterDC.UpdateTeacherMobileNo();
        }

        public Int32 UpdateTeachersAcademicYrApplicable()
        {
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = this.moTeacherInfoStruct;
            return moSchoolWiseTeacherMasterDC.UpdateTeachersAcademicYrApplicable();
        }

        /// <summary>
        /// This method is used to Get User Id from User Name.
        /// </summary>
        /// <param name="asUserName"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public Int32 GetUserIdFromUserName(string asUserName, int aiSchoolId, int aiAcademicYearId)
        {
            return moSchoolWiseTeacherMasterDC.GetUserIdFromUserName(asUserName, aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get teacher id of selected year.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataTable GetTeacherDetails(int aiSchoolId, int aiAcademicYrId, int aiTeacherId)
        {
            return SchoolWiseTeacherMasterDC.GetTeacherDetails(aiSchoolId, aiAcademicYrId, aiTeacherId);
        }

        public DataTable GetAssignedClassTeacher(int aiSchoolId, int aiStandardId, int aiDivisionId)
        {
            return moSchoolWiseTeacherMasterDC.GetAssignedClassTeacher(aiSchoolId, aiStandardId, aiDivisionId);
        }
        public DataTable GetAssignedClassTeacher(int aiSchoolId, int aiStdDivId)
        {
            return moSchoolWiseTeacherMasterDC.GetAssignedClassTeacher(aiSchoolId, aiStdDivId);
        }

        public static DataSet GetTeacherDetailsForControlPanel(int aiTeacherId, int aiSchoolId, int aiAcademicYrId)
        {
            return SchoolWiseTeacherMasterDC.GetTeacherDetailsForControlPanel(aiTeacherId, aiSchoolId, aiAcademicYrId);
        }


        public void UploadTeacherstPhoto(Collection<SchoolWiseTeacherMasterBL> oTeachers)
        {
            IEnumerator oIEnum = oTeachers.GetEnumerator();
            ArrayList oArrayList = new ArrayList();
            while (oIEnum.MoveNext())
            {
                SchoolWiseTeacherMasterBL oSchoolWiseTeacherMasterBL = (SchoolWiseTeacherMasterBL)oIEnum.Current;
                oArrayList.Add(oSchoolWiseTeacherMasterBL.GetUpdateStaementForPhotoUpload());
                
            }

            SchoolWiseTeacherMasterDC oSchoolWiseTeacherMasterDC = new SchoolWiseTeacherMasterDC();
            moSchoolWiseTeacherMasterDC.UploadTeacherPhoto(oArrayList);
        }

        private string GetUpdateStaementForPhotoUpload()
        {
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = moTeacherInfoStruct;
            return moSchoolWiseTeacherMasterDC.GetUpdateStaementForPhotoUpload();
        }
        #endregion

        public DataSet getTeacherIdentityCards(int iSchoolID, int iAcademicYrID, int miTeacherId,string asTeacherName)
        {
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = moTeacherInfoStruct;
            return moSchoolWiseTeacherMasterDC.getTeacherIdentityCards(iSchoolID, iAcademicYrID, miTeacherId, asTeacherName);   
        }        

        public void InsertExperienceDetails()
        {
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = moTeacherInfoStruct;
            moSchoolWiseTeacherMasterDC.InsertExperienceDetails();
        }

        public void DeleteExperienceDetails(int iExpDetailsId, string sSchoolName, int iUserId)
        {
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = moTeacherInfoStruct;
            moSchoolWiseTeacherMasterDC.DeleteExperienceDetails(iExpDetailsId, sSchoolName, iUserId);
        }
        public bool IsDuplicateSchoolName()
        {
            moSchoolWiseTeacherMasterDC.TeacherInfoStructure = moTeacherInfoStruct;
            bool bIsDuplicate = moSchoolWiseTeacherMasterDC.IsDuplicateName();
            if (bIsDuplicate == false)
                throw new BusinessLogic.Exceptions.DuplicateEntityException(S_DUPLICATE_SCHOOL_NAME);
            return bIsDuplicate;
        }

        public void UpdateExperienceDetails()
        {
             moSchoolWiseTeacherMasterDC.TeacherInfoStructure = moTeacherInfoStruct;
             moSchoolWiseTeacherMasterDC.UpdateExperienceDetails();
        }
    }

    public class SchoolWiseTeacherMasterCollectionBL
    {
        private SchoolWiseTeacherMasterCollectionDC moSchoolWiseTeacherMasterCollectionDC = null;
        TeacherEducationDetailsCollectionBL oTeacherEducationDetailsCollectionBL =
                                             new TeacherEducationDetailsCollectionBL();
        TeacherSubjectDetailsCollectionBL oTeacherSubjectDetailsCollectionBL =
                                             new TeacherSubjectDetailsCollectionBL();
        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL =
                                            new TeacherStandardDetailsCollectionBL();

        SchoolUserCollectionBL oSchoolUserCollectionBL = new SchoolUserCollectionBL();

        SchoolWiseStandardDivisionTeacherAssignmentMasterBL oAssignClassTeacher =
                                                   new SchoolWiseStandardDivisionTeacherAssignmentMasterBL();

        public SchoolWiseTeacherMasterCollectionBL()
        {
            moSchoolWiseTeacherMasterCollectionDC = new SchoolWiseTeacherMasterCollectionDC();
        }
        /// <summary>
        /// This method returns the message informing about the dependencies.
        /// </summary>
        /// <returns></returns>
        public string CheckDependenciesForTeacher(DictionaryEntry aoTeacher, int aiAcademicYearId)
        {
            string sReturn = "";
            sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.Teacher), Convert.ToInt32(aoTeacher.Key), aoTeacher.Value.ToString(), aiAcademicYearId);
            return sReturn;
        }

        ///// <summary>
        ///// This method is used to delete the teacher dependency details.
        ///// </summary>
        ///// <param name="aiUserId"></param>
        ///// <param name="aiSchoolId"></param>
        ///// <param name="aiAcademicYearId"></param>
        ///// <param name="aiTeacherId"></param>
        //public void DeleteTeacherDependencyDetails(int aiUserId, int aiSchoolId, int aiAcademicYearId, int aiTeacherId)
        //{
        //    ReferenceDC.DeleteTeacherDependencyDetails(aiUserId, aiSchoolId, aiAcademicYearId, aiTeacherId);
        //}
        /// <summary>
        /// This method returns the message informing about the dependencies.
        /// </summary>
        /// <returns></returns>
        public string CheckDependenciesForTeacher(int aiTecherId,string sTeacherName, int aiAcademicYearId)
        {
            string sReturn = "";
            sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.Teacher), aiTecherId, sTeacherName, aiAcademicYearId);
            return sReturn;
        }

        public void DeleteTeacher(int aiSchoolId, int aiTecherId, string sTeacherName, int aiAcademicYearId, int aiUserId, int aiFinancialYearId)
        {
            //DeleteTeacherDependencyDetails(aiUserId, aiSchoolId, aiAcademicYearId, aiTecherId);
            ArrayList oArralist = new ArrayList();
            string sMessage = "";
            string sReferenceMessage = CheckDependenciesForTeacher(aiTecherId, sTeacherName, aiAcademicYearId);
            if (!sReferenceMessage.Equals(""))
            {
                sMessage = sMessage + "<BR>" + sReferenceMessage;
                throw new Exceptions.ReferenceExceptions(sMessage);
            }
            else if (sMessage.Equals(""))
            {
                oTeacherEducationDetailsCollectionBL.DeleteTeacherEducationDetails(aiTecherId);
                oTeacherSubjectDetailsCollectionBL.DeleteTeacherSubjectDetails(aiTecherId);
                oTeacherStandardDetailsCollectionBL.DeleteTeacherStandardDetails(aiTecherId);
                oSchoolUserCollectionBL.DeleteUserWhoAreTeacher(aiTecherId);
                oAssignClassTeacher.DeleteAssignStandardDivisionForTeacher(aiTecherId);
                moSchoolWiseTeacherMasterCollectionDC.DeleteTeacher(aiTecherId, aiUserId);
                SchoolWiseTeacherMasterCollectionDC.DeleteTeacherConfiguration(aiSchoolId, aiAcademicYearId, Convert.ToInt32(Constants.SchoolConfigurations.Teacher), aiFinancialYearId);
            }


        }

        public void InsertMultipleTeachers(int aiSchoolId, int aiAcademicYearId, int aiInsertedById,
                                           string asTeacherDetails, bool abCanPublishUnpublishExam)
        {
            moSchoolWiseTeacherMasterCollectionDC.InsertMultipleTeachers(aiSchoolId, aiAcademicYearId,
                                                                         aiInsertedById, asTeacherDetails, abCanPublishUnpublishExam);
        }
        public bool DeleteMultipleTeacher(Hashtable aoArrDeleteTeacherIds, int aiAcademicYearId)
        {
            ArrayList oArralist = new ArrayList();
            string sMessage = "";

            foreach (DictionaryEntry oTeacher in aoArrDeleteTeacherIds)
            {

                string sReferenceMessage = CheckDependenciesForTeacher(oTeacher, aiAcademicYearId);
                if (!sReferenceMessage.Equals(""))
                {
                    sMessage = sMessage + "<BR>" + sReferenceMessage;

                }
                else if (sMessage.Equals(""))
                {
                    oArralist.Add(Convert.ToInt32(oTeacher.Key.ToString()));
                }

            }
            if (sMessage.Equals(""))
            {


                oTeacherEducationDetailsCollectionBL.DeleteTeacherEducationDetails(oArralist);
                oTeacherSubjectDetailsCollectionBL.DeleteTeacherSubjectDetails(oArralist);
                oTeacherStandardDetailsCollectionBL.DeleteTeacherStandardDetails(oArralist);
                oSchoolUserCollectionBL.DeleteMultipleUserWhoAreTeacher(oArralist);
                oAssignClassTeacher.DeleteAssignStandardDivisionForTeacher(oArralist);
                return moSchoolWiseTeacherMasterCollectionDC.DeleteMultipleTeacher(oArralist);
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }
        }

        public static DataTable FetchTeacherDetailsForMessageFacillity(int aiSchoolId, int aiAcademicYrId, int aiTeacherID, int aiUserId, int aiUserRoleId, int aiTypeId)
        {
            //This function is used to fetch Teacher details for Messaging.

            return SchoolWiseTeacherMasterCollectionDC.FetchTeacherDetailsForMessageFacillity(aiSchoolId, aiAcademicYrId, aiTeacherID, aiUserId, aiUserRoleId, aiTypeId);
        }
        public static DataSet FetchAllTeachers(int aiSchoolId, int aiAcademicYrId)
        {
            return SchoolWiseTeacherMasterCollectionDC.FetchAllTeachers(aiSchoolId, aiAcademicYrId);
        }

         /// <summary>
        /// This method is used to get associated standards of a particular teacher.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataTable GetAssociatedStdLstForTeacher(int aiUserId, int aiAcademicYrId)
        {
            return SchoolWiseTeacherMasterCollectionDC.GetAssociatedStdLstForTeacher(aiUserId, aiAcademicYrId);
        }

        //public static DataTable FetchTeacherDetailsByUserID(int aiUserID)
        //{
        //    //This function is used to fetch the teacher details by user ID.

        //    return SchoolWiseTeacherMasterCollectionDC.FetchTeacherDetailsByUserID(aiUserID);
        //}

        public static DataTable GetAllTeachersByName(Int32 aiSchoolId, Int32 aiAcademicYrId, string asRegNumbers)
        {

            return SchoolWiseTeacherMasterCollectionDC.GetAllTeachersByName(aiSchoolId, aiAcademicYrId, asRegNumbers);
        }

    }

}
