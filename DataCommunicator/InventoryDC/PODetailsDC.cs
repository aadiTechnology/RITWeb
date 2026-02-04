using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SchoolEntities;
using Utility;


namespace DataCommunicator
{
    public class PODetailsDC : DataCommunicatorBaseDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miFinancialYearId;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public PODetailsDC(int aiSchoolId, int aiUserId)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUserId;
        }

        public PODetailsDC(int aiSchoolId,int aiFinancialYearId, int aiUserId, int aiAcademicYearId)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUserId;
            this.miFinancialYearId = aiFinancialYearId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        public PODetailsDC()
        {
            // TODO: Complete member initialization
        } 

        #endregion
        
        #region Public Method(s)

        /// <summary>
        /// This method is used to delete particular record.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteExternalPODetails");
            }
        }

        /// <summary>
        /// This method is used to non duplicate PONo
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="asInvoiceNo"></param>
        /// <returns></returns>
        public bool IsExternalPONoDuplicate(int aiId, string asPONo, bool abIsPO)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PONo", asPONo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPO", abIsPO, SqlDbType.Bit);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsDuplicate", false, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsExternalPONoDuplicate");
                return oSqlParameter.Value.ToBool();
            }
        }

        /// <summary>
        /// This method is used to return POD description.
        /// </summary>
        /// <param name="aId"></param>
        /// <returns></returns>
        public List<ExternalPODescription> GetPODescriptions(int aId)
        {
            List<ExternalPODescription> lstPODetails = new List<ExternalPODescription>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExternalPODescriptions"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ExternalPODescription oPODetails = new ExternalPODescription();
                        oPODetails.Amount = oSqlDataReader["Amount"].ToDecimal();
                        oPODetails.Quantity = oSqlDataReader["Quantity"].ToInt();
                        oPODetails.Rate = oSqlDataReader["Rate"].ToDecimal();
                        oPODetails.Name = oSqlDataReader["Name"].ToString();
                        oPODetails.Description = oSqlDataReader["Description"].ToString();
                        oPODetails.GSTCategoryId = oSqlDataReader["GSTCategoryId"].ToInt();

                        lstPODetails.Add(oPODetails);
                    }
                }
                return lstPODetails;
            }
        }

        /// <summary>
        /// This method is used to get External PO details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asFilter"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<ExternalPODetails> GetAll(int aiSchoolId, string asFilter, bool abIsPO, int aiFinancialYearId, int aiStatusId, int aiLoginUserId, string asSortExpression, int aiStartIndex, int aiEndIndex, int aiAcademicYearId)
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsPO", abIsPO, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StatusId", aiStatusId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LoginUserId", aiLoginUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExternalPODetails"))
                {
                    List<ExternalPODetails> lstPO = new List<ExternalPODetails>();

                    List<ExternalPODescription> lstExternalPODescription = new List<ExternalPODescription>();
                    while (oSqlDataReader.Read())
                    {
                        lstExternalPODescription.Add(new ExternalPODescription
                       {
                           Id = Convert.ToInt32(oSqlDataReader["Id"]),
                           Description = Convert.ToString(oSqlDataReader["Description"]),
                           Quantity = Convert.ToInt32(oSqlDataReader["Quantity"]),
                           Rate = Convert.ToInt32(oSqlDataReader["Rate"]),
                           GST = Convert.ToDecimal(oSqlDataReader["GST"]),
                           Amount = Convert.ToInt32(oSqlDataReader["Amount"]),
                           PODId = Convert.ToInt32(oSqlDataReader["PODId"])
                       });
                    }

                    oSqlDataReader.NextResult();
                    while (oSqlDataReader.Read())
                    {
                        ExternalPODetails obj = new ExternalPODetails
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            ReceiverId = Convert.ToInt32(oSqlDataReader["ExternalPOUserId"]),
                            PONo = Convert.ToString(oSqlDataReader["PONo"]),
                            PODate = Convert.ToDateTime(oSqlDataReader["PODate"]),
                            TotalAmount = Convert.ToDecimal(oSqlDataReader["TotalAmount"]),
                            GST = Convert.ToDecimal(oSqlDataReader["GST"]),
                            GrandTotal = Convert.ToDecimal(oSqlDataReader["GrandTotal"]),
                            Subject = Convert.ToString(oSqlDataReader["Subject"]),
                            //StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]),
                            //EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]),
                            TotalRows = oSqlDataReader["TotalRows"].ToInt(),
                            ReceiverName = Convert.ToString(oSqlDataReader["Name"]),
                            Descriptions = lstExternalPODescription.Where(dc => dc.PODId == Convert.ToInt32(oSqlDataReader["Id"])).ToList(),
                            PreparedBy = Convert.ToString(oSqlDataReader["PreparedBy"]),
                            Status = Convert.ToString(oSqlDataReader["Status"]),
                            StatusId = Convert.ToInt32(oSqlDataReader["StatusId"]),
                            Comment = Convert.ToString(oSqlDataReader["Comment"]),
                            TotalPaidAmount = Convert.ToInt32(oSqlDataReader["TotalPaidAmount"])
                        };

                        if (oSqlDataReader["StartDate"] != DBNull.Value)
                            obj.StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]);

                        if (oSqlDataReader["EndDate"] != DBNull.Value)
                            obj.EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]);

                        lstPO.Add(obj);

                    }
                    return lstPO;
                }
            }
        }

        /// <summary>
        /// This method is used to save External PO Details.
        /// </summary>
        /// <param name="sXml"></param>
        /// <param name="oGSTInvoiceDetails"></param>
        public void Save(string sXml, ExternalPODetails oExternalPODetails)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("POXmlDetails", sXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Id", oExternalPODetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReceiverId", oExternalPODetails.ReceiverId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PONo", oExternalPODetails.PONo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("PODate", oExternalPODetails.PODate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("TotalAmount", oExternalPODetails.TotalAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GST", oExternalPODetails.GST, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("GrandTotal", oExternalPODetails.GrandTotal, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("Subject", oExternalPODetails.Subject, SqlDbType.NVarChar);
                if (oExternalPODetails.StartDate != DateTime.MinValue)
                    oSQLServerDbUtility.AddParameter("StartDate", oExternalPODetails.StartDate, SqlDbType.DateTime);
                if (oExternalPODetails.EndDate != DateTime.MinValue)
                oSQLServerDbUtility.AddParameter("EndDate", oExternalPODetails.EndDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("ExternalPOMasterId", oExternalPODetails.ExternalPOMasterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ExternalPOInstructionId", oExternalPODetails.ExternalPOInstructionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InstructionIds", oExternalPODetails.InstructionIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsPO", oExternalPODetails.IsPO, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AdditionalRemarks", oExternalPODetails.AdditionalRemarks, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveExternalPODetails");
            }
        }

        /// <summary>
        /// This method is used to return all External PO details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public ExternalPODetails Get(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                ExternalPODetails oExternalPODetails = new ExternalPODetails();
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);

                List<int> lstInstructions = new List<int>();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDataToReadExternalPODetails"))
                {  
                    if (oSqlDataReader.Read())
                    {
                        oExternalPODetails.Id = oSqlDataReader["Id"].ToInt();
                        oExternalPODetails.ExternalPOUserId = oSqlDataReader["ExternalPOUserId"].ToInt();
                        oExternalPODetails.PONo = oSqlDataReader["PONo"].ToString();
                        oExternalPODetails.PODate = oSqlDataReader["PODate"].ToDateTime();
                        oExternalPODetails.TotalAmount = oSqlDataReader["TotalAmount"].ToDecimal();
                        //oExternalPODetails.GST = oSqlDataReader["GST"].ToDecimal();
                        //oExternalPODetails.GrandTotal = oSqlDataReader["GrandTotal"].ToDecimal();
                        oExternalPODetails.Subject = oSqlDataReader["Subject"].ToString();
                        //oExternalPODetails.StartDate = oSqlDataReader["StartDate"].ToDateTime();
                        //oExternalPODetails.EndDate = oSqlDataReader["EndDate"].ToDateTime();
                        if (oSqlDataReader["StartDate"] != DBNull.Value)
                            oExternalPODetails.StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]);
                        if (oSqlDataReader["EndDate"] != DBNull.Value)
                            oExternalPODetails.EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]);
                        oExternalPODetails.PreparedBy = oSqlDataReader["PreparedBy"].ToString();
                        oExternalPODetails.AdditionalRemarks = oSqlDataReader["AdditionalRemarks"].ToString();
                        oExternalPODetails.TotalPaidAmount = oSqlDataReader["TotalPaidAmount"].ToDecimal();
                        oExternalPODetails.IsPO = oSqlDataReader["IsPO"].ToBool();
                    }

                    oSqlDataReader.NextResult();
                    while (oSqlDataReader.Read())
                    {
                        lstInstructions.Add(oSqlDataReader["ExternalPOInstructionId"].ToInt());
                    }

                    oExternalPODetails.InstructionList = lstInstructions;
                }

                return oExternalPODetails;
            }
        }

        /// <summary>
        /// This method is used to fill receiver name dropdown.
        /// </summary>
        /// <returns></returns>
        public List<ReceiverName> GetReceiverName()
        {
            List<ReceiverName> olstReceiverName = new List<ReceiverName>();
            string sSQLStatement = "SELECT Id, Name FROM [dbo].[ExternalPOUsers] WHERE IsDeleted = 0 Order by Name";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSQLDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSQLStatement))
                {
                    while (oSQLDataReader.Read())
                    {
                        ReceiverName oReceiverName = new ReceiverName
                        {
                            ReceiverId = oSQLDataReader["Id"].ToInt(),
                            Name = oSQLDataReader["Name"].ToString(),
                        };
                        olstReceiverName.Add(oReceiverName);
                    }
                }
            }
            return olstReceiverName;
        }

        /// <summary>
        /// This method is used to fill GSTCategory dropdown
        /// </summary>
        /// <returns></returns>
        public List<GSTCategory> GetGSTCategory()
        {
            List<GSTCategory> olstGSTCategory = new List<GSTCategory>();
            string sSQLStatement = "SELECT Id, Name, Percentage FROM [dbo].[GSTDetails] WHERE IsDeleted = 0 order by Name";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSQLDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSQLStatement))
                {
                    while (oSQLDataReader.Read())
                    {
                        GSTCategory oGSTCategory = new GSTCategory
                        {
                            Id = oSQLDataReader["Id"].ToInt(),
                            Name = oSQLDataReader["Name"].ToString(),
                            Percentage = oSQLDataReader["Percentage"].ToDecimal()
                        };
                        olstGSTCategory.Add(oGSTCategory);
                    }
                }
            }
            return olstGSTCategory;
        }

        /// <summary>
        /// This method is used to fill instruction checkboxlist.
        /// </summary>
        /// <returns></returns>
        public POInstructionDetails GetInstructions()
        {
            List<Instruction> olstInstructions = new List<Instruction>();
            string sSQLStatement = "SELECT Id, InstructionName, InstCategoryId FROM [dbo].[ExternalPOInstructions] WHERE IsDeleted = 0";

            List<POExternalCategory> olstCategories = new List<POExternalCategory>();
            sSQLStatement += ";SELECT Id, Category FROM [dbo].[POExternalCategories] WHERE IsDeleted = 0";

            POInstructionDetails oPOInstructionDetails = new POInstructionDetails();
             
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSQLDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSQLStatement))
                {
                    while (oSQLDataReader.Read())
                    {
                        Instruction oInstructions = new Instruction
                        {
                            Id = oSQLDataReader["Id"].ToInt(),
                            InstructionName = oSQLDataReader["InstructionName"].ToString(),
                            InstCategoryId = oSQLDataReader["InstCategoryId"].ToInt(),
                        };
                        olstInstructions.Add(oInstructions);
                    }

                    oPOInstructionDetails.Instructions = olstInstructions;
                    oSQLDataReader.NextResult();
                    while (oSQLDataReader.Read())
                    {
                        olstCategories.Add(new POExternalCategory
                        {
                            Id = oSQLDataReader["Id"].ToInt(),
                            Category = oSQLDataReader["Category"].ToString()
                        });
                    }
                    oPOInstructionDetails.Categories = olstCategories;
                    return oPOInstructionDetails;
                }
            }
        }

        #endregion

        #region Private MEthod(s)

        /// <summary>
        /// This method is used to set receiver details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private ExternalPODetails SetReceiverDetails(SqlDataReader aoSqlDataReader)
        {
            return new ExternalPODetails
            {
                Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                GSTCategoryId = Convert.ToInt32(aoSqlDataReader["GSTCategoryId"]),
                ExternalPOUserId = Convert.ToInt32(aoSqlDataReader["ExternalPOUserId"]),
                PONo = Convert.ToString(aoSqlDataReader["PONo"]),
                PODate = Convert.ToDateTime(aoSqlDataReader["PODate"]),
                TotalAmount = Convert.ToDecimal(aoSqlDataReader["TotalAmount"]),
                TotalRows = aoSqlDataReader["TotalRows"].ToInt(),
                ReceiverName = Convert.ToString(aoSqlDataReader["Name"]),
                Subject = Convert.ToString(aoSqlDataReader["Subject"]),
                StartDate = Convert.ToDateTime(aoSqlDataReader["StartDate"]),
                EndDate = Convert.ToDateTime(aoSqlDataReader["EndDate"]),
            };
        }

        /// <summary>
        /// This method is used to get prefixes.
        /// </summary>
        /// <returns></returns>
        public ExternalOrderPrefix GetPrefixes()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                ExternalOrderPrefix oExternalOrderPrefix = new ExternalOrderPrefix();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExternalOrderPrefixes"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oExternalOrderPrefix.POPrefix = oSqlDataReader["POPrefix"].ToString();
                        oExternalOrderPrefix.WOPrefix = oSqlDataReader["WOPrefix"].ToString();
                    }
                }
                return oExternalOrderPrefix;
            }
        }

        /// <summary>
        /// This method is used to send request for approval.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public int SendRequestForApproval(int aiId)
        {
            int iNotificationReceiverUserId = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId",this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_SendRequestForApproval"))
                {
                    if(oSqlDataReader.Read())
                    {
                        iNotificationReceiverUserId = oSqlDataReader["UserId"].ToInt();
                    }
                }
            }
            return iNotificationReceiverUserId;
        }

        /// <summary>
        /// This method is used to approve request.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="asComment"></param>
        /// <param name="abIsApproved"></param>
        /// <returns></returns>
        public int ApproveRequest(int aiId, string asComment, bool abIsApproved)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Comment", asComment, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsApproved", abIsApproved, SqlDbType.Bit);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ApproveExternalPORequest"))
                {
                    if (oSqlDataReader.Read())
                        return oSqlDataReader["UserId"].ToInt();
                }
            }
            return 0;
        }

        /// <summary>
        /// This method is used to return payment details.
        /// </summary>
        /// <param name="aiPoMasterId"></param>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public List<POFeePayment> GetAllPayments(int aiPoMasterId, int aiId)
        {
            List<POFeePayment> lstPOFeePayment = new List<POFeePayment>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PoMasterId", aiPoMasterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllPOPayments"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstPOFeePayment.Add(new POFeePayment {
                            Amount = oSqlDataReader["Amount"].ToDecimal(),
                            BankName = oSqlDataReader["Bank_Name"].ToString(),
                            PaymentDate = oSqlDataReader["PaymentDate"].ToDateTime(),
                            PaymentModeId = oSqlDataReader["PaymentModeId"].ToInt(),
                            TxnNo = oSqlDataReader["TransactionNo"].ToString(),
                            Type = oSqlDataReader["Type"].ToString(),
                            Id = oSqlDataReader["Id"].ToInt(),
                            BankId = oSqlDataReader["BankId"].ToInt(),
                            TypeId = oSqlDataReader["TypeId"].ToInt(),
                            Remark = oSqlDataReader["Remark"].ToString(),
                            ChequeDate = (oSqlDataReader["ChequeDate"] == DBNull.Value?DateTime.MinValue : oSqlDataReader["ChequeDate"].ToDateTime())
                        });
                    }
                }
            }
            return lstPOFeePayment;
        }

        /// <summary>
        /// This method is used to save payment details.
        /// </summary>
        /// <param name="aoPOFeePayment"></param>
        public void SavePayment(POFeePayment aoPOFeePayment)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.AddParameter("Id", aoPOFeePayment.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentModeId", aoPOFeePayment.PaymentModeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Amount", aoPOFeePayment.Amount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("BankId", aoPOFeePayment.BankId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PaymentDate", aoPOFeePayment.PaymentDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("TxnNo", aoPOFeePayment.TxnNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("TypeId", aoPOFeePayment.TypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ChequeDate", aoPOFeePayment.ChequeDate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("POMasterId", aoPOFeePayment.POMasterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Remark", aoPOFeePayment.Remark, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveExternalPOPaymentDetails");
            }
        }

        /// <summary>
        /// This method is used to delete payment details.
        /// </summary>
        /// <param name="aiPoMasterId"></param>
        /// <param name="aiId"></param>
        public void DeletePaymentDetails(int aiPoMasterId, int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", this.miFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PoMasterId", aiPoMasterId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteExternalPOPaymentDetails");
            }
        }

        #endregion
    }
}
