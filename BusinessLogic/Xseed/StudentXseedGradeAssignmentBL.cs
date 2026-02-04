using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCommunicator;
using XseedReportEntities;

namespace BusinessLogic
{
    public class StudentXseedGradeAssignmentBL
    {
        StudentXseedGradeAssignmentDC moStudentXseedGradeAssignmentDC;
        
        #region Constructor

        public StudentXseedGradeAssignmentBL()
        {
            moStudentXseedGradeAssignmentDC = new StudentXseedGradeAssignmentDC();
        }
        #endregion

        #region Properties

        public string Obsevation
        {
            get { return moStudentXseedGradeAssignmentDC.Obsevation; }
            set { moStudentXseedGradeAssignmentDC.Obsevation = value; }
        }

        public LearningOutcomesObservation LearningOutcomesObservation
        {
            get { return moStudentXseedGradeAssignmentDC.LearningOutcomesObservation; }
            set { moStudentXseedGradeAssignmentDC.LearningOutcomesObservation = value; }
        }
       
        public GradeSubmitStatus GradeSubmitStatus
        {
            get { return moStudentXseedGradeAssignmentDC.GradeSubmitStatus; }
            set { moStudentXseedGradeAssignmentDC.GradeSubmitStatus = value; }
        }
        public LearningOutcomesGrade LearningOutcomesGradeDetails
        {
            get { return moStudentXseedGradeAssignmentDC.LearningOutcomesGradeDetails; }
            set { moStudentXseedGradeAssignmentDC.LearningOutcomesGradeDetails = value; }
        }
        public LearningOutcomeConfigMaster LearningOutcomeConfig
        {
            get { return moStudentXseedGradeAssignmentDC.LearningOutcomeConfig; }
            set { moStudentXseedGradeAssignmentDC.LearningOutcomeConfig = value; }
        }

        public List<YearwiseStudentMaster> YearwiseStudentsList
        {
            get { return moStudentXseedGradeAssignmentDC.lstYearwiseStudents; }
            set { moStudentXseedGradeAssignmentDC.lstYearwiseStudents = value; }
        }

        public List<SubjectSectionConfigurationMaster> SubjectSectionConfigurationDetailList
        {
            get { return moStudentXseedGradeAssignmentDC.lstSubjectSectionConfigurationDetail; }
            set { moStudentXseedGradeAssignmentDC.lstSubjectSectionConfigurationDetail = value; }
        }

        public List<GradeMaster> GradeDetailsList
        {
            get { return moStudentXseedGradeAssignmentDC.lstGradeDetails; }
            set { moStudentXseedGradeAssignmentDC.lstGradeDetails = value; }
        }

        public AssessmentMaster AssessmentDetails
        {
            get { return moStudentXseedGradeAssignmentDC.AssessmentDetails; }
            set { moStudentXseedGradeAssignmentDC.AssessmentDetails = value; }
        }

        public string ClassName
        {
            get { return moStudentXseedGradeAssignmentDC.ClassName; }
            set { moStudentXseedGradeAssignmentDC.ClassName = value; }
        }

        public string SubjectName
        {
            get { return moStudentXseedGradeAssignmentDC.SubjectName; }
            set { moStudentXseedGradeAssignmentDC.SubjectName = value; }
        }

        public bool IsExamPublished
        {
            get { return this.moStudentXseedGradeAssignmentDC.IsExamPublished; }
        }

        #endregion

        #region Methods

        public void GetStudentsForStdDiv()
        {
            moStudentXseedGradeAssignmentDC.GetStudentsForStdDiv();
        }
        public List<LearningOutcomeConfigMaster> GetLearningOutcomesForStdDiv(int aiSubjectId)
        {
            return moStudentXseedGradeAssignmentDC.GetLearningOutcomesForStdDiv(aiSubjectId);
        }

        /// <summary>
        /// This method is used to get all student list to assign grades and observations.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiAssessmwntId"></param>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public static List<StudentXseedGradeDetails> GetAllStudents(int aiSchoolId, int aiAcademicYearId, int aiStandardDivId,int aiAssessmwntId, int aiSubjectId)
        {
            return StudentXseedGradeAssignmentDC.GetAllStudents(aiSchoolId, aiAcademicYearId, aiStandardDivId, aiAssessmwntId,aiSubjectId);
        }
        public void Save(int aiSubjectId)
        {
            moStudentXseedGradeAssignmentDC.Save(aiSubjectId);
        }

        /// <summary>
        /// This method is used to save, update and delete assigned grade and observation details.
        /// </summary>
        /// <param name="asXseedGradesXML"></param>
        public void Save(string asXseedGradesXML)
        {
            moStudentXseedGradeAssignmentDC.Save(asXseedGradesXML);
        }
        /// <summary>
        /// This method is used to fill student and subject section combobox.
        /// </summary>
        public void FillStudentAndSubjectComboboxes(System.Web.UI.WebControls.DropDownList cmbStudent, System.Web.UI.WebControls.DropDownList cmbSubjectSections)
        {
            cmbStudent.DataSource = YearwiseStudentsList;
            cmbStudent.DataTextField = "StudentName";
            cmbStudent.DataValueField = "YearwiseStudentId";
            cmbStudent.DataBind();
            cmbStudent.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Value = "0", Text = "-- Select --" });

            cmbSubjectSections.DataSource = SubjectSectionConfigurationDetailList;
            cmbSubjectSections.DataTextField = "SubjectSectionName";
            cmbSubjectSections.DataValueField = "SubjectSectionConfigurationId";
            cmbSubjectSections.DataBind();
            cmbSubjectSections.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Value = "0", Text = "-- Select --" });
        }

        #endregion
    }
}
