using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Xml;
using Utility;
using System.Text.RegularExpressions;

namespace BusinessLogic
{
    /// <summary>
    /// Purpose :To upload items through excel sheet and save them in item Master.
    /// Validate all the field from excel sheet,
    /// also check duplication and finally save change to the database.
    /// </summary>
    public class ImportBookBL
    {
        #region " Constants "

        const string S_EXCEL_FILE_MESSAGE = "Please select the excel file only. Select another file to upload.";
        const string S_ASSIGNED_CATEGORY = "Category cannot be added. Since it is already assigned to book in worksheet at row number : ";
        const string S_DUPLICATE_BOOK_NUMBER = "Accession number already exist. Please enter another accession number in worksheet at row number :";
        const string S_NULL_MEDIA_TYPE = "Media type should not be blank in worksheet at row number : ";
        const string S_NULL_CATEGORY = "Category should not be blank in worksheet at row number : ";
        const string S_NULL_SUB_CATEGORY = "Sub category should not be blank in worksheet at row number : ";
        const string S_NULL_BOOK_TITLE = "Book title should not be blank in worksheet at row number : ";
        const string S_NULL_AUTHORS = "Author should not be blank in worksheet at row number : ";
        const string S_NULL_PUBLISHER = "Publisher should not be blank in worksheet at row number : ";
        const string S_NULL_PRICE = "Price should not be blank in worksheet at row number : ";
        const string S_NULL_RACK_NUMBER = "Rack number should not be blank in worksheet at row number : ";
        const string S_NULL_SHELF = "Shelf should not be blank in worksheet at row number : ";
        const string S_NULL_BOOK_NUMBER = "Accession number should not be blank in worksheet at row number : ";
        const string S_NO_RECORD_FOUND = "File to be imported should not be empty ";
        const string S_NO_PROPER_FORMAT = "Data in uploaded file is not in correct format.";
        const string S_NULL_LANGUAGE = "Language should not be blank in worksheet at row number : ";
        const string S_NULL_ONLY_FOR_READING = "Please select can be issued from the list in worksheet at row number : ";
        const string S_NULL_IS_GIFTED = "Please select IsGifted from the list in worksheet at row number : ";
        const string S_NULL_CLASSIFICATION = "Classification should not be blank in worksheet at row number : ";
        const string S_NULL_LOST_PERCENTAGE = "Lost percentage should not be blank in worksheet at row number : ";
        const string S_NULL_TOTAL_PAGES = "Total pages of the book should not be blank in worksheet at row number : ";
        const string S_NULL_VENDOR_NAME = "Vendor name should not be blank in worksheet at row number : ";
        const string S_NULL_BILL_NUMBER = "Bill number should not be blank in worksheet at row number : ";
        const string S_VALID_BILL_NUMBER = "Bill number should be alphanumeric in worksheet at row number : ";
        const string S_ZERO_BILL_NO = "Bill number should not be zero in Worksheet at row number : ";
        const string S_NULL_PURCHASE_DATE = "Purchase date should not be blank in worksheet at row number : ";
        const string S_FORMAT_PURCHASE_DATE = " Date of purchase format is wrong in Worksheet at row number : ";
        const string S_VALID_PURCHASE_DATE = "Date of purchase is a future date in Worksheet at row number : ";
        const string S_NULL_VENDOR_MOBILE_NO = "Contact number should not be blank in Worksheet at row number : ";
        const string S_VALID_VENDOR_CONTACT_NO = "Invalid Contact number in Worksheet at row number : ";
        const string S_VALID_LENGTH_VENDOR_CONTACT_NO = "Contact number should be of greater than or equal to 1 digits and less than or equal to 15 digits in Worksheet at row number : ";
        const string S_ZERO_VENDOR_CONTACT_NO = "Contact number should not be zero in Worksheet at row number : ";
        

        const int I_XLS_MEDIA_TYPE = 0;
        const int I_XLS_CATEGORY = 1;
        const int I_XLS_SUB_CATEGORY = 2;
        const int I_XLS_BOOK_TITLE = 3;
        const int I_XLS_AUTHORS = 4;
        const int I_XLS_PUBLISHER = 5;
        const int I_XLS_CLASSIFICATION = 6;
        const int I_XLS_LANGUAGE = 7;
        const int I_XLS_STANDARD = 8;
        const int I_XLS_ONLY_FOR_READING = 9;
        const int I_XLS_LOST_PERCENTAGE = 10;
        const int I_XLS_RACK_NUMBER = 11;
        const int I_XLS_SHELF = 12;
        const int I_XLS_BOOK_NUMBER = 15;
        const int I_XLS_PRICE = 16;
        const int I_XLS_TOTAL_PAGES = 17;
        const int I_XLS_IS_GIFTED = 18;
        const int I_XLS_BILL_NO = 19;
        const int I_XLS_PURCHASE_DATE = 20;
        const int I_XLS_VENDOR_NAME = 21;
        const int I_XLS_VENDOR_CONTACT_NO = 22;
        const int I_XLS_ISBN = 23;
        const int I_XLS_NO_PROPER_FORMAT = 24;
        const int I_XLS_NO_RECORD_FOUND = 25;
       
       
        #endregion

        #region " Data Members "

        private string msSourceFileName = string.Empty;
        private string msServerFilePath = string.Empty;
        private struct BookInfo
        {
            public int iSchoolId;
            public int iAcademicYearId;
            public int iUserId;
        };

        BookInfo moBookInfoStruct;

        #endregion

        #region " Properties "

        public int SchoolId
        {
            set { moBookInfoStruct.iSchoolId = value; }
        }

        public int AcademicYearId
        {
            set { moBookInfoStruct.iAcademicYearId = value; }
        }

        public int UserId
        {
            set { moBookInfoStruct.iUserId = value; }
        }

        #endregion

        /// <summary>
        /// Constructor will accept the excel file name containing the item list.
        /// </summary>
        /// <param name="asFileName"></param>
        public ImportBookBL(string asSourceFileName, string asServerFolderPath)
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
        /// This will also store all the Item details in the database table employee_m.
        /// </summary>
        /// <returns></returns>
        public string UploadFile(int aiOrgConfigId)
        {
            // Validate the uploaded file.
            if (!(IsValidFileExtension()))
            {
                return S_EXCEL_FILE_MESSAGE;
            }
            // Book file upload. Save records from excel sheet to database.
            // Get dataset containing book details. The dataset is created from the excel sheet uploaded by process mgr.

            DataSet oDSBookDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Book Data");

            if (oDSBookDetails != null && oDSBookDetails.Tables.Count > 0)
            {
                // Check if data is loaded in dataset successfully.
                DataTable oDTBooks = oDSBookDetails.Tables[0];
                oDTBooks = CommonUtility.DeleteEmptyRows(oDTBooks);
                if (oDSBookDetails.Tables[0].Rows.Count > 0)
                {
                    string sBookDetails = GetXMLStringFromXLSRows(oDTBooks, "BookDetails", "BookDetail");
                    string sBookNoDetails = GetBNoXMLStringFromXLSRows(oDTBooks, "Standards", "BNo");

                    BookCollectionBL oBookCollectionBL = new BookCollectionBL(moBookInfoStruct.iSchoolId, moBookInfoStruct.iAcademicYearId,
                                                            moBookInfoStruct.iUserId);
                    oBookCollectionBL.InsertMultipleBooks(sBookDetails, sBookNoDetails, aiOrgConfigId);
                }
                else
                    ThrowInvalidBookDataException(0, "0");
            }
            else
                ThrowInvalidBookDataException(23, "0");

            return "";
        }

        /// <summary>
        /// This function checks if the extention of the file to be uploaded is .XLS
        /// Reason - only excel files can be uploaded for employee type file upload.
        /// </summary>
        /// <returns></returns>
        private bool IsValidFileExtension()
        {
            return (msSourceFileName.ToUpper().EndsWith(".XLS") || msSourceFileName.ToUpper().EndsWith(".XLSX"));
        }

        #region "XML Creation"

        /// <summary>
        /// This method is used to create XML to add book details to the database.
        /// </summary>
        /// <param name="aoDTBookDetails"></param>
        /// <param name="asRootElementName"></param>
        /// <param name="asElementName"></param>
        /// <returns></returns>
        public string GetXMLStringFromXLSRows(DataTable aoDTBookDetails, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("Media_Type");
            oArrayList.Add("Category");
            oArrayList.Add("SubCategory");
            oArrayList.Add("Book_Title");
            oArrayList.Add("Authors");
            oArrayList.Add("Publisher");
            oArrayList.Add("Classification");
            oArrayList.Add("Language");
            oArrayList.Add("Standard");
            oArrayList.Add("IsOnlyForReading");
            oArrayList.Add("Lost_Percentage");
            oArrayList.Add("Rack_Number");
            oArrayList.Add("Shelf");
            oArrayList.Add("Description");
            oArrayList.Add("Remark");
            oArrayList.Add("BookNo");
            oArrayList.Add("Price");
            oArrayList.Add("TotalPages");
            oArrayList.Add("IsGifted");
            oArrayList.Add("BillNo");
            oArrayList.Add("PurchaseDate");
            oArrayList.Add("VendoreName");
            oArrayList.Add("VendoreMobileNo");
            oArrayList.Add("ISBN");
            oArrayList.Add("SNo");            
            oArrayList.Add("BookEdition");
            oArrayList.Add("BookYear");
            oArrayList.Add("CallNumber");
            oArrayList.Add("Series");
            oArrayList.Add("Status");
            oArrayList.Add("PublicationDate");
          
            //oArrayList.Add("VendoreMobileNo");

            if (CheckForMandatoryFields(aoDTBookDetails))
            {
                // Loop through all the grid rows.
                for (int iRowCount = 0; iRowCount <= aoDTBookDetails.Rows.Count - 1; iRowCount++)
                {

                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
                    DataRow oDataRow = aoDTBookDetails.Rows[iRowCount];

                    // Loop through all the columns for the row.
                    string sBookTitle = "";
                    sBookTitle = aoDTBookDetails.Rows[iRowCount][I_XLS_BOOK_TITLE].ToString();

                    for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                    {
                        sAtrrName = oArrayList[iCount].ToString();
                        if (sAtrrName != "Standard")
                        {
                            attr = oDoc.CreateAttribute(sAtrrName);
                            if (sAtrrName == "SNo")
                            {
                                string sRowNo = iRowCount.ToString();
                                attr.Value = sRowNo;
                            }
                            else if (sAtrrName == "Media_Type")
                            {
                                if (aoDTBookDetails.Rows[iRowCount][iCount].ToString() == "Printable")
                                    attr.Value = true.ToString();
                                else
                                    attr.Value = false.ToString();
                            }
                            else if (sAtrrName == "IsOnlyForReading")
                            {
                                if (aoDTBookDetails.Rows[iRowCount][iCount].ToString() == "Y")
                                    attr.Value = true.ToString();
                                else
                                    attr.Value = false.ToString();
                            }
                            else if (sAtrrName == "IsGifted")
                            {
                                if (aoDTBookDetails.Rows[iRowCount][iCount].ToString() == "Y")
                                    attr.Value = true.ToString();
                                else
                                    attr.Value = false.ToString();
                            }
                            else if (sAtrrName == "BookEdition")
                            {
                                attr.Value = aoDTBookDetails.Rows[iRowCount]["Book Edition"].ToString();
                            }
                            else if (sAtrrName == "BookYear")
                            {
                                attr.Value = aoDTBookDetails.Rows[iRowCount]["Book Year"].ToString();
                            }
                        

                            else if (sAtrrName == "ISBN")
                            {
                                attr.Value = aoDTBookDetails.Rows[iRowCount]["ISBN"].ToString();
                            }
                          
                          
                                else if (sAtrrName == "CallNumber")
                            {
                                attr.Value = aoDTBookDetails.Rows[iRowCount]["Call Number"].ToString();
                            }
                            else if (sAtrrName == "Series")
                            {
                                attr.Value = aoDTBookDetails.Rows[iRowCount]["Series"].ToString();
                            }
                            else if (sAtrrName == "Status")
                            {
                                attr.Value = aoDTBookDetails.Rows[iRowCount]["Status"].ToString();
                            }
                            else if (sAtrrName == "PublicationDate")
                            {
                                attr.Value = aoDTBookDetails.Rows[iRowCount]["Publication Date"].ToString();
                            }
                            else
                            {
                                attr.Value = aoDTBookDetails.Rows[iRowCount][iCount].ToString();
                            }


                            oXmlNode.Attributes.Append(attr);
                        }
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
        /// This method is used to create XML to add Accession number details in database.
        /// </summary>
        /// <param name="aoDTBookDetails"></param>
        /// <param name="asRootElementName"></param>
        /// <param name="asElementName"></param>
        /// <returns></returns>
        public string GetBNoXMLStringFromXLSRows(DataTable aoDTBookDetails, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();

            oArrayList.Add("SNo");
            oArrayList.Add("Standards");

            if (IsBookNumbersDuplicate(aoDTBookDetails))
            {
                // Loop through all the grid rows.
                for (int iRowCount = 0; iRowCount <= aoDTBookDetails.Rows.Count - 1; iRowCount++)
                {
                    DataRow oDataRow = aoDTBookDetails.Rows[iRowCount];

                    // Loop through all the columns for the row.
                    string sBookNumber = "";
                    sBookNumber = aoDTBookDetails.Rows[iRowCount][I_XLS_STANDARD].ToString();

                    string[] sArrBookNo = sBookNumber.Split(',');
                    for (int iBNoCount = 0; iBNoCount < sArrBookNo.Length; iBNoCount++)
                    {
                        // Create root xml element.
                        XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");

                        for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                        {
                            sAtrrName = oArrayList[iCount].ToString();
                            attr = oDoc.CreateAttribute(sAtrrName);

                            if (sAtrrName == "SNo")
                            {
                                string sRowNo = iRowCount.ToString();
                                attr.Value = sRowNo;
                            }
                            else if (sAtrrName == "Standards")
                            {
                                attr = oDoc.CreateAttribute(sAtrrName);
                                attr.Value = sArrBookNo[iBNoCount].ToString();
                            }
                            oXmlNode.Attributes.Append(attr);
                        }
                        // Add the node to root node.
                        oXmlRootNode.AppendChild(oXmlNode);
                    }
                    // Add the root node to document element. 
                    root.AppendChild(oXmlRootNode);
                }
            }

            // return the string generated.
            return root.InnerXml;
        }

        /// <summary>
        /// This method is used to check mandatory field and through exception.
        /// </summary>
        /// <param name="aoDTBookDetails"></param>
        /// <returns></returns>
        private bool CheckForMandatoryFields(DataTable aoDTBookDetails)
        {
            for (int iColCount = 0; iColCount <= aoDTBookDetails.Columns.Count - 1; ++iColCount)
            {
                string sRowNumber = "";
                string sContents = "";
                object odtPurchaseGate;
                Type tdtPurchaseDate;
                string sDateFormat = string.Empty;
                string sFutureDate = string.Empty;
                DateTime dtCurrent = DateTime.Today;
                string sIsGifted = string.Empty;
                string sVendorName = string.Empty;
                for (int iRowcount = 0; iRowcount < aoDTBookDetails.Rows.Count; iRowcount++)
                {
                    if (iColCount != 19 && iColCount != 20 && iColCount != 21 && iColCount != 22 && iColCount != 13 && iColCount != 14 && iColCount != 23 && iColCount != 24 && iColCount != 25 && iColCount != 26)
                    {
                        sContents = aoDTBookDetails.Rows[iRowcount][iColCount].ToString();
                        if (sContents.Trim() == "")
                            sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                    }
                    if (iColCount == 19)
                    {
                        sIsGifted = aoDTBookDetails.Rows[iRowcount][18].ToString();
                        if (sIsGifted == "N")
                        {
                            sContents = aoDTBookDetails.Rows[iRowcount][18].ToString();
                            if (sContents.Trim() == "")
                                sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                    }
                    if (iColCount == 20)
                    {
                        sIsGifted = aoDTBookDetails.Rows[iRowcount][18].ToString();
                        if (sIsGifted == "N")
                        {
                            sContents = aoDTBookDetails.Rows[iRowcount][18].ToString();
                            if (sContents.Trim() == "")
                                sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }

                        if (aoDTBookDetails.Rows[iRowcount][iColCount].ToString() != string.Empty)
                        {
                            odtPurchaseGate = aoDTBookDetails.Rows[iRowcount][iColCount];
                            tdtPurchaseDate = odtPurchaseGate.GetType();
                            if (!tdtPurchaseDate.FullName.Equals("System.DateTime"))
                            {
                                sDateFormat = sDateFormat + (iRowcount + 1).ToString() + ", ";
                            }
                            else if (Convert.ToDateTime(odtPurchaseGate) > dtCurrent)
                            {
                                sFutureDate = sFutureDate + (iRowcount + 1).ToString() + ", ";
                            }
                        }
                    }
                  
                    if (iColCount == 22)
                    {
                        sIsGifted = aoDTBookDetails.Rows[iRowcount][18].ToString();
                        sVendorName = aoDTBookDetails.Rows[iRowcount][21].ToString();
                        if (sIsGifted == "N" && sVendorName != string.Empty)
                        {
                            sContents = aoDTBookDetails.Rows[iRowcount][iColCount].ToString();
                            if (sContents != "")
                            {
                                if (sContents.Length <= 0 && sContents.Length > 15)
                                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                            }
                            //else
                            //    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                        }
                    }

                }
                if (sRowNumber.Trim() != "")
                    ThrowInvalidBookDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
                if (sDateFormat != "")
                {
                    sDateFormat = sDateFormat.Substring(0, sDateFormat.Length - 2);
                    throw new BusinessLogic.Exceptions.NullStudentDateofBirthExceptions(S_FORMAT_PURCHASE_DATE + sDateFormat + ".");
                }
                if (sFutureDate != "")
                {
                    sFutureDate = sFutureDate.Substring(0, sFutureDate.Length - 2);
                    throw new BusinessLogic.Exceptions.NullStudentDateofBirthExceptions(S_VALID_PURCHASE_DATE + sFutureDate + ".");
                }
            }
            //if (IsBookTitlesDuplicate(aoDTBookDetails))
            //    return false;
            if (IsBillNoInvalid(aoDTBookDetails))
                return false;
            if (IsBillNoZero(aoDTBookDetails))
                return false;
            if (IsValidContactNo(aoDTBookDetails))
                return false;
            if (IsContactNoZero(aoDTBookDetails))
                return false;
            if (IsContactNoValidLength(aoDTBookDetails))
                return false;
            return true;
        }
        private bool IsBillNoInvalid(DataTable aoDTBookDetails)
        {
            string sRowNumber = string.Empty;
            string sBillNumber = string.Empty;
            string sBillNo = string.Empty;

            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDTBookDetails.Rows.Count; iRowcount++)
            {
                sBillNumber = Convert.ToString(aoDTBookDetails.Rows[iRowcount][I_XLS_BILL_NO]);
                string sNonAlphaNumericChar = "$,!,#,%,^,&,*,(,),_,+,=,{,[,],],;,',.,/,?,\\,|*, ,.";
                string[] sNonAlphaNumericCharArray = sNonAlphaNumericChar.Split(',');
                int iLen = sNonAlphaNumericCharArray.Length;
                int iRowcnt = 0;
                for (iRowcnt = 0; iRowcnt < iLen - 1; iRowcnt++)
                {
                    if (sBillNumber.Contains(sNonAlphaNumericCharArray[iRowcnt]) || sBillNumber.Contains(","))
                    {
                        sBillNo = "1";
                        if (sBillNo != string.Empty)
                        {
                            if (sRowNumber == string.Empty)
                                sRowNumber = (iRowcount + 1).ToString();
                            else
                                sRowNumber += ", " + (iRowcount + 1).ToString();
                        }
                        break;
                    }
                }
            }
            if (sBillNo != string.Empty)
            {
                sBillNumber = sBillNumber.Substring(0, sBillNumber.Length - 2);
                throw new BusinessLogic.Exceptions.ValidMobileNumberExceptions(S_VALID_BILL_NUMBER + sRowNumber + ".");
            }
            return false;
        }
        private bool IsBillNoZero(DataTable aoDTBookDetails)
        {
            string sStr = string.Empty;
            string sRowNumber = string.Empty;
            string sBillNo = string.Empty;

            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDTBookDetails.Rows.Count; iRowcount++)
            {
                sBillNo = Convert.ToString(aoDTBookDetails.Rows[iRowcount][I_XLS_BILL_NO]);
                 Regex reg = new Regex(@"^[0-9]*$");
                 if (reg.IsMatch(sBillNo) == true && sBillNo!="")
                 {
                     if (Convert.ToInt64(sBillNo) == 0)
                     {
                         sStr = "1";
                         if (sStr != string.Empty)
                         {
                             if (sRowNumber == string.Empty)
                                 sRowNumber = (iRowcount + 1).ToString();
                             else
                                 sRowNumber += ", " + (iRowcount + 1).ToString();
                         }
                     }
                 }
            }
            if (sStr != string.Empty)
            {
                sBillNo = sBillNo.Substring(0, sBillNo.Length - 2);
                throw new BusinessLogic.Exceptions.ValidMobileNumberExceptions(S_ZERO_BILL_NO + sRowNumber + ".");
            }
            return false;
        }
        private bool IsValidContactNo(DataTable aoDTBookDetails)
        {
            string sStr = string.Empty;
            string sRowNumber = string.Empty;
            string sMobileNo = string.Empty;

            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDTBookDetails.Rows.Count; iRowcount++)
            {
                sMobileNo = Convert.ToString(aoDTBookDetails.Rows[iRowcount][I_XLS_VENDOR_CONTACT_NO]);
               
                Regex reg = new Regex(@"^[0-9]*$");
                if (reg.IsMatch(sMobileNo) == false)
                {
                    sStr = "1";
                    if (sStr != string.Empty)
                    {
                        if (sRowNumber == string.Empty)
                            sRowNumber = (iRowcount + 1).ToString();
                        else
                            sRowNumber += ", " + (iRowcount + 1).ToString();
                    }
                }
            }
            if (sStr != string.Empty)
            {
                sMobileNo = sMobileNo.Substring(0, sMobileNo.Length - 2);
                throw new BusinessLogic.Exceptions.ValidMobileNumberExceptions(S_VALID_VENDOR_CONTACT_NO + sRowNumber + ".");
            }
            return false;
        }
        private bool IsContactNoZero(DataTable aoDTBookDetails)
        {
            string sStr = string.Empty;
            string sRowNumber = string.Empty;
            string sMobileNo = string.Empty;

            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDTBookDetails.Rows.Count; iRowcount++)
            {
                sMobileNo = Convert.ToString(aoDTBookDetails.Rows[iRowcount][I_XLS_VENDOR_CONTACT_NO]);
                if (sMobileNo != "")
                {
                    if (Convert.ToInt64(sMobileNo) == 0)
                    {
                        sStr = "1";
                        if (sStr != string.Empty)
                        {
                            if (sRowNumber == string.Empty)
                                sRowNumber = (iRowcount + 1).ToString();
                            else
                                sRowNumber += ", " + (iRowcount + 1).ToString();
                        }
                    }
                }
            }
            if (sStr != string.Empty)
            {
                sMobileNo = sMobileNo.Substring(0, sMobileNo.Length - 2);
                throw new BusinessLogic.Exceptions.ValidMobileNumberExceptions(S_ZERO_VENDOR_CONTACT_NO + sRowNumber + ".");
            }
            return false;
        }
        private bool IsContactNoValidLength(DataTable aoDTBookDetails)
        {
            string sRowNumber = string.Empty;
            string sMobileRowNumber = string.Empty;
            string sMobileNumber = string.Empty;

            StudentBL oStudentBL = new StudentBL();

            for (int iRowcount = 0; iRowcount < aoDTBookDetails.Rows.Count; iRowcount++)
            {
                sMobileNumber = Convert.ToString(aoDTBookDetails.Rows[iRowcount][I_XLS_VENDOR_CONTACT_NO]);
                if (sMobileNumber.Trim().Length < 1 || sMobileNumber.Trim().Length > 15)
                {
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
            }
            if (sMobileRowNumber != string.Empty)
            {
                sMobileRowNumber = sMobileRowNumber.Substring(0, sMobileRowNumber.Length - 2);
                throw new BusinessLogic.Exceptions.ValidMobileNumberExceptions(S_VALID_LENGTH_VENDOR_CONTACT_NO + sMobileRowNumber + ".");
            }
            return false;
        }
        /// <summary>
        /// This method is used to throw an appropriate exception.
        /// </summary>
        /// <param name="iColCount"></param>
        private void ThrowInvalidBookDataException(int iColCount, string sRowNumber)
        {
            switch (iColCount)
            {
                case I_XLS_MEDIA_TYPE:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_MEDIA_TYPE + sRowNumber + ".");
                case I_XLS_CATEGORY:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_CATEGORY + sRowNumber + ".");
                case I_XLS_BOOK_TITLE:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_BOOK_TITLE + sRowNumber + ".");
                case I_XLS_AUTHORS:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_AUTHORS + sRowNumber + ".");
                case I_XLS_PUBLISHER:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_PUBLISHER + sRowNumber + ".");
                case I_XLS_CLASSIFICATION:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_CLASSIFICATION + sRowNumber + ".");
                case I_XLS_LANGUAGE:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_LANGUAGE + sRowNumber + ".");
                case I_XLS_ONLY_FOR_READING:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_ONLY_FOR_READING + sRowNumber + ".");
                case I_XLS_LOST_PERCENTAGE:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_LOST_PERCENTAGE + sRowNumber + ".");
                case I_XLS_RACK_NUMBER:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_RACK_NUMBER + sRowNumber + ".");
                case I_XLS_SHELF:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_SHELF + sRowNumber + ".");
                case I_XLS_BOOK_NUMBER:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_BOOK_NUMBER + sRowNumber + ".");
                case I_XLS_PRICE:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_PRICE + sRowNumber + ".");
                case I_XLS_TOTAL_PAGES:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_TOTAL_PAGES + sRowNumber + ".");
                case I_XLS_IS_GIFTED:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_IS_GIFTED + sRowNumber + ".");
                case I_XLS_BILL_NO:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_BILL_NUMBER + sRowNumber + ".");
                case I_XLS_PURCHASE_DATE:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_PURCHASE_DATE + sRowNumber + ".");
                case I_XLS_VENDOR_NAME:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_VENDOR_NAME + sRowNumber + ".");
                case I_XLS_NO_RECORD_FOUND:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NO_RECORD_FOUND + ".");
                case I_XLS_NO_PROPER_FORMAT:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NO_PROPER_FORMAT);
                case I_XLS_VENDOR_CONTACT_NO:
                    throw new BusinessLogic.Exceptions.InvalidBookDataException(S_NULL_VENDOR_MOBILE_NO + sRowNumber + ".");
              
            }
        }

        #endregion

        /// <summary>
        /// This method is used to check book title duplicate or not. if true then through exception.
        /// </summary>
        /// <param name="aoDTBookDetails"></param>
        /// <returns></returns>
        private bool IsBookTitlesDuplicate(DataTable aoDTBookDetails)
        {
            int iSchoolId = moBookInfoStruct.iSchoolId;
            bool bIsDuplicateBookTitle;
            string sRowNumber = "";
            int MediaType;
            BookBL oBookBL = new BookBL();
            sRowNumber = IsBookDuplicateInExcel(aoDTBookDetails);
            if (sRowNumber == "")
            {
                for (int iRowcount = 0; iRowcount < aoDTBookDetails.Rows.Count; iRowcount++)
                {
                    string sMediaType = string.Empty;
                    string sSubCategory = string.Empty;
                    string sCategory = aoDTBookDetails.Rows[iRowcount]["Category"].ToString();
                    sMediaType = aoDTBookDetails.Rows[iRowcount]["Media Type"].ToString();
                    sSubCategory = aoDTBookDetails.Rows[iRowcount]["SubCategory"].ToString();

                    if (sMediaType == "NonPrintable")
                        MediaType = 0;
                    else
                        MediaType = 1;
                    bIsDuplicateBookTitle = oBookBL.IsAssignedCategory(sCategory, MediaType, sSubCategory);
                    if (!bIsDuplicateBookTitle)
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new BusinessLogic.Exceptions.InvalidBookDataException(S_ASSIGNED_CATEGORY + sRowNumber + ".");
            }

            return false;
        }

        /// <summary>
        /// This method is used to check duplicate book title in excel sheet.
        /// </summary>
        /// <param name="aoDTBookDetails"></param>
        /// <returns></returns>
        private string IsBookDuplicateInExcel(DataTable aoDTBookDetails)
        {
            int iIndex = 0;
            string sRowNumber = "";
            DataRow[] oDR1 = null;
            int iRecordCount = aoDTBookDetails.Rows.Count;
            for (iIndex = 0; iIndex < iRecordCount; iIndex++)
            {
                string sBookTitle = aoDTBookDetails.Rows[iIndex]["Book Title"].ToString();
                string sAuthor = aoDTBookDetails.Rows[iIndex]["Author"].ToString();

                string sCategory = aoDTBookDetails.Rows[iIndex]["Category"].ToString();
                string sSubCategory = aoDTBookDetails.Rows[iIndex]["SubCategory"].ToString();
                if (sSubCategory == string.Empty)
                    oDR1 = aoDTBookDetails.Select("[Category]='" + StringUtility.ReplaceSingleQuoteInString(sCategory, true) + "' ");

                if (oDR1 != null)
                {
                    if (oDR1.Length > 1)
                        sRowNumber = sRowNumber + (iIndex + 1).ToString() + ", ";
                }
            }
            return sRowNumber;
        }

        /// <summary>
        /// This method is used to check Accession number is duplicate or not if duplicate then through exception.
        /// </summary>
        /// <param name="aoDTBookDetails"></param>
        /// <returns></returns>
        private bool IsBookNumbersDuplicate(DataTable aoDTBookDetails)
        {
            int iSchoolId = moBookInfoStruct.iSchoolId;
            bool bIsDuplicateNumber = false;
            string sRowNumber = "";
            BookBL oBookBL = new BookBL();
            ArrayList sArrLstBookNo = new ArrayList();

            for (int iRowcount = 0; iRowcount < aoDTBookDetails.Rows.Count; iRowcount++)
            {
                string sBookNumber = "";
                sBookNumber = aoDTBookDetails.Rows[iRowcount][I_XLS_BOOK_NUMBER].ToString();
                string[] sArrBookNo = sBookNumber.Split(',');
                for (int iCount = 0; iCount < sArrBookNo.Length; iCount++)
                {
                    bIsDuplicateNumber = sArrLstBookNo.Contains(sArrBookNo[iCount]);
                    if (!bIsDuplicateNumber)
                    {
                        sArrLstBookNo.Add(sArrBookNo[iCount]);
                        bIsDuplicateNumber = !oBookBL.IsDuplicateBookNumber(sArrBookNo[iCount].ToString());
                    }
                    if (bIsDuplicateNumber == true)
                        break;

                }
                if (bIsDuplicateNumber)
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
            }
            if (sRowNumber != "")
            {
                sRowNumber = sRowNumber.Substring(0, sRowNumber.Length - 2);
                throw new BusinessLogic.Exceptions.InvalidBookDataException(S_DUPLICATE_BOOK_NUMBER + sRowNumber + ".");
            }

            return true;
        }
    }
}