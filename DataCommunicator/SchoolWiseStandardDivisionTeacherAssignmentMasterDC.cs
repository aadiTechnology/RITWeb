using System;
using System.Data;
using System.Collections;
using Utility;

namespace DataCommunicator
{

    public class SchoolWiseStandardDivisionTeacherAssignmentMasterDC : DataCommunicatorBaseDC
    {

        #region Constant and structures

        #region structure

        public struct SchoolWiseStandardDivisionTeacherAssignmentMasterStruct
        {
            public int miSchoolWiseStandardDivisionSubjectTeacherId;
            public int miSchoolId;
            public int miStandardId;
            public int miDivisionId;
            public int miTeacherId;
            public int miOrgTeacherId;
            public int miAcademicYearId;
            public char msIsClassTeacher;
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

        private SchoolWiseStandardDivisionTeacherAssignmentMasterStruct moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct;

        #endregion

        #region Properties

        public SchoolWiseStandardDivisionTeacherAssignmentMasterStruct SchoolWiseStandardDivisionTeacherAssignmentMasterStructDetails
        {

            get { return moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct; }
            set { moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct = value; }
        }

        #endregion

        #endregion

        #region Constructors

        public SchoolWiseStandardDivisionTeacherAssignmentMasterDC()
        {
        }

        #endregion

        #region Public Methods

        public Int32 InsertSchoolWiseStandardDivisionSubjectTeacherAssignmentMaster()
        {
            string sInsertStatement = "INSERT INTO SchoolWise_Standard_Division_Teacher_Assignment_Master ( " +
                "  school_id" +
                " , standard_id" +
                " , division_id" +
                " , teacher_id" +
                " , Academic_Year_Id " +
                " , is_classteacher" +
                " , inserted_by_id" +
                " , updated_by_id" +
            ") VALUES (" +
                 "   " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miSchoolId +
                 " , " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miStandardId +
                 " , " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miDivisionId +
                 " , " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miTeacherId +
                 " , " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miAcademicYearId +
                 " , N'" + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.msIsClassTeacher + "' " +
                 " , " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miInsertedByid +
                 " , " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miUpdatedById +
            " ) ";

            int iId = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iId = oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);

            UpdateClassTeacherMailIngGroup(moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miSchoolId, moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miAcademicYearId);

            return iId;
        }

        /// <summary>
        /// This method is used to update class teacher mailing group.
        /// </summary>
        /// <param name="aischoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        private void UpdateClassTeacherMailIngGroup(int aischoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aischoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateClassTeacherMailingGroup");
            }
        }

        public void UpdateTeacherDetailsForStandardDivision()
        {

            string sUpdateStatement = " UPDATE SchoolWise_Standard_Division_Teacher_Assignment_Master SET " +
                " teacher_id =  " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miTeacherId +
                " , updated_by_id =  " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miUpdatedById +
             " WHERE " +
                " is_deleted = N'" + Constants.C_NO + "'" +
                " AND standard_id =  " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miStandardId +
                " AND division_id =  " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miDivisionId +
                " AND teacher_id = " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miOrgTeacherId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

            UpdateClassTeacherMailIngGroup(moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miSchoolId, moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miAcademicYearId);

        }

        public bool DeleteAssignStandardDivisionForTeacher(ArrayList aoArrDeleteTeacherIds)
        {
            string sDeleteUserList = "(";
            for (int iCount = 0; iCount < aoArrDeleteTeacherIds.Count; iCount++)
            {
                sDeleteUserList = sDeleteUserList + aoArrDeleteTeacherIds[iCount];
                sDeleteUserList = sDeleteUserList + ",";

            }
            sDeleteUserList = sDeleteUserList + ")";
            sDeleteUserList = sDeleteUserList.Remove(sDeleteUserList.Length - 2, 1);

            string sSqlDeleteUser = " DELETE SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                               " WHERE " +
                                   " teacher_id in " + sDeleteUserList;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);
            return true;

        }


        public bool DeleteAssignTeacherForStandardDivision()
        {
            string sSqlDeleteUser = " DELETE Top(1) SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                               " WHERE " +
                                   " Standard_id=" + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miStandardId +
                                   " AND Division_Id=" + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miDivisionId +
                                   " AND teacher_id = " + moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miOrgTeacherId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);

            UpdateClassTeacherMailIngGroup(moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miSchoolId, moSchoolWiseStandardDivisionTeacherAssignmentMasterStruct.miAcademicYearId);

            return true;

        }

        public bool DeleteAssignStandardDivisionForTeacher(int aiTeacherId)
        {

            string sSqlDeleteUser = " DELETE SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                               " WHERE " +
                                   " teacher_id = " + aiTeacherId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);
            return true;

        }

        public bool IsStandardDivisionAssignToTeacher(int aiStandardId, int aiDivisionId)
        {

            string sSelectStatement;

            sSelectStatement = " SELECT " +
                     " count(*) " +
                 " FROM " +
                     " SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                 " WHERE " +
                     " Standard_Id =" + aiStandardId +
                     " AND Division_Id =" + aiDivisionId;

            // Perform the stetement on server.
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);

            // If the count is zero there is no duplication of Buyer login. 
            if (iCount == 0)
                return false;
            else
                return true;

        }

        /// <summary>
        /// This function returns all the class teachers in the given school
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static DataTable GetAllClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetAllPrimaryClassTeachers]");
            }
        }
        /// <summary>
        /// This function returns all the class teachers in the given school
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static DataTable GetAllClassTeachers1(int aiSchoolId, int aiAcademicYearId,int miuserid)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", miuserid, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetAllPrimaryClassTeachers1]");
            }
        }

        /// <summary>
        /// This method returns the standard - division info of teacher
        /// </summary>
        /// <param name="aiTeacher"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static DataTable GetStandardDivisionOfTeacher(int aiTeacherId, int aiAcademicYearId)
        {
            string sSelectStatement = " SELECT " +
                    " standard_Id " +
                    " , division_Id " +
                    " , standard_Name " +
                    " , division_Name " +
                    " , SchoolWise_Standard_Division_Id " +
					", standard_Name+' - '+division_Name as StandardDivision" +
                " FROM " +
                    " vw_ClassTeacher " +
                " WHERE " +
                    " Teacher_Id =" + aiTeacherId +
                    " AND Academic_Year_Id = N'" + aiAcademicYearId + "'"+
					"Order by Original_Standard_Id, Original_Division_Id";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);

        }

        public static DataSet GetStdDivTeacherAssociation(int aiSchoolId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetClassTeacherAssociation");
            }
        }
        #endregion
    }

}
