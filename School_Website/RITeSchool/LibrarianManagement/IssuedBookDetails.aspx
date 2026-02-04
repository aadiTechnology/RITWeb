<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="IssuedBookDetails.aspx.cs" Inherits="IssuedBookDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="90%">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="2" align="center" width="100%">
                    <tr align="center">
                        <td align="center">
                            <table align="center" width="100%">
                                <tr align="center">
                                    <td align="center" style="width: 65%">
                                        <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table align="center" style="width: 1000px;">
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <asp:Label ID="Label4" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                Text=" " EnableViewState="False"></asp:Label>
                                                            <asp:Label ID="Label5" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                Text=" " EnableViewState="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="" colspan="4">
                                                            <asp:Label ID="lblError" EnableViewState="false" Visible="false" CssClass="LblErrorMsg"  runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" class="" colspan="4">
                                                            <asp:Label ID="lblUpdateSucess" runat="server" CssClass="ClsLabel" 
                                                                EnableViewState="False" Font-Bold="True" ForeColor="Blue" Height="20px" 
                                                                Visible="False" Width="100%"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <span class="ClsLblLgnd" style="font-weight: bold">Search Criteria :</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="4">
                                                           <div id="div2" runat="server" class="ClsGreenBG" style="width: 150px; height: 18px;
                                                                    vertical-align: bottom; padding-top: 4px; padding-right: 2px">
                                                                 <asp:HyperLink ID="hlnkReserveBook" runat="server" CssClass="SubTitle " NavigateUrl="ReservedBookDetailsPopUpUI.aspx?"
                                                                        Text="Claimed Book Details"></asp:HyperLink>
                                                           </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL" style="width: 200px">
                                                            <span id="lblBookName" class="ClsLabel">Book Title :</span>
                                                        </td>
                                                        <td style="width: 300px">
                                                            <asp:TextBox ID="txtBookName" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                TabIndex="1" Width="250px"></asp:TextBox><span style="color: red"></span>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL" style="width: 200px">
                                                            <span id="lblAccessionNumber" class="ClsLabel" style="width: 200px">Accession Number
                                                                :</span>
                                                        </td>
                                                        <td style="width: 300px">
                                                            <asp:TextBox ID="txtAccessionNumber" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                TabIndex="2" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL" style="width: 200px">
                                                            <span id="lblAuthorName" class="ClsLabel">Author :</span>
                                                        </td>
                                                        <td style="width: 300px">
                                                            <asp:TextBox ID="txtAuthorName" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                TabIndex="3" Width="250px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 200px" class="ClsBorderlight paddingL">
                                                            <span id="lblMediaType" class="ClsLabel">Media Type :</span>
                                                        </td>
                                                        <td style="width: 300px">
                                                            <div>
                                                                <asp:RadioButton ID="optAll" runat="server" CssClass="ClsLabel" OnCheckedChanged="optAll_CheckedChanged"
                                                                    Text="All" GroupName="GrpMediaType" Checked="True" TabIndex="4" AutoPostBack="True" />
                                                                <asp:RadioButton ID="optPrintable" runat="server" CssClass="ClsLabel" OnCheckedChanged="optPrintable_CheckedChanged"
                                                                    Text="Printable" GroupName="GrpMediaType" TabIndex="4" AutoPostBack="True" />
                                                                <asp:RadioButton ID="optNonPrintable" runat="server" CssClass="ClsLabel" OnCheckedChanged="optNonPrintable_CheckedChanged"
                                                                    Text="NonPrintable" GroupName="GrpMediaType" TabIndex="4" AutoPostBack="True" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL" style="width: 200px">
                                                            <span id="lblPublisher" class="ClsLabel">Publisher :</span>
                                                        </td>
                                                        <td style="width: 300px">
                                                            <asp:TextBox ID="txtPublisher" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                TabIndex="5" Width="250px"></asp:TextBox><span style="color: red"></span>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL" style="width: 200px">
                                                           <%-- <span id="Span9" class="ClsLabel">Category :</span>--%>
                                                            <span id="Span9" class="ClsLabel">Language :</span>
                                                        </td>
                                                        <td align="left" style="padding-right: 15px; width: 300px">
                                                            <asp:UpdatePanel ID="upnlCatagory" runat="server">
                                                                <ContentTemplate>
                                                                    <div>
                                                                       <%-- <asp:DropDownList ID="cmbMainCategory" runat="server" CssClass="SmlTxtBox" TabIndex="6"
                                                                            Width="129px">
                                                                        </asp:DropDownList>--%>
                                                                          <asp:DropDownList ID="cmbLanguage" runat="server" CssClass="SmlTxtBox" TabIndex="6"
                                                                            Width="129px">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hidMediaType" runat="server" />
                                                                        <asp:HiddenField ID="hidBookId" runat="server" Value="0" />
                                                                    </div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="optAll" EventName="CheckedChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="optPrintable" EventName="CheckedChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="optNonPrintable" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <%--<td class="ClsBorderLight" style="width: 200px">
                                                            <span id="lblDescription" class="ClsLabel">Description :</span>
                                                        </td>
                                                        <td style="width: 300px">
                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                TabIndex="7" Width="250px"></asp:TextBox><span style="color: red"></span>
                                                        </td>--%>
                                                        <td class="ClsBorderLight paddingL" style="width: 200px">
                                                            <span id="Span3" class="ClsLabel">Standard :</span>
                                                        </td>
                                                        <td align="left" style="padding-right: 15px; width: 300px">
                                                            <div>
                                                                <asp:DropDownList ID="cmbStandard" runat="server" CssClass="SmlTxtBox" TabIndex="8"
                                                                    Width="129px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td colspan="4" valign="top" class="ClsBorderlight">
                                                            <asp:CheckBox ID="chkShowIssuedBooks" Text="Show Books With Me" runat="server" AutoPostBack="false" />
                                                        </td>
                                                        
                                                    </tr>--%>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Font-Bold="True" TabIndex="9"
                                                                Text="Search" OnClick="btnSearch_Click" Width="75px" />
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:Button ID="btnClear" runat="server" CssClass="ClsBtn" Font-Bold="True" TabIndex="10"
                                                                Text="Clear" OnClick="btnClear_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="DataBound" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnlBookDetails" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr id="trBookDetails" runat="server">
                                <td>
                                    <table align="center" width="1000px">
                                        <tr>
                                            <td align="left">
                                                <span class="ClsLblLgnd" style="font-weight: bold">Books Details :</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trPagerBookDetails" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwBookMaster">
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
                                    <asp:ListView ID="lstvwBookMaster" runat="server" 
                                        DataKeyNames="Book_Id,Available_Books,Book_Title,IsForIssue" OnItemCommand="lstvwBookMaster_ItemCommand"
                                        DataSourceID="ObjDSBookDetails" OnDataBound="lstvwBookMaster_DataBound" 
                                        OnItemDataBound="lstvwBookMaster_ItemDataBound" OnSorting="lstvwBookMaster_Sorting">
                                        <LayoutTemplate>
                                            <table align="center" width="1000px" runat="server" id="tblShiftInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th align="left" class="paddingLSML" style="width: 180px;">
                                                        <asp:LinkButton ID="lnkAccessionNo" runat="server" CommandName="S_Sort" CommandArgument="Book_No"
                                                            ForeColor="Black">Accession No.</asp:LinkButton>
                                                    </th>
                                                    <th align="left" class="paddingLSML" style="width: 380px;">
                                                        <asp:LinkButton ID="lnkSortCode" runat="server" CommandName="S_Sort" CommandArgument="Book_Title"
                                                            ForeColor="Black"> Book Title</asp:LinkButton>
                                                    </th>
                                                    <th align="left" class="paddingLSML" style="width: 200px;">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="S_Sort" CommandArgument="Author_Name"
                                                            ForeColor="Black"> Author</asp:LinkButton>
                                                    </th>
                                                    <th align="left" class="paddingLSML" style="width: 200px;">
                                                        <asp:LinkButton ID="LinkButton5" runat="server" CommandName="S_Sort" CommandArgument="Published_By"
                                                            ForeColor="Black"> Publisher</asp:LinkButton>
                                                    </th>
                                                    <th align="left" class="paddingLSML" style="width: 150px;">
                                                        <asp:Label ID="LinkButton7" runat="server" CommandName="S_Sort" CommandArgument=""
                                                            ForeColor="Black"> Standards</asp:Label>
                                                    </th>
                                                    <th align="left" class="paddingLSML" style="width: 130px;">
                                                      <asp:LinkButton ID="lnkBtnLanguage" runat="server" CommandName="S_Sort" CommandArgument="Language"
                                                            ForeColor="Black"> Language</asp:LinkButton>
                                                    </th>
                                                    <th align="center" style="width: 130px;">
                                                     <asp:Label ID="lblAvailable" runat="server" Text="Available"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 100px;">
                                                        <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                                    </th>
                                                    <th>
                                                        <asp:Label ID="lblReserve" runat="server" Text="Claim"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager">
                                                    <td colspan="9">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwBookMaster"
                                                            PageSize="20">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left">
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
                                            <tr id="TrBookMaster" runat="server" class="ClsGridRow">
                                              <td align="left" class="paddingLSML">
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("Book_No ") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="lblBookName" runat="server" Text='<%# Eval(" Book_Title") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval(" Author_Name") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval(" Published_By") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval(" Standards") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Eval(" Language") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblAvailable" runat="server" Text='<%# Eval(" Available_Books") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval(" Total_Book_Quantity") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                         <asp:LinkButton ID="lnkbtnReserve" CommandName="Resrve_Book" runat="server">Claim</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="TrBookMaster" runat="server" class="ClsGridAltRow">
                                              <td align="left" class="paddingLSML">
                                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("Book_No") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="lblBookName" runat="server" Text='<%# Eval(" Book_Title") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval(" Author_Name") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval(" Published_By") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval(" Standards") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingLSML">
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Eval(" Language") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblAvailable" runat="server" Text='<%# Eval(" Available_Books") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval(" Total_Book_Quantity") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:LinkButton ID="lnkbtnReserve"  CommandName="Resrve_Book" runat="server">Claim</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="1000px">
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
                        </table>
                        <asp:ObjectDataSource TypeName="BusinessLogic.BookBL" EnablePaging="True" ID="ObjDSBookDetails"
                            runat="server" SelectMethod="GetPagedBookList"
                            SelectCountMethod="GetCount" EnableCaching="False">
                            <SelectParameters>
                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                <asp:ControlParameter Name="asBookName" Type="String" ControlID="txtBookName" PropertyName="Text" />
                                <asp:ControlParameter Name="asAccessionNumber" Type="String" ControlID="txtAccessionNumber"
                                    PropertyName="Text" />
                                <asp:ControlParameter Name="asAuthorName" Type="String" ControlID="txtAuthorName"
                                    PropertyName="Text" />
                                <asp:ControlParameter Name="asPublisher" Type="String" ControlID="txtPublisher" PropertyName="Text" />
                                <%--<asp:ControlParameter Name="asDescription" Type="String" ControlID="txtDescription"
                                    PropertyName="Text" />--%>
                               <%-- <asp:ControlParameter Name="aiMainCategoryId" Type="Int32" ControlID="cmbMainCategory"
                                    PropertyName="SelectedValue" />--%>
                                    <asp:ControlParameter Name="asLanguage" Type="String" ControlID="cmbLanguage"
                                    PropertyName="SelectedValue" />
                                <asp:ControlParameter Name="aiStandardId" Type="Int32" ControlID="cmbStandard" PropertyName="SelectedValue" />
                                <asp:ControlParameter Name="aiMediaType" ControlID="hidMediaType" Type="Int32" />
                                <asp:ControlParameter Name="aiBookId" ControlID="hidBookId" Type="Int32" />
                                <asp:ControlParameter Name="sortExpression" ControlID="hidSortExpression" Type="String" PropertyName="Value" />
                                <asp:ControlParameter Name="sortDirection" ControlID="hidSortDirection" Type="String" PropertyName="Value" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                               <%-- <asp:Parameter Name="sortExpression" Type="String" />--%>
                                <asp:Parameter Name="aiParentStaffId" Type="Int32" DefaultValue="0" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidReserveBookCount" runat="server" />
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidPageNo" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="optAll" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="optPrintable" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="optNonPrintable" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="ItemCommand" />
                         <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="Sorting" />
                         <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="ItemDataBound" />
                         <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="DataBound" />
                         
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnlUserList" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr id="trIssuedbookHeader" runat="server">
                                <td>
                                    <table align="center" width="1000px">
                                        <tr>
                                            <td align="left">
                                                <br />
                                                <span class="ClsLblLgnd" style="font-weight: bold">Books With Me :</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwUsersBookDetails" DataKeyNames="Book_No,Book_Id,IsForParent" 
                                        runat="server" onitemdatabound="lstvwUsersBookDetails_ItemDataBound" 
                                        ondatabound="lstvwUsersBookDetails_DataBound">
                                        <LayoutTemplate>
                                            <table width="1000px" runat="server" id="tblSubList" style="color: #333333" cellpadding="0"
                                                cellspacing="1" class="GridBorder" align="center">
                                                <tr>
                                                    <td>
                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                            cellspacing="1">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="ClspaddingL" width="30%">
                                                                    Book Title
                                                                </th>
                                                                <th align="left" class="ClspaddingL" width="17%">
                                                                    Accession No
                                                                </th>
                                                                <th align="center" width="17%">
                                                                    Issue Date
                                                                </th>
                                                                <th align="center" width="17%">
                                                                    Return Date
                                                                </th>
                                                                <th runat="server" id="thForParent"> 
                                                                    Issued To Parent?
                                                                </th>
                                                                <%--<th align="center">
                                                Return/Renew
                                            </th>--%>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridRow" align="center">
                                                <td align="left" class="ClspaddingL">
                                                    <asp:Label ID="lblBookTitle" runat="server" Text='<%# Eval("Book_Title") %>' />
                                                </td>
                                                <td align="left" class="ClspaddingL">
                                                    <asp:Label ID="lblBookNo" runat="server" Text='<%# Eval("Book_No") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# Eval("Issue_Date","{0:dd-MMM-yyyy}") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblReturnDate" runat="server" Text='<%# Eval("Return_Date","{0:dd-MMM-yyyy}") %>' />
                                                </td>
                                                <td id="tdForParent" runat="server">
                                                    <asp:Image ID="imgBtnForParent" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                </td>
                                                <%--<td align="center">
                                <asp:HyperLink ID="lnkbtnDetail" runat="server" Text="Return/Renew" NavigateUrl="ReturnRenewUI.aspx"
                                    ToolTip="Details" />
                            </td>--%>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridAltRow" align="center">
                                                <td align="left" class="ClspaddingL">
                                                    <asp:Label ID="lblBookTitle" runat="server" Text='<%# Eval("Book_Title") %>' />
                                                </td>
                                                <td align="left" class="ClspaddingL">
                                                    <asp:Label ID="lblBookNo" runat="server" Text='<%# Eval("Book_No") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblIssueDate" runat="server" Text='<%# Eval("Issue_Date","{0:dd-MMM-yyyy}") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblReturnDate" runat="server" Text='<%# Eval("Return_Date","{0:dd-MMM-yyyy}") %>' />
                                                </td>
                                                <td id="tdForParent" runat="server">
                                                    <asp:Image ID="imgBtnForParent" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                </td>
                                                <%--<td align="center">
                                <asp:HyperLink ID="lnkbtnDetail" runat="server" Text="Return/Renew" NavigateUrl="ReturnRenewUI.aspx"
                                    ToolTip="Details" />
                            </td>--%>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="1000px" align="center">
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
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="optAll" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="optPrintable" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="optNonPrintable" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:HiddenField ID="hidForParent" runat="server" />
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clienthidForParent="<%=this.hidForParent.ClientID %>"

        function ConfirmReservation() {
            var bResult = true;
            if (window.confirm("Do you want to claim this book for parent? \nIf yes click on OK otherwise it will be claimed for student.")) {
                document.getElementById(_clienthidForParent).value = 1;

            }
            else {
                document.getElementById(_clienthidForParent).value = 0;
                bResult = false;
            }
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
