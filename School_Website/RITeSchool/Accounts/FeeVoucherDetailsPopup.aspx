<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" AutoEventWireup="true" CodeFile="FeeVoucherDetailsPopup.aspx.cs" Inherits="FeeVoucherDetailsPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" Runat="Server">
    <table>
	<tr>
		<td align="center" colspan="2">
			<table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; margin: 5px 0; float: none;">
				<tr>
					<td align="left" style="height: 20px">
						<span class="MainTitleHead">Fee Voucher Particular Details</span>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr id="trDetails" runat="server" visible="false">
		<td align="left">
			<table>
				<tr>
					<td align="left" class="ClsBorderlight" style="height: 24px;">
						<span class="ClsLblLgnd" style="padding: 0 4px;">Serial No : </span>
					</td>
					<td align="left" class="ClsHilightBGB">
						<asp:Label ID="lblSerialNo"
								   runat="server"
								   CssClass="ClsLabel"
								   style="padding: 0px;" />
					</td>
				</tr>
				<tr>
					<td class="ClsBorderlight" style="height: 24px;">
						<span class="ClsLblLgnd" style="padding: 0 4px;">Fee particular : </span>
					</td>
					<td class="ClsHilightBGB">
						<asp:Label ID="lblFeeParticular"
								   runat="server"
								   CssClass="ClsLabel"
								   style="padding: 0px;" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td align="center" colspan="2">
        <div id="divFeeVoucherDetails" class="GridBorder"  style="width: 970px; overflow: scroll;overflow-y:hidden" runat="server">      
			<asp:ListView ID="lstvwFeeVoucherDetails"
						  runat="server"
						  OnDataBound="lstvwFeeVoucherDetails_DataBound"
						  OnItemDataBound="lstvwFeeVoucherDetails_ItemDataBound">
				<LayoutTemplate>
					<table cellspacing="1" cellpadding="3" class="GridBorder" style="margin-top: 5px;" width="100%">
						<tr id="trHeader" runat="server" class="ClsGridHeader">
							<th align="left" style="font-size: 9pt; white-space:nowrap;padding:10px">Student Name (Reg No.)</th>
							<th align="center" style="font-size: 9pt; white-space:nowrap;padding:10px">Class</th>
                            <th align="center" style="font-size: 9pt; white-space:nowrap;padding:10px">Receipt No.</th>
                            <th align="center" style="font-size: 9pt; white-space:nowrap;padding:10px">Transaction No.</th>
							<th align="center" style="font-size: 9pt; white-space:nowrap;padding:10px">Academic Year</th>
							<th align="center" style="font-size: 9pt; white-space:nowrap;padding:10px">Payment Mode</th>
							<th align="left" style="font-size: 9pt;white-space:nowrap;padding:10px">Payment Details</th>
							<th id="trPayableFor" runat="server" align="left" style="font-size: 9pt;white-space:nowrap;padding-left:10px">
								<asp:Label ID="lblPayableForHeader"
										   runat="server"
										   Text="Payable For" />
							</th>
							<th id="trDepositedIn" runat="server" align="left" style="font-size: 9pt; width: 100px;white-space:nowrap;padding-left:10px">
								<asp:Label ID="lblDepositedInHeader"
										   runat="server"
										   Text="Deposited In" />
							</th>
							<th align="right" style="white-space:nowrap;font-size: 9pt; width: 75px;padding:10px">Amount (Rs.)</th>
						</tr>
						<tr id="itemPlaceholder" runat="server"></tr>
						<tr class="ClsBorderPager">
							<td colspan="8" align="right" class="ClsUnread">Total (Rs.) : </td>
							<td align="right">
								<asp:Label ID="lblTotal"
										   runat="server"
										   CssClass="ClsUnread" />
							</td>
						</tr>
					</table>
				</LayoutTemplate>
				<ItemTemplate>
					<tr id="trGridRow" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
						<td align="left" style="white-space:nowrap;padding:10px"><%# String.Format("{0} ({1})",Eval("StudentName"),Eval("RegNo")) %></td>
						<td align="center" style="white-space:nowrap;padding:10px"><%# Eval("Class") %></td>
                        <td align="center" style="white-space:nowrap;padding:10px"><%# Eval("ReceiptNumber")%></td>
                        <td align="center" style="white-space:nowrap;padding:10px"><%# Eval("TransactionNumber")%></td>
						<td align="center" style="white-space:nowrap;padding:10px"><%# Eval("AcademicYear") %></td>
						<td align="center" style="white-space:nowrap;padding:10px"><%# Eval("PaymentMode") %></td>
						<td align="left" style="white-space:nowrap;padding:10px"><%# Eval("PaymentDetails") %></td>
						<td align="left" Visible='<%# !IsFeeHead %>' style="white-space:nowrap;width:200px">
							<asp:Label ID="lblPayableFor"
									   runat="server"
									   Text='<%# Eval("PayableFor") %>' />
						</td>
						<td align="left" Visible='<%# IsFeeHead %>' style="white-space:nowrap;padding-left:10px">
							<asp:Label ID="lblDepositedIn"
									   runat="server"
									   Text='<%# Eval("DepositLedger.Name") %>' />
						</td>
						<td align="right"><%# Utility.CommonUtility.FormatCurrency(Eval("Amount")) %></td>
					</tr>
				</ItemTemplate>
				<EmptyDataTemplate>
					<div class="LblNoRecord" style="margin: 10px 0; width: 650px;">No record found.</div>
				</EmptyDataTemplate>
			</asp:ListView>
        </div>
		</td>
	</tr>
    <tr>
    <td>
    <div id="divContainer" class="GridBorder"  style="width: 970px; overflow: scroll;overflow-y:hidden" runat="server" visible="false">      
    <asp:GridView ID="grdPayments" UseAccessibleHeader="true" runat="server" Width="100%"
        CssClass="GridBorder" AutoGenerateColumns="true" 
        AllowPaging="false" CellPadding="0" CellSpacing="1" ForeColor="#333333" 
        GridLines="None" onrowdatabound="grdPayments_RowDataBound" ondatabound="grdPayments_DataBound"
        >
        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
        <Columns>
        </Columns>
        <RowStyle CssClass="ClsGridRow" />
        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
        </PagerStyle>
        <HeaderStyle CssClass="ClsGridHeader" HorizontalAlign="Center" />
        <AlternatingRowStyle CssClass="ClsGridAltRow" />
    </asp:GridView>   
    </div>
    </td>
    </tr>
	<tr>
		<td align="center" colspan="2">
			<asp:Button ID="btnClose"
						runat="server"
						CssClass="ClsBtn"
						Text="Close"
						style="margin-top: 5px;"
						UseSubmitBehavior="false"
						CausesValidation="false"
						OnClientClick="window.close();" />
		</td>
	</tr>
    </table>
 
</asp:Content>