<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="TeacherSubjectAssignmentUI.aspx.cs" Inherits="TeacherSubjectAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td align="center">
                    <table id="LegendTable" runat="server">
                        <tr>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                    Text="<%$ Resources:LocalizedResources, Legend %>" EnableViewState="false"></asp:Label></td>
                            <td align="left" colspan="1">
                                <asp:Label ID="TextBox1" runat="server" BackColor="#e6e9c7" Height="20px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px">
                                    <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, NoTeacherIsAssociated %>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px">
                            </td>
                            <td align="right">
                                <asp:Label ID="TextBox2" runat="server" BackColor="#5DAD8E" Height="20px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px">
                                    <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label></td>
                            <td align="left">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, TeacherNotAssigned %>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                            </td>
                            <td align="left" style="width: 5px">
                            </td>
                            <td align="left">
                                <asp:Label ID="TextBox3" runat="server" BackColor="#eaeaea" Height="20px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px">
                                    <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label></td>
                            <td align="left">
                                <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, SubjectNotApplicable %>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr id = "trStandard" runat="server">
                        <td align="center">
                            <table>
                                <tbody>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            <asp:Label ID="lblStandard" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, SelectStandard%>"></asp:Label>
                                            <span class="colonPadding ClsLabel">:</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStandard" runat="server" Width="100px" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ClsBorderlight">
                                            <asp:Label ID="lblCategory" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, Category%>"></asp:Label>
                                            <span class="colonPadding clsLabel">:</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="LrgCombo" AutoPostBack="true" OnSelectedIndexChanged = "ddlStandard_SelectedIndexChanged">
                                            <asp:ListItem Value="1" Text="Teacher Name"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Subject Name"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ClsBorderlight">
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel" Text="Name"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" runat ="server" class="LargeTextbox" Text=" " MaxLength="100"></asp:TextBox>
                                        </td>
                                        <td>
                                             <asp:Button ID="btnsearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
            <tr>
                <td align="center">
                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>                

                    <div id="GridViewScrollContainer" runat="server" class="GridBorder" style="width: 635pt; overflow: scroll;">
                        <asp:GridView ID="grdStandardDivision" UseAccessibleHeader="true" runat="server"
                            AutoGenerateColumns="False" PageSize="1000" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                            GridLines="None" DataKeyNames="Standard_Id,Schoolwise_Standard_Division_Id" EnableViewState="false"
                            OnRowDataBound="grdStandardDivision_RowDataBound" AllowPaging="false">
                            <Columns>
                                <asp:BoundField HeaderText="  " SortExpression="StandardDivision" DataField="StandardDivision">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnsearch" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>

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
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn" 
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidTeacherName" runat="server"></asp:HiddenField>      
    </div>
    </asp:Content>
