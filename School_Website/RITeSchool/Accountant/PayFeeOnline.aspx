<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" AutoEventWireup="true" CodeFile="PayFeeOnline.aspx.cs" Inherits="PayFeeOnline" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
		<table style="width: 98%" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td>
					<Wizard:AdmissionSteps ID="SubmissionWizardSteps" runat="server" ActiveSteps="1"
					                       IsStudentFee="true"></Wizard:AdmissionSteps>
				</td>
			</tr>
			<tr>
				<td>
					<table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
						<tr>
							<td class="ClsGrayMainTitle" style="height: 20px; width: 99%;" align="left">
								<asp:Label ID="lblHeader" Text="Fee Payment" runat="server" CssClass="MainTitleHead"
								           Font-Bold="True"></asp:Label>
							</td>
						</tr>
                        <tr id="trErrMessage" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblErrMessage" runat="server" Text="" Font-Bold="true" ForeColor="Red" EnableViewState="false" ></asp:Label>
                            </td>
                        </tr>
					</table>
					<table border="0" cellpadding="0" cellspacing="2" style="padding-left: 5px; width: 95%;"
					       align="center">
						<tr id="trBankList" runat="server" visible="false">
							<td colspan="2">
							</td>
							<td align="center" class="ClsGreenBG" width="35%">
								<asp:HyperLink ID="hlnkBankDetails" runat="server" Text="Online Bank / Card Details" NavigateUrl="OnlineBankDetails.aspx"
								               CssClass="SubTitle " Style="padding-right: 0px;white-space : nowrap;" />
							</td>
						</tr>
                        <tr>
                            <td style="height:5px;" colspan="3">
                                <asp:CustomValidator ID="cstMinimumAmount" runat="server" ErrorMessage="" ClientValidationFunction="CheckMinimumAmount"
                                    Display="None"></asp:CustomValidator>
                            </td>                            
                        </tr>
                        <tr id="trFeeDetailsPPSH" runat="server" visible="false">
                            <td colspan="3">
                                <table width="100%">                                  
                                   <tr>
                                       <td>
                                           <asp:ListView ID="lstvwStudentFee" runat="server" ViewStateMode="Enabled"
                                               
                                               DataKeyNames="SchoolwiseStudentFeeId,DebitOrCredit,SerialNumber,ReceiptNumberOutput,StandardwiseFeeTypeId,ConcessionAmount,AccountHeaderId" 
                                               onitemdatabound="lstvwStudentFee_ItemDataBound">
                                               <LayoutTemplate>
                                                   <table width="100%" runat="server" viewstatemode="Enabled" id="tblStudentInfo" style="color: #333333" cellpadding="0"
                                                       cellspacing="1" class="GridBorder">
                                                       <tr id="trHeader" runat="server" class="ClsMarksGridHeader">
                                                           <%--<th id="thchk" runat="server" align="center" width="3%">                                                                    
                                                               <asp:CheckBox ID="chkSelectAll" runat="server" ViewStateMode="Enabled" onclick="CheckAll(this);" AutoPostBack="false" />
                                                           </th>--%>
                                                           <th id="thFeeType" runat="server" align="left" width="15%" style="padding-left: 5px;
                                                               font-weight: inherit">
                                                               Fee Type
                                                           </th>
                                                           <th id="thPaybleFor" runat="server" align="left" width="18%" style="padding-left: 5px;
                                                               font-weight: inherit">
                                                               Payble For
                                                           </th>
                                                           <th id="thAmount" runat="server" align="right" width="8%" style="padding-right: 5px;
                                                               font-weight: inherit">
                                                               Amount
                                                           </th>
                                                           <th id="thDueDate" runat="server" align="center" width="16%" style="font-weight: inherit;
                                                               white-space: nowrap">
                                                               Due Date
                                                           </th>
                                                           <th id="thAmountPayable" runat="server" align="right" width="12%" style="padding-right: 5px;
                                                               font-weight: inherit; white-space: nowrap">
                                                               Amt. Payable
                                                           </th>                                                               
                                                           <th id="thLateFee" runat="server" align="center" width="8%" style="font-weight: inherit;
                                                               white-space: nowrap">
                                                               Late Fee
                                                           </th>
                                                           <th id="thActualAmount" runat="server" align="center" width="12%" style="font-weight: inherit;
                                                               white-space: nowrap">
                                                               Actual Amount
                                                           </th>
                                                       </tr>
                                                       <tr runat="server" id="itemPlaceholder">
                                                       </tr>
                                                   </table>
                                               </LayoutTemplate>
                                               <ItemTemplate>
                                                   <tr id="trlstvwRow" runat="server" viewstatemode="Enabled" class="ClsMarksGridAltRowN">
                                                       <td id="tdFeeType" runat="server" align="left" style="padding-left: 5px">
                                                           <asp:Label ID="lblFeeType" runat="server" ViewStateMode="Enabled" Text='<%# Eval("FeeType") %>' />                                                          
                                                       </td>
                                                       <td id="tdPaybleFor" runat="server" align="left" style="padding-left: 5px">
                                                           <asp:Label ID="lblPaybleFor" runat="server" ViewStateMode="Enabled" Text='<%# Eval("PayableFor") %>' />
                                                           <asp:DropDownList ID="cmbPayableFor" runat="server" ViewStateMode="Enabled" Visible="false" CssClass="MidCombo"
                                                               Enabled="false">
                                                           </asp:DropDownList>
                                                           <asp:TextBox ID="txtNewPayableFor" runat="server" ViewStateMode="Enabled" CssClass="MidTxtBox" Visible="false"
                                                               Enabled="false"></asp:TextBox>
                                                       </td>
                                                       <td id="tdAmount" runat="server" align="right" style="padding-right: 5px">
                                                           <asp:Label ID="lblAmount" runat="server" viewstatemode="Enabled" Text='<%# Eval("Amount") %>' />
                                                       </td>
                                                       <td id="tdDueDate" runat="server" align="center">                                                               
                                                           <asp:Label ID="lblDueDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("PaymentDate","{0:dd-MMM-yyyy}") %>' />
                                                           <asp:TextBox ID="txtDueDate" runat="server" ViewStateMode="Enabled" Width="80px" CssClass="MidCombo" MaxLength="11"
                                                               AutoPostBack="false" Visible="false"></asp:TextBox>
                                                           <rjs:PopCalendar ID="calDueDate" runat="server" ViewStateMode="Enabled" Control="txtDueDate" Format="dd MMM yyyy" Culture="en"
                                                               Visible="false" ShowWeekend="true" ShowErrorMessage="false" />
                                                           <rjs:PopCalendarMessageContainer ID="PopCalendarMessageContainer3" runat="server" ViewStateMode="Enabled"
                                                               Calendar="PopCalendar3" Visible="false" />
                                                       </td>
                                                       <td id="tdAmountPayable" runat="server" viewstatemode="Enabled" align="right" style="padding-right: 5px">
                                                           <asp:Label ID="lblAmountPayable" runat="server" ViewStateMode="Enabled" Text='<%# Eval("AmountPayable") %>' />
                                                       </td>                                                          
                                                       <td id="tdLateFee" runat="server" align="right" style="padding-right: 5px">
                                                           <asp:Label ID="lblLateFee" runat="server" ViewStateMode="Enabled" Text='<%# Eval("LateFeeAmount") %>' />
                                                       </td>
                                                       <td id="tdActualAmount" runat="server" align="center">                                                               
                                                           <asp:TextBox ID="txtActualAmount" runat="server" ViewStateMode="Enabled" MaxLength="6" CssClass="MidTxtBox" AutoPostBack="false" 
                                                               Width="70px" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                               onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                               ondrop="event.returnValue=false"></asp:TextBox>
                                                           <asp:HiddenField ID="hidPreviousActualAmt" runat="server" ViewStateMode="Enabled"/>
                                                       </td>
                                                   </tr>
                                               </ItemTemplate>
                                               <EmptyDataTemplate>
                                                   <table width="100%">
                                                       <tr>
                                                           <td class="LblNoRecord" align="center">
                                                               No Records Found.
                                                           </td>
                                                       </tr>
                                                   </table>
                                               </EmptyDataTemplate>
                                           </asp:ListView>
                                       </td>
                                   </tr>                                  
                                </table>
                            </td>
                        </tr>
                        <tr>
                             <td style="height:10px;" colspan="3"></td>
                        </tr>
						<tr>
							<td align="right" valign="top" class="ClsBorderlight" style="width: 42%">
								<asp:Label ID="Label16" runat="server" Text="Payment Date :" CssClass="ClsLabel"
								           EnableViewState="false"></asp:Label>
							</td>
							<td align="left" colspan="2">
								<asp:TextBox ID="txtPaymentDate" CssClass="SmlTxtBox" runat="server" TabIndex="1"
								             Enabled="false"></asp:TextBox>
								<rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtPaymentDate" Format="dd MMM yyyy"
								                 ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Cheque date should not be blank."
								                 Enabled="false" />
							</td>
						</tr>
						<tr>
							<td align="right" valign="top" class="ClsBorderlight" style="width: 35%">
								<asp:Label ID="Label1" runat="server" Text="Payable Amount :" CssClass="ClsLabel"
								           EnableViewState="false"></asp:Label>
							</td>
							<td align="left" colspan="2">
								<asp:TextBox ID="txtPayableAmt" CssClass="SmlTxtBox" runat="server" TabIndex="2"
								             Enabled="false"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td align="right" valign="top" class="ClsBorderlight" style="width: 35%">
								<asp:Label ID="Label2" runat="server" Text="Late Fee Amount :" CssClass="ClsLabel"
								           EnableViewState="false"></asp:Label>
							</td>
							<td align="left" width="70%">
								<asp:TextBox ID="txtLateFeeAmt" CssClass="SmlTxtBox" runat="server" TabIndex="2"
								             Enabled="false"></asp:TextBox> &nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblLateFeeDetails" runat="server"  Visible="false"></asp:Label>
							</td>
							<td align="left">
							</td>
						</tr>
                        <tr id="trConcession" runat="server" visible="false">
							<td valign="top" class="ClsBorderlight" style="width: 35%">
								<asp:Label ID="Label5" runat="server" Text="Concession Amount :" CssClass="ClsLabel"
								           EnableViewState="false"></asp:Label>
							</td>
							<td align="left" colspan="2">
								<asp:TextBox ID="txtConcessionAmount" CssClass="SmlTxtBox" runat="server" TabIndex="2"
								             Enabled="false"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td align="right" valign="top" class="ClsBorderlight" style="width: 35%">
								<asp:Label ID="Label3" runat="server" Text="Amount to be paid :" CssClass="ClsLabel"
								           EnableViewState="false"></asp:Label>
							</td>
							<td align="left" colspan="2">
								<asp:TextBox ID="txtAmountTobePaid" CssClass="SmlTxtBox" runat="server" TabIndex="2"
								             Enabled="false"></asp:TextBox>
							</td>
						</tr>                        
						<tr>
							<td align="left" class="ClsBorderlight" style="width: 35%">
								<asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Remarks :" EnableViewState="false"></asp:Label>
							</td>
							<td align="left" class="ClsMdtStar" colspan="2">
								<asp:TextBox ID="txtRemarks" TabIndex="7" runat="server" MaxLength="2000" CssClass="SmlTxtBox"
								             Width="395px" TextMode="MultiLine" Enabled="false"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td colspan="3">
								&nbsp;
							</td>
						</tr>
                        <%--<tr>
                            <td colspan="2">
							<asp:CheckBox ID="chkTermOfuse" runat="server" Text=" I have read and agree to Payment" onclick="EnableDisablePayButton(this);"/>
                            <a style="color:Blue;text-decoration:underline;cursor:pointer;" onclick="OpenTermsOfusePopup();">Terms and Conditions. </a>
                            </td>

						</tr>--%>
                        <tr>
							<td colspan="3">
								&nbsp;
							</td>
						</tr>
                        <tr id="trConcesionMessage" runat="server" visible="false">
                            <td colspan="3" align="center">
                                <asp:Image ImageUrl="~/RITeSchool/images/newLink.gif" runat="server" ID="Image1" />
                                <asp:Label ID="lblConcessionMessage" runat="server" Text="" CssClass="ClsLabel" style="font-weight:bold;color:maroon;float:inherit;"></asp:Label>                                
                                <div style="height:5px;">
                                </div>
                            </td>
                        </tr>
                        <tr id="trNotePPSHStudent" runat="server" visible="false">
							<td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
								<asp:Label ID="lblPPSHNote" runat="server" class="LblNrmlB" style="font-weight: bold" EnableViewState="false" Text="Note "></asp:Label>
								<span class="colonPadding">:</span>
							</td>
							<td align="left" colspan ="2" class="ClsBorderlight" style="padding-left: 5px;">
								<asp:Label ID="lblPPSHNoteData" runat="server" BorderWidth="0px" Text="The late fee displayed here may vary later as per the revised Late fee structure." CssClass="LblSmlV"></asp:Label>
							</td>
						</tr>
						<tr>
							<td align="center" colspan="3">
								<asp:Button ID="btnPay" Text="Pay" runat="server" CssClass="ClsBtnMid" TabIndex="19"
								            UseSubmitBehavior="false" OnClick="btnPay_Click" />
								<asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtnMid" CausesValidation="False"
								            TabIndex="20" UseSubmitBehavior="false" OnClick="btnClose_Click" />
							</td>
						</tr>
						<tr>
							<td colspan="3">
								&nbsp;
							</td>
						</tr>
						<tr runat="server" id="trNote" visible="false">
							<td align="left" colspan="1" class="ClsBorderlight " style="background-color: #ffffc4; width: 19%;">
								<asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 1 :"
								           CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
							</td>
							<td align="left" colspan="2" class="ClsBorderlight" style="padding-left: 5px; width: 60%">
								<asp:Label ID="lblVerifyNote" runat="server" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
							</td>
						</tr>
					</table>
					
					<asp:HiddenField ID="hidStudentId" runat="server" />
					<asp:HiddenField ID="hidDuedates" runat="server" />
					<asp:HiddenField ID="hidStandard" runat="server" Value="0" />
					<asp:HiddenField ID="hidRemarks" runat="server" />
					<asp:HiddenField ID="hidAcademicYrId" runat="server" Value="0" />
					<asp:HiddenField ID="hidIsForNextYear" runat="server" Value="N" />
					<asp:HiddenField ID="hidTotalAmount" runat="server" />
					<asp:HiddenField ID="hidLateFeeAmount" runat="server" Value ="0"/>
					<asp:HiddenField ID="hidIsFinalYear" runat="server" Value="N" />
                    <asp:HiddenField ID="hidSchoolwiseStudentFeeId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsCautionMoneyPayentOnline" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsInternalFeePaymentOnline" runat="server" Value="0" />
                    <asp:HiddenField ID="hidInternalFeeDetailsID" runat="server" />
                    <asp:HiddenField ID="hidNextAcademicYearId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsOldAcademicYearPayment" runat="server" Value="0" />
                    <asp:HiddenField ID="hidActualAmount" runat="server" Value="0" />
                    <asp:HiddenField ID="hidFinalRemark" runat="server" Value="" />
                    <asp:HiddenField ID="hidPartialAmount" runat="server" Value="0" />
                    <asp:HiddenField ID="hidMinimumPartialAmount" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsPartialFeePaymentEnabled" runat="server" Value="False" />
                    <asp:HiddenField ID="hidRestrictStudentsFeePayment" runat="server" Value="N" />
                    <asp:HiddenField ID="hidConcessionAmount" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSelectedFeeType" runat="server" Value="" />
				</td>
			</tr>
		</table>
        </div>
		<script type="text/javascript">
			_clientbtnPay = "<%=this.btnPay.ClientID%>";
			_clientlstvwStudentFee = "<%=this.lstvwStudentFee.ClientID %>";
			_sClienttxtActualAmt = "<%=this.txtAmountTobePaid.ClientID %>";
			_sClienttxtRemarks = "<%=this.txtRemarks.ClientID %>"
			_sClienttxtLateFeeAmt = "<%=this.txtLateFeeAmt.ClientID %>"
			_sClienttxtPayableAmt = "<%=this.txtPayableAmt.ClientID %>"
			_sclienthidActualAmount = "<%=this.hidActualAmount.ClientID %>"
			_clienthidFinalRemark = "<%=this.hidFinalRemark.ClientID %>"
			_clienthidPartialAmount = "<%=this.hidPartialAmount.ClientID %>"
			_clienthidMinimumPartialAmount = "<%=this.hidMinimumPartialAmount.ClientID %>"
			_clienthidIsPartialFeePaymentEnabled = "<%=this.hidIsPartialFeePaymentEnabled.ClientID %>"

			$(document).ready(function () {

//			    if ($("#ctl00_PopupMainBody_chkTermOfuse").is(':checked')) {
//			            $("#ctl00_PopupMainBody_btnPay").removeProp("disabled");
//			        }
//			        else {
//			            $("#ctl00_PopupMainBody_btnPay").prop("disabled", true);
//			        }
			    
			});			

//			function EnableDisablePayButton(asCheckBox) {

//			    document.getElementById(_clientbtnPay).disabled = !document.getElementById(_clientchkTermOfuse).checked;
//			}

//			function OpenTermsOfusePopup() {

//			    window.open('../Admission/TermsOfUseForOnlinePayment.aspx', '_blank', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=50,left=400,width=850,height=450');
//			}

			function truncateRemark(remark) {
			    if (remark.length > 2000) {
			        return remark.substring(0, 1998) + "..";
			    }
			    return remark;
			}

			function CalculateActualAmt(obj, iRowCount) {			   
			    var PreviousTotalActualAmt = 0, PreviousActualAmt, AmountAdded;

			        var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");
			        var lblLateFee = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblLateFee");
			        var lblAmountPayable = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblAmountPayable");
			        if (lblAmountPayable != null && lblAmountPayable.innerHTML == "0")
			            lblAmountPayable = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblAmount");
			        var hidPreviousActualAmt = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidPreviousActualAmt");
			        PreviousTotalActualAmt = $get(_sClienttxtActualAmt).value;

			        var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");

			        if (txtActualAmount != null) {

			            if (PreviousTotalActualAmt == "-" || PreviousTotalActualAmt == "")
			                PreviousTotalActualAmt = 0;

			            if (parseInt(txtActualAmount.value) > parseInt(lblAmountPayable.innerHTML))
			                txtActualAmount.value = lblAmountPayable.innerHTML;

			            if (txtActualAmount.value == "")
			                txtActualAmount.value = "0";
			            if (hidPreviousActualAmt.value == "")
			                hidPreviousActualAmt.value = lblAmountPayable.innerHTML;

			            ChangeActualAmount();

			            $get(_sClienttxtActualAmt).value = parseInt(PreviousTotalActualAmt) - parseInt(hidPreviousActualAmt.value) + parseInt(txtActualAmount.value);
			            $get(_clienthidPartialAmount).value = $get(_sClienttxtActualAmt).value;
			            
			            if (parseInt($get(_sClienttxtActualAmt).value) < 0) {
			                $get(_sClienttxtConcessionAmt).value = "0";
			                CalculateTotalActualAmount();			                
			                var LateFee = $get(_sClienttxtLateFeeAmt).value;
			                var Concession = $get(_sClienttxtConcessionAmt).value;
			                $get(_sClienttxtAmtToBePaid).value = parseInt($get(_sClienttxtPayableAmt).value) + parseInt(LateFee) - parseInt(Concession);
			            }
			            hidPreviousActualAmt.value = txtActualAmount.value;
			        }			    
			    GenerateRemarks();
			}

			function ChangeActualAmount() {            
            if ($get(_clienthidIsPartialFeePaymentEnabled).value = "True") {
                    var iRowCount = 0;
                    var ActualAmount = 0;
			        var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");

			        while (txtActualAmount != null) {
			            ActualAmount = parseInt(ActualAmount) + parseInt(txtActualAmount.value);

			            iRowCount++;
			            txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");
			        }

			        $get(_sClienttxtPayableAmt).value = parseInt(ActualAmount);
			    }
			}

			function GenerateRemarks() {
			    if ($get(_clienthidIsPartialFeePaymentEnabled).value = "True") {
			        var strRemark = "";
			        var finalRemark = "";
			        var PaybleFor = "";
			        var FeeType = "";
			        var Amount = 0;
			        var iRowCount = 0;
			        var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");

			        $get(_sClienttxtRemarks).value = "";
			        while (txtActualAmount != null) {			           
			           var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
			           var lblFeeType = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblFeeType");
			           var lblPaybleFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblPaybleFor");

			           if (txtActualAmount.value != "0")
			               strRemark = strRemark + lblPaybleFor.innerHTML + " (" + lblFeeType.textContent + " - Rs. " + txtActualAmount.value + "/-) , ";

			           iRowCount++;
                       txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");
			        }

			        finalRemark = " Amount paid for " + strRemark;
			        var index = finalRemark.lastIndexOf(",");
			        finalRemark = finalRemark.substring(0, index) + finalRemark.substring(index + 1);

			        $get(_sClienttxtRemarks).value = finalRemark;
			        $get(_clienthidFinalRemark).value = finalRemark;

			        if (_sClienttxtLateFeeAmt != null && $get(_sClienttxtLateFeeAmt).value != "0")
			            GenerateLateFeeRemarks();

			        $get(_sClienttxtRemarks).value = truncateRemark($get(_sClienttxtRemarks).value);
			        $get(_clienthidFinalRemark).value = $get(_sClienttxtRemarks).value;
			    }
			}

			function GenerateLateFeeRemarks() {
			    if ($get(_clienthidIsPartialFeePaymentEnabled).value = "True") {			        
			        var strRemark = "";
			        var strSelectedFees = "";
			        var finalRemark = "";
			        var PaybleFor = "";
			        var FeeType = "";
			        var iRowCount = 0;

			        var lblLateFee = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblLateFee");
			        while (lblLateFee != null) {			            
			            var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
			            var lblPaybleFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblPaybleFor");			            
			            if (lblLateFee != null && lblLateFee.innerHTML != "" && lblLateFee.innerHTML != "0") {
			                if (strRemark.match(lblPaybleFor.innerHTML) == null) {
			                    strRemark = strRemark + lblPaybleFor.innerHTML;
			                    strRemark = strRemark + ", ";
			                }
			            }
                        
			            if (strSelectedFees.match(lblPaybleFor.innerHTML) == null) {
			                strSelectedFees = strSelectedFees + lblPaybleFor.innerHTML;
			                strSelectedFees = strSelectedFees + ", ";
			            }

			            iRowCount++;
			            lblLateFee = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblLateFee");		            
			        }

			        var index = strRemark.lastIndexOf(",");
			        strRemark = strRemark.substring(0, index) + strRemark.substring(index + 1);

			        var isLateFee = 0;
			        if (strRemark.trim() != "")
			            isLateFee = 1;
			        else
			            isLateFee = 0;

			        index = strSelectedFees.lastIndexOf(",");
			        strSelectedFees = strSelectedFees.substring(0, index) + strSelectedFees.substring(index + 1);

			        if (isLateFee == 1) {
			            strRemark = strRemark + " ( Rs. " + $get(_sClienttxtLateFeeAmt).value + "/-)  ";
			            finalRemark = "& Late fee for " + strRemark;
			        }
			        else {
			            strSelectedFees = strSelectedFees + " ( Rs. " + $get(_sClienttxtLateFeeAmt).value + "/-)  ";
			            finalRemark = "& Late fee for " + strSelectedFees;
			        }

			        $get(_sClienttxtRemarks).value = $get(_sClienttxtRemarks).value + finalRemark;
			        $get(_sClienttxtRemarks).value = truncateRemark($get(_sClienttxtRemarks).value);
			        $get(_clienthidFinalRemark).value = $get(_sClienttxtRemarks).value;
			    }
			}

			function CheckMinimumAmount(oSrc, args) {
			    var TotalPartialAmont = $get(_clienthidPartialAmount).value;
			    var MinimumAmount = $get(_clienthidMinimumPartialAmount).value;
			    var IsPartialFeePaymentEnabled = $get(_clienthidIsPartialFeePaymentEnabled).value;

			    if (IsPartialFeePaymentEnabled = "True" && parseInt(TotalPartialAmont) < parseInt(MinimumAmount)) {
			        alert('Total payable amount should be grater than Rs. ' + MinimumAmount+'.');
			        args.IsValid = false
			        return true
			    }
			    args.IsValid = true
			    return false
			}

			function ShowPendingFeeAlert() {
			    alert('You cannot pay current year fee till the pending payment of last year fee.');
			    return false;
			}	

            window.history.forward(1);
            history.go(1);

		</script>
        <script type="text/javascript" for="window" event="onunload">
            window.opener.location = window.opener.location;
        </script>
	
</asp:Content>