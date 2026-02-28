using System.Data;
using DataCommunicator;
using SchoolEntities;
using System.Collections.Generic;
using SchoolEntities.Admin;

namespace BusinessLogic
{
    public class ConfigureMenuBL
    {

        #region -- MEMBER(s) --

        private ConfigureMenuDC.ConfigureMenuStruct moConfigureMenuStruct;
        private ConfigureMenuDC moConfigureMenuDC = new ConfigureMenuDC();
        
		#endregion -- MEMBER(s) --
        
		#region -- PROPERTIES --

        public int ConfigureMenuId
        {

            get { return moConfigureMenuStruct.miConfigureMenuId; }
            set { moConfigureMenuStruct.miConfigureMenuId = value; }
        }

        public string ConfigureMenuName
        {

            get { return moConfigureMenuStruct.msConfigureMenuName; }
            set { moConfigureMenuStruct.msConfigureMenuName = value; }
        }

        public string ConfigureMenuContent
        {

            get { return moConfigureMenuStruct.msConfigureMenuContent; }
            set { moConfigureMenuStruct.msConfigureMenuContent = value; }
        }

        public int Priority
        {

            get { return moConfigureMenuStruct.miPriority; }
            set { moConfigureMenuStruct.miPriority = value; }
        }
        
		public string End_Date
        {
            get { return moConfigureMenuStruct.msUpdateEndDate; }
            set { moConfigureMenuStruct.msUpdateEndDate = value; }
        }

        public int SchoolId
        {

            get { return moConfigureMenuStruct.miSchoolId; }
            set { moConfigureMenuStruct.miSchoolId = value; }
        }

        public int ParentMenuId
        {

            get { return moConfigureMenuStruct.miParentMenuId; }
            set { moConfigureMenuStruct.miParentMenuId = value; }
        }

        public char IsExternal
        {

            get { return moConfigureMenuStruct.mcIsExternal; }
            set { moConfigureMenuStruct.mcIsExternal = value; }
        }

        public char IsDefault
        {

            get { return moConfigureMenuStruct.mcIsDefault; }
            set { moConfigureMenuStruct.mcIsDefault = value; }
        }

        public char IsActive
        {

            get { return moConfigureMenuStruct.mcIsActive; }
            set { moConfigureMenuStruct.mcIsActive = value; }
        }
        
		public char IsOnPopUp
        {

            get { return moConfigureMenuStruct.mcIsOnPopUp; }
            set { moConfigureMenuStruct.mcIsOnPopUp = value; }
        }

        public int InsertedById
        {

            get { return moConfigureMenuStruct.miInsertedById; }
            set { moConfigureMenuStruct.miInsertedById = value; }
        }

        public int UpdatedById
        {

            get { return moConfigureMenuStruct.miUpdatedById; }
            set { moConfigureMenuStruct.miUpdatedById = value; }
        }

        public string UserRoleIds
        {

            get { return moConfigureMenuStruct.msUserRoleIds; }
            set { moConfigureMenuStruct.msUserRoleIds = value; }
        }


        public int SubMenuCount
        {

            get { return moConfigureMenuStruct.miSubMenuCount; }
            set { moConfigureMenuStruct.miSubMenuCount = value; }
        }
        public bool ApplyAllSubMenu
        {

            get { return moConfigureMenuStruct.mApplyAllSubMenu; }
            set { moConfigureMenuStruct.mApplyAllSubMenu = value; }
        }

        public string AssoiciatedStandards
        {
            get { return moConfigureMenuStruct.mAssociatedStandards; }
            set { moConfigureMenuStruct.mAssociatedStandards = value; }
        }
        
        #endregion -- PROPERTIES --
        
        #region -- CONSTRUCTOR(s) --

        /// <summary>
        ///		Default constructor.
        /// </summary>
		public ConfigureMenuBL()
        {
        }
        
		/// <summary>
		///		Initializes the class and loads the specified menu.
		/// </summary>
		/// <param name="aiId"></param>
		public ConfigureMenuBL(int aiId)
        {
            moConfigureMenuDC = new ConfigureMenuDC(aiId);
            moConfigureMenuStruct = moConfigureMenuDC.ConfigureMenuStructDetails;
        }
        
		#endregion -- CONSTRUCTOR(s) --

        #region -- PUBLIC METHOD(s) --

        public DataTable GetUserRolesForSelectedMenuId(int aiMenuId)
        {
            return moConfigureMenuDC.GetUserRolesForSelectedMenuId(aiMenuId);
        }

        public int InsertConfigureMenu(string sEndDate, int aiAcademicYearId)
        {

            moConfigureMenuDC.ConfigureMenuStructDetails = moConfigureMenuStruct;
            return moConfigureMenuDC.InsertConfigureMenu(sEndDate, aiAcademicYearId);
        }

        public string GetInsetStatementForDefaultConfigureMenu()
        {
            moConfigureMenuDC.ConfigureMenuStructDetails = moConfigureMenuStruct;
            return moConfigureMenuDC.GetInsetStatementForDefaultConfigureMenu();
        }

        public void UpdateConfigureMenu(bool abIncludeParent, string asUpdateEndDate, int aiAcademicYearId)
        {
            moConfigureMenuDC.ConfigureMenuStructDetails = moConfigureMenuStruct;
            moConfigureMenuDC.UpdateConfigureMenu(abIncludeParent, asUpdateEndDate, aiAcademicYearId);
        }

        public void UpdateChildNodes(string asUpdateEndDate)
        {
            moConfigureMenuDC.ConfigureMenuStructDetails = moConfigureMenuStruct;
            moConfigureMenuDC.UpdateChildNodes(asUpdateEndDate);
        }

        public void DeleteConfigureMenu()
        {
            moConfigureMenuDC.ConfigureMenuStructDetails = moConfigureMenuStruct;
            moConfigureMenuDC.DeleteConfigureMenu();
        }

        public DataTable FetchMenuContentDetails(int aiMenuId)
        {
            //This function is used to fetch the Menu contents details.
            return moConfigureMenuDC.FetchMenuContentDetails(aiMenuId);
        }

        public static DataTable FetchMenuContentDetails()
        {
            //This function is used to fetch the Menu contents details.
            return ConfigureMenuDC.FetchMenuContentDetails();
        }

        public static bool IsChildMenu(int aiSchoolId, int aiMenuId)
        {
            int iParentMenuId = ConfigureMenuDC.IsChildMenu(aiSchoolId, aiMenuId);
            return iParentMenuId != 0;
        }

        public string IsMenuNameAlreadyExists()
        {
            string sMessage="False";
            moConfigureMenuDC.ConfigureMenuStructDetails = moConfigureMenuStruct;
            int iRowCount = moConfigureMenuDC.IsMenuNameAlreadyExists();
	        if (iRowCount > 0)
		        return "Menu Name already exists.";
	        
			iRowCount = moConfigureMenuDC.IsPriorityAlreadyExists();
	        return iRowCount > 0 ? "Menu priority already exists." : sMessage;
        }

        public int GetMenuIdByMenuName(string asMenuName, int aiParentMenuId, int aiSchoolId)
        {
            //This function is used to get the Menu id by menu name.
            return moConfigureMenuDC.GetMenuIdByMenuName(asMenuName, aiParentMenuId, aiSchoolId);
        }

        public int GetHighestPriority(int aiSchoolId)
        {
            return moConfigureMenuDC.GetHighestPriority(aiSchoolId) + 10;
        }

        public DataTable FetchSchoolNoticess()
        {
            return moConfigureMenuDC.FetchSchoolNoticess();
        }

		/// <summary>
		///		Gets the specified menu.
		/// </summary>
		/// <param name="aiMenuId"></param>
		/// <returns></returns>
		public Menu GetMenu(int aiMenuId)
		{
			return moConfigureMenuDC.GetMenu(aiMenuId);
		}
        
		#endregion -- PUBLIC METHOD(s) --
        /// <summary>
        /// this method is used to get associated standard and divisions.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="aiMenuId"></param>
        /// <returns></returns>

        public List<ConfigMenuAssociatedClasses> GetConfigMenuAssociatedClasses(int aiSchoolId, int aiAcademicYearId,int aiMenuId)
        {
            return moConfigureMenuDC.GetConfigMenuAssociatedClasses(aiSchoolId, aiAcademicYearId, aiMenuId);

        }
    }

    public class ConfigureCollectionMenuBL
    {

	    #region -- MEMBER(s) --

	    private ConfigureCollectionMenuDC moConfigureCollectionMenuDC;

	    #endregion -- MEMBER(s) --

	    #region -- CONSTRUCTOR(s) --

	    public ConfigureCollectionMenuBL()
	    {
		    moConfigureCollectionMenuDC = new ConfigureCollectionMenuDC();
	    }

	    #endregion -- CONSTRUCTOR(s) --

		#region -- PUBLIC METHOD(s) --

	    public DataTable FetchConfigureMenuCollection(int aiSchoolId)
	    {
		    //This function is used to fetch the configuration menu collection Items. 
		    return moConfigureCollectionMenuDC.FetchConfigureMenuCollection(aiSchoolId);
	    }

	    public DataTable FetchAllInternalMenus(int aiSchoolId, string asFilterString)
	    {
            return moConfigureCollectionMenuDC.FetchAllInternalMenus(aiSchoolId, asFilterString);
	    }

	    public DataTable FetchAllActiveInternalMenus(int aiSchoolId, int aiUserRoleId)
	    {
            return moConfigureCollectionMenuDC.FetchAllActiveInternalMenus(aiSchoolId, aiUserRoleId);
	    }

	    public DataTable FetchAllExternalMenus(int aiSchoolId)
	    {
		    return moConfigureCollectionMenuDC.FetchAllExternalMenus(aiSchoolId);
	    }

	    public DataTable GetAllParentMenus(int aiCurrentMenuId, int aiSchoolId)
	    {
		    return moConfigureCollectionMenuDC.GetAllParentMenus(aiCurrentMenuId, aiSchoolId);
	    }

        public DataTable GetAllSubMenus(int aiCurrentMenuId, int aiSchoolId)
	    {
            return moConfigureCollectionMenuDC.GetAllSubMenus(aiCurrentMenuId, aiSchoolId);
	    }        

	    #endregion -- PUBLIC METHOD(s) --

    }
}
