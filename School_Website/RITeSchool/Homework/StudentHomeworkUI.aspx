<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
	AutoEventWireup="true" CodeFile="StudentHomeworkUI.aspx.cs" Inherits="StudentHomeworkUI" ViewStateMode="Disabled" %>
<%--<%@ Register TagPrefix="uc" Src="~/UserControls/HomeworkListUC.ascx" TagName="Homework" %>--%>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
	TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server" ViewStateMode="Enabled">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
	<div align="right" width="90%" class="LblNormal ClsMdtStar">
		* Mandatory Fields
	</div>
	<table width="1000px">
         <tr>
            <td>
                <span class="clsLabel" style="font-weight:bold;">Homework :</span>
            </td>
        </tr>
		<tr>
			<td align="center">
				<table>
					<tr>
                         <td class="ClsBorderlight paddingL">
							Select Homework Status:
						 </td>
						 <td>
						 <asp:DropDownList runat="server" ViewStateMode="Enabled"  ID="drdwnHomeWorkStatus"  AutoPostBack="True" 
                                 onselectedindexchanged="drdwnHomeWorkStatus_SelectedIndexChanged">
                            <asp:ListItem Text="Assigned Date" Selected="True" Value="AssignedDate"></asp:ListItem>
                            <asp:ListItem Text="Complete By Date"  Value="CompleteByDate"></asp:ListItem>
                         </asp:DropDownList>
					     </td>
						 <td>
							<asp:TextBox ID="txtSearchDt" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox"></asp:TextBox>
							<rjs:PopCalendar ID="calAssignedDtSearch" runat="server" ViewStateMode="Enabled" Control="txtSearchDt" Format="d mmm yyyy" Culture="en"
								ShowErrorMessage="false" OnSelectionChanged="calAssignedDtSearch_SelectionChanged" 
								AutoPostBack="True" />
							<span class="ClsMdtStar">*</span>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:ListView ID="lstvwHomeworkStudent" runat="server" DataKeyNames="IsPublished"
					onitemdatabound="lstvwHomeworkStudent_ItemDataBound">
					<LayoutTemplate>
						<table id="tblhomework" align="center" width="1000px" runat="server" class="GridBorder">
							<tr id="trHeader" runat="server" class="ClsGridHeader">
								<th align="left" class="paddingL">
									<asp:Label ID="lblSubject" runat="server" ViewStateMode="Enabled" Text="Subject"></asp:Label>
								</th>
								<th align="left" class="paddingL">
									<asp:Label ID="Label2" runat="server" ViewStateMode="Enabled" Text="Title"></asp:Label>
								</th>
								<th align="center" class="paddingL">
									<asp:Label ID="Label4" runat="server" ViewStateMode="Enabled" Text="Complete By Date"></asp:Label>
								</th>
							</tr>
							<tr runat="server" id="itemPlaceholder">
							</tr>
						</table>
					</LayoutTemplate>
					<ItemTemplate>
						<tr id="trItem" runat="server" class="ClsGridRow">
							<td align="left" class="paddingL">
								<asp:Label ID="lblSubject" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Subject.SubjectName") %>'></asp:Label>
							</td>
							<td align="left" class="paddingL">
								<asp:LinkButton ID="lnkTitle" PostBackUrl="ViewHomeworkUI.aspx?"
									runat="server" ViewStateMode="Enabled" Text='<%# Eval("Title") %>'></asp:LinkButton>
							</td>
							<td align="center" class="paddingL">
								<asp:Label ID="lblCompleteDt" runat="server" ViewStateMode="Enabled" Text='<%# Eval("CompleteByDate") %>'></asp:Label>
							</td>
						</tr>
					</ItemTemplate>
					<AlternatingItemTemplate>
						<tr id="trItem" runat="server" class="ClsGridAltRow">
							<td align="left" class="paddingL">
								<asp:Label ID="lblSubject" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Subject.SubjectName") %>'></asp:Label>
							</td>
							<td align="left" class="paddingL">
								<asp:LinkButton ID="lnkTitle" PostBackUrl="ViewHomeworkUI.aspx?"
									runat="server" ViewStateMode="Enabled" Text='<%# Eval("Title") %>'></asp:LinkButton>
							</td>
							<td align="center" class="paddingL">
								<asp:Label ID="lblCompleteDt" runat="server" ViewStateMode="Enabled" Text='<%# Eval("CompleteByDate") %>'></asp:Label>
							</td>
						</tr>
					</AlternatingItemTemplate>
					<EmptyDataTemplate>
						<tr>
							<td width="550px" align="center" class="LblNoRecord">
								No record found.
							</td>
						</tr>
					</EmptyDataTemplate>
				</asp:ListView>
			</td>
		</tr>
        <tr id="trHR" runat="server">
            <td>
                <hr style="border-color:Gray;border-width:2px;" />
            </td>
        </tr>
        <tr id="trLogHeader" runat="server">
            <td>
                <span class="clsLabel" style="font-weight:bold;">Daily Logs :</span>
            </td>
        </tr>
        <tr id="trLogFilter" runat="server">
                <td align="center">
                    <table>
                        <tr>
                          	<td class="ClsBorderLight paddingL" style="width: 130px">
								<span class="ClsLabel">Date:</span>
							</td>
							<td>
								<asp:TextBox ID="txtStartDate" runat="server" EnableViewState="true" CssClass="SmlTxtBox" MaxLength="11"
						      TabIndex="4"></asp:TextBox>
                              	 <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" ValidationGroup="Save" InvalidDateMessage=""
                                            AutoPostBack="False" />
                                   
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Show%>"
                                    CssClass="ClsBtn"  CausesValidation="false"  OnClick="btnSearch_Click"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <tr id="trLogData" runat="server">
                <td align="center">
                    <table width="50%">
                        <tr runat="server" id="trTotalRec" align="center">
                            <td align="center">
                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwHomeworklogs"  OnDataBound="lstvwHomeworklogs_DataBound" ViewStateMode="Enabled"
                                        OnItemDataBound="lstvwHomeworklogs_ItemDataBound">
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
                                        <asp:ListView ID="lstvwHomeworklogs" runat="server" DataKeyNames="Id" OnDataBound="lstvwHomeworklogs_DataBound"
                                            onitemdatabound="lstvwHomeworklogs_ItemDataBound" 
                                            onsorting="lstvwHomeworklogs_Sorting" ViewStateMode="Enabled" >
                                    <LayoutTemplate>
                                    <table id="tblhomework" align="center" width="100%" runat="server" class="GridBorder">
											<tr id="trHeader" runat="server" class="ClsGridHeader">													
												<th align="center" width="150px">
                                                <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
												<%--<asp:LinkButton ID="Label4" runat="server"  CausesValidation="false" Text="Date" CommandName="Sort" CommandArgument="Date"></asp:LinkButton>--%>
												</th>
												<th align="left" class="paddingL" width="150px">
													<asp:Label ID="Label5" runat="server" Text="Attachment"></asp:Label>
												</th>
                                                    
											</tr>
											<tr runat="server" id="itemPlaceholder">
											</tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                <td colspan="2">
                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwHomeworklogs" PageSize="20">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" ViewStateMode="Enabled" />
                                                                                <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged" ViewStateMode="Enabled">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="right" class="LblNormal">
                                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" ViewStateMode="Enabled" />
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
											<td align="center" class="paddingL">
												<asp:Label ID="lblCompleteDt" runat="server" Text='<%# Eval("Date" ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidIsPublished" runat="server" Value='<%# Eval("IsPublished") %>' />
											</td>
											<td align="left" class="paddingL">
												<asp:HyperLink ID="lnkAttachment" runat="server" Text="Click Here"></asp:HyperLink>
                                            </td>                                              
										</tr>
									</ItemTemplate>
									<AlternatingItemTemplate>
										<tr id="Tr2" runat="server" class="ClsGridAltRow">												
											<td align="center" class="paddingL">
												<asp:Label ID="lblCompleteDt" runat="server" Text='<%# Eval("Date" ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidIsPublished" runat="server" Value='<%# Eval("IsPublished") %>' />
											</td>
											<td align="left" class="paddingL">
												<asp:HyperLink ID="lnkAttachment" runat="server" Text="Click Here"></asp:HyperLink>
                                            </td>                                            
										</tr>
									</AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <tr>
                                            <td class="LblNoRecord" align="center" colspan="2">
                                                <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                            </td>
                                        </tr>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                    <asp:ObjectDataSource TypeName="BusinessLogic.HomeworkDailyLogBL" EnablePaging="True" ViewStateMode="Enabled"
                                    ID="objdsHomeworks" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                    SelectCountMethod="Count" EnableCaching="False">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:SessionParameter Name="aiUserRoleId" SessionField="S_USERLOGIN_ROLE_ID" Type="int32" />
                                        <asp:ControlParameter ControlID="txtStartDate" Name="asFilter" Type="String" PropertyName="Text" />
                                        <asp:SessionParameter Name="asStdDivId" SessionField="S_STUDENT_STANDERED_DIVISION_ID" Type="String" />
                                        <asp:Parameter Name="sortExpression" Type="String" />
                                        <asp:Parameter Name="sortDirection" Type="String" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                        <asp:Parameter Name="maximumRows" Type="Int32" />                                               
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                               
                                <asp:HiddenField ID="hidSortExpression" runat="server" Value="" ViewStateMode="Enabled" />
                                <asp:HiddenField ID="hidSortDirection" runat="server" Value="" ViewStateMode="Enabled" />
                                <asp:HiddenField ID="hidmsgConfirmDelete" runat="server" ViewStateMode="Enabled" />
                                    <asp:HiddenField ID="hidDueDateShouldNotBeBlank" runat="server" ViewStateMode="Enabled" />
                            </td>
                        </tr>                                
                    </table>
                </td>
            </tr>
	</table>

       <script language="javascript" type="text/javascript">
        function OpenFile(file) {
                window.open(file, '_blank')
                return false;
            }
            </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server" ViewStateMode="Enabled">
</asp:Content>
