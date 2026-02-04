// Class Name       :- CareerDetailsDC
// Purpose          :- This class is used to manage Carrer details.
// Date Of creation :- 1 Decemeber 2012
// Author Name      :- 


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.SqlClient;
using CareerEntities;

namespace DataCommunicator
{
    public class CareerDetailsDC
    {

        #region Data Members

        private CareerDetailsInfo moCareerDetailsInfo;

        #endregion

        #region Constructor

        public CareerDetailsDC()
        {
            moCareerDetailsInfo = new CareerDetailsInfo();
        }

        public CareerDetailsDC(int aiCareerDetailsID)
        {
            moCareerDetailsInfo.CareerDetailsID = aiCareerDetailsID;
        }

        #endregion

        #region Properties

        /// <Summary>
        ///The developer has to use this property to get/set the data members of entity object from UI layer
        ///</Summary>
        ///<returns></returns>
        ///
        public CareerDetailsInfo CareerDetails
        {
            get
            {
                return moCareerDetailsInfo;
            }
            set
            {
                moCareerDetailsInfo = value;
            }
        }

        #endregion

        #region Public Methods
        
        /// <Summary>
        ///This function is used to insert the Career Details 
        ///</Summary>
        ///<returns></returns>
        public void Save()
        {
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                {
                    oSQLServerDbUtility.AddParameter("Name", moCareerDetailsInfo.Name, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("DOB", moCareerDetailsInfo.DOB, SqlDbType.DateTime);
                    oSQLServerDbUtility.AddParameter("Address", moCareerDetailsInfo.Address, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("Email", moCareerDetailsInfo.Email, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("MobileNo", moCareerDetailsInfo.MobileNo, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("YearOfExperience", moCareerDetailsInfo.YearOfExperience, SqlDbType.Float);
                    oSQLServerDbUtility.AddParameter("Post", moCareerDetailsInfo.Post, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("LastOrganisationName", moCareerDetailsInfo.LastOrganisationName, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("AreaOfSpecialization", moCareerDetailsInfo.AreaOfSpecialization, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("Resume", moCareerDetailsInfo.Resume, SqlDbType.NVarChar);
                    oSQLServerDbUtility.AddParameter("IsActive", moCareerDetailsInfo.IsActive, SqlDbType.Bit);
                    if(moCareerDetailsInfo.Education != null)
                        oSQLServerDbUtility.AddParameter("Education", moCareerDetailsInfo.Education, SqlDbType.NVarChar);
                    oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[dbo].[usp_InsertCareerDetails]");
                }
        }

        /// <Summary>
        ///This Methos is used to get the All the Employee Details from the CareerDetails table
        ///</Summary>
        ///<returns></returns>
        public static List<CareerDetailsInfo> GetAll()
        {
            List<CareerDetailsInfo> lstCareerDetails = new List<CareerDetailsInfo>();
            string sSelectStatement = " SELECT " +
                                      " CareerDetailsID, " +
                                      " Name, " +
                                      " MobileNo, " +
                                      " YearOfExperience, " +
                                      " Post, " +
                                      " Resume " +
                                      " FROM " +
                                      " CareerDetails order by Name desc";
            SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility();
            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
            {
                lstCareerDetails = ReadCareerDetails(oSqlDataReader);
                return lstCareerDetails;
            }
        }

        /// <Summary>
        ///This Methos is used to get the All the Employee Details from the CareerDetails table depending upon the search criteria
        ///</Summary>
        ///<returns></returns>
        public static List<CareerDetailsInfo> GetEmployeeCareerDetails(string asName, string asExperience, string asPost)
        {
            List<CareerDetailsInfo> lstCareerDetails = new List<CareerDetailsInfo>();
            string sSelectStatement = " SELECT " +
                                      " CareerDetailsID, " +
                                      " Name, " +
                                      " MobileNo, " +
                                      " YearOfExperience, " +
                                      " Post, " +
                                      " Resume " +
                                      " FROM " +
                                      " CareerDetails WHERE ";
            if (asName != string.Empty)
                sSelectStatement = String.Concat(sSelectStatement, " Name like N'%" + asName + "%' " + " And ");
            if (asExperience.ToString() != string.Empty)
                sSelectStatement = String.Concat(sSelectStatement, " YearOfExperience = " + asExperience + " And ");
            if (asPost != string.Empty)
                sSelectStatement = String.Concat(sSelectStatement, " Post = N'" + asPost + "' And ");
            
            if(sSelectStatement.EndsWith("And "))
            {
                int i = sSelectStatement.LastIndexOf("And ");
                if (i >= 0)
                    sSelectStatement = sSelectStatement.Substring(0, i);
            }

            SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility();
            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
            {
                lstCareerDetails = ReadCareerDetails(oSqlDataReader);
                return lstCareerDetails;
            }
        }

        private static List<CareerDetailsInfo> ReadCareerDetails(SqlDataReader oSqlDataReader)
        {
            CareerDetailsInfo oCareerDetails;
            List<CareerDetailsInfo> lstCareerDetails = new List<CareerDetailsInfo>();
            while (oSqlDataReader.Read())
            {
                oCareerDetails = new CareerDetailsInfo
                {
                    
                      CareerDetailsID = Convert.ToInt32(oSqlDataReader["CareerDetailsID"]),
                      Name = Convert.ToString(oSqlDataReader["Name"]),
                      MobileNo = Convert.ToString(oSqlDataReader["MobileNo"]),
                      YearOfExperience = Convert.ToInt32(oSqlDataReader["YearOfExperience"]),
                      Post = Convert.ToString(oSqlDataReader["Post"]),
                      Resume = Convert.ToString(oSqlDataReader["Resume"]),
                };
                lstCareerDetails.Add(oCareerDetails);
            }
            return lstCareerDetails;
        }
      
        #endregion

    }
}
