<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="StandardVIII.aspx.cs" Inherits="RITeSchool_Common_StandardVIII" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
<table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        height: 100%">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="middle">
            <tr>
                <td colspan="4" align="left" id="tdNursury" runat="server" visible="false">
                    <table>
                        <tr>
                            <td align="center" style="font-weight: bold">
                                <asp:Label ID="lblHeader" runat="server" CssClass="ClsConfigText" Text="Syllabus for Standard VIII"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td align="center" style="font-size: large">
                        <asp:LinkButton ID="lnkStdard" runat="server" CssClass="Lbl10ptB">Term 1</asp:LinkButton>
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
                            <asp:Label ID="Label2" runat="server" CssClass="ClsConfigText" Text="Syllabus for Standard-VIII"></asp:Label>
                        </td>
                    </tr>
                    <tr width="45%">
                        <td align="center" style="font-size: large; background-color: #ffcccc" width="20%"
                            class="WeekDCell ClsBorderlight">
                            <asp:LinkButton ID="lnkStdTermI" runat="server" CssClass="Lbl10ptB">Term 1 - Click Here to Download</asp:LinkButton>
                        </td>
                    </tr>
                    </table>
               </td>
        </tr>
    </table>
</asp:Content>

