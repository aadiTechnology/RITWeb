using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using XseedReportEntities;
using Utility;

namespace DataCommunicator
{
    public class TeacherSubjectDetailsDC  : DataCommunicatorBaseDC
    {
        #region " Data Members & Properties "

        #region " Data Members "

        //This Structure is for the members of the Item

        public struct TeacherSubInfoStruct
        {                  
            public int miTeacherSubjectId;
            public int miTeacherId;           
            public int miSubjectId;
            public int miYearOfPassingId;
            public string msSubjectName;
            //public string msComments;                      
            public int miInsertedById;
            public int miUpdatedById;
         
        }
        TeacherSubInfoStruct moTeacherSubInfoStruct;

        #endregion

        #region " Properties "

        public TeacherSubInfoStruct TeacherSubInfoStructure
        {
            get
            {
                return moTeacherSubInfoStruct;
            }
            set
            {
                moTeacherSubInfoStruct = value;
            }
        }

      //  An unhandled exception of type 'System.StackOverflowException' occurred in DataCommunicator.DLL
        #endregion

        #endregion

        #region " Overloaded Constructor"

        public TeacherSubjectDetailsDC()
        {
            //Default constructor is used to create the object.
            moTeacherSubInfoStruct.miTeacherId = 0;
        }

        public TeacherSubjectDetailsDC(int aiTeacherId)
        {
            // This Overloaded constructor get the parameter as ItemId.
            // And is used to View / Edit the Item.
            // LoadTeacherSubjectDetails(aiTeacherId);
        }


        #endregion

        #region " Public Methods "

        /// <summary>
        /// constructs a statement for inserting an item.
        /// </summary>
        /// <returns></returns>
        public string GetSubjectDetailsInsertStatement()
        {
            string sTeacherId;
            if (moTeacherSubInfoStruct.miTeacherId != 0)
                sTeacherId = "   " + moTeacherSubInfoStruct.miTeacherId;          
            else            
                sTeacherId = "   " + Constants.S_LAST_INSERTED_P_KEY; 

            string sInsertStatement = "INSERT INTO Teacher_Subject_Details (" +
                                  " Teacher_Id " +
                                  ",Subject_Id " +
                                  //",Comments" +                                                     
                                  ",Inserted_By_id " +
                                  ",Updated_By_Id " +

                " ) VALUES ( " +
                         sTeacherId +                  
                    ",   " + moTeacherSubInfoStruct.miSubjectId +
                   // " , '" + StringUtility.ReplaceSingleQuoteInString(moTeacherSubInfoStruct.msComments, true) + "' " +
                    " ,  " + moTeacherSubInfoStruct.miInsertedById +
                    " ,  " + moTeacherSubInfoStruct.miUpdatedById +
            " ) ";

            return sInsertStatement;
        }

        public DataTable FetchSubjectDetailsForTeacherId(int aiTeacherId)
        {
            string sFetchSubjectsDetails = " SELECT " +
                                               " Teacher_Subject_Details.Subject_Id " +
                                               " , Subject_Master.Subject_Name " +
                                               " , Subject_Master.Original_Subject_Id " +                                          
                                               " , Subject_Master.School_Id " +
                                           " FROM  " +
                                               " Subject_Master " +
                                           " INNER JOIN " +
                                                " Teacher_Subject_Details " +
                                                " ON Subject_Master.Subject_Id = Teacher_Subject_Details.Subject_Id " +
                                           " INNER JOIN " +
                                               " vw_BaseTeacherDetails " +
                                               " ON Teacher_Subject_Details.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id " +
                                           " WHERE " +
                                             " Teacher_Subject_Details.Teacher_Id = " + aiTeacherId +
                                             " AND Teacher_Subject_Details.Is_Deleted =N'" + Constants.C_NO + "'" +
                                             " AND Subject_Master.Is_Deleted =N'" + Constants.C_NO + "'";
          using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
               return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchSubjectsDetails);
        }


        public DataTable FetchSubjectDetailsForEditDetails(int aiTeacherId, int aiSchoolId, int aiAcademicYearId)
        {
            string sFetchSubjectsDetails = " SELECT " +
                                          //" Teacher_Subject_Details.Comments " +
                                       " Teacher_Subject_Details.Subject_Id " +
                                       ",Subject_Master.Original_Subject_Id " +
                                       ",Teacher_Subject_Details.Teacher_Id " +
                                       ",Subject_Master.Subject_Name " +
                                  " FROM " +
                                       " Teacher_Subject_Details " +
                                  " INNER JOIN " +
                                       " Subject_Master " +
                                  " ON Teacher_Subject_Details.Subject_Id = Subject_Master.Subject_Id " +
                                  " WHERE " +
                                      " Teacher_Subject_Details.Teacher_Id =" + aiTeacherId +
                                       " AND Subject_Master.Academic_Year_Id = " + aiAcademicYearId +
                                       " AND Teacher_Subject_Details.Is_Deleted =N'" + Constants.C_NO + "'" +
                                       " AND Subject_Master.Is_Deleted =N'" + Constants.C_NO + "'" +
                           " UNION " +
                              " SELECT  " +                      
                              " Subject_Id " +
                              ",Original_Subject_Id " +
                              ",'0000' As Teacher_Id " +
                              ",Subject_Master.Subject_Name " +
                           " FROM " +
                              " Subject_master " +
                           " WHERE " +
                               " School_Id = " + aiSchoolId + //is NULL  " +
                               " AND Academic_Year_Id = " + aiAcademicYearId +
                               " AND Subject_Master.Is_Deleted =N'" + Constants.C_NO + "'" +
                               " AND Original_Subject_Id NOT IN (" +
                                                         " SELECT " +
                                                             " Subject_Master.Original_Subject_Id " +
                                                         " FROM " +
                                                             " Teacher_Subject_Details " +
                                                         " INNER JOIN " +
                                                             " Subject_Master " +
                                                         " ON Teacher_Subject_Details.Subject_Id = Subject_Master.Subject_Id " +
                                                         " WHERE " +
                                                             " Teacher_Subject_Details.Teacher_Id =" + aiTeacherId +
                                                             " AND Teacher_Subject_Details.Is_Deleted =N'" + Constants.C_NO + "'" +
                                                             " AND Subject_Master.Is_Deleted =N'" + Constants.C_NO + "')" +
                            " ORDER BY  Subject_Master.Original_Subject_Id ";
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
               return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchSubjectsDetails);
           
        }

        public ArrayList GetAllSubjectsForTeacher(int aiTeacherId)
        {
            ArrayList arrayReturn = new ArrayList();
            string sQuery = " SELECT Subject_Id " +
                            //",Comments " +
                            " FROM Teacher_Subject_Details " +                             
                            " WHERE Teacher_Id = N'" + aiTeacherId + "'" +
                            //" AND Standard_Id = '" + aiStandardId + "'" +
                            " AND Is_Deleted = N'" + Constants.C_NO + "'";
            DataTable oTable;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oTable = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);

            if (oTable.Rows.Count > 0)
            {
                int iCnt = oTable.Rows.Count;
                for (int i = 0; i < iCnt; i++)
                {
                    if (oTable.Rows[0]["Subject_Id"] != DBNull.Value)
                        arrayReturn.Add(oTable.Rows[i]["Subject_Id"]);
                }
            }
            return arrayReturn;
        }



        //public string InsertTeacherSubjectDetails()
        //{
        //    string sValue;

        //    string sInsertQuery = GetInsertStatement();
        //    string sReturnvalue = using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                //oSQLServerDbUtility.ExecuteTransaction(sInsertQuery);
        //    if (sReturnvalue.StartsWith("OK"))
        //    {
        //        sValue = sReturnvalue.Remove(0, 3);
        //    }
        //    else
        //        sValue = "0";

        //    return sValue;
        //}

        /// <summary>
        ///  This method is used to get teacher associated subjects.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiStandardId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static List<SubjectMaster> GetTeacherAssociatedSubjects(int aiTeacherId, int aiStandardId, int aiAcademicYearId, int aiSchoolId, bool abConsiderSubjectSections)
        {
            List<SubjectMaster> lstSubjectMaster = new List<SubjectMaster>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYEarId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacehrId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ConsiderSubjectSections", abConsiderSubjectSections, SqlDbType.Bit);

                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetTeacherAssociatedSubjects]"))
                {
                    if (oSqlDataReader != null)
                    {
                        while (oSqlDataReader.Read())
                        {
                            SubjectMaster oSubjectMaster = new SubjectMaster();
                            if (oSqlDataReader["SchoolWise_Standard_Subject_Id"] != DBNull.Value)
                                oSubjectMaster.StandardwiseSubjectId = Convert.ToInt32(oSqlDataReader["SchoolWise_Standard_Subject_Id"]);
                            if (oSqlDataReader["Subject_Name"] != DBNull.Value)
                                oSubjectMaster.SubjectName = Convert.ToString(oSqlDataReader["Subject_Name"]);
                            lstSubjectMaster.Add(oSubjectMaster);
                        }
                    }
                }
            }
            return lstSubjectMaster;
        }

        #endregion

    }

    public class TeacherSubjectDetailsCollectionDC : DataCommunicatorBaseDC
    {
        public TeacherSubjectDetailsCollectionDC()
        {
        }

        public bool DeleteTeacherSubjectDetails(ArrayList aoArrDeleteTeacherIds)
        {
            string sDeleteTeacherIdList = "(";
            for (int iCount = 0; iCount < aoArrDeleteTeacherIds.Count; iCount++)
            {
                sDeleteTeacherIdList = sDeleteTeacherIdList + aoArrDeleteTeacherIds[iCount];
                sDeleteTeacherIdList = sDeleteTeacherIdList + ",";
            }
            sDeleteTeacherIdList = sDeleteTeacherIdList + ")";
            sDeleteTeacherIdList = sDeleteTeacherIdList.Remove(sDeleteTeacherIdList.Length - 2, 1);

            string sSqlDeleteEducationDetails = " UPDATE Teacher_Subject_Details " +
                                " SET Is_Deleted =N'" + Utility.Constants.C_YES + "'" +
                                " WHERE Teacher_Id in " + sDeleteTeacherIdList;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteEducationDetails);
            return true;
        }

        public bool DeleteTeacherSubjectDetails(int aiTeacherId)
        {
           
            string sSqlDeleteEducationDetails = " UPDATE Teacher_Subject_Details " +
                                " SET Is_Deleted =N'" + Utility.Constants.C_YES + "'" +
                                " WHERE Teacher_Id = " + aiTeacherId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteEducationDetails);
            return true;
        }

        public static string RemoveAllSubjectsForTeacherId(int aiTeacherId)
        {
            // This procedure accepts parameter as asBusinessId. This method logically deletes all the 
            // locations for the passed businessid from the database.
            string sDeleteStatement;

            sDeleteStatement = " DELETE Teacher_Subject_Details " +
                               " WHERE " +
                                   " teacher_id in (" + aiTeacherId + ")";

           return sDeleteStatement;
        }
    }
}
