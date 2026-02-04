<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="DisableOnlineBank.aspx.cs" Inherits="DisableOnlineBank"
    EnableEventValidation="false" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" style="width: 95%">
        <tr align="center">
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table align="center" style="width: 95%">
                         <tr id="trMandetory" runat="server">
                                <td align="right">
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </td>
                            </tr>
                            <tr align="left">
                                <td align="left">
                                    <asp:ValidationSummary ID="valSumBank" CssClass="LblErrorMsg" ShowSummary="true"
                                        runat="server" ValidationGroup="Save" />
                                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" Text="" EnableViewState="false"></asp:Label>
                                    <asp:CompareValidator ID="cmpLedgerValidator" CssClass="LblErrorMsg" runat="server" Display="None" ControlToValidate="ddlBankName"
                                        Operator="GreaterThan" ValueToCompare="0" ValidationGroup="Save" ErrorMessage="Bank name should be selected." />
                                    <asp:CustomValidator ID="cstStartEndDateValidation" CssClass="LblErrorMsg" runat="server"
                                        EnableViewState="false" SetFocusOnError="True" Display="None" ClientValidationFunction="ValidateStartEndDate"
                                        ValidationGroup="Save"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstEndDateValidation" runat="server" CssClass="LblErrorMsg"
                                        EnableViewState="false" SetFocusOnError="True" Display="None" ClientValidationFunction="ValidateEndDate"
                                        ValidationGroup="Save"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstTimeRangeValidation" runat="server" CssClass="LblErrorMsg"
                                        EnableViewState="false" SetFocusOnError="True" Display="None" ClientValidationFunction="ValidateTimeRange"
                                        ValidationGroup="Save"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstInvalidStartTime" CssClass="LblErrorMsg" runat="server"
                                        EnableViewState="false" SetFocusOnError="True" Display="None" ClientValidationFunction="ValidateStartTime"
                                        ValidationGroup="Save"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstInvalidEndTime" CssClass="LblErrorMsg" runat="server"
                                        EnableViewState="false" SetFocusOnError="True" Display="None" ClientValidationFunction="ValidateEndTime"
                                        ValidationGroup="Save" Height="16px"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <asp:Label ID="lblSuccessMsg" runat="server" CssClass="LblNormalImg" Font-Bold="true"
                                        EnableViewState="false" ForeColor="Blue"></asp:Label>
                                </td>
                            </tr>                            
                            <tr id="trLegend" runat="server">
                                <td align="left" style="padding-left: 28px;">
                                    <table>
                                        <tr>
                                            <td width="65px">
                                                <span id="lblLegend" runat="server" class="ClsLblLgnd">Legend :</span>
                                            </td>
                                            <td width="20px" align="left">
                                                <asp:Label ID="lblActiveRuleColor" runat="server" BackColor="#FFCCCC" Height="20px"
                                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                    EnableViewState="False"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                            </td>
                                            <td width="80px">
                                                <span id="Span2" class="ClsLblLgnd">Active Rule</span>
                                            </td>
                                            <td width="20px" align="left">
                                                <asp:Label ID="lblPastRuleColor" runat="server" BackColor="Silver" Height="20px"
                                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                    EnableViewState="False"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                            </td>
                                            <td width="620px">
                                                <span id="Span3" class="ClsLblLgnd">Inactive Rule</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <table style="vertical-align: top; width: 500px;" align="center">
                            <tr class="ClspaddingL">
                                <td class="ClsBorderlight" style="width: 130px; height: 25px;">
                                    <span id="Span5" class="paddingLSML">Bank Name :</span>
                                </td>
                                <td align="left" style="padding-left: 5px; width: 300px; height: 25px;">
                                    <div>
                                        <asp:DropDownList ID="ddlBankName" runat="server" Width="190px">
                                        </asp:DropDownList>
                                        <span style="color: #ff0000; font-size: 9pt;">*</span>
                                    </div>
                                </td>
                            </tr>
                            <tr style="width: 32px;">
                                <td class="ClsBorderlight" style="width: 130px; height: 25px;">
                                    <span class="paddingLSML">Start Date and Time : </span>
                                </td>
                                <td style="width: 300px; height: 25px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" MaxLength="11" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <rjs:PopCalendar ID="calFromDate" runat="server" Control="txtStartDate" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid Start Date." Format="dd MMM yyyy" ShowWeekend="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStartTime" MaxLength="8" runat="server" Width="65px"></asp:TextBox><span
                                                    style="color: #ff0000; font-size: 9pt;"> *</span><span class="LblNormal" style="padding-left: 4px">e.g. 
                                                10:10 AM </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="width: 32px;">
                                <td class="ClsBorderlight" style="width: 130px; height: 25px;">
                                    <span class="paddingLSML">End Date and Time : </span>
                                </td>
                                <td style="width: 300px; height: 25px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" MaxLength="11" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <rjs:PopCalendar ID="calToDate" runat="server" Control="txtEndDate" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid Start Date." Format="dd MMM yyyy" ShowWeekend="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndTime" MaxLength="8" runat="server" Style="margin-left: 0px"
                                                    Width="65px"></asp:TextBox><span class="LblNormal" style="padding-left: 10px;"> 
                                                e.g. 04:10 PM </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="save"
                                        ValidationGroup="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" CausesValidation="False" CssClass="ClsBtn"
                                        runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table id="tblListView" style="width: 850px;">
                            <tr>
                                <td align="center">
                                    <tr id="trPhotoPager" runat="server">
                                        <td align="center">
                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwDisabledBankDetails">
                                                <Fields>
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                CssClass="LblNrmlB" />
                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                CssClass="LblNrmlB" />
                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                CssClass="LblNrmlB" />
                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                                            <br />
                                                        </PagerTemplate>
                                                    </asp:TemplatePagerField>
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center">
                                            <asp:ListView ID="lstvwDisabledBankDetails" runat="server" Style="text-align: left"
                                                DataKeyNames="DisabledBankId,RuleStatus" OnItemDataBound="lstvwDisabledBankDetails_ItemDataBound"
                                                OnItemCommand="lstvwDisabledBankDetails_ItemCommand" OnDataBound="lstvwDisabledBankDetails_DataBound">
                                                <LayoutTemplate>
                                                    <table runat="server" id="tblBankList" style="color: #333333" width="800px" cellpadding="0"
                                                        cellspacing="1" class="GridBorder" align="center">
                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                            <th align="center" width="90px">
                                                                Sr. No.
                                                            </th>
                                                            <th align="left" class="paddingLSML" width="450px">
                                                                Bank Name
                                                            </th>
                                                            <th align="center" width="300px">
                                                                Start Date and Time
                                                            </th>
                                                            <th align="center" width="300px">
                                                                End Date and Time
                                                            </th>
                                                            <th align="center" width="50px">
                                                                Edit
                                                            </th>
                                                            <th align="center" width="50px">
                                                                Delete
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                        <tr class="ClsBorderPager" width="100%" runat="server" id="trDataPager" style="color: #333333"
                                                            cellpadding="0" cellspacing="1">
                                                            <td align="left" colspan="6">
                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwDisabledBankDetails"
                                                                    PageSize="20">
                                                                    <Fields>
                                                                        <asp:TemplatePagerField>
                                                                            <PagerTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <span id="MessageLabel" class="LblNrmlB">Select a page:</span>
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
                                                    <tr id="trListViewRow" runat="server" style="width: 90px;" align="center" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                        <td align="center">
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </td>
                                                        <td align="left" class="paddingLSML" style="width: 450px">
                                                            <asp:Label ID="lblTask" runat="server" Text='<%# Eval("RegBankDetails.RegisterdBankName") %>' />
                                                        </td>
                                                        <td align="center" style="width: 300px">
                                                            <asp:Label ID="lblStartDateTime" runat="server" Text='<%# Eval("StartDateTime","{0:dd-MMM-yyyy hh:mm tt}") %>' />
                                                        </td>
                                                        <td align="center" style="width: 300px">
                                                            <asp:Label ID="lblEndDateTime" runat="server" Text='<%# Eval("EndDateTime","{0:dd-MMM-yyyy hh:mm tt}") %>' />
                                                        </td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ToolTip="Edit" CommandName="UpdateCommand"
                                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="imgBtnDelete" ToolTip="Delete" runat="server" CommandName="RemoveCommand"
                                                                OnClientClick="if(!ConfirmRemove()) {return false;}" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                            <%--we cant access object.Property in the Datakey.. Hence we have taken HIDDEN variable......--%>
                                                            <asp:HiddenField ID="hidSchoolwiseBankId" runat="server" Value='<%# Eval("RegBankDetails.NetBankingBankId") %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="LblNoRecord" align="center">
                                                                No Records Found.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                            <%--<asp:ObjectDataSource TypeName="BusinessLogic.SchoolwiseBankMasterBL" EnablePaging="true"
                                                        ID="lstvwDsObj" runat="server" SelectMethod="GetDisabledBankDetails" SelectCountMethod="CountDisabledBankDetails"
                                                        EnableCaching="false">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>--%>
                                            <asp:HiddenField ID="hidIsNewOrIsExisting" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidRowId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidPageNo" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidBlockedBanksJSON" runat="server" Value="0" />
                                        </td>
                                    </tr>
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
            <td align="center">
                <asp:Button CssClass="ClsBtn" ID="btnBack" runat="server" Text="Back"
                    CausesValidation="False" EnableViewState="false" UseSubmitBehavior="false" PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx" />&nbsp;
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        _ClienttxtStartTime = "#<%=this.txtStartTime.ClientID%>";
        _ClienttxtEndTime = "#<%=this.txtEndTime.ClientID%>";
        _ClienttxtStartDate = "#<%=this.txtStartDate.ClientID%>";
        _ClienttxtEndDate = "#<%=this.txtEndDate.ClientID%>";
        _ClientlblErrorMsg = "#<%=this.lblErrorMsg.ClientID%>";
        _ClientlblSuccessMsg = "#<%=this.lblSuccessMsg.ClientID%>";
        _ClienthidIsNewOrIsExisting = "#<%=this.hidIsNewOrIsExisting.ClientID%>";

        function ClearMessages() {
            $get("<%=this.lblErrorMsg.ClientID%>").innerHTML = "";
            $get("<%=this.lblSuccessMsg.ClientID%>").innerHTML = "";
        }

        function ValidateStartTime(oSrc, args) {
            ClearMessages()
            args.IsValid = true;
            var sStrtTime = $.trim($get("<%=this.txtStartTime.ClientID%>").value);
            if (sStrtTime == "") {
                oSrc.errormessage = "Start time should not be blank.";
                args.IsValid = false;
            }
            else
                if (!isTimeValid(_ClienttxtStartTime)) {
                    oSrc.errormessage = "Please enter valid start time e.g. 10:10 AM.";
                    args.IsValid = false;
                }
            return args.IsValid;
        }

        function ValidateEndTime(oSrc, args) {
            args.IsValid = true;
            var sEndTime = $.trim($(_ClienttxtEndTime).val());
            var sEndDate = $.trim($(_ClienttxtEndDate).val())
            if (sEndDate != "" && sEndTime == "") {
                oSrc.errormessage = "End time should not be blank.";
                args.IsValid = false;
            }
            else if (sEndTime != "" && !isTimeValid(_ClienttxtEndTime)) {
                oSrc.errormessage = "Please enter valid end time e.g. 04:10 PM.";
                args.IsValid = false;
            }
            return args.IsValid;
        }
        function ValidateEndDate(oSrc, args) {
            args.IsValid = true;
            var sStrtDate = $.trim($(_ClienttxtStartDate).val());   
            var sEndTime = $.trim($(_ClienttxtEndTime).val());
            var sEndDate = $.trim($(_ClienttxtEndDate).val());
            if (sEndDate == "" && sEndTime != "") {
                oSrc.errormessage = "End date should not be blank.";
                args.IsValid = false;
            }
            else if (sStrtDate != "" && sEndDate != "") {
                sStrtDate = GetFormattedDate(sStrtDate);
                sEndDate = GetFormattedDate(sEndDate);

                if (sStrtDate > sEndDate) {
                    oSrc.errormessage = "End date should greater than Start date.";
                    args.IsValid = false;
                }
            }
            return args.IsValid;
        }

        function ValidateStartEndDate(oSrc, args) {
            args.IsValid = true;
            var sStrtDate = $.trim($(_ClienttxtStartDate).val());
            var sStrtTime = $.trim($(_ClienttxtStartTime).val());
            if (sStrtDate == "") {
                oSrc.errormessage = "Start date should not be blank.";
                args.IsValid = false;
            }
            if ($(_ClienthidIsNewOrIsExisting).val() == 0) {
                if (sStrtDate != "") {
                    sStrtDate = GetFormattedDate(sStrtDate + " " + sStrtTime)
                    var today = new Date();
                    today.setSeconds(00);
                    today.setMilliseconds(00);
                    sStrtDate.setMilliseconds(00);
                    // If current date time is greater than start date
                    if (sStrtDate < today) {
                        oSrc.errormessage = "Start date time should be greater than or equal to the current date time.";
                        args.IsValid = false;
                    }
                }
            }
            return args.IsValid;
        }

        function ValidateTimeRange(oSrc, args) {
            args.IsValid = true;
            var sStrtDate = $.trim($(_ClienttxtStartDate).val());
            var sEndDate = $.trim($(_ClienttxtEndDate).val());
            var sStrtTime = $.trim($(_ClienttxtStartTime).val());
            var sEndTime = $.trim($(_ClienttxtEndTime).val());

            if (sStrtDate != "" && sEndDate != "" && sStrtTime != "" && sEndTime != "") {
                sStrtDate = GetFormattedDate(sStrtDate + " " + sStrtTime);
                sEndDate = GetFormattedDate(sEndDate + " " + sEndTime);
                if (sStrtDate >= sEndDate) {
                    oSrc.errormessage = "End date time should be greater than start date time.";
                    args.IsValid = false;
                }
            }
            return args.IsValid;
        }

        function ConfirmRemove() {
            var bResult = true;
            if (!window.confirm('Are you sure you want to delete this record?'))
                bResult = false;
            return bResult;
        }

        function isTimeValid(result) {
            var timeStr = $.trim($(result).val());
            if (trimAll(timeStr) == '')
                return false;
            // Regular expression to validate date

            var timePat = /^(\d{1,2}):(\d{2})(\s)(am|pm)$/;
            var matchArray = (timeStr.toLowerCase()).match(timePat);

            if (matchArray == null)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            ampm = matchArray[4];

            if (ampm == "") { ampm = null; }
            if (hour < 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + '0' + minute;
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            $(result).val(str);
            return true;
        }
    </script>
</asp:Content>
