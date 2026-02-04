// -----------------------------------------------------------------------
// <copyright file="BirthDaySMSDC.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DataCommunicator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Data.SqlClient;
    using SchoolEntities.Admin;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BirthDaySMSDC
    {
        public BirthDaySMS moBirthDaySMS;
        public String msMobileNumbers;
		public String msUserNames;
       

        #region Properties

        public BirthDaySMS BirthDaySMS
        {
            get { return moBirthDaySMS; }
        }

        public string MobileNumbers
		 {
            get{return msMobileNumbers;}
         }

		public string UserName
		{
			get{return msUserNames;}
		}

        #endregion 

        #region Methods
        /// <summary>
        /// This method is used to get data for sending birthday sms.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<BirthDaySMS> SendBirthDaySMS(int aiSchoolId ,string aNotificationUserId)
        {
            var lstBirthday = new List<BirthDaySMS>();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("NotificationUserId", aNotificationUserId, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ScheduleBirthdaySMS"))
                {
                    while (oSqlDataReader.Read())
                    {
                        BirthDaySMS olstBirthday = new BirthDaySMS
                        {
                            UserId = Convert.ToInt32(oSqlDataReader["UserId"]),
                            FirstName = oSqlDataReader["FirstName"].ToString(),
                            LastName=oSqlDataReader["LastName"].ToString(),
                            PhoneNumber = oSqlDataReader["PhoneNumber"].ToString(),
                            SalutationName = oSqlDataReader["SalutationName"].ToString(),
                            IsStuudent = oSqlDataReader["IsStudent"].ToString(),
                            Designation = oSqlDataReader["Designation"].ToString()
                        };
                        lstBirthday.Add(olstBirthday);
                    }

                    if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                    {
                        moBirthDaySMS = new BirthDaySMS
                        {
                            AcademicYearId =Convert.ToInt32(oSqlDataReader["AcademicYearId"])
                        };
                    }

					if (oSqlDataReader.NextResult() && oSqlDataReader.Read())
                    {
							msMobileNumbers = Convert.ToString(oSqlDataReader["MobileNumbers"]);
							msUserNames = Convert.ToString(oSqlDataReader["UserNames"]);
					}
                }
            }

            return lstBirthday;
        }

        /// <summary>
        /// This method is used to get data for sending scheduled sms.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <returns></returns>
        public List<ScheduledSMSDetails> ProcessSheduledSMS(int aiSchoolId)
        {
            var lstScheduledSMSDetails = new List<ScheduledSMSDetails>();
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_ProcessScheduledSMS"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ScheduledSMSDetails oScheduledSMSDetails = new ScheduledSMSDetails
                        {
                            SMSText = oSqlDataReader["SMSText"].ToString(),
                            MoblieNumbers = oSqlDataReader["MobileNumbers"].ToString(),
                            ScheduleAt = Convert.ToDateTime(oSqlDataReader["ScheduledAt"])
                        };
                        lstScheduledSMSDetails.Add(oScheduledSMSDetails);
                    }
                }
            }

            return lstScheduledSMSDetails;
        }
    }

        #endregion 
}
