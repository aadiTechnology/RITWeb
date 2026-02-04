using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Caching;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using Utility;

public partial class ImageProcessor : SchoolBase
{
    #region Constant(s)
    
    private const string S_STUDENTS_PHOTO = "StudentsPhoto"; 

    #endregion

    #region Data Member(s)
    
    private static object myLock = new object();

    private static List<PhotoMaster> lstPhotoMaster = HttpContext.Current.Cache[S_STUDENTS_PHOTO] as List<PhotoMaster>;

    #endregion

    #region Event(s)
    
    /// <summary>
    /// This event is used to read binary details and display it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {   
        try
        {
            int iUserId = Request.QueryString["Value"].ToInt();
            int iPhotoTypeId = Constants.I_ZERO;
            if (Request.QueryString["PhotoTypeId"] != null)
                iPhotoTypeId = Request.QueryString["PhotoTypeId"].ToInt();

            if (Request.QueryString["IsBirthdayScreen"] != null && Convert.ToBoolean(Request.QueryString["IsBirthdayScreen"]))
            {
                if (lstPhotoMaster == null || lstPhotoMaster.Count == 1)
                {
                    // Get lock before retrieving from database to avoid multiple DB calls and multiple cache inserts by parallel user requests.
                    lock (myLock)
                    {
                        // Try to get from cache if filled by some other user request while waiting on lock.
                        lstPhotoMaster = HttpContext.Current.Cache[S_STUDENTS_PHOTO] as List<PhotoMaster>;

                        // Still if not found in cache then get from database and insert into cache.
                        if (lstPhotoMaster == null)
                        {
                            lstPhotoMaster = SchoolBL.GetStudentsBinaryPhoto(miSchoolId, miAcademicYearId);
                            // expire time  = 12:02 AM of each day.
                            DateTime dtCacheExpireDate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " 00:02:00");
                            HttpContext.Current.Cache.Insert(S_STUDENTS_PHOTO, lstPhotoMaster,
                                                              null,
                                                              dtCacheExpireDate,
                                                              Cache.NoSlidingExpiration,
                                                              CacheItemPriority.NotRemovable,
                                                              null);
                        }
                    }
                }
                
                SetImageData(lstPhotoMaster, iUserId);
            }
            else
            {
                List<PhotoMaster> lstPhotos;
                if (Request.QueryString["IsFromGuestScreen"] != null)
                {
                    lstPhotos = SchoolBL.GetGuestsBinaryPhoto(iUserId, miSchoolId);
                }
                else
                {
                    lstPhotos = SchoolBL.GetUserBinaryPhoto(iUserId, miSchoolId, miAcademicYearId, iPhotoTypeId);
                }             
              SetImageData(lstPhotos, iUserId);              
            }
        }
        catch (ThreadAbortException) 
        {
        }
        catch (Exception ex)
        {   
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to set image data.
    /// </summary>
    /// <param name="alstPhotos"></param>
    /// <param name="aiUserId"></param>
    private void SetImageData(List<PhotoMaster> alstPhotos, int aiUserId)
    {
        if (alstPhotos.Where(usr => usr.UserId == aiUserId).Any())
        {
            byte[] imageByteArray = alstPhotos.Where(usr => usr.UserId == aiUserId).Select(bytes => bytes.TotalBytes).FirstOrDefault();
            if (imageByteArray != null && imageByteArray.Length > 0)
            {
                Response.Clear();
                Response.ContentType = "image/jpg";
                Response.BinaryWrite(imageByteArray);
                Response.End();
            }
        }
    } 

    #endregion
}