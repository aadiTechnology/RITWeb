using System;
using System.Data;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Utility;
using DataCommunicator;
using MasterEntities;
using StandardwiseAcademicYear;
using SchoolEntities;

namespace BusinessLogic
{
    public class SchoolWiseAcademicYearMasterBL
    {
        #region Data members

        private SchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStruct moSchoolWiseAcademicYearMasterStruct;
        private SchoolWiseAcademicYearMasterDC moSchoolWiseAcademicYearMasterDC = new SchoolWiseAcademicYearMasterDC();

        const string S_ACADEMICYEARALREADYDEFINED = "ValAcademicYearAlreadyDefined";

        #endregion

        #region Properties

        public int SchoolWiseAcademicYearId
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.miSchoolWiseAcademicYearId = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.miSchoolId;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.miSchoolId = value;
            }
        }

        public DateTime StartDate
        {

            get { return moSchoolWiseAcademicYearMasterStruct.mdtStartdate; }
            set { moSchoolWiseAcademicYearMasterStruct.mdtStartdate = value; }
        }

        public DateTime EndDate
        {

            get { return moSchoolWiseAcademicYearMasterStruct.mdtEndDate; }
            set { moSchoolWiseAcademicYearMasterStruct.mdtEndDate = value; }
        }
        public DateTime SchoolReOpenDate
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.mdtSchoolReOpenDate;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.mdtSchoolReOpenDate = value;
            }
        }

        public string IsCurrentYear
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.msIsCurrentYear;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.msIsCurrentYear = value;
            }
        }
        public string IsCloseYear
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.msIsCloseYear;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.msIsCloseYear = value;
            }
        }

        public string Is_NewlyCreated
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.msIs_NewlyCreated;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.msIs_NewlyCreated = value;
            }
        }

        public string Is_FinalYear_Generated
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.msIs_FinalYear_Generated;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.msIs_FinalYear_Generated = value;
            }
        }

        public string IsDeleted
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.msIsDeleted;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.msIsDeleted = value;
            }
        }

        public DateTime InsertDate
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.mdtInsertDate;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.mdtInsertDate = value;
            }
        }

        public int InsertedByid
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.miInsertedByid;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.miInsertedByid = value;
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.mdtUpdateDate;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.mdtUpdateDate = value;
            }
        }

        public int UpdatedById
        {
            get
            {
                return moSchoolWiseAcademicYearMasterStruct.miUpdatedById;
            }
            set
            {
                moSchoolWiseAcademicYearMasterStruct.miUpdatedById = value;
            }
        }

        #endregion

        #region Constructors

        public SchoolWiseAcademicYearMasterBL()
        {
        }
        public SchoolWiseAcademicYearMasterBL(Int32 aiSchoolId, Int32 aiAcademicYearId)
        {
            SchoolWiseAcademicYearMasterDC moSchoolWiseAcademicYearMasterDC = new SchoolWiseAcademicYearMasterDC(aiSchoolId, aiAcademicYearId);
            moSchoolWiseAcademicYearMasterStruct = moSchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStructDetails;
        }

        #endregion

        #region Public Methods

        public static List<string> GetYearsForAnnualPalanner(int aiSchoolId)
        {
            return SchoolWiseAcademicYearMasterDC.GetYearsForAnnualPalanner(aiSchoolId);
        }
        public static List<MonthMaster> GetAllMonth()
        {
            return SchoolWiseAcademicYearMasterDC.GetAllMonth();
        }
        public Int32 InsertSchoolWiseAcademicYearMaster()
        {
            moSchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStructDetails = moSchoolWiseAcademicYearMasterStruct;
            return moSchoolWiseAcademicYearMasterDC.InsertSchoolWiseAcademicYearMaster();
        }

        public DataTable GetAllAcademicYearsForSchool(int aiSchoolId, int aiUserId, int aiUserRoleId)
        {
            return SchoolWiseAcademicYearMasterDC.GetAllAcademicYearsForSchool(aiSchoolId, aiUserId, aiUserRoleId);
        }

        /// <summary>
        /// This method is used to get Academic Year details by giving school id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetAllAcademicYearsForSchool(int aiSchoolId)
        {
            return SchoolWiseAcademicYearMasterDC.GetAllAcademicYearsForSchool(aiSchoolId);
        }
        
        public DataTable GetAllAcademicYearsForSuperAdmin(int aiSchoolId, int aiUserId) {
			return SchoolWiseAcademicYearMasterDC.GetAllAcademicYearsForSuperAdmin(aiSchoolId, aiUserId);
        }
        
        public void UpdateSchoolWiseAcademicYearMaster()
        {
            moSchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStructDetails = moSchoolWiseAcademicYearMasterStruct;
            moSchoolWiseAcademicYearMasterDC.UpdateSchoolWiseAcademicYearMaster();
        }

        /// <summary>
        /// This method is used to fetch all Schoolwise Academic Year data. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataTable GetAllSchoolwiseAcademicYearInfo(Int32 aiSchoolId)
        {
            return moSchoolWiseAcademicYearMasterDC.GetAllSchoolwiseAcademicYearInfo(aiSchoolId);
        }

        public void UpdateIsCurrentFlag(Int32 aiSchoolId)
        {
            moSchoolWiseAcademicYearMasterDC.UpdateIsCurrentFlag(aiSchoolId);
        }

        /// <summary>
        /// This method is used to check perdefined start and end date.
        /// </summary>   
        public void IsAcademicYrStartAndEndDtPredefined()
        {
            moSchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStructDetails = moSchoolWiseAcademicYearMasterStruct;
            Int32 iPerdefinedDateCount = moSchoolWiseAcademicYearMasterDC.IsAcademicYrStartAndEndDtPredefined();
            if (iPerdefinedDateCount != Constants.I_ZERO)
            {
                throw new BusinessLogic.HolidaysMasterBL.PerdefinedStartAndEndDate(S_ACADEMICYEARALREADYDEFINED);
            }
        }

        /// <summary>
        /// This method is used to check perdefined start and end date.
        /// </summary>   
        public void IsAcademicYrOtherThanNewDtPredefined()
        {
            moSchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStructDetails = moSchoolWiseAcademicYearMasterStruct;
            Int32 iPerdefinedDateCount = moSchoolWiseAcademicYearMasterDC.IsAcademicYrOtherThanNewDtPredefined();
            if (iPerdefinedDateCount != Constants.I_ZERO)
            {
                throw new BusinessLogic.HolidaysMasterBL.PerdefinedStartAndEndDate(S_ACADEMICYEARALREADYDEFINED);
            }
        }

        /// <summary>
        /// This method is used to get next configured year from database.
        /// </summary>
        /// <returns></returns>
        public DataSet GetNextConfiguredAcademicYear(int miSchoolId, string acAdmissionForCurrentYear)
        {
            return moSchoolWiseAcademicYearMasterDC.GetNextConfiguredAcademicYear(miSchoolId,acAdmissionForCurrentYear);
        }

        /// <summary>
        /// This method is used to get academic year as well school organisation name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataTable GetSchoolInfo(int aiSchoolId, int aiAcademicYearId)
        {
            return moSchoolWiseAcademicYearMasterDC.GetSchoolInfo(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This methos is used to get closed academic years.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataTable GetPassedAcademicYears(int aiSchoolId, int aiStudentid, bool abIncludeCurrentYear = false)
        {
            return SchoolWiseAcademicYearMasterDC.GetPassedAcademicYears(aiSchoolId, aiStudentid, abIncludeCurrentYear);
        }
        public static DataSet GetPendingFeeAcademicYears(int aiSchoolId, int aiStudentid, int aiAcademicYearId, bool bIsInternalFee, bool abIsAdvanceFee)
        {
            return SchoolWiseAcademicYearMasterDC.GetPendingFeeAcademicYears(aiSchoolId, aiStudentid, aiAcademicYearId, bIsInternalFee, abIsAdvanceFee);
        }

        /// <summary>
        /// This method is used to return mid or current academic year depending on ShowAdmissionForCurrentYear flag.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static string GetAcademicYearForOnlineAdmission(int aiSchoolId)
        {
            return SchoolWiseAcademicYearMasterDC.GetAcademicYearForOnlineAdmission(aiSchoolId);
        }


        public void GenerateNextYearData(string asConfIds, DateTime oStartDate, DateTime oEndDate, Boolean bGenerateRollNos, Boolean bGenerateRegNos, Boolean bGenerateDebitEntries,Boolean bGenerateTransportData, Boolean bIsOnlyInMidAcademic)
        {
            moSchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStructDetails = moSchoolWiseAcademicYearMasterStruct;
            moSchoolWiseAcademicYearMasterDC.GenerateNextYearData(asConfIds, oStartDate, oEndDate, bGenerateRollNos, bGenerateRegNos,bGenerateDebitEntries,bGenerateTransportData, bIsOnlyInMidAcademic);
        }

        public string IsNewlyCreated(int aiSchoolId, int aiAcademicYearId)
        {
            return moSchoolWiseAcademicYearMasterDC.IsNewlyCreated(aiSchoolId, aiAcademicYearId);
        }

		/// <summary>
		/// This method returns the current academic year id.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <returns></returns>
		public int GetCurrentAcademicYearId(int aiSchoolId)
	    {
		    
		    DataTable oDataTable = GetAllSchoolwiseAcademicYearInfo(aiSchoolId);
		    DataRow[] oDataRow = oDataTable.Select(" is_current_year = '" + Constants.C_YES + "'");
		    return oDataRow[0]["Academic_Year_ID"].ToInt();
	    }

	    #endregion

        public static List<StandardwiseAcademicYearEntity> GetStandardwiseAcademicYear(int iSchoolId, int iAcademicYearId)
        {
            return SchoolWiseAcademicYearMasterDC.GetStandardwiseAcademicYear(iSchoolId, iAcademicYearId);
        }

        public void SaveStandardwiseAcademicYear(string StandardwiseAcademicYearXML)
        {
            moSchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStructDetails = moSchoolWiseAcademicYearMasterStruct;
            moSchoolWiseAcademicYearMasterDC.SaveStandardwiseAcademicYear(StandardwiseAcademicYearXML);
        }

        public void CheckOverlappingofStandardwiseAcademicYear(string StandardwiseAcademicYearXML, out string Message)
        {
            moSchoolWiseAcademicYearMasterDC.SchoolWiseAcademicYearMasterStructDetails = moSchoolWiseAcademicYearMasterStruct;
            moSchoolWiseAcademicYearMasterDC.CheckOverlappingofStandardwiseAcademicYear(StandardwiseAcademicYearXML, out Message);
        }

        public static DataTable GetAcademicDatesForStandard(int iSchoolID, int iAcademicYearID, int StandardId)
        {
            return SchoolWiseAcademicYearMasterDC.GetAcademicDatesForStandard(iSchoolID, iAcademicYearID, StandardId);
        }

        public static DataTable GetAcademicDatesForStudent(int iSchoolID, int iAcademicYearID, string RegNo)
        {
            return SchoolWiseAcademicYearMasterDC.GetAcademicDatesForStudent(iSchoolID, iAcademicYearID, RegNo);   
        }

        public static DataTable GetAcademicYearForStudent(int iSchoolID, int iAcademicYearID, int StudentId)
        {
            return SchoolWiseAcademicYearMasterDC.GetAcademicYearForStudent(iSchoolID, iAcademicYearID, StudentId);   

        }

        public static DataTable GetAcademicDatesForStandardDivision(int iSchoolId, int iAcademicYearId, int StandardDivisionId)
        {
            return SchoolWiseAcademicYearMasterDC.GetAcademicDatesForStandardDivision(iSchoolId, iAcademicYearId, StandardDivisionId);   
        }
        
        /// <summary>
        /// This method is used to check report is empty or not.
        /// </summary>
        /// <param name="asStandardwiseAcademicYearXML"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="aiAcadYearID"></param>
        /// <returns></returns>
        public static bool IsReportEmpty(string asStandardwiseAcademicYearXML, int aiSchoolID, int aiAcadYearID)
        {
            return SchoolWiseAcademicYearMasterDC.IsReportEmpty(asStandardwiseAcademicYearXML, aiSchoolID, aiAcadYearID);
        }

        public static List<AcademicYear> GetAllYears(int aiSchoolId)
		{
			return SchoolWiseAcademicYearMasterDC.GetAllYears(aiSchoolId);
		}

        public DataTable GetAcademicYearsforStudentFeeChallan(int aiSchoolId, int aiAcademicYearId, int aiStudentId)
        { 
            SchoolWiseAcademicYearMasterDC oSchoolWiseAcademicYearMasterDC = new SchoolWiseAcademicYearMasterDC();
            return oSchoolWiseAcademicYearMasterDC.GetAcademicYearsforStudentFeeChallan(aiSchoolId, aiAcademicYearId, aiStudentId);
        }

        /// <summary>
        /// This method is used for get Student Details for Challan import.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public DataTable GetStudentIdAndStandardIdForChallan(int aiSchoolId, int aiAcademicYearId, int aiAcademicYrId, int aiStudentId)
        {
            SchoolWiseAcademicYearMasterDC oSchoolWiseAcademicYearMasterDC = new SchoolWiseAcademicYearMasterDC();
            return oSchoolWiseAcademicYearMasterDC.GetStudentIdAndStandardIdForChallan(aiSchoolId, aiAcademicYearId, aiAcademicYrId, aiStudentId);
        }
    }
}
