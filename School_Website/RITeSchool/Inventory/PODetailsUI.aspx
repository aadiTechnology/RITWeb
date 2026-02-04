<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PODetailsUI.aspx.cs" Inherits="PODetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="98%">
        <tr>
            <td align="right">
                <div style="float: right;" class="LblErrorMsg" id="lblMandatoryMark" runat="server"
                    viewstatemode="Enabled">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />                                    
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="rdCategory" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="ClsGreenBG" width="150px" style="float: right; padding-right: 5px; padding-top: 5px;">
                                <asp:LinkButton ID="lnkReceiverDetails" runat="server" Text="External PO Receiver"
                                    CssClass="SubTitle" Style="text-align: left;" CausesValidation="false"></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="LblUpdateSuccess" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                                        CssClass="ClsLabel" Font-Bold="true"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="rdCategory" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="tr1" runat="server">
            <td align="center">
                <table id="tblGSTDetails" width="80%" runat="server" style="padding-bottom: 15px !important">
                    <tr align="center">
                        <td align="center">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1" runat="server" width="98%">
                                        <tr>
                                            <td class="txtNormal" colspan="4">
                                                <asp:RequiredFieldValidator ID="RequiredReceiverName" runat="server" ErrorMessage="Receiver Name Should not be blank."
                                                    Display="None" ControlToValidate="ddlReceiverName" InitialValue="0"></asp:RequiredFieldValidator>                                                
                                                <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidatePoNo"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidatePoDate"></asp:CustomValidator>                                                
                                                <%--<asp:RequiredFieldValidator ID="RequiredGSTCategoryId" runat="server" ErrorMessage="GSTCategoryId should be selected"
                                                    Display="None" ControlToValidate="ddlGSTCategory" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="PO No. should not be duplicate."
                                                    Display="None" CssClass="ClsMdtStar" OnServerValidate="Validate_PONo"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="RequiredSubject" runat="server" ErrorMessage="Subject Should not be blank."
                                                    Display="None" ControlToValidate="txtSubject"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredStartDate" runat="server" ErrorMessage="Start Date Should not be blank." Enabled="false"
                                                    Display="None" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredEndDate" runat="server" ErrorMessage="End Date Should not be blank." Enabled="false"
                                                    Display="None" ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomDescription" runat="server" ErrorMessage="Value should be set for at least one Description."
                                                    CssClass="ClsMdtStar" ClientValidationFunction="ValidateDescription" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomAmount" runat="server" ErrorMessage="Quantity should not be blank or zero for enterd Description."
                                                    Display="None" CssClass="ClsMdtStar" ClientValidationFunction="ValidateAmount"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Description should not be blank for enterd Quantity."
                                                    Display="None" CssClass="ClsMdtStar" ClientValidationFunction="ValidateAmountDescription"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="Rate should not be blank or zero for enterd Description."
                                                    Display="None" CssClass="ClsMdtStar" ClientValidationFunction="ValidateRate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="Description should not be blank for enterd Rate."
                                                    Display="None" CssClass="ClsMdtStar" ClientValidationFunction="ValidateRateDescription"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr align="center" style="text-align: center; margin: 0px auto;">
                                            <td align="center" style="text-align: center;">
                                                <table align="center">
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <asp:Label ID="lblCategory" runat="server" CssClass="clsLabel" Text="Category :"></asp:Label>
                                                        </td>
                                                        <td align="left" class="clsBorderLight">
                                                            <asp:RadioButtonList ID="rdCategory" runat="server" AutoPostBack="true" RepeatColumns="2" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdCategory_SelectedIndexChanged">
                                                                <asp:ListItem Text="Purchase Order" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Work Order" Value="2"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left" width="150px">
                                                            <span class="ClsLabel">Receiver Name :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlReceiverName" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <asp:Label ID="lblPONo" runat="server" CssClass="clsLabel" Text="PO No :"></asp:Label>
                                                        </td>
                                                        <td class="TxtNormal" align="left">
                                                            <asp:TextBox ID="txtPONoPrefix" CssClass="MidTxtBox" runat="server" MaxLength="20"
                                                                Width="130px" ReadOnly="true">
                                                            </asp:TextBox>
                                                            <asp:TextBox ID="txtPONo" CssClass="SmlTxtBox" runat="server" MaxLength="10" onblur="extractNumber(this,2,false);"
                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false">
                                                            </asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <asp:Label ID="lblPODate" runat="server" CssClass="clsLabel" Text="PO Date :"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPODate" CssClass="MidTxtBox" runat="server">
                                                            </asp:TextBox>
                                                            <rjs:PopCalendar ID="cPODate" runat="server" Control="txtPODate" Format="dd MMM yyyy"
                                                                ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid PO date." />
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">GST Category :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlGSTCategory" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">Subject :</span>
                                                        </td>
                                                        <td class="txtnormal" align="left">
                                                            <asp:TextBox ID="txtSubject" CssClass="LrgTxtBox" runat="server" MaxLength="100"
                                                                Width="300px"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr id="trAMCstart" runat="server" visible="false">
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">AMC Start Date :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtStartDate" CssClass="MidTxtBox" runat="server">
                                                            </asp:TextBox>
                                                            <rjs:PopCalendar ID="cStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                                ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid start date." />
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr id="trAMCend" runat="server" visible="false">
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">AMC End Date :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtEndDate" CssClass="MidTxtBox" runat="server">
                                                            </asp:TextBox>
                                                            <rjs:PopCalendar ID="cEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                                ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid End date." />
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="Tr2" runat="server">
                                            <td colspan="4" align="center">
                                                <asp:ListView ID="lstvwPODetails" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwPODetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table id="tblPODetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                            class="GridBorder" width="60%">
                                                            <tr id="TrHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" width="25%">
                                                                    <asp:Label ID="Label1" runat="server" Text="Description"></asp:Label>
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    <asp:Label ID="Label2" runat="server" Text="Quantity"></asp:Label>
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    <asp:Label ID="Label3" runat="server" Text="Rate"></asp:Label>
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    <asp:Label ID="label10" runat="server" Text="GST Category"></asp:Label>
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    <asp:Label ID="Label4" runat="server" Text="Amount"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trItemtemplates" runat="server" class="ClsGridRow">
                                                            <td align="center">
                                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="300" Width="98%" TextMode="MultiLine" Text='<%# Eval("Description") %>'>                                                                
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="txtQuantity" runat="server" MaxLength="10" Width="98%" Style="text-align: right;
                                                                    padding-right: 5px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                    onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                    onpaste="event.returnValue=false">                                                                
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="txtRate" runat="server" MaxLength="10" Width="98%" Style="text-align: right;
                                                                    padding-right: 5px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                    onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                    onpaste="event.returnValue=false">                                                                
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:DropDownList ID="ddlGSTCategory" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" Width="98%" Style="text-align: right;
                                                                    padding-right: 5px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                    onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                    onpaste="event.returnValue=false" Enabled="false">                                                                
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <table width="60%">
                                                    <tr>
                                                        <td align="right" class="ClsBorderlight">
                                                            <asp:Label ID="Label8" runat="server" Style="padding-right: 5px; float: right;" CssClass="clsLabel"
                                                                Text="Total"></asp:Label>
                                                        </td>
                                                        <td align="right" width="20%">
                                                            <asp:TextBox ID="txtTotal" runat="server" Width="98%" Text="" onblur="extractNumber(this,2,false);"
                                                                disabled="true" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td align="right" class="ClsBorderlight">
                                                            <asp:Label ID="Label5" runat="server" Style="padding-right: 5px; float: right;" CssClass="clsLabel"
                                                                Text="GST Percentage"></asp:Label>
                                                        </td>
                                                        <td align="center" width="20%">--%>
                                                            <%--<asp:LinkButton ID="lnkPlus" runat="server" Text="<<" style="font-weight:bold;" OnClientClick="Decrease(); return false;"></asp:LinkButton>--%>
                                                            <%--<asp:TextBox ID="txtGST" runat="server" Width="100%" Text="" onblur="extractNumber(this,2,false);"
                                                                disabled="true" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>--%>
                                                            <%--<asp:LinkButton ID="LinkButton2" runat="server" style="font-weight:bold;" OnClientClick="Increase(); return false;">>></asp:LinkButton>--%>
                                                        <%--</td>
                                                    </tr>--%>
                                                    <%--<tr>
                                                        <td align="right" class="ClsBorderlight">
                                                            <asp:Label ID="Label9" runat="server" Style="padding-right: 5px; float: right;" CssClass="clsLabel"
                                                                Text="GST Amount"></asp:Label>
                                                        </td>
                                                        <td align="center" width="20%">
                                                            <asp:TextBox ID="txtGSTAmount" runat="server" Width="100%" Text="" onblur="extractNumber(this,2,false);"
                                                                disabled="true" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="ClsBorderlight">
                                                            <asp:Label ID="lblGrandTotal" runat="server" Style="padding-right: 5px; float: right;"
                                                                CssClass="clsLabel" Text="Grand Total"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:TextBox ID="txtGrandTotal" runat="server" Width="98%" Text="" onblur="extractNumber(this,2,false);"
                                                                disabled="true" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                    <%--<tr>
                                                        <td align="left" colspan="2" class="ClsBorderLight">
                                                            <asp:CheckBoxList ID="ChkInstructionList" runat="server" Width="100%">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <span class="ClsLabel">Additional Remark :</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtAdditionalRemarks" runat="server" Height="75px" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>                                                   
                                                    <tr>
                                                        <td align="left" colspan="2" style="height:20px">
                                                        </td>
                                                    </tr>
                                                    <tr>                                                       
                                                        <td align="left" class="ClsBorderlight" colspan="2">
                                                            <asp:Label ID="lblPreparedBy" runat="server" Style="padding-left: 5px;font-weight:bold;"
                                                                CssClass="clsLabel" Text="Prepared By : -"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <table width="925px">
                                                <tr>
                                                     <td>
                                                     </td>
                                                     <td align="left" class="ClsBorderLight" colspan="2">
                                                        <asp:Label ID="Label7" runat="server" Style="padding-right: 5px;font-weight:bold" CssClass="clsLabel"
                                                           Text="Instructions : "></asp:Label>
                                                     </td>
                                                 </tr>
                                                <tr>
                                                <td width="100pz">
                                                    <asp:CheckBox ID="chkAll" runat="server" Text="Select All" />
                                                </td>
                                                <td>                                               
                                                     <asp:ListView ID="lstvwInstructions" runat="server" ViewStateMode="Enabled" DataKeyNames="Id" OnItemCommand="lstvwInstructions_ItemCommand"
                                                             OnItemDataBound="lstvwInstructions_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table id="tbllstvwInstructions" runat="server" align="center" width="100%" class="GridBorder">
                                                                        <%--<tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                            <th align="left" colspan="2">
                                                                                <asp:Label ID="lblHeader" runat="server" CssClass="paddingL" Text="Category"></asp:Label>
                                                                            </th>
                                                                        </tr>--%>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="InstructionTrItemTemplate1" runat="server" class="ClsGridRow">
                                                                        <td align="left" class="paddingL" colspan="2">
                                                                            <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("Category") %>' Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdInstSelection" runat="server">
                                                                            <asp:CheckBox ID="chkAllInstructions" runat="server" />
                                                                        </td>
                                                                        <td id="tdInstruction" runat="server">
                                                                            <asp:CheckBoxList ID="ChkInstructionList" runat="server" Width="100%"></asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                             </asp:ListView>
                                                </td>
                                                </tr>
                                                </table>
                                            </td>
                                            </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                    class="ClsBtn" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                                                    class="ClsBtn" OnClick="btnCancel_Click" CausesValidation="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="hidData" EventName="ValueChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="rdCategory" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <hr style="border: 1px solid gray; width: 90%; height: 1px; color: Gray;" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
             <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
             <ContentTemplate>
                <table>
                    <tr>
                        <td class="clsBorderLight">
                            <span class="ClsLabel" id="Span1" runat="server">Status : </span>
                        </td>
                        <td colspan="2" align="left">
                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" 
                                Width="185px" TabIndex="2" 
                                onselectedindexchanged="ddlStatus_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsBorderLight">
                            <span class="ClsLabel" id="spnFilterHeader" runat="server">Receiver Name / PO No. : </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>"
                                class="ClsBtn" OnClick="btnSearch_Click" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
             </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="hidData" EventName="ValueChanged" />
                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="rdCategory" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trLink" runat="server">
            <td>
                <table id="tblLstvwReceiverDetails" align="center" width="90%" runat="server">
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="upnlLstvwReceiverDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table align="center" width="100%">
                                        <tr id="trDtPgCount" runat="server" visible="true">
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwReceiverDetails"
                                                    PageSize="20">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1%>" />
                                                                <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                <br />
                                                            </PagerTemplate>
                                                        </asp:TemplatePagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                        <tr id="trPager" runat="server" width="100%">
                                            <td align="center">
                                                <asp:ListView ID="lstvwReceiverDetails" runat="server" DataKeyNames="Id" ViewStateMode="Enabled"
                                                    OnItemDataBound="lstvwReceiverDetails_ItemDataBound"
                                                    OnDataBound="lstvwReceiverDetails_DataBound" 
                                                    OnSorting="lstvwReceiverDetails_Sorting" 
                                                    onitemcommand="lstvwReceiverDetails_ItemCommand">
                                                    <LayoutTemplate>
                                                        <table id="tbllstvwReceiverDetails1" runat="server" align="center" cellpadding="0"
                                                            cellspacing="1" class="GridBorder" width="100%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="PaddingL">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                        Text="Receiver Name"></asp:Label>
                                                                </th>
                                                                <th id="thPONO" runat="server" align="left" class="PaddingL" width="200px">
                                                                    <asp:LinkButton ID="lnkPONO" runat="server" CommandName="Sort" CommandArgument="PONo"
                                                                        Style="padding-left: 5px;" CausesValidation="false" ForeColor="Black" Text="PO No."></asp:LinkButton>
                                                                </th>
                                                                <th id="thPODate" runat="server" align="center" class="PaddingL" width="120px">
                                                                    <asp:LinkButton ID="lnkPODate" runat="server" Style="padding-left: 5px;" CommandName="Sort"
                                                                        CommandArgument="PODate" CausesValidation="false" ForeColor="Black" Text="PO Date"></asp:LinkButton>
                                                                </th>                                                                
                                                                <th align="right" class="PaddingLR" width="120px">
                                                                    <asp:Label ID="Label11" runat="server" Text="Final Amount" Style="padding-right: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="left" class="PaddingL" width="200px">
                                                                    <asp:Label ID="Label12" runat="server" Text="Prepared By" Style="padding-left: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="left" class="PaddingL">
                                                                    <asp:Label ID="Label9" runat="server" Text="Status" Style="padding-left: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="center" id="thSend" runat="server">
                                                                    <asp:Label ID="Label13" runat="server" Text="Send" Style="padding-right: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 75px;" id="thEdit" runat="server">
                                                                    Edit
                                                                </th>
                                                                <th align="center" style="width: 75px;" id="thDelete" runat="server">
                                                                    Delete
                                                                </th>
                                                                <th align="center" style="width: 75px;">
                                                                    Details
                                                                </th>
                                                                <th align="center" style="width: 75px;">
                                                                    <asp:Label ID="lblReport" runat="server" Text="PO" Style="padding-left: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 75px;" id="thAction" runat="server" visible="false">
                                                                    Action
                                                                </th>
                                                                <th align="center" style="width: 100px;" id="thPay" runat="server">
                                                                    Pay Pending
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr id="trDataPager" class="ClsBorderPager">
                                                                <td colspan="13">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwReceiverDetails"
                                                                        PageSize="20">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
                                                                                                <asp:DropDownList ID="ddlCnt" ViewStateMode="Enabled" runat="server" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged"
                                                                                                    AutoPostBack="true">
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
                                                        <tr id="trItemtemplate" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                            <td align="left" class="PaddingL">
                                                                <asp:Label ID="lblReceiverName1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("ReceiverName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td1" align="left" class="PaddingLR" runat="server">
                                                                <asp:Label ID="lblPONo1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("PONo") %>'>
                                                                </asp:Label>
                                                                <asp:HiddenField ID="hidData1" runat="server" Value="" />
                                                            </td>
                                                            <td id="Td2" align="center" class="PaddingL" runat="server">
                                                                <asp:Label ID="lblPODate1" runat="server" Text='<%# Eval("PODate") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td6" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="Label14" runat="server" Style="padding-right: 5px;" Text='<%# Eval("GrandTotal") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td4" align="left" class="PaddingL" runat="server">
                                                                <asp:Label ID="Label15" runat="server" Style="padding-left: 5px;" Text='<%# Eval("PreparedBy") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="PaddingL">
                                                                <asp:Label ID="lblStatus" runat="server" Style="padding-left: 5px;" Text='<%# Eval("Status") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" id="tdSend" runat="server">
                                                                <asp:ImageButton ID="imgBtnSend" runat="server" CausesValidation="false" CommandName="SendForApproval" Visible="false"
                                                                    ImageUrl="../images/DisableSelect.gif" ToolTip="Send For Approval" />
                                                            </td>
                                                            <td align="center" id="tdEdit" runat="server">
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" CommandName="UpdatePODetails"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center" id="tdDelete" runat="server">
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="DeletePODetails"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:LinkButton ID="LnkBtnDetails" runat="server" Text="Details" CausesValidation="false"></asp:LinkButton>
                                                            </td>
                                                            <td id="Td7" runat="server" align="center">
                                                                <asp:LinkButton ID="lnkExport" runat="server" CommandName="InvoiceDetails" CausesValidation="false"
                                                                    Text="Open"></asp:LinkButton>
                                                            </td>
                                                            <td align="center" id="tdAction" runat="server" visible="false">
                                                                <asp:LinkButton ID="lnkBtnActions" runat="server" Text="Actions" CausesValidation="false" CommandName="Action"></asp:LinkButton>
                                                            </td>
                                                            <td align="center" id="tdPay" runat="server" visible="false">
                                                                <asp:LinkButton ID="lnkBtnPay" runat="server" Text="Pay" CausesValidation="false" CommandName="PAY"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr3" runat="server" style="display: none">
                                                            <td align="center" colspan="10">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:ListView ID="lstvwDescription" runat="server" ViewStateMode="Enabled">
                                                                                <LayoutTemplate>
                                                                                    <table id="tbllstvwDescription" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                                                        class="GridBorder" width="50%">
                                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                            <th align="left" width="5%">
                                                                                                <span class="PaddingL" style="padding-left: 10px;">Description</span>
                                                                                            </th>
                                                                                            <th align="center" class="PaddingL" width="5%">
                                                                                                <span>Quantity</span>
                                                                                            </th>
                                                                                            <th align="center" width="5%">
                                                                                                <span>Rate</span>
                                                                                            </th>
                                                                                            <th align="center" width="5%">
                                                                                                <span>Amount</span>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblDescription" runat="server" CssClass="ClsPaddingL" Text='<%# Eval("Description") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center" class="PaddingL">
                                                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Quantity") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("Rate") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("Amount") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                    <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAlterRow">
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblDescription" runat="server" CssClass="ClsPaddingL" Text='<%# Eval("Description") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center" class="PaddingL">
                                                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Quantity") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("Rate") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("Amount") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:ListView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:LinkButton ID="lnkHide" runat="server" Text="Hide" CausesValidation="false"
                                                                                OnClientClick="HideDescription(this); return false;"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trComment" runat="server">
                                                            <td align="center" colspan="10">
                                                                <table>
                                                                    <tr>
                                                                        <td style="width:100px" class="ClsBorderlight">
                                                                            <span class="ClsLabel">Comment : </span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtComment" runat="server" CssClass="ExLrgTxtBox" Width="400px" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                                                            <span class="ClsMdtStar">*</span>
                                                                            <asp:HiddenField ID="hidOrderId" runat="server" Value="0" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" colspan="2">
                                                                            <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="ClsBtn" OnClick="btnApprove_Click" CausesValidation="false" />
                                                                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="ClsBtn" OnClick="btnReject_Click" CausesValidation="false" />
                                                                            <asp:Button ID="btnCloseComment" runat="server" Text="Close" CausesValidation="false" CssClass="ClsBtn" OnClientClick="HideComment(this); return false;" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                           </td>
                                                        </tr>
                                                    </ItemTemplate>                                                    
                                                    <EmptyDataTemplate>
                                                        <tr style="width: 800px">
                                                            <td align="center" class="LblNoRecord">
                                                                No record Found
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.PODetailsBL" EnablePaging="true" ID="objdsReceiverDetails"
                                                    runat="server" SelectMethod="GetAll" SortParameterName="SortExpression" SelectCountMethod="GetCount"
                                                    EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                        <asp:ControlParameter ControlID="rdCategory" Name="aiIsPO" Type="Int32" PropertyName="SelectedValue" />                                                        
                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="int32" />
                                                        <asp:SessionParameter Name="aiFinancialYearId" SessionField="S_FINANCIAL_YEAR_ID" Type="int32" />

                                                        <asp:ControlParameter ControlID="ddlStatus" Name="aiStatusId" Type="String" PropertyName="SelectedValue" />
                                                        <asp:SessionParameter Name="aiLoginUserId" SessionField="I_USER_ID" Type="int32" />

                                                        <asp:Parameter Name="SortExpression" Type="String" />
                                                        <asp:Parameter Name="SortDirection" Type="String" />
                                                        <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                        <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidGSTData" runat="server" Value="" />
                                    <asp:HiddenField ID="hidData" runat="server" OnValueChanged="hidData_ValueChanged" />
                                    <asp:HiddenField ID="hidPOPrefix" runat="server" Value="" />
                                    <asp:HiddenField ID="hidWoPrefix" runat="server" Value="" />

                                    <asp:HiddenField ID="hidStatusId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                                    <asp:HiddenField ID="hidHasFullAccess" runat="server" Value="" />                                    

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="rdCategory" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        _ClientchkAll = "<%=this.chkAll.ClientID %>";

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }


        function ValidateDescription(oSrc, args) {
            if ($('[id$=txtDescription][value!=""]').length == 0) {
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateAmount(oSrc, args) {
            var isFound = false
            $('[id$=txtDescription][value!=""]').each(function () {
                var index = this.id.replace('ctl00_MainBody_lstvwPODetails_ctrl', '').replace('_txtDescription', '')
                var amt = $('[id$=' + index + '_txtQuantity]').val()

                if (amt == 0 || amt == '') {
                    isFound = true
                }
            })

            if (isFound) {
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateRate(oSrc, args) {
            var isFound = false
            $('[id$=txtDescription][value!=""]').each(function () {
                var index = this.id.replace('ctl00_MainBody_lstvwPODetails_ctrl', '').replace('_txtDescription', '')
                var amt = $('[id$=' + index + '_txtRate]').val()

                if (amt == 0 || amt == '') {
                    isFound = true
                }
            })

            if (isFound) {
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateAmountDescription(oSrc, args) {
            var isFound = false
            $('[id$=txtQuantity][value!=""][value!="0"]').each(function () {
                var index = this.id.replace('ctl00_MainBody_lstvwPODetails_ctrl', '').replace('_txtQuantity', '')
                var amt = $('[id$=' + index + '_txtDescription]').val()

                if (amt == 0 || amt == '') {
                    isFound = true
                }
            })

            if (isFound) {
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateRateDescription(oSrc, args) {
            var isFound = false
            $('[id$=txtRate][value!=""][value!="0"]').each(function () {
                var index = this.id.replace('ctl00_MainBody_lstvwPODetails_ctrl', '').replace('_txtRate', '')
                var amt = $('[id$=' + index + '_txtDescription]').val()

                if (amt == 0 || amt == '') {
                    isFound = true
                }
            })

            if (isFound) {
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }


        function SetTotal(index) {
            var qty = $('[id$=ctrl' + index + '_txtQuantity]').val()
            var rt = $('[id$=ctrl' + index + '_txtRate]').val()
            var gst = $('[id$=ctrl' + index + '_ddlGSTCategory]').val()

            var gstRules = $('[id$=hidGSTData]').val()
            //var gstId = $('[id$=ddlGSTCategory]').val()
            var rules = eval('[' + gstRules + ']')[0]

            var gstRule = rules.filter(function (dt) {
                return dt.Id == gst
            })

           var sum = parseFloat(qty) * parseFloat(rt)
           var amt = (sum / 100 * gstRule[0].Percentage) + sum
            
            if (qty != '' && rt != '' && gst != '')
                $('[id$=ctrl' + index + '_txtAmount]').val(amt)
            else
                $('[id$=ctrl' + index + '_txtAmount]').val('0')

            SetTotalAmount();
        }

        function SetTotalAmount() {

            var gstRules = $('[id$=hidGSTData]').val()
            var gstId = $('[id$=ddlGSTCategory]').val()
            var rules = eval('[' + gstRules + ']')[0]

            var gstRule = rules.filter(function (dt) {
                return dt.Id == gstId
            })

            var totalAmt = 0;
            $('[id$=txtAmount][value!=""][value!="0"]').each(function () {
                totalAmt = totalAmt + parseFloat($(this).val())
            })

            var tax = ((totalAmt * gstRule[0].Percentage) / 100)

            //  alert(tax)

            //   tax = Math.ceil(tax);

            //  alert(tax)


            var finalAmt = totalAmt + tax

            var gst = tax / 2


            finalAmt = Math.round(finalAmt);

            $('[id$=txtTotal]').val(totalAmt)

            $('[id$=txtGSTAmount]').val(tax)
//            $('[id$=txtGST]').val(gstRule[0].Percentage)
            $('[id$=txtGrandTotal]').val(finalAmt)
        }

        function ShowDescription(index) {
            //$('[id$=tr3]').hide(2000);
            $('#ctl00_MainBody_lstvwReceiverDetails_ctrl' + index + '_tr3').show()
        }

        function HideDescription(obj) {
            var id = obj.id.replace('ctl00_MainBody_lstvwReceiverDetails_ctrl', '').replace('_lnkHide', '')
            $('#ctl00_MainBody_lstvwReceiverDetails_ctrl' + id + '_tr3').hide()
        }

        function ResetLabel() {
            $('[id$=LblUpdateSuccess]').html('')
        }

        function OpenReceiverPopup() {
            window.open('POUserDetailsPopup.aspx', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=700')
        }

        function OpenBankPopup() {
            window.open('DepositeBankDetailsPopup.aspx', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=700')
        }

        function RefreshData() {
            var dt = Math.random()
            $('[id$=hidData]').val(dt)
            __doPostBack(document.getElementById("<%=this.hidData.ClientID %>").name, '')
        }

        function OpenReport(index) {
            var str = $('[id$=' + index + '_hidData1]').val()
            window.open('../Admission/AdmissionFormReport.aspx?' + str, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=500,height=150')
        }

        function Increase() {
            UpdateAmount(0.5);
        }

        function UpdateAmount(val) {
            var totalAmt = $('[id$=txtTotal]').val()
            //         var cgst = $('[id$=txtCGST]').val()
            var gst = $('[id$=txtGST]').val()

            //            if (cgst != '') {
            if (gst != '') {
                var finalAmt = $('[id$=txtGrandTotal]').val()

                //                cgst = parseFloat(cgst) + val;
                gst = parseFloat(gst) + val;

                //              finalAmt = parseFloat(totalAmt) + cgst + cgst;
                finalAmt = parseFloat(totalAmt) + gst;

                //              $('[id$=txtCGST]').val(cgst)
                //              $('[id$=txtSGST]').val(cgst)
                $('[id$=txtGST]').val(gst)
                $('[id$=txtGrandTotal]').val(finalAmt)
            }
        }

        function Decrease() {
            UpdateAmount(-0.5);
        }

        function CheckUncheckRow(obj, index) {
            $('[id*=ctrl' + index + '_ChkInstructionList]').prop('checked', obj.checked)
            SetBaseField();
        }

        function SelectAllInstructions(obj) {
            $('[id*=ChkInstructionList]').prop('checked', obj.checked)
            $('[id*=chkAllInstructions]').prop('checked', obj.checked)
        }

        function CheckAllDependancy(index, inst) {
            
            if ($('[id*=ctrl' + index + '_ChkInstructionList_]').length == $('[id*=ctrl' + index + '_ChkInstructionList_]:checked').length)
                inst.checked = true;
            else
                inst.checked = false;

            SetBaseField();
        }

        function SetBaseField() {
            var chkAll = $('[id$=_chkAll]');
            if ($('[id*=_ChkInstructionList_]').length == $('[id*=_ChkInstructionList_]:checked').length)
                chkAll.prop('checked', true)
            else
                chkAll.prop('checked', false)
        }

        function ValidatePoNo(src, args) {
            var poNo = $('#' + '<%=this.txtPONo.ClientID %>').val()            
            if (poNo == '') {
                if ($('[id$=rdCategory_0]').prop('checked'))
                    src.errormessage = 'PO No. should not be blank.'
                else
                    src.errormessage = 'WO No. should not be blank.'

                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidatePoDate(src, args) {
            var poDate = $('#' + '<%=this.txtPODate.ClientID %>').val()
            if (poDate == '') {
                if ($('[id$=rdCategory_0]').prop('checked'))
                    src.errormessage = 'PO Date should not be blank.'
                else
                    src.errormessage = 'WO Date should not be blank.'

                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }


        function ValidateComment(commentId) {            
            var comment = $('#' + commentId).val()

            if (comment == '') {
                alert('Comment should not be blank.');
                return false;
            }
            else if (comment.length > 500) {
                alert('Comment lenght should not be greater than 500 characters.');
                return false;
            }

            return true;
        }

        function HideComment(obj) {
            $('[id$=trComment]').hide();
        }        

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
