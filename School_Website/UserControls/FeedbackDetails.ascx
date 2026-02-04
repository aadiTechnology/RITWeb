<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FeedbackDetails.ascx.cs"
    Inherits="FeedbackDetails" %>
<style type="text/css">
    
</style>
<table width="1250px">
    <tr>
        <td width="100%">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="lblNormal"
                HeaderText="Please fix following error(s):" ShowMessageBox="False" ShowSummary="True"
                Height="70px"></asp:ValidationSummary>
        </td>
    </tr>
    <tr>
        <td align="center" width="100%">
            <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                ForeColor="Blue" Visible="false" EnableViewState="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" width="100%">
            <table id="tblDescription" visible="false" runat="server" align="left" border="0"
                width="1250px" class="ClsBorderlight">
                <thead>
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
        <td align="left" width="100%">
            <table align="left" border="0" width="1250px" class="ClsBorderlight" style="background-color: #F3F3F3">
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
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3; width: 245px">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">Feedback for :</span>
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="optlstFeedbackFor" runat="server" CssClass="ClsLabel" Width="83%"
                            RepeatDirection="Horizontal" Font-Bold="true" OnSelectedIndexChanged="optlstFeedbackFor_SelectedIndexChanged"
                            AutoPostBack="True">
                            <asp:ListItem Text="School" Value="School" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Software" Value="Software"></asp:ListItem>
                        </asp:RadioButtonList>
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
        <td width="100%">
            <table id="tblFeedbackControls" runat="server" align="left" border="0" width="1250px"
                class="ClsBorderlight" style="background-color: #F3F3F3">
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3; width: 245px">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">Type :</span>
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="optlstFeedbackType" runat="server" CssClass="ClsLabel" Width="950px"
                            RepeatDirection="Horizontal" Font-Bold="true">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3; width: 245px">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">Name :</span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3; width: 245px">
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">E-mail :</span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                    </td>
                </tr>                
            </table>
        </td>
    </tr>
    <tr >
        <td width="100%" colspan="4">
            <table width="1250px" class="ClsBorderlight" style="background-color: #F3F3F3" id="tdtxtComments" runat="server">
                <tr>
                    <td align="left" class="ClsBorderlight" style="background-color: #F3F3F3; width: 245px" >
                        <span class="ClsLabel" style="width: 100%; font-weight: bold">Comments :</span>
                    </td>
                    <td align="left" colspan="3" style="background-color: #F3F3F3; white-space: nowrap;" width="80%">
                        <asp:TextBox ID="txtContent" runat="server" MaxLength="1000" TextMode="MultiLine"
                            CssClass="LrgTxtBox" Height="125px" Width="100%"></asp:TextBox>
                        <span class="ClsMdtStar">*</span>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr >
        <td width="100%" colspan="4">
            <table width="1250px" id="trSoftwareFeedbackNote" runat="server">
                <tr>
                    <td align="left" class="ClsBorderlight " style="background-color: #ffffc4; width: 245px;">
                        <asp:Label ID="lblNote1" runat="server" class="LblNrmlB" Style="font-weight: bold"
                            EnableViewState="false" Text="<%$ Resources:LocalizedResources, Note%>"></asp:Label>
                        <span class="colonPadding">:</span>
                    </td>
                    <td colspan="3" align="left" class="ClsBorderlight" style="padding-left: 5px; width: 96%">
                        <asp:Label ID="lblSoftwareFeedbackNote" runat="server" BorderWidth="0px" Text="On click of ‘Submit Your Feedback’ button, you will be redirected to another link, where you can give detailed feedback about software."
                            CssClass="LblSmlV"></asp:Label>
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
                                CssClass="ClsBtnSml" Text="Clear" Visible="True" OnClick="btnCancel_Click" />
                        </td>
                        <td align="center">
                            <asp:Button runat="server" Text="Close" ID="btnClose" BorderStyle="Solid" BorderWidth="1px"
                                CausesValidation="false" CssClass="ClsBtnSml" OnClientClick="javascript:HidePopup2();return false;" />
                        </td>
                        <td>
                            <asp:CustomValidator ID="cstName" runat="server" ClientValidationFunction="NameValidation"
                                Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstValContent" runat="server" ClientValidationFunction="NameValidation"
                                Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation"
                                ControlToValidate="txtEmail" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstValidContents" runat="server" ClientValidationFunction="ContentValidation"
                                ControlToValidate="txtContent" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                        <asp:HiddenField ID="hidMode" runat="server" />
                        <asp:HiddenField ID="hidFeedbackId" runat="server" />
                        <asp:HiddenField ID="HidIsStudentLogin" runat="server" Value="0" />
                        <asp:HiddenField ID="hidSoftwareFeedbackURL" runat="server" />
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
    _clientLoginUserRole = "<%=this.HidIsStudentLogin.ClientID %>"
    _clienthidSoftwareFeedbackURL = "<%=this.hidSoftwareFeedbackURL.ClientID %>"
    _clientcstValName = "<%=this.cstName.ClientID %>"
    _clienttxtName = "<%=this.txtName.ClientID %>"
    _clientcstValContents = "<%=this.cstValidContents.ClientID %>"
    _clienttxtContent = "<%=this.txtContent.ClientID %>"

    function NameValidation(oSrc, args) {
        if (GetFeedbackType()) {
            args.IsValid = true
            return false
        }

        var sName;
        var sContents;
        sName = stripLeadingTrailingBlanks(document.getElementById(_clienttxtName).value)
        sContents = stripLeadingTrailingBlanks(document.getElementById(_clienttxtContent).value)
        if (isEmpty(sName)) {
            document.getElementById(_clientcstValName).errormessage = "Name should not be blank."
            args.IsValid = false
            return true
        }
        if (isEmpty(sContents)) {
            document.getElementById(_clientcstValContents).errormessage = "Comments should not be blank."
            args.IsValid = false
            return true
        }
        else {
            if (_clientcstValContents.length > 2000) {
                document.getElementById(_clientcstValEmailId).errormessage = "Comments should be of length less than 2000."
                args.IsValid = false
                return true
            }
        }
        args.IsValid = true
        return false
    }

    function ContentValidation(oSrc, args) {
        if (GetFeedbackType()) {
            args.IsValid = true
            return false
        }

        var sContent;
        sContent = stripLeadingTrailingBlanks(document.getElementById(_clienttxtContent).value)
        if (isEmpty(sContent)) {
            document.getElementById(_clientcstValContents).errormessage = "Comments should not be blank."
            args.IsValid = false
            return true
        }
        else if (_clientcstValContents.length > 2000) {
            document.getElementById(_clientcstValEmailId).errormessage = "Comments should be of length less than 2000."
            args.IsValid = false
            return true
        }
        args.IsValid = true
        return false
    }

    function VisibleSuccessMsg() {
        if (document.getElementById(_sClientlblMessageId) != undefined) {
            document.getElementById(_sClientlblMessageId).style.display = "none"
        }
    }
    function EmailValidation(oSrc, args) {

        if (GetFeedbackType()) {
            args.IsValid = true
            return false
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

    function OpenSoftwareFeedbackPopUp() {
        VisibleSuccessMsg();
        var FeedbackFor = $('#<%=optlstFeedbackFor.ClientID %> input[type=radio]:checked').val();
        if (FeedbackFor == 'Software') {
            window.open(document.getElementById(_clienthidSoftwareFeedbackURL).value, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700');
        }
    }

    SetFeedbackViewAsPerRole();

    function SetFeedbackViewAsPerRole() {
        var FeedbackFor = $('#<%=optlstFeedbackFor.ClientID %> input[type=radio]:checked').val();
        var sUserRole = document.getElementById(_clientLoginUserRole).value;
        if (sUserRole == '1') {
            if (FeedbackFor == 'Software') {
                document.getElementById("<%=this.tblFeedbackControls.ClientID %>").style.display = 'none';
                document.getElementById("<%=this.trSoftwareFeedbackNote.ClientID %>").style.display = 'block';
                document.getElementById("<%=this.tdtxtComments.ClientID %>").style.display = 'none';
                document.getElementById("<%=this.btnCancel.ClientID %>").style.display = 'none';
            }
            else {
                document.getElementById("<%=this.tblFeedbackControls.ClientID %>").style.display = 'block';
                document.getElementById("<%=this.trSoftwareFeedbackNote.ClientID %>").style.display = 'none';             
                document.getElementById("<%=this.tdtxtComments.ClientID %>").style.display = 'block';
                document.getElementById("<%=this.btnCancel.ClientID %>").style.display = 'block';
            }
        }
        else
            document.getElementById("<%=this.trSoftwareFeedbackNote.ClientID %>").style.display = 'none';
    }

    function GetFeedbackType() {
        var FeedbackFor = $('#<%=optlstFeedbackFor.ClientID %> input[type=radio]:checked').val();
        var sUserRole = document.getElementById(_clientLoginUserRole).value;
        if (sUserRole == '1') {
            if (FeedbackFor == 'Software')
                return true;
        }
        return false;
    }
</script>
