/*File Name - ObservationDetailsBL.cs
 * Created By - Sachin
 * Created Date - 18-Sept-2015
 * Description- This class is used to manage observation details.
 */
using System.Collections.Generic;
using DataCommunicator;
using SchoolEntities;

namespace BusinessLogic
{
    public class ObservationDetailsBL
    {
        #region Data Member(s)
        
        private ObservationDetailsDC moObservationDetailsDC; 

        #endregion

        #region Constructor(s)

        public ObservationDetailsBL()
        {
            this.moObservationDetailsDC = new ObservationDetailsDC();
        }

        public ObservationDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.moObservationDetailsDC = new ObservationDetailsDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
        } 

        #endregion

        #region Property(s)

        public List<ObservationSkill> Skills
        {
            get { return this.moObservationDetailsDC.Skills; }
        }

        public List<ObservationGrade> Grades
        {
            get { return this.moObservationDetailsDC.Grades; }
        }
        public List<ObservationRemarks> Remarks
        {
            get { return this.moObservationDetailsDC.Remarks; }
        }
        public List<ObservationParameter> Parameters
        {
            get { return this.moObservationDetailsDC.Parameters; }
        }

        public List<ObservationDetails> Observations
        {
            get { return this.moObservationDetailsDC.Observations; }
        }

        public string ClassName
        {
            get { return this.moObservationDetailsDC.ClassName; }
        }

        public string TestName
        {
            get { return this.moObservationDetailsDC.TestName; }
        }

        public string SubjectName
        {
            get { return this.moObservationDetailsDC.SubjectName; }
        }

        public bool IsSubmitted
        {
            get { return this.moObservationDetailsDC.IsSubmitted; }
        }

        public bool IsPublished
        {
            get { return moObservationDetailsDC.IsPublished; }
        }

        #endregion


        #region Public Method(s)

        /// <summary>
        /// This method is used to return observation details.
        /// </summary>
        /// <param name="aiTestId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public List<StudentBasicDetails> GetObservationDetails(int aiTestId, int aiStdDivId, int aiSubjectId, bool abIsSummaryMode)
        {
            return this.moObservationDetailsDC.GetObservationDetails(aiTestId, aiStdDivId, aiSubjectId, abIsSummaryMode);
        }

        /// <summary>
        /// This method is used to save observation details.
        /// </summary>
        /// <param name="aiTestId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="asObservationXml"></param>
        public void Save(int aiTestId, int aiSubjectId, int aiStdDivId, string asObservationXml, bool abIsSummaryMode)
        {
            moObservationDetailsDC.Save(aiTestId, aiSubjectId, aiStdDivId, asObservationXml, abIsSummaryMode);
        }

        /// <summary>
        /// This method is used to submit observation details.
        /// </summary>
        /// <param name="aiTestId"></param>
        /// <param name="aiSubjectId"></param>
        /// <param name="aiStdDivId"></param>
        public void Submit(int aiTestId, int aiSubjectId, int aiStdDivId, int aiIsSubmitted, bool abIsSummaryMode)
        {
            moObservationDetailsDC.Submit(aiTestId, aiSubjectId, aiStdDivId, aiIsSubmitted, abIsSummaryMode);
        } 

        #endregion
    }
}
