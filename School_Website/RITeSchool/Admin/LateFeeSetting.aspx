<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LateFeeSetting.aspx.cs" Inherits="LateFeeSetting"
    MasterPageFile="../MasterPages/MasterPage.master" %>

<%@ OutputCache VaryByParam="none" Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" border="0" align="center">
            <tr id="trerr" runat="server">
                <td align="center">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg"  EnableViewState="false"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table id="LegendTable" runat="server" cellpadding="2" cellspacing="2">
                        <tr>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                    Text="<%$ Resources:LocalizedResources, Legend%>" EnableViewState="false"></asp:Label></td>
                            <td align="left" colspan="1" style="padding-right: 3px">
                                <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" CssClass="ClsConfigNotdone" Height="20px" ReadOnly="True" Text=" "
                                    Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LateFeeNotConfigured %>"
                                    EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px">
                            </td>
                            <td align="left" colspan="1" style="padding-right: 3px">
                                <asp:Label ID="TextBox2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" CssClass="ClsUpdate" Height="20px" ReadOnly="True" Text=" "
                                    Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsTextNormal" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LateFeeConfigured %>"
                                    EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px">
                            </td>
                            <td align="right" style="width: 5px">
                            </td>
                            <td align="left" colspan="1" style="padding-right: 3px">
                                <asp:Label ID="TextBox3" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" CssClass="ClsNotAssignDark height20" Height="20px" ReadOnly="True" Text=" "
                                    Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsTextNormal" Font-Bold="True" Text="<%$ Resources:LocalizedResources, StandardwiseFeeTypeNotConfigured %>"
                                    EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divGridView" runat="server" style="width: 60%;" class="GridBorder">
                        <asp:GridView ID="grdStandards" runat="server" AutoGenerateColumns="False" Height="43px"
                            PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                            DataKeyNames="original_standard_id,standard_id,standard_name" EnableViewState="False">
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next %>" LastPageText="<%$ Resources:LocalizedResources, Last %>" 
                            PreviousPageText="<%$ Resources:LocalizedResources, Previous %>" FirstPageText="<%$ Resources:LocalizedResources, First %>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                            <Columns>
                                <asp:BoundField DataField="standard_name" HeaderText="<%$ Resources:LocalizedResources, Standard %>">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                        </asp:GridView>
                    </div>
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
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn" OnClick="btnCancel_Click"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidStandardId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidCultureInfo" runat="server"  />
    </div>
    
</asp:Content>
