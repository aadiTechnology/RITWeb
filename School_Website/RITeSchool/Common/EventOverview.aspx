<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" AutoEventWireup="true" CodeFile="EventOverview.aspx.cs" Inherits="EventOverview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 750px; float: none; padding-right: 5px; margin: 10px 0;">
	<tr>
		<td align="left" style="height: 20px">
			<span class="MainTitleHead"><asp:Label ID="lblEventsOverviewText" runat="server" Text="<%$ Resources:LocalizedResources, EventsOverview %>"></asp:Label></span>
		</td>
	</tr>
</table>
<asp:UpdatePanel ID="mainUpdatePanel"
				 runat="server">
	<ContentTemplate>
	<table cellpadding="0" cellspacing="5" border="0">
		<tr>
			<td class="ClsBorderlight">
				<span class="ClsLabel"><asp:Label ID="lblStandardText" runat="server" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label> : </span>
			</td>
			<td>
				<asp:DropDownList ID="ddlStandards"
								  runat="server"
								  CssClass="MidCombo"
								  AutoPostBack="true"
								  OnSelectedIndexChanged="ddl_SelectedIndexChanged" />
			</td>
			<td class="ClsBorderlight">
				<span class="ClsLabel"><asp:Label ID="lblMonthText" runat="server" Text="<%$ Resources:LocalizedResources, Month %>"></asp:Label> : </span>
			</td>
			<td>
				<asp:DropDownList ID="ddlMonths"
								  runat="server"
								  CssClass="MidCombo"
								  AutoPostBack="true"
								  OnSelectedIndexChanged="ddl_SelectedIndexChanged" />
			</td>
			<td id="tdAcademicYearlbl" runat="server" class="ClsBorderlight">
				<span class="ClsLabel"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, AcademicYear %>"></asp:Label> : </span>
			</td>
			<td id="tdAcademicYearsddl" runat="server">
				<asp:DropDownList ID="ddlAcademicYears"
								  runat="server"
								  CssClass="MidCombo"
								  AutoPostBack="true"
								  OnSelectedIndexChanged="ddl_SelectedIndexChanged" />
			</td>
		</tr>
	</table>
	<table>
		<tr>
			<td align="center">
				<asp:ListView ID="lstvwEvents"
							  runat="server"
							  OnDataBound="lstvwEvents_DataBound">
					<LayoutTemplate>
						<table id="tblEvents" cellpadding="5" cellspacing="1" style="width: 750px;" class="GridBorder">
							<tr id="trHeaderRow" runat="server" class="ClsGridHeader">
								<th align="center" valign="middle" style="font-size: 9pt; padding: 0 3px; width: 100px;">
									<asp:Label ID="lblMonthText" runat="server" Text="<%$ Resources:LocalizedResources, Month %>"></asp:Label>
								</th>
								<th align="left" valign="middle" style="font-size: 9pt; padding: 0 3px; width: 100px;">
									<asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Day %>"></asp:Label>
								</th>
								<th align="left" style="font-size: 9pt; padding: 0 3px; width: 300px;">
								<asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, EventTitle %>"></asp:Label>
                                
								</th>
								<th align="left" style="font-size: 9pt; padding: 0 3px; width: 250px;">
									<asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, Standards %>"></asp:Label>
								</th>
							</tr>
							<tr id="itemPlaceholder" runat="server">
							</tr>
						</table>
					</LayoutTemplate>
					<ItemTemplate>
						<tr id="trGridRow" runat="server">
							<td align="center">
								<asp:Label ID="lblMonth"
										   runat="server"
										   Text='<%# Eval("Month") %>' />
							</td>
							<td align="left">
								<asp:Label ID="lblDay"
										   runat="server"
										   Text='<%# Eval("Day") %>' />
							</td>
							<td align="left">
								<span><%# Eval("EventDescription")%></span>
							</td>
							<td align="left">
								<span><%# Eval("Standards") %></span>
							</td>
						</tr>
					</ItemTemplate>
					<EmptyDataTemplate>
						<div class="LblNoRecord" style="margin: 10px 0; width: 750px; text-align: center;"><asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, NoEventsFound %>"></asp:Label></div>
					</EmptyDataTemplate>
				</asp:ListView>
			</td>
		</tr>
	</table>
</ContentTemplate>
</asp:UpdatePanel>
<table style="width: 750px;">
	<tr>
		<td align="center">
			<asp:Button ID="btnClose"
						runat="server"
						CssClass="ClsBtn"
						Text="<%$ Resources:LocalizedResources, Close %>"
						CausesValidation="false"
						UseSubmitBehavior="false"
						OnClientClick="window.close()"
						style="margin-top: 5px;" />
		</td>
	</tr>
</table>
</asp:Content>