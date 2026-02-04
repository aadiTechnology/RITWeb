<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentXseedGradeAssignmentUI.aspx.cs" Inherits="StudentXseedGradeAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnl11" runat=server>
                                    <ContentTemplate>                            
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsLabel" ForeColor="Red" />
                                        <asp:CustomValidator ID="custValLearningOutcomes" runat="server" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, GradeShouldBeAssignedForEveryOutcome%>"
                                            ClientValidationFunction="ValidateOutcomes"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValObservation" runat="server" Display="None" ClientValidationFunction="ValidateObservation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateSubjectRemark"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right">
                                <span class="ClsLabelNrml" style="color: Red;">*
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label></span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="right" width="50px">
                                <span class="ClsLblLgnd">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                    <span id="Span2" class="colonPadding">:</span> </span>
                            </td>
                            <td align="left" class="ClsHilightBGB" width="150px">
                                <asp:Label ID="lblClass" runat="server" Text="class"></asp:Label>
                            </td>
                            <td width="10px">
                            </td>
                            <td align="left" width="90px">
                                <span class="ClsLblLgnd">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Assessment%>"></asp:Label>
                                    <span id="Span1" class="colonPadding">:</span> </span>
                            </td>
                            <td align="left" class="ClsHilightBGB" width="200px">
                                <asp:Label ID="lblAssessment" runat="server" Text="Assessment"></asp:Label>
                            </td>
                            <td width="10px">
                            </td>
                            <td align="left" width="65px">
                                <span class="ClsLblLgnd">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, Subject1%>"></asp:Label>
                                    <span id="Span3" class="colonPadding">:</span> </span>
                            </td>
                            <td align="left" class="ClsHilightBGB" width="200px">
                                <asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
                            </td>
                        </tr>
                        <asp:Panel runat="server" ID="pnlSubmitStatus" Visible="false">
                            <tr>
                                <td colspan="8" align="center" class="ClsHilightBGB" visible="false">
                                    <asp:Label ID="lblSubmitMessage" runat="server" Text="<%$ Resources:LocalizedResources, StudentGradesAreAlreadySubmitted%>"
                                        EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <table align="center" width="100%">
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="80px">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, Student%>"></asp:Label>
                                                        <span id="Span4" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left" valign="middle">
                                                    <asp:DropDownList ID="cmbStudent" runat="server" CssClass="ExLrgCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbStudent_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span class="ErrMsg">*</span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" width="130px">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, SubjectSection%>"></asp:Label>
                                                        <span id="Span5" class="colonPadding">:</span></span>
                                                </td>
                                                <td align="left" valign="middle">
                                                    <asp:DropDownList ID="cmbSubjectSections" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbSubjectSections_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span class="ErrMsg">*</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblMessage" runat="server" Text="" CssClass="ClsLabelNrml" ForeColor="Blue"
                                            EnableViewState="false" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div id="divContainer" class="GridBorder" runat="server" visible="false" style="width: 800px;
                                            height: 390px; overflow: scroll">
                                            <asp:ListView ID="lstvwLearningOutcome" runat="server" DataKeyNames="LearningOutcomeGradeId,LearningOutcomeConfigId"
                                                OnItemDataBound="lstvwLearningOutcome_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                        cellspacing="1" class="GridBorder">
                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                            <th align="left" style="padding-left: 10px;" width="75%">
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, LearningOutcome%>"></asp:Label>
                                                            </th>
                                                            <th align="left" style="padding-left: 10px;">
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, Grade%>"></asp:Label>
                                                            </th>
                                                        </tr>
                                                        <tr id="trHeaderContol" runat="server" class="ClsGridHeader">
                                                            <th align="left" style="padding-left: 10px;" width="75%">
                                                            </th>
                                                            <th align="left" style="padding-left: 10px;">
                                                                <asp:DropDownList ID="cmbAllGrades" runat="server" CssClass="MidCombo">
                                                                </asp:DropDownList>
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="trItem" runat="server" class="ClsGridRow">
                                                        <td class="paddingL">
                                                            <asp:Label ID="lblLearningOutcome" runat="server" Text='<%#Eval("LearningOutcome") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL">
                                                            <asp:DropDownList ID="cmbGrades" runat="server" CssClass="MidCombo">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                                        <td class="paddingL">
                                                            <asp:Label ID="lblLearningOutcome" runat="server" Text='<%#Eval("LearningOutcome") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL" valign="middle">
                                                            <asp:DropDownList ID="cmbGrades" runat="server" CssClass="MidCombo">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:ListView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table id="tblObservation" runat="server" width="800px" visible="false">
                                            <tr>
                                                <td width="160">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, FacilitatorsObservation%>"></asp:Label>
                                                        <span id="Span6" class="colonPadding">:</span> </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtObservation" runat="server" MaxLength="1000" TextMode="MultiLine"
                                                        Width="100%" Height="50px"></asp:TextBox>
                                                    <%--<span class="ErrMsg" style="vertical-align: middle;">*</span>--%>
                                                    
                                                </td>
                                            </tr>
                                            <tr id="trSubjectRemark" runat="server" visible="true">
                                                <td colspan="2">
                                                    <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <hr style="border-style: dashed; border-width: thin; color:Silver" />
                                                        </td>                                                       
                                                    </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="Label10" runat="server" Text="Subject Remark" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>                                                        
                                                            <td>
                                                                <asp:TextBox ID="txtSubjectRemark" runat="server" MaxLength="300" TextMode="MultiLine"
                                                                    Width="100%" Height="50px"></asp:TextBox>                                                                                                                
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>                                                
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trNoRecordFoundMessage" runat="server" visible="false">
                                    <td class="LblNoRecord" align="center">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="18%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>"
                                                        CssClass="ClsBtn" CausesValidation="false" />
                                                </td>
                                                <td align="center">
                                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                                        CssClass="ClsBtn" Visible="False" OnClick="btnSave_Click" />
                                                </td>
                                            </tr>
                                            <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidAssessmentId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidSubjectId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidLearningOutcomesObservationId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidTeacherId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidIsReadOnly" runat="server" />
                                            <asp:HiddenField ID="hidFrom" runat="server" />
                                            <asp:HiddenField ID="hidIsAbsent" runat="server" />
                                            <asp:HiddenField ID="hidIsExempted" runat="server" />
                                            <asp:HiddenField ID="HidObservationLengthShouldBeLess" runat="server" />
                                            <asp:HiddenField ID="HidObservationShouldNotBeBlank" runat="server" />
                                            <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                            <asp:HiddenField ID="hidSchoolId" runat="server"  Value="0"/>
                                            <asp:HiddenField ID="hidRemarkLength" runat="server" Value="0" />
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clinetlstvwLearningOutcome = "<%=this.lstvwLearningOutcome.ClientID %>";
        _clienttxtObservation = "<%=this.txtObservation.ClientID %>";
        _clientcstValObservation = "<%=this.cstValObservation.ClientID %>";
        _clienthidIsAbsent = "<%=this.hidIsAbsent.ClientID %>";
        _clienthidIsExempted = "<%=this.hidIsExempted.ClientID %>";
        _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>";
        _clienttxtSubjectRemark = "<%=this.txtSubjectRemark.ClientID %>"
        _clienthidRemarkLength = "<%=this.hidRemarkLength.ClientID %>"

        function ValidateOutcomes(oSrc, args) {
            var rowNumber = 0;
            var isAllSelected = true;
            var grades = document.getElementById(_clinetlstvwLearningOutcome + '_ctrl' + rowNumber + '_cmbGrades');
            while (grades != null) {
                if (grades.value == 0) {
                    isAllSelected = false;
                    break;
                }
                grades = document.getElementById(_clinetlstvwLearningOutcome + '_ctrl' + rowNumber + '_cmbGrades');
                rowNumber++;
            }
            if (!isAllSelected) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function SelectAll(grade) {
            var rowNumber = 0;
            var isAllSelected = true;
            var grades = document.getElementById(_clinetlstvwLearningOutcome + '_ctrl' + rowNumber + '_cmbGrades');
            while (grades != null) {
                grades.value = grade.value;
                grades = document.getElementById(_clinetlstvwLearningOutcome + '_ctrl' + rowNumber + '_cmbGrades');
                rowNumber++;
            }
        }

        function ValidateObservation(oSrc, args) {
            var rowNumber = 0;
            var isSelected = true;

            var considerAsAbsent = document.getElementById(_clienthidIsAbsent).value;
            var considerAsExempted = document.getElementById(_clienthidIsExempted).value;

            var grades = document.getElementById(_clinetlstvwLearningOutcome + '_ctrl' + rowNumber + '_cmbGrades');
            while (grades != null) {
                if (grades.value != considerAsAbsent && grades.value != considerAsExempted) {
                    isSelected = false;
                    break;
                }
                grades = document.getElementById(_clinetlstvwLearningOutcome + '_ctrl' + rowNumber + '_cmbGrades');
                rowNumber++;
            }

            if (!isSelected) {
                var observation = document.getElementById(_clienttxtObservation).value;
                var SchoolId = document.getElementById(_clienthidSchoolId).value;
                if (SchoolId != 18) {
//                    if (observation.trim() == '') {
//                        document.getElementById(_clientcstValObservation).errormessage = document.getElementById("<%=this.HidObservationShouldNotBeBlank.ClientID %>").value
//                        document.getElementById(_clientcstValObservation).innerHTML = document.getElementById("<%=this.HidObservationShouldNotBeBlank.ClientID %>").value
//                        args.IsValid = false;
//                        return true;
//                    }

                    if (observation.length > 1000) {
                        document.getElementById(_clientcstValObservation).errormessage = document.getElementById("<%=this.HidObservationLengthShouldBeLess.ClientID %>").value
                        document.getElementById(_clientcstValObservation).innerHTML = document.getElementById("<%=this.HidObservationLengthShouldBeLess.ClientID %>").value
                        args.IsValid = false;
                        return true;
                    }
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateSubjectRemark(oSrc, args) {
            var remark = $('#' + _clienttxtSubjectRemark).val().trim()
            var maxLength = parseInt(document.getElementById(_clienthidRemarkLength).value)
            if (remark.length > maxLength) {
                oSrc.errormessage = "Subject Remark length should not be greater than " + maxLength + ".";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
