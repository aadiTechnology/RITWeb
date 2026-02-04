<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="HealthParametersUI.aspx.cs" Inherits="HealthParametersUI" %>

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
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="LblErrorMsg"
                                                        ShowSummary="true" ValidationGroup="Save" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwParameters" EventName="ItemCommand" />
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
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
                                        <asp:AsyncPostBackTrigger ControlID="lstvwParameters" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="190px">
                                                    <span class="ClsLabel">Component Name :</span>
                                                </td>
                                                <td style="white-space: nowrap">
                                                    <asp:DropDownList ID="cmbComponentName" runat="server" CssClass="LrgCombo" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqcmbComponentName" runat="server" Display="None"
                                                        ControlToValidate="cmbComponentName" CssClass="ClsMdtStar" ErrorMessage="Component Name should be selected"
                                                        ValidationGroup="Save" InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="190px">
                                                    <span class="ClsLabel">Parameter Name :</span>
                                                </td>
                                                <td style="white-space: nowrap">
                                                    <asp:TextBox ID="txtParameterName" runat="server" CssClass="LrgTxtBox">                                                   
                                                    </asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqtxtParameterName" runat="server" Display="None"
                                                        ControlToValidate="txtParameterName" CssClass="ClsMdtStar" ErrorMessage="Parameter Name should not be blank"
                                                        ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr id="trTest" style="display: none">
                                                <td align="left" class="ClsBorderlight" width="190px">
                                                    <span class="ClsLabel">Test :</span>
                                                </td>
                                                <td style="white-space: nowrap">
                                                    <asp:TextBox ID="txtTest" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trMeasure" style="display: none">
                                                <td align="left" class="ClsBorderlight" width="190px">
                                                    <span class="ClsLabel">Measure :</span>
                                                </td>
                                                <td style="white-space: nowrap">
                                                    <asp:TextBox ID="txtMeasure" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
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
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwParameters" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" disable-page="true"
                                            OnClick="btnSave_Click" ValidationGroup="Save" OnClientClick="ClearMessages();" />
                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                            Text="Cancel" OnClick="btnCancel_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwParameters" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ListView ID="lstvwParameters" runat="server" DataKeyNames="Id" OnItemCommand="lstvwParameters_ItemCommand">
                                            <LayoutTemplate>
                                                <table width="95%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="center" width="10%">
                                                            Component Name
                                                        </th>
                                                        <th align="center" width="25%">
                                                            Parameter Name
                                                        </th>
                                                        <th align="center" width="20%">
                                                            Test
                                                        </th>
                                                        <th align="center" width="25%">
                                                            Measure
                                                        </th>
                                                        <th align="center" width="6%">
                                                            Sort Order
                                                        </th>
                                                        <th align="center" width="5%">
                                                            Edit
                                                        </th>
                                                        <th align="center" width="5%">
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
                                                        <asp:HiddenField ID="hidHealthComponentId" runat="server" Value='<%# Eval("HealthComponentId") %>' />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblParameter" runat="server" CssClass="ClsLabel" Text='<%#Eval("ParameterName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidHealthParameterId" runat="server" Value='<%# Eval("Id") %>' />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblTest" runat="server" CssClass="ClsLabel" Text='<%#Eval("TestName") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblMeasure" runat="server" CssClass="ClsLabel" Text='<%#Eval("Measure") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblSortOrder" runat="server" CssClass="ClsLabelR" Text='<%#Eval("SortOrder") %>'></asp:Label>
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
                                                        <asp:HiddenField ID="hidHealthComponentId" runat="server" Value='<%# Eval("HealthComponentId") %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblParameter" runat="server" CssClass="ClsLabel" Text='<%#Eval("ParameterName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidHealthParameterId" runat="server" Value='<%# Eval("Id") %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblTest" runat="server" CssClass="ClsLabel" Text='<%#Eval("TestName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblMeasure" runat="server" CssClass="ClsLabel" Text='<%#Eval("Measure") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblSortOrder" runat="server" CssClass="ClsLabelR" Text='<%#Eval("SortOrder") %>'></asp:Label>
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
                                        <asp:HiddenField ID="hidHealthParameterId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidHealthComponentIdIsFitnessComponent" runat="server" Value="0" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwParameters" EventName="ItemCommand" />
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
                        ClientValidationFunction="ValidateParameterName" EnableClientScript="true" />
                    <asp:CustomValidator ID="SortOrderValidator" runat="server" Display="None" ValidationGroup="Save"
                        ClientValidationFunction="ValidateSortOrder" EnableClientScript="true" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientlstvwParameters = "<%=this.lstvwParameters.ClientID %>"
        _clientcmbComponentName = "<%=this.cmbComponentName.ClientID %>"
        _clienttxtParameterName = "<%=this.txtParameterName.ClientID %>"
        _clienttxtSortOrder = "<%=this.txtSortOrder.ClientID %>"
        _clienthidhidHealthComponentIdIsFitnessComponent = "<%=this.hidHealthComponentIdIsFitnessComponent.ClientID %>"
        _clienthidHealthParameterId = "<%= this.hidHealthParameterId.ClientID %>"
        _clientlblMessage = "<%=this.lblMessage.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)

        function EndReqHandler(sender, args) {        
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id.match("btnEdit") != null) {
                ActivateTestMeasure();
            }
        }

        function ConfirmDelete() {
            return window.confirm('Are you sure you want to delete this record?')
        }
               
        ActivateTestMeasure();
        function ActivateTestMeasure() {
            var cComponentId = $get(_clientcmbComponentName).value;
            var vhidHealthComponentIdIsFitnessComponent = $get(_clienthidhidHealthComponentIdIsFitnessComponent);
            var componentIds = vhidHealthComponentIdIsFitnessComponent.value.split(',');
            var i;
            var isfound = false;
            for (i = 0; i < componentIds.length; i++) {
                if (componentIds[i] != "" && parseInt(cComponentId) == parseInt(componentIds[i])) {
                    isfound = true;
                    break;
                }
            }
            if (isfound) {
                $('#trTest').fadeIn(200);
                $('#trMeasure').fadeIn(200);
            }
            else {
                $('#trTest').fadeOut(200);
                $('#trMeasure').fadeOut(200);
            }
        }

        function ValidateSortOrder(src, args) {
            var HealthParameterId = $get(_clienthidHealthParameterId);
            var cmbComponentName
            var sMessage = false
            var rowIndex = 0
            var componentName = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblComponent");
            var cComponentId = $get(_clientcmbComponentName).value;
            var txtSortOrder = $get(_clienttxtSortOrder);
            var HealthParameterId = $get(_clienthidHealthParameterId);
            var sortOrder = parseInt(txtSortOrder.value.trim());

            if (sortOrder == 0) {
                src.errormessage = "Sort Order should be greater than zero.";
                args.IsValid = false
                return true
            }
            while (componentName != null) {
                var SortOrder = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblSortOrder")
                var hidHealthComponentId = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_hidHealthComponentId")
                var ParameterId = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_hidHealthParameterId")
                if (cComponentId == hidHealthComponentId.value && HealthParameterId.value != ParameterId.value) {
                    if (sortOrder == SortOrder.innerHTML.trim()) {
                        sMessage = true
                        break;
                    }

                }
                rowIndex = rowIndex + 1;
                componentName = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblComponent");
            }

            if (sMessage == true) {
                src.errormessage = "Sort Order should not be duplicate in selected component.";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }


        function ValidateParameterName(src, args) {
            var sMessage = false
            var rowIndex = 0
            var componentName = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblComponent");
            var cComponentId = $get(_clientcmbComponentName).value;
            var txtParameterName = $get(_clienttxtParameterName);
            var HealthParameterId = $get(_clienthidHealthParameterId);
            while (componentName != null) {
                var ParameterName = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblParameter")
                var hidHealthComponentId = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_hidHealthComponentId")
                var ParameterId = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_hidHealthParameterId")
                if (cComponentId == hidHealthComponentId.value && HealthParameterId.value != ParameterId.value) {
                    if (txtParameterName.value.trim() == ParameterName.innerHTML.trim()) {
                        sMessage = true
                        break;
                    }

                }
                rowIndex = rowIndex + 1;
                componentName = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblComponent");
            }
            if (sMessage == true) {
                src.errormessage = "Parameter Name should not be duplicate in selected component.";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ClearMessages() {
            var lblErrorMessage = $get(_clientlblMessage);
            if (lblErrorMessage)
                lblErrorMessage.innerHTML = '';
        }         

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
