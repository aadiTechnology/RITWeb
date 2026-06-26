<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CautionMoneyReciept.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="CautionMoneyReciept" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <title>Mini Reciept</title>
    <script src="../Scripts/jquery-1.7.min.js" type="text/javascript"></script>
    <link href="../../assets/css/font-awesome.min.css" rel="stylesheet" />
    <style type="text/css">
        .style5
        {
            background-color: #fff;
            border: 1px solid #000;
            margin: 0;
            padding: 2px;
            height: 20px;
            width: 9%;
        }
        .style6
        {
            width: 23%;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
    <table style="width: 100%;display:none;" cellspacing="1" cellpadding="0" border="0">
        <tbody>
            <tr>
                <td style="background-color: white; padding-top: 10px;" id="MainDataTable" align="center"
                    valign="top">
                    <!-- Data Insert Here -->
                    <table style="width: 96%; margin-right: 0px;" border="0" cellpadding="0" cellspacing="0">
                    
                        <tr>
                            <td align="left" colspan="4" class="PTotalHead">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr id="trLogo" runat="server">
                                <td>
                                <table width="100%">

                                <tr>
                                <td style="width:130px; " valign="middle">
                                <img id="imgPhoto" alt="image"  runat="server" height="78" style="width:100%"/> 
                                </td>
                                 <td align="center" valign="top">
                                 <table>
                                 <tr>
                                 <td style="text-align:center"><span id = "spnSchoolName" runat="server" style="font-size:x-large; font-weight:bold; text-transform:uppercase;"  ></span></td>
                                 </tr>
                                 <tr>
                                 <td style="text-align:center"><span id = "spnAddress" runat="server"   ></span></td>
                                 </tr>
                                 <tr>
                                 <td style="text-align:center"><span id = "spnAddress1" runat="server"  ></span></td>
                                 </tr>
                                 <tr>
                                 <td style="text-align:center"><span id = "spnWebsite" runat="server"  ></span></td>
                                 </tr>
                                 </table>
                                 </td>
                                </tr>
                                </table>
                                </td>
                                </tr>
                                    <tr id = "trPageSetting" runat="server">
                                        <td align="center">
                                            <table cellspacing="1" cellpadding="0" width="100%" border="0" align="center">
                                                <tr>
                                                    <td style="height: 110px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="width: 50%" class="">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0" class="ClsBorderP">
                                    <tr>
                                        <td>
                                            <tr>
                                                <td align="left" colspan="4" style="padding-left: 20px; padding-right: 15px">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <span style="font-weight: bold;">
                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, SrNo%>"></asp:Label>
                                                                    <span id="Span2" class="colonPadding">:</span></span>
                                                                <asp:Label ID="lblDataRcptNo" runat="server" CssClass="Lbl10pt" Font-Bold="true" />&nbsp;
                                                                <span style="color: #ff0000"></span>
                                                            </td>
                                                            <td align="right" colspan="2" style="width: 68%">
                                                                <span style="font-weight: bold;">
                                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, DATE1%>"></asp:Label>
                                                                </span>
                                                                <asp:Label ID="lblDataPaymentDate" runat="server" CssClass="Lbl10pt" Font-Bold="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4" style="padding-left: 20px; padding-right: 15px">
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" colspan="6" style="padding-bottom: 10px">
                                                                <asp:Label ID="lblStudentName" runat="server" CssClass="" Font-Bold="true" />
                                                            </td>
                                                        </tr>
                                                        <tr id="trPPSHFoMo" runat="server" visible="false">
                                                            <td align="left" colspan="6" style="padding-bottom: 10px">
                                                                <asp:Label ID="lblPPSHFoMo" runat="server" CssClass="" Font-Bold="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="6" style="padding-bottom: 10px">
                                                                <asp:Label ID="lblMoney" runat="server" CssClass="" Font-Bold="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="6" style="padding-bottom: 10px">
                                                                <asp:Label ID="lblNote" runat="server" Text="<%$ Resources:LocalizedResources, CautionMoneyDuringAdmissionTo%>" Font-Bold="True"
                                                                    EnableViewState="False" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="1" class="style6" style="padding-bottom: 10px">
                                                                <span style="font-weight: bold;"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, STANDARD1%>"></asp:Label> </span>
                                                            </td>
                                                            <td align="left" colspan="1" class="" style="width: 18%; padding-bottom: 10px;">
                                                                <asp:Label ID="lblStandard" runat="server" CssClass="" Font-Bold="true" />
                                                            </td>
                                                            <td align="left" colspan="1" style="width: 12%; padding-bottom: 10px" class="">
                                                                <span style="font-weight: bold;"><asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, FormNo%>"></asp:Label></span>
                                                            </td>
                                                            <td align="left" colspan="1" class="" style="width: 18%; padding-bottom: 10px">
                                                                <asp:Label ID="lblGRNumber" runat="server" CssClass="" Font-Bold="true" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="1" class="style6" style="padding-bottom: 10px">
                                                                <span style="font-weight: bold;"><asp:Label ID="Label5" runat="server" Text="CHEQUE/D.D. NO./TR. NO.:"></asp:Label></span>
                                                            </td>
                                                            <td align="left" colspan="1" class="" style="width: 18%; padding-bottom: 10px">
                                                                <asp:Label ID="lblChequeNo" runat="server" CssClass="" Font-Bold="true" />
                                                            </td>
                                                            <td align="left" colspan="1" style="width: 15%; padding-bottom: 10px" class="">
                                                                <span style="font-weight: bold;"><asp:Label ID="Label13" runat="server" Text="<%$ Resources:LocalizedResources,DATE1 %>"></asp:Label></span>
                                                            </td>
                                                            <td align="left" colspan="1" class="" style="width: 18%; padding-bottom: 10px">
                                                                <asp:Label ID="lblChequeDate" runat="server" CssClass="" Font-Bold="true" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="1" class="style6" style="padding-bottom: 10px">
                                                                <span style="font-weight: bold;"><asp:Label ID="Label16" runat="server" Text="<%$ Resources:LocalizedResources,Bank1 %>"></asp:Label></span>
                                                            </td>
                                                            <td align="left" colspan="3" class="" style="padding-bottom: 10px">
                                                                <asp:Label ID="lblBankName" runat="server" CssClass="" Font-Bold="true" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="1" class="style6" style="padding-bottom: 10px">
                                                                <span style="font-weight: bold;"><asp:Label ID="Label24" runat="server" Text="REMARK : "></asp:Label></td>
                                                            <td align="left" colspan="3" class="" style="padding-bottom: 10px">
                                                                <asp:Label ID="lblRemark" runat="server" CssClass="" Font-Bold="true" /></td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr id="trFeePaidBy" runat="server" visible="false">
                                                            <td align="left" colspan="1" class="style6" style="padding-bottom: 10px">
                                                                <span style="font-weight: bold;"><asp:Label ID="Label25" runat="server" Text="FEE PAID BY : "></asp:Label></td>
                                                            <td align="left" colspan="3" class="" style="padding-bottom: 10px">
                                                                <asp:Label ID="lblFeePaidBy" runat="server" CssClass="" Font-Bold="true" /></td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr id="trConcessionAmount" runat="server" visible="false">
                                                            <td id="tdConcessionlbl" runat="server" align="left" colspan="1" style="padding-bottom: 10px;">
                                                               <span style="font-weight: bold;"> CONCESSION AMOUNT : </span>                                                             
                                                            </td>
                                                            <td align="left" colspan="1" style="width: 15%; padding-bottom: 10px;" id="tdConcessionAmount" runat="server"> 
                                                                <asp:Label ID="lblConcession" runat="server" CssClass="LblNormal" Font-Bold="true" BackColor="White" />                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                           <td colspan="1">
                                                                <table>
                                                                    <tr style="font-size: large;">
                                                                        <td align="left" style="border-width: thin; background-color: Black;" width="5%"
                                                                            class="ClsBorder">
                                                                            <span style="color: white;"><asp:Label ID="Label22" runat="server" Text="<%$ Resources:LocalizedResources,Rs %>"></asp:Label></span>
                                                                        </td>
                                                                        <td align="left" style="border-width: thin;" class="style5">
                                                                            <asp:Label ID="lblAmount" runat="server" CssClass="" BackColor="White" />
                                                                        </td>                                                                        
                                                                    </tr>
                                                                </table>
                                                            </td>                                                                                                                  
                                                            <td align="left" colspan="1" class="" style="width: 18%; padding-bottom: 10px">
                                                            </td>
                                                            <td align="left" colspan="1" class="" style="width: 18%; padding-bottom: 10px">
                                                            </td>
                                                            <td align="left" colspan="1" class="" style="width: 18%; padding-bottom: 10px">
                                                            </td>
                                                            <td align="right" style="font-weight: bold; padding-right: 30px; padding-bottom: 10px;"
                                                                colspan="2">
                                                                <span class="LblNormal"><asp:Label ID="Label23" runat="server" Text="<%$ Resources:LocalizedResources,SignSeal %>"></asp:Label></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            &nbsp;
                                                        </tr>
                                                        <tr>
                                                            &nbsp;
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="6">
                                                                <asp:Label ID="lblSub" runat="server" CssClass="LblNrmlB" Font-Size="Smaller" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="6">
                                                                <asp:Label ID="lblRefund" runat="server" CssClass="LblNrmlB" Font-Size="Smaller" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height:10px;"></td>
                                                        </tr>
                                                         <tr id="trReceiptDisplay" runat="server" visible = "false" style="height:20px;">
                                                            <td align="left" colspan="6">
                                                                <asp:Label ID="lblTempReceipt" runat="server" CssClass="LblNrmlB" Font-Size="20px" Font-Bold="true" style="color:Red;" Text="This is temporary receipt. Please collect original receipt from account department of school." />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trRefund" runat="server" visible="false">
                <td style="background-color: white; padding-top: 10px;" id="Td1" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table style="width: 95%;" border="0" cellpadding="4" cellspacing="3" class="ClsBorderP">
                        <tr>
                            <td align="left" colspan="4" class="PTotalHead">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <table cellspacing="1" cellpadding="0" width="100%" border="0" align="center">
                                                <tr>
                                                    <td style="width: 100%; height: 19px; padding-bottom: 5px" align="center" colspan="6">
                                                        <asp:Label ID="Label6" runat="server" Font-Size="Large" Font-Bold="True" CssClass="LblNrmlB"
                                                            Text="<%$ Resources:LocalizedResources,HeadingApplicationRefund %>" EnableViewState="False" /><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="padding-bottom: 10px">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, DATE1%>" Font-Size="12px" Font-Bold="False"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources,ThePrincipal %>" Font-Size="12px" Font-Bold="False"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6">
                                                        <asp:Label ID="lblSchool" runat="server" CssClass="" Font-Size="Small" Font-Bold="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="padding-bottom: 10px">
                                                        <asp:Label ID="lblCity" runat="server" CssClass="" Font-Size="12px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="padding-bottom: 10px">
                                                        <asp:Label ID="Label9" runat="server" Font-Size="12px" Font-Bold="True" Text="<%$ Resources:LocalizedResources, TextRefundingOfCautionMaoney%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="padding-bottom: 10px">
                                                        <asp:Label ID="Label10" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, TextDearMadam%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1" style="padding-bottom: 5px" width="32%">
                                                        <asp:Label ID="Label11" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, TextWithReferencTo%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                    <td align="left" colspan="4" style="border-bottom-width: thin; border-bottom-style: double;"
                                                        valign="top" bordercolorlight="black">
                                                        &nbsp;
                                                    </td>
                                                    <td align="left" colspan="1" width="2%" style="padding-bottom: 5px">
                                                        <asp:Label ID="Label12" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, TextFrom%>"

                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2" style="border-bottom-width: thin; border-bottom-style: double;"
                                                        valign="top" bordercolorlight="black">
                                                        &nbsp;
                                                    </td>
                                                    <td align="left" colspan="4" style="padding-bottom: 5px; padding-top: 10px">
                                                        <asp:Label ID="Label14" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, TextRequestingForRefund%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="padding-bottom: 10px; padding-top: 10px">
                                                        <asp:Label ID="Label15" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, TextRequestingForRefundingCheque%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="5" style="border-bottom-width: thin; border-bottom-style: double;"
                                                        valign="top" bordercolorlight="black">
                                                        &nbsp;
                                                    </td>
                                                    <td align="right" colspan="1" style="padding-bottom: 5px" width="18%">
                                                        <asp:Label ID="Label17" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, AtTheEarliest%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="padding-bottom: 5px; padding-top: 10px">
                                                        <asp:Label ID="Label18" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, ThankingYou%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="padding-bottom: 10px">
                                                        <asp:Label ID="Label19" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, YourSincerely%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="padding-bottom: 10px">
                                                        <asp:Label ID="Label20" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, SignatureOfTheParent%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1" style="padding-bottom: 10px" width="8%">
                                                        <asp:Label ID="Label21" runat="server" Font-Size="12px" Font-Bold="False" Text="<%$ Resources:LocalizedResources, NameAddressContactNo%>"
                                                            EnableViewState="False" />
                                                    </td>
                                                    <td align="left" colspan="5" style="border-bottom-width: thin; border-bottom-style: double;"
                                                        valign="top" bordercolorlight="black">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="6" style="border-bottom-width: thin; border-bottom-style: double;
                                                        padding-top: 10px" valign="top" bordercolorlight="black">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidStudentId" runat="server" />
                    <asp:HiddenField ID="hidYearwiseStudentId" runat="server" />
                    <asp:HiddenField ID="hidCautionMode" runat="server" />
                    <asp:HiddenField ID="hidStudentRegNo" runat="server" />
                    <asp:HiddenField ID="hidPageIndex" runat="server" />
                    <asp:HiddenField ID="hidQueryString" runat="server" />
                    <asp:HiddenField ID="hidPostBackUrl" runat="server" />
                    <asp:HiddenField ID="hidIsReturnMode" runat="server" Value="0" />
                    <asp:HiddenField ID="hidStudentCautionMoneyId" runat="server" />
                    
                </td>
            </tr>
        </tbody>
    </table>
    <div style="padding-left: 16px;">
        <asp:Label ID="lblCreaterName" Font-Size="Smaller" runat="server"></asp:Label>
    </div>
    <table>
     <tr>
        <td>
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
					ToolPanelView="None"/>
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
        $(document).ready(function () {
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
