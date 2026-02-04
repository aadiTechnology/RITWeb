<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="EarningAndDeductionFormula.aspx.cs" Inherits="EarningAndDeductionFormulaUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%" style="vertical-align: top">
        <tr valign="top">
            <td class="ClsGrayMainTitle" valign="top" align="left">
                <span class="MainTitleHead">Earnings / Deductions Formula</span>
            </td>
            <td style="height: 10px;">
            </td>
        </tr>
    </table>
    <div class="MainBodyDiv">
        <table align="left" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnl2" runat="server">
                        <ContentTemplate>
                            <table align="left" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="height: 10px;">
                                    </td>
                                </tr>
                                <tr align="center" id="trValSummary">
                                    <td align="center">
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:ValidationSummary ID="valSummary" CssClass="LblErrorMsg" ValidationGroup="FormulaAndRange"
                                                        runat="server" />
                                                    <asp:ValidationSummary ID="valSummaryMonthwise" CssClass="LblErrorMsg" ValidationGroup="MonthwiseAmount"
                                                        runat="server" />
                                                    <asp:ValidationSummary ID="valSumAmountRange" CssClass="LblErrorMsg" ValidationGroup="grpAmountSave"
                                                        runat="server" />
                                                    <asp:ValidationSummary ID="valSumFormulaValue" CssClass="LblErrorMsg" ValidationGroup="grptxtFormulaValue"
                                                        runat="server" />
                                                    <asp:CustomValidator ID="cstMonthwiseAmount" runat="server" ClientValidationFunction="ValidateMonthwiseAmount"
                                                        ValidationGroup="MonthwiseAmount" SetFocusOnError="True" Display="None"></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="reqFormulaName" runat="server" ControlToValidate="txtFormulaName"
                                                        ValidationGroup="FormulaAndRange" ErrorMessage="Formula name should not be blank."
                                                        SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="reqFormulaValue" runat="server" ControlToValidate="txtFormulaValue"
                                                        ValidationGroup="grptxtFormulaValue" ErrorMessage="Formula value should not be blank."
                                                        SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="reqFromula" runat="server" ControlToValidate="txtFormula"
                                                        ValidationGroup="FormulaAndRange" ErrorMessage="Formula should not be blank."
                                                        SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="right">
                                                    <span class="ClsMdtStar">* Mandatory Fields </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trMessage" runat="server">
                                    <td align="center">
                                        <asp:Label ID="lblError" runat="server" EnableViewState="false" Text="" CssClass="ClsMdtStar"></asp:Label>
                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Blue" Text=""
                                            EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trFormulaField" runat="server">
                                    <td align="center" style="height: 15px">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" class="ClsBorderlight" width="100px">
                                                    <span class="ClsLblLgnd" style="font-family: Arial; font-size: 9pt;">Formula Field :</span>
                                                </td>
                                                <td align="left" class="ClsHilightBGB">
                                                    <asp:Label ID="lblFormulaField" runat="server" CssClass="ClsLabel" Style="font-family: Arial;
                                                        font-size: 9pt" Text=" D.A." Font-Bold="True" Height="16px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <span class="ClsLabel">First formula/range will be default formula/range.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label2" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note2 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <span class="ClsLabel">If respective Earning/Deduction is already associated with Staff
                                                        Group - Earning and Deduction, then you can not delete all formulae/ranges.</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trOptionButtons" runat="server">
                                    <td align="center">
                                        <table width="100%" align="center">
                                            <tr id="trAlertMessage" runat="server" visible="false">
                                                <td align="left" width="100%" class="ClsHilightBGB" colspan="4">
                                                    <span class="LblNrmlB" style="border-width: 0px; font-weight: bold;">
                                                    The selected earning/deduction is already in use. Once formula/range is added, previous<br>&nbsp values cannot be regained. </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <table>
                                                        <tr>
                                                            <td align="center" width="50%">
                                                                <asp:RadioButton ID="optFormula" runat="server" Text="Formula" Style="font-family: Arial;
                                                                    font-size: 9pt" GroupName="FormulaRange" AutoPostBack="true" OnCheckedChanged="optFormula_CheckedChanged" />
                                                            </td>
                                                            <td align="center" width="50%">
                                                                <asp:RadioButton ID="optRange" runat="server" GroupName="FormulaRange" Text="Range"
                                                                    Style="font-family: Arial; font-size: 9pt" AutoPostBack="true" OnCheckedChanged="optRange_CheckedChanged" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Formula/Range Name :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFormulaName" runat="server" MaxLength="50" CssClass="LrgTxtBox"></asp:TextBox>
                                                    <span style="color: Red">*</span>
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkIsDefault" runat="server" Text="Is Default?" CssClass="ClsLabel" />
                                                </td>
                                            </tr>                                            
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trFormula" runat="server">
                                    <td align="center">
                                        <asp:UpdatePanel ID="upnlFormula" runat="server">
                                            <ContentTemplate>
                                                <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                                    style="width: 80%;">
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" style="width: 25%">
                                                            <span class="ClsLabel">Name :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar" style="width: 70%">
                                                            <asp:DropDownList ID="cmbEarningDeductions" runat="server" CssClass="MidCombo" Style="width: 98%"
                                                                AutoPostBack="true" OnSelectedIndexChanged="cmbEarningDeductions_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="ClsBtn" ID="btnAddEarningDeduction" runat="server" Text="Add Name"
                                                                Enabled="false" Style="width: 90px;" BorderWidth="1px" UseSubmitBehavior="false"
                                                                OnClick="btnAddEarningDeduction_Click"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" style="width: 16%">
                                                            <span class="ClsLabel">Operator :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:DropDownList ID="cmbOperators" runat="server" CssClass="MidCombo" Style="width: 98%"
                                                                AutoPostBack="true" OnSelectedIndexChanged="cmbOperators_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                                <asp:ListItem>(</asp:ListItem>
                                                                <asp:ListItem>)</asp:ListItem>
                                                                <asp:ListItem>+</asp:ListItem>
                                                                <asp:ListItem>-</asp:ListItem>
                                                                <asp:ListItem>*</asp:ListItem>
                                                                <asp:ListItem>/</asp:ListItem>
                                                                <asp:ListItem>%</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="ClsBtn" ID="btnAddOperator" runat="server" Text="Add Operator"
                                                                Enabled="false" BorderWidth="1px" Style="width: 90px;" UseSubmitBehavior="false"
                                                                OnClick="btnAddOperator_Click"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" style="width: 16%">
                                                            <span class="ClsLabel">Value :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtFormulaValue" runat="server" CssClass="MidTxtBox" MaxLength="5"
                                                                onchange="EnableButton(_clienttxtFormulaValue, _clientbtnAddFormulaValue)" Style="width: 95%;
                                                                text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="ClsBtn" ID="btnAddFormulaValue" runat="server" Text="Add Value"
                                                                ValidationGroup="grptxtFormulaValue" BorderWidth="1px" Style="width: 90px;" UseSubmitBehavior="false"
                                                                OnClick="btnAddConstant_Click"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" style="width: 16%">
                                                            <span class="ClsLabel">Formula :</span>
                                                        </td>
                                                        <td id="Td1" align="left" style="color: red;" valign="middle" colspan="1">
                                                            <asp:TextBox ID="txtFormula" runat="server" MaxLength="20" CssClass="ExLrgTxtBox"
                                                                ReadOnly="true" Style="height: 100px; width: 98%; background-color: Transparent"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                            <asp:Button ID="btnRollBackFormula" runat="server" Text="Undo" UseSubmitBehavior="true"
                                                                CssClass="ClsBtn" BorderWidth="1px" Style="width: 90px;" OnClick="btnRollBackFormula_Click" />
                                                            <asp:HiddenField ID="hidFormulaValue" runat="server" Value="" />
                                                            <asp:HiddenField ID="hidFormula" runat="server" Value="" />
                                                            <asp:HiddenField ID="hidFormulaId" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hidChildIds" runat="server" Value="0" />
                                                        </td>
                                                        <td align="left" valign="middle">
                                                            <span style="color: Red">*</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="trRange" runat="server">
                                    <td>
                                        <table id="tblRange" runat="server" border="0" cellpadding="1" cellspacing="2" style="width: 100%;">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:ListView ID="lstvwAmountRange" runat="server" DataKeyNames="AmountRangeId,RangeId"
                                                        OnItemDataBound="lstvwAmountRange_ItemDataBound" OnItemCommand="lstvwAmountRange_ItemCommand">
                                                        <LayoutTemplate>
                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center">
                                                                        <asp:CheckBox ID="ChkAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                                    </th>
                                                                    <th align="center">
                                                                        From Amount
                                                                    </th>
                                                                    <th align="left" style="padding-left: 10px;">
                                                                        Upto Amount
                                                                    </th>
                                                                    <th align="left" style="padding-left: 10px;">
                                                                        Amount
                                                                    </th>
                                                                    <th align="left" style="padding-left: 10px;">
                                                                        Details
                                                                    </th>
                                                                    <th align="left" style="padding-left: 10px;">
                                                                        Save
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center">
                                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtFromAmount" runat="server" Style="text-align: right; padding-right: 5px"
                                                                        MaxLength="9" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers (this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                        onpaste="event.returnValue=false" Text='<%#Eval("FromAmount") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:TextBox ID="txtUptoAmount" runat="server" Style="text-align: right; padding-right: 5px"
                                                                        MaxLength="9" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers (this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                        onpaste="event.returnValue=false" Text='<%#Eval("UptoAmount") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:TextBox ID="txtAmount" runat="server" Style="text-align: right; padding-right: 5px"
                                                                        MaxLength="9" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers (this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                        onpaste="event.returnValue=false" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:LinkButton ID="lnkBtnDetails" runat="server" Text="Details" CommandName="DETAILS"></asp:LinkButton>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="ClsBtn" ID="BtnSaveRange" runat="server" Text="Save" BorderWidth="1px"
                                                                        ValidationGroup="grpAmountSave" CommandName="SAVE"></asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr id="trlstvwRange" runat="server" visible="false">
                                                                <td id="tdlstvwRange" runat="server" align="center" colspan="4">
                                                                    <table width="70%">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:ListView ID="lstvwRange" runat="server" DataKeyNames="MonthID,MonthwiseAmountId">
                                                                                    <LayoutTemplate>
                                                                                        <table width="100%" runat="server" id="tblRange" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" width="40%" style="padding-left: 10px;">
                                                                                                    Month
                                                                                                </th>
                                                                                                <th align="left" style="padding-left: 10px;">
                                                                                                    Amount
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="7" Text='<%#Eval("Amount") %>'
                                                                                                    onblur="extractNumber(this,2,false);" Style="text-align: right; padding-right: 5px"
                                                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="7" onblur="extractNumber(this,2,false);"
                                                                                                    Style="text-align: right; padding-right: 5px" ondrop="event.returnValue=false"
                                                                                                    onkeypress="return blockNonNumbers (this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                                                    onpaste="event.returnValue=false" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                </asp:ListView>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:Button CssClass="ClsBtn" ID="BtnSaveMonthAmount" runat="server" Text="Save"
                                                                                    ValidationGroup="MonthwiseAmount" BorderWidth="1px" UseSubmitBehavior="false"
                                                                                    OnClick="BtnSaveMonthAmount_Click"></asp:Button>
                                                                                <asp:Button CssClass="ClsBtn" ID="BtnCancelMonthAmount" CausesValidation="false"
                                                                                    runat="server" Text="Cancel" BorderWidth="1px" OnClick="BtnCancelMonthAmount_Click">
                                                                                </asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="center">
                                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtFromAmount" runat="server" MaxLength="9" onblur="extractNumber(this,2,false);"
                                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" Style="text-align: right;
                                                                        padding-right: 5px" Text='<%#Eval("FromAmount") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:TextBox ID="txtUptoAmount" runat="server" MaxLength="9" onblur="extractNumber(this,2,false);"
                                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" Style="text-align: right;
                                                                        padding-right: 5px" Text='<%#Eval("UptoAmount") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:TextBox ID="txtAmount" runat="server" MaxLength="9" onblur="extractNumber(this,2,false);"
                                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" Style="text-align: right;
                                                                        padding-right: 5px" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:LinkButton ID="lnkBtnDetails" runat="server" Text="Details" CommandName="DETAILS"></asp:LinkButton>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="ClsBtn" ID="BtnSaveRange" runat="server" Text="Save" BorderWidth="1px"
                                                                        CommandName="SAVE"></asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr id="trlstvwRange" runat="server" visible="false">
                                                                <td id="tdlstvwRange" runat="server" align="center" colspan="4">
                                                                    <table width="60%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ListView ID="lstvwRange" runat="server" DataKeyNames="MonthID,MonthwiseAmountId">
                                                                                    <LayoutTemplate>
                                                                                        <table width="100%" runat="server" id="tblRange" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th width="40%" align="left" style="padding-left: 10px;">
                                                                                                    Month
                                                                                                </th>
                                                                                                <th align="left" style="padding-left: 10px;">
                                                                                                    Amount
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="7" Text='<%#Eval("Amount") %>'
                                                                                                    onblur="extractNumber(this,2,false);" Style="text-align: right; padding-right: 5px"
                                                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("Month") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="7" Text='<%#Eval("Amount") %>'
                                                                                                    onblur="extractNumber(this,2,false);" Style="text-align: right; padding-right: 5px"
                                                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                </asp:ListView>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:Button CssClass="ClsBtn" ID="BtnSaveMonthAmount" runat="server" Text="Save"
                                                                                    ValidationGroup="MonthwiseAmount" BorderWidth="1px" UseSubmitBehavior="false"
                                                                                    OnClick="BtnSaveMonthAmount_Click"></asp:Button>
                                                                                <asp:Button CssClass="ClsBtn" ID="BtnCancelMonthAmount" CausesValidation="false"
                                                                                    runat="server" Text="Cancel" BorderWidth="1px" OnClick="BtnCancelMonthAmount_Click">
                                                                                </asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="padding-top: 5px">
                                        <asp:Button CssClass="ClsBtn" ID="BtnSave" runat="server" Text="Save" BorderWidth="1px" disable-page="true"
                                            ValidationGroup="FormulaAndRange" OnClick="BtnSave_Click"></asp:Button>
                                        <asp:Button CssClass="ClsBtn" ID="btnClear" CausesValidation="false" runat="server"
                                            UseSubmitBehavior="false" Text="Clear" BorderWidth="1px" OnClick="btnClear_Click">
                                        </asp:Button>
                                        <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                            UseSubmitBehavior="false" Text="Cancel" BorderWidth="1px" OnClick="btnCancel_Click">
                                        </asp:Button>
                                        <asp:HiddenField ID="hidEarningsDeductionsId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidMode" runat="server" Value="" />
                                        <asp:HiddenField ID="hidAmountRangeId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidUpdateMonthwiseAmount" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidUptoAmount" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsDefault" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidOldRangeName" runat="server" Value="" />
                                        <asp:HiddenField ID="hidRangeId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidDefaultRange" runat="server" Value="Y" />
                                    </td>
                                </tr>
                                <tr style="height: 20px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trLegend" runat="server">
                                    <td align="left">
                                        <table id="tblLegent" runat="server">
                                            <tr>
                                                <td width="60px">
                                                    <asp:Label ID="lblLegend" runat="server" CssClass="ClsLblLgnd" Text="Legend : "></asp:Label>
                                                </td>
                                                <td width="20px">
                                                    <asp:Label ID="lblDefaultNoticeColor" runat="server" BackColor="LightSkyBlue" Height="20px"
                                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                        EnableViewState="False"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" CssClass="ClsLblLgnd" Text="Default Formula/Range"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListView ID="lstvwFormula" runat="server" DataKeyNames="IsDefault" OnItemDataBound="lstvwFormula_ItemDataBound"
                                            OnItemCommand="lstvwFormula_ItemCommand">
                                            <LayoutTemplate>
                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333;" cellpadding="0"
                                                    cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" style="padding-left: 5px;">
                                                            Formula/Range Name
                                                        </th>
                                                        <th align="left" style="padding-left: 5px;">
                                                            Formula/Range
                                                        </th>
                                                        <th>
                                                            Edit
                                                        </th>
                                                        <th>
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td>
                                                        <asp:Label ID="lblFormulaName" runat="server" CssClass="ClsLabel" Text='<%#Eval("FormulaName") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("FormulaValue") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument='<%#Eval("FormulaId") %>' ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgbtndelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument='<%#Eval("FormulaId") %>' ImageUrl="../images/IconGrid_Delete.GIF" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                    <td>
                                                        <asp:Label ID="lblFormulaName" runat="server" CssClass="ClsLabel" Text='<%#Eval("FormulaName") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("FormulaValue") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument='<%#Eval("FormulaId") %>' ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgbtndelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument='<%#Eval("FormulaId") %>' ImageUrl="../images/IconGrid_Delete.GIF" />
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
                                <tr style="height: 10px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="upnlDeleterButton" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button CssClass="ClsBtn" ID="btnDeleteFormula" CausesValidation="false" runat="server"
                                                    ValidationGroup="grpDelete" UseSubmitBehavior="false" Text="Delete All" BorderWidth="1px"
                                                    OnClick="btnDeleteFormula_Click"></asp:Button>
                                                <asp:Button CssClass="ClsBtn" ID="BtnClose" CausesValidation="false" runat="server"
                                                    Text="Close" BorderWidth="1px"></asp:Button>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="optFormula" EventName="CheckedChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="optRange" EventName="CheckedChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clientlstvwAmountRange = "<%=this.lstvwAmountRange.ClientID %>"
        _ClientChkAll = _clientlstvwAmountRange + "_ChkAll";
        _clientcstMonthwiseAmount = "<%=this.cstMonthwiseAmount.ClientID %>"
        _clientvalSummaryMonthwise = "<%=this.valSummaryMonthwise.ClientID %>"
        _clientBtnSave = "<%=this.BtnSave.ClientID %>"
        _clientbtnDeleteFormula = "<%=this.btnDeleteFormula.ClientID %>"
        _clientBtnClose = "<%=this.BtnClose.ClientID %>"
        _clientOptFormula = "<%=this.optFormula.ClientID %>"
        _clientoptRange = "<%=this.optRange.ClientID %>"
        _clienttxtFormula = "<%=this.txtFormula.ClientID %>"
        _clienthidMode = "<%=this.hidMode.ClientID %>"
        _clienthidUptoAmount = "<%=this.hidUptoAmount.ClientID %>"
        _clienthidUpdateMonthwiseAmount = "<%=this.hidUpdateMonthwiseAmount.ClientID %>"
        _clienttxtFormulaValue = "<%=this.txtFormulaValue.ClientID %>"
        _clientbtnAddFormulaValue = "<%=this.btnAddFormulaValue.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clienttrMessage = "<%=this.trMessage.ClientID %>"
        _clienttxtFormulaName = "<%=this.txtFormulaName.ClientID %>"
        _clientbtnClear = "<%=this.btnClear.ClientID %>"
        _clientbtnDeleteFormula = "<%=this.btnDeleteFormula.ClientID %>"
        _clientBtnClose = "<%=this.BtnClose.ClientID %>"

        _clientbtnAddEarningDeduction = "<%=this.btnAddEarningDeduction.ClientID %>"
        _clientbtnAddOperator = "<%=this.btnAddOperator.ClientID %>"
        _clientbtnAddFormulaValue = "<%=this.btnAddFormulaValue.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)

        function EndReqHandler(sender, args) {
            DisableControls(false, sender)
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientOptFormula || postBackElement.id == _clientoptRange)
                DiasableDeleteAll();
        }
        function beginRequestHandler(sender, args) {
            DisableControls(true, sender)
        }

        function DisableControls(action, sender) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (document.getElementById(_clientBtnSave) != null)
                document.getElementById(_clientBtnSave).disabled = action;
            if (document.getElementById(_clientbtnCancel) != null)
                document.getElementById(_clientbtnCancel).disabled = action;
            if (document.getElementById(_clientbtnClear) != null)
                document.getElementById(_clientbtnClear).disabled = action;
            if (document.getElementById(_clientBtnClose) != null)
                document.getElementById(_clientBtnClose).disabled = action;
            if (document.getElementById(_clientbtnAddFormulaValue) != null)
                document.getElementById(_clientbtnAddFormulaValue).disabled = action;

            if (document.getElementById(_clientbtnDeleteFormula) != null && (postBackElement.id != _clientbtnDeleteFormula || (postBackElement.id == _clientbtnDeleteFormula && action)))
                document.getElementById(_clientbtnDeleteFormula).disabled = action;

            if ((postBackElement.id == _clientBtnSave || postBackElement.id == _clientbtnCancel ||
                postBackElement.id == _clientbtnDeleteFormula || postBackElement.id == _clientbtnClear)) {
                if (!action)
                    action = true;
                if (document.getElementById(_clientbtnAddEarningDeduction) != null)
                    document.getElementById(_clientbtnAddEarningDeduction).disabled = action;
                if (document.getElementById(_clientbtnAddOperator) != null)
                    document.getElementById(_clientbtnAddOperator).disabled = action;
            }
        }

        function CheckAllUncheckAlls() {
            if (document.getElementById(_ClientChkAll) != null)
                var checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                VisibleDetailsLink(_clientlstvwAmountRange, iRowCount)
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        function Confermation() {
            var sMode = document.getElementById(_clienthidMode).value
            var bResult = true
            var optFormula = document.getElementById(_clientOptFormula)
            if (sMode.match("FORMULA") == null) {
                if (CheckSelection(_clientlstvwAmountRange, 'ChkSelect')) {
                    bResult = true
                    if (bResult) {
                        if (sMode.match("FORMULA") == null) {
                            if (!confirm("Are you sure you want to delete all range(s)?"))
                                return false
                        }
                    }
                }
            }
            else {
                if (!confirm("Are you sure you want to delete all formula(e)?"))
                    return false
            }

            return true
        }
        function CheckSelectedAmountRange(objBtn, rowNum, chkSelect) {
            var bResult = true
            var optFormula = document.getElementById(_clientOptFormula)
            var sMode = document.getElementById(_clienthidMode).value
            var formula = optFormula.value
            if (optFormula.checked == false) {
                if (CheckSelection(_clientlstvwAmountRange, 'ChkSelect')) {
                    bResult = true
                    if (bResult) {
                        if (sMode.match("FORMULA") != null) {
                            if (!confirm("Are you sure you want to delete formula?"))
                                return false
                        }
                        if (document.getElementById(_clientbtnDeleteFormula) != null)
                            document.getElementById(_clientbtnDeleteFormula).disabled = true
                        document.getElementById(_clientBtnClose).disabled = true
                    }
                }
                else {
                    alert("At least one range should be selected.")
                    if (document.getElementById(_clienttrMessage) != null)
                        document.getElementById(_clienttrMessage).style.display = "none"
                    bResult = false
                }
            }
            else {
                if (typeof (Page_ClientValidate) == 'function')
                    bResult = Page_ClientValidate("FormulaAndRange")
                if (bResult) {
                    if (sMode.match("FORMULA") == null) {
                        if (!confirm("Are you sure you want to delete range?"))
                            return false
                    }
                }
            }

            if (bResult == true && optFormula.checked == false && sMode == "EDIT RANGE" &&
                 document.getElementById(_clientlstvwAmountRange + "_ctrl" + rowNum + "_lnkBtnDetails") != null
                && document.getElementById(_clientlstvwAmountRange + "_ctrl" + rowNum + "_ChkSelect").checked == true) {
                if (!confirm("Do you want to update amount of all the months?"))
                    $get(_clienthidUpdateMonthwiseAmount).value = "N"
                else
                    $get(_clienthidUpdateMonthwiseAmount).value = "Y"
            }
            return bResult
        }
        function VisibleDetailsLink(listview, RowId) {
            var chkSelect = document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_ChkSelect")
            if (chkSelect.checked == true) {
                document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_txtFromAmount").disabled = false
                document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_txtUptoAmount").disabled = false
                document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_txtAmount").disabled = false
                if (document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_lnkBtnDetails") != null)
                    document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_lnkBtnDetails").disabled = false
                document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_BtnSaveRange").disabled = false
            }
            else {
                if (document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_trlstvwRange") != null)
                    document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_trlstvwRange").style.display = "none"
                if (document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_lnkBtnDetails") != null)
                    document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_lnkBtnDetails").disabled = true
                document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_txtFromAmount").disabled = true
                document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_txtUptoAmount").disabled = true
                document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_txtAmount").disabled = true
                if (document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_lnkBtnDetails") == null)
                    document.getElementById(_clientlstvwAmountRange + "_ctrl" + RowId + "_BtnSaveRange").disabled = true
            }
        }

        function ValidateAmountRange(objBtn, rowNum, chkSelect) {
            var sEmptyMessage = ""
            var sRangeMessage = ""
            var sDuplicateMessage = ""
            var iCheckedCount = 0
            var lnkFlage = "N";
            var iRowCount = 0
            var optFormula = document.getElementById(_clientOptFormula)
            if (optFormula.checked == false) {
                chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_ChkSelect")

                if (document.getElementById(_clienttxtFormulaName).value.trim() == "") {
                    alert("Range Name should not be blank.");
                    return false;
                }

                while (chk != null) {
                    if (chk.checked == true) {
                        iCheckedCount = iCheckedCount + 1
                        txtFromAmount = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_txtFromAmount")
                        txtUptoAmounte = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_txtUptoAmount")
                        txtAmount = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_txtAmount")
                        if (txtFromAmount.value.trim() == "" || txtUptoAmounte.value.trim() == "" || txtAmount.value.trim() == "") {
                            sEmptyMessage = "Values of selected rows should not be blank."
                            break
                        }
                        else {
                            var fromAmount = txtFromAmount.value.trim()
                            var uptoAmount = txtUptoAmounte.value.trim()
                            var amount = txtAmount.value.trim()
                            if (parseFloat(fromAmount) >= parseFloat(uptoAmount)) {
                                sEmptyMessage = "Selected 'From Amount' should be less than 'Upto amount'."
                                break
                            }
                            else if (parseFloat(uptoAmount) < parseFloat(amount)) {
                                sEmptyMessage = "Selected 'Amount' should be less than 'Upto amount'."
                                break
                            }
                            else if (iCheckedCount > 1) {
                                var isDuplicate = CheckIsDuplicate(iCheckedCount, parseInt(fromAmount), parseInt(uptoAmount), parseInt(amount))
                                if (isDuplicate == true) {
                                    sEmptyMessage = "Selected ranges are overlapping."
                                    break
                                }
                            }
                            if (document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_lnkBtnDetails") != null) {
                                lnkFlage = "Y"
                            }
                            else
                                lnkFlage = "N"
                        }
                    }
                    iRowCount = iRowCount + 1
                    chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_ChkSelect")
                }
                if (sEmptyMessage != "") {
                    alert(sEmptyMessage)
                    if (document.getElementById(_clienttrMessage) != null)
                        document.getElementById(_clienttrMessage).style.display = "none"
                    return false
                }
                else {
                    if (!CheckSelectedAmountRange(objBtn, rowNum, chkSelect)) {
                        return false
                    }
                    return true
                }
            }
            return true
        }
        function CheckIsDuplicate(iCheckedCount, fromAmount, uptoAmount, amount) {
            var iRowCount = 0
            var IsDuplicate = false
            iCheckedCount = iCheckedCount - 1
            chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true && iRowCount < iCheckedCount) {
                    txtFromAmount = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_txtFromAmount").value
                    txtUptoAmount = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_txtUptoAmount").value
                    txtAmount = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_txtAmount").value
                    if ((txtFromAmount <= fromAmount && fromAmount <= txtUptoAmount) ||
                        (txtFromAmount <= uptoAmount && uptoAmount <= txtUptoAmount) ||
                        (txtFromAmount >= fromAmount && uptoAmount >= txtUptoAmount) ||
                        (txtFromAmount <= fromAmount && uptoAmount <= txtUptoAmount)) {
                        IsDuplicate = true
                        break
                        iRowCount = iRowCount + 1
                    }
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_ChkSelect")
            }
            return IsDuplicate
        }
        function ValidateMonthwiseAmount(oSrc, args) {
            var iRowCount = 0
            var iMonthCount = 0
            var sMonths = ""
            var sMonthwiseAmount = ""
            chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    if (document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_lstvwRange_tblRange") != null) {
                        iMonthCount = 0
                        sMonths = ""
                        var txtMonthwiseAmount = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_lstvwRange" + "_ctrl" + iMonthCount + "_txtAmount")
                        while (txtMonthwiseAmount != null) {
                            var sMonthName = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_lstvwRange" + "_ctrl" + iMonthCount + "_lblMonth").innerHTML
                            if (txtMonthwiseAmount.value.trim() == "")
                                sMonths = sMonths + "," + sMonthName
                            else {
                                var iMonthwiseAmount = parseInt(txtMonthwiseAmount.value.trim())
                                var iUptoAmount = $get(_clienthidUptoAmount).value
                                if (iMonthwiseAmount >= iUptoAmount) {
                                    sMonthwiseAmount = sMonthwiseAmount + "," + sMonthName
                                }
                            }
                            iMonthCount = iMonthCount + 1
                            txtMonthwiseAmount = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_lstvwRange" + "_ctrl" + iMonthCount + "_txtAmount")
                        }
                    }
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (sMonths != "") {
                sMonths = sMonths.substring(1)
                $get(_clientcstMonthwiseAmount).errormessage = "Amount should not be blank of month(s) :" + sMonths
                args.IsValid = false
                return true
            }
            else if (sMonthwiseAmount != "") {
                sMonthwiseAmount = sMonthwiseAmount.substring(1)
                $get(_clientcstMonthwiseAmount).errormessage = "Amount should be less than 'Upto Amount' for month(s) :" + sMonthwiseAmount
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ActivateLink(iRowId) {
            chk = document.getElementById(_clientlstvwAmountRange + "_ctrl" + iRowId + "_ChkSelect")
            if (chk.checked == false)
                return false
            return true
        }
        function EnableButton(txt, btn) {
        }

        function ClearConfermation() {
            if (document.getElementById(_clienttxtFormula) != null && document.getElementById(_clienttxtFormula).value != "") {
                if (!confirm("Are you sure you want to clear the formula?"))
                    return false;
            }
        }

        function ConfirmDelete() {
            var sMode = document.getElementById(_clienthidMode).value
            var sMessage = "Are you sure you want to delete this formula?";
            if (sMode.match("FORMULA") == null)
                sMessage = "Are you sure you want to delete this range?";

            if (!confirm(sMessage))
                return false;
            return true;
        }

        function DisableFormulaButton(cmb, btn) {
            if (cmb.value == "0")
                btn.disabled = true;
            else
                btn.disabled = false;
        }

        function DoPostback(btn) {
            __doPostBack(btn.name, '')
        }

        function DiasableDeleteAll() {
            if (document.getElementById(_clienthidMode).value.match("NEW") != null) {
                if (document.getElementById(_clientbtnDeleteFormula) != null)
                    document.getElementById(_clientbtnDeleteFormula).disabled = true;
            }
            else
                if (document.getElementById(_clientbtnDeleteFormula) != null) {
                    document.getElementById(_clientbtnDeleteFormula).disabled = false;
                }
        }
        
    </script>
</asp:Content>
