<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="StaffLeaveExportPopup.aspx.cs" Inherits="StaffLeaveExportPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                        <tr>
                            <td align="left" class="MainTitleHead" style="height: 20px">
                                <span style="font-weight: bold">Export Staff Leaves</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSum" runat="server" />
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Year should be selected."
                                            Display="None" Type="Integer" ValueToCompare="0" ControlToValidate="cmbYear"
                                            Operator="NotEqual"></asp:CompareValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Month should be selected."
                                            Display="None" Type="Integer" ValueToCompare="0" ControlToValidate="cmbMonth"
                                            Operator="NotEqual"></asp:CompareValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroups" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                                        <asp:PostBackTrigger ControlID="btnExport" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right" width="200px">
                                <span class="ClsMdtStar">*</span>
                                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Year : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbYear" runat="server" CssClass="SmlCombo" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbYear_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel">Month : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbMonth" runat="server" CssClass="SmlCombo">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel">Staff Groups : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbStaffGroups" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbStaffGroups_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel">User : </span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbUser" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroups" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr class="height10">
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnExport" runat="server" Text="Export Leave Details" CssClass="ClsBtn"
                        OnClick="btnExport_Click" Width="150px" />
                    <asp:Button ID="btnMIS" runat="server" Text="Attendance MIS Report" CssClass="ClsBtn"
                        Width="150px" OnClick="btnMIS_Click" />
                    <asp:Button ID="btnStaffAttendance" runat="server" Text="Staff Attendance" CssClass="ClsBtn"
                        Width="100px" OnClick="btnStaffAttendance_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <hr style="color: #C0C0C0" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Staff Groups : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbStaffgroupForBalance" runat="server" CssClass="MidCombo"
                                    AutoPostBack="True" OnSelectedIndexChanged="cmbStaffgroupForBalance_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel">User : </span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbUserForBalance" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffgroupForBalance" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr class="height10">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnExportBalance" runat="server" Text="Export Current Leave Balance"
                                    CausesValidation="false" CssClass="ClsBtn" Width="180px" OnClick="btnExportBalance_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <hr style="color: #C0C0C0" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Staff Groups : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbGroups" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbGroups_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel">User : </span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbStaff" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbGroups" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel">Start Date : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtStartDate" CssClass="MidTxtBox" runat="server" ReadOnly="true" />
                                <rjs:PopCalendar ID="cal_startDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                    Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Start date should not be blank."
                                    AutoPostBack="False" To-Today="true" />
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="ClsLabel">End Date : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtEndDate" CssClass="MidTxtBox" runat="server" ReadOnly="true" />
                                <rjs:PopCalendar ID="cal_txtEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                    Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Payment date should not be blank."
                                    AutoPostBack="False" To-Today="true" />
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr class="height10">
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnExportStaffLeave" runat="server" Text="Export Staff Leaves" CausesValidation="false"
                                    CssClass="ClsBtn" Width="180px" OnClick="btnExportStaffLeave_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <hr style="color: #C0C0C0" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="window.close()"
                        CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
