<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="SalaryDifferenceConfigPopup.aspx.cs" Inherits="SalaryDifferenceConfigPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td>
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="center" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">Salary Difference Configuration</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left">
                                <asp:RadioButton ID="optDefault" runat="server" Text="Default" CssClass="ClsLabel"
                                    AutoPostBack="true" GroupName="Configuration" OnCheckedChanged="optDefault_CheckedChanged" />
                            </td>
                            <td align="left">
                                <asp:RadioButton ID="optSaved" runat="server" Text="Saved" CssClass="ClsLabel" AutoPostBack="true"
                                    GroupName="Configuration" OnCheckedChanged="optSaved_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height:10px;">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="50%">
                        <tr>
                            <td width="20%" class="ClsBorderlight">
                                <span class="ClsLabel">Year : </span>
                            </td>
                            <td class="ClsHilightBGB">
                                <asp:Label ID="lblYear" runat="server" CssClass="ClsLabel"></asp:Label>
                            </td>
                            <td width="10%">
                            </td>
                            <td width="20%" class="ClsBorderlight">
                                <span class="ClsLabel">Month : </span>
                            </td>
                            <td class="ClsHilightBGB" align="center">
                                <asp:Label ID="lblMonth" runat="server" CssClass="ClsLabel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <table border="0" cellpadding="1" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <asp:ListView ID="lstvwEarningDeduction" runat="server" DataKeyNames="EarningsDeductionsId"
                                            OnItemDataBound="lstvwEarningDeduction_ItemDataBound">
                                            <LayoutTemplate>
                                                <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader">
                                                        <th align="center">
                                                            <asp:CheckBox ID="ChkAll" runat="server" onclick="CheckUncheckAll(this)" AutoPostBack="true"
                                                                OnCheckedChanged="ChkAll_CheckedChanged" />
                                                        </th>
                                                        <th align="left" width="50%">
                                                            Earning / Deduction
                                                        </th>
                                                        <th align="center">
                                                            Formula / Range Name
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="ChkSelect_CheckedChanged" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblShortName" runat="server" CssClass="ClsLabel" Text='<%#Eval("ShortName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="cmbEarnDeductFormula" runat="server" CssClass="ExLrgCombo">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trlstvwUsers" runat="server" visible="false">
                                                    <td id="tdlstvwUsers" runat="server" align="center" colspan="3">
                                                        <table width="80%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ListView ID="lstvwUsers" runat="server" OnItemDataBound="lstvwUsers_ItemDataBound"
                                                                        DataKeyNames="UserId,FormulaRangeId">
                                                                        <LayoutTemplate>
                                                                            <table width="100%" runat="server" id="tblUsers" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1" class="GridBorder">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="left" width="50%">
                                                                                        User Name
                                                                                    </th>
                                                                                    <th align="center">
                                                                                        Formula / Range Name
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                            </table>
                                                                        </LayoutTemplate>
                                                                        <ItemTemplate>
                                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblUserName" runat="server" class="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:DropDownList ID="cmbEarnDeduct" runat="server" CssClass="ExLrgCombo">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblUserName" runat="server" class="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:DropDownList ID="cmbEarnDeduct" runat="server" CssClass="ExLrgCombo">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                    </asp:ListView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="ChkSelect_CheckedChanged" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblShortName" runat="server" CssClass="ClsLabel" Text='<%#Eval("ShortName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="cmbEarnDeductFormula" runat="server" CssClass="ExLrgCombo">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trlstvwUsers" runat="server" visible="false">
                                                    <td id="tdlstvwUsers" runat="server" align="center" colspan="3">
                                                        <table width="80%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:ListView ID="lstvwUsers" runat="server" OnItemDataBound="lstvwUsers_ItemDataBound"
                                                                        DataKeyNames="UserId,FormulaRangeId">
                                                                        <LayoutTemplate>
                                                                            <table width="100%" runat="server" id="tblUsers" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1" class="GridBorder">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="left" width="50%">
                                                                                        User Name
                                                                                    </th>
                                                                                    <th align="center">
                                                                                        Formula / Range Name
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                            </table>
                                                                        </LayoutTemplate>
                                                                        <ItemTemplate>
                                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblUserName" class="ClsLabel" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:DropDownList ID="cmbEarnDeduct" runat="server" CssClass="ExLrgCombo">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblUserName" class="ClsLabel" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:DropDownList ID="cmbEarnDeduct" runat="server" CssClass="ExLrgCombo">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                    </asp:ListView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table id="tblNote" runat="server">
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                            <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note1 : </span>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                            <span style="border-width: 0px" class="LblSmlV">'Default' option will show details according to configuration from ‘Earnings and Deductions’ screen.</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                            <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note2 : </span>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                            <span style="border-width: 0px" class="LblSmlV">'Saved' option will show details that have previously saved from this screen.</span>
                                        </td>
                                    </tr>                                    
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="50%">
                                    <tr>
                                        <td align="center">
                                            <asp:Button CssClass="ClsBtn" ID="BtnSave" runat="server" Text="Save" disable-page="true"
                                                OnClick="BtnSave_Click" />
                                            <asp:Button CssClass="ClsBtn" ID="BtnClose" CausesValidation="false" runat="server"
                                                OnClientClick="ClosePopup()" Text="Close" />
                                            <asp:HiddenField ID="hidEarningDeductionId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                            <asp:CustomValidator ID="cstvalConfiguration" runat="server" ClientValidationFunction="ValidateConfiguration"
                                                SetFocusOnError="True" Display="None" ErrorMessage="At least one Earning / Deduction should be selected."></asp:CustomValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clientlstvwEarningDeduction = "<%=this.lstvwEarningDeduction.ClientID %>"
        _clientBtnSave = "<%=this.BtnSave.ClientID %>"
        _clientHidQueryString = "<%=this.hidQueryString.ClientID %>"

        function CheckUncheckAll(obj) {
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwEarningDeduction + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = obj.checked;
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwEarningDeduction + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        function DisableControls(obj, iRowNumber) {
            var chkSelect = document.getElementById(_clientlstvwEarningDeduction + "_ctrl" + iRowNumber + "_ChkSelect")
            var checked = !chkSelect.checked

            var formula = document.getElementById(_clientlstvwEarningDeduction + "_ctrl" + iRowNumber + "_cmbEarnDeductFormula");
            if (formula != null)
                formula.disabled = checked;
        }

        function DisableAll() {
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwEarningDeduction + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                DisableControls(chk, iRowCount);
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwEarningDeduction + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        DisableAll();

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        function EndRequestHandler(sender, args) {
            DisableAll();
        }

        function ClosePopup() {
            window.close();
        }

        function ValidateConfiguration(oSrc, args) {
            var chk
            var iRowCount = 0
            var isFound = false;
            chk = document.getElementById(_clientlstvwEarningDeduction + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked) {
                    isFound = true;
                    break;
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwEarningDeduction + "_ctrl" + iRowCount + "_ChkSelect")
            }
            args.IsValid = isFound;
            return !isFound;
        }
        

    </script>
</asp:Content>
