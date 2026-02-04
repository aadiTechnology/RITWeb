<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="ReservedBookDetailsPopUpUI.aspx.cs" Inherits="ReservedBookDetailsPopUpUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%">
        <tr>
            <td align="left" valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="98%">
                    <tr>
                        <td class="ClsGrayMainTitle" style="width: 100%;">
                            <span class="MainTitleHead" style="font-weight: bold">Claimed Book Details</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="False"
                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr align="center">
            <td>
                <table>
                    <tr>
                        <td class="ClsBorderlight paddingL" style="width:100px;">
                            <span class="ClsLbl">User Name:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" CssClass="LrgTxtBox" runat="server"></asp:TextBox>
                        </td>
                        <td class="ClsBorderlight paddingL" style="width:100px;">
                            <span class="ClsLbl">Book Title:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBookTitle" CssClass="LrgTxtBox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="Search" OnClick="btnSearch_Click" />
                     <%--   </td>
                        <td>--%>
                             <asp:Button ID="btnClear" runat="server" CssClass="ClsBtn" Text="Clear"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       
        <tr>
            <td align="left" id="tdChkAll" runat="server" >
                 <asp:CheckBox ID="chkShowAll" runat="server"  AutoPostBack="true"
                      OnCheckedChanged ="chkShowAll_CheckedChanged"/>
                 <span class="ClsBorderlight" ><b>Show all claimed books by all users</b></span>
            </td>
        </tr>
        <tr style="width: 100%;">
            <td align="center">
                <asp:ListView ID="lstvwReservedBooks" runat="server" DataKeyNames="Book_Id,UserId,IsForParent"
                    OnDataBound="lstvwReservedBooks_DataBound" OnItemCommand="lstvwReservedBooks_ItemCommand"
                    OnItemDataBound="lstvwReservedBooks_ItemDataBound">
                    <LayoutTemplate>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwReservedBooks"
                                        PageSize="20">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                        Text="<%# Container.StartRowIndex + 1%>"></asp:Label>
                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                        Text="To "></asp:Label>
                                                    <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"></asp:Label>
                                                    <asp:Label ID="lblOutOf" CssClass="LblNormal" runat="server" Text="Out Of"></asp:Label>
                                                    <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>"></asp:Label>
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records"></asp:Label>
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                        <table align="center" width="100%" runat="server" id="tblShiftInfo" style="color: #333333"
                            cellpadding="0" cellspacing="1" class="GridBorder">
                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                <th align="left" class="paddingLSML" style="width:16%;">
                                    <asp:LinkButton ID="lnkSortCode" runat="server" CommandName="S_SORT" CommandArgument="Book_Title"
                                        ForeColor="Black"> Book Title</asp:LinkButton>
                                </th>
                                <th align="left" id="thUserName" class="paddingLSML" style="width: 13%;">
                                    <asp:LinkButton ID="lnkBtnUserName" runat="server" CommandName="S_SORT" CommandArgument="Name"
                                        ForeColor="Black"> User Name</asp:LinkButton>
                                </th>
                                <th align="left" id="thClass" class="paddingLSML" style="width: 5%;">
                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandName="S_SORT" CommandArgument="Class"
                                        ForeColor="Black">Class</asp:LinkButton>
                                </th>
                                <th align="left" id="thDesignation" class="paddingLSML" style="width:5%;">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="S_SORT" CommandArgument="DesignationId"
                                        ForeColor="Black">Designation</asp:LinkButton>
                                </th>
                                <th id="thDate" runat="server" style="width: 6%;">
                                    <asp:LinkButton ID="lnkBtnDate" runat="server" CommandName="S_SORT" CommandArgument="ReservationDate"
                                        ForeColor="Black"> Date</asp:LinkButton>
                                </th>
                                <th id="thForParent" runat="server" style="width: 10%;">
                                    <asp:Label ID="lblForParent" runat="server" Text="Claimed By Parent"></asp:Label>
                                </th>
                                <th style="width: 2%">
                                    <asp:Label ID="lblCancel" runat="server" Text="Cancel"></asp:Label>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                            <tr class="ClsBorderPager" id="trDataPager">
                                <td colspan="9">
                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwReservedBooks"
                                        PageSize="20">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
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
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="TrBookMaster" runat="server" class="ClsGridRow">
                            <td align="left" class="paddingLSML">
                                <asp:Label ID="lblBookName" runat="server" Text='<%# Eval(" Book_Title") %>'></asp:Label>
                            </td>
                            <td align="left" id="tdUserName" runat="server" class="paddingLSML">
                                <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                            </td>
                            <td align="left" id="tdClass" runat="server" class="paddingLSML">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ClassNameDesignation") %>'></asp:Label>
                            </td>
                            <td align="left" id="tdDesignation" runat="server" class="paddingLSML">
                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                            </td>
                            <td align="center" id="tdDate" runat="server">
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ReservationDate") %>'></asp:Label>
                            </td>
                            <td align="center" id="tdParent" runat="server">
                                <asp:Image ID="imgBtnForParent" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="imgBtnCancel" CommandName="CANCEL_RESERVATION" runat="server"
                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"></asp:ImageButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="TrBookMaster" runat="server" class="ClsGridAltRow">
                            <td align="left" class="paddingLSML">
                                <asp:Label ID="lblBookName" runat="server" Text='<%# Eval("Book_Title") %>'></asp:Label>
                            </td>
                            <td align="left" id="tdUserName" runat="server" class="paddingLSML">
                                <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                            </td>
                            <td align="left" id="tdClass" runat="server" class="paddingLSML">
                                <asp:Label ID="lblClass" runat="server" Text='<%# Eval("ClassNameDesignation") %>'></asp:Label>
                            </td>
                             <td align="left" id="tdDesignation" runat="server" class="paddingLSML">
                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                            </td>
                            <td align="center" id="tdDate" runat="server">
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ReservationDate") %>'></asp:Label>
                            </td>
                            <td align="center" id="tdParent" runat="server">
                                 <asp:Image ID="imgBtnForParent" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="imgBtnCancel" CommandName="CANCEL_RESERVATION" runat="server"
                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"></asp:ImageButton>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <table width="750px">
                            <tr>
                                <td class="LblNoRecord" align="center">
                                    No Records Found.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td align="center">
                            <asp:ObjectDataSource ID="objdsReservedBooksList" runat="server" TypeName="BusinessLogic.BookBL"
                                SelectMethod="GetReservedBookDetails" SelectCountMethod="GetReservedBookCount"
                                EnablePaging="true">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="hidUserId" Name="aiUserId" PropertyName="Value"
                                        Type="Int32" />
                                    <asp:ControlParameter ControlID="hidSortExpression" PropertyName="Value" Name="sortExpression"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="hidSortDirection" PropertyName="Value" Name="sortDirection"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="txtUserName" PropertyName="Text" Name="asUserName"
                                        DefaultValue=' ' Type="String" />
                                    <asp:ControlParameter ControlID="txtBookTitle" PropertyName="Text" Name="asBookTitle"
                                        DefaultValue=' ' Type="String" />
                                    <asp:ControlParameter ControlID="chkShowAll" PropertyName="Checked" Name="aiAllUser" Type="Int32" />
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:HiddenField ID="hidUserId" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnClose" runat="server" CssClass="ClsBtn" Text="Close" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        _clienttxtUserName = "<%=this.txtUserName.ClientID %>"
        _clienttxtBookTitle = "<%=this.txtBookTitle.ClientID %>"

        
        function ConfirmRemove() {
            var bResult = true;
            if (!window.confirm("Are you sure you want to cancel the book claim?"))
                bResult = false;
            return bResult;
        }

        function ClearControl() {
            if (document.getElementById(_clienttxtBookTitle))
                document.getElementById(_clienttxtBookTitle).value = "";
            if (document.getElementById(_clienttxtBookTitle))
                document.getElementById(_clienttxtUserName).value = "";
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
