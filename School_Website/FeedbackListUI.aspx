<%@ Page Title="" Language="C#" MasterPageFile="~/PPSMaster.master" AutoEventWireup="true"
    CodeFile="FeedbackListUI.aspx.cs" Inherits="FeedbackListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 95%" align="center">
        <div id="nifty" align="center">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
            </b></b>
            <table width="100%">
                <tr>
                    <td class="HeadTxtB borderBtm" height="25px" align="left">
                        <label id="lblheadr" runat="server">
                        </label>
                    </td>
                </tr>
                <tr id="trNoRecord" runat="server">
                    <td class="LblNoRecord" align="center">
                        No Record Found.
                    </td>
                </tr>
                    <tr id="trUser" runat="server">
                     <td align="center">
                         <table id="tblParameterUser" runat="server" width="95%">
                         </table>
                     </td>
                 </tr>
                 <tr id="trOther" runat="server">
                    <td align="center" valign="top">
                        <asp:ListView ID="lstvwOtherFeedback" runat="server" DataKeyNames="LinkId,FilePath"
                            OnItemDataBound="lstvwOtherFeedback_ItemDataBound">
                            <LayoutTemplate>
                                <table id="Table2" align="center" width="100%" runat="server" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsProgressGridTestHeader1">
                                        <th align="left" class="paddingLSML">
                                            <asp:Label ID="lblName" runat="server" Text="Details"></asp:Label>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="Tr2" runat="server" class="ClsProgressGridTestBody1">
                                    <td class="paddingL" align="left">
                                        <asp:HyperLink ID="lnkName" NavigateUrl="#" runat="server" Text='<%# Eval("LinkName") %>'></asp:HyperLink>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            </asp:ListView>
                    </td>
                </tr>
            </table>
            <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
            </b></b>
        </div>
    </div>
</asp:Content>
