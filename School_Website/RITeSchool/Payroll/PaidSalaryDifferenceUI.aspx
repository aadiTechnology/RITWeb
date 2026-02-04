<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="PaidSalaryDifferenceUI.aspx.cs" Inherits="PaidSalaryDifferenceUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            <tr>
                <td class="ClsGrayMainTitle" valign="top" align="left" style="width: 100%">                    
                    <asp:Label ID="lblHeader2" runat="server" CssClass="MainTitleHead" Text ="Paid Salary Difference" EnableViewState="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="middle" align="left" style="width: 100%;padding-left:10px;">                    
                    <asp:Label ID="lblHeader" runat="server" CssClass="HeadTxtBWOPadding" style="font-size:small;color:Black" Text ="" EnableViewState="true"></asp:Label>
                </td>
            </tr>            
            <tr>
                <td>
                    <tr>
                        <td align="center" visible="true" runat="server" id="tdGrid">
                            <div id="divContainer" class="GridBorder" runat="server" style="width: 800px; height: 390px;
                                overflow: scroll">
                                <asp:GridView ID="grdSalaryDifference" Width="100%" runat="server"
                                    PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                    UseAccessibleHeader="true" CssClass="GridBorder" 
                                    onrowdatabound="grdSalaryDifference_RowDataBound">
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <Columns>
                                    </Columns>
                                    <RowStyle CssClass="ClsGridRow" />
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                    </PagerStyle>
                                    <HeaderStyle CssClass="ClsGridHeader" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                No record found.
                                            </td>
                                        </tr>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                            <asp:Label ID="lblNoRecordMessage" runat="server" Text="No record found." CssClass="LblNoRecord"
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="ClsBtn" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function ClosePopup() {
            window.close();
        }
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
