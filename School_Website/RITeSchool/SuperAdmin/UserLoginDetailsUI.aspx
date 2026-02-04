<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserLoginDetailsUI.aspx.cs" Inherits="UserLoginDetailsUI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="UpanelGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table>
                <tr>
                    <td height="20px" colspan="2">
                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                            <asp:Label ID="lblErrorMsg" Style="text-align: center" runat="server" ForeColor="Red"
                                Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false" ></asp:Label></asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight paddingL" style="width: 150px;" colspan="1">
                        User Role :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="MidCombo" AutoPostBack="False"
                            Width="150px" />
                        <span class="ClsMdtStar">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="width: 150px">
                        <asp:Button ID="btnExport" runat="server" CausesValidation="False" Text="Export" OnClientClick="ClearMessages();"
                            CssClass="ClsBtnMid" OnClick="btnExport_Click" />
                    </td>
                    <%--<td align="left">
                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" Text="Back" CssClass="ClsBtnMid" PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx"  />
                    </td>--%>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">

        var _clientlblErrorMsg = '<%= this.lblErrorMsg.ClientID %>';

        // This function is used to clear Update Message.
        function ClearMessages() {
            var lblUpdateMsg = $get(_clientlblErrorMsg);
            if (lblUpdateMsg)
                lblUpdateMsg.innerHTML = '';
        }
    </script>
</asp:Content>
