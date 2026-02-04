<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SubjectGroupsListUI.aspx.cs" Inherits="SubjectGroupsListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
        <tr>
            <td>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" CssClass="LblErrorMsg" />
            </td>
        </tr>
        <tr align="center">
            <td align="center">
                <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                    ID="UpdatePanel2">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <table width="65%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" EnableViewState="False"
                                                    ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="tblClassCombo" runat="server">
                                        <tr>
                                            <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                                colspan="1">
                                                <span class="ClsLabel">
                                                    <asp:Label ID="lblClass" runat="server" Text="<%$ Resources:LocalizedResources, Class %>"></asp:Label><span
                                                        id="Span1" class="colonPadding">:</span> </span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cmbClass" runat="server" CssClass="SmlCombo" AutoPostBack="true"
                                                    Height="22px" Width="125px" OnSelectedIndexChanged="cmbClass_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="tdSubjectGroups" runat="server" visible="false">
                                    <div class="GridBorder ClsGridBG" id="divGridView" runat="server" style="width: 65%;
                                        height: 205pt;">
                                        <asp:GridView ID="GrdSubjectGroup" AllowSorting="true" DataKeyNames="parent_Subject_id,Parent_Group_Id,parent_Subject_Name"
                                            runat="server" OnRowCreated="grdSubjectGroups_RowCreated" OnSorting="GrdSubjectGroup_sorting"
                                            OnRowCommand="grdStudents_RowCommand" PageSize="30" CellPadding="0" CellSpacing="1"
                                            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowDataBound="grdSubjectGroups_DataBound"
                                            EmptyDataText="<%$ Resources:LocalizedResources, NoRecordsFound%>" Width="100%">
                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                            </PagerStyle>
                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <input id="ChkAllDel" type="checkbox" runat="server" onclick="CheckAllOrUncheckAllGridItems(document,_clientGridId,this,'chkDelete')" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDelete" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" />
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="paddingLSML" Width="1%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="parent_Subject_Name" HeaderText="<%$ Resources:LocalizedResources, ParentSubject %>" SortExpression="parent_Subject_Name">
                                                    <HeaderStyle Width="25%" HorizontalAlign="left" CssClass="paddingLSML" />
                                                    <ItemStyle HorizontalAlign="left" CssClass="paddingLSML" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, ChildSubjects %>">
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                    <HeaderStyle Wrap="True" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtSubjectNames" runat="server" EnableViewState="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:ButtonField CommandName="EditGroup" HeaderText="<%$ Resources:LocalizedResources, Edit %>" Text="Edit Group" ButtonType="Image"
                                                    ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif">
                                                    <HeaderStyle Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <RowStyle CssClass="ClsGridRow" />
                                            <HeaderStyle CssClass="ClsGridHeader" />
                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        </asp:GridView>
                                    </div>
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidIsConfigured" runat="server" />
                                      <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divErr">
                                    </div>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" colspan="1">
                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                        CssClass="ClsBtn" OnClick="btnBack_Click" CausesValidation="False" UseSubmitBehavior="false" />
                                    <asp:Button ID="btnAdd" runat="server" Enabled="false" Text="<%$ Resources:LocalizedResources, Add %>"
                                        CssClass="ClsBtn" OnClick="btnAdd_Click" UseSubmitBehavior="false" />
                                    <asp:Button ID="btnDelete" runat="server" Visible="false" Text="<%$ Resources:LocalizedResources, Delete %>"
                                        CssClass="ClsBtn" OnClick="btnDelete_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <asp:HiddenField ID="hidAreYouSureUWantTODeleteGroups" runat="server"/>
    </table>
    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.GrdSubjectGroup.ClientID %>"
        _clientbtnAdd = "<%=this.btnAdd.ClientID %>"
        _clientbtnDelete = "<%=this.btnDelete.ClientID %>"
        _clientbtnBack = "<%=this.btnBack.ClientID %>"
        function ConfirmDelete(iPageCount, sActionName, objBtn) {
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'chkDelete', sActionName, 'false', iPageCount, 'true')) {
                if (!window.confirm(document.getElementById("<%=hidAreYouSureUWantTODeleteGroups.ClientID %>").value)) {
                    bResult = false
                }
                else if (bResult == true) {
                    document.getElementById(_clientbtnAdd).disabled = true
                    document.getElementById(_clientbtnDelete).disabled = true
                    document.getElementById(_clientbtnBack).disabled = true
                    __doPostBack(objBtn.name, '')
                }
            }
            else
            { bResult = false; }
            return bResult
        }
    </script>
</asp:Content>
