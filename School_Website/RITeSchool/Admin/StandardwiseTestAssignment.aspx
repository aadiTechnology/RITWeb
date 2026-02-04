<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardwiseTestAssignment.aspx.cs"
    Inherits="StandardwiseTestAssignment" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <div class="MainBodyDiv">
        <table style="width: 97%" align="center">
            <tr>
                <td align="center">
                    <asp:Label ID="lblError" Style="text-align: left" runat="server" 
                        Width="100%" CssClass="LblErrorMsg" EnableViewState="False"></asp:Label>
                </td>
            </tr>
             <tr>
                <td style="width: 100%;" align="center">
                    <table width="855">
                        <tr>
                            <td align="center">
                                <asp:CheckBox ID="chkAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll%>" CssClass="SubTitle" onclick="CheckAllCheckBox()" />
                            </td>
                            <td align="center" style="width:130px" runat="server" id="divToprLinkHlilight"><div class="ToprLinkHlilight" Style="height:20px">
                                <asp:HyperLink ID="hlnkSortOrder" runat="server" CssClass="ClsHilightTextB" 
                                    NavigateUrl="~/RITeSchool/Admin/TestsSortOrderPopUp.aspx" Target="_blank" Text="<%$ Resources:LocalizedResources, ExamSortOrder%>"></asp:HyperLink></div></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                   <div id="GridViewScrollContainer" runat="server" class="GridBorder" style="width: 635pt; overflow: scroll;"  >
                        <asp:GridView ID="grdStandards" UseAccessibleHeader="true" Width="100%" runat="server" AutoGenerateColumns="False"
                             AllowPaging="False" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                            GridLines="None" DataKeyNames='standard_id,original_standard_id,standard_name'
                            OnRowDataBound="grdStandards_RowDataBound" OnPageIndexChanging="grdStandards_PageIndexChanging" OnRowCreated="grdStandards_RowCreated">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>" PreviousPageText="<%$ Resources:LocalizedResources, Previous%>"
                                FirstPageText="<%$ Resources:LocalizedResources, First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:TemplateField HeaderImageUrl="~/RITeSchool/images/GridHead_Std_Exam.gif" HeaderText="Standards/Exams">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckAllForRow" runat="server"  CssClass="paddingLSML" />
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False" Width="5%" />
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
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn" OnClick="BtnSave_Click"  disable-page="true"/>
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" 
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidSchoolId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidColumnCount" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
        <asp:HiddenField ID="hidPleaseFixFollowingErrors" runat="server" />
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdStandards.ClientID %>"
        _clientchkAllId = "<%=this.chkAll.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        function DisableButtons() {
            if (document.getElementById(_clientbtnSave)) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            }
              }

              var Page_IsValid = true;
              function saveChk(msg, msg1, objBtn) {
                  Page_IsValid = true;
                  var msgHeader = document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID %>").value;
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
                	alert(msgHeader + "\n" + msg)
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
                	alert(msgHeader + "\n" + msg1)
                	Page_IsValid = false;
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
//            colNumber = colNumber - start
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
