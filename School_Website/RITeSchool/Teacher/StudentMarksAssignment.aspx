<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="StudentMarksAssignment.aspx.cs" Inherits="StudentMarksAssignment" ViewStateMode="Enabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .GridWidth1
        {
            width: 1200px;
        }
        
        .RemarkStyle
        {
            padding-left: 10px;
            float: left;
        }
    </style>
    <div style="padding-left: 10px;">
        <a id="top"></a>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td align="center">
                    <table width="100%" class="GridWidth1">
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlControls" runat="server" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" valign="bottom" class="GridWidth">
                                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ViewStateMode="Enabled"
                                                    CssClass="ClsLabel" />
                                            </td>
                                            <td align="right" valign="top">
                                                <span style="width: 100%" class="ClsMdtStar">*</span>
                                                <asp:Label ID="lblMandatoryFields" runat="server" Style="width: 100%" class="ClsMdtStar"
                                                    Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="GridWidth" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" ViewStateMode="Enabled"></asp:Label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center" valign="top">
                                            <td align="center" class="GridWidth" colspan="2">
                                                <table cellpadding="0" cellspacing="1" width="90%">
                                                    <tr>
                                                        <td colspan="4" style="padding-bottom: 10px">
                                                            <table cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td class="ClsPaddingR">
                                                                        <asp:Label ID="lblClass" runat="server" class="ClsLblLgnd" Text="<%$ Resources:LocalizedResources, Class %>"></asp:Label>
                                                                        <span class="colonPadding">:</span>&nbsp;
                                                                    </td>
                                                                    <td class="ClsHilightBGB">
                                                                        <asp:Label ID="lblDataStdDiv" runat="server" EnableViewState="False"></asp:Label>
                                                                    </td>
                                                                    <td class="ClsPaddingR">
                                                                    </td>
                                                                    <td class="ClsPaddingR">
                                                                        <asp:Label ID="lblExam" runat="server" class="ClsLblLgnd" Text="<%$ Resources:LocalizedResources, Exam%>"></asp:Label>
                                                                        <span class="colonPadding">:</span>
                                                                    </td>
                                                                    <td class="ClsHilightBGB">
                                                                        <asp:Label ID="LblDataExam" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, TestNo1%>"> </asp:Label>
                                                                        <span class="colonPadding">:</span>
                                                                    </td>
                                                                    <td class="ClsPaddingR">
                                                                    </td>
                                                                    <td class="ClsPaddingR">
                                                                        <asp:Label ID="lblSubjectName" runat="server" class="ClsLblLgnd" Text="<%$ Resources:LocalizedResources, SubjectName%>"></asp:Label>
                                                                        <span class="colonPadding">:</span>
                                                                    </td>
                                                                    <td class="ClsHilightBGB">
                                                                        <asp:Label ID="lblDataSubjectName" runat="server" EnableViewState="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <asp:Panel runat="server" ID="pnlSubmitStatus" ViewStateMode="Enabled" Visible="false">
                                                        <tr>
                                                            <td colspan="4" align="center" class="ClsHilightBGB">
                                                                <asp:Label ID="lblSubmitMessage" runat="server" Text="<%$ Resources:LocalizedResources, StudentMarksAreAlreadySubmitted%>"
                                                                    EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </asp:Panel>
                                                    <tr>
                                                        <td class="ClsBorderlight" style="width: 10%">
                                                            <asp:Label ID="lblExamDate" runat="server" ViewStateMode="Enabled" class="ClsLabel"
                                                                Text="<%$ Resources:LocalizedResources, ExamDate%>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td class="ClsBorderlight" colspan="3">
                                                            <asp:TextBox ID="calTestDate" CssClass="SmlCombo" runat="server" ViewStateMode="Enabled"
                                                                AutoPostBack="True" ReadOnly="true"></asp:TextBox>
                                                            <rjs:PopCalendar ID="cTestDate" runat="server" ViewStateMode="Enabled" Control="calTestDate"
                                                                Format="dd MMM yyyy" Culture="en" ShowWeekend="True" ShowMessageBox="false" ShowErrorMessage="false"
                                                                InvalidDateMessage="<%$ Resources:LocalizedResources, ExamDateShouldBeValid%>"
                                                                MessageAlignment="RightCalendarControl" RequiredDate="true" RequiredDateMessage="<%$ Resources:LocalizedResources, ExamDateShouldNotBeBlank%>"
                                                                AutoPostBack="True" OnSelectionChanged="calTestDate_TextChanged" />
                                                            <span class="ClsMdtStar" id="spExamCalendar" runat="server" viewstatemode="Enabled">
                                                                *</span>
                                                            <asp:CustomValidator ID="cstExamDate" runat="server" ViewStateMode="Enabled" ErrorMessage=""
                                                                ClientValidationFunction="CheckExamDate" CssClass="ClsMdtStar" Display="None"
                                                                ControlToValidate="calTestDate" ValidateEmptyText="True"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="custTestDate" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateAcademicYear"
                                                                ControlToValidate="calTestDate" CssClass="ClsMdtStar" Display="None" EnableClientScript="true"
                                                                ErrorMessage="" Visible="true"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" viewstatemode="Enabled" id="trMarks">
                                                        <td style="width: 10%" class="ClsBorderlight">
                                                            <asp:Label ID="lblTotalMarks" runat="server" CssClass="ClsHilightText" Text="<%$ Resources:LocalizedResources, TotalMarks%>"
                                                                EnableViewState="false"></asp:Label>
                                                            <span class="colonPadding">:</span>
                                                        </td>
                                                        <td style="width: 10%" class="ClsHilightTextB ClsBorderlight HilightBGGray">
                                                            <asp:Label ID="lblDataTotalMarks" runat="server" CssClass="ClsHilightText" EnableViewState="false"></asp:Label>
                                                        </td>
                                                        <td style="width: 10%" class="ClsBorderlight">
                                                            <asp:Label ID="lblPassingMarks" runat="server" CssClass="ClsHilightText" Text="<%$ Resources:LocalizedResources, PassingMarks%>"
                                                                EnableViewState="false"></asp:Label>
                                                            <span class="colonPadding">:</span>
                                                        </td>
                                                        <td style="width: 10%" class="ClsHilightTextB ClsBorderlight HilightBGGray">
                                                            <asp:Label ID="lblDataPassingMarks" runat="server" CssClass="ClsHilightText" EnableViewState="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trGrade" viewstatemode="Enabled">
                                                        <td style="width: 10%" class="ClsBorderlight">
                                                            <asp:Label ID="lbPassingGrade" runat="server" class="ClsHilightText" Text="<%$ Resources:LocalizedResources, PassingGrade%>"></asp:Label>
                                                            <span class="colonPadding">:</span>
                                                        </td>
                                                        <td style="width: 10%" class="ClsHilightTextB ClsBorderlight HilightBGGray" colspan="3">
                                                            <asp:Label ID="lblPassingGrade" runat="server" CssClass="ClsHilightText" EnableViewState="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trNote" viewstatemode="Enabled">
                                                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                                            <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Note%>"
                                                                CssClass="LblNrmlB"></asp:Label>
                                                            <span class="colonPadding">:</span>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;" colspan="3">
                                                            <asp:Label ID="lblNote" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="<%$ Resources:LocalizedResources, MarksAssignmentCanBeDoneInDecimalNumbers%>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center" valign="top">
                                            <td align="center" class="GridWidth" colspan="2">
                                                <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" ID="upnlMarksgrd" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div id="Div1" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td valign="top" align="right" id="tdStudGrid" runat="server" visible="false">                                                                        
                                                                        <asp:Panel ID="pnl1" runat="server" ScrollBars="Horizontal" style="float:inherit;">                                                                        
                                                                            <asp:GridView CssClass="GridBorder" ID="grdvwStudents" Width="90%" 
                                                                                runat="server" ViewStateMode="Enabled"
                                                                                AutoGenerateColumns="False" PageSize="200" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                                                GridLines="None" DataKeyNames='Roll_No' onrowdatabound="grdvwStudents_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblRollNoHeader" runat="server" ViewStateMode="Enabled" style="white-space: nowrap;padding-right:15px;" Text="Roll No"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblRollNo" runat="server" style="text-align:center" ViewStateMode="Enabled"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" CssClass="ClsPaddingL" />
                                                                                        <HeaderStyle HorizontalAlign="Left" CssClass="ClsPaddingL" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, StudentName%>" SortExpression="Name"
                                                                                        DataField="Name">
                                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClsPaddingL" Wrap="false" />
                                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClsPaddingL" />
                                                                                    </asp:BoundField>
                                                                                </Columns>
                                                                                <RowStyle CssClass="ClsGridRow" />
                                                                                <HeaderStyle CssClass="ClsGridHeader" HorizontalAlign="Center" />
                                                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                                <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                                            </asp:GridView>
                                                                            </asp:Panel>                                                                       
                                                                    </td>
                                                                    <td align="center">
                                                                        <div id="divMarks" runat="server" style="overflow: auto;width:1000px;inherit;">
                                                                            <asp:GridView CssClass="GridBorder" ID="grdStudentMarks" Width="90%" runat="server"
                                                                                ViewStateMode="Enabled" AutoGenerateColumns="False" PageSize="200" CellPadding="0"
                                                                                CellSpacing="1" ForeColor="#333333" GridLines="None" OnRowDataBound="grdStudentMarks_RowDataBound"
                                                                                DataKeyNames='Student_Id,Joining_Date,Name, SalutationId,FName,MName,LName, Roll_No'
                                                                                OnRowCommand="grdStudentMarks_RowCommand">
                                                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                                </PagerStyle>
                                                                                <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>"
                                                                                    PreviousPageText="<%$ Resources:LocalizedResources, Previous%>" FirstPageText="<%$ Resources:LocalizedResources, First%>"
                                                                                    Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblRollNoHeader" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, RollNo%>"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblRollNo" runat="server" ViewStateMode="Enabled"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="ClsPaddingL" />
                                                                                    <HeaderStyle HorizontalAlign="Left" CssClass="ClsPaddingL" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, StudentName%>" SortExpression="Name"
                                                                                    DataField="Name">
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClsPaddingL" Wrap="false" />
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClsPaddingL" />
                                                                                </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, ExamStatus %>" Visible="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:DropDownList ID="ddlExamStatus" runat="server" ViewStateMode="Enabled" CssClass="MidCombo" />
                                                                                        </ItemTemplate>
                                                                                        <ControlStyle Width="100px" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="False">
                                                                                        <HeaderTemplate>
                                                                                            <asp:DropDownList ID="ddlHeaderGrade" runat="server" ViewStateMode="Enabled" CssClass="SmlCombo" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:DropDownList ID="ddlGrade" runat="server" ViewStateMode="Enabled" />
                                                                                        </ItemTemplate>
                                                                                        <ControlStyle CssClass="SmlCombo" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="false" HeaderText="Remark">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtRemark" runat="server" ViewStateMode="Enabled" TextMode="MultiLine"
                                                                                                CssClass="LrgTxtBox" Width="250px" Style="margin-left: 5px;"></asp:TextBox>
                                                                                            <asp:Button ID="btnTemplate" runat="server" ViewStateMode="Enabled" Text="..." CssClass="ClsBtn"
                                                                                                CausesValidation="false" Width="25px" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                CommandName="OPEN_TEMPLATE_POPUP" />
                                                                                            <asp:Label ID="lblRemarkLength" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel"
                                                                                                Style="background-color: transparent;"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ControlStyle CssClass="SmlCombo" />
                                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Bottom" />
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="325px" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <RowStyle CssClass="ClsGridRow" />
                                                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                                <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cTestDate" EventName="SelectionChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnPopupSave" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="grdStudentMarks" EventName="RowCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="100%" Visible="false">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblPreCondition" Style="text-align: left" runat="server" ForeColor="blue"
                                        Width="100%" Visible="False" CssClass="ClsConfigText" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:HyperLink ID="lnkRollnumbers" CssClass="ClsConfigLink" Text="<%$ Resources:LocalizedResources, Rollnumbers%>"
                                        runat="server" ViewStateMode="Enabled" NavigateUrl="~/RITeSchool/Admin/AllStudentsUI.aspx"
                                        Visible="false"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:HyperLink ID="lnkExamDates" CssClass="ClsConfigLink" Text="<%$ Resources:LocalizedResources, ExamDates%>"
                                        runat="server" ViewStateMode="Enabled" NavigateUrl="~/RITeSchool/Student/StandardwiseExamScheduleList.aspx"
                                        Visible="false"></asp:HyperLink>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr align="center" valign="top">
                            <td align="center">
                                <asp:UpdatePanel ID="UpdtPnl1" runat="server">
                                    <ContentTemplate>
                                        <table align="left" class="GridWidth" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnCancel" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn"
                                                        Text="<%$ Resources:LocalizedResources, Back%>" OnClick="btnCancel_Click" UseSubmitBehavior="false"
                                                        TabIndex="0" CausesValidation="False" />
                                                    <asp:Button ID="btnSave" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn"
                                                        Text="<%$ Resources:LocalizedResources, Save%>" OnClick="btnSave_Click" disable-page="true" />
                                                    <asp:CustomValidator ID="cstvalRemark" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateRemarkLength"
                                                        CssClass="ClsMdtStar" Display="None"></asp:CustomValidator>
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
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlTimer" runat="server">
                        <ContentTemplate>
                            <asp:Timer ID="timer" runat="server" ViewStateMode="Enabled" Interval="60000" Enabled="false"
                                OnTick="timer_Tick">
                            </asp:Timer>
                            <asp:HiddenField ID="hidTimerVisibleState" runat="server" ViewStateMode="Enabled" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidStandardDivisionId" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidIsTestPublished" runat="server" ViewStateMode="Enabled" Value="N" />
        <asp:HiddenField ID="hidSubjectId" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidStandardId" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidTestId" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidSchoolSubjectTestId" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidIsExamStatusApplicable" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidTeacherId" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidAllowDecimal" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidMarksOrGrades" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidIsReadOnly" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidSelectedStandardDivisionId" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidCanOverride" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidAllTestTypes" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidAcademicStartDate" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidAcademicEndDate" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="HidGradeRange" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidTestOutOfMarksAvailable" runat="server" ViewStateMode="Enabled"
            Value="N" />
        <asp:HiddenField ID="hidTestTypeOutOfMarksAvailable" runat="server" ViewStateMode="Enabled"
            Value="N" />
        <asp:HiddenField ID="hidTestOutOfMarks" runat="server" ViewStateMode="Enabled" Value="0" />
        <asp:HiddenField ID="hidIsCoCurricullarSubject" runat="server" ViewStateMode="Enabled"
            Value="0" />
        <asp:HiddenField ID="hidShowTotalAsPerOutOfMarks" runat="server" ViewStateMode="Enabled"
            Value="N" />
        <asp:HiddenField ID="hidCultureInfo" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidRollnumbers" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidPleaseSelectGradesForFollowingStudents" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidMarksForFollowingStudentsShouldNotBeBlank" runat="server"
            ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidhidPleaseFixFollowingErrors" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidExamIsAlreadyPublished" runat="server" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidExamDateShouldBeWithinCurrentAcademicYear" runat="server"
            ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidMarksForFollowingStudentsShouldBeLessThan" runat="server"
            ViewStateMode="Enabled" />
        <asp:HiddenField ID="hidShowGrade" runat="server" ViewStateMode="Enabled" Value="0" />
        <asp:HiddenField ID="hidConvertDecimalMarks" runat="server" ViewStateMode="Enabled"
            Value="0" />
    </div>
    <div id="divTemplates" runat="server" viewstatemode="Enabled" style="visibility: hidden;
        display: none; position: absolute; margin: 0px; padding: 0px; width: 760px; height: 430px;
        border-width: 1px; left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen;
        margin: -110px 0px 0px 00px; background-color: white;">
        <div class="StudentWiseRemarkMasterPop">
            <div style="font-size: 12px; width: 350px; letter-spacing: 1px; padding-left: 8px;
                font-weight: bold; color: Green; float: left; height: 10px" align="left">
                Select Appropriate Template
            </div>
            <span style="cursor: hand; float: right;" onclick="javascript:HidePopup();">
                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif" border="0" />
            </span>
        </div>
        <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
            color: #333; overflow: auto; height: 380px; width: 750px; margin-left: 1px" id="Div5">
            <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                        vertical-align: top">
                        <tr>
                            <td>
                                <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                                    <tr>
                                        <td align="center">
                                            <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Student Name :</span>
                                                                </td>
                                                                <td class="ClsHilightBGB">
                                                                    <asp:Label CssClass="ClsLabel" runat="server" ViewStateMode="Enabled" ID="lblStudName"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Remark Category:</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbRemarksOnDiv" runat="server" ViewStateMode="Enabled" AutoPostBack="true"
                                                                        CssClass="LrgCombo" OnSelectedIndexChanged="cmbRemarksOnDiv_SelectedIndexChanged"
                                                                        TabIndex="0">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Grades:</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbGradesOnDiv" runat="server" ViewStateMode="Enabled" AutoPostBack="true"
                                                                        CssClass="LrgCombo" OnSelectedIndexChanged="cmbGradesOnDiv_SelectedIndexChanged"
                                                                        TabIndex="0">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:ListView ID="lstvwTemplates" runat="server" ViewStateMode="Enabled" DataKeyNames="TemplateId"
                                                                        OnItemDataBound="lstvwTemplates_ItemDataBound" OnSorting="lstvwTemplates_Sorting">
                                                                        <LayoutTemplate>
                                                                            <table cellpadding="0" cellspacing="1" align="center" width="100%" id="tblPagerUserDetails"
                                                                                runat="server">
                                                                            </table>
                                                                            <table align="center" width="710px" runat="server" id="tblStaffInfo" style="color: #333333"
                                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                <tr id="trHeader" runat="server" viewstatemode="Enabled" class="ClsGridHeader">
                                                                                    <th align="center" style="width: 30px">
                                                                                    </th>
                                                                                    <th align="left" style="width: 600px; padding-left: 5px;">
                                                                                        <asp:LinkButton ID="lnkRemarkTemplate" runat="server" ViewStateMode="Enabled" CommandName="Sort"
                                                                                            CommandArgument="Template" CausesValidation="false" ForeColor="Black"> Remark Template </asp:LinkButton>
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                                    <td colspan="7">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </LayoutTemplate>
                                                                        <ItemTemplate>
                                                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                                                <td align="center">
                                                                                    <asp:CheckBox ID="chkTemplate" runat="Server" ViewStateMode="Enabled" />
                                                                                </td>
                                                                                <td align="left" style="padding-left: 5px">
                                                                                    <asp:Label ID="lblTemplate" ViewStateMode="Enabled" runat="server" Text='<%# Eval("Template") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                                <td align="center">
                                                                                    <asp:CheckBox ID="chkTemplate" runat="Server" ViewStateMode="Enabled" />
                                                                                </td>
                                                                                <td align="left" style="padding-left: 5px;">
                                                                                    <asp:Label ID="lblTemplate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Template") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                        <EmptyDataTemplate>
                                                                            <table width="700px" align="center">
                                                                                <tr>
                                                                                    <td class="LblNoRecord" style="text-align: center">
                                                                                        <span>No record found.</span>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </EmptyDataTemplate>
                                                                    </asp:ListView>
                                                                    <asp:HiddenField ID="hidFname" runat="server" ViewStateMode="Enabled" />
                                                                    <asp:HiddenField ID="hidMname" runat="server" ViewStateMode="Enabled" />
                                                                    <asp:HiddenField ID="hidLname" runat="server" ViewStateMode="Enabled" />
                                                                    <asp:HiddenField ID="hidSalutationId" runat="server" ViewStateMode="Enabled" />
                                                                    <asp:HiddenField ID="hidSelectedStudentId" runat="server" ViewStateMode="Enabled"
                                                                        Value="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="bottom">
                                            <asp:HiddenField ID="hidSortExpression" runat="server" ViewStateMode="Enabled" />
                                            <asp:HiddenField ID="hidSortDirection" runat="server" ViewStateMode="Enabled" />
                                            <asp:HiddenField ID="hidRemarkLength" runat="server" ViewStateMode="Enabled" Value="0" />
                                            <asp:HiddenField ID="hidShowTotalGrade" runat="server" ViewStateMode="Enabled" Value="Y" />
                                            <asp:HiddenField ID="hidSelectedRowIndex" runat="server" ViewStateMode="Enabled"
                                                Value="0" />
                                            <asp:HiddenField ID="hidMarksGradesConfigurationDetailsId" runat="server" ViewStateMode="Enabled" />
                                            <asp:Button ID="btnPopupSave" runat="server" ViewStateMode="Enabled" Text="Select"
                                                CssClass="ClsBtn" OnClick="btnPopupSave_Click" CausesValidation="false" OnClientClick="if(!CheckAtleastOneSelected()) return false;" />
                                            <asp:Button ID="btnClosePopUp" runat="server" ViewStateMode="Enabled" Text="Close"
                                                CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HidePopup();return false;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnPopupSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="grdStudentMarks" EventName="RowCommand" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="divTest" style="display: none;">
    </div>
    <script language="javascript" type="text/javascript">

        _clientGridId = "<%=this.grdStudentMarks.ClientID %>";
        _clientHidCanOverride = "<%=this.hidCanOverride.ClientID %>";
        _clienthidIsTestPublished = "<%=this.hidIsTestPublished.ClientID %>";
        _clientBtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>";
        _clienthidAllTestTypes = "<%=this.hidAllTestTypes.ClientID %>";
        _clienttxtTestDate = "<%=this.calTestDate.ClientID %>";
        _clienthidAcademicStartDate = "<%=this.hidAcademicStartDate.ClientID %>";
        _clienthidAcademicEndDate = "<%=this.hidAcademicEndDate.ClientID %>";
        _clientcustTestDate = "<%=this.custTestDate.ClientID %>";
        _clientvalSumErrorMsg = "<%=this.valSumErrorMsg.ClientID %>";
        _clientcustExamDate = "<%=this.cstExamDate.ClientID %>";
        _clientHidGradeRange = "<%=this.HidGradeRange.ClientID %>";
        _clienthidTestOutOfMarksAvailable = "<%=this.hidTestOutOfMarksAvailable.ClientID %>";
        _clienthidTestTypeOutOfMarksAvailable = "<%=this.hidTestTypeOutOfMarksAvailable.ClientID %>";
        _clienthidTestOutOfMarks = "<%=this.hidTestOutOfMarks.ClientID %>";
        _clienthidShowTotalAsPerOutOfMarks = "<%=this.hidShowTotalAsPerOutOfMarks.ClientID %>";
        _clienthidIsExamStatusApplicable = '<%= this.hidIsExamStatusApplicable.ClientID %>';
        _clienthidAllowDecimal = "<%=this.hidAllowDecimal.ClientID %>";
        _clienthidRemarkLength = "<%=this.hidRemarkLength.ClientID %>";
        _clienthidShowTotalGrade = "<%=this.hidShowTotalGrade.ClientID %>"

        var InvalidDate = "N";
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);
        function EndReqHandler(sender, args) {
            DisableAllUncheckedRows();
        }

        function DisableRelatedControl(obj, RowNumber, controlname) {
            var sname;
            if (RowNumber < 10)
                sname = _clientGridId + "_ctl0" + RowNumber + "_" + controlname;
            else
                sname = _clientGridId + "_ctl" + RowNumber + "_" + controlname;

            if (obj.value != "N") {
                if (controlname != "ddlGrade") {
                    document.getElementById(sname).value = "";
                }
                document.getElementById(sname).disabled = true;
                document.getElementById(sname).value = "0";
            }
            else {
                if (controlname != "ddlGrade") {
                    document.getElementById(sname).value = "";
                }
                document.getElementById(sname).disabled = false;
            }
        }

        function DisableThisRowControls(thisobj, RowName) {
            var childContainer = document.getElementById(RowName);
            for (var i = 0; i < childContainer.childNodes.length; i++) {
                //check if the child node is an element node
                if (childContainer.childNodes[i].tagName != null) {
                    if (childContainer.childNodes[i].tagName.toLowerCase() == "td") {
                        DisableOrEnableControls(thisobj, childContainer.childNodes[i]);
                    }
                }
            }
        }

        function DisableOrEnableControls(thisObj, Container) {
            for (var i = 0; i < Container.childNodes.length; i++) {
                if (Container.childNodes[i].tagName != null) {
                    if (Container.childNodes[i].childNodes.length > 0) {
                        DisableOrEnableControls(thisObj, Container.childNodes[i])
                    } else if (Container.childNodes[i].tagName.toLowerCase() == "input" &&
                    ((Container.childNodes[i].type.toLowerCase() == "checkbox") || (Container.childNodes[i].type.toLowerCase() == "text"))
                    && Container.childNodes[i] != thisObj) {
                        if (!thisObj.checked) {
                            if ((Container.childNodes[i].type.toLowerCase() == "text") && (Container.childNodes[i].className == "TxtAlignRghtB"))
                                Container.childNodes[i].value = "0";
                            else if (Container.childNodes[i].type.toLowerCase() == "text")
                                Container.childNodes[i].value = "";
                            else if (Container.childNodes[i].type.toLowerCase() == "checkbox")
                                Container.childNodes[i].checked = false;
                            else if (Container.childNodes[i].tagName.toLowerCase() == "span" &&
                                    Container.childNodes[i].className == "LblGrade")
                                Container.childNodes[i].innerHTML = "";
                        }
                        if (thisObj.disabled == false || thisObj.disabled) {
                            if (Container.childNodes[i].type.toLowerCase() == "text") {
                                var checkbox = document.getElementById(Container.childNodes[i].id.replace("txtMarks", "IsAbsent"));
                                if (!thisObj.checked || (checkbox != null && checkbox.value != "N" && !Container.childNodes[i].id.match("txtTotalMarks")))
                                    Container.childNodes[i].disabled = true;
                                // It means Exam is PUBLISH
                                else if (thisObj.checked && thisObj.disabled)
                                    Container.childNodes[i].disabled = true;
                                else
                                    Container.childNodes[i].disabled = !thisObj.checked
                            } else if (Container.childNodes[i].type.toLowerCase() == "checkbox")
                                Container.childNodes[i].disabled = !thisObj.checked;
                            else if (Container.childNodes[i].tagName.toLowerCase() == "span" &&
                                Container.childNodes[i].className == "LblGrade")
                                Container.childNodes[i].innerHTML = "";
                        }
                    }
                    if (!thisObj.checked && Container.childNodes[i].tagName.toLowerCase() == "span") {
                        if (Container.childNodes[i].className == "LblGrade" && Container.childNodes[i] != thisObj) {
                            Container.childNodes[i].innerHTML = "";
                        }
                    }

                    if (Container.childNodes[i].tagName.toLowerCase() == "select" && Boolean.parse($get(_clienthidIsExamStatusApplicable).value)) {
                        if (thisObj.checked) {
                            Container.childNodes[i].disabled = false;
                            sIsAbsent = Container.childNodes[i].value;
                        }
                        else {
                            Container.childNodes[i].disabled = true;
                            if (Container.childNodes[i].value != "J")
                                Container.childNodes[i].value = "N";
                        }

                        if (thisObj.disabled)
                            Container.childNodes[i].disabled = true;
                    }
                }
            }
        }

        function DisableAllUncheckedRows() {
            var gridView = document.getElementById(_clientGridId);
            for (var i = 0; i < gridView.rows.length; i++) {
                var iRowNumber = i + 1;
                if (iRowNumber < 10) {
                    var isOptional = _clientGridId + "_ctl0" + iRowNumber + "_chkOptional"
                } else {
                    var isOptional = _clientGridId + "_ctl" + iRowNumber + "_chkOptional"
                }
                var cmbIsOptional = document.getElementById(isOptional);
                if (cmbIsOptional != null && (cmbIsOptional.checked) != null)
                    DisableOrEnableControls(document.getElementById(isOptional), gridView.rows[i])
            }
        }
        function DisableIfAbsent(obj) {
            var sname;
            var txtbox = document.getElementById(obj.id.replace("IsAbsent", "txtMarks"));
            var txtboxlbl = document.getElementById(obj.id.replace("IsAbsent", "txtMarkslbl"));
            if (obj.checked) {
                if (txtbox != null) {
                    txtbox.value = "";
                    txtbox.disabled = true;
                    txtboxlbl.value = "";
                }
            }
        }

        DisableAllUncheckedRows();

        function DisableMarksControl(obj, controlname, Totalcontrolname) {        
            var sname;
            if (controlname.indexOf('grdStudentMarks') == -1)
                controlname = obj.id.replace('IsAbsent', 'txtMarks');
            if (obj.value != "N") {
                document.getElementById(controlname).value = "";
                document.getElementById(controlname).disabled = true;
                document.getElementById(controlname + "lbl").innerHTML = "";
                document.getElementById(controlname + "lblConvertedMarks").innerHTML = "";
            }
            else {
                document.getElementById(controlname).value = "";
                document.getElementById(controlname).disabled = false;
            }
            var ContainerName = controlname.replace('txtMarks', '');
            ContainerName = ContainerName.substring(0, controlname.lastIndexOf('_'));
            SetRowTotalMarks(ContainerName);
        }

        function SetSelectedGradeForAllRows() {
            var n = document.getElementById(_clientGridId).rows.length + 1;
            var selectedheadergrade = document.getElementById(_clientGridId + "_ctl01_ddlHeaderGrade").value;
            var gradecombo;
            for (i = 2; i < n; i++) {
                if (i < 10)
                    gradecombo = _clientGridId + "_ctl0" + i + "_ddlGrade";
                else
                    gradecombo = _clientGridId + "_ctl" + i + "_ddlGrade";

                if (document.getElementById(gradecombo).disabled == false)
                    document.getElementById(gradecombo).value = selectedheadergrade;

            }
        }

        function IsAllMarksAssigned() {
            Page_IsValid = true;
            if (document.getElementById(_clientHidCanOverride).value == "True") {
                var n = document.getElementById(_clientGridId).rows.length + 1;
                var sMessage = "", sGradeMessage = "";
                var arr = document.getElementById(_clienthidAllTestTypes).value.split("||");
                var arrMsg = new Array(arr.length)

                for (i = 2; i < n; i++) {
                    RowNumber = i;

                    if (RowNumber < 10) {
                        gradecombo = _clientGridId + "_ctl0" + RowNumber + "_ddlGrade";
                        isAbsentGrade = _clientGridId + "_ctl0" + RowNumber + "_ddlExamStatus";
                        var isOptional = _clientGridId + "_ctl0" + RowNumber + "_chkOptional"
                    }
                    else {
                        gradecombo = _clientGridId + "_ctl" + RowNumber + "_ddlGrade";
                        isAbsentGrade = _clientGridId + "_ctl" + RowNumber + "_ddlExamStatus";
                        var isOptional = _clientGridId + "_ctl" + RowNumber + "_chkOptional"
                    }

                    if (document.getElementById(isOptional) != null && document.getElementById(isOptional).checked == false) {
                        continue;
                    }
                    var j, rollno;
                    j = RowNumber - 1;

                    isValidGrade = true;
                    if (document.getElementById(isOptional) != null)
                        rollno = document.getElementById(_clientGridId).rows[j].cells[1].innerText;
                    else
                        rollno = document.getElementById(_clientGridId).rows[j].cells[0].innerText;

                    if (document.getElementById(gradecombo) != null) {
                        if (document.getElementById(gradecombo).value == "0" && document.getElementById(isAbsentGrade).value == "N")
                            isValidGrade = false;

                    }
                    else {
                        var iTextTypeCnt;
                        for (iTextTypeCnt = 0; iTextTypeCnt < arr.length; iTextTypeCnt++) {
                            var marks, txtName, ChkName;
                            if (RowNumber < 10) {
                                txtName = _clientGridId + "_ctl0" + RowNumber + "_" + arr[iTextTypeCnt].substring(0, arr[iTextTypeCnt].lastIndexOf(";")) + "txtMarks";
                                ddlExamStatus = _clientGridId + "_ctl0" + RowNumber + "_" + arr[iTextTypeCnt].substring(0, arr[iTextTypeCnt].lastIndexOf(";")) + "IsAbsent";
                            } else {
                                txtName = _clientGridId + "_ctl" + RowNumber + "_" + arr[iTextTypeCnt].substring(0, arr[iTextTypeCnt].lastIndexOf(";")) + "txtMarks";
                                ddlExamStatus = _clientGridId + "_ctl" + RowNumber + "_" + arr[iTextTypeCnt].substring(0, arr[iTextTypeCnt].lastIndexOf(";")) + "IsAbsent";
                            }

                            if (document.getElementById(txtName) != null) {

                                marks = document.getElementById(txtName).value;
                                if (marks == "" && document.getElementById(ddlExamStatus).value == "N") {
                                    if (arrMsg[iTextTypeCnt] != null && arrMsg[iTextTypeCnt] != "undefined")
                                        arrMsg[iTextTypeCnt] = arrMsg[iTextTypeCnt] + " \n\r -" + document.getElementById("<%=hidRollnumbers.ClientID%>").value + " : " + rollno;
                                    else
                                        arrMsg[iTextTypeCnt] = " \n\r -" + document.getElementById("<%=hidRollnumbers.ClientID%>").value + " : " + rollno;
                                }
                            }
                        }
                    }
                    if (!isValidGrade)
                        sGradeMessage = sGradeMessage + " \n\r -" + document.getElementById("<%=hidRollnumbers.ClientID%>").value + " : " + rollno;

                    if (document.getElementById(gradecombo) == null) {
                    }

                }

                if (sGradeMessage != "" && document.getElementById(_clientHidCanOverride).value == "True")
                    sMessage = sMessage + document.getElementById("<%=hidPleaseSelectGradesForFollowingStudents.ClientID%>").value + sGradeMessage + "\n\r";

                if (document.getElementById(gradecombo) == null) {
                    var iTextTypeCnt;
                    for (iTextTypeCnt = 0; iTextTypeCnt < arrMsg.length; iTextTypeCnt++) {
                        var temp = arr[iTextTypeCnt].split("/");
                        arr[iTextTypeCnt] = arr[iTextTypeCnt].replace("/" + temp[temp.length - 1].trim(), "");
                        if (arrMsg[iTextTypeCnt] != null && arrMsg[iTextTypeCnt] != "undefined")
                            sMessage = sMessage + arr[iTextTypeCnt] + " " + document.getElementById("<%=hidMarksForFollowingStudentsShouldNotBeBlank.ClientID%>").value + arrMsg[iTextTypeCnt] + "\n\r";
                    }
                }
                if (sMessage != "") {
                    alert(document.getElementById("<%=hidhidPleaseFixFollowingErrors.ClientID %>").value + " \n\r\n\r" + sMessage);
                    Page_IsValid = false;
                    return false;
                }
                else
                { return true; }
            }
            else {
                return true;
            }

        }

        function ConfirmAction() {
            if (document.getElementById(_clienthidIsTestPublished).value == "Y") {

                if (window.confirm(document.getElementById("<%=hidExamIsAlreadyPublished.ClientID%>").value)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else
                return true;

        }

        var Page_IsValid = true;

        function IsMarksAreGreaterThanTotalMarks() {

            Page_IsValid = true;
            var str;
            str = window.location.href;
            var iIndex = str.lastIndexOf("/#");
            if (iIndex != -1) {
                str = str.substr(0, iIndex)
            }
            str = str + "/#top";

            if (document.all && navigator.appVersion.indexOf('MSIE 7') == 0)
                window.navigate(str);

            if (IsAllMarksAssigned()) {
                var n = document.getElementById(_clientGridId).rows.length + 1;
                var sMessage = "", sGradeMessage = "";
                var arr = document.getElementById(_clienthidAllTestTypes).value.split("||");
                var arrPassingMarks = new Array(arr.length)
                var arrMsg = new Array(arr.length)

                var iTextTypeCnt;
                for (iTextTypeCnt = 0; iTextTypeCnt < arr.length; iTextTypeCnt++) {
                    var temp = arr[iTextTypeCnt].split("/");
                    arrPassingMarks[iTextTypeCnt] = parseFloat(temp[temp.length - 1].trim().substring(0, temp[temp.length - 1].lastIndexOf(";")));
                }

                for (i = 2; i < n; i++) {
                    RowNumber = i;

                    if (RowNumber < 10) {
                        gradecombo = _clientGridId + "_ctl0" + RowNumber + "_ddlGrade";
                        isAbsentGrade = _clientGridId + "_ctl0" + RowNumber + "_ddlExamStatus";
                        var isOptional = _clientGridId + "_ctl0" + RowNumber + "_chkOptional"
                    }
                    else {
                        gradecombo = _clientGridId + "_ctl" + RowNumber + "_ddlGrade";
                        isAbsentGrade = _clientGridId + "_ctl" + RowNumber + "_ddlExamStatus";
                        var isOptional = _clientGridId + "_ctl" + RowNumber + "_chkOptional"
                    }

                    var j, rollno;
                    j = RowNumber - 1;

                    isValidGrade = true;

                    if (document.getElementById(isOptional) != null)
                        rollno = document.getElementById(_clientGridId).rows[j].cells[1].innerText;
                    else
                        rollno = document.getElementById(_clientGridId).rows[j].cells[0].innerText;

                    if (document.getElementById(gradecombo) != null) {
                        if (document.getElementById(gradecombo).value == "0" && document.getElementById(isAbsentGrade).value == "N")
                            isValidGrade = false;

                    }
                    else {
                        var iTextTypeCnt;
                        for (iTextTypeCnt = 0; iTextTypeCnt < arr.length; iTextTypeCnt++) {
                            var marks, txtName, ChkName;
                            if (RowNumber < 10) {
                                txtName = _clientGridId + "_ctl0" + RowNumber + "_" + arr[iTextTypeCnt].substring(0, arr[iTextTypeCnt].lastIndexOf(";")) + "txtMarks";
                                ddlExamStatus = _clientGridId + "_ctl0" + RowNumber + "_" + arr[iTextTypeCnt].substring(0, arr[iTextTypeCnt].lastIndexOf(";")) + "IsAbsent";
                            } else {
                                txtName = _clientGridId + "_ctl" + RowNumber + "_" + arr[iTextTypeCnt].substring(0, arr[iTextTypeCnt].lastIndexOf(";")) + "txtMarks";
                                ddlExamStatus = _clientGridId + "_ctl" + RowNumber + "_" + arr[iTextTypeCnt].substring(0, arr[iTextTypeCnt].lastIndexOf(";")) + "IsAbsent";
                            }

                            if (document.getElementById(txtName) != null) {

                                marks = document.getElementById(txtName).value;
                                if (marks != "" && document.getElementById(ddlExamStatus).value == "N") {
                                    if (parseFloat(RemoveLeadingZeroes(marks)) > arrPassingMarks[iTextTypeCnt]) {
                                        if (arrMsg[iTextTypeCnt] != null && arrMsg[iTextTypeCnt] != "undefined")
                                            arrMsg[iTextTypeCnt] = arrMsg[iTextTypeCnt] + " \n\r -" + document.getElementById("<%=hidRollnumbers.ClientID %>").value + rollno;
                                        else
                                            arrMsg[iTextTypeCnt] = " \n\r -" + document.getElementById("<%=hidRollnumbers.ClientID %>").value + " : " + rollno;
                                    }
                                }
                            }
                        }
                    }
                    if (!isValidGrade)
                        sGradeMessage = sGradeMessage + " \n\r -" + document.getElementById("<%=hidRollnumbers.ClientID %>").value + " : " + rollno;
                }

                if (sGradeMessage != "" && document.getElementById(_clientHidCanOverride).value == "True")
                    sMessage = sMessage + document.getElementById("<%=hidPleaseSelectGradesForFollowingStudents.ClientID %>").value + sGradeMessage + "\n\r";

                if (document.getElementById(gradecombo) == null) {
                    var iTextTypeCnt;
                    for (iTextTypeCnt = 0; iTextTypeCnt < arrMsg.length; iTextTypeCnt++) {
                        var temp = arr[iTextTypeCnt].split("/");
                        arr[iTextTypeCnt] = arr[iTextTypeCnt].replace("/" + temp[temp.length - 1].trim(), "");
                        if (arrMsg[iTextTypeCnt] != null && arrMsg[iTextTypeCnt] != "undefined")
                            sMessage = sMessage + arr[iTextTypeCnt] + " " + document.getElementById("<%=hidMarksForFollowingStudentsShouldBeLessThan.ClientID%>").value.replace("%PassingMarks%", arrPassingMarks[iTextTypeCnt]) + arrMsg[iTextTypeCnt] + "\n\r";
                    }
                }

                if (sMessage != "") {
                    alert(document.getElementById("<%=hidhidPleaseFixFollowingErrors.ClientID%>").value + " \n\r\n\r" + sMessage);
                    Page_IsValid = false;
                    return false;
                }
                else {
                    var validationResult = true;
                    if (typeof (Page_ClientValidate) == 'function') {
                        validationResult = Page_ClientValidate("");
                    }

                    if (ConfirmAction() && validationResult) {

                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            else {
                return false;
            }

        }

        function ValidateAcademicYear(oSrc, args) {
            var bReturn;
            if (InvalidDate == "N") {
                var dtTestDate = document.getElementById(_clienttxtTestDate).value;
                var TestDate = new Date(convertdate(dtTestDate));
                var dtYearStartDate = new Date(document.getElementById(_clienthidAcademicStartDate).value);
                var dtYearEndDate = new Date(document.getElementById(_clienthidAcademicEndDate).value);
                if (dtTestDate.length == 0) {
                    args.IsValid = false;
                    return true;
                }
                else if (TestDate > dtYearEndDate || TestDate < dtYearStartDate) {
                    document.getElementById(_clientcustTestDate).errormessage = document.getElementById("<%=hidExamDateShouldBeWithinCurrentAcademicYear.ClientID%>").value.replace("%dtYearStartDate%", getDateString(dtYearStartDate)).replace("%dtYearEndDate%", getDateString(dtYearEndDate));
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }

        function CheckExamDate(oSrc, args) {
            var dtTestDate = document.getElementById(_clienttxtTestDate).value;
            if (dtTestDate.length == 0) {
                InvalidDate = "Y";
                args.IsValid = false;
                return true;
            }
            else {
                try {
                    var TestDate = new Date(convertdate(dtTestDate));
                } catch (e) {
                    InvalidDate = "Y";
                    args.IsValid = false;
                    return true;
                }
            }
            InvalidDate = "N";
            args.IsValid = true;
            return false;
        }

        function DisableButtons() {
            if (document.getElementById(_clientBtnSave)) {
                document.getElementById(_clientBtnSave).disabled = true;
            }
        }


        function SetRowTotalMarks(ContainerName) {
            var totmarks = 0;
            var TotalControlName = ContainerName + "_" + "txtTotalMarks";
            var arr = document.getElementById(_clienthidAllTestTypes).value.split("||");
            var iTextTypeCnt;
            var IsAllAbsent = true;
            for (iTextTypeCnt = 0; iTextTypeCnt < arr.length; iTextTypeCnt++) {
                var marks, txtName, ChkIsAbsentName, TestType, TestTypeOutOfMarks;
                TestType = (arr[iTextTypeCnt].split(";"))[0];
                TestTypeOutOfMarks = (arr[iTextTypeCnt].split(";"))[1];
                txtName = ContainerName + "_" + TestType + "txtMarks";
                ChkIsAbsentName = ContainerName + "_" + TestType + "IsAbsent";
                if (document.getElementById(txtName) != null) {
                    marks = document.getElementById(txtName).value;
                    if (marks != "" && marks != ".") {
                        if (document.getElementById(_clienthidShowTotalAsPerOutOfMarks).value != "N" && document.getElementById(_clienthidTestTypeOutOfMarksAvailable).value != "N")
                            totmarks = totmarks + parseFloat(Round(RemoveLeadingZeroes(marks) * TestTypeOutOfMarks / parseInt(((arr[iTextTypeCnt].split(";"))[0]).substring(((arr[iTextTypeCnt].split(";"))[0]).lastIndexOf('/') + 1)), (Boolean.parse($("input:hidden[id*=hidAllowDecimal]")[0].value)) ? 1 : 0));
                        else
                            totmarks = totmarks + parseFloat(RemoveLeadingZeroes(marks));
                    }
                    else
                        document.getElementById(txtName).value = "";
                }
                if (document.getElementById(ChkIsAbsentName) != null) {
                    if (!document.getElementById(ChkIsAbsentName).checked)
                        IsAllAbsent = false;
                }
            }
            var arr = document.getElementById(_clienthidAllTestTypes).value.split("||");
            var arrPassingMarks = new Array(arr.length)
            var arrMsg = new Array(arr.length)
            var totalmarks = 0;
            var iTextTypeCnt;
            for (iTextTypeCnt = 0; iTextTypeCnt < arr.length; iTextTypeCnt++) {
                var temp = arr[iTextTypeCnt].split("/");
                if (document.getElementById(_clienthidShowTotalAsPerOutOfMarks).value != "N" && document.getElementById(_clienthidTestTypeOutOfMarksAvailable).value != "N")
                    totalmarks = totalmarks + parseFloat(temp[temp.length - 1].trim().split(";")[1]);
                else
                    totalmarks = totalmarks + parseFloat(temp[temp.length - 1].trim().split(";")[0]);
            }
            if (document.getElementById(_clienthidShowTotalAsPerOutOfMarks).value == "Y" && document.getElementById(_clienthidTestOutOfMarksAvailable).value == "Y") {
                totmarks = Round((totmarks * document.getElementById(_clienthidTestOutOfMarks).value) / totalmarks, (Boolean.parse($("input:hidden[id*=hidAllowDecimal]")[0].value)) ? 1 : 0);

                if (Boolean.parse($("input:hidden[id*=hidAllowDecimal]")[0].value) && $("input:hidden[id*=hidConvertDecimalMarks]")[0].value == "1") {
                    var numVal = parseInt(totmarks)
                    if (parseFloat(totmarks) <= parseFloat(numVal + 0.2))
                        totmarks = numVal;
                    else if ((parseFloat(totmarks) > parseFloat(numVal + 0.2)) && (parseFloat(totmarks) <= parseFloat(numVal + 0.5)))
                        totmarks = parseFloat(numVal + 0.5)
                    else if ((parseFloat(totmarks) > parseFloat(numVal + 0.5)) && (parseFloat(totmarks) <= parseFloat(numVal + 0.7)))
                        totmarks = parseFloat(numVal + 0.5)
                    else
                        totmarks = parseInt(Math.ceil(totmarks)).toFixed(0)
                }
            }

            document.getElementById(TotalControlName).value = (Boolean.parse($("input:hidden[id*=hidAllowDecimal]")[0].value)) ? parseFloat(totmarks).toFixed(("" + totmarks).split(".").length == 1 || ("" + totmarks).split(".")[1] == "0" ? 0 : 1) : parseInt(Math.ceil(totmarks)).toFixed(0);
            SetTotalGrade(ContainerName, IsAllAbsent);
        }

        function SetGrade(TxtBox, SubjMarks) {
            var arrGrades = document.getElementById(_clientHidGradeRange).value.split("#");
            var oTxtBox = document.getElementById(TxtBox);
            if (document.getElementById(_clienthidShowTotalAsPerOutOfMarks).value == 'Y') {
                var arr = document.getElementById(_clienthidAllTestTypes).value.split("||");
                var iIndex = 0
                for (iIndex = 0; iIndex < arr.length; iIndex++) {
                    var TestName = arr[iIndex].split(";")[0];
                    var OutOfMarks = arr[iIndex].split(";")[1];
                    if (TxtBox.indexOf(TestName) != -1) {
                        var TestMarks = TestName.split("/")[TestName.split("/").length - 1]
                        if (OutOfMarks != 0 && TestMarks != OutOfMarks)
                            if (oTxtBox.value != "")
                                document.getElementById(TxtBox + "lblConvertedMarks").innerHTML = "(" + Round((parseFloat(oTxtBox.value) * OutOfMarks) / TestMarks, (Boolean.parse($("input:hidden[id*=hidAllowDecimal]")[0].value)) ? 1 : 0) + "/" + OutOfMarks + ")";
                            else
                                document.getElementById(TxtBox + "lblConvertedMarks").innerHTML = "";
                    }
                }
            }

            if (parseInt($('#' + '<%=this.hidShowGrade.ClientID %>').val()) == 1) {
                var sDisplayCntrl = document.getElementById(TxtBox + "lbl");
                var ObtainedMarks = parseFloat(oTxtBox.value);
                var ObtainedPercentage = ObtainedMarks * 100 / parseInt(SubjMarks);
                var iGradeCnt;
                for (iGradeCnt = 0; iGradeCnt < arrGrades.length; iGradeCnt++) {
                    var arrRanges = arrGrades[iGradeCnt].split(":");
                    var GradeName = arrRanges[0];
                    var GradeMinMarks = parseFloat(arrRanges[1]);
                    var GradeMaxMarks = parseFloat(arrRanges[2]);
                    //Now calculate grades for test type marks
                    if (ObtainedPercentage >= GradeMinMarks && ObtainedPercentage <= GradeMaxMarks) {
                        sDisplayCntrl.innerHTML = GradeName;
                        break;
                    } else {
                        sDisplayCntrl.innerHTML = "";
                    }
                }
            }
        }

        function SetTotalGrade(ContainerName, ChkIsAbsentName) {
            var ObtainedTotmarks = document.getElementById(ContainerName + "_" + "txtTotalMarks").value;
            var ObtainedTotmarkslbl = document.getElementById(ContainerName + "_" + "txtTotalMarkslbl");
            var showGrade = document.getElementById(_clienthidShowTotalGrade).value

            if (ChkIsAbsentName || showGrade == 'N')
                ObtainedTotmarkslbl.innerHTML = "";
            else {
                var arr = document.getElementById(_clienthidAllTestTypes).value.split("||");
                var arrPassingMarks = new Array(arr.length)
                var arrMsg = new Array(arr.length)
                var totmarks = 0;
                var iTextTypeCnt;
                for (iTextTypeCnt = 0; iTextTypeCnt < arr.length; iTextTypeCnt++) {
                    var temp = arr[iTextTypeCnt].split("/");
                    if (document.getElementById(_clienthidShowTotalAsPerOutOfMarks).value != "N" && document.getElementById(_clienthidTestTypeOutOfMarksAvailable).value != "N")
                        totmarks = totmarks + parseInt(temp[temp.length - 1].trim().split(";")[1]);
                    else
                        totmarks = totmarks + parseInt(temp[temp.length - 1].trim().split(";")[0]);
                }
                if (document.getElementById(_clienthidShowTotalAsPerOutOfMarks).value == "Y" && document.getElementById(_clienthidTestOutOfMarksAvailable).value == "Y")
                    totmarks = document.getElementById(_clienthidTestOutOfMarks).value;
                var ObtainedTotParcentage = ObtainedTotmarks * 100 / totmarks;
                var arrGrades = document.getElementById(_clientHidGradeRange).value.split("#");
                var iGradeCnt;

                if (parseInt($('#' + '<%=this.hidShowGrade.ClientID %>').val()) == 1) {
                    for (iGradeCnt = 0; iGradeCnt < arrGrades.length; iGradeCnt++) {
                        var arrRanges = arrGrades[iGradeCnt].split(":");
                        var GradeName = arrRanges[0];
                        var GradeMinMarks = parseFloat(arrRanges[1]);
                        var GradeMaxMarks = parseFloat(arrRanges[2]);
                        //Now calculate grades for total marks.
                        if (ObtainedTotParcentage >= GradeMinMarks && ObtainedTotParcentage <= GradeMaxMarks) {
                            ObtainedTotmarkslbl.innerHTML = GradeName;
                            break;
                        } else {
                            ObtainedTotmarkslbl.innerHTML = "";
                        }
                    }
                }
            }
        }


        function SetTotal() {
            if (document.getElementById(_clienthidShowTotalAsPerOutOfMarks).value == "N") {
                for (iRowIndex = 0; iRowIndex < document.getElementById(_clientGridId).rows.length; iRowIndex++) {
                    var TotalMarks = 0;
                    for (iCellIndex = 0; iCellIndex < document.getElementById(_clientGridId).rows[iRowIndex].cells.length; iCellIndex++) {
                        var input = []
                        input = document.getElementById(_clientGridId).rows[iRowIndex].cells[iCellIndex].getElementsByTagName('input');
                        for (iControl = 0; iControl < input.length; iControl++) {
                            if (input[iControl].id.match("txtMarks") && input[iControl].value.trim() != "") {
                                TotalMarks += parseFloat(input[iControl].value);
                            }
                            if (input[iControl].id.match("txtTotalMarks") && TotalMarks != 0)
                                input[iControl].value = TotalMarks;
                        }
                    }
                }
            }
        }

        function OpenPopup(btnShowPopup) {
            _clientdivTemplates = "<%=this.divTemplates.ClientID %>"
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divTemplates.ClientID %>").style
            var width = 750
            var height = 380
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
            //return false;

            setPopupPosition();

            //            $("#divTest").show();
            //            ContentWindow = $('#divTemplates').kendoWindow({
            //                title: "Templates",
            //                visible: false,
            //                modal: true,
            //                resizable: true,
            //                width: '600px'
            ////                ,
            ////                actions: []
            //            }).data("kendoWindow");
            //            ContentWindow.open();
            //            ContentWindow.center();
        }

        function HidePopup() {
            $get("<%=this.divTemplates.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divTemplates.ClientID %>").style.display = "none"
            return false
        }

        function RefreshRemarkLength(rowIndex) {
            HidePopup();
            UpdateLength(rowIndex);
        }

        _clientlstvwTemplates = "<%=this.lstvwTemplates.ClientID %>"
        function CheckAtleastOneSelected() {
            var iRowCount = 0;
            var bSelected = false;

            var maxRemarksLength = $get("<%=this.hidRemarkLength.ClientID %>").value;
            var remark = ''
            var chk = document.getElementById(_clientlstvwTemplates + "_ctrl" + iRowCount + "_chkTemplate");
            while (chk != null) {

                if (chk.checked) {
                    title = document.getElementById(_clientlstvwTemplates + "_ctrl" + iRowCount + "_lblTemplate").innerHTML
                    remark = remark + "," + title;
                }

                iRowCount++;
                chk = document.getElementById(_clientlstvwTemplates + "_ctrl" + iRowCount + "_chkTemplate")
            }

            if (remark.length > 0)
                remark = remark.substring(1)

            if (remark.length == 0)
                alert("At least one Remark Template should be selected.");
            else if (remark.length > maxRemarksLength)
                alert("Remarks length should not be more than " + maxRemarksLength + ".");
            else
                bSelected = true;

            return bSelected;
        }

        function alertMsgLength(e, txtRemarks) {
            if (txtRemarks.value.length > parseInt($get(_clienthidRemarkLength).value)) {
                txtRemarks.value = txtRemarks.value.substring(0, parseInt($get(_clienthidRemarkLength).value));
                return false;
            }
            if ($get(txtRemarks.id.replace("_txt", "_lbl")) != null) {
                updateTextBoxCounter(txtRemarks);
            }
        }

        function updateTextBoxCounter(txtRemarks) {
            var unicodeFlag = 0;
            var extraChars = 0;
            var msgCount = 0;
            var sMsgTxt = txtRemarks.value;
            var TotalCount = 0;
            var i = 0;
            for (; (i < sMsgTxt.length); i++) {
                if ((sMsgTxt.charAt(i) >= '0') && (sMsgTxt.charAt(i) <= '9')) {
                }
                else if ((sMsgTxt.charAt(i) >= 'A') && (sMsgTxt.charAt(i) <= 'Z')) {
                }
                else if ((sMsgTxt.charAt(i) >= 'a') && (sMsgTxt.charAt(i) <= 'z')) {
                }
                else if (sMsgTxt.charAt(i) == '@') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA3) {
                }
                else if (sMsgTxt.charAt(i) == '$') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xEC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF2) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC7) {
                }
                else if (sMsgTxt.charAt(i) == '\r') {
                }
                else if (sMsgTxt.charAt(i) == '\n') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x394) {
                }
                else if (sMsgTxt.charAt(i) == '_') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x393) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39B) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A0) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A3) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x398) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39E) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xDF) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC9) {
                }
                else if (sMsgTxt.charAt(i) == ' ') {
                }
                else if (sMsgTxt.charAt(i) == '!') {
                }
                else if (sMsgTxt.charAt(i) == '\"') {
                }
                else if (sMsgTxt.charAt(i) == '#') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA4) {
                }
                else if (sMsgTxt.charAt(i) == '%') {
                }
                else if (sMsgTxt.charAt(i) == '&') {
                }
                else if (sMsgTxt.charAt(i) == '\'') {
                }
                else if (sMsgTxt.charAt(i) == '(') {
                }
                else if (sMsgTxt.charAt(i) == ')') {
                }
                else if (sMsgTxt.charAt(i) == '*') {
                }
                else if (sMsgTxt.charAt(i) == '+') {
                }
                else if (sMsgTxt.charAt(i) == ',') {
                }
                else if (sMsgTxt.charAt(i) == '-') {
                }
                else if (sMsgTxt.charAt(i) == '.') {
                }
                else if (sMsgTxt.charAt(i) == '/') {
                }
                else if (sMsgTxt.charAt(i) == ':') {
                }
                else if (sMsgTxt.charAt(i) == ';') {
                }
                else if (sMsgTxt.charAt(i) == '<') {
                }
                else if (sMsgTxt.charAt(i) == '=') {
                }
                else if (sMsgTxt.charAt(i) == '>') {
                }
                else if (sMsgTxt.charAt(i) == '?') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xDC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA7) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xBF) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xFC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE0) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x391) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x392) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x395) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x396) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x397) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x399) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39A) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39C) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39D) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39F) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A7) {
                }
                else if (sMsgTxt.charAt(i) == '^') {
                }
                else if (sMsgTxt.charAt(i) == '{') {
                }
                else if (sMsgTxt.charAt(i) == '}') {
                }
                else if (sMsgTxt.charAt(i) == '\\') {
                }
                else if (sMsgTxt.charAt(i) == '[') {
                }
                else if (sMsgTxt.charAt(i) == '~') {
                }
                else if (sMsgTxt.charAt(i) == ']') {
                }
                else if (sMsgTxt.charAt(i) == '|') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x20AC) {
                }
                else {
                    unicodeFlag = 1;
                }
                TotalCount = parseInt(i + extraChars);
                if (TotalCount >= parseInt($get(_clienthidRemarkLength).value)) {
                    sMsgTxt = sMsgTxt.substring(0, i);
                    break;
                }
            }
            if (TotalCount >= parseInt($get(_clienthidRemarkLength).value))
                txtRemarks.value = sMsgTxt;
            if (unicodeFlag) {
                msgCount = sMsgTxt.length;
                if (msgCount <= 70) {
                    msgCount = 1;
                }
                else {
                    msgCount += (67 - 1);
                    msgCount -= (msgCount % 67);
                    msgCount /= 67;
                }
                $get(txtRemarks.id.replace("_txt", "_lbl")).innerHTML = "&nbsp;(" + (parseInt($get(_clienthidRemarkLength).value) - sMsgTxt.length) + ")";
            }
            else {
                msgCount = sMsgTxt.length + extraChars;
                if (msgCount <= 160) {
                    msgCount = 1;
                }
                else {
                    msgCount += (153 - 1);
                    msgCount -= (msgCount % 153);
                    msgCount /= 153;
                }
                $get(txtRemarks.id.replace("_txt", "_lbl")).innerHTML = "&nbsp;(" + (parseInt($get(_clienthidRemarkLength).value) - sMsgTxt.length) + ")";
            }
        }

        function UpdateLength(rowIndex) {
            rowIndex = rowIndex + 2
            if (rowIndex >= 10)
                str = "_ctl" + rowIndex;
            else
                str = "_ctl0" + rowIndex;

            txt = document.getElementById(_clientGridId + str + "_txtRemark")
            lbl = document.getElementById(_clientGridId + str + "_lblRemarkLength")

            length = parseInt($get(_clienthidRemarkLength).value)
            remarkLength = txt.value.length;
            lbl.innerHTML = (length - remarkLength)
        }

        function ValidateRemarkLength(oSrc, args) {
            var rowIndex = 0
            var rollNos = ''
            var length = parseInt($get(_clienthidRemarkLength).value)
            rowIndex = rowIndex + 2
            if (rowIndex >= 10)
                str = "_ctl" + rowIndex;
            else
                str = "_ctl0" + rowIndex;
            txt = document.getElementById(_clientGridId + str + "_txtRemark")
            while (txt != null) {
                txt.value = txt.value.trim()

                if (txt.value != "") {
                    if (txt.value.length > length) {
                        lbl = document.getElementById(_clientGridId + str + "_lblRollNo")
                        rollNos = rollNos + ", " + lbl.innerHTML;
                    }
                }

                rowIndex = rowIndex + 1;
                if (rowIndex >= 10)
                    str = "_ctl" + rowIndex;
                else
                    str = "_ctl0" + rowIndex;

                txt = document.getElementById(_clientGridId + str + "_txtRemark")
            }

            if (rollNos.length > 0) {
                rollNos = rollNos.substring(1);
                oSrc.errormessage = "Remark length should not be greater than " + length + " characters for Roll No(s) : " + rollNos;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

    </script>
    <script language="javascript" type="text/javascript">

        _cltdivTemplates = "<%=this.divTemplates.ClientID %>"

        var _totalWinHeight;
        var _adjWinHeight;
        var _rightFooterPos;
        var _bottomFooterPos;
        var _adjWinWidth;
        var _rightPosition;

        window.onresize = setPopupPosition;
        window.onscroll = setPopupPosition;
        window.onload = setPopupPosition;

        function setPopupPosition() {
            _totalWinHeight = document.body.scrollHeight;
            _adjWinHeight = _totalWinHeight; //-608;
            _adjWinWidth = document.body.scrollWidth;

            if (document.getElementById(_cltdivTemplates) != null) {
                _rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltdivTemplates).style.height);
                document.getElementById(_cltdivTemplates).style.top = _rightFooterPos;
            }

            if (document.getElementById(_cltdivTemplates) != null) {
                _rightPosition = parseInt(screen.width / 2) - parseInt(parseInt(document.getElementById(_cltdivTemplates).style.width) / 2);
                //_rightPosition = document.body.clientWidth - parseInt(document.getElementById(_cltdivTemplates).style.width) - wdt;
                document.getElementById(_cltdivTemplates).style.left = _rightPosition;
            }

            window_onscroll();
        }

        function window_onscroll() {
            if (document.body.scrollTop <= _adjWinHeight) {
                if (document.getElementById(_cltdivTemplates) != null) {
                    document.getElementById(_cltdivTemplates).style.top = document.body.scrollTop + _rightFooterPos;
                }
            }

            if (document.body.scrollLeft <= _adjWinWidth) {
                if (document.getElementById(_cltdivTemplates) != null) {
                    document.getElementById(_cltdivTemplates).style.left = document.body.scrollLeft + _rightPosition;
                }
            }
        }        
    </script>
    <script language="javascript" type="text/javascript">

        function OnGridKeyUp(obj, decimalPlaces, allowNegative, e) {
            extractNumber(obj, decimalPlaces, allowNegative);
            UpDownKeyPress(obj.id, e, _clientGridId);
        }

        function SetData(obj, cls) {
            if (confirm('This action will set new value for all students. Do you want to continue?')) {
                cls = '.' + cls
                $(cls).each(function () {
                    if (!$(this).is(':disabled')) {
                        $(this).val($(obj).val())

                        var data = this.id.split('_ctl')
                        var dt = data[0] + '_ctl' + data[1].substring(0, 2)
                        SetRowTotalMarks(dt)
                    }
                })
            }
        }

    </script>
</asp:Content>
