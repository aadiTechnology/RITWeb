<%@ Page Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="IdentityCardDetailsUI.aspx.cs" Inherits="IdentityCardDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
<table>
     <tr>
        <td style="background-color: white;" id="Td1" colspan="2">
                            
                                <table>
                                <tr>
                                    <td colspan="2">
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ShowSummary="true"  EnableViewState="false"/>
                                    </td>
                                </tr>
                                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                            <span class="ClsLabel" id="Span2">Upload Logo :</span>
                        </td>
                        <td align="left" colspan="1" style="height: 151px">                            
                             <img id="imgPhoto" alt="image"  runat="server" height="151" /> 
                            <br />
                            <asp:FileUpload ID="UploadLogo" runat="server" TabIndex="12" />
                            
                             <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ClientValidationFunction="ValidateLogo"
                                ErrorMessage="Invalid file format. Only bitmap(*.bmp) is allowed." CssClass="TxtNormal"
                                ></asp:CustomValidator>
                            <span class="LblSmlGray">(Supports files of types - .BMP)</span>
                        </td>
                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 146px">
                                            <span class="ClsLabel" id="lblAddress">Address For I-Card :</span>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="300" CssClass="LrgTxtBox"
                                                Width="500px" TabIndex="21"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 146px">
                                            <span class="ClsLabel" id="Span8">Upload Principal's Sign :</span>
                                        </td>
                                        <td align="left" colspan="1" style="height: 30px;">                                            
                                              <img id="imgSign" alt="image"  runat="server" height="30" width="77"/>     
                                            <br />
                                            <asp:FileUpload ID="UploadSign" runat="server" TabIndex="22" />
                                             <asp:CustomValidator ID="CustValidateSign" Display="None" runat="server" ClientValidationFunction="ValidateSignLogo"
                                                CssClass="LblErrorMsg"></asp:CustomValidator>
                                              <span class="LblSmlGray">(Supports files of types - .PNG,.JPG,.JPEG,.BMP)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 146px">
                                            <span class="ClsLabel" id="Span9">Upload Logo For I-Card :</span>
                                        </td>
                                        <td align="left" colspan="1" style="height: 50px;">                                            
                                            <img id="imgLogoICard" alt="image"  runat="server" height="50" width="72"/>  
                                            <br />
                                            <asp:FileUpload ID="UploadICard" runat="server"  TabIndex="23" />
                                            <asp:CustomValidator ID="CustICardLogo" Display="None" runat="server" ClientValidationFunction="ValidateICardLogo"
                                                ErrorMessage="Invalid file format." ControlToValidate="UploadICard" CssClass="LblErrorMsg"
                                                ></asp:CustomValidator>
                                          <span class="LblSmlGray">(Supports files of types - .PNG,.JPG,.JPEG,.BMP)</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="height: 28px">
                                            <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server"
                                                Text="Save" BorderWidth="1px" UseSubmitBehavior="false" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnBack" CssClass="ClsBtn" runat="server" CausesValidation="false"  Text="Back" />
                                        </td>
                                    </tr>
                                </table>
                        </td>
     </tr>
</table>
<asp:HiddenField ID="hidSignPath" runat="server" />
<asp:HiddenField ID="hidSchoolName" runat="server" />
<asp:HiddenField ID="hidFilePath" runat="server" />
<asp:HiddenField ID="hidICardPath" runat="server" />
<script language="javascript" type="text/javascript">

    _clientCustValidateSign = "<%=this.CustValidateSign.ClientID %>"
    _clientUploadSign = "<%=this.UploadSign.ClientID%>"
    _clientCustICardLogo = "<%=this.CustICardLogo.ClientID %>"
    _clientUploadICard = "<%=this.UploadICard.ClientID%>"
    _clientCstValidateLogo = "<%=this.cstValidateLogo.ClientID %>"
    _clientFileUploadLogo = "<%=this.UploadLogo.ClientID%>"
    _clientCustValidateSign = "<%=this.CustValidateSign.ClientID %>"
    _clientUploadSign = "<%=this.UploadSign.ClientID%>"
    _clientCustICardLogo = "<%=this.CustICardLogo.ClientID %>"
    _clientUploadICard = "<%=this.UploadICard.ClientID%>"
    _clienthidFilePath = "<%=this.hidFilePath.ClientID %>"

    function ValidateSignLogo(aSrc, args) {
     
        if (CheckFileTypeSign(document.getElementById(_clientUploadSign).value)) { }
        else {
            document.getElementById(_clientCustValidateSign).errormessage = "Invalid file format."
            args.IsValid = false
            return true
        }
        args.IsValid = true
        return false
    }

    function ValidateICardLogo(aSrc, args) {
        if (CheckFileTypeSign(document.getElementById(_clientUploadICard).value)) { }
        else {
            document.getElementById(_clientCustICardLogo).errormessage = "Invalid file format."
            args.IsValid = false
            return true
        }
        args.IsValid = true
        return false
    }
    function CheckFileTypeSign(sFileName) {
        var bIsValid
        if (sFileName != "") {
            if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG"
                || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP") {

                bIsValid = true
            }
            else {
                bIsValid = false
            }
        }
        else {
            bIsValid = true
        }
        return bIsValid
    }
    function ValidateLogo(aSrc, args) {
        var myImage = new Image()
        myImage.src = document.getElementById(_clientFileUploadLogo).value

        if (document.getElementById(_clientFileUploadLogo).value == '') {
            args.IsValid = true
            return false
        }
        else if (!CheckFileType(document.getElementById(_clientFileUploadLogo).value)) {
            document.getElementById(_clientCstValidateLogo).errormessage = "Invalid file format. Only bitmap(*.bmp) is allowed."
            args.IsValid = false
            return true
        }
        args.IsValid = true
        return false
    }
    function CheckFileType(sFileName) {
        var bIsValid = true
        if (sFileName != "") {
            if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP") {
                bIsValid = true
            }
            else {
                bIsValid = false
            }
        }
        return bIsValid
    }
    
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" Runat="Server">
</asp:Content>

