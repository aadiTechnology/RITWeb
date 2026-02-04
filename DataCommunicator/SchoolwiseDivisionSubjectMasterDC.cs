using System;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{

    public class SchoolwiseDivisionSubjectMasterDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        #region structure

        public struct SchoolwiseDivisionSubjectMasterStruct
        {
            public int miSchoolwiseDivisionSubjectId;
            public string msDivisionSubjectName;
            public int miSchoolId;
            public int miStandardId;
            public int miDivisionId;
            public int miSubjectId;
            public string msIsDeleted;
            public int miInsertedById;
            public DateTime mdtInsertDate;
            public int miUpdatedById;
            public DateTime mdtUpdateDate;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private SchoolwiseDivisionSubjectMasterStruct moSchoolwiseDivisionSubjectMasterStruct;

        #endregion
        #region Properties

        public SchoolwiseDivisionSubjectMasterStruct SchoolwiseDivisionSubjectMasterStructDetails
        {

            get { return moSchoolwiseDivisionSubjectMasterStruct; }
            set { moSchoolwiseDivisionSubjectMasterStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public SchoolwiseDivisionSubjectMasterDC()
        {
        }
        public SchoolwiseDivisionSubjectMasterDC(int aiStandardDivisionId, int aiSubjectId)
        {
            LoadSchoolwiseDivisionSubjectMasterDetails(aiStandardDivisionId, aiSubjectId);
        }
        #endregion

        #region Private Methods

        public void LoadSchoolwiseDivisionSubjectMasterDetails(int aiStandardDivisionId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseDivisionSubjectMasterDataFromDatabase(aiStandardDivisionId, aiSubjectId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Schoolwise_Division_Subject_Id"] != DBNull.Value)
                                moSchoolwiseDivisionSubjectMasterStruct.miSchoolwiseDivisionSubjectId = Convert.ToInt32(oDR["Schoolwise_Division_Subject_Id"].ToString());
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolwiseDivisionSubjectMasterStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"].ToString());
                            if (oDR["Subject_Id"] != DBNull.Value)
                                moSchoolwiseDivisionSubjectMasterStruct.miSubjectId = Convert.ToInt32(oDR["Subject_Id"].ToString());
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolwiseDivisionSubjectMasterStruct.msIsDeleted = oDR["Is_Deleted"].ToString();
                            if (oDR["Inserted_By_Id"] != DBNull.Value)
                                moSchoolwiseDivisionSubjectMasterStruct.miInsertedById = Convert.ToInt32(oDR["Inserted_By_Id"].ToString());
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolwiseDivisionSubjectMasterStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"].ToString());
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolwiseDivisionSubjectMasterStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"].ToString());
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolwiseDivisionSubjectMasterStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"].ToString());
                        }
                    }
                }
            }
        }
        public string FetchSchoolwiseDivisionSubjectMasterDataFromDatabase(int aiStandardDivisionId, int aiSubjectId)
        {

            string sSelectStatement = " SELECT  " +
                "Schoolwise_Division_Subject_Id" +
                " , school_id" +
                " , academic_year_id" +
                " , subject_id" +
                " , is_deleted" +
                " , inserted_by_id" +
                " , insert_date" +
                " , updated_by_id" +
                " , update_date" +
            " FROM  " +
                "Schoolwise_Division_Subject_Master " +
            " WHERE  " +
                 " Standard_Division_Id = " + aiStandardDivisionId +
                 " AND  Subject_Id = " + aiSubjectId +
                " AND is_deleted = '" + Constants.C_NO + "'";
            return sSelectStatement;
        }

        #endregion

        #region Public Methods

        public Int32 InsertSchoolwiseDivisionSubjectMaster()
        {

            string sInsertStatement = "INSERT INTO Schoolwise_Division_Subject_Master ( " +
                "schoolwise_division_subject_id" +
                " , school_id" +
                " , standard_id" +
                " , division_id" +
                " , subject_id" +
                " , is_deleted" +
                " , inserted_by_id" +
                " , insert_date" +
                " , updated_by_id" +
                " , update_date" +

            ") VALUES (" + "  " + moSchoolwiseDivisionSubjectMasterStruct.miSchoolwiseDivisionSubjectId +
                 " , " + moSchoolwiseDivisionSubjectMasterStruct.miSchoolId +
                 " , " + moSchoolwiseDivisionSubjectMasterStruct.miStandardId +
                 " , " + moSchoolwiseDivisionSubjectMasterStruct.miDivisionId +
                 " , " + moSchoolwiseDivisionSubjectMasterStruct.miSubjectId +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseDivisionSubjectMasterStruct.msIsDeleted, false) + "' " +
                 " , " + moSchoolwiseDivisionSubjectMasterStruct.miInsertedById +
                 " , N'" + moSchoolwiseDivisionSubjectMasterStruct.mdtInsertDate + "' " +
                 " , " + moSchoolwiseDivisionSubjectMasterStruct.miUpdatedById +
                 " , N'" + moSchoolwiseDivisionSubjectMasterStruct.mdtUpdateDate + "' " +
            " ) ";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        public void UpdateSchoolwiseDivisionSubjectMaster()
        {

            string sUpdateStatement = " UPDATE Schoolwise_Division_Subject_Master SET " +
                "schoolwise_division_subject_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miSchoolwiseDivisionSubjectId +
                " , school_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miSchoolId +
                " , standard_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miStandardId +
                " , division_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miDivisionId +
                " , subject_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miSubjectId +
                " , is_deleted =  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseDivisionSubjectMasterStruct.msIsDeleted, false) + "' " +
                " , inserted_by_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miInsertedById +
                " , insert_date =  N'" + moSchoolwiseDivisionSubjectMasterStruct.mdtInsertDate + "' " +
                " , updated_by_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miUpdatedById +
                " , update_date =  N'" + moSchoolwiseDivisionSubjectMasterStruct.mdtUpdateDate + "' " +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND schoolwise_division_subject_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miSchoolwiseDivisionSubjectId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public void DeleteSchoolwiseDivisionSubjectMaster()
        {

            string sUpdateStatement = " UPDATE Schoolwise_Division_Subject_Master SET " +
                 " is_deleted = N'" + Constants.C_YES + "'" +
                " , update_date = " + Constants.S_SERVER_CURRENT_DATE_TIME +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND schoolwise_division_subject_id =  " + moSchoolwiseDivisionSubjectMasterStruct.miSchoolwiseDivisionSubjectId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }


        #endregion

        #region Static Methods

        /// <summary>
        /// Returns dataset containing 3 tables.
        /// Table 1 - All Standard-divisions associated for the school.
        /// Table 2 - All Subjects defined for the school.
        /// Table 3 - All Standard-divisions-subjects(if any) associated for the school.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicId"></param>
        /// <returns></returns>

        public static DataSet GetStandardDivisionSubjectsAssociation(int aiSchoolId, int aiAcademicId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetStandardDivisionSubjectsAssociation");
            }
        }
        
        #endregion Static Methods
    }
}
