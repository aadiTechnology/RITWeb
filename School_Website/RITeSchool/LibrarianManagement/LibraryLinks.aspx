<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="LibraryLinks.aspx.cs" Inherits="LibraryLinks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
 <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr runat="server" id="trStudentLibrary" viewstatemode="Enabled" >
                                                <td align="center" colspan="1" class="ClsBorderlight">
                                                   <asp:HyperLink ID="hlnkStudentLibrary" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        Visible="true" NavigateUrl="~/RITeSchool/LibrarianManagement/IssuedBookDetails.aspx"
                                                        Text="School Library"></asp:HyperLink>
                                                </td>
                                            </tr>
                                             <tr runat="server" id="trExternalLibrary" viewstatemode="Enabled" >
                                                <td align="center" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkExternalLibrary" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        Visible="true" NavigateUrl="https://drive.google.com/folderview?id=1fTRvhiriVwY-dP4m2uxQDi0AE_XD8vXK"  Text="External Library Link"></asp:HyperLink>
                                                       
                                                </td>
                                            </tr>
                </table>
                </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" Runat="Server">
</asp:Content>

