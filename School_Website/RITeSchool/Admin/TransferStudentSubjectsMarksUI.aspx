<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransferStudentSubjectsMarksUI.aspx.cs" Inherits="TransferStudentSubjectsMarksUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="updtpnl1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="90%">
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                            Width="100%" CssClass="ClsMdtStar"></asp:Label>
                    </td>
                    <td align="right" style="color: #ff3333" valign="top">
                        <span class="ClsMdtStar">* Mandatory Fields </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div runat="server" id="divErr">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblNoOptionalSubjectConfigMsg" Style="text-align: center" runat="server"
                            ForeColor="blue" Width="60%" Visible="False" CssClass="LblNoRecord" EnableViewState="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table id="tblSearchFilters" runat="server" width="1000px">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    <table width="900px">
                                        <tr id="trTeacherControl" runat="server">
                                            <td id="tdTeacherlbl" runat="server" align="left" class="ClsBorderlight" style="width: 200px"
                                                colspan="1">
                                                <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="clsLabel" Text="Select Class Teacher :"></asp:Label>
                                            </td>
                                            <td id="tdTeachercmb" runat="server" align="left" colspan="1">
                                                <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="ExLrgCombo"
                                                    OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span id="ctl00_MainBody_Label3" class="ClsMdtStar" style="color:Red;">*</span>
                                            </td>
                                            <%--</tr>
                                        <tr>--%>
                                            <td class="ClsBorderlight" align="right" style="width: 186px">
                                                <span class="clsLabel">Student Name / Reg.No. :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" CssClass="MidTxtBox" AutoPostBack="False" autocomplete="off"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" CausesValidation="true"
                                                    OnClick="btnSearch_Click" />
                                                <asp:Button ID="btnSearchBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="False" UseSubmitBehavior="false" OnClick="btnBack_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:UpdatePanel ID="upnl2" runat="server">
                            <ContentTemplate>
                                <table id="tblPagerStudentMarksTransfer" runat="server" border="0" cellpadding="0"
                                    cellspacing="2" style="width: 100%;">
                                    <tr id="trPagerStudentMarksTransfer" runat="server">
                                        <td align="center">
                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwStudentMarks">
                                                <Fields>
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                CssClass="LblNrmlB" />
                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                CssClass="LblNrmlB" />
                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                CssClass="LblNrmlB" />
                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                            <br />
                                                        </PagerTemplate>
                                                    </asp:TemplatePagerField>
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <%-- <div id="div1" runat="server" style="width: 635pt; height: 100%; ">--%>
                                            <table id="tblTransferDetails" align="center" width="1100px">
                                                <tr align="center" id="trStudentInfo" runat="server" style="width: 1100px">
                                                    <td style="width: 1100px">
                                                        <table width="1100px">
                                                            <tr>
                                                                <td align="center" style="width: 760px; vertical-align: top">
                                                                    <asp:ListView ID="lstvwStudentMarks" DataKeyNames="YearwiseStudentId,Standard_Division_Id"
                                                                        runat="server" OnDataBound="lstvwStudentMarks_DataBound">
                                                                        <LayoutTemplate>
                                                                            <table align="center" width="760px" runat="server" id="tblTransfer" style="color: #333333"
                                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="center" width="50px">
                                                                                        <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckUncheckAllCheckBoxes(this);" />
                                                                                    </th>
                                                                                    <th align="left" width="100px" style="padding-left: 8px;">
																			            Reg. No.
																		            </th>
                                                                                    <th align="left" width="80px" style="padding-left: 8px;">
                                                                                        Roll No.
                                                                                    </th>
                                                                                    <th align="left" width="300px" style="padding-left: 8px;">
                                                                                        Student Name
                                                                                    </th>
                                                                                    <%--<th align="center" width="150px">
                                                                                        Class
                                                                                    </th>--%>
                                                                                    <th align="left" width="350px" class="paddingL">
                                                                                        Current Applicable Subjects
                                                                                    </th>
                                                                                    <%--<th align="center" width="200px">
																			Transfer Marks To Subject
																		</th>--%>
                                                                                </tr>
                                                                                <tr runat="server" id="itemPlaceholder">
                                                                                </tr>
                                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                                    <td colspan="7">
                                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentMarks"
                                                                                            PageSize="20">
                                                                                            <Fields>
                                                                                                <asp:TemplatePagerField>
                                                                                                    <PagerTemplate>
                                                                                                        <table width="750px">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
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
                                                                                <td align="center">
                                                                                    <asp:CheckBox ID="ChkSelectAll" runat="server" />
                                                                                </td>
                                                                                <td align="left" class="paddingL">
																		            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%# Eval("RegNo") %>'></asp:Label>
																	            </td>
                                                                                <td align="left" class="paddingL">
                                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("RollNo") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" class="paddingL">
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                                </td>
                                                                                <%--<td align="center">
                                                                                    <asp:Label ID="lblClass" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                                                                                </td>--%>
                                                                                <td align="left" class="paddingL">
                                                                                    <asp:Label ID="lblTransferSubjectName" runat="server" Text='<%# Eval("TransferFromSubjectName") %>'></asp:Label>
                                                                                </td>
                                                                                <%--<td align="center">
																		<asp:DropDownList ID="ddlSubjects" runat="server" CssClass="SmlCombo" AutoPostBack="true"
																			Height="22px" Width="125px">
																		</asp:DropDownList>
																	</td>--%>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                <td align="center">
                                                                                    <asp:CheckBox ID="ChkSelectAll" runat="server" />
                                                                                </td>
                                                                                <td align="left" class="paddingL">
																		            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%# Eval("RegNo") %>'></asp:Label>
																	            </td>
                                                                                <td align="left" class="paddingL">
                                                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("RollNo") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" class="paddingL">
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                                </td>
                                                                                <%--<td align="center">
                                                                                    <asp:Label ID="lblClass" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                                                                                </td>--%>
                                                                                <td align="left" class="paddingL">
                                                                                    <asp:Label ID="lblTransferSubjectName" runat="server" Text='<%# Eval("TransferFromSubjectName") %>'></asp:Label>
                                                                                </td>
                                                                                <%--<td align="center">
																		<asp:DropDownList ID="ddlSubjects" runat="server" CssClass="SmlCombo" AutoPostBack="true"
																			Height="22px" Width="125px">
																		</asp:DropDownList>
																	</td>--%>
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
                                                                </td>
                                                                <td id="tdOptionalSubject" runat="server" style="vertical-align: top">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span style="color: #1e90ff; font-weight: bold; margin-left: 5px; font-family: Arial; font-size: 10pt;">Optional Subjects :</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="divTreeView" class="GridBorder" runat="server">
                                                                                    <asp:TreeView ID="trvwOptionalSubject" runat="server" ImageSet="Arrows" CollapseImageUrl="">
                                                                                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                                                        <Nodes>
                                                                                            <asp:TreeNode Text="Optional Subjects" RunAt="server"></asp:TreeNode>
                                                                                        </Nodes>
                                                                                        <NodeStyle Font-Names="Arial" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px"
                                                                                            NodeSpacing="0px" VerticalPadding="0px" />
                                                                                        <RootNodeStyle Font-Names="Arial" Font-Bold="true" />
                                                                                        <ParentNodeStyle Font-Names="Arial" Font-Bold="true" />
                                                                                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                                                                                            VerticalPadding="0px" />
                                                                                    </asp:TreeView>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <div runat="server" id="divErrMsg">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <table id="tblNotes" runat="server" align="center" width="1100px">
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight " style="width: 100px; background-color: #ffffc4;">
                                                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 1 :"
                                                                        CssClass="LblNrmlB"></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                                    <asp:Label ID="lblNote" runat="server" Width="1000px" BorderWidth="0px" CssClass="LblSmlV"
                                                                        Text="At least 2 optional subjects should have been configured to transfer marks."></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight " style="width: 100px; background-color: #ffffc4;">
                                                                    <asp:Label ID="Label3" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 2 :"
                                                                        CssClass="LblNrmlB"></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                                    <asp:Label ID="Label4" runat="server" Width="1000px" BorderWidth="0px" CssClass="LblSmlV"
                                                                        Text="If marks are not assigned to current subject(s) for selected student and if you change to new subject(s) then only new subject(s) will be assigned to student."></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight " style="width: 100px; background-color: #ffffc4;">
                                                                    <asp:Label ID="Label7" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 3 :"
                                                                        CssClass="LblNrmlB"></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                                    <asp:Label ID="Label8" runat="server" Width="1000px" BorderWidth="0px" CssClass="LblSmlV"
                                                                        Text="If marks are assigned to current subject(s) for selected student and if you change to new subject(s) then along with new subject(s) assignment, marks of current subject(s) will be transferred to new subject(s)."></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight " style="width: 100px; background-color: #ffffc4;">
                                                                    <asp:Label ID="Label5" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 4 :"
                                                                        CssClass="LblNrmlB"></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                                    <asp:Label ID="Label6" runat="server" Width="1000px" BorderWidth="0px" CssClass="LblSmlV"
                                                                        Text="Marks cannot be transferred across the subject groups."></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button ID="btnTransfer" Text="Transfer" runat="server" CausesValidation="true"
                                                            CssClass="ClsBtn" OnClick="btnTransfer_Click" />
                                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                                                            CausesValidation="False" UseSubmitBehavior="false" OnClick="btnBack_Click" />
                                                    </td>
                                                </tr>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.StudentSubjectMarksBL" EnablePaging="True"
                                                    ID="ObjDSStudentMarksTransfer" runat="server" SelectMethod="GetStudentsToTransferMarks"
                                                    SelectCountMethod="GetStudentsCountToTransferMarks" EnableCaching="False">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                            Type="int32" />
                                                        <asp:ControlParameter ControlID="hidStandardDivisionId" Type="Int32" PropertyName="Value"
                                                            DefaultValue="-1" Name="aiStandardDivisionId" />
                                                        <asp:ControlParameter ControlID="txtSearch" Type="String" PropertyName="Text" DefaultValue=""
                                                            Name="asName" />
                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:HiddenField ID="hidMode" runat="server" />
                                                <asp:HiddenField ID="hidServerDate" runat="server" />
                                                <asp:HiddenField ID="hidUserHasFullAccess" runat="server" />
                                                <asp:HiddenField ID="hidTeacherId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidStandardDivisionId" runat="server" />
                                                <asp:HiddenField ID="hidPageNo" runat="server" Value="1" />
                                            </table>
                                            <%--</div>--%>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnTransfer" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnTransfer" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lstvwStudentMarks" EventName="DataBound" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript" language="javascript">
        _clienthidPageNo = "<%=hidPageNo.ClientID %>"
        _clientlblUpdateSucess = "<%=lblUpdateSucess.ClientID %>"
        _sClientGridId = "<%=lstvwStudentMarks.ClientID %>"
        _ClienthidTeacherId = "<%=hidTeacherId.ClientID %>"
        _ClientcmbTeachers = "<%=cmbTeachers.ClientID %>"
        _clienttrvwOptionalSubject = "<%=this.trvwOptionalSubject.ClientID %>"

        function ConfirmMessage(oCmb) {
            document.getElementById(_clientlblUpdateSucess).value = "";
            var bIsValid
            if (window.confirm("Modified data on the current page will be lost. Do you want to continue?"))
                bIsValid = true
            else {
                if (document.getElementById(oCmb) != null) {
                    document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                }
                document.getElementById(_ClientcmbTeachers).value = document.getElementById(_ClienthidTeacherId).value
                bIsValid = false
            }
            return bIsValid
        }

        function ValidateSubjects() {
            var IsChecked = false;

            var len = $("input:checkbox[id*=ChkSelectAll]:checked").length;

            if (len == 0) {
                alert("At least one student subject should be selected.");
                return false;
            }
            else return true;
        }

        function ClearTreeview() {
            $("input:checkbox[id*=trvwOptionalSubject]").attr('checked', false);
            $("input:checkbox[id*=ChkSelectAll]").attr('checked', false);
        }

        function CheckUncheckAllCheckBoxes(chkSelect) {
            $("input:checkbox[id*=ChkSelectAll]").attr('checked', chkSelect.checked);
        }

    </script>    

     <script language="javascript" type="text/javascript">

         $(document).ready(function () {
             AutoSearch();
         });
         function AutoSearch() {
             var SchoolId = "<%=miSchoolId %>";
             _clienttxtRegNumber = '#<%=txtSearch.ClientID%>';
             var AcademicYearId = "<%=miAcademicYearId %>"
             BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, _ClientcmbTeachers, 1);
         }
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         prm.add_endRequest(EndRequestHandler);

         // This function is used to enabled controls once a postback is complete.

         function EndRequestHandler() {
             AutoSearch();
         }

         function SearchSelectedValue(val) {
             txt = document.getElementById("<%=this.txtSearch.ClientID %>");
             bt = document.getElementById("<%=this.btnSearch.ClientID %>");             
             SearchResult(txt, val, bt);
         }
     </script>

</asp:Content>
