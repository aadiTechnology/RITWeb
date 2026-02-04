<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VideoGallery.ascx.cs"
    Inherits="VideoGalleryUC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>jQuery YouTube Popup Player Plugin</title>
  
   <link type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/themes/redmond/jquery-ui.css" rel="stylesheet" />
   <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>
    <script src="../RITeSchool/Scripts/jquery.youtubepopup.min.js" type="text/javascript"></script>
   <script type="text/javascript">
       $(function () {
           $("a.youtube").YouTubePopup({ autoplay: 0 });
       });
    </script>
</head>
<body>
<div runat="server" id="divVideoGallaryMain" class="video_gallary_main">
    <div runat="server" id="divHeadingVideoGallery" class="registration_nursary_box">
        <div  class="heading_video">
            Video Gallery</div>
    </div>
    <div class="video_gallary_section" style="color: Black;">
        <div class="demo-wrapper">
            <div id="divVideoGallery" runat="server">
            </div>
            <div id="gallery-container" style="position: inherit;">
                <ul class="gallery clearfix">
                </ul>
                <ul class="items--small gallery clearfix" style="padding-left: .2px;">
                </ul>
               <div runat="server" id="divMoreVideos" class="moreVideos">
                   <asp:LinkButton ID="LnkBtnLoadMoreVideos" runat ="server" 
                       onclick="LnkBtnLoadMoreVideos_Click">Load more videos...</asp:LinkButton></div>
            </div>
            <div id="divVideoGalleryNote" runat="server" style="text-align: justify">
                <p class="credit" style="padding-left: 10px;">
                    Note: If you are not able to view videos in Internet Explorer, follow the path:
                    Tools -> Internet Options -> Security -> Internet -> Custom Level and change "Access
                    data sources across domains" option (under Miscellaneous) to Enable.
                </p>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hidVideoUrls" runat="server" />
    <asp:HiddenField ID="hidVideoNames" runat="server" />
    <asp:HiddenField ID="hidVideoListCount" runat="server" Value="0" />
</div>
</body>
</html>
