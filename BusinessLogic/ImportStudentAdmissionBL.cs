using System;
using System.Data;
using System.Xml;
using System.Collections;
using Utility;
using System.Text.RegularExpressions;
using BusinessLogic.Exceptions;
using DataCommunicator;
using System.Text;
namespace BusinessLogic
{
    public class ImportStudentAdmissionBL
    {
        #region  " Constants "

        /// <summary>
        /// Purpose :To upload items through excel sheet and save them in item Master.
        /// Validate all the field from excel sheet,
        /// also check duplication and finally save change to the database.
        /// </summary>
        const string S_EXCEL_FILE_MESSAGE = "Please select the excel file only. Select another file to upload.";
        const string S_NULL_FORM_NO = "Form number should not be blank in worksheet at row number : ";
        const string S_NULL_STUDENT_FIRST_NAME = "Student first name should not be blank in worksheet at row number : ";
        const string S_NULL_SEX = "Sex should not be blank in worksheet at row number : ";
        const string S_NULL_DOB = "Date of birth should not be blank in worksheet at row number : ";
        const string S_NULL_GUARDIAN_FIRST_NAME = "Guardian's first name should not be blank in worksheet at row number : ";
        const string S_VALID_CATEGORY = "Student's Category should be selected only from the list in Worksheet at row number : ";
        const string S_NULL_ADDRESS = "Address should not be blank in worksheet at row number : ";
        const string S_NULL_CITY = "City should not be blank in worksheet at row number : ";
        const string S_NULL_STATE = "State should not be blank in worksheet at row number : ";
        const string S_NULL_PINCODE = "Pincode should not be blank in worksheet at row number : ";
        const string S_NULL_MOBILE = "Mobile number should not be blank in worksheet at row number : ";
        const string S_NULL_EMAIL = "Email Address should not be blank in worksheet at row number : ";
        const string S_NULL_LIVING_LOCATION = "Living location should not be blank in worksheet at row number : ";
        const string S_NULL_FATHER_FIRST_NAME = "Father first name should not be blank in worksheet at row number : ";
        const string S_NULL_FATHER_OCCUPATION = "Father occupation should not be blank in worksheet at row number : ";
        const string S_NULL_MOTHER_FIRST_NAME = "Mother first name should not be blank in worksheet at row number : ";

        const string S_INVALID_STUDENT = " Student already present in worksheet at row number : ";
        const string S_INVALID_FORM_NO = "Form number already present in worksheet at row number : ";
        const string S_INVALID_BOD = "Date of birth should be less than 1st January 2010 in worksheet at row number : ";
        const string S_INVALID_SEX = "Please select valid student sex in worksheet at row number : ";
        const string S_INVALID_RELIGION = "Please select valid religion in worksheet at row number : ";
        const string S_INVALID_CATEGORY = "Please select valid category in worksheet at row number : ";
        const string S_INVALID_LAST_BOARD_NAME = "Please select valid last school board name in worksheet at row number : ";
        const string S_INVALID_BOARD_RECOGNISATION = "Please select valid board recognisation in worksheet at row number : ";
        const string S_INVALID_EMAIL = "Email Address format is invalid in worksheet at row number : ";
        const string S_INVALID_LIVING_LOCATION = "Please select valid living location in worksheet at row number : ";
        const string S_INVALID_FATHER_RELIGION = "Please select valid father religion in worksheet at row number : ";
        const string S_INVALID_FATHER_OCCUPATION = "Please select valid father occupation in worksheet at row number : ";
        const string S_INVALID_MOTHER_RELIGION = "Please select valid mother religion in worksheet at row number : ";
        const string S_INVALID_MOTHER_OCCUPATION = "Please select valid mother occupation in worksheet at row number : ";
        const string S_INVALID_CELEBRATION = "Please select valid participation in celebration in worksheet at row number : ";
        const string S_INVALID_NEWS_PUBLICITY = "Please select valid participation in newletter and publicity in worksheet at row number : ";
        const string S_INVALID_ACTIVITIES = "Please select valid participate in co-curricular activities in worksheet at row number : ";
        const string S_INVALID_COMP_SOFT = "Please select valid participate in computer software in worksheet at row number : ";
        const string S_INVALID_EXCURSION = "Please select valid participate in excursion and visits in worksheet at row number : ";
        const string S_INVALID_SPORT = "Please select valid Participate in Sports in worksheet at row number : ";
        const string S_INVALID_SIBLING_STANDARD = "Please select valid Sibling Student Standard";
        const string S_INVALID_SIBLING_DIVISON = "Please select valid Sibling Student Division";
        const string S_INVALID_SIBLING_CLASS = "Class is not present for sibling student";
        const string S_EMPTY_SIBLING_STANDARD = "Sibiling Student Standard should be selected.";
        const string S_EMPTY_SIBLING_DIVISION = "Sibiling Student Division should be selected.";
        const string S_EMPTY_SIBLING_STUDENT_NAME = "Sibiling Student Name should not be empty.";

        const string S_NO_RECORD_FOUND = "File to be imported should not be empty. ";
        const string S_INVALID_FORMAT = "Data in uploaded file is not in correct format.";

        const string S_DUPLICATE_FORM_NUMBER = "Form number already present at rownumber : ";
        
        const string S_MIN_DOB = "1/1/2010";

        const int I_XLS_FORM_NO = 0;
        const int I_XLS_STUDENT_FIRST_NAME = 1;
        const int I_XLS_SEX = 4;
        const int I_XLS_DOB = 6;
        const int I_XLS_GUARDIAN_FIRST_NAME = 16;
        const int I_XLS_CATEGORY = 11;
        const int I_XLS_ADDRESS = 20;
        const int I_XLS_CITY = 21;
        const int I_XLS_STATE = 22;
        const int I_XLS_PINCODE = 23;
        const int I_XLS_MOBILE = 26;
        const int I_XLS_EMAIL = 28;
        const int I_XLS_LIVING_LOCATION = 29;
        const int I_XLS_FATHER_FIRST_NAME = 30;
        const int I_XLS_FATHER_OCCUPATION = 38;
        const int I_XLS_MOTHER_FIRST_NAME = 44;
        const int I_XLS_CELEBRATION = 58;
        const int I_XLS_NO_RECORD_FOUND = 100;
        const int I_XLS_OTHER_LIVING_LOCATION = 64;
        const int I_XLS_SIBLING_STANDARD = 66;
        const int I_XLS_SIBLING_DIVISION = 67;        
        const int I_XLS_SIBLING_CLASS = 200;
        const int I_XLS_INVALID_FORMAT = -1;

        #endregion " Constants "

        #region " Data Members "

        private string msSourceFileName = string.Empty;
        private string msServerFilePath = string.Empty;
        Hashtable moManualMobileNo = new Hashtable();
        private bool mbAllowDuplicateStudent = false;

        private DateTime mdtMinDOB;
        private DateTime mdtMaxDOB;

        private struct AdmissionInfo
        {
            public int iSchoolId;
            public int iAcademicYearId;
            public int iStandardId;
            public int iUserId;
            public bool bIsOnlineAdmission;
            public DateTime iSchoolStartDate;
        };

        AdmissionInfo moAdmissionInfoStruct;

        #endregion " Data Members "

        #region " Properties "

        public bool AllowDuplicateStudent
        {
            get
            {
                return mbAllowDuplicateStudent;
            }
            set
            {
                mbAllowDuplicateStudent = value;
            }
        }

        public int SchoolId
        {
            set { moAdmissionInfoStruct.iSchoolId = value; }
        }

        public int AcademicYearId
        {
            set { moAdmissionInfoStruct.iAcademicYearId = value; }
        }

        public int StandardId
        {
            set { moAdmissionInfoStruct.iStandardId = value; }
        }

        public DateTime SchoolStartDate
        {
            set { moAdmissionInfoStruct.iSchoolStartDate = value; }
            get {return moAdmissionInfoStruct.iSchoolStartDate; }
        }

        public int UserId
        {
            set { moAdmissionInfoStruct.iUserId = value; }
        }

        public bool IsOnlineAdmission
        {
            set { moAdmissionInfoStruct.bIsOnlineAdmission = value; }
        }

        public Hashtable oHashtable
        {
            get { return moManualMobileNo; }
        }

        #endregion " Properties "

        #region " Private Methods "

        /// <summary>
        /// Constructor will accept the excel file name containing the item list.
        /// </summary>
        /// <param name="asSourceFileName"></param>
        /// <param name="asServerFolderPath"></param>
        public ImportStudentAdmissionBL(string asSourceFileName, string asServerFolderPath)
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
        public string UploadFile()
        {
            // Validate the uploaded file.
            if (!(IsValidFileExtension()))
            {
                return S_EXCEL_FILE_MESSAGE;
            }
            // Item file upload. Save records from excel sheet to database.
            // Get dataset containing item details
            DataSet oDSStudentDetails = CommonUtility.ReadExcelSheetAndFetchData(msServerFilePath, "", "Student Data");

            // Check if data is loaded in dataset successfully.
            if (oDSStudentDetails != null && oDSStudentDetails.Tables.Count > 0)
            {
				// Check if data is loaded in dataset successfully.
				
                DataTable oDTStudentDetails = oDSStudentDetails.Tables[0];
                oDTStudentDetails = CommonUtility.DeleteEmptyRows(oDTStudentDetails);
				if (oDTStudentDetails.Rows.Count > 0)
				{
					string sXMLStudentDetails = GetXMLOfStudentDetails(oDTStudentDetails, "StudentDetails", "StudentDetails");
					string sXMLParentDetails = GetXMLOfParentDetails(oDTStudentDetails, "ParentDetails", "ParentDetails");
					string sXMLParentInAssociationEvent = GetXMLOfParentInAssociationEvent(oDTStudentDetails, "ParentInAssociationEvent", "ParentInAssociationEvent");
                    string sXMLAdmissionOnlineTransactionDetails = string.Empty;

                    if (moAdmissionInfoStruct.bIsOnlineAdmission)
                    {
                        sXMLAdmissionOnlineTransactionDetails = GetXMLOfOnlineTransactionDetails(oDTStudentDetails, "StudentOnlineDetails", "StudentOnlineDetails");
                    }

					StudentAdmissionsCollectionBL oStudentAdmissionsCollectionBL = new StudentAdmissionsCollectionBL(moAdmissionInfoStruct.iSchoolId, moAdmissionInfoStruct.iAcademicYearId,
													   moAdmissionInfoStruct.iStandardId, moAdmissionInfoStruct.iUserId);
                    oStudentAdmissionsCollectionBL.InsertMultipleStudentAdmission(sXMLStudentDetails, sXMLParentDetails, sXMLParentInAssociationEvent, sXMLAdmissionOnlineTransactionDetails);
				}
				else
					ThrowInvalidItemDataException(I_XLS_NO_RECORD_FOUND, "0");
			}
			else
				ThrowInvalidItemDataException(I_XLS_INVALID_FORMAT, "0");

            return string.Empty;
        }

        /// <summary>
        /// Gets studentnt information in xml format 
        /// </summary>
        /// <param name="oDTStudentDetails"></param>
        /// <param name="asRootElementName"></param>
        /// <param name="asElementName"></param>
        /// <returns></returns>
        private string GetXMLOfStudentDetails(DataTable oDTStudentDetails, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";

            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();

            ArrayList oArrayList = new ArrayList();
            oArrayList.Add("FormNo");
            oArrayList.Add("StudentFirstName");
            oArrayList.Add("StudentMiddleName");
            oArrayList.Add("StudentLastName");
            oArrayList.Add("Sex");
            oArrayList.Add("MotherTongue");
            oArrayList.Add("DOB");
            oArrayList.Add("DOBInText");
            oArrayList.Add("PlaceOfBirth");
            oArrayList.Add("Nationality");
            oArrayList.Add("ReligionId");
            oArrayList.Add("CastSubcaste");
            oArrayList.Add("CategoryId");
            oArrayList.Add("LastSchoolName");
            oArrayList.Add("LastStandard");
            oArrayList.Add("LastSchoolBoardName");
            oArrayList.Add("IsRecognised");
            oArrayList.Add("GuardianFirstName");
            oArrayList.Add("GuardianMiddleName");
            oArrayList.Add("GuardianLastName");
            oArrayList.Add("GuardianAge");
            oArrayList.Add("Address");
            oArrayList.Add("City");
            oArrayList.Add("State");
            oArrayList.Add("Pincode");
            oArrayList.Add("ResidentialPhone");
            oArrayList.Add("OfficePhone");
            oArrayList.Add("Mobile");
            oArrayList.Add("Mobile2");
            oArrayList.Add("EmailAddress");
            oArrayList.Add("LivingLocationId");
            oArrayList.Add("SNo");
            oArrayList.Add("AadharCardNo");           //new field add

            if (CheckForMandatoryFields(oDTStudentDetails))
            {
                ValidateCategory(oDTStudentDetails);
                // Loop through all the grid rows.
                for (int iRowCount = 0; iRowCount <= oDTStudentDetails.Rows.Count - 1; iRowCount++)
                {
                    int IArrayCount = 0;
                    string sForm = "";
                   
                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
                    DataRow oDataRow = oDTStudentDetails.Rows[iRowCount];

                    for (int iCount = 0; iCount < oArrayList.Count; iCount++)
                    {
                        if (IArrayCount < oArrayList.Count)
                        {
                            sAtrrName = oArrayList[IArrayCount].ToString();
                            attr = oDoc.CreateAttribute(sAtrrName);
                            if (sAtrrName == "SNo")
                            {
                                string sRowNo = iRowCount.ToString();
                                attr.Value = sRowNo;
                            }
                            else if (sAtrrName == "Sex")
                            {
                                string sValue = oDTStudentDetails.Rows[iRowCount][iCount].ToString();
                                attr.Value = sValue.Substring(0, 1);

                            }
                            else if (sAtrrName == "CategoryId")
                            {
                                string sCategory = oDTStudentDetails.Rows[iRowCount][iCount].ToString();
                                int iCategoryId = oMasterDataCollectionBL.GetCategoryIdForCategory(sCategory);                                
                                attr.Value = iCategoryId.ToString();
                            }
                            else if (sAtrrName == "ReligionId")
                            {
                                string sReligion = oDTStudentDetails.Rows[iRowCount][iCount].ToString();
                                int iReligionId = oMasterDataCollectionBL.GetReligionIdForReligionName(sReligion);
                                attr.Value = iReligionId.ToString();
                            }
                            else if (sAtrrName == "DOB")
                            {
                                string sDOBInText = oDTStudentDetails.Rows[iRowCount]["Date of Birth"].ToString();
                                sDOBInText = Convert.ToDateTime(sDOBInText).ToString();
                                attr.Value = sDOBInText.ToString();
                            }
                            else if (sAtrrName == "DOBInText")
                            {
                                string sDOBInText = oDTStudentDetails.Rows[iRowCount]["Date of Birth"].ToString();
                                sDOBInText = CommonUtility.GetDateInWords(Convert.ToDateTime(sDOBInText));
                                attr.Value = sDOBInText.ToString();
                                iCount--;
                            }
                            else if (sAtrrName == "LivingLocationId")
                            {
                                string sLivingLocation = oDTStudentDetails.Rows[iRowCount][iCount].ToString();
                                int iLivingLocationId = oMasterDataCollectionBL.GetLivingLocationIdForLivingLocationName(sLivingLocation);
                                attr.Value = iLivingLocationId.ToString();
                            }

                            else if (sAtrrName == "AadharCardNo")        //////aadhar card field
                            {
                                string sValue = oDTStudentDetails.Rows[iRowCount]["Aadhar Card No"].ToString().Trim();
                                attr.Value = sValue;
                            }  
                            else
                            {
                                if (sAtrrName == "FormNo")
                                    sForm = attr.Value = oDTStudentDetails.Rows[iRowCount][iCount].ToString();
                                if (sAtrrName == "Mobile")
                                    moManualMobileNo.Add(sForm, oDTStudentDetails.Rows[iRowCount][iCount].ToString());
                                attr.Value = oDTStudentDetails.Rows[iRowCount][iCount].ToString();                                
                            }
                           
                            oXmlNode.Attributes.Append(attr);
                            IArrayCount++;
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
        /// Gets parent information in xml format 
        /// </summary>
        /// <param name="oDTStudentDetails"></param>
        /// <param name="asRootElementName"></param>
        /// <param name="asElementName"></param>
        /// <returns></returns>
        private string GetXMLOfParentDetails(DataTable oDTStudentDetails, string asRootElementName, string asElementName)
        {
            const char C_FATHER = 'F';
            const char C_MOTHER = 'M';
            const string S_ELEMENT = "element";
            const int I_OTHER_OCCUPATION_ID = 5;
            const string S_OTHER_OCCUPATTION = "Other";

            XmlDocument oDoc = new XmlDocument();

            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            if (CheckResidanceTypeAndBloodGroup(oDTStudentDetails))
            {
                // Create a root level element.
                XmlElement root = oDoc.CreateElement(asRootElementName);
                XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asElementName, string.Empty);
                // Loop through all the grid rows.
                for (int iRowCount = 0; iRowCount <= oDTStudentDetails.Rows.Count - 1; iRowCount++)
                {

                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, string.Empty);


                    string sAtrrName = "FormNo";
                    XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Form No"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Father_Or_Mother";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = C_FATHER.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "First_Name";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father First Name"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Middle_Name";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Middle Name"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Last_Name";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Last Name"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Age";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Age"].ToString();
                    oXmlNode.Attributes.Append(attr);


                    sAtrrName = "Educational_Qualification";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Educational Qualification"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Mother_Tongue";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Mother Tongue"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Other_Lang_Spoken";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Other Languages Spoken"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "ReligionId";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    string sReligion = oDTStudentDetails.Rows[iRowCount]["Father Religion"].ToString();
                    int iReligionId = oMasterDataCollectionBL.GetReligionIdForReligionName(sReligion);
                    attr.Value = iReligionId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "OccupationId";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    string sParentOccupation = oDTStudentDetails.Rows[iRowCount]["Father Occupation"].ToString();
                    int iParentOccupationId = oMasterDataCollectionBL.GetParentOccupationIdForParentOccupationName(sParentOccupation);
                    if (iParentOccupationId == I_OTHER_OCCUPATION_ID && sParentOccupation != S_OTHER_OCCUPATTION)
                        attr.Value = 5.ToString();
                    else if (iParentOccupationId == I_OTHER_OCCUPATION_ID && sParentOccupation == S_OTHER_OCCUPATTION)
                        attr.Value = iParentOccupationId.ToString();
                    else
                        attr.Value = iParentOccupationId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Company_Name";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Name of the Father Company"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Occupation_Details";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Designation Type of Business"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Office_Phone_Number";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Office Telephone no"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Email_Address";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Email Address"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "FaxNumber";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Father Fax Number"].ToString();
                    oXmlNode.Attributes.Append(attr);



                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);

                    // Add the root node to document element. 
                    root.AppendChild(oXmlRootNode);

                    oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");

                    sAtrrName = "FormNo";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Form No"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Father_Or_Mother";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = C_MOTHER.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "First_Name";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother First Name"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Middle_Name";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Middle Name"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Last_Name";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Last Name"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Age";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Age"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Educational_Qualification";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Educational Qualification"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Mother_Tongue";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Mother Tongue"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Other_Lang_Spoken";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Other Languages Spoken"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "ReligionId";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    sReligion = oDTStudentDetails.Rows[iRowCount]["Mother Religion"].ToString();
                    iReligionId = oMasterDataCollectionBL.GetReligionIdForReligionName(sReligion);
                    attr.Value = iReligionId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "OccupationId";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    sParentOccupation = oDTStudentDetails.Rows[iRowCount]["Mother Occupation"].ToString();
                    iParentOccupationId = oMasterDataCollectionBL.GetParentOccupationIdForParentOccupationName(sParentOccupation);
                    if (iParentOccupationId == I_OTHER_OCCUPATION_ID && sParentOccupation != S_OTHER_OCCUPATTION)
                        attr.Value = 5.ToString();
                    else if (iParentOccupationId == I_OTHER_OCCUPATION_ID && sParentOccupation == S_OTHER_OCCUPATTION)
                        attr.Value = iParentOccupationId.ToString();
                    else
                        attr.Value = iParentOccupationId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Company_Name";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Name of the Company"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Occupation_Details";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Designation Type of Business"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Office_Phone_Number";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Office Telephone no"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Email_Address";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Email Address"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "FaxNumber";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Mother Fax Number"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "IsDeleted";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = "N";
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "OtherLivingLocation";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Other Living Location"].ToString();
                    oXmlNode.Attributes.Append(attr);

                    string sAddSiblingDetails = oDTStudentDetails.Rows[iRowCount]["Add Sibling Details"].ToString();

                    int iStandardId = Constants.I_ZERO;
                    int iDivisionId = Constants.I_ZERO;
                    string sSiblingStudentName = string.Empty;

                    if (sAddSiblingDetails == "Yes")
                    {
                        string sSiblingStandard = oDTStudentDetails.Rows[iRowCount]["Sibling Student Standard"].ToString();

                        if (sSiblingStandard != string.Empty)
                        {
                            DataSet oDtStandardCollection = new DataSet();
                            var oStandardCollectionBL = new StandardCollectionBL(moAdmissionInfoStruct.iSchoolId, moAdmissionInfoStruct.iAcademicYearId);
                            oDtStandardCollection = oStandardCollectionBL.GetAllStandardDivisionDetails();


                            CheckSiblingDetails(oDTStudentDetails, iRowCount, sSiblingStandard, oDtStandardCollection, out iStandardId, out iDivisionId);


                            sSiblingStudentName = oDTStudentDetails.Rows[iRowCount]["Sibling Student Name"].ToString();

                            if (sSiblingStudentName == string.Empty)
                                throw new InvalidItemDataException(S_EMPTY_SIBLING_STUDENT_NAME);
                        }
                        else
                            throw new InvalidItemDataException(S_EMPTY_SIBLING_STANDARD);
                    }
                    sAtrrName = "SiblingStudentStandard";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = iStandardId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "SiblingStudentDivision";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = iDivisionId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "SiblingStudentName";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = sSiblingStudentName;
                    oXmlNode.Attributes.Append(attr);


                    sAtrrName = "TwinsSelection";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    if (oDTStudentDetails.Rows[iRowCount]["Twins Selection"].ToString() == "Yes")
                        attr.Value = Constants.S_ONE;
                    else
                        attr.Value = Constants.S_ZERO;

                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "ResidenceType";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Residence Type"].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "BloodGroup";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Blood Group"].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "SecondLanguageSubject";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Second Language Subject"].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "ThirdLanguageSubject";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Third Language Subject"].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "LastSchoolAddress";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Last School Address"].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "LastSchoolUDISE"; 
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = oDTStudentDetails.Rows[iRowCount]["Last School UDISE No"].ToString().Trim();
                    oXmlNode.Attributes.Append(attr);

                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                }
                // Add the root node to document element. 
                root.AppendChild(oXmlRootNode);

                // return the string generated.
                return root.InnerXml;
            }
            return string.Empty;
        }

        private void CheckSiblingDetails(DataTable oDTStudentDetails, int iRowCount, string sSiblingStandard, DataSet oDtStandardCollection, out int iStandardId, out int iDivisionId)
        {
            string sStandard = string.Empty;
            string sDivision = string.Empty;            
            int iCounter = Constants.I_ZERO;

            iStandardId = Constants.I_ZERO;
            iDivisionId = Constants.I_ZERO;

            DataTable dtStandard = oDtStandardCollection.Tables[Constants.I_ZERO] as DataTable;

            for (int iVal = 0; iVal < dtStandard.Rows.Count; iVal++)
            {
                sStandard = dtStandard.Rows[iVal]["Standard_Name"].ToString();

                if (sSiblingStandard == sStandard)
                {
                    iStandardId = dtStandard.Rows[iVal]["Standard_Id"].ToInt();
                    iCounter = Constants.I_ONE;
                    break;
                }
            }

            if (iCounter == Constants.I_ZERO)
                throw new InvalidItemDataException(S_INVALID_SIBLING_STANDARD);
            else
            {
                string sSibilingStudentDivision = oDTStudentDetails.Rows[iRowCount]["Sibling Student Division"].ToString();
                int iDivCount = Constants.I_ZERO;

                if (sSibilingStudentDivision != string.Empty)
                {
                    DataTable dtDivision = oDtStandardCollection.Tables[Constants.I_ONE] as DataTable;

                    for (int iDiv = 0; iDiv < dtDivision.Rows.Count; iDiv++)
                    {
                        sDivision = dtDivision.Rows[iDiv]["Division_Name"].ToString();

                        if (sSibilingStudentDivision == sDivision)
                        {
                            iDivisionId = dtDivision.Rows[iDiv]["Division_Id"].ToInt();
                            iDivCount = Constants.I_ONE;
                            break;
                        }
                    }

                    if (iDivCount == Constants.I_ZERO)
                        throw new InvalidItemDataException(S_INVALID_SIBLING_DIVISON);
                    else
                    {
                        DataTable dtStandardDivision = oDtStandardCollection.Tables[Constants.I_TWO] as DataTable;

                        string sClassName = string.Empty;
                        string sDtClass = string.Empty;
                        int iStdDivCount = Constants.I_ZERO;
                        sClassName = sStandard.TrimAll() + "-" + sDivision.TrimAll();

                        for (int iStdDiv = 0; iStdDiv < dtStandardDivision.Rows.Count; iStdDiv++)
                        {
                            sDtClass = dtStandardDivision.Rows[iStdDiv]["className"].ToString();

                            if (sClassName == sDtClass)
                            {
                                iStdDivCount = Constants.I_ONE;
                                break;
                            }
                        }

                        if (iStdDivCount == Constants.I_ZERO)
                            throw new InvalidItemDataException(S_INVALID_SIBLING_CLASS);
                    }
                }
                else
                    throw new InvalidItemDataException(S_EMPTY_SIBLING_DIVISION);
            }
        }


        /// <summary>
        /// Gets parent information in xml format if parent involved in Association
        /// </summary>
        /// <param name="oDTStudentDetails"></param>
        /// <param name="asRootElementName"></param>
        /// <param name="asElementName"></param>
        /// <returns></returns>
        private string GetXMLOfParentInAssociationEvent(DataTable oDTStudentDetails, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");


            ArrayList oArrayList = new ArrayList();

            oArrayList.Add("Celebration");
            oArrayList.Add("NewsletterandPublicity");
            oArrayList.Add("CocurricularActivities");
            oArrayList.Add("ComputerSoftware");
            oArrayList.Add("ExcursionsandVisits");
            oArrayList.Add("Sports");
            oArrayList.Add("FormNo");
            oArrayList.Add("SNo");
            oArrayList.Add("AadharCardNo");   ////aadhar card field add
            // Loop through all the grid rows.
            for (int iRowCount = 0; iRowCount <= oDTStudentDetails.Rows.Count - 1; iRowCount++)
            {
                int iArrayCount = 0;
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
                DataRow oDataRow = oDTStudentDetails.Rows[iRowCount];

                for (int iCount = I_XLS_CELEBRATION; iCount < I_XLS_OTHER_LIVING_LOCATION; iCount++)
                {
                    sAtrrName = oArrayList[iArrayCount].ToString();
                    attr = oDoc.CreateAttribute(sAtrrName);
                    if (sAtrrName == "SNo")
                    {
                        string sRowNo = iRowCount.ToString();
                        attr.Value = sRowNo;
                    }
                    else if (sAtrrName == "FormNo")
                    {
						attr.Value = oDTStudentDetails.Rows[iRowCount][iCount].ToString();
                    }
                    else
                    {
						if(String.IsNullOrEmpty(oDTStudentDetails.Rows[iRowCount][iCount].ToString()))
							attr.Value = Constants.S_ZERO;
						else
							attr.Value = Convert.ToBoolean(oDTStudentDetails.Rows[iRowCount][iCount].ToString()) ? Constants.S_ONE : Constants.S_ZERO;
                    }
                    oXmlNode.Attributes.Append(attr);
                    iArrayCount++;
                }
                sAtrrName = oArrayList[iArrayCount].ToString();
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = oDTStudentDetails.Rows[iRowCount]["Form No"].ToString();
                oXmlNode.Attributes.Append(attr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);

            // return the string generated.
            return root.InnerXml;
        }

        /// <summary>
        /// This method is used to generate XML of net banking details.
        /// </summary>
        /// <param name="oDTStudentDetails"></param>
        /// <param name="asRootElementName"></param>
        /// <param name="asElementName"></param>
        /// <returns></returns>
        private string GetXMLOfOnlineTransactionDetails(DataTable oDTStudentDetails, string asRootElementName, string asElementName)
        {
            const string S_ELEMENT = "element";
            XmlDocument oDoc = new XmlDocument();
            string sAtrrName;
            XmlAttribute attr;
            // Create a root level element.
            XmlElement root = oDoc.CreateElement(asRootElementName);
            XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, asRootElementName, "");

            ArrayList oArrayList = new ArrayList();

            oArrayList.Add("NetBankingStatus");
            oArrayList.Add("TransactionId");
            oArrayList.Add("TransactionDateTime");
            oArrayList.Add("PaymentMode");
            oArrayList.Add("PaymentAmount");
            oArrayList.Add("FormNo");
           
            if (!CheckForNetbankingMandateroyFields(oDTStudentDetails))
            {
                for (int iRowCount = Constants.I_ZERO; iRowCount <= oDTStudentDetails.Rows.Count - 1; iRowCount++)
                {
                    // Create root xml element.
                    XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, asElementName, "");
                    DataRow oDataRow = oDTStudentDetails.Rows[iRowCount];

                    for (int iCount = Constants.I_ZERO; iCount < oArrayList.Count; iCount++)
                    {
                        sAtrrName = oArrayList[iCount].ToString().Trim();
                        attr = oDoc.CreateAttribute(sAtrrName);

                        if (sAtrrName == "NetBankingStatus")
                        {
                            string sValue = oDTStudentDetails.Rows[iRowCount]["Netbanking Status"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "TransactionId")
                        {
                            string sValue = oDTStudentDetails.Rows[iRowCount]["Netbanking Transaction Id"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "TransactionDateTime")
                        {
                            string sValue = oDTStudentDetails.Rows[iRowCount]["Transaction Date Time"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "PaymentMode")
                        {
                            string sValue = oDTStudentDetails.Rows[iRowCount]["Payment Mode"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        else if (sAtrrName == "PaymentAmount")
                        {
                            string sValue = oDTStudentDetails.Rows[iRowCount]["Payment Amount"].ToString().Trim();
                            attr.Value = sValue;
                        }                        
                        else if (sAtrrName == "FormNo")
                        {
                            string sValue = oDTStudentDetails.Rows[iRowCount]["Form No"].ToString().Trim();
                            attr.Value = sValue;
                        }
                        oXmlNode.Attributes.Append(attr);                        
                    }
                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                }
                root.AppendChild(oXmlRootNode);
                return root.InnerXml;
            }

            return string.Empty;
        }

        /// <summary>
        /// This method is used to check netbanking related fields are inserted or not
        /// </summary>
        /// <param name="dtStudentDetails"></param>
        /// <returns></returns>
        private bool CheckForNetbankingMandateroyFields(DataTable dtStudentDetails)
        {
            StringBuilder sStatus = new StringBuilder();
            StringBuilder sTransactionId = new StringBuilder();
            StringBuilder sTransactionDatetime = new StringBuilder();
            StringBuilder sPaymetMode = new StringBuilder();
            StringBuilder sPaymentAmount = new StringBuilder();

            for (int iRowcount = Constants.I_ZERO; iRowcount < dtStudentDetails.Rows.Count; iRowcount++)
            {
                if(dtStudentDetails.Rows[iRowcount]["Netbanking Status"].ToString().Trim() == string.Empty)
                    sStatus.Append((iRowcount + 1).ToString() + ", ");

                if (dtStudentDetails.Rows[iRowcount]["Netbanking Transaction Id"].ToString().Trim() == string.Empty)
                    sTransactionId.Append((iRowcount + 1).ToString() + ", ");

                if (dtStudentDetails.Rows[iRowcount]["Transaction Date Time"].ToString().Trim() == string.Empty)
                    sTransactionDatetime.Append((iRowcount + 1).ToString() + ", ");

                if (dtStudentDetails.Rows[iRowcount]["Payment Mode"].ToString().Trim() == string.Empty)
                    sPaymetMode.Append((iRowcount + 1).ToString() + ", ");

                if (dtStudentDetails.Rows[iRowcount]["Payment Amount"].ToString().Trim() == string.Empty)
                    sPaymentAmount.Append((iRowcount + 1).ToString() + ", ");
            }
            string sErrorMessage = string.Empty;

            if (sStatus.ToString().Length > Constants.I_ZERO)
                sErrorMessage = "Netbanking Status should not be blank for row(s) no. :" + sStatus.ToString();

            if (sTransactionId.ToString().Length > Constants.I_ZERO)
            { 
                if(sErrorMessage == string.Empty)
                    sErrorMessage = "Netbanking Transaction Id should not be blank for row(s) no. :" + sTransactionId.ToString();
                else
                    sErrorMessage += "<br />Netbanking Transaction Id should not be blank for row(s) no. :" + sTransactionId.ToString();
            }

            if (sTransactionDatetime.ToString().Length > Constants.I_ZERO)
            {
                if (sErrorMessage == string.Empty)
                    sErrorMessage = "Transaction Date Time should not be blank for row(s) no. :" + sTransactionDatetime.ToString();
                else
                    sErrorMessage += "<br />Transaction Date Time should not be blank for row(s) no. :" + sTransactionDatetime.ToString();
            }

            if (sPaymetMode.ToString().Length > Constants.I_ZERO)
            {
                if (sErrorMessage == string.Empty)
                    sErrorMessage = "Payment Mode should not be blank for row(s) no. :" + sPaymetMode.ToString();
                else
                    sErrorMessage += "<br />Payment Mode should not be blank for row(s) no. :" + sPaymetMode.ToString();
            }

            if (sPaymentAmount.ToString().Length > Constants.I_ZERO)
            {
                if (sErrorMessage == string.Empty)
                    sErrorMessage = "Payment Amount should not be blank for row(s) no. :" + sPaymentAmount.ToString();
                else
                    sErrorMessage += "<br />Payment Amount should not be blank for row(s) no. :" + sPaymentAmount.ToString();
            }

            if (sErrorMessage != string.Empty)
                throw new InvalidItemDataException(sErrorMessage);


            return false;
        }

        /// <summary>
        /// Checks Valid category is selected by user
        /// </summary>
        /// <param name="aoDTStudentDetails"></param>
        /// <returns></returns>
        private void ValidateCategory(DataTable aoDTStudentDetails)
        {
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            string sRowNumber = string.Empty;
            string sContents = string.Empty;
            for (int iRowcount = 0; iRowcount < aoDTStudentDetails.Rows.Count; iRowcount++)
            {
                if (oMasterDataCollectionBL.GetCategoryIdForCategory(aoDTStudentDetails.Rows[iRowcount][I_XLS_CATEGORY].ToString()) == 0)
                    sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
            }
            if (sRowNumber.Trim() != String.Empty)
                ThrowInvalidItemDataException(I_XLS_CATEGORY, sRowNumber.Substring(0, sRowNumber.Length - 2));
            
        }
        
        private bool ValidateMinAndMaxDOB(DateTime adtDOB)
        {         
		    if (mdtMinDOB == DateTime.MinValue && mdtMaxDOB == DateTime.MinValue)
                return true;

            if (mdtMinDOB == DateTime.MinValue || mdtMaxDOB == DateTime.MinValue)
            {
                if (mdtMinDOB != DateTime.MinValue && adtDOB >= mdtMinDOB)
					return true;
                
				if (mdtMaxDOB != DateTime.MinValue && adtDOB <= mdtMaxDOB)
					return true;
            }
			else if (adtDOB >= mdtMinDOB && adtDOB <= mdtMaxDOB)
				return true;

			return false;		
            
        }

	private string 	GetDOBValidationMessage()
	{
		if (mdtMinDOB != DateTime.MinValue && mdtMaxDOB != DateTime.MinValue)
			return String.Format("Date of birth should be greater than or equal to {0:dd-MMM-yyyy} and less than or equal to {1:dd-MMM-yyyy} in worksheet at row number : ", mdtMinDOB, mdtMaxDOB);
			
		if (mdtMinDOB != DateTime.MinValue)
			return String.Format("Date of birth should be greater than or equal to {0:dd-MMM-yyyy} in worksheet at row number : ", mdtMinDOB);

		return String.Format("Date of birth should be less than or equal to {0:dd-MMM-yyyy} in worksheet at row number : ", mdtMaxDOB);		
	}


        /// <summary>
        /// Checks for mandatory fields of student record
        /// </summary>
        /// <param name="aoDTStudentDetails"></param>
        /// <returns></returns>
        private bool CheckForMandatoryFields(DataTable aoDTStudentDetails)
        {
            for (int iColCount = 0; iColCount < aoDTStudentDetails.Columns.Count; iColCount++)
            {
                string sRowNumber = string.Empty;
                string sContents = string.Empty;
                for (int iRowcount = 0; iRowcount < aoDTStudentDetails.Rows.Count; iRowcount++)
                {
                    sContents = aoDTStudentDetails.Rows[iRowcount][iColCount].ToString();
                    if (sContents.Trim() == "")
                        sRowNumber = sRowNumber + (iRowcount + 1).ToString() + ", ";
                }
                if (sRowNumber.Trim() != "")
                    ThrowInvalidItemDataException(iColCount, sRowNumber.Substring(0, sRowNumber.Length - 2));
            }
            if (IsDuplicateData(aoDTStudentDetails))
                return false;
            return true;
        }

        private bool CheckResidanceTypeAndBloodGroup(DataTable aoDTStudentDetails)
        {
            MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
            DataSet dtData = oMasterDataCollectionBL.GetResidanceTypeMasterDataForadmission();

            StringBuilder obj1 = new StringBuilder();
            StringBuilder obj2 = new StringBuilder();
            StringBuilder obj3 = new StringBuilder();
            StringBuilder obj4 = new StringBuilder();

            for (int iRowcount = 0; iRowcount < aoDTStudentDetails.Rows.Count; iRowcount++)
            {
                int iRow = iRowcount;
                string sResidanceType = aoDTStudentDetails.Rows[iRowcount]["Residence Type"].ToString();
                string sBloodGroup = aoDTStudentDetails.Rows[iRowcount]["Blood Group"].ToString();
                string sSecondLanguage = aoDTStudentDetails.Rows[iRowcount]["Second Language Subject"].ToString();
                string sThirdLanguage = aoDTStudentDetails.Rows[iRowcount]["Third Language Subject"].ToString();

                if (sResidanceType != string.Empty && sResidanceType != null)
                {
                    DataRow[] dr = dtData.Tables[0].Select("Name='" + aoDTStudentDetails.Rows[iRowcount]["Residence Type"].ToString() + "'");
                    if (dr.Length == Constants.I_ZERO)
                        obj1.Append((iRowcount + 1).ToString() + ", ");
                }

                if (sBloodGroup != string.Empty && sBloodGroup != null)
                {
                    DataRow[] blood = dtData.Tables[1].Select("BloodGroup='" + aoDTStudentDetails.Rows[iRowcount]["Blood Group"].ToString() + "'");
                    if (blood.Length == Constants.I_ZERO)
                        obj2.Append((iRowcount + 1).ToString() + ", ");
                }

                if (sSecondLanguage != string.Empty && sSecondLanguage != null)
                {
                    DataRow[] Secondlanguage = dtData.Tables[2].Select("Subject_Name='" + aoDTStudentDetails.Rows[iRowcount]["Second Language Subject"].ToString() + "'");
                    if (Secondlanguage.Length == Constants.I_ZERO)
                        obj3.Append((iRowcount + 1).ToString() + ", ");
                }

                if (sThirdLanguage != string.Empty && sThirdLanguage != null)
                {
                    DataRow[] Thirdlanguage = dtData.Tables[2].Select("Subject_Name='" + aoDTStudentDetails.Rows[iRowcount]["Third Language Subject"].ToString() + "'");
                    if (Thirdlanguage.Length == Constants.I_ZERO)
                        obj4.Append((iRowcount + 1).ToString() + ", ");
                }
            }

            string sErrorMessage = string.Empty;

            if (obj1.ToString().Length > Constants.I_ZERO)
            {
                sErrorMessage = "Please enter valid Residence Type for row no. - " + obj1.ToString();
            }

            if (obj2.ToString().Length > Constants.I_ZERO)
            {
                if (sErrorMessage == string.Empty)
                    sErrorMessage = "Please enter valid Blood Group for row no. - " + obj2.ToString();
                else
                    sErrorMessage += "<br />Please enter valid Blood Group for row no. - " + obj2.ToString();
            }

            if (obj3.ToString().Length > Constants.I_ZERO)
            {
                if (sErrorMessage == string.Empty)
                    sErrorMessage = "Please enter valid Second Language for row no. - " + obj3.ToString();
                else
                    sErrorMessage += "<br />Please enter valid Second Language for row no. - " + obj3.ToString();
            }

            if (obj4.ToString().Length > Constants.I_ZERO)
            {
                if (sErrorMessage == string.Empty)
                    sErrorMessage = "Please enter valid Third Language for row no. - " + obj4.ToString();
                else
                    sErrorMessage += "<br />Please enter valid Third Language for row no. - " + obj4.ToString();
            }

            if (sErrorMessage != string.Empty)
                throw new InvalidItemDataException(sErrorMessage);                         

            return true;
        }

        /// <summary>
        /// Checks if duplicate data is present in worksheet
        /// </summary>
        /// <param name="aoDTStudentDetails"></param>
        /// <returns></returns>
        private bool IsDuplicateData(DataTable aoDTStudentDetails)
        {
            int iRecordCount = aoDTStudentDetails.Rows.Count;
            string sRowNoFormNo = string.Empty;
            string sRowNoStudent = string.Empty;
            string sRowNoBOD = string.Empty;
            string sRowEmailId = string.Empty;
            string sErrorMessage = string.Empty;
            DataRow[] oDR;

            StudentAdmissionsCollectionBL oStudentAdmissionsCollectionBL = new StudentAdmissionsCollectionBL(moAdmissionInfoStruct.iSchoolId, moAdmissionInfoStruct.iAcademicYearId,
                                                   moAdmissionInfoStruct.iStandardId, moAdmissionInfoStruct.iUserId);

            DataTable oDTNewStudentDetails = oStudentAdmissionsCollectionBL.GetNewAdmissionListForImport();
            StudentAdmissionsDC oStudentAdmissionsDC = new StudentAdmissionsDC();

            DateTime dtMinDOB, dtMaxDOB;
            oStudentAdmissionsDC.GetMinMaxDOBforStandard(moAdmissionInfoStruct.iSchoolId, moAdmissionInfoStruct.iAcademicYearId, moAdmissionInfoStruct.iStandardId, out dtMinDOB, out dtMaxDOB);
            mdtMinDOB = dtMinDOB;
            mdtMaxDOB = dtMaxDOB;			
            for (int iIndex = 0; iIndex < iRecordCount; iIndex++)
            {
                // Used to check form no 
                string sFormNo = aoDTStudentDetails.Rows[iIndex]["Form No"].ToString();
                oDR = oDTNewStudentDetails.Select("Form_Number ='" + sFormNo + "'");
                if (oDR.Length > 0)
                    sRowNoFormNo = sRowNoFormNo + (iIndex + 1).ToString() + ", ";

                // Used to check student
                string sStudentFirstName = aoDTStudentDetails.Rows[iIndex]["Student First Name"].ToString();
                string sStudentLastName = aoDTStudentDetails.Rows[iIndex]["Student Last Name"].ToString();
              DateTime  dtStudentDOB = Convert.ToDateTime(aoDTStudentDetails.Rows[iIndex]["Date of Birth"].ToString());
                string sEmailId = aoDTStudentDetails.Rows[iIndex]["Email Address"].ToString();

                string sformNumber = aoDTStudentDetails.Rows[iIndex]["Form No"].ToString();
                if (AllowDuplicateStudent)
                {
                    oDR = oDTNewStudentDetails.Select("First_Name ='" + sStudentFirstName.Trim() + "' " +
                                                                          "AND Last_Name ='" + sStudentLastName.Trim() + "' " +
                                                                          "AND Form_Number ='" + sformNumber.Trim() + "' " +
                                                                          "AND DOB ='" + dtStudentDOB + "' ");
                }
                else
                {
                    oDR = oDTNewStudentDetails.Select("First_Name ='" + sStudentFirstName.Trim() + "' " +
                                                      "AND Last_Name ='" + sStudentLastName.Trim() + "' " +
                                                      "AND DOB ='" + dtStudentDOB + "' ");
                }
                if (oDR.Length > 0)
                    sRowNoStudent = sRowNoStudent + (iIndex + 1).ToString() + ", ";
                // Used to check the age of the student
                // S_MIN_DOB needs to be updated each year, according to the rule set by the school for minimum dob for new admissions.           

                if (!ValidateMinAndMaxDOB(dtStudentDOB))
                    sRowNoBOD = sRowNoBOD + (iIndex + 1).ToString() + ", ";

                // Validate the email address
                if (!Regex.Match(sEmailId, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.None).Success)
                {
					sRowEmailId = sRowEmailId + (iIndex + 1).ToString() + ", ";
                }
            }
            if (sRowNoFormNo != string.Empty)
            {
                sRowNoFormNo = sRowNoFormNo.Substring(0, sRowNoFormNo.Length - 2);
                sErrorMessage = S_INVALID_FORM_NO + sRowNoFormNo + ".<br>";
            }
            if (sRowNoStudent != string.Empty)
            {
                sRowNoStudent = sRowNoStudent.Substring(0, sRowNoStudent.Length - 2);
                sErrorMessage = sErrorMessage + S_INVALID_STUDENT + sRowNoStudent + ".<br>";
            }
            if (sRowNoBOD != string.Empty)
            {
				string sDOBValidationMessage = GetDOBValidationMessage();
                sRowNoBOD = sRowNoBOD.Substring(0, sRowNoBOD.Length - 2);
				sErrorMessage = sErrorMessage + sDOBValidationMessage + sRowNoBOD + ".<br>";
            }
            if (sErrorMessage != string.Empty)
                throw new InvalidItemDataException(sErrorMessage);

            return false;
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
        /// This method is used to throw an appropriate exception.
        /// </summary>
        /// <param name="iColCount"></param>
        /// <param name="sRowNumber"></param>
        private void ThrowInvalidItemDataException(int iColCount, string sRowNumber)
        {
            switch (iColCount)
            {
                case I_XLS_FORM_NO:
                    throw new InvalidItemDataException(S_NULL_FORM_NO + sRowNumber + ".");
                case I_XLS_STUDENT_FIRST_NAME:
                    throw new InvalidItemDataException(S_NULL_STUDENT_FIRST_NAME + sRowNumber + ".");
                case I_XLS_SEX:
                    throw new InvalidItemDataException(S_NULL_SEX + sRowNumber + ".");
                case I_XLS_DOB:
                    throw new InvalidItemDataException(S_NULL_DOB + sRowNumber + ".");				
                case I_XLS_CATEGORY:
                    throw new InvalidItemDataException(S_VALID_CATEGORY + sRowNumber + ".");
                case I_XLS_ADDRESS:
                    throw new InvalidItemDataException(S_NULL_ADDRESS + sRowNumber + ".");
                case I_XLS_CITY:
                    throw new InvalidItemDataException(S_NULL_CITY + sRowNumber + ".");
                case I_XLS_STATE:
                    throw new InvalidItemDataException(S_NULL_STATE + sRowNumber + ".");
                case I_XLS_PINCODE:
                    throw new InvalidItemDataException(S_NULL_PINCODE + sRowNumber + ".");
                case I_XLS_MOBILE:
                    throw new InvalidItemDataException(S_NULL_MOBILE + sRowNumber + ".");;
				case I_XLS_LIVING_LOCATION:
					throw new InvalidItemDataException(S_NULL_LIVING_LOCATION + sRowNumber + ".");
				case I_XLS_FATHER_FIRST_NAME:
					throw new InvalidItemDataException(S_NULL_FATHER_FIRST_NAME + sRowNumber + ".");
                case I_XLS_FATHER_OCCUPATION:
                    throw new InvalidItemDataException(S_NULL_FATHER_OCCUPATION + sRowNumber + ".");
                case I_XLS_MOTHER_FIRST_NAME:
                    throw new InvalidItemDataException(S_NULL_MOTHER_FIRST_NAME + sRowNumber + ".");
                case I_XLS_NO_RECORD_FOUND:
                    throw new InvalidItemDataException(S_NO_RECORD_FOUND);
                case I_XLS_INVALID_FORMAT:
					throw new InvalidItemDataException(S_INVALID_FORMAT);
            }
        }



        #endregion " Private Methods "
    }
}
