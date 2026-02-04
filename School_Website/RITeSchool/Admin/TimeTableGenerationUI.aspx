<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TimeTableGenerationUI.aspx.cs" Inherits="TimeTableGenerationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <table align="center" width="80%">
                                <tr>
                                    <td align="center" class="ClsHilightBGB">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Auto Timetable Generation"
                                            EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="valSumTimetable" runat="server" CssClass="ErrMsg" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trTimeTable" runat="server">
                        <td>
                            <table align="center" width="80%">
                                <tr>
                                    <td align="center" class="ClsBorderlight">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" CssClass="ClsLabelNrml" Text="Click on Download button to download XML file for generating time table using timeTable software"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnDownload" runat="server" CssClass="ClsBtn" Text="Download" CausesValidation="false"
                                                        OnClick="btnDownload_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr> 
                                                <td align="center">
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabelNrml" EnableViewState="false"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="reqValFile" runat="server" CssClass="ClsLabelNrml"
                                                        Font-Bold="true" ForeColor="Red" ControlToValidate="flUploadXML" ErrorMessage="File should be selected to upload."
                                                        Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="ClsBorderlight">
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="Label3" runat="server" CssClass="ClsLabelNrml" Text="Upload time table XML file generated from timeTable software"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:FileUpload CssClass="LrgTxtBox" ID="flUploadXML" runat="server" />
                                                            </td>
                                                            <td width="5px">
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnUpload" runat="server" CssClass="ClsBtn" Text="Upload" OnClick="btnUpload_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <span class="LblSmlGray">(Supports only .XML files type)</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div runat="server" id="divErr">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientlblMessage = "<%= this.lblMessage.ClientID %>";
        function HideLabel() {
            $get(_clientlblMessage).innerHTML = '';
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
