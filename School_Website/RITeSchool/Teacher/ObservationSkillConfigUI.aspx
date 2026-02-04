<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ObservationSkillConfigUI.aspx.cs" Inherits="ObservationSkillConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:RequiredFieldValidator ID="reqStandard" runat="server" ErrorMessage="Standard should be Selected."
                                            SetFocusOnError="True" InitialValue="0" ControlToValidate="cmbStandard" Display="None"
                                            ValidationGroup="SAVE"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqSubject" runat="server" Display="None" ControlToValidate="cmbSubject"
                                            InitialValue="0" ErrorMessage="Subject should be Selected." ValidationGroup="SAVE"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cstvalParameter" runat="server" ClientValidationFunction="ValidateSkill"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="SAVE"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValSortOrder" runat="server" ClientValidationFunction="ValidateSortOrder"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="SAVE"></asp:CustomValidator>
                                        <asp:ValidationSummary ID="valSave" runat="server" CssClass="lblNormal" ValidationGroup="SAVE"
                                            HeaderText="Please correct following errors." />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstvwobSkillConfig" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td width="20%" align="right">
                                <span class="ClsMdtStar">* Mandatory Fields </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdMessage" runat="server" colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwobSkillConfig" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trControls" runat="server">
                <td align="center">
                    <table width="75%">
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources,Standard%>"></asp:Label>
                                            <span class="ClsLabel">:</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                        <td style="width: 15px">
                                        </td>
                                        <td class="ClsBorderlight">
                                            <asp:Label ID="lblSubject" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources,Subject%>"></asp:Label>
                                            <span class="ClsLabel">:</span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnl10" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbSubject" runat="server" CssClass="ExLrgCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbSubject_SelectedIndexChanged">
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
                                <hr style="border: thin solid #C0C0C0" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="90%">
                                            <tr>
                                                <td class="ClsBorderlight" width="150px">
                                                    <asp:Label ID="lblSkill" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources,Skill%>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSkill" runat="server" CssClass="ExLrgTxtBox" Style="width: 95%"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" width="150px">
                                                    <asp:Label ID="lblSortOrder" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources,SortOrder%>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSortOrder" runat="server" CssClass="SmlTxtBox" MaxLength="2"
                                                        Style="text-align: right; width: 50px; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources,Save%>"
                                                        CssClass="ClsBtn" OnClick="btnSave_Click" ValidationGroup="SAVE" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources,Cancel%>"
                                                        CssClass="ClsBtn" OnClick="btnCancel_Click" ValidationGroup="CANCEL" CausesValidation="false" />
                                                </td>
                                            </tr>
                                            <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                            <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstvwobSkillConfig" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td class="ClsBorderLight" align="left">
                                            <asp:Label ID="lblSearch" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources,SkillSubject%>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" Width="500px"></asp:TextBox>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources,Search%>"
                                                class="ClsBtn" OnClick="btnSearch_Click" CausesValidation="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ListView ID="lstvwobSkillConfig" runat="server" DataKeyNames="Id" OnItemCommand="lstvwobSkillConfig_ItemCommand"
                                                        OnItemDataBound="lstvwobSkillConfig_ItemDataBound" OnSorting="lstvwobSkillConfig_Sorting">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th align="center" class="clsLabelgrd" width="400px">
                                                                        <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                            CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, Skill%>">Skill</asp:LinkButton>
                                                                    </th>
                                                                    <th align="right" class="clsLabelgrd" width="100px">
                                                                        <asp:LinkButton ID="lnkSort" runat="server" CommandName="Sort" CommandArgument="SortOrder"
                                                                            CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, SortOrder%>"></asp:LinkButton>
                                                                    </th>
                                                                    <th width="50px" align="center" class="clsLabelgrd">
                                                                        <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"></asp:Label>
                                                                    </th>
                                                                    <th width="50px" class="clsLabelgrd">
                                                                        <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="left">
                                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidSkillId" runat="server" Value='<%#Eval("Id") %>' />
                                                                </td>
                                                                <td align="right" style="padding-right: 5px;">
                                                                    <asp:Label ID="lblSortOrder" runat="server" Text='<%#Eval("SortOrder") %>'></asp:Label>
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
                                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidSkillId" runat="server" Value='<%#Eval("Id") %>' />
                                                                </td>
                                                                <td align="right" style="padding-right: 5px;">
                                                                    <asp:Label ID="lblSortOrder" runat="server" Text='<%#Eval("SortOrder") %>'></asp:Label>
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources,Back%>"
                                                        CssClass="ClsBtn" OnClick="btnBack_Click" CausesValidation="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstvwobSkillConfig" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">
            _clientTxtSkill = "<%=this.txtSkill.ClientID %>";
            _clientTxtSortOrder = "<%=this.txtSortOrder.ClientID %>";
            _clienthidId = "<%=this.hidId.ClientID %>";
            _clientlblMessage = "<%=this.lblMessage.ClientID %>";
            _clientlstvwobSkillConfig = "<%=this.lstvwobSkillConfig.ClientID %>"

            function ClearFields() {
                $get(_clientTxtSkill).value = "";
                $get(_clientTxtSortOrder).value = "";
                $get(_clienthidId).value = 0;
            }

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?');
            }

            function ValidateSkill(oSrc, args) {
                var skill = $get(_clientTxtSkill).value;
                if (skill.trim() == "") {
                    oSrc.errormessage = "Skill should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (skill.length > 200) {
                    oSrc.errormessage = "Skill length should not be greater than 200 characters.";
                    args.IsValid = false;
                    return true;
                }
                else {
                    var rowIndex = 0;
                    var isDuplicate = false;
                    var Skill = document.getElementById(_clientlstvwobSkillConfig + "_ctrl" + rowIndex + "_lblName")
                    while (Skill != null) {
                        var id = document.getElementById(_clientlstvwobSkillConfig + "_ctrl" + rowIndex + "_hidSkillId").value
                        if (Skill.innerHTML.trim() == skill.trim() && $get(_clienthidId).value != id) {
                            isDuplicate = true;
                            break;
                        }

                        rowIndex = rowIndex + 1;
                        Skill = document.getElementById(_clientlstvwobSkillConfig + "_ctrl" + rowIndex + "_lblName")
                    }
                    if (isDuplicate) {
                        oSrc.errormessage = "Skill should not be duplicate.";
                        args.IsValid = false;
                        return true;
                    }
                }
                args.IsValid = true;
                return false;
            }

            function ValidateSortOrder(oSrc, args) {
                var sortOrder = $get(_clientTxtSortOrder).value;
                if (sortOrder == "") {
                    oSrc.errormessage = "Sort Order should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else {
                    var rowIndex = 0;
                    var isDuplicate = false;
                    var lstSortOrder = document.getElementById(_clientlstvwobSkillConfig + "_ctrl" + rowIndex + "_lblSortOrder")
                    while (lstSortOrder != null) {
                        var id = document.getElementById(_clientlstvwobSkillConfig + "_ctrl" + rowIndex + "_hidSkillId").value
                        if (lstSortOrder.innerHTML.trim() == sortOrder && $get(_clienthidId).value != id) {
                            isDuplicate = true;
                            break;
                        }

                        rowIndex = rowIndex + 1;
                        lstSortOrder = document.getElementById(_clientlstvwobSkillConfig + "_ctrl" + rowIndex + "_lblSortOrder")
                    }
                    if (isDuplicate) {
                        oSrc.errormessage = "Sort Order should not be duplicate.";
                        args.IsValid = false;
                        return true;
                    }
                }
                args.IsValid = true;
                return false;
            }

            function ClearMessage() {
                $get(_clientlblMessage).innerHTML = "";
            }
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
