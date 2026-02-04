<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentBulkEmailUI.aspx.cs" Inherits="StudentBulkEmailUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
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
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" Height="20px"
                                            Width="100%" EnableViewState="False" CssClass="LblErrorMsg"></asp:Label>
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="NewClsLabel"
                                            ValidationGroup="Save" />
                                        <asp:CustomValidator ID="vstValLang" runat="server" Display="None" ErrorMessage=""
                                            ClientValidationFunction="ValidateEmailAddress" ValidationGroup="Save"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ErrorMessage=""
                                            ClientValidationFunction="CheckEmailIsDuplicate" ValidationGroup="Save"></asp:CustomValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveUp" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
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
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
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
                                                        CssClass="ClsBtn" disable-page="true" Visible="False" OnClick="btnSave_Click"
                                                        ValidationGroup="Save" />
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
                                <table style="width: 100%; height: 100%;" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ListView ID="lstvwStudentEmail" ItemPlaceholderID="ContactRowContainer" runat="server"
                                                        DataKeyNames="StudentId,StandardId,DivisionId" OnItemDataBound="lstvwStudentEmail_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table style="width: 60%; height: 100%; color: #333333" runat="server" id="tblContacts"
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
                                                                    <th align="left" class="ClspaddingL" width="300px">
                                                                        <asp:Label ID="Label5" runat="server" Text="Email Address"></asp:Label>
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
                                                                <td align="center" class="ClspaddingL">
                                                                    <asp:TextBox ID="txtEmailAddress" class="LrgTxtBox" runat="server" Width="260px"
                                                                        Text='<%#Eval("EmailAddress")%>'></asp:TextBox>
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
                                                                <td align="center" class="ClspaddingL">
                                                                    <asp:TextBox ID="txtEmailAddress" class="LrgTxtBox" runat="server" Width="260px"
                                                                        Text='<%#Eval("EmailAddress")%>'></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                    <asp:HiddenField ID="hidTotalStudentCount" runat="server" Value="" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSaveUp" EventName="Click" />
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
                            <td align="center" colspan="1" id="tdBack" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                                ID="UpdatePanel4">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" CausesValidation="true" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                        CssClass="ClsBtn" disable-page="true" Visible="False" OnClick="btnSave_Click"
                                                        ValidationGroup="Save" />
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
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        _clientlstvwStudentEmail = "<%=this.lstvwStudentEmail.ClientID %>"
        _clienthidTotalStudentCount = "<%=this.hidTotalStudentCount.ClientID %>"

        function ValidateEmailAddress(oSrc, args) {            
            var lbl
            var iRowCount = 0
            lbl = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + iRowCount + "_lblRollNo")
            var sEmptyCount = 0;
            var sRollNos = "";            

            while (lbl != null) {
                var txt = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + iRowCount + "_txtEmailAddress")
                var lblRollNo = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + iRowCount + "_lblRollNo")
                var TotalStudents = document.getElementById(_clienthidTotalStudentCount).value;
                var sEmail = txt.value;
                if (!isEmpty(sEmail)) {
                    if (!isEmail(sEmail)) {
                        sRollNos = sRollNos + ", " + lblRollNo.innerText;
                    }                    
                }
                else
                    sEmptyCount++;               

                iRowCount++;
                lbl = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + iRowCount + "_lblRollNo");
            }

            if (sEmptyCount == TotalStudents) {
                oSrc.errormessage = "At least one Email Address Should be entered."
                args.IsValid = false
                return true;
            }
            else if (sRollNos != "") {
                oSrc.errormessage = "Email Address should be in proper format for Roll No(s) : " + sRollNos.substr(1) + "."
                args.IsValid = false
                return true;
            }          

            args.IsValid = true
            return false;
        }

        function CheckEmailIsDuplicate(oSrc, args) {
            debugger;          
            var lbl
            var iRowCount = 0
            lbl = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + iRowCount + "_lblRollNo")
            var sEmptyCount = 0;
            var sRollNos = "";
            var sDuplicateRollNos = "";

            while (lbl != null) {
                var j = iRowCount + 1;
                var Innerlbl = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + j + "_lblRollNo")
                var txt = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + iRowCount + "_txtEmailAddress")
                var lblRollNo = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + iRowCount + "_lblRollNo")
                var sEmail = txt.value;
                while (Innerlbl != null) {
                    var Sectxt = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + j + "_txtEmailAddress")
                    var SeclblRollNo = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + j + "_lblRollNo")
                    var SecEmail = Sectxt.value;

                    if (!isEmpty(SecEmail) && isEmail(SecEmail)) {
                        if (sEmail == SecEmail) {
                            sDuplicateRollNos = sDuplicateRollNos + ", " + SeclblRollNo.innerText;
                        }
                    }

                    j++;
                    var Innerlbl = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + j + "_lblRollNo")
                }
                if (SeclblRollNo != "") {
                    break;
                }

                iRowCount++;
                lbl = document.getElementById(_clientlstvwStudentEmail + "_ctrl" + iRowCount + "_lblRollNo");
            }

            if (sDuplicateRollNos != "") {
                oSrc.errormessage = "Email Address should not be duplicate for Roll No(s) : " + sDuplicateRollNos.substr(1) + "."
                args.IsValid = false
                return true;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
