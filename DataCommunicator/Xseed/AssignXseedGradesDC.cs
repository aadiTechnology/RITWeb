//------------------------------------------------------------------------------------------------------------------------------
// Class Name       :- AssignXseedGradesDC
// Purpose          :- This class is used to manage Edit,Submit grades of all selected subjects of the selected subject teachers.
// Date Of creation :- 6/01/2011
// Author Name      :- Shobha Patil.
//------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Utility;
using XseedReportEntities;

namespace DataCommunicator
{
    public class AssignXseedGradesDC
    {

        #region "DATA MEMBERS"

        public GradeSubmitStatus moGradeSubmitEntity = null;
        public XseedResultPublishStatus moXseedResultPublishStatus;
        
        #endregion

        #region "PROPERTIES"
       
        public GradeSubmitStatus GradeSubmitEntity
        {
            get { return moGradeSubmitEntity; }
            set { moGradeSubmitEntity = value; }
        }

        #endregion

        #region "CONSTRUCTORS"

        public AssignXseedGradesDC()
        {
        }

        #endregion

        #region "PUBLIC METHODS"

        /// <summary>
        /// This method is used to get the assessment details to fill the Assessment combobox.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public static List<AssessmentMaster> GetAssessments(int aiSchoolId,int aiAcademicYearId)
        {
            List<AssessmentMaster> lstAssessmentMaster = new List<AssessmentMaster>();
            string sSelect= " SELECT AssessmentId,Name "+
                            " FROM Xseed.AssessmentMaster "+
                            " WHERE Is_Deleted=N'"+Constants.C_NO+ "'"+ 
                            " AND Academic_Year_Id="+aiAcademicYearId+
                            " AND SchoolId="+aiSchoolId +
                            " AND AssessmentId IN(SELECT AssessmentId FROM Xseed.StandardwiseAssessmentMaster "+
                                                    " WHERE Is_Deleted=N'"+Constants.C_NO+ "'"+
                            " AND Academic_Year_Id="+aiAcademicYearId+
                            " AND SchoolId=" +aiSchoolId + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelect))
                {
                    AssessmentMaster oAssessmentMaster;
                    while (oSqlDataReader.Read())
                    {
                        oAssessmentMaster = new AssessmentMaster
                        {
                            AssessmentId = Convert.ToInt32(oSqlDataReader["AssessmentId"]),
                            Name = Convert.ToString(oSqlDataReader["Name"]),
                        };
                        lstAssessmentMaster.Add(oAssessmentMaster);
                    }
                }
            }
            return lstAssessmentMaster;
        }

        /// <summary>
        /// This stored procedure is used to get the all subject details with edit or submit grade status of the seleted teacher and assessment.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiAsseessmentId"></param>
        /// <returns></returns>
        public static List<XseedGradesStatus> GetTeacherSubjectDetails(int aiTeacherId, int aiSchoolId, int aiAcademicYearId, int aiAsseessmentId)
        {
            List<XseedGradesStatus> lstXseedGradesStatus=new List<XseedGradesStatus>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("TeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", aiAsseessmentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetTeacherXseedSubjects]"))
                {
                    XseedGradesStatus oXseedGradesStatus;
                    while (oSqlDataReader.Read())
                    {
                        oXseedGradesStatus = new XseedGradesStatus
                        {
                            Class = oSqlDataReader["StandardDivision"].ToString(),
                            StandardDivisionID = Convert.ToInt32(oSqlDataReader["StandardDivisionID"]),
                            SubjectId = Convert.ToInt32(oSqlDataReader["SubjectId"]),
                            SubjectName = Convert.ToString(oSqlDataReader["Subject_Name"]),
                            EditStatus = Convert.ToInt32(oSqlDataReader["EditStatus"]),
                            SubmitStatus = Convert.ToInt32(oSqlDataReader["SubmitStatus"]),
                            IsXseedSubject = Convert.ToChar(oSqlDataReader["IsXseedSubject"]),
                            IncompleteRollNo = oSqlDataReader["IncompleteRollNoString"].ToString()
                        };
                        lstXseedGradesStatus.Add(oXseedGradesStatus);
                    }
                }
            }
            return lstXseedGradesStatus;
        }

        /// <summary>
        /// This method is used to submit the assigned grades to class teachers. 
        /// </summary>
        public void Submit()
        {
            string sInsertStatement = "INSERT INTO [Xseed].[GradeSubmitStatus]" +
                            " ([StandardDivisionId]" +
                            " ,[AssessmentId]" +
                            " ,[SubjectId]" +
                            " ,[IsSubmitted]" +
                            " ,[IsPublished]" +
                            " ,[Academic_Year_Id]" +
                            " ,[SchoolId]" +
                            " ,[InsertedById]" +
                            " ,[InsertDate]" +
                            " )VALUES(" +
                            " " + moGradeSubmitEntity.StandardDivisionId +
                            " ," + moGradeSubmitEntity.AssessmentId +
                            " ," + moGradeSubmitEntity.SubjectId +
                            " ,1, 0" +
                            " ," + moGradeSubmitEntity.AcademicYearId +
                            " ," + moGradeSubmitEntity.SchoolId +
                            " ," + moGradeSubmitEntity.InsertedById +
                            " ,N'" + DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI) + "')";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                 oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        /// <summary>
        /// This stored procedure is used to get the all subject details of the selected class teacher and assessment.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAssessmentId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public List<XseedGradesStatus> GetClassTeacherSubjects(int aiStdDivId, int aiSchoolId, int aiAssessmentId, int aiAcademicYearId)
        {
            List<XseedGradesStatus> lstXseedGradesStatus = new List<XseedGradesStatus>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssessmentId", aiAssessmentId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Xseed].[usp_GetClassTeacherXseedSubjects]"))
                {
                    XseedGradesStatus oXseedGradesStatus;

                    while (oSqlDataReader.Read())
                    {
                        oXseedGradesStatus = new XseedGradesStatus
                        {
                            StandardDivisionID = Convert.ToInt32(oSqlDataReader["StandardDivisionID"]),
                            SubjectId = Convert.ToInt32(oSqlDataReader["SubjectId"]),
                            SubjectName = Convert.ToString(oSqlDataReader["Subject_Name"]),
                            EditStatus = Convert.ToChar(oSqlDataReader["EditStatus"]),
                            IsXseedSubject = Convert.ToChar(oSqlDataReader["IsXseedSubject"]),
                            IsSubmitted = Convert.ToChar(oSqlDataReader["IsPublished"]),
                        };
                        lstXseedGradesStatus.Add(oXseedGradesStatus);
                    }
                    if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                    {
                        moXseedResultPublishStatus = new XseedResultPublishStatus
                        {
                            StandardDivisionId = Convert.ToInt32(oSqlDataReader["StandardDivisionID"]),
                            PublishStatus = Convert.ToChar(oSqlDataReader["PublishStatus"]),
                            IsPublished = Convert.ToChar(oSqlDataReader["IsPublished"]),
                        };
                    }
                }
            }
            return lstXseedGradesStatus;
        }

        /// <summary>
        /// This method is used to publish the assigned grades to class teachers. 
        /// </summary>
        public void Publish()
        {
            string sInsertStatement = "INSERT INTO [Xseed].[XseedResultPublishStatus]" +
                            " ([StandardDivisionId]" +
                            " ,[AssessmentId]" +
                            " ,[IsPublished]" +
                            " ,[Academic_Year_Id]" +
                            " ,[SchoolId]" +
                            " ,[InsertedById]" +
                            " ,[InsertDate]" +
                            " )VALUES(" +
                            " " + moGradeSubmitEntity.StandardDivisionId +
                            " ," + moGradeSubmitEntity.AssessmentId +
                            " , 1" +
                            " ," + moGradeSubmitEntity.AcademicYearId +
                            " ," + moGradeSubmitEntity.SchoolId +
                            " ," + moGradeSubmitEntity.InsertedById +
                            " ,N'" + DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI) + "')";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
        }

        /// <summary>
        /// This method is used to get the class teacher for which the assessments are assigned to fill the class teacher combobox.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
		public static List<ClassTeacherDetails> GetClassTeachers(int aiSchoolId, int aiAcademicYearId)
        {
			List<ClassTeacherDetails> lstClassTeachers = new List<ClassTeacherDetails>();
            string sSelect = " SELECT DISTINCT "+
					        " Standard_Name + N'-' + Division_Name + N' : ' + TeacherName AS TeacherName"+
                            " , Teacher_Id, Designation_Id, Teacher_First_Name"+
                            " , Original_Standard_Id, Original_Division_Id "+
							" , SchoolWise_Standard_Division_Id"+
	                        " FROM vw_ClassTeacher "+
	                        " WHERE vw_ClassTeacher.Standard_Id IN (SELECT Standard_Id "+
                                                                    " FROM Xseed.StandardwiseAssessmentMaster "+
                                                                    " WHERE Is_Deleted=N'" + Constants.C_NO + "' "+
                                                                    " AND academic_Year_Id="+aiAcademicYearId+ " AND SchoolId="+aiSchoolId+" )" +
                            " AND Is_Deleted=N'" + Constants.C_NO + "'" +
                            " AND Academic_Year_Id=" + aiAcademicYearId +
                            " AND School_Id=" + aiSchoolId +
                            " AND Is_ClassTeacher=N'" + Constants.C_YES + "'" +
                            " ORDER BY Original_Standard_Id, Original_Division_Id, Designation_Id, Teacher_First_Name";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelect))
                {
                    ClassTeacherDetails oClassTeachers;
                    while (oSqlDataReader.Read())
                    {
                        oClassTeachers = new ClassTeacherDetails
                        {
                            TeacherId = Convert.ToInt32(oSqlDataReader["Teacher_Id"]),
                            TeacherName = Convert.ToString(oSqlDataReader["TeacherName"]),
                            StandardDivisionId = Convert.ToInt32(oSqlDataReader["SchoolWise_Standard_Division_Id"]),
                        };
                        lstClassTeachers.Add(oClassTeachers);
                    }
                }
            }
            return lstClassTeachers;
        }

        /// <summary>
        /// This method is used to to publish all the Xseed Result.
        /// </summary>
        /// <param name="aiStandardDivId"></param>
        /// <param name="aiAssessmentId"></param>
        /// <param name="aiAcademicYrID"></param>
        /// <param name="aiSchoolID"></param>
        /// <param name="asUnpublishReason"></param>
        /// <param name="aiUpdatedId"></param>
        public static void Unpublish(int aiStandardDivId, int aiAssessmentId, int aiAcademicYrID, int aiSchoolID, string asUnpublishReason, int aiUpdatedId)
        {
            string sUpdateStatement = "UPDATE [Xseed].[XseedResultPublishStatus] SET " +
                                     " UnPublishReason= N'" + StringUtility.ReplaceSingleQuoteInString(asUnpublishReason, false) + "' " +
                                     ", IsPublished=0"+
                                     ", UpdatedById= " + aiUpdatedId +
                                     ", UpdateDate= N'" + DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                                     " WHERE StandardDivisionId=" + aiStandardDivId +
                                     " AND AssessmentId=" +aiAssessmentId+
                                     " AND SchoolId = " + aiSchoolID +
                                     " AND Academic_Year_Id = " + aiAcademicYrID;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        #endregion


        public static List<ClassTeacher> GetTeacher(int aiSchoolId, int aiAcademicYearId)
        {
            List<ClassTeacher> lstClassTeachers = new List<ClassTeacher>();

            string sSelect = " SELECT " +
                                       " DISTINCT Teacher_Id " +
                                       ", TeacherName AS TeacherName" +
                                       ", Designation_Id " +
                                       ",Teacher_First_Name " +
                                       ",VTD.DesignationSortOrder,Teacher_Middle_Name, Teacher_Last_Name" +
                                       " FROM vw_Get_Subject_Assigned_TeacherName" +
                                       " LEFT OUTER JOIN vw_TeacherDesignations VTD"+
					                   " ON vw_Get_Subject_Assigned_TeacherName.Designation_Id = VTD.Teacher_Designation_Id"+
                            " WHERE vw_Get_Subject_Assigned_TeacherName.Standard_Id IN (SELECT Standard_Id " +
                                                                    " FROM Xseed.StandardwiseAssessmentMaster " +
                                                                    " WHERE Is_Deleted=N'" + Constants.C_NO + "' " +
                                                                    " AND academic_Year_Id=" + aiAcademicYearId + " AND SchoolId=" + aiSchoolId + " )" +
                            " AND Is_deleted=N'" + Constants.C_NO + "'" +
                            " AND academic_Year_Id=" + aiAcademicYearId +
                            " AND School_Id=" + aiSchoolId +
                            " ORDER BY VTD.DesignationSortOrder, Teacher_First_Name,Teacher_Middle_Name, Teacher_Last_Name";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelect))
                {
                    ClassTeacher oClassTeacher;
                    while (oSqlDataReader.Read())
                    {
                        oClassTeacher = new ClassTeacher
                        {
                            TeacherId = Convert.ToInt32(oSqlDataReader["Teacher_Id"]),
                            TeacherName = Convert.ToString(oSqlDataReader["TeacherName"]),
                        };
                        lstClassTeachers.Add(oClassTeacher);
                    }
                }
            }
            return lstClassTeachers;
        }
    }     
}
