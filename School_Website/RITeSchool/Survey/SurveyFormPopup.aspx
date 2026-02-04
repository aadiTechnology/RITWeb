<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="SurveyFormPopup.aspx.cs" Inherits="SurveyFormPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                        <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                            <tr>
                                                <td align="left" class="MainTitleHead" style="height: 20px">
                                                    <span style="font-weight: bold">Registration Details</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="Height10">
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <span class="ClsMdtStar">*</span>
                                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name should not be blank."
                                            Display="None" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                                            ClientValidationFunction="ValidateMobileNo"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None"
                                            ClientValidationFunction="ValidateMobileNo2"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="School Name should be selected."
                                            Display="None" ControlToValidate="cmbSchool" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Economical Condition should be selected."
                                            Display="None" ControlToValidate="cmbCategory" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="right" colspan="2">
                                                  <div style="width:150px;text-align:left;" class="ClsGreenBG">
                                                             <asp:LinkButton ID="lnkStandards" runat="server" Text="School Registration" OnClientClick="OpenPopup(); return false;"
                                                                        CssClass="SubTitle"></asp:LinkButton>
                                                        </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" id="tdMessage" runat="server">
                                                    <asp:Label ID="lblMessage" runat="server" Text="" Style="font-size: 12px;" EnableViewState="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="200px">
                                                    <span class="ClsLabel">Registration No. :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblRegNo" runat="server" Text="-" Style="font-weight: bold; font-size: 15px;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Name :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Gender :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:RadioButton ID="optMale" runat="server" CssClass="ClsLabel" Text="Male" GroupName="Gender" />
                                                    <asp:RadioButton ID="optFemale" runat="server" CssClass="ClsLabel" Text="Female"
                                                        GroupName="Gender" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Mobile No. 1 :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMobile1" runat="server" CssClass="MidTxtBox" MaxLength="10" onblur="extractNumber(this,2,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Mobile No. 2 :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMobile2" runat="server" CssClass="MidTxtBox" MaxLength="10" onblur="extractNumber(this,2,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Address :</span>
                                                </td>
                                                <td align="left">
                                                   <asp:TextBox ID="txtAddress" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">School Name :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbSchool" runat="server" CssClass="ExLrgCombo" Width="350px">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Standard :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStandard" runat="server" CssClass="LrgCombo" 
                                                        >
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Economical Condition :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="LrgCombo">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Is Interested for competition? :</span>
                                                </td>
                                                <td align="left">
                                                   <asp:CheckBox ID="chkIsInterested" runat="server" TabIndex="11" />
                                                </td>
                                            </tr>
                                            <tr class="Height10">
                                                <td colspan="2">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSaveAndContinue" runat="server" Text="Save and Continue" CssClass="ClsBtn"
                                            Width="150px" OnClick="btnSaveAndContinue_Click" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                            OnClientClick="RefreshBaseScreen(); return false;" />
                                        <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSchool" runat="server" Value="" OnValueChanged="hidSchool_OnValueChanged" />
                                        <asp:HiddenField ID="hidIsIterestedForConpetition" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            _clienttxtMobile1 = "<%=this.txtMobile1.ClientID %>"
            _clienttxtMobile2 = "<%=this.txtMobile2.ClientID %>"
            _clienthidSchool = "<%=this.hidSchool.ClientID %>"

            function ClosePopup() {
                window.close();
            }

            function RefreshBaseScreen() {
                //queryString = document.getElementById("<%=this.hidQueryString.ClientID %>").value
                window.opener.location = "SurveyFormDetailsUI.aspx";
                window.close();
                window.opener.focus();
            }

            function ValidateMobileNo(oSrc, args) {
                var mobileNo = $('#' + _clienttxtMobile1).val();

                if (mobileNo == "") {
                    oSrc.errormessage = "Mobile No. 1 should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (mobileNo.length != 10) {
                    oSrc.errormessage = "Mobile No. 1 should be 10 digit long.";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function ValidateMobileNo2(oSrc, args) {
                var mobileNo1 = $('#' + _clienttxtMobile2).val();

                if (mobileNo1 != "" && mobileNo1.length != 10) {
                    oSrc.errormessage = "Mobile No. 2 should be 10 digit long.";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }


            function OpenPopup() {
                window.open('../Survey/SurveySchoolDetailsPopup.aspx?', '_blank', 'width=850, height=700, left=' + ((screen.width - 850) / 2) + ', top=' + ((screen.heigth - 700) / 2)).focus();

            }

            function RefreshSchoolCombo() {
                var num = Math.random();
                $('#' + _clienthidSchool).val(num);
                __doPostBack($get(_clienthidSchool).value.name, '')
            }

        </script>
    </div>
</asp:Content>
