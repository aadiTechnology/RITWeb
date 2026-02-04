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


namespace BusinessLogic
{
   public  class LectureWiseAttendanceDetailsBL
    {
        #region Data members

        private LectureWiseAttendanceDetailsDC.LectureWiseAttendanceDetailsStruct moSchoolWiseAttendanceDetailsStruct;
        private LectureWiseAttendanceDetailsDC moAttendanceDetailsDC = new LectureWiseAttendanceDetailsDC();
        private Constants.Action eAction;

        #endregion


        #region Properties

        //public DayDetails DayDetails
        //{
        //    get
        //    {
        //        return moAttendanceDetailsDC.DayDetails;
        //    }
        //}

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

        public int LectureNo
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miLectureNo;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miLectureNo = value;
            }
        }

        public int SubjectId
        {
            get
            {
                return moSchoolWiseAttendanceDetailsStruct.miSubjectId;
            }
            set
            {
                moSchoolWiseAttendanceDetailsStruct.miSubjectId = value;
            }
        }

        #endregion

        #region Constructors

        public LectureWiseAttendanceDetailsBL()
        {
        }
        public LectureWiseAttendanceDetailsBL(int aiId)
        {
            LectureWiseAttendanceDetailsDC moAttendanceDetailsDC = new LectureWiseAttendanceDetailsDC(aiId);
        }
        #endregion



        /// <summary>
        /// This method is used to mark the student's attendance.
        /// </summary>
        /// <param name="sAttendanceXML"></param>
        public void MarkStudentAttendence(string sAttendanceXML, int LectureNo, int SubjectId)
        {
            moAttendanceDetailsDC.SchoolWiseAttendanceDetailsStructDetails = moSchoolWiseAttendanceDetailsStruct;
            moAttendanceDetailsDC.MarkStudentAttendence(sAttendanceXML, LectureNo, SubjectId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiFlag"></param>
        /// <param name="adtTodaysDate"></param>
        /// <returns></returns>
        public DataSet FetchAttendenceDetails(int aiSchoolId, Int32 aiAcademicYearId, Int32 aiStandardDivisionId, DateTime adtTodaysDate, int LectureNo, int SubjectId)
        {
            return moAttendanceDetailsDC.FetchAttendenceDetails(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, adtTodaysDate, LectureNo, SubjectId);
        }

        public static void DeleteSchoolWiseAttendanceDetails(int aiSchoolId, int aiAcademicYearId, string asAttendanceDate, int aiStdDivId, int LectureNo, int SubjectId)
        {
            LectureWiseAttendanceDetailsDC.DeleteSchoolWiseAttendanceDetails(aiSchoolId, aiAcademicYearId, asAttendanceDate, aiStdDivId, LectureNo, SubjectId);
        }
    }



}
