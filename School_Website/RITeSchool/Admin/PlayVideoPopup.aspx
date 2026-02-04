<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" AutoEventWireup="true" CodeFile="PlayVideoPopup.aspx.cs" Inherits="PlayVideoPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceHolder" Runat="Server">
<link href="//amp.azure.net/libs/amp/2.3.6/skins/amp-default/azuremediaplayer.min.css" rel="stylesheet"/>
<script src="//amp.azure.net/libs/amp/2.3.6/azuremediaplayer.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlaceHolder" Runat="Server">
<div>
 <video id="vidsrcMedia" runat="server" class="azuremediaplayer amp-default-skin" autoplay controls width="640" height="500" poster="poster.jpg" data-setup='{"nativeControlsForTouch": false}'>
                <source id="srcMedia" runat="server" type="application/vnd.ms-sstr+xml" />
                <p class="amp-no-js">
                    To view this video please enable JavaScript, and consider upgrading to a web browser that supports HTML5 video
   
                </p>
            </video>
</div>
        <asp:HiddenField ID="hidStreamingUrl" runat="server" />
         <asp:HiddenField ID="HiddenField1" runat="server" />

</asp:Content>

