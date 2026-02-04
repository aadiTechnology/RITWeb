<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SupportUI.aspx.cs" Inherits="SupportUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 97%">
                        <tr>
                            <td align="left" colspan="4" valign="top">
                                <table border="0" width="100%">
                                    <tr>
                                        <td align="left">
                                            <table width="100%">
                                                <tr id="trlblErrorMsg" runat="server" visible="false">
                                                    <td align="left">
                                                        <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="valsum" runat="server" HeaderText="Please fix following error(s):"
                                                            ShowMessageBox="False" ShowSummary="True" CssClass="ClsLabel" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trlblMessage" runat="server" visible="false">
                                        <td align="center">
                                            <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                ForeColor="Blue" Visible="false" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <table align="left" border="0" width="100%" class="ClsBorderlight">
                                                <thead style="height: 20; background-color: #AAAAAA">
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                                <span class="ClsLabel" style="color:White;font-size:15px;font-weight:bold">Support Request</span>
                                                        </td>
                                                    </tr>
                                                </thead>
                                                <tr>
                                                    <td style="height: 5px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblUser" runat="server" Font-Bold="True" Text="Dear " 
                                                            CssClass="LblNormalImg" EnableViewState="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 5px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                            <span class="LblNormalImg">Mention the Subject for your Support Request and Description of the problem in detail with exact steps if possible. You may attach a file as a supporting document. It will help our support member to understand the problem in full and speed up resolution of your request.</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 5px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table align="left" border="0" width="100%" class="ClsBorderlight" style="background-color: #F3F3F3">
                                                <thead align="left" style="height: 20; background-color: #AAAAAA">
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                                <span class="ClsLabel" style="color:White;font-size:15px;font-weight:bold">Write to Us</span>
                                                        </td>
                                                    </tr>
                                                </thead>
                                                <tr>
                                                    <td style="height: 5px" colspan="3">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" style="background-color: #F3F3F3" width="150px">
                                                            <span class="ClsLabel" style="font-weight:bold">E-mail Address :</span>
                                                        &nbsp;
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"
                                                            TabIndex="1"></asp:TextBox>
                                                        <span class="ClsMdtStar">*</span>
                                                        <asp:RequiredFieldValidator ID="req_Email" runat="server" ControlToValidate="txtEmail"
                                                            Display="None" ErrorMessage="Email Address should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="reg_Email" runat="server" ControlToValidate="txtEmail"
                                                            Display="None" ErrorMessage="E-mail should be in valid format.(For Example : john.smith@yahoo.com)"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="ClsLabel"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td align="right" style="width: 15%">
                                                            <span class="ClsMdtStar">* Mandatory Fields</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3">
                                                            <span class="ClsLabel" style="font-weight:bold">Mobile Number :</span>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtPhone" CssClass="MidTxtBox" runat="server" MaxLength="10" onblur="extractNumber(this,0,false);"
                                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" TabIndex="2" />
                                                        <asp:CustomValidator ID="cst_MobileNumber" runat="server" ControlToValidate="txtPhone"
                                                            Display="None" ClientValidationFunction="MobileNumberValidation" CssClass="ClsLabel"></asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="Req_txtProbSub" runat="server" ControlToValidate="txtProbSub"
                                                            Display="None" ErrorMessage="Problem Subject should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" style="background-color: #F3F3F3">
                                                            <span class="ClsLabel" style="font-weight:bold">Problem Subject :</span>
                                                        &nbsp;
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtProbSub" runat="server" MaxLength="200" CssClass="LrgTxtBox"
                                                            Width="98%" TabIndex="3"></asp:TextBox>
                                                        <span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" style="background-color: #F3F3F3">
                                                            <span class="ClsLabel" style="font-weight:bold">Description :</span>
                                                    </td>
                                                    <td colspan="2" style="background-color: #F3F3F3">
                                                        <asp:TextBox ID="txtProblem" runat="server" MaxLength="1000" TextMode="MultiLine"
                                                            CssClass="LrgTxtBox" Height="100px" Width="98%" TabIndex="4"></asp:TextBox>
                                                        <span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" style="background-color: #F3F3F3">
                                                            <span class="ClsLabel" style="font-weight:bold">Attachment :</span>&nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:FileUpload ID="File_attatchment" runat="server" CssClass="ExLrgTxtBox" TabIndex="5" />
                                                        <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                            ControlToValidate="File_attatchment" CssClass="ClsLabel" Display="None" ValidateEmptyText="false"
                                                            ErrorMessage="Invalid file type."></asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="req_txtProblem" runat="server" ControlToValidate="txtProblem"
                                                            Display="None" ErrorMessage="Problem Description should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="Reg_Expr_ValiProblem" runat="server" Display="None"
                                                            ControlToValidate="txtProblem" ErrorMessage="Problem Description should be of length less than 2000."
                                                            ValidationExpression="^[\s\S]{0,2000}$" CssClass="ClsLabel"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1">
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="1" style="background-color: #F3F3F3">
                                                            <span class="ClsLabel"> (Supports only .XLS, .XLSX, .DOC, .DOCX, .PDF, .JPG, .JPEG files types upto 200 KB)</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnSubmit" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                                                OnClick="btnSubmit_Click" Text="Submit" Visible="True" TabIndex="6" />
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
        _clientCustomValId = "<%=this.CstValFileType.ClientID%>"
        _clientFileUploadClientId = "<%=this.File_attatchment.ClientID%>"
        _clientPhoneId = "<%=this.txtPhone.ClientID%>"
        _clientMobileCustomValId = "<%=this.cst_MobileNumber.ClientID%>"
        _sClientlblMessageId = "<%=this.lblMessage.ClientID %>"
        function VisibleSuccessMsg() {
            if (document.getElementById(_sClientlblMessageId) != undefined) {
                document.getElementById(_sClientlblMessageId).style.display = "none"
            } 
        }
        function validateFile(source, args) {
            var oFileName = document.getElementById(_clientFileUploadClientId).value
            var bIsValid = true
            if (oFileName != "") {
            	if (oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".XLS" || oFileName.substr(oFileName.lastIndexOf('.'), 5).toUpperCase() == ".XLSX" || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".DOC" || oFileName.substr(oFileName.lastIndexOf('.'), 5).toUpperCase() == ".DOCX" || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF" || oFileName.substr(oFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || oFileName.substr(oFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG") {
                    bIsValid = true
                }
                else {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage = "Invalid file format."
                    if ($get("<%=this.lblErrorMsg.ClientID %>") != null)
                        $get("<%=this.lblErrorMsg.ClientID %>").innerHTML = "";

                } 
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        function MobileNumberValidation(oSrc, args) {
            var sMobileNumber = document.getElementById(_clientPhoneId).value
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)
            if (sMobileNumber.length < 10) {
                document.getElementById(_clientMobileCustomValId).errormessage = "Mobile Number should be of 10 digits."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
    </script>
</asp:Content>
