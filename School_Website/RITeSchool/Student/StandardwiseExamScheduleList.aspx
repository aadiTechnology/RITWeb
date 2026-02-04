<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="StandardwiseExamScheduleList.aspx.CS" Inherits="StandardwiseExamScheduleList" ViewStateMode="Disabled"%>

<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td align="left" colspan="6" style="padding-bottom: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                                <tr> 
                                                    <asp:Panel runat="server" ID="pnlTittle" Visible="false">
                                                      <td class="ClsGrayMainTitle" style="height: 20px">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                                                            <tr>
                                                                <td align="left" style="width: 90%">
                                                                    <span class="MainTitleHead" style="font-weight: bold;">View Exam Schedule</span>
                                                                 </td>
                                                            </tr>
                                                         </table>
                                                        </td>
                                                    </asp:Panel>
                                                </tr>
                                            </table>
                 </td></tr>
             
            <tr id="Tr1" runat="server" align="center">
                                <td align="center">
                                <table runat="server" >
                                    <tr><td><asp:label ID="lblStandard" runat="server" Font-Bold="true" Visible="false" >Select Standard:</asp:label></td>
                                 <td align="left"><asp:DropDownList runat="server" ViewStateMode="Enabled" ID="ddlStandard" Visible="false"
                                      AutoPostBack="True" onselectedindexchanged="ddlStandard_SelectedIndexChanged"> 
                                 </asp:DropDownList></td></tr>
                                 </table>
                               </td>
                               
            </tr>
             <tr>
                <td align="center">
                    <asp:Panel ID="pnlErrorMsg" Visible="false" align="center" runat="server" Width="100%">
                        <table align="center" width="100%" class="LblNoRecord">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblError" runat="server" CssClass="ClsConfigText" EnableViewState="False"></asp:Label>
                                </td>
                               
                             </tr>  
                             
                            
                        </table>
                    </asp:Panel>
                </td>
            </tr>
              <tr>
                <td>
                    <asp:Panel ID="pnlFields" runat="server">
                        <table runat="server" id="tblGridsubjects" cellpadding="0" cellspacing="0" width="98%">
                            <tr align="center" valign="top" runat="server" id="trBasePanel" visible="false">
                                <td align="center">
                                    <cc1:CollapsablePanel ID="colpnlSubjectSchedule" runat="server" TitleText="Schedule" 
                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" 
                                        TitleStyle-Height="45px" Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                        <div class="GridBorder" id="Div1" style="width: 60%;" runat="server">
                                            <asp:GridView ID="grdSubjectSchedule" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BackColor="White" CellPadding="0" CellSpacing="1" 
                                                EmptyDataText="Subject Schedule not configured yet." ForeColor="#333333" GridLines="None"
                                                Width="100%" onrowdatabound="grdSubjectSchedule_RowDataBound" 
                                                EnableViewState="False" style="margin-right: 0px">
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                    NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                                <RowStyle CssClass="ClsGridRow" />
                                                <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                <Columns>
                                                    <asp:BoundField DataField="Start_DateTime" HeaderText="Exam Date"
                                                        SortExpression="Start_DateTime">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Subject_Name" HeaderText="Subject" SortExpression="Subject_Name">
                                                        <HeaderStyle CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Middle"
                                                            Width="12%" />
                                                        <ItemStyle CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Top" Width="12%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TestType_Name" HeaderText="Exam Type" SortExpression="TestType_Name">
                                                        <HeaderStyle CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Middle"
                                                            Width="8%" Wrap="false" />
                                                        <ItemStyle CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Top" Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Start_DateTime" HeaderText="Start Time"
                                                        SortExpression="Start_DateTime" HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%"/>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="7%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="End_DateTime" HeaderText="End Time"
                                                        HtmlEncode="False" SortExpression="End_DateTime">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%"/>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="7%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TotalTime" HeaderText="Total Time"
                                                        HtmlEncode="False" SortExpression="End_DateTime">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"/>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" HtmlEncode="False">
                                                        <HeaderStyle CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Middle"
                                                            Width="28%" />
                                                        <ItemStyle CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Top" Width="28%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerStyle CssClass="ClsNwGridPaging" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                            </asp:GridView>
                                        </div>
                                    </cc1:CollapsablePanel>
                                </td>
                            </tr>
                            <tr align="center" valign="top">
                                <td align="center">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <center>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style=" padding-left:5px;">
                                        
                                            <asp:Button ID="btnClose" runat="server" Visible="false" Text="Close" Height="24px" CssClass="ClsBtn" CausesValidation="False" OnClientClick="window.close(); return false;"
                                                 />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </center>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSchoolId" runat="server" />
                            <asp:HiddenField ID="hidAcademicYearId" runat="server" />
                            <asp:HiddenField ID="hidStandardId" runat="server" /><asp:HiddenField ID="hidDivisionID" runat="server" Value="0" />
                            <asp:HiddenField ID="hidIsAdmin" runat="server" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript" language="javascript">
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this details?')) {
                bResult = false
            }
            return bResult
        }
    </script>
</asp:Content>
