<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VideoGallery.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="VideoGallery" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <title>Video Gallery</title>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
    <div>
        <object width="425" height="344">
            <param name="movie" value="<%=sSourcePath%>" />
            <param name="allowFullScreen" value="true" />
            <param name="allowscriptaccess" value="always" />
            <embed src="<%=sSourcePath%>" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="425" height="344"></embed>
        </object>
    </div>
</asp:Content>