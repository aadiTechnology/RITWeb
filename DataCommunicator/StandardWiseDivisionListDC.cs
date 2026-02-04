using System;
using System.Collections;
using System.Data;
using Utility;

namespace DataCommunicator
{
    public class StandardWiseDivisionListDC
    {
        #region constants and structures
        /// <summary>
        /// Table structure for SchoolWise_Standard_Division_Master
        /// </summary>
        public struct StandardDivision
        {
            public int iId;
            public int iSchoolId;
            public int iStandardId;
            public int iDivisionId;
            public string sDivisionName;
            public int iUpdatedBy;
        }
        /// <summary>
        /// Table structure for  Standard_Master table
        /// </summary>
        public struct StandardMaster
        {
            public int iId;
            public int iSchoolId;
            public int iStandardId;
            public string sStandardName;
        }
        /// <summary>
        /// Table structure for  Division_Master table.
        /// </summary>
        public struct DivisionMaster
        {
            public int iId;
            public int iSchoolId;
            public int iDivisionId;
            public string sDivisionName;
            public int iUpdatedBy;
        }

        #endregion

        #region data members
        private StandardDivision moStandardDivision;
        private StandardMaster moStandardMaster;
        private DivisionMaster moDivisionMaster;
        #endregion

        private bool mbIsValid;
        #region properties
        public bool IsValid
        {
            get
            {
                return mbIsValid;
            }
            set
            {
                mbIsValid = value;
            }
        }
        public StandardDivision StandardDivisionInfo
        {
            get
            {
                return moStandardDivision;
            }
            set
            {
                moStandardDivision = value;
            }

        }
        public StandardMaster StandardMasterInfo
        {
            get
            {
                return moStandardMaster;
            }
            set
            {
                moStandardMaster = value;
            }
        }
        public DivisionMaster DivisionMasterInfo
        {
            get
            {
                return moDivisionMaster;
            }
            set
            {
                moDivisionMaster = value;
            }
        }
        #endregion

        #region constructors
        public StandardWiseDivisionListDC()
        {
        }
        public StandardWiseDivisionListDC(int aiSchoolId)
        {
            mbIsValid = ChkIfStandardDivisionCOnfigured(aiSchoolId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        public StandardWiseDivisionListDC(int aiSchoolId, int aiStandardId, int aiDivisionId)
        {
            moStandardDivision.iSchoolId = aiSchoolId;
            moStandardDivision.iStandardId = aiStandardId;
            moStandardDivision.iDivisionId = aiDivisionId;
        }


        #endregion

        #region public methods
        public bool ChkIfStandardDivisionCOnfigured(int aiSchoolId)
        {
            bool bReturn;
            string sChkString = "SELECT count(Standard_Id) as count from standard_Master " +
                                " WHERE School_id=N'" + aiSchoolId + "'" +
                                " AND Is_Deleted=N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                DataTable oDtStandardCount = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sChkString);

                sChkString = "SELECT count(Division_Id) as count from Division_Master " +
                                    " WHERE School_id=N'" + aiSchoolId + "'" +
                                    " AND Is_Deleted=N'" + Constants.C_NO + "'";

                DataTable oDtDivisionCount = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sChkString);
                if ((Convert.ToInt32(oDtDivisionCount.Rows[0]["count"]) > 0) && (Convert.ToInt32(oDtStandardCount.Rows[0]["count"]) > 0))
                {
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }
            }
            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet GetAllDivisions(int aiSchoolId)
        {

            string sQuery = "SELECT Division_Id, Division_Name, Original_Division_Id " +
                            " FROM Division_Master " +
                            " WHERE School_Id = N'" + aiSchoolId + "'" +
                            " AND Is_Deleted = N'" + Constants.C_NO + "'" +
                            " ORDER BY Division_Name";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sQuery);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public DataSet GetAllStandardDivisionsForSchool(int aiSchoolId)
        {
            string sQuery = " SELECT " +
                            "  Standard_Master.Standard_Name + ' - ' + Division_Master.Division_Name As StandardDivision " +
                //", Division_Master.Division_Name " +
                            ", SchoolWise_Standard_Division_Master.Schoolwise_Standard_division_Id " +
                            ", Standard_Master.Standard_Id " +
                //", Division_Master.Division_Id " +
                        " FROM " +
                            " Standard_Master " +
                        " INNER JOIN " +
                            " SchoolWise_Standard_Division_Master " +
                        " ON " +
                            " Standard_Master.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id " +
                        " INNER JOIN " +
                            " Division_Master " +
                        " ON " +
                            " Division_Master.Division_Id = SchoolWise_Standard_Division_Master.Division_Id " +
                        " WHERE " +
                            " SchoolWise_Standard_Division_Master.School_Id =" + aiSchoolId +
                              " AND Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                              " AND SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                              " AND Standard_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                              " ORDER BY SchoolWise_Standard_Division_Master.Standard_Id ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sQuery);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public ArrayList GetAllDivisionsForStandard(int aiSchoolId, int aiStandardId)
        {
            ArrayList arrayReturn = new ArrayList();
            string sQuery = "SELECT Division_Id FROM SchoolWise_Standard_Division_Master " +
                            " WHERE School_Id = N'" + aiSchoolId + "'" +
                            " AND Standard_Id = N'" + aiStandardId + "'" +
                            " AND Is_Deleted = N'" + Constants.C_NO + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                DataTable oTable = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);

                if (oTable.Rows.Count > 0)
                {
                    int iCnt = oTable.Rows.Count;
                    for (int i = 0; i < iCnt; i++)
                    {
                        if (oTable.Rows[0]["Division_Id"] != DBNull.Value)
                            arrayReturn.Add(oTable.Rows[i]["Division_Id"]);
                    }
                }
            }

            return arrayReturn;
        }
        public ArrayList GetStudentDivisionsForStandard(int aiSchoolId, int aiStandardId)
        {
            ArrayList arrayReturn = new ArrayList();
            string sQuery = "SELECT  Division_id FROM YearWise_Student_Details WHERE Standard_Id=N'" +
                            aiStandardId + "' AND School_Id= N'" + aiSchoolId + "'" + "";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                DataTable oTable = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
                if (oTable.Rows.Count > 0)
                {
                    int iCnt = oTable.Rows.Count;
                    for (int i = 0; i < iCnt; i++)
                    {
                        if (oTable.Rows[0]["Division_id"] != DBNull.Value)
                            arrayReturn.Add(oTable.Rows[i]["Division_Id"]);
                    }
                }
            }
            return arrayReturn;
        }

        public string GetInsertStatementForDivision()
        {
            string sInsertStatement = "";
            sInsertStatement = " INSERT INTO SchoolWise_Standard_Division_Master " +
                                    " ( " + "school_id" +
                                    " , " + "Standard_Id" +
                                    " , " + "Division_Id" +
                                    " , " + "Division_Name" +
                                    " , " + "Is_Deleted" +
                                    " , " + "Inserted_By_id" +
                                    " , " + "Updated_By_Id" +
                               " ) VALUES ( " +
                                    " N'" + moStandardDivision.iSchoolId + "'" +
                                    " , N'" + moStandardDivision.iStandardId + "'" +
                                    " , N'" + moStandardDivision.iDivisionId + "'" +
                                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moStandardDivision.sDivisionName, true) + "'" +
                                    " , N'" + Constants.C_NO + "'" +
                                    " , N'" + moStandardDivision.iUpdatedBy + "'" +
                                    " , N'" + moStandardDivision.iUpdatedBy + "'" +
                               ")";

            return sInsertStatement;
        }
        public string GetUpdateStatementForConfigurationDetails()
        {
            string sUpdateStatement = "";
            sUpdateStatement = " UPDATE SchoolWise_Standard_Division_Master SET  " +
                               "  Division_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moStandardDivision.sDivisionName, true) + "'" +
                               "  WHERE " +
                               " Standard_Id = " + moStandardDivision.iStandardId +
                               " AND Division_Id = " + moStandardDivision.iDivisionId +
                               " AND is_deleted = N'" + Constants.C_NO + "'";
            return sUpdateStatement;
        }

        public string CheckIfConfigurationInUse()
        {
            string sUpdateStatement = "";
            return sUpdateStatement;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDeleteStatementForConfigurationDetails()
        {
            string sUpdateStatement = "";
            sUpdateStatement = " DELETE FROM SchoolWise_Standard_Division_Master " +
                               "  WHERE " +
                                    " Standard_Id  = N'" + moStandardDivision.iStandardId + "'" +
                                    " AND Division_Id = N'" + moStandardDivision.iDivisionId + "'" +
                                    " AND is_deleted = N'" + Constants.C_NO + "'";
            return sUpdateStatement;
        }
        public void UpdateConfigurationDetails(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }
        #endregion

        #region Code Needed

        #endregion
    }
}
