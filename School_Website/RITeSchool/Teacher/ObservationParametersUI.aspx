<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ObservationParametersUI.aspx.cs" Inherits="ObservationParametersUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr id="trControls" runat="server">
                <td align="center">
                    <table width="75%">
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Parameter should not be blank."
                                            ControlToValidate="txtParameter" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" viewstatemode="Enabled" Display="None" ControlToValidate="txtParameter" ErrorMessage="Length of parameter should not exceed 300 characters." CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,300}$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Sort Order should not be blank."
                                            ControlToValidate="txtSortOrder" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Paramter Name should not be duplicate." Display="None" ClientValidationFunction="ValidateDuplicateParameter"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Sort Order should not be duplicate." Display="None" ClientValidationFunction="ValidateDuplicateSortOrder"></asp:CustomValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbSkill" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwobParameter" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnUnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td colspan="5" align="center">
                                            <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true" Text=""
                                                        EnableViewState="false"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbSkill" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwobParameter" EventName="ItemCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnUnSubmit" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            <span class="ClsLabel">Standard : </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                        <td style="width: 50px;">
                                        </td>
                                        <td class="ClsBorderlight">
                                            <span class="ClsLabel">Skill : </span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbSkill" runat="server" CssClass="ExLrgCombo" Width="300px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="cmbSkill_SelectedIndexChanged1">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr style="border-style: solid; border-width: thin; color: Silver;" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="90%">
                                            <tr>
                                                <td class="ClsBorderlight" width="150px">
                                                    <span class="ClsLabel">Parameter : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParameter" runat="server" CssClass="ExLrgTxtBox" Style="width: 95%; height:50px;"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" width="150px">
                                                    <span class="ClsLabel">Sort Order : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSortOrder" runat="server" CssClass="SmlTxtBox" MaxLength="2"
                                                        Style="text-align: right; width: 50px; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwobParameter" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click"
                                            Enabled="false" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" OnClick="btnCancel_Click"
                                            CausesValidation="false" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbSkill" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwobParameter" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ListView ID="lstvwobParameter" runat="server" DataKeyNames="Id" OnItemCommand="lstvwobParameter_ItemCommand"
                                                        OnItemDataBound="lstvwobParameter_ItemDataBound" OnSorting="lstvwobParameter_Sorting">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th align="left" class="clsLabelgrd">
                                                                        Parameter
                                                                        <%--<asp:LinkButton ID="lnkTitle" runat="server" CommandName="Sort" CommandArgument="Title"
                                                                            CausesValidation="false" ForeColor="Black"> Parameter </asp:LinkButton>--%>
                                                                    </th>
                                                                    <th align="right" class="clsLabelgrd" width="100px">
                                                                        Sort Order
                                                                        <%--<asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="SortOrder"
                                                                            CausesValidation="false" ForeColor="Black"> Sort Order </asp:LinkButton>--%>
                                                                    </th>
                                                                    <th width="100px" align="center" class="clsLabelgrd">
                                                                        Is Submitted?
                                                                    </th>
                                                                    <th width="50px" align="center" class="clsLabelgrd">
                                                                        Edit
                                                                    </th>
                                                                    <th width="50px" class="clsLabelgrd">
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
                                                                    <asp:Label ID="lblTitle" runat="server" CssClass="ClsLabel" Text='<%#Eval("Parameter") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                                </td>
                                                                <td align="right" class="ClsLabelR">
                                                                    <asp:Label ID="lblSortOrder" runat="server" Text='<%#Eval("SortOrder") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnIsSubmitted" runat="server" CausesValidation="false" CommandName=""
                                                                        Visible="false" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                                        ToolTip="Is Submitted?" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                        ToolTip="Edit" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                        ToolTip="Delete" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="left">
                                                                    <asp:Label ID="lblTitle" runat="server" CssClass="ClsLabel" Text='<%#Eval("Parameter") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                                </td>
                                                                <td align="right" class="ClsLabelR">
                                                                    <asp:Label ID="lblSortOrder" runat="server" Text='<%#Eval("SortOrder") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnIsSubmitted" runat="server" CausesValidation="false" CommandName=""
                                                                        Visible="false" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                                        ToolTip="Is Submitted?" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                        ToolTip="Edit" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                        ToolTip="Delete" />
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
                                                    <asp:HiddenField ID="hidParameterId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbSkill" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwobParameter" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnUnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:UpdatePanel ID="upnl4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" OnClick="btnSubmit_Click"
                                            Enabled="false" CausesValidation="false" />
                                        <asp:Button ID="btnUnSubmit" runat="server" Text="un Submit" CssClass="ClsBtn" OnClick="btnUnSubmit_Click"
                                            Enabled="false" CausesValidation="false" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbSkill" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwobParameter" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnUnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            _clienttxtParameter = "<%=this.txtParameter.ClientID %>"
            _clienttxtSortOrder = "<%=this.txtSortOrder.ClientID %>"
            _clienthidParameterId = "<%=this.hidParameterId.ClientID %>"
            _clientlstvwobParameter = "<%=this.lstvwobParameter.ClientID %>"
            _clientlblMessage = "<%=this.lblMessage.ClientID %>"

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?')
            }

            function ClearMessage()
            {
                $('#'+_clientlblMessage).html('')
            }

            function ValidateDuplicateParameter(src, args) {                
                var newParamter = $('#' + _clienttxtParameter).val()                
                var newId = $('#' + _clienthidParameterId).val()

                var bDuplicateFound = false;
                $('[id$=_lblTitle]').each(function () {
                    var parameter = $(this).html()
                    var id = $('#' + this.id.replace('_lblTitle', '_hidId')).val()
                    if (newId != id && newParamter == parameter) {
                        bDuplicateFound = true;
                        return false;
                    }
                })

                if (bDuplicateFound) {
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }

            function ValidateDuplicateSortOrder(src, args) {
                var newSortOrder = $('#' + _clienttxtSortOrder).val()
                var newId = $('#' + _clienthidParameterId).val()

                var bDuplicateFound = false;
                $('[id$=_lblSortOrder]').each(function () {
                    var sortOrder = $(this).html()
                    var id = $('#' + this.id.replace('_lblSortOrder', '_hidId')).val()
                    if (newId != id && newSortOrder == sortOrder) {
                        bDuplicateFound = true;
                        return false;
                    }
                })

                if (bDuplicateFound) {
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }

            

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
