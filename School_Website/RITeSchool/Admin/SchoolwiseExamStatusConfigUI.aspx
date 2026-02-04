<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="SchoolwiseExamStatusConfigUI.aspx.cs" Inherits="SchoolwiseExamStatusConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">

<div>

 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
	<table width="100%">
		<tr>
			<td align="right" width="100%" class="LblNormal ClsMdtStar">
				* Mandatory Fields
			</td>
		</tr>
		<tr>
			<td>
				<asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
					CssClass="ClsLabel" />
			</td>
		</tr>
		<tr>
			<td align="left">
				<asp:Label ID="lblError" CssClass="ClsMdtStar" Visible="false" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td align="center">
				<table width="700px">
					<tr id="trLegend" runat="server">
						<td align="center">
							<table width="700px">
								<tr>
									<td align="center">
										<asp:Label ID="lblSuccess" CssClass="ClsLabelUpdate" Font-Bold="True" ForeColor="Blue"
											Visible="false" runat="server"></asp:Label>
									</td>
								</tr>
                            </table>
						</td>
					</tr>
					<tr>
						<td>
						</td>
					</tr>
					<tr>
						<td align="center">
							<table id="tblControls" runat="server">
								<tr>
									<td class="ClsBorderlight paddingL" style="width: 200px;" runat="server" id="tdSubject">
										 <asp:Label ID="lblDisplyName" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, StatusDisplayName%>"
                                          EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td style="width: 250px;" runat="server" id="tdSubjectCmb">
										<asp:DropDownList ID="cmbDisplayName" runat="server" Width="200px" 
											AutoPostBack="true" onselectedindexchanged="cmbDisplayName_SelectedIndexChanged">
										</asp:DropDownList>
                                       <asp:RequiredFieldValidator ID="reqCmbCategory" runat="server" Display="None" ControlToValidate="cmbDisplayName" ValidationGroup="Save"
                                       CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="<%$ Resources:LocalizedResources, DisplayNameSelect %>"></asp:RequiredFieldValidator>
                                       <span class="ClsMdtStar">*</span>
									</td>
								</tr>
							
                                 <tr>
									<td class="ClsBorderlight paddingL" style="width: 200px; height:20px;">
										 <asp:Label ID="lblDisplayValue" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, StatusDisplayValue%>"
                                          EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td>
									  <div class="ClsBorderlight" style="width:200px; height:20px;">
                                         <asp:Label id="lblStatusDisplayValue" CssClass="LblNormal" runat="server" Text=" "></asp:Label>
                                      </div>	
                                     
									</td>
								</tr>

								<tr>
									<td class="ClsBorderlight paddingL">
										 <asp:Label ID="lblForeColor" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, StatusForeColor%>"
                                          EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td align="left" style="width: 150px;">           
                                      <asp:DropDownList ID="cmbForColor" runat="server" CssClass="LrgCombo" 
                                         CausesValidation="true" Width="200px">
                                      </asp:DropDownList>
                                      <span class="ClsMdtStar">*</span>
                                     <%-- <asp:CustomValidator ID="CstDuplicateForeColorValidation" runat="server" ClientValidationFunction="DuplicateForeColorValidation"
                                       SetFocusOnError="True" Display="None" ErrorMessage="" CssClass="LblErrorMsg" ValidationGroup="Save"></asp:CustomValidator>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None" ControlToValidate="cmbForColor" ValidationGroup="Save"
                                       CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="<%$ Resources:LocalizedResources, ForeColorSelect%>"></asp:RequiredFieldValidator>--%>
                                    </td>
								</tr>
                                
								<tr>
									<td class="ClsBorderlight paddingL" style="height:20px;" >
									   <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, StatusConsiderInTotal%>"
                                       EnableViewState="false"></asp:Label>
                                       <span class="ClsLabel colonPadding">:</span> 
									</td>
									<td align="left" style="width: 150px;">           
            
                                      <asp:CheckBox ID="chkbxConsiderInTotal" runat="server" />
                                     
                                    </td>
								</tr>
                                 <tr>
									<td class="ClsBorderlight paddingL" style="height:20px;">
									   <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, StatusDisplayTotal%>"
                                       EnableViewState="false"></asp:Label>
                                       <span class="ClsLabel colonPadding">:</span> 	 
									</td>
									<td align="left" style="width: 150px;">           
                                        <asp:CheckBox ID="chkbxDisplayTotal" runat="server" />
                                     </td>
								</tr>
                                <%-- <tr>
									<td class="ClsBorderlight paddingL" style="height:20px;">
										<asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, StatusConsiderAsPresent%>"
                                       EnableViewState="false"></asp:Label>
                                       <span class="ClsLabel colonPadding">:</span> 	 
									</td>
									<td align="left" style="width: 150px;">           
                                        <asp:CheckBox ID="chkbxConsiderAsPresent" runat="server" />
                                    </td>
								</tr>--%>
                                <tr>
									<td align="center" colspan="2">
										<asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn" ValidationGroup="Save"
											OnClick="btnSave_Click" />
										<asp:Button ID="btnCancel" runat="server" 
                                            Text="<%$ Resources:LocalizedResources, Cancel%>"  CssClass="ClsBtn" CausesValidation="false"
											OnClientClick="Clear();return false;" onclick="btnCancel_Click" />
								    </td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td>
						  <asp:HiddenField ID="hidId" runat="server" Value="0" />
                          <asp:HiddenField ID="hidCstShortNameDuplicate" runat="server"  />
                          <asp:HiddenField ID="hidCstDisplayValueDuplicate" runat="server"  />
                          <asp:HiddenField ID="hidCstForeColorDuplicate" runat="server"  />
                            <asp:HiddenField ID="hidCultureInfo" runat="server" Value="0" />
					    </td>
					</tr>
					
					<tr>
						<td align="center">
							<table width="1200px">
								<tr id="trGridTitle" runat="server" align="left" style="margin-left:30%;">
									<td class="ClsLblLgnd" style="padding-left: 25%;">
                                        <asp:Label ID="lblStatusHeader" runat="server" Text="<%$ Resources:LocalizedResources, ExamStatus%>"></asp:Label>
										<span class="ClsLblLgnds LblNrmlB">:</span>
									</td>
								</tr>
								<tr id="trLstview" runat="server">
									<td>
										<asp:ListView ID="lstvwExamStatus" runat="server" DataKeyNames="ExamStatusId" 
                                            onitemdatabound="lstvwExamStatus_ItemDataBound">
											<LayoutTemplate>
												<table id="tblhomework" align="center" runat="server" class="GridBorder" width="600px">
													<tr id="trHeader" runat="server" class="ClsGridHeader" style="height:15px;">
													    <th align="left" width="120px" class="ClsPaddingL">
															 <asp:Label ID="lblDisplayName" runat="server" Text="<%$ Resources:LocalizedResources, StatusDisplayName%>"
                                                              EnableViewState="false"></asp:Label>
														</th>
                                                        <th align="left" width="120px" class="ClsPaddingL">
															 <asp:Label ID="lblDisplayValue" runat="server" Text="<%$ Resources:LocalizedResources, StatusDisplayValue%>"
                                                              EnableViewState="false"></asp:Label>
														</th>
														<th align="left" width="120px" class="ClsPaddingL">
															 <asp:Label ID="lblForeColor" runat="server" CssClass="" Text="<%$ Resources:LocalizedResources, StatusForeColor%>"
                                                              EnableViewState="false"></asp:Label>
														</th>
                                                        <th align="center" width="130px">
															 <asp:Label ID="lblConsiderInTotal" runat="server" CssClass="ClsPaddingC" Text="<%$ Resources:LocalizedResources, StatusConsiderInTotal%>"
                                                             EnableViewState="false"></asp:Label>
														</th>
														<th align="center" width="100px">
															  <asp:Label ID="lblDisplayTotal" runat="server" CssClass="ClsPaddingC" Text="<%$ Resources:LocalizedResources, StatusDisplayTotal%>"
                                                               EnableViewState="false"></asp:Label>
														</th>

                                                    </tr>
													<tr runat="server" id="itemPlaceholder">
													</tr>
												</table>
											</LayoutTemplate>
											<ItemTemplate>
												<tr id="Tr2" runat="server" class="ClsGridRow">
													<td  align="left"  width="120px" class="ClspaddingL">
														<asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidExamStatusId" runat="server" Value='<%# Eval("ExamStatusId") %>' />
													</td>
                                                    <td  align="left"  width="120px"  class="ClspaddingL">
														<asp:Label ID="lblDisplayValue" runat="server" Text='<%# Eval("DisplayValue") %>'></asp:Label>
													</td>
													<td id="tdforecolor" align="left"  width="120px"  class="ClspaddingL">
														<asp:Label ID="lblForeColor" runat="server" Text='<%# Eval("ForeColor") %>'></asp:Label>
													</td>
                                                    <td align="center">
													     <asp:Image ID="imgConsiderInTotal" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" runat="server" Visible="false" />
                                                         <asp:HiddenField ID="hidConsiderInTtl" runat="server" Value='<%# Eval("ConsiderInTotal") %>' />
													</td>
													<td align="center">
														 <asp:Image ID="imgDisplayTotal" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" runat="server" Visible="false" />
                                                         <asp:HiddenField ID="hidDsplyTtl" runat="server" Value='<%# Eval("DisplayTotal") %>' />
													</td>

                                                </tr>
											</ItemTemplate>
											<AlternatingItemTemplate>
												<tr id="Tr2" runat="server" class="ClsGridAltRow">
													<td  align="left"  width="120px"  class="ClspaddingL">
														<asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidExamStatusId" runat="server" Value='<%# Eval("ExamStatusId") %>' />
													</td>
                                                    <td  align="left"  width="120px" class="ClspaddingL" >
														<asp:Label ID="lblDisplayValue" runat="server" Text='<%# Eval("DisplayValue") %>'></asp:Label>
													</td>
													<td id="tdforecolor" align="left"  width="120px"  class="ClspaddingL">
														<asp:Label ID="lblForeColor" runat="server" Text='<%# Eval("ForeColor") %>'></asp:Label>
													</td>
                                                    <td align="center">
													     <asp:Image ID="imgConsiderInTotal" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" runat="server" Visible="false" />
                                                         <asp:HiddenField ID="hidConsiderInTtl" runat="server" Value='<%# Eval("ConsiderInTotal") %>' />
													</td>
													<td align="center">
														 <asp:Image ID="imgDisplayTotal" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" runat="server" Visible="false" />
                                                         <asp:HiddenField ID="hidDsplyTtl" runat="server" Value='<%# Eval("DisplayTotal") %>' />
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
		             <tr>
						<td align="center">
							<asp:Button ID="btnBackEnd" runat="server" CausesValidation="false" 
                                Text="<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtn" onclick="btnBackEnd_Click"
								 />
						</td>
					</tr>

				</table>
			</td>
		</tr>
	</table>
               </ContentTemplate>
        </asp:UpdatePanel>
        </div>
	<script type="text/javascript">
	    _clientcmbDisplayName = "<%=this.cmbDisplayName.ClientID %>";
	    _clientvalSumErrorMsg ="<%=this.valSumErrorMsg.ClientID %>";
	    _clientlstvwExamStatus = "<%=this.lstvwExamStatus.ClientID%>";
        _clientlblStatusDisplayValue = "<%=this.lblStatusDisplayValue.ClientID%>";
        
      
      function ChangeColor(cmbForColor) {
	        cmbForColor.style.backgroundColor = cmbForColor.options[cmbForColor.selectedIndex].style.backgroundColor;
	    }
	
	    function Clear() {
	        $("#" + _clientlblStatusDisplayValue).val(' - ');
	        $("#" + _clientcmbDisplayName)[0].selectedIndex =0;
	        $("#" + _clientcmbForColor)[0].disabled = false;
	        $("#" + _clientvalSumErrorMsg).html('');
	        document.getElementById("chkbxConsiderInTotal").checked(true);
	        document.getElementById("chkbxDisplayTotal").checked(true);
	        document.getElementById("chkbxConsiderAsPresent").checked(true);
	        document.getElementById("lblSucess").style.visibility = "hidden";
            
         }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
