<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GuestDetailsUI.aspx.cs" Inherits="GuestDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            <tr>
                <td align="right" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>"
                        runat="server" ValidationGroup="Save" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <tr>
                        <td align="center" id="tdMessage" runat="server" colspan="2">
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <table width="80%" align="center">
                        <tr>
                            <td>
                                <table align="center" cellpadding="1" cellspacing="2">
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                            <asp:Label CssClass="ClsLabel" ID="lblName" runat="server" Text="Name"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                            <asp:Label CssClass="LblSmlGray floatR" ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsMdtStar" style="width: 50%">
                                            <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" TabIndex="1">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="30" Width="186px" onblur="formatName(this)"
                                                CssClass="MidTxtBox" TabIndex="2"></asp:TextBox>
                                            *&nbsp;
                                            <asp:RequiredFieldValidator ID="rqdFirstName" runat="server" ControlToValidate="txtFirstName"
                                                ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, FirstNameValidation%>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                            <asp:Label CssClass="LblSmlGray floatR" ID="lblMiddleName" runat="server" Text="Middle Initial"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsMdtStar">
                                            <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="1"
                                                onblur="formatName(this)" Width="50px" TabIndex="3"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                            <asp:Label CssClass="LblSmlGray floatR" ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsMdtStar">
                                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" onblur="formatName(this)"
                                                CssClass="MidTxtBox" TabIndex="4"></asp:TextBox>
                                            *<asp:RequiredFieldValidator ID="rqdLastName" runat="server" ControlToValidate="txtLastName"
                                                ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValLastNameBlank%>"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 42%; height: 24px">
                                            <asp:Label CssClass="ClsLabel" ID="lblArea" runat="server" Text="Area"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" style="width: 72%; height: 24px;">
                                            <asp:TextBox ID="txtArea" runat="server" CssClass="ExLrgTxtBox" MaxLength="30" TabIndex="5"></asp:TextBox>
                                            <span class="ClsMdtStar">*</span>
                                            <asp:RequiredFieldValidator ID="ReqArea" runat="server" ControlToValidate="txtArea"
                                                ValidationGroup="Save" Display="None" ErrorMessage="Area should not be blank."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                            <asp:Label CssClass="ClsLabel" ID="lblMobileNumber" runat="server" EnableViewState="False"
                                                Text="Mobile Number"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" class="ClsMdtStar">
                                            <asp:TextBox ID="txtMobileNo" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                ondrop="event.returnValue=false" TabIndex="5" />&nbsp;*
                                            <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                                ValidationGroup="Save" Visible="true" ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                            <asp:Label CssClass="ClsLabel" ID="lblIsReference" Height="16px" runat="server" Text="Is Reference?"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td id="Td22" runat="server" align="left">
                                            <asp:CheckBox ID="chkIsReference" runat="server" TabIndex="6" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                            <asp:Label CssClass="ClsLabel" ID="lblDesignation" runat="server" EnableViewState="False"
                                                Text="Reference Guest Name"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" style="width: 72%; height: 24px;">
                                            <asp:DropDownList ID="cmbReferenceGuestName" runat="server" CssClass="MidCombo" Style="width: 190px;"
                                                TabIndex="10">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar" id="spnrgn" style="display: none">*</span>
                                            <asp:CustomValidator ID="cstReferenceGuestName" runat="server" ValidationGroup="Save"
                                                ClientValidationFunction="ReferenceGuestNameValidation"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                            <asp:Label CssClass="ClsLabel" ID="lblSendSMS" Height="16px" runat="server" Text="Send SMS?"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td id="Td1" runat="server" align="left">
                                            <asp:CheckBox ID="chkSendSMS" runat="server" TabIndex="6" />
                                        </td>
                                    </tr>
                                </table>
                                <tr>
                                    <td align="center" colspan="2" style="height: 32px">
                                        <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="Save" TabIndex="7"
                                            OnClick="btnSave_Click" ValidationGroup="Save"></asp:Button>&nbsp;
                                        <asp:Button CssClass="ClsBtn" ID="BtnCancel" runat="server" Text="Cancel" TabIndex="8"
                                            OnClick="BtnCancel_Click" CausesValidation="false"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr id="trItemCount" runat="server">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwGuestDetails"
                                                        Visible="true">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                                        Text="<%# Container.StartRowIndex + 1%>" />
                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                        Text=" To " />
                                                                    <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                        Text=" Out Of " />
                                                                    <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                        Text="Records " />
                                                                    <br />
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ListView ID="lstvwGuestDetails" runat="server" DataKeyNames="GuestId,IsSendSMS"
                                                        OnItemCommand="lstvwGuestDetails_ItemCommand" OnItemDataBound="lstvwGuestDetails_ItemDataBound"
                                                        OnDataBound="lstvwGuestDetails_DataBound">
                                                        <LayoutTemplate>
                                                            <table cellpadding="0" cellspacing="1" class="GridBorder" width="100%" style="color: #333333"
                                                                align="center" class="GridBorder">
                                                                <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" class="paddingL" style="width: 25%; font-size: 9pt;">
                                                                        <asp:LinkButton ID="lnkbtnName" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            CommandArgument="FullName" CommandName="SortRow">Name</asp:LinkButton>
                                                                    </th>
                                                                    <th align="left" class="paddingL" style="width: 20%; font-size: 9pt;">
                                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            CommandArgument="Area" CommandName="SortRow">Area</asp:LinkButton>
                                                                    </th>
                                                                    <th align="right" id="th1" runat="server" style="width: 12%; font-size: 9pt; padding-right:8px">
                                                                        <asp:Label ID="lblMobile" runat="server" Text="Mobile Number"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="paddingL" runat="server" style="width: 25%; font-size: 9pt;">
                                                                        <asp:Label ID="lblReference" runat="server" Text="Reference Name"></asp:Label>
                                                                    </th>
                                                                    <th id="Th2" align="left" class="paddingL" runat="server" style="width: 7%; font-size: 9pt;">
                                                                        <asp:Label ID="Label1" runat="server" Text="Sent SMS ?"></asp:Label>
                                                                    </th>
                                                                    <th align="center" width="4%">
                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"></asp:Label>
                                                                    </th>
                                                                    <th align="center" width="4%">
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                    <td colspan="7">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwGuestDetails">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td align="right" class="LblNormal">
                                                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </PagerTemplate>
                                                                                </asp:TemplatePagerField>
                                                                            </Fields>
                                                                        </asp:DataPager>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblArea" runat="server" Text='<%# Eval("Area") %>'></asp:Label>
                                                                </td>
                                                                <td align="right" style="padding-right:8px">
                                                                    <asp:Label ID="lblMobileNumber" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblReferenceName" runat="server" Text='<%# Eval("ReferenceGuestFullName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="ImgBtn1" runat="server" CausesValidation="false" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" ImageUrl="../images/IconGrid_Delete.gif" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="trRow" runat="server" class="ClsGridAltRow">
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblArea" runat="server" Text='<%# Eval("Area") %>'></asp:Label>
                                                                </td>
                                                                <td align="right" style="padding-right:8px">
                                                                    <asp:Label ID="lblMobileNumber" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblReferenceName" runat="server" Text='<%# Eval("ReferenceGuestFullName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="ImgBtn1" runat="server" CausesValidation="false" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" ImageUrl="../images/IconGrid_Delete.gif" />
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <table width="70%" align="center">
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                        No record found.
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                    <asp:HiddenField ID="hidGuestId" runat="server" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.Survey.GuestDetailsBL" EnablePaging="true"
                                                        ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                                        EnableCaching="false">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="int32" />
                                                            <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" PropertyName="Value" />
                                                            <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" PropertyName="Value" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:HiddenField ID="HidTestTypeId" runat="server" />
                                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" 
                                    TabIndex="9" onclick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        _clienttxtMobileNo = "<%=this.txtMobileNo.ClientID %>";
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>";
        _clientchkIsReference = "<%=this.chkIsReference.ClientID %>";
        _clientcmbReferenceGuestName = "<%=this.cmbReferenceGuestName.ClientID %>";
        _clientcstReferenceGuestNameValidator = "<%=this.cstReferenceGuestName.ClientID %>";

        $(document).ready(function () {
            var chk = $get(_clientchkIsReference)
            SetFieldState(chk.checked);
        })

        function SetFieldStatus(obj) {
            SetFieldState(obj.checked)
        }

        function SetFieldState(Status) {
            var cmbReferenceGuestName = $get(_clientcmbReferenceGuestName)
            if (Status == false) {
                cmbReferenceGuestName.disabled = true;
                cmbReferenceGuestName.value = "";
                $('#spnrgn').hide()
            }
            else {
                cmbReferenceGuestName.disabled = false
                $('#spnrgn').show()
            }
        }

        function ReferenceGuestNameValidation(aSrc, args) {
            var e = document.getElementById(_clientcmbReferenceGuestName);
            if ($get(_clientchkIsReference).checked) {
                var str = e.options[e.selectedIndex].text;
                if (str == "-- Select --") {
                    document.getElementById(_clientcstReferenceGuestNameValidator).errormessage = "Reference name should be selected."
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }


        function MobileNumberValidation(oSrc, args) {
            var sMobileNumber = document.getElementById(_clienttxtMobileNo).value;
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber);
            document.getElementById(_clientcst_MobileNumber).errormessage = "";
            if (sMobileNumber.length == 0) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile number should not be blank.";
                args.IsValid = false;
                return true;
            }
            if (sMobileNumber.length < 10) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile Number should be of 10 digits.";
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile No. should not start with zero.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
