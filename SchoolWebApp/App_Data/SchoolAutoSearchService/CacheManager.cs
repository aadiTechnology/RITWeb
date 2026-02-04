using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;
using DataCommunicator;
using Utility;

namespace SchoolAutoSearchService
{
	public static class CacheManager
	{
		#region -- MEMBER(s) --

		private static MemoryCache _cache = MemoryCache.Default;

		#endregion -- MEMBER(s) --

        public static Dictionary<string, bool> Students
        {
            get
            {
                return _cache[AutoSearchConstants.S_STUDENT_KEY] as Dictionary<string, bool>;
            }
        }
        
		#region -- BUILD CACHE --

        public static Dictionary<string, bool> GetStudentList(string asFilter)
        {
            if (_cache[AutoSearchConstants.S_STUDENT_KEY] == null)
                 RefreshStudentCache(asFilter);

            return _cache[AutoSearchConstants.S_STUDENT_KEY] as Dictionary<string, bool>;
        }

        public static void RefreshStudentCache(string asFilter)
        {
            AutoSuggestDC oAutoSuggest = new AutoSuggestDC();
            InsertCacheItem(AutoSearchConstants.S_STUDENT_KEY, oAutoSuggest.GetDataForAutoSuggest(AutoSearchConstants.S_RESULT_TYPE, asFilter));
        }

		#endregion -- BUILD CACHE --

		#region -- PRIVATE METHOD(s) --

		/// <summary>
		/// Caches an object in memory for an indefinite time.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		private static void InsertCacheItem(string key, object value)
		{
			InsertCacheItem(key, value, ObjectCache.InfiniteAbsoluteExpiration);
		}

		/// <summary>
		/// Caches an object in memory for a specified time.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="cacheTimeout"></param>
		private static void InsertCacheItem(string key, object value, DateTimeOffset cacheTimeout)
		{
			_cache.Set(key, value, cacheTimeout);			
		}

		#endregion  -- PRIVATE METHOD(s) --

    }
}