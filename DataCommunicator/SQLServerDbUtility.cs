using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using SchoolEntities;
using Utility;
 
namespace DataCommunicator
{
	/// <summary>
	///		Provides an API for CRUD operations on the database.
	/// </summary>
	public class SQLServerDbUtility : IDisposable
	{
		#region -- MEMBER(s) --

		private SqlConnection moConn;
		private SqlTransaction motransaction;
		private SqlCommand moSqlCommand;

        public bool mbIsServiceCall { get; set; }
        #endregion -- MEMBER(s) --

		#region -- PROPERTIES --

		private PageRequestLog PageRequestLog
		{
			get
			{
				if (HttpContext.Current.IsNull() || HttpContext.Current.Session.IsNull())
					return null;
				return HttpContext.Current.Session[Constants.S_SESSION_PAGE_REQUEST] as PageRequestLog;
			}
		}

        private PageRequestLog PageRequestOfService { get; set; }

		#endregion -- PROPERTIES --

		#region -- CONSTRUCTOR(s) --

        /// <summary>
        /// Class constructor
        /// Main task of constructor to initialize connection and command objects.
        /// </summary>
        public SQLServerDbUtility()
        {
            InitializeSqlConnection();
        }

        /// <summary>
        /// Class constructor
        /// Main task of constructor to initialize connection and command objects.
        /// </summary>
        public SQLServerDbUtility(string asConnectionString)
        {
            InitializeSqlConnection(asConnectionString);
        }

        /// <summary>
        /// This constructor is used to establish a SQL connection and initialize page request log and its activity log. 
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiUserId"></param>
        /// <param name="abIsServiceCall"></param>
        public SQLServerDbUtility(int aiSchoolId, int aiAcademicYearId, int aiUserId, bool abIsServiceCall)
        {
            InitializeSqlConnection();

            mbIsServiceCall = abIsServiceCall;

            PageRequestOfService = new PageRequestLog
            {
                SessionId = string.Empty,
                Page = Constants.S_PAGE_REQUEST_SERVICE,
                IsPostBack = false,
                InsertDate = DateTime.Now,
                RequestSchoolId = aiSchoolId,
                RequestAcademicYearId = aiAcademicYearId,
                UserId = aiUserId,
                ActivityLog = new System.Collections.Generic.List<ActivityLog>()
            };
        }

		#endregion -- CONSTRUCTOR(s) --

		#region -- DESTRUCTOR --
		
		/// <summary>
		/// Use C# destructor syntax for finalization code.
		/// This destructor will run only if the Dispose method
		/// does not get called.
		/// It gives your base class the opportunity to finalize.
		/// Do not provide destructors in types derived from this class.
		/// </summary>
		~SQLServerDbUtility()
		{
			CloseHandles();
		}

		#endregion -- DESTRUCTOR --
		
		#region -- PUBLIC METHOD(s) --

		/// <summary>
		/// Implemented IDisposable so implement Dispose() method. This method is 
		/// </summary>
		public void Dispose()
		{
			CloseHandles();
			// Use SupressFinalize to suppress the garbage collector from calling Finalize
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// This method is used to add parameter to command object.
		/// </summary>
		/// <param name="aoName"></param>
		/// <param name="aoValue"></param>
		/// <param name="aoSQLDbType"></param>
		public void AddParameter(string aoName, object aoValue, SqlDbType aoSQLDbType)
		{
			var oSqlParameter = new SqlParameter
			                    	{
			                    		ParameterName = "@" + aoName,
			                    		Value		  = aoValue,
			                    		SqlDbType	  = aoSQLDbType
			                    	};
			moSqlCommand.Parameters.Add(oSqlParameter);
		}

		/// <summary>
		/// This method is used to add parameter to command object. This method is override specially for output type parameter.
		/// </summary>
		/// <param name="aoName"></param>
		/// <param name="aoValue"></param>
		/// <param name="aoSQLDbType"></param>
		/// <param name="aenumDirection"></param>
		/// <returns></returns>
		public SqlParameter AddParameter(string aoName, object aoValue, SqlDbType aoSQLDbType, ParameterDirection aenumDirection)
		{
			return AddParameter(aoName, aoValue, aoSQLDbType, aenumDirection, 0);
		}

		/// <summary>
		/// This method is used to add parameter to command object. This method is override specially for output type parameter having data type other than intger..
		/// </summary>
		/// <param name="aoName"></param>
		/// <param name="aoValue"></param>
		/// <param name="aoSQLDbType"></param>
		/// <param name="aenumDirection"></param>
		/// <param name="oSize"></param>
		/// <returns></returns>
		public SqlParameter AddParameter(string aoName, object aoValue, SqlDbType aoSQLDbType, ParameterDirection aenumDirection, int oSize)
		{
			var oSqlParameter = new SqlParameter
			                    	{
			                    		ParameterName = "@" + aoName,
			                    		Value		  = aoValue,
			                    		Size		  = oSize,
			                    		SqlDbType	  = aoSQLDbType,
			                    		Direction	  = aenumDirection
			                    	};
			moSqlCommand.Parameters.Add(oSqlParameter);
			return oSqlParameter;
		}

		/// <summary>
		/// Executes the query and return integer value of first column of first row of result.
		/// </summary>
		/// <param name="asCommandText"></param>
		/// <returns></returns>
		public int PerformIntQueryOnSqlServer(String asCommandText)
		{
			object result = ExecuteScalarQuery(asCommandText);
			return result != DBNull.Value ? result.ToInt() : 0;
		}

		/// <summary>
		/// Executes the query and return integer value of first column of first row of result.
		/// </summary>
		/// <param name="asCommandText"></param>
		/// <returns></returns>
		public string PerformStringQueryOnSqlServer(String asCommandText)
		{
			return Convert.ToString(ExecuteScalarQuery(asCommandText));
		}

		/// <summary>
		/// This method is used to execute given statement in a single transaction and return integer value = x
		/// x = if statement is select then Executes the query and return integer value of first column of first row of result.
		/// x = if statement is insert then identity id of last inserted record.
		/// x = if update/delete then number of records effected by this query.
		/// </summary>
		/// <param name="asTransactionStatement"></param>
		/// <returns></returns>
		public int ExecuteTransaction(string asTransactionStatement)
		{
			motransaction = moConn.BeginTransaction(IsolationLevel.ReadUncommitted);
			moSqlCommand.Transaction = motransaction;
			int iReturnValue;
			bool bIsTransactionCommitted = false;
			
			try
			{
				if (asTransactionStatement.IsNullOrEmpty())
					throw new ArgumentNullException("asTransactionStatement");
				
				if (asTransactionStatement.Trim().Substring(0, 6).ToUpper() == "SELECT")
					iReturnValue = ExecuteScalarQuery(asTransactionStatement).ToInt();
				else
				{
					//asTransactionStatement = asTransactionStatement.Replace(String.Format("'{0}'", Constants.S_SERVER_CURRENT_DATE_TIME), "CURRENT_TIMESTAMP");
                    asTransactionStatement = asTransactionStatement.Replace(String.Format("'{0}'", Constants.S_SERVER_CURRENT_DATE_TIME), "dbo.GetLocalDate(DEFAULT)");
					moSqlCommand.CommandText = asTransactionStatement;
					DateTime dtStart = DateTime.Now;
					Stopwatch timer = Stopwatch.StartNew();
					
					try
					{
						iReturnValue = moSqlCommand.ExecuteNonQuery();
					}
					finally
					{
						timer.Stop();
						if (Constants.B_ACTIVITY_LOGGING)
							CreateActivityLog(asTransactionStatement, false, null, dtStart, timer.ElapsedMilliseconds);
					}

					if (asTransactionStatement.Trim().ToUpper().StartsWith("INSERT"))
						iReturnValue = ExecuteScalarQuery("SELECT SCOPE_IDENTITY()").ToInt();
				}

				motransaction.Commit();
				bIsTransactionCommitted = true;
			}
			catch (SqlException)
			{
				RollBackTransaction(bIsTransactionCommitted);

				// Throw the original exception, so the calling method can handle it.
				throw;
			}

			return iReturnValue;
		}

		/// <summary>
		/// This method is used to execute given array of sql statements in a single transaction and return integer value = x
		/// x = if statement is select then Executes the query and return integer value of first column of first row of result.
		/// </summary>
		/// <param name="asArrTransactionStatements"></param>
		/// <returns></returns>
		public int ExecuteTransaction(string[] asArrTransactionStatements)
		{
			return ExecuteTransaction(asArrTransactionStatements, Constants.PrimaryKeyRecord.First);
		}

		/// <summary>
		/// This method is used to execute given array of sql statements in a single transaction and return integer value = x
		/// x = if aePrimaryKeyRecord is Constants.PrimaryKeyRecord.First then identity id of first inserted record.
		/// x = if aePrimaryKeyRecord is Constants.PrimaryKeyRecord.Last then identity id of last inserted record.
		/// </summary>
		/// <param name="asArrTransactionStatements"></param>
		/// <param name="aePrimaryKeyRecord"></param>
		/// <returns></returns>
		public int ExecuteTransaction(string[] asArrTransactionStatements, Constants.PrimaryKeyRecord aePrimaryKeyRecord)
		{
			Int32 iReturnValue = 0;
			Int32 iReplaceValueForLastInsertedPKey = 0;
			Int32 iReplaceValueForLastInsertedPKey2 = 0;
			Int32 iReplaceValueForLastInsertedPKey3 = 0;
			Int32 iReplaceValueForLastInsertedPKey4 = 0;
			Int32 iReplaceValueForLastInsertedPKey5 = 0;
			Int32 iReplaceValueForLastInsertedPKey6 = 0;
			Int32 iReplaceValueForLastInsertedPKey7 = 0;
			Int32 iReplaceValueForLastInsertedPKey8 = 0;
			Int32 iReplaceValueForLastInsertedPKey9 = 0;

			bool bIsTransactionCommitted = false;

			motransaction = moConn.BeginTransaction(IsolationLevel.ReadUncommitted);
			moSqlCommand.Transaction = motransaction;
			try
			{
				foreach (string sCurrentString in asArrTransactionStatements.Where(str => str != null))
				{
					if (sCurrentString.Trim().Substring(0, 6).ToUpper() == "SELECT")
					{
						/**
						 *	The statement is a SELECT.
						 *	Perform the SELECT (which we assume will return one value) and store it
						 *	in a variable depending upon which S_LAST_INSERTED_P_KEY was used in the SELECT.  Currently, the
						 *	code supports 9 temporary DB variables which are defined in ClientServerCommonUtilities.ConstantsAndStructures.vb 
						 *	(S_LAST_INSERTED_P_KEY, S_LAST_INSERTED_P_KEY2, S_LAST_INSERTED_P_KEY3, S_LAST_INSERTED_P_KEY4 ,S_LAST_INSERTED_P_KEY5, S_LAST_INSERTED_P_KEY6, S_LAST_INSERTED_P_KEY7, S_LAST_INSERTED_P_KEY8 and S_LAST_INSERTED_P_KEY9).  
						 *	This value will be used to replace the 
						 *	corresponding database variable constant which may be in a subsequent UPDATE, INSERT, or DELETE 
						 *	transaction statement.
						 */
							
						iReturnValue = ExecuteScalarQuery(sCurrentString).ToInt();

						if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY) > 0)
							iReplaceValueForLastInsertedPKey = iReturnValue;
						else if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY2) > 0)
							iReplaceValueForLastInsertedPKey2 = iReturnValue;
						else if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY3) > 0)
							iReplaceValueForLastInsertedPKey3 = iReturnValue;
						else if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY4) > 0)
							iReplaceValueForLastInsertedPKey4 = iReturnValue;
						else if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY5) > 0)
							iReplaceValueForLastInsertedPKey5 = iReturnValue;
						else if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY6) > 0)
							iReplaceValueForLastInsertedPKey6 = iReturnValue;
						else if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY7) > 0)
							iReplaceValueForLastInsertedPKey7 = iReturnValue;
						else if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY8) > 0)
							iReplaceValueForLastInsertedPKey8 = iReturnValue;
						else if (sCurrentString.IndexOf(Constants.S_LAST_INSERTED_P_KEY9) > 0)
							iReplaceValueForLastInsertedPKey9 = iReturnValue;

						if (iReturnValue == -1)
							return -1;
					}
					else
					{
						/**
						 *	The statement is either an UPDATE, INSERT, or DELETE.
						 *	Replace the database variable constant, S_LAST_INSERTED_P_KEY, S_LAST_INSERTED_P_KEY2, S_LAST_INSERTED_P_KEY3, or S_LAST_INSERTED_P_KEY4,
						 *	(if it is present in the statement) with the value obtained by a previous SELECT statement.
						 */
							
						string sSqlQuery = sCurrentString;
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY , Convert.ToString(iReplaceValueForLastInsertedPKey ));
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY2, Convert.ToString(iReplaceValueForLastInsertedPKey2));
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY3, Convert.ToString(iReplaceValueForLastInsertedPKey3));
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY4, Convert.ToString(iReplaceValueForLastInsertedPKey4));
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY5, Convert.ToString(iReplaceValueForLastInsertedPKey5));
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY6, Convert.ToString(iReplaceValueForLastInsertedPKey6));
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY7, Convert.ToString(iReplaceValueForLastInsertedPKey7));
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY8, Convert.ToString(iReplaceValueForLastInsertedPKey8));
						sSqlQuery = sSqlQuery.Replace(Constants.S_LAST_INSERTED_P_KEY9, Convert.ToString(iReplaceValueForLastInsertedPKey9));
							
						/**
						 *	Replace the server current datetime constant, ServerUtilities.ServerSQLBroker.S_SERVER_CURRENT_DATE_TIME, 
						 *	(if it is present in the statement) with the DB's current datetime.
						 *	We try replacing both with and without the ticks (ie. '').  This is because some client code may have
						 *	used 'S_SERVER_CURRENT_DATE_TIME' in their code and others may have used just S_SERVER_CURRENT_DATE_TIME.
						 */
                        //sSqlQuery = sSqlQuery.Replace(String.Format("'{0}'", Constants.S_SERVER_CURRENT_DATE_TIME), "CURRENT_TIMESTAMP");
                        //sSqlQuery = sSqlQuery.Replace(Constants.S_SERVER_CURRENT_DATE_TIME, "CURRENT_TIMESTAMP");

                        sSqlQuery = sSqlQuery.Replace(String.Format("'{0}'", Constants.S_SERVER_CURRENT_DATE_TIME), "dbo.GetLocalDate(DEFAULT)");
                        sSqlQuery = sSqlQuery.Replace(Constants.S_SERVER_CURRENT_DATE_TIME, "dbo.GetLocalDate(DEFAULT)");

						moSqlCommand.CommandText = sSqlQuery;
						DateTime dtStart = DateTime.Now;
						Stopwatch timer = Stopwatch.StartNew();
							
						try
						{
							moSqlCommand.ExecuteNonQuery();
						}
						finally
						{
							timer.Stop();
							if (Constants.B_ACTIVITY_LOGGING)
								CreateActivityLog(sSqlQuery, false, null, dtStart, timer.ElapsedMilliseconds);
						}
					}
					if (aePrimaryKeyRecord == Constants.PrimaryKeyRecord.First)
					{
						if (asArrTransactionStatements[0].Trim().ToUpper().StartsWith("INSERT") && iReturnValue == 0)
							iReturnValue = ExecuteScalarQuery("SELECT SCOPE_IDENTITY()").ToInt();
					}
					else
					{
						if (asArrTransactionStatements[0].Trim().ToUpper().StartsWith("INSERT"))
							iReturnValue = ExecuteScalarQuery("SELECT SCOPE_IDENTITY()").ToInt();
					}
				}

				motransaction.Commit();
				bIsTransactionCommitted = true;
			}
			catch (SqlException)
			{
				RollBackTransaction(bIsTransactionCommitted);

				// Throw the original exception, so the calling method can handle it.
				throw;
			}
			return iReturnValue;
		}

		/// <summary>
		/// This method expects a "SELECT" statement passed to it.
		/// It will execute the select statement on Sql Server and either return 0 or the first row's first column's value
		/// which is expected to be an integer.
		/// </summary>
		/// <param name="aoImageBinaryData"></param>
		/// <param name="asQuery"></param>
		/// <returns></returns>
		public int ExecuteTransaction(byte[] aoImageBinaryData, String asQuery)
		{
			var oCommand = new SqlCommand(asQuery, moSqlCommand.Connection);

			//'add and initalize a parameter to the SqlCommand
			SqlParameter oSqlParameter = oCommand.Parameters.Add("@Image", SqlDbType.Binary);
			oSqlParameter.Direction = ParameterDirection.Input;
			oSqlParameter.Value = aoImageBinaryData;

			var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
			moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
			DateTime dtStart = DateTime.Now;
			Stopwatch timer = Stopwatch.StartNew();

			int iReturnValue;

			try
			{
				iReturnValue = oCommand.ExecuteScalar().ToInt();
			}
			finally
			{
				timer.Stop();
				if (Constants.B_ACTIVITY_LOGGING)
					CreateActivityLog(asQuery, false, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
			}

			return iReturnValue;
		}

		/// <summary>
		/// This method is used to execute sql statement and return datatable filled with result set.
		/// </summary>
		/// <param name="asCommandText"></param>
		/// <returns></returns>
		public DataTable ExecuteSqlStatementAndGetDataTable(string asCommandText)
		{
			moSqlCommand.CommandText = asCommandText;
			moSqlCommand.CommandType = CommandType.Text;
			
			var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
			moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
			DateTime dtStart = DateTime.Now;
			Stopwatch timer = Stopwatch.StartNew();
			
			DataTable oDataTable;

			try
			{
				oDataTable = ExecuteSqlCommandAndGetDataTable(false);
			}
			finally
			{
				timer.Stop();
				if (Constants.B_ACTIVITY_LOGGING)
					CreateActivityLog(asCommandText, false, oSqlParameters, dtStart, timer.ElapsedMilliseconds);	
			}

			return oDataTable;
		}

        /// <summary>
		/// This method is used to execute sql statement and return datatable filled with result set.
		/// </summary>
		/// <param name="asCommandText"></param>
		/// <returns></returns>
        public DataTable ExecuteSqlStatementAndGetDataTable(string asCommandText, bool abUseTransaction)
        {
            moSqlCommand.CommandText = asCommandText;
            moSqlCommand.CommandType = CommandType.Text;

            var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
            moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
            DateTime dtStart = DateTime.Now;
            Stopwatch timer = Stopwatch.StartNew();

            DataTable oDataTable;

            try
            {
                oDataTable = ExecuteSqlCommandAndGetDataTable(abUseTransaction);
            }
            finally
            {
                timer.Stop();
                if (Constants.B_ACTIVITY_LOGGING)
                    CreateActivityLog(asCommandText, false, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
            }

            return oDataTable;
        }

		/// <summary>
		/// Executes the query and return first column of first row of result.
		/// </summary>
		/// <param name="asCommandText"></param>
		/// <returns></returns>
		public SqlDataReader ExecuteSqlStatementAndGetResults(string asCommandText)
		{
			moSqlCommand.CommandText = asCommandText;
			
			var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
			moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
			DateTime dtStart = DateTime.Now;
			Stopwatch timer = Stopwatch.StartNew();
			
			SqlDataReader oReader;

			try
			{
				oReader = moSqlCommand.ExecuteReader();
			}
			finally
			{
				timer.Stop();
				if (Constants.B_ACTIVITY_LOGGING)
					CreateActivityLog(asCommandText, false, oSqlParameters, dtStart, timer.ElapsedMilliseconds);	
			}

			return oReader;
		}

		/// <summary>
		/// This method is used to execute sql statement and return dataset filled with result set.
		/// </summary>
		/// <param name="asCommandText"></param>
		/// <returns></returns>
		public DataSet ExecuteSqlStatementAndGetDataSet(string asCommandText)
		{
			moSqlCommand.CommandText = asCommandText;
			moSqlCommand.CommandType = CommandType.Text;
			
			var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
			moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
			DateTime dtStart = DateTime.Now;
			Stopwatch timer = Stopwatch.StartNew();
			
			DataSet oDataSet;

			try
			{
				oDataSet = ExecuteSqlCommandAndGetDataSet(false);
			}
			finally
			{
				timer.Stop();
				if (Constants.B_ACTIVITY_LOGGING)
					CreateActivityLog(asCommandText, false, oSqlParameters, dtStart, timer.ElapsedMilliseconds);	
			}

			return oDataSet;
		}

        /// <summary>
        /// This method is used to execute sql statement and return dataset filled with result set.
        /// </summary>
        /// <param name="asCommandText"></param>
        /// <returns></returns>
        public DataSet ExecuteSqlStatementAndGetDataSet(string asCommandText, bool abUseTransaction)
        {
            moSqlCommand.CommandText = asCommandText;
            moSqlCommand.CommandType = CommandType.Text;

            var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
            moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
            DateTime dtStart = DateTime.Now;
            Stopwatch timer = Stopwatch.StartNew();

            DataSet oDataSet;

            try
            {
                oDataSet = ExecuteSqlCommandAndGetDataSet(abUseTransaction);
            }
            finally
            {
                timer.Stop();
                if (Constants.B_ACTIVITY_LOGGING)
                    CreateActivityLog(asCommandText, false, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
            }

            return oDataSet;
        }

		/// <summary>
		/// This method is used to execute stored procedure with given parameter values.
		/// </summary>
		/// <param name="asStoredProcedureName"></param>
		public void ExecuteStoredProcedureOnServer(string asStoredProcedureName)
		{
			bool bIsTransactionCommitted = false;

			try
			{
				motransaction = moConn.BeginTransaction(IsolationLevel.ReadUncommitted);
				moSqlCommand.Transaction = motransaction;
				moSqlCommand.CommandText = asStoredProcedureName;
				moSqlCommand.CommandType = CommandType.StoredProcedure;
				
				var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
				moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
				DateTime dtStart = DateTime.Now;
				Stopwatch timer = Stopwatch.StartNew();
				
				try
				{
					moSqlCommand.ExecuteNonQuery();
					motransaction.Commit();
					bIsTransactionCommitted = true;
				}
				finally
				{
					timer.Stop();
					if (Constants.B_ACTIVITY_LOGGING)
						CreateActivityLog(asStoredProcedureName, true, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
				}
			}
			catch (SqlException)
			{
				RollBackTransaction(bIsTransactionCommitted);

				// Throw the original exception, so the calling method can handle it.
				throw;
			}
			finally
			{
				ClearSQLParameters();
			}
		}

		/// <summary>
		/// This method is used to execute stored procedure with given parameter values and return dataset filled with result set.
		/// </summary>
		/// <param name="asStoredProcedureName"></param>
		/// <returns></returns>
		public DataSet ExecuteStoredProcedureAndGetDataSet(string asStoredProcedureName)
		{
			moSqlCommand.CommandType = CommandType.StoredProcedure;
			moSqlCommand.CommandText = asStoredProcedureName;
			
			var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
			moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
			DateTime dtStart = DateTime.Now;
			Stopwatch timer = Stopwatch.StartNew();
			
			DataSet oDS;

			try
			{
				oDS = ExecuteSqlCommandAndGetDataSet(false);
			}
			finally
			{
				timer.Stop();
				if (Constants.B_ACTIVITY_LOGGING)
					CreateActivityLog(asStoredProcedureName, true, oSqlParameters, dtStart, timer.ElapsedMilliseconds);	
			}
			
			return oDS;
		}

        /// <summary>
        /// This method is used to execute stored procedure with given parameter values and return dataset filled with result set.
        /// </summary>
        /// <param name="asStoredProcedureName"></param>
        /// <returns></returns>
        public DataSet ExecuteStoredProcedureAndGetDataSet(string asStoredProcedureName, bool abUseTransaction)
        {
            moSqlCommand.CommandType = CommandType.StoredProcedure;
            moSqlCommand.CommandText = asStoredProcedureName;

            var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
            moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
            DateTime dtStart = DateTime.Now;
            Stopwatch timer = Stopwatch.StartNew();

            DataSet oDS;

            try
            {
                oDS = ExecuteSqlCommandAndGetDataSet(abUseTransaction);
            }
            finally
            {
                timer.Stop();
                if (Constants.B_ACTIVITY_LOGGING)
                    CreateActivityLog(asStoredProcedureName, true, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
            }

            return oDS;
        }

		/// <summary>
		/// This method is used to execute stored procedure and return datatable filled with result set.
		/// </summary>
		/// <param name="asStoredProcedureName"></param>
		/// <returns></returns>
		public DataTable ExecuteStoredProcedureAndGetDataTable(string asStoredProcedureName)
		{
			moSqlCommand.CommandType = CommandType.StoredProcedure;
			moSqlCommand.CommandText = asStoredProcedureName;
			
			var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
			moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
			DateTime dtStart = DateTime.Now;
			Stopwatch timer = Stopwatch.StartNew();
			
			DataTable oDT;
			
			try
			{
				oDT = ExecuteSqlCommandAndGetDataTable(false);
			}
			finally
			{
				timer.Stop();
				if (Constants.B_ACTIVITY_LOGGING)
					CreateActivityLog(asStoredProcedureName, true, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
			}

			return oDT;
		}

        /// <summary>
        /// This method is used to execute stored procedure and return datatable filled with result set.
        /// </summary>
        /// <param name="asStoredProcedureName"></param>
        /// <returns></returns>
        public DataTable ExecuteStoredProcedureAndGetDataTable(string asStoredProcedureName, bool abUseTransaction)
        {
            moSqlCommand.CommandType = CommandType.StoredProcedure;
            moSqlCommand.CommandText = asStoredProcedureName;

            var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
            moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
            DateTime dtStart = DateTime.Now;
            Stopwatch timer = Stopwatch.StartNew();

            DataTable oDT;

            try
            {
                oDT = ExecuteSqlCommandAndGetDataTable(abUseTransaction);
            }
            finally
            {
                timer.Stop();
                if (Constants.B_ACTIVITY_LOGGING)
                    CreateActivityLog(asStoredProcedureName, true, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
            }

            return oDT;
        }

		/// <summary>
		/// Executes a stored procedure on the server and returns an SqlDataReader for it.
		/// </summary>
		/// <param name="asProcedureName">The StoreProcedure to be executed.</param>
		/// <returns>An SqlDataReader object for the returned result(s).</returns>
		public SqlDataReader ExecuteStoredProcedureAndGetresult(string asProcedureName)
		{
			moSqlCommand.CommandType = CommandType.StoredProcedure;
			moSqlCommand.CommandText = asProcedureName;
			
			var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
			moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
			DateTime dtStart = DateTime.Now;
			Stopwatch timer = Stopwatch.StartNew();
			SqlDataReader oReader;

			try
			{
				oReader = moSqlCommand.ExecuteReader();
			}
			finally
			{
				timer.Stop();
				if (Constants.B_ACTIVITY_LOGGING)
					CreateActivityLog(asProcedureName, true, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
			}
			
			return oReader;
		}

        /// <summary>
        /// Executes a stored procedure on the server and returns an SqlDataReader for it.
        /// </summary>
        /// <param name="asProcedureName">The StoreProcedure to be executed.</param>
        /// <returns>An SqlDataReader object for the returned result(s).</returns>
        public SqlDataReader ExecuteStoredProcedureAndGetresult(string asProcedureName, bool abUseTransaction)
        {
            bool bIsTransactionCompleted = false;
            moSqlCommand.CommandType = CommandType.StoredProcedure;
            moSqlCommand.CommandText = asProcedureName;
            
            var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
            moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
            DateTime dtStart = DateTime.Now;
            Stopwatch timer = Stopwatch.StartNew();
            SqlDataReader oReader;

            try
            {
                StartTransaction(abUseTransaction);
                oReader = moSqlCommand.ExecuteReader();
                bIsTransactionCompleted = true;
                CommitTransaction(abUseTransaction);
            }
            finally
            {
                RollBackCurrentTransaction(abUseTransaction, bIsTransactionCompleted);
                timer.Stop();
                if (Constants.B_ACTIVITY_LOGGING)
                    CreateActivityLog(asProcedureName, true, oSqlParameters, dtStart, timer.ElapsedMilliseconds);
            }

            return oReader;
        }

		#endregion -- PUBLIC METHOD(s) --

		#region -- PRIVATE METHOD(s) --

        /// <summary>
        /// This method is used to initialize sql connection. 
        /// </summary>
        private void InitializeSqlConnection()
        {
            if (Constants.S_CONNECTION_STRING.IsNullOrEmpty())
                throw new ArgumentNullException("Constants.S_CONNECTION_STRING", "Connection string constant have not been initialized. This may occur due to session lost.");

            moConn = new SqlConnection(Constants.S_CONNECTION_STRING);
            moConn.Open();
            moSqlCommand = new SqlCommand
            {
                Connection = moConn,
                CommandTimeout = 720
                //CommandTimeout = 240
            };
        }

        /// <summary>
        /// This method is used to initialize sql connection. 
        /// </summary>
        private void InitializeSqlConnection(string asConnectionString)
        {
            if (asConnectionString.IsNullOrEmpty())
                throw new ArgumentNullException("asConnectionString", "Connection string constant have not been initialized. This may occur due to session lost.");

            moConn = new SqlConnection(asConnectionString);
            moConn.Open();
            moSqlCommand = new SqlCommand
            {
                Connection = moConn,
                CommandTimeout = 720
                //CommandTimeout = 240
            };
        }

		/// <summary>
		/// All held resources are closed by this method
		/// </summary>
		private void CloseHandles()
		{
			if (moSqlCommand != null)
				moSqlCommand.Dispose();
			if (moConn != null)
				moConn.Close();
			if (motransaction != null)
				motransaction.Dispose();
		}

		/// <summary>
		/// Executes the query and return first column of first row of result.
		/// </summary>
		/// <param name="asCommandText"></param>
		/// <returns></returns>
		private object ExecuteScalarQuery(string asCommandText)
		{
			moSqlCommand.CommandText = asCommandText;
			
			var oSqlParameters = new SqlParameter[moSqlCommand.Parameters.Count];
			moSqlCommand.Parameters.CopyTo(oSqlParameters, 0);
			DateTime dtStart = DateTime.Now;
			Stopwatch timer = Stopwatch.StartNew();
			
			object result;

			try
			{
				result = moSqlCommand.ExecuteScalar();
			}
			finally
			{
				timer.Stop();
				if (Constants.B_ACTIVITY_LOGGING)
					CreateActivityLog(asCommandText, false, oSqlParameters, dtStart, timer.ElapsedMilliseconds);	
			}
			
			return result;
		}

		/// <summary>
		/// This method is used to execute sql statement and return datatable filled with result set.
		/// </summary>
		/// <returns></returns>
		private DataTable ExecuteSqlCommandAndGetDataTable()
		{
			var oDataTable = new DataTable();
			bool bIsTransactionCommitted = false;

			try
			{
				motransaction = moConn.BeginTransaction(IsolationLevel.ReadUncommitted);
				moSqlCommand.Transaction = motransaction;
				using (var oAdapter = new SqlDataAdapter(moSqlCommand))
					oAdapter.Fill(oDataTable);
				
				motransaction.Commit();
				bIsTransactionCommitted = true;
			}
			catch (SqlException)
			{
				RollBackTransaction(bIsTransactionCommitted);

				throw;
			}
			finally
			{
				ClearSQLParameters();
			}
			return oDataTable;

		}

        /// <summary>
        /// This method is used to execute sql statement and return datatable filled with result set.
        /// </summary>
        /// <returns></returns>
        private DataTable ExecuteSqlCommandAndGetDataTable(bool abUseTransaction)
        {
            var oDataTable = new DataTable();
            bool bIsTransactionCommitted = false;

            try
            {
                StartTransaction(abUseTransaction);

                using (var oAdapter = new SqlDataAdapter(moSqlCommand))
                    oAdapter.Fill(oDataTable);

                CommitTransaction(abUseTransaction);
                bIsTransactionCommitted = true;
            }
            catch (SqlException)
            {
                RollBackCurrentTransaction(abUseTransaction, bIsTransactionCommitted);

                throw;
            }
            finally
            {
                ClearSQLParameters();
            }
            return oDataTable;

        }

        private void StartTransaction(bool abUseTransaction)
        {
            if (abUseTransaction)
            {
                motransaction = moConn.BeginTransaction(IsolationLevel.ReadUncommitted);
                moSqlCommand.Transaction = motransaction;
            }
        }

        private void CommitTransaction(bool abUseTransaction)
        {
            if (abUseTransaction)
                motransaction.Commit();
        }

        private void RollBackCurrentTransaction(bool abUseTransaction, bool bIsTransactionCommitted)
        {
            if (abUseTransaction)
                RollBackTransaction(bIsTransactionCommitted);
        }
        
		/// <summary>
		/// This method is used to execute sql statement and return datatable filled with result set.
		/// </summary>
		/// <returns></returns>
		private DataSet ExecuteSqlCommandAndGetDataSet()
		{
			var oDataSet = new DataSet();
			bool bIsTransactionCommitted = false;

			try
			{
				motransaction = moConn.BeginTransaction(IsolationLevel.ReadUncommitted);
				moSqlCommand.Transaction = motransaction;
				using (var oAdapter = new SqlDataAdapter(moSqlCommand))
					oAdapter.Fill(oDataSet);
				
				motransaction.Commit();
				bIsTransactionCommitted = true;
			}
			catch (SqlException)
			{
				RollBackTransaction(bIsTransactionCommitted);

				throw;
			}
			finally
			{
				ClearSQLParameters();
			}
			return oDataSet;
		}

        /// <summary>
        /// This method is used to execute sql statement and return datatable filled with result set.
        /// </summary>
        /// <returns></returns>
        private DataSet ExecuteSqlCommandAndGetDataSet(bool abUseTransaction)
        {
            var oDataSet = new DataSet();
            bool bIsTransactionCommitted = false;

            try
            {
                StartTransaction(abUseTransaction);
                //motransaction = moConn.BeginTransaction(IsolationLevel.ReadUncommitted);
                //moSqlCommand.Transaction = motransaction;
                using (var oAdapter = new SqlDataAdapter(moSqlCommand))
                    oAdapter.Fill(oDataSet);

                CommitTransaction(abUseTransaction);
                //motransaction.Commit();
                bIsTransactionCommitted = true;
            }
            catch (SqlException)
            {
                RollBackCurrentTransaction(abUseTransaction, bIsTransactionCommitted);
                //RollBackTransaction(bIsTransactionCommitted);

                throw;
            }
            finally
            {
                ClearSQLParameters();
            }
            return oDataSet;
        }

		/// <summary>
		///		Rolls back the current transaction.
		/// </summary>
		/// <param name="abIsTransactionCommitted">Indicates if the transaction is committed.</param>
		private void RollBackTransaction(bool abIsTransactionCommitted)
		{
			try
			{
				// We try rolling back the transaction if it is not committed.
				// This is wrapped in a try catch since it could possibly raise an exception and swallow the original exception.
				// Incase it does, we simply log that exception to the database and re-throw the original exception.
				if (motransaction != null && !abIsTransactionCommitted)
					motransaction.Rollback();
			}
			catch (Exception ex)
			{
				try
				{
					ErrorLogDC.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
				}
				catch
				{
					// Empty Catch block
					// We are just trying to log the exception, so we don't do anything even if something goes wrong.
				}
			}
		}

		/// <summary>
		///		This method is used to clear sql parameters added to sql command object.
		///		Need to use if there are multiple statements need to fire one by one with this single objects.
		/// </summary>
		private void ClearSQLParameters()
		{
			moSqlCommand.Parameters.Clear();
		}

		/// <summary>
		///		This function is used to create an ActivityLog object and add it to the session.
		/// </summary>
		/// <param name="asSqlStatement">The SQLStatement that was executed.</param>
		/// <param name="abIsSproc">If the CommandType is StoredProcedure.</param>
		/// <param name="aoParameters">The parameters passed to the SQLStatement.</param>
		/// <param name="adtStart">The startime of the transactions.</param>
		/// <param name="alExecutionTime">The time taken to execute the SQLStatement.</param>
		private void CreateActivityLog(string asSqlStatement, bool abIsSproc, SqlParameter[] aoParameters, DateTime adtStart, long alExecutionTime)
		{
			try
			{
                if (mbIsServiceCall && Constants.B_SERVICE_LOGGING_ENABLED)
                {
                        PageRequestLog request = PageRequestOfService;
                        request.ActivityLog.Add(new ActivityLog
                        {
                            SQLStatement = StringUtility.SanitizeXML(asSqlStatement, false, true),
                            Parameters = ConvertParameters(aoParameters),
                            IsSproc = abIsSproc,
                            ExecutionTime = alExecutionTime,
                            InsertDate = adtStart
                        });

                        ActivityLoggingDC.LogActivity(request, null, string.Empty, true);
                }
                else {

                    PageRequestLog request = PageRequestLog;

                    if (request == null || request.ActivityLog == null)
                        return;

                    request.ActivityLog.Add(new ActivityLog
                    {
                        SQLStatement = StringUtility.SanitizeXML(asSqlStatement, false, true),
                        Parameters = ConvertParameters(aoParameters),
                        IsSproc = abIsSproc,
                        ExecutionTime = alExecutionTime,
                        InsertDate = adtStart
                    });
                }
			}
			catch (Exception ex)
			{
				ErrorLogDC.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
			}
		}

		/// <summary>
		///		This function is used to convert a given SqlParemeter Collection into a string object.
		/// </summary>
		/// <param name="aoParameters"></param>
		/// <returns>A String representing the SqlParameterCollection object.</returns>
		private string ConvertParameters(SqlParameter[] aoParameters)
		{
			if (aoParameters.IsNull() || aoParameters.Length <= 0)
				return String.Empty;
			
			var sbParams = new StringBuilder();
			foreach (SqlParameter param in aoParameters)
				sbParams.AppendFormat("{0} {1} = {2}{3}, ",
									   param.ParameterName,
									   param.SqlDbType.ToString().ToUpper(),
									   GetParamValue(param),
									   param.Direction == ParameterDirection.Output ? " OUTPUT" : String.Empty);

			string sParams = sbParams.ToString();
			return sParams.Substring(0, sParams.Length - 2);
		}

		/// <summary>
		///		Returns the value of SqlParameter in string format.
		///		If parameter is of type Char, VarChar, NChar, NVarChar or Xml, value is enclosed in single quotes.
		/// </summary>
		/// <param name="aoParameter"></param>
		/// <returns></returns>
		private string GetParamValue(SqlParameter aoParameter)
		{
			if (aoParameter == null || aoParameter.Value == null)
				return "NULL";
			
			switch (aoParameter.SqlDbType)
			{
				case SqlDbType.Char:
				case SqlDbType.VarChar:
				case SqlDbType.NChar:
				case SqlDbType.NVarChar:
				case SqlDbType.Xml:
				case SqlDbType.Date:
				case SqlDbType.DateTime:
				case SqlDbType.DateTime2:
				case SqlDbType.DateTimeOffset:
				case SqlDbType.SmallDateTime:
					return StringUtility.SanitizeXML(String.Format("'{0}'", aoParameter.Value), false, true);
				default:
					return StringUtility.SanitizeXML(aoParameter.Value.ToString(), false, true);
			}
		}

		#endregion -- PRIVATE METHOD(s) --
	}
}
