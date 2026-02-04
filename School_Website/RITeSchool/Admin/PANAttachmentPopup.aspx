<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="PANAttachmentPopup.aspx.cs" Inherits="PANAttachmentPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="upnl" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                        <tr>
                                                            <td align="center" class="MainTitleHead" style="height: 20px">
                                                                <span style="font-weight: bold" id="spnTopHeader" runat="server"></span>
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                                    <asp:ValidationSummary ID="valSum" runat="server"/>
                                                </td>
                                                <td>
                                                    <div style="float: right; vertical-align: top;">
                                                        <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields</span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="80%">
                                            <tr>
                                                <td id="tdMessage" runat="server">
                                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <table width="80%">
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="150px">
                                                    <asp:Label ID="lblName" runat="server" Text="User Name:" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                                <td class="ClsHilightBGB">
                                                    <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Document Name :</span>
                                                </td>
                                                <td class="ClsHilightBGB">
                                                    <asp:Label ID="lblInvestmentMethod" runat="server" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel" id="spnHeader" runat="server"></span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPANNo" runat="server" CssClass="LrgTxtBox" MaxLength="29"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidatePANNo"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr id="trAadharCard" runat="server">
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Name on Aadhar Card :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNameonAadharCard" runat="server" CssClass="LrgTxtBox" MaxLength="29"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name on Aadhar Card should not blank."  Display="None" ControlToValidate="txtNameonAadharCard"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Upload Document :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:FileUpload ID="flDocument" runat="server" />
                                                    <asp:ImageButton ID="btnDownload" runat="server" CausesValidation="false" ToolTip="View Attachment" Visible="false"
                                                        CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                    <asp:ImageButton ID="btnDeleteImage" runat="server" CausesValidation="false" 
                                                        ToolTip="Delete" Visible="false"
                                                        CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" 
                                                        onclick="btnDeleteImage_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <span class="LblSmlGray" id="spnFileType" runat="server">(Attachment supports files of types - .BMP, .JPG,.JPEG, .PNG, .PDF
                                                        upto 1 MB.)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" OnClick="BtnSave_Click" />
                                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="ClsBtn" Visible="false"
                                                        onclick="btnDelete_Click" />
                                                    <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                                        OnClick="btnClose_Click" />
                                                    <asp:CustomValidator ID="cstFileType" runat="server" ErrorMessage="" ClientValidationFunction="ValidateFile"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnSave" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <asp:HiddenField ID="hidBtnState" runat="server" />
            <asp:HiddenField ID="hidDocumentTypeId" runat="server" />
            <asp:HiddenField ID="hidFileName" runat="server" Value="" />
        </table>
        <script type="text/javascript" language="javascript">

            _clienttxtPANNo = "<%=this.txtPANNo.ClientID %>"
            _clienthidDocumentTypeId = "<%=this.hidDocumentTypeId.ClientID %>"
            _clienthidFileName = "<%=this.hidFileName.ClientID %>"

            function ConfirmDelete() {
                var bResult = true
                if (!window.confirm('Are you sure you want to delete uploaded file?')) {
                    bResult = false
                }
                return bResult
            }

            function ConfirmAllDelete() {
                var bResult = true
                var msg = 'Are you sure you want to delete PAN Card details?'
                if (parseInt($('#' + _clienthidDocumentTypeId).val()) == 9)
                    msg = 'Are you sure you want to delete Aadhar Card details?'

                if (!window.confirm(msg)) {
                    bResult = false
                }
                return bResult
            }

            function ValidateFile(oSrc, args) {
                var fl = $get("<%=this.flDocument.ClientID %>").value;

                if (fl != "") {
                    if (!(fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".JPG" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".JPEG" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".BMP" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".PDF" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".PNG"
                    )) {
                        oSrc.errormessage = "Please select valid file type.";
                        args.IsValid = false;
                        return true;
                    }
                }

                args.IsValid = true;
                return false;
            }

            function ResetMessage() {
                if ($get("<%=this.lblMessage.ClientID %>") != null)
                    $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
            }

            function ValidatePANNo(oSrc, args) {
                var PANNo = $('#' + _clienttxtPANNo).val();
                if (PANNo.trim() == "") {
                    if (parseInt($('#' + _clienthidDocumentTypeId).val()) == 9)
                        oSrc.errormessage = "Aadhar Card No. should not be Blank.";
                    else
                        oSrc.errormessage = "PAN No. should not be Blank.";
                    
                    args.IsValid = false
                    return true
                }

                args.IsValid = true
                return false
            }

            function CloseWindow(ItemCount) {

                window.opener.UpdateFileUploadCount(ItemCount);
                window.close();
                window.opener.focus();
            }

            function CloseWindow() {
                window.close();
                window.opener.focus();
                window.opener.FilterPANDetails();
            }


            function ClosePerformanceWindow(Count, ClientId) {
                window.opener.focus();
                window.opener.RefreshLinkButton(Count, ClientId);
                window.close();
            }
        </script>
    </div>
</asp:Content>
