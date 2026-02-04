using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SchoolEntities;
using Utility;


namespace DataCommunicator
{
    public class GSTInvoiceDetailsDC : DataCommunicatorBaseDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public GSTInvoiceDetailsDC(int aiSchoolId, int aiUserId, int aiAcademicYearId)
        {
            // TODO: Complete member initialization
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUserId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        public GSTInvoiceDetailsDC()
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
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteGSTInvoiceDetails");
            }
        }

        /// <summary>
        /// This method is used to non duplicate InvoiceNo
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="asInvoiceNo"></param>
        /// <returns></returns>
        public bool IsInvoiceNoDuplicate(int aiId, string asInvoiceNo)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InvoiceNo", asInvoiceNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsDuplicate", false, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsInvoiceNoDuplicate");
                return oSqlParameter.Value.ToBool();
            }
        }

        /// <summary>
        /// This method is used to return GST description.
        /// </summary>
        /// <param name="aId"></param>
        /// <returns></returns>
        public List<GSTInvoiceDescription> GetGSTDescriptions(int aId)
        {
            List<GSTInvoiceDescription> lstGSTDetails = new List<GSTInvoiceDescription>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetGSTInvoiceDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        GSTInvoiceDescription oGSTInvoiceDetails = new GSTInvoiceDescription();
                        oGSTInvoiceDetails.Amount = oSqlDataReader["Amount"].ToInt();
                        oGSTInvoiceDetails.Description = oSqlDataReader["Description"].ToString();

                        lstGSTDetails.Add(oGSTInvoiceDetails);
                    }
                }
                return lstGSTDetails;
            }
        }

        /// <summary>
        /// This method is used to get GST Invoice details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asFilter"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<GSTInvoiceDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asFilter, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetGSTReceiverDetails"))
                {
                    List<GSTInvoiceDetails> lstInvoice = new List<GSTInvoiceDetails>();

                    List<GSTInvoiceDescription> lstGSTInvoiceDescription = new List<GSTInvoiceDescription>();
                    while (oSqlDataReader.Read())
                    {
                        lstGSTInvoiceDescription.Add(new GSTInvoiceDescription
                       {
                           Id = Convert.ToInt32(oSqlDataReader["Id"]),
                           Description = Convert.ToString(oSqlDataReader["Description"]),
                           Amount = Convert.ToInt32(oSqlDataReader["Amount"]),
                           GSTInvoiceId = Convert.ToInt32(oSqlDataReader["GSTInvoiceId"])
                       });
                    }

                    oSqlDataReader.NextResult();
                    while (oSqlDataReader.Read())
                    {
                        lstInvoice.Add(new GSTInvoiceDetails
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            GSTCategoryId = Convert.ToInt32(oSqlDataReader["GSTCategoryId"]),
                            ReceiverId = Convert.ToInt32(oSqlDataReader["ServiceReceiverId"]),
                            InvoiceNo = Convert.ToString(oSqlDataReader["InvoiceNo"]),
                            InvoiceDate = Convert.ToDateTime(oSqlDataReader["InvoiceDate"]),
                            TotalAmount = Convert.ToDecimal(oSqlDataReader["TotalAmount"]),
                            CGST = Convert.ToDecimal(oSqlDataReader["CGST"]),
                            SGST = Convert.ToDecimal(oSqlDataReader["SGST"]),
                            FinalAmount = Convert.ToDecimal(oSqlDataReader["FinalAmount"]),
                            TotalRows = oSqlDataReader["TotalRows"].ToInt(),
                            ReceiverName = Convert.ToString(oSqlDataReader["Name"]),
                            Descriptions = lstGSTInvoiceDescription.Where(dc => dc.GSTInvoiceId == Convert.ToInt32(oSqlDataReader["Id"])).ToList()
                        });
                    }
                    return lstInvoice;
                }
            }
        }

        /// <summary>
        /// This method is used to save GST Invoice Details.
        /// </summary>
        /// <param name="sXml"></param>
        /// <param name="oGSTInvoiceDetails"></param>
        public void Save(string sXml, GSTInvoiceDetails oGSTInvoiceDetails)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("GSTXmlDetails", sXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Id", oGSTInvoiceDetails.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ReceiverId", oGSTInvoiceDetails.ReceiverId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InvoiceNo", oGSTInvoiceDetails.InvoiceNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InvoiceDate", oGSTInvoiceDetails.InvoiceDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("TotalAmount", oGSTInvoiceDetails.TotalAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("GSTCategoryId", oGSTInvoiceDetails.GSTCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CGST", oGSTInvoiceDetails.CGST, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("SGST", oGSTInvoiceDetails.SGST, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("FinalAmount", oGSTInvoiceDetails.FinalAmount, SqlDbType.Decimal);
                oSQLServerDbUtility.AddParameter("AdditionalRemark", oGSTInvoiceDetails.AdditionalRemark, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveGSTInvoiceDetails");
            }
        }

        /// <summary>
        /// This method is used to return all GST Invoice details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public GSTInvoiceDetails Get(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                GSTInvoiceDetails oGSTInvoiceDetails = new GSTInvoiceDetails();
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDataToReadGSTInvoiceDetails"))
                {  
                    if (oSqlDataReader.Read())
                    {
                        oGSTInvoiceDetails.Id = oSqlDataReader["Id"].ToInt();
                        oGSTInvoiceDetails.ServiceReceiverId = oSqlDataReader["ServiceReceiverId"].ToInt();
                        oGSTInvoiceDetails.InvoiceNo = oSqlDataReader["InvoiceNo"].ToString();
                        oGSTInvoiceDetails.InvoiceDate = oSqlDataReader["InvoiceDate"].ToDateTime();
                        oGSTInvoiceDetails.GSTCategoryId = oSqlDataReader["GSTCategoryId"].ToInt();
                        oGSTInvoiceDetails.TotalAmount = oSqlDataReader["TotalAmount"].ToDecimal();
                        oGSTInvoiceDetails.CGST = oSqlDataReader["CGST"].ToDecimal();
                        oGSTInvoiceDetails.SGST = oSqlDataReader["SGST"].ToDecimal();
                        oGSTInvoiceDetails.FinalAmount = oSqlDataReader["FinalAmount"].ToDecimal();
                        oGSTInvoiceDetails.AdditionalRemark = oSqlDataReader["AdditionalRemark"].ToString();
                    }
                }

                return oGSTInvoiceDetails;
            }
        }

        /// <summary>
        /// This method is used to fill receiver name dropdown.
        /// </summary>
        /// <returns></returns>
        public List<ReceiverName> GetReceiverName()
        {
            List<ReceiverName> olstReceiverName = new List<ReceiverName>();
            string sSQLStatement = "SELECT Id, Name FROM [dbo].[ServiceReceiverDetails] WHERE IsDeleted = 0 and AcademicYearId = " + this.miAcademicYearId + " Order by Name";

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

        #endregion

        #region Private MEthod(s)

        /// <summary>
        /// This method is used to set receiver details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private GSTInvoiceDetails SetReceiverDetails(SqlDataReader aoSqlDataReader)
        {
            return new GSTInvoiceDetails
            {
                Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                GSTCategoryId = Convert.ToInt32(aoSqlDataReader["GSTCategoryId"]),
                ReceiverId = Convert.ToInt32(aoSqlDataReader["ServiceReceiverId"]),
                InvoiceNo = Convert.ToString(aoSqlDataReader["InvoiceNo"]),
                InvoiceDate = Convert.ToDateTime(aoSqlDataReader["InvoiceDate"]),
                TotalAmount = Convert.ToDecimal(aoSqlDataReader["TotalAmount"]),
                TotalRows = aoSqlDataReader["TotalRows"].ToInt(),
                ReceiverName = Convert.ToString(aoSqlDataReader["Name"])
            };
        } 

        #endregion
    }
}
