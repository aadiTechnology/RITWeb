<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchoolwiseAcademicYearUI.aspx.cs"
    Inherits="SchoolwiseAcademicYearUI" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" class="CPanelSpace">
                </td>
                <td align="center" colspan="1" rowspan="20" style="width: 5%">
                </td>
                <td align="center" colspan="1">
                </td>
            </tr>            
            <tr>
                <td align="center">
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                            ID="uPnl">
                            <ContentTemplate>
                                <asp:GridView CssClass="GridBorder" ID="grdAcademicYear" runat="server" AutoGenerateColumns="False"
                                    CellPadding="0" AllowSorting="True" CellSpacing="1" GridLines="None" DataKeyNames="Academic_Year_Id,School_Id,Is_Current_Year,Is_Close_Year"
                                    OnRowDataBound="grdAcademicYear_RowDataBound" ForeColor="#333333" OnSorting="grdAcademicYear_Sorting"
                                    OnRowCreated="grdAcademicYear_RowCreated" Width="55%" EnableViewState="False">
                                    <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>" PreviousPageText="<%$ Resources:LocalizedResources, Previous%>" 
                                        FirstPageText="<%$ Resources:LocalizedResources, First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, CurrentYear %>">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCurrentYear" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Start_Date" HeaderText="<%$ Resources:LocalizedResources, StartDate %>" SortExpression="Start_Date">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="End_Date" HeaderText="<%$ Resources:LocalizedResources, EndDate%>" SortExpression="End_Date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, CloseYear %>">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCloseYear" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources, Edit %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                            CommandName="EDIT_ACADEMIC_YEAR" Text="Edit">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <PagerStyle CssClass="ClsNwGridPaging" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False"
                                        ForeColor="Black" Font-Names="Arial" Font-Size="Small" />
                                    <RowStyle CssClass="ClsGridRow" />
                                    <HeaderStyle CssClass="ClsGridHeader" />
                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="ClsLblNoRecords" />
                                </asp:GridView>
                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </center>
                </td>
            </tr>
            <tr>
                <td align="center" style="padding-top:5px">
                    <asp:Button ID="btnBack" runat="server" 
                        Text="<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn"
                         TabIndex="0" CausesValidation="False" OnClick="btnBack_Click" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript"></script>

</asp:Content>
