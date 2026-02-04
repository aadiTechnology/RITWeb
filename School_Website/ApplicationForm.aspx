<%@ Page Language="C#" MasterPageFile="~/PPSMaster.master" AutoEventWireup="true" CodeFile="ApplicationForm.aspx.cs" Inherits="ApplicationForm"
Title="PPS Careers - Join Us for your bright future" EnableViewState="false" EnableSessionState="False" %>

<%@ Register Assembly= "MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">   
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

       <div style="width: 90%">
        <br />
        <div id="nifty">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
            </b></b>
            <div>
                <div class="HeadTxtB borderBtm admissiondivstyle">
                    Careers
                </div>

                    <div style="float:left;margin:48px 0px 0px 70px ">
                         <img src="images/Carrer.png"   />                             
                   </div>
                <div class="TxtNormalPadding10 topLeftAlignemnt">
        <center style="padding-left:10%;">
        
            <table style="text-align:left;" >
                <tr>
                    <td colspan="2"align="right" style="padding-right: 10px;">
                         <span class="ClsMdtStar">*&nbsp; Mandatory Fields</span>                         
                    </td>
                </tr>
                <tr >
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <contenttemplate>
                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                        </contenttemplate>
                        </asp:UpdatePanel>
                    </td>
              </tr>
                <tr>
                <td>
                <div>
                <table>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" runat="server" Text="Label" Visible="False" 
                            ForeColor="Red">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblSuccessful" CssClass="ClsLabel" ForeColor="Blue" runat="server" Text="Label" Visible="false" style="font-weight:bold;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span style="width: 140px">Name:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtName" runat="server" CssClass="MidTxtBox" Width="210px" MaxLength="100"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                        <asp:RequiredFieldValidator ID="reqvalStopName" runat="server" ControlToValidate="txtName" ErrorMessage="Name should not be blank." Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span style="width: 140px">Date of Birth:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="cal_DOB" runat="server" CssClass="MidTxtBox" Width="210px"></asp:TextBox>
                        
                        <rjs:PopCalendar ID="calendar_DOB" runat="server" Control="cal_DOB" Format="dd MMM yyyy"
                                                    To-Message="Please select valid Date of Birth." From-Message="Please select valid Date of Birth."
                                                    To-Today="true" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="Please select valid Date of Birth." />
                                                    <span class="ClsMdtStar">*</span>
                        <asp:CustomValidator ID="cst_DOB" runat="server" Visible="true" ValidateEmptyText="false" EnableClientScript="true" ClientValidationFunction="DOBValidation" Display="None">
                        </asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="req_DateOfBirth" runat="server" ControlToValidate="cal_DOB" ErrorMessage="Date of birth should not be blank." Display="None">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span class="productdemo_text">Address:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="MidTxtBox" TextMode="MultiLine" Width="210px" Height="50px" MaxLength="300"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                         <asp:RequiredFieldValidator ID="reqvalAddress" style="vertical-align:top;" runat="server" Display="None" ControlToValidate="txtAddress" ErrorMessage="Address should not be blank." >
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="cst_Remark" style="vertical-align:top;" runat="server" Display="None" ControlToValidate="txtAddress" ErrorMessage="Address should be less than 300 characters." ValidationExpression="^[\s\S]{0,300}$">
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span class="productdemo_text">E-mail:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="MidTxtBox" Width="210px" MaxLength="40"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                        <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation" ControlToValidate="txtEmail" EnableClientScript="true" Display="None" ValidateEmptyText="true">
                        </asp:CustomValidator>
                    </td>                                
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span class="productdemo_text">Mobile Number:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtMobileNo" runat="server" Width="210px" CssClass="MidTxtBox" onblur="extractNumber(this,0,false);"
                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="10">
                        </asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                        <asp:CustomValidator ID="cstMobileNumber" runat="server"
                            ClientValidationFunction="MobileNumberValidation" Display="None" ValidateEmptyText="false" Visible="true" EnableClientScript="true">
                        </asp:CustomValidator>
                          <asp:RequiredFieldValidator ID="reqvalMobileNo" runat="server" Display="None" ControlToValidate="txtMobileNo" ErrorMessage="Mobile number should not be blank.">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span class="productdemo_text">Years of Experience:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtYearOfExperience" runat="server" CssClass="MidTxtBox" onblur="extractNumber(this,1,false);" Width="210px"
                            onkeyup="extractNumber(this,1,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="4">
                        </asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                        <asp:CustomValidator ID="cstExperienceValidation" runat="server" ClientValidationFunction="ExperienceValidation" Display="None" ValidateEmptyText="false" Visible="true" EnableClientScript="true">
                        </asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="reqvalYearOfExperience" runat="server" Display="None" ControlToValidate="txtYearOfExperience" ErrorMessage="Years of experience should not be blank.">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td  class="ClsBorderlight" style="padding-left:5px;">
                        <span class="productdemo_text">Position Applied For:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtPosition" runat="server" CssClass="MidTxtBox" MaxLength="100" Width="210px"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                        <asp:RequiredFieldValidator ID="reqvalPosition" runat="server" Display="None" ControlToValidate="txtPosition" ErrorMessage="Position applied for should not be blank.">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px; width:200px;">
                        <span class="productdemo_text">Last Organization Worked For:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtLastOrganisation" runat="server" CssClass="MidTxtBox" Width="210px" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span class="productdemo_text">Area of Specialization:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtAreaOfSpecialization" runat="server" CssClass="MidTxtBox" Width="210px" MaxLength="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span class="productdemo_text">Upload Resume:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:FileUpload ID="File_attatchment" runat="server" CssClass="ExLrgTxtBox" Width="215px" />
                        <span class="ClsMdtStar">*</span>
                          <asp:Label ID="lblFileError" runat="server" Text="Label" Visible="False" 
                            ForeColor="Red">
                        </asp:Label>
                        <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                            ControlToValidate="File_attatchment" Display="None" ValidateEmptyText="false" Visible="true" EnableClientScript="true" >
                        </asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="reqvalResume" runat="server" ControlToValidate="File_attatchment" ErrorMessage="Please select the file." Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td> <span class="LblSmlGray">(Supports only .DOC/.DOCX/.PDF files type upto 200 KB)</span> </td>
                </tr>
                <tr>
                    <td class="ClsBorderlight" style="padding-left:5px;">
                        <span class="productdemo_text">Verification Code:</span>
                    </td>
                    <td class="ClsMdtStar">
                        <asp:TextBox ID="txtVerificationCode" runat="server" CssClass="MidTxtBox" Width="210px" autocomplete="off"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                        
                        <asp:CustomValidator ID="cstVerificationCode" runat="server" ControlToValidate ="txtVerificationCode"  
                            Visible="true" EnableClientScript="true" ValidateEmptyText="true"
                            ClientValidationFunction="VerificationCodeValidation" Display="None">
                        </asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td >
                    </td>
                    <td style="text-align:left;" class="ClsMdtStar" >
                    
                    <div style= "float:left;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" 
                            UpdateMode="Conditional">
                    <contenttemplate>
                            <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" 
                                    CaptchaLength="5" CustomValidatorErrorMessage="The verification code you typed does not match the code in the image."
                                    CaptchaHeight="60" CaptchaWidth="210" CaptchaLineNoise="None" 
                                    FontColor="#529E00" CaptchaMaxTimeout="300" Width="210px" />
                                    </contenttemplate>
                                    <triggers>
                                    <asp:AsyncPostBackTrigger ControlID="imgbtnRefresh" EventName="Click"  />
                                </triggers>
                                    
                                    </asp:UpdatePanel>
                                    </div>
                                    <div style="float:left; vertical-align:middle; padding-top:20px; padding-right:5px; padding-left:15px;">
                                     <asp:ImageButton runat="server" ImageUrl="images/refresh_icon.png" ID="imgbtnRefresh"                             
                                          CausesValidation="false" ToolTip="Refresh the verification code"/>
                            </div>                        
                       
                    </td>
                </tr>
                   
                <tr>
             
                    <td></td>
                    <td align="left">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" CausesValidation="true" OnClick="btnSubmit_Click" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="ClsBtn" CausesValidation="False" OnClick="btnClear_Click" />
                    </td>
                </tr> 
                </table>
                </div>
                </td>
                </tr>      
            </table>
        </center>
    </div>
            </div>
            <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
            </b></b>
        </div>
        <br />
    </div>
    <div style="width:90%;">
      <asp:HiddenField runat="server" ID="hidOrgColor" />
      <asp:HiddenField runat="server" ID="hidOrgCSS" />      
    </div>        
    
    <script src="js/validate2.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        _clientcal_DOB = "<%=this.cal_DOB.ClientID %>";
        _clientcst_DOB = "<%=this.cst_DOB.ClientID %>";
        _clientCustomValId = "<%=this.CstValFileType.ClientID%>"
        _clientcst_MobileNumber = "<%=this.cstMobileNumber.ClientID%>";
        _clientcst_Experience = "<%=this.cstExperienceValidation.ClientID%>";
        _clientcstVerificationCode = "<%=this.cstVerificationCode.ClientID%>";
        _clientFileUploadClientId = "<%=this.File_attatchment.ClientID%>"
        _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>";
        _clienttxtEmailId = "<%=this.txtEmail.ClientID %>";
        _clienlblSuccessful = "<%=this.lblSuccessful.ClientID %>";
        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobileNo.ClientID %>"
        _sClienttxtExperienceId = "<%=this.txtYearOfExperience.ClientID %>"
        _sClienttxtVerificationCode = "<%=this.txtVerificationCode.ClientID %>"
        _sClientlblMessage = "<%=this.lblMessage.ClientID %>"
        _clienthidOrgColorId = "<%=this.hidOrgColor.ClientID %>";
        _clienthidOrgCSSId = "<%=this.hidOrgCSS.ClientID %>";        

        //To Set the successful label message to blank
        function ResetUpdateLbl() {
            if (document.getElementById(_clienlblSuccessful) != null) {
                document.getElementById(_clienlblSuccessful).style.display = "none"
            }
        }

        //Give Effect to button on Mouse Over
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#BB4A00"
            objTXT.style.backgroundImage = "url('RITeSchool/images/BtnBGRollNew.jpg')"
        }

        //Give Effect to button on Mouse Out
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#008000"
            objTXT.style.backgroundImage = "url('RITeSchool/images/BtnBG.jpg')"
        }


        function fnTXTFocus(varname) {

            var objTXT = document.getElementById(varname)
            document.getElementById(_clienthidOrgColorId).value = objTXT.style.backgroundColor;

            objTXT.style.backgroundColor = "PapayaWhip";

        }

        function fnTXTLostFocus(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.backgroundColor = document.getElementById(_clienthidOrgColorId).value;

        }

        function fnOnLoad() {
            var t = document.getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < t.length; i++) {
                if (t[i].type == "text" || t[i].type == "password") {
                    if (document.addEventListener) {
                        // as Timo suggested
                        t[i].addEventListener('focus', new Function("fnTXTFocus('" + t[i].id + "')"), true);
                        t[i].addEventListener('blur', new Function("fnTXTLostFocus('" + t[i].id + "')"), true);
                    }
                    else if (document.attachEvent) {
                        // IE style
                        t[i].attachEvent('onfocus', new Function("fnTXTFocus('" + t[i].id + "')"));
                        t[i].attachEvent('onblur', new Function("fnTXTLostFocus('" + t[i].id + "')"));
                    }
                }
                else if (t[i].type == "submit" || t[i].type == "button") {
                    if (document.addEventListener) {
                        t[i].addEventListener('mouseOver', new Function("fnover('" + t[i].id + "')"), true);
                        t[i].addEventListener('mouseOut', new Function("fnout('" + t[i].id + "')"), true);
                    }
                    else {
                        t[i].attachEvent('onmouseover', new Function("fnover('" + t[i].id + "')"));
                        t[i].attachEvent('onmouseout', new Function("fnout('" + t[i].id + "')"));
                    }
                }
            }
            var t = document.getElementsByTagName('textarea');
            for (i = 0; i < t.length; i++) {
                if (document.addEventListener) {
                    // as Timo suggested
                    t[i].addEventListener('focus', new Function("fnTXTFocus('" + t[i].id + "')"), true);
                    t[i].addEventListener('blur', new Function("fnTXTLostFocus('" + t[i].id + "')"), true);
                }
                else if (document.attachEvent) {
                    // IE style
                    t[i].attachEvent('onfocus', new Function("fnTXTFocus('" + t[i].id + "')"));
                    t[i].attachEvent('onblur', new Function("fnTXTLostFocus('" + t[i].id + "')"));
                }
            }

        }

        //Date of Birth Validation
        function DOBValidation(oSrc, args) {
            ResetUpdateLbl()
            var oDOBObj;
            oDOBObj = document.getElementById(_clientcal_DOB).value;

            var sDate;
            if (document.all)
                sDate = new Date(oDOBObj.replace('-', ' '));
            else
                sDate = new Date(convertdate(oDOBObj));

            var today = new Date();
            var DOBYear = parseInt(sDate.getFullYear());
            var thisYear = parseInt(today.getFullYear());

            var yearDiff = thisYear - parseInt(DOBYear);

            var sMinYear = parseInt(thisYear) - 60;
            var sMaxYear = parseInt(thisYear) - 18;

            if (parseInt(yearDiff) > 60) {
                document.getElementById(_clientcst_DOB).errormessage = "Age should  be less than 60 years";
                document.getElementById(_clientcst_DOB).innerHTML = "Age should  be less than 60 years";
                args.IsValid = false;
                return true;
            }
            if (parseInt(yearDiff) < 18) {
                document.getElementById(_clientcst_DOB).errormessage = "Age should  be greater than 18 years.";
                document.getElementById(_clientcst_DOB).innerHTML = "Age should  be greater than 18 years.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        //Verification Code Validation
        function VerificationCodeValidation(oSrc, args) {
            var _txtVerificationCode = document.getElementById(_sClienttxtVerificationCode).value
            if (_txtVerificationCode == "") {
                if (document.getElementById(_sClientlblMessage) != null) {
                    document.getElementById(_sClientlblMessage).style.display = "none"
                }
                document.getElementById(_clientcstVerificationCode).errormessage = "Verification code should not be blank.";
                document.getElementById(_clientcstVerificationCode).innerHTML = "Verification code should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
        }

        //Mobile number validation 
        function MobileNumberValidation(oSrc, args) {
            ResetUpdateLbl()
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value
            sMobileNumber = sMobileNumber.replace(" ", "");
            document.getElementById(_clientcst_MobileNumber).errormessage = ""

            if (sMobileNumber.substring(0, 1) == '0') {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile number should not start with zero.";
                document.getElementById(_clientcst_MobileNumber).innerHTML = "Mobile number should not start with zero.";
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber.length > 0 && sMobileNumber.length < 10) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile number should be 10 digits.";
                document.getElementById(_clientcst_MobileNumber).innerHTML = "Mobile number should be 10 digits.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
        }

        //Year of Experience validation 
        function ExperienceValidation(oSrc, args) {
            ResetUpdateLbl()
            var sYearOfExperience = document.getElementById(_sClienttxtExperienceId).value
            document.getElementById(_clientcst_Experience).errormessage = ""
            if (sYearOfExperience < 0 || sYearOfExperience > 40) {
                document.getElementById(_clientcst_Experience).errormessage = "Years of experience should be between 0 to 40.";
                document.getElementById(_clientcst_Experience).innerHTML = "Years of experience should be between 0 to 40.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
        }

        //Email Address Validation
        function EmailValidation(oSrc, args) {
            ResetUpdateLbl()
            var sEmail = document.getElementById(_clienttxtEmailId).value;           
            sEmail = stripLeadingTrailingBlanks(sEmail);
            if (isEmpty(sEmail)) {
                document.getElementById(_clientcstValEmailId).errormessage = "E-mail should not be blank.";
                args.IsValid = false;
                return true;
            }
           // sEmail = sEmail.replace(" ", "");
            // If email is not blank then validate for valid email address.
            document.getElementById(_clientcstValEmailId).errormessage = ""
            if (!isEmail(sEmail) && sEmail != "") {
                document.getElementById(_clientcstValEmailId).errormessage = "E-mail should be in valid format (For Example :\"john.smith@yahoo.com\").";
                document.getElementById(_clientcstValEmailId).innerHTML = "E-mail should be in valid format (For Example :\"john.smith@yahoo.com\").";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function isEmail(emailStr) {
            ResetUpdateLbl()
            /* The following pattern is used to check if the entered e-mail address
            fits the user@domain format.  It also is used to separate the username
            from the domain. */
            var emailPat = /^(.+)@(.+)$/
            /* The following string represents the pattern for matching all special
            characters.  We don't want to allow special characters in the address. 
            These characters include ( ) < > @ , ; : \ " . [ ]    */
            var specialChars = "\\(\\)<>@,;:\\\\\\\"\\.\\[\\]"
            /* The following string represents the range of characters allowed in a 
            username or domainname.  It really states which chars aren't allowed. */
            var validChars = "\[^\\s" + specialChars + "\]"
            /* The following pattern applies if the "user" is a quoted string (in
            which case, there are no rules about which characters are allowed
            and which aren't; anything goes).  E.g. "jiminy cricket"@disney.com
            is a legal e-mail address. */
            var quotedUser = "(\"[^\"]*\")"
            /* The following pattern applies for domains that are IP addresses,
            rather than symbolic names.  E.g. joe@[123.124.233.4] is a legal
            e-mail address. NOTE: The square brackets are required. */
            var ipDomainPat = /^\[(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})\]$/
            /* The following string represents an atom (basically a series of
            non-special characters.) */
            var atom = validChars + '+'
            /* The following string represents one word in the typical username.
            For example, in john.doe@somewhere.com, john and doe are words.
            Basically, a word is either an atom or quoted string. */
            var word = "(" + atom + "|" + quotedUser + ")"
            // The following pattern describes the structure of the user
            var userPat = new RegExp("^" + word + "(\\." + word + ")*$")
            /* The following pattern describes the structure of a normal symbolic
            domain, as opposed to ipDomainPat, shown above. */
            var domainPat = new RegExp("^" + atom + "(\\." + atom + ")*$")
            /* Finally, let's start trying to figure out if the supplied address is
            valid. */

            /* Begin with the coarse pattern to simply break up user@domain into
            different pieces that are easy to analyze. */
            var matchArray = emailStr.match(emailPat)
            if (matchArray == null) {
                /* Too many/few @'s or something; basically, this address doesn't
                even fit the general mould of a valid e-mail address. */
                //alert("Email address seems incorrect (check @ and .'s)")
                return false
            }
            var user = matchArray[1]
            var domain = matchArray[2]

            // See if "user" is valid 
            if (user.match(userPat) == null) {
                // user is not valid
                //alert("The username doesn't seem to be valid.")
                return false
            }

            /* if the e-mail address is at an IP address (as opposed to a symbolic
            host name) make sure the IP address is valid. */
            var IPArray = domain.match(ipDomainPat)
            if (IPArray != null) {
                // this is an IP address
                for (var i = 1; i <= 4; i++) {
                    if (IPArray[i] > 255) {
                        //      alert("Destination IP address is invalid!")
                        return false
                    }
                }
                return true
            }

            // Domain is symbolic name
            var domainArray = domain.match(domainPat)
            if (domainArray == null) {
                //alert("The domain name doesn't seem to be valid.")
                return false
            }

            /* domain name seems valid, but now make sure that it ends in a
            three-letter word (like com, edu, gov) or a two-letter word,
            representing country (uk, nl), and that there's a hostname preceding 
            the domain or country. */

            /* Now we need to break up the domain to get a count of how many atoms
            it consists of. */
            var atomPat = new RegExp(atom, "g")
            var domArr = domain.match(atomPat)
            var len = domArr.length
            if (domArr[domArr.length - 1].length < 2 ||
                domArr[domArr.length - 1].length > 3) {
                // the address must end in a two letter or three letter word.
                //alert("The address must end in a three-letter domain, or two letter country.")
                return false
            }

            // Make sure there's a host name preceding the domain.
            if (len < 2) {
                var errStr = "This address is missing a hostname!"
                //alert(errStr)
                return false
            }

            // If we've gotten this far, everything's valid!
            return true;
        }

        //Pdf or Doc file validation
        function validateFile(source, args) {
            ResetUpdateLbl()
            var oFileName = document.getElementById(_clientFileUploadClientId).value
            var Extension = oFileName.toUpperCase().substring(oFileName.indexOf("."))
            var bIsValid = true
            if (oFileName != "") {
                if (oFileName.toUpperCase().indexOf(".DOCX") == -1 && oFileName.toUpperCase().indexOf(".DOC") == -1 && oFileName.toUpperCase().indexOf(".PDF") == -1) {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage = "File to upload should be in valid format.";
                    document.getElementById(_clientCustomValId).innerHTML = "File to upload should be in valid format.";
                }
                else if (oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".DOC" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".DOCX" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".PDF") {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage = "File to upload should be in valid format.";
                    document.getElementById(_clientCustomValId).innerHTML = "File to upload should be in valid format.";
                }
            }
            else {
                bIsValid = false
                document.getElementById(_clientCustomValId).errormessage = "File size should be less than or Equal to 200 KB.";
                document.getElementById(_clientCustomValId).innerHTML = "File size should be less than or Equal to 200 KB.";
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
      
    </script>
  <script type="text/javascript">

window.onload = AddClickEvent;

function AddClickEvent()
{          
    fnOnLoad();    
}
</script>

</asp:Content>