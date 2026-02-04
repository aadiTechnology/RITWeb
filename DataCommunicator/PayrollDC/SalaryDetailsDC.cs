// Class Name       :- SalaryDetailsDC
// Purpose          :- This class is used to manage SalaryDetails details.
// Date Of creation :- 11/18/2009
// Creator Name      :- Sachin


using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Utility;
using PayrollEntities;
using SchoolEntities.Dashboard;
using System.Configuration;

namespace DataCommunicator
{
    public class SalaryDetailsDC
    {
        #region Data Members

        public SalaryDetails SalaryDetails { get; set; }
        public SalaryEntityList moSalaryEntityLists = new SalaryEntityList();
        public SalaryCommonUtility oSalaryMonthAndYear = null;
        public MonthAndYear oMonthAndYear = new MonthAndYear();
        public List<SalaryMonth> Months = new List<SalaryMonth>();
        public List<SalaryYear> Years = new List<SalaryYear>();

        public List<PaidSalaryDetails> moUserSalaryDetails = new List<PaidSalaryDetails>();
        public List<PaidSalaryDetails> moPaidSalaryDetails = new List<PaidSalaryDetails>();
        public List<StaffAttendance> moStaffAttendanceList = new List<StaffAttendance>();
        public List<StaffLeaveDetails> moStaffLeaveDetailsList = new List<StaffLeaveDetails>();

        public BasicDetails moBasicDetails = new BasicDetails();

        public int iMinUserStaffGroupId = 0;
    
        #endregion

        int miSchoolId;
        int miAcademicYearId;
        List<PaidSalaryDifference> mlstPaidSalaryDifferences;
        List<PaidSalaryDifference> mlstSalaryDifferencePaidDetails;

        #region Constructor

        public SalaryDetailsDC()
        {
        }

        public SalaryDetailsDC(int iSchoolId, int iAcademicYearId)
        {   
            miSchoolId = iSchoolId;
            miAcademicYearId = iAcademicYearId;
        } 

        #endregion

        #region UserId

        public SalaryEntityList SalaryEntityLists
        {
            get
            {
                return moSalaryEntityLists;
            }
            set
            {
                moSalaryEntityLists = value;
            }
        }

        public List<PaidSalaryDetails> UserSalaryDetails
        {
            get{ return moUserSalaryDetails; }
        }

        public List<PaidSalaryDetails> PaidSalaryDetails
        {
            get { return moPaidSalaryDetails; }
        }

        public List<PaidSalaryDifference> PaidSalaryDifferences
        {
            get { return mlstPaidSalaryDifferences; }
        }

        public List<PaidSalaryDifference> SalaryDifferencePaidDetails
        {
            get { return mlstSalaryDifferencePaidDetails; }
        }

        #endregion

        #region Methods
        // This function is used to insert the SalaryDetails Details
        public void Insert()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", SalaryDetails.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", SalaryDetails.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", SalaryDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", SalaryDetails.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupsId", SalaryDetails.StaffGroupsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", SalaryDetails.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SalaryDetailsXml", SalaryDetails.SalaryDetailsXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ChequeNo", SalaryDetails.ChequeNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("ChequeDate", SalaryDetails.ChequeDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ChequeAmount", SalaryDetails.ChequeAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("LeaveTransferMonthId", SalaryDetails.LeaveTransferMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPreviewDisplayed", SalaryDetails.IsPreviewDisplayed, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("SalaryDifferenceXml", SalaryDetails.SalayDifferenceXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolWiseBankAccountDetailsId", SalaryDetails.SchoolWiseBankAccountDetailsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsOnlineTransaction", SalaryDetails.IsOnlineTransaction, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_InsertSalaryDetails"))
                {
                    if (oSqlDataReader != null)
                    {
                        FillPaidSalaryDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillSalaryDetails(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillStaffAttendacne(oSqlDataReader);
                        if (oSqlDataReader.NextResult())
                            FillStafLeaveDetails(oSqlDataReader);
                    }
                }
            }
        }

        private void FillStafLeaveDetails(SqlDataReader oSqlDataReader)
        {
            StaffLeaveDetails oStaffLeaveDetails;
            while (oSqlDataReader.Read())
            {
                oStaffLeaveDetails = new StaffLeaveDetails
                {
                    StaffAttendanceId = Convert.ToInt32(oSqlDataReader["StaffAttendanceId"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    Days = Convert.ToDecimal(oSqlDataReader["Days"])
                };
                moStaffLeaveDetailsList.Add(oStaffLeaveDetails);
            }
        }

        private void FillStaffAttendacne(SqlDataReader oSqlDataReader)
        {
            StaffAttendance oStaffAttendance;
            while (oSqlDataReader.Read())
            {
                oStaffAttendance = new StaffAttendance
                {
                    StaffAttendanceId = Convert.ToInt32(oSqlDataReader["StaffAttendanceId"]),
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    PresentDays = Convert.ToDecimal(oSqlDataReader["PresentDays"])
                };
                moStaffAttendanceList.Add(oStaffAttendance);
            }
        }

        private void FillSalaryDetails(SqlDataReader oSqlDataReader)
        {
            PaidSalaryDetails oPaidSalaryDetails;
            while (oSqlDataReader.Read())
            {
                oPaidSalaryDetails = new PaidSalaryDetails
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    EarnDeductName = Convert.ToString(oSqlDataReader["EarnDeductName"]),
                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"])
                };
                moPaidSalaryDetails.Add(oPaidSalaryDetails);
            }
        }

        private void FillPaidSalaryDetails(SqlDataReader oSqlDataReader)
        {
            PaidSalaryDetails oUserSalaryDetails;
            while (oSqlDataReader.Read())
            {
                oUserSalaryDetails = new PaidSalaryDetails
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    Name = Convert.ToString(oSqlDataReader["Name"]),
                    MobileNo = Convert.ToString(oSqlDataReader["MobileNo"]),
                    AdminId = Convert.ToInt32(oSqlDataReader["AdminId"]),
                    NetSalary = Convert.ToDecimal(oSqlDataReader["NetSalary"]),
                    Month = Convert.ToString(oSqlDataReader["Month"]),
                    Year = Convert.ToInt32(oSqlDataReader["Year"])
                };
                moUserSalaryDetails.Add(oUserSalaryDetails);
            }
        }

        public static void DeleteSalary(int aiSchoolId, int aiAcademicYearId, int aiMonthid, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthid, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteSalaryDetails");
            }
        }

        public void InsertIndividualDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", SalaryDetails.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", SalaryDetails.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", SalaryDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", SalaryDetails.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", SalaryDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", SalaryDetails.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IndividualXml", SalaryDetails.IndividualXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ReturnModifiedData", true, SqlDbType.Bit);
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_InsertIndivudualsSalaryDetails"))
                LoadModifiedSalaryDetails(oSqlDataReader);
            }

        }

        /// <summary>
        /// This method is used to save salary details of all users.
        /// </summary>
        public void Save()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", SalaryDetails.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", SalaryDetails.AcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", SalaryDetails.MonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", SalaryDetails.Year, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", SalaryDetails.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SalaryDetailsXml", SalaryDetails.SalaryDetailsXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ReturnModifiedData", true, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertUsersSalaryDetails");                
            }
        }

        private void LoadModifiedSalaryDetails(SqlDataReader oSqlDataReader)
        {
            if (oSqlDataReader != null)
            {
                LoadAttendance(oSqlDataReader);

                oSqlDataReader.NextResult();
                LoadStaffLeavesDetails(oSqlDataReader);

                oSqlDataReader.NextResult();
                LoadUsersED(oSqlDataReader);

                oSqlDataReader.NextResult();
                LoadUsersLeaves(oSqlDataReader);

                LoadLateMarkDetails(oSqlDataReader);
            }
        }

        public static DataSet GetSalaryMonthAndYear(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("[usp_GetSalaryMonthAndYear]");
            }
        }

        public static void Unpublish(int aiSchoolId, int aiAcademicYearId, int aiMonthId, int aiYear, int aiInsertedById, int aiLeaveTransferMonth)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveTransferMonth", aiLeaveTransferMonth, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UnpublishSalary");
            }
        }

        public void GetStaffGroupsAndMonths(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetBasicDetails]"))
                {
                    LoadStaffGroups(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetSalaryMonths(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetSalaryYears(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                    {
                        if (oSqlDataReader.Read())
                            iMinUserStaffGroupId = Convert.ToInt16(oSqlDataReader["StaffGroupsId"]);
                    }
                    if (oSqlDataReader.NextResult())
                        LoadMonthAndYear(oSqlDataReader);
                }
            }
        }

        /// <summary>
        /// This method is used to return all user details For in Out as per date.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcadmicYearId"></param>
        /// <param name="dtFromDate"></param>
        /// <param name="dtToDate"></param>
        /// <param name="aiUserId"></param>
        /// <param name="lstEmployeeNoDetails"></param>
        /// <returns></returns>
        public List<StaffInOutDetails> GetUserInOutDetails(int aiSchoolId, int aiAcademicYearId, string aiUserId, int aiUserStaffGroupId, DateTime dtFromDate, DateTime dtToDate, out List<EmployeeNoDetails> lstEmployeeNoDetails)
        {
            string sEmployeeNos = string.Empty;
            lstEmployeeNoDetails = new List<EmployeeNoDetails>();
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiUserStaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EmployeeNo", aiUserId, SqlDbType.NVarChar);

                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllEmployeeNosForStaffInOutDetails");
                while (oSqlDataReader.Read())
                {
                    EmployeeNoDetails oEmployeeNoDetails = new EmployeeNoDetails();
                    oEmployeeNoDetails.EmployeeNo = Convert.ToString(oSqlDataReader["EmployeeNo"]);
                    oEmployeeNoDetails.SrNo = Convert.ToInt32(oSqlDataReader["SrNo"]);
                    sEmployeeNos = sEmployeeNos + ",'" + Convert.ToString(oSqlDataReader["EmployeeNo"]) + "'";

                    lstEmployeeNoDetails.Add(oEmployeeNoDetails);
                }
            }
            if (sEmployeeNos.StartsWith(","))
                sEmployeeNos = sEmployeeNos.Substring(1);

            List<StaffInOutDetails> alstStaffInOutDetails = new List<StaffInOutDetails>();
            string connectionString = "Data Source= " + ConfigurationManager.AppSettings["SchoolLocationsDataSource"] + "; Database=" + ConfigurationManager.AppSettings["SchoolLocationsDataBaseName"]
                            + "; User ID=" + ConfigurationManager.AppSettings["SchoolLocationsUserId"] + "; Password=" + ConfigurationManager.AppSettings["SchoolLocationsPassword"];

            using (SqlConnection oSqlConnection = new SqlConnection(connectionString))
            {
                string command = string.Empty;
                if (aiSchoolId != 0)                
                    command = "SELECT * FROM BiometricSchools WHERE SchoolID = " + aiSchoolId + " AND IsDeleted = 0 AND BiometricDatabaseName != '' AND BiometricDatabaseServer != '' AND BiometricUserID != ''";
                

                SqlCommand oSqlCommand = new SqlCommand(command, oSqlConnection);
                oSqlConnection.Open();

                string sConnectionString = string.Empty;
                SqlDataReader oSqlDataReader = oSqlCommand.ExecuteReader();

                if (oSqlDataReader.Read())
                {
                    string sUserName = oSqlDataReader["BiometricUserID"].ToString();
                    string sPassword = oSqlDataReader["BiometricPassword"].ToString();
                    string dbPassword = CommonUtility.GetDecryptedPassword(sUserName, sPassword);

                    sConnectionString = "Data Source= " + oSqlDataReader["BiometricDatabaseServer"].ToString() + "; Database=" + oSqlDataReader["BiometricDatabaseName"].ToString() + "; User ID=" + sUserName + "; Password=" + dbPassword;
                }

                if (sConnectionString != string.Empty)
                {
                    string sEntitiesDetails;
                    if (aiSchoolId == Constants.SchoolId.DPIS.ToInt() || aiSchoolId == Constants.SchoolId.VPMCPS.ToInt() || aiSchoolId == Constants.SchoolId.CKInstOfCulinaryArtAndHotelMgmt.ToInt())
                    {
                        sEntitiesDetails = "SELECT " + aiSchoolId + " AS SchoolId, [EMPCODE] as Employee_No,[DATEANDTIME] as InDateTime,'' as UserName" +
                                              " FROM [dbo].[UserPunchingDetails]" +
                                              " WHERE [EMPCODE] IN (" + sEmployeeNos + ") AND CONVERT(DATE,[DATEANDTIME]) >= Convert(DATE,'" + dtFromDate.ToString(Constants.S_DATE_FORMAT).ToDateTime() + "') AND CONVERT(DATE,[DATEANDTIME]) <= CONVERT(DATE,'" + dtToDate.ToString(Constants.S_DATE_FORMAT).ToDateTime() + "')" +
                                              " ORDER BY CONVERT(DATE,DATEANDTIME) ASC";
                    }
                    else if (aiSchoolId == Constants.SchoolId.PIONEER.ToInt() || aiSchoolId == Constants.SchoolId.SNS.ToInt())
                    {
                        sEntitiesDetails = "SELECT " + aiSchoolId + " AS SchoolId, [INTEGRATION_REFERENCE] as Employee_No,[EventDateTime_D] as InDateTime,'' as UserName" +
                                              " FROM [dbo].[Mx_VEW_APIUserAccessCtrlEvts]" +
                                              " WHERE [INTEGRATION_REFERENCE] IN (" + sEmployeeNos + ") AND CONVERT(DATE,[EventDateTime_D]) >= Convert(DATE,'" + dtFromDate.ToString(Constants.S_DATE_FORMAT).ToDateTime() + "') AND CONVERT(DATE,[EventDateTime_D]) <= CONVERT(DATE,'" + dtToDate.ToString(Constants.S_DATE_FORMAT).ToDateTime() + "')" +
                                              " ORDER BY CONVERT(DATE,EventDateTime_D) ASC";
                    }
                    else
                    {
                        sEntitiesDetails = "SELECT " + aiSchoolId + " AS SchoolId " +
                                                    ",dbo.Mx_UserMst.IntegrationRef AS Employee_No " +
                                                    ",dbo.Mx_UserMst.Name AS UserName " +
                                                   ",dbo.Mx_ACSEventTrn.Edatetime AS InDateTime" +
                                                    " FROM Mx_UserMst " +
                                                " INNER JOIN Mx_ACSEventTrn ON Mx_ACSEventTrn.UserID = Mx_UserMst.UserID"
                                                    + " WHERE (Mx_UserMst.IntegrationRef IN(" + sEmployeeNos + "))" +
                                                     " AND (CONVERT(DATE,Mx_ACSEventTrn.Edatetime) >= " + "Convert(DATE" + ",'" + dtFromDate.ToString(Constants.S_DATE_FORMAT).ToDateTime() + "'))" +
                                                     " AND (CONVERT(DATE,Mx_ACSEventTrn.Edatetime) <= " + "Convert(DATE" + ",'" + dtToDate.ToString(Constants.S_DATE_FORMAT).ToDateTime() + "'))" +
                                                     " AND (Mx_UserMst.IntegrationRef != NULL OR Mx_UserMst.IntegrationRef != '')" +
                                                     "ORDER BY CONVERT(DATE,dbo.Mx_ACSEventTrn.Edatetime) ASC";
                    }

                    oSqlConnection.Close();
                    SqlConnection aoSqlConnection = new SqlConnection(sConnectionString);
                    SqlCommand aoSqlCommand = new SqlCommand(sEntitiesDetails, aoSqlConnection);                    
                    aoSqlConnection.Open();

                    SqlDataReader aoSqlDataReader = aoSqlCommand.ExecuteReader();                

                    while (aoSqlDataReader.Read())
                    {
                        alstStaffInOutDetails.Add(new StaffInOutDetails
                        {
                            EmployeeNo = aoSqlDataReader["Employee_No"] != null ? aoSqlDataReader["Employee_No"].ToString() : "",
                            UserName = aoSqlDataReader["UserName"] != null ? aoSqlDataReader["UserName"].ToString() : "",
                            InDateTime = aoSqlDataReader["InDateTime"].ToDateTime()
                        });
                    }
                    aoSqlConnection.Close();
                }               
            }

            if (aiSchoolId == Constants.SchoolId.DPIS.ToInt() || aiSchoolId == Constants.SchoolId.VPMCPS.ToInt() || aiSchoolId == Constants.SchoolId.PIONEER.ToInt() || aiSchoolId == Constants.SchoolId.SNS.ToInt())
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    string sCommand = "SELECT UBD.EmployeeNo, APU.UserName " +
                                      " FROM vw_AllPayrollUsers APU INNER JOIN UserBasicDetails UBD ON APU.UserId = UBD.UserId" +
                                      " WHERE UBD.EmployeeNo IS NOT NULL AND UBD.EmployeeNo <> ''";

                    DataTable DT = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sCommand);

                    if (DT.Rows.Count > 0)
                    {
                        foreach (StaffInOutDetails oItem in alstStaffInOutDetails)
                        {
                            DataRow[] dr = DT.Select("EmployeeNo='" + oItem.EmployeeNo+"'");
                            if (dr.Length > 0)
                            {
                                oItem.UserName = dr[0]["UserName"].ToString();
                            }
                        }
                    }
                }
            }


            return alstStaffInOutDetails;
        }

        public static int GetStaffGroupId(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiMonthId, int aiYear)
        {
            string sSelectStatement = "SELECT " +
                                "StaffGroupsId" +
                                " FROM " +
                                "UsersStaffGroupsAssoHistory" +
                                " INNER JOIN " +
                                "(" +
                                "SELECT " +
                                "UsersStaffGroupsAssociationId," +
                                "MONTH(MAX(CONVERT(DATETIME,CAST(Year AS NVARCHAR(4))+'-'+ CAST(MonthId AS NVARCHAR(2))+'-1'))) as MonthId," +
                                "YEAR(MAX(CONVERT(DATETIME,CAST(Year AS NVARCHAR(4))+'-'+ CAST(MonthId AS NVARCHAR(2))+'-1'))) AS Year" +
                                " FROM " +
                                "UsersStaffGroupsAssoHistory" +
                                " WHERE " +
                                " CONVERT(DATETIME,CAST(Year AS NVARCHAR(4))+'-'+ CAST(MonthId AS NVARCHAR(2))+'-1') " +
                                " <= CONVERT(DATE,CAST(" + @aiYear + " AS NVARCHAR(4))+'-'+ CAST(" + aiMonthId + " AS NVARCHAR(2))+'-1')" +
                                " AND SchoolId = " + aiSchoolId +
                                " AND UserId = " + aiUserId +
                                " AND Is_Deleted = N'N'" +
                                " GROUP BY UsersStaffGroupsAssociationId" +
                                ")S" +
                                " ON  UsersStaffGroupsAssoHistory.UsersStaffGroupsAssociationId = S.UsersStaffGroupsAssociationId" +
                                " AND UsersStaffGroupsAssoHistory.MonthId = S.MonthId" +
                                " AND UsersStaffGroupsAssoHistory.Year = S.Year" +
                                " WHERE Is_Deleted = N'N'";

            using (SQLServerDbUtility OSQLServerDbUtility = new SQLServerDbUtility())
                return OSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }

        public static DataSet GetUsersStaffGroupDetais(int aiSchoolId, int aiAcademicYearId, int aiUserId, string asStartDate, string asEndDate, int aiFinancialYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);

                if(!string.IsNullOrEmpty(asStartDate))
                    oSQLServerDbUtility.AddParameter("StartDate", asStartDate, SqlDbType.Date);

                if (!string.IsNullOrEmpty(asEndDate))
                    oSQLServerDbUtility.AddParameter("EndDate", asEndDate, SqlDbType.Date);

                if (aiFinancialYearId != 0)
                    oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetUsersStaffGroupDetais");
            }
        }

        public static DataTable GetUserDetails(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupsId", aiStaffGroupId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUsersDetails");
            }
        }

        public void GetLeavesAndUsers(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId, int aiMonthId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                if (aiStaffGroupId != 0)
                    oSQLServerDbUtility.AddParameter("StaffGroupsId", aiStaffGroupId, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("CurrentMonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CurrentYear", aiYear, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLeavesAndUsers"))
                {
                    FillUserDetails(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        LoadStaffLeaves(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        SetSalaryMonthAndYear(oSqlDataReader);
                    if (oSqlDataReader.NextResult())
                        LoadStaffGroups(oSqlDataReader);
                }
            }
        }
        private void SetSalaryMonths(SqlDataReader aoSqlDataReader)
        {
            SalaryMonth oMonth = null;
            while(aoSqlDataReader.Read())
            {
                oMonth = new SalaryMonth
                {
                    MonthId = Convert.ToInt32(aoSqlDataReader["MonthId"]),
                    Month = Convert.ToString(aoSqlDataReader["Month"])
                };
                Months.Add(oMonth);
            }
        }

        private void SetSalaryYears(SqlDataReader aoSqlDataReader)
        {
            SalaryYear oYear = null;
            while(aoSqlDataReader.Read())
            {
                oYear = new SalaryYear
                {
                    Year = Convert.ToInt32(aoSqlDataReader["Year"])
                };
                Years.Add(oYear);
            }
        }

        private void SetSalaryMonthAndYear(SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                oSalaryMonthAndYear = new SalaryCommonUtility
                {
                    MonthId = Convert.ToInt32(aoSqlDataReader["MonthId"]),
                    Year = Convert.ToInt32(aoSqlDataReader["Year"])
                };
            }
        }

        private void FillUserDetails(SqlDataReader aoSqlDataReader)
        {
            SalaryCommonUtility oSalaryCommonUtility = null;
            while (aoSqlDataReader.Read())
            {
                oSalaryCommonUtility = new SalaryCommonUtility
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"])
                };
                moSalaryEntityLists.lstSalaryCommonUtility.Add(oSalaryCommonUtility);
            }
        }
        public static DataSet GetSalaryDetailTables(int aiSchoolId, int aiAcademicYearId, int aiMonthId, int aiYear, int aiStaffGroupsId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                if (aiStaffGroupsId != 0)
                    oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupsId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetSalaryDetailTables");
            }
        }

        /// <summary>
        /// This method is used to return salary payment details.
        /// </summary>
        /// <returns></returns>
        public List<SalaryPaymentDetails> GetAllPaymentDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSalaryPaymentDetails"))
                {
                    List<SalaryPaymentDetails> lstPaymentDetails = new List<SalaryPaymentDetails>();
                    SalaryPaymentDetails oSalaryPaymentDetails = null;
                    while (oSqlDataReader.Read())
                    {
                        lstPaymentDetails.Add
                            (
                                oSalaryPaymentDetails = new SalaryPaymentDetails
                                {
                                    MonthId = Convert.ToInt32(oSqlDataReader["MonthId"]),
                                    IsOnlineTransaction = Convert.ToBoolean(oSqlDataReader["IsOnlineTransaction"]),
                                    Year = Convert.ToInt32(oSqlDataReader["Year"]),
                                    TransactionNumber = Convert.ToString(oSqlDataReader["TransactionNumber"]),
                                    Month = Convert.ToString(oSqlDataReader["Month"]),
                                    IsLastRecord = Convert.ToBoolean(oSqlDataReader["IsLastRecord"]),
                                    IsOnlineTransactionText = (Convert.ToBoolean(oSqlDataReader["IsOnlineTransaction"]) ? "Yes" : "No")
                                }
                            );
                    }
                    return lstPaymentDetails;
                }
            }
        }
        
        /// <summary>
        /// this method is used to get Payroll related details.
        /// </summary>
        /// <returns></returns>
        public static PayrollSummary GetPayrollSummary(int aiSchoolId, int aiYear, int aiFinancialYearId, int aiMonth, bool abIsServiceCall = false)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, Constants.I_ZERO, Constants.I_ZERO, abIsServiceCall))
            {
                PayrollSummary oPayrollSummary = new PayrollSummary();
                string strAmountFormat = "{0:#,###,###.##}";// format amount with comma.

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonth, SqlDbType.Int);


                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPayrollSummary"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oPayrollSummary = new PayrollSummary()
                        {
                            PreviousMonthPaidSalary = oSqlDataReader["PreviousMonthPaidSalary"].ToString() != Constants.S_ZERO ? String.Format(strAmountFormat, Convert.ToDouble(oSqlDataReader["PreviousMonthPaidSalary"])) : Constants.S_ZERO,
                            IncomeTaxAmount = oSqlDataReader["IncomeTaxAmount"].ToString() != Constants.S_ZERO ? String.Format(strAmountFormat, Convert.ToDouble(oSqlDataReader["IncomeTaxAmount"])) : Constants.S_ZERO,
                            MonthWiseSalaryAmount = GetMonthWiseSalary(oSqlDataReader)
                        };
                    }
                }

                oPayrollSummary.MaxPaidSalaryAmount = oPayrollSummary.MonthWiseSalaryAmount.Max();
                return oPayrollSummary;

            }
        }   

		/// <summary>
        /// this method is used to get month wise salary details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private static double[] GetMonthWiseSalary(SqlDataReader oSqlDataReader)
        {
            int iFieldCount = oSqlDataReader.FieldCount;
            double[] arraySalaryAmount = new double[iFieldCount];

            arraySalaryAmount[0] = Convert.ToDouble(oSqlDataReader["PreviousMonth2PaidSalary"]);
            arraySalaryAmount[1] = Convert.ToDouble(oSqlDataReader["PreviousMonth1PaidSalary"]);
            arraySalaryAmount[2] = Convert.ToDouble(oSqlDataReader["PreviousMonthPaidSalary"]);

            return arraySalaryAmount;
        }

        #endregion

        #region Salary Detail Tables

        public void GetSalaryTables(int aiMonthId, int aiYear, int aiStaffGroupsId)
        {
            moSalaryEntityLists.lstUsersDetails.Clear();
            moSalaryEntityLists.lstStaffGroups.Clear();
            moSalaryEntityLists.lstEarningsDeductions.Clear();

            moSalaryEntityLists.lstEarningsDeductionsFormulae.Clear();
            moSalaryEntityLists.lstAmountRange.Clear();
            moSalaryEntityLists.lstMonthwiseAmount.Clear();

            moSalaryEntityLists.lstConfiguredLeaves.Clear();
            moSalaryEntityLists.lstStaffAttendance.Clear();
            moSalaryEntityLists.lstStaffLeaveDetails.Clear();

            moSalaryEntityLists.lstUsersSGAssociation.Clear();
            moSalaryEntityLists.lstUsersEarningsDeduction.Clear();
            moSalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.Clear();
            moSalaryEntityLists.lstUserLeaveConfiguration.Clear();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                if (aiStaffGroupsId != 0)
                    oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupsId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSalaryDetailTables"))
                {
                    oSqlDataReader.Read();
                    moBasicDetails.IsStaticData = Convert.ToBoolean(oSqlDataReader["StaticOutput"]);
                    moBasicDetails.UnpaidLeave = oSqlDataReader["UnpaidLeave"].ToString();
                    moBasicDetails.DisplaySalaryDifference = Convert.ToBoolean(oSqlDataReader["DisplaySalaryDifference"]);
                    moBasicDetails.IsLeaveIntervalMonth = Convert.ToChar(oSqlDataReader["IsLeaveIntervalMonth"]);

                    oSqlDataReader.NextResult();

                    if (moBasicDetails.IsStaticData == true)
                    {
                        LoadSalaryDetails(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadUnpublishStatus(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            LoadSalaryDiference(oSqlDataReader);
                    }
                    else
                    {
                        LoadUsersDetails(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadStaffGroups(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadEarningsDeductions(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadEarningsDeductionsFormula(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadAmountRanges(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadMonthwiseAmounts(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadLeaves(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadAttendance(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadStaffLeavesDetails(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadUsersSGAssociation(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadUsersED(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadUsersLeaves(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadSGEDAssociation(oSqlDataReader);

                        oSqlDataReader.NextResult();
                        LoadUsersFormulaAndRanges(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            LoadMonthAndYear(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            LoadLateMarkDetails(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            LoadLateMarkConfig(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            LoadStaffHolidayConfiguration(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            LoadSalaryDiference(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            LoadUserJoiningDate(oSqlDataReader);

                        if (oSqlDataReader.NextResult())
                            LoadUserSpecialCases(oSqlDataReader);
                    }
                }
            }
        }

        private void LoadUserSpecialCases(SqlDataReader aoSqlDataReader)
        {
            UserSpecialCaseDetails oStaffBaseDetails = null;
            while (aoSqlDataReader.Read())
            {
                oStaffBaseDetails = new UserSpecialCaseDetails
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    CaseName = Convert.ToString(aoSqlDataReader["Name"]),
                    EarningDeductionName = Convert.ToString(aoSqlDataReader["EarningDeductionName"]),
                    MultiplierValue = Convert.ToDecimal(aoSqlDataReader["MultiplierValue"])
                };
                moSalaryEntityLists.lstUserSpecialCaseDetails.Add(oStaffBaseDetails);
            }
        }

        private void LoadUserJoiningDate(SqlDataReader aoSqlDataReader)
        {
            StaffBaseDetails oStaffBaseDetails = null;
            while (aoSqlDataReader.Read())
            {
                oStaffBaseDetails = new StaffBaseDetails
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    JoiningDate = aoSqlDataReader["DateOfJoining"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(aoSqlDataReader["DateOfJoining"]),
                    ResignDate = aoSqlDataReader["DateOfResign"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(aoSqlDataReader["DateOfResign"])
                };
                moSalaryEntityLists.lstStaffBaseDetails.Add(oStaffBaseDetails);
            }
        }

        private void LoadStaffHolidayConfiguration(SqlDataReader aoSqlDataReader)
        {
            UsersSalaryDeduction oUsersSalaryDeduction = null;
            while (aoSqlDataReader.Read())
            {
                oUsersSalaryDeduction = new UsersSalaryDeduction
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    Days = Convert.ToInt32(aoSqlDataReader["Days"]),
                    PercentageToDeduct = Convert.ToDecimal(aoSqlDataReader["PercentageToDeduct"]),
                };
                moSalaryEntityLists.lstUsersSalaryDeductions.Add(oUsersSalaryDeduction);
            }
        }
        private void LoadLateMarkConfig(SqlDataReader aoSqlDataReader)
        {
            LateMarkConfiguration oLateMarkConfiguration = null;
            while(aoSqlDataReader.Read())
            {
                oLateMarkConfiguration = new LateMarkConfiguration
                {
                    LateMarkCount = Convert.ToInt32(aoSqlDataReader["LateMarkCount"]),
                    SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                    ConsideredLeaves = Convert.ToDecimal(aoSqlDataReader["ConsideredLeaves"]),
                };
                moSalaryEntityLists.lstLateMarkConfigurations.Add(oLateMarkConfiguration);
            }
        }

        private void LoadLateMarkDetails(SqlDataReader aoSqlDataReader)
        {
            UserLateMarkLeave oUserLateMark;
            while (aoSqlDataReader.Read())
            {
                oUserLateMark = new UserLateMarkLeave
                {
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    LeaveId = Convert.ToInt32(aoSqlDataReader["LeaveId"]),
                    Days = Convert.ToDecimal(aoSqlDataReader["Days"]),
                    IsUnPaidLeave = Convert.ToBoolean(aoSqlDataReader["IsUnPaidLeave"])
                };
                moSalaryEntityLists.lstUserLateMarkLeaves.Add(oUserLateMark);
            }
        }

        private void SetSalaryDiferenceMonths(SqlDataReader oSqlDataReader)
        {
            string sMonth = string.Empty;
            while (oSqlDataReader.Read())
            {
                sMonth = Convert.ToString(oSqlDataReader["SalaryDifferenceMonths"]);
                moSalaryEntityLists.lstSalaryDifferenceMonths.Add(sMonth);
            }
        }

        private void LoadSalaryDiference(SqlDataReader oSqlDataReader)
        {
            SalaryDifference oSalaryDifference;
            while (oSqlDataReader.Read())
            {
                oSalaryDifference = new SalaryDifference
                {  
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    GrossSalary = Convert.ToDecimal(oSqlDataReader["GrossSalary"]),
                    ProvidentFund = Convert.ToDecimal(oSqlDataReader["ProvidentFund"]),
                    MonthId = Convert.ToInt32(oSqlDataReader["MonthId"]),
                    Year = Convert.ToInt32(oSqlDataReader["Year"])
                };
                moSalaryEntityLists.lstSalaryDifference.Add(oSalaryDifference);
            }
        }

        private void LoadUsersFormulaAndRanges(SqlDataReader oSqlDataReader)
        {
            UsersFormulaAndRanges oUsersFormulaAndRanges;
            while (oSqlDataReader.Read())
            {
                oUsersFormulaAndRanges = new UsersFormulaAndRanges
                {
                    FormulaRangeId = Convert.ToInt32(oSqlDataReader["FormulaRangeId"]),
                    IsFormula = Convert.ToBoolean(oSqlDataReader["IsFormula"]),
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"])
                };
                moSalaryEntityLists.lstUsersFormulaAndRanges.Add(oUsersFormulaAndRanges);
            }
        }

        private void LoadMonthAndYear(SqlDataReader oSqlDataReader)
        {
            if (oSqlDataReader.Read())
            {
                oMonthAndYear = new MonthAndYear
                {
                    MonthId = Convert.ToInt32(oSqlDataReader["MonthId"]),
                    Year = Convert.ToInt32(oSqlDataReader["Year"])
                };
            }
        }

        private void LoadSalaryDetails(SqlDataReader oSqlDataReader)
        {
            StaticSalaryDetails oStaticSalaryDetails;
            while (oSqlDataReader.Read())
            {
                oStaticSalaryDetails = new StaticSalaryDetails
                {
                    SalaryDetailsXml = Convert.ToString(oSqlDataReader["SalaryDetailsXml"]),
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    Name = Convert.ToString(oSqlDataReader["Name"]),
                    StaffGroupId = Convert.ToInt32(oSqlDataReader["StaffGroupId"]),
                    Is_Deleted = oSqlDataReader["Is_Deleted"] != DBNull.Value && Convert.ToChar(oSqlDataReader["Is_Deleted"]) == Constants.C_YES ? 1 : 0
                };
                moSalaryEntityLists.lstStaticSalaryDetails.Add(oStaticSalaryDetails);
            }
        }

        private void LoadUnpublishStatus(SqlDataReader oSqlDataReader)
        {
            oSqlDataReader.Read();
            moBasicDetails.UnpublishStatus = Convert.ToChar(oSqlDataReader["AllowUnpublish"]);
            moBasicDetails.IsNextMonthAttendanceAvailable = Convert.ToChar(oSqlDataReader["IsNextMonthAttendanceAvailable"]);
        }

        private void LoadSGEDAssociation(SqlDataReader oSqlDataReader)
        {
            StaffGroupsEarningDeductionAssociation oStaffGroupsEarningDeductionAssociationDC;
            while (oSqlDataReader.Read())
            {
                oStaffGroupsEarningDeductionAssociationDC = new StaffGroupsEarningDeductionAssociation
                {
                    StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"])
                };
                moSalaryEntityLists.lstStaffGroupsEarningDeductionAssociation.Add(oStaffGroupsEarningDeductionAssociationDC);
            }
        }

        private void LoadUsersLeaves(SqlDataReader oSqlDataReader)
        {
            UserLeaveConfiguration oUserLeaveConfigurationDC;
            while (oSqlDataReader.Read())
            {
                oUserLeaveConfigurationDC = new UserLeaveConfiguration
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    LeaveBalance = Convert.ToDecimal(oSqlDataReader["LeaveBalance"]),
                    OriginalLeaveBalance = Convert.ToDecimal(oSqlDataReader["OriginalLeaveBalance"])
                };
                moSalaryEntityLists.lstUserLeaveConfiguration.Add(oUserLeaveConfigurationDC);
            }
        }

        private void LoadUsersED(SqlDataReader oSqlDataReader)
        {
            UsersEarningsDeduction oUsersEarningsDeductionDC;
            while (oSqlDataReader.Read())
            {
                oUsersEarningsDeductionDC = new UsersEarningsDeduction
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),                    
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    IsAttendanceDependent = Convert.ToBoolean(oSqlDataReader["IsAttendanceDependent"]),
                    IsEarning = Convert.ToBoolean(oSqlDataReader["IsEarning"]),
                    HasFormula = Convert.ToBoolean(oSqlDataReader["HasFormula"]),
                    EarningsDeductionsValue = Convert.ToDecimal(oSqlDataReader["EarningsDeductionsValue"])
                };
                moSalaryEntityLists.lstUsersEarningsDeduction.Add(oUsersEarningsDeductionDC);
            }
        }

        private void LoadUsersSGAssociation(SqlDataReader oSqlDataReader)
        {
            UsersSGAssociation oUsersSGAssociationDC;
            while (oSqlDataReader.Read())
            {
                oUsersSGAssociationDC = new UsersSGAssociation
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"])
                };
                moSalaryEntityLists.lstUsersSGAssociation.Add(oUsersSGAssociationDC);
            }
        }

        private void LoadAttendance(SqlDataReader oSqlDataReader)
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
                moSalaryEntityLists.lstStaffAttendance.Add(oStaffAttendanceDC);
            }
        }

        private void LoadStaffLeavesDetails(SqlDataReader oSqlDataReader)
        {
            StaffLeaveDetails oStaffLeaveDetailsDC;
            while (oSqlDataReader.Read())
            {
                oStaffLeaveDetailsDC = new StaffLeaveDetails
                {
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    StaffAttendanceId = Convert.ToInt32(oSqlDataReader["StaffAttendanceId"]),
                    Days = Convert.ToDecimal(oSqlDataReader["Days"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"])
                };
                moSalaryEntityLists.lstStaffLeaveDetails.Add(oStaffLeaveDetailsDC);
            }
        }

        private void LoadLeaves(SqlDataReader oSqlDataReader)
        {
            ConfiguredLeaves oConfiguredLeavesDC;
            while (oSqlDataReader.Read())
            {
                oConfiguredLeavesDC = new ConfiguredLeaves
                {
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    IsUnpaidLeave = Convert.ToBoolean(oSqlDataReader["IsUnpaidLeave"]),
                    OriginalLeaveId = Convert.ToInt32(oSqlDataReader["OriginalLeaveId"]),
                    MinimumBalance = Convert.ToInt32(oSqlDataReader["MinimumBalance"])
                };
                moSalaryEntityLists.lstConfiguredLeaves.Add(oConfiguredLeavesDC);
            }
        }

        private void LoadStaffLeaves(SqlDataReader oSqlDataReader)
        {
            ConfiguredLeaves oConfiguredLeavesDC;
            while (oSqlDataReader.Read())
            {
                oConfiguredLeavesDC = new ConfiguredLeaves
                {
                    LeaveId = Convert.ToInt32(oSqlDataReader["LeaveId"]),
                    ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                    IsUnpaidLeave = Convert.ToBoolean(oSqlDataReader["IsUnpaidLeave"]),
                    OriginalLeaveId = Convert.ToInt32(oSqlDataReader["OriginalLeaveId"]),
                    MinimumBalance = Convert.ToInt32(oSqlDataReader["MinimumBalance"]),
                    AllowZeroBalance = Convert.ToBoolean(oSqlDataReader["AllowZeroBalance"]),
                };
                moSalaryEntityLists.lstConfiguredLeaves.Add(oConfiguredLeavesDC);
            }
        }

        private void LoadAmountRanges(SqlDataReader oSqlDataReader)
        {
            AmountRange oAmountRangeDC;
            while (oSqlDataReader.Read())
            {
                oAmountRangeDC = new AmountRange
                {
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"]),
                    AmountRangeId = Convert.ToInt32(oSqlDataReader["AmountRangeId"]),
                    FromAmount = Convert.ToDecimal(oSqlDataReader["FromAmount"]),
                    UptoAmount = Convert.ToDecimal(oSqlDataReader["UptoAmount"]),
                    IsDefault = Convert.ToBoolean(oSqlDataReader["IsDefault"]),
                    RangeId = Convert.ToInt32(oSqlDataReader["RangeId"])
                };
                moSalaryEntityLists.lstAmountRange.Add(oAmountRangeDC);
            }
        }

        private void LoadMonthwiseAmounts(SqlDataReader oSqlDataReader)
        {
            MonthwiseAmount oMonthwiseAmountDC;
            while (oSqlDataReader.Read())
            {
                oMonthwiseAmountDC = new MonthwiseAmount
                {                    
                    Amount = Convert.ToDecimal(oSqlDataReader["Amount"]),
                    AmountRangeId = Convert.ToInt32(oSqlDataReader["AmountRangeId"]),
                    MonthId = Convert.ToInt32(oSqlDataReader["MonthId"])
                };
                moSalaryEntityLists.lstMonthwiseAmount.Add(oMonthwiseAmountDC);
            }
        }

        private void LoadEarningsDeductionsFormula(SqlDataReader oSqlDataReader)
        {
            EarningsDeductionsFormulae oEarningsDeductionsFormulaeDC;
            while (oSqlDataReader.Read())
            {
                oEarningsDeductionsFormulaeDC = new EarningsDeductionsFormulae
                {
                    FormulaId = Convert.ToInt32(oSqlDataReader["FormulaId"]),
                    EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                    Formula = Convert.ToString(oSqlDataReader["Formula"]),
                    IsDefault = Convert.ToBoolean(oSqlDataReader["IsDefault"])
                };
                moSalaryEntityLists.lstEarningsDeductionsFormulae.Add(oEarningsDeductionsFormulaeDC);
            }
        }

        private void LoadEarningsDeductions(SqlDataReader oSqlDataReader)
        {
            EarningsDeductions oEarningsDeductions;
            while (oSqlDataReader.Read())
            {
                oEarningsDeductions = new EarningsDeductions
                                    {
                                        EarningsDeductionsId = Convert.ToInt32(oSqlDataReader["EarningsDeductionsId"]),
                                        ShortName = Convert.ToString(oSqlDataReader["ShortName"]),
                                        IsAttendanceDependent = Convert.ToBoolean(oSqlDataReader["IsAttendanceDependent"]),
                                        IsEarning = Convert.ToBoolean(oSqlDataReader["IsEarning"]),
                                        HasFormula = Convert.ToBoolean(oSqlDataReader["HasFormula"]),
                                        OriginalEarningsDeductionsId = Convert.ToInt32(oSqlDataReader["OriginalEarningsDeductionsId"]),
                                        IsBasic = Convert.ToBoolean(oSqlDataReader["IsBasic"])
                                    };
                moSalaryEntityLists.lstEarningsDeductions.Add(oEarningsDeductions);
            }
        }

        private void LoadUsersDetails(SqlDataReader oSqlDataReader)
        {
            UsersDetails oUsersDetails;
            while (oSqlDataReader.Read())
            {
                oUsersDetails = new UsersDetails
                {
                    SrNo = Convert.ToInt32(oSqlDataReader["SrNo"]),
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    Name = Convert.ToString(oSqlDataReader["Name"]),
                    Designation = Convert.ToString(oSqlDataReader["Designation"]),
                    StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupId"]),
                    OriginalStaffGroupsId = Convert.ToInt32(oSqlDataReader["OriginalstaffGroupId"]),
                    JoiningDate = oSqlDataReader["JoiningDate"] == DBNull.Value? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["JoiningDate"]),
                    ResignDate = oSqlDataReader["ResignationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["ResignationDate"]),
                    Is_Deleted = oSqlDataReader["Is_Deleted"] != DBNull.Value && Convert.ToChar(oSqlDataReader["Is_Deleted"]) == Constants.C_YES ? 1 : 0,
                    Gender = Convert.ToChar(oSqlDataReader["Gender"])
                };
                moSalaryEntityLists.lstUsersDetails.Add(oUsersDetails);
            }
        }

        private void LoadStaffGroups(SqlDataReader oSqlDataReader)
        {
            StaffGroupsEntity oStaffGroupsDC;
            while (oSqlDataReader.Read())
            {
                oStaffGroupsDC = new StaffGroupsEntity
                {
                    StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"]),
                    OriginalStaffGroupsId = Convert.ToInt32(oSqlDataReader["OriginalStaffGroupsId"]),
                    StaffGroupsName = Convert.ToString(oSqlDataReader["StaffGroupsName"])
                };
                moSalaryEntityLists.lstStaffGroups.Add(oStaffGroupsDC);
            }
        }

        #endregion

        public static void SetSalaryDetails(string asSalaryDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sStatement = "TRUNCATE TABLE TempSalaryXml;INSERT INTO [dbo].[TempSalaryXml]([SalaryXml])VALUES(N'" + StringUtility.ReplaceSingleQuoteInString(asSalaryDetails, true) + "')";
                oSQLServerDbUtility.ExecuteTransaction(sStatement);
            }
        }

        public static bool CheckPTChallanDetailsExists(int aiSchoolId, int aiMonthId, int aiYear)
        {
            string sSelectStatement = "SELECT  TOP 1 1 " +
                                      " FROM " +
                                      "MonthwiseProfessionalTaxDetails" +
                                      " WHERE " +
                                      " SchoolId = " + aiSchoolId +
                                      " AND MonthId = " + aiMonthId +
                                      " AND Year = " + aiYear +
                                      " AND Is_Deleted = N'N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                return oSqlDataReader != null && oSqlDataReader.HasRows;
            }
        }

        /// <summary>
        /// This method is used to return salary details according to given user id.
        /// </summary>
        /// <param name="aiUserId"></param>
        public void GetSalaryStructureOfUser(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSalaryStructureDetails"))
                {
                    LoadEarningsDeductions(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadEarningsDeductionsFormula(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadAmountRanges(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadMonthwiseAmounts(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadUsersED(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadSGEDAssociation(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadUsersFormulaAndRanges(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadUsersSGAssociation(oSqlDataReader);
                }
            }
        }

        public void SavePaymentDetails(bool abIsOnlineTransaction, string asTransactionNumber, int aiSchoolId, int aiMonthId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TransactionNumber", asTransactionNumber, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsOnlinetRANSACTION", abIsOnlineTransaction.ToInt(), SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSalaryPaymentDetails");
            }
        }

        public List<GrossSalaryDetails> GetGrossSalary(int aiSchoolId, int aiMonthId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<GrossSalaryDetails> lstSalaryDetails = new List<GrossSalaryDetails>();
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetLastMonthsGrossSalary"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstSalaryDetails.Add
                            (
                                new GrossSalaryDetails
                                {
                                    UserId = oSqlDataReader["UserId"].ToInt(),
                                    Amount = oSqlDataReader["Amount"].ToInt()
                                }
                            );
                    }
                }

                return lstSalaryDetails;
            }
        }

        /// <summary>
        /// This method is used return salary changes.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<SalaryChange> GetAllSalaryChanges(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<SalaryChange> lstSalaryChanges = new List<SalaryChange>();
                oSQLServerDbUtility.AddParameter("SchoolId",this.miSchoolId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSalaryChanges"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstSalaryChanges.Add(
                            new SalaryChange
                            {
                                Basic = oSqlDataReader["Basic"].ToInt(),
                                GP = oSqlDataReader["GP"].ToInt(),
                                DAPercentage = oSqlDataReader["DAPercentage"].ToInt(),
                                HRAPercentage = oSqlDataReader["HRAPercentage"].ToInt(),
                                TA = oSqlDataReader["TA"].ToInt(),
                                RA = oSqlDataReader["RA"].ToInt(),
                                MonthName = oSqlDataReader["MonthAbbreviation"].ToString(),
                                Year = oSqlDataReader["Year"].ToInt(),
                                MonthId = oSqlDataReader["MonthId"].ToInt(),
                            }
                            );
                    }

                    oSqlDataReader.NextResult();
                    mlstPaidSalaryDifferences = new List<PaidSalaryDifference>();
                    while (oSqlDataReader.Read())
                    {
                        mlstPaidSalaryDifferences.Add(
                            new PaidSalaryDifference
                            {
                                EdId = oSqlDataReader["EdId"].ToInt(),
                                ShortName = oSqlDataReader["ShortName"].ToString(),
                                OriginalEdId = oSqlDataReader["OriginalEdId"].ToInt(),
                                Amount = oSqlDataReader["TotalAmount"].ToInt(),
                                MonthName = oSqlDataReader["MonthAbbreviation"].ToString(),
                                Year = oSqlDataReader["Year"].ToInt(),
                                MonthId = oSqlDataReader["MonthId"].ToInt(),
                            }
                            );
                    }

                    oSqlDataReader.NextResult();
                    mlstSalaryDifferencePaidDetails = new List<PaidSalaryDifference>();
                    while (oSqlDataReader.Read())
                    {
                        mlstSalaryDifferencePaidDetails.Add(
                            new PaidSalaryDifference
                            {
                                EdId = oSqlDataReader["EdId"].ToInt(),
                                ShortName = oSqlDataReader["ShortName"].ToString(),
                                OriginalEdId = oSqlDataReader["OriginalEdId"].ToInt(),
                                Amount = oSqlDataReader["TotalAmount"].ToInt(),
                                MonthName = oSqlDataReader["MonthAbbreviation"].ToString(),
                                PaidYear = oSqlDataReader["PaidYearId"].ToInt(),
                                PaidMonthId = oSqlDataReader["PaidMonthId"].ToInt(),
                            }
                            );
                    }
                }
                return lstSalaryChanges;
            }
        }

        public List<UserBasicDetails> GetUsers(int aiStaffGroupId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<UserBasicDetails> lstUserBasicDetails = new List<UserBasicDetails>();
                oSQLServerDbUtility.AddParameter("SchoolId",this.miSchoolId,SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSalaryUsers"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstUserBasicDetails.Add(new UserBasicDetails { 
                            UserId = oSqlDataReader["UserId"].ToInt(),
                            StaffName = oSqlDataReader["UserName"].ToString()
                        });
                    }
                }
                return lstUserBasicDetails;
            }
        }
    }

    public class BasicDetails
    {   
        public char UnpublishStatus;
        public bool DisplaySalaryDifference;
        public string UnpaidLeave = string.Empty;
        public char IsNextMonthAttendanceAvailable;
        public bool IsStaticData;
        public char IsLeaveIntervalMonth;
    }

}
