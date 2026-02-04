using System;
using System.Data;
using Utility;

namespace DataCommunicator
{
    public class YearWiseStudentsDC : DataCommunicatorBaseDC
    {
        #region Constants & Structure

        public struct YrWiseStudentInfo
        {
            // This structure is replica of Company_Master database table.
            public Int32 iSchoolId;
            public Int32 iYearWIseStudentId;
            public Int32 iStudentId;
            public Int32 iYearId;
            public Int32 iStandardId;
            public Int32 iDivisionId;
            public string sRollNo;
            public Int32 iSchoolWiseAcademicYearId;
            public Double fFeesTobePaid;
            public char cIsFeeApplicable;
            public Int32 iInsertedById;
            public Int32 iUpdatedById;
        }

        #endregion

        #region DataMembers & Properties

        #region DataMembers

        private YrWiseStudentInfo moYrWiseStudentInfo;

        #endregion
        #region Properties
        public YrWiseStudentInfo YearWiseStudentInfo
        {
            get
            {
                return moYrWiseStudentInfo;
            }
            set
            {
                moYrWiseStudentInfo = value;
            }
        }
        #endregion

        #endregion

        #region constructors
        public YearWiseStudentsDC()
        {
            //Default constructor
        }

        public YearWiseStudentsDC(Int32 aiYearWiseStudentId)
        {
            //Parameterised constructor
            PopulateYearWiseStructFields(aiYearWiseStudentId);
        }

        #endregion

        #region public methods

        #region Year wise Details
        /// <summary>
        /// Function inserts yearwise student details record
        /// </summary>
        /// <returns></returns>
        public Int32 InsertYrWiseStudentInformation()
        {
            string sInsertString = "INSERT INTO YearWise_Student_Details" +
                                       " (Academic_Year_ID" +
                                       " , School_Id" +
                                       " , Student_Id" +
                                       " , Standard_Id" +
                                       " , Division_Id " +
                                       " , Is_fee_applicable " +
                                       " , Fees_to_be_paid " +
                                       " , Roll_No " +
                                       " , Inserted_By_Id" +
                                       " , Updated_By_Id" +
                                       ")" +
                              "VALUES " +
                                       " ( N'" + moYrWiseStudentInfo.iSchoolWiseAcademicYearId + "' " +
                                       "  , N'" + moYrWiseStudentInfo.iSchoolId + "' " +
                                       "  , N'" + moYrWiseStudentInfo.iStudentId + "' " +
                                       "  , N'" + moYrWiseStudentInfo.iStandardId + "' " +
                                       "  , N'" + moYrWiseStudentInfo.iDivisionId + "' " +
                                       "  , N'" + moYrWiseStudentInfo.cIsFeeApplicable + "' " +
                                       "  , N'" + moYrWiseStudentInfo.fFeesTobePaid + "' " +
                                       "  , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moYrWiseStudentInfo.sRollNo, true) + "' " +
                                       "  , N'" + moYrWiseStudentInfo.iInsertedById + "' " +
                                       "  , N'" + moYrWiseStudentInfo.iInsertedById + "' " +
                                       ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sInsertString);
        }
        /// <summary>
        /// Function updatess yearwise student details record
        /// </summary>
        /// <returns></returns>
        public Int32 UpdateYrWiseStudentInformation()
        {
            string sUpdateString = "UPDATE " +
                                        "  YearWise_Student_Details SET " +
                                        "  Standard_Id = N'" + moYrWiseStudentInfo.iStandardId + "'" +
                                        " , Division_Id = N'" + moYrWiseStudentInfo.iDivisionId + "' " +
                                        " , Roll_No = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moYrWiseStudentInfo.sRollNo, true) + "' " +
                                        " , Updated_By_id= N'" + moYrWiseStudentInfo.iInsertedById + "' " +
                                        " , Update_Date= N'" + System.DateTime.Today + "' " +
                                        " , Student_Id = N'" + moYrWiseStudentInfo.iStudentId + "'" +
                                        " , Is_fee_applicable = N'" + moYrWiseStudentInfo.cIsFeeApplicable + "'" +
                                        " , Fees_to_be_paid = N'" + moYrWiseStudentInfo.fFeesTobePaid + "'" +
                                      "WHERE " +
                                       "YearWise_Student_Id= N'" + moYrWiseStudentInfo.iYearWIseStudentId + "'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sUpdateString);
        }

        /// <summary>
        /// This function gets the record set for a perticular student
        /// </summary>
        /// <param name="aiYearWiseStudentId"></param>
        private void PopulateYearWiseStructFields(Int32 aiYearWiseStudentId)
        {
            string sSelectStament = "SELECT " +
                                             " YearWise_Student_Id " +
                                             " , Academic_Year_ID " +
                                             " , Student_Id " +
                                             " , Standard_Id " +
                                             " , Roll_No " +
                                             " , Division_id " +
                                             ", Is_fee_applicable " +
                                             ", Fees_to_be_paid" +
                                             " , Roll_No " +
                                    " FROM YearWise_Student_Details" +
                                    " WHERE " +
                                            " YearWise_Student_Id= N'" + aiYearWiseStudentId + "'" +
                                            " AND is_deleted = N'" + Constants.C_NO + "'";
            DataTable oDTStudent;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oDTStudent = oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStament);
            if (oDTStudent.Rows.Count > 0)
                FillYearWiseStruct(oDTStudent);
        }

        /// <summary>
        /// This function populates the student basic details' structure  with values in table
        /// </summary>
        /// <param name="aDoTable"></param>
        private void FillYearWiseStruct(DataTable aDoTable)
        {
            if (aDoTable.Rows[0]["Academic_Year_ID"] != DBNull.Value)
                moYrWiseStudentInfo.iSchoolWiseAcademicYearId = Convert.ToInt32(aDoTable.Rows[0]["Academic_Year_ID"]);
            if (aDoTable.Rows[0]["Student_Id"] != DBNull.Value)
                moYrWiseStudentInfo.iStudentId = Convert.ToInt32(aDoTable.Rows[0]["Student_Id"]);
            if (aDoTable.Rows[0]["YearWise_Student_Id"] != DBNull.Value)
                moYrWiseStudentInfo.iYearWIseStudentId = Convert.ToInt32(aDoTable.Rows[0]["YearWise_Student_Id"]);
            if (aDoTable.Rows[0]["Standard_Id"] != DBNull.Value)
                moYrWiseStudentInfo.iStandardId = Convert.ToInt32(aDoTable.Rows[0]["Standard_Id"]);
            if (aDoTable.Rows[0]["Roll_No"] != DBNull.Value)
                moYrWiseStudentInfo.sRollNo = aDoTable.Rows[0]["Roll_No"].ToString();
            if (aDoTable.Rows[0]["Division_id"] != DBNull.Value)
                moYrWiseStudentInfo.iDivisionId = Convert.ToInt32(aDoTable.Rows[0]["Division_id"]);
            if (aDoTable.Rows[0]["Is_fee_applicable"] != DBNull.Value)
                moYrWiseStudentInfo.cIsFeeApplicable = Convert.ToChar(aDoTable.Rows[0]["Is_fee_applicable"]);
            if (aDoTable.Rows[0]["Fees_to_be_paid"] != DBNull.Value)
                moYrWiseStudentInfo.fFeesTobePaid = Convert.ToInt32(aDoTable.Rows[0]["Fees_to_be_paid"]);

        }

        #endregion

        #endregion
    }
}
