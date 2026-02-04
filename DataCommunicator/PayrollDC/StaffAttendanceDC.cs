// Class Name       :- StaffAttendanceDC
// Purpose          :- This class is used to manage StaffAttendance details.
// Date Of creation :-22-March-2011
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;

namespace DataCommunicator
{
    public class StaffAttendanceDC
    {
        #region Data Members

        private StaffAttendance moStaffAttendance;
        private List<StaffAttendance> mlstStaffAttendances = new List<StaffAttendance>();
        private List<StaffLeaveDetails> mlstStaffLeaveDetails = new List<StaffLeaveDetails>();
        private List<StaffAttendance> mlstStaffAttendance = new List<StaffAttendance>();
        private List<ConfiguredLeaves> mlstConfiguredLeaves = new List<ConfiguredLeaves>();
        private bool mbIsSalaryPublished;

        #endregion

        #region Constructor

        public StaffAttendanceDC()
        {
        }

        #endregion

        #region Properties

        public StaffAttendance StaffAttendance
        {
            get
            {
                return moStaffAttendance;
            }
            set
            {
                moStaffAttendance = value;
            }
        }

        public List<StaffAttendance> StaffAttendances
        {
            get
            {
                return mlstStaffAttendances;
            }
            set
            {
                mlstStaffAttendances = value;
            }
        }

        public List<StaffLeaveDetails> StaffLeaveDetails
        {
            get { return mlstStaffLeaveDetails; }
            set { mlstStaffLeaveDetails = value; }
        }

        public List<StaffAttendance> StaffAttendanceDetails
        {
            get { return mlstStaffAttendance; }
            set { mlstStaffAttendance = value; }
        }

        public List<ConfiguredLeaves> ConfiguredLeaves
        {
            get { return mlstConfiguredLeaves; }
        }

        public bool IsSalaryPublished
        {
            get { return mbIsSalaryPublished; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to return all the users of selected staff group.
        /// </summary>
        /// <returns></returns>
        public void GetStaffGroupUsers()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moStaffAttendance.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moStaffAttendance.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", moStaffAttendance.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", moStaffAttendance.Year, SqlDbType.Int);
                if (moStaffAttendance.StaffGroupsId != 0)
                    oSQLServerDbUtility.AddParameter("StaffGroupId", moStaffAttendance.StaffGroupsId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStaffGroupUsers"))
                {
                    if (oSqlDataReader != null)
                    {
                        FillStaffAttendance(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillLeaveDetails(oSqlDataReader);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to fill leave details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private void FillLeaveDetails(SqlDataReader oSqlDataReader)
        {
            StaffLeaveDetails oStaffLeaveDetails;
            while (oSqlDataReader.Read())
            {
                oStaffLeaveDetails = new StaffLeaveDetails
                {
                    StaffAttendanceId = Convert.ToInt32(oSqlDataReader["StaffAttendanceId"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    Days = Convert.ToDecimal(oSqlDataReader["Days"]),
                    OriginalLeaveId = Convert.ToInt16(oSqlDataReader["OriginalLeaveId"]),
                };
                mlstStaffLeaveDetails.Add(oStaffLeaveDetails);
            }
        }

        /// <summary>
        /// This method is used to fill staff attendance.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        private void FillStaffAttendance(SqlDataReader oSqlDataReader)
        {
            StaffAttendance oStaffAttendance;
            while (oSqlDataReader.Read())
            {
                oStaffAttendance = new StaffAttendance
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    Name = Convert.ToString(oSqlDataReader["Name"]),
                    Designation = Convert.ToString(oSqlDataReader["Designation"]),
                    StaffAttendanceId = Convert.ToInt32(oSqlDataReader["StaffAttendanceId"])
                };
                mlstStaffAttendances.Add(oStaffAttendance);
            }
        }

        /// <summary>
        /// This method is used to save staff attendance.
        /// </summary>
        public void SaveStaffAttendance()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", moStaffAttendance.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", moStaffAttendance.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moStaffAttendance.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", moStaffAttendance.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", moStaffAttendance.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserIdsXML", moStaffAttendance.UserIdsXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStaffAttendance");
            }
        }

        public List<DaywiseStaffAttendance> GetAll(int aiSchoolId, int aiAcademicYearId, DateTime adtDate, int aiStaffGroupId, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", adtDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDatewiseAttendanceDetails"))
                {
                    List<DaywiseStaffAttendance> lstAttendance = GetAttendanceDetails(oSqlDataReader);
                    oSqlDataReader.NextResult();
                    LoadLeaveDetails(oSqlDataReader);
                    oSqlDataReader.NextResult();
                    LoadSupportingDetails(oSqlDataReader);
                    return lstAttendance;
                }
            }
        }

        public void SaveDaywiseLeaves(int aiSchoolId, int aiUpdatedById, DateTime adtDate, string asLeaveXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", adtDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("LeaveXml", asLeaveXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveDaywiseLeaves");
            }
        }

        #endregion

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill staff attendance entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetAttendance(SqlDataReader oSqlDataReader)
        {
            StaffAttendance oStaffAttendanceDC;
            while (oSqlDataReader.Read())
            {
                oStaffAttendanceDC = new StaffAttendance
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    StaffAttendanceId = Convert.ToInt32(oSqlDataReader["StaffAttendanceId"]),
                    PresentDays = Convert.ToDecimal(oSqlDataReader["PresentDays"])
                };
                mlstStaffAttendance.Add(oStaffAttendanceDC);
            }
        }

        /// <summary>
        /// This method is used to return staff attendance details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<DaywiseStaffAttendance> GetAttendanceDetails(SqlDataReader aoSqlDataReader)
        {
            List<DaywiseStaffAttendance> lstAttendances = new List<DaywiseStaffAttendance>();
            while (aoSqlDataReader.Read())
            {
                lstAttendances.Add(new DaywiseStaffAttendance
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    Designation = Convert.ToString(aoSqlDataReader["Designation"]),
                    IsHalfLeave = Convert.ToBoolean(aoSqlDataReader["IsHalfLeave"]),
                    IsLateMark = Convert.ToBoolean(aoSqlDataReader["IsLateMark"]),
                    JoiningDate = (aoSqlDataReader["JoiningDate"] != DBNull.Value ? Convert.ToDateTime(aoSqlDataReader["JoiningDate"]) : DateTime.MinValue),
                    ResignationDate = (aoSqlDataReader["ResignationDate"] != DBNull.Value ? Convert.ToDateTime(aoSqlDataReader["ResignationDate"]) : DateTime.MinValue),
                    SrNo = Convert.ToInt32(aoSqlDataReader["SrNo"]),
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                    MobileNo=Convert.ToString(aoSqlDataReader["MobileNo"]),
                    PartialLeaveId = Convert.ToInt32(aoSqlDataReader["PartialLeaveId"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    LeaveBalance = Convert.ToString(aoSqlDataReader["LeaveBalance"]),
                    LeaveDetails = Convert.ToString(aoSqlDataReader["LeaveDetails"])
                });
            }

            return lstAttendances;
        }

        /// <summary>
        /// This method is used to load supporting details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadSupportingDetails(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
                mbIsSalaryPublished = Convert.ToBoolean(aoSqlDataReader["IsSalaryPublished"]);
        }

        /// <summary>
        /// This method is sued to load leave details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void LoadLeaveDetails(SqlDataReader aoSqlDataReader)
        {
            while (aoSqlDataReader.Read())
                mlstConfiguredLeaves.Add(new ConfiguredLeaves 
                { 
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]), 
                    ShortName = Convert.ToString(aoSqlDataReader["ShortName"]), 
                    ColorCode = Convert.ToString(aoSqlDataReader["ColorCode"]),
                    AllowZeroBalance = Convert.ToBoolean(aoSqlDataReader["AllowZeroBalance"]),
                });
        }

        #endregion
    }
}
