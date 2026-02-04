using System;
using System.Data;
using DataCommunicator;

namespace BusinessLogic
{

    public class SubjectTestTypeConfigurationBL
    {


        #region DataMembers and properties

        #region Data members

        private SubjectTestTypeConfigurationDC.SubjectTestTypeConfigurationStruct moSubjectTestTypeConfigurationStruct;
        private SubjectTestTypeConfigurationDC moSubjectTestTypeConfigurationDC = new SubjectTestTypeConfigurationDC();

        #endregion
        #region Properties



        public int TestWiseSubjectMarksDetailId
        {

            get { return moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksDetailId; }
            set { moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksDetailId = value; }
        }

        public int TestWiseSubjectMarksId
        {

            get { return moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksId; }
            set { moSubjectTestTypeConfigurationStruct.miTestWiseSubjectMarksId = value; }
        }

        public int TestTypeId
        {

            get { return moSubjectTestTypeConfigurationStruct.miTestTypeId; }
            set { moSubjectTestTypeConfigurationStruct.miTestTypeId = value; }
        }

        public int TestTypeTotalMarks
        {

            get { return moSubjectTestTypeConfigurationStruct.miTestTypeTotalMarks; }
            set { moSubjectTestTypeConfigurationStruct.miTestTypeTotalMarks = value; }
        }

        public int TestTypePassingMarks
        {

            get { return moSubjectTestTypeConfigurationStruct.miTestTypePassingMarks; }
            set { moSubjectTestTypeConfigurationStruct.miTestTypePassingMarks = value; }
        }

        public string IsDeleted
        {

            get { return moSubjectTestTypeConfigurationStruct.msIsDeleted; }
            set { moSubjectTestTypeConfigurationStruct.msIsDeleted = value; }
        }

        public DateTime InsertDate
        {

            get { return moSubjectTestTypeConfigurationStruct.mdtInsertDate; }
            set { moSubjectTestTypeConfigurationStruct.mdtInsertDate = value; }
        }

        public int InsertedByid
        {

            get { return moSubjectTestTypeConfigurationStruct.miInsertedByid; }
            set { moSubjectTestTypeConfigurationStruct.miInsertedByid = value; }
        }

        public DateTime UpdateDate
        {

            get { return moSubjectTestTypeConfigurationStruct.mdtUpdateDate; }
            set { moSubjectTestTypeConfigurationStruct.mdtUpdateDate = value; }
        }

        public int UpdatedById
        {

            get { return moSubjectTestTypeConfigurationStruct.miUpdatedById; }
            set { moSubjectTestTypeConfigurationStruct.miUpdatedById = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SubjectTestTypeConfigurationBL()
        {
        }
        //public SubjectTestTypeConfigurationBL(int aiId)
        //{

        //    SubjectTestTypeConfigurationDC SubjectTestTypeConfigurationDC = new SubjectTestTypeConfigurationDC(aiId);

        //}
        #endregion

        #region Public Methods
        public static DataTable FetchAllTestSubjectMarksDetailsDataFromDatabase(int aiTestWiseSubjectMarksId, int aiSubjectId)
        {

            return SubjectTestTypeConfigurationDC.FetchAllTestSubjectMarksDetailsDataFromDatabase(aiTestWiseSubjectMarksId, aiSubjectId);
        }


        #endregion

    }
    public class SubjectTestTypeConfigurationCollectionBL
    {
        public static DataSet GetAllTestTypesForStandardDivisionSubjectTest(int aiStandardDivisionId,
                                                                            int aiSubjectId,
                                                                            int aiTestId,
                                                                            int aiSchoolId,
                                                                            int aiAcademicYrId,
                                                                         string asShowTotalAsPerOutOfMarks)
        {
            return SubjectTestTypeConfigurationCollectionDC.GetAllTestTypesForStandardDivisionSubjectTest(aiStandardDivisionId, aiSubjectId, aiTestId, aiSchoolId, aiAcademicYrId, asShowTotalAsPerOutOfMarks);
        }

        public static DataSet GetAllTestsResultDetails(int aiStandardDivisionId,
                                                                            int aiSchoolId,
                                                                            int aiAcademicYrId)
        {
            return SubjectTestTypeConfigurationCollectionDC.GetAllTestsResultDetails(aiStandardDivisionId, aiSchoolId, aiAcademicYrId);
        }

        public static void SubmitTestMarksToClassTeacher(int aiStandardDivisionId,
                                                                           int aiSubjectId,
                                                                           int aiTestId,
                                                                           int aiSchoolId,
                                                                           int aiAcademicYrId,
                                                                           string asIsSubmitted)
        {
            SubjectTestTypeConfigurationCollectionDC.SubmitTestMarksToClassTeacher(aiStandardDivisionId, aiSubjectId, aiTestId, aiSchoolId, aiAcademicYrId, asIsSubmitted);
        }


        public static bool IsTestAndSubjectConfiguredForRemark(int aiSchoolId, int aiAcademicYearId, int aiTestId, int aiSubjectId)
        {
            return SubjectTestTypeConfigurationCollectionDC.IsTestAndSubjectConfiguredForRemark(aiSchoolId, aiAcademicYearId, aiTestId, aiSubjectId);
        }
    }
}
