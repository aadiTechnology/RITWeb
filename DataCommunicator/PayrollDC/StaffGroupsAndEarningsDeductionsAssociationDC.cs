// Class Name       :- StaffGroupsAndEarningsDeductionsAssociationDC
// Purpose          :- This class is used to manage StaffGroupsAndEarningsDeductionsAssociation details.
// Date Of creation :- 11/2/2009
// Author Name      :- 

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    public class StaffGroupsAndEarningsDeductionsAssociationDC
    {
        #region Data Member(s)

        public StaffGroupsEarningDeductionAssociation moStaffGroupsEarningDeductionAssociation = new StaffGroupsEarningDeductionAssociation();
        private List<StaffGroupsEarningDeductionAssociation> mlstStaffGroupsEarningDeductionAssociations = new List<StaffGroupsEarningDeductionAssociation>();

        #endregion

        #region Property(s)

        public StaffGroupsEarningDeductionAssociation StaffGroupsEarningDeductionAssociation
        {
            get { return moStaffGroupsEarningDeductionAssociation; }
            set { moStaffGroupsEarningDeductionAssociation = value; }
        }

        public List<StaffGroupsEarningDeductionAssociation> StaffGroupsEarningDeductionAssociations
        {
            get { return mlstStaffGroupsEarningDeductionAssociations; }
            set { mlstStaffGroupsEarningDeductionAssociations = value; }
        }

        #endregion

        #region Method(s)

        /// <summary>
        /// This method is used to return earning-deduction formula.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiEarningDeductionId"></param>
        /// <returns></returns>
        public DataSet GetStaffGroupsAndEarningsDeductionsIds(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStaffGroupsAndEarningsDeductionsIds");
            }
        }

        /// <summary>
        /// This method is used to save staff attendance details.
        /// </summary>
        public void Save()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moStaffGroupsEarningDeductionAssociation.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moStaffGroupsEarningDeductionAssociation.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moStaffGroupsEarningDeductionAssociation.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssociationXml", moStaffGroupsEarningDeductionAssociation.AssociationXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertStaffGroupsAndEarningsDeductionsAsso");
            }
        }

        /// <summary>
        /// This method is used to return a dateset with category,subcategory and association details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public DataSet GetAssociation(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStaffGroupAndEarningsDeductionsAssociation");
            }
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill  staff group and earning deduction entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetSGEDAssociation(SqlDataReader oSqlDataReader)
        {
            StaffGroupsEarningDeductionAssociation oStaffGroupsEarningDeductionAssociationDC;
            while (oSqlDataReader.Read())
            {
                oStaffGroupsEarningDeductionAssociationDC = new StaffGroupsEarningDeductionAssociation
                {
                    StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"])
                };
                mlstStaffGroupsEarningDeductionAssociations.Add(oStaffGroupsEarningDeductionAssociationDC);
            }
        }

        #endregion
    }
}
