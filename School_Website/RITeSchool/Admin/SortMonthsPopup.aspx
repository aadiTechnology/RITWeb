<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SortMonthsPopup.aspx.cs" Inherits="SortMonthsPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1" style="height: 20px">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <span class="MainTitleHead">Months Sort Order</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center" valign="middle" style="height: 30px">
                <td>
                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                        Visible="true" EnableViewState="false" CssClass="ClsLabel"></asp:Label>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td valign="top" align="center">
                    <div id="div1" class="GridBorder" style="text-align: center; width: 50%; overflow: auto;">
                        <asp:GridView ID="grdMonths" runat="server" Width="100%" AutoGenerateColumns="False"
                            PageSize="100" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                            DataKeyNames="PrePrimaryProgressReportMonthId,SortOrder" EmptyDataText="No Records Found."
                            OnRowDataBound="grdMonths_RowDataBound">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField DataField="Month" HeaderText="Month">
                                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Sort Order">
                                    <ItemTemplate>
                                        <select id="ddlOrder" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    <asp:Button ID="imgBtnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="imgBtnSave_Click" UseSubmitBehavior="false" />
                    <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="False" UseSubmitBehavior="false" />
                    <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdMonths.ClientID %>"
        _clientimgBtnSave = "<%=this.imgBtnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        function DisableButtons() {
            window.close()
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }
    </script>

</asp:Content>
