<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
	AutoEventWireup="true" CodeFile="ViewHomeworkUI.aspx.cs" Inherits="ViewHomeworkUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
	<table width="700px;">
		<tr>
			<td colspan="2" align="left">
				<table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 100%;
					padding-right: 5px;">
					<tr>
						<td style="height: 20px">
							<asp:Label ID="lblSelectUser" runat="server" Font-Bold="True" Text="View Homework"
								EnableViewState="false"></asp:Label>
						</td>
					</tr>
					<tr>
						<td></td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td></td>
		</tr>
		<tr>
			<td class="ClsBorderlight paddingL" style="width: 150px;">
				Subject :
			</td>
			<td class="LblNormal ClsBorderlight">
				<asp:Label ID="lblSubject" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="ClsBorderlight paddingL" style="width: 150px;">
				Title :
			</td>
			<td class="LblNormal ClsBorderlight">
				<asp:Label ID="lblTitle" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="ClsBorderlight paddingL">
				Assigned Date :
			</td>
			<td class="LblNormal ClsBorderlight">
				<asp:Label ID="lblAssignedDt" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="ClsBorderlight paddingL">
				Complete By Date :
			</td>
			<td class="LblNormal ClsBorderlight">
				<asp:Label ID="lblCompleteDt" runat="server"></asp:Label>
			</td>
		</tr>
		<tr style="display:none;">
			<td class="ClsBorderlight paddingL">
				Attachment :
			</td>
			<td class="LblNormal ClsBorderlight">
				<asp:HyperLink ID="lnkAttachment" runat="server"></asp:HyperLink>
			</td>
		</tr>
        <tr>
			<td class="ClsBorderlight paddingL">
				More Attachment(s) :
			</td>
			<td class="LblNormal ClsBorderlight">
				<asp:LinkButton ID="lnkAddAttachments" runat="server" CssClass="clsLabel" Text="More Attachments"></asp:LinkButton>
			</td>
		</tr>
		<tr>
			<td class="ClsBorderlight paddingL ">
				Details :
			</td>
			<td class="LblNormal ">
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:TextBox ID="txtDetails" ReadOnly="true" runat="server" TextMode="MultiLine"
					Height="200px" Width="700px"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td align="center" colspan="2">
				<asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="ClsBtn" />
			</td>
		</tr>
	</table>
	<script type="text/javascript">
		_clientbtnClose = "<%=this.btnCancel.ClientID %>"
		$("#" + _clientbtnClose).click(function () {
			window.close();
		});
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
