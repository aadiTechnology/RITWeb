using System;
using System.Data;
using System.Collections;
using Utility;

namespace DataCommunicator
{
    public class DivisionMasterDC
    {

        #region Constant and structures

        #region structure

        public struct DivisionMasterStruct
        {
            public int miDivisionId;
            public int miSchoolId;
            public int miAcademicYearId;
            public string msDivisionName;
            public int miOriginalDivisionId;
            public int miInsertedByid;
            public int miUpdatedById;
            public int miBatchWiseId;
        }

        // Structure for divisions per standard.
        public struct StandardDivisionStruct
        {
            public int miStandardId;
            public string msDisplayName;
        }

        #endregion
        #endregion

        #region DataMembers and properties

        #region Data members

        private DivisionMasterStruct moDivisionMasterStruct;
        private StandardDivisionStruct moStandardDivisionStruct;

        #endregion
        #region Properties

        public DivisionMasterStruct DivisionMasterStructDetails
        {

            get { return moDivisionMasterStruct; }
            set { moDivisionMasterStruct = value; }
        }

        public StandardDivisionStruct StandardDivisionStructDetails
        {

            get { return moStandardDivisionStruct; }
            set { moStandardDivisionStruct = value; }
        }

        #endregion
        #endregion

        #region Constructors

        public DivisionMasterDC()
        {
        }
        #endregion

        #region Public Methods

        #region Divisions

        public string GetInsertStatementForDivision()
        {

            string sInsertStatement = "INSERT INTO Division_Master ( " +
                " school_id" +
                " , division_name" +
                " , original_division_id" +
                " , inserted_by_id" +
                " , updated_by_id" +
                " , academic_year_id" +
            ") VALUES (" +
                 moDivisionMasterStruct.miSchoolId +
                 " , N'" + StringUtility.ReplaceSingleQuoteInString(moDivisionMasterStruct.msDivisionName, false) + "' " +
                 " , " + moDivisionMasterStruct.miOriginalDivisionId +
                 " , " + moDivisionMasterStruct.miInsertedByid +
                 " , " + moDivisionMasterStruct.miUpdatedById +
                 " , " + moDivisionMasterStruct.miAcademicYearId +
            " ) ";
            return sInsertStatement;

        }

        public string GetUpdateStatementForDivision()
        {

            string sUpdateStatement = " UPDATE Division_Master SET " +
                " school_id =  " + moDivisionMasterStruct.miSchoolId +
                " , division_name = N'" + StringUtility.ReplaceSingleQuoteInString(moDivisionMasterStruct.msDivisionName, false) + "' " +
                " , original_division_id =  " + moDivisionMasterStruct.miOriginalDivisionId +
                " , updated_by_id =  " + moDivisionMasterStruct.miUpdatedById +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND division_id =  " + moDivisionMasterStruct.miDivisionId;
            return sUpdateStatement;
        }

        public string GetDeleteStatementForDivision()
        {

            string sUpdateStatement = " DELETE Division_Master " +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                 " AND division_id =  " + moDivisionMasterStruct.miDivisionId;
            return sUpdateStatement;
        }

        public DataTable GetDivisionsForHomeWork(int aiStdDivId, int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiSubjectId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardDivisionId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubjectId", aiSubjectId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetDivisionForHomeWork");
            }
        }

        #endregion

        #region Standard Divisions

        public string GetInsertStatementForStandardDivision()
        {
            string sInsertStatement = "";
            string sDisplayName = "NULL";
            if (moStandardDivisionStruct.msDisplayName.TrimAll() != string.Empty)
                sDisplayName = "N'" + StringUtility.ReplaceSingleQuoteInString(moStandardDivisionStruct.msDisplayName, true) + "'";
            sInsertStatement = " INSERT INTO SchoolWise_Standard_Division_Master " +
                                    " ( " + "school_id" +
                                    " , " + "Standard_Id" +
                                    " , " + "Division_Id" +
                                    " , " + "Inserted_By_id" +
                                    " , " + "Updated_By_Id" +
                                    " , " + "academic_year_Id" +
                                    " , " + "DisplayNameForDivision" +
                                     " , " + "BatchWiseId" +
                                    
                               " ) VALUES ( " +
                                    " N'" + moDivisionMasterStruct.miSchoolId + "'" +
                                    " , N'" + moStandardDivisionStruct.miStandardId + "'" +
                                    " , N'" + moDivisionMasterStruct.miDivisionId + "'" +
                                    " , N'" + moDivisionMasterStruct.miUpdatedById + "'" +
                                    " , N'" + moDivisionMasterStruct.miUpdatedById + "'" +
                                     " , N'" + moDivisionMasterStruct.miAcademicYearId + "'" +
                                    
                                    " ," + sDisplayName + "" +
                                     " , N'" + moDivisionMasterStruct.miBatchWiseId + "'" +
                                     ")";

            return sInsertStatement;
        }

        public string GetDeleteStatementForStandardDivision()
        {
            string sUpdateStatement = "";
            sUpdateStatement = " DELETE FROM SchoolWise_Standard_Division_Master " +
                               "  WHERE " +
                                    " Standard_Id  = N'" + moStandardDivisionStruct.miStandardId + "'" +
                                    " AND Division_Id = N'" + moDivisionMasterStruct.miDivisionId + "'" +
                                    " AND is_deleted = N'" + Constants.C_NO + "'";
            return sUpdateStatement;
        }

        public string GetUpdateStatementForStandardDivision()
        {
            string sUpdateStatement = "";
            string sDisplayName = "SET DisplayNameForDivision=NULL";
            if (moStandardDivisionStruct.msDisplayName.TrimAll()!=string.Empty)
                sDisplayName = "SET DisplayNameForDivision= N'" + StringUtility.ReplaceSingleQuoteInString(moStandardDivisionStruct.msDisplayName, true) + "'";
            sUpdateStatement = " UPDATE  SchoolWise_Standard_Division_Master " +
                               //"SET DisplayNameForDivision= N'" + StringUtility.ReplaceSingleQuoteInString(moStandardDivisionStruct.msDisplayName, true) +"'"+
                               sDisplayName+   
                               "  WHERE " +
                                    " Standard_Id  = N'" + moStandardDivisionStruct.miStandardId + "'" +
                                    " AND Division_Id = N'" + moDivisionMasterStruct.miDivisionId + "'" +
                                    " AND is_deleted = N'" + Constants.C_NO + "'";
            return sUpdateStatement;
        }

        #endregion

        #endregion

    }

    public class DivisionCollectionDC
    {
        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        public DivisionCollectionDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
        }
        public DivisionCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        public DataTable GetAllDivisions()
        {
            // This method returns datatable populated with master standards from databse.
            string sSelectStatement;
            sSelectStatement = " SELECT " +
                       " -9999 as school_id" +
                       " , original_division_id " +
                       " , division_id " +
                       " , division_name " +
                   " FROM " +
                        " division_master " +
                   " WHERE " +
                        " is_deleted = N'" + Constants.C_NO + "'" +
                        " AND school_id is null " +
                        " AND division_id NOT IN " +
                        " ( " +
                         " SELECT  " +
                               " original_division_id " +
                           " FROM " +
                                " division_master " +
                           " WHERE " +
                                " is_deleted = N'" + Constants.C_NO + "'" +
                                " AND school_id = " + miSchoolId +
                                " AND academic_year_id = " + miAcademicYearId +
                           " )" +
                   " UNION " +
                    " SELECT  " +
                       " school_id " +
                       " , original_division_id " +
                       " , division_id " +
                       " , division_name " +
                   " FROM " +
                        " division_master " +
                   " WHERE " +
                        " is_deleted = N'" + Constants.C_NO + "'" +
                        " AND school_id = " + miSchoolId +
                        " AND academic_year_id = N'" + miAcademicYearId + "'" +
                    " ORDER BY " +
                         " original_division_id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        public DataTable GetAllDivisionsForStandard(int aiStandardId)
        {
            string sQuery = "SELECT " +
                    " SchoolWise_Standard_Division_Id " +
                    ", division_id " +
                    " , division_name " +
                "FROM " +
                    " vw_standard_division " +
                " WHERE " +
                    " school_id = N'" + miSchoolId + "'" +
                    " AND academic_year_id = N'" + miAcademicYearId + "'" +
                    " AND standard_id = N'" + aiStandardId + "'" +
                    " ORDER BY " +
                         " original_division_id";
                    

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);

        }

        public DataTable GetAllDivisionsForStandardForAdmissionConfirmation(int aiStandardId, int aiAdmissionTypeId)
        {
            string sQuery = string.Empty;
            if (miSchoolId != Constants.SchoolId.SPS.ToInt())
            {
                sQuery = "SELECT " +
                        " SchoolWise_Standard_Division_Id " +
                        ", division_id " +
                        " , division_name " +
                    "FROM " +
                        " vw_standard_division " +
                    " WHERE " +
                        " school_id = N'" + miSchoolId + "'" +
                        " AND academic_year_id = N'" + miAcademicYearId + "'" +
                        " AND standard_id = N'" + aiStandardId + "'" +
                        " ORDER BY " +
                             " original_division_id";
            }
            else
            {
                if (aiAdmissionTypeId == Constants.I_ONE)
                {
                    sQuery = "SELECT " +
                        " SchoolWise_Standard_Division_Id " +
                        ", division_id " +
                        " , division_name " +
                    "FROM " +
                        " vw_standard_division " +
                    " WHERE " +
                        " school_id = N'" + miSchoolId + "'" +
                        " AND academic_year_id = N'" + miAcademicYearId + "'" +
                        " AND standard_id = N'" + aiStandardId + "'" +
                        " AND division_name LIKE ('%DS%') " +
                        " ORDER BY " +
                             " original_division_id";
                }
                else if (aiAdmissionTypeId == Constants.I_TWO || aiAdmissionTypeId == Constants.I_THREE)
                {
                    sQuery = "SELECT " +
                        " SchoolWise_Standard_Division_Id " +
                        ", division_id " +
                        " , division_name " +
                    "FROM " +
                        " vw_standard_division " +
                    " WHERE " +
                        " school_id = N'" + miSchoolId + "'" +
                        " AND academic_year_id = N'" + miAcademicYearId + "'" +
                        " AND standard_id = N'" + aiStandardId + "'" +
                        " AND division_name NOT LIKE ('%DS%') " +
                        " AND division_name NOT LIKE ('%DB%') " +
                        " ORDER BY " +
                             " original_division_id";
                }
                else if (aiAdmissionTypeId == Constants.I_FOUR)
                {
                    sQuery = "SELECT " +
                        " SchoolWise_Standard_Division_Id " +
                        ", division_id " +
                        " , division_name " +
                    "FROM " +
                        " vw_standard_division " +
                    " WHERE " +
                        " school_id = N'" + miSchoolId + "'" +
                        " AND academic_year_id = N'" + miAcademicYearId + "'" +
                        " AND standard_id = N'" + aiStandardId + "'" +                        
                        " AND division_name LIKE ('%DB%') " +
                        " ORDER BY " +
                             " original_division_id";
                }
            }


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);

        }

        public DataTable GetAllDivisionsForAdmissionSibling(int aiStandardId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAssociatedDivisionForSiblingDetails");
            }
        }

        public DataTable GetAllDivisionsForStandards(string asStandardIds)
        {
            string sQuery = "SELECT DISTINCT" +
                    " SchoolWise_Standard_Division_Id " +
                    ", division_id " +
                    " , division_name " +
                "FROM " +
                    " vw_standard_division " +
                " WHERE " +
                    " school_id = N'" + miSchoolId + "'" +
                    " AND academic_year_id = N'" + miAcademicYearId + "'" +
                    " AND standard_id IN (" + asStandardIds + ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);

        }

        

        /// <summary>
        /// This method is used to get all school associated divisions.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSchoolDivisions()
        {
            string sQuery = " SELECT " +
                                " * " +
                            " FROM " +
                                " dbo.Division_Master " +
                            " WHERE " +
                               " school_id = " + miSchoolId +
                               " AND " +
                               " academic_year_id = " + miAcademicYearId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }

        /// <summary>
        /// This method is sued to get std-div ids for school.
        /// </summary>
        /// <returns></returns>
        public DataTable GetStdDivIdForSchool()
        {
            string sQuery = " SELECT " +
                  " SchoolWise_Standard_Division_Id " +
                  ", division_id " +
                  " , division_name " +
              "FROM " +
                  " vw_standard_division " +
              " WHERE " +
                  " school_id = " + miSchoolId +
                  " AND academic_year_id = " + miAcademicYearId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }

        /// <summary>
        /// This method is used to get stddivid for given class.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <returns></returns>
        public DataTable GetStdDivIdForClass(int aiStandardId, int aiDivisionId)
        {
            string sQuery = " SELECT DISTINCT" +
                                " SchoolWise_Standard_Division_Id " +
                            "FROM " +
                                " vw_standard_division " +
                            " WHERE " +
                                " school_id = " + miSchoolId +
                                " AND academic_year_id = " + miAcademicYearId +
                                " AND standard_id = " + aiStandardId +
                                " AND Division_Id = " + aiDivisionId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);

        }

        public void UpdateDivisions(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }
    }
}
