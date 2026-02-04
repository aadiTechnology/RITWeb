<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SecondLanguageUI.aspx.cs" Inherits="SecondLanguageUI"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                            <span class="ClsMdtStar">*
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Always">
                                    <ContentTemplate>
                                        <table width="100%" id="tblMassage" runat="server" visible="true" style="color: Blue;
                                            font-weight: bold;">
                                            <tr>
                                                <td align="center" valign="top">
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                        EnableViewState="false" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" Height="20px"
                                    Width="100%" EnableViewState="False" CssClass="LblErrorMsg"></asp:Label>
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="NewClsLabel" />
                                <asp:CustomValidator ID="vstValLang" runat="server" Display="None" ErrorMessage="Selected Second and Third Language should not be from same subjet group."
                                    ClientValidationFunction="ValidateLanguage"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="tblSearch" runat="server" colspan="3">
                                <table cellpadding="0" cellspacing="2">
                                    <tr id="trCombo">
                                        <td align="center" class="ClsBorderlight" colspan="1">
                                            <span class="ClsLabel" style="width: 70px;">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                                <span id="Span2" class="colonPadding">:</span></span>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:DropDownList ID="cmbStandard" Width="95px" AutoPostBack="true" runat="server"
                                                CssClass="SmlTxtBox" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                            <asp:RequiredFieldValidator ID="reqdvalStandard" runat="server" ControlToValidate="cmbStandard"
                                                Display="None" ErrorMessage="Please select standard." InitialValue="0"></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="center" class="ClsBorderlight" colspan="1">
                                            <span class="ClsLabel" style="width: 60px;">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Division %>"></asp:Label>
                                                <span id="Span1" class="colonPadding">:</span></span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                ID="uPnl">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbDivision" runat="server" CssClass="SmlTxtBox" Width="95px"
                                                        CausesValidation="True" AutoPostBack="true" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqdvalDivision" runat="server" ControlToValidate="cmbDivision"
                                                        Display="None" ErrorMessage="Please select division." InitialValue="0"></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="1" id="td1" runat="server">
                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel5">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveUp" CausesValidation="true" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                        CssClass="ClsBtn" disable-page="true" Visible="False" OnClick="btnSave_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBackUp" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                                        CssClass="ClsBtn" CausesValidation="false" Visible="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" valign="top">
                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <table style="width: 100%; height: 100%;" cellpadding="0" cellspacing="2">
                                            <tr>
                                                <td align="center">
                                                    <asp:ListView ID="lstvwSecondLanguage" ItemPlaceholderID="ContactRowContainer" runat="server"
                                                        DataKeyNames="SchoolwiseStudentId,OptionalSubjectId" OnItemDataBound="lstvwSecondLanguage_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table style="width: 70%; height: 100%; color: #333333" runat="server" id="tblContacts"
                                                                class="GridBorder" cellpadding="0" cellspacing="1">
                                                                <tr class="ClsGridHeader">
                                                                    <th d align="left" class="ClspaddingL" width="100px">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, RegNo %>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL" width="80px">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, RollNo %>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, StudentName %>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL" width="150px">
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, SecondLanguage %>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL" width="150px">
                                                                        <asp:Label ID="Label7" runat="server" Text="Third Language"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="ContactRowContainer" />
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trStudentRow" runat="server" class="ClsGridRow">
                                                                <td align="left" class="ClspaddingL" width="100px">
                                                                    <asp:Label ID="lblEnrollNo" runat="server" Text='<%#Eval("RegNo")%>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("StudentName")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:DropDownList ID="cmbSecondLanguage" AppendDataBoundItems="true" Width="150px"
                                                                        runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:DropDownList ID="cmbThirdLanguage" AppendDataBoundItems="true" Width="150px"
                                                                        runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="trStudentRow" runat="server" class="ClsGridAltRow">
                                                                <td align="left" class="ClspaddingL" width="10px">
                                                                    <asp:Label ID="lblEnrollNo" runat="server" Text='<%#Eval("RegNo")%>'></asp:Label>
                                                                </td>
                                                                <td class="ClspaddingL">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("StudentName")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:DropDownList ID="cmbSecondLanguage" Width="150px" runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:DropDownList ID="cmbThirdLanguage" AppendDataBoundItems="true" Width="150px"
                                                                        runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                    <asp:HiddenField ID="hidSubjectGroupIds" runat="server" Value="" />
                                                    <asp:HiddenField ID="hidLanguageGroupIds" runat="server" Value="" />
                                                    <asp:HiddenField ID="hidPrimarySection" runat="server" Value="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="1" id="tdBack" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                                ID="UpdatePanel3">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" CausesValidation="true" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                        CssClass="ClsBtn" disable-page="true" Visible="False" OnClick="btnSave_Click" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                                CssClass="ClsBtn" CausesValidation="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
            ID="UpdatePanel1">
            <ContentTemplate>
                <asp:HiddenField ID="hidSortDirection" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidCanEdit" runat="server" Value="Y" />
                <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        _clientlstvwSecondLanguage = "<%=this.lstvwSecondLanguage.ClientID %>"
        _clienthidSubjectGroupIds = "<%=this.hidSubjectGroupIds.ClientID %>"
        _clienthidLanguageGroupIds = "<%=this.hidLanguageGroupIds.ClientID %>"
        _clienthidPrimarySection = "<%=this.hidPrimarySection.ClientID %>"

        function ValidateLanguage(oSrc, args) {

            var index = 0
            var subjectGroups = $("#" + _clienthidSubjectGroupIds).val();
            var RollNos = ""
            if (subjectGroups != "") {
                var Ids = subjectGroups.split("$")
                var rollNo = document.getElementById(_clientlstvwSecondLanguage + "_ctrl" + index + "_lblRollNo")
                while (rollNo != null) {
                    var secondLangSubjectid = document.getElementById(_clientlstvwSecondLanguage + "_ctrl" + index + "_cmbSecondLanguage").value
                    var thirdLangSubjectId = document.getElementById(_clientlstvwSecondLanguage + "_ctrl" + index + "_cmbThirdLanguage").value

                    var secondLangGroupId = 0
                    var thirdLangGroupId = 0

                    for (var k = 0; k < Ids.length; k++) {
                        if (parseInt(Ids[k].split(",")[0]) == parseInt(secondLangSubjectid)) {
                            secondLangGroupId = parseInt(Ids[k].split(",")[1])
                            break;
                        }
                    }

                    for (var j = 0; j < Ids.length; j++) {
                        if (parseInt(Ids[j].split(",")[0]) == parseInt(thirdLangSubjectId)) {
                            thirdLangGroupId = parseInt(Ids[j].split(",")[1])
                            break;
                        }
                    }

                    if (parseInt(secondLangSubjectid) != 0 && parseInt(thirdLangSubjectId) != 0 && secondLangGroupId == thirdLangGroupId) {
                        if (RollNos == "")
                            RollNos = rollNo.innerHTML;
                        else
                            RollNos = RollNos + ", " + rollNo.innerHTML;
                    }

                    index++;
                    rollNo = document.getElementById(_clientlstvwSecondLanguage + "_ctrl" + index + "_lblRollNo")
                }
            }

            if (RollNos != "") {
                oSrc.errormessage = "Selected Second and Third Language should not be from same subject group for row(s) : " + RollNos + "."
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false;
        }

        function ChangeSecondAndThirdLanguage(iValue, iRowIndex) {
            
            var LanguageGroups = $("#" + _clienthidLanguageGroupIds).val();
            var SectionId = $("#" + _clienthidPrimarySection).val();
            var secondLangSubjectid = document.getElementById(_clientlstvwSecondLanguage + "_ctrl" + iRowIndex + "_cmbSecondLanguage")
            var thirdLangSubjectId = document.getElementById(_clientlstvwSecondLanguage + "_ctrl" + iRowIndex + "_cmbThirdLanguage")

            if (LanguageGroups != "") {
                var Ids = LanguageGroups.split("$");

                var sFlag = true;
                if (iValue == 1 && secondLangSubjectid.value == 0) {
                    thirdLangSubjectId.value = 0;
                    sFlag = false;
                }
                else if (iValue == 2 && thirdLangSubjectId.value == 0) {
                    secondLangSubjectid.value = 0;
                    sFlag = false;
                }
                
                if (sFlag == true) {
                    
                    if (SectionId == "N") {
                        for (var j = 0; j < Ids.length; j++) {
                            var s = Ids[j].split(",");
                            if (iValue == 1) {
                                if (s[0] == secondLangSubjectid.value)
                                    thirdLangSubjectId.value = s[1]
                            }
                            else {
                                if (s[1] == thirdLangSubjectId.value)
                                    secondLangSubjectid.value = s[0]
                            }
                        }
                    }
                    else {
                        var s = Ids[0].split(",")
                        if (iValue == 1) {
                            if (s[0] == secondLangSubjectid.value)
                                thirdLangSubjectId.value = s[1]
                            else
                                thirdLangSubjectId.value = s[0]
                        }
                        else {
                            if (s[1] == thirdLangSubjectId.value)
                                secondLangSubjectid.value = s[0]
                            else
                                secondLangSubjectid.value = s[1]
                        }
                    }
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
