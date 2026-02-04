<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="StudentChangePassword.aspx.cs" Inherits="StudentChangePassword" ViewStateMode="Disabled" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
  <style>
    .warning-box {
      display: flex;
      align-items: center;
      background-color: #fff4e5;
      color: #663c00;
      border: 1px solid #ffa726;
      border-radius: 6px;
      padding: 5px 16px;
      font-family: Arial, sans-serif;
      font-size: 15px;
      max-width: 650px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      margin: 10px;
    }

    .warning-icon {
      font-size: 20px;
      margin-right: 10px;
      color: #ff9800;
    }

    .warning-text {
      flex: 1;
    }
  </style>

    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        vertical-align: top">
        <tr >
            <td style="background-color: white" id="MainDataTable" align="center">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
				<tr id="trTitle" runat="server" visible="false">
                        <td style="height: 19px" align="left" colspan="4" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="ClsGrayMainTitle" style="height: 20px">
                                        <asp:Label ID="Label2" CssClass="MainTitleHead" runat="server" BorderWidth="0px"
                                            Text="Change Password" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" style="height: 19px">
                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                <asp:Label ID="lblErrorMsg"  runat="server" CssClass="LblErrorMsg" Visible="False" EnableViewState="False" ></asp:Label></asp:Panel>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ViewStateMode="Enabled" ShowMessageBox="False"
                                ShowSummary="True" CssClass="ClsLabel" />
                        </td>
                        <td align="right" class="ClsTextNormal" style="width: 25%; padding-right: 30px; " valign="top">
                            <span class="ClsMdtStar">* Mandatory Fields</span>
                        </td>
                    </tr>
                    <tr id="trPasswordNote" runat="server" visible = "false">
                        <td align="center" colspan="4">                            
                            <div class="warning-box">
                                <span class="warning-icon">⚠️</span>
                                <span class="warning-text">
                                  Your password has not been changed recently. Please update it to maintain security.
                                </span>
                          </div>

                        </td>                        
                    </tr>
                    <tr id="trNote" runat="server" visible="false">
                        <td colspan="4" align="center" >
                            <table id="tblNoteData" runat="server" ViewStateMode="Enabled" style="white-space:nowrap;">
                                <tr>
                                    <td style="width: 100px">
                                    </td>
                                    <td align="left" class="ClsBorderlight " style="width: 7%; background-color: #ffffc4;">
                                        <span class="LblNrmlB" style="font-weight: bold">Note :</span>
                                    </td>
                                    <td align="left" class="ClsBorderlight" style="padding-left: 5px">
                                        <span class="LblSmlV" style="color: Red;">It seems you have not changed the system generated password. Please reset your password for security purpose.</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Label ID="lblUpdateSucess" runat="server" 
                                    Height="20px" Width="100%" Visible="False" EnableViewState="False" CssClass="ClsLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <table id="tblUserInfo" border="0" cellpadding="1" cellspacing="2" align="center">
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="lblUserName" class="ClsLabel">User Name :</span><span id="cstValEmail" style="color: Red; display: none;"></span>
                                    </td>
                                    <td align="left" >
                                        <asp:TextBox ID="txtLogin" runat="server" ViewStateMode="Enabled" MaxLength="20" CssClass="ExLrgTxtBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                            <span class="ClsLabel" style="position:relative">Old Password :</span>
                                    </td>
                                    <td align="left" style="color: red">
                                        <asp:TextBox ID="txtOldPasswd" runat="server" ViewStateMode="Enabled" TextMode="Password" MaxLength="20"
                                            CssClass="ExLrgTxtBox" Style="position: relative; top: 0px; left: 0px;"></asp:TextBox>
                                            <span class="ClsMdtStar">*</span>
                                           <%-- <asp:RequiredFieldValidator ID="req_OldPwd" runat="server" ControlToValidate="txtOldPasswd"
                                            Display="None" ErrorMessage="Old Password should not be blank." CssClass="LblErrorMsg"></asp:RequiredFieldValidator> --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="lblPassword" class="ClsLabel">New Password :</span>
                                    </td>
                                    <td align="left">
                                            <asp:TextBox ID="txtPasswd" runat="server" ViewStateMode="Enabled" TextMode="Password" MaxLength="20" CssClass="ExLrgTxtBox"></asp:TextBox>
                                            <span class="ClsMdtStar">*<%--<asp:RequiredFieldValidator ID="reqPasswd" runat="server" ControlToValidate="txtPasswd"
                                            Display="None" ErrorMessage="New Password should not be blank."></asp:RequiredFieldValidator>    --%>
                                            </span>       
                                            
                                    </td>                                  
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="lblConfirmPasswd" class="ClsLabel">Confirm Password :</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtConfirmPasswd" runat="server" ViewStateMode="Enabled" TextMode="Password" MaxLength="20"
                                            CssClass="ExLrgTxtBox"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>                                        
                                                 <asp:CustomValidator ID="cstValPassword" runat="server" ViewStateMode="Enabled" ClientValidationFunction="PasswordValidation"
                                                ControlToValidate="txtPasswd" Display="None" ValidateEmptyText="True" CssClass="LblErrorMsg"></asp:CustomValidator>
                                                <asp:CustomValidator ID="reqPass" runat="server" ViewStateMode="Enabled" ClientValidationFunction="reqPass"
                                                ControlToValidate="txtPasswd" Display="None" ValidateEmptyText="True" CssClass="LblErrorMsg"></asp:CustomValidator>                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td align="left" class="ClsMdtStar">                                    
                                       <span class="LblSmlGray">Capitalization Matters! Min 6 characters, Max 15 characters.
                                            <br /> </span>
                                         <span class="LblSmlGray" id="PassNote" runat="server"> Password should be combination of at least one lowercase letter, one uppercase letter, one numeric digit, and one special character.</span>
                                    </td>
                                </tr>
                            </table>
                                              <asp:CustomValidator ID="cstValConfirmPassword" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ComparePasswordValidation"
                                                ControlToValidate="txtConfirmPasswd" Display="None" ValidateEmptyText="True" CssClass="LblErrorMsg"></asp:CustomValidator>                                                           
                                     </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 25%; height: 20px;white-space:nowrap;">
                        </td>
                        <td align="right" style="height: 20px;white-space:nowrap;">
                            <asp:Button CssClass="ClsBtn" ID="imgBtnSubmit" runat="server" ViewStateMode="Enabled" Text="Save" BorderWidth="1px" disable-page="true"
                                OnClick="imgBtnSave_Click" ></asp:Button></td>
                        <td align="left" style="height: 20px;white-space:nowrap;">
                            <asp:Button CssClass="ClsBtn" ID="imgBtnCancel" CausesValidation="false" runat="server" ViewStateMode="Enabled"
                                Text="Cancel" BorderWidth="1px" OnClick="imgBtnCancel_Click" UseSubmitBehavior="false"></asp:Button></td>
                        <td align="left" style="width: 25%; height: 20px;white-space:nowrap;">
                            &nbsp;&nbsp;</td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidSuperAdmin" runat="server" ViewStateMode="Enabled"/>
                 <asp:HiddenField ID="hidUrl" runat="server" ViewStateMode="Enabled"/> 
                 <asp:HiddenField ID="hidOldPassword" runat="server" ViewStateMode="Enabled"/> 
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clientLabelId = "<%= this.lblErrorMsg.ClientID%>"
        _clientlblUpdateSucess = "<%= this.lblUpdateSucess.ClientID%>"
        _clienttxtPasswd = "<%= this.txtPasswd.ClientID%>"
        _clientcstValPassword = "<%=this.cstValPassword.ClientID%>"
        _clienttxtConfirmPasswd = "<%=this.txtConfirmPasswd.ClientID %>"
        _clientcstValConfirmPassword = "<%=this.cstValConfirmPassword.ClientID %>"
        _clientimgBtnSubmit = "<%=this.imgBtnSubmit.ClientID%>"
        _clientimgBtnCancel = "<%=this.imgBtnCancel.ClientID%>"
        _clienthidOldPassword = "<%=this.hidOldPassword.ClientID %>"
        _clienthidSuperAdmin = "<%=this.hidSuperAdmin.ClientID %>"
        _clienttxtOldPasswd = "<%=this.txtOldPasswd.ClientID %>"
        _clientreqPass = "<%=this.reqPass.ClientID %>"
                
        var blanks = " \t\n\r"
        function ResetErrLabel() {
        
            var isPageValid = true
            if (document.getElementById(_clientLabelId) != null)
                document.getElementById(_clientLabelId).style.display = "none"
            if (document.getElementById(_clientlblUpdateSucess) != null)
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            if (typeof (Page_ClientValidate) == 'function') {
            
                isPageValid = Page_ClientValidate()
            }
           
        }
        function DisableButtons() {
            document.getElementById(_clientimgBtnSubmit).disabled = true
            document.getElementById(_clientimgBtnCancel).disabled = true
        }

        function reqPass(oSrc, args) {
            var sPassword = document.getElementById(_clienttxtPasswd).value;
            if (sPassword == "") {
                document.getElementById(_clientreqPass).errormessage = "New Password should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function PasswordValidation(oSrc, args) {
            var sPassword = document.getElementById(_clienttxtPasswd).value
            var sOldPassword = document.getElementById(_clienthidOldPassword).value;
            var oldTextbox = document.getElementById(_clienttxtOldPasswd).value;
            
            var password = sPassword;
//            var passed = validatePassword(password, {
//                length: [6, Infinity],
//                //lower: 0,
//                //upper: 0,
//                alpha: 1,
//                numeric: 1,
//                special: 1
            //            });

            var passed = CheckPassword(sPassword);

            if (oldTextbox == "") {
                document.getElementById(_clientcstValPassword).errormessage = "Old Password should not be blank."
                args.IsValid = false
                return true
            }
            else if (oldTextbox != sOldPassword) {
                document.getElementById(_clientcstValPassword).errormessage = "Old Password is wrong."
                args.IsValid = false
                return true
            }
            else if (sPassword == sOldPassword && $get(_clienthidSuperAdmin).value != "true") {
                document.getElementById(_clientcstValPassword).errormessage = "Old Password and New Password should not be the same."
                args.IsValid = false
                return true
            }
            else if (sPassword.length < 6) {
                document.getElementById(_clientcstValPassword).errormessage = "Password should be of minimum 6 characters."
                args.IsValid = false
                return true
            }
            else if (!passed && $get(_clienthidSuperAdmin).value != "true") {
                document.getElementById(_clientcstValPassword).errormessage = "Password should be combination of at least one character, digit & special character.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true
            return false
        }

        //This function is used to validate whether password and confirm password are same or not.
        function ComparePasswordValidation(oSrc, args) {
            
            var sConfirmPassword = document.getElementById(_clienttxtConfirmPasswd).value
            var sPassword = document.getElementById(_clienttxtPasswd).value
            if (sConfirmPassword == "") {
                document.getElementById(_clientcstValConfirmPassword).errormessage = "Confirm Password should not be blank."
                args.IsValid = false
                return true
            }
            else if (sPassword !="" && sConfirmPassword != "" && sPassword != sConfirmPassword) {
                document.getElementById(_clientcstValConfirmPassword).errormessage = "New Password and Confirm Password should be same."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CheckPassword(inputtxt) {
            var decimal = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{6,15}$/;
            if (inputtxt.match(decimal)) {
                return true;
            }
            else {
                return false;
            }
        }   

    </script>
</asp:Content>
