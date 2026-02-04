/* File Name :- VideoGallery.aspx.cs
 * Created By :- sachin
 * Created Date :- 7-March-2009
 * Class Description :- This class is used ti display video gallery.
*/

using System;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class VideoGallery : SchoolBase
{

	#region -- MEMBER(s) --

	protected string sSourcePath = string.Empty;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// Ths event is used to set video url.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{			
			string sVideoUrl = String.Empty;

			if (!QueryString["url"].IsNullOrEmpty())
				sVideoUrl = QueryString["url"];

			if (!QueryString["src"].IsNullOrEmpty())
			{
				int iSchoolId = miSchoolId == 0 ? QueryString["Schoolid"].ToInt() : miSchoolId;
				int iVideoId = QueryString["src"].ToInt();
				var oVideoGalleryBL = new VideoGalleryBL();
				string sUrl = oVideoGalleryBL.GetVideoUrl(iSchoolId, iVideoId);
				sSourcePath = sUrl;
			}
			else
				sSourcePath = Constants.S_YOUTUBE_URL + "v/" + sVideoUrl;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	#endregion -- EVENT HANDLER(s) --

}
