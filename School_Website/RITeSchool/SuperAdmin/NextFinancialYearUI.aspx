<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="NextFinancialYearUI.aspx.cs" Inherits="NextFinancialYearUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="30%">
        <tr id="trSuccess" runat="server" visible="false">
            <td align="center">
                <table width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblSuccess" CssClass="LblNrmlB" ForeColor="Blue" runat="server" Text="Next financial year generated successfully !!!"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnOk" CssClass="ClsBtn" runat="server" Text="Ok" PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trControls" runat="server">
            <td>
                <table width="100%" class="ClsBorderlight">
                    <tr id="trNew" runat="server">
                        <td colspan="2">
                            <span class="LblNrmlB">Generate next financial year </span><span id="spnNewYear"
                                style="color: Blue; font-weight: bold" runat="server"></span><span style="font-weight: bold">
                                    .</span>
                        </td>
                    </tr>
                    <tr id="trExist" runat="server">
                        <td colspan="2">
                            <span class="LblNrmlB">There already exist next financial year </span><span id="spnYear"
                                runat="server" style="color: Blue; font-weight: bold"></span><span style="color: #333333;
                                    font-weight: bold;">.
                                    <br /><br />
                                </span><span class="LblNrmlB">On click of Create button, it will be deleted and created
                                    again.</span>
                        </td>
                    </tr>
                    <tr style="height:5px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="middle">
                            <asp:CheckBox ID="chkMarkAsCurrent" runat="server" Text="Mark new financial year as current financial year"
                                CssClass="LblNormal" />
                        </td>
                    </tr>
                    <tr style="height:5px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 50%">
                            <asp:Button ID="btnCreate" CssClass="ClsBtn" runat="server" Text="Create" OnClick="btnCreate_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="Cancel" PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
