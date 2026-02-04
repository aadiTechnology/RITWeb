<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="PaymentParameterPopup.aspx.cs" Inherits="PaymentParameterPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <asp:UpdatePanel runat="server" ID="Update1">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="0" cellspacing="1">
                <tr>
                    <td align="left" colspan="3">
                        <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td style="height: 20px" class="MainTitleHead">                                    
                                    <asp:Label ID="Label1" runat="server" style="font-weight: bold; padding-right: 5px;" Text="<%$ Resources:LocalizedResources, PaymentParameter%>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trMandatory" runat="server">
                    <td align="right" colspan="6">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:ValidationSummary ID="valSumError" runat="server" HeaderText="Please correct following errors."
                            CssClass="ClsMdtStar" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td align="center" id="tdMessage" runat="server" colspan="2">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" id="td1" runat="server" colspan="2">
                        <asp:Label ID="lblErrorMessage" runat="server" EnableViewState="false" CssClass="ClsLabel"
                            Style="color:Red;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table align="center">
                            <tr>
                                <td style="white-space: nowrap" class="ClsBorderlight">
                                    <asp:Label ID="lblParameter" runat="server" CssClass="ClsLabel" Text="<%$Resources:LocalizedResources, Parameter%>"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td  style="white-space: nowrap">
                                    <asp:TextBox ID="txtParameter" runat="server" CssClass="LrgTxtBox" Width="290px" EnableViewState="false"
                                        MaxLength="50" TabIndex="1"></asp:TextBox>
									<span style="color: red" id="spnMandatory" runat="server">*</span>
                                    <asp:CustomValidator ID="cstvalParameter" runat="server" ClientValidationFunction="ValidateParameter"
                                        CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                        TabIndex="2" BorderWidth="1px" CommandName="Save" OnClick="btnSave_Click"></asp:Button>
                                    <asp:CustomValidator ID="cstvalSave" runat="server" ClientValidationFunction="ValidateDuplicates"
                                        CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                    <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                        TabIndex="3" Text="<%$ Resources:LocalizedResources, Cancel%>" BorderWidth="1px"
                                        OnClick="btnCancel_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table style="width: 100%;" cellpadding="0" cellspacing="1">
                <tr>
                    <td align="center" valign="top" runat="server" id="tdGroup">
                        <asp:ListView ID="lstvwParameters" runat="server" ItemPlaceholderID="trItemPlaceholder"
                            ClientIDMode="Inherit" OnItemCommand="lstvwParameters_ItemCommand" DataKeyNames="Id">
                            <LayoutTemplate>
                                <table id="tblParamenter" style="width: 550px; color: #333333" class="GridBorder"
                                    cellpadding="0" cellspacing="1">
                                    <tr id="trParameterHeader" runat="server" class="ClsGridHeader">
                                        <th class="ClspaddingL" style="width: 76%;">
                                            <asp:Label ID="lblParameterHeader" runat="server" CssClass="ClsLabelC" Text="<%$Resources:LocalizedResources, Parameter%>"></asp:Label>
                                        </th>
                                        <th align="center" style="width: 12%;" class="Clspadding" id="thEdit" runat="server">
                                            <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                        </th>
                                        <th align="center" style="width: 12%;" class="Clspadding" id="thDelete" runat="server">
                                            <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                        </th>
                                    </tr>
                                    <tr id="trItemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <EmptyDataTemplate>
                                <table align="center" width="550px">
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Label ID="lblNoRecordFound" Width="550px" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"
                                                CssClass="LblNoRecord"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                    <td class="ClspaddingL" style="width: 76%;">
                                        <asp:Label runat="server" ID="lblParameter" Text='<%#Eval("Parameter")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hiddenId" Value='<%#Eval("Id")%>' />
                                    </td>
                                    <td align="center" style="width: 12%;" class="Clspadding" id="tdEdit" runat="server">
                                        <asp:ImageButton runat="server" ID="imgEdit" Text="Edit" CommandName="UpdateCommand"
                                            TabIndex="5" CommandArgument='<%#Eval("Id")%>' CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>"></asp:ImageButton>
                                    </td>
                                    <td align="center" style="width: 12%;" class="Clspadding" id="tdDelete" runat="server">
                                        <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="RemoveCommand"
                                            TabIndex="6" CommandArgument='<%#Eval("Id")%>' CausesValidation="false" ToolTip="<%$ Resources:LocalizedResources, Delete%>"
                                            ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDelete()) return false;">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hidParameterId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidCultureInfo" runat="server" />
                        <asp:HiddenField ID="hidAlert" runat="server" />
                        <asp:HiddenField ID="hidParameterEmpty" runat="server" />
                        <asp:HiddenField ID="hidParameterDeleted" runat="server" />
                        <asp:HiddenField ID="hidParameterSaved" runat="server" />
                        <asp:HiddenField ID="hidParameterUpdated" runat="server" />
                        <asp:HiddenField ID="hidRICheck" runat="server" />
                        <asp:HiddenField ID="hidAlreadyExists" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnClose" runat="server" CssClass="ClsBtnSml" UseSubmitBehavior="false" Text="<%$ Resources:LocalizedResources, Close%>"
                            CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript" language="javascript">
        _clientlstvwGroup = "<%=this.lstvwParameters.ClientID %>";
        _clientlblErrorMessage = "<%=this.lblErrorMessage.ClientID %>";
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        _clienthidParameterId = "<%=this.hidParameterId.ClientID %>";
        _clienttxtParameter = "<%=this.txtParameter.ClientID %>";
        
        function ClearMessage() {
            if ($get(_clientlblErrorMessage) != null)
                $get(_clientlblErrorMessage).innerHTML = "";
            if ($get(_clientlblMessage) != null)
                $get(_clientlblMessage).innerHTML = "";
        }

        function ConfirmDelete() {
            var bResult = true;
            if (!window.confirm($get("<%=hidAlert.ClientID%>").value)) {
                bResult = false;
            }
            return bResult;
        }

        function ValidateParameter(oSrc, args) {
            if ($get(_clienttxtParameter) != null && ($get(_clienttxtParameter).value).trim() == "") {
                ClearMessage();
                oSrc.errormessage = $get("<%=hidParameterEmpty.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return true;
        }

        function CloseWindow() {
            window.opener.location = window.opener.location.pathname;
            window.opener.focus();
            window.close();
        }

        function ValidateDuplicates(oSrc, args) {
            var ParameterId = $get(_clienthidParameterId).value;
            var ParameterName = $get(_clienttxtParameter).value;
            var IsEdit = false;

            if (ParameterId != "" && ParameterId != "0")
                IsEdit = true;

            var IsExists = false;
            var iRowCount = 0
            var ParameterName1 = document.getElementById(_clientlstvwGroup + "_ctrl" + iRowCount + "_lblParameter");
            var Id = document.getElementById(_clientlstvwGroup + "_ctrl" + iRowCount + "_hiddenId");

            while (Id != null && ParameterName.trim() != "") {
                    if (ParameterId != null && Id.value != ParameterId && ParameterName1 != null && ParameterName1.innerHTML.toLowerCase() == ParameterName.trim().toLowerCase()) {
                        IsExists = true;
                        break;
                    }               

                iRowCount = iRowCount + 1;
                ParameterName1 = document.getElementById(_clientlstvwGroup + "_ctrl" + iRowCount + "_lblParameter");
                Id = document.getElementById(_clientlstvwGroup + "_ctrl" + iRowCount + "_hiddenId");
            }

            if (IsExists) {
                ClearMessage();
                oSrc.errormessage = $get("<%=hidAlreadyExists.ClientID%>").value;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return true;
        }    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
