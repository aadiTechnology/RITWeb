<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LessonPlanReportingUserConfigUI.aspx.cs" Inherits="LessonPlanReportingUserConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="upnlMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="0" cellspacing="1">
                <tr id="trMandatory" runat="server">
                    <td align="right" colspan="6">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:ValidationSummary ID="valSumError" runat="server" HeaderText="Please correct following errors."
                            CssClass="ClsMdtStar" ShowMessageBox="false" ShowSummary="true" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct following errors."
                            ValidationGroup="Submit" CssClass="ClsMdtStar" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td align="center" id="tdMessage" runat="server" colspan="2">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="lblStaffName" runat="server" CssClass="ClsLabel" Text="Teacher Name :"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbStaffName" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                        Width="250px" OnSelectedIndexChanged="cmbStaffName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">*</span>
                                    <asp:CustomValidator ID="cstvalcmbStaffName" runat="server" ClientValidationFunction="ValidatecmbStaffName"
                                        CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfiguration" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="70%">
                    <tr>
                        <td align="center">
                            <hr style="border: thin solid #C0C0C0" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="lblReportingStaffName" runat="server" CssClass="ClsLabel" Text="Reporting Teacher Name : "></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbReportingStaffName" runat="server" CssClass="LrgCombo" Width="250px">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">*</span>
                                    <asp:CustomValidator ID="cstValReportingStaffName" runat="server" ClientValidationFunction="ValidateReportingStaffName"
                                        CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="lblIsFinalApprover" runat="server" CssClass="ClsLabel" Text="<%$Resources:LocalizedResources, IsFinalApprover%>"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:CheckBox ID="chkfinalApprover" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Approval Sort Order"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtApprovalSortOrder" runat="server" CssClass="SmlTxtBox" MaxLength="2"
                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateApprovalOrder"
                                        CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateApprovalSortOrder"
                                        CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                        BorderWidth="1px" CommandName="Save" OnClick="btnSave_Click"></asp:Button>
                                    <asp:CustomValidator ID="cstvalSave" runat="server" ClientValidationFunction="ValidateDuplicateStaff"
                                        CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                </td>
                                <td align="left">
                                    <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                        Text="<%$ Resources:LocalizedResources, Cancel%>" BorderWidth="1px" OnClick="btnCancel_Click">
                                    </asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:HiddenField ID="hidReportingConfigId" Value="0" runat="server" />
                                    <asp:HiddenField ID="hidEditedUserId" Value="0" runat="server" />
                                    <asp:HiddenField ID="hidStaffNameSelected" runat="server" />
                                    <asp:HiddenField ID="hidReporting" runat="server" />
                                    <asp:HiddenField ID="hidStaffAlreadyexist" runat="server" />
                                    <asp:HiddenField ID="hidStaffNameshouldNotSame" runat="server" />
                                    <asp:HiddenField ID="hidQueryString" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfiguration" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbStaffName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table width="70%">
                            <tr>
                                <td align="center" valign="top" runat="server" id="tdDetails">
                                    <asp:ListView ID="lstvwConfiguration" runat="server" ItemPlaceholderID="trItemPlaceholder"
                                        ClientIDMode="Inherit" OnItemCommand="lstvwConfiguration_ItemCommand" DataKeyNames="Id,IsSubmitted,ReportingUserId"
                                        OnItemDataBound="lstvwConfiguration_ItemDataBound">
                                        <LayoutTemplate>
                                            <table id="tblDetails" style="width: 100%; color: #333333" class="GridBorder" cellpadding="0"
                                                cellspacing="1">
                                                <tr id="trGroupHeader" runat="server" class="ClsGridHeader">
                                                    <th class="ClspaddingL" style="width: 40%;">
                                                        <asp:Label ID="lblName" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Name%>"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 20%; text-align: center; padding-left: 10px; white-space: nowrap">
                                                        <asp:Label ID="lblhidIsFinalApprover" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, IsFinalApprover%>"></asp:Label>
                                                    </th>
                                                    <th align="right" style="width: 18%; white-space: nowrap; padding-right: 5px">
                                                        <asp:Label ID="lblhidIsSupervisor" runat="server" EnableViewState="False" Text="Approval Sort Order"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 10%; white-space: nowrap" class="Clspadding" id="thEdit"
                                                        runat="server">
                                                        <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 12%; white-space: nowrap" class="Clspadding" id="thDelete"
                                                        runat="server">
                                                        <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="trItemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                <td class="ClspaddingL" style="width: 46%;">
                                                    <asp:Label runat="server" ID="lblName" Text='<%#Eval("UserName")%>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hidUserId" Value='<%#Eval("ReportingUserId")%>' />
                                                </td>
                                                <td align="center">
                                                    <img id="imgIsFinal" runat="server" src="../images/IconGrid_AssignTrue.gif" alt="Is Final Approver"
                                                        visible='<%#Eval("IsFinalApprover")%>' />
                                                    <asp:HiddenField runat="server" ID="hidIsFinalApprover" />
                                                </td>
                                                <td align="right" style="padding-right: 5px">
                                                    <asp:Label runat="server" ID="lblApprovalSortOrder" Text='<%#Eval("ApprovalSortOrder")%>'></asp:Label>
                                                </td>
                                                <td align="center" style="width: 10%;" class="Clspadding" id="tdEdit" runat="server">
                                                    <asp:ImageButton runat="server" ID="imgEdit" CommandName="UpdateCommand" CommandArgument='<%#Eval("Id")%>'
                                                        CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" AlternateText="<%$ Resources:LocalizedResources, Edit%>"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>"></asp:ImageButton>
                                                </td>
                                                <td align="center" style="width: 12%;" class="Clspadding" id="tdDelete" runat="server">
                                                    <asp:ImageButton runat="server" ID="imgDelete" CommandName="RemoveCommand" AlternateText="<%$ Resources:LocalizedResources, Delete%>"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument='<%#Eval("Id")%>'
                                                        CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDelete()) return false;">
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td align="center" colspan="5" class="LblNoRecord">
                                                    <asp:Label ID="lblNoRecordFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbStaffName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnlButton" runat="server">
                    <ContentTemplate>
                        <asp:Button CssClass="ClsBtn" ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>"
                            BorderWidth="1px" CausesValidation="false"></asp:Button>
                        <asp:Button CssClass="ClsBtn" ID="btnSubmit" runat="server" ValidationGroup="Submit"
                            Enabled="false" Text="<%$ Resources:LocalizedResources, Submit%>" BorderWidth="1px"
                            OnClick="btnSubmit_Click"></asp:Button>
                        <asp:Button CssClass="ClsBtn" ID="btnUnSubmit" CausesValidation="false" runat="server"
                            Enabled="false" Text="<%$ Resources:LocalizedResources, UnSubmit%>" BorderWidth="1px"
                            OnClick="btnUnSubmit_Click"></asp:Button>
                        <asp:Button CssClass="ClsBtn" ID="btnDeleteAll" CausesValidation="false" runat="server"
                            Enabled="false" Text="<%$ Resources:LocalizedResources, DeleteAll%>" BorderWidth="1px"
                            OnClick="btnDeleteAll_Click" OnClientClick="if(!ConfirmDeleteAll()) return false;">
                        </asp:Button>
                        <asp:Button CssClass="ClsBtn" ID="btnCopyConfig" CausesValidation="false" OnClientClick="OpenPopup(); return false;"
                            runat="server" Width="125px" Text="Copy Configuration" BorderWidth="1px"></asp:Button>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="ValidateFinalApprover"
                            ValidationGroup="Submit" CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfiguration" EventName="ItemDataBound" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table align="center" width="50%">
                    <tr>
                        <td align="left" class="ClsBorderlight " style="width: 3%; background-color: #ffffc4;
                            white-space: nowrap;">
                            <asp:Label ID="lblNote" runat="server" Text="<%$ Resources:LocalizedResources, Note%>"
                                CssClass="LblNrmlB" Style="font-weight: bold; white-space: nowrap;" />
                            <span class="colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="padding-left: 5px; white-space: nowrap;">
                            <asp:Label ID="lblVerifyNote1" runat="server" BorderWidth="0px" Text="On submitting the configuration, Lesson Plan screen will be made available to the reporting teacher and if configuration is un-submitted then screen will be removed."></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        _clientcmbStaffName = "<%=this.cmbStaffName.ClientID %>";
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        _clientcmbReportingStaffName = "<%=this.cmbReportingStaffName.ClientID %>";
        _clienthidReportingConfigId = "<%=this.hidReportingConfigId.ClientID %>";
        _clientlstvwConfiguration = "<%=this.lstvwConfiguration.ClientID %>";
        _clienthidEditedUserId = "<%=this.hidEditedUserId.ClientID %>";
        _clientchkfinalApprover = "<%=this.chkfinalApprover.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);

        function EndReqHandler(sender, args) {

        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm("Are you sure you want to delete this record?")) {
                bResult = false
            }
            return bResult
        }

        function ConfirmDeleteAll() {
            var bResult = true
            if (!window.confirm("Are you sure you want to delete all records?")) {
                bResult = false
            }
            return bResult
        }

        function ValidatecmbStaffName(oSrc, args) {
            if ($get(_clientcmbStaffName) != null && ($get(_clientcmbStaffName).value).trim() == "0") {
                $get(_clientlblMessage).innerHTML = "";
                oSrc.errormessage = $get("<%=hidStaffNameSelected.ClientID%>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return true;
        }

        function ValidateReportingStaffName(oSrc, args) {

            if ($get(_clientcmbReportingStaffName) != null && ($get(_clientcmbReportingStaffName).value).trim() == "0") {
                $get(_clientlblMessage).innerHTML = "";
                oSrc.errormessage = $get("<%=hidReporting.ClientID%>").value + ' ' + $get("<%=hidStaffNameSelected.ClientID%>").value;
                args.IsValid = false;
                return true;
            }
            else if ($get(_clientcmbStaffName) != null && $get(_clientcmbReportingStaffName) != null && $get(_clientcmbReportingStaffName).value != "0" && $get(_clientcmbReportingStaffName).value == $get(_clientcmbStaffName).value) {
                $get(_clientlblMessage).innerHTML = "";
                oSrc.errormessage = $get("<%=hidStaffNameshouldNotSame.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return true;
        }

        function ValidateDuplicateStaff(oSrc, args) {
            var IsEdit = false, IsExists = false;
            var EditedUserId = $get(_clienthidEditedUserId);
            var SelectedUserId = $get(_clientcmbReportingStaffName);

            if ($get(_clienthidReportingConfigId).value != "0" && $get(_clienthidReportingConfigId).value != "")
                IsEdit = true;

            var iRowCount = 0
            var hidUserId = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidUserId")
            while (hidUserId != null) {
                if (!IsEdit) {
                    if (SelectedUserId != null && hidUserId.value == SelectedUserId.value) {
                        IsExists = true;
                        break;
                    }
                }
                else {
                    if (SelectedUserId != null && hidUserId.value == SelectedUserId.value && EditedUserId != null && hidUserId.value != EditedUserId.value) {
                        IsExists = true;
                        break;
                    }
                }

                iRowCount = iRowCount + 1;
                hidUserId = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidUserId");
            }

            if (IsExists) {
                $get(_clientlblMessage).innerHTML = "";
                oSrc.errormessage = $get("<%=hidStaffAlreadyexist.ClientID%>").value;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return true;
        }

        function ValidateApprovalOrder(oSrc, args) {
            var newSortOrder = $('#' + "<%=this.txtApprovalSortOrder.ClientID %>").val()
            if (newSortOrder == "" || parseInt(newSortOrder) == 0) {
                oSrc.errormessage = "Approval Sort Order should not be blank or zero."
                args.IsValid = false;
                return true;
            }

            var iRowCount = 0
            var isFound = false
            var IsMoreThanfinalApproverSortOrder = false
            var EditedUserId = $get(_clienthidEditedUserId);

            var hidUserId = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidUserId")
            while (hidUserId != null) {
                SortOrder = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_lblApprovalSortOrder").innerHTML
                if (parseInt(EditedUserId.value) != parseInt(hidUserId.value) && parseInt(SortOrder) == parseInt(newSortOrder)) {
                    isFound = true
                    break
                }
                else {
                    var isFinalApprover = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidIsFinalApprover").value
                    if (parseInt(EditedUserId.value) != parseInt(hidUserId.value) && isFinalApprover == "Y" && parseInt(SortOrder) <= parseInt(newSortOrder)) {
                        IsMoreThanfinalApproverSortOrder = true
                        break
                    }
                }

                iRowCount = iRowCount + 1;
                hidUserId = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidUserId");
            }

            if (isFound) {
                oSrc.errormessage = "Approval Sort Order should not be duplicate."
                args.IsValid = false;
                return true;
            }
            else if (IsMoreThanfinalApproverSortOrder) {
                var isFinalApprover = $get(_clientchkfinalApprover).checked
                if (!isFinalApprover) {
                    oSrc.errormessage = "Approval Sort Order should not be greater than approval sort order of final approver."
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateApprovalSortOrder(oSrc, args) {
            var newSortOrder = $('#' + "<%=this.txtApprovalSortOrder.ClientID %>").val()
            var isFinalApprover = $get(_clientchkfinalApprover).checked

            if (newSortOrder != "" && parseInt(newSortOrder) != 0) {
                var iRowCount = 0
                var isFound = false
                var IsMoreThanfinalApproverSortOrder = false
                var EditedUserId = $get(_clienthidEditedUserId);

                var hidUserId = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidUserId")
                while (hidUserId != null) {
                    SortOrder = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_lblApprovalSortOrder").innerHTML
                    if (parseInt(EditedUserId.value) != parseInt(hidUserId.value) && isFinalApprover && parseInt(SortOrder) > parseInt(newSortOrder)) {
                        isFound = true
                        break
                    }

                    iRowCount = iRowCount + 1;
                    hidUserId = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidUserId");
                }

                if (isFound) {
                    oSrc.errormessage = "Approval Sort Order of final approver should not be less than approval sort order of other approvers."
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }
        }

        function ValidateFinalApprover(oSrc, args) {
            var iRowCount = 0
            var isFound = false
            var hidUserId = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidUserId")
            while (hidUserId != null) {
                var isFinalApprover = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidIsFinalApprover").value
                if (isFinalApprover == "Y") {
                    isFound = true
                    break
                }

                iRowCount = iRowCount + 1;
                hidUserId = document.getElementById(_clientlstvwConfiguration + "_ctrl" + iRowCount + "_hidUserId");
            }

            if (!isFound) {
                oSrc.errormessage = "Please set at least one user as final approver."
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }

        }

        function OpenPopup() {
            var queryString = $('#' + "<%=this.hidQueryString.ClientID %>").val()
            window.open('../StaffPerformance/CopyReportingConfigPopup.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=500,height=500').focus();
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
