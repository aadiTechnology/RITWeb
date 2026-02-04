<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentListForNoteDetailsUI.aspx.cs" Inherits="StudentListForNoteDetailsUI" MasterPageFile="../MasterPages/MasterPage.master"
    ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .centerText {
            text-align: center;
            display: block;
        }
    </style>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <span class="ClsMdtStar">* Mandatory Field</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="center" class="ClsBorderlight" style="width: 100px;">
                                            <span class="ClsLabel">Standard :</span>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged" ViewStateMode="Enabled">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                        <td style="width: 50px;"></td>
                                        <td align="center" class="ClsBorderlight" style="width: 100px;">
                                            <span class="ClsLabel">Division :</span>
                                        </td>
                                        <td align="left">
                                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbDivision" CssClass="MidCombo" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged" EnableViewState="true">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <table width="50%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ListView ID="lstvwStudentList" runat="server" DataKeyNames="SchoolWiseStudentId"
                                                        OnItemDataBound="lstvwStudentList_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th width="150px" align="left" class="clsLabelgrd">
                                                                        <asp:Label ID="lblGrNo" runat="server" Text="GR No."></asp:Label>
                                                                    </th>
                                                                    <th width="60px" align="Center" class="clsLabelgrd">
                                                                        <asp:Label ID="lblRollNo" runat="server" Text="<%$ Resources:LocalizedResources, RollNo%>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="clsLabelgrd">
                                                                        <asp:Label ID="lblStudentName" runat="server" Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                    </th>
                                                                    <th width="100px" align="center" class="clsLabelgrd">
                                                                        <asp:Label ID="lblEdit" runat="server" Text="Add/Edit"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class ='<%# Container.DisplayIndex % 2 == 0?"ClsGridRow":"ClsGridAltRow" %>'>
                                                                <td align="Center">
                                                                    <asp:Label ID="lblGrNO" runat="server" CssClass="ClsLabel" Text='<%#Eval("GRNumber") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidData" runat="server" Value="" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblRollNo" runat="server" CssClass="centerText" Text='<%#Eval("RollNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text='<%#Eval("studentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                        ToolTip="Edit" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>                                                        
                                                        <EmptyDataTemplate>
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">No record found.
                                                                </td>
                                                            </tr>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                    <asp:HiddenField ID="hidHasFullAccess" runat="server" Value="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>                        
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script>
        function OpenPopup(index, hid) {
            var str = $('#' + hid).val();
            window.open('StudentAchievementPopup.aspx?' + str, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=850,height=600')
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
