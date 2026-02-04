using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Utility;

namespace DataCommunicator
{
    public class SchoolWiseTeacherMasterDC : DataCommunicatorBaseDC
    {
        #region " Data Members & Properties "

        #region " Data Members "

        //This Structure is for the members of the Item

        public struct TeacherInfoStruct
        {
            public int miTeacherId;
            public int miSchoolId;
            public int miUserId;
            public string msTeacherFirstName;
            public string msTeacherMiddleName;
            public string msTeacherLastName;
            public string msDesignation;
            public string msLocalAddress;
            public string msLocalCity;
            public int miLocalPincode;
            public string msLocalStateName;
            public string msPermanentAddress;
            public string msPermanentCity;
            public int miPermanentPincode;
            public string msPermanentState;
            public string msPhoneNumber;
            public string msMobileNumber;
            public DateTime mdtDateofBirth;
            public string msNationality;
            public int miSalutationId;
            public char mcIsLocalAddress;
            public char mcIsTemporary;
            public int miExpInYears;
            public int miExpInMonths;
            public DateTime mdtDateOfJoining;
            public string msAchivements;
            public int miReligionId;
            public int miCategoryId;
            public int miDesignationId;
            public string msCasteSubCaste;
            public DateTime mdtDateOfRetirement;
            public int miInsertedByid;
            public int miUpdatedById;
            public string msSalutation;
            public string msLocalState;
            public string msCategoryName;
            public string msReligionName;
            public int miAcademicYearId;
            public char mcIsAcademicYrApplicable;
            public string msPhotoFilePath;
            public byte[] msBinaryPhotoImage;
            public int miExpDetailsId;
            public string msSchoolName;
            public DateTime mdtJoinDate;
            public DateTime mdtLeftDate;
            public System.DateTime mdtInsertDate;
            public System.DateTime mdtUpdateDate;
			public bool IsFinancialYearApplicable;
            public int AssociatedStandardCategory;
            public int TeacherTypeId;

            public string msPreviousDesignation;
            public decimal msLast_Salary;
            public string msJob_Description;
            public string msReason_for_Leaving;
            public string msDurationDays;

        }
        TeacherInfoStruct moTeacherInfoStruct;

        #endregion

        #region " Properties "

        public TeacherInfoStruct TeacherInfoStructure
        {
            get
            {
                return moTeacherInfoStruct;
            }
            set
            {
                moTeacherInfoStruct = value;
            }
        }

        #endregion

        #endregion

        #region " Overloaded Constructor"

        public SchoolWiseTeacherMasterDC()
        {
            //Default constructor is used to create the object.
            moTeacherInfoStruct.miTeacherId = 0;
        }

        public SchoolWiseTeacherMasterDC(int aiTeacherId)
        {
            // This Overloaded constructor get the parameter as ItemId.
            // And is used to View / Edit the Item.
            LoadTeacherPersonalDetails(aiTeacherId);
        }


        #endregion

        #region " Public Methods "

        /// <summary>
        /// constructs a statement for inserting an item.
        /// </summary>
        /// <returns></returns>

        public string GetTeacherDetailsInsertStatement()
        {
            string sInsertStatement = "INSERT INTO SchoolWise_Teacher_Master (" +
                                  " School_Id " +
                                  ",User_Id " +
                                  ",Teacher_First_Name" +
                                  ",Teacher_Middle_Name " +
                                  ",Teacher_Last_Name " +
                                  ",Local_Address " +
                                  ",Local_City " +
                                  ",Local_Pincode " +
                                  ",Local_State " +
                                  ",Permanent_Address " +
                                  ",Permanent_City " +
                                  ",Permanent_Pincode " +
                                  ",Permanent_State " +
                                  ",Phone_Number " +
                                  ",Mobile_Number " +
                                  ",Date_of_Birth " +
                                  ",Nationality " +
                                  ",Salutation_Id " +
                                  ",Is_LocalAddress" +
                                  ",Is_Temporary " +
                                  ",Exprince_In_Years " +
                                  ",Exprince_In_Months " +
                                  ",Date_Of_Joining " +
                                  ",Achivements " +
                                  ",Religion_Id " +
                                  ",Category_Id " +
                                  ",Designation_Id " +
                                  ",Caste_SubCaste " +
                                  ",Date_of_Retirement " +
                                  ",Inserted_By_id " +
                                  ",Updated_By_Id " +
                                  ",academic_year_id " +
                                  ",TeacherSectionId " +
                                  ",TypeId "+

                " ) VALUES ( " +
                    "   " + moTeacherInfoStruct.miSchoolId +
                    " , " + moTeacherInfoStruct.miUserId +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msTeacherFirstName, false) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msTeacherMiddleName, true) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msTeacherLastName, false) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msLocalAddress, false) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msLocalCity, false) + "' " +
                    " ,  " + moTeacherInfoStruct.miLocalPincode +
                     " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msLocalState, false) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPermanentAddress, true) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPermanentCity, true) + "' " +
                    " ,  " + moTeacherInfoStruct.miPermanentPincode +
                     " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPermanentState, true) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPhoneNumber, true) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msMobileNumber, true) + "' " +
                    " , N'" + StringUtility.ReplaceDefaultDateToNull(moTeacherInfoStruct.mdtDateofBirth).ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msNationality, false) + "' " +
                    " ,  " + moTeacherInfoStruct.miSalutationId +
                    " ,  N'" + moTeacherInfoStruct.mcIsLocalAddress + "' " +
                    " ,  N'" + moTeacherInfoStruct.mcIsTemporary + "' " +
                    " ,  " + moTeacherInfoStruct.miExpInYears +
                    " ,  " + moTeacherInfoStruct.miExpInMonths +
                    " ,  " + (moTeacherInfoStruct.mdtDateOfJoining == DateTime.MinValue ? "NULL" : "N'" + StringUtility.ReplaceDefaultDateToNull(moTeacherInfoStruct.mdtDateOfJoining).ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' ") +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msAchivements, true) + "' " +
                    " ,  " + moTeacherInfoStruct.miReligionId +
                    " ,  " + moTeacherInfoStruct.miCategoryId +
                    " ,  " + moTeacherInfoStruct.miDesignationId +
                    " , N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msCasteSubCaste, true) + "' " +
                    " ,  " + (moTeacherInfoStruct.mdtDateOfRetirement == DateTime.MinValue ? "NULL" : " N'" + StringUtility.ReplaceDefaultDateToNull(moTeacherInfoStruct.mdtDateOfRetirement).ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' ") +
                    " ,  " + moTeacherInfoStruct.miInsertedByid +
                    " ,  " + moTeacherInfoStruct.miUpdatedById +
                    " ,  " + moTeacherInfoStruct.miAcademicYearId +
                    ",   " + moTeacherInfoStruct.AssociatedStandardCategory + 
                    ",   " + moTeacherInfoStruct.TeacherTypeId + 
                " ) ";

            return sInsertStatement;
        }

        /// <summary>
        /// This method is used to get teacher id of selected year.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataTable GetTeacherDetails(int aiSchholId, int aiAcademicYrId, int aiTeacherId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchholId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYear_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Teacher_Id", aiTeacherId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTeacherDetailsOfYear");
            }
        }

        public int InsertTeacherDetails(ArrayList aoArrayListInsertStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction((string[])aoArrayListInsertStatements.ToArray(typeof(string)), Constants.PrimaryKeyRecord.Last);
        }

        public Int32 UpdateTeacherDetails(ArrayList aoArrayListTeacherInfo)
        {
            string sUpdateStatement = "UPDATE SchoolWise_Teacher_Master SET " +
                                  " School_Id =" + moTeacherInfoStruct.miSchoolId +
                                  ",User_Id = " + moTeacherInfoStruct.miUserId +
                                  ",Teacher_First_Name = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msTeacherFirstName, false) + "' " +
                                  ",Teacher_Middle_Name= N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msTeacherMiddleName, true) + "' " +
                                  ",Teacher_Last_Name =N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msTeacherLastName, false) + "' " +
                                  ",Local_Address = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msLocalAddress, false) + "' " +
                                  ",Local_City = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msLocalCity, false) + "' " +
                                  ",Local_Pincode = " + moTeacherInfoStruct.miLocalPincode +
                                  ",Local_State = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msLocalState, false) + "' " +
                                  ",Permanent_Address = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPermanentAddress, true) + "' " +
                                  ",Permanent_City = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPermanentCity, true) + "' " +
                                  ",Permanent_Pincode = " + moTeacherInfoStruct.miPermanentPincode +
                                  ",Permanent_State = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPermanentState, true) + "' " +
                                  ",Phone_Number = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPhoneNumber, true) + "' " +
                                  ",Mobile_Number = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msMobileNumber, true) + "' " +
                                  ",Date_of_Birth = N'" + StringUtility.ReplaceDefaultDateToNull(moTeacherInfoStruct.mdtDateofBirth).ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                                  ",Nationality = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msNationality, false) + "' " +
                                  ",Salutation_Id = " + moTeacherInfoStruct.miSalutationId +
                                  ",Is_LocalAddress = N'" + moTeacherInfoStruct.mcIsLocalAddress + "' " +
                                  ",Is_Temporary = N'" + moTeacherInfoStruct.mcIsTemporary + "' " +
                                  ",Exprince_In_Years =" + moTeacherInfoStruct.miExpInYears +
                                  ",Exprince_In_Months =" + moTeacherInfoStruct.miExpInMonths +
                                  ",Date_Of_Joining =" + (moTeacherInfoStruct.mdtDateOfJoining == DateTime.MinValue ? "NULL" : "N'" + StringUtility.ReplaceDefaultDateToNull(moTeacherInfoStruct.mdtDateOfJoining).ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' ") +
                                 ",Achivements = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msAchivements, true) + "' " +
                                  ",Religion_Id= " + moTeacherInfoStruct.miReligionId +
                                  ",Category_Id= " + moTeacherInfoStruct.miCategoryId +
                                  ",Designation_Id= " + moTeacherInfoStruct.miDesignationId +
                                  ",Caste_SubCaste= N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msCasteSubCaste, true) + "' " +
                                  ",Date_of_Retirement =" + (moTeacherInfoStruct.mdtDateOfRetirement == DateTime.MinValue ? "NULL" : " N'" + StringUtility.ReplaceDefaultDateToNull(moTeacherInfoStruct.mdtDateOfRetirement).ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI) + "' ") +
                                  ",Inserted_By_id =" + moTeacherInfoStruct.miInsertedByid +
                                  ",Updated_By_Id =" + moTeacherInfoStruct.miUpdatedById +
                                  ",TeacherSectionId = "+ moTeacherInfoStruct.AssociatedStandardCategory +
                                  ",TypeId =" + moTeacherInfoStruct.TeacherTypeId +
                             " WHERE " +
                                     " teacher_id = " + moTeacherInfoStruct.miTeacherId +
                                     " AND is_deleted = N'" + Constants.C_NO + "'";
            aoArrayListTeacherInfo.Insert(0, sUpdateStatement);
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListTeacherInfo.ToArray(typeof(string)));
        }


        public Int32 UpdateTeacherMobileNo()
        {
            string sUpdateStatement = "UPDATE SchoolWise_Teacher_Master SET " +
                                            " Mobile_Number = N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msMobileNumber, true) + "' " +
                                            ",Updated_By_Id =" + moTeacherInfoStruct.miUpdatedById +
                                        " WHERE " +
                                            " teacher_id = " + moTeacherInfoStruct.miTeacherId +
                                            " AND is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }


        public Int32 UpdateTeachersAcademicYrApplicable()
        {
            string sUpdateStatement = "UPDATE SchoolWise_Teacher_Master SET " +
                                            " IsAcademicYrApplicable = N'" + moTeacherInfoStruct.mcIsAcademicYrApplicable + "' " +
											",IsFinancialYearApplicable = N'" + moTeacherInfoStruct.IsFinancialYearApplicable + "'" +
                                            ",Updated_By_Id =" + moTeacherInfoStruct.miUpdatedById +
                                        " WHERE " +
                                            " User_Id = " + moTeacherInfoStruct.miUserId +
                                            " AND is_deleted = N'" + Constants.C_NO + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        public DataTable GetAssignedClassTeacher(int aiSchoolId, int aiStandardId, int aiDivisionId)
        {
            string sSelectstatement = " SELECT " +
                                        " vw_BaseTeacherDetails.Teacher_Id " +
                                        ",vw_BaseTeacherDetails.TeacherName" +
                                        ",vw_BaseTeacherDetails.Teacher_Middle_Name " +
                                        ",vw_BaseTeacherDetails.MPT_Applicable " +
                                        ",vw_BaseTeacherDetails.Assembly_Applicable " +
                                     " FROM " +
                                        " vw_BaseTeacherDetails " +
                                     " INNER JOIN " +
                                        " SchoolWise_Standard_Division_Teacher_Assignment_Master " +
                                     " ON " +
                                        " vw_BaseTeacherDetails.Teacher_Id = SchoolWise_Standard_Division_Teacher_Assignment_Master.Teacher_Id " +
                                        " AND vw_BaseTeacherDetails.academic_year_id = SchoolWise_Standard_Division_Teacher_Assignment_Master.Academic_Year_Id " +
                                     " WHERE " +
                                            " vw_BaseTeacherDetails.school_id =" + aiSchoolId +
                                            " AND vw_BaseTeacherDetails.Is_Deleted =N'" + Constants.C_NO + "'" +
                                            " AND  SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                            " AND SchoolWise_Standard_Division_Teacher_Assignment_Master.Standard_Id =" + aiStandardId +
                                            " AND  SchoolWise_Standard_Division_Teacher_Assignment_Master.Division_Id =" + aiDivisionId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectstatement);
        }

        public DataTable GetAssignedClassTeacher(int aiSchoolId, int aiStdDivId)
        {
            string sSelectstatement = " SELECT        vw_BaseTeacherDetails.Teacher_Id, vw_BaseTeacherDetails.TeacherName, vw_BaseTeacherDetails.Teacher_Middle_Name, " +
                                               " vw_BaseTeacherDetails.MPT_Applicable, vw_BaseTeacherDetails.Assembly_Applicable " +
                                 " FROM          SchoolWise_Standard_Division_Teacher_Assignment_Master INNER JOIN " +
                                               " vw_BaseTeacherDetails ON SchoolWise_Standard_Division_Teacher_Assignment_Master.Teacher_Id = vw_BaseTeacherDetails.Teacher_Id AND " +
                                               " SchoolWise_Standard_Division_Teacher_Assignment_Master.School_Id = vw_BaseTeacherDetails.School_Id RIGHT OUTER JOIN " +
                                               " SchoolWise_Standard_Division_Master INNER JOIN " +
                                               " Standard_Master ON SchoolWise_Standard_Division_Master.Standard_Id = Standard_Master.Standard_Id AND " +
                                               " SchoolWise_Standard_Division_Master.School_Id = Standard_Master.School_Id AND " +
                                               " SchoolWise_Standard_Division_Master.academic_year_id = Standard_Master.academic_Year_Id INNER JOIN " +
                                               " Division_Master ON SchoolWise_Standard_Division_Master.Division_Id = Division_Master.Division_Id AND " +
                                               " SchoolWise_Standard_Division_Master.School_Id = Division_Master.School_Id AND " +
                                               " SchoolWise_Standard_Division_Master.academic_year_id = Division_Master.academic_year_id ON " +
                                               " SchoolWise_Standard_Division_Teacher_Assignment_Master.School_Id = SchoolWise_Standard_Division_Master.School_Id AND " +
                                               " SchoolWise_Standard_Division_Teacher_Assignment_Master.Standard_Id = SchoolWise_Standard_Division_Master.Standard_Id AND " +
                                               " SchoolWise_Standard_Division_Teacher_Assignment_Master.Division_Id = SchoolWise_Standard_Division_Master.Division_Id AND " +
                                               " SchoolWise_Standard_Division_Teacher_Assignment_Master.Academic_Year_Id = SchoolWise_Standard_Division_Master.academic_year_id " +
                                 " WHERE         (SchoolWise_Standard_Division_Master.School_Id =" + aiSchoolId + ") AND (Division_Master.Is_Deleted = N'" + Constants.C_NO + "') AND " +
                                                       " (SchoolWise_Standard_Division_Master.Is_Deleted = N'" + Constants.C_NO + "') AND (Standard_Master.Is_Deleted = N'" + Constants.C_NO + "') AND " +
                                                       " (SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_ClassTeacher = N'" + Constants.C_YES + "') AND " +
                                                       " (SchoolWise_Standard_Division_Teacher_Assignment_Master.Is_Deleted = N'" + Constants.C_NO + "') AND " +
                                                       " (SchoolWise_Standard_Division_Master.SchoolWise_Standard_Division_Id =" + aiStdDivId + ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectstatement);

        }

        public void UploadTeacherPhoto(ArrayList aoArrayListUpdateStatements)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((String[])aoArrayListUpdateStatements.ToArray(typeof(string)));            
        }

        public string GetUpdateStaementForPhotoUpload()
        {
            string sUpdateStatement;

            sUpdateStatement = " UPDATE SchoolWise_Teacher_Master SET " +
                               " Photo_file_Path = N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPhotoFilePath, false) + "'" +
                               " WHERE User_Id = " + moTeacherInfoStruct.miUserId +
                               " AND School_Id = " + moTeacherInfoStruct.miSchoolId;

            // Query for list of teacher whose update profile pic only. 
            string sQry = "UPDATE Schoolwise_Teacher_Master " +
                " SET BinaryPhotoImage = @Image , ProfilePicUpdateDate = '" + System.DateTime.Now.ToString() + "'" +
                " WHERE User_Id = " + moTeacherInfoStruct.miUserId +
                " AND School_Id = " + moTeacherInfoStruct.miSchoolId;
            using (SQLServerDbUtility oSQLServerUility = new SQLServerDbUtility())
                oSQLServerUility.ExecuteTransaction(moTeacherInfoStruct.msBinaryPhotoImage, sQry);
            return sUpdateStatement;
        }


        /// <summary>
        /// This method is used to Get User Id from User Name.
        /// </summary>
        /// <param name="asUserName"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public Int32 GetUserIdFromUserName(string asUserName, int aiSchoolId, int aiAcademicYearId)
        {
            int aiReturnUserId = Constants.I_ZERO;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserName", asUserName, SqlDbType.NVarChar);

                using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserIdFromUserNameForFillUserCombo"))
                    if (oSqlDataReader.Read())
                    {
                        aiReturnUserId = oSqlDataReader["UserId"].ToInt();
                    }
                return aiReturnUserId;
            }
        }

        #endregion

        #region Private Methods

        private void LoadTeacherPersonalDetails(int aiTeacherId)
        {
            // This Function is take the ItemId as parameter and populate the data from database. 
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sFetchQuery = FetchTeacherPersonalDataFromDatabase(aiTeacherId);
                using(SqlDataReader oTeacherDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sFetchQuery))
                {
                    if (oTeacherDR != null)
                    {
                        while (oTeacherDR.Read())
                        {
                            moTeacherInfoStruct.miTeacherId = Convert.ToInt32(oTeacherDR["Teacher_Id"]);
                            moTeacherInfoStruct.miUserId = Convert.ToInt32(oTeacherDR["User_Id"]);
                            moTeacherInfoStruct.miSalutationId = Convert.ToInt32(oTeacherDR["Salutation_Id"]);
                            moTeacherInfoStruct.msTeacherFirstName = Convert.ToString(oTeacherDR["Teacher_First_Name"]);
                            if (oTeacherDR["Teacher_Middle_Name"] != DBNull.Value)
                                moTeacherInfoStruct.msTeacherMiddleName = Convert.ToString(oTeacherDR["Teacher_Middle_Name"]);
                            moTeacherInfoStruct.msTeacherLastName = Convert.ToString(oTeacherDR["Teacher_Last_Name"]);
                            moTeacherInfoStruct.msNationality = Convert.ToString(oTeacherDR["Nationality"]);
                            //moTeacherInfoStruct.miCasteId = Convert.ToInt32(oTeacherDR["Caste_Id"]);
                            //moTeacherInfoStruct.miSubCasteId = Convert.ToInt32(oTeacherDR["Sub_caste_Id"]);                 
                            if (oTeacherDR["Phone_Number"] != DBNull.Value)
                                moTeacherInfoStruct.msPhoneNumber = Convert.ToString(oTeacherDR["Phone_Number"]);
                            if (oTeacherDR["Mobile_Number"] != DBNull.Value)
                                moTeacherInfoStruct.msMobileNumber = Convert.ToString(oTeacherDR["Mobile_Number"]);
                            moTeacherInfoStruct.msLocalAddress = Convert.ToString(oTeacherDR["Local_Address"]);
                            moTeacherInfoStruct.msLocalCity = Convert.ToString(oTeacherDR["Local_City"]);
                            moTeacherInfoStruct.miLocalPincode = Convert.ToInt32(oTeacherDR["Local_Pincode"]);
                            moTeacherInfoStruct.msLocalState = Convert.ToString(oTeacherDR["Local_State"]);
                            //  moTeacherInfoStruct.msLocalState = oTeacherDR["LocalState"].ToString();
                            if (oTeacherDR["Permanent_Address"] != DBNull.Value)
                                moTeacherInfoStruct.msPermanentAddress = Convert.ToString(oTeacherDR["Permanent_Address"]);
                            if (oTeacherDR["Permanent_City"] != DBNull.Value)
                                moTeacherInfoStruct.msPermanentCity = Convert.ToString(oTeacherDR["Permanent_City"]);
                            if (oTeacherDR["Permanent_City"] != DBNull.Value)
                                moTeacherInfoStruct.msPermanentCity = Convert.ToString(oTeacherDR["Permanent_City"]);
                            if (oTeacherDR["Permanent_Pincode"] != DBNull.Value)
                                moTeacherInfoStruct.miPermanentPincode = Convert.ToInt32(oTeacherDR["Permanent_Pincode"]);
                            if (oTeacherDR["Permanent_State"] != DBNull.Value)
                                moTeacherInfoStruct.msPermanentState = Convert.ToString(oTeacherDR["Permanent_State"]);
                            if (oTeacherDR["Phone_Number"] != DBNull.Value)
                                moTeacherInfoStruct.msPhoneNumber = Convert.ToString(oTeacherDR["Phone_Number"]);
                            if (oTeacherDR["Mobile_Number"] != DBNull.Value)
                                moTeacherInfoStruct.msMobileNumber = Convert.ToString(oTeacherDR["Mobile_Number"]);
                            moTeacherInfoStruct.mdtDateofBirth = Convert.ToDateTime(oTeacherDR["Date_of_Birth"]);
                            moTeacherInfoStruct.msSalutation = Convert.ToString(oTeacherDR["Salutation_Name"]);
                            moTeacherInfoStruct.mcIsLocalAddress = Convert.ToChar(oTeacherDR["Is_LocalAddress"]);
                            moTeacherInfoStruct.mcIsTemporary = Convert.ToChar(oTeacherDR["Is_Temporary"]);
                            moTeacherInfoStruct.msDesignation = Convert.ToString(oTeacherDR["Teacher_Designation_Name"]);
                            moTeacherInfoStruct.miExpInYears = Convert.ToInt32(oTeacherDR["Exprince_In_Years"]);
                            moTeacherInfoStruct.miExpInMonths = Convert.ToInt32(oTeacherDR["Exprince_In_Months"]);
                            if (oTeacherDR["Date_Of_Joining"] != DBNull.Value)
                                moTeacherInfoStruct.mdtDateOfJoining = Convert.ToDateTime(oTeacherDR["Date_Of_Joining"]);
                            moTeacherInfoStruct.msAchivements = Convert.ToString(oTeacherDR["Achivements"]);
                            moTeacherInfoStruct.miReligionId = Convert.ToInt32(oTeacherDR["Religion_Id"]);
                            moTeacherInfoStruct.miCategoryId = Convert.ToInt32(oTeacherDR["Category_Id"]);
                            moTeacherInfoStruct.miDesignationId = Convert.ToInt32(oTeacherDR["Designation_Id"]);
                            moTeacherInfoStruct.msCasteSubCaste = Convert.ToString(oTeacherDR["Caste_SubCaste"]);
                            moTeacherInfoStruct.msCategoryName = Convert.ToString(oTeacherDR["Category_Name"]);
                            moTeacherInfoStruct.msReligionName = Convert.ToString(oTeacherDR["Religion_Name"]);
                            if (oTeacherDR["Date_of_Retirement"] != DBNull.Value)
                                moTeacherInfoStruct.mdtDateOfRetirement = Convert.ToDateTime(oTeacherDR["Date_of_Retirement"]);
                            if (oTeacherDR["IsAcademicYrApplicable"] != DBNull.Value)
                                moTeacherInfoStruct.mcIsAcademicYrApplicable = Convert.ToChar(oTeacherDR["IsAcademicYrApplicable"]);
                            if (oTeacherDR["Photo_file_Path"] != DBNull.Value)
                                moTeacherInfoStruct.msPhotoFilePath = Convert.ToString(oTeacherDR["Photo_file_Path"]);
                            if (oTeacherDR["IsFinancialYearApplicable"] != DBNull.Value)
                                moTeacherInfoStruct.IsFinancialYearApplicable = Convert.ToBoolean(oTeacherDR["IsFinancialYearApplicable"]);
                        }
                    }
                }
            }
        }

        private string FetchTeacherPersonalDataFromDatabase(int aiTeacherId)
        {
            // This Function is used to fetch the data from the database as per aiItemId. 
            //It returns the dataset.

            string sFetchQuery;
            sFetchQuery = " SELECT " +
                          "  vw_BaseTeacherDetails.Teacher_Id " +
                          ", vw_BaseTeacherDetails.User_Id " +
                          ", vw_BaseTeacherDetails.School_Id " +
                          ", vw_BaseTeacherDetails.Teacher_First_Name " +
                          ", vw_BaseTeacherDetails.Teacher_Middle_Name " +
                          ", vw_BaseTeacherDetails.Teacher_Last_Name " +
                          ", Teacher_Designation_Master.Teacher_Designation_Name " +
                          ", vw_BaseTeacherDetails.Local_Address " +
                          ", vw_BaseTeacherDetails.Local_City " +
                          ", vw_BaseTeacherDetails.Local_Pincode " +
                          ", vw_BaseTeacherDetails.Local_State " +
                          ", vw_BaseTeacherDetails.Permanent_Address " +
                          ", vw_BaseTeacherDetails.Permanent_City " +
                          ", vw_BaseTeacherDetails.Permanent_Pincode " +
                          ", vw_BaseTeacherDetails.Permanent_State " +
                          ", vw_BaseTeacherDetails.Phone_Number " +
                          ", vw_BaseTeacherDetails.Mobile_Number " +
                          ", vw_BaseTeacherDetails.Date_of_Birth " +
                          ", vw_BaseTeacherDetails.Nationality " +
                          ", vw_BaseTeacherDetails.Salutation_Id " +
                          ", vw_BaseTeacherDetails.Is_LocalAddress " +
                          ", vw_BaseTeacherDetails.Is_Temporary " +
                          ", vw_BaseTeacherDetails.Exprince_In_Years " +
                          ", vw_BaseTeacherDetails.Exprince_In_Months " +
                          ", vw_BaseTeacherDetails.Date_Of_Joining " +
                          ", vw_BaseTeacherDetails.Achivements " +
                          ", vw_BaseTeacherDetails.Religion_Id " +
                          ", vw_BaseTeacherDetails.Category_Id " +
                          ", vw_BaseTeacherDetails.Designation_Id " +
                          ", vw_BaseTeacherDetails.Caste_SubCaste " +
                          ", vw_BaseTeacherDetails.Salutation_Name " +
                          ", Category_Master.Category_Name " +
                          ", Religion_Master.Religion_Name " +
                          ", vw_BaseTeacherDetails.Date_of_Retirement " +
                          ", vw_BaseTeacherDetails.IsAcademicYrApplicable " +
                          ", vw_BaseTeacherDetails.Photo_file_Path " +
						  ", vw_BaseTeacherDetails.IsFinancialYearApplicable " +
                    " FROM " +
                        " User_Master " +
                    " INNER JOIN " +
                         " vw_BaseTeacherDetails " +
                         " ON User_Master.User_Id = vw_BaseTeacherDetails.User_Id " +
                    " INNER JOIN  " +
                         " Category_Master " +
                         " ON vw_BaseTeacherDetails.Category_Id  =  Category_Master.Category_Id " +
                    " INNER JOIN  " +
                         " Teacher_Designation_Master " +
                         " ON vw_BaseTeacherDetails.Designation_Id = Teacher_Designation_Master.Teacher_Designation_Id " +
                    " INNER JOIN  " +
                         " Religion_Master " +
                         " ON vw_BaseTeacherDetails.Religion_Id = Religion_Master.Religion_Id ";

            sFetchQuery += " WHERE " +
                                   "    teacher_id = " + aiTeacherId +
                             " AND " +
                                   "   vw_BaseTeacherDetails.is_deleted = N'" + Utility.Constants.C_NO + "'";

            return sFetchQuery;
        }

        public static DataSet FetchTeacherStdSubjectDetails(int aiAcademicYrId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_Teacher_StdSubject");
            }
        }

        public static DataSet FetchAllTeacherDetails(int aiTeacherId, int aiAcademicYrId, int aiSchoolId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iTeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iUserId", aiUserId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_Teacher_Details");
            }
        }

        /// <summary>
        /// Retutns datatable containing limited details to be displayed on control panel.
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataSet GetTeacherDetailsForControlPanel(int aiTeacherId, int aiSchoolId, int aiAcademicYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iAcademicYrId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_iTeacher_Id", aiTeacherId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetTeacherDetailsForControlPanel");
            }
        }
        #endregion

        public DataSet getTeacherIdentityCards(int aiSchoolID, int aiAcademicYrID, int aiTeacherId,string msTeacherName)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolID", aiSchoolID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYrID", aiAcademicYrID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iTeacherId", aiTeacherId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TeacherName", msTeacherName, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_getTeacherIdentityCards");
            }
        }        

        public void InsertExperienceDetails()
        {
            string sTeacherId;
            if (moTeacherInfoStruct.miUserId != 0)
                sTeacherId = "   " + moTeacherInfoStruct.miUserId;
            else
                sTeacherId = "   " + Constants.S_LAST_INSERTED_P_KEY;

            string sInsertStatement=" INSERT INTO TeacherExperienceDetails("+
                                    " User_Id "+
                                    ",School_Id"+
                                    ",SchoolName"+
                                    ",JoiningDate"+
                                    ",leftDate"+
                                    ",Is_Deleted"+
                                    ",InsertDate"+
                                    ",Inserted_By_id"+
                                    ",Update_Date"+
                                    ",Updated_By_Id"+
                                    ",PreviousDesignation"+
                                    ",Last_Salary"+
                                    ",Job_Description"+
                                    ",Reason_for_Leaving"+
                                    ",DurationDays"+
                        ")VALUES( " +
                          sTeacherId +
                        ","+moTeacherInfoStruct.miSchoolId +
                        ",N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msSchoolName, false) + "'" +
                        ",N' "+moTeacherInfoStruct.mdtJoinDate.ToString(Constants.S_DATE_FORMAT_MARATHI)+"'"+
                        ",N' " + moTeacherInfoStruct.mdtLeftDate.ToString(Constants.S_DATE_FORMAT_MARATHI) + "'" +
                        ",N'" + 0 + "'"+
                        ",N' " + moTeacherInfoStruct.mdtInsertDate.ToString(Constants.S_DATE_FORMAT_MARATHI) + "'" +
                        ","+moTeacherInfoStruct.miInsertedByid+
                        ",N' " + moTeacherInfoStruct.mdtUpdateDate.ToString(Constants.S_DATE_FORMAT_MARATHI) + "'" +
                        ","+moTeacherInfoStruct.miUpdatedById+
                        " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPreviousDesignation, true) + "'" +
                          "," +moTeacherInfoStruct.msLast_Salary+
                          " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msJob_Description, true) + "'" +
                           " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msReason_for_Leaving, true) + "'" +
                          " , N'" + Utility.StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msDurationDays, true) + "'" +
                        ")";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);

        }

        public void DeleteExperienceDetails(int iExpDetailsId, string sSchoolName,int iUserId)
        {
            string sDeleteStatement="UPDATE TeacherExperienceDetails "+
                                    " SET"+
                                    " Is_Deleted=1"+
                                    " WHERE " +
                                    " User_Id="+iUserId+
                                    " AND ExperienceDetailsId="+iExpDetailsId+
                                    " AND Is_Deleted=0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);

        }
        /// <summary>
        /// This function is used to duplicate entry of Stop Name.
        /// </summary>
        /// <returns></returns>
        public bool IsDuplicateName()
        {
            string sWhere = "";
            bool bFlag = true;
            if (moTeacherInfoStruct.miExpDetailsId != 0)
            {
                sWhere = " AND ExperienceDetailsId<> '" + moTeacherInfoStruct.miExpDetailsId + "'";
            }
            string sSelectStatement = "SELECT COUNT(*) " +
                " FROM TeacherExperienceDetails " +
                " WHERE Is_Deleted=0" +
                " AND SchoolName= '" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msSchoolName, false) + "'" +
                  sWhere;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int iDuplicateNameCount = oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
                if (iDuplicateNameCount > 0)
                    bFlag = false;
            }
            return bFlag;
        }
        
        public void UpdateExperienceDetails()
        {
            string sUpdateStatement="UPDATE TeacherExperienceDetails "+
                                    " SET"+
                                    " SchoolName= N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msSchoolName, false) + "'" +
                                    " ,JoiningDate=N' " + moTeacherInfoStruct.mdtJoinDate.ToString(Constants.S_DATE_FORMAT_MARATHI) + "'" +
                                    " ,leftDate=N' "+moTeacherInfoStruct.mdtLeftDate+"'"+
                                    ",Update_Date= N'" + DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI) + "' " +
                                    ",Updated_By_Id= " + moTeacherInfoStruct.miUpdatedById +""+
                                     " PreviousDesignation= N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msPreviousDesignation, false) + "'" +
                                      " Last_Salary=" + moTeacherInfoStruct.msLast_Salary + "" +
                                       " Job_Description= N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msJob_Description, false) + "'" +
                                        " Reason_for_Leaving= N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msReason_for_Leaving, false) + "'" +
                                         " DurationDays= N'" + StringUtility.ReplaceSingleQuoteInString(moTeacherInfoStruct.msDurationDays, false) + "'" +
                                    " WHERE Is_Deleted=0" +
                                    " AND School_Id="+moTeacherInfoStruct.miSchoolId +
                                    " AND ExperienceDetailsId="+ moTeacherInfoStruct.miExpDetailsId +"";
                                    using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }
    }

    public class SchoolWiseTeacherMasterCollectionDC : DataCommunicatorBaseDC
    {
        public SchoolWiseTeacherMasterCollectionDC()
        {
        }

        public void InsertMultipleTeachers(int aiSchoolId, int aiAcademicYearId, int aiInsertedById,
                                           string asTeacherDetails, bool abCanPublishUnpublishExam )
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_Id", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Inserted_By_Id", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CanPublishUnpublishExam", abCanPublishUnpublishExam, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("TeacherDetails", asTeacherDetails, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AddMultpleTeacherDetails");
            }
        }
        public bool DeleteMultipleTeacher(ArrayList aoArrDeleteUserIds)
        {
            string sDeleteUserList = "(";
            for (int iCount = 0; iCount < aoArrDeleteUserIds.Count; iCount++)
            {
                sDeleteUserList = sDeleteUserList + aoArrDeleteUserIds[iCount];
                sDeleteUserList = sDeleteUserList + ",";

            }
            sDeleteUserList = sDeleteUserList + ")";
            sDeleteUserList = sDeleteUserList.Remove(sDeleteUserList.Length - 2, 1);

            string sSqlDeleteUser = " UPDATE Schoolwise_Teacher_Master " +
                                 " SET Is_Deleted =N'" + Utility.Constants.C_YES + "'" +
                                 " WHERE Teacher_Id in " + sDeleteUserList;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);
            return true;
        }
        /// <summary>
        /// This method fetches teachers for message facility.
        /// It call SP usp_GetTeachersForMsging which check the user role and returns the dataset accordingly.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiTeacherID"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public static DataTable FetchTeacherDetailsForMessageFacillity(int aiSchoolId, int aiAcademicYrId, int aiTeacherID, int aiUserId, int aiUserRoleId, int aiTypeId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                int IsIncludeCoOrdinators = Constants.I_ZERO;

                if (aiSchoolId == Constants.SchoolId.PPSH.ToInt())
                    IsIncludeCoOrdinators = Constants.I_ONE;

                oSQLServerDbUtility.AddParameter("prm_intSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intAcademicYearID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intUserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intUserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_intTeacher_Id", aiTeacherID, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TypeId", aiTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_IsForMobile", IsIncludeCoOrdinators, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetTeachersForMsging");
            }
        }

        public bool DeleteTeacher(int aiTeacherId,int aiUserId)
        {
            string sSqlDeleteUser = " UPDATE Schoolwise_Teacher_Master " +
                                 " SET Is_Deleted =N'" + Utility.Constants.C_YES + "'" +
                                 " WHERE Teacher_Id =" + aiTeacherId;           
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUser);
            string sSqlDeleteUserBasic = " UPDATE UserBasicDetails " +
                                 " SET IsDeleted =N'" + Utility.Constants.I_ONE + "'" +
                                 " WHERE UserId =" + aiUserId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSqlDeleteUserBasic);
            return true;
        }
         
        /// <summary>
        /// This method delete the configuration for teacher if teachers not available
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiOriginalConfigurationId"></param>
        /// <param name="aiFinancialYearId"></param>

        public static void DeleteTeacherConfiguration(int aiSchoolId, int aiAcademicYearId, int aiOriginalConfigurationId, int aiFinancialYearId)
        {

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iAcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iOriginalConfigurationId", aiOriginalConfigurationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("iFinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteTeachersConfiguration");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiTeacherId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiUserId"></param>
        /// <returns></returns>
        public static DataSet FetchAllTeachers(int aiSchoolId, int aiAcademicYrId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("prm_School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_AcadYr_Id", aiAcademicYrId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_GetAllTeachers");
            }
        }

        /// <summary>
        /// This method is used to get associated standards of a particular teacher.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <returns></returns>
        public static DataTable GetAssociatedStdLstForTeacher(int aiUserId, int aiAcademicYrId)
        {
            string sQuery = " SELECT " +
                                    " dbo.Teacher_Standard_Details.Standard_Id " +
                                    " , dbo.Standard_Master.Original_Standard_Id " +
                                    " , dbo.Teacher_Standard_Details.Teacher_Id " +
                                    " , dbo.Standard_Master.Standard_Name " +
                                    " , dbo.User_Master.User_Id " +
                            " FROM " +
                                    " dbo.Teacher_Standard_Details " +
                                    " INNER JOIN " +
                                    " dbo.Standard_Master " +
                                    " ON " +
                                    " dbo.Teacher_Standard_Details.Standard_Id = dbo.Standard_Master.Standard_Id " +
                                    " INNER JOIN " +
                                    " dbo.SchoolWise_Teacher_Master " +
                                    " ON " +
                                    " dbo.Teacher_Standard_Details.Teacher_Id = dbo.SchoolWise_Teacher_Master.Teacher_Id " +
                                    " AND " +
                                    " dbo.Standard_Master.School_Id = dbo.SchoolWise_Teacher_Master.School_Id " +
                                    " AND " +
                                    " dbo.Standard_Master.academic_Year_Id = dbo.SchoolWise_Teacher_Master.academic_year_id INNER JOIN " +
                                    " dbo.User_Master ON dbo.SchoolWise_Teacher_Master.User_Id = dbo.User_Master.User_Id AND  " +
                                    " dbo.SchoolWise_Teacher_Master.School_Id = dbo.User_Master.School_Id " +
                            " WHERE " +
                                    "dbo.Standard_Master.academic_Year_Id = " + aiAcademicYrId +
                                    " AND " +
                                    " dbo.Standard_Master.Is_Deleted = N'" + Constants.C_NO + "'" +
                                    " AND " +
                                    " dbo.Teacher_Standard_Details.Is_Deleted = N'" + Constants.C_NO + "'" +
                                    " AND " +
                                    " dbo.User_Master.User_Id = " + aiUserId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }

        public static DataTable GetAllTeachersByName(Int32 aiSchoolId, Int32 aiAcademicYrId, string asRegNumbers)
        {
            string sSelectStatement = "SELECT " +
                                               "Teacher_Id" +
                                               ",TeacherName as Name" +
                                               ",vw_BaseTeacherDetails.User_Id as UserId" +
                                               ",Teacher_Designation_Master.Teacher_Designation_Name as Designation" +
                                               ", 0 as StaffGroupId " +
                                               ",0 as MonthId"+
                                               ",0 AS Year "+
                                      " FROM " +
                                               "vw_BaseTeacherDetails  INNER JOIN Teacher_Designation_Master" +
                                      " ON " +
                                               "vw_BaseTeacherDetails.Designation_Id=Teacher_Designation_Master.Teacher_Designation_Id" +
                                               " INNER JOIN User_Master ON vw_BaseTeacherDetails.User_Id=User_Master.User_Id" +
                                      " WHERE " +
                                              " vw_BaseTeacherDetails.School_Id =" + aiSchoolId +
                                              " and User_Master.Is_Locked='N'" +
                                              " and Academic_Year_ID =" + aiAcademicYrId +
                                             " and vw_BaseTeacherDetails.Is_Deleted = 'N' " +
                                             " and (Teacher_First_Name LIKE N'%" + asRegNumbers + "%' OR Teacher_Middle_Name like N'%" + asRegNumbers + "%' OR Teacher_Last_Name like N'%" + asRegNumbers + "%')" +
                                     " ORDER BY Designation_Id,Teacher_First_Name,Teacher_Middle_Name,Teacher_Last_Name";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }

    }
}
