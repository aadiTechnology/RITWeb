<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentwiseProgreesReportUI.aspx.cs"
    Inherits="StudentwiseProgreesReportUI" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table id="tblLearningOutcome" runat="server" border="0" cellpadding="0" cellspacing="2"
                            style="height: 100%; width: 100%;">
                            <tr>
                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                    <span class="ClsMdtStar">* <asp:Label ID="lblStandardText" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label></span>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Blue" CssClass="LblNormal" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="LegendTable" visible="false" runat="server" align="center" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td align="left" colspan="1" style="padding-right: 5px">
                                                <asp:Label ID="lblLegend" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                    EnableViewState="false" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                            </td>                                      
                                            <td align="left" colspan="1">
                                                &nbsp;<asp:Image ID="Image1" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/icoGrid_MarkEntryNotStart.gif" />
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:Label ID="Label4" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                    Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryNotStarted %>"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1">
                                                &nbsp;<asp:Image ID="Image2" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/icoGrid_MarkEntryPartDone.gif" />
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:Label ID="Label8" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                    Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryPartiallyDone %>"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1">
                                                &nbsp;
                                                <asp:Image ID="Image3" runat="server" CssClass="img-align-unset" EnableViewState="False" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:Label ID="Label9" runat="server" CssClass="ClsTextNormal" EnableViewState="False"
                                                    Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryCompleted %>"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="tblTeachers" runat="server" align="center" width="850px">
                                        <tr>
                                            <td align="center">
                                                <table>
                                                    <tr>
                                                        <td id="tdClassTeacherLable" runat="server" align="left" class="ClsBorderlight" colspan="1">
                                                            <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, SelectClassTeacher %>"></asp:Label>
                                                                <span class="ClsLblLgnd colonPadding"> :</span>
                                                        </td>
                                                        <td id="tdcmbTeachers" runat="server" align="left" colspan="1">
                                                            <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="ExLrgCombo"
                                                                OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged" Width="260px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td id="tdAssessmentLable" runat="server" align="left" class="ClsBorderlight" visible="false" colspan="1">
                                                            <asp:Label ID="lblAssessmnt" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, SelectAssessment %>"></asp:Label>
                                                                <span class="ClsLblLgnd colonPadding"> :</span>
                                                        </td>
                                                        <td id="tdCmbAssessment" runat="server" align="left" colspan="1" visible="false">
                                                            <asp:DropDownList ID="CmbAssessment" AutoPostBack="true" runat="server" 
                                                                CssClass="ExLrgCombo" Width="260px" 
                                                                onselectedindexchanged="CmbAssessment_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trPrecondition" runat="server" visible="false">
                                <td>
                                    <div runat="server" id="divErr">
                                    </div>
                                </td>
                            </tr>
                            <tr id="trStudentDetails" runat="server">
                                <td align="center">
                                    <table align="center" width="850px">
                                        <tr id="trPager" runat="server">
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentDetails">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <asp:Label EnableViewState="false" runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblTo" runat="server" EnableViewState="false" CssClass="LblNormal"
                                                                    Text="<%$ Resources:LocalizedResources, To %>" />
                                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                    Text="<%$ Resources:LocalizedResources, OutOf %>"/>
                                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblRecords" runat="server" EnableViewState="false" CssClass="LblNormal"
                                                                    Text="<%$ Resources:LocalizedResources, Records %>" />
                                                                <br />
                                                            </PagerTemplate>
                                                        </asp:TemplatePagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstvwStudentDetails" runat="server" DataKeyNames="YearwiseStudentId,ProgresSheetID,Standard_Division_Id,StandardId,ProgressReportType,ShowProgressReport,EditStatus,RollNo,ShowDeleteButton"
                                                    OnDataBound="lstvwStudentDetails_DataBound" OnItemCommand="lstvwStudentDetails_ItemCommand"
                                                    OnItemDataBound="lstvwStudentDetails_ItemDataBound" OnSorting="lstvwStudentDetails_Sorting"
                                                    OnPreRender="lstvwStudentDetails_PreRender">
                                                    <LayoutTemplate>
                                                        <table align="center" width="100%" runat="server" id="tblStudentDetails" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" style="padding-left: 7px; width: 100px;">
                                                                    <asp:LinkButton ID="lnkBtnRollNo" runat="server" CommandName="Sort" CommandArgument="Roll_No"
                                                                        CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, RollNo %>"></asp:LinkButton>
                                                                </th>
                                                                <th align="left" style="padding-left: 7px; width: 500px;">
                                                                    <asp:LinkButton ID="lnkBtnStudentName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                        CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, StudentName %>"></asp:LinkButton>
                                                                </th>
                                                                <th align="center" style="padding-left: 3px; width: 140px">
                                                                    <asp:Label runat="server" ID="lblEditText" Text="<%$ Resources:LocalizedResources, Edit %>"></asp:Label>
                                                                </th>
                                                                <%-- <th align="center" style="width: 140px;">
                                                                    Progress Report
                                                                </th>--%>
                                                                <th ID="thdelete" runat="server"   align="center" style="padding-left: 3px; width: 140px" >
                                                                Delete
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="5">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentDetails"
                                                                        PageSize="20">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectAPage %>" runat="server" CssClass="LblNrmlB" />
                                                                                                <asp:DropDownList ID="ddlCnt" runat="server" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged"
                                                                                                    AutoPostBack="true">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td align="right" class="LblNormal">
                                                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </PagerTemplate>
                                                                            </asp:TemplatePagerField>
                                                                        </Fields>
                                                                    </asp:DataPager>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblMenuName" EnableViewState="false" runat="server" Text='<%# Eval("RollNo") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblLinkName" runat="server" EnableViewState="false" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:HyperLink ID="hyplnkEdit" runat="server" NavigateUrl="~/RITeSchool/ProgressReport/AssignExamWiseStudentMarksUI.aspx"
                                                                    Text="Assign Marks"></asp:HyperLink>
                                                            </td>
                                                            <td id="tddelete" runat="server"  class="paddingL"  align="center">
                                                             <asp:ImageButton ID="btndelete" runat="server" CausesValidation="false" CommandName="RemoveCommand" CommandArgument='<%# Eval("StandardId") %>'
                                                                     ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                            <%--<td align="center">
                                                            <asp:HyperLink ID="hyplnkView" runat="server" Visible="false" NavigateUrl="~/RITeSchool/ProgressReport/StudentWiseProgressSheet.aspx" Text="View Progress Report"></asp:HyperLink>
                                                                <%--<asp:ImageButton ID="imgBtnView" runat="server" Visible="false" CausesValidation="false"
                                                                    CommandName="Report" ImageUrl="../images/view.png" />
                                                            </td>--%>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblMenuName" runat="server" EnableViewState="false" Text='<%# Eval("RollNo") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblLinkName" runat="server" EnableViewState="false" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:HyperLink ID="hyplnkEdit" runat="server" NavigateUrl="~/RITeSchool/ProgressReport/AssignExamWiseStudentMarksUI.aspx"
                                                                    Text="Assign Marks"></asp:HyperLink>
                                                            </td>
                                                            <td id="tddelete" runat="server"   class="paddingL" align="center">
                                                              <asp:ImageButton ID="btndelete" runat="server" CausesValidation="false" CommandName="RemoveCommand" CommandArgument='<%# Eval("StandardId") %>'
                                                                     ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                            <%--<td align="center">
                                                                <%--<asp:ImageButton ID="imgBtnView" runat="server" Visible="false" CausesValidation="false"
                                                                    CommandName="Report" ImageUrl="../images/view.png" />
                                                                <asp:HyperLink ID="hyplnkView" runat="server" Visible="false" NavigateUrl="~/RITeSchool/ProgressReport/StudentWiseProgressSheet.aspx" Text="View Progress Report"></asp:HyperLink>
                                                            </td>--%>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr>
                                                            <td class="LblNoRecord" align="center">
                                                               <asp:Label ID="lblNoRecordFound" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>" runat="server"/>
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td align="center">                   
                                                <asp:Button ID="btnPublish" runat="server" style="width: 90px" CssClass="ClsBtn" Text="Publish" Visible="false" OnClick="btnPublish_Click" />
                                             </td>
                                         </tr>
                                        <tr>
                                            <td>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="True" ID="ObjDSStudentDetails"
                                                    SortParameterName="sortExpression" runat="server" SelectMethod="GetAll" SelectCountMethod="GetCount"
                                                    EnableCaching="False" OnObjectDisposing="ObjDSStudentDetails_ObjectDisposing"
                                                    OnObjectCreating="ObjDSStudentDetails_ObjectCreating">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                            Type="int32" />
                                                        <asp:ControlParameter Name="aiStdDivId" ControlID="hidTeacherId" Type="Int32"
                                                            PropertyName="Value" />
                                                        <asp:ControlParameter Name="aiAssessmentId" ControlID="hidAssessmentId" Type="Int32"
                                                            PropertyName="Value" />
                                                        <asp:Parameter Name="sortExpression" Type="String" />
                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                <asp:HiddenField ID="hidUserHasFullAccess" runat="server" />
                                                <asp:HiddenField ID="hidTeacherId" runat="server" />
                                                <asp:HiddenField ID="hidAssessmentId" runat="server" />
                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                <asp:HiddenField ID="hidEdited" runat="server" />
                                                  <asp:HiddenField ID="hidShowProgressReport" runat="server" />
                                                <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            <td align="center">
                             <asp:Button ID="btndeleteGrade" runat="server" Text="Delete" CssClass="ClsBtn" CausesValidation="false" OnClick="btndeleteGrade_Click"/>
                             </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script>
        function ConfirmDelete() {
            return confirm('Are you sure you want to delete grades of selected assessment of selected student?')
        }

        function ConfirmDeleteAll() {
            return confirm('Are you sure you want to delete grades of selected assessment of all students.?')
        }

    </script>
</asp:Content>