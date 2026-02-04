<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="MathFormula.aspx.cs" Inherits="MathFormula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <script type="text/javascript" src="https://polyfill.io/v3/polyfill.min.js?features=es6"></script>
    <script id="MathJax-script" type="text/javascript" async src="https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js">
    </script>
    <table style="width: 100%;">
        <tr>
            <td style="height: 20px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                            ShowSummary="true" ValidationGroup="Formula" />
                        <asp:RequiredFieldValidator ID="reqFormulaText" runat="server" ErrorMessage="Formula text should not be blank."
                            ControlToValidate="txtFormula" Display="None" ValidationGroup="Formula"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnDisplay" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <table style="text-align: center; margin: 0px auto;" align="center" width="45%">
                    <tr>
                        <td style="width:150px;">
                            <asp:Label ID="lblEnterFormula" CssClass="ClsLabel" runat="server" Text="Enter Formula : "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormula" runat="server" TextMode="MultiLine" Height="100px" Width="100%"></asp:TextBox>
                            <span class="ClsMdtStar">* </span>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnDisplay" runat="server" CssClass="ClsBtn" Text="Display Formula"
                                OnClick="btnDisplay_Click" ValidationGroup="Formula" />
                        </td>
                    </tr>
                   <tr>
                        <td style="height: 10px;" colspan="2">
                            <hr style="border: 1px solid Gray" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                       <asp:Label ID="Label13" CssClass="ClsLabel" runat="server" Font-Bold="true" Text="Converted Formula : "></asp:Label>
                        </td>
                    </tr>
                    <tr id="trActualFormula" runat="server" align="center" style="text-align: center;"
                        visible="false">
                        <td colspan="2" align="center" style="text-align: center;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblForumla" CssClass="ClsLabel" style="float:inherit;" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnDisplay" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;" colspan="2">
                            <hr style="border: 1px solid Gray" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" CssClass="ClsLabel" runat="server" Font-Bold="true" Text="Sample : "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width: 80%; text-align: center; margin: 0px auto;" align="center">
                                <tr>
                                    <td style="height: 30px; border: 1px solid; text-align: center; margin: 0px auto;"
                                        align="center">
                                        <asp:Label ID="Label2" CssClass="ClsLabel" Font-Size="11pt" Font-Bold="true" runat="server"
                                            Text="Header"></asp:Label>
                                    </td>
                                    <td style="border: 1px solid;width:30%">
                                        <asp:Label ID="Label3" CssClass="ClsLabel" Font-Size="11pt" Font-Bold="true" runat="server"
                                            Text="Display Text"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px; border: 1px solid;">
                                        <asp:Label ID="Label4" CssClass="ClsLabel" Font-Size="11pt" Font-Bold="true" runat="server"
                                            Text="\ne"></asp:Label>
                                    </td>
                                    <td style="border: 1px solid;">
                                        <asp:Label ID="Label5" CssClass="ClsLabel" Font-Size="11pt" runat="server" Text="Not Equal To"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px; border: 1px solid;">
                                        <asp:Label ID="Label6" CssClass="ClsLabel" Font-Size="11pt" Font-Bold="true" runat="server"
                                            Text="\sqrt"></asp:Label>
                                    </td>
                                    <td style="border: 1px solid;">
                                        <asp:Label ID="Label7" CssClass="ClsLabel" Font-Size="11pt" runat="server" Text="Squar Root"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px; border: 1px solid;">
                                        <asp:Label ID="Label8" CssClass="ClsLabel" Font-Size="11pt" Font-Bold="true" runat="server"
                                            Text="\pm"></asp:Label>
                                    </td>
                                    <td style="border: 1px solid;">
                                        <asp:Label ID="Label9" CssClass="ClsLabel" Font-Size="11pt" runat="server" Text="Plus Minus"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" CssClass="ClsLabel" runat="server" Font-Bold="true" Text="Example : "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="text-align: center; margin: 0px auto;" align="center" width="80%">
                                <tr>
                                    <td style="height: 30px; border: 1px solid; text-align: left; margin: 0px auto;"
                                        align="center">
                                        <asp:Label ID="Label11" CssClass="ClsLabel" Font-Size="11pt" Font-Bold="true" runat="server"
                                            Text="Formula Text"></asp:Label>
                                    </td>
                                    <td style="border: 1px solid;width:30%">
                                        <asp:Label ID="Label12" CssClass="ClsLabel" Font-Size="11pt" Font-Bold="true" runat="server"
                                            Text="Formula"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px; border: 1px solid;text-align:left;">
                                        <asp:Label ID="lblExample" runat="server" CssClass="ClsLabel" Font-Size="11pt">
                                        Solve following example [x = {-a \pm \sqrt{b^2-4ac} }.] <br /><br />
                                        Note - Add "\" before opening and closing square brackets.
                                        </asp:Label>
                                    </td>
                                    <td style="border: 1px solid;">
                                        <asp:Label ID="Label14" CssClass="ClsLabel" Font-Size="11pt" runat="server" Text="Solve following example  \[x = {-a \pm \sqrt{b^2-4ac} }.\]"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
