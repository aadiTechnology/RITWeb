<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardWiseDivisionListUI.aspx.cs"
    MasterPageFile="../MasterPages/MasterPage.master" Inherits="StandardWiseDivisionListUI" %>

<%@ OutputCache VaryByParam="none" Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        .paddingLSML > label {
            font-family: Open Sans;
        }
    </style>
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr style="height: 0">
                <td align="center">
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="False"></asp:Label>
                    <asp:Label ID="lblError" runat="server" Text="" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr style="height: 0">
                <td align="center">
                    <asp:CheckBox ID="chkAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" CssClass="SubTitle" onclick="CheckAllCheckBox()" />
                </td>
            </tr>
            <tr>
                <td align="center" visible="true" runat="server" id="tdGrid">
                    <div id="GridViewScrollContainer" class="GridBorder" style="width: 850px; overflow: scroll">
                        <asp:GridView ID="grdStandards" Width="100%" UseAccessibleHeader="true" runat="server"
                            AutoGenerateColumns="False" Height="43px" PageSize="20" AllowPaging="false" CellPadding="0"
                            CellSpacing="1" ForeColor="#333333" GridLines="None" OnRowDataBound="grdStandandard_RowDataBound">
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText %>" LastPageText="<%$ Resources:LocalizedResources, LastPageText %>"
                             PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText %>" FirstPageText="<%$ Resources:LocalizedResources, FirstPageText %>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                            <Columns>
                                <asp:TemplateField HeaderImageUrl="<%$ Resources:LocalizedResources, HeaderURL %>"
                                    HeaderText="Standard/Division">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckAllForRow" runat="server" CssClass="paddingLSML" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="5%"
                                        CssClass="ClspaddingL" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="5%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Standard_Id" HeaderText="Standard ID" SortExpression="Standard_Id">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Original_Standard_Id" HeaderText="Original Standard ID"
                                    SortExpression="Standard_Name">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Standard_Name" HeaderText=" Standard " SortExpression="Standard_Name">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtn" OnClick="BtnSave_Click" disable-page="true" />
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidRowCount" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidFirstFxFollowingErrors" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidCultureInfo" runat="server"></asp:HiddenField>
    </div>
    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdStandards.ClientID %>"
        _clientchkAllId = "<%=this.chkAll.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        function CheckAllCheckBox() {
            var inputs = []
            var IsAllchecked = document.getElementById(_clientchkAllId).checked
            var grdViewElement = document.getElementById(_clientGridId)
            inputs = grdViewElement.getElementsByTagName("input")
            if (IsAllchecked) {
                for (i = 0; i < inputs.length; i++) {
                    inputs[i].checked = true
                }
            }
            else {
                for (i = 0; i < inputs.length; i++) {
                    inputs[i].checked = false
                }
            }
        }
        function CheckAll(obj, colNumber, iPageCnt) {
       
            CheckAllInColumn(document, _clientGridId, colNumber, obj.checked, false)
        }
        function CheckUncheckAllInRow(obj, RowNumber) {
            CheckAllInRow(document, _clientGridId, RowNumber, obj.checked)
           }

           var Page_IsValid = true;

           function saveChk(msg, msg1, objBtn, iPageCnt) {
           	Page_IsValid = true;
           	var msgHeader = document.getElementById("<%=hidFirstFxFollowingErrors.ClientID %>").value
            var bRetRow = ChkIfAtleastOneCheckedInEachRow(document, _clientGridId, iPageCnt, 1)
            var bRetCol = ChkIfAtleastOneCheckedInEachColumn(document, _clientGridId, iPageCnt, 1)
            if (!bRetRow && !bRetCol) {
            	alert(msgHeader + "\n" + msg + "\n" + msg1)
            	Page_IsValid = false;
                return false
            }
            else if (bRetCol) {
                if (bRetRow) {
                    
                    __doPostBack(objBtn.name, '')
                    return true
                }
                else {
                	alert(msgHeader + "\n" + msg)
                	Page_IsValid = false;
                    return false
                }
            }
            else if (bRetRow) {
                if (bRetCol) {
                    
                    __doPostBack(objBtn.name, '')
                    return true
                }
                else {
                	alert(msgHeader + "\n" + msg1)
                	Page_IsValid = false;
                    return false
                }
            }
        }
        function DisableButtons() {
            if (document.getElementById(_clientbtnSave)) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            }
        }

        function CheckAllInColumn(oDocument, sGridName, colNumber, Checked, iPageCnt) {

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
            for (var i = start; i < nRows; i++) {

                if (i < 10) {
                    sRow = "_ctl0";
                }
                else {
                    sRow = "_ctl";
                }
                if (colNumber < 10)
                    sCol = "ctl0";
                else
                    sCol = "ctl";

                sId = sGridName + sRow + i + "_" + sCol + colNumber ;

                if (oDocument.getElementById(sId) != null && oDocument.getElementById(sId).type=="checkbox") {
                    oDocument.getElementById(sId).checked = Checked;
                }
            }


        }

        function CheckAllInRow(oDocument, sGridName, RowNumber, Checked, iPageCnt) {         
            var bReturn = true;
            var sArr = new Array();
            var k = 0;
            var sId;
            var n = (oDocument.getElementById(sGridName).rows.length);
            var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
            nCols = nCols + (nCols / 2);
            var sRow = "";
            var sCol = "";
            var start;
            start = 2;
            RowNumber = parseInt(RowNumber) + parseInt(start);

            for (var j = 0; j < nCols; j++) {

                if (RowNumber < 10) {
                    sRow = "_ctl0";
                }
                else {
                    sRow = "_ctl";
                }

                if (j < 10)
                    sCol = "ctl0";
                else
                    sCol = "ctl";

                sId = sGridName + sRow + RowNumber + "_" + sCol + j;

                if (oDocument.getElementById(sId)) {
                    oDocument.getElementById(sId).checked = Checked;
                }

            }
        }

        function ChkIfAtleastOneCheckedInEachColumn(oDocument, sGridName, iPageCnt) {      
            var start;
            start = getStartIndex(iPageCnt);

            var bReturn = true;
            var sArr = new Array();
            var k = 0;
            var sId;
            var n = (oDocument.getElementById(sGridName).rows.length);
            var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
            var nColsForLoop = nCols + (nCols / 2);
            var nRows = n + start - 1; //(no of rows + row no for 1st row - header row)
            var sRow = "";
            var sCol = "";

            for (var j = 0; j < nColsForLoop; j++) {

                if (j < 10)
                    sCol = "ctl0";
                else
                    sCol = "ctl";
                for (var i = start; i < nRows; i++) {
                    if (i < 10) {
                        sRow = "_ctl0";
                    }
                    else {
                        sRow = "_ctl";
                    }

                    sId = sGridName + sRow + i + "_" + sCol + j;
                    if (oDocument.getElementById(sId) != null) {
                        if (oDocument.getElementById(sId).checked) {
                            sArr[k] = i;
                            k++;
                            break
                        }
                    }
                }
            }
            var chkRowColCnt = 0;
            for (var j = 0; j < nCols; j++) {
                if (oDocument.getElementById(sGridName).rows[0].cells[j].childNodes[0].type == "checkbox") {
                    chkRowColCnt++;
                }
            }

            if (sArr.length < (chkRowColCnt)) {
                bReturn = false;
            }
            else {
                bReturn = true;
            }
            return bReturn;
        }

        function ChkIfAtleastOneCheckedInEachRow(oDocument, sGridName, iPageCnt) {

            var start;
            start = getStartIndex(iPageCnt);

            var bReturn = true;
            var sArr = new Array();
            var k = 0;
            var sId;
            var n = (oDocument.getElementById(sGridName).rows.length);
            var nCols = (oDocument.getElementById(sGridName).rows[0].cells.length - 1);
            var nColsForLoop = nCols + (nCols / 2);
            var nRows = n + start - 1; //(no of rows + row no for 1st row - header row)
            var sRow = "";
            var sCol = "";

            for (var i = start; i < nRows; i++) {
                if (i < 10) {
                    sRow = "_ctl0";
                }
                else {
                    sRow = "_ctl";
                }
                for (var j = 0; j < nColsForLoop; j++) {

                    if (j < 10)
                        sCol = "ctl0";
                    else
                        sCol = "ctl";

                    sId = sGridName + sRow + i + "_" + sCol + j;

                    if (oDocument.getElementById(sId) != null) {

                        if (oDocument.getElementById(sId).checked) {
                            sArr[k] = i;
                            k++;
                            break;
                        }
                    }

                }

            }

            if (sArr.length < (nRows - start)) {
                bReturn = false;
            }
            else {
                bReturn = true;
            }
            return bReturn;
        }

    </script>
</asp:Content>
