<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ImportTransportAllocationUI.aspx.cs" Inherits="ImportTransportAllocationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
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
                                    <tr>
                                        <td align="right">
                                            <table style="font-weight:bold;">
                                                <tr>
                                                    <td><span>Vehicle Reading Allocation : </span>
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink ID="lnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank" ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip="Download Vehicle Reading Allocation template."></asp:HyperLink>
                                                    </td>
                                                    </tr>
                                                <tr>
                                                    <td><span>Vehicle Maintenance Expenses : </span>
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink ID="lnkDownloadMaintenance" runat="server" CssClass="CursorHand" Target="_blank" ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip="Download Vehicle Reading Allocation template."></asp:HyperLink>
                                                    </td>
                                                
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="valErrorMsg" runat="server" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Type should be selected." ControlToValidate="cmbAllocationType" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustValidateFile" runat="server" ClientValidationFunction="ValidateFile" CssClass="ClsLabel" ErrorMessage=""></asp:CustomValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td id="tdMessage" align="center">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true" Height="20px"
                                                Width="100%" Text="" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table>
                                                <tr>
                                                    <td align="center">
                                                        <table width="60%" align="center">
                                                            <tr align="center">
                                                                <td align="center" style="width: 300px;" class="ClsOnlyBorderlght">
                                                                    <span class="ClsLabel">Type :</span>
                                                                </td>
                                                                <td align="left" style="width: 10px;">
                                                                    <asp:DropDownList ID="cmbAllocationType" runat="server" CssClass="LrgCombo" AutoPostBack="false">
                                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                        <asp:ListItem Value="1">Reading Allocation</asp:ListItem>
                                                                        <asp:ListItem Value="2">Maintenance</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="center" class="ClsOnlyBorderlght">
                                                                    <span class="ClsLabel">Select File :</span>
                                                                </td>
                                                                <td align="left" colspan="2" style="width: 500px;">
                                                                    <asp:FileUpload ID="fileUploadAllocation" runat="server" />
                                                                    <span class="ClsMdtStar">*</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <span class="LblSmlGray">(Supports only .XLS/.XLSX files type) </span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnImport" CssClass="ClsBtn" Text="Import" runat="server" OnClick="btnImport_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clientFileUploadClientId = "<%=this.fileUploadAllocation.ClientID%>"
        _clientCustomValId = "<%=this.CustValidateFile.ClientID%>"

        function ValidateFile(source, args) {
            var oFileName = document.getElementById(_clientFileUploadClientId).value
            var Extension = oFileName.toUpperCase().substring(oFileName.indexOf("."))
            var bIsValid = true

            if (oFileName != "") {
                if (oFileName.toUpperCase().indexOf(".XLS") == -1 && oFileName.toUpperCase().indexOf(".XLSX") == -1) {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage = "Uploaded file should be in valid format."
                }
                else if (oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLS" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLSX") {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage = "Uploaded file should be in valid format."
                }
            }
            else {
                bIsValid = false
                document.getElementById(_clientCustomValId).errormessage = "File should be selected to upload."
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
