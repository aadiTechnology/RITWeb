using System;
using System.Data;
using System.Collections;
using Utility;

namespace DataCommunicator
{

    public class SubjectMasterDC
    {

        #region Constant and structures

        public struct SubjectMasterStruct
        {
            public int miSubjectId;
            public int miSchoolId;
            public string msSubjectName;
			public string msShortName;
            public int miOriginalSubjectId;
            public string msIsDeleted;
            public int miInsertedByid;
            public int miUpdatedById;
            public int miAcademicyearId;
            public int miStandardDivisionId;
            public bool mbIsCoCurricularActivity;
            public bool mbIsAttitudeSubject;
        }

        public struct SubjectGroupsStruct
        {
            public int miGroupId;
            public int miParentGroupId;
            public int miParentSubjectId;
        }

        public struct StandardSubjectStruct
        {
            public int miStandardId;
        }
        public struct StandardDivisionSubjectStruct
        {
            public int miStandardDivisionId;
        }

        #endregion


        #region DataMembers and properties

        #region Data members

        private SubjectMasterStruct moSubjectMasterStruct;
        private StandardSubjectStruct moStandardSubjectStruct;
        private StandardDivisionSubjectStruct moStandardDivisionSubjectStruct;
        private SubjectGroupsStruct moSubjectGroupsStruct;


        #endregion

        #region Properties

        public SubjectMasterStruct SubjectMasterStructDetails
        {

            get { return moSubjectMasterStruct; }
            set { moSubjectMasterStruct = value; }
        }

        public StandardSubjectStruct StandardSubjectStructDetails
        {

            get { return moStandardSubjectStruct; }
            set { moStandardSubjectStruct = value; }
        }

        public StandardDivisionSubjectStruct StandardDivisionSubjectStructDetails
        {

            get { return moStandardDivisionSubjectStruct; }
            set { moStandardDivisionSubjectStruct = value; }
        }

        public SubjectGroupsStruct SubjectGroupsStructDetails
        {

            get { return moSubjectGroupsStruct; }
            set { moSubjectGroupsStruct = value; }
        }

        #endregion

        #endregion


        #region Constructors

        public SubjectMasterDC()
        {
        }

        #endregion


        #region Public Methods

        #region Subjects

        public string GetInsertStatementForSubject()
        {
            string sInsertStatement = String.Format("INSERT INTO Subject_Master (School_Id, Subject_Name, Short_Name, Original_Subject_Id, Inserted_By_Id, Academic_Year_Id, Is_Deleted, Is_CoCurricularActivity,IsAttitudeSubject) " +
													"VALUES ({0}, N'{1}', N'{2}', {3}, {4}, {5}, N'N', {6},{7})",
													moSubjectMasterStruct.miSchoolId,
													StringUtility.ReplaceSingleQuoteInString(moSubjectMasterStruct.msSubjectName, false),
													StringUtility.ReplaceSingleQuoteInString(moSubjectMasterStruct.msShortName, false),
													moSubjectMasterStruct.miOriginalSubjectId,
													moSubjectMasterStruct.miInsertedByid,
													moSubjectMasterStruct.miAcademicyearId,
													Convert.ToInt32(moSubjectMasterStruct.mbIsCoCurricularActivity),
                                                    Convert.ToInt32(moSubjectMasterStruct.mbIsAttitudeSubject));

            return sInsertStatement;
        }

        public string GetUpdateStatementForSubject()
        {
            string sUpdateStatement = String.Format("UPDATE Subject_Master SET School_Id = {0}, Subject_Name = N'{1}', Short_Name = N'{2}', Original_Subject_Id = {3}, Updated_By_Id = {4}, Is_CoCurricularActivity = {5}, IsAttitudeSubject={7}" +
													" WHERE Is_Deleted = N'N' AND Subject_Id = {6}",
													moSubjectMasterStruct.miSchoolId,
													StringUtility.ReplaceSingleQuoteInString(moSubjectMasterStruct.msSubjectName, false),
													StringUtility.ReplaceSingleQuoteInString(moSubjectMasterStruct.msShortName, false),
													moSubjectMasterStruct.miOriginalSubjectId,
													moSubjectMasterStruct.miUpdatedById,
													Convert.ToInt32(moSubjectMasterStruct.mbIsCoCurricularActivity),
													moSubjectMasterStruct.miSubjectId,
                                                    Convert.ToInt32(moSubjectMasterStruct.mbIsAttitudeSubject)
                                                    );

            return sUpdateStatement;
        }

        public string GetDeleteStatementForSubject()
        {
			string sDeleteStatement = String.Format("DELETE FROM Subject_Master WHERE Is_Deleted = 'N' AND Subject_Id = {0}", moSubjectMasterStruct.miSubjectId);

			return sDeleteStatement;
        }

        public string GetSubjectName(int aiSchoolId, int aiSubjectId)
        {
            string sSubjectName = " SELECT " +
                                   " Subject_Master.Subject_Name " +
                               " FROM " +
                                   " Subject_Master " +
                                " WHERE " +
                                   " Subject_Id= " + aiSubjectId +
                                   " AND School_Id=" + aiSchoolId +
                                   " AND Is_Deleted= '" + Constants.C_NO + "' ";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSubjectName);
        }

        #endregion

        #region Standard-Division Subjects

        public string GetInsertStatementForStandardDivisionSubjects()
        {
            string sInsertStatement = "";
            sInsertStatement = " INSERT INTO Schoolwise_Division_Subject_Master " +
                                    " ( " + "school_id" +
                                    " , " + "Standard_Division_Id" +
                                    " , " + " Subject_Id " +
                                    " , " + "Inserted_By_Id" +
                                    " , " + "Updated_By_Id" +
                                     " , " + "academic_Year_Id" +
                               " ) VALUES ( " +
                                    " N'" + moSubjectMasterStruct.miSchoolId + "'" +
                                    " , N'" + moStandardDivisionSubjectStruct.miStandardDivisionId + "'" +
                                    " , N'" + moSubjectMasterStruct.miSubjectId + "'" +
                                    " , N'" + moSubjectMasterStruct.miUpdatedById + "'" +
                                    " , N'" + moSubjectMasterStruct.miUpdatedById + "'" +
                                    " , N'" + moSubjectMasterStruct.miAcademicyearId + "'" +
                               ")";

            return sInsertStatement;
        }

        public string GetDeleteStatementForStandardDivisionSubjects()
        {
            string sUpdateStatement = "";
            sUpdateStatement = " DELETE FROM Schoolwise_Division_Subject_Master " +
                               "  WHERE " +
                                    " Standard_Division_Id  = N'" + moStandardDivisionSubjectStruct.miStandardDivisionId + "'" +
                                    " AND Subject_Id = N'" + moSubjectMasterStruct.miSubjectId + "'" +
                                    " AND is_deleted = N'" + Constants.C_NO + "'";
            return sUpdateStatement;
        }

        #endregion

        #region Standard Subjects

        public string GetInsertStatementForStandardSubjects()
        {
            string sInsertStatement = "";
            sInsertStatement = " INSERT INTO SchoolWise_Standard_Subject_Master " +
                                    " ( " + "school_id" +
                                    " , " + "Standard_Id" +
                                    " , " + "Subject_Id" +
                                    " , " + "Inserted_By_id" +
                                    " , " + "Updated_By_Id" +
                                    " , " + "academic_year_id" +
                                    " , " + "Sort_Order" +

                               " ) SELECT  " +
                                    " N'" + moSubjectMasterStruct.miSchoolId + "'" +
                                    " , N'" + moStandardSubjectStruct.miStandardId + "'" +
                                    " , N'" + moSubjectMasterStruct.miSubjectId + "'" +
                                    " , N'" + moSubjectMasterStruct.miUpdatedById + "'" +
                                    " , N'" + moSubjectMasterStruct.miUpdatedById + "'" +
                                    " , N'" + moSubjectMasterStruct.miAcademicyearId + "'" +
                                    " , ISNULL(MAX(Sort_Order),0)+1  " +
                                    " FROM   SchoolWise_Standard_Subject_Master  " +
                                    " WHERE     (Is_Deleted = 'N') " +
                                    " AND (academic_year_id = N'" + moSubjectMasterStruct.miAcademicyearId + "') " +
                                    " AND (School_Id = N'" + moSubjectMasterStruct.miSchoolId + "') " +
                                    " AND (Standard_Id = N'" + moStandardSubjectStruct.miStandardId + "') ";
            return sInsertStatement;
        }

        public string GetDeleteStatementForStandardSubjects()
        {
            string sUpdateStatement = "";
            sUpdateStatement = " DELETE FROM SchoolWise_Standard_Subject_Master " +
                               "  WHERE " +
                                    " Standard_Id  = N'" + moStandardSubjectStruct.miStandardId + "'" +
                                    " AND Subject_Id = N'" + moSubjectMasterStruct.miSubjectId + "'" +
                                    " AND is_deleted = N'" + Constants.C_NO + "'";
            return sUpdateStatement;
        }

        /// <summary>
        /// This method is used to update sort order of subjects of given standard
        /// </summary>
        /// <param name="ischoolId"></param>
        /// <param name="iAcademicYearId"></param>
        /// <param name="iStandardId"></param>
        /// <param name="sXmlSubjectOrder"></param>
        public void UpdateSubjectSortOrder(int ischoolId, int iAcademicYearId, int iStandardId, string sXmlSubjectOrder)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", ischoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", iAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_ID", iStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("sXmlSubjectOrder", sXmlSubjectOrder, SqlDbType.Xml);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateStandardSubjectSortOrder");
            }
        }

        #endregion

        #region Subject-groups
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public string GetInsertStamentForSubjectGroups()
        {
            string sInsertStatement = "INSERT INTO Subject_Groups ( " +
                "  school_id" +
                " , Parent_Subject_Id" +
                " , parent_group_id" +
                " , subject_id" +
                " , academic_year_id" +
                " , Is_Deleted" +
                " , Inserted_By_Id" +
                " , Standard_Division_Id" +
            ") VALUES (" + "  " +
                " N'" + moSubjectMasterStruct.miSchoolId + "'" +
                 " , N'" + moSubjectGroupsStruct.miParentSubjectId + "'" +
                  " , N'" + moSubjectGroupsStruct.miParentGroupId + "'" +
                 " , N'" + moSubjectMasterStruct.miSubjectId + "'" +
                 " , N'" + moSubjectMasterStruct.miAcademicyearId + "'" +
                 " , N'" + Constants.C_NO + "'" +
                  " , N'" + moSubjectMasterStruct.miInsertedByid + "'" +
                  " , " + moSubjectMasterStruct.miStandardDivisionId + 
            " ) ";
            return sInsertStatement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDeleteStatementForSubjectGroups()
        {
            string sDeleteStatement;

            sDeleteStatement = " DELETE FROM Subject_Groups " +
                               " WHERE " +
                                    " parent_group_id  = N'" + moSubjectGroupsStruct.miParentGroupId + "'" +
                                    " AND Subject_Id = N'" + moSubjectMasterStruct.miSubjectId + "'" +
                                    " AND School_Id = N'" + moSubjectMasterStruct.miSchoolId + "'" +
                                    " AND academic_year_id = N'" + moSubjectMasterStruct.miAcademicyearId + "'" +
                                    " AND Standard_Division_Id = " + moSubjectMasterStruct.miStandardDivisionId +
                                    " AND is_deleted = N'" + Constants.C_NO + "'";

            return sDeleteStatement;
        }
        public string GetUpdateStamentSubjectGroups(int aiParentSubjectIdToChange)
        {

            string sUpdateStatement = " UPDATE Subject_Groups " +
                              " SET " +
                              " Parent_Subject_Id= N'" + moSubjectGroupsStruct.miParentSubjectId + "' " +
                              ", Updated_By_Id= N'" + moSubjectMasterStruct.miUpdatedById + "' " +
                              "  WHERE " +
                               " Parent_Subject_Id  = N'" + aiParentSubjectIdToChange + "' " +
                               " AND  school_id  = N'" + moSubjectMasterStruct.miSchoolId + "'" +
                                " AND academic_year_id  = N'" + moSubjectMasterStruct.miAcademicyearId + "'" +
                                " AND Standard_Division_Id = " + moSubjectMasterStruct.miStandardDivisionId +
                               " AND is_deleted = N'" + Constants.C_NO + "'";
            return sUpdateStatement;
        }

        public string GetSubjectNameForSubjectId(int aiSubjectId)
        {
            string sSelectStatement = " SELECT " +
                                      " Subject_Name " +
                                      " FROM " +
                                        " Subject_Master " +
                                      " WHERE " +
                                         " Subject_id=" + aiSubjectId +
                                         " AND is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);
        }

        #endregion

        public static int CheckMarksAssigned(int aiSubjectId, int aiSchoolId, int aiAcademicYearId)
        {
            string sSelectStatement = " SELECT COUNT(Subject_Id)"+
                                      " FROM SchoolWise_Test_Subject_Marks_Master" +
                                      " WHERE Is_Deleted = N'" + Constants.C_NO + "'" +
                                      " AND Subject_Id=" + aiSubjectId +
                                      " AND School_Id=" + aiSchoolId +
                                      " AND academic_year_id =" + aiAcademicYearId;
            int iCount;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
            return iCount;
        }

        #endregion

    }

    public class SubjectCollectionDC
    {
        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        public SubjectCollectionDC(int aiSchoolId)
        {
            miSchoolId = aiSchoolId;
        }
        public SubjectCollectionDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
        }

        public DataTable GetAllSubjects()
        {
            // This method returns datatable populated with master subjects from databse.
            string sSelectStatement = String.Format("SELECT -9999 as school_id,original_subject_id,subject_id,subject_name,Short_Name,Is_CoCurricularActivity,IsAttitudeSubject FROM subject_master " +
													" WHERE is_deleted = 'N' AND school_id is null AND original_subject_id NOT IN (SELECT original_subject_id FROM subject_master WHERE is_deleted = 'N' AND school_id = {0} AND academic_year_id = {1} ) " +
                                                    "UNION SELECT school_id, original_subject_id,subject_id,subject_name,Short_Name,Is_CoCurricularActivity,IsAttitudeSubject FROM subject_master WHERE is_deleted = 'N' AND school_id = {0} AND academic_year_id = {1} ORDER BY original_subject_id",
													miSchoolId,
													miAcademicYearId);
			
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        /// <summary>
        /// This method is used to fill dropdown list of child and parent subjects. 
        /// </summary>
        /// <returns></returns>

        public DataSet GetChildParentSubjects()
        {
            string sParentSelectStatement = " SELECT  " +
                                   " school_id " +
                                   " , original_subject_id " +
                                   " , subject_id " +
                                   " , subject_name " +
                                   " , '0000' As Teacher_Id " +
                               " FROM " +
                                    " subject_master " +
                               " WHERE " +
                                    " is_deleted = N'" + Constants.C_NO + "'" +
                                    " AND school_id = " + miSchoolId +
                                     " AND academic_year_id = " + miAcademicYearId +
                                     " ORDER BY " +
                                     " original_subject_id";
            //"AND subject_id not in (SELECT DISTINCT  Subject_id FROM Subject_Groups " +
            //                               "WHERE  School_id = " + miSchoolId + " AND is_deleted = '" + Constants.C_NO + "'" +
            //                                "AND academic_Year_Id =" + miAcademicYearId + ")" +
            //" ORDER BY " +
            //     " original_subject_id";

            string sChildSelectStatement = " SELECT  " +
                                   " school_id " +
                                   " , original_subject_id " +
                                   " , subject_id " +
                                   " , subject_name " +
                                   " , '0000' As Teacher_Id " +
                               " FROM " +
                                    " subject_master " +
                               " WHERE " +
                                    " is_deleted = N'" + Constants.C_NO + "'" +
                                    " AND school_id = " + miSchoolId +
                                    " AND academic_year_id = " + miAcademicYearId +
                                     "AND subject_id not in (SELECT DISTINCT  parent_Subject_id FROM vw_SubjectGroup " +
                                                               "WHERE  School_id = " + miSchoolId + " AND is_deleted = N'" + Constants.C_NO + "'" +
                                                                "AND academic_Year_Id =" + miAcademicYearId + ")" +
                                     " ORDER BY " +
                                     " original_subject_id";

            string sFinalQuery = sParentSelectStatement + ";" + sChildSelectStatement;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sFinalQuery);
        }


        // <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public DataTable GetAssociatedSubjects()
        {
            string sSelectStatement = " SELECT  " +
                                   " school_id " +
                                   " , original_subject_id " +
                                   " , subject_id " +
                                   " , subject_name " +
                                   " , '0000' As Teacher_Id " +
                               " FROM " +
                                    " subject_master " +
                               " WHERE " +
                                    " is_deleted = N'" + Constants.C_NO + "'" +
                                    " AND school_id = " + miSchoolId +
                                     " AND academic_year_id = " + miAcademicYearId +
                                " ORDER BY " +
                                     " original_subject_id";


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

        //public DataTable GetAllStandardSubjects()
        //{
        //    Constants.ParameterNameValuePair[] oArrParameterNameValuePair = new Constants.ParameterNameValuePair[2];
        //    SqlParameter[0].Name = "School_Id";
        //    SqlParameter[0].DbType = DbType.Int32;
        //    SqlParameter[0].Value = miSchoolId.ToString();

        //    SqlParameter[1].Name = "AcademicYear_ID";
        //    SqlParameter[1].DbType = DbType.Int32;
        //    SqlParameter[1].Value = miAcademicYearId.ToString();

        //    return using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
        //oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetStandardSubjects", oArrParameterNameValuePair);


        /// <summary>
        /// This method is used to get all subjcts of given standard
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <returns></returns>
        public DataTable GetSubjectsForStandard(int aiStandard)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_ID", aiStandard, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetSubjectsForStandard");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiClassId"></param>
        /// <returns></returns>
        public DataSet GetAllSubjectsforDivision(int aiClassId)
        {
            ArrayList arrReturnArray = new ArrayList();
            string sSelectStatement = " SELECT " +
                                        " Schoolwise_Division_Subject_Master.Subject_Id  " +
                                        ",Subject_Master.Subject_Name" +
                                        ",SchoolWise_Standard_Subject_Master.Sort_Order" +
                                    " FROM " +
                                        " Schoolwise_Division_Subject_Master  " +
                                    " INNER JOIN " +
                                        " SchoolWise_Standard_Division_Master  " +
                                    " ON " +
                                          " SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = Schoolwise_Division_Subject_Master.Standard_Division_Id " +
                                    " INNER JOIN SchoolWise_Standard_Subject_Master ON SchoolWise_Standard_Division_Master.Standard_Id = SchoolWise_Standard_Subject_Master.Standard_Id and Schoolwise_Division_Subject_Master.subject_Id = SchoolWise_Standard_Subject_Master.subject_Id" +
                                    " INNER JOIN " +
                                          " Subject_Master " +
                                    " ON " +
                                          "Schoolwise_Division_Subject_Master.Subject_Id = Subject_Master.Subject_Id " +
                                    " INNER JOIN " +
                                        " Standard_Master " +
                                    " ON  " +
                                        " Standard_Master.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id " +
                                     " INNER JOIN " +
                                        " Division_Master " +
                                    " ON  " +
                                     " Division_Master.Division_Id = SchoolWise_Standard_Division_Master.Division_Id  " +
                                    " WHERE " +
                                        " Schoolwise_Division_Subject_Master.School_Id = " + miSchoolId +
                                        " AND Schoolwise_Division_Subject_Master.academic_year_id = " + miAcademicYearId +
                                        " AND Schoolwise_Division_Subject_Master.Standard_Division_Id= " + aiClassId +
                //  " AND SchoolWise_Standard_Division_Master.Standard_Id = " + aiStandardId +
                                        " AND Schoolwise_Division_Subject_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Standard_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Division_Master.Is_Deleted = N'" + Constants.C_NO + "' " +
                                        " AND Subject_Master.Is_Deleted= N'" + Constants.C_NO + "' " +
                                        " ORDER BY " +
                                        " original_subject_id"; ;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement);




            #region Code needed
            //            SELECT     Schoolwise_Division_Subject_Master.Subject_Id
            //FROM         Division_Master INNER JOIN
            //                      Schoolwise_Division_Subject_Master ON Division_Master.Division_Id = Schoolwise_Division_Subject_Master.Division_Id INNER JOIN
            //                      Subject_Master ON Schoolwise_Division_Subject_Master.Subject_Id = Subject_Master.Subject_Id INNER JOIN
            //                      Standard_Master ON Schoolwise_Division_Subject_Master.Standard_Id = Standard_Master.Standard_Id

            // FROM  Subject_Master 
            // INNER JOIN  
            //Schoolwise_Division_Subject_Master 
            // ON  
            //Schoolwise_Division_Subject_Master.Subject_Id = Subject_Master.Subject_Id 
            //INNER JOIN  SchoolWise_Standard_Division_Master
            //ON 
            //SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = Schoolwise_Division_Subject_Master.Standard_Division_Id
            //INNER JOIN  Standard_Master  
            //ON  
            // Standard_Master.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id 
            // INNER JOIN  Division_Master 
            // ON   Division_Master.Division_Id = SchoolWise_Standard_Division_Master.Division_Id  
            //WHERE  Schoolwise_Division_Subject_Master.School_Id = 57
            // AND
            //SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id = 928
            // AND Schoolwise_Division_Subject_Master.Is_Deleted = 'N'  
            //AND Standard_Master.Is_Deleted = 'N'  
            //AND Division_Master.Is_Deleted = 'N' 
            //AND Subject_Master.Is_Deleted= 'N' 
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>
        public void UpdateStandardDivisionsSubjects(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoArrayListInsertStatements"></param>
        public void UpdateSubjects(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListInsertStatements.ToArray(typeof(string)));
        }


    }

}
