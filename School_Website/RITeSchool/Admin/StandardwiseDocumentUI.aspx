<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StandardwiseDocumentUI.aspx.cs" Inherits="StandardwiseDocumentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
                    <tr id="trLables" runat="server">
                        <td valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                            CssClass="ClsLabel" ShowSummary="true" EnableViewState="true" />
                                        <asp:CustomValidator ID="cstValDocName" runat="server" ClientValidationFunction="CstDuplicateTextValidation"
                                            ErrorMessage="" Display="None" CssClass="LblErrorMsg" ValidationGroup="Save"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CstDocument" runat="server" ClientValidationFunction="CheckAtListOne"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                                            CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstTextValidation" runat="server" ClientValidationFunction="TextValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                                            CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateBlankSortOrder" ValidationGroup="Save"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateSortOrder" ValidationGroup="Save"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1" class="ClsTextNormal" align="center">
                                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center" id="trlblErr" runat="server">
                        <td>
                            <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                    <tr id = "trStandard" runat="server">
                        <td align="center">
                            <table>
                                <tbody>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            <asp:Label ID="lblSelectStandard" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, SelectStandard%>"></asp:Label>
                                            <span class="colonPadding ClsLabel">:</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbStandards" runat="server" OnSelectedIndexChanged="cmbStandards_SelectedIndexChanged"
                                                AutoPostBack="true" Width="100px">
                                            </asp:DropDownList>
                                            

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr id="trTermList"  runat="server">
                        <td align="center">
                            <table id="tblTermList"  runat="server" align="center" width="97%">
                                <tr align="center">
                                    <td align="center">
                                        <asp:ListView ID="lstvwDocumentConfiguration" runat="server" DataKeyNames="SchoolId, IsContinue, StandardwiseDocumentId, Is_Deleted,OriginalDocumentId"
                                            OnItemDataBound="lstvwDocumentConfiguration_ItemDataBound">
                                            <LayoutTemplate>
                                                <table align="center" width="95%" runat="server" id="tblTermInfo" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" id="chkAll" style="width: 40px" runat="server">
                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                        </th>
                                                        <th align="left" class="paddingL" style="width: 260px">
                                                          <asp:Label ID="lblDocumentName" runat="server" Text="<%$ Resources:LocalizedResources, DocumentName%>"> </asp:Label>
                                                        </th>
                                                         <th align="left" class="paddingL" style="width: 260px">
                                                          <asp:Label ID="Label2" runat="server" Text="Sort Order"> </asp:Label>
                                                        </th>
                                                        <th align="center" class="paddingL paddingLR" style="width: 90px">
                                                           <asp:Label ID="lblIsContinued" runat="server" Text="<%$ Resources:LocalizedResources,IsContinued%>"> </asp:Label>
                                                        </th>
                                                        <th align="center" class="paddingL paddingLR" style="width: 240px">
                                                           <asp:Label ID="lblIsApplicable" runat="server" Text="<%$ Resources:LocalizedResources,IsApplicableForExistingStudent%>"> </asp:Label>
                                                        </th>
                                                        <th align="center" class="paddingL paddingLR" style="width: 100px">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources,IsSubmitted%>"> </asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trData" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtDocumentName" Width="250px" CssClass="LrgTxtBox" runat="server"
                                                            MaxLength="50" Text='<%#Eval("DocumentName")%>'></asp:TextBox>
                                                    </td>
                                                      <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtsortorder" Width="250px" CssClass="LrgTxtBox" runat="server"
                                                            MaxLength="50" Text='<%#Eval("SortOrder")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsContinue" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsAppForExisStud" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsSubmitted" runat="server" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trData" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtDocumentName" Width="250px" CssClass="LrgTxtBox" runat="server"
                                                            MaxLength="50" Text='<%#Eval("DocumentName")%>'></asp:TextBox>
                                                    </td>
                                                      <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtsortorder" Width="250px" CssClass="LrgTxtBox" runat="server"
                                                            MaxLength="50" Text='<%#Eval("SortOrder")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsContinue" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsAppForExisStud" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsSubmitted" runat="server" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divErr" runat="server">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources,Save%>" OnClick="btnSave_Click" CssClass="ClsBtn" disable-page="true"
                                ValidationGroup="Save" />
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources,Cancel%>" OnClick="btnCancel_Click"
                                CssClass="ClsBtn" CausesValidation="False" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                    <asp:HiddenField ID="hidRowCnt" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                    <asp:HiddenField ID="hidDocumentNameShouldNotBeDuplicated" runat="server" />
                    <asp:HiddenField ID="hidAtLeastOneDocumentShouldBeSelected" runat="server" />
                    <asp:HiddenField ID="hidDocumentNameShouldNotBeBlank" runat="server" />
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        _clientListViewId = "<%=this.lstvwDocumentConfiguration.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _ClientChkAll = _clientListViewId + "_ChkSelectAll";
        _clientCstDocument = "<%=this.CstDocument.ClientID %>"
        _clientcstValDocName = "<%=this.cstValDocName.ClientID %>"
        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientcstTextValidation = "<%=this.cstTextValidation.ClientID %>"

        function DisableButtons() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnCancel).disabled = true
            __doPostBack(document.getElementById(_clientbtnCancel).name, '')
        }

        function CheckAllUncheckAlls() {

            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var enble
            var iRowCount = 0
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            var IsAppChk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkIsAppForExisStud").disabled;
            while (chk != null) {
                chk.checked = checkAll
                document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkIsContinue").checked = checkAll;
                if (IsAppChk == false) {
                    document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkIsAppForExisStud").checked = checkAll;
                }

                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
                if (document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkIsAppForExisStud") != null)
                    IsAppChk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkIsAppForExisStud").disabled;
            }
        }
        function CstDuplicateTextValidation(oSrc, args) {
            var chk;
            var txt;
            var txt1;
            var iRowNo = 0
            var iRowCount = 0;
            var iRowCount1 = 0;
            var chkCount = 0;

            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iRowCnt = document.getElementById(_clienthidRowCnt).value
                    txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtDocumentName")
                    while (iRowCnt > 0) {
                        chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_ChkSelect")
                        if (chk.checked == true) {
                            txt1 = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_txtDocumentName")
                            if (trimAll(txt.value) == trimAll(txt1.value) && iRowCount1 != iRowCount) {
                                iRowNo = iRowNo + 1;
                                oSrc.errormessage = document.getElementById("<%=hidDocumentNameShouldNotBeDuplicated.ClientID%>").value;
                                document.getElementById(_clientcstValDocName).innerHTML = document.getElementById("<%=hidDocumentNameShouldNotBeDuplicated.ClientID%>").value;
                                args.IsValid = false
                                return true
                            }
                        }
                        iRowCount1 = iRowCount1 + 1;
                        iRowCnt = iRowCnt - 1;
                    }
                    iRowCount1 = 0
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }
            args.IsValid = true
            return false
        }
        function TextValidation(oSrc, args) {
            var chk;
            var iRowCount = 0;
            var chkCount = 0;
            var txt;

            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtDocumentName")
                    if (txt.value != undefined) {
                        if (txt.value == '') {
                            oSrc.errormessage = document.getElementById("<%=hidDocumentNameShouldNotBeBlank.ClientID%>").value;
                            document.getElementById(_clientcstTextValidation).innerHTML = document.getElementById("<%=hidDocumentNameShouldNotBeBlank.ClientID%>").value;
                            args.IsValid = false
                            return true
                        }
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")

            }
        }
        function CheckAtListOne(oSrc, args) {

            var chk;
            var iRowCount = 0;
            var chkCount = 0;

            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    chkCount = chkCount + 1;

                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }


            if (chkCount == 0) {
                oSrc.errormessage = document.getElementById("<%=hidAtLeastOneDocumentShouldBeSelected.ClientID%>").value;
                document.getElementById(_clientCstDocument).innerHTML = document.getElementById("<%=hidAtLeastOneDocumentShouldBeSelected.ClientID%>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ResetUpdateLbl() {

            if (document.getElementById(_clientlblUpdateSucess) != null)
                document.getElementById(_clientlblUpdateSucess).style.display = "none"

        }
        function CheckUncheckSelectAllCheckBox() {
            document.getElementById(_ClientChkAll).checked = false;
        }
        function SetIscontinuedSatus(obj, iRowNo, abFlag) {
            if (obj.checked) {
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_chkIsContinue").checked = true;
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_chkIsAppForExisStud").checked = abFlag;
                
            }
            else {
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_chkIsContinue").checked = false;
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_chkIsAppForExisStud").checked = false;
                
            }
        }
        function SetIsSubmitStatus(obj, iRowNo) {
            if (!obj.checked) {
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_chkIsSubmitted").checked = false;
            }
        }
        function SetIsAppStatus(obj, iRowNo) {
            if (obj.checked) {
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_chkIsAppForExisStud").checked = true;
            }
        }

        function ValidateBlankSortOrder(oSrc, args) {
            var isFound = false;
            var index = 0;
            var chk = document.getElementById(_clientListViewId + '_ctrl' + index + '_ChkSelect')

            while (chk != null) {

                if (chk.checked) {
                    var sortOrder = document.getElementById(_clientListViewId + '_ctrl' + index + '_txtsortorder')
                    if (sortOrder.value == '') {

                        sortOrder.style.backgroundColor = "#ffffa0";                       
                        isFound = true;
                    }
                }
                
                index++
                chk = document.getElementById(_clientListViewId + '_ctrl' + index + '_ChkSelect')
            }

            if (isFound) {
                oSrc.errormessage = 'Sort Order should not be blank for yellow colored documents.'
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateSortOrder(oSrc, args) {
            var isFound = false;
            var index = 0;
            var chk = document.getElementById(_clientListViewId + '_ctrl' + index + '_ChkSelect')

            while (chk != null) {

                if (chk.checked) {
                    var sortOrder = document.getElementById(_clientListViewId + '_ctrl' + index + '_txtsortorder')
                    if (sortOrder.value != '') {

                        var rowIndex = index + 1
                        var chk1 = document.getElementById(_clientListViewId + '_ctrl' + rowIndex + '_ChkSelect')

                        while (chk1 != null) {

                            if (chk1.checked) {
                                var sortOrder1 = document.getElementById(_clientListViewId + '_ctrl' + rowIndex + '_txtsortorder')

                                if (sortOrder.value == sortOrder1.value) {
                                    sortOrder1.style.backgroundColor = "lightGray";

                                    isFound = true;
                                }
                            }

                            rowIndex++;
                            chk1 = document.getElementById(_clientListViewId + '_ctrl' + rowIndex + '_ChkSelect')
                        }
                    }
                }


                index++
                chk = document.getElementById(_clientListViewId + '_ctrl' + index + '_ChkSelect')
            }

            if (isFound) {
                oSrc.errormessage = 'Sort Order should not be duplicate for gray colored documents.'
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }


        

    </script>

</asp:Content>
