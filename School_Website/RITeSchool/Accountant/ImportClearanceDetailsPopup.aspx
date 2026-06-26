<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportClearanceDetailsPopup.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master" Inherits="ImportClearanceDetailsPopup" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top">
        <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <span class="MainTitleHead" style="font-weight: bold">Import Clearance Details</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="Please fix following error(s)"
                        runat="server" ValidationGroup="Import" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" Visible="false"
                        EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblSuccessMsg" runat="server" CssClass="LblNrmlB" ForeColor="Blue"
                        Visible="false" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table align="center" border="0" cellpadding="0" cellspacing="3">
                        <tr>
                            <td align="left" class="ClsBorderlight" style="width: 100px">
                                <span class="ClsLabel">Select File : </span>
                            </td>
                            <td align="left">
                                <asp:FileUpload ID="fileUploadClearance" runat="server" />
                                <asp:CustomValidator ID="cstValFileType" runat="server" ClientValidationFunction="validateFile"
                                    ControlToValidate="fileUploadClearance" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                    ErrorMessage="Invalid file type." ValidationGroup="Import"></asp:CustomValidator>
                                <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblFileType" runat="server" CssClass="LblSmlGray" Text="(Supports only .XLS/.XLSX files type)"
                                    EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnImport" runat="server" Text="Import" CssClass="ClsBtn" OnClick="btnImport_Click"
                                    ValidationGroup="Import" CausesValidation="true" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidValFileUpload" runat="server" Value="Please select file to import." />
        <asp:HiddenField ID="hidValFileUploadType" runat="server" Value="File should be only in xls or xlsx format." />
    </div>
    <script language="javascript" type="text/javascript">
        var _clientFileUploadClientId = "<%= fileUploadClearance.ClientID %>";
        var _clientCustomValId = "<%= cstValFileType.ClientID %>";

        function validateFile(source, args) {
            var oFileName = document.getElementById(_clientFileUploadClientId).value;
            var bIsValid = true;

            if (oFileName != "") {
                var sExtension = oFileName.substring(oFileName.lastIndexOf(".")).toUpperCase();
                if (sExtension != ".XLS" && sExtension != ".XLSX") {
                    bIsValid = false;
                    source.errormessage = document.getElementById("<%= hidValFileUploadType.ClientID %>").value;
                }
            }
            else {
                bIsValid = false;
                source.errormessage = document.getElementById("<%= hidValFileUpload.ClientID %>").value;
            }

            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CloseWindow() {
            window.close(); 
            window.opener.focus();
        }

    </script>
</asp:Content>
