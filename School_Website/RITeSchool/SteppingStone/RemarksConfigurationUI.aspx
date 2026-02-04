<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RemarksConfigurationUI.aspx.cs" Inherits="RemarksConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="mainUpdatePanel" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                        CssClass="ClsLabel" ShowSummary="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                        Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                                        ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table border="0" cellpadding="1" cellspacing="2" width="350px" style="margin-left: 19px;">
                                        <tr>
                                            <td align="left" style="width: 120px;" class="ClsBorderlight">
                                            <asp:Label ID="lblRemarkType" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, RemarkType%>"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtRemarkName" runat="server" CssClass="MidTxtBox" 
                                                    MaxLength="30" /><span
                                                    class="ClsMdtStar">&nbsp;&nbsp;*</span>
                                                <asp:RequiredFieldValidator ID="reqvalRemarksName" runat="server" ControlToValidate="txtRemarkName"
                                                    Display="None" ValidationGroup="Save" ErrorMessage="<%$ Resources:LocalizedResources, RemarkTypeShouldNotBeBlank%>"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cstvalRemarkNameValidator" runat="server" ValidationGroup="Save"
                                                    ClientValidationFunction="ValidateRemarkName" Display="None" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 120px" class="ClsBorderlight">
                                                <asp:Label ID="lblSortorder" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,SortOrder%>"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td style="width: 100px">
                                                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="MidTxtBox" MaxLength="2"
                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" /><span
                                                        class="ClsMdtStar">&nbsp;&nbsp;*</span>
                                                <asp:RequiredFieldValidator ID="reqSortOrder" runat="server" ControlToValidate="txtSortOrder"
                                                    Display="None" ValidationGroup="Save" ErrorMessage="<%$ Resources:LocalizedResources, SortOrderShouldNotBeBlank%>"> </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cstvalSortOrderValidator" runat="server" ValidationGroup="Save"
                                                    ClientValidationFunction="ValidateSortOrder" Display="None" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 120px">
                                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Save%>" ValidationGroup="Save" disable-page="true"
                                                    CausesValidation="true" OnClick="btnSave_Click" />
                                            </td>
                                            <td align="left" style="width: 100px">
                                                <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Cancel%>" CausesValidation="false"
                                                    UseSubmitBehavior="false" OnClientClick="ResetControls();" OnClick="btnCancel_Click" />
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
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwRemarks" runat="server" DataKeyNames="Id" OnItemCommand="lstvwRemarks_ItemCommand"
                                        OnDataBound="lstvwRemarks_DataBound" OnItemDataBound="lstvwRemarks_ItemDataBound">
                                        <LayoutTemplate>
                                            <table align="center" width="600px" runat="server" id="tblStaffInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" style="width: 220px; padding-left: 7px;">
                                                       <asp:Label ID="lblRemkType" runat="server" Text="<%$ Resources:LocalizedResources, RemarkType%>"> </asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 80px;">
                                                       <asp:Label ID="lblSortOrder" runat="server" Text="<%$ Resources:LocalizedResources, SortOrder%>"> </asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 50px;">
                                                       <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 50px;">
                                                      <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="left" style="padding-left: 7px">
                                                    <asp:Label ID="lblRemarkName" runat="server" Text='<%# Eval("Name") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"
                                                        CausesValidation="false" CommandName="EDIT_ROW" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"
                                                        CausesValidation="false" CommandName="DELETE_ROW" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                <td align="left" style="padding-left: 7px">
                                                    <asp:Label ID="lblRemarkName" runat="server" Text='<%# Eval("Name") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip="Edit"
                                                        CausesValidation="false" CommandName="EDIT_ROW" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip="Delete"
                                                        CausesValidation="false" CommandName="DELETE_ROW" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="lblNoRecord" CssClass="LblNoRecord" Text="<%$ Resources:LocalizedResources, NoRecordFound%>"
                                                        Width="50%" />
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:HiddenField ID="hidRowCount" runat="server" />
                                    <asp:HiddenField ID="hidRowNo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidRemarksConfigId" runat="server" />
                                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                    <asp:HiddenField ID="hidSave" runat="server" />
                                    <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisRemarkAndAllReleatedTemplates" runat="server" />
                                    <asp:HiddenField ID="hidRemarkTypeShouldNotBeDuplicated" runat="server" />
                                    <asp:HiddenField ID="hidSortOrderShouldNotBeDuplicated" runat="server" />
                                    <asp:HiddenField ID="hidSortOrderShouldNotBeZero" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <br />
                                    <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Back%>" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var _clientbtnSave = '<%=this.btnSave.ClientID %>';
        var _clientlstvwRemarks = '<%=this.lstvwRemarks.ClientID %>';
        var _clienttxtRemarksName = '<%=this.txtRemarkName.ClientID %>';
        var _clienttxtSortOrder = '<%=this.txtSortOrder.ClientID %>';
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>";
        _clienthidRowNo = "<%=this.hidRowNo.ClientID %>";
        _clientlblUpdateMessage = "<%=this.lblUpdateMessage.ClientID %>";
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>";
        var _empty = '';

        // This function is used to reset controls on the page to their default values.
        function ResetControls() {
            var txtRemarksName = $get(_clienttxtRemarksName);
            if (txtRemarksName)
                txtRemarksName.value = _empty;

            var txtSortOrder = $get(_clienttxtSortOrder);
            if (txtSortOrder)
                txtSortOrder.value = _empty;

            var btnSave = $get(_clientbtnSave);
            if (btnSave)
                btnSave.value = document.getElementById("<%=hidSave.ClientID%>").value;
        }

        function ValidateRemarkName(src, args) {
            document.getElementById(_clientlblUpdateMessage).innerHTML = "";
            document.getElementById(_clientlblErrorMsg).innerHTML = "";
            args.IsValid = true;
            var lblRemarkName;
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var remarkName = $get(_clienttxtRemarksName).value;
            var iRowNo = document.getElementById(_clienthidRowNo).value
            for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                lblRemarkName = $get(_clientlstvwRemarks + '_ctrl' + iRowNumber + '_lblRemarkName');
                if (lblRemarkName.innerHTML.toLowerCase() == remarkName.trim().toLowerCase() && iRowNumber != (iRowNo - 1))
                {
                    src.errormessage = document.getElementById("<%=hidRemarkTypeShouldNotBeDuplicated.ClientID%>").value + (iRowNumber + 1);
                    args.IsValid = false;
                    break;
                }
            }
            return !args.IsValid;
        }

        function ValidateSortOrder(src, args) {
            document.getElementById(_clientlblUpdateMessage).innerHTML = "";
            document.getElementById(_clientlblErrorMsg).innerHTML = "";
            args.IsValid = true;
            var lblSortOrder;
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var iRowNo = document.getElementById(_clienthidRowNo).value
            var sortOrder = $get(_clienttxtSortOrder).value;
            for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                lblSortOrder = $get(_clientlstvwRemarks + '_ctrl' + iRowNumber + '_lblSortOrder');
                if (lblSortOrder.innerHTML == sortOrder.trim() && iRowNumber != (iRowNo - 1)) {
                    src.errormessage = document.getElementById("<%=hidSortOrderShouldNotBeDuplicated.ClientID%>").value + (iRowNumber + 1);
                    args.IsValid = false;
                    break;
                } 
            }
            if (sortOrder.trim() == 0 && sortOrder != "") {
                src.errormessage = document.getElementById("<%=hidSortOrderShouldNotBeZero.ClientID%>").value;
                args.IsValid = false;
            }
            return !args.IsValid;
        }

        function ConfirmDelete() {
            var bResult = true  
            if (!window.confirm(document.getElementById("<%=hidAreYouSureYouWantToDeleteThisRemarkAndAllReleatedTemplates.ClientID%>").value)) {
                bResult = false

            }
            return bResult
        }
    </script>
</asp:Content>
