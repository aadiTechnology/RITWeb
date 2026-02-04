// File Name    : ExamTypesConfigurationDC.cs
// Created By   : Yogesh
// Crested Date : 23-May-2015 

namespace BusinessLogic
{
    using System.Collections.Generic;
    using DataCommunicator;
    using SchoolEntities.Admin;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExamTypesConfigurationBL
    {
        #region Data Member(s)
        public ExamTypesConfigurationDC oExamTypesConfigurationDC;
        #endregion

        #region Constructor(s)
        public ExamTypesConfigurationBL(int aiSchoolId, int aiAcademicYearId)
        {
            oExamTypesConfigurationDC = new ExamTypesConfigurationDC(aiSchoolId, aiAcademicYearId);
        }
        public ExamTypesConfigurationBL()
        {
            oExamTypesConfigurationDC = new ExamTypesConfigurationDC();
        }
        public ExamTypesConfigurationBL(int miUserId)
        {
            oExamTypesConfigurationDC = new ExamTypesConfigurationDC(miUserId);
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// This method is used to get yearwise subjects.
        /// </summary>
        /// <returns></returns>
        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects(int aiStdDivId = 0)
        {
            return oExamTypesConfigurationDC.GetAllYearwiseSubjects(aiStdDivId);
        }

        /// <summary>
        /// This method is used to get yearwise subjects.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<YearWiseSubjectsDetails> GetAllYearwiseSubjects(int aiSchoolId, int aiAcademicYearId)
        {
            return oExamTypesConfigurationDC.GetAllYearwiseSubjects(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This is used to get list for exam type configuration status.
        /// </summary>
        /// <param name="aiSubjectId"></param>
        /// <returns></returns>
        public List<SubjectwiseExamTypeDetails> GetAll(int aiSubjectId)
        {
            return oExamTypesConfigurationDC.GetAll(aiSubjectId);
        }

        /// <summary>
        /// This is used to save Exam Type configuration status.
        /// </summary>
        /// <param name="asSubjectId"></param>
        /// <param name="asTestTypeIdsForInsert"></param>
        /// <param name="asTestTypeIdsForDelete"></param>
        public void Save(int asSubjectId, string asTestTypeIdsForInsert, string asTestTypeIdsForDelete)
        {
            oExamTypesConfigurationDC.Save(asSubjectId, asTestTypeIdsForInsert, asTestTypeIdsForDelete);
        }
        /// <summary>
        /// This method is used to get selected test type
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public SubjectwiseExamTypeDetails GetTestType(int aiId)
        {
            return oExamTypesConfigurationDC.GetTestType(aiId);
        }
        /// <summary>
        /// This method is used to get all test type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SubjectwiseExamTypeDetails> GetAllTestType(int id)
        {
            return oExamTypesConfigurationDC.GetAllTestType(id);
        }
        /// <summary>
        /// This method is used save test type
        /// </summary>
        /// <param name="aiTestTypeid"></param>
        /// <param name="asTestTypeName"></param>
        /// <param name="aiExamTypeStatus"></param>
        /// <param name="aisSortOrder"></param>
        public void SaveTestType(int aiTestTypeid, string asTestTypeName, bool abExamTypeStatus, int aisSortOrder)
        {
            oExamTypesConfigurationDC.SaveTestType(aiTestTypeid, asTestTypeName, abExamTypeStatus, aisSortOrder);
        }
        /// <summary>
        /// This method is used to delete test type
        /// </summary>
        /// <param name="aiTestTypeId"></param>
        public void Delete(int aiTestTypeId)
        {
            oExamTypesConfigurationDC.Delete(aiTestTypeId);
        }
        /// <summary>
        /// This method is used to update test type
        /// </summary>
        /// <param name="aiTestTypeId"></param>
        /// <param name="asTestTypeName"></param>
        /// <param name="aiExamTypeStatus"></param>
        /// <param name="aiSortOrder"></param>
        //public void UpdateTestType(int aiTestTypeId, string asTestTypeName, bool abExamTypeStatus, int aiSortOrder)
        //{
        //    oExamTypesConfigurationDC.UpdateTestType(aiTestTypeId, asTestTypeName, abExamTypeStatus, aiSortOrder);

        //}
        #endregion
    }
}