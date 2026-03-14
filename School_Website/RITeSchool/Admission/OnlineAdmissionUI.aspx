<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="OnlineAdmissionUI.aspx.cs" EnableEventValidation="false"
    Inherits="OnlineAdmissionUI" Title="Online Admission for 2013 - 14" ErrorPage="~/RITeSchool/Admission/Error.aspx"
    EnableViewState="false" %>

<%--<%@ OutputCache Duration="60" VaryByParam="none" %>--%>
<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
    .Height5
    {
        height:5px;
    }
    
    .Height20    {
        height:20px;
    }
    
    .Height10    {
        height:10px;
    }
    
    .clsMainPoints
    {
        padding-left:5px;
        color: #000033;
        padding-right:5px;
    }
    
    .BorderLeftCell
    {
        border-bottom:1px solid gray;border-right:1px solid gray
    }
    
    .BorderRightCell
    {
        border-bottom:1px solid gray;
    }

    .disabled-link 
     {
       text-decoration: none !important;  
       cursor: default !important;
       color:Green;
     }
    
</style>
    <div>
        <table>
            <tr>
                <td>
                    <Wizard:AdmissionSteps ID="SubmissionWizardSteps" runat="server" ActiveSteps="1">
                    </Wizard:AdmissionSteps>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 97%; height: 100%" align="center" id="divOuter" runat="server">
        <div id="nifty" align="center" style="height: 100%">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
            </b></b>
            <table class="paddingLR" cellspacing="2" cellpadding="0" border="0" style="width: 100%;
                height: 100%">
                <tr>
                    <td id="OnlineAdmissionText" runat="server" align="left" class="HeadTxtB borderBtm" style="height: 25px" colspan="2">                       
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2" style="font-family: Tahoma; font-size: 14px; font-weight: bold;
                        color: Red" visible="false">
                        <asp:Label ID="lblErrorMsg" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                 <tr id="trMCPSadmissionClosed" runat="server" visible="false">
                    <td align="left" colspan="2" style="font-family: Tahoma; font-size: 14px; font-weight: bold;
                        color: Red">
                        Online Admission is temporary closed.<br />Admissions are Open & available on personal visit basis.<br />Office Timing: 09:00 AM to 02:00 PM <br />Thank You <br />
                    </td>
                </tr>
                <tr id="trJOSAdmissionClosed" runat="server" visible="false">
                    <td align="left" colspan="2" style="font-family: Verdana; font-size: 16px; color: Blue">
                        <table>
                            <tr>
                                <td style="color:Red;">
                                    <b>Online Admissions are temporarily Closed.</b>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:15px;"></td>
                            </tr>
                            <tr>
                                <td>
                                    Call : 9970028563 (for any admission query)<br />    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>SEATS ARE AVAILABLE ON FIRST COME FIRST SERVE BASIS ONLY. </b>Admission Application Forms are available at
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    School Office between 10 am to 2 pm (Monday To Saturday on personal visit basis only) <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Admission Form Fee Rs 100/- Playgroup to UKG</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Admission Form Fee Rs 200/- Std-1 & above<br /></b>
                                </td>    
                            </tr>
                            <tr>
                                <td>
                                    <b>Age Criteria for Nursery :</b> 3 years completed on or before 30th September 2019, Likewise for other classes.
                                </td>
                            </tr>
                        </table>                             
                    </td>                    
                </tr>
                <tr runat="server" id="trLoginButton">
                    <td id="tdCongrates" runat="server" align="center" style="font-family: Tahoma; font-size: 14px;
                        font-weight: bold; color: Red" visible="false">
                        Admission closed for Nursery.
                        <script type="text/javascript" language="javascript">
                            setInterval('blinkIt()', 600)
                        </script>
                    </td>
                    <td class="TxtNormalB" align="right">
                        <asp:HiddenField ID="hidLotteryDate" runat="server" />
                        <asp:Label ID="lblLoginLink" runat="server" Text="If you have already submitted your application, click here to login."
                            Visible="false"></asp:Label>
                        <asp:Button ID="LoginButton" runat="server" BorderStyle="Solid" BorderWidth="1px"
                            Visible="false" CssClass="ClsButton" Text="Log In" PostBackUrl="~/RITeSchool/Admission/OnlineAdmissionlLoginUI.aspx" />
                    </td>
                </tr>
                <tr id="trDPISBranch" runat="server" visible="false">
                    <td align="center">
                        <div style="background-color:#gray;box-shadow: 2px 2px silver;width:350px;padding:5px;">
                                <asp:Label ID="lblBranchName" runat="server" Text="" style="font-size:20px;font-weight:bold;"></asp:Label>
                         </div>
                    </td>
                </tr>
                <tr id="trAdmissionProcessDetails" runat="server">
                    <td class="TxtNormalB" align="left" colspan="2">
                        <table align="left" cellspacing="1" style="width: 100%; border-collapse: collapse;
                            float: left">
                            <tr id="trOnlineHeaderOther" runat="server">
                                <td align="left">
                                    <span class="ClsLblLgnd" style="font-weight: bold">Online Admission Instructions :
                                    </span>
                                </td>
                            </tr>
                             <tr id="trOnlineHeaderDPIS" runat="server">
                                <td align="left">
                                    <span class="ClsLblLgnd" style="font-weight: bold">ONLINE ADMISSION INSTRUCTION :
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="1" cellspacing="0" width="100%">
                                        <tr>
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;">
                                                The Application Form can be filled online with the help of following instructions
                                                :
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                1.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Click on the 'Submit Form' button for the Online Admission Application. (Please
                                                refer the section "Standard List" mentioned below)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                2.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Fill out the details mentioned in the admission application form.
                                            </td>
                                        </tr>
                                        <tr id="trPaymentOnline" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                3.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal; color: Blue;">
                                               <span id="spnFormFeeNote" runat="server" style="font-weight:bold;" enableviewstate="true"></span> 
                                            </td>
                                        </tr>
                                        <tr id="trPaymentOffline" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                3.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal; color: Blue;">
                                                <b>Admission Application Form & Prospectus Fee of Rs.<%= SchoolBase.Settings.AdmissionFormFees %>/-
                                                is payable for each admission of any standard at time of admission in school office.</b>
                                            </td>
                                        </tr>
                                        <tr id="trPaymentOffline_SS" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                3.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal; color: Blue;">
                                                <b>Admission Application Form Fee of Rs.<%= SchoolBase.Settings.AdmissionFormFees %>/-
                                                is payable for each admission of any standard at time of admission in school office.</b>
                                            </td>
                                        </tr>
                                        <tr id="trOnlinePaymentDPIS" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                3.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal; color: Blue;">
                                                <b>Admission Application Form Fee of Rs.<%= SchoolBase.Settings.AdmissionFormFees %> (Non Refundable)
                                                is payable for each application of any grade.</b>
                                            </td>
                                        </tr>
                                        <tr id="trPrintPayment" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                                4.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                After completing Admission form, <b style="color: Blue;">please print the receipt and
                                                    the admission form before closing the screen.</b> If the screen is closed, the
                                                admission form/receipt will not be available. This receipt and admission form is
                                                required to be used as a proof at the time of taking admission.
                                            </td>
                                        </tr>
                                        <tr id="trPrintPaymentDPIS" runat="server" visible="false">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                            <%-- <label id="lblNo" runat="server" >   4.</label>--%>
                                            4.

                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                After completing Admission form, <b style="color: Blue;">please print the admission form before closing the screen.</b> If the screen is closed, the
                                                admission form will not be available. This admission form is
                                                required to be used as a proof at the time of taking admission.
                                            </td>
                                        </tr>
                                        <tr id="trPrint" runat="server" visible="false">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                                3.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                After completing Admission form, <b style="color: Blue;">please print the admission
                                                    form before closing the screen.</b> If the screen is closed, the admission form
                                                will not be available. This admission form is required to be used as a proof at
                                                the time of taking admission.
                                            </td>
                                        </tr>
                                        <tr id="trFive" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                              5.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Required documents are given on the first page of the admission form.                                                
                                            </td>
                                        </tr>    
                                        <tr id="trFiveDPIS" runat="server" visible = "false">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                              <label id="lblNo" runat="server" >    5.</label>
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                List of required documents are given on the first page of the admission form.                                                
                                            </td>
                                        </tr>                                        
                                        <tr id="trFour" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                                4.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Required documents are given on the first page of the admission form.
                                            </td>
                                        </tr>
                                        <tr id="trSix" runat="server" visible="false">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                                6.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">                                                
                                                Admission confirmation is subject to submission of the necessary documents and the payment of the fees.
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                                6.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Once the admission application form is submitted, you can check the status of the
                                                form by clicking on ‘Status’.
                                            </td>
                                        </tr>--%>
                                        <tr id="trSelectionCriteria" runat="server">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                <font style="font-family: Arial; font-size: 12px; font-style: normal; font-weight: bold;
                                                    color: #477694; letter-spacing: normal; text-align: justify; word-spacing: normal;">
                                                    <b>Selection Criteria</b></font>
                                            </td>
                                        </tr>
                                        <tr id="trSelectiontext" runat="server">
                                            <td id="tdSelectiontext" colspan="2" style="font-family: Verdana; font-size: 11px;
                                                font-style: normal; font-weight: normal; color: #000033; text-align: justify;
                                                word-spacing: normal;" valign="top" runat="server">
                                                The admission seats will be allotted on a lottery system at a random basis as selected
                                                on date
                                                <%=hidLotteryDate.Value %>
                                                by our online admission system.
                                            </td>
                                        </tr>
                                        <tr id="trSelectedCandidates" runat="server">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                <font style="font-family: Arial; font-size: 12px; font-style: normal; font-weight: bold;
                                                    color: #477694; letter-spacing: normal; text-align: justify; word-spacing: normal;">
                                                    <b>Announcements of Selected Candidates</b></font>
                                            </td>
                                        </tr>
                                        <tr id="trSelectedtext" runat="server">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top" runat="server" id="tdSelectedtext">
                                                The selected candidates will be informed via SMS on the mobile number mentioned
                                                on the application form.<%-- The candidates should check the status of their admission
                                                from the school web site.--%>
                                            </td>
                                        </tr>
                                        <tr id="trConfirmation" runat="server">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                <font style="font-family: Arial; font-size: 12px; font-style: normal; font-weight: bold;
                                                    color: #477694; letter-spacing: normal; text-align: justify; word-spacing: normal;">
                                                    <b>Confirmation of Admission</b></font>
                                            </td>
                                        </tr>
                                        <tr id="trConfirmationText" runat="server">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                After selection, the admission will be confirmed only after receiving required documents
                                                and fees.
                                            </td>
                                        </tr>
                                        <tr id="trConfirmation_Copy" runat="server" visible="false">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                <font style="font-family: Arial; font-size: 12px; font-style: normal; font-weight: bold;
                                                    color: #477694; letter-spacing: normal; text-align: justify; word-spacing: normal;">
                                                    <b>Confirmation of Admission</b></font>
                                            </td>
                                        </tr>
                                        <tr id="trConfirmationText_Copy" runat="server" visible="false">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                The admission will be confirmed only after receiving required documents and fees.
                                            </td>
                                        </tr>
                                        <tr id="trConfirmationText_BFS" runat="server" visible="false">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                Submission of this form does not gaurantee admission of the child. Seats are subject to availability.
                                            </td>
                                        </tr>
                                        <tr id="trAdmissionSpace" runat="server" visible="false" style="height:15px">
                                        <td colspan="2"></td>
                                        </tr>
                                        <tr id="trAdmission" runat="server" visible="false">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                <b>For Admission in 1st & above standard, Visit the office & can meet the Principal, Timing: 11 AM to 1 PM.</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                                &nbsp;
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
                <tr id="trAdmissionProcessPPSH" runat="server" visible="false">
                    <td>
                        <table width="100%" cellpadding="1" cellspacing="0" style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;">
                            <tr id="tr3" runat="server">
                                <td align="left">
                                    <span class="ClsLblLgnd" style="font-weight: bold">Online Admission Instructions :
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" >
                                    The Application Form can be filled online with the help of the following instructions:
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <ol style="line-height:20px;">
                                        <li>Click on the 'Admission Form' link for the Online Admission Application. (Please refer to the section "Grade Selection for Admission Application" mentioned below)</li>
                                        <li>Fill out the details mentioned in the admission application form.</li>
                                        <li>Admission Application Form Fee of Rs. 1000 is payable for each application of any grade.</li>
                                        <li>After filling up and submission of the online admission form, three pdf files will be generated containing :
                                        <ol style="list-style-type: lower-alpha;">
                                            <li>Form Fee Receipt</li>
                                            <li>Filled form with Administration Copy and the Teacher’s Copy</li>
                                            <li>Blank Medical History Sheet, Parental Consent Form along with undertakings</li>
                                        </ol>                                        
                                        </li>
                                        <li>An email will be sent with the attachments of above generated files on the 'Email for Communication' entered while filling the form. Kindly download, save, print , sign and share the signed scanned copy of the admission form with required documents to admissions@ppshinjewadi.com</li>
                                        <li>The required documents are mentioned on the first page of the Admission Form.</li>
                                        <li>Admission confirmation is subject to submission of the necessary authentic documents and the online payment of the fees.</li>
                                        <li>If the documents are not authentic and do not fulfill the eligibility criteria, the school will have the right to cancel the admission.</li>
                                        <li>Fees paid against Admission fees and quarterly fee at the time of admission will not be refunded under any circumstances except the Caution Money Deposit in the event of cancelling the admission.</li>
                                    </ol>
                                </td>
                            </tr>
                           
                           
                        </table>
                    </td>
                </tr>

                <asp:HiddenField ID="hidFormCount" runat="server" Value="" />
                <tr id="trAdmissionDetailsForPPSN" runat="server">
                    <td class="TxtNormalB" align="left" colspan="2">
                        <table align="left" cellspacing="1" style="width: 100%; border-collapse: collapse;
                            float: left">
                            <tr>
                                <td align="left">
                                    <span class="ClsLblLgnd" style="font-weight: bold">ONLINE ADMISSION INSTRUCTIONS FOR THE YEAR 2018 – 19
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="1" cellspacing="0" width="100%">
                                        <tr>
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;">
                                                The Application Form can be filled online with the help of following instructions
                                                :
                                            </td>
                                        </tr>
                                        <tr style="height:10px;">
                                            <td>
                                            </td>
                                        </tr>
                                          <tr>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                1.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Forms will be available <u>ONLINE ONLY</u> to ALL ELIGIBLE candidates. www.ppsnandedcity.com  The cost of the form is <u>Rs.800/- (Non-refundable)</u>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                2.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Click on the 'Admission Form' button for the Online Admission Application. (Please refer to the section "Grade List" mentioned below).
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                3.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Fill out the details mentioned in the admission application form.
                                            </td>
                                        </tr>
                                       <%-- <tr id="tr3" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                3.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                Admission Application Form Fee of <b> Rs.<%= SchoolBase.Settings.AdmissionFormFees %>/- (Non Refundable)</b>
                                                is payable for each application of any grade.
                                            </td>
                                        </tr>--%>
                                        <tr id="tr4" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                4.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                After completing the online admission form, pdf files will be generated containing the Administration Copy of the filled form along with the Medical History Sheet Form, Parental Consent Form and the payment receipt. Kindly download and save the admission form and payment receipt and print them subsequently at time of admission.
                                            </td>
                                        </tr>
                                        <tr id="tr5" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                5.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                               List of required documents are given in admission notice.
                                            </td>
                                        </tr> 
                                        <tr>
                                            <td style="height:10px;"></td>
                                        </tr>
                                        <tr id="tr15" runat="server">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                <font style="font-family: Arial; font-size: 12px; font-style: normal; font-weight: bold;
                                                    color: #477694; letter-spacing: normal; text-align: justify; word-spacing: normal;">
                                                    <b>Submission of Admission Form</b></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height:5px;"></td>
                                        </tr>
                                        <tr id="tr16" runat="server">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                After selection, the admission will be confirmed only after receiving the required documents and fees.
                                            </td>
                                        </tr>   
                                        <tr>
                                            <td style="height:5px;"></td>
                                        </tr>
                                        <tr id="tr1" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                1.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                               For Online Form Fee Payment, you should be confirm payment amount and proceed further to make payment through Internet Banking/Debit Card/Credit Card. Please make sure you know your Net banking USER ID and PASSWORD. Banks may differ as bank selection will happen at payment gateway.
                                            </td>
                                        </tr> 
                                        <tr id="tr6" runat="server">
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top" width="15">
                                                2.
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                               	GST is applicable only on Bank Processing Charges.
                                            </td>
                                        </tr>                                     
                                        <tr id="tr19" runat="server" visible="false" style="height:15px">
                                        <td colspan="2"></td>
                                        </tr>
                                        <tr id="tr20" runat="server" visible="false">
                                            <td colspan="2" style="font-family: Verdana; font-size: 11px; font-style: normal;
                                                font-weight: normal; color: #000033; text-align: justify; word-spacing: normal;"
                                                valign="top">
                                                <b>For Admission in 1st & above standard, Visit the office & can meet the Principal, Timing: 11 AM to 1 PM.</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;" valign="top">
                                                &nbsp;
                                            </td>
                                            <td style="font-family: Verdana; font-size: 11px; font-style: normal; font-weight: normal;
                                                color: #000033; text-align: justify; word-spacing: normal;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
                <tr id="trPaymentInfo" runat="server">
                    <td colspan="2">
                        <table width="100%" style="margin-bottom:10px; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);">
                            <tr style="background-color: #45ABE4;height:30px;color:White;font-weight:bold;">
                                <td align="justify" style="padding-left:5px;">
                                    PAYMENT INFORMATION
                                </td>
                            </tr>
                            <tr style="height:5px;">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" style="padding-left:5px;color: #000033">
                                    For Online Fee Payment, you need to confirm payment amount and proceed further to make payment through Internet Banking. Please make sure you know your Net banking USER ID and PASSWORD.
                                </td>
                            </tr>
                            <tr style="height:5px;">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" style="padding-left:5px;">
                                   <span style="color: #000033">Note : </span><asp:Label ID="Label9" runat="server"  Text="The GST is applicable only on Processing Charges." style="color:Red;" CssClass="LblUsrNameHead"></asp:Label>                                   
                                </td>
                            </tr>     
                            <tr style="height:5px;">
                                <td>
                                </td>
                            </tr>                       
                        </table>
                    </td>
                </tr>

                <tr id="trPPSHAgeCriteria" runat="server" visible="false">
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <b>Eligibility Criteria</b>
                                </td>
                            </tr>
                             <tr>
                                <td colspan="2" align="center">
                                    <table cellpadding="1" style="border:1px solid gray;line-height:20px">
                                        <tr style="border:1px solid gray">
                                            <td style="width:200px;" class="BorderLeftCell">
                                               <b>CLASS</b>
                                            </td>
                                            <td style="width:300px;" class="BorderRightCell">
                                                <b>AGE CRITERIA</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BorderLeftCell">
                                                NURSERY
                                            </td>
                                            <td class="BorderRightCell">
                                                Born between 1<sup>st</sup> Oct 2022 to 31<sup>st</sup> Dec 2023
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BorderLeftCell">
                                                JUNIOR KG
                                            </td>
                                            <td class="BorderRightCell">
                                                Born between 1<sup>st</sup> Oct 2021 to 31<sup>st</sup> Dec 2022
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BorderLeftCell">
                                                SENIOR KG
                                            </td>
                                            <td class="BorderRightCell">
                                               Born between 1<sup>st</sup> Oct 2020 to 31<sup>st</sup> Dec 2021
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="BorderLeftCell">
                                                1
                                            </td>
                                            <td class="BorderRightCell">
                                               Born between 1<sup>st</sup> Oct 2019 to 31<sup>st</sup> Dec 2020
                                            </td>
                                        </tr>                                        
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>(Age criteria are strictly adhered to as per the Education Department’s guidelines.)</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="Height10">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Preference of Admission:</b>
                                </td>
                            </tr>
                            <tr>    
                                <td>
                                    We strictly follow first come first serve.We have limited seats for each class. Hence admissions will be granted subject to availability of seats.
                                </td>
                            </tr>
                             <tr>
                                <td class="Height10">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr id="trPaymentInfoPPSN" runat="server" visible="false">
                    <td colspan="2">
                        <table width="100%" style="margin-bottom:10px;text-align:justify; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);">
                            <tr style="background-color: #45ABE4;height:30px;color:White;font-weight:bold;">
                                <td align="justify" style="padding-left:5px;">
                                    ONLINE ADMISSION INSTRUCTIONS
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                    The Application Form can be filled online with the help of the following instructions :
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                    1. Forms will be available <u>ONLINE ONLY</u> to ALL ELIGIBLE candidates. The cost of the form is <b>Rs. 2500/- (Non-refundable)</b> The form fees will not be refunded under any circumstances.
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                    2. Click on the 'Submit Form' button for the Online Admission Application. (Please refer to the section "Grade List" mentioned below).
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                    3. Fill out the details mentioned in the admission application form.
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                    4. After completing the online admission form, PDF files will be generated containing the Administration Copy of the filled form along with the Medical History Sheet Form, Parental Consent Form, Undertaking and the payment receipt. Kindly download and save the admission form and payment receipt and print them subsequently at time of admission.
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                    5. List of required documents are given in admission notice, Point No 4.
                                </td>
                            </tr>
                            <tr class="Height20">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                    <span style="font-size:12px;"><b>Submission of Admission Form</b></span>
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                   After selection, the admission will be confirmed only after receiving the required documents and fees.(Refer Point No. 4 & 5 in admission notice)
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <%--<tr>
                                <td align="justify" class="clsMainPoints">
                                  1. For Online Form Fee Payment, you should confirm payment amount and proceed further to make payment through Internet Banking/Debit Card/Credit Card. Please make sure you know your Net banking USER ID and PASSWORD. Banks may differ as bank selection will happen at payment gateway.
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify" class="clsMainPoints">
                                  2. GST is applicable only on Bank Processing Charges.
                                </td>
                            </tr>
                            <tr class="Height5">
                                <td>
                                </td>
                            </tr>         --%>                                  
                        </table>
                    </td>
                </tr>
                <tr id="trNetbankingDetails" runat="server" visible="false">
                    <td align="left" class="borderBtm" colspan="2">
                        <table cellpadding="3" cellspacing="1" width="100%">
                            <tr>
                                <td class="ClslblNetBanking" align="left">
                                    <asp:Label ID="lblNote1" runat="server" Text="
                                For Online Fee Payment, you need to confirm payment amount and proceed further to
                                make payment through Internet Banking. Please make sure you know your Net banking
                                USER ID and PASSWORD. The Internet banking is available for the selected banks/cards only.
                                Here is the list of the same." CssClass="LblUsrNameHead"></asp:Label>
                                    <asp:Label ID="lblNote2" runat="server" Text="
                                For Online Fee Payment, you need to confirm payment amount and proceed further to
                                make payment through Internet Banking. Please make sure you know your Net banking
                                USER ID and PASSWORD. Banks may differ as bank selection will happen at payment gateway."
                                        CssClass="LblUsrNameHead"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                               <td class="ClslblNetBanking" align="left">
                                    <asp:Label ID="Label5" runat="server" Text=" Note :-" CssClass="LblUsrNameHead"></asp:Label>
                                    <asp:Label ID="Label6" runat="server"  Text="The Service Tax is applicable only on Processing Charges." style="color:Red;" CssClass="LblUsrNameHead"></asp:Label>
                               </td>
                            </tr>
                            <tr id="trBankLabel" runat="server" visible="False" >
                                <td align="left">
                                    <span class="ClsLblLgnd" style="font-weight: bold">Bank(s) : </span>
                                </td>
                              
                            </tr>
                            <tr id="trLstReqItems" runat="server" visible="true">
                                <td valign="top" width="100%">
                                    <asp:ListView ID="lstvwBankDetails" runat="server" DataKeyNames="">
                                        <LayoutTemplate>
                                            <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                cellspacing="1" class="GridBorder">
                                                <tr>
                                                    <td>
                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                            cellspacing="1">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" style="width: 5%">
                                                                    No.
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 20%">
                                                                    Bank Name
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 15%">
                                                                    Processing Charges
                                                                </th>
                                                                  <th align="left" style="padding-left: 5px; width: 10%">
                                                                    Service Tax
                                                                </th>
                                                                <th align="center" style="width: 5%">
                                                                    No.
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 20%">
                                                                    Bank Name
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 15%">
                                                                    Processing Charges
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 10%">
                                                                    Service Tax
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                <td align="center" class="ClspaddingMidT">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("OrginalRowNo") %>' CssClass="ClspaddingMidT" />
                                                </td>
                                                <td align="left" style="padding-left: 5px">
                                                    <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("RegisterdBankName") %>' />
                                                </td>
                                                <td align="left" style="padding-left: 5px">
                                                    <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("ProcessingCharge") %>' />
                                                </td>
                                                  
                                                   <td class="paddingLR" align="left" style="padding-left: 5px">
                                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("ServiceTaxInPercentInWord") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("OrginalRowNoSecond") %>' CssClass="ClspaddingMidT" />
                                                </td>
                                                <td align="left" style="padding-left: 5px">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("RegisterdBankNameSecond") %>' />
                                                </td>
                                                <td class="paddingLR" align="left" style="padding-left: 5px">
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("ProcessingChargeSecond") %>' />
                                                </td>
                                                   <td class="paddingLR" align="left" style="padding-left: 5px">
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("ServiceTaxInPercentInWordSecond") %>' />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                          <%--   </tr>
                            </tr>--%>
                          <%--  <tr>
                                
                                    <td class="ClslblNetBanking" align="left">
                                    <asp:Label ID="Label5" runat="server" Text=" Note :- "
                               CssClass="LblUsrNameHead"></asp:Label>
                                    <asp:Label ID="Label6" runat="server" Text="
                                Service Tax calculation is based on processing charges."
                                        CssClass="LblUsrNameHead"></asp:Label>
                                
                                </td>
                            </tr>--%>
                            <tr id="trCardGateway" runat="server">
                                <td align="left">
                                    <span class="ClsLblLgnd" style="font-weight: bold">Card(s) : </span>
                                </td>
                            </tr>
                            <tr id="trCardDetails" runat="server">
                                <td align="left" class="LblUsrNameHead">
                                    <asp:ListView ID="lstvwCardDetails" runat="server" DataKeyNames="">
                                        <LayoutTemplate>
                                            <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                cellspacing="1" class="GridBorder">
                                                <tr>
                                                    <td>
                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                            cellspacing="1">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" class="Clspadding" style="width: 5%">
                                                                    No.
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 20%">
                                                                    Bank Name
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 15%">
                                                                    Processing Charges
                                                                </th>
                                                                 <th align="left" style="padding-left: 5px; width: 10%">
                                                                    Service Tax
                                                                </th>
                                                                <th align="center" class="Clspadding" style="width: 5%">
                                                                    No.
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 20%">
                                                                    Bank Name
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 15%">
                                                                    Processing Charges
                                                                </th>
                                                                 <th align="left" style="padding-left: 5px; width: 10%">
                                                                    Service Tax
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                <td align="center" class="ClspaddingMidT">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("OrginalRowNo") %>' CssClass="ClspaddingMidT" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("RegisterdBankName") %>'
                                                        Style="padding-left: 5px" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("ProcessingCharge") %>'
                                                        Style="padding-left: 5px" />
                                                </td>
                                                 <td class="paddingLR" align="left">
                                                    <asp:Label ID="Label7" runat="server" Text='<%# Eval("ServiceTaxInPercentInWord") %>'
                                                        Style="padding-left: 5px" />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("OrginalRowNoSecond") %>' CssClass="ClspaddingMidT" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("RegisterdBankNameSecond") %>'
                                                        Style="padding-left: 5px" />
                                                </td>
                                                <td class="paddingLR" align="left">
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("ProcessingChargeSecond") %>'
                                                        Style="padding-left: 5px" />
                                                </td>
                                                   <td class="paddingLR" align="left">
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("ServiceTaxInPercentInWordSecond") %>'
                                                        Style="padding-left: 5px" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="height:30px" id="trHeight1" runat="server" visible="false">
                <td>
                </td>
                </tr>
                <tr id="trNextYear" runat="server" visible="false">
                
                 <td align="left">
                                    <span class="ClsLblLgnd" id="spnNextYearLabel" runat="server" >Standard selection for admission application for year 2022-23
                                    </span>
                                </td>
                                </tr>
                 <tr>
                <td align="left" colspan="2">
                <asp:ListView ID="lstvwAdmissionStatusNxtYear" runat="server" ItemPlaceholderID="itemPlaceholder" DataKeyNames="EnableAdmissionFormFee,Standard_Id,Academic_Year_Id,Standard_Name"
                            OnItemDataBound="lstvwAdmissionStatusNxtYear_ItemDataBound" OnItemCommand="lstvwAdmissionStatusNxtYear_ItemCommand"
                            OnDataBound="lstvwAdmissionStatusNxtYear_DataBound">
                            <LayoutTemplate>
                                <table runat="server" id="tblContacts" style="color: #333333; width: 100%; height: 100%"
                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader" style="background-image: url(../images/GridHeaderBGLrg.gif)">
                                        <th align="left" style="padding-left: 10px; width: 10%">
                                            Standard
                                        </th>
                                        <th align="center" style="width: 12%" id="thStartDate" runat="server">
                                            Form Available Date & Time
                                        </th>
                                        <th align="center" style="width: 12%" id="thEndDate" runat="server">
                                            Form Closing Date & Time
                                        </th>
                                        <th align="center" style="width: 12%;" id="thLottaryDate" runat="server">
                                            Admission List Display Date
                                        </th>
                                        <%--<th align="center" style="width: 17%">
                                            Admission Confirmation Last Date
                                        </th>--%>
                                        <th align="center" style="padding-right: 5px; width: 7%" id="thTotalForms" runat="server">
                                            Total Forms
                                        </th>
                                        <th align="right" style="padding-right: 5px; width: 10%" visible="false">
                                            Forms Left
                                        </th>
                                        <th align="center" style="width: 15%" id="thDOBMinLimit" runat="server">
                                            DOB Min. Limit
                                        </th>
                                        <th align="center" style="width: 15%" id="thDOBMaxLimit" runat="server">
                                            DOB Max. Limit
                                        </th>
                                        <th align="center" style="padding-right: 5px; width: 16%">
                                            Submit Form
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="trItem" runat="server" class="ClsGridRow">
                                    <td align="left" style="padding-left: 10px; width: 10%">
                                        <asp:Label runat="server" ID="StdName" Text='<%#Eval("Standard_Name")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdStartDate" runat="server">
                                        <asp:Label runat="server" ID="formOpenDate" Text='<%#Eval("FormOpenDate","{0:dd-MMM-yyyy hh:mm tt}")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdEndDate" runat="server">
                                        <asp:Label runat="server" ID="lblCloseDt" Text='<%#Eval("FormCloseDate","{0:dd-MMM-yyyy hh:mm tt}")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdLottaryDate" runat="server">
                                        <asp:Label runat="server" ID="lblLottoryDate" Text='<%#Eval("LottoryDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                    </td>
                                    <%--   <td align="center">
                                        <asp:Label runat="server" ID="formCloseDate" Text='<%#Eval("AdmissionConfirmLastDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                    </td>--%>
                                    <td align="center" style="padding-right: 5px;" id="tdTotalForms" runat="server">
                                        <asp:Label runat="server" ID="TotalformsCount" Text='<%#Eval("TotalOnlineForms")%>'></asp:Label>
                                    </td>
                                    <td align="right" style="padding-right: 5px;" visible="false">
                                        <asp:Label runat="server" ID="RemainingformsCount" Text='<%#Eval("RemainingformsCount")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdDOBMin" runat="server">
                                        <asp:Label runat="server" ID="lblMinDOB" Text='<%#Eval("DOBMin")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdDOBMax" runat="server">
                                        <asp:Label runat="server" ID="lblMaxDOB" Text='<%#Eval("DOBMax")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdEdit" runat="server">
                                        <asp:LinkButton ID="lnkbtnAdmission" runat="server" Text="Submit Form" ToolTip="Submit Form"
                                            CommandArgument='<%# Eval("Standard_Id")%>' CommandName="Admission" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                    <td align="left" style="padding-left: 10px; width: 10%">
                                        <asp:Label runat="server" ID="StdName" Text='<%#Eval("Standard_Name")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdStartDate" runat="server">
                                        <asp:Label runat="server" ID="formOpenDate" Text='<%#Eval("FormOpenDate","{0:dd-MMM-yyyy hh:mm tt}")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdEndDate" runat="server">
                                        <asp:Label runat="server" ID="lblCloseDt" Text='<%#Eval("FormCloseDate","{0:dd-MMM-yyyy hh:mm tt}")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdLottaryDate" runat="server">
                                        <asp:Label runat="server" ID="lblLottoryDate" Text='<%#Eval("LottoryDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                    </td>
                                    <%--    <td align="center">
                                        <asp:Label runat="server" ID="formCloseDate" Text='<%#Eval("AdmissionConfirmLastDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                    </td>--%>
                                    <td align="center" style="padding-right: 5px;" id="tdTotalForms" runat="server">
                                        <asp:Label runat="server" ID="TotalformsCount" Text='<%#Eval("TotalOnlineForms")%>'></asp:Label>
                                    </td>
                                    <td align="right" style="padding-right: 5px;" visible="false">
                                        <asp:Label runat="server" ID="RemainingformsCount" Text='<%#Eval("RemainingformsCount")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdDOBMin" runat="server">
                                        <asp:Label runat="server" ID="lblMinDOB" Text='<%#Eval("DOBMin")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdDOBMax" runat="server">
                                        <asp:Label runat="server" ID="lblMaxDOB" Text='<%#Eval("DOBMax")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdEdit" runat="server">
                                        <asp:LinkButton ID="lnkbtnAdmission" runat="server" Text="Submit Form" ToolTip="Submit Form"
                                            CommandArgument='<%# Eval("Standard_Id")%>' CommandName="Admission" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                </td>
              </tr>
                <tr style="height:20px" id="trHeight2" runat="server" visible="false">
                <td>
                </td>
                </tr>
                <tr id="trOldStandardRow" runat="server">
                    <td align="left" style="font-weight: bold">
                        <asp:Label ID="lblStandardList" CssClass="ClsLblLgnd" runat="server" ViewStateMode="Enabled"></asp:Label>                         
                    </td>
                </tr>
                <tr id="trOldStandardListview" runat="server">
                    <td align="left" colspan="2">
                        <asp:ListView ID="lstvwAdmissionStatus" runat="server" ItemPlaceholderID="itemPlaceholder" DataKeyNames="EnableAdmissionFormFee,Standard_Id,Academic_Year_Id,Standard_Name"
                            OnItemDataBound="lstvwAdmissionStatus_ItemDataBound" OnItemCommand="lstvwAdmissionStatus_ItemCommand"
                            OnDataBound="lstvwAdmissionStatus_DataBound">
                            <LayoutTemplate>
                                <table runat="server" id="tblContacts" style="color: #333333; width: 100%; height: 100%"
                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader" style="background-image: url(../images/GridHeaderBGLrg.gif)">
                                        <th align="left" style="padding-left: 10px; width: 10%">
                                            Standard
                                        </th>
                                        <th align="center" style="width: 12%" id="thStartDate" runat="server">
                                            Form Available Date & Time
                                        </th>
                                        <th align="center" style="width: 12%" id="thEndDate" runat="server">
                                            Form Closing Date & Time
                                        </th>
                                        <th align="center" style="width: 12%;" id="thLottaryDate" runat="server">
                                            Admission List Display Date
                                        </th>
                                        <%--<th align="center" style="width: 17%">
                                            Admission Confirmation Last Date
                                        </th>--%>
                                        <th align="center" style="padding-right: 5px; width: 7%" id="thTotalForms" runat="server">
                                            Total Forms
                                        </th>
                                        <th align="right" style="padding-right: 5px; width: 10%" visible="false">
                                            Forms Left
                                        </th>
                                        <th align="center" style="width: 15%" id="thDOBMinLimit" runat="server">
                                            DOB Min. Limit
                                        </th>
                                        <th align="center" style="width: 15%" id="thDOBMaxLimit" runat="server">
                                            DOB Max. Limit
                                        </th>
                                        <th align="center" style="padding-right: 5px; width: 16%">
                                            Submit Form
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="trItem" runat="server" class="ClsGridRow">
                                    <td align="left" style="padding-left: 10px; width: 10%">
                                        <asp:Label runat="server" ID="StdName" Text='<%#Eval("Standard_Name")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdStartDate" runat="server">
                                        <asp:Label runat="server" ID="formOpenDate" Text='<%#Eval("FormOpenDate","{0:dd-MMM-yyyy hh:mm tt}")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdEndDate" runat="server">
                                        <asp:Label runat="server" ID="lblCloseDt" Text='<%#Eval("FormCloseDate","{0:dd-MMM-yyyy hh:mm tt}")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdLottaryDate" runat="server">
                                        <asp:Label runat="server" ID="lblLottoryDate" Text='<%#Eval("LottoryDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                    </td>
                                    <%--   <td align="center">
                                        <asp:Label runat="server" ID="formCloseDate" Text='<%#Eval("AdmissionConfirmLastDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                    </td>--%>
                                    <td align="center" style="padding-right: 5px;" id="tdTotalForms" runat="server">
                                        <asp:Label runat="server" ID="TotalformsCount" Text='<%#Eval("TotalOnlineForms")%>'></asp:Label>
                                    </td>
                                    <td align="right" style="padding-right: 5px;" visible="false">
                                        <asp:Label runat="server" ID="RemainingformsCount" Text='<%#Eval("RemainingformsCount")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdDOBMin" runat="server">
                                        <asp:Label runat="server" ID="lblMinDOB" Text='<%#Eval("DOBMin")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdDOBMax" runat="server">
                                        <asp:Label runat="server" ID="lblMaxDOB" Text='<%#Eval("DOBMax")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdEdit" runat="server">
                                        <asp:LinkButton ID="lnkbtnAdmission" runat="server" Text="Submit Form" ToolTip="Submit Form"
                                            CommandArgument='<%# Eval("Standard_Id")%>' CommandName="Admission" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                    <td align="left" style="padding-left: 10px; width: 10%">
                                        <asp:Label runat="server" ID="StdName" Text='<%#Eval("Standard_Name")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdStartDate" runat="server">
                                        <asp:Label runat="server" ID="formOpenDate" Text='<%#Eval("FormOpenDate","{0:dd-MMM-yyyy hh:mm tt}")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdEndDate" runat="server">
                                        <asp:Label runat="server" ID="lblCloseDt" Text='<%#Eval("FormCloseDate","{0:dd-MMM-yyyy hh:mm tt}")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdLottaryDate" runat="server">
                                        <asp:Label runat="server" ID="lblLottoryDate" Text='<%#Eval("LottoryDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                    </td>
                                    <%--    <td align="center">
                                        <asp:Label runat="server" ID="formCloseDate" Text='<%#Eval("AdmissionConfirmLastDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                    </td>--%>
                                    <td align="center" style="padding-right: 5px;" id="tdTotalForms" runat="server">
                                        <asp:Label runat="server" ID="TotalformsCount" Text='<%#Eval("TotalOnlineForms")%>'></asp:Label>
                                    </td>
                                    <td align="right" style="padding-right: 5px;" visible="false">
                                        <asp:Label runat="server" ID="RemainingformsCount" Text='<%#Eval("RemainingformsCount")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdDOBMin" runat="server">
                                        <asp:Label runat="server" ID="lblMinDOB" Text='<%#Eval("DOBMin")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdDOBMax" runat="server">
                                        <asp:Label runat="server" ID="lblMaxDOB" Text='<%#Eval("DOBMax")%>'></asp:Label>
                                    </td>
                                    <td align="center" id="tdEdit" runat="server">
                                        <asp:LinkButton ID="lnkbtnAdmission" runat="server" Text="Submit Form" ToolTip="Submit Form"
                                            CommandArgument='<%# Eval("Standard_Id")%>' CommandName="Admission" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>


                        
                    </td>
                </tr>
                <tr>
                    <td>
                            <asp:HiddenField ID="hidAcademicYearForOnlineAdmission" runat="server" />
                            <asp:HiddenField ID="hidStandardName" runat="server" />

                    </td>
                </tr>
            </table>
            <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
            </b></b>
        </div>
        <br />
    </div>
    <script language="javascript" type="text/javascript">
        var _clientTdCongrates = "<%= this.tdCongrates.ClientID %>"
        _ClienthidAcademicYearForOnlineAdmission = "<%= this.hidAcademicYearForOnlineAdmission.ClientID %>"

        var year = document.getElementById(_ClienthidAcademicYearForOnlineAdmission).value;
        if (document.getElementById('OnlineAdmissionText') != null) {
            if (year != "")
                document.getElementById('OnlineAdmissionText').innerHTML = "Online Admission For The Year " + year;
            else
                document.getElementById('OnlineAdmissionText').innerHTML = "Online Admission";
        }

        function openGuidlinesDetails() {
            window.open('Guidelines.aspx', '_self', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=800,height=950')
        }
        function blinkIt() {
            s = document.getElementById(_clientTdCongrates)
            if (s != null)
                s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible'
        }
        function MessageAboutSave(aSrc) {
            var bIsValid;
            if (alert(aSrc))
                bIsValid = false;
            return bIsValid;
        }
        function AdmissionAlertMessage(aStdId, iSchoolID) {
            if (iSchoolID == 11) {
                if (aStdId == 990)
                    alert("Online admission is not applicable for this class. Please visit school for 9th standard admission.");
                if (aStdId == 991)
                    alert("Online admission is not applicable for this class. Please visit school for 10th standard admission.");
                return false;
            }
            return true;        
        }
    </script>
</asp:Content>
