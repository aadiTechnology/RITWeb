using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using DataCommunicator;
using SchoolEntities;
using System.Configuration;

namespace BusinessLogic
{
	public static class CacheManagerBL
	{
        #region Constant(s)

        public const string STUDENT = "Student";
        public const string STAFF = "Staff";
        public const string USER = "User";
        public const string CLASSES = "Classes"; 

        #endregion

		#region -- MEMBER(s) --

		private static MemoryCache _cache = MemoryCache.Default;

		#endregion -- MEMBER(s) --

		#region -- BUILD CACHE --

        #region Student

        public static List<Student> GetStudentList(int aiSchoolId, int aiAcademicYearId)
        {
            if (_cache[STUDENT + aiAcademicYearId] == null)
                RebuildStudentCache(aiSchoolId, aiAcademicYearId, string.Empty);

            return _cache[STUDENT + aiAcademicYearId] as List<Student>;
        }

        private static void RebuildStudentCache(int aiSchoolId, int aiAcademicYearId, string asYearwiseStudentId)
        {
            InsertCacheItem(STUDENT + aiAcademicYearId, AutoSuggestDC.GetStudentDataForAutoSearch(aiSchoolId, aiAcademicYearId, asYearwiseStudentId));
        } 

        #endregion

        #region Staff

        public static List<Staff> GetStaffList(int aiSchoolId, int aiAcademicYearId)
        {
            if (_cache[STAFF + aiAcademicYearId] == null)
                RebuildStaffCache(aiSchoolId, aiAcademicYearId, string.Empty);

            return _cache[STAFF + aiAcademicYearId] as List<Staff>;
        }

        private static void RebuildStaffCache(int aiSchoolId, int aiAcademicYearId, string asUserIds)
        {
            InsertCacheItem(STAFF + aiAcademicYearId, AutoSuggestDC.GetStaffDataForAutoSearch(aiSchoolId, aiAcademicYearId, asUserIds));
        } 

        #endregion

		#endregion -- BUILD CACHE --

		#region -- PRIVATE METHOD(s) --

		/// <summary>
		/// Caches an object in memory for a specified time.
		/// </summary>
		/// <param name="asKey"></param>
		/// <param name="aoValue"></param>
		/// <param name="cacheTimeout"></param>
		private static void InsertCacheItem(string asKey, object aoValue)
		{
            CacheItemPolicy oPolicy = new CacheItemPolicy();

            string sConfigurationManager = ConfigurationManager.AppSettings["AutoSearchCachePath"];
            if (sConfigurationManager.Trim() == string.Empty)
                sConfigurationManager = "G:\\AutoSearchCache.txt";
            List<string> filePaths = new List<string>();
            filePaths.Add(sConfigurationManager);

            oPolicy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

            _cache.Set(asKey, aoValue, oPolicy);
		}

        public static List<AutoSearchUser> GetUserListForMessageCenter(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, int aiUserId)
        {
            if (_cache[USER + aiAcademicYearId] == null)
                RebuildUserCache(aiSchoolId, aiAcademicYearId, aiUserRoleId, aiUserId);

            return _cache[USER + aiAcademicYearId] as List<AutoSearchUser>;
        }

        public static List<TeacherClassAsso> GetClassesForMessageCenter(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, int aiUserId)
        {
            if (_cache[CLASSES + aiAcademicYearId] == null)
                RebuildUserCache(aiSchoolId, aiAcademicYearId, aiUserRoleId, aiUserId);

            return _cache[CLASSES + aiAcademicYearId] as List<TeacherClassAsso>;
        }

        public static void RebuildUserCache(int aiSchoolId, int aiAcademicYearId, int aiUserRoleId, int aiUserId)
        {
            List<TeacherClassAsso> lstTeacherClassAsso;
            List<AutoSearchUser> lstAutoSearchUser = AutoSuggestDC.GetUserDataForMessageCenter(aiSchoolId, aiAcademicYearId, aiUserRoleId, aiUserId, out lstTeacherClassAsso);
            InsertCacheItem(USER + aiAcademicYearId, lstAutoSearchUser);
            InsertCacheItem(CLASSES + aiAcademicYearId, lstTeacherClassAsso);
        }

        #endregion  -- PRIVATE METHOD(s) --
    }
}