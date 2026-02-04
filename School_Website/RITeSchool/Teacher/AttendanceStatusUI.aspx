<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="AttendanceStatusUI.aspx.cs" Inherits="AttendanceStatusUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="upnlAttendance" runat="server">
        <ContentTemplate>
            <table width="94%">
                <tr>
                    <td align="left" rowspan="1">
                        <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                            <tr>
                                <td style="height: 20px">
                                    <span class="MainTitleHead">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, AttendanceStatus%>"></asp:Label></span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        <span class="ClsMdtStar">*
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label></span>
                    </td>
                </tr>
            </table>
            <table align="center" cellpadding="0" cellspacing="1" border="0">
                <tr style="height: 0">
                    <td align="left">
                        <asp:Label ID="lblError" runat="server" Text="" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 20px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <table align="center">
                            <tr id="trDate" runat="server">
                                <td class="ClsBorderlight" style="width: 80px;">
                                    <span class="ClsLabel">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, SelectDate%>"></asp:Label>
                                        <span id="Span9" class="colonPadding">:</span></span>
                                </td>
                                <td>
                                    <asp:TextBox ID="calTodaysDate" CssClass="SmlCombo" runat="server" AutoPostBack="true" 
                                        Style="vertical-align: bottom" MaxLength="11" onpaste="event.returnValue=false"
                                        TabIndex="1" ondrop="event.returnValue=false"></asp:TextBox>
                                    <rjs:PopCalendar ID="cAttendDate" runat="server" Control="calTodaysDate" Format="dd MMM yyyy" Culture="en"
                                        RequiredDate="true" ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>"
                                        RequiredDateMessage="<%$ Resources:LocalizedResources, DateShouldNotBeBlank %>"
                                        AutoPostBack="True" To-Today="true" OnSelectionChanged="cAttendDate_SelectionChanged" />
                                    <span class="ClsMdtStar">*</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table align="left">
                            <tr id="trHoliday" runat="server">
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trLegend" runat="server" align="center">
                    <td align="left">
                        <table id="LegendTable" runat="server" align="left" cellpadding="0" cellspacing="1">
                            <tr>
                                <td align="left" width="60px">
                                    <span class="ClsLblLgnd" style="border-width: 0px; font-weight: bold">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
                                        <span id="Span2" class="colonPadding">:</span> </span>
                                </td>
                                <td width="25px" id="tdimgAttendanceDone" runat="server">
                                    <asp:Image ID="Image6" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                </td>
                                <td align="center" width="120px" id="tdlblAttendanceDone" runat="server">
                                    <span class="ClsTextNormal" style="font-weight: bold">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, AttendanceMarked%>"></asp:Label>
                                      </span>
                                </td>
                                <td width="10px">
                                </td>
                                <td width="32px">
                                    &nbsp;<asp:Image ID="Image1" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                </td>
                                <td width="140px">
                                    <span class="ClsTextNormal" style="font-weight: bold"><asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, AttendanceNotMarked%>"></asp:Label> </span>
                                </td>
                                <td width="10px">
                                </td>
                                <td width="25px" align="center" style="background-color: #FFCCFF; text-align: center">
                                    -
                                </td>
                                <td width="140px" style="padding-left: 10px">
                                    <span class="ClsTextNormal" style="font-weight: bold"><asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, OutsideAcademicYear%>"></asp:Label> </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" visible="true" runat="server" id="tdGrid">
                        <div id="divMain" runat="server" style="width: 700px">
                            <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Horizontal" Width="700px">
                                <asp:GridView ID="grdStandards" Width="100%" UseAccessibleHeader="true" runat="server"
                                    CssClass="GridBorder" AutoGenerateColumns="true" Height="43px" PageSize="20"
                                    AllowPaging="false" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                    OnRowDataBound="grdStandards_RowDataBound">
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                    <Columns>
                                    </Columns>
                                    <RowStyle CssClass="ClsGridRow" />
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                    </PagerStyle>
                                    <HeaderStyle CssClass="ClsGridHeader" HorizontalAlign="Center" />
                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
                <tr id="trPrecondition" runat="server" visible="false">
                    <td align="left">
                        <div runat="server" id="divErr">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <br />
                        <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Close %>" CausesValidation="false"
                            OnClientClick="window.close();return false;" TabIndex="2" />
                    </td>
                </tr>
                <tr>
                    <asp:HiddenField ID="hidShowCount" runat="server" />
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript" src="../Scripts/jquery-1.7.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-blink.js"></script>
    <script src="../../js/jquery.qtip-1.0.0-rc3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/Validate2.js"></script>
    <script type="text/javascript" src="../Scripts/Validations.js"></script>
    <style type="text/css">
        .class1
        {
            border: 1;
        }
    </style>
    <style type="text/css">
        .class2
        {
            border: 1;
        }
    </style>
    <script type="text/javascript">
        function EndRequestHandler() {
            showtooltip();
        }

        function showtooltip() {
            $('.class1').qtip({
                content: {
                    text: false // Use each elements title attribute
                },
                style: {
                    name: 'cream',
                    color: 'black',  //'cream', // Give it some style
                    border: {
                        width: 3,
                        radius: 5
                    },
                    tip: 'topRight',
                    width: 200
                },

                position: { adjust: { x: -210, y: 0} }
            });
        }
        showtooltip();
    </script>
</asp:Content>
