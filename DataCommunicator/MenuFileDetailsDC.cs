/* File Name = MenuFileDetailsDC
 * Created Date - 12 July 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage Menu Files.*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
	public class MenuFileDetailsDC
	{
		#region -- MEMBER(s) --

		public MenuFile moMenuFileDetails;

		#endregion -- MEMBER(s) --

		#region -- CONSTRUCTOR(s) --

		public MenuFileDetailsDC()
		{
			moMenuFileDetails = new MenuFile();
		}

		public MenuFileDetailsDC(int aiMenuFileDetailsId)
		{
			moMenuFileDetails = new MenuFile();
			Load(aiMenuFileDetailsId);
		}

		#endregion -- CONSTRUCTOR(s) --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		///		This method is used to get details of all menu files.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="sortExpression"></param>
		/// <param name="aiEndRowIndex"></param>
		/// <param name="aiStartRowIndex"></param>
		/// <returns></returns>
		public List<MenuFile> GetAll(int aiSchoolId,string asSearchText, string sortExpression, int aiEndRowIndex, int aiStartRowIndex)
		{
			var lstMenuFileDetails = new List<MenuFile>();
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolID", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("asSearchText", StringUtility.ReplaceSingleQuoteInString(asSearchText,true), SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("SortExp", "ORDER BY " + (sortExpression.IsNullOrEmpty() ? " ConfigureMenuName" : sortExpression), SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("StartIndex", aiStartRowIndex, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("EndIndex", aiEndRowIndex, SqlDbType.Int);
				
				using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedMenuFilesDetails"))
				{
					if (oSqlDataReader != null)
						while (oSqlDataReader.Read())
                            lstMenuFileDetails.Add(new MenuFile
                                                    {
                                                        Id = oSqlDataReader["Id"].ToInt(),
                                                        Menu = new Menu
                                                                    {
                                                                        Id = oSqlDataReader["MenuId"].ToInt(),
                                                                        Name = oSqlDataReader["MenuName"].ToString(),                                                                        
                                                                        ParentMenu = new Menu
                                                                                        {
                                                                                            Name = oSqlDataReader["ParentMenuName"].ToString()
                                                                                        },
                                                                        SubMenu = new Menu
                                                                                        {
                                                                                            Name = oSqlDataReader["SubMenuName"].ToString()
                                                                                        },
                                                                        ChildMenu = new Menu 
                                                                                        {
                                                                                            Name = oSqlDataReader["ChildMenuName"].ToString()
                                                                                        }
                                                                    },
                                                        Name = oSqlDataReader["Name"].ToString(),
                                                        Path = oSqlDataReader["Path"].ToString(),
                                                        IsURL = oSqlDataReader["IsURL"].ToBool(),
                                                        TotalRows = oSqlDataReader["TotalRows"].ToInt()
                                                    });
				}
			}

			return lstMenuFileDetails;
		}

		/// <summary>
		/// This method is used to get details of a menu file.
		/// </summary>
		/// <param name="aiMenuFileDetailsId"></param>
		private void Load(int aiMenuFileDetailsId)
		{
			string sSelectStatement = "SELECT CM.ConfigureMenuName" +
									  "		 ,MFD.LinkName" +
									  "		 ,MFD.FilePath" +
                                       "	 ,ISNULL(MFD.IsURL,0) AS IsURL" +
									  "		 ,MFD.MenuFileDetailsId" +
									  "		 ,MFD.MenuId " +
									  "  FROM dbo.MenuFileDetails MFD INNER JOIN dbo.ConfigureMenu CM" +
									  "	   ON MFD.MenuId = CM.ConfigureMenuId " +
									  " WHERE MFD.MenuFileDetailsId = " + aiMenuFileDetailsId;
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			using (var oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
			{
				if (oSqlDataReader != null)
					while (oSqlDataReader.Read())
						moMenuFileDetails = new MenuFile
											{
												Id	 = oSqlDataReader["MenuFileDetailsId"].ToInt(),
												Menu = new Menu
														{
															Id	 = oSqlDataReader["MenuId"].ToInt(),
															Name = oSqlDataReader["ConfigureMenuName"].ToString()
														},
												Name = oSqlDataReader["LinkName"].ToString(),
												Path = oSqlDataReader["FilePath"].ToString(),
                                                IsURL = oSqlDataReader["IsURL"].ToBool()

											};
			}
		}

		/// <summary>
		///	Saves the MenuFile details to db.
		/// </summary>
		/// <param name="aoMenuFile"></param>
		/// <param name="abIsNewFile"></param>
		public static void SaveMenuFile(MenuFile aoMenuFile, bool abIsNewFile)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("MenuId"	 , aoMenuFile.Menu.Id	  , SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("MenuFileId", aoMenuFile.Id		  , SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("Name"		 , StringUtility.ReplaceSingleQuoteInString(aoMenuFile.Name, true), SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("Path"		 , aoMenuFile.Path		  , SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("SchoolId"	 , aoMenuFile.SchoolId	  , SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UserId"	 , aoMenuFile.InsertedById, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("IsNewFile" , abIsNewFile			  , SqlDbType.Bit);
                oSQLServerDbUtility.AddParameter("IsURL", aoMenuFile.IsURL,  SqlDbType.Bit);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_InsertUpdateMenuFile");
			}
		}

        public static void UpdateFileDetails(int aiSchoolId, string asOldFilePath, string asNewFilePath, string asURL)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("OldFilePath", asOldFilePath, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("NewFilePath", asNewFilePath, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("IsURL", asURL, SqlDbType.Bit);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_UpdateMenuFile");
			}
		}

		/// <summary>
		///		Deletes the specified menu file.
		/// </summary>
		/// <param name="aiMenuFileId"></param>
		public static void DeleteMenuFile(int aiMenuFileId)
		{
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			{
				oSQLServerDbUtility.AddParameter("MenuFileId", aiMenuFileId, SqlDbType.Int);
				oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteMenuFile");
			}			
		}

		#endregion -- PUBLIC METHOD(s) --
	}
}
