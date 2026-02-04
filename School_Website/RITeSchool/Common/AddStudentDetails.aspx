<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="AddStudentDetails.aspx.cs" Inherits="AddStudentDetails" ViewStateMode="Enabled" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
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
                                            Text="Change sdf" EnableViewState="false"></asp:Label>
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
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Label ID="lblUpdateSucess" runat="server" 
                                    Height="20px" Width="100%" Visible="False" EnableViewState="False" CssClass="ClsLabel"></asp:Label>
                            <asp:RequiredFieldValidator ID = "reqAadharNumber" Display="None" runat = "server" ErrorMessage = "Please enter Aadhar Card Number." ViewStateMode="Enabled" ControlToValidate = "txtAadharNumber"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID = "RequiredFieldValidator1" Display="None" runat = "server" ErrorMessage = "Please enter name present on Aadhar Card." ViewStateMode="Enabled" ControlToValidate = "txtNameOnAadharCard"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <table id="tblUserInfo" border="0" cellpadding="1" cellspacing="2" align="center">
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="lblUserName" class="ClsLabel">Name :</span><span id="cstValEmail" style="color: Red; display: none;"></span>
                                    </td>
                                    <td align="left" >
                                        <asp:TextBox ID="txtLogin" runat="server" ViewStateMode="Enabled" MaxLength="100" CssClass="ExLrgTxtBox"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                            <span class="ClsLabel" style="position:relative">Aadhar Card Number :</span>
                                    </td>
                                    <td align="left" style="color: red">
                                        <asp:TextBox ID="txtAadharNumber" MaxLength = "12" runat="server" ViewStateMode="Enabled" onkeyup="extractNumber(this, 0,false);" 
                                        onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                            CssClass="ExLrgTxtBox" Style="position: relative; top: 0px; left: 0px;"></asp:TextBox>
                                            <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="ClsLabel" style="position:relative">Name Present on Aadhar Card :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtNameOnAadharCard" runat="server" ViewStateMode="Enabled" 
                                            MaxLength="150" CssClass="ExLrgTxtBox"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="ClsLabel" style="position:relative">Mother Tongue :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtMothertongue" runat="server" ViewStateMode="Enabled" MaxLength="20" CssClass="MidTxtBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="ClsLabel" style="position:relative">Email :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" CssClass="MidTxtBox" runat="server" MaxLength="50"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:CustomValidator ID="CustEmail" runat="server" ClientValidationFunction="ValidateEmail"
                                         ControlToValidate="txtEmail" Display="None" ValidateEmptyText="true"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="height: 31px">
                                        <span class="ClsLabel" style="position:relative">Blood Group :</span>
                                    </td>
                                    <td style="height: 31px">
                                        <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="MidCombo" width="150px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem>O+</asp:ListItem>
                                            <asp:ListItem>A+</asp:ListItem>
                                            <asp:ListItem>B+</asp:ListItem>
                                            <asp:ListItem>AB+</asp:ListItem>
                                            <asp:ListItem>O-</asp:ListItem>
                                            <asp:ListItem>A-</asp:ListItem>
                                            <asp:ListItem>B-</asp:ListItem>
                                            <asp:ListItem>AB-</asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Blood Group should be selected." Display="None" ControlToValidate="ddlBloodGroup" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="lblUpload" class="ClsLabel">Upload Scan Copy of Aadhar Card :</span>
                                    </td>
                                    <td align="left">
                                          <asp:FileUpload ID = "fuAadharNumber" CssClass = "LrgTxtBox" runat = "server" /> 
                                          <asp:ImageButton ID="btnView" runat="server" ViewStateMode = "Enabled" CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false"  /> 
                                          <asp:CustomValidator ID="cstValidateAadharScanCopy" Display="None" runat="server" ClientValidationFunction="ValidateAadharScanCopy"
                                                                ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="fuAadharNumber"
                                                                CssClass="LblErrorMsg"></asp:CustomValidator>
                                    </td>                                  
                                </tr>
                                 <tr runat="server" id="trBirthCertificate">
                                     <td class="ClsBorderLight" align="left" id="td1" runat="server">
                                        <span id="lblUpload1" class="ClsLabel">Birth Certificate :</span>
                                     </td>
                                     <td  align="left">
                                        <asp:FileUpload ID="fuBirthCertificate" runat="server" CssClass="LrgTxtBox"/>
                                         <asp:ImageButton ID="btnViewBirthCert" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                              ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                         <asp:CustomValidator ID="cstValidateBirthCertificate" runat="server" ControlToValidate="fuBirthCertificate"
                                             ClientValidationFunction="ValidateBirthCertificate" Display="None"  ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ></asp:CustomValidator>
                                     </td>                                   
                                  </tr>
                                     <tr>
                                        <td align = "center" colspan = "2" class = "ClsBorderlight" style="height: 22px">
                                           <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed
                                                    3MB.)</span>
                                        </td>
                                   </tr>
                            </table>                                                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan = "2" align="center" style="width: 50%; height: 20px;white-space:nowrap;">
                        </td>
                        <td align="left" style="height: 20px;white-space:nowrap;">
                            <asp:Button CssClass="ClsBtn" ID="imgBtnSubmit" runat="server" ViewStateMode="Enabled" Text="Submit" BorderWidth="1px" disable-page="true"
                                OnClick="imgBtnSubmit_Click"></asp:Button></td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidAadharImage" runat="server" ViewStateMode="Enabled"/>
                   <asp:HiddenField ID="hidBirthCertificate" runat="Server" ViewStateMode="Enabled"
                                Value="" />
            </td>
        </tr>
    </table>

    <script type="text/javascript" lang="javascript">
        _clientLabelId = "<%= this.lblErrorMsg.ClientID%>"
        _clientlblUpdateSucess = "<%= this.lblUpdateSucess.ClientID%>"
        _cstValidateAadharScanCopy = "<%= this.cstValidateAadharScanCopy.ClientID %>";
         _clientfuAadharNumber = "<%= this.fuAadharNumber.ClientID %>";
        _cstValidateBirthCertificate = "<%= this.cstValidateBirthCertificate.ClientID %>";
        _clientfuBirthCertificate = "<%= this.fuBirthCertificate.ClientID %>"
        _clientCustEmail = "<%=this.CustEmail.ClientID %>";
        _clienttxtEmail = "<%=this.txtEmail.ClientID %>";

        function ValidateEmail(osrc, args) {
            var sEmail = document.getElementById(_clienttxtEmail).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);
            if (isEmpty(sEmail)) {
               osrc.errormessage = "Email address should not be blank."
                args.IsValid = false;
                return true;
            }
            else {
                // If email is not blank then validate for valid email address.
                if (!isEmail(sEmail)) {
                    osrc.errormessage = "Email address should be in proper format."
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

  </script>
    <script src="../Scripts/Common/AddStudentDetails.js?version=1.1" type="text/javascript"></script>
</asp:Content>
