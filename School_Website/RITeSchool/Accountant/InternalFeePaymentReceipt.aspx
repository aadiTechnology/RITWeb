<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InternalFeePaymentReceipt.aspx.cs"
	MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="InternalFeePaymentReceipt" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <title>Mini reciept</title>
    <script src="../Scripts/jquery-1.7.min.js" type="text/javascript"></script>
    <link href="../../assets/css/font-awesome.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
    <style type="text/css">
    .clsVerticalLine
    {
        border-left: 1px solid black;
    }
    
    .clsBottomLine
    {
        border-bottom:1px solid black;
        font-weight:bold;
        float:inherit
    }
    
        .style1
        {
            width: 15px;
            height: 19px;
        }
        .style2
        {
            height: 19px;
        }
    
    </style>
	<table style="width: 100%;display:none;" cellspacing="1" cellpadding="0" border="0">
		<tbody>
        <tr id="trExtraSpace" runat = "server" visible = "false">
            <td style="height:90px;"></td>
        </tr>
			<tr>
				<td style="background-color: white; padding-top: 10px;" id="MainDataTable" align="center"
					valign="top">
					<!-- Data Insert Here -->
					<table style="width: 95%; padding-bottom:0px;" border="0" cellpadding="4" cellspacing="3" class="ClsBorderP">
						<tbody>
                            <tr id="trSchoolTelephone" runat="server" visible = "false">
                                <td colspan="1" style="width:120px;"></td>
                                <td align="center" colspan="2" runat="server" id="tdReceiptHeader">												
										<asp:Label ID="LblReceiptHeader" runat="server" Font-Size="22px" Font-Bold="true" EnableViewState="false"></asp:Label><span
											style="color: #ff0000"></span>
								</td>	
								<td align="right" style="width:150px" colspan="1">																					
								    <asp:Label ID="lblTelephone" runat="server" CssClass="Lbl10ptBH" Font-Size="14px" Font-Bold="true" EnableViewState="false" Text="Tel.: 02316651800"></asp:Label>&nbsp;<span
									   style="color: #ff0000"></span>											
								</td>
							</tr>                                                       
                            <tr id="trSchoolAddress" runat="server" visible = "false">								
							    <td align="center" colspan="4" runat="server" id="td1" style="border-bottom:1px solid;">												
										<asp:Label ID="lblSchoolAddress" runat="server" CssClass="Lbl10ptBH" Font-Size="14px" Font-Bold="true" EnableViewState="false" Text="R S No. 134, E Near Shivaji University, Morewadi Road, Kolhapur - 416004"></asp:Label>&nbsp;<span
											style="color: #ff0000"></span>
								</td>	
							</tr>                            
							<tr>
								<td align="left" colspan="4" class="PTotalHead">
									<table cellspacing="2" cellpadding="3" width="100%" border="0">
										<tr>
											<td align="left" runat="server" id="tdRecpNo">
												<span class="LblIB"><asp:Label ID="lblReceiptNumberText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ReceiptNumber %>"></asp:Label> : </span>
												<asp:Label ID="lblDataRcptNo" runat="server" CssClass="Lbl10pt" EnableViewState="false"></asp:Label>&nbsp;<span
													style="color: #ff0000"></span>
											</td>
											<td align="left">
												&nbsp;
											</td>
											<td align="right">
												<span class="LblIB"><asp:Label ID="Label1" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Date %>"></asp:Label> : </span>
												<asp:Label ID="lblDataPaymentDate" runat="server" CssClass="Lbl10pt" EnableViewState="false"></asp:Label>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td align="center" colspan="4">
									<table cellspacing="1" cellpadding="1" width="100%" border="0">
										<tr>
											<td align="left" colspan="1" class="" style="width: 12%">
											</td>
											<td align="left" colspan="1" class="" style="width: 12%">
											</td>
											<td align="left" colspan="1" style="width: 18%;" class="">
											</td>
											<td align="left" colspan="1" class="" style="width: 8%">
											</td>
										</tr>
										<tr>
											<td align="left" colspan="1" style="height: 20px" class="" valign="bottom">
												<asp:Label ID="lblMaster" runat="server" CssClass="Lbl10ptI" Text="<%$ Resources:LocalizedResources, Master %>" EnableViewState="false"></asp:Label>
												<span class="Lbl10ptI">/</span>
												<asp:Label ID="lblMiss" runat="server" CssClass="Lbl10ptI" Text="<%$ Resources:LocalizedResources, Miss %>" EnableViewState="false"></asp:Label>
											</td>
											<td colspan="3" style="border-bottom: black 1px dashed;" class="PBorderBtm" align="left">
												<asp:Label ID="lblDataStudentName" runat="server" CssClass="Lbl10pt" EnableViewState="false"></asp:Label>&nbsp;
											</td>
										</tr>
										<tr>
											<td align="left" colspan="4" style="height: 8px">
											</td>
										</tr>
										<tr id="trClass" runat="server">
											<td align="left" class="" colspan="1" valign="bottom" style="height: 21px">
												<span class="Lbl10ptI"><asp:Label ID="Label2" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Class %>"></asp:Label></span>
											</td>
											<td align="left" class=" PBorderBtm" colspan="1" style="border-bottom: black 1px dashed;
												height: 21px;">
												<asp:Label ID="lblDataClass" runat="server" CssClass="Lbl10pt" EnableViewState="false"></asp:Label>
											</td>
                                            <td align="right" class="" colspan="1" valign="bottom" id="tdRegNo" runat="server" style="height: 21px">
												<span class="Lbl10ptI"><asp:Label ID="Label11" runat="server" EnableViewState="False" Text="Reg No. :"></asp:Label></span>
											</td>
											<td align="left" class=" PBorderBtm" colspan="2" id="tdRegNoLbl" runat="server" style="border-bottom: black 1px dashed;
												height: 21px; width:300px;">
												<asp:Label ID="lblStudentRegNo" runat="server" CssClass="Lbl10pt" EnableViewState="false"></asp:Label>
											</td>
										</tr>
										<tr>
											<td align="left" colspan="4" style="height: 8px">
											</td>
										</tr>
										<tr>
											<td align="left" class="" colspan="1" valign="bottom" style="height: 21px; width: 20%">
												<span class="Lbl10ptI"><asp:Label ID="Label3" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SumOfRsInWords %>"></asp:Label></span>
											</td>
											<td align="left" class=" PBorderBtm" colspan="3" style="border-bottom: black 1px dashed;
												height: 21px;">
												<asp:Label ID="lblRsInWords" runat="server" CssClass="Lbl10pt" EnableViewState="false"></asp:Label>
											</td>                                            
										</tr>
										<tr>
											<td align="left" colspan="4" style="height: 8px">
											</td>
										</tr>
										<tr id="trFeeType" runat="server">
											<td align="left" class="" colspan="1" valign="bottom" style="height: 21px; width: 15%">
												<span class="Lbl10ptI"><asp:Label ID="Label4" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, FeeType %>"></asp:Label></span>
											</td>
											<td align="left" class=" PBorderBtm" colspan="3" style="border-bottom: black 1px dashed;
												height: 21px;">
												<asp:Label ID="lblFeeType" runat="server" CssClass="Lbl10pt" EnableViewState="false"></asp:Label>
											</td>
										</tr>
										<tr>
											<td align="left" colspan="4" style="height: 8px">
											</td>
										</tr>
										<tr>
											<td align="left" class="" colspan="1" valign="bottom" style="height: 21px; width: 15%">
												<span class="Lbl10ptI"><asp:Label ID="Label5" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PayableFor %>"></asp:Label></span>
											</td>
											<td align="left" class=" PBorderBtm" colspan="3" style="border-bottom: black 1px dashed;
												height: 21px;">
												<asp:Label ID="lblPayableFor" runat="server" CssClass="Lbl10pt" EnableViewState="false"></asp:Label>
											</td>
										</tr>
										<tr>
											<td align="left" colspan="4" style="height: 8px">
											</td>
										</tr>
										<tr>
                                            <td align="left" class="" colspan="1" valign="bottom">
												<span class="Lbl10ptI"><asp:Label ID="Label6" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, AmountRs %>"></asp:Label></span>
											</td>
                                            <td align="left" colspan="3">
                                                <table>
                                                    <tr>
											            <td align="left">
												            <div class="ClsBorderP" style="width: 80px; padding: 2px">
													            <asp:Label ID="lblDataAmount" runat="server" CssClass="Lbl10ptB" EnableViewState="false"></asp:Label>
													            <b>/ -</b></div>                                                    
											            </td>
                                                        <td align="left" colspan="2">
                                                            <asp:Label ID="lblPaymentType" runat="server" CssClass="ClsLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
										</tr>
										<tr>
											<td align="left" class="" colspan="4" valign="bottom" style="height: 8px">
											</td>
										</tr>
										<tr id="trRemarkDetails" runat="server">
											<td align="left" valign="bottom" style="width: 86px; height: 6px">
												<span class="Lbl10ptI"><asp:Label ID="Label7" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Remarks %>"></asp:Label></span>
											</td>
											<td align="left" colspan="3" valign="bottom" style="border-bottom: black 1px dashed">
												<asp:Label ID="lblRemarks" runat="server" CssClass="Lbl10pt" EnableViewState="False"></asp:Label>
											</td>
										</tr>
										<tr>
											<td align="left" class="" colspan="4" valign="bottom" style="height: 8px">
											</td>
										</tr>
                                        <tr id="trChequeDetails" runat="server" visible="false">
                                            <td colspan="4">
                                                <table width="100%" style="border: 1px solid #000000">
                                                    <tr>
                                                        <td colspan="7" align="center" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit;font-size:15px;font-weight:bold;">Cheque Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr style="font-weight:bold;" align="center">
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Cheque Number</span>
                                                        </td>
                                                        <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Cheque Date</span>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="40%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Bank Name</span>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Amount(Rs)</span>
                                                        </td>
                                                    </tr>                                                    
                                                    <tr align="center">
                                                        <td width="20%">
                                                            <asp:Label ID="lblCheckNo" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                        <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%">
                                                            <asp:Label ID="lblChequeDate" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="40%">
                                                            <asp:Label ID="lblBankName" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%">
                                                            <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
											<td align="left" class="" colspan="4" valign="bottom" style="height: 8px">
											</td>
										</tr>
                                         <tr id="trNetBankingDetails" runat="server" visible="false">
                                            <td colspan="4">
                                                <table width="100%" style="border: 1px solid #000000">
                                                    <tr>
                                                        <td colspan="7" align="center" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit;font-size:15px;font-weight:bold;">Net Banking Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr style="font-weight:bold;" align="center">
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Transaction Number</span>
                                                        </td>
                                                        <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Transaction Date</span>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="40%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Bank Name</span>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Amount(Rs)</span>
                                                        </td>
                                                    </tr>                                                    
                                                    <tr align="center">
                                                        <td width="20%">
                                                            <asp:Label ID="lblTransactionNo" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                        <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%">
                                                            <asp:Label ID="lblTransactionDate" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="40%">
                                                            <asp:Label ID="lblTransactionBankName" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%">
                                                            <asp:Label ID="lblTransactionAmount" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
											<td align="left" class="" colspan="4" valign="bottom" style="height: 8px">
											</td>
										</tr>
                                        <tr id="trElectroniDetails" runat="server" visible="false">
                                            <td colspan="4">
                                                <table width="100%" style="border: 1px solid #000000">
                                                    <tr>
                                                        <td colspan="7" align="center" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit;font-size:15px;font-weight:bold;">Electronic Payment Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr style="font-weight:bold;" align="center">
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Transaction Number</span>
                                                        </td>
                                                        <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Payment Type</span>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="40%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Bank Name</span>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%" class="clsBottomLine">
                                                            <span class="ClsLabel" style="float:inherit">Amount(Rs)</span>
                                                        </td>
                                                    </tr> 
                                                    <tr align="center">
                                                        <td width="20%">
                                                            <asp:Label ID="lblETransactionNo" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                        <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%">
                                                            <asp:Label ID="lblEPaymentType" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="40%">
                                                            <asp:Label ID="lblEBankName" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                         <td class="clsVerticalLine">
                                                        </td>
                                                        <td width="20%">
                                                            <asp:Label ID="lblEAmount" runat="server" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

										<tr>
											<td align="left" class="" colspan="1" valign="bottom">
											</td>
											<td align="left" colspan="2">
											</td>
											<td align="left" colspan="1">
											</td>
										</tr>
                                        <tr>
											<td colspan="4">
											</td>
										</tr>
										<tr>
											<td colspan="4">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">
												            &nbsp;
											            </td>
											            <td align="left">
											            </td>
                                                        <td align="left">
											            </td>
                                                    </tr>
                                                    <tr runat="server" id="trSignature" visible="true">	
                                                        <td align="left" class="" valign="bottom">
												
											            </td>																					
											            <td align="center">
                                                            <asp:Label ID="lblChequeNote" runat="server"  Text="* Subject to Cheque Realization"></asp:Label>
											            </td>
                                                        <td align="left" class="" valign="bottom">
												
											            </td>											
										            </tr>
                                                    <tr runat="server" id="tr1" visible="true">
											            <td align="left" class="" valign="bottom">
												            <span class="Lbl10pt"><asp:Label ID="Label8" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, AccountsOfficer %>"></asp:Label></span>
											            </td>
                                                         <td id="tdSNSChequeDetails" runat="server" visible="false" align="left">
                                                            <asp:Label ID="Label12" runat="server"  Text="* Subject to Cheque Realization"></asp:Label>
											            </td>
											            <td align="center">
                                                            <asp:Label ID="lblRefundableNote" runat="server" Text="* Non Refundable"></asp:Label>
											            </td>
											            <td align="right">
												            <span class="Lbl10pt" style="width: 90px;"><asp:Label ID="Label10" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SrClerk %>"></asp:Label></span>
											            </td>
										            </tr>
                                                </table>
											</td>											
										</tr>						
                                        
										<tr runat="server" id="trComgenNote" visible="false">
											<td align="right" class="" colspan="4" valign="bottom">
												<span style="padding-right:5px; font-size:10pt;"><asp:Label ID="Label9" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ComputerGeneratedStatementNoSignature %>"></asp:Label></span>
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
               <table style="width: 96.5%;">
                    <tr>
                        <td>               
			 	           <asp:Label ID="lblCreaterName" runat="server" CssClass="clsLabel"></asp:Label>
                         </td>
                    </tr>
                </table>
			    </td>                                   
            </tr>
			<tr>
				<td>
					<asp:HiddenField ID="hidReceiptNo" runat="server" Value="0" />
					<asp:HiddenField ID="hidAcaYear" runat="server" />
					<asp:HiddenField ID="hidInternalFeeDetailsId" runat="server" Value="0" />
					<asp:HiddenField ID="hidStudentId" runat="server" Value="0" />
					<asp:HiddenField ID="hidRegNo" runat="server" />
					<asp:HiddenField ID="hidFromDate" runat="server" />
					<asp:HiddenField ID="hidToDate" runat="server" />
					<asp:HiddenField ID="hidIncludePaid" runat="server" />
					<asp:HiddenField ID="hidPayForNextYear" runat="server" />
					<asp:HiddenField ID="hidQueryString" runat="server" />
					<asp:HiddenField ID="hidIsRegNoFilter" runat="server" />
					<asp:HiddenField ID="hidStandardID" runat="server" />
					<asp:HiddenField ID="hidDivisionID" runat="server" />
					<asp:HiddenField ID="hidFeeTypeID" runat="server" />
					<asp:HiddenField ID="hidPageIndex" runat="server" />
					<asp:HiddenField ID="hidDuplicateInternalFeeDetailsId" runat="server" />
                    <asp:HiddenField ID="hidIsNextYearFeePayment" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSerialNo" runat="server" Value="0" />
                    <asp:HiddenField ID="hidDate" runat="server" Value="" />
                    <asp:HiddenField ID="hidAccountHeaderId" runat="server" Value="0" />
				</td>
			</tr>
		</tbody>
	</table>
    <table>
     <tr>
        <td align="left">
            <%--<i style="margin-left:10px;" class="fa fa-print" onclick="HandlePrint()" id="imgPrint"></i>--%>
            <div style="float:left;background-color:White;width:100%;padding-top:5px;">
                <i style="margin-left:10px;" class="fa fa-download" onclick="HandleExport()" id="imgExport"></i>
            </div>
            <CR:CrystalReportViewer ID="reportViewer"
					runat="server"
							AutoDataBind="True"
							DisplayStatusbar="False"
							EnableDatabaseLogonPrompt="False"
							EnableDrillDown="False"
							EnableParameterPrompt="False"
							HasCrystalLogo="False"
							HasDrilldownTabs="False"
							HasDrillUpButton="False"
							HasGotoPageButton="False"
							HasPageNavigationButtons="False"
							HasSearchButton="False"
							HasToggleGroupTreeButton="False"
							HasToggleParameterPanelButton="False"
							HasZoomFactorList="False"
							ToolPanelView="None" />
        </td>
    </tr>		
    </table>
	<script language="javascript" type="text/javascript">
		function PrintSheet() {
			window.print();
			return false;
		}
		//PrintSheet();
	</script>
	<script type="text/javascript">
		_clienthidQueryString = "<%=this.hidQueryString.ClientID %>";

		//        window.onunload = SetQueryString;
		//        function SetQueryString() {
		//            var sQuery = document.getElementById('hidQueryString').value;
		//            window.opener.location = window.opener.location.pathname + sQuery;
		//        }
        
	</script>

    <script type="text/javascript">
	    $(document).ready(function(){
			// We programatically click the print button to invoke the print dialog.
	        //$('#IconImg_reportViewer_toptoolbar_print').click();
	        //$('#reportViewer_toptoolbar').hide();
	        $('#IconImg_reportViewer_toptoolbar_print').click();
	    });

	    function HandlePrint() {
	        $("#imgPrint").hide();
	        $("#imgExport").hide();
	        window.print();
	        $("#imgPrint").show();
	        $("#imgExport").show();
	    }

	    function HandleExport() {
	        $('#IconImg_reportViewer_toptoolbar_export').click();
	    }
    </script>
</asp:Content>
