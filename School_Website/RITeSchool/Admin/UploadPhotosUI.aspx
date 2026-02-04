<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadPhotosUI.aspx.cs" Inherits="UploadPhotosUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
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
                        </td>
                    </tr>
                    <tr>
                        <td id="tdMessage" runat="server" align="center">
                            <asp:Label ID="lblMessage" runat="server" Height="20px" Width="100%" CssClass="ClsLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="text-align: center;">
                            <table align="center" style="text-align: center; margin: 0px auto; width: 50%;">
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span10" class="ClsLabel">Name :</span><span id="Span11" style="color: Red;
                                            display: none;"></span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblTeacherName" runat="server" Height="20px" Width="100%" Visible="true"
                                            EnableViewState="False" BackColor="Transparent"></asp:Label>
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
                                                        <img id="imgTeacherPhoto" alt="image" runat="server" height="151" width="119" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span id="Span13" class="ClsLabel">Upload Scan Copy of Photo :</span>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="FuTeacherPhoto" CssClass="LrgTxtBox" runat="server" Width="225px" />
                                        <asp:CustomValidator ID="cstValidateTeacherPhoto" Display="None" runat="server" ClientValidationFunction="ValidateTeacherPhoto"
                                            ErrorMessage="" ValidationGroup="Save"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" class="ClsBorderlight" style="font-weight: bold;">                                        
                                        <span class="LblSmlGray"> Upload or Capture photo for selected user(s). (Max Height: 151px and Max Width: 112px).<br />
                                                (Image size should not exceed 250 kb. Supported file formats are JPG, JPEG)</span>
                                    </td>
                                </tr>
                                <tr align="center" style="text-align: center; margin: 0px auto;">
                                    <td align="center" style="text-align: center; width: 100%;" colspan="2">
                                        <asp:Button CssClass="ClsBtn" ID="btnSubmit" runat="server" ViewStateMode="Enabled"
                                            Text="Submit" BorderWidth="1px" ValidationGroup="Save" onclick="btnSubmit_Click">
                                        </asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        _clientFuTeacherPhoto = "<%=this.FuTeacherPhoto.ClientID %>";
        
        function ConfirmSubmit() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function')
                isPageValid = Page_ClientValidate()

            if (isPageValid)
                return confirm('After this action you will not be able to change your photo from this screen again. Do you want to continue?')
            else
                return false;
        }

        function ValidateTeacherPhoto(oSrc, args) {        
            var myImage = new Image();
            myImage.src = document.getElementById(_clientFuTeacherPhoto).value;
            
            if ((document.getElementById(_clientFuTeacherPhoto).value == "")) {
                oSrc.errormessage = "User photo should be uploaded.";
                args.IsValid = false;
                return true;
            }
            else {                
                var sFamilyFileName = myImage.src
                if (sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP")
                {
                    if (document.getElementById(_clientFuTeacherPhoto).files[0].size > 81920) {
                        oSrc.errormessage = "Photo file size should not be greater than 80kb.";
                        args.IsValid = false;
                        return true;
                    }
                }
                else//if file type is not valid
                {
                    oSrc.errormessage = "Photo file type should be from .jpg, .jpeg, .png and .bmp.";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
