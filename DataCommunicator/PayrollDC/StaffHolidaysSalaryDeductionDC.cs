// Class Name       :- StaffHolidayAndLeavesConfigurationDC
// Purpose          :- This class is used to configuration Staff holiday for salary deduction details.
// Date Of creation :- 12/09/2010
// Author Name      :- Shobha Patil

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class StaffHolidaysSalaryDeductionDC
    {
        #region Constant

        const int I_TOTAL_ROWS = 5; 

        #endregion

        #region Data Members

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId;
        private StaffHolidaysSalaryDeduction moStaffHolidaysSalaryDeduction;
        private List<StaffHolidaysSalaryDeduction> moStaffHolidaysSalaryDeductions = new List<StaffHolidaysSalaryDeduction>();
        private List<DatewiseStaffLeave> moDatewiseStaffLeaves = new List<DatewiseStaffLeave>();
        private List<ConfiguredLeaves> moConfiguredLeaves = new List<ConfiguredLeaves>();
        private List<StaffBaseDetails> moStaffBaseDetails = new List<StaffBaseDetails>();        
        private List<StaffHolidayLeavesConfigTypes> mlstStaffHolidayLeavesConfigTypes = new List<StaffHolidayLeavesConfigTypes>();

        #endregion

        #region Constructor(s)

        public StaffHolidaysSalaryDeductionDC()
        {
        }

        public StaffHolidaysSalaryDeductionDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miUserId = aiUserId;
        } 

        #endregion

        #region Properties

        public StaffHolidaysSalaryDeduction StaffHolidaysSalaryDeductionConfig
        {
            get { return moStaffHolidaysSalaryDeduction; }
            set { moStaffHolidaysSalaryDeduction = value; }
        }

        public List<StaffHolidaysSalaryDeduction> StaffHolidaysSalaryDeductions
        {
            get { return moStaffHolidaysSalaryDeductions; }
            set { moStaffHolidaysSalaryDeductions = value; }
        }

        public List<DatewiseStaffLeave> DatewiseStaffLeaves
        {
            get { return moDatewiseStaffLeaves; }
            set { moDatewiseStaffLeaves = value; }
        }

        public List<ConfiguredLeaves> ConfiguredLeaves
        {
            get { return moConfiguredLeaves; }
            set { moConfiguredLeaves = value; }
        }

        public List<StaffBaseDetails> StaffBaseDetails
        {
            get { return moStaffBaseDetails; }
            set { moStaffBaseDetails = value; }
        }

        public List<StaffHolidayLeavesConfigTypes> StaffHolidayLeavesConfigTypes
        {
            get { return mlstStaffHolidayLeavesConfigTypes; }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// This method is used to save the staff holiday configuration details.
        /// </summary>
        public void Save(string asHolidayConfigXML)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {                
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);               
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AttachedLeaveConfigurationXML", asHolidayConfigXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[USP_InsertAttachedLeaveConfigurationDetails]");
            }
        }

        /// <summary>
        /// This method is used to save the staff holiday configuration details.
        /// </summary>
        public void SaveWeekendConfiguration(StaffHolidaysSalaryDeduction aoStaffHolidaysSalaryDeduction)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveType", aoStaffHolidaysSalaryDeduction.Type, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PercentageToDeduct", aoStaffHolidaysSalaryDeduction.PercentageToDeduct, SqlDbType.Decimal);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[usp_AddWeekendDetails]");
            }
        }

        /// <summary>
        /// This method is used to get the staff holiday configuration details.
        /// </summary>
        public void GetAll()
        {
            List<StaffHolidaysSalaryDeduction> oStaffHolidaysSalaryDeductionList = new List<StaffHolidaysSalaryDeduction>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetHolidaySalaryDeductionConfiguration"))
                {
                    FillHolidayConfigDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillDatewiseStaffLeaves(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillStaffLeaves(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        FillUserDetails(oSqlDataReader);
                    oSqlDataReader.NextResult();
                    FillStaffHolidayLeavesConfigTypes(oSqlDataReader);
                }
            }
        }

        private void FillStaffHolidayLeavesConfigTypes(SqlDataReader aoSqlDataReader)
        {
            StaffHolidayLeavesConfigTypes oStaffHolidayLeavesConfigTypes;
            while (aoSqlDataReader.Read())
            {
                oStaffHolidayLeavesConfigTypes = new StaffHolidayLeavesConfigTypes
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Type = Convert.ToString(aoSqlDataReader["Type"])
                };
                mlstStaffHolidayLeavesConfigTypes.Add(oStaffHolidayLeavesConfigTypes);
            }
        }

        /// <summary>
        /// This method is used to fill user details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillUserDetails(SqlDataReader aoSqlDataReader)
        {
            StaffBaseDetails oStaffBaseDetails;
            while (aoSqlDataReader.Read())
            {
                oStaffBaseDetails = new StaffBaseDetails
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"])
                };
                moStaffBaseDetails.Add(oStaffBaseDetails);
            }
        }

        /// <summary>
        /// This method is used to fill staff leaves.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillStaffLeaves(SqlDataReader aoSqlDataReader)
        {
            ConfiguredLeaves oConfiguredLeaves;
            while (aoSqlDataReader.Read())
            {
                oConfiguredLeaves = new ConfiguredLeaves
                {
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]),
                    ExcludeFromSalaryDeduction = Convert.ToBoolean(aoSqlDataReader["ExcludeFromDeduction"])
                };
                moConfiguredLeaves.Add(oConfiguredLeaves);
            }
        }

        /// <summary>
        /// This method is used to fill datewise staff leaves.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillDatewiseStaffLeaves(SqlDataReader aoSqlDataReader)
        {
            DatewiseStaffLeave oDatewiseStaffLeave;
            while (aoSqlDataReader.Read())
            {
                oDatewiseStaffLeave = new DatewiseStaffLeave
                {
                    DatewiseStaffLeavesId = Convert.ToInt32(aoSqlDataReader["DatewiseStaffLeavesId"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    Date = Convert.ToDateTime(aoSqlDataReader["Date"])
                };
                moDatewiseStaffLeaves.Add(oDatewiseStaffLeave);
            }
        }

        //This method  is used to assign values for class object.
        private void FillHolidayConfigDetails(SqlDataReader oSqlDataReader)
        {   
            StaffHolidaysSalaryDeduction StaffHolidaysSalaryDeductionConfig;
            while (oSqlDataReader.Read())
            {
                StaffHolidaysSalaryDeductionConfig = new StaffHolidaysSalaryDeduction
                {
                    StaffHolidaysSalaryDeductionId = Convert.ToInt32(oSqlDataReader["StaffHolidayLeavesConfiguratonId"]),
                    HolidayStartDate = Convert.ToDateTime(oSqlDataReader["HolidayStartDate"]),
                    HolidayEndDate = Convert.ToDateTime(oSqlDataReader["HolidayEndDate"]),
                    HolidayName = Convert.ToString(oSqlDataReader["HolidayName"]),
                    Days = Convert.ToInt32(oSqlDataReader["Days"]),
                    PercentageToDeduct = Convert.ToDecimal(oSqlDataReader["PercentageToDeduct"]),
                    Type = Convert.ToInt32(oSqlDataReader["Type"]),
                    IsWeekend = Convert.ToBoolean(oSqlDataReader["IsWeekend"])
                };
                moStaffHolidaysSalaryDeductions.Add(StaffHolidaysSalaryDeductionConfig);
            }
           
                StaffHolidaysSalaryDeduction oStaffHolidaysSalaryDeduction;
	        for (int iRowIndex = 0; iRowIndex < I_TOTAL_ROWS; iRowIndex++)
	        {
		        oStaffHolidaysSalaryDeduction = new StaffHolidaysSalaryDeduction
			        {
				        StaffHolidaysSalaryDeductionId = 0,
				        HolidayName = string.Empty,
				        HolidayStartDate = DateTime.Now.Date,
				        HolidayEndDate = DateTime.Now.Date,
				        Days = 1,
				        PercentageToDeduct = 0,
				        Type = 0
			        };
		        moStaffHolidaysSalaryDeductions.Add(oStaffHolidaysSalaryDeduction);
	        }
        }

        #endregion
    }
}
