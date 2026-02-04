<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadParentPhotosUI.aspx.cs" Inherits="UploadParentPhotosUI"
    ViewStateMode="Enabled" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        vertical-align: top">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <tr>
                        <td style="height: 20px;">
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" Visible="False"
                                EnableViewState="False"></asp:Label>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ViewStateMode="Enabled"
                                ShowMessageBox="False" ShowSummary="True" CssClass="ClsLabel" ValidationGroup="Save" />
                            <asp:CustomValidator ID="cstFileIsUploaded" Display="None" runat="server" ClientValidationFunction="CheckPhotoIsUploaded"
                                ErrorMessage="Test Message" CssClass="LblErrorMsg" ValidationGroup="Save"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstTransportFileIsUploaded" Display="None" runat="server" ClientValidationFunction="CkeckTransportPersonPhotoIsUploaded"
                                ErrorMessage="Test Message" CssClass="LblErrorMsg" ValidationGroup="Transport"></asp:CustomValidator>
                            <asp:ValidationSummary ID="valTransportErrorMsg" runat="server" ViewStateMode="Enabled"
                                ShowMessageBox="False" ShowSummary="True" CssClass="ClsLabel" ValidationGroup="Transport" />
                        </td>
                    </tr>
                    <tr>
                        <td id="tdMessage" runat="server" align="center">
                            <asp:Label ID="lblUpdateSucess" runat="server" Height="20px" Width="100%" CssClass="ClsLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="text-align: center;">
                            <table align="center" style="text-align: center; margin: 0px auto;">
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="lblUserName" class="ClsLabel">Father Name :</span><span id="cstValEmail"
                                            style="color: Red; display: none;"></span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblFatherName" runat="server" Height="20px" Width="100%" Visible="true"
                                            EnableViewState="False" BackColor="Transparent"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span1" class="ClsLabel">Photo :</span>
                                    </td>
                                    <td>
                                        <table id="tblRelativePhoto" runat="server">
                                            <tr>
                                                <td>
                                                    <div class="ClsBorderlight" style="vertical-align: middle">
                                                        <img id="imgFatherPhoto" alt="image" runat="server" height="151" width="119" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="lblUpload" class="ClsLabel">Upload Scan Copy of Father Photo :</span>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="fuFatherPhoto" CssClass="LrgTxtBox" runat="server" Width="225px" />
                                        <asp:CustomValidator ID="cstValidateFatherPhoto" Display="None" runat="server" ClientValidationFunction="ValidateFatherPhoto"
                                            ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="fuFatherPhoto"
                                            CssClass="LblErrorMsg" ValidationGroup="Save"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" class="ClsBorderlight" style="font-weight: bold;">
                                        <asp:Label ID="lblUploadHeight" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageHeight%>"></asp:Label><br />
                                        <asp:Label ID="lblUploadSize" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageSize%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 20px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span2" class="ClsLabel">Mother Name :</span><span id="Span3" style="color: Red;
                                            display: none;"></span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblMotherName" runat="server" Height="20px" Width="100%" Visible="true"
                                            EnableViewState="False" BackColor="Transparent"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span5" class="ClsLabel">Photo :</span>
                                    </td>
                                    <td>
                                        <table id="Table1" runat="server">
                                            <tr>
                                                <td>
                                                    <div class="ClsBorderlight" style="vertical-align: middle">
                                                        <img id="imgMotherPhoto" alt="image" runat="server" height="151" width="119" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span4" class="ClsLabel">Upload Scan Copy of Mother Photo :</span>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="fuMotherPhoto" CssClass="LrgTxtBox" runat="server" Width="225px" />
                                        <asp:CustomValidator ID="cstValidateMotherPhoto" Display="None" runat="server" ClientValidationFunction="ValidateMotherPhoto"
                                            ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="fuMotherPhoto"
                                            CssClass="LblErrorMsg" ValidationGroup="Save"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" class="ClsBorderlight" style="font-weight: bold;">
                                        <asp:Label ID="Label1" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageHeight%>"></asp:Label><br />
                                        <asp:Label ID="Label3" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageSize%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 20px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span6" class="ClsLabel">Relative Name :</span><span id="Span7" style="color: Red;
                                            display: none;"></span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtRelativeName" CssClass="LrgCombo" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span9" class="ClsLabel">Photo :</span>
                                    </td>
                                    <td>
                                        <table id="Table2" runat="server">
                                            <tr>
                                                <td>
                                                    <div class="ClsBorderlight" style="vertical-align: middle">
                                                        <img id="imgParentPhoto" alt="image" runat="server" height="151" width="119" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span8" class="ClsLabel">Upload Scan Copy of Relative Photo :</span>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="fuParentPhoto" CssClass="LrgTxtBox" runat="server" Width="225px" />
                                        <asp:CustomValidator ID="cstValidateParentPhoto" Display="None" runat="server" ClientValidationFunction="ValidateParentPhoto"
                                            ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="fuParentPhoto"
                                            CssClass="LblErrorMsg" ValidationGroup="Save"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" class="ClsBorderlight" style="font-weight: bold;">
                                        <asp:Label ID="Label4" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageHeight%>"></asp:Label><br />
                                        <asp:Label ID="Label5" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageSize%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 10px;">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center; margin: 0px auto;">
                        <td align="center" style="text-align: center; width: 100%;">
                            <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" ViewStateMode="Enabled"
                                Text="Save" BorderWidth="1px" disable-page="true" OnClick="btnSave_Click" ValidationGroup="Save">
                            </asp:Button>
                            <asp:Button CssClass="ClsBtn" ID="btnSubmit" runat="server" ViewStateMode="Enabled"
                                Text="Submit" BorderWidth="1px" disable-page="true" OnClick="btnSubmit_Click">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr style="color: #C0C0C0" />
                        </td>
                    </tr>
                    <tr id="trTransportPickUp" runat="server" visible="false">
                        <td align="center" style="text-align: center;">
                            <table align="center" style="text-align: center; margin: 0px auto; width:40%;">
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span10" class="ClsLabel">Transport Pickup Person Name :</span><span id="Span11"
                                            style="color: Red; display: none;"></span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtTransportPickUpPerson" CssClass="LrgCombo" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqTransportPickName"  ControlToValidate ="txtTransportPickUpPerson" runat="server" ErrorMessage="Transport Pickup Person Name should not be blank." ValidationGroup="Transport" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span12" class="ClsLabel">Photo :</span>
                                    </td>
                                    <td>
                                        <table id="Table3" runat="server">
                                            <tr>
                                                <td>
                                                    <div class="ClsBorderlight" style="vertical-align: middle">
                                                        <img id="imgTransportPerson" alt="image" runat="server" height="151" width="119" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span13" class="ClsLabel">Upload Scan Copy of Transport pickup Person Photo
                                            :</span>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="fuTransportPersonPhoto" CssClass="LrgTxtBox" runat="server" Width="225px" />
                                        <asp:CustomValidator ID="cstValidateTransportPersonPhoto" Display="None" runat="server"
                                            ClientValidationFunction="ValidateTransportPersonPhoto" ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>"
                                            ControlToValidate="fuTransportPersonPhoto" CssClass="LblErrorMsg" ValidationGroup="Transport"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" class="ClsBorderlight" style="font-weight: bold;">
                                        <asp:Label ID="Label2" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageHeight%>"></asp:Label><br />
                                        <asp:Label ID="Label6" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageSize%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 10px;">
                                    </td>
                                </tr>
                                <tr align="center" style="text-align: center; margin: 0px auto;">
                                    <td align="center" colspan="2" style="text-align: center; width: 100%;">
                                        <asp:Button CssClass="ClsBtn" ID="btnSaveTransport" runat="server" ViewStateMode="Enabled"
                                            Text="Save" BorderWidth="1px" disable-page="true" 
                                            ValidationGroup="Transport" onclick="btnSaveTransport_Click">
                                        </asp:Button>
                                        <asp:Button CssClass="ClsBtn" ID="btnsubmitTransport" runat="server" ViewStateMode="Enabled"
                                            Text="Submit" BorderWidth="1px" disable-page="true" 
                                            onclick="btnsubmitTransport_Click">
                                        </asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <asp:HiddenField ID="hidIsSibling" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSaveSiblingDetails" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsFatherPhotoSaved" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsMotherPhotoSaved" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsParentPhotoSaved" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSubmitSiblingDetails" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsTransportPickUpPersonPhotosaved" runat="server" Value="0" />
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientcstValidateFatherPhoto = "<%=this.cstValidateFatherPhoto.ClientID %>"
        _clientfuFatherPhoto = "<%=this.fuFatherPhoto.ClientID %>"
        _clientcstValidateMotherPhoto = "<%=this.cstValidateMotherPhoto.ClientID %>"
        _clientfuMotherPhoto = "<%=this.fuMotherPhoto.ClientID %>"
        _clientcstValidateParentPhoto = "<%=this.cstValidateParentPhoto.ClientID %>"
        _clientfuParentPhoto = "<%=this.fuParentPhoto.ClientID %>"
        _clientcstFileIsUploaded = "<%=this.cstFileIsUploaded.ClientID %>"
        _clienthidIsSibling = "<%=this.hidIsSibling.ClientID %>"
        _clienthidSaveSiblingDetails = "<%=this.hidSaveSiblingDetails.ClientID %>"
        _clienthidIsFatherPhotoSaved = "<%=this.hidIsFatherPhotoSaved.ClientID %>"
        _clienthidIsMotherPhotoSaved = "<%=this.hidIsMotherPhotoSaved.ClientID %>"
        _clienthidIsParentPhotoSaved = "<%=this.hidIsParentPhotoSaved.ClientID %>"
        _clienthidSubmitSiblingDetails = "<%=this.hidSubmitSiblingDetails.ClientID %>"
        _clientfuTransportPersonPhoto = "<%=this.fuTransportPersonPhoto.ClientID %>"
        _clientcstValidateTransportPersonPhoto = "<%=this.cstValidateTransportPersonPhoto.ClientID %>"
        _clienthidIsTransportPickUpPersonPhotosaved = "<%=this.hidIsTransportPickUpPersonPhotosaved.ClientID %>"
        _clientcstTransportFileIsUploaded = "<%=this.cstTransportFileIsUploaded.ClientID %>"        

        function ValidateFatherPhoto(aSrc, args) {
            var myImage = new Image();
            myImage.src = document.getElementById(_clientfuFatherPhoto).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForPhoto(myImage.src))//if file type is valid
            {
                if (document.getElementById(_clientfuFatherPhoto).files[0].size > 81920) {
                    document.getElementById(_clientcstValidateFatherPhoto).errormessage = "Father Photo file size should not be greater than 80kb.";
                    args.IsValid = false;
                    return true;
                }
            }
            else//if file type is not valid
            {
                document.getElementById(_clientcstValidateFatherPhoto).errormessage = "Father Photo file type should be between .jpg, .jpeg, .png and .bmp.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateMotherPhoto(aSrc, args) {
            var myImage = new Image();
            myImage.src = document.getElementById(_clientfuMotherPhoto).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForPhoto(myImage.src))//if file type is valid
            {
                if (document.getElementById(_clientfuMotherPhoto).files[0].size > 81920) {
                    document.getElementById(_clientcstValidateMotherPhoto).errormessage = "Mother Photo file size should not be greater than 80kb.";
                    args.IsValid = false;
                    return true;
                }
            }
            else//if file type is not valid
            {
                document.getElementById(_clientcstValidateMotherPhoto).errormessage = "Mother Photo file type should be between .jpg, .jpeg, .png and .bmp.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateParentPhoto(aSrc, args) {
            var myImage = new Image();
            myImage.src = document.getElementById(_clientfuParentPhoto).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForPhoto(myImage.src))//if file type is valid
            {
                if (document.getElementById(_clientfuParentPhoto).files[0].size > 81920) {
                    document.getElementById(_clientcstValidateParentPhoto).errormessage = "Parent Photo file size should not be greater than 80kb.";
                    args.IsValid = false;
                    return true;
                }
            }
            else//if file type is not valid
            {
                document.getElementById(_clientcstValidateParentPhoto).errormessage = "Parent Photo file type should be between .jpg, .jpeg, .png and .bmp.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateTransportPersonPhoto(aSrc, args) {            
            var myImage = new Image();
            myImage.src = document.getElementById(_clientfuTransportPersonPhoto).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForPhoto(myImage.src))//if file type is valid
            {
                if (document.getElementById(_clientfuTransportPersonPhoto).files[0].size > 81920) {
                    document.getElementById(_clientcstValidateTransportPersonPhoto).errormessage = "Transport PickUp person Photo file size should not be greater than 80kb.";
                    args.IsValid = false;
                    return true;
                }
            }
            else//if file type is not valid
            {
                document.getElementById(_clientcstValidateTransportPersonPhoto).errormessage = "Transport PickUp person Photo file type should be between .jpg, .jpeg, .png and .bmp.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function CheckPhotoIsUploaded(aSrc, args) {
            if ((document.getElementById(_clientfuFatherPhoto).value == "" && document.getElementById(_clienthidIsFatherPhotoSaved).value == 0) && (document.getElementById(_clientfuMotherPhoto).value == "" && document.getElementById(_clienthidIsMotherPhotoSaved).value == 0) && (document.getElementById(_clientfuParentPhoto).value == "" && document.getElementById(_clienthidIsParentPhotoSaved).value == 0)) {
                document.getElementById(_clientcstFileIsUploaded).errormessage = "At least one file should be uploaded.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function CkeckTransportPersonPhotoIsUploaded(aSrc, args) {            
            if (document.getElementById(_clientfuTransportPersonPhoto).value == "" && document.getElementById(_clienthidIsTransportPickUpPersonPhotosaved).value == 0) {
                document.getElementById(_clientcstTransportFileIsUploaded).errormessage = "Transport Pickup Person(s) Photo should be uploaded.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function CheckForSiblingDetails() {
            var isSibling = document.getElementById(_clienthidIsSibling).value;
            if (isSibling == "True")
                if (window.confirm('Do You want to save details for sibling?')) {
                    document.getElementById(_clienthidSaveSiblingDetails).value = 1;
                }
                else {
                    document.getElementById(_clienthidSaveSiblingDetails).value = 0;
                }
            return true;
        }

        function SubmitDataForSibling() {
            var isSibling = document.getElementById(_clienthidIsSibling).value;
            if (isSibling == "True")
                if (window.confirm('Do You want to submit details for sibling?')) {
                    document.getElementById(_clienthidSubmitSiblingDetails).value = 1;
                }
                else {
                    document.getElementById(_clienthidSubmitSiblingDetails).value = 0;
                }
            return true;
        }

        function CheckFileTypeForPhoto(sFamilyFileName) {
            var bIsValid;
            if (sFamilyFileName != "") {

                if (sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP") {

                    bIsValid = true;
                }
                else {
                    bIsValid = false;
                }
            }
            else {
                bIsValid = false;
            }
            return bIsValid;
        }       
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
