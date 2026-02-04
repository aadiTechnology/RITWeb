// Class Name       :- SchoolwiseEventDescriptionDC
// Purpose          :- This class is used to manage SchoolwiseEventDescription details.
// Date Of creation :- 6/18/2008
// Author Name      :- Anu

using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Utility;
using System.Collections.Generic;
using SchoolEntities;
using System.Globalization;
using SchoolEntities.Dashboard;

namespace DataCommunicator
{

    public class EventDescriptionDC : DataCommunicatorBaseDC
    {
        #region data Members
        private EventDescriptionStruct moSchoolwiseEventDescriptionStruct;
        private int miSchoolId;
        private int miUpdatedById;
        private int miAcademicYearId;
        #endregion

        #region Constructors

        public EventDescriptionDC()
        {
        }
        public EventDescriptionDC(int aiSchoolId, int aiUpdatedById, int aiAcademicYearId)
        {
            miSchoolId = aiSchoolId;
            miUpdatedById = aiUpdatedById;
            miAcademicYearId = aiAcademicYearId;
        }
        public EventDescriptionDC(int miEventId)
        {
            LoadSchoolwiseEventDescriptionDetails(miEventId);
        }

        #endregion

        #region Structure

        public struct EventDescriptionStruct
        {

            public int miEventId;

            public string msEventDescription;

            public System.DateTime mdtEventStartDate;

            public System.DateTime mdtEventEndDate;

            public int miDisplayOnHomepage;

            public int miSchoolId;

            public int miSchoolwiseAcademicYearId;

            public string msIsDeleted;

            public System.DateTime mdtInsertDate;

            public int miInsertedByid;

            public System.DateTime mdtUpdateDate;

            public int miUpdatedById;

            public string msEventImageName;

            public string msEventComments;
        }

        #endregion

        #region Properties

        public virtual EventDescriptionStruct EventDescriptionStructDetails
        {
            get
            {
                return moSchoolwiseEventDescriptionStruct;
            }
            set
            {
                moSchoolwiseEventDescriptionStruct = value;
            }
        }

        #endregion

        #region Private Methods
        // This function is used to load the SchoolwiseEventDescription Details
        private void LoadSchoolwiseEventDescriptionDetails(int miEventId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sSelectStatement = FetchSchoolwiseEventDescriptionDetailsFromDatabase(miEventId);
                using(SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
                {
                    if (oDR != null)
                    {
                        while (oDR.Read())
                        {
                            if (oDR["Event_Id"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.miEventId = Convert.ToInt32(oDR["Event_Id"]);
                            if (oDR["Event_Description"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.msEventDescription = Convert.ToString(oDR["Event_Description"]);
                            if (oDR["Event_Start_Date"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.mdtEventStartDate = Convert.ToDateTime(oDR["Event_Start_Date"]);
                            if (oDR["Event_End_Date"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.mdtEventEndDate = Convert.ToDateTime(oDR["Event_End_Date"]);
                            if (oDR["School_Id"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.miSchoolId = Convert.ToInt32(oDR["School_Id"]);
                            if (oDR["Academic_Year_ID"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.miSchoolwiseAcademicYearId = Convert.ToInt32(oDR["Academic_Year_ID"]);
                            if (oDR["Is_Deleted"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.msIsDeleted = Convert.ToString(oDR["Is_Deleted"]);
                            if (oDR["Insert_Date"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.mdtInsertDate = Convert.ToDateTime(oDR["Insert_Date"]);
                            if (oDR["Inserted_By_id"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.miInsertedByid = Convert.ToInt32(oDR["Inserted_By_id"]);
                            if (oDR["Update_Date"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.mdtUpdateDate = Convert.ToDateTime(oDR["Update_Date"]);
                            if (oDR["Updated_By_Id"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.miUpdatedById = Convert.ToInt32(oDR["Updated_By_Id"]);
                            if (oDR["Display_On_Homepage"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.miDisplayOnHomepage = Convert.ToInt32(oDR["Display_On_Homepage"]);
                            if (oDR["Event_Image"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.msEventImageName = Convert.ToString(oDR["Event_Image"]);
                            if (oDR["Event_Comment"] != DBNull.Value)
                                moSchoolwiseEventDescriptionStruct.msEventComments = Convert.ToString(oDR["Event_Comment"]);

                        }
                    }
                }
            }
        }

        // This function is used to fetch the SchoolwiseEventDescription Details
        private string FetchSchoolwiseEventDescriptionDetailsFromDatabase(int miEventId)
        {
            string sSelectStatement = " SELECT  " +
            "Event_Id" +
            ",Event_Description" +
            ",Event_Comment" +
            ",Event_Start_Date" +
            ",Event_End_Date" +
			",Display_On_Homepage"+
            ",Event_Image" +
            ",School_Id" +
            ",Academic_Year_ID" +
            ",Is_Deleted" +
            ",Insert_Date" +
            ",Inserted_By_id" +
            ",Update_Date" +
            ",Updated_By_Id" +
            " FROM Schoolwise_Events" +
            " WHERE Event_Id=" + miEventId;
            return sSelectStatement;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to save file details
        /// </summary>
        /// <param name="asLinkUrl"></param>

        public void SaveFileDetails(string asLinkUrl)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LinkUrl", asLinkUrl, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveYearwiseAnnualPlannerDetails");
            }
        }

        /// <summary>
        /// This method is used to get file details
        /// </summary>
        /// <returns></returns>
        public string GetFileDetails()
        {
            string sSelectStatement = " SELECT  " +
                "LinkUrl" +
                " FROM  " +
                "YearwiseAnnualPlannerDetails WITH(NOLOCK)" +
                " WHERE  " +
                "AcademicYearId = " + this.miAcademicYearId +
                " AND SchoolId =" + this.miSchoolId +
                " AND IsDeleted = 0";

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.PerformStringQueryOnSqlServer(sSelectStatement);
        }
        /// <summary>
        /// This method is used to delete file details
        /// </summary>
        public void DeleteFileDetails()
        {
            string sSelectStatement = " Update YearwiseAnnualPlannerDetails  " +
                "Set IsDeleted = 1" +
                " WHERE  " +
                "AcademicYearId = " + this.miAcademicYearId +
                " AND SchoolId =" + this.miSchoolId;

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sSelectStatement);
        }


        /// <summary>
        /// This function is used to insert the SchoolwiseEventDescription Details
        /// </summary>
        public void InsertEventDescription(ArrayList oarrStdLst)
        {
            ArrayList sArrInsert = new ArrayList();

            string sInsertStatement = string.Empty;

            if (moSchoolwiseEventDescriptionStruct.msEventImageName == string.Empty)
            {
                     sInsertStatement = " INSERT " +
                                          " INTO " +
                                          " Schoolwise_Events ( " +
                                          " Event_Description " +
                                          " ,Event_Comment " +
                                          " ,Event_Start_Date " +
                                          " ,Event_End_Date " +
                                          " ,Display_On_Homepage " +
                                          " ,School_Id " +
                                          " ,Academic_Year_ID" +
                                          " ,Inserted_By_Id " +
                                          " ,Updated_By_Id " +
                                          " ) VALUES (" + "  " +
                                                "  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseEventDescriptionStruct.msEventDescription, false) + "' " +
                                                "  , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseEventDescriptionStruct.msEventComments,false) + "' " +
                                                 " , N'" + moSchoolwiseEventDescriptionStruct.mdtEventStartDate.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")) + "' " +
                                                 " , N'" + moSchoolwiseEventDescriptionStruct.mdtEventEndDate.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")) + "' " +
                                                 " , N'" + moSchoolwiseEventDescriptionStruct.miDisplayOnHomepage + "' " +
                                                 " , " + moSchoolwiseEventDescriptionStruct.miSchoolId +
                                                 " , " + moSchoolwiseEventDescriptionStruct.miSchoolwiseAcademicYearId +
                                                 " , " + moSchoolwiseEventDescriptionStruct.miInsertedByid +
                                                 " , " + moSchoolwiseEventDescriptionStruct.miUpdatedById +
                                          ")";
            }
            else
            {
                  sInsertStatement = " INSERT " +
                                          " INTO " +
                                          " Schoolwise_Events ( " +
                                          " Event_Description " +
                                          " ,Event_Comment " +
                                          " ,Event_Start_Date " +
                                          " ,Event_End_Date " +
                                          " ,Display_On_Homepage " +
                                          " ,Event_Image" +
                                          " ,School_Id " +
                                          " ,Academic_Year_ID" +
                                          " ,Inserted_By_Id " +
                                          " ,Updated_By_Id " +
                                          " ) VALUES (" + "  " +
                                                "  N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseEventDescriptionStruct.msEventDescription, false) + "' " +
                                                "  , N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseEventDescriptionStruct.msEventComments, false) + "' " +
                                                 " , N'" + moSchoolwiseEventDescriptionStruct.mdtEventStartDate.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")) + "' " +
                                                 " , N'" + moSchoolwiseEventDescriptionStruct.mdtEventEndDate.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")) + "' " +
                                                 " , N'" + moSchoolwiseEventDescriptionStruct.miDisplayOnHomepage + "' " +
                                                 " , N'" + moSchoolwiseEventDescriptionStruct.msEventImageName + "' " +
                                                 " , " + moSchoolwiseEventDescriptionStruct.miSchoolId +
                                                 " , " + moSchoolwiseEventDescriptionStruct.miSchoolwiseAcademicYearId +
                                                 " , " + moSchoolwiseEventDescriptionStruct.miInsertedByid +
                                                 " , " + moSchoolwiseEventDescriptionStruct.miUpdatedById +
                                          ")";
            }

            sArrInsert.Add(sInsertStatement);
            sInsertStatement = GetSelectStatementForLastInsertedPKey(Constants.S_LAST_INSERTED_P_KEY);
            sArrInsert.Add(sInsertStatement);
            for (int iRowCnt = 0; iRowCnt < oarrStdLst.Count; iRowCnt++)
            {
                string sInsertQuery = " INSERT INTO [dbo].[Schoolwise_Events_Detail] " +
                                      "( " +
                                      " [Event_Id] " +
                                      ", [StandardDivisionId] " +
                                      ", [Inserted_By_id] " +
                                      " )" +
                                      "  VALUES " +
                                      " ( " +
                                      "  N'" + Constants.S_LAST_INSERTED_P_KEY + "'" +
                                      ", " + Convert.ToInt32(oarrStdLst[iRowCnt].ToString()) +
                                      ", " + moSchoolwiseEventDescriptionStruct.miInsertedByid +
                                      " )";
                sArrInsert.Add(sInsertQuery);
            }
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((string[])sArrInsert.ToArray(typeof(string)));
        }

        /// <summary>
        /// This function is used to update the SchoolwiseEventDescription Details of a particular event.
        /// </summary>
        public void UpdateEventDescription(ArrayList oarrStdLst)
        {
            ArrayList arrUpdate = new ArrayList();
            string sUpdateStatement = " UPDATE " +
                                      " Schoolwise_Events " +
                                      " SET " +
                                      " Event_Description= N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseEventDescriptionStruct.msEventDescription, false) + "' " +
                                      " ,Event_Comment= N'"+ StringUtility.ReplaceSingleQuoteInString(moSchoolwiseEventDescriptionStruct.msEventComments,false) + "' " +
                                      " ,Event_Start_Date= N'" + moSchoolwiseEventDescriptionStruct.mdtEventStartDate.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")) + "' " +
                                      " ,Event_End_Date= N'" + moSchoolwiseEventDescriptionStruct.mdtEventEndDate.ToString(Constants.S_DATE_FORMAT_MARATHI, new CultureInfo("en")) + "' " +
                                      " ,Display_On_Homepage= N'" + moSchoolwiseEventDescriptionStruct.miDisplayOnHomepage + "' " +
                                      " ,Event_Image=N'" + moSchoolwiseEventDescriptionStruct.msEventImageName + "' " +
                                      " " +
                                      " WHERE " +
                                      " Event_Id = " + moSchoolwiseEventDescriptionStruct.miEventId +
                                      " AND " +
                                      " Is_Deleted= N'" + Constants.C_NO + "'";
            arrUpdate.Add(sUpdateStatement);
            string sDeleteQuery = " DELETE " +
                                  " FROM " +
                                  " [dbo].[Schoolwise_Events_Detail]" +
                                  " WHERE " +
                                  " Event_Id=" + moSchoolwiseEventDescriptionStruct.miEventId;
            arrUpdate.Add(sDeleteQuery);
            for (int iRowCnt = 0; iRowCnt < oarrStdLst.Count; iRowCnt++)
            {
                string sInsertQuery = " INSERT INTO [dbo].[Schoolwise_Events_Detail] " +
                                      "( " +
                                      " [Event_Id] " +
                                      ", [StandardDivisionId] " +
                                      ", [Inserted_By_id] " +
                                      " ) " +
                                      "  VALUES " +
                                      " ( " +
                                      moSchoolwiseEventDescriptionStruct.miEventId +
                                      ", " + Convert.ToInt32(oarrStdLst[iRowCnt].ToString()) +
                                      ", " + moSchoolwiseEventDescriptionStruct.miInsertedByid +
                                      " )";
                arrUpdate.Add(sInsertQuery);
            }
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction((string[])arrUpdate.ToArray(typeof(string)));
        }

        /// <summary>
        /// This method is used to delete the SchoolwiseEventDescription Details of a particular event.
        /// </summary>
        public void DeleteSchoolwiseEventDescription()
        {
            string sDeleteStatement = " DELETE " +
                                      " Schoolwise_Events " +
                                      " WHERE Event_Id=  N'" + moSchoolwiseEventDescriptionStruct.miEventId + "'";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sDeleteStatement);
        }

        /// <summary>
        /// This method is used to delete Image of a particular event.
        /// </summary>
        public void DeleteEventImage()
        {
            string sUpdateStatement = "UPDATE Schoolwise_Events" +
                                      " SET Event_Image = null" +
                                        " WHERE  " +
                                        " Academic_Year_ID = " + this.miAcademicYearId +
                                        " AND School_Id =" + this.miSchoolId +
                                        " AND Event_Id =" + moSchoolwiseEventDescriptionStruct.miEventId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
        }

        /// <summary>
        /// This method is used to get event description for a particular date.
        /// </summary>
        /// <param name="adtEventDate"></param>
        /// <returns></returns>
        public DataTable GetEventDescription(DateTime adtEventDate, Int32 aiSchoolId, Int32 aiAcademicYrId, Int32 aiStandardId, Int32 aiDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("EventDate", adtEventDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetEventDescription");
            }
        }

        /// <summary>
        /// This method is used to get events data for particular month.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public DataTable GetEventsData(int aiSchoolId, int aiAcademicYrId, int aiMonthId, int aiYear, int aiStdId, int aiDivisionId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Month_id", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Standard_Id", aiStdId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DivisionId", aiDivisionId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_Annual_Planner");
            }
        }

        /// <summary>
        /// This method is used to get Events by giving count value.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiCountValue"></param>
        /// <returns></returns>
        public static DataTable GetEventsByCountValue(int aiSchoolId, int aiAcademicYrId, int aiCountValue)
        { 
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("CountValue", aiCountValue, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetEventsByCountValue");
            }
        }

        /// <summary>
        /// This method is used to get events data for particular month.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public DataTable GetEventsData(int aiSchoolId, int aiAcademicYrId, int aiMonthId, int aiYear)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Month_id", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_Annual_Planner");
            }
        }


        /// <summary>
        /// This method is used to get events data for particular month.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYrId"></param>
        /// <param name="aiMonthId"></param>
        /// <param name="aiYear"></param>
        /// <returns></returns>
        public DataTable GetEventsDataForStudent(int aiSchoolId, int aiAcademicYrId, int aiMonthId, int aiYear, int aiStudentId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("School_Id", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Academic_Year_ID", aiAcademicYrId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Month_id", aiMonthId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Year", aiYear, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StudentId", aiStudentId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_Annual_Planner");
            }
        }

        /// <summary>
        /// This method Check whether event is pduplicate or not
        /// </summary>
        /// <returns></returns>
        public Int32 CheckForDuplicateEvent()
        {
            StringBuilder sFilter = new StringBuilder();
            sFilter.Append(" WHERE " +
                              " Event_Description = N'" + StringUtility.ReplaceSingleQuoteInString(moSchoolwiseEventDescriptionStruct.msEventDescription, false) + "' " +
                              " AND School_Id = " + moSchoolwiseEventDescriptionStruct.miSchoolId + " " +
                              " AND Academic_Year_ID = " + moSchoolwiseEventDescriptionStruct.miSchoolwiseAcademicYearId +
                              " AND Event_Start_Date = N'" + moSchoolwiseEventDescriptionStruct.mdtEventStartDate.ToString("MM-dd-yyyy",new CultureInfo("en")) + "'" +
                              " AND Event_End_Date = N'" + moSchoolwiseEventDescriptionStruct.mdtEventEndDate.ToString("MM-dd-yyyy", new CultureInfo("en")) + "'" +
                              " AND Is_Deleted= N'" + Constants.C_NO + "'");
            if (moSchoolwiseEventDescriptionStruct.miEventId != Constants.I_ZERO)
            {
                sFilter.Append(" AND Event_Id <> " + moSchoolwiseEventDescriptionStruct.miEventId + " ");
            }
            string sSelectStatment = " SELECT " +
                                        " COUNT(Event_Id)" +
                                     " FROM Schoolwise_Events" +
                                        sFilter.ToString();

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return (oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatment));
        }

        /// <summary>
        /// This method is used to get associated standards list for a given event.
        /// </summary>
        /// <param name="aiEventId"></param>
        /// <returns></returns>
        public static DataTable GetAssociatedStdLst(int aiEventId)
        {
            string sQuery = " SELECT " +
                                " StandardDivisionId , " +
                                " Event_Id " +
                            " FROM " +
                            " Schoolwise_Events_Detail " +
                            " WHERE " +
                            " Is_Deleted = '" + Constants.C_NO + "'" +
                            " AND " +
                            " Event_Id = " + aiEventId;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sQuery);
        }

		/// <summary>
		/// Returns all the Events of the School
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiAcademicYearId"></param>
		/// <param name="aiStandardId"></param>
		/// <returns>A List object of Event entity class</returns>
		public static List<Event> GetAllEvents(int aiSchoolId, int aiAcademicYearId, int aiStandardId, int aiMonthId)
		{
			List<Event> lstEvents = new List<Event>();
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
				if(aiStandardId != 0)
					oSQLServerDbUtility.AddParameter("StandardId", aiStandardId, SqlDbType.Int);
				if(aiMonthId != 0)
					oSQLServerDbUtility.AddParameter("MonthId", aiMonthId, SqlDbType.Int);
				
				using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllEvents"))
				{
					if(oSqlReader.HasRows)
					{
						Utility.GenericClass<Event> oGenricClass = new GenericClass<Event>();
						lstEvents = oGenricClass.GetFilledObjectList(oSqlReader);
					}
				}
			}
			return lstEvents;
		}

        /// <summary>
        /// Returns all the Events of the School
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiStandardId"></param>
        /// <returns>A List object of Event entity class</returns>
        public static List<Event> GetAllEvents(int aiSchoolId)
        {
            List<Event> lstEvents = new List<Event>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllEvents_KNS"))
                {
                    if (oSqlReader.HasRows)
                    {
                        Utility.GenericClass<Event> oGenricClass = new GenericClass<Event>();
                        lstEvents = oGenricClass.GetFilledObjectList(oSqlReader);
                    }
                }
            }
            return lstEvents;
        }

        /// <summary>
        /// this method is used to get upcoming event related data
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="aiUserRoleId"></param>
        /// <returns></returns>
        public static List<UpcomingEvents> GetUpcomingEvents(int aiSchoolId, int aiAcademicYearId, int aiUserId, int aiUserRoleId, string isScreenFullAccess, bool abIsServiceCall = false)
        {
            List<UpcomingEvents> lstEvents = new List<UpcomingEvents>();


            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, aiAcademicYearId, aiUserId, abIsServiceCall))
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsScreenFullAccess", isScreenFullAccess, SqlDbType.NVarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetUpcomingEvents"))
                {
                    UpcomingEvents oUpcomingEvents = null;

                    while (oSqlDataReader.Read())
                    {
                        oUpcomingEvents = new UpcomingEvents()
                        {
                            StartDate = Convert.ToDateTime(oSqlDataReader["StartDate"]).ToString(Constants.S_DATE_FORMAT),
                            EndDate = Convert.ToDateTime(oSqlDataReader["EndDate"]).ToString(Constants.S_DATE_FORMAT),
							EndDateUniversal = Convert.ToDateTime(oSqlDataReader["EndDate"]).ToString(Constants.S_DATE_FORMAT_MARATHI),
                            EventTitle = oSqlDataReader["EventTitle"].ToString(),
                            StandardName = oSqlDataReader["StandardName"].ToString(),
                            EventType = oSqlDataReader["Type"].ToString()
                        };

                        lstEvents.Add(oUpcomingEvents);
                    }
                }
            }

            return lstEvents;
        }

        public List<EventDetails> GetSelectedEvents(int aiSchoolId)
        {
            List<EventDetails> lstSelectedEventsDetails = new List<EventDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSelectedEvents"))
                {
                    while (oSqlDataReader.Read())
                    {
                        EventDetails oEventDetails = new EventDetails
                        {
                            Event_Id = Convert.ToInt32(oSqlDataReader["Event_Id"]),
                            Event_Description = Convert.ToString(oSqlDataReader["Event_Description"]),
                            Event_Comment = Convert.ToString(oSqlDataReader["Event_Comment"]),
                            Event_Start_Date = Convert.ToString(oSqlDataReader["Event_Start_Date"]),
                            Display_On_Homepage = Convert.ToInt32(oSqlDataReader["Display_On_Homepage"]),
                            Event_Image=Convert.ToString(oSqlDataReader["Event_Image"]),
                            AssociatedStandards = Convert.ToString(oSqlDataReader["AssociatedClasses"])
                        };
                        lstSelectedEventsDetails.Add(oEventDetails);
                    }
                }
            }

            return lstSelectedEventsDetails;
        }     
        #endregion

        /// <summary>
        /// This method is used to return top events.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiTopCount"></param>
        /// <returns></returns>
        public static List<Event> GetAllTopEvents(int aiSchoolId, int aiTopCount)
        {
            List<Event> lstEvents = new List<Event>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("TopCount", aiTopCount, SqlDbType.Int);
                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllTopEvents"))
                {
                    if (oSqlReader.HasRows)
                    {
                        Utility.GenericClass<Event> oGenricClass = new GenericClass<Event>();
                        lstEvents = oGenricClass.GetFilledObjectList(oSqlReader);
                    }
                }
            }
            return lstEvents;
        }
    }

    public class EventDescriptionCollectionDC
    {

        // This function is used to Fetch the SchoolwiseEventDescription Details
        public static DataSet FetchSchoolwiseEventDescriptionDetails()
        {
            string sFetchStatement = "SELECT  * FROM Schoolwise_Events";
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
                return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataSet(sFetchStatement);
        }
    }   
}



