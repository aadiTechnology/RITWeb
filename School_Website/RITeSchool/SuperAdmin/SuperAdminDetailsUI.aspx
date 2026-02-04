<%@ Page Language="C#" CodeFile="SuperAdminDetailsUI.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" Inherits="SuperAdminDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td id="MainDataTable" align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <!-- Data Insert Here -->
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 97%;">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="80%">
                                                <asp:Panel ID="pnlErrorMsg" runat="server">
                                                    <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" Height="20px"
                                                        Width="100%" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label></asp:Panel>
                                            </td>
                                            <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                                <span class="ClsMdtStar">* Mandatory Fields</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="1">
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                        ValidationGroup="Save" ShowSummary="true" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="1">
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" class="ClsTextNormal" align="center">
                                    <!-- User InfoTable starts here -->
                                    <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                        style="width: 595px;">
                                        <tr>
                                            <td align="left" class="ClsBorderLight" style="width: 50%">
                                                <asp:Label ID="Label10" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Name :"></asp:Label>
                                                <asp:Label ID="Label11" runat="server" CssClass="LblSmlGray floatR" EnableViewState="False"
                                                    Font-Italic="False" Text="(First Name)"></asp:Label>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="width: 300px">
                                                <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" Width="50px"
                                                    TabIndex="1">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" Width="186px" CssClass="MidTxtBox"
                                                    TabIndex="2"></asp:TextBox>
                                                *&nbsp;
                                                <asp:RequiredFieldValidator ID="rqdFirstName" runat="server" ControlToValidate="txtFirstName"
                                                    Display="None" ValidationGroup="Save" ErrorMessage="First name should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderLight" style="width: 42%">
                                                <asp:Label ID="Label12" runat="server" CssClass="LblSmlGray floatR" EnableViewState="False"
                                                    Font-Italic="False" Text="(Middle Initial)"></asp:Label>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="width: 300px">
                                                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="1"
                                                    Width="50px" TabIndex="3"></asp:TextBox>
                                                <%--*<asp:RequiredFieldValidator ID="reqMiddleName" runat="server" ControlToValidate="txtMiddleName"
                                            Display="None" ErrorMessage="Middle initial should not be blank."></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderLight" style="width: 42%">
                                                <asp:Label ID="Label13" runat="server" CssClass="LblSmlGray floatR" EnableViewState="False"
                                                    Font-Italic="False" Text="(Last Name)"></asp:Label>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="width: 300px">
                                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="MidTxtBox"
                                                    TabIndex="4"></asp:TextBox>
                                                <%--*<asp:RequiredFieldValidator ID="rqdLastName" runat="server" ControlToValidate="txtLastName"
                                            Display="None" ValidationGroup="Save" ErrorMessage="Last name should not be blank."></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                    <td class="ClsBorderlight" align="center" style="width: 42%">
                                        <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                            Text="Date of Birth :"></asp:Label>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtDOB" runat="server" CssClass="SmlTxtBox" MaxLength="11" TabIndex="5"></asp:TextBox>
                                        <rjs:PopCalendar ID="cBirthDate" runat="server" Control="txtDOB" Format="dd MMM yyyy"
                                            ShowWeekend="True" Enabled="true" ShowErrorMessage="false" To-Today="true" InvalidDateMessage="Please select valid birth date."
                                            ControlFocusOnError="True" ValidationGroup="Save" />
                                        <asp:CustomValidator ID="cstBirthDate" runat="server" Display="none" EnableClientScript="true"
                                            ClientValidationFunction="ValidateBirthDate" ErrorMessage="Birth date should not be future date."></asp:CustomValidator>
                                    </td>
                                </tr>
                                _clienttxtDOB = "<%=this.txtDOB.ClientID %>";
                                _clientcstBirthDate = "<%=this.cstBirthDate.ClientID%>";
                                _clientServerDate = "<%=this.hidServerDate.ClientID %>";
                                --%>
                                        <tr>
                                            <td align="left" class="ClsBorderLight" style="width: 42%">
                                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Font-Italic="False" Text="Mobile No.:"></asp:Label>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="width: 300px">
                                                <asp:TextBox ID="txtMobileNo" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" TabIndex="6" />*
                                                <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    Visible="true" ValidationGroup="Save" EnableClientScript="true" ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderLight" style="width: 42%">
                                                <asp:Label ID="lblUserName" runat="server" Text="User Name :" CssClass="ClsLabel"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td id="Td1" align="left" style="color: red;">
                                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="20" CssClass="ExLrgTxtBox"
                                                    TabIndex="9"></asp:TextBox>
                                                     <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqUsername" runat="server" ControlToValidate="txtUserName" ValidationGroup="Save" ></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="regUsername" runat="server" ValidationGroup="Save" 
                                             ErrorMessage="User name accepts only alphanumeric characters, an underscore (_) and a dot (.)." style="font-size:9pt" 
                                            ValidationExpression="[A-Za-z0-9_.]+"  
                                            ControlToValidate="txtUserName"> <span class="ClsMdtStar"></span> </asp:RegularExpressionValidator>

                                                <asp:CustomValidator ID="cst_UserName"  runat="server" ClientValidationFunction="UserNameValidation" ControlToValidate="txtUserName"
                                                   ValidationGroup="Save" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderLight" style="width: 42%">
                                                <asp:Label ID="lblPassword" runat="server" Text="Password :" CssClass="ClsLabel"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 300px">
                                                <asp:TextBox ID="txtPasswd" runat="server" TextMode="Password" MaxLength="15" CssClass="ExLrgTxtBox"
                                                    TabIndex="10"></asp:TextBox>
                                                <span class="ClsMdtStar">*
                                                    <asp:CustomValidator ID="cstValPassword" runat="server" ClientValidationFunction="PasswordValidation"
                                                        ControlToValidate="txtPasswd" ValidationGroup="Save" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="reqPasswd" runat="server" ControlToValidate="txtPasswd"
                                                        Display="None" ValidationGroup="Save" ErrorMessage="Password should not be blank."></asp:RequiredFieldValidator>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderLight" style="width: 42%">
                                                <asp:Label ID="lblConfirmPasswd" runat="server" Text="Confirm Password :" CssClass="ClsLabel"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td class="ClsMdtStar" align="left" style="width: 300px">
                                                <asp:TextBox ID="txtConfirmPasswd" runat="server" TextMode="Password" MaxLength="15"
                                                    CssClass="ExLrgTxtBox" TabIndex="11"></asp:TextBox>
                                                *<asp:CustomValidator ID="cstValConfirmPassword" runat="server" ClientValidationFunction="ComparePasswordValidation"
                                                ControlToValidate="txtConfirmPasswd" Display="None" ValidateEmptyText="True" ValidationGroup="Save"></asp:CustomValidator>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="left" class="ClsMdtStar">
                                                <asp:Label ID="lblNote" Width="300px" runat="server" Style="vertical-align: top" BorderWidth="0px"
                                                    BorderColor="Gray" Text="Capitalization Matters! Min 6 characters, Max 15 characters."
                                                    CssClass="LblSmlGray" EnableViewState="False"></asp:Label>
													 <span class="LblSmlGray" id="PassNote" runat="server"> Password should be combination of at least one character, digit & special character.
                                            </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 42%">
                                                <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="Save" BorderWidth="1px" 
                                                    OnClick="btnSave_Click" ValidationGroup="Save" UseSubmitBehavior="false" TabIndex="12">
                                                </asp:Button>
                                            </td>
                                            <td align="left" colspan="1" style="width: 300px">
                                                <asp:Button CssClass="ClsBtn" ID="BtnCancel" CausesValidation="false" runat="server"
                                                    Text="Cancel" BorderWidth="1px" OnClick="BtnCancel_Click"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <asp:HiddenField ID="hidServerDate" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- User InfoTable end here -->
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hidUserId" runat="server" />
                        <asp:HiddenField ID="hidFilePath" runat="server" />
                        <asp:HiddenField ID="hidSuperAdminDetailsId" runat="server" />

                        <!-- Data Insert End Here -->
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trLstvw" runat="server">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <table align="center" width="800px">
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwSuperAdminDetails" runat="server" DataKeyNames="SuperAdminDetailsId,UserId"
                                        OnItemDataBound="lstvwSuperAdminDetails_ItemDataBound" OnItemCommand="lstvwSuperAdminDetails_ItemCommand"
                                        OnItemUpdating="lstvwSuperAdminDetails_ItemUpdating">
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" width="40%" style="padding-left: 7px;">
                                                        Name
                                                    </th>
                                                    <th align="center" width="20%" style="padding-left: 4px;">
                                                        Mobile Number
                                                    </th>
                                                    <th align="center" width="10%">
                                                        Edit
                                                    </th>
                                                    <th align="center" width="10%">
                                                        Delete
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblSuperAdminName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Update"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="Remove"
                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblSuperAdminName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Update"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="Remove"
                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr id="trNoRecordMsg" runat="server">
                                                <td style="height: 10px;" align="center">
                                                    <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                                        Text="No Record Found." EnableViewState="False" Width="85%"></asp:Label>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="1">
                                    <asp:Button CssClass="ClsBtn" ID="btnBack" CausesValidation="false" runat="server"
                                        Text="Back" BorderWidth="1px" OnClick="btnBack_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        _clienttxtPasswdId = "<%=this.txtPasswd.ClientID %>";
        _clientcstValPasswdId = "<%=this.cstValPassword.ClientID %>";
        _clienttxtConfirmPasswd = "<%=this.txtConfirmPasswd.ClientID %>";
        _clientcstValConfirmPassword = "<%=this.cstValConfirmPassword.ClientID %>";
        _clienttxtUserName = "<%=this.txtUserName.ClientID %>";
        _clientcst_UserNameId = "<%=this.cst_UserName.ClientID %>";
        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnCancel = "<%=this.BtnCancel.ClientID %>";
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>";
        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobileNo.ClientID %>";


        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_beginRequest(BeginReqHandler)
        prm.add_endRequest(EndReqHandler)
        function BeginReqHandler(sender, args) {

            var postBackElement = sender._postBackSettings.sourceElement

        }
        function EndReqHandler(sender, args) {

            var postBackElement = sender._postBackSettings.sourceElement
        }

        //This function is used to validate password.
        function PasswordValidation(oSrc, args) {

        	var sPassword = document.getElementById(_clienttxtPasswdId).value;        	
        	var passed = validatePassword(sPassword, {
        		length: [6, Infinity],
        		//lower: 0,
        		//upper: 0,
        		alpha: 1,
        		numeric: 1,
        		special: 1
        	});
        	if (sPassword.length < 6) {
        		document.getElementById(_clientcstValPasswdId).errormessage = "Password should be of minimum 6 characters.";
        		args.IsValid = false;
        		return true;
        	}
        	else if (!passed) {
        		document.getElementById(_clientcstValPasswdId).errormessage = "Password should be combination of at least one character, digit & special character.";
        		args.IsValid = false;
        		return true;
			}
            args.IsValid = true;
            return false;
        }

        //This function is used to validate whether password and confirm password are same or not.
        function ComparePasswordValidation(oSrc, args) {
            var sConfirmPassword = document.getElementById(_clienttxtConfirmPasswd).value
            var sPassword = document.getElementById(_clienttxtPasswdId).value
            if (sPassword != sConfirmPassword) {
                document.getElementById(_clientcstValConfirmPassword).errormessage = "Password and Confirm Password should be same."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        //This function is used to validate user name.
        function UserNameValidation(oSrc, args) {
      
            var sEmail = document.getElementById(_clienttxtUserName).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);
            if (isEmpty(sEmail)) {
                document.getElementById(_clientcst_UserNameId).errormessage = "User Name should not be blank.";
                args.IsValid = false;
                return true;
            }

            if (sEmail.length < 6) {
                document.getElementById(_clientcst_UserNameId).errormessage = "User Name should be of minimum 6 characters.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        //This function is used to validate mobile No.
        function MobileNumberValidation(oSrc, args) {

            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value;
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber);
            document.getElementById(_clientcst_MobileNumber).errormessage = "";
            if (sMobileNumber.length == 0) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile Number should not be blank.";
                args.IsValid = false;
                return true;
            }
            if (sMobileNumber.length < 10) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile Number should be of 10 digits.";
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile number should not start with zero.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        //This function is used to validate birth date.
        function ValidateBirthDate(source, args) {

            var bIsValid = true;

            if (document.getElementById(_clienttxtDOB).value != "") {
                var serverDate = document.getElementById(_clientServerDate).value;
                dtStartDate = new Date(convertdate(document.getElementById(_clienttxtDOB).value));
                var today = new Date(serverDate);
                if (today < dtStartDate) {
                    document.getElementById(_clientcstBirthDate).errormessage =
                                    "Birth Date should not be future date.";
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }
        function ConfirmDelete() {

            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }
        function btnsaveonclick(varname) {
            ResetErrorMsgLbl();
        }

        function ResetErrorMsgLbl() {

            if (document.getElementById(_clientcst_LblErrMsg) != null) {
                document.getElementById(_clientcst_LblErrMsg).style.display = "none";
            }
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none";
            }
        }
    </script>

</asp:Content>
