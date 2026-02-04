<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master" CodeFile="DisplayMenuContents.aspx.cs" Inherits="DisplayMenuContents" ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
	<table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%; height: 100%">
		<tr>
			<td style="background-color: white" id="MainDataTable" align="center" valign="top">
				<!-- Data Insert Here -->
				<table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
					<tr>
						<td colspan="4" align="left">
							<div class="MainDataTable">
								<%= sMenuContent %>
								<% if (sMenuContent != null && sMenuContent.Trim().Length > 0 && lstvwAttachments.Items.Count > 0) { %>
								<hr style="border: 0; height: 0; border-top: 1px solid rgba(0, 0, 0, 0.1); border-bottom: 1px solid rgba(255, 255, 255, 0.3);"/>
								<% } %>
								<div id="attachments">
									<asp:ListView ID="lstvwAttachments"
												  runat="server"
                                                  EnableViewState="true"
												  OnItemDataBound="lstvwAttachments_ItemDataBound"
												  OnDataBound="lstvwAttachments_DataBound">
										<LayoutTemplate>
											<table align="center" width="45%">
												<tr>
													<td align="center" width="100%" style="font-weight: bold; padding: 3px;" class="TotalCount ClsBorderBlue">
														<asp:Label ID="lblAttachmentTitle"
																   runat="server"
                                                                   EnableViewState="true"
																   CssClass="ClsConfigText" />
													</td>
												</tr>
												<tr id="itemPlaceholder" runat="server" EnableViewState="true" />
											</table>
										</LayoutTemplate>
										<ItemTemplate>
											<tr>
												<td align="center" width="20%" style="font-size: large; padding: 3px;" class="WeekDCell ClsBorderlight">
													<asp:HyperLink ID="attchmentLink"
																   runat="server"
                                                                   EnableViewState="true"
																   CssClass="Lbl10ptB"
																   NavigateUrl="#"
																   Text='<%# Eval("Name") %>' />
												</td>
											</tr>
										</ItemTemplate>
										<EmptyDataTemplate>
										</EmptyDataTemplate>
									</asp:ListView>
                                    <div style="height:15px;"></div>

                                    <asp:ListView ID="lstvwURLs"
												  runat="server"
                                                  EnableViewState="true" ondatabound="lstvwURLs_DataBound" 
                                        onitemdatabound="lstvwURLs_ItemDataBound">
										<LayoutTemplate>
											<table align="center" width="45%">
												<tr>
													<td align="center" width="100%" style="font-weight: bold; padding: 3px;" class="TotalCount ClsBorderBlue">
														<asp:Label ID="lblAttachmentTitle"
																   runat="server"
                                                                   EnableViewState="true"
																   CssClass="ClsConfigText" />
													</td>
												</tr>
												<tr id="itemPlaceholder" runat="server" EnableViewState="true" />
											</table>
										</LayoutTemplate>
										<ItemTemplate>
											<tr>
												<td align="center" width="20%" style="font-size: large; padding: 3px;" class="WeekDCell ClsBorderlight">
													<asp:HyperLink ID="attchmentLink"
																   runat="server"
                                                                   EnableViewState="true"
																   CssClass="Lbl10ptB"
																   NavigateUrl="#"
																   Text='<%# Eval("Name") %>' />
												</td>
											</tr>
										</ItemTemplate>
										<EmptyDataTemplate>
										</EmptyDataTemplate>
									</asp:ListView>
								</div>
							</div>
						</td>
					</tr>
					<tr>
						<td colspan="4" align="center">
							<asp:Button ID="btnBack"
										runat="server"
                                        EnableViewState="true"
										Text="Back"
										CssClass="ClsBtn"
										Height="24px"
										CausesValidation="False"
										PostBackUrl="~/RITeSchool/Common/ControlPanel.aspx"
										UseSubmitBehavior="false" />
						</td>
					</tr>
				</table>
				<!-- Data Insert End Here -->
			</td>
		</tr>
	</table>
</asp:Content>
