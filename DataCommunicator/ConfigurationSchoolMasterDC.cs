using System;
using Utility;

namespace DataCommunicator
{

    public class ConfigurationSchoolMasterDC
    {
        #region structure

        public struct ConfigurationSchoolMasterStruct
        {
            public int miConfigId;
            public int miOriginalConfigId;
            public int miSchoolId;
            public int miAcademicYearId;
            public int miFinancialYearId;
            public char msIsConfigure;
            public char msIsDeleted;
            public int miInsertedById;
            public DateTime mdtInsertDate;
            public int miUpdateById;
            public DateTime mdtUpdatedDate;
        }

        #endregion

        #region DataMembers and properties

        #region Data members

        private ConfigurationSchoolMasterStruct moConfigurationSchoolMasterStruct;

        #endregion

        #region Properties

        public ConfigurationSchoolMasterStruct ConfigurationSchoolMasterStructDetails
        {
            get { return moConfigurationSchoolMasterStruct; }
            set { moConfigurationSchoolMasterStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public ConfigurationSchoolMasterDC()
        {
        }

        #endregion

        #region Public Methods

        public Int32 InsertConfigurationSchoolMaster()
        {
            string sInsertStatement = "";
            if (moConfigurationSchoolMasterStruct.miFinancialYearId != 0)
            {
                 sInsertStatement = "INSERT INTO Configuration_School_Master ( " +
                    "  original_config_id" +
                    " , school_id" +
                    " , is_configure" +
                    " , inserted_by_id" +
                    " , update_by_id" +
                     " , academic_year_id" +
                    " , FinancialYearId " +
                ") VALUES (" +
                     "  " + moConfigurationSchoolMasterStruct.miOriginalConfigId +
                     " , " + moConfigurationSchoolMasterStruct.miSchoolId +
                     " , N'" + moConfigurationSchoolMasterStruct.msIsConfigure + "' " +
                     " , " + moConfigurationSchoolMasterStruct.miInsertedById +
                     " , " + moConfigurationSchoolMasterStruct.miUpdateById +
                     " , " + moConfigurationSchoolMasterStruct.miAcademicYearId +
                     " , " + moConfigurationSchoolMasterStruct.miFinancialYearId +
                " ) ";
            }
            else
            {
                 sInsertStatement = "INSERT INTO Configuration_School_Master ( " +
                    "  original_config_id" +
                    " , school_id" +
                    " , is_configure" +
                    " , inserted_by_id" +
                    " , update_by_id" +
                     " , academic_year_id" +
                    " , FinancialYearId " +
                ") VALUES (" +
                     "  " + moConfigurationSchoolMasterStruct.miOriginalConfigId +
                     " , " + moConfigurationSchoolMasterStruct.miSchoolId +
                     " ,N'" + moConfigurationSchoolMasterStruct.msIsConfigure + "' " +
                     " , " + moConfigurationSchoolMasterStruct.miInsertedById +
                     " , " + moConfigurationSchoolMasterStruct.miUpdateById +
                     " , " + moConfigurationSchoolMasterStruct.miAcademicYearId +
                     " , NULL "+
                " ) ";
            }
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        public void UpdateConfigurationSchoolMaster()
        {
            string sUpdateStatement = " UPDATE Configuration_School_Master SET " +
              " original_config_id =  " + moConfigurationSchoolMasterStruct.miOriginalConfigId +
              " , school_id =  " + moConfigurationSchoolMasterStruct.miSchoolId +
              " , is_configure =  N'" + moConfigurationSchoolMasterStruct.msIsConfigure + "' " +
              " , inserted_by_id =  " + moConfigurationSchoolMasterStruct.miInsertedById +
              " , update_by_id =  " + moConfigurationSchoolMasterStruct.miUpdateById +

           " WHERE " +
              " is_deleted = N'" + Constants.C_NO + "'" +
               " AND Original_config_id =  " + moConfigurationSchoolMasterStruct.miOriginalConfigId +
            " AND school_Id = " + moConfigurationSchoolMasterStruct.miSchoolId +
            " AND academic_year_Id = " + moConfigurationSchoolMasterStruct.miAcademicYearId;
            if(moConfigurationSchoolMasterStruct.miFinancialYearId!=0)
            sUpdateStatement+= "  AND FinancialYearId = " + moConfigurationSchoolMasterStruct.miFinancialYearId;
            else
                sUpdateStatement += "  AND FinancialYearId = NULL";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public void DeleteConfigurationSchoolMaster()
        {
            
            string sDeleteStatement = " DELETE Configuration_School_Master  " +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                " AND Original_config_id =  " + moConfigurationSchoolMasterStruct.miOriginalConfigId +
                " AND school_Id = " + moConfigurationSchoolMasterStruct.miSchoolId +
                " AND academic_year_Id = " + moConfigurationSchoolMasterStruct.miAcademicYearId ;

            if (moConfigurationSchoolMasterStruct.miFinancialYearId != Constants.I_ZERO)
                sDeleteStatement += "" + " AND (FinancialYearId = " + moConfigurationSchoolMasterStruct.miFinancialYearId + 
                    " OR  FinancialYearId IS NULL)";
            else
                sDeleteStatement += "" + " AND FinancialYearId IS NULL ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        public bool IsSchoolConfigured()
        {
            string sSelectStatement = " SELECT " +
                                      " Count(*) " +
                                   " FROM " +
                                       " Configuration_Master" +
                                   " LEFT OUTER JOIN " +
                                        " Configuration_School_Master " +
                                   " ON " +
                                      " Configuration_Master.Configure_Id = Configuration_School_Master.Original_Config_Id " +
                                   " WHERE " +
                                      " Configuration_School_Master.School_Id =" + moConfigurationSchoolMasterStruct.miSchoolId +
                                      " AND Configuration_School_Master.Original_Config_Id =" + moConfigurationSchoolMasterStruct.miOriginalConfigId +
                                      " AND Configuration_School_Master.academic_year_id =" + moConfigurationSchoolMasterStruct.miAcademicYearId +
                                      " AND Configuration_Master.Is_Deleted =N'" + Constants.C_NO + "' " +
			                          " AND Configuration_School_Master.Is_Deleted =N'" + Constants.C_NO + "' ";

            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            // If the count is zero there is no duplication of Buyer login. 
            if (iCount == 0)
                return false;
            else
                return true;
        }

        #endregion

    }

}
