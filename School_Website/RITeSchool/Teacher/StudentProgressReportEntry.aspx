<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentProgressReportEntry.aspx.cs" Inherits="StudentProgressReportEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel ID="upd" runat="server">
            <ContentTemplate>
                <table width="97%" align="center">
                    <tr id="trOldAcadmicYr" runat="server" visible="false">
                        <td align="right">
                            <asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen" Height="15px" ID="hlnkOldAcademicRecord"
                                NavigateUrl="#" runat="server" Target="_blank">Old Academic Records</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSuccessfulMsg" Style="text-align: center" runat="server" ForeColor="blue"
                                Width="100%" CssClass="ClsConfigText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnlHeader" runat="server" Visible="true" Style="overflow: auto; width: 842px;
                                left: 0px;">
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trPrecondition" runat="server" visible="false">
                        <td>
                            <div runat="server" id="divErr">
                                <table class="LblNoRecord" width="100%">
                                    <tr>
                                        <td class="ClsConfigText">
                                            <asp:Label ID="lblnotyetPublish" runat="server" Text="Required details for the pre-primary progress report are not configured"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblModuleName" runat="server" CssClass="HeadTxtBWOPadding" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" visible="true" runat="server" id="tdGrid">
                            <div id="GridViewScrollContainer" class="GridBorder" style="width: 842px; overflow: scroll"
                                runat="server">
                                <asp:GridView ID="grdWithOutSubjects" Width="100%" UseAccessibleHeader="true" runat="server"
                                    Height="43px" AllowPaging="false" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                    GridLines="None" OnRowDataBound="grdWithOutSubjects_RowDataBound" EnableViewState="false">
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                    <Columns>
                                    </Columns>
                                    <RowStyle CssClass="Lbl10pt ConfigHeadBG" Font-Size="9pt" Font-Bold="false" />
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                    </PagerStyle>
                                    <HeaderStyle Height="40px" CssClass="ClsProgressGridTestHeader" Font-Size="10pt" />
                                    <AlternatingRowStyle CssClass="Lbl10pt ConfigHeadBG" Font-Size="9pt" Font-Bold="false"
                                        BackColor="#eef1ea" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblModuleNameWithSubject" runat="server" CssClass="HeadTxtBWOPadding"
                                Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" visible="true" runat="server" id="td1">
                            <div id="GridViewSubjects" class="GridBorder" style="width: 842px; overflow: scroll"
                                runat="server">
                                <asp:GridView ID="grdWithSubjects" Width="100%" UseAccessibleHeader="true" runat="server"
                                    Height="43px" AllowPaging="false" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                    GridLines="None" OnRowDataBound="grdWithSubjects_RowDataBound" EnableViewState="false">
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                    <Columns>
                                    </Columns>
                                    <RowStyle CssClass="Lbl10pt ConfigHeadBG" Font-Size="9pt" Font-Bold="false" />
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                    </PagerStyle>
                                    <HeaderStyle Height="40px" CssClass="ClsProgressGridTestHeader" Font-Size="10pt" />
                                    <AlternatingRowStyle CssClass="Lbl10pt ConfigHeadBG" Font-Size="9pt" Font-Bold="false"
                                        BackColor="#eef1ea" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="false"
                                ShowSummary="true" HeaderText="Please fix following error(s):" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblRemarks" runat="server" CssClass="HeadTxtBWOPadding" Text="Remarks"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" runat="server" id="tdRemarks" visible="false">
                            <div id="Div1" class="GridBorder" style="width: 642px; overflow: scroll" runat="server">
                                <asp:GridView ID="grdViewRemarks" Width="100%" UseAccessibleHeader="true" runat="server"
                                    Height="43px" AllowPaging="false" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                    GridLines="None" EnableViewState="false" AutoGenerateColumns="false" DataKeyNames="MonthId,Progress_Entry_Id,IsSubmitted,IsPublished,Comment"
                                    OnRowDataBound="grdViewRemarks_RowDataBound">
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Header" DataField="Header">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="160px" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Comments">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtremarks" runat="server" MaxLength="200" TextMode="MultiLine"
                                                    Width="400px" Height="80px"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="Reg_Expr_ValidContent" runat="server" Display="None"
                                                    ControlToValidate="txtremarks" ErrorMessage="Remarks should be of length less than 500."
                                                    ValidationExpression="^[\s\S]{0,500}$" CssClass="ClsLabel" Visible="true"> </asp:RegularExpressionValidator>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="Lbl10pt ConfigHeadBG" Font-Size="9pt" Font-Bold="false" />
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                    </PagerStyle>
                                    <HeaderStyle Height="40px" CssClass="ClsProgressGridTestHeader" Font-Size="10pt" />
                                    <AlternatingRowStyle CssClass="Lbl10pt ConfigHeadBG" Font-Size="9pt" Font-Bold="false"
                                        BackColor="#eef1ea" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" UseSubmitBehavior="false"
                                OnClick="btnBack_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="updatpnl2" runat="server">
                                <ContentTemplate>
                                    <table width="50%">
                                        <tr>
                                            <td id="tdlistview" runat="server">
                                                <asp:ListView ID="lstvwMonths" runat="server" Visible="false" DataKeyNames="IsPublished,PrePrimaryProgressReportMonthId"
                                                    OnItemDataBound="lstvwMonths_ItemDataBound" OnDataBound="lstvwMonths_DataBound">
                                                    <LayoutTemplate>
                                                        <table align="center" width="50%" runat="server" id="tblMonths" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsProgressGridTestHeader">
                                                                <th align="center" width="10%">
                                                                    Select
                                                                </th>
                                                                <th width="40%">
                                                                    Months
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="Lbl10pt ConfigHeadBG">
                                                            <td align="center">
                                                                <asp:CheckBox ID="chkMonth" runat="server"></asp:CheckBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("MonthAbbreviation") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr2" runat="server" class="Lbl10pt ConfigHeadBG" style="background-color: #eef1ea">
                                                            <td align="center">
                                                                <asp:CheckBox ID="chkMonth" runat="server"></asp:CheckBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("MonthAbbreviation") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr>
                                                            <td>
                                                                No Record Found.
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table id="tblNote" runat="server" width="600px" visible="false">
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                                            <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                                CssClass="LblNrmlB"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px; ">
                                                            <asp:Label ID="lblNote" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="To publish result, first select months and click on 'Publish' button."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                                            <asp:Label ID="Label1" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note2 :"
                                                                CssClass="LblNrmlB"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px; ">
                                                            <asp:Label ID="Label2" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="To unpublish result, first remove months selection and click on 'Publish' button."></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBackDown" runat="server" Text="Back" CssClass="ClsBtn" UseSubmitBehavior="false"
                                OnClick="btnBack_Click" Visible="false" />
                            <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtn" Visible="false"
                                OnClick="btnPublish_Click" />
                            <asp:Button ID="btnView" runat="server" Text="View Progress Report" CssClass="ClsBtnLrg"
                                Visible="false" OnClick="btnView_Click" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidSubName" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hidRowSpan" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hidBackUrl" runat="server" />
                <asp:HiddenField ID="hidFrom" runat="server" />
                <asp:HiddenField ID="hidIsStudentWiseProgressReport" runat="server" Value="N" />
                <asp:HiddenField ID="hidRowNo" runat="server" Value="-1"></asp:HiddenField>
                <asp:HiddenField ID="hidRowCount" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdWithOutSubjects.ClientID %>"
        _clientgrdWithSubjects = "<%=this.grdWithSubjects.ClientID %>"
        _clientlstvwMonths = "<%=this.lstvwMonths.ClientID %>"
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"

        function SelectAll(obj, colNumber, iPageCnt) {

            SelectAllInColumn(document, _clientGridId, colNumber, obj, false)
        }
        function SelectAllWithSubjects(obj, colNumber, iPageCnt) {

            SelectAllInColumn(document, _clientgrdWithSubjects, colNumber, obj, false)
        }

        function SelectAllInColumn(oDocument, sGridName, colNumber, obj, iPageCnt) {

            var value = obj.value;
            var start;
            start = getStartIndex(iPageCnt);
            var bReturn = true;
            var sArr = new Array();
            var k = 0;
            var sId;
            var n = (oDocument.getElementById(sGridName).rows.length);
            var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
            var nRows = n + start - 1; //(no of rows + row no for 1st row - header row)
            var sRow = "";
            var sCol = "";
            var s = obj.id.replace('ctl00_MainBody_grdWithOutSubjects_ctl01_cmb', '');
            s = s.replace('ctl00_MainBody_grdWithSubjects_ctl01_cmb', '');
            for (var j = 1; j < n; j++) {
                if (nCols == oDocument.getElementById(sGridName).rows[j].cells.length - 1) {
                    k = 0;
                }
                inputs = (oDocument.getElementById(sGridName)).rows[j].cells[colNumber - k].getElementsByTagName("select")
                for (i = 0; i < inputs.length; i++) {
                    if ((inputs[i].id.match(s))) {
                        inputs[i].value = value;
                        k = 1;
                    }
                }
            }
        }
        function ShowOldProgressReports(sQryStr) {
            _sClienthlnkOldAcademicRecord = "<%=this.trOldAcadmicYr.ClientID %>"
            if ((document.getElementById(_sClienthlnkOldAcademicRecord) == null) || (document.getElementById(_sClienthlnkOldAcademicRecord) == "") || (document.getElementById(_sClienthlnkOldAcademicRecord).disabled))
                return false
            window.open(sQryStr, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1200,height=1000').focus();
            return false
        }

        function ConfirmAction(btn) {
            bAlert = false;
            var iCount = 0;
            var sConfirmMsg = "";
            var iRowCount = document.getElementById(_clienthidRowCount).value
            if (btn.value == "Publish") {
                for (var RowNumber = 0; RowNumber < iRowCount; RowNumber++) {
                    chk = document.getElementById(_clientlstvwMonths + "_ctrl" + RowNumber + "_chkMonth");
                    if (chk.checked == true) {
                        bAlert = true;
                        break;
                    }
                }
                sConfirmMsg = "Once you publish the result it will be visible to parents/student. Are you sure you want to continue?";
                if (!bAlert) {
                    if (!window.alert('At least one month should be selected.'))
                        return false;
                }
            }
            else if (btn.value == "View Progress Report")
                sConfirmMsg = "By this operation unsaved data will be lost. Are you sure you want to continue?";

            if (window.confirm(sConfirmMsg))
                bResult = true;
            else
                bResult = false;
            return bResult;
        }
        
    </script>

</asp:Content>
