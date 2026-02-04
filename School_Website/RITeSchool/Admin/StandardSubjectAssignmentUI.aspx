<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardSubjectAssignmentUI.aspx.cs"
    Inherits="StandardSubjectAssignmentUI" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <div class="MainBodyDiv">
        <table style="width: 97%" align="center">
            <tr align="left">
                <td>
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg"  EnableViewState="False"></asp:Label>
                </td>
            </tr>
            <tr>  
                <td style="width: 100%;" align="center">                
                    <table width="853px" align="center">
                        <tr>
                            <td align="center" style="padding-left:70px">
                                <asp:CheckBox ID="chkAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" CssClass="SubTitle" onclick="CheckAllCheckBox()" />
                            </td>
                            <td align="center" style="width:140px" runat="server" id="divToprLinkHlilight">
                                <div class="" >
                                    <asp:HyperLink  CssClass="ClsHilightTextB ToprLinkHlilight" 
                                        ID="hlnkSortOrder" runat="server" NavigateUrl="~/RITeSchool/Admin/SubjectsSortOrderPopUp.aspx"  Target="_blank">
                                        <asp:Label Style="display: inline-block; width: 150px" ID="lblSubjectSortOrder" runat="server" Text="<%$ Resources:LocalizedResources, SubjectSortOrder %>"></asp:Label>
                                    </asp:HyperLink></div>
                           </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" visible="true" runat="server" id="tdGrid">
                    <div id="GridViewScrollContainer" class="GridBorder" style="width: 635pt; overflow: scroll;">
                        <asp:GridView ID="grdStandards" UseAccessibleHeader="true" Width="100%" runat="server"
                            AutoGenerateColumns="False" PageSize="20" AllowPaging="False" CellPadding="0"
                            CellSpacing="1" ForeColor="#333333" GridLines="None" OnRowDataBound="grdStandards_RowDataBound"
                            DataKeyNames='standard_id,original_standard_id,standard_name' OnPageIndexChanging="grdStandards_PageIndexChanging"
                            OnRowCreated="grdStandards_RowCreated">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:TemplateField HeaderImageUrl="<%$ Resources:LocalizedResources, ImageStandardSubjects %>"
                                    HeaderText="Standards/Subjects">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckAllForRow" runat="server" CssClass="paddingLSML" />
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False" Width="5%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="5%" />
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
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtn" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" 
                        UseSubmitBehavior="false" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidColumnCount" runat="server"></asp:HiddenField>
         <asp:HiddenField ID="hidFirstFxFollowingErrors" runat="server"></asp:HiddenField>
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdStandards.ClientID %>"
        _clientchkAllId = "<%=this.chkAll.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        function DisableButtons() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnCancel).disabled = true
        }
        function saveChk(msg, msg1, objBtn) {
            var msgHeader = document.getElementById("<%=hidFirstFxFollowingErrors.ClientID %>").value
            if (!ChkIfAtleastOneCheckedInEachRow(document, _clientGridId) && !ChkIfAtleastOneCheckedInEachColumn(document, _clientGridId)) {
                alert(msgHeader + "\n" + msg + "\n" + msg1)
                return false
            }
            else if (ChkIfAtleastOneCheckedInEachColumn(document, _clientGridId)) {
                if (ChkIfAtleastOneCheckedInEachRow(document, _clientGridId)) {
                    document.getElementById(_clientbtnSave).disabled = true
                    document.getElementById(_clientbtnCancel).disabled = true
                    __doPostBack(objBtn.name, '')
                    return true
                }
                else {
                    alert(msgHeader + "\n" + msg)
                    return false
                } 
            }
            else if (ChkIfAtleastOneCheckedInEachRow(document, _clientGridId)) {
                if (ChkIfAtleastOneCheckedInEachColumn(document, _clientGridId)) {
                    document.getElementById(_clientbtnSave).disabled = true
                    document.getElementById(_clientbtnCancel).disabled = true
                    __doPostBack(objBtn.name, '')
                    return true
                }
                else {
                    alert(msgHeader + "\n" + msg1)
                    return false
                } 
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
                    inputs[i].checked = true
                } 
            }
            else {
                for (i = 0; i < inputs.length; i++) {
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
                oDocument.getElementById(sId).checked = Checked
            }
        }

        function CheckAll(obj, colNumber) {
       
            CheckAllInColumn(document, _clientGridId, colNumber, obj.checked)
        }
    </script>
</asp:Content>
