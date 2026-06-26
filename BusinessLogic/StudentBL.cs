/*
 * File Name         :- StudentBL.cs
 * Purpose           :- This Class is used as an interface between the UILayer and DCLayer.
 *                      Actual business logic is present in this class.
 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using BusinessLogic.Exceptions;
using DataCommunicator;
using Management.Entities;
using StudentEntities;
using Utility;
using SchoolEntities;
using SchoolEntities.Admin;
using SchoolEntities.Common;

namespace BusinessLogic
{
    public class StudentBL : BusinessLogicBaseBL
    {
        #region DataMembers & Properties

        #region DataMembers
        StudentDC moStudentDC;
        SchoolUserBL moSchoolUserBL;
        StudentDC.StudentInfo moStudentInfo;
        StudentDC.YearWiseStudentInfo moYrWiseStudentInfo;
		static int RowCount;
		int miTotalRows;

        #endregion

        #region Properties

        #region Student Basic details

        public SchoolUserBL StudentUserBL
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

        public bool IsNewStudent
        {
            get
            {
                return moStudentInfo.bIsNewStudent;
            }
            set
            {
                moStudentInfo.bIsNewStudent = value;
            }
        }

		public bool IsRTEStudent
		{
			get
			{
				return moStudentInfo.bIsRTEStudent;
			}
			set
			{
				moStudentInfo.bIsRTEStudent = value;
			}
		}

        public int Rule_Id
        {
            get
            {
                return moStudentInfo.iRule_Id;
            }
            set
            {
                moStudentInfo.iRule_Id = value;
            }
        }

        public bool IsStaffKid
        {
            get
            {
                return moStudentInfo.bIsStaffKid;
            }
            set
            {
                moStudentInfo.bIsStaffKid = value;
            }
        }

        public Int32 StudentId
        {
            get
            {
                return moStudentInfo.iStudentId;

            }
            set
            {
                moStudentInfo.iStudentId = value;
            }
        }
        public string SalutationName
        {
            get
            {
                return moStudentInfo.sSalutationName;

            }
            set
            {
                moStudentInfo.sSalutationName = value;
            }
        }
        public Int32 SalutationId
        {
            get
            {
                return moStudentInfo.iSalutationId;

            }
            set
            {
                moStudentInfo.iSalutationId = value;
            }
        }
        public string PhotoFilePath
        {
            get
            {
                return moStudentInfo.sPhotoFilePath;

            }
            set
            {
                moStudentInfo.sPhotoFilePath = value;
            }
        }
        public Byte[] PhotoFilePathInBinary
        {
            get
            {
                return moStudentInfo.sPhotoFilePathInBinary;

            }
            set
            {
                moStudentInfo.sPhotoFilePathInBinary = value;
            }
        }
        public Int32 SchoolId
        {
            get
            {
                return moStudentInfo.SchoolId;
            }
            set
            {
                moStudentInfo.SchoolId = value;
            }
        }
        public Int32 UserId
        {
            get
            {
                return moStudentInfo.iUser_Id;
            }
            set
            {
                moStudentInfo.iUser_Id = value;
            }
        }
        
        public string Email
        {
            get
            {
                return moStudentInfo.sEmail;
            }
            set
            {
                moStudentInfo.sEmail = value;
            }
        }

        public string FirstName
        {
            get
            {
                return moStudentInfo.sFirstName;
            }
            set
            {
                moStudentInfo.sFirstName = value;
            }
        }
        public string MiddleName
        {
            get
            {
                return moStudentInfo.sMiddleName;
            }
            set
            {
                moStudentInfo.sMiddleName = value;
            }
        }
        public string LastName
        {
            get
            {
                return moStudentInfo.sLastName;
            }
            set
            {
                moStudentInfo.sLastName = value;
            }
        }
        public string MotherName
        {
            get
            {
                return moStudentInfo.sMother_Name;
            }
            set
            {
                moStudentInfo.sMother_Name = value;
            }
        }

        public string BloodGroup
        {
            get
            {
                return moStudentInfo.sBloodGroup;
            }
            set
            {
                moStudentInfo.sBloodGroup = value;
            }
        }

        public char Sex
        {
            get
            {
                return moStudentInfo.cSex;
            }
            set
            {
                moStudentInfo.cSex = value;
            }
        }
		
		public DateTime Dob
		{
			get
			{
				return moStudentInfo.dDob;
			}
			set
			{
				moStudentInfo.dDob = value;
			}
		}
		public string BirthPlace
		{
			get
			{
				return moStudentInfo.sBirthPlace;
			}
			set
			{
				moStudentInfo.sBirthPlace = value;
			}
		}
		public string Nationality
		{
			get
			{
				return moStudentInfo.sNationality;
			}
			set
			{
				moStudentInfo.sNationality = value;
			}
		}
        public DateTime AdmissionDate
        {
            get
            {
                return moStudentInfo.dAdmissionDate;
            }
            set
            {
                moStudentInfo.dAdmissionDate = value;
            }
        }

        public DateTime JoiningDate
        {
            get
            {
                return moStudentInfo.dJoining_Date;
            }
            set
            {
                moStudentInfo.dJoining_Date = value;
            }
        }

        public DateTime dLeftDate
        {
            get
            {
                return moStudentInfo.dLeftDate;
            }
            set
            {
                moStudentInfo.dLeftDate = value;
            }
        }

        public int CancellationFormNo
        {
            get
            {
                return moStudentInfo.CancellationFormNo;
            }
            set
            {
                moStudentInfo.CancellationFormNo = value;
            }
        }

        public string PaentName
        {
            get
            {
                return moStudentInfo.sParentName;
            }
            set
            {
                moStudentInfo.sParentName = value;
            }
        }
        public int ParentOcupation
        {
            get
            {
                return moStudentInfo.iParentOcupation;
            }
            set
            {
                moStudentInfo.iParentOcupation = value;
            }
        }

        public string ParentOtherOcupation
        {
            get
            {
                return moStudentInfo.sOtherOcupation;
            }
            set
            {
                moStudentInfo.sOtherOcupation = value;
            }
        }

        public string Address
        {
            get
            {
                return moStudentInfo.sAddress;
            }
            set
            {
                moStudentInfo.sAddress = value;
            }
        }

       public string BusPickupCity
        {
            get
            {
                return moStudentInfo.sBusPickupCity;
            }
            set
            {
                moStudentInfo.sBusPickupCity = value;
            }
        }
        public string City
        {
            get
            {
                return moStudentInfo.sCity;
            }
            set
            {
                moStudentInfo.sCity = value;
            }
        }
      
        public string PinCode
        {
            get
            {
                return moStudentInfo.sPincode;
            }
            set
            {
                moStudentInfo.sPincode = value;
            }
        }
        public string State
        {
            get
            {
                return moStudentInfo.sState;
            }
            set
            {
                moStudentInfo.sState = value;
            }
        }
        public Int32 CategoryId
        {
            get
            {
                return moStudentInfo.iCategoryId;
            }
            set
            {
                moStudentInfo.iCategoryId = value;
            }
        }
		public int RTECategoryId
		{
			get
			{
				return moStudentInfo.iRTECategoryId;
			}
			set
			{
				moStudentInfo.iRTECategoryId = value;
			}
		}
        //new code addded
        public string RTEFormNo
        {
            get 
            {
                return moStudentInfo.sRTEFormNo;
            
            }
            set
            {
                moStudentInfo.sRTEFormNo = value;
            }
        
        }
        public int AnnualIncome
        {
            get
            {
                return moStudentInfo.sAnnualIncome;

            }
            set
            {
                moStudentInfo.sAnnualIncome = value;
            }

        }

		public string EnrolementNo
        {
            get
            {
                return moStudentInfo.sEnrollmentNo;
            }
            set
            {
                moStudentInfo.sEnrollmentNo = value;
            }
        }

        public string LoginName
        {
            get
            {
                return moStudentInfo.sLoginName;
            }
            set
            {
                moStudentInfo.sLoginName = value;
            }
        }

        public string CasteAndSubCaste
        {
            get
            {
                return moStudentInfo.sCasteAndSubCaste;
            }
            set
            {
                moStudentInfo.sCasteAndSubCaste = value;
            }
        }
        public string ResidencePhoneNo
        {
            get
            {
                return moStudentInfo.sResidencePhoneNo;
            }
            set
            {
                moStudentInfo.sResidencePhoneNo = value;
            }
        }
        public string Religion
        {
            get
            {
                return moStudentInfo.sReligion;
            }
            set
            {
                moStudentInfo.sReligion = value;
            }
        }
        public bool AreAdditionalDetailsApplicable
        {
            get
            {
                return moStudentInfo.AreAdditionalDetailsApplicable;
            }
            set
            {
                moStudentInfo.AreAdditionalDetailsApplicable = value;
            }
        }
        public string Category
        {
            get
            {
                return moStudentInfo.sCategory;
            }
            set
            {
                moStudentInfo.sCategory = value;
            }
        }
        public string UDISEnumber
        {
            get
            {
                return moStudentInfo.sUDISEnumber;
            }
            set
            {
                moStudentInfo.sUDISEnumber = value;
            }
        }
        public string MobilePhoneNo
        {
            get
            {
                return moStudentInfo.sMobilePhoneNo;
            }
            set
            {
                moStudentInfo.sMobilePhoneNo = value;
            }
        }
        public string PlaceOfBirth
        {
            get
            {
                return moStudentInfo.sPlaceOfBirth;
            }
            set
            {
                moStudentInfo.sPlaceOfBirth = value;
            }
        }

        public string MobilePhoneNo2
        {
            get
            {
                return moStudentInfo.sMobilePhoneNo2;
            }
            set
            {
                moStudentInfo.sMobilePhoneNo2 = value;
            }
        }
        public string AadharCardNo
        {
            get
            {
                return moStudentInfo.sAadharCardNo;
            }
            set
            {
                moStudentInfo.sAadharCardNo = value;
            }
        }

        public string StudentNameAadharCard
        {
            get
            {
                return moStudentInfo.sNameOnAadharCard;
            }
            set
            {
                moStudentInfo.sNameOnAadharCard = value;
            }
        }

        public string AadharCardNumberPhotoCopyName
        {
            get
            {
                return moStudentInfo.sAadharCardNumberPhotoCopyName;
            }
            set
            {
                moStudentInfo.sAadharCardNumberPhotoCopyName = value;
            }
        }

        public string Family_Photo_Copy_Path
        {
            get
            {
                return moStudentInfo.sFamilyPhoto;
            }
            set
            {
                moStudentInfo.sFamilyPhoto = value;
            }
        }
        public string CasteCertificate_Photo_Copy_Path
        {
            get
            {
                return moStudentInfo.sCasteCertPhoto;
            }
            set
            {
                moStudentInfo.sCasteCertPhoto = value;
            }
        }

        public string MotherTongue
        {
            get
            {
                return moStudentInfo.sMotherTongue;
            }
            set
            {
                moStudentInfo.sMotherTongue = value;
            }
        }


        public Int32 UpdatedBY
        {
            get
            {
                return moStudentInfo.iUpdatedById;
            }
            set
            {
                moStudentInfo.iUpdatedById = value;
            }
        }
        public Int32 InsertedBY
        {
            get
            {
                return moStudentInfo.iInsertedById;
            }
            set
            {
                moStudentInfo.iInsertedById = value;
            }
        }
        public char IsLeave
        {
            get
            {
                return moStudentInfo.mcIsLeave;
            }
            set
            {
                moStudentInfo.mcIsLeave = value;
            }

        }

        public string DateOfBirthInText
        {
            get
            {
                return moStudentInfo.sDateOfBirthInText;
            }
            set
            {
                moStudentInfo.sDateOfBirthInText = value;
            }
        }

        public string StandardName
        {
            get
            {
                return moStudentInfo.sStandardName;
            }
            set
            {
                moStudentInfo.sStandardName = value;
            }
        }

        public char Is_Dummy_Admission
        {
            get
            {
                return moStudentInfo.cIs_Dummy_Admission;
            }
            set
            {
                moStudentInfo.cIs_Dummy_Admission = value;
            }
        }

        public int AcademicYearId
        {
            get
            {
                return moStudentInfo.iAcademicYearId;
            }
            set
            {
                moStudentInfo.iAcademicYearId = value;
            }
        }
        public int OptionalSubjectId
        {
            get
            {
                return moStudentInfo.iOptionalSubjectId;
            }
            set
            {
                moStudentInfo.iOptionalSubjectId = value;
            }
        }

        public int SecondLanguageSubjectId
        {
            get
            {
                return moStudentInfo.iSecondLanguageSubjectId;
            }
            set
            {
                moStudentInfo.iSecondLanguageSubjectId = value;
            }
        }

        public int ThirdLanguageSubjectId
        {
            get
            {
                return moStudentInfo.iThirdLanguageSubjectId;
            }
            set
            {
                moStudentInfo.iThirdLanguageSubjectId = value;
            }
        }

        public int ParentUserRoleId
        {
            get
            {
                return moStudentInfo.iParentUserRoleId;
            }
            set
            {
                moStudentInfo.iParentUserRoleId = value;
            }
        }

        public int ParentUserId
        {
            get
            {
                return moStudentInfo.iParentUserId;
            }
            set
            {
                moStudentInfo.iParentUserId = value;
            }
        }

        public string sFormNo
        {
            get
            {
                return moStudentInfo.sFormNo;
            }
            set
            {
                moStudentInfo.sFormNo = value;
            }
        }        
        public string OfficeNumber
        {
            get { return moStudentInfo.sOfficeNo; }
            set { moStudentInfo.sOfficeNo = value; }
        }
        public string NeighbourNumber
        {
            get { return moStudentInfo.sNeighbourNo; }
            set { moStudentInfo.sNeighbourNo=value; }
        }
        public string StudentSiblingNames
        {
            get { return moStudentInfo.sStidentSiblingNames; }
            set { moStudentInfo.sStidentSiblingNames = value; }
        }

        public double Height
        {
            get { return moStudentInfo.dHeight; }
            set { moStudentInfo.dHeight = value; }
        }

        public double Weight
        {
            get { return moStudentInfo.dWeight; }
            set { moStudentInfo.dWeight = value; }
        }

		public string LastSchoolName
		{
			get { return moStudentInfo.sLastSchoolName; }
			set { moStudentInfo.sLastSchoolName = value; }
		}

        public string LastSchoolAddress
        {
            get { return moStudentInfo.sLastSchoolAddress; }
            set { moStudentInfo.sLastSchoolAddress = value; }
        }

		public string LastSchoolStandard
		{
			get { return moStudentInfo.sLastSchoolStandard; }
			set { moStudentInfo.sLastSchoolStandard = value; }
		}

        public string LastSchoolUDISENo
        {
            get { return moStudentInfo.sLastSchoolUDISENo; }
            set { moStudentInfo.sLastSchoolUDISENo = value; }
        }

		public string LastSchoolBoardName
		{
			get { return moStudentInfo.sLastSchoolBoardName; }
			set { moStudentInfo.sLastSchoolBoardName = value; }
		}
		public bool IsRecognised
		{
			get { return moStudentInfo.bIsRecognised; }
			set { moStudentInfo.bIsRecognised = value; }
		}

        public string UDISENumber
        {
            get
            {
                return moStudentInfo.sUDISENumber;
            }
            set
            {
                moStudentInfo.sUDISENumber = value;
            }
        }

        public string BoardRegistrationNo
        {
            get
            {
                return moStudentInfo.sBoardRegNo;
            }
            set
            {
                moStudentInfo.sBoardRegNo = value;
            }
        }

        public bool IsRiseAndShine
        {
            get 
            {
                return moStudentInfo.IsRiseAndShine;
            }
            set
            {
                moStudentInfo.IsRiseAndShine = value;
            }
        }

        public int AdmissionForId
        {
            get
            {
                return moStudentInfo.AdmissionForId;
            }
            set
            {
                moStudentInfo.AdmissionForId = value;
            }
        }
       
        public string GRNumber
        {
            get
            {
                return moStudentInfo.sGRNumber;
            }
            set
            {
                moStudentInfo.sGRNumber = value; ;
            }
        }
        public string StudentUniqueNo
        {
            get
            {
                return moStudentInfo.sStudentUniqueNo;
            }
            set
            {
                moStudentInfo.sStudentUniqueNo = value; ;
            }
        }

        public string ConfirmedByText
        {
            get
            {
                return moStudentInfo.sConfirmedByText;
            }
            set
            {
                moStudentInfo.sConfirmedByText = value;
            }
        }

       
        public string UpdatedByText
        {
            get
            {
                return moStudentInfo.sUpdatedByText;

            }
            set
            {
                moStudentInfo.sUpdatedByText = value;
            }
        }
        
        
      
        public bool IsForDayBoarding
        {
            get
            {
                return moStudentInfo.IsForDayBoarding;
            }
            set
            {
                moStudentInfo.IsForDayBoarding = value;
            }
        }

        public bool IsDayBoardingFeePaid
        {
            get 
            {
                return moStudentInfo.IsDayBoardingFeePaid;
            }
            set
            {
                moStudentInfo.IsDayBoardingFeePaid = value;
            }
        }

        public int FeeCategoryId
        {
            get
            {
                return moStudentInfo.FeeCategoryId;
            }
            set
            {
                moStudentInfo.FeeCategoryId = value;
            }
        }

        public string SralNo
        {
            get
            {
                return moStudentInfo.SaralNo;
            }
            set
            {
                moStudentInfo.SaralNo = value;
            }
        }

        public bool IsOnlyChild
        {
            get
            {
                return moStudentInfo.IsOnlyChild;
            }
            set
            {
                moStudentInfo.IsOnlyChild = value;
            }
        }

        public bool Minority
        {
            get
            {
                return moStudentInfo.Minority;
            }
            set
            {
                moStudentInfo.Minority = value;
            }
        }
        //new fields added
        public int Stream
        {
            get
            {
                return moStudentInfo.StreamId;
            }
            set
            {
                moStudentInfo.StreamId = value;
            }
        }
        public int Group
        {
            get
            {
                return moStudentInfo.StreamwiseGroupId;
            }
            set
            {
                moStudentInfo.StreamwiseGroupId = value;
            }
        }
        public int FirstOptionalSubject
        {
            get
            {
                return moStudentInfo.OptSubjectOne;
            }
            set
            {
                moStudentInfo.OptSubjectOne = value;
            }
        }
        public int SecondOptionalSubject
        {
            get
            {
                return moStudentInfo.OptSubjectTwo;
            }
            set
            {
                moStudentInfo.OptSubjectTwo = value;
            }
        }
        public string CompulsorySubject
        {
            get
            {
                return moStudentInfo.CompulsorySubject;
            }
            set
            {
                moStudentInfo.CompulsorySubject = value;
            }
        }
        public string CompitativeExams
        {
            get
            {
                return moStudentInfo.chkCompitativeExams;
            }
            set
            {
                moStudentInfo.chkCompitativeExams = value;
            }
        }
         public int ResidenceTypeId
        {
            get
            {
                return moStudentInfo.iResidenceTypeId;
            }
            set
            {
                moStudentInfo.iResidenceTypeId = value;
            }
        }
         public string ResidenceTypeName
         {
             get
             {
                 return moStudentInfo.sResidenceTypeName;
             }
             set
             {
                 moStudentInfo.sResidenceTypeName = value;
             }
         }
         public string AdmissionStandard
         {
             get
             {
                 return moStudentInfo.sAdmissionStandard;
             }
             set
             {
                 moStudentInfo.sAdmissionStandard = value;
             }
         }

         public string RFID
         {
             get
             {
                 return moStudentInfo.sRFID;
             }
             set
             {
                 moStudentInfo.sRFID = value;
             }
         }
        #endregion

        #region Year wise Student details

        public string Name
        {
            get
            {
                return moYrWiseStudentInfo.sName;
            }
            set
            {
                moYrWiseStudentInfo.sName = value;
            }
        }
        public Int32 StandardId
        {
            get
            {
                return moYrWiseStudentInfo.iStandardId;
            }
            set
            {
                moYrWiseStudentInfo.iStandardId = value;
            }
        }

        public Int32 YearId
        {
            get
            {
                return moYrWiseStudentInfo.iYearId;
            }
            set
            {
                moYrWiseStudentInfo.iYearId = value;
            }
        }

        public Int32 DivisionId
        {
            get
            {
                return moYrWiseStudentInfo.iDivisionId;
            }
            set
            {
                moYrWiseStudentInfo.iDivisionId = value;
            }
        }
        public string StandardDivisionName
        {
            get
            {
                return moYrWiseStudentInfo.sStandardDivisionName;
            }
            set
            {
                moYrWiseStudentInfo.sStandardDivisionName = value;
            }
        }
        public Int32 YearWiseStudentId
        {
            get
            {
                return moYrWiseStudentInfo.iYearWIseStudentId;
            }
            set
            {
                moYrWiseStudentInfo.iYearWIseStudentId = value;
            }
        }
        public Int32 RollNo
        {
            get
            {
                return moYrWiseStudentInfo.iRollNo;
            }
            set
            {
                moYrWiseStudentInfo.iRollNo = value;
            }
        }

        public bool IsPrePrimaryStandard
        {
            get
            {
                return moStudentInfo.sIsPrePrimaryStandard == Constants.S_YES;
            }            
        }

        public string PrePrimaryEnrolmentNumber
        {
            get
            {
                return moStudentInfo.sPrePrimaryEnrolmentNumber;
            }            
        }
        

        #endregion

        #endregion

        public int StudentCount
        {
            get
            {
                return moStudentDC.miStudentCount;
            }
            set
            {
                moStudentDC.miStudentCount = value;
            }
        }
        #endregion

        #region constructors

        /// <summary>
        ///  Default constructor
        /// </summary>

        public StudentBL()
        {

            moStudentDC = new StudentDC();
            moSchoolUserBL = new SchoolUserBL();

        }

        public StudentBL(Int32 aiYearStudentId)
        {

            moStudentDC = new StudentDC(aiYearStudentId);
            this.moStudentInfo = moStudentDC.StudentDetails;
            this.moYrWiseStudentInfo = moStudentDC.YearWiseStudentDetails;
            moSchoolUserBL = new SchoolUserBL();

        }

        /// <summary>
        /// parameterised consructor
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>

        public StudentBL(Int32 aiSchoolId, Int32 aiStudentId)
        {

            moStudentDC = new StudentDC(aiSchoolId, aiStudentId);
            this.moStudentInfo = moStudentDC.StudentDetails;
            this.moYrWiseStudentInfo = moStudentDC.YearWiseStudentDetails;
            moSchoolUserBL = new SchoolUserBL();

        }

        /// <summary>
        /// parameterised consructor
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>

        public StudentBL(Int32 aiSchoolId, Int32 aiStudentId, bool abIsYrwise)
        {

            moStudentDC = new StudentDC(aiSchoolId, aiStudentId, abIsYrwise);
            this.moStudentInfo = moStudentDC.StudentDetails;
            this.moYrWiseStudentInfo = moStudentDC.YearWiseStudentDetails;
            moSchoolUserBL = new SchoolUserBL();

        }

        /// <summary>
        /// parameterised consructor
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>

        public StudentBL(Int32 aiSchoolId, Int32 aiAccYearId, Int32 aiStudentId)
        {

            moStudentDC = new StudentDC(aiSchoolId, aiAccYearId, aiStudentId);
            this.moStudentInfo = moStudentDC.StudentDetails;
            this.moYrWiseStudentInfo = moStudentDC.YearWiseStudentDetails;
            moSchoolUserBL = new SchoolUserBL();

        }
		

        /// <summary>
        /// Update student streamwise subject details
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        public void UpdateStudentStreamwiseDetails(int aiStudentId)
        {
            moStudentDC.StudentDetails = moStudentInfo;
            moStudentDC.UpdateStudentStreamwiseSubjectDetails(aiStudentId);
        }
        public void SaveTransferredStudentDetails(string ids, int aiSchoolID, int aiAcademicYEarId, int aiInsertedById)
        {
            moStudentDC.SaveTransferredStudentDetails(ids, aiSchoolID, aiAcademicYEarId, aiInsertedById);
        }

        public DataTable GetStudentListToActiveTransfer(int aiSchoolid, string asName, bool abShowOnlyNonActivated, bool abIsFrom)
        {
            return moStudentDC.GetStudentListToActiveTransfer(aiSchoolid, asName, abShowOnlyNonActivated, abIsFrom);
        }

        public DataTable GetStudentDetails(int aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, bool abIncludeUserName)
        {
            return moStudentDC.GetStudentDetails(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName, abIncludeUserName);
        }

        public void TransferStudents(string Ids, int aiSchoolid, int aiAcademicYearId, int aiTargetSchoolId, int aiInsertedById)
        {
            moStudentDC.TransferStudents(Ids, aiSchoolid, aiAcademicYearId, aiTargetSchoolId, aiInsertedById);
        }

        public List<SchoolBranchDetails> GetSchoolBranchDetails(int aiSchoolId)
        {
            return moStudentDC.GetSchoolBranchDetails(aiSchoolId);
        }
       
        /// <summary>
        /// This function is used to delete family photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>

        public void DeleteFamilyPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            moStudentDC.DeleteFamilyPhoto(aiStudentId, aiSchoolId, aiUpdatedById);
        }

        /// <summary>
        /// This function is used to delete Father photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteFatherPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            moStudentDC.DeleteFatherPhoto(aiStudentId, aiSchoolId, aiUpdatedById);
        }

        /// <summary>
        /// This function is used to delete Mother photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteMotherPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            moStudentDC.DeleteMotherPhoto(aiStudentId, aiSchoolId, aiUpdatedById);
        }

        /// <summary>
        /// This function is used to delete Guardian photo of perticular student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteGuardianPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            moStudentDC.DeleteGuardianPhoto(aiStudentId, aiSchoolId, aiUpdatedById);
        }
        /// <summary>
        /// This function is used to delete Caste certificate photo of perticular student
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteCasteCertificatePhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            moStudentDC.DeleteCasteCertificatePhoto(aiStudentId, aiSchoolId, aiUpdatedById);
        }
        /// <summary>
        /// This function is used to delete Mother Aadhar card  photo of perticular student
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteMotherAadharPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            moStudentDC.DeleteMotherAadharPhoto(aiStudentId, aiSchoolId, aiUpdatedById);
        }
        /// <summary>
        ///  This function is used to delete Father Aadhar card  photo of perticular student
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUpdatedById"></param>
        public void DeleteFatherAadharPhoto(int aiStudentId, int aiSchoolId, int aiUpdatedById)
        {
            moStudentDC.DeleteFatherAadharPhoto(aiStudentId, aiSchoolId, aiUpdatedById);
        }

        /// <summary>
        /// Custructor to initiate student object for a provided school id and user id
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiRegNo"></param>
        public StudentBL(Int32 aiSchoolId, String asRegNo, Int32 aiUserId)
        {

            moStudentDC = new StudentDC(aiSchoolId, asRegNo, aiUserId);
            this.moStudentInfo = moStudentDC.StudentDetails;
            this.moYrWiseStudentInfo = moStudentDC.YearWiseStudentDetails;
            moSchoolUserBL = new SchoolUserBL();
        }

        public SchoolEntities.StudentAdditionalDetails GetStudentAdditionalDetails(int aiSchoolId, int aiStudentId)
        {
            return moStudentDC.GetStudentAdditionalDetails(aiSchoolId, aiStudentId);
        }

        public StudentBL(Int32 aiSchoolId, int aiAcademicYrId, string asRegistration)
        {
            moStudentDC = new StudentDC(aiSchoolId, aiAcademicYrId, asRegistration);
            this.moStudentInfo = moStudentDC.StudentDetails;
            this.moYrWiseStudentInfo = moStudentDC.YearWiseStudentDetails;
            moSchoolUserBL = new SchoolUserBL();
        }
        #endregion

        #region Public methods
        #region  Basic Details

        /// <summary>
        /// Function to update student information
        /// </summary>
        /// <returns></returns>
        public Int32 UpdateStudent(DateTime adtOldJoiningDate, bool bDeleteFee, out int aiTrackingId)
        {
            moStudentDC.StudentDetails = moStudentInfo;
            moYrWiseStudentInfo.iSchoolId = moStudentInfo.SchoolId;
            moYrWiseStudentInfo.iStudentId = moStudentInfo.iStudentId;
            moStudentDC.YearWiseStudentDetails = moYrWiseStudentInfo;
            return moStudentDC.UpdateStudent(adtOldJoiningDate, bDeleteFee, out aiTrackingId);
        }

        public void UpdateStudentTrackingDetails(int aiSchoolId, int aiInsertedById, int aiStudentId, int aiTrackingId, int aiAcademicYearId)
        {
            moStudentDC.UpdateStudentTrackingDetails(aiSchoolId, aiInsertedById, aiStudentId, aiTrackingId, aiAcademicYearId);
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
            moStudentDC.DeleteDayBoardingFees(aischoolId, aiAcademicYearId, aiYearwiseStudentId, aiUpdatedById);
        }

        /// <summary>
        /// This method is used to insert student additional details.
        /// </summary>
        /// <param name="iYrwiseStudentId"></param>
        public void AddStudentAdditionalDetails(int aiSchoolId, int aiUserId, StudentAdditionalDetails aoStudentAdditionalDetails)
        {
            moStudentDC.AddStudentAdditionalDetails(aiSchoolId, aiUserId, aoStudentAdditionalDetails);
        }

        /// <summary>
        /// Function to update student information
        /// </summary>
        /// <returns></returns>

        public Int32 UpdateStudentsMobileNo()
        {
            moStudentDC.StudentDetails = this.moStudentInfo;
            return moStudentDC.UpdateStudentsMobileNo();
        }

        /// <summary>
        /// This method adds new student 
        /// </summary>
        /// <returns></returns>

        public Int32 InsertStudent(string asSiblingStudentIds)
        {
            moStudentDC.StudentDetails = moStudentInfo;
            moStudentDC.YearWiseStudentDetails = moYrWiseStudentInfo;
            return moStudentDC.InsertStudent(asSiblingStudentIds);
        }

        public string GetInsertStatementForStudents()
        {
            // This function returns the insert statement for the employee.
            moStudentDC.StudentDetails = moStudentInfo;
            return moStudentDC.CreateInsertStatementForStudentBasicDetais(Constants.I_ZERO);
        }

        public void UpdateIsLeaveFlag(int aiStudentId)
        {
            moStudentDC.StudentDetails = moStudentInfo;
            moStudentDC.UpdateIsLeaveFlag(aiStudentId);
        }

        /// <summary>
        /// This method is used to change division of multiple students.
        /// </summary>
        /// <param name="aoArrDeleteUserIds"></param>
        /// <param name="aiDivisionId"></param>
		public string UpdateStudentDivision(string asStudentIdsXML, int aiSrcStdId, int aiSrcDivId, int aiTargerStdId, int aiTargetDivId, int aiSchoolId, int aiAcademicYrId, int aiFinancialYearId)
		{
			moStudentDC.StudentDetails = moStudentInfo;
			return moStudentDC.UpdateStudentDivision(asStudentIdsXML, aiSrcStdId, aiSrcDivId, aiTargerStdId, aiTargetDivId, aiSchoolId, aiAcademicYrId, aiFinancialYearId);
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
            moStudentDC.StudentDetails = moStudentInfo;
            moStudentDC.UpdateStudentsRollNos(ischoolId, iAcademicYearId, iStandardId, iDivisionId, sXmlStudentsRollNos);
        }


        public DataSet getStudentIdentityCards(int iSchoolID, int iAcademicYrID, int mistandardId, int miDivisionId, object miStudentId,string asStudentName,string asStudentReg)
        {
            return moStudentDC.getStudentIdentityCards(iSchoolID, iAcademicYrID, mistandardId, miDivisionId, miStudentId, asStudentName, asStudentReg);
        }

        #endregion

        public static List<Operator> GetOperators()
        {
            return StudentDC.GetOperators();  
        }

        /// <summary>
        /// This method is used to get Fee Area Names
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public  List<FeeAreaNamesDetails> GetFeeAreaNames(int aiSchoolId)
        {
            return moStudentDC.GetFeeAreaNames(aiSchoolId);
        }

        public static bool CheckIsEnrollmentNumber(string asEnrollmentNo, int aiSchoolId)
        {
            return StudentDC.CheckIsEnrollmentNumber(asEnrollmentNo, aiSchoolId);
        }
        public void RetriveStudentdetailsForLC(Int32 aiSchoolId, string asRegNo)
        {
            moStudentDC = new StudentDC();
            moStudentDC.RetriveStudentdetailsForLC(aiSchoolId, asRegNo);
            this.moStudentInfo = moStudentDC.StudentDetails;
            this.moYrWiseStudentInfo = moStudentDC.YearWiseStudentDetails;
        }
        public static bool CheckIsStudentLeaveSchool(string asEnrollmentNo, int aiSchoolId)
        {
            return StudentDC.CheckIsStudentLeaveSchool(asEnrollmentNo, aiSchoolId);
        }
        /// <summary>
		/// <summary>
        /// This method is used uo get the mobile numbers for the stuent.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static string GetStudentMobileNumbers(int aiUserId,int aiSchoolId)
        {
            return StudentDC.GetStudentMobileNumbers(aiUserId, aiSchoolId);
        }
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicID"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <returns></returns>
        public static bool CheckIfStudentsAreAvailableForStandardDivision(int aiSchoolId, int aiAcademicID, int aiStandardDivisionId)
        {
            DataSet oDS = StudentDC.CheckIfStudentsAreAvailableForStandardDivision(aiSchoolId, aiAcademicID, aiStandardDivisionId);
            if (Convert.ToInt32(oDS.Tables[0].Rows[0][0].ToString()) == 0)
                return false;
            else
                return true;

        }

        public bool CheckIsEnrollmentNumberIsDuplicate(int aiSchoolId, string asRegistrationNumber)
        {
            moStudentDC.StudentDetails = moStudentInfo;
            return moStudentDC.CheckIsEnrollmentNumberIsDuplicate(aiSchoolId, asRegistrationNumber);
        }

        public DataTable CheckIsGeneralRegistrationNumberIsDuplicate()
        
        {
            moStudentDC.StudentDetails = moStudentInfo;
      
            return moStudentDC.CheckIsGeneralRegistrationNumberIsDuplicate();
        }

        public DataTable CheckIsStudentUniqueNumberIsDuplicate()
        {
            moStudentDC.StudentDetails = moStudentInfo;
            return moStudentDC.CheckIsStudentUniqueNumberIsDuplicate();
        }

        public bool CheckIsStudentIsDuplicate(int aiSchoolId, string asFirstName, string asLastName, DateTime adtDOB)
        {
            moStudentDC.StudentDetails = moStudentInfo;
            return moStudentDC.CheckIsStudentIsDuplicate(aiSchoolId, asFirstName, asLastName, adtDOB);
        }

        public bool CheckIsRollNumberDuplicate(int aiSchoolId, int aiAcademicYearId, int aiStandardId,
                                                int aiDivisionId, int aiRollNo, int aiStudentId)
        {
            //moStudentDC.StudentDetails = moStudentInfo;
            return moStudentDC.CheckIsRollNumberDuplicate(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiRollNo, aiStudentId);
        }
        public bool CheckIsRFormNumberDuplicate(int aiSchoolId,string sFormNo, int aiStudentId)
        {
            return moStudentDC.CheckIsRFormNumberDuplicate(aiSchoolId, sFormNo, aiStudentId);
        }

        public string CheckDependenciesForFees(int aiStudentId, int aiAcademicYearId)
        {
            string sReturn = "";

            sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.StudentPaidFee), aiStudentId, string.Empty, aiAcademicYearId);
            return sReturn;
        }

        /// <summary>
        /// Returns datatable contianing some of the details of student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static DataSet GetStudentDetailsForControlPanel(int aiStudentId, int aiSchoolId, int aiAcademicYrId)
        {
            return StudentDC.GetStudentDetailsForControlPanel(aiStudentId, aiSchoolId, aiAcademicYrId);
        }

        /// <summary>
        /// Returns datatable contianing some of the details of student.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static DataSet GetControlPanelDetails(int aiUserId, int aiSchoolId, int aiAcademicYrId)
        {
            return StudentDC.GetControlPanelDetails(aiUserId, aiSchoolId, aiAcademicYrId);
        }

        public static DataTable GetNextStudentRollNoAndLogin(int aiStdId, int aiDivId, int aiSchoolId)
        {
            return StudentDC.GetNextStudentRollNoAndLogin(aiStdId, aiDivId, aiSchoolId);
        }

        public string GetMobileNo(int aiSchoolId, int aiStudentId)
        {
            return StudentDC.GetMobileNo(aiSchoolId, aiStudentId);
        }

        public static int GetNextLoginId(int aiSchoolId, int aiUserRoleId)
        {
            return StudentDC.GetNextLoginId(aiSchoolId, aiUserRoleId);
        }

        /// <summary>
        /// This function is used to validate the student's data before complete deleting student.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStudentId"></param>
        public void ValidateStudent(int aiSchoolId, int aiStudentId)
        {
            StudentDC oStudentDC = new StudentDC();
            string sMessage = oStudentDC.ValidateStudent(aiSchoolId, aiStudentId);
            if (!sMessage.IsNullOrEmpty())
                throw new ReferenceExceptions(sMessage);
        }

        public void DeleteStudent(int aiSchoolId, int aiAcademicYearId, int iStudentId, DateTime oLeftDate, char cPermanent_delete, int IsFormNo, int iCancellationFormNo, int aiUpdatedById, bool abIsIncludeinBlackList, string asComment)
        {
            StudentDC oStudentDC = new StudentDC();
            oStudentDC.DeleteStudent(aiSchoolId, aiAcademicYearId, iStudentId, oLeftDate, cPermanent_delete, IsFormNo, iCancellationFormNo, aiUpdatedById, abIsIncludeinBlackList, asComment);
        }

        public bool IsStudentPrePrimary(int iSchoolId, int iAcademicYearId, int iStudentId)
        {
            bool bIsPrePrimary = false;
            StudentDC oStudentDC = new StudentDC();
            DataTable oDataTable = oStudentDC.IsStudentPrePrimary(iSchoolId, iAcademicYearId, iStudentId);
            if (oDataTable != null)
            {
                if (oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != null)
                {
                    if (Convert.ToChar(oDataTable.Rows[0][0]).Equals(Constants.C_YES))
                        bIsPrePrimary = true;
                }
            }
            return bIsPrePrimary;
        }

        /// <summary>
        /// This method is used to check precondition for a standard 
        /// ie fee configuration for particular standard is completed or not. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public static string CheckPreConditionAndGetMsg(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            return StudentDC.CheckPreConditionAndGetMsg(aiSchoolId, aiAcademicYearId, aiStandardId);
        }

        public static DataSet GetNewAdmissionsCount(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiAdmissionType, string asLocationIds, string asResidenceIds)
        {
            return StudentDC.GetNewAdmissionsCount(aiSchoolId, aiAcademicYearId, aiStandardId, aiAdmissionType, asLocationIds, asResidenceIds);
        }
        /// <summary>
        /// This method is used to get standarwise fee configuration as per school id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiConfigId"></param>
        /// <returns></returns>
        public static string IsStandardwiseFeeConfig(int aiSchoolId, Constants.SchoolConfigurations aiConfigId)
        {
            return StudentDC.IsStandardwiseFeeConfig(aiSchoolId, Convert.ToInt32(aiConfigId));
        }

		/// <summary>
		/// This method is used to get new admission count.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <returns></returns>
		public static int GetNewAdmissionCount(int aiSchoolId,string asShowStudentAdmission, int aiAcademicYearId)
		{
            return StudentDC.GetNewAdmissionCount(aiSchoolId, asShowStudentAdmission, aiAcademicYearId);
		}

        #region Display all

        /// <summary>
        /// This function gets all the students from the table According to specified search criteria
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            Boolean bChekLeftDate = false;
            DataTable oDt = StudentDC.GetAllStudents(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName, sortExpression, iEndIndex, iStartIndex, bChekLeftDate, false);
            if (oDt != null && oDt.Rows.Count > 0)
                StudentCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asName"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static List<StudentInfo> GetStudentsForFeesUpdate(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, bool mbConsiderForRTEConcession)
        {           
            Boolean bChekLeftDate = true;
            return StudentDC.GetStudentsForFeesUpdate(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName, bChekLeftDate, mbConsiderForRTEConcession);
        }

        /// <summary>
        /// This function gets all the students from the table According to specified search criteria
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, string asRegNo, bool abIsExactMatch, bool abPhotoFilePath, String sortExpression, int maximumRows, int startRowIndex, string asOperator, string asPrefix)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = " Original_Standard_Id, Original_Division_Id,Roll_No ";
            
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            Boolean bChekLeftDate = true;
            //return StudentDC.GetAllStudents(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName,asRegNo,abIsExactMatch,sortExpression, iEndIndex, iStartIndex, bChekLeftDate, false, abPhotoFilePath,asOperator,asPrefix);
            DataTable oDt = StudentDC.GetAllStudents(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName, asRegNo, abIsExactMatch, sortExpression, iEndIndex, iStartIndex, bChekLeftDate, false, abPhotoFilePath, asOperator, asPrefix); ;
            if (oDt != null && oDt.Rows.Count > 0)
                StudentCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        /// <summary>
        /// This function gets all the students from the table According to specified search criteria
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, int aiStandardDivisionId, string asName, string asRegNo, bool abIsExactMatch, String sortExpression, int maximumRows, int startRowIndex, string asOperator,string asPrefix)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";
            
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            Boolean bChekLeftDate = false;
            DataTable oDt = StudentDC.GetAllStudents(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, aiStandardDivisionId, asName, asRegNo, abIsExactMatch, sortExpression, iEndIndex, iStartIndex, bChekLeftDate, false, asOperator,asPrefix);
            if (oDt != null && oDt.Rows.Count > 0)
                StudentCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        public DataTable GetAllStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, string asRegNo, bool abIsExactMatch, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            Boolean bChekLeftDate = false;
            DataTable oDt = StudentDC.GetAllStudents(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName, asRegNo, abIsExactMatch, sortExpression, iEndIndex, iStartIndex, bChekLeftDate, false);
            if (oDt != null && oDt.Rows.Count > 0)
                StudentCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }
        public DataTable DeleteAllStudent(int aiStdDivId, int aischoolid, int aiAcademicYearId, int aiUpdatedById, int aiAssessmentId)
        {
            return moStudentDC.DeleteAllStudent(aiStdDivId, aischoolid, aiAcademicYearId, aiUpdatedById, aiAssessmentId);

        }
        public DataTable Delete(int aiStudentId, int aiSchoolId, int aiAcademicYearId, int AiAssessmentId, int aiUpdatedBYId)
        {
            return moStudentDC.Delete(aiStudentId, aiSchoolId, aiAcademicYearId, AiAssessmentId, aiUpdatedBYId);

        }
		public List<StudentInfo> GetAll(Int32 aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiAssessmentId, string sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
			return moStudentDC.GetAll(aiSchoolId, aiAcademicYearId, aiStdDivId, aiAssessmentId, sortExpression, iEndIndex, iStartIndex);
        }

        public int CountRows(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, int aiStandardDivisionId, string asName, string asRegNo, bool abIsExactMatch, string asOperator, string asPrefix)
        {
            return StudentCount;
        }

		public int GetCount(Int32 aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiAssessmentId, string sortExpression, int maximumRows, int startRowIndex)
        {
            return StudentCount;
        }
       
        /// <summary>
        /// This function gets all the students from the table According to specified search criteria
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="asName"></param>
        /// <returns></returns>

        public static DataTable GetAllLeaveStudents(Int32 aiSchoolId, int aiAcademicYearId, string asName, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            Boolean bChekLeftDate = false;
            return StudentDC.GetAllLeaveStudents(aiSchoolId, aiAcademicYearId, asName, sortExpression, iEndIndex, iStartIndex, bChekLeftDate);
        }

        public static int CountRowsOfLeaveStaudent(Int32 aiSchoolId, int aiAcademicYearId, string asName)
        {
            return StudentDC.CountRowsOfLeaveStaudent(aiSchoolId, aiAcademicYearId,asName, false);
        }

        public static DataTable GetStudentsMonthWiseAttendance(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardDivisionId, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "Roll_No";
            }
            string sSortDirection = "ASC";
            if (sortExpression.ToUpper().Contains("DESC"))
            {
                sSortDirection = "DESC";
                sortExpression = sortExpression.Replace("DESC", "");
            }
            else
                sortExpression = sortExpression.Replace("ASC", "");
            sortExpression = "[" + sortExpression + "] " + sSortDirection;
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return StudentDC.GetStudentsMonthWiseAttendance(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, 1000, 0, sortExpression, iEndIndex, iStartIndex);
        }

        public static DataTable GetStudentsMonthWiseAttendance(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardDivisionId, int topRanker, int student_id, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                sortExpression = "Roll_No";
            }
            string sSortDirection = "ASC";
            if (sortExpression.ToUpper().Contains("DESC"))
            {
                sSortDirection = "DESC";
                sortExpression = sortExpression.Replace("DESC", "");
            }
            else
                sortExpression = sortExpression.Replace("ASC", "");
            sortExpression = "[" + sortExpression + "] " + sSortDirection;
            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            return StudentDC.GetStudentsMonthWiseAttendance(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, topRanker, student_id, sortExpression, iEndIndex, iStartIndex);
        }

        public int CountStudentsMonthWiseAttendance(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardDivisionId)
        {
            DataTable oDataTable = StudentDC.GetStudentsMonthWiseAttendance(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, 1000, 0, "", 1000, 0);
            int iRet = 0;
            if (oDataTable.Rows.Count > 0)
            {
                iRet = oDataTable.Rows.Count;
            }
            return iRet;
        }

        public DataTable GetAllCurrentStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, String sortExpression, int maximumRows, int startRowIndex, bool abIncludeUserName)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            Boolean bChekLeftDate = true;
            DataTable oDt = StudentDC.GetAllStudents(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName, sortExpression, iEndIndex, iStartIndex, bChekLeftDate, abIncludeUserName);
            if (oDt != null && oDt.Rows.Count > 0)
                StudentCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }    

        public DataTable GetAllCurrentStudents(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiUserTypeId, Int32 aiStandardId, Int32 aiDivisionId, string asName, String sortExpression, int maximumRows, int startRowIndex,bool abIncludeUserName)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            Boolean bChekLeftDate = true;
            DataTable oDt = StudentDC.GetAllStudents(aiSchoolId, aiAcademicYearId, aiUserTypeId, aiStandardId, aiDivisionId, asName, sortExpression, iEndIndex, iStartIndex, bChekLeftDate, abIncludeUserName);
            if (oDt != null && oDt.Rows.Count > 0)
                StudentCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
		}      
        
        public DataTable GetAllStudentsForFee(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Roll_No";

            int iStartIndex = startRowIndex;
            int iEndIndex = iStartIndex + maximumRows;
            DataTable oDt = StudentDC.GetAllStudentsForFee(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName, sortExpression, iEndIndex, iStartIndex, false, false);
            if (oDt != null && oDt.Rows.Count > 0)
                StudentCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            return oDt;
        }

        public string GetFormNumber(int aiSchoolId, int aiStudentId)
        {
            StudentDC oStudentDC = new StudentDC();
            return oStudentDC.GetFormNumber(aiSchoolId, aiStudentId);
        }

        public int CountStudentsForFee(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName)
        {
            return StudentCount;
        }

		public static List<string> GetPaidFeesStudents(int aiSchoolId, int aiAcademicYearId, int aiCurrentStandardId, int aiCurrentDivId)
		{
			return StudentDC.GetPaidFeesStudents(aiSchoolId, aiAcademicYearId, aiCurrentStandardId, aiCurrentDivId);
		}

        public int CountRows(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName)
        {
            return StudentCount;
        }

        public int CountRows(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, string asRegNo, bool abIsExactMatch, bool abPhotoFilePath, string asOperator, string asPrefix)
        {
            return StudentCount;
        }

		public int CountCurrentStudentRows(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asName, bool abIncludeUserName)
        {
            return StudentCount;
        }

		public int CountCurrentStudentRows(Int32 aiSchoolId, int aiAcademicYearId, Int32 aiUserTypeId, Int32 aiStandardId, Int32 aiDivisionId, string asName, bool abIncludeUserName)
        {
            return StudentCount;
        }
        public static DataTable GetAllStudentsForMarks(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, DateTime dTestDate)
        {
            return StudentDC.GetAllStudentsForMarks(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, dTestDate);
        }

        public static DataTable GetAllStudentsForSubject(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, DateTime dTestDate, int aiSubjectId)
        {
            return StudentDC.GetAllStudentsForSubject(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, dTestDate, aiSubjectId);
        }
        
        public static DataTable GetAllStudents(Int32 aiSchoolId, int aiStandardId, int aiDivisionId, Int32 aiAcademicYrId)
        {
            return StudentDC.GetAllStudents(aiSchoolId, aiStandardId, aiDivisionId, aiAcademicYrId);
        }

        public static DataTable GetAllStudentsWithLeftFilter(Int32 aiSchoolId, int aiStandardId, int aiDivisionId, Int32 aiAcademicYrId, bool abFilterLeft)
        {
            return StudentDC.GetAllStudentsWithLeftFilter(aiSchoolId, aiStandardId, aiDivisionId, aiAcademicYrId, abFilterLeft);
        }

        public static DataTable GetAllStudents(Int32 aiSchoolId, Int32 aiAcademicYrId, string asRegNumbers)
        {
            return StudentDC.GetAllStudents(aiSchoolId, aiAcademicYrId, asRegNumbers);
        }

        public static DataTable GetAllStudentsByName(Int32 aiSchoolId, Int32 aiAcademicYrId, string asRegNumbers, bool abIsOnlyPrimary, int aiReportId)
        {
            return StudentDC.GetAllStudentsByName(aiSchoolId, aiAcademicYrId, asRegNumbers, abIsOnlyPrimary, aiReportId);
        }

        public static DataTable GetFinancial(Int32 aiSchoolId, int aiStudentId)
        {
            return StudentDC.GetFinancial(aiSchoolId, aiStudentId);
        }

        public static DataTable GetAllAcademicYearsOfStudent(int aiSchoolId, int aiYearwiseStudentId)
        {
            return StudentDC.GetAllAcademicYearsOfStudent(aiSchoolId, aiYearwiseStudentId);
        }

        public DataSet GetStudentPhoto(int aiSchoolId, int aiAcademicYrId, string asStandardId, string asDivisionId, string asName, string asRegNo, int aIsExactMatch, string Operator, string Prefix)
        {
            StudentDC oStudentDC = new StudentDC();
            return oStudentDC.GetStudentPhoto(aiSchoolId, aiAcademicYrId, asStandardId, asDivisionId,asName, asRegNo,aIsExactMatch,Operator,Prefix);
        }

        /// <summary>
        /// This method is used to get student result list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataTable GetStudentsResultList(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId,
                                int aiPageNumber, int aiPageSize, string asOrderBy, string asSortOrder)
        {
            return StudentDC.GetStudentsResultList(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, aiPageNumber, aiPageSize, asOrderBy, asSortOrder);
        }

        /// <summary>
        /// This method is used to get student result list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataTable getPrePrimaryProgressSheetStudentList(Int32 aiSchoolId, Int32 aiStandardDivisionId, int aiTestId, Int32 aiAcademicYrId, bool abIsMonthConfig, String sortExpression, int maximumRows, int startRowIndex)
        {
            if (!abIsMonthConfig)
                return StudentDC.getPrePrimaryProgressSheetStudentList(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, aiTestId, sortExpression, maximumRows, startRowIndex);
            else
                return StudentDC.GetStudentsResultList(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, sortExpression, maximumRows, startRowIndex);

        }

        public static int CountPrePrimaryProgressSheetStudentList(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, int aiTestId,bool abIsMonthConfig)
        {
            int iRet = 0;
            DataTable oDt;
            if (!abIsMonthConfig)
                oDt = StudentDC.getPrePrimaryProgressSheetStudentList(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, aiTestId, "", 15, 0);
            else
                oDt = StudentDC.GetStudentsResultList(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, "", 15, 0);
            if (oDt.Rows.Count > 0)
            {
                iRet = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            }
            return iRet;
        }

        /// <summary>
        /// This method is used to get student result list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static String getPrePrimaryProgressSheetCompleteStatus(Int32 aiSchoolId, Int32 aiStandardDivisionId, int aiTestId, Int32 aiAcademicYrId)
        {
            String sStatus = string.Empty;
            DataTable oDataTable = StudentDC.getPrePrimaryProgressSheetCompleteStatus(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, aiTestId);
            if (oDataTable != null && oDataTable.Rows.Count > 0)
                sStatus = Convert.ToString(oDataTable.Rows[0][0]);
            return sStatus;
        }

        /// <summary>
        /// This method is used to get student result list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataTable GetStudentsResultList(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, String sortExpression, int maximumRows, int startRowIndex)
        {
            return StudentDC.GetStudentsResultList(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, sortExpression, maximumRows, startRowIndex);
        }
        public static int CountStudentsResultList(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId)
        {
            int iRet = 0;
            DataTable oDt = StudentDC.GetStudentsResultList(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, "", 15, 0);
            if (oDt.Rows.Count > 0)
            {
                iRet = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            }
            return iRet;
        }

        /// <summary>
        /// This Functions is used to fetch students list in a splitted record set.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataSet GetStudentsForSubjectMarkSheet(Int32 aiSchoolId, Int32 aiStandardDivisionId
            , Int32 aiAcademicYrId, Int32 aiNoOfRecords, Int32 iTestId, Int32 iSubjectId)
        {
            return StudentDC.GetStudentsForSubjectMarkSheet(aiSchoolId, aiStandardDivisionId
                , aiAcademicYrId, aiNoOfRecords, iTestId, iSubjectId);
        }


        /// <summary>
        /// This method is used to get Annual toppers
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiNoOfRecords"></param>
        /// <returns></returns>
        public static DataSet GetAnnualResult(Int32 aiSchoolId, Int32 aiAcademicYrId, Int32 aiStandardDivisionId, Int32 aiNoOfRecords)
        {
            return StudentDC.GetAnnualResult(aiSchoolId, aiAcademicYrId, aiStandardDivisionId, aiNoOfRecords);
        }

        /// <summary>
        /// This method is used to get Annual toppers
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="aiNoOfRecords"></param>
        /// <returns></returns>
        public static DataSet GetAnnualStanderedResult(Int32 aiSchoolId, Int32 aiAcademicYrId, Int32 aiStandardId, Int32 aiNoOfRecords)
        {
            return StudentDC.GetAnnualStanderedResult(aiSchoolId, aiAcademicYrId, aiStandardId, aiNoOfRecords);
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
            return StudentDC.GetFirstThreeToopers(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, aiTestId, aiSubjectId);
        }

        public static DataTable GetAllStudentsByStdDivForMessageFacillity(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, string asName, int aiTypeId, bool abIsForLeftStudents)
        {
            //This function is used to get all the students by Standard and division id.
            return StudentDC.GetAllStudentsByStdDivForMessageFacillity(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, asName, aiTypeId, abIsForLeftStudents);
        }

        public static DataTable GetAllStudentsByStdDivForMessageFacillity(Int32 aiSchoolId, String asStandardDivisionIds, Int32 aiAcademicYrId, string asName)
        {
            //This function is used to get all the students by Standard and division id.
            return StudentDC.GetAllStudentsByStdDivForMessageFacillity(aiSchoolId, asStandardDivisionIds, aiAcademicYrId, asName);
        }

        public static DataTable GetAllStudentsByGivenStdDivs(Int32 aiSchoolId, Int32 aiAcademicYrId, string sStdDivIds, bool abIsLeftStudents)
        {
            //This function is used to get all the students by Standard and division id.
            return StudentDC.GetAllStudentsByGivenStdDivs(aiSchoolId, aiAcademicYrId, sStdDivIds, abIsLeftStudents);
        }

        public static DataTable GetAllStudentsByStdDivForMessageFacillity(Int32 aiSchoolId, string asUserIds, Int32 aiAcademicYrId)
        {
            //This function is used to get all the students by Standard and division id.
            return StudentDC.GetAllStudentsByStdDivForMessageFacillity(aiSchoolId, asUserIds, aiAcademicYrId);
        }

        public static DataTable GetAllStudentsByStdDivForBookIssue(Int32 aiSchoolId, Int32 aiStandardDivisionId, Int32 aiAcademicYrId, string asName)
        {
            //This function is used to get all students (excluding school leaved) by Standard and division id.
            return StudentDC.GetAllStudentsByStdDivForBookIssue(aiSchoolId, aiStandardDivisionId, aiAcademicYrId, asName);
        }

        public static DataTable GetAllStudentsByStdDivForBookIssue(Int32 aiSchoolId, String asStandardDivisionIds, Int32 aiAcademicYrId, string asName)
        {
            //This function is used to get all the students (excluding school leaved) by Standard and division id.
            return StudentDC.GetAllStudentsByStdDivForBookIssue(aiSchoolId, asStandardDivisionIds, aiAcademicYrId, asName);
        }

        /// <summary>
        /// Thiws method is used to get student id of given academic year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static int GetYearwiseStudentId(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            int iStudentId = 0;
            DataTable oDataTable = StudentDC.GetYearwiseStudentId(aiSchoolId, aiAcademicYrId, aiStudentId);
            if (oDataTable != null && oDataTable.Rows.Count > 0 && oDataTable.Rows[0][0] != DBNull.Value)
                iStudentId = Convert.ToInt32(oDataTable.Rows[0][0]);
            return iStudentId;
        }

        /// <summary>
        /// This method is used to get the list of prefixes.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static List<string> GetPrefixes(int aiSchoolId, int aiAcademicYearId)
        {
            return StudentDC.GetPrefixes(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get the list of postfixes.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static List<string> GetAllRegNoPostfixes(int aiSchoolId, int aiAcademicYearId)
        {
            return StudentDC.GetAllRegNoPostfixes(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// Thiws method is used to get student id of given academic year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static DataTable GetYearwiseStudentDetails(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {            
            DataTable oDataTable = StudentDC.GetYearwiseStudentId(aiSchoolId, aiAcademicYrId, aiStudentId);            
            return oDataTable;
        }


        public static DataTable GetYearwiseStudentDetailsForService(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            return StudentDC.GetYearwiseStudentDetailsForService(aiSchoolId, aiAcademicYrId, aiStudentId);            
        }

        public static DataTable GetAllStudentsForMessageFacillity(Int32 aiSchoolId, Int32 aiAcademicYrId)
        {
            //This function is used to get all the students by Standard and division id.
            return StudentDC.GetAllStudentsForMessageFacillity(aiSchoolId, aiAcademicYrId);
        }

        public static DataTable GetPendingFeeStudentList(Int32 aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asRegNo, string odtStartDate, bool abLeftStudent, bool abPDCStudent, int aiFeeTypeId, string asPayableFor, string asOperator, int aiAmount, String sortExpression, int maximumRows, int startRowIndex, string asPercentFilter)
        {
			RowCount = 0;
            if (String.IsNullOrEmpty(sortExpression))
                sortExpression = "Std_Div_ID";
			if (asPercentFilter != "2")
				asPercentFilter = string.Empty;				
			else
				asPercentFilter = aiAmount.ToString();
			if(asOperator == "--Select--")
				asOperator=">";

            if(asPayableFor == "0")
                asPayableFor = string.Empty;

            DataTable oDt = StudentDC.GetPendingFeeStudentList(aiSchoolId, aiAcademicYearId, odtStartDate, aiStandardId, aiDivisionId, asRegNo, sortExpression, startRowIndex, maximumRows, abLeftStudent, abPDCStudent, aiFeeTypeId, asPayableFor, asOperator, aiAmount, asPercentFilter);
			if (oDt.Rows.Count > 0)
			{
				if (oDt.Columns.Contains("TotalRows"))
				RowCount = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
			}
			return oDt;
        }
        
        public int GetCountPendingFeeStudentList(Int32 aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId, string asRegNo, string odtStartDate, bool abLeftStudent, bool abPDCStudent, int aiFeeTypeId, string asPayableFor, string asOperator, int aiAmount,string asPercentFilter)
        {
            return RowCount;
        }

        public DataTable GetPendingFeeDetailsForExport(int aiSchoolId, int aiAcademicYearId, DateTime adtDueDate, string asFilterString, string asStdDivFilter, string asAmountFilter, int aiFeeTypeId, string asIsIgnotePDCStudents, string asSortExpr, int aiStartIndex, int aiPageSize, string asOperator, string asPayableFor)
        {
            DataSet ODS = moStudentDC.GetPendingFeeDetailsForExport(aiSchoolId, aiAcademicYearId, adtDueDate, asFilterString, asStdDivFilter, asAmountFilter, aiFeeTypeId, asIsIgnotePDCStudents, asSortExpr, aiStartIndex, aiPageSize, asOperator, asPayableFor);
            return ODS.Tables[0];
        }

        public DataSet GetAllDetailsOfPendingFee(int aiSchoolId, int aiAcademicYearId, DateTime adtDueDate, string asFilterString, string asStdDivFilter, string asAmountFilter, int aiFeeTypeId, string asIsIgnotePDCStudents, string asSortExpr, int aiStartIndex, int aiPageSize, string asOperator, string asPayableFor)
        {
            return moStudentDC.GetPendingFeeDetailsForExport(aiSchoolId, aiAcademicYearId, adtDueDate, asFilterString, asStdDivFilter, asAmountFilter, aiFeeTypeId, asIsIgnotePDCStudents, asSortExpr, aiStartIndex, aiPageSize, asOperator, asPayableFor);
        }


        public void RegenerateStudentRollNo(int iSchoolId, int iAcadmicYearId, int iStdId, int iDivId, string sFilter)
        {
            moStudentDC.RegenerateStudentRollNo(iSchoolId, iAcadmicYearId, iStdId, iDivId, sFilter);
        }

        /// <summary>
        /// This method is used to get data table to fill Dropdownlist of SORTING FIELD.
        /// </summary>
        /// <returns></returns>   
        public DataTable RetriveSortingField()
        {
            return moStudentDC.RetriveSortingField();
        }

        /// <summary>
        /// This method used to get count student with blank registration number.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public static DataTable GetBlankRegNoCount(Int32 aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionId,string asName)
        {
            return StudentDC.GetBlankRegNoCount(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId, asName);
        }

        /// <summary>
        /// This method used to get student regiatration record.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asEnrolmentNumber"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="abIsStudBlankRegNo"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public static DataTable GetStudentsWithEnrolmentNumber(Int32 aiSchoolId, Int32 aiAcademicYearId, string asEnrolmentNumber, Int32 aiStandardId, Int32 aiDivisionId, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix)
        {
            if (abIsStudBlankRegNo)
            {
                asEnrolmentNumber = string.Empty;
                asRegNo = string.Empty;
            }
            return StudentDC.GetStudentsWithEnrolmentNumber(aiSchoolId, aiAcademicYearId, asEnrolmentNumber, aiStandardId, aiDivisionId, abIsStudBlankRegNo, asRegNo, abIsExact,asOperator,asPrefix);
        }

        /// <summary>
        /// This method used to get count of student as per filter.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asEnrolmentNumber"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="abIsStudBlankRegNo"></param>
        /// <returns></returns>
        public static int GetCountStudents(Int32 aiSchoolId, int aiAcademicYearId, string asEnrolmentNumber, Int32 aiStandardId, Int32 aiDivisionId, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix)
        {
            int iRet = 0;
            DataTable oDt = StudentDC.GetStudentsWithEnrolmentNumber(aiSchoolId, aiAcademicYearId, asEnrolmentNumber, aiStandardId, aiDivisionId, abIsStudBlankRegNo, asRegNo, abIsExact,asOperator,asPrefix);
            if (oDt.Rows.Count > 0)
            {
                iRet = Convert.ToInt32(oDt.Rows[0]["TotalRows"]);
            }
            return iRet;
        }

        public void RemoveStudentPhoto(int aiStudentId, int aiSchoolId)
        {
            StudentDC oStudentDC = new StudentDC();
            oStudentDC.RemoveStudentPhoto(aiStudentId, aiSchoolId);
        }



        #endregion

        #endregion

        #region private functions

        public Boolean isRegisterNoAlreadyPresent()
        {
            Boolean bResult = false;
            moStudentDC.StudentDetails = moStudentInfo;
            bResult = moStudentDC.isRegisterNoAlreadyPresent();
            return bResult;

        }
        public Boolean isGeneralRegisterNoAlreadyPresent()
        {
            Boolean bResult = false;
            moStudentDC.StudentDetails = moStudentInfo;
            bResult = moStudentDC.isGeneralRegisterNoAlreadyPresent();
            return bResult;

        }
        public Boolean isStudentUniqueNoAlreadyPresent()
        {
            Boolean bResult = false;
            moStudentDC.StudentDetails = moStudentInfo;
            bResult = moStudentDC.isStudentUniqueNoAlreadyPresent();
            return bResult;

        }
        #endregion private functions

        public static void UpdateAllStudentsLogins(int iSchoolId, string sStudentXml)
        {
            StudentDC.UpdateAllStudentsLogins(iSchoolId, sStudentXml);
        }

		public static DataSet GetAllStudents(int iSchoolId, int iAcademicYrId)
        {
            return StudentDC.GetAllStudents(iSchoolId, iAcademicYrId);
        }

        /// <summary>
        /// This function is used to get Standarwise DOB Message.
        /// </summary>
        /// <param name="iSchoolId"></param>
        /// <param name="StandardId"></param>
        /// <returns></returns>
        public static DataTable GetStandardwiseDOBDetails(int iSchoolId, int StandardId)
        {
            return StudentDC.GetStandardwiseDOBDetails(iSchoolId, StandardId);
        }

        public static DataTable GetAllStudents(int iSchoolId, Char cIsNewStudent, int iAcademicYrId)
        {
            return StudentDC.GetAllStudents(iSchoolId, cIsNewStudent, iAcademicYrId);
        }

        public List<StudentInfo> GetAllStudentForHouseAssignment(int aiSchoolId, int aiAcademicYearID, int aiStandardID, int aiDivisionID, int aiConfigured)
        {
            return moStudentDC.GetAllStudentForHouseAssignment(aiSchoolId, aiAcademicYearID, aiStandardID, aiDivisionID, aiConfigured);
        }

        public void UploadStudentPhoto(Collection<StudentBL> oStudents)
        {
            IEnumerator oIEnum = oStudents.GetEnumerator();
            ArrayList oArrayList = new ArrayList();
            while (oIEnum.MoveNext())
            {
                StudentBL oStudentBL = (StudentBL)oIEnum.Current;
                oArrayList.Add(oStudentBL.GetUpdateStaementForPhotoUpload());
            }

            StudentDC oStudentDC = new StudentDC();
            oStudentDC.UploadStudentPhoto(oArrayList);
        }

        private string GetUpdateStaementForPhotoUpload()
        {
            moStudentDC.StudentDetails = moStudentInfo;
            return moStudentDC.GetUpdateStaementForPhotoUpload();
        }

        /// <summary>
        /// //////////////////////////////////////
        /// </summary>
        /// <param name="ImageBinaryData"></param>
        public void UpdateStudentPhoto(Byte[] ImageBinaryData)
        {
            moStudentDC.UpdateStudentPhoto(ImageBinaryData);
        }
        
		public void UpdateStudentPhotoDetails()
        {
            moStudentDC.StudentDetails = moStudentInfo;
            moStudentDC.UpdateStudentPhotoDetails();
        }
        
		public string GetFormNumber(int aiSchoolId, int aiStudentId, int iAccYearID)
        {
            moStudentDC.StudentDetails = moStudentInfo;
            return moStudentDC.GetFormNumber(aiSchoolId, aiStudentId, iAccYearID);
        }
        
		public int GetFormNoCount(int iCancellationFormNo, int iSchoolId)
        {
            return moStudentDC.GetFormNoCount(iCancellationFormNo, iSchoolId);
        }
        
		public List<StudentInfo> GetStudentDetails(int aiSchoolId,int aiAcademicYearID,int aiStandardID, int aiDivisionID)
        {
            return moStudentDC.GetStudentDetails(aiSchoolId,aiAcademicYearID, aiStandardID, aiDivisionID);    
        }
        /// <summary>
        /// This method is used to Get Student userId from yearwise student Id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public int GetStudentUserId(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            return moStudentDC.GetStudentUserId(aiSchoolId, aiAcademicYearId, aiStudentId);
        }

        public void OverwriteAllSiblingDetails(int aiStudentId, int aiMode, string SiblingId)
        {
            moStudentDC.StudentDetails = moStudentInfo;
            moStudentDC.OverwriteAllSiblingDetails(aiStudentId, aiMode, SiblingId);
        }

		/// <summary>
		/// This method is used to retrive standard wise IDs of RTE student.
		/// </summary>		
		/// <returns></returns>
		public List<int> GetStandardwiseRTEStudentIDs(int aiSchoolId, int aiAcademicYearId,int aiStandardId)
		{
			return moStudentDC.GetStandardwiseRTEStudentIDs(aiSchoolId,aiAcademicYearId,aiStandardId);
		}
        
		public DataTable RetriveStudentInfo(Int32 aiSchoolId, Int32 aiAccYearId, Int32 aiStudentId)
        {
            return moStudentDC.RetriveStudentInfo(aiSchoolId, aiAccYearId, aiStudentId);
        }

        public DataTable RetriveMidYearInfo(Int32 aiSchoolId, Int32 aiAccYearId)
        {
            return moStudentDC.RetriveMidYearInfo(aiSchoolId, aiAccYearId);
        }

        /// <summary>
        /// This method is used to activate all student logins.
        /// </summary>
	    public static void ActivateStudentLogins(int aiSchoolId)
        {
            StudentDC.ActivateStudentLogins(aiSchoolId);
        }

        public StudentDetails GetStudentInfo(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        {
            return moStudentDC.GetStudentInfo(aiSchoolId, aiAcademicYearId, aiStudentId);
        }

        

        /// <summary>
        /// This method is sued to return student mandatory fields.
        /// </summary>
        /// <returns></returns>
        public string GetStudentMandatoryFields(int aiSchoolId)
        {
            return moStudentDC.GetStudentMandatoryFields(aiSchoolId);
        }
		
		public List<StudentInfo> GetStudentDetails(int miSchoolId, int miAcademicYearId, int aiHomeworkId)
        {
            return moStudentDC.GetStudentDetails(miSchoolId, miAcademicYearId, aiHomeworkId);
        }
		
		public static StudentDetailsForSMS GetStudentDetailsForSMS(int aiSchoolId, int aiStudentId)
        {
            return StudentDC.GetStudentDetailsForSMS(aiSchoolId, aiStudentId);
        }

        /// <summary>
        /// This method is used to return student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asRegNo"></param>
        /// <returns></returns>
        public SchoolEntities.Student GetStudentDetails(int aiSchoolId, int aiAcademicYearId, string asRegNo)
        {
            return moStudentDC.GetStudentDetails(aiSchoolId, aiAcademicYearId, asRegNo);
        }

        /// <summary>
        /// This method is used to return student count details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<StandardwiseStudentCount> GetStandardwiseStudentCountDetails(int aiSchoolId, int aiAcademicYearId)
        {
            return moStudentDC.GetStandardwiseStudentCountDetails(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        ///  This method is sued to generate transport fee entries.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolwiseStudentId"></param>
        public void GenerateTrasnportFeeEntry(int aiSchoolId, int aiAcademicYearId, int aiSchoolwiseStudentId)
        {
            moStudentDC.GenerateTrasnportFeeEntry(aiSchoolId, aiAcademicYearId, aiSchoolwiseStudentId);
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
            return this.moStudentDC.GetStudentParentPhotoDetails(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        /// <summary>
        /// This method is used to Save student parents photos.
        /// </summary>
        /// <param name="oStudentParentDetails"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        public void SaveStudentParentPhotoDetails(StudentAdditionalDetails oStudentAdditionalDetails, int aiSchoolId, int aiUserId, bool abSaveForSibling, int aiAcademicYearId)
        {
            this.moStudentDC.SaveStudentParentPhotoDetails(oStudentAdditionalDetails, aiSchoolId, aiUserId, abSaveForSibling, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to Submit the Parent photos.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        public void SubmitStudentParentPhotoDetails(int aiUserId, int aiSchoolId, int aiAcademicYearId, bool abSubmitForSibling)
        {
            this.moStudentDC.SubmitStudentParentPhotoDetails(aiUserId, aiSchoolId, aiAcademicYearId, abSubmitForSibling);
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
            return moStudentDC.GetStudentDetailsForBulkEmail(aiSchoolId, aiAcademicYearId, aiStandardId, aiDivisionId);
        }

        /// <summary>
        /// This method is used to Update Email Address in DB.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="asStudentEmailDetails"></param>
        public void UpdateStudentsEmailInBulk(int aiSchoolId, int aiUserId, string asStudentEmailDetails)
        {
            moStudentDC.UpdateStudentsEmailInBulk(aiSchoolId, aiUserId, asStudentEmailDetails);
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
            return this.moStudentDC.GetTransportPickUpPersonPhotoDetails(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        /// <summary>
        /// This method is used to save Transport PickUp person details.
        /// </summary>
        /// <param name="oStudentAdditionalDetails"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="abSaveForSibling"></param>
        /// <param name="aiAcademicYearId"></param>
        public void SaveTransportPickUpPersonPhotoDetails(StudentAdditionalDetails oStudentAdditionalDetails, int aiSchoolId, int aiUserId, bool abSaveForSibling, int aiAcademicYearId)
        {
            this.moStudentDC.SaveTransportPickUpPersonPhotoDetails(oStudentAdditionalDetails, aiSchoolId, aiUserId, abSaveForSibling, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to submit the Transport PickUp person details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="abSubmitForSibling"></param>
        public void SubmitTransportPickUpPersonPhotoDetails(int aiUserId, int aiSchoolId, int aiAcademicYearId, bool abSubmitForSibling)
        {
            this.moStudentDC.SubmitTransportPickUpPersonPhotoDetails(aiUserId, aiSchoolId, aiAcademicYearId, abSubmitForSibling);
        }
        /// <summary>
        /// This method is used to get stremwise subject details of student
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public DataSet RetriveStudentSubjectInfo(Int32 aiSchoolId, Int32 aiStudentId, int aiAcademicYearId)
        {
            StudentDC moStudentDC = new StudentDC();
            this.moStudentInfo = moStudentDC.StudentDetails;
            return moStudentDC.RetriveStudentSubjectInfo(aiSchoolId, aiStudentId, aiAcademicYearId);
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
            return moStudentDC.GetNonValidRegNos(sRegNos, aiSchoolId, aiAcademicYearId);
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
            moStudentDC.UpdatePhotos(aiSchoolId, aiAcademicYearId, aiInsertedById, alstRegNos);
        }

        public string ValidateTransferStudent(int aiSchoolId, int aiAcademicYearId, string asStudentIdsXML, int aiDivisionId, int aiStandardId)
        {
            return moStudentDC.ValidateTransferStudent(aiSchoolId, aiAcademicYearId, asStudentIdsXML, aiDivisionId, aiStandardId);
        }

        public string GetStudentName(int aiSchoolwiseStudentId)
        {
            return moStudentDC.GetStudentName(aiSchoolwiseStudentId);
        }
		
		/// <summary>
        /// This method is used to delete student from black list.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiId"></param>
        /// <param name="aiUpdatedById"></param>
        public void UpdateBlackListStudent(int aiSchoolId, int aiId, int aiUpdatedById, int aiActionId, int aiSchoolwiseStudentId, string asComment)
        {
            moStudentDC.UpdateBlackListStudent(aiSchoolId, aiId, aiUpdatedById, aiActionId, aiSchoolwiseStudentId, asComment);
        }

        public List<BlackListedStudent> GetAllBlackListedStudents(int aiSchoolId, string asFilter, bool abShowAllLeft, string SortExpression, string SortDirection, int StartRowIndex, int MaximumRows)
        {
            if (asFilter == null)
                asFilter = string.Empty;

            SortExpression = SortExpression.ToLower().Replace("asc", string.Empty).Replace("desc", string.Empty);

            if (SortExpression != string.Empty && SortExpression != null)
                SortExpression = SortExpression + " " + SortDirection;
            else
                SortExpression = "SchoolLeft_Date DESC";

            int iEndRowIndex = StartRowIndex + MaximumRows;
            List<BlackListedStudent> lstBlackListStudent = moStudentDC.GetAllBlackListedStudents(aiSchoolId, asFilter, abShowAllLeft, SortExpression, SortDirection, StartRowIndex, iEndRowIndex);
            if (lstBlackListStudent.Count > 0)
                miTotalRows = lstBlackListStudent[0].TotalRows;
            else
                miTotalRows = 0;
            return lstBlackListStudent;
        }

        public int GetBlackListedStudentsCount(int aiSchoolId, string asFilter, bool abShowAllLeft, string SortExpression, string SortDirection, int StartRowIndex, int MaximumRows)
        {            
            return miTotalRows;
        }

        /// <summary>
        /// Resolves conflict in pending fee screen by calling the StudentDC ResolveConflict method.
        /// </summary>
        /// <param name="aiSchoolId">School ID</param>
        /// <param name="aiAcademicYearId">Academic Year ID</param>
        /// <param name="aiUpdatedById">Updated By User ID</param>
        /// <returns>TransactionResult indicating success or failure with error message</returns>
        public TransactionResult ResolveConflict(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            return moStudentDC.ResolveConflict(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        }
    }

    public class StudentCollectionBL
    {
        private StudentCollectionDC moStudentCollectionDC = null;

        public StudentCollectionBL()
        {
            moStudentCollectionDC = new StudentCollectionDC();
        }
        
		public StudentCollectionBL(int aiSchoolId, int aiAcademicId)
        {
            moStudentCollectionDC = new StudentCollectionDC(aiSchoolId, aiAcademicId);
        }

        public StudentCollectionBL(int aiSchoolId, int aiAcademicId,bool bConsiderLeftStudent)
        {
            moStudentCollectionDC = new StudentCollectionDC(aiSchoolId, aiAcademicId, bConsiderLeftStudent);
        }

        public string InsertMultipleStudents(int aiSchoolId, int aiAcademicYearId, int aiInsertedById,
                                             int aiStandardId, int aiDivisionId, string asStudentDetails)
        {
          return  moStudentCollectionDC.InsertMultipleStudents(aiSchoolId, aiAcademicYearId, aiInsertedById,
                                                         aiStandardId, aiDivisionId, asStudentDetails);
        }

        public DataTable InsertMultipleStudents(int aiSchoolId, int aiAcademicYearId,  int aiInsertedById,
                                             int aiStandardId, int aiDivisionId, string asStudentDetails, int aiUserRoleId, string asShowAdmissionForCurrentYear, bool abKeyValue)
        {
           return  moStudentCollectionDC.InsertMultipleStudents(aiSchoolId, aiAcademicYearId,  aiInsertedById,
                                                         aiStandardId, aiDivisionId, asStudentDetails, aiUserRoleId, asShowAdmissionForCurrentYear, abKeyValue);
        }

        /// <summary>
        /// This method is used to get list of students for selected class teacher.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
		public DataTable GetStudentListOfGivenClassTeacher(int aiStdDivId)
        {
			return moStudentCollectionDC.GetStudentListOfGivenClassTeacher(aiStdDivId);
        }
      
        /// <summary>
        /// This method is used to get list of students for selected class teacher.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public DataTable GetStudentListOfGivenStdDiv(int aiStdDivId)
        {
            return moStudentCollectionDC.GetStudentListOfGivenStdDiv(aiStdDivId);
        }

        public DataSet GetClassStudentsAssociation()
        {
            return moStudentCollectionDC.GetClassStudentsAssociation();
        }
        
		public DataTable GetBirthDayReminder()
        {
            return moStudentCollectionDC.GetBirthDayReminder();
        }

		/// <summary>
		/// Returns the total count of students in the school, including left students.
		/// </summary>
		/// <returns></returns>
		public int GetStudentCount()
		{
			return moStudentCollectionDC.GetStudentCount();	
		}

		public StudentMISDetails GetStudentAttendanceDetails(DateTime adtAttendanceDate)
		{
			return moStudentCollectionDC.GetStudentAttendanceDetails(adtAttendanceDate);
		}
    }
}