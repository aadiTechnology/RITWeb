// File Name : RetirementNoticeBL.cs
// Creator : Sunny
// Created Date : 12-June-2013
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using DataCommunicator;
using PayrollReportingUserEntities;
using System.Data;
namespace BusinessLogic
{
	/// <summary>
	///  This class is used for processing business logic and communicate with data access layer.
	/// </summary>
	public class ReportingUserConfigurationBL
	{
        ReportingUserConfigDC moReportingUserConfigDC;

        /// <summary>
        /// this is a default constructor.
        /// </summary>
        public ReportingUserConfigurationBL()
        {
            moReportingUserConfigDC = new ReportingUserConfigDC();
        }

        /// <summary>
        /// This is a constructor to initialize the members.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        public ReportingUserConfigurationBL(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            moReportingUserConfigDC = new ReportingUserConfigDC(aiSchoolId, aiAcademicYearId, aiUserId);
        }

        /// <summary>
        /// This is a constructor to initialize the members.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiConfigId"></param>
        public ReportingUserConfigurationBL(int aiSchoolId, int aiAcademicYearId)
        {
            moReportingUserConfigDC = new ReportingUserConfigDC(aiSchoolId, aiAcademicYearId);
        }


        /// <summary>
        /// This method is called for saving the details.
        /// </summary>
        /// <param name="olstAttendanceConfigDetails"></param>
        /// <returns></returns>
        public void Save(ReportingUserConfiguration oReportingUserConfiguration, int aiConfigId)
        {
             moReportingUserConfigDC.Save(oReportingUserConfiguration, aiConfigId);
        }

        /// <summary>
        /// This method is called to delete the details.
        /// </summary>
        /// <param name="olstAttendanceConfigDetails"></param>
        public void Delete(int aiConfigId, int aiReportingTypeId)
        {
            moReportingUserConfigDC.Delete(aiConfigId, aiReportingTypeId);
        }


        /// <summary>
        /// This mehotd is used to get the data for selected user.
        /// </summary>
        /// <param name="olstTempConfigDetails"></param>
        /// <returns></returns>
        public ReportingUserConfiguration Get(int aiConfigId)
        {
            return moReportingUserConfigDC.Get(aiConfigId);
        }

        /// <summary>
        /// This method is used to get parameters details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<ReportingUserConfiguration> GetAll()
        {
            return moReportingUserConfigDC.GetAll();
        }

        /// <summary>
        /// This method is used to get all reporting parameters.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>        
        public List<ReportingParameter> GetAllReportingParameters()
        {
            return moReportingUserConfigDC.GetAllReportingParameters();
        }      

       
    }
}
