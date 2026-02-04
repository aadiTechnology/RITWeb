<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XseedProgressReportPrint.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="XseedProgressReportPrint" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
	<link href="../Styles/XseedProgressReport.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
	<div>
		<table align="center" border="0" cellpadding="0" style="width: 100%; vertical-align: top">
			<tr>
				<td align="center">
					<table width="100%">
						<tr id="tblProgressReportDetails" runat="server">
							<td align="center">
								<table id="tblMainProgressReport" runat="server" width="850px"></table>
							</td>
						</tr>
						<tr id="trErrorMessage" runat="server" visible="false">
							<td align="center" width="100%">
								<asp:Label ID="lblErrorMsg" runat="server" CssClass="LblNoRecord" Text="<%$ Resources:LocalizedResources, MsgAssessmentResultUnAvailable %>"  EnableViewState="False" />
								<asp:HiddenField ID="hidAssessment" runat="server" Value="0" />
								<asp:HiddenField ID="hidstdDivId" runat="server" Value="0" />
								<asp:HiddenField ID="hidStudentId" runat="server" Value="0" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<script type="text/javascript" language="javascript">
			window.print();
		</script>
	</div>
</asp:Content>