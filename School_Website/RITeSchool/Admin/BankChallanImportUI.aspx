<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BankChallanImportUI.aspx.cs" Inherits="BankChallanImportUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                        <tr>
                            <td align="right" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" colspan="4">
                                            <asp:ValidationSummary ID="valErrorMsg" runat="server" CssClass="ClsLabel" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:Label ID="lblHead" runat="server" Text="Your file has been uploaded sucessfully."
                                                Visible="False" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" CssClass="LblErrorMsg"
                                                Visible="false" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div style="float: right">
                                    <asp:HyperLink ID="lnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank"
                                        ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip="Download the template for adding teacher by template."></asp:HyperLink>
                                    <br />
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblError" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table align="center" style="text-align:center; margin:0px auto;">
                                    <tr align="center" style="text-align:center;">
                                        <td align="center" style="width:100%; padding-left:300px;">
                                            <table align="center" style="text-align:center; margin:0px auto;">
                                                <tr align="center">
                                                    <td align="center" class="ClsOnlyBorderlght">
                                                        <span class="ClsLabel">Fee Type :</span>
                                                    </td>
                                                    <td align="left" style="width: 10px;">
                                                          <asp:DropDownList ID="cmbFeeType" runat="server" CssClass="LrgCombo" AutoPostBack="false">
                                                          </asp:DropDownList> 
                                                          <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                                              ControlToValidate="cmbFeeType" runat="server" 
                                                              ErrorMessage="Fee type should be selected." Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </td>             
                                                </tr>
                                                <%--<tr>
                                                    <td style="width: 150px;" class="ClsBorderlight">
                                                        <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Payable For" Height="16px"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td align="left">                                                        
                                                        <asp:DropDownList ID="cmbPayableFor" runat="server" CssClass="LrgCombo">
                                                            <asp:ListItem Value="0" Text="-- Select --"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>                                                       
                                                        <asp:RequiredFieldValidator ID="reqPayableFor" 
                                                              ControlToValidate="cmbPayableFor" runat="server" 
                                                              ErrorMessage="Payable For should be selected." Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>--%>
                                                <tr align="center">
                                                     <td align="center" class="ClsOnlyBorderlght">
                                                        <span class="ClsLabel">Select File :</span>
                                                    </td>
                                                    <td align="left" colspan="2" style="width: 500px;">
                                                        <asp:FileUpload ID="fileUploadStudents" runat="server" />
                                                        <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                            ControlToValidate="fileUploadStudents" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                            ErrorMessage="Invalid file type."></asp:CustomValidator>
                                                        <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>
                                                    </td>
                                                </tr>                                                
                                                <td style="width:100px;"></td>
                                                    <td colspan="2" style="text-align:left; float:left;">
                                                        <span class="LblSmlGray">(Supports only .XLS/.XLSX files type)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>                                
                                <asp:Button ID="btnImportChallans" Text="Import" runat="server" CssClass="ClsBtnLrg"
                                    BorderStyle="Solid" Visible="True" BorderWidth="1px" OnClick="btnImportChallans_Click" UseSubmitBehavior="false" />
                                <asp:Button ID="imgbtnBack" Text="Back" runat="server" CssClass="ClsBtnSml" BorderStyle="Solid"
                                    PostBackUrl="~/RITeSchool/Accountant/StudentPayFeeUI.aspx" Visible="True" BorderWidth="1px"
                                    CausesValidation="false" UseSubmitBehavior="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                    <tr>
                                        <td align="left" style="width: 2px;">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                </table>
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
        _clientbtnImportChallans = "<%=this.btnImportChallans.ClientID%>"
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
                    document.getElementById(_clientCustomValId).errormessage = "File to upload should be in valid format."
                }
                else if (oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLS" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLSX") {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage = "File to upload should be in valid format."
                }
            }
            else {
                bIsValid = false
                document.getElementById(_clientCustomValId).errormessage = "File to upload should be selected."
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        
        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnImportChallans)) {
                var isPageValid = true
                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate()
                }
                if (isPageValid) {
                    document.getElementById(_clientbtnImportChallans).disabled = true
                    document.getElementById(_clientimgbtnBack).disabled = true
                }
            }
            else if (ObjBtn == document.getElementById(_clientimgbtnBack)) {
                document.getElementById(_clientbtnImportChallans).disabled = true
                document.getElementById(_clientimgbtnBack).disabled = true
            }
        }
    </script>
</asp:Content>
