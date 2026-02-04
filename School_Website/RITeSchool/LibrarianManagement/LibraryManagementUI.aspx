<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="LibraryManagementUI.aspx.cs" Inherits="LibraryManagementUI" EnableEventValidation="false" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <center>
        <div class="MainBodyDiv" style="width: 850px">
            <table width="98%" align="center">
                <tr>
                    <td>
                         <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
                            <ContentTemplate>
                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                            Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="ItemCommand" />
                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                            </Triggers>
                            </asp:UpdatePanel>
                            
                    </td>
                </tr>
                 
                <tr align="center">
                    <td>
                    <asp:UpdatePanel UpdateMode="Conditional" runat="server" >
                    <ContentTemplate> 
                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                    </ContentTemplate> 
                    <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="ItemCommand" />
                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                    </asp:UpdatePanel>
                    </td>

                </tr>
                <tr>
                    <td align="left" runat="server" id="trLegend">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                        Text="Legend" EnableViewState="False"></asp:Label>
                                </td>
                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                    <asp:Label ID="Label7" runat="server" BackColor="White" CssClass="ClsLblLgnd" Text="Can not be issued"
                                        ForeColor="Red" Font-Bold="False" BorderStyle="None" BorderWidth="1px" ReadOnly="True"
                                        Width="120px" EnableViewState="False"></asp:Label>
                                </td>
                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                    <asp:Label ID="Label4" runat="server" BackColor="Gainsboro" CssClass="ClsLblLgnd"
                                        Text="Deactivated User" ForeColor="Red" Font-Bold="False" BorderStyle="None" BorderWidth="1px"
                                        ReadOnly="True" Width="120px" EnableViewState="False"></asp:Label>
                                </td>
                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                    <asp:Label ID="Label8" runat="server" BackColor="White" CssClass="ClsLblLgnd" Text="Lost Book / WriteOff Book"
                                        ForeColor="Purple" Font-Bold="False" BorderStyle="None" BorderWidth="1px" ReadOnly="True"
                                        Width="150px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trSearch">
                    <td align="center" colspan="2">
                        <table cellpadding="0" cellspacing="2" align="center" width="100%">
                            <tr>
                                <td>
                                    <table width="95%">
                                        <tr>
                                            <td align="left" style="width: 10%;" valign="middle">
                                                <div id="divAddBookBtn" runat="server" class="ClsGreenBG" style="width: 100px; height: 18px;
                                                    vertical-align: bottom; padding-top: 4px; padding-right: 2px">
                                                    <asp:HyperLink ID="hlnkAdBooks" runat="server" CssClass="SubTitle " NavigateUrl="BookUI.aspx"
                                                        Text="Add Book"></asp:HyperLink>
                                                </div>
                                                
                                               
                                            </td>
                                             
                                            <td>
                                           <div id="div1" runat="server" class="ClsGreenBG" style="width: 180px; height: 18px;
                                                    vertical-align: bottom; padding-top: 4px; padding-right: 2px">
                                                 <asp:HyperLink ID="hlnkReserveBook" runat="server" CssClass="SubTitle " NavigateUrl="ReservedBookDetailsPopUpUI.aspx?"
                                                        Text="Claimed Book Details"></asp:HyperLink>
                                                </div>
                                            </td>
                                            <td align="right" style="width: 20%;" valign="middle">
                                                <div id="divImportBook" runat="server" class="ClsGreenBG" style="width: 110px; height: 18px;
                                                    vertical-align: bottom; padding-top: 4px; padding-left: 0px; padding-right: 4px">
                                                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="SubTitle " NavigateUrl="~/RITeSchool/LibrarianManagement/ImportBookUI.aspx"
                                                        Text="Import Books"></asp:HyperLink>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 554px">
                                                <span class="ClsLblLgnd" style="font-weight: bold">Search for users :</span>
                                            </td>
                                            <td align="left">
                                                <span class="ClsLblLgnd" style="font-weight: bold">Search for books : </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top; border-right-color: Black; border-right-width: thick">
                                                <table style="width: 273px; vertical-align: top; border-right-color: Black; border-right-width: thin;
                                                    border-right-style: solid; height: 164px">
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span id="Span5" class="ClsLabel">User Role :</span>
                                                        </td>
                                                        <td align="left" style="padding-right: 10px;">
                                                            <div>
                                                                <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="SmlTxtBox" TabIndex="1" Width="129px"
                                                                    OnSelectedIndexChanged="cmbUser_SelectedIndexChanged" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                           </div>
                                                        </td>
                                                    </tr>
                                                    <tr id="trClass" runat="server">
                                                        <td class="ClsBorderLight paddingL">
                                                            <span id="Span12" class="ClsLabel">Class :</span>
                                                        </td>
                                                        <td align="left" style="padding-right: 10px;">
                                                            <div>
                                                                <asp:DropDownList ID="cmbUserClass" runat="server" CssClass="SmlTxtBox" TabIndex="1"
                                                                    Width="129px" Enabled="false" AutoPostBack="True">
                                                                    <asp:ListItem Text="---All---" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr id="trRollNo" runat="server">
                                                        <td class="ClsBorderLight paddingL">
                                                            <span id="Span15" class="ClsLabel">Roll No. :</span>
                                                        </td>
                                                        <td align="left" >
                                                             <asp:TextBox ID="txtRollNo" runat="server" oonkeyup="extractNumber(this,0,false);"
                                                              CssClass="SmlTxtBox" MaxLength="100" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                               Width="129px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trEmployeeNo" runat="server">
                                                        <td class="ClsBorderLight paddingL">
                                                            <span id="Span16" class="ClsLabel">Employee No. :</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmployeeNo" runat="server" CssClass="SmlTxtBox" MaxLength="10"
                                                                TabIndex="3" Width="129px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span id="Span13" class="ClsLabel">Name :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="SmlTxtBox" MaxLength="100" autocomplete="off"
                                                                TabIndex="2" Width="129px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span id="Span14" class="ClsLabel">Barcode :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUsrBarcode" runat="server" CssClass="SmlTxtBox" MaxLength="10"
                                                                TabIndex="3" Width="129px" AutoPostBack="True" OnTextChanged="txtUsrBarcode_TextChanged"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <span id="Span1"></span>
                                                            <asp:Button ID="btnUserSearch" runat="server" CssClass="ClsBtn" Font-Bold="True"
                                                                TabIndex="4" Text="Search" OnClick="btnUserSearch_Click" />
                                                            <asp:Button ID="btnUserClear" runat="server" CssClass="ClsBtn" Font-Bold="True" TabIndex="5"
                                                                Text="Clear" OnClick="btnUserClear_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left" style="width: 600px;">
                                                <asp:UpdatePanel ID="upnlbookSearch" runat="server"><ContentTemplate>
                                                <table style="width:600px; height: 160px;">
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL" style="width:100px;" >
                                                            <span id="lblBookName" class="ClsLabel">Book Title :</span>
                                                        </td>
                                                        <td style="width:100px;" >
                                                            <asp:TextBox ID="txtBookName" runat="server" CssClass="MidTxtBox" MaxLength="100"
                                                                TabIndex="6" Width="129px"></asp:TextBox><span style="color: red"></span>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL" style="width:105px;" >
                                                            <span id="lblAccessionNumber" class="ClsLabel" >Accession Number
                                                                :</span>
                                                        </td>
                                                        <td style="width:130px;" >
                                                            <asp:TextBox ID="txtAccessionNumber" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                TabIndex="7" Width="129px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL" >
                                                            <span id="lblAuthorName" class="ClsLabel">Author :</span>
                                                        </td>
                                                        <td >
                                                            <asp:TextBox ID="txtAuthorName" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                                TabIndex="8" Width="129px"></asp:TextBox><span style="color: red"></span>
                                                        </td>
                                                         <td class="ClsBorderLight paddingL" >
                                                            <span id="Span2" class="ClsLabel">Barcode :</span>
                                                        </td>
                                                        <td >
                                                            <asp:TextBox ID="txtBookBarcode" runat="server" CssClass="SmlTxtBox" MaxLength="10"
                                                                 TabIndex="9" Width="129px" AutoPostBack="True" OnTextChanged="txtBookBarcode_TextChanged"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL" >
                                                            <span id="lblPublisher" class="ClsLabel">Publisher :</span>
                                                        </td>
                                                        <td >
                                                            <asp:TextBox ID="txtPublisher" runat="server" CssClass="SmlTxtBox" MaxLength="100"
                                                               AutoCompleteType="Search" TabIndex="10" Width="129px"></asp:TextBox><span style="color: red"></span>
                                                        </td>
                                                        
                                                           <td class="ClsBorderLight paddingL" >
                                                             <span id="lblLanguge" class="ClsLabel">Language :</span>
                                                        </td>
                                                        <td align="left" >
                                                            <asp:DropDownList ID="cmbLanguage" TabIndex="11" Width="129px" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                       
                                                        <td class="ClsBorderLight paddingL" >
                                                            <span id="Span3" class="ClsLabel">Standard :</span>
                                                        </td>
                                                        <td align="left" style="padding-right: 15px;">
                                                            <div>
                                                                <asp:DropDownList ID="cmbStandard" runat="server" CssClass="SmlTxtBox" TabIndex="12"
                                                                    Width="129px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL" rowspan="2" >
                                                            <span id="lblMediaType" class="ClsLabel">Media Type :</span>
                                                        </td>
                                                        <td rowspan="2" >
                                                            <div>
                                                                <asp:RadioButton ID="optAll" runat="server" CssClass="ClsLabel" 
                                                                    Text="All" GroupName="GrpMediaType" Checked="True" TabIndex="13" AutoPostBack="True" />
                                                                <asp:RadioButton ID="optPrintable" runat="server" CssClass="ClsLabel"
                                                                    Text="Printable" GroupName="GrpMediaType" TabIndex="13" AutoPostBack="True" /><br />
                                                               
                                                            </div>
                                                            <asp:RadioButton ID="optNonPrintable" runat="server" CssClass="ClsLabel" 
                                                                    Text="NonPrintable" GroupName="GrpMediaType" TabIndex="13" AutoPostBack="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight paddingL" >
                                                            <span class="ClsLabel">Is For Parent / Staff :</span>
                                                            </td>
                                                        <td align="left"  >
                                                            <asp:CheckBox ID="chkForParentStaff" TabIndex="14" CssClass="ClsLbl"  runat="server" />  </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <asp:Button ID="btnSearch"  runat="server" CssClass="ClsBtn" Font-Bold="True" TabIndex="15"
                                                                Text="Search" OnClick="btnSearch_Click" />
                                                            <asp:Button ID="btnClear" runat="server" CssClass="ClsBtn" Font-Bold="True" TabIndex="16"
                                                                Text="Clear" OnClick="btnClear_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trUserHeader" runat="server">
                    <td>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <br />
                                    <span class="ClsLblLgnd" style="font-weight: bold">Select user for issue :</span>
                                </td>
                            </tr>
                            
                        </table>
                    </td>
                </tr>
                <tr valign="top" id="trUserInfo" runat="server">
                    <td align="center" colspan="2">
                    <asp:UpdatePanel ID="upnlUser" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                   
                                <asp:ListView ID="lstvwUsers"
											  runat="server"
											  DataKeyNames="Book_Id,UserId,UserRoleId,UserName,IsActive"
											  OnItemCommand="lstvwUsers_ItemCommand"
											  OnItemDataBound="lstvwUsers_ItemDataBound"
											  OnSorting="lstvwUsers_Sorting"
											  OnDataBound="lstvwUsers_DataBound">
                                    <LayoutTemplate>
                                        <table cellpadding="0" cellspacing="1" align="center" width="100%" id="tblPagerUserDetails" runat="server">
											<tr>
												<td align="center">
													<asp:DataPager ID="UsersDtPgCount"
																   runat="server"
																   PagedControlID="lstvwUsers"
																   PageSize="10">
														<Fields>
															<asp:TemplatePagerField>
																<PagerTemplate>
																	<asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>" CssClass="LblNrmlB" />
																	<asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
																	<asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" CssClass="LblNrmlB" />
																	<asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
																	<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" CssClass="LblNrmlB" />
																	<asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
																	<br />
																</PagerTemplate>
															</asp:TemplatePagerField>
														</Fields>
													</asp:DataPager>
												</td>
											</tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="1" align="center" width="100%" runat="server" id="tblShiftInfo" style="color: #333333" class="GridBorder">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th align="center">
                                                    Select
                                                </th>
                                                <th id="thRegNo" align="left" class="paddingLSML">
													<asp:LinkButton ID="lnkRegNum" runat="server" CommandName="Sort" CommandArgument="RegNo"
                                                        ForeColor="Black"> Reg No.</asp:LinkButton>
                                                </th>
                                                <th id="thRollNo" align="center" class="paddingLSML">
                                                    <asp:LinkButton ID="Label5" runat="server" CommandName="Sort" CommandArgument="Roll_No"
                                                        ForeColor="Black"> Roll No.</asp:LinkButton>
                                                </th>
                                                <th id="thEmpNo" align="center" >
                                                     <asp:LinkButton ID="lnkEmpNo" runat="server" CommandName="Sort" CommandArgument="EmployeeNo"
                                                        ForeColor="Black">Employee No. </asp:LinkButton>
                                                </th>
                                                <th align="left" class="paddingLSML">
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="FirstName"
                                                        ForeColor="Black">Name</asp:LinkButton>
                                                </th>
                                                <th align="left" class="paddingLSML" id="thClassDesg" runat="server">
                                                    <asp:LinkButton ID="lnkClassDesg" runat="server" CommandName="Sort" CommandArgument="ClassNameDesignation"
                                                        ForeColor="Black">Class</asp:LinkButton>
                                                </th>
                                                <th align="center">
                                                    Books
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
											<tr class="ClsBorderPager" id="trDataPager">
												<td colspan="10">
													<asp:DataPager ID="DtPgDropDown"
																   runat="server"
																   PagedControlID="lstvwUsers"
																   PageSize="10">
														<Fields>
															<asp:TemplatePagerField>
																<PagerTemplate>
																	<table width="100%">
																		<tr>
																			<td align="left">
																				<asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
																				<asp:DropDownList ID="ddlCnt"
																								  runat="server"
																								  AutoPostBack="true"
																								  OnSelectedIndexChanged="cmbddlCnt_SelectedIndexChanged">
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
                                            <td align="center">
                                                <asp:RadioButton ID="optSelectUser" runat="server" CommandName="UserDetails" />
                                            </td>
                                            <td id="tdRegNo" align="left" class="paddingLSML" runat="server">
                                                <asp:Label ID="lblRegNum" runat="server" Text='<%# Eval(" RegNo") %>'></asp:Label>
                                            </td>
                                            <td id="tdRollNo" align="left" class="paddingLSML" runat="server">
                                                <asp:Label ID="Label6" runat="server" Text='<%# Eval(" RollNo") %>'></asp:Label>
                                            </td>
                                            <td id="tdEmpNo" align="center">
                                                <asp:Label ID="lblEmpNo" runat="server" Text='<%# Eval("EmployeeNo") %>'></asp:Label>
                                            </td>
                                            <td align="left" class="paddingLSML">
                                                <asp:Label ID="lblFullName" runat="server" Text='<%# Eval(" UserName") %>'></asp:Label>
                                            </td>
                                            <td align="left" class="paddingLSML">
                                                <asp:Label ID="lblCategory" runat="server" Text='<%# Eval(" ClassNameDesignation") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:LinkButton ID="lnkbtnDetail" runat="server" Text="Show" CommandName="DETAIL"
                                                    ToolTip="Details" />
                                            </td>
                                        </tr>
                                        <tr id="trBookDetails" runat="server" visible="false" align="center">
                                            <td id="tdBookDetails" runat="server" colspan="6" align="center">
                                                <table width="90%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ListView ID="lstvwUsersBookDetails"
																		  runat="server"
																		  DataKeyNames="Book_No,Book_Id"
																		  OnItemDataBound="lstvwUsersBookDetails_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table width="100%" runat="server" id="tblSubList" style="color: #333333" cellpadding="0"
                                                                        cellspacing="1" class="GridBorder" align="center">
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1">
                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                        <th align="left" class="ClspaddingL" width="18%">
                                                                                            Book Title
                                                                                        </th>
                                                                                        <th align="left" class="ClspaddingL" width="17%">
                                                                                            Accession No
                                                                                        </th>
                                                                                        <th align="center" width="17%">
                                                                                            Issue Date
                                                                                        </th>
                                                                                        <th align="center">
                                                                                            Return Date
                                                                                        </th>
                                                                                        <th align="center">
                                                                                            Return/Renew
                                                                                        </th>
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
                                                                        <td align="center" >
                                                                            <asp:HyperLink ID="lnkbtnDetail" runat="server" Text="Return/Renew" NavigateUrl="ReturnRenewUI.aspx" ToolTip="Details" />
                                                                        </td>
                                                                        
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
                                                                        <td align="center">
                                                                            <asp:HyperLink ID="lnkbtnDetail" runat="server" Text="Return/Renew" NavigateUrl="ReturnRenewUI.aspx" ToolTip="Details" />
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    <table width="100%">
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
                                                        <td align="center">
                                                            <asp:Button CssClass="ClsBtn" ID="BtnCancelRenewReturn" CausesValidation="false"
                                                                runat="server" Text="Cancel" BorderWidth="1px" OnClick="BtnCancelRenewReturn_Click">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td class="LblNoRecord" align="center">
                                                    No Records Found.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                 <asp:HiddenField ID="hidUserSortExpression" runat="server" Value="FirstName" />
                                 <asp:HiddenField ID="hidIsActivated" runat="server" Value="N" />
                                 <asp:HiddenField ID="hidUserSortDirection" runat="server" />
                                <asp:ObjectDataSource ID="ObjDSUserDetails" runat="server" EnablePaging="true" 
                                    SelectCountMethod="GetAllUsersCount" SelectMethod="GetAllUsers" 
                                    SortParameterName="sortExpression" TypeName="BusinessLogic.BookBL">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" 
                                            Type="Int32" />
                                        <asp:SessionParameter Name="aiAcademicYearId" 
                                            SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="Int32" />
                                        <asp:ControlParameter ControlID="txtUserName" Name="asFilter" 
                                            PropertyName="Text" Type="String" />
                                        <asp:ControlParameter ControlID="cmbUserRole" Name="aiUserRoleId" 
                                            PropertyName="SelectedValue" Type="Int32" />
                                        <asp:ControlParameter ControlID="cmbUserClass" Name="aiStandardDivisionId" 
                                            PropertyName="SelectedValue" Type="Int32" />
                                        <asp:ControlParameter ControlID="txtRollNo" Name="aiRollNo" PropertyName="Text" 
                                            Type="Int32" />
                                        <asp:Parameter Name="sortExpression" Type="String" />
                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                        <asp:ControlParameter ControlID="txtEmployeeNo" Name="asEmployeNo" 
                                            PropertyName="Text" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                     </ContentTemplate>
                    <Triggers >
                     <asp:AsyncPostBackTrigger ControlID="lstvwUsers" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="lstvwUsers" EventName="Sorting" />
                    <asp:AsyncPostBackTrigger ControlID="lstvwUsers" EventName="ItemDataBound" />
                    <asp:AsyncPostBackTrigger ControlID="lstvwUsers" EventName="DataBound" />
                     <asp:AsyncPostBackTrigger ControlID="btnIssueBooks" EventName="Click" />
                    </Triggers>
                    </asp:UpdatePanel>
                    </td>
                </tr>
                <tr id="trlbl" runat="server">
                    <td>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <br />
                                    <span class="ClsLblLgnd" style="font-weight: bold">Select books to be issued :</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <div id="divMain" runat="server" class="overlay" style="visibility: hidden; display: none;">
                        </div>
                        <div id="updtpnlPopUp" runat="server" style="visibility: hidden; display: none; position: absolute;
                            margin: 0px; padding: 0px; width: 350px; height: 150px; border-width: 0px; left: 0px;
                            top: 0px; line-height: normal; width: auto; border: solid 1px black; margin: 275px 0px 0px 60px;
                            background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
                            <div class="ReturnRenewPopop">
                                <div style="padding: 1px; font-size: 12px; font-weight: bold; color:Black; float: left;">
                                    Book Issue Date</div>
                                <span style="cursor: hand" onclick="javascript:HidePopup();">
                                    <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                </span>
                            </div>
                            <div style="padding: 10px; text-align: left;" class="ClsLabel">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel4" ChildrenAsTriggers="True" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="240px">
                                            <tr style=" height:50px">
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblSchoolleaving" runat="server" Text="Issue date :" CssClass="LblNormal" />
                                                    <asp:TextBox ID="txtReturnDate" Width="100px" CssClass="SmlCombo" runat="server" ReadOnly = "true" 
                                                        MaxLength="11"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calendarIssueDate" runat="server" Control="txtReturnDate" Format="dd MMM yyyy"
                                                                To-Message="Please select valid Issue Date." From-Message="Please select valid Issue Date."
                                                                To-Today="true" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="Please select valid Issue Date." />
                                                    <span style="color: #ff0000">*</span>
                                                    <asp:CustomValidator ID="custReturnDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        ErrorMessage="Return date should not be blank." Visible="true" EnableClientScript="true"
                                                        ></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr style=" height:25px">
                                                <td align="center" colspan="2">
                                                    <asp:Button ID="btnOk" runat="server" Text="OK" CssClass="ClsBtn" onclick="btnOk_Click"  
                                                         />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                        OnClientClick="javascript:HidePopup();return false;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr id="trPagerBookDetails" runat="server">
                    <td align="center">
                       
                    </td>
                </tr>
                <tr valign="top" id="trBookListView" runat="server">
                    <td align="center" colspan="2">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                             <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwBookMaster">
                            <Fields>
                                <asp:TemplatePagerField>
                                    <PagerTemplate>
                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>" CssClass="LblNrmlB" />
                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" CssClass="LblNrmlB" />
                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" CssClass="LblNrmlB" />
                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                        <br />
                                    </PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                            <asp:ListView ID="lstvwBookMaster"
										  runat="server"
										  DataKeyNames="Book_Id,Available_Books,Book_Title,IsForIssue"
										  OnItemDataBound="lstvwBookMaster_ItemDataBound"
										  OnItemCommand="lstvwBookMaster_ItemCommand"
										  OnSorting="lstvwBookMaster_Sorting"
										  OnDataBound="lstvwBookMaster_DataBound">
                                <LayoutTemplate>
                                    <table align="center" width="900px" runat="server" id="tblShiftInfo" style="color: #333333"
                                        cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                             <th align="left" class="paddingLSML" style="width: 190px;">
                                                <asp:LinkButton ID="LinkButton5" runat="server" CommandName="S_Sort" CommandArgument="AccesNo" 
                                                    ForeColor="Black"> Accession No.</asp:LinkButton>
                                            </th>
                                            <th align="left" class="paddingLSML" style="width:400px;">
                                                <asp:LinkButton ID="lnkSortCode" runat="server" CommandName="S_Sort" CommandArgument="Book_Title"
                                                    ForeColor="Black"> Book Title</asp:LinkButton>
                                            </th>
                                            <th align="left" class="paddingLSML" style="width: 200px;">
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="S_Sort" CommandArgument="Author_Name"
                                                    ForeColor="Black"> Author</asp:LinkButton>
                                            </th>
                                            <th align="left" class="paddingLSML" style="width: 160px;">
                                                <asp:LinkButton ID="lnkBtnLanguage" runat="server" CommandName="S_Sort" CommandArgument="Language"
                                                    ForeColor="Black"> Language</asp:LinkButton>
                                            </th>
                                            <th align="center" style="width: 60px;">
                                             <asp:Label ID="lblAvailable" runat="server" Text="Available"></asp:Label>
                                            </th>
                                            <th align="center" style="width: 50px;">
                                                <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                            </th>
                                            <th align="center">
                                                Select
                                            </th>
                                            <th>
                                                Claim
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="Label3" runat="server" ForeColor="Black"> Edit</asp:Label>
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="Label1" runat="server" ForeColor="Black"> Delete</asp:Label>
                                            </th>
                                            <th align="center">
                                                <asp:Label ID="Label2" runat="server" ForeColor="Black">Add</asp:Label>
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                        <tr class="ClsBorderPager" id="trDataPager">
                                            <td colspan="11">
                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwBookMaster"
                                                    PageSize="20">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                            <asp:DropDownList ID="ddlCnt"
																							  runat="server"
																							  AutoPostBack="true"
																							  onchange="if(!WarnOnPageChange(this)){return false;};"
																							  OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
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
                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("Book_No") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingLSML">
                                            <asp:Label ID="lblBookName" runat="server" Text='<%# Eval(" Book_Title") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingLSML">
                                            <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval(" Author_Name") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingLSML">
                                            <asp:Label ID="lbllanguage" runat="server" Text='<%# Eval(" Language") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblAvailable" runat="server" Text='<%# Eval(" Available_Books") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval(" Total_Book_Quantity") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnSelectBook" runat="server" CommandName="DETAIL" ToolTip="Details"
                                                ImageUrl="~/RITeSchool/images/Selection5.gif" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkReservBooks" CommandName="Reserve_Book" runat="server">Claim</asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/RITeSchool/images/Icon_BookDelete.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Icon_BookAdd.gif" />
                                        </td>
                                    </tr>
                                    <tr id="trBookDetails" runat="server" visible="false" align="center">
                                        <td id="tdBookDetails" runat="server" colspan="6" align="center">
                                            <table width="65%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:ListView ID="lstvwBookDetails" runat="server" DataKeyNames="Book_Detail_Id,Book_No,IsBookLost,IsWriteOffBook"
                                                            OnItemDataBound="lstvwBookDetails_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table width="100%" runat="server" id="tblRange" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1" class="GridBorder" align="center">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="center" width="13%">
                                                                                        Select
                                                                                    </th>
                                                                                    <th align="left" class="ClspaddingL" width="18%">
                                                                                        Accession No
                                                                                    </th>
                                                                                    <th align="left" class="ClspaddingL" width="20%">
                                                                                        Rack/Shelf
                                                                                    </th>
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
                                                                    <td align="center">
                                                                        <asp:RadioButton ID="optSelectToIssue" runat="server" GroupName="A"/>
                                                                        <asp:HiddenField ID="hidBookNo" runat="server" />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Book_No") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("RackShelfNo") %>' />
                                                                        <asp:HiddenField ID="hidBookDetailsId" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="Tr3" runat="server" class="ClsGridAltRow" align="center">
                                                                    <td align="center">
                                                                        <asp:RadioButton ID="optSelectToIssue" runat="server" GroupName="A" />
                                                                        <asp:HiddenField ID="hidBookNo" runat="server" />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Book_No") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("RackShelfNo") %>' />
                                                                        <asp:HiddenField ID="hidBookDetailsId" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                        </asp:ListView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button CssClass="ClsBtn" ID="BtnCancelBook" CausesValidation="false" runat="server"
                                                            Text="Cancel" BorderWidth="1px" OnClick="BtnCancelBook_Click"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="TrBookMaster" runat="server" class="ClsGridAltRow">
                                    <td align="left" class="paddingLSML">
                                            <asp:Label ID="lblAccNo" runat="server" Text='<%# Eval("Book_No") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingLSML">
                                            <asp:Label ID="lblBookName" runat="server" Text='<%# Eval(" Book_Title") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingLSML">
                                            <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval(" Author_Name") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingLSML">
                                            <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("Language") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblAvailable" runat="server" Text='<%# Eval(" Available_Books") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval(" Total_Book_Quantity") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnSelectBook" runat="server" CommandName="DETAIL" ToolTip="Details"
                                                ImageUrl="~/RITeSchool/images/Selection5.gif" />
                                        </td>
                                          <td>
                                            <asp:LinkButton ID="lnkReservBooks" CommandName="Reserve_Book" runat="server">Claim</asp:LinkButton>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/RITeSchool/images/Icon_BookDelete.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Icon_BookAdd.gif" />
                                        </td>
                                    </tr>
                                    <tr id="trBookDetails" runat="server" visible="false" align="center">
                                        <td id="tdBookDetails" runat="server" colspan="6" align="center">
                                            <table width="65%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:ListView ID="lstvwBookDetails" runat="server" DataKeyNames="Book_Detail_Id,Book_No,IsBookLost,IsWriteOffBook"
                                                            OnItemDataBound="lstvwBookDetails_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table width="100%" runat="server" id="tblRange" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1" class="GridBorder" align="center">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="center" width="13%">
                                                                                        Select
                                                                                    </th>
                                                                                    <th align="left" class="ClspaddingL" width="18%">
                                                                                        Accession No
                                                                                    </th>
                                                                                    <th align="left" class="ClspaddingL" width="20%">
                                                                                        Rack/Shelf
                                                                                    </th>
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
                                                                    <td align="center">
                                                                        <asp:RadioButton ID="optSelectToIssue" runat="server" />
                                                                        <asp:HiddenField ID="hidBookNo" runat="server" />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Book_No") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("RackShelfNo") %>' />
                                                                        <asp:HiddenField ID="hidBookDetailsId" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="Tr3" runat="server" class="ClsGridAltRow" align="center">
                                                                    <td align="center">
                                                                        <asp:RadioButton ID="optSelectToIssue" runat="server" />
                                                                        <asp:HiddenField ID="hidBookNo" runat="server" />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Book_No") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("RackShelfNo") %>' />
                                                                        <asp:HiddenField ID="hidBookDetailsId" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <table width="100%">
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
                                                    <td align="center">
                                                        <asp:Button CssClass="ClsBtn" ID="BtnCancelBook" CausesValidation="false" runat="server"
                                                            Text="Cancel" BorderWidth="1px" OnClick="BtnCancelBook_Click"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                No Records Found.
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <asp:HiddenField ID="hidBookDetailsCount" runat="server" />
                            <asp:HiddenField ID="hidReserveBookCount" runat="server" />
                            <asp:HiddenField ID="hidBookId" runat="server" Value = "0" />            
                            <asp:HiddenField ID="hidBookSortExpression" runat="server" Value="Book_Title" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                             <asp:HiddenField ID="hidBooksPageNo" runat="server" Value="0" />

                            <asp:HiddenField ID="hidSortExpression" runat="server" />

                            <asp:ObjectDataSource ID="ObjDSBookDetails" runat="server" 
                                EnableCaching="False" EnablePaging="True" SelectCountMethod="GetCount" 
                                SelectMethod="GetPagedBookList" 
                                TypeName="BusinessLogic.BookBL">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" 
                                        Type="int32" />
                                    <asp:ControlParameter ControlID="txtBookName" Name="asBookName" 
                                        PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="txtAccessionNumber" Name="asAccessionNumber" 
                                        PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="txtAuthorName" Name="asAuthorName" 
                                        PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="txtPublisher" Name="asPublisher" 
                                        PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="cmbLanguage" Name="asLanguage" 
                                        PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="cmbStandard" Name="aiStandardId" 
                                        PropertyName="SelectedValue" Type="Int32" />
                                    <asp:ControlParameter ControlID="hidMediaType" Name="aiMediaType" 
                                        Type="Int32" />
                                    <asp:ControlParameter ControlID="hidBookId" Name="aiBookId" Type="Int32" />
                                    <asp:ControlParameter ControlID="chkForParentStaff" DefaultValue="0" 
                                        Name="aiParentStaffId" PropertyName="Checked" Type="Int32" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                    <asp:ControlParameter ControlID="hidSortExpression" PropertyName="Value" Type="String" Name="sortExpression" />
                                    <asp:ControlParameter ControlID="hidSortDirection" PropertyName="Value"  Type="String" Name="sortDirection" />
                                  </SelectParameters>
                            </asp:ObjectDataSource>

                     </ContentTemplate>
                    <Triggers >
                    <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="Sorting" />
                    <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="ItemDataBound" />
                    <asp:AsyncPostBackTrigger ControlID="lstvwBookMaster" EventName="DataBound" />
                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                    </asp:UpdatePanel>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <div runat="server" id="divErr">
                        </div>
                    </td>
                </tr>
                 <tr id="trButtons" runat="server">
                    <td align="center">
                        <table align="center" cellpadding="2" width="100%">
                            <tr id="tr4" runat="server">
                                <td align="center">
                                    <asp:Button ID="btnIssueBooks" runat="server" CssClass="ClsBtn" Font-Bold="True"
                                        Text="Issue" />
                                    <asp:Button ID="btnCancelBooks" runat="server" CssClass="ClsBtn" Font-Bold="True"
                                        Text="Cancel" OnClick="btnCancelBooks_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hidIsEditAccess" runat="server" />
                        <asp:HiddenField ID="hidBookCount" runat="server" />
                        <asp:HiddenField ID="hidUsersBookDetails" runat="server" />
                        <asp:HiddenField ID="hidUserRowCount" runat="server" />
                        <asp:HiddenField ID="hidReturnDays" runat="server" />
                        <asp:HiddenField ID="hidUserRoleID" runat="server" Value="3" />
                        <asp:HiddenField ID="hidUserRole" runat="server" />
                        <asp:HiddenField ID="hidUserID" runat="server" />
                        <asp:HiddenField ID="hidUserName" runat="server" />
                        <asp:HiddenField ID="hidForParent" runat="server" />
                        <asp:HiddenField ID="hidUsersPageNo" runat="server" />
                        <asp:HiddenField ID="hidRowIndex" runat="server" />
                        <asp:HiddenField ID="hidActIssueDate" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hidMediaType" runat="server" />
       
        </div>
    </center>
    <script lang = "javascript" type="text/javascript">
        _clientListId = "<%=this.lstvwBookMaster.ClientID%>";
        _clienthidBookCount = "<%=this.hidBookCount.ClientID %>";
        _clienttxtReturnDate = "<%=this.txtReturnDate.ClientID %>"
        _clienthidActIssueDate = "<%=this.hidActIssueDate.ClientID %>";
        _clienthidBookPageNo = "<%= this.hidBooksPageNo.ClientID %>";
        _clienthidBookDetailsCount = "<%=this.hidBookDetailsCount.ClientID %>";
        _clienthidUserRowCnt = "<%=this.hidUserRowCount.ClientID %>";
        _clienthidUsersPageNo = "<%= this.hidUsersPageNo.ClientID %>";
        _clienthidUsersBookDetails = "<%=this.hidUsersBookDetails.ClientID %>";
        _clientcmbUser = "<%=this.cmbUserRole.ClientID %>"
        _clientlstvwUsers = "<%=this.lstvwUsers.ClientID%>";
        _clienthidRowIndex = "<%=this.hidRowIndex.ClientID %>"
        _clienthidUserRoleID = "<%=this.hidUserRoleID.ClientID %>"
        _clientchkForParentStaff = "<%=this.chkForParentStaff.ClientID %>"
        _clientcmbStandard = "<%=this.cmbStandard.ClientID %>"
        _clientupdtpnlPopUp = "<%=this.updtpnlPopUp.ClientID %>"
        _clientdivMain = "<%=this.divMain.ClientID %>"
        _clienttxtUserName = "<%=this.txtUserName.ClientID %>"
        _clientbtnUserSearch = "<%=this.btnUserSearch.ClientID %>"
    </script>
    <script src="../Scripts/LibrarianManagement/LibraryManagementUI.js" type="text/javascript"></script>
    <script lang ="javascript" type="text/javascript">
        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtUserName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            _clientddlUserRole = '<%=cmbUserRole.ClientID%>';
            var _clientddlStdDiv = '<%=cmbUserClass.ClientID%>';

            if ($get(_clientddlUserRole).value == "9")
                $get(_clientddlUserRole).value = "3";
            
            BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _slienttxtUserName, _clientddlUserRole, 1, null, null, _clientddlStdDiv);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }
	</script>
</asp:Content>
