// Class Name       :- UserStaffGroupAssociationDC
// Purpose          :- This class is used to manage OtherStaffGroupAssociation details.
// Date Of creation :- 11/10/2009
// Author Name      :- Sachin

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using PayrollEntities;
using Utility;
using SchoolEntities;

namespace DataCommunicator
{
    public class UsersStaffGroupsAssociationDC
    {
        #region "Data Members"

        private int miSchoolId = 0;
        private int miAcademicYearId = 0;
        private UsersSGAssociation moUsersSGAssociation;        
        private UsersStaffGroupsAssociationDC moUsersStaffGroupsAssociationDC = null;        
        private List<UsersSGAssociation> mlstUsersSGAssociation = new List<UsersSGAssociation>();
        private Insurance moInsurances = new Insurance();        
        private List<UsersInsuranceDependent> molstInsuranceDependents = new List<UsersInsuranceDependent>();
        private List<Salutations> lstSalutationsForName = new List<Salutations>();

        #endregion "Data Members"

        #region "Constructors"
        
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public UsersStaffGroupsAssociationDC()
        {
        }

        /// <summary>
        /// Initializes member variables.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        public UsersStaffGroupsAssociationDC(int aiSchoolId, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miAcademicYearId = aiAcademicYearId;
            moUsersStaffGroupsAssociationDC = new UsersStaffGroupsAssociationDC();
        }

        #endregion "Constructors"

        #region "Properties"

        public Insurance Insurances
        {
            get { return moInsurances; }
            set { moInsurances = value; }
        }

        public List<UsersInsuranceDependent> InsuranceDependents
        {
            get { return molstInsuranceDependents; }
            set { molstInsuranceDependents = value; }
        }

        public List<Salutations> SalutationsForName
        {
            get { return lstSalutationsForName; }
            set { lstSalutationsForName = value; }
        }

        public UsersSGAssociation UsersSGAssociation
        {
            set { moUsersSGAssociation = value; }
        }

        public List<UsersSGAssociation> UsersSGAssociations
        {
            get { return mlstUsersSGAssociation; }
            set { mlstUsersSGAssociation = value;  }
        }

        #endregion "Properties"

        #region "Public Methods"

        /// <summary>
        /// This method is used to reurn staff group and roles.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static DataSet GetStaffGroupsAndRoles(int aiSchoolId)
        {
            StringBuilder sSelectStatement = new StringBuilder();
            sSelectStatement.Append("SELECT " +
                                  "User_Role_Id" +
                                  ",User_Role_Name" +
                                  "	FROM " +
                                  "User_Role_Master" +
                                  "	WHERE " +
                                  "Is_Deleted = N'N'" +
                                 " AND User_Role_Id NOT IN(" +
                                  Convert.ToInt32(Constants.UserRoles.Student) + "," +
                                  Convert.ToInt32(Constants.UserRoles.TransportStaff) + "," +
                                  //Convert.ToInt32(Constants.UserRoles.ExAdmin) + "," +
                                  Convert.ToInt32(Constants.UserRoles.Parent) +
                                  ");");

            sSelectStatement.Append("SELECT " +
                                       "StaffGroupsId" +
                                       ",StaffGroupsName" +
                                       ",OriginalStaffGroupsId" +
                                       " FROM " +
                                       "StaffGroups" +
                                       " WHERE " +
                                       "Is_Deleted = N'N'" +
                                       " AND SchoolId = " + aiSchoolId +
                                    " ORDER BY " +
                                       "OriginalStaffGroupsId");

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sSelectStatement.ToString());
        }

        /// <summary>
        /// This method is used to return user details.
        /// </summary>
        public DataTable GetUserDetails(int aiUserRoleId, string asUserNames, int aiUserTypeId, bool abWithSalutation)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserName", asUserNames, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UserTypeId", aiUserTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WithSalutation", abWithSalutation, SqlDbType.Bit);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUserStaffGroupAssociation");
            }
        }

        /// <summary>
        /// This method is used to insert/edit/delete association.
        /// </summary>
        public DataSet Save(int aiLeaveSeperaterDay)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moUsersSGAssociation.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserXml", moUsersSGAssociation.UserXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("LeaveSeperaterDay", aiLeaveSeperaterDay, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("usp_InsertUserStaffGroupAsso",true);
            }
        }

        /// <summary>
        /// This method is used to local/unlock user from payroll module.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiIsLocked"></param>
        public void LockUnlocksalaryUser(int aiUserId, int aiSchoolId, bool aiIsLocked, int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsLocked", aiIsLocked, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_LockUnlocksalaryUser");
            }
        }

        /// <summary>
        /// This method is used to return user's insurance details.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SchoolId"></param>
        public void GetUserInsuranceDetails(int UserId, int SchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", UserId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserInsuranceDetails"))
                {
                    while (oSqlDataReader.Read())
                    {
                        Salutations oSalutationse = new Salutations
                        {
                            SalutationId = Convert.ToInt32(oSqlDataReader["salutation_id"]),
                            SalutationName = Convert.ToString(oSqlDataReader["salutation_name"])
                        };
                        lstSalutationsForName.Add(oSalutationse);
                    }

                    if (oSqlDataReader.NextResult())
                    {
                        if (oSqlDataReader.Read())
                        {
                            moInsurances = new Insurance
                            {
                                InsuranceAmount = (oSqlDataReader["InsuranceAmount"] != DBNull.Value) ? Convert.ToDecimal(oSqlDataReader["InsuranceAmount"]) : Convert.ToDecimal(0),
                                UserStatus = (oSqlDataReader["UserStatus"] != DBNull.Value) ? Convert.ToInt32(oSqlDataReader["UserStatus"]) : 0,
                                InsuranceCardNumber = (oSqlDataReader["InsuranceCardNumber"] != DBNull.Value) ? oSqlDataReader["InsuranceCardNumber"].ToString() : string.Empty,
                            };
                        }
                    }

                    if (oSqlDataReader.NextResult())
                    {
                        while (oSqlDataReader.Read())
                        {
                            UsersInsuranceDependent oInsuranceDependent = new UsersInsuranceDependent
                            {
                                UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                                UsersInsuranceDependentId = Convert.ToInt32(oSqlDataReader["UsersInsuranceDependentId"]),
                                SalutationId = Convert.ToInt32(oSqlDataReader["SalutationId"]),
                                Relation = Convert.ToString(oSqlDataReader["Relation"]),
                                FirstName = Convert.ToString(oSqlDataReader["FirstName"]),
                                MiddleName = Convert.ToString(oSqlDataReader["MiddleName"]),
                                LastName = Convert.ToString(oSqlDataReader["LastName"]),
                                InsuranceCardNumber = Convert.ToString(oSqlDataReader["InsuranceCardNumber"]),
                                Name = oSqlDataReader["Name"].ToString(),
                                DateOfBirth = Convert.ToDateTime(oSqlDataReader["DateOfBirth"]),

                            };
                            molstInsuranceDependents.Add(oInsuranceDependent);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to return user insurance anount and status.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SchoolId"></param>
        /// <returns></returns>
        public static Insurance GetUserInsuranceAmountAndStatus(int UserId, int SchoolId)
        {
            Insurance oUserInsurance = new Insurance();
            string sSelectStatment = " SELECT  " +
                                     "InsuranceAmount  " +
                                     " , UserStatus   " +
                                     " FROM " +
                                     " UsersStaffGroupsAssociation " +
                                   " WHERE " +
                                       " UserId =  " + UserId +
                                   " AND SchoolId = " + SchoolId +
                                   " AND Is_Deleted = N'N'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatment))
                {

                    if (oSqlDataReader.Read())
                    {
                        Insurance oInsurance = new Insurance
                        {
                            InsuranceAmount = (oSqlDataReader["InsuranceAmount"] != DBNull.Value) ? Convert.ToDecimal(oSqlDataReader["InsuranceAmount"]) : Convert.ToDecimal(0),
                            UserStatus = (oSqlDataReader["UserStatus"] != DBNull.Value) ? Convert.ToInt32(oSqlDataReader["UserStatus"]) : 0,
                        };
                        oUserInsurance = oInsurance;
                    }
                    return oUserInsurance;
                }
            }
        }
        /// <summary>
        /// This method is used to return user dependent details.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SchoolId"></param>
        /// <returns></returns>
        public static List<UsersInsuranceDependent> GetUserDependentDetails(int UserId, int SchoolId)
        {
            List<UsersInsuranceDependent> oUsersInsuranceDependent = new List<UsersInsuranceDependent>();
            string sSelectStatment = "SELECT UsersInsuranceDependentId," +
                                     "UserId, " +
                                     "DateOfBirth," +
                                     "Relation, " +
                                     "(Salutation_Master.Salutation_Name+N' '+FirstName+N' '+CASE MiddleName WHEN  N'' THEN N'' ELSE MiddleName+N'. 'END + LastName) as Name," +
                                     " InsuranceCardNumber" +
                                    " FROm UsersInsuranceDependent INNER JOIN Salutation_Master ON UsersInsuranceDependent.SalutationId=Salutation_Master.Salutation_Id" +
                                    " WHERE UserId=" + UserId +
                                    " AND UsersInsuranceDependent.Is_Deleted=0";


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatment))
                {
                    while (oSqlDataReader.Read())
                    {
                        UsersInsuranceDependent oInsuranceDependent = new UsersInsuranceDependent
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            UsersInsuranceDependentId = Convert.ToInt32(oSqlDataReader["UsersInsuranceDependentId"]),
                            Relation = Convert.ToString(oSqlDataReader["Relation"]),
                            Name = Convert.ToString(oSqlDataReader["Name"]),
                            InsuranceCardNumber = Convert.ToString(oSqlDataReader["InsuranceCardNumber"]),
                            DateOfBirth = Convert.ToDateTime(oSqlDataReader["DateOfBirth"]),

                        };
                        oUsersInsuranceDependent.Add(oInsuranceDependent);
                    }
                    return oUsersInsuranceDependent;
                }
            }
        }
        /// <summary>
        /// This method is used to save insurance details.
        /// </summary>
        /// <param name="aiAmount"></param>
        /// <param name="Status"></param>
        /// <param name="UserId"></param>
        /// <param name="asInsuranceCardNumber"></param>
        public static void SaveInsuranceDetails(decimal aiAmount, int Status, int UserId, string asInsuranceCardNumber, int aiUpdatedById)
        {
            string sUpdateStatement = " UPDATE UsersStaffGroupsAssociation SET " +
                                     "InsuranceAmount  = " + aiAmount +
                                     " , UserStatus  = " + Status +
                                     " , InsuranceCardNumber=" + " N'" + asInsuranceCardNumber + "' " +
                                     ", UpdateDate=GETDATE()"+
                                     ", UpdatedById="+aiUpdatedById+
                                   " WHERE " +
                                       " UserId =  " + UserId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }

        /// <summary>
        /// This method is used to remove insurance details.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SchoolId"></param>
        public static void RemoveOldInsuranceDetails(int UserId, int SchoolId, int aiUpdatedById)
        {
            string sUpdateStatement = " UPDATE UsersStaffGroupsAssociation SET " +
                                      "InsuranceAmount  = NULL" +
                                      " , InsuranceCardNumber=NULL" +
                                      " , UserStatus  = NULL" +
                                      ", UpdateDate=GETDATE()" +
                                      ", UpdatedById=" + aiUpdatedById +
                                      " WHERE " +
                                      " UserId =  " + UserId +
                                      " AND SchoolId =" + SchoolId;

            sUpdateStatement += " UPDATE UsersInsuranceDependent SET " +
                                "Is_Deleted  = 1" +
                                 ", UpdateDate=GETDATE()" +
                                 ", UpdatedById=" + aiUpdatedById +
                                " WHERE " +
                                " UserId =  " + UserId +
                                " AND School_Id =" + SchoolId +
                                " AND Relation = N'Spouse'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to save dependent details.
        /// </summary>
        /// <param name="oUsersInsuranceDependent"></param>
        /// <param name="UserId"></param>
        /// <param name="SchoolId"></param>
        /// <param name="InsertedById"></param>
        public static void InsertDependentDetails(UsersInsuranceDependent oUsersInsuranceDependent, int UserId, int SchoolId, int InsertedById)
        {
            string sInsertStatement = "INSERT INTO UsersInsuranceDependent(" +
               "UserId" +
               ",SalutationId" +
               ",FirstName" +
               ",MiddleName" +
               ",LastName" +
               ",InsuranceCardNumber" +
               ",DateOfBirth" +
               ",Relation" +
               ",School_Id" +
               ",InsertedById" +
               ",UpdatedById" +

               ")VALUES(" + UserId +
               " , " + oUsersInsuranceDependent.SalutationId +
               " , " + " N'" + StringUtility.ReplaceSingleQuoteInString(oUsersInsuranceDependent.FirstName, false) + "' " +
              " , " + " N'" + StringUtility.ReplaceSingleQuoteInString(oUsersInsuranceDependent.MiddleName, false) + "' " +
              " , " + " N'" + StringUtility.ReplaceSingleQuoteInString(oUsersInsuranceDependent.LastName, false) + "' " +
               " , N'" + Convert.ToString(oUsersInsuranceDependent.InsuranceCardNumber) + "'" +
               " , N'" + Convert.ToString(oUsersInsuranceDependent.DateOfBirth) + "'" +
               " , N'" + StringUtility.ReplaceSingleQuoteInString(oUsersInsuranceDependent.Relation, false) + "'" +
               " , " + SchoolId +
               " , " + InsertedById +
                " , " + InsertedById + ")";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);

        }
        
        /// <summary>
        /// This method is used to update dependent details.
        /// </summary>
        /// <param name="oUsersInsuranceDependent"></param>
        /// <param name="InsertedById"></param>
        /// <param name="UsersInsuranceDependentId"></param>
        public static void UpdateDependentDetails(UsersInsuranceDependent oUsersInsuranceDependent, int InsertedById, int UsersInsuranceDependentId)
        {
            string sUpdateStatement = " UPDATE UsersInsuranceDependent SET " +
                                     "SalutationId  = " + oUsersInsuranceDependent.SalutationId +
                                     " , FirstName  = " + " N'" + StringUtility.ReplaceSingleQuoteInString(oUsersInsuranceDependent.FirstName, false) + "' " +
                                      " , MiddleName  = " + " N'" + StringUtility.ReplaceSingleQuoteInString(oUsersInsuranceDependent.MiddleName, false) + "' " +
                                       " , LastName  = " + " N'" + StringUtility.ReplaceSingleQuoteInString(oUsersInsuranceDependent.LastName, false) + "' " +
                                       ",InsuranceCardNumber= " + " N'" + oUsersInsuranceDependent.InsuranceCardNumber + "' " +
                                       " , DateOfBirth  = " + " N'" + StringUtility.ReplaceSingleQuoteInString(Convert.ToString(oUsersInsuranceDependent.DateOfBirth), false) + "' " +
                                       " , Relation  = " + " N'" + StringUtility.ReplaceSingleQuoteInString(oUsersInsuranceDependent.Relation, false) + "' " +
                                       " , UpdatedById  = " + InsertedById +
                                       ", UpdateDate = GETDATE()" +

                                   " WHERE " +
                                       " UsersInsuranceDependentId =  " + UsersInsuranceDependentId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }

        /// <summary>
        /// This method is used to delete dependent details.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SchoolId"></param>
        public static void DeleteDependentDetails(int UserId, int SchoolId, int aiUpdatedById)
        {
            string sUpdateStatement = " UPDATE UsersInsuranceDependent SET " +
                                    "Is_Deleted  = 1" +
                                     ", UpdatedById  = " + aiUpdatedById +
                                     ", UpdateDate = GETDATE()" +
                                  " WHERE " +
                                      " UserId =  " + UserId +
                                  " AND School_Id =" + SchoolId +                                  
                                  " AND Relation <> N'Spouse'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to delete dependents.
        /// </summary>
        /// <param name="iUsersInsuranceDependentId"></param>
        /// <param name="iSchoolID"></param>
        public static void DeleteDependent(int iUsersInsuranceDependentId, int iSchoolID, int aiUpdatedById)
        {
            string sUpdateStatement = " UPDATE UsersInsuranceDependent SET " +
                                      " Is_Deleted  = 1" +
                                       ", UpdatedById  = " + aiUpdatedById +
                                       ", UpdateDate = GETDATE()" +
                                      " WHERE " +
                                      " UsersInsuranceDependentId =  " + iUsersInsuranceDependentId +
                                      " AND School_Id =" + iSchoolID +
                                      " AND Is_Deleted = 0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);

        }

        /// <summary>
        /// This method is used to reurn dependen details.
        /// </summary>
        /// <param name="iUsersInsuranceDependentId"></param>
        /// <param name="iSchoolID"></param>
        /// <returns></returns>
        public static UsersInsuranceDependent GetDependent(int iUsersInsuranceDependentId, int iSchoolID)
        {
            UsersInsuranceDependent oUsersInsuranceDependent = new UsersInsuranceDependent();
            string sSelectStatment = " SELECT UsersInsuranceDependentId," +
                                     " UserId," +
                                     " SalutationId," +
                                     " FirstName," +
                                     " MiddleName," +
                                     " LastName," +
                                     "InsuranceCardNumber," +
                                     " DateOfBirth," +
                                     " Relation " +
                                     " FROM UsersInsuranceDependent  " +
                                     " WHERE " +
                                     " UsersInsuranceDependentId =  " + iUsersInsuranceDependentId +
                                     " AND School_Id = " + iSchoolID +
                                     " AND Is_Deleted = 0";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatment))
                {
                    while (oSqlDataReader.Read())
                    {
                        UsersInsuranceDependent oInsuranceDependent = new UsersInsuranceDependent
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            UsersInsuranceDependentId = Convert.ToInt32(oSqlDataReader["UsersInsuranceDependentId"]),
                            SalutationId = Convert.ToInt32(oSqlDataReader["SalutationId"]),
                            Relation = Convert.ToString(oSqlDataReader["Relation"]),
                            FirstName = Convert.ToString(oSqlDataReader["FirstName"]),
                            MiddleName = Convert.ToString(oSqlDataReader["MiddleName"]),
                            LastName = Convert.ToString(oSqlDataReader["LastName"]),
                            InsuranceCardNumber = Convert.ToString(oSqlDataReader["InsuranceCardNumber"]),
                            DateOfBirth = Convert.ToDateTime(oSqlDataReader["DateOfBirth"]),

                        };
                        oUsersInsuranceDependent = oInsuranceDependent;
                    }
                    return oUsersInsuranceDependent;
                }
            }
        }
        /// <summary>
        /// This method is used to get USerBasicDetails
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public static UserBasicDetails GetUserBasicDetails(int aiUserId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUserBasicDetails"))
                {
                    UserBasicDetails oUsersBasicDetails = new UserBasicDetails();
                    if (oSqlDataReader.Read())
                    {
                        oUsersBasicDetails.PanNo = oSqlDataReader["PanNo"].ToString();
                        oUsersBasicDetails.EmployeeNo = oSqlDataReader["EmployeeNo"].ToString();
                        if (oSqlDataReader["JoiningDate"].ToString() != string.Empty)
                            oUsersBasicDetails.JoiningDate = (Convert.ToDateTime(oSqlDataReader["JoiningDate"])).ToString("dd-MMM-yyyy");
                        if (oSqlDataReader["PermanentDate"].ToString() != string.Empty)
                            oUsersBasicDetails.PermanentDate = (Convert.ToDateTime(oSqlDataReader["PermanentDate"])).ToString("dd-MMM-yyyy");
                        if (oSqlDataReader["ResignationDate"].ToString() != string.Empty)
                            oUsersBasicDetails.ResignationDate = (Convert.ToDateTime(oSqlDataReader["ResignationDate"])).ToString("dd-MMM-yyyy");
                        if (oSqlDataReader["TransferDate"].ToString() != string.Empty)
                            oUsersBasicDetails.TransferDate = (Convert.ToDateTime(oSqlDataReader["TransferDate"])).ToString("dd-MMM-yyyy");
                        if (oSqlDataReader["PanAttachment"].ToString() != string.Empty)
                            oUsersBasicDetails.FilePath = oSqlDataReader["PanAttachment"].ToString();
                        oUsersBasicDetails.JobTypeId = oSqlDataReader["StaffStatusId"].ToInt();
                        if (oSqlDataReader["StaffStatusId"] != DBNull.Value)
                            oUsersBasicDetails.JobTypeId = oSqlDataReader["StaffStatusId"].ToInt();
                        if (oSqlDataReader["StatusName"] != DBNull.Value)
                            oUsersBasicDetails.JobTypeName = oSqlDataReader["StatusName"].ToString();
                        if (oSqlDataReader["WorkingStatusId"] != DBNull.Value)
                            oUsersBasicDetails.WorkingStatusId = oSqlDataReader["WorkingStatusId"].ToInt();
                        else
                            oUsersBasicDetails.WorkingStatusId = Constants.I_ONE;
                        if (oSqlDataReader["AadharCardNo"] != DBNull.Value)
                            oUsersBasicDetails.AadharNo = oSqlDataReader["AadharCardNo"].ToString();
                        if (oSqlDataReader["AadharCardPhotoCopyPath"] != DBNull.Value)
                            oUsersBasicDetails.AadharFileUpload = oSqlDataReader["AadharCardPhotoCopyPath"].ToString();
                        oUsersBasicDetails.IsOnCHB = oSqlDataReader["IsOnClockHoursBasis"].ToBool();
                        oUsersBasicDetails.GradePay = oSqlDataReader["GradePay"].ToInt();
                        oUsersBasicDetails.BloogGroupId = oSqlDataReader["BloodGroupId"].ToInt();
                    }
                    return oUsersBasicDetails;
                }
            }
        }
        /// <summary>
        /// This method is used to Save User Basic details.
        /// </summary>
        /// <param name="aoUserBasicDetails"></param>
        /// <param name="aiAcademicYearId"></param>
        public static void SaveBasicDetails(UserBasicDetails aoUserBasicDetails, int aiAcademicYearId, int aiLeaveSeperaterDay)  /////
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aoUserBasicDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PanNo", aoUserBasicDetails.PanNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EmployeeNo", aoUserBasicDetails.EmployeeNo, SqlDbType.NVarChar);
                if (aoUserBasicDetails.JoiningDate  !=  string.Empty)
                    oSQLServerDbUtility.AddParameter("JoiningDate", aoUserBasicDetails.JoiningDate, SqlDbType.VarChar);
                if (aoUserBasicDetails.PermanentDate != string.Empty)
                    oSQLServerDbUtility.AddParameter("PermanentDate", aoUserBasicDetails.PermanentDate, SqlDbType.VarChar);
                if (aoUserBasicDetails.ResignationDate != string.Empty)
                    oSQLServerDbUtility.AddParameter("ResignationDate", aoUserBasicDetails.ResignationDate, SqlDbType.VarChar);
                if (aoUserBasicDetails.TransferDate != string.Empty)
                    oSQLServerDbUtility.AddParameter("TransferDate", aoUserBasicDetails.TransferDate, SqlDbType.VarChar);
                oSQLServerDbUtility.AddParameter("JobTypeId", aoUserBasicDetails.JobTypeId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aoUserBasicDetails.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FilePath", aoUserBasicDetails.FilePath, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("InsertedById", aoUserBasicDetails.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LeaveSeperaterDay", aiLeaveSeperaterDay, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("WorkingStatusId", aoUserBasicDetails.WorkingStatusId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AadharCardNo", aoUserBasicDetails.AadharNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("AadharCardUploadFile", aoUserBasicDetails.AadharFileUpload, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsOnClockHourBasic", aoUserBasicDetails.IsOnCHB, SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("BloodGroupId", aoUserBasicDetails.BloogGroupId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertUserBasicDetails");
            }
        }

        /// <summary>
        /// This function is used to validate profile details for user.
        /// </summary>
        /// <param name="aoUserBasicDetails"></param>
        /// <param name="aiAcademicYearId"></param>
        public static void ValidateProfileDetails(UserBasicDetails aoUserBasicDetails, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("UserId", aoUserBasicDetails.UserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("PanNo", aoUserBasicDetails.PanNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("EmployeeNo", aoUserBasicDetails.EmployeeNo, SqlDbType.NVarChar);
                if (aoUserBasicDetails.JoiningDate  !=  string.Empty)
                    oSQLServerDbUtility.AddParameter("JoiningDate", aoUserBasicDetails.JoiningDate, SqlDbType.VarChar);
                if (aoUserBasicDetails.PermanentDate  !=  string.Empty)
                    oSQLServerDbUtility.AddParameter("PermanentDate", aoUserBasicDetails.PermanentDate, SqlDbType.VarChar);
                if (aoUserBasicDetails.ResignationDate  !=  string.Empty)
                    oSQLServerDbUtility.AddParameter("ResignationDate", aoUserBasicDetails.ResignationDate, SqlDbType.VarChar);
            //    oSQLServerDbUtility.AddParameter("JobTypeId", aoUserBasicDetails.JobTypeId, SqlDbType.Int);
                if (aoUserBasicDetails.AadharNo != string.Empty)
                    oSQLServerDbUtility.AddParameter("AadharCardNo", aoUserBasicDetails.AadharNo, SqlDbType.NVarChar);
                if (aoUserBasicDetails.AadharFileUpload != string.Empty)
                    oSQLServerDbUtility.AddParameter("AadharCardPhotoCopyPath", aoUserBasicDetails.AadharFileUpload, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", aoUserBasicDetails.SchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", aoUserBasicDetails.InsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ValidateProfileDetails");
            }
        }
        
		/// <summary>
		///	Checks if the passed UserInsuranceDependant details are a duplicate or no.
		/// </summary>
		/// <param name="aoDependentDetails"></param>
		/// <returns>true if the details are duplicate, false otherwise.</returns>
		public static bool CheckDuplicateDependantDetails(UsersInsuranceDependent aoDependentDetails)

		{
			string sSqlStatement = String.Format("SELECT TOP 1 1 FROM dbo.UsersInsuranceDependent UID" +
												 " WHERE UID.SalutationId = {0} AND UID.FirstName = N'{1}' AND UID.MiddleName = N'{2}' AND UID.LastName = N'{3}' AND UID.DateOfBirth = N'{4}' AND UID.UsersInsuranceDependentId <> {5} AND UID.UserID={6} AND UID.Is_Deleted = 0",
												 aoDependentDetails.SalutationId,
												 StringUtility.ReplaceSingleQuoteInString(aoDependentDetails.FirstName, true),
												 StringUtility.ReplaceSingleQuoteInString(aoDependentDetails.MiddleName, true),
												 StringUtility.ReplaceSingleQuoteInString(aoDependentDetails.LastName, true),
												 aoDependentDetails.DateOfBirth,
												 aoDependentDetails.UsersInsuranceDependentId,
                                                 aoDependentDetails.UserId);
			
			using (var oSqlDbUtility = new SQLServerDbUtility())
				return oSqlDbUtility.PerformIntQueryOnSqlServer(sSqlStatement)  !=  0;

		}

        /// <summary>
        ///		This method is used to update all users whose resigned date is over and but still they are active in system.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static void UpdateUserDetails(int aiSchoolId)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateResignedUserDetails");
            }
        }

        /// <summary>
        /// This method is used to return user details.
        /// </summary>
        /// <param name="aiStaffGroupId"></param>
        /// <returns></returns>
        public List<UserBasicDetails> GetPayrollUsers(int aiStaffGroupId)
        {
            string sFilter = string.Empty;
            if (aiStaffGroupId != 0)
                sFilter = " AND StaffGroupId = " + aiStaffGroupId;
            string sSelectStatement = "select UserId," +
                                      "Name" +
                                      " FROM " +
                                      "[dbo].[udf_GetPayrollUsers](" + miSchoolId + "," + miAcademicYearId + ")" +
                                      " WHERE IsLocked=0" +
                                      sFilter +
                                      "ORDER BY SrNo";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    List<UserBasicDetails> lstUserBasicDetails = new List<UserBasicDetails>();
                    while (oSqlDataReader.Read())
                    {
                        UserBasicDetails oUserBasicDetails = new UserBasicDetails
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            StaffName = Convert.ToString(oSqlDataReader["Name"])
                        };
                        lstUserBasicDetails.Add(oUserBasicDetails);
                    }
                    return lstUserBasicDetails;
                }
            }
        }

        public static DataTable GetAllBloodGroups()
        {
            string sSelectStatement = " SELECT " +
                                     "Id" +
                                     ",BloodGroup " +
                                     " FROM " +
                                     "BloodGroups " +
                                     " WHERE " +
                                     "IsDeleted =" + Constants.I_ZERO ;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
        }
        #endregion "Public Methods"

        #region Payroll Method(s)

        /// <summary>
        /// This method is used to fill user staff group association entity list.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        public void SetUsersSGAssociation(SqlDataReader oSqlDataReader)
        {
            UsersSGAssociation oUsersSGAssociationDC;
            while (oSqlDataReader.Read())
            {
                oUsersSGAssociationDC = new UsersSGAssociation
                {
                    UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                    StaffGroupsId = Convert.ToInt32(oSqlDataReader["StaffGroupsId"])
                };
                mlstUsersSGAssociation.Add(oUsersSGAssociationDC);
            }
        }
        
        #endregion
    }
}
