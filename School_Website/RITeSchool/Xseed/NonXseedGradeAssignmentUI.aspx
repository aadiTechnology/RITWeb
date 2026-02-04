<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="NonXseedGradeAssignmentUI.aspx.cs" Inherits="NonXseedGradeAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div style="padding-left: 10px;">
        <a id="top"></a>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td align="left">
                    <table width="100%">
                        <tr align="center" id="trValSummary" runat="server">
                            <td align="center" id="trValidationSummary">
                                <asp:ValidationSummary ID="valSumNonXseedGrades" CssClass="LblErrorMsg" ShowSummary="true"
                                    runat="server" />
                                <asp:CustomValidator ID="cstvalValidateOservation" runat="server" ClientValidationFunction="ValidateObservation"
                                    SetFocusOnError="True" Display="None" ErrorMessage="Observation should not be blank."></asp:CustomValidator>
                                <asp:CustomValidator ID="cstvalLengthObservation" runat="server" ClientValidationFunction="ValidateLengthObservation"
                                    SetFocusOnError="True" Display="None" ErrorMessage="Observation should not be blank."></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>                                
                                <table width="100%">
                                    <tr align="center" valign="top">
                                        <td align="center" class="GridWidth">
                                            <table cellpadding="0" cellspacing="1" width="90%">
                                                <tr>
                                                    <td align="center" style="padding-bottom: 10px">
                                                        <table cellpadding="0" cellspacing="1">
                                                            <tr>
                                                                <td class="ClsPaddingR">
                                                                    <span class="ClsLblLgnd" id="lblClassvName">Class :</span>&nbsp;
                                                                </td>
                                                                <td class="ClsHilightBGB" width="80px">
                                                                    <asp:Label ID="lblClass" runat="server" EnableViewState="False"></asp:Label>
                                                                </td>
                                                                <td class="ClsPaddingR">
                                                                </td>
                                                                <td class="ClsPaddingR">
                                                                    <span class="ClsLblLgnd" id="LblAssessmentName">Assessment :</span>
                                                                </td>
                                                                <td class="ClsHilightBGB" width="150px">
                                                                    <asp:Label ID="LblAssessment" runat="server" EnableViewState="False"></asp:Label>
                                                                </td>
                                                                <td class="ClsPaddingR">
                                                                </td>
                                                                <td class="ClsPaddingR">
                                                                    <span class="ClsLblLgnd">Subject Name :</span>
                                                                </td>
                                                                <td class="ClsHilightBGB" width="150px">
                                                                    <asp:Label ID="lblDataSubjectName" runat="server" EnableViewState="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                 <asp:Panel runat="server" ID="pnlSubmitStatus" Visible="false">
                                                <tr>
                                                            <td colspan="4" align="center" class="ClsHilightBGB" visible="false">
                                                                <asp:Label ID="lblSubmitMessage" runat="server" Text="Student grades are already submitted."
                                                                    EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                  </asp:Panel>
                                            </table>
                                        </td>
                                    </tr>
                                </table>                                
                            </td>
                        </tr>
                        <tr id="trlstvwStudentGrade" runat="server">
                            <td align="center">
                                <table id="tblStudentGrades" runat="server" align="center" width="90%">
                                    <tr align="center" style="width: 100%">
                                        <td align="center" style="width: 100%">
                                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" ID="upnlMarksgrd" UpdateMode="Conditional">
                                                <ContentTemplate>                                                    
                                                    <asp:ListView ID="lstvwStudentGradeDetails" runat="server" DataKeyNames="YaerwiseStudentId"
                                                        OnItemDataBound="lstvwStudentGradeDetails_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table align="center" width="800px" runat="server" id="tblGradeDetails" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" width="50px">
                                                                        Roll No.
                                                                    </th>
                                                                    <th align="left" class="paddingL">
                                                                        Student Name
                                                                    </th>
                                                                    <th align="center">
                                                                        Grade
                                                                    </th>
                                                                    <th align="center">
                                                                        Observations
                                                                    </th>
                                                                </tr>
                                                                <tr id="trTopHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" width="50px">
                                                                    </th>
                                                                    <th align="left">
                                                                    </th>
                                                                    <th align="center">
                                                                        <asp:DropDownList ID="cmbAllGrades" runat="server" CssClass="MidCombo" Width="100px">
                                                                        </asp:DropDownList>
                                                                    </th>
                                                                    <th align="center">
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RollNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:DropDownList ID="cmbGrades" CssClass="MidCombo" runat="server" Width="100px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtObservations" runat="server" Text='<%# Eval("Observations") %>'
                                                                        Width="300px" Height="30px" CssClass="MidTxtBox" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RollNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:DropDownList ID="cmbGrades" CssClass="MidCombo" runat="server" Width="100px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtObservations" runat="server" Text='<%# Eval("Observations") %>'
                                                                        Width="300px" Height="30px" CssClass="MidTxtBox" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    No record found.
                                                                </td>
                                                            </tr>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>                                                    
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr align="center" valign="top">
                            <td align="center">
                                <asp:UpdatePanel ID="UpdtPnl1" runat="server">
                                    <ContentTemplate>
                                        <table align="center" class="GridWidth">
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Text="Back" OnClick="btnBack_Click"
                                                        UseSubmitBehavior="false" TabIndex="0" CausesValidation="False" />
                                                    <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" OnClick="btnSave_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidStandardDivisionId" runat="server" />
        <asp:HiddenField ID="hidSubjectId" runat="server" />
        <asp:HiddenField ID="hidAssessmentId" runat="server" />
        <asp:HiddenField ID="hidTeacherId" runat="server" />
        <asp:HiddenField ID="hidIsReadOnly" runat="server" />
        <asp:HiddenField ID="hidFrom" runat="server" />
        <asp:HiddenField ID="hidIsAbsent" runat="server" />
        <asp:HiddenField ID="hidIsExempted" runat="server" />
    </div>

    <script language="javascript" type="text/javascript">
        _clientStudentGradeDetailsListview = "<%=this.lstvwStudentGradeDetails.ClientID %>"
        _clientcstvalcstvalValidateOservation = "<%=this.cstvalValidateOservation.ClientID %>";
        _clientcstvalcstvalLengthObservation = "<%=this.cstvalLengthObservation.ClientID %>";
        _clienthidIsAbsent = "<%=this.hidIsAbsent.ClientID %>";
        _clienthidIsExempted = "<%=this.hidIsExempted.ClientID %>";

        function SelectAllControls(objid, ListIndex) {
            var cmbGrades = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + ListIndex + "_cmbGrades");
            var txtObservation = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + ListIndex + "_txtObservations");
            if (cmbGrades.value != 0 && cmbGrades.value != "9" && cmbGrades.value != "10") {
                txtObservation.disabled = false;
            }
            else {
                txtObservation.value = "";
                txtObservation.disabled = true;
            }
        }


        function ValidateObservation(oSrc, args) {
            var bResult = true
            var cmbGrades
            var iRowCount = 0
            var emptyMesage = "";
            cmbGrades = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + iRowCount + "_cmbGrades")
            while (cmbGrades != null) {
                if (cmbGrades.value != 0 && cmbGrades.value != document.getElementById(_clienthidIsAbsent).value && cmbGrades.value != document.getElementById(_clienthidIsExempted).value) {
                    var txtObservation = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + iRowCount + "_txtObservations")
                    if (txtObservation.value.trim() == "")
                        emptyMesage = emptyMesage + ", " + (iRowCount + 1);
                }
                iRowCount = iRowCount + 1
                cmbGrades = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + iRowCount + "_cmbGrades")
            }
            if (emptyMesage.length > 1) {
                emptyMesage = emptyMesage.substring(1);
                document.getElementById(_clientcstvalcstvalValidateOservation).errormessage = "Observation should not be blank for row(s): " + emptyMesage;
                document.getElementById(_clientcstvalcstvalValidateOservation).innerHTML = "Observation should not be blank for row(s): " + emptyMesage;                //                
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function SelectAll(grade) {
            var rowNumber = 0;
            var txtObservation = "";
            var isAllSelected = true;
            var grades = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + rowNumber + "_cmbGrades");
            while (grades != null) {
                grades.value = grade.value;
                txtObservation = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + rowNumber + "_txtObservations")
                if (grade.value != "0" && grades.value != "9" && grades.value != "10") {
                    if (txtObservation.value != null) {
                        txtObservation.disabled = false
                    }
                }
                else {
                    if (txtObservation.value != null) {
                        txtObservation.disabled = true
                        txtObservation.value = "";
                    }
                }
                rowNumber++;
                grades = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + rowNumber + "_cmbGrades");
                
            }
        }


        function ValidateLengthObservation(oSrc, args) {
            var bResult = true
            var cmbGrades
            var iRowCount = 0
            var emptyMesage = "";
            var sStudentObservation = "";
            cmbGrades = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + iRowCount + "_cmbGrades")
            while (cmbGrades != null) {
                if (cmbGrades.value != 0) {
                    var txtObservation = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + iRowCount + "_txtObservations")
                    if (txtObservation.value != null) {
                        sStudentObservation = txtObservation.value
                        if (sStudentObservation.length > 500)
                            emptyMesage = emptyMesage + ", " + (iRowCount + 1);

                    }
                }
                iRowCount = iRowCount + 1
                cmbGrades = document.getElementById(_clientStudentGradeDetailsListview + "_ctrl" + iRowCount + "_cmbGrades")
            }
            if (emptyMesage.length > 1) {
                emptyMesage = emptyMesage.substring(1);
                document.getElementById(_clientcstvalcstvalLengthObservation).errormessage = "Observation should not be greater than 500 characters for row(s): " + emptyMesage;
                document.getElementById(_clientcstvalcstvalLengthObservation).innerHTML = "Observation should not be greater than 500 characters for row(s): " + emptyMesage;                //                
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

    </script>

</asp:Content>
