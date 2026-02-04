<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SchoolConfigurationControlPanel.aspx.cs" Inherits="SchoolConfigurationControlPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div>
        <table cellpadding="2" cellspacing="3" style="width: 97%" align="center">
            <tr id="tdMenu" runat="server">
                <td style="width: 30%; padding-left: 15px" align="left" class="ClsBorderlight td-vertical-align-top" valign="top">
                    <asp:UpdatePanel runat="server" ID="UPanelMenuItem" UpdateMode="Conditional" ChildrenAsTriggers="False">
                        <ContentTemplate>
                            <div id="Div1" runat="server">
                                <table cellpadding="2" cellspacing="2" style="width: 100%">
                                    <tr>
                                        <td style="padding-right: 5px; padding-left: 5px; padding-bottom: 5px; padding-top: 5px;
                                            background-color: #ffffff" valign="top">
                                            <asp:Menu ID="ConfigurationMenu" runat="server" BackColor="Transparent" BorderWidth="0px"
                                                ForeColor="#8e99a0" StaticMenuItemStyle-ItemSpacing="3px" StaticMenuItemStyle-VerticalPadding="3px"
                                                Width="100%" OnMenuItemClick="ConfigurationMenu_Click">
                                                <StaticSelectedStyle BackColor="#FFDA8C" BorderStyle="Solid" BorderColor="Black"
                                                    BorderWidth="1px" ForeColor="Black" />
                                                <StaticMenuItemStyle HorizontalPadding="5px" ItemSpacing="3px" VerticalPadding="3px"
                                                    CssClass="ClsBorderNoBg ConfigHead" />
                                                <DynamicHoverStyle BackColor="Wheat" />
                                                <DynamicMenuStyle HorizontalPadding="5px" VerticalPadding="5px" />
                                                <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="5px" />
                                                <StaticHoverStyle BackColor="#E6E6E6" Font-Underline="True" ForeColor="Black" CssClass="ClsBorderBlue" />
                                                <Items>
                                                </Items>
                                            </asp:Menu>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ConfigurationMenu" EventName="MenuItemClick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td id="MainDataTable" style="width: 70%" align="left" class="ClsGridRow ClsBorderlight" valign="top">
                    <asp:UpdatePanel runat="server" ID="UPanelGridView" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <div id="divMenu" runat="server" style="width: 100%; overflow: auto;">
                                <asp:GridView ID="grdvwConfigurationMenu" runat="server" BackColor="White" Width="100%"
                                    AutoGenerateColumns="False" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                    GridLines="None" OnRowDataBound="grdvwConfigurationMenu_RowDataBound" DataKeyNames="Configure_Id,Is_Configure,NavigateURL">
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                    </PagerStyle>
                                    <PagerSettings NextPageText="<%$ Resources:LocalizedResources,Next%>" LastPageText="<%$ Resources:LocalizedResources,Last%>" PreviousPageText="<%$ Resources:LocalizedResources,Previous%>"
                                        FirstPageText="<%$ Resources:LocalizedResources,First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources,Status%>">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:ButtonField>
                                        <asp:HyperLinkField DataTextField="Configure_Name" HeaderText="<%$ Resources:LocalizedResources,Configuration%>" SortExpression="Configure_Name"
                                            NavigateUrl="~/Home.aspx">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="85%" CssClass="GridLinks ClspaddingL" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                            <ControlStyle CssClass="GridLinks" />
                                        </asp:HyperLinkField>
                                    </Columns>
                                    <RowStyle CssClass="ClsGridRow" />
                                    <HeaderStyle CssClass="ClsGridHeader" />
                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                </asp:GridView>
                            </div>
                            <asp:HiddenField ID="hidScreenParentId" runat="server"></asp:HiddenField>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ConfigurationMenu" EventName="MenuItemClick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>            
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        var oldMenu_HideItems = Menu_HideItems
        if (oldMenu_HideItems) {
            Menu_HideItems = function(items) {
                if (!items || ((typeof (items.tagName) == "undefined") && (items instanceof Event))) { items = __rootMenuItem; }
                if (items && items.rows && items.rows.length == 0) { items.insertRow(0); }
                return oldMenu_HideItems(items)
            } 
        }

    </script>

</asp:Content>
