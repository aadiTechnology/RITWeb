<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ExternalPOPaymentDetailsUI.aspx.cs" Inherits="ExternalPOPaymentDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="98%">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left" style="width: 50%">
                                    <asp:ValidationSummary ID="ValSum" runat="server" CssClass="ClsMdtStar" />
                                </td>
                                <td align="right" valign="top">
                                    <span class="ClsMdtStar">* Mandatory Field</span>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="optLstModes" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblMessage" runat="server" CssClass="ClsLbl" Style="float: inherit;"
                                        Text="" EnableViewState="false" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight" style="width: 150px">
                                    <span id="spnPONo" runat="server" class="ClsLabel">PO No. : </span>
                                </td>
                                <td align="left" class="ClsHilightBGB">
                                    <asp:Label ID="lblPONo" runat="server" Text="" CssClass="clsLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight" style="width: 150px">
                                    <span class="ClsLabel">Payment Mode : </span>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="optLstModes" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="True" OnSelectedIndexChanged="optLstModes_SelectedIndexChanged">
                                    </asp:RadioButtonList>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Payment Mode should be selected."
                                        Display="None" ClientValidationFunction="ValidatePaymentMode"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Payment Date : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPaymentDate" CssClass="MidTxtBox" runat="server" MaxLength="20"
                                        ReadOnly="true" />
                                    <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtPaymentDate" Format="dd MMM yyyy"
                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Payment date should not be blank."
                                        AutoPostBack="False" To-Today="true" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Payment Date should not be blank."
                                        ControlToValidate="txtPaymentDate" Display="None"></asp:RequiredFieldValidator>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Total Amount : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtTotalAmount" CssClass="SmlTxtBox" runat="server" MaxLength="7"
                                        ReadOnly="true" Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Pending Amount : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPendingAmount" CssClass="SmlTxtBox" runat="server" MaxLength="7"
                                        ReadOnly="true" Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Payable Amount : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPayableAmount" CssClass="SmlTxtBox" runat="server" MaxLength="7"
                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Payable Amount should not be blank."
                                        ControlToValidate="txtPayableAmount" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" Display="None" ErrorMessage="Payable Amount should not be greater than Pending Amount."
                                        ControlToCompare="txtPendingAmount" ControlToValidate="txtPayableAmount" Operator="LessThanEqual"
                                        Type="Double"></asp:CompareValidator>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" Display="None" ErrorMessage="Payable Amount should be greater than zero."
                                        ControlToValidate="txtPayableAmount" Operator="NotEqual" Type="Double" ValueToCompare="0"></asp:CompareValidator>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Remark : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtRemark" CssClass="ExLrgTxtBox" runat="server" TextMode="MultiLine"
                                        MaxLength="500" Height="75px" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ViewStateMode="Enabled"
                                        Display="None" ControlToValidate="txtRemark" ErrorMessage="Length of Remarks should not exceed 500 characters."
                                        CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
                                    <%--<span class="ClsMdtStar">* </span>--%>
                                </td>
                            </tr>
                            <tr id="trChequeNo" runat="server" visible="true">
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Cheque Number : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtChequeNumber" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                        Style="text-align: left; padding-right: 5px" onblur="extractNumber(this,0,false);"
                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                        onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Cheque Number should not be blank."
                                        ControlToValidate="txtChequeNumber" Display="None"></asp:RequiredFieldValidator>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr id="trTxnNo" runat="server" visible="false">
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Transaction Number : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtTxnNumber" CssClass="LrgTxtBox" runat="server" MaxLength="20" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Transaction Number should not be blank."
                                        ControlToValidate="txtTxnNumber" Display="None"></asp:RequiredFieldValidator>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr id="trChequeDate" runat="server" visible="true">
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Cheque Date : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtChequeDate" CssClass="MidTxtBox" runat="server" ReadOnly="true" />
                                    <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtChequeDate" Format="dd MMM yyyy"
                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Cheque Date should not be blank."
                                        AutoPostBack="False" To-Today="true" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Cheque Date should not be blank."
                                        ControlToValidate="txtChequeDate" Display="None"></asp:RequiredFieldValidator>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr id="trType" runat="server" visible="false">
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Type : </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbTypes" runat="server" CssClass="LrgCombo">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Type should be selected."
                                        ControlToValidate="cmbTypes" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel">Bank Name : </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbBanks" runat="server" CssClass="LrgCombo">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Bank should be selected."
                                        ControlToValidate="cmbBanks" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None"
                                        ClientValidationFunction="ValidateDuplication"></asp:CustomValidator>
                                    <span class="ClsMdtStar">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="False"
                                        OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="optLstModes" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="80%">
                            <tr>
                                <td align="center">
                                    <asp:ListView ID="lstvwPayments" runat="server" DataKeyNames="Id" OnItemCommand="lstvwPayments_ItemCommand"
                                        OnItemDataBound="lstvwPayments_ItemDataBound">
                                        <LayoutTemplate>
                                            <table width="90%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="left" class="clsLabelgrd" width="150px">
                                                        <asp:Label ID="Label1" runat="server" Text="Payment Mode"> </asp:Label>
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="100px">
                                                        <asp:Label ID="Label2" runat="server" Text="Payment Date"> </asp:Label>
                                                    </th>
                                                    <th align="right" width="100px" class="clsLabelgrd">
                                                        <asp:Label ID="Label3" runat="server" Text="Amount"> </asp:Label>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd" width="200px">
                                                        <asp:Label ID="Label4" runat="server" Text="Cheque / Txn No."> </asp:Label>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd" width="150px">
                                                        <asp:Label ID="Label5" runat="server" Text="Type"> </asp:Label>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd" width="">
                                                        <asp:Label ID="Label6" runat="server" Text="Bank Name"> </asp:Label>
                                                    </th>
                                                    <th width="50px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                    </th>
                                                    <th width="50px" class="clsLabelgrd" align="center">
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
                                                    <asp:Label ID="lblPaymentMode" runat="server" CssClass="ClsLabel" Text='<%#Eval("PaymentMode") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidCurrentId" runat="server" Value='<%#Eval("Id") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblPaymentDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label8" runat="server" CssClass="ClsLabel" Text='<%#Eval("Amount") %>'
                                                        Style="float: right"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblTxnNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("TxnNo") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label10" runat="server" CssClass="ClsLabel" Text='<%#Eval("Type") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblBankName" runat="server" CssClass="ClsLabel" Text='<%#Eval("BankName") %>'></asp:Label>
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
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td class="LblNoRecord" align="center">
                                                    <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hidPOMasterId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidId" runat="server" Value="0" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="False" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        _clientoptLstModes = '<%=this.optLstModes.ClientID %>'
        function ValidatePaymentMode(src, args) {
            if ($('[id*=_optLstModes_]:checked').length == 0) {
                args.IsValid = false
                return true
            }
            args.IsValid = true;
            return false;
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete record?');
        }

        function ResetLabel() {
            $('#' + '<%=this.lblMessage.ClientID %>').html('')
        }


        function ValidateDuplication(src, args) {

            var selectedValue = $('[id*=optLstModes]').filter(function () {
                return $(this).is(":checked");
            }).val();

            var txnno = ''
            var msg = ''
            if (selectedValue == 2) {
                txnno = $('#' + '<%=this.txtChequeNumber.ClientID %>').val()
                msg = 'Cheque Number should not be duplicate for selected bank.'
            }
            else {
                txnno = $('#' + '<%=this.txtTxnNumber.ClientID %>').val()
                msg = 'Transaction Number should not be duplicate for selected bank.'
            }

            var bankName = $("#" + "<%=this.cmbBanks.ClientID %>" + " option:selected").text();

            var id = $('#' + '<%=this.hidId.ClientID %>').val()

            var data = $("[id$=lblTxnNo]").filter(function () { return $(this).text() == txnno })

            var isFound = false;
            if (data.length > 0) {
                data.each(function () {
                    var newBankNm = $('#' + this.id.replace('_lblTxnNo', '_lblBankName')).text()
                    var newId = $('#' + this.id.replace('_lblTxnNo', '_hidCurrentId')).val()

                    if (id != newId && bankName == newBankNm) {
                        isFound = true;
                    }
                })
            }

            if (isFound) {
                src.errormessage = msg;

                args.IsValid = false
                return true;
            }
            else {
                args.IsValid = true
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
