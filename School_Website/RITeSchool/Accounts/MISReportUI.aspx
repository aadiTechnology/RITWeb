<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="MISReportUI.aspx.cs" Inherits="MISReportUI" %>

<asp:Content ID="contentMainBody" ContentPlaceHolderID="MainBody" Runat="Server">
<style type="text/css">
#btnExpandCollapse{display:inline-block;padding:5px 0 0 5px;color:Blue;text-decoration: underline;cursor:pointer;}
#reportWrapper{border:1px solid #ACBB8F;margin:8px 3px;}
#reportWrapper .inner{margin: 0 5px 5px 5px;}
.overflow .inner{overflow:auto;width:1080px;}
.sectionWrapper{margin: 5px 0;}
.overflow .sectionWrapper{width:1720px;}
.sectionFooter{margin-top: 5px;}
.sectionTotal,.sectionFooter td{font-weight: bold;}
.vwAnnual,.vwTerm,.vwQuarter, .vwMonth,.Annual .expand .vwAnnual,.Term .expand .vwTerm,.Quarter .expand .vwQuarter,.Month .expand .vwMonth,.expand .budget,.expand .variance{display: none;}
.Annual .vwAnnual,.Term .vwTerm,.Quarter .vwQuarter,.Month .vwMonth,.collapse .budget,.collapse .variance{display: table-cell;}
.node{padding-left: 17px;cursor: pointer;}
.parent.expand .node{background: url("../images/node_close.gif") no-repeat scroll 2px 1px transparent;}
.parent.collapse .node{background: url("../images/node_open.gif") no-repeat scroll 2px 1px transparent;}
.neg{color: red;}
</style>
<!--[if lt IE 9]>
<style id="styleIE" type="text/css">
.sectionView .parent td {background-color: #D6E1B7; font-weight: bold;}
.Annual .vwAnnual,.Term .vwTerm,.Quarter .vwQuarter,.collapse .budget,.collapse .variance {display: inline;}
</style>
<![endif]-->
<asp:UpdatePanel ID="updatePanelMain"
				 runat="server">
	<ContentTemplate>
		<table id="tblHeader" runat="server" width="100%">
            <tr>
                <td align="center" style="height: 20px; width: 99%; margin-bottom: 5px;" class="ClsGrayMainTitle">
                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                        <tr>
                            <td class="MainTitleHead" style="height: 20px">
                                <span style="font-weight: bold">MIS Report</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
		<table id="reportContainer" class="Annual" cellpadding="0" cellspacing="0" style="font-size: 9pt; margin-top: 10px;">
			<tr id="trViewTypeRow" runat="server">
				<td align="left">
					<table cellpadding="0">
						<tr>
							<td class="ClsBorderlight">
								<span class="ClsLabel" style="padding: 5px;">Report View :</span>
							</td>
							<td class="ClsBorderlight">
								<asp:RadioButtonList ID="rdlstReportView"
													 runat="server"
													 AutoPostBack="false"
													 EnableViewState="false"
													 RepeatColumns="4"
													 RepeatDirection="Horizontal"
													 style="font-size: 9pt;" >
									<asp:ListItem Text="Annual"
												  Value="1"
												  Enabled="true"
												  Selected="true" />
									<asp:ListItem Text="Bi-Annual"
												  Value="2" />
									<asp:ListItem Text="Quarterly"
												  Value="3" />
									<asp:ListItem Text="Monthly"
												  Value="4" />
								</asp:RadioButtonList>
							</td>
						</tr>
					</table>
				</td>
				<td align="right">
					<table id="tblStudentCount" runat="server" cellpadding="5">
						<tr>
							<td class="ClsBorderlight">
								<span class="ClsLabel" style="padding: 5px;">Total Students :</span>
							</td>
							<td class="ClsHilightBG" style="font-weight: bold;">
								<asp:HyperLink ID="hlnkStudentCount"
											   runat="server"
											   NavigateUrl="../Admin/AllStudentsUI.aspx"
											   Target="_blank" />
							</td>
						</tr>
					</table>
				</td>
			</tr> 
			<tr id="trExpandCollapse" runat="server">
				<td colspan="2" align="left">
					<span id="btnExpandCollapse">Expand/Collapse All</span>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<div id="reportWrapper">
						<div class="inner">
							<asp:ListView ID="lstvwMISReport"
										  runat="server"
										  DataKeyNames="Title"
										  OnItemDataBound="lstvwMISReport_OnItemDataBound">
								<LayoutTemplate>
									<div id="itemPlaceholder" runat="server"></div>
								</LayoutTemplate>
								<ItemTemplate>
									<div class="sectionWrapper">
										<div class="sectionHeader"><%# Eval("Title") %></div>
										<div class="sectionView">
											<div class="sectionContainer">
												<asp:ListView ID="lstvwSection"
														  runat="server"
														  DataSource='<%# Eval("MISReportGroups") %>'
														  OnItemDataBound="lstvwSection_OnItemDataBound"
														  OnDataBound="lstvwSection_OnDataBound">
												<LayoutTemplate>
													<table cellpadding="3" cellspacing="1" style="font-size: 9pt; width: 100%;">
														<tr class="tableHeader">
															<th align="left" style="width: 220px;">Name</th>
															<th align="center" style="width: 135px;" class="vwAnnual">
																<asp:Label ID="lblAnnualTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 135px;" class="vwTerm">
																<asp:Label ID="lblTerm1Title"
																		   runat="server" />
															</th>
															<th align="center" style="width: 135px;" class="vwTerm">
																<asp:Label ID="lblTerm2Title"
																		   runat="server" />
															</th>
															<th align="center" style="width: 135px;" class="vwQuarter">
																<asp:Label ID="lblQuarter1Title"
																		   runat="server" />
															</th>
															<th align="center" style="width: 135px;" class="vwQuarter">
																<asp:Label ID="lblQuarter2Title"
																		   runat="server" />
															</th>
															<th align="center" style="width: 135px;" class="vwQuarter">
																<asp:Label ID="lblQuarter3Title"
																		   runat="server" />
															</th>
															<th align="center" style="width: 135px;" class="vwQuarter">
																<asp:Label ID="lblQuarter4Title"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblAprTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblMayTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblJunTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblJulTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblAugTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblSepTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblOctTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblNovTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblDecTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblJanTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblFebTitle"
																		   runat="server" />
															</th>
															<th align="center" style="width: 100px;" class="vwMonth">
																<asp:Label ID="lblMarTitle"
																		   runat="server" />
															</th>
															<th align="right" style="width: 100px;">Budget (Rs.)</th>
															<th align="right" style="width: 100px;">Variance (Rs.)</th>
														</tr>
														<tr id="itemPlaceholder" runat="server"></tr>
														<tr class="sectionTotal">
															<td align="right">Total (Rs.) :</td>
															<td align="right" class="vwAnnual">
																<asp:Label ID="lblAnnualTotal"
																		   runat="server" />
															</td>
															<td align="right" class="vwTerm">
																<asp:Label ID="lblTerm1Total"
																		   runat="server" />
															</td>
															<td align="right" class="vwTerm">
																<asp:Label ID="lblTerm2Total"
																		   runat="server" />
															</td>
															<td align="right" class="vwQuarter">
																<asp:Label ID="lblQuarter1Total"
																		   runat="server" />
															</td>
															<td align="right" class="vwQuarter">
																<asp:Label ID="lblQuarter2Total"
																		   runat="server" />
															</td>
															<td align="right" class="vwQuarter">
																<asp:Label ID="lblQuarter3Total"
																		   runat="server" />
															</td>
															<td align="right" class="vwQuarter">
																<asp:Label ID="lblQuarter4Total"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblAprTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblMayTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblJunTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblJulTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblAugTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblSepTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblOctTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblNovTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblDecTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblJanTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblFebTotal"
																		   runat="server"/>
															</td>
															<td align="right" class="vwMonth">
																<asp:Label ID="lblMarTotal"
																		   runat="server"/>
															</td>
															<td align="right">
																<asp:Label ID="lblBudgetTotal"
																		   runat="server" />
															</td>
															<td align="right">
																<asp:Label ID="lblVarianceTotal"
																		   runat="server" />
															</td>
														</tr>
													</table>
												</LayoutTemplate>
												<ItemTemplate>
													<tr class="parent expand">
														<td class="node" colspan="4"><%# Eval("Name") %></td>
														<td align="right" class="vwAnnual">
															<asp:Label ID="lblAnnualGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwTerm">
															<asp:Label ID="lblTerm1GroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwTerm">
															<asp:Label ID="lblTerm2GroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwQuarter">
															<asp:Label ID="lblQuarter1GroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwQuarter">
															<asp:Label ID="lblQuarter2GroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwQuarter">
															<asp:Label ID="lblQuarter3GroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwQuarter">
															<asp:Label ID="lblQuarter4GroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblAprGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblMayGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblJunGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblJulGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblAugGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblSepGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblOctGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblNovGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblDecGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblJanGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblFebGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="vwMonth">
															<asp:Label ID="lblMarGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="budget">
															<asp:Label ID="lblBudgetGroupTotal"
																	   runat="server" />
														</td>
														<td align="right" class="variance">
															<asp:Label ID="lblVarianceGroupTotal"
																	   runat="server" />
														</td>
													</tr>
													<asp:ListView ID="lstvwInner"
																	runat="server"
																	DataSource='<%# Eval("MISReportLedgers") %>'
																	OnItemDataBound="lstvwInner_OnItemDataBound">
														<LayoutTemplate>
															<tr id="itemPlaceholder" runat="server"></tr>
														</LayoutTemplate>
														<ItemTemplate>
															<%--<tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>--%>
															<tr class="ClsGridRow">
																<td align="left"><%# Eval("Name") %></td>
																<td align="right" class="vwAnnual">
																	<asp:HyperLink ID="hlnkAnnualAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwTerm">
																	<asp:HyperLink ID="hlnkTerm1Amount"
																				   runat="server" />
																</td>
																<td align="right" class="vwTerm">
																	<asp:HyperLink ID="hlnkTerm2Amount"
																				   runat="server" />
																</td>
																<td align="right" class="vwQuarter">
																	<asp:HyperLink ID="hlnkQuarter1Amount"
																				   runat="server" />
																</td>
																<td align="right" class="vwQuarter">
																	<asp:HyperLink ID="hlnkQuarter2Amount"
																				   runat="server" />
																</td>
																<td align="right" class="vwQuarter">
																	<asp:HyperLink ID="hlnkQuarter3Amount"
																				   runat="server" />
																</td>
																<td align="right" class="vwQuarter">
																	<asp:HyperLink ID="hlnkQuarter4Amount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkAprAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkMayAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkJunAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkJulAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkAugAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkSepAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkOctAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkNovAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkDecAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkJanAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkFebAmount"
																				   runat="server" />
																</td>
																<td align="right" class="vwMonth">
																	<asp:HyperLink ID="hlnkMarAmount"
																				   runat="server" />
																</td>
																<td align="right">
																	<asp:Label ID="lblBudgetAmount"
																			   runat="server" />
																</td>
																<td align="right">
																	<asp:Label ID="lblVarianceAmount"
																			   runat="server" />
																</td>
															</tr>
														</ItemTemplate>
													</asp:ListView>
												</ItemTemplate>
											</asp:ListView>
											</div>
										</div>
										<div id="secfooter" class="sectionFooter" runat="server">
											<table cellpadding="3" cellspacing="1" style="border: 1px solid #ACBB8F; width: 100%;">
												<tr style="font-size: 9pt; background-color: #bcdbca !important">
													<td align="right" style="width: 220px;">Surplus/Deficit (Inflow-Outflow) (Rs.) :</td>
													<td align="right" style="width: 135px;" class="vwAnnual">
														<asp:Label ID="lblAnnualGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 135px;" class="vwTerm">
														<asp:Label ID="lblTerm1GrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 135px;" class="vwTerm">
														<asp:Label ID="lblTerm2GrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 135px;" class="vwQuarter">
														<asp:Label ID="lblQuarter1GrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 135px;" class="vwQuarter">
														<asp:Label ID="lblQuarter2GrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 135px;" class="vwQuarter">
														<asp:Label ID="lblQuarter3GrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 135px;" class="vwQuarter">
														<asp:Label ID="lblQuarter4GrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblAprGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblMayGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblJunGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblJulGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblAugGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblSepGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblOctGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblNovGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblDecGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblJanGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblFebGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;" class="vwMonth">
														<asp:Label ID="lblMarGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;">
														<asp:Label ID="lblBudgetGrossTotal"
																   runat="server" />
													</td>
													<td align="right" style="width: 100px;">
														<asp:Label ID="lblVarianceGrossTotal"
																   runat="server" />
													</td>
												</tr>
											</table>
										</div>
									</div>
								</ItemTemplate>
								<EmptyDataTemplate>
									<div class="LblNoRecord" style="margin: 10px 0; width: 800px; text-align: center;">No record found.</div>
								</EmptyDataTemplate>
							</asp:ListView>
						</div>
					</div>
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center">
					<asp:Button ID="btnBack"
								runat="server"
								Text="Back"
								CssClass="ClsBtn"
								CausesValidation="false"
								PostBackUrl="~/RITeSchool/Common/ControlPanel.aspx" />
					<asp:Button ID="btnClose"
								runat="server"
								Text="Close"
								CssClass="ClsBtn"
								Visible="false"
								CausesValidation="false"
								UseSubmitBehavior="false"
								OnClientClick="window.close();"
								style="margin-left: 5px;" />
				</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
var _clientrdlstReportView = '<%= rdlstReportView.ClientID %>';


// Actions to be performed on page load.
$(document).ready(function() {
	
	// Event listener for Report View change.
	$('#' + _clientrdlstReportView).click(function() {
		$('#reportContainer').attr('class', GetModeString());
		
		var mode = GetMode();
		
		$('.expand .node').attr('colspan', mode + 3);
		$('.collapse .node').attr('colspan', '1');
		
		if (mode == 12)
			$('#reportWrapper').addClass('overflow');
		else
			$('#reportWrapper').removeClass('overflow');
		
		ReApplyStyleForIE();
	});

	// Event listener for Group Expand/Collapse.
	$('.parent .node').click(function() {
		var context = this.parentNode;
		if (context.className.lastIndexOf('expand') > 0) {
			context.className = context.className.replace('expand', 'collapse');
			$('.node', context).attr('colspan', '1');
		}
		else {
			context.className = context.className.replace('collapse', 'expand');
			$('.node', context).attr('colspan', GetMode() + 3);
		}
		$(this).parents('tr.parent').nextUntil('tr.parent').not('.sectionTotal').toggle();
	});
	
	// Event listener for Section Expand/Collapse.
	$('.sectionHeader').click(function() {
		var context = this.parentNode;
		if ($('.expand', context).length > 0)
			$('.expand .node', context).click();
		else
			$('.collapse .node', context).click();
	});
	
	// Event listener for Expand Collapse all link.
	$('#btnExpandCollapse').click(function() {
		if ($('.expand').length > 0)
			$('.expand .node').click();
		else
			$('.collapse .node').click();
	});
});

function GetMode() {
	if ($('#' + _clientrdlstReportView + '_0')[0].checked)
		return 1;
	else if ($('#' + _clientrdlstReportView + '_1')[0].checked)
		return 2;
	else if ($('#' + _clientrdlstReportView + '_2')[0].checked)
		return 4;
	else
		return 12;
}

function GetModeString() {
	if ($('#' + _clientrdlstReportView + '_0')[0].checked)
		return 'Annual';
	else if ($('#' + _clientrdlstReportView + '_1')[0].checked)
		return 'Term';
	else if ($('#' + _clientrdlstReportView + '_2')[0].checked)
		return 'Quarter';
	else
		return 'Month';
}

function ReApplyStyleForIE() {
	var style = $('#styleIE').get(0);
	if (style) {
		var text = style.innerHTML;
		style.parentNode.removeChild(style);
		var newStyle = document.createElement('style');
		newStyle.setAttribute("type", "text/css");
		newStyle.id = 'styleIE';
		if (newStyle.styleSheet)
			newStyle.styleSheet.cssText = text;
		else
			newStyle.appendChild(document.createTextNode(text));
		var head = $('head').get(0);
		head.appendChild(newStyle);
	}
}
</script>
</asp:Content>