<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="CopyReportingConfigPopup.aspx.cs" Inherits="CopyReportingConfigPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td class="ClsGrayMainTitle" align="left">
                <span class="MainTitleHead">Copy Configuration</span>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:ValidationSummary ID="ValSum" runat="server" CssClass="ClsMdtStar" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="80%">
                    <tr>
                        <td align="left">
                            <span class="ClsLabel" style="font-weight: bold;">Copy From :</span>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr id="trCopyFromRole" runat="server">
                        <td align="left" class="ClsBorderlight" width="100px">
                            <span class="ClsLabel">Role :</span>
                        </td>
                        <td class="ClsHilightBGB">
                            <asp:Label ID="lblUserRole" runat="server" Text="" CssClass="ClsLabel" EnableViewState="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel">Name :</span>
                        </td>
                        <td class="ClsHilightBGB">
                            <asp:Label ID="lblName" runat="server" Text="" CssClass="ClsLabel" EnableViewState="true"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td align="left" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span class="ClsLabel" style="font-weight: bold;">Copy To :</span>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:ListView ID="lstvwUsers" runat="server" ItemPlaceholderID="trItemPlaceholder"
                                ClientIDMode="Inherit" DataKeyNames="ReportingUserId">
                                <LayoutTemplate>
                                    <table id="tblDetails" style="width: 100%; color: #333333" class="GridBorder" cellpadding="0"
                                        cellspacing="1">
                                        <tr id="trGroupHeader" runat="server" class="ClsGridHeader">
                                            <th style="width: 50px" align="center">
                                            </th>
                                            <th align="left" style="padding-left: 5px;">
                                                <asp:Label ID="lblNameHeader" runat="server" EnableViewState="False" Text="Name"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr id="trItemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblNoRecordFound" Width="100%" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"
                                                CssClass="LblNoRecord"></asp:Label>
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                        <td align="center">
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </td>
                                        <td align="left">
                                            <asp:Label runat="server" ID="lblName" CssClass="ClsLabel" Text='<%#Eval("Name")%>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <table width="100%">
                                <tr>
                                    <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                        <asp:Label ID="Label14" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                            CssClass="LblNrmlB" Height="16px"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                        <asp:Label ID="Label15" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="This functionality will copy configuration to selected users and mark it as submitted."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="ClsBtn" OnClick="btnCopy_Click"
                                disable-page="true" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="window.close()"
                                CausesValidation="false" />
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                                ClientValidationFunction="ValidateSelection"></asp:CustomValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        _clientlstvwUsers = "<%=this.lstvwUsers.ClientID %>"

        function ValidateSelection(oSrc, args) {
            var isFound = false
            var rowIndex = 0
            var chk = $get(_clientlstvwUsers + '_ctrl' + rowIndex + '_chkSelect')
            while (chk != null) {

                if (chk.checked) {
                    isFound = true;
                    break;
                }

                rowIndex++;
                chk = $get(_clientlstvwUsers + '_ctrl' + rowIndex + '_chkSelect')
            }

            if (!isFound) {
                oSrc.errormessage = "At least one user should be selected."
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

    </script>
</asp:Content>
