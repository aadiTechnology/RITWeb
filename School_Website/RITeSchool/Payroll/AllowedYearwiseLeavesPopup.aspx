<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="AllowedYearwiseLeavesPopup.aspx.cs" Inherits="AllowedYearwiseLeavesPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td id="MainDataTable" align="center">
                <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                    <tr>
                        <td class="ClsGrayMainTitle" align="left">
                            <span class="MainTitleHead">Configure Yearwise Leaves</span>
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
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table align="center" width="85%">
                    <tr id="trYear" runat="server">
                        <td align="left" valign="middle" class="ClsBorderlight" style="padding-left: 10px">
                            <span class="ClsLabel">Year : </span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbYear" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                OnSelectedIndexChanged="cmbYear_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">&nbsp *</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight" style="padding-left: 10px">
                            <asp:Label ID="lblHeaderUserName" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                Text="Name : "></asp:Label>
                        </td>
                        <td id="tdUserName" class="ClsHilightBGB" style="width: 50%" align="left" runat="server">
                            <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Width="76%"></asp:Label>
                        </td>
                    </tr>                    
                </table>
            </td>
        </tr>
        <!-- User InfoTable ListView -->
        <tr>
            <td align="center">
                <table width="90%">
                    <tr id="trMessage" runat="server">
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblWarningMessage" runat="server" CssClass="ClsHilightErrorB" EnableViewState="False"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr style="width: 100%">
                        <td style="width: auto" align="center">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divContainer" class="GridBorder" runat="server" style="width: 500px; height: 200px;
                                        overflow: scroll">
                                        <asp:ListView ID="lstvwLeave" runat="server" DataKeyNames="LeaveId,UserLeavesYearwiseConfigurationId"
                                            OnItemDataBound="lstvwLeave_ItemDataBound">
                                            <LayoutTemplate>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height: 10px" runat="server" id="divErr" visible="false">
            <td align="center">
                <table align="center">
                    <tr>
                        <td>
                            <div>
                                <table class="LblNoRecord" cellspacing="0" cellpadding="0" width="100%">
                                    <tr>
                                        <td class="ClsConfigText">
                                            Please configure following details for User :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Staff Leaves
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trBasicLEaveMsg" runat="server" visible = "false">
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divBasicLeave" runat="server">
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">                
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" BorderWidth="1px" 
                            CausesValidation="True" disable-page="true" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                            CausesValidation="False" UseSubmitBehavior="False" />
                            <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                        <asp:HiddenField ID="hidIsSaveButtonClick" runat="server" Value="N"></asp:HiddenField>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:HiddenField ID="hidIsConfigured" runat="server" />
                <asp:HiddenField ID="hidUserId" runat="server" />
                <asp:HiddenField ID="hidUserRoleId" runat="server" />
                <asp:HiddenField ID="hidStaffGroupId" runat="server" Value="0" />
                <asp:HiddenField ID="hidStaffGroupName" runat="server" Value="" />
                <asp:HiddenField ID="hidUserName" runat="server" Value="" />
                <asp:HiddenField ID="hidRecordCount" runat="server" />
                <asp:HiddenField ID="hidDisplayMessage" runat="server" Value="N" />
                <asp:HiddenField ID="hidApplyToAllUsersOfStaffGroup" runat="server" Value="N" />
                <asp:HiddenField ID="hidIsLeapYear" runat="server" Value="N" />
                <asp:HiddenField ID="hidTxtValue" runat="server" />
                <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                <asp:CustomValidator ID="cstLeave" runat="server" ClientValidationFunction="ValidateLeave"
                    SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                <asp:CustomValidator ID="cstNegativeLeaveValidaion" runat="server" ClientValidationFunction="ValidateNegativeLeave"
                    SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        _clientlstvwLeave = "<%=this.lstvwLeave.ClientID %>"
        _ClientcstLeave = "<%=this.cstLeave.ClientID %>"
        _clienthidRecordCount = "<%=this.hidRecordCount.ClientID %>"
        _clienthidApplyToAllUsersOfStaffGroup = "<%=this.hidApplyToAllUsersOfStaffGroup.ClientID %>"
        _clienthidIsLeapYear = "<%=this.hidIsLeapYear.ClientID %>"
        _clienthidTxtValue = "<%=this.hidTxtValue.ClientID %>"        
        _clientcstNegativeLeaveValidaion = "<%=this.cstNegativeLeaveValidaion.ClientID %>"
        _clientBtnSave = "<%=this.btnSave.ClientID %>"


        function GetValue(txt) {
            document.getElementById(_clienthidTxtValue).value = txt.value
        }

        function Validate(textbox, MaxVal) {
            if (MaxVal == "True")
                var iDaysOfYear = 366
            else
                var iDaysOfYear = 365
            var sMarks = textbox.value
            var iMarks = parseInt(sMarks)
            if (sMarks == "" || iMarks > iDaysOfYear) {
                textbox.value = document.getElementById(_clienthidTxtValue).value
                textbox.focus()
            }
            else {
                var floatValue = parseFloat(textbox.value)
                var intValue = parseInt(textbox.value)
                intValue = parseFloat(intValue)
                var difference = parseFloat((floatValue * 10) % 10)
                if (difference != 5 && difference != 0) {
                    if (difference > 5)
                        difference = intValue + 1
                    else
                        difference = intValue + 0.5
                    textbox.value = difference
                }
            }
        }

        function ValidateLeave(oSrc, args) {
            var iRowCount = 0
            var sLeave = ""
            var sDays = ""
            var sAvailable = ""
            var sMessage = ""
            var iAvailable = 0
            var sAvailableLeaves = ""
            if ($get(_clienthidIsLeapYear).value == "True")
                var iDaysOfYear = 366
            else
                var iDaysOfYear = 365
            var txtLeaveValue = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_txtLeave")
            var txtAvailableLeaveValue = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_txtLeaveBalance")
            var iLeaveDays = 0
            while (txtLeaveValue != null) {
                var sLeaveName = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_lblLeaveName").innerHTML
                if (txtLeaveValue.value.trim() == "")
                    sLeave = sLeave + "," + sLeaveName
                else {
                    var iLeaveDays = iLeaveDays + parseFloat(txtLeaveValue.value.trim())
                    if (txtAvailableLeaveValue != null)
                        var iAvailable = iAvailable + parseFloat(txtAvailableLeaveValue.value.trim())
                }
                if (txtAvailableLeaveValue != null && txtAvailableLeaveValue.value.trim() == "")
                    sAvailableLeaves = sAvailableLeaves + "," + sLeaveName
                iRowCount = iRowCount + 1
                txtLeaveValue = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_txtLeave")
                txtAvailableLeaveValue = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_txtLeaveBalance")
            }
            if (parseInt(iLeaveDays) > parseInt(iDaysOfYear))
                sDays = sDays + "," + sLeaveName
            if (parseInt(iAvailable) > parseInt(iDaysOfYear))
                sAvailable = sAvailable + "," + sLeaveName
            if (sLeave != "") {
                sLeave = sLeave.substring(1)
                $get(_ClientcstLeave).errormessage = "Allowed leaves should not be blank or zero for  :" + sLeave
                args.IsValid = false
                return true
            }
            else if (sDays != "" || sAvailable != "" || iLeaveDays > iDaysOfYear || iAvailable > iDaysOfYear) {
                sDays = sDays.substring(1)
                $get(_ClientcstLeave).errormessage = "Total of leave days should not be greater than total days of year."
                args.IsValid = false
                return true
            }
            else if (sAvailableLeaves != "") {
                sAvailableLeaves = sAvailableLeaves.substring(1)
                $get(_ClientcstLeave).errormessage = "Available leaves should not be blank for  :" + sAvailableLeaves
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateNegativeLeave(oSrc, args) {
            var iRowCount = 0
            var sLeave = ""
            var sDays = ""
            var sAvailable = ""
            var sMessage = ""
            var iAvailable = 0
            var sAvailableLeaves = ""
            if ($get(_clienthidIsLeapYear).value == "True")
                var iDaysOfYear = 366
            else
                var iDaysOfYear = 365
            var txtLeaveValue = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_txtLeave")
            var txtAvailableLeaveValue = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_txtLeaveBalance")
            var iLeaveDays = 0
            while (txtLeaveValue != null) {
                var sLeaveName = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_lblLeaveName").innerHTML
                if ((txtLeaveValue.value.trim() != "" && parseFloat(txtLeaveValue.value.trim()) < 0) || (txtAvailableLeaveValue.value.trim() != "" && parseFloat(txtAvailableLeaveValue.value.trim()) < 0))
                    sLeave = sLeave + "," + sLeaveName

                iRowCount = iRowCount + 1
                txtLeaveValue = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_txtLeave")
                txtAvailableLeaveValue = document.getElementById(_clientlstvwLeave + "_ctrl" + iRowCount + "_txtLeaveBalance")
            }

            if (sLeave != "") {
                $get(_clientcstNegativeLeaveValidaion).errormessage = "Basic Leaves and Leave Balance should not be negative.";
                $get(_clientcstNegativeLeaveValidaion).innerHTML = "Basic Leaves and Leave Balance should not be negative.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function SetInitStatus() {
            document.getElementById("<%=this.hidIsSaveButtonClick.ClientID %>").value = "Y";
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        function EndRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientBtnSave) {
                var QueryString = $get("<%=this.hidQueryString.ClientID %>").value
                window.opener.location=window.opener.location.pathname+QueryString
                window.close();
                window.opener.focus();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
