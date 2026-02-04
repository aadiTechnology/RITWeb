<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="WeekDayTimeTable.aspx.cs" Inherits="WeekDayTimeTable" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
    <a id="top"></a>
        <table width="97%" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center">
                    <table id="LegendTable" runat="server">
                        <tr>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                    Text="Legend" EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px;">
                            </td>
                            <td align="left" colspan="1">
                                <asp:Label ID="TextBox1" runat="server" CssClass="TTNotAssignDark" Height="20px"
                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Text=" Lecture Not Applicable"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px;">
                            </td>
                            <td align="left" colspan="1">
                                <asp:Label ID="TextBox2" runat="server" CssClass="TTNotAssignLght" Height="20px"
                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text=" Teacher is not class Teacher"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ValidationSummary ID="valSumErrorMsg" ValidationGroup="show" runat="server"
                        CssClass="ClsLabel" />
                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                        ID="UpdatePanel1">
                        <ContentTemplate>
                            <asp:Label ID="lblError" CssClass="ClsLabel" runat="server" EnableViewState="False"
                                ForeColor="Red" Width="100%" Visible="False"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <asp:Panel ID="pnlFields" runat="server">
                <tr>
                    <td align="center">
                        <table runat="server" id="tblInputFields" cellpadding="0" cellspacing="1">
                            <tr runat="Server" id="trStandard">
                                <td class="ClsBorderlight">
                                    <asp:Label ID="Label1" runat="server" EnableViewState="false">Select  Weekday :</asp:Label>
                                </td>
                                <td class="ClsBorderlight">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbWeekDay" runat="server" AutoPostBack="false" CssClass="MidCombo">
                                            </asp:DropDownList><span style="font-size: 9pt; color: #ff0000"> * </span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:CompareValidator ValidationGroup="show" ID="cmp_standard" runat="server" ControlToValidate="cmbWeekDay"
                                        Display="None" ErrorMessage="Week day should be selected." Operator="NotEqual"
                                        ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                        <ContentTemplate>
                                            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtnLrg" Height="24px"
                                                CausesValidation="true" ValidationGroup="show" TabIndex="1" OnClick="btnShow_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
            </asp:Panel>
            <tr>
                <td>
                    <div runat="server" id="div1">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                        ID="uPnl">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="center">
                                        <div id="divTimeTable" runat="server" visible="false">
                                            <div id="GridViewScrollContainer" style="width: 850px; overflow: scroll">
                                                <asp:GridView ID="grdTemp" runat="server" AutoGenerateColumns="false" PageSize="200"
                                                    BackColor="#5c6f7b" DataKeyNames="Teacher_Id" CellPadding="0" CellSpacing="1"
                                                    ForeColor="#333333" GridLines="None">
                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                    </PagerStyle>
                                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                    <Columns>
                                                        <asp:BoundField DataField="TeacherName" HeaderText="Teacher Name" SortExpression="Standard_Name">
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <RowStyle CssClass="TTCells" />
                                                    <HeaderStyle CssClass="UsrGridHead" />
                                                    <AlternatingRowStyle CssClass="TTCells" />
                                                    <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                </asp:GridView>
                                            </div>
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
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" Height="24px"
                                                        CausesValidation="true" TabIndex="1" OnClick="btnSave_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientcstbtnSave = "<%=this.btnSave.ClientID%>"
        function DisableButtons(objBtn) {
            var isPageValid = true
            if (objBtn == document.getElementById(_clientcstbtnSave)) {
                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate()
                } 
            }
            if (isPageValid) {
                if (document.getElementById(_clientcstbtnSave) != null)
                    document.getElementById(_clientcstbtnSave).disabled = true
                
                __doPostBack(objBtn.name, '')
            } 
        }
        function GoToTop() {
            var str
            str = window.location.href
            var iIndex = str.lastIndexOf("/#")
            if (iIndex != -1) {
                str = str.substr(0, iIndex)
            }
            str = str + "/#top"
        }
    </script>
</asp:Content>
