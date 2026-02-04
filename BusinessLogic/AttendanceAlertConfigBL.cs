// -----------------------------------------------------------------------
//  FileName	: AttendanceAlertConfigurationBL.cs
//	Created by	: Pravin
//	Date		: 5 May 2012
//	Description	: This class is used to Adding,Removing Users From Attendance Mail Configuration
// -----------------------------------------------------------------------

namespace BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using DataCommunicator;
    using BookEntities;
    using SchoolEntities.Admin;
    using MasterEntities;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AttendanceAlertConfigBL
    {

        AttendanceAlertConfigDC moAttendanceAlertConfigDC;

        /// <summary>
        /// this is a default constructor.
        /// </summary>
        public AttendanceAlertConfigBL()
        {
            moAttendanceAlertConfigDC = new AttendanceAlertConfigDC();
        }

        /// <summary>
        /// This is a constructor to initialize the members.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        public AttendanceAlertConfigBL(int aiSchoolId, int aiAcademicYearId, int aiConfigId)
        {
            moAttendanceAlertConfigDC = new AttendanceAlertConfigDC(aiSchoolId, aiAcademicYearId, aiConfigId);
        }

        /// <summary>
        /// This is a constructor to initialize the members.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        public AttendanceAlertConfigBL(int aiSchoolId, int aiAcademicYearId)
        {
            moAttendanceAlertConfigDC = new AttendanceAlertConfigDC(aiSchoolId, aiAcademicYearId);
        }


        /// <summary>
        /// This method is called for saving the details.
        /// </summary>
        /// <param name="olstAttendanceConfigDetails"></param>
        /// <returns></returns>
        public int Save(AttendanceAlertConfigDetails olstAttendanceAlertConfigDetails)
        {
            return moAttendanceAlertConfigDC.Save(olstAttendanceAlertConfigDetails);
        }

        /// <summary>
        /// This method is called to delete the details.
        /// </summary>
        /// <param name="olstAttendanceConfigDetails"></param>
        public void Delete(AttendanceAlertConfigDetails oAttendanceAlertConfigDetails)
        {
            moAttendanceAlertConfigDC.Delete(oAttendanceAlertConfigDetails);
        }


        /// <summary>
        /// This mehotd is used to get the data for selected user.
        /// </summary>
        /// <param name="olstTempConfigDetails"></param>
        /// <returns></returns>
        public AttendanceAlertConfigDetails GetDetails()
        {
            return moAttendanceAlertConfigDC.GetDetails();
        }

        /// <summary>
        /// This method is used to get all the attendnace details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<AttendanceAlertConfigDetails> GetAll()
        {
            return moAttendanceAlertConfigDC.GetAll();
        }

        /// <summary>
        /// This method is used to fill the details on poopup
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<AttendanceAlertDetails> GetMissingAttendanceDetailsForUser(int aiUserId,int aiStandardDivisionId)
        {
            return moAttendanceAlertConfigDC.GetMissingAttendanceDetailsForUser(aiUserId, aiStandardDivisionId);
        }

        /// <summary>
        /// This method is used to fill the details on poopup
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<AbsentStudentDetails> GetAbsentStudentDetailsForPopup(int aiUSerId, out bool abIsLinkVisibel)
        {
            return moAttendanceAlertConfigDC.GetAbsentStudentDetailsForPopup(aiUSerId, out abIsLinkVisibel);
        }

        ///// <summary>
        ///// This method is used to get the dates for selected count.
        ///// </summary>
        ///// <param name="aiSchoolId"></param>
        ///// <param name="aiAcademicYearId"></param>
        ///// <param name="aiStandardDivisionId"></param>
        ///// <returns></returns>
        public List<DateTime> GetMissingAttendanceDates(int aiStandardDivisionId,int aiUserId)
        {
            return moAttendanceAlertConfigDC.GetMissingAttendanceDates(aiStandardDivisionId, aiUserId);
        }

        ///// <summary>
        ///// This method is used to get the nonPermenant Teacher details whose joining date is gretter than 1 Year.
        ///// </summary>
        public List<NonPermanentTeacherDetails> GetNonPermanentTeacherDetails()
        {
            return moAttendanceAlertConfigDC.GetNonPermanentTeacherDetails();
        }
    }
}
