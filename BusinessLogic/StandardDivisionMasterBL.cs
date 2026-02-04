using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;
using DataCommunicator;
using MasterEntities;
using System.Collections.Generic;

namespace BusinessLogic
{

    public class StandardDivisionMasterBL
    {
        #region DataMembers and properties

        #region Data members

        private StandardDivisionMasterDC.StandardDivisionStruct moStandardDivisionStruct;
        private StandardDivisionMasterDC moStandardDivisionMasterDC = new StandardDivisionMasterDC();
        private Collection<SubjectMasterBL> moSubjectCollectionBL;
        private Constants.Action eAction;

        #endregion

        #region Properties

        public int StandardDIvisionId
        {

            get { return moStandardDivisionStruct.miStandardDivisionId; }
            set { moStandardDivisionStruct.miStandardDivisionId = value; }
        }
        public string StandardDivisionName
        {

            get { return moStandardDivisionStruct.msStandardDivisionName; }
            set { moStandardDivisionStruct.msStandardDivisionName = value; }
        }

        public int StandardId
        {

            get { return moStandardDivisionStruct.miStandardId; }
            set { moStandardDivisionStruct.miStandardId = value; }
        }

        public int DivisionId
        {

            get { return moStandardDivisionStruct.miDivisionId; }
            set { moStandardDivisionStruct.miDivisionId = value; }
        }

        public int SchoolId
        {

            get { return moStandardDivisionStruct.miSchoolId; }
            set { moStandardDivisionStruct.miSchoolId = value; }
        }

        public int InsertedByid
        {

            get { return moStandardDivisionStruct.miInsertedByid; }
            set { moStandardDivisionStruct.miInsertedByid = value; }
        }

        public int UpdatedById
        {

            get { return moStandardDivisionStruct.miUpdatedById; }
            set { moStandardDivisionStruct.miUpdatedById = value; }
        }

        public Collection<SubjectMasterBL> SubjectCollection
        {
            get { return moSubjectCollectionBL; }
            set { moSubjectCollectionBL = value; }
        }

        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }
        #endregion
        #endregion

        #region Constructors

        public StandardDivisionMasterBL()
        {
        }
        public StandardDivisionMasterBL(int aiStandardDivisionId)
        {

            StandardDivisionMasterDC moStandardDivisionMasterDC = new StandardDivisionMasterDC(aiStandardDivisionId);
            moStandardDivisionStruct = moStandardDivisionMasterDC.StandardDivisionStructDetails;

        }
        public StandardDivisionMasterBL(int aiStandardId, int aiDivisionId)
        {

            StandardDivisionMasterDC moStandardDivisionMasterDC = new StandardDivisionMasterDC(aiStandardId, aiDivisionId);
            moStandardDivisionStruct = moStandardDivisionMasterDC.StandardDivisionStructDetails;

        }

        public DataTable GetStandardDivisionNamesForMessaging(int aiSchoolId, int aiAcademicYearId, int aiTeacherId, int aiTypeId)
        {
            StandardDivisionMasterDC oStandardDivisionMasterDC = new StandardDivisionMasterDC();
            return oStandardDivisionMasterDC.GetStandardDivisionNamesForMessaging(aiSchoolId, aiAcademicYearId, aiTeacherId, aiTypeId);
        }

        #endregion

        #region Public Methods

        public DataTable GetStandardAndDivisionName(int aiSchoolId, int aiStandardDivisionId)
        {
            return moStandardDivisionMasterDC.GetStandardAndDivisionName(aiSchoolId, aiStandardDivisionId);
        }

        #endregion

    }

    public class StandardDivisionCollectionBL
    {
        #region DataMembers
        StandardDivisionCollectionDC moStandardDivisionCollectionDC = null;

        #endregion

        #region Constructor

        public StandardDivisionCollectionBL(int aiSchoolId)
        {
            moStandardDivisionCollectionDC = new StandardDivisionCollectionDC(aiSchoolId);
        }
        public StandardDivisionCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            moStandardDivisionCollectionDC = new StandardDivisionCollectionDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion

        #region Public Methods
        public DataTable GetAssociatedStandardsDivisions()
        {
            return moStandardDivisionCollectionDC.GetAssociatedStandardsDivisions();
        }

        /// <summary>
        /// This method is used to get configured standered division for a test.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedStandardsDivisionsGorTest(int aiTestId)
        {
            return moStandardDivisionCollectionDC.GetAssociatedStandardsDivisionsGorTest(aiTestId);
        }

        /// <summary>
        /// This method is used to get configured standered division for a test.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedStandardsDivisionsGorTest()
        {
            return moStandardDivisionCollectionDC.GetAssociatedStandardsDivisionsGorTest();
        }

        /// <summary>
        /// This method is used to get configureds standered for a test.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedStandardsGorTest(int aiTestId)
        {
            return moStandardDivisionCollectionDC.GetAssociatedStandardsGorTest(aiTestId);
        }

        /// <summary>
        /// This method is used to get configureds standered where atleast one test associated.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedStandardsGorTest()
        {
            return moStandardDivisionCollectionDC.GetAssociatedStandardsGorTest();
        }

        /// <summary>
        /// This method is used to get configured standered division fo annual Result
        /// </summary>
        /// <returns></returns>
        public DataTable GetAnnualResultStandardsDivisions()
        {
            return moStandardDivisionCollectionDC.GetAnnualResultStandardsDivisions();
        }


        /*
          public DataSet GetAssociatedStandards()
         {
             return moStandardDivisionCollectionDC.GetAssociatedStandards();

         }

         public void UpdateStandards(Collection<StandardMasterBL> aoStandards)
         {
             IEnumerator oIEnum = aoStandards.GetEnumerator();
             ArrayList oArrayList = new ArrayList();
             while (oIEnum.MoveNext())
             {
                 StandardMasterBL oStandardMasterBL = (StandardMasterBL)oIEnum.Current;
                 switch (oStandardMasterBL.ConfigurationAction)
                 {
                     case Constants.Action.Insert:
                         oArrayList.Add(oStandardMasterBL.GetInsertStatementforStandard());
                         break;
                     case Constants.Action.Update:
                         oArrayList.Add(oStandardMasterBL.GetUpdateStatementforStandard());
                         break;
                     case Constants.Action.Delete:
                         oArrayList.Add(oStandardMasterBL.GetDeleteStatementforStandard());
                         break;
                 }
             }
             moStandardCollectionDC.UpdateStandards(oArrayList);
         }*/



        public void UpdateDivisionsSubjects(Collection<StandardDivisionMasterBL> aoStandardDivision)
        {
            IEnumerator oIEnum = aoStandardDivision.GetEnumerator();
            ArrayList oArrayList = new ArrayList();


            while (oIEnum.MoveNext())
            {
                StandardDivisionMasterBL oStandardDivisionMasterBL = (StandardDivisionMasterBL)oIEnum.Current;
                Collection<SubjectMasterBL> oDivisions = oStandardDivisionMasterBL.SubjectCollection;
                IEnumerator oIEnumSubjects = oDivisions.GetEnumerator();
                while (oIEnumSubjects.MoveNext())
                {
                    SubjectMasterBL oSubjectMasterBL = (SubjectMasterBL)oIEnumSubjects.Current;
                    switch (oSubjectMasterBL.ConfigurationAction)
                    {
                        case Constants.Action.Insert:
                            oArrayList.Add(oSubjectMasterBL.GetInsertStatementForStandardDivisionSubjects());
                            break;
                        case Constants.Action.Delete:
                            oArrayList.Add(oSubjectMasterBL.GetDeleteStatementForStandardDivisionSubjects());
                            break;
                    }
                }
            }

            moStandardDivisionCollectionDC.UpdateStandardDivisionsSubjects(oArrayList);


        }

        public DataSet GetStdDivAssociation()
        {
            return moStandardDivisionCollectionDC.GetStdDivAssociation();
        }

        public List<StandardDivisionMaster> GetStandardDivisionList()
        {
            return moStandardDivisionCollectionDC.GetStandardDivisionList();
        }

       
        #endregion
    }

}
