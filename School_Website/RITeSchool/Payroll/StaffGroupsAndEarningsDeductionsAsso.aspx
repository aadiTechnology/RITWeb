<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StaffGroupsAndEarningsDeductionsAsso.aspx.cs"
    MasterPageFile="../MasterPages/MasterPage.master" Inherits="StaffGroupsAndEarningsDeductionsAsso" %>

<%@ OutputCache VaryByParam="none" Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">            
            <tr style="height: 0">
                <td align="center">
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" ForeColor="Red" EnableViewState="False"></asp:Label>
                    <asp:Label ID="lblError" runat="server" Text="" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr style="height: 0">
                <td align="center">
                    <asp:CheckBox ID="chkAll" runat="server" Text="Select All" CssClass="SubTitle" onclick="CheckAllCheckBox()" />
                </td>
            </tr>
            <tr>
                <td align="center" visible="true" runat="server" id="tdGrid">
                    <div id="GridViewScrollContainer" class="GridBorder" runat="server" style="width: 850px; overflow: scroll">
                        <asp:UpdatePanel ID="upnl" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdAssociation" Width="100%" UseAccessibleHeader="true" runat="server"
                                    AutoGenerateColumns="false" Height="43px" PageSize="20" AllowPaging="false" CellPadding="0"
                                    CellSpacing="1" ForeColor="#333333" GridLines="None" DataKeyNames="StaffGroupsId,StaffGroupsName"
                                    OnRowDataBound="grdAssociation_RowDataBound">
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                    <Columns>
                                        <asp:TemplateField HeaderImageUrl="~/RITeSchool/images/GridHeader_StaffGroups_EarningsDeductions_Title.png"
                                            HeaderText="Category/Subcategory">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckAllForRow" runat="server"  />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="10%" 
                                                CssClass="paddingLR" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="StaffGroupsId" HeaderText="Staff Group ID">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OriginalStaffGroupsId" HeaderText="Oroginal Staff Group ID">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StaffGroupsName" HeaderText=" Staff Group ">
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
                                <asp:HiddenField ID="hidIsSaveClick" runat="server" Value="N"></asp:HiddenField>
                                <asp:HiddenField ID="hidColumnIds" runat="server" Value=""></asp:HiddenField>                                
                                <asp:HiddenField ID="hidColumnValues" runat="server" Value=""></asp:HiddenField>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr id="trNote" runat="server">
                <td align="center">
                    <table id="tblNote" runat="server" align="center" width="850px">
                        <tr>
                            <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                    CssClass="LblNrmlB"></asp:Label>
                            </td>
                            <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">                                
								<span class="LblSmlV" style="border-width:0px;">If any of the associated earning or deduction is deselected for staff group, values set to the earning or deduction will be removed from individual staff member of the respective staff group.</span>
                            </td>
                        </tr>
                    </table>
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
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="BtnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" UseSubmitBehavior="false" />                    
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidRowCount" runat="server"></asp:HiddenField>
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdAssociation.ClientID %>"
        _clientchkAllId = "<%=this.chkAll.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clienthidIsSaveClick = "<%=this.hidIsSaveClick.ClientID %>"
        _clienthidColumnIds = "<%=this.hidColumnIds.ClientID %>"
        _clienthidColumnValues = "<%=this.hidColumnValues.ClientID %>"
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
                    if (inputs[i].disabled == false)
                        inputs[i].checked = false
                } 
            } 
        }
        function CheckAll(obj, colNumber, iPageCnt, iStaffGroupId, iEarningDeductionId) {
            CheckAllInGridColumn(document, _clientGridId, colNumber, obj.checked, false, iStaffGroupId, iEarningDeductionId)
        }
        function CheckUncheckAllInRow(obj, RowNumber, iPageCnt, iStaffGroupId, iEarningDeductionId) {
            CheckAllInGridRow(document, _clientGridId, RowNumber, obj.checked, iPageCnt, iStaffGroupId, iEarningDeductionId)
        }
        function saveChk(msg, msg1, objBtn, iPageCnt) {
            var msgHeader = "Please fix following error(s):"
            var bRetRow = ChkIfAtleastOneCheckedInEachRow(document, _clientGridId, iPageCnt, 1)
            var bRetCol = ChkIfAtleastOneCheckedInEachColumn(document, _clientGridId, iPageCnt, 1)
            if (!bRetRow && !bRetCol) {
                alert(msgHeader + "\n" + msg + "\n" + msg1)
                return false
            }
            else if (bRetCol) {
                if (bRetRow) {
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
            else if (bRetRow) {
                if (bRetCol) {
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
        function DisableButtons() {
            if (document.getElementById(_clientbtnSave)) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            } 
        }
        function CheckAllInGridColumn(oDocument, sGridName, colNumber, Checked, iPageCnt, iStaffGroupId, iEarningDeductionId) {
            colNumber = colNumber + 1
            var inputs = []
            var grdViewElement = document.getElementById(_clientGridId)
            var n = grdViewElement.rows.length
            inputs = grdViewElement.rows[3].cells[2].getElementsByTagName("input")
            var IsChecked = false
            for (j = 0; j < n; j++) {
                inputs = grdViewElement.rows[j].cells[colNumber].getElementsByTagName("input")
                for (i = 0; i < inputs.length; i++) {
                    if (inputs[i].disabled == false)
                        inputs[i].checked = Checked
                } 
            }
            $get(_clienthidColumnIds).value = sGridName + "_ctl00_" + "chk_" + iStaffGroupId + "_" + iEarningDeductionId
            __doPostBack($get(_clienthidColumnIds).value.name, '')
        }
        function CheckAllInGridRow(oDocument, sGridName, RowNumber, Checked, iPageCnt, iStaffGroupId, iEarningDeductionId) {
            RowNumber = RowNumber + 1
            var inputs = []
            var grdViewElement = document.getElementById(_clientGridId)
            inputs = grdViewElement.rows[RowNumber].getElementsByTagName("input")
            var IsChecked = false
            for (i = 0; i < inputs.length; i++) {
                if (inputs[i].disabled == false)
                    inputs[i].checked = Checked
            } 
        }
        function SetValue() {
            var bResult = false
            if (ValidateAssociation()) {
                $get(_clienthidIsSaveClick).value = "Y"
                return true
            }
            return false
        }
        function ValidateAssociation() {
            var inputs = []
            var grdViewElement = document.getElementById(_clientGridId)
            inputs = grdViewElement.getElementsByTagName("input")
            var IsChecked = false
            for (i = 0; i < inputs.length; i++) {
                if (inputs[i].checked == true) {
                    IsChecked = true
                    break
                } 
            }
            if (IsChecked == false) {
                alert("At least one earnings deduction should be assigned to at least one staff group.")
                return false
            }
            return true
        }

        function ConfirmSave() {
            var bResult = true
            if (!window.confirm('If any of the associated earning or deduction is deselected for staff group, values set to the earning or deduction will be removed from individual staff member of the respective staff group. Do you want to continue?')) {
                bResult = false
            }
            return bResult
        }

        function CheckUnCheckAll(obj, colNumber, headerCheckbox) {
//            colNumber = colNumber + 1
//            var inputs = []
//            var grdViewElement = document.getElementById(_clientGridId)
//            var n = grdViewElement.rows.length
//            var isAllChecked = true;         
//            var IsChecked = false
//            for (j = 0; j < n; j++) {
//                inputs = grdViewElement.rows[j].cells[colNumber].getElementsByTagName("input")
//                for (i = 0; i < inputs.length; i++) {
//                    if (inputs[i].checked == false) {
//                        isAllChecked = false;
//                        break;
//                    }
//                }

//                if (isAllChecked == false)
//                    break;
//            }

//            if (isAllChecked) {
//                headerCheckbox.checked = true;
//            }
        }

    </script>
</asp:Content>
