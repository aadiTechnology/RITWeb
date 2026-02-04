<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestsSortOrderPopUp.aspx.cs"
    Inherits="TestsSortOrderPopUp" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1" style="height: 20px">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                               <asp:Label ID="lblExamSortOrder" class="MainTitleHead" runat="server" Text="<%$ Resources:LocalizedResources, ExamSortOrder%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center" valign="middle" style="height: 30px">
                <td>
                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Blue" Height="20px" 
                        Width="100%" Visible="true" EnableViewState="false" CssClass="ClsLabel"></asp:Label>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td valign="top" align="center">
                    <table>
                        <tr>
                            <td align="center" colspan="1" class="ClsOnlyBorderlght">
                                <asp:Label ID="lblStandard" class="ClsLabel" runat="server" Text="<%$ Resources:LocalizedResources,Standard%>"></asp:Label>
                                <span class="colonPadding">:</span>
                                <span class="ClsMdtStar" style="color: #ff0000"></span>
                            </td>
                            <td align="center" colspan="1" >
                                <asp:DropDownList ID="cmbStandard" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged"
                                    CssClass="SmlCombo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 5px">
                            <td style="height: 5px" colspan="2">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td valign="top" align="center">
                    <div id="div1" class="GridBorder" style="text-align:center; width: 50%; overflow: auto;">
                        <asp:GridView ID="grdExam" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnRowDataBound="grdExam_RowDataBound" PageSize="100" CellPadding="0" CellSpacing="1"
                            ForeColor="#333333" GridLines="None" DataKeyNames="SchoolWise_Test_Id,Original_SchoolWise_Test_Id,Sort_Order"
                             EmptyDataText="No Records Found." OnRowCreated="grdExam_RowCreated">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources,Next%>" LastPageText="<%$ Resources:LocalizedResources,Last%>" PreviousPageText="<%$ Resources:LocalizedResources,Previous%>"
                                FirstPageText="<%$ Resources:LocalizedResources,First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField DataField="SchoolWise_Test_Name" HeaderText="<%$ Resources:LocalizedResources,ExamName%>">
                                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources,SortOrder%>">
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
                            <EmptyDataRowStyle  BackColor="#E6EEFC" CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    &nbsp;</td>
            </tr>
            <tr align="center" valign="top">
                <td>
                    <asp:Button ID="imgBtnSave" Text="<%$ Resources:LocalizedResources,Save%>" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="imgBtnSave_Click" disable-page="true" /><asp:Button ID="btnCancel"
                            runat="server" Text="<%$ Resources:LocalizedResources,Close%>" CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False"
                            UseSubmitBehavior="false" />
                    <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdExam.ClientID %>"
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
