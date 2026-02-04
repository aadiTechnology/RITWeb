<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PerformanceParametersUI.aspx.cs" Inherits="PerformanceParametersUI" %>

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
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsMdtStar" />
                                        <asp:CustomValidator ID="cstvalParameter" runat="server" ClientValidationFunction="ValidateParameter"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValSortOrder" runat="server" ClientValidationFunction="ValidateSortOrder"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstvalRole" runat="server" ClientValidationFunction="IsFormTypeSelected"
                                            Display="None" ErrorMessage="Select Form Type." SetFocusOnError="True">
                                        </asp:CustomValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
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
                         <asp:AsyncPostBackTrigger ControlID="lstvwParameter" EventName="ItemCommand" />
                              <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbSkill" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbFormType" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                         <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                         <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                         <asp:AsyncPostBackTrigger ControlID="btnUnSubmit" EventName="Click" />

                        </Triggers>
                        </asp:UpdatePanel>

                </td>
            </tr>
            <tr id="trPreCondition" runat="server" visible="false">
                <td>
                    <table width="100%" id="tblPreCondition">
                        <tr>
                            <td align="left">
                                <div runat="server" id="divErr">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnBackUp" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trControls" runat="server">
                <td align="center">
                    <table width="75%">
                        <tr>
                            <td colspan="4" align="center">
                                <table width="80%">
                                    <tr>
                                        <td class="ClsBorderlight">
                                            <span class="ClsLabel">Year : </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbYear" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbYear_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ClsBorderlight">
                                            <span class="ClsLabel">Skill : </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbSkill" runat="server" CssClass="ExLrgCombo" OnSelectedIndexChanged="cmbSkill_SelectedIndexChanged"
                                                Width="300px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ClsBorderlight">
                                            <span class="ClsLabel">Form Type : </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbFormType" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                                OnSelectedIndexChanged="cmbFormType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr style="border-style: solid; border-width: thin; color: Silver;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="90%">
                                            <tr>
                                                <td class="ClsBorderlight" width="150px">
                                                    <span class="ClsLabel">Parameter : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParameter" runat="server" CssClass="ExLrgTxtBox" Style="width: 95%"
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
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" OnClick="btnCancel_Click"
                                                        CausesValidation="false" />
                                                     <asp:HiddenField ID="hidParameterId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                                </td>
                                            </tr>
                                            <%--   <tr>
                                        <td class="ClsBorderlight" width="150px">
                                            <span class="ClsLabel">Form Type : </span>
                                            </td>
                                        <td>
                                           <asp:DropDownList ID="cmbFormType" runat="server" CssClass="MidCombo" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                             <span class="ClsMdtStar">*</span>
                                            </td>
                                    </tr>--%>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstvwParameter" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                      <%-- <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" OnClick="btnCancel_Click"
                                    CausesValidation="false" />
                            </td>
                        </tr>--%>
                        <tr style="display: none;">
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Form Type : </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbFilterFormType" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbFilterFormType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <table width="100%">
                                            <tr>
                                                <td align="center">
                                        <asp:ListView ID="lstvwParameter" runat="server" DataKeyNames="Id, AppraisalFormTypeId"
                                            OnItemCommand="lstvwParameter_ItemCommand" OnItemDataBound="lstvwParameter_ItemDataBound"
                                            OnSorting="lstvwParameter_Sorting">
                                            <LayoutTemplate>
                                                <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" class="clsLabelgrd">
                                                            <asp:LinkButton ID="lnkTitle" runat="server" CommandName="Sort" CommandArgument="Title"
                                                                CausesValidation="false" ForeColor="Black"> Parameter </asp:LinkButton>
                                                        </th>
                                                        <th align="right" class="clsLabelgrd" width="100px">
                                                            <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="SortOrder"
                                                                CausesValidation="false" ForeColor="Black"> Sort Order </asp:LinkButton>
                                                        </th>
                                                        <th align="left" class="clsLabelgrd" runat="server" visible="false">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Title"
                                                                CausesValidation="false" ForeColor="Black"> Form Type </asp:LinkButton>
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
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="ClsLabel" Text='<%#Eval("Title") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblSortOrder" runat="server" Text='<%#Eval("SortOrder") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" runat="server" visible="false">
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("FormType") %>'></asp:Label>
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
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="ClsLabel" Text='<%#Eval("Title") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblSortOrder" runat="server" Text='<%#Eval("SortOrder") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" runat="server" visible="false">
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("FormType") %>'></asp:Label>
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
                                        </td>
                                      </tr>
                                    </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbSkill" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbFormType" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwParameter" EventName="ItemCommand" />
                                             <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                         <asp:AsyncPostBackTrigger ControlID="btnUnSubmit" EventName="Click" />


                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                     
                            <td colspan="4" align="center">
                              <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                              <ContentTemplate>
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" OnClick="btnSubmit_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnUnSubmit" runat="server" Text="un Submit" CssClass="ClsBtn" OnClick="btnUnSubmit_Click"
                                    CausesValidation="false" />   
                                    </ContentTemplate>
                                    <Triggers>
                                     <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                         <asp:AsyncPostBackTrigger ControlID="btnUnSubmit" EventName="Click" />
                                         <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                    </asp:UpdatePanel>  
                                                              
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">

            _clientTxtParameter = "<%=this.txtParameter.ClientID %>";
            _clientTxtSortOrder = "<%=this.txtSortOrder.ClientID %>";
            _clienthidParameterId = "<%=this.hidParameterId.ClientID %>";
            _clientlblMessage = "<%=this.lblMessage.ClientID %>";
            _clientlstvwParameter = "<%=this.lstvwParameter.ClientID %>"
            _clientcmbUsers = "<%=this.cmbFilterFormType.ClientID %>";
            _clientcmbUsers = "<%=this.cmbFormType.ClientID %>"

            function ClearFields() {
                $get(_clientTxtParameter).value = "";
                $get(_clientTxtSortOrder).value = "";
                $get(_clienthidParameterId).value = 0;
            }

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?');
            }

            function ValidateParameter(oSrc, args) {
                var parameter = $get(_clientTxtParameter).value;
                if (parameter.trim() == "") {
                    oSrc.errormessage = "Parameter should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (parameter.length > 300) {
                    oSrc.errormessage = "Parameter length should not be greater than 300 characters.";
                    args.IsValid = false;
                    return true;
                }
                else {
                    var rowIndex = 0;
                    var isDuplicate = false;
                    var title = document.getElementById(_clientlstvwParameter + "_ctrl" + rowIndex + "_lblTitle")
                    while (title != null) {
                        var id = document.getElementById(_clientlstvwParameter + "_ctrl" + rowIndex + "_hidId").value
                        if (title.innerHTML.trim() == parameter.trim() && $get(_clienthidParameterId).value != id) {
                            isDuplicate = true;
                            break;
                        }

                        rowIndex = rowIndex + 1;
                        title = document.getElementById(_clientlstvwParameter + "_ctrl" + rowIndex + "_lblTitle")
                    }
                    if (isDuplicate) {
                        oSrc.errormessage = "Parameter should not be duplicate.";
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
                    var lstSortOrder = document.getElementById(_clientlstvwParameter + "_ctrl" + rowIndex + "_lblSortOrder")
                    while (lstSortOrder != null) {
                        var id = document.getElementById(_clientlstvwParameter + "_ctrl" + rowIndex + "_hidId").value
                        if (lstSortOrder.innerHTML.trim() == sortOrder && $get(_clienthidParameterId).value != id) {
                            isDuplicate = true;
                            break;
                        }

                        rowIndex = rowIndex + 1;
                        lstSortOrder = document.getElementById(_clientlstvwParameter + "_ctrl" + rowIndex + "_lblSortOrder")
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



            function IsFormTypeSelected(oSrc, args) {
                var UserSelected = true;
                var UserId = document.getElementById(_clientcmbUsers)
                if (UserId != null && (UserId.value == "-- Select --" || UserId.value == "0"))
                    UserSelected = false;

                if (!UserSelected) {
                    oSrc.errormessage = "Form type should be selected.";
                    args.IsValid = false;
                    return false;
                }
                args.IsValid = true;
                return true
            }


        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
