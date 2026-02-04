<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentMarksExportUI.aspx.cs" Inherits="StudentMarksExportUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
    <div style="float: right;vertical-align:top;">
                        <span class="ClsMdtStar">* Mandatory Fields</span>
                    </div>
        <table border="0" cellpadding="0" cellspacing="0" width="97%">            
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="VAlSum" runat="server" />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Academic Year should be selected."
                                ControlToValidate="cmbAcademicYear" Display="None" Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Standard should be selected."
                                ControlToValidate="cmbStandard" Display="None" Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="Division should be selected."
                                ControlToValidate="cmbDivision" Display="None" Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbAcademicYear" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                            <asp:PostBackTrigger ControlID="btnExport" />
                        </Triggers>
                    </asp:UpdatePanel>                    
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight" style="width: 100px">
                                <span class="clsLabel">Academic Year :</span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbAcademicYear" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                    OnSelectedIndexChanged="cmbAcademicYear_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="clsLabel">Standard :</span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbStandard" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                            OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbAcademicYear" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="clsLabel">Division :</span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbDivision" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                            OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbAcademicYear" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="clsLabel">Subject :</span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbSubject" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbAcademicYear" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="clsLabel">Test :</span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbTest" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbAcademicYear" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" OnClick="btnExport_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnExport" />
                                    </Triggers>
                                </asp:UpdatePanel>
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
