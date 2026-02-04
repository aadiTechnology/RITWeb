using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Utility;
using SchoolEntities;
using MasterEntities;
using System.Data.SqlClient;
using XseedReportEntities;
using SchoolEntities.Admin;

namespace DataCommunicator
{

    public class StandardMasterDC
    {

        #region Constant and structures

        #region structure

        public struct StandardMasterStruct
        {
            public int miStandardId;
            public string msStandardName;
            public int miOriginalStandardId;
            public int miSchoolId;
            public int miAcademicYearId;
            public string msInsertedByid;
            public string msUpdatedById;
            public string msIsPrePrimary;
            public int miSectionId;
            public int miStudentStrength;
            public int miThreshold;
            public int miNextOriginalStandardId;
            public System.DateTime mdtInsertedDate;
            public System.DateTime mdtUpdatedDate;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private StandardMasterStruct moStandardMasterStruct;

        #endregion
        #region Properties

        public StandardMasterStruct StandardMasterStructDetails
        {

            get { return moStandardMasterStruct; }
            set { moStandardMasterStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public StandardMasterDC()
        {
        }
        #endregion

        #region Public Methods

        public string GetInsertStatementforStandard()
        {

            string sInsertStatement = "INSERT INTO Standard_Master ( " +
                " standard_name" +
                " , original_standard_id" +
                " , school_id" +
                " , academic_Year_Id" +
                " , inserted_by_id" +
                " , updated_by_id" +
                " , is_preprimary" +
                " , section" +
                ", StudentStrength" +
                ", Threshold" +
                ", NextOriginalStandardId" +
            ") VALUES (" +
                 " '" + StringUtility.ReplaceSingleQuoteInString(moStandardMasterStruct.msStandardName, false) + "' " +
                 " , " + moStandardMasterStruct.miOriginalStandardId +
                 " , " + moStandardMasterStruct.miSchoolId +
                 " , N'" + moStandardMasterStruct.miAcademicYearId + "'" +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moStandardMasterStruct.msInsertedByid, false) + "' " +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moStandardMasterStruct.msUpdatedById, false) + "' " +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moStandardMasterStruct.msIsPrePrimary, false) + "' " +
                 " , " + moStandardMasterStruct.miSectionId +
                 " , " + moStandardMasterStruct.miStudentStrength +
                 " , " + moStandardMasterStruct.miThreshold +
                 " , " + moStandardMasterStruct.miNextOriginalStandardId +
                 " ) ";
            return sInsertStatement;
        }

        public string GetInsertStmtforStdCautionMoney(int iDefaultCautionMoney)
        {
            string sInsertStatement = "INSERT INTO Standard_Caution_Money_Details ( " +
                " School_Id" +
                " , Academic_Year_Id" +
                " , Standard_Id" +
                " , Amount" +
                " , inserted_by_id" +
            ") VALUES (" +
                 " " + moStandardMasterStruct.miSchoolId +
                 " , " + moStandardMasterStruct.miAcademicYearId +
                 " , N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                 " , " + iDefaultCautionMoney +
                 " , " + moStandardMasterStruct.msInsertedByid +
                 " ) ";
            return sInsertStatement;
        }

        public string GetUpdateStatementforStandard()
        {

            string sUpdateStatement = " UPDATE Standard_Master SET " +
                " standard_name =  N'" + StringUtility.ReplaceSingleQuoteInString(moStandardMasterStruct.msStandardName, false) + "' " +
                " , original_standard_id =  " + moStandardMasterStruct.miOriginalStandardId +
                " , school_id =  " + moStandardMasterStruct.miSchoolId +
                " , StudentStrength =  " + moStandardMasterStruct.miStudentStrength +
                " , Threshold =  " + moStandardMasterStruct.miThreshold +
                " , updated_by_id =  N'" + StringUtility.ReplaceSingleQuoteInString(moStandardMasterStruct.msUpdatedById, false) + "' " +
                " , update_date =  N'" + moStandardMasterStruct.mdtUpdatedDate.ToString("MM/dd/yyyy") + "' " +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                " AND standard_id =  " + moStandardMasterStruct.miStandardId;

            return sUpdateStatement;
        }

        public string GetDeleteStatementforStandard()
        {
            string sDeleteStatement =
                                    " DELETE Standardwise_Academic_Year " +
                                    " WHERE " +
                                    " StandardId =  " + moStandardMasterStruct.miStandardId + ";";

            sDeleteStatement += " DELETE Standard_Caution_Money_Details " +
                               " WHERE " +
                               " Standard_Id =  " + moStandardMasterStruct.miStandardId + ";";

            sDeleteStatement += " DELETE Standard_Master " +
                                     " WHERE " +
                                        " is_deleted = N'" + Constants.C_NO + "'" +
                                        " AND standard_id =  " + moStandardMasterStruct.miStandardId;

            return sDeleteStatement;

        }

        /// <summary>
        /// This method is used to get sandards which are associated to assessments.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYeaId"></param>
        /// <param name="aiTeacherId"></param>
        /// <returns></returns>
        public static List<StandardMaster> GetStandardsAssociatedToAssessments(int aiSchoolId, int aiAcademicYeaId, int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<StandardMaster> lstStandards = new List<StandardMaster>();
                StandardMaster oStandardMaster = null;

                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYEarId", aiAcademicYeaId, SqlDbType.Int);
                if (aiTeacherId != 0)
                    oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetStandardsForSubjectSections]"))
                {
                    if (oSqlDataReader != null)
                    {
                        while (oSqlDataReader.Read())
                        {
                            oStandardMaster = new StandardMaster
                            {
                                StandardId = Convert.ToInt32(oSqlDataReader["standard_id"]),
                                StandardName = Convert.ToString(oSqlDataReader["standard_name"]),
                            };
                            lstStandards.Add(oStandardMaster);
                        }
                    }
                }
                return lstStandards;
            }
        }


        public static int GetProgressReportForStandard(int aiSchoolId, int aiAcademicYeaId, int aiClassTeacherId)
        {
            int iReportId = 0;
            string sSelect = " SELECT StandardwiseProgressReportMaster.Report_Id " +
                             " FROM vw_ClassTeacher INNER JOIN " +
                             " StandardwiseProgressReportMaster ON" +
                             " StandardwiseProgressReportMaster.Standard_Id = vw_ClassTeacher.Standard_Id" +
                             " AND vw_ClassTeacher.SchoolWise_Standard_Division_Id = " + aiClassTeacherId +
                             " WHERE StandardwiseProgressReportMaster.academic_year_id = " + aiAcademicYeaId +
                             " AND StandardwiseProgressReportMaster.School_Id = " + aiSchoolId +
                             " AND StandardwiseProgressReportMaster.Is_Deleted = 'N'" +
                             " AND vw_ClassTeacher.academic_year_id = " + aiAcademicYeaId +
                             " AND vw_ClassTeacher.School_Id = " + aiSchoolId +
                             " AND vw_ClassTeacher.Is_Deleted = 'N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iReportId = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelect);
            return iReportId;
        }

        /// <summary>
        /// This is used to set the standard name on page load
        /// </summary>
        /// <param name="sStdID"></param>
        /// <param name="sSchoolID"></param>
        /// <param name="sAcadID"></param>
        /// <returns></returns>
        public static DataTable GetStandardDetails(string asStdID, string asSchoolID, string asAcademicYearId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", asSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", asAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", asStdID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStandardAcademicDetails");
            }
        }

        /// <summary>
        ///		Gets the standards for which marks are given, but only grades are displayed on the report.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static List<StandardMaster> GetStandardsWithOnlyGradeSetting(int aiSchoolId, int aiAcademicYearId)
        {
            var lstStandards = new List<StandardMaster>();

            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardsWithOnlyGradeSettings"))
                    if (oReader.HasRows)
                        lstStandards = new GenericClass<StandardMaster>().GetFilledObjectList(oReader);
            }

            return lstStandards;
        }

        #endregion


        public static bool IsGradingStandard(int aiSchoolId, int aiAcademicYearId, int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsGradingStandard", 0, SqlDbType.Bit,ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsGradingStandard");
                return oSqlParameter.Value.ToBool();
            }
        }
    }

    public class StandardCollectionDC
    {
        private int miSchoolId = 0;
        private int miAcademicYearId = 0;

        public StandardCollectionDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
        }
        public StandardCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        public DataTable GetAllStandards()
        {
            // This method returns dataset populated with master standards from databse.
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllStandards");
            }
        }

        /// <summary>
        /// This method is used to get all Fee Types.
        /// </summary>
        public DataTable GetAllFeeTypes()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetReceiptHeaderForReport");
            }
        }

        /// <summary>
        /// This method is used to get all Fee Types for challan import.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiStandardDivisionId"></param>
        public DataTable GetAllFeeTypesForChallanImport(int aiAcademicYearId, int aiStandardId, int aiStandardDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolWise_Standard_Division_Id", aiStandardDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllFeeTypesForChallan");
            }
        }

        /// <summary>
        /// This method is used to get all payable for For Challan import.
        /// </summary>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiOriginalFeeTypeId"></param>
        public DataTable GetAllPayableforChallan(int aiAcademicYearId, int aiStandardId, int aiOriginalFeeTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Original_Fee_Type_Id", aiOriginalFeeTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetFeeTypeWisePayableForChallan");
            }
        }

        public DataTable GetAllStandardsForFee(int aiStandardID)
        {
            // This method returns dataset populated with master standards from databse.
            string sSelectStatement;
            sSelectStatement = " SELECT     Standard_Id, Standard_Name " +
                               " FROM         Standard_Master " +
                               " WHERE     (Is_Deleted = 'N') AND (School_Id = " + miSchoolId + ") AND (academic_Year_Id = " + miAcademicYearId +
                               " ) AND Standard_Id <> " + aiStandardID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetAssociatedStandards()
        {

            string sSelectStatement = " SELECT  " +
                                 " school_id " +
                                 " , original_standard_id " +
                                 " , standard_id " +
                                 " , standard_name " +
                             " FROM " +
                                  " standard_master " +
                             " WHERE " +
                                  " is_deleted = N'" + Constants.C_NO + "'" +
                                  " AND school_id = " + miSchoolId +
                                  " AND academic_year_id = " + miAcademicYearId +
                              " ORDER BY " +
                                   " original_standard_id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetAssociatedStandardsForEnquiry(int aiAcmissionForId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcmissionForId", aiAcmissionForId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStandardsForEnquiry");
            }   
        }

        public DataTable GetAdmissionForCategories()
        {
            string sSelectStatement = " SELECT  " +
                                 " Id " +
                                 " , AdmissionFor " +                                 
                             " FROM " +
                                  " StudentAdmissionForDetails " +
                             " WHERE " +
                                  "  IsDeleted = 0 ORDER BY " +
                                   " SortOrder";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetAssociatedStandardsForHealth()
        {
            string sSelectStatement = " SELECT  " +
                                 " school_id " +
                                 " , original_standard_id " +
                                 " , Value_Member AS standard_id " +
                                 " , Display_Member AS standard_name " +
                             " FROM " +
                                  " vw_StandardForHealthReport " +
                             " WHERE " +                                                            
                                  "  academic_year_id = " + miAcademicYearId +
                              " ORDER BY " +
                                   " original_standard_id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetAssociatedStandardsForSiblingDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAssociatedStandardsForSiblingDetails");
            }
        }

        /// <summary>
        /// This method is used to get all standard division details.
        /// </summary>
        public DataSet GetAllStandardDivisionDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllStandardDivisionDetails");
            }
        }

        public List<StandardDivisionMaster> GetAllClasses()
        {
            List<StandardDivisionMaster> lstStandardDivisionMaster = new List<StandardDivisionMaster>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStandardDivisions"))
                {                   
                   while (oSqlDataReader.Read())
                   {
                       StandardDivisionMaster oStandardDivisionMaster = new StandardDivisionMaster
                                               {
                                                   StandardDivisionId = oSqlDataReader["SchoolWise_Standard_Division_Id"].ToInt(),
                                                   StandardId = oSqlDataReader["Standard_Id"].ToInt(),
                                                   StandardName = oSqlDataReader["Standard_Name"].ToString(),
                                                   DivisionId = oSqlDataReader["Division_Id"].ToInt(),
                                                   DivisionName = oSqlDataReader["Division_Name"].ToString()
                                               };
                       lstStandardDivisionMaster.Add(oStandardDivisionMaster);
                   }
                return lstStandardDivisionMaster;
                }                
            }            
        }

        public DataTable GetAssociatedStandardsForHouse()
        {
            string sSelectStatement = " SELECT  " +
                                 " school_id " +
                                 " , original_standard_id " +
                                 " , standard_id " +
                                 " , standard_name " +
                             " FROM " +
                                  " standard_master " +
                             " WHERE " +
                                  " is_deleted = N'" + Constants.C_NO + "'" +
                                  "AND AllowHouseConfiguration= N'" + Constants.I_ONE + "'" +
                                  " AND school_id = " + miSchoolId +
                                  " AND academic_year_id = " + miAcademicYearId +
                              " ORDER BY " +
                                   " original_standard_id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetPrePrimaryStandards()
        {
            string sSelectStatement = " SELECT  " +
                                 " school_id " +
                                 " , original_standard_id " +
                                 " , standard_id " +
                                 " , standard_name " +
                             " FROM " +
                                  " standard_master " +
                             " WHERE " +
                                  " is_deleted = N'" + Constants.C_NO + "'" +
                                  " AND is_preprimary = N'" + Constants.C_YES + "'" +
                                  " AND school_id = " + miSchoolId +
                                  " AND academic_year_id = " + miAcademicYearId +
                              " ORDER BY " +
                                   " original_standard_id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetConfiguredPrePrimaryStandards()
        {
            string sSelectStatement = " SELECT  " +
                                 "   school_id " +
                                 " , original_standard_id " +
                                 " , standard_id " +
                                 " , standard_name " +
                             " FROM " +
                                  " standard_master " +
                             " WHERE " +
                                  " is_deleted = N'" + Constants.C_NO + "'" +
                                  " AND is_preprimary = N'" + Constants.C_YES + "'" +
                                  " AND school_id = " + miSchoolId +
                                  " AND academic_year_id = " + miAcademicYearId +
                                  " AND standard_id IN (SELECT standard_id " +
                                                        "FROM PrePrimaryStandardForNewProgressReport " +
                                                        " WHERE school_id = " + miSchoolId +
                                                        " AND academic_year_id = " + miAcademicYearId +
                                                        " AND Is_Deleted=0)" +
                              " ORDER BY " +
                                   " original_standard_id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to standard details for which exam configuration is not done.
        /// </summary>
        /// <param name="abIsXseed"></param>
        /// <returns></returns>
        public List<StandardMaster> GetStandardsForExamConfiguration()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader =
                    oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetExamConfiguredStandards"))
                {
                    List<StandardMaster> lstStandards = new List<StandardMaster>();

                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            lstStandards.Add(new StandardMaster()
                            {
                                StandardId = oSqlDataReader["StandardId"].ToInt(),
                                StandardName = oSqlDataReader["StandardName"].ToString()

                            });
                        }
                    }


                    return lstStandards;
                }
            }
        }

        /// <summary>
        /// This method is used to standard details for which exam configuration is not done.
        /// </summary>
        /// <param name="abIsXseed"></param>
        /// <returns></returns>
        public List<StandardMaster> GetStandardsForExamConfiguration(bool abIsXseed)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsXseed", abIsXseed, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader =
                    oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardsForExamConfiguration"))
                {
                    List<StandardMaster> lstStandards = new List<StandardMaster>();

                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            lstStandards.Add(new StandardMaster()
                                                                    {
                                                                        StandardId = oSqlDataReader["StandardId"].ToInt(),
                                                                        StandardName = oSqlDataReader["StandardName"].ToString()

                                                                    });
                        }
                    }

                    return lstStandards;
                }
            }
        }

        /// <summary>
        /// This method is used to standard details for which exam configuration is not done.
        /// </summary>
        /// <param name="abIsXseed"></param>
        /// <returns></returns>
        public List<StandardMaster> GetStandardsForStandardwiseAssessment()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardsForStandardwiseAssessment"))
                {
                    List<StandardMaster> lstStandards = new List<StandardMaster>();
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            lstStandards.Add(new StandardMaster()
                            {
                                StandardId = oSqlDataReader["StandardId"].ToInt(),
                                StandardName = oSqlDataReader["StandardName"].ToString()

                            });
                        }
                    }

                    return lstStandards;
                }
            }
        }


        /// <summary>
        /// This method is used to save standards for grading system.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public void SaveStandardsForGradingSystem(string asStandardIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StandardIds", asStandardIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolID", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStandardsForGradingSystem");
            }
        }

        /// <summary>
        /// This method is used to return all standards which are available or not available for grading system .
        /// </summary>
        /// <param name="abIsXseed"></param>
        /// <returns></returns>
        public List<StandardMaster> GetStandardsForGradingSystem()
        {
            List<StandardMaster> lstStandards = new List<StandardMaster>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStandardsForGradingSystem"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstStandards.Add(new StandardMaster()
                        {
                            StandardId = oSqlDataReader["Standard_Id"].ToInt(),
                            StandardName = oSqlDataReader["Standard_Name"].ToString(),
                            IsForGrading = Convert.ToBoolean(oSqlDataReader["IsForGradingSystem"])
                        });
                    }
                }
            }
            return lstStandards;
        }

        public static List<StandardMaster> GetAll(int aiSchoolId, int aiAcademicYearId)
        {
            List<StandardMaster> lstStandard = new List<StandardMaster>();
            StandardMaster oStandardMaster = null;
            string sSelectstmt = "SELECT Standard_Name +'-'+Division_Name as Standard_Name " +
                                 ",SchoolWise_Standard_Division_Id as Standard_Id " +
                                 " from vw_standard_division " +
                                 " where School_Id=" + aiSchoolId + " " +
                                 " and academic_year_id=" + aiAcademicYearId + " " +
                                 " and Standard_Id  NOT IN( " +
                                 " select Standard_Id from Standard_Master " +
                                 " where School_Id = " + aiSchoolId + " " +
                                 " and academic_year_id=" + aiAcademicYearId + " " +
                                 " and Is_PrePrimary='Y' )" +
                                 " order by Original_Standard_Id,Original_Division_Id ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectstmt))
                {
                    if (oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            oStandardMaster = new StandardMaster
                            {
                                StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"]),
                                StandardName = oSqlDataReader["Standard_Name"].ToString()
                            };
                            lstStandard.Add(oStandardMaster);
                        }
                    }
                }
            }
            return lstStandard;
        }

        public DataTable GetAnualToppersStandards()
        {
            string sSelectStatement = "SELECT DISTINCT " +
                                           "  Standard_Master.Standard_Name" +
                                           " , Standard_Master.Standard_Id" +
                                           " , Standard_Master.Original_Standard_Id" +
                                     " FROM         SchoolWise_Standard_Division_Master INNER JOIN" +
                                           " Standard_Master ON SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id AND " +
                                           " SchoolWise_Standard_Division_Master.School_Id = Standard_Master.School_Id INNER JOIN" +
                                           " YearWise_Student_Details ON SchoolWise_Standard_Division_Master.Standard_Id = YearWise_Student_Details.Standard_Id AND " +
                                           " SchoolWise_Standard_Division_Master.Division_Id = YearWise_Student_Details.Division_id AND " +
                                           " SchoolWise_Standard_Division_Master.academic_year_id = YearWise_Student_Details.Academic_Year_ID AND  " +
                                           " SchoolWise_Standard_Division_Master.School_Id = YearWise_Student_Details.School_Id INNER JOIN " +
                                           " SchoolWise_StudentResult ON YearWise_Student_Details.YearWise_Student_Id = SchoolWise_StudentResult.Student_Id " +
                                     " WHERE     (SchoolWise_Standard_Division_Master.School_Id = " + miSchoolId + ") " +
                                             " AND (SchoolWise_Standard_Division_Master.academic_year_id = " + miAcademicYearId + ") AND " +
                                             " (SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" + ") " +
                                             " AND (Standard_Master.Is_Deleted = N'" + Constants.C_NO + "'" + ") " +
                                             " AND (dbo.Udf_isAllResultsGeneratedForStdDiv(SchoolWise_Standard_Division_Master.School_Id," +
                                                  " SchoolWise_Standard_Division_Master.academic_year_id, SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id) = 1) " +
                                     " GROUP BY SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id, " +
                                             " Standard_Master.Standard_Name, " +
                                             " Standard_Master.Standard_Id, " +
                                             " Standard_Master.Original_Standard_Id " +
                                     " ORDER BY Standard_Master.Original_Standard_Id";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        public void UpdateStandards(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        public void UpdateStandardDivisions(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }


        public void UpdateStandardSubjects(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to execute transaction of standard test association.
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>
        public void UpdateStandardTests(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to execute transaction of standard feetype association.
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>
        public void UpdateStandardFeeTypes(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        public void UpdateLectureCount(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to return classes wise student count details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public List<StudentStrengthDetails> GetClasseswiseStudentCountDetails(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetClasswiseStudentCounts"))
                {
                    List<StudentStrengthDetails> lstClasses = new List<StudentStrengthDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstClasses.Add(new StudentStrengthDetails
                        {
                            ClassName = Convert.ToString(oSqlDataReader["ClassName"]),
                            MaxStrength = Convert.ToInt32(oSqlDataReader["MaxStrength"]),
                            StudentCount = Convert.ToInt32(oSqlDataReader["StudentCount"]),
                            IsExceeded = Convert.ToBoolean(oSqlDataReader["IsExceeded"])
                        });
                    }

                    return lstClasses;
                }
            }
        }
    }

}
