<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="LeaveEnCashmentPopup.aspx.cs" Inherits="LeaveEnCashmentPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td align="center">
                <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                    <tr>
                        <td class="ClsGrayMainTitle" align="left">
                            <span class="MainTitleHead">Encash Leave Details</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td style="width: 77%">
                                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                Height="20px" Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                    <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                        <span class="ClsMdtStar">&nbsp * Mandatory Fields</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                                <asp:CustomValidator ID="cstCheckLeaveCount" runat="server" ErrorMessage="" ClientValidationFunction="CheckLeaveCount"
                                                    Display="None"></asp:CustomValidator>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwEncashLeaveDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdMessage" runat="server" align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwEncashLeaveDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center" width="70%">
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 90px;">
                                    <asp:Label ID="lblHeaderUserName" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                        Text="Name : "></asp:Label>
                                </td>
                                <td id="tdUserName" class="ClsHilightBGB" colspan="5" align="left" runat="server">
                                    <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Width="76%" Height="15px"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trYear" runat="server">
                                <td align="left" valign="middle" class="ClsBorderlight">
                                    <span class="ClsLabel">Year : </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbYear" runat="server" CssClass="MidCombo" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">&nbsp*</span>
                                    <asp:RequiredFieldValidator ID="reqLeaveYear" runat="server" ErrorMessage="Year should be selected."
                                        ControlToValidate="cmbYear" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td align="left" valign="middle" class="ClsBorderlight" style="width: 100px;">
                                    <span class="ClsLabel">Leave Type : </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbLeaveType" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                        OnSelectedIndexChanged="cmbLeaveType_SelectedIndexChanged" EnableViewState="true">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">&nbsp *</span>
                                    <asp:RequiredFieldValidator ID="reqLeaveType" runat="server" ErrorMessage="Leave Type should be selected."
                                        ControlToValidate="cmbLeaveType" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="middle" class="ClsBorderlight">
                                    <span class="ClsLabel">Date : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDate" CssClass="SmlCombo" runat="server"
                                        Width="122px"></asp:TextBox>
                                    <rjs:PopCalendar ID="CalDobPopup" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                        To-Today="false" />
                                </td>
                                <td>
                                </td>
                                <td align="left" valign="middle" class="ClsBorderlight">
                                    <span class="ClsLabel">Leave Balance : </span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtLeaveBalance" CssClass="SmlCombo" runat="server" Enabled="false"
                                                Width="150px"></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbLeaveType" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="middle" class="ClsBorderlight">
                                    <span class="ClsLabel">Leave Count : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLeaveCount" CssClass="SmlCombo" runat="server" MaxLength="5" 
                                        Width="145px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                        onpaste="event.returnValue=false"></asp:TextBox>
                                    <span style="color: #ff0000">&nbsp;*</span>
                                </td>
                                <td>
                                </td>
                                <td align="left" valign="middle" class="ClsBorderlight">
                                    <span class="ClsLabel">Amount : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAmount" CssClass="SmlCombo" runat="server" MaxLength="10"
                                        Width="150px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                        onpaste="event.returnValue=false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="middle" class="ClsBorderlight">
                                    <span class="ClsLabel">Description : </span>
                                </td>
                                <td colspan="5" align="left">
                                    <asp:TextBox ID="txtDescription" CssClass="LrgTxtBox" runat="server"
                                        TextMode="MultiLine" Height="40px" Width="100%"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regTxtDescription" runat="server" ViewStateMode="Enabled"
                                        Display="None" ControlToValidate="txtDescription" ErrorMessage="Length of remarks should not exceed 200 characters."
                                        CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,200}$"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwEncashLeaveDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height: 5px;">
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: center;">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" CausesValidation="True"
                            disable-page="true" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="False"
                            UseSubmitBehavior="False" OnClick="btnCancel_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwEncashLeaveDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height: 5px;">
            </td>
        </tr>
        <tr align="center">
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="70%" align="center" style="text-align: center; margin: 0px auto;">
                            <tr align="center" style="text-align: center;">
                                <td align="center">
                                    <asp:ListView ID="lstvwEncashLeaveDetails" runat="server" DataKeyNames="Id,LeaveId"
                                        OnItemDataBound="lstvwEncashLeaveDetails_ItemDataBound" OnItemCommand="lstvwEncashLeaveDetails_ItemCommand">
                                        <LayoutTemplate>
                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="left" class="clsLabelgrd">
                                                        Leave Type
                                                    </th>
                                                    <th align="center" width="120px" class="clsLabelgrd">
                                                        Date
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="100px">
                                                        Count
                                                    </th>
                                                    <th align="right" class="clsLabelgrd" width="100px" style="padding-right: 5px;">
                                                        Amount
                                                    </th>
                                                    <th width="40px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="lblEdit" runat="server" Text="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                    </th>
                                                    <th width="40px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class="ClsGridRow">
                                                <td align="center">
                                                    <asp:Label ID="lblLeaveType" runat="server" CssClass="ClsLabel" Text='<%#Eval("LeaveType") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("Date") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEncashCount" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("EncashCount") %>'></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                        padding-right: 5px;" Text='<%#Eval("Amount") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="ClsGridAltRow">
                                                <td align="center">
                                                    <asp:Label ID="lblLeaveType" runat="server" CssClass="ClsLabel" Text='<%#Eval("LeaveType") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("Date") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEncashCount" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("EncashCount") %>'></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                        padding-right: 5px;" Text='<%#Eval("Amount") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
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
                                    <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwEncashLeaveDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: center;">
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="False"
                    UseSubmitBehavior="False" OnClientClick="ClosePopup()" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hidUserId" runat="server" />
                <asp:HiddenField ID="hidUserRoleId" runat="server" />
                <asp:HiddenField ID="hidStaffGroupId" runat="server" Value="0" />
                <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                <asp:HiddenField  ID = "hidAmount" runat="server"/>                
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        _clienttxtLeaveCount = "<%=this.txtLeaveCount.ClientID %>"
        _clienttxtLeaveBalance = "<%=this.txtLeaveBalance.ClientID %>"
        _clienthidAmount = "<%=this.hidAmount.ClientID%>"
        _clienttxtDate = "<%=this.txtDate.ClientID %>"
        _clienttxtAmount = "<%=this.txtAmount.ClientID%>"
        function CountAmount() {
            var LeaveCount = $('#' + _clienttxtLeaveCount).val();
            var Amount = $('#' + _clienthidAmount).val();
            var d = $('#' + _clienttxtDate).val();
            var curDate = new Date(document.getElementById(_clienttxtDate).value);
            var curr_year = curDate.getFullYear();
            var curr_month = curDate.getMonth() + 1;
            var Curr_Date = curDate.getDate();
            var NoOfDays = new Date(curr_year, curr_month, 0).getDate();

            var TotalAmount = Math.ceil((Amount / NoOfDays) * LeaveCount)
            document.getElementById(_clienttxtAmount).value = TotalAmount;
        }
        function ClosePopup() {
            window.close();
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function CheckLeaveCount(oSrc, args) {
            var LeaveCount = $('#' + _clienttxtLeaveCount).val();
            var LeaveBalance = $('#' + _clienttxtLeaveBalance).val();
            if (LeaveCount == "") {
                oSrc.errormessage = "Leave count should not be blank.";
                args.IsValid = false
                return true
            }
            else {
                if (LeaveCount == 0) {
                    oSrc.errormessage = "Leave Count should be greater then zero.";
                    args.IsValid = false
                    return true
                }
                else if (parseFloat(LeaveCount) > parseFloat(LeaveBalance)) {
                    oSrc.errormessage = "Leave Count should be less then actual Leave Balance.";
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
