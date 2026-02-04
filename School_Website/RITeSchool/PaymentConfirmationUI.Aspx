<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="PaymentConfirmationUI.Aspx.cs" EnableEventValidation="false"
    Inherits="PaymentConfirmationUI" Title="Online Admission." %>

<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table>
        <tr>
            <td>
                <Wizard:AdmissionSteps ID="SubmissionWizardSteps" runat="server" ActiveSteps="3" />
            </td>
        </tr>
    </table>
    <div id="divNetBanking" runat="server">
        <div style="width: 97%" align="center">
            <div id="nifty" align="center">
                <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
                </b></b>
                <table align="center" class="paddingLR" cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:UpdatePanel ID="upnlvalsum" runat="server">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="valSumNetBankingFees" runat="server" />
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Terms and Conditions should be accepted." Display="None" ClientValidationFunction="ValidateTerms"></asp:CustomValidator>
                                    <%--<asp:CustomValidator ID="cstValidateBank" runat="server" ClientValidationFunction="ValidateBank"
                                                    Display="None" />--%>
                                                <%--<asp:CustomValidator ID="cstvalValidateBlockedBanks" runat="server" ClientValidationFunction="ValidateBlockedBanks"
                                                    Display="None" />--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" visible="false">
                            <asp:UpdatePanel runat="server" ID="upnlMsg">
                                <ContentTemplate>
                                    <asp:Label ID="lblErrorMessage" CssClass="LblErrorMsg" runat="server" EnableViewState="false"
                                        ForeColor="Red"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbBanks" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnProceed" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="HeadTxtBWOPadding borderBtm" align="left" colspan="2">
                            Pay With NetBanking / Card
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td align="left" class="TxtNormal" style="width: 135px">
                                                <asp:Image ID="Image1" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                                    Width="150px" />
                                            </td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <%--<tr id="trBankList" runat="server" visible="false">
                                            <td align="right" colspan="2">
                                                <div class="ClsGreenBG" style="float:right;background-color:#e7eed2;border-left:#000 1px solid;padding-left:5px;">
                                                    <asp:HyperLink ID="hlnkBankDetails" runat="server" Text="Online Bank / Card Details" NavigateUrl="Accountant/OnlineBankDetails.aspx"
								                        CssClass="SubTitle " Style="padding-right: 0px;white-space : nowrap;" />
                                                </div>
                                            </td>
                                        </tr>--%>
                                        <tr id="trNote" runat="server">
                                            <td align="left" colspan="2">
                                                <span class="LblNrmlB ClsBorderlight" style="background-color: #ffffc4;">
                                                    Note :</span> <span class="ClsBorderlight" style="font-family: Verdana; font-size: 8pt;
                                                        border: 100%;" id="spnNote" runat="server">Bank selection for net banking will happen at payment gateway.</span>
                                            </td>
                                        </tr>
                                        <tr style="height:10px;">
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr id="trPaymentType" runat="server" visible="false">
                                            <td align="left" class="ClsBorderlight">
                                                <span id="Span1" class="ClsLabel" runat="server">Select Type :</span>
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList ID="rdoPaymentTypes" runat="server" RepeatDirection="Horizontal" CssClass="ClsLabel"
                                                    AutoPostBack="true" DataTextField="Type" DataValueField="Id" OnSelectedIndexChanged="rdoPaymentTypes_SelectedIndexChanged">
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr id="trBankSelection" runat="server">
                                            <td align="left" class="TxtNormal" valign="top">
                                                <span id="lblbankName" runat="server">Select Bank :</span>
                                                <%--<img src="../../images/newLink.gif" runat="server" id="img2" />--%>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:DropDownList ID="cmbBanks" runat="server" CssClass="TxtBoxLogin" AutoPostBack="false"
                                                    OnSelectedIndexChanged="cmbBanks_SelectedIndexChanged" CausesValidation="True">
                                                    <asp:ListItem Text="--Select--" Value="0" />
                                                </asp:DropDownList>
                                                <span style="color: #ff0000">*</span>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="height:22px;">
                                                <asp:Label ID="lblForm" runat="server" CssClass="ClsLabel" />
                                            </td>
                                            <td align="left" style="padding-left:15px;">
                                                Rs.
                                                <asp:TextBox ID="txtFormFees" runat="server" CssClass="TxtBox" MaxLength="50" Enabled="False"
                                                    Style="border: 1px solid black; font-family: Verdana; font-size: 8pt" />
                                            </td>
                                        </tr>
                                        <tr id="trProcessingCharges" runat="server" visible="false">
                                            <td id="trProcessChrges" align="left" class="TxtNormal" valign="top" runat="server"
                                                visible="false">
                                                Processing Charges :
                                            </td>
                                            <td align="left" class="TxtNormal">
                                                <asp:Label ID="txtAmtPerTransaction" runat="server" />
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trServiceTax" visible="false">
                                            <td align="left" class="ClsBorderlight" style="height:22px;">
                                               <span class="ClsLabel">GST :</span>
                                            </td>
                                            <td align="left" style="padding-left:15px;">
                                                <asp:Label ID="txtServiceTax" runat="server" />
                                                <b>(<span style="color:Red; font-size:11px; " >applicable on Processing Charges</span>)</b></td>
                                        </tr>
                                        <tr id="trTotalAmount" runat="server">
                                            <td align="left" class="TxtNormal" valign="top">
                                                Total Amount :
                                            </td>
                                            <td align="left" class="TxtNormal">
                                                Rs.
                                                <asp:TextBox ID="txtTotalFees" runat="server" CssClass="TxtBox" MaxLength="50" Enabled="False"
                                                    Style="border: 1px solid black; font-family: Verdana; font-size: 8pt" />
                                            </td>
                                        </tr>
                                        <tr style="height:10px;">
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
							                    <asp:CheckBox ID="chkTermOfuse" runat="server" Text=" I have read and agree to Payment" onclick="EnableDisablePayButton(this);"/>
                                                <a style="color:Blue;text-decoration:underline;cursor:pointer;" onclick="OpenTermsOfusePopup();">Terms and Conditions. </a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="TxtNormal" colspan="2" valign="top">
                                                <asp:HiddenField ID="hidTerms" runat="server" Value="" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbBanks" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rdoPaymentTypes" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>                   
                    <tr>
                        <td colspan="2" align="left">
                        </td>
                    </tr>   
                      <tr>                        
                        <td class="TxtNormal" colspan="2">
                            <asp:HiddenField runat="server" ID="hidTransactionFrom" Value="Admission" />
                            <asp:HiddenField runat="server" ID="hidBlockedBanksJSON" Value="" />
                            <asp:HiddenField runat="server" ID="hidSchoolwiseStudentFeeIds" Value="" />
                            <asp:HiddenField runat="server" ID="hidSelectedFeeTypes" Value="" />
                            <asp:HiddenField runat="server" ID="hidScStudentId" Value="" />
                            <asp:HiddenField runat="server" ID="hidNxtAcYearId" Value="0" />                            
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="TxtNormal" valign="top" colspan="2">
                            <asp:Button ID="btnProceed" runat="server" Text="Proceed To Pay" CssClass="ClsButton" 
                                Width="149px" CausesValidation="true" OnClick="btnProceed_Click" />
                        </td>
                    </tr>                
                </table>
                <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
                </b></b>
            </div>
            <br />
        </div>
    </div>    
    <div id="divConformation" runat="server" visible="false">
        <div style="width: 97%" align="center">
            <div id="Div1" align="center">
                <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
                </b></b>
                <table>
                    <tr>
                        <td align="left" visible="false">
                            <asp:Label ID="lblConfirmMsg" runat="server" EnableViewState="false" ForeColor="Red"
                                Visible="False" CssClass="LblErrorMsg"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                <%--<tr>
                    <td colspan="2">
                    </td>
                </tr>
                <tr id="trDebitCardNote" runat="server" visible="false">
                    <td colspan="2">
                         <table style="text-align:left;" align="left">
                            <tr>
                                <td style="font-family: Tahoma; font-size: 12px; font-weight: bold;
                                    color: Red; height: 19px;">
                                    <asp:Label ID="Label1" runat="server" EnableViewState="False" ForeColor="Red" Text="Note : Debit card option is temporarily disabled.
                                    Please use other options. We regret the inconvenience."></asp:Label>
                                </td>  

                            </tr>
                         </table>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2" align="center" style="text-align:center; margin:0px auto;">
                        <table style="text-align:center;" align="center">
                            <tr align="center" style="text-align:center; margin:0px auto;">
                                <td align="center" class="HeadTxtBWOPadding " >
                                    <span style="color: #ff0000">**</span>                     
                                </td>
                                <td>
                                    <asp:Label ID="lblNote2" runat="server" class="HeadTxtBWOPadding borderBtm" style="font-family: Verdana; font-size: 10pt;
                                            border: 100%;"></asp:Label>    
                                </td>   
                            </tr>
                        </table>
                    </td>
                </tr>
                </table>
                <table align="center" class="paddingLR" cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td align="left" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trConfirmationBank" runat="server">
                        <td class="HeadTxtBWOPadding borderBtm" align="left">
                            <span class="ClsLblLgnd">Selected Bank / Card :</span>
                        </td>
                        <td class="HeadTxtBWOPadding borderBtm" align="left">
                            <asp:Label ID="lblbankNameVal" runat="server" CssClass="LblNrmlB"></asp:Label>
                            <asp:HiddenField ID="hidOrigionalAmt" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="HeadTxtBWOPadding borderBtm" align="left">
                            <span class="ClsLblLgnd">Payable Amount :</span>                              
                            <span style="color: #ff0000">**</span>
                        </td>                        
                        <td class="HeadTxtBWOPadding borderBtm" align="left">
                            <asp:Label ID="lblAmountPay" runat="server" BorderColor="#333300" BorderStyle="Solid"
                                BorderWidth="1px" CssClass="LblNrmlB"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" style="font-family: Tahoma; font-size: 14px; font-weight: bold;
                            color: Red; height: 19px;" visible="false">
                            <asp:Label ID="lblErrorMsg" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                        </td>
                        <td class="TxtNormal">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="TxtNormal" valign="top" colspan="2">
                            &nbsp;
                        </td>
                    </tr>                    
                    <tr>
                        <td align="center" class="TxtNormal" valign="top" colspan="2">
                            <asp:HiddenField ID="hidParameters" runat="server" />
                            <asp:HiddenField ID="hidRequestUrl" runat="server" />
                            <asp:HiddenField ID="hidGatewayId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidAccessCode" runat="server" Value="" />
                            <asp:HiddenField ID="hidAcademicYearId" runat="server" Value="0" />                            
                            <asp:Button runat="server" ID="btnConfirm" Text="Confirm" CssClass="ClsButton" OnClick="btnConfirm_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                        </td>
                    </tr>
                </table>
                <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
                </b></b>
            </div>
            <br />
        </div>
    </div>   

    <%--<script type="text/javascript" src="https://services.billdesk.com/checkout-widget/src/app.bundle.js?v=1.3" async></script>--%>

    <script type="text/javascript" src="https://pgi.billdesk.com/payments-checkout-widget/src/app.bundle.js" async></script>
    
    <script language="JavaScript" type="text/javascript">
        _clientlblErrorMsg = "<%=this.lblErrorMessage.ClientID %>";
        _clientdivNetBanking = "<%=this.divNetBanking.ClientID %>";
        _clienthidGatewayId = "<%=this.hidGatewayId.ClientID %>"

        window.history.forward(1);
        history.go(1);
        var blockedBanks = eval('[' + $get('<%=this.hidBlockedBanksJSON.ClientID%>').value + ']')[0];

        // Register listeners for Postbacks
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);

        function BeginRequestHandler(sender, args) {
            if ($get("<%=this.btnProceed.ClientID %>") != null)
                $get("<%=this.btnProceed.ClientID %>").disabled = true;
            var postBackElement = sender._postBackSettings.sourceElement;
             if (postBackElement.id == "<%=this.btnConfirm.ClientID %>") 
                $get("<%=this.btnConfirm.ClientID %>").disabled = true;           
        }

        function EndRequestHandler(sender, args) {
            init();
            if ($get("<%=this.btnProceed.ClientID %>") != null)
                $get("<%=this.btnProceed.ClientID %>").disabled = false;
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == "<%=this.btnProceed.ClientID %>") {
                $get("<%=this.btnConfirm.ClientID %>").disabled = true;

                //if ($('#' + _clienthidGatewayId).val() == 6)
                if (document.getElementById(_clienthidGatewayId).value == "6")
                    SubmitForm(0);
                else if (document.getElementById(_clienthidGatewayId).value == "8")
                    SubmitBilldeskForm()
                else if (document.getElementById(_clienthidGatewayId).value == "9") {
                    SubmitBilldeskDYPForm()
                }
                else if (document.getElementById(_clienthidGatewayId).value == "10")
                    SubmitCCAvenueForm()
                else if (document.getElementById(_clienthidGatewayId).value == "12")
                    SubmitCCAvenueVPMCPSForm()                
                 else
                     SubmitForm(1);
            }
         }

         function SubmitCCAvenueForm() {
             
             var msgData = $get("<%=this.hidParameters.ClientID %>").value;
             var returnURL = $get("<%=this.hidRequestUrl.ClientID %>").value;
             var code = $get("<%=this.hidAccessCode.ClientID %>").value;

             $get('nonseamless').action = returnURL;

             var input = document.createElement("input");
             input.setAttribute("type", "hidden");
             input.setAttribute("id", "encRequest");
             input.setAttribute("name", "encRequest");
             input.setAttribute("value", msgData);
             document.getElementById("nonseamless").appendChild(input);

             var input1 = document.createElement("input");
             input1.setAttribute("type", "hidden");
             input1.setAttribute("id", "Hidden1");
             input1.setAttribute("name", "access_code");
             input1.setAttribute("value", code);
             document.getElementById("nonseamless").appendChild(input1);

             $get('nonseamless').submit()

         }

         function SubmitCCAvenueVPMCPSForm() {

             var msgData = $get("<%=this.hidParameters.ClientID %>").value;
             var returnURL = $get("<%=this.hidRequestUrl.ClientID %>").value;
             var code = $get("<%=this.hidAccessCode.ClientID %>").value;

             $get('nonseamless').action = returnURL;

             var input = document.createElement("input");
             input.setAttribute("type", "hidden");
             input.setAttribute("id", "encRequest");
             input.setAttribute("name", "encRequest");
             input.setAttribute("value", msgData);
             document.getElementById("nonseamless").appendChild(input);

             var input1 = document.createElement("input");
             input1.setAttribute("type", "hidden");
             input1.setAttribute("id", "Hidden1");
             input1.setAttribute("name", "access_code");
             input1.setAttribute("value", code);
             document.getElementById("nonseamless").appendChild(input1);

             $get('nonseamless').submit()

         }

        function SubmitBilldeskDYPForm() {
            var msgData = $get("<%=this.hidParameters.ClientID %>").value;
            var returnURL = $get("<%=this.hidRequestUrl.ClientID %>").value;

            bdPayment.initialize({
                msg: msgData,
                callbackUrl: returnURL,
                options: {
                    enableChildWindowPosting: true,
                    enablePaymentRetry: true,
                    retry_attempt_count: 3
                }
            });
        }

        function SubmitBilldeskForm() {            
            $get('PostForm').action = $get("<%=this.hidRequestUrl.ClientID %>").value;          
            var vPostForm = document.PostForm;
            vPostForm.submit();           
        }

        function SubmitForm(cnt) {
        
            $get('PostForm').action = $get("<%=this.hidRequestUrl.ClientID %>").value;                                                
            var sValues = $get("<%=this.hidParameters.ClientID %>");
            var sArr = sValues.value.split(",");
            if (sArr.length > cnt) {
                for (var i = 0; i < sArr.length; i++) {
                    var sTemp = sArr[i].split("=");
                    var input = document.createElement("input");
                    input.setAttribute("type", "hidden");
                    input.setAttribute("name", sTemp[0]);
                    input.setAttribute("value", sTemp[1]);
                    document.getElementById("PostForm").appendChild(input);                    
                }
                var vPostForm = document.PostForm;
                vPostForm.submit();
            }
        }

        function ValidateBlockedBanks(src, args) {            
            args.IsValid = true;
            var cmbBanks = $get("<%=this.cmbBanks.ClientID %>");
            var bankName = cmbBanks.options[cmbBanks.selectedIndex].text;
            if (blockedBanks != undefined && blockedBanks[cmbBanks.value]) {
                args.IsValid = false;
                if ($get("<%=this.btnProceed.ClientID %>") != null)
                    $get("<%=this.btnProceed.ClientID %>").disabled = true;
                var blockedBank = blockedBanks[cmbBanks.value];
                var startDate = new Date(blockedBank.StartDate).format("dd-MMM-yyyy hh:mm tt");
                $get(_clientlblErrorMsg).innerHTML = "";
                if (blockedBank.EndDate == "0") {
                    src.errormessage = bankName + " is unavailable for the online fee payment from " + startDate + ".";
                } else {
                    var endDate = new Date(blockedBank.EndDate).format("dd-MMM-yyyy hh:mm tt");
                    src.errormessage = bankName + " is unavailable for the online fee payment from " + startDate + " to " + endDate + ".";
                }
            }
            else {
                if ($get("<%=this.btnProceed.ClientID %>") != null)
                $get("<%=this.btnProceed.ClientID %>").disabled = false;
            }
            return args.IsValid;
        }

        function ValidateBank(src, args) {            
            args.IsValid = true;
            var cmbBanks = $get("<%=this.cmbBanks.ClientID %>");
            if (cmbBanks != null && cmbBanks.value == "0") {
                args.IsValid = false;
                src.errormessage = "Bank should be selected.";
            }
            return !args.IsValid;
        }

        function fireEvent(element, event) {            
            var evt;
            if (document.createEventObject) {
                // dispatch for IE
                evt = document.createEventObject();
                return element.fireEvent('on' + event, evt);
            }
            else {
                // dispatch for firefox + others
                evt = document.createEvent("HTMLEvents");
                evt.initEvent(event, true, true); // event type,bubbling,cancelable
                return !element.dispatchEvent(evt);
            }
        }

        function banklistOnChange() {            
            __doPostBack('<%= this.cmbBanks.ClientID %>', '');
        }

        function banklistOnKeyUp(evt) {            
            // prevent tab, alt, ctrl keys from fireing the event
            if (evt && evt.keyCode && (evt.keyCode == 1 || evt.keyCode == 9 || evt.keyCode == 16 || evt.keyCode == 18 || evt.ctrlKey))
                return true;

            fireEvent($get('<%= this.cmbBanks.ClientID %>'), "change");
            return true;
        }

        function OpenTermsOfusePopup() {        
            var qStr = document.getElementById('<%=this.hidTerms.ClientID %>').value            
            window.open('Admission/TermsOfUseForOnlinePayment.aspx?' + qStr, '_blank', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=50,left=400,width=850,height=450');
        }

        function ValidateTerms(src, args) {        
        var chkStatus = document.getElementById('<%=this.chkTermOfuse.ClientID %>').checked        
            args.IsValid = chkStatus;
            return !chkStatus;
        }

        function init() {            
            var cmbBanks = $get('<%= this.cmbBanks.ClientID %>');

            if (cmbBanks) {
                addEvent(cmbBanks, "keyup", banklistOnKeyUp);
                addEvent(cmbBanks, "change", banklistOnChange);
            }
            return true;
        }

        function addEvent(elem, event, handler) {            
            if (elem) {
                if (elem.addEventListener)
                    elem.addEventListener(event, handler, false);
                else if (elem.attachEvent)
                    elem.attachEvent("on" + event, handler);
                else
                    elem[event] = handler;
            }
        }

        addEvent(window, "load", init);
        _clientbtnConfirm = "<%=this.btnConfirm.ClientID %>"
        function DisableButtons() {          
              //$('#<%=this.btnConfirm.ClientID %>').attr('disabled', 'disabled');
            //__doPostBack(document.getElementById(_clientbtnConfirm).name, '')
        }
        window.history.forward(1)
        history.go(1);
    </script>
    <script type="text/javascript" for="window" event="onunload">
        if(window.opener)
		    window.opener.location = window.opener.location;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFooter" runat="Server">
    <form id="PostForm" name="PostForm" action="" method="post" >     
 </form>


  <form id="nonseamless" method="post" name="redirect"> 
       
    </form>

</asp:Content>
