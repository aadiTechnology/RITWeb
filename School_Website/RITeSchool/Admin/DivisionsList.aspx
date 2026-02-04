<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="DivisionsList.aspx.cs" Inherits="DivisionsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td align="center">
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ShowMessageBox="True" ShowSummary="False"
                        CssClass="LblErrorMsg" ForeColor="" />
                    <asp:CustomValidator ID="cst_Division" runat="server" ClientValidationFunction="CstDuplicateTextValidation"
                        CssClass="LblErrorMsg" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, DivisionNameAlreadyExists %>"></asp:CustomValidator>
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label></td>
                <td>
                </td>
            </tr>
            <tr align="center">
                <td colspan="2" align="center">
                    <div id="div1" class="GridBorder" style="width: 50%; height: 189pt; overflow: auto;">
                        <asp:GridView ID="grdDivisions" UseAccessibleHeader="true" runat="server" Width="100%"
                            AutoGenerateColumns="False" PageSize="50" AllowPaging="False" OnRowDataBound="grdGroupDetails_RowDataBound"
                            CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" DataKeyNames="Division_Name,Division_Id,Original_Division_Id,School_Id">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                Font-Size="Small"></PagerStyle>
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText %>" LastPageText="<%$ Resources:LocalizedResources, LastPageText  %>" 
                            PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText  %>" FirstPageText="<%$ Resources:LocalizedResources, FirstPageText  %>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:TemplateField >
                                    <HeaderTemplate>
                                        <input id="ChkAllDel" type="checkbox" runat="server" onclick="CheckAllOrUncheckAllGridItems(document,_clientGridId,this,'ChkBoxDelete')" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkBoxDelete" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                    <HeaderStyle Width="30px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, DivisionListgrdDivisionsHeader %>" SortExpression="Division_Name">
                                    <EditItemTemplate>
                                        &nbsp;
                                    </EditItemTemplate>
                                    <ItemStyle Width="92%" Wrap="False" />
                                    <HeaderStyle Wrap="False" />
                                    <ItemTemplate>
                                        &nbsp;<asp:TextBox ID="txtDivisionName" runat="server" MaxLength="15" CssClass="SmlTxtBox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqTxtPrefixVal" runat="server" ControlToValidate="txtDivisionName"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, DivisionNameShouldNotBeBlank %>"   ></asp:RequiredFieldValidator>&nbsp;
                                    </ItemTemplate><ItemStyle HorizontalAlign="Left" CssClass="paddingLSML"/>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" BorderWidth="0px" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr align="center">
                <td align="center" colspan="2"><asp:Button ID="imgBtnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server" CssClass="ClsBtn" BorderWidth="1px"
            OnClick="imgBtnSave_Click" disable-page="true" />
        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" BorderWidth="1px"
            CausesValidation="False" UseSubmitBehavior="false" />
                </td>
            </tr>
            <asp:HiddenField ID="hidCultureInfo" runat="server" />
        </table>
    </div>
    
    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdDivisions.ClientID %>"
        _clientimgBtnSave = "<%=this.imgBtnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        var Page_IsValid = true;
        function ConfirmAction(iPageCount, sActionName) {
        	 Page_IsValid = true;
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxDelete', sActionName, 'false', iPageCount, 'true')) {
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
                 
            }
            else {
            		bResult = false;
            		Page_IsValid = false;
			 }
            return bResult
        }
        function DisableButtons() {
            document.getElementById(_clientimgBtnSave).disabled = true
            document.getElementById(_clientbtnCancel).disabled = true
            __doPostBack(document.getElementById(_clientbtnCancel).name, '')
        }
        function CstDuplicateTextValidation(oSrc, args) {
            if (DuplicateTextValidation(document, _clientGridId, "txtDivisionName", "ChkBoxDelete", false)) {
                args.IsValid = true
                return false
            }
            else {
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
    </script>
</asp:Content>
