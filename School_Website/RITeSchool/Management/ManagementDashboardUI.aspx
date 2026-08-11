<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
	AutoEventWireup="true" CodeFile="ManagementDashboardUI.aspx.cs" Inherits="Management.ManagementDashboardUI"
	ClientIDMode="Static" ValidateRequest="false" %>

<%--<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master" AutoEventWireup="true" CodeFile="DashboardUI.aspx.cs" Inherits="Management.DashboardUI" ClientIDMode="Static" ValidateRequest="false" %>--%>
<asp:Content ID="headContent" ContentPlaceHolderID="headContentPlaceholder" runat="server">
    <link href="../Styles/kendo.common.min.css" rel="stylesheet" type="text/css" />
	<link href="../Styles/kendo.blueopal.min.css" rel="stylesheet" type="text/css" />
	<script src="../Scripts/kendo.jquery.min.js" type="text/javascript"></script>
	<%--<script src="../Scripts/jquery-1.8.2-vsdoc.js" type="text/javascript"></script>--%>
	<script src="../Scripts/kendo.web.min.js" type="text/javascript"></script>
	<style type="text/css">
		#content
		{
			box-sizing: border-box;
			font-family: Verdana;
			font-size: 9pt;
			padding: 15px;
			text-align: left;
			width: 100%;
			max-width: 1000px;
		}
		#data,
		#widgetFrame,
		#gridFrame,
		#gridContent,
		#branches .sectionContent,
		#branches .k-button,
		#widgetFrame .widget,
		#dummyWidget
		{
			box-sizing: border-box;
		}
		#ddlElements
		{
			overflow: hidden;
			text-align: center;
		}
		.section
		{
			margin-top: 10px;
			width: auto;
		}
		.section .k-header{
			border: 1px solid #94C0D2;
			border-radius: 6px 6px 0 0;
		}
		.sectionTitle
		{
			margin: 0;
			padding: 4px 10px !important;
		}
		.sectionContent
		{
			background-color: #EAF4F9;
			border: 1px solid #94C0D2;
			margin-top: -1px;
			padding: 10px;
		}
		#branches .sectionContent
		{
			display: flex;
			flex-wrap: nowrap;
			align-items: stretch;
			gap: 6px;
			overflow-x: auto;
		}
		#branches .k-button
		{
			flex: 0 0 auto;
			width: auto;
			font-weight: bold;
			text-align: center;
			white-space: nowrap;
			padding-left: 10px;
			padding-right: 10px;
		}
		#data
		{
			display: flex;
			align-items: flex-start;
			margin: 10px 0;
			width: 100%;
			position: relative;
		}
		#widgetFrame, #gridFrame
		{
			float: none;
		}
		#widgetFrame
		{
			flex: 0 0 250px;
			width: 250px;
			min-width: 250px;
			position: relative;
			z-index: 2;
		}
		#widgetFrame #dummyWidget
		{
			display: none;
			height: 0;
			margin: 0;
			padding: 0;
			border: 0;
		}
		#widgetFrame .widget
		{
			background: url("../styles/textures/highlight.png") repeat-x scroll 0 center #DAECF4;
			border: 1px solid #94C0D2;
			border-right: none;
			border-radius: 0;
			box-shadow: none;
			box-sizing: border-box;
			color: #171E28;
			margin: 0;
			overflow: hidden;
			width: 250px;
		}
		#widgetFrame .widget + .widget
		{
			border-top-width: 0;
		}
		.widget table
		{
			width: 100%;
			height: 100%;
			border-collapse: collapse;
		}
		.widget table td
		{
			vertical-align: middle;
			padding: 0;
		}
		/* Hide unused arrow rows so they do not affect height */
		.widget table tr:first-child,
		.widget table tr:last-child
		{
			display: none;
		}
		.widget .iconWrapper
		{
			text-align: center;
		}
		.widget .widgetContent
		{
			text-align: left;
			padding: 10px 12px;
			box-sizing: border-box;
		}
		.widget .widgetTitle
		{
			font-weight: bold;
			margin-bottom: 4px;
			text-align: left;
			line-height: 1.3;
		}
		.widgetTitle .close-icon
		{
			background-position: -32px -16px;
			margin-left: -2px;
			visibility: hidden;
		}
		.widgetTitle:hover .close-icon
		{
			/*visibility: visible;*/
		}
		#studentWidget .k-datepicker,
		#studentWidget .k-picker-wrap
		{
			width: 145px;
		}
		#studentWidget .k-input
		{
			width: 110px;
			font-size: 9pt;
			height: 22px;
			line-height: 22px;
		}
		#feeWidget .widgetContent div
		{
			line-height: 1.25;
			white-space: nowrap;
		}
		#gridFrame
		{
			flex: 1 1 auto;
			min-width: 0;
			width: auto;
			position: relative;
			z-index: 1;
		}
		#gridContent
		{
			background-color: #EAF4F9;
			border: 1px solid #94C0D2;
			border-radius: 0 8px 8px 0;
			box-shadow: none;
			padding: 5px;
			width: 100%;
			box-sizing: border-box;
		}
		#schoolGrid
		{
			border-color: #DAECF4;
			font-size: 10pt;
		}
		#schoolGrid .k-grid-header
		{
			font-size: 14px;
			margin-bottom: 0;
			padding: 0;
		}
		.k-drag-clue
		{
			font-family: Lucida Sans, Verdana;
			font-size: 14px;
		}
		#schoolGrid .k-header
		{
			font-weight: bold;
		}
		#schoolGrid .k-grid-content
		{
			overflow: hidden !important;
		}
		#schoolGrid .k-grid-content table
		{
			border-collapse: collapse;
		}
		#schoolGrid .k-grid-content tr[data-uid],
		#schoolGrid .k-grid-content tr
		{
			height: auto;
		}
		#schoolGrid .k-grid-content td
		{
			vertical-align: middle;
			padding-top: 10px;
			padding-bottom: 10px;
		}
		#schoolGrid th, #schoolGrid td
		{
			text-align: center;
		}
		#schoolGrid th, #schoolGrid td, #schoolGrid .k-grid-header, #schoolGrid .k-grid-header-wrap
		{
			border-color: transparent;
		}
		#schoolGrid th, #schoolGrid td, #schoolGrid .k-grid-header, #schoolGrid .k-header
		{
			/*background-color: transparent;*/
		}
		.up-arrow
		{
			background-position: 0 -129px;
		}
		.down-arrow
		{
			background-position: 0 -159px;
		}
		
		.feeBlock
		{
			margin: 0 auto;
			width: 150px;
			line-height: 1.25;
		}
		.feeBlock div
		{
			text-align: center;
			padding: 0;
			margin: 0;
		}
		.widget .k-icon.up-arrow, .widget .k-icon.down-arrow
		{
			display: none;
		}
		
		/* KendoUI Custom CSS Adjustments */
		.k-button-icontext .k-icon
		{
			margin: 0 -3px 0 3px;
		}
		.k-grid .k-grid-header
		{
		}
		.k-grid-content
		{
			overflow: hidden;
		}
		.widget .k-icon
		{
			cursor: pointer;
		}
		#schoolGrid .k-button
		{
			line-height: 1.3;
			padding: 4px 10px;
			margin: 2px 0;
		}
	</style>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="mainUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
		<ContentTemplate>
			<div id="content">
				<div id="ddlElements">
					<span style="font-weight: bold">Academic Year </span>
					<asp:DropDownList ID="ddlAcademicYear" runat="server" OnSelectedIndexChanged="ddlAcademicYear_SelectedIndexChanged">
					</asp:DropDownList>
					<asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year " Style="margin-left: 20px;
						font-weight: bold;" />
					<asp:DropDownList ID="ddlFinancialYear" runat="server" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged">
					</asp:DropDownList>
					<div style="float: right; text-align: center;" class="k-header k-input k-widget k-dropdown k-header k-dropdown-wrap k-state-default">
						<asp:HyperLink ID="lnkChangePwd" 
							runat="server" Style="text-align: center;  " ForeColor="#0000CC">Change Password</asp:HyperLink>
					</div>
				</div>
				<div id="branches" class="section">
					<div class="k-header">
						<h3 class="sectionTitle">
							Branches</h3>
					</div>
					<div class="sectionContent">
					</div>
				</div>
				<div id="data">
					<div id="widgetFrame">
						<div id="dummyWidget">
						</div>
						<div id="studentWidget" runat="server" class="widget">
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle;
								text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												Student Details <span class="k-icon close-icon"></span>
											</div>
											Attendance
											<asp:TextBox ID="studentAttendanceDate" runat="server" value="" OnTextChanged="studentAttendanceDate_TextChanged" />
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
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle;
								text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												Staff Count <span class="k-icon close-icon"></span>
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
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle;
								text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												Fee Collection <span class="k-icon close-icon"></span>
											</div>
											<div>
												<asp:CheckBox ID="chkIncludeInternalFees" runat="server" Text="Include Internal Fees"
													Checked="true" AutoPostBack="true" OnCheckedChanged="chkFeeCollection_CheckedChanged" />
											</div>
											<div>
												<asp:CheckBox ID="chkIncludeCautionMoney" runat="server" Text="Include Caution Money"
													Checked="true" AutoPostBack="true" OnCheckedChanged="chkFeeCollection_CheckedChanged" />
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
							<table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle;
								text-align: center;">
								<tr>
									<td align="center">
										<span class="k-icon up-arrow"></span>
									</td>
								</tr>
								<tr>
									<td align="center">
										<div class="widgetContent">
											<div class="widgetTitle">
												MIS Report <span class="k-icon close-icon"></span>
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
						<div id="adminloginWidget" runat="server" class="widget">
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
												LogIn
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
					</div>
					<div id="gridFrame">
						<div id="gridContent">
							<div id="schoolGrid">
								<table style="display: none;">
									<tr id="trHeaderRow" runat="server">
										<td>
										</td>
									</tr>
								</table>
							</div>
						</div>
					</div>
				</div>
				<asp:HiddenField ID="hidJSON" runat="server" />
                <asp:HiddenField ID="HidUserId" runat="server" />
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:UpdatePanel ID="MISReportUpdatePanel" runat="server" ChildrenAsTriggers="true">
		<ContentTemplate>
			<asp:HiddenField ID="hidMISReportCurrentSchoolId" runat="server" />
			<%-- This button is kept on the page so that we can handle post backs for viewing MIS Report --%>
			<asp:Button ID="btnViewMISReport" runat="server" Text="" CausesValidation="false"
				UseSubmitBehavior="false" OnClick="btnViewMISReport_Click" Style="display: none;" />            
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
				.each(function () {
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
			year.bind("change", function (e) { __doPostBack('ddlFinancialYear', ''); });
		}

		function BindDatePickers() {
			var opt = { format: "dd-MMM-yyyy" };

			$('#studentAttendanceDate').kendoDatePicker(opt);
			$('#studentAttendanceDate').attr("readonly", "readonly");
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
					reorderable: true,
					dataBound: function () {
						SetGridRowHeight();
					}
				});

			SetGridRowHeight();
			setTimeout(SetGridRowHeight, 50);
			setTimeout(SetGridRowHeight, 200);
			MapMISReportHandlers();
		}

		function SetGridRowHeight() {
			var ROW_PADDING = 8; // extra top+bottom space so rows are not cramped
			var $frame = $('#widgetFrame');
			var $widgets = $frame.children('.widget');
			var $content = $('#schoolGrid .k-grid-content');
			var $rows = $content.find('table tbody > tr');
			if ($rows.length === 0)
				$rows = $content.find('tbody > tr');
			if ($rows.length === 0)
				$rows = $content.find('tr');

			if ($rows.length === 0 || $widgets.length === 0)
				return;

			var count = Math.min($widgets.length, $rows.length);

			// 1) Measure natural left-side heights (in normal flow).
			$widgets.each(function () {
				$(this).css({
					position: 'relative',
					top: 'auto',
					left: 'auto',
					height: 'auto',
					width: '250px'
				});
			});

			var naturalHeights = [];
			for (var i = 0; i < count; i++)
				naturalHeights[i] = $widgets.eq(i).outerHeight();

			// 2) Make each grid row at least as tall as its left widget + padding.
			$rows.each(function () {
				this.style.height = 'auto';
				$(this).children('td').css({ height: '', minHeight: '', paddingTop: '', paddingBottom: '' });
			});
			$content.css({ height: 'auto', overflow: 'hidden' });

			var totalHeight = 0;
			for (var r = 0; r < count; r++) {
				var $row = $rows.eq(r);
				var rowHeight = Math.max($row.outerHeight(), naturalHeights[r]) + ROW_PADDING;
				$row[0].style.height = rowHeight + 'px';
				$row.children('td').css({
					height: rowHeight + 'px',
					'vertical-align': 'middle',
					'padding-top': '10px',
					'padding-bottom': '10px'
				});
				totalHeight += rowHeight;
			}
			$content.height(totalHeight);

			// 3) Pin each left widget exactly onto its matching grid row.
			var frameTop = $frame.offset().top;
			var last = $rows.eq(count - 1);
			var frameHeight = (last.offset().top - frameTop) + last.outerHeight();
			$frame.css({ height: frameHeight + 'px' });

			for (var w = 0; w < count; w++) {
				var $rowMatch = $rows.eq(w);
				var top = $rowMatch.offset().top - frameTop;
				var height = $rowMatch.outerHeight();
				var radius = '';
				if (w === 0)
					radius = '5px 0 0 0';
				else if (w === count - 1)
					radius = '0 0 0 5px';
				else
					radius = '0';

				$widgets.eq(w).css({
					position: 'absolute',
					top: top + 'px',
					left: '0',
					width: '250px',
					height: height + 'px',
					'box-sizing': 'border-box',
					'border-radius': radius,
					margin: '0'
				});
			}

			// Keep spacer unused with absolute layout.
			$('#dummyWidget').hide();
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

				    if (this.innerText == "View Report")
				        $('#btnViewMISReport').click();
				    else {

				        //$('#btnLogin').click();

				        //$('#btnLogin').click(LoginToSchool);
				        LoginToSchool(id);
				    }

				});

			$('#schoolGrid .k-button.k-state-disabled').unbind('click');
}

function LoginToSchool(id) {
    var userId = $('#HidUserId').val();
        $.ajax({
            type: "POST",
            data: '{"asId": "' + id + '","aiUserId":"'+userId+'"}',
            url: "ManagementDashboardUI.aspx/GetQueryString",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
               window.open(msg.d,'_blank')
            },
            error: function (msg) {
                alert("Failed to login.");                
            }
        });
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

			SetGridRowHeight();
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
