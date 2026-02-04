using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{

    public class SchoolWiseTestMasterBL
    {


        #region DataMembers and properties

        #region Data members

        private SchoolWiseTestMasterDC.SchoolWiseTestMasterStruct moSchoolWiseTestMasterStruct;
        private SchoolWiseTestMasterDC moSchoolWiseTestMasterDC = new SchoolWiseTestMasterDC();
        private Constants.Action eAction;
        #endregion
        #region Properties

        public int SchoolWiseTestId
        {

            get { return moSchoolWiseTestMasterStruct.miSchoolWiseTestId; }
            set { moSchoolWiseTestMasterStruct.miSchoolWiseTestId = value; }
        }

        public string SchoolWiseTestName
        {

            get { return moSchoolWiseTestMasterStruct.msSchoolWiseTestName; }
            set { moSchoolWiseTestMasterStruct.msSchoolWiseTestName = value; }
        }
        public int AcademicYearId
        {

            get { return moSchoolWiseTestMasterStruct.miAcademicYearId; }
            set { moSchoolWiseTestMasterStruct.miAcademicYearId = value; }
        }

        public int OriginalSchoolWiseTestId
        {

            get { return moSchoolWiseTestMasterStruct.miOriginalSchoolWiseTestId; }
            set { moSchoolWiseTestMasterStruct.miOriginalSchoolWiseTestId = value; }
        }

        public int SchoolId
        {

            get { return moSchoolWiseTestMasterStruct.miSchoolId; }
            set { moSchoolWiseTestMasterStruct.miSchoolId = value; }
        }

        public string IsDeleted
        {

            get { return moSchoolWiseTestMasterStruct.msIsDeleted; }
            set { moSchoolWiseTestMasterStruct.msIsDeleted = value; }
        }

        public DateTime InsertDate
        {

            get { return moSchoolWiseTestMasterStruct.mdtInsertDate; }
            set { moSchoolWiseTestMasterStruct.mdtInsertDate = value; }
        }

        public int InsertedByid
        {

            get { return moSchoolWiseTestMasterStruct.miInsertedByid; }
            set { moSchoolWiseTestMasterStruct.miInsertedByid = value; }
        }

        public DateTime UpdateDate
        {

            get { return moSchoolWiseTestMasterStruct.mdtUpdateDate; }
            set { moSchoolWiseTestMasterStruct.mdtUpdateDate = value; }
        }

        public int UpdatedById
        {

            get { return moSchoolWiseTestMasterStruct.miUpdatedById; }
            set { moSchoolWiseTestMasterStruct.miUpdatedById = value; }
        }
        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }

        public int TermId
        {
            get { return moSchoolWiseTestMasterStruct.miTermId; }
            set { moSchoolWiseTestMasterStruct.miTermId = value; }
        }


        public int IsFinalExam
        {
            get { return moSchoolWiseTestMasterStruct.miIsFinalExam; }
            set { moSchoolWiseTestMasterStruct.miIsFinalExam = value; }
        }
        

        #endregion
        #endregion

        #region Constructors

        public SchoolWiseTestMasterBL()
        {
        }
        //public SchoolWiseTestMasterBL(int aiId){

        //SchoolWiseTestMasterDC moSchoolWiseTestMasterDC = new SchoolWiseTestMasterDC(aiId);

        //}
        #endregion

        #region Public Methods

        public string GetInsertStatementForTestMaster()
        {

            moSchoolWiseTestMasterDC.SchoolWiseTestMasterStructDetails = moSchoolWiseTestMasterStruct;
            return moSchoolWiseTestMasterDC.GetInsertStatementForTestMaster();
        }
        public string GetUpdateStamentForTestMaster()
        {

            moSchoolWiseTestMasterDC.SchoolWiseTestMasterStructDetails = moSchoolWiseTestMasterStruct;
            return moSchoolWiseTestMasterDC.GetUpdateStamentForTestMaster();
        }
        public string GetDeleteStatementForTestMaster()
        {

            moSchoolWiseTestMasterDC.SchoolWiseTestMasterStructDetails = moSchoolWiseTestMasterStruct;
            return moSchoolWiseTestMasterDC.GetDeleteStatementForTestMaster();
        }
        /// <summary>
        /// This method returns the message informing about the dependencies.
        /// </summary>
        /// <returns></returns>
        public string CheckDependenciesForTestNames()
        {
            string sReturn = "";
            sReturn = ReferenceDC.CheckDependenciesAndGetErrorMessages(Convert.ToInt32(Constants.ReferenceId.ExamName), moSchoolWiseTestMasterStruct.miSchoolWiseTestId,
                moSchoolWiseTestMasterStruct.msSchoolWiseTestName, moSchoolWiseTestMasterStruct.miAcademicYearId
                );
            return sReturn;
        }

        /// <summary>
        /// This method is used to find latest exam id.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAccYearId"></param>
        /// <returns></returns>
        public static int GetLatestExamId(int aiSchoolId, int aiAccYearId, int aiStandardDivisionId,int aiStandardId)
        {
            return SchoolWiseTestMasterDC.GetLatestExamId(aiSchoolId, aiAccYearId, aiStandardDivisionId, aiStandardId);
        }

        #endregion

    }
    public class TestCollectionBL
    {
        #region Data Members
        TestCollectionDC moTestCollectionDC = null;
        #endregion
        #region Constructor
        public TestCollectionBL(int aiSchoolId)
        {
            moTestCollectionDC = new TestCollectionDC(aiSchoolId);
        }
        public TestCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            moTestCollectionDC = new TestCollectionDC(aiSchoolId, aiAcademicYearId);
        }

        public TestCollectionBL(int aiSchoolId, int aiAcademicYearId, int aiStanderedDivisionId)
        {
            moTestCollectionDC = new TestCollectionDC(aiSchoolId, aiAcademicYearId, aiStanderedDivisionId);
        }

        #endregion
        #region public methods
        public DataTable GetAllTests()
        {
            return moTestCollectionDC.GetAllTests();
        }
        /// <summary>
        /// Get all test for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForSchool()
        {
            return moTestCollectionDC.GetAllTestsForSchool();
        }

        /// <summary>
        /// This method is uedd to get test details for fill combobox.
        /// </summary>
        /// <param name="miStandardDivId"></param>
        /// <returns></returns>
        public DataTable GetAllTestsForClass(int aiStandardDivId)
        {
            return moTestCollectionDC.GetAllTestsForClass(aiStandardDivId);
        }

        /// <summary>
        /// Get all published test for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllpublishedTestsForClass()
        {
            return moTestCollectionDC.GetAllpublishedTestsForClass();
        }

        /// <summary>
        /// Get all tests for which toppers are generated.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForWhichToppersGenerated()
        {
            return moTestCollectionDC.GetAllTestsForWhichToppersGenerated();
        }

        /// <summary>
        /// Get all published tests for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllpublishedTestsForStandard(int @Standard_Id, int aiStudentId)
        {
            return moTestCollectionDC.GetAllpublishedTestsForStandard(@Standard_Id, aiStudentId);
        }

        /// <summary>
        /// Get all published tests for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForStandard(int @Standard_Id, bool abIsServiceCall = false)
        {
            return moTestCollectionDC.GetAllTestsForStandard(@Standard_Id, abIsServiceCall);
        }

        /// <summary>
        /// Get all published tests for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTestsForStandard(int aiStandard_Id,int aiHeaderId)
        {
            return moTestCollectionDC.GetAllTestsForStandard(aiStandard_Id,aiHeaderId);
        }

        /// <summary>
        /// This method calls a function to check the RI dependencies for standards that are to be deleted
        /// </summary>
        /// <param name="aoStandards"></param>
        /// <param name="aiAcadId"></param>
        /// <returns></returns>
        private string CheckDependenciesForTests(Collection<SchoolWiseTestMasterBL> aoTests, int aiAcadId)
        {
            //get the id and name of the standards to be deleted into hashtable.
            GenericReferenceList<SchoolWiseTestMasterBL> objStdRefereces = new GenericReferenceList<SchoolWiseTestMasterBL>(aoTests, aiAcadId);
            return objStdRefereces.CheckDependencies("SchoolWiseTestId", "SchoolWiseTestName", "ConfigurationAction", Constants.ReferenceId.ExamName, false);
        }

        public void UpdateTests(Collection<SchoolWiseTestMasterBL> aoTests, int aiAcadYrId)
        {
            string sMessage = CheckDependenciesForTests(aoTests, aiAcadYrId);
            if (string.IsNullOrEmpty(sMessage))
            {
                IEnumerator oIEnum = aoTests.GetEnumerator();
                ArrayList oArrayList = new ArrayList();
                while (oIEnum.MoveNext())
                {
                    SchoolWiseTestMasterBL oTestMasterBL = (SchoolWiseTestMasterBL)oIEnum.Current;
                    switch (oTestMasterBL.ConfigurationAction)
                    {
                        case Constants.Action.Insert:
                            oArrayList.Add(oTestMasterBL.GetInsertStatementForTestMaster());
                            break;
                        case Constants.Action.Update:
                            oArrayList.Add(oTestMasterBL.GetUpdateStamentForTestMaster());
                            break;
                        case Constants.Action.Delete:
                            oArrayList.Add(oTestMasterBL.GetDeleteStatementForTestMaster());
                            break;
                    }
                }

                moTestCollectionDC.UpdateTests(oArrayList);
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }

        }

        public DataTable GetAllExamsForTestwiseTopperReport(int aiStandardId)
        {
            return moTestCollectionDC.GetAllExamsForTestwiseTopperReport(aiStandardId);
        }

        #endregion
    }

}
