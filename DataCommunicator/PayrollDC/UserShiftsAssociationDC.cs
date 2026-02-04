using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using System.Data.SqlClient;
using System.Data;

namespace DataCommunicator
{
    public class UserShiftsAssociationDC : DataCommunicatorBaseDC
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
    }
}
