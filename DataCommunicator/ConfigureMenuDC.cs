using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;
using SchoolEntities.Admin;

namespace DataCommunicator
{
	public class ConfigureMenuDC : DataCommunicatorBaseDC
	{
		#region -- STRUCTURE(s) --

		public struct ConfigureMenuStruct
		{
			public int miConfigureMenuId;
			public string msConfigureMenuName;
			public string msConfigureMenuContent;
			public int miPriority;
			public int miSchoolId;
			public int miParentMenuId;
			public char mcIsExternal;
			public char mcIsDefault;
			public char mcIsActive;
			public string msUpdateEndDate;
			public char mcIsOnPopUp;
            public int miInsertedById;
            public int miUpdatedById;
            public string msUserRoleIds;
            public int miSubMenuCount;
            public bool mApplyAllSubMenu;
            public bool mIsApplicable;
            public string mAssociatedStandards;
           
		}

		#endregion -- STRUCTURE(s) --

		#region -- MEMBER(s) --

		private ConfigureMenuStruct moConfigureMenuStruct;

		#endregion -- MEMBER(s) --
		
		#region -- PROPERTIES --

		/// <summary>
		///		Exposes the Menu details struct.
		/// </summary>
		public ConfigureMenuStruct ConfigureMenuStructDetails
		{
			get { return moConfigureMenuStruct; }
			set { moConfigureMenuStruct = value; }
		}

		#endregion -- PROPERTIES --

		#region -- CONSTRUCTOR(s) --

		/// <summary>
		///		Default constructor.
		/// </summary>
		public ConfigureMenuDC()
		{
		}
	
		/// <summary>
		///		Initializes the class and loads the specified menu.
		/// </summary>
		/// <param name="aiId"></param>
		public ConfigureMenuDC(int aiId)
		{
			LoadConfigureMenuDetails(aiId);
		}
	
		#endregion -- CONSTRUCTOR(s) --

		#region -- PUBLIC METHO(s) --

		public void LoadConfigureMenuDetails(int aiId)
		{
			string sSelectStatement = FetchConfigureMenuDataFromDatabase(aiId);
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			using (SqlDataReader oDR = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
			{
				if (oDR != null)
				{
					while (oDR.Read())
					{
						if (oDR["ConfigureMenuId"] != DBNull.Value)
							moConfigureMenuStruct.miConfigureMenuId = oDR["ConfigureMenuId"].ToInt();
						if (oDR["ConfigureMenuName"] != DBNull.Value)
							moConfigureMenuStruct.msConfigureMenuName = oDR["ConfigureMenuName"].ToString();
						if (oDR["ConfigureMenuContent"] != DBNull.Value)
							moConfigureMenuStruct.msConfigureMenuContent = oDR["ConfigureMenuContent"].ToString();
						if (oDR["Parent_Menu_Id"] != DBNull.Value)
							moConfigureMenuStruct.miParentMenuId = oDR["Parent_Menu_Id"].ToInt();
						if (oDR["Is_External"] != DBNull.Value)
							moConfigureMenuStruct.mcIsExternal = Convert.ToChar(oDR["Is_External"].ToString());
						if (oDR["End_Date"] != DBNull.Value)
							moConfigureMenuStruct.msUpdateEndDate = oDR["End_Date"].ToString();
						if (oDR["Is_Default"] != DBNull.Value)
							moConfigureMenuStruct.mcIsDefault = Convert.ToChar(oDR["Is_Default"].ToString());
						if (oDR["Priority"] != DBNull.Value)
							moConfigureMenuStruct.miPriority = oDR["Priority"].ToInt();
						if (oDR["Is_Active"] != DBNull.Value)
							moConfigureMenuStruct.mcIsActive = Convert.ToChar(oDR["Is_Active"].ToString());
						if (oDR["IsOnPopUp"] != DBNull.Value)
							moConfigureMenuStruct.mcIsOnPopUp = Convert.ToChar(oDR["IsOnPopUp"].ToString());
                        if (oDR["SubMenuCount"] != DBNull.Value)
                            moConfigureMenuStruct.miSubMenuCount =oDR["SubMenuCount"].ToInt();
                        if (oDR["ApplyAllSubMenu"] != DBNull.Value)
                            moConfigureMenuStruct.mApplyAllSubMenu = Convert.ToBoolean(oDR["ApplyAllSubMenu"].ToBool());

					}
				}
			}
		}

		public string FetchConfigureMenuDataFromDatabase(int aiId)
		{
            string sSelectStatement = " SELECT  " +
                "configuremenuid" +
                " , configuremenuname" +
                " , configuremenucontent" +
                " , Priority" +
                " , End_Date" +
                " , Parent_Menu_Id " +
                " , Is_External " +
                " , Is_Default " +
                " , Is_Active " +
                " , IsOnPopUp " +
                " , SubMenuCount " +
                " , ApplyAllSubMenu " +

            " FROM  " +
                "ConfigureMenu " +
            " WHERE  " +
                 "configuremenuid = " + aiId +
                 "AND  IsDeleted=0";
			return sSelectStatement;
		}

        /// <summary>
        /// This method is used to retrieve User Role Ids for selected menu.
        /// </summary>
        /// <param name="aiMenuId"></param>
        /// <returns></returns>
        public DataTable GetUserRolesForSelectedMenuId(int aiMenuId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("MenuId", aiMenuId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetUserRolesForSelectedMenuId");
            }
        }

		public Int32 InsertConfigureMenu(string sEndDate, int aiAcademicYearId)
		{
			string sInsertStatement;
			if (sEndDate != Constants.S_EMPTY_STRING)
			{
				sInsertStatement = "INSERT INTO ConfigureMenu ( " +
					"  configuremenuname" +
					" , configuremenucontent" +
					" , Priority" +
					" , End_Date" +
					" , SchoolId" +
					" , Parent_Menu_Id" +
					" , Is_External " +
					" , Is_Default " +
					" , Is_Active " +
					" , IsOnPopUp " +
                    " , IsDeleted "+
                    " , InsertedById " +
                     " , InsertDate " +
                    " , UpdatedById " +
                    " , UpdateDate " +
                    " , SubMenuCount " +
                    " , ApplyAllSubMenu " +
                  	" ) VALUES (" +
					 "   N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuName, false) + "' " +
					 " , N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuContent, false) + "' " +
					 " ,  " + moConfigureMenuStruct.miPriority +
					 " , N'" + sEndDate + "'" +
					 " ,  " + moConfigureMenuStruct.miSchoolId +
					 " ,  " + moConfigureMenuStruct.miParentMenuId +
					 " , N'" + moConfigureMenuStruct.mcIsExternal + "'" +
					 " , N'" + moConfigureMenuStruct.mcIsDefault + "'" +
					 " , N'" + moConfigureMenuStruct.mcIsActive + "'" +
					 " , N'" + moConfigureMenuStruct.mcIsOnPopUp + "'" +
                     ", N'"  + 0 +"'"+
                     " , N'" + moConfigureMenuStruct.miInsertedById + "'" +
                     " , N'" + DateTime.Now + "'" +
                     " , N'" + moConfigureMenuStruct.miUpdatedById + "'" +
                     " , N'" + DateTime.Now + "'" +
                     " , N'" + moConfigureMenuStruct.miSubMenuCount + "'" +
                     " , N'" + moConfigureMenuStruct.mApplyAllSubMenu + "'" +
                      " ) ";
			}
			else
			{
				sInsertStatement = "INSERT INTO ConfigureMenu ( " +
				   "  configuremenuname" +
				   " , configuremenucontent" +
				   " , Priority" +
				   " , SchoolId" +
				   " , Parent_Menu_Id" +
				   " , Is_External " +
				   " , Is_Default " +
				   " , Is_Active " +
				   " , IsOnPopUp " +
                    ", IsDeleted " +
                   " , InsertedById " +
                   " , InsertDate " +
                   " , UpdatedById " +
                   " , UpdateDate " +
                    " , SubMenuCount " +
                    " , ApplyAllSubMenu " +
                    " ) VALUES (" +
					"   N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuName, false) + "' " +
					" , N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuContent, false) + "' " +
					" ,  " + moConfigureMenuStruct.miPriority +
					" ,  " + moConfigureMenuStruct.miSchoolId +
					" ,  " + moConfigureMenuStruct.miParentMenuId +
					" , N'" + moConfigureMenuStruct.mcIsExternal + "'" +
					" , N'" + moConfigureMenuStruct.mcIsDefault + "'" +
					" , N'" + moConfigureMenuStruct.mcIsActive + "'" +
					" , N'" + moConfigureMenuStruct.mcIsOnPopUp + "'" +
                      ", N'" + 0 + "'" +
                    " , N'" + moConfigureMenuStruct.miInsertedById + "'" +
                    " , N'" + DateTime.Now + "'" +
                    " , N'" + moConfigureMenuStruct.miUpdatedById + "'" +
                    " , N'" + DateTime.Now + "'" +
                     " , N'" + moConfigureMenuStruct.miSubMenuCount + "'" +
                     " , N'" + moConfigureMenuStruct.mApplyAllSubMenu + "'" +
                  " ) ";
           }

            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
               int iMenuId =  oSQLServerDbUtility.ExecuteTransaction(sInsertStatement);
               oSQLServerDbUtility.AddParameter("ConfigureMenuId", iMenuId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("InsertedById", moConfigureMenuStruct.miInsertedById, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("UpdatedById ", moConfigureMenuStruct.miUpdatedById, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("UserRoleIds", moConfigureMenuStruct.msUserRoleIds, SqlDbType.NVarChar);
               oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertConfigureMenuDetails");

               oSQLServerDbUtility.AddParameter("ConfigureMenuId", iMenuId, SqlDbType.Int);               
               oSQLServerDbUtility.AddParameter("UpdatedById", moConfigureMenuStruct.miUpdatedById, SqlDbType.Int);               
               oSQLServerDbUtility.AddParameter("AssociatedStdDivIds", moConfigureMenuStruct.mAssociatedStandards, SqlDbType.NVarChar);
               oSQLServerDbUtility.AddParameter("SchoolId", moConfigureMenuStruct.miSchoolId, SqlDbType.Int);
               oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
               oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveConfigMenuAssociatedClasses");

               return iMenuId;
            }
         
           
		}

		public string GetInsetStatementForDefaultConfigureMenu()
		{
			string sInsertStatement = "INSERT INTO ConfigureMenu ( " +
				"  configuremenuname" +
				" , configuremenucontent" +
				" , Priority" +
				" , SchoolId" +
				" , Parent_Menu_Id" +
				" , Is_External " +
				" , Is_Default " +
				" , IsOnPopUp " +
                " , IsDeleted " +
                " , InsertedById " +
                " , InsertDate " +
               " , Is_Active " +
                 " , SubMenuCount " +
                   " , ApplyAllSubMenu " +

			" ) VALUES (" +
				 "   N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuName, false) + "' " +
				 " , N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuContent, false) + "' " +
				 " ,  " + moConfigureMenuStruct.miPriority +
				 " ,  " + Constants.S_LAST_INSERTED_P_KEY +
				 " ,  " + moConfigureMenuStruct.miParentMenuId +
				 " , N'" + moConfigureMenuStruct.mcIsExternal + "'" +
				 " , N'" + moConfigureMenuStruct.mcIsDefault + "'" +
				 " , N'" + moConfigureMenuStruct.mcIsOnPopUp + "'" +
                  " , N'" + 0 + "'" +
                 " , N'" + moConfigureMenuStruct.miInsertedById + "'" +
                  " , N'" + DateTime.Now + "'" +
                  " , N'" + Constants.C_YES + "'" +
                   " , N'" + moConfigureMenuStruct.miSubMenuCount + "'" +
                    " , N'" + moConfigureMenuStruct.mApplyAllSubMenu + "'" +

			" ) ";
			return sInsertStatement;
		}

		public void UpdateConfigureMenu(bool abIncludeParent, string sUpdateEndDate, int aiAcademicYearId)
		{
			string sUpdateStatement;
			if (sUpdateEndDate != null)
			{
                sUpdateStatement = " UPDATE ConfigureMenu SET " +
               "   configuremenuname =  N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuName, false) + "' " +
               " , configuremenucontent =  N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuContent, false) + "' " +
               " , Priority =  " + moConfigureMenuStruct.miPriority +
               " , End_Date= N'" + StringUtility.ReplaceSingleQuoteInString(sUpdateEndDate, false) + "' " +
               " , Is_External  = N'" + moConfigureMenuStruct.mcIsExternal + "'" +
               " , Is_Default  = N'" + moConfigureMenuStruct.mcIsDefault + "'" +
               " , IsOnPopUp  = N'" + moConfigureMenuStruct.mcIsOnPopUp + "'" +
              " , UpdatedById  = N'" + moConfigureMenuStruct.miUpdatedById + "'" +
               " , UpdateDate  = N'" + DateTime.Now + "'" +
               " , Is_Active  = N'" + moConfigureMenuStruct.mcIsActive+"'"+
               " , SubMenuCount  = N'" + moConfigureMenuStruct.miSubMenuCount + "'" +
               " , ApplyAllSubMenu  = N'" + moConfigureMenuStruct.mApplyAllSubMenu + "'";
              
				if (abIncludeParent)
					sUpdateStatement = sUpdateStatement + " , Parent_Menu_Id =  " + moConfigureMenuStruct.miParentMenuId;

				sUpdateStatement = sUpdateStatement + " WHERE " +
									" configuremenuid =  " + moConfigureMenuStruct.miConfigureMenuId +
                                    "AND  IsDeleted=0"; 
			}
			else
			{
                sUpdateStatement = " UPDATE ConfigureMenu SET " +
               "   configuremenuname =  N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuName, false) + "' " +
               " , configuremenucontent =  N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuContent, false) + "' " +
               " , Priority =  " + moConfigureMenuStruct.miPriority +
               " , End_Date= " + "NULL" +
               " , Is_External  = N'" + moConfigureMenuStruct.mcIsExternal + "'" +
               " , Is_Default  = N'" + moConfigureMenuStruct.mcIsDefault + "'" +
               " , IsOnPopUp  = N'" + moConfigureMenuStruct.mcIsOnPopUp + "'" +
               " , UpdatedById  = N'" + moConfigureMenuStruct.miUpdatedById + "'" +
               " , UpdateDate  = N'" + DateTime.Now + "'" +
               " , Is_Active  = N'" + moConfigureMenuStruct.mcIsActive+"'"+
               " , SubMenuCount  = N'" + moConfigureMenuStruct.miSubMenuCount + "'" +
               " , ApplyAllSubMenu  = N'" + moConfigureMenuStruct.mApplyAllSubMenu + "'";
              
				if (abIncludeParent)
					sUpdateStatement = sUpdateStatement + " , Parent_Menu_Id =  " + moConfigureMenuStruct.miParentMenuId;

				sUpdateStatement = sUpdateStatement + " WHERE " +
									" configuremenuid =  " + moConfigureMenuStruct.miConfigureMenuId +
                                     "AND  IsDeleted=0"; 
			}
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
                oSQLServerDbUtility.AddParameter("ConfigureMenuId", moConfigureMenuStruct.miConfigureMenuId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", moConfigureMenuStruct.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById ", moConfigureMenuStruct.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleIds", moConfigureMenuStruct.msUserRoleIds, SqlDbType.NVarChar);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertConfigureMenuDetails");

                
                oSQLServerDbUtility.AddParameter("ConfigureMenuId", moConfigureMenuStruct.miConfigureMenuId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", moConfigureMenuStruct.miUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AssociatedStdDivIds", moConfigureMenuStruct.mAssociatedStandards, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", moConfigureMenuStruct.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);                
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_SaveConfigMenuAssociatedClasses");
                
            }
		}

		public void UpdateChildNodes(string sUpdateEndDate)
		{
			string sUpdateStatement;
			if (sUpdateEndDate != null)
			{
				sUpdateStatement = " UPDATE ConfigureMenu SET " +
									"Is_Active  = N'" + moConfigureMenuStruct.mcIsActive + "'" +
									" , Is_External  = N'" + moConfigureMenuStruct.mcIsExternal + "'" +
									" , IsOnPopUp  = N'" + moConfigureMenuStruct.mcIsOnPopUp + "'" +
                                    " , End_Date= N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msUpdateEndDate, false) + "' " +
									" WHERE " +
										" Parent_Menu_Id =  " + moConfigureMenuStruct.miConfigureMenuId +
                                        "AND  IsDeleted=0"; ;
			}
			else
			{
				sUpdateStatement = " UPDATE ConfigureMenu SET " +
									"Is_Active  = N'" + moConfigureMenuStruct.mcIsActive + "'" +
									" , Is_External  = N'" + moConfigureMenuStruct.mcIsExternal + "'" +
									" , IsOnPopUp  = N'" + moConfigureMenuStruct.mcIsOnPopUp + "'" +
                                    " , End_Date= " + "NULL" +
                                    " , SubMenuCount  = N'" + moConfigureMenuStruct.miSubMenuCount + "'" +
                                    " , ApplyAllSubMenu  = N'" + moConfigureMenuStruct.mApplyAllSubMenu + "'" +
									" WHERE " +
										" Parent_Menu_Id =  " + moConfigureMenuStruct.miConfigureMenuId +
                                        "AND  IsDeleted=0";
			}
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
		}

		public void DeleteConfigureMenu()
		{
            string sUpdateStatement = " UPDATE ConfigureMenu SET " +
                                      " IsDeleted=1 " +
                                        " , UpdatedById  = N'" + moConfigureMenuStruct.miUpdatedById + "'" +
                                        " , UpdateDate  = N'" + DateTime.Now +"'"+
                                       "WHERE" +
                                       " Parent_Menu_Id =  " + moConfigureMenuStruct.miConfigureMenuId+
                                        "AND  IsDeleted=0"+ 
                                       ";"+
                                       " UPDATE ConfigureMenu SET " +
                                       " IsDeleted=1 " +
                                       " , UpdatedById  = N'" + moConfigureMenuStruct.miUpdatedById + "'" +
                                       " , UpdateDate  = N'" + DateTime.Now + "'" +
                                       "WHERE" +
                                       " configuremenuid =  " + moConfigureMenuStruct.miConfigureMenuId+
                                        "AND  IsDeleted=0" +
                                        ";" +
                                        " UPDATE ConfigMenuAssociatedClasses SET " +
                                        " IsDeleted = 1, " +
                                        " UpdatedById = " + moConfigureMenuStruct.miUpdatedById + ", " +
                                        " UpdatedDate = GETDATE() " +
                                        " WHERE ConfigMenuId = " + moConfigureMenuStruct.miConfigureMenuId +
                                        " AND IsDeleted = 0";

			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sUpdateStatement);
		}

		public DataTable FetchMenuContentDetails(int aiMenuId)
		{
			//This function is used to fetch the Menu contents details.

			string sSelectStatement = " SELECT " +
											" ConfigureMenu.ConfigureMenuName, " +
											" ConfigureMenu_1.ConfigureMenuName AS ParentMenuName, " +
											" ConfigureMenu.ConfigureMenuId, " +
											" ConfigureMenu.ConfigureMenuContent, " +
											" ConfigureMenu.Priority, " +
											" ConfigureMenu.Parent_Menu_Id, " +
											" ConfigureMenu.Is_External, " +
											" ConfigureMenu.Is_Default " +
                                            " ConfigureMenu.SubMenuCount " +
                                            " ConfigureMenu.ApplyAllSubMenu " +
										" FROM " +
											" ConfigureMenu LEFT OUTER JOIN " +
											" ConfigureMenu AS ConfigureMenu_1 ON ConfigureMenu.Parent_Menu_Id = ConfigureMenu_1.ConfigureMenuId " +
										" WHERE " +
											" ConfigureMenu.ConfigureMenuId = " + aiMenuId+"'";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
		}

		public static DataTable FetchMenuContentDetails()
		{
			string sSelectStatement = " SELECT TOP 1" +
										   " ConfigureMenu.ConfigureMenuName, " +
										   " ConfigureMenu_1.ConfigureMenuName AS ParentMenuName, " +
										   " ConfigureMenu.ConfigureMenuId, " +
										   " ConfigureMenu.ConfigureMenuContent, " +
										   " ConfigureMenu.Priority, " +
										   " ConfigureMenu.Parent_Menu_Id, " +
										   " ConfigureMenu.Is_External, " +
										   " ConfigureMenu.Is_Default " +
                                            " ConfigureMenu.SubMenuCount " +
                                            " ConfigureMenu.ApplyAllSubMenu " +
									   " FROM " +
										   " ConfigureMenu LEFT OUTER JOIN " +
										   " ConfigureMenu AS ConfigureMenu_1 ON ConfigureMenu.Parent_Menu_Id = ConfigureMenu_1.ConfigureMenuId " +
                                           " AND ConfigureMenu_1 IsDeleted=0" +
									   " WHERE " +
										   " ConfigureMenu.Is_External = 'Y'" +
										   " AND ConfigureMenu.Is_Active = 'Y'" +
                                           " AND ConfigureMenu.IsDeleted=0" +
										" ORDER BY ConfigureMenu.Priority";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
		}

		public int IsMenuNameAlreadyExists()
		{
			//This function is used to check whether the menu name already exists or not.
			//if yes then return true otherwise false.

            string sSelectString = "SELECT ConfigureMenuId " +
                                 " FROM " +
                                        " ConfigureMenu " +
                                 " WHERE " +
                                        " ConfigureMenuName= N'" + StringUtility.ReplaceSingleQuoteInString(moConfigureMenuStruct.msConfigureMenuName, false) + "'" +
                                        " AND Parent_Menu_Id=" + moConfigureMenuStruct.miParentMenuId +
                                        " AND SchoolId = " + moConfigureMenuStruct.miSchoolId +
                                        " AND IsDeleted = 0";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectString);
		}

		public int IsPriorityAlreadyExists()
		{
			string sSelectString = "SELECT Priority " +
								 " FROM " +
										" ConfigureMenu " +
								 " WHERE " +
										" Priority=" + moConfigureMenuStruct.miPriority +
										 " AND Parent_Menu_Id=" + moConfigureMenuStruct.miParentMenuId +
										" AND SchoolId = " + moConfigureMenuStruct.miSchoolId;
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectString);
		}

        public int GetMenuIdByMenuName(string asMenuName, int aiParentMenuId, int aiSchoolId)
		{
			//This function is used to get the Menu id by menu name.

			string sSelectString = "SELECT ConfigureMenuId FROM ConfigureMenu WHERE" +
                                        " Parent_Menu_Id = " + aiParentMenuId + " AND ConfigureMenuName = N'" + StringUtility.ReplaceSingleQuoteInString(asMenuName, false) + "'" +
										" AND SchoolId = " + aiSchoolId+ "AND IsDeleted=0" ;
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectString);
		}

		public int GetHighestPriority(int aiSchoolId)
		{
			string sSelectStatement = "SELECT " +
									"MAX(Priority)" +
									" FROM " +
									"ConfigureMenu" +
									" WHERE " +
									"SchoolId = " + aiSchoolId +
                                    "AND IsDeleted= 0 ";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
		}

		public static int IsChildMenu(int aiSchoolId, int aiMenuId)
		{
			string sSelectStatement = "SELECT " +
									  "Parent_Menu_Id" +
									  " FROM " +
									  "ConfigureMenu" +
									  " WHERE " +
									  "SchoolId = " + aiSchoolId +
                                      " AND IsDeleted = 0 " +
									  " AND ConfigureMenuId =" + aiMenuId +
									  " AND Is_External = 'Y'";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.PerformIntQueryOnSqlServer(sSelectStatement);
		}

		public DataTable FetchSchoolNoticess()
		{
			string sSelectStatement = " SELECT TOP 2" +
										   " ConfigureMenu.ConfigureMenuName, " +
										   " ConfigureMenu_1.ConfigureMenuName AS ParentMenuName, " +
										   " ConfigureMenu.ConfigureMenuId, " +
										   " ConfigureMenu.ConfigureMenuContent, " +
										   " ConfigureMenu.Priority, " +
										   " ConfigureMenu.Parent_Menu_Id, " +
										   " ConfigureMenu.Is_External, " +
										   " ConfigureMenu.Is_Default, " +
                                            " ConfigureMenu.SubMenuCount, " +
                                            " ConfigureMenu.ApplyAllSubMenu " +
									   " FROM " +
										   " ConfigureMenu LEFT OUTER JOIN " +
										   " ConfigureMenu AS ConfigureMenu_1 ON ConfigureMenu.Parent_Menu_Id = ConfigureMenu_1.ConfigureMenuId " +
                                           " AND ConfigureMenu_1.IsDeleted=0" +
									   " WHERE " +
										   " ConfigureMenu.Is_Active = 'Y'" +
										   " AND ConfigureMenu.IsOnPopUp = 'Y'" +
										   " AND ConfigureMenu.Parent_Menu_Id <> 0 " +
                                           "AND ConfigureMenu.IsDeleted=0 " +
										   " ORDER BY ConfigureMenu.Priority";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sSelectStatement);
		}

		/// <summary>
		///		Gets the specified menu.
		/// </summary>
		/// <param name="aiMenuId"></param>
		/// <returns></returns>
		public Menu GetMenu(int aiMenuId)
		{
			var oMenu = new Menu();
			
			using (var oSqlServerDbUtility = new SQLServerDbUtility())
			{
				oSqlServerDbUtility.AddParameter("MenuId", aiMenuId, SqlDbType.Int);
				using (var oReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetMenuDetails"))
				{
					if (oReader.HasRows && oReader.Read())
					{
						oMenu = new Menu
								{
									Id		  = oReader["Id"].ToInt(),
									Name	  = oReader["Name"].ToString(),
									Content   = oReader["Content"].ToString(),
									MenuFiles = new List<MenuFile>()
								};

						if (oReader.NextResult())
							while (oReader.Read())
							{
								oMenu.MenuFiles.Add(new MenuFile
													{
														Name = oReader["Name"].ToString(),
														Path = oReader["Path"].ToString(),
                                                        IsURL = oReader["IsURL"].ToBool()
													});
							}
					}
				}
			}

			return oMenu;
		}

        /// <summary>
        /// this method is used to get associated standard and divisions.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiMenuId"></param>
        /// <returns></returns>
        public List<ConfigMenuAssociatedClasses> GetConfigMenuAssociatedClasses(int aiSchoolId, int aiAcademicYearId, int aiMenuId)
        {
            List<ConfigMenuAssociatedClasses> lstConfigMenuAssociatedClasses = new List<ConfigMenuAssociatedClasses>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MenuId", aiMenuId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllConfigMenuAssociatedClasses"))
                {
                    while (oSqlDataReader.Read())
                    {
                        ConfigMenuAssociatedClasses oStandardDivisionMaster = new ConfigMenuAssociatedClasses
                        {
                            StandardwiseDivisionId = oSqlDataReader["SchoolWise_Standard_Division_Id"].ToInt(),
                            StandardId = oSqlDataReader["Standard_Id"].ToInt(),
                            StandardName = oSqlDataReader["Standard_Name"].ToString(),
                            DivisionId = oSqlDataReader["Division_Id"].ToInt(),
                            DivisionName = oSqlDataReader["Division_Name"].ToString(),
                            SavedStandardDivisionId = oSqlDataReader["SavedStandardDivisionId"].ToInt(),
                            IsRecordSaved = oSqlDataReader["IsRecordSaved"].ToBool(),

                        };
                        lstConfigMenuAssociatedClasses.Add(oStandardDivisionMaster);
                    }
                    return lstConfigMenuAssociatedClasses;
                }
            }

        }
    }

		#endregion -- PUBLIC METHO(s) --

  public class ConfigureCollectionMenuDC
	{
		#region -- PUBLIC METHOD(s) --

		public DataTable FetchConfigureMenuCollection(int aiSchoolId)
		{
			//This function is used to fetch the configuration menu collection Items. 

            string sFetchString = "SELECT ConfigureMenuId, ConfigureMenuName, Parent_Menu_Id,Priority,Is_External,End_Date,SubMenuCount,ApplyAllSubMenu FROM ConfigureMenu " +
									" WHERE SchoolId = " + aiSchoolId + "AND IsDeleted= 0 "+
									" ORDER BY Priority,ConfigureMenuName";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchString);
		}

        public DataTable FetchAllInternalMenus(int aiSchoolId, string asFilterString, bool abShowOnlyActive)
		{
			//This function is used to fetch the configuration menu collection Items. 

            if (asFilterString.Contains("'"))
                asFilterString = asFilterString.Replace("'", "''");

            string sFetchString;
            if (abShowOnlyActive)
            {
                sFetchString = "SELECT CM1.ConfigureMenuId, CM1.ConfigureMenuName, CM1.Parent_Menu_Id, ISNULL(CM4.ConfigureMenuName + ' - ', '') + ISNULL(CM3.ConfigureMenuName + ' - ', '') + ISNULL(CM2.ConfigureMenuName + ' - ', '')  + CM1.ConfigureMenuName [MenuName], CM1.Priority, CM1.Is_External, CM1.End_Date,CM1.SubMenuCount,CM1.ApplyAllSubMenu, CM1.InsertDate" +
                                      "  FROM dbo.ConfigureMenu CM1 LEFT OUTER JOIN dbo.ConfigureMenu CM2" +
                                      "	   ON CM1.Parent_Menu_Id = CM2.ConfigureMenuId" +
                                      " AND CM2.IsDeleted= 0 AND CM2.Is_Active = 'Y'" +
                                      "   LEFT OUTER JOIN dbo.ConfigureMenu CM3 ON CM2.Parent_Menu_Id = CM3.ConfigureMenuId" + " " +
                                      " And CM3.IsDeleted=0 AND CM3.Is_Active = 'Y'" +
                                      "   LEFT OUTER JOIN dbo.ConfigureMenu CM4 ON CM3.Parent_Menu_Id = CM4.ConfigureMenuId" + " " +
                                      " And CM4.IsDeleted=0 AND CM4.Is_Active = 'Y'" +
                                      " WHERE CM1.Is_Active = 'Y' AND CM1.SchoolId = " + aiSchoolId + "AND CM1.IsDeleted=0 AND (CM1.ConfigureMenuName LIKE '%" + asFilterString + "%' OR CM2.ConfigureMenuName LIKE '%" + asFilterString + "%' OR CM3.ConfigureMenuName LIKE '%" + asFilterString + "%' OR CM4.ConfigureMenuName LIKE '%" + asFilterString + "%')" +
                                      " ORDER BY CM1.Priority, CM1.ConfigureMenuName";
            }
            else
            {
                sFetchString = "SELECT CM1.ConfigureMenuId, CM1.ConfigureMenuName, CM1.Parent_Menu_Id, ISNULL(CM4.ConfigureMenuName + ' - ', '') + ISNULL(CM3.ConfigureMenuName + ' - ', '') + ISNULL(CM2.ConfigureMenuName + ' - ', '')  + CM1.ConfigureMenuName [MenuName], CM1.Priority, CM1.Is_External, CM1.End_Date,CM1.SubMenuCount,CM1.ApplyAllSubMenu, CM1.InsertDate" +
                                      "  FROM dbo.ConfigureMenu CM1 LEFT OUTER JOIN dbo.ConfigureMenu CM2" +
                                      "	   ON CM1.Parent_Menu_Id = CM2.ConfigureMenuId" +
                                      " AND CM2.IsDeleted= 0" +
                                      "   LEFT OUTER JOIN dbo.ConfigureMenu CM3 ON CM2.Parent_Menu_Id = CM3.ConfigureMenuId" + " " +
                                      " And CM3.IsDeleted=0" +
                                      "   LEFT OUTER JOIN dbo.ConfigureMenu CM4 ON CM3.Parent_Menu_Id = CM4.ConfigureMenuId" + " " +
                                      " And CM4.IsDeleted=0" +
                                      " WHERE CM1.SchoolId = " + aiSchoolId + "AND CM1.IsDeleted=0 AND (CM1.ConfigureMenuName LIKE '%" + asFilterString + "%' OR CM2.ConfigureMenuName LIKE '%" + asFilterString + "%' OR CM3.ConfigureMenuName LIKE '%" + asFilterString + "%' OR CM4.ConfigureMenuName LIKE '%" + asFilterString + "%')" +
                                      " ORDER BY CM1.Priority, CM1.ConfigureMenuName";
            }
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchString);
		}

        public DataTable FetchAllActiveInternalMenus(int aiSchoolId, int aiUserRoleId)
		{
			//This function is used to fetch the configuration menu collection Items. 

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserRoleId", aiUserRoleId, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetConfiguredInternalMenuDetails");
            }
		}

		public DataTable FetchAllExternalMenus(int aiSchoolId)
		{
			//This function is used to fetch the configuration menu collection Items. 

            string sFetchString = "SELECT ConfigureMenuId, ConfigureMenuName, Parent_Menu_Id,Priority,Is_External,Is_Active,End_Date,SubMenuCount,ApplyAllSubMenu FROM ConfigureMenu " +
									" WHERE SchoolId = " + aiSchoolId + "AND IsDeleted= 0 "+
                                    " AND is_external = N'" + Constants.C_YES + "'" +
									" ORDER BY Priority,ConfigureMenuName";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchString);
		}

		public DataTable GetAllParentMenus(int aiCurrentMenuId, int aiSchoolId)
		{
			string sWhereClause = String.Empty;
			if (aiCurrentMenuId != 0)
				sWhereClause = " AND ConfigureMenuId <> " + aiCurrentMenuId;

            string sFetchString = "SELECT ConfigureMenuId, ConfigureMenuName, Priority, Parent_Menu_Id, Is_External, End_Date,SubMenuCount,ApplyAllSubMenu FROM ConfigureMenu " +
									" WHERE SchoolId = " + aiSchoolId +
									" AND Parent_Menu_Id = 0 " +
									sWhereClause +
                                    " AND IsDeleted= 0 "+
									" ORDER BY Priority,ConfigureMenuName";
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
				return oSQLServerDbUtility.ExecuteSqlStatementAndGetDataTable(sFetchString);
		}

        public DataTable GetAllSubMenus(int aiCurrentMenuId, int aiSchoolId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("ParentMenuId", aiCurrentMenuId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);

                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetAllSubMenus");
            }
        }

		#endregion -- PUBLIC METHOD(s) --
	}
}
