<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/BasicMaster.master"
    AutoEventWireup="true" CodeFile="FeedbackFormUI.aspx.cs" Inherits="FeedbackFormUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <style>
            .ProgressReportHeader
            {
                font-weight: 700;
                font-size: 12pt;
                color: White;
                text-decoration: none;
                height: 20px;
                background-color: #8080C0;
                border-style: solid;
                border-width: 1px;
                border-color: Navy;
            }
            
            .ProgressReportRow
            {
                font-weight: 700;
                font-size: 11pt;
                color: #333;
                text-decoration: none;
                height: 20px;
                background-color: skyblue;
            }
            
            .ProgressReportParameter
            {
                font-size: 11pt;
                color: #333;
                text-decoration: none;
                height: 20px;
                background-color: #c8dffe;
            }
            
            .StudentDetailsHeader
            {
                font-weight: 700;
                font-size: 12pt;
                color: White;
                text-decoration: none;
                height: 20px;
                padding-left: 5px;
                background-color: #3D7C7C;
                border-style: solid;
                border-width: 1px;
                border-color: Navy;
            }
        </style>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="valSum" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <h2>
                                    <u>Parent Feedback Form</u></h2>
                            </td>
                        </tr>
                        <tr>
                            <td align="justify">
                                <span style="font-size: 17px; font-family: Times New Roman;">We at shantiniketan school
                                    appreciate the confidance and trust you place in us to educate your child in holistick
                                    way.As partners in the education of your child,your opinion can help us understand
                                    how satisfied you are with the educational services we provide.We are committed
                                    to providing the best possible experience for you and your child.So,please take
                                    a moment to complete this short survey.Your opinions are valued and will help us
                                    to improve.We shall be discreet about your identity.Thanks for helping us out. On
                                    scale of 1-5 please circle only one answer per question that best indicates how
                                    satisfied you are with us. </span>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ListView ID="lstvwParameters" runat="server" DataKeyNames="Id,ParentQuestionId"
                                            OnItemDataBound="lstvwParameters_ItemDataBound">
                                            <LayoutTemplate>
                                                <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ProgressReportHeader" id="trHeader" runat="server">
                                                        <th align="right" style="width: 50px; padding-right: 5px;">
                                                            Sr. No.
                                                        </th>
                                                        <th align="left" style="padding-left: 5px">
                                                            Title
                                                        </th>
                                                        <th align="left" style="padding-left: 5px" width="50%">
                                                            Grade / Description
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server">
                                                    <td align="right">
                                                        <asp:Label ID="lblSrNo" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                            padding-right: 5px;"></asp:Label>
                                                    </td>
                                                    <td align="center" id="tdTitle" runat="server">
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="ClsLabel" Text='<%#Eval("Title") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" style="padding: 5px">
                                                        <asp:DropDownList ID="cmbGrade" runat="server" CssClass="MidCombo" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="ExLrgTxtBox"
                                                            Height="50px" Width="99%" Visible="false"></asp:TextBox>
                                                        <asp:CheckBox ID="chkOption" runat="server" Visible="false" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" 
                                    OnClick="btnCancel_Click" CausesValidation="False" />
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="ClsBtn" 
                                    OnClick="btnClear_Click" CausesValidation="False" />
                                <asp:HiddenField ID="hidIsFromTerms" runat="server" Value="N" />
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                                    ClientValidationFunction="ValidateFields"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None"
                                    ClientValidationFunction="ValidateTextFields"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        _clientlstvwParameters = "<%=this.lstvwParameters.ClientID %>"

        function ValidateFields(oSrc, args) {
            var rowIndex = 0
            var found = false;
            var title = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblTitle")
            while (title != null) {

                var cmbGrade = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_cmbGrade")
                if (cmbGrade != null) {
                    if (cmbGrade.value == "0") {
                        cmbGrade.style.backgroundColor = "lightyellow"
                        found = true;
                    }
                    else
                        cmbGrade.style.backgroundColor = "white"
                }

                rowIndex++;
                title = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblTitle")
            }

            if (found) {
                oSrc.errormessage = "Fields marked in yellow color should be selected.";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateTextFields(oSrc, args) {
            var rowIndex = 0
            var found = false;
            var maxFound = false
            var chk
            var chkIndex = 9999
            var title = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblTitle")
            while (title != null) {

                var chkOption = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_chkOption")

                if (chkOption != null && chkOption.checked == false) {
                    chkIndex = rowIndex
                }

                var txtDescription = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_txtDescription")
                if (txtDescription != null && rowIndex < chkIndex) {
                    if (txtDescription.value.trim() == "") {
                        txtDescription.style.backgroundColor = "lightyellow"
                        found = true;
                    }
                    else if (txtDescription.value.trim().length > 1000) {
                        txtDescription.style.backgroundColor = "lightgreen"
                        maxFound = true;
                    }
                    else
                        txtDescription.style.backgroundColor = "white"
                }

                rowIndex++;
                title = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblTitle")
            }

            if (found || maxFound) {
                if (found && !maxFound)
                    oSrc.errormessage = "Fields marked in yellow color should not be blank.";
                else if (!found && maxFound)
                    oSrc.errormessage = "Length of fields marked in green color should not be greater than 1000.";
                else
                    oSrc.errormessage = "Fields marked in yellow color should not be blank and length of fields marked in green color should not be greater than 1000.";

                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        SetFields();

        function SetFields() {
            var rowIndex = 0
            var title = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblTitle")
            while (title != null) {
                var chkOption = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_chkOption")
                if (chkOption != null) {
                    SetFieldState(rowIndex)
                    break
                }

                rowIndex++;
                title = document.getElementById(_clientlstvwParameters + "_ctrl" + rowIndex + "_lblTitle")
            }
        }

        function SetFieldState(index) {
            var chkOption = document.getElementById(_clientlstvwParameters + "_ctrl" + index + "_chkOption")
            var title = document.getElementById(_clientlstvwParameters + "_ctrl" + index + "_lblTitle")
            while (title != null) {

                var txtDescription = document.getElementById(_clientlstvwParameters + "_ctrl" + index + "_txtDescription")
                if (txtDescription != null) {
                    txtDescription.value = ""
                    if (chkOption.checked)
                        txtDescription.disabled = false
                    else
                        txtDescription.disabled = true

                }

                index++;
                title = document.getElementById(_clientlstvwParameters + "_ctrl" + index + "_lblTitle")
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
