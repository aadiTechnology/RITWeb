<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="ViewQueryDetailsPopup.aspx.cs" Inherits="ViewQueryDetailsPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
    <style type="text/css">
        .ClsReceiverHeader
        {
            font-weight: 700;
            font-size: 9pt;
            color: #006;
            text-decoration: none;
            padding-right: 5px;
            height: 20px;
            background-color: #D9E8AA;
            border-bottom-style: solid;
            border-bottom-width: 1px;
        }

        .ClsReceiverCell
        {
            background-color: #E4EFC4;
            font-family: Arial;
            font-size: 9pt;
            padding-right: 5px;
        }
    </style>    
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="left">
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="left" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">Communications</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="90%" id="tblCommunications" runat="server">
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="window.close()" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            function OpenFile(fileName) {
                window.open('../Downloads/AskMe/' + fileName);
            }
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
