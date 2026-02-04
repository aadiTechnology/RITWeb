<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StandardWiseExamConfigurationUI.aspx.cs" Inherits="StandardWiseExamConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" style="height: 20px">
        <asp:UpdatePanel ID="updtpnl1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <table style="width: 97%" align="center">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="right" style="color: #ff3333" valign="top">
                                        <span class="ClsMdtStar">* Mandatory Fields </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:ValidationSummary ID="valSumErrorMsg" CssClass="LblErrorMsg" runat="server"
                                            ShowMessageBox="False" ShowSummary="True" ValidationGroup="Save" />
                                        <asp:CustomValidator ID="cstConfirmAction" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                            ClientValidationFunction="ConfirmAction"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblError" Style="text-align: left" runat="server" Width="100%" CssClass="LblErrorMsg"
                                EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblSuccessfullMsg" Style="text-align: center" runat="server" ForeColor="blue"
                                Width="100%" CssClass="ClsConfigText" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <table>
                                <tr>
                                    <td class="ClsBorderlight" style="width: 120px">
                                        <span class="LblNormal">Standard :</span>
                                    </td>
                                    <td align="left" style="width: 120px">
                                        <asp:DropDownList ID="cmbStandard" runat="server" CssClass="SmlCombo" AutoPostBack="true"
                                            OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                    </td>
                                    <td align="center" style="width: 130px" runat="server" id="divToprLinkHlilight">
                                        <div class="ToprLinkHlilight" style="height: 20px">
                                            <asp:HyperLink ID="hlnkSortOrder" runat="server" CssClass="ClsHilightTextB" NavigateUrl="~/RITeSchool/Admin/TestsSortOrderPopUp.aspx"
                                                Target="_blank">Exam Sort Order</asp:HyperLink></div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ListView ID="lstvwTests" runat="server" OnItemDataBound="lstvwTests_ItemDataBound"
                                DataKeyNames="SchoolwiseStandardTestId,SchoolwiseTestId,OutOfMarks,IsPublished">
                                <LayoutTemplate>
                                    <table align="center" width="600px" runat="server" id="tblTeacherDetails" style="color: #333333"
                                        cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="center" style="width: 150px">
                                                <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                            </th>
                                            <th id="thTestName" runat="server" style="padding-left: 10px;" align="left" width="210px">
                                                Exam Name
                                            </th>
                                            <th align="left" style="padding-left: 10px;">
                                                Out Of Marks
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <td align="center" style="width: 150px">
                                            <asp:CheckBox ID="ChkSelect" runat="server" />
                                        </td>
                                        <td align="left" style="padding-left: 10px;" width="210px">
                                            <asp:Label ID="lblTestName" runat="server" Width="210px" Text='<%# Eval("SchoolwiseTestName") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="padding-left: 10px;" width="240px">
                                            <asp:TextBox ID="txtConsiderMarksOutOf" CssClass="MidTxtBox" runat="server" MaxLength="3"
                                                Enabled="false" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                ondrop="event.returnValue=false" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <td align="center" style="width: 150px">
                                            <asp:CheckBox ID="ChkSelect" runat="server" />
                                        </td>
                                        <td align="left" style="padding-left: 10px" width="210px">
                                            <asp:Label ID="lblTestName" runat="server" Width="210px" Text='<%# Eval("SchoolwiseTestName") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="padding-left: 10px;" width="240px">
                                            <asp:TextBox ID="txtConsiderMarksOutOf" CssClass="MidTxtBox" runat="server" MaxLength="3"
                                                Enabled="false" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                ondrop="event.returnValue=false" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <table style="width: 600px">
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                No record found.
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <%--<tr id="trNoRecordFound" runat="server">
                        <td>
                            <table align="center" width="600px" runat="server">
                                <tr>
                                    <td class="LblNoRecord" align="center" style="width: 600px">
                                        No record found.
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <div runat="server" id="divErr">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="Save" CssClass="ClsBtn"
                                OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidSchoolId" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hidColumnCount" runat="server"></asp:HiddenField>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">
        _clientListViewId = "<%=lstvwTests.ClientID %>"
        _ClientChkAll = _clientListViewId + "_ChkSelectAll";
        _ClientcstConfirmAction = "<%=cstConfirmAction.ClientID %>"
        function CheckAllUncheckAlls() {

            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var enble
            var iRowCount = 0
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                if (checkAll) {
                    if (document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtConsiderMarksOutOf").value == "")
                        document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtConsiderMarksOutOf").disabled = false;
                }
                else
                    if (!document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtConsiderMarksOutOf").disabled && document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtConsiderMarksOutOf").value == "") {
                        document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtConsiderMarksOutOf").value = "";
                        document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtConsiderMarksOutOf").disabled = true;
                    }
                
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            }
        }

        function ConfirmAction(oSrc, args) {
            var chk
            var enble
            var iRowCount = 0
            var sMessage = "";
            var AtLeastOneCheckBoxChecked = false;
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked) {
                    if (document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtConsiderMarksOutOf").value.trim() == "0")
                        sMessage += ", " + document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_lblTestName").innerHTML;
                    AtLeastOneCheckBoxChecked = true;
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            }
            if (!AtLeastOneCheckBoxChecked) {
                document.getElementById(_ClientcstConfirmAction).errormessage = "At least one exam should be selected." ;
                args.IsValid = false
                return true
            }
            if (sMessage != "") {
                sMessage = sMessage.substring(1);
                document.getElementById(_ClientcstConfirmAction).errormessage = "Out of Marks should not be zero for exam(s): " + sMessage;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function EnableDisableControlsOfRow(chk, RowIndex, IsPublished) {
            if (!chk.checked && IsPublished != 'Y')
                document.getElementById(_clientListViewId + "_ctrl" + RowIndex + "_txtConsiderMarksOutOf").value = "";
            if (IsPublished != 'Y')
                document.getElementById(_clientListViewId + "_ctrl" + RowIndex + "_txtConsiderMarksOutOf").disabled = !chk.checked;
        }
    </script>
</asp:Content>
