<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AssignSummaryGradesUI.aspx.cs" Inherits="AssignSummaryGradesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
                            <asp:CustomValidator ID="cstGrades" runat="server" ClientValidationFunction="ValidatesGrades"
                                Display="None" ErrorMessage="Grade should be assigned for all students."></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdMessage" runat="server">
                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true" Text=""
                                EnableViewState="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Exam : </span>
                            </td>
                            <td class="ClsHilightBGB" width="150px">
                                <asp:Label ID="lblExam" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                            </td>
                            <td class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Subject : </span>
                            </td>
                            <td class="ClsHilightBGB" width="150px">
                                <asp:Label ID="lblSubject" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="50%">
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwAssignSummeryGrades" runat="server" DataKeyNames="YearwiseStudentId"
                                            OnItemDataBound="lstvwAssignSummeryGrades_ItemDataBound">
                                            <LayoutTemplate>
                                                <table id="Table1" width="100%" runat="server" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" class="paddingLR" width="100px">
                                                            Roll No.
                                                        </th>
                                                        <th align="left" class="paddingLR">
                                                            Student Name
                                                        </th>
                                                        <th align="center" class="paddingLR" width="100px">
                                                            Grade
                                                        </th>
                                                    </tr>
                                                    <tr id="tr1" runat="server" class="ClsGridHeader">
                                                        <th>
                                                        </th>
                                                        <th>
                                                        </th>
                                                        <th id="thGradeAll" runat="server">
                                                            <asp:DropDownList ID="ddlGradeAll" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trItemtemplate" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                    <td align="center">
                                                        <asp:Label ID="lblRollNo" runat="server" Style="float: inherit" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="ddlGrade" runat="server" CssClass="MidCombo GradeData" ViewStateMode="Enabled">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <div class="LblNoRecord">
                                                    No Record Found.
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBack" Text="Back" runat="server" CssClass="ClsBtn" CausesValidation="false" />
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" OnClick="btnSave_Click"
                                Enabled="false" AutoPostback="false" />
                            <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="ClsBtn" OnClick="btnSubmit_Click"
                                Enabled="false" CausesValidation="false" />
                            <asp:HiddenField ID="hidTestId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidSubjectId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidTeacherId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidIsClassTeacher" runat="server" Value="N" />
                            <asp:HiddenField ID="hidFilterStdDivId" runat="server" Value="0" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">
            _clientlstvwAssignSummeryGrades = "<%=this.lstvwAssignSummeryGrades.ClientID %>"

            function ValidatesGrades(src, args) {
                var isFound = false;
                $('[id$=_ddlGrade]').each(function () {

                    if ($(this).val() == '0') {
                        isFound = true;
                        return;
                    }
                })

                if (isFound) {
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }

            function SetGrades(obj) {
                $('.GradeData').val($('#' + obj).val())
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
