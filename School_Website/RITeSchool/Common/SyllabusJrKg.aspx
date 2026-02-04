<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SyllabusJrKg.aspx.cs" Inherits="RITeSchool_Syllabus_SyllabusJrKg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        height: 100%">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="middle">
            <tr>
                <td colspan="4" align="left" id="tdNursury" runat="server" visible="false">
                    <table>
                        <tr>
                            <td align="center" style="font-weight: bold">
                                <asp:Label ID="lblHeader" runat="server" CssClass="ClsConfigText" Text="Syllabus for Nursury"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="font-size: large">
                                <asp:LinkButton ID="lnkNursuryJune" runat="server" CssClass="Lbl10ptB">June</asp:LinkButton>
                                <asp:LinkButton ID="lnkNursuryJuly" runat="server" CssClass="Lbl10ptB">July</asp:LinkButton>                                
                                <asp:LinkButton ID="lnkNursuryAug" runat="server" CssClass="Lbl10ptB">August</asp:LinkButton>
                                <asp:LinkButton ID="lnkNursurySept" runat="server" CssClass="Lbl10ptB">September</asp:LinkButton>
                                <asp:LinkButton ID="lnkNursuryOct" runat="server" CssClass="Lbl10ptB">October</asp:LinkButton>
                                <asp:LinkButton ID="lnkNursuryNov" runat="server" CssClass="Lbl10ptB">November</asp:LinkButton>
                             </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <tr>
            <td colspan="4" align="center" id="tdJuniorKG" runat="server" visible="true">
                <table align="center" style="width: 448px">
                    <tr>
                        <td align="center" style="font-weight: bold; width: 106%;" 
                            class="TotalCount ClsBorderBlue">
                            <asp:Label ID="Label3" runat="server" CssClass="ClsConfigText" Text="Activities for Junior KG"></asp:Label>
                        </td>
                    </tr>
                    <tr width="40%">
                        <td align="center" 
                            style="font-size: large; width: 106%;" 
                            class="WeekDCell ClsBorderlight">
                            <asp:LinkButton ID="lnkJuniorKGJune" runat="server" CssClass="Lbl10ptB">June & July - Click Here to Download</asp:LinkButton>
                        </td>
                    </tr>
                   <tr width="40%">
                        <td align="center" style="font-size: large;background-color:#ffcccc" width="20%" class="WeekDCell ClsBorderlight">
                            <asp:LinkButton ID="lnkJuniorKGAugSept" runat="server" CssClass="Lbl10ptB">August & September - Click Here to Download</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
