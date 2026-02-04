<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VoucherPrint.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="VoucherPrint" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <title>Voucher Details</title>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
    <table style="width: 100%;" cellspacing="1" cellpadding="0" border="0">
        <tbody>
            <tr>
                <td style="background-color: white; padding-top: 10px;" id="MainDataTable" align="center"
                    valign="top">
                    <!-- Data Insert Here -->
                    <table style="width: 95%; padding-bottom:0px;" border="0" cellpadding="4" cellspacing="3" class="ClsBorderP">
                        <tbody>
                            <tr>
                                <td align="left" colspan="4" class="PTotalHead">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td align="center" class="ActualSchoolName">
                                                    <table cellspacing="1" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="center" style="width: 100%;">
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <tr>
                                                                        <td align="left" width="25%">
                                                                            <span class="Lbl10pt">Regd. No.: </span>
                                                                            <asp:Label ID="lblRegNo" runat="server" CssClass="Lbl10pt" EnableViewState="false" />
                                                                        </td>
                                                                        <td align="center" width="50%">
                                                                            <asp:Label ID="lblOrgName" runat="server" CssClass="Lbl10pt" EnableViewState="false">Organization Name</asp:Label>
                                                                        </td>
                                                                        <td align="right" width="25%" style="padding-right:7px">
                                                                            <span class="Lbl10pt">Tel.:</span>
                                                                            <asp:Label ID="lblPhone" runat="server" CssClass="Lbl10pt" EnableViewState="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lblSchoolName" runat="server" Text="School Name" EnableViewState="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lblAddress" Font-Bold="False" BorderWidth="0px" runat="server" CssClass="LblNormal"
                                                                    EnableViewState="False">Address</asp:Label><asp:Label ID="lblcity" runat="server"
                                                                        CssClass="LblNormal" EnableViewState="False" Font-Bold="False">city</asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 33%; padding-left:2px;">
                                                <span class="LblIB">Serial No. : </span>
                                                <asp:Label ID="lblSerialNo" runat="server" CssClass="Lbl10pt" EnableViewState="false">Serial No.</asp:Label>
                                            </td>
                                            <td align="center" style="width: 33%">
                                                <span class="LblIB">Voucher Type : </span>
                                                <asp:Label ID="lblVoucherType" runat="server" CssClass="Lbl10pt" EnableViewState="false">Voucher Type</asp:Label>
                                            </td>
                                            <td align="right" style="width: 33%; padding-right:7px">
                                                <span class="LblIB">Date : </span>
                                                <asp:Label ID="lblDate" runat="server" CssClass="Lbl10pt" EnableViewState="false">Date</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ListView ID="lstvwVoucherDetails" runat="server" DataKeyNames="IsDebit,Amount"
                                        OnItemDataBound="lstvwVoucherDetails_ItemDataBound">
                                        <LayoutTemplate>
                                            <table id="tblVoucherDetails" runat="server" border="0" cellpadding="3" cellspacing="0"
                                                style = "width: 800px; border:1px solid black; margin-top: 10px;">
                                                <tr id="trHeader" runat="server">
                                                    <th style="font-size: 9pt; width: 50px; border:1px solid black;">
                                                        Sr. No.
                                                    </th>
                                                    <th align="left" style="font-size: 9pt; padding-left: 5px; border:1px solid black;">
                                                        Particulars
                                                    </th>
                                                    <th align="right" style="font-size: 9pt; width: 80px;border:1px solid black;">
                                                        Debit (Rs.)
                                                    </th>
                                                    <th align="right" style="font-size: 9pt; width: 80px; border:1px solid black;">
                                                        Credit (Rs.)
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceHolder" runat="server">
                                                </tr>
                                                <tr style="background-color:White;">
                                                    <td id="tdTotal" runat="server" align="right" colspan="2" style="border:1px solid black;">
                                                        <span class="ClsUnread">Total (Rs.) :</span>
                                                    </td>
                                                    <td align="right" style="border:1px solid black;">
                                                        <asp:Label ID="lblDebitTotal" runat="server" class="ClsUnread" />
                                                    </td>
                                                    <td align="right" style="border:1px solid black;">
                                                        <asp:Label ID="lblCreditTotal" runat="server" class="ClsUnread" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server">
                                                <td align="center" style="border:1px solid black;">
                                                    <asp:Label ID="lblSrNo" runat="server" Text="<%# Container.DataItemIndex + 1 %>" />
                                                </td>
                                                <td align="left" style="border:1px solid black;">
                                                    <asp:Label ID="lblLedger" runat="server" CssClass="ClsLabel" Width="250px" Text='<%# Eval("Ledger.Name") %>' />
                                                </td>
                                                <td align="right" style="border:1px solid black;">
                                                    <asp:Label ID="lblDebitAmount" runat="server" Style="width: 75px;" />
                                                </td>
                                                <td align="right" style="border:1px solid black;">
                                                    <asp:Label ID="lblCreditAmount" runat="server" Style="width: 75px;" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="800px">
                                        <tr>
                                            <td style="width: 80px" align="left" valign="top">
                                                <span class="LblIB">Narration : </span>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblNarration" runat="server" CssClass="Lbl10pt" EnableViewState="false" style="display: block;"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
							<tr>
								<td align="center">
									<table width="808px" cellpadding="5">
										<tr>
											<td align="left">
												<span class="Lbl10ptB">Receiver's Signature :</span>
											</td>
											<td>
												
											</td>
											<td align="left">
												<span class="Lbl10ptB">Authorized Signatory :</span>
											</td>
											<td>
												
											</td>
										</tr>
										<tr>
											<td align="left">
												<span class="Lbl10ptB">Checked by :</span>
											</td>
											<td>
												
											</td>
											<td align="left">
												<span class="Lbl10ptB">Verified by :</span>
											</td>
											<td>
												
											</td>
										</tr>
									</table>
								</td>
								
							</tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidVoucherId" runat="server" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
					
                </td>
            </tr>
			<tr><td><div style="width:950px" >
					<div style="padding-left: 25px;" >
					<div>
						<asp:Label ID="lblCreaterName"  Font-Size="11px" runat="server"  ></asp:Label>
						</div>
						</div>
						</div></td></tr>
        </tbody>
    </table>
	<script language="javascript" type="text/javascript">
		function PrintSheet() {
			window.print();
			return false;
		}
		PrintSheet();
	</script>
</asp:Content>