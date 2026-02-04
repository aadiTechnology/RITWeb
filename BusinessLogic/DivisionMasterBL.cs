using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{

    public class DivisionMasterBL
    {


        #region DataMembers and properties

        #region Data members

        private DivisionMasterDC.DivisionMasterStruct moDivisionMasterStruct;
        private DivisionMasterDC.StandardDivisionStruct moStandardDivisionStruct;

        private DivisionMasterDC moDivisionMasterDC = new DivisionMasterDC();
        private Constants.Action eAction;

        #endregion
        #region Properties

        public int DivisionId
        {

            get { return moDivisionMasterStruct.miDivisionId; }
            set { moDivisionMasterStruct.miDivisionId = value; }
        }
        public int AcademicYearId
        {

            get { return moDivisionMasterStruct.miAcademicYearId; }
            set { moDivisionMasterStruct.miAcademicYearId = value; }
        }

        public int StandardId
        {

            get { return moStandardDivisionStruct.miStandardId; }
            set { moStandardDivisionStruct.miStandardId = value; }
        }

        public string DisplayName
        {

            get { return moStandardDivisionStruct.msDisplayName; }
            set { moStandardDivisionStruct.msDisplayName = value; }
        }

        public int SchoolId
        {

            get { return moDivisionMasterStruct.miSchoolId; }
            set { moDivisionMasterStruct.miSchoolId = value; }
        }

        public string DivisionName
        {

            get { return moDivisionMasterStruct.msDivisionName; }
            set { moDivisionMasterStruct.msDivisionName = value; }
        }

        public int OriginalDivisionId
        {

            get { return moDivisionMasterStruct.miOriginalDivisionId; }
            set { moDivisionMasterStruct.miOriginalDivisionId = value; }
        }

        public int InsertedByid
        {

            get { return moDivisionMasterStruct.miInsertedByid; }
            set { moDivisionMasterStruct.miInsertedByid = value; }
        }

        public int UpdatedById
        {

            get { return moDivisionMasterStruct.miUpdatedById; }
            set { moDivisionMasterStruct.miUpdatedById = value; }
        }

        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public DivisionMasterBL()
        {
        }
        /*public DivisionMasterBL(int aiId)
        {

            DivisionMasterDC moDivisionMasterDC = new DivisionMasterDC(aiId);

        } */
        #endregion

        #region Public Methods

        #region Standard Divisions

        public string GetInsertStatementForStandardDivision()
        {
            moDivisionMasterDC.DivisionMasterStructDetails = moDivisionMasterStruct;
            moDivisionMasterDC.StandardDivisionStructDetails = moStandardDivisionStruct;
            return moDivisionMasterDC.GetInsertStatementForStandardDivision();
        }

        public string GetDeleteStatementForStandardDivision()
        {
            moDivisionMasterDC.DivisionMasterStructDetails = moDivisionMasterStruct;
            moDivisionMasterDC.StandardDivisionStructDetails = moStandardDivisionStruct;
            return moDivisionMasterDC.GetDeleteStatementForStandardDivision();
        }

        public string GetUpdateStatementForStandardDivision()
        {
            moDivisionMasterDC.DivisionMasterStructDetails = moDivisionMasterStruct;
            moDivisionMasterDC.StandardDivisionStructDetails = moStandardDivisionStruct;
            return moDivisionMasterDC.GetUpdateStatementForStandardDivision();
        }

       


        #endregion

        #region Divisions

        public string GetInsertStatementForDivision()
        {

            moDivisionMasterDC.DivisionMasterStructDetails = moDivisionMasterStruct;
            return moDivisionMasterDC.GetInsertStatementForDivision();
        }
        public string GetUpdateStatementForDivision()
        {

            moDivisionMasterDC.DivisionMasterStructDetails = moDivisionMasterStruct;
            return moDivisionMasterDC.GetUpdateStatementForDivision();
        }
        public string GetDeleteStatementForDivision()
        {

            moDivisionMasterDC.DivisionMasterStructDetails = moDivisionMasterStruct;
            return moDivisionMasterDC.GetDeleteStatementForDivision();
        }

        public DataTable GetDivisionsForHomeWork(int aiStdDivId, int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiSubjectId)
        {
            return moDivisionMasterDC.GetDivisionsForHomeWork(aiStdDivId, aiSchoolId, aiAcademicYearId, aiUserId, aiSubjectId);
        }

        #endregion
        #endregion


       
    }

    public class DivisionCollectionBL
    {
        #region DataMembers
        DivisionCollectionDC moDivisionCollectionDC = null;
        #endregion

        #region Constructor
        public DivisionCollectionBL(int aiSchoolId)
        {
            moDivisionCollectionDC = new DivisionCollectionDC(aiSchoolId);
        }
        public DivisionCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            moDivisionCollectionDC = new DivisionCollectionDC(aiSchoolId, aiAcademicYearId);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method calls a function to check the RI dependencies for standards that are to be deleted
        /// </summary>
        /// <param name="aoStandards"></param>
        /// <param name="aiAcadId"></param>
        /// <returns></returns>
        private string CheckDependenciesForDivs(Collection<DivisionMasterBL> aoDivisions, int aiAcadId)
        {
            //get the id and name of the standards to be deleted into hashtable.
            GenericReferenceList<DivisionMasterBL> objDivRefereces = new GenericReferenceList<DivisionMasterBL>(aoDivisions, aiAcadId);
            return objDivRefereces.CheckDependencies("DivisionId", "DivisionName", "ConfigurationAction", Constants.ReferenceId.Division,  false);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoStandards"></param>
        /// <returns></returns>
        private Hashtable GetHashTable(Collection<DivisionMasterBL> aoStandards)
        {
            Hashtable oHashStds = new Hashtable();
            IEnumerator oIEnum = aoStandards.GetEnumerator();
            while (oIEnum.MoveNext())
            {
                DivisionMasterBL oDivisionMasterBL = (DivisionMasterBL)oIEnum.Current;

                if (oDivisionMasterBL.ConfigurationAction.Equals(Constants.Action.Delete))
                {
                    oHashStds.Add(oDivisionMasterBL.DivisionId, oDivisionMasterBL.DivisionName);
                }
            }
            return oHashStds;
        }
       
        public void UpdateDivisions(Collection<DivisionMasterBL> aoDivisions, int aiAcaYrId)
        {
            IEnumerator oIEnum = aoDivisions.GetEnumerator();
            ArrayList oArrayList = new ArrayList();
            string sMessage = "";
            sMessage = CheckDependenciesForDivs(aoDivisions, aiAcaYrId);
            if (string.IsNullOrEmpty(sMessage))
            {
                while (oIEnum.MoveNext())
                {
                    DivisionMasterBL oDivisionMasterBL = (DivisionMasterBL)oIEnum.Current;
                    switch (oDivisionMasterBL.ConfigurationAction)
                    {
                        case Constants.Action.Insert:
                            oArrayList.Add(oDivisionMasterBL.GetInsertStatementForDivision());
                            break;
                        case Constants.Action.Update:
                            oArrayList.Add(oDivisionMasterBL.GetUpdateStatementForDivision());
                            break;
                        case Constants.Action.Delete:
                            oArrayList.Add(oDivisionMasterBL.GetDeleteStatementForDivision());
                            break;
                    }
                }
                moDivisionCollectionDC.UpdateDivisions(oArrayList);
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }

        }

        public DataTable GetAllDivisions()
        {
            return moDivisionCollectionDC.GetAllDivisions();
        }

        public DataTable GetAllDivisionsForStandard(int aiStandardId)
        {
            return moDivisionCollectionDC.GetAllDivisionsForStandard(aiStandardId);
        }

        public DataTable GetAllDivisionsForStandardForAdmissionConfirmation(int aiStandardId, int aiAdmissionTypeId)
        {
            return moDivisionCollectionDC.GetAllDivisionsForStandardForAdmissionConfirmation(aiStandardId, aiAdmissionTypeId);
        }

        public DataTable GetAllDivisionsForAdmissionSibling(int aiStandardId)
        {
            return moDivisionCollectionDC.GetAllDivisionsForAdmissionSibling(aiStandardId);
        }

        public DataTable GetAllDivisionsForStandards(string asStandardIds)
        {
            return moDivisionCollectionDC.GetAllDivisionsForStandards(asStandardIds);
        }

        

         /// <summary>
        /// This method is used to get all school associated divisions.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSchoolDivisions()
        {
            return moDivisionCollectionDC.GetAllSchoolDivisions();
        }

        /// <summary>
        /// This method is sued to get std-div ids for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GettStdDivIdForSchool()
        {
            return moDivisionCollectionDC.GetStdDivIdForSchool();
        }

         /// <summary>
        /// This method is used to get stddivid for given class.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public DataTable GetStdDivIdForClass(int aiStandardId, int aiDivisionId)
        {
            return moDivisionCollectionDC.GetStdDivIdForClass(aiStandardId, aiDivisionId);
        }

        #endregion
    }

}
