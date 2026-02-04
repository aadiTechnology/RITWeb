<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardsForGradingSystemConfigUI.aspx.cs"
    MasterPageFile="../MasterPages/MasterPage.master" Inherits="StandardsForGradingSystemConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr align="center">
                <td>
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                        Visible="true" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
            </tr>
            <tr align="center">
                <td align="center">
                    <div id="Div1" runat="server" style="width: 50%; overflow: auto;">
                        <asp:ListView ID="lstvwStandards" runat="server" DataKeyNames="StandardId">
                            <LayoutTemplate>
                                <table id="lstvwPayFee" width="350px" style="color: #333" cellpadding="3" cellspacing="1"
                                    class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="left" style="padding: 0 0 0 15px;">
                                            <asp:Label ID="lblStandardName" runat="server" Text="<%$ Resources:LocalizedResources, StandardListgrdStandardsHeader%>"
                                                CausesValidation="false" ForeColor="Black"> </asp:Label>
                                            <th id="thSelectAll" runat="server" align="center" style="padding: 0;">
                                                <asp:CheckBox ID="chkSelectAll" Text="<%$ Resources:LocalizedResources, IsForGradingSystem%>"
                                                    runat="server" Style="font-weight: bold;" onclick="CheckAllUncheckAlls()" CssClass="vertical-align-top all-checkbox" />
                                            </th>
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr class="ClsGridRow">
                                    <td align="center">
                                        <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>'
                                            CssClass="ClspaddingL" />
                                    </td>
                                    <td align="center" id="tdchkPay" runat="server">
                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsForGrading") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="ClsGridAltRow">
                                    <td align="center">
                                        <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>'
                                            CssClass="ClspaddingL" />
                                    </td>
                                    <td align="center" id="tdchkPay" runat="server">
                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsForGrading") %>' />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <tr>
                                    <td class="LblNoRecord" align="center" colspan="4" style="width: 100%; float: left">
                                        No record found.
                                    </td>
                                </tr>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center" id="trNote" runat="server">
                    <table align="center" width="830px">
                        <tr>
                            <td align="left" class="ClsBorderlight " style="width: 80px; background-color: #ffffc4;">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Note%>"
                                    CssClass="LblNrmlB"></asp:Label>
                                <span id="Span1" class="colonPadding">:</span>
                            </td>
                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                <asp:Label ID="lblNote" runat="server" Width="750px" BorderWidth="0px" CssClass="LblSmlV"
                                    Text="<%$ Resources:LocalizedResources, GradingMarkingSystemNote%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr align="center" id="trbtn" runat="server">
                <td align="center">
                    <asp:Button ID="imgBtnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server"
                        CssClass="ClsBtn" BorderWidth="1px" UseSubmitBehavior="false" OnClick="imgBtnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                        CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" UseSubmitBehavior="false"
                        OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _clientListViewId = "<%=this.lstvwStandards.ClientID %>"
        _ClientChkAll = _clientListViewId + "_chkSelectAll";

        function CheckAllUncheckAlls() {
            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked

            var iRowCount = 0
            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkSelect");
            }
        }
       
    </script>
</asp:Content>
