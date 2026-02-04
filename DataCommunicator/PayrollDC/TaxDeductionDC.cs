// File Name - TaxDeductionDC.cs
// Creator - Pravin
// Created Date -

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using Utility;
using System.Data;
using DataCommunicator;
using PayrollEntities;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to communicate with database for insert/delete/update/ display of investment declarations.
    /// </summary>    
    public class TaxDeductionDC
    {
        #region Data Member(s)

            private int miSchoolId;
            private int miFinYearId;
            private int miUpdatedById;
            private int miAcademicYearId;
        
        #endregion

        #region Constructor(s)

            /// <summary>
            /// Default constructor.
            /// </summary>
            public TaxDeductionDC()
            {
            }

            /// <summary>
            /// Initializes member variables.
            /// </summary>
            /// <param name="aiSchoolId"></param>
            /// <param name="aiFinYearId"></param>
            /// <param name="aiUpdatedById"></param>
            public TaxDeductionDC(int aiSchoolId, int aiFinYearId, int aiUserId,int aiAcademicYearId)
            {
                this.miSchoolId = aiSchoolId;
                this.miFinYearId = aiFinYearId;
                this.miUpdatedById = aiUserId;
                this.miAcademicYearId = aiAcademicYearId;
            } 

        #endregion

        #region Public Method(s)

            /// <summary>
            /// This method is used to return all the Tax deduction of respective user.
            /// </summary>
            /// <param name="aiUserId"></param>
            /// <param name="aiSectionId"></param>
            /// <param name="asSortExpression"></param>
            /// <param name="asSortDirection"></param>
            /// <returns>List<InvestmentDeclaration></returns>
            public List<TaxDeduction> GetAll(int aiUserId, string asSortExpression, string asSortDirection)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.VarChar);
                    oSQLServerDbUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.VarChar);
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTaxDeductions"))
                    {
                        List<TaxDeduction> lstTaxDeductions = this.ReadFromDataReader(oSqlDataReader);
                        return lstTaxDeductions;
                    }
                }
            }
            
            /// <summary>
            /// This method is used to save the Tax deduction details.
            /// </summary>
            /// <param name="aoTaxDeductionDetails"></param>
            public void Save(TaxDeduction aoTaxDeductionDetails)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("Id", aoTaxDeductionDetails.Id, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("QuarterId", aoTaxDeductionDetails.QuarterId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("TaxDeductionAmount", aoTaxDeductionDetails.TaxDeductionAmount, SqlDbType.Decimal);
                    oSQLServerDbUtility.AddParameter("TaxDepositedAmount", aoTaxDeductionDetails.TaxDepositedAmount, SqlDbType.Decimal);
                    oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("UserId", aoTaxDeductionDetails.UserId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("IsDeleted", aoTaxDeductionDetails.Is_Deleted, SqlDbType.Bit);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveTaxDeduction");
                }
            }
            
            /// <summary>
            /// This method is used to get all the quarters.
            /// </summary>
            /// <returns></returns>
            public List<Quarter> GetAllQuarters()
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetQuarters"))
                    {
                        var oGenericClass = new GenericClass<Quarter>();
                        return oGenericClass.GetFilledObjectList(oSqlDataReader);
                    }
                }
            }

            /// <summary>
            /// This method is used to get Tax deducotr details.
            /// </summary>
            /// <returns></returns>
            public TaxDeductorDetails GetTaxDeductorDetails()
            {
                TaxDeductorDetails oTaxDeductorDetails = null;
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTaxDeductorDetails"))
                    {
                        if (oSqlDataReader.Read())
                        {
                            oTaxDeductorDetails = new TaxDeductorDetails
                            {
                                Id = oSqlDataReader["Id"].ToInt(),
                                SalutationId = oSqlDataReader["SalutationId"].ToInt(),
                                DesignationId = oSqlDataReader["DesignationId"].ToInt(),
                                Name = oSqlDataReader["Name"].ToString(),
                                FatherName = oSqlDataReader["FatherName"].ToString()
                            };
                        }
                        return oTaxDeductorDetails;
                    }
                }
            }

            /// <summary>
            /// This method is used to return user details.
            /// </summary>
            /// <param name="aiStaffGroupId"></param>
            /// <returns></returns>
            public List<UserBasicDetails> GetPayrollUsers(int aiStaffGroupId)
            {
                List<UserBasicDetails> lstUserBasicDetails = new List<UserBasicDetails>();
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    if (aiStaffGroupId == 0)
                        aiStaffGroupId = -9999;
                    oSQLServerDbUtility.AddParameter("StaffGroupsId", aiStaffGroupId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Academic_Year_Id", this.miAcademicYearId, SqlDbType.Int);
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetInvestmentDeclarationUserDetails"))
                    {
                        while (oSqlDataReader.Read())
                        {
                            UserBasicDetails oUserBasicDetails = new UserBasicDetails
                            {
                                UserId = oSqlDataReader["Value_Member"].ToInt(),
                                StaffName = oSqlDataReader["Display_Member"].ToString()
                            };
                            lstUserBasicDetails.Add(oUserBasicDetails);
                        }
                    }
                    return lstUserBasicDetails;
                }
            }

            /// <summary>
            /// This method is used to get the CIT details.
            /// </summary>
            /// <returns></returns>
            public ITCommissionerDetails GetCITDetails()
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    ITCommissionerDetails oITCommissionerDetails = null;
                    oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                    using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetCITDetails"))
                    {
                        if (oSqlDataReader.Read())
                        {
                            oITCommissionerDetails = new ITCommissionerDetails
                            {
                                Address = oSqlDataReader["Address"].ToString(),
                                City = oSqlDataReader["City"].ToString(),
                                Pincode = oSqlDataReader["Pincode"].ToString()
                            };
                        }

                        return oITCommissionerDetails;
                    }
                }
            }

            /// <summary>
            /// This method is used to save CIT details.
            /// </summary>
            /// <param name="oITCommissionerDetails"></param>
            public void SaveCITDetails(ITCommissionerDetails oITCommissionerDetails)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("Address", oITCommissionerDetails.Address, SqlDbType.VarChar);
                    oSQLServerDbUtility.AddParameter("City", oITCommissionerDetails.City, SqlDbType.VarChar);
                    oSQLServerDbUtility.AddParameter("PinCode", oITCommissionerDetails.Pincode, SqlDbType.VarChar);
                    oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);                    
                    oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveCITDetails");
                }
            }

            /// <summary>
            /// This method is used to save quarter details.
            /// </summary>
            /// <param name="saQuarterXML"></param>
            public void SaveQuarters(string asQuarterXML)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("QuarterXML", asQuarterXML, SqlDbType.Xml);
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);                    
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveQuarters");
                }
            }

            /// <summary>
            /// This method is used to tax deductor details.
            /// </summary>
            /// <param name="aoTaxDeductorDetails"></param>
            public void SaveTaxDeductorDetails(TaxDeductorDetails aoTaxDeductorDetails)
            {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("Id", aoTaxDeductorDetails.Id, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("SalutationId", aoTaxDeductorDetails.SalutationId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("DesignationId", aoTaxDeductorDetails.DesignationId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("Name", aoTaxDeductorDetails.Name, SqlDbType.VarChar);
                    oSQLServerDbUtility.AddParameter("FatherName", aoTaxDeductorDetails.FatherName, SqlDbType.VarChar);                                        
                    oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);                    
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveTaxDeductorDetails");
                }
            }

            #endregion

            #region Private Method(s)

            /// <summary>
            /// This method is used to fill tax deduction in entity list.
            /// </summary>
            /// <param name="aoSqlDataReader"></param>
            /// <returns>Entity List of InvestmentDeclaration</returns>
            private List<TaxDeduction> ReadFromDataReader(SqlDataReader aoSqlDataReader)
            {
                List<TaxDeduction> lstTaxDeductions = new List<TaxDeduction>();
                while (aoSqlDataReader.Read())
                {
                    TaxDeduction oInvestmentDeclaration = new TaxDeduction
                    {
                        Id = Convert.ToInt32(aoSqlDataReader["Id"]),     
                        TaxDeductionAmount =Convert.ToDecimal(aoSqlDataReader["TaxDeductionAmount"]),
                        TaxDepositedAmount =Convert.ToDecimal(aoSqlDataReader["TaxDepositedAmount"]),
                        QuarterId=Convert.ToInt32(aoSqlDataReader["QuarterId"]),
                        QuarterName=Convert.ToString(aoSqlDataReader["QuarterName"]),
                        UserId=Convert.ToInt32(aoSqlDataReader["UserId"])
                    };
                    lstTaxDeductions.Add(oInvestmentDeclaration);
                }

                return lstTaxDeductions;
            }

            #endregion
    }
}
