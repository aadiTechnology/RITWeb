<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalaryChangesUI.aspx.cs" Inherits="SalaryChangesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="right">
                    <span class="ClsMdtStar">* Mandatory Fields</span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Staff Group should be selected."
                                Display="None" ControlToValidate="cmbStaffGroup" ValueToCompare="0" Operator="NotEqual"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="User should be selected."
                                Display="None" ControlToValidate="cmbUser" ValueToCompare="0" Operator="NotEqual"></asp:CompareValidator>
                            <asp:ValidationSummary ID="ValSum" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Staff Group : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbStaffGroup" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">User : </span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbUser" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
