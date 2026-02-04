using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using DataCommunicator;
using Utility;
using SchoolEntities.Admin;

namespace DataCommunicator
{
    public class EmployeeDetailsDC
    {
        #region " Data Members "

        public int miSchoolId;
        public int miAcademicYearId;
        public int miUpdatedById;

        #endregion
        #region " Constructor "

        public EmployeeDetailsDC() { }

        public EmployeeDetailsDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }
        public void save(string asEmployeeOtherDetailsXML, string asEmployeeFamilyDetailsXML,  string asEmployeeStatutoryDetailsXML, string asemail, int iAcademicYearId, int iSchoolId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", iSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EmployeeOtherDetailsXML", asEmployeeOtherDetailsXML, SqlDbType.Xml);
                //   oSQLServerDbUtility.AddParameter("EmployeeContactDetailsXML", asEmployeeContactDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("EmployeeFamilyDetailsXML", asEmployeeFamilyDetailsXML, SqlDbType.Xml);
               // oSQLServerDbUtility.AddParameter("EmployeePreEmploymentDetailsXML", asEmployeePreEmploymentDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("EmployeeStatutoryDetailsXML", asEmployeeStatutoryDetailsXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("EmailAddress", asemail, SqlDbType.NVarChar);
                //  oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveEmployeeDetails");
            }
        }

        /// <summary>
        /// This method is used to get USerBasicDetails
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public  EmployeeDetails GetEmployeeBasicDetails(int aiUserId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetEmployeeBasicDetails"))
                {
                    EmployeeDetails oEmployeeDetails = new EmployeeDetails();
                    if (oSqlDataReader.Read())
                    {

                        oEmployeeDetails.Gender = Convert.ToInt32(oSqlDataReader["Gender"]).ToBool();
                        oEmployeeDetails.Reference = oSqlDataReader["Reference"].ToString();
                        oEmployeeDetails.Maritalstatus =Convert.ToInt32( oSqlDataReader["Maritalstatus"]).ToBool();
                        oEmployeeDetails.SalaryScale = oSqlDataReader["SalaryScale"].ToDecimal();
                        oEmployeeDetails.WhatsAppNo = oSqlDataReader["WhatsAppNo"].ToString();
                        oEmployeeDetails.GPFAcNumber = oSqlDataReader["GPFAcNumber"].ToString();
                        oEmployeeDetails.AccountNo = oSqlDataReader["BankAcNo"].ToString();
                        oEmployeeDetails.UAN = oSqlDataReader["UAN"].ToString();
                        if (oSqlDataReader.NextResult())
                        {
                            while (oSqlDataReader.Read())
                            {
                                oEmployeeDetails.Age = oSqlDataReader["Age"].ToInt();
                                oEmployeeDetails.FamilyMemberName = oSqlDataReader["FamilyMemberName"].ToString();
                                oEmployeeDetails.Occupaton = oSqlDataReader["Occupaton"].ToString();
                                oEmployeeDetails.Relation = oSqlDataReader["Relation"].ToString();
                            }
                        }
                        if (oSqlDataReader.NextResult())
                        {
                            while (oSqlDataReader.Read())
                            {
                                oEmployeeDetails.CompanyContactNo = oSqlDataReader["CompanyContactNo"].ToString();//
                                oEmployeeDetails.CompanyEmail = oSqlDataReader["CompanyEmail"].ToString();//
                                oEmployeeDetails.PermanentContactNo = oSqlDataReader["PermanatContactNo"].ToString();
                                oEmployeeDetails.Extensionno = oSqlDataReader["ExtensionNo"].ToString();
                                oEmployeeDetails.EPFNumber = oSqlDataReader["EPFNumber"].ToString();
                                oEmployeeDetails.IsVPSDeduction = oSqlDataReader["IsVPSDeduction"].ToBool();
                                oEmployeeDetails.VPSContributionId = oSqlDataReader["VPSContributionId"].ToInt();
                                oEmployeeDetails.VPFPercentage = oSqlDataReader["VPFPercentage"].ToDecimal();
                                oEmployeeDetails.VPSContributionEffectiveForm = oSqlDataReader["VPSContributionEffectiveForm"].ToDateTime();
                                oEmployeeDetails.UPFAmount = oSqlDataReader["UPFAmount"].ToDecimal();
                                oEmployeeDetails.BankName = oSqlDataReader["BankName"].ToString();
                                oEmployeeDetails.Branch = oSqlDataReader["Branch"].ToString();
                                oEmployeeDetails.IncrementDate = oSqlDataReader["IncrementDate"].ToDateTime();
                                oEmployeeDetails.IncomeTaxStatusId = oSqlDataReader["IncomeTaxStatusId"].ToInt();
                                oEmployeeDetails.PayrollId = oSqlDataReader["PayrollId"].ToInt();
                                oEmployeeDetails.BasicPay = oSqlDataReader["BasicPay"].ToDecimal();
                                oEmployeeDetails.PayrollGroupId = oSqlDataReader["PayrollGroupId"].ToInt();
                                oEmployeeDetails.PayScale = oSqlDataReader["PayScale"].ToDecimal();
                                oEmployeeDetails.EPFJoinDate = oSqlDataReader["EPFJoinDate"].ToDateTime();
                                oEmployeeDetails.EPFNumber = oSqlDataReader["EPFNumber"].ToString();
                            }
                        }
                       
                        //if (oSqlDataReader.NextResult())
                        //{
                        //    while (oSqlDataReader.Read())
                        //    {
                        //        oEmployeeDetails.Duration = oSqlDataReader["DurationDays"].ToString();
                        //        oEmployeeDetails.LastSalary = oSqlDataReader["Last Salary"].ToDecimal();
                        //        oEmployeeDetails.JobDescription = oSqlDataReader["Job Description"].ToString();
                        //        oEmployeeDetails.ReasonforLeaving = oSqlDataReader["Reason for Leaving"].ToString();
                        //        oEmployeeDetails.DesignationName = oSqlDataReader["PreviousDesignation"].ToString();
                        //    }
                        //}
                        if (oSqlDataReader.NextResult())
                        {
                            while (oSqlDataReader.Read())
                            {
                                oEmployeeDetails.PrimaryEmailId = oSqlDataReader["Email_Address"].ToString();
                            }
                        }

                    }
                    return oEmployeeDetails;
                }
            }
        }



        public DataTable GetAllBank(int aiSchoolId)
        {
            string sSelectStatement = " SELECT " +
                                    "Schoolwise_Bank_Id" +
                                    ",Bank_Name " +
                                    " FROM " +
                                    "Schoolwise_Bank_Master " +
                                    " WHERE " +
                                    " Is_Deleted =  N'" + Constants.C_NO + "'" +
                                   "AND" +
                                    " School_Id =" + aiSchoolId +
                                   " ORDER BY Bank_Name";
                                   
                                  

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        #endregion
    }
}
