// File Name - VideoGalleryUC.ascx.cs
// Creator - Yogesh
// Created Date - 9-Oct-2013
// Description - This class is used to Fill Video Gallery.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Text.RegularExpressions;

public partial class VideoGalleryUC : System.Web.UI.UserControl
{

    #region Member(s)
    VideoGalleryBL oVideoGalleryBL = new VideoGalleryBL();
    List<VideoDetails> lstAllVedioUrls;
    List<string> lstVideoCodes = new List<string>();

    public string YouTubeUrl { get; set; }

    public bool ShowHeading { get; set; }

    

    public string YouTubeVideoId
    {
        get
        {
            var youtubeMatch =
                new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)")
                .Match(this.YouTubeUrl);
            return youtubeMatch.Success ? youtubeMatch.Groups[1].Value : string.Empty;
        }
    }

    #endregion

    #region EVENT(S)

    /// <summary>
    /// This event is will fired on page load.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
                FillVideoGallary();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will call wile user will click on view more videos link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LnkBtnLoadMoreVideos_Click(object sender, EventArgs e)
    {
        try
        {
            FillVideoGallary();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    #endregion

    #region PRIVATE METHOD(S)

    /// <summary>
    /// This method is used to load video Gallery and contents.
    /// </summary>
    private void FillVideoGallary()
    {
        string sOauthClientKey = "AIzaSyAElG-4SbDEAt86LwP0y9cEaLEqLVF43Cg";
        var sThumbnailUrl = "http://img.youtube.com/vi/";
        var sYoutubewatch = "http://www.youtube.com/watch?v=";
        var sOldPatternUrl = "http://www.youtube.com/v/";
        lstAllVedioUrls = oVideoGalleryBL.GetAllVideoUrls();
        string sVideoDetails = string.Empty;
       
        for (int iIndex = 0; iIndex < lstAllVedioUrls.Count; iIndex++)
        {
            YouTubeUrl = lstAllVedioUrls[iIndex].sVideoUrl.ToString();
            lstVideoCodes.Add(YouTubeVideoId);
        }

        int iStartIndex = 0;
        int iEndIndex = 5;

        if (hidVideoListCount.Value != Constants.S_ZERO)
        {
            iStartIndex = hidVideoListCount.Value.ToInt();
            iEndIndex = iStartIndex + 5;
        }

        hidVideoListCount.Value = iEndIndex.ToString();

        if (iEndIndex >= lstVideoCodes.Count)
        {
            iEndIndex = lstVideoCodes.Count;
            divMoreVideos.Visible = false;
        }

        for (int i = 0; i < iEndIndex; i++)
        {
            string url = string.Format("https://www.googleapis.com/youtube/v3/videos?id={0}&key={1}&part=snippet,contentDetails", lstVideoCodes[i], sOauthClientKey);
            HttpWebRequest httpRequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
            httpRequest.Method = "GET";
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            // Get the stream associated with the response.
            Stream receiveStream = httpResponse.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            string reponseText = readStream.ReadToEnd();
            httpResponse.Close();
            readStream.Close();

            YouTubeVideoDetails videoDetails = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<YouTubeVideoDetails>(reponseText);


            if (YouTubeUrl.Contains(sYoutubewatch))
                sYoutubewatch = sYoutubewatch.ToString();
            else if (YouTubeUrl.Contains(sOldPatternUrl))
                sYoutubewatch = sOldPatternUrl.ToString();

            sVideoDetails += "<a class='youtube' href='" + sYoutubewatch + lstVideoCodes[i] + "'>" +
                                "<li class='item' style='width:100%'>" +
                                    "<img src='" + sThumbnailUrl + lstVideoCodes[i] + "/default.jpg'/>" +
                                    "<div id='VideoData'>" +
                                        "<span class='Heading'>Title:&nbsp;</span>" + videoDetails.items[0].snippet.title + "<br />" +
                                         "<span class='Heading'>Description:&nbsp;</span>" + videoDetails.items[0].snippet.description +
                                    "</div>" +
                                "</li>" +
                             "</a>";
        }


        if ((sVideoDetails).IsNullOrEmpty() != true)
        {
            divVideoGallery.InnerHtml = sVideoDetails;
            divVideoGalleryNote.Visible = true;
        }
        else
        {
            divVideoGallery.InnerHtml = "<div align = 'center' style='text-align:center; color:Red; font-weight:bold; font-size:medium;' >No Video Uploaded.</div>";
            divVideoGalleryNote.Visible = false;
        }
        

        if (ShowHeading == false)
            divHeadingVideoGallery.Visible = false;
    }
#endregion

}


public class PageInfo
{
    public int totalResults { get; set; }
    public int resultsPerPage { get; set; }
}

public class Default
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Medium
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class High
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Standard
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Maxres
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Thumbnails
{
    public Default @default { get; set; }
    public Medium medium { get; set; }
    public High high { get; set; }
    public Standard standard { get; set; }
    public Maxres maxres { get; set; }
}

public class Localized
{
    public string title { get; set; }
    public string description { get; set; }
}

public class Snippet
{
    public string publishedAt { get; set; }
    public string channelId { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public Thumbnails thumbnails { get; set; }
    public string channelTitle { get; set; }
    public List<string> tags { get; set; }
    public string categoryId { get; set; }
    public string liveBroadcastContent { get; set; }
    public Localized localized { get; set; }
}

public class ContentDetails
{
    public string duration { get; set; }
    public string dimension { get; set; }
    public string definition { get; set; }
    public string caption { get; set; }
    public bool licensedContent { get; set; }
}

public class Item
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string id { get; set; }
    public Snippet snippet { get; set; }
    public ContentDetails contentDetails { get; set; }
}

public class YouTubeVideoDetails
{
    public string kind { get; set; }
    public string etag { get; set; }
    public PageInfo pageInfo { get; set; }
    public List<Item> items { get; set; }
}