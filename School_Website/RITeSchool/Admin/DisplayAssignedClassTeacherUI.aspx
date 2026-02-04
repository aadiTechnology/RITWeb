<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="DisplayAssignedClassTeacherUI.aspx.cs" Inherits="DisplayAssignedClassTeacherUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" cellpadding="0" cellspacing="0" width="97%">
           
            <tr>
                <td align="center">
                    <table id="LegendTable" runat="server">
                        <tr>
                            <td align="left" colspan="1">                           
                            <span class="ClsLblLgnd"><asp:Label ID="Label" runat="server" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label></span>
                            </td>
                            <td align="left" colspan="1">
                                &nbsp;<asp:Label ID="txtDivisionNotApplicable" runat="server" BackColor="#eaeaea"
                                    Height="20px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True"
                                    Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left" colspan="1">
                                <span class="ClsTextNormal" style="Font-Weight:bold"><asp:Label ID="lblDivisionText" runat="server" Text="<%$ Resources:LocalizedResources, DivisionNotApplicable %>" EnableViewState="false"></asp:Label>
                                </span>
                                    </td>
                            <td align="right" style="width: 5px">
                            </td>
                            <td align="right">
                                <asp:Label ID="Label1" runat="server" BackColor="#5dad8e" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" EnableViewState="False" Height="20px" ReadOnly="True" Width="20px"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left">
                                 <span class="ClsTextNormal" style="Font-Weight:bold"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, ClassTeacherNotAssigned %>" EnableViewState="false"></asp:Label></span>
                            </td>
                            <td align="left" style="width: 5px">
                            </td>
                            <td align="left">
                                <asp:Label ID="Label5" runat="server" BackColor="#A8B39D" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" EnableViewState="False" Height="20px" ReadOnly="True" Width="20px">
                                    <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                            </td>
                            <td align="left" valign="middle">
                             <span class="ClsTextNormal" style="Font-Weight:bold"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, LgdNoTeacherAvailable %>" EnableViewState="false"></asp:Label></span>
                             </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr  id="Tr1" enableviewstate ="false">
                <td align="center" colspan="1" style="height: 5px">
                </td>
            </tr>
            <tr>
                <td align="center"  visible="true" runat="server" id="tdGrid">
                    <div id="GridViewScrollContainer" class="GridBorder" style="width: 635pt; overflow: scroll;">
                        <asp:GridView ID="grdStandards" UseAccessibleHeader="true" runat="server" AutoGenerateColumns="false" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                            DataKeyNames="Standard_Id" OnRowDataBound="grdStandards_RowDataBound" EnableViewState="False">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField HeaderText="  " SortExpression="Standard_Name" DataField="Standard_Name">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
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
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn" OnClick="btnBack_Click" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidTeacherName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
    </div>

    <script language="javascript" type="text/javascript">        
    _clientGridId = "<%=this.grdStandards.ClientID %>"
        function saveChk(msg) {
            if (ChkIfAtleastOneCheckedInEachRow(document, _clientGridId)) {
                return true
            }
            else {
                alert(msg)
                return false
            }
        }        
        function ConfirmAction(iPageCountStandard, iPageCountDivision, sActionName) {
            var bResult = false
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientgrdDivisionsId, 'ChkBoxDelete', sActionName, 'false', iPageCountDivision, 'true')) {
                bResult = true
            }
            else
            { bResult = false; }
            return bResult
        }
    </script>
</asp:Content>
