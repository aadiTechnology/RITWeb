/*
 * File Name         :- SchoolDC.cs
 * Purpose           :- This Class is used to perform all the database related operations regarding 
 *                      Company_Master table.
 * Date of creation  :- 16-April-2007.  
 * Update Date       :- 07-07-07
 *                      Insert the delete method by Mahesh.
*/

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Utility;
using System.Collections.Generic;
using SchoolEntities;
using MasterEntities;
using System.Configuration;
using System.Collections.ObjectModel;

namespace DataCommunicator
{
    public class SchoolDC : DataCommunicatorBaseDC
    {
        #region Constants & Structure

        public struct SchoolInfo
        {
            // This structure is replica of Company_Master database table.
            public Int32 SchoolId;
            public string SchoolName;
            public string SMSSenderName;
            public string SMSSenderMobileNo;
            public int AllowedSMSCount;
            public DateTime SubscriptionDate;
            public string SchoolOrgnName;
            public string Address;
            public string RegNo;
            public DateTime SchoolSinceDate;
            public string City;
            public string StateName;
            public string PhoneNo;
            public string PhoneNo2;
            public string Pincode;
            public string sAccountNo;
            public string msPTRegCertificateNo;
            public string InsertedBy;
            public string UpdatedBy;
            public System.DateTime mdtInsertDate;
            public System.DateTime mdtUpdateDate;
            public string sWebSite;
            public string sFaxNumber;
            public string sAddress1;
            public string sEmail;
            public string sAddress2;
            public string sLogoPath;
            public string sSignLogo;
            public string sICardLogo;
            public string SchoolRecNoPrimary;
            public string SchoolRecNoSecondary;
            public string IndexNo;
            public int AdminId;
            public int AdminRoleId;
            public string sFeedbackEmail;
            public string sCareerEmails;
            public string sForgotPasswordEmail;
            public string PanNo;
            public string TanNo;
            public string GSTIN;
            public string UDISENumber;
            public string Lattitude;
            public string Longitude;
        }

        #endregion Constants & Structure

        #region DataMembers & Properties

        #region DataMembers
        // Member variable Declaration of company structure.
        private SchoolInfo moSchoolInfo;
        private SchoolUserDC moSchoolUserDC = new SchoolUserDC();

        #endregion

        #region Properties

        public SchoolInfo SchoolDetails
        {
            get
            {
                return moSchoolInfo;
            }
            set
            {
                moSchoolInfo = value;
            }
        }

        public SchoolUserDC CompanyUserDetails
        {
            set
            {
                moSchoolUserDC = value;
            }
        }


        #endregion Properties

        #endregion  DataMembers & Properties

        #region Overloaded Constructors
        public SchoolDC()
        {
            //Default constructor.
            moSchoolInfo.SchoolId = 0;
        }

        public SchoolDC(int aiSchoolId)
        {
            LoadSchoolDetails(aiSchoolId);
        }

        #endregion Overloaded Constructors

        #region Public Method

        public bool CheckIfSchoolNameExists()
        {
            string sWhere = "";
            if (moSchoolInfo.SchoolId != 0)
            {
                sWhere = " AND School_Id<> N'" + moSchoolInfo.SchoolId + "'";
            }
            string sSelectStatement;
            bool bReturn;
            sSelectStatement = " SELECT " +
                                    " Count(*) " +
                               " FROM " +
                                    " school_master" +
                               " WHERE " +
                                    "School_Name =N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolName, false) + "'" +
                                    " AND is_deleted = N'" + Constants.C_NO + "'" +
                                    sWhere;
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            if (iCount > 0)
                bReturn = true;
            else
                bReturn = false;
            return bReturn;
        }

        /// <summary>
        /// This method is used check duplication of sender name to the school for sms sending
        /// </summary>
        /// <returns></returns>
        public bool CheckIfSMSSenderNameExists()
        {
            string sSelectStatement;
            bool bReturn;
            sSelectStatement = " SELECT " +
                                    " Count(*) " +
                               " FROM " +
                                    " school_master" +
                               " WHERE " +
                                    "SMSSender_Name =N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SMSSenderName, false) + "'" +
                                    " AND is_deleted = N'" + Constants.C_NO + "'";
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            if (iCount > 0)
                bReturn = true;
            else
                bReturn = false;
            return bReturn;
        }

        // THis method is used to get Menu details.
        /// </summary>
        /// <param name="PageName"></param>
        /// <returns></returns>
        public List<StudentsCornerMenu> GetStudentsCornerMenuDetails(string asPageName, int aiSchoolId)
        {
            List<StudentsCornerMenu> lstStudentsCornerMenu = new List<StudentsCornerMenu>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("PageName", asPageName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetConfigMenuDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        StudentsCornerMenu oStudentsCornerMenu = new StudentsCornerMenu
                        {
                            ConfigureMenuId = Convert.ToInt32(oSqlDataReader["ConfigureMenuId"]),
                            ConfigureMenuName = oSqlDataReader["ConfigureMenuName"].ToString()
                        };
                        lstStudentsCornerMenu.Add(oStudentsCornerMenu);
                    }
                }
            }
            return lstStudentsCornerMenu;
        }

        /// <summary>
        /// This method will fetch all the settings from the database for all the academic years. It will simply convert the result into a List object and return it.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<SchoolSettings> GetSchoolSettings(int aiSchoolId)
        {
            List<SchoolSettings> lstSchoolSettings = new List<SchoolSettings>();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSchoolSettings"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstSchoolSettings.Add(new SchoolSettings { Id = Convert.ToInt32(oSqlDataReader["Id"]), Name = Convert.ToString(oSqlDataReader["Name"]), Value = Convert.ToString(oSqlDataReader["Value"]), AcademicYearId = Convert.ToInt32(oSqlDataReader["AcademicYearId"]),
                                                                   PossibleValues = Convert.ToString(oSqlDataReader["PossibleValues"]),
                                                                   Description = Convert.ToString(oSqlDataReader["Description"])
                        });
                    }
                }
            }
            return lstSchoolSettings;
        }

        /// <summary>
        ///  This method returns the All school Modules Name.
        /// </summary>
        /// <returns></returns>
        public List<SchoolModule> GetAllModuleSetting()
        {
            List<SchoolModule> lstSchoolModule = new List<SchoolModule>();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllModule"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstSchoolModule.Add(new SchoolModule
                        {
                            Name = Convert.ToString(oSqlDataReader["SchoolModulesName"]),
                            Id = Convert.ToInt32(oSqlDataReader["SchoolModulesId"]),
                            IsActive=Convert.ToBoolean(oSqlDataReader["IsActive"])
                        });
                    }
                }
            }
            return lstSchoolModule;
        }
        
        /// <summary>
        /// This method is used for get setting's value of current acdemic year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="sKeyName"></param>
        /// <returns></returns>
        public string GetSchoolSettingByName(int aiSchoolId, string sKeyName)
        {
            string settingKeyValue = string.Empty;
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("KeyName", sKeyName, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSchoolSettingsByName"))
                {
                    while (oSqlDataReader.Read())
                    {
                        settingKeyValue = oSqlDataReader["Value"].ToString();
                    }
                }
            }
            return settingKeyValue;
        }

        public Int32 InsertSchoolRegistrationDetails(ArrayList aoArrayListInsertStatement)
        {
            // This method insert newly registered company details,default user details and Registration
            // Payment details at once by using transaction.
            string sInsertSQL = " INSERT INTO school_master " +
                                "(" +
                                        " school_orgn_name " +
                                        " , school_since_date " +
                                        " , school_name " +
                                        " , SMSSender_Name " +
                                        " , SMSSender_MobileNo " +
                                        " , Registration_No " +
                                        " , address " +
                                        " , city " +
                                        " , pincode " +
                                        " , state_Name " +
                                        " , phone_number " +
                                        " , phone_number2 " +
                                        " , SchoolRecNoPrimary " +
                                        " , SchoolRecNoSecondary " +
                                        " , IndexNo " +
                                        " , inserted_by_id " +
                                        " , updated_by_id " +
                                        " , FaxNumber " +
                                        " , WebSite " +
                                        " , Address1 " +
                                        " , Email " +
                                        " ,FeedbackEmail " +
                                        " ,CareerEmails " +
                                        " , Address2 " +
                                        " , AccountNo" +
                                         " , PanNo " +
                                        " , TanNo" +
                                        " , GSTIN" +
                                        " , UDISENumber" +
                                        " , Lattitude" +
                                        " , Longitude" +

                                        " ,ForgotPasswordEmails " +
                                " )" +
                                " VALUES " +
                                "( " +
                                        " N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolOrgnName, true) + "'" +
                                        ", N'" + StringUtility.ReplaceDefaultDateToNull(moSchoolInfo.SchoolSinceDate) + "' " +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolName, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SMSSenderName, false) + "'" +
                                        ", N'" + null + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.RegNo, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Address, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.City, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Pincode.ToString(), false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.StateName.ToString(), false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.PhoneNo, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.PhoneNo2, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolRecNoPrimary, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolRecNoSecondary, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.IndexNo, false) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.InsertedBy, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.UpdatedBy, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sFaxNumber, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sWebSite, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sAddress1, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sEmail, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sFeedbackEmail, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sCareerEmails, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sAddress2, true) + "'" +
                                        ", N'" + moSchoolInfo.sAccountNo + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.PanNo, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.TanNo, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.GSTIN, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.UDISENumber, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Lattitude, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Longitude, true) + "'" +
                                        ", N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sForgotPasswordEmail, true) + "'" +

                                 ")";

            string sInsertYearSQL = "INSERT INTO SchoolWise_Academic_Year_Master " +
                                     "(" +
                                        "  school_id" +
                                        " , start_date" +
                                        " , end_date" +
                                        " , school_reopen_date" +
                                        " , is_current_year" +
                                        " , Is_NewlyCreated" +
                                        " , Is_FinalYear_Generated" +
                                        " , is_deleted" +
                                        " , SentSMS_Count" +
                                        " , insert_date" +
                                        " , inserted_by_id" +
                                        " , updated_by_id" +

                                " )" +
                                " VALUES " +
                                "( " +
                                        "  N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                                        ", dbo.GetLocalDate(DEFAULT)" +
                                        ", DATEADD(Year,1,dbo.GetLocalDate(DEFAULT))" +
                                        ", DATEADD(Month,14,dbo.GetLocalDate(DEFAULT))" +
                                        ", N'" + Constants.C_YES + "'" +
                                        ", N'" + Constants.C_NO + "'" +
                                        ", N'" + Constants.C_YES + "'" +
                                        ", N'" + Constants.C_NO + "'" +
                                        ", " + Constants.I_ZERO + " " +
                                        ", dbo.GetLocalDate(DEFAULT) " +
                                        ", N'" + moSchoolInfo.InsertedBy + "'" +
                                        ", N'" + moSchoolInfo.UpdatedBy + "'" +
                                 ")";

            string sInsertNoticeBoardMsg = "INSERT INTO Notice_Board " +
                                                 "(" +
                                                    "  [Message]" +
                                                    " , School_Id" +
                                                    " , Academic_Year_Id" +
                                                    " , Start_Date" +
                                                    " , End_Date" +
                                                    " , Is_Default_Msg" +
                                                    " , Inserted_By_Id" +
                                            " )" +
                                            " VALUES" +
                                            "( " +
                                                    "N'" + Constants.S_NOTICE_BOARD_MESSAGE + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolName, false) + "'" +
                                                    " , N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                                                    " , N'" + Constants.S_LAST_INSERTED_P_KEY2 + "'" +
                                                    ", dbo.GetLocalDate(DEFAULT)" +
                                                    ", DATEADD(Year,1,dbo.GetLocalDate(DEFAULT))" +
                                                    ", N'" + Constants.I_ONE + "'" +
                                                    ", N'" + moSchoolInfo.InsertedBy + "'" +
                                                    ")";
            string sInsertStmtNoticeBordRole = " INSERT INTO [dbo].[Notice_Board_Roles] " +
                                              " ([Message_Id] " +
                                              " ,[User_Role_Id] " +
                                              " ,[Is_Deleted] " +
                                              " ,[Insert_Date] " +
                                              " ,[Inserted_By_Id] " +
                                              " ,[Update_Date] " +
                                              " ,[Updated_By_Id]) " +
                                         " VALUES " +
                                              " (1 " +
                                              " ,1 " +
                                              " ,0 " +
                                              " ,dbo.GetLocalDate(DEFAULT) " +
                                              " ,1 " +
                                              " ,dbo.GetLocalDate(DEFAULT) " +
                                              " ,1) ";
            string sInsertPTRegCertificateNo = "INSERT INTO PTRegistrationCertificateNoMaster " +
                                                 "(" +
                                                    "  PTRegCertificateNo" +
                                                    " , SchoolId" +
                                                    " , Is_Deleted " +

                                            " )" +
                                            " VALUES" +
                                            "( " +
                                                    "N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.msPTRegCertificateNo, false) + "'" +
                                                    " , N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                                                    ", N'" + "N" + "'" +
                                                    ")";

            aoArrayListInsertStatement.Insert(0, sInsertSQL);
            aoArrayListInsertStatement.Insert(1, GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY));
            aoArrayListInsertStatement.Insert(2, sInsertYearSQL);
            aoArrayListInsertStatement.Insert(3, GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY2));
            aoArrayListInsertStatement.Insert(4, GetInsertForConfigurationSchoolMaster(Convert.ToInt32(Constants.SchoolConfigurations.AcademicYear)));
            aoArrayListInsertStatement.Insert(5, GetInsertForConfigurationSchoolMaster(Convert.ToInt32(Constants.SchoolConfigurations.Menu)));
            aoArrayListInsertStatement.Insert(6, sInsertNoticeBoardMsg);
            aoArrayListInsertStatement.Insert(7, sInsertPTRegCertificateNo);
            aoArrayListInsertStatement.Insert(8, sInsertStmtNoticeBordRole);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatement.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to insert configuration details of academic year and menu
        /// to configuration school master.
        /// </summary>
        /// <returns></returns>
        public string GetInsertForConfigurationSchoolMaster(int aiOriginalConfigId)
        {
            string iSchoolId = Constants.S_LAST_INSERTED_P_KEY;
            string iAcademicYrId = Constants.S_LAST_INSERTED_P_KEY2;

            string sInsertStatement = "INSERT INTO Configuration_School_Master ( " +
                "  original_config_id" +
                " , school_id" +
                " , is_configure" +
                " , academic_year_id" +

            ") VALUES (" +
                 "  " + aiOriginalConfigId +
                 " , " + iSchoolId +
                 " , N'" + Constants.C_YES + "' " +
                 " , " + iAcademicYrId +
            " ) ";
            return sInsertStatement;
        }

        /// <summary>
        /// This method is used to get Mobile numbers of current login Admin person.
        /// </summary>
        /// <param name="amiSchoolId"></param>
        /// <param name="asUserid"></param>
        /// <returns></returns>
        public string GetMobileNo(int amiSchoolId, int aiUserid)
        {
            string sSelectStatement;
            sSelectStatement = "SELECT Mobile_Number FROM user_master WHERE user_id =" + aiUserid +
                             "and school_id =" + amiSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);


        }

        public Int32 UpdateSchoolInformation()
        {
            ArrayList aoArrayListUpdateStatement = new ArrayList();
            string sUpdate_School_Master;
            sUpdate_School_Master = "UPDATE School_Master SET " +
                                    " school_name = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolName, false) + "'" +
                                    " , SMSSender_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SMSSenderName, false) + "'" +
                                    " , SMSSender_MobileNo =N'" + null + "'" +
                                    " , school_orgn_name = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolOrgnName, true) + "'" +
                                    " , school_since_date = N'" + moSchoolInfo.SchoolSinceDate.ToString(Constants.S_DATE_FORMAT_MARATHI) + "'" +
                                    " , Address = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Address, false) + "'" +
                                    " , City = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.City, false) + "'" +
                                    " , pincode = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Pincode, false) + "'" +
                                    " , State_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.StateName, false) + "'" +
                                    " , Phone_Number = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.PhoneNo, false) + "'" +
                                    " , Phone_Number2 = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.PhoneNo2, false) + "'" +
                                    " , SchoolRecNoPrimary = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolRecNoPrimary, false) + "'" +
                                    " , SchoolRecNoSecondary =N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.SchoolRecNoSecondary, false) + "'" +
                                    " , IndexNo =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.IndexNo, false) + "'" +
                                    " , Registration_No = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.RegNo, false) + "'" +
                                    " , inserted_by_id = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.InsertedBy, false) + "'" +
                                    " , updated_by_id = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.UpdatedBy, false) + "'" +
                                    " , Update_Date = N'" + moSchoolInfo.mdtUpdateDate.ToString(Constants.S_DATE_FORMAT_MARATHI) + "'" +
                                    " , FaxNumber = N'" + moSchoolInfo.sFaxNumber + "' " +
                                    " , WebSite = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sWebSite, true) + "'" +
                                    " , Address1 = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sAddress1, true) + "'" +
                                    " , Email = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sEmail, true) + "'" +
                                    ", FeedbackEmail = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sFeedbackEmail, true) + "'" +
                                    ", CareerEmails = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sCareerEmails, true) + "'" +
                                    " , Address2 = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sAddress2, true) + "'" +
                                    " , AccountNo = N'" + moSchoolInfo.sAccountNo + "'" +
                                    " , LogoPath = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sLogoPath, true) + "'" +
                                    " , PanNo =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.PanNo, false) + "'" +
                                    " , TanNo =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.TanNo, false) + "'" +
                                    " , GSTIN =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.GSTIN, false) + "'" +
                                    " , UDISENumber =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.UDISENumber, false) + "'" +
                                    " , Lattitude =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Lattitude, false) + "'" +
                                    " , Longitude =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Longitude, false) + "'" +
                                    " , ForgotPasswordEmails =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sForgotPasswordEmail, false) + "'" +
                          " WHERE " +
                                 " school_id = " + moSchoolInfo.SchoolId +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";


            string sUpdate_PTRegistrationCertificateNoMaster = "UPDATE PTRegistrationCertificateNoMaster SET " +
                                    " PTRegCertificateNo = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.msPTRegCertificateNo, false) + "'" +
                               " WHERE " +
                                 " schoolid = " + moSchoolInfo.SchoolId +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";

            aoArrayListUpdateStatement.Insert(0, sUpdate_School_Master);
            aoArrayListUpdateStatement.Insert(1, sUpdate_PTRegistrationCertificateNoMaster);

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListUpdateStatement.ToArray(typeof(string)));
        }

        /// <summary>
        /// Update ICard Image path.
        /// </summary>
        public void UpdateIcardDetails()
        {
            string sUpdateStatement;
            sUpdateStatement = " UPDATE School_Master SET " +
                               ", ICardLogo = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sICardLogo, true) + "'" +
                               ", Address = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Address, false) + "'" +
                                   " WHERE " +
                                 " school_id = " + moSchoolInfo.SchoolId +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }
        /// <summary>
        /// Update Principal Sign Image path.
        /// </summary>
        public void UpdatePrincipalSignDetails()
        {
            string sUpdateStatement;
            sUpdateStatement = " UPDATE School_Master SET " +
                               " SignPath = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sSignLogo, true) + "'" +
                               ", Address = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Address, false) + "'" +
                                   " WHERE " +
                                 " school_id = " + moSchoolInfo.SchoolId +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public void UpdatePrincipalSignAndIcardDetails()
        {
            string sUpdateStatement;
            sUpdateStatement = " UPDATE School_Master SET " +
                               " SignPath = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sSignLogo, true) + "'" +
                               ", ICardLogo = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sICardLogo, true) + "'" +
                               ", Address = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.Address, false) + "'" +
                                   " WHERE " +
                                 " school_id = " + moSchoolInfo.SchoolId +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public void DeleteCompanyByComopanyID(int aiSchoolId)
        {
            //This function is used to Delete the Company by Company ID.
            string DeleteString = "DELETE FROM Company_Master WHERE company_id=" + aiSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(DeleteString);
        }

        /// <summary>
        /// Get list of all schools which are registered to our school 
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSchoolForActivation()
        {
            string sSelectStatement = " SELECT " +
                                      " School_Name " +
                                      " , Is_Active" +
                                      " , School_Id " +
                                      " , AllowedSMS_Count " +
                                      " , SentSMS_Count " +
                                      " , Subscription_Date " +
                                  " FROM " +
                                      "vw_GetAllSchoolForActivation" +
                                   " ORDER BY School_Name ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

        }

        /// <summary>
        /// This method is used to get the current financial Year id
        /// </summary>
        /// <returns></returns>
        public int GetCurrentFinancialYrId(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("FinancialYearId", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GetCurrentFinancialYrId");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        /// <summary>
        /// This method is used to get schoolwise academic year details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataTable GetSchoolAcademicDetails(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSchoolDetails");
            }
        }

        public static DataSet GetTimeTableDetails(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTimeTableDetails");
            }
        }

        public static List<PhotoMaster> GetUserBinaryPhoto(int aiUserId, int aiSchoolId, int aiAcademicYearId, int aiPhotoTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<PhotoMaster> lstPhotoMaster = new List<PhotoMaster>();
                PhotoMaster oPhotoMaster = null;
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PhotoTypeId", aiPhotoTypeId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetBinaryImages"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oPhotoMaster = new PhotoMaster
                        {
                            UserId = oSqlDataReader["UserId"].ToInt(),
                            TotalBytes = oSqlDataReader["TotalBytes"] as byte[]
                        };
                        lstPhotoMaster.Add(oPhotoMaster);
                    }
                }
                return lstPhotoMaster;
            }
        }

        public static List<PhotoMaster> GetGuestsBinaryPhoto(int aiGuestId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<PhotoMaster> lstPhotoMaster = new List<PhotoMaster>();
                PhotoMaster oPhotoMaster = null;
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("GuestId", aiGuestId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetGuestBinaryImage"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oPhotoMaster = new PhotoMaster
                        {
                            UserId = oSqlDataReader["UserId"].ToInt(),
                            TotalBytes = oSqlDataReader["TotalBytes"] as byte[]
                        };
                        lstPhotoMaster.Add(oPhotoMaster);
                    }
                }
                return lstPhotoMaster;
            }
        }

        public SchoolEntity GetSchoolDetails(SqlDataReader aoSqlDataReader)
        {
            SchoolEntity oSchoolEntity = new SchoolEntity();
            if (aoSqlDataReader.Read())
            {
                oSchoolEntity = new SchoolEntity
                {
                    OrganizationName = aoSqlDataReader["School_Orgn_Name"].ToString(),
                    SchoolName = aoSqlDataReader["School_Name"].ToString(),
                    Address = aoSqlDataReader["Address"].ToString()
                };
            }
            return oSchoolEntity;
        }

		public List<StatisticalData> GetAllStatisticalData()
		{
			List<StatisticalData> lstStatisticalData = new List<StatisticalData>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStatisticalData"))
                {
                    if (oSqlDataReader != null && oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            lstStatisticalData.Add(new SchoolEntities.StatisticalData
                            {
                                QueryId = Convert.ToInt32(oSqlDataReader["QueryId"]),
                                QueryName = Convert.ToString(oSqlDataReader["QueryName"]),
                                TotalCount = Convert.ToInt32(oSqlDataReader["TotalCount"]),
                            });
                        }
                    }
                }
            }
			return lstStatisticalData;
		}

        /// <summary>
        /// THis method is used to get the Mobile user details of specific school.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public string GetMobileUserDetails(int aiSchoolId)
        {
            string sTotalCount = string.Empty;

            string connectionString = "Data Source= " + ConfigurationManager.AppSettings["SchoolLocationsDataSource"] + "; Database= RITeSchool" + "; User ID=" + ConfigurationManager.AppSettings["SchoolLocationsUserId"] + "; Password=" + ConfigurationManager.AppSettings["SchoolLocationsPassword"];

            using (SqlConnection oSqlConnection = new SqlConnection(connectionString))
            {
                string sCommand =  "SELECT COUNT(1)  AS TotalUserCount FROM ( SELECT UserId FROM Mobile.PushNotificationRegistrations WHERE SchoolId = " + aiSchoolId + "AND IsDeleted = 0 AND UserId <> 0 GROUP BY UserId) AA";

                SqlCommand oSqlCommand = new SqlCommand(sCommand, oSqlConnection);
                oSqlConnection.Open();

                using (SqlDataReader oSqlDataReader = oSqlCommand.ExecuteReader())
                {
                    if (oSqlDataReader.Read())
                    {
                        sTotalCount = oSqlDataReader["TotalUserCount"].ToString();
                    }
                }

                oSqlConnection.Close();
            }

            return sTotalCount;
        }

        /// <summary>
        /// This method is used to get the login details of nobile app as well as school website.
        /// </summary>
        /// <returns></returns>
        public DataTable GetLoginDetailsForFeatureUsage()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetLoginDetailsForFeatureUsage");
            }
        }

        /// <summary>
        /// This method is used to return photo details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static List<PhotoMaster> GetStudentsBinaryPhoto(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<PhotoMaster> lstPhotoMaster = new List<PhotoMaster>();
                PhotoMaster oPhotoMaster = null;
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_GetStudentBinaryImages]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        oPhotoMaster = new PhotoMaster
                        {
                            UserId = oSqlDataReader["UserId"].ToInt(),
                            TotalBytes = oSqlDataReader["TotalBytes"] as byte[]
                        };

                        lstPhotoMaster.Add(oPhotoMaster);
                    }
                }
                return lstPhotoMaster;
            }
        }

        public void ExecuteOtherActivities(int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ExecuteOtherActivities");
            }
        }

        #endregion

        #region  Private Methods

        private void LoadSchoolDetails(int aiSchoolId)
        {
            // This Function  take the SchoolId as parameter and populate the data from database.
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolDetails(aiSchoolId);
                using(SqlDataReader oSchoolDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oSchoolDR != null)
                    {
                        while (oSchoolDR.Read())
                        {
                            moSchoolInfo.SchoolId = Convert.ToInt32(oSchoolDR["school_id"]);
                            moSchoolInfo.SchoolName = Convert.ToString(oSchoolDR["school_name"]);
                            if (oSchoolDR["SMSSender_Name"] != DBNull.Value)
                                moSchoolInfo.SMSSenderName = Convert.ToString(oSchoolDR["SMSSender_Name"]);
                            if (oSchoolDR["SMSSender_MobileNo"] != DBNull.Value)
                                moSchoolInfo.SMSSenderMobileNo = Convert.ToString(oSchoolDR["SMSSender_MobileNo"]);
                            if (oSchoolDR["Subscription_Date"] != DBNull.Value)
                                moSchoolInfo.SubscriptionDate = Convert.ToDateTime(oSchoolDR["Subscription_Date"]);
                            if (oSchoolDR["school_orgn_name"] != DBNull.Value)
                                moSchoolInfo.SchoolOrgnName = Convert.ToString(oSchoolDR["school_orgn_name"]);

                            moSchoolInfo.SchoolSinceDate = Convert.ToDateTime(oSchoolDR["school_since_date"]);
                            moSchoolInfo.Address = Convert.ToString(oSchoolDR["Address"]);

                            moSchoolInfo.City = Convert.ToString(oSchoolDR["city"]);
                            moSchoolInfo.StateName = Convert.ToString(oSchoolDR["state_Name"]);
                            moSchoolInfo.Pincode = Convert.ToString(oSchoolDR["pincode"]);
                            moSchoolInfo.PhoneNo = Convert.ToString(oSchoolDR["phone_number"]);
                            if (oSchoolDR["phone_number2"] != DBNull.Value)
                                moSchoolInfo.PhoneNo2 = oSchoolDR["phone_number2"].ToString();
                            if (oSchoolDR["Registration_No"] != DBNull.Value)
                                moSchoolInfo.RegNo = Convert.ToString(oSchoolDR["Registration_No"]);

                            if (oSchoolDR["WebSite"] != DBNull.Value)
                                moSchoolInfo.sWebSite = Convert.ToString(oSchoolDR["WebSite"]);
                            else
                                moSchoolInfo.sWebSite = "";

                            if (oSchoolDR["FaxNumber"] != DBNull.Value)
                                moSchoolInfo.sFaxNumber = Convert.ToString(oSchoolDR["FaxNumber"]);
                            else
                                moSchoolInfo.sFaxNumber = "";

                            if (oSchoolDR["Address1"] != DBNull.Value)
                                moSchoolInfo.sAddress1 = Convert.ToString(oSchoolDR["Address1"]);
                            else
                                moSchoolInfo.sAddress1 = "";

                            if (oSchoolDR["Address2"] != DBNull.Value)
                                moSchoolInfo.sAddress2 = Convert.ToString(oSchoolDR["Address2"]);
                            else
                                moSchoolInfo.sAddress2 = "";

                            if (oSchoolDR["AccountNo"] != DBNull.Value)
                                moSchoolInfo.sAccountNo = Convert.ToString(oSchoolDR["AccountNo"]);
                            else
                                moSchoolInfo.sAccountNo = "";

                            if (oSchoolDR["Email"] != DBNull.Value)
                                moSchoolInfo.sEmail = Convert.ToString(oSchoolDR["Email"]);
                            else
                                moSchoolInfo.sEmail = "";

                            if (oSchoolDR["FeedbackEmail"] != DBNull.Value)
                                moSchoolInfo.sFeedbackEmail = Convert.ToString(oSchoolDR["FeedbackEmail"]);
                            else
                                moSchoolInfo.sFeedbackEmail = "";

                            if (oSchoolDR["CareerEmails"] != DBNull.Value)
                                moSchoolInfo.sCareerEmails = Convert.ToString(oSchoolDR["CareerEmails"]);
                            else
                                moSchoolInfo.sCareerEmails = "";

                            if (oSchoolDR["LogoPath"] != DBNull.Value)
                                moSchoolInfo.sLogoPath = Convert.ToString(oSchoolDR["LogoPath"]);
                            else
                                moSchoolInfo.sLogoPath = "";

                            if (oSchoolDR["SignPath"] != DBNull.Value)
                                moSchoolInfo.sSignLogo = Convert.ToString(oSchoolDR["SignPath"]);
                            else
                                moSchoolInfo.sSignLogo = "";

                            if (oSchoolDR["ICardLogo"] != DBNull.Value)
                                moSchoolInfo.sICardLogo = Convert.ToString(oSchoolDR["ICardLogo"]);
                            else
                                moSchoolInfo.sICardLogo = "";

                            if (oSchoolDR["PTRegCertificateNo"] != DBNull.Value)
                                moSchoolInfo.msPTRegCertificateNo = Convert.ToString(oSchoolDR["PTRegCertificateNo"]);
                            else
                                moSchoolInfo.msPTRegCertificateNo = "";

                            if (oSchoolDR["SchoolRecNoPrimary"] != DBNull.Value)
                                moSchoolInfo.SchoolRecNoPrimary = Convert.ToString(oSchoolDR["SchoolRecNoPrimary"]);

                            if (oSchoolDR["SchoolRecNoSecondary"] != DBNull.Value)
                                moSchoolInfo.SchoolRecNoSecondary = Convert.ToString(oSchoolDR["SchoolRecNoSecondary"]);

                            if (oSchoolDR["IndexNo"] != DBNull.Value)
                                moSchoolInfo.IndexNo = Convert.ToString(oSchoolDR["IndexNo"]);
                            if (oSchoolDR["AdminId"] != DBNull.Value)
                                moSchoolInfo.AdminId = Convert.ToInt32(oSchoolDR["AdminId"]);
                            if (oSchoolDR["AdminRoleId"] != DBNull.Value)
                                moSchoolInfo.AdminRoleId = Convert.ToInt32(oSchoolDR["AdminRoleId"]);

                            if (oSchoolDR["PanNo"] != DBNull.Value)
                                moSchoolInfo.PanNo = oSchoolDR["PanNo"].ToString();

                            if (oSchoolDR["TanNo"] != DBNull.Value)
                                moSchoolInfo.TanNo = oSchoolDR["TanNo"].ToString();

                            if (oSchoolDR["GSTIN"] != DBNull.Value)
                                moSchoolInfo.GSTIN = oSchoolDR["GSTIN"].ToString();

                            if (oSchoolDR["UDISENumber"] != DBNull.Value)
                                moSchoolInfo.UDISENumber = oSchoolDR["UDISENumber"].ToString();

                            if (oSchoolDR["Lattitude"] != DBNull.Value)
                                moSchoolInfo.Lattitude = oSchoolDR["Lattitude"].ToString();

                            if (oSchoolDR["Longitude"] != DBNull.Value)
                                moSchoolInfo.Longitude = oSchoolDR["Longitude"].ToString();

                            if (oSchoolDR["ForgotPasswordEmails"] != DBNull.Value)
                                moSchoolInfo.sForgotPasswordEmail = Convert.ToString(oSchoolDR["ForgotPasswordEmails"]);
                            else
                                moSchoolInfo.sForgotPasswordEmail = "";
                        }
                    }
                }
            }
        }

        private string FetchSchoolDetails(int aiSchoolId)
        {
            // This function is used to fetch the data from the database for the specified school id. 
            // It returns the datareader containging the details.
            string sSelectStatement;

            sSelectStatement = " SELECT " +
                                " school_id " +
                                " , school_orgn_name " +
                                " , school_since_date " +
                                " , school_name " +
                                " , SMSSender_Name " +
                                " , SMSSender_MobileNo " +
                                " , Subscription_Date " +
                                " , Address as address " +
                                " , city " +
                                " , pincode " +
                                " , state_Name " +
                                " , phone_number " +
                                " , phone_number2 " +
                                " , SchoolRecNoPrimary " +
                                " , SchoolRecNoSecondary " +
                                " , IndexNo " +
                                " , PanNo " +
                                " , TanNo " +
                                " , GSTIN " +
                                " , UDISENumber " +
                                 " , Lattitude " +
                                  " , Longitude " +
                                " , Registration_No " +
                                " , FaxNumber " +
                                " , WebSite " +
                                " , Address1 " +
                                " , Address2 " +
                                " , AccountNo " +
                                " , Email " +
                                " , FeedbackEmail " +
                                " , CareerEmails " +
                                " , ForgotPasswordEmails " +
                                " , LogoPath " +
                                " , ICardLogo " +
                                " , SignPath " +
                                " , PTRegCertificateNo " +
                                ",(SELECT  " +
                                          " User_Id " +
                                      "FROM User_Master " +
                                     "WHERE User_Role_Id=1  " +
                                     " AND School_Id=" + aiSchoolId +
                                      " AND Is_Deleted='N'" +
                                       "AND User_First_Name NOT LIKE'%Software Coordinator%'  ) as AdminId " +
                                 " ,(SELECT  " +
                                           "User_Role_Id " +
                                      "FROM User_Master " +
                                     "WHERE User_Role_Id=1  " +
                                     " AND School_Id=" + aiSchoolId +
                                      " AND Is_Deleted='N'" +
                                       "AND User_First_Name NOT LIKE'%Software Coordinator%'  ) as AdminRoleId	" +
                           " FROM " +
                                " school_master INNER JOIN PTRegistrationCertificateNoMaster ON school_master.School_Id=PTRegistrationCertificateNoMaster.SchoolId" +
                           " WHERE " +
                                " School_Master.school_id = " + aiSchoolId +
                                " AND School_Master.is_deleted = N'" + Constants.C_NO + "'";
            return sSelectStatement;
        }

        public static DataTable GetSchoolDetailsForSchoolShortName(string asShortName)
        {
            string sSelectStatement = " SELECT " +
                                            " School_Master.School_Name, " +
                                            " School_Master.School_Id, " +
                                            " School_Master.School_Short_Name, " +
                                            " ConfigureMenu.ConfigureMenuId, " +
                                            " ConfigureMenu.ConfigureMenuName " +
                                        " FROM " +
                                            " School_Master INNER JOIN " +
                                            " ConfigureMenu ON School_Master.School_Id = ConfigureMenu.SchoolId " +
                                        " WHERE  " +
                                            " ConfigureMenu.Is_Default = N'" + Constants.C_YES + "'" +
                                            " AND School_Master.Is_Active = N'" + Constants.C_YES + "' " +
                                            " AND School_Short_Name = N'" + StringUtility.ReplaceSingleQuoteInString(asShortName, false) + "'" +
                                            " AND is_deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

        }

        public void UpdateSchoolLogo(Byte[] ImageBinaryData)
        {
            string sSQL = "UPDATE School_Master SET Logo = @Image" +
                          ", LogoPath = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolInfo.sLogoPath, true) + "'" +
                          " WHERE School_Id = " + moSchoolInfo.SchoolId.ToString();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(ImageBinaryData, sSQL);
        }

        public void UpdateICardLogo(Byte[] ImageBinaryData)
        {
            string sSQL = "UPDATE School_Master SET ICardLogoImage = @Image" +
                      " WHERE School_Id = " + moSchoolInfo.SchoolId.ToString();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(ImageBinaryData, sSQL);
        }

        public void UpdatePrincipalSignatureLogo(Byte[] ImageBinaryData)
        {
            string sSQL = "UPDATE School_Master SET SignPathImage = @Image" +
                      " WHERE School_Id = " + moSchoolInfo.SchoolId.ToString();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(ImageBinaryData, sSQL);
        }

        public void GetSignPath(int aiSchoolId)
        {
            string sSelectStmt = "select Address,SignPath,ICardLogo,LogoPath,Logo,School_Name from School_Master where School_Id=" + aiSchoolId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStmt))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            if (oSqlDataReader["Address"] != DBNull.Value)
                                moSchoolInfo.Address = oSqlDataReader["Address"].ToString();
                            if (oSqlDataReader["SignPath"] != DBNull.Value)
                                moSchoolInfo.sSignLogo = Convert.ToString(oSqlDataReader["SignPath"]);
                            else
                                moSchoolInfo.sLogoPath = "";
                            if (oSqlDataReader["ICardLogo"] != DBNull.Value)
                                moSchoolInfo.sICardLogo = Convert.ToString(oSqlDataReader["ICardLogo"]);
                            else
                                moSchoolInfo.sICardLogo = "";
                            if (oSqlDataReader["LogoPath"] != DBNull.Value)
                                moSchoolInfo.sLogoPath = Convert.ToString(oSqlDataReader["LogoPath"]);
                            else
                                moSchoolInfo.sLogoPath = "";
                            if (oSqlDataReader["School_Name"] != DBNull.Value)
                                moSchoolInfo.SchoolName = oSqlDataReader["School_Name"].ToString();
                            else
                                moSchoolInfo.SchoolName = "";
                        }
                    }
                }

            };
        }

        /// <summary>
        /// This method is used to send Notification Mail about DB Log file size.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static List<DBLogFileSizeDetails> GetDBLogSizeDetails()
        {
            DBLogFileSizeDetails oDBLogFileSizeDetails;
            List<DBLogFileSizeDetails> lstDBLogSizeDetails = new List<DBLogFileSizeDetails>();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDBLogSize"))
                {
                    if (oSqlReader != null && oSqlReader.HasRows)
                    {
                        while (oSqlReader.Read())
                        {
                            oDBLogFileSizeDetails = new DBLogFileSizeDetails()
                            {
                                DBName = oSqlReader["DBName"].ToString(),
                                DbSizeGb = oSqlReader["DbSizeGb"].ToString(),
                                DbMdfSizeGb = oSqlReader["DbSizeGbmdf"].ToString()
                            };

                            lstDBLogSizeDetails.Add(oDBLogFileSizeDetails);
                        }

                    }
                }
            }
            return lstDBLogSizeDetails;
        }

        /// <summary>
        /// This method is used to Add/Update Statistical data of all database and insert this data into RITeSchool Database.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static void AddStatisticalData(int aiSchoolId)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_AddStatisticalData");
            }
        }


        /// <summary>
        /// This method is used to get feedback details .
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static void UpdateRiteSchoolUsageDetails(int aiSchoolId)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_AddRitUsageData");
        }

        /// <summary>
        /// This method is used to get feedback details .
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static void UpdateFeedbackDetails(int aiSchoolId)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_GetUserFeedBackDetails");
            }
        }

        public List<SchoolFolder> GetAllSchoolFolders()
        {
            List<SchoolFolder> lstSchoolFolders = new List<SchoolFolder>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sStatement = "SELECT SchoolId, FolderName FROM [Mobile].[Schools]";
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sStatement))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstSchoolFolders.Add(new SchoolFolder { SchoolId = oSqlDataReader["SchoolId"].ToInt(), FolderName = oSqlDataReader["Foldername"].ToString() });
                    }
                }
            }
            return lstSchoolFolders;
        }

        #endregion

        #region Activate Or Deactivate flag

        /// <summary>
        /// This method is used to update flag for school activation
        /// </summary>
        /// <returns></returns>
        public void UpdateSchoolActivationFlag(int aiSchoolId)
        {
            string sUpdateStatement;

            sUpdateStatement = "UPDATE School_Master SET " +
                                    " Is_Active = N'" + Constants.C_YES + "'" +
                                    " , Subscription_Date =N'" + System.DateTime.Now.AddYears(1).ToString() + "'" +
                          " WHERE " +
                                 " school_id = " + aiSchoolId +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }


        /// <summary>
        /// This method is used to update allowed SMS Count.
        /// </summary>
        /// <returns></returns>
        public void UpdateSchoolSMSCount(int iAcademicYrId)
        {
            string sUpdateStatement;

            sUpdateStatement = "UPDATE SchoolWise_Academic_Year_Master SET " +
                                    " AllowedSMS_Count = N'" + moSchoolInfo.AllowedSMSCount + "'" +
                          " WHERE " +
                                   " school_id = " + moSchoolInfo.SchoolId +
                                 " AND (Academic_Year_ID = " + iAcademicYrId + ")" +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to update allowed SMS Count.
        /// </summary>
        /// <returns></returns>
        public void UpdateSchoolSentSMSCount(int aiSmsSentCount, int iAcademicYrId)
        {
            string sUpdateStatement;

            sUpdateStatement = "UPDATE SchoolWise_Academic_Year_Master SET " +
                                    " SentSMS_Count = SentSMS_Count + " + aiSmsSentCount +
                          " WHERE " +
                                 " school_id = " + moSchoolInfo.SchoolId +
                                 " AND (Academic_Year_ID = " + iAcademicYrId + ")" +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }


        /// <summary>
        /// This method is used to deactivate school.
        /// </summary>
        /// <returns></returns>

        public void UpdateSchoolDeActivationFlag(int aiSchoolId)
        {
            string sUpdateStatement;

            sUpdateStatement = "UPDATE School_Master SET " +
                                    " Is_Active = N'" + Constants.C_NO + "'" +
                          " WHERE " +
                                 " school_id = " + aiSchoolId +
                                 " AND Is_Active = N'" + Constants.C_YES + "'" +
                                 " AND is_deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }



        /// <summary>
        ///		Returns the total staff count in the school.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public int GetStaffCount(int aiAcademicYearId)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", moSchoolInfo.SchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AttendanceDate", DateTime.Now, SqlDbType.DateTime);

                using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStaffDetails"))
                {
                    if (oReader.Read() && oReader["UserCount"] != DBNull.Value)
                        return oReader["UserCount"].ToInt();
                }
            }

            return 0;
        }

        /// <summary>
        /// THis method is used to get Menu details.
        /// </summary>
        /// <param name="PageName"></param>
        /// <returns></returns>
        public List<CounsellorMenu> GetMenuDetails(string asPageName,int aiSchoolId)
        {
            List<CounsellorMenu> lstCounsellorMenu = new List<CounsellorMenu>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("PageName", asPageName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetConfigMenuDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        CounsellorMenu oCounsellorMenu = new CounsellorMenu
                        {
                           ConfigureMenuId = Convert.ToInt32(oSqlDataReader["ConfigureMenuId"]),
                           ConfigureMenuName = oSqlDataReader["ConfigureMenuName"].ToString()
                        };
                        lstCounsellorMenu.Add(oCounsellorMenu);
                    }             
                }
            }
            return lstCounsellorMenu;
        }

         //<summary>
         //THis method is used to get NewsLetter Menu details.
         //</summary>
         //<param name="PageName"></param>
         //<returns></returns>
        public List<NewsLetterDetails> GetNewsLetterDetails(int aiParentMenuId, int aiSchoolId, int aiDisplaymonth)
        {
            List<NewsLetterDetails> lstNewsLetter = new List<NewsLetterDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("parent_menu_id", aiParentMenuId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Displaymonth", aiDisplaymonth, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetConfigureMenuDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        NewsLetterDetails oNewLetter = new NewsLetterDetails
                        {
                            ConfigureMenuId = Convert.ToInt32(oSqlDataReader["ConfigureMenuId"]),
                            ConfigureMenuName = oSqlDataReader["ConfigureMenuName"].ToString(),
                            FilePath = oSqlDataReader["FilePath"].ToString()
                        };
                        lstNewsLetter.Add(oNewLetter);
                    }
                }
            }
            return lstNewsLetter;
        }

        /// <summary>
        /// This method is used to save school setting.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiId"></param>
        /// <param name="asValue"></param>
        /// <param name="asName"></param>
        public static void SaveSchoolSetting(int aiSchoolId, int aiAcademicYearId, int aiId, string asValue, string asName)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Value", asValue, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Name", asName, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSchoolsetting");
            }
        }
        public static void UpdateModuleDetails(string asModuleId)
        { 
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
             {
                // oSQLServerDbUtility.AddParameter("Name", asSchoolModulesName, SqlDbType.NVarChar);
                 oSQLServerDbUtility.AddParameter("SchoolModulesId", asModuleId, SqlDbType.NVarChar);
                 oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_SaveModule");
             }
            
        }

        public List<MonthMaster> GetAllMonths()
        {
            List<MonthMaster> lstMonthMaster = new List<MonthMaster>();
            string sStatement = "SELECT MonthId,Month FROM MonthsOfYear";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sStatement))
                {
                    while (oSqlDataReader.Read())
                    {
                        MonthMaster oMonthMaster = new MonthMaster();
                        oMonthMaster.MonthId = oSqlDataReader["MonthId"].ToInt();
                        oMonthMaster.Month = oSqlDataReader["Month"].ToString();
                        lstMonthMaster.Add(oMonthMaster);
                    }
                }
            }

            return lstMonthMaster;
        }

        #endregion
    }
}

