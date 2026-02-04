<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserSurveyDetailsUI.aspx.cs" Inherits="UserSurveyDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .ClsSurveyHeader
        {
            font-weight: 700;
            font-size: 9pt;
            color: White;
            text-decoration: none;
            padding-right: 5px;
            height: 20px;
            background-color: #006697;
            border-bottom-style: solid;
            border-bottom-width: 1px;
        }
        
        .ClsSurveyChildHeader
        {
            font-weight: bold;
            font-size: 9pt;
            color: Black;
            text-decoration: none;
            padding-right: 5px;
            height: 20px;
            background-color: #BFD2FF;
            border-bottom-style: solid;
            border-bottom-width: 1px;
        }
        
        
        .ClsSurveyCell
        {
            background-color: #E1EAFF;
            font-family: Arial;
            font-size: 9pt;
            padding-right: 5px;
            border-color: White;
        }
        
        .ClsSurveySchoolHead
        {
            font-weight: 700;
            font-family: Tahoma;
            color: White;
            text-transform: capitalize;
            font-size: 13pt;
            border-bottom: 1px solid #ddd;
            background-color: #265B75;
            padding: 2px 2px 3px 5px;
        }
    </style>
    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ErrMsg" />
                    <asp:CustomValidator ID="cstValAnswerLength" runat="server" ClientValidationFunction="ValidateAnswerLength"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tblQuestions" runat="server" cellspacing="1" width="80%">
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" UseSubmitBehavior="false"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" UseSubmitBehavior="false"
                        Enabled="false" OnClick="btnSubmit_Click" />
                    <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSurveyId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                    <asp:HiddenField ID="hidUserRoleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSaveAndSubmit" runat="server" Value="" />
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            _clienthidSaveAndSubmit = "<%=this.hidSaveAndSubmit.ClientID %>";

            function ConfirmSubmit() {
                return confirm('This action will submit only saved details. Do you want to continue?');
            }

            function ValidateAnswerLength(oSrc, args) {
                var sRows = ""
                var answers = document.getElementsByTagName("textarea");
                for (var k = 0; k < answers.length; k++) {
                    var answer = answers[k]
                    if (answer.value.trim() != "" && answer.value.trim().length > 500) {
                        if (sRows.match((k + 1)) == null)
                            sRows = sRows + ", " + (k + 1)
                    }
                }

                if (sRows != "") {
                    sRows = sRows.substring(1)
                    oSrc.errormessage = "Answer length should not be greater than 500 characters.";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
