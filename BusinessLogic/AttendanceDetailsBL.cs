// File Name    : SchoolwiseAttendanceDetailsBL.cs
// Created By   : Ketan
// Crested Date : 6/12/2007  
using System;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;
using DataCommunicator;
using System.Collections.Generic;
using SchoolEntities.Admin;
using SchoolEntities;
using SchoolEntities.Teacher;

namespace BusinessLogic
{
    /// <summary>
    /// This Class is used to performed insert,delete and update opertation on SchoolWise_Attendance_Details. 
    /// </summary>
    public class AttendanceDetailsBL
    {
        #region Data members

        private AttendanceDetailsDC.SchoolWiseAttendanceDetailsStruct moSchoolWiseAttendanceDetailsStruct;
        private AttendanceDetailsDC moAttendanceDetailsDC = new AttendanceDetailsDC();
        private Constants.Action eAction;

        #endregion

        #region Properties

        public DayDetails DayDetails
        {
            get
            {
                return moAttendanceDetailsDC.DayDetails;
            }
        }

        public Constants.Action ConfigurationAction
        {
            get
            {
                return eAction;
            }
            set
            {
                eAction = value;
            }
        }

        public int StandardDivisionId
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miStandardDivisionId;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miStandardDivisionId = value;
            }
        }

        public int AcademicYearId
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miAcademicYearId;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miAcademicYearId = value;
            }
        }

        public int SchoolWiseAttendanceId
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miSchoolWiseAttendanceId;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miSchoolWiseAttendanceId = value;
            }
        }

        public int SchoolId
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miSchoolId;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miSchoolId = value;
            }
        }

        public DateTime AttendanceDate
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.mdtAttendanceDate;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.mdtAttendanceDate = value;
            }
        }

        public int StudentId
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miStudentId;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miStudentId = value;
            }
        }

        public string IsPresent
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.msIsPresent;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.msIsPresent = value;
            }
        }

        public string IsDeleted
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.msIsDeleted;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.msIsDeleted = value;
            }
        }

        public DateTime InsertDate
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.mdtInsertDate;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.mdtInsertDate = value;
            }
        }

        public int InsertedByid
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miInsertedByid;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miInsertedByid = value;
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.mdtUpdateDate;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.mdtUpdateDate = value;
            }
        }

        public int UpdatedById
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miUpdatedById;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miUpdatedById = value;
            }
        }


        #endregion

        #region Constructors

        public AttendanceDetailsBL()
        {
        }
        public AttendanceDetailsBL(int aiId)
        {
            AttendanceDetailsDC moAttendanceDetailsDC = new AttendanceDetailsDC(aiId);
        }
        #endregion

        #region Public Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiFlag"></param>
        /// <param name="adtTodaysDate"></param>
        /// <returns></returns>
        public DataSet FetchAttendenceDetails(int aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardDivisionId, DateTime adtTodaysDate, int aiUserId)
        {
            return moAttendanceDetailsDC.FetchAttendenceDetails(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, adtTodaysDate, aiUserId);
        }

        /// <summary>
        /// This method is used to return select statement.
        /// </summary>
        /// <returns></returns>

        public string InsertSchoolWiseAttendanceDetails()
        {
            moAttendanceDetailsDC.SchoolWiseAttendanceDetailsStructDetails = moSchoolWiseAttendanceDetailsStruct;
            return moAttendanceDetailsDC.InsertSchoolWiseAttendanceDetails();
        }

        /// <summary>
        /// This method is used to return update statement.
        /// </summary>

        public string UpdateSchoolWiseAttendanceDetails()
        {
            moAttendanceDetailsDC.SchoolWiseAttendanceDetailsStructDetails = moSchoolWiseAttendanceDetailsStruct;
            return moAttendanceDetailsDC.UpdateSchoolWiseAttendanceDetails();
        }

        /// <summary>
        /// This method is used to return delete statement.
        /// </summary>

        //public string DeleteSchoolWiseAttendanceDetails()
        //{
        //    moSchoolWiseAttendanceDetailsDC.SchoolWiseAttendanceDetailsStructDetails = moSchoolWiseAttendanceDetailsStruct;
        //    return moSchoolWiseAttendanceDetailsDC.DeleteSchoolWiseAttendanceDetails();
        //}

        public static void DeleteSchoolWiseAttendanceDetails(int aiSchoolId, int aiAcademicYearId, string asAttendanceDate, int aiStdDivId)
        {
            AttendanceDetailsDC.DeleteSchoolWiseAttendanceDetails(aiSchoolId, aiAcademicYearId, asAttendanceDate, aiStdDivId);
        }

        /// <summary>
        /// This method is used to fetch attendance of student.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet FetchStudentAttendanceForCalender(Int32 aiSchoolID, Int32 aiStudentID,
                                    Int32 aiAcademicYearId, Int32 aiStandardId, Int32 aiDivisionid, Int32 aiMonthId, Int32 aiYear)
        {
            return moAttendanceDetailsDC.FetchStudentAttendanceForCalender(aiSchoolID, aiStudentID,
                                    aiAcademicYearId, aiStandardId, aiDivisionid, aiMonthId, aiYear);
        }

        public DataTable GetTeachersForLecturewiseAttendance(int aiSchoolId, int aiAcademicYearId, int aiLoginUserId)
        {
            return moAttendanceDetailsDC.GetTeachersForLecturewiseAttendance(aiSchoolId, aiAcademicYearId, aiLoginUserId);
        }
        
        public DataSet FetchStudentMonthlyAttendance(int aiSchoolID, int aiStudentID, int aiAcademicYearId, int aiMonthId, int aiYear)
        {
            return moAttendanceDetailsDC.FetchStudentMonthlyAttendance(aiSchoolID, aiStudentID,
                                     aiAcademicYearId, aiMonthId, aiYear);
        }

        /// <summary>
        /// This method is used to check for non working day.
        /// </summary>
        /// <param name="adtTodaysDate"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public string CheckWeekEnd(DateTime adtTodaysDate, Int32 aiSchoolId, Int32 aiAcademicYearId, Int32 iStandardDivisionId)
        {
            string sResult = string.Empty;
            DataTable oDataTable = moAttendanceDetailsDC.CheckTodaysDay(adtTodaysDate, aiSchoolId, aiAcademicYearId, 0, iStandardDivisionId).Tables[0];
            if ((oDataTable != null) && (Convert.ToInt32(oDataTable.Rows[0][0]) == 0))
                sResult = "Selected date is weekend. ";

            oDataTable = moAttendanceDetailsDC.CheckTodaysDay(adtTodaysDate, aiSchoolId, aiAcademicYearId, 0, iStandardDivisionId).Tables[1];
            if ((oDataTable != null) && (Convert.ToInt32(oDataTable.Rows[0][0]) > 0))
                sResult = "Selected date is holiday. ";


            return sResult;
        }

        /// <summary>
        /// This method is used to check for non working day.
        /// </summary>
        /// <param name="adtTodaysDate"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet IsDayNonWorking(DateTime adtTodaysDate, Int32 aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardId)
        {
            return moAttendanceDetailsDC.CheckTodaysDay(adtTodaysDate, aiSchoolId, aiAcademicYearId, aiStandardId, 0);
        }

        /// <summary>
        /// This method is used to check if attendance is marked before the given date or not.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public bool CheckIfAttendanceMarked(DateTime dateTime, int iStandardId, int iDivisionId)
        {
            return moAttendanceDetailsDC.CheckIfAttendanceMarked(dateTime, iStandardId, iDivisionId);
        }

        /// <summary>
        /// This method is used to mark the student's attendance.
        /// </summary>
        /// <param name="sAttendanceXML"></param>
        public void MarkStudentAttendence(string sAttendanceXML, bool abSendMessage)
        {
            moAttendanceDetailsDC.SchoolWiseAttendanceDetailsStructDetails = moSchoolWiseAttendanceDetailsStruct;
            moAttendanceDetailsDC.MarkStudentAttendence(sAttendanceXML, abSendMessage);
        }

        public void MarkStudentMonthlyAttendence(string sAttendanceXML, int aiYear, int aiMonth)
        {
            moAttendanceDetailsDC.SchoolWiseAttendanceDetailsStructDetails = moSchoolWiseAttendanceDetailsStruct;
            moAttendanceDetailsDC.MarkStudentMonthlyAttendence(sAttendanceXML, aiYear, aiMonth);
        }

        /// <summary>
        /// This method is used to get the attendance details for attendance status
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aSelectedDate"></param>
        /// <returns></returns>
        public List<ClasswiseAttendanceStatus> Get(int aiSchoolId, int aiAcademicYearId, string aSelectedDate)
        {
            return moAttendanceDetailsDC.Get(aiSchoolId, aiAcademicYearId, aSelectedDate);
        }

        /// <summary>
        /// This method is used to get absent student Ids.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="adSelectDate"></param>
        /// <param name="aiMaxAbsentDyasLimit"></param>
        /// <returns></returns>
        public List<int> GetAbsentStudentIds(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, DateTime adSelectDate, int aiMaxAbsentDyasLimit, out List<int> aolstHalfDayPresentStudentId)
        {
            return moAttendanceDetailsDC.GetAbsentStudentIds(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, adSelectDate, aiMaxAbsentDyasLimit,out aolstHalfDayPresentStudentId);
        }

        /// <summary>
        /// This method is used to get absent student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aistudentIds"></param>
        /// <returns></returns>
        public List<AttendanceDetails> GetAbsentStudentDetails(int aiSchoolId, int aiAcademicYearId, string aistudentIds, int aiStandardDivisionId, DateTime adtSelectedDate,string asHalfDayAttendanceStudentIds, out List<AttendanceDetails> lstHalfDayStudentAttendanceDetails)
        {
            return moAttendanceDetailsDC.GetAbsentStudentDetails(aiSchoolId, aiAcademicYearId, aistudentIds, aiStandardDivisionId, adtSelectedDate,asHalfDayAttendanceStudentIds,out lstHalfDayStudentAttendanceDetails);
        }

        /// <summary>
        /// This method is used to get class name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <returns></returns>
        public string GetClassName(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            return moAttendanceDetailsDC.GetClassName(aiSchoolId, aiAcademicYearId, aiStandardDivisionId);
        }

        /// <summary>
        /// This Method is used to mark monthwise attendance for all classes one time.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <param name="dtAttendanceDate"></param>
        /// <param name="aiUpdatedById"></param>
        public void MarkClassMothwiseAttendance(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, DateTime dtAttendanceDate, int aiUpdatedById, bool abOverriteAttendance)
        {
            moAttendanceDetailsDC.MarkClassMothwiseAttendance(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, dtAttendanceDate, aiUpdatedById, abOverriteAttendance);
        }
        
	      public List<CoordinateDetails> GetCoordinatorDetails(int aiSchoolId, int aiAcademicYearId)
	      {
	        return  moAttendanceDetailsDC.GetCoordinatorDetails(aiSchoolId, aiAcademicYearId);
	      }

		#endregion
    }

    /// <summary>
    /// This collection class is used to update and insert all student attendance configuration details. 
    /// </summary>
    public class AttendanceCollectionBL : IEnumerable
    {
        #region DataMember

        private Collection<AttendanceDetailsBL> moAttendanceConfigListBL = null;
        StudentAttendanceCollectionDC oStudentAttendanceCollectionDC;

        #endregion

        #region Properties

        public Collection<AttendanceDetailsBL> AttendanceConfigListBL   //WeekdaysConfigListBL
        {
            get
            {
                return moAttendanceConfigListBL;
            }
            set
            {
                moAttendanceConfigListBL = value;
            }
        }

        #endregion

        #region Constructor

        public AttendanceCollectionBL()
        {
            moAttendanceConfigListBL = new Collection<AttendanceDetailsBL>();
            oStudentAttendanceCollectionDC = new StudentAttendanceCollectionDC();
        }

        #endregion

        #region Public Method

        /// <summary>
        /// This method is used to add collection data.
        /// </summary>
        /// <param name="aoWeekDaysMasterBL"></param>
        public void Add(AttendanceDetailsBL aoSchoolWiseAttendanceDetailsBL)
        {
            moAttendanceConfigListBL.Add(aoSchoolWiseAttendanceDetailsBL);
        }

        /// <summary>
        /// This method is used to remove collection data.
        /// </summary>
        /// <param name="aoWeekDaysMasterBL"></param>
        public void Remove(AttendanceDetailsBL aoSchoolWiseAttendanceDetailsBL)
        {
            moAttendanceConfigListBL.Remove(aoSchoolWiseAttendanceDetailsBL);
        }

        public IEnumerator GetEnumerator()
        {
            return new AttendanceCollectionEnumerator(this);
        }

        /// <summary>
        /// This method is used to update all student attendance configuration details.
        /// </summary>
        //public void UpdateAttendanceConfigurationDetails()
        //{
        //    IEnumerator oEnum = moAttendanceConfigListBL.GetEnumerator();
        //    StringBuilder oSB = new StringBuilder();
        //    while (oEnum.MoveNext())
        //    {
        //        SchoolWiseAttendanceDetailsBL oWeekDaysMasterBL = (SchoolWiseAttendanceDetailsBL)oEnum.Current;
        //        switch (oWeekDaysMasterBL.ConfigurationAction)
        //        {
        //            case Constants.Action.Insert:
        //                oSB.Append(((SchoolWiseAttendanceDetailsBL)oEnum.Current).InsertSchoolWiseAttendanceDetails());
        //                break;

        //            case Constants.Action.Delete:
        //                oSB.Append(((SchoolWiseAttendanceDetailsBL)oEnum.Current).DeleteSchoolWiseAttendanceDetails());
        //                break;
        //            case Constants.Action.Update:
        //                oSB.Append(((SchoolWiseAttendanceDetailsBL)oEnum.Current).UpdateSchoolWiseAttendanceDetails());
        //                break;
        //        }
        //        oSB.Append(";");
        //    }
        //    oStudentAttendanceCollectionDC.UpdateAttendanceConfiguration(oSB);
        //}


        #endregion

        #region " public static methods "

        public static void MarkAttendanceForTestDdate(int aiSchoolId, int aiAcademicYearId, int aiStandatdDivisionId, int aiTestId, int aiSubjectId, int aiInsertedById)
        {
            StudentAttendanceCollectionDC.MarkAttendanceForTestDdate(aiSchoolId, aiAcademicYearId, aiStandatdDivisionId, aiTestId, aiSubjectId, aiInsertedById);
        }

        #endregion " public static methods "

        private class AttendanceCollectionEnumerator : IEnumerator
        {
            #region DataMember

            private int position = -1;
            private AttendanceCollectionBL moAttendanceCollectionBL;

            #endregion

            #region Constructor

            public AttendanceCollectionEnumerator(AttendanceCollectionBL aoAttendanceCollectionBL)
            {
                moAttendanceCollectionBL = aoAttendanceCollectionBL;
            }

            #endregion

            #region Public Method
            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (position < moAttendanceCollectionBL.moAttendanceConfigListBL.Count - 1)
                {
                    position++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                position = -1;
            }
            #endregion

            #region Property
            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return moAttendanceCollectionBL.moAttendanceConfigListBL[position];
                }
            }
            #endregion
        }
    }
}
