<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
	AutoEventWireup="true" CodeFile="XseedProgressReportUI.aspx.cs" Inherits="XseedProgressReportUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" style="width: 100%; vertical-align: top">	
        <tr>
			<td align="center">
				<table id="tblMain" runat="server" width="100%">
					<tr>
						<td>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr id="trHeader" runat="server" visible="false">
									<td class="ClsGrayMainTitle" width="98%" height="20px">
										<table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; height: 15px">
											<tr>
												<td align="center">
													<asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" ID="UpdatePanel3"
														runat="server" ViewStateMode="Enabled">
														<ContentTemplate>
															<span class="MainTitleHead">
                                                            <asp:Label ID="lblOldAcademicRecordText" runat="server" Text="<%$ Resources:LocalizedResources, OldAcademicRecord %>"></asp:Label>
                                                            </span>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr id="trOldAcademicDetails" runat="server">
						<td>
							<table width="100%">
								<tr>
									<td align="left">
										<asp:ValidationSummary ID="valSummary" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" ForeColor="Red" />
									</td>
									<td align="right">
										<span class="ClsLabelNrml" style="color: Red;">* 
                                        <asp:Label ID="Label1" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                        </span>
									</td>
								</tr>
								<tr>
									<td>
										<table>
											<tr>
												<td align="left" width="100px" class="ClsBorderlight" id="tdAcademicYrs" runat="server" visible="false">
													<span class="ClsLabel" id="lblacademicYear" style="height: 16px; width: 95px" runat="server">
														<asp:Label ID="Label2" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, AcademicYear %>"></asp:Label> :</span>
												</td>
												<td align="left" width="100px">
													<asp:DropDownList ID="cmbAcademicYear" runat="server" ViewStateMode="Enabled" AutoPostBack="true" Width="100px"
														OnSelectedIndexChanged="cmbAcademicYear_SelectedIndexChanged">
													</asp:DropDownList>
												</td>
												<td class="ErrHeadNew" align="left" colspan="3">
													<asp:Label ID="lblOldAcademicYear" runat="server" EnableViewState="False"></asp:Label>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>      
                    <tr>
                        <td align="right">
                            <asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen" Height="15px" ID="hlnkOldAcademicRecord"
							    NavigateUrl="#" runat="server" ViewStateMode="Enabled" Target="_blank" Text="<%$ Resources:LocalizedResources, OldAcademicRecords %>"></asp:HyperLink>
                        </td>
                    </tr>              
					<tr id="trFilterSelection" runat="server">
						<td align="center">
							<table>
								<tr id="trStudentDetails" runat="server">
									<td width="110px" class="ClsBorderlight" id="tdClassTeacher" runat="server">
										<span class="ClsLabel"><asp:Label ID="Label3" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, ClassTeacher %>"></asp:Label> :</span>
									</td>
									<td id="tdcmbClassTeacher" runat="server">
										<asp:DropDownList ID="cmbClassTeacher" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" AutoPostBack="true"
											OnSelectedIndexChanged="cmbClassTeacher_SelectedIndexChanged">
										</asp:DropDownList>
										<span class="ClsMdtStar">*</span>
									</td>
									<td width="70px" class="ClsBorderlight" id="tdStudents" runat="server">
										<span class="ClsLabel">
                                        <asp:Label ID="Label4" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Student %>"></asp:Label> :</span>
									</td>
									<td id="tdCmbStudents" runat="server">
										<asp:UpdatePanel ID="upnl1" runat="server" ViewStateMode="Enabled" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:DropDownList ID="cmbStudents" runat="server" CssClass="LrgCombo" AutoPostBack="true"
													OnSelectedIndexChanged="cmbStudents_SelectedIndexChanged">
													<asp:ListItem Text="-- All --" Value="0"></asp:ListItem>
												</asp:DropDownList>
											</ContentTemplate>
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="cmbClassTeacher" EventName="SelectedIndexChanged" />
											</Triggers>
										</asp:UpdatePanel>
									</td>
									<td>
									</td>
								</tr>
								<tr id="tmpRow" runat="server">
									<td id="tdAssemetns" runat="server" width="100px" class="ClsBorderlight">
										<span class="ClsLabel"><asp:Label ID="Label5" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Assessment %>"></asp:Label> :</span>
									</td>
									<td id="tdcmbAssemetns" runat="server">
										<asp:UpdatePanel ID="UpdatePanel1" runat="server" ViewStateMode="Enabled" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:DropDownList ID="cmbAssessment" runat="server" CssClass="LrgCombo" AutoPostBack="true"
													OnSelectedIndexChanged="cmbAssessment_SelectedIndexChanged">
												</asp:DropDownList>
												<asp:Label ID="lblMandetory" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
													Text="*"></asp:Label>
											</ContentTemplate>
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="cmbClassTeacher" EventName="SelectedIndexChanged" />
											</Triggers>
										</asp:UpdatePanel>
									</td>
									<td align="left" colspan="2">
										<table>
											<tr>
												<td align="center" id="tdShow" runat="server">
													<asp:Button ID="btnShow" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Show %>" CssClass="ClsBtn" OnClick="btnShow_Click" />
												</td>
												<td align="center" id="tdPrintPreview" runat="server">
													<asp:UpdatePanel ID="upnlPrint" runat="server" ViewStateMode="Enabled">
														<ContentTemplate>
															<asp:Button ID="btnPrintPreview" runat="server" Text="<%$ Resources:LocalizedResources, PrintPreview %>" CssClass="ClsBtn"
																Style="width: 110px;" OnClick="btnPrintPreview_Click" />
															<asp:HiddenField ID="hidQueryString" runat="server" Value="" />
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
                                                <td align="right" id="tdDownloadPDF" runat="server" visible="false">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnDownload" runat="server" CausesValidation="false" CssClass="ClsBtnMid"                                                                                    
                                                                Text="Download PDF" onclick="btnDownload_Click"/>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnDownload" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
												<%--<td align="center">
													<asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen" Height="15px" ID="hlnkOldAcademicRecord"
														NavigateUrl="#" runat="server" Target="_blank" Text="<%$ Resources:LocalizedResources, OldAcademicRecords %>"></asp:HyperLink>
												</td>--%>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ViewStateMode="Enabled" InitialValue="0"
											Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ClassTeacherShouldBeSelected %>" ControlToValidate="cmbClassTeacher"></asp:RequiredFieldValidator>
									</td>
									<td>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ViewStateMode="Enabled" InitialValue="0"
											Display="None" ErrorMessage="<%$ Resources:LocalizedResources, AssessmentShouldSelected %>" ControlToValidate="cmbAssessment"></asp:RequiredFieldValidator>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr id="tblProgressReportDetails" runat="server">
						<td align="center">
							<asp:UpdatePanel ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server"
								ID="uPnl" ViewStateMode="Enabled">
								<ContentTemplate>
									<table id="tblMainProgressReport" runat="server" width="850px">
									</table>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
									<asp:AsyncPostBackTrigger ControlID="cmbAssessment" EventName="SelectedIndexChanged" />
									<asp:AsyncPostBackTrigger ControlID="cmbClassTeacher" EventName="SelectedIndexChanged" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					</tr>
					<tr id="trErrorMessage" runat="server" visible="false">
						<td align="center">
							<asp:Label ID="lblErrorMsg" Width="90%" runat="server" CssClass="LblNoRecord" EnableViewState="False"></asp:Label>
						</td>
					</tr>
					<tr>
						<td style="height: 10px">
						</td>
					</tr>
					<tr id="trBloclkProgress" runat="server" visible="false">
						<td align="center">
							<asp:Label ID="lblBlockProgressReortReason" Width="90%" runat="server" CssClass="LblNoRecord"
								EnableViewState="False"></asp:Label>
						</td>
					</tr>
				</table>
			</td>
		</tr>
        <tr id="trPrecondition" runat="server">
			<td>
					<div runat="server" id="divErr">
				</div>
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:Button ID="btnBack" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Back %>" CausesValidation="false" />
				<asp:HiddenField ID="hidAssessment" runat="server" Value="0" ViewStateMode="Enabled"/>
				<asp:HiddenField ID="hidstdDivId" runat="server" Value="0" ViewStateMode="Enabled"/>
				<asp:HiddenField ID="hidStudentId" runat="server" Value="0" ViewStateMode="Enabled"/>
				<asp:HiddenField ID="hidIsOldReport" runat="server" Value="N" ViewStateMode="Enabled"/>
                <asp:HiddenField ID="hidCultureInfo" runat="server" ViewStateMode="Enabled"/>
                <asp:HiddenField ID="hidBtnBack" runat="server" ViewStateMode="Enabled"/>
                <asp:HiddenField ID="hidShowCurrentYearData" runat="server" Value="0" ViewStateMode="Enabled"/>
			</td>
		</tr>
	</table>
	<style type="text/css">
		.ProgressReportHeader
		{
			font-weight: 700;
			font-size: 10pt;
			color: #333;
			text-decoration: none;
			height: 20px;
			background-color: #c8dffe;
		}
		.StudentDetailsHeader
		{
			font-weight: 700;
			font-size: 10pt;
			color: #333;
			text-decoration: none;
			height: 20px;
			padding-left: 5px;
			background-color: #c8dffe;
		}
	</style>
	<script type="text/javascript" language="javascript">
		_clientbtnPrintPreview = "<%=this.btnPrintPreview.ClientID %>";
		_clienthidQueryString = "<%=this.hidQueryString.ClientID %>";
		_clientcmbAssessment = "<%=this.cmbAssessment.ClientID %>";

		var prm = Sys.WebForms.PageRequestManager.getInstance()
		prm.add_endRequest(EndReqHandler)
		prm.add_beginRequest(beginRequestHandler)
		function EndReqHandler(sender, args) {
			var postBackElement = sender._postBackSettings.sourceElement
			if (postBackElement.id == _clientbtnPrintPreview) {
				var queryString = document.getElementById(_clienthidQueryString).value;
				window.open('XseedProgressReportPrint.aspx?' + queryString, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=1050,height=700').focus();
			}
		}
		function beginRequestHandler(sender, args) {
		}

		function ShowOldProgressReports(queryStrung) {
			window.open(queryStrung, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1050,height=700').focus();
		}
       
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
