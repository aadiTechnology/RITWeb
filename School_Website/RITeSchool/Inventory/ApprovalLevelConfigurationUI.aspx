<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ApprovalLevelConfigurationUI.aspx.cs" Inherits="ApprovalLevelConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        legend {
            padding-left: 2px;
            padding-right: 2px;
            width: unset;
            border-bottom:unset;
            color: #36C;
            margin-bottom:5px!important;
        }

        fieldset {
            padding: .35em .625em .75em;
            margin: 0 10px;
            border: 2px groove threedface;
        }
    </style>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%"
        class="ClsPanel">
        <tr>
            <td align="center">
                <asp:Panel ID="pnlConfiguration" runat="server" GroupingText="Approval Level Configuration" 
                    BorderStyle="None">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%;
                        font-family: Arial,sans-serif;">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ApprovalValidationSummary" runat="server" CssClass="LblErrorMsg"
                                            EnableViewState="false" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstvwApprovalLevel" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                    </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" class="ClsBorderlight Clspadding">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" runat="server" id="tblConfiguration">
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label CssClass="LblErrorMsg" ID="lblErr" runat="server" EnableViewState="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" id="tblCreatorLevel">
                                                        <tr>
                                                            <td>                                                               
                                                                    <span id="lblCreatorDesignation" class="ClsLblLgnd">Creator</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCreatorDesignation" runat="server" AutoPostBack="True" Width="150px"
                                                                    OnSelectedIndexChanged="ddlCreatorDesignation_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="reqValddlCreatorApprovalLevel0" runat="server" ControlToValidate="ddlCreatorDesignation"
                                                                    Display="None" ErrorMessage="Creator should be selected." InitialValue="0"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" runat="server" id="tblFirstLevel"
                                                        visible="false">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                              
                                                                    <span id="lblFirstApprovalLevel" class="LblNrmlB">First Approver</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="middle" width="30px">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/RITeSchool/images/ArrowOrangeDbl.gif" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlFirstApprovalLevel" runat="server" AutoPostBack="True" Width="150px"
                                                                    OnSelectedIndexChanged="ddlFirstApprovalLevel_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="reqValddlFifthApprovalLevel" runat="server" ControlToValidate="ddlFirstApprovalLevel"
                                                                    Display="None" ErrorMessage="First Approver should be selected." InitialValue="0"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" runat="server" id="tblSecondLevel"
                                                        visible="false">
                                                        <tr>
                                                            <td>
                                                               
                                                                    <span id="Span1" class="LblNrmlB">Second Approver</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="margin-left: 40px">
                                                                <asp:DropDownList ID="ddlSecondApprovalLevel" runat="server" AutoPostBack="True"
                                                                    Width="150px" OnSelectedIndexChanged="ddlSecondApprovalLevel_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="reqdValddlSecondApprovalLevel" runat="server" ControlToValidate="ddlSecondApprovalLevel"
                                                                    Display="None" ErrorMessage="Second Approver should be selected." InitialValue="0"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" runat="server" id="tblThirdLevel"
                                                        visible="false">
                                                        <tr>
                                                            <td>
                                                               
                                                                <span id="Span2" class="LblNrmlB">Third Approver</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlThirdApprovalLevel" runat="server" AutoPostBack="True" Width="150px"
                                                                    OnSelectedIndexChanged="ddlThirdApprovalLevel_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="reqdValddlThirdApprovalLevel" runat="server" ControlToValidate="ddlThirdApprovalLevel"
                                                                    Display="None" ErrorMessage="Third Approver should be selected." InitialValue="0"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" runat="server" id="tblFourthLevel"
                                                        visible="false">
                                                        <tr>
                                                            <td>
                                                               
                                                                    <span id="Span3" class="LblNrmlB">Fourth Approver</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlFourthApprovalLevel" runat="server" AutoPostBack="True"
                                                                    Width="155px" OnSelectedIndexChanged="ddlFourthApprovalLevel_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="reqdValddlFourthApprovalLevel" runat="server" ControlToValidate="ddlFourthApprovalLevel"
                                                                    Display="None" ErrorMessage="Fourth Approver should be selected." InitialValue="0"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" runat="server" id="tblFifthLevel"
                                                        visible="false">
                                                        <tr>
                                                            <td cssclass="LblNrmlB">
                                                               
                                                                <span id="Span4" class="LblNrmlB">Fifth Approver</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlFifthApprovalLevel" runat="server" AutoPostBack="True" Width="150px"
                                                                    OnSelectedIndexChanged="ddlFifthApprovalLevel_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="reqdValddlFifthApprovalLevel" runat="server" ControlToValidate="ddlFifthApprovalLevel"
                                                                    Display="None" ErrorMessage="Fifth Approver should be selected." InitialValue="0"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" visible="true">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <asp:Button ID="btnAddLevel" runat="server" CssClass="ClsBtnLrg" OnClick="btnAddLevel_Click"
                                                                    Text="Add Next Level" Visible="False" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnRemoveLevel" runat="server" CssClass="ClsBtnLrg" Text="Remove Last Level"
                                                                    OnClick="btnRemoveLevel_Click" CausesValidation="False" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblActionCntrls" runat="server" visible="true">
                                            <tr>
                                                <td valign="top" align="left">
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="ClsBtnMid" OnClick="btnAdd_Click"
                                                        Text="Add" CommandName="AddLevel" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtnMid" OnClick="btnCancel_Click"
                                                        Text="Cancel" CausesValidation="False" CommandName="CancelLevel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstvwApprovalLevel" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%">
                                            <tr>
                                                <td valign="top">
                                                    <asp:ListView ID="lstvwApprovalLevel" ItemPlaceholderID="ContactRowContainer" runat="server"
                                                        OnItemCommand="lstvwApprovalLevel_ItemCommand">
                                                        <LayoutTemplate>
                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" class="GridBorder"
                                                                cellpadding="0" cellspacing="1">
                                                                <tr class="ClsGridHeader">
                                                                    <th align="left" class="ClspaddingL">
                                                                        Creator
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        First Appover
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        Second Appover
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        Third Appover
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        Fourth Appover
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        Fifth Appover
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        Edit
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        Delete
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="ContactRowContainer" />
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr class="ClsGridRow">
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidCreator" runat="server" Value='<%# Eval("RequisitionByDesignationID") %>' />
                                                                    <asp:Label ID="lblCreator" runat="server" Text='<%# Eval("RequisitionByDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidFirstAppover" runat="server" Value='<%# Eval("FirstDesignationID") %>' />
                                                                    <asp:Label ID="lblFirstAppover" runat="server" Text='<%# Eval("FirstDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidSecondAppover" runat="server" Value='<%# Eval("SecondDesignationID") %>' />
                                                                    <asp:Label ID="lblSecondAppover" runat="server" Text='<%# Eval("SecondDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidThirdAppover" runat="server" Value='<%# Eval("ThirdDesignationID") %>' />
                                                                    <asp:Label ID="lblThirdAppover" runat="server" Text='<%# Eval("ThirdDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidFourthAppover" runat="server" Value='<%# Eval("FourthDesignationID") %>' />
                                                                    <asp:Label ID="lblFourthAppover" runat="server" Text='<%# Eval("FourthDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidFifthAppover" runat="server" Value='<%# Eval("fifthDesignationID") %>' />
                                                                    <asp:Label ID="lblFifthAppover" runat="server" Text='<%# Eval("fifthDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                        CommandArgument='<%# Eval("ApprovalLevelConfigurationID") %>' CommandName="EditLevel" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgbtnDelete" CausesValidation="false" CommandArgument='<%# Eval("ApprovalLevelConfigurationID") %>'
                                                                        runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove"
                                                                        OnClientClick="if(!ConfirmDelete()) return false;" Visible="true" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr class="ClsGridAltRow">
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidCreator" runat="server" Value='<%# Eval("RequisitionByDesignationID") %>' />
                                                                    <asp:Label ID="lblCreator" runat="server" Text='<%# Eval("RequisitionByDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidFirstAppover" runat="server" Value='<%# Eval("FirstDesignationID") %>' />
                                                                    <asp:Label ID="lblFirstAppover" runat="server" Text='<%# Eval("FirstDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidSecondAppover" runat="server" Value='<%# Eval("SecondDesignationID") %>' />
                                                                    <asp:Label ID="lblSecondAppover" runat="server" Text='<%# Eval("SecondDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidThirdAppover" runat="server" Value='<%# Eval("ThirdDesignationID") %>' />
                                                                    <asp:Label ID="lblThirdAppover" runat="server" Text='<%# Eval("ThirdDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidFourthAppover" runat="server" Value='<%# Eval("FourthDesignationID") %>' />
                                                                    <asp:Label ID="lblFourthAppover" runat="server" Text='<%# Eval("FourthDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:HiddenField ID="hidFifthAppover" runat="server" Value='<%# Eval("fifthDesignationID") %>' />
                                                                    <asp:Label ID="lblFifthAppover" runat="server" Text='<%# Eval("fifthDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                        CommandArgument='<%# Eval("ApprovalLevelConfigurationID") %>' CommandName="EditLevel" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgbtnDelete" CausesValidation="false" CommandArgument='<%# Eval("ApprovalLevelConfigurationID") %>'
                                                                        runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove"
                                                                        OnClientClick="if(!ConfirmDelete()) return false;" Visible="true" />
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="hidCurrentLevel" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hidSelectedDesignationIds" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hidIsConfig" runat="server" Value="Y" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddLevel" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAddLevel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Panel ID="pnlFinalApproval" runat="server" GroupingText="Final Approver Configuration"
                    BorderStyle="None">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 30%; height: 98%"
                        class="LblUsrNameSml">
                        <tr>
                            <td align="center">
                                <asp:ListView ID="lstvwFinalApprover" ItemPlaceholderID="ContactRowContainer" runat="server"
                                    DataKeyNames="Designation_Id">
                                    <LayoutTemplate>
                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" class="GridBorder"
                                            cellpadding="0" cellspacing="1">
                                            <tr class="ClsGridHeader">
                                                <th>
                                                </th>
                                                <th align="left" class="ClspaddingL">
                                                    Final Appover
                                                </th>
                                            </tr>
                                            <tr runat="server" id="ContactRowContainer" />
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class="ClsGridRow">
                                            <td align="left" class="ClspaddingL" width="10%">
                                                <asp:CheckBox ID="chkCanFinalApprove" runat="server" Checked='<%# Eval("IsFinalApproval") %>'>
                                                </asp:CheckBox>
                                            </td>
                                            <td align="left" class="ClspaddingL">
                                                <asp:Label ID="lblFirstAppover" runat="server" Text='<%# Eval("Designation_Name") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ClsGridAltRow">
                                            <td align="left" class="ClspaddingL" width="10%">
                                                <asp:CheckBox ID="chkCanFinalApprove" runat="server" Checked='<%# Eval("IsFinalApproval") %>'>
                                                </asp:CheckBox>
                                            </td>
                                            <td align="left" class="ClspaddingL">
                                                <asp:Label ID="lblFirstAppover" runat="server" Text='<%# Eval("Designation_Name") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="height: 40px">
                                <asp:Button ID="btnSaveFinalApprovers" runat="server" CssClass="ClsBtnMid" OnClick="btnSaveFinalApprovers_Click" disable-page="true"
                                    Text="Save" CausesValidation="False" />
                                <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" OnClick="btnBack_Click"
                                    Text="Back" BorderWidth="1px" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        _clientbtnSaveFinalApprovers = "<%=this.btnSaveFinalApprovers.ClientID %>"
        _clientbtnBack = "<%=this.btnBack.ClientID %>"
        _clientbtnAdd = "<%=this.btnAdd.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clientbtnAddLevel = "<%=this.btnAddLevel.ClientID %>"
        _clientbtnRemoveLevel = "<%=this.btnRemoveLevel.ClientID %>"
        _clientlblErr = "<%=this.lblErr.ClientID %>"
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_beginRequest(BeginReqHandler)
        prm.add_endRequest(EndReqHandler)
        function BeginReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnSaveFinalApprovers || postBackElement.id == _clientbtnAdd)
                DisableButtons(true)
        }
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnSaveFinalApprovers || postBackElement.id == _clientbtnAdd)
                DisableButtons(false)
        }
        function DisableButtons(action) {

            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function' && action)
                isPageValid = Page_ClientValidate()
            if (isPageValid) {
                if (document.getElementById(_clientbtnBack) != null)
                    document.getElementById(_clientbtnBack).disable = action
                if (document.getElementById(_clientbtnSaveFinalApprovers) != null)
                    document.getElementById(_clientbtnSaveFinalApprovers).disable = action
                if (document.getElementById(_clientbtnAdd) != null)
                    document.getElementById(_clientbtnAdd).disable = action
                if (document.getElementById(_clientbtnCancel) != null)
                    document.getElementById(_clientbtnCancel).disable = action
                if (document.getElementById(_clientbtnAddLevel) != null)
                    document.getElementById(_clientbtnAddLevel).disable = action
                if (document.getElementById(_clientbtnRemoveLevel) != null)
                    document.getElementById(_clientbtnRemoveLevel).disable = action

            }
        }
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this Configuration?')) {
                bResult = false
            }
            return bResult
        }

        function SetError() {
            if (document.getElementById(_clientlblErr) != null)
                document.getElementById(_clientlblErr).innerHTML = "";
        }
    </script>

</asp:Content>
