//------------------------------------------------------------------------------------------------------------------------------
// Class Name       :- AssignXseedGradesBL
// Purpose          :- This class is used to manage Edit,Submit grades of all selected subjects of the selected subject teachers.
// Date Of creation :- 6/01/2011
// Author Name      :- Shobha Patil.
//------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Data;
using System.Collections;
using DataCommunicator;
using System.Collections.Generic;
using XseedReportEntities;


namespace BusinessLogic
{
    public class AssignXseedGradesBL
    {
        #region "DATAMEMBERS"
        private AssignXseedGradesDC moAssignXseedGradesDC;

        public AssignXseedGradesBL()
        {
            moAssignXseedGradesDC = new AssignXseedGradesDC();
        }
        #endregion

        #region "PROPERTIES"

        public  GradeSubmitStatus GradeSubmitEntity
        {
            get
            {
                return moAssignXseedGradesDC.GradeSubmitEntity;
            }
            set
            {
                moAssignXseedGradesDC.GradeSubmitEntity = value;
            }
        }
        public XseedResultPublishStatus XseedResultPublishEntity
        {
            get
            {
                return moAssignXseedGradesDC.moXseedResultPublishStatus;
            }
            set
            {
                moAssignXseedGradesDC.moXseedResultPublishStatus = value;
            }
        }

        #endregion

        #region "PUBLIC METHODS"

        /// <summary>
        /// This method is used to fill the assessments combo boxes.
        /// </summary>
        public static List<AssessmentMaster> GetAssessments(int aiSchoolId, int aiAcademicYearId)
        {
            return AssignXseedGradesDC.GetAssessments(aiSchoolId,aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to get the all subject details with edit or submit grade status of the seleted teacher and assessment.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiAsseessmentId"></param>
        /// <returns></returns>
        public static List<XseedGradesStatus> GetTeacherSubjectDetails(int aiTeacherId, int aiSchoolId, int aiAcademicYearId,int aiAsseessmentId)
        {
            return AssignXseedGradesDC.GetTeacherSubjectDetails(aiTeacherId, aiSchoolId, aiAcademicYearId,aiAsseessmentId);
        }

        /// <summary>
        /// This method is used to submit assigned grades to class teacher.
        /// </summary>
        public void Submit()
        {
            moAssignXseedGradesDC.Submit();
        }

        /// <summary>
        /// This method is used to get the all subject details of the selected class teacher and assessment.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiSchoolId"></param>	
        /// <param name="aiAssessmentId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
		public List<XseedGradesStatus> GetClassTeacherSubjects(int aiStdDivId, int aiSchoolId, int aiAssessmentId, int aiAcademicYearId)
        {
			return moAssignXseedGradesDC.GetClassTeacherSubjects(aiStdDivId, aiSchoolId, aiAssessmentId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to publish the assigned grades to class teachers. 
        /// </summary>
        public void Publish()
        {
            moAssignXseedGradesDC.Publish();
        }

        /// <summary>
        /// This method is used to get the class teacher for which the assessments are assigned to fill the class teacher combobox.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
		public static List<ClassTeacherDetails> GetClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            return AssignXseedGradesDC.GetClassTeachers(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to to publish all the Xseed Result.
        /// </summary>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiAssessmentId"></param>
        /// <param name="aiAcademicYrID"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="asUnpublishReason"></param>
        /// <param name="aiUpdatedId"></param>
        public static void Unpublish(int aiStandardDivId, int aiAssessmentId, int aiAcademicYrID,
                                       int aiSchoolID, string asUnpublishReason, int aiUpdatedId)
        {
            AssignXseedGradesDC.Unpublish(aiStandardDivId, aiAssessmentId, aiAcademicYrID, 
                                                    aiSchoolID, asUnpublishReason, aiUpdatedId);
        }

        #endregion

        public static List<ClassTeacher> GetTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            return AssignXseedGradesDC.GetTeacher(aiSchoolId, aiAcademicYearId);
        }
    
    }
}
