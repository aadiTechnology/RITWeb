// Class Name       :- ImportClearanceDetailsBL
// Purpose          :- Business logic for importing MIS clearance details from excel file.
// Date Of creation :- 21 May 2026
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Utility;

namespace BusinessLogic
{
    /// <summary>
    /// Result of save operation for import clearance details.
    /// </summary>
    public class ImportClearanceSaveResult
    {
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }

    /// <summary>
    /// Business logic for importing online transaction clearance details from excel.
    /// </summary>
    public class ImportClearanceDetailsBL
    {
        #region Constants

        private const string S_SHEET_NAME = "Easypay";
        private const string S_ELEMENT = "element";
        private const string S_NETBANKING_PAYMENT_TRANSACTION_ID_ATTR = "NetBankingPaymentTransactionID";
        private const string S_TRANSACTION_ID_LIST = "TransactionIdList";
        private const string S_COLUMN_TRANSACTION_ID = "Transaction Id";
        private const string S_COLUMN_SETTLED_DATE = "Settled_date";
        private const string S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID = "NetBankingPaymentTransactionID";
        private const string S_COLUMN_TPSL_TRANSACTION_ID = "TPSLTransactionID";
        private const string S_COLUMN_DEPOSITEDBANKID = "DepositedBankId";

        private static readonly string[] S_REQUIRED_COLUMNS = new[]
        {
            "Merchant Id",
            S_COLUMN_TRANSACTION_ID,
            "Transaction From",
            "Amount",
            S_COLUMN_SETTLED_DATE
        };

        #endregion Constants

        #region Member(s)

        private readonly int miSchoolId;
        private readonly int miAcademicYearId;
        private readonly int miFinancialYearId;
        private readonly int miUserId;
        private readonly bool mbIsAccountsModuleEnabled;

        #endregion Member(s)

        #region Constructor(s)

        public ImportClearanceDetailsBL(int aiSchoolId, int aiAcademicYearId, int aiFinancialYearId, int aiUserId, bool abIsAccountsModuleEnabled)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            miFinancialYearId = aiFinancialYearId;
            miUserId = aiUserId;
            mbIsAccountsModuleEnabled = abIsAccountsModuleEnabled;
        }

        #endregion Constructor(s)

        #region Public Method(s)

        /// <summary>
        /// Validates uploaded file name and extension.
        /// </summary>
        public string ValidateUploadedFile(bool abHasFile, string asFileName)
        {
            if (!abHasFile)
                return "Please select file to import.";

            string sExtension = Path.GetExtension(asFileName).ToLower();
            if (sExtension != ".xls" && sExtension != ".xlsx")
                return "File should be only in xls or xlsx format.";

            return string.Empty;
        }

        /// <summary>
        /// Validates excel dataset for sheet data, required columns and blank file.
        /// </summary>
        public string ValidateExcelData(DataSet aoDSExcelData)
        {
            if (aoDSExcelData == null || aoDSExcelData.Tables.Count == 0)
                return "File should not be blank.";

            DataTable odtExcelData = aoDSExcelData.Tables[0];
            odtExcelData = CommonUtility.DeleteEmptyRows(odtExcelData);

            if (odtExcelData == null || odtExcelData.Rows.Count == 0)
                return "File should not be blank.";

            if (!HasRequiredColumns(odtExcelData))
                return "File should contains all required columns (Merchant Id, Transaction Id, Transaction From, Amount, Settled_date).";

            return string.Empty;
        }

        /// <summary>
        /// Creates XML for transaction ids present in excel file.
        /// </summary>
        public string GenerateTransactionIdsXml(DataTable aoDataTable)
        {
            var oDoc = new XmlDocument();
            XmlElement oRoot = oDoc.CreateElement(S_TRANSACTION_ID_LIST);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, S_TRANSACTION_ID_LIST, string.Empty);

            foreach (DataRow oDataRow in aoDataTable.Rows)
            {
                string sTransactionId = GetColumnValue(oDataRow, S_COLUMN_TRANSACTION_ID);
                if (string.IsNullOrEmpty(sTransactionId))
                    continue;

                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "Transaction", string.Empty);
                XmlAttribute oAttr = oDoc.CreateAttribute(S_NETBANKING_PAYMENT_TRANSACTION_ID_ATTR);
                oAttr.Value = sTransactionId;
                oXmlNode.Attributes.Append(oAttr);
                oXmlRootNode.AppendChild(oXmlNode);
            }

            oRoot.AppendChild(oXmlRootNode);
            return oRoot.InnerXml;
        }

        /// <summary>
        /// Validates transaction ids from excel against database records.
        /// </summary>
        public string ValidateTransactionIds(DataTable aoExcelData, DataTable aoDbTransactionDetails)
        {
            if (aoDbTransactionDetails == null || aoDbTransactionDetails.Rows.Count == 0)
                return "All transaction IDs present in the file either do not match the system records or have already been cleared.";

            string sMerchantId = aoDbTransactionDetails.Rows[0]["MerchantId"].ToString();
            List<string> lstInvalidMerchantIds = aoExcelData.AsEnumerable().Where(rs => Convert.ToString(rs["Merchant Id"]) != sMerchantId).Select(rs => Convert.ToString(rs["Transaction Id"])).ToList();
            if (lstInvalidMerchantIds.Count > 0)
                return "Please set correct Merchant Id in excel file for Transaction Ids : " + string.Join(",", lstInvalidMerchantIds);

            List<string> lstAllowedFroms = new List<string> { "STUDENTFEE", "INTERNALFEE", "CAUTIONMONEY", "ADMISSION" };
            List<string> lstInvalidTxnFrom = aoExcelData.AsEnumerable().Where(rs => !lstAllowedFroms.Contains(Convert.ToString(rs["Transaction From"]).ToUpper())).Select(rs => Convert.ToString(rs["Transaction Id"])).ToList();
            if (lstInvalidTxnFrom.Count > 0)
                return "Only StudentFee, InternalFee and CautionMoney is allowed in 'Transaction From' : " + string.Join(",", lstInvalidTxnFrom);

            var oMatchedTxnIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow oDbRow in aoDbTransactionDetails.Rows)
            {
                string sTxnId = GetDataTableColumnValue(oDbRow, S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID);
                if (!string.IsNullOrEmpty(sTxnId))
                    oMatchedTxnIds.Add(sTxnId);
            }

            var oInvalidTxnIds = new List<string>();
            foreach (DataRow oExcelRow in aoExcelData.Rows)
            {
                string sTransactionId = GetColumnValue(oExcelRow, S_COLUMN_TRANSACTION_ID);
                if (string.IsNullOrEmpty(sTransactionId))
                    continue;

                if (!oMatchedTxnIds.Contains(sTransactionId))
                    oInvalidTxnIds.Add(sTransactionId);
            }

            if (oInvalidTxnIds.Count > 0)
            {
                var oErrorMessage = new StringBuilder();
                oErrorMessage.Append("The transaction ID present in the file either does not match the system records or has already been cleared for : ");
                oErrorMessage.Append(string.Join(", ", oInvalidTxnIds.ToArray()));
                oErrorMessage.Append(".");
                return oErrorMessage.ToString();
            }

            var mismatchedTransactionIds = (
                    from excel in aoExcelData.AsEnumerable()
                    join data in aoDbTransactionDetails.AsEnumerable()
                    on Convert.ToString(excel["Transaction Id"])
                       equals Convert.ToString(data["NetBankingPaymentTransactionID"])
                    where Convert.ToDecimal(excel["Amount"]) != Convert.ToDecimal(data["Amount"])
                    select Convert.ToString(excel["Transaction Id"])
                ).ToList();

            if (mismatchedTransactionIds.Count > 0)
                return "Amount is not matching for Transaction Ids : " + string.Join(",", mismatchedTransactionIds);

            return string.Empty;
        }

        /// <summary>
        /// Saves online transactions imported from excel for clearance.
        /// </summary>
        public ImportClearanceSaveResult SaveOnlineTrasactionPayments(DataTable aoExcelData, DataTable aoTransactionDetails, Action<string, bool> recordPayment)
        {
            var oResult = new ImportClearanceSaveResult();
            var oFeeData = aoExcelData.AsEnumerable().Where(dr => dr.Field<string>("Transaction From").ToUpper() == "STUDENTFEE" || dr.Field<string>("Transaction From").ToUpper() == "CAUTIONMONEY" || dr.Field<string>("Transaction From").ToUpper() == "INTERNALFEE");

            List<string> alstNonUpdatedTxnNos = new List<string>();
            var oUpdateNetBankingPaymentTransactions = new NetBankingPaymentTransactionsBL();
            if (oFeeData != null && oFeeData.Count() > 0)
            {
                DataTable dtData = oFeeData.CopyToDataTable();
                string sOnlineTrasactionXML = GenerateOnlineTransactionXML(dtData, aoTransactionDetails, out alstNonUpdatedTxnNos);

                if (alstNonUpdatedTxnNos.Count > 0)
                {
                    oResult.ErrorMessage = "These Transaction Nos are not matched with system record : " + string.Join(",", alstNonUpdatedTxnNos);
                    return oResult;
                }

                oUpdateNetBankingPaymentTransactions.SetOnlineTransactionDetails(sOnlineTrasactionXML);

                if (mbIsAccountsModuleEnabled && recordPayment != null)
                {
                    int iCount = dtData.AsEnumerable().Count(dr => dr.Field<string>("Transaction From").ToUpper() == "STUDENTFEE" || dr.Field<string>("Transaction From").ToUpper() == "CAUTIONMONEY");
                    if (iCount > 0)
                    {
                        string sDayBookXml = GetXMLFromGrid(dtData, aoTransactionDetails);
                        recordPayment(sDayBookXml, false);
                    }
                }
            }

            var oAdmissionData = aoExcelData.AsEnumerable().Where(dr => dr.Field<string>("Transaction From").ToUpper() == "ADMISSION");

            if (oAdmissionData != null && oAdmissionData.Count() > 0)
            {
                DataTable dtAdmission = oAdmissionData.CopyToDataTable();
                List<string> alstNonUpdatedAdmissionTxnNos;
                string sAdmissionXml = GetAdmissionXml(dtAdmission, aoTransactionDetails, out alstNonUpdatedAdmissionTxnNos);

                if (alstNonUpdatedAdmissionTxnNos.Count > 0)
                {
                    oResult.ErrorMessage = "These Admission Transaction Nos are not matched with system record : " + string.Join(",", alstNonUpdatedAdmissionTxnNos);
                    return oResult;
                }

                oUpdateNetBankingPaymentTransactions.SetOnlineAdmissionFeeDetails(sAdmissionXml, miSchoolId, miAcademicYearId);
                if (mbIsAccountsModuleEnabled && recordPayment != null)
                {
                    string sDayBookAdmissionXml = GetAdmissionXml(dtAdmission, aoTransactionDetails, out alstNonUpdatedAdmissionTxnNos);
                    recordPayment(sDayBookAdmissionXml, true);
                }
            }

            if (alstNonUpdatedTxnNos.Count == 0)
                oResult.SuccessMessage = "Online transaction clearance data updated successfully !!!";
            else
                oResult.SuccessMessage = "Online transaction clearance data updated successfully !!!. Following transactions are not imported : \n" + string.Join(",", alstNonUpdatedTxnNos);

            return oResult;
        }

        /// <summary>
        /// Returns admission related XML.
        /// </summary>
        public string GetAdmissionXml(DataTable aoExcelData, DataTable aoTransactionDetails, out List<string> alstNonUpdatedAdmissionTxnNos)
        {
            const string S_ELEMENT_NAME = "element";
            string sAttribute;
            var oDoc = new XmlDocument();
            XmlElement oElement = oDoc.CreateElement("OnlineAdmissionFeeInfo");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT_NAME, "OnlineAdmissionFeeInfo", String.Empty);
            Dictionary<string, DataRow> oTransactionDetailsLookup = BuildTransactionDetailsLookup(aoTransactionDetails);
            alstNonUpdatedAdmissionTxnNos = new List<string>();
            foreach (DataRow oExcelRow in aoExcelData.Rows)
            {
                string sTransactionId = GetColumnValue(oExcelRow, S_COLUMN_TRANSACTION_ID);
                if (string.IsNullOrEmpty(sTransactionId) || !oTransactionDetailsLookup.ContainsKey(sTransactionId))
                {
                    alstNonUpdatedAdmissionTxnNos.Add(sTransactionId);
                    continue;
                }

                DataRow oDbRow = oTransactionDetailsLookup[sTransactionId];

                XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT_NAME, "OnlineAdmissionFeeInfo", String.Empty);

                sAttribute = S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID;
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = GetDataTableColumnValue(oDbRow, S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID);
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = S_COLUMN_TPSL_TRANSACTION_ID;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = GetDataTableColumnValue(oDbRow, S_COLUMN_TPSL_TRANSACTION_ID);
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "Update_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = DateTime.Now.ToString();
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "Updated_By_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = miUserId.ToString();
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "ClearanceDate";
                oAttr = oDoc.CreateAttribute(sAttribute);
                string sClearanceDate = GetSettledDateValue(oExcelRow);
                oAttr.Value = !string.IsNullOrEmpty(sClearanceDate) ? sClearanceDate : DBNull.Value.ToString();
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "DepositeBankId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = GetDataTableColumnValue(oDbRow, S_COLUMN_DEPOSITEDBANKID);
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "FormNumber";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = GetDataTableColumnValue(oDbRow, "FormNumber");
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "StudentAdmissionId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = GetDataTableColumnValue(oDbRow, "StudentAdmissionId");
                oXMLNode.Attributes.Append(oAttr);

                oXmlRootNode.AppendChild(oXMLNode);
            }

            oElement.AppendChild(oXmlRootNode);
            return oElement.InnerXml;
        }

        /// <summary>
        /// Returns day book related XML.
        /// </summary>
        public string GetXMLFromGrid(DataTable aoExcelData, DataTable aoTransactionDetails)
        {
            const string S_ELEMENT_NAME = "element";
            string sAttribute;
            var oDoc = new XmlDocument();
            XmlElement oElement = oDoc.CreateElement("ClearanceInfo");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT_NAME, "ClearanceInfo", String.Empty);
            Dictionary<string, DataRow> oTransactionDetailsLookup = BuildTransactionDetailsLookup(aoTransactionDetails);

            foreach (DataRow oExcelRow in aoExcelData.Rows)
            {
                string sTxnFrom = oExcelRow["Transaction From"].ToString().Trim().ToUpper();

                if (sTxnFrom == "STUDENTFEE" || sTxnFrom == "CAUTIONMONEY")
                {
                    string sTransactionId = GetColumnValue(oExcelRow, S_COLUMN_TRANSACTION_ID);
                    if (string.IsNullOrEmpty(sTransactionId) || !oTransactionDetailsLookup.ContainsKey(sTransactionId))
                        continue;

                    DataRow oDbRow = oTransactionDetailsLookup[sTransactionId];
                    XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT_NAME, "ClearanceInfo", String.Empty);

                    sAttribute = "TransId";
                    XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                    oAttr.Value = GetDataTableColumnValue(oDbRow, S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID);
                    oXMLNode.Attributes.Append(oAttr);

                    oAttr = oDoc.CreateAttribute("ClearanceDate");
                    string sClearanceDate = GetSettledDateValue(oExcelRow);
                    oAttr.Value = !string.IsNullOrEmpty(sClearanceDate) ? sClearanceDate : DBNull.Value.ToString();
                    oXMLNode.Attributes.Append(oAttr);

                    oAttr = oDoc.CreateAttribute("DepositBankId");
                    oAttr.Value = GetDataTableColumnValue(oDbRow, S_COLUMN_DEPOSITEDBANKID);
                    oXMLNode.Attributes.Append(oAttr);

                    oAttr = oDoc.CreateAttribute("IsCautionMoney");
                    oAttr.Value = sTxnFrom == "CAUTIONMONEY" ? Constants.S_ONE : Constants.S_ZERO;
                    oXMLNode.Attributes.Append(oAttr);

                    oAttr = oDoc.CreateAttribute("IsReturnPayment");
                    oAttr.Value = Constants.S_ZERO;
                    oXMLNode.Attributes.Append(oAttr);

                    oAttr = oDoc.CreateAttribute("IsElectronicPayment");
                    oAttr.Value = Constants.S_ZERO;
                    oXMLNode.Attributes.Append(oAttr);

                    oXmlRootNode.AppendChild(oXMLNode);
                }
            }

            oElement.AppendChild(oXmlRootNode);
            return oElement.InnerXml;
        }

        /// <summary>
        /// Creates an XML string for online transaction details from excel and database data.
        /// </summary>
        public string GenerateOnlineTransactionXML(DataTable aoExcelData, DataTable aoTransactionDetails, out List<string> alstNonUpdatedTxnNos)
        {
            const string S_ELEMENT_NAME = "element";
            string sAttribute;
            var oDoc = new XmlDocument();
            XmlElement oElement = oDoc.CreateElement("OnlineTrasactionInfo");
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT_NAME, "OnlineTrasactionInfo", String.Empty);
            Dictionary<string, DataRow> oTransactionDetailsLookup = BuildTransactionDetailsLookup(aoTransactionDetails);
            alstNonUpdatedTxnNos = new List<string>();
            foreach (DataRow oExcelRow in aoExcelData.Rows)
            {
                string sTransactionId = GetColumnValue(oExcelRow, S_COLUMN_TRANSACTION_ID);
                if (string.IsNullOrEmpty(sTransactionId) || !oTransactionDetailsLookup.ContainsKey(sTransactionId))
                {
                    alstNonUpdatedTxnNos.Add(sTransactionId);
                    continue;
                }

                DataRow oDbRow = oTransactionDetailsLookup[sTransactionId];

                XmlNode oXMLNode = oDoc.CreateNode(S_ELEMENT_NAME, "OnlineTrasactionInfo", String.Empty);

                sAttribute = S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID;
                XmlAttribute oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = GetDataTableColumnValue(oDbRow, S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID);
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = S_COLUMN_TPSL_TRANSACTION_ID;
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = GetDataTableColumnValue(oDbRow, S_COLUMN_TPSL_TRANSACTION_ID);
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "Update_Date";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = DateTime.Now.ToString();
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "Updated_By_Id";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = miUserId.ToString();
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "ClearanceDate";
                oAttr = oDoc.CreateAttribute(sAttribute);
                string sClearanceDate = GetSettledDateValue(oExcelRow);
                oAttr.Value = !string.IsNullOrEmpty(sClearanceDate) ? sClearanceDate : DBNull.Value.ToString();
                oXMLNode.Attributes.Append(oAttr);

                sAttribute = "DepositBankId";
                oAttr = oDoc.CreateAttribute(sAttribute);
                oAttr.Value = GetDataTableColumnValue(oDbRow, S_COLUMN_DEPOSITEDBANKID);
                oXMLNode.Attributes.Append(oAttr);

                oXmlRootNode.AppendChild(oXMLNode);
            }

            oElement.AppendChild(oXmlRootNode);
            return oElement.InnerXml;
        }

        #endregion Public Method(s)

        #region Private Method(s)

        private Dictionary<string, DataRow> BuildTransactionDetailsLookup(DataTable aoTransactionDetails)
        {
            var oLookup = new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow oRow in aoTransactionDetails.Rows)
            {
                string sTxnId = GetDataTableColumnValue(oRow, S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID);
                if (!string.IsNullOrEmpty(sTxnId) && !oLookup.ContainsKey(sTxnId))
                    oLookup[sTxnId] = oRow;
            }
            return oLookup;
        }

        private string GetSettledDateValue(DataRow aoExcelRow)
        {
            foreach (DataColumn oColumn in aoExcelRow.Table.Columns)
            {
                if (!string.Equals(oColumn.ColumnName.Trim(), S_COLUMN_SETTLED_DATE, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (aoExcelRow[oColumn.ColumnName] == DBNull.Value)
                    return string.Empty;

                if (aoExcelRow[oColumn.ColumnName] is DateTime)
                    return (Convert.ToDateTime(aoExcelRow[oColumn.ColumnName])).ToString("dd-MMM-yyyy", CultureInfo.GetCultureInfo("en"));

                if (oColumn.ColumnName.ToUpper() == "SETTLED_DATE")
                {
                    string[] formats = { "dd-MM-yyyy", "dd-MMM-yyyy", "yyyy-MM-dd", "dd-MM-yyyy HH:mm:ss", "dd-MMM-yyyy HH:mm:ss" };
                    DateTime dtSettledDate;
                    if (DateTime.TryParseExact(aoExcelRow[oColumn.ColumnName].ToString(),formats,CultureInfo.InvariantCulture,DateTimeStyles.None,out dtSettledDate))
                    {
                    }
                    else
                        return string.Empty;

                    return dtSettledDate.ToString("dd-MMM-yyyy", CultureInfo.GetCultureInfo("en"));
                }
                return Convert.ToString(aoExcelRow[oColumn.ColumnName]).Trim();
            }
            return string.Empty;
        }

        private bool HasRequiredColumns(DataTable aoDataTable)
        {
            foreach (string sColumnName in S_REQUIRED_COLUMNS)
            {
                if (!ColumnExists(aoDataTable, sColumnName))
                    return false;
            }
            return true;
        }

        private bool ColumnExists(DataTable aoDataTable, string asColumnName)
        {
            foreach (DataColumn oColumn in aoDataTable.Columns)
            {
                if (string.Equals(oColumn.ColumnName.Trim(), asColumnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private string GetColumnValue(DataRow aoDataRow, string asColumnName)
        {
            foreach (DataColumn oColumn in aoDataRow.Table.Columns)
            {
                if (string.Equals(oColumn.ColumnName.Trim(), asColumnName, StringComparison.OrdinalIgnoreCase))
                    return Convert.ToString(aoDataRow[oColumn.ColumnName]).Trim();
            }
            return string.Empty;
        }

        private string GetDataTableColumnValue(DataRow aoDataRow, string asColumnName)
        {
            foreach (DataColumn oColumn in aoDataRow.Table.Columns)
            {
                if (string.Equals(oColumn.ColumnName.Trim(), asColumnName, StringComparison.OrdinalIgnoreCase))
                    return Convert.ToString(aoDataRow[oColumn.ColumnName]).Trim();
            }
            return string.Empty;
        }

        #endregion Private Method(s)
    }
}
