<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GRNListUI.aspx.cs" Inherits="GRNListUI" Title="Untitled Page" %>

<asp:Content ID="CntGRN" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" style="width: 800px">
        <table width="100%" align="center">
            <tr>
                <td  align="left" style="background-color: white;" valign="top"
                    colspan="4">                   
                    <asp:ValidationSummary ID="valsumItems" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                        ShowSummary="True" ValidationGroup="valsumItems" />
                </td>
            </tr>          
            <tr>
                <td>
                    <asp:UpdatePanel ID="UPanelGRNList" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr id="trLstItems" runat="server">
                                    <td>
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr id="trPagerGRNCnt" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwGRNList">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                                                            <br />
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:ListView ID="lstvwGRNList" runat="server"  OnDataBound="lstvwGRNList_DataBound"
                                                                OnItemCommand="lstvwGRNList_ItemCommand" 
                                                                OnItemDataBound="lstvwGRNList_ItemDataBound" 
                                                                onsorting="lstvwGRNList_Sorting" DataSourceID="ObjDSGRNList">
                                                                <LayoutTemplate>
                                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                        cellspacing="0" class="GridBorder">
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1">
                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                        <th id="thCode" runat="server" align="left" class="ClspaddingL" style="width: 15%">
                                                                                            <asp:LinkButton ID="lnkSortCode" runat="server" CommandName="Sort" CommandArgument="GRNCode"
                                                                                                ForeColor="Black">
                                                                                                  GRN Code</asp:LinkButton>
                                                                                        </th>
                                                                                        <th id="thName" runat="server" align="left" class="ClspaddingL" style="width: 50%">
                                                                                            <asp:LinkButton ID="lnkSortName" runat="server" CommandName="Sort" CommandArgument="GRNName"
                                                                                                ForeColor="Black">
                                                                                                  GRN Name</asp:LinkButton>
                                                                                        </th>
                                                                                        <th id="thDate" runat="server">
                                                                                            <asp:LinkButton ID="lnkSortDate" runat="server" CommandName="Sort" CommandArgument="Insert_Date"
                                                                                                ForeColor="Black">
                                                                                                  Created Date</asp:LinkButton>
                                                                                        </th>
                                                                                        <th id="thEdit" runat="server" style="width: 10%">
                                                                                            View
                                                                                        </th>
                                                                                        <th id="thDelete" runat="server" style="width: 10%">
                                                                                            Delete
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                                                        <td colspan="5">
                                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwGRNList" PageSize="20">
                                                                                                <Fields>
                                                                                                    <asp:TemplatePagerField>
                                                                                                        <PagerTemplate>
                                                                                                            <table width="100%">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
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
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="trItem" runat="server" class="ClsGridRow">
                                                                        <td align="left" id="tdCode" runat="server">
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("GRNCode") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="left" id="tdName" runat="server">
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("GRNName") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="center" id="tdDate" runat="server">
                                                                            <asp:Label ID="lblReqDate" runat="server" Text='<%#Eval("Insert_Date","{0:dd-MMM-yyyy}")%>' />
                                                                        </td>
                                                                        <td align="center" id="tdEdit" runat="server">
                                                                            <asp:ImageButton ID="imgbtnViewGRN" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" 
                                                                                ToolTip = "View" />
                                                                        </td>
                                                                        <td align="center" id="tdDelete" runat="server">
                                                                            <asp:ImageButton ID="imgbtnDeleteGRN" CommandArgument='<%# Eval("GRNID") %>' runat="server"
                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove" ToolTip = "Delete"/>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" id="tdCode" runat="server">
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("GRNCode") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="left" id="tdName" runat="server">
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("GRNName") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="center" id="tdDate" runat="server">
                                                                            <asp:Label ID="lblReqDate" runat="server" Text='<%#Eval("Insert_Date","{0:dd-MMM-yyyy}")%>' />
                                                                        </td>
                                                                        <td align="center" id="tdEdit" runat="server">
                                                                            <asp:ImageButton ID="imgbtnViewGRN" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                ToolTip="View" />
                                                                        </td>
                                                                        <td align="center" id="tdDelete" runat="server">
                                                                            <asp:ImageButton ID="imgbtnDeleteGRN" CommandArgument='<%# Eval("GRNID") %>' runat="server"
                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove" ToolTip="Delete"/>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td class="LblNoRecord" align="center">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ObjectDataSource TypeName="BusinessLogic.GRNDetailsBL" EnablePaging="true"
                                                                ID="ObjDSGRNList" runat="server" SelectMethod="GetGRNList" SortParameterName="sortExpression"
                                                                SelectCountMethod="CountTotalGRN" EnableCaching="false">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:SessionParameter Name="aiUserId" SessionField="I_USER_ID" Type="int32" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwGRNList" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnAdd" runat="server" BorderStyle="Solid" BorderWidth="1px" 
                                            CssClass="ClsBtnSml" Height="24px"
                                            CausesValidation="false" Text="Add" Visible="True" 
                                            PostBackUrl="~/RITeSchool/Inventory/GRNDetailsUI.aspx" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwGRNList" EventName="ItemCommand" />
                       </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    
    <script language="javascript" type="text/javascript">
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this GRN?')) {
                bResult = false
            }
            return bResult
        }
    </script>
</asp:Content>
