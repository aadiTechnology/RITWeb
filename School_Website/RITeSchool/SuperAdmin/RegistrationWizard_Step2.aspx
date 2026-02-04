<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    CodeFile="RegistrationWizard_Step2.aspx.cs" Inherits="RegistrationWizard_Step2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; vertical-align: top">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center">
                <!-- Data Insert Here -->
                <table border="0" cellpadding="0" cellspacing="2" style="width: 97%;">
                    <tr>
                        <td style="height: 19px" align="left" colspan="1" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td class="ClsGrayMainTitle" style="height: 20px" id="tdUserLbl" runat="server" visible="false">
                                            <span class="MainTitleHead">User Information</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="1">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" />
                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                <asp:Label ID="lblErrorMsg"  runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="ClsTextNormal" style="padding-right: 30px; height: 19px;">
                            <span class="ClsMdtStar">* Mandatory Fields</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" class="ClsTextNormal" align="center">
                            <!-- User InfoTable starts here -->
                            <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                style="width: 60%;">
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 25%">
                                            <span class="ClsLabel">User Name :</span>
                                            <span class="LblSmlGray floatR">(First Name)</span></td>
                                    <td align="left" class="ClsMdtStar" style="width: 50%">
                                        <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" Width="50px">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="MidTxtBox" Width="186px"></asp:TextBox>
                                        *&nbsp;
                                        <asp:RequiredFieldValidator ID="rqdFirstName" runat="server" ControlToValidate="txtFirstName"
                                            Display="None" ErrorMessage="First name should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                            <span class="LblSmlGray floatR">(Middle Initial)</span></td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="1"
                                            Width="50px"></asp:TextBox>
                                        *<asp:RequiredFieldValidator ID="reqMiddleInitial" runat="server" ControlToValidate="txtMiddleName"
                                            Display="None" ErrorMessage="Middle initial should not be blank."></asp:RequiredFieldValidator>                                            
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                            <span class="LblSmlGray floatR">(Last Name)</span></td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="MidTxtBox"></asp:TextBox>
                                        *<asp:RequiredFieldValidator ID="rqdLastName" runat="server" ControlToValidate="txtLastName"
                                            Display="None" ErrorMessage="Last name should not be blank."></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                            <span class="ClsLabel">Mobile No.:</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:TextBox ID="txtMobileNo" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" />
                                        *<asp:RequiredFieldValidator ID="reqtxtMobileNo" runat="server" ControlToValidate="txtMobileNo"
                                            Display="None" ErrorMessage="Mobile No. should not be blank."></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                            Visible="true" ErrorMessage="Mobile number should be of 10 digits." EnableClientScript="true"
                                            ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                            <table id="tblUserInfo" runat="server" border="0" cellpadding="1" cellspacing="2"
                                style="width: 60%;">
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 25%; height: 24px">
                                        <span class="ClsLabel">E-mail :</span>
                                        <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation"
                                            ControlToValidate="txtEmail" Display="None" ValidateEmptyText="True"></asp:CustomValidator></td>
                                    <td align="left" style="width: 50%; height: 24px;">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span></td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 25%; height: 24px">
                                        <span class="ClsLabel">Designation :</span>
                                    </td>
                                    <td align="left" style="width: 50%; height: 24px;">
                                        <asp:DropDownList ID="cmbDesignations" runat="server" CssClass="MidCombo" style="width:170px;">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>                                        
                                        <asp:RequiredFieldValidator ID="reqDesignations" runat="server" Display="None" ErrorMessage="Designation should be selected."
                                            ControlToValidate="cmbDesignations" InitialValue="0" CssClass="ClsMdtStar"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="ClsLabel">Login :</span></td>
                                    <td id="Td1" align="left" style="color: red;">
                                        <asp:TextBox ID="txtLogin" runat="server" MaxLength="20" CssClass="ExLrgTxtBox"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:CustomValidator ID="cst_UserName" runat="server" ClientValidationFunction="LoginValidation"
                                            Display="None"></asp:CustomValidator></td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="">
                                            <span class="ClsLabel">Password :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtPasswd" runat="server" TextMode="Password" MaxLength="15" CssClass="ExLrgTxtBox"></asp:TextBox>
                                        <span class="ClsMdtStar">*
                                            <asp:CustomValidator ID="cstValPassword" runat="server" ClientValidationFunction="PasswordValidation"
                                                ControlToValidate="txtPasswd" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="reqPasswd" runat="server" ControlToValidate="txtPasswd"
                                                Display="None" ErrorMessage="Password should not be blank."></asp:RequiredFieldValidator>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="">
                                            <span class="ClsLabel">Confirm Password :</span></td>
                                    <td class="ClsMdtStar" align="left">
                                        <asp:TextBox ID="txtConfirmPasswd" runat="server" TextMode="Password" MaxLength="15"
                                            CssClass="ExLrgTxtBox"></asp:TextBox>
                                        *<asp:CustomValidator ID="cstValConfirmPassword" runat="server" ClientValidationFunction="ComparePasswordValidation"
                                                ControlToValidate="txtConfirmPasswd" Display="None" ValidateEmptyText="True" ></asp:CustomValidator>                                       
                                   </td>
                                </tr>
                                <tr>
                                    <td align="left" style="">
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:Button CssClass="ClsBtn" ID="imgBtnSubmit" runat="server" Text="Save" BorderWidth="1px"
                                            OnClick="imgBtnSubmit_Click" UseSubmitBehavior="false"></asp:Button>&nbsp;<asp:Button
                                                CssClass="ClsBtn" ID="imgBtnCancel" CausesValidation="false" runat="server" Text="Cancel"
                                                BorderWidth="1px" OnClick="imgBtnCancel_Click" UseSubmitBehavior="false"></asp:Button></td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                    </td>
                                </tr>
                            </table>
                            <!-- User InfoTable end here -->
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidUserId" runat="server" />
                <asp:HiddenField ID="hidUserRoleId" runat="server" />
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>"
        _clienttxtEmailId = "<%=this.txtEmail.ClientID %>"
        _clienttxtPasswdId = "<%=this.txtPasswd.ClientID %>"
        _clientcstValPasswdId = "<%=this.cstValPassword.ClientID %>"
        _clienttxtConfirmPasswd = "<%=this.txtConfirmPasswd.ClientID %>"
        _clientcstValConfirmPassword = "<%=this.cstValConfirmPassword.ClientID %>"
        _clienttxtLoginId = "<%=this.txtLogin.ClientID %>"
        _clientcst_UserNameId = "<%=this.cst_UserName.ClientID %>"
        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientimgBtnSubmit = "<%=this.imgBtnSubmit.ClientID %>"
        _clientimgBtnCancel = "<%=this.imgBtnCancel.ClientID %>"
        function ResetErrorMsgLbl() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            if (isPageValid) {
                document.getElementById(_clientimgBtnSubmit).disabled = true
                document.getElementById(_clientimgBtnCancel).disabled = true
            }
            document.getElementById(_clientcst_LblErrMsg).innerText = " "
            document.getElementById(_clientcst_LblErrMsg).innerHTML = " "
        }
        function DisableButtons() {
            document.getElementById(_clientimgBtnSubmit).disabled = true
            document.getElementById(_clientimgBtnCancel).disabled = true
        }
        var blanks = " \t\n\r"
        function EmailValidation(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtEmailId).value
            sEmail = stripLeadingTrailingBlanks(sEmail)
            if (isEmpty(sEmail)) {
                document.getElementById(_clientcstValEmailId).errormessage = "E-mail should not be blank."
                args.IsValid = false
                return true
            }
            else {
                if (!isEmail(sEmail)) {
                    document.getElementById(_clientcstValEmailId).errormessage = "E-mail should be in valid format(For Example :\" john.smith@yahoo.com \")."
                    args.IsValid = false
                    return true
                } 
            }
            args.IsValid = true
            return false
        }
        function PasswordValidation(oSrc, args) {
            var sPassword = document.getElementById(_clienttxtPasswdId).value           
            if (sPassword.length < 6) {
                document.getElementById(_clientcstValPasswdId).errormessage = "Password should be of minimum 6 characters."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        //This function is used to validate whether password and confirm password are same or not.
        function ComparePasswordValidation(oSrc, args) {
            var sConfirmPassword = document.getElementById(_clienttxtConfirmPasswd).value
            var sPassword = document.getElementById(_clienttxtPasswdId).value
            if (sPassword != sConfirmPassword) {
                document.getElementById(_clientcstValConfirmPassword).errormessage = "Password and Confirm Password should be same."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }


        function LoginValidation(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtLoginId).value
            sEmail = stripLeadingTrailingBlanks(sEmail)
            if (isEmpty(sEmail)) {
                document.getElementById(_clientcst_UserNameId).errormessage = "Login should not be blank."
                args.IsValid = false
                return true
            }
            if (sEmail.length < 6) {
                document.getElementById(_clientcst_UserNameId).errormessage = "Login should be of minimum 6 characters."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobileNo.ClientID %>"
        function MobileNumberValidation(oSrc, args) {
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)
            if (sMobileNumber.length == 0) { }
            else if (sMobileNumber.length < 10) {
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
    </script>
</asp:Content>
