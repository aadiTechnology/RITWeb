using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{

    public class SubjectGroupsBL
    {

        #region Constants
        const string S_ERR_MSG_DUPLICATE_NAME = "Group name already exists.";
        #endregion

        #region DataMembers and properties

        #region Data members

        private SubjectGroupsDC.SubjectGroupsStruct moSubjectGroupsStruct;
        private SubjectGroupsDC moSubjectGroupsDC = new SubjectGroupsDC();
        private Collection<SubjectMasterBL> moSubjectCollectionBL;
        private Constants.Action eAction;

        #endregion
        #region Properties

        public int GroupId
        {

            get { return moSubjectGroupsStruct.miGroupId; }
            set { moSubjectGroupsStruct.miGroupId = value; }
        }
        public int SchoolId
        {

            get { return moSubjectGroupsStruct.miSchoolId; }
            set { moSubjectGroupsStruct.miSchoolId = value; }
        }
        public int ChangedParentSubjectId
        {

            get { return moSubjectGroupsStruct.miChangedParentSubjectId; }
            set { moSubjectGroupsStruct.miChangedParentSubjectId = value; }
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

            get { return moSubjectGroupsStruct.miSubjectId; }
            set { moSubjectGroupsStruct.miSubjectId = value; }
        }

        public int academicyearId
        {

            get { return moSubjectGroupsStruct.miacademicyearId; }
            set { moSubjectGroupsStruct.miacademicyearId = value; }
        }

        public Constants.Action ConfigurationAction
        {
            get { return eAction; }
            set { eAction = value; }
        }

        public Collection<SubjectMasterBL> SubjectCollection
        {
            get { return moSubjectCollectionBL; }
            set { moSubjectCollectionBL = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SubjectGroupsBL()
        {
        }
      
        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to check whether group of this subject is available or not.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asSubjectIds"></param>
        /// <returns></returns>
        public static bool IsSubjectGroupAvailable(int aiSchoolId, int aiAcademicYearId, string asSubjectIds, int aiStandardDivisionId)
        {
            return SubjectGroupsDC.IsSubjectGroupAvailable(aiSchoolId, aiAcademicYearId, asSubjectIds, aiStandardDivisionId);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoArrSubjectGroups"></param>
        public void DeleteSubjectGroup(ArrayList aoArrSubjectGroups)
        {
            moSubjectGroupsDC.SubjectGroupsStructDetails = moSubjectGroupsStruct;
            moSubjectGroupsDC.DeleteSubjectGroup(aoArrSubjectGroups);
        }
        public void UpdateSubjectGroups()
        {
            IEnumerator oIEnum = moSubjectCollectionBL.GetEnumerator();
            ArrayList oArrayList = new ArrayList();
            while (oIEnum.MoveNext())
            {
                SubjectMasterBL oSubjectBL = (SubjectMasterBL)oIEnum.Current;
                 
                switch (oSubjectBL.ConfigurationAction)
                {
                    case Constants.Action.Insert:
                        oArrayList.Add(oSubjectBL.GetInsertStamentForSubjectGroups());

                        break;
                    case Constants.Action.Update:
                        oArrayList.Add(oSubjectBL.GetUpdateStamentSubjectGroups(moSubjectGroupsStruct.miChangedParentSubjectId));
                    break;
                    case Constants.Action.Delete:
                        oArrayList.Add(oSubjectBL.GetDeleteStatementForSubjectGroups());
                        break;
                }
            }
            moSubjectGroupsDC.UpdateSubjectGroups(oArrayList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSubjectGroupId"></param>
        /// <returns></returns>
        public string RetriveSubjectsForGroup()
        {
            moSubjectGroupsDC.SubjectGroupsStructDetails = moSubjectGroupsStruct;
            return moSubjectGroupsDC.RetriveSubjectsForGroup();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetNextParentGroupId()
        {
            return SubjectGroupsDC.GetNextParentGroupId();
        }

        public DataTable RetriveSubjectIdsForGroup()
        {
            moSubjectGroupsDC.SubjectGroupsStructDetails = moSubjectGroupsStruct;
            return moSubjectGroupsDC.RetriveSubjectIdsForGroup();

        }

        public bool IsSubjectGroupPresent()
        {
            moSubjectGroupsDC.SubjectGroupsStructDetails = moSubjectGroupsStruct;
            return moSubjectGroupsDC.IsSubjectGroupPresent();
        }

        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SubjectGroupsCollectionBL
    {
        #region DataMembers
        SubjectGroupsCollectionDC moSubjectGroupsCollectionDC = null;
        #endregion

        #region Constructor


        public SubjectGroupsCollectionBL(int aiSchoolId, int aiAcademicYearId)
        {
            moSubjectGroupsCollectionDC = new SubjectGroupsCollectionDC(aiSchoolId, aiAcademicYearId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get all subject groups.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardDivisionId"></param>
        /// <returns></returns>
        public static DataTable GetAllSubjectGroups(int aiSchoolId, int aiAcademicYearId, int aiStandardDivisionId)
        {
            return SubjectGroupsCollectionDC.GetAllSubjectGroups(aiSchoolId, aiAcademicYearId, aiStandardDivisionId);
        }
       

        #endregion
       

    }
    #region exceptions

    /// <summary>
    /// This is an exception class
    /// Represents the error when there already exists a group with the name specified by the user
    /// </summary>
    
    public class DuplicateName : Exception
    {
        private string msMessage = "";

        public override string Message
        {
            get
            {
                return msMessage;
            }
        }

        public DuplicateName(string asMessage)
            : base(asMessage)
        {
            msMessage = asMessage;
        }

    }
    
    #endregion

}
