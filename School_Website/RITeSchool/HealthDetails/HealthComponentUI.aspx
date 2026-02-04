<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="HealthComponentUI.aspx.cs" Inherits="HealthComponentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="95%">
            <tr id="trComponentsDetails" runat="server">
                <td>
                    <table width="100%">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="LblErrorMsg"
                                                        ShowSummary="true" ValidationGroup="Save" />
                                                </ContentTemplate>
                                                 <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwComponents" EventName="ItemCommand" />
                                                </Triggers>
                                                </asp:UpdatePanel>
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
                                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="50%">
                                            <tr>
                                                <td align="center" id="tdMessage" runat="server">
                                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                                        Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lstvwComponents" EventName="ItemCommand" />
                                        </Triggers>
                                        </asp:UpdatePanel> 
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    <asp:UpdatePanel ID="upnl3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="190px">
                                                    <span class="ClsLabel">Component Name :</span>
                                                </td>
                                                <td style="white-space: nowrap">
                                                    <asp:TextBox ID="txtComponentName" runat="server" CssClass="LrgTxtBox">                                                   
                                                    </asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqtxtComponentName" runat="server" Display="None"
                                                        ControlToValidate="txtComponentName" CssClass="ClsMdtStar" ErrorMessage="Component Name should not be blank" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle" style="white-space: nowrap">
                                                    <span class="ClsLabel">Sort Order :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSortOrder" runat="server" CssClass="LrgTxtBox" MaxLength="5"
                                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqtxtSortOrder" runat="server" Display="None" ControlToValidate="txtSortOrder"
                                                        CssClass="ClsMdtStar" ErrorMessage="Sort Order should not be blank" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle" style="white-space: nowrap">
                                                    <span class="ClsLabel">Is Fitness Component? :</span>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsFitnessComponent" runat="server" CssClass="checkbox" >
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>                                            
                                        </table>
                                        </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwComponents" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                        <td colspan="2" align="center">
                                        <asp:UpdatePanel ID="upnl4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn"  Text="Save" disable-page="true"
                                                OnClick="btnSave_Click" ValidationGroup="Save" OnClientClick="ClearMessages();" />                                    
                                            <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                                Text="Cancel" OnClick="btnCancel_Click" />
                                        </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwComponents" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                <tr>
                                    <td align="center">
                                     <asp:UpdatePanel ID="upnl5" runat="server" UpdateMode="Conditional">
                                     <ContentTemplate>
                                        <asp:ListView ID="lstvwComponents" runat="server" DataKeyNames="Id" onItemDataBound="lstvwComponents_ItemDataBound"
                                            onItemCommand="lstvwComponents_ItemCommand">
                                            <LayoutTemplate>
                                                <table width="60%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" width="50%">
                                                            Component Name
                                                        </th>
                                                        <th align="center" width="10%">
                                                            Sort Order
                                                        </th>
                                                        <th align="center" width="20%">
                                                            Is Fitness Component?
                                                        </th>
                                                        <th align="center" width="10%">
                                                            Edit
                                                        </th>
                                                        <th width="10%">
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
                                                     <asp:Label ID="lblComponent" runat="server" CssClass="ClsLabel" Text='<%#Eval("ComponentName") %>'></asp:Label>
                                                     <asp:HiddenField ID="hidHealthComponentId" runat="server" Value='<%# Eval("Id") %>'  />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblSortOrder" runat="server" CssClass="ClsLabelR" Text='<%#Eval("SortOrder") %>'></asp:Label>
                                                    </td>                                                 
                                                    <td align="center">                        
                                                        <asp:Image ID="imgIsFitnessComponent" runat="server" ImageUrl='<%# Eval("IsFitnessComponent") %>' ImageAlign="Right"/>
                                                    </td>
                                                    <td align="center" width="">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" ToolTip="Delete" OnClientClick="return ConfirmDelete()" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblComponent" runat="server" CssClass="ClsLabel" Text='<%#Eval("ComponentName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidHealthComponentId" runat="server" Value='<%# Eval("Id") %>'  />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblSortOrder" runat="server" CssClass="ClsLabelR" Text='<%#Eval("SortOrder") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Image ID="imgIsFitnessComponent" runat="server" ImageUrl='<%# Eval("IsFitnessComponent") %>' ImageAlign="Right"  />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" ToolTip="Delete" OnClientClick="return ConfirmDelete()" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:HiddenField ID="hidIsConfigured" runat="server" />
                                        <asp:HiddenField ID="hidHealthComponentId" runat="server" Value="0" />
                                        </ContentTemplate>                                      
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />                                               
                                                <asp:AsyncPostBackTrigger ControlID="lstvwComponents" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button CssClass="ClsBtn" ID="btnBack" CausesValidation="false" runat="server"
                                            Text="Back" OnClick="btnBack_Click" />
                                        
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <asp:CustomValidator ID="ParameterNameValidator" runat="server" Display="None" ValidationGroup="Save"
		                        ClientValidationFunction="ValidateComponentName" EnableClientScript="true" />
                            <asp:CustomValidator ID="SortOrderValidator" runat="server" Display="None" ValidationGroup="Save"
	                            ClientValidationFunction="ValidateSortOrder" EnableClientScript="true" />
                            </td> </tr> </table>
       
    </div>
    	<script type="text/javascript" language="javascript">
    	    
            _clientlstvwComponents = "<%=this.lstvwComponents.ClientID %>"
            _clienttxtComponentName = "<%=this.txtComponentName.ClientID %>"
            _clienttxtSortOrder = "<%=this.txtSortOrder.ClientID %>"
            _clienthidHealthComponentId = "<%=this.hidHealthComponentId.ClientID %>"   
            _clientlblMessage = "<%=this.lblMessage.ClientID %>"   
            var _empty = '';     
            function ConfirmDelete() {
    	        return window.confirm('Are you sure you want to delete this record?')
    	    }
            
    	    function ValidateSortOrder(src, args) {
                var HealthComponentId = $get(_clienthidHealthComponentId); 
                var sMessage = false
                var sOrderMessage = false
    	        var rowIndex = 0
                var cComponentId = $get(_clienttxtComponentName).value;
    	        var componentName = document.getElementById(_clientlstvwComponents + "_ctrl" + rowIndex + "_lblComponent");
    	        var txtSortOrder = $get(_clienttxtSortOrder);
                var sortOrder = parseInt(txtSortOrder.value.trim());

                if(sortOrder == 0 ) {
                      src.errormessage = "Sort Order should be greater than zero.";   
                       args.IsValid = false
    	            return true          
                }
               
    	        while (componentName != null) {
                    var ComponentId = document.getElementById(_clientlstvwComponents + "_ctrl" + rowIndex + "_hidHealthComponentId")
    	            var SortOrder = document.getElementById(_clientlstvwComponents + "_ctrl" + rowIndex + "_lblSortOrder") 
                      	            
    	            if ( sortOrder == SortOrder.innerHTML.trim() && ComponentId.value != HealthComponentId.value) {
    	                sMessage = true
    	                break;
    	            }    	         
    	            rowIndex = rowIndex + 1;
    	            componentName = document.getElementById(_clientlstvwComponents + "_ctrl" + rowIndex + "_lblComponent");
    	        }
    	        if (sMessage == true) {
                    src.errormessage = "Sort Order should not be duplicate.";
    	            args.IsValid = false
    	            return true
    	        }
    	        args.IsValid = true
    	        return false
    	    }

            function ValidateComponentName(src, args) {
                var sMessage = false
                var rowIndex = 0
	            var componentName =document.getElementById(_clientlstvwComponents + "_ctrl" + rowIndex + "_lblComponent");
                var txtComponentName = $get(_clienttxtComponentName);  
                var cComponentId = $get(_clienttxtComponentName).value;
                var HealthComponentId = $get(_clienthidHealthComponentId); 
                while (componentName != null)  {
                    var lstcomponentName = document.getElementById(_clientlstvwComponents + "_ctrl" + rowIndex + "_lblComponent")
                    var ComponentId = document.getElementById(_clientlstvwComponents + "_ctrl" + rowIndex + "_hidHealthComponentId")
                    if(txtComponentName.value.trim() == lstcomponentName.innerHTML.trim() && ComponentId.value != HealthComponentId.value)
                    {
                        sMessage = true
                        break;
                    }                       
                    rowIndex =rowIndex + 1;
                    componentName =document.getElementById(_clientlstvwComponents + "_ctrl" + rowIndex + "_lblComponent");
                }                           
                if (sMessage == true) {
                    src.errormessage = "Component Name should not be duplicate.";
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false                             
            }   

            function ClearMessages() {
	            var lblErrorMessage = $get(_clientlblMessage);
	            if(lblErrorMessage)
		            lblErrorMessage.innerHTML = _empty;	
            }

	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
