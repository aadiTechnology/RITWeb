using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{

    public class SchoolWiseStandardSubjectMasterDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        #region structure

        public struct SchoolWiseStandardSubjectMasterStruct
        {
            public int miSchoolWiseStandardSubjectId;
            public int miStandardId;
            public string msSubjectId;
            public string msSchoolId;
            public string msIsDeleted;
            public DateTime mdtInsertDate;
            public int miInsertedByid;
            public DateTime mdtUpdateDate;
            public int miUpdatedById;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private SchoolWiseStandardSubjectMasterStruct moSchoolWiseStandardSubjectMasterStruct;

        #endregion
        #region Properties

        public SchoolWiseStandardSubjectMasterStruct SchoolWiseStandardSubjectMasterStructDetails
        {

            get { return moSchoolWiseStandardSubjectMasterStruct; }
            set { moSchoolWiseStandardSubjectMasterStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SchoolWiseStandardSubjectMasterDC()
        {
        }
        public SchoolWiseStandardSubjectMasterDC(int aiStandardId, int aiSubjectId)
        {
            LoadSchoolWiseStandardSubjectMasterDetails(aiStandardId, aiSubjectId);
        }

        #endregion

        #region Private Methods

        public void LoadSchoolWiseStandardSubjectMasterDetails(int aiStandardId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolWiseStandardSubjectMasterDataFromDatabase(aiStandardId, aiSubjectId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {

                            if (oDR["SchoolWise_Standard_Subject_Id"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.miSchoolWiseStandardSubjectId = Convert.ToInt32(oDR["SchoolWise_Standard_Subject_Id"].ToString());
                            if (oDR["Standard_Id"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.miStandardId = Convert.ToInt32(oDR["Standard_Id"].ToString());
                            if (oDR["Subject_Id"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.msSubjectId = oDR["Subject_Id"].ToString();
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.msSchoolId = oDR["School_Id"].ToString();
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.msIsDeleted = oDR["Is_Deleted"].ToString();
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"].ToString());
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"].ToString());
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"].ToString());
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolWiseStandardSubjectMasterStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"].ToString());

                        }
                    }
                }
            }
        }
        public string FetchSchoolWiseStandardSubjectMasterDataFromDatabase(int aiStandardId, int aiSubjectId)
        {

            string sSelectStatement = " SELECT  " +
                "schoolwise_standard_subject_id" +
                " , standard_id" +
                " , subject_id" +
                " , school_id" +
                " , is_deleted" +
                " , insert_date" +
                " , inserted_by_id" +
                " , update_date" +
                " , updated_by_id" +

            " FROM  " +
                "SchoolWise_Standard_Subject_Master " +
            " WHERE  " +
                 "standard_Id = " + aiStandardId +
                 " AND subject_Id = N'" + aiSubjectId + "'" +
                " AND is_deleted = N'" + Constants.C_NO + "'";
            return sSelectStatement;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to get standard id of year.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiStudentId"></param>
        /// <returns></returns>
        public static int GetStandardOfYear(int aiSchoolId, int aiAcademicYrId, int aiStudentId)
        {
            string sSelectStatement = "SELECT [dbo].[Udf_GetStandardIdOfYear](" + aiSchoolId + "," + aiAcademicYrId + "," + aiStudentId + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
        }


        public Int32 InsertSchoolWiseStandardSubjectMaster()
        {

            string sInsertStatement = "INSERT INTO SchoolWise_Standard_Subject_Master ( " +
                "schoolwise_standard_subject_id" +
                " , standard_id" +
                " , subject_id" +
                " , school_id" +
                " , is_deleted" +
                " , insert_date" +
                " , inserted_by_id" +
                " , update_date" +
                " , updated_by_id" +

            ") VALUES (" + "  " + moSchoolWiseStandardSubjectMasterStruct.miSchoolWiseStandardSubjectId +
                 " , " + moSchoolWiseStandardSubjectMasterStruct.miStandardId +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseStandardSubjectMasterStruct.msSubjectId, false) + "' " +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseStandardSubjectMasterStruct.msSchoolId, false) + "' " +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseStandardSubjectMasterStruct.msIsDeleted, false) + "' " +
                 " , N'" + moSchoolWiseStandardSubjectMasterStruct.mdtInsertDate + "' " +
                 " , " + moSchoolWiseStandardSubjectMasterStruct.miInsertedByid +
                 " , N'" + moSchoolWiseStandardSubjectMasterStruct.mdtUpdateDate + "' " +
                 " , " + moSchoolWiseStandardSubjectMasterStruct.miUpdatedById +
            " ) ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        public void UpdateSchoolWiseStandardSubjectMaster()
        {

            string sUpdateStatement = " UPDATE SchoolWise_Standard_Subject_Master SET " +
                "schoolwise_standard_subject_id =  " + moSchoolWiseStandardSubjectMasterStruct.miSchoolWiseStandardSubjectId +
                " , standard_id =  " + moSchoolWiseStandardSubjectMasterStruct.miStandardId +
                " , subject_id =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseStandardSubjectMasterStruct.msSubjectId, false) + "' " +
                " , school_id =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseStandardSubjectMasterStruct.msSchoolId, false) + "' " +
                " , is_deleted =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolWiseStandardSubjectMasterStruct.msIsDeleted, false) + "' " +
                " , insert_date =  N'" + moSchoolWiseStandardSubjectMasterStruct.mdtInsertDate + "' " +
                " , inserted_by_id =  " + moSchoolWiseStandardSubjectMasterStruct.miInsertedByid +
                " , update_date =  N'" + moSchoolWiseStandardSubjectMasterStruct.mdtUpdateDate + "' " +
                " , updated_by_id =  " + moSchoolWiseStandardSubjectMasterStruct.miUpdatedById +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND schoolwise_standard_subject_id =  " + moSchoolWiseStandardSubjectMasterStruct.miSchoolWiseStandardSubjectId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public void DeleteSchoolWiseStandardSubjectMaster()
        {

            string sUpdateStatement = " UPDATE SchoolWise_Standard_Subject_Master SET " +
                 " is_deleted = N'" + Constants.C_YES + "'" +
                " , update_date = " + Constants.S_SERVER_CURRENT_DATE_TIME +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND schoolwise_standard_subject_id =  " + moSchoolWiseStandardSubjectMasterStruct.miSchoolWiseStandardSubjectId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public ArrayList GetAllSubjectsforStandard(int aiSchoolId, int aiStandardId)
        {
            ArrayList arrReturnArray = new ArrayList();
            string sSelectStatement = " SELECT " +
                                        " SchoolWise_Standard_Subject_Master.Subject_Id " +
                                    " FROM " +
                                        " Subject_Master " +
                                    " INNER JOIN " +
                                        " SchoolWise_Standard_Subject_Master " +
                                    " ON " +
                                          " SchoolWise_Standard_Subject_Master.Subject_Id = Subject_Master.Subject_Id " +
                                    " INNER JOIN " +
                                        " Standard_Master " +
                                    " ON  " +
                                     " Standard_Master.Standard_Id = SchoolWise_Standard_Subject_Master.Standard_Id " +
                                    " WHERE " +
                                        " SchoolWise_Standard_Subject_Master.School_Id= " + aiSchoolId +
                                        " AND SchoolWise_Standard_Subject_Master.Standard_Id= " + aiStandardId +
                                        " AND SchoolWise_Standard_Subject_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Standard_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Subject_Master.Is_Deleted= N'" + Constants.C_NO + "' ";
            DataTable oDT;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oDT = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

            if (oDT.Rows.Count > 0)
            {
                int iCnt = oDT.Rows.Count;
                for (int i = 0; i < iCnt; i++)
                {
                    if (oDT.Rows[0]["Subject_Id"] != DBNull.Value)
                        arrReturnArray.Add(oDT.Rows[i]["Subject_Id"]);
                }
            }
            return arrReturnArray;
        }

        public DataSet GetStdSubjectAssociation(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardSubjectAssociation");
            }
        }

        #endregion

    }

}
