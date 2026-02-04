<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" AutoEventWireup="true" CodeFile="TermsOfUse.aspx.cs" Inherits="TermsOfUse"  %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script language="javascript" type="text/javascript">
    _clientbtnContinueId = "<%=this.btnContinue.ClientID%>";
    _clientrdoAcceptId = "<%=this.rdoAccept.ClientID %>";
    _clientrdoNoAcceptId = "<%=this.rdoNoAccept.ClientID %>";
        //This function is used to close window.
        function closewindow()
        {
        window.close();
        }
        //This function is used to disable button.
        function enabledisablecontrols(btn) {            
            if (btn == _clientrdoAcceptId)
                document.getElementById( _clientbtnContinueId).disabled = false;
            else 
                document.getElementById( _clientbtnContinueId).disabled = true;
        }
    </script>

    <div style="width: 97%;" align="center" >
        <div align="center">
            <!-- Data Insert Here -->
                <div align="center">
                <br />
                    <div id="nifty" align="center">
                        <div class="paddingLR" style="padding-top:1px;" align="center">
                            <div class="HeadTxtB borderBtm admissiondivstyle" align="center">
                                Terms of Use&nbsp;
                            </div>
                            <div class="TxtNormal" align="left" style="padding:1px 0 0 2px;">
                                Pawar Public School(PPS) provides web based educational software and services. This End User Licensing
                                Agreement ("EULA") sets forth the terms and conditions of your use of these software
                                and services.
                                <br />
                                <br />
                                PPS may modify this EULA from time to time and will post a modified version
                                of the EULA on this web site. Modified versions of this EULA shall be effective
                                upon posting by PPS. You agree to be bound to any changes to this EULA by using
                                the software and services after any such modification is posted. Accordingly, you
                                must review this EULA regularly to ensure that you are aware of the current terms
                                and conditions.
                                <br />
                                <br />
                                You must agree to keep your PPS account password secret and known only to yourself. At the sole
                                discretion of SV your account may be suspended or deleted without notice and
                                any customization may be lost. Subject to the terms and restrictions set forth in
                                this EULA, PPS grants you a non-exclusive, non-transferable license, to use
                                the software and access the services solely for the educational benefit of school
                                students. You may not use, copy, modify, or transfer the software, in whole or in
                                part, or use the services except as expressly provided in this EULA. Except for
                                the foregoing license grant, this EULA does not grant you any rights to patents,
                                copyrights, trade secrets, trademarks, source code, or any other rights in respect
                                to the software or services.
                                <br />
                                <br />
                                No guarantees are provided that this web site will be available for use on any particular
                                day or time or that site data will be accessible.
                                <br />
                                <br />
                                You may not reverse engineer, disassemble, decompile, modify or translate the software,
                                or otherwise attempt to derive the source code of the software, or authorize any
                                third party to do any of the foregoing. The software is licensed, not sold, to you
                                for use only under the terms of this EULA, and PPS reserves all rights not expressly
                                granted to you.
                                <br />
								<div id="divOnline" runat="server">
									<div class="HeadTxtB borderBtm admissiondivstyle" align="center" style="font-size:10pt; margin-top:10px" >The following are the Terms and Conditions for online Payments. Please go through the same carefully.</div>
									<div class="TxtNormal paddingL">
									<ol  style="text-align:left; margin-top:12px; margin-bottom:1px;">
                                        <li> I / We agree and accept the services provided by Axis Bank and Aggregator. At my / our request to carry out my payments on my/our account, given by me / us.</li>
										<li>I / We have no objection whatsoever, to the online payment facility providing my / our billing details to the Institute.</li>
										<li>While the Institute will take all reasonable  steps to ensure the accuracy of the payment details, the Institute is not liable for any error. I / We shall not hold the Institute responsible for any  loss, damages, etc. that may be incurred / suffered by me / us if the information contained turns out to be inaccurate / incorrect.</li>
										<li>I / We agree that any disputes on Payment details will be settled directly with the Institute and the responsibility limited to provision of information only.</li>
										<li>I / We agree that we will make bill amount  payments as required by the Institute. I / We will not hold the Institute  responsible for rejecting the payment amount because of incorrect or incomplete  entries.</li>
										<li>I / We agree that the record of the instructions given and transactions with the Institute shall be conclusive proof and binding for all purposes and can be used as evidence in any  proceeding.</li>
										<li>I / We agree that charges, if any, for the online payment services will be at the sole discretion of the Institute is at  liberty to vary the same from time to time, without giving any notice.</li>
										<li>I / We agree that the Institute is at liberty to  withdraw at anytime the online payment facility, or any services provided there  under, in respect of any or all the account(s) without assigning any reason  whatsoever, without giving me / us any notice.</li>
									</ol>
									</div>
								</div>
                            </div>
                            <div runat="server" id ="trline" class="borderBtm" align="left" ></div>
                            <div runat="server" id ="trBtns">
                                <div class="borderBtm" align="left">
                                    <div class="TxtNormal" align="left">
                                        <asp:RadioButton runat="server" ID="rdoAccept" GroupName="terms" Text="I accept the Terms of Use" />
                                    </div>
                                    <div class="TxtNormal" align="left">
                                        <asp:RadioButton runat="server" ID="rdoNoAccept" Checked="true" GroupName="terms"
                                            Text="I do not accept the Terms of Use" />
                                    </div>
                                    <div class="TxtNormal" align="left">
                                        <asp:Button runat="server" ID="btnContinue" Enabled="false" CssClass="btnMid" Text="Continue"
                                            OnClick="btnContinue_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    <br />
                    </div>
                </div>
        </div>
    </div>
<script language="javascript" type="text/javascript">
        window.focus();
        
        function fnover(varname)
        {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "maroon";
            objTXT.style.backgroundImage = "url(images/BtnBGRollNew.jpg)";
            //objTXT.style.color = "maroon";
        }

        function fnout(varname)
        {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "#a3c07b";
            objTXT.style.backgroundImage = "url(images/BtnBG.jpg)";
            //objTXT.style.color = "Black";
        }
        
</script>
</asp:Content>

