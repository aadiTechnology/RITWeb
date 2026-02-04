using System;
using System.Data;
using System.Collections;
using Utility;
using DataCommunicator;
using StandardWiseExamConfigurationEntities;

namespace BusinessLogic
{

    public class SubjectTestConfigurationBL
    {
        #region DataMembers and properties

        #region Data members

        public SubjectTestConfigurationBL()
        {
            moSubjectTestConfigurationDC = new SubjectTestConfigurationDC();
        }

        public SubjectTestConfigurationBL(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
       {
           moSubjectTestConfigurationDC = new SubjectTestConfigurationDC(aiSchoolId, aiAcademicYearId, aiUpdatedById);
       }

        private SubjectTestConfigurationDC.SubjectTestConfigurationMasterStruct moSubjectTestConfigurationMasterStruct;
        private readonly SubjectTestConfigurationDC moSubjectTestConfigurationDC = new SubjectTestConfigurationDC();

        #endregion

        #region Properties

        public int TestWiseSubjectMarksId
        {

            get { return moSubjectTestConfigurationMasterStruct.miTestWiseSubjectMarksId; }
            set { moSubjectTestConfigurationMasterStruct.miTestWiseSubjectMarksId = value; }
        }

        public int SchoolId
        {

            get { return moSubjectTestConfigurationMasterStruct.miSchoolId; }
            set { moSubjectTestConfigurationMasterStruct.miSchoolId = value; }
        }

        public int AcademicYearId
        {

            get { return moSubjectTestConfigurationMasterStruct.miAcademicYearId; }
            set { moSubjectTestConfigurationMasterStruct.miAcademicYearId = value; }
        }

        public int StandardDivisionId
        {

            get { return moSubjectTestConfigurationMasterStruct.miStandardDivisionId; }
            set { moSubjectTestConfigurationMasterStruct.miStandardDivisionId = value; }
        }

        public int SubjectId
        {

            get { return moSubjectTestConfigurationMasterStruct.miSubjectId; }
            set { moSubjectTestConfigurationMasterStruct.miSubjectId = value; }
        }

        public int SchoolWiseTestId
        {

            get { return moSubjectTestConfigurationMasterStruct.miSchoolWiseTestId; }
            set { moSubjectTestConfigurationMasterStruct.miSchoolWiseTestId = value; }
        }

        public string GradeOrMarks
        {

            get { return moSubjectTestConfigurationMasterStruct.msGradeOrMarks; }
            set { moSubjectTestConfigurationMasterStruct.msGradeOrMarks = value; }
        }

        public int SubjectTotalMarks
        {
            get { return moSubjectTestConfigurationMasterStruct.miSubjectTotalMarks; }
            set { moSubjectTestConfigurationMasterStruct.miSubjectTotalMarks = value; }
        }

        public decimal PassingTotalMarks
        {

            get { return moSubjectTestConfigurationMasterStruct.mdPassingTotalMarks; }
            set { moSubjectTestConfigurationMasterStruct.mdPassingTotalMarks = value; }
        }
        
		public int PassingGradeId
        {

            get { return moSubjectTestConfigurationMasterStruct.miPassingGradeId; }
            set { moSubjectTestConfigurationMasterStruct.miPassingGradeId = value; }
        }

		public int OutOfMarks
		{
			get { return moSubjectTestConfigurationMasterStruct.miOutOfMarks; }
			set { moSubjectTestConfigurationMasterStruct.miOutOfMarks = value; }
		}

		public bool IsExamStatusApplicable
		{
			get { return moSubjectTestConfigurationMasterStruct.mbIsExamStatusApplicable; }
			set { moSubjectTestConfigurationMasterStruct.mbIsExamStatusApplicable = value; }
		}

        public string IsDeleted
        {

            get { return moSubjectTestConfigurationMasterStruct.msIsDeleted; }
            set { moSubjectTestConfigurationMasterStruct.msIsDeleted = value; }
        }
        public string ResultConsideration
        {

            get { return moSubjectTestConfigurationMasterStruct.msResultConsideration; }
            set { moSubjectTestConfigurationMasterStruct.msResultConsideration = value; }
        }

        public string TotalConsideration
        {

            get { return moSubjectTestConfigurationMasterStruct.msTotalConsideration; }
            set { moSubjectTestConfigurationMasterStruct.msTotalConsideration = value; }
        }

        
        public double RsltFactor
        {
            get { return moSubjectTestConfigurationMasterStruct.mdRsltFactor; }
            set { moSubjectTestConfigurationMasterStruct.mdRsltFactor = value; }
        }

    	public bool DisplayGrade
    	{
            get { return moSubjectTestConfigurationMasterStruct.mbDisplayGrade; }
            set { moSubjectTestConfigurationMasterStruct.mbDisplayGrade = value; }
    	}

        public DateTime InsertDate
        {

            get { return moSubjectTestConfigurationMasterStruct.mdtInsertDate; }
            set { moSubjectTestConfigurationMasterStruct.mdtInsertDate = value; }
        }

        public int InsertedByid
        {

            get { return moSubjectTestConfigurationMasterStruct.miInsertedByid; }
            set { moSubjectTestConfigurationMasterStruct.miInsertedByid = value; }
        }

        public DateTime UpdateDate
        {

            get { return moSubjectTestConfigurationMasterStruct.mdtUpdateDate; }
            set { moSubjectTestConfigurationMasterStruct.mdtUpdateDate = value; }
        }

        public int UpdatedById
        {

            get { return moSubjectTestConfigurationMasterStruct.miUpdatedById; }
            set { moSubjectTestConfigurationMasterStruct.miUpdatedById = value; }
        }

        public bool AllowDecimal
        {

            get { return moSubjectTestConfigurationMasterStruct.mbAllowDecimal; }
            set { moSubjectTestConfigurationMasterStruct.mbAllowDecimal = value; }
        }

        #endregion
        
		#endregion

        #region Public Methods

        public string CheckDependenciesForExamConfiguration(string asExamName)
        {
            string sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.ExamConfiguration), moSubjectTestConfigurationMasterStruct.miTestWiseSubjectMarksId, asExamName, moSubjectTestConfigurationMasterStruct.miAcademicYearId);
            return sReturn;
        }

        public string CheckDependenciesForExamConfiguration(Hashtable aHtTests)
        {
            ReferenceBL oRefBL = new ReferenceBL();
            string sReturnMsg = oRefBL.CheckDependencies(Constants.ReferenceId.ExamConfiguration, aHtTests, moSubjectTestConfigurationMasterStruct.miAcademicYearId);
            if (!sReturnMsg.Equals(""))
            {
                sReturnMsg = "Exam configuration was not copied.<BR>" + sReturnMsg; 
            }
            return sReturnMsg;

        }
        public DataTable DeleteAllExams(int aiStandardDivisionId, int aiSubjectId, int aiUserId, int aiAcademicYearId, int aiSchoolId)
        {
            return moSubjectTestConfigurationDC.DeleteAllExams(aiStandardDivisionId, aiSubjectId, aiUserId, aiAcademicYearId, aiSchoolId);
        }
        public void AddSubjectTestConfiguration(string asXmlString)
        {

            moSubjectTestConfigurationDC.SubjectTestConfigurationMasterStructDetails = moSubjectTestConfigurationMasterStruct;
            moSubjectTestConfigurationDC.AddSubjectTestConfiguration(asXmlString);
        }
        public void Update(string asXmlString, string asExamName)
        {
            string sReferenceMessage = CheckDependenciesForExamConfUpdate(asExamName);
            if (sReferenceMessage.Equals(""))
            {
                moSubjectTestConfigurationDC.SubjectTestConfigurationMasterStructDetails = moSubjectTestConfigurationMasterStruct;
                moSubjectTestConfigurationDC.Update(asXmlString);
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sReferenceMessage);
            }
        }

        private string CheckDependenciesForExamConfUpdate(string asExamName)
        {
            moSubjectTestConfigurationDC.SubjectTestConfigurationMasterStructDetails = moSubjectTestConfigurationMasterStruct;
            return moSubjectTestConfigurationDC.CheckDependenciesForExamConfUpdate(moSubjectTestConfigurationMasterStruct.miTestWiseSubjectMarksId, asExamName);            
        }

       
        public void DeleteSubjectTestConfiguration(string asExamName)
        {
            string sReferenceMessage = CheckDependenciesForExamConfiguration(asExamName);
            if (sReferenceMessage.Equals(""))
            {
                moSubjectTestConfigurationDC.SubjectTestConfigurationMasterStructDetails = moSubjectTestConfigurationMasterStruct;
                moSubjectTestConfigurationDC.DeleteSubjectTestConfiguration();
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sReferenceMessage);
            }
        }
        public void DeleteTestExamMarkDetails(bool abDeleteStudentWiseSavedMarks)
        {
            moSubjectTestConfigurationDC.SubjectTestConfigurationMasterStructDetails = moSubjectTestConfigurationMasterStruct;
            moSubjectTestConfigurationDC.DeleteTestExamMarkDetails(abDeleteStudentWiseSavedMarks);
        }
        public DataSet CopyTestConfiguration(int aiStandardDivisionId, int aiSubjectId, string asTestConfiguration, int aiUserId, int aiAcademicYearId, Hashtable aohashTests,string ids)
        {
            //string sReferenceMessage = CheckDependenciesForExamConfiguration(aohashTests);
            string sReferenceMessage = string.Empty;
            if (sReferenceMessage.Equals(""))
            {
                moSubjectTestConfigurationDC.SubjectTestConfigurationMasterStructDetails = moSubjectTestConfigurationMasterStruct;
                return moSubjectTestConfigurationDC.CopyTestConfiguration(aiStandardDivisionId, aiSubjectId, asTestConfiguration, aiUserId, aiAcademicYearId,ids);
            }
            throw new Exceptions.ReferenceExceptions(sReferenceMessage);
        }

        public void UpdateResultFactor()
        {
            moSubjectTestConfigurationDC.SubjectTestConfigurationMasterStructDetails = moSubjectTestConfigurationMasterStruct;
            moSubjectTestConfigurationDC.UpdateResultFactor();
        }

        public DataTable GetFillStandardWiseSubjects(int aiStandardId)
        {
            return moSubjectTestConfigurationDC.GetFillStandardWiseSubjects(aiStandardId);
        }

        #endregion
    }

    public class SubjectTestConfigurationCollectionBL
    {
        #region datamembers

        readonly SubjectTestConfigurationCollectionDC moSubjectTestConfigurationCollectionDC;
        #endregion

        #region constructor

        public SubjectTestConfigurationCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
           moSubjectTestConfigurationCollectionDC = new SubjectTestConfigurationCollectionDC(aiSchoolId, aiAcademicYearId);
        }
        #endregion
      
        public StandardGradeConfiguration CheckPreConditioOfGrades(int aiStandardDivisionId)
        {
            return moSubjectTestConfigurationCollectionDC.CheckPreConditioOfGrades(aiStandardDivisionId);
        }
        public DataTable RetriveAllTestConfiguration(int aiStandardDivisionId, int aiSubjectId)
        {
            return moSubjectTestConfigurationCollectionDC.RetriveAllTestConfiguration(aiStandardDivisionId, aiSubjectId);
        }
        public DataTable FetchTestsConfigurationForTeacher(int aiTeacherId, int aiTestId, string asAllowPartialSubmit)
        {
            return moSubjectTestConfigurationCollectionDC.FetchTestsConfigurationForTeacher(aiTeacherId, aiTestId, asAllowPartialSubmit);
        }

        public DataTable FetchTestsConfigurationForMySubjects(int aiTeacherId, int aiTestId, string asAllowPartialSubmit, int aiStandardDivisionId)
        {
            return moSubjectTestConfigurationCollectionDC.FetchTestsConfigurationForMySubjects(aiTeacherId, aiTestId, asAllowPartialSubmit, aiStandardDivisionId);
        }

        public DataTable FetchTestsConfigurationForMyClass(int aiTeacherId, int aiTestId, string asAllowPartialSubmit, int aiStandardDivisionId)
        {
            return moSubjectTestConfigurationCollectionDC.FetchTestsConfigurationForMyClass(aiTeacherId, aiTestId, asAllowPartialSubmit, aiStandardDivisionId);
        }

        public DataSet GetClassSubjectTestsAssociation( int aiStandardId,int aiSubjectId)
        {
            return moSubjectTestConfigurationCollectionDC.GetClassSubjectTestsAssociation(aiStandardId ,aiSubjectId );
        }


        public DataTable GetSubjectTeachers(int aiTeacherId, int aiTestId, bool abIsClassTeacher, int aiStdDivId)
        {
            return moSubjectTestConfigurationCollectionDC.GetSubjectTeachers(aiTeacherId, aiTestId, abIsClassTeacher, aiStdDivId);
        }

        public void PublishObservationTest(int aiTestId, int aiStandardDivId, int aiInsertedById, bool abPublish)
        {
            moSubjectTestConfigurationCollectionDC.PublishObservationTest(aiTestId, aiStandardDivId, aiInsertedById, abPublish);
        }

        public DataTable GetAllClasses(int aiTeacherId)
        {
           return moSubjectTestConfigurationCollectionDC.GetAllClasses(aiTeacherId);
        }
    }


}
