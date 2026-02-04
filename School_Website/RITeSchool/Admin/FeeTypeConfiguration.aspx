<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeeTypeConfiguration.aspx.cs" Inherits="FeeTypeConfiguration" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
	<div class="MainBodyDiv">
		<table align="center" border="0" cellpadding="0" cellspacing="0" width="97%" style="margin: 10px 0;">
			<tr>
				<td>
					<table border="0" cellpadding="0" cellspacing="0" width="100%">
						<tr>
							<td valign="top">
								<asp:ValidationSummary ID="valSumErrorMsg"
													   runat="server"
													   CssClass="LblErrorMsg"
													   ShowSummary="false"
													   ShowMessageBox="true" />
							</td>
							<td>
							</td>
						</tr>
						<tr align="center">
							<td>
								<asp:Label ID="lblErr"
										   runat="server"
										   CssClass="LblErrorMsg"
										   EnableViewState="false" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align="center">
					<div>
						<asp:GridView ID="grdFeeTypeConfiguration"
									  runat="server"
									  Width="800px"
									  CellPadding="2"
									  CellSpacing="1"
									  AutoGenerateColumns="False"
									  PageSize="20"
									  ForeColor="#333333"
									  GridLines="None"
									  OnRowDataBound="grdFeeTypeConfiguration_RowDataBound"
									  DataKeyNames="Fee_Type_Id,School_Id,Original_Fee_Type_Id,ConsiderForITReconciliation,ConsiderForBifurcation,ConsiderForRTEConcession"
									  class="GridBorder">
							<PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial" Font-Size="Small" />
							<PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous" FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast" />
							<Columns>
								<asp:TemplateField>
									<HeaderTemplate>
										<input id="ChkWeekDays" runat="server" type="checkbox" onclick="CheckAllCheckBox(this);" />
									</HeaderTemplate>
									<ItemTemplate>
										<asp:CheckBox ID="ChkAllCheckedWeekDays" runat="server" />
									</ItemTemplate>
									<ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Middle" />
									<HeaderStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Middle" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText= "<%$ Resources:LocalizedResources, FeeTypeName%>" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paddingLSML">
									<ItemTemplate>
										<asp:TextBox ID="txtFeeType"
													 runat="server"
													 MaxLength="50"
													 Width="200px"
													 CssClass="LrgTxtBox"
													 Text='<%# Eval("Fee_Type") %>' />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" />
									<HeaderStyle Font-Size="9pt" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText= "<%$ Resources:LocalizedResources, ConsiderForITReconciliation%>" HeaderStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<asp:CheckBox ID="chkITR" runat="server" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
									<HeaderStyle Font-Size="9pt" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText= "<%$ Resources:LocalizedResources, ConsiderForBifurcation%>" HeaderStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<asp:CheckBox ID="chkBifurcate" runat="server" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
									<HeaderStyle Font-Size="9pt" />
								</asp:TemplateField>
                                <asp:TemplateField HeaderText= "Consider For RTE Concession?" HeaderStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<asp:CheckBox ID="chkRTE" runat="server" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
									<HeaderStyle Font-Size="9pt" />
								</asp:TemplateField>
							</Columns>
							<RowStyle CssClass="ClsGridRow" />
							<HeaderStyle CssClass="ClsGridHeader" />
							<AlternatingRowStyle CssClass="ClsGridAltRow" />
							<EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
						</asp:GridView>
					</div>
				</td>
			</tr>
			<tr id="trLedgerNotice" runat="server">
				<td align="center" style="padding: 0;">
					<table cellspacing="2" style="width: 604px; margin-top: 10px;">
						<tr>
							<td align="center" class="ClsBorderlight " style="background-color: #ffffc4; width: 50px;">
								<%--<span class="LblNrmlB" style="font-weight: bold; padding: 2px 5px;">Note : </span>--%>
                                  <asp:Label  ID="lblNote" CssClass="LblNrmlB" runat="server" EnableViewState="False" Font-Bold= "true"  Text="<%$ Resources:LocalizedResources, Note %>"></asp:Label>
                                  <span class="colonPadding">:</span>
							</td>
							<td align="left" class="ClsBorderlight" style="padding: 4px 5px;">
								
                                  <asp:Label  ID="lblNoteForFeeType" CssClass="LblSmlV" runat="server" EnableViewState="False" Font-Bold= "true"  Text="<%$ Resources:LocalizedResources, NoteForFeeType %>"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align="center">
					<table border="0" cellpadding="0" cellspacing="2">
						<tr>
							<td>
								<asp:CustomValidator ID="cstValidateType"
													 runat="server"
													 EnableClientScript="true"
													 ClientValidationFunction="cstValidateTypes"
													 Display="None"
													 ErrorMessage=""
													 SetFocusOnError="true" />
								<asp:Button ID="btnSave"
											runat="server"
											Text= "<%$ Resources:LocalizedResources,Save%>"
											OnClick="btnSave_Click"
											CssClass="ClsBtn"
											disable-page="true"
											CausesValidation="true" />
							</td>
							<td>
								<asp:Button ID="btnCancel"
											runat="server"
											Text= "<%$ Resources:LocalizedResources, Cancel%>"
											CssClass="ClsBtn"
											CausesValidation="False"
											UseSubmitBehavior="false" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<script language="javascript" type="text/javascript">
		_clientGridId = "<%=this.grdFeeTypeConfiguration.ClientID %>";
		_clientbtnSave = "<%=this.btnSave.ClientID %>";
		_clientbtnCancel = "<%=this.btnCancel.ClientID %>";
		_clientcstValidateType = "<%=this.cstValidateType.ClientID %>";
		_clientvalSumErrorMsg = "<%=this.valSumErrorMsg.ClientID %>";
		_clientlblErr = "<%=this.lblErr.ClientID %>";

		var Page_IsValid = true;
		function ConfirmAction(iPageCount, sActionName) {
			 Page_IsValid = true;
			var bResult = true;
			if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkAllCheckedWeekDays', sActionName, 'false', iPageCount, 'true')) {
				bResult = true;
			}
			else {
				 Page_IsValid = false;
				bResult = false;
			}
			return bResult;
		}

		function DisableButtons() {
			$get(_clientbtnSave).disabled = true;
			$get(_clientbtnCancel).disabled = true;
		}

		function CheckAllCheckBox(chk) {
			$('#<%=grdFeeTypeConfiguration.ClientID %> input:checkbox').attr('checked', chk.checked);

			var grid = $get(_clientGridId);
			var icount = grid.rows.length;
			for (var i = 0; i < icount - 1; i++)
				ChkOnChange(i);
		}

		function cstValidateTypes(src, args) {
			var Msg = "";
			var Msg1 = "";
			var cntrl;
			var grid = $get(_clientGridId);
			var icount = grid.rows.length;
			var iRowCount = 0;
			for (var i = 0; i < icount - 1; i++) {

				iRowCount = i + 2;
				var checkbox;
				cntrl = iRowCount < 10 ? "_ctl0" : "_ctl";

				checkbox = $get(_clientGridId + cntrl + iRowCount + '_ChkAllCheckedWeekDays');

				if (checkbox != null && checkbox.checked) {
					var txt;

					txt = $get(_clientGridId + cntrl + iRowCount + '_txtFeeType').value;
					if (txt.trim() == "")
						Msg1 = "1";
					var iRowCount1 = 0;
					for (var j = 0; j < icount - 1; j++) {

						iRowCount1 = j + 2;
						var cntrl1;
						var checkbox1;

						cntrl1 = iRowCount1 < 10 ? "_ctl0" : "_ctl";

						checkbox1 = $get(_clientGridId + cntrl1 + iRowCount1 + '_ChkAllCheckedWeekDays');

						if (j != i && checkbox1 != null && checkbox1.checked) {
							var txt1;
							txt1 = $get(_clientGridId + cntrl1 + iRowCount1 + '_txtFeeType').value;
							if (txt.trim().toUpperCase() == txt1.trim().toUpperCase()) {
								if (!Msg.match((txt1)))
									Msg = Msg + txt1 + ", ";
							}
						}
					}
				}
			}

			if (Msg1 != "") {
				$get(_clientlblErr).innerHTML = "";
				($get(_clientcstValidateType)).errormessage = document.getElementById("<%=this.hidvalFeeTypeName.ClientID %>").value;
				args.IsValid = false;
				return true;
			}

			else if (Msg != "") {
				Msg = Msg.substring(0, Msg.length - 2);
				$get(_clientlblErr).innerHTML = "";
				($get(_clientcstValidateType)).errormessage = document.getElementById("<%=this.hidFeeTypeName.ClientID %>").value + " " +  Msg +  " "+ document.getElementById("<%=this.hidvalShouleNotBeDuplicated.ClientID %>").value;
				args.IsValid = false;
				return true;
			}
			args.IsValid = true;
			return false;
		}

		// This function is used to enable/disable textbox when it's checkbox is clicked.
		function ChkOnChange(iRowCount) {

			iRowCount = iRowCount + 2;
			var cntrl;
			var txt;
			var chkITR;
			
			cntrl = iRowCount < 10 ? "_ctl0" : "_ctl";

			var checkbox = $get(_clientGridId + cntrl + iRowCount + '_ChkAllCheckedWeekDays');
			txt = $get(_clientGridId + cntrl + iRowCount + '_txtFeeType');
			chkITR = $get(_clientGridId + cntrl + iRowCount + '_chkITR');
			var chkBifurcate = $get(_clientGridId + cntrl + iRowCount + '_chkBifurcate');
			var chkIsRTE = $get(_clientGridId + cntrl + iRowCount + '_chkRTE');

			if (checkbox != null && checkbox.checked) {
				txt.disabled = chkITR.disabled = chkBifurcate.disabled = chkIsRTE.disabled = false;
			}
			else {
				txt.disabled = chkITR.disabled = chkBifurcate.disabled = chkIsRTE.disabled = true;
				chkITR.checked = false;
				chkBifurcate.checked = false;
				chkIsRTE.checked = false;
			}
		}
	</script>
	<asp:HiddenField ID="hidConfigurationFlag" runat="server" />
    <asp:HiddenField ID = "hidvalFeeTypeName" runat = "server" />
    <asp:HiddenField ID = "hidvalShouleNotBeDuplicated" runat = "server" />
    <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
    <asp:HiddenField ID = "hidFeeTypeName" runat = "server" />
</asp:Content>
