<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AttendanceAlertConfigurationUI.aspx.cs" Inherits="AttendanceAlertConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="upnlError" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="right" class="TxtNormal" style="padding-right: 10px; top: 20px">
                         <span class="ClsMdtStar">*</span>
                                            <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="valsumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"  HeaderText= "<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" />
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
                                    <asp:UpdatePanel ID="upnlData" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td class="ClsBorderlight" width="149px" runat="server" id="tdRole">
                                            <asp:Label ID="lblUserRole" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SelectUserRole%>"
                                                EnableViewState="false"></asp:Label>
                                                 <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbRole" Width="200px" runat="server" CssClass="LrgCombo" CausesValidation="false"
                                                            OnSelectedIndexChanged="cmbRole_SelectedIndexChanged" AutoPostBack="true" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" width="130px" runat="server" id="tdUser">
                                                     <asp:Label ID="lblUser" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SelectUser%>"
                                                EnableViewState="false"></asp:Label>
                                                 <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbUsers" Width="200px" runat="server" AutoPostBack="false"
                                                            TabIndex="2" Enabled="false" CssClass="LrgCombo" CausesValidation="false">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" width="130px" runat="server" id="tdTimeSpan">
                                                 <asp:Label ID="lblTimeSpan" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, TimeSpanInDays%>"
                                                EnableViewState="false"></asp:Label>
                                                 <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDays" runat="server" CssClass="MidTxtBox" Width="75px" onblur="extractNumber(this,0,false);"
                                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                            onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" MaxLength="2"
                                                            CausesValidation="false" Height="19px" TabIndex="3"></asp:TextBox>
                                                        <span class="ClsMdtStar">*</span>                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                       <asp:Label ID="lblTimeSpanMSG" runat="server" CssClass="LblSmlGray" Text= "<%$ Resources:LocalizedResources, DelayTimeSpan%>"
                                                EnableViewState="false"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbRole" EventName="SelectedIndexChanged" />
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
                                                        <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text= "<%$ Resources:LocalizedResources, Save%>" CausesValidation="true" disable-page="true"
                                                            TabIndex="4" OnClick="btnSave_Click" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text= "<%$ Resources:LocalizedResources, Cancel%>" UseSubmitBehavior="true"
                                                            TabIndex="5" CausesValidation="false" OnClick="btnCancel_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbRole" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="60%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlListView" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:ListView ID="lstvwAttendanceAlertConfig" runat="server" DataKeyNames="UserId,NoOfDays,RoleId,ConfigId"
                                                            OnItemCommand="lstvwAttendanceAlertConfig_ItemCommand" OnItemDataBound="lstvwAttendanceAlertConfig_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table align="center" width="100%" runat="server" id="tblUserInfo" style="color: #333333"
                                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th align="left" style="width: 300px; padding-left: 5px">
                                                                        <asp:Label ID="lblUserName" runat="server" Text="<%$ Resources:LocalizedResources, UserName %>" />
                                                                        </th>
                                                                        <th align="right" style="width: 130px; padding-right: 5px">
                                                                           <asp:Label ID="lblTimeSpan" runat="server" Text="<%$ Resources:LocalizedResources, TimeSpan %>" />
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
                                                                    </td>
                                                                    <td align="right" style="padding-right: 5px">
                                                                        <asp:Label ID="lblTimeSpan" runat="server" Text='<%# Eval("NoOfDays") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip= "<%$ Resources:LocalizedResources, Edit %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="UpdateCommand" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip= "<%$ Resources:LocalizedResources, Delete %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                    <td align="left" style="padding-left: 5px">
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                    </td>
                                                                    <td align="right" style="padding-right: 5px">
                                                                        <asp:Label ID="lblTimeSpan" runat="server" Text='<%# Eval("NoOfDays") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip= "<%$ Resources:LocalizedResources, Edit %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="UpdateCommand" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip= "<%$ Resources:LocalizedResources, Delete %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                         <td class="LblNoRecord" align="center">
                                                               <asp:Label ID="lblNoRecord" runat="server" Text= "<%$ Resources:LocalizedResources, NoRecordsFound%>" 
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
                                    <asp:Button ID="btnBack" CssClass="ClsBtn" Text= "<%$ Resources:LocalizedResources, Back%>" runat="server" CausesValidation="false"
                                        TabIndex="20" />
                                    <asp:CustomValidator ID="cstvalRole" runat="server" ClientValidationFunction="validateRole"
                                        Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, ValSelectUserRole%>" SetFocusOnError="True">
                                    </asp:CustomValidator>
                                    <asp:CustomValidator ID="cstvalUser" runat="server" ClientValidationFunction="validateUser"
                                        CausesValidation="false" Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, ValSelectUser%>"
                                        SetFocusOnError="false">
                                    </asp:CustomValidator>
                                    <asp:CustomValidator ID="cstvalDays" runat="server" ClientValidationFunction="validateDays"
                                        CausesValidation="false" Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, ValTimeSpan%>" 
                                        SetFocusOnError="false">
                                    </asp:CustomValidator>
                                    <asp:HiddenField ID="hidConfigId" runat="server" Value="0" />
                                     <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                    <asp:HiddenField ID = "hidValTimeSpan" runat = "server" />
                                    <asp:HiddenField ID = "hidValBlankTimeSpan" runat = "server" />
                                    <asp:HiddenField ID = "hidAlertDeleteUser" runat = "server" />
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

        _clientcmbRole = "<%=this.cmbRole.ClientID %>"
        _clientcmbUsers = "<%=this.cmbUsers.ClientID %>"
        _clienttxtDays = "<%=this.txtDays.ClientID %>"
        _clientlblUpdateMessage = "<%=this.lblUpdateMessage.ClientID %>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientvalsumErrorMsg = "<%=this.valsumErrorMsg.ClientID %>"

        function validateRole(aSrc, args) {
            if (document.getElementById(_clientcmbRole).value == 0) {
                args.IsValid = false;
                document.getElementById(_clientcmbUsers).disabled = true;
                document.getElementById(_clientlblUpdateMessage).innerHTML = "";
                document.getElementById(_clientlblErrorMsg).innerHTML = "";
            }
            else {
                args.IsValid = true;
                document.getElementById(_clientcmbUsers).disabled = false;
                return false;
            }
            return false;
        }

        function validateUser(aSrc, args) {
            if (document.getElementById(_clientcmbUsers).value == 0) {
                args.IsValid = false;
                document.getElementById(_clientlblUpdateMessage).innerHTML = "";
                document.getElementById(_clientlblErrorMsg).innerHTML = "";
            }
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function validateDays(aSrc, args) {
            if (document.getElementById(_clienttxtDays).value == "") {
                args.IsValid = false;
                document.getElementById(_clientlblUpdateMessage).innerHTML = "";
                document.getElementById(_clientlblErrorMsg).innerHTML = "";
                document.getElementById(_clientvalsumErrorMsg).innerHTML = document.getElementById("<%=this.hidValBlankTimeSpan.ClientID %>").value
            }
            else if (document.getElementById(_clienttxtDays).value > 100) {
                args.IsValid = false;
                document.getElementById(_clientlblUpdateMessage).innerHTML = "";
                document.getElementById(_clientlblErrorMsg).innerHTML = "";
                document.getElementById(_clientvalsumErrorMsg).innerHTML = document.getElementById("<%=this.hidValTimeSpan.ClientID %>").value;
            }
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidAlertDeleteUser.ClientID %>").value)) {
                bResult = false                
            }
            return bResult
        }
    </script>
</asp:Content>
