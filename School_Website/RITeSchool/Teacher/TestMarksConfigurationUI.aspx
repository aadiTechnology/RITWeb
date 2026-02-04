<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="TestMarksConfigurationUI.aspx.cs" Inherits="TestMarksConfigurationUI"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%">
            <tr id="trPrecondition" runat="server" visible="false">
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
           
            <tr>
                <td align="center">
                    <table style="width: 100%;">
                        <tr>
                            <td align="center">
                                <table id="LegendTable" runat="server" align="center" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td align="left" colspan="1" rowspan="3" style="padding-right: 5px">
                                            <asp:Label ID="lblLegend" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                EnableViewState="false" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                        </td>
                                        <td rowspan="2">
                                            <asp:Label ID="TextBox4" runat="server" CssClass="clsLegendLbl" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="1px" EnableViewState="False" Height="20px" ReadOnly="True" Width="20px"><img alt="spacer" src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" style="padding-left: 5px">
                                            <asp:Label ID="Label3" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, MsgTestMarksConfiguration %>"></asp:Label>
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
                                    </tr>
                                    <tr>
                                        <td rowspan="1">
                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/RITeSchool/images/icoGrid_SubmitExamMarks.gif" />
                                        </td>
                                        <td align="left" colspan="1" style="padding-left: 5px">
                                            <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, MsgSubmitExamMarks %>"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:ImageButton ID="imgbtnunpublish" runat="server" CausesValidation="false" CommandName="UnpublishExamMarks"
                                              ImageUrl="../images/unsubmit.jpg" />                                           
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label10" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                             Font-Bold="true" Text="Unsubmit Exam Marks"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            &nbsp;
                                            <asp:Image ID="Image3" CssClass="img-align-unset" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label9" runat="server" CssClass="ClsTextNormal" EnableViewState="False"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryCompleted %>"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Image ID="Image5" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/GridIcon_ExamDateNC.gif"
                                                Visible="false" />
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label1" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, ExamDatesNotConfigured %>" Visible="false"> </asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlFields" runat="server" Width="100%">
                                    <table  width="100%">
                                        <tr>
                                            <td align="center" colspan="2" valign="bottom">
                                                <table id="Table1" runat="server">
                                                    <tr>
                                                        <td class="ClsBorderlight" id="tdTeacher" runat="server">
                                                            <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, SelectSubjectTeacher %>" EnableViewState="false"></asp:Label>
                                                                <span class="ClsLblLgnd"> :</span>
                                                            <%--<span class="ClsLblLgnd" style="font-weight:bold">Select Subject Teacher :</span>--%>
                                                        </td>
                                                        <td align="left" style="padding-right: 15px;">
                                                        
                                                            <asp:DropDownList ID="cmbTeachers" AutoPostBack="true"  runat="server" CssClass="LrgCombo"
                                                                OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="ClsBorderlight"  runat="server">
                                                            <asp:Label ID="Label2" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, Class %>" EnableViewState="false"></asp:Label>
                                                                <span class="ClsLblLgnd"> :</span>
                                                            <%--<span class="ClsLblLgnd" style="font-weight:bold">Select Subject Teacher :</span>--%>
                                                        </td>
                                                        <td align="left">
                                                        <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbClass" AutoPostBack="true"  runat="server" 
                                                                CssClass="LrgCombo" onselectedindexchanged="cmbClass_SelectedIndexChanged" >
                                                            </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                            <asp:AsyncPostBackTrigger  ControlID="cmbClass"  />
                                                            </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight">
                                                            <%--<asp:Label ID="Label2" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                            Text="Select Exam :" EnableViewState="false"></asp:Label>--%>
                                                            <span class="ClsLblLgnd" style="font-weight: bold"><asp:Label runat="server" ID="lblselectExamText" Text="<%$ Resources:LocalizedResources, SelectExam %>"></asp:Label>
                                                             :</span>&nbsp;
                                                        </td>
                                                        <td align="left" style="padding-right: 15px;">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="cmbExams" runat="server" CssClass="LrgCombo" 
                                                                       AutoPostBack="true" onselectedindexchanged="cmbExams_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger  ControlID="cmbClass"  />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                       
                        <tr>
                            <td align="center">
                          
                                <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                    ID="uPnl">
                                    <ContentTemplate>
                                        <div id="divGridView" runat="server" visible="true" align="center">
                                        <div  style="text-align:left; width:70%; margin-top:10px;" >
                                        <span id ="spnMySubject" style="color:#066;font-weight:700;font-size:9pt;padding-left:2px;"  runat ="server">My Subject(s):-</span>
                                        </div>
                                            <asp:GridView CssClass="GridBorder" ID="grdSubjects" runat="server" AutoGenerateColumns="False"
                                                Height="100%" PageSize="1100" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                GridLines="None" DataKeyNames="Standard_Id,Division_Id,Standard_Division_Id,Subject_Id,Is_Submitted,Status,Is_MonthConfig,AllowPartialSubmit"
                                                OnRowCreated="grdSubjects_RowDatabound" Width="70%" EnableViewState="false">
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                </PagerStyle>
                                                <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                    FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Class %>" SortExpression="StandardDivision" DataField="StandardDivision">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Subject %>" SortExpression="Subject_Name" DataField="Subject_Name">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:HyperLinkField HeaderText="<%$ Resources:LocalizedResources, Edit %>">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="CursorHand" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="10%"/>
                                                    </asp:HyperLinkField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Submit %>">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width ="50%"/>
                                                        <ItemTemplate>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="ClsGridRow" />
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" VerticalAlign="Middle"
                                                    Width="100%" />
                                            </asp:GridView>
                                            <asp:HiddenField ID="hidAlert" runat="server" />
                                            <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
                                            <asp:HiddenField ID="hidValRegenerateMsg" runat="server" />
                                            <asp:HiddenField ID="hidValTestMarksConfiguration" runat="server" />
                                            <asp:HiddenField ID="hidRollNos" runat="server" />
                                             <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                             <asp:HiddenField ID="hidOldExamId" runat="server" Value="0" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbExams" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                        <td></td>
                        </tr>
                         <tr id = "trMyClass" runat ="server"> 
                            <td align="center">
                                <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                    ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <div id="divClassSubjectsTitle" runat="server" visible="true" align="center">
                                         <div  style="text-align:left; width:70%; margin-top:10px;" runat="server" >
                                        <span id ="spnMyClassSubjects" runat ="server" style="color:#066;font-weight:700;font-size:9pt;padding-left:2px;" visible ="false">My Class Subject(s):-</span>
                                        </div>
                                            <asp:GridView CssClass="GridBorder" ID="grdMyClassSubjects" runat="server" AutoGenerateColumns="False"
                                                Height="100%" PageSize="1100" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                GridLines="None" DataKeyNames="Standard_Id,Division_Id,Standard_Division_Id,Subject_Id,Is_Submitted,Status,Is_MonthConfig,AllowPartialSubmit"
                                                OnRowCreated="grdMyClassSubjects_RowDatabound" Width="70%" EnableViewState="false">
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                </PagerStyle>
                                                <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                    FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                <Columns>
                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Class %>" SortExpression="StandardDivision" DataField="StandardDivision">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Subject %>"  SortExpression="Subject_Name" DataField="Subject_Name">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:HyperLinkField HeaderText="<%$ Resources:LocalizedResources, Edit %>">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="CursorHand" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                    </asp:HyperLinkField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Submit %>">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50%" />
                                                        <ItemTemplate>
                                                        </ItemTemplate>                                                       
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="ClsGridRow" />
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" VerticalAlign="Middle"
                                                    Width="100%" />
                                            </asp:GridView>

                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                         <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbExams" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientGridId = "<%=grdSubjects.ClientID %>";
        _clienthidAlert = "<%=hidAlert.ClientID %>";
        
        function saveChk(msg) {
            if (ChkIfAtleastOneCheckedInEachRow(document, _clientGridId))
                return true;
            else {
                alert(msg);
                return false;
            }
        }

        function SubmitMarksToClassTeacher(StandardDivisionId, SubjectId, TestID, AcademicYearId, SchoolId, InsertedById, sIncompleteRollNos, IsSubmitted) {

            var bResult = true;
            var sAlert = document.getElementById(_clienthidAlert).value

            if (SubjectId == -1) {
                if (sAlert.length > 0) {
                    if (window.confirm(document.getElementById(_clienthidAlert).value + "\n" + document.getElementById("<%=this.hidValRegenerateMsg.ClientID %>").value))
                        bResult = true;
                    else
                        bResult = false;
                }
            }
            if (sIncompleteRollNos.length > 0) {
                var sIncompleteAlert = "Marks not entered for\n";
                sIncompleteAlert = sIncompleteAlert + document.getElementById("<%=this.hidRollNos.ClientID %>").value + " : " + sIncompleteRollNos + " ";
                if (sIncompleteAlert.length > 0) {
                    if (window.confirm(sIncompleteAlert + "\n" + document.getElementById("<%=this.hidValRegenerateMsg.ClientID %>").value))
                        bResult = true;
                    else
                        bResult = false;
                }
            }
            if (bResult) {
                if (IsSubmitted == 'N' || ConfirmSubmitAction()) {
                    var parentTag = document.getElementById('ctl00_updateProgressMaster');
                    parentTag.style.display = 'block';
                    parentTag.style.visibility = 'visible';
                    var xmlHttpObj = null;
                    xmlHttpObj = CreateHTTPReqObj();
                    if (xmlHttpObj) {

                        var url = "../Ajax.ashx?SchoolId=" + SchoolId + "&StandardDivisionId=" + StandardDivisionId
                            + "&SubjectId=" + SubjectId + "&AcademicYearId=" + AcademicYearId
                            + "&TestId=" + TestID + "&InsertedById=" + InsertedById + "&task=SubmitMarksToClassTeacher" + "&IsSubmitted=" + IsSubmitted;
                        xmlHttpObj.open("POST", url, true);
                        xmlHttpObj.onreadystatechange = function () {
                            if (xmlHttpObj.readyState == 4) {
                                if (xmlHttpObj.status == 200) {
                                    var optionText = xmlHttpObj.responseText;
                                    document.forms[0].submit();
                                    parentTag.style.display = 'none';
                                    parentTag.style.visibility = 'hidden';
                                }
                            }
                        }
                        xmlHttpObj.send(null);
                    }
                    else
                        alert('Sad!!');
                }
            }
        }


        function ConfirmAction(iPageCountStandard, iPageCountDivision, sActionName) {
            var bResult = false;
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxDelete', sActionName, 'false', iPageCountDivision, 'true'))
                bResult = true;
            else
                bResult = false;
            return bResult;
        }

        function ConfirmSubmitAction() {
            var bResult = false;
            if (window.confirm(document.getElementById("<%=this.hidValTestMarksConfiguration.ClientID %>").value))
                bResult = true;
            else
                bResult = false;
            return bResult;
        }
        </script>
</asp:Content>
