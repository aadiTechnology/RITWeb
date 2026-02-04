<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="SchoolWebApp.FeedbackDetails" Codebehind="FeedbackDetails.ascx.cs" %>
<style type="text/css">
</style>
<table width="100%">
    <tr>
        <td>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="lblNormal"
                HeaderText="Please fix following error(s):" ShowMessageBox="False" ShowSummary="True"
                Height="70px"></asp:ValidationSummary>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                ForeColor="Blue" Visible="false" EnableViewState="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left">
            <table id="tblDescription" visible="false" runat="server" align="left" border="0"
                width="100%" class="ClsBorderlight">
                <thead >
                    <tr style="height: 20; background-color: #AAAAAA">
                        <td align="left">
                            <span class="ClsLabel" style="color: White; font-size: 15px; font-weight: bold">About
                                School and Software</span>
                        </td>
                    </tr>
                </thead>
                <tr>
                    <td style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblUser" runat="server" Font-Bold="True" Text="Dear User " CssClass="LblNormalImg"
                            EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span class="LblNormalImg">Thank you for using Software for</span>
                        <asp:Label ID="lblSchoolName" runat="server" Font-Bold="False" Text="" CssClass="LblNormalImg"
                            EnableViewState="false"></asp:Label>
                        <span class="LblNormalImg">You are a valued user and we are committed to provide the
                            best possible services that will fulfill the need of our users. Your valuable feedback
                            is very important to us which encourages us serve you more better! If you have any
                            suggestions related school or software, queries or even a testimonial you would
                            like to share, please submit below.</span>
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
        <td align="left">
            <table align="left" border="0" width="100%" class="ClsBorderlight" style="background-color: #F3F3F3">
                <thead align="left" style="background-color: #AAAAAA">
                    <tr>
                        <td align="left" colspan="4">
                            <span class="ClsLabel" style="color: White; font-weight: bold; font-size: 15px">Feedback</span>
                        </td>
                    </tr>
                </thead>
                <tr>
                    <td colspan="3" class="style1">
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">Feedback for :</span>
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="optlstFeedbackFor" runat="server" CssClass="ClsLabel" Width="83%"
                            RepeatDirection="Horizontal" Font-Bold="true">
                            <asp:ListItem Text="School" Value="School" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Software" Value="Software"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td align="right" style="width: 15%">
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">Type :</span>
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="optlstFeedbackType" runat="server" CssClass="ClsLabel" Width="100%"
                            RepeatDirection="Horizontal" Font-Bold="true">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">Name :</span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" MaxLength="100" 
                           ></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">E-mail :</span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtEmail" runat="server"  CssClass="ExLrgTxtBox" 
                            MaxLength="50"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">Comments :</span>
                    </td>
                    <td align="left" colspan="2" style="background-color: #F3F3F3">
                        <asp:TextBox ID="txtContent" runat="server" MaxLength="1000" TextMode="MultiLine"
                            CssClass="LrgTxtBox" Height="125px" Width="100%" ></asp:TextBox>
                    </td>
                    <td align="left">
                        <span class="ClsMdtStar">*</span>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px" colspan="3">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="center">
            <table border="0" cellpadding="0" align="center">
                <tbody>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSubmit" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                                Text="Submit Your Feedback" Visible="True" Width="150px" OnClick="btnSubmit_Click"
                                CausesValidation="true" />
                        </td>
                        <td align="center">
                            <asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                                CssClass="ClsBtnSml" Text="Cancel" Visible="True" OnClick="btnCancel_Click" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="reqValName" runat="server" ErrorMessage="Name should not be blank."
                                ControlToValidate="txtName" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation"
                                ControlToValidate="txtEmail" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="req_txtContent" runat="server" ControlToValidate="txtContent"
                                Display="None" ErrorMessage="Comments should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Reg_Expr_ValidContent" runat="server" Display="None"
                                ControlToValidate="txtContent" ErrorMessage="Comments should be of length less than 2000."
                                ValidationExpression="^[\s\S]{0,2000}$" CssClass="ClsLabel"> </asp:RegularExpressionValidator>
                        </td>
                        <asp:HiddenField ID="hidMode" runat="server" />
                        <asp:HiddenField ID="hidFeedbackId" runat="server" />
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
</table>
<script type="text/javascript" language="javascript">
    _sClientbtnSubmitId = "<%=this.btnSubmit.ClientID %>"
    _sClientlblMessageId = "<%=this.lblMessage.ClientID %>"
    _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>"
    _clienttxtEmailId = "<%=this.txtEmail.ClientID %>"
    _clientlblMessage = "<%=this.lblMessage.ClientID %>"

    function VisibleSuccessMsg() {
        if (document.getElementById(_sClientlblMessageId) != undefined) {
            document.getElementById(_sClientlblMessageId).style.display = "none"
        }
    }
    function EmailValidation(oSrc, args) {
        if (document.getElementById(_clientlblMessage)) {
            document.getElementById(_clientlblMessage).innerHTML = ""
            document.getElementById(_clientlblMessage).innerText = "";
        }
        var sEmail;
        sEmail = stripLeadingTrailingBlanks(document.getElementById(_clienttxtEmailId).value)
        if (isEmpty(sEmail)) {
            document.getElementById(_clientcstValEmailId).errormessage = "E-mail should not be blank."
            args.IsValid = false
            return true
        }
        else {
            if (!isEmail(sEmail)) {
                document.getElementById(_clientcstValEmailId).errormessage = "E-mail should be in valid format.(For Example : \" john.smith@yahoo.com \")"
                args.IsValid = false
                return true
            }
        }
        args.IsValid = true
        return false
    }
</script>
