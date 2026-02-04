// Class Name       :- StaffGroupsBL
// Purpose          :- This class is used to manage StaffGroups details.
// Date Of creation :- 11/2/2009
// Author Name      :- Sachin


using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using DataCommunicator;
using PayrollEntities;
using Utility;

namespace BusinessLogic
{
    public class StaffGroupsBL
    {
        #region Data Member(s)

        private StaffGroupsDC moStaffGroupsDC;
        private StaffGroupsEntity moStaffGroupsEntity = null;
        private Collection<StaffGroupsAndEarningsDeductionsAssociationBL> moStaffGroupsAndEarningsDeductionsAssociationBL; 

        #endregion

        #region Consturctor(s)

        public StaffGroupsBL()
        {
            moStaffGroupsDC = new StaffGroupsDC();
            moStaffGroupsEntity = new StaffGroupsEntity();
        } 

        #endregion

        #region Property(s)

        public StaffGroupsEntity StaffGroupsDetails
        {
            get { return moStaffGroupsEntity; }
            set { moStaffGroupsEntity = value; }
        }

        public Collection<StaffGroupsAndEarningsDeductionsAssociationBL> StaffGroupsAndearningsDeductionsCollection
        {
            get { return moStaffGroupsAndEarningsDeductionsAssociationBL; }
            set { moStaffGroupsAndEarningsDeductionsAssociationBL = value; }
        }

        public StaffGroupsDC StaffGroupsDC
        {
            get { return moStaffGroupsDC; }
            set { moStaffGroupsDC = value; }
        }

        public List<StaffGroupsEntity> StaffGroups
        {
            get { return moStaffGroupsDC.StaffGroups; }
        } 

        #endregion

        #region Method(s)

        /// <summary>
        /// This emthod is used to return insert statement.
        /// </summary>
        /// <returns></returns>
        public string GetInsertStatement()
        {
            moStaffGroupsDC.StaffGroupsEntity = moStaffGroupsEntity;
            return moStaffGroupsDC.GetInsertStatement();
        }

        /// <summary>
        /// This emthod is used to return update statement.
        /// </summary>
        /// <returns></returns>
        public string GetUpdateStatement()
        {
            moStaffGroupsDC.StaffGroupsEntity = moStaffGroupsEntity;
            return moStaffGroupsDC.GetUpdateStatement();
        }

        /// <summary>
        /// This emthod is used to return delete statement.
        /// </summary>
        /// <returns></returns>
        public string GetDeleteStatement()
        {
            moStaffGroupsDC.StaffGroupsEntity = moStaffGroupsEntity;
            return moStaffGroupsDC.GetDeleteStatement();
        }

        /// <summary>
        /// This method is used to return all staff groups.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataTable GetAll(int aiSchoolId)
        {
            return StaffGroupsDC.GetAll(aiSchoolId);
        }

        /// <summary>
        /// This method is used to return all staff groups.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<StaffGroupsEntity> GetAllStaffGroups(int aiSchoolId)
        {
            return moStaffGroupsDC.GetAllStaffGroups(aiSchoolId);
        }

        /// <summary>
        /// This method is used to add querystrings into arraylist to insert staff gropus into table StaffGroups.
        /// </summary>
        /// <param name="aoStaffGroups"></param>
        /// <param name="aiAcademicYearId"></param>
        public void Update(List<StaffGroupsEntity> aoStaffGroups, int aiAcademicYearId)
        {
            string sMessage = CheckDependencies(aoStaffGroups, aiAcademicYearId);
            if (string.IsNullOrEmpty(sMessage))
            {
                IEnumerator oIEnum = aoStaffGroups.GetEnumerator();
                ArrayList oArrayList = new ArrayList();
                while (oIEnum.MoveNext())
                {
                    StaffGroupsEntity oStaffGroupsEntity = (StaffGroupsEntity)oIEnum.Current;
                    moStaffGroupsEntity = oStaffGroupsEntity;
                    switch (oStaffGroupsEntity.Action)
                    {
                        case Constants.Action.Insert:
                            oArrayList.Add(GetInsertStatement());
                            break;
                        case Constants.Action.Update:
                            oArrayList.Add(GetUpdateStatement());
                            break;
                        case Constants.Action.Delete:
                            oArrayList.Add(GetDeleteStatement());
                            break;
                    }
                }
                moStaffGroupsDC.Update(oArrayList);
            }
            else
                throw new Exceptions.ReferenceExceptions(sMessage);
        }

        /// <summary>
        /// This method is used to check dependancies of category.
        /// </summary>
        /// <param name="aoStaffGroups"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        private string CheckDependencies(List<StaffGroupsEntity> aoStaffGroups, int aiAcademicYearId)
        {
            GenericReferenceList<StaffGroupsEntity> objStdRefereces = new GenericReferenceList<StaffGroupsEntity>(aoStaffGroups, aiAcademicYearId);
            return objStdRefereces.CheckDependenciesForList("StaffGroupsId", "StaffGroupsName", "Action", Constants.ReferenceId.StaffGroups, false);
        }

        #endregion
    }
}