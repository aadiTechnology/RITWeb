<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ExternalStudentFeeUI.aspx.cs" Inherits="ExternalStudentFeeUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="95%">
            <tr id="trComponentsDetails" runat="server">
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="LblErrorMsg"
                                                        ShowSummary="true" ValidationGroup="Pay" />
                                                    <asp:RequiredFieldValidator ID="reqCalDtDate" runat="server" Display="None" ControlToValidate="txtCalDt"
                                                        CssClass="ClsMdtStar" ErrorMessage="Please select valid Date." ValidationGroup="Pay"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="reqtxtStudentName" runat="server" Display="None"
                                                        ControlToValidate="txtStudentName" CssClass="ClsMdtStar" ErrorMessage="Studnet Name should not be blank"
                                                        ValidationGroup="Pay"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="reqcmbFeeType" runat="server" Display="None" ControlToValidate="cmbFeeType"
                                                        CssClass="ClsMdtStar" ErrorMessage="Fee Type should be selected" ValidationGroup="Pay"
                                                        InitialValue="0"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="reqtxtAmount" runat="server" Display="None" ControlToValidate="txtAmount"
                                                        CssClass="ClsMdtStar" ErrorMessage="Amount should not be blank" ValidationGroup="Pay"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                                        ControlToValidate="txtMobileNo" CssClass="ClsMdtStar" ErrorMessage="Mobile Number should not be blank"
                                                        ValidationGroup="Pay"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cstValidateChequeNo" runat="server" Display="None" ClientValidationFunction="ValidateChequeNo"
                                                        ValidationGroup="Pay"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cstValidateChequeDate" runat="server" Display="None" ClientValidationFunction="ValidateChequeDate"
                                                        ValidationGroup="Pay"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cstValidateChequeBank" runat="server" Display="None" ClientValidationFunction="ValidateBank"
                                                        ValidationGroup="Pay"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cstElectronicType" runat="server"   Display="None"  ClientValidationFunction="ValidateElectronicType" 
                                                        ValidationGroup="Pay"></asp:CustomValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnPay" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwExternalStudentFee" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
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
                                <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="50%">
                                            <tr>
                                                <td align="center" id="tdMessage" runat="server">
                                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                                        Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnPay" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwExternalStudentFee" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="170px">
                                                    <span class="ClsLabel">Date :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCalDt" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"
                                                        ReadOnly="true"></asp:TextBox>
                                                    <rjs:PopCalendar ID="CalDtPopup" runat="server" Control="txtCalDt" Format="dd MMM yyyy"
                                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Date."
                                                        To-Today="true" />
                                                    <span class="ClsMdtStar">*&nbsp;</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Student Name :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStudentName" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Fee Type :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbFeeType" runat="server" CssClass="LrgCombo" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Amount :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="LrgTxtBox" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Mobile Number :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="LrgTxtBox" MaxLength="10"
                                                        Style="text-align: left; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Mode :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:RadioButton ID="optCash" runat="server" GroupName="PaymentMode" Text="Cash" />
                                                    <asp:RadioButton ID="optCheque" runat="server" GroupName="PaymentMode" Text="Cheque" />
                                                     <asp:RadioButton ID="optElectronic" runat="server" GroupName="PaymentMode" Text="Electonic" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 22px">
                                                    <table id="tblChequeDetails" style="display: none;">
                                                        <tr id="trChequeNo">
                                                            <td align="left" class="ClsBorderlight" style="width:170px;">
                                                                <span  id="lblChequeNo"class="ClsLabel">Cheque No. :</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtChequeNo" runat="server" CssClass="LrgTxtBox" MaxLength="25"
                                                                    Style="text-align: left; padding-right: 5px" onblur="extractNumber(this,1,false);"
                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trChequeDate">
                                                            <td align="left" class="ClsBorderlight">
                                                                <span class="ClsLabel">Cheque Date :</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtChequeDt" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"></asp:TextBox>
                                                                <rjs:PopCalendar ID="CalChequeDtPopup" runat="server" Control="txtChequeDt" Format="dd MMM yyyy"
                                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Date." />
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr  id="trElectronicTypes" style="display:none;">
                                                            <td align="left" valign="top" class="ClsBorderlight">
                                                               <span class="ClsLabel">Type :</span>
                                                          </td>
                                                           <td align="left" class="ClsTextNormal" style="height: 9px">
                                                              <asp:DropDownList ID="cmbElectronicTypes" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="17">
                                                              </asp:DropDownList>
                                                              &nbsp; <span class="ClsMdtStar">* </span>                                                          
                                                           </td>
                                                        </tr>
                                                        <tr id="trBankName">
                                                            <td align="left" class="ClsBorderlight">
                                                                <span class="ClsLabel">Bank Name :</span>
                                                            </td>
                                                            <td style="white-space: nowrap" align="left">
                                                                <asp:DropDownList ID="cmbBankName" runat="server" CssClass="LrgCombo" AutoPostBack="false">
                                                                </asp:DropDownList>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnPay" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwExternalStudentFee" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:UpdatePanel ID="upnl4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnPay" runat="server" CssClass="ClsBtn" Text="Pay" disable-page="true"
                                            ValidationGroup="Pay" OnClientClick="ClearMessages();" OnClick="btnPay_Click" />
                                        <asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                            Text="Clear" OnClick="btnClear_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnPay" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwExternalStudentFee" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;">
                            </td>
                        </tr>
                        <tr align="center" style="text-align: center; margin: 0px auto;">
                            <td colspan="2" style="text-align: center;">
                                <table width="35%" align="center" style="text-align: center; margin: 0px auto;">
                                    <tr align="center" style="text-align: center; margin: 0px auto;">
                                        <td class="ClsBorderlight" align="center" style="width: 150px;">
                                            <asp:Label ID="lblNameSearch" runat="server" CssClass="ClsLabel" Text="Name / Mobile No. "></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtSearch" CssClass="ExLrgTxtBox" runat="server" TabIndex="11"></asp:TextBox>
                                            <asp:Button ID="btnSearch" CssClass="ClsBtn" runat="server" Text="Search" TabIndex="12"
                                                CausesValidation="false" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="80%">
                                            <tr id="trItemCount" runat="server">
                                                <td align="center" style="width: 100%;">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwExternalStudentFee"
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
                                                    <asp:ListView ID="lstvwExternalStudentFee" runat="server" DataKeyNames="Id,ReceiptNumber,AccountHeaderId" OnItemDataBound="lstvwExternalStudentFee_ItemDataBound"
                                                        OnItemCommand="lstvwExternalStudentFee_ItemCommand" OnDataBound="lstvwExternalStudentFee_DataBound">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th align="center" width="10%">
                                                                        <asp:LinkButton ID="lnkDate" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            CommandArgument="PaymentDate" CommandName="SortRow">Date</asp:LinkButton>
                                                                    </th>
                                                                    <th align="left" style="padding-left: 10px;">
                                                                        Student Name
                                                                    </th>
                                                                    <th align="center" width="10%">
                                                                        Mobile No.
                                                                    </th>
                                                                    <th align="left" style="padding-left: 10px;" width="15%">
                                                                        Fee Type
                                                                    </th>
                                                                    <th align="center" width="8%">
                                                                        Amount
                                                                    </th>
                                                                    <th align="center" width="13%">
                                                                        Payment Mode
                                                                    </th>
                                                                    <th align="center" width="07%">
                                                                        Edit
                                                                    </th>
                                                                    <th width="07%">
                                                                        Delete
                                                                    </th>
                                                                    <th width="07%">
                                                                        Receipt
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                    <td colspan="9" align="left">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwExternalStudentFee">
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
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center" style="padding-left: 10px;">
                                                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text='<%#Eval("Date") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" style="padding-left: 5px;">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabelR" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMobileNo" runat="server" CssClass="ClsLabelR" Text='<%#Eval("MobileNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" style="padding-left: 5px;">
                                                                    <asp:Label ID="lblFeeType" runat="server" CssClass="ClsLabelR" Text='<%#Eval("FeeType") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabelR" Text='<%#Eval("Amount") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMode" runat="server" CssClass="ClsLabelR" Text='<%#Eval("PaymentMode") %>'></asp:Label>
                                                                </td>
                                                                <td align="center" width="">
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" ToolTip="Delete" OnClientClick="return ConfirmDelete()" />
                                                                </td>
                                                                <td id="tdReceipt" runat="server" align="center">
                                                                    <asp:HyperLink ID="hlnkReceipt" runat="server" Text="<%$ Resources:LocalizedResources,Receipt%>"
                                                                        Visible="true" NavigateUrl="CustomizeInternalRecieptPopUp.aspx"> </asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center" style="padding-left: 10px;">
                                                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text='<%#Eval("Date") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" style="padding-left: 5px;">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabelR" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMobileNo" runat="server" CssClass="ClsLabelR" Text='<%#Eval("MobileNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" style="padding-left: 5px;">
                                                                    <asp:Label ID="lblFeeType" runat="server" CssClass="ClsLabelR" Text='<%#Eval("FeeType") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabelR" Text='<%#Eval("Amount") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMode" runat="server" CssClass="ClsLabelR" Text='<%#Eval("PaymentMode") %>'></asp:Label>
                                                                </td>
                                                                <td align="center" width="">
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" ToolTip="Delete" OnClientClick="return ConfirmDelete()" />
                                                                </td>
                                                                <td id="tdReceipt" runat="server" align="center">
                                                                    <asp:HyperLink ID="hlnkReceipt" runat="server" Text="<%$ Resources:LocalizedResources,Receipt%>"
                                                                        Visible="true" NavigateUrl="ExternalStudentsFeePaymentReceipt.aspx"> </asp:HyperLink>
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
                                        <asp:HiddenField ID="hidExternalStudentFeeId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidFeeDetails" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:ObjectDataSource TypeName="BusinessLogic.ExternalStudentFeeBL" EnablePaging="true"
                                            ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                            EnableCaching="false">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="int32" />
                                                <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" PropertyName="Value" />
                                                <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" PropertyName="Value" />
                                                <asp:ControlParameter Name="asFilter" ControlID="txtSearch" PropertyName="Text" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnPay" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwExternalStudentFee" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <%--<tr>
                            <td align="center">
                                <asp:Button CssClass="ClsBtn" ID="btnResetReceipt" CausesValidation="false" runat="server"
                                    Text="Reset Receipt" Width="122px" />
                                <br />
                            </td>
                        </tr>--%>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clienttxtCalDt = "<%=this.txtCalDt.ClientID %>"
        _clienttxtStudentName = "<%=this.txtStudentName.ClientID %>"
        _clientcmbFeeType = "<%=this.cmbFeeType.ClientID %>"
        _clienttxtAmount = "<%=this.txtAmount.ClientID %>"
        _clienttxtMobileNo = "<%=this.txtMobileNo.ClientID %>"
        _clientoptCash = "<%=this.optCash.ClientID %>"
        _clientoptCheque = "<%=this.optCheque.ClientID %>"
        _clientoptElectronic = "<%=this.optElectronic.ClientID %>"
        _clienttxtChequeNo = "<%=this.txtChequeNo.ClientID %>"
        _clienttxtChequeDt = "<%=this.txtChequeDt.ClientID %>"
        _clientcmbBankName = "<%=this.cmbBankName.ClientID %>"

        _clientlblMessage = "<%=this.lblMessage.ClientID %>"
        _clienthidFeeDetails = "<%=this.hidFeeDetails.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);

        function BeginRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
        }

        function EndRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
            ShowChequeControls();
        }



        function ClearMessages() {
            var lblErrorMessage = $get(_clientlblMessage);
            if (lblErrorMessage)
                lblErrorMessage.innerHTML = '';
        }

        function SetAmmount() {
            var FeeDetails = document.getElementById(_clienthidFeeDetails).value;
            var selectedFeeTypeId = document.getElementById(_clientcmbFeeType).value;
            var arr = new Array();
            arr = FeeDetails.split(",");
            var FeeArry;
            var sValue = false;

            for (var ival = 0; ival < arr.length; ival++) {
                FeeArry = arr[ival].split("$");

                if (FeeArry[0] == selectedFeeTypeId) {
                    $("#" + _clienttxtAmount).val(FeeArry[1]);
                    sValue = true;
                    break;
                }
            }

            if (sValue == false) {
                $("#" + _clienttxtAmount).val(0);
            }
        }
        function ValidateChequeNo(oSrc, args) {
            var ChequeNo = document.getElementById(_clienttxtChequeNo).value;
            var isCheque = $get(_clientoptCheque).checked;
            var isElectronic = $get(_clientoptElectronic).checked;

           if (isCheque) {
                if (ChequeNo.trim() === "") {
                    oSrc.errormessage = "Cheque No. should not be blank.";
                    args.IsValid = false;
                    return;
                }
            }
           if (isElectronic) {
                if (ChequeNo.trim() === "") {
                    oSrc.errormessage = "Transaction No. should not be blank.";
                    args.IsValid = false;
                    return;
                }
            }
         args.IsValid = true;
        }

        function ValidateElectronicType(oSrc, args) {
            var isElectronic = $get(_clientoptElectronic).checked;
            var typeValue = document.getElementById("<%= cmbElectronicTypes.ClientID %>").value;

            if (isElectronic) {
                if (typeValue === "" || typeValue === "0") {
                    oSrc.errormessage = "Electronic Type should be selected.";
                    args.IsValid = false;
                    return;
                }
            }

            args.IsValid = true;
        }

        function ValidateChequeDate(oSrc, args) {
            var ChequeDate = document.getElementById(_clienttxtChequeDt).value;
            if ($get(_clientoptCheque).checked) {
                if (ChequeDate == "") {
                    oSrc.errormessage = "Cheque Date should not be blank."
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }

        function ValidateBank(oSrc, args) {
            var bankId = document.getElementById(_clientcmbBankName).value;
            var isCheque = $get(_clientoptCheque).checked;
            var isElectronic = $get(_clientoptElectronic).checked;

            args.IsValid = true;

           if (isCheque || isElectronic) {
                if (bankId === "0" || bankId === "") {
                    oSrc.errormessage = "Bank Name should be selected.";
                    args.IsValid = false;
                    return;
                }
            }
        }

        function ShowChequeControls() {
            var isCash = $get(_clientoptCash).checked;
            var isCheque = $get(_clientoptCheque).checked;
            var isElectronic = $get(_clientoptElectronic).checked;

            if (isCash) {
                $("#tblChequeDetails").hide();
            }
            else if (isCheque) {
                $("#tblChequeDetails").show();
                $("#trChequeDate").show();
                $("#trElectronicTypes").hide();
                $("#lblChequeNo").text("Cheque No. :");
            }
            else if (isElectronic) {
                $("#tblChequeDetails").show();
                $("#trChequeDate").hide();
                $("#trElectronicTypes").show();
                $("#lblChequeNo").text("Transaction No. :");
            }
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function OpenRecieptPopup(sQueryString) {
            window.open('ExternalStudentsFeePaymentReceipt.aspx?' + sQueryString, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=670,height=450');
            return false;
        }

    </script>
</asp:Content>
