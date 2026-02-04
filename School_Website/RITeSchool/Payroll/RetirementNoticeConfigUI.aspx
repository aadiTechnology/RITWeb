<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="RetirementNoticeConfigUI.aspx.cs" Inherits="RetirementNoticeConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
    <style>
        .clsLabelC {
            font-family: Open Sans !important;
        }       
    </style>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="95%">
            <tr id="trInvestmentDetails" runat="server">
                <td>
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                                </td>
                                                <td align="left" width="150px">
                                                    <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="50%">
                                            <tr>
                                                <td align="center" id="tdMessage" runat="server">												
                                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                                        Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>												
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>                                           
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="190px">
                                                    <span class="ClsLabel">User Role :</span>
                                                </td>
                                                <td style="white-space:nowrap">
                                                    <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="LrgCombo" AutoPostBack="true" >                                      
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqCmbCategory" runat="server" Display="None" ControlToValidate="cmbUserRole"
                                                        CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="User Role should be selected."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle" style="white-space:nowrap">
                                                    <span class="ClsLabel">Retirement Age :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAge" runat="server" CssClass="LrgTxtBox" MaxLength="2" 
                                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
														<span class="ClsMdtStar">*</span>																												
                                                </td>																						
                                                                                        
                                            </tr>
											<tr>
											<td class="ClsBorderlight" align="left" style="width:150px;white-space:nowrap">
                                                <span class="ClsLabel">Reminder (in days) :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDays" runat="server" MaxLength="3" CssClass="LrgTxtBox" onblur="extractNumber(this,1,false);"
                                                       Style="text-align: right; padding-right: 5px" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>												
												<span class="ClsMdtStar">*</span>                                                   
                                            </td>
											</tr>                                         									
                                            
											<tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" disable-page="true"
                                                    Text="Save" onclick="btnSave_Click"  />													
                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                                     Text="Cancel" onclick="btnCancel_Click"  />
                                            </td>
                                           </tr>
                                            
                                        </table>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwRetirementNotice" runat="server" DataKeyNames="Id" 
											onitemcommand="lstvwRetirementNotice_ItemCommand" onitemdatabound="lstvwRetirementNotice_ItemDataBound">					                                            
                                            
                                            <LayoutTemplate>
                                                <table width="50%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="center" width="10%" style="white-space:nowrap;">
															Sr. No.
														</th>
														<th   align="left" width="40%" class="paddingL" style="white-space:nowrap;">
													         User Role
												        </th>
                                                        <th   align="center" width="15%" style="white-space:nowrap;">
													        Retirement Age
												        </th>
                                                       <th   align="center" width="15%" style="white-space:nowrap;">
													        Reminder (in days)
												        </th>
                                                       														
                                                        <th  align="center" width="5%">
                                                            Edit
                                                        </th>                                                        
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
													<td align="center">
															<asp:Label ID="lblSrNo" runat="server"></asp:Label>
													 </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblUserRole" runat="server" CssClass="clsLabelC" Text='<%#Eval("UserRole.Name") %>'></asp:Label>                                                       
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRetirementAge" runat="server" CssClass="clsLabelC" Text='<%#Eval("RetirementAge") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblReminderDays" runat="server" CssClass="clsLabelC" Text='<%#Eval("ReminderDays") %>'></asp:Label>
                                                    </td>                                                    													
                                                    <td align="center" width="" >
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                             ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit" />
                                                    </td>                                                    
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
													<td align="center">
															<asp:Label ID="lblSrNo" runat="server"></asp:Label>
													 </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblUserRole" runat="server" CssClass="clsLabelC" Text='<%#Eval("UserRole.Name") %>'></asp:Label>                                                       
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRetirementAge" runat="server" CssClass="clsLabelC" Text='<%#Eval("RetirementAge") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblReminderDays" runat="server" CssClass="clsLabelC" Text='<%#Eval("ReminderDays") %>'></asp:Label>
                                                    </td>                                                    													
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                             ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit" />
                                                    </td>                                                    
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr >
                                                    <td class="LblNoRecord" align="center" >
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>                                        
                                    </td>
                                </tr>
								<tr>
									<td align="center">
										 <asp:Button CssClass="ClsBtn" ID="btnBack" CausesValidation="false" runat="server"
                                                     Text="Back" onclick="btnBack_Click"   />
									</td>
								</tr>
                                
                               </table>
							            <asp:CustomValidator ID="cstValidateAge" runat="server" Display="None" ErrorMessage=""
                                                        SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidateAge">
                                        </asp:CustomValidator>
										<asp:CustomValidator ID="cstValidateDays" runat="server" Display="None" ErrorMessage=""
                                                        SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidateDays">
                                        </asp:CustomValidator>										
										<asp:HiddenField ID="hidRetNoticeConfigId" runat="server" value="0"/>
										 <asp:HiddenField ID="hidIsConfigured" runat="server" />                                      
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>        
            
        </table>
    </div>
	<script type="text/javascript" language="javascript">

		_clienttxtAge = "<%= this.txtAge.ClientID %>";
		_clienttxtDays = "<%= this.txtDays.ClientID %>";
		_clientcmbUserRole = "<%= this.cmbUserRole.ClientID %>";
		_clienthidRetNoticeId = "<%= this.hidRetNoticeConfigId.ClientID %>";
		_clientlblMessage = "<%=this.lblMessage.ClientID %>";
		_clientlstvwRetirementNotice = "<%= this.lstvwRetirementNotice.ClientID %>";

		function ClearSuccessfulMessage() {
			document.getElementById(_clientlblMessage).innerHTML = "";
		}

		function ResetFields() {
			
			$get(_clientcmbUserRole).disabled = false;
			$get(_clienttxtAge).value = "";
			$get(_clienthidRetNoticeId).value = 0;
			$get(_clienttxtDays).value = "";
			$get(_clientcmbUserRole).value = 0;

		}

		function ValidateAge(src, args) {
			var Age = $get(_clienttxtAge);
			if (Age.value.trim() == "") {
				src.errormessage = "Retirement Age should not be blank.";
				args.IsValid = false;
				return true;
			}
			else
				if (parseInt(Age.value) <= 49 || parseInt(Age.value) >= 100 ) {
					src.errormessage = "Retirement Age should be between 50 to 99.";
					args.IsValid = false;
					return true;
				}
			args.IsValid = true;
			return false;
		}

		function ValidateDays(src, args) {
			var Days = $get(_clienttxtDays);
			if (Days.value.trim() == "") {
				src.errormessage = "Reminder (in days) should not be blank.";
				args.IsValid = false;
				return true;
			}
			else
				if (parseInt(Days.value) <= 29 || parseInt(Days.value) >= 181) {
					src.errormessage = "Reminder (in days) should be between 30 to 180.";
					args.IsValid = false;
					return true;
				}
			args.IsValid = true;
			return false;
		}

	</script>
	</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>

