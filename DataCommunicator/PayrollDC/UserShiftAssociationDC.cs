using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.Data.SqlClient;
using System.Data;

namespace DataCommunicator
{
    public class UserShiftAssociationDC : DataCommunicatorBaseDC
    {
        #region structure

        public struct UserShiftsAssociationDetailsStruct
        {
            public int miUserShiftId;
            public int miShiftId;
            public int miSchoolId;
            public int miUserId;
            public int miAcademicYearId;
            public char mbIs_Deleted;
            public int miInsertedByid;
            public DateTime mdtInsertDate;
        }
        UserShiftsAssociationDetailsStruct moUserShiftAssociationDetailStruct;

        #endregion

        #region DataMembers and properties

        public UserShiftsAssociationDetailsStruct userShiftAssociationDetailStruct
        {

            get { return moUserShiftAssociationDetailStruct; }
            set { moUserShiftAssociationDetailStruct = value; }
        }

        #endregion
      
        /// <summary>
        /// constructs a statement for inserting an item.
        /// </summary>
        /// <returns></returns>
        public string GetUserShiftAssociationInsertStatement()
        {
            string sTeacherId;
            if (userShiftAssociationDetailStruct.miUserId != 0)
                sTeacherId = "   " + moUserShiftAssociationDetailStruct.miUserId;
            else
                sTeacherId = "   " + Constants.S_LAST_INSERTED_P_KEY;

            string sInsertStatement = "INSERT INTO UserShiftsAssociation (" +
                                  " UserId " +
                                  ",ShiftId" +
                                  ",SchoolId" +
                                  ",AcademicYearId" +
                                  ",Is_Deleted" +
                                  ",InsertedDate" +
                                  ",InsertedById" +

                " ) VALUES ( " +
                         sTeacherId +
                    ",   " + moUserShiftAssociationDetailStruct.miShiftId +
                    " ,  " + moUserShiftAssociationDetailStruct.miSchoolId +
                    " ,  " + moUserShiftAssociationDetailStruct.miAcademicYearId +
                    " , N'" + moUserShiftAssociationDetailStruct.mbIs_Deleted + "' " +
                    " , N'" + moUserShiftAssociationDetailStruct.mdtInsertDate.ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                    " , " + moUserShiftAssociationDetailStruct.miInsertedByid +
            " ) ";

            return sInsertStatement;
        }

        /// <summary>
        /// This function is used to associate default shift for other staff.
        /// </summary>
        public void InsertUserShiftAssociationDetailsForOtherStaff()
        {
            string sInsertStatement = "INSERT INTO UserShiftsAssociation (" +
                                 " UserId " +
                                 ",ShiftId" +
                                 ",SchoolId" +
                                 ",AcademicYearId" +
                                 ",Is_Deleted" +
                                 ",InsertedDate" +
                                 ",InsertedById" +

               " ) VALUES ( " +
                        moUserShiftAssociationDetailStruct.miUserId +
                   ",   " + moUserShiftAssociationDetailStruct.miShiftId +
                   " ,  " + moUserShiftAssociationDetailStruct.miSchoolId +
                   " ,  " + moUserShiftAssociationDetailStruct.miAcademicYearId +
                   " , N'" + moUserShiftAssociationDetailStruct.mbIs_Deleted + "' " +
                   " , N'" + moUserShiftAssociationDetailStruct.mdtInsertDate.ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                   " , " + moUserShiftAssociationDetailStruct.miInsertedByid +
           " ) ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        /// <summary>
        /// This function is used to get default shift for other staff.
        /// </summary>
        public static int GetDefaultShift(int aiSchoolId, int aiAcademicYrId)
        {
            int ishiftId = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetDefaultShift");
                if (oSqlDataReader.HasRows)
                {
                    while (oSqlDataReader.Read())
                    {
                        ishiftId = Convert.ToInt32(oSqlDataReader["ShiftId"]);
                    }
                }
                oSqlDataReader.Close();
            }
            
            return ishiftId; 
        }

        /// <summary>
        /// This function is used to get user details for shift association.
        /// </summary>
        public static DataTable GetUserDetails(int aiSchoolId, int aiAcademicYearId, int aiShiftId, int aistaffGroupId, String sSortExpression, string asSearchText, int iEndIndex, int startRowIndex)
        {
            if (sSortExpression == string.Empty || sSortExpression == "Name" || sSortExpression == "Name ASC")
                sSortExpression = "Name";
            else if (sSortExpression == "Name DESC")
                sSortExpression = "Name DESC";

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShiftId", aiShiftId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aistaffGroupId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sSortExpression, SqlDbType.NVarChar);
                if(asSearchText != null)
                    oSQLServerDbUtility.AddParameter("SearchText", asSearchText, SqlDbType.NVarChar);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllUserDetailsforShifts");
			}
        }

        /// <summary>
        /// This function is used to Insert USer Shift Association Details in database.
        /// </summary>
        public void InsertUserShiftAssociationDetailsForUser(string asUserIdXML, int aiSchoolId, int aiAcademicYrId, int aiShiftId, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {                
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserIdsXML", asUserIdXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("ShiftId", aiShiftId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IfAlreadyExistUserShiftAssociation");
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetAllUsersDetails(int aiSchoolId, int aiAcademicYearId, int aiStaffGroupId, int aishiftId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StaffGroupId", aiStaffGroupId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllUserDetailsforShifts");
            }
        }      

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetUsersforSearch(string asName, int aiSchoolId, int aiAcademicYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AcademicYrId", aiAcademicYrId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUserShiftDetailsforSearch");
            }
        }
    }
}
