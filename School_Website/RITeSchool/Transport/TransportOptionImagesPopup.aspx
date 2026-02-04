<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="TransportOptionImagesPopup.aspx.cs" Inherits="TransportOptionImagesPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
        <tr align="center">
            <td align="center">
                <table width="100%" align="center">
                    <tr>
                        <td class="ClsGrayMainTitle" style="height: 20px;" align="left">
                            <asp:Label ID="lblAddAcademicYear" runat="server" CssClass="MainTitleHead" Font-Bold="True"
                                Text="View Images" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td id="tdMessage" runat="server" align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwImages" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table width="80%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td align="left" class="ClsBorderlight" style="width: 150px;">
                            <asp:Label ID="lblType" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" EnableViewState="False"
                                Font-Bold="True" Text="Document Type : "></asp:Label>
                        </td>
                        <td class="ClsHilightPhotoBGB" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblTypeName" runat="server" EnableViewState="true" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight" style="width: 150px;">
                            <asp:Label ID="lblVehicle" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                EnableViewState="False" Font-Bold="True" Text="Vehicle Number : "></asp:Label>
                        </td>
                        <td class="ClsHilightPhotoBGB" style="text-align: left; padding-left: 10px;">
                            <asp:Label ID="lblVehicleName" runat="server" EnableViewState="true" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 20px">
            </td>
        </tr>
        <tr align="center" style="text-align: center;">
            <td align="center" style="text-align: center;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center" style="text-align: center; margin: 0px auto;" width="70%" cellpadding="1"
                            cellspacing="1">
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwImages" runat="server" DataKeyNames="TypeId,VehicleId,DetailId,Images"
                                        OnItemCommand="lstvwImages_ItemCommand" OnItemDataBound="lstvwImages_ItemDataBound">
                                        <LayoutTemplate>
                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="center" class="paddingL" style="font-size: 10pt;">
                                                        <span>Image</span>
                                                    </th>
                                                    <th width="60px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                        <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                <td align="center">
                                                    <asp:ImageButton ID="imgTransportPhoto" runat="server" Height="120px" Width="160px"
                                                        ImageUrl='<%#Eval("Images") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td align="center">
                                                    <asp:ImageButton ID="imgTransportPhoto" runat="server" Height="120px" Width="160px"
                                                        ImageUrl='<%#Eval("Images") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hidImageCount" runat="server" Value="0" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwImages" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height:10px;">                
            </td>
        </tr>
        <tr align="center" style="text-align:center;">
            <td align="center" style="text-align: center; margin: 0px auto;">
                <asp:Button ID="btnClose" CssClass="ClsBtn" runat="server" Text="Close" CausesValidation="False" />
            </td>
        </tr>
         <asp:HiddenField ID="hidTypeId" runat="server" Value="0" />
         <asp:HiddenField ID="hidDetailsId" runat="server" Value="0" />
         <asp:HiddenField ID="hidVehicleId" runat="server" Value="0" />         
    </table>
    <script language="javascript" type="text/javascript">

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this image?');
        }

        function OpenWindow(sfilepath) {
            window.open(sfilepath);
            return false;
        }

        function ClosePopup() {
            var cnt = $('[id$=hidImageCount]').val();
            if (cnt > 0) {
                window.close();
                return false;
            }
            else {
                window.opener.location.href = window.opener.location.href;
                window.close(); 
                window.opener.focus();
            }           
        }

    </script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
