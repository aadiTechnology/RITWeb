<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="IncomeTaxSlabsUI.aspx.cs" Inherits="IncomeTaxSlabsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
    <style>
        .ClsLabel
        {
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
                                                    <span class="ClsLabel">Category :</span>
                                                </td>
                                                <td style="white-space:nowrap">
                                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="LrgCombo" AutoPostBack="true"                                                      
														onselectedindexchanged="cmbCategory_SelectedIndexChanged" >
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqCmbCategory" runat="server" Display="None" ControlToValidate="cmbCategory"
                                                        CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="Category should be selected."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle" style="white-space:nowrap">
                                                    <span class="ClsLabel">From Amount :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromAmount" runat="server" CssClass="LrgTxtBox" MaxLength="10" Enabled="false"
                                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false">0</asp:TextBox>														
                                                </td>																						
                                                                                        
                                            </tr>
											<tr>
											<td class="ClsBorderlight" align="left" style="width:150px;white-space:nowrap">
                                                <span class="ClsLabel">To Amount :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToAmount" runat="server" MaxLength="9" CssClass="LrgTxtBox" onblur="extractNumber(this,1,false);"
                                                       Style="text-align: right; padding-right: 5px" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                               
												<span class="ClsMdtStar">*</span>                                                   
                                            </td>
											</tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle" style="white-space:nowrap">
                                                    <span class="ClsLabel">Percentage :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPercentage" runat="server" CssClass="LrgTxtBox" MaxLength="3"
                                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
													onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
														<span class="ClsMdtStar">*</span>                                                    
                                                </td>																						
                                                                                                            
                                            </tr>									
                                            
											<tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" disable-page="true"
                                                    Text="Save" onclick="btnSave_Click"  />													
                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                                     Text="Cancel" onclick="btnCancel_Click" />
                                            </td>
                                           </tr>
                                            
                                        </table>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwSlabs" runat="server" 
											onitemcommand="lstvwSlabs_ItemCommand" DataKeyNames="Id" onitemdatabound="lstvwSlabs_ItemDataBound">                                            
                                            
                                            <LayoutTemplate>
                                                <table width="50%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th   align="left" width="40%">
													        Category
												        </th>
                                                        <th   align="center" width="15%">
													        From Amount
												        </th>
                                                       <th   align="center" width="15%">
													        To Amount
												        </th>
                                                        <th align="center" width="10%">
													        Percentage
												        </th>														
                                                        <th align="center" width="5%">
                                                            Edit
                                                        </th>
                                                        <th width="5%">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblCategory" runat="server" CssClass="ClsLabel" Text='<%#Eval("Category.Name") %>'></asp:Label>                                                       
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblFromAmount" runat="server" CssClass="ClsLabelR" Text='<%#Eval("FromAmount") %>'></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblToAmount" runat="server" CssClass="ClsLabelR" Text='<%#Eval("ToAmount") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPercentage" runat="server" CssClass="ClsLabelR" Text='<%#Eval("Percentage") %>'></asp:Label>                                                        
                                                    </td>													
                                                    <td align="center" width="" >
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                             ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="Delete" OnClientClick="return ConfirmDelete()" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblCategory" runat="server" CssClass="ClsLabel" Text='<%#Eval("Category.Name") %>'></asp:Label>                                                        
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblFromAmount" runat="server" CssClass="ClsLabelR" Text='<%#Eval("FromAmount") %>'></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblToAmount" runat="server" CssClass="ClsLabelR" Text='<%#Eval("ToAmount") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPercentage" runat="server" CssClass="ClsLabelR" Text='<%#Eval("Percentage") %>'></asp:Label>                                                        
                                                    </td>													
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                             ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="Delete" OnClientClick="return ConfirmDelete()"/>
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
                                                     Text="Back" onclick="btnBack_Click"  />
									</td>
								</tr>
                                
                               </table>
							            <asp:CustomValidator ID="cstTGF" runat="server" Display="None" ErrorMessage=""
                                                        SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidateAmount">
                                        </asp:CustomValidator>
										<asp:CustomValidator ID="cstValidatePercentage" runat="server" Display="None" ErrorMessage=""
                                                        SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidatePercentage">
                                        </asp:CustomValidator>
										<asp:HiddenField ID="hidIncomeTaxRangeId" runat="server" Value = "0"/>
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

	_clienttxtFromAmount = "<%=this.txtFromAmount.ClientID %>";
    _clienttxtToAmount = "<%=this.txtToAmount.ClientID %>";
    _clienttxtPercentage = "<%=this.txtPercentage.ClientID %>";
    _clientlblMessage = "<%=this.lblMessage.ClientID %>";
    
    function ConfirmDelete() {
    	return window.confirm('Are you sure you want to delete this record?')    		
    }

    function ClearSuccessfulMessage() {
    	document.getElementById(_clientlblMessage).innerHTML = "";
    }

    function ValidateAmount(src,args) {
    	var FromAmount = document.getElementById(_clienttxtFromAmount);
    	var ToAmount = document.getElementById(_clienttxtToAmount);

    	if (ToAmount.value.trim() == "") {
    		src.errormessage = "To Amount should not be blank.";
    		args.IsValid = false;
    		return true;
    	}
    	else
    		if (parseInt(ToAmount.value) <= parseInt(FromAmount.value)) {
    			src.errormessage = "To Amount should be greater than From Amount.";
    			args.IsValid = false;
    			return true;
    		}    	
    			args.IsValid = true;
    			return false;
      }

      function ValidatePercentage(src, args) {
      	var Percentage = document.getElementById(_clienttxtPercentage);
      	if (Percentage.value.trim() == "") {
      		src.errormessage = "Percentage should not be blank.";
      		args.IsValid = false;
      		return true;
      	}
      	else
      		if (parseFloat(Percentage.value.trim()) < 0 || parseFloat(Percentage.value.trim()) > 100) {
      			src.errormessage = "Percentage should be between 0 to 100.";
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