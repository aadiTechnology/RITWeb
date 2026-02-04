<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RouteShiftTimingUI.aspx.cs" Inherits="RouteShiftTimingUI"
    Title="Untitled Page" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table id="tblRouteStopDetails" runat="server" align="center" border="0" cellpadding="0"
                            cellspacing="1" style="width: 100%; vertical-align: top">
                            <tr>
                                <td id="MainDataTable" align="center">
                                    <!-- Data Insert Here -->
                                    <table id="tblRouteTimingDetails" runat="server" border="0" cellpadding="0" cellspacing="2"
                                        style="height: 100%; width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 77%">
                                                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    Visible="false" Height="20px" Width="100%" CssClass="ClsMdtStar" EnableViewState="false"></asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                            <span class="ClsMdtStar">* Mandatory Fields</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" class="ClsTextNormal" align="center">
                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                    Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>                                       
                                        <tr>
                                            <td align="center">
                                                <!-- User InfoTable starts here -->
                                                <table id="tblRouteTimeDetails" runat="server" border="0" cellpadding="1" cellspacing="2"
                                                    style="width: 95%; margin-left: 19px;">
                                                    <tr id="trOverrideDetailsHeader" runat="server" visible="false">
                                                        <td colspan="8" align="center">
                                                            <span style="font-weight:bold;font-size:large;"><u>Route-Shift-Timing Override Details</u></span>
                                                        </td>
                                                    </tr>
                                                    <tr id="trOverrideDetails" runat="server" visible="false">
                                                        <td align="center" colspan="8">
                                                        <asp:UpdatePanel ID="upnl10" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>                                                            
                                                            <table>
                                                                <tr>
                                                                    <td style="width:150px" class="ClsBorderlight">
                                                                        <span class="ClsLabel">Type : </span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="cmbTypes" runat="server" CssClass="MidCombo" 
                                                                            AutoPostBack="True" onselectedindexchanged="cmbTypes_SelectedIndexChanged">
                                                                            <asp:ListItem Text="Date Range" Value="-1"></asp:ListItem>
                                                                            <asp:ListItem Text="Weekdays" Value="-2"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="width:100px">
                                                                    </td>
                                                                    <td align="left" class="ClsBorderlight" style="width:150px">
                                                                        <span class="ClsLabel">Name :</span>
                                                                    </td>
                                                                    <td align="left" class="ClsMdtStar">
                                                                        <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgCombo" Width="95%" 
                                                                            MaxLength="100"></asp:TextBox>
                                                                        <span class="ClsMdtStar">* </span>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trDates" runat="server">
                                                                    <td align="left" class="ClsBorderlight">
                                                                        <span class="ClsLabel">Start Date :</span>
                                                                    </td>
                                                                    <td align="left" class="ClsMdtStar">
                                                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="cal_Date" runat="server" Control="txtStartDate" Format="dd MMM yyyy" Culture = "en"
                                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Date should not be blank."
                                                                            AutoPostBack="False" From-Today="true" />
                                                                        <span class="ClsMdtStar">* </span>
                                                                    </td>
                                                                    <td style="width:100px">
                                                                    </td>
                                                                    <td align="left" class="ClsBorderlight">
                                                                        <span class="ClsLabel">End Date :</span>
                                                                    </td>
                                                                    <td align="left" class="ClsMdtStar">
                                                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtEndDate" Format="dd MMM yyyy" Culture = "en"
                                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Date should not be blank."
                                                                            AutoPostBack="False" From-Today="true" />
                                                                        <span class="ClsMdtStar">* </span>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trWeekdays" runat="server" visible="false">
                                                                    <td align="left" class="ClsBorderlight">
                                                                        <span class="ClsLabel">Weekdays :</span>
                                                                        <%--<asp:CheckBox ID="chkAll" runat="server" onclick="CheckUnCheckAll(this);" Text="All" />--%>
                                                                    </td>
                                                                    <td align="left" colspan="3">
                                                                        <asp:CheckBoxList ID="chkWeekdays" runat="server" RepeatDirection="Horizontal">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTypes" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        </td>
                                                    </tr>     
                                                    <tr id="trHR" runat="server" visible="false">
                                                        <td colspan="8">
                                                            <hr style="margin-top:10px; margin-bottom:10px;" />
                                                        </td>
                                                    </tr>                                       
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel">Vehicle :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:DropDownList ID="cmbVehicle" runat="server" MaxLength="20" CssClass="MidCombo" Width="95%"
                                                                OnSelectedIndexChanged="cmbVehicle_SelectedIndexChanged" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            *&nbsp;
                                                        </td>
                                                        <td align="left" class="ClsBorderlight">                                                            
                                                            <asp:Label ID="lblRouteName" CssClass="ClsLabel" runat="server" Text="Route Name :"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:DropDownList ID="cmbRouteName" runat="server" MaxLength="20" CssClass="LrgCombo"
                                                                OnSelectedIndexChanged="cmbRouteName_SelectedIndexChanged" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            *
                                                        </td>

                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel">Journey Type :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                             <asp:DropDownList ID="ddlJourneyType" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="ddlJourneyType_SelectedIndexChanged" AutoPostBack="true">
                                                             <asp:ListItem Text="-- Select--" Value = "0"></asp:ListItem>
                                                                <asp:ListItem Text="Pick Up" Value = "1"></asp:ListItem>
                                                                <asp:ListItem Text="Drop" Value = "2"></asp:ListItem>
                                                                </asp:DropDownList>      
                                                            *&nbsp;
                                                        </td>


                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="lblJourney" CssClass="ClsLabel" runat="server" Text="Shift"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:UpdatePanel ID="upnl12" runat="server" UpdateMode="Conditional">   
                                                            <ContentTemplate>                                                     
                                                                <asp:DropDownList ID="cmbShift" runat="server" MaxLength="20" CssClass="MidCombo"
                                                                    OnSelectedIndexChanged="cmbShift_SelectedIndexChanged" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                                *&nbsp;                                                            
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlJourneyType" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                            </asp:UpdatePanel>
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
                                    <div runat="server" id="divErr">
                                    </div>
                                </td>
                            </tr>
                            <!-- User InfoTable ListView -->
                            <tr>
                                <td align="center">
                                    <table id="tblStopTimeDetails" runat="server" align="center" width="70%">
                                        <tr align="center" style="width: 80%">
                                            <td align="center" style="width: 100%">
                                                <div id="divContainer" class="GridBorder" runat="server" visible="true" style="width: 95%;
                                                    height: 300px; overflow: scroll">
                                                    <asp:ListView ID="lstvwStopsTimeDetails" runat="server" 
                                                        DataKeyNames="miRouteStopId,miRouteTimingDetailsId,miRouteShiftVehicleDetailsId" 
                                                        onitemdatabound="lstvwStopsTimeDetails_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" width="25%" style="padding-left: 9px;">
                                                                        <asp:Label ID="lnkBtnSortName" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            EnableViewState="false">Stop Name </asp:Label>
                                                                    </th>
                                                                    <th align="center" width="25%">
                                                                        <asp:Label ID="LinkButton1" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            EnableViewState="false">Order </asp:Label>
                                                                    </th>
                                                                    <th align="center" width="25%" id="thPickUpTime" runat="server">
                                                                        <asp:Label ID="LinkButton2" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            EnableViewState="false">Pickup Time </asp:Label>
                                                                    </th>
                                                                    <th align="center" width="25%" id="thDropTime" runat="server">
                                                                        <asp:Label ID="LinkButton3" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            EnableViewState="false">Drop Time </asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("msStopName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtOrder" runat="server" onkeypress="return blockNonNumbers (this, event, true, true)"
                                                                        MaxLength="2" onkeyup="OnGridKeyUpNumber(this,0,false,event);" Text='<%# Eval("miSortOrder") %>' ></asp:TextBox>
                                                                </td>
                                                                <td align="center" id="tdPickupTime" runat="server">
                                                                    <asp:TextBox ID="txtPickupTime" runat="server" onkeyup="OnGridKeyUpNumber(this,0,false,event);" Text='<%# Eval("msPickupTime") %>'></asp:TextBox>
                                                                </td>
                                                                <td align="center" id="tdDropTime" runat="server">
                                                                    <asp:TextBox ID="txtDropTime" runat="server" Text='<%# Eval("msDropTime") %>' onkeyup="OnGridKeyUpNumber(this,0,false,event);"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td class="paddingL" align="left">
                                                                    <asp:Label ID="lblName" runat="server" onkeyup="OnGridKeyUpNumber(this,0,false,event);" Text='<%# Eval("msStopName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtOrder" runat="server" onkeyup="OnGridKeyUpNumber(this,0,false,event);" onkeypress="return blockNonNumbers (this, event, true, true)"
                                                                        MaxLength="2" Text='<%# Eval("miSortOrder") %>'></asp:TextBox>
                                                                </td>
                                                                <td align="center" id="tdPickupTime" runat="server">
                                                                    <asp:TextBox ID="txtPickupTime" runat="server" onkeyup="OnGridKeyUpNumber(this,0,false,event);" Text='<%# Eval("msPickupTime") %>'></asp:TextBox>
                                                                </td>
                                                                <td align="center" id="tdDropTime" runat="server">
                                                                    <asp:TextBox ID="txtDropTime" runat="server" onkeyup="OnGridKeyUpNumber(this,0,false,event);" Text='<%# Eval("msDropTime") %>'></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trSave" runat="server">
                                <td align="center">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                        CausesValidation="true" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" OnClick="btnDelete_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" />&nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hidRowCount" runat="server" />
                        <asp:HiddenField ID="hidPickupShiftId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidDropShiftId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidId" runat="server" />
                        <asp:HiddenField ID="hidCategoryId" runat="server" />
                        <asp:HiddenField ID="hidRouteNo" runat="server" />
                        <asp:HiddenField ID="hidRouteName" runat="server" />
                        <asp:HiddenField ID="hidVehicleNo" runat="server" />
                        <asp:HiddenField ID="hidJourneyName" runat="server" />
                        <asp:HiddenField ID="hidName" runat="server" />
                        <asp:CustomValidator ID="cstSortOrder" runat="server" ClientValidationFunction="ValidateSortOrder"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        <asp:CustomValidator ID="cstPickupTime" runat="server" ClientValidationFunction="ValidatePickUpTime"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        <asp:CustomValidator ID="cstDropTime" runat="server" ClientValidationFunction="ValidateDropTime"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        <asp:CustomValidator ID="cstPickUpTimeFormat" runat="server" ClientValidationFunction="ValidatePickUpTimeFormat"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        <asp:CustomValidator ID="cstDropTimeFormat" runat="server" ClientValidationFunction="ValidateDropTimeFormat"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="reqValName" runat="server" ErrorMessage="Name should not be blank" Display="None" ControlToValidate="txtName" Enabled="false"></asp:RequiredFieldValidator>                        
                        <asp:CustomValidator ID="cstValEndDate" runat="server" ClientValidationFunction="ValidateDates"
                            SetFocusOnError="True" Display="None" ErrorMessage="" Enabled="false"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstValWeekdays" runat="server" ClientValidationFunction="ValidateWeekdays"
                            SetFocusOnError="True" Display="None" ErrorMessage="" Enabled="false"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstValDates" runat="server" OnServerValidate="NameAndDates_Validate"
                            SetFocusOnError="True" Display="None" ErrorMessage="" Enabled="false"></asp:CustomValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">


        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlstvwStopsTimeDetails = "<%=this.lstvwStopsTimeDetails.ClientID %>"
        _ClientcstSortOrder = "<%=this.cstSortOrder.ClientID %>"
        _ClientcstPickupTime = "<%=this.cstPickupTime.ClientID %>"
        _ClientcstDropTime = "<%=this.cstDropTime.ClientID %>"
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _ClientcstPickUpTimeFormat = "<%=this.cstPickUpTimeFormat.ClientID %>"
        _ClientcstDropTimeFormat = "<%=this.cstDropTimeFormat.ClientID %>"

        _clienttxtStartDate = '<%=this.txtStartDate.ClientID %>'
        _clienttxtEndDate = '<%=this.txtEndDate.ClientID %>'
        _clientcmbTypes = '<%=this.cmbTypes.ClientID %>'

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this records?')) {
                bResult = false
            }
            return bResult
        }

        function isTimeValid(txtTimeId) {

            var timeStr = trimAll(document.getElementById(txtTimeId).value.toUpperCase());
            if (trimAll(timeStr) == '')
                return false;

            // Checks if time is in HH:MM 12 hour format.
            // The seconds are optional.
            var timePat = /^(\d{1,2}):(\d{1,2})?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            if (timeStr.length < 6)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            ampm = matchArray[4];

            if (ampm == "") {
                return false;
            }

            if (hour <= 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + minute + '0';
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(txtTimeId).value = str;
            return true;
        }


        function isTimeValidFor24Hours(txtTimeId) {

            var timeStr = document.getElementById(txtTimeId).value;
            if (trimAll(timeStr) == '')
                return false;

            // Checks if time is in HH:MM:SS 24 hour format.
            // The seconds are optional.   
            var timePat = /^(\d{1,2}):(\d{1,2})(:(\d{1,2}))?((\s?(AM|am|PM|pm)))?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            if (timeStr.length < 3)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            second = matchArray[4];
            ampm = matchArray[6];

            ampm = trimAll(ampm).toUpperCase();
            if (second == "") { second = null; }
            if (ampm == "") { ampm = null; }
            else {
                if (ampm == 'PM' && hour < 12)
                    hour = "" + (parseInt(hour) + 12);
                else if (ampm == 'PM' && hour >= 13)
                    return false;

                if (ampm == 'AM' && hour == 12)
                    hour = "" + (parseInt(hour) - 12);
                else if (ampm == 'AM' && hour >= 13)
                    return false;
            }

            if (hour < 0 || hour > 23)
                return false;
            if (minute < 0 || minute > 59)
                return false;
            if (second != null && (second < 0 || second > 59))
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
            if (second == null)
                str = str + ':' + '00'
            else if (second.length == 1)
                str = str + ':' + '0' + second;
            else
                str = str + ':' + second
            document.getElementById(txtTimeId).value = str;
            return true;
        }

        function ValidateSortOrder(oSrc, args) {
            var iRowCount = 0;
            var sSortOrder = document.getElementById(_clienthidRowCount).value;
            var sLimitMsg = "";
            var sBlankMsg = "";
            var sDuplicateMsg = "";

            var txtOrder = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtOrder")
            while (txtOrder != null) {
                if (txtOrder.value == "" || parseInt(txtOrder.value) == 0)
                    sBlankMsg = sBlankMsg + ", " + (iRowCount + 1);
                else if (parseInt(sSortOrder) < parseInt(txtOrder.value))
                    sLimitMsg = sLimitMsg + ", " + (iRowCount + 1);
                else {

                    var NextRow = iRowCount + 1
                    var nextTxtOrder = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + NextRow + "_txtOrder")
                    while (nextTxtOrder != null) {
                        if (parseInt(txtOrder.value) == parseInt(nextTxtOrder.value)) {
                            sDuplicateMsg = sDuplicateMsg + ", " + +(iRowCount + 1) + "-" + (NextRow + 1);
                        }

                        NextRow = NextRow + 1
                        nextTxtOrder = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + NextRow + "_txtOrder")
                    }
                }

                iRowCount = iRowCount + 1
                txtOrder = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtOrder")

            }

            if (sBlankMsg != "") {
                sBlankMsg = sBlankMsg.substring(1)
                $get(_ClientcstSortOrder).errormessage = "Sort Order should not be blank or zero for row number " + sBlankMsg + "."
                args.IsValid = false
                return true
            }
            else if (sLimitMsg != "") {
                sLimitMsg = sLimitMsg.substring(1)
                $get(_ClientcstSortOrder).errormessage = "Sort Order should not be greater than " + sSortOrder + " for row number: " + sLimitMsg + "."
                args.IsValid = false
                return true
            }
            else if (sDuplicateMsg) {
                sDuplicateMsg = sDuplicateMsg.substring(1)
                $get(_ClientcstSortOrder).errormessage = "Sort Order should not be duplicated for row number : " + sDuplicateMsg + "."
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }


        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            }
        }


        function ValidatePickUpTime(oSrc, args) {
            var iRowCount = 0;
            var sBlankMsg = "";
            var sTimeMsg = "";
            var txtPickupTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtPickupTime")
            var txtDropTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtDropTime")

            if (txtDropTime != null) {
                while (txtPickupTime != null) {

                    var sDate1 = new Date('1/1/2001' + ' ' + txtPickupTime.value);
                    var sDate2 = new Date('1/1/2001' + ' ' + txtDropTime.value);
                    if (txtPickupTime.value == "")
                        sBlankMsg = sBlankMsg + ", " + (iRowCount + 1);
                    else
                        if (sDate1 >= sDate2)
                            sTimeMsg = sTimeMsg + ", " + (iRowCount + 1);

                    iRowCount = iRowCount + 1
                    txtPickupTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtPickupTime")
                    txtDropTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtDropTime")
                }
            }
            else {            
                while (txtPickupTime != null) {
                    if (txtPickupTime.value == "")
                        sBlankMsg = sBlankMsg + ", " + (iRowCount + 1);
                    
                    iRowCount = iRowCount + 1
                    
                    txtPickupTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtPickupTime")                    
                }
            }

            if (sBlankMsg != "") {
                sBlankMsg = sBlankMsg.substring(1)
                $get(_ClientcstPickupTime).errormessage = "Pickup Time should not be blank for row number " + sBlankMsg + "."
                args.IsValid = false
                return true
            }
            else if (sTimeMsg != "") {
                sTimeMsg = sTimeMsg.substring(1)
                $get(_ClientcstPickupTime).errormessage = "Pickup Time should not be equal or greater than drop time for row number " + sTimeMsg + "."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }



        function ValidatePickUpTimeFormat(oSrc, args) {

            var iRowCount = 0;
            var sBlankMsg = "";
            var txtPickupTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtPickupTime")

            while (txtPickupTime != null) {
                if (!isTimeValid(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtPickupTime") && !(txtPickupTime.value.substring(0, 2) == "00" || txtPickupTime.value.substring(0, 2) == "0:"))
                    sBlankMsg = sBlankMsg + ", " + (iRowCount + 1);
                iRowCount = iRowCount + 1
                txtPickupTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtPickupTime")
            }

            if (sBlankMsg != "") {
                sBlankMsg = sBlankMsg.substring(1)
                $get(_ClientcstPickUpTimeFormat).errormessage = "Pickup Time should be in HH:MM AM/PM (e.g 10:00 AM) for row number " + sBlankMsg + "."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateDropTime(oSrc, args) {

            var iRowCount = 0;
            var sBlankMsg = "";
            var txtDropTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtDropTime")
            while (txtDropTime != null) {
                if (txtDropTime.value == "")
                    sBlankMsg = sBlankMsg + ", " + (iRowCount + 1);
                iRowCount = iRowCount + 1
                txtDropTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtDropTime")
            }
            if (sBlankMsg != "") {
                sBlankMsg = sBlankMsg.substring(1)
                $get(_ClientcstDropTime).errormessage = "Drop Time should not be blank for row number " + sBlankMsg + "."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateDropTimeFormat(oSrc, args) {
            var iRowCount = 0;
            var sBlankMsg = "";
            var txtDropTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtDropTime")
            while (txtDropTime != null) {
                if (!isTimeValid(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtDropTime") && !(txtDropTime.value.substring(0, 2) == "00" || txtDropTime.value.substring(0, 2) == "0:"))
                    sBlankMsg = sBlankMsg + ", " + (iRowCount + 1);
                iRowCount = iRowCount + 1
                txtDropTime = document.getElementById(_clientlstvwStopsTimeDetails + "_ctrl" + iRowCount + "_txtDropTime")
            }
            if (sBlankMsg != "") {
                sBlankMsg = sBlankMsg.substring(1)
                $get(_ClientcstDropTimeFormat).errormessage = "Drop Time should be in HH:MM AM/PM (e.g 10:00 AM)  for row number " + sBlankMsg + "."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function OnGridKeyUpNumber(obj, decimalPlaces, allowNegative, e) {
          //  extractNumber(obj, decimalPlaces, allowNegative);
            UpDownKeyPress(obj.id, e);
        }

        function CheckUnCheckAll(obj) {
            if(obj.checked)
                $('[id*=_chkWeekdays_]').attr('checked', 'checked')
            else
                $('[id*=_chkWeekdays_]').removeAttr('checked')
        }

//        function CheckMain() {
//            if ($('[id*=_chkWeekdays_]').length == $('[id*=_chkWeekdays_]:checked').length)
//                $('[id$=chkAll]').attr('checked', 'checked')
//            else
//                $('[id$=chkAll]').removeAttr('checked')
//        }

        function ValidateWeekdays(src, args) {
            if ($('#' + _clientcmbTypes).val() == "-2") {
                if ($('[id*=_chkWeekdays_]:checked').length == 0) {
                    src.errormessage = "At least one weekday should be selected.";
                    args.IsValid = false
                    return true
                }
            }
           
            args.IsValid = true
            return false
        }

        function ValidateDates(src, args) {        
            if ($('#' + _clientcmbTypes).val() == "-1") {
                var dtStartDate = document.getElementById(_clienttxtStartDate).value;
                var dtEndDate = document.getElementById(_clienttxtEndDate).value;

                if (dtStartDate == '' && dtEndDate == '') {
                    src.errormessage = 'Start and End Date should not be blank.'
                    args.IsValid = false;
                    return true;
                }
                else if (dtStartDate == '') {
                    src.errormessage = 'Start Date should not be blank.'
                    args.IsValid = false;
                    return true;
                }
                else if (dtEndDate == '') {
                    src.errormessage = 'End Date should not be blank.'
                    args.IsValid = false;
                    return true;
                }

                var startDate;
                if (document.all)
                    startDate = new Date(dtStartDate.replace('-', ' '));
                else
                    startDate = new Date(convertdate(dtStartDate));

                var endDate;
                if (document.all)
                    endDate = new Date(dtEndDate.replace('-', ' '));
                else
                    endDate = new Date(convertdate(dtEndDate));

                if (startDate > endDate) {
                    src.errormessage = 'End Date should be greater than or equal to Start Date.'
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;            
        }

//        function SetStatus(obj) {
//            if (obj.value == "1") {
//                $('[id$=trDates]').show()
//                $('[id$=trWeekdays]').hide()
//            }
//            else {
//                $('[id$=trDates]').hide()
//                $('[id$=trWeekdays]').show()
//            }
//        }
        
    </script>

</asp:Content>
