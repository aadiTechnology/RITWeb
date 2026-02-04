<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeacherIdentityCardUI.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="TeacherIdentityCardUI" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <style type="text/css">
		P.breakhere {page-break-before: always}
		#rotate { height: 30; width: 150px; float: left; filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3); }
    </style>
	<script language="javascript" type="text/javascript">
		function PrintSheet() {
			window.print();
			return false;
		}
	</script>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
    <table id="GridViewContainer" runat="server" enableviewstate="false" visible="true" >
    </table>
	<script language="javascript" type="text/javascript">
		PrintSheet();
	</script>
</asp:Content>