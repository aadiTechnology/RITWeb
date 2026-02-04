<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="SavedSalaryDifferencePopup.aspx.cs" Inherits="SavedSalaryDifferencePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td valign="top">
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="center" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">Salary Difference</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:RadioButton ID="optSaved" runat="server" Text="Saved" CssClass="ClsLabel" AutoPostBack="true"
                                                        GroupName="Configuration" OnCheckedChanged="optSaved_CheckedChanged" />
                                                </td>
                                                <td align="left">
                                                    <asp:RadioButton ID="optPaid" runat="server" Text="Paid" CssClass="ClsLabel" AutoPostBack="true"
                                                        GroupName="Configuration" OnCheckedChanged="optPaid_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height:10px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="50%">
                                            <tr>
                                                <td width="20%" class="ClsBorderlight">
                                                    <span class="ClsLabel">Year : </span>
                                                </td>
                                                <td class="ClsHilightBGB">
                                                    <asp:Label ID="lblYear" runat="server" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                                <td width="10%">
                                                </td>
                                                <td width="20%" class="ClsBorderlight">
                                                    <span class="ClsLabel">Month : </span>
                                                </td>
                                                <td class="ClsHilightBGB" align="center">
                                                    <asp:Label ID="lblMonth" runat="server" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblmessage" runat="server" class="ClsLabelNrml" EnableViewState="false"
                                            Visible="false" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <table border="0" cellpadding="1" cellspacing="2" style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <asp:ListView ID="lstvwEarningDeduction" runat="server" DataKeyNames="SalaryDifferenceId,UserId"
                                                        OnItemCommand="lstvwEarningDeduction_ItemCommand" OnItemDeleting="lstvwEarningDeduction_ItemDeleting">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader">
                                                                    <th class="ClsLabelL">
                                                                        Name
                                                                    </th>
                                                                    <th width="50%" align="left" valign="top" style="font-size:9pt;font-family:Arial;padding-left:5px;padding-right:2px;">
                                                                        Designation
                                                                    </th>
                                                                    <th class="ClsLabel" style="float:right;font-size:9pt;font-family:Arial;padding-left:5px;padding-right:5px;">
                                                                        Total Amount
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                <td align="left">
                                                                    <asp:Label ID="lblUserName" runat="server" class="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblDesignation" runat="server" class="ClsLabel" Text='<%#Eval("Designation") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">                                                                    
                                                                    <asp:LinkButton ID="lblAmount" runat="server" class="ClsLabelR" Text='<%#Eval("Amount") %>' CommandName="DETAILS" ></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr id="trlstvwEarnDeducts" runat="server" visible="false">
                                                                <td id="tdlstvwEarnDeducts" runat="server" align="center" colspan="3">
                                                                    <table width="80%">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:ListView ID="lstvwEarnDeduct" runat="server" DataKeyNames="SalaryDifferenceId,IsLastTransaction">
                                                                                    <LayoutTemplate>
                                                                                        <table width="100%" runat="server" id="tblUsers" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" width="50%" style="padding-left: 5px;" class="ClsLabel">
                                                                                                    Earning / Deduction
                                                                                                </th>
                                                                                                <th align="right" valign="top" style="font-size:9pt;font-family:Arial;padding-left:5px;padding-right:5px;">
                                                                                                    Amount
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblEarningDeductionName" runat="server" class="ClsLabel" Text='<%#Eval("EarningDeductionName") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lblAmount" CssClass="ClsLabelR" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblEarningDeductionName" runat="server" class="ClsLabel" Text='<%#Eval("EarningDeductionName") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lblAmount" CssClass="ClsLabelR" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
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
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="ClsBtn" CommandName="DELETE" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="left">
                                                                    <asp:Label ID="lblUserName" runat="server" class="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblDesignation" runat="server" class="ClsLabel" Text='<%#Eval("Designation") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">                                                                    
                                                                    <asp:LinkButton ID="lblAmount" runat="server" class="ClsLabelR" Text='<%#Eval("Amount") %>' CommandName="DETAILS" ></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr id="trlstvwEarnDeducts" runat="server" visible="false">
                                                                <td id="tdlstvwEarnDeducts" runat="server" align="center" colspan="3">
                                                                    <table width="80%">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:ListView ID="lstvwEarnDeduct" runat="server" DataKeyNames="SalaryDifferenceId,IsLastTransaction">
                                                                                    <LayoutTemplate>
                                                                                        <table width="100%" runat="server" id="tblUsers" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" width="50%" style="padding-left: 5px;" class="ClsLabel">
                                                                                                    Earning / Deduction
                                                                                                </th>
                                                                                                <th align="right" valign="top" style="font-size:9pt;font-family:Arial;padding-left:5px;padding-right:5px;">
                                                                                                    Amount
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblEarningDeductionName" runat="server" class="ClsLabel" Text='<%#Eval("EarningDeductionName") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lblAmount" CssClass="ClsLabelR" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblEarningDeductionName" runat="server" class="ClsLabel" Text='<%#Eval("EarningDeductionName") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lblAmount" CssClass="ClsLabelR" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
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
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="ClsBtn" CommandName="DELETE" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                                            <tr>
                                                <td align="left">
                                                    <table id="tblNote" runat="server">
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                                                <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note1 : </span>
                                                            </td>
                                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                                <span style="border-width: 0px" class="LblSmlV">'Saved' option will show user wise salary
                                                                    difference of earning and deduction that are saved but not paid.</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                                                <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note2 : </span>
                                                            </td>
                                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                                <span style="border-width: 0px" class="LblSmlV">'Paid' option will show user wise salary
                                                                    difference of earning and deduction that are already paid.</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                                                <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note3 : </span>
                                                            </td>
                                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                                <span style="border-width: 0px" class="LblSmlV">User can delete only last transaction
                                                                    of salary difference that is saved but not paid.</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="50%">
                        <tr>
                            <td align="center">
                                <asp:Button CssClass="ClsBtn" ID="BtnClose" CausesValidation="false" runat="server"
                                    Text="Close" OnClick="BtnClose_Click" />
                                <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>    
</asp:Content>
