<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StaffBirthDay.aspx.cs" Inherits="StaffBirthDay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UPanelInput" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table align="center" width="90%">
                                <tr runat="server" id="trTotalRec" align="center">
                                    <td colspan="6">
                                        <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                        <span class="LblNormal">To</span>
                                        <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                        <span class="LblNormal">Out Of</span>
                                        <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                        <span class="LblNormal">Records</span>
                                    </td>
                                </tr>
                                <tr runat="server" id="trTotal" align="center">
                                    <td>
                                        <asp:GridView ID="grdStaffBirthday" runat="server" CssClass="GridBorder" ForeColor="#333333"
                                            OnRowDataBound="grdStaffBirthday_RowDataBound" GridLines="None" DataKeyNames="User_Id,Sort_Order"
                                            AllowPaging="True" CellSpacing="1" CellPadding="0" PageSize="20" AllowSorting="True"
                                            AutoGenerateColumns="False" Width="100%" EnableViewState="False" OnPageIndexChanging="grdStaffBirthday_PageIndexChanging"
                                            DataSourceID="GrdDSobj">
                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                FirstPageText="First" Position="Bottom" Mode="Numeric"></PagerSettings>
                                            <Columns>
                                                <asp:BoundField DataField="StaffName" HeaderText="Name">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%" CssClass="ClspaddingL" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                        Width="25%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DOB" HeaderText="DOB">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Designation" HeaderText="Designation">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="18%" CssClass="ClspaddingL" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="18%" CssClass="ClspaddingL" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Email" DataField="Email_Address">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" CssClass="ClspaddingL" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%" CssClass="ClspaddingL" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Mobile No." DataField="Mobile_Number">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%" />
                                                </asp:BoundField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <RowStyle CssClass="ClsGridRow" />
                                            <HeaderStyle CssClass="ClsGridHeader" />
                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                            <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" />
                                            <PagerTemplate>
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                         <span class="LblNormal">Select a page:</span>
                                                         <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" AppendDataBoundItems="true"
                                                                CssClass="LblNormal" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged"
                                                                runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </PagerTemplate>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="grdStaffBirthday" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <tr>
                        <td>
                            <asp:ObjectDataSource TypeName="BusinessLogic.SchoolUserBL" EnablePaging="True" ID="GrdDSobj"
                                runat="server" SelectMethod="GetStaffBirthday" SortParameterName="sortExpression"
                                SelectCountMethod="GetBirthdayCount" OnSelected="GrdDSobj_Selected">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                    <asp:SessionParameter Name="aiAcademicYrId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                        Type="int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:HiddenField ID="hidRowCnt" runat="server" />
                    </td>
                    </tr>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
