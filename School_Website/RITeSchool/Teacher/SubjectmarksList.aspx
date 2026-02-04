<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="SubjectmarksList.aspx.cs" Inherits="SubjectmarksList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%">
            <tr align="center" valign="top">
                <td align="center">
                    <table style="width: 98%">
                        <tr>
                            <td style="width: 50%" align="center">
                                <table>
                                    <tr>
                                        <td>
                                            <%--<asp:Label ID="lblStdDivName" runat="server" Text="Class :" EnableViewState="False"
                                                CssClass="ClsLblLgnd"></asp:Label>--%>
                                                <span class="ClsLblLgnd" id="lblStdDivName">Class :</span>
                                        </td>
                                        <td class="ClsHilightBGB">
                                            <asp:Label ID="lblDataStdDiv" runat="server" EnableViewState="False"></asp:Label>
                                        </td>
                                        <td class="ClsHilightText ">
                                            <%--<asp:Label ID="lblExam" runat="server" Text="Exam :" EnableViewState="False" CssClass="ClsLblLgnd"></asp:Label>--%>
                                            <span class="ClsLblLgnd" id="lblExam">Exam :</span>
                                        </td>
                                        <td class="ClsHilightBGB">
                                            <asp:Label ID="lblDataExam" runat="server" EnableViewState="False"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:Label ID="lblSubjectName" runat="server" Text="Subject Name :" EnableViewState="False"
                                                CssClass="ClsLblLgnd"></asp:Label>--%>
                                                <span class="ClsLblLgnd" id="lblSubjectName">Subject Name :</span>
                                        </td>
                                        <td class="ClsHilightBGB">
                                            <asp:Label ID="lblDataSubjectName" runat="server" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td align="center">
                                <table id="LegendTable" runat="server">
                                    <tr id="trLedgend" runat="server">
                                        <td id="Td1" runat="server" align="left" class="ClsBorderlight" colspan="1" enableviewstate="false" visible="true">
											<span class="ClsLblLgnd" id="Span1" style="padding: 2px;">Legend :</span>
                                        </td>
										<td style="width: 5px;"></td>
                                        <td align="right" id="BlanktdAssignment" visible="false" runat="server" style="width: 5px;">
                                        </td>
                                        <%--<td id="tdAbsent" runat="server" enableviewstate="false" align="left" visible="false" colspan="1" class="lblBorderRedB">
                                            <asp:Label ID="lblAbsent"
													   runat="server"
													   Font-Bold="True"
													   Text="A : Absent"
													   EnableViewState="False" />
                                        </td>
                                        <td id="tdExempted" runat="server" enableviewstate="false" align="left" visible="false" colspan="1" class="lblBorderMaroonB">
                                            <asp:Label ID="lblExempted"
													   runat="server"
													   Font-Bold="True"
													   Text="E : Exempted"
													   EnableViewState="False" />
                                        </td>--%>
                                        <td align="right" id="BlanktdAbsent" visible="false" runat="server" style="width: 5px;">
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight ClslblWithBorder" id="tdFailStudentLegend" runat="server">
                                            <asp:Label ID="Label1"
													   runat="server"
													   ForeColor="Red"
													   Text="Fail Student"
													   EnableViewState="False"
													   Width="70px" />
                                        </td>
                                        <td id="BlankFailStudent" runat="server" align="right" style="width: 5px;">
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
											<span class="ClsLblLgnd" id="Span2">Toppers of the class :</span>
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <img src="../images/Number1.gif" />
                                            <img src="../images/Number2.gif" />
                                            <img src="../images/Number3.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="divGridView" runat="server" visible="true" style="overflow: auto">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                            <tr id="trGrid" runat="server">
                                <td align="center" valign="top">
                                    <asp:GridView CssClass="GridBorder" ID="grdStudentMarks" Width="25%" runat="server"
                                        AutoGenerateColumns="False" PageSize="30" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                        GridLines="None" DataKeyNames='student_id,Name' EnableViewState="false" EmptyDataText="Record Not found">
                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                        </PagerStyle>
                                        <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                            FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                        <Columns>
                                            <asp:BoundField HeaderText="R.No." SortExpression="Roll_No" DataField="Roll_No">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Student Name" SortExpression="Name" DataField="Name"
                                                Visible="False">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                            <asp:TemplateField Visible="False">
                                                <HeaderTemplate>
                                                    <asp:DropDownList ID="ddlHeaderGrade" runat="server" Visible="false" />
                                                    <asp:Label ID="txtHeaderGrade" runat="server" Text="Grade" BackColor="Transparent"
                                                        ForeColor="black" BorderStyle="None" CssClass="ClsLbl" Font-Bold="true" EnableViewState="false"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="ddlGrade" runat="server" BackColor="Transparent" ForeColor="black"
                                                        BorderStyle="None" CssClass="ClsLbl" EnableViewState="false" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="ClsGridRow" />
                                        <HeaderStyle CssClass="ClsGridHeader" />
                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        <EmptyDataRowStyle CssClass="LblNoRecord" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" OnClick="btnBack_Click" />
                </td>
            </tr>           
        </table>
        <table id="tblNote" runat="server" width="450px">
                    <tr>
                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">                            
                                <span class="LblNrmlB" style="font-weight:bold;border-width:0px;">Note :</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">                            
                            <span class="LblSmlV" style="border-width:0px;">To view the student name take your mouse on the roll number.</span>
                        </td>
                    </tr>
                                            </table>
        <asp:HiddenField ID="hidTeacherName" runat="server"></asp:HiddenField>
        <br />
        <br />
    </div>
    <asp:HiddenField ID="hidStandardDivisionId" runat="server" />
    <asp:HiddenField ID="hidSubjectId" runat="server" />
    <asp:HiddenField ID="hidTestId" runat="server" />
    <asp:HiddenField ID="hidTeacherId" runat="server" />
    <asp:HiddenField ID="hidOralPassingMarks" runat="server" />
    <asp:HiddenField ID="hidWrittenPassingMarks" runat="server" />
    <asp:HiddenField ID="hidPracticalPassingMarks" runat="server" />
    <asp:HiddenField ID="hidHomeworkPassingMarks" runat="server" />
    <asp:HiddenField ID="hidAssignmentPassingMarks" runat="server" />
    <asp:HiddenField ID="hidSchoolSubjectTestId" runat="server" />
    <asp:HiddenField ID="hidMarksOrGrades" runat="server" />
    <asp:HiddenField ID="hidIsAttendanceConfigure" runat="server" />

    <script type="text/javascript" src="../Scripts/jquery-1.7.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-blink.js"></script>
    <script src="../../js/jquery.qtip-1.0.0-rc3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/Validate2.js"></script>
    <script type="text/javascript" src="../Scripts/Validations.js"></script>
    <style type="text/css">
        .class1
        {
            border: 1;
        }
    </style>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);


        function EndRequestHandler() {
            showtooltip();
            
        }

        function showtooltip() {
            $('.class1').qtip({
                content: {
                    text: false // Use each elements title attribute
                },
                style: {
                    name: 'cream',
                    color: 'black',  //'cream', // Give it some style
                    border: {
                        width: 3,
                        radius: 5
                    },
                    tip: 'topRight',
                    width: 200
                },

                position: { adjust: { x: -210, y: 0} }
            });
        }
        showtooltip();
       
    </script>
</asp:Content>
