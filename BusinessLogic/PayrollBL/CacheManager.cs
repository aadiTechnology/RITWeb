using System.Web.Caching;
using System.Web;
namespace BusinessLogic
{
    public class CacheManager
    {
        #region DataMember
        
        private static Cache _cache = HttpContext.Current.Cache; 

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to insert static data into cache.
        /// </summary>
        /// <param name="asKey"></param>
        /// <param name="aoValue"></param>
        public static void InsertStaticData(string asKey, object aoValue)
        {
            _cache.Insert(asKey, aoValue,
                      new CacheDependency(HttpContext.Current.Server.MapPath(@"~\ITCache.txt")),
                      Cache.NoAbsoluteExpiration,
                      Cache.NoSlidingExpiration,
                      CacheItemPriority.NotRemovable,
                      null);
        }

        /// <summary>
        ///  This method is used to insert static data into cache.
        /// </summary>
        /// <param name="asKey"></param>
        /// <param name="aoValue"></param>
        public static void Insert(string asKey, object aoValue)
        {   
            _cache.Insert(asKey, aoValue,
                      new CacheDependency(HttpContext.Current.Server.MapPath(@"~\ITCache.txt")),
                      Cache.NoAbsoluteExpiration,
                      Cache.NoSlidingExpiration,
                      CacheItemPriority.NotRemovable,
                      null);
        }

        /// <summary>
        /// This method is used to return object from cache according to the given key.
        /// </summary>
        /// <param name="asKey"></param>
        /// <returns></returns>
        public static object Get(string asKey)
        {
            return _cache[asKey];
        }

        /// <summary>
        /// This method is used to return whether there exist value for given key in cache.
        /// </summary>
        /// <param name="asKey"></param>
        /// <returns></returns>
        public static bool HasValue(string asKey)
        {
            return _cache[asKey] != null;
        } 

        #endregion
    }
}
