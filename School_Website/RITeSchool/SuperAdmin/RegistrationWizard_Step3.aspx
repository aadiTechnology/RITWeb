<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    CodeFile="RegistrationWizard_Step3.aspx.cs" Inherits="RegistrationWizard_Step3" %>


    
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 98%;">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center">
                <!-- Data Insert Here -->
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <tr>
                        <td style="height: 19px" align="left" colspan="4">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td class="ClsGrayMainTitle" style="height: 20px">
                                        <%--<asp:Label ID="lblBuyer" CssClass="MainTitleHead" runat="server" BorderWidth="0px">Congratulation</asp:Label>--%>
                                        <span class="MainTitleHead">Congratulation</span>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <!-- User InfoTable starts here -->
                            <table id="tblUserInfo" runat="server" border="0" cellpadding="0" cellspacing="1"
                                style="width: 60%;">
                                <tr>
                                    <td align="center" colspan="4" style="height: 5px" class="ClsHilightBGB">
                                        <asp:Label ID="LblThankyou" runat="server" BorderWidth="0px" Font-Bold="True"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px;
                                        padding-top: 10px;" colspan="4" class="ClsBorderLight">
                                        <asp:Label ID="lblMessage" runat="server" BorderWidth="0px" Font-Bold="True"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4" style="height: 20px">
                                        <asp:Button CssClass="ClsBtn" ID="btnOk" runat="server" Text="Ok" OnClick="btnOk_Click">
                                        </asp:Button></td>
                                </tr>
                            </table>
                            <!-- User InfoTable end here -->
                        </td>
                    </tr>
                </table>
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>
</asp:Content>
