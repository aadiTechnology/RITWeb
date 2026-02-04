<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="StaffInOutDetailsPopup.aspx.cs" Inherits="StaffInOutDetailsPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%">
        <tr>
            <td valign="top">
                <table width="100%">
                    <tr>
                        <td align="left" style="height: 20px; width: 99%;" class="ClsGrayMainTitle">
                            <span style="font-weight: bold">Staff In/Out Details.</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>        
        <tr>
            <td align="center">
                <table align="center">
                    <tr>
                        <td style="height: 15px;">
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight" style="width: 110px;">
                            <asp:Label ID="lblStaffGroup" runat="server" CssClass="ClsLabel" Text="Staff Group"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbStaffGroup" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                TabIndex="1" Width="218px" 
                                onselectedindexchanged="cmbStaffGroup_SelectedIndexChanged">
                            </asp:DropDownList>                                                        
                        </td>                        
                    </tr>
                    <tr>
                        <td class="ClsBorderlight" style="width: 110px;">
                            <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text="Name"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                             <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                            <asp:DropDownList ID="cmbUserName" runat="server" CssClass="LrgCombo" TabIndex="2"
                                Width="219px" AutoPostBack="true">
                                <asp:ListItem Value ="0" Text ="-- All --"></asp:ListItem>                                
                            </asp:DropDownList>                            
                            </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>                            
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text="Start Date / Time"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtStartDate" CssClass="SmlTxtBox" runat="server" ReadOnly="true"
                                TabIndex="3"></asp:TextBox>
                            <rjs:PopCalendar ID="cal_FormOpenDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                Culture="en" ShowWeekend="True" AutoPostBack="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="End Date / Time"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEndDate" CssClass="SmlTxtBox" runat="server" ReadOnly="true"
                                TabIndex="5"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                Culture="en" ShowWeekend="True" AutoPostBack="False" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                            <asp:Button ID="btnDisplay" CssClass="ClsBtn" runat="server" Text="Display" OnClientClick="ClearMessages()"
                                TabIndex="9" onclick="btnDisplay_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Close %>"
                                CssClass="ClsBtn" OnClientClick="ClosePopup(); return false;" CausesValidation="false"
                                TabIndex="13" />
                            <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwODDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:HiddenField ID="hidYearId" runat="server" />
        <asp:HiddenField ID="hidUserId" runat="server" />
    </table>
    <script language="javascript" type="text/javascript">
        function ClosePopup() {
            window.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
