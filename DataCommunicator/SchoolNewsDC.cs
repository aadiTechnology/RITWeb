// -----------------------------------------------------------------------
// File Name : SchoolNewsDC.cs
// Creator : Sunny
// Created Date : 21-Feb-2014
// -----------------------------------------------------------------------
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using MasterEntities;

namespace DataCommunicator
{
    /// <summary>
    /// This class is used to communicate with database to insert,update and select NEWS.
    /// </summary>
    public class SchoolNewsDC
    {
        #region Data Member(s)

		private int miSchoolId;		
		private int miUpdatedById;

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public SchoolNewsDC()
		{
		}

		/// <summary>
		/// Initializes member variables.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinYearId"></param>
		/// <param name="aiUpdatedById"></param>
		/// <param name="aiAcademicYearId"></param>
        public SchoolNewsDC(int aiSchoolId, int aiUpdatedById)
		{
			this.miSchoolId = aiSchoolId;			
			this.miUpdatedById = aiUpdatedById;
		}

		#endregion

		#region Public Method(s)

		/// <summary>
		/// This method is used to return all news details.
		/// </summary>
		/// <returns></returns>
        public List<NewsDetails> GetAll(int aiIsText)
        {
            List<NewsDetails> lstNewsDetails = new List<NewsDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsText", aiIsText, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllNewsDetails"))
                {
                    while (oSqlDataReader.Read())
                        lstNewsDetails.Add(ReadObjectFromReader(oSqlDataReader));
                }
            }
            return lstNewsDetails;
        }

        ///// <summary>
        ///// This method is used to retrive retirement notice details for particular ID.
        ///// </summary>
        ///// <param name="aiIncomeTaxRangeId"></param>
        ///// <returns></returns>
        public NewsDetails Get(int aiNewsId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NewsId", aiNewsId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSingleNewsDetails"))
                {
                    if (oSqlDataReader.Read())
                        return ReadObjectFromReader(oSqlDataReader);
                }
            }
            return null;
        }

        /// <summary>
        /// This method is used to get Notice ID for Inputed Notice Name.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        ///  <param name="aiNoticeName"></param>
        /// <returns></returns>
        public static int GetIDByName(int aiSchoolId, string asNewsHeading)
        {
            int iNewsId = 0;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NewsHeading", asNewsHeading, SqlDbType.Char);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Usp_GetNewsIdForNews"))
                {
                    if (oSqlDataReader != null)
                    {
                        oSqlDataReader.Read();
                        {
                            if (oSqlDataReader.HasRows)
                                iNewsId = Convert.ToInt32(oSqlDataReader["NewsId"]);
                        }
                    }
                }
            }
            return iNewsId;
        }

		/// <summary>
		/// This method is used to insert/update retirement notice configuration. 
		/// </summary>
		/// <param name="aoIncomeTaxSlab"></param>
        public void Save(NewsDetails aoNewsDetails)
		{
			using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NewsId", aoNewsDetails.NewsId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NewHeading", aoNewsDetails.NewsHeading, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("NewsContent", aoNewsDetails.NewsContent, SqlDbType.NText);
                oSQLServerDbUtility.AddParameter("NewsDate", aoNewsDetails.NewsDate, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortOrder", aoNewsDetails.SortOrder, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FileName", aoNewsDetails.FileName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsText", aoNewsDetails.IsText, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveNewsDetails");
			}
		}


        /// <summary>
        /// This method is used to delete news details.
        /// </summary>
        /// <param name="aiIncomeTaxRangeId"></param>
        public void Delete(int aiNewsId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("NewsId", aiNewsId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteNewsDetails");
            }
        }
        

        ///// <summary>
        ///// This method is used to populate object of retirement notice config.
        ///// </summary>
        ///// <param name="oSqlDataReader"></param>
        ///// <returns></returns>
        private NewsDetails ReadObjectFromReader(SqlDataReader aoSqlDataReader)
        {
            NewsDetails oNewsDetails = new NewsDetails
            {
                NewsId = Convert.ToInt32(aoSqlDataReader["NewsId"]),
                NewsHeading = Convert.ToString(aoSqlDataReader["NewsHeading"]),
                NewsDate = Convert.ToString(aoSqlDataReader["NewsDate"]),
                SortOrder = Convert.ToInt32(aoSqlDataReader["SortOrder"]),
                FileName = Convert.ToString( aoSqlDataReader["FileName"]),
                IsSelected = Convert.ToBoolean(aoSqlDataReader["IsSelected"]),
                NewsContent = Convert.ToString(aoSqlDataReader["NewsContent"]),
            };

            return oNewsDetails;
        }

        /// <summary>
        /// This method is used to save selected news to be displayed on home page.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public void SaveSelectedNews(string asXML)
        {
            
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("NewsXML", asXML, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveSelectedNews");
            }
        }

        /// <summary>
        /// This method is used to get selected news to be displayed on home page.
        /// </summary>
        /// <param name="aoIncomeTaxSlab"></param>
        public List<NewsDetails> GetSelectedNews(int aiSchoolId)
        {
            List<NewsDetails> lstSelectedNewsDetails = new List<NewsDetails>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetSelectedNews"))
                {
                    while (oSqlDataReader.Read())
                    {
                        NewsDetails oNewsDetails = new NewsDetails
                        {
                            NewsId = Convert.ToInt32(oSqlDataReader["NewsId"]),
                            NewsHeading = Convert.ToString(oSqlDataReader["NewsHeading"]),
                            NewsDate = Convert.ToString(oSqlDataReader["NewsDate"]),
                            SortOrder = Convert.ToInt32(oSqlDataReader["SortOrder"]),
                            IsSelected = Convert.ToBoolean(oSqlDataReader["IsSelected"]),
                            NewsContent = HttpUtility.HtmlDecode(Convert.ToString(oSqlDataReader["NewsContent"])),
                            IsText = Convert.ToInt32(oSqlDataReader["IsText"]),
                            FileName = Convert.ToString(oSqlDataReader["FileName"])
                        };
                        lstSelectedNewsDetails.Add(oNewsDetails);
                    }
                }
            }
            return lstSelectedNewsDetails;
        }        

		#endregion
    }
}
