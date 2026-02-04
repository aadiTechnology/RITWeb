// File Name - InvestmentDeclarationDC.cs
// Create By - Sachin
// Created Date - 

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to communicate with database for insert/delete/update/ display of investment declarations.
    /// </summary>
    public class InvestmentDeclarationDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miFinYearId;
        private int miUpdatedById;
        
        private UserDetails moUserDetails;
        private List<SectionDetails> mlstSectionDetails;
        private List<InvestmentDeclaration> mlstInvestmentDeclaration;
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        public InvestmentDeclarationDC()
        {
        }

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinYearId"></param>
        /// <param name="aiUpdatedById"></param>
        public InvestmentDeclarationDC(int aiSchoolId, int aiFinYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miFinYearId = aiFinYearId;
            this.miUpdatedById = aiUpdatedById;
        } 

        #endregion


        #region Property(s)

        public UserDetails UserDetails
        {
            get { return moUserDetails; }
        }

        public List<SectionDetails> SectionDetails
        {
            get { return mlstSectionDetails; }
        }

        public List<InvestmentDeclaration> InvestmentDeclarations
        {
            get { return mlstInvestmentDeclaration; }
        }

        #endregion

        #region Public Method(s)

       /// <summary>
        /// This method is used to return all the investment declrations of respective user.
       /// </summary>
       /// <param name="aiUserId"></param>
       /// <param name="aiSectionId"></param>
       /// <param name="asSortExpression"></param>
       /// <param name="asSortDirection"></param>
        /// <returns>List<InvestmentDeclaration></returns>
        public List<InvestmentDeclaration> GetAll(int aiUserId, int aiSectionId, string asSortExpression, string asSortDirection)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SectionId", aiSectionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortDirection", asSortDirection, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllInvestmentDeclarations"))
                {
                    List<InvestmentDeclaration> lstInvestmentDeclaration = this.ReadFromDataReader(oSqlDataReader);
                    return lstInvestmentDeclaration;
                }
            }
        }

        /// <summary>
        /// This method is used to save investment declarations of respective user.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asXml"></param>
        public void Save(int aiUserId, string asXml, int aiRegimId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Xml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("RegimId", aiRegimId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveInvestmentDeclarationDetails");
            }
        }

        /// <summary>
        ///  This method is used to return investment documents.
        /// </summary>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<InvestmentDocument> GetDocuments(int aiDocuentId, int aiUserId, int aiDocumnetTypeId, int aiAcademicYearId,int aiReportingUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocuentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumnetTypeId", aiDocumnetTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", this.miUpdatedById, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllInvestmentDocuments"))
                {

                    List<InvestmentDocument> lstDocuments = new List<InvestmentDocument>();
                    InvestmentDocument oInvestmentDocument;
                    while (oSqlDataReader.Read())
                    {
                        oInvestmentDocument = new InvestmentDocument
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            FileName = Convert.ToString(oSqlDataReader["FileNAme"])
                        };
                        lstDocuments.Add(oInvestmentDocument);
                    }
                    return lstDocuments;
                }
            }
        }

        /// <summary>
        ///  This method is used to save document.
        /// </summary>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="asFileName"></param>
        /// <param name="aiUserId"></param>
        public void SaveDocument(int aiDocumentId, string asFileName, int aiUserId, int aiDocumnetTypeId, int aiAcademicYearId,int aiReportingUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FileName", asFileName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumnetTypeId", aiDocumnetTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReportingUserId", aiReportingUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveInvestmentDocuments");
                
            }
        }

        /// <summary>
        /// This method is used to delete document.
        /// </summary>
        /// <param name="iId"></param>
        public void DeleteDocument(int aiDocumentId, int aiDocumnetTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumnetTypeId", aiDocumnetTypeId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteInvestmentDocument");
            }
        }

        /// <summary>
        /// This method is used to return user and investment method details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiInvestmentMethodId"></param>
        /// <param name="asInvestmentMethod"></param>
        /// <returns></returns>
        public string GetUserInvestmentMethodDetails(int aiUserId, int aiDocumentId, out string asDocumentName, int aiDocumentTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentId", aiDocumentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentTypeId", aiDocumentTypeId, SqlDbType.Int);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserInvestmentDetails"))
                {
                    oSqlDataReader.Read();
                    asDocumentName = Convert.ToString(oSqlDataReader["DocumentName"]);
                    return Convert.ToString(oSqlDataReader["UserName"]);
                }
            }
        }

        /// <summary>
        /// This method is used to return investment details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<InvestmentMethod> GetInvestmentDetails(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetInvestmentDetails");
                List<InvestmentMethod> lstInvestmentMethod = FillInvestmentDetails(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillUserDetails(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillSectionDetails(oSqlDataReader);

                oSqlDataReader.NextResult();
                FillInvestmentDeclarations(oSqlDataReader);

                return lstInvestmentMethod;
            }
        }

        /// <summary>
        /// This method is used to save investment details.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asDeclarations"></param>
        public void SaveInvestmentDeclaration(int aiUserId, string asDeclarations, int aiRegimId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DeclarationXML", asDeclarations, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("RegimeId", aiRegimId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveInvestmentDetails");
            }
        }

        /// <summary>
        /// This method is used to submit investment details.
        /// </summary>
        /// <param name="aiUserId"></param>
        public void SubmitInvestmentDetails(int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitInvestmentDetails");
            }
        }

        public List<UserDetails> GetRegimeDetails()
        {
            List<UserDetails> olstUserDetails = new List<UserDetails>();
            string sSQLStatement = "SELECT Id, Name FROM [dbo].[RegimeCategories] WHERE IsDeleted = 0";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSQLDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSQLStatement))
                {
                    while (oSQLDataReader.Read())
                    {
                        UserDetails oUserDetails = new UserDetails
                        {
                            Id = oSQLDataReader["Id"].ToInt(),
                            Name = oSQLDataReader["Name"].ToString()
                        };
                        olstUserDetails.Add(oUserDetails);
                    }
                }
            }
            return olstUserDetails;
        }
       
        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to fill investment declaration in entity list.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns>Entity List of InvestmentDeclaration</returns>
        private List<InvestmentDeclaration> ReadFromDataReader(SqlDataReader aoSqlDataReader)
        {
            List<InvestmentDeclaration> lstInvestmentDeclaration = new List<InvestmentDeclaration>();
            while (aoSqlDataReader.Read())
            {
                InvestmentDeclaration oInvestmentDeclaration = new InvestmentDeclaration
                {
                    Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                    InvestmentMethodId = Convert.ToInt32(aoSqlDataReader["InvestmentMethodId"]),
                    Name = Convert.ToString(aoSqlDataReader["Name"]),
                    Amount = Convert.ToDecimal(aoSqlDataReader["Amount"]),                    
                    DocumentCount = Convert.ToInt32(aoSqlDataReader["DocumentCount"]),
                    IsDocSubmitted = Convert.ToBoolean(aoSqlDataReader["IsDocSubmitted"]),
                    SectionId = Convert.ToInt32(aoSqlDataReader["SectionId"]),
                    SectionName = Convert.ToString(aoSqlDataReader["SectionName"]),
                    UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                    RegimId = Convert.ToInt32(aoSqlDataReader["RegimId"])
                };
                lstInvestmentDeclaration.Add(oInvestmentDeclaration);
            }

            return lstInvestmentDeclaration;
        }

        /// <summary>
        /// This method is used to fill up investment declaration details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillInvestmentDeclarations(SqlDataReader aoSqlDataReader)
        {
            mlstInvestmentDeclaration = new List<InvestmentDeclaration>();
            while (aoSqlDataReader.Read())
            {
                mlstInvestmentDeclaration.Add
                    (
                        new InvestmentDeclaration
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Amount = Convert.ToInt32(aoSqlDataReader["Amount"]),
                            InvestmentMethodId = Convert.ToInt32(aoSqlDataReader["InvestmentMethodId"]),
                            SectionId = Convert.ToInt32(aoSqlDataReader["SectionId"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill up section details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillSectionDetails(SqlDataReader aoSqlDataReader)
        {
            mlstSectionDetails = new List<SectionDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstSectionDetails.Add
                    (
                        new SectionDetails
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"]),
                            SectionGroupId = Convert.ToInt32(aoSqlDataReader["SectionGroupId"]),
                            CategoryId = Convert.ToInt32(aoSqlDataReader["CategoryId"]),
                            SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                            GroupMaxAmount = Convert.ToInt32(aoSqlDataReader["GroupMaxAmount"])
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill up user details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillUserDetails(SqlDataReader aoSqlDataReader)
        {
            aoSqlDataReader.Read();
            moUserDetails = new UserDetails
            {
                Address = Convert.ToString(aoSqlDataReader["Address"]),
                Designation = Convert.ToString(aoSqlDataReader["Designation"]),
                EmployeeNo = Convert.ToString(aoSqlDataReader["EmployeeNo"]),
                FinancialYear = Convert.ToString(aoSqlDataReader["FinancialYear"]),
                Gender = Convert.ToString(aoSqlDataReader["Gender"]),
                PanNo = Convert.ToString(aoSqlDataReader["PanNo"]),
                SchoolAddress = Convert.ToString(aoSqlDataReader["SchoolAddress"]),
                SchoolName = Convert.ToString(aoSqlDataReader["SchoolName"]),
                UserId = Convert.ToInt32(aoSqlDataReader["UserId"]),
                UserName = Convert.ToString(aoSqlDataReader["UserName"]),
                IsSubmitted = Convert.ToBoolean(aoSqlDataReader["IsSubmitted"]),
                IsSaved = Convert.ToBoolean(aoSqlDataReader["IsSaved"]),
                FinancialYearEnd = Convert.ToString(aoSqlDataReader["FinancialYearEnd"]),
                Id = Convert.ToInt32(aoSqlDataReader["RegimeId"]),

            };
        }

        /// <summary>
        /// This method is used to fill up investment methods.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<InvestmentMethod> FillInvestmentDetails(SqlDataReader aoSqlDataReader)
        {
            List<InvestmentMethod> lstInvestmentMethod = new List<InvestmentMethod>();
            while (aoSqlDataReader.Read())
            {
                lstInvestmentMethod.Add
                    (
                        new InvestmentMethod
                        {
                            Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                            SectionId = Convert.ToInt32(aoSqlDataReader["SectionId"]),
                            Name = Convert.ToString(aoSqlDataReader["Name"]),
                            AssociatedEarnDeductId = Convert.ToInt32(aoSqlDataReader["AssociatedEarnDeductId"]),
                            MaxAmount = Convert.ToInt32(aoSqlDataReader["MaxAmount"]),
                            DocumentCount = Convert.ToInt32(aoSqlDataReader["DocumentCount"])
                        }
                    );
            }
            return lstInvestmentMethod;
        }

        #endregion
    }
}
