<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadStudentPhotoUI.aspx.cs" Inherits="UploadStudentPhotoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <div style="float: right; vertical-align: top;">
                                                <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <tr>
                                    <td id="tdMessage" align="center">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true" Text=""
                                            EnableViewState="false" CssClass="LblNormal"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <table width="50%">
                                            <tr>
                                                <td align="center" class="ClsBorderlight">
                                                    <asp:Label ID="lblName" runat="server" Text="Student Name :" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                                <td id="Td1" align="left" runat="server" class="ClsHilightBGB">
                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:Label CssClass="ClsLabel" ID="Label1" runat="server" EnableViewState="False"
                                                        Text="Existing Photo"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                                <td align="left" colspan="1" valign="middle">
                                                    <img id="imgExistingPhoto" alt="image" runat="server" height="151" width="119" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:Label CssClass="ClsLabel" ID="lblPhoto" runat="server" EnableViewState="False"
                                                        Text="New Photo"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                                <td align="left" colspan="1" valign="middle">
                                                    <asp:Image ID="imgPhoto" runat="server" Height="151" Width="119" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblUploadPhoto" CssClass="ClsLabel" runat="server" EnableViewState="False"
                                                        Text="Upload_CapturePhoto"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:FileUpload ID="FileUploadLogo" runat="server" />
                                                                <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ClientValidationFunction="ValidateFile"
                                                                    ErrorMessage="InvalidFileFormat" CssClass="LblErrorMsg"></asp:CustomValidator>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                            <td>
                                                                <img id="ImgWebCam" title="CapturePhoto" runat="server" style="cursor: pointer;"
                                                                    src="../images/WebCam.png" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblNote" runat="server" CssClass="ClsLabel" Text="Note: The student photo to be uploaded should be in school uniform."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="2">
                                                    <span class="LblSmlGray">
                                                        <asp:Label ID="lblUploadImage" runat="server" EnableViewState="False" Text="Upload or Capture an image file for student's photo"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lblUploadHeight" runat="server" EnableViewState="False" Text="(Max Height: 151px and Max Width: 112px)"></asp:Label><br />
                                                        <asp:Label ID="lblUploadSize" runat="server" EnableViewState="False" Text="(Image size should not exceed 1 mb. Supported file formats are JPG, JPEG, PNG, BMP)"></asp:Label></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" OnClick="BtnSave_Click" />
                                        <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                            OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                            OnClick="btnSubmit_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidStudentId" runat="server" />
                                        <asp:HiddenField ID="hidIsPhotoCaptured" runat="server" />
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">

            _clientFileUploadLogo = "<%=this.FileUploadLogo.ClientID%>";
            _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete new photo?')
            }

            function ConfirmSubmit() {
                return confirm('After Submit, you will not be able to change photo. Do you want to continue?')
            }

            function ResetMessage() {
                if ($get("<%=this.lblMessage.ClientID %>") != null)
                    $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
            }

            function ValidateFile(oSrc, args) {
                var _clientFileUploadLogo = "<%=this.FileUploadLogo.ClientID %>"
                if ($get(_clientFileUploadLogo) != null) {
                    var fl = $get(_clientFileUploadLogo).value;

                    if (fl == '' && $get(_clienthidIsPhotoCaptured).value != "Y") {
                        oSrc.errormessage = "Please select File to Upload.";
                        args.IsValid = false;
                        return true;
                    }
                    else if (fl != "") {
                        var file = $get("<%=this.FileUploadLogo.ClientID %>")
                        if (!(fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPEG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PNG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP"
                                )) {
                            oSrc.errormessage = "Please select valid file type to upload Student photo.";
                            args.IsValid = false;
                            return true;
                        }
                        else if (file.files[0].size >= 1048576) {
                            oSrc.errormessage = "Photo file size should not be more than 1 MB."
                            args.IsValid = false
                            return true
                        }
                    }
                }

                args.IsValid = true;
                return false;
            }

            function OpenWebcamPopup(sQueryString) {
                window.open('../Common/WebcamNewPopup.aspx?' + sQueryString, 'mywindow', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=400').focus();
                return true;
            }

            function UpdateHiddenField() {
                $get(_clienthidIsPhotoCaptured).value = "Y";
            }

        </script>
    </div>
</asp:Content>
