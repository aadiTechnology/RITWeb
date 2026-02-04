<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubjectSectionConfigurationUI.aspx.cs"
    Inherits="SubjectSectionConfigurationUI" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table id="tblSubjectSection" runat="server" border="0" cellpadding="0" cellspacing="2"
                            style="height: 100%; width: 100%;">
                            <tr>
                                <td style="width: 77%">
                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                            Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                    </asp:Panel>
                                </td>
                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                    <span class="ClsMdtStar">*
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlCheckdepandency" runat="server" Width="96%">
                                        <asp:Label ID="lblCheckDependency" Visible="true" Style="text-align: left" runat="server"
                                            ForeColor="Red" Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                        CssClass="NewClsLabel" ShowSummary="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <table>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Visible="true" EnableViewState="False"
                                                    CssClass="ClsLabel" Width="100%" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, StandardName %>"></asp:Label>
                                                    <span id="Span2" class="colonPadding">:</span> </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbStandards" runat="server" AutoPostBack="true" Width="100px"
                                                    OnSelectedIndexChanged="cmbStandards_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqvalStandardName" runat="server" ControlToValidate="cmbStandards"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, StandardNameShouldBeSelected %>"
                                                    InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, SubjectName %>"></asp:Label>
                                                    <span id="Span1" class="colonPadding">:</span> </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbSubjects" runat="server" AutoPostBack="true" Width="100px"
                                                    OnSelectedIndexChanged="cmbSubjects_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqcmbSubjects" runat="server" ControlToValidate="cmbSubjects"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SubjectNameShouldBeSelected %>"
                                                    InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table align="center" width="50%">
                            <tr>
                                <td>
                                    <div id="divSubjectSectionDetails" class="GridBorder" runat="server" visible="false"
                                        style="overflow: auto;">
                                        <asp:ListView ID="lstvwSubjectSectionDetails" runat="server" DataKeyNames="SubjectSectionConfigurationId,OrginalSubjectSectionId,Is_Deleted,SortOrder"
                                            OnItemDataBound="lstvwSubjectSectionDetails_ItemDataBound" OnDataBound="lstvwSubjectSectionDetails_DataBound">
                                            <LayoutTemplate>
                                                <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" style="width: 8%; padding-left: 5px">
                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                        </th>
                                                        <th class="paddingL">
                                                            <asp:Label ID="lblRowNo" Width="50px" runat="server" Text="<%$ Resources:LocalizedResources, SrNo %>"></asp:Label>
                                                        </th>
                                                        <th align="left" width="65%" style="padding-left: 7px;">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, SubjectSectionName  %>" ></asp:Label>
                                                        </th>
                                                        <th>
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, SortOrder  %>" ></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="center" style="width: 8%">
                                                        <asp:CheckBox ID="chkIsSubmitted" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRowNo" runat="server" Text="No"></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtSubjectSectionName" Width="95%" MaxLength="80" runat="server"
                                                            Text='<%#Eval("SubjectSectionName") %>'></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="cmbSortOrder" runat="server">                                                           
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="center" style="width: 8%">
                                                        <asp:CheckBox ID="chkIsSubmitted" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRowNo" runat="server" Text="No"></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtSubjectSectionName" Width="95%" MaxLength="80" runat="server"
                                                            Text='<%#Eval("SubjectSectionName") %>'></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="cmbSortOrder" runat="server">
                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                            <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CustomValidator ID="cstvalSubjectSection" runat="server" ClientValidationFunction="ValidateSubjectSection"
                                        SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SelectedSubjectSectionShouldNotBeBlank%>"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstvalSubjectSectionSortOrder" runat="server" ClientValidationFunction="ValidateSubjectSectionSortOrder"
                                        SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SelectedSubjectSectionSortOrderShouldNotBeBlank%>"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstvalDuplicateValue" runat="server" ClientValidationFunction="DuplicateValue"
                                        SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, YouHaveEnteredDuplicateValueForSelectedSubjectSection%>"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstvalDuplicateSortOrder" runat="server" ClientValidationFunction="DuplicateSortOrder"
                                        SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, YouHaveEnteredDuplicateValueForSelectedSubjectSectionSortOrder%>"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstvalCheckSelectCheckBox" runat="server" ClientValidationFunction="IsSelectCheckBoxChecked"
                                        SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, AtLeastOneSubjectSectionShouldBeSelectedForSaving%>"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidSaveCount" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidRowCount" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidRowId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidScreenWidth" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidAreYouSureYouWantToDeleteCurrentlyUncheckedSubjectSection" runat="server" />
                                    <asp:HiddenField ID="hidCultureInfo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSortOrderShouldBeSelectedForRow" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSortOrderShouldNotBeDuplicatedForRow" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSubjectSectionNameShouldNotBeDuplicatedForRow" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSubjectSectionShouldNotBeBlankForRow" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divErr">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server"
                                        CssClass="ClsBtn" BorderWidth="1px" disable-page="false" ValidationGroup="Save"
                                        CausesValidation="true" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                                        CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        _clientlbl_CheckDependency = "<%=this.lblCheckDependency.ClientID %>"
        _clientlbl_UpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlbl_ErrorMessage = "<%=this.lblErrorMsg.ClientID %>"
        _clientlstvwSubjectSectionDetails = "<%=this.lstvwSubjectSectionDetails.ClientID %>"
        _clienthidSaveCount = "<%=this.hidSaveCount.ClientID %>"
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _ClientcstvalSubjectSection = "<%=this.cstvalSubjectSection.ClientID %>"
        _ClientcstvalSubjectSectionSortOrder = "<%=this.cstvalSubjectSectionSortOrder.ClientID %>"
        _ClientcstvalDuplicateSortOrder = "<%=this.cstvalDuplicateSortOrder.ClientID %>"
        _ClientcstvalDuplicateValue = "<%=this.cstvalDuplicateValue.ClientID %>"
        _ClientChkAll = _clientlstvwSubjectSectionDetails + "_ChkSelectAll";


        function ValidateSubjectSection(aSrc, args) {
            var chk
            var sMessage = false
            var iRowCount = 0
            var sMsg = ""
            chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted")
            while (chk != null) {
                if (chk.checked == true) {
                    txtSubjectSectionName = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_txtSubjectSectionName")
                    if (txtSubjectSectionName.value.trim() == "") {
                        sMessage = true
                        sMsg = sMsg + (iRowCount + 1) + ", "
                    }
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted")
            }
            if (sMessage == true) {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                $get(_ClientcstvalSubjectSection).errormessage = document.getElementById("<%=this.hidSubjectSectionShouldNotBeBlankForRow.ClientID %>").value + " " + sMsg
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ValidateSubjectSectionSortOrder(aSrc, args) {
            var chk
            var sMessage = false
            var iRowCount = 0
            var sMsg = ""
            chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted")
            while (chk != null) {
                if (chk.checked == true) {
                    cmbSortOrder = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_cmbSortOrder")
                    if (cmbSortOrder[0].selected == true) {
                        sMessage = true
                        sMsg = sMsg + (iRowCount + 1) + ", "
                    }
                }

                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted")
            }
            if (sMessage == true) {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                $get(_ClientcstvalSubjectSectionSortOrder).errormessage = document.getElementById("<%=this.hidSortOrderShouldNotBeDuplicatedForRow.ClientID %>").value + " "+ sMsg
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function DuplicateValue(oSrc, args) {
            var chk;
            var txt;
            var txt1;
            var iRowNo = 0
            var iRowCount = 0;
            var iRowCount1 = 0;
            var chkCount = 0;
            var txtShortName = "";
            var sRowNo = "";
            var lblNo = "";
            var lblNo1 = "";
            var iRowNumber = "";

            var iRowCnt = document.getElementById(_clienthidRowCount).value

            for (var iRowCount = 0; iRowCount < iRowCnt; iRowCount++) {
                chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted")
                if (chk.checked == true) {
                    txt = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_txtSubjectSectionName")
                    lblNo = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_lblRowNo").innerHTML
                    iRowNumber = ""
                    for (var iRowCount1 = iRowCount + 1; iRowCount1 < iRowCnt; iRowCount1++) {
                        chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount1 + "_chkIsSubmitted")
                        if (chk.checked == true) {
                            txt1 = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount1 + "_txtSubjectSectionName")
                            if (trimAll(txt.value) != "" && trimAll(txt1.value) != "") {
                                if (trimAll(txt.value.toLowerCase()) == trimAll(txt1.value.toLowerCase())) {
                                    lblNo1 = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount1 + "_lblRowNo").innerHTML
                                    iRowNo = iRowNo + 1;
                                    if (iRowNumber == "")
                                        iRowNumber = lblNo1
                                    else
                                        iRowNumber += ', ' + lblNo1;
                                }
                            }
                        }
                    }
                    if (iRowNumber != "") {
                        sRowNo += ', ' + lblNo + ' -> (' + iRowNumber + ')'
                    }
                }
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substring(1, sRowNo.length)
                oSrc.errormessage = document.getElementById("<%=this.hidSubjectSectionNameShouldNotBeDuplicatedForRow.ClientID %>").value + " " + sRowNo + ".";
                document.getElementById(_ClientcstvalDuplicateValue).innerHTML = document.getElementById("<%=this.hidSubjectSectionNameShouldNotBeDuplicatedForRow.ClientID %>").value + " " + sRowNo + ".";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function DuplicateSortOrder(oSrc, args) {

            var iRowCount = 0;
            var sortOrders = "";
            var isDuplicate = false;
            var sCnt = "";

            chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted");
            cmb = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_cmbSortOrder");

            while (chk != null) {
                if (chk.checked == true) {
                    if (cmb.value != 0) {
                        if (sortOrders.match("," + cmb.value + ",") != null) {
                            isDuplicate = true;
                            if (sCnt != "")
                                sCnt = sCnt + ", " + (iRowCount + 1);
                            else
                                sCnt = (iRowCount + 1);
                        }
                        else {
                            if (cmb.value != "9999")
                                sortOrders = sortOrders + "," + cmb.value + ",";
                        }
                    }
                }

                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + (iRowCount) + "_chkIsSubmitted")
                cmb = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + (iRowCount) + "_cmbSortOrder");
            }

            if (isDuplicate) {
                document.getElementById(_ClientcstvalDuplicateSortOrder).errormessage = document.getElementById("<%=this.hidSortOrderShouldNotBeDuplicatedForRow.ClientID %>").value + " " + (sCnt) + ".";
                document.getElementById(_ClientcstvalDuplicateSortOrder).innerHTML = document.getElementById("<%=this.hidSortOrderShouldNotBeDuplicatedForRow.ClientID %>").value + " " + (sCnt) + ".";
                args.IsValid = false;
            }

            if (args.IsValid == false)
                return true;

            if (args.IsValid == true)
                return false;
        }


        function IsSelectCheckBoxChecked(oSrc, args) {
            var Status = false;
            var iRowCount = document.getElementById(_clienthidRowCount).value
            for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                var chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowNumber + "_chkIsSubmitted");
                if (chk.checked == true) {
                    args.IsValid = true
                    return false
                }
            }
            args.IsValid = false
            return true
        }

        function ConfirmDelete() {
            var lbl = document.getElementById(_clientlbl_CheckDependency);
            lbl.innerHTML = "";
            var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
            lbl1.innerHTML = "";
            var lbl1 = document.getElementById(_clientlbl_ErrorMessage);
            lbl1.innerHTML = "";
            var bResult = true
            var sSvaeCount = $get(_clienthidSaveCount).value
            var iCount = 0
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted")
            while (chk != null) {
                if (chk.checked == true)
                    iCount = iCount + 1
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted")
            }
            if (parseInt(sSvaeCount) > iCount) {
                if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantToDeleteCurrentlyUncheckedSubjectSection.ClientID %>").value))
                    bResult = false
            }
            return bResult
        }

        function EnableControls(iRowid) {

            var chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowid + "_chkIsSubmitted")
            var cmbSortOrder = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowid + "_cmbSortOrder")
            var txtSubjectSectionName = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowid + "_txtSubjectSectionName")
            if (chk.checked) {
                cmbSortOrder.disabled = false;
                txtSubjectSectionName.disabled = false;
            }
            else {
                cmbSortOrder.disabled = true;
                cmbSortOrder.value = 0;
                txtSubjectSectionName.disabled = true;
            }
        }

        function SetWidth() {
            if (document.getElementById('hidScreenWidth') != null)
                $get('hidScreenWidth').value = "" + window.screen.width
        }
        SetWidth()

        function CheckAllUncheckAlls() {

            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var enble
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted")
            while (chk != null) {
                chk.checked = checkAll
                document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_txtSubjectSectionName").disabled = !checkAll;
                document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_cmbSortOrder").disabled = !checkAll;
                if (checkAll == false)
                    document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_cmbSortOrder").value = 0;
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwSubjectSectionDetails + "_ctrl" + iRowCount + "_chkIsSubmitted");
            }
        }
        function OnGridKeyUp(obj, e) {
            UpDownKeyPress(obj.id, e);
        }
    </script>
</asp:Content>
