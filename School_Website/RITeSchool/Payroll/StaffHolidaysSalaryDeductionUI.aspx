<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StaffHolidaysSalaryDeductionUI.aspx.cs" Inherits="StaffHolidaysSalaryDeductionUI" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" width="85%">
            <tr align="center" id="trValSummary" runat="server">
                <td align="center" id="trValidationSummary">
                    <asp:ValidationSummary ID="valSumHolidayConfig" CssClass="LblErrorMsg" ShowSummary="true"
                        runat="server" />
                    <asp:CustomValidator ID="cstvalValidateHoliday" runat="server" ClientValidationFunction="ValidateHoliday"
                        SetFocusOnError="True" Display="None" ErrorMessage="Holiday name should not be duplicate."></asp:CustomValidator>
                    <asp:CustomValidator ID="cstvalDuplicateValue" runat="server" ClientValidationFunction="DuplicateValue"
                        SetFocusOnError="True" Display="None" ErrorMessage="Holiday name should not be duplicate."></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValidateDates" runat="server" ClientValidationFunction="cstValidateDates"
                        Display="None"></asp:CustomValidator>
                    <asp:CustomValidator ID="cst_StartAndEndDate" runat="server" ClientValidationFunction="cstStartAndEndDate"
                        Display="None"></asp:CustomValidator>
                    <asp:CustomValidator ID="cst_AcademicYear" runat="server" ClientValidationFunction="cstValidateAcademicYear"
                        Display="None"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstvalCheckDateOverlapping" runat="server" ClientValidationFunction="CheckDateOverlapping"
                        Display="None"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValPercentage" runat="server" ClientValidationFunction="ValidatePercentage"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValCheckPercentage" runat="server" ClientValidationFunction="CheckPercentage"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateType"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateLeaveType"
                        SetFocusOnError="True" Display="None" ErrorMessage="Leave type for weekend should be selected."></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="ValidateWeekendPercentage"
                        SetFocusOnError="True" Display="None" ErrorMessage="Percentage should not be blank for weekend."></asp:CustomValidator>
                    <div style="float: right; vertical-align: top;">
                        <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                    </div>
                </td>
            </tr>
            <tr align="center" width="100%">
                <td align="center" width="100%">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                    Font-Bold="True" ForeColor="Blue" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trErrorMessage" runat="server">
                <td align="left" width="100%">
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" EnableViewState="False"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="center" width="230px">
                                <asp:CheckBox ID="chkWeekend" runat="server" Text="Consider Weekend as Holiday?" CssClass="ClsLabel"
                                    onclick="IsWeekend()" />
                            </td>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel">Type : </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbLeaveType" runat="server" CssClass="MidCombo">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                            </td>
                            <td class="ClsBorderlight" width="180px">
                                <span class="ClsLabel">Percentage to Deduct Leave : </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPercentage" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,2,false);"
                                    MaxLength="5" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table align="center" width="100%">
                        <tr>
                            <td>
                                <asp:ListView ID="lstHolidays" runat="server" DataKeyNames="StaffHolidaysSalaryDeductionId"
                                    OnItemDataBound="lstHolidays_ItemDataBound">
                                    <LayoutTemplate>
                                        <table width="100%" runat="server" id="tblHoliday" style="color: #333333" cellpadding="0"
                                            cellspacing="1" class="GridBorder">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th>
                                                </th>
                                                <th>
                                                    Sr. No.
                                                </th>
                                                <th align="left" style="padding-left: 10px;">
                                                    Holiday Name
                                                </th>
                                                <th align="center">
                                                    Holiday Start Date
                                                </th>
                                                <th align="center">
                                                    Holiday End Date
                                                </th>
                                                <th align="right" style="padding-right: 10px;" visible="false">
                                                    Total Day(s)
                                                </th>
                                                <th>
                                                    Type
                                                </th>
                                                <th align="left" style="padding-left: 10px;">
                                                    Percentage to Deduct Leave
                                                </th>
                                                <th align="left" style="padding-left: 10px;">
                                                    Select Holiday
                                                </th>
                                            </tr>
                                            <tr id="ItemPlaceHolder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="tr2" runat="server" class="ClsGridRow">
                                            <td align="center">
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblSrNo" runat="server"></asp:Label>
                                            </td>
                                            <td style="padding-left: 10px;">
                                                <asp:TextBox ID="txtHolidayName" runat="server" Text='<%#Eval("HolidayName") %>'
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtStartDate" CssClass="SmlCombo" runat="server" MaxLength="15"
                                                    Text='<%#Eval("HolidayStartDate","{0:dd-MMM-yyyy}")%>'></asp:TextBox><rjs:PopCalendar
                                                        ID="cStartDate" runat="server" Control="txtStartDate" Format="dd mmm yyyy" ShowErrorMessage="false"
                                                        ShowWeekend="true" InvalidDateMessage="Start date should be in the valid format." />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEndDate" CssClass="SmlCombo" runat="server" MaxLength="15" Text='<%#Eval("HolidayEndDate","{0:dd-MMM-yyyy}")%>'></asp:TextBox><rjs:PopCalendar
                                                    ID="cEnddate" runat="server" Control="txtEndDate" Format="dd mmm yyyy" ShowErrorMessage="false"
                                                    ShowWeekend="true" InvalidDateMessage="End date should be in the valid format." />
                                            </td>
                                            <td align="right" style="padding-right: 10px;" visible="false">
                                                <asp:Label ID="lblDays" runat="server" Text='<%#Eval("Days") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="cmbType" runat="server" CssClass="MidCombo">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" style="padding-left: 10px;">
                                                <asp:TextBox ID="txtPercentage" runat="server" Text='<%#Eval("PercentageToDeduct") %>' MaxLength="5"
                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <img src="../images/selection5.gif" alt="Select Holiday" id='<%# Container.DisplayIndex %>_select'
                                                    title="Select Holiday" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="tr3" runat="server" class="ClsGridAltRow">
                                            <td align="center">
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblSrNo" runat="server"></asp:Label>
                                            </td>
                                            <td style="padding-left: 10px;">
                                                <asp:TextBox ID="txtHolidayName" runat="server" Text='<%#Eval("HolidayName") %>'
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtStartDate" CssClass="SmlCombo" runat="server" MaxLength="15"
                                                    Text='<%#Eval("HolidayStartDate","{0:dd-MMM-yyyy}")%>'></asp:TextBox><rjs:PopCalendar
                                                        ID="cStartDate" runat="server" Control="txtStartDate" Format="dd mmm yyyy" ShowErrorMessage="false"
                                                        ShowWeekend="true" InvalidDateMessage="please select valid date." />
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtEndDate" CssClass="SmlCombo" runat="server" MaxLength="15" Text='<%#Eval("HolidayEndDate","{0:dd-MMM-yyyy}")%>'></asp:TextBox><rjs:PopCalendar
                                                    ID="cEnddate" runat="server" Control="txtEndDate" Format="dd mmm yyyy" ShowErrorMessage="false"
                                                    ShowWeekend="true" InvalidDateMessage="please select valid date." />
                                            </td>
                                            <td align="right" style="padding-right: 10px;" visible="false">
                                                <asp:Label ID="lblDays" runat="server" Text='<%#Eval("Days") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="cmbType" runat="server" CssClass="MidCombo">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" style="padding-left: 10px;">
                                                <asp:TextBox ID="txtPercentage" runat="server" Text='<%#Eval("PercentageToDeduct") %>' MaxLength="5"
                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <img src="../images/selection5.gif" alt="" id='<%# Container.DisplayIndex %>_select'
                                                    title="Select Holiday" />
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
                    <table align="left" width="100%">
                        <tr>
                            <td align="left" class="ClsBorderlight " style="width: 8%; background-color: #ffffc4;">
                                <span id="spnNote" class="LblNrmlB" style="font-weight: bold">Note 1 :</span>
                            </td>
                            <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                <asp:Label ID="lblVerifyNote1" runat="server" BorderWidth="0px" CssClass="LblSmlV"
                                    Text="If already configured holiday / weekend details are unchecked and saved, then unchecked holiday / weekend details will be deleted."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight " style="width: 8%; background-color: #ffffc4;">
                                <span id="Span1" class="LblNrmlB" style="font-weight: bold">Note 2 :</span>
                            </td>
                            <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                <asp:Label ID="Label1" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If user consumes a leave attached to the configured holiday / weekend or leaves those enclose configured holiday / weekend, then the configured percentage of the holiday / weekend period will be considered as an unpaid leave(s)."></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidAcademicYearStartDate" runat="server" />
                    <asp:HiddenField ID="hidAcademicYearEndDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <table align="center" width="10%">
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" disable-page="true" />
                            </td>
                            <td align="center">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="divHoliday" runat="server" style="visibility: hidden; display: none; position: absolute;
        margin: 0px; padding: 0px; width: 760px; height: 430px; border-width: 1px; left: 5px;
        top: 150px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
        background-color: white;">
        <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
            background-repeat: repeat-x; color: Black; width: 760px; text-align: right">
            <div style="font-size: 12px; width: 350px; letter-spacing: 1px; padding-left: 8px;
                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                Select Holiday
            </div>
            <span style="cursor: hand" onclick="javascript:HidePopup();">
                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                    border="0" />
            </span>
        </div>
        <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
            color: #333; overflow: auto; height: 380px; width: 750px; margin-left: 1px" id="Div5">
            <table>
                <tr>
                    <td colspan="2">
                        <asp:ListView ID="lstvwHoliday" runat="server">
                            <LayoutTemplate>
                                <table align="center" width="710px" runat="server" id="tblStaffInfo" style="color: #333333"
                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th style="text-align:left; padding-left:10px;">
                                            Holiday
                                        </th>
                                        <th>
                                            Start Date
                                        </th>
                                        <th>
                                            End Date
                                        </th>
                                        <th>
                                            Select
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                    <td align="left" class="paddingL">
                                        <asp:Label ID="lblHolidayName" runat="server" Text='<%# Eval("Holiday_Name") %>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("Holiday_Start_Date","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("Holiday_End_Date","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <a id='<%# Container.DisplayIndex %>_lnkbtnSubmit' style="color: Blue; cursor: pointer;
                                            text-decoration: underline" onclick="OpenStaffHolidayPopup(this);">Select</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                    <td align="left" class="paddingL">
                                        <asp:Label ID="lblHolidayName" runat="server" Text='<%# Eval("Holiday_Name") %>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("Holiday_Start_Date","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("Holiday_End_Date","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                    </td>
                                    <td align="center">
                                        <a id='<%# Container.DisplayIndex %>_lnkbtnSubmit' style="color: Blue; cursor: pointer;
                                            text-decoration: underline" onclick="OpenStaffHolidayPopup(this);">Select</a>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table width="740px" align="center">
                                    <tr>
                                        <td class="LblNoRecord" style="text-align: center">
                                            <span>No record found.</span>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:ListView>
                        <asp:HiddenField ID="hidHolidayId" runat="server" />
                        <asp:HiddenField ID="hidStaffHolidaysLeaveDeductionId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="bottom">
                        <asp:Button ID="btnClosePopUp" runat="server" Text="Close" CssClass="ClsBtnMid" CausesValidation="false"
                            Width="75px" OnClientClick="javascript:HidePopup();return false;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        _clientHolidaysListview = "<%=this.lstHolidays.ClientID %>"
        _clientcst_StartAndEndDate = "<%=this.cst_StartAndEndDate.ClientID %>";
        _ClientcstvalDuplicateValue = "<%=this.cstvalDuplicateValue.ClientID %>"
        _clientcstvalValidateHoliday = "<%=this.cstvalValidateHoliday.ClientID %>";
        _clientcstValPercentage = "<%=this.cstValPercentage.ClientID %>";
        _clientcstValCheckPercentage = "<%=this.cstValCheckPercentage.ClientID %>";
        _clientcstvalCheckDateOverlapping = "<%=this.cstvalCheckDateOverlapping.ClientID %>";
        _clienthidStaffHolidaysLeaveDeductionId = "<%=this.hidStaffHolidaysLeaveDeductionId.ClientID %>";
        _clientLblErrorMessage = "<%=this.lblErrorMessage.ClientID %>";
        _ClientvlblMessage = "<%=this.lblMessage.ClientID %>";
        _clienthidAcademicYearStartDate = "<%=this.hidAcademicYearStartDate.ClientID %>";
        _clienthidAcademicYearEndDate = "<%=this.hidAcademicYearEndDate.ClientID %>";
        _clientcstValidateDates = "<%=this.cstValidateDates.ClientID %>";
        _clientcst_AcademicYear = "<%=this.cst_AcademicYear.ClientID %>";
        _clientchkWeekend = "<%=this.chkWeekend.ClientID %>";

        function IsTextChange(Id) {
            var Ids = document.getElementById(_clienthidStaffHolidaysLeaveDeductionId).value;
            document.getElementById(_clienthidStaffHolidaysLeaveDeductionId).value = Ids + "," + Id;

        }

        function SelectAllControls(objid, ListIndex) {

            DisableControls(objid.checked, ListIndex);

        }

        SetView();
        function SetView() {
            var ListIndex = 0;
            if (ListIndex < 10)
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_chkSelect")
            else
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_chkSelect")

            while (chk != null) {

                DisableControls(chk.checked, ListIndex);
                ListIndex = ListIndex + 1;
                if (ListIndex < 10)
                    chk = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_chkSelect")
                else
                    chk = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_chkSelect")
            }
        }

        function DisableControls(action, ListIndex) {
            var txtHolidayName = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_txtHolidayName");
            var txtStartDate = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_txtStartDate");
            var txtEndDate = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_txtEndDate");
            var txtPercentage = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_txtPercentage");
            var cEnddate = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_cEnddate");
            var cStartDate = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_cStartDate");
            var cmbType = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_cmbType");
            var imgSelect = document.getElementById(ListIndex + "_select");
            
            txtHolidayName.disabled = !action;
            txtStartDate.disabled = !action;
            txtEndDate.disabled = !action;
            txtPercentage.disabled = !action;
            cEnddate.disabled = !action;
            cStartDate.disabled = !action;
            cmbType.disabled = !action;

            var handler = function () {
                OpenPopup(imgSelect);
            }
            
            if (action) {
                imgSelect.src = "../images/selection5.gif";
                $(imgSelect).bind('click', handler);

            }
            else {
                imgSelect.src = "../images/disableselect.gif";
                $(imgSelect).unbind();
            }

        }

        function cstValidateDates(oSrc, args) {
            var sMsg = "";
            var isValid = true;
            var chk
            var iRow = 0;
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_chkSelect")
            while (chk != null) {
                if (chk.checked) {
                    var HolidyStartDate = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_txtStartDate").value.trim();
                    var HolidyEndDate = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_txtEndDate").value.trim();

                    if (HolidyStartDate == "" || HolidyEndDate == "")
                        sMsg = sMsg + (iRow + 1) + ", ";
                }
                iRow = iRow + 1;
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_chkSelect")
            }

            if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                $get(_clientcstValidateDates).errormessage = "Start Date & End date should not be blank for rows(s) : " + sMsg + ".";
                $get(_clientLblErrorMessage).style.display = 'block';
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function cstValidateAcademicYear(oSrc, args) {
            var sMsg = "";
            var isValid = true;
            var chk
            var iRow = 0;
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_chkSelect")
            while (chk != null) {
                if (chk.checked) {
                    var HolidyStartDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_txtStartDate").value.trim());
                    var HolidyEndDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_txtEndDate").value.trim());
                    var AcademicStartDate = getDate($get(_clienthidAcademicYearStartDate).value);
                    var AcademicEndDate = getDate($get(_clienthidAcademicYearEndDate).value);

                    if ((HolidyStartDate < AcademicStartDate) || (HolidyStartDate > AcademicEndDate) || (HolidyEndDate < AcademicStartDate) || (HolidyEndDate > AcademicEndDate))
                        sMsg = sMsg + (iRow + 1) + ", ";
                }
                iRow = iRow + 1;
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_chkSelect");
            }

            if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                $get(_clientcst_AcademicYear).errormessage = "Start Date & End date should be within the current academic year ( " + $get(_clienthidAcademicYearStartDate).value + " To " + $get(_clienthidAcademicYearEndDate).value + " ) for rows(s) : " + sMsg + ".";
                $get(_clientLblErrorMessage).style.display = 'block';
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function cstStartAndEndDate(oSrc, args) {
            var dtStartDate;
            var dtEndDate;
            var sMsg = "", sMsg2 = "";
            var isValid = true;
            var chk
            var i = 1;
            var iRow = 0;
            var iPercent = "";
            var sHolidayName = "";

            if (i < 10)
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_chkSelect")
            else
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_chkSelect")
            while (chk != null) {
                if (chk.checked) {
                    var HolidyStartDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_txtStartDate").value.trim());
                    var HolidyEndDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_txtEndDate").value.trim());
                    var iPercentage = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_txtPercentage").value;
                    var txtHolidayName = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_txtHolidayName");
                    if (txtHolidayName.value.trim() == "") {
                        sHolidayName = sHolidayName + i + ", ";
                    }

                    if (HolidyStartDate != null && HolidyEndDate != null && HolidyStartDate != "NaN" && HolidyEndDate != "NaN" && HolidyStartDate != "Invalid Date" && HolidyEndDate != "Invalid Date") {
                        if (!(HolidyStartDate <= HolidyEndDate))
                            sMsg = sMsg + i + ", ";
                    }
                    if (iPercentage > 100.00)
                        iPercent = iPercent + i + ", ";
                }
                i = i + 1;
                iRow = iRow + 1;
                if (i < 10)
                    chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_chkSelect")
                else
                    chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRow + "_chkSelect")
            }
            if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                document.getElementById(_clientcst_StartAndEndDate).errormessage = "End date should be greater than start date for row(s) : " + sMsg;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function getDate(obj) {
            var strDate = obj.replace('-', ' ').replace('-', ' ');
            return new Date(strDate);
        }

        function CheckDateOverlapping(oSrc, args) {
            var chk
            var sDuplicate = false;
            var duplicateMessage = "";
            var iRowCount = 0;
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var HolidyStartDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_txtStartDate").value.trim());
                    var HolidyEndDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_txtEndDate").value.trim());

                    var i_RowCount = iRowCount + 1

                    var chk_next = document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_chkSelect")
                    while (chk_next != null) {

                        if (chk_next.checked == true) {
                            var next_HolidyStartDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_txtStartDate").value.trim());
                            var next_HolidyEndDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_txtEndDate").value.trim());

                            if ((HolidyStartDate >= next_HolidyStartDate && HolidyStartDate <= next_HolidyEndDate) || (HolidyEndDate >= next_HolidyStartDate && HolidyEndDate <= next_HolidyEndDate) ||
                                 (next_HolidyStartDate >= HolidyStartDate && next_HolidyStartDate <= HolidyEndDate) || (next_HolidyEndDate >= HolidyStartDate && next_HolidyEndDate <= HolidyEndDate) ||
                                 ((next_HolidyStartDate >= HolidyStartDate && next_HolidyStartDate <= HolidyEndDate) && (next_HolidyEndDate >= HolidyStartDate && next_HolidyEndDate <= HolidyEndDate)) ||
                                 ((HolidyStartDate >= next_HolidyStartDate && HolidyStartDate <= next_HolidyEndDate) && (HolidyEndDate >= next_HolidyStartDate && HolidyEndDate <= next_HolidyEndDate))) {
                                sDuplicate = true;
                                if (duplicateMessage.match(', ' + (i_RowCount + 1)) == null)
                                    duplicateMessage = duplicateMessage + ", " + (i_RowCount + 1);
                                break;
                            }
                        }
                        i_RowCount = i_RowCount + 1;

                        chk_next = document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_chkSelect")
                    }

                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            }
            if (sDuplicate) {
                duplicateMessage = duplicateMessage.substring(1);
                document.getElementById(_clientcstvalCheckDateOverlapping).errormessage = "Holiday dates should not be overlapped for row(s): " + duplicateMessage;
                document.getElementById(_clientcstvalCheckDateOverlapping).innerHTML = "Holiday dates should not be overlapped for row(s): " + duplicateMessage;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        var Page_IsValid = true;
        function ConfirmToSave() {
        	Page_IsValid = true;
            var bResult = true
            var IsFound  = false
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    IsFound = true;
                    break;
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            }
            if (!IsFound && $get(_clientchkWeekend).checked == false) {
            	window.alert('At least one holiday configuration or weekend configuration should be selected.')
            	Page_IsValid = false;
                    bResult = false
            }
            if (bResult) {
                document.getElementById(_clientLblErrorMessage).style.display = 'none';
                document.getElementById(_clientLblErrorMessage).innerHTML = '';
                document.getElementById(_ClientvlblMessage).innerHTML = "";
            }
            return bResult
        }

        function GetTotalDays(ListIndex) {
            var HolidyStartDate = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_txtStartDate").value.trim()
            var HolidyEndDate = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_txtEndDate").value.trim()
            var dtStartDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_txtStartDate").value)
            var dtEndDate = getDate(document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_txtEndDate").value)
            var chk = document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_ChkSelect")
            if (chk.checked) {
                if (HolidyStartDate == HolidyEndDate)
                    document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_lblDays").innerHTML = "1"
                else
                    document.getElementById(_clientHolidaysListview + "_ctrl" + ListIndex + "_lblDays").innerHTML = ((dtEndDate - dtStartDate) / (3600 * 24000)) + 1

            }
        }

        function ValidateHoliday(oSrc, args) {
            var bResult = true
            var chk
            var iRowCount = 0
            var emptyMesage = "";
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var txtHolidayName = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_txtHolidayName")
                    if (txtHolidayName.value.trim() == "")
                        emptyMesage = emptyMesage + ", " + (iRowCount + 1);
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            }
            if (emptyMesage.length > 1) {
                emptyMesage = emptyMesage.substring(1);
                document.getElementById(_clientcstvalValidateHoliday).errormessage = "Holiday name should not be blank for row(s): " + emptyMesage;
                document.getElementById(_clientcstvalValidateHoliday).innerHTML = "Holiday name should not be blank for row(s): " + emptyMesage;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                document.getElementById(_ClientvlblMessage).innerHTML = "";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }


        function ValidatePercentage(oSrc, args) {

            var bResult = true
            var chk
            var iRowCount = 0
            var emptyMesage = "";
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iPercentage = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_txtPercentage").value.trim();
                    if (iPercentage == "")
                        emptyMesage = emptyMesage + ", " + (iRowCount + 1);
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            }
            if (emptyMesage.length > 1) {
                emptyMesage = emptyMesage.substring(1);
                document.getElementById(_clientcstValPercentage).errormessage = "Percentage should not be blank for row(s): " + emptyMesage;
                document.getElementById(_clientcstValPercentage).innerHTML = "Percentage should not be blank for row(s): " + emptyMesage;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                document.getElementById(_ClientvlblMessage).innerHTML = "";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ValidateType(oSrc, args) {
            var bResult = true
            var chk
            var iRowCount = 0
            var emptyMesage = "";
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var type = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_cmbType").value;
                    if (type == 0)
                        emptyMesage = emptyMesage + ", " + (iRowCount + 1);
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            }

            if (emptyMesage.length > 1) {
                emptyMesage = emptyMesage.substring(1);
                oSrc.errormessage = "Type should be selected for row(s): " + emptyMesage;
                oSrc.innerHTML = "Type should be selected for row(s): " + emptyMesage;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                document.getElementById(_ClientvlblMessage).innerHTML = "";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function CheckPercentage(oSrc, args) {

            var bResult = true
            var chk
            var iRowCount = 0
            var emptyMesage = "";
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iPercentage = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_txtPercentage").value.trim();
                    if (iPercentage != "" && parseFloat(iPercentage) > 100)
                        emptyMesage = emptyMesage + ", " + (iRowCount + 1);
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            }
            if (emptyMesage.length > 1) {
                emptyMesage = emptyMesage.substring(1);
                document.getElementById(_clientcstValCheckPercentage).errormessage = "Percentage should not be greater than 100 for row(s): " + emptyMesage;
                document.getElementById(_clientcstValCheckPercentage).innerHTML = "Percentage should not be greater than 100 for row(s): " + emptyMesage;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                document.getElementById(_ClientvlblMessage).innerHTML = "";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function DuplicateValue(oSrc, args) {
            var chk
            var sDuplicate = false;
            var duplicateMessage = "";
            var iRowCount = 0;
            chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var txtsTextBox = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_txtHolidayName")

                    txtstartDate = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_txtStartDate")
                    txtEndDate = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_txtEndDate")

                    var i_RowCount = iRowCount + 1

                    var chk_next = document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_chkSelect")
                    while (chk_next != null) {

                        if (chk_next.checked == true) {
                            txt_next_sTextBox = document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_txtHolidayName")

                            txt_next_startDate = document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_txtStartDate")
                            txt_next_EndDate = document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_txtEndDate")

                            var upper_txtsTextBox = txtsTextBox.value.trim();
                            var upper_txt_next_sTextBox = txt_next_sTextBox.value.trim();

                            var upperStartDate = txtstartDate.value.trim();
                            var upperNextStartDate = txt_next_startDate.value.trim();

                            var upperEndDate = txtEndDate.value.trim();
                            var upperNextStartDate = txt_next_EndDate.value.trim();

                            if (upper_txtsTextBox.toUpperCase() != "" && upper_txt_next_sTextBox.toUpperCase() != "") {
                                if (upper_txtsTextBox.toUpperCase() == upper_txt_next_sTextBox.toUpperCase() &&
                                    upperStartDate.toUpperCase() == upperNextStartDate.toUpperCase() &&
                                    upperEndDate.toUpperCase() == upperNextStartDate.toUpperCase()) {
                                    sDuplicate = true;
                                    duplicateMessage = duplicateMessage + ", " + (i_RowCount + 1);
                                    break;
                                }
                            }
                        }
                        i_RowCount = i_RowCount + 1;

                        chk_next = document.getElementById(_clientHolidaysListview + "_ctrl" + i_RowCount + "_chkSelect")
                    }

                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientHolidaysListview + "_ctrl" + iRowCount + "_chkSelect")
            }

            if (sDuplicate) {
                duplicateMessage = duplicateMessage.substring(1);
                document.getElementById(_ClientcstvalDuplicateValue).errormessage = "Holiday name should not be duplicate for row(s): " + duplicateMessage;
                document.getElementById(_ClientcstvalDuplicateValue).innerHTML = "Holiday name should not be duplicate for row(s): " + duplicateMessage;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                document.getElementById(_ClientvlblMessage).innerHTML = "";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        IsWeekend();
        function IsWeekend() {
            var cmbType = document.getElementById("<%=this.cmbLeaveType.ClientID %>");
            var percentage = document.getElementById("<%=this.txtPercentage.ClientID %>");
            var isweekend = $get(_clientchkWeekend).checked;
            cmbType.disabled = !isweekend;
            percentage.disabled = !isweekend;
            if (!isweekend) {
                cmbType.value = "0";
                percentage.value = "0";
            }
        }

        function ValidateLeaveType(oSrc, args) {
           var chkWeekend = document.getElementById(_clientchkWeekend);
           var cmbType = document.getElementById("<%=this.cmbLeaveType.ClientID %>");
            if (chkWeekend.checked && cmbType.value == "0") {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        
        function ValidateWeekendPercentage(oSrc, args) {
            var chkWeekend = document.getElementById(_clientchkWeekend);
            var txtPercentage = document.getElementById("<%=this.txtPercentage.ClientID %>");
            if (chkWeekend.checked) {
                if (txtPercentage.value == "") {
                    oSrc.errormessage = "Percentage should not be blank for weekend.";
                    args.IsValid = false;
                    return true;
                }
                else if (parseInt(txtPercentage.value) > 100) {
                    oSrc.errormessage = "Percentage should not be greater than 100 for weekend.";
                    args.IsValid = false;
                    return true;
                }
            }
            
            args.IsValid = true;
            return false;
        }


        var txtEndDate;
        var txtStartDate;
        var txtHolidayName;
        var lblSrNo;
        function OpenPopup(obj) {            
            var row = $(obj).closest('tr').get(0);
            txtEndDate = $('input[type=text][id$=txtEndDate]', row);
            txtStartDate = $('input[type=text][id$=txtStartDate]', row);
            txtHolidayName = $('input[type=text][id$=txtHolidayName]', row);
            lblSrNo = $('span[id$=lblSrNo]', row);

            _clientdivTemplates = "<%=this.divHoliday.ClientID %>"
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divHoliday.ClientID %>").style
            var width = 750
            var height = 380
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"

        }

        function OpenStaffHolidayPopup(obj) {

            var row = $(obj).closest('tr').get(0);
            var lblEndDate = $('span[id$=lblEndDate]', row);
            var lblStartDate = $('span[id$=lblStartDate]', row);
            var lblHolidayName = $('span[id$=lblHolidayName]', row);


            txtEndDate.val(lblEndDate[0].innerHTML);

            txtStartDate.val(lblStartDate[0].innerHTML);

            txtHolidayName.val(lblHolidayName[0].innerHTML);

            var Ids = document.getElementById(_clienthidStaffHolidaysLeaveDeductionId).value;
            document.getElementById(_clienthidStaffHolidaysLeaveDeductionId).value = Ids + "," + (parseInt(lblSrNo[0].innerHTML) - 1);

            var cssstyle = $get("<%=this.divHoliday.ClientID %>").style
            cssstyle.visibility = "hidden";
            cssstyle.display = "none";

        }

        function HidePopup() {
            $get("<%=this.divHoliday.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divHoliday.ClientID %>").style.display = "none"
            return false
        }

    </script>
</asp:Content>
