<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterForTermsAndUse.master" AutoEventWireup="true" CodeFile="TermsOfUseForOnlinePayment.aspx.cs" Inherits="TermsOfUseForOnlinePayment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .ClsBackBtn {
            background: #c62d1f linear-gradient(to bottom, #ff9a82 5%, #e94c29 100%) repeat scroll 0 0 !important;
            border: 1px solid #ff623f !important;
            color: white !important;
            border-radius: 3px !important;
            color: rgb(255, 255, 255) !important;
            display: inline-block !important;
            height: auto !important;
            min-width: 75px;
            padding: 4px 10px !important;
            text-transform: uppercase;
            width: auto !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="width: 98%;" align="center" >
        <div align="center" >
            <!-- Data Insert Here -->
                <div align="center">
                <br />
                    <div id="nifty" align="center">
                        <div class="paddingLR" style="padding-top:1px;" align="center">
                            <div class="HeadTxtB borderBtm admissiondivstyle" align="center">
                                Terms of Use&nbsp;
                            </div>
                            <div class="TxtNormal" align="left" style="padding:1px 0 0 2px;">
                               
								<div id="divOnline" runat="server">
									<div class="HeadTxtB borderBtm admissiondivstyle" align="center" style="font-size:10pt; margin-top:10px" >The following are the Terms and Conditions for online Payments. Please go through the same carefully.</div>
									<div class="TxtNormal paddingL">
									<ol  style="text-align:left; margin-top:12px; margin-bottom:1px;">
                                    <li> I / We agree and accept the services provided by  <asp:Label ID="lblspacific"  class="TxtNormal" runat="server" Text="Axis Bank and Aggregator"
                                                   ></asp:Label>. At my / our request to carry out my payments on my / our account, given by me / us.</li>
										<li>I / We have no objection whatsoever, to the online payment facility providing my / our billing details to the Institute.</li>
										<li>While the Institute will take all reasonable  steps to ensure the accuracy of the payment details, the Institute is not liable for any error. I / We shall not hold the Institute responsible for any  loss, damages, etc. that may be incurred / suffered by me / us if the information contained turns out to be inaccurate / incorrect.</li>
										<li> I / We agree that any disputes on Payment details will be settled directly with the Institute and the responsibility limited to provision of information only.</li>
										<li> I / We agree that we will make bill amount  payments as required by the Institute. I / We will not hold the Institute  responsible for rejecting the payment amount because of incorrect or incomplete  entries.</li>
										<li>I / We agree that the record of the instructions given and transactions with the Institute shall be conclusive proof and binding for all purposes and can be used as evidence in any  proceeding.</li>
										<li> I / We agree that charges, if any, for the online payment services will be at the sole discretion of the Institute is at  liberty to vary the same from time to time, without giving any notice.</li>
										<li> I / We agree that the Institute is at liberty to  withdraw at anytime the online payment facility, or any services provided there  under, in respect of any or all the account(s) without assigning any reason  whatsoever, without giving me / us any notice.      </li>
										<li id = "liSpecific1" runat ="server" visible = "false" >I / We agree that if Fees paid twice for one transaction, I / we okay to receive the one amount refunded within 15 to 20 working days via same source.</li>
										<li id = "liSpecific2" runat = "server" visible = "false" >I / We agree and accept that Surcharge Amount will not be refunded/ reversed back.</li>
                                        <li id = "liSpecific3" runat = "server" visible = "false" >I / We agree that in case of Cancellation of Admission, there will be no upfront refund. I / We need to visit Institute and contact Accounts department ( <asp:Label ID="lblSpecificTeachernameAndContact"  class="TxtNormal" runat="server"></asp:Label>) for further procedure. If Institute  authorities agree for refund, then refund will be made via same source.</li>
									</ol>
									</div>
								</div>
							</div>
                             <div class="HeadTxtB admissiondivstyle" style="text-align:center;">
                               <asp:Button ID="btnBack" Text="Back" runat="server" CssClass="ClsBackBtn" CausesValidation="False"
								            TabIndex="2" UseSubmitBehavior="false" OnClick="btnBack_Click" />

                            </div>
                        </div>
                    <br />
                    </div>
                </div>
        </div>
    </div>
</asp:Content>

