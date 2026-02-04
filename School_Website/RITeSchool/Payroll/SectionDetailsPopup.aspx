<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SectionDetailsPopup.aspx.cs" Inherits="SectionDetailsPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td>
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left" style="height: 20px" class="ClsGrayMainTitle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="left" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold; text-align: left;">Section Configuration</span>
                                        </td>
                                    </tr>
                                </table>
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
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left" width="50%">
                            </td>
                            <td width="50%">
                                <div style="float: right;">
                                    <span class="ClsMdtStar">* Mandatory Fields </span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trDetails" runat="server">
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="1" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" id="tdMessage" runat="server">
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Section Group :</span>
                                                </td>
                                                <td align="left" width="180px" style="white-space: nowrap">
                                                    <asp:DropDownList ID="cmbSectionGroup" runat="server" class="MidCombo" Width="200px" onchange="EnableDisableCategory()">                                                        
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:CustomValidator ID="cstSectionGroup" runat="server" ClientValidationFunction="ValidateSectionGroup"
                                                        Display="None" ErrorMessage="Section Group should be selected.">
                                                    </asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Section Name :</span>
                                                </td>
                                                <td align="left" width="180px" style="white-space: nowrap">
                                                    <asp:TextBox ID="txtSectionName" runat="server" MaxLength="30" CssClass="LrgTxtBox"
                                                        Style="width: 330px;"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:CustomValidator ID="cstSectionName" runat="server" Display="None" ErrorMessage=""
                                                        ClientValidationFunction="DuplicateSectionName">
                                                    </asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="130px" class="ClsBorderlight">
                                                    <span class="ClsLabel">Sort Order :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSortOrder" runat="server" MaxLength="2" CssClass="SmlTxtBox"
                                                        Style="width: 50px;" onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false"
                                                        onkeypress="return blockNonNumbers(this, event, false, false);" onkeyup="extractNumber(this,1,false);"
                                                        onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ErrorMessage=""
                                                        ClientValidationFunction="DuplicateSortOrder">
                                                    </asp:CustomValidator>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle" style="white-space: nowrap">
                                                    <span class="ClsLabel">Max. Amount Limit :</span>
                                                </td>
                                                <td align="left" >
                                                    <asp:TextBox ID="txtMaxAmount" runat="server" CssClass="SmlTxtBox" MaxLength="10" Enabled="false"
                                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false">0.0</asp:TextBox>                                                    
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Category :</span>
                                                </td>
                                                <td align="left" style="white-space: nowrap" >
                                                <asp:DropDownList ID="cmbCategory" runat="server" class="MidCombo" Enabled="false">
                                                    <asp:ListItem Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="A" Value="1"></asp:ListItem>                                                    
                                                    <asp:ListItem Text="B" Value="2"></asp:ListItem>                                                    
                                                </asp:DropDownList>
                                                    <span id="spanCategory" runat="server" class="ClsMdtStar" style="visibility:hidden">*</span>
                                                    <asp:CustomValidator ID="cstCategoryVal" runat="server" ClientValidationFunction="ValidateCategory"
                                                        Display="None" ErrorMessage="Category should be selected for selected Section Group.">
                                                    </asp:CustomValidator>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
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
                                <tr>
                                    <td align="center">
                                        <table align="center">
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="The Maximum Amount Limit and Category is only applicable to the sections under the section group - Deduction under Chapter VIA."></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwSections" runat="server" DataKeyNames="Id,SectionGroupId,CategoryId"
                                            OnItemCommand="lstvwSections_ItemCommand" OnItemDataBound="lstvwSections_ItemDataBound"
                                            OnSorting="lstvwSections_Sorting">
                                            <LayoutTemplate>
                                                <table width="800px" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" style="padding-left: 5px" width="170px">
                                                            <asp:LinkButton ID="lnkSectionGroup" runat="server" CommandName="Sort" CommandArgument="SectionGroupName"
                                                                CausesValidation="false" ForeColor="Black"> Section Group </asp:LinkButton>
                                                        </th>
                                                        <th align="left" class="ClsLabelL" width="200px">
                                                            <asp:LinkButton ID="lnkSectionName" runat="server" CommandName="Sort" CommandArgument="SectionName"
                                                                CausesValidation="false" ForeColor="Black"> Section Name </asp:LinkButton>
                                                        </th>
                                                        <th align="right" class="clsLabelgrd" width="100px">
                                                            <asp:LinkButton ID="lnkSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder"
                                                                CausesValidation="false" ForeColor="Black"> Sort Order </asp:LinkButton>
                                                        </th>
                                                        <th align="right" class="ClsLabelR" width="120px">
                                                            <asp:LinkButton ID="lnkMaxAmount" runat="server" CommandName="Sort" CommandArgument="MaxAmount" style="white-space:nowrap"
                                                                CausesValidation="false" ForeColor="Black"> Max. Amount Limit </asp:LinkButton>
                                                        </th>
                                                        <th width="50px" align="center">
                                                            Category
                                                        </th>
                                                        <th width="50px" align="center">
                                                            Edit
                                                        </th>
                                                        <th width="30px">
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
                                                        <asp:Label ID="lblGroupName" runat="server" CssClass="ClsLabel" Text='<%#Eval("SectionGroupName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidSectionId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="right" style="text-align:right;">
                                                        <asp:Label ID="lblSortOrder" runat="server" CssClass="ClsLabelR" Text='<%#Eval("SortOrder") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="text-align:right;">
                                                        <asp:Label ID="lblMaxAmount" runat="server" CssClass="ClsLabelR" Text='<%#Eval("MaxAmount") %>'></asp:Label>
                                                    </td>
                                                    <td align="center" style="text-align:center;">
                                                        <asp:Label ID="lblCategory" runat="server" CssClass="ClsGridRow" ></asp:Label>
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
                                                        <asp:Label ID="lblGroupName" runat="server" CssClass="ClsLabel" Text='<%#Eval("SectionGroupName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidSectionId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblSortOrder" runat="server" CssClass="ClsLabelR" Text='<%#Eval("SortOrder") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="text-align:right;">
                                                        <asp:Label ID="lblMaxAmount" runat="server" CssClass="ClsLabelR" Text='<%#Eval("MaxAmount") %>'></asp:Label>
                                                    </td>
                                                    <td align="center" style="text-align:center;">
                                                        <asp:Label ID="lblCategory" runat="server" CssClass="ClsGridRow" ></asp:Label>
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
                                        <asp:HiddenField ID="hidSectionId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCategoryFor" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="30%">
                        <tr>
                            <td align="center">
                                <asp:Button CssClass="ClsBtn" ID="btnClose" CausesValidation="false" runat="server"
                                    OnClientClick="ClosePopup()" Text="Close" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientTxtSectionName = "<%=this.txtSectionName.ClientID %>"
        _clientTxtSortOrder = "<%=this.txtSortOrder.ClientID %>"
        _clientLstvwSections = "<%=this.lstvwSections.ClientID %>"
        _clientHidSectionId = "<%=this.hidSectionId.ClientID %>"
        _clientcmbSectionGroup = "<%=this.cmbSectionGroup.ClientID %>"
        _clienthidCategoryFor = "<%=this.hidCategoryFor.ClientID %>"
        _clientcmbCategory = "<%=this.cmbCategory.ClientID %>"
        _clientspanCategory = "<%=this.spanCategory.ClientID %>"
        _clienttxtMaxAmount = "<%=this.txtMaxAmount.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler() {
            EnableDisableCategory();
        }

        function EndRequestHandler() {
            EnableDisableCategory();
        }

        function ClosePopup() {
            window.opener.location = window.opener.location.pathname;
            window.close();
            window.opener.focus();
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this section?');
        }

        function ValidateSectionGroup(aSrc, args) {
            if ($get(_clientcmbSectionGroup).value == 0)
			{
                args.IsValid = false;
				return true;
			}
            else {
                args.IsValid = true;
                return false;
            }
            
        }

        function EnableDisableCategory() {
            var Selected = $get(_clientcmbSectionGroup);
            var SelectedGroup = Selected.options[Selected.selectedIndex].value;
            var CategoryFor = $get(_clienthidCategoryFor).value;
            var control = document.getElementById(_clientspanCategory);
            var Category = $get(_clientcmbCategory);            

            if (CategoryFor == SelectedGroup) {
                
                $get(_clientcmbCategory).disabled = false;
                $get(_clienttxtMaxAmount).disabled = false;
                control.style.visibility = "visible";                
            }
            else {
                Category.value = "0";
                $get(_clienttxtMaxAmount).value = "0";
                $get(_clientcmbCategory).disabled = true;
                $get(_clienttxtMaxAmount).disabled = true;
                control.style.visibility = "hidden";                
            }                
        }

        function DuplicateSectionName(oSrc, args) {
            var sectionName = $get(_clientTxtSectionName).value
            var found = getValidationMessage(sectionName, "Section Name", "lblName");
            if (found != "") {
                oSrc.errormessage = found;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function DuplicateSortOrder(oSrc, args) {
            var sortOrder = $get(_clientTxtSortOrder).value
            var found = ValidatSortOrder(sortOrder, "Sort Order", "lblSortOrder");
            if (found != "") {
                oSrc.errormessage = found;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function getValidationMessage(obj, objName, fieldName) {
            var SectionId = $get(_clientHidSectionId).value
            var iRowIndex = 0;
            var found = false;
            if (obj.trim() == "") {
                found = true
                return objName + " should not be blank.";
            }
            else {
                var name = $get(_clientLstvwSections + "_ctrl" + iRowIndex + "_" + fieldName)
                var hidId = $get(_clientLstvwSections + "_ctrl" + iRowIndex + "_hidSectionId")
                while (name != null) {
                    if (name.innerHTML.trim().toUpperCase() == obj.trim().toUpperCase() && SectionId != hidId.value) {
                        return objName + " should not be duplicated.";
                        found = true;
                        break;
                    }
                    iRowIndex++;
                    var name = $get(_clientLstvwSections + "_ctrl" + iRowIndex + "_" + fieldName)
                    var hidId = $get(_clientLstvwSections + "_ctrl" + iRowIndex + "_hidSectionId")
                }
            }
            return "";
        }

        function ValidatSortOrder(obj, objName, fieldName) {
            var SectionId = $get(_clientHidSectionId).value
            var iRowIndex = 0;
            var found = false;
            if (obj.trim() == "") {
                found = true
                return objName + " should not be blank.";
            }
            else if (parseInt(obj.trim()) == 0) {
                found = true
                return objName + " should not be zero.";
            }
            else {
                var name = $get(_clientLstvwSections + "_ctrl" + iRowIndex + "_" + fieldName)
                var hidId = $get(_clientLstvwSections + "_ctrl" + iRowIndex + "_hidSectionId")
                while (name != null) {
                    if (parseInt(name.innerHTML.trim()) == parseInt(obj.trim()) && SectionId != hidId.value) {
                        return objName + " should not be duplicated.";
                        found = true;
                        break;
                    }
                    iRowIndex++;
                    var name = $get(_clientLstvwSections + "_ctrl" + iRowIndex + "_" + fieldName)
                    var hidId = $get(_clientLstvwSections + "_ctrl" + iRowIndex + "_hidSectionId")
                }
            }
            return "";
        }

        function SetState() {
            $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
        }

        function ValidateCategory(oSrc, args) {           
            var Selected = $get(_clientcmbSectionGroup);
            var SelectedGroup = Selected.options[Selected.selectedIndex].value;
            var CategoryFor = $get(_clienthidCategoryFor).value;
            var Category = $get(_clientcmbCategory);
            if (CategoryFor == SelectedGroup && Category.value=="0") {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
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

    </script>
</asp:Content>
