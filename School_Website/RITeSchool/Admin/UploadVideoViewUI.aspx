<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadVideoViewUI.aspx.cs" Inherits="UploadVideoViewUI" %>

<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content2" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/themes/redmond/jquery-ui.css"
        rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>
    <script src="../Scripts/jquery.youtubepopup.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            OpenPopup();
        });

        function OpenPopup() {
            $("a.youtube").YouTubePopup({ autoplay: 0, modal: false });
        }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
        <tr>
            <td align="right">
                <span class="ClsMdtStar">* Mandatory Fields</span>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSumHeader" runat="server" ValidationGroup="GalleryTitle" />
                        <asp:ValidationSummary ID="ValSum" runat="server" ValidationGroup="GalleryDetails" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                            Display="None" ErrorMessage="Name should not be blank." ValidationGroup="GalleryTitle"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtURL"
                            Display="None" ErrorMessage="URL should not be blank." ValidationGroup="GalleryDetails"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                            ClientValidationFunction="ValidateComment" ValidationGroup="GalleryDetails"></asp:CustomValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdVideos" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnVideoUpdate" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="50%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td align="center" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblErrorMessage" runat="server" EnableViewState="false" CssClass="ClsLabel"
                                        Style="width: 100%; margin: 8px 0;" ForeColor="Red" Visible="false" />
                                    <asp:Label ID="lblUpateMessage" runat="server" EnableViewState="false" CssClass="ClsLabel"
                                        Style="width: 100%; margin: 8px 0;" ForeColor="Blue" Font-Bold="true" Visible="false" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdVideos" EventName="RowCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="btnVideoUpdate" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr align="left">
                        <td align="center" colspan="2">
                            <table width="100%">
                                <tr>
                                    <td width="100px" class="ClsBorderlight" align="left">
                                        <asp:Label ID="lblGalleryNameHeader" runat="server" CssClass="ClsLblLgnd clsLabel" EnableViewState="False" style="padding-left:5px"
                                            Font-Bold="True" Text="Gallery Name : "></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightPhotoBGB">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblGalleryName" runat="server" EnableViewState="true" CssClass="clsLabel"
                                                    Font-Bold="True"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:5px;"></td>
                                </tr>
                                <tr id="trSubject" runat="server" visible="false">
                                    <td width="100px" class="ClsBorderlight" align="left">
                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLblLgnd clsLabel" EnableViewState="False" style="padding-left:5px"
                                            Font-Bold="True" Text="Subject Name : "></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightPhotoBGB">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblSubjectName" runat="server" EnableViewState="true" CssClass="clsLabel"
                                                    Font-Bold="True"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                  <tr>
                                    <td style="height:5px;"></td>
                                </tr>
                                  <tr id="UrlLink" runat="server" visible="false">
                                    <td width="100px" class="ClsBorderlight" align="left">
                                        <asp:Label ID="Label2" runat="server" CssClass="ClsLblLgnd clsLabel" EnableViewState="False" style="padding-left:5px"
                                            Font-Bold="True" Text="Url Source : "></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightPhotoBGB">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblUrlSource" runat="server" EnableViewState="true" CssClass="clsLabel"
                                                    Font-Bold="True"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                               
                            </table>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td colspan="2">
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="center" colspan="2">
                            <table width="100%">
                                <tr id="trUserName" runat="server" visible="false">
                                    <td width="100px" class="ClsBorderlight" align="left">
                                        <span class="ClsLabel">Name :</span>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="LrgTxtBox" Width="98%"
                                                    TextMode="SingleLine"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="trbtnUpload" runat="server" visible="false">
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="ClsBtn" OnClick="btnUpdate_Click"
                                            ValidationGroup="GalleryTitle" />
                                    </td>
                                </tr>
                                <tr id="trLine" runat="server" visible="false">
                                    <td colspan="2">
                                        <hr style="border: 1px solid silver" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100px" class="ClsBorderlight" align="left">
                                                            <span class="ClsLabel">URL :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtURL" runat="server" MaxLength="100" CssClass="LrgTxtBox" Width="98%"
                                                                TextMode="SingleLine"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100px" class="ClsBorderlight" align="left">
                                                            <span class="ClsLabel">Title :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtComment" runat="server" MaxLength="200" CssClass="LrgTxtBox"
                                                                Width="98%"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnVideoUpdate" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="grdVideos" EventName="RowCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnVideoUpdate" runat="server" BorderStyle="Solid" CssClass="ClsBtnMid"
                                                    ValidationGroup="GalleryDetails" Text="Update" UseSubmitBehavior="False" OnClick="btnVideoUpdate_Click" />
                                                <asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" CssClass="ClsBtnMid"
                                                    Text="Cancel" UseSubmitBehavior="False" CausesValidation="false" OnClick="btnCancel_Click" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnVideoUpdate" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="grdVideos" EventName="RowCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr align="center">
            <td style="background-color: white;" align="center" valign="top">
                <table width="70%" cellpadding="1" cellspacing="1" align="center">
                    <tr align="center">
                        <td id="s" align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="grdVideos" runat="server" AutoGenerateColumns="False" CellPadding="0"
                                        CellSpacing="1" CssClass="GridBorder" DataKeyNames="VideoId,URL,VideoDetailsId,VideoName"
                                        ForeColor="#333333" GridLines="None" OnRowCommand="grdVideos_RowCommand" OnRowDataBound="grdVideos_RowDataBound"
                                        PageSize="1000" Width="100%" EmptyDataText="No video available.">
                                        <PagerStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                            NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                        <Columns>
                                            <asp:BoundField DataField="URL" HeaderText="Video Path">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Description" HeaderText="Comments">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Wrap="true" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                    Wrap="true" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="View">
                                                <ItemTemplate>
                                                    <a id="aView" class="youtube" runat="server" onclick="return false;">View</a>                                                      
                                                    <asp:LinkButton ID="a1" runat="server"   Text="Play Video"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:ButtonField ButtonType="Image" CommandName="EDIT_ROW" HeaderText="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                Text="Edit">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                            </asp:ButtonField>
                                            <asp:ButtonField ButtonType="Image" CommandName="DELETE_ROW" HeaderText="Delete"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" Text="Delete">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                            </asp:ButtonField>
                                        </Columns>
                                        <RowStyle CssClass="ClsGridRow" />
                                        <HeaderStyle CssClass="ClsGridHeader" />
                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdVideos" EventName="RowCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="btnVideoUpdate" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" Text="Back" CssClass="ClsBtnSml" BorderStyle="Solid" runat="server"
                    BorderWidth="1px" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnBack_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidVideoGalleryId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidVideoGallaryDetailsId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidSubjectId" runat="server" Value="0" />
                         <asp:HiddenField ID="hidUrlSourceId" runat="server" Value="0" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdVideos" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnVideoUpdate" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        _clienttxtComment = "<%=txtComment.ClientID %>"
        function ConfirmPhotoDelete() {
            var bResult = true;
            if (!window.confirm("Are you sure you want to delete this video?"))
                bResult = false;
            return bResult;
        }
        function refreshParent() {
            window.opener.location = window.opener.location;
            window.close();
            window.opener.focus();
        }

        function ValidateComment(osrc, args) {
            var comment = $('#' + _clienttxtComment).val()
            comment = comment.trim()
            if (comment == "") {
                osrc.errormessage = "Title should not be blank.";
                args.IsValid = false
                return true
            }
            else if (comment.length > 200) {
                osrc.errormessage = "Title length should not be greater than 200.";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ClearMessage() {
            $('#' + "<%=lblErrorMessage.ClientID %>").html("")
            $('#' + "<%=lblUpateMessage.ClientID %>").html("")
        }


        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
            OpenPopup()
        }

        function beginRequestHandler(sender, args) {
        }       
    </script>
</asp:Content>
