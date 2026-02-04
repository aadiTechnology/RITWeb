<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlineEnquiryFormUI.aspx.cs"
    Inherits="OnlineEnquiryFormUI" %>
<%--<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enquiry Form</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body
        {
            font-family: Arial, sans-serif;
            margin: 20px;
            padding: 0;
            background-color: #f5f5f5;
        }
        
        .form-container
        {
            max-width: 600px;
            margin: auto;
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        
        h2
        {
            text-align: center;
            margin-bottom: 20px;
        }
        
        .form-group
        {
            margin-bottom: 15px;
            display: flex;
            flex-direction: column;
        }
        
        label
        {
            font-weight: bold;
            margin-bottom: 5px;
        }
        
        input[type="text"], input[type="email"], textarea
        {
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            font-size: 14px;
        }
        
        textarea
        {
            resize: vertical;
        }
        
        .submit-btn
        {
            background-color: #007ACC;
            color: white;
            border: none;
            padding: 12px;
            width: 100%;
            font-size: 16px;
            border-radius: 4px;
            cursor: pointer;
        }
        
        .submit-btn:hover
        {
            background-color: #005f99;
        }
        
        @media (max-width: 600px)
        {
            .form-container
            {
                padding: 15px;
            }
        
            .submit-btn
            {
                font-size: 14px;
            }
        }
        
        select.form-control
        {
            appearance: none; /* Remove default arrow styling */
            -webkit-appearance: none;
            -moz-appearance: none;
            padding: 10px;
            font-size: 14px;
            border: 1px solid #ccc;
            border-radius: 4px;
            background-color: #fff;
            background-image: url('data:image/svg+xml;utf8,<svg fill="%23666" height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M7 10l5 5 5-5z"/></svg>');
            background-repeat: no-repeat;
            background-position: right 10px center;
            background-size: 16px 16px;
            cursor: pointer;
        }
        /* Ensure full width and responsiveness */select.form-control:focus
        {
            outline: none;
            border-color: #007ACC;
            box-shadow: 0 0 3px rgba(0, 122, 204, 0.5);
        }
        
        .name-row
        {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
        }
        
        .name-input
        {
            flex: 1;
            min-width: 100px;
        }
        
        /* Optional for better mobile experience */
        @media (max-width: 600px)
        {
            .name-row
            {
                flex-direction: column;
            }
        }
        
        .gender-options
        {
            display: flex;
            gap: 20px;
            flex-wrap: wrap;
            padding-top: 5px;
        }
        .gender-radio
        {
            display: flex;
            align-items: center;
            font-size: 14px;
            gap: 5px;
        }
        
        .mandatory
        {
            background-color: #FFFFE0;
        }
        
        .clsLabel, .ClsLabel
        {
            font-family: Open Sans;
            font-size: 14px;
        }
        
        /* Modal background overlay */
        .modal-overlay
        {
            display: none; /* Hidden by default */
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.5);
            z-index: 999;
        }
        
        /* Modal box */
        .modal-box
        {
            background-color: #fff;
            width: 400px;
            margin: 15% auto;
            padding: 20px 0px;
            border-radius: 5px;
            text-align: center;
            position: relative;
        }
        
        /* Close button */
        .close-btn
        {
            margin-top: 20px;
            padding: 8px 20px;
            background-color: #4CAF50;
            color: white;
            border: none;
            cursor: pointer;
        }
        
        .close-btn:hover
        {
            background-color: #45a049;
        }
        
        .center-image
        {
            display: block;
            margin-left: auto;
            margin-right: auto;
            max-width: 100%; /* Responsive */
            height: auto;
        }
    </style>
    <asp:PlaceHolder ID="phScripts" runat="server"></asp:PlaceHolder>
    
    <script type="text/javascript">
        function showModal() {
            document.getElementById('customModal').style.display = 'block';
        }

        function closeModal() {
            document.getElementById('customModal').style.display = 'none';
            window.location.href = window.location.href;
        }
    </script>
    <script src="../Scripts/Validations.js" type="text/javascript"></script>
    <script src="../Scripts/validate2.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <%--<input type="hidden" id="g-recaptcha-token" name="g-recaptcha-token" />
    <asp:HiddenField ID="hidCaptData" runat="server" Value="" />--%>
    <div class="form-container">
        <div class="form-group" style="margin-bottom:0px;">
            <asp:Image ID="img" runat="server" ImageUrl="~/RITeSchool/images/Logos/School_Logo.bmp"
                Height="125px" Width="100px" CssClass="center-image"></asp:Image>
        </div>
        <div class="form-group">
            <label id="lblSchoolName" runat="server" style="text-align: center; font-size: 30px;">
            </label>
        </div>
        <div style="width: 100%;">
            <hr style="width: 100%;" />
        </div>
        <div class="form-group">
            <div style="float: right;">
                <span style="float: right; font-size: small; color: Red;">Note : Fields with yellow
                    background are mandatory.</span>
            </div>
        </div>
        <h2>
            <u>Admission Enquiry Form</u></h2>
        <div class="form-group">
            <asp:ValidationSummary ID="valSum" runat="server" DisplayMode="BulletList" ShowMessageBox="false"
                CssClass="ClsLabel" ShowSummary="true" />
            <asp:CompareValidator ID="cmp_valYear" runat="server" ControlToValidate="cmbYear"
                Display="None" ErrorMessage="Academic Year should be selected." Operator="NotEqual"
                ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
            <asp:CompareValidator ID="cmp_valStdr" runat="server" ControlToValidate="cmbStd"
                Display="None" ErrorMessage="'Grade/Std. Applying for' should be selected." Operator="NotEqual"
                ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="reqSName" runat="server" ErrorMessage="Student First Name should not be blank."
                Display="None" ControlToValidate="txtStudFirstName"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Gender should be selected."
                Display="None" ControlToValidate="cmbGender" InitialValue="0"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="DOB should not be blank."
                Display="None" ControlToValidate="txtCalDobPopup"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvPastDate" runat="server" ControlToValidate="txtCalDobPopup"
                ErrorMessage="DOB should be in the past." OnServerValidate="DOB_ServerValidate"
                ClientValidationFunction="validatePastDate" Display="none" ForeColor="Red">
            </asp:CustomValidator>
            <asp:CustomValidator ID="CustomValidator4" Display="None" runat="server" CssClass="ClsMdtStar"
                Visible="true" ErrorMessage="" EnableClientScript="true" ClientValidationFunction="ValidateAadharCardNo"></asp:CustomValidator>
            <asp:RequiredFieldValidator ID="reqFName" runat="server" ErrorMessage="Father's First Name should not be blank."
                Display="None" ControlToValidate="txtFFirstName"> </asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                Visible="true" ErrorMessage="" EnableClientScript="true" ClientValidationFunction="FatherMobileNumberValidation"></asp:CustomValidator>
            <asp:CustomValidator ID="CustomValidator3" Display="None" runat="server" CssClass="ClsMdtStar"
                Visible="true" ErrorMessage="" EnableClientScript="true" ClientValidationFunction="ValidateFatherMobileNo2"></asp:CustomValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Mother's First Name should not be blank."
                Display="None" ControlToValidate="txtMFirstName"> </asp:RequiredFieldValidator>
            <asp:CustomValidator ID="CustomValidator1" Display="None" runat="server" CssClass="ClsMdtStar"
                Visible="true" ErrorMessage="" EnableClientScript="true" ClientValidationFunction="MotherMobileNumberValidation"></asp:CustomValidator>
            <asp:CustomValidator ID="CustomValidator2" Display="None" runat="server" CssClass="ClsMdtStar"
                Visible="true" ErrorMessage="" EnableClientScript="true" ClientValidationFunction="ValidateMotherMobileNo2"></asp:CustomValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Email Address should not be blank."
                Display="None" ControlToValidate="txtEmail"> </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="Email Address should be in valid format." ForeColor="Red" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                Display="None" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Address should not be blank."
                Display="None" ControlToValidate="txtAddress"> </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regAddress" runat="server" ControlToValidate="txtAddress"
                Display="None" ErrorMessage="Address should not exceed than 300 characters."
                ValidationExpression="^[\s\S]{0,300}$"></asp:RegularExpressionValidator>
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Verification Code should not be blank."
                Display="None" ControlToValidate="txtCaptcha"> </asp:RequiredFieldValidator>--%>
        </div>
        <div class="form-group">
            <label for="ddlInquiryType">
                Admission Year (Academic Year)</label>
            <asp:DropDownList ID="cmbYear" runat="server" CssClass="form-control" AutoPostBack="true"
                ViewStateMode="Enabled" OnSelectedIndexChanged="cmbYear_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label for="ddlInquiryType">
                Enquiry No</label>
            <asp:TextBox ID="txtEnqNo" runat="server" Enabled="false" ViewStateMode="Enabled"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="ddlInquiryType">
                Grade/Std. Applying for</label>
            <asp:DropDownList ID="cmbStd" runat="server" CssClass="form-control mandatory" AutoPostBack="false"
                Style="background-color: #FFFFE0;" ViewStateMode="Enabled">
                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label>
                Student Name</label>
            <div class="name-row">
                <asp:TextBox ID="txtStudLastName" runat="server" CssClass="form-control name-input"
                    placeholder="Last Name" MaxLength="50" />
                <asp:TextBox ID="txtStudFirstName" runat="server" CssClass="form-control name-input mandatory"
                    MaxLength="50" placeholder="First Name" />
                <asp:TextBox ID="txtStudMiddleName" runat="server" CssClass="form-control name-input"
                    MaxLength="50" placeholder="Middle Name" />
            </div>
        </div>
        <div class="form-group">
            <label for="cmbGender">
                Gender</label>
            <asp:DropDownList ID="cmbGender" runat="server" CssClass="form-control mandatory"
                ViewStateMode="Enabled" Style="background-color: #FFFFE0;">
                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                <asp:ListItem Text="Female" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label for="txtCalDobPopup">
                DOB</label>
            <asp:TextBox ID="txtCalDobPopup" runat="server" CssClass="form-control mandatory"
                Style="padding: 10px; font-size: 14px; border: 1px solid #ccc;" />
        </div>
        <div class="form-group">
            <label for="txtPhone">
                Aadhar Card Number</label>
            <asp:TextBox ID="txtAadharCardNumber" runat="server" CssClass="form-control" MaxLength="12"
                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                ondrop="event.returnValue=false" />
        </div>
        <div class="form-group">
            <label>
                Father's Name</label>
            <div class="name-row">
                <asp:TextBox ID="txtFLastName" runat="server" CssClass="form-control name-input"
                    MaxLength="50" placeholder="Last Name" />
                <asp:TextBox ID="txtFFirstName" runat="server" CssClass="form-control name-input mandatory"
                    MaxLength="50" placeholder="First Name" />
                <asp:TextBox ID="txtFMiddleName" runat="server" CssClass="form-control name-input"
                    placeholder="Middle Name" MaxLength="50" />
            </div>
        </div>
        <div class="form-group">
            <label>
                Father's Mobile No.</label>
            <div class="name-row">
                <asp:TextBox ID="txtFatherMob1" runat="server" CssClass="form-control name-input mandatory"
                    MaxLength="10" placeholder="Mobile No. 1" onblur="extractNumber(this,0,false);"
                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                <asp:TextBox ID="txtFatherMob2" runat="server" CssClass="form-control name-input"
                    MaxLength="10" placeholder="Mobile No. 2" onblur="extractNumber(this,0,false);"
                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
            </div>
        </div>
        <div class="form-group">
            <label>
                Mother's Name</label>
            <div class="name-row">
                <asp:TextBox ID="txtMLastName" runat="server" CssClass="form-control name-input"
                    placeholder="Last Name" MaxLength="50" />
                <asp:TextBox ID="txtMFirstName" runat="server" CssClass="form-control name-input mandatory"
                    MaxLength="50" placeholder="First Name" />
                <asp:TextBox ID="txtMMiddleName" runat="server" CssClass="form-control name-input"
                    placeholder="Middle Name" MaxLength="50" />
            </div>
        </div>
        <div class="form-group">
            <label>
                Mother's Mobile No.</label>
            <div class="name-row">
                <asp:TextBox ID="txtMotherMob1" runat="server" CssClass="form-control name-input mandatory"
                    MaxLength="10" placeholder="Mobile No. 1" onblur="extractNumber(this,0,false);"
                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                <asp:TextBox ID="txtMotherMob2" runat="server" CssClass="form-control name-input"
                    MaxLength="10" placeholder="Mobile No. 2" onblur="extractNumber(this,0,false);"
                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
            </div>
        </div>
        <div class="form-group">
            <label for="txtEmail">
                Email Address</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mandatory" MaxLength="50" />
        </div>
        <div class="form-group">
            <label for="txtAddress">
                Address</label>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control mandatory" TextMode="MultiLine"
                MaxLength="500" Rows="5" />
        </div>
        <div class="form-group">
            <label for="ddlInquiryType">
                Area</label>
            <asp:DropDownList ID="cmbArea" runat="server" CssClass="form-control" ViewStateMode="Enabled">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label for="txtPhone">
                Current School</label>
            <asp:TextBox ID="txtSchoolName" runat="server" CssClass="form-control" MaxLength="200" />
        </div>
        <div class="form-group">
            <label for="txtPhone">
                Siblings at Shantiniketan</label>
            <asp:TextBox ID="TxtSibling" runat="server" CssClass="form-control" MaxLength="100" />
        </div>
        <div class="form-group">
            <label for="txtPhone">
                Friends/Colleague</label>
            <asp:TextBox ID="txtFrnd" runat="server" CssClass="form-control" MaxLength="100" />
        </div>
        <div class="form-group">
            <label for="ddlInquiryType">
                Heard of Shantiniketan from</label>
            <asp:CheckBoxList ID="chklstReferences" runat="server" CssClass="TxtNormal" RepeatDirection="Horizontal"
                RepeatColumns="4">
            </asp:CheckBoxList>
        </div>
        <%--<div class="form-group">
            <label for="ddlInquiryType">
                Verification Code</label>            
            <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Medium" CaptchaLength="5"
            CustomValidatorErrorMessage="Incorrect Verification Code. Please try again."
            CaptchaHeight="60" CaptchaWidth="210" CaptchaLineNoise="Low" FontColor="#529E00"
            CaptchaMaxTimeout="300" Width="100%" />
            <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control" MaxLength="10" />
        </div>--%>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Enquiry" CssClass="submit-btn"
            OnClick="btnSubmit_Click" />
        <div id="customModal" class="modal-overlay">
            <div class="modal-box">
                <h4>
                    Your enquiry form is submitted successfully!!!</h4>
                <button type="button" class="close-btn" onclick="closeModal()">
                    Close</button>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        _clienttxtFatherMob1 = "<%=this.txtFatherMob1.ClientID %>"
        _clienttxtMotherMob1 = "<%=this.txtMotherMob1.ClientID %>"
        _clienttxtMotherMob2 = "<%=this.txtMotherMob2.ClientID %>"
        _clienttxtFatherMob2 = "<%=this.txtFatherMob2.ClientID %>"
        _clienttxtAadharCardNumber = "<%=this.txtAadharCardNumber.ClientID %>"

        function FatherMobileNumberValidation(oSrc, args) {
            var fatherMobileNo = document.getElementById(_clienttxtFatherMob1).value;

            if (fatherMobileNo == "") {
                oSrc.errormessage = "Father's Mobile No 1 should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (fatherMobileNo.length != 10) {
                oSrc.errormessage = "Father's Mobile No 1 should be of 10 digits.";
                args.IsValid = false;
                return true;
            }
            else if (fatherMobileNo.substring(0, 1) == "0") {
                oSrc.errormessage = "Father's Mobile No 1 should not start with zero.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function MotherMobileNumberValidation(oSrc, args) {
            var motherMobileNo = document.getElementById(_clienttxtMotherMob1).value;
            if (motherMobileNo == "") {
                oSrc.errormessage = "Mother's Mobile No 1 should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (motherMobileNo.length != 10) {
                oSrc.errormessage = "Mother's Mobile No 1 should be of 10 digits.";
                args.IsValid = false;
                return true;
            }
            else if (motherMobileNo.substring(0, 1) == "0") {
                oSrc.errormessage = "Mother's Mobile No 1 should not start with zero.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateMotherMobileNo2(oSrc, args) {
            var motherMobileNo = document.getElementById(_clienttxtMotherMob2).value;
            if (motherMobileNo != "") {
                if (motherMobileNo.length != 10) {
                    oSrc.errormessage = "Mother's Mobile No 2 should be of 10 digits.";
                    args.IsValid = false;
                    return true;
                }
                else if (motherMobileNo.substring(0, 1) == "0") {
                    oSrc.errormessage = "Mother's Mobile No 2 should not start with zero.";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateFatherMobileNo2(oSrc, args) {
            var fatherMobileNo = document.getElementById(_clienttxtFatherMob2).value;
            if (fatherMobileNo != "") {
                if (fatherMobileNo.length != 10) {
                    oSrc.errormessage = "Father's Mobile No 2 should be of 10 digits.";
                    args.IsValid = false;
                    return true;
                }
                else if (fatherMobileNo.substring(0, 1) == "0") {
                    oSrc.errormessage = "Father's Mobile No 2 should not start with zero.";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateAadharCardNo(oSrc, args) {
            var aadharCardNo = document.getElementById(_clienttxtAadharCardNumber).value;
            if (aadharCardNo != "") {
                if (aadharCardNo.length != 12) {
                    oSrc.errormessage = "Aadhar Card Number should be of 12 digits.";
                    args.IsValid = false;
                    return true;
                }
                else if (aadharCardNo.substring(0, 1) == "0") {
                    oSrc.errormessage = "Aadhar Card Number should not start with zero.";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function validatePastDate(sender, args) {
            var inputDate = new Date(args.Value);
            var today = new Date();
            today.setHours(0, 0, 0, 0);
            args.IsValid = inputDate < today;
        }

    </script>
    </form>
</body>
</html>
