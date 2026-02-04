<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LedgerSummaryUI.aspx.cs" Inherits="LedgerSummaryUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
        <asp:UpdatePanel ID="mainUpdatePanel" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="padding-right: 15px;">
                <tr id="trLedgerSummary" runat="server">
                    <td align="center" class="ClsGrayMainTitle" style="height: 20px">
                        <span style="font-weight: bold">Ledger Summary</span>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="4" width="800px">
                <tr>
                    <td align="right">
                        <span class="ClsMdtStar">* Mandatory Fields</span>
                    </td>
                </tr>
                <tr style="width: 100%">
                    <td align="left" valign="middle">
                        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                    </td>
                </tr>
                <%-- <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" Visible="false" EnableViewState="false"
                            Style="width: 100%; text-align: center; margin: 5px 0;" />
                    </td>
                </tr>--%>
                <tr id="trLedgerDDL" runat="server">
                    <td align="center" valign="middle">
                        <table>
                            <tr>
                                <td class="ClsBorderlight">
                                    <span class="ClsLabel" style="float: none; margin-right: 5px;">Ledger : </span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLedgers" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlLedgers_SelectedIndexChanged" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight">
                                    <span class="ClsLabel" style="float: none;">From Date : </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStartDate" runat="server" Width="97px" CssClass="LrgTxtBox" MaxLength="11"></asp:TextBox>
                                    <rjs:PopCalendar ID="calStartDate" runat="server" Control="txtStartDate" ShowErrorMessage="false"
                                        InvalidDateMessage="Please select valid Start Date." Format="dd MMM yyyy" ShowWeekend="True" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td class="ClsBorderlight">
                                    <span class="ClsLabel" style="float: none; margin-right: 5px;">To Date : </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndDate" runat="server" Width="97px" CssClass="LrgTxtBox" MaxLength="11"></asp:TextBox>
                                    <rjs:PopCalendar ID="calEndDate" runat="server" Control="txtEndDate" ShowErrorMessage="false"
                                        InvalidDateMessage="Please select valid End Date." Format="dd MMM yyyy" ShowWeekend="True" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="4">
                                    <asp:Button ID="btnShow" CssClass="ClsBtn" runat="server" CausesValidation="true"
                                        Text="Show" OnClick="btnShow_Click" />
                                    <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                        UseSubmitBehavior="true" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ObjectDataSource ID="objdsVouchers" runat="server" TypeName="SchoolBusinessService.AccountVoucherClient"
                            SelectMethod="GetAllVouchersForLedger" SelectCountMethod="GetCountofVouchersForLedger"
                            EnablePaging="true">
                            <SelectParameters>
                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="Int32" />
                                <asp:SessionParameter Name="aiFinancialYearId" SessionField="S_FINANCIAL_YEAR_ID" Type="Int32" />
                                <asp:ControlParameter Name="aiLedgerId" ControlID="ddlLedgers" PropertyName="SelectedValue" Type="Int32" DefaultValue="0" />
                                <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="asStartDate" Type="String" />
                                <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="asEndDate" Type="String" />
                                <asp:ControlParameter ControlID="hidSortExpression" PropertyName="Value" Name="sortExpression" Type="String" />
                                <asp:ControlParameter ControlID="hidSortDirection" PropertyName="Value" Name="sortDirection" Type="String" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ListView ID="lstvwVouchers" runat="server" OnItemDataBound="lstvwVouchers_ItemDataBound"
                            OnDataBound="lstvwVouchers_DataBound" DataKeyNames="VoucherId" OnItemCommand="lstvwVouchers_ItemCommand">
                            <LayoutTemplate>
                                <table width="750px">
                                    <tr>
                                        <td align="left" style="width: 100px; font-size: 9pt;">
                                            <a id="lnkToggel" runat="server" href="#" class="Toggel" onclick="Toggel(this)">Collapse All</a>
                                        </td>
                                        <td align="center">
                                            <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwVouchers" PageSize="15">
                                                <Fields>
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                                Text="<%# Container.StartRowIndex + 1%>" />
                                                            <asp:Label ID="lblTo" runat="server" EnableViewState="false" CssClass="LblNormal"
                                                                Text=" To " />
                                                            <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                Text=" Out Of " />
                                                            <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                Text="Records" />
                                                            <br />
                                                        </PagerTemplate>
                                                    </asp:TemplatePagerField>
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="3" cellspacing="1" class="GridBorder" width="750px">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="center" style="font-size: 9pt; width: 60px; white-space: nowrap;">
                                            <asp:LinkButton ID="lnbtnDate" runat="server" CausesValidation="false" CommandName="SORT_ROW"
                                                CommandArgument="Date" Text="Date" ForeColor="Black"  />
                                        </th>
                                        <th align="left" style="font-size: 9pt; width: 260px; white-space: nowrap;">
                                            <asp:Label ID="lblPerticuler" runat="server" CausesValidation="false" Text="Particulars"
                                                ForeColor="Black" />
                                        </th>
                                        <th align="center" style="font-size: 9pt; width: 105px; white-space: nowrap;">
                                            <asp:LinkButton ID="lnkbtnVoucherType" runat="server" CausesValidation="false" CommandName="SORT_ROW"
                                                CommandArgument="VoucherType" Text="Voucher Type" ForeColor="Black" />
                                        </th>
                                        <th align="center" style="font-size: 9pt; width: 70px; white-space: nowrap;">
                                            <asp:LinkButton ID="lnbtnSerialNumber" runat="server" CausesValidation="false" CommandName="SORT_ROW"
                                                CommandArgument="SerialNumber" Text="Sr. No." ForeColor="Black"  />
                                        </th>
                                        <th align="right" style="font-size: 9pt; width: 70px; padding-right: 6px; white-space: nowrap;">
                                            <asp:Label ID="lblDebit" runat="server" CausesValidation="false" Text="Debit (Rs.)"
                                                ForeColor="Black" />
                                        </th>
                                        <th align="right" style="font-size: 9pt; width: 70px; white-space: nowrap;">
                                            <asp:Label ID="lblCrdit" runat="server" CausesValidation="false" Text="Credit (Rs.)"
                                                ForeColor="Black" />
                                        </th>
                                        <th align="center" style="font-size: 9pt; width: 40px; white-space: nowrap;">
                                            <asp:Label ID="lblAction" runat="server" CausesValidation="false" Text="View" ForeColor="Black" />
                                        </th>
                                        <th align="center" style="font-size: 9pt; width: 40px; white-space: nowrap;">
                                            <asp:Label ID="lblExport" runat="server" CausesValidation="false" Text="Export" ForeColor="Black" />
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceHolder" runat="server">
                                    </tr>
                                    <tr id="trDataPager" runat="server" class="ClsBorderPager">
                                        <td colspan="8">
                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwVouchers" PageSize="15">
                                                <Fields>
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="right" class="LblNormal">
                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </PagerTemplate>
                                                    </asp:TemplatePagerField>
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="trGridRow" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                    <td align="center">
                                        <%# ((DateTime)Eval("Date")).ToString("dd-MMM-yyyy")%>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblPerticular" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td align="center">
                                        <%# Eval("VoucherType.Name")%>
                                    </td>
                                    <td align="right">
                                        <%# Eval("SerialNumber")%>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblDebit" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblCredit" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:ImageButton ID="imgbtnView" runat="server" AlternateText="View" ToolTip="View"
                                            CausesValidation="false" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                            Style="vertical-align: middle;" />
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="lbtnExport" runat="server" ViewStateMode="Enabled" Text="Export"
                                                            CommandName="EXPORT"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr id="trPerticulersDetails" runat="server" visible="false" align="center" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow clsPerticulars" : "ClsGridAltRow clsPerticulars" %>'>
                                    <td style="width: 60px;">
                                    </td>
                                    <td id="tdPerticulersDetails" runat="server" align="center" style="width: 260px">
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ListView ID="lstvwPerticulersDetails" runat="server">
                                                        <LayoutTemplate>
                                                            <table width="100%" runat="server" id="tblSubList" style="color: #333333; font-size: 9pt"
                                                                cellpadding="0" cellspacing="1" align="center">
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server">
                                                                <td align="left" style="width: 165px">
                                                                    <%# Eval("Ledger.Name")%>
                                                                </td>
                                                                <td align="right" style="width: 85px">
                                                                    <%# Utility.CommonUtility.FormatCurrency(Eval("Amount"))%>
                                                                    <%# Convert.ToBoolean(Eval("IsDebit")) ? "Dr" : "Cr" %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 105px;">
                                    </td>
                                    <td style="width: 70px;">
                                    </td>
                                    <td style="width: 70px;">
                                    </td>
                                    <td style="width: 70px;">
                                    </td>
                                    <td style="width: 40px;">
                                    </td>
                                    <td style="width: 40px;">
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="LblNoRecord" style="margin: 10px 0; width: 750px; text-align: center;">
                                    No record found.</div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr class="height20">
                    <td>
                    </td>
                </tr>
                <tr id="trLedgerTotal" runat="server" visible="false">
                    <td align="center">
                        <table>
                            <tr>
                                <td style="background-color: #e4efc4;" align="left">
                                    <span class="LblNrmlB" style="width: 200px">Total Debit Amount :</span>
                                </td>
                                <td align="left" style="background-color: #eaeaea">
                                    <asp:Label ID="lblTotalDebitAmount" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightFeeL"
                                        Text="0" />
                                </td>
                                <td style="width: 50px">
                                </td>
                                <td style="background-color: #e4efc4;" align="left">
                                    <span class="LblNrmlB" style="width: 200px">Total Credit Amount :</span>
                                </td>
                                <td align="left" style="background-color: #eaeaea">
                                    <asp:Label ID="lblTotalCreditAmount" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightFeeL"
                                        Text="0" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr class="height20">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnExport" runat="server" Text="Export All" CssClass="ClsBtn" 
                            Visible= "false" CausesValidation="false" OnClientClick="CheckShowParticulares()" onclick="btnExport_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                    </td>
                </tr>
            </table>
            <%-- HIDDEN FIELDS --%>
            <asp:HiddenField ID="hidSortExpression" runat="server" />
            <asp:HiddenField ID="hidSortDirection" runat="server" />
            <asp:HiddenField ID="hidFinancialYrStartDt" runat="server" />
            <asp:HiddenField ID="hidFinancialYrEndDt" runat="server" />
            <asp:HiddenField ID="hidToggel" runat="server" />
            <asp:HiddenField ID="hidIsParticularsDisplay" runat="server" />
            <%--<asp:RequiredFieldValidator ID="reqStartDt" runat="server" ControlToValidate="txtStartDate"
                CssClass="LblErrorMsg" Display="None" ErrorMessage="Start Date should not be blank."></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="reqEndDt" runat="server" ControlToValidate="txtEndDate"
                CssClass="LblErrorMsg" Display="None" ErrorMessage="End Date should not be blank."></asp:RequiredFieldValidator>--%>
            <%-- VALIDATOR --%>
            <asp:CompareValidator ID="cmpLedgerValidator" runat="server" Display="None" ControlToValidate="ddlLedgers"
                Operator="GreaterThan" ValueToCompare="0" ErrorMessage="Ledger should be selected."> </asp:CompareValidator>
            <asp:CustomValidator ID="cstStartDateValidator" runat="server" Display="None" ClientValidationFunction="ValidateStartDate"
                EnableClientScript="true" />
            <asp:CustomValidator ID="cstEndDateValidator" runat="server" Display="None" ClientValidationFunction="ValidateEndDate"
                EnableClientScript="true" />
            <asp:CustomValidator ID="cstDateValidator" runat="server" Display="None" ClientValidationFunction="ValidateDates"
                EnableClientScript="true" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lstvwVouchers" />
            <asp:AsyncPostBackTrigger ControlID="ddlLedgers" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var _clienttxtStartDate = '<%= this.txtStartDate.ClientID %>';
        var _clienttxtEndDate = '<%= this.txtEndDate.ClientID %>';
        var _clientFinancialYrStartDt = '<%= this.hidFinancialYrStartDt.ClientID %>';
        var _clientFinancialYrEndDt = '<%= this.hidFinancialYrEndDt.ClientID %>';
        var _clienthidToggel = '<%= this.hidToggel.ClientID %>';

        function Toggel(src) {       
            $(".clsPerticulars").toggle();
            if (src.innerHTML == "Collapse All") {
                src.innerHTML = "Expand All";
                $get(_clienthidToggel).value = 0;
            }
            else {
                src.innerHTML = "Collapse All";
                $get(_clienthidToggel).value = 1;
            }
           
        }

//        function Toggel1() {
//            
//            $(".clsPerticulars").toggle();           
//        }

        function IsValidDate(date) {
            if (typeof (date) == 'string')
                date = new Date(date);
            return !(date == 'Invalid Date' || date == 'NaN' || date.getFullYear() < 1900);
        }

        function ValidateStartDate(src, args) {
            var txtStartDate = $get(_clienttxtStartDate);
            var FinancialYrStartDate = $get(_clientFinancialYrStartDt);
            var FinancialYrEndDate = $get(_clientFinancialYrEndDt);

            args.IsValid = true;

            if (txtStartDate.value.trim() == '') {
                args.IsValid = false;
                src.errormessage = 'From Date should be selected.';
            }
            else {

                var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));
                var dtFinancialYrStart = new Date(FinancialYrStartDate.value.replace(/-/g, ' '));
                var dtFinancialYrEnd = new Date(FinancialYrEndDate.value.replace(/-/g, ' '));

                if (!IsValidDate(dtStartDate)) {
                    args.IsValid = false;
                    src.errormessage = 'Please select a valid From Date.';
                }
                else if (!(dtStartDate >= dtFinancialYrStart && dtStartDate <= dtFinancialYrEnd)) {
                    args.IsValid = false;
                    src.errormessage = 'From Date should be in current financial year.';
                }
                //                else if (dtStartDate > dtToday) {
                //                    args.IsValid = false;
                //                    src.errormessage = 'From date should not be a future date.';
                //                }
            }

            return !args.IsValid;
        }

        // This function validates the end date.
        function ValidateEndDate(src, args) {
            //            var chkDateRange = $get(_clientchkDateRange);

            args.IsValid = true;


            var txtEndDate = $get(_clienttxtEndDate);
            var FinancialYrStartDate = $get(_clientFinancialYrStartDt);
            var FinancialYrEndDate = $get(_clientFinancialYrEndDt);

            if (txtEndDate.value.trim() == '') {
                args.IsValid = false;
                src.errormessage = 'To Date should be selected.';
            }
            else {
                var dtToday = new Date();
                var dtEndDate = new Date(txtEndDate.value.replace(/-/g, ' '));
                var dtFinancialYrStart = new Date(FinancialYrStartDate.value.replace(/-/g, ' '));
                var dtFinancialYrEnd = new Date(FinancialYrEndDate.value.replace(/-/g, ' '));

                if (!IsValidDate(dtEndDate)) {
                    args.IsValid = false;
                    src.errormessage = 'Please select a valid To Date.';
                }
                else if (!(dtEndDate >= dtFinancialYrStart && dtEndDate <= dtFinancialYrEnd)) {
                    args.IsValid = false;
                    src.errormessage = 'To Date should be in current financial year.';
                }
                //                    else if (dtEndDate > dtToday) {
                //                        args.IsValid = false;
                //                        src.errormessage = 'To date should not be a future date.';
                //                    }
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
                src.errormessage = 'From Date should not be greater than To Date.';
            }
            return !args.IsValid;
        }

        function CheckShowParticulares() {
            var confirmation = confirm("Please click on OK to include voucher particulars");
            var sShowParticulars = document.getElementById('<%= hidIsParticularsDisplay.ClientID %>');
            if (confirmation) {
                sShowParticulars.value = "Y";
            }
            else {
                sShowParticulars.value = "N"
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
