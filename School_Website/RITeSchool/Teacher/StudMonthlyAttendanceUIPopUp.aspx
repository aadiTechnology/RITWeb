<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/PopupMaster.master"
    EnableEventValidation="false" CodeFile="StudMonthlyAttendanceUIPopUp.aspx.cs"
    Inherits="StudMonthlyAttendanceUIPopUp" %>

<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td align="left" colspan="4">
                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="height: 20px" class="ClsGrayMainTitle">
                                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                <tr>
                                                    <td align="center" class="MainTitleHead" style="height: 20px">
                                                        <%--<asp:Label ID="lblBuyer" runat="server" BorderWidth="0px" Text="Individual Attendance"
                                                            Font-Bold="True" EnableViewState="false"></asp:Label>--%>
                                                            <span style="font-weight:bold">
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, IndividualAttendance %>"></asp:Label></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False" ></asp:Label>
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ShowMessageBox="True" ShowSummary="False"
                                        CssClass="ClsLabel" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr  id="trSaveSuccess" runat="server">
                            <td id="tdlblSaveSuccess" runat="server" align="center">
                                <asp:UpdatePanel runat="server" ID="updateLabel" UpdateMode="Always">
                                             <ContentTemplate>
                                                 <asp:Label runat="server" ID="lblSaveSuccess" EnableViewState="false" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                             </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trStudentCombo" runat="server">
                            <td align="center">
                                <table>
                                   <tr>
                                        <td runat="server" id="tdlblStudent" class="ClsBorderlight">
                                            <%--<asp:Label ID="lblStudent" runat="server" CssClass="ClsLabel" Text="Student :" EnableViewState="False"></asp:Label>--%>
                                            <span class="ClsLabel">
                                                <asp:Label ID="lblStudent" runat="server" Text="<%$ Resources:LocalizedResources, Student %>"></asp:Label> 
                                                <span id="Span1" class="colonPadding">:</span>
                                                </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbStudents" Width="345px" runat="server" AutoPostBack="True"
                                                CssClass="LrgCombo" OnSelectedIndexChanged="cmbStudents_SelectedIndexChanged">
                                                <asp:ListItem Text="-- All --" Value="0" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trChkAll" runat="server">
                            <td align="center">
                                <span class="SubTitle">
                                    <input type="checkbox" id="chkAll" name="chkAll" id="chkAll" onclick="javascript:checkAll();"
                                        onchange="javascript:checkAll();" /><asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>"></asp:Label>
                                    <%--<label for="ctl00_MainBody_chkAll">
                                        Select All</label>--%></span>
                            </td>
                        </tr>
                        <tr id="trcalendar" runat="server">
                            <td colspan="4" align="center" valign="top">
                                <asp:UpdatePanel runat="server" ID="updateCalender" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <Calender:EventCalendar ID="AttendanceCalendar" runat="server" BackColor="White"
                                            BorderColor="Silver" CellPadding="0" DayNameFormat="Full" EventBackColorName=""
                                            EventDescriptionColumnName="" EventEndDateColumnName="" EventForeColorName=""
                                            EventHeaderColumnName="" EventStartDateColumnName="" Font-Names="Arial" Font-Size="8pt"
                                            ForeColor="Black" Height="310px" NextPrevFormat="FullMonth" ShowDescriptionAsToolTip="True"
                                            ShowGridLines="True" Width="95%" OnVisibleMonthChanged="AttendanceCalendar_VisibleMonthChanged"
                                            Font-Bold="False" SelectionMode="None">
                                            <SelectedDayStyle BackColor="#E7E7E7" Font-Bold="True" ForeColor="Black" BorderColor="LightSteelBlue"
                                                BorderStyle="Solid" BorderWidth="1px" />
                                            <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                            <WeekendDayStyle BackColor="Transparent" Font-Bold="False" />
                                            <OtherMonthDayStyle ForeColor="#999999" Height="50px" />
                                            <NextPrevStyle Font-Size="8pt" HorizontalAlign="Left" ForeColor="Navy" />
                                            <DayHeaderStyle ForeColor="White" Height="25px" CssClass="DayHeader" />
                                            <TitleStyle Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="25px" BorderStyle="None"
                                                CssClass="MonthHeader" />
                                            <DayStyle Height="46px" />
                                        </Calender:EventCalendar>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trNoRecordFound" runat="server" visible="false">
                            <td align="center" colspan="1">
                                <asp:Label ID="lblNoRecordFound" runat="server" Text="" CssClass="LblNoRecord"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="1" id="tdBack" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtn" OnClick="btnBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
            ID="UpdatePanel1">
            <ContentTemplate>
                <asp:HiddenField ID="hidSortDirection" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                <asp:HiddenField ID="hidDate" runat="server" Value="0" />
                <asp:HiddenField ID="hidDays" runat="server" />
                <asp:HiddenField ID="hidAbsentDays" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidSchoolId" runat="server" />
        <asp:HiddenField ID="hidAcademicYearId" runat="server" />
        <asp:HiddenField ID="hidIsConfig" runat="server" />
    </div>

    <script language="javascript" type="text/javascript">
        _sClientAcademicYearId = "<%=this.hidAcademicYearId.ClientID %>";
        _sClientGridId = "aa";
        _sClienthidSchoolId = "<%=this.hidSchoolId.ClientID %>";
        _sClienthidDays = "<%=this.hidDays.ClientID %>";
        _sClienthidAbsentDays = "<%=this.hidAbsentDays.ClientID %>";

        _clientSave = "<%=this.btnSave.ClientID%>";
        _clientBack = "<%=this.btnBack.ClientID%>";
        _clientcmbStudents = "<%=this.cmbStudents.ClientID%>";
        _clientAttendanceCalendar = "<%=this.AttendanceCalendar.ClientID%>";

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginReqHandler);
        prm.add_endRequest(EndReqHandler);

        function ConfirmDelete(iPageCount, sActionName) {
            var bResult = true;

            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientGridId, 'chkDelete', sActionName, 'false', iPageCount, 'true')) {
                if (!window.confirm("Are you sure you want to delete the selected students(s)?")) {
                    bResult = false;
                }
            }
            else
            { bResult = false; }

            return bResult;
        }

        function checkAll() {
            var chkAll = document.getElementById('chkAll');
            var checks = document.forms[0].elements;
            var removeButton = document.getElementById('removeChecked');
            var boxLength = checks.length;
            var allChecked = false;
            var totalChecked = 0;

            if (chkAll.checked == true) {
                for (i = 0; i < boxLength; i++) {
                    if (checks[i].type == 'checkbox')
                        checks[i].checked = true;
                }
            }
            else {
                for (i = 0; i < boxLength; i++) {
                    if (checks[i].type == 'checkbox')
                        checks[i].checked = false;
                }
            }
        }

        function CalculateAttendance() {
            var sIds = ''
            var sAbsentIds = ''
            var checks = document.forms[0].elements;
            var boxLength = checks.length;
            var allChecked = false;
            var totalChecked = 0;

            for (j = 0; j < boxLength; j++) {
                if (checks[j].type == 'checkbox' && checks[j].id != 'chkAll') {
                    if (checks[j].checked == true) {
                        totalChecked++;
                        sIds = sIds + checks[j].id.split('_')[1] + '$';
                    }
                    else {
                        sAbsentIds = sAbsentIds + checks[j].id.split('_')[1] + '$';
                    }
                }
            }

            sIds = sIds.substring(0, sIds.length - 1)
            sAbsentIds = sAbsentIds.substring(0, sAbsentIds.length - 1)
            $get(_sClienthidDays).value = sIds;
            $get(_sClienthidAbsentDays).value = sAbsentIds;
        }

        function assignDivision(obj) {

            document.getElementById(_sClienthidDivisionId).value = obj.value;

        }
        function assignStandard(obj) {

            if (obj.value == 0) {
                document.getElementById(_sClienthidStandardId).value = obj.value;
            }
            else {
                document.getElementById(_sClienthidStandardId).value = obj.value;
            }


        }
        function HideAddButton(objIdStd, objIdDiv) {

            if (document.getElementById(objIdDiv).value == 0) {

                document.getElementById(_sClientbtnAdd).style.display = 'none';
            }
            else {
                document.getElementById(_sClientbtnAdd).style.display = '';
            }


        }

        function refreshParent(qry) {
            window.opener.location = window.opener.location.pathname + "?" + qry;
            window.close();
            window.opener.focus();
        }

        function fnover(varname, doc) {

            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "maroon";
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
            //objTXT.style.color = "maroon";
        }

        function fnout(varname, doc) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "#a3c07b";
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
            //objTXT.style.color = "Black";
        }

        function ConfirmDelete() {
            var bResult = true;
            if (window.confirm("Are you sure you want to delete this Student?")) {
                bResult = true;
            }
            else {
                bResult = false;
            }

            return bResult;
        }

        function BeginReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement == null || postBackElement.id == _clientSave)
                DisableButtons(true);
        }

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement == null || postBackElement.id != _clientcmbStudents)
                DisableButtons(false);
        }

        function DisableButtons(action) {
            var isPageValid = true;
            if (typeof (Page_ClientValidate) == 'function' && action)
                isPageValid = Page_ClientValidate();

            if (isPageValid) {
                if (document.getElementById(_clientSave) != null)
                    document.getElementById(_clientSave).disabled = action;
                if (document.getElementById(_clientBack) != null)
                    document.getElementById(_clientBack).disabled = action;
                if (document.getElementById('chkAll') != null)
                    document.getElementById('chkAll').disabled = action;
                if (document.getElementById(_clientcmbStudents) != null)
                    document.getElementById(_clientcmbStudents).disabled = action;
                if (document.getElementById(_clientAttendanceCalendar) != null)
                    document.getElementById(_clientAttendanceCalendar).disabled = action;
            }
        }

        function UncheckSelectAll() {
            var chkAll = document.getElementById('chkAll')
            chkAll.checked = false;

        }
       
    </script>

</asp:Content>
