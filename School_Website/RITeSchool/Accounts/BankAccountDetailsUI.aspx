<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BankAccountDetailsUI.aspx.cs" Inherits="BankAccountDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="mainUpdatePanel" runat="server">
        <ContentTemplate>
            <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 850px;
                vertical-align: top">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 900px;">
                            <tr>
                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px;">
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsMdtStar" ShowSummary="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblUpdateSucess" runat="server" CssClass="ClsLabelUpdate" EnableViewState="False"
                            Font-Bold="True" ForeColor="Blue" Style="display: block; margin: 5px 0;" />
                        <asp:Label ID="lblError" runat="server" EnableViewState="false" ForeColor="Red" CssClass="ClsTextNormal"
                            Style="display: block; margin: 5px 0;" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="1" cellspacing="2" align="center">
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 170px; padding-left: 5px;">
                                    <span>Bank Name :</span>
                                </td>
                                <td class="ClsLabel" style="padding: 0;">
                                    <asp:DropDownList ID="cmbBankName" runat="server" Width="360px" />
                                    <span class="ClsMdtStar">* </span>
                                    <asp:RequiredFieldValidator ID="reqBankName" runat="server" ControlToValidate="cmbBankName"
                                        InitialValue="0" ErrorMessage="Bank name should be selected." Display="None" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 170px; padding-left: 5px;">
                                    <span>Alias :</span>
                                </td>
                                <td class="ClsLabel" style="padding: 0;">
                                    <asp:TextBox ID="txtAlias" runat="server" MaxLength="100" CssClass="SmlTxtBox" AutoPostBack="false"
                                        Width="360px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 170px; padding-left: 5px;">
                                    <span>Account Number :</span>
                                </td>
                                <td class="ClsLabel" style="padding: 0;">
                                    <asp:TextBox ID="txtAcNo" runat="server" MaxLength="50" CssClass="ExLrgTxtBox" AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 170px; padding-left: 5px;">
                                    <span>Opening Balance :</span>
                                </td>
                                <td class="ClsLabel" style="padding: 0;">
                                    <asp:TextBox ID="txtAmt" runat="server" MaxLength="11" CssClass="SmlTxtBox" AutoPostBack="false"
                                        onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);"
                                        onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                        ondrop="event.returnValue=false" />
                                    <asp:DropDownList ID="cmbDebit" runat="server" CssClass="MidCombo">
                                        <asp:ListItem Value="0" Selected="True">Credit</asp:ListItem>
                                        <asp:ListItem Value="1">Debit</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 170px; padding-left: 5px;">
                                    <span>Bank Address : </span>
                                </td>
                                <td class="ClsLabel" style="padding: 0;">
                                    <asp:TextBox ID="txtBankAddress" runat="server" CssClass="ExLrgTxtBox" MaxLength="200"
                                        AutoPostBack="false" TextMode="MultiLine" />
                                    <asp:CustomValidator ID="cstValAddress" runat="server" Display="None" ClientValidationFunction="ValidateAddress" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 170px; padding-left: 5px;">
                                    <span>Is Online Bank ?</span>
                                </td>
                                <td class="ClsLabel" style="padding: 0;">
                                    <asp:CheckBox ID="chkOnlineTransactions" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 170px; padding-left: 5px;">
                                    <span>Is School Fee Default Bank ?</span>
                                </td>
                                <td class="ClsLabel" style="padding: 0;">
                                    <asp:CheckBox ID="chkIsDefault" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 170px; padding-left: 5px;">
                                    <span>Is Internal Fee Default Bank ?</span>
                                </td>
                                <td class="ClsLabel" style="padding: 0;">
                                    <asp:CheckBox ID="chkIsInternalDefault" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr align="center">
                    <td align="center" colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click"
                            Style="margin: 5px 0;" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" UseSubmitBehavior="false"
                            CausesValidation="false" Style="margin: 5px 0;" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table id="tblThemes" align="center">
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwBankDetails" runat="server" DataKeyNames="Id,OpeningBalance,IsDebit,OriginalLedger,Bank,Address,IsForOnlineTransactions"
                                        OnItemCommand="lstvwBankDetails_ItemCommand" OnItemDataBound="lstvwBankDetails_ItemDataBound"
                                        OnSorting="lstvwBankDetails_Sorting">
                                        <LayoutTemplate>
                                            <table style="margin: 5px 0;" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 0; white-space: nowrap;">
                                                                    <span class="ClsLblLgnd">Legend :</span>
                                                                </td>
                                                                <td style="width: 0;">
                                                                    <span style="display: inline-block; background-color: LightBlue; border: 1px solid black;
                                                                        height: 20px; width: 20px; float: left"></span>
                                                                </td>
                                                                <td style="width: 0; white-space: nowrap;" align="left">
                                                                    <span class="ClsLblLgnd">Online Bank</span>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                                    <asp:Label ID="Label7" runat="server" BackColor="White" CssClass="ClsLblLgnd" Text="School Fee Default Bank"
                                                                        ForeColor="Maroon" Font-Bold="True" BorderStyle="None" BorderWidth="1px" ReadOnly="True"
                                                                        Width="140px" EnableViewState="False"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                                    <asp:Label ID="Label1" runat="server" BackColor="White" CssClass="ClsLblLgnd" Text="Internal Fee Default Bank"
                                                                        ForeColor="Navy" Font-Bold="True" BorderStyle="None" BorderWidth="1px" ReadOnly="True"
                                                                        Width="150px" EnableViewState="False"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                                    <asp:Label ID="Label2" runat="server" BackColor="White" CssClass="ClsLblLgnd" Text="Default Bank for School Fee & Internal Fee"
                                                                        ForeColor="Olive" Font-Bold="True" BorderStyle="None" BorderWidth="1px" ReadOnly="True"
                                                                        Width="240px" EnableViewState="False"></asp:Label>
                                                                </td>
                                                            </tr>                                                            
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td align="right">
                                                        <div class="ClsGreenBG" style="float: right; height: 18px; vertical-align: bottom;
                                                            padding-top: 4px; padding-right: 2px">
                                                            <a href="#" class="SubTitle" onclick="window.open('ChequeTemplateConfigurationUI.aspx', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1,top=0,left=0,width=900,height=700'); return false;">
                                                                Cheque Template Configuration</a>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>                                            
                                            <table cellspacing="1" cellpadding="3" width="850px" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="center" style="font-size: 9pt; width: 40px; white-space: nowrap;">
                                                        Sr. No.
                                                    </th>
                                                    <th align="left" style="font-size: 9pt; width: 250px; white-space: nowrap;">
                                                        <asp:LinkButton ID="lnkBankName" runat="server" CommandArgument="BankName" CommandName="Sort"
                                                            CausesValidation="false" ForeColor="Black" Text="Bank Name" />
                                                    </th>
                                                    <th align="left" style="font-size: 9pt; width: 200px; white-space: nowrap;">
                                                        <asp:LinkButton ID="lnkbtnAlias" runat="server" CommandArgument="Alias" CommandName="Sort"
                                                            CausesValidation="false" ForeColor="Black" Text="Alias" />
                                                    </th>
                                                    <th align="left" style="font-size: 9pt; width: 130px; white-space: nowrap;">
                                                        <asp:LinkButton ID="lnkAccountNo" runat="server" CommandArgument="BankAccountNumber"
                                                            CommandName="Sort" CausesValidation="false" ForeColor="Black" Text="Account Number" />
                                                    </th>
                                                    <th align="right" style="font-size: 9pt; width: 160px; padding-right: 6px; white-space: nowrap;">
                                                        <asp:LinkButton ID="lnkbtnOpeningBal" runat="server" CommandArgument="OpeningBalance"
                                                            CommandName="Sort" CausesValidation="false" ForeColor="Black" Text="Opening Balance (Rs.)" />
                                                    </th>
                                                    <th align="center" style="font-size: 9pt; width: 50px;">
                                                        Action
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="center">
                                                    <asp:Label ID="lblSrNo" runat="Server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("Bank.Name") %>' />
                                                    <asp:HiddenField ID="hidIsDefault" runat="server" Value='<%#Eval("IsDefault")%>' />
                                                    <asp:HiddenField ID="hidIsInternalDefault" runat="server" Value='<%#Eval("IsInternalDefault")%>' />
                                                    <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAlias" runat="server" Text='<%# Eval("Alias") %>' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblAcNo" runat="server" Text='<%# Eval("AccountNumber") %>' />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblOpeningBalance" runat="server" Style="padding-right: 3px;"><%# GetOpeningBalText(Eval("OpeningBalance"), Eval("IsDebit")) %></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="LblNoRecord" style="text-align: center; width: 850px;">
                                                No record found.
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                            UseSubmitBehavior="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hidMode" runat="server" />
                        <asp:HiddenField ID="hidLedgerId" runat="server" />
                        <asp:HiddenField ID="hidRowNo" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        var _clientcmbBankName = "<%= cmbBankName.ClientID %>";
        var _clienttxtAlias = "<%= txtAlias.ClientID %>";
        var _clienttxtAcNo = "<%= txtAcNo.ClientID %>";
        var _clienttxtAmt = "<%= txtAmt.ClientID %>";
        var _clientcmbDebit = "<%= cmbDebit.ClientID %>";
        var _clienttxtBankAddress = "<%= txtBankAddress.ClientID %>";
        var _clientvalSumErrorMsg = "<%= valSumErrorMsg.ClientID %>";
        var _clientlblUpdateSucess = "<%= lblUpdateSucess.ClientID%>";
        var _clientbtnSave = "<%= btnSave.ClientID %>";
        var _clienthidMode = "<%= hidMode.ClientID %>";
        var _clientlblError = "<%= lblError.ClientID %>";
        var _clientcstValAddress = "<%= cstValAddress.ClientID %>";
        var _clientchkOnlineTransactions = '<%= chkOnlineTransactions.ClientID %>';
        var _clientlstvwBankDetails = "<%=this.lstvwBankDetails.ClientID %>";
        var _clienthidLedgerId = "<%=this.hidLedgerId.ClientID %>";
        var _clientchkIsDefault = "<%=this.chkIsDefault.ClientID %>";
        var _clientchkIsInternalDefault = "<%=this.chkIsInternalDefault.ClientID %>";

        function ClearMessages() {
            $get(_clientlblUpdateSucess).innerText = "";
            $get(_clientlblUpdateSucess).innerHTML = "";
            $get(_clientlblError).innerText = "";
            $get(_clientlblError).innerHTML = "";
        }

        function ClearControls() {
            ClearMessages();
            $get(_clienthidMode).value = "New";
            $get(_clientvalSumErrorMsg).style.display = "none";
            $get(_clientcmbBankName).value = "0";
            $get(_clienttxtAlias).value = "";
            $get(_clienttxtAcNo).value = ""
            $get(_clienttxtAmt).value = "";
            $get(_clienttxtBankAddress).value = "";
            $get(_clientcmbDebit).value = 0;
            $get(_clienttxtAcNo).disabled = false;
            $get(_clientcmbBankName).disabled = false;
            $get(_clientchkOnlineTransactions).checked = false;
            $get(_clientchkOnlineTransactions).onclick = '';
            $get(_clientchkIsDefault).checked = false;
            $get(_clientchkIsInternalDefault).checked = false;
            $get(_clienthidLedgerId).value = "0";
            var btnSave = $get(_clientbtnSave);
            btnSave.value = "Save";

            return true;
        }

        function ValidateAddress(source, args) {
            var txtBankAddress = $get(_clienttxtBankAddress).value;
            args.IsValid = true;
            if (txtBankAddress.trim() != "" && txtBankAddress.length > 200) {
                args.IsValid = false;
                $get(_clientcstValAddress).errormessage = "Length of address should not exceed 200 characters.";
            }
            return !args.IsValid;
        }

        function ConfirmRemove() {
            return window.confirm('Are you sure you want to delete this record?');
        }

        function WarnIsForOnlineBank() {
            alert("This Bank is designated for Online transactions.\nPlease designate another Bank for Online transactions before deleting this Bank.");
        }

        function WarnOnOnlineTransactionCheck() {
            alert("This Bank is designated for Online transactions.\nIf you wish to designate another Bank for Online transactions,\nplease edit that bank and select the 'Is For Online Transactions' option.");
        }

        function Confirm() {     
            ClearMessages();

            var LedgerId = $get(_clienthidLedgerId).value;
            var duplicate = false;
            var Internalduplicate = false;
            var IsDefault = $get(_clientchkIsDefault).checked;
            var IsInternalDefault = $get(_clientchkIsInternalDefault).checked;
            var iRowCount = 0;
            var Id = $get(_clientlstvwBankDetails + "_ctrl" + iRowCount + "_hidId");
            var Is_Default = $get(_clientlstvwBankDetails + "_ctrl" + iRowCount + "_hidIsDefault");            
            while (IsDefault != null && IsDefault == true && Id != null && $get(_clientcmbBankName).value != "0") {
                if (LedgerId != Id.value && Is_Default != null && Is_Default.value == 'True') {
                    duplicate = true;
                    break;
                }
                iRowCount = iRowCount + 1;
                Id = $get(_clientlstvwBankDetails + "_ctrl" + iRowCount + "_hidId");
                Is_Default = $get(_clientlstvwBankDetails + "_ctrl" + iRowCount + "_hidIsDefault");
            }
            if (IsInternalDefault != null) {
                var iRowCount = 0;
                var Id = $get(_clientlstvwBankDetails + "_ctrl" + iRowCount + "_hidId");
                var Is_Internal_Default = $get(_clientlstvwBankDetails + "_ctrl" + iRowCount + "_hidIsInternalDefault");
                while (IsInternalDefault != null && IsInternalDefault == true && Id != null && $get(_clientcmbBankName).value != "0") {
                    if (LedgerId != Id.value && Is_Internal_Default != null && Is_Internal_Default.value == 'True') {
                        Internalduplicate = true;
                        break;
                    }
                    iRowCount = iRowCount + 1;
                    Id = $get(_clientlstvwBankDetails + "_ctrl" + iRowCount + "_hidId");
                    Is_Internal_Default = $get(_clientlstvwBankDetails + "_ctrl" + iRowCount + "_hidIsInternalDefault");
                }
            }

            if (duplicate) {
                return (window.confirm('Do you want to set the current bank as School Fee default bank? This will remove the existing School Fee default bank. Are you sure you want to continue?'));
            }
            else if (Internalduplicate) {
                return (window.confirm('Do you want to set the current bank as Internal Fee default bank? This will remove the existing Internal Fee default bank. Are you sure you want to continue?'));
            }
            else
                return true;
        }

    </script>
</asp:Content>
