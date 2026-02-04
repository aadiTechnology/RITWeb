using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Admin;
using Utility;
using StudentEntities;
using System.Linq;

namespace DataCommunicator
{
    public class UpdateStudentDetailsInBulkDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;
        private static string msOperator = string.Empty;

        #endregion

        #region Constructor(s)

        public UpdateStudentDetailsInBulkDC()
        {
        }

        public UpdateStudentDetailsInBulkDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUpdatedById = aiUpdatedById;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion Constructor(s)

        #region Public Method(s)

        public DataTable GetFillCategoy()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("[usp_GetStudentDataCategory]");
            }
        }

        public List<UpdateStudentDetailsInBulk> GetAll(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiDivisionId, int aiCategoryId, string asEnrolmentNumber, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix, int aiStartIndex, int aiEndIndex, string asSortExpression)
        {
            string sFilter = CreateRegNoReassignFilter(aiStandardId, aiDivisionId, asEnrolmentNumber, abIsStudBlankRegNo, asRegNo, abIsExact, asOperator, asPrefix, aiCategoryId);
            
            List<UpdateStudentDetailsInBulk> lstUpdateStudentDetailsInBulk = new List<UpdateStudentDetailsInBulk>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCategoryId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", sFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentDetailsInBulk"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstUpdateStudentDetailsInBulk.Add(new UpdateStudentDetailsInBulk
                        {
                            EnrollmentNumber = oSqlDataReader["Enrolment_Number"].ToString(),
                            RollNumber = Convert.ToInt32(oSqlDataReader["Roll_No"]),
                            StudentName = oSqlDataReader["StudentName"].ToString(),
                            ExistingValue = oSqlDataReader["ExistingValue"].ToString(),
                            YearWise_Student_Id = Convert.ToInt32(oSqlDataReader["YearWise_Student_Id"]),
                            ClassName = oSqlDataReader["className"].ToString(),
                            TotalRecords = oSqlDataReader["TotalRecords"].ToInt()
                        });
                    }
                }
            }
            return lstUpdateStudentDetailsInBulk;
        }

        private static string CreateRegNoReassignFilter(int aiStandardId, int aiDivisionId, string asEnrolmentNumber, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix, int aiCategoryId)
        {
            if (!abIsExact || asRegNo.IsNullOrEmpty())
                msOperator = string.Empty;
            else
            {
                List<String> operators = GetOperators().Where(opr => opr.Value.ToString() == asOperator).Select(opr => opr.Text).ToList();
                msOperator = operators.Count > Constants.I_ZERO ? operators.First() : string.Empty;
            }

            //if (abIsExact == true && asRegNo.IsNullOrEmpty())
            //    abIsExact = false;

            string sFilter = "";
            if (aiStandardId != 0)
                sFilter = " AND YSD.Standard_Id = " + aiStandardId.ToString();

            if (aiDivisionId != 0)
                sFilter = sFilter + " AND YSD.Division_id=" + aiDivisionId.ToString();

            if (!String.IsNullOrEmpty(asEnrolmentNumber))
            {
                string sName = Utility.StringUtility.ReplaceSingleQuoteInString(asEnrolmentNumber, true);
                sFilter = sFilter + " AND (BSD.StudentName LIKE '%" + sName + "%' OR BSD.Enrolment_Number LIKE '%" + sName + "%' OR BSD.Enrolment_Number + ' - ' + BSD.StudentName LIKE N'%" + sName + "%' )";
            }
            else if (!String.IsNullOrEmpty(asRegNo) && abIsExact)            
            {
                if (!asPrefix.IsNullOrEmpty())
                {
                    if (asPrefix == Constants.S_ALL)
                        asPrefix = string.Empty;

                    sFilter = sFilter + " AND Enrolment_Number <> '' AND Enrolment_Number LIKE '%" + asPrefix + "%' " + " AND CONVERT(BIGINT,REPLACE(BSD.Enrolment_Number,'PP','')) " + msOperator + "'" + asRegNo + "'";
                }
                else
                    sFilter = sFilter + " AND BSD.Enrolment_Number " + msOperator + "'" + asRegNo + "'";
            }
            else if (String.IsNullOrEmpty(asRegNo) && abIsExact)
            {
                if (!asPrefix.IsNullOrEmpty())
                {
                    if (asPrefix != Constants.S_ALL)
                        sFilter = sFilter + " AND Enrolment_Number <> '' AND Enrolment_Number LIKE '%" + asPrefix + "%' ";
                }               
            }
            else if (abIsExact)
                sFilter = sFilter + " AND BSD.Enrolment_Number =''";

            if (abIsStudBlankRegNo)
            {
                if (aiCategoryId == 1)
                    sFilter = sFilter + " AND (BSD.SaralNo IS NULL OR LTRIM(RTRIM(BSD.SaralNo)) = '')";
                else if (aiCategoryId == 2)
                    sFilter = sFilter + " AND (SAD.PenNo IS NULL OR LTRIM(RTRIM(SAD.PenNo)) = '')";
                else if (aiCategoryId == 3)
                    sFilter = sFilter + " AND (SAD.ApaarId IS NULL OR LTRIM(RTRIM(SAD.ApaarId)) = '')";
            }

            return sFilter;
        }


        //private static string CreateRegNoReassignFilter(int aiStandardId, int aiDivisionId, string asEnrolmentNumber, bool abIsStudBlankRegNo, string asRegNo, bool abIsExact, string asOperator, string asPrefix)
        //{
        //    if (!abIsExact || asRegNo.IsNullOrEmpty())
        //        msOperator = string.Empty;
        //    else
        //    {
        //        List<String> operators = GetOperators().Where(opr => opr.Value.ToString() == asOperator).Select(opr => opr.Text).ToList();
        //        msOperator = operators.Count > Constants.I_ZERO ? operators.First() : string.Empty;
        //    }

        //    if (abIsExact == true && asRegNo.IsNullOrEmpty())
        //        abIsExact = false;

        //    string sFilter = "";
        //    if (aiStandardId != 0)
        //        sFilter = " AND vw_GetAllStudentsForStandardDivision.[Standard_Id] = CAST(" + aiStandardId.ToString() + " AS VARCHAR(15))";

        //    if (aiDivisionId != 0)
        //        sFilter = sFilter + " AND vw_GetAllStudentsForStandardDivision.[Division_id] =+ CAST(" + aiDivisionId.ToString() + " AS VARCHAR(15))";

        //    if (!String.IsNullOrEmpty(asEnrolmentNumber))
        //    {
        //        string sName = Utility.StringUtility.ReplaceSingleQuoteInString(asEnrolmentNumber, true);
        //        sFilter = sFilter + " AND (vw_GetAllStudentsForStandardDivision.Name LIKE '%" + sName + "%' OR vw_GetAllStudentsForStandardDivision.Enrolment_Number LIKE '%" + sName + "%' OR vw_GetAllStudentsForStandardDivision.Enrolment_Number + ' - ' + Name LIKE N'%" + sName + "%' )";
        //    }
        //    else if (!String.IsNullOrEmpty(asRegNo) && abIsExact)
        //    //sFilter = sFilter + " AND vw_GetAllStudentsForStandardDivision.Enrolment_Number  IN (SELECT Name FROM udf_GetTableFromStringList('" + StringUtility.ReplaceSingleQuoteInString(asRegNo.Trim(), true) + "'))";
        //    {
        //        if (!asPrefix.IsNullOrEmpty())
        //        {
        //            if (asPrefix == Constants.S_ALL)
        //                asPrefix = string.Empty;

        //            sFilter = sFilter + " AND Enrolment_Number LIKE '%" + asPrefix + "%' " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
        //        }
        //        else
        //            sFilter = sFilter + " AND #tblStudents.HasPrefix=0 " + " AND #tblStudents.EnrollmentNo " + msOperator + asRegNo;
        //    }
        //    else if (abIsExact)
        //        sFilter = sFilter + " AND Enrolment_Number =''";

        //    if (abIsStudBlankRegNo)
        //        sFilter = sFilter + " AND ( vw_GetAllStudentsForStandardDivision.Enrolment_Number LIKE '')";

        //    return sFilter;
        //}

        public static List<Operator> GetOperators()
        {
            List<Operator> olstOperators = new List<Operator>();
            olstOperators.Add(new Operator { Value = 1, Text = "=" });
            olstOperators.Add(new Operator { Value = 2, Text = "<" });
            olstOperators.Add(new Operator { Value = 3, Text = "<=" });
            olstOperators.Add(new Operator { Value = 4, Text = ">" });
            olstOperators.Add(new Operator { Value = 5, Text = ">=" });
            return olstOperators;
        }

        public void Save(string asUpdateStudentDetailsInBulkXML, int aiStandardId, int aiDivisionId, int aiCatgegoryId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UpdateStudentDetailsInBulkXML", asUpdateStudentDetailsInBulkXML, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("divisionId", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CategoryId", aiCatgegoryId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveUpdateStudentDetailsInBulk");
            }
        } 

        #endregion
    }
}
