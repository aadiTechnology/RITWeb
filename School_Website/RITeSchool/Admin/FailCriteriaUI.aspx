<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="FailCriteriaUI.aspx.cs" Inherits="FailCriteriaUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>

    
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td align="left">
                    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                        <tr>
                            <td align="left">
                                <asp:ValidationSummary ID="valSumError" runat="server" CssClass="LblErrorMsg" />
                            </td>
                            <td>
                                <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" ForeColor="Red" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top" class="GridBorder" style="width: 100%;">
                    <asp:GridView ID="grdvwFailCriteria" runat="server" Width="100%" AutoGenerateColumns="False"
                        PageSize="200" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                        AllowSorting="False" OnRowDataBound="grdvwFailCriteria_RowDataBound" DataKeyNames="Standard_Id,Marks_Grades_configuration_Id,Number_Of_subjects,IsFailCriteriaNotApplicable">
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                            Font-Size="Small"></PagerStyle>
                        <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>" PreviousPageText="<%$ Resources:LocalizedResources, Previous%>"
                            FirstPageText="<%$ Resources:LocalizedResources, First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Standard%>" DataField="Standard_Name" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, FailCriteriaNotApplicable%>">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="chkIsFailCriteriaNotApp" runat="server" Visible="True">
                                    </asp:CheckBox >
                                </ItemTemplate>
                                <ItemStyle Width="15%" Wrap="True" HorizontalAlign="Center"/>
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <HeaderStyle Width="15%" HorizontalAlign="Center" Wrap="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, GradeName%>" SortExpression="Grade_Name">
                                <ItemTemplate>
                                    &nbsp;<asp:DropDownList ID="cmbGrades" runat="server" Visible="True">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle Width="10%" Wrap="True" HorizontalAlign="Center"/>
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <HeaderStyle Width="10%" HorizontalAlign="Center" Wrap="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources,NumberOfSubjects%>" SortExpression="No_of_Subjects">
                                <ItemTemplate>
                                    <asp:TextBox CssClass="MidTxtBox" ID="txtNoOfSubjects" runat="server" MaxLength="1" onblur="extractNumber(this,0,false);"
                                        Text='<%# Eval("Number_Of_Subjects") %>' Visible="True" onkeyup="extractNumber(this,0,false);"
                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                        ondrop="event.returnValue=false" />
                                    <asp:HiddenField ID="hidTotalSubjects" Value='<%# Eval("TotalSubjects") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="15%" HorizontalAlign="Center" Wrap="True" />
                                <EditItemTemplate>
                                    &nbsp;
                                </EditItemTemplate>
                                <HeaderStyle Width="15%" HorizontalAlign="Center" Wrap="True" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="TotalSubjects" HeaderText="<%$ Resources:LocalizedResources,TotalNumberOfSubjects%>" SortExpression="TotalSubjects">
                                <ItemStyle Width="15%" HorizontalAlign="Center" Wrap="True" />
                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Original_Grade_Id%>" DataField="Marks_Grades_configuration_Id"
                                Visible="False" />
                        </Columns>
                        <RowStyle CssClass="ClsGridRow" />
                        <HeaderStyle CssClass="ClsGridHeader" />
                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 15px">
                    <table align="center" border="0" cellpadding="0" cellspacing="3" height="100%" width="100%">
                        <tr>
                            <td align="center" style="height: 30px;" id="tdCancel" runat="server" colspan="3">
                                &nbsp;<asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save%>" runat="server" OnClick="btnSave_Click"
                                    CssClass="ClsBtn" BorderStyle="Solid" BorderWidth="1px" CausesValidation="true" disable-page="true" />
                                <asp:Button ID="imgbtnCancel" Text="<%$ Resources:LocalizedResources, Cancel%>" runat="server" CssClass="ClsBtn" BorderStyle="Solid"
                                    OnClick="imgbtnCancel_Click" Visible="True" BorderWidth="1px" CausesValidation="false" UseSubmitBehavior="false"/></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
             <asp:HiddenField ID="hidCultureInfo" runat="server" />
             <asp:HiddenField ID="hidStandard" runat="server" />
             <asp:HiddenField ID="hidNumberOfSubjectsShouldBeLessThanTotalNumberOfSubjects" runat="server" />
             <asp:HiddenField ID="hidPleaseFixFollowingErrors" runat="server" />
             <asp:HiddenField ID="hidGradeNameShouldBeSelectedForFollowingStandard" runat="server" />
             <asp:HiddenField ID="hidNumberOfSubjectsShouldNotBeBlankForFollowingStandards" runat="server" />
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientGridId = "<%=this.grdvwFailCriteria.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientimgbtnCancel = "<%=this.imgbtnCancel.ClientID %>"

        var Page_IsValid = true;

        function EnterNoOfSubjects() {
        	 Page_IsValid = true;
            if (CheckGradeNameIsNotSelected()) {
                var sMessage = ""
                var sSubjectsMessage = ""
                var n = document.getElementById(_clientGridId).rows.length + 1
                var enterSubjects
                var totalSubject
                for (i = 2; i < n; i++) {
                    j = i - 1
                    var standardname = document.getElementById(_clientGridId).rows[j].cells[0].innerHTML
                    if (i < 10) {
                        totalSubject = _clientGridId + "_ctl0" + i + "_hidTotalSubjects"
                        enterSubjects = _clientGridId + "_ctl0" + i + "_txtNoOfSubjects"
                    }
                    else {
                        totalSubject = _clientGridId + "_ctl" + i + "_hidTotalSubjects"
                        enterSubjects = _clientGridId + "_ctl" + i + "_txtNoOfSubjects"
                    }
                    var noOfSubjects = document.getElementById(enterSubjects).value
                    var valTotalSubjects = document.getElementById(totalSubject).value
                    if (parseInt(noOfSubjects) > parseInt(valTotalSubjects))
                        sSubjectsMessage = sSubjectsMessage + "\n\r -" + document.getElementById("<%=hidStandard.ClientID%>").value + ":" + standardname
                }
                if (sSubjectsMessage != "") {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID %>").value + "\n\r" + document.getElementById("<%=hidNumberOfSubjectsShouldBeLessThanTotalNumberOfSubjects.ClientID %>").value + sSubjectsMessage)
                	 Page_IsValid = false;
                    return false
                }
                else {                    
                    return true
                } 
            }
            return false
        }
        function DisableButtons() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientimgbtnCancel).disabled = true
        }
        function CheckGradeNameIsNotSelected() {
        	 Page_IsValid = true;
            var sGradesMessage = ""
            var sMessage = ""
            var sSubjectsMessage = ""
            var sSaveMessage = ""
            var n = document.getElementById(_clientGridId).rows.length + 1
            var gradeName
            var noOfSubjects
            var num = 0
            for (i = 2; i < n; i++) {
                j = i - 1
                var standardname = document.getElementById(_clientGridId).rows[j].cells[0].innerHTML
                if (i < 10) {
                    noOfSubjects = _clientGridId + "_ctl0" + i + "_txtNoOfSubjects"
                    gradeName = _clientGridId + "_ctl0" + i + "_cmbGrades"
                }
                else {
                    noOfSubjects = _clientGridId + "_ctl" + i + "_txtNoOfSubjects"
                    gradeName = _clientGridId + "_ctl" + i + "_cmbGrades"
                }
                if (document.getElementById(noOfSubjects).value != "" && document.getElementById(gradeName).value == "0") {
                    sGradesMessage = sGradesMessage + "\n\r -" + document.getElementById("<%=hidStandard.ClientID%>").value + ":" + standardname
                }
                else if (document.getElementById(noOfSubjects).value == "" && document.getElementById(gradeName).value != "0") {
                    sSubjectsMessage = sSubjectsMessage + " \n\r -" + document.getElementById("<%=hidStandard.ClientID%>").value + ":" + standardname
                }
                else if (document.getElementById(noOfSubjects).value == "" && document.getElementById(gradeName).value == "0") {
                    num = num + 1
                } 
            }
            if (sGradesMessage != "") {
                if (sSubjectsMessage != "") {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID %>").value + " \n\r" + document.getElementById("<%=hidGradeNameShouldBeSelectedForFollowingStandard.ClientID%>").value + ":" + sGradesMessage + "\n" + document.getElementById("<%=hidNumberOfSubjectsShouldNotBeBlankForFollowingStandards.ClientID%>").value + ":" + sSubjectsMessage)
                	 Page_IsValid = false;
                    return false
                }
                else {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID %>").value + " \n\r" + document.getElementById("<%=hidGradeNameShouldBeSelectedForFollowingStandard.ClientID%>").value + ":" + sGradesMessage)
                	Page_IsValid = false;
                    return false
                } 
            }
            else if (sSubjectsMessage != "") {
                if (sGradesMessage != "") {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID %>").value + " \n\r" + document.getElementById("<%=hidGradeNameShouldBeSelectedForFollowingStandard.ClientID%>").value + ":" + sGradesMessage + "\n" + document.getElementById("<%=hidNumberOfSubjectsShouldNotBeBlankForFollowingStandards.ClientID%>").value + ":" + sSubjectsMessage)
                	Page_IsValid = false;
                    return false
                }
                else {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID%>").value +" \n\r" + document.getElementById("<%=hidNumberOfSubjectsShouldNotBeBlankForFollowingStandards.ClientID%>").value + ":" + sSubjectsMessage)
                	Page_IsValid = false;
                    return false
                } 
            }

            return true
        }
        function SetControlEnability(chkObj, iRowNo) {          
            var gradeName
            var noOfSubjects
            //Grd start row JS ContolId start with 2
            iRowNo = iRowNo + 2
            if (iRowNo < 10) {
                gradeName = document.getElementById(_clientGridId + "_ctl0" + (iRowNo) + "_cmbGrades")
                noOfSubjects = document.getElementById(_clientGridId + "_ctl0" + (iRowNo) + "_txtNoOfSubjects")
            }
            else {
                gradeName = document.getElementById(_clientGridId + "_ctl" + (iRowNo) + "_cmbGrades")
                noOfSubjects = document.getElementById(_clientGridId + "_ctl" + (iRowNo) + "_txtNoOfSubjects")
            }
            if (!chkObj.checked) {
                gradeName.disabled = false;
                noOfSubjects.disabled = false;
                noOfSubjects.value = "";
            }
            else {
                gradeName.disabled = true;
                noOfSubjects.disabled = true;
                gradeName.value = "0";
                noOfSubjects.value = "";
            }
        
        }
    </script>
</asp:Content>
