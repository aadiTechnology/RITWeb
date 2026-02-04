// Class Name       :- StudentPaidFeeDetailsReportDC
// Purpose          :- This class is used to get students paid fee details for export report..
// Date Of creation :- 02/11/2019
// Author Name      :- Dnyaneshwar Shinde

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.StudentPaidFeeDetails;
using Utility;

namespace DataCommunicator.StudentPaidFeeDetailsReport
{
    public class StudentPaidFeeDetailsReportDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private List<PayableForDetails> mlstPayableForDetails;
        private List<PaidFeeDetails> mlstPaidFeeDetails;
        private List<StudentFeeDetails> mlstStudentFeeDetails;

        #endregion

        #region Constructor(s)

        public StudentPaidFeeDetailsReportDC()
        {
        }

        public StudentPaidFeeDetailsReportDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        #endregion

        #region Property(s)

        public List<PayableForDetails> PayableForDetails
        {
            get { return this.mlstPayableForDetails; }
        }

        public List<PaidFeeDetails> PaidFeeDetails
        {
            get { return this.mlstPaidFeeDetails; }
        }

        public List<StudentFeeDetails> StudentFeeDetails
        {
            get { return this.mlstStudentFeeDetails; }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to get student paid fee details for report.
        /// </summary>
        /// <param name="aiStandardId"></param>
        /// <param name="aiDivisionId"></param>
        /// <param name="aiFeeTypeId"></param>
        /// <returns></returns>
        public List<StudentDetails> GetStudentPaidFeeDetailsForReport(int aiStandardId, int aiDivisionId, int aiFeeTypeId)
        {
            List<StudentDetails> lstStudentDetails;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Division_Id", aiDivisionId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FeeTypeId", aiFeeTypeId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetStudentPaidFeeDetailsForReport"))
                {
                    lstStudentDetails = LoadStudentDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillPayableDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillStudentsPaidFeeDetails(oSqlDataReader);

                    oSqlDataReader.NextResult();
                    FillStudentFeeDetails(oSqlDataReader);
                }
            }
            return lstStudentDetails;
        }

        #endregion

        #region Private Method(s)

        /// <summary>
        /// This method is used to load student details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        /// <returns></returns>
        private List<StudentDetails> LoadStudentDetails(SqlDataReader aoSqlDataReader)
        {
            List<StudentDetails> lstStudentDetails = new List<StudentDetails>();
            while (aoSqlDataReader.Read())
            {
                lstStudentDetails.Add(
                    new StudentDetails
                    {
                        YearwiseStudentId = aoSqlDataReader["Yearwise_Student_Id"].ToInt(),
                        RollNo = aoSqlDataReader["Roll_No"].ToInt(),
                        StudentName = aoSqlDataReader["StudentName"].ToString(),
                        EnrolmentNumber = aoSqlDataReader["Enrolment_Number"].ToString(),
                        ClassName = aoSqlDataReader["className"].ToString()
                    }
                    );
            }
            return lstStudentDetails;
        }

        /// <summary>
        /// This method is used to fill payable details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillPayableDetails(SqlDataReader aoSqlDataReader)
        {
            mlstPayableForDetails = new List<PayableForDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstPayableForDetails.Add(
                    new PayableForDetails
                    {
                        PayableFor = aoSqlDataReader["IntervalName"].ToString()
                    }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill students paid fee details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillStudentsPaidFeeDetails(SqlDataReader aoSqlDataReader)
        {
            mlstPaidFeeDetails = new List<PaidFeeDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstPaidFeeDetails.Add(
                        new PaidFeeDetails
                        {
                            StudentId = aoSqlDataReader["Student_Id"].ToInt(),
                            FeeType = aoSqlDataReader["Fee_Type"].ToString(),
                            PayableFor = aoSqlDataReader["Payable_For"].ToString(),
                            PaidDate = aoSqlDataReader["Paid_Date"].ToDateTime(),
                            Amount = aoSqlDataReader["Amount"].ToInt(),                            
                            ChequeNumber = aoSqlDataReader["ChequeNumber"].ToString()
                        }
                    );
            }
        }

        /// <summary>
        /// This method is used to fill students paid and pending fee details.
        /// </summary>
        /// <param name="aoSqlDataReader"></param>
        private void FillStudentFeeDetails(SqlDataReader aoSqlDataReader)
        {
            mlstStudentFeeDetails = new List<StudentFeeDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstStudentFeeDetails.Add(
                    new StudentFeeDetails {
                        StudentId = aoSqlDataReader["StudentId"].ToInt(),
                        PaidAmount = aoSqlDataReader["PaidAmount"].ToInt(),
                        PedingAmount = aoSqlDataReader["PedingAmount"].ToInt()
                    }
                    );
            }
        }

        #endregion
    }
}
