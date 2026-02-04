// Class Name       :- OtherStaffDC
// Purpose          :- This class is used to manage OtherStaff details.
// Date Of creation :- 12/5/2009
// Author Name      :- Deepak

using System;
using System.Data;
using System.Data.SqlClient;
using PayrollEntities;
using Utility;

namespace DataCommunicator
{
    public class OtherStaffDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miUserId;
        private OtherStaff moOtherStaff; 

        #endregion        
        
        #region Constructor(s)

        public OtherStaffDC()
        {
            this.moOtherStaff = new OtherStaff();
        }

        public OtherStaffDC(int aiSchoolId, int aiUserId)
        {
            this.miSchoolId = aiSchoolId;
            this.miUserId = aiUserId;
            this.moOtherStaff = new OtherStaff();
        } 

        #endregion

        #region Property(s)

        public OtherStaff OtherStaff
        {
            get { return moOtherStaff; }
            set { moOtherStaff = value; }
        } 

        #endregion

        #region Method(s)

        public static DataTable GetAll(int aiSchoolId, string sSortExpression, int iEndIndex, int startRowIndex, int aiUserTypeId, string asFilter)
        {
            if (sSortExpression == string.Empty || sSortExpression == "Name" || sSortExpression == "Name ASC")
                sSortExpression = "Teacher_Designation_master.SortOrder, FirstName,MiddleName,LastName";
            else if (sSortExpression == "Name DESC")
                sSortExpression = "Teacher_Designation_master.SortOrder DESC , FirstName DESC,MiddleName DESC,LastName DESC";
          
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StartIndex", startRowIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", iEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", " ORDER BY " + sSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserTypeId", aiUserTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);  //
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetPagedOtherStaff");
            }
        }
       
     
         public static int CountTotalOtherStaff(int aiSchoolId, string sortExpression, int maximumRows, int startRowIndex, int aiUserType,string asFilter)
              {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("iSchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserTypeId", aiUserType, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("Cnt", 0, SqlDbType.Int, ParameterDirection.Output);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);  // 
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetCountOtherStaff");
                return Convert.ToInt32(oSqlParameter.Value);
            }
        }

        // This function is used to insert the OtherStaff Details
        public int Insert(string asUserName, string asPassword, string asIsLocked)
        {
            string[] sQueryString = new string[3];
            int iStaffId;
            int iStaffUserId = 0;
            if (this.moOtherStaff.DateOfBirth.ToString() != Constants.S_EMPTY_STRING)
            {
                sQueryString[0] = " INSERT INTO User_Master ( " +
                                               "  School_Id " +
                                               " , User_Role_Id " +
                                               " , User_Login " +
                                               " , Email_Address " +
                                               " , User_Password " +
                                               " , Inserted_By_id " +
                                               " , Updated_By_Id " +
                                               " , User_First_Name " +
                                               " , User_Middle_Name " +
                                               " , User_Last_Name " +
                                               " , Address " +
                                               " , Salutation_Id " +
                                               " , Mobile_Number " +
                                               " , EmergencyContactNumber " +
                                               " , DOB " +
                                               " , Is_Locked " +
                                                " , ShowOnStaffBirthday " +
                                         " ) VALUES ( " +
                                               this.miSchoolId +
                                               " ,  " + Convert.ToInt32(Constants.UserRoles.OtherStaff) +
                                               " , N'" + asUserName + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.EmailId, true) + "'" +
                                               " , N'" + asPassword + "'" +
                                               " , " + this.miUserId +
                                               " , " + this.miUserId +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.FirstName, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.MiddleName, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.LastName, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.Address, false) + "'" +
                                               " ,  " + Convert.ToString(this.moOtherStaff.SalutationId) +
                                               " , N'" + this.moOtherStaff.MobileNo + "'" +
                                               " , N'" + this.moOtherStaff.EmergencyNo + "'" +
                                               " , N'" + this.moOtherStaff.DateOfBirth + "'" +
                                               " , N'" + asIsLocked + "'" +
                                                " , N'" + Constants.I_ONE + "'" +
                                         " ) ";
            }
            else
            {
                sQueryString[0] = " INSERT INTO User_Master ( " +
                                               "  School_Id " +
                                               " , User_Role_Id " +
                                               " , User_Login " +
                                               " , Email_Address " +
                                               " , User_Password " +
                                               " , Inserted_By_id " +
                                               " , Updated_By_Id " +
                                               " , User_First_Name " +
                                               " , User_Middle_Name " +
                                               " , User_Last_Name " +
                                               " , Address " +
                                               " , Salutation_Id " +
                                               " , Mobile_Number " +
                                               " , EmergencyContactNumber " +
                                                 " , ShowOnStaffBirthday " +
                                         " ) VALUES ( " +
                                               this.miSchoolId +
                                               " ,  " + Convert.ToInt32(Constants.UserRoles.OtherStaff) +
                                               " , N'" + asUserName + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.EmailId, true) + "'" +
                                               " , N'" + asPassword + "'" +
                                               " , " + this.miUserId +
                                               " , " + this.miUserId +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.FirstName, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.MiddleName, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.LastName, true) + "'" +
                                               " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.Address, true) + "'" +
                                               " ,  " + Convert.ToString(this.moOtherStaff.SalutationId) +
                                               " , N'" + this.moOtherStaff.MobileNo + "'" +
                                               " , N'" + this.moOtherStaff.EmergencyNo + "'" +
                                                " , N'" + Constants.I_ONE + "'" +
                                        " ) ";
            }

            sQueryString[1] = "SELECT SCOPE_IDENTITY() as " + Constants.S_LAST_INSERTED_P_KEY;
            string sConvertedImage = CommonUtility.ConvertImageToHex(this.moOtherStaff.BinaryFormatPhoto);

            sQueryString[2] = "INSERT INTO OtherStaff(" +
                                "SalutationId" +
                                ",FirstName" +
                                ",MiddleName" +
                                ",LastName" +
                                ",Address " +
                                ",DateOfBirth" +
                                ",MobileNo" +
                                ",EmailId" +
                                ",DesignationId" +
                                ",UserId" +
                                ",SchoolId" +
                                ",InsertedDate" +
                                ",InsertedById" +
                                ",UpdatedDate" +
                                ",UpdatedById" +
                                ",PhotoFilePath" +
                                ",BinaryPhotoImage" +
                                ",ProfilePicUpdateDate" + 
                                ")VALUES(" +
                                " " + this.moOtherStaff.SalutationId +
                                 " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.FirstName, false) + "' " +
                                 " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.MiddleName, false) + "' " +
                                 " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.LastName, false) + "' " +
                                 " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.Address, false) + "' " +
                                 " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.DateOfBirth, false) + "' " +
                                 " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.MobileNo, false) + "' " +
                                 " , N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.EmailId, false) + "' " +
                                 " , " + this.moOtherStaff.DesignationId +
                                 " , " + Constants.S_LAST_INSERTED_P_KEY +
                                 " , " + this.miSchoolId +
                                 " , N'" + DateTime.Now.ToString("MM/dd/yyyy") + "' " +
                                 " , " + this.miUserId +
                                 " , N'" + DateTime.Now.ToString("MM/dd/yyyy") + "' " +
                                 " , " + this.miUserId +
                                 " , N'" + this.moOtherStaff.PhotoFilePath + "' " +
                                 " , @Image " +
                                 ", N'" + DateTime.Now.ToString() + "' " +
                                ")";

            if (sConvertedImage != string.Empty)
                sQueryString[2] = sQueryString[2].Replace("@Image", sConvertedImage);
            else
                sQueryString[2] = sQueryString[2].Replace("@Image", "null");

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                iStaffId = oSQLServerDbUtility.ExecuteTransaction(sQueryString, Constants.PrimaryKeyRecord.Last);

            string SqlStatement = "SELECT UserId FROM OtherStaff Where OtherStaffId=N'" + iStaffId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(SqlStatement))
                {
                    if (oSqlDataReader.Read())
                        iStaffUserId = Convert.ToInt32(oSqlDataReader["UserId"]);
                }
            }

            AssignDefaultScreens(iStaffUserId);

            return iStaffUserId;
        }

        /// <summary>
        /// This method is used to add default screens / reports to other staff.
        /// </summary>
        /// <param name="iStaffUserId"></param>
        private void AssignDefaultScreens(int iStaffUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", iStaffUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_AssignExtraScreensToOtherStaff");
            }
        }

        // This function is used to update the OtherStaff Details
        public void Update()
        {
            string sUpdateStatement = "UPDATE OtherStaff SET " +
            "SalutationId= " + this.moOtherStaff.SalutationId +
            ",FirstName= N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.FirstName, false) + "' " +
            ",MiddleName= N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.MiddleName, false) + "' " +
            ",LastName= N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.LastName, false) + "' " +
            ",Address= N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.Address, false) + "'" +
            ",DateOfBirth= N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.DateOfBirth, false) + "' " +
            ",MobileNo= N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.MobileNo, false) + "' " +
            ",EmailId= N'" + StringUtility.ReplaceSingleQuoteInString(this.moOtherStaff.EmailId, false) + "' " +
            ",DesignationId= " + this.moOtherStaff.DesignationId +
            ",UpdatedDate= N'" + DateTime.Now.ToShortDateString() + "' " +
            ",UpdatedById= " + this.miUserId +           
            " WHERE OtherStaffId=" + this.moOtherStaff.OtherStaffId +
            " AND " +
            "SchoolId=" + this.miSchoolId +
            " AND Is_Deleted=N'N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        // This function is used to delete the OtherStaff Details
        public void Delete()
        {
            string sDeleteStatement = "DELETE OtherStaff WHERE OtherStaffId=N'" + this.moOtherStaff.OtherStaffId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        public void Delete(int aiOtherStaffId, int aiUserId)
        {
            string sDeleteStatement = "UPDATE OtherStaff" +
                                      " SET " +
                                      "Is_Deleted = N'Y'" +
                                      ",UpdatedDate= N'" + DateTime.Now.ToShortDateString() + "' " +
                                      ",UpdatedById= " + this.miUserId +   
                                      " WHERE " +
                                      "OtherStaffId=" + aiOtherStaffId +
                                      " AND SchoolId=" + this.miSchoolId +
                                      " AND Is_Deleted = N'N'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
            string sDeleteBasicDetails = "UPDATE UserBasicDetails set Isdeleted =N'" + Constants.I_ONE + "'" +
                ",UpdateDate= N'" + DateTime.Now.ToShortDateString() + "' " +
                ",UpdatedById= " + this.miUserId +   
                " WHERE UserId =" + aiUserId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteBasicDetails);
        }

        // This function is used to load the OtherStaff Details
        public OtherStaff Get(int aiOtherStaffId)
        {
            OtherStaff oOtherStaff = new OtherStaff();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = this.FetchDetailsFromDatabase(aiOtherStaffId);
                using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        if (oDR.Read())
                        {
                            if (oDR["OtherStaffId"] != DBNull.Value)
                                oOtherStaff.OtherStaffId = Convert.ToInt32(oDR["OtherStaffId"]);
                            if (oDR["SalutationId"] != DBNull.Value)
                                oOtherStaff.SalutationId = Convert.ToInt32(oDR["SalutationId"]);
                            if (oDR["FirstName"] != DBNull.Value)
                                oOtherStaff.FirstName = Convert.ToString(oDR["FirstName"]);
                            if (oDR["MiddleName"] != DBNull.Value)
                                oOtherStaff.MiddleName = Convert.ToString(oDR["MiddleName"]);
                            if (oDR["LastName"] != DBNull.Value)
                                oOtherStaff.LastName = Convert.ToString(oDR["LastName"]);
                            if (oDR["Address"] != DBNull.Value)
                                oOtherStaff.Address = Convert.ToString(oDR["Address"]);
                            if (oDR["DateOfBirth"] != DBNull.Value)
                                oOtherStaff.DateOfBirth = Convert.ToString(oDR["DateOfBirth"]);
                            if (oDR["MobileNo"] != DBNull.Value)
                                oOtherStaff.MobileNo = Convert.ToString(oDR["MobileNo"]);
                            if (oDR["EmailId"] != DBNull.Value)
                                oOtherStaff.EmailId = Convert.ToString(oDR["EmailId"]);
                            if (oDR["DesignationId"] != DBNull.Value)
                                oOtherStaff.DesignationId = Convert.ToInt32(oDR["DesignationId"]);
                            if (oDR["UserId"] != DBNull.Value)
                                oOtherStaff.UserId = Convert.ToInt32(oDR["UserId"]);
                            if (oDR["PhotoFilePath"] != DBNull.Value)
                                oOtherStaff.PhotoFilePath = oDR["PhotoFilePath"].ToString();
                            if (oDR["EmergencyContactNumber"] != DBNull.Value)
                                oOtherStaff.EmergencyNo = oDR["EmergencyContactNumber"].ToString();
                            if (oDR["BinaryPhotoImage"] != DBNull.Value)
                                oOtherStaff.BinaryFormatPhoto = oDR["BinaryPhotoImage"] as byte[];
                        }
                    }
                }
                return oOtherStaff;
            }
        }

        public DataTable GetAll()
        {
            string sStatement = " SELECT  * " +

              " FROM OtherStaff" +
              " WHERE " +
              " SchoolId = " + this.miSchoolId +
              " AND Is_Deleted=N'N'";

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sStatement);
        }

        // This function is used to fetch the OtherStaff Details
        private string FetchDetailsFromDatabase(int aiOtherStaffId)
        {
            string sSelectStatement = " SELECT  " +
                                      "OtherStaffId" +
                                      ",SalutationId" +
                                      ",FirstName" +
                                      ",MiddleName" +
                                      ",LastName" +
                                      ",OtherStaff.Address " +
                                      ",DateOfBirth" +
                                      ",MobileNo" +
                                      ",EmailId" +
                                      ",OtherStaff.DesignationId" +
                                      ",UserId" +
                                      ",OtherStaff.PhotoFilePath" +
                                      ",OtherStaff.BinaryPhotoImage" +
                                      ", User_Master.EmergencyContactNumber " +
                                      " FROM OtherStaff" +
                                      " INNER JOIN User_Master " +
                                      " ON OtherStaff.UserId=User_Master.User_Id " +
                                      " AND OtherStaff.SchoolId=User_Master.School_Id" +
                                      " WHERE OtherStaffId=" + aiOtherStaffId +
                                      " AND SchoolId = " + this.miSchoolId +
                                      " AND OtherStaff.Is_Deleted=N'N'" +
                                      " AND User_Master.Is_Deleted=N'N'";
            return sSelectStatement;
        } 

        #endregion

        public DataSet GetOtherStaffDetailsForControlPanel(int aiUserId, int aiSchoolId, int aiAcademicYearId)
        {
            string sUSPName = "[usp_GetOtherStaffDetailsForControlPanel]";
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);                
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet(sUSPName);
            }
        }
    }
}
