// Class Name       :- StaffGroupsAndEarningsDeductionsAssociationBL
// Purpose          :- This class is used to manage StaffGroupsAndEarningsDeductionsAssociation details.
// Date Of creation :- 11/2/2009
// Author Name      :- 

using System.Collections.Generic;
using System.Data;
using DataCommunicator;
using PayrollEntities;
using Utility;

namespace BusinessLogic
{
    public class StaffGroupsAndEarningsDeductionsAssociationBL
    {
        #region Data Member(s)

        private StaffGroupsAndEarningsDeductionsAssociationDC moStaffGroupsAndEarningsDeductionsAssociationDC;
        
        #endregion
        
        #region Constructor(s)

        public StaffGroupsAndEarningsDeductionsAssociationBL()
        {
            moStaffGroupsAndEarningsDeductionsAssociationDC = new StaffGroupsAndEarningsDeductionsAssociationDC();
        } 

        #endregion

        #region Property(s)

        public List<StaffGroupsEarningDeductionAssociation> StaffGroupsEarningDeductionAssociations
        {
            get { return moStaffGroupsAndEarningsDeductionsAssociationDC.StaffGroupsEarningDeductionAssociations; }
            set { moStaffGroupsAndEarningsDeductionsAssociationDC.StaffGroupsEarningDeductionAssociations = value; }
        }

        public StaffGroupsEarningDeductionAssociation StaffGroupsEarningDeductionAssociation
        {
            get { return moStaffGroupsAndEarningsDeductionsAssociationDC.StaffGroupsEarningDeductionAssociation; }
            set { moStaffGroupsAndEarningsDeductionsAssociationDC.StaffGroupsEarningDeductionAssociation = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to save association.
        /// </summary>
        public void Save()
        {         
            moStaffGroupsAndEarningsDeductionsAssociationDC.Save();
        }

        /// <summary>
        /// This method is used to return association details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public  DataSet GetStaffGroupsAndEarningsDeductionsIds(int aiSchoolId, int aiAcademicYearId)
        {
            return moStaffGroupsAndEarningsDeductionsAssociationDC.GetStaffGroupsAndEarningsDeductionsIds(aiSchoolId, aiAcademicYearId);
        }

        /// <summary>
        /// This method is used to return a dateset with category,subcategory and association details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public  DataSet GetAssociation(int aiSchoolId, int aiAcademicYearId)
        {
            return moStaffGroupsAndEarningsDeductionsAssociationDC.GetAssociation(aiSchoolId, aiAcademicYearId);
        } 

        #endregion
    }
}
