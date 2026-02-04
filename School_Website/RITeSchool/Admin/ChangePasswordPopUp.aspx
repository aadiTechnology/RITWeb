<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePasswordPopUp.aspx.cs"
    MasterPageFile="../MasterPages/PopupMasterSml.master" Inherits="ChangeUserpassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        vertical-align: top">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center">
                <table border="0" cellpadding="0" cellspacing="4" style="width: 100%;">
                    <tr>
                        <td style="height: 19px" align="left" colspan="2" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td class="ClsGrayMainTitle" style="height: 20px">
                                        <asp:Label ID="Label2" CssClass="MainTitleHead" runat="server" BorderWidth="0px"
                                            Text= "<%$ Resources:LocalizedResources, ChangePassword%>" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                     <td align="right" class="ClsTextNormal" style="width: 100%; ">
                              <span class="ClsMdtStar">*</span>
                              <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="100%">
                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" 
                                Height="20px" Width="100%" Visible="False" EnableViewState="False" CssClass="LblErrorMsg"></asp:Label></asp:Panel>                         
                        </td>                       
                    </tr>
                    <tr>
                        <td align="left">
                               <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ShowMessageBox="False"
                                ShowSummary="True" CssClass="ClsLabel" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Font-Bold="true"
                                Height="20px" Width="100%" Visible="False" EnableViewState="False" CssClass="ClsLabel"></asp:Label></asp:Panel>                         
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <table id="tblUserInfo" border="0" cellpadding="1" cellspacing="2" style="width: 78%;"
                                align="center">
                                <tr>
                                    <td align="left" style="width: 20%; height: 21px;" class="ClsBorderlight">                                        
                                        <asp:Label CssClass = "ClsLabel" ID="lblUserName" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserName%>"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                    <td align="left" style=" width: 35%; height: 21px;">
                                        <asp:TextBox ID="txtLogin" runat="server" MaxLength="20" CssClass="LrgTxtBox"></asp:TextBox>
                                        <span class="ClsMdtStar"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                       <asp:Label CssClass = "ClsLabel" ID="lblNewPassword" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NewPassword%>"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <span class="ClsMdtStar">
                                            <asp:TextBox ID="txtPasswd" runat="server" TextMode="Password" 
                                            MaxLength="15" CssClass="LrgTxtBox"></asp:TextBox>                                            
                                            <span class="ClsMdtStar">* </span>
                                            <asp:CustomValidator ID="cstValPassword" runat="server" ClientValidationFunction="PasswordValidation"
                                                ControlToValidate="txtPasswd" Display="None" ValidateEmptyText="True" CssClass="LblErrorMsg"></asp:CustomValidator>                                            
                                            
                                        </span>
                                   </td>  
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label CssClass = "ClsLabel" ID="lblConfirmPassword" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ConfirmPassword%>"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                    <td class="ClsMdtStar" align="left">
                                        <asp:TextBox ID="txtConfirmPasswd" runat="server" TextMode="Password" MaxLength="15"
                                            CssClass="LrgTxtBox"></asp:TextBox>
                                        <span class="ClsMdtStar">* </span>                                        
                                            <asp:CustomValidator ID="cstValConfirmPassword" runat="server" ClientValidationFunction="ComparePasswordValidation"
                                                ControlToValidate="txtConfirmPasswd" Display="None" ValidateEmptyText="True" CssClass="LblErrorMsg"></asp:CustomValidator>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td class="ClsMdtStar" align="left">                                   
                                       <span class="LblSmlGray">
                                    <asp:Label  ID="lblNotePass" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PasswordCondition %>"></asp:Label>
                                            <br />
                                           <asp:Label  ID="lblNoteConfirmPass" runat="server" EnableViewState="False" Text="Password should be combination of at least one lowercase letter, one uppercase letter, one numeric digit, and one special character."></asp:Label>
                                            </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center"  colspan="2">
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td align="center" class="ClsMdtStar" colspan="2">
                                        &nbsp;<asp:Button CssClass="ClsBtn" ID="BtnSave" runat="server" Text= "<%$ Resources:LocalizedResources, Save%>" BorderWidth="1px"
                                OnClick="BtnSave_Click" disable-page="true"></asp:Button>
                            <asp:Button CssClass="ClsBtn" ID="BtnCancel" CausesValidation="false" runat="server"
                                Text= "<%$ Resources:LocalizedResources, Cancel%>" BorderWidth="1px" OnClick="BtnCancel_Click" UseSubmitBehavior="false">
                            </asp:Button></td>
                                </tr>
                            </table>
                   
                        </td>
                         
                    </tr>
                </table>
                <asp:HiddenField ID="hidUserId" runat="server" />       
                <asp:HiddenField ID="hidOldPassword" runat="server" />  
                <asp:HiddenField ID = "hidCultureInfo"  runat = "server" />
                <asp:HiddenField ID = "hidValNewPasswordBlank" runat = "server" />
                <asp:HiddenField ID = "hidPasswordConditionErrorMsg" runat = "server" />
                <asp:HiddenField ID = "hidPasswordCondition1" runat = "server" />  
                 <asp:HiddenField ID = "hidConfirmPasswordErrorMessage" runat = "server" />       
                 <asp:HiddenField ID = "hidValNewAndConfirmPassword" runat = "server" />                            
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clientLabelId = "<%=this.lblErrorMsg.ClientID %>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clienttxtPasswd = "<%=this.txtPasswd.ClientID %>"
        _clienttxtConfirmPasswd = "<%=this.txtConfirmPasswd.ClientID %>"
        _clientcstValPassword = "<%=this.cstValPassword.ClientID %>"
        _clientcstValConfirmPassword = "<%=this.cstValConfirmPassword.ClientID %>"
        _clientBtnSave = "<%=this.BtnSave.ClientID %>"
        _clientBtnCancel = "<%=this.BtnCancel.ClientID %>"
        _clienthidOldPassword = "<%=this.hidOldPassword.ClientID %>"                

        var blanks = " \t\n\r"
        function ResetErrLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function')
                isPageValid = Page_ClientValidate()
            
            if (document.getElementById(_clientLabelId))
                document.getElementById(_clientLabelId).style.display = "none"
            if (document.getElementById(_clientlblUpdateSucess))
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
        }

        function PasswordValidation(oSrc, args) {
            var sPassword = document.getElementById(_clienttxtPasswd).value
            var sOldPassword = document.getElementById(_clienthidOldPassword).value;            
            var password = sPassword;
//            var passed = validatePassword(password, {
//                length: [6, Infinity],                
//                alpha: 1,
//                numeric: 1,
//                special: 1
            //            });

            var passed = CheckPassword(sPassword)

            if (sPassword == "") {
                document.getElementById(_clientcstValPassword).errormessage = document.getElementById("<%=this.hidValNewPasswordBlank.ClientID %>").value;
                args.IsValid = false
                return true
            }            
            else if (sPassword.length < 6) {
                document.getElementById(_clientcstValPassword).errormessage = document.getElementById("<%=this.hidPasswordConditionErrorMsg.ClientID %>").value;
                args.IsValid = false
                return true
            }
            else if (!passed) {
                document.getElementById(_clientcstValPassword).errormessage = document.getElementById("<%=this.hidPasswordCondition1.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true
            return false
        }
        function ComparePasswordValidation(oSrc, args) {
            var sConfirmPassword = document.getElementById(_clienttxtConfirmPasswd).value
            var sPassword = document.getElementById(_clienttxtPasswd).value
            if (sConfirmPassword == "") {
                document.getElementById(_clientcstValConfirmPassword).errormessage = document.getElementById("<%=this.hidConfirmPasswordErrorMessage.ClientID %>").value;
                args.IsValid = false
                return true
            }
            else if (sPassword != "" && sConfirmPassword != "" && sPassword != sConfirmPassword) {
                document.getElementById(_clientcstValConfirmPassword).errormessage = document.getElementById("<%=this.hidValNewAndConfirmPassword.ClientID %>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
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
