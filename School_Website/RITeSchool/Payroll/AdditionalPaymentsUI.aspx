<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AdditionalPaymentsUI.aspx.cs" Inherits="AdditionalPaymentsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right" width="150px">
                                <span class="ClsMdtStar">*</span>
                                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <table id="tblBasicLeaves" runat="server">
                        <tr>
                            <td align="right" style="height: 25px" class="ClsGreenBG">
                                <asp:LinkButton ID="lnkPaymentParameter" runat="server" Text="<%$ Resources:LocalizedResources, PaymentParameter%>"
                                    CssClass="SubTitle"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdMessage" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td width="150px" class="ClsBorderlight">
                                        <asp:Label ID="lbl1" runat="server" Text="<%$ Resources:LocalizedResources, AdditionalPaymentDate%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPaymentDate" CssClass="MidTxtBox" runat="server" />
                                        <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtPaymentDate" Format="dd MMM yyyy" Culture = "en"
                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Payment date should not be blank."
                                            AutoPostBack="False" To-Today="true" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150px" class="ClsBorderlight">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, BankName%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbBank" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                            OnSelectedIndexChanged="cmbBank_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150px" class="ClsBorderlight">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, AccountNo%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upnl5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="cmbAccountNo" runat="server" CssClass="LrgCombo">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">* </span>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbBank" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150px" class="ClsBorderlight">
                                        <asp:Label ID="lbl2" runat="server" Text="<%$ Resources:LocalizedResources, PaymentParameter%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbParameter" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150px" class="ClsBorderlight">
                                        <asp:Label ID="lbl3" runat="server" Text="<%$ Resources:LocalizedResources, StaffGroup%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbStaffGroup" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                            OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150px" class="ClsBorderlight">
                                        <asp:Label ID="lbl4" runat="server" Text="<%$ Resources:LocalizedResources, StaffName%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upnlStaffName" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="cmbStaffName" runat="server" CssClass="LrgCombo">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">* </span>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150px" class="ClsBorderlight">
                                        <asp:Label ID="lbl5" runat="server" Text="<%$ Resources:LocalizedResources, Amount%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="MidTxtBox" MaxLength="7" Style="text-align: right;
                                            padding-right: 5px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                            onkeypress="return blockNonNumbers(this, event, false, false);" onkeyup="extractNumber(this,2,false);"
                                            onpaste="event.returnValue=false"></asp:TextBox>
                                        <span class="ClsMdtStar">* </span>
                                        <asp:HiddenField ID="hidPaymentId" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnlBtns" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>                    
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                            CssClass="ClsBtn" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                            CssClass="ClsBtn" OnClick="btnCancel_Click" CausesValidation="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                    </Triggers>
                    </asp:UpdatePanel>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" Display="None" ErrorMessage=""
                        SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidatePaymentDate"> </asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, valBankNameSelection%>"
                        Display="None" ControlToValidate="cmbBank" InitialValue="0"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, valAccountNoSelection%>"
                        Display="None" ControlToValidate="cmbAccountNo" InitialValue="0"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, valParameterSelection%>"
                        Display="None" ControlToValidate="cmbParameter" InitialValue="0"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, valStaffGroupSelection%>"
                        Display="None" ControlToValidate="cmbStaffGroup" InitialValue="0"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, valStaffNameSelection%>"
                        Display="None" ControlToValidate="cmbStaffName" InitialValue="0"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                        ClientValidationFunction="ValidateAmount"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr>
                            <td>
                                <hr style="border: thin solid #C0C0C0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight">
                                <asp:Label ID="lblSearch" runat="server" CssClass="ClsLabel" Text="User Name / Payment Parameter :"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search%>"
                                    CssClass="ClsBtn" OnClick="btnSearch_Click" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="80%">
                                <tr runat="server" id="trTotalRec" align="center">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwPayments">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwPayments" runat="server" DataKeyNames="Id" OnDataBound="lstvwPayments_DataBound"
                                            OnItemDataBound="lstvwPayments_ItemDataBound" OnItemCommand="lstvwPayments_ItemCommand"
                                            OnSorting="lstvwPayments_Sorting">
                                            <LayoutTemplate>
                                                <table width="90%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" class="clsLabelgrd">
                                                            <asp:LinkButton ID="lnkUserName" runat="server" CommandName="Sort" CommandArgument="UserName"
                                                                CausesValidation="false" ForeColor="Black"> User Name (Designation) </asp:LinkButton>
                                                        </th>
                                                        <th align="center" class="clsLabelgrd" width="150px">
                                                            <asp:LinkButton ID="lnkPaymentDate" runat="server" CommandName="Sort" CommandArgument="PaymentDate"
                                                                CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, AdditionalPaymentDate %>"></asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="150px" class="clsLabelgrd">
                                                            <asp:LinkButton ID="lnkParameter" runat="server" CommandName="Sort" CommandArgument="Parameter"
                                                                CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, PaymentParameter%>"></asp:LinkButton>
                                                        </th>
                                                        <th align="right" class="clsLabelgrd" width="100px">
                                                            <asp:LinkButton ID="lnkAmount" runat="server" CommandName="Sort" CommandArgument="Amount"
                                                                CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, Amount %>"></asp:LinkButton>
                                                        </th>
                                                        <th width="50px" align="center" class="clsLabelgrd">
                                                            <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                        </th>
                                                        <th width="50px" class="clsLabelgrd">
                                                            <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                        <td colspan="8">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwPayments" PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
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
                                                    <td align="left">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPaymentDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblParameter" runat="server" CssClass="ClsLabelL" Text='<%#Eval("Parameter") %>'></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabelL" Style="float: inherit;
                                                            padding-right: 5px;" Text='<%#Eval("Amount") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblPaymentDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblParameter" runat="server" CssClass="ClsLabelL" Text='<%#Eval("Parameter") %>'></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabelL" Style="float: inherit;
                                                            padding-right: 5px;" Text='<%#Eval("Amount") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>"/>
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
                                        <asp:ObjectDataSource TypeName="BusinessLogic.AdditionalPaymentBL" EnablePaging="True"
                                            ID="objdsPayments" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                            SelectCountMethod="Count" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiFinancialYearId" SessionField="S_FINANCIAL_YEAR_ID"
                                                    Type="int32" />
                                                <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                <asp:Parameter Name="sortExpression" Type="String" />
                                                <asp:Parameter Name="sortDirection" Type="String" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                        <asp:HiddenField ID="hidmsgConfirmDelete" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:HiddenField ID="hidPageNo" runat="server" />
                    <asp:HiddenField ID="hidServerDate" runat="server" />                    
                    <asp:HiddenField ID="hidvalnonZeroAmount" runat="server" />
                    <asp:HiddenField ID="hidvalBlankPaymentDate" runat="server" />
                    <asp:HiddenField ID="hidvalFuturePaymentDate" runat="server" />
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">
            _clientTxtAmount = "<%=this.txtAmount.ClientID %>";
            _clientTxtPaymentDate = "<%=this.txtPaymentDate.ClientID %>"
            _clientServerDate = "<%=this.hidServerDate.ClientID %>";
            _clienthidmsgConfirmDelete = "<%=this.hidmsgConfirmDelete.ClientID %>"
            _clienthidvalnonZeroAmount = "<%=this.hidvalnonZeroAmount.ClientID %>"
            _ClienthidvalBlankPaymentDate = "<%=this.hidvalBlankPaymentDate.ClientID %>"
            _clienthidvalFuturePaymentDate = "<%=this.hidvalFuturePaymentDate.ClientID %>"

            function ConfirmDelete() {
                return confirm($get(_clienthidmsgConfirmDelete).value)
            }

            function OpenPopup() {
                window.open('PaymentParameterPopup.aspx?', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=650')
            }

            function ValidateAmount(oSrc, args) {
                var amount = $get(_clientTxtAmount).value;
                if (amount.trim() == "" || parseInt(amount.trim()) == 0) {
                    oSrc.errormessage = $get(_clienthidvalnonZeroAmount).value;
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function ValidatePaymentDate(oSrc, args) {
                var bIsValid = true;
                var dtPaymentDate = $get(_clientTxtPaymentDate);
                dtPaymentDate.value = dtPaymentDate.value.trim();
                if (dtPaymentDate.value == "") {
                    oSrc.errormessage = $get(_ClienthidvalBlankPaymentDate).value;
                    bIsValid = false;
                }
                else if (dtPaymentDate.value != "") {
                    var serverDate = $get(_clientServerDate).value;
                    dtStartDate = new Date(convertvaliddate2(dtPaymentDate.value));
                    var today = new Date(convertvaliddate2(serverDate));    
                    if (today < dtStartDate) {
                        oSrc.errormessage = $get(_clienthidvalFuturePaymentDate).value;
                        bIsValid = false;
                    }
                }
                args.IsValid = bIsValid;
                return !bIsValid;
            }

            function ClearMessage() {
                $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
