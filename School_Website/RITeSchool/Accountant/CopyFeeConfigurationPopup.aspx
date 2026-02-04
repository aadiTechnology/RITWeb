<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="CopyFeeConfigurationPopup.aspx.cs" Inherits="CopyFeeConfigurationPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td align="left" colspan="6" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True"> Copy Fee </asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="6" style="color: #ff3333" valign="top">
                    <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="6" style="margin-left: 40px">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="Please fix following error(s)"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 10%;" class="ClspaddingR">
                    <span class="ClsLblLgnd" id="lblStdDivName">Fee Type :</span>&nbsp;
                </td>
                <td align="left" style="width: 20%" class="ClsHilightBGB">
                    <asp:Label ID="lblFeeType" runat="server" CssClass="LblNrmlB"></asp:Label>
                </td>
                <td style="width: 5%">
                </td>
                <td align="left" style="width: 10%;" class="ClspaddingR">
                    <span class="ClsLblLgnd" id="Span1">Payable For :</span>&nbsp;
                </td>
                <td align="left" style="width: 20%" class="ClsHilightBGB">
                    <asp:Label ID="lblPayableFor" runat="server" CssClass="LblNrmlB"></asp:Label>
                </td>
                <td style="width: 5%">
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 10%;" class="ClspaddingR">
                    <span class="ClsLblLgnd" id="Span2">Due Date :</span>&nbsp;
                </td>
                <td align="left" style="width: 20%" class="ClsHilightBGB">
                    <asp:Label ID="lblPaidDate" runat="server" CssClass="LblNrmlB"></asp:Label>
                </td>
                <td style="width: 5%">
                </td>
                <td align="left" style="width: 10%;" class="ClspaddingR">
                    <span class="ClsLblLgnd" id="Span3">Amount :</span>&nbsp;
                </td>
                <td align="left" style="width: 20%" class="ClsHilightBGB">
                    <asp:Label ID="lblAmount" runat="server" CssClass="LblNrmlB"></asp:Label>
                </td>
                <td style="width: 5%">
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 10%;" class="ClspaddingR">
                    <span class="ClsLblLgnd" id="Span4">Remarks :</span>&nbsp;
                </td>
                <td align="left" class="ClsHilightBGB" colspan="4">
                    <asp:Label ID="lblRemarks" runat="server" CssClass="LblNrmlB"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="4" class="ClsBorderlight">
                    <table align="left">
                        <tr>
                            <td>
                                <span class="ClsLabel" style="vertical-align: middle">Send SMS :</span>
                            </td>
                            <td>
                                <span class="ClsLabel" style="vertical-align: top">
                                    <asp:CheckBox ID="chkSendSMS" runat="server" OnCheckedChanged="chkSendSMS_CheckedChanged"
                                        AutoPostBack="true" TabIndex="1" />
                                </span>
                            </td>
                        </tr>
                    </table>
                    <table align="left">
                        <tr>
                            <td>
                                <span class="ClsLabel" style="vertical-align: middle">Send Message :</span>
                            </td>
                            <td>
                                <span class="ClsLabel" style="vertical-align: middle">
                                    <asp:CheckBox ID="chkSendMessage" runat="server" OnCheckedChanged="chkSendMessage_CheckedChanged"
                                        AutoPostBack="true" TabIndex="2" />
                                </span>
                            </td>
                        </tr>
                    </table>
                    <table align="left">
                        <tr>
                            <td>
                                <span class="ClsLabel" style="vertical-align: middle">Consider For RTE Concession? :</span>
                            </td>
                            <td>
                                <span class="ClsLabel" style="vertical-align: middle">
                                    <asp:CheckBox ID="chkRTEStudent" runat="server" TabIndex="3" />
                                </span>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidSendMsg" runat="server" />
                    <asp:HiddenField ID="hidSendSms" runat="server" />
                </td>
            </tr>
            <tr id="trNote1" runat="server">
                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                    <span class="LblNrmlB" style="font-weight: bold">Note 1 :</span>
                </td>
                <td align="left" class="ClsBorderlight" style="padding-left: 5px;" colspan="4">
                    <asp:Label ID="lblVerifyNote1" runat="server" BorderWidth="0px" CssClass="LblSmlV">
                    Once the fee is copied to the standard, can not be copied again. 
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:GridView CssClass="GridBorder" ID="grdStandards" runat="server" Width="100%"
                        AutoGenerateColumns="False" Height="43px" PageSize="20" AllowPaging="False" CellPadding="0"
                        CellSpacing="1" ForeColor="#333333" GridLines="None" DataKeyNames="Standard_Id"
                        OnRowDataBound="grdStandards_RowDataBound" OnDataBound="grdStandards_DataBound">
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                        </PagerStyle>
                        <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                            FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="ChkAllCopy" type="checkbox" runat="server" style="margin-left: 2px" onclick="CheckOrUncheckAllChkBoxForStudent()"
                                        tabindex="3" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkBoxCopy" runat="server" TabIndex="4" />
                                </ItemTemplate>
                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                                <HeaderStyle Width="30px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Standard Name" DataField="Standard_Name">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="92%" CssClass="ClspaddingL" />
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="92%" CssClass="ClsPaddingL" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle CssClass="ClsGridRow" />
                        <HeaderStyle CssClass="ClsGridHeader" />
                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="ClsBtn" OnClick="btnCopy_Click"
                        TabIndex="5" />
                    <asp:Button ID="btnClose" Text="Close" CssClass="ClsBtn" runat="server" CausesValidation="false"
                        OnClick="btnClose_Click" TabIndex="6" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidStandardID" runat="server" />
                    <asp:HiddenField ID="hidSerialNumber" runat="server" />
                    <asp:HiddenField ID="hidDebitID" runat="server" />
                    <asp:HiddenField ID="hidIsInternalFee" runat="server" />
                    <asp:HiddenField ID="hidStandardList" runat="server" />
                    <asp:HiddenField ID="hidStandardIDList" runat="server" />
                    <asp:HiddenField ID="hidRowCnt" runat="server" />
                    <asp:HiddenField ID="hidCanOverWrite" runat="server" />
                    <asp:HiddenField ID="hidSelectedStdList" runat="server" />
                    <asp:HiddenField ID="hidAccountHeaderId" runat="server" />  
                    <asp:HiddenField ID="hidIsForInternalFee" runat="server" />                    
                    <asp:HiddenField ID="hidIsDueDateApplicable" runat="server" Value="N" />
                    <asp:HiddenField ID="hidIsOnlinePaymentApplicable" runat="server" Value="N" />
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientGridId = "<%=this.grdStandards.ClientID %>"
        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _clienthidStandardList = "<%=this.hidStandardList.ClientID %>"
        _clienthidCanOverWrite = "<%=this.hidCanOverWrite.ClientID %>"
        _clientlblFeeType = "<%=this.lblFeeType.ClientID %>"
        _clientchkSendSMS = "<%=this.chkSendSMS.ClientID %>"
        _clienthidSendSms = "<%=this.hidSendSms.ClientID %>"
        _clientchkSendMsg = "<%=this.chkSendMessage.ClientID %>"
        _clienthidSendMsg = "<%=this.hidSendMsg.ClientID %>"

        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxCopy', sActionName, 'false', iPageCount, 'true')) {
                var iRowCnt = document.getElementById(_clienthidRowCnt).value
                var sStandardlist = "";
                for (i = 1; i <= iRowCnt; i++) {
                    if (i < 9) {
                        sRow = "_ctl0" + (i + 1) + "_ChkBoxCopy"
                        var chk = document.getElementById(_clientGridId + sRow)
                        if (chk.checked) {
                            var StandardName = document.getElementById(_clientGridId).rows[i].cells[1].innerHTML
                            if ((document.getElementById(_clienthidStandardList).value).match(StandardName)) {
                                sStandardlist += StandardName + " , "
                            }
                        }
                    }
                    else {
                        sRow = "_ctl" + (i + 1) + "_ChkBoxCopy"
                        var chk = document.getElementById(_clientGridId + sRow)
                        if (chk.checked) {
                            var StandardName = document.getElementById(_clientGridId).rows[i].cells[1].innerHTML
                            if ((document.getElementById(_clienthidStandardList).value).match(StandardName)) {
                                sStandardlist += StandardName + " , "
                            }
                        }
                    }
                }
                if (sStandardlist != "") {
                    sStandardlist = sStandardlist.substring(0, (sStandardlist.lastIndexOf(" , ")))
                    if (window.confirm('You already have same fee type and payable for the standards ' + sStandardlist + '. You can not copy configuration.'))
                        document.getElementById(_clienthidCanOverWrite).value = true.toString()
                }
                else
                    document.getElementById(_clienthidCanOverWrite).value = false.toString()
            }
            else
            { bResult = false; }
            return bResult
        }
        function ConfirmCopy(iPageCount, sActionName) {
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxCopy', sActionName, 'false', iPageCount, 'true')) {
                var feetype = document.getElementById(_clientlblFeeType).innerHTML;
                var msg = "Are you sure you want to copy '" + feetype + "' fee to the selected standard(s)?"
                if (!window.confirm(msg)) {
                    bResult = false
                }
                else {
                    bResult = ConfirmSendMessage(iPageCount, sActionName);
                }
            }
            else
                bResult = false

            return bResult
        }

        function ConfirmSendMessage() {

            var bResult = true

            var chkSendSMS = document.getElementById(_clientchkSendSMS)
            var SendSms = document.getElementById(_clienthidSendSms)

            var chkSendMsg = document.getElementById(_clientchkSendMsg)
            var SendMsg = document.getElementById(_clienthidSendMsg)

            if (chkSendSMS.checked == true && chkSendMsg.checked == true) {
                if (!window.confirm('Do you want to send SMS and Message to students of selected standard(s)?')) {
                    SendSms.value = "N";
                    SendMsg.value = "N";
                }
                else {
                    SendSms.value = "Y";
                    SendMsg.value = "Y";
                }
            }
            else if (chkSendSMS.checked == true) {
                if (!window.confirm('Do you want to send SMS to selected Standard(s).')) {
                    SendSms.value = "N";
                }
                else {
                    SendSms.value = "Y";
                }
            }
            else if (chkSendMsg.checked == true) {
                if (!window.confirm('Do you want to send Message to selected Standard(s).')) {
                    SendMsg.value = "N";
                }
                else {
                    SendMsg.value = "Y";
                }
            }

            return bResult;
        }

        function CheckOrUncheckAllChkBoxForStudent() {
            var ChkBox = document.getElementById(_clientGridId + "_ctl01_ChkAllCopy")
            var iRowCnt = document.getElementById(_clientGridId).rows.length - 1
            if (ChkBox.checked) {
                for (i = 0; i < iRowCnt; i++) {
                    if (i < 8) {
                        sRow = "_ctl0" + (i + 2) + "_ChkBoxCopy"
                    }
                    else {
                        sRow = "_ctl" + (i + 2) + "_ChkBoxCopy"
                    }
                    var ChkBox1 = document.getElementById(_clientGridId + sRow)
                    if (ChkBox1 != null) {
                        ChkBox1.checked = true
                    }
                }
            }
            else {
                for (i = 0; i < iRowCnt; i++) {
                    if (i < 8) {
                        sRow = "_ctl0" + (i + 2) + "_ChkBoxCopy"
                    }
                    else {
                        sRow = "_ctl" + (i + 2) + "_ChkBoxCopy"
                    }
                    var ChkBox1 = document.getElementById(_clientGridId + sRow)
                    if (ChkBox1 != null) {
                        ChkBox1.checked = false
                    }
                }
            }
        }
    </script>
</asp:Content>
