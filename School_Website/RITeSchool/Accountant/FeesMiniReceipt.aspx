<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeesMiniReceipt.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="FeesMiniReceipt" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <script src="../Scripts/jquery-1.7.min.js" type="text/javascript"></script>
    <link href="../../assets/css/font-awesome.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">

<style type="text/css" media="print">
        @page {
            size: auto; / auto is the initial value /
            margin: 0; / this affects the margin in the printer settings /
        }
    </style>

    <i style="margin-left:10px;" class="fa fa-print" onclick="HandlePrint()" id="imgPrint"></i>
    <i style="margin-left:10px;" class="fa fa-download" onclick="HandleExport()" id="imgExport"></i>
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
							ToolPanelView="None"/>
	<asp:HiddenField ID="hidReceiptNo" runat="server" />
	<asp:HiddenField ID="hidAcaYear" runat="server" />
	<asp:HiddenField ID="hidSubmissionID" runat="server" Value="0" />
	<asp:HiddenField ID="hidSerialNo" runat="server" Value="0" />
	<asp:HiddenField ID="hidQueryString" runat="server" />
	<asp:HiddenField ID="hidStudentId" runat="server" />
	<asp:HiddenField ID="hidPostBackUrl" runat="server" />
    <asp:HiddenField ID="hidHeaderId" runat="server" />
    <asp:HiddenField ID="hidIsRefundFee" runat="server" Value="0" />
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
