<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="HolidayListUI.aspx.cs" Inherits="HolidayListUI" viewstatemode="Disabled" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                        ID="uPnl">
                        <ContentTemplate>
                            <asp:Panel ID="pnlFields" runat="server" Width="1100px">
                                <table align="left" width="1100px">
                                    <tr runat="server" id="trTotalRec" align="center">
                                        <td>
                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                <asp:Label ID="lblTo" CssClass = "LblNormal"  runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, To%>"></asp:Label>
                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                               <asp:Label ID="lblOutOf" CssClass = "LblNormal"  runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, OutOf%>"></asp:Label>
                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                <asp:Label ID="lblRecords" CssClass = "LblNormal"  runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Records%>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="1500px">
                                            <asp:GridView ID="grdHoliDaysManagement" runat="server" CssClass="GridBorder" ForeColor="#333333"
                                                EmptyDataText="<%$ Resources:LocalizedResources,HolidaysNotYetDeclared%>" 
                                                OnRowCommand="grdHoliDaysManagement_RowCommand"
                                                OnRowDataBound="grdHoliDaysManagement_RowDataBound" GridLines="None" DataKeyNames="Holiday_Id,StartDay,EndDay"
                                                AllowPaging="True" CellSpacing="1" CellPadding="0" PageSize="20" AllowSorting="True"
                                                AutoGenerateColumns="False" Width="100%" EnableViewState="False" 
                                                DataSourceID="GrdDSobj">
                                                <Columns>
                                                    <asp:BoundField DataField="Holiday_Start_Date" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="<%$ Resources:LocalizedResources, Start_Date %>">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Holiday_End_Date" DataFormatString="{0:dd-MMM-yyyy}" HeaderText= "<%$ Resources:LocalizedResources, End_Date%>" >
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Holiday_Name" HeaderText= "<%$ Resources:LocalizedResources, Name%>" >
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%" CssClass="ClspaddingL" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, AssociatedClass%>" DataField="AssociatedStandard">
                                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="25%" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Width="25%" CssClass="ClspaddingL" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, TotalDays%>" DataField="TotalDays">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField ButtonType="Image" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                        HeaderText="<%$ Resources:LocalizedResources, Edit%>" Text="Edit" CommandName="EDIT_HOLIDAYS">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                    </asp:ButtonField>                                                    
                                                    <asp:TemplateField HeaderText= "<%$ Resources:LocalizedResources, Delete%>" >
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnDeleteHoliday" ToolTip= "<%$ Resources:LocalizedResources, Delete%>" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                                CommandName="DELETE_HOLIDAYS" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                        </PagerStyle>
                                                <RowStyle CssClass="ClsGridRow" />
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                <PagerTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="70%" align="left"  class="ClsBorderPager" valign="middle">
                                                                <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage%>" runat="server" CssClass="LblNrmlB" />
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
                                    <tr>
                                        <td>
                                            <asp:ObjectDataSource TypeName="BusinessLogic.HolidaysMasterBL" EnablePaging="True"
                                                ID="GrdDSobj" runat="server" SelectMethod="GetHolidayDetails" SortParameterName="sortExpression"
                                                SelectCountMethod="GetHolidayCount" OnSelected="GrdDSobj_Selected">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                                    <asp:SessionParameter Name="aiAccYrId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"  Type="String"   />
                                                    <asp:ControlParameter Name="sortExpression" ControlID="hidSortExpression" Type="String"
                                                        PropertyName="Value" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                            <asp:HiddenField ID="hidIsConfig" runat="server" />
                                            <asp:HiddenField ID="hidCanEdit" runat="server" />
                                            <asp:HiddenField ID = "hidAlertMessageForHoliday" runat = "server" />
                                            <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" style="width: 98%" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnBack" runat="server" Text= "<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtn" 
                                        CausesValidation="False" />
                                </td>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:LocalizedResources, Add%>" CssClass="ClsBtn"
                                        CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidAlertMessageForHoliday.ClientID %>").value)) {
                bResult = false
            }
            return bResult
        }
    </script>
</asp:Content>
