<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdmissionFormReport.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="AdmissionFormReport" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
	<title>Admission Form</title>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
<table style="margin:10px auto;width:300px" id="tblButtons" runat="server" visible="false">
    <tr>
        <td align="center">
            <asp:Button ID="btnAdminCopy" runat="server" Text="ADMINISTRATION COPY" 
                CssClass="ClsBtn" Width="200px" onclick="btnAdminCopy_Click" />
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Button ID="btnTeachersCopy" runat="server" Text="TEACHER'S COPY" 
                CssClass="ClsBtn" Width="200px" onclick="btnTeachersCopy_Click" />
        </td>
    </tr>
</table>
		<asp:HiddenField ID="hidStudentAdmissionId" runat="server" />
        <asp:HiddenField ID="hidEnquiryId" runat="server" />
</asp:Content>