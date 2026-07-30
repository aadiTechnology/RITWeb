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
        private const string S_COLUMN_TRANSACTION_ID = "order_receipt";
        private const string S_COLUMN_SETTLED_DATE = "settled_at";
        private const string S_COLUMN_NETBANKING_PAYMENT_TRANSACTION_ID = "NetBankingPaymentTransactionID";
        private const string S_COLUMN_TPSL_TRANSACTION_ID = "TPSLTransactionID";
        private const string S_COLUMN_DEPOSITEDBANKID = "DepositedBankId";

        private static readonly string[] S_REQUIRED_COLUMNS = new[]
        {
            S_COLUMN_TRANSACTION_ID,
            "amount",
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
                return "File should contains all required columns (order_receipt, amount, settled_at).";

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
                    on GetColumnValue(excel, S_COLUMN_TRANSACTION_ID)
                       equals Convert.ToString(data["NetBankingPaymentTransactionID"])
                    where Convert.ToDecimal(GetColumnValue(excel, "amount")) != Convert.ToDecimal(data["Amount"])
                    select GetColumnValue(excel, S_COLUMN_TRANSACTION_ID)
                ).ToList();

            if (mismatchedTransactionIds.Count > 0)
                return "Amount is not matching for Transaction Ids : " + string.Join(",", mismatchedTransactionIds);

            return string.Empty;
        }

        /// <summary>
        /// Saves online transactions imported from excel for clearance.
        /// Routes transactions based on DB Transaction_From column instead of Excel column.
        /// </summary>
        public ImportClearanceSaveResult SaveOnlineTrasactionPayments(DataTable aoExcelData, DataTable aoTransactionDetails, Action<string, bool> recordPayment)
        {
            var oResult = new ImportClearanceSaveResult();
            var alstNonUpdatedTxnNos = new List<string>();
            var alstInvalidTxnFrom = new List<string>();
            var oTransactionDetailsLookup = BuildTransactionDetailsLookup(aoTransactionDetails);

            DataTable dtFeeData = aoExcelData.Clone();
            DataTable dtAdmissionData = aoExcelData.Clone();

            foreach (DataRow oExcelRow in aoExcelData.Rows)
            {
                string sTransactionId = GetColumnValue(oExcelRow, S_COLUMN_TRANSACTION_ID);
                if (string.IsNullOrEmpty(sTransactionId) || !oTransactionDetailsLookup.ContainsKey(sTransactionId))
                {
                    alstNonUpdatedTxnNos.Add(sTransactionId);
                    continue;
                }

                DataRow oDbRow = oTransactionDetailsLookup[sTransactionId];
                string sTxnFrom = GetDataTableColumnValue(oDbRow, "Transaction_From").ToUpper();

                if (sTxnFrom == "STUDENTFEE" || sTxnFrom == "INTERNALFEE" || sTxnFrom == "CAUTIONMONEY")
                {
                    dtFeeData.ImportRow(oExcelRow);
                }
                else if (sTxnFrom == "ADMISSION")
                {
                    dtAdmissionData.ImportRow(oExcelRow);
                }
                else
                {
                    alstInvalidTxnFrom.Add(sTransactionId);
                }
            }

            if (alstInvalidTxnFrom.Count > 0)
            {
                oResult.ErrorMessage = "Invalid Transaction_From value for Transaction Ids : " + string.Join(",", alstInvalidTxnFrom);
                return oResult;
            }

            var oUpdateNetBankingPaymentTransactions = new NetBankingPaymentTransactionsBL();

            if (dtFeeData.Rows.Count > 0)
            {
                List<string> alstNonUpdatedFeeTxnNos;
                string sOnlineTrasactionXML = GenerateOnlineTransactionXML(dtFeeData, aoTransactionDetails, out alstNonUpdatedFeeTxnNos);

                if (alstNonUpdatedFeeTxnNos.Count > 0)
                {
                    oResult.ErrorMessage = "These Transaction Nos are not matched with system record : " + string.Join(",", alstNonUpdatedFeeTxnNos);
                    return oResult;
                }

                oUpdateNetBankingPaymentTransactions.SetOnlineTransactionDetails(sOnlineTrasactionXML);

                if (mbIsAccountsModuleEnabled && recordPayment != null)
                {
                    string sDayBookXml = GetXMLFromGrid(dtFeeData, aoTransactionDetails);
                    if (!string.IsNullOrEmpty(sDayBookXml))
                        recordPayment(sDayBookXml, false);
                }
            }

            if (dtAdmissionData.Rows.Count > 0)
            {
                List<string> alstNonUpdatedAdmissionTxnNos;
                string sAdmissionXml = GetAdmissionXml(dtAdmissionData, aoTransactionDetails, out alstNonUpdatedAdmissionTxnNos);

                if (alstNonUpdatedAdmissionTxnNos.Count > 0)
                {
                    oResult.ErrorMessage = "These Admission Transaction Nos are not matched with system record : " + string.Join(",", alstNonUpdatedAdmissionTxnNos);
                    return oResult;
                }

                oUpdateNetBankingPaymentTransactions.SetOnlineAdmissionFeeDetails(sAdmissionXml, miSchoolId, miAcademicYearId);
                if (mbIsAccountsModuleEnabled && recordPayment != null)
                {
                    string sDayBookAdmissionXml = GetAdmissionXml(dtAdmissionData, aoTransactionDetails, out alstNonUpdatedAdmissionTxnNos);
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
                string sTransactionId = GetColumnValue(oExcelRow, S_COLUMN_TRANSACTION_ID);
                if (string.IsNullOrEmpty(sTransactionId) || !oTransactionDetailsLookup.ContainsKey(sTransactionId))
                    continue;

                DataRow oDbRow = oTransactionDetailsLookup[sTransactionId];
                string sTxnFrom = GetDataTableColumnValue(oDbRow, "Transaction_From").ToUpper();

                if (sTxnFrom == "STUDENTFEE" || sTxnFrom == "CAUTIONMONEY")
                {
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

                if (oColumn.ColumnName.ToUpper() == "SETTLED_AT")
                {
                    string[] formats = { "dd-MM-yyyy", "dd-MMM-yyyy", "yyyy-MM-dd", "dd-MM-yyyy HH:mm:ss", "dd-MMM-yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss" };
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
