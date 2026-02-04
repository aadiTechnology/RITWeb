<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SurveySendSMSPopup.aspx.cs" Inherits="SurveySendSMSPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                        <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                            <tr>
                                                <td align="left" class="MainTitleHead" style="height: 20px">
                                                    <span style="font-weight: bold">Send SMS</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="Height10">
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                            ControlToValidate="cmbCategroryGroup" ErrorMessage="Category should be selected."
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ErrorMessage="At least one standard should be selected."
                                            ClientValidationFunction="ValidateStandard"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None"
                                            ControlToValidate="txtSMSText" ErrorMessage="SMS Text should not be blank."></asp:RequiredFieldValidator>
                                        <div style="float: right; vertical-align: top;">
                                            <span class="ClsMdtStar">*</span>
                                            <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="center" colspan="3" id="tdMessage" runat="server">
                                                    <asp:Label ID="lblMessage" runat="server" Text="" Style="font-size: 12px;" EnableViewState="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="150px">
                                                    <span class="ClsLabel">Category :</span>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:DropDownList ID="cmbCategroryGroup" Width="200px" runat="server" CssClass="LrgCombo"
                                                        onchange="ClearMessage()">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <asp:Label ID="Label6" runat="server" Text="Applicable to :" CssClass="ClsLabel"
                                                        Style="white-space: nowrap;" EnableViewState="False"></asp:Label><br>
                                                    <asp:CheckBox ID="chkAll" runat="server" Text="Select All" Style="white-space: nowrap;
                                                        padding-right: 08px" onclick="CheckAllUncheckAlls()" />
                                                </td>
                                                <td align="left" valign="top" colspan="2">
                                                    <asp:CheckBoxList ID="chkListStandards" runat="server" CellPadding="0" CellSpacing="0"
                                                        CssClass="ClsLabel" RepeatColumns="3" RepeatDirection="Horizontal" Width="90%">
                                                    </asp:CheckBoxList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">SMS Text :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSMSText" runat="server" TextMode="MultiLine" MaxLength="320"
                                                        Width="350px" Height="100px" onkeyup="countChar(this)" Columns="3" />
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td align="left" width="100px">
                                                    <asp:Label ID="lblCharNum" runat="server" Text="" class="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">SMS Count :</span>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblSMSCount" runat="server" Text="" class="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="Height10">
                                                <td colspan="3" align="center">
                                                    <asp:Button ID="btnSendSMS" CssClass="ClsBtn" runat="server" Text="Send SMS" OnClick="btnSendSMS_Click"
                                                        OnClientClick="if(!ConfirmSMS()) return false;" />
                                                    <asp:Button ID="btnClose" CssClass="ClsBtn" runat="server" Text="Close" OnClientClick="window.close()"
                                                        CausesValidation="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientchkAllText = "<%=this.chkAll.ClientID %>";
        _clientchkListStandards = "<%=this.chkListStandards.ClientID %>";
        _clienttxtSMSText = "<%=this.txtSMSText.ClientID %>";
        _clientlblCharNum = "<%=this.lblCharNum.ClientID %>"
        _clientlblSMSCount = "<%=this.lblSMSCount.ClientID %>"
        _clientbtnSendSMS = "<%=this.btnSendSMS.ClientID %>"
        _clientbtnClose = "<%=this.btnClose.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)

        function EndReqHandler(sender, args) {
            DisableButtons(false)
            countChar($get(_clienttxtSMSText))
        }
        function beginRequestHandler(sender, args) {
            DisableButtons(true)
        }

        function DisableButtons(flag) {
            $get(_clientbtnSendSMS).disabled = flag
            $get(_clientbtnClose).disabled = flag
        }

        function countChar(val) {
            var len = val.value.length;
            if (len > 320) {
                val.value = "(" + val.value.substring(0, 320) + ")";
            } else {
                $('#' + _clientlblCharNum).text("(" + len + ")");

                var cnt;
                if (len == 160)
                    $('#' + _clientlblSMSCount).text(1)
                else {
                    cnt = parseInt((len / 160)) + 1;
                    $('#' + _clientlblSMSCount).text(cnt)
                }
            }
        };


        function CheckAllUncheckAlls() {
            var checkAll;
            if (document.getElementById(_clientchkAllText) != null)
                checkAll = document.getElementById(_clientchkAllText).checked

            var iRowCount = 0
            var chk = document.getElementById(_clientchkListStandards + "_" + iRowCount)
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientchkListStandards + "_" + iRowCount);
            }
        }


        function ValidateStandard(oSrc, args) {
            var j = 0
            var abresult = true;
            var checks = document.forms[0].elements
            var boxLength = checks.length

            for (i = 0; i < boxLength; i++) {
                if ((checks[i].type == 'checkbox' && checks[i].id.match("chkListStandards_") != null)) {
                    if (checks[i].checked == true) {
                        j++
                    }
                }
            }
            if (j == 0) {
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ClearMessage() {
            $('#' + "<%=this.lblMessage.ClientID %>").text("");
        }

        function ConfirmSMS() {
            ClearMessage();
            var result = true;
            if (typeof (Page_ClientValidate) == 'function')
                result = Page_ClientValidate("");

            if (result)
                return confirm('Are you sure you want to send SMS?')
        }

    </script>
</asp:Content>
