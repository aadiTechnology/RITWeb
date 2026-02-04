<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SortSubSubjectPopup.aspx.cs" Inherits="SortSubSubjectPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1" style="height: 20px">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <span class="MainTitleHead">Skills / Behaviour Sort Order</span>
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
            <tr align="center">
                <td>
                    <table>
                        <tr>
                            <td align="left" class="ClsOnlyBorderlght">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Module Name : " EnableViewState="false"
                                    Width="150px"></asp:Label>
                                <span class="ClsMdtStar" style="color: #ff0000"></span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbModuleName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbModuleName_SelectedIndexChanged"
                                    CssClass="LrgCombo" TabIndex="2">
                                </asp:DropDownList>
                            </td>
                            <td id="tdSubjectName" runat="server" align="left" class="ClsOnlyBorderlght">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Pre-Primary Subject Name : "
                                    EnableViewState="False" Width="161px"></asp:Label>
                                <span class="ClsMdtStar" style="color: #ff0000"></span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbSubjectName" runat="server" CssClass="LrgCombo" 
                                    TabIndex="2" AutoPostBack="True" 
                                    onselectedindexchanged="cmbSubjectName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center"><td>
            <asp:Button ID="btnShow" Text="show" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="imgBtnShow_Click" UseSubmitBehavior="false" />
            </td></tr>
            <tr align="center" valign="top">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center" valign="top">
                <td valign="top" align="center">
                    <div id="divSubSubjectGrid" runat="server" class="GridBorder" style="text-align: center; width: 50%; overflow: auto;">
                        <asp:GridView ID="grdSubSubject" runat="server" Width="100%" AutoGenerateColumns="False"
                            PageSize="100" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                            DataKeyNames="SubSubjectId" EmptyDataText="No Records Found." OnRowDataBound="grdSubSubject_RowDataBound"
                            OnPageIndexChanging="grdSubSubject_PageIndexChanging" OnRowCreated="grdSubSubject_RowCreated">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField DataField="SubSubjectName" HeaderText="Skills / Behaviour">
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
        _clientGridId = "<%=this.grdSubSubject.ClientID %>"
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
