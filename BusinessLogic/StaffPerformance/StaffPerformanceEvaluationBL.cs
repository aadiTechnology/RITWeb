/*
 *  File Name - StaffPerformanceEvaluationBL.cs
 *  Created By - Sachin
 *  Created Date - 30 Sept 2013
 *  Description - This class is used to communicate with data access layer for managing performance evaluation details.
 */

using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;
using StaffPerformanceEntity;

namespace BusinessLogic
{
    public class StaffPerformanceEvaluationBL
    {
        #region Data member(s)
        
        private StaffPerformanceEvaluationDC moStaffPerformanceEvaluationDC; 

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Parameterized constructor(s)
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUpdatedById"></param>
        public StaffPerformanceEvaluationBL(int aiSchoolId, int aiUpdatedById, int aiSelectedUserId, int aiAcademicYearId)
        {
            this.moStaffPerformanceEvaluationDC = new StaffPerformanceEvaluationDC(aiSchoolId, aiUpdatedById, aiSelectedUserId, aiAcademicYearId);
        } 

        #endregion

        #region Property(s)
      
        public SchoolEntity SchoolEntity
        {
            get { return this.moStaffPerformanceEvaluationDC.SchoolEntity; }
        }

        public List<ReportingStaff> ReportingStaffs
        {
            get { return this.moStaffPerformanceEvaluationDC.ReportingStaffs; }
        }

        public List<PerformanceGrade> PerformanceGrades
        {
            get { return this.moStaffPerformanceEvaluationDC.PerformanceGrades; }
        }

        public List<PerformanceParameter> PerformanceParameters
        {
            get { return this.moStaffPerformanceEvaluationDC.PerformanceParameters; }
        }

        public List<PerformanceSkill> PerformanceSkills
        {
            get { return this.moStaffPerformanceEvaluationDC.PerformanceSkills; }
        }

        public List<StaffPerformanceStatus> StaffPerformanceStatus
        {
            get { return this.moStaffPerformanceEvaluationDC.StaffPerformanceStatus; }
        }

        public ReportingStaff UserDetails
        {
            get { return this.moStaffPerformanceEvaluationDC.UserDetails; }
        }

        public ButtonState ButtonState
        {
            get { return this.moStaffPerformanceEvaluationDC.ButtonState; }
        }

        #endregion

        #region Public Method(s)
     
        /// <summary>
        /// This method is used to return performance observations for given staff.
        /// </summary>
        /// <param name="aiUserid"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public List<StaffPerformanceObservation> GetAll(int aiReportingUserId, int aiYear)
        {
            return this.moStaffPerformanceEvaluationDC.GetAll(aiReportingUserId, aiYear);
        }

        /// <summary>
        /// This method is used to save performance observations of given staff.
        /// </summary>
        /// <param name="aiUserid"></param>
        /// <param name="aiReportingUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="asXml"></param>
        public void Save(int aiReportingUserId, int aiYear, string asXml,string asclass,string assubject)
        {
            this.moStaffPerformanceEvaluationDC.Save(aiReportingUserId, aiYear, asXml,asclass,assubject);
        }
        /// <summary>
        /// This method is used to publish performance observations of given staff.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        /// <param name="abIsPublish"></param>
        public void Publish(int aiYear, bool abIsPublish, string asEffectiveDate, string asLastIncrementDate)
        {
            this.moStaffPerformanceEvaluationDC.Publish(aiYear, abIsPublish, asEffectiveDate, asLastIncrementDate);
        }

        /// <summary>
        /// This method is used to submit performance observations of given staff.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiYear"></param>
        public void Submit(int aiYear, bool abIsSubmitAction)
        {
            this.moStaffPerformanceEvaluationDC.Submit(aiYear, abIsSubmitAction);
        }

        /// <summary>
        /// This methos is used to Get Attachment count
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiDocumentTypeId"></param>
        /// <returns></returns>
        public int GetAttachmentCount(int aiAcademicYearId, int aiDocumentTypeId)
        {
            return this.moStaffPerformanceEvaluationDC.GetAttachmentCount( aiAcademicYearId, aiDocumentTypeId);
        }
        #endregion

        public void RejectSubmittion(int aiUserId, string asReason, int aiReportingUserId, int aiAcademicYearId, int aiYear)
        {
            this.moStaffPerformanceEvaluationDC.RejectSubmittion(aiUserId, asReason, aiReportingUserId, aiAcademicYearId, aiYear);
        }
    }
}
