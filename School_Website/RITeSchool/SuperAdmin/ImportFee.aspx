<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    CodeFile="ImportFee.aspx.cs" Inherits="ImportFee" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                        <tr>
                            <td align="left" >
                                <asp:ValidationSummary ID="valErrorMsg" runat="server" CssClass="ClsLabel" />
                            </td>
                            <td align="right" colspan="3">
                                <div style="float: right">
                                    <asp:HyperLink ID="lnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank"
                                        ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip="Download the template for adding fees by template."></asp:HyperLink>
                                        <br />
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:Label ID="lblHead" runat="server" Text="Your file has been uploaded sucessfully."
                                                Visible="False" CssClass="ClsMdtStar" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                    <tr>
                                        <td align="center" colspan="6">
                                            <table border="0" cellpadding="0" cellspacing="3">
                                                <tr id="tr1" runat="server">
                                                    <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                        <%--<asp:Label ID="Label1" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 1 :"
                                                            CssClass="LblNrmlB" EnableViewState="False"></asp:Label>--%>
                                                            <span class="LblNrmlB" style="font-weight:bold">Note 1 :</span>
                                                    </td>
                                                    <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%" >
                                                        <%--<asp:Label ID="Label2" runat="server" BorderWidth="0px" CssClass="LblSmlV" 
                                                            EnableViewState="False">Students' fees is being paid for June, Term-I, Annual, July, August, September and October</asp:Label>--%>
                                                             <span class="LblSmlV" >Students' fees is being paid for June, Term-I, Annual, July, August, September and October.</span>
                                                    </td>
                                                </tr>
                                                <tr id="tr4" runat="server">
                                                    <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                        <%--<asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 2 :"
                                                            CssClass="LblNrmlB" EnableViewState="False"></asp:Label>--%>
                                                            <span class="LblNrmlB" style="font-weight:bold">Note 2 :</span>
                                                    </td>
                                                    <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%" >
                                                        <%--<asp:Label ID="Label10" runat="server" BorderWidth="0px" CssClass="LblSmlV" 
                                                            EnableViewState="False">Fee structure considered is as - Monthly - Rs. 1550, Term - Rs. 1550 and Annual - Rs. 2400</asp:Label>--%>
                                                             <span class="LblSmlV" >Fee structure considered is as - Monthly - Rs. 1550, Term - Rs. 1550 and Annual - Rs. 2400.</span>
                                                    </td>
                                                </tr>
                                                 <tr id="tr2" runat="server">
                                                    <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                        <%--<asp:Label ID="Label5" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 3 :"
                                                            CssClass="LblNrmlB" EnableViewState="False"></asp:Label>--%>
                                                            <span class="LblNrmlB" style="font-weight:bold">Note 3 :</span>
                                                    </td>
                                                    <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%" >
                                                        <%--<asp:Label ID="Label6" runat="server" BorderWidth="0px" CssClass="LblSmlV" 
                                                            EnableViewState="False">Import pre-primary and primary data separately.</asp:Label>--%>
                                                             <span class="LblSmlV" >Import pre-primary and primary data separately.</span>
                                                    </td>
                                                </tr>
                                                <tr id="tr3" runat="server">
                                                    <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                        <%--<asp:Label ID="Label7" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 4 :"
                                                            CssClass="LblNrmlB" EnableViewState="False"></asp:Label>--%>
                                                            <span class="LblNrmlB" style="font-weight:bold">Note 4 :</span>
                                                    </td>
                                                    <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%" >
                                                        <%--<asp:Label ID="Label8" runat="server" BorderWidth="0px" CssClass="LblSmlV" 
                                                            EnableViewState="False">Cheque pass date is also updated with the import.</asp:Label>--%>
                                                    
                                                     <span class="LblSmlV" >Cheque pass date is also updated with the import.</span>
                                                     </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="3" style="height: 3px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                
                                                    <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                                        <%--<asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Select File : " EnableViewState="False"></asp:Label>--%>
                                                        <span class="ClsLabel">Select File :</span> 
                                                    </td>
                                                    <td align="left" colspan="1" style="width:30%">
                                                        <asp:FileUpload ID="fileUploadStudents" runat="server" />
                                                        <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                            ControlToValidate="fileUploadStudents" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                            ErrorMessage="Invalid file type."></asp:CustomValidator>
                                                        <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <%--<asp:Label ID="Label3" runat="server" Text="  (Supports only .XLS/.XLSX files type)"
                                                            CssClass="LblSmlGray" EnableViewState="False"></asp:Label>--%>
                                                            <span class="LblSmlGray">(Supports only .XLS/.XLSX files type)</span> 
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button ID="btnImportFee" Text="Import Fee Details" runat="server" CssClass="ClsBtnLrg"
                                    BorderStyle="Solid" OnClick="btnImportCautionMoney_Click" Visible="True" CausesValidation="true"
                                    BorderWidth="1px" UseSubmitBehavior="false" />
                                <asp:Button ID="imgbtnBack" Text="Back" runat="server" CssClass="ClsBtnSml" BorderStyle="Solid"
                                    OnClick="imgbtnBack_Click" Visible="True" BorderWidth="1px" CausesValidation="false"
                                    UseSubmitBehavior="false" />
                            </td>
                        </tr>                        
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientFileUploadClientId = "<%=this.fileUploadStudents.ClientID%>"
        _clientbtnImportFee = "<%=this.btnImportFee.ClientID%>"
        _clientimgbtnBack = "<%=this.imgbtnBack.ClientID%>"
        _clientlblHead = "<%=this.lblHead.ClientID%>"
        _clientCustomValId = "<%=this.CstValFileType.ClientID%>"
        function ClearLabel() {
            if (document.getElementById(_clientlblHead)) {
                document.getElementById(_clientlblHead).innerText = ""
                document.getElementById(_clientlblHead).innerHTML = ""
            } 
        }
        function validateFile(source, args) {
            ClearLabel()
            var oFileName = document.getElementById(_clientFileUploadClientId).value
            var Extension = oFileName.toUpperCase().substring(oFileName.indexOf("."))
            var bIsValid = true
            if (oFileName != "") {
                if (oFileName.toUpperCase().indexOf(".XLS") == -1 && oFileName.toUpperCase().indexOf(".XLSX") == -1) {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage =
"File to upload should be in valid format."
                }
                else if (oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLS" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLSX") {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage =
"File to upload should be in valid format."
                } 
            }
            else {
                bIsValid = false
                document.getElementById(_clientCustomValId).errormessage =
"File to upload should be selected."
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnImportFee)) {
                var isPageValid = true
                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate()
                }
                if (isPageValid) {
                    document.getElementById(_clientbtnImportFee).disabled = true
                    document.getElementById(_clientimgbtnBack).disabled = true
                } 
            }
            else if (ObjBtn == document.getElementById(_clientimgbtnBack)) {
                document.getElementById(_clientbtnImportFee).disabled = true
                document.getElementById(_clientimgbtnBack).disabled = true
            } 
        }
    </script>
</asp:Content>
