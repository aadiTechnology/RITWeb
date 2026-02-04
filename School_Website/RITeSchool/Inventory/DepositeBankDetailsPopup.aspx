<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="DepositeBankDetailsPopup.aspx.cs" Inherits="DepositeBankDetailsPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                    <tr>
                        <td align="center" class="MainTitleHead" style="height: 20px">
                            <span style="font-weight: bold">Deposite Bank Details</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSum" runat="server" DisplayMode="BulletList" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Year should be selected."
                            InitialValue="0" ControlToValidate="cmbYear" Display="None"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Month should be selected."
                            InitialValue="0" ControlToValidate="cmbMonth" Display="None"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Month should not be duplicate."
                            Display="None" OnServerValidate="Month_Validate"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="Month should not be duplicate."
                            Display="None" ClientValidationFunction="ValidateMonth"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Category should be selected."
                            InitialValue="0" ControlToValidate="cmbCategory" Display="None"></asp:RequiredFieldValidator>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Cheque / Txn No. should not be blank."
                            ControlToValidate="txtChequeNo" Display="None"></asp:RequiredFieldValidator>--%>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Cheque No. should not be duplicate."
                            Display="None" OnServerValidate="ChequeNo_Validate"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="Cheque No. should not be duplicate."
                            Display="None" ClientValidationFunction="ValidateChequeNo"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Date should not be blank."
                            ControlToValidate="txtDate" Display="None"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
                <div style="float: right; vertical-align: top;">
                    <span class="ClsMdtStar">* Mandatory Fields</span>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" Style="color: Blue;
                                        font-weight: bold;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width: 150px">
                                    <span class="ClsLabel">Year : </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbYear" runat="server" CssClass="SmlCombo">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <span class="ClsLabel">Month : </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbMonth" runat="server" CssClass="SmlCombo">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <span class="ClsLabel">Category : </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="SmlCombo">
                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Cheque" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Online" Value="5"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <span class="ClsLabel" id="spnCheque">Cheque / Transaction No : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtChequeNo" runat="server" CssClass="MidTxtBox" MaxLength="25"></asp:TextBox>
                                    <%--<span class="ClsMdtStar">*</span>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <span class="ClsLabel">Date : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDate" CssClass="MidTxtBox" runat="server" />
                                    <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Date should not be blank."
                                        AutoPostBack="False" To-Today="true" />
                                    <span class="ClsMdtStar">*</span>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="False"
                            OnClick="btnCancel_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <hr style="border: 1px solid gray; width: 90%; height: 1px; color: Gray;" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td class="ClsBorderlight">
                            <span class="clsLabel">Date : </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateSearch" CssClass="MidTxtBox" runat="server" />
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtDateSearch" Format="dd MMM yyyy"
                                Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Date should not be blank."
                                AutoPostBack="False" To-Today="true" />
                        </td>
                        <td>
                            <span class="clsLabel">Cheque / Txn No. : </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChequeSearch" CssClass="MidTxtBox" runat="server" MaxLength="25" />
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClick="btnSearch_Click"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
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
                                    <asp:ListView ID="lstvwPayments" runat="server" DataKeyNames="Id" OnItemCommand="lstvwPayments_ItemCommand"
                                        OnDataBound="lstvwPayments_DataBound" OnItemDataBound="lstvwPayments_ItemDataBound"
                                        OnSorting="lstvwPayments_Sorting">
                                        <LayoutTemplate>
                                            <table width="90%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="left" class="clsLabelgrd">
                                                        <asp:LinkButton ID="lnkUserName" runat="server" CommandName="Sort" CommandArgument="Month"
                                                            CausesValidation="false" ForeColor="Black"> Month </asp:LinkButton>
                                                    </th>
                                                    <th>
                                                        <span class="clsLabel">Category</span>
                                                    </th>
                                                    <th>
                                                        <span class="clsLabel">Cheque / Transaction No.</span>
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="100px">
                                                        <asp:LinkButton ID="lnkPaymentDate" runat="server" CommandName="Sort" CommandArgument="Date"
                                                            CausesValidation="false" ForeColor="Black" Text="Date"></asp:LinkButton>
                                                    </th>
                                                    <th width="50px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="lblEdit" runat="server" Text="Edit"> </asp:Label>
                                                    </th>
                                                    <th width="50px" class="clsLabelgrd">
                                                        <asp:Label ID="lblDelete" runat="server" Text="Delete"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="6">
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
                                                    <asp:Label ID="lblMonth" runat="server" CssClass="ClsLabel" Text='<%#Eval("Month") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblCategory" runat="server" CssClass="ClsLabel" Text='<%#Eval("Category") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblChequeNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("ChequeNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblPaymentDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
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
                                                    <asp:Label ID="lblMonth" runat="server" CssClass="ClsLabel" Text='<%#Eval("Month") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblCategory" runat="server" CssClass="ClsLabel" Text='<%#Eval("Category") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblChequeNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("ChequeNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblPaymentDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
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
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td class="LblNoRecord" align="center">
                                                    <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:ObjectDataSource TypeName="BusinessLogic.DepositeBankDetailsBL" EnablePaging="True"
                                        ID="objdsPayments" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                        SelectCountMethod="Count" EnableCaching="False">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                            <asp:ControlParameter ControlID="txtDateSearch" Name="asDate" Type="String" PropertyName="Text" />
                                            <asp:ControlParameter ControlID="txtChequeSearch" Name="asChequeNo" Type="String"
                                                PropertyName="Text" />
                                            <asp:Parameter Name="sortExpression" Type="String" />
                                            <asp:Parameter Name="sortDirection" Type="String" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                    <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPayments" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false" OnClientClick="ClosePopup();" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        _clientcmbYear = "<%=this.cmbYear.ClientID %>"
        _clientcmbMonth = "<%=this.cmbMonth.ClientID %>"
        _clientlstvwPayments = "<%=this.lstvwPayments.ClientID %>";
        _clientcmbCategory = "<%=this.cmbCategory.ClientID %>"
        _clienttxtChequeNo = '<%=this.txtChequeNo.ClientID %>'
        _clienthidId = "<%=this.hidId.ClientID %>"

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function SetText(obj) {
            if (obj.value == 2)
                $('#spnCheque').html('Cheque No. :')
            else if (obj.value == 5)
                $('#spnCheque').html('Transaction No. :')
            else
                $('#spnCheque').html('Cheque / Transaction No. :')
        }


        function ValidateMonth(oSrc, args) {
            var month = $('#' + _clientcmbMonth + ' option:selected').text() + '-' + $('#' + _clientcmbYear).val()
            var id = $('#' + _clienthidId).val()
            var isFound = false;
            var k = 0

            var nextMOnth = document.getElementById(_clientlstvwPayments + '_ctrl' + k + '_lblMonth')

            while (nextMOnth != null) {

                var nextId = document.getElementById(_clientlstvwPayments + '_ctrl' + k + '_hidId').value

                if (id != nextId && nextMOnth.innerHTML == month) {
                    isFound = true;
                    break
                }

                k++
                nextMOnth = document.getElementById(_clientlstvwPayments + '_ctrl' + k + '_lblMonth')
            }

            if (isFound) {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }

        }

        function ValidateChequeNo(oSrc, args) {
            var chq = $('#' + _clienttxtChequeNo).val()
            var isFound = false;

            if (chq != '') {
                var category = $('#' + _clientcmbCategory + ' option:selected').text()
                var id = $('#' + _clienthidId).val()

                $('[id$=lblCategory]:contains("' + category + '")').each(function () {

                    var index = this.id.replace(_clientlstvwPayments, '').replace('_ctrl', '').replace('_lblCategory', '')

                    var chqNo = $('#' + _clientlstvwPayments + '_ctrl' + index + '_lblChequeNo').html()
                    var nextId = $('#' + _clientlstvwPayments + '_ctrl' + index + '_hidId').val()

                    if (id != nextId && chq == chqNo) {
                        isFound = true
                        return false
                    }
                })
            }

            if (isFound) {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function ClosePopup() {
            window.close();
        }

    </script>
</asp:Content>
