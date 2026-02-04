<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="AdmissionFormPopup.aspx.cs" Inherits="AdmissionFormPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%" style="height: 150px;">
        <tr>
            <td align="center">
                <table width="50%" id="tblLinks" runat="server" visible="false">
                    <tr>
                        <td align="center">
                            <span style="font-weight: bold; font-size: 20px;">Admission Details</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <hr style="color: #C0C0C0; border: thin solid #C0C0C0" />
                        </td>
                    </tr>
                    <tr style="height:10px;">
                        <td align="center">                            
                        </td>
                    </tr>
                    <tr id="trPaymentReceipt" runat="server">
                        <td class="TxtNormal" align="center" style="font-weight: bold">                            
                            <asp:HyperLink ID="hlnkReceipt" runat="server" NavigateUrl="../Accountant/FeesMiniReceipt.aspx">Click here</asp:HyperLink>
                            &nbsp;to view your payment receipt.
                        </td>
                    </tr>
                     <tr style="height:25px;">
                        <td align="center">                            
                        </td>
                    </tr>
                    <tr>
                        <td class="TxtNormal" align="center" style="font-weight: bold">                            
                            <asp:HyperLink ID="hlnkAdmissionForm" runat="server" NavigateUrl="../Admission/AdmissionFormReport.aspx">Click here</asp:HyperLink>
                            &nbsp;to view admission form.
                        </td>
                    </tr>
                     <tr style="height:25px;">
                        <td align="center">                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFooter" runat="Server">
</asp:Content>
