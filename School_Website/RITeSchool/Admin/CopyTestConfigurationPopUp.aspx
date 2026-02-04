<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="CopyTestConfigurationPopUp.aspx.cs" Inherits="CopyTestConfigurationPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" >
        <table width="97%" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" Text="" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="ClsBtmBorderGray"  visible="true" align="center">
                    <table id="tblLegend" runat="server">
                        <tr>
                            <td>                             
                            <span class="ClsLblLgnd" style="Font-weight:bold">Legend</span>
                             </td>
                            <td align="left" colspan="1">
                                &nbsp;<asp:Label ID="Label3" runat="server" BackColor="#FFC0C0" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" EnableViewState="False" Height="20px" ReadOnly="True"
                                    Width="20px"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label>&nbsp;
                            </td>
                            <td align="left" colspan="1">
                            <span class="ClsTextNormal" style="Font-weight:bold"> Copy exam configuration from this subject.</span>
                            </td>
                            <td align="right" style="width: 5px">
                            </td>
                            <td align="left" colspan="1">
                                &nbsp;<asp:Label ID="TextBox4" runat="server"  CssClass="ClsNotAssignDark" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" EnableViewState="False" Height="20px" ReadOnly="True"
                                    Width="20px"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label></td>
                            <td align="left" colspan="1">
                               <span class="ClsTextNormal" style="Font-weight:bold"> Already configured.</span></td>
                            <td align="right" style="width: 5px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td  visible="true" style="height: 5px">
                </td>
            </tr>
             <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr >
                <td id="tdCheckAll" runat="server" align="center" visible="true">
                    <div>
                        <table>
                            <tr>
                                <td  visible="true" colspan="1" class="ClsHilightText" id="Td3">
                                    <span class="ClsLblLgnd" style="font-weight:bold">Subject :</span></td>
                                <td align="left"  visible="true" class="ClsHilightBGB" style="padding-right: 10px"
                                    id="Td4">
                                    <asp:Label ID="lblSubject" runat="server" CssClass="ClsLabel" EnableViewState="true"></asp:Label></td>
                                    <td  colspan="1" visible="true" id="Td1" class="ClsHilightText">
                                    <span class="ClsLblLgnd" style="font-weight:bold">Class :</span></td>
                                <td align="left" runat="server" visible="true" id="Td2" class="ClsHilightBGB">
                                    <asp:Label ID="lblStandardDivision" runat="server" CssClass="ClsLabel" EnableViewState="true"></asp:Label></td>
                                <td style="width: 25px">
                                    &nbsp;</td>
                                <td ><asp:CheckBox ID="chkAll" runat="server" Text="Select All" CssClass="SubTitle" onclick="SelectAll(this);" Width="108px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                </td>
            </tr>
              <tr id="trLink" runat="server">
                            <td>
                                <table id="tblLstvwAchievement" align="center" width="100%" runat="server">
                                    <tr>
                                        <td align="center" style="width: 100%">
                                                    <table align="center" width="80%">
                                                        <tr id="trPager" runat="server" width="80%">
                                                            <td align="center">
                                                                <asp:ListView ID="lstStdDiv" runat="server" DataKeyNames="SchoolWise_Test_Id,TestWise_Subject_Marks_Id,Grade_Or_Marks, Result_Consideration,IsSubmitted,IsPublished,IsExamMarkEntered,IsStudentWiseProgressReportPublished" ViewStateMode="Enabled"
                                                                   >
                                                                    <LayoutTemplate>
                                                                        <table id="tblNoticeDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                                            class="GridBorder" width="75%">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingL" style="width: 20px;">
                                                                                <%-- <input id="ChkAllDel" type="checkbox" runat="server" style="margin-left: 2px" onclick="SelectAll11(this);" />--%>
                                                                                    Consider
                                                                                </th>
                                                                                <th align="left" class="paddingL" style="width: 200px;">
                                                                                   Exam Name
                                                                                </th>
                                                                                <th style="width:60px">
                                                                                    Total Marks
                                                                                </th>
                                                                                <th align="center" style="width: 100px;">
                                                                                    Total Passing Marks
                                                                                </th>
                                                                                <th align="center" style="width: 50px;">
                                                                                   Out of Marks
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:CheckBox ID="ChkSelectAll" runat="server" Checked="true"/>
                                                                            </td>
                                                                            <td align="left">
                                                                               <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("SchoolWise_Test_Name") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                              <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject_Total_Marks") %>' ></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Passing_Total_Marks") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                               <asp:Label ID="Label4" runat="server" Text='<%# Eval("OutOfMarks") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                                              <td align="left" class="paddingL">
                                                                                <asp:CheckBox ID="ChkSelectAll" runat="server"  Checked="true" />
                                                                            </td>
                                                                            <td align="left">
                                                                               <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("SchoolWise_Test_Name") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                              <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject_Total_Marks") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Passing_Total_Marks") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                               <asp:Label ID="Label4" runat="server" Text='<%# Eval("OutOfMarks") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <tr>
                                                                            <td align="center" class="LblNoRecord">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

            <tr>
                <td align="center" style="padding-top: 10px;">
                    <div id="GridViewScrollContainer" visible="true" class="GridBorder" style="width: 900px; overflow:scroll;">
                        <asp:GridView ID="grdDivisions" UseAccessibleHeader="true" Width="100%" runat="server"
                            AutoGenerateColumns="False" PageSize="1000" CellPadding="0"
                            CellSpacing="1" ForeColor="#333333" GridLines="None" OnRowDataBound="grdDivisions_RowDataBound"
                            >
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:TemplateField HeaderImageUrl="~/RITeSchool/images/GridHeader_StdDiv_Sub_Title.gif" HeaderText="Division/Subjects">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckAllForRow" runat="server" Width="35%" />
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Standard_Id" HeaderText="Standard_Id" SortExpression="Standard_Id" />
                                <asp:BoundField DataField="Schoolwise_Standard_Division_Id" HeaderText="Standard Division ID"
                                    SortExpression="Schoolwise_Standard_Division_Id">
                                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StandardDivision" HeaderText="Standard - Division" SortExpression="StandardDivision">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                    <asp:HiddenField ID="hidSelectedStdList" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" OnClick="btnBack_Click"
                        UseSubmitBehavior="false" />
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="ClsBtn" OnClick="btnCopy_Click" />
                </td>
            </tr>
           
        </table>
        
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdDivisions.ClientID %>"
        _ClientCheckboxId = "<%=this.chkAll.ClientID %>"
        _ClientbtnCopy = "<%=this.btnCopy.ClientID %>"
        _ClientbtnBack = "<%=this.btnBack.ClientID %>"
        function ChkIfAtleastOneCheckedInTwoDGrid1(oDocument, sGridName) {
                    var start
            start = 2
            var bReturn = false
            var sArr = new Array()
            var k = 0
            var sId
            var n = (oDocument.getElementById(sGridName).rows.length)
            var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1)
            var nRows = n + start - 1
            var sRow = ""
            var sCol = ""
            nCols = nCols * 2
            for (var j = 0; j < nCols; j++) {
                if (j < 10)
                    sCol = "ctl0"
                else
                    sCol = "ctl"
                for (var i = start; i < nRows; i++) {
                    if (i < 10) {
                        sRow = "_ctl0"
                    }
                    else {
                        sRow = "_ctl"
                    }
                    sId = sGridName + sRow + i + "_" + sCol + j
                    if (oDocument.getElementById(sId) != null) {
                        if (oDocument.getElementById(sId).checked) {
                            bReturn = true
                            break
                        } 
                    } 
                } 
            }
            return bReturn
        }

        function saveChk(objBtn) {
            var bResult = true
            if (ChkIfAtleastOneCheckedInTwoDGrid1(document, _clientGridId)) {
                var ErrMsg = "If exam is configured then this action will overwrite all predefined exam configuration for selected subjects."
+ "\n" + "Are you sure you want to continue?"
                if (!window.confirm(ErrMsg)) {
                    bResult = false
                }
                else {
                    document.getElementById(_ClientbtnCopy).disabled = true
                    document.getElementById(_ClientbtnBack).disabled = true
                    __doPostBack(objBtn.name, '')
                }
                return bResult
            }
            else {
                alert("At least one subject should be selected for copy.")
            }
        }

        function CheckUncheckAllInRow(obj, RowNumber) {           
                RowNumber = RowNumber + 1
                var inputs = []
                var grdViewElement = document.getElementById(_clientGridId)
                inputs = grdViewElement.rows[RowNumber].getElementsByTagName("input")
                var IsChecked = false
                for (i = 0; i < inputs.length; i++) {
                    if (inputs[i].disabled == false)
                        inputs[i].checked = obj.checked
                }            
        }

        function CheckAllInRow1(oDocument, sGridName, RowNumber, Checked) {                  
            CheckAllInRow(oDocument, sGridName, RowNumber, Checked, 0);
        }
        function SelectAll(chk) {
            $('#<%=grdDivisions.ClientID %> input:checkbox').attr('checked', chk.checked);
        }
//        function SelectAll11(chk) {
//            $('#<%=grdDivisions.ClientID %> input:checkbox').attr('checked', chk.checked);
//        }
       
        function CheckAtleastOneCheckBoxSelected() {
            var inputs = []
            var bResult = true
            var grdViewElement = document.getElementById(_clientGridId)
            inputs = grdViewElement.getElementsByTagName("input")
            var nRows = document.getElementById(_clientGridId).rows.length
            var nCols = document.getElementById(_clientGridId).rows[0].cells.length
            nCols = nCols * 2
            alert(nCols)
            for (iRowCount = 1; nRows > iRowCount; iRowCount++) {
                for (iColCount = 1; nCols > iColCount; iColCount++) {
                    for (i = 0; i < inputs.length; i++) {
                        if (inputs[i] != null) {
                            if (inputs[i].checked == true) {
                                return true
                            } 
                        } 
                    } 
                } 
            }
            return false
        }
        function CheckAllInColumn(oDocument, sGridName, colNumber, Checked) {
            var start
            start = 2
            
            var bReturn = true
            var sArr = new Array()
            var k = 0
            var sId
            var n = (oDocument.getElementById(sGridName).rows.length)
            var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1)
            var nRows = n + 1
            var sRow = ""
            var sCol = ""
            for (var i = start; i <= nRows; i++) {
                if (i < 10) {
                    sRow = "_ctl0"
                }
                else {
                    sRow = "_ctl"
                }
                if (colNumber < 10)
                    sCol = "ctl0"
                else
                    sCol = "ctl"
                sId = sGridName + sRow + i + "_" + sCol + colNumber
                if (oDocument.getElementById(sId) != null)
                    oDocument.getElementById(sId).checked = Checked
                        } 
        }
        function CheckAll(obj, colNumber) {
            CheckAllInColumn(document, _clientGridId, colNumber, obj.checked)
        }
    </script>
</asp:Content>
