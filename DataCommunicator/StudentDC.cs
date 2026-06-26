using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Utility;
using System.Web;
using System.Configuration;
using Management.Entities;
using StudentEntities;
using SchoolEntities;
using SchoolEntities.Dashboard;
using SchoolEntities.Admin;
using SchoolEntities.Common;
using System.Reflection;

namespace DataCommunicator
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentDC : DataCommunicatorBaseDC
    {
        #region Constants & Structure

        private const int I_BONAFIED_CERTIFICATE_ID = 83;
        private const int I_BONAFIED_CERTIFICATE_FOR_SS_ID = 117;
        //structure for basic information of student
        public struct StudentInfo
        {
            // This structure is replica of Company_Master database table.
            public Int32 SchoolId;
            public string sLoginName;
            public string sPhotoFilePath;
            public string sPhotoFileInBinary;
            public Byte[] sPhotoFilePathInBinary;
            public Int32 iStudentId;
            public string sEnrollmentNo;
            public string sFirstName;
            public string sMiddleName;
            public string sLastName;
            public string sMother_Name;
            public DateTime dDob;
            public string sBirthPlace;
            public string sNationality;
            public string sBloodGroup;
            public char cSex;
            public DateTime dAdmissionDate;
            public DateTime dJoining_Date;
            public DateTime dLeftDate;
            public int CancellationFormNo;
            public string sParentName;
            public int iParentOcupation;
            public string sOtherOcupation;
            public string sAddress;
            public string sBusPickupCity;
            public string sCity;
            public string sState;
            public string sPincode;
            public string sResidencePhoneNo;
            public string sMobilePhoneNo;
            public string sMobilePhoneNo2;
            public string sMotherTongue;
            public Int32 iCategoryId;
            public int iRTECategoryId;
            public string sRTEFormNo;
            public int sAnnualIncome;
            public string sCasteAndSubCaste;
            public char mcIsLeave;
            public Int32 iUser_Id;
            public string sEmail;
            public Int32 iSalutationId;
            public Int32 iInsertedById;
            public Int32 iUpdatedById;
            public string sDateOfBirthInText;
            public string sSalutationName;
            public string sStandardName;
            public char cIs_Dummy_Admission;
            public bool bIsNewStudent;
            public bool bIsRTEStudent;
            public int iRule_Id;
            public bool bIsStaffKid;
            public SchoolUserDC oSchoolUserDC;
            public int iAcademicYearId;
            public int iOptionalSubjectId;
            public string sFormNo;
            public string sOfficeNo;
            public string sNeighbourNo;
            public string sStidentSiblingNames;
            public double dHeight;
            public double dWeight;
            public string sLastSchoolName;
            public string sLastSchoolAddress;
            public string sLastSchoolStandard;
            public string sLastSchoolUDISENo;
            public string sLastSchoolBoardName;
            public bool bIsRecognised;
            public string sAadharCardNo;
            public string sNameOnAadharCard;
            public string sAadharCardNumberPhotoCopyName;
            public string sFamilyPhoto;
            public string sCasteCertPhoto;
            public int iSecondLanguageSubjectId;
            public int iThirdLanguageSubjectId;
            public int iParentUserRoleId;
            public int iParentUserId;
            public string sUDISENumber;
            public string sBoardRegNo;
            public string AdditionalStudentStatus;
            public string AdditionalAdmissionAcademicYear;
            public string AdditionalAdmissionStandard;
            public string PreviousYearOfPassing;
            public bool AdditionalIsHandicapped;
            public string AdditionalCurrentAcademicYear;
            public string AdditionalCurrentStandard;
            public int AdditionalPreviousMarksObtained;
            public int AdditionalPrviousMarksOutOff;
            public string AdditionalPreviousYearOfPassing;
            public string AdditionalSubjectNames;
            public int SchoolwiseStudentId;
            public string sReligion;
            public string sCategory;
            public string sUDISEnumber;
            public string sPlaceOfBirth;
            public bool IsRiseAndShine;
            public string sPassword;
            public string sOldEnolmentNo;
            public int AdmissionForId;
            public string sGRNumber;
            public string sStudentUniqueNo;
            public string sConfirmedByText;
            public string sUpdatedByText;
            public bool IsForDayBoarding;
            public bool IsDayBoardingFeePaid;
            public int FeeCategoryId;
            public bool AreAdditionalDetailsApplicable;
            public string SaralNo;
            public bool IsOnlyChild;
            public bool Minority;
            public int StreamId; //
            public int StreamwiseGroupId; /////
            public int OptSubjectOne; ////
            public int OptSubjectTwo; ////
            public string CompulsorySubject; ////
            public string chkCompitativeExams; /////

            public int iResidenceTypeId;
            public string sResidenceTypeName;
            public string sRFID;
            public string sAdmissionStandard;
            public string sIsPrePrimaryStandard;
            public string sPrePrimaryEnrolmentNumber;
            public string PenNo;
        }

        //structure for yearwise details of student
        public struct YearWiseStudentInfo
        {
            // This structure is replica of Company_Master database table.
            public Int32 iYearWIseStudentId;
            public Int32 iSchoolId;
            public Int32 iStudentId;
            public Int32 iYearId;
            public Int32 iStandardId;
            public Int32 iDivisionId;
            public string sStandardDivisionName;
            public Int32 iRollNo;
            public string sName;
            public Int32 iSchoolWiseAcademicYearId;
        }

        #endregion

        #region DataMembers & Properties

        #region DataMembers
        private StudentInfo moStudentInfo;
        private YearWiseStudentInfo moYearWiseStudentInfo;
        private static bool mbIncludeUserName = false;
        public int miStudentCount;
        private static string msOperator = string.Empty;
		public List<BlackListedStudent> lstBlackListStudent = new List<BlackListedStudent>();
        #endregion

        #region Properties
        public StudentInfo StudentDetails
        {
            get
            {
                return moStudentInfo;
            }
            set
            {
                moStudentInfo = value;
            }
        }
        public YearWiseStudentInfo YearWiseStudentDetails
        {
            get
            {
                return moYearWiseStudentInfo;
            }
            set
            {
                moYearWiseStudentInfo = value;
            }
        }

        #endregion

        #endregion

        #region constructors
        public StudentDC()
        {
            //Default constructor
        }

        public StudentDC(Int32 aiYearwiseStudentId)
        {

            DataTable oDTStudent = RetriveStudentInfo(aiYearwiseStudentId);
            if (oDTStudent.Rows.Count > 0)
            {
                FillStruct(oDTStudent);

            }
        }

        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        public StudentDC(Int32 aiSchoolId, Int32 aiStudentId)
        {

            DataTable oDTStudent = RetriveStudentInfo(aiSchoolId, aiStudentId);
            if (oDTStudent.Rows.Count > 0)
            {
                FillStruct(oDTStudent);

            }
        }

        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        public StudentDC(Int32 aiSchoolId, Int32 aiStudentId, bool abIsYrwise)
        {

            DataTable oDTStudent = RetriveStudentInfo(aiSchoolId, aiStudentId, abIsYrwise);
            if (oDTStudent.Rows.Count > 0)
            {
                FillStruct(oDTStudent);

            }
        }

        /// <summary>
        /// Parameterised constructor
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        public StudentDC(Int32 aiSchoolId, Int32 aiAccYearId, Int32 aiStudentId)
        {

            DataTable oDTStudent = RetriveStudentInfo(aiSchoolId, aiAccYearId, aiStudentId);
            if (oDTStudent.Rows.Count > 0)
            {
                FillStruct(oDTStudent);
            }
        }

        public StudentDC(Int32 aiSchoolId, int aiAcademicYrId, string asRegistration)
        {

            DataTable oDTStudent = RetriveStudentInfo(aiSchoolId, aiAcademicYrId, asRegistration);
            if (oDTStudent.Rows.Count > 0)
            {
                FillStruct(oDTStudent);

            }
        }

        /// <summary>
        /// Custructor to initiate student object for a provided school id and user id
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiRegNo"></param>
        /// <param name="aiUserID"></param>
        public StudentDC(Int32 aiSchoolId, String asRegNo, Int32 aiUserID)
        {
            DataTable oDTStudent = RetriveStudentInfo(aiSchoolId, asRegNo, aiUserID);
            if (oDTStudent.Rows.Count > 0)
            {
                FillStruct(oDTStudent);

            }
        }


        /// <summary>
        /// Get student stremwise subject details
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiRegNo"></param>
        /// <param name="aiUserID"></param>

        public DataSet RetriveStudentSubjectInfo(Int32 aiSchoolId, Int32 aiStudentId, int aiAcademicYearId)
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);

                // return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("GetStudentStreamwiseSubjectDetails");
                DataSet ods = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStudentStreamwiseSubjectDetails");

                return ods;
            }

        }



        #endregion

        #region public methods

        #region Basic Details
        /// <summary>
        /// This method inserts student records in 
        /// basic informatoion table
        /// yearwise infrmation table
        /// It gets the insert statements for each forms array of statements for transaction
        /// And executes the transaction
        /// </summary>
        /// <returns>returns academic year wise student id </returns>
        public int InsertStudent(string asSiblingStudentIds)
        {
            int iYrwiseStudentId = 0;

            int iChallanNo = 0;
            if (moStudentInfo.SchoolId == Constants.SchoolId.SNS.ToInt())
            {
                iChallanNo = GetNextChallanNo(moStudentInfo.SchoolId, Constants.I_ZERO);
            }

            string[] sArrInsert = new string[4]; //contains query strings to form transactions
            //insert statement to inser basic details
            sArrInsert[0] = CreateInsertStatementForStudentBasicDetais(iChallanNo);
            //insert statement for Streamwise subjects of students

            //statement to get value of last inserted key (student id)
            sArrInsert[1] = GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY);
            //insert statement to inser yearwise details
            sArrInsert[2] = CreateinsertStatementForYearWiseDetails();
            int iReturnValue;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iReturnValue = oSQLServerDbUtility.ExecuteTransaction(sArrInsert, Constants.PrimaryKeyRecord.Last);


            if (iReturnValue != 0)
            {
                iYrwiseStudentId = iReturnValue;
                if (HttpContext.Current.Session[Constants.S_SESSION_USER_IMAGE_DATA] != null || moStudentInfo.sPhotoFilePath != string.Empty)
                {
                    string sSQL = "UPDATE SchoolWise_Student_Master SET Photo_file_Path_Image = @Image" +
                  ", ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                  " WHERE School_Id = " + moStudentInfo.SchoolId.ToString() +
                  " AND SchoolWise_Student_Id = (SELECT Student_Id FROM YearWise_Student_Details WHERE School_Id = " + moStudentInfo.SchoolId + " AND Academic_Year_ID = " + moStudentInfo.iAcademicYearId + " AND YearWise_Student_Id = " + iYrwiseStudentId + " )" +
                  " AND Is_Deleted = '" + Constants.C_NO + "'";

                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                        oSQLServerDbUtility.ExecuteTransaction(moStudentInfo.sPhotoFilePathInBinary, sSQL);
                }

                if (moStudentInfo.AreAdditionalDetailsApplicable == false)
                {
                    string sAdditionalDetails = "INSERT INTO StudentAdditionalDetails(SchoolwiseStudentId, Religion,City,IsDeleted) SELECT Student_Id, '" + moStudentInfo.sReligion + "','" + moStudentInfo.sBusPickupCity + "',0 FROM YearWise_Student_Details WHERE School_Id = " + moStudentInfo.SchoolId + " AND Academic_Year_ID = " + moStudentInfo.iAcademicYearId + " AND YearWise_Student_Id = " + iYrwiseStudentId;
                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                        oSQLServerDbUtility.ExecuteTransaction(sAdditionalDetails);
                }

                MarkNewStudentsMarks(iYrwiseStudentId, new DateTime(1900, 1, 1));
                UpdateAttendance(false, iYrwiseStudentId);
                InsertStudentFeeDetails(iYrwiseStudentId, iChallanNo);

                SetOptionalSubjects(iYrwiseStudentId);

                if (!moStudentInfo.bIsRTEStudent)
                    InsertStudentCautionMoneyFeeDetails(iYrwiseStudentId);
                if (asSiblingStudentIds != string.Empty)
                    InsertStudentSiblingDetails(iYrwiseStudentId, asSiblingStudentIds);

                if (moStudentInfo.SchoolId == Constants.SchoolId.SNS.ToInt())   ////////////
                {
                    string sStmt = CreateInsertStatementForStudentStreamwiseSubjectDetails(iYrwiseStudentId);
                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                        oSQLServerDbUtility.ExecuteTransaction(sStmt);
                }
                InsertSwitchLoginDetails(moStudentInfo.SchoolId, iYrwiseStudentId, moStudentInfo.iParentUserId);
            }
            return iYrwiseStudentId;
        }

        private void SetOptionalSubjects(int aiYrwiseStudentId)
        {
            SecondLanguageDC oSecondLanguageDC = new SecondLanguageDC(moStudentInfo.SchoolId, moYearWiseStudentInfo.iYearId);
            oSecondLanguageDC.Update(string.Empty, moStudentInfo.iInsertedById, moYearWiseStudentInfo.iStandardId, moYearWiseStudentInfo.iDivisionId, aiYrwiseStudentId);
        }

        public void UploadStudentPhoto(ArrayList aoArrayListUpdateStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListUpdateStatements.ToArray(typeof(string)));
        }

        public string GetUpdateStaementForPhotoUpload()
        {
            string sUpdateStatement;

            sUpdateStatement = " UPDATE SchoolWise_Student_Master SET " +
                               " Photo_file_Path = '" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sPhotoFilePath, false) + "'" +
                               " WHERE SchoolWise_Student_Id = " + moStudentInfo.iStudentId +
                               " AND School_Id = " + moStudentInfo.SchoolId;

            // Query for list of student whose update profile pic only. 
            string sSQL = " UPDATE SchoolWise_Student_Master SET Photo_file_Path_Image = @Image, ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                           " WHERE School_Id = " + moStudentInfo.SchoolId.ToString() +
                           " AND SchoolWise_Student_Id=" + moStudentInfo.iStudentId +
                           " AND Is_Deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(moStudentInfo.sPhotoFilePathInBinary, sSQL);

            return sUpdateStatement;
        }



        private void MarkNewStudentsMarks(int iYrwiseStudentId, DateTime adtOldJoiningDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", moStudentInfo.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", moYearWiseStudentInfo.iYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudentId", iYrwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OldJoiningDate", adtOldJoiningDate.Date, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("UpdatedById", moStudentInfo.iInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateNewStudentsMarks");
            }

        }

        private void DeleteStudentFeeDetails(int aiYrwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moStudentInfo.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", moYearWiseStudentInfo.iYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiYrwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsNewStudent", moStudentInfo.bIsNewStudent, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Rule_Id", moStudentInfo.iRule_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", moStudentInfo.iUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("USP_DeleteFeeDetailsForStudent");
            }
        }

        /// <summary>
        /// This method is used to insert default fee entries to fee table.
        /// </summary>
        /// <param name="aiYrwiseStudentId"></param>
        private void InsertStudentFeeDetails(int aiYrwiseStudentId, int aiChallanNo)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moStudentInfo.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", moYearWiseStudentInfo.iYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiYrwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsNewStudent", moStudentInfo.bIsNewStudent, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Rule_Id", moStudentInfo.iRule_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", moStudentInfo.iInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChallanNo", aiChallanNo, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("USP_InsertFeeDetailsForStudent");
            }
        }

        /// <summary>
        /// This method is used to insert student default caution money details.
        /// </summary>
        /// <param name="iYrwiseStudentId"></param>
        private void InsertStudentCautionMoneyFeeDetails(int iYrwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", moStudentInfo.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcadmicYearId", moYearWiseStudentInfo.iYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandardId", moYearWiseStudentInfo.iStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iYearwiseStudentId", iYrwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iInsertedByid", moStudentInfo.iInsertedById, SqlDbType.Bit);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertCautionMoneyDetails");
            }
        }

        /// <summary>
        /// Function to get  insert statement to add new student's basic information into database
        /// </summary>
        /// <returns></returns>
        /// 

        public string CreateInsertStatementForStudentBasicDetais(int aiChallanNo)
        {
            string sFoto = "";
            if (HttpContext.Current.Session[Constants.S_SESSION_USER_IMAGE_DATA] != null || !moStudentInfo.sPhotoFilePath.IsNullOrEmpty())
            {
                sFoto = " , '" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sPhotoFilePath, true) + "'";
            }
            else
            {
                sFoto = " , NULL ";
            }
            string sInsertString = "INSERT INTO " +
                                       "  SchoolWise_Student_Master (" +
                                       "  School_Id" +
                                       " , Salutation_Id " +
                                       " , First_Name" +
                                       " , Middle_Name" +
                                       " , Last_Name " +
                                       " , Mother_Name " +
                                       " , Blood_Group " +
                                       " , Enrolment_Number" +
                                       " , Sex " +
                                       " , Dob " +
                                       " , Birth_Place" +
                                       " , Nationality" +
                                       " , Admission_date " +
                                       " , Joining_Date " +
                                       " , Parent_Name" +
                                       " , Parent_Occupation " +
                                       " , Other_Occupation " +
                                       " , Address " +
                                       " , City " +
                                       " , Pincode " +
                                       " , State " +
                                       " , Category_Id " +
                                       " , CasteAndSubCaste " +
                                       " , User_Id " +
                                       " , Residence_Phone_Number " +
                                       " , Mobile_Number " +
                                       " , Mobile_Number2 " +
                                       " , Office_Number " +
                                       " , Neighbour_Number " +
                                       " , Inserted_By_Id" +
                                       " , Updated_By_Id" +
                                       " , Is_Deleted" +
                                       " , Photo_file_Path " +
                                       " , DateOfBirthInText" +
                                       " , Is_Dummy_Admission" +
                                       " , FormNumber" +
                                       " , Optional_Subject_Id" +
                                       " , Mother_Tongue" +
                                       " , LastSchoolName" +
                                       " , LastSchoolAddress" +
                                       " , LastCompletedStd" +
                                       " , LastSchoolUDISENo" +
                                       " , LastCompletedBoard" +
                                       " , IsRecognisedBoard" +
                                       " , AadharCardNo" +
                                       " , NameOnAadharCard" +
                                       " , AadharCard_Photo_Copy_Path" +
                                        " , Family_Photo_Copy_Path" +
                                       " , UDISENumber" +
                                       " , BoardRegistrationNo" +
                                       " , StudentChallanNo" +
                                       " , IsRiseAndShine" +
                                       " , AdmissionSectionId" +
                                       " ,  GRNumber" +
                                       " , StudentUniqueNo" +
                                       " , SaralNo" +
                                       " , IsOnlyChild" +
                                       " , Minority" +
                                       " , PrePrimaryEnrolmentNumber" +
                                       " , CasteCertificate_Photo_Copy_Path" +
                                       ")" +
                              " VALUES " +
                                       " ( " +
                                       "  N'" + moStudentInfo.SchoolId + "'" +
                                       " , N'" + moStudentInfo.iSalutationId + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sFirstName, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMiddleName, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastName, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMother_Name, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sBloodGroup, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sEnrollmentNo, true) + "'" +
                                       " , N'" + moStudentInfo.cSex + "'" +
                                       " , N'" + moStudentInfo.dDob.ToString("yyyy-MM-dd") + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sBirthPlace, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sNationality, true) + "'" +
                                       " , N'" + moStudentInfo.dAdmissionDate.ToString("yyyy-MM-dd") + "'" +
                                       " , N'" + moStudentInfo.dJoining_Date.ToString("yyyy-MM-dd") + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sParentName, true) + "'" +
                                       " , N'" + moStudentInfo.iParentOcupation + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sOtherOcupation, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sAddress, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sCity, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sPincode, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sState, true) + "'" +
                                       " , N'" + moStudentInfo.iCategoryId + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sCasteAndSubCaste, true) + "'" +
                                       " , N'" + moStudentInfo.iUser_Id + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sResidencePhoneNo, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMobilePhoneNo, true) + "'" +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMobilePhoneNo2, true) + "'" +
                                       ",  N'" + moStudentInfo.sOfficeNo + "'" +
                                       ",  N'" + moStudentInfo.sNeighbourNo + "'" +
                                       " , N'" + moStudentInfo.iInsertedById + "'" +
                                       " , N'" + moStudentInfo.iInsertedById + "'" +
                                       " , N'" + Constants.C_NO + "'" +
                                        sFoto +
                                       " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sDateOfBirthInText, true) + "'" +
                                       " , N'" + moStudentInfo.cIs_Dummy_Admission + "'" +
                                       " , N'" + moStudentInfo.sFormNo + "'" +
                                        " , N'" + moStudentInfo.iOptionalSubjectId + "'" +
                                        " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMotherTongue, true) + "'" +
                                        " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolName, true) + "'" +
                                        " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolAddress, true) + "'" +
                                        " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolStandard, true) + "'" +
                                        " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolUDISENo, true) + "'" +
                                        " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolBoardName, true) + "'" +
                                        " , N'" + moStudentInfo.bIsRecognised + "'" +
                                        " , N'" + moStudentInfo.sAadharCardNo + "'" +
                                         " , N'" + moStudentInfo.sNameOnAadharCard + "'" +
                                        " , N'" + moStudentInfo.sAadharCardNumberPhotoCopyName + "'" +
                                         " , N'" + moStudentInfo.sFamilyPhoto + "'" +
                                        " , N'" + moStudentInfo.sUDISENumber + "'" +
                                        " ,N'" + moStudentInfo.sBoardRegNo + "'" +
                                        " , N'" + aiChallanNo + "'" +
                                        " , N'" + moStudentInfo.IsRiseAndShine + "'" +
                                        " , N'" + moStudentInfo.AdmissionForId + "'" +
                                        " , N'" + moStudentInfo.sGRNumber + "'" +
                                        " , N'" + moStudentInfo.sStudentUniqueNo + "'" +
                                        " , N'" + moStudentInfo.SaralNo + "'" +
                                        " , N'" + moStudentInfo.IsOnlyChild + "'" +
                                        " , N'" + moStudentInfo.Minority + "'" +
                                        " , NULL" +
                                        " , N'" + moStudentInfo.sCasteCertPhoto + "'" +
                                       ")  ";

            return sInsertString;
        }

        public string CreateInsertStatementForStudentStreamwiseSubjectDetails(int aiYrStudId)
        {
            string sInsertString;
            if (moStudentInfo.OptSubjectTwo == 0)
            {
                sInsertString = "INSERT INTO " +
                                         "  StudentStreamwiseSubjectDetails (" +
                                         "  SchoolWiseStudentId" +
                                         " , StreamId " +
                                         " , GroupId" +
                                         " , CompulsorySubjects" +
                                         " , OptionalSubjects " +
                                         " , CompitativeExam " +
                                         " , SchoolId " +
                                         " , IsDeleted" +
                                         ")" +
                                " VALUES " +
                                         " ( " +
                                         "  (select top 1 Student_Id from Yearwise_Student_Details where yearwise_Student_Id=" + aiYrStudId + ")" +
                                         " , N'" + moStudentInfo.StreamId + "'" +
                                         " , N'" + moStudentInfo.StreamwiseGroupId + "'" +
                                         " , N'" + moStudentInfo.CompulsorySubject + "'" +
                                         " , N'" + moStudentInfo.OptSubjectOne + "'" +
                                         " , N'" + moStudentInfo.chkCompitativeExams + "'" +
                                         " , N'" + moStudentInfo.SchoolId + "'" +
                                         " , N'" + Constants.I_ZERO + "'" +
                                         ")  ";
            }
            else
            {
                sInsertString = "INSERT INTO " +
                                        "  StudentStreamwiseSubjectDetails (" +
                                        "  SchoolWiseStudentId" +
                                        " , StreamId " +
                                        " , GroupId" +
                                        " , CompulsorySubjects" +
                                        " , OptionalSubjects " +
                                        " , CompitativeExam " +
                                        " , SchoolId " +
                                        " , IsDeleted" +
                                        ")" +
                               " VALUES " +
                                        " ( " +
                                        "  (select top 1 Student_Id from Yearwise_Student_Details where yearwise_Student_Id=" + aiYrStudId + ")" +
                                        " , N'" + moStudentInfo.StreamId + "'" +
                                        " , N'" + moStudentInfo.StreamwiseGroupId + "'" +
                                        " , N'" + moStudentInfo.CompulsorySubject + "'" +
                                        " , N'" + moStudentInfo.OptSubjectOne + "," + moStudentInfo.OptSubjectTwo + "'" +
                                        " , N'" + moStudentInfo.chkCompitativeExams + "'" +
                                        " , N'" + moStudentInfo.SchoolId + "'" +
                                        " , N'" + Constants.I_ZERO + "'" +
                                        ")  ";
            }

            return sInsertString;
        }

        /// <summary>
        /// This method is used to get the max + 1 challan no for add the students details.
        /// </summary>
        /// <returns></returns>
        public int GetNextChallanNo(int aiSchoolId, int aiStudentId)
        {
            int iChallanNo = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetMaxChallanNo");

                if (oSqlDataReader.Read())
                {
                    iChallanNo = oSqlDataReader["MaxChallanNo"].ToInt();
                }
            }
            return iChallanNo;
        }

        /// <summary>
        /// Function to get insert statement for yearwise information details
        /// </summary>
        /// <returns></returns>

        public string CreateinsertStatementForYearWiseDetails()
        {
            string sInsertString = "";

            sInsertString = getinsertStmtForYearWiseDetailsWithRollNo();

            return sInsertString;
        }

        /// <summary>
        /// This function is used to check that for register number duplication.
        /// </summary>
        /// <returns></returns>

        public Boolean isRegisterNoAlreadyPresent()
        {
            Boolean bResult = false;
            string sSqlRollNo = " SELECT Enrolment_Number" +
                                 " FROM " +
                                 " vw_BaseStudentDetails  " +
                                 " WHERE  " +
                                 " Enrolment_Number = N'" + StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sEnrollmentNo, false) + "'" +
                                 " AND SchoolWise_Student_Id !=N'" + moStudentInfo.iStudentId + "'" +
                                 " AND School_Id = N'" + moStudentInfo.SchoolId + "'" +
                                 " AND Is_deleted = N'" + Constants.C_NO + "'" +
                                     " AND Enrolment_Number != N''";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                DataSet oDSStudent = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSqlRollNo);
                if ((oDSStudent != null) && (oDSStudent.Tables[0].Rows.Count > 0) && (oDSStudent.Tables[0].Rows[0][0] != DBNull.Value))
                    bResult = true;
            }
            return bResult;
        }

        public Boolean isGeneralRegisterNoAlreadyPresent()
        {
            Boolean bResult = false;
            string sSqlGRNumber = " SELECT GRNumber" +
                                 " FROM " +
                                 " vw_BaseStudentDetails  " +
                                 " WHERE  " +
                                 " GRNumber = N'" + StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sGRNumber, false) + "'" +
                                 " AND SchoolWise_Student_Id !=N'" + moStudentInfo.iStudentId + "'" +
                                 " AND School_Id = N'" + moStudentInfo.SchoolId + "'" +
                                 " AND Is_deleted = N'" + Constants.C_NO + "'" +
                                     " AND  GRNumber != N''";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                DataSet oDSStudent = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSqlGRNumber);
                if ((oDSStudent != null) && (oDSStudent.Tables[0].Rows.Count > 0) && (oDSStudent.Tables[0].Rows[0][0] != DBNull.Value))
                    bResult = true;
            }
            return bResult;
        }

        public Boolean isStudentUniqueNoAlreadyPresent()
        {
            Boolean bResult = false;
            string sSqlStudentUniqueNo = " SELECT StudentUniqueNo" +
                                 " FROM " +
                                 " vw_BaseStudentDetails  " +
                                 " WHERE  " +
                                 " StudentUniqueNo = N'" + StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sStudentUniqueNo, false) + "'" +
                                 " AND SchoolWise_Student_Id !=N'" + moStudentInfo.iStudentId + "'" +
                                 " AND School_Id = N'" + moStudentInfo.SchoolId + "'" +
                                 " AND Is_deleted = N'" + Constants.C_NO + "'" +
                                     " AND  StudentUniqueNo != N''";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                DataSet oDSStudent = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSqlStudentUniqueNo);
                if ((oDSStudent != null) && (oDSStudent.Tables[0].Rows.Count > 0) && (oDSStudent.Tables[0].Rows[0][0] != DBNull.Value))
                    bResult = true;
            }
            return bResult;
        }

        public static bool CheckIsEnrollmentNumber(string asEnrollmentNo, int aiSchoolId)
        {
            // Here we check is the entered enrollment number is present in this school.
            // If yes return true else return false.
            bool bResult = false;
            string sEnrollnum = " SELECT Count(*)" +
                                 " FROM vw_BaseStudentDetailsForLC " +
                             " WHERE " +
                                 " LCEnrolmentNumber=N'" + asEnrollmentNo + "' " +
                                 " AND School_Id= " + aiSchoolId +
                                 " AND Is_Deleted=N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sEnrollnum);
                if (iCount == 1)
                    bResult = true;
            }
            return bResult;
        }

        /// <summary>
        /// This method is used to get student Id of given academic year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static DataTable GetYearwiseStudentId(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchool_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademic_Year_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudent_Id", aiStudentId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetYearwiseStudentId");
            }
        }

        /// <summary>
        /// This method is used to get student Id of given academic year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static DataTable GetYearwiseStudentDetailsForService(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchool_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademic_Year_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudent_Id", aiStudentId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetYearwiseStudentDetailsForService");
            }
        }

        /// <summary>
        /// This method is used to get the list of prefixes.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static List<string> GetPrefixes(int aiSchoolId, int aiAcademicYearId)
        {
            List<string> olstPrefixes = new List<string>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPrefixes"))
                {
                    if (oSqlReader != null && oSqlReader.HasRows)
                    {
                        while (oSqlReader.Read())
                            olstPrefixes.Add(oSqlReader["Reg_No_Prefix"].ToString());
                    }
                }
                return olstPrefixes;
            }
        }

        /// <summary>
        /// This method is used to get Fee Area Names
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<FeeAreaNamesDetails> GetFeeAreaNames(int aiSchoolId)
        {
            List<FeeAreaNamesDetails> lstFeeAreaNamesDetails = new List<FeeAreaNamesDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFeeAreaNames"))
                {
                    if (oSqlReader != null && oSqlReader.HasRows)
                    {
                        while (oSqlReader.Read())
                        {
                            lstFeeAreaNamesDetails.Add
                               (
                                   new FeeAreaNamesDetails
                                   {
                                       FeeAreaNameId = Convert.ToInt32(oSqlReader["FeeAreaNameId"]),
                                       FeeAreaName = Convert.ToString(oSqlReader["FeeAreaName"])
                                   }
                               );
                        }
                    }
                }
            }
            return lstFeeAreaNamesDetails;
        }

        public static List<string> GetAllRegNoPostfixes(int aiSchoolId, int aiAcademicYearId)
        {
            List<string> lstRegNoPostfixes = new List<string>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllRegNoPostfixes"))
                {
                    if (oSqlReader != null && oSqlReader.HasRows)
                    {
                        while (oSqlReader.Read())
                            lstRegNoPostfixes.Add(oSqlReader["RegNoPostfix"].ToString());
                    }
                }
            }
            return lstRegNoPostfixes;
        }

        public static bool CheckIsStudentLeaveSchool(string asEnrollmentNo, int aiSchoolId)
        {
            // Here we check is the entered enrollment number of student having Is_Leave flag true 
            //i.e.Is that student left school or the leaving certificate of that student is created.
            // If yes return true else return false.
            bool bResult = false;
            string sEnrollnum = " SELECT Count(*)" +
                                   " FROM vw_BaseStudentDetailsForLC " +
                               " WHERE " +
                                   " LCEnrolmentNumber='" + asEnrollmentNo + "' " +
                                   " AND School_Id= " + aiSchoolId +
                                   " AND NOT (SchoolLeft_Date IS NULL)" +
                                   " AND Is_Leave= 'Y'" +
                                   " AND Is_Deleted=N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sEnrollnum);
                if (iCount == 1)
                    bResult = true;
            }
            return bResult;
        }
        /// <summary>
        /// This method is used uo get the mobile numbers for the stuent.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static string GetStudentMobileNumbers(int aiUserId, int aiSchoolId)
        {
            string sMobiles = string.Empty;
            string sQuery = " SELECT (Mobile_Number+" + "','" + "+Mobile_Number2) AS Mobiles" +
                                    " FROM vw_BaseStudentDetails " +
                                " WHERE " +
                                    " User_Id=N'" + aiUserId + "' " +
                                    " AND School_Id= " + aiSchoolId +
                                    " AND Is_Deleted=N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sQuery))
                {
                    if (oSqlDataReader.Read())
                        sMobiles = oSqlDataReader["Mobiles"].ToString();
                }
            }
            return sMobiles;

        }
        /// <summary>
        /// Function to update student information
        /// </summary>
        /// <returns></returns>
        public Int32 UpdateStudent(DateTime adtOldJoiningDate, bool bDeleteFee, out int aiTrackingId)
        {
            string sPhotoUpdate = "";
            string sUpdateCautionMoneyDetails = "";
            string sUpdateJoiningDateStmt = string.Empty;

            aiTrackingId = 0;
            string sUSPName = "Usp_SaveTrackedStudentDetails";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("studentId", moStudentInfo.iStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", moStudentInfo.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moStudentInfo.iInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", 0, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moStudentInfo.iAcademicYearId, SqlDbType.Int);
                DataTable dt = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable(sUSPName);
                aiTrackingId = dt.Rows[0][0].ToInt();
            }

            if (moStudentInfo.sFormNo != string.Empty)
                sUpdateCautionMoneyDetails = " UPDATE SchoolWise_Student_Master SET" +
                                              " SchoolWise_Student_Master.FormNumber=N'" + StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sFormNo, false) + "'" +
                                              ", Updated_By_Id = " + moStudentInfo.iInsertedById +
                                              ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                              " WHERE " +
                                              " Schoolwise_Student_Id= N'" + moStudentInfo.iStudentId + "'";
            else
                sUpdateCautionMoneyDetails = " UPDATE SchoolWise_Student_Master SET" +
                                              " SchoolWise_Student_Master.FormNumber=" + "NULL" +
                                              ", Updated_By_Id = " + moStudentInfo.iInsertedById +
                                              ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                              " WHERE " +
                                              " Schoolwise_Student_Id= N'" + moStudentInfo.iStudentId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateCautionMoneyDetails);

            if (!moStudentInfo.sPhotoFilePath.IsNullOrEmpty())
            {
                sPhotoUpdate = " , Photo_file_Path = " + "  '" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sPhotoFilePath, true) + "'";
            }
            //ArrayList oArrListStmts = new ArrayList();
            //oArrListStmts.Add(
            sUpdateJoiningDateStmt = "UPDATE " +
                                    "  SchoolWise_Student_Master SET" +
                                    "  First_Name = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sFirstName, true) + "'" +
                                    " , Middle_Name = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMiddleName, true) + "' " +
                                    " , Last_Name= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastName, true) + "' " +
                                    " , Mother_Name= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMother_Name, true) + "' " +
                                    " , Blood_Group= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sBloodGroup, true) + "' " +
                                    " , Enrolment_Number= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sEnrollmentNo, true) + "' " +
                                    " , Parent_Name= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sParentName, true) + "' " +
                                    " , Parent_Occupation= N'" + moStudentInfo.iParentOcupation + "' " +
                                    " , Other_Occupation= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sOtherOcupation, true) + "' " +
                                    " , Address= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sAddress, true) + "' " +
                                    " , City= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sCity, true) + "' " +
                                    " , State= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sState, true) + "' " +
                                    " , Pincode= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sPincode, true) + "' " +
                                    " , Residence_Phone_Number= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sResidencePhoneNo, true) + "' " +
                                    " , Mobile_Number= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMobilePhoneNo, true) + "' " +
                                    " , Mobile_Number2= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMobilePhoneNo2, true) + "' " +
                                    " , Office_Number= N'" + moStudentInfo.sOfficeNo + "' " +
                                    " , Neighbour_Number= N'" + moStudentInfo.sNeighbourNo + "' " +
                                    " , Updated_By_id= N'" + moStudentInfo.iInsertedById + "' " +
                                    " , Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                    " , DOB= N'" + moStudentInfo.dDob.ToString("yyyy-MM-dd") + "'" +
                                    " , Birth_Place= N'" + moStudentInfo.sBirthPlace + "'" +
                                    " , Nationality= N'" + moStudentInfo.sNationality + "'" +
                                    " , Sex= N'" + moStudentInfo.cSex + "'" +
                                    " , Salutation_Id= N'" + moStudentInfo.iSalutationId + "'" +
                                    " , Category_Id = N'" + moStudentInfo.iCategoryId + "' " +
                                    " , CasteAndSubCaste = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sCasteAndSubCaste, true) + "' " +
                                    " , Admission_Date= N'" + moStudentInfo.dAdmissionDate.ToString("yyyy-MM-dd") + "' " +
                                    " , Joining_Date= N'" + moStudentInfo.dJoining_Date.ToString("yyy-MM-dd") + "' " +
                                    sPhotoUpdate +
                                    " , DateOfBirthInText = N'" + moStudentInfo.sDateOfBirthInText + "'" +
                                     " , Optional_Subject_Id=N'" + moStudentInfo.iOptionalSubjectId.ToString() + "' " +
                                      " , Mother_Tongue=N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMotherTongue, true) + "' " +
                                      " , LastSchoolName =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolName, true) + "' " +
                                      ", LastSchoolAddress =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolAddress, true) + "' " +
                                    " , LastCompletedStd =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolStandard, true) + "' " +
                                    " , LastSchoolUDISENo =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolUDISENo, true) + "' " +
                                    " , LastCompletedBoard =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolBoardName, true) + "' " +
                                    " ,IsRecognisedBoard =N'" + moStudentInfo.bIsRecognised + "' " +
                                    " ,AadharCardNo =N'" + moStudentInfo.sAadharCardNo + "' " +
                                    " ,NameOnAadharCard =N'" + moStudentInfo.sNameOnAadharCard + "' " +
                                    " ,AadharCard_Photo_Copy_Path =N'" + moStudentInfo.sAadharCardNumberPhotoCopyName + "' " +
                                    " ,Family_Photo_Copy_Path =N'" + moStudentInfo.sFamilyPhoto + "' " +
                                    " ,UDISENumber =N'" + moStudentInfo.sUDISENumber + "' " +
                                    " ,BoardRegistrationNo =N'" + moStudentInfo.sBoardRegNo + "' " +
                                    " ,IsRiseAndShine =N'" + moStudentInfo.IsRiseAndShine + "' " +
                                    " ,AdmissionSectionId =N'" + moStudentInfo.AdmissionForId + "' " +
                                    " ,GRNumber =N'" + moStudentInfo.sGRNumber + "' " +
                                    " ,StudentUniqueNo =N'" + moStudentInfo.sStudentUniqueNo + "' " +
                                    " ,SaralNo=N'" + moStudentInfo.SaralNo + "' " +
                                    " ,IsOnlyChild=N'" + moStudentInfo.IsOnlyChild + "' " +
                                    " ,Minority=N'" + moStudentInfo.Minority + "' " +
                                    " ,CasteCertificate_Photo_Copy_Path =N'" + moStudentInfo.sCasteCertPhoto + "' " +
                                 " WHERE " +
                                   " SchoolWise_Student_Id= " + moStudentInfo.iStudentId + " ; ";
            //'");

            //oArrListStmts.Add(
            sUpdateJoiningDateStmt += " Update " +
                                    " YearWise_Student_Details SET " +
                                        " YearWise_Student_Details.Roll_No = " + moYearWiseStudentInfo.iRollNo.ToString() +
                                        ", Rule_Id = " + moStudentInfo.iRule_Id +
                                        ", IsStaffKid = " + (moStudentInfo.bIsStaffKid ? 1 : 0) +
                                        ", Height = " + moStudentInfo.dHeight +
                                        ", Weight = " + moStudentInfo.dWeight +
                                        ", Updated_By_Id = " + moStudentInfo.iUpdatedById +
                                        ", RTECategoryId = " + moStudentInfo.iRTECategoryId +
                                        ", SecondLanguageSubjectId = " + moStudentInfo.iSecondLanguageSubjectId +
                                        ", ThirdLanguageSubjectId = " + moStudentInfo.iThirdLanguageSubjectId +
                                        ", IsForDayBoarding = N'" + moStudentInfo.IsForDayBoarding + "' " +
                                        ", FeeCategoryDetailsId = " + moStudentInfo.FeeCategoryId +
                                         ", RTEApplicationFormNo = N'" + moStudentInfo.sRTEFormNo + "' " +
                                          ", AnnualIncome = N'" + moStudentInfo.sAnnualIncome + "' " +
                                        ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                    " WHERE " +
                                        " Academic_Year_ID = " + moYearWiseStudentInfo.iYearId.ToString() +
                                        " AND School_Id = " + moYearWiseStudentInfo.iSchoolId.ToString() +
                                        " AND Standard_Id = " + moYearWiseStudentInfo.iStandardId.ToString() +
                                        " AND Division_id = " + moYearWiseStudentInfo.iDivisionId +
                                        " AND Student_Id = " + moYearWiseStudentInfo.iStudentId.ToString() + " ; ";
            sUpdateJoiningDateStmt += " Update " +
                                  " StudentAdditionalDetails SET " +
                                   "  Religion= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sReligion, true) + "' " +
                                   ",   City= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sBusPickupCity, true) + "' " +
                                  " WHERE " +
                                      " SchoolwiseStudentId = " + moYearWiseStudentInfo.iStudentId;
            //);
            int iReturnValue;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iReturnValue = oSQLServerDbUtility.ExecuteTransaction(sUpdateJoiningDateStmt);

            //here we are updating email address field of user master table.
            UpdateEmailAddressOfStudent();

            if (iReturnValue != 0)
            {
                UpdateAttendance(adtOldJoiningDate < moStudentInfo.dJoining_Date, moYearWiseStudentInfo.iYearWIseStudentId);
            }

            // We update the marks of the student only if his joining date has changed.
            if (adtOldJoiningDate.Date != moStudentInfo.dJoining_Date.Date)
                MarkNewStudentsMarks(moYearWiseStudentInfo.iYearWIseStudentId, adtOldJoiningDate);

            if (bDeleteFee)
                DeleteStudentFeeDetails(moYearWiseStudentInfo.iYearWIseStudentId);

            SetOptionalSubjects(moYearWiseStudentInfo.iYearWIseStudentId);

            InsertSwitchLoginDetails(moStudentInfo.SchoolId, moYearWiseStudentInfo.iYearWIseStudentId, moStudentInfo.iParentUserId);

            return iReturnValue;

        }

        public void UpdateStudentTrackingDetails(int aiSchoolId, int aiInsertedById, int aiStudentId, int aiTrackingId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("studentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiTrackingId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("Usp_SaveTrackedStudentDetails");
            }
        }

        /// <summary>
        /// Function to update student streamwise subject details
        /// </summary>
        /// <returns></returns>
        public void UpdateStudentStreamwiseSubjectDetails(int iStudentId)
        {
            int iIsRecordPresent = 0;
            string sStatement = "IF EXISTS(SELECT TOP 1 1 FROM StudentStreamwiseSubjectDetails WHERE SchoolWiseStudentId= " + moStudentInfo.iStudentId + " AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                iIsRecordPresent = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sStatement);
            }

            if (iIsRecordPresent == 1)
            {
                string sStreamUpdate = "";
                if (moStudentInfo.OptSubjectTwo == 0)
                {
                    sStreamUpdate = " UPDATE StudentStreamwiseSubjectDetails SET" +
                                                  " StreamId = '" + moStudentInfo.StreamId + "'" +
                                                  ", GroupId = " + moStudentInfo.StreamwiseGroupId +
                                                  ", CompulsorySubjects = '" + moStudentInfo.CompulsorySubject + "'" +
                                                  ", OptionalSubjects = " + moStudentInfo.OptSubjectOne +
                                                  ", CompitativeExam = '" + moStudentInfo.chkCompitativeExams + "'" +
                                                  " WHERE " +
                                                  " SchoolWiseStudentId= N'" + moStudentInfo.iStudentId + "'";
                }
                else
                {
                    sStreamUpdate = " UPDATE StudentStreamwiseSubjectDetails SET" +
                                                 " StreamId = '" + moStudentInfo.StreamId + "'" +
                                                 ", GroupId = " + moStudentInfo.StreamwiseGroupId +
                                                ", CompulsorySubjects = '" + moStudentInfo.CompulsorySubject + "'" +
                                                 ", OptionalSubjects = '" + moStudentInfo.OptSubjectOne + "," + moStudentInfo.OptSubjectTwo + "'" +
                                                 ", CompitativeExam = '" + moStudentInfo.chkCompitativeExams + "'" +
                                                 " WHERE " +
                                                 " SchoolWiseStudentId= N'" + moStudentInfo.iStudentId + "'";
                }
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sStreamUpdate);
            }
            else
            {
                string sStmt = CreateInsertStatementForStudentStreamwiseSubjectDetails(moYearWiseStudentInfo.iYearWIseStudentId);
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(sStmt);
            }
        }
        /// <summary>
        /// Function to update student information
        /// </summary>
        /// <returns></returns>
        public Int32 UpdateAadharNumber(DateTime adtOldJoiningDate, bool bDeleteFee)
        {
            string sPhotoUpdate = "";
            string sUpdateCautionMoneyDetails = "";
            string sUpdateJoiningDateStmt = string.Empty;
            if (moStudentInfo.sFormNo != string.Empty)
                sUpdateCautionMoneyDetails = " UPDATE SchoolWise_Student_Master SET" +
                                              " SchoolWise_Student_Master.FormNumber=N'" + StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sFormNo, false) + "'" +
                                              ", Updated_By_Id = " + moStudentInfo.iInsertedById +
                                              ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                              " WHERE " +
                                              " Schoolwise_Student_Id= N'" + moStudentInfo.iStudentId + "'";
            else
                sUpdateCautionMoneyDetails = " UPDATE SchoolWise_Student_Master SET" +
                                              " SchoolWise_Student_Master.FormNumber=" + "NULL" +
                                              ", Updated_By_Id = " + moStudentInfo.iInsertedById +
                                              ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                              " WHERE " +
                                              " Schoolwise_Student_Id= N'" + moStudentInfo.iStudentId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateCautionMoneyDetails);

            if (!moStudentInfo.sPhotoFilePath.IsNullOrEmpty())
            {
                sPhotoUpdate = " , Photo_file_Path = " + "  '" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sPhotoFilePath, true) + "'";
            }
            //ArrayList oArrListStmts = new ArrayList();
            //oArrListStmts.Add(
            sUpdateJoiningDateStmt = "UPDATE " +
                                    "  SchoolWise_Student_Master SET" +
                                    "  First_Name = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sFirstName, true) + "'" +
                                    " , Middle_Name = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMiddleName, true) + "' " +
                                    " , Last_Name= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastName, true) + "' " +
                                    " , Mother_Name= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMother_Name, true) + "' " +
                                    " , Blood_Group= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sBloodGroup, true) + "' " +
                                    " , Enrolment_Number= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sEnrollmentNo, true) + "' " +
                                    " , Parent_Name= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sParentName, true) + "' " +
                                    " , Parent_Occupation= N'" + moStudentInfo.iParentOcupation + "' " +
                                    " , Other_Occupation= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sOtherOcupation, true) + "' " +
                                    " , Address= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sAddress, true) + "' " +
                                    " , City= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sCity, true) + "' " +
                                    " , State= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sState, true) + "' " +
                                    " , Pincode= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sPincode, true) + "' " +
                                    " , Residence_Phone_Number= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sResidencePhoneNo, true) + "' " +
                                    " , Mobile_Number= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMobilePhoneNo, true) + "' " +
                                    " , Mobile_Number2= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMobilePhoneNo2, true) + "' " +
                                    " , Office_Number= N'" + moStudentInfo.sOfficeNo + "' " +
                                    " , Neighbour_Number= N'" + moStudentInfo.sNeighbourNo + "' " +
                                    " , Updated_By_id= N'" + moStudentInfo.iInsertedById + "' " +
                                    " , Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                    " , DOB= N'" + moStudentInfo.dDob.ToString("yyyy-MM-dd") + "'" +
                                    " , Birth_Place= N'" + moStudentInfo.sBirthPlace + "'" +
                                    " , Nationality= N'" + moStudentInfo.sNationality + "'" +
                                    " , Sex= N'" + moStudentInfo.cSex + "'" +
                                    " , Salutation_Id= N'" + moStudentInfo.iSalutationId + "'" +
                                    " , Category_Id = N'" + moStudentInfo.iCategoryId + "' " +
                                    " , CasteAndSubCaste = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sCasteAndSubCaste, true) + "' " +
                                    " , Admission_Date= N'" + moStudentInfo.dAdmissionDate.ToString("yyyy-MM-dd") + "' " +
                                    " , Joining_Date= N'" + moStudentInfo.dJoining_Date.ToString("yyy-MM-dd") + "' " +
                                    sPhotoUpdate +
                                    " , DateOfBirthInText = N'" + moStudentInfo.sDateOfBirthInText + "'" +
                                     " , Optional_Subject_Id=N'" + moStudentInfo.iOptionalSubjectId.ToString() + "' " +
                                      " , Mother_Tongue=N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMotherTongue, true) + "' " +
                                      " , LastSchoolName =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolName, true) + "' " +
                                      ", LastSchoolAddress =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolAddress, true) + "' " +
                                    " , LastCompletedStd =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolStandard, true) + "' " +
                                    " , LastSchoolUDISENo =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolUDISENo, true) + "' " +
                                    " , LastCompletedBoard =N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sLastSchoolBoardName, true) + "' " +
                                    " ,IsRecognisedBoard =N'" + moStudentInfo.bIsRecognised + "' " +
                                    " ,AadharCardNo =N'" + moStudentInfo.sAadharCardNo + "' " +
                                    " ,SaralNo =N'" + moStudentInfo.SaralNo+ "' " +
                                    " ,NameOnAadharCard =N'" + moStudentInfo.sNameOnAadharCard + "' " +
                                 " WHERE " +
                                   " SchoolWise_Student_Id= " + moStudentInfo.iStudentId + " ; ";
            //'");

            //oArrListStmts.Add(
            sUpdateJoiningDateStmt += " Update " +
                                    " YearWise_Student_Details SET " +
                                        " YearWise_Student_Details.Roll_No = " + moYearWiseStudentInfo.iRollNo.ToString() +
                                        ", Rule_Id = " + moStudentInfo.iRule_Id +
                                        ", IsStaffKid = " + moStudentInfo.bIsStaffKid +
                                        ", Height = " + moStudentInfo.dHeight +
                                        ", Weight = " + moStudentInfo.dWeight +
                                        ", Updated_By_Id = " + moStudentInfo.iUpdatedById +
                                        ", RTECategoryId = " + moStudentInfo.iRTECategoryId +
                                        ", SecondLanguageSubjectId = " + moStudentInfo.iSecondLanguageSubjectId +
                                        ", ThirdLanguageSubjectId = " + moStudentInfo.iThirdLanguageSubjectId +
                                        ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                    " WHERE " +
                                        " Academic_Year_ID = " + moYearWiseStudentInfo.iYearId.ToString() +
                                        " AND School_Id = " + moYearWiseStudentInfo.iSchoolId.ToString() +
                                        " AND Standard_Id = " + moYearWiseStudentInfo.iStandardId.ToString() +
                                        " AND Division_id = " + moYearWiseStudentInfo.iDivisionId +
                                        " AND Student_Id = " + moYearWiseStudentInfo.iStudentId.ToString();
            //);
            int iReturnValue;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iReturnValue = oSQLServerDbUtility.ExecuteTransaction(sUpdateJoiningDateStmt);

            //here we are updating email address field of user master table.
            UpdateEmailAddressOfStudent();

            if (iReturnValue != 0)
            {
                UpdateAttendance(adtOldJoiningDate < moStudentInfo.dJoining_Date, moYearWiseStudentInfo.iYearWIseStudentId);
            }

            // We update the marks of the student only if his joining date has changed.
            if (adtOldJoiningDate.Date != moStudentInfo.dJoining_Date.Date)
                MarkNewStudentsMarks(moYearWiseStudentInfo.iYearWIseStudentId, adtOldJoiningDate);

            if (bDeleteFee)
                DeleteStudentFeeDetails(moYearWiseStudentInfo.iYearWIseStudentId);

            return iReturnValue;
        }


        private void UpdateAttendance(bool bDeleteAttendance, int iStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moStudentInfo.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYrId", moYearWiseStudentInfo.iYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", iStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("bDeleteAttendance", bDeleteAttendance, SqlDbType.Bit);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_JoiningDateUpdateAttendance");
            }

        }

        /// <summary>
        /// Function to update student Mobile No
        /// </summary>
        /// <returns></returns>
        /// 
        public Int32 UpdateStudentsMobileNo()
        {
            string sUpdateString = "UPDATE " +
                                        "  SchoolWise_Student_Master SET" +
                                        "  Mobile_Number= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMobilePhoneNo, true) + "' " +
                                        " , Mobile_Number2= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sMobilePhoneNo2, true) + "' " +
                                        " , Updated_By_id= '" + moStudentInfo.iInsertedById + "' " +
                                        " , Update_Date = dbo.GetLocalDate(DEFAULT) " +
                                     " WHERE " +
                                       " SchoolWise_Student_Id= '" + moStudentInfo.iStudentId + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }


        /// <summary>
        /// This method is used to return student details.
        /// </summary>
        /// <returns></returns>
        public List<StudentEntities.StudentInfo> GetStudentDetails(int aiSchoolId, int aiAcademicYearId, int aiHomeworkId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HomeworkId", aiHomeworkId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetailsForHomework"))
                {
                    List<StudentEntities.StudentInfo> lstStudents = new List<StudentEntities.StudentInfo>();
                    while (oSqlDataReader.Read())
                    {
                        lstStudents.Add
                            (
                                new StudentEntities.StudentInfo
                                {
                                    UserId = Convert.ToInt32(oSqlDataReader["User_Id"]),
                                    YearwiseStudentId = Convert.ToInt32(oSqlDataReader["Student_Id"]),
                                    MobileNo1 = Convert.ToString(oSqlDataReader["Mobile_Number"]),
                                    MobileNo2 = Convert.ToString(oSqlDataReader["Mobile_Number2"]),
                                    ClassName = Convert.ToString(oSqlDataReader["Standard_Division_Name"])
                                }
                            );
                    }
                    return lstStudents;
                }
            }
        }


        /// <summary>
        /// This method is used to insert student additional details.
        /// </summary>
        /// <param name="iYrwiseStudentId"></param>
        public void AddStudentAdditionalDetails(int aiSchoolId, int aiUserId, StudentAdditionalDetails aoStudentAdditionalDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AdmissionAcadmicYear", aoStudentAdditionalDetails.AdmissionAcademicYear, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AdmissionStandard", aoStudentAdditionalDetails.AdmissionStandard, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("CurrentAcademicYear", aoStudentAdditionalDetails.CurrentAcademicYear, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("CurrentStandard", aoStudentAdditionalDetails.CurrentStandard, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsHandicapped", aoStudentAdditionalDetails.IsHandicapped, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("PreviousMarksObtained", aoStudentAdditionalDetails.PreviousYearMarksObtained, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PreviousMarksOutOff", aoStudentAdditionalDetails.PreviousYearMarksOutOff, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PreviousYearOfPassing", aoStudentAdditionalDetails.PreviousYearOfPassing, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SubjectNames", aoStudentAdditionalDetails.StubjectNames, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aoStudentAdditionalDetails.SchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Userid", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Religion", aoStudentAdditionalDetails.Religion, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BirthTaluka", aoStudentAdditionalDetails.BirthTaluka, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("BirthDistrict", aoStudentAdditionalDetails.BirthDistrict, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("HouseNoPlotNo", aoStudentAdditionalDetails.HouseNoPlotNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MainArea", aoStudentAdditionalDetails.MainArea, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SubareaName", aoStudentAdditionalDetails.SubareaName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Landmark", aoStudentAdditionalDetails.Landmark, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Taluka", aoStudentAdditionalDetails.Taluka, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("District", aoStudentAdditionalDetails.District, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FeeAreaName", aoStudentAdditionalDetails.FeeAreaName, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FatherOccupation", aoStudentAdditionalDetails.FatherOccupation, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherQualification", aoStudentAdditionalDetails.FatherQualification, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherEmail", aoStudentAdditionalDetails.FatherEmail, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherOfficeName", aoStudentAdditionalDetails.FatherOfficeName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherOfficeAddress", aoStudentAdditionalDetails.FatherOfficeAddress, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherOccupation", aoStudentAdditionalDetails.MotherOccupation, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherQualification", aoStudentAdditionalDetails.MotherQualification, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherEmail", aoStudentAdditionalDetails.MotherEmail, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherOfficeName", aoStudentAdditionalDetails.MotherOfficeName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherOfficeAddress", aoStudentAdditionalDetails.MotherOfficeAddress, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherDOB", aoStudentAdditionalDetails.FatherDOB, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("MotherDOB", aoStudentAdditionalDetails.MotherDOB, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("FatherDesignation", aoStudentAdditionalDetails.FatherDesignation, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherDesignation", aoStudentAdditionalDetails.MotherDesignation, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherPhoto", aoStudentAdditionalDetails.FatherPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherPhoto", aoStudentAdditionalDetails.MotherPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AnniversaryDate", aoStudentAdditionalDetails.MarriageAnniversaryDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("LocalGuardianPhoto", aoStudentAdditionalDetails.GuardianPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RelativeName", aoStudentAdditionalDetails.RelativeName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherBinaryPhoto", aoStudentAdditionalDetails.FatherBinaryPhoto, SqlDbType.Image);
                oSQLServerDbUtility.AddParameter("MotherBinaryPhoto", aoStudentAdditionalDetails.MotherBinaryPhoto, SqlDbType.Image);
                oSQLServerDbUtility.AddParameter("RelativeBinaryPhoto", aoStudentAdditionalDetails.ParentBinaryPhoto, SqlDbType.Image);
                oSQLServerDbUtility.AddParameter("FatherWeight", aoStudentAdditionalDetails.FatherWeight, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MotherWeight", aoStudentAdditionalDetails.MotherWeight, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FatherHeight", aoStudentAdditionalDetails.FatherHeight, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MotherHeight", aoStudentAdditionalDetails.MotherHeight, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FatherAdharcardNo", aoStudentAdditionalDetails.FatherAadharCardNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherAadharCardNo", aoStudentAdditionalDetails.MotherAadharCardNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherBloodGroup", aoStudentAdditionalDetails.FatherBloodGroup, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherBloodGroup", aoStudentAdditionalDetails.MotherBloodGroup, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FamilyMonthlyIncome", aoStudentAdditionalDetails.FamilyMonthlyIncome, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("CWSN", aoStudentAdditionalDetails.CWSN, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherAnnualIncome", aoStudentAdditionalDetails.FatherAnnualIncome, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("MotherAnnualIncome", aoStudentAdditionalDetails.MotherAnnualIncome, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("BirthState", aoStudentAdditionalDetails.BirthState, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Name1", aoStudentAdditionalDetails.Name1, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Age1", aoStudentAdditionalDetails.Age1, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Institute1", aoStudentAdditionalDetails.Institute1, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Standard1", aoStudentAdditionalDetails.Standard1, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Name2", aoStudentAdditionalDetails.Name2, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Age2", aoStudentAdditionalDetails.Age2, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Institute2", aoStudentAdditionalDetails.Institute2, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Standard2", aoStudentAdditionalDetails.Standard2, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ResidenceType", aoStudentAdditionalDetails.ResisdenceTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RFID", aoStudentAdditionalDetails.RFID, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PenNo", aoStudentAdditionalDetails.PenNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ApaarId", aoStudentAdditionalDetails.ApaarId, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FatherAadharCardScanCopy", aoStudentAdditionalDetails.FatherAadharCardPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherAadharCardScanCopy", aoStudentAdditionalDetails.MotherAadharCardPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AddStudentAdditionalDetails");
            }
        }

        /// <summary>
        /// This method is used to delete day boarding related students fee.
        /// </summary>
        /// <param name="aischoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiYearwiseStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteDayBoardingFees(int aischoolId, int aiAcademicYearId, int aiYearwiseStudentId, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aischoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteDayBoardingFeeOfStudent");
            }
        }

        /// <summary>
        /// This method is used to get student additional details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public StudentAdditionalDetails GetStudentAdditionalDetails(int aiSchoolId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentAdditionalDetails"))
                {
                    SchoolEntities.StudentAdditionalDetails oStudentAdditionalDetials = new SchoolEntities.StudentAdditionalDetails();
                    if (oSqlDataReader.Read())
                    {
                        oStudentAdditionalDetials.StudentStatus = Convert.ToString(Constants.StudentAdditionalStatus.Admission);
                        oStudentAdditionalDetials.AdmissionAcademicYear = Convert.ToString(oSqlDataReader["AddmissionAcademicYear"]);
                        oStudentAdditionalDetials.AdmissionStandard = Convert.ToString(oSqlDataReader["AddmissionStandard"]);
                        oStudentAdditionalDetials.CurrentAcademicYear = Convert.ToString(oSqlDataReader["CurrentAcademicYear"]);
                        oStudentAdditionalDetials.CurrentStandard = Convert.ToString(oSqlDataReader["CurrentStandard"]);
                        oStudentAdditionalDetials.IsHandicapped = Convert.ToBoolean(oSqlDataReader["IsHandicapped"]);
                        oStudentAdditionalDetials.PreviousYearMarksObtained = Convert.ToInt32(oSqlDataReader["PreviousMarksObtained"]);
                        oStudentAdditionalDetials.PreviousYearMarksOutOff = Convert.ToInt32(oSqlDataReader["PreviousMarksOutOff"]);
                        oStudentAdditionalDetials.PreviousYearOfPassing = Convert.ToString(oSqlDataReader["PreviousYearOfPassing"]);
                        oStudentAdditionalDetials.StubjectNames = Convert.ToString(oSqlDataReader["SubjectNames"]);
                        oStudentAdditionalDetials.Religion = Convert.ToString(oSqlDataReader["Religion"]);
                        oStudentAdditionalDetials.BirthTaluka = Convert.ToString(oSqlDataReader["BirthTaluka"]);
                        oStudentAdditionalDetials.BirthDistrict = Convert.ToString(oSqlDataReader["BirthDistrict"]);
                        oStudentAdditionalDetials.HouseNoPlotNo = Convert.ToString(oSqlDataReader["HouseNoPlotNo"]);
                        oStudentAdditionalDetials.MainArea = Convert.ToString(oSqlDataReader["MainArea"]);
                        oStudentAdditionalDetials.SubareaName = Convert.ToString(oSqlDataReader["SubareaName"]);
                        oStudentAdditionalDetials.Landmark = Convert.ToString(oSqlDataReader["Landmark"]);
                        oStudentAdditionalDetials.Taluka = Convert.ToString(oSqlDataReader["Taluka"]);
                        oStudentAdditionalDetials.District = Convert.ToString(oSqlDataReader["District"]);
                        oStudentAdditionalDetials.FeeAreaName = Convert.ToInt32(oSqlDataReader["FeeAreaName"]);
                        oStudentAdditionalDetials.FatherOccupation = Convert.ToString(oSqlDataReader["FatherOccupation"]);
                        oStudentAdditionalDetials.FatherQualification = Convert.ToString(oSqlDataReader["FatherQualification"]);
                        oStudentAdditionalDetials.FatherEmail = Convert.ToString(oSqlDataReader["FatherEmail"]);
                        oStudentAdditionalDetials.FatherOfficeName = Convert.ToString(oSqlDataReader["FatherOfficeName"]);
                        oStudentAdditionalDetials.FatherOfficeAddress = Convert.ToString(oSqlDataReader["FatherOfficeAddress"]);
                        oStudentAdditionalDetials.MotherOccupation = Convert.ToString(oSqlDataReader["MotherOccupation"]);
                        oStudentAdditionalDetials.MotherQualification = Convert.ToString(oSqlDataReader["MotherQualification"]);
                        oStudentAdditionalDetials.MotherEmail = Convert.ToString(oSqlDataReader["MotherEmail"]);
                        oStudentAdditionalDetials.MotherOfficeName = Convert.ToString(oSqlDataReader["MotherOfficeName"]);
                        oStudentAdditionalDetials.MotherOfficeAddress = Convert.ToString(oSqlDataReader["MotherOfficeAddress"]);
                        oStudentAdditionalDetials.FatherDOB = Convert.ToDateTime(oSqlDataReader["FatherDOB"]);
                        oStudentAdditionalDetials.MotherDOB = Convert.ToDateTime(oSqlDataReader["MotherDOB"]);
                        oStudentAdditionalDetials.FatherDesignation = Convert.ToString(oSqlDataReader["FatherDesignation"]);
                        oStudentAdditionalDetials.MotherDesignation = Convert.ToString(oSqlDataReader["MotherDesignation"]);
                        oStudentAdditionalDetials.FatherPhoto = Convert.ToString(oSqlDataReader["FatherPhoto"]);
                        oStudentAdditionalDetials.MotherPhoto = Convert.ToString(oSqlDataReader["MotherPhoto"]);
                        oStudentAdditionalDetials.MarriageAnniversaryDate = Convert.ToDateTime(oSqlDataReader["AnniversaryDate"]);
                        oStudentAdditionalDetials.GuardianPhoto = Convert.ToString(oSqlDataReader["GuardianPhoto"]);
                        oStudentAdditionalDetials.RelativeName = Convert.ToString(oSqlDataReader["RelativeName"]);
                        oStudentAdditionalDetials.FatherWeight = Convert.ToInt32(oSqlDataReader["FatherWeight"]);
                        oStudentAdditionalDetials.MotherWeight = Convert.ToInt32(oSqlDataReader["MotherWeight"]);
                        oStudentAdditionalDetials.FatherHeight = Convert.ToInt32(oSqlDataReader["FatherHeight"]);
                        oStudentAdditionalDetials.MotherHeight = Convert.ToInt32(oSqlDataReader["MotherHeight"]);
                        oStudentAdditionalDetials.FatherAadharCardNo = Convert.ToString(oSqlDataReader["FatherAadharcardNo"]);
                        oStudentAdditionalDetials.MotherAadharCardNo = Convert.ToString(oSqlDataReader["MotherAadharcardNo"]);
                        oStudentAdditionalDetials.FatherBloodGroup = Convert.ToString(oSqlDataReader["FatherBloodGroup"]);
                        oStudentAdditionalDetials.MotherBloodGroup = Convert.ToString(oSqlDataReader["MotherBloodGroup"]);
                        oStudentAdditionalDetials.FamilyMonthlyIncome = Convert.ToDecimal(oSqlDataReader["FamilyMonthlyIncome"]);
                        oStudentAdditionalDetials.CWSN = Convert.ToString(oSqlDataReader["CWSN"]);

                        if (oSqlDataReader["FatherAnnualIncome"] != DBNull.Value)
                            oStudentAdditionalDetials.FatherAnnualIncome = Convert.ToDecimal(oSqlDataReader["FatherAnnualIncome"]);
                        if (oSqlDataReader["MotherAnnualIncome"] != DBNull.Value)
                            oStudentAdditionalDetials.MotherAnnualIncome = Convert.ToDecimal(oSqlDataReader["MotherAnnualIncome"]);
                        oStudentAdditionalDetials.BirthState = Convert.ToString(oSqlDataReader["BirthState"]);
                        oStudentAdditionalDetials.Name1 = Convert.ToString(oSqlDataReader["Name1"]);
                        oStudentAdditionalDetials.Institute1 = Convert.ToString(oSqlDataReader["Institution1"]);
                        oStudentAdditionalDetials.Standard1 = Convert.ToString(oSqlDataReader["StandardName1"]);
                        oStudentAdditionalDetials.Name2 = Convert.ToString(oSqlDataReader["Name2"]);
                        oStudentAdditionalDetials.Institute2 = Convert.ToString(oSqlDataReader["Institution2"]);
                        oStudentAdditionalDetials.Standard2 = Convert.ToString(oSqlDataReader["StandardName2"]);
                        oStudentAdditionalDetials.Age1 = Convert.ToInt32(oSqlDataReader["Age1"]);
                        oStudentAdditionalDetials.Age2 = Convert.ToInt32(oSqlDataReader["Age2"]);
                        oStudentAdditionalDetials.ResisdenceTypeId = Convert.ToInt32(oSqlDataReader["ResidenceType"]);
                        oStudentAdditionalDetials.RFID = Convert.ToString(oSqlDataReader["RFID"]);
                        oStudentAdditionalDetials.PenNo = Convert.ToString(oSqlDataReader["PenNo"]);
                        oStudentAdditionalDetials.ApaarId = Convert.ToString(oSqlDataReader["ApaarId"]);
                        oStudentAdditionalDetials.BirthCertificateFileName = Convert.ToString(oSqlDataReader["BirthCertificateScanCopyFileName"]);
                        oStudentAdditionalDetials.FatherAadharCardPhoto = Convert.ToString(oSqlDataReader["FatherAadharCardScanCopy"]);
                        oStudentAdditionalDetials.MotherAadharCardPhoto = Convert.ToString(oSqlDataReader["MotherAadharCardScanCopy"]);
                    }
                    return oStudentAdditionalDetials;
                }
            }
        }

        /// <summary>
        /// Function to update email adress.
        /// </summary>
        /// <returns></returns>
        /// 
        public int UpdateEmailAddressOfStudent()
        {
            string sUpdateString = string.Empty;
            if ((moStudentInfo.SchoolId == Constants.SchoolId.SVNP.ToInt()) && (moStudentInfo.sEnrollmentNo != moStudentInfo.sOldEnolmentNo))
            {
                string sEnrolmentNo = Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sEnrollmentNo, true);
                string sOldPass = CommonUtility.GetDecryptedPassword(moStudentInfo.sLoginName.ToString(), moStudentInfo.sPassword.ToString());
                string sNewPass = Utility.CommonUtility.GetEncryptedPassword(sEnrolmentNo, sOldPass);

                sUpdateString = "  UPDATE " +
                                "  User_Master SET" +
                                "  Email_Address= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sEmail, true) + "' " +
                                ", User_Login = N'" + sEnrolmentNo + "' " +
                                ", User_Password = N'" + sNewPass + "' " +
                                ", Updated_By_Id =" + moStudentInfo.iUpdatedById +
                                ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                "  WHERE " +
                                "  User_Id= '" + moStudentInfo.iUser_Id + "'";
            }
            else
            {
                sUpdateString = "  UPDATE " +
                                "  User_Master SET" +
                                "  Email_Address= N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sEmail, true) + "' " +
                                ", Updated_By_Id =" + moStudentInfo.iUpdatedById +
                                ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                "  WHERE " +
                                "  User_Id= '" + moStudentInfo.iUser_Id + "'";
            }
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }

        /// <summary>
        /// Used in RetriveStudentInfo
        /// </summary>
        /// <returns></returns>
        private string GetCommonSelectStatementToFetchData()
        {
            return "SELECT " +
                         " SchoolWise_Student_Id " +
                         " , User_Login " +
                         " , First_Name " +
                         " , Middle_Name " +
                         " , Last_Name " +
                         " , Mother_Name " +
                         " , Blood_Group " +
                         " , DOB " +
                          " , Birth_Place " +
                            " , Nationality " +
                         " , Enrolment_Number " +
                         ", Parent_Name " +
                         ", Address " +
                         ", Sex " +
                         ", Admission_date " +
                         ", Joining_Date " +
                         ", Enrolment_Number " +
                         ", Parent_Occupation " +
                         ", Other_Occupation " +
                         ", City " +
                         ", Pincode " +
                         " ,Roll_No " +
                         " ,Standard_Id " +
                         " ,Standard_Division_Name " +
                         ", State " +
                         ", Residence_Phone_Number " +
                         ", Mobile_Number " +
                         ", Mobile_Number2 " +
                         ", Office_Number " +
                         ", Neighbour_Number " +
                         ", Category_Id " +
                         ",Category_Name" +
                         ", CasteAndSubCaste " +
                         ", Student_Id " +
                         ", Is_Leave " +
                         ", Salutation_Name " +
                          " , Photo_File_Path " +
                           ",User_Id" +
                           " ,Standard_Name " +
                           " ,SchoolLeft_Date " +
                           " ,Is_New_Student " +
                           " ,Is_RTE_Student " +
                            " ,RTECategoryId " +
                           " ,Rule_Id " +
                           " ,IsStaffKid " +
                            " ,Optional_Subject_Id" +
                            " ,Height" +
                            " ,Weight" +
                            " , Mother_Tongue" +
                            " , LastSchoolName" +
                            " , LastSchoolAddress " +
                            " , LastCompletedStd" +
                            " , LastSchoolUDISENo" +
                            " , LastCompletedBoard" +
                            ", IsRecognisedBoard" +
                            ", Division_id" +
                            ", AadharCardNo" +
                            ", NameOnAadharCard" +
                            ", UDISENumber" +
                            ", BoardRegistrationNo" +
                            ",SecondLanguageSubjectId" +
                            ",ThirdLanguageSubjectId" +
                            ",CancellationFormNo" +
                            ",Religion" +
                            ",IsRiseAndShine" +
                            ", AdmissionSectionId AS AdmissionForId" +
                            ", ' ' AS User_Password" +
                            ",Family_Photo_Copy_Path" +
                             ",GRNumber" +
                              ",StudentUniqueNo" +
                              ",[dbo].[udf_GetConfirmedByName](Student_Id) AS ConfirmedByText" +
                             ",[dbo].[udf_GetUpdatedByText](Student_Id) AS UpdatedByText" +
                              ", IsForDayBoarding" +
                              ", [dbo].[udf_CheckIsDayBoardingFeeIsPaid](Student_Id,School_Id,Academic_Year_ID) AS IsDayBoardingFeePaid" +
                              ", FeeCategoryDetailsId " +
                              ", SaralNo " +
                              ", IsOnlyChild " +
                              ", Minority " +
                               ", RTEApplicationFormNo " +
                               ", Name1" +
                               ", Age1" +
                               ", Institution1" +
                               ", StandardName1" +
                               ", Name2" +
                               ", Age2" +
                               ", Institution2" +
                               ", StandardName2" +
                               ", ResidenceTypeId" +
                               ", ResidenceName" +
                               ",'' as AdmissionStandard" +    //////////////////
                               ",RFID" +
                               ",BusPickupCity" +
                " FROM vw_GetAllStudentsForStandardDivision " +
                " WHERE Is_Deleted= N'" + Constants.C_NO + "' ";
        }

        /// <summary>
        /// Resolves conflict in pending fee screen by calling the stored procedure.
        /// </summary>
        /// <param name="aiSchoolId">School ID</param>
        /// <param name="aiAcademicYearId">Academic Year ID</param>
        /// <param name="aiUpdatedById">Updated By User ID</param>
        /// <returns>TransactionResult indicating success or failure with error message</returns>
        public TransactionResult ResolveConflict(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            TransactionResult oResult = new TransactionResult();
            try
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                    SqlParameter oWasIssuePresent = oSQLServerDbUtility.AddParameter("WasIssuePresent", false, SqlDbType.Bit, ParameterDirection.Output);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ResolvePendingFeeScreenConflict");

                    oResult.IsSuccess = true;
                    if (!oWasIssuePresent.Value.ToBool())
                        oResult.Message = "No any conflict found.";
                    else
                        oResult.Message = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorLogDC.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
                oResult.IsSuccess = false;
                oResult.Message = "An error occurred while resolving conflict.";
            }
            return oResult;
        }

        private DataTable RetriveStudentInfo(Int32 aiYearwiseStudentId)
        {
            // Student_Id is the column Yearwise_student_id.
            string sSelectStament = GetCommonSelectStatementToFetchData() +
                                    " AND Student_Id= N'" + aiYearwiseStudentId + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStament);
        }

        /// <summary>
        /// This function gets the record set for a perticular student
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>

        private DataTable RetriveStudentInfo(Int32 aiSchoolId, Int32 aiStudentId)
        {
            string sSelectStament = GetCommonSelectStatementToFetchData() +
                                    " AND SchoolWise_Student_Id= N'" + aiStudentId + "'" +
                                    " AND School_Id= N'" + aiSchoolId + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStament);
        }

        /// <summary>
        /// This function gets the record set for a perticular student
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>

        private DataTable RetriveStudentInfo(Int32 aiSchoolId, Int32 aiStudentId, bool abIsYrwise)
        {
            string sFilter = "";
            if (abIsYrwise)
                sFilter = " AND Student_Id= N'" + aiStudentId + "'";
            else
                sFilter = " AND SchoolWise_Student_Id= N'" + aiStudentId + "'";

            string sSelectStament = GetCommonSelectStatementToFetchData() +
                                    " AND School_Id= N'" + aiSchoolId + "'" +
                                     sFilter;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStament);
        }

        /// <summary>
        /// This function gets the record set for a perticular student
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>

        public DataTable RetriveStudentInfo(Int32 aiSchoolId, Int32 aiAccYearId, Int32 aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAccYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolWise_Student_Id", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSingleStudentDetails");
            }
        }

        public DataTable RetriveMidYearInfo(Int32 aiSchoolId, Int32 aiAccYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAccYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetIsMidYear");
            }
        }

        /// <summary>
        /// This function is used to delete family photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>

        public void DeleteFamilyPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            string sUpdateString = "UPDATE " +
                                        "  SchoolWise_Student_Master SET" +
                                        "  Family_Photo_Copy_Path = N'" + string.Empty + "' " +
                                        ", Updated_By_Id =" + aiUpdatedById +
                                        ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                       "   WHERE " +
                                       " SchoolWise_Student_Id = '" + aiStudentId + "'" +
                                       "AND School_Id = '" + aiSchoolId + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }

        // <summary>
        /// This function is used to delete Father photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteFatherPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            string sUpdateString = "UPDATE " +
                                   " StudentAdditionalDetails SET" +
                                   " FatherPhoto = N'" + string.Empty + "' " +
                                   ", UpdatedById = " + aiUpdatedById +
                                   ", UpdateDate = dbo.GetLocalDate(DEFAULT)" +
                                   "   WHERE " +
                                   "SchoolwiseStudentId = '" + aiStudentId + "' " +
                                   "AND SchoolId = '" + aiSchoolId + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }

        // <summary>
        /// This function is used to delete Mother photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteMotherPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            string sUpdateString = "UPDATE " +
                                   " StudentAdditionalDetails SET" +
                                   " MotherPhoto = N'" + string.Empty + "' " +
                                   ", UpdatedById = " + aiUpdatedById +
                                   ", UpdateDate = dbo.GetLocalDate(DEFAULT)" +
                                   "   WHERE " +
                                   "SchoolwiseStudentId = '" + aiStudentId + "' " +
                                   "AND SchoolId = '" + aiSchoolId + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }

        // <summary>
        /// This function is used to delete Guardian photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteGuardianPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            string sUpdateString = "UPDATE " +
                                   " StudentAdditionalDetails SET" +
                                   " GuardianPhoto = N'" + string.Empty + "' " +
                                   ", UpdatedById = " + aiUpdatedById +
                                   ", UpdateDate = dbo.GetLocalDate(DEFAULT)" +
                                   "   WHERE " +
                                   "SchoolwiseStudentId = '" + aiStudentId + "' " +
                                   "AND SchoolId = '" + aiSchoolId + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }

        /// <summary>
        /// This function is used to delete Caste Certificate photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
     public void DeleteCasteCertificatePhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            string sUpdateString = "UPDATE " +
                                        "  SchoolWise_Student_Master SET" +
                                        "  CasteCertificate_Photo_Copy_Path = N'" + string.Empty + "' " +
                                        ", Updated_By_Id =" + aiUpdatedById +
                                        ", Update_Date = dbo.GetLocalDate(DEFAULT)" +
                                       "   WHERE " +
                                       " SchoolWise_Student_Id = '" + aiStudentId + "'" +
                                       "AND School_Id = '" + aiSchoolId + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }

        // <summary>
        /// This function is used to delete Mother Aadhar card photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteMotherAadharPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            string sUpdateString = "UPDATE " +
                                   " StudentAdditionalDetails SET" +
                                   " MotherAadharCardScanCopy = N'" + string.Empty + "' " +
                                   ", UpdatedById = " + aiUpdatedById +
                                   ", UpdateDate = dbo.GetLocalDate(DEFAULT)" +
                                   "   WHERE " +
                                   "SchoolwiseStudentId = '" + aiStudentId + "' " +
                                   "AND SchoolId = '" + aiSchoolId + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }

        // <summary>
        /// This function is used to delete Father Aadhar card photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteFatherAadharPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            string sUpdateString = "UPDATE " +
                                   " StudentAdditionalDetails SET" +
                                   " FatherAadharCardScanCopy = N'" + string.Empty + "' " +
                                   ", UpdatedById = " + aiUpdatedById +
                                   ", UpdateDate = dbo.GetLocalDate(DEFAULT)" +
                                   "   WHERE " +
                                   "SchoolwiseStudentId = '" + aiStudentId + "' " +
                                   "AND SchoolId = '" + aiSchoolId + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }
        /// <summary>
        /// This method is used to activate all student logins.
        /// </summary>
        public static void ActivateStudentLogins(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ActivateAllStudentLogins");
            }
        }

        /// <summary>
        /// This function gets the record set for a perticular student
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>

        private DataTable RetriveStudentInfo(Int32 aiSchoolId, string asRegNo, Int32 aiUserId)
        {
            string sSelectStament = GetCommonSelectStatementToFetchData() +
                                    " AND User_Id = '" + aiUserId + "'" +
                                    " AND Enrolment_Number = '" + StringUtility.ReplaceSingleQuoteInString(asRegNo, false) + "'" +
                                    " AND School_Id= '" + aiSchoolId + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStament);

        }

        private DataTable RetriveStudentInfo(Int32 aiSchoolId, int aiAcademicYrId, string asRegNo)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iReg_No", asRegNo, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_StudentByRegNo");
            }
        }

        /// <summary>
        /// This method used to get count of student having blank register number.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public static DataTable GetBlankRegNoCount(Int32 aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iDivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NameRegNo", StringUtility.ReplaceSingleQuoteInString(asName, true), SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetBlankRegNoCount");
            }
        }

        /// <summary>
        /// This method gives student registration number records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asEnrolmentNumber"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="abIsStudBlankRegNo"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public static DataTable GetStudentsWithEnrolmentNumber(Int32 aiSchoolId, int aiAcademicYearId, string asEnrolmentNumber, Int32 aiStandardId, Int32 aiDivisionId, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix)
        {
            string sFilter = CreateRegNoReassignFilter(aiStandardId, aiDivisionId, asEnrolmentNumber, abIsStudBlankRegNo, asRegNo, abIsExact, asOperator, asPrefix);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                //oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                //oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedStudentsRegNo");
            }
        }

        public DataSet GetStudentPhoto(int aiSchoolId, int aiAcademicYrId, string asStandardId, string asDivisionId, string asName, string asRegNo, int aIsExactMatch, string Operator, string Prefix)
        {
            string sFilter = CreateFilter(Convert.ToInt32(asStandardId), Convert.ToInt32(asDivisionId), asName, true, asRegNo, (asName == string.Empty && asRegNo != string.Empty), Operator, Prefix);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("USP_GetStudentWithPhoto");
            }
        }

        /// <summary>
        /// This event total count of student as per filter condition.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asEnrolmentNumber"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="abIsStudBlankRegNo"></param>
        /// <returns></returns>
        //public static int GetCountStudents(Int32 aiSchoolId, Int32 aiAcademicYearId, String asEnrolmentNumber, Int32 aiStandardId, Int32 aiDivisionId, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact)
        //{
        //    string sFilter = CreateRegNoReassignFilter(aiStandardId, aiDivisionId, asEnrolmentNumber, abIsStudBlankRegNo, asRegNo, abIsExact);

        //    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //    {
        //        oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("iAcademicYearId", aiAcademicYearId, SqlDbType.Int);
        //        oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
        //        SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);

        //        oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentsRegNoCount");
        //        return Convert.ToInt32(oSqlParameter.Value);
        //    }

        //}

        /// <summary>
        /// This methhod used to form filter condition.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asEnrolmentNumber"></param>
        /// <param name="abIsStudBlankRegNo"></param>
        /// <returns></returns>
        private static string CreateRegNoReassignFilter(int aiStandardId, int aiDivisionId, string asEnrolmentNumber, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix)
        {
            if (!abIsExact || asRegNo.IsNullOrEmpty())
                msOperator = string.Empty;
            else
            {
                List<String> operators = GetOperators().Where(opr => opr.Value.ToString() == asOperator).Select(opr => opr.Text).ToList();
                msOperator = operators.Count > Constants.I_ZERO ? operators.First() : string.Empty;
            }

            if (abIsExact == true && asRegNo.IsNullOrEmpty())
                abIsExact = false;

            string sFilter = "";
            if (aiStandardId != 0)
                sFilter = " AND vw_GetAllStudentsForStandardDivision.[Standard_Id] =+ CAST(" + aiStandardId.ToString() + " AS VARCHAR(15))";

            if (aiDivisionId != 0)
                sFilter = sFilter + " AND vw_GetAllStudentsForStandardDivision.[Division_id] =+ CAST(" + aiDivisionId.ToString() + " AS VARCHAR(15))";

            if (!String.IsNullOrEmpty(asEnrolmentNumber))
            {
                string sName = Utility.StringUtility.ReplaceSingleQuoteInString(asEnrolmentNumber, true);
                sFilter = sFilter + " AND (vw_GetAllStudentsForStandardDivision.Name LIKE '%" + sName + "%' OR vw_GetAllStudentsForStandardDivision.Enrolment_Number LIKE '%" + sName + "%' OR vw_GetAllStudentsForStandardDivision.Enrolment_Number + ' - ' + Name LIKE N'%" + sName + "%' )";
            }
            else if (!String.IsNullOrEmpty(asRegNo) && abIsExact)
            //sFilter = sFilter + " AND vw_GetAllStudentsForStandardDivision.Enrolment_Number  IN (SELECT Name FROM udf_GetTableFromStringList('" + StringUtility.ReplaceSingleQuoteInString(asRegNo.Trim(), true) + "'))";
            {
                if (!asPrefix.IsNullOrEmpty())
                {
                    if (asPrefix == Constants.S_ALL)
                        asPrefix = string.Empty;

                    sFilter = sFilter + " AND Enrolment_Number LIKE '%" + asPrefix + "%' " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
                }
                else
                    sFilter = sFilter + " AND #tblStudents.HasPrefix=0 " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
            }
            else if (abIsExact)
                sFilter = sFilter + " AND Enrolment_Number =''";

            if (abIsStudBlankRegNo)
                sFilter = sFilter + " AND ( vw_GetAllStudentsForStandardDivision.Enrolment_Number LIKE '')";

            return sFilter;
        }
        /// <summary>
        /// This function populates the student basic details' structure  with values in table
        /// </summary>
        /// <param name="aDoTable"></param>

        private void FillStruct(DataTable aDoTable)
        {
            if (aDoTable.Rows[0]["SchoolWise_Student_Id"] != DBNull.Value)
                moStudentInfo.iStudentId = Convert.ToInt32(aDoTable.Rows[0]["SchoolWise_Student_Id"]);
            if (aDoTable.Rows[0]["User_Login"] != DBNull.Value)
                moStudentInfo.sLoginName = aDoTable.Rows[0]["User_Login"].ToString();
            if (aDoTable.Rows[0]["Enrolment_Number"] != DBNull.Value)
            {
                moStudentInfo.sEnrollmentNo = aDoTable.Rows[0]["Enrolment_Number"].ToString();
                moStudentInfo.sOldEnolmentNo = aDoTable.Rows[0]["Enrolment_Number"].ToString();
            }
            if (aDoTable.Rows[0]["First_Name"] != DBNull.Value)
                moStudentInfo.sFirstName = aDoTable.Rows[0]["First_Name"].ToString();
            if (aDoTable.Rows[0]["Middle_Name"] != DBNull.Value)
                moStudentInfo.sMiddleName = aDoTable.Rows[0]["Middle_Name"].ToString();
            if (aDoTable.Rows[0]["Last_Name"] != DBNull.Value)
                moStudentInfo.sLastName = aDoTable.Rows[0]["Last_Name"].ToString();
            if (aDoTable.Rows[0]["Mother_Name"] != DBNull.Value)
                moStudentInfo.sMother_Name = aDoTable.Rows[0]["Mother_Name"].ToString();
            if (aDoTable.Rows[0]["Blood_Group"] != DBNull.Value)
                moStudentInfo.sBloodGroup = aDoTable.Rows[0]["Blood_Group"].ToString();
            if (aDoTable.Rows[0]["DOB"] != DBNull.Value)
                moStudentInfo.dDob = Convert.ToDateTime(aDoTable.Rows[0]["DOB"]);
            if (aDoTable.Rows[0]["Birth_Place"] != DBNull.Value)
                moStudentInfo.sBirthPlace = aDoTable.Rows[0]["Birth_Place"].ToString();
            if (aDoTable.Rows[0]["Nationality"] != DBNull.Value)
                moStudentInfo.sNationality = aDoTable.Rows[0]["Nationality"].ToString();
            if (aDoTable.Rows[0]["Sex"] != DBNull.Value)
                moStudentInfo.cSex = Convert.ToChar(aDoTable.Rows[0]["Sex"]);
            if (aDoTable.Rows[0]["Admission_date"] != DBNull.Value)
                moStudentInfo.dAdmissionDate = Convert.ToDateTime(aDoTable.Rows[0]["Admission_date"]);
            if (aDoTable.Rows[0]["Joining_Date"] != DBNull.Value)
                moStudentInfo.dJoining_Date = Convert.ToDateTime(aDoTable.Rows[0]["Joining_Date"]);
            if (aDoTable.Rows[0]["Parent_Name"] != DBNull.Value)
                moStudentInfo.sParentName = aDoTable.Rows[0]["Parent_Name"].ToString();
            if (aDoTable.Rows[0]["Address"] != DBNull.Value)
                moStudentInfo.sAddress = aDoTable.Rows[0]["Address"].ToString();
           if (aDoTable.Rows[0]["BusPickupCity"] != DBNull.Value)
                moStudentInfo.sBusPickupCity = Convert.ToString(aDoTable.Rows[0]["BusPickupCity"]);
            if (aDoTable.Rows[0]["State"] != DBNull.Value)
                moStudentInfo.sState = Convert.ToString(aDoTable.Rows[0]["State"]);
            if (aDoTable.Rows[0]["City"] != DBNull.Value)
                moStudentInfo.sCity = Convert.ToString(aDoTable.Rows[0]["City"]);
            if (aDoTable.Rows[0]["Parent_Occupation"] != DBNull.Value)
                moStudentInfo.iParentOcupation = Convert.ToInt32(aDoTable.Rows[0]["Parent_Occupation"]);
            if (aDoTable.Rows[0]["Other_Occupation"] != DBNull.Value)
                moStudentInfo.sOtherOcupation = Convert.ToString(aDoTable.Rows[0]["Other_Occupation"]);
            if (aDoTable.Rows[0]["Residence_Phone_Number"] != DBNull.Value)
                moStudentInfo.sResidencePhoneNo = aDoTable.Rows[0]["Residence_Phone_Number"].ToString();
            if (aDoTable.Rows[0]["Mobile_Number"] != DBNull.Value)
                moStudentInfo.sMobilePhoneNo = aDoTable.Rows[0]["Mobile_Number"].ToString();
            if (aDoTable.Rows[0]["Mobile_Number2"] != DBNull.Value)
                moStudentInfo.sMobilePhoneNo2 = aDoTable.Rows[0]["Mobile_Number2"].ToString();
            if (aDoTable.Rows[0]["Mother_Tongue"] != DBNull.Value)
                moStudentInfo.sMotherTongue = aDoTable.Rows[0]["Mother_Tongue"].ToString();
            if (aDoTable.Rows[0]["Neighbour_Number"] != DBNull.Value)
                moStudentInfo.sNeighbourNo = aDoTable.Rows[0]["Neighbour_Number"].ToString();
            if (aDoTable.Rows[0]["Office_Number"] != DBNull.Value)
                moStudentInfo.sOfficeNo = aDoTable.Rows[0]["Office_Number"].ToString();
            if (aDoTable.Rows[0]["Category_Id"] != DBNull.Value)
                moStudentInfo.iCategoryId = Convert.ToInt32(aDoTable.Rows[0]["Category_Id"]);
            if (aDoTable.Rows[0]["CasteAndSubCaste"] != DBNull.Value)
                moStudentInfo.sCasteAndSubCaste = Convert.ToString(aDoTable.Rows[0]["CasteAndSubCaste"]);
            if (aDoTable.Rows[0]["Pincode"] != DBNull.Value)
                moStudentInfo.sPincode = Convert.ToString(aDoTable.Rows[0]["Pincode"]);
            if (aDoTable.Rows[0]["Is_Leave"] != DBNull.Value)
                moStudentInfo.mcIsLeave = Convert.ToChar(aDoTable.Rows[0]["Is_Leave"]);
            if (aDoTable.Rows[0]["Standard_Division_Name"] != DBNull.Value)
                moYearWiseStudentInfo.sStandardDivisionName = Convert.ToString(aDoTable.Rows[0]["Standard_Division_Name"]);
            if (aDoTable.Rows[0]["Student_Id"] != DBNull.Value)
                moYearWiseStudentInfo.iYearWIseStudentId = Convert.ToInt32(aDoTable.Rows[0]["Student_Id"]);
            if (aDoTable.Rows[0]["Standard_Id"] != DBNull.Value)
                moYearWiseStudentInfo.iStandardId = Convert.ToInt32(aDoTable.Rows[0]["Standard_Id"]);
            if (aDoTable.Rows[0]["Roll_No"] != DBNull.Value)
                moYearWiseStudentInfo.iRollNo = Convert.ToInt32(aDoTable.Rows[0]["Roll_No"]);
            if (aDoTable.Rows[0]["Photo_File_Path"] != DBNull.Value)
                moStudentInfo.sPhotoFilePath = aDoTable.Rows[0]["Photo_File_Path"].ToString();
            if (aDoTable.Rows[0]["User_Id"] != DBNull.Value)
                moStudentInfo.iUser_Id = Convert.ToInt32(aDoTable.Rows[0]["User_Id"]);
            if (aDoTable.Rows[0]["AadharCardNo"] != DBNull.Value)
                moStudentInfo.sAadharCardNo = Convert.ToString(aDoTable.Rows[0]["AadharCardNo"]);
            if (aDoTable.Columns.Contains("NameOnAadharCard") && aDoTable.Rows[0]["NameOnAadharCard"] != DBNull.Value)
                moStudentInfo.sNameOnAadharCard = Convert.ToString(aDoTable.Rows[0]["NameOnAadharCard"]);
            if (aDoTable.Columns.Contains("AadharCard_Photo_Copy_Path") && aDoTable.Rows[0]["AadharCard_Photo_Copy_Path"] != DBNull.Value)
                moStudentInfo.sAadharCardNumberPhotoCopyName = Convert.ToString(aDoTable.Rows[0]["AadharCard_Photo_Copy_Path"]);
            if (aDoTable.Columns.Contains("Family_Photo_Copy_Path") && aDoTable.Rows[0]["Family_Photo_Copy_Path"] != DBNull.Value)
                moStudentInfo.sFamilyPhoto = Convert.ToString(aDoTable.Rows[0]["Family_Photo_Copy_Path"]);
            if (aDoTable.Rows[0]["UDISENumber"] != DBNull.Value)
                moStudentInfo.sUDISENumber = Convert.ToString(aDoTable.Rows[0]["UDISENumber"]);
            if (aDoTable.Rows[0]["BoardRegistrationNo"] != DBNull.Value)
                moStudentInfo.sBoardRegNo = Convert.ToString(aDoTable.Rows[0]["BoardRegistrationNo"]);
            if (aDoTable.Columns.Contains("Email_Address"))
            {
                if (aDoTable.Rows[0]["Email_Address"] != DBNull.Value)
                    moStudentInfo.sEmail = aDoTable.Rows[0]["Email_Address"].ToString();
            }
            if (aDoTable.Rows[0]["Salutation_Name"] != DBNull.Value)
                moStudentInfo.sSalutationName = Convert.ToString(aDoTable.Rows[0]["Salutation_Name"]);
            if (aDoTable.Rows[0]["Standard_Name"] != DBNull.Value)
                moStudentInfo.sStandardName = Convert.ToString(aDoTable.Rows[0]["Standard_Name"]);
            if (aDoTable.Rows[0]["SchoolLeft_Date"] != DBNull.Value)
                moStudentInfo.dLeftDate = Convert.ToDateTime(aDoTable.Rows[0]["SchoolLeft_Date"]);
            if (aDoTable.Rows[0]["CancellationFormNo"] != DBNull.Value)
                moStudentInfo.CancellationFormNo = Convert.ToInt32(aDoTable.Rows[0]["CancellationFormNo"]);
            if (aDoTable.Columns.Contains("Is_New_Student"))
            {
                if (aDoTable.Rows[0]["Is_New_Student"] != DBNull.Value)
                    moStudentInfo.bIsNewStudent = Convert.ToBoolean(aDoTable.Rows[0]["Is_New_Student"]);
            }
            if (aDoTable.Columns.Contains("Is_RTE_Student"))
            {
                if (aDoTable.Rows[0]["Is_RTE_Student"] != DBNull.Value)
                    moStudentInfo.bIsRTEStudent = Convert.ToBoolean(aDoTable.Rows[0]["Is_RTE_Student"]);
            }
            if (aDoTable.Columns.Contains("RTECategoryId"))
            {
                if (aDoTable.Rows[0]["RTECategoryId"] != DBNull.Value)
                    moStudentInfo.iRTECategoryId = Convert.ToInt32(aDoTable.Rows[0]["RTECategoryId"]);
            }
            //new code RTFFormNo
            if (aDoTable.Columns.Contains("RTEApplicationFormNo"))
            {
                if (aDoTable.Rows[0]["RTEApplicationFormNo"] != DBNull.Value)
                    moStudentInfo.sRTEFormNo = Convert.ToString(aDoTable.Rows[0]["RTEApplicationFormNo"]);
            }
            if (aDoTable.Columns.Contains("AnnualIncome"))
            {
                if (aDoTable.Rows[0]["AnnualIncome"] != DBNull.Value)
                    moStudentInfo.sAnnualIncome = Convert.ToInt32(aDoTable.Rows[0]["AnnualIncome"]);
            }

            if (aDoTable.Columns.Contains("Rule_Id"))
            {
                if (aDoTable.Rows[0]["Rule_Id"] != DBNull.Value)
                    moStudentInfo.iRule_Id = Convert.ToInt32(aDoTable.Rows[0]["Rule_Id"]);
            }
            if (aDoTable.Columns.Contains("IsStaffKid"))
            {
                if (aDoTable.Rows[0]["IsStaffKid"] != DBNull.Value)
                    moStudentInfo.bIsStaffKid = Convert.ToBoolean(aDoTable.Rows[0]["IsStaffKid"]);
            }
            if (aDoTable.Rows[0]["Optional_Subject_Id"] != DBNull.Value)
                moStudentInfo.iOptionalSubjectId = Convert.ToInt32(aDoTable.Rows[0]["Optional_Subject_Id"]);
            if (aDoTable.Columns.Contains("SiblingStudentName"))
            {
                if (aDoTable.Rows[0]["SiblingStudentName"] != DBNull.Value)
                    moStudentInfo.sStidentSiblingNames = Convert.ToString(aDoTable.Rows[0]["SiblingStudentName"]);
            }

            if (aDoTable.Rows[0]["Height"] != DBNull.Value)
                moStudentInfo.dHeight = Convert.ToDouble(aDoTable.Rows[0]["Height"]);
            if (aDoTable.Rows[0]["Weight"] != DBNull.Value)
                moStudentInfo.dWeight = Convert.ToDouble(aDoTable.Rows[0]["Weight"]);
            if (aDoTable.Rows[0]["LastSchoolName"] != DBNull.Value)
                moStudentInfo.sLastSchoolName = Convert.ToString(aDoTable.Rows[0]["LastSchoolName"]);
            if (aDoTable.Rows[0]["LastSchoolAddress"] != DBNull.Value)
                moStudentInfo.sLastSchoolAddress = Convert.ToString(aDoTable.Rows[0]["LastSchoolAddress"]);
            if (aDoTable.Rows[0]["LastCompletedStd"] != DBNull.Value)
                moStudentInfo.sLastSchoolStandard = Convert.ToString(aDoTable.Rows[0]["LastCompletedStd"]);
            if (aDoTable.Rows[0]["LastSchoolUDISENo"] != DBNull.Value)
                moStudentInfo.sLastSchoolUDISENo = Convert.ToString(aDoTable.Rows[0]["LastSchoolUDISENo"]);
            if (aDoTable.Rows[0]["LastCompletedBoard"] != DBNull.Value)
                moStudentInfo.sLastSchoolBoardName = Convert.ToString(aDoTable.Rows[0]["LastCompletedBoard"]);
            if (aDoTable.Rows[0]["IsRecognisedBoard"] != DBNull.Value)
                moStudentInfo.bIsRecognised = Convert.ToBoolean(aDoTable.Rows[0]["IsRecognisedBoard"]);
            if (aDoTable.Columns.Contains("Photo_file_Path_Image"))
            {
                if (aDoTable.Rows[0]["Photo_file_Path_Image"] != DBNull.Value)
                    moStudentInfo.sPhotoFilePathInBinary = aDoTable.Rows[0]["Photo_file_Path_Image"] as Byte[];
            }
            if (aDoTable.Columns.Contains("Division_id"))
            {
                if (aDoTable.Rows[0]["Division_id"] != DBNull.Value)
                    moYearWiseStudentInfo.iDivisionId = Convert.ToInt32(aDoTable.Rows[0]["Division_id"]);
            }

            if (aDoTable.Rows[0]["SecondLanguageSubjectId"] != DBNull.Value)
                moStudentInfo.iSecondLanguageSubjectId = Convert.ToInt32(aDoTable.Rows[0]["SecondLanguageSubjectId"]);

            if (aDoTable.Rows[0]["ThirdLanguageSubjectId"] != DBNull.Value)
                moStudentInfo.iThirdLanguageSubjectId = Convert.ToInt32(aDoTable.Rows[0]["ThirdLanguageSubjectId"]);

            if (aDoTable.Columns.Contains("ParentUserId") && aDoTable.Rows[0]["ParentUserId"] != DBNull.Value)
                moStudentInfo.iParentUserId = Convert.ToInt32(aDoTable.Rows[0]["ParentUserId"]);

            if (aDoTable.Columns.Contains("ParentUserRoleId") && aDoTable.Rows[0]["ParentUserRoleId"] != DBNull.Value)
                moStudentInfo.iParentUserRoleId = Convert.ToInt32(aDoTable.Rows[0]["ParentUserRoleId"]);

            if (aDoTable.Rows[0]["IsRiseAndShine"] != DBNull.Value)
                moStudentInfo.IsRiseAndShine = aDoTable.Rows[0]["IsRiseAndShine"].ToBool();

            if (aDoTable.Rows[0]["AdmissionForId"] != DBNull.Value)
                moStudentInfo.AdmissionForId = aDoTable.Rows[0]["AdmissionForId"].ToInt();

            if (aDoTable.Rows[0]["User_Password"] != DBNull.Value)
                moStudentInfo.sPassword = aDoTable.Rows[0]["User_Password"].ToString();
            if (aDoTable.Rows[0]["GRNumber"] != DBNull.Value)
                moStudentInfo.sGRNumber = aDoTable.Rows[0]["GRNumber"].ToString();
            if (aDoTable.Rows[0]["StudentUniqueNo"] != DBNull.Value)
                moStudentInfo.sStudentUniqueNo = aDoTable.Rows[0]["StudentUniqueNo"].ToString();
            if (aDoTable.Rows[0]["Category_Name"] != DBNull.Value)
                moStudentInfo.sCategory = aDoTable.Rows[0]["Category_Name"].ToString();
            if (aDoTable.Rows[0]["Religion"] != DBNull.Value)
                moStudentInfo.sReligion = aDoTable.Rows[0]["Religion"].ToString();
            if (aDoTable.Rows[0]["ConfirmedByText"] != DBNull.Value)
                moStudentInfo.sConfirmedByText = aDoTable.Rows[0]["ConfirmedByText"].ToString();

            if (aDoTable.Rows[0]["UpdatedByText"] != DBNull.Value)
                moStudentInfo.sUpdatedByText = aDoTable.Rows[0]["UpdatedByText"].ToString();

            if (aDoTable.Rows[0]["IsForDayBoarding"] != DBNull.Value)
                moStudentInfo.IsForDayBoarding = aDoTable.Rows[0]["IsForDayBoarding"].ToBool();
            if (aDoTable.Rows[0]["IsDayBoardingFeePaid"] != DBNull.Value)
                moStudentInfo.IsDayBoardingFeePaid = aDoTable.Rows[0]["IsDayBoardingFeePaid"].ToBool();
            if (aDoTable.Rows[0]["FeeCategoryDetailsId"] != DBNull.Value)
                moStudentInfo.FeeCategoryId = aDoTable.Rows[0]["FeeCategoryDetailsId"].ToInt();
            if (aDoTable.Rows[0]["SaralNo"] != DBNull.Value)
                moStudentInfo.SaralNo = aDoTable.Rows[0]["SaralNo"].ToString();
            if (aDoTable.Rows[0]["IsOnlyChild"] != DBNull.Value)
                moStudentInfo.IsOnlyChild = aDoTable.Rows[0]["IsOnlyChild"].ToBool();
            if (aDoTable.Rows[0]["Minority"] != DBNull.Value)
                moStudentInfo.Minority = aDoTable.Rows[0]["Minority"].ToBool();
            if (aDoTable.Rows[0]["ResidenceTypeId"] != DBNull.Value)
                moStudentInfo.iResidenceTypeId = Convert.ToInt32(aDoTable.Rows[0]["ResidenceTypeId"]);
            if (aDoTable.Rows[0]["ResidenceName"] != DBNull.Value)
                moStudentInfo.sResidenceTypeName = Convert.ToString(aDoTable.Rows[0]["ResidenceName"]);
            if (aDoTable.Columns.Contains("AdmissionStandard") && aDoTable.Rows[0]["AdmissionStandard"] != DBNull.Value)
                moStudentInfo.sAdmissionStandard = Convert.ToString(aDoTable.Rows[0]["AdmissionStandard"]);
            if (aDoTable.Columns.Contains("Is_PrePrimary") && aDoTable.Rows[0]["Is_PrePrimary"] != DBNull.Value)
                moStudentInfo.sIsPrePrimaryStandard = Convert.ToString(aDoTable.Rows[0]["Is_PrePrimary"]);
            if (aDoTable.Columns.Contains("PrePrimaryEnrolmentNumber") && aDoTable.Rows[0]["PrePrimaryEnrolmentNumber"] != DBNull.Value)
                moStudentInfo.sPrePrimaryEnrolmentNumber = Convert.ToString(aDoTable.Rows[0]["PrePrimaryEnrolmentNumber"]);
            if (aDoTable.Columns.Contains("CasteCertificate_Photo_Copy_Path") && aDoTable.Rows[0]["CasteCertificate_Photo_Copy_Path"] != DBNull.Value)
                moStudentInfo.sCasteCertPhoto = Convert.ToString(aDoTable.Rows[0]["CasteCertificate_Photo_Copy_Path"]);
        }

        /// <summary>
        /// Returns dataset containing number of students for the specified standard and division.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicID"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <returns></returns>
        public static DataSet CheckIfStudentsAreAvailableForStandardDivision(int aiSchoolId, int aiAcademicID, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTotalNumberOfStudentsForStandardDivision");
            }

        }

        public bool CheckIsEnrollmentNumberIsDuplicate(int aiSchoolId, string asRegistrationNumber)
        {
            string sSelectStatement = " SELECT COUNT(*) " +
                                    " FROM " +
                                       " vw_BaseStudentDetails " +
                                     " WHERE Enrolment_Number=N'" + asRegistrationNumber.ToString() + "' " +
                                     " AND School_Id=" + aiSchoolId +
                                     " AND Is_Deleted= N'" + Constants.C_NO + "' ";

            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            // If the count is zero there is no duplication of Buyer login. 
            if (iCount == 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public DataTable CheckIsGeneralRegistrationNumberIsDuplicate()
        {

            string sSelectStatement =
             "select GRNumber FRom SchoolWise_Student_Master Where GRNumber <> '' and GRNumber IS NOT NULL";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        public DataTable CheckIsStudentUniqueNumberIsDuplicate()
        {

            string sSelectStatement =
             "select StudentUniqueNo FRom SchoolWise_Student_Master Where StudentUniqueNo <> ''and StudentUniqueNo IS NOT NULL";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }


        public bool CheckIsStudentIsDuplicate(int aiSchoolId, string asFirstName, string asLastName, DateTime adtDOB)
        {
            string sFilter = "";
            if (asLastName != null && asLastName != string.Empty)
                sFilter = " AND Last_Name='" + StringUtility.ReplaceSingleQuoteInString(asLastName.ToString(), false) + "' ";


            string sSelectStatement = " SELECT COUNT(*) " +
                                    " FROM " +
                                       " vw_BaseStudentDetails " +
                                     " WHERE First_Name=N'" + StringUtility.ReplaceSingleQuoteInString(asFirstName.ToString(), false) + "' " +
                                     sFilter +
                                     " AND DOB=N'" + adtDOB.ToString() + "' " +
                                     " AND School_Id=" + aiSchoolId +
                                     " AND Is_Deleted= N'" + Constants.C_NO + "' ";

            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            // If the count is zero there is no duplication of Buyer login. 
            if (iCount == 0)
                return false;
            else
                return true;
        }


        public bool CheckIsRollNumberDuplicate(int aiSchoolId, int aiAcademicYearId, int aiStandardId,
                                                int aiDivisionId, int aiRollNo, int aiStudentId)
        {
            string sSelectStatement = " SELECT Count(*) " +
                                    " FROM " +
                                      " vw_BaseStudentDetails " +
                                    " INNER JOIN " +
                                       " YearWise_Student_Details " +
                                 " ON " +
                                    " vw_BaseStudentDetails.SchoolWise_Student_Id = YearWise_Student_Details.Student_Id " +
                                 " WHERE " +
                                     " vw_BaseStudentDetails.School_Id =" + aiSchoolId +
                                     " AND YearWise_Student_Details.Academic_Year_ID=" + aiAcademicYearId +
                                     " AND YearWise_Student_Details.Standard_Id =" + aiStandardId +
                                     " AND YearWise_Student_Details.Division_id =" + aiDivisionId +
                                     " AND YearWise_Student_Details.Roll_No =" + aiRollNo +
                                     " AND YearWise_Student_Details.Student_Id !=" + aiStudentId +
                                     " AND vw_BaseStudentDetails.SchoolLeft_Date IS NULL" +
                                     " AND vw_BaseStudentDetails.Is_Deleted=N'" + Constants.C_NO + "' " +
                                     " AND YearWise_Student_Details.Is_Deleted=N'" + Constants.C_NO + "' ";

            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            // If the count is zero there is no duplication of Buyer login. 
            if (iCount == 0)
                return false;
            else
                return true;
        }
        public bool CheckIsRFormNumberDuplicate(int aiSchoolId, string sFormNo, int aiStudentId)
        {
            string sWhere = "";
            if (aiStudentId != 0)
            {
                sWhere = " AND SchoolWise_Student_Master.Schoolwise_Student_Id<> '" + aiStudentId + "'";
            }
            string sSelectStatement = "SELECT COUNT(*) From SchoolWise_Student_Master" +
                " WHERE SchoolWise_Student_Master.Is_Deleted='N' " +
                " and SchoolWise_Student_Master.School_Id=" + aiSchoolId +
                " AND SchoolWise_Student_Master.FormNumber=N'" + StringUtility.ReplaceSingleQuoteInString(sFormNo, false) + "'" +
                sWhere;

            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            // If the count is zero there is no duplication of Buyer login. 
            if (iCount == 0)
                return false;
            else
                return true;
        }

        public void UpdateIsLeaveFlag(int aiStudentId)
        {
            string sUpdateStatement = " UPDATE " +
                                         " Schoolwise_Student_Master " +
                                      " SET " +
                                         " Is_Leave = N'" + Constants.C_YES + "' " +
                                     " WHERE " +
                                         " Schoolwise_Student_Id=" + aiStudentId +
                                         " AND Is_Deleted = N'" + Constants.C_NO + "' ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to change division of multiple students.
        /// </summary>
        /// <param name="aoArrDeleteUserIds"></param>
        /// <param name="aiDivisionId"></param>
        public string UpdateStudentDivision(string asStudentIdsXML, int aiSrcStdId, int aiSrcDivId, int aiTargerStdId, int aiTargetDivId, int aiSchoolId, int aiAcademicYrId, int aiFinancialYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_intSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intAcademicYear_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_SrcStdId", aiSrcStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_srcDivId", aiSrcDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_StudentList", asStudentIdsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("prm_TargetStdId", aiTargerStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intTargetDivId", aiTargetDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                SqlParameter OSqlParameter = oSQLServerDbUtility.AddParameter("InternalFeeMessage", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 500);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_TransferStudent");
                return OSqlParameter.Value.ToString();
            }
        }


        /// <summary>
        /// This method is used update students roll nos.
        /// </summary>
        /// <param name="ischoolId"></param>
        /// <param name="iAcademicYearId"></param>
        /// <param name="iStandardId"></param>
        /// <param name="iDivisionId"></param>
        /// <param name="sXmlStudentsRollNos"></param>
        public void UpdateStudentsRollNos(int ischoolId, int iAcademicYearId, int iStandardId, int iDivisionId, string sXmlStudentsRollNos)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ischoolId", ischoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStandardId", iStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iDivisionId", iDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sXmlStudentsRollNos", sXmlStudentsRollNos, SqlDbType.Xml);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStudentsRollNos");
            }
        }

        /// <summary>
        /// Returns datatable contianing some of the details of student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static DataSet GetStudentDetailsForControlPanel(int aiStudentId, int aiSchoolId, int aiAcademicYrId)
        {
            string sUSPName = "usp_GetStudentDetailsForControlPanel";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iStudent_Id", aiStudentId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(sUSPName);
            }
        }

        /// <summary>
        /// Returns datatable contianing some of the details of student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static DataSet GetControlPanelDetails(int aiUserId, int aiSchoolId, int aiAcademicYrId)
        {
            string sUSPName = "usp_GetControlPanelDetails";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iUser_Id", aiUserId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(sUSPName);
            }
        }

        private DataTable RetriveStudentInfo(Int32 aiSchoolId, string asRegNo)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iReg_No", asRegNo, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStuentDetailsForLC");
            }
        }
        public void RetriveStudentdetailsForLC(Int32 aiSchoolId, string asRegNo)
        {

            DataTable oDTStudent = RetriveStudentInfo(aiSchoolId, asRegNo);
            if (oDTStudent.Rows.Count > 0)
                FillStruct(oDTStudent);
        }


        public static string GetMobileNo(int aiSchoolId, int aiStudentId)
        {
            string sSelectStatement = "SELECT " +
                                      "DISTINCT   vw_GetAllStudentsForStandardDivision.Mobile_Number " +
                                      " + ';' + ISNULL(vw_GetAllStudentsForStandardDivision.Mobile_Number2,' ')+ ';' " +
                                      "FROM " +
                                      "vw_GetAllStudentsForStandardDivision " +
                                      "WHERE " +
                                      "(vw_GetAllStudentsForStandardDivision.School_Id = " + aiSchoolId + ") " +
                                      "AND (vw_GetAllStudentsForStandardDivision.Student_Id = " + aiStudentId + ") " +
                                      "AND (vw_GetAllStudentsForStandardDivision.Is_Deleted = 'N')";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);
        }

        public DataTable IsStudentPrePrimary(int iSchoolId, int iAcademicYearId, int iStudentId)
        {
            string sQueryString = " SELECT     Standard_Master.Is_PrePrimary " +
                                    " FROM         Standard_Master INNER JOIN " +
                                    " YearWise_Student_Details ON Standard_Master.Standard_Id = YearWise_Student_Details.Standard_Id AND " +
                                    " Standard_Master.School_Id = YearWise_Student_Details.School_Id AND " +
                                    " Standard_Master.academic_Year_Id = YearWise_Student_Details.Academic_Year_ID " +
                                    " WHERE     (Standard_Master.School_Id = " + iSchoolId + ") AND " +
                //" (Standard_Master.academic_Year_Id = " + iAcademicYearId + ") AND " +
                                    " (Standard_Master.Is_Deleted = 'N') AND " +
                                    " (YearWise_Student_Details.Is_Deleted = 'N') AND " +
                                    " (YearWise_Student_Details.YearWise_Student_Id = " + iStudentId + ") AND " +
                                    " (Standard_Master.Is_PrePrimary = 'Y')";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQueryString);
        }

        public void RemoveStudentPhoto(int aiStudentId, int aiSchoolId)
        {
            string sUpdateStatement = " UPDATE [dbo].[SchoolWise_Student_Master] SET " +
                                      " [Photo_file_Path] = ''" +
                                      " ,[Photo_file_Path_Image] = null " +
                                      " ,[ProfilePicUpdateDate] = '" + System.DateTime.Now.ToString() + "'" +
                                      " WHERE  [SchoolWise_Student_Id] = " + aiStudentId +
                                      "  AND [School_Id] = " + aiSchoolId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        #endregion

        public void SaveTransferredStudentDetails(string ids, int aiSchoolId, int aiAcademicYEarId, int aiInsertedById)
        {
            DataTable dt = new DataTable();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentIds", ids, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYEarId", aiAcademicYEarId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                dt = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_SaveTransferredStudentDetails");
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in dt.Rows)
                {
                    string sOldUserName = dr["OldUserName"].ToString();
                    string sOldPassword = dr["OldPassword"].ToString();
                    string sNewUserName = dr["NewUserName"].ToString();
                    string sUserId = dr["UserId"].ToString();

                    string sPassword = CommonUtility.GetDecryptedPassword(sOldUserName, sOldPassword);
                    string sNewPassword = CommonUtility.GetEncryptedPassword(sNewUserName, sOldPassword);

                    sb.Append(";" + "UPDATE USER_MASTER SET USER_PASSWORD='" + sNewPassword + "' WHERE USER_ID=" + sUserId);
                }

                if (sb.Length > 0)
                    oSQLServerDbUtility.PerformStringQueryOnSqlServer(sb.ToString().Substring(1));
            }
        }


        public DataTable GetStudentListToActiveTransfer(int aiSchoolId, string asName, bool abShowOnlyNonActivated, bool abIsFrom)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("BranchSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ShowOnlyNonActivated", abShowOnlyNonActivated, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsFrom", abIsFrom, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentListToActiveTransfer");
            }
        }

        public DataTable GetStudentDetails(int aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, bool abIncludeUserName)
        {
            mbIncludeUserName = abIncludeUserName;
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, asName, true);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTransferStudent");
            }
        }

        public void TransferStudents(string Ids, int aiSchoolId, int aiAcademicYearId, int aiTargetSchoolId, int aiInsertedById)
        {
            DataTable dt = new DataTable();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentIds", Ids, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                dt = oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentDetailsToTransferAcrossBranch");
            }
            string TransferData = (dt.Rows[0]["TransferData"].ToString());

            string connectionString = "Data Source= " + ConfigurationManager.AppSettings["ReportingDataSource"] + "; Database=RITeSchool"
                      + "; User ID=" + ConfigurationManager.AppSettings["ReportingUserId"] + "; Password=" + ConfigurationManager.AppSettings["ReportingPassword"];

            string sConnectionString = string.Empty;

            using (SqlConnection oSqlConnection = new SqlConnection(connectionString))
            {
                string command = string.Empty;

                if (aiTargetSchoolId != 0)
                    command = "SELECT * FROM SchoolBranchDetails WHERE  SchoolId=" + aiTargetSchoolId + " ";
                SqlCommand oSqlCommand = new SqlCommand(command, oSqlConnection);
                oSqlConnection.Open();
                using (SqlDataReader oSqlDataReader = oSqlCommand.ExecuteReader())
                {
                    if (oSqlDataReader.Read())
                    {
                        sConnectionString = "Data Source= " + oSqlDataReader["ReportingServer"].ToString() + "; Database=" + oSqlDataReader["DatabaseName"].ToString() + "; User ID=" + oSqlDataReader["Username"].ToString() + "; Password=" + oSqlDataReader["Password"].ToString();
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(sConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_SubmitTransferredStudentDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SchoolId", SqlDbType.Int).Value = aiSchoolId;
                    cmd.Parameters.AddWithValue("@TransferData", SqlDbType.Xml).Value = TransferData;
                    cmd.Parameters.AddWithValue("@Inserted_By_Id", SqlDbType.Int).Value = aiInsertedById;
                    con.Open();
                    cmd.ExecuteNonQuery();

                }
                // return dt;
            }

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StudentIds", Ids, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TargetSchoolId", aiTargetSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_MarkLeftStudents");
            }
        }

        public List<SchoolBranchDetails> GetSchoolBranchDetails(int aiSchoolId)
        {
            List<SchoolBranchDetails> lstSchoolBranchDetails = new List<SchoolBranchDetails>();
            SchoolBranchDetails oSchoolBranchDetails;

            string connectionString = "Data Source= " + ConfigurationManager.AppSettings["ReportingDataSource"] + "; Database=RITeSchool"
                        + "; User ID=" + ConfigurationManager.AppSettings["ReportingUserId"] + "; Password=" + ConfigurationManager.AppSettings["ReportingPassword"];

            using (SqlConnection oSqlConnection = new SqlConnection(connectionString))
            {
                string command = string.Empty;
                if (aiSchoolId != 0)
                    command = "SELECT  * FROM SchoolBranchDetails WHERE SchoolId <>" + aiSchoolId + " and GroupId in (select GroupId from SchoolBranchDetails where SchoolId =" + aiSchoolId + ") ORDER BY SchoolName ASC";
                SqlCommand oSqlCommand = new SqlCommand(command, oSqlConnection);
                oSqlConnection.Open();

                string sConnectionString = string.Empty;
                SqlDataReader oSqlDataReader = oSqlCommand.ExecuteReader();

                while (oSqlDataReader.Read())
                {
                    oSchoolBranchDetails = new SchoolBranchDetails();
                    oSchoolBranchDetails.Id = Convert.ToInt32(oSqlDataReader["Id"]);
                    oSchoolBranchDetails.SchoolId = Convert.ToInt32(oSqlDataReader["SchoolId"]);
                    oSchoolBranchDetails.SchoolName = oSqlDataReader["SchoolName"].ToString();
                    oSchoolBranchDetails.GroupId = Convert.ToInt32(oSqlDataReader["GroupId"]);
                    oSchoolBranchDetails.DatabaseName = Convert.ToString(oSqlDataReader["DatabaseName"]);
                    oSchoolBranchDetails.ReportingServer = Convert.ToString(oSqlDataReader["ReportingServer"]);
                    oSchoolBranchDetails.Username = Convert.ToString(oSqlDataReader["Username"]);
                    oSchoolBranchDetails.Password = Convert.ToString(oSqlDataReader["Password"]);
                    lstSchoolBranchDetails.Add(oSchoolBranchDetails);
                    sConnectionString = "Data Source= " + oSqlDataReader["ReportingServer"].ToString() + "; Database=" + oSqlDataReader["DatabaseName"].ToString() + "; User ID=" + oSqlDataReader["Username"].ToString() + "; Password=" + oSqlDataReader["Password"].ToString();
                }
                oSqlConnection.Close();


            }
            return lstSchoolBranchDetails;
        }

        #region Display All

        public static DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, String sortExpression, int iEndIndex, int iStartIndex, Boolean bChekLeftDate, bool abIncludeUserName)
        {
            mbIncludeUserName = abIncludeUserName;
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, asName, bChekLeftDate, true);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedStudents");
            }
        }

        public static DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiUserTypeId, Int32 aiStandardId, Int32 aiDivisionId, string asName, String sortExpression, int iEndIndex, int iStartIndex, Boolean bChekLeftDate, bool abIncludeUserName)
        {
            mbIncludeUserName = abIncludeUserName;
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, asName, bChekLeftDate, true);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_UserTypeId", aiUserTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedStudents");
            }
        }

        /// <summary>
        /// This returns a count of total students for given filter.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asName"></param>
        /// <param name="bChekLeftDate"></param>
        /// <param name="abIncludeUserName"></param>
        /// <returns></returns>                       
        public static DataTable GetAllStudentsForFee(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, String sortExpression, int iEndIndex, int iStartIndex, Boolean bChekLeftDate, bool abIncludeUserName)
        {
            mbIncludeUserName = abIncludeUserName;
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, asName, bChekLeftDate, true);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedStudentsForFee");
            }
        }

        public string GetFormNumber(int aiSchoolId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetFormNumber"))
                {
                    string sFormNumber = string.Empty;
                    if (oSqlDataReader.Read())
                        sFormNumber = oSqlDataReader["FormNumber"].ToString();

                    return sFormNumber;
                }
            }
        }

        /// <summary>
        /// This returns all students for given filter.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asName"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="bChekLeftDate"></param>
        /// <param name="abIncludeUserName"></param>
        /// <returns></returns>        
        public static int CountStudentsForFee(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, Boolean bChekLeftDate, bool abIncludeUserName)
        {
            mbIncludeUserName = abIncludeUserName;
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, asName, bChekLeftDate, true);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountStudentsForFee");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// Returns the total count of students in the school, including left students.
        /// </summary>
        /// <returns></returns>
        public static List<StudentEntities.StudentInfo> GetStudentsForFeesUpdate(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, Boolean bChekLeftDate, bool mbConsiderForRTEConcession)
        {
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, asName);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ConsiderForRTEConcession", mbConsiderForRTEConcession, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentsForFeesUpdate"))
                {
                    List<StudentEntities.StudentInfo> lstStudeentDetails = new List<StudentEntities.StudentInfo>();
                    StudentEntities.StudentInfo oStudentInfo;
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            oStudentInfo = new StudentEntities.StudentInfo()
                            {
                                StudentName = oSqlDataReader["StudentName"].ToString(),
                                UserId = Convert.ToInt32(oSqlDataReader["User_Id"])
                            };
                            lstStudeentDetails.Add(oStudentInfo);
                        }
                    }
                    return lstStudeentDetails;
                }
            }
        }

        public static List<string> GetPaidFeesStudents(int aiSchoolId, int aiAcademicYearId, int aiCurrentStandardId, int aiCurrentDivId)
        {
            List<string> lstPaidFeesStudentIds = new List<string>();

            string sSqlStatement = "SELECT		a.Student_Id " +
                                    "FROM		Schoolwise_Student_Fee_Details a " +
                                    "			INNER JOIN SchoolWise_Standard_Division_Master b " +
                                    "			ON a.Standard_Div_Id = b.SchoolWise_Standard_Division_Id " +
                                    "WHERE		a.[Debit/Credit] = 'Credit' AND a.Is_Deleted = 'N' " +
                                    "			AND a.School_Id = " + aiSchoolId + " AND a.Academic_Year_Id = " + aiAcademicYearId + " " +
                                    "			AND b.Standard_Id = " + aiCurrentStandardId + " AND b.Division_Id = " + aiCurrentDivId + " " +
                                    "GROUP BY	a.Student_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
            {
                if (oSqlReader != null && oSqlReader.HasRows)
                {
                    while (oSqlReader.Read())
                    {
                        lstPaidFeesStudentIds.Add(oSqlReader["Student_Id"].ToString());
                    }
                }
            }

            return lstPaidFeesStudentIds;
        }


        public static DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, string asRegNo, bool abIsExactMatch, String sortExpression, int iEndIndex, int iStartIndex, Boolean bChekLeftDate, bool abIncludeUserName, bool abPhotoFilePath, string asOperator, string asPrefix)
        {
            mbIncludeUserName = abIncludeUserName;
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, asName, bChekLeftDate, abPhotoFilePath, asRegNo, abIsExactMatch, asOperator, asPrefix);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedStudents");
            }
        }





        public static DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, string asRegNo, bool abIsExactMatch, String sortExpression, int iEndIndex, int iStartIndex, Boolean bChekLeftDate, bool abIncludeUserName)
        {
            mbIncludeUserName = abIncludeUserName;
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, asName, bChekLeftDate, asRegNo, abIsExactMatch);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedStudents");
            }
        }
        public DataTable DeleteAllStudent(int aiStdDivId, int aischoolid, int aiAcademicYearId, int aiUpdatedById, int aiAssessmentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aischoolid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", aiAssessmentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DeleteAllStudentGrades", true);
            }

        }
        public DataTable Delete(int aiStudentId, int aiSchoolId, int aiAcademicYearId, int AiAssessmentId, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", AiAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_DeleteAssignMarks", true);
            }

        }
        public static DataTable GetAllLeaveStudents(Int32 aiSchoolId, int aiAcademicYearId, string asName, String sortExpression, int iEndIndex, int iStartIndex, Boolean bChekLeftDate)
        {
            mbIncludeUserName = false;
            string sFilter = CreateFilter(Constants.I_ZERO, Constants.I_ZERO, asName, bChekLeftDate, true);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedLeavedStudents");
            }
        }

        public static int CountRowsOfLeaveStaudent(Int32 aiSchoolId, int aiAcademicYearId, string asName, Boolean bChekLeftDate)
        {
            string sFilter = CreateFilter(Constants.I_ZERO, Constants.I_ZERO, asName, bChekLeftDate, true);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_CountLeaveStudents");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get month wise atttendance of given standard division.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="iStartIndex"></param>
        /// <returns></returns>
        public static DataTable GetStudentsMonthWiseAttendance(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardDivisionId, int topRanker, int Student_Id, String sortExpression, int iEndIndex, int iStartIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("topRanker", topRanker, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Student_Id", Student_Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageSize", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetMonthWiseAttendance");
            }

        }

        ///// <summary>
        ///// This function gets all the students from the table According to specified search criteria
        ///// the search parameters are:
        ///// name
        ///// standard
        ///// and division
        ///// </summary>
        ///// <param name="aiStudentId"></param>
        ///// <returns></returns>
        public static DataTable GetAllStudents(Int32 aiSchoolId, int aiStandardId, int aiDivisionId, Int32 aiAcademicYrId)
        {
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, string.Empty, true);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStudents");
            }

        }

        public static DataTable GetAllStudentsWithLeftFilter(Int32 aiSchoolId, int aiStandardId, int aiDivisionId, Int32 aiAcademicYrId, bool abFilterLeft)
        {
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, string.Empty, abFilterLeft);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStudents");
            }

        }

        public static DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, int aiStandardDivisionId, string asName, string asRegNo, bool abIsExactMatch, String sortExpression, int iEndIndex, int iStartIndex, Boolean bChekLeftDate, bool abIncludeUserName, string asOperator, string asPrefix)
        {
            mbIncludeUserName = abIncludeUserName;
            string sFilter = CreateFilter(aiStandardId, aiDivisionId, aiStandardDivisionId, asName, bChekLeftDate, asRegNo, abIsExactMatch, asOperator, asPrefix);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", iStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedStudents");
            }
        }

        ///// <summary>
        ///// This function gets all the students from the table According to specified search criteria
        ///// the search parameters are:
        ///// name
        ///// standard
        ///// and division
        ///// </summary>
        ///// <param name="aiStudentId"></param>
        ///// <returns></returns>
        public static DataTable GetAllStudents(Int32 aiSchoolId, Int32 aiAcademicYrId, string asRegNumbers)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sRegNumbers", asRegNumbers, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentsByRegNos");
            }
        }

        public static DataTable GetAllStudentsByName(Int32 aiSchoolId, Int32 aiAcademicYrId, string asRegNumbers, bool abIsOnlyPrimary, int aiReportId)
        {
            string sPrimaryFilter = string.Empty;
            if (aiReportId == I_BONAFIED_CERTIFICATE_ID || aiReportId == I_BONAFIED_CERTIFICATE_FOR_SS_ID)//Bonafied certificate
            {
                sPrimaryFilter += "and SchoolLeft_Date is NULL";
                aiReportId = 0;
            }
            if (aiReportId != 0)
                sPrimaryFilter = " AND Standard_Id IN (SELECT Standard_Id FROM StandardwiseProgressReportMaster WHERE Report_Id = " + aiReportId.ToString() + " AND Academic_Year_ID = " + aiAcademicYrId + " AND School_Id = " + aiSchoolId + " AND Is_Deleted = 'N') ";
            if (abIsOnlyPrimary == true)
                sPrimaryFilter += " and Is_PrePrimary = 'N' ";

            string sSelectStatement = "select " +
                                      "Enrolment_Number," +
                                      "SchoolWise_Standard_Division_Id," +
                                      "Standard_Id," +
                                      "Student_Id," +
                                      "Division_Id," +
                                      "Name," +
                                      "Standard_Division_Name" +
                                      " from " +
                                      "vw_GetAllStudentsForStandardDivision" +
                                      " where " +
                                      " School_Id = " + aiSchoolId + sPrimaryFilter +
                                      " and Academic_Year_ID = " + aiAcademicYrId +
                                      " and Is_Deleted = 'N'" +
                                      " and (Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asRegNumbers, false) + "%' OR Enrolment_Number like N'%" + StringUtility.ReplaceSingleQuoteInString(asRegNumbers, false) + "%')" +
                                      " ORDER BY Name,Enrolment_Number";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public static DataTable GetFinancial(Int32 aiSchoolId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetFinacialYear");
            }

        }

        public static DataTable GetAllAcademicYearsOfStudent(int aiSchoolId, int aiYearwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", aiYearwiseStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllAcademicYearsOfStudent");
            }
        }

        private static string CreateFilter(int aiStandardId, int aiDivisionId, string asName, Boolean bChekLeftDate)
        {
            string sFilter = "";
            if (aiStandardId != 0)
            {
                sFilter = " AND [Standard_Id] =+ CAST(" + aiStandardId.ToString() + "AS NVARCHAR(15))";
            }


            if (bChekLeftDate)
                sFilter = sFilter + " AND SchoolLeft_Date is null ";
            if (aiDivisionId != 0)
            {
                sFilter = sFilter + " AND [Division_id] =+ CAST(" + aiDivisionId.ToString() + "AS NVARCHAR(15))";

            }
            if (!String.IsNullOrEmpty(asName))
            {
                string sName = StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false);
                if (mbIncludeUserName)
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + sName + "%' OR Middle_Name LIKE N'%" + sName + "%' OR Last_Name LIKE N'%" + sName + "%' OR Enrolment_Number LIKE N'%" + sName + "%' OR User_Login LIKE N'%" + sName + "%')";
                else
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + sName + "%' OR Middle_Name LIKE N'%" + sName + "%' OR Last_Name LIKE N'%" + sName + "%' OR Enrolment_Number LIKE N'%" + sName + "%' OR Enrolment_Number + ' - ' + Name LIKE N'%" + sName + "%')";
            }
            return sFilter;
        }

        private static string CreateFilter(int aiStandardId, int aiDivisionId, string asName, Boolean bChekLeftDate, bool abIncludeFullName)
        {
            string sFilter = "";
            if (aiStandardId != 0)
            {
                sFilter = " AND [Standard_Id] =+ CAST(" + aiStandardId.ToString() + "AS VARCHAR(15))";
            }
            if (bChekLeftDate)
                sFilter = sFilter + " AND SchoolLeft_Date is null ";
            if (aiDivisionId != 0)
            {
                sFilter = sFilter + " AND [Division_id] =+ CAST(" + aiDivisionId.ToString() + "AS VARCHAR(15))";

            }
            if (!String.IsNullOrEmpty(asName))
            {
                string sName = StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false);
                if (mbIncludeUserName)
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + sName + "%' OR Middle_Name LIKE N'%" + sName + "%' OR Last_Name LIKE N'%" + sName + "%' OR Enrolment_Number LIKE N'%" + sName + "%' OR User_Login LIKE N'%" + sName + "%' OR Enrolment_Number + ' - ' + Name  LIKE N'%" + sName + "%')";
                else
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + sName + "%' OR Middle_Name LIKE N'%" + sName + "%' OR Last_Name LIKE N'%" + sName + "%' OR Enrolment_Number LIKE N'%" + sName + "%' OR Enrolment_Number + ' - ' + Name LIKE N'%" + sName + "%')";
            }
            return sFilter;
        }

        private static string CreateFilter(int aiStandardId, int aiDivisionId, string asName)
        {
            string sFilter = "";
            if (aiStandardId != 0)
            {
                sFilter = " AND YearWise_Student_Details.Standard_Id =+ CAST(" + aiStandardId.ToString() + "AS VARCHAR(15))";
            }

            sFilter = sFilter + " AND SchoolLeft_Date is null ";

            if (aiDivisionId != 0)
            {
                sFilter = sFilter + " AND YearWise_Student_Details.Division_id =+ CAST(" + aiDivisionId.ToString() + "AS VARCHAR(15))";

            }
            if (!String.IsNullOrEmpty(asName))
            {
                if (mbIncludeUserName)
                    sFilter = sFilter + " AND (First_Name = N'" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "' OR Middle_Name = N'" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "' OR Last_Name = N'" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "' OR Enrolment_Number = N'" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "' OR User_Login = '" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "')";
                else
                    sFilter = sFilter + " AND (First_Name = N'" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "' OR Middle_Name = N'" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "' OR Last_Name = N'" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "' OR Enrolment_Number = N'" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false) + "') ";
            }
            return sFilter;
        }

        private static string CreateFilter(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, string asRegNo, bool abIsExactMatch, String sortExpression, int iEndIndex, int iStartIndex, Boolean bChekLeftDate, bool abIncludeUserName, bool abPhotoFilePath, string asOperator, string asPrefix)
        {
            if (!abIsExactMatch || asRegNo.IsNullOrEmpty())
                msOperator = string.Empty;
            else
            {
                List<String> operators = GetOperators().Where(opr => opr.Value.ToString() == asOperator).Select(opr => opr.Text).ToList();
                msOperator = operators.Count > Constants.I_ZERO ? operators.First() : string.Empty;
            }
            return CreateFilter(aiStandardId, aiDivisionId, asName, bChekLeftDate, abPhotoFilePath, asRegNo, abIsExactMatch, asOperator, asPrefix);
        }


        private static string CreateFilter(int aiStandardId, int aiDivisionId, string asName, Boolean bChekLeftDate, bool abPhotoFilePath, string asRegNo, bool abIsExactMatch, string asOperator, string asPrefix)
        {
            if (!abIsExactMatch && asRegNo.IsNullOrEmpty())
                msOperator = string.Empty;
            else
            {
                List<String> operators = GetOperators().Where(opr => opr.Value.ToString() == asOperator).Select(opr => opr.Text).ToList();
                msOperator = operators.Count > Constants.I_ZERO ? operators.First() : string.Empty;
            }


            if (abIsExactMatch == true && asRegNo.IsNullOrEmpty())
                abIsExactMatch = false;

            string sFilter = "";
            if (aiStandardId != 0)
            {
                sFilter = " AND [Standard_Id] =+ CAST(" + aiStandardId.ToString() + "AS VARCHAR(15))";
            }
            if (bChekLeftDate)
                sFilter = sFilter + " AND SchoolLeft_Date is null ";
            if (aiDivisionId != 0)
            {
                sFilter = sFilter + " AND [Division_id] =+ CAST(" + aiDivisionId.ToString() + "AS VARCHAR(15))";

            }
            if (!String.IsNullOrEmpty(asName) && !abIsExactMatch)
            {
                string sName = StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true);
                if (mbIncludeUserName)
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + sName + "%' OR Middle_Name LIKE N'%" + sName + "%' OR Last_Name LIKE N'%" + sName + "%' OR Enrolment_Number LIKE N'%" + sName + "%' OR User_Login LIKE N'%" + sName + "%')";
                else
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + sName + "%' OR Middle_Name LIKE N'%" + sName + "%' OR Last_Name LIKE N'%" + sName + "%' OR Enrolment_Number LIKE N'%" + sName + "%' OR Enrolment_Number + ' - ' + Name LIKE N'%" + sName + "%')";
            }
            //else if (!String.IsNullOrEmpty(asRegNo))
            //    sFilter = sFilter + " AND Enrolment_Number IN (SELECT Name FROM udf_GetTableFromStringList('" + StringUtility.ReplaceSingleQuoteInString(asRegNo.Trim(), true) + "'))";
            //else if (abIsExactMatch)
            //    sFilter = sFilter + " AND Enrolment_Number =''";
            else if (!String.IsNullOrEmpty(asRegNo))
            {
                if (!asPrefix.IsNullOrEmpty())
                {
                    if (asPrefix == Constants.S_ALL)
                        asPrefix = string.Empty;

                    sFilter = sFilter + " AND Enrolment_Number LIKE N'%" + asPrefix + "%' " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
                }
                else
                    sFilter = sFilter + " AND #tblStudents.HasPrefix=0 " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
            }
            else if (abIsExactMatch)
                sFilter = sFilter + " AND Enrolment_Number =''";

            if (abPhotoFilePath)
                sFilter = sFilter + " AND (Photo_file_Path_Image is null)";
            return sFilter;
        }

        //This is for Photos only
        private static string CreateFilter(int aiStandardId, int aiDivisionId, string asName, Boolean bChekLeftDate, string asRegNo, bool abIsExactMatch, string Operator, string Prefix)
        {
            if (!abIsExactMatch || asRegNo.IsNullOrEmpty())
                msOperator = string.Empty;
            else
            {
                List<String> operators = GetOperators().Where(opr => opr.Value.ToString() == Operator).Select(opr => opr.Text).ToList();
                msOperator = operators.Count > Constants.I_ZERO ? operators.First() : string.Empty;
            }


            if (abIsExactMatch == true && asRegNo.IsNullOrEmpty())
                abIsExactMatch = false;

            string sFilter = "";
            if (aiStandardId != 0)
            {
                sFilter = " AND [Standard_Id] =+ CAST(" + aiStandardId.ToString() + "AS VARCHAR(15))";
            }
            if (bChekLeftDate)
                sFilter = sFilter + " AND SchoolLeft_Date is null ";
            if (aiDivisionId != 0)
            {
                sFilter = sFilter + " AND [Division_id] =+ CAST(" + aiDivisionId.ToString() + "AS VARCHAR(15))";

            }
            if (!String.IsNullOrEmpty(asName) && !abIsExactMatch)
            {
                if (mbIncludeUserName)
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Middle_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Last_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Enrolment_Number LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR User_Login LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%')";
                else
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Middle_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Last_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Enrolment_Number LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%') ";
            }
            //else if (!String.IsNullOrEmpty(asRegNo))
            //{
            //    sFilter = sFilter + " AND Enrolment_Number IN (SELECT Name FROM udf_GetTableFromStringList('" + StringUtility.ReplaceSingleQuoteInString(asRegNo.Trim(), true) + "'))";
            //}
            else if (!String.IsNullOrEmpty(asRegNo))
            {
                if (!Prefix.IsNullOrEmpty())
                {
                    if (Prefix == Constants.S_ALL)
                        Prefix = string.Empty;

                    sFilter = sFilter + " AND Enrolment_Number LIKE '%" + Prefix + "%' " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
                }
                else
                    sFilter = sFilter + " AND #tblStudents.HasPrefix=0 " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
            }
            else if (abIsExactMatch)
                sFilter = sFilter + " AND Enrolment_Number =''";
            return sFilter;
        }

        private static string CreateFilter(int aiStandardId, int aiDivisionId, string asName, Boolean bChekLeftDate, string asRegNo, bool abIsExactMatch)
        {
            string sFilter = "";
            if (aiStandardId != 0)
            {
                sFilter = " AND [Standard_Id] =+ CAST(" + aiStandardId.ToString() + "AS VARCHAR(15))";
            }
            if (bChekLeftDate)
                sFilter = sFilter + " AND SchoolLeft_Date is null ";
            if (aiDivisionId != 0)
            {
                sFilter = sFilter + " AND [Division_id] =+ CAST(" + aiDivisionId.ToString() + "AS VARCHAR(15))";

            }
            if (!String.IsNullOrEmpty(asName) && !abIsExactMatch)
            {
                if (mbIncludeUserName)
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Middle_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Last_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Enrolment_Number LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR User_Login LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%')";
                else
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Middle_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Last_Name LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%' OR Enrolment_Number LIKE N'%" + StringUtility.ReplaceSingleQuoteInString(asName.Trim(), true) + "%') ";
            }
            else if (!String.IsNullOrEmpty(asRegNo))
                sFilter = sFilter + " AND Enrolment_Number IN (SELECT Name FROM udf_GetTableFromStringList('" + StringUtility.ReplaceSingleQuoteInString(asRegNo.Trim(), true) + "'))";
            else if (abIsExactMatch)
                sFilter = sFilter + " AND Enrolment_Number =''";
            return sFilter;
        }

        private static string CreateFilter(int aiStandardId, int aiDivisionId, int aiStandardDivisionId, string asName, Boolean bChekLeftDate, string asRegNo, bool abIsExactMatch, string asOperator, string asPrefix)
        {
            if (!abIsExactMatch || asRegNo.IsNullOrEmpty())
                msOperator = string.Empty;
            else
            {
                List<String> operators = GetOperators().Where(opr => opr.Value.ToString() == asOperator).Select(opr => opr.Text).ToList();
                msOperator = operators.Count > Constants.I_ZERO ? operators.First() : string.Empty;
            }
            return CreateFilter(aiStandardId, aiDivisionId, aiStandardDivisionId, asName, bChekLeftDate, asRegNo, abIsExactMatch, asPrefix);
        }

        private static string CreateFilter(int aiStandardId, int aiDivisionId, int aiStandardDivisionId, string asName, Boolean bChekLeftDate, string asRegNo, bool abIsExactMatch, string asPrefix)
        {
            if (abIsExactMatch == true && asRegNo.IsNullOrEmpty())
                abIsExactMatch = false;
            string sFilter = "";
            if (aiStandardId != 0)
                sFilter = " AND [Standard_Id] =+ CAST(" + aiStandardId.ToString() + "AS VARCHAR(15))";

            if (bChekLeftDate)
                sFilter = sFilter + " AND SchoolLeft_Date is null ";

            if (aiDivisionId != 0)
                sFilter = sFilter + " AND [Division_id] =+ CAST(" + aiDivisionId.ToString() + "AS VARCHAR(15))";

            if (aiStandardDivisionId != 0)
                sFilter = sFilter + " AND [SchoolWise_Standard_Division_Id] =+ CAST(" + aiStandardDivisionId.ToString() + "AS VARCHAR(15))";

            if (!String.IsNullOrEmpty(asName) && !abIsExactMatch)
            {
                string sName = StringUtility.ReplaceSingleQuoteInString(asName.Trim(), false);
                if (mbIncludeUserName)
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + sName + "%' OR Middle_Name LIKE N'%" + sName + "%' OR Last_Name LIKE N'%" + sName + "%' OR Enrolment_Number LIKE N'%" + sName + "%' OR User_Login LIKE N'%" + sName + "%' OR Enrolment_Number + ' - ' + Name  LIKE N'%" + sName + "%')";
                else
                    sFilter = sFilter + " AND (First_Name LIKE N'%" + sName + "%' OR Middle_Name LIKE N'%" + sName + "%' OR Last_Name LIKE N'%" + sName + "%' OR Enrolment_Number LIKE N'%" + sName + "%' OR Enrolment_Number + ' - ' + Name LIKE N'%" + sName + "%')";
            }
            else if (!String.IsNullOrEmpty(asRegNo))
            {
                if (!asPrefix.IsNullOrEmpty())
                {
                    if (asPrefix == Constants.S_ALL)
                        asPrefix = string.Empty;

                    sFilter = sFilter + " AND Enrolment_Number LIKE '%" + asPrefix + "%' " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
                }
                else
                    sFilter = sFilter + " AND #tblStudents.HasPrefix=0 " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
            }
            else if (abIsExactMatch)
                sFilter = sFilter + " AND Enrolment_Number =''";

            return sFilter;
        }

        public static DataTable GetAllStudents(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId)
        {
            string sSelectStatement = " SELECT " +
                                        " Student_Id " +
                                        " , Enrolment_Number " +
                                        " , Roll_No " +
                                        " , Name " +
                                        " , 'false' as is_absent" +
                                        " , '' as Marks_Scored " +
                                    " FROM " +
                                        " vw_GetAllStudentsForStandardDivision" +
                                   " WHERE " +
                                        " School_Id =" + aiSchoolId +
                                        " AND Academic_Year_ID = " + aiAcademicYrId +
                                        " AND SchoolWise_Standard_Division_Id = " + aiStandardDivisionId +
                                    " ORDER BY " +
                                        " Roll_No ASC";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public static DataTable GetAllStudentsForMarks(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, DateTime dTestDate)
        {
            string sSelectStatement = " SELECT " +
                                        " Student_Id " +
                                        " , Enrolment_Number " +
                                        " , Roll_No " +
                                        " , Name " +
                                        " , 'false' as is_absent" +
                                        " , '' as Marks_Scored,Joining_Date " +
                                    " FROM " +
                                        " vw_GetAllStudentsForStandardDivision" +
                                   " WHERE " +
                                        " School_Id =" + aiSchoolId +
                                        " AND Academic_Year_ID = " + aiAcademicYrId +
                                        " AND SchoolWise_Standard_Division_Id = " + aiStandardDivisionId +
                                        " AND (SchoolLeft_Date IS NULL OR " +
                                        " SchoolLeft_Date >= '" + dTestDate.ToShortDateString() + "') " +
                                    " ORDER BY " +
                                        " Roll_No ASC";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public static DataTable GetAllStudentsForSubject(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, DateTime dTestDate, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Division_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestDate", dTestDate, SqlDbType.DateTime);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStudentsForMarksAssignment");
            }
        }

        public static DataTable GetStudentsResultList(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId,
                                int aiPageNumber, int aiPageSize, string asOrderBy, string asSortOrder)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageNumber", aiPageNumber, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageSize", aiPageSize, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("OrderBy", asOrderBy, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder", asSortOrder, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_getStudentResultList");
            }
        }

        public static DataTable getPrePrimaryProgressSheetStudentList(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, int aiTestId, String sortExpression, int maximumRows, int startRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_stdDivId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageSize", maximumRows, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);
                if (!string.IsNullOrEmpty(sortExpression))
                    oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_getPrePrimaryProgressSheetStudentList");
            }
        }

        public static DataTable getPrePrimaryProgressSheetCompleteStatus(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, int aiTestId)
        {
            string sSelectStr = "SELECT dbo.udf_getPrePrimaryProgressSheetStatus (" + aiSchoolId.ToString() +
                                "," + aiStandardDivisionId.ToString() + "," +
                                aiAcademicYrId.ToString() + "," + aiTestId.ToString() + ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStr);
        }

        public static DataTable GetStudentsResultList(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, String sortExpression, int maximumRows, int startRowIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_stdDivId", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PageSize", maximumRows, SqlDbType.Int);
                if (!string.IsNullOrEmpty(sortExpression))
                    oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sortExpression, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedStudentResult");
            }
        }

        /// <summary>
        /// This method is used to fetch Division toppers
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataSet GetAnnualResult(Int32 aiSchoolId, Int32 aiAcademicYrId, Int32 aiStandardDivisionId, Int32 aiNoOfRecords)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ToppersCount", aiNoOfRecords, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAnnualResultToppers");
            }
        }

        /// <summary>
        /// This method is used to fetch standered toppers
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataSet GetAnnualStanderedResult(Int32 aiSchoolId, Int32 aiAcademicYrId, Int32 aiStandardId, Int32 aiNoOfRecords)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ToppersCount", aiNoOfRecords, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAnnualResultStdToppers");
            }
        }

        /// <summary>
        /// This Functions is used to fetch students list in a splitted record set.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataSet GetStudentsForSubjectMarkSheet(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, Int32 aiNoOfRecords, Int32 iTestId, Int32 iSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NoOfRecords", aiNoOfRecords, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iTestId", iTestId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iSubjectId", iSubjectId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStudentsForSubjectMarkSheet");
            }
        }

        /// <summary>
        /// This Functions is used to fetch first 3 toppers students.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataSet GetFirstThreeToopers(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, Int32 aiTestId, Int32 aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivision_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Subject_Id", aiSubjectId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TestId", aiTestId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTopperStudents");
            }
        }

        /// <summary>
        /// This function is used to validate the student's data before complete deleting student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        public string ValidateStudent(int aiSchoolId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Message", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 500);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ValidateStudentDelete"))
                    return oSqlParameter.Value.ToString();
            }

        }

        public void DeleteStudent(int aiSchoolId, int aiAcademicYearId, int iStudentId, DateTime oLeftDate, char cPermanent_delete, int IsFormNo, int iCancellationFormNo, int aiUpdatedById, bool abIsIncludeinBlackList, string asComment)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolWise_Student_ID", iStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Left_Date", oLeftDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Permanent_Delete", cPermanent_delete, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsForm", IsFormNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CancellationFormNo", iCancellationFormNo, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("IsIncludeinBlackList", abIsIncludeinBlackList, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("Comment", asComment, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteStudent");
            }
        }


        public static DataTable GetAllStudentsByStdDivForMessageFacillity(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, string asName, int aiTypeId, bool abIsForLeftStudents)
        {
            //This function is used to get all the students by Standard and division id.
            if (aiSchoolId == Constants.SchoolId.SPS.ToInt())
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStandardDivisionId, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("Name", asName, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);

                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStudentsByStdDivForMsgFacillity");
                }
            }
            else
            {
                if (abIsForLeftStudents)
                {
                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    {
                        oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                        oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                        oSQLServerDbUtility.AddParameter("StandardDivId", aiStandardDivisionId, SqlDbType.Int);
                        oSQLServerDbUtility.AddParameter("Name", asName, SqlDbType.NVarChar);
                        oSQLServerDbUtility.AddParameter("IsFromPopup", false, SqlDbType.Bit);

                        return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllLeftStudentsForSMS");
                    }
                }
                else
                {
                    string sSqlStatement = String.Format("SELECT a.ID, a.FullName, a.Mobile_Number, a.Mobile_Number2, a.StudentCount, a.Name, a.Roll_No,a.Name as OriginalName " +
                                                         "FROM   vw_FetchSelectedStudentDetails a INNER JOIN User_Master b ON a.ID = b.User_Id " +
                                                         "WHERE  a.School_Id = {0} AND a.Academic_Year_ID = {1} AND a.SchoolLeft_Date IS NULL AND b.IsConsideredForMessage = 1 {2} {3}"
                                                         , aiSchoolId
                                                         , aiAcademicYrId
                                                         , (aiStandardDivisionId != 0 ? "AND a.SchoolWise_Standard_Division_Id = " + aiStandardDivisionId : String.Empty)
                                                         , (String.IsNullOrEmpty(asName) ? String.Empty : String.Format("AND (StudentName LIKE N'%{0}%' OR Enrolment_Number LIKE N'%{0}%' OR Enrolment_Number +' - '+StudentName LIKE N'%{0}%')", StringUtility.ReplaceSingleQuoteInString(asName, false))));

                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                        return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSqlStatement);
                }
            }
        }

        public static DataTable GetAllStudentsByStdDivForMessageFacillity(Int32 aiSchoolId, String asStandardDivisionIds, Int32 aiAcademicYrId, string asName)
        {
            StringBuilder sFilter = new StringBuilder();
            sFilter.Append(" AND SchoolLeft_Date IS NULL ");
            if (asStandardDivisionIds != string.Empty)
            {
                sFilter.Append(" AND [SchoolWise_Standard_Division_Id] IN ( " + asStandardDivisionIds + ")");
            }
            if (!String.IsNullOrEmpty(asName))
            {
                sFilter.Append(" AND (StudentName LIKE '%" + asName + "%' OR Enrolment_Number LIKE '%" + asName + "%' OR Enrolment_Number +' - '+ StudentName LIKE '%" + asName + "%')");
            }

            string sSelectStatement = " SELECT  ID,FullName,Mobile_Number,Mobile_Number2,StudentCount,Name,Roll_No FROM vw_FetchSelectedStudentDetails " +
                                     " WHERE " +
                                     " (School_Id = " + aiSchoolId + ") " +
                                     " AND " +
                                     " (Academic_Year_ID = " + aiAcademicYrId + ") " +
                                     sFilter.ToString();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public static DataTable GetAllStudentsByGivenStdDivs(Int32 aiSchoolId, Int32 aiAcademicYrId, string sStdDivIds, bool abIsLeftStudents)
        {
            //This function is used to get all the students by Standard and division id.

            if (abIsLeftStudents)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Name", sStdDivIds, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("IsFromPopup", true, SqlDbType.Bit);

                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllLeftStudentsForSMS");
                }
            }
            else
            {
                StringBuilder sFilter = new StringBuilder();
                sFilter.Append("AND SchoolLeft_Date IS NULL");
                if (sStdDivIds != string.Empty)
                {
                    sFilter.Append(" AND [SchoolWise_Standard_Division_Id] IN (" + sStdDivIds + ")");
                }

                string sSelectStatement = " SELECT  ID," +
                                          " FullName," +
                                          " Mobile_Number," +
                                          " Mobile_Number2," +
                                          " StudentCount," +
                                          " Name," +
                                          " Roll_No" +
                                          " FROM vw_FetchSelectedStudentDetails " +
                                         " WHERE " +
                                         " (School_Id = " + aiSchoolId + ") " +
                                         " AND " +
                                         " (Academic_Year_ID = " + aiAcademicYrId + ") " +
                                         sFilter.ToString();

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
            }
        }

        public static DataTable GetAllStudentsByStdDivForMessageFacillity(Int32 aiSchoolId, string asUserID, Int32 aiAcademicYrId)
        {
            //This function is used to get all the students by Standard and division id.

            string sSelectStatement = " SELECT  ID,FullName,Mobile_Number,Mobile_Number2,StudentCount,Name,Roll_No FROM vw_FetchSelectedStudentDetails " +
                                     " WHERE " +
                                     " ID IN  (" + asUserID + ") " +
                                     " AND " +
                                     " (School_Id = " + aiSchoolId + ") " +
                                     " AND " +
                                     " (Academic_Year_ID = " + aiAcademicYrId + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public static DataTable GetAllStudentsForMessageFacillity(Int32 aiSchoolId, Int32 aiAcademicYrId)
        {
            //This function is used to get all the students by Standard and division id.
            string sSelectStatement = String.Format("SELECT a.Mobile_Number, a.Mobile_Number2, a.ID " +
                                                    "FROM	vw_FetchSelectedStudentDetails a INNER JOIN User_Master b ON a.ID = b.User_Id " +
                                                    "WHERE	a.School_Id = {0} AND a.Academic_Year_ID = {1} AND a.SchoolLeft_Date IS NULL AND b.IsConsideredForMessage = 1", aiSchoolId, aiAcademicYrId);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public static DataTable GetAllStudentsByStdDivForBookIssue(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, string asName)
        {
            //This function is used to get all the students by Standard and division id for book issue.


            string sFilter = "";
            if (aiStandardDivisionId != 0)
            {
                sFilter = " AND [SchoolWise_Standard_Division_Id] =+ CAST(" + aiStandardDivisionId.ToString() + "AS VARCHAR(15))";
            }
            if (!String.IsNullOrEmpty(asName))
            {
                sFilter = sFilter + " AND (StudentName LIKE '%" + asName + "%' OR Enrolment_Number LIKE '%" + asName + "%')";
            }

            string sSelectStatement = " SELECT  ID,FullName,Mobile_Number,Mobile_Number2,StudentCount,Name,Roll_No FROM vw_FetchSelectedStudentDetails " +
                                     " WHERE " +
                                     " (School_Id = " + aiSchoolId + ") " +
                                     " AND " +
                                     " (Academic_Year_ID = " + aiAcademicYrId + ") " +
                                     " AND " +
                                     " (SchoolLeft_Date IS NULL) " +
                                     sFilter;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public static DataTable GetAllStudentsByStdDivForBookIssue(Int32 aiSchoolId, String asStandardDivisionIds, Int32 aiAcademicYrId, string asName)
        {
            //This function is used to get all the students by Standard and division id for book issue.

            string sFilter = "";
            if (asStandardDivisionIds != string.Empty)
            {
                sFilter = " AND [SchoolWise_Standard_Division_Id] IN ( " + asStandardDivisionIds + ")";
            }
            if (!String.IsNullOrEmpty(asName))
            {
                sFilter = sFilter + " AND (StudentName LIKE N'%" + asName + "%' OR Enrolment_Number LIKE N'%" + asName + "%')";
            }

            string sSelectStatement = " SELECT  ID,FullName,Mobile_Number,Mobile_Number2,StudentCount,Name,Roll_No FROM vw_FetchSelectedStudentDetails " +
                                     " WHERE " +
                                     " (School_Id = " + aiSchoolId + ") " +
                                     " AND " +
                                     " (Academic_Year_ID = " + aiAcademicYrId + ") " +
                                     " AND " +
                                     " (SchoolLeft_Date IS NULL) " +
                                     sFilter;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to check precondition for a standard 
        /// ie fee configuration for particular standard is set or not. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public static string CheckPreConditionAndGetMsg(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            string sSelectStatement = "SELECT COUNT(*) " +
                                        " FROM " +
                                        " Schoolwise_Standard_Fee_Configuration_Master " +
                                      " WHERE " +
                                        " School_Id =" + aiSchoolId +
                                        " AND academic_Year_Id =" + aiAcademicYearId +
                                        " AND Standard_Id =" + aiStandardId +
                                        " AND Is_Deleted =N'" + Constants.C_NO + "'";
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            if (iCount > 0)
                return "true";
            else
                return "false";
        }

        public static DataSet GetNewAdmissionsCount(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiAdmissionType, string asLocationIds, string asResidenceIds)
        {
            StringBuilder sSelectStatement = new StringBuilder();
            string sFilter = string.Empty;
            //if (asResidenceIds.TrimAll() != string.Empty)
            //    sFilter = " AND Student_Admissions.ResidenceTypeId IN(" + asResidenceIds + ")";
            sSelectStatement.Append("SELECT " +
                                  "COUNT(Student_Admissions.Form_Number)" +
                                  " FROM " +
                                  "Student_Admissions INNER JOIN Standard_Master" +
                                  " ON " +
                                  "Student_Admissions.For_Standard = Standard_Master.Standard_Id" +
                                  " AND " +
                                  "Student_Admissions.School_Id = Standard_Master.School_Id" +
                                  " INNER JOIN Salutation_Master" +
                                  " ON " +
                                  "Student_Admissions.Salutation_Id = Salutation_Master.Salutation_Id" +
                                  " WHERE " +
                                  "Student_Admissions.Is_Deleted = 0" +
                                  " AND Student_Admissions.IsAdmitted = 1" +
                                  " AND Student_Admissions.IsConfirmed = 0" +
                                  " AND Student_Admissions.School_Id = " + aiSchoolId +
                                  " AND Student_Admissions.Acedemic_Year_Id = " + aiAcademicYearId +
                                  " AND Student_Admissions.For_Standard =" + aiStandardId +
                                  sFilter +
                                  " AND Student_Admissions.LivingLocationId IN(" + asLocationIds + ")");

            sSelectStatement.Append("SELECT" +
                                   " COUNT(AdmissionProcessId) " +
                                   " FROM " +
                                   "AdmissionProcessDetails" +
                                   " WHERE " +
                                   "School_ID = " + aiSchoolId +
                                   " AND Academic_Year_ID = " + aiAcademicYearId +
                                   " AND Standard_Id = " + aiStandardId +
                                   " AND IsLotteryConfirmed = 1");

            sSelectStatement.Append("SELECT " +
                                     "COUNT(Student_Admissions.Form_Number)" +
                                     " FROM " +
                                     "Student_Admissions INNER JOIN Standard_Master" +
                                     " ON " +
                                     "Student_Admissions.For_Standard = Standard_Master.Standard_Id" +
                                     " AND " +
                                     "Student_Admissions.School_Id = Standard_Master.School_Id" +
                                     " INNER JOIN Salutation_Master" +
                                     " ON " +
                                     "Student_Admissions.Salutation_Id = Salutation_Master.Salutation_Id" +
                                     " WHERE " +
                                     "Student_Admissions.Is_Deleted = 0" +
                                     " AND Student_Admissions.IsAdmitted = 1" +
                                     " AND Student_Admissions.IsConfirmed = 0" +
                                     " AND NOT Student_Admissions.SelectedInLottery IS NULL" +
                                     " AND Student_Admissions.School_Id = " + aiSchoolId +
                                     " AND Student_Admissions.Acedemic_Year_Id = " + aiAcademicYearId +
                                     " AND Student_Admissions.For_Standard =" + aiStandardId);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement.ToString());
        }


        /// <summary>
        /// This method is used to get standarwise fee configuration as per school id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiConfigId"></param>
        /// <returns></returns>
        public static string IsStandardwiseFeeConfig(int aiSchoolId, int aiConfigId)
        {
            string sSelectStatement = "SELECT " +
                                          " Is_Configure" +
                                        " FROM " +
                                          " Configuration_School_Master" +
                                        " WHERE " +
                                        " School_Id =" + aiSchoolId +
                                        " AND Original_Config_Id =" + aiConfigId +
                                        " AND Is_Deleted =N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);
        }

        public static DataTable GetPendingFeeStudentList(Int32 iSchoolId, Int32 iAcademicYearId, string odtStartDate, Int32 aiStandardId, Int32 aiDivisionId, string sRegNo, String sortExpression, int endIndex, int maximumRows, bool abLeftStudent, bool abPDCStudent, int aiFeeTypeId, String asPayableFor, string asOperator, int aiAmount, string asPercentFilter)
        {
            if (sRegNo == null)
                sRegNo = "";

            string IsPDCStudent = abPDCStudent == true ? "Y" : "N";
            string sAmountFilter = CreateAmountFilter(aiFeeTypeId, asOperator, aiAmount);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcadmicYearId", iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Due_Date", odtStartDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("sFilterRegAndName", sRegNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IncludeLeftStudent", abLeftStudent, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("FeeTypeId", aiFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PayableFor", asPayableFor, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sAmountFilter", aiAmount, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsIgnorePDCStudent", IsPDCStudent, SqlDbType.NVarChar);
                if (sortExpression != Constants.S_EMPTY_STRING)
                    oSQLServerDbUtility.AddParameter("sortExp", sortExpression, SqlDbType.NVarChar);
                else
                    oSQLServerDbUtility.AddParameter("sortExp", sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", endIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PazeSize", maximumRows, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PercentFilter", asPercentFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Oprater", asOperator, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_PendingFeeStudents");
            }

        }

        private static string CreateAmountFilter(int aiFeeTypeId, string asOperator, int aiAmount)
        {
            StringBuilder sFilter = new StringBuilder();

            if (asOperator != null && aiAmount >= 0)
                sFilter.Append("Amount " + (asOperator != "--Select--" ? asOperator : ">=") + aiAmount);

            return sFilter.ToString();
        }

        /// <summary>
        /// This method is used to return next user id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static int GetNextLoginId(int aiSchoolId, int aiUserRoleId)
        {
            string sSelectStatement = "SELECT " +
                                      "MAX(User_Master.User_Login)" +
                                      "	FROM " +
                                      "User_Master" +
                                      " INNER JOIN " +
                                      "SchoolWise_Student_Master" +
                                      " ON " +
                                      "User_Master.User_Id = SchoolWise_Student_Master.User_Id" +
                                      "	WHERE " +
                                      "User_Master.User_Role_Id = " + aiUserRoleId +
                                      " AND User_Master.School_Id = " + aiSchoolId +
                                      " AND User_Master.Is_Deleted = 'N'" +
                                      " AND SchoolWise_Student_Master.Is_Deleted = 'N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        /// <summary>
        /// This method is used to get data table to fill Dropdownlist of SORTING FIELD.
        /// </summary>
        /// <returns></returns>   
        public DataTable RetriveSortingField()
        {
            string sSelectStatement = "SELECT " +
                                     " * " +
                                    " FROM " +
                                    "Sorting_Field" +
                                    " WHERE " +
                                    "Is_Deleted='N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }


        public void RegenerateStudentRollNo(int iSchoolId, int iAcadmicYearId, int iStdId, int iDivId, string sFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", iAcadmicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", iStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", iDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sFilter", sFilter, SqlDbType.NVarChar);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GenerateStudentRollNo");
            }
        }

        public DataSet GetPendingFeeDetailsForExport(int aiSchoolId, int aiAcademicYearId, DateTime adtDueDate, string asFilterString, string asStdDivFilter, string asAmountFilter, int aiFeeTypeId, string asIsIgnotePDCStudents, string asSortExpr, int aiStartIndex, int aiPageSize, string asOperator, string asPayableFor)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcadmicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Due_Date", adtDueDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("sFilterRegAndName", asFilterString, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sStdDivFilter", asStdDivFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sAmountFilter", asAmountFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("FeeTypeId", aiFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsIgnorePDCStudent", asIsIgnotePDCStudents, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("sortExp", asSortExpr, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PazeSize", aiPageSize, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Oprater", asOperator, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PayableFor", asPayableFor, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_PendingFeeStudentList");
            }
        }

        #endregion

        /// <summary>
        /// this method is used to get student count details to show on statistics widget.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static StudentCountDetails GetStudentCountDetails(int aiSchoolId, int aiAcademicYearId, bool abIsServiceCall = false)
        {

            StudentCountDetails oStudentCountDetails = new StudentCountDetails();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, aiAcademicYearId, Constants.I_ZERO, abIsServiceCall))
            {

                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_year_Id", aiAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_StudentCountDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oStudentCountDetails = new StudentCountDetails()
                        {
                            GirlsCount = Convert.ToInt16(oSqlDataReader["GirlsCount"]),
                            BoysCount = Convert.ToInt16(oSqlDataReader["BoysCount"]),
                            TotalCount = Convert.ToInt16(oSqlDataReader["Total"]),
                            LeftCount = Convert.ToInt16(oSqlDataReader["LeftStudentCount"]),
                            NewJoinCount = Convert.ToInt16(oSqlDataReader["NewCount"]),
                            RteCount = Convert.ToInt16(oSqlDataReader["RTECount"])
                        };
                    }
                }
            }

            return oStudentCountDetails;
        }

        #endregion

        #region Private functions

        public static List<Operator> GetOperators()
        {
            List<Operator> olstOperators = new List<Operator>();
            olstOperators.Add(new Operator { Value = 1, Text = "=" });
            olstOperators.Add(new Operator { Value = 2, Text = "<" });
            olstOperators.Add(new Operator { Value = 3, Text = "<=" });
            olstOperators.Add(new Operator { Value = 4, Text = ">" });
            olstOperators.Add(new Operator { Value = 5, Text = ">=" });
            return olstOperators;
        }

        /// <summary>
        /// /////////////////////////
        /// </summary>
        /// <returns></returns>
        public void UpdateStudentPhotoDetails()
        {
            string sUpdateStatement;
            sUpdateStatement = " UPDATE SchoolWise_Student_Master SET " +
                               " Photo_File_Path = N'" + StringUtility.ReplaceSingleQuoteInString(moStudentInfo.sPhotoFilePath, true) + "'" +
                                   " `WHERE " +
                                 " School_Id = " + moStudentInfo.SchoolId +

                                 " AND SchoolWise_Student_Id = " + moStudentInfo.iStudentId +
                                 " AND Is_Deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }
        public void UpdateStudentPhoto(Byte[] ImageBinaryData)
        {
            string sSQL = "UPDATE SchoolWise_Student_Master SET Photo_file_Path_Image = @Image , ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                      " WHERE School_Id = " + moStudentInfo.SchoolId.ToString() +
                      " AND SchoolWise_Student_Id = " + moStudentInfo.iStudentId +
                      " AND Is_Deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(ImageBinaryData, sSQL);
        }

        private static string CreateFilterStdDiv(Int32 aiStandardId, Int32 aiDivisionId, bool abLeftStudent)
        {
            StringBuilder sFilter = new StringBuilder();

            if (abLeftStudent == true)
                sFilter.Append("AND SchoolLeft_Date IS NULL ");

            if (aiStandardId == 0)
                return sFilter.ToString();
            else if (aiStandardId != 0 && aiDivisionId == 0)
                sFilter.Append(" AND Standard_Master.Standard_Id=" + aiStandardId);
            else
                sFilter.Append("AND Standard_Master.Standard_Id=" + aiStandardId + " AND Division_Master.Division_Id=" + aiDivisionId);

            return sFilter.ToString();
        }

        // <summary>
        /// This functions create query string with roll no for Year Wise Details insert and return to the caller 
        /// </summary>
        /// <returns></returns>
        private string getinsertStmtForYearWiseDetailsWithRollNo()
        {
            string sInsertString = " INSERT INTO YearWise_Student_Details" +
                          " (Academic_Year_ID" +
                           " , School_Id" +
                           " , Student_Id" +
                           " , Standard_Id" +
                           " , Division_Id " +
                           " , Roll_No " +
                           " , Is_New_Student " +
                           " , Is_RTE_Student " +
                           " , RTECategoryId " +
                           " , Height " +
                           " , Weight " +
                           " , Rule_Id" +
                           " , IsStaffKid" +
                           " , Updated_By_Id" +
                           " , Update_Date" +
                           " , SecondLanguageSubjectId" +
                           " ,ThirdLanguageSubjectId" +
                           " ,IsForDayBoarding" +
                           " ,FeeCategoryDetailsId" +
                           " ,RTEApplicationFormNo " +
                           " , AnnualIncome) " +
                           " VALUES(" +
                           "  N'" + moYearWiseStudentInfo.iYearId + "' " +
                           " , N'" + moStudentInfo.SchoolId + "' " +
                           " , N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                           " , N'" + moYearWiseStudentInfo.iStandardId + "' " +
                           " , N'" + moYearWiseStudentInfo.iDivisionId + "' " +
                           " , N'" + moYearWiseStudentInfo.iRollNo + "' " +
                           " , N'" + moStudentInfo.bIsNewStudent + "' " +
                           " , N'" + moStudentInfo.bIsRTEStudent + "' " +
                            " , N'" + moStudentInfo.iRTECategoryId + "' " +
                           " , N'" + moStudentInfo.dHeight + "' " +
                           " , N'" + moStudentInfo.dWeight + "' " +
                           " , " + moStudentInfo.iRule_Id +
                           " , " + (moStudentInfo.bIsStaffKid ? 1 : 0) +
                           " , " + moStudentInfo.iInsertedById +
                           " , dbo.GetLocalDate(DEFAULT)" +
                           "," + moStudentInfo.iSecondLanguageSubjectId +
                           "," + moStudentInfo.iThirdLanguageSubjectId +
                           " , N'" + moStudentInfo.IsForDayBoarding + "' " +
                           " , N'" + moStudentInfo.FeeCategoryId + "' " +
                            " , N'" + moStudentInfo.sRTEFormNo + "' " +
                             ", N'" + moStudentInfo.sAnnualIncome + "' " +
                           ")";
            return sInsertString;

        }

        /// <summary>
        /// This funtion is used to get next roll no for a student of given division, standered and school.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetNextStudentRollNoAndLogin(int aiStdId, int aiDivId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStdId", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iDivId", aiDivId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetNextStudentRollAndLogin");
            }
        }

        private void InsertStudentSiblingDetails(int aiYrwiseStudentId, string asSiblingStudentIds)
        {
            string[] SiblingStudentIds = asSiblingStudentIds.Split(',');
            if (SiblingStudentIds.Length > 0)
            {
                string sSiblingDetailsXML = "<SiblingDetails>";
                foreach (string SiblingStudent in SiblingStudentIds)
                {
                    if (SiblingStudent != string.Empty)
                        sSiblingDetailsXML += "<SiblingDetails  Yearwise_Student_Id=\"" + aiYrwiseStudentId + "\" YearwiseSiblingStudentId=\"" + SiblingStudent + "\" Insert_Date=\"" + System.DateTime.Now.ToString() + "\" Update_Date=\"" + System.DateTime.Now.ToString() + "\"/>";
                }
                sSiblingDetailsXML += "</SiblingDetails>";
                StudentEntities.StudentInfo oStudentInfo = new StudentEntities.StudentInfo
                {
                    YearwiseStudentId = aiYrwiseStudentId,
                    AcademicYearId = moStudentInfo.iAcademicYearId,
                    SchoolId = moStudentInfo.SchoolId,
                    InsertedById = moStudentInfo.iInsertedById,
                    UpdatedById = moStudentInfo.iInsertedById
                };
                StudentSiblingDetailsDC oStudentSiblingDetailsDC = new StudentSiblingDetailsDC();
                oStudentSiblingDetailsDC.StudentInfoEntity = oStudentInfo;
                oStudentSiblingDetailsDC.SaveStudentSiblingDetails(sSiblingDetailsXML);
            }
        }

        private void InsertSwitchLoginDetails(int iSchoolId, int iYrwiseStudentId, int iParentUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("YearwiseStudentId", iYrwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ParentUserId", iParentUserId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_SaveSwitchLoginDetails");
            }
        }

        #endregion Private functions

        public DataSet getStudentIdentityCards(int iSchoolID, int iAcademicYrID, int mistandardId, int miDivisionId, object miStudentId, string asStudentName, string asStudentReg)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolID", iSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYrID", iAcademicYrID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("istandardId", mistandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iDivisionId", miDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iStudentId", miStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentName", asStudentName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StudentReg", asStudentReg, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_getStudentIdentityCards");
            }
        }

        public static void UpdateAllStudentsLogins(int iSchoolId, string sStudentXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sStudentXml", sStudentXml, SqlDbType.Xml);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateAllStudentLoginsPwd");
            }
        }

        public static DataSet GetAllStudents(int iSchoolId, int iAcademicYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYrId", iAcademicYrId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllSchoolStudents");
            }
        }

        /// <summary>
        /// This function is used to get Standarwise DOB Message.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public static DataTable GetStandardwiseDOBDetails(int aiSchoolId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStandardwiseMinMaxDOB");
            }
        }


        public static DataTable GetAllStudents(int iSchoolId, Char cIsNewStudent, int iAcademicYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsNewStudent", cIsNewStudent, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("iAcademicYrId", iAcademicYrId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllSchoolStudents");
            }
        }


        public string GetFormNumber(int aiSchoolId, int aiStudentId, int iAccYearID)
        {
            string sSelectStatement = "SELECT FormNumber FROM SchoolWise_Student_Master" +
                " WHERE SchoolWise_Student_Master.Is_Deleted='N' " +
                " and SchoolWise_Student_Master.School_Id=" + aiSchoolId +
                " AND SchoolWise_Student_Master.Schoolwise_Student_Id=" + aiStudentId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);

        }



        public int GetFormNoCount(int iCancellationFormNo, int iSchoolId)
        {
            string sSelectStatement = "SELECT COUNT(SchoolWise_Student_Id)  " +
                                        " FROM " +
                                        " SchoolWise_Student_Master " +
                                      " WHERE " +
                                        " School_Id =" + iSchoolId +
                                        " AND CancellationFormNo =" + iCancellationFormNo +
                                        " AND Is_Deleted =N'" + Constants.C_NO + "'";
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iCount;
        }

        public List<StudentEntities.StudentInfo> GetAllStudentForHouseAssignment(int aiSchoolId, int aiAcademicYearID, int aiStandardID, int aiDivisionID, int aiConfigured)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionID", aiDivisionID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Configured", aiConfigured, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentsHouseAssignmentForReport"))
                    return SetStudentHouseDetails(oSqlDataReader);
            };
        }


        public List<StudentEntities.StudentInfo> SetStudentHouseDetails(SqlDataReader oSqlDataReader)
        {
            List<StudentEntities.StudentInfo> lstStudeentDetails = new List<StudentEntities.StudentInfo>();
            StudentEntities.StudentInfo oStudentInfo;
            if (oSqlDataReader.HasRows)
            {
                while (oSqlDataReader.Read())
                {
                    oStudentInfo = new StudentEntities.StudentInfo()
                    {
                        StudentName = oSqlDataReader["StudentName"].ToString(),
                        SchoolwiseStudentId = Convert.ToInt32(oSqlDataReader["SchoolwiseStudentId"]),
                        RegNo = oSqlDataReader["Enrolment_Number"].ToString(),
                        HouseId = Convert.ToInt32(oSqlDataReader["HouseId"]),
                        RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                        HouseColor = oSqlDataReader["Color"].ToString(),
                    };
                    lstStudeentDetails.Add(oStudentInfo);
                }
            }
            return lstStudeentDetails;
        }

        public List<StudentEntities.StudentInfo> GetStudentDetails(int aiSchoolId, int aiAcademicYearID, int aiStandardID, int aiDivisionID)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionID", aiDivisionID, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetailsWithOptionalSubject"))
                    return SetStudentDetails(oSqlDataReader);
            };
        }

        public List<StudentEntities.StudentInfo> SetStudentDetails(SqlDataReader oSqlDataReader)
        {
            List<StudentEntities.StudentInfo> lstStudeentDetails = new List<StudentEntities.StudentInfo>();
            StudentEntities.StudentInfo oStudentInfo;
            if (oSqlDataReader.HasRows)
            {
                while (oSqlDataReader.Read())
                {
                    oStudentInfo = new StudentEntities.StudentInfo()
                    {
                        StudentName = oSqlDataReader["StudentName"].ToString(),
                        SchoolwiseStudentId = Convert.ToInt32(oSqlDataReader["SchoolWise_Student_Id"]),
                        RegNo = oSqlDataReader["Enrolment_Number"].ToString(),
                        SecondLanguageSubjectId = Convert.ToInt32(oSqlDataReader["SecondLanguageSubjectId"]),
                        ThirdLanguageSubjectId = Convert.ToInt32(oSqlDataReader["ThirdLanguageSubjectId"]),
                        RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"])
                    };
                    lstStudeentDetails.Add(oStudentInfo);
                }
            }
            return lstStudeentDetails;
        }

        public List<StudentEntities.StudentInfo> GetAll(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiAssessmentId, string asSortExpression, int aiEndIndex, int aiStartIndex)
        {
            List<StudentEntities.StudentInfo> olstStudentInfo = new List<StudentEntities.StudentInfo>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                asSortExpression = "ORDER BY " + asSortExpression;
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", aiAssessmentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", asSortExpression, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedStudentsForMarkAssignment"))
                {
                    GenericClass<StudentEntities.StudentInfo> oStudentInfo = new GenericClass<StudentEntities.StudentInfo>();
                    olstStudentInfo = oStudentInfo.GetFilledObjectList(oSqlDataReader);

                    if (oSqlDataReader.NextResult())
                        while (oSqlDataReader.Read())
                            miStudentCount = Convert.ToInt32(oSqlDataReader["Count"]);
                }
            };
            return olstStudentInfo;
        }

        public static int GetCount(int aiSchoolId, int aiAcademicYearId, int aiClassTeacherId)
        {
            int iCount = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ClassTeacherID", aiClassTeacherId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CountStudentsForMarkAssignment");
                iCount = Convert.ToInt32(oSqlParameter.Value);
            };
            return iCount;
        }

        public void OverwriteAllSiblingDetails(int aiStudentId, int aiMode, string aiSiblingId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moStudentInfo.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moStudentInfo.iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Mode", aiMode, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SiblingId", aiSiblingId, SqlDbType.Text);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_OverwriteAllSiblingDetails");
            }
        }

        /// <summary>
        /// This method is used to retrive standard wise IDs of RTE student.
        /// </summary>		
        /// <returns></returns>
        public List<int> GetStandardwiseRTEStudentIDs(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            List<int> lstRTEStudIDs = new List<int>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardwiseRTEStudentIDs"))
                {
                    if (oSqlDataReader != null)
                    {
                        while (oSqlDataReader.Read())
                            lstRTEStudIDs.Add(oSqlDataReader["YearWise_Student_Id"].ToInt());
                    }

                }
            }
            return lstRTEStudIDs;
        }

        /// <summary>
        /// This method is used to get new admission count for mid year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static int GetNewAdmissionCount(int aiSchoolId, string asShowStudentAdmission, int aiAcademicYearId)
        {
            int iCount = 0;
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AdmissionForCurrentYear", asShowStudentAdmission, SqlDbType.VarChar);
                oSqlDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetNewAdmissionCount"))
                    if (oReader.HasRows && oReader.Read())
                    {
                        if (oReader["Count"] != DBNull.Value)
                            iCount = oReader["Count"].ToInt();
                    }
            }
            return iCount;
        }


        /// <summary>
        /// This method is used to Deactive Studens Login whose Long Leave Started today.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static void LockLongLeaveUsers(int aiSchoolId)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_LockLongLeaveUsersLogin");
            }
        }

        /// <summary>
        /// This method is used to retrieve student info.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public StudentDetails GetStudentInfo(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader aoSqlDataReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetails"))
                {
                    if (aoSqlDataReader.Read())
                        return ReadObjectFromReader(aoSqlDataReader);
                }
            }
            return null;
        }

        /// <summary>
        /// This method is used to populate object of student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private StudentDetails ReadObjectFromReader(SqlDataReader aoSqlDataReader)
        {
            StudentDetails oStudentDetails = new StudentDetails();
            if (aoSqlDataReader["Name"] != DBNull.Value)
                oStudentDetails.Name = aoSqlDataReader["Name"].ToString();
            if (aoSqlDataReader["Roll_No"] != DBNull.Value)
                oStudentDetails.RollNo = aoSqlDataReader["Roll_No"].ToInt();
            if (aoSqlDataReader["SchoolWise_Standard_Division_Id"] != DBNull.Value)
                oStudentDetails.SchoolwiseStandardDivisionId = aoSqlDataReader["SchoolWise_Standard_Division_Id"].ToInt();
            if (aoSqlDataReader["Is_Locked"] != DBNull.Value)
                oStudentDetails.IsLock = aoSqlDataReader["Is_Locked"].ToString();
            if (aoSqlDataReader["Mobile_Number"] != DBNull.Value)
                oStudentDetails.MobileNo1 = aoSqlDataReader["Mobile_Number"].ToString();
            if (aoSqlDataReader["YearWise_Student_Id"] != DBNull.Value)
                oStudentDetails.YearwiseStudentId = aoSqlDataReader["YearWise_Student_Id"].ToInt();
            if (aoSqlDataReader["Is_New_Student"] != DBNull.Value)
                oStudentDetails.IsNewStudent = aoSqlDataReader["Is_New_Student"].ToInt();
            if (aoSqlDataReader["DOB"] != DBNull.Value)
                oStudentDetails.DOB = aoSqlDataReader["DOB"].ToDateTime();
            if (aoSqlDataReader["Enrolment_Number"] != DBNull.Value)
                oStudentDetails.EnrollmentNo = aoSqlDataReader["Enrolment_Number"].ToString();
            if (aoSqlDataReader["User_Id"] != DBNull.Value)
                oStudentDetails.UserId = aoSqlDataReader["User_Id"].ToInt();
            if (aoSqlDataReader["SchoolWise_Student_Id"] != DBNull.Value)
                oStudentDetails.SchoolwiseStudentId = aoSqlDataReader["SchoolWise_Student_Id"].ToInt();
            if (aoSqlDataReader["StandardDivision"] != DBNull.Value)
                oStudentDetails.StandrdDivision = aoSqlDataReader["StandardDivision"].ToString();
            if (aoSqlDataReader["Is_Leave"] != DBNull.Value)
                oStudentDetails.IsLeave = aoSqlDataReader["Is_Leave"].ToString();
            if (aoSqlDataReader["Standard_Id"] != DBNull.Value)
                oStudentDetails.StandrdId = aoSqlDataReader["Standard_Id"].ToInt();
            if (aoSqlDataReader["Division_id"] != DBNull.Value)
                oStudentDetails.DivisionId = aoSqlDataReader["Division_id"].ToInt();
            if (aoSqlDataReader["Photo_file_Path"] != DBNull.Value)
                oStudentDetails.PhotoFilePath = aoSqlDataReader["Photo_file_Path"].ToString();
            if (aoSqlDataReader["HasDebitEntries"] != DBNull.Value)
                oStudentDetails.HasDebitEntries = Convert.ToBoolean(aoSqlDataReader["HasDebitEntries"]);
            return oStudentDetails;
        }

        /// <summary>
        /// This method is sued to return student mandatory fields.
        /// </summary>
        /// <returns></returns>
        public string GetStudentMandatoryFields(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentMandatoryFields"))
                {
                    List<string> lstFields = new List<string>();
                    while (oSqlDataReader.Read())
                        lstFields.Add(oSqlDataReader["FieldName"].ToString());
                    return string.Join(",", lstFields);
                }
            }
        }

        public static StudentDetailsForSMS GetStudentDetailsForSMS(int aiSchoolId, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetailsForSMS"))
                {
                    StudentDetailsForSMS oStudentDetailsForSMS = new StudentDetailsForSMS();
                    if (oSqlDataReader.Read())
                    {
                        oStudentDetailsForSMS.MobileNo1 = Convert.ToString(oSqlDataReader["Mobile_Number"]);
                        oStudentDetailsForSMS.MobileNo2 = Convert.ToString(oSqlDataReader["Mobile_Number2"]);
                        oStudentDetailsForSMS.StudentName = Convert.ToString(oSqlDataReader["StudentName"]);
                        oStudentDetailsForSMS.UserId = Convert.ToInt32(oSqlDataReader["User_Id"]);
                    }

                    return oStudentDetailsForSMS;
                }
            }
        }

        /// <summary>
        /// This method is used to return student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        public Student GetStudentDetails(int aiSchoolId, int aiAcademicYearId, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetailsByRegNo"))
                {
                    Student oStudent = new Student();
                    if (oSqlDataReader.Read())
                    {
                        oStudent.RegistraionNo = Convert.ToString(oSqlDataReader["Enrolment_Number"]);
                        oStudent.StudentId = Convert.ToInt32(oSqlDataReader["Schoolwise_Student_Id"]);
                        oStudent.Name = Convert.ToString(oSqlDataReader["StudentName"]);
                        oStudent.StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"]);
                        oStudent.DivisionId = Convert.ToInt32(oSqlDataReader["Division_Id"]);
                        oStudent.StdDivId = Convert.ToInt32(oSqlDataReader["Schoolwise_Standard_Division_Id"]);
                        oStudent.YearWiseStudentId = Convert.ToInt32(oSqlDataReader["Yearwise_Student_Id"]);
                        oStudent.ClassName = Convert.ToString(oSqlDataReader["ClassName"]);
                    }
                    return oStudent;
                }
            }
        }

        /// <summary>
        /// This Method is used to get Student userId from his yearwise academic YearId.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public int GetStudentUserId(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            int userId = 0;
            string sSelectStatement = " SELECT User_Id  FROM SchoolWise_Student_Master " +
                                      " INNER JOIN YearWise_Student_Details " +
                                      " ON YearWise_Student_Details.Student_Id =  SchoolWise_Student_Master.SchoolWise_Student_Id " +
                                      " WHERE YearWise_Student_Details.YearWise_Student_Id =" + aiStudentId +
                                      " AND YearWise_Student_Details.Is_Deleted = 'N'";

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
            {
                while (oSqlDataReader.Read())
                    return userId = oSqlDataReader["User_Id"].ToInt();
            }
            return userId;
        }

        /// <summary>
        /// This method is used to return student count details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<StandardwiseStudentCount> GetStandardwiseStudentCountDetails(int aiSchoolId, int aiAcademicYearId)
        {
            List<StandardwiseStudentCount> lstStandardwiseStudentCount = new List<StandardwiseStudentCount>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentCountDetails");
                while (oSqlDataReader.Read())
                {
                    StandardwiseStudentCount oStandardwiseStudentCount = new StandardwiseStudentCount
                    {
                        Date = oSqlDataReader["Date"].ToDateTime(),
                        Header = oSqlDataReader["Header"].ToString(),
                        IsNewStudent = oSqlDataReader["IsNewStudent"].ToBool(),
                        IsRteStudent = oSqlDataReader["IsRteStudent"].ToBool(),
                        MonthId = oSqlDataReader["MonthId"].ToInt(),
                        Sex = Convert.ToChar(oSqlDataReader["Sex"]),
                        StandardId = oSqlDataReader["StandardId"].ToInt(),
                        StudentCount = oSqlDataReader["StudentCount"].ToInt(),
                        Year = oSqlDataReader["Year"].ToInt(),
                        StandardName = oSqlDataReader["StandardName"].ToString(),
                        IsStartingCount = oSqlDataReader["IsStartingCount"].ToBool(),
                        IsStudentRepeatingClass = oSqlDataReader["IsStudentRepeatingClass"].ToBool()
                    };
                    lstStandardwiseStudentCount.Add(oStandardwiseStudentCount);
                }
                return lstStandardwiseStudentCount;
            }
        }

        /// <summary>
        /// This method is sued to generate transport fee entries.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolwiseStudentId"></param>
        public void GenerateTrasnportFeeEntry(int aiSchoolId, int aiAcademicYearId, int aiSchoolwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GenerateTransportFeeEntries");
            }
        }

        /// <summary>
        /// This Method is used to get Students parent details for uploading photos.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public DataTable GetStudentParentPhotoDetails(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStudentParentPhotoDetails");
            }
        }

        /// <summary>
        /// This Method is used to get Students parent details for uploading photos.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public DataTable GetTransportPickUpPersonPhotoDetails(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTransportPickUpPersonPhotoDetails");
            }
        }

        /// <summary>
        /// This method is used to Save student parents photos.
        /// </summary>
        /// <param name="oStudentParentDetails"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        public void SaveStudentParentPhotoDetails(StudentAdditionalDetails oStudentAdditionalDetails, int aiSchoolId, int aiUserId, bool abSaveForSibling, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FatherPhoto", oStudentAdditionalDetails.FatherBinaryPhoto, SqlDbType.Image);
                oSQLServerDbUtility.AddParameter("MotherPhoto", oStudentAdditionalDetails.MotherBinaryPhoto, SqlDbType.Image);
                oSQLServerDbUtility.AddParameter("ParentPhoto", oStudentAdditionalDetails.ParentBinaryPhoto, SqlDbType.Image);
                oSQLServerDbUtility.AddParameter("FatherImgPhoto", oStudentAdditionalDetails.FatherPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("MotherImgPhoto", oStudentAdditionalDetails.MotherPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("LocalGuardianPhoto", oStudentAdditionalDetails.GuardianPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", Constants.I_ZERO, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RelativeName", oStudentAdditionalDetails.RelativeName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SaveForSibling", abSaveForSibling, SqlDbType.Bit);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentParentPhotos");
            }
        }

        /// <summary>
        /// This method is used to Submit the Parent photos.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        public void SubmitStudentParentPhotoDetails(int aiUserId, int aiSchoolId, int aiAcademicYearId, bool abSubmitForSibling)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", Constants.I_ONE, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SaveForSibling", abSubmitForSibling, SqlDbType.Bit);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentParentPhotos");
            }
        }

        /// <summary>
        /// This method is used to Get Student details for update Email address in bulk.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public List<StudentsBulkEmail> GetStudentDetailsForBulkEmail(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId)
        {
            List<StudentsBulkEmail> lstStudentsBulkEmail = new List<StudentsBulkEmail>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentsForBulkEmail"))
                {
                    while (oSqlDataReader.Read())
                    {
                        StudentsBulkEmail oStudentsBulkEmail = new StudentsBulkEmail();
                        oStudentsBulkEmail.StudentId = oSqlDataReader["Student_Id"].ToInt();
                        oStudentsBulkEmail.RegNo = oSqlDataReader["Enrolment_Number"].ToString();
                        oStudentsBulkEmail.RollNo = oSqlDataReader["Roll_No"].ToInt();
                        oStudentsBulkEmail.StudentName = oSqlDataReader["StudentName"].ToString();
                        oStudentsBulkEmail.StandardId = oSqlDataReader["Standard_Id"].ToInt();
                        oStudentsBulkEmail.DivisionId = oSqlDataReader["Division_id"].ToInt();
                        oStudentsBulkEmail.EmailAddress = oSqlDataReader["EmailAddress"].ToString();

                        lstStudentsBulkEmail.Add(oStudentsBulkEmail);
                    }
                }
                return lstStudentsBulkEmail;
            }
        }

        /// <summary>
        /// This method is used to Update Email Address in DB.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asStudentEmailDetails"></param>
        public void UpdateStudentsEmailInBulk(int aiSchoolId, int aiUserId, string asStudentEmailDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EmailDetails", asStudentEmailDetails, SqlDbType.Xml);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStudentsEmailInBulk");
            }
        }

        /// <summary>
        /// This method is used to Save student parents photos.
        /// </summary>
        /// <param name="oStudentParentDetails"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        public void SaveTransportPickUpPersonPhotoDetails(StudentAdditionalDetails oStudentAdditionalDetails, int aiSchoolId, int aiUserId, bool abSaveForSibling, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", Constants.I_ZERO, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SaveForSibling", abSaveForSibling, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("TransportPickUpPersonName", oStudentAdditionalDetails.TransportPickUpPersonName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TransportPickUpPersonPhoto", oStudentAdditionalDetails.TransportPickUpPersonPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TransportPickUpPersonImgPhoto", oStudentAdditionalDetails.TransportPickUpPersonBinartPhoto, SqlDbType.Image);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveTransportPickUpPersonPhoto");
            }
        }

        /// <summary>
        /// This method is used to Submit the Parent photos.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        public void SubmitTransportPickUpPersonPhotoDetails(int aiUserId, int aiSchoolId, int aiAcademicYearId, bool abSubmitForSibling)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsSubmit", Constants.I_ONE, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SaveForSibling", abSubmitForSibling, SqlDbType.Bit);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveTransportPickUpPersonPhoto");
            }
        }

        /// <summary>
        /// This method is used to check invalid reg. Nos.
        /// </summary>
        /// <param name="sRegNos"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<StudentPhoto> GetNonValidRegNos(string sRegNos, int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<StudentPhoto> lstRegNos = new List<StudentPhoto>();
                oSQLServerDbUtility.AddParameter("RegNos", sRegNos, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetNonValidRegNos"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstRegNos.Add(new StudentPhoto
                        {

                            RegNo = oSqlDataReader["RegNo"].ToString(),
                            SchoolwiseStudentId = oSqlDataReader["SchoolWise_Student_Id"].ToInt()
                        });
                    }
                }
                return lstRegNos;
            }
        }

        /// <summary>
        /// This method is used to upload photos.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiInsertedById"></param>
        /// <param name="alstRegNos"></param>
        public void UpdatePhotos(int aiSchoolId, int aiAcademicYearId, int aiInsertedById, List<StudentPhoto> alstRegNos)
        {
            List<int> lstStudentIds = alstRegNos.Select(reg => reg.SchoolwiseStudentId).ToList();
            string sStudentIDs = string.Join(",", lstStudentIds);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentIDs", sStudentIDs, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_CopyStudentPhotos");
            }

            foreach (var photo in alstRegNos)
            {
                string sSQL = "UPDATE SchoolWise_Student_Master " +
                " SET Photo_file_Path_Image = @Image" +
                 ", ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                 " WHERE School_Id = " + aiSchoolId +
                 " AND SchoolWise_Student_Id =" + photo.SchoolwiseStudentId +
                 " AND Is_Deleted = '" + Constants.C_NO + "'";

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    oSQLServerDbUtility.ExecuteTransaction(photo.PhotoInBinary, sSQL);
            }
        }

        public string ValidateTransferStudent(int aiSchoolId, int aiAcademicYearId, string asStudentIdsXML, int aiDivisionId, int aiStandardId)
        {
            string sStudentNames = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentIds", asStudentIdsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ValidateStudentTransfer"))
                {
                    if (oSqlDataReader.Read())
                        sStudentNames = oSqlDataReader["StudentName"].ToString();
                }
            }
            return sStudentNames;
        }

        public string GetStudentName(int aiSchoolwiseStudentId)
        {
            string sSelectStatement = " SELECT " +
                                        " StudentName " +
                                      " FROM " +
                                        " vw_BaseStudentDetails" +
                                   " WHERE " +
                                        " SchoolWise_Student_Id = " + aiSchoolwiseStudentId;
            string sName = string.Empty;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                DataTable oDT = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
                if (oDT.Rows.Count > 0 && oDT.Rows[0][0] != DBNull.Value)
                    sName = oDT.Rows[0][0].ToString();
            }

            return sName;
        }
		
		/// <summary>
        /// This method is used to delete student from blacklist.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiId"></param>
        /// <param name="aiUpdatedById"></param>
        public void UpdateBlackListStudent(int aiSchoolId, int aiId, int aiUpdatedById, int aiActionId, int aiSchoolwiseStudentId, string asComment)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ActionId", aiActionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Comment", asComment, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_UpdateBlackListStudent");
            }
        }

        public List<BlackListedStudent> GetAllBlackListedStudents(int aiSchoolId, string asFilter, bool abShowAllLeft, string SortExpression, string SortDirection, int StartRowIndex, int iEndRowIndex)
        {
            List<BlackListedStudent> lstBlackListStudent = new List<BlackListedStudent>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentName", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ShowAllLeft", abShowAllLeft, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("SortExpression", SortExpression, SqlDbType.NVarChar);
                //oSQLServerDbUtility.AddParameter("SortDirection", SortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartRowIndex", StartRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndRowIndex", iEndRowIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllBlackListedStudents"))
                {
                    if (oSqlDataReader != null)
                    {
                        while (oSqlDataReader.Read())
                        {
                            BlackListedStudent oBlackListedStudent = new BlackListedStudent
                            {
                                Id = Convert.ToInt32(oSqlDataReader["Id"]),
                                EnrolmentNumber = Convert.ToString(oSqlDataReader["Enrolment_Number"]),
                                StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                                SchoolLeftDate = (Convert.ToDateTime(oSqlDataReader["SchoolLeft_Date"])).ToString(Constants.S_DATE_FORMAT),
                                Comment = Convert.ToString(oSqlDataReader["Comment"]),
                                SchoolwiseStudentId = Convert.ToInt32(oSqlDataReader["SchoolwiseStudentId"]),
                                TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"])
                            };
                            lstBlackListStudent.Add(oBlackListedStudent);
                        }

                    }
                }
            }
            return lstBlackListStudent;
        }
    }

        public class StudentCollectionDC
        {
            private int miSchoolId;
            private int miAcadamicId;
            private bool mbConsiderLeftStudent;

            public StudentCollectionDC()
            {
            }

            public StudentCollectionDC(int aiSchoolId, int aiAcademicId)
            {
                miSchoolId = aiSchoolId;
                miAcadamicId = aiAcademicId;
            }

            public StudentCollectionDC(int aiSchoolId, int aiAcademicId, bool bConsiderLeftStudent)
            {
                miSchoolId = aiSchoolId;
                miAcadamicId = aiAcademicId;
                mbConsiderLeftStudent = bConsiderLeftStudent;
            }

            public string InsertMultipleStudents(int aiSchoolId, int aiAcademicYearId, int aiUserId,
                                                int aiStandardId, int aiDivisionId, string asStudentDetails)
            {
                string sIsStudentLocked = Constants.S_NO;
                if (!ConfigurationManager.AppSettings["EnableStudentLogin"].IsNullOrEmpty() && ConfigurationManager.AppSettings["EnableStudentLogin"].Trim() == Constants.S_NO)
                    sIsStudentLocked = Constants.S_YES;

                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Inserted_By_Id", aiUserId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("StudentDetails", asStudentDetails, SqlDbType.Xml);
                    oSQLServerDbUtility.AddParameter("IsStudentLocked", sIsStudentLocked, SqlDbType.Char);
                    SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("RTEStudIDs", string.Empty, SqlDbType.NVarChar, ParameterDirection.Output, 4000);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AddMultpleStudentDetails");
                    return Convert.ToString(oSqlParameter.Value);
                }
            }
            public DataTable InsertMultipleStudents(int aiSchoolId, int aiAcademicYearId, int aiUserId,
                                                int aiStandardId, int aiDivisionId, string asStudentDetails, int aiUserRoleId, string asShowAdmissionForCurrentYear, bool abKeyValue)
            {
                string sIsStudentLocked = Constants.S_NO;
                if (!ConfigurationManager.AppSettings["EnableStudentLogin"].IsNullOrEmpty() && ConfigurationManager.AppSettings["EnableStudentLogin"].Trim() == Constants.S_NO)
                    sIsStudentLocked = Constants.S_YES;
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Inserted_By_Id", aiUserId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("StudentDetails", asStudentDetails, SqlDbType.Xml);
                    oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("AdmissionForCurrentYear", asShowAdmissionForCurrentYear, SqlDbType.VarChar);
                    oSQLServerDbUtility.AddParameter("IsStudentLocked", sIsStudentLocked, SqlDbType.Char);
                    oSQLServerDbUtility.AddParameter("KeyValue", abKeyValue, SqlDbType.Bit);
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_InsertStudentsDetails", true);
                }
            }

            /// <summary>
            /// This method is used to get list of students for selected class teacher.
            /// </summary>
            /// <param name="aiTeacherId"></param>
            /// <returns></returns>
            public DataTable GetStudentListOfGivenClassTeacher(int aiStdDivId)
            {
                string sFilter = string.Empty;
                if (mbConsiderLeftStudent)
                    sFilter = " AND SchoolLeft_Date IS NULL ";

                String SQLstr = "SELECT * FROM vw_StudentListOfGivenClassTeacher" +
                              " WHERE " +
                                      " (Academic_Year_Id = N'" + miAcadamicId + "')" +
                                      " AND (School_Id = N'" + miSchoolId + "')" +
                                      " AND (SchoolWise_Standard_Division_Id = N'" + aiStdDivId + "')" +
                                      sFilter +
                                      " ORDER BY roll_no";
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(SQLstr);
            }
            public DataSet GetClassStudentsAssociation()
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcadamicId, SqlDbType.Int);

                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_ClassStudentsAssociation");
                }

            }
            public DataTable GetBirthDayReminder()
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcadamicId, SqlDbType.Int);

                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_BirthdayReminder");
                }

            }


            public DataTable GetStudentListOfGivenStdDiv(int aiStdDivId)
            {
                String SQLstr = "SELECT DISTINCT Student_Id,roll_no, Student_Name" +
                               " FROM vw_StudentListOfGivenClassTeacher" +
                               " WHERE " +
                               " (Academic_Year_Id = N'" + miAcadamicId + "')" +
                               " AND (School_Id = N'" + miSchoolId + "')" +
                               " AND (SchoolWise_Standard_Division_Id = N'" + aiStdDivId + "')" +
                               " GROUP BY Teacher_Id,Student_Id,Student_Name,roll_no" +
                               " ORDER BY roll_no";
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                    return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(SQLstr);
            }

            /// <summary>
            /// Returns the total count of students in the school, including left students.
            /// </summary>
            /// <returns></returns>
            public int GetStudentCount()
            {
                string sSqlStatement = String.Format("SELECT SUM(studentCount) AS TotalCount FROM vw_classStudents WHERE School_Id = {0} AND academic_Year_Id = {1}",
                                                      miSchoolId,
                                                      miAcadamicId);
                using (SQLServerDbUtility oSqlDbUtility = new SQLServerDbUtility())
                    return oSqlDbUtility.PerformIntQueryOnSqlServer(sSqlStatement);
            }

            public StudentMISDetails GetStudentAttendanceDetails(DateTime adtAttendanceDate)
            {
                StudentMISDetails oStudentDetails = null;

                using (var oSqlDbUtility = new SQLServerDbUtility())
                {
                    oSqlDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                    oSqlDbUtility.AddParameter("AcademicYearId", miAcadamicId, SqlDbType.Int);
                    oSqlDbUtility.AddParameter("AttendanceDate", adtAttendanceDate, SqlDbType.DateTime);

                    using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentAttendanceDetails"))
                        if (oReader.HasRows && oReader.Read())
                        {
                            oStudentDetails = new StudentMISDetails
                                {
                                    TotalCount = oReader["TotalStudents"].ToInt(),
                                    TotalAttendanceCount = oReader["PresentStudents"].ToInt()
                                };
                        }
                }
                return oStudentDetails;
            }
    }
}

