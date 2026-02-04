<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PaymentGroupUI.aspx.cs" Inherits="PaymentGroupUI " %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
     <style>
        .clsLabel, .ClsLabel {
            font-family: open sans;
        }    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="left" width="50%">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwGroups" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td width="50%">
                                            <div style="float: right;">
                                                <span class="ClsMdtStar">*</span>
                                                    <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnlFields" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="center" id="tdMessage" runat="server" colspan="2">
                                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                                        ForeColor="Blue" Style="text-align: center"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="100px" class="ClsBorderlight">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Name%>" CssClass="ClsLabel"></asp:Label>
                                                    <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="LrgTxtBox" MaxLength="50"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqValName" runat="server" ControlToValidate="txtName"
                                                        ErrorMessage="<%$ Resources:LocalizedResources, ValBlankName%>" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateGroupName"
                                                        SetFocusOnError="True" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, valDuplicateName%>"></asp:CustomValidator>
                                                </td>
                                            </tr>                                           
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwGroups" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="35%">
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:ListView ID="lstvwParameters" runat="server" DataKeyNames="EarningsDeductionsId,IsEarning"
                                                        OnItemDataBound="lstvwParameters_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th align="left" style="padding-left: 5px">
                                                                         <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, EarningDeductionName%>"></asp:Label>
                                                                    </th>
                                                                    <th align="right" style="padding-right: 7px;" width="150px">
                                                                         <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Amount%>"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblEDName" runat="server" CssClass="ClsLabel" Text='<%#Eval("EarningsDeductionsName") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidIsEarning" runat="server" Value='<%#Eval("IsEarning") %>' />
                                                                </td>
                                                                <td align="right" style="padding-right: 5px;">
                                                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="MidTxtBox" Style="text-align: right;
                                                                        padding-right: 2px;" onblur="extractNumber(this,1,true);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers(this, event, false, true);" onkeyup="extractNumber(this,1,true);"
                                                                        onpaste="event.returnValue=false" MaxLength="9"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblEDName" runat="server" CssClass="ClsLabel" Text='<%#Eval("EarningsDeductionsName") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidIsEarning" runat="server" Value='<%#Eval("IsEarning") %>' />
                                                                </td>
                                                                <td align="right" style="padding-right: 5px;">
                                                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="MidTxtBox" Style="text-align: right;
                                                                        padding-right: 2px;" onblur="extractNumber(this,1,true);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers(this, event, false, true);" onkeyup="extractNumber(this,1,true);"
                                                                        onpaste="event.returnValue=false" MaxLength="9"></asp:TextBox>
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
                                                    <asp:CustomValidator ID="cstValParameter" runat="server" ClientValidationFunction="ValidateParameterValue"
                                                        SetFocusOnError="True" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValAmountForEarnDeduct%>"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="ClsBorderlight" style="float:right;">
                                                    <asp:Label ID="lblGrossSalaryHeader" runat="server" Font-Bold="true" CssClass="ClsLabel"
                                                        Text="<%$ Resources:LocalizedResources, GrossSalary%>"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td width="150px" class="ClsBorderlight" align="right">
                                                    <asp:Label ID="lblGrossSalary" runat="server" Font-Bold="true" CssClass="ClsLabel"
                                                        Style="float: right; padding-right: 10px;" Text="0"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwGroups" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td colspan="2" align="center">
                                             <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" OnClick="btnCancel_Click" CausesValidation="false" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwGroups" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:UpdatePanel ID="upnlListview" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <table width="50%">
                                            <tr>
                                                <td>
                                                    <asp:ListView ID="lstvwGroups" runat="server" DataKeyNames="Id" OnItemCommand="lstvwGroups_ItemCommand"
                                                        OnItemDataBound="lstvwGroups_ItemDataBound" OnSorting="lstvwGroups_Sorting">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th align="left" style="padding-left: 5px">
                                                                        <asp:LinkButton ID="lnkSectionGroup" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                            CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, Name%>"></asp:LinkButton>
                                                                    </th>
                                                                    <th width="30px" align="center" class="clsLabelC">
                                                                       <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>" CssClass="ClsLabel"></asp:Label>
                                                                    </th>
                                                                    <th width="50px" class="clsLabelC">
                                                                       <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>" CssClass="ClsLabel"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidGroupId" runat="server" Value='<%#Eval("Id") %>' />
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
                                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidGroupId" runat="server" Value='<%#Eval("Id") %>' />
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
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwGroups" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>" CausesValidation="false" CssClass="ClsBtn" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hidPaymentGroupId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />                                        
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwGroups" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>           
                    <asp:HiddenField ID="hidmsgConfirmDelete" runat="server" Value="" />                                           
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            _clientlstvwParameters = "<%=this.lstvwParameters.ClientID %>";
            _clientlstvwGroups = "<%=this.lstvwGroups.ClientID %>";

            function ShowConfirmation() {
                return confirm($get("<%=this.hidmsgConfirmDelete.ClientID %>").value);
            }

            function ValidateParameterValue(oSrc, args) {
                var isFound = false;
                var rowNumber = 0;
                var txt = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_txtAmount")

                while (txt != null) {

                    if (txt.value.trim() != "" && parseInt(txt.value.trim()) > 0) {
                        isFound = true;
                        break;
                    }

                    rowNumber++;
                    txt = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_txtAmount")
                }

                args.IsValid = isFound;
                return !isFound;
            }

            function OpenPopup() {
                window.open('SalaryParameterPopup.aspx?', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=615,height=500')
                return false;
            }

            function UpdateGrossSalary() {
                var grossAmount = 0;
                var isFound = false;
                var rowNumber = 0;
                var txt = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_txtAmount")

                while (txt != null) {
                    txt.value = txt.value.trim();
                    if (txt.value != "" && parseInt(txt.value) > 0) {
                        var hid = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_hidIsEarning")
                        if (hid.value == "True")
                            grossAmount = grossAmount + parseInt(txt.value)
                        else
                            grossAmount = grossAmount - parseInt(txt.value)
                    }

                    rowNumber++;
                    txt = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_txtAmount")
                }

                $get("<%=this.lblGrossSalary.ClientID %>").innerHTML = grossAmount;

            }

            function ValidateGroupName(oSrc, args) {
                var grossAmount = 0;
                var isFound = false;
                var rowNumber = 0;

                var selectedGroupId = document.getElementById("<%=this.hidPaymentGroupId.ClientID %>").value;
                var newName = document.getElementById("<%=this.txtName.ClientID %>").value;
                var Name = document.getElementById(_clientlstvwGroups + "_ctrl" + rowNumber + "_lblName")

                while (Name != null) {

                    var GroupId = document.getElementById(_clientlstvwGroups + "_ctrl" + rowNumber + "_hidGroupId").value;
                    if (selectedGroupId != GroupId && Name.innerHTML.toLowerCase() == newName.toLowerCase()) {
                        isFound = true;
                        break;
                    }

                    rowNumber++;
                    Name = document.getElementById(_clientlstvwGroups + "_ctrl" + rowNumber + "_lblName")
                }

                args.IsValid = !isFound;
                return isFound;
            }

            function ResetMessage() {
                $get("<%=this.lblMessage.ClientID %>").innerHTML = '';
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
