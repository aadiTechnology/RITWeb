<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="InvestmentMethodUI.aspx.cs" Inherits="InvestmentMethodUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr id="trInvestmentDetails" runat="server">
                <td>
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="50%">
                                            <tr>
                                                <td align="center" id="tdMessage" runat="server" colspan="2">
                                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                                        Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="190px">
                                                    <span class="ClsLabel">Section :</span>
                                                </td>
                                                <td width="520px" style="white-space:nowrap">
                                                    <asp:DropDownList ID="cmbSection" runat="server" CssClass="ExLrgCombo" AutoPostBack="true"
                                                        Style="width: 400px;" OnSelectedIndexChanged="cmbSection_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqCmbSection" runat="server" Display="None" ControlToValidate="cmbSection"
                                                        CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="Section should be selected."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle">
                                                    <span class="ClsLabel">Investment / Income Method :</span>
                                                </td>
                                                <td style="white-space:nowrap">
                                                    <asp:TextBox ID="txtMethod" runat="server" TextMode="MultiLine" Rows="3" CssClass="LrgTxtBox"
                                                        Width="400px"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ErrorMessage=""
                                                        CssClass="ClsMdtStar" ClientValidationFunction="ValidateInvestmentMethod">
                                                    </asp:CustomValidator>
                                                </td>
                                            </tr>                                          
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle" style="white-space: nowrap">
                                                    <span class="ClsLabel">Associated Earning / Deduction : </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbEarningDeduction" runat="server" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                    <asp:CustomValidator ID="cstvalidateED" runat="server" Display="None" ErrorMessage=""
                                                        SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidateED">
                                                    </asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" style="white-space: nowrap" 
                                                    valign="middle">
                                                   <span class="ClsLabel">Max. Amount Limit : </span>
                                                    </td>
                                                <td>
                                                    <asp:TextBox ID="txtMaxLimit" runat="server" CssClass="MidTxtBox" MaxLength="7"
                                                                onchange="EnableButton(_clienttxtFormulaValue, _clientbtnAddFormulaValue)" Style="width: 20%;
                                                                text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" ></asp:TextBox><span
                                                    class="LblSmlGray">
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" valign="middle" style="white-space: nowrap">
                                                    <span class="ClsLabel">Apply to All Users :</span>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkApplyToUsers" runat="server" CssClass="ClsLabel" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <table width="30%">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button CssClass="ClsBtn" ID="BtnSave" runat="server" Text="Save"  disable-page="true"
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
                                    <td align="center">
                                        <table width="80%">
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left" class="ClsGreenBG" width="150px">
                                                    <asp:LinkButton ID="lnkSectionDetails" runat="server" Text="Section Configuraton"
                                                        CssClass="SubTitle" Style="text-align: left;"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwMethods" runat="server" DataKeyNames="Id,AssociatedEarnDeductId"
                                            OnItemCommand="lstvwMethods_ItemCommand" OnItemDataBound="lstvwMethods_ItemDataBound"
                                            OnSorting="lstvwMethods_Sorting">
                                            <LayoutTemplate>
                                                <table width="80%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" width="150px" class="clsLabelgrd">
                                                            <asp:LinkButton ID="lnkSectionName" runat="server" CommandName="Sort" CommandArgument="SectionName"
                                                                CausesValidation="false" ForeColor="Black"> Section Name </asp:LinkButton>
                                                        </th>
                                                        <th align="left" class="clsLabelgrd">
                                                            <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="MethodName"
                                                                CausesValidation="false" ForeColor="Black"> Name </asp:LinkButton>
                                                        </th>                                                        
                                                        <th align="left" width="220px" style="padding-left:5px" >
                                                            Associated Earning / Deduction 
                                                        </th>
                                                        <th align="right" width="140px" style="padding-right:5px" >
                                                            Max. Amount Limit 
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
                                                        <asp:Label ID="lblSection" runat="server" CssClass="ClsLabel" Text='<%#Eval("SectionName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblMethod" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                    </td>                                                   
                                                    <td align="left">
                                                        <asp:Label ID="lblEarningDeduction" runat="server" CssClass="ClsLabelL" Text='<%#Eval("AssociatedEarnDeductName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidEarningDeduction" runat="server" Value='<%#Eval("AssociatedEarnDeductId") %>' />
                                                    </td>
                                                    <td align="right" style="padding-right:1px">
                                                        <asp:Label ID="lblMaxLimit" runat="server" CssClass="ClsLabelR" Text='<%#Eval("MaxLimit") %>'></asp:Label>
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
                                                        <asp:Label ID="lblSection" runat="server" CssClass="ClsLabel" Text='<%#Eval("SectionName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblMethod" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                    </td>                                                   
                                                    <td align="left">
                                                        <asp:Label ID="lblEarningDeduction" runat="server" CssClass="ClsLabelL" Text='<%#Eval("AssociatedEarnDeductName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidEarningDeduction" runat="server" Value='<%#Eval("AssociatedEarnDeductId") %>' />
                                                    </td>
                                                     <td align="right" style="padding-right:1px">
                                                        <asp:Label ID="lblMaxLimit" runat="server" CssClass="ClsLabelR" Text='<%#Eval("MaxLimit") %>'></asp:Label>
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
                                        <asp:ObjectDataSource TypeName="BusinessLogic.InvestmentMethodBL" EnablePaging="True"
                                            ID="objdsInvestmentMethods" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                            SelectCountMethod="Count" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiFinYearId" SessionField="S_FINANCIAL_YEAR_ID" Type="int32" />                                               
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                <asp:ControlParameter Name="sortExpression" Type="String" ControlID="hidSortExpression" PropertyName="Value" />
                                                <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection" PropertyName="Value" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr id="trNote" runat="server">
                                    <td align="center">
                                        <table id="tblNote" runat="server" align="center" width="80%">
                                            <tr id="trFullAccess" runat="server">
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <span class="LblNrmlB">Note1 :</span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <span class="LblSmlV">The investment / income method with a section belongs to the 'Gross Salary' and 'Other Income' section groups is not allowed to be associated with an earning or a deduction.</span>
                                                </td>
                                            </tr>
                                            <tr id="tr1" runat="server">
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <span class="LblNrmlB">Note2 :</span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <span class="LblSmlV">If  'Apply to All Users' option is selected for a investment method, then it will add / update investment declaration amount of selected investment method with total amount earned / deducted under selected earning / deduction in selected financial year. </span>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hidInvestmentMethodId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                        <asp:HiddenField ID="hidAssociatedEDId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsConfirmed" runat="server" />                                        
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

        _clientCmbSectionName = "<%=this.cmbSection.ClientID %>";
        _clientTxtMethod = "<%=this.txtMethod.ClientID %>";
        _clientHidInvestmentMethodId = "<%=this.hidInvestmentMethodId.ClientID %>";        
        _clientcstvalidateED = "<%=this.cstvalidateED.ClientID %>";
        _clientcmbEarningDeduction = "<%=this.cmbEarningDeduction.ClientID %>";
        _clientlstvwMethods = "<%=this.lstvwMethods.ClientID %>";
        _clientchkApplyToUsers = "<%=this.chkApplyToUsers.ClientID %>";
        _clientcmbSection = "<%=this.cmbSection.ClientID %>";
        _clienthidAssociatedEDId = "<%=this.hidAssociatedEDId.ClientID %>";
        _clienthidIsConfirmed = "<%=this.hidIsConfirmed.ClientID %>";
        
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        function EndRequestHandler(Sender, args) {
            SetState();
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this investment method?');
        }

        function ValidateInvestmentMethod(oSrc, args) {
            var iRowIndex = 0;
            var found = false;
            obj = $get(_clientTxtMethod).value
            var hidId = $get(_clientHidInvestmentMethodId).value;
            obj = obj.trim()

            if (obj == "") {
                found = true
                oSrc.errormessage = "Investment Method should not be blank."
            }
            else if (obj.length > 300) {
                found = true
                oSrc.errormessage = "Investment Method length should not be greater than 300."
            }

            if (found) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function OpenPopup() {
            window.open('SectionDetailsPopup.aspx?', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=915,height=580')
            return false;
        }

        function SetMessageState() {
            $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
        }

        //This function is used to validate of selected fee types.
        function ValidateED(aSrc, args) {
            var rowIndex = 0;
            var found = false;
            var SavedED = $get(_clientlstvwMethods + "_ctrl" + rowIndex + "_hidEarningDeduction");

            var MethodId = $get(_clientHidInvestmentMethodId).value;
            var SelectedED = $get(_clientcmbEarningDeduction).value;

            while (SavedED != null) {

                var SavedId = $get(_clientlstvwMethods + "_ctrl" + rowIndex + "_hidId").value;
                if (MethodId != SavedId) {
                    if (SelectedED != "0" && SelectedED == SavedED.value) {
                        found = true;
                        break;
                    }
                }

                rowIndex++;
                var SavedED = $get(_clientlstvwMethods + "_ctrl" + rowIndex + "_hidEarningDeduction");
            }
            if (found) {
                aSrc.errormessage = "Selected Earning / Deduction is already associated with Section.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false; ;
        }      

        function SetState() {
            var value = $get(_clientcmbEarningDeduction).value;
            
            if (value == 0) {
                $get(_clientchkApplyToUsers).disabled = true;
                $get(_clientchkApplyToUsers).checked = false;            
            }
            else {
                $get(_clientchkApplyToUsers).disabled = false;                
            }
        }
        SetState();

        var Page_IsValid = true;
        function DisplayConfirmation() {
            SetMessageState();
            Page_IsValid = true;
            var SelectedED = $get(_clientcmbEarningDeduction).value;
            var OldED = $get(_clienthidAssociatedEDId).value;

            var bResult = true;
            if (typeof (Page_ClientValidate) == 'function') {
                bResult = Page_ClientValidate()
            }
            
            if (bResult && $get(_clientchkApplyToUsers).checked) {
            	if (!confirm('This action will update investment declarations of all the users. Are you sure you want to continue?')) {
            		Page_IsValid = false;
            		return false;
            	}
            	else
            		return true;
            }
            
            if (bResult && OldED !== "0" && SelectedED == "0" && SelectedED != OldED) {
                if (confirm('You have removed the associated earning/deduction for this investment/income method. The amount is already set as per the associated earning/deduction which may have been updated for individual staff. Do you want to reset the amount to zero?') == true)
                    $get(_clienthidIsConfirmed).value = 'Y';
                else
                    $get(_clienthidIsConfirmed).value = 'N';

                return true;
            }

            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
