<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="OwnerAssignmentPopup.aspx.cs" Inherits="OwnerAssignmentPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="left">
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="left" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">Owner(s)</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td align="right">
                    <div style="float: right; vertical-align: top;">
                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="* Mandatory Fields"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" />
                    <asp:CustomValidator ID="cstValStaffGroup" runat="server" Display="None" ClientValidationFunction="ValidateStaffGroup"
                        SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValUser" runat="server" Display="None" ClientValidationFunction="ValidateUser"
                        SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Blue"
                                EnableViewState="false" CssClass="ErrMsg"></asp:Label>                            
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwOwners" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="90%">
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="150px">
                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Date "></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                     <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Query "></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblTitle" runat="server" CssClass="ClsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Category(s) "></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblCategories" runat="server" CssClass="ClsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trUserRole" runat="server">
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text="User Role "></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                            OnSelectedIndexChanged="cmbUserRole_SelectedIndexChanged" Enabled="False">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr id="trUser" runat="server">
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" Text="Owner Name "></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="cmbUser" runat="server" CssClass="ExLrgCombo">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">* </span>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwOwners" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                OnClick="btnCancel_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwOwners" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwOwners" runat="server" DataKeyNames="Id,OwnerId,UserRoleId"
                                            OnItemDataBound="lstvwOwners_ItemDataBound" OnItemCommand="lstvwOwners_ItemCommand">
                                            <LayoutTemplate>
                                                <table width="95%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" style="width: 200px; padding-left: 5px">
                                                            User Role
                                                        </th>
                                                        <th align="left" style="padding-left: 5px">
                                                            Owner Name
                                                        </th>
                                                        <th width="50px" align="center" class="clsLabelgrd">
                                                            <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                        </th>
                                                        <th width="50px" class="clsLabelgrd">
                                                            <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblUserRolw" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserRole") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("OwnerName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblUserRolw" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserRole") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("OwnerName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidQuestionId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsOwnerAssignmentSubmitted" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsCommunicationStarted" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwOwners" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" OnClick="btnSubmit_Click"
                                CausesValidation="False" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false"
                                OnClientClick="ClosePopup()" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwOwners" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            var _clientCmbUserRole = "<%=this.cmbUserRole.ClientID %>";
            var _clientCmbUser = "<%=this.cmbUser.ClientID %>";

            function ClosePopup() {
                window.close();
            }

            function ValidateStaffGroup(oSrc, args) {
                var staffGroupId = $get(_clientCmbUserRole).value
                if (staffGroupId == 0) {
                    oSrc.errormessage = "User Role should be selected.";
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function ValidateUser(oSrc, args) {
                var userId = $get(_clientCmbUser).value
                if (userId == 0) {
                    oSrc.errormessage = "Owner Name should be selected.";
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function CloseWindow() {
                window.close();
                window.opener.focus();
                window.opener.ShowMessage(9999);
            }

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?')
            }

        </script>
    </div>
</asp:Content>
