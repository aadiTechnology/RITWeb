<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ReportingUserConfigurationUI.aspx.cs" Inherits="ReportingUserConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="upnlError" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="right" class="TxtNormal" style="padding-right: 10px; top: 20px">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                            Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="valsumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                            HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                            ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                        <asp:Label ID="lblErrorMsg" Style="text-align: center" runat="server" ForeColor="Red"
                            Width="100%" CssClass="ClsMdtStar"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%" align="center">
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="upnlMaster">
                    <ContentTemplate>
                        <table width="70%" align="center" cellpadding="0" cellspacing="1" border="0">
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upnlData" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td class="ClsBorderlight" width="149px" runat="server" id="td1">
                                                        <asp:Label ID="lblReportingParameter" runat="server" CssClass="ClsLabel" Text=" Reporting Parameter"
                                                            EnableViewState="false"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbReportingParameter" Width="200px" runat="server" CssClass="LrgCombo"
                                                            AutoPostBack="true" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                        <asp:RequiredFieldValidator ID="reqcmbReportingParameter" runat="server" Display="None"
                                                            InitialValue="0" ControlToValidate="cmbReportingParameter" ErrorMessage="Reporting Parameter should be selected."></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" width="149px" runat="server" id="tdRole">
                                                        <asp:Label ID="lblUserRole" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, SelectUserRole%>"
                                                            EnableViewState="false"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbRole" Width="200px" runat="server" CssClass="LrgCombo" OnSelectedIndexChanged="cmbRole_SelectedIndexChanged"
                                                            AutoPostBack="true" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                        <asp:RequiredFieldValidator ID="reqcmbRole" runat="server" Display="None" InitialValue="0"
                                                            ControlToValidate="cmbRole" ErrorMessage="User Role should be selected."></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" width="130px" runat="server" id="tdUser">
                                                        <asp:Label ID="lblUser" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, SelectUser%>"
                                                            EnableViewState="false"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbUsers" Width="200px" runat="server" AutoPostBack="false"
                                                            TabIndex="2" Enabled="false" CssClass="LrgCombo">
                                                        </asp:DropDownList>
                                                        <%--  <asp:ListItem Text="---Select---" Value="0" />--%>
                                                        <span class="ClsMdtStar">*</span>
                                                      <%--  <asp:RequiredFieldValidator ID="reqcmbUsers" runat="server" Display="None" InitialValue="0"
                                                            ControlToValidate="cmbUsers" ErrorMessage="User should be selected."></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbRole" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbReportingParameter" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnls" runat="server">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Save%>"
                                                            disable-page="true" TabIndex="4" OnClick="btnSave_Click" CausesValidation="true" />
                                                        <asp:CustomValidator ID="cstvalRole" runat="server" ClientValidationFunction="CheckDuplication"
                                                            Display="None" ErrorMessage="Select User Role" SetFocusOnError="True">
                                                        </asp:CustomValidator>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Cancel%>" CausesValidation="false"
                                                            UseSubmitBehavior="true" TabIndex="5" OnClick="btnCancel_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbRole" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbReportingParameter" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="60%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlListView" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ListView ID="lstvwReportingParameter" runat="server" DataKeyNames="UserId,RoleId,ReportingPrameterId"
                                                            OnItemCommand="lstvwReportingParameter_ItemCommand" OnItemDataBound="lstvwReportingParameter_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table align="center" width="100%" runat="server" id="tblUserInfo" style="color: #333333"
                                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th align="left" style="width: 300px; padding-left: 5px">
                                                                            <asp:Label ID="lblUserName" runat="server" Text="<%$ Resources:LocalizedResources, UserName %>" />
                                                                        </th>
                                                                        <th align="left" style="width: 300px; padding-right: 5px">
                                                                            <asp:Label ID="lblReportingParameter" runat="server" Text="Reporting Parameter" />
                                                                        </th>
                                                                        <th align="center" style="width: 75px">
                                                                            <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit %>" />
                                                                        </th>
                                                                        <th align="center" style="width: 75px">
                                                                            <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete %>" />
                                                                        </th>
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                                    <td align="left" style="padding-left: 5px">
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                        <asp:HiddenField ID="hidUserId" runat="server" Value='<%# Eval("UserId") %>' />
                                                                    </td>
                                                                    <td align="left" style="padding-right: 5px">
                                                                        <asp:Label ID="lblParameter" runat="server" Text='<%# Eval("ReportingParameterName") %>' />
                                                                        <asp:HiddenField ID="HidParameterId" runat="server" Value='<%# Eval("ReportingPrameterId") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="UpdateCommand" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                        <asp:HiddenField ID="ReportingId" runat="server"  Value='<%# Eval("ReportingId") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                    <td align="left" style="padding-left: 5px">
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                        <asp:HiddenField ID="hidUserId" runat="server" Value='<%# Eval("UserId") %>' />
                                                                    </td>
                                                                    <td align="left" style="padding-right: 5px">
                                                                        <asp:Label ID="lblParameter" runat="server" Text='<%# Eval("ReportingParameterName") %>' />
                                                                        <asp:HiddenField ID="HidParameterId" runat="server" Value='<%# Eval("ReportingPrameterId") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="UpdateCommand" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                        <asp:HiddenField ID="ReportingId" runat="server"  Value='<%# Eval("ReportingId") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="LblNoRecord" align="center">
                                                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"
                                                                                EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnBack" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Back%>"
                                        runat="server" TabIndex="20" CausesValidation="false" />                                   
                                    <asp:HiddenField ID="hidConfigId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                    <asp:HiddenField ID="hidValTimeSpan" runat="server" />
                                    <asp:HiddenField ID="hidValBlankTimeSpan" runat="server" />
                                    <asp:HiddenField ID="hidAlertDeleteUser" runat="server" />
                                </td>
                            </tr>
                            <tr id="trPrecondition" runat="server" visible="false">
                                <td align="left">
                                    <div runat="server" id="divErr">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        _clientcmbRole = "<%=this.cmbRole.ClientID %>";
        _clientcmbUsers = "<%=this.cmbUsers.ClientID %>";
        _clientcmbReportType = "<%=this.cmbReportingParameter.ClientID %>";
        _clientlblUpdateMessage = "<%=this.lblUpdateMessage.ClientID %>";
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clientvalsumErrorMsg = "<%=this.valsumErrorMsg.ClientID %>";
        _clientListViewId = "<%=this.lstvwReportingParameter.ClientID %>";
        _clienthidConfigId = "<%=this.hidConfigId.ClientID %>";

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidAlertDeleteUser.ClientID %>").value)) {
                bResult = false
            }
            return bResult
        }

        function CheckDuplication(oSrc, args) {
            var iRowCount = 0;
            var IsFound = false;var UserSelected = true;
            var Type = document.getElementById(_clientcmbReportType)
            var UserId = document.getElementById(_clientcmbUsers)
            var EditedId = $get(_clienthidConfigId).value;

            if (UserId != null && (UserId.value == "-- Select --" || UserId.value == "0"))
                UserSelected = false;

            var IsFound = false;
            var Parameter = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_HidParameterId")
            var ReportingId = $get(_clientListViewId + "_ctrl" + iRowCount + "_ReportingId");
            while (Parameter != null && UserSelected) {
                if (Parameter.value == Type.value && EditedId != ReportingId.value) {
                    var User = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_hidUserId")
                    if (User.value == UserId.value) {
                        IsFound = true;
                        break;
                    }
                }
                iRowCount = iRowCount + 1;
                Parameter = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_HidParameterId")
                ReportingId = $get(_clientListViewId + "_ctrl" + iRowCount + "_ReportingId");
            }

            if (!UserSelected) {
                oSrc.errormessage = "User should be selected.";
                args.IsValid = false;
                return false;
            }
            else if (IsFound) {
                oSrc.errormessage = "Reporting Parameter should not be duplicate for user.";
                args.IsValid = false;
                return false;
            }
            args.IsValid = true;
            return true
        }


    </script>
</asp:Content>
