<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ExamScheduleList.aspx.cs" Inherits="ExamScheduleListUI" %>

<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="updtpnl1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Panel ID="pnlFields" runat="server" Width="98%">
                            <table runat="server" id="tblGridsubjects" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlErrorMsg" Visible="false" align="center" runat="server" Width="100%">
                                            <table align="center" width="800px" class="LblNoRecord">
                                                <tr>
                                                    <td align="center" style="width:800px;">
                                                        <asp:Label ID="lblError" runat="server" CssClass="ClsConfigText" EnableViewState="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr align="center" valign="top" runat="server" id="trBasePanel" visible="false">
                                    <td align="center">
                                        <cc1:CollapsablePanel ID="colpnlSubjectSchedule" runat="server" TitleText="Schedule"
                                            TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                            CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" 
                                            TitleStyle-Height="45px" Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div class="GridBorder" id="divgrid" style="width: 400px; overflow: auto" runat="server">
                                                            <asp:GridView ID="grdSubjectSchedule" runat="server" AllowSorting="True" Width="100%"
                                                                BackColor="White" CellPadding="0" CellSpacing="1" EmptyDataText="Subject Schedule not configured yet."
                                                                ForeColor="#333333" GridLines="None" 
                                                                OnRowDataBound="grdSubjectSchedule_RowDataBound" EnableViewState="False">
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                    NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                                                <RowStyle CssClass="ClsGridRow" />
                                                                <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                                <PagerStyle CssClass="ClsNwGridPaging" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </cc1:CollapsablePanel>
                                    </td>
                                </tr>
                                <tr align="center" valign="top">
                                    <td align="center">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>        
</asp:Content>
