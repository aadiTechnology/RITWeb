<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SurveyFeedbackUI.aspx.cs" Inherits="SurveyFeedbackUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <style type="text/css">
            .ClsSurveyHeader
            {
                font-weight: 700;
                font-size: 9pt;
                color: #000080;
                text-decoration: none;
                padding-right: 5px;
                height: 20px;
                background-color: #F2BFEE;
                border-bottom-style: solid;
                border-bottom-width: 1px;
            }
            
            
            .ClsSurveyCell
            {
                background-color: #FAE7FC;
                font-family: Arial;
                font-size: 9pt;
                padding-right: 5px;
            }
            
            .ClsSurveySchoolHead
            {
                font-weight: 700;
                font-family: Tahoma;
                color: #000;
                text-transform: capitalize;
                font-size: 13pt;
                border-bottom: 1px solid #ddd;
                background-color: #FAC7FE;
                padding: 2px 2px 3px 5px;
            }
        </style>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td align="left" id="td1" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                            <asp:CustomValidator ID="cstValGrade" runat="server" ClientValidationFunction="ValidateGrade"
                                SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                            <asp:CustomValidator ID="cstvalObservation" runat="server" ClientValidationFunction="ValidateObservation"
                                SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                            <asp:CustomValidator ID="cstValObservationLength" runat="server" ClientValidationFunction="ValidateObservationLength"
                                SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdMessage" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                ForeColor="Blue" Style="text-align: center"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdFinalApprover" runat="server" visible="false">
                    <table width="70%">
                        <tr>
                            <td align="center" class="LblNoRecord">
                                <asp:Label ID="lblAdminMessage" runat="server" EnableViewState="false" Text="Feedback details are not yet configured."
                                    ForeColor="Blue" Style="text-align: center"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%" id="tblEvaluation" runat="server">
                        <tr>
                            <td align="right">
                                <span class="ClsMdtStar">* Mandatory field</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="70%" id="tblSchoolDetails" runat="server">
                                    <tr>
                                        <td>
                                            <span class="ClsLabel" style="padding-left:0px">If you do not wish to participate in online feedback then please  
                                            <asp:LinkButton ID="lnkForm" runat="server" OnClientClick="DownloadForm(); return false;"><B>Click Here</B></asp:LinkButton> to download empty form, print it and submit the completed form in school office.</span>
                                        </td>
                                    </tr>
                                    <tr style="height:10px">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td align="center" class="SocietyName">
                                            <asp:Label ID="lblOrgName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="SocietyName">
                                            <asp:Label ID="lblSchoolName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ActualSchoolName">
                                            <asp:Label ID="lblSchoolAddress" runat="server" Style="font-size: 15px;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="SocietyName">
                                            <asp:Label ID="lblStaffPerformance" runat="server" Text="School Feedback / Survey Form"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table id="tblGrades" runat="server" width="70%">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="tblParameter" runat="server" width="70%">
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table id="tblNote" runat="server">
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                            <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                            <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="After submit, feedback details will be available for management review and you will not be able to update feedback details."></asp:Label>
                                        </td>
                                    </tr>
                                 </table>
                            </td>
                        </tr>
                        <tr id="trButtons" runat="server">
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                            CssClass="ClsBtn" UseSubmitBehavior="False" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalizedResources, Submit%>"
                                            CssClass="ClsBtn" UseSubmitBehavior="False" CausesValidation="true" OnClick="btnSubmit_Click" />
                                        <asp:HiddenField ID="hidSurveyId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidNonMandatoryFieldIds" runat="server" Value="0" />
                    <asp:HiddenField ID="hidConfirmSubmit" runat="server" Value="N" />
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">

            _clienthidNonMandatoryFieldIds = "<%=this.hidNonMandatoryFieldIds.ClientID %>"
            _clienthidConfirmSubmit = "<%=this.hidConfirmSubmit.ClientID %>"

            function ConfirmSubmit() {
                ResetMessage();

                var validationResult = false;                
                if (typeof (Page_ClientValidate) == 'function')
                    validationResult = Page_ClientValidate("");
                if (validationResult)
                    return confirm("This action will save and submit feedback details. Do you want to continue?");
                else
                    return false;
            }

            function ValidateGrade(oSrc, args) {
                var sRows = ""
                var fields = $('#' + _clienthidNonMandatoryFieldIds).val()
                var nonMandatoryFields = fields.split(',')
                var grades = document.getElementsByTagName("select");
                for (var k = 0; k < grades.length; k++) {
                    var grade = grades[k]
                    if (grade.value == 0 && grade.value != "") {

                        if (nonMandatoryFields.length > 0) {
                            var id = grade.id.substring(grade.id.lastIndexOf('_') + 1)
                            var index = nonMandatoryFields.indexOf(id)
                            if (index != -1) {
                                continue;
                            }
                        }


                        if (sRows.match((k + 1)) == null)
                            sRows = sRows + ", " + (k + 1)
                    }
                }

                if (sRows != "") {
                    sRows = sRows.substring(1)
                    oSrc.errormessage = "Grade should be selected for row(s) : " + sRows+".";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function ValidateObservation(oSrc, args) {

                var fields = $('#' + _clienthidNonMandatoryFieldIds).val()
                var nonMandatoryFields = fields.split(',')

                var sRows = ""
                var observations = document.getElementsByTagName("textarea");
                for (var k = 0; k < observations.length; k++) {
                    var observation = observations[k]
                    if (observation.value.trim() == "") {

                        if (nonMandatoryFields.length > 0) {
                            var id = observation.id.substring(observation.id.lastIndexOf('_') + 1)
                            var index = nonMandatoryFields.indexOf(id)
                            if (index != -1) {
                                continue;
                            }
                        }


                        if (sRows.match((k + 1)) == null)
                            sRows = sRows + ", " + (k + 1)
                    }
                }

                if (sRows != "") {
                    sRows = sRows.substring(1)
                    oSrc.errormessage = "Observation should not be blank for row(s) : " + sRows+".";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function ValidateObservationLength(oSrc, args) {
                var sRows = ""
                var observations = document.getElementsByTagName("textarea");
                for (var k = 0; k < observations.length; k++) {
                    var observation = observations[k]
                    if (observation.value.trim() != "" && observation.value.trim().length > 500) {
                        if (sRows.match((k + 1)) == null)
                            sRows = sRows + ", " + (k + 1)
                    }
                }

                if (sRows != "") {
                    sRows = sRows.substring(1)
                    oSrc.errormessage = "Observation length should not be greater than 500 characters for row(s) : " + sRows+".";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function DownloadForm() {
                window.open('../downloads/Feedback Form.pdf')
            }

            function ResetMessage() {
                $('#' + "<%=this.lblMessage.ClientID %>").text("")
            }

            function ConfirmSave() {
                ResetMessage();

                var validationResult = false;
                if (typeof (Page_ClientValidate) == 'function')
                    validationResult = Page_ClientValidate("");
                if (validationResult) {
                    if (confirm('Do you want to submit feedback details once it is saved?'))
                        $('#' + _clienthidConfirmSubmit).val("Y")
                    else
                        $('#' + _clienthidConfirmSubmit).val("N")
                }
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
