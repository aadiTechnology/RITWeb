using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using SchoolEntities;
using XseedReportEntities;

namespace BusinessLogic
{
    public class XseedProgressReportBL
    {
        XseedProgressReportDC moXseedProgressReportDC;

        public XseedProgressReportBL()
        {
            moXseedProgressReportDC = new XseedProgressReportDC();
        }

        public ExamResult ExamResult
        {
            get { return moXseedProgressReportDC.oExamResult; }
            set { moXseedProgressReportDC.oExamResult = value; }
        }

        public List<StandardwiseSubject> StandardwiseSubjects
        {
            get { return moXseedProgressReportDC.lstStandardwiseSubjects; }
            set { moXseedProgressReportDC.lstStandardwiseSubjects = value; }
        }

        public List<SubjectSectionConfigurationMaster> SubjectSections
        {
            get { return moXseedProgressReportDC.lstSubjectSections; }
            set { moXseedProgressReportDC.lstSubjectSections = value; }
        }

        public List<StudentsLearningOutcome> StudentsLearningOutcomes
        {
            get { return moXseedProgressReportDC.lstStudentsLearningOutcomes; }
            set { moXseedProgressReportDC.lstStudentsLearningOutcomes = value; }
        }
       

        public List<LearningOutcomesObservation> LearningOutcomesObservations
        {
            get { return moXseedProgressReportDC.lstLearningOutcomesObservations; }
            set { moXseedProgressReportDC.lstLearningOutcomesObservations = value; }
        }

        public List<NonXseedSubjectGrades> NonXseedSubjectGrades
        {
            get { return moXseedProgressReportDC.lstNonXseedSubjectGardes; }
            set { moXseedProgressReportDC.lstNonXseedSubjectGardes = value; }
        }

        public SchoolEntity SchoolEntity
        {
            get { return moXseedProgressReportDC.moSchoolEntity; }
            set { moXseedProgressReportDC.moSchoolEntity = value; }
        }

        public List<YearwiseStudentMaster> YearwiseStudentMaster
        {
            get { return moXseedProgressReportDC.lstYearwiseStudentMaster; }
            set { moXseedProgressReportDC.lstYearwiseStudentMaster = value; }
        }

        public List<GradeMaster> GradeMaster
        {
            get { return moXseedProgressReportDC.lstGradeMaster; }
            set { moXseedProgressReportDC.lstGradeMaster = value; }
        }

        public List<AssessmentMaster> AssessmentMaster
        {
            get { return moXseedProgressReportDC.lstAssessmentMaster; }
            set { moXseedProgressReportDC.lstAssessmentMaster = value; }
        }

        public List<ClassTeacherDetails> ClassTeacherDetails
        {
            get { return moXseedProgressReportDC.lstClassTeacherDetails; }
            set { moXseedProgressReportDC.lstClassTeacherDetails = value; }
        }

        public List<StudentAttendance> StudentAttendance
        {
            get { return moXseedProgressReportDC.lstStudentAttendance; }
            set { moXseedProgressReportDC.lstStudentAttendance = value; }
        }

        public string LearningOutcomeXML
        {
            get { return moXseedProgressReportDC.msLearningOutcomeXML; }
            set { moXseedProgressReportDC.msLearningOutcomeXML = value; }
        }

        public string XseedGradesXML
        {
            get { return moXseedProgressReportDC.msXseedGradesXML; }
            set { moXseedProgressReportDC.msXseedGradesXML = value; }
        }

        public bool AssessmentPublishStatus
        {
            get { return moXseedProgressReportDC.mbAssessmentPublishStatus; }
            set { moXseedProgressReportDC.mbAssessmentPublishStatus = value; }
        }

        public bool StudentWiseAssessmentPublishStatus
        {
            get { return moXseedProgressReportDC.mbStudentWiseAssessmentPublishStatus; }
            set { moXseedProgressReportDC.mbStudentWiseAssessmentPublishStatus = value; }
        }

        public string Remark
        {
            get { return moXseedProgressReportDC.msRemark; }
            set { moXseedProgressReportDC.msRemark = value; }
        }

        public string SubjectRemark
        {
            get { return moXseedProgressReportDC.msSubjectRemark; }
            set { moXseedProgressReportDC.msSubjectRemark = value; }
        }

        public List<XseedRemark> XseedRemarks
        {
            get { return moXseedProgressReportDC.lstRemarks; }
            set { moXseedProgressReportDC.lstRemarks = value; }
        }

        public List<SubjectRemark> SubjectRemarks
        {
            get { return moXseedProgressReportDC.lstSubjectRemarks; }
            set { moXseedProgressReportDC.lstSubjectRemarks = value; }
        }
    

        public void GetXseedProgressReport()
        {
            moXseedProgressReportDC.GetProgressReportDetails();
        }

        public void GetClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            moXseedProgressReportDC.GetClassTeachers(aiSchoolId, aiAcademicYearId);
        }

        public List<AssessmentMaster> GetPublishedAssesments(int aiSchoolId, int aiAcademicYearId, int aiStdDivId)
        {
            return moXseedProgressReportDC.GetPublishedAssesments(aiSchoolId, aiAcademicYearId, aiStdDivId);
        }

        public List<YearwiseStudentMaster> GetStudents(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId, int aiStudentId)
        {
          return moXseedProgressReportDC.GetStudents(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, aiStudentId);
        }

        public bool IsXseedApplicable(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiTeachersStandardDivisionId)
        {
            return moXseedProgressReportDC.IsXseedApplicable(aiSchoolId, aiAcademicYearId, aiStandardId, aiTeachersStandardDivisionId);
        }

        public void ManageStudentWiseAssessmentGrades(int aiSchoolId, int aiAcademicYearId, int aiStudentId, int aiStandardDivisionId, int aiAssessmentId, int aiUserId, string asMode)
        {
            moXseedProgressReportDC.ManageStudentWiseAssessmentGrades(aiSchoolId, aiAcademicYearId, aiStudentId, aiStandardDivisionId, aiAssessmentId, aiUserId, asMode);
        }
        
        public void PublishXseedResult( int aiSchoolId, int aiAcademicYearId , int aiStandardDivisionId, int aiAssessmentId, string asMode,int aiUserId)
        {
            moXseedProgressReportDC.PublishXseedResult(aiSchoolId, aiAcademicYearId, aiStandardDivisionId, aiAssessmentId, asMode,aiUserId);
        }
        public PublishStatus GetPublishStatus(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, int aiAssessmentId)
        {
            return moXseedProgressReportDC.GetPublishStatus(aiSchoolId, aiAcademicYearId, aiStdDivId, aiAssessmentId);
        }


        public bool IsXseedResultPublished(int miSchoolId, int miAcademicYearId, int miStdDivId, int miAssessmentId, int aiStudentId)
        {
            return moXseedProgressReportDC.IsXseedResultPublished(miSchoolId, miAcademicYearId, miStdDivId, miAssessmentId, aiStudentId);
        }
    }
}
