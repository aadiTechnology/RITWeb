using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DataCommunicator
{
    public class StudentRecordDC
    {
        #region Data MEmber(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miUserId; 

        #endregion

        #region Constructor(s)

        public StudentRecordDC()
        {
        }

        public StudentRecordDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUserId = aiUserId;
        } 

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to return student record details.
        /// </summary>
        /// <param name="aiSchoowiseStudentId"></param>
        /// <param name="abIsReadMode"></param>
        /// <returns></returns>
        public StudentDataCollction GetAllStudentRecords(int aiSchoowiseStudentId, bool abIsReadMode)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                StudentDataCollction oStudentDataCollction = new StudentDataCollction();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoowiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsReadMode", abIsReadMode, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("UserId", this.miUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentRecordData"))
                {
                    LoadStudentBasicDetails(oStudentDataCollction, oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadSiblingDetails(oStudentDataCollction, oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadSections(oStudentDataCollction, oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadParameters(oStudentDataCollction, oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadStudentRecord(oStudentDataCollction, oSqlDataReader);

                    oSqlDataReader.NextResult();
                    LoadStudentComments(oStudentDataCollction, oSqlDataReader);
                }
                return oStudentDataCollction;
            }
        }

        /// <summary>
        /// This method is used to save student record.
        /// </summary>
        /// <param name="aiStudentId"></param>
        /// <param name="asData"></param>
        public void Save(int aiStudentId, string asData, DateTime adtDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DataXML", asData, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("Date", adtDate, SqlDbType.DateTime);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentRecord");
            }
        }

        /// <summary>
        /// This method is used to get comment details.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiCommentId"></param>
        /// <returns></returns>
        public StudentRecordComment GetCommentDetails(int aiSchoolwiseStudentId, int aiCommentId)
        {
            StudentRecordComment StudentRecordComments = new StudentRecordComment();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CommentId", aiCommentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentRecordComment"))
                {
                    if (oSqlDataReader.Read())
                    {
                        StudentRecordComments.Date = oSqlDataReader["Date"].ToDateTime();
                        StudentRecordComments.Comment = oSqlDataReader["Comment"].ToString();
                        StudentRecordComments.LectureName = oSqlDataReader["LectureName"].ToString();                        
                    }
                }
            }
            return StudentRecordComments;
        }

        /// <summary>
        /// This method is used to save comment.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiCommentId"></param>
        /// <param name="aoStudentRecordCommnet"></param>
        /// <param name="abAllowSubmit"></param>
        public void SaveComment(int aiSchoolwiseStudentId, int aiCommentId, StudentRecordComment aoStudentRecordCommnet, bool abAllowSubmit, int aiStdDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CommentId", aiCommentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", aoStudentRecordCommnet.Date, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("Comment", aoStudentRecordCommnet.Comment, SqlDbType.NVarChar, ParameterDirection.Input, 500);
                oSQLServerDbUtility.AddParameter("LectureName", aoStudentRecordCommnet.LectureName, SqlDbType.NVarChar);                
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsDeleteAction", false, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AllowSubmit", abAllowSubmit, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentRecordComment");
            }
        }

        /// <summary>
        /// This method is used to delete comment
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiCommentId"></param>
        public void DeleteComment(int aiSchoolwiseStudentId, int aiCommentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CommentId", aiCommentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsDeleteAction", true, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AllowSubmit", false, SqlDbType.Bit);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveStudentRecordComment");
            }
        }

        /// <summary>
        /// This method is used to submit comment.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        /// <param name="aiCommentId"></param>
        /// <param name="abSubmitAllComments"></param>
        public void Submit(int aiSchoolwiseStudentId, int aiCommentId, bool abSubmitAllComments)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CommentId", aiCommentId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SubmitAllComments", abSubmitAllComments, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("AcademicYearId",this.miAcademicYearId,SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SubmitStudentRecordComment");
            }
        }

        /// <summary>
        /// This method is used to  return status.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStdDivId"></param>
        /// <param name="asFilter"></param>
        /// <param name="abShowSaved"></param>
        /// <param name="asHasEditAccess"></param>
        /// <param name="aiUserId"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public List<StudentRecordStatus> GetAllStudentStatus(int aiSchoolId, int aiAcademicYearId, int aiStdDivId, string asFilter, bool abShowSaved, bool abIncludeRiseAndShinde, string asHasEditAccess, int aiUserId, string sortExpression, string sortDirection, int startRowIndex, int iEndIndex)
        {
            List<StudentRecordStatus> lstStudentRecordStatus = new List<StudentRecordStatus>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sortExpression", sortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("sortDirection", sortDirection, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowSaved", abShowSaved, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IncludeRiseAndShine", abIncludeRiseAndShinde, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("HasEditAccess", asHasEditAccess, SqlDbType.Char);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllStudentsForRecordStatus"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstStudentRecordStatus.Add
                            (
                               new StudentRecordStatus
                                {
                                    Class = oSqlDataReader["Class"].ToString(),
                                    IsRecordFound = oSqlDataReader["IsRecordFound"].ToBool(),
                                    Name = oSqlDataReader["Name"].ToString(),
                                    RegNo = oSqlDataReader["RegNo"].ToString(),
                                    RollNo = oSqlDataReader["RollNo"].ToInt(),
                                    SchoolwiseStudentId = oSqlDataReader["SchoolwiseStudentId"].ToInt(),
                                    TotalRows = oSqlDataReader["TotalRows"].ToInt(),
                                    ReadyToReadCount = oSqlDataReader["ReadyToReadCount"].ToInt(),
                                    ReadyToSubmitCount = oSqlDataReader["ReadyToSubmitCount"].ToInt()
                                    //IsReadByPrincipal = oSqlDataReader["IsReadByPrincipal"].ToBool(),
                                    //IsReadByCounsellor = oSqlDataReader["IsReadByCounsellor"].ToBool(),
                                    //IsSubmitted = oSqlDataReader["IsSubmitted"].ToBool(),
                                    //PrincipalCommentCount = oSqlDataReader["PrincipalUnreadCommentCount"].ToInt(),
                                    //CouncellorCommentCount = oSqlDataReader["CouncellorUnreadCommentCount"].ToInt()
                                }
                            );
                    }

                    return lstStudentRecordStatus;

                }
            }
        }

        /// <summary>
        /// This method is used to return teacher list.
        /// </summary>
        /// <param name="abHasFullAccess"></param>
        /// <returns></returns>
        public Tuple<bool, bool, int, List<AssociatedTeacher>, bool> GetTeacherList(bool abHasFullAccess)
        {
            int iAssociatedClassId = -1;
            bool bIsPrincipal = false, bIsCounsellor = false, bIsSubjectTeacher = false;
            List<AssociatedTeacher> lstTeachers = new List<AssociatedTeacher>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("HasFullAccess", abHasFullAccess, SqlDbType.Bit);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetTeacherList"))
                {

                    while (oSqlDataReader.Read())
                    {
                        lstTeachers.Add
                            (
                                new AssociatedTeacher
                                {
                                    StdDivId = oSqlDataReader["StdDivId"].ToInt(),
                                    TeacherName = oSqlDataReader["TeacherName"].ToString()
                                }
                            );
                    }

                    oSqlDataReader.NextResult();
                    if (oSqlDataReader.Read())
                    {
                        iAssociatedClassId = oSqlDataReader["AssociatedClassId"].ToInt();
                        bIsPrincipal = oSqlDataReader["IsPrincipal"].ToBool();
                        bIsCounsellor = oSqlDataReader["IsCounsellor"].ToBool();
                        bIsSubjectTeacher = oSqlDataReader["IsSubjectTeacher"].ToBool();
                    }
                }
            }
            return new Tuple<bool, bool, int, List<AssociatedTeacher>, bool>(bIsPrincipal, bIsCounsellor, iAssociatedClassId, lstTeachers, bIsSubjectTeacher);
        }

        /// <summary>
        /// This method is used to mark comment as read.
        /// </summary>
        /// <param name="aiSchoolwiseStudentId"></param>
        public void MarkAsRead(int aiSchoolwiseStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", this.miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolwiseStudentId", aiSchoolwiseStudentId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_MarkRecordAsRead");
            }
        } 

        #endregion

        #region Private method(s)

        /// <summary>
        /// This method is used to load student comments.
        /// </summary>
        /// <param name="aoStudentDataCollction"></param>
        /// <param name="aoSqlDataReader"></param>
        private static void LoadStudentComments(StudentDataCollction aoStudentDataCollction, SqlDataReader aoSqlDataReader)
        {
            aoStudentDataCollction.StudentRecordComments = new List<StudentRecordComment>();
            while (aoSqlDataReader.Read())
            {
                aoStudentDataCollction.StudentRecordComments.Add
                    (
                        new StudentRecordComment
                        {
                            Id = aoSqlDataReader["Id"].ToInt(),
                            Date = aoSqlDataReader["Date"].ToDateTime(),
                            Comment = aoSqlDataReader["Comment"].ToString(),
                            LectureName = aoSqlDataReader["LectureName"].ToString(),
                            IsCommentReadByConsellor = aoSqlDataReader["IsCommentReadByConsellor"].ToBool(),
                            IsDefaultComment = aoSqlDataReader["IsDefaultComment"].ToBool(),
                            IsSubmitted = aoSqlDataReader["IsSubmitted"].ToBool(),
                            IsCommentReadByPrincipal = aoSqlDataReader["IsCommentReadByPrincipal"].ToBool(),
                            IsCommentReadByClassTeacher = aoSqlDataReader["IsCommentReadByClassTeacher"].ToBool(),
                            LoginUserDesignation = aoSqlDataReader["LoginUserDesignation"].ToInt(),
                            InsertedById = aoSqlDataReader["InsertedById"].ToInt(),
                             UserName=aoSqlDataReader["UserName"].ToString()
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to student record.
        /// </summary>
        /// <param name="aoStudentDataCollction"></param>
        /// <param name="aoSqlDataReader"></param>
        private static void LoadStudentRecord(StudentDataCollction aoStudentDataCollction, SqlDataReader aoSqlDataReader)
        {
            aoStudentDataCollction.StudentRecords = new List<StudentRecord>();
            while (aoSqlDataReader.Read())
            {
                aoStudentDataCollction.StudentRecords.Add
                    (
                        new StudentRecord
                        {
                            Id = aoSqlDataReader["Id"].ToInt(),
                            ParameterId = aoSqlDataReader["ParameterId"].ToInt(),
                            Answer = aoSqlDataReader["Answer"].ToString()
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to load parameters
        /// </summary>
        /// <param name="aoStudentDataCollction"></param>
        /// <param name="aoSqlDataReader"></param>
        private static void LoadParameters(StudentDataCollction aoStudentDataCollction, SqlDataReader aoSqlDataReader)
        {
            aoStudentDataCollction.StudentRecordParameters = new List<StudentRecordParameter>();
            while (aoSqlDataReader.Read())
            {
                aoStudentDataCollction.StudentRecordParameters.Add
                    (
                        new StudentRecordParameter
                        {
                            Id = aoSqlDataReader["Id"].ToInt(),
                            SectionId = aoSqlDataReader["SectionId"].ToInt(),
                            ControlId = aoSqlDataReader["ControlId"].ToInt(),
                            Name = aoSqlDataReader["Name"].ToString(),
                            SortOrder = aoSqlDataReader["SortOrder"].ToInt()
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to load sections.
        /// </summary>
        /// <param name="aoStudentDataCollction"></param>
        /// <param name="aoSqlDataReader"></param>
        private static void LoadSections(StudentDataCollction aoStudentDataCollction, SqlDataReader aoSqlDataReader)
        {
            aoStudentDataCollction.StudentRecordSections = new List<StudentRecordSection>();
            while (aoSqlDataReader.Read())
            {
                aoStudentDataCollction.StudentRecordSections.Add
                    (
                        new StudentRecordSection
                        {
                            Id = aoSqlDataReader["Id"].ToInt(),
                            DisplayOnScreen = aoSqlDataReader["DisplayOnScreen"].ToBool(),
                            Name = aoSqlDataReader["Name"].ToString(),
                            SortOrder = aoSqlDataReader["SortOrder"].ToInt()
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to load sibling details.
        /// </summary>
        /// <param name="aoStudentDataCollction"></param>
        /// <param name="aoSqlDataReader"></param>
        private static void LoadSiblingDetails(StudentDataCollction aoStudentDataCollction, SqlDataReader aoSqlDataReader)
        {
            aoStudentDataCollction.StudentRecordSiblings = new List<StudentRecordSibling>();
            while (aoSqlDataReader.Read())
            {
                aoStudentDataCollction.StudentRecordSiblings.Add
                    (
                        new StudentRecordSibling
                        {
                            Age = aoSqlDataReader["Age"].ToInt(),
                            Sex = Convert.ToChar(aoSqlDataReader["Sex"]),
                            SiblingName = aoSqlDataReader["SiblingName"].ToString(),
                            Standard = aoSqlDataReader["Standard"].ToString()
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to load student basic details.
        /// </summary>
        /// <param name="aoStudentDataCollction"></param>
        /// <param name="aoSqlDataReader"></param>
        private static void LoadStudentBasicDetails(StudentDataCollction aoStudentDataCollction, SqlDataReader aoSqlDataReader)
        {
            if (aoSqlDataReader.Read())
            {
                aoStudentDataCollction.StudentBasicInformation = new StudentBasicInfo
                {
                    DOB = aoSqlDataReader["DOB"].ToDateTime(),
                    FatherName = aoSqlDataReader["FatherName"].ToString(),
                    MotherName = aoSqlDataReader["MotherName"].ToString(),
                    FatherOccupation = aoSqlDataReader["FatherOccupation"].ToString(),
                    MotherOccupation = aoSqlDataReader["MotherOccupation"].ToString(),
                    StudentName = aoSqlDataReader["StudentName"].ToString()
                };
            }
        }
        
        #endregion
    }
}
