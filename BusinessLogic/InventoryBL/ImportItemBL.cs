using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections;
using Utility;
using DataCommunicator;

namespace BusinessLogic
{
    public class ImportItemBL
    {
        #region  " Constants "

        /// <summary>
        /// Purpose :To upload items through excel sheet and save them in item Master.
        /// Validate all the field from excel sheet,
        /// also check duplication and finally save change to the database.
        /// </summary>
        const string S_EXCEL_FILE_MESSAGE = "Please select the excel file only. Select another file to upload.";
        const string S_DUPLICATE_ITEM_NAME = "Item already exists in inventory. Please enter another item in worksheet at row number : ";
        const string S_DUPLICATE_ITEM_NAME_IN_FILE = "Item name duplicate in worksheet. Please enter another item in worksheet at row number : ";
        const string S_DUPLICATE_ITEM_CODE = "Item code already exist in inventory. Please enter another item code in worksheet at row number : ";
        const string S_DUPLICATE_ITEM_CODE_IN_FILE = "Item code duplicate in worksheet. Please enter another item code in worksheet at row number : ";
        const string S_NULL_ITEM_CODE = "Item code should not be blank in worksheet at row number : ";
        const string S_NULL_ITEM_NAME = "Item name should not be blank in worksheet at row number : ";
        const string S_NULL_ITEM_QUANTITY = "Item quantity should not be blank in worksheet at row number : ";
        const string S_NULL_UOM = "Unit of measurement should not be blank in worksheet at row number : ";
        const string S_NULL_ITEM_CATEGORY = "Item category should not be blank in worksheet at row number : ";
        const string S_NULL_ITEM_REORDER_LVL = "Item reorder level should not be blank in worksheet at row number : ";
        const string S_NULL_ITEM_MAKE = "Make should not be blank in worksheet at row number : ";
        const string S_INVALID_UOM = "Please select valid unit of measurement in worksheet at row number : ";
        const string S_INVALID_ITEM_CATEGORY = "Please select valid item category in worksheet at row number : ";
        const string S_INVALID_ITEM_QUANTITY = "Please insert valid item quantity in worksheet at row number : ";
        const string S_INVALID_ITEM_REORDER_LEVEL = "Please insert valid item reorder level in worksheet at row number : ";
        const string S_NO_RECORD_FOUND = "File to be imported should not be empty.";
        const string S_NO_PROPER_FORMAT = "Data in uploaded file is not in correct format.";

        const string S_NULL_ITEM_UNITCOUNTOFUOM = "Unit Count of UOM should not be blank in worksheet at row number : ";
        const string S_NULL_ITEM_ISCONSIDERFORDETAILLEVEL = "Value of 'Consider For Detail Level?' should not be blank in worksheet at row number : ";
        const string S_NULL_ITEM_UNITCOUNT_INCORRECT = "Unit Count of UOM should be same as Unit Count of first entry of same UOM in worksheet at row number : ";
        const string S_NULL_ITEM_IS_UNIT = "Value of 'Is Quantity in Unit?' should not be blank in worksheet at row number : ";
        const string S_NULL_ITEM_IS_UNIT_REORDER = "Value of 'Is Reorder Level in Unit?' should not be blank in worksheet at row number : ";

        const int I_XLS_ITEM_CODE = 0;
        const int I_XLS_ITEM_NAME = 1;
        const int I_XLS_UOM = 2;
        const int I_XLS_ITEM_CATEGORY = 3;
        const int I_XLS_ITEM_QUANTITY = 4;
        const int I_XLS_ITEM_REORDER_LVL = 5;
        const int I_XLS_ITEM_MAKE = 6;
        const int I_XLS_NO_RECORD_FOUND = 12;
        const int I_XLS_NO_PROPER_FORMAT = 13;

        const int I_XLS_ITEM_UNITCOUNTOFUOM = 7;
        const int I_XLS_ITEM_ISCONSIDERFORDETAILLEVEL = 8;
        const int I_XLS_ITEM_IS_UNIT = 10;
        const int I_XLS_ITEM_IS_UNIT_REORDER = 11;
        
        //Table Indices
        const int I_TBL_ITEM_UOM = 0;
        const int I_TBL_ITEM_CATEGORY = 1;

        #endregion " Constants "

        #region " Data Members "

        private string msSourceFileName = string.Empty;
        private string msServerFilePath = string.Empty;
        private struct ItemInfo
        {
            public int iSchoolId;
            public int iAcademicYearId;
            public int iUserId;
        };

        ItemInfo moItemInfoStruct;

        #endregion " Data Members "

        #region " Properties "

        public int SchoolId
        {
            set { moItemInfoStruct.iSchoolId = value; }
        }

        public int AcademicYearId
        {
            set { moItemInfoStruct.iAcademicYearId = value; }
        }

        public int UserId
        {
            set { moItemInfoStruct.iUserId = value; }
        }

        #endregion " Properties "

        #region " Private Methods "

        /// <summary>
        /// Constructor will accept the excel file name containing the item list.
        /// </summary>
        /// <param name="asSourceFileName"></param>
        /// <param name="asServerFolderPath"></param>
        public ImportItemBL(string asSourceFileName, string asServerFolderPath)
        {
            msSourceFileName = asSourceFileName;
            msServerFilePath = asServerFolderPath;

            if (!(IsValidFileExtension()))
            {
                Exception ex = new Exception(S_EXCEL_FILE_MESSAGE);
                throw ex;
            }
        }

        /// <summary>
        /// This function will upload the excel sheet on the server.
        /// This will also store all the Item details in the database table.
        /// </summary>
        /// <param name="abSetAutoCode"></param>
        /// <returns></returns>
        public string UploadFile(bool abSetAutoCode)
        {
            // Validate the uploaded file.
            if (!(IsValidFileExtension()))
            {
                return S_EXCEL_FILE_MESSAGE;
            }
            // Item file upload. Save records from excel sheet to database.
            // Get dataset containing item details
            DataSet oDSItemDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Item Data");

            // Check if data is loaded in dataset successfully.
            if (oDSItemDetails != null && oDSItemDetails.Tables.Count>0)
            {
                DataTable oDTItems = oDSItemDetails.Tables[0];
                oDTItems = CommonUtility.DeleteEmptyRows(oDTItems);
                if (oDSItemDetails.Tables[0].Rows.Count > 0)
                {
                    string sItemDetails = GetXMLStringFromXLSRows(oDTItems, "ItemDetails", "ItemDetails", abSetAutoCode);

                    ItemCollectionBL oItemCollectionBL = new ItemCollectionBL(moItemInfoStruct.iSchoolId, moItemInfoStruct.iAcademicYearId,
                                                            moItemInfoStruct.iUserId);
                    oItemCollectionBL.InsertMultipleItems(sItemDetails, abSetAutoCode);
                }
                else
                    ThrowInvalidItemDataException(12, "0");
            }
            else
                ThrowInvalidItemDataException(13, "0");

            return "";
        }

        /// <summary>
        /// This function checks if the extention of the file to be uploaded is .XLS
        /// Only excel files can be uploaded for file upload.
        /// </summary>
        /// <returns></returns>
        private bool IsValidFileExtension()
        {
            return (msSourceFileName.ToUpper().EndsWith(".XLS") || msSourceFileName.ToUpper().EndsWith(".XLSX"));
        }

        /// <summary>
        /// This method is used to create XML to add item details to the database.
        /// </summary>
        /// <param name="aoDTItemDetails"></param>
        /// <param name="asRootElementName"></param>
        /// <param name="asElementName"></param>
        /// <param name="abSetAutoCode"></param>
        /// <returns></returns>
        public string GetXMLStringFromXLSRows(DataTable aoDTItemDetails, string asRootElementName, string asElementName, bool abSetAutoCode)
        {
            const string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("Item_Code");
            oArrayList.Add("Item_Name");
            oArrayList.Add("UOM");
            oArrayList.Add("Item_Category");
            oArrayList.Add("Item_Quantity");
            oArrayList.Add("Item_Reorder_Level");
            oArrayList.Add("Make");
            oArrayList.Add("Units_Of_UOM");
            oArrayList.Add("COnsider_For_Details_Level");
            oArrayList.Add("Price");
            oArrayList.Add("Is_Quantity_In_Unit");
            oArrayList.Add("Is_Reorder_Level_In_Unit");
            oArrayList.Add("SNo");
            oArrayList.Add("Hall");
            oArrayList.Add("RackNo");
            oArrayList.Add("ShelfNo");
            
            if (CheckForMandatoryFields(aoDTItemDetails, abSetAutoCode))
            {
                // Loop through all the grid rows.
                for (int iRowCount = 0; iRowCount <= aoDTItemDetails.Rows.Count - 1; iRowCount++)
                {

                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
                    DataRow oDataRow = aoDTItemDetails.Rows[iRowCount];

                    // Loop through all the columns for the row.
                    string sItemName = "";
                    sItemName = aoDTItemDetails.Rows[iRowCount][I_XLS_ITEM_NAME].ToString();

                    for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                    {
                        sAtrrName = oArrayList[iCount].ToString();
                        attr = oDoc.CreateAttribute(sAtrrName);
                        if (sAtrrName == "SNo")
                        {
                            string sRowNo = iRowCount.ToString();
                            attr.Value = sRowNo;
                        }
                        else if (sAtrrName == "Price")
                        {
                            if (aoDTItemDetails.Rows[iRowCount][iCount] == DBNull.Value || aoDTItemDetails.Rows[iRowCount][iCount].ToString().Trim() == string.Empty)
                                attr.Value = Constants.S_ZERO;
                            else
                                attr.Value = aoDTItemDetails.Rows[iRowCount][iCount].ToString();
                        }
                        else if (sAtrrName == "Hall")        //////Hall
                        {
                            string sValue = aoDTItemDetails.Rows[iRowCount]["Hall"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "RackNo")        //////Rack
                        {
                            string sValue = aoDTItemDetails.Rows[iRowCount]["Rack No"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "ShelfNo")        //////Shelf
                        {
                            string sValue = aoDTItemDetails.Rows[iRowCount]["Shelf No"].ToString().Trim();
                            attr.Value = sValue;
                        }  
                        else
                        {
                            attr.Value = aoDTItemDetails.Rows[iRowCount][iCount].ToString();
                        }
                        oXmlNode.Attributes.Append(attr);
                    }

                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                }
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }

        /// <summary>
        /// This method is used to throw an appropriate exception.
        /// </summary>
        /// <param name="iColCount"></param>
        /// <param name="sRowNumber"></param>
        private void ThrowInvalidItemDataException(int iColCount, string sRowNumber)
        {
            switch (iColCount)
            {
                case I_XLS_ITEM_CODE:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_CODE + sRowNumber + ".");
                case I_XLS_ITEM_NAME:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_NAME + sRowNumber + ".");
                case I_XLS_UOM:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_UOM + sRowNumber + ".");
                case I_XLS_ITEM_CATEGORY:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_CATEGORY + sRowNumber + ".");
                case I_XLS_ITEM_QUANTITY:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_QUANTITY + sRowNumber + ".");
                case I_XLS_ITEM_REORDER_LVL:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_REORDER_LVL + sRowNumber + ".");
                case I_XLS_NO_RECORD_FOUND:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NO_RECORD_FOUND);
                case   I_XLS_NO_PROPER_FORMAT :
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NO_PROPER_FORMAT);
                case I_XLS_ITEM_ISCONSIDERFORDETAILLEVEL:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_ISCONSIDERFORDETAILLEVEL + sRowNumber + ".");
                case I_XLS_ITEM_UNITCOUNTOFUOM:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_UNITCOUNTOFUOM + sRowNumber + ".");
                case I_XLS_ITEM_IS_UNIT:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_IS_UNIT + sRowNumber + ".");
                case I_XLS_ITEM_IS_UNIT_REORDER:
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(S_NULL_ITEM_IS_UNIT_REORDER + sRowNumber + ".");
            }
        }

        /// <summary>
        /// This method is used to check mandatory field and through exception.
        /// </summary>
        /// <param name="aoDTItemDetails"></param>
        /// <param name="abSetAutoCode"></param>
        /// <returns></returns>
        private bool CheckForMandatoryFields(DataTable aoDTItemDetails, bool abSetAutoCode)
        {
            int iCheckFromCol = I_XLS_ITEM_CODE;

            //If 'Set Auto Item Code' checkbox checked then ,
            //it check mendatory condition from Item Name column in Excel sheet. 
            if (abSetAutoCode)
                iCheckFromCol = I_XLS_ITEM_NAME;

            for (int iColCount = iCheckFromCol; iColCount < aoDTItemDetails.Columns.Count; iColCount++)
            {
                string sRowNumber = "";
                string sContents = "";
                for (int iRowcount = 0; iRowcount < aoDTItemDetails.Rows.Count; iRowcount++)
                {
                    sContents = aoDTItemDetails.Rows[iRowcount][iColCount].ToString();
                    if (sContents.Trim() == "")
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
                if (sRowNumber.Trim() != "")
                    ThrowInvalidItemDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
            }
            if (IsItemNamesDuplicate(aoDTItemDetails) || (!abSetAutoCode && IsItemCodesDuplicate(aoDTItemDetails))
                        || IsValidItemDetails(aoDTItemDetails) || IsDuplicateUOMUnitCount(aoDTItemDetails) || IsInCorrectValue(aoDTItemDetails, I_XLS_ITEM_ISCONSIDERFORDETAILLEVEL, "Consider For Detail Level?")
                || IsInCorrectValue(aoDTItemDetails, I_XLS_ITEM_IS_UNIT, "Is Quantity in Unit?") || IsInCorrectValue(aoDTItemDetails, I_XLS_ITEM_IS_UNIT_REORDER, "Is Reorder Level in Unit?"))
                return false;
            return true;

        }

        private bool IsInCorrectValue(DataTable aoDTItemDetails, int aiColumnIndex, string asColumnName)
        {
            string sIncorrectRows = string.Empty;
         
            for (int iRowcount = 0; iRowcount < aoDTItemDetails.Rows.Count; iRowcount++)
            {
                string sUOM = aoDTItemDetails.Rows[iRowcount][aiColumnIndex].ToString();
                if (sUOM.ToLower() != "true" && sUOM.ToLower() != "false")
                    sIncorrectRows = sIncorrectRows + "," + iRowcount;
            }

            if (sIncorrectRows != string.Empty)
                throw new BusinessLogic.Exceptions.InvalidItemDataException("Value of column '" + asColumnName + "' should be either True or False in worksheet at row number :" + sIncorrectRows.Substring(1) + ".");
          
            return false;
        }

        private bool IsDuplicateUOMUnitCount(DataTable aoDTItemDetails)
        {
            string sIncorrectRows = string.Empty;
            string sUnitCount = string.Empty;
            string sUOMName = string.Empty;

            DataTable dt = UOMMasterBL.GetAll(moItemInfoStruct.iSchoolId);

            Dictionary<string, int> dictUOMUnitCount = new Dictionary<string, int>();
            for (int iRowcount = 0; iRowcount < aoDTItemDetails.Rows.Count; iRowcount++)
            {
                string sUOM = aoDTItemDetails.Rows[iRowcount][I_XLS_UOM].ToString();
                int iUnitCount = aoDTItemDetails.Rows[iRowcount][I_XLS_ITEM_UNITCOUNTOFUOM].ToInt();

                DataRow[] dr = dt.Select("Name='" + sUOM+"'");
                if (dr.Length > 0)
                {
                    int iSavedUnitCount = dr[0]["PieceCount"].ToInt();
                    if (iUnitCount != iSavedUnitCount)
                    {
                        sUOMName = sUOMName + ", " + dr[0]["Name"].ToString() + "(" + dr[0]["PieceCount"] + ")";
                        sUnitCount = sUnitCount + "," + iRowcount;
                    }
                }

                if (!dictUOMUnitCount.Keys.Contains(sUOM))
                    dictUOMUnitCount.Add(sUOM, iUnitCount);
                else
                {
                    if (iUnitCount != dictUOMUnitCount[sUOM])
                        sIncorrectRows = sIncorrectRows + "," + iRowcount;
                }
            }

            string sFinalMessage = string.Empty;
            if (sUnitCount != string.Empty || sIncorrectRows != string.Empty)
            {
                if (sUnitCount != string.Empty)
                {
                    sFinalMessage = "Unit Count of UOM should be " + sUOMName.Substring(1) + " in worksheet at row number :" + sUnitCount.Substring(1) + ".";
                     if (sIncorrectRows != string.Empty)
                         sFinalMessage = sFinalMessage + "<br />"+S_NULL_ITEM_UNITCOUNT_INCORRECT + sIncorrectRows.Substring(1) + ".";
                    throw new BusinessLogic.Exceptions.InvalidItemDataException(sFinalMessage);
                }
                else
                {
                    if (sIncorrectRows != string.Empty)
                         sFinalMessage = S_NULL_ITEM_UNITCOUNT_INCORRECT + sIncorrectRows.Substring(1) + ".";
                }
                throw new BusinessLogic.Exceptions.InvalidItemDataException(sFinalMessage);
            }

            return false;
        }

        /// <summary>
        /// This method is used to check item name duplicate or not. if true then throws exception.
        /// </summary>
        /// <param name="aoDTItemDetails"></param>
        /// <returns></returns>
        private bool IsItemNamesDuplicate(DataTable aoDTItemDetails)
        {
            int iSchoolId = moItemInfoStruct.iSchoolId;
            bool bIsDuplicateItemName;
            string sDuplicateInExcelRowNumber = string.Empty;
            string sRowNumber = string.Empty;
            string sItemName = string.Empty;

            sDuplicateInExcelRowNumber = IsItemDuplicateInExcel(aoDTItemDetails);
            if (sDuplicateInExcelRowNumber == "")
            {
                for (int iRowcount = 0; iRowcount < aoDTItemDetails.Rows.Count; iRowcount++)
                {
                    sItemName =StringUtility.ReplaceSingleQuoteInString(aoDTItemDetails.Rows[iRowcount]["Item Name"].ToString(),true);
                    bIsDuplicateItemName = IsDuplicateItemName(sItemName);
                    if (!bIsDuplicateItemName)
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
            }
            if (sDuplicateInExcelRowNumber != "")
            {
                sDuplicateInExcelRowNumber = sDuplicateInExcelRowNumber.Substring(0, sDuplicateInExcelRowNumber.Length - 2);
                throw new BusinessLogic.Exceptions.InvalidItemDataException(S_DUPLICATE_ITEM_NAME_IN_FILE + sDuplicateInExcelRowNumber + ".");
            }
            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new BusinessLogic.Exceptions.InvalidItemDataException(S_DUPLICATE_ITEM_NAME + sRowNumber + ".");
            }

            return false;
        }

        /// <summary>
        /// This method is used to check item code duplicate or not. If true then throws exception.
        /// </summary>
        /// <param name="aoDTItemDetails"></param>
        /// <returns></returns>
        private bool IsItemCodesDuplicate(DataTable aoDTItemDetails)
        {
            int iSchoolId = moItemInfoStruct.iSchoolId;
            bool bIsDuplicateItemCode;
            string sDuplicateInExcelRowNumber = string.Empty;
            string sRowNumber = string.Empty;
            string sItemCode = string.Empty;

            sDuplicateInExcelRowNumber = IsItemCodeDuplicateInExcel(aoDTItemDetails);
            if (sDuplicateInExcelRowNumber == "")
            {
                for (int iRowcount = 0; iRowcount < aoDTItemDetails.Rows.Count; iRowcount++)
                {
                    sItemCode = aoDTItemDetails.Rows[iRowcount]["Item Code"].ToString();
                    bIsDuplicateItemCode = IsDuplicateItemCode(sItemCode);
                    if (!bIsDuplicateItemCode)
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
            }
            if (sDuplicateInExcelRowNumber != "")
            {
                sDuplicateInExcelRowNumber = sDuplicateInExcelRowNumber.Substring(0, sDuplicateInExcelRowNumber.Length - 2);
                throw new BusinessLogic.Exceptions.InvalidItemDataException(S_DUPLICATE_ITEM_CODE_IN_FILE + sDuplicateInExcelRowNumber + ".");
            }
            else if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new BusinessLogic.Exceptions.InvalidItemDataException(S_DUPLICATE_ITEM_CODE + sRowNumber + ".");
            }
            return false;
        }

        /// <summary>
        /// This method is used to check duplicate item name in excel sheet.
        /// </summary>
        /// <param name="aoDTItemDetails"></param>
        /// <returns></returns>
        private string IsItemDuplicateInExcel(DataTable aoDTItemDetails)
        {
            int iIndex = 0;
            string sRowNumber = "";
            int iRecordCount = aoDTItemDetails.Rows.Count;
            for (iIndex = 0; iIndex < iRecordCount; iIndex++)
            {
                string sItemName = StringUtility.ReplaceSingleQuoteInString(aoDTItemDetails.Rows[iIndex]["Item Name"].ToString(), true);                
                DataRow[] oDR = aoDTItemDetails.Select("[Item Name]='" + sItemName + "'");  
                if (oDR.Length > 1)
                    sRowNumber = sRowNumber + (iIndex + 1).ToString() + ", ";
            }
            return sRowNumber;
        }

        /// <summary>
        /// This method is used to check duplicate item code in excel sheet.
        /// </summary>
        /// <param name="aoDTItemDetails"></param>
        /// <returns></returns>
        private string IsItemCodeDuplicateInExcel(DataTable aoDTItemDetails)
        {
            int iIndex = 0;
            string sRowNumber = "";
            int iRecordCount = aoDTItemDetails.Rows.Count;
            for (iIndex = 0; iIndex < iRecordCount; iIndex++)
            {
                string sItemCode = aoDTItemDetails.Rows[iIndex]["Item Code"].ToString();
                DataRow[] oDR = aoDTItemDetails.Select("[Item Code]='" + sItemCode + "'");
                if (oDR.Length > 1)
                    sRowNumber = sRowNumber + (iIndex + 1).ToString() + ", ";
            }
            return sRowNumber;
        }

        /// <summary>
        /// This method is used to check duplicate item name.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateItemName(string asItemName)
        {
            ItemsMasterDC oItemsMasterDC = new ItemsMasterDC();
            bool bIsDuplicate = oItemsMasterDC.IsDuplicateItemName(asItemName);
            return bIsDuplicate;
        }

        /// <summary>
        /// This method is used to check duplicate item code.
        /// </summary>
        /// <param name="asItemCode"></param>
        /// <returns></returns>
        public bool IsDuplicateItemCode(string asItemCode)
        {
            ItemsMasterDC oItemsMasterDC = new ItemsMasterDC();
            bool bIsDuplicate = oItemsMasterDC.IsDuplicateItemCode(asItemCode);
            return bIsDuplicate;
        }

        /// <summary>
        /// This method is used to check valid item data in excel sheet.
        /// </summary>
        /// <param name="aoDTItemDetails"></param>
        /// <returns></returns>
        private bool IsValidItemDetails(DataTable aoDTItemDetails)
        {
            int iRecordCount = aoDTItemDetails.Rows.Count;

            int iQuantity ;

            string sRowNoUOM = string.Empty;
            string sRowNoItemCategory = string.Empty;
            string sRowNoQty = string.Empty;
            string sRowNoReorderQty = string.Empty;
            string sErrorMessage = string.Empty;          

            ItemsMasterBL oItemsMasterBL = new ItemsMasterBL();
            int iSchoolID = moItemInfoStruct.iSchoolId;
            DataSet oDSItemInfo = oItemsMasterBL.GetAddItemDetails(iSchoolID);

            for (int iIndex = 0; iIndex < iRecordCount; iIndex++)
            {
                // Used to check valid unit of measurement 
                string sItemUOM = aoDTItemDetails.Rows[iIndex][" Unit Of Measurement"].ToString();              
                if (sItemUOM.Length == 0)
                    sRowNoUOM = sRowNoUOM + (iIndex + 1).ToString() + ", ";

                // Used to check valid item category
                string sItemCategory = aoDTItemDetails.Rows[iIndex]["Item Category"].ToString();              
                if (sItemCategory.Length == 0)
                    sRowNoItemCategory = sRowNoItemCategory + (iIndex + 1).ToString() + ", ";

                // Used to check valid item quantity
                if(!int.TryParse(aoDTItemDetails.Rows[iIndex]["Item Quantity"].ToString(),out iQuantity))
                    sRowNoQty = sRowNoQty + (iIndex + 1).ToString() + ", ";

                // Used to check valid item reorder level
                if (!int.TryParse(aoDTItemDetails.Rows[iIndex]["Item Reorder Level "].ToString(), out iQuantity))
                    sRowNoReorderQty = sRowNoReorderQty + (iIndex + 1).ToString() + ", ";
            }
            if (sRowNoUOM != string.Empty)
            {
                sRowNoUOM = sRowNoUOM.Substring(0, sRowNoUOM.Length - 2);
                sErrorMessage = S_INVALID_UOM + sRowNoUOM + ".<BR>";
            }
            if (sRowNoItemCategory != string.Empty)
            {
                sRowNoItemCategory = sRowNoItemCategory.Substring(0, sRowNoItemCategory.Length - 2);
                sErrorMessage = sErrorMessage + S_INVALID_ITEM_CATEGORY + sRowNoItemCategory + ".<BR>";
            }
            if (sRowNoQty != string.Empty)
            {
                sRowNoQty = sRowNoQty.Substring(0, sRowNoQty.Length - 2);
                sErrorMessage = sErrorMessage + S_INVALID_ITEM_QUANTITY + sRowNoQty + ".<BR>";
            }
            if (sRowNoReorderQty != string.Empty)
            {
                sRowNoReorderQty = sRowNoReorderQty.Substring(0, sRowNoReorderQty.Length - 2);
                sErrorMessage = sErrorMessage + S_INVALID_ITEM_REORDER_LEVEL + sRowNoReorderQty + ".";
            }
            if (sErrorMessage != string.Empty)
                throw new BusinessLogic.Exceptions.InvalidItemDataException(sErrorMessage);

            return false;
        }

        #endregion " Private Methods "
    }
}
