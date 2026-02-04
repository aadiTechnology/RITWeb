<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="MonthwiseStaffAttendancePopup.aspx.cs" Inherits="MonthwiseStaffAttendancePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table style="width: 100%;">
        <tr>
            <td valign="top">
                <table style="width: 100%;">
                    <tr>
                        <td align="left" style="height: 20px; width: 99%;" class="ClsGrayMainTitle">
                            <span style="font-weight: bold">Monthwise Staff Attendance</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="60%">
                                <tr style="height: 5px;">
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr class="ClsBorderlight">
                                    <td class="ClsBorderlight" style="width: 140px;">
                                        <asp:Label ID="lblMonth" runat="server" CssClass="ClsLabel" Font-Bold="true">Year</asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td class="ClsHilightBGB" style="text-align: center; width: 60px;">
                                        <asp:Label ID="lblYear" runat="server"></asp:Label>
                                    </td>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="lblStaffGroup" runat="server" CssClass="ClsLabel" Font-Bold="true">Staff Group</asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbStaffGroup" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                            OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
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
                <table width="100%">
                    <tr>
                        <td align="center">
                            <table width="100%" align="left">
                                <tr>
                                    <td align="left" class="ClsBorderlight" style="width: 5%; background-color: #ffffc4;">
                                        <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                            CssClass="LblNrmlB"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                        <asp:Label ID="lblNoteText" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Present days and working days are calculated based on configured weekdays and holidays."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Panel ID="Pnl1" runat="server" ScrollBars="Vertical" Height="320px" Width="100%">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView CssClass="GridBorder" ID="grdStaffAttendance" runat="server" AllowPaging="false"
                                            AutoGenerateColumns="True" Width="100%" AllowSorting="false" CellPadding="0" CellSpacing="1"
                                            ForeColor="#333333" EmptyDataText="<%$ Resources:LocalizedResources, NoAttendanceAvailable %>"
                                            GridLines="None" EnableViewState="False" OnRowCreated="grdStaffAttendance_RowCreated"
                                            OnRowDataBound="grdStaffAttendance_RowDataBound">
                                            <PagerStyle></PagerStyle>
                                            <Columns>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <RowStyle CssClass="ClsGridRow ClspaddingL" />
                                            <HeaderStyle CssClass="ClsGridHeader" />
                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                            <AlternatingRowStyle CssClass="ClsGridAltRow ClspaddingL" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Close %>"
                                OnClientClick="ClosePopup(); return false;" CssClass="ClsBtn" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:HiddenField ID="hidYear" runat="server" />
    </table>
    <script language="javascript" type="text/javascript">

        function ClosePopup() {
            window.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
