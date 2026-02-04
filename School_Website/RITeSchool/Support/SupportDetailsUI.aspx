<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="SupportDetailsUI.aspx.cs" Inherits="SupportDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
    <div>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
	<table width="100%">
		<tr>
			<td align="right" class="LblNormal ClsMdtStar">
				* Mandatory Fields
			</td>
		</tr>
		<tr>
			<td align="center">
				<table width="700px">
				    <tr>
						<td align="center">
							<table id="tblControls" runat="server">
								
                                
                                <tr>
                                    <td class="ClsBorderlight paddingL" style="width: 200px;" runat="server" id="tdSubject">
										 <asp:Label ID="lblSubject" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, SupportUserSubject%>"
                                          EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td class="ClsBorderlight paddingL" style="width: 250px;" runat="server"  id="tdSubjectCmb">
										<asp:TextBox runat="server" ID="txtSubject" TextMode="MultiLine" Height="50px" Width="250px" ReadOnly="true"></asp:TextBox>
                                    </td>
								</tr>
								<tr>
									<td class="ClsBorderlight paddingL" style="width: 200px;">
									     <asp:Label ID="lblEmlAddress" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, SupportUserEmailAddress%>"
                                         EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td class="ClsBorderlight paddingL" style="width: 250px;">
										 <asp:Label CssClass="LblNormal" ID="lblEmailAddress" runat="server"></asp:Label>
									</td>
								</tr>
                                <tr>
									<td class="ClsBorderlight paddingL" style="width: 200px;">
										 <asp:Label ID="lblMobileNo" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, SupportUserMobNumber%>"
                                                            EnableViewState="false"></asp:Label>
                                          <span class="ClsLabel colonPadding">:</span>
									</td>
									<td class="ClsBorderlight paddingL"  style="width: 250px;">
										<asp:Label ID="lblMobileNumber" CssClass="LblNormal" runat="server"></asp:Label>
									
									</td>
								</tr>
                                 <tr>
									<td class="ClsBorderlight paddingL" style="width: 200px;">
										 <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, SupportUserDescription%>"
                                         EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td class="ClsBorderlight paddingL" style="width: 250px;">
										<asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Height="75px" ReadOnly="true"  Width="250px"></asp:TextBox>
								    </td>
								</tr>
                                 <tr id="attachement" runat="server">
									<td class="ClsBorderlight paddingL" style="width: 200px;">
										 <asp:Label ID="lblAttachment" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, SupportUserAttachement%>"
                                         EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td class="ClsBorderlight paddingL" style="width: 250px;">
										 <asp:LinkButton CssClass="btnDwnload" ID="btnDownload" runat="server" CausesValidation="false" ToolTip="<%$ Resources:LocalizedResources, ToolTipViewAttachment%>"
                                           CommandName="DOWNLOAD" />
									</td>
								</tr>
                            </table>
						</td>
					</tr>
					<tr>
                      <td>
                        <asp:HiddenField runat="server" ID="hidFileName" />
                        <asp:HiddenField runat="server" ID="hidRowCount" />
                      </td>
                    </tr>
					<tr>
						<td align="center">
							<table width="1300px">
                                <tr id="trLstview" runat="server">
									<td>
										<asp:ListView ID="lstvwSupportDetails" runat="server" DataKeyNames="Id" 
                                            onitemcommand="lstvwSupportDetails_ItemCommand" 
											>
											<LayoutTemplate>
												<table id="tblhomework" align="center" runat="server" class="GridBorder" width="1200">
													<tr id="trHeader" runat="server" class="ClsGridHeader" style="height:20px;">
														<th align="left"  class="paddingL" width="60px" style="white-space:nowrap;">
															<asp:Label ID="lblUserRoll" runat="server" Text="<%$ Resources:LocalizedResources, UserRole%>"></asp:Label>
														</th>
                                                        <th align="left"  class="paddingL" width="100px" style="white-space:nowrap;">
															<asp:Label ID="lblUserName" runat="server" Text="<%$ Resources:LocalizedResources, UserName%>"></asp:Label>
														</th>
                                                        <th align="left"  class="paddingL" width="300px">
															<asp:Label ID="lblDisplayName" runat="server" Text="<%$ Resources:LocalizedResources, SupportUserSubject%>"></asp:Label>
														</th>
														<th align="left" class="paddingL" width="30px">
															<asp:Label ID="lblShortName" runat="server" Text="<%$ Resources:LocalizedResources, SupportUserEmailAddress%>"></asp:Label>
														</th>
														<th align="left" class="paddingL"   width="95px">
															<asp:Label ID="lblDisplayValue" runat="server" Text="<%$ Resources:LocalizedResources, SupportUserMobNumber%>"></asp:Label>
														</th>
													    <th align="center" width="30px">
															<asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, SupportDetailsView%>"></asp:Label>
														</th>
													</tr>
													<tr runat="server" id="itemPlaceholder">
													</tr>
                                                  </table>
											</LayoutTemplate>
											<ItemTemplate>
												<tr id="Tr2" runat="server" class="ClsGridRow">
													<td align="left" class="paddingL">
														<asp:Label ID="lblUserRoll" runat="server" Text='<%# Eval("UserRole") %>'></asp:Label>
													</td>
                                                    <td align="left" class="paddingL" width="100px" style="white-space:nowrap;">
														<asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
													</td>
                                                    <td align="left" class="paddingL"  width="380px">
														<asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL" width="40px" >
														<asp:Label ID="lblShortName" runat="server" Text='<%# Eval("EmailAddress") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL"  width="70px">
														<asp:Label ID="lblDisplayValue" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
													</td>
												    <td align="center">
														<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
															ToolTip="<%$ Resources:LocalizedResources, ToolTipViewSupprtDetls%>" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
													</td>
												</tr>
											</ItemTemplate>
											<AlternatingItemTemplate>
												<tr id="Tr2" runat="server" class="ClsGridAltRow">
													<td align="left" class="paddingL">
														<asp:Label ID="lblUserRoll" runat="server" Text='<%# Eval("UserRole") %>'></asp:Label>
													</td>
                                                    <td align="left" class="paddingL" width="100px" style="white-space:nowrap;">
														<asp:Label ID="lblUserName" width="100px" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
													</td>
                                                    <td align="left" class="paddingL" width="380px" >
														<asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL" width="40px">
														<asp:Label ID="lblShortName" runat="server" Text='<%# Eval("EmailAddress") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL"  width="70px">
														<asp:Label ID="lblDisplayValue" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
													</td>
												    <td align="center">
														<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
															ToolTip="<%$ Resources:LocalizedResources, ToolTipViewSupprtDetls%>" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
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
								<tr>
									<td class="style1">
									</td>
								</tr>
                             </table>
						</td>
					</tr>
                </table>
			</td>
		</tr>
	</table>
               </ContentTemplate>
        </asp:UpdatePanel>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" Runat="Server">
</asp:Content>

