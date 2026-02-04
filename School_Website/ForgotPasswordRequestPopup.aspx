<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMaster.master" AutoEventWireup="true"
    EnableViewState="false" CodeFile="ForgotPasswordRequestPopup.aspx.cs" Inherits="ForgotPasswordRequestPopup"
    EnableSessionState="False" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .ClsHilightTextB
        {
            padding-left: 2px;
            padding-right: 2px;
            padding-top: 4px;
            padding-bottom: 4px;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="ClsFPasswordPopupBody" style="min-height: 520px">
        <asp:UpdatePanel ID="upnl" runat="server">
            <ContentTemplate>
                <table style="width: 100%" cellpadding="0" cellspacing="1" class="ClsFPasswordPopupName">
                    <tr>
                        <td align="left">
                            <table class="HeadTxtB borderBtm admissiondivstyle" style="width: 100%;">
                                <tr>
                                    <td>
                                        <span style="font-weight: bold">Change Mobile Number </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trMandatory" runat="server">
                        <td align="right">
                            <span class="ClsMdtStar">* Mandatory Fields</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:ValidationSummary ID="valSumError" runat="server" HeaderText="Please correct following errors."
                                ShowMessageBox="False" ShowSummary="True" />
                        </td>
                    </tr>
                    <tr id="trError">
                        <td align="center" style="height: 40px">
                            <asp:UpdatePanel ID="upnl1" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                                        ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                                    <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="False" ForeColor="Red"
                                        Visible="false"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="50%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td class="MyClsBorderlight ClsHilightTextB" width="20%">
                                        <span class="ClsLabel" style="white-space: nowrap">Select Role :</span>
                                    </td>
                                    <td style="white-space: nowrap" align="left">
                                        <asp:RadioButtonList ID="rdolstRole" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="false" Onclick="ChangeLabel()">
                                            <asp:ListItem Text="Student" Selected="True" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="MyClsBorderlight ClsHilightTextB" width="20%">
                                        <span class="ClsLabel" style="white-space: nowrap">Name :</span>
                                    </td>
                                    <td style="white-space: nowrap" align="left">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="ClsTxtLarge" Width="250px" MaxLength="100"
                                            TabIndex="1"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:RequiredFieldValidator ID="reqvalStopName" runat="server" ControlToValidate="txtName"
                                            ErrorMessage="Name should not be blank." Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="MyClsBorderlight ClsHilightTextB">
                                        <span class="ClsLabel" style="white-space: nowrap">Date Of Birth :</span>
                                    </td>
                                    <td style="white-space: nowrap" align="left">
                                        <asp:TextBox ID="txtBirhtDate" runat="server" CssClass="ClsTxtLarge" Width="100px"
                                            MaxLength="11" TabIndex="2"></asp:TextBox>
                                        <rjs:PopCalendar ID="calendar_DOB" runat="server" Control="txtBirhtDate" Format="dd MMM yyyy"
                                            To-Message="Please select valid Date of Birth." From-Message="Please select valid Date Of Birth."
                                            To-Today="true" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="Please select valid Date Of Birth." />
                                        <span class="ClsMdtStar">*</span>
                                        <asp:CustomValidator ID="cst_DOB" runat="server" Visible="true" ValidateEmptyText="false"
                                            EnableClientScript="true" ClientValidationFunction="DOBValidation" Display="None"> </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="MyClsBorderlight ClsHilightTextB">
                                        <asp:Label ID="lblRegNo" runat="server" class="ClsLabel" Style="white-space: nowrap">Registration No :</asp:Label>
                                    </td>
                                    <td style="white-space: nowrap" align="left">
                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="ClsTxtLarge" TextMode="SingleLine"
                                            TabIndex="4" Width="210px" MaxLength="30"></asp:TextBox>
                                        <span class="ClsMdtStar" id="spanEmpNo" runat="server">*</span>
                                        <%--<asp:RequiredFieldValidator ID="reqvalAddress" Style="vertical-align: top;" runat="server"
                                            Display="None" ControlToValidate="txtRegNo" ErrorMessage="Reg.No / Emp. No should not be blank."> </asp:RequiredFieldValidator>--%>
                                        <asp:CustomValidator ID="cstvalRegNo" runat="server" ClientValidationFunction="RegNoValidation"
                                            ControlToValidate="txtRegNo" EnableClientScript="true" Display="None" ValidateEmptyText="true"> </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="MyClsBorderlight ClsHilightTextB">
                                        <span class="ClsLabel" style="white-space: nowrap">E-mail :</span>
                                    </td>
                                    <td style="white-space: nowrap" align="left">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="ClsTxtLarge" Width="210px" MaxLength="40"
                                            TabIndex="5"></asp:TextBox>                                        
                                        <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation"
                                            ControlToValidate="txtEmail" EnableClientScript="true" Display="None" ValidateEmptyText="true"> </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="MyClsBorderlight ClsHilightTextB">
                                        <span class="ClsLabel" style="white-space: nowrap">Old Mobile Number :</span>
                                    </td>
                                    <td style="white-space: nowrap" align="left">
                                        <asp:TextBox ID="txtMobileNo" runat="server" Width="100px" CssClass="ClsTxtLarge"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" TabIndex="6"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="10"> </asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:CustomValidator ID="cstMobileNumber" runat="server" ClientValidationFunction="MobileNumberValidation"
                                            Display="None" ValidateEmptyText="false" Visible="true" EnableClientScript="true"> </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="MyClsBorderlight ClsHilightTextB">
                                        <span class="ClsLabel" style="white-space: nowrap">New Mobile Number :</span>
                                    </td>
                                    <td style="white-space: nowrap" align="left">
                                        <asp:TextBox ID="txtNewMobile" runat="server" Width="100px" CssClass="ClsTxtLarge"
                                            TabIndex="7" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" MaxLength="10"> </asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:CustomValidator ID="cstNewMobile" runat="server" ClientValidationFunction="NewMobileNumberValidation"
                                            Display="None" ValidateEmptyText="false" Visible="true" EnableClientScript="true"> </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="MyClsBorderlight ClsHilightTextB" >
                                        <span class="ClsLabel" style="white-space: nowrap">Verification Code:</span>
                                    </td>
                                    <td style="white-space: nowrap" align="left">
                                        <asp:TextBox ID="txtVerificationCode" runat="server" CssClass="MidTxtBox" Width="210px" TabIndex="8"
                                            autocomplete="off"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:CustomValidator ID="cstVerificationCode" runat="server" ControlToValidate="txtVerificationCode"
                                            Visible="true" EnableClientScript="true" ValidateEmptyText="true" ClientValidationFunction="VerificationCodeValidation"
                                            Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="text-align: left;" class="ClsMdtStar">
                                        <div style="float: left;">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                                        CustomValidatorErrorMessage="The verification code you typed does not match the code in the image."
                                                        CaptchaHeight="60" CaptchaWidth="210" CaptchaLineNoise="None" FontColor="#529E00"
                                                        CaptchaMaxTimeout="300" Width="210px" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="imgbtnRefresh" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div style="float: left; vertical-align: middle; padding-top: 20px; padding-right: 5px;
                                            padding-left: 15px;">
                                            <asp:ImageButton runat="server" ImageUrl="images/refresh_icon.png" ID="imgbtnRefresh"
                                                CausesValidation="false" ToolTip="Refresh the verification code" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" disable-page="true"
                                CausesValidation="true" TabIndex="8" UseSubmitBehavior="false" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="ClsBtn" CausesValidation="false"
                                TabIndex="9" OnClick="btnClear_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false"
                                TabIndex="10" UseSubmitBehavior="false" OnClientClick="window.close(); return false;" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript" src="../Scripts/jquery-1.7.min.js"></script>
    <script language="javascript" type="text/javascript">
        _clientcal_DOB = "<%=this.txtBirhtDate.ClientID %>";
        _clientcst_DOB = "<%=this.cst_DOB.ClientID %>";
        _clientcst_MobileNumber = "<%=this.cstMobileNumber.ClientID%>";
        _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>";
        _clienttxtEmailId = "<%=this.txtEmail.ClientID %>";
        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobileNo.ClientID %>"
        _clienttxtNewMobile = "<%=this.txtNewMobile.ClientID %>";
        _clientlblRegNo = "<%=this.lblRegNo.ClientID %>";
        _clientlblUpdateMessage = "<%=this.lblUpdateMessage.ClientID %>";
        _clientlblError = "<%=this.lblError.ClientID %>";
        _clientbtnSubmit = "<%=this.btnSubmit.ClientID %>";
        _clientbtnClear = "<%=this.btnClear.ClientID %>";
        _clientbtnClose = "<%=this.btnClose.ClientID %>";
        _sClienttxtVerificationCode = "<%=this.txtVerificationCode.ClientID %>";
        _clientspanEmpNo = "<%=this.spanEmpNo.ClientID %>";
        _clienttxtRegNo = "<%=this.txtRegNo.ClientID %>";

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(beginRequestHandler)
        prm.add_endRequest(EndReqHandler);

        function beginRequestHandler(sender, args) {
            if ($get(_clientbtnSubmit) != null)
                $get(_clientbtnSubmit).disabled = true;
            if ($get(_clientbtnClear) != null)
                $get(_clientbtnClear).disabled = true;
            if ($get(_clientbtnClose) != null)
                $get(_clientbtnSubmit).disabled = true;
        }

        function EndReqHandler(sender, args) {
            if ($get(_clientbtnSubmit) != null)
                $get(_clientbtnSubmit).disabled = false;
            if ($get(_clientbtnClear) != null)
                $get(_clientbtnClear).disabled = false;
            if ($get(_clientbtnClose) != null)
                $get(_clientbtnSubmit).disabled = false;
        }

        //Verification Code Validation
        function VerificationCodeValidation(oSrc, args) {
            var _txtVerificationCode = $get(_sClienttxtVerificationCode).value;
            if (_txtVerificationCode == "") {
                if ($get(_clientlblError) != null) {
                    $get(_clientlblError).style.display = "none";
                }
                oSrc.errormessage = "Verification Code should not be blank.";                
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        //To Set the successful label message to blank
        function ResetUpdateLbl() {
            if ($get(_clientlblUpdateMessage) != null) {
                $get(_clientlblUpdateMessage).innerHTML = "";
            }
        }

        function IsValidDate(date) {
            if (typeof (date) == 'string')
                date = new Date(date);
            return !(date == 'Invalid Date' || date == 'NaN' || date.getFullYear() < 1900);
        }

        //Give Effect to button on Mouse Over
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#BB4A00"
            objTXT.style.backgroundImage = "url('images/BtnBGRollNew.jpg')"
        }

        //Give Effect to button on Mouse Out
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#008000"
            objTXT.style.backgroundImage = "url('images/BtnBG.jpg')"
        }

        function ChangeLabel() {
            var RB1 = document.getElementById("<%=rdolstRole.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            var label = RB1.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    if (label[i].innerHTML == "Student") {
                        $get(_clientlblRegNo).innerHTML = "Registration No :";
                        $get(_clientspanEmpNo).style.display = '';
                    }
                    if (label[i].innerHTML == "Staff") {
                        $get(_clientlblRegNo).innerHTML = "Employee No :";
                        $get(_clientspanEmpNo).style.display = 'none';
                    }
                }
            }
        }

        //Date of Birth Validation
        function DOBValidation(oSrc, args) {
            ResetUpdateLbl();
            var txtDate = $get(_clientcal_DOB);
            args.IsValid = true;

            if (txtDate.value.trim() == '') {
                args.IsValid = false;
                oSrc.errormessage = 'Date Of Birth should not be blank.';
            }
            else {
                var dtToday = new Date();
                var dtStartDate = new Date(txtDate.value.replace(/-/g, ' '));

                if (!IsValidDate(dtStartDate)) {
                    args.IsValid = false;
                    oSrc.errormessage = 'Date Of Birth should be in valid format.';
                }
            }
            return !args.IsValid;
        }

        //Mobile number validation 
        function MobileNumberValidation(oSrc, args) {
            ResetUpdateLbl();
            var sMobileNumber = $get(_sClienttxtMobilePhoneNumberId).value;
            sMobileNumber = sMobileNumber.replace(" ", "");

            if (sMobileNumber == "") {
                oSrc.errormessage = "Old Mobile Number should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                oSrc.errormessage = "Old Mobile Number should not start with zero.";
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber.length > 0 && sMobileNumber.length < 10) {
                oSrc.errormessage = "Old Mobile Number should be 10 digits.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
        }

        function NewMobileNumberValidation(oSrc, args) {
            ResetUpdateLbl();
            var sMobileNumber = $get(_clienttxtNewMobile).value;
            sMobileNumber = sMobileNumber.replace(" ", "");

            var sMobileNumber1 = $get(_sClienttxtMobilePhoneNumberId).value;

            if (sMobileNumber == "") {
                oSrc.errormessage = "New Mobile Number should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                oSrc.errormessage = "New Mobile number should not start with zero.";
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber.length > 0 && sMobileNumber.length < 10) {
                oSrc.errormessage = "New Mobile Number should be 10 digits.";
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber == sMobileNumber1) {
                oSrc.errormessage = "Old Mobile Number & New Mobile Number should not be same.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        //Email Address Validation
        function EmailValidation(oSrc, args) {
            ResetUpdateLbl();
            var sEmail = document.getElementById(_clienttxtEmailId).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);
            
            // If email is not blank then validate for valid email address.
            document.getElementById(_clientcstValEmailId).errormessage = ""
            if (!isEmpty(sEmail) && !isEmail(sEmail) && sEmail != "") {
                document.getElementById(_clientcstValEmailId).errormessage = "E-mail should be in valid format (For Example :\"john.smith@yahoo.com\").";
                document.getElementById(_clientcstValEmailId).innerHTML = "E-mail should be in valid format (For Example :\"john.smith@yahoo.com\").";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function RegNoValidation(oSrc, args) {
            ResetUpdateLbl();

            var RB1 = document.getElementById("<%=rdolstRole.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            var label = RB1.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked && label[i].innerHTML == "Student") {
                    if ($get(_clienttxtRegNo).value.trim() == "") {
                            oSrc.errormessage = "Registration No should not be blank.";
                            args.IsValid = false;
                            return true;
                    }                    
                }
            }

            args.IsValid = true;
            return false;
        }

        function isEmail(emailStr) {
            ResetUpdateLbl();
            var emailPat = /^(.+)@(.+)$/
            var specialChars = "\\(\\)<>@,;:\\\\\\\"\\.\\[\\]"
            var validChars = "\[^\\s" + specialChars + "\]"
            var quotedUser = "(\"[^\"]*\")"
            var ipDomainPat = /^\[(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\]$/
            var atom = validChars + '+'
            var word = "(" + atom + "|" + quotedUser + ")"
            var userPat = new RegExp("^" + word + "(\\." + word + ")*$")
            var domainPat = new RegExp("^" + atom + "(\\." + atom + ")*$")
            var matchArray = emailStr.match(emailPat)
            if (matchArray == null) {
                return false
            }
            var user = matchArray[1]
            var domain = matchArray[2]

            if (user.match(userPat) == null) {
                return false
            }

            var IPArray = domain.match(ipDomainPat)
            if (IPArray != null) {
                for (var i = 1; i <= 4; i++) {
                    if (IPArray[i] > 255) {
                        return false
                    }
                }
                return true
            }

            // Domain is symbolic name
            var domainArray = domain.match(domainPat)
            if (domainArray == null) {
                return false
            }

            var atomPat = new RegExp(atom, "g")
            var domArr = domain.match(atomPat)
            var len = domArr.length
            if (domArr[domArr.length - 1].length < 2 ||
                domArr[domArr.length - 1].length > 3) {
                return false
            }

            // Make sure there's a host name preceding the domain.
            if (len < 2) {
                var errStr = "This address is missing a hostname!"
                return false
            }

            // If we've gotten this far, everything's valid!
            return true;
        }
                 
    </script>
</asp:Content>
