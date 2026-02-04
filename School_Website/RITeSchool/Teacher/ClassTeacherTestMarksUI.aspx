<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="ClassTeacherTestMarksUI.aspx.cs" Inherits="ClassTeacherTestMarksUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    
    <div class="MainBodyDiv">
        <table width="97%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr id="tdErr" runat="server" visible="false">
                                                <td align="center" width="100%" class="ClsHilightBGB">
                                                    <asp:Label ID="lblError" runat="server" BorderWidth="0px" CssClass="LblNrmlB" Font-Bold="True"
                                                        EnableViewState="false"></asp:Label>&nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="bottom">
                                                    <table id="tblFilter" runat="server">
                                                        <tr>
                                                            <td align="left" id="tdTeacher" runat="server" class="ClsBorderlight" colspan="1">
                                                                <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                    Font-Bold="True" Text="Select Class Teacher :"></asp:Label>&nbsp;
                                                            </td>
                                                            <td align="left" colspan="1">
                                                                <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="ExLrgCombo"
                                                                    OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged" Width="260px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                            </td>
                                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                                <span class="ClsLblLgnd" style="font-weight: bold">Select Exam :</span>&nbsp;
                                                            </td>
                                                            <td align="left" colspan="1">
                                                                <asp:DropDownList ID="cmbTests" runat="server" CssClass="LrgCombo" OnSelectedIndexChanged="cmbTest_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" id="tdToppers" runat="server" colspan="1" class="ClsPaddingGen LblNrmlB ">
                                                                <%--<asp:HyperLink Height="18px" ID="hlnkToppers" CssClass="ClsHilightTextB ToprLinkHlilight "
                                                                    Enabled="False" NavigateUrl="../Student/ExamToppersUI.aspx" Style="padding-right: 3px;"
                                                                    Target="_blank" runat="server" Text="Toppers"></asp:HyperLink>--%>
																	<%if (!SchoolBase.Settings.IsMiniSite) %>
																	<%{ %>
                                                                <span ID="hlnkToppers" style="cursor:pointer;" runat="server" class="ToprLinkHlilight LblNrmlB ClsPaddingGen"><u>Toppers</u></span>
																<%} %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="trgrdSubjects" visible="false">
                                    <td align="center">
                                        <div id="divGridView" runat="server" visible="true">
                                            <table width="900px" id="tblStudentRow" runat="server" class="GridBorder">
                                                <tr>
                                                    <td align="center" class="ClsGridHeader">
                                                        <span>Students</span>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:GridView CssClass="GridBorder" ID="grdSubjects" runat="server" AutoGenerateColumns="False"
                                                Height="100%" AllowPaging="False" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                GridLines="None" DataKeyNames="Standard_Division_Id,Is_Submitted,Subject_Id"
                                                OnRowDataBound="grdSubjects_RowDataBound" Width="900px">
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                </PagerStyle>
                                                <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                    FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                <Columns>
                                                    <asp:HyperLinkField DataNavigateUrlFields="Subject_Id,Standard_Division_Id,SchoolWise_Test_Id"
                                                        DataTextField="Subject_Name" HeaderText="Subject" DataNavigateUrlFormatString="~/RITeSchool/Teacher/SubjectmarksList.aspx?SubjectId={0}&amp;StandardDivisionId={1}&amp;TestID={2}&amp;"
                                                        Text="Subject Name">
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:HyperLinkField>
                                                    <asp:HyperLinkField DataNavigateUrlFields="Subject_Id,Standard_Division_Id,SchoolWise_Test_Id"
                                                        HeaderText="Edit Marks" DataNavigateUrlFormatString="~/RITeSchool/Teacher/StudentMarksAssignment.aspx?SubjectId={0}&amp;StandardDivisionId={1}&amp;TestId={2}"
                                                        Text="Edit Marks">
                                                        <ItemStyle HorizontalAlign="Center" Width="170px" />
                                                    </asp:HyperLinkField>
                                                </Columns>
                                                <RowStyle CssClass="ClsGridRow" />
                                                <HeaderStyle CssClass="ClsGridHeader" Width="150px" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" Width="150px" />
                                                <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" Width="100%" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trStudentGrid" visible="false">
                                    <td align="left">
                                        <table align="center">
                                            <tr runat="server" id="trTotalRec" align="center">
                                                <td>
                                                    <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                    <span class="LblNormal">To</span>
                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                    <span class="LblNormal">Out Of</span>
                                                    <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                    <span class="LblNormal">Records</span>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="GridViewScrollContainer" style="overflow: auto; width: 100%">
                                            <asp:GridView CssClass="GridBorder" ID="grdStudents" DataSourceID="GrdODStudent"
                                                runat="server" AllowPaging="True" PageSize="20" AutoGenerateColumns="False" OnSorting="grdStudents_Sorting"
                                                AllowSorting="True" OnRowCreated="grdStudents_RowCreated" OnRowDataBound="grdStudents_RowDatabound"
                                                Width="100%" CellPadding="0" CellSpacing="1" ForeColor="#333333" DataKeyNames="Student_Id,Status"
                                                GridLines="None" OnPageIndexChanging="grdStudents_PageIndexChanging" EmptyDataText="No Records Found."
                                                OnDataBinding="grdStudents_DataBinding" OnDataBound="grdStudents_DataBound">
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                </PagerStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="Roll_No" HeaderText="Roll No." SortExpression="Roll_No">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Name" HeaderText="Student Name" SortExpression="Name">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:HyperLinkField DataNavigateUrlFields="Student_Id" HeaderText="Generate" DataNavigateUrlFormatString="~/RITeSchool/Teacher/PrePrimaryProgressSheetEntry.aspx?StudentId={0}&amp;Mode=Edit"
                                                        Text="Generate">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:HyperLinkField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <RowStyle CssClass="ClsGridRow" />
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                <PagerTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                               <span class="LblNrmlB">Select a page:</span>
                                                                <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                    OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </PagerTemplate>
                                                <PagerSettings PageButtonCount="1" />
                                            </asp:GridView>
                                            <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdODStudent"
                                                runat="server" SelectMethod="getPrePrimaryProgressSheetStudentList" SortParameterName="sortExpression"
                                                SelectCountMethod="CountPrePrimaryProgressSheetStudentList" EnableCaching="false"
                                                OnSelected="GrdODStudent_Selected">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                    <asp:ControlParameter ControlID="hidStandardDivisionId" PropertyName="Value" Name="aiStandardDivisionId" />
                                                    <asp:SessionParameter Name="aiAcademicYrId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                        Type="string" />
                                                    <asp:ControlParameter ControlID="cmbTests" PropertyName="SelectedValue" Name="aiTestId" />
                                                    <asp:ControlParameter ControlID="hidIsMonthConfig" PropertyName="Value" Name="abIsMonthConfig"
                                                        Type="Boolean" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSendMessage" runat="server" visible="false">									
									
                                    <td align="center" colspan="5" class="ClsPaddingGen LblNrmlB ">
									<%if (!Settings.IsMiniSite) %>
													<%{ %>
                                        <asp:CheckBox ID="chkSendMessage" runat="server" Text="Send Message and Mobile Notification" />
										<%} %>
                                    </td>									
                                </tr>
                            </table>
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidStandardDivisionId" runat="server" Value="0"></asp:HiddenField>
                            <asp:HiddenField ID="hidPublish" runat="server" Value="0"></asp:HiddenField>
                            <asp:HiddenField ID="hidQery" runat="server" />
                            <asp:HiddenField ID="hidAlert" runat="server" />
                            <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
                            <asp:HiddenField ID="hidIsMonthConfig" runat="server" Value="False" />
                            <asp:HiddenField ID="hidUserID" runat="server" Value="False" />
                            <asp:HiddenField ID="hidConfirmSms" runat="server" />
                            <asp:HiddenField ID="hidExamDependencyMsg" runat="server" />
                            <asp:HiddenField ID="hidDependentExamNames" runat="server" />
                            <asp:HiddenField ID="hidQueryString" runat="server" />
                            <asp:HiddenField ID="hidOldExamId" runat="server" Value="0" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnViewProgress" runat="server" Text="View Progress Report" Visible="false"
                    Enabled="false" CssClass="ClsBtn" UseSubmitBehavior="false" Width="153px" />
                <asp:Button ID="btnGenerateToppers" runat="server" Text="Generate Toppers" 
                    Visible="true" Enabled="false"
                    CssClass="ClsBtn" UseSubmitBehavior="false" Width="125px" 
                    onclick="btnGenerateToppers_Click"/>
                <asp:Button ID="btnPublish" runat="server" Text="Publish" Visible="true" Enabled="false"
                    CssClass="ClsBtn" OnClick="btnPublish_Click" UseSubmitBehavior="false" />                
                <asp:Button ID="btnUnPublish" runat="server" Text="Unpublish" Visible="true" CssClass="ClsBtn"
                    UseSubmitBehavior="false" Enabled="False" />
                <asp:HyperLink Height="15px" ID="HyperLinkProgressRem" CssClass="ClsHilightTextB ToprLinkHlilight "
                    Style="padding-right: 3px;" Visible="False" runat="server" Text="Progress Remarks" NavigateUrl="#"></asp:HyperLink>
                <asp:HyperLink Height="15px" ID="hyplnkTransferStudentMarks" CssClass="ClsHilightTextB ToprLinkHlilight "
                    Style="padding-right: 3px;" runat="server" Text = "Transfer Optional Subject Marks"
                    NavigateUrl="~/RITeSchool/Admin/TransferStudentSubjectsMarksUI.aspx" Visible="false"></asp:HyperLink>
                <asp:HyperLink Height="15px" ID="HyperLinkCaptureHeightWeight" CssClass="ClsHilightTextB ToprLinkHlilight "
                    Style="padding-right: 3px;" Visible="False" runat="server" Text="Termwise Height-Weight"
                    NavigateUrl="#"></asp:HyperLink>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdSubjects.ClientID %>";
        _clientbtnPublish = "<%=this.btnPublish.ClientID %>";
        _clienthidAlert = "<%=this.hidAlert.ClientID %>";
        _clienthidConfirmSms = "<%=this.hidConfirmSms.ClientID %>"
        _clienthidExamDependencyMsg = "<%=this.hidExamDependencyMsg.ClientID %>"



        function saveChk(msg) {
            if (ChkIfAtleastOneCheckedInEachRow(document, _clientGridId)) {
                return true;
            }
            else {
                alert(msg);
                return false;
            }

        }

        function SubmitMarksToClassTeacher(StandardDivisionId, SubjectId, TestID, AcademicYearId, SchoolId) {
            var xmlHttpObj = CreateHTTPReqObj();
            if (xmlHttpObj) {

                var url = "../Ajax.ashx?SchoolId=" + SchoolId + "&StandardDivisionId=" + StandardDivisionId
                + "&SubjectId=" + SubjectId + "&AcademicYearId=" + AcademicYearId
                + "&TestId=" + TestID + "&task=SubmitMarksToClassTeacher";

                xmlHttpObj.open("GET", url, true);
                xmlHttpObj.onreadystatechange = function () {
                    if (xmlHttpObj.readyState == 4) {
                        if (xmlHttpObj.status == 200) {
                            var optionText = xmlHttpObj.responseText;
                            alert(optionText);
                            window.location.reload(true);

                        }
                    }
                }
                xmlHttpObj.send(null);

            }
            else {
                alert('Sad!!');
            }
        }


        function ConfirmAction(hidExamDependencyMsg, hidDependentExamNames, AllowPartialSubmit) {
            var sExamDependencyMsg = document.getElementById(hidExamDependencyMsg).value
            var sDependentExamNames = document.getElementById(hidDependentExamNames).value
            var sMessage = "";
            if (sExamDependencyMsg != "") {
                alert(sExamDependencyMsg)
                return false;
            }
            var bAction = true;
            var sAlert = document.getElementById(_clienthidAlert).value

            if (sAlert.length > 0) {
                if (window.confirm(sAlert + "\nAre you sure you want to continue?")) {
                    bAction = true;
                }
                else {
                    bAction = false;
                }
            }

            var bResult = false;
            if (bAction) {
                if (sDependentExamNames != "") {
                    if (AllowPartialSubmit == "Y")
                        sMessage = sDependentExamNames + " exams will be published. \nIf marks are partially submitted, result will be published with partial mark submission. \nOnce you publish the result it will be visible to parents/students. \nAre you sure you want to continue? ";
                    else
                        sMessage = sDependentExamNames + " exams will be published. \nOnce you publish the result it will be visible to parents/students. \nAre you sure you want to continue? ";
                }
                else {
                    if (AllowPartialSubmit == "Y")
                        sMessage = "\nIf marks are partially submitted, result will be published with partial mark submission. \nOnce you publish the result it will be visible to parents/students. \nAre you sure you want to continue?"
                    else
                        sMessage = "Once you publish the result it will be visible to parents/students. \nAre you sure you want to continue?"
                }
                if (window.confirm(sMessage)) {
                    bResult = true;
                    document.getElementById(_clientbtnPublish).disabled = true;
                }
                else {
                    bResult = false;
                }
            }
            return bResult;
        }

        function ShowToppers(sQryStr) {
            _sClienthlnkToppers = "<%=this.hlnkToppers.ClientID %>";
            if ((document.getElementById(_sClienthlnkToppers) == null) || (document.getElementById(_sClienthlnkToppers) == "") || (document.getElementById(_sClienthlnkToppers).disabled))
                return false;
            window.open(sQryStr, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=840,height=600');
            return false;
        }

        function ShowProgressSheet() {
            _sClienthidQery = "<%=this.hidQery.ClientID %>";
            var validationResult = true;
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("");
            }
            if (validationResult == false) {
                return false;
            }
            window.open("../Student/StudentProgressSheetPrint.aspx?" + document.getElementById(_sClienthidQery).value, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=' + screen.width + ' ,height=600');
        }

        function OpenStudentRemarks() {
            _sClienthidQueryString = "<%=this.hidQueryString.ClientID %>";
            var sEncryptedString = document.getElementById(_sClienthidQueryString).value;
            window.open('StudentwiseRemarkMasterUI.aspx?' + sEncryptedString, '_self')
            return false;
        }

        function OpenTermwiseHeightWeight() {
            _sClienthidQueryString = "<%=this.hidQueryString.ClientID %>";
            var sEncryptedString = document.getElementById(_sClienthidQueryString).value;
            window.open('StudentwiseHeightWeightCaptureUI.aspx?' + sEncryptedString, '_self')
            return false;
        }

    </script>
</asp:Content>
