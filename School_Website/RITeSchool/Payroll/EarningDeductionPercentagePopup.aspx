<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="EarningDeductionPercentagePopup.aspx.cs" Inherits="EarningDeductionPercentagePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align:top;">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;vertical-align:top">
                    <tr>
                        <td class="ClsGrayMainTitle" align="left" style="width:99%;">
                            <asp:Label ID="Label2" runat="server" CssClass="MainTitleHead" Text="<%$ Resources:LocalizedResources, PaymentCategories%>"></asp:Label>
                        </td>
                    </tr>                   
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:UpdatePanel ID="upnl3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="valSum" runat="server" />
                                                <asp:RequiredFieldValidator ID="reqValName" runat="server" Display="None" ControlToValidate="txtName"
                                                    ErrorMessage="<%$ Resources:LocalizedResources, valCategoryName%>"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ClientValidationFunction="DuplicateName"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstValPercentage" runat="server" Display="None" ClientValidationFunction="ValidatePercentage"></asp:CustomValidator>                                                
                                            </ContentTemplate>
                                            <Triggers>
                                                 <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                 <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                 <asp:AsyncPostBackTrigger ControlID="lstvwCategory" EventName="ItemCommand" />
                                                 <asp:AsyncPostBackTrigger ControlID="lstvwCategory" EventName="Sorting" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="right" valign="top">
                                        <span class="ClsMdtStar">* </span>
                                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr id="trMessage" runat="server" visible="false">
                                            <td align="center" id="tdMessage" runat="server">
                                                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                                    Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" width="150px">                                                            
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Name%>"></asp:Label>
                                                            <span class="colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtName" runat="server" CssClass="LrgTxtBox" MaxLength="50"></asp:TextBox>
                                                            <span class="ClsMdtStar">* </span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px;">
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:ListView ID="lstvwEarningsDeductions" runat="server" DataKeyNames="Id,EarningDeductionId,EarnDeduct"
                                                    OnItemDataBound="lstvwEarningsDeductions_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table width="45%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th class="paddingLSML" width="80%">
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, EarningDeduction%>"></asp:Label>
                                                                </th>
                                                                <th align="right" style="padding-right: 5px;" width="20%">
                                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Percentage%>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trItem" runat="server" class="ClsGridRow">
                                                            <td class="paddingLSML">
                                                                <asp:Label ID="lblShortName" runat="server" Text='<%#Eval("EarnDeduct.ShortName") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="padding-right: 5px;">
                                                                <asp:TextBox ID="txtPercentage" runat="server" CssClass="SmlTxtBox" Style="text-align: right;"
                                                                    Text='<%#Eval("Percentage") %>' MaxLength="5" onblur="extractNumber(this,1,false);"
                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                            <td class="paddingLSML">
                                                                <asp:Label ID="lblShortName" runat="server" Text='<%#Eval("EarnDeduct.ShortName") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="padding-right: 5px;">
                                                                <asp:TextBox ID="txtPercentage" runat="server" CssClass="SmlTxtBox" Style="text-align: right;"
                                                                    Text='<%#Eval("Percentage") %>' MaxLength="5" onblur="extractNumber(this,1,false);"
                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                                <asp:HiddenField ID="hidCategoryId" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwCategory" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" CausesValidation="false" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="90%">
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstvwCategory" runat="server" DataKeyNames="Id" OnItemCommand="lstvwCategory_ItemCommand"
                                                    OnSorting="lstvwCategory_Sorting" OnItemDataBound="lstvwCategory_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th class="paddingLSML" width="75%">                                                                    
                                                                    <asp:LinkButton ID="lnkCategoryName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                    CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, Name%>"></asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="13%">
                                                                   <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                                </th>
                                                                <th align="center" width="12%">
                                                                    <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trItem" runat="server" class="ClsGridRow">
                                                            <td class="paddingLSML">
                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
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
                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                            <td class="paddingLSML">
                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
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
                                                                <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                <asp:HiddenField ID="hidSortDirection" runat="server" />                                               
                                                <asp:HiddenField ID="hidmsgConfirmDelete" runat="server"/>   
                                                <asp:HiddenField ID="hiddupvalCategoryName" runat="server"/>   
                                                <asp:HiddenField ID="hidvalPercentage" runat="server"/>   
                                                <asp:HiddenField ID="hidvalCategoryName" runat="server"/>                                                
                                                <asp:HiddenField ID="hidUpdateUserData" runat="server" Value="N"/>                                                
                                                <asp:HiddenField ID="hidUpdateConfirmMsg" runat="server"/>                                                 
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwCategory" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close%>" CausesValidation="false"
                                CssClass="ClsBtn" OnClientClick="ClosePopup()" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clientlstvwEarningsDeductions = "<%=this.lstvwEarningsDeductions.ClientID %>"
        _clienttxtName = "<%=this.txtName.ClientID %>"
        _clienthidCategoryId = "<%=this.hidCategoryId.ClientID %>"
        _clientlstvwCategory = "<%=this.lstvwCategory.ClientID %>"
        _clienthidUpdateUserData = "<%=this.hidUpdateUserData.ClientID %>"

        function ClosePopup() {
            window.close();
        }

        function ConfirmDelete() {
            return window.confirm($get("<%=this.hidmsgConfirmDelete.ClientID %>").value);
        }

        function ValidatePercentage(oSrc, args) {    
            var rowNumber = 0
            var isFound = false;
            var name = document.getElementById(_clientlstvwEarningsDeductions + "_ctrl" + rowNumber + "_lblShortName")
            while (name != null) {
                var txt = document.getElementById(_clientlstvwEarningsDeductions + "_ctrl" + rowNumber + "_txtPercentage")

                if (txt.value.trim() != "" && parseInt(txt.value.trim()) != 0) {
                    isFound = true;
                    break;
                }
                rowNumber++;
                name = document.getElementById(_clientlstvwEarningsDeductions + "_ctrl" + rowNumber + "_lblShortName")
            }

            if (!isFound) {
                oSrc.errormessage = $get("<%=this.hidvalPercentage.ClientID %>").value;
            }

            args.IsValid = isFound;
            return !isFound;
        }

        function DuplicateName(oSrc, args) {            
            var rowNumber = 0
            var isFound = false;
            var newName = $get(_clienttxtName).value;
            var newId = $get(_clienthidCategoryId).value;

            var name = document.getElementById(_clientlstvwCategory + "_ctrl" + rowNumber + "_lblName")
            while (name != null) {
                var hidId = document.getElementById(_clientlstvwCategory + "_ctrl" + rowNumber + "_hidId")

                if (parseInt(hidId.value) != parseInt(newId) && newName.trim().toLowerCase() == name.innerHTML.trim().toLowerCase()) {
                    isFound = true;
                    break;
                }
               
                rowNumber++;
                name = document.getElementById(_clientlstvwCategory + "_ctrl" + rowNumber + "_lblName")
            }

            if (isFound) {
                oSrc.errormessage = $get("<%=this.hiddupvalCategoryName.ClientID %>").value;
            }

            args.IsValid = !isFound;
            return isFound;
        }

        function ClosePopup() {
            window.opener.UpdateGrid();
            window.close();
            window.opener.focus();
        }

        function ShowConfirmationMessage() {
            if (document.getElementById("<%=this.trMessage.ClientID %>") != null)
                document.getElementById("<%=this.trMessage.ClientID %>").style.display = "none";

            if ($get(_clienthidCategoryId).value != "0" && $get(_clienthidCategoryId).value != "") {
                if (confirm($get("<%=this.hidUpdateConfirmMsg.ClientID %>").value))
                    $get(_clienthidUpdateUserData).value = "Y";
                else
                    $get(_clienthidUpdateUserData).value = "N";
            }
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
