<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SurveyFormDetailsUI.aspx.cs" Inherits="SurveyFormDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td align="center">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td id="tdMessage" runat="server">
                                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" style="font-size:14px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trTotalRec" align="center">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwForms">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwForms" runat="server" DataKeyNames="Id" OnDataBound="lstvwForms_DataBound"
                                            OnItemDataBound="lstvwForms_ItemDataBound" OnItemCommand="lstvwForms_ItemCommand"
                                            OnSorting="lstvwForms_Sorting">
                                            <LayoutTemplate>
                                                <table width="98%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" class="clsLabelgrd" width="150px" style="padding-left: 5px;">
                                                            <asp:LinkButton ID="lnkRegNo" runat="server" CommandName="Sort" CommandArgument="RegNo"
                                                                CausesValidation="false" ForeColor="Black"> Registration No. </asp:LinkButton>
                                                        </th>
                                                        <th align="left" class="clsLabelgrd" style="padding-left: 5px;">
                                                            <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                CausesValidation="false" ForeColor="Black" Text="Name"></asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="100px" class="clsLabelgrd">
                                                            <span>Gender</span>
                                                        </th>
                                                        <th align="center" width="100px" class="clsLabelgrd">
                                                            <span>Mobile No. 1</span>
                                                        </th>
                                                        <th align="left" style="padding-left: 5px;" class="clsLabelgrd" width="400px">
                                                            <span>School</span>
                                                        </th>
                                                        <th align="left" width="100px" style="padding-left: 5px;" class="clsLabelgrd">
                                                            <span>Standard</span>
                                                        </th>
                                                        <th align="left" width="100px" style="padding-left: 5px;" class="clsLabelgrd">
                                                            <span>Category</span>
                                                        </th>
                                                        <th width="50px" align="center" class="clsLabelgrd">
                                                            <asp:Label ID="lblEdit" runat="server" Text="Edit"> </asp:Label>
                                                        </th>
                                                        <th width="50px" class="clsLabelgrd">
                                                            <asp:Label ID="lblDelete" runat="server" Text="Delete"> </asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                        <td colspan="9">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwForms" PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td align="right" class="LblNormal">
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblRegNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("RegNo") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidQueryString" Value="" runat="server" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("Gender") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblMobileNo1" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                            Text='<%#Eval("MobileNo1") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblSchoolName" runat="server" CssClass="ClsLabelL" Text='<%#Eval("School") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabelL" Text='<%#Eval("Standard") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblCategory" runat="server" CssClass="ClsLabelL" Text='<%#Eval("Category") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblRegNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("RegNo") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidQueryString" Value="" runat="server" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("Gender") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblMobileNo1" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                            Text='<%#Eval("MobileNo1") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblSchoolName" runat="server" CssClass="ClsLabelL" Text='<%#Eval("School") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabelL" Text='<%#Eval("Standard") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblCategory" runat="server" CssClass="ClsLabelL" Text='<%#Eval("Category") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.SurveyStudentBL" EnablePaging="True"
                                            ID="objdsSurvey" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                            SelectCountMethod="Count" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="Int32" />
                                                <asp:Parameter Name="sortExpression" Type="String" />
                                                <%--<asp:Parameter Name="sortDirection" Type="String" />--%>
                                                <%--<asp:ControlParameter ControlID="hidSortExpression" Name="asSortExpression" Type="String" />--%>
                                                <asp:ControlParameter ControlID="hidSortDirection" Name="sortDirection" Type="String" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <%--   <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ClsBtn" />
                    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" OnClick="btnExport_Click" />
                      <asp:Button ID="btnSendSMS" runat="server" OnClientClick="OpenSendSMSPopup(); return false;" Text="Send SMS" CssClass="ClsBtn"  />
                </td>
            </tr>
        </table>
        <script type="text/javascript">

            function OpenSendSMSPopup() {

                window.open('SurveySendSMSPopup.aspx?', '_blank', 'width=600, height=590,scrollbars=yes,resizable=no,left=' + ((screen.width - 600) / 2) + ', top=' + ((screen.heigth - 700) / 2)).focus();
            }

            function OpenPopup(rowIndex, isEditMode) {
                if (isEditMode == 0)
                    window.open('SurveyFormPopup.aspx?', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=650,height=530').focus();
                else {
                    var queryString = $get("<%=this.lstvwForms.ClientID %>" + '_ctrl' + rowIndex + '_hidQueryString')
                    window.open('SurveyFormPopup.aspx?' + queryString.value, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=650,height=530').focus();
                }
                return false;
            }

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?')
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
