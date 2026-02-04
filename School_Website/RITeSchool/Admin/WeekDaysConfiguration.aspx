<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeekDaysConfiguration.aspx.cs"
    Inherits="WeekDaysConfiguration" MasterPageFile="../MasterPages/MasterPage.master"  ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td>
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ShowMessageBox="True" ShowSummary="False"
                        CssClass="LblErrorMsg" />
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView CssClass="GridBorder" ID="grdWeekDaysConfiguration" runat="server" Width="50%" CellPadding="0"
                        CellSpacing="1" AllowPaging="False" AutoGenerateColumns="False" PageSize="20"
                        ForeColor="#333333" GridLines="None" OnRowDataBound="grdWeekDaysConfiguration_RowDataBound"
                        DataKeyNames="WeekDays_Id,School_Id,Original_WeekDays_Id,WeekDay_Short_Name" ViewStateMode="Enabled">
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                            Font-Size="Small"></PagerStyle>
                        <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                            FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="ChkWeekDays" runat="server" type="checkbox" onclick="CheckAllOrUncheckAllGridItems(document,_clientGridId,this,'ChkAllCheckedWeekDays' ) " />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkAllCheckedWeekDays" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="1%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                <HeaderStyle Width="1%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText= "<%$ Resources:LocalizedResources, WeekDayName %>" DataField="WeekDay_Name">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40%" CssClass="ClspaddingL" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Weekday Short Name">
									<ItemTemplate>										
                                        <asp:TextBox ID="txtWeekdayShortName" runat="server" style="width:50%;" onkeypress="return onlyAlphanumeric(event,this);" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqTxtPrefixVal" runat="server" ControlToValidate="txtWeekdayShortName"
                                            Display="None" ErrorMessage="Weekday should not be blank."   ></asp:RequiredFieldValidator>
									</ItemTemplate>
									<ControlStyle CssClass="SmlTxtBox" />
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
									<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
							</asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="ClsGridRow" />
                        <HeaderStyle CssClass="ClsGridHeader" />
                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                    </asp:GridView>
                    <table runat="server" >
                        <tr align="left" runat="server">
                            <td colspan=2 runat="server">
                                <asp:CheckBox ID="IsOtherStaffApplicable" runat="server"  Text=" Is Other Staff Applicable" ViewStateMode="Enabled"/>
                            </td>
                        </tr>
                    </table>
                    <table style="left: 62px; top: 1px">
                        <tr>
                            <td style="height: 28px">
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>" OnClick="btnSave_Click" CssClass="ClsBtn" disable-page="true" />
                            </td>
                            <td style="height: 28px">
                                <asp:Button ID="btnCancel" runat="server" Text= "<%$ Resources:LocalizedResources, Cancel%>" 
                                    CssClass="ClsBtn" CausesValidation="False" UseSubmitBehavior="false" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidConfigurationFlag" runat="server" ViewStateMode="Enabled" />
                    <asp:HiddenField ID = "hidCultureInfo" runat = "server" ViewStateMode="Enabled"/>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                         <span class="LblNrmlB">Note :</span>
                </td>
                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                    <span class="LblSmlV"> 1. If the checkbox "Is Other Staff Applicable" is checked then those Weekdays/Weekends are applicable to all the staff 
                    and all the changes made on User Weekend Association screen will override.</span><br/>
                    <span class="LblSmlV"> 2. If anyone wanted to change their own weekends they can change from User Weekend Association screen.</span>
                </td>
             </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdWeekDaysConfiguration.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"             
    </script>
    <script src="../Scripts/Admin/WeekDaysConfiguration.js" type="text/javascript"></script>
</asp:Content>
