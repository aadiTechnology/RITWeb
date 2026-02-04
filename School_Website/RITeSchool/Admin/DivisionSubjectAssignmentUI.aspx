<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="DivisionSubjectAssignmentUI.aspx.cs" Inherits="DivisionSubjectAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <div class="MainBodyDiv">
        <table width="97%" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" Text=""  EnableViewState="false"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 100%">
                    <asp:CheckBox ID="chkAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" CssClass="SubTitle" onclick="CheckAllCheckBox()" /></td>
            </tr>
            <tr>
                <td align="center" visible="true" runat="server" id="tdGrid">
                    <div id="GridViewScrollContainer" visible="true" class="GridBorder" style="width: 850px; overflow: scroll;">
                        <asp:GridView ID="grdDivisions" Width="100%" runat="server" AutoGenerateColumns="False"
                            AllowPaging="false" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                            OnRowDataBound="grdDivisions_RowDataBound">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:TemplateField HeaderImageUrl="<%$ Resources:LocalizedResources, ImageDivisionSubjects %>" HeaderText="Division/Subjects">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckAllForRow" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false"  />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false"  />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Standard_Id" HeaderText="Standard_Id" SortExpression="Standard_Id" />
                                <asp:BoundField DataField="Schoolwise_Standard_Division_Id" HeaderText="Standard Division ID"
                                    SortExpression="Schoolwise_Standard_Division_Id">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StandardDivision" HeaderText="Standard - Division" SortExpression="StandardDivision">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
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
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" OnClick="btnCancel_Click"
                        UseSubmitBehavior="False" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidTeacherName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidSchoolId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidColumnCount" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidSelectedValue" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidFirstFxFollowingErrors" runat="server"></asp:HiddenField>
         <asp:HiddenField ID="hidCultureInfo" runat="server" />
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdDivisions.ClientID %>"
        _clientchkAllId = "<%=this.chkAll.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"

        var Page_IsValid = true;
        function saveChk(msg, msg1, objBtn) {
        	 Page_IsValid = true;
        	 var msgHeader = document.getElementById("<%=hidFirstFxFollowingErrors.ClientID %>").value
            if (!ChkIfAtleastOneCheckedInEachRow(document, _clientGridId) && !ChkIfAtleastOneCheckedInEachColumn(document, _clientGridId)) {
            	alert(msgHeader + "\n" + msg + "\n" + msg1)
            	 Page_IsValid = false;
                return false
            }
            else if (ChkIfAtleastOneCheckedInEachColumn(document, _clientGridId)) {
                if (ChkIfAtleastOneCheckedInEachRow(document, _clientGridId)) {
                    
                    __doPostBack(objBtn.name, '')
                    return true
                }
                else {
                	alert(msgHeader + "\n" + msg1)
                	Page_IsValid = false;
                    return false
                } 
            }
            else if (ChkIfAtleastOneCheckedInEachRow(document, _clientGridId)) {
                if (ChkIfAtleastOneCheckedInEachColumn(document, _clientGridId)) {
                   
                    __doPostBack(objBtn.name, '')
                    return true
                }
                else {
                	alert(msgHeader + "\n" + msg)
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
        function CheckUncheckAllInRow(obj, RowNumber) {
            CheckAllInRow(document, _clientGridId, RowNumber, obj.checked)
        }
        function CheckAllCheckBox() {
            var inputs = []
            var IsAllchecked = document.getElementById(_clientchkAllId).checked
            var grdViewElement = document.getElementById(_clientGridId)
            inputs = grdViewElement.getElementsByTagName("input")
            if (IsAllchecked) {
                for (i = 0; i < inputs.length; i++) {
                    if (inputs[i] != null)
                        inputs[i].checked = true
                } 
            }
            else {
                for (i = 0; i < inputs.length; i++) {
                    if (inputs[i] != null)
                        inputs[i].checked = false
                } 
            } 
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
            for (var i = start ; i < nRows; i++) {
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
