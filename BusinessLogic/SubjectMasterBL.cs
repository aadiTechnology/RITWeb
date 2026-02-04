using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{

    public class SubjectMasterBL
    {
        #region DataMembers and properties

        #region Data members

        private SubjectMasterDC.SubjectMasterStruct moSubjectMasterStruct;
        private SubjectMasterDC.StandardSubjectStruct moStandardSubjectStruct;
        private SubjectMasterDC.StandardDivisionSubjectStruct moStandardDivisionSubjectStruct;
        private SubjectMasterDC.SubjectGroupsStruct moSubjectGroupsStruct;


        private SubjectMasterDC moSubjectMasterDC = new SubjectMasterDC();
        private Constants.Action eAction;

        #endregion

        #region Properties
        public int GroupId
        {

            get { return moSubjectGroupsStruct.miGroupId; }
            set { moSubjectGroupsStruct.miGroupId = value; }
        }
     
        public int ParentSubjectId
        {

            get { return moSubjectGroupsStruct.miParentSubjectId; }
            set { moSubjectGroupsStruct.miParentSubjectId = value; }
        }

        public int ParentGroupId
        {

            get { return moSubjectGroupsStruct.miParentGroupId; }
            set { moSubjectGroupsStruct.miParentGroupId = value; }
        }

        public int SubjectId
        {

            get { return moSubjectMasterStruct.miSubjectId; }
            set { moSubjectMasterStruct.miSubjectId = value; }
        }

        public int StandardId
        {

            get { return moStandardSubjectStruct.miStandardId; }
            set { moStandardSubjectStruct.miStandardId = value; }
        }
        public int SchoolId
        {

            get { return moSubjectMasterStruct.miSchoolId; }
            set { moSubjectMasterStruct.miSchoolId = value; }
        }

        public string SubjectName
        {

            get { return moSubjectMasterStruct.msSubjectName; }
            set { moSubjectMasterStruct.msSubjectName = value; }
        }

		public string ShortName
		{
			get { return moSubjectMasterStruct.msShortName; }
			set { moSubjectMasterStruct.msShortName = value; }
		}

        public int OriginalSubjectId
        {

            get { return moSubjectMasterStruct.miOriginalSubjectId; }
            set { moSubjectMasterStruct.miOriginalSubjectId = value; }
        }

        public int InsertedByid
        {

            get { return moSubjectMasterStruct.miInsertedByid; }
            set { moSubjectMasterStruct.miInsertedByid = value; }
        }

        public int UpdatedById
        {

            get { return moSubjectMasterStruct.miUpdatedById; }
            set { moSubjectMasterStruct.miUpdatedById = value; }
        }

        public int StandardDivisionId
        {

            get { return moStandardDivisionSubjectStruct.miStandardDivisionId; }
            set { moStandardDivisionSubjectStruct.miStandardDivisionId = value; }
        }
        public int AcademicYearId
        {

            get { return moSubjectMasterStruct.miAcademicyearId; }
            set { moSubjectMasterStruct.miAcademicyearId = value; }
        }

        public int StandardDivisionID
        {

            get { return moSubjectMasterStruct.miStandardDivisionId; }
            set { moSubjectMasterStruct.miStandardDivisionId = value; }
        }

        public bool IsCoCurricularActivity
        {

            get { return moSubjectMasterStruct.mbIsCoCurricularActivity; }
            set { moSubjectMasterStruct.mbIsCoCurricularActivity = value; }
        }

        public bool IsAttitudeSubject
        {

            get { return moSubjectMasterStruct.mbIsAttitudeSubject; }
            set { moSubjectMasterStruct.mbIsAttitudeSubject = value; }
        }

        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }


        #endregion

        #endregion

        #region Constructors

        public SubjectMasterBL()
        {
        }
        /*public SubjectMasterBL(int aiId)
        {

            SubjectMasterDC moSubjectMasterDC = new SubjectMasterDC(aiId);

        } */
        #endregion

        #region Public Methods

        #region Subjects

        public string GetInsertStatementForSubject()
        {

            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            return moSubjectMasterDC.GetInsertStatementForSubject();
        }
        public string GetUpdateStatementForSubject()
        {

            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            return moSubjectMasterDC.GetUpdateStatementForSubject();
        }
        public string GetDeleteStatementForSubject()
        {

            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            return moSubjectMasterDC.GetDeleteStatementForSubject();
        }
        public string GetSubjectName(int aiSchoolId, int aiSubjectId)
        {
            return moSubjectMasterDC.GetSubjectName(aiSchoolId, aiSubjectId);
        }
       
        #endregion

        #region Standard Subjects


        public string GetInsertStatementForStandardSubjects()
        {
            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            moSubjectMasterDC.StandardSubjectStructDetails = moStandardSubjectStruct;
            return moSubjectMasterDC.GetInsertStatementForStandardSubjects();
        }
        public string GetDeleteStatementForStandardSubjects()
        {
            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            moSubjectMasterDC.StandardSubjectStructDetails = moStandardSubjectStruct;
            return moSubjectMasterDC.GetDeleteStatementForStandardSubjects();
        }
        /// <summary>
        /// This method is used to update sort order of subjects of given standard
        /// </summary>
        /// <param name="ischoolId"></param>
        /// <param name="iAcademicYearId"></param>
        /// <param name="iStandardId"></param>
        /// <param name="sXmlSubjectOrder"></param>
        public void UpdateSubjectSortOrder(int ischoolId, int iAcademicYearId, int iStandardId, string sXmlSubjectOrder)
        {
            moSubjectMasterDC.UpdateSubjectSortOrder(ischoolId, iAcademicYearId, iStandardId, sXmlSubjectOrder);
        }
        #endregion

        #region Standard-Division Subjects


        public string GetInsertStatementForStandardDivisionSubjects()
        {
            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            moSubjectMasterDC.StandardSubjectStructDetails = moStandardSubjectStruct;
            moSubjectMasterDC.StandardDivisionSubjectStructDetails = moStandardDivisionSubjectStruct;
            return moSubjectMasterDC.GetInsertStatementForStandardDivisionSubjects();
        }

        public string GetDeleteStatementForStandardDivisionSubjects()
        {
            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            moSubjectMasterDC.StandardSubjectStructDetails = moStandardSubjectStruct;
            moSubjectMasterDC.StandardDivisionSubjectStructDetails = moStandardDivisionSubjectStruct;
            return moSubjectMasterDC.GetDeleteStatementForStandardDivisionSubjects();
        }
        #endregion

        #region Subjects groups


        public string GetInsertStamentForSubjectGroups()
        {
            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            moSubjectMasterDC.SubjectGroupsStructDetails = moSubjectGroupsStruct;
            return moSubjectMasterDC.GetInsertStamentForSubjectGroups();
        }

        public string GetDeleteStatementForSubjectGroups()
        {
            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            moSubjectMasterDC.SubjectGroupsStructDetails = moSubjectGroupsStruct;
            return moSubjectMasterDC.GetDeleteStatementForSubjectGroups();
        }

        public string GetUpdateStamentSubjectGroups(int aiParentSubjectIdToChange)
        {
            moSubjectMasterDC.SubjectMasterStructDetails = moSubjectMasterStruct;
            moSubjectMasterDC.SubjectGroupsStructDetails = moSubjectGroupsStruct;
            return moSubjectMasterDC.GetUpdateStamentSubjectGroups(aiParentSubjectIdToChange);
        }

        public string GetSubjectNameForSubjectId(int aiSubjectId)
        {
            moSubjectMasterDC.SubjectGroupsStructDetails = moSubjectGroupsStruct;
            return moSubjectMasterDC.GetSubjectNameForSubjectId(aiSubjectId); 
        }
        public static int CheckMarksAssigned(int aiSubjectId, int aiSchoolId, int aiAcademicYearId)
        {
            return SubjectMasterDC.CheckMarksAssigned(aiSubjectId,aiSchoolId,aiAcademicYearId);
        }
        
#endregion


        #endregion


       
    }

    public class SubjectCollectionBL
    {
        #region DataMembers
        SubjectCollectionDC moSubjectCollectionDC = null;
        #endregion

        #region Constructor

        public SubjectCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            moSubjectCollectionDC = new SubjectCollectionDC(aiSchoolId, aiAcademicYearId);
        }
      
        public SubjectCollectionBL(int aiSchoolId)
        {
            moSubjectCollectionDC = new SubjectCollectionDC(aiSchoolId);
        }
        #endregion

        #region Public Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSubject()
        {
            return moSubjectCollectionDC.GetAllSubjects();
        }
        /// <summary>
        /// This method is used to fill dropdown list of child and parent subjects. 
        /// </summary>
        /// <returns></returns>
        public DataSet GetChildParentSubjects()
        {
            return moSubjectCollectionDC.GetChildParentSubjects();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssociatedSubjects()
        {
            return moSubjectCollectionDC.GetAssociatedSubjects();
        }

        /// <summary>
        /// This method is used to get all subjcts of given standard
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public DataTable GetSubjectsForStandard(int aiStandardId)
        {
            return moSubjectCollectionDC.GetSubjectsForStandard(aiStandardId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        //public DataTable GetAllStandardSubjects()
        //{
        //    return moSubjectCollectionDC.GetAllStandardSubjects();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiClassId"></param>
        /// <returns></returns>
        public DataSet GetAllSubjectsforDivision(int aiClassId)
        {
            return moSubjectCollectionDC.GetAllSubjectsforDivision(aiClassId);
        }
        #region update collection

        private string CheckDependenciesForSubjects(Collection<SubjectMasterBL> aoSubjects, int aiAcadId)
        {
            //get the id and name of the standards to be deleted into hashtable.
            GenericReferenceList<SubjectMasterBL> objSubjectsRefereces = new GenericReferenceList<SubjectMasterBL>(aoSubjects, aiAcadId);
            string sReturn = "";
            sReturn = objSubjectsRefereces.CheckDependencies("SubjectId", "SubjectName", "ConfigurationAction", Constants.ReferenceId.Subjects, false);
            return sReturn;
        }
        public void UpdateStandardDivisionsSubject(Collection<StandardDivisionMasterBL> aoStandardDivisions)
        {
            IEnumerator oIEnum = aoStandardDivisions.GetEnumerator();
            ArrayList oArrayList = new ArrayList();
            while (oIEnum.MoveNext())
            {
                StandardDivisionMasterBL oStandardDivisionMasterBL = (StandardDivisionMasterBL)oIEnum.Current;
                Collection<SubjectMasterBL> oSubjects = oStandardDivisionMasterBL.SubjectCollection;
                IEnumerator oIEnumDivisions = oSubjects.GetEnumerator();
                while (oIEnumDivisions.MoveNext())
                {
                    SubjectMasterBL oSubjectMasterBL = (SubjectMasterBL)oIEnumDivisions.Current;
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
            moSubjectCollectionDC.UpdateStandardDivisionsSubjects(oArrayList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoSubjects"></param>

        public void UpdateSubjects(Collection<SubjectMasterBL> aoSubjects, int aiAcadYrId)
        {

            string sMessage = CheckDependenciesForSubjects(aoSubjects, aiAcadYrId);
            if (string.IsNullOrEmpty(sMessage))
            {
                IEnumerator oIEnum = aoSubjects.GetEnumerator();
                ArrayList oArrayList = new ArrayList();
                while (oIEnum.MoveNext())
                {

                    SubjectMasterBL oSubjectMasterBL = (SubjectMasterBL)oIEnum.Current;
                    switch (oSubjectMasterBL.ConfigurationAction)
                    {
                        case Constants.Action.Insert:
                            oArrayList.Add(oSubjectMasterBL.GetInsertStatementForSubject());
                            break;
                        case Constants.Action.Update:
                            oArrayList.Add(oSubjectMasterBL.GetUpdateStatementForSubject());
                            break;
                        case Constants.Action.Delete:
                            {
                                oArrayList.Add(oSubjectMasterBL.GetDeleteStatementForSubject());
                            }

                            break;
                    }
                }
                    moSubjectCollectionDC.UpdateSubjects(oArrayList);
            }
            else
            {
                throw new Exceptions.ReferenceExceptions(sMessage);
            }
           
        }
        #endregion

        #endregion

    }

}
