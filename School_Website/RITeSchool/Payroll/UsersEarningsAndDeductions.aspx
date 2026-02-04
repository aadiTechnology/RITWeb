<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="UsersEarningsAndDeductions.aspx.cs" Inherits="UsersEarningsAndDeductions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
	<table width="100%" style="vertical-align: top">
        <tr valign="top" style="width: 100%">
            <td class="ClsGrayMainTitle" valign="top" align="left" style="width: 100%">
                <span class="MainTitleHead">Earnings/Deductions Configuration</span>
            </td>
            <td style="height: 10px;">
            </td>
        </tr>
    </table>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 10px;" align="right">
                    <span class="ClsMdtStar">* Mandatory Fields</span>
                </td>
            </tr>
            <tr align="center" id="trValSummary" runat="server">
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">  
                        <ContentTemplate>
                        <asp:RequiredFieldValidator ID="reqPay" runat="server" Display="None" ErrorMessage="Pay Scale should be selected."
                            ControlToValidate="cmbPayScale" InitialValue="0"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cstvalReason" runat="server" Display="None" ErrorMessage="Reason should not be blank." ClientValidationFunction="ValidateReason"></asp:CustomValidator>
                        <asp:ValidationSummary ID="valSum" CssClass="LblErrorMsg" ShowSummary="true" runat="server" />
                        <asp:CustomValidator ID="cstEarningDeductions" runat="server" ClientValidationFunction="ValidateEarningsDeductions"
                                SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>                        
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateEarningsDeductions"
                                SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>                                           
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbPayScale" EventName="SelectedIndexChanged" />                            
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>            
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr id="trRole" runat="server">
                <td align="center">
                    <table width="70%">                        
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Name :</span>
                            </td>
                            <td class="ClsHilightBGB">
                                <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="ClsBorderlight">
                                <span class="ClsLabel">Pay Scale :</span>
                            </td>
                            <td align="left">                           
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbPayScale" runat="server" AutoPostBack="true"
                                                CssClass="MidCombo" OnSelectedIndexChanged="cmbPayScale_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                        <td align="left">
                                         <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">  
                                            <ContentTemplate>
                                                <asp:CheckBox ID="chkIsActive" runat="server" CssClass="clsLabel" 
                                                    Text="Mark as active pay scale?" TextAlign="Left" />
                                                     </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbPayScale" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>           
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="ClsBorderlight">
                                <span class="ClsLabel">Reason :</span>
                            </td>
                            <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">  
                                <ContentTemplate>
                                    <asp:TextBox ID="txtReason" runat="server" MaxLength="100" TextMode="MultiLine" CssClass="LrgTxtBox" Width="95%"></asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbPayScale" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="ClsBorderlight">
                                <span class="ClsLabel">Pay Matrix :</span>
                            </td>
                            <td align="left">                           
                                <asp:DropDownList ID="cmbPayMatrix" runat="server" AutoPostBack="false"
                                    CssClass="SmlCombo">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>      
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px">
                <td align="center">
                    <table align="center">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">  
                                <ContentTemplate>
                                    <asp:Label ID="lblWarningMessage" runat="server" CssClass="ClsHilightErrorB"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbPayScale" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style='display:none;'>
                <td align="center">
                    <table width="70%">
                        <tr>
                            <td align="left" colspan="2">
                                <asp:CheckBox ID="chkApplyToAll" Checked="false" runat="server" Text="Apply to all the users of this Staff Group"
                                    onclick="SetGroupName()" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trlistview" runat="server" align="center">
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">  
                        <ContentTemplate>
                            <div id="Div1" runat="server" style="width: 70%; overflow: auto;">
                        <asp:ListView ID="lstvwEarningsDeductions" runat="server" DataKeyNames="UsersEarningsDeductionsId,EarningsDeductionsId">
                            <LayoutTemplate>
                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                    cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th class="paddingLSML">
                                            Earnings/Deductions Name
                                        </th>
                                        <th align="right">
                                            Amount
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="trItem" runat="server" class="ClsGridRow">
                                    <td class="paddingLSML">
                                        <asp:Label ID="lblEarningDeductionName" runat="server" Text='<%#Eval("EarningsDeductionsName") %>'></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="MidTxtBox" Style="text-align: right;
                                            padding-right: 5px" Text='<%#Eval("EarningsDeductionsValue") %>' MaxLength="10"
                                            onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);"
                                            onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false"></asp:TextBox>
                                        <asp:HiddenField ID="hidValue" runat="server" Value='<%#Eval("EarningsDeductionsValue") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="trItem" runat="server" class="ClsGridAltRow">
                                    <td class="paddingLSML">
                                        <asp:Label ID="lblEarningDeductionName" runat="server" Text='<%#Eval("EarningsDeductionsName") %>'></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="MidTxtBox" Style="text-align: right;
                                            padding-right: 5px" Text='<%#Eval("EarningsDeductionsValue") %>' MaxLength="10"
                                            onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);"
                                            onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false"></asp:TextBox>
                                        <asp:HiddenField ID="hidValue" runat="server" Value='<%#Eval("EarningsDeductionsValue") %>' />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>                        
                    </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbPayScale" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="70%">
                        <tr>
                            <td align="left" colspan="2">
                                <asp:CheckBox ID="chkNewFormulaToAll" Checked="false" runat="server" Text="Apply to all the users of this Staff Group"
                                    onclick="SetGroupName()" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="tr1" runat="server" align="center">
                <td align="center" >                  
                        <div id="Div2" runat="server" style="width: 100%; overflow: auto;">
                        <asp:ListView ID="lstvwFormulaAndRangeED" runat="server" DataKeyNames="UsersFormulaRangeId,EarningsDeductionsId,IsFormula,FormulaRangeId"
                            OnItemDataBound="lstvwFormulaAndRangeED_ItemDataBound">
                            <LayoutTemplate>
                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                    cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th class="paddingLSML">
                                            Earnings/Deductions Name
                                        </th>
                                        <th class="paddingLSML">
                                            Default Formula/Range
                                        </th>
                                        <th class="paddingLSML">
                                            Formula/Range
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="trItem" runat="server" class="ClsGridRow">
                                    <td class="paddingLSML">
                                        <asp:Label ID="lblEarningDeductionName" runat="server" Text='<%#Eval("EarningsDeductionsName") %>'></asp:Label>
                                    </td>
                                     <td class="paddingLSML">
                                        <asp:Label ID="lblDefaultFormula" runat="server" ></asp:Label>
                                    </td>
                                    <td class="paddingLSML">
                                        <asp:DropDownList ID="ddlFormula" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hidFormula" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="trItem" runat="server" class="ClsGridAltRow">
                                    <td class="paddingLSML">
                                        <asp:Label ID="lblEarningDeductionName" runat="server" Text='<%#Eval("EarningsDeductionsName") %>'></asp:Label>
                                    </td>
                                     <td class="paddingLSML">
                                        <asp:Label ID="lblDefaultFormula" runat="server"></asp:Label>
                                    </td>
                                    <td class="paddingLSML">
                                        <asp:DropDownList ID="ddlFormula" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hidFormula" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>                        
                    </div>                    
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                    <tr>
                        <td align="left" class="ClsBorderlight " style="width: 100pxs; background-color: #ffffc4;">
                            <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                CssClass="LblNrmlB"></asp:Label>
                        </td>
                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                            <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Formula / range is not based on selected pay scale."></asp:Label>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                </td>
            </tr>
            <tr id="trButtons" runat="server" align="center">
                <td align="center">
                <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px" 
                        OnClick="BtnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="false" UseSubmitBehavior="false" />
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">  
                    <ContentTemplate>                    
                        <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidUserName" runat="server" Value="" />
                        <asp:HiddenField ID="hidStaffGroupName" runat="server" Value="" />
                        <asp:HiddenField ID="hidStaffGroupId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidRecordCount" runat="server" Value="0" />
                        <asp:HiddenField ID="hidDisplayMessage" runat="server" Value="N" />
                        <asp:HiddenField ID="HidApplyToAllUsersOfStaffGroup" runat="server" Value="N" />
                        <asp:HiddenField ID="hidIsConfigured" runat="server" Value="N" />
                        <asp:HiddenField ID="hidUserRoleId" runat="server" Value="N" />
                        <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                        <asp:HiddenField ID="hidUsersEarningsDeductionsId" runat="server"/>
                        <asp:HiddenField ID="hidActivePayScaleId" runat="server" Value="0"/>
                        <asp:HiddenField ID="hidLastSelectedPayScaleId" runat="server" Value="0"/>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbPayScale" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientsaveid = "<%=this.BtnSave.ClientID %>"
        _clientbtncancelid = "<%=this.btnCancel.ClientID %>"
        _clientlstvwearningsdeductions = "<%=this.lstvwEarningsDeductions.ClientID %>"
        _clientcstEarningDeductions = "<%=this.cstEarningDeductions.ClientID %>"
        _clientvalsum = "<%=this.valSum.ClientID %>"
        _clientHidApplyToAllUsersOfStaffGroup = "<%=this.HidApplyToAllUsersOfStaffGroup.ClientID %>"
        _clienthidRecordCount = "<%=this.hidRecordCount.ClientID %>"
        
        
        _clienttxtReason = "<%=this.txtReason.ClientID %>"
        _clientlstvwFormulaAndRangeED = "<%=this.lstvwFormulaAndRangeED.ClientID %>"
        _clientcmbPayScale = "<%=this.cmbPayScale.ClientID %>"
        _clienthidLastSelectedPayScaleId = "<%=this.hidLastSelectedPayScaleId.ClientID %>"

        function DisableButtons() {       
            if (document.getElementById(_clientsaveid) != null)
                document.getElementById(_clientsaveid).disabled = true

            if (document.getElementById(_clientbtncancelid) != null)
                document.getElementById(_clientbtncancelid).disabled = true         
        }
        function ValidateEarningsDeductions(aSrc, args) {
            var txt
            var iRowCount = 0
            var sEmptyMessage = ""
            txt = document.getElementById(_clientlstvwearningsdeductions + "_ctrl" + iRowCount + "_txtValue")
            while (txt != null) {
                if (txt.value.trim() == "") {
                    var lblName = document.getElementById(_clientlstvwearningsdeductions + "_ctrl" + iRowCount + "_lblEarningDeductionName").innerHTML
                    sEmptyMessage = sEmptyMessage + "," + lblName
                }
                iRowCount = iRowCount + 1
                txt = document.getElementById(_clientlstvwearningsdeductions + "_ctrl" + iRowCount + "_txtValue")
            }
            if (sEmptyMessage != "") {
                sEmptyMessage = sEmptyMessage.substring(1)
                $get(_clientcstEarningDeductions).errormessage = "Value of earnings and deductions should not be empty. Name(s) : " + sEmptyMessage
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function SetGroupName() {        
                $get("<%=this.lblUserName.ClientID %>").innerHTML = $get("<%=this.hidUserName.ClientID %>").value
        }
        
        function ValidateReason(oSrc, args) {
            var IsValid = true;
            var cstReason = document.getElementById("<%=this.cstvalReason.ClientID %>");
            var IsNew = $get("<%=this.hidUsersEarningsDeductionsId.ClientID %>").value;
            if (IsNew != "0") {
                if (document.getElementById(_clienttxtReason).value.trim() == "") {
                    cstReason.errormessage = "Reason should not be blank.";
                    cstReason.innerHTML = "Reason should not be blank.";
                    IsValid = false;
                }
                else if (document.getElementById(_clienttxtReason).value.length > 100) {
                    cstReason.errormessage = "Reason length should not be greater than 100.";
                    cstReason.innerHTML = "Reason length should not be greater than 100.";
                    IsValid = false;
                }
            }
            args.IsValid = IsValid;
            return !IsValid;
        }

        function ConfirmChange() {
            if (confirm('This action will load details of selected pay scale. Do you want to continue?'))
                return true
            else {
                $('#' + _clientcmbPayScale).val($('#'+_clienthidLastSelectedPayScaleId).val())
                return false
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
