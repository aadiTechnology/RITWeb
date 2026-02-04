<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="IssueRenewReturnUI.aspx.cs" Inherits="IssueRenewReturnUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<script runat="server">
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
<style type="text/css">
       .show_late_feepopup 
        {
			position:absolute;
			left: 50%;
			top: 50%;
			border:1px solid black;
			background-color: lightyellow;
			font-family: Tahoma;
		}
		.show_late_feecontent
		{
			padding: 15px;
			text-align: left;
			vertical-align: top;
			overflow: auto;
		}
		.show_late_feeoverlay
		{
			position:fixed;
			height: 100%;
			width: 100%;
			background: transparent;
			opacity: .15;
			filter: alpha(opacity=15);
			-moz-opacity: .15;
			z-index: 1001;
			display: none;
		}		
</style>
    <script src="../Scripts/jquery-1.3.2-vsdoc2.js" type="text/javascript"></script>
    <div class="MainBodyDiv" style="width: 1200px">
        <table width="90%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td valign="top">
                    <table width="100%" align="center" border="0" cellspacing="0" cellpadding="0">
                        <tr align="center">
                            <td align="center">
                                <table align="center" width="100%">
                                    <tr align="center">
                                        <td align="center" style="width: 65%">
                                            <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <table align="center" style="width: 950px;">
                                                        <tr>
                                                            <td align="right" style="width: 23%; padding-right: 150px" valign="top">
                                                                <span class="ClsMdtStar">* Mandatory Fields</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <!--lblError label insert here-->
                                                                <asp:ValidationSummary CssClass="LblErrorMsg" ID="valSumErrorMsg" runat="server"
                                                                    ShowMessageBox="False" ShowSummary="true" EnableViewState="false" ValidationGroup="Search" />
                                                                <asp:ValidationSummary CssClass="LblErrorMsg" ID="valSumErrorMessage" runat="server"
                                                                    ShowMessageBox="False" ShowSummary="true" EnableViewState="false" ValidationGroup="Return" />
                                                            </td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td>
                                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="False"
                                                                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwUsers" EventName="ItemCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnReturnBook" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="chkShowDeactiveUser" EventName="CheckedChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnIssue" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trSearch">
                            <td align="center" colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="2" align="center" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <table width="750px">
                                                        <tr>
                                                            <td style="width: 750px" align="left">
                                                                <span class="ClsLblLgnd" style="font-weight: bold">Search for users :</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table width="750px">
                                                        <tr>
                                                            <td style="vertical-align: top; width: 750px" align="right">
                                                                <table style="width: 100%; vertical-align: top;" align="center">
                                                                    <tr>
                                                                        <td class="ClsBorderlight paddingL" style="width: 190px">
                                                                            <span id="Span5" class="ClsLabel" style="width: 130">User Role :</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="SmlTxtBox" Width="129px"
                                                                                AutoPostBack="True" OnSelectedIndexChanged="cmbUserRole_SelectedIndexChanged">
                                                                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <span class="ClsMdtStar" style="color: #ff0000">&nbsp;*</span>
                                                                        </td>
                                                                        <td class="ClsBorderlight paddingL" style="width: 190px">
                                                                            <span id="Span13" class="ClsLabel" style="width: 100px">Name :</span>
                                                                        </td>
                                                                        <td align="left" style="width: 170px">
                                                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="SmlTxtBox" MaxLength="100" autocomplete="off"
                                                                                onkeypress="ClearControls(this)" Width="129px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trClassRollNo" runat="server">
                                                                        <td class="ClsBorderlight paddingL">
                                                                            <span id="Span12" class="ClsLabel" style="width: 100px;">Class :</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="cmbClass" runat="server" CssClass="SmlTxtBox" Width="129px"
                                                                                Enabled="false">
                                                                            </asp:DropDownList>
                                                                            <span class="ClsMdtStar" style="color: #ff0000" id="spnStar" runat="server" visible="false">
                                                                                &nbsp;*</span>
                                                                        </td>
                                                                        <td class="ClsBorderlight paddingL">
                                                                            <span id="spnRollNoOrEmpNo" runat="server" class="ClsLabel" style="width: 100px">Employee
                                                                                No. :</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtRollNoOrEmpNo" runat="server" CssClass="SmlTxtBox" Width="129px"
                                                                                onkeypress="ClearControls(this)"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="ClsBorderlight paddingL">
                                                                            <span id="Span14" class="ClsLabel" style="width: 140px">User Barcode :</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtBarcode" runat="server" CssClass="SmlTxtBox" onkeypress="ClearControls(this)"
                                                                                Width="129px"></asp:TextBox>
                                                                        </td>
                                                                        <td class="ClsBorderlight paddingL">
                                                                            <span id="Span2" class="ClsLabel" style="width: 159px">Show De-activated Users :</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkShowDeactiveUser" runat="server" CssClass="LblNormal" Width="129px">
                                                                            </asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4" align="center">
                                                                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Font-Bold="True" Text="Search"
                                                                                OnClick="btnSearch_Click" ValidationGroup="Search" />
                                                                            <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Font-Bold="True" Text="Cancel"
                                                                                CausesValidation="false" UseSubmitBehavior="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:CustomValidator runat="server" ID="cstUserRoleValidator" ErrorMessage="User role should be selected."
                                                        ClientValidationFunction="ValiDateUserRoleSelection" CssClass="LblNormal" Display="None"
                                                        ValidationGroup="Search"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trUserListView" runat="server">
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:CustomValidator runat="server" ID="cstMaxIssueBookValidation" Display="None"
                                            ValidationGroup="Search"></asp:CustomValidator>
                                        <table width="100%">
                                            <tr id="trLegend" runat="server" visible="false">
                                                <td id="Td1" align="left" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                    Text="Legend: " EnableViewState="False"></asp:Label>
                                                            </td>
                                                            <td align="center" style="border: 1px solid #000000;" valign="middle">
                                                                <asp:Label ID="Label1" runat="server" BackColor="Pink" BorderStyle="None" BorderWidth="1px"
                                                                    CssClass="ClsLblLgnd" EnableViewState="False" Font-Bold="False" ForeColor="Black"
                                                                    ReadOnly="True" Text="Late Return" Width="110px"></asp:Label>
                                                            </td>
                                                            <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                                <asp:Label ID="Label4" runat="server" BackColor="Gainsboro" CssClass="ClsLblLgnd"
                                                                    Text="Deactivated User" ForeColor="Red" Font-Bold="False" BorderStyle="None"
                                                                    BorderWidth="1px" ReadOnly="True" Width="120px" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trBookAssignment" runat="server" visible="false" style="text-align:center; margin:0px auto;" align="center">
                                                <td style="text-align:center; margin:0px auto;" align="center">
                                                    <asp:RadioButton ID="rdoSingleBook" runat="server" AutoPostBack="true"
                                                        Text="Single Assignment" GroupName="BookAssignmet" 
                                                        oncheckedchanged="rdoSingleBook_CheckedChanged" />
                                                    <asp:RadioButton ID="rdoBulkBook" runat="server" Text="Bulk Assignment"  AutoPostBack="true"
                                                        GroupName="BookAssignmet" oncheckedchanged="rdoBulkBook_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr id="trPagerUser" runat="server">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwUsers">
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
                                                <td>
                                                    <asp:ListView ID="lstvwUsers" runat="server" DataKeyNames="UserId,StandardDivisionId,IsActive,BookwiseRenewCount,UserIssueBookCount,HasLateEntry"
                                                        OnItemDataBound="lstvwUsers_ItemDataBound" OnDataBound="lstvwUsers_DataBound"
                                                        OnItemCommand="lstvwUsers_ItemCommand">
                                                        <LayoutTemplate>
                                                            <table cellpadding="0" cellspacing="1" align="center" width="100%" runat="server"
                                                                id="tblShiftInfo" style="color: #333333" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th id="thSrNo" align="center" style="width: 80px">
                                                                        <asp:Label runat="server" ID="lblSrNo" CssClass="LblNormal" Text="Sr. No."></asp:Label>
                                                                    </th>
                                                                     <th id="thGrNo" align="center" style="width: 100px">
                                                                        <asp:Label runat="server" ID="Label2" CssClass="LblNormal" Text="GR. No"></asp:Label>
                                                                    </th>
                                                                      <th id="thRollNo" align="center" style="width: 100px">
                                                                        <asp:Label runat="server" ID="lblRollNoEmployeeNo" CssClass="LblNormal" Text="Roll No."></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="paddingLSML" style="width: 250px">
                                                                        Name
                                                                    </th>
                                                                    <th align="left" style="width: 170px;">
                                                                        <asp:Label runat="server" ID="lblDesignationClass" CssClass="LblNormal" Text="Class"></asp:Label>
                                                                    </th>
                                                                    <th align="center" style="width: 180px">
                                                                        Accession No. / Barcode
                                                                    </th>
                                                                    <th id="thBookIssue" runat="server" align="center">
                                                                        Issue
                                                                    </th>
                                                                    <th id="thBookRenew" runat="server" align="center">
                                                                        Renew
                                                                    </th>
                                                                    <th id="thBookReturn" runat="server" align="center">
                                                                        Return
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                    <td colspan="8">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwUsers" PageSize="50">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td align="left">
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
                                                            <tr id="trUser" runat="server" class="ClsGridRow">
                                                                <td id="tdSrNo" align="center" runat="server">
                                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("RowNo") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidHasLateEntry" runat="server" Value="0" />
                                                                    <asp:HiddenField ID="hidIssueCount" runat="server" Value="0" />
                                                                </td>
                                                                 <td id="tdEnrollmentNo" align="center" runat="server">
                                                                    <asp:Label ID="lblEnrollmentNo" runat="server" Text='<%# Eval("EnrollmentNo") %>'></asp:Label>
                                                                </td>
                                                                <td id="td2" align="center" runat="server">
                                                                    <asp:Label ID="lblRollNoEmployeeNo" runat="server" Text='<%# Eval("IssueBookUserRollNoDesig.RollNoEmployeeNo") %>'></asp:Label>
                                                                </td>
                                                                <td id="tdUSerName" align="left" class="paddingLSML" runat="server">
                                                                    <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                                </td>
                                                                <td id="tdDesignationClass" align="left" class="paddingLSML">
                                                                    <asp:Label ID="lblDesignationClass" runat="server" Text='<%# Eval("IssueBookUserRollNoDesig.ClassNameDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td align="center" id="tdAccessionNoBarcode">
                                                                    <asp:TextBox ID="txtAccessionNoBarcode" MaxLength="10" runat="server"></asp:TextBox>
                                                                    <asp:HiddenField ID="hidUserIssueBookCount" runat="server" Value='<%# Eval("UserIssueBookCount") %>' />
                                                                    <asp:HiddenField ID="hidUserID" runat="server" Value='<%# Eval("UserId") %>' />
                                                                </td>
                                                                <td id="tdImgbtnIssue" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgbtnIssue" runat="server" ToolTip="Issue Book" CommandName="ISSUE" 
                                                                        ImageUrl="~/RITeSchool/images/AddressBook.gif" />
                                                                </td>
                                                                <td id="tdimgbtnRenew" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgbtnRenew" runat="server" ToolTip="Renew Book" CommandName="RENEW"
                                                                        ImageUrl="~/RITeSchool/images/book_Renew_2.gif" />
                                                                </td>
                                                                <td id="tdimgbtnReturn" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgbtnReturn" runat="server" ToolTip="Return Book" CommandName="RETURN"
                                                                        ImageUrl="~/RITeSchool/images/book_submit_2.gif" />
                                                                    <asp:HiddenField ID="hidUserBookRenewDetails" runat="server" Value='<%# Eval("BookwiseRenewCount") %>' />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="trUser" runat="server" class="ClsGridAltRow">
                                                                <td id="tdSrNo" align="center" runat="server">
                                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("RowNo") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidHasLateEntry" runat="server" Value="0" />
                                                                    <asp:HiddenField ID="hidIssueCount" runat="server" Value="0" />
                                                                </td>
                                                                <td id="tdEnrollmentNo" align="center" runat="server">
                                                                    <asp:Label ID="lblEnrollmentNo" runat="server" Text='<%# Eval("EnrollmentNo") %>'></asp:Label>
                                                                </td>
                                                                <td id="td3" align="center" runat="server">
                                                                    <asp:Label ID="lblRollNoEmployeeNo" runat="server" Text='<%# Eval("IssueBookUserRollNoDesig.RollNoEmployeeNo") %>'></asp:Label>
                                                                </td>
                                                                <td id="tdUSerName" align="left" class="paddingLSML" runat="server">
                                                                    <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                                </td>
                                                                <td id="tdDesignationClass" align="left" class="paddingLSML">
                                                                    <asp:Label ID="lblDesignationClass" runat="server" Text='<%# Eval("IssueBookUserRollNoDesig.ClassNameDesignation") %>'></asp:Label>
                                                                </td>
                                                                <td align="center" id="tdAccessionNoBarcode">
                                                                    <asp:TextBox ID="txtAccessionNoBarcode" MaxLength="10" runat="server" onkeypress="return CheckEnteredChar(event);"></asp:TextBox>
                                                                    <asp:HiddenField ID="hidUserIssueBookCount" runat="server" Value='<%# Eval("UserIssueBookCount") %>' />
                                                                    
                                                                    <asp:HiddenField ID="hidUserID" runat="server" Value='<%# Eval("UserId") %>' />
                                                                </td>
                                                                <td id="tdImgbtnIssue" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgbtnIssue" runat="server" ToolTip="Issue Book" CommandName="ISSUE" 
                                                                        ImageUrl="~/RITeSchool/images/AddressBook.gif" />
                                                                </td>
                                                                <td id="tdimgbtnRenew" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgbtnRenew" runat="server" ToolTip="Renew Book" CommandName="RENEW"
                                                                        ImageUrl="~/RITeSchool/images/book_Renew_2.gif" />
                                                                </td>
                                                                <td id="tdimgbtnReturn" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgbtnReturn" runat="server" ToolTip="Return Book" CommandName="RETURN"
                                                                        ImageUrl="~/RITeSchool/images/book_submit_2.gif" />
                                                                    <asp:HiddenField ID="hidUserBookRenewDetails" runat="server" Value='<%# Eval("BookwiseRenewCount") %>' />
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                        No Records Found.
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                            <tr id="trBulkStudentsButton" runat="server" visible="false" align="center" style="text-align:center; margin:0px auto;">
                                                <td colspan="4" align="center" style="text-align:center; margin:0px auto;">
                                                     <asp:Button ID="btnIssue" runat="server" CssClass="ClsBtn" Font-Bold="True"
                                                         Text="Issue" onclick="btnIssue_Click" />
                                                     <asp:Button ID="btnRenew" runat="server" CssClass="ClsBtn" Font-Bold="True" 
                                                         Text="Renew" CausesValidation="false" UseSubmitBehavior="false" 
                                                         onclick="btnRenew_Click" />
                                                     <asp:Button ID="btnReturn" runat="server" CssClass="ClsBtn" Font-Bold="True" 
                                                         Text="Return" CausesValidation="false" onclick="btnReturn_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:HiddenField ID="hidMaxIssueBookCount" runat="server" />
                                                    <asp:HiddenField ID="hidMaxRenewBookCount" runat="server" />
                                                    <asp:HiddenField ID="hidUserBookRenewDetails" runat="server" />
                                                    <asp:HiddenField ID="hidLateFeePerDay" runat="server" />
                                                    <asp:HiddenField ID="hidLateFeeEffectiveFrom" runat="server" />
                                                    <asp:HiddenField ID="hidReturnDate" runat="server" />
                                                    <asp:HiddenField ID="hidIssueDate" runat="server" />
                                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                                    <asp:HiddenField ID="hidBookNo" runat="server" />
                                                    <asp:HiddenField ID="hidActReturnDate" runat="server" />
                                                    <asp:HiddenField ID="hidLateFeeAmt" runat="server" />
                                                    <asp:HiddenField ID="hidCommandName" runat="server" />
                                                    <asp:HiddenField ID="hidtxtAccessionOrBarcode" runat="server" />
                                                    <asp:HiddenField ID="hidRowNo" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hidBookName" runat="server" />
                                                    <asp:HiddenField ID="hidBookReserveUserList" runat="server" />
                                                    <asp:HiddenField ID="hidPageNo" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hidShowLateValidation" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnReturnBook" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnLateFee" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="chkShowDeactiveUser" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divMain" runat="server" class="show_late_feeoverlay"  style="visibility: hidden; display: none;">
                    </div>
                    <div id="updtpnlPopUp" class="show_late_feepopup" runat="server" style="z-index: 5000;
		                    width: 250px; height: 150px; margin: -160px -190px 0 -40px; background-color: white;
		                    visibility: hidden; display: none;">
                        <div  style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;
                            margin-bottom: 3px;">
                            <div style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left;">
                                Book Return Date</div>
                            <span style="cursor: hand" onclick="javascript:HidePopup();">
                                <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                            </span>
                        </div>
                        <div style="padding:5px; margin-top:-1px; text-align: left;"  class="show_late_feecontent" class="ClsLabel">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel4" ChildrenAsTriggers="True" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <caption>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSchoolleaving" runat="server" CssClass="LblNormal" Text="Return date :" />
                                                    <asp:TextBox ID="txtReturnDate" runat="server" CssClass="SmlCombo" MaxLength="11"></asp:TextBox>
                                                    <rjs:PopCalendar ID="caltxtReturnDate" runat="server" Control="txtReturnDate" Format="dd MMM yyyy"
                                                        Separator="-" ShowErrorMessage="false" ShowWeekend="True" To-Today="true" />
                                                </td>
                                                <td>
                                                    <span style="color: #ff0000">*</span>
                                                    <asp:CustomValidator ID="custReturnDate" runat="server" ClientValidationFunction="IsValidReturnDate"
                                                                    CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ErrorMessage="Return date should not be blank."
                                                                    ValidationGroup="Return" Visible="true"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblLateFee" runat="server" CssClass="LblNormal" Text="Late Fee :"></asp:Label>&nbsp;&nbsp;&nbsp&nbsp
                                                    <asp:TextBox ID="txtLateFee" runat="server" CssClass="SmlCombo" MaxLength="3" onblur="extractNumber(this,0,false);"
                                                        ondrop="event.returnValue=false;" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false;" Width="100px"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 25px;">
                                                    <asp:Button ID="btnReturnBook" runat="server" CssClass="ClsBtn" OnClick="btnReturnBook_Click"
                                                        Text="OK" ValidationGroup="Return" />
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                                        OnClientClick="javascript:HidePopup();return false;" Text="Cancel" />
                                                </td>
                                            </tr>
                                        </caption>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lstvwUsers" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td id="tdLateFee" runat="server">
                    <div id="divMainLateFee" runat="server" class="show_late_feeoverlay" >
                    </div>
                    <div id="DivLateFeeAmt" class="show_late_feepopup" runat="server" style="z-index: 5000;
		                    width: 250px; height: 130px; margin: -160px -190px 0 -100px; background-color: white; position:fixed;
		                    visibility: hidden; display: none;">
                            <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                                background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                                <div style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left;">
                                    Book Return Date</div>
                                <span style="cursor: hand" onclick="javascript:HideLateFeePopup();">
                                    <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                </span>
                            </div>
                        <div style="font-family: Times New Roman; font-size: large;
			                  font-weight: bold; color: #333;" class="show_late_feecontent" >
                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                ID="UpdatePanel5">
                                <ContentTemplate>
                                    <table width="200px">
                                        <tr align="center">
                                            <td>
                                                <asp:Label ID="lblLateFeeAmt" runat="server" Text="Late Fee Amount :" Font-Size="9"
                                                    ForeColor="#000333" />
                                                <asp:TextBox ID="txtAmt" CssClass="SmlCombo" runat="server" Height="20px" Width="40%"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;"></asp:TextBox>
                                                 
                                            </td>
                                        </tr>
                                        <br />
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnLateFee" runat="server" Text="OK" CssClass="ClsBtn" OnClientClick="javascript:HideLateFeePopup();"
                                                    OnClick="btnLateFee_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                               
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientlstvwUsers = "<%= this.lstvwUsers.ClientID %>"
        _clientcmbUserRole = "<%= this.cmbUserRole.ClientID %>"
        _clientcmbClass = "<%= this.cmbClass.ClientID %>"
        _clienttxtUserName = "<%= this.txtUserName.ClientID %>"
        _clienttxtRollNoOrEmpNo = "<%= this.txtRollNoOrEmpNo.ClientID %>"
        _clienttxtBarcode = "<%= this.txtBarcode.ClientID %>"
        _clientcstUserRoleValidator = "<%= this.cstUserRoleValidator.ClientID %>"
        _clienthidMaxIssueBookCount = "<%= this.hidMaxIssueBookCount.ClientID %>"
        _clientcstMaxIssueBookValidation = "<%= this.cstMaxIssueBookValidation.ClientID %>"
        _clientLblErrorMsg = "<%= this.lblErrorMsg.ClientID %>"
        _clientlblUpdateSucess = "<%= this.lblUpdateSucess.ClientID %>"
        _clienttxtReturnDate = "<%=this.txtReturnDate.ClientID %>"
        _clientCstValRetDate = "<%=this.custReturnDate.ClientID %>"
        _clienthidIssueDate = "<%=this.hidIssueDate.ClientID %>"
        _clienthidCommandName = "<%=this.hidCommandName.ClientID %>"
        _clienthidRowNo = "<%=this.hidRowNo.ClientID %>"
        _clientchkShowDeactiveUser = "<%=this.chkShowDeactiveUser.ClientID %>"
        _clientvalSumErrorMsg = "<%=this.valSumErrorMsg.ClientID %>"
        _clientvalSumErrorMessage = "<%=this.valSumErrorMessage.ClientID %>"
        _clienthidMaxRenewBookCount = "<%=this.hidMaxRenewBookCount.ClientID %>"
        _clientUpdatePanel = "<%=this.UpdatePanel12.ClientID %>"
        _clienttxtLateFee = "<%=this.txtLateFee.ClientID %>"
        _clientlblLateFee = "<%=this.lblLateFee.ClientID %>"
        _clienttxtAmt = "<%=this.txtAmt.ClientID %>"
        _clienthidUserId = "<%= this.hidUserId.ClientID %>>"
        _clienthidtxtAccessionOrBarcode = "<%=this.hidtxtAccessionOrBarcode.ClientID %>"
        _clientOption = "<%=this.rdoBulkBook.ClientID %>"
        _clienthidShowLateValidation = "<%=this.hidShowLateValidation.ClientID %>"
        
        function ShowLateFeePopup(e,sLateFee, iUserId, iAccessBarcode, sRenewalDetails) {
            
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.DivLateFeeAmt.ClientID %>").style
            document.getElementById(_clienttxtReturnDate).value = ''
            var now = new Date()
            //$get("<%=this.hidBookNo.ClientID %>").value = iBookId
            $get("<%=this.hidUserId.ClientID %>").value = iUserId
            $("#divMain").show();
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
            document.getElementById(_clienttxtAmt).value = sLateFee;
            document.getElementById(_clienthidtxtAccessionOrBarcode).value = iAccessBarcode;
           
           }
        function HideLateFeePopup() {
            
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            $get("<%=this.DivLateFeeAmt.ClientID %>").style.visibility = "hidden"
            $get("<%=this.DivLateFeeAmt.ClientID %>").style.display = "none"
            var iLateFeeAmt = document.getElementById(_clienttxtAmt).value
            $get("<%=this.hidLateFeeAmt.ClientID %>").value = iLateFeeAmt
            var cssstyleMain = $get("<%=this.DivLateFeeAmt.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            return false
        }
        function ClearControls(obj) {
            if ("<%= this.txtBarcode.ClientID %>" == obj.id) {
                document.getElementById(_clienttxtUserName).value = "";
                document.getElementById(_clienttxtRollNoOrEmpNo).value = "";
            }
            else {
                document.getElementById(_clienttxtBarcode).value = "";
            }
        }

        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }


        function ClearAllControl() {
            document.getElementById(_clienttxtUserName).value = "";
            document.getElementById(_clienttxtRollNoOrEmpNo).value = "";
            document.getElementById(_clienttxtBarcode).value = "";
            document.getElementById(_clientchkShowDeactiveUser).checked = false;
            ClearMessages();
            return false;
        }
        function ClearMessages() {
            if ((document.getElementById(_clientLblErrorMsg) != null) && (document.getElementById(_clientLblErrorMsg) != "undefined")) {
                document.getElementById(_clientLblErrorMsg).innerHTML = ""
                document.getElementById(_clientLblErrorMsg).innerText = "";
            }
            if ((document.getElementById(_clientlblUpdateSucess) != null) && (document.getElementById(_clientlblUpdateSucess) != "undefined")) {
                document.getElementById(_clientlblUpdateSucess).innerHTML = ""
                document.getElementById(_clientlblUpdateSucess).innerText = "";
            }
            if ((document.getElementById(_clientvalSumErrorMessage) != null) && (document.getElementById(_clientvalSumErrorMessage) != "undefined")) {
                document.getElementById(_clientvalSumErrorMessage).innerHTML = ""
                document.getElementById(_clientvalSumErrorMessage).innerText = "";
            }
            if ((document.getElementById(_clientvalSumErrorMsg) != null) && (document.getElementById(_clientvalSumErrorMsg) != "undefined")) {
                document.getElementById(_clientvalSumErrorMsg).innerHTML = ""
                document.getElementById(_clientvalSumErrorMsg).innerText = "";
            }
        }
        function ValiDateUserRoleSelection(oSrc, args) {
            ClearMessages();
            var UserRole = document.getElementById(_clientcmbUserRole)
            var txtbarcode = document.getElementById(_clienttxtBarcode).value;
            if (UserRole.value != "0") {
                if ((UserRole.value == "3" || UserRole.value == "9") && txtbarcode == "") {
                    if (document.getElementById(_clientcmbClass).value == "0") {
                        oSrc.errormessage = "Class should be selected.";
                        document.getElementById(_clientcstUserRoleValidator).errormessge = "Class should be selected.";
                        args.IsValid = false;
                        return true;
                    }
                }
            }
            else {
                oSrc.errormessage = "User role should be selected.";
                document.getElementById(_clientcstUserRoleValidator).errormessge = "User role should be selected.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        function ConfirmRenewBook(e, iRowId, iSchoolId, sLateFee, iUserId) {
          
            ClearMessages();
            var sRenewDetails =document.getElementById(_clientlstvwUsers + "_ctrl" + iRowId + "_hidUserBookRenewDetails").value;
            var MaxRenewBookCount = document.getElementById(_clienthidMaxRenewBookCount).value;
            var txtValue = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowId + "_txtAccessionNoBarcode").value;
            if (txtValue == "") {
                document.getElementById(_clientLblErrorMsg).innerText = "Accession No. / Barcode should not be blank.";
                document.getElementById(_clientLblErrorMsg).innerHTML = "Accession No. / Barcode should not be blank.";
                return false;
            }
            var iAccBarcodevalue = txtValue;
            if (sRenewDetails != "") {
                var sArray = sRenewDetails.split(',');
                for (var iarrIndex = 0; iarrIndex < sArray.length; iarrIndex++) {
                    var AccessionNo = sArray[iarrIndex].substring(0, sArray[iarrIndex].indexOf("-")).replace("&","'");
                    var subStr = sArray[iarrIndex].substring(sArray[iarrIndex].indexOf("-") + 1, sArray[iarrIndex].length);
                    var RenewCount = subStr.substring(0, subStr.indexOf("-"));
                    var sRenwAttempt = parseInt(RenewCount) + 1;
                    var BookDetailId = sArray[iarrIndex].substring(sArray[iarrIndex].lastIndexOf("-") + 1, sArray[iarrIndex].length);
                    if (AccessionNo == txtValue || BookDetailId == txtValue.substring(1, txtValue.length - 3)) {
                        if (MaxRenewBookCount == 0 || MaxRenewBookCount == "") {
                            document.getElementById(_clientLblErrorMsg).innerText = "Not allow to renew this book : " + AccessionNo + ". Please return this book.";
                            document.getElementById(_clientLblErrorMsg).innerHTML = "Not allow to renew this book : " + AccessionNo + ". Please return this book.";
                            return false;
                        }
                        else if (RenewCount < MaxRenewBookCount) {
                           if(BookDetailId!="") {
                            var sMassge = "Book renew attempt - #" + sRenwAttempt + ". Are you sure you want to renew this book?";
                            if (!window.confirm(sMassge)) {
                                return false;
                            }
                            else {
                                document.getElementById(_clienthidRowNo).value = iRowId;
                                if (sLateFee != 0 && sLateFee != "") {
                                    document.getElementById(_clientlstvwUsers + "_ctrl" + iRowId + "_txtAccessionNoBarcode").value = "";
                                    ShowLateFeePopup(e,sLateFee, iUserId, iAccBarcodevalue, sRenewDetails);
                                    return false;
                                }
                                return true;
                            }
                           } 
                        }
                        else if (RenewCount == MaxRenewBookCount) {
                            document.getElementById(_clientLblErrorMsg).innerText = "You already have renewed this book for " + MaxRenewBookCount + " time(s). Please return this book.";
                            document.getElementById(_clientLblErrorMsg).innerHTML = "You already have renewed this book for " + MaxRenewBookCount + " time(s). Please return this book.";
                            return false;
                        }


                    }
                }
            }
            var iMaxIssueCount = document.getElementById(_clienthidMaxIssueBookCount).value;
            if (iMaxIssueCount != "") {
                ClearMessages();
                document.getElementById(_clienthidRowNo).value = iRowId;
                var txtAccesionNo = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowId + "_txtAccessionNoBarcode").value;
                if (txtAccesionNo == "") {
                    document.getElementById(_clientLblErrorMsg).innerText = "Accession No. / Barcode should not be blank."
                    document.getElementById(_clientLblErrorMsg).innerHTML = "Accession No. / Barcode should not be blank."
                    return false;
                }
                else {
                    if (!ValidateBarcode(txtAccesionNo, iSchoolId))
                        return false;
                }
            }
            else {
                document.getElementById(_clientLblErrorMsg).innerHTML = "Library settings details are not configured for the selected user.";
                document.getElementById(_clientLblErrorMsg).innerText = "Library settings details are not configured for the selected user.";
                return false;
            }
            return true;
        }


        function ConfirmReturnBook(iRowId, iSchoolId, txtAccesionNo) {

            
            var iMaxIssueCount = document.getElementById(_clienthidMaxIssueBookCount).value;
            if (iMaxIssueCount != "") {
                ClearMessages();
                document.getElementById(_clienthidRowNo).value = iRowId;
               // var txtAccesionN= document.getElementById(_clientlstvwUsers + "_ctrl" + iRowId + "_txtAccessionNoBarcode").value;
               if (txtAccesionNo == "") {
                    document.getElementById(_clientLblErrorMsg).innerText = "Accession No. / Barcode should not be blank."
                    document.getElementById(_clientLblErrorMsg).innerHTML = "Accession No. / Barcode should not be blank."
                    return false;
                }
                else {
                    if (!ValidateBarcode(txtAccesionNo, iSchoolId))
                        return false;
                    else {
                        if (!ConfirmReturn())
                            return false;

                    }
                }
            }
            else {
                document.getElementById(_clientLblErrorMsg).innerHTML = "Library settings details are not configured for the selected user.";
                document.getElementById(_clientLblErrorMsg).innerText = "Library settings details are not configured for the selected user.";
                return false;
            }
            return true;
        }
        function ValidateBarcode(txtAccesionNo, iSchoolId) {
           
               ClearMessages();
               var sBarcode = txtAccesionNo.substring(txtAccesionNo.length - (iSchoolId.length + 1), txtAccesionNo.length).toLowerCase()
               var sActualBarcodeStd = ("P".toLowerCase()).concat(iSchoolId);

               var reg0Str = "^[0-9]+$"
               if (sBarcode == sActualBarcodeStd) {
                   var sBookDetailId = txtAccesionNo.substring(1, txtAccesionNo.length - sBarcode.length);
                        if (sBookDetailId.match(/^[0-9]+$/))
                            return true;
                        else
                            if (document.getElementById(_clientLblErrorMsg) != null && document.getElementById(_clientLblErrorMsg) != "undefined") {
                                    document.getElementById(_clientLblErrorMsg).visible = true;
                                    document.getElementById(_clientLblErrorMsg).innerHTML = "Please enter valid Barcode:" + txtAccesionNo;
                                    document.getElementById(_clientLblErrorMsg).innerText = "Please enter valid Barcode:" + txtAccesionNo;
                                    return false;
                                }
                        }
                return true;
          }

        function ValidateUserIssueBookCount(iRowId, iSchoolId, hidUserIssueBookCount) {
            var iUserIssueCount = 0;
            var hidUserIssueBookCountID = document.getElementById(hidUserIssueBookCount)
            if (hidUserIssueBookCountID != null && hidUserIssueBookCountID != "undefined")
                iUserIssueCount = hidUserIssueBookCountID.value;
            var iMaxIssueCount = document.getElementById(_clienthidMaxIssueBookCount).value;
            if (iMaxIssueCount != "") {
                ClearMessages();

                var txtAccesionNo = trimAll(document.getElementById(_clientlstvwUsers + "_ctrl" + iRowId + "_txtAccessionNoBarcode").value);
                var UserRole = document.getElementById(_clientcmbUserRole)

                if (txtAccesionNo != "") {
                    var arrAccessionNo = txtAccesionNo.split(',');
                    if (!ValidateBarcode(txtAccesionNo, iSchoolId))
                        return false;

                    if (iUserIssueCount < iMaxIssueCount) {
                        if ((Number(arrAccessionNo.length) + Number(iUserIssueCount)) > iMaxIssueCount) {
                            txtAccesionNo = "";
                            document.getElementById(_clientLblErrorMsg).innerHTML = "Can not issue more than " + iMaxIssueCount + " book(s).";
                            document.getElementById(_clientLblErrorMsg).innerText = "Can not issue more than " + iMaxIssueCount + " book(s).";
                            return false;
                        }
                        else {
                            var sVal = "";
                            var iCnt = arrAccessionNo.length;
                            sVal = arrAccessionNo[0];
                            var iNo = 1;
                            var sMessage = "";
                            while (iCnt > 0) {
                                var iCount = arrAccessionNo.length - 1;
                                while (iNo <= iCount) {
                                    if (sVal == arrAccessionNo[iNo]) {
                                        if (sMessage == "")
                                            sMessage = sVal;
                                        else
                                            sMessage = sMessage + ',' + sVal
                                    }
                                    else
                                        iNo = iNo + 1
                                }
                                iCnt = iCnt - 1
                            }
                            if (sMessage != "") {
                                document.getElementById(_clientLblErrorMsg).innerHTML = sMessage + ": Accession No./ Barcode should not be duplicate.";
                                document.getElementById(_clientLblErrorMsg).innerText = sMessage + ": Accession No./ Barcode should not be duplicate.";
                                return false;
                            }
                        }
                    }
                    else {
                        document.getElementById(_clientLblErrorMsg).innerHTML = "Can not issue more than " + iMaxIssueCount + " book(s).";
                        document.getElementById(_clientLblErrorMsg).innerText = "Can not issue more than " + iMaxIssueCount + " book(s).";
                        txtAccesionNo = "";
                        return false;
                    }
                }
                else {
                    document.getElementById(_clientLblErrorMsg).innerHTML = "Accession No. / Barcode should not be blank.";
                    document.getElementById(_clientLblErrorMsg).innerText = "Accession No. / Barcode should not be blank.";
                    return false;
                }
            }
            else {
                document.getElementById(_clientLblErrorMsg).innerHTML = "Library settings details are not configured for the selected user.";
                document.getElementById(_clientLblErrorMsg).innerText = "Library settings details are not configured for the selected user.";
                return false;
            }

            if ($get(_clienthidShowLateValidation).value == "1") {
                var val = IssueIndividual(iRowId)
                if (val != '') {
                    document.getElementById(_clientLblErrorMsg).innerHTML = val;
                    return false
                }
            }
            return true;
        }

        function CheckEnteredChar(e, iRowId) {
            var keynum
            var keychar
            // For Internet Explorer  
            if (window.event)
                keynum = e.keyCode
            // For Netscape/Firefox/Opera  
            else if (e.which)
                keynum = e.which
            keychar = String.fromCharCode(keynum)
            if (keychar == ",")
                keychar = ""
            //List of special characters you want to restrict  
            // Not Allow - check for space , 
            //|| keynum == 39 || keynum == 59 || keynum == 17 || keynum == 37 || keynum == 27 || keynum == 186 || keynum == 8 || (keynum > 47 && keynum < 91) || (keynum > 95 && keynum < 106))
            //Restrict comma
            if (keynum != 190 && keynum != 44 && keynum != 32 && keynum != 188)// || keynum == 59 || keynum == 17 || keynum == 37 || keynum == 27 || keynum == 186 || keynum == 8 || (keynum > 47 && keynum < 91) || (keynum > 95 && keynum < 106))
                return true;
            else
                return false;
        }

        function ShowPopup(e, iBookNo, ReturnDate, IssueDate, iLateFee, iRowIndex, iUserId) {
           
            //iBookNo=iBookNo.replace("$","'")
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.updtpnlPopUp.ClientID %>").style
            var btnReturn = $get("<%=this.btnReturnBook.ClientID %>")
            var dtReturn = new Date(ReturnDate).format("dd-MMM-yyyy")
            var dtIssue = new Date(IssueDate).format("dd-MMM-yyyy")
            var now = new Date()
            document.getElementById(_clienthidRowNo).value = iRowIndex;
            $get("<%=this.hidReturnDate.ClientID %>").value = dtReturn
            $get("<%=this.txtReturnDate.ClientID %>").value = now.format("dd-MMM-yyyy")
            $get("<%=this.hidIssueDate.ClientID %>").value = dtIssue
            $get("<%=this.hidUserId.ClientID %>").value = iUserId
            $get("<%=this.hidBookNo.ClientID %>").value = iBookNo
            var width = 250
            var height = 180
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            // Override the z-index of the topmost wz_dragdrop.js D&D item
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010);
            cssstyle.zIndex = 2000;
            cssstyle.visibility = "visible";
            cssstyle.display = "block";
            var txtLateFee = document.getElementById(_clienttxtLateFee);
            var lblLateFee = document.getElementById(_clientlblLateFee);

            if (txtLateFee && iLateFee != 0) {
                document.getElementById(_clienttxtLateFee).style.display = "";
                document.getElementById(_clienttxtLateFee).value = iLateFee;
            }
            else
                document.getElementById(_clienttxtLateFee).style.display = "none";
            if (lblLateFee && iLateFee != "")
                document.getElementById(_clientlblLateFee).style.display = "";
            else
                document.getElementById(_clientlblLateFee).style.display = "none";

        }
        function HidePopup() {
            $get("<%=this.updtpnlPopUp.ClientID %>").style.visibility = "hidden"
            $get("<%=this.updtpnlPopUp.ClientID %>").style.display = "none"
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            var dtActReturnDate = document.getElementById(_clienttxtReturnDate).value
            $get("<%=this.hidActReturnDate.ClientID %>").value = dtActReturnDate
            var cssstyleMain = $get("<%=this.divMain.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            return false
        }
        function ClosePopup() {
            HidePopup();
        }
        function ConfirmReturn() {
           
            var bResult = true
            var validationResult = true
            var iLateFee = 0
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            var sMsg = "Are you sure you want to return this book?"
            if (!window.confirm(sMsg)) {
                bResult = false
            }

            return bResult
        }
        function IsValidReturnDate(oSrc, args) {
            if ($get("<%=this.updtpnlPopUp.ClientID %>").style.visibility == "visible") {
                var ocstValRetDate = document.getElementById(_clientCstValRetDate)
                if (document.getElementById(_clienttxtReturnDate).value == '') {
                    if (ocstValRetDate != null) {
                        ocstValRetDate.innerHTML = 'Return date should not be blank.'
                        ocstValRetDate.errormessage = 'Return date should not be blank.'
                        args.IsValid = false
                        return true
                    }
                }
                else {
                    var dtActReturnDate = document.getElementById(_clienttxtReturnDate).value
                    var dtToday
                    var TodayDate = new Date().format("dd-MMM-yyyy")
                    if (document.all)
                        dtToday = new Date(TodayDate.replace('-', ' '))
                    else
                        dtToday = new Date(convertdate(TodayDate))
                    if (dtActReturnDate.length > 0) {
                        var ReturnDate, dtIssueDate
                        var IssueDate = document.getElementById(_clienthidIssueDate).value
                        if (document.all) {
                            ReturnDate = new Date(dtActReturnDate.replace('-', ' '))
                            dtIssueDate = new Date(IssueDate.replace('-', ' '))
                        }
                        else {
                            ReturnDate = new Date(convertdate(dtActReturnDate))
                            dtIssueDate = new Date(convertdate(IssueDate))
                        }
                        var strIssueDate = getDateString(dtIssueDate)
                        if (ReturnDate < dtIssueDate) {
                            ocstValRetDate.errormessage = "Book return date should be greater than or equal to the book issue date. (i.e " + strIssueDate + " )."
                            ocstValRetDate.innerHTML = "Book return date should be greater than or equal to the book issue date. (i.e " + strIssueDate + " )."
                            oSrc.errormessage = "Book return date should be greater than or equal to the book issue date. (i.e " + strIssueDate + " )."
                            args.IsValid = false
                            return true
                        }
                        //Book return date should be greater than or equal to the book issue date. 
                        if (ReturnDate > dtToday) {
                            ocstValRetDate.errormessage = "Return date should not be future date.";
                            ocstValRetDate.innerHTML = "Return date should not be future date.";
                            oSrc.errormessage = "Return date should not be future date.";
                            args.IsValid = false
                            return true
                        }
                    }
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateLateCase() {
            if ($get(_clienthidShowLateValidation).value == "1") {
                var msg = ConfirmLateCase();

                if (msg != '') {
                    document.getElementById(_clientLblErrorMsg).innerHTML = msg;
                    return false
                }
            }
            
            return true;
        }

        function IssueIndividual(index) {
            var msg = ''
            var Nos = ''

            var srNo = $get(_clientlstvwUsers + '_ctrl' + index + '_lblSrNo')

            var val = $get(_clientlstvwUsers + '_ctrl' + index + '_txtAccessionNoBarcode').value
            if (val.trim() != "") {
                var lateCase = $get(_clientlstvwUsers + '_ctrl' + index + '_hidHasLateEntry').value;
                if (parseInt(lateCase) == 1) {
                    var rollNo = $get(_clientlstvwUsers + '_ctrl' + index + '_lblSrNo').innerHTML;
                    Nos = Nos + ", " + srNo.innerHTML;
                }
            }

            if (Nos.length > 0) {
                Nos = Nos.substring(2);
                msg = "Return date of already issued book has passed, so you can't issue another book for Sr. No. '" + Nos + "'.";
            }
            return msg;
        }

        function ConfirmLateCase() {
        
            if($get('<%=this.lblErrorMsg.ClientID %>') != null)
                $get('<%=this.lblErrorMsg.ClientID %>').innerHTML = "";
            
            if($get('<%=this.lblUpdateSucess.ClientID %>') != null)
                $get('<%=this.lblUpdateSucess.ClientID %>').innerHTML = "";

            var opt = $get(_clientOption).checked;
            var msg = ''
            var exceedMsg = ''
            if (opt) {
                var index = 0;
                var Nos = ''
                var exceedCount = ''
                var maxCount = $get(_clienthidMaxIssueBookCount).value
                var srNo = $get(_clientlstvwUsers + '_ctrl' + index + '_lblSrNo')

                while (srNo != null) {
                    var val = $get(_clientlstvwUsers + '_ctrl' + index + '_txtAccessionNoBarcode').value
                    
                    if (val.trim() != "") {
                        var lateCase = $get(_clientlstvwUsers + '_ctrl' + index + '_hidHasLateEntry').value;
                        var issueCount = $get(_clientlstvwUsers + '_ctrl' + index + '_hidIssueCount').value;

                        if((issueCount + 1) > maxCount)
                            exceedCount = exceedCount +", "+srNo.innerHTML

                        if (parseInt(lateCase) == 1) {
                            Nos = Nos + ", " + srNo.innerHTML;
                        }
                    }

                    index++;
                    srNo = $get(_clientlstvwUsers + '_ctrl' + index + '_lblSrNo')
                }

                if(exceedCount.length>0)
                {
                    exceedCount = exceedCount.substring(2);
                    exceedMsg = "You cannot issue more than "+maxCount+" book for selected user role for Sr. No. '" + exceedCount + "'.";
                }

                if (Nos.length > 0) {
                    Nos = Nos.substring(2);
                    msg = "Return date of already issued book has passed, so you can't issue another book for Sr. No. '" + Nos + "'.";
                }
            }

            if(exceedMsg !="" && msg !="")
                return (exceedMsg+"<br />"+msg);
            else if(exceedMsg !="" && msg =="") 
                return exceedMsg;
            else
                return msg;
        }

    </script>
    <script language="javascript" type="text/javascript">
    
        _cltDivLateFeeAmt = "<%=this.DivLateFeeAmt.ClientID %>"

        var _totalWinHeight;
        var _adjWinHeight;
        var _rightFooterPos;
        var _bottomFooterPos;

        window.onresize = setTotal;
        window.onscroll = setTotal;
        window.onload = setTotal;

        function setTotal() {
            _totalWinHeight = document.body.scrollHeight;
            _adjWinHeight = _totalWinHeight; //-608;

            if (document.getElementById(_cltDivLateFeeAmt) != null) {
                _rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltDivLateFeeAmt).style.height);
                document.getElementById(_cltDivLateFeeAmt).style.top = _rightFooterPos;
            }
            window_onscroll();
        }

        function window_onscroll() {
            if (document.body.scrollTop <= _adjWinHeight) {
                if (document.getElementById(_cltDivLateFeeAmt) != null) {
                    document.getElementById(_cltDivLateFeeAmt).style.top = document.body.scrollTop + _rightFooterPos;
                }
            }
        }
    </script>

    <script language="javascript" type="text/javascript">

        _cltDivLateFeeAmt1 = "<%=this.updtpnlPopUp.ClientID %>"

        var _totalWinHeight;
        var _adjWinHeight;
        var _rightFooterPos;
        var _bottomFooterPos;
        
        window.onresize = setTotal;
        window.onscroll = setTotal;
        window.onload = setTotal;

        function setTotal() {
            _totalWinHeight = document.body.scrollHeight;
            _adjWinHeight = _totalWinHeight; //-608;

            if (document.getElementById(_cltDivLateFeeAmt1) != null) {
                _rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltDivLateFeeAmt1).style.height);
                document.getElementById(_cltDivLateFeeAmt1).style.top = _rightFooterPos;
            }
            window_onscroll();
        }

        function window_onscroll() {
            if (document.body.scrollTop <= _adjWinHeight) {
                if (document.getElementById(_cltDivLateFeeAmt1) != null) {
                    document.getElementById(_cltDivLateFeeAmt1).style.top = document.body.scrollTop + _rightFooterPos;
                }
            }
        }
    </script>

    <script language="javascript" type="text/javascript">
        var nav = window.Event ? true : false;
        if (nav) {
            window.captureEvents(Event.KEYDOWN);
            window.onkeydown = NetscapeEventHandler_KeyDown;
        } else {
            document.onkeydown = MicrosoftEventHandler_KeyDown;
        }

        function NetscapeEventHandler_KeyDown(e) {
            if (e.which == 13 && e.target.type != 'textarea' && e.target.type != 'submit') {
                return false;
            }
            return true;
        }

        function MicrosoftEventHandler_KeyDown() {
            if (event.keyCode == 13 && event.srcElement.type != 'textarea' &&
            event.srcElement.type != 'submit')
                return false;
            return true;
        }

        $(document).ready(function () {
            AutoSearch();
        });
        function AutoSearch() {
            var SchoolId = "<%=miSchoolId %>";
            _clienttxtRegNumber = '#<%=txtUserName.ClientID%>';
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _clienttxtRegNumber, _clientcmbUserRole, 1, null, null, null);
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.

        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtUserName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
