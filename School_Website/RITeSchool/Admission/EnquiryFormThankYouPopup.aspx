<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="EnquiryFormThankYouPopup.aspx.cs" Inherits="EnquiryFormThankYouPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr style="height: 30px;">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center" style="padding-left: 2px; font-weight: bold; border-style: solid;
                    border-color: White; padding-top: 20px; padding-bottom: 20px; font-size: 15px;">
                    Your enquiry form is submitted successfully.
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnClose" runat="server" Text="Close" Style="padding: 5px;font-weight:bold;width:100px;" onclick="btnClose_Click" />
                </td>
            </tr>
             <tr style="height: 30px;">
                <td>
                </td>
            </tr>
        </table>
        <script>

            function ClosePopup() {            
                window.close()
            }
        
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFooter" runat="Server">
</asp:Content>
