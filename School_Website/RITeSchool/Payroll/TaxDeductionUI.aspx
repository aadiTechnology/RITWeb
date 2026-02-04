<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TaxDeductionUI.aspx.cs" Inherits="TaxDeductionUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="80%">
            <tr id="trInvestmentDetails" runat="server">
                <td>
                    <asp:UpdatePanel ID="upnl21" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                    </td>
                                    <td align="left" width="150px">
                                        <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                    </td>
                                </tr>
                            </table>
                            <table width="65%" align="center">
                                <tr id="trPublishMessage" runat="server" visible="false">
                                    <td align="center" width="100%" class="ClsHilightBGB">
                                        <span class="LblNrmlB" style="border-width: 0px; font-weight: bold;">Income tax details
                                            of this financial year has been published.</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" id="tdMessage" runat="server" width="50%">
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="700px" style="white-space:nowrap">
                                            <tr id="trUser" runat="server">
                                                <td align="right" valign="middle" class="ClsBorderlight">
                                                    <span class="ClsLabel">Staff Group :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStaffGroups" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="10%">
                                                </td>
                                                <td align="left" class="ClsBorderlight" id="tdUser" runat="server">
                                                    <span class="ClsLabel">User :</span>
                                                </td>
                                                <td id="tdCmbUser" runat="server">
                                                    <asp:UpdatePanel ID="upnl1" runat="server" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbUser" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cmbUser_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqcmbUser" runat="server" Display="None" ControlToValidate="cmbUser"
                                                                CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="User should be selected."></asp:RequiredFieldValidator>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroups" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="70%">
                                            <tr>
                                                <td align="right" width="50%" class="ClsBorderlight">
                                                    <span class="ClsLabel">Quarter Name :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbQuarter" runat="server" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqcmbQuarter" runat="server" Display="None" ControlToValidate="cmbQuarter"
                                                        CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="Quarter Name should be selected."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle">
                                                    <span class="ClsLabel">Tax Deduction Amount :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTaxDeductAmt" runat="server" CssClass="SmlTxtBox" MaxLength="10"
                                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqtxtTaxDeductAmt" runat="server" Display="None"
                                                        ControlToValidate="txtTaxDeductAmt" CssClass="ClsMdtStar" ErrorMessage="Tax Deduction Amount should not be blank."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle">
                                                    <span class="ClsLabel">Tax Deposited / Remitted Amount :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDeposited" runat="server" CssClass="SmlTxtBox" MaxLength="10"
                                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqtxtDeposited" runat="server" Display="None" ControlToValidate="txtDeposited"
                                                        CssClass="ClsMdtStar" ErrorMessage="Tax Deposited / Remitted Amount should not be blank."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <table width="30%">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button CssClass="ClsBtn" ID="BtnSave" runat="server" Text="Save" disable-page="true"
                                                                    OnClick="BtnSave_Click" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:Button CssClass="ClsBtn" ID="BtnCancel" CausesValidation="false" runat="server"
                                                                    Text="Cancel" OnClick="BtnCancel_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>                               
                                <tr>
                                    <td align="center" height="11px">
                                        <asp:CustomValidator ID="cstvalidateTDS" runat="server" Display="None" ErrorMessage=""
                                            SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidateTDS"></asp:CustomValidator>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="right">
                                        <table>
                                            <tr>                                                          
                                                <td align="right" class="ClsGreenBG" style="white-space:nowrap" >
                                                    <asp:LinkButton ID="lnkCITDetails" runat="server" Text="Income Tax Configuration"
                                                        CssClass="SubTitle" Style="text-align: left;"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="upnl2" runat="server" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                <asp:ListView ID="lstvwTaxDeduction" runat="server" DataKeyNames="Id" OnItemCommand="lstvwTaxDeduction_ItemCommand"
                                                    OnItemDataBound="lstvwTaxDeduction_ItemDataBound" OnSorting="lstvwTaxDeduction_Sorting">
                                                    <LayoutTemplate>
                                                        <table width="730px" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                <th align="left" width="150px" class="clsLabelgrd">
                                                                    <asp:LinkButton ID="lnkQuarterName" runat="server" CommandName="Sort" CommandArgument="QuarterName"
                                                                        CausesValidation="false" ForeColor="Black"> Quarter Name </asp:LinkButton>
                                                                </th>
                                                                <th align="right" class="ClsLabelR" width="150px">
                                                                    <asp:LinkButton ID="lnkDeductedAmount" runat="server" CommandName="Sort" CommandArgument="TaxDeductionAmount"
                                                                        CausesValidation="false" ForeColor="Black"> Tax Deducted Amount </asp:LinkButton>
                                                                </th>
                                                                <th align="right" class="clsLabelgrd" width="230px">
                                                                    <asp:LinkButton ID="lnkTaxRemittedAmt" runat="server" CommandName="Sort" CommandArgument="TaxDepositedAmount"
                                                                        CausesValidation="false" ForeColor="Black"> Tax Deposited / Remitted Amount </asp:LinkButton>
                                                                </th>
                                                                <th align="center" class="clsLabelgrd" width="70px">
                                                                    Edit
                                                                </th>
                                                                <th align="center" class="clsLabelgrd" width="70px">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="center">
                                                                <asp:Label ID="lblQuarterName" runat="server" CssClass="ClsLabel" Text='<%#Eval("QuarterName") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblDeductedAmt" CssClass="ClsLabelR" Style="text-align: right; padding-right: 5px"
                                                                    runat="server" Text='<%#Eval("TaxDeductionAmount") %>'></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblDepositedAmt" runat="server" CssClass="ClsLabelR" Style="text-align: right;
                                                                    padding-right: 5px" Text='<%#Eval("TaxDepositedAmount") %>'></asp:Label>
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
                                                            <td align="center">
                                                                <asp:Label ID="lblQuarterName" runat="server" CssClass="ClsLabel" Text='<%#Eval("QuarterName") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblDeductedAmt" runat="server" CssClass="ClsLabelR" Style="text-align: right;
                                                                    padding-right: 5px" Text='<%#Eval("TaxDeductionAmount") %>'></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblDepositedAmt" runat="server" CssClass="ClsLabelR" Style="text-align: right;
                                                                    padding-right: 5px" Text='<%#Eval("TaxDepositedAmount") %>'></asp:Label>
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
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbUser" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="cmbStaffGroups" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:HiddenField ID="hidTaxDeductionId" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />                                        
                                        <asp:HiddenField ID="hidStaffGroupsId" runat="server" Value="" />
                                        <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                        <asp:HiddenField ID="hidIsPublished" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divErr" runat="server">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="30%">
                        <tr>
                            <td align="center">
                                <asp:Button CssClass="ClsBtn" ID="btnBack" CausesValidation="false" runat="server"
                                    Text="Back" OnClick="btnBack_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clienttxtTaxDeductAmt = "<%=this.txtTaxDeductAmt.ClientID %>";
        _clienttxtDeposited = "<%=this.txtDeposited.ClientID %>";
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        _clientcstvalidateTDS = "<%=this.cstvalidateTDS.ClientID %>";
        _clientcmbQuarter = "<%=this.cmbQuarter.ClientID %>";
        _clienthidTaxDeductionId = "<%=this.hidTaxDeductionId.ClientID %>";
        _clientlstvwTaxDeduction = "<%=this.lstvwTaxDeduction.ClientID %>";

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this Tax Deduction details for this quarter?');
        }

        function SetState() {
            $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
        }

        function CheckValue(obj) {
            if (obj.value.trim() == "")
                obj.value = "0"
            else {
                var floatValue = parseFloat(obj.value)
                var intValue = parseInt(obj.value)

                if (floatValue < 1)
                    intValue = 0

                intValue = parseFloat(intValue)
                var difference = parseFloat((floatValue * 10) % 10)
                if (difference != 5 && difference != 0) {
                    if (difference > 5)
                        difference = intValue + 1
                    else
                        difference = intValue + 0.5

                    obj.value = difference
                }
            }
        }

        function ValidateTDS(oSrc, args) {

                var rowIndex = 0;
                var found = false;
                var SavedTDS = null;
                if ($get(_clientlstvwTaxDeduction + "_ctrl" + rowIndex + "_lblQuarterName")!=null)
                    SavedTDS = $get(_clientlstvwTaxDeduction + "_ctrl" + rowIndex + "_lblQuarterName").innerHTML;
                var TaxDeductionId = $get(_clienthidTaxDeductionId).value;
                var Selected = $get(_clientcmbQuarter);
                var SelectedTDS = Selected.options[Selected.selectedIndex].innerHTML;

                while (SavedTDS != null) {

                    if ($get(_clientlstvwTaxDeduction + "_ctrl" + rowIndex + "_hidId") != null) {
                        var SavedId = $get(_clientlstvwTaxDeduction + "_ctrl" + rowIndex + "_hidId").value;
                        if (TaxDeductionId != SavedId) {
                            if (SelectedTDS == SavedTDS) {
                                found = true;
                                break;
                            }
                        }

                        rowIndex++;
                        if ($get(_clientlstvwTaxDeduction + "_ctrl" + rowIndex + "_lblQuarterName") != null)
                            var SavedTDS = $get(_clientlstvwTaxDeduction + "_ctrl" + rowIndex + "_lblQuarterName").innerHTML;
                    }
                    else
                        SavedTDS = null;
                }
                if (found) {
                    oSrc.errormessage = "Tax Deduction details are already saved for selected Quarter.";
                    args.IsValid = false;
                    return true;
                }

            args.IsValid = true;
            return false;

        }

        function OpenPopup() {
            window.open('TaxDeductionConfigPopup.aspx?', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=700')
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
