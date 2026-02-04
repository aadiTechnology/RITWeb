<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="../MasterPages/PopupMaster.master"
    CodeFile="TeacherToSubjectsAssignmentUI.aspx.cs" Inherits="TeacherToSubjectsAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <!-- Data Insert Here -->
                <table border="0" align="center" cellpadding="0" cellspacing="2" style="width: 95%;">
                    <tr>
                        <td align="left" colspan="4" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="ClsGrayMainTitle" width="98%" height="20px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; height: 15px">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblHeading" CssClass="MainTitleHead" runat="server" Text="<%$ Resources:LocalizedResources, AssignTeacherToSubjects %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4" valign="top" height="10%">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="valAddEduDetails" runat="server" ValidationGroup="valGrpAddEduDetails"
                                            CssClass="ClsLabel" />
                                        <asp:CompareValidator ID="cmp_StandardDivision" runat="server" ControlToValidate="cmbStandardDivision"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ClassShouldBeSelected %>"
                                            Operator="NotEqual" ValidationGroup="valGrpAddEduDetails" ValueToCompare="0"> </asp:CompareValidator>
                                        <asp:CompareValidator ID="cmp_Subjects" runat="server" ControlToValidate="cmbSubjects"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SubjectShouldBeSelected %>"
                                            Operator="NotEqual" ValueToCompare="0" ValidationGroup="valGrpAddEduDetails"
                                            Visible="True"></asp:CompareValidator>
                                    </td>
                                    <td align="right" style="width: 23%; padding-right: 0px; height: 19px;" valign="top">
                                        <span class="ClsMdtStar">* </span>
                                        <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, MandatoryFields %>" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" valign="top">
                            <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
                                <tr>
                                    <td align="left" colspan="3" style="height: 19px">
                                        <span class="ClsLblLgnd" style="font-weight: bold; width: 200px">
                                        <asp:Label ID="lblTeacherDetailText" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, TeacherDetails %>"></asp:Label><span class="colonPadding"> :</span></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 15%; height: 1px" class="ClsBorderlight">
                                    <span class="ClsLabel">
                                        <asp:Label ID="lblTeacherNameText" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, TeacherName %>">
                                        </asp:Label><span class="colonPadding"> :</span>                                        
                                        </span>
                                    </td>
                                    <td align="left" class="ClsBorderlight" colspan="3" style="width: 85%; height: 1px">
                                        <asp:Label ID="lblTeacherName" runat="server" CssClass="ClsLblRslt" EnableViewState="False"></asp:Label>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="height: 1px" class="ClsBorderlight">
                                    <span class="ClsLabel">
                                        <asp:Label ID="lblDesignationText" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, Designation %>">
                                        </asp:Label><span class="colonPadding"> :</span>
                                     </span>
                                    </td>
                                    <td align="left" style="width: 25%; height: 1px" class="ClsBorderlight">
                                        <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLblRslt" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsBorderlight" style="width: 15%; height: 1px">
                                    <span class="ClsLabel">
                                        <asp:Label ID="Label1" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, StandardDetails %>">
                                        </asp:Label><span class="colonPadding"> :</span>
                                        </span>
                                    </td>
                                    <td align="left" class="ClsBorderlight" style="width: 25%; height: 1px">
                                        <asp:Label ID="lblStandards" runat="server" CssClass="ClsLblRslt" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="height: 1px" class="ClsBorderlight">
                                    <span class="ClsLabel">
                                        <asp:Label ID="Label2" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, SubjectsDetails %>">
                                        </asp:Label><span class="colonPadding"> :</span>
                                        </span>
                                    </td>
                                    <td align="left" colspan="3" style="height: 1px" class="ClsBorderlight">
                                        <asp:Label ID="lblSubjects" runat="server" CssClass="ClsLblRslt" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="4" style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="4" style="height: 1px">
                                        <span class="ClsLblLgnd" style="font-weight: bold">
                                            <asp:Label ID="lblAssignTeacherToAssignText" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, AssignTeacherTo %>">
                                            </asp:Label><span class="colonPadding"> :</span></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" width="100%" align="left">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <table width="100%" cellpadding="0" cellspacing="2">
                                                    <tr>
                                                        <td align="left" style="width: 18%;" class="ClsBorderlight">
                                                            <span class="ClsLabel">
                                                                <asp:Label ID="lblClassText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Class %>"></asp:Label><span class="colonPadding"> :</span></span>
                                                        </td>
                                                        <td align="left" style="width: 25%;">
                                                            <asp:DropDownList ID="cmbStandardDivision" runat="server" CssClass="MidTxtBox" OnSelectedIndexChanged="cmbStandardDivision_SelectedIndexChanged"
                                                                AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar" style="color: #ff0000">*</span>
                                                        </td>
                                                        <td align="left" style="width: 10%;" class="ClsBorderlight">
                                                            <span class="ClsLabel"><asp:Label ID="lblSubjectsText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Subject %>"></asp:Label><span class="colonPadding"> :</span></span></span>
                                                        </td>
                                                        <td align="left" style="width: 23%;">
                                                            <asp:DropDownList ID="cmbSubjects" runat="server" CssClass="MidTxtBox" EnableViewState="true"
                                                                OnSelectedIndexChanged="cmbSubjects_SelectedIndexChanged" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <span style="color: #ff0000">*
                                                                <asp:HiddenField ID="hidTeacherName" runat="server" />
                                                            </span>
                                                        </td>
                                                        <td align="left" style="width: 23%">
                                                            <asp:Button ID="btnAddDetails" runat="server" Text="<%$ Resources:LocalizedResources, AddSubjects %>" OnClick="btnAddDetails_Click"
                                                                CssClass="ClsBtnMid" BorderStyle="Solid" ValidationGroup="valGrpAddEduDetails"
                                                                BorderWidth="1px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" width="100%" align="center">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="2" cellspacing="1" style="width: 100%; height: 100%;">
                                                    <tr>
                                                        <td colspan="3" align="center">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblDuplicateDetails" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr align="center">
                                                                    <td align="center">
                                                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                            Visible="False" EnableViewState="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <tr>
                                                                <td align="center" style="height: 1px">
                                                                </td>
                                                                <td align="center" style="height: 1px">
                                                                </td>
                                                                <td align="center" style="height: 1px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="1">
                                                                    <asp:Label ID="lblSaveSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                                        Visible="false" EnableViewState="false" CssClass="ClsLabel" Font-Bold="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="3">
                                                                    <asp:GridView CssClass="GridBorder" ID="grdDivisionWiseSubjects" runat="server" Width="80%"
                                                                        Height="70%" AutoGenerateColumns="False" PageSize="10" CellPadding="0" CellSpacing="1"
                                                                        ForeColor="#333333" GridLines="None" OnRowDataBound="grdDivisionWiseSubjects_RowDataBound"
                                                                        DataKeyNames="Standard_Division_Id      ,Subject_Id               ,Teacher_Subject_Id&#9;     ,Teacher_Id &#9;&#9;    ">
                                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                        </PagerStyle>
                                                                        <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                            FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <input id="chkAllDelete" type="checkbox" runat="server" onclick="CheckAllOrUncheckAllGridItems(document,_ClientGridId ,this,'chkIsSelected')" />
                                                                                </HeaderTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkIsSelected" runat="server" Checked="false" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="StandardDivision" HeaderText="<%$ Resources:LocalizedResources, Class %>" SortExpression="StandardDivision">
                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Subject_Name" HeaderText="<%$ Resources:LocalizedResources, Subject %>" SortExpression="Subject_Name">
                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="ClsGridRow" />
                                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="3">
                                                                    <div style="width: 80%" id="divNote" runat="server" visible="false">
                                                                        <table>
                                                                            <tr>
                                                                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                                                <asp:Label ID="spnNote" Font-Bold="true" CssClass="LblNrmlB" runat="server" Text="<%$ Resources:LocalizedResources, Note1 %>"></asp:Label>
                                                                                <span class="colonPadding">:</span></span>
                                                                                </td>
                                                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                                                    <asp:Label ID="lblVerifyNote1" runat="server" BorderWidth="0px" CssClass="LblSmlV" 
                                                                                        Text="<%$ Resources:LocalizedResources, TeacherVerifyNote %>"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="hidFlag" runat="server" />
                            <asp:HiddenField ID="hidTeacherId" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" id="tdSave" runat="server">
                            &nbsp;
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSave" runat="server" CausesValidation="true" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtnSml"
                                        BorderStyle="Solid" OnClick="btnSave_Click" UseSubmitBehavior="false" />
                                    <asp:Button ID="btnBack" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtnSml" BorderStyle="Solid" runat="server"
                                        BorderWidth="1px" OnClientClick="window.close(); return false;" CausesValidation="false" UseSubmitBehavior="false" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="grdDivisionWiseSubjects" EventName="RowCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidAreYouSureDeleteDetails" runat="server" />
                <asp:HiddenField ID="hidAtLeastOneClassSubjectSelected" runat="server" />
                 <asp:HiddenField ID="hidSomeClassSubjectsAlreadyAssignedMsg" runat="server" />
                 <asp:HiddenField ID="hidAreYouSureYouWantToContinue" runat="server" />
                <asp:HiddenField ID="hidCultureInfo" runat="server" />
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        var blanks = " \t\n\r";  // Ek whitespace chars

        _ClientGridId = "<%=this.grdDivisionWiseSubjects.ClientID %>";


        _clienthidFlagId = "<%=this.hidFlag.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnBack = "<%=this.btnBack.ClientID %>";
        _clientValSummary = "<%=this.valAddEduDetails.ClientID %>";

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);

        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "maroon";
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
        }

        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "#a3c07b";
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
        }

        //This function is used to clear validation summary.       
        function ClearValSummary() {
            var valSum = document.getElementById(_clientValSummary);
            if (valSum != null)
                document.getElementById(_clientValSummary).style.display = "none";
        }
        //This function is used to validate grid data.                                                       
        function validateGridData(oSrc, args) {
            var grdViewElement = document.getElementById(_ClientGridId)
            if (grdViewElement == null) {
                args.IsValid = false;
                return true;
            }
            else
                return false;
        }
        //This function is used to disable buttons.
        function DisableButtons() {
            if (document.getElementById(_clientbtnSave) != null) {
                document.getElementById(_clientbtnSave).disabled = true;
                document.getElementById(_clientbtnBack).disabled = true;
            }
            else
                document.getElementById(_clientbtnBack).disabled = true;
        }


        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientbtnBack)
                CloseCurrentWindow();
        }
        //This function is used to close current window.
        function CloseCurrentWindow() {
            window.close();
            window.opener.focus();
        }
        //This function is used to check duplicate assignment.
        function saveChk() {
            var bResult = true;
            if (document.getElementById(_ClientGridId) != null) {
                if (document.getElementById(_clienthidFlagId).value == "true") {
                    var ErrMsg = document.getElementById("<%=this.hidSomeClassSubjectsAlreadyAssignedMsg.ClientID %>").value + "\n" + document.getElementById("<%=this.hidAreYouSureYouWantToContinue.ClientID %>").value;
                    if (!window.confirm(ErrMsg))
                        bResult = false;
                    if (bresult = true) {
                        document.getElementById(_clientbtnSave).disabled = true;
                        document.getElementById(_clientbtnBack).disabled = true;
                    }
                    return bResult;
                }
                if (bresult = true) {
                    document.getElementById(_clientbtnSave).disabled = true;
                    document.getElementById(_clientbtnBack).disabled = true;
                }
                return bResult;
            }
            else
                alert(document.getElementById("<%=this.hidAtLeastOneClassSubjectSelected.ClientID %>").value);
        }
        //This function is used to display confirmation message at the time of assignment delete.
        function ConfirmDelete() {
            var bResult = true;
            {
                if (!window.confirm(document.getElementById("<%=this.hidAreYouSureDeleteDetails.ClientID %>").value))
                { bResult = false; }
            }
            return bResult;
        }

    </script>
</asp:Content>
