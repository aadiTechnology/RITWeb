<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="DashboardUI.aspx.cs" Inherits="Management.DashboardUI" ClientIDMode="Static" ValidateRequest="false" %>
<%--<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master" AutoEventWireup="true" CodeFile="DashboardUI.aspx.cs" Inherits="Management.DashboardUI" ClientIDMode="Static" ValidateRequest="false" %>--%>

<asp:Content ID="headContent" ContentPlaceHolderID="headContentPlaceholder" runat="server">
	<link href="../Styles/kendo.common.min.css" rel="stylesheet" type="text/css" />
	<link href="../Styles/kendo.blueopal.min.css" rel="stylesheet" type="text/css" />
	<script src="../Scripts/kendo.jquery.min.js" type="text/javascript"></script>
	<%--<script src="../Scripts/jquery-1.8.2-vsdoc.js" type="text/javascript"></script>--%>
	<script src="../Scripts/kendo.web.min.js" type="text/javascript"></script>
	<style type="text/css">
		#content {
			font-family: Verdana;
			font-size: 9pt;
			padding: 15px;
			text-align: left;
			width: 1000px;
		}
		#ddlElements {
			text-align: center;
		}
		.section {
			margin-top: 10px;
			width: auto;
		}
		.section .k-header {
			border: 1px solid #94C0D2;
			border-radius: 6px 6px 0 0;
		}
		.sectionTitle {
			margin: 0;
			padding: 4px 10px !important;
		}
		.sectionContent {
			background-color: #EAF4F9;
			border: 1px solid #94C0D2;
			margin-top: -1px;
			padding: 10px;
		}
		#branches .k-button { font-weight: bold; }
		#data { margin: 10px 0; }
		#widgetFrame, #gridFrame {
			float: left;
		}
		#widgetFrame {
			width: 260px;
		}
		#widgetFrame #dummyWidget {
			height: 40px;
		}
		#widgetFrame .widget {
			background: url("../styles/textures/highlight.png") repeat-x scroll 0 center #DAECF4;
			border: 1px solid #94C0D2;
			border-right: none;
			border-radius: 5px 0 0 5px;
			box-shadow: 0 0 6px rgba(0,0,0,.17);
			color: #171E28;
			/*height: 100px;*/
			margin: 11px 0;
		}
		.widget table { width: 100%; }
		.widget .iconWrapper { text-align: center; }
		.widget .widgetContent {
			text-align: left;
			padding: 5px 10px;
		}
		.widget .widgetTitle {
			font-weight: bold;
			margin-bottom: 3px;
			text-align: left;
		}
		.widgetTitle .close-icon {
			background-position: -32px -16px;
			margin-left: -2px;
			visibility: hidden;
		}
		.widgetTitle:hover .close-icon {
			/*visibility: visible;*/
		}
		#gridFrame {
			width: 740px;
		}
		#gridContent {
			background-color: #EAF4F9;
			border: 1px solid #94C0D2;
			border-radius: 8px;
			box-shadow: 0 0 6px rgba(0, 0, 0, 0.17);
			padding: 5px;
		}
		#schoolGrid {
			border-color: #DAECF4;
			font-size: 10pt;
		}
		#schoolGrid .k-grid-header {
			font-size: 14px;
			margin-bottom: 8px;
		}
		.k-drag-clue {
			font-family: Lucida Sans, Verdana;
			font-size: 14px;
		}
		#schoolGrid .k-header {
			font-weight: bold;
		}
		#schoolGrid tr[data-uid] {
			height: 68px;
		}
		#schoolGrid th, #schoolGrid td {
			text-align: center;
		}
		#schoolGrid th, #schoolGrid td,
		#schoolGrid .k-grid-header, #schoolGrid .k-grid-header-wrap {
			border-color: transparent;
		}
		#schoolGrid th, #schoolGrid td,
		#schoolGrid .k-grid-header, #schoolGrid .k-header {
			background-color: transparent;
		}
		.up-arrow { background-position: 0 -129px; }
		.down-arrow { background-position: 0 -159px; }

		.feeBlock {
			margin: 0 auto;
			width: 150px;
		}
		.feeBlock div {
			text-align: right;
		}
		.widget .k-icon.up-arrow, .widget .k-icon.down-arrow { visibility: hidden; }

		/* KendoUI Custom CSS Adjustments */
		.k-button-icontext .k-icon { margin: 0 -3px 0 3px; }
		.k-grid .k-grid-header { }
		.k-grid-content { overflow: auto; }
		.widget .k-icon { cursor: pointer; }
	</style>
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="MainBody" Runat="Server">
	<asp:UpdatePanel ID="mainUpdatePanel"
					 runat="server"
					 UpdateMode="Conditional"
					 ChildrenAsTriggers="true">
		<ContentTemplate>
			<div id="content">
				<div id="ddlElements">
					<span style="font-weight: bold">Academic Year </span>
					<asp:DropDownList ID="ddlAcademicYear"
									  runat="server"
									  OnSelectedIndexChanged="ddlAcademicYear_SelectedIndexChanged" >
					</asp:DropDownList>
					<asp:Label ID="lblFinancialYear"
							   runat="server"
							   Text="Financial Year "
							   style="margin-left: 20px; font-weight: bold;" />
					<asp:DropDownList ID="ddlFinancialYear"
									  runat="server"
									  OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" >
					</asp:DropDownList>
				</div>
		
				<div id="branches" class="section" style="height: auto;">
					<div class="k-header" style="height: auto;">
						<h3 class="sectionTitle">Branches</h3>
					</div>
					<div class="sectionContent">
					</div>
				</div>
	
				<div id="data">
					<div id="widgetFrame">
						<div id="dummyWidget"></div>
						<div id="studentWidget" runat="server" class="widget">
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle; text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												Student Details
												<span class="k-icon close-icon"></span>
											</div>
											Attendance 
											<asp:TextBox id="studentAttendanceDate"
																	runat="server"
																	value="" ontextchanged="studentAttendanceDate_TextChanged" />
										</div>
									</td>
								</tr>
								<tr>
									<td align="center">
										<span class="k-icon down-arrow"></span>
									</td>
								</tr>
							</table>
						</div>
						<div id="staffWidget" runat="server" class="widget">
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle; text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												Staff Count
												<span class="k-icon close-icon"></span>
											</div>
											<%--Attendance <input id="staffAttendanceDate" value="" />--%>
										</div>
									</td>
								</tr>
								<tr>
									<td align="center">
										<span class="k-icon down-arrow"></span>
									</td>
								</tr>
							</table>
						</div>
						<div id="feeWidget" runat="server" class="widget">
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle; text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												Fee Collection
												<span class="k-icon close-icon"></span>
											</div>
											<div>
											<asp:CheckBox ID="chkIncludeInternalFees"
														  runat="server"
														  Text="Include Internal Fees"
														  Checked="true"
														  AutoPostBack="true"
														  OnCheckedChanged="chkFeeCollection_CheckedChanged" />
											</div>
											<div>
											<asp:CheckBox ID="chkIncludeCautionMoney"
														  runat="server"
														  Text="Include Caution Money"
														  Checked="true"
														  AutoPostBack="true"
														  OnCheckedChanged="chkFeeCollection_CheckedChanged" />
											</div>
										</div>
									</td>
								</tr>
								<tr>
									<td align="center">
										<span class="k-icon down-arrow"></span>
									</td>
								</tr>
							</table>
						</div>
						<div id="misreportWidget" runat="server" class="widget">
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle; text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												MIS Report
												<span class="k-icon close-icon"></span>
											</div>
										</div>
									</td>
								</tr>
								<tr>
									<td align="center">
										<span class="k-icon down-arrow"></span>
									</td>
								</tr>
							</table>
						</div>
						<%--<div id="adminloginWidget" runat="server" class="widget">
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle; text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												School Dashboard
												<span class="k-icon close-icon"></span>
											</div>
										</div>
									</td>
								</tr>
								<tr>
									<td align="center">
										<span class="k-icon down-arrow"></span>
									</td>
								</tr>
							</table>
						</div>--%>
					</div>
					<div id="gridFrame">
						<div id="gridContent">
							<div id="schoolGrid">
								<table>
									<tr id="trHeaderRow" runat="server">
										<td></td>
									</tr>
								</table>
							</div>
						</div>
					</div>
					<div style="clear: both;"></div>

				</div>
				<asp:HiddenField ID="hidJSON" runat="server" />
			</div>
				
		</ContentTemplate>
	</asp:UpdatePanel>
	
	<asp:UpdatePanel ID="MISReportUpdatePanel"
					 runat="server"
					 ChildrenAsTriggers="true">
		<ContentTemplate>
			<asp:HiddenField ID="hidMISReportCurrentSchoolId" runat="server" />
			
			<%-- This button is kept on the page so that we can handle post backs for viewing MIS Report --%>
			<asp:Button ID="btnViewMISReport"
						runat="server"
						Text=""
						CausesValidation="false"
						UseSubmitBehavior="false"
						OnClick="btnViewMISReport_Click"
						style="display: none;" />
		</ContentTemplate>
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="btnViewMISReport" EventName="Click" />
		</Triggers>
	</asp:UpdatePanel>

	<script type="text/javascript">
		// JSON vars
		var json = {};

		// Other vars
		var schoolState = {}, gridColumns = [], isPostBack = false, isOnlyReportView = false;
		
		// Document.Ready
		$(function () {
			Init();
			BindRequestHandlers();
		});
		
		function Init() {
			ParseJSON();
			InitSchools();
			BindComboboxes();
			BindDatePickers();
			BindGrid();

			if (isPostBack)
				ReconfigureGrid();
		}
		
		function ParseJSON() {
			json = eval($('#hidJSON').val())[0];
		}

		function InitSchools() {
			var el = $('#branches .sectionContent');
			for (var sc in json.schools) {
				el.append('\n<a class="k-button k-button-icontext k-state-selected" data-field="' + json.schools[sc].SchoolShortName + '">' + json.schools[sc].SchoolName + '</a>');
				if (!isPostBack)
					schoolState[json.schools[sc].SchoolShortName] = { visible: true };
			}
			
			if (isPostBack)
				ModifySchoolsButtonState();
			
			$('#branches a.k-button').click(SchoolBtnClickHandler);
		}
		
		function ModifySchoolsButtonState() {
			$('#branches a.k-button.k-state-selected')
				.each(function() {
					if (!schoolState[this.getAttribute('data-field')].visible)
						$(this).removeClass('k-state-selected');
				});
		}
		
		function SchoolBtnClickHandler() {
			var btn = this;
			var selected = this.className.indexOf('k-state-selected') > -1;
			var grid = $("#schoolGrid").data('kendoGrid');
			schoolState[btn.getAttribute('data-field')].visible = !selected;
			// button is currently selected, so we need to perform a remove action.
			if (selected) {
				if ($('#branches .sectionContent .k-state-selected').length <= 1) {
					alert('Atleast one school should be selected.');
					return;
				}

				grid.hideColumn(btn.getAttribute('data-field'));
				$(btn).removeClass('k-state-selected');
			}
			else {
				grid.showColumn(btn.getAttribute('data-field'));
				$(btn).addClass('k-state-selected');
			}
		}
		
		function BindComboboxes() {
			$('#ddlAcademicYear').kendoDropDownList();
			
			if ($('#ddlFinancialYear').length > 0)
				$('#ddlFinancialYear').kendoDropDownList();
			
			MapComboboxHandlers();
		}
		
		function MapComboboxHandlers() {
			var year = $('#ddlAcademicYear').data("kendoDropDownList");
			year.bind("change", function (e) { __doPostBack('ddlAcademicYear', ''); });

			if ($('#ddlFinancialYear').length <= 0)
				return;
			
			year = $('#ddlFinancialYear').data("kendoDropDownList");
			year.bind("change", function(e) { __doPostBack('ddlFinancialYear', ''); });
		}

		function BindDatePickers() {
			var opt = { format: "dd-MMM-yyyy" };
			
			$('#studentAttendanceDate').kendoDatePicker(opt);
			$('#studentAttendanceDate').attr("readonly","readonly");
			$('#staffAttendanceDate').kendoDatePicker(opt);
			
			MapDatePickerHandlers();
		}
		
		function MapDatePickerHandlers() {
			var attDate = $('#studentAttendanceDate').data("kendoDatePicker");
			attDate.bind("change", function (e) { __doPostBack('studentAttendanceDate', ''); });
		}

		function BindGrid() {
			var columns = [];

			for (var i = 0; i < json.schools.length; i++) {
				var school = json.schools[i];
				columns[i] = {
					field: school.SchoolShortName,
					title: school.SchoolName,
					encoded: false
				};
			}
			
			$("#schoolGrid")
				.kendoGrid({
					dataSource: {
						data: json.datasource
					},
					columns: columns,
					reorderable: true
				});

			SetGridRowHeight();
			MapMISReportHandlers();
		}

		function SetGridRowHeight() {
			var rows = $('#schoolGrid .k-grid-content tr');
			$('.widget')
				.each(function(index) {
					if (index >= rows.length)
						return;
					
					var height = this.scrollHeight;
					rows[index].style.height = (height + 13) + 'px';
				});
		}
		
		function MapMISReportHandlers() {
			$('#schoolGrid .k-button')
				.click(function () {
					// We do not execute the click handler for disabled buttons.
					if (this.className.indexOf('k-state-disabled') > -1)
						return;

					isOnlyReportView = true;
					var id = $(this).attr('data-key');
					$('#hidMISReportCurrentSchoolId').val(id);
					$('#btnViewMISReport').click();
				});
		
			$('#schoolGrid .k-button.k-state-disabled').unbind('click');
		}
		
		function ReconfigureGrid() {
			var grid = $("#schoolGrid").data('kendoGrid');

			// Hide columns as per preview view.
			for (var sc in schoolState)
				if (!schoolState[sc].visible)
					grid.hideColumn(sc);
			
			// Reorder columns as per old order.
			for (var i = 0; i < grid.columns.length; i++) {
				var x = GetIndexOfColumn(grid.columns[i].field);
				if (i != x)
					grid.reorderColumn(x, grid.columns[i]);
			}
		}
		
		function GetIndexOfColumn(dataField) {
			for (var column in gridColumns)
				if (gridColumns[column].field == dataField)
					return gridColumns.indexOf(gridColumns[column]);
			
			return -1;
		}
		
		function BindRequestHandlers() {
			var prm = Sys.WebForms.PageRequestManager.getInstance();
			prm.add_beginRequest(BeginRequestHandler);
			prm.add_endRequest(EndReqHandler);
		}

		function BeginRequestHandler(sender, args) {
			var grid = $("#schoolGrid").data('kendoGrid');
			gridColumns = grid.columns;
		}

		function EndReqHandler(sender, args) {
			isPostBack = true;
			if (!isOnlyReportView) {
				Init();
				isOnlyReportView = false;
			}
			else
				isOnlyReportView = false;
		}
	</script>
</asp:Content>