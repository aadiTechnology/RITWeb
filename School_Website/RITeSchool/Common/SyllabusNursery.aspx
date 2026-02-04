<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SyllabusNursery.aspx.cs" Inherits="RITeSchool_Syllabus_SyllabusNursery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        height: 100%">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
            <tr>
                <td colspan="4" align="left" id="tdNursury" runat="server" visible="false">
                    <table>
                        <tr>
                            <td align="center" style="font-weight: bold">
                                <asp:Label ID="lblHeader" runat="server" CssClass="ClsConfigText" Text="Activities for Nursury"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="font-size: large">
                                <asp:LinkButton ID="lnkNursuryJune" runat="server" CssClass="Lbl10ptB">June</asp:LinkButton>
                               <asp:LinkButton ID="lnkNursuryJuly" runat="server" CssClass="Lbl10ptB">July</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <tr>
            <td colspan="4" align="left" id="tdJuniorKG" runat="server" visible="false">
                <table>
                    <tr>
                        <td align="center" style="font-weight: bold">
                            <asp:Label ID="Label1" runat="server" CssClass="ClsConfigText" Text="Activities for Junior KG"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-size: large">
                            <asp:LinkButton ID="lnkJuniorKGJune" runat="server" CssClass="Lbl10ptB">June</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center" id="tdSeniorKG" runat="server" visible="true">
                <table width="45%" align="center">
                    <tr>
                        <td align="center" style="font-weight: bold" width="100%" class="TotalCount ClsBorderBlue">
                            <asp:Label ID="Label2" runat="server" CssClass="ClsConfigText" Text="Activities for Nursery"></asp:Label>
                        </td>
                    </tr>
                    <tr width="45%">
                        <td align="center" style="font-size: large" width="20%" class="WeekDCell ClsBorderlight">
                            <asp:LinkButton ID="lnkNurseryJune" runat="server" CssClass="Lbl10ptB">June & July - Click Here to Download</asp:LinkButton>
                        </td>
                    </tr>
                    <tr width="45%">
                        <td align="center" style="font-size: large;background-color:#ffcccc" width="20%" class="WeekDCell ClsBorderlight">
                            <asp:LinkButton ID="lnkNurseryAugSep" runat="server" CssClass="Lbl10ptB">August & September - Click Here to Download</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
