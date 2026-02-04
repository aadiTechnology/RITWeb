using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;


namespace DataCommunicator
{
    public class CancellationFormDC : DataCommunicatorBaseDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;

        #endregion

        #region Constructor(s)

        public CancellationFormDC(int aiSchoolId, int aiUserId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUserId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        public CancellationFormDC()
        {
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to delete details.
        /// </summary>
        /// <param name="aiId"></param>
        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteCancellationFormDetails");
            }
        }

        /// <summary>
        /// This method is used to get search student 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asFilter"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<SearchStudentDetails> GetAllSearchStudents(int aiSchoolId, int aiAcademicYearId, string asFilter, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSearchedStudentDetailsForCancellationForm"))
                {
                    List<SearchStudentDetails> lstSearchStudentDetails = new List<SearchStudentDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstSearchStudentDetails.Add(new SearchStudentDetails
                        {
                            SchoolWiseStudentId = Convert.ToInt32(oSqlDataReader["SchoolWise_Student_Id"]),
                            EnrolmentNumber = Convert.ToString(oSqlDataReader["Enrolment_Number"]),
                            RollNo = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            ClassName = Convert.ToString(oSqlDataReader["ClassName"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"])
                        });
                    }
                    return lstSearchStudentDetails;
                }
            }
        }

        /// <summary>
        /// This method is used to get student details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <returns></returns>
        public List<CancellationFormStudentDetails> GetAllStudents(int aiSchoolId, int aiAcademicYearId, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetailsForCancellationForm"))
                {
                    List<CancellationFormStudentDetails> lstCancellationStudents = new List<CancellationFormStudentDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstCancellationStudents.Add(new CancellationFormStudentDetails
                        {
                            Enrolment_Number = Convert.ToString(oSqlDataReader["Enrolment_Number"]),
                            Roll_No = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            ClassName = Convert.ToString(oSqlDataReader["ClassName"]),
                            StudentName = Convert.ToString(oSqlDataReader["StudentName"]),
                            TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]),
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            SchoolWiseStudentId = Convert.ToInt32(oSqlDataReader["SchoolWise_Student_Id"]),
                            StandardId = Convert.ToInt32(oSqlDataReader["Standard_Id"]),
                            DivisionId = Convert.ToInt32(oSqlDataReader["Division_Id"]),
                            StudentId = Convert.ToInt32(oSqlDataReader["Student_Id"]),
                            SubmittedBy = Convert.ToInt32(oSqlDataReader["SubmittedBy"]),
                        });
                    }
                    return lstCancellationStudents;
                }
            }
        }

        /// <summary>
        /// This method is used to save details.
        /// </summary>
        /// <param name="oCancellationForm"></param>
        public void Save(CancellationForm oCancellationForm)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Reason", oCancellationForm.Reason, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RefundChequeInFavoutOf", oCancellationForm.RefundChequeInFavourOf, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Cell", oCancellationForm.Cell, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", oCancellationForm.SchoolWiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", oCancellationForm.Id, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveCancellationFormDetails");
            }
        }

        /// <summary>
        /// This method is used to return details.
        /// </summary>
        /// <param name="aiId"></param>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <returns></returns>
        public CancellationForm Get(int aiId, int aiSchoolwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                CancellationForm oCancellationForm = new CancellationForm();
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetControlDetailsForCancellationForm"))
                {
                    if (oSqlDataReader.Read())
                    {
                        oCancellationForm.SchoolWiseStudentId = oSqlDataReader["SchoolWise_Student_Id"].ToInt();
                        oCancellationForm.Reason = oSqlDataReader["Reason"].ToString();
                        oCancellationForm.RefundChequeInFavourOf = oSqlDataReader["RefundChequeInFavourOf"].ToString();
                        oCancellationForm.Cell = oSqlDataReader["Cell"].ToString();
                        oCancellationForm.StudentName = oSqlDataReader["StudentName"].ToString();
                        oCancellationForm.Id = oSqlDataReader["Id"].ToInt();
                    }
                }
                return oCancellationForm;
            }
        }

        /// <summary>
        /// This method is used to get control details.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiId"></param>
        /// <returns></returns>
        public CancellationForm GetControlDetails(int aiSchoolwiseStudentId, int aiId)
         {
             using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
             {
                 CancellationForm oCancellationForm = new CancellationForm();
                 oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                 oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                 
                 using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetControlDetailsForCancellationForm"))
                 {
                     if (oSqlDataReader.Read())
                     {
                         oCancellationForm.SchoolWiseStudentId = oSqlDataReader["SchoolWise_Student_Id"].ToInt();
                         oCancellationForm.Reason = oSqlDataReader["Reason"].ToString();
                         oCancellationForm.RefundChequeInFavourOf = oSqlDataReader["RefundChequeInFavourOf"].ToString();
                         oCancellationForm.Cell = oSqlDataReader["Cell"].ToString();
                         oCancellationForm.StudentName = oSqlDataReader["StudentName"].ToString();
                         oCancellationForm.Id = oSqlDataReader["Id"].ToInt();
                     }
                 }
                 return oCancellationForm;
             }
         }

        public void ApplyConcessionFormFee(int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ApplyConcessionFormFee");
            }
        }

        #endregion
    }
}
