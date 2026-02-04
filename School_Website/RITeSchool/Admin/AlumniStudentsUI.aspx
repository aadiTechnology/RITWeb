<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile ="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="AlumniStudentsUI.aspx.cs" Inherits="AlumniStudentsUI" EnableEventValidation="false" ViewStateMode = "Disabled"%>

<asp:Content ID="Content2" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
<center>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="60%">
            <tr>
                
                <td colspan = "2" align="right" class="ClsTextNormal" style="width:20%; padding-right: 10px; top: 20px; height: 19px;">
                    <span class="ClsMdtStar">* Mandatory Fields</span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="upnlErrorMsg" runat="server">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" ShowSummary = "true" HeaderText="Please fix following error(s)" ValidationGroup = "passoutyear"/>
                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExport"/>
                    </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>

            <tr>
                <td align = "center">
                    <asp:Label ID = "lblNoRecordFound" runat = "server" style = "color: Blue; font-weight:bold" Text = "No Record Found." Visible ="false"></asp:Label>
                </td>
            </tr>
           
            <tr>
                    <td align="center" colspan = "2">
                       <asp:UpdatePanel runat = "server" ID = "upnlListview" >
                            <ContentTemplate>
                                <div id = "dvListview" style="padding-top: 50px" >
                                <table>
                                     <tr id="trDtPgCount" runat="server" visible="true">
                                        <td align ="center">
                                            <asp:DataPager ID="dtPagerCount" runat="server" PagedControlID="lstvwAlumniDetails"
                                                PageSize="20">
                                                <Fields>
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB"
                                                                Text="<%# Container.StartRowIndex + 1%>" />
                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal"
                                                                Text=" To " />
                                                            <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal"
                                                                Text=" Out Of " />
                                                            <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal"
                                                                Text="Records " />
                                                            <br />
                                                        </PagerTemplate>
                                                    </asp:TemplatePagerField>
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                                    <asp:ListView ID = "lstvwAlumniDetails" runat = "server" OnSorting = "lstvwAlumniDetails_Sorting" OnDataBound = "lstvwAlumniDetails_DataBound" ViewStateMode = "Enabled">
                                        <LayoutTemplate>
                                              <table id="tblAlumniDetails" runat="server" align="center" cellpadding="0" cellspacing="1" class="GridBorder" width="80%">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" class="paddingL" width="25%">
                                                            <asp:LinkButton ID="lnkBtnStudentName" runat="server" CausesValidation="false" CommandName="Sort"
                                                                CommandArgument="StudentName" ForeColor="Black">Student Name</asp:LinkButton>
                                                        </th>
                                                        <th id="thEmailId" class="paddingL" runat="server" align="left" style="width: 14%;">
                                                            <asp:Label ID="lnkBtnEmailId" runat="server" CausesValidation="false" CommandArgument="EmailId"
                                                                ForeColor="Black">Email Id</asp:Label>
                                                        </th>
                                                        
                                                         <th id="thMobileNo" runat="server" align="cenyer" style="width: 10%;">
                                                            <asp:Label ID="lnkBtnMobileNo" runat="server" CausesValidation="false" CommandArgument="MobileNo"
                                                                ForeColor="Black">Mobile No.</asp:Label>
                                                        </th>
                                                       
                                                         <th id="thBatch" runat="server" align="center" style="width: 6%;">
                                                            <asp:LinkButton ID="lnkBtnBatch" runat="server" CausesValidation="false" CommandArgument="Batch"
                                                                CommandName="Sort" ForeColor="Black">Batch</asp:LinkButton>
                                                        </th>
                                                        <th id="thPassoutyr" runat="server" align="center" style="width: 14%;">
                                                            <asp:LinkButton ID="lnkBtnPassoutyr" runat="server" CausesValidation="false" CommandArgument="PassOutYear"
                                                                CommandName="Sort" ForeColor="Black">Passing Out Year</asp:LinkButton>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr id="trDataPager" class="ClsBorderPager">
                                                        <td colspan="15">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwAlumniDetails"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
                                                                                        <asp:DropDownList ID="ddlCnt" ViewStateMode = "Enabled" runat="server" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged" AutoPostBack = "true">
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
                                            <tr id = "trItemtemplate" runat="server" class="ClsGridRow">
                                                <td align = "left" class="paddingL">
                                                     <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                </td>
                                                <td align = "left" class="paddingL">
                                                     <asp:Label ID="lblEmailID" runat="server" Text='<%# Eval("EmailID") %>'></asp:Label>
                                                </td>
                                                <td align = "center">
                                                     <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                </td>
                                                <td align = "center">
                                                     <asp:Label ID="lblBatch" runat="server" Text='<%# Eval("Batch") %>'></asp:Label>
                                                </td>
                                                <td align = "center">
                                                     <asp:Label ID="lblPassOutYear" runat="server" Text='<%# Eval("PassOutYear") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                <td align = "left" class="paddingL">
                                                     <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                </td>
                                                <td align = "left"  class="paddingL">
                                                     <asp:Label ID="lblEmailID" runat="server" Text='<%# Eval("EmailID") %>'></asp:Label>
                                                </td>
                                                <td align = "center">
                                                     <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                </td>
                                                <td align = "center">
                                                     <asp:Label ID="lblBatch" runat="server" Text='<%# Eval("Batch") %>'></asp:Label>
                                                </td>
                                                <td align = "center">
                                                     <asp:Label ID="lblPassOutYear" runat="server" Text='<%# Eval("PassOutYear") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
											<tr>
												<td width = "80%" class="LblNoRecord" align="center">
													No record found.
												</td>
											</tr>
										</EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                                <asp:HiddenField ID="hidSortDirection" runat="server" ViewStateMode = "Enabled"/>
                                <asp:HiddenField ID="hidSortExpression" runat="server" ViewStateMode = "Enabled" />
                            </ContentTemplate>
                            <Triggers> 
                                <asp:PostBackTrigger ControlID="btnExport"/>
                                <asp:AsyncPostBackTrigger ControlID = "lstvwAlumniDetails" EventName = "Sorting"/> 
                            </Triggers>
                       </asp:UpdatePanel>
                    </td>
                </tr>   
                
                <table style="padding-top: 20px">
                    <tr>
                         <td class = "ClsBorderlight">
                            <span class="ClsLabel">Passing Out Year : </span>
                        </td>
                        <td>
                            <asp:TextBox ID = "txtPassoutYr" runat = "server" CssClass="MidTxtBox" MaxLength="5" onpaste="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"></asp:TextBox>
                        </td>
			        </tr>
                    <tr>
                        <td style="padding-top: 20px" align="center" colspan = "2">
                              <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" BorderWidth="1px" ValidationGroup = "passoutyear"
						        Visible="true" onclick="btnExport_Click"  />
                        </td>
                    </tr>
                </table>
                <tr>
                    <asp:ObjectDataSource ID = "odsAlumniDetails" runat = "server" TypeName = "BusinessLogic.AlumniStudentBL" SelectMethod = "GetAllAumniStudentDetails" EnableCaching = "false" EnablePaging = "false">
                        <SelectParameters>
                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                            <asp:ControlParameter Name="aiSortExpression" ControlID="hidSortExpression" Type="String" PropertyName="Value"/>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </tr>
        </table>
    </div>
</center>
</asp:Content>