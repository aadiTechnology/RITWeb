<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentAnnualResultPrint.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="StudentAnnualResultPrint" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
	<style type="text/css">
		.Clspadding{padding:0 1px}
		.ClsBGWhite{background-color:#FFF}
		.PBorderBtm{font-family:Arial; font-size:8pt; font-weight:Normal; border-bottom-color:#000; border-bottom-style:dashed; border-bottom-width:1px}
		.PClsMarksCell{font-family:Arial; font-size:8pt; font-weight:Normal; padding-right:1px; background-color:#fff}
		.PClsTotalMarksCell{color:#000; font-family:Arial; font-size:8pt; font-weight:400; background-color:#fff; padding:0 1px}
		.PClsMarksGridHeader{font-weight:700; font-size:8pt; color:#333; text-decoration:none; padding-right:1px; height:20px; background-color:#fff}
		.PClsTestHeader{font-size:8pt; font-family:Verdana; color:#000; text-decoration:none; height:20px; background-color:#fff; padding:0 1px}
		.PClsMarksGridHeaderBG{font-weight:700; font-size:8pt; color:#333; text-decoration:none; padding-right:1px; padding-left:1px; height:20px; vertical-align:bottom; background-color:#fff}
		.PClsMarksGridRow{margin-left:1px; height:20px; font-weight:700; font-size:8pt; padding-left:1px; background-color:#fff}
		.PClsMarksGridAltRow{margin-left:1px; height:20px; font-weight:700; font-size:8pt; padding-left:1px; background-color:#fff}
		.PTotalType{font-weight:700; font-size:8pt; color:#000; background-color:#fff; padding:0 1px}
		.PTotalHead{background-color:#fff; font-weight:700; font-size:8pt; color:#000; padding:0 1px}
		.PClsHilightTextB{padding-left:1px; color:#000; font-size:11pt; font-weight:700}
		.PActualSchoolName{font-weight:700; font-family:Tahoma; color:#000; font-size:15pt; text-transform:capitalize; border-bottom:1px solid #ddd; background-color:#fff; padding:1px}
		.PSocietyName{font-weight:700; font-family:Tahoma; color:#000; font-size:12pt; text-transform:capitalize; border-bottom:1px solid #ddd; background-color:#fff; padding:1px}
		.PClsReportHead{font-weight:700; font-family:Tahoma; color:#000; text-transform:capitalize; font-size:13pt; border-bottom:1px solid #ddd; background-color:#fff; padding:1px}
		.Lbl8ptB{font-weight:700; font-style:normal; font-size:8pt; padding-left:1px}
		.ConfigHeadBG{background-color:#eaeaea}
		.LblSmlV{font-family:Verdana; font-size:8pt; color:#000}
		.LblSmlVP{font-family:Verdana; font-size:8pt; color:#000; padding:0 1px}
		.LblSmlVB{font-family:Verdana; font-size:8pt; color:#000; font-weight:700}
		.Dottedhr{border-bottom:#000 1px dashed; border-top:none; border-right:none; border-left:none; margin:21px 0}
		.ClspaddingL{padding-left:1px; text-align:center}
		.ClspaddingR{padding-right:1px}
	</style>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
	<table runat="server" id="tblMsg" visible="false" enableviewstate="false" width="60%" cellpadding="0" cellspacing="0" align="center">
		<tr>
			<td style="height: 100px;"></td>
		</tr>
		<tr>
			<td colspan="6" style="padding: 1px;" class="LblNoRecord" align="center">
				<asp:Label ID="lblErrorsMsg" Style="text-align: left" runat="server" Visible="False" EnableViewState="false" />
			</td>
		</tr>
	</table>
	<asp:Panel ID="Containers" runat="server" Style="padding-top: 50px; padding-left: 7%; padding-right: 8%;" EnableViewState="false" Visible="true">
	</asp:Panel>
	<script language="javascript" type="text/javascript">
		function PrintSheet() {
			_sClientlblErrorsMsg = "<%=this.lblErrorsMsg.ClientID %>";
			if (document.getElementById(_sClientlblErrorsMsg) == null)
				window.print();
			return false;
		}
		PrintSheet();
	</script>
</asp:Content>
