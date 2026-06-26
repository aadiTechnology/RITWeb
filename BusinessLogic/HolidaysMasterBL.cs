// File Name  : HolidaysMasterBL.cs
// Created By : Ketan
// Date       : 28/11/2007   

using System;
using System.Data;
using Utility;
using DataCommunicator;
using System.Collections.Generic;
using SchoolEntities;
namespace BusinessLogic
{
    /// <summary>
    /// This class is used to performed insert,delete and update opertation on Weekdays_Master.  
    /// </summary>
    public class HolidaysMasterBL
    {
        #region Data members

        private HolidaysMasterDC.HolidaysMasterStruct moHolidaysMasterStruct;
        private HolidaysMasterDC moHolidaysMasterDC = new HolidaysMasterDC();

        const string S_HOLIDAYNAME = "ValSameHolidayName";
        const string S_HOLIDAYDEFINE = "ValHolidayDefine";

        #endregion

        #region Properties

        
        public int AcademicYearId
        {
            get
            {
                return moHolidaysMasterStruct.miAcademicYearId;  
            }
            set 
            {
                moHolidaysMasterStruct.miAcademicYearId=value;
            }
        }
        public int SchoolId
        {
            get
            {
                return moHolidaysMasterStruct.miSchoolId;
            }
            set
            {
                moHolidaysMasterStruct.miSchoolId = value;
            }
        }
        public int HolidayId
        {
            get
            {
                return moHolidaysMasterStruct.miHolidayId;
            }
            set
            {
                moHolidaysMasterStruct.miHolidayId = value;
            }
        }

        public string HolidayName
        {
            get
            {
                return moHolidaysMasterStruct.msHolidayName;
            }
            set
            {
                moHolidaysMasterStruct.msHolidayName = value;
            }
        }

        public DateTime HolidayStartDate
        {
            get
            {
                return moHolidaysMasterStruct.mdtHolidayStartDate;
            }
            set
            {
                moHolidaysMasterStruct.mdtHolidayStartDate = value;
            }
        }

        public string AssoiciatedStandards
        {
            get { return moHolidaysMasterStruct.AssociatedStandards; }
            set { moHolidaysMasterStruct.AssociatedStandards = value; }
        }

        public int InsertedById
        {
            get { return moHolidaysMasterStruct.miInsertedById; }
            set { moHolidaysMasterStruct.miInsertedById = value; }
        }

        public DateTime HolidayEndDate
        {
            get
            {
                return moHolidaysMasterStruct.mdtHolidayEndDate;
            }
            set
            {
                moHolidaysMasterStruct.mdtHolidayEndDate = value;
            }
        }

        public string Remarks
        {
            get
            {
                return moHolidaysMasterStruct.msRemarks;
            }
            set
            {
                moHolidaysMasterStruct.msRemarks = value;
            }
        }

        public bool AllowOverLapping
        {
            get
            {
                return moHolidaysMasterStruct.mbAllowOverLapping;
            }
            set
            {
                moHolidaysMasterStruct.mbAllowOverLapping = value;
            }
        }

        public string IsDeleted
        {
            get
            {
                return moHolidaysMasterStruct.msIsDeleted;
            }
            set
            {
                moHolidaysMasterStruct.msIsDeleted = value;
            }
        }

        public DateTime InsertDate
        {
            get
            {
                return moHolidaysMasterStruct.mdtInsertDate;
            }
            set
            {
                moHolidaysMasterStruct.mdtInsertDate = value;
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                return moHolidaysMasterStruct.mdtUpdateDate;
            }
            set
            {
                moHolidaysMasterStruct.mdtUpdateDate = value;
            }
        }

        #endregion

        #region Constructors

        public HolidaysMasterBL()
        {
        }
        public HolidaysMasterBL(int aiHolidayId,int aiSchoolId, int aiAcademicYearId) 
        {
            HolidaysMasterDC moHolidaysMasterDC = new HolidaysMasterDC(aiHolidayId,aiSchoolId,aiAcademicYearId);
            moHolidaysMasterStruct = moHolidaysMasterDC.HolidaysMasterStructDetails;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// This method is used to insert all holiday configuration.
        /// </summary>
        public void InsertHolidaysMaster()
        {
            moHolidaysMasterDC.HolidaysMasterStructDetails = moHolidaysMasterStruct;
            moHolidaysMasterDC.InsertHolidaysMaster();
        }

        /// <summary>
        /// This method is used to fill Holiday grid.
        /// </summary>
        /// <param name="aiSchoolId"></param>       
        /// <param name="aiAccYrId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="maximumRows"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public DataTable GetHolidayDetails(int aiSchoolId, int aiAccYrId, String sortExpression, int maximumRows, int startRowIndex)
        {
            int iStandardId = 0;
            int iDivisionId = 0;
            if ((Constants.UserRoles)System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Student)
            {
                iStandardId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
                iDivisionId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_STUDENT_DIVISION_ID]);
            }

            return moHolidaysMasterDC.GetHolidayDetails(aiSchoolId, aiAccYrId, sortExpression, Constants.I_GRID_PAGE_COUNT, startRowIndex, iStandardId,iDivisionId);
        }

        /// <summary>
        /// This method is used to fill Holiday grid.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAccYrId"></param>
        /// <returns></returns>
        public List<Holiday> GetHolidayDetails(int aiSchoolId, int aiAccYrId)
        {           
            return moHolidaysMasterDC.GetHolidayDetails(aiSchoolId, aiAccYrId);
        }

        /// <summary>
        /// This method is used to count total records.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAccYrId"></param>
        /// <returns></returns>
        public int GetHolidayCount(int aiSchoolId, int aiAccYrId, String sortExpression)
        {
            int iStandardId = 0;
            int iDivisionId = 0;
            if ((Constants.UserRoles)System.Web.HttpContext.Current.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Student)
            {
                iStandardId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
                iDivisionId = Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_STUDENT_DIVISION_ID]);
            }
            return moHolidaysMasterDC.GetHolidayCount(aiSchoolId, Convert.ToInt32(System.Web.HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]),iStandardId,iDivisionId);
        }   
        /// <summary>
        /// This method is used to update all holiday configuration.
        /// </summary>
        public void UpdateHolidaysMaster()
        {
            moHolidaysMasterDC.HolidaysMasterStructDetails = moHolidaysMasterStruct;
            moHolidaysMasterDC.UpdateHolidaysMaster();
        }

        /// <summary>
        /// This method is used to delete holiday configuration from Holidays_Master.
        /// </summary>
        public void DeleteHolidaysMaster()
        {
            moHolidaysMasterDC.HolidaysMasterStructDetails = moHolidaysMasterStruct;
            moHolidaysMasterDC.DeleteHolidaysMaster();
        }
        
        /// <summary>
        /// This method is used to check duplicate holiday name. 
        /// </summary>
        /// <param name="asHolidayName"></param>
        /// <returns></returns>
        public void IsHolidayNameDuplicate()//string asHolidayName, int aiSchoolId, int aiAcademicYearId, int aiHolidayId)
        {
            moHolidaysMasterDC.HolidaysMasterStructDetails = moHolidaysMasterStruct;
            Int32 iHolidayNameCount= moHolidaysMasterDC.CheckForDuplicateHolidayName();//asHolidayName,aiSchoolId,aiAcademicYearId,aiHolidayId);
            if (iHolidayNameCount != Constants.I_ZERO)
            {
                throw new BusinessLogic.HolidaysMasterBL.DuplicateHolidayName(S_HOLIDAYNAME);
            }
        }

        /// <summary>
        /// This method is used to check perdefined start and end date.
        /// </summary>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <returns></returns>
        public void IsHolidayStartAndEndDatePredefined() //DateTime adtStartDate, DateTime adtEndDate,int aiSchoolId,int aiAcademicYearId,int aiHolidayId)
        {
            moHolidaysMasterDC.HolidaysMasterStructDetails = moHolidaysMasterStruct;
            Int32 iPerdefinedDateCount = moHolidaysMasterDC.IsHolidayStartAndEndDatePredefined();//adtStartDate, adtEndDate, aiSchoolId, aiAcademicYearId, aiHolidayId);
            if (iPerdefinedDateCount != Constants.I_ZERO)
            {
                throw new BusinessLogic.HolidaysMasterBL.PerdefinedStartAndEndDate(S_HOLIDAYDEFINE);
            }
        }

        public static bool IsDateHoliday(int aiSchoolId, int aiAcademicYearId, DateTime aoDt)
        {
            
            return HolidaysMasterDC.IsDateHoliday(aiSchoolId,aiAcademicYearId,aoDt);
        }

        /// <summary>
        /// This method is used to get upcoming holiday date.
        /// </summary>
        /// <param name="aiSchooId"></param>
        /// <param name="aiAccYearId"></param>
        /// <returns></returns>
        public static DateTime GetUpcomingHolidayDate(int aiSchooId, int aiAccYearId, int aiStdId, int aiDivId)
        {
           DataTable oDataTable =  HolidaysMasterDC.GetUpcomingHolidayDate(aiSchooId, aiAccYearId,aiStdId,aiDivId);
           if (oDataTable != null && oDataTable.Rows.Count > 0)
               return Convert.ToDateTime(oDataTable.Rows[0][0]);
           else
               return DateTime.MinValue;
        }


        #endregion

        #region Exception Class
        
        public class DuplicateHolidayName : Exception
        {
            private string msMessage = " ";
            public override string Message
            {
                get
                {
                    return msMessage;
                }
            }
            public DuplicateHolidayName(string asMessage)
            {
                msMessage = asMessage;
            }
        }

        public class PerdefinedStartAndEndDate : Exception
        {
            private string msMessage = " ";
            public override string Message
            {
                get
                {
                    return msMessage;
                }
            }
            public PerdefinedStartAndEndDate(string asMessage)
            {
                msMessage = asMessage;
            }
        }

        public class NonWorkingDay : Exception
        {
            private string msMessage = "";
            public override string Message
            {
                get
                {
                    return msMessage;
                }
            }
            public NonWorkingDay(string asMessage)
            {
                msMessage = asMessage;
            }
        }

        #endregion
    }
}
