<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PPSMaster.master"
    CodeFile="ShowImageGallery.aspx.cs" Inherits="ShowImageGallery" Title="Photo/Video Gallery" %>

<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="MainBodyDiv">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table width="100%" height="100%">
            <tr>
                <td align="center" class="ClsPhotoGal">
                    <cc1:CollapsablePanel ID="colpnlImageGallery" runat="server" TitleText="Photo Gallery"
                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="RITeSchool/images/node_open.gif"
                        CollapseImageUrl="RITeSchool/images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                        Collapsed="True" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table align="center" border="0" cellpadding="0" cellspacing="1" width="100%">
                                    <tr>
                                        <td style="background-color: white" id="MainDataTable" align="center">
                                            <!-- Data Insert Here -->
                                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%; height: 100%">
                                                <tr>
                                                    <td align="center" valign="top" class="ClsPhotoGal">
                                                        <asp:GridView ID="grdImageGallery" runat="server" CssClass="GridBorder" ForeColor="#333333"
                                                            EmptyDataText="No Photo Gallery is uploaded yet." OnSorting="grdImageGallery_Sorting"
                                                            OnRowCreated="grdImageGallery_RowCreated" GridLines="None" AllowPaging="False"
                                                            CellSpacing="1" CellPadding="0" PageSize="20" AllowSorting="False" OnRowDataBound="grdImageGallery_RowDataBound"
                                                            AutoGenerateColumns="False" EnableViewState="False" Width="750px">
                                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                            <Columns>
                                                                <asp:BoundField DataField="Gallery_Name" HeaderText="Gallery Name" SortExpression="Gallery_Name">
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="250px" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Count" HeaderText="Total Images in gallery" SortExpression="Count">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/RITeSchool/images/GridIcon_Slideshow.gif"
                                                                    HeaderText="Slide Show" Text="View Slide Show" CommandName="SHOW_GALLERY">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="100px" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/RITeSchool/images/download_transparent.png"
                                                                    HeaderText="Download" Text="Download" CommandName="DOWNLOAD">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="100px" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="UpdatedOn" HeaderText="Last Updated Date">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                                </asp:BoundField>                                                                
                                                            </Columns>
                                                            <PagerStyle CssClass="ClsNwGridPaging" HorizontalAlign="Right" />
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                            <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center"/>                                                             
                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <!-- Data Insert End Here -->
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="grdImageGallery" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </cc1:CollapsablePanel>
                </td>
            </tr>
            <tr>
                <td align="center" class="ClsPhotoGal">
                    <cc1:CollapsablePanel ID="colpnlVideoGallery" runat="server" TitleText="Video Gallery"
                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="RITeSchool/images/node_open.gif"
                        CollapseImageUrl="RITeSchool/images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                        Collapsed="True" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                        <asp:UpdatePanel ID="UPanelVideoGallery" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;
                                    background-color: white">
                                    <tr id="trTotalRec" runat="server">
                                        <td align="center">
                                            <asp:Label ID="lblStartIndex" runat="server" />
                                            <asp:Label ID="lblTo" runat="server" Text=" To " />
                                            <asp:Label ID="lblEndIndex" runat="server" />
                                            <asp:Label ID="lblOutOf" runat="server" Text=" Out Of " />
                                            <asp:Label ID="lblTotal" runat="server" />
                                            <asp:Label ID="lblRecords" runat="server" Text="Records " />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%; height: 100%;
                                                background-color: white">
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="grdVideoGallery" runat="server" AutoGenerateColumns="False" CellPadding="0"
                                                            CellSpacing="1" CssClass="GridBorder" DataKeyNames="Video_Id,Video_Url" EmptyDataText="No video available."
                                                            ForeColor="#333333" GridLines="None" OnRowCommand="grdVideoGallery_RowCommand"
                                                            PageSize="20" TabIndex="5" OnRowCreated="grdVideoGallery_RowCreated" OnPageIndexChanging="grdVideoGallery_PageIndexChanging"
                                                            OnRowDataBound="grdVideoGallery_RowDataBound" OnSorting="grdVideoGallery_Sorting"
                                                            AllowPaging="True" AllowSorting="True" Width="750px">
                                                            <Columns>
                                                                <asp:BoundField DataField="Video_Name" HeaderText="Video Name">
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="480px"/>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                        Width="480px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="View">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnViewVideo" runat="server" CausesValidation="false" CommandName="VIEW_VIDEO_NAME"
                                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/GridIcon_Slideshow.gif" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Update_Date" HeaderText="Last Updated Date">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px"/>
                                                                </asp:BoundField>  
                                                            </Columns>
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center"/>
                                                            <PagerTemplate>
                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                            <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                            <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                                OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </PagerTemplate>
                                                        </asp:GridView>
                                                        <asp:HiddenField ID="hidVSortDirection" runat="server" />
                                                        <asp:HiddenField ID="hidVSortExpression" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hidVedioId" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hidVideoGalaryPath" runat="server" />
                                                        <asp:HiddenField ID="hidSchoolId" runat="server" />
                                                        <asp:ObjectDataSource ID="ObjectDSVideoGallery" runat="server" EnablePaging="True"
                                                            OnSelected="ObjectDSVideoGallery_Selected" SelectCountMethod="GetCountFromVedioList"
                                                            SelectMethod="VideoGalleryDetails" TypeName="BusinessLogic.VideoGalleryCollectionBL">
                                                            <SelectParameters>
                                                                <asp:ControlParameter Name="aiSchoolId" ControlID="hidSchoolId" Type="Int32" PropertyName="Value" />
                                                                <asp:ControlParameter Name="sortExp" ControlID="hidVSortExpression" Type="String"
                                                                    PropertyName="Value" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </cc1:CollapsablePanel>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        //This function is used to display video gallery.
        function ShowVideoGallery(_VideoId) {
            window.open('RITeSchool/Gallery/VideoGallery.aspx?' + _VideoId + '', '_blank', 'scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=20,width=445,height=364,resizable=no');
            return false;
        }
    </script>

</asp:Content>
