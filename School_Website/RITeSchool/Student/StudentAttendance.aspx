<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="StudentAttendance.aspx.cs" Inherits="StudentAttendance" viewstatemode="Disabled" %>

<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td >
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" valign="top" class="WorkingDay">
                                <span id ="lblWorkingDays" class="ClsHilightText">School working days :</span>
                            &nbsp;
                        </td>
                        <td align="left" valign="top" class="WorkingDayRslt">
                            <asp:Label ID="lblWorkingDaysR" runat="server" Text="Total working days :" EnableViewState="false"></asp:Label>
                        </td>
                        <td >
                        </td>
                        <td align="left" valign="top" class="PresentDay">
                                <span id ="lblPresentDays" class="ClsHilightText">Total present days :</span>
                            &nbsp;
                        </td>
                        <td align="left" valign="top" class="PresentDayRslt">
                            <asp:Label ID="lblPresentDaysR" runat="server" Text="Total present days :" EnableViewState="false"></asp:Label>
                        </td>
                        <td style="width: 20px">
                        </td>
                        <td align="left" valign="top" class="AbsentDay">
                                <span id ="lblAbsentDays" class="ClsHilightText">Total absent days :</span>
                            &nbsp;
                        </td>
                        <td align="left" valign="top" class="AbsentDayRslt">
                            <asp:Label ID="lblAbsentDaysR" runat="server" Text="Total absent days :" EnableViewState="false"></asp:Label>
                        </td>
                        <td align="center" style="width: 30px">
                        </td>
                        <td colspan="1" id="tdhlnkToppers" align="center" runat="server" >
                            <asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen" Height="20px" ID="hlnkToppers"
                                NavigateUrl="~/RITeSchool/Student/StudAttendanceRankersListUIPopUp.aspx" runat="server" ViewStateMode="Enabled"
                                Target="_blank">Attendance Toppers</asp:HyperLink>
                        </td>
                        <td>&nbsp;</td>
                        <td  >
                         <asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen"
                            Height="20px" ID="hlnkOldToppers" NavigateUrl="StudAttendanceRankersListUIPopUp.aspx" ViewStateMode="Enabled"
                            runat="server" Target="_new">Old Attendance Records</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top" style="padding-top: 10px">
                <Calender:EventCalendar ID="AttendanceCalendar" runat="server" ViewStateMode="Enabled" BackColor="White"
                    BorderColor="Silver" CellPadding="0" DayNameFormat="Full" EventBackColorName=""
                    EventDescriptionColumnName="" EventEndDateColumnName="" EventForeColorName=""
                    EventHeaderColumnName="" EventStartDateColumnName="" Font-Names="Arial" Font-Size="8pt"
                    ForeColor="Black" Height="310px" NextPrevFormat="FullMonth" ShowDescriptionAsToolTip="True"
                    ShowGridLines="True" Width="95%" OnVisibleMonthChanged="AttendanceCalendar_VisibleMonthChanged"
                    Font-Bold="False" SelectionMode="None">
                    <SelectedDayStyle BackColor="#E7E7E7" Font-Bold="True" ForeColor="Black" BorderColor="LightSteelBlue"
                        BorderStyle="Solid" BorderWidth="1px" />
                    <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                    <WeekendDayStyle BackColor="Transparent" Font-Bold="False" ForeColor="Black" />
                    <OtherMonthDayStyle ForeColor="#999999" Height="50px" />
                    <NextPrevStyle Font-Size="8pt" ForeColor="Navy" />
                    <DayHeaderStyle ForeColor="White" Height="25px" CssClass="DayHeader" />
                    <TitleStyle Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="25px" BorderStyle="None"
                        CssClass="MonthHeader" />
                    <DayStyle Height="46px" />
                </Calender:EventCalendar>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                &nbsp;
            </td>
        </tr>
        <%--<tr>
            <td align="center" style="height: 21px">
                <asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                    CssClass="ClsBtnSml" OnClick="btnCancel_Click" Text="Back" Visible="True" />
            </td>
        </tr>--%>
    </table>

    <script language="javascript" type="text/javascript">
        function ShowToppers() {
            _sClienthlnkToppers = "<%=this.hlnkToppers.ClientID %>";
           
            if ((document.getElementById(_sClienthlnkToppers) == null) || (document.getElementById(_sClienthlnkToppers) == "") || (document.getElementById(_sClienthlnkToppers).disabled))
                return false;

            window.open("StudAttendanceRankersListUIPopUp.aspx", '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=500');


            return false;
        }

        function ShowOldToppers() {
             _clienthlnkOldToppers = "<%=hlnkOldToppers.ClientID %>";
            if ((document.getElementById(_clienthlnkOldToppers) == null) || (document.getElementById(_clienthlnkOldToppers) == "") || (document.getElementById(_clienthlnkOldToppers).disabled))
                return false;

            window.open("StudAttendanceRankersListUIPopUp.aspx ? ", '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=500');

        }
    </script>

</asp:Content>
