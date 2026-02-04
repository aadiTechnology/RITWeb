<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    ClientIDMode="AutoID" AutoEventWireup="true" CodeFile="TrialBalanceReportUI.aspx.cs"
    Inherits="TrialBalanceReportUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%" align="center">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <tr>
                    <td align="right">
                        <span class="ClsMdtStar">* Mandatory Fields</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellspacing="4">
                            <tr>
                                <td>
                                    <asp:Label ID="lblStartDate" runat="server" Text="From Date:" CssClass="ClsBorderlight"
                                        Style="font-size: 9pt; padding: 5px 3px; vertical-align: middle;" />
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="SmlTxtBox" Style="vertical-align: middle;" />
                                    <rjs:PopCalendar ID="dtStartDate" runat="server" Control="txtStartDate" Format="dd mmm yyyy"
                                        ShowWeekend="True" ShowErrorMessage="false" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td id="tdEndDate" runat="server">
                                    <span class="ClsBorderlight" style="font-size: 9pt; padding: 5px 3px; vertical-align: middle;">
                                        To Date :</span>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="SmlTxtBox" Style="vertical-align: middle;" />
                                    <rjs:PopCalendar ID="dtEndDate" runat="server" Control="txtEndDate" Format="dd mmm yyyy"
                                        ShowWeekend="True" ShowErrorMessage="false" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td>
                                    <asp:Button ID="btnShow" runat="server" CssClass="ClsBtn" Text="Show" OnClick="btnShow_Click" />
                                    <asp:CustomValidator ID="cstStartDateValidator" runat="server" Display="None" ClientValidationFunction="ValidateStartDate"
                                        EnableClientScript="true" />
                                    <asp:CustomValidator ID="cstEndDateValidator" runat="server" Display="None" ClientValidationFunction="ValidateEndDate"
                                        EnableClientScript="true" />
                                    <asp:CustomValidator ID="cstDateValidator" runat="server" Display="None" ClientValidationFunction="ValidateDates"
                                        EnableClientScript="true" />
                                    <asp:CustomValidator ID="cstAcDateValidator" runat="server" Display="None" EnableClientScript="true"
                                        ClientValidationFunction="AccountsValidateDate" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trMenu" runat="server">
                    <td align="center">
                        <table width="50%">
                            <tr>
                                <td>
                                    <asp:Menu runat="server" ID="MenuControl" Orientation="Horizontal" CssClass="ClsRoot" disable-page="true"
                                        Font-Bold="True" RenderingMode="List" Visible="True" Font-Underline="True" OnMenuItemClick="MenuControl_MenuItemClick">
                                    </asp:Menu>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ContentTemplate>
        </asp:UpdatePanel>
    </table>
    <table id="tblNoRec" runat="server" visible="False" height="30px">
        <tr>
            <td class="LblNoRecord" align="center" width="650px">
                <asp:Label runat="server" Text="No Records Found." Width="650px"></asp:Label>
            </td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="20" border="0" align="center">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <tbody id="GroupBody" runat="server">
                    <tr>
                        <td align="center">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:ListView ID="lstvwGroups" runat="server" DataKeyNames="Id,IsPrimary,IsConsideredForTrialBalance,GroupNature,IsSystemDefined"
                                            OnItemDataBound="lstvwGroups_ItemDataBound" OnItemCommand="lstvwGroups_ItemCommand">
                                            <LayoutTemplate>
                                                <table id="tblGroups" class="GridBorder" cellpadding="3" cellspacing="1" width="650px"
                                                    style="border-color: darkgray">
                                                    <tr style="font-size: 9pt; background-color: #bcdbca !important; border-color: darkgray">
                                                        <th align="left" rowspan="2">
                                                            Group Name
                                                        </th>
                                                        <th align="center" colspan="2">
                                                            Closing Balance
                                                        </th>
                                                    </tr>
                                                    <tr style="font-size: 9pt; background-color: #bcdbca !important; border-color: darkgray">
                                                        <th align="right">
                                                            Debit (Rs.)
                                                        </th>
                                                        <th align="right">
                                                            Credit (Rs.)
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trGridRow" runat="server" style="border-color: darkgray" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                    <td align="left">
                                                        <asp:LinkButton runat="server" ID="lnkGroupName" Text='<%# Eval("Name") %>' CommandName="GETLEDGERS" disable-page="true"></asp:LinkButton>
                                                        <asp:Label runat="server" ID="lblGrandTotal" Text='<%# Eval("Name") %>' Visible="False"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblDebit" runat="server" Style="padding-right: 2px;">
                                        <%# Utility.CommonUtility.FormatCurrency(Eval("Debit"))%></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblCredit" runat="server" Style="padding-right: 2px;">
                                        <%# Utility.CommonUtility.FormatCurrency(Eval("Credit"))%></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <table width="650px">
                                                    <tr>
                                                        <td class="LblNoRecord" align="center">
                                                            No Records Found.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <tbody id="LedgerBody" runat="server">
                    <tr>
                        <td align="center">
                            <table>
                                <tr>
                                    <td>
                                        <asp:ListView ID="lstvwLedger" runat="server" DataKeyNames="Id,GroupId" OnItemDataBound="lstvwLedger_ItemDataBound"
                                            OnItemCommand="lstvwLedger_ItemCommand">
                                            <LayoutTemplate>
                                                <table id="tblGroups" class="GridBorder" cellpadding="3" cellspacing="1" width="650px"
                                                    style="border-color: darkgray">
                                                    <tr style="font-size: 9pt; background-color: #C6D0AB;" style="border-color: darkgray">
                                                        <th align="left" rowspan="2">
                                                            Ledger Name
                                                        </th>
                                                        <th align="center" colspan="2">
                                                            Closing Balance
                                                        </th>
                                                    </tr>
                                                    <tr style="font-size: 9pt; background-color: #C6D0AB; border-color: darkgray">
                                                        <th align="right">
                                                            Debit (Rs.)
                                                        </th>
                                                        <th align="right">
                                                            Credit (Rs.)
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trGridRow" runat="server" style="border-color: darkgray" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                    <td align="left">
                                                        <asp:LinkButton runat="server" ID="lnkLedgerName" Text='<%# Eval("Name") %>' CommandName="MONTHWISEDETAILS" disable-page="true"></asp:LinkButton>
                                                        <asp:Label runat="server" ID="lblGrandTotal" Text='<%# Eval("Name") %>' Visible="False"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblDebit" runat="server" Style="padding-right: 2px;">
                                        <%# Utility.CommonUtility.FormatCurrency(Eval("Debit"))%></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblCredit" runat="server" Style="padding-right: 2px;">
                                        <%# Utility.CommonUtility.FormatCurrency(Eval("Credit"))%></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <table width="650px">
                                                    <tr>
                                                        <td class="LblNoRecord" align="center">
                                                            No Records Found.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
                <tbody id="MonthwiseDetailsBody" runat="server">
                    <tr>
                        <td>
                            <asp:ListView ID="lstvwMonthwiseDetails" runat="server" DataKeyNames="GroupId,LedgerId,StartDate,EndDate,MonthId,ClosingBalance"
                                OnItemDataBound="lstvwMonthwiseDetails_ItemDataBound">
                                <LayoutTemplate>
                                    <table id="tblGroups" class="GridBorder" cellpadding="3" cellspacing="1" width="650px"
                                        style="border-color: darkgray">
                                        <tr style="font-size: 9pt; background-color: #C6D0AB; border-color: darkgray">
                                            <th align="left" rowspan="2">
                                                Month Name
                                            </th>
                                            <th align="center" colspan="2">
                                                Transaction
                                            </th>
                                            <th rowspan="2" align="right">
                                                Closing Balance
                                            </th>
                                        </tr>
                                        <tr style="font-size: 9pt; background-color: #C6D0AB; border-color: darkgray">
                                            <th align="right">
                                                Debit (Rs.)
                                            </th>
                                            <th align="right">
                                                Credit (Rs.)
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trGridRow" runat="server" style="border-color: darkgray" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                        <td align="left">
                                            <asp:HyperLink runat="server" ID="lnkMonthName" Text='<%# Eval("MonthName") %>' Font-Underline="True"
                                                NavigateUrl="#"></asp:HyperLink>
                                            <asp:Label runat="server" ID="lblGrandTotal" Text='<%# Eval("MonthName") %>' Visible="False"
                                                Font-Bold="True"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblDebit" runat="server" Style="padding-right: 2px;">
                                        <%# Utility.CommonUtility.FormatCurrency(Eval("Debit"))%></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblCredit" runat="server" Style="padding-right: 2px;">
                                        <%# Utility.CommonUtility.FormatCurrency(Eval("Credit"))%></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblClosingBalance" runat="server" Style="padding-right: 2px;">
                                        <%# Utility.CommonUtility.FormatCurrency(Eval("ClosingBalance"))%></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <table width="650px">
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                No Records Found.
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                </tbody>
            </ContentTemplate>
        </asp:UpdatePanel>
    </table>
    <table>
        <tr>
            <td align="center">
                <asp:HiddenField ID="hidFinancialYearJSON" runat="server" />
                <asp:HiddenField ID="hidCanEditOldFinancialYear" runat="server" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        var _clienttxtStartDate = '<%= this.txtStartDate.ClientID %>';
        var _clienttxtEndDate = '<%= this.txtEndDate.ClientID %>';

        // Register listeners for Postbacks
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //prm.add_beginRequest(BeginRequestHandler);
        //prm.add_endRequest(EndRequestHandler);

        var _FinancialYear = eval('[' + $get('<%= this.hidFinancialYearJSON.ClientID %>').value + ']')[0];
        var _CanEditOldFinancialYear = Boolean($get('<%= this.hidCanEditOldFinancialYear.ClientID %>').value == 'true');
        //        //                // This function is used to disable controls on the page when a postback occurs.
        //        function BeginRequestHandler() {

        //        }

        //        //                // This function is used to enabled controls once a postback is complete.
        //        function EndRequestHandler() {
        //          
        //        }

        function IsValidDate(date) {
            if (typeof (date) == 'string')
                date = new Date(date);
            return !(date == 'Invalid Date' || date == 'NaN' || date.getFullYear() < 1900);
        }

        // This function validates the start date
        function ValidateStartDate(src, args) {
            var txtStartDate = $get(_clienttxtStartDate);

            args.IsValid = true;

            if (txtStartDate.value.trim() == '') {
                args.IsValid = false;
                src.errormessage = 'From Date should be selected.';
            }
            else {
                var dtToday = new Date();
                var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));

                if (!IsValidDate(dtStartDate)) {
                    args.IsValid = false;
                    src.errormessage = 'Please select a valid From Date.';
                }
            }

            return !args.IsValid;
        }

        // This function validates the end date.
        function ValidateEndDate(src, args) {
            args.IsValid = true;
            var txtEndDate = $get(_clienttxtEndDate);

            if (txtEndDate.value.trim() == '') {
                args.IsValid = false;
                src.errormessage = 'To Date should be selected.';
            }
            else {
                var dtToday = new Date();
                var dtEndDate = new Date(txtEndDate.value.replace(/-/g, ' '));

                if (!IsValidDate(dtEndDate)) {
                    args.IsValid = false;
                    src.errormessage = 'Please select a valid To Date.';
                }
            }
            return !args.IsValid;
        }

        // This function validates both the dates to check if start date is not greater than end date.
        function ValidateDates(src, args) {
            args.IsValid = true;
            var txtStartDate = $get(_clienttxtStartDate);
            var txtEndDate = $get(_clienttxtEndDate);
            var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));
            var dtEndDate = new Date(txtEndDate.value.replace(/-/g, ' '));

            if (IsValidDate(dtStartDate) && IsValidDate(dtEndDate) && dtStartDate > dtEndDate) {
                args.IsValid = false;
                src.errormessage = 'To Date should be greater than or equal to From Date.';
            }
            return !args.IsValid;
        }

        function AccountsValidateDate(src, args) {
            args.IsValid = true;
            if (!_FinancialYear)
                return !args.IsValid;

            if (_FinancialYear.IsClosed && !_CanEditOldFinancialYear) {
                args.IsValid = false;
                src.errormessage = 'Financial year is closed and you do not have edit access.';
            }
            else {
                var dtFinancialYearStartDate = new Date(parseInt(_FinancialYear.StartDate.replace("/Date(", "").replace(")/", ""), 10));
                var dtFinancialYearEndDate = new Date(parseInt(_FinancialYear.EndDate.replace("/Date(", "").replace(")/", ""), 10));
                var dtFromdate = new Date(convertdate($get(_clienttxtStartDate).value));
                var dtTodate = new Date(convertdate($get(_clienttxtEndDate).value));

                if ((dtFromdate < dtFinancialYearStartDate || dtFromdate > dtFinancialYearEndDate) || (dtTodate < dtFinancialYearStartDate || dtTodate > dtFinancialYearEndDate)) {
                    args.IsValid = false;
                    src.errormessage = 'From Date & To Date should be within current financial year (i.e. from 1-April-' + dtFinancialYearStartDate.getFullYear() + ' to 31-March-' + dtFinancialYearEndDate.getFullYear() + ').';
                }
            }
            return !args.ISValid;
        }

    </script>
</asp:Content>
