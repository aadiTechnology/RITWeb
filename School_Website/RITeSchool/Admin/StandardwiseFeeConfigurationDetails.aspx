<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="StandardwiseFeeConfigurationDetails.aspx.cs" Inherits="StandardwiseFeeDetails" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div id="divGridView" runat="server" visible="true">
            <table cellpadding="0" cellspacing="0" style="width: 97%" align="center">
                <tr width="100%">
                    <td>
                        <table border="0" cellpadding="0" cellspacing="3">
                            <tr>
                                <td align="left" class="ClsBorderlight" colspan="3" style="padding-left: 5px; width: 78%;
                                    font-weight: bold">
                                    <asp:Label ID="lblWhileUpdatingConfiguration" runat="server" class="LblSmlV" Text="<%$ Resources:LocalizedResources, WhileUpdatingConfiguration %>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                    <asp:Label ID="lblNote1" runat="server" class="LblNrmlB" style="font-weight:bold" Text="<%$ Resources:LocalizedResources, Note1 %>"> </asp:Label>
                                    <span class="colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                     <asp:Label ID="lblPaybleForWillNotChange" runat="server" class="LblSmlV" Text="<%$ Resources:LocalizedResources, Note1Text%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr2" runat="server">
                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                     <asp:Label ID="lblNote2" runat="server" class="LblNrmlB" style="font-weight:bold" Text="<%$ Resources:LocalizedResources, Note2%>"> </asp:Label>
                                     <span class="colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                       <asp:Label ID="lblNote2StudentHasPaidEntries" runat="server" class="LblSmlV" Text="<%$ Resources:LocalizedResources, Note2Text%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr3" runat="server">
                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                      <asp:Label ID="lblNote3" runat="server" class="LblNrmlB" style="font-weight:bold" Text="<%$ Resources:LocalizedResources, Note3%>"> </asp:Label>
                                      <span class="colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                      <asp:Label ID="lblNote3AmpuntUnpade" runat="server" class="LblSmlV" Text="<%$ Resources:LocalizedResources, Note3Text%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr4" runat="server">
                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                   <asp:Label ID="lblNote4" runat="server" class="LblNrmlB" style="font-weight:bold" Text="<%$ Resources:LocalizedResources, Note4%>"> </asp:Label>
                                     <span class="colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                      <asp:Label ID="Label2" runat="server" class="LblSmlV" Text="<%$ Resources:LocalizedResources, Note4Text%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="tr5" runat="server">
                                <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                     <asp:Label ID="lblNote5" runat="server" class="LblNrmlB" style="font-weight:bold" Text="<%$ Resources:LocalizedResources, Note5%>"> </asp:Label>
                                     <span class="colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                    <asp:Label ID="Label10" runat="server" BorderWidth="0px" CssClass="LblSmlV" 
                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, Note5Text%>"> </asp:Label>                                       
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="0" cellspacing="1" style="width: 620px">
                            <tr align="center">
                                <td>
                                    <asp:Panel ID="pnlContainer" runat="server">
                                        <table cellpadding="0" cellspacing="1" style="width: 100%">
                                            <tr>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td class="ClsBorderlight" align="right" style="width: 70px;">
                                                    <asp:Label ID="Label1" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, Standard%>" runat="server" EnableViewState="false"></asp:Label>
                                                     <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" class="ClsHilightBGB  " style="width: 70px;">
                                                    <asp:Label ID="lblStandard" runat="server" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                                </td>
                                                <td style="width: 20px;">
                                                </td>
                                                <td class="ClsBorderlight" align="right" style="width: 70px;">
                                                    <asp:Label ID="lblFeeTypeName" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, FeeType%>" runat="server"
                                                        EnableViewState="false"></asp:Label>
                                                     <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" class="ClsHilightBGB  " style="width: 87px;">
                                                    <asp:Label ID="lblFeeType" runat="server" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <table cellpadding="0" cellspacing="1" style="width: 100%;" class="GridBorder">
                                        <tr>
                                            <td align="right" class="ClsGridBG" colspan="2" style="padding-right: 0px; height: 26px">
                                                <asp:GridView DataKeyNames="Fee_SubType_Id" ID="grdFeeTypes" runat="server" Width="100%"
                                                    AutoGenerateColumns="False" AllowPaging="False" CellPadding="0" CellSpacing="1"
                                                    ForeColor="#333333" GridLines="None" OnRowDataBound="grdFeeTypes_rowDatabound"
                                                    OnPageIndexChanging="grdFeeTypes_PageIndexChanging" OnRowCreated="grdFeeTypes_RowCreated" style="border-spacing: 0px !important;">
                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                                        Font-Size="Small"></PagerStyle>
                                                    <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText%>" LastPageText="<%$ Resources:LocalizedResources, LastPageText%>" PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText%>"
                                                        FirstPageText="<%$ Resources:LocalizedResources, FirstPageText%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkFeeSubType" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="1%" HorizontalAlign="Center" />
                                                            <HeaderStyle Width="1%" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, FeeSubType%>" DataField="Fee_SubType" SortExpression="Fee_SubType">
                                                            <ItemStyle Width="80%" CssClass="ClspaddingL" />
                                                            <HeaderStyle Width="80%" CssClass="ClspaddingL" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, NewStudents%>" SortExpression="NewStudent">
                                                            <EditItemTemplate>
                                                                &nbsp;
                                                            </EditItemTemplate>
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                            <HeaderStyle Wrap="False" CssClass="ClspaddingR"  />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFeeAmountForNew" CssClass="SmlTxtBox" runat="server" MaxLength="6"
                                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    Text='<%# Eval("Fee_Amount_ForNewStudents") %>' onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                                <asp:HiddenField ID="hidFeeSubTypeForNew" runat="Server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, OldStudents%>" SortExpression="OldStudent">
                                                            <EditItemTemplate>
                                                                &nbsp;
                                                            </EditItemTemplate>
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                            <HeaderStyle Wrap="False" CssClass="ClspaddingR"/>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFeeAmountForOld" CssClass="SmlTxtBox" runat="server" MaxLength="6"
                                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    Text='<%# Eval("Fee_Amount_ForOldStudents") %>' onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                                <asp:HiddenField ID="hidFeeSubTypeForOld" runat="Server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                                    <RowStyle CssClass="ClsGridRow" />
                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                    <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="ClsGridBG">
                                            <td align="right" width="420px">
                                                <asp:Label ID="lblTotalFee" runat="server" CssClass="LblNrmlB" EnableViewState="false" Text="<%$ Resources:LocalizedResources, Total%>"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>                                            
                                            <td style="height: 26px; padding-left: 0px; padding-right:1px;" align="right">
                                                <asp:TextBox ID="txtTotalFeeNew" CssClass="ClsHilightBGB" runat="server" onblur="extractNumber(this,0,false);"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" Width=" 93px"
                                                    Height="22px" ReadOnly="True" />
                                                <asp:TextBox ID="txtTotalFeeOld" CssClass="ClsHilightBGB" runat="server" onblur="extractNumber(this,0,false);" 
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" Width=" 94px"
                                                    Height="22px" ReadOnly="True" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 509px;" colspan="2" align="center">
                                    <asp:Button ID="btnSave" CausesValidation="true" runat="server" CssClass="ClsBtn"
                                        Text="<%$ Resources:LocalizedResources, Save%>" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" OnClick="btnBack_Click"
                                        CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Cancel%>" UseSubmitBehavior="false" />
                                    <asp:HiddenField ID="hidStandardId" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hidMode" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hidIsConfig" runat="server" />
                                    <asp:HiddenField ID="hidOldStudentAmt" runat="server" />
                                    <asp:HiddenField ID="hidNewStudentAmt" runat="server" />
                                    <asp:HiddenField ID="hidTotalAmount" runat="server" />
                                    <asp:HiddenField ID="hidStartDate" runat="server" />
                                    <asp:HiddenField ID="hidEndDate" runat="server" />
                                    <asp:HiddenField ID="hidIsStudentPayFee" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div runat="server" id="divErr">
        </div>
        <tr>
            <td>
                <div id="divMain" runat="server" class="overlay" style="visibility: hidden; display: none;">
                </div>
                <div id="updtpnlPopUp" runat="server" style="visibility: hidden; display: none; position: absolute;
                    margin: 0px; padding: 0px; width: 160px; height: 125px; border-width: 0px; left: 0px;
                    top: 0px; line-height: normal; width: auto; border: solid 1px black; margin: 0px 0px 0px 5px;
                    background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
                    <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                        <div style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left;">
                            <asp:Label ID="lblDueDate" runat="server" Text="<%$ Resources:LocalizedResources, DueDate%>"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HidePopup(false);">
                            <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                        </span>
                    </div>
                    <div style="padding: 10px; text-align: left;" class="ClsLabel">
                        <table width="250px">
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="valSave" runat="server" ShowMessageBox="true" ShowSummary="false"
                                        CssClass="ClsLabel" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                    <asp:Label ID="lblSchoolleaving" runat="server" Text="<%$ Resources:LocalizedResources, DueDate%>" CssClass="LblNormal" />
                                    <span class="colonPadding">:</span>
                                    <asp:TextBox ID="txtDueDate" CssClass="SmlCombo" runat="server" MaxLength="11"></asp:TextBox>
                                    <rjs:PopCalendar ID="caltxtDueDate" runat="server" Control="txtDueDate" ShowErrorMessage="false"
                                        From-Today="True" Format="dd MMM yyyy" ShowWeekend="True" Separator="-" />
                                    <span style="color: #ff0000">*</span>
                                    <asp:CustomValidator ID="custDueDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                        ErrorMessage="<%$ Resources:LocalizedResources, DueDateShouldNotBeBlank%>" Visible="true" EnableClientScript="true"
                                        ClientValidationFunction="IsValidReturnDate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnOk" runat="server" Text="<%$ Resources:LocalizedResources, OK%>" CssClass="ClsBtn" OnClick="btnSave_Click"
                                        OnClientClick="javascript:HidePopup(true);return false;" />
                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" CausesValidation="false"
                                        OnClientClick="javascript:HidePopup(false);return false;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <br />
    </div>
    <asp:HiddenField ID="hidCultureInfo" runat="server" />
    <asp:HiddenField ID="hidAtLeastOneFeeSubTypeShouldBeSelectedForSaving" runat="server" />
    <asp:HiddenField ID="hidPleaseFixFollowingErrors" runat="server" />
    <asp:HiddenField ID="hidFeeAmountShouldNotBe0ForFollowingSubTypes" runat="server" />
    <asp:HiddenField ID="hidUpdatedFeeAmountShouldBeGreaterThanPreviousAmount" runat="server" />
    <asp:HiddenField ID="hidAreYouSureYouWantToReviseTheFeeStructure" runat="server" />
    <asp:HiddenField ID="hidAreYouSureYouWantToReturnThisBook" runat="server" />
    <asp:HiddenField ID="hidDueDateShouldNotBeBlank" runat="server" />
    <script type="text/javascript" language="javascript">

        _clientFeeTypeGridId = "<%=this.grdFeeTypes.ClientID %>"
        _clienttxtTotalFeeIdOld = "<%=this.txtTotalFeeOld.ClientID %>"
        _clienttxtTotalFeeIdNew = "<%=this.txtTotalFeeNew.ClientID %>"
        _clienthidTotalAmount = "<%=this.hidTotalAmount.ClientID %>"
        _clienthidAmountForOldStudent = "<%=this.hidOldStudentAmt.ClientID %>"
        _clienthidAmountForNewStudent = "<%=this.hidNewStudentAmt.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnBack = "<%=this.btnBack.ClientID %>"
        _clientCstValRetDate = "<%=this.custDueDate.ClientID %>"
        _clienttxtReturnDate = "<%=this.txtDueDate.ClientID %>"
        _clienthidStartDate = "<%=this.hidStartDate.ClientID %>"
        _clienthidEndDate = "<%=this.hidEndDate.ClientID %>"
        _clienthidIsStudentPayFee = "<%=this.hidIsStudentPayFee.ClientID %>"


        var bIsPaging = false;
        //This function is used to enable disable textbxees depending on checkboxes.
        function EnableDisableGridTextBox(obj, iRowIndex) {
            var sRow
            var iStart
            iStart = getStartIndex(bIsPaging)
            iRowIndex = iRowIndex + iStart
            if (iRowIndex < 10) {
                sRow = "0" + iRowIndex
            }
            else {
                sRow = iRowIndex
            }
            var sIdTotalFeeOld = _clientFeeTypeGridId + "_ctl" + sRow + "_txtFeeAmountForOld";
            var sIdTotalFeeNew = _clientFeeTypeGridId + "_ctl" + sRow + "_txtFeeAmountForNew";
            if (obj.checked) {
                document.getElementById(sIdTotalFeeOld).disabled = false
                document.getElementById(sIdTotalFeeNew).disabled = false
            }
            else {
                document.getElementById(sIdTotalFeeOld).disabled = true
                document.getElementById(sIdTotalFeeOld).value = "0"
                document.getElementById(sIdTotalFeeNew).disabled = true
                document.getElementById(sIdTotalFeeNew).value = "0"
                SetTotals()
            }
        }

        //This function is used to set the total of selected fee types.
        function SetTotals() {
            var iStart = getStartIndex(bIsPaging)
            var iCount = document.getElementById(_clientFeeTypeGridId).rows.length
            var sRow = ""
            var iTotalFeeOld = 0
            var iTotalFeeNew = 0
            for (var i = iStart; i <= iCount; i++) {
                if (i < 10) {
                    sRow = "0" + i
                }
                else {
                    sRow = i
                }
                var sIdTotalFeeOld = _clientFeeTypeGridId + "_ctl" + sRow + "_txtFeeAmountForOld";
                var sIdTotalFeeNew = _clientFeeTypeGridId + "_ctl" + sRow + "_txtFeeAmountForNew";
                var sIdChkFeeType = _clientFeeTypeGridId + "_ctl" + sRow + "_chkFeeSubType"
                if (document.getElementById(sIdTotalFeeOld).value == "") {
                    document.getElementById(sIdTotalFeeOld).value = 0
                }
                if (document.getElementById(sIdTotalFeeNew).value == "") {
                    document.getElementById(sIdTotalFeeNew).value = 0
                }
                if (document.getElementById(sIdChkFeeType).checked) {
                    iFees = parseFloat(document.getElementById(sIdTotalFeeOld).value)
                    iTotalFeeOld = iFees + iTotalFeeOld;

                    iFees1 = parseFloat(document.getElementById(sIdTotalFeeNew).value)
                    iTotalFeeNew = iFees1 + iTotalFeeNew;
                } 
            }
            document.getElementById(_clienttxtTotalFeeIdOld).value = iTotalFeeOld;
            document.getElementById(_clienttxtTotalFeeIdNew).value = iTotalFeeNew;
        }
        
        //This function is used to validate the fee before saving the configuration.
        function ValidateFee(objBtn) {
            var istart = getStartIndex(bIsPaging)
            var iCount = document.getElementById(_clientFeeTypeGridId).rows.length
            var srow = ""
            var ifeetypecnt = 0
            var bReturn = false
            var iChkCount = 0
            var completeMessage = ""

            var ActualAmtOld = document.getElementById(_clienthidAmountForOldStudent).value;
            var CurrentAmtOld = document.getElementById(_clienttxtTotalFeeIdOld).value;

            var ActualAmtNew = document.getElementById(_clienthidAmountForNewStudent).value;
            var CurrentAmtNew = document.getElementById(_clienttxtTotalFeeIdNew).value;
            serrmessage = document.getElementById("<%=hidAtLeastOneFeeSubTypeShouldBeSelectedForSaving.ClientID%>").value;
            for (var i = istart; i <= iCount; i++) {
                if (i < 10) {
                    srow = "0" + i
                }
                else {
                    srow = i
                }
                var sIdChkFeeType = _clientFeeTypeGridId + "_ctl" + srow + "_chkFeeSubType"
                if (document.getElementById(sIdChkFeeType).checked) {
                    ifeetypecnt++
                    iChkCount
                    var sIdTotalFeeOld = _clientFeeTypeGridId + "_ctl" + srow + "_txtFeeAmountForOld";
                    var sIdTotalFeeNew = _clientFeeTypeGridId + "_ctl" + srow + "_txtFeeAmountForNew"
                    
                    var sFeeName = document.getElementById(_clientFeeTypeGridId + "_ctl" + srow + "_hidFeeSubTypeForNew").value;
                    var cnt = i - istart + 1
                    var sBlankTotalErrMessage = sFeeName
                    if (parseInt(document.getElementById(sIdTotalFeeOld).value) == 0 && parseInt(document.getElementById(sIdTotalFeeNew).value) == 0) {
                        completeMessage = completeMessage + " \n  - " + sBlankTotalErrMessage
                        bReturn = true
                    } 
                }
            }
            if (document.getElementById(_clienthidIsStudentPayFee).value == 'Y') {
                if (ifeetypecnt == 0) {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID%>").value + "\n\r\n\r" + serrmessage)
                    bReturn = false
                    return false
                }
                if (bReturn) {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID%>").value + "\n\r" + document.getElementById("<%=hidFeeAmountShouldNotBe0ForFollowingSubTypes.ClientID%>").value + ":" + completeMessage)
                    return false
                }
                if ((parseInt(ActualAmtOld) > parseInt(CurrentAmtOld)) || (parseInt(ActualAmtNew) > parseInt(CurrentAmtNew))) {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID%>").value + "\n\r" + document.getElementById("<%=hidUpdatedFeeAmountShouldBeGreaterThanPreviousAmount.ClientID%>").value)
                    return false
                }
               else if (((parseInt(ActualAmtOld) < parseInt(CurrentAmtOld)) || (parseInt(ActualAmtOld) == parseInt(CurrentAmtOld))) || (parseInt(ActualAmtNew) < parseInt(CurrentAmtNew)) || (parseInt(ActualAmtNew) == parseInt(CurrentAmtNew))) {
                   if (!window.confirm(document.getElementById("<%=hidAreYouSureYouWantToReviseTheFeeStructure.ClientID%>").value)) {
                        return false 
                    }
                    else {
                        ShowPopup()
                        var ocstValRetDate = document.getElementById(_clientCstValRetDate)
                        if (ocstValRetDate != null) {
                            ocstValRetDate.innerHTML = ''
                            ocstValRetDate.errormessage = ''
                        } 
                    } 
                }
                else {
                    document.getElementById(_clientbtnSave).disabled = true
                    document.getElementById(_clientbtnBack).disabled = true
                    __doPostBack(objBtn.name, '')
                    return true
                } 
            }
            else {
                if (ifeetypecnt == 0) {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID%>").value + "\n\r\n\r" + serrmessage)
                    bReturn = false
                    return false
                }
                if (bReturn) {
                    alert(document.getElementById("<%=hidPleaseFixFollowingErrors.ClientID%>").value + "\n\r" + document.getElementById("<%=hidFeeAmountShouldNotBe0ForFollowingSubTypes.ClientID%>").value + completeMessage)
                    return false
                }
                else {
                    document.getElementById(_clientbtnSave).disabled = true
                    document.getElementById(_clientbtnBack).disabled = true
                    __doPostBack(objBtn.name, '')
                    return true
                } 
            }
        }

        function DisableButtons() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnBack).disabled = true
        }

        //This will shows a popup to revise the fee structure.
        function ShowPopup() {
            var x, y, tt_ovr_
            var cssstyleMain = $get("<%=this.divMain.ClientID %>").style
            cssstyleMain.visibility = "visible"
            cssstyleMain.display = "block"
            var cssstyle = $get("<%=this.updtpnlPopUp.ClientID %>").style
            var btnReturn = $get("<%=this.btnOk.ClientID %>")
            var now = new Date()
            $get("<%=this.txtDueDate.ClientID %>").value = now.format("dd-MMM-yyyy")
            var width = 250
            var height = 110
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010)
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
        }
        function HidePopup(oBtnName) {
            if (oBtnName) {
                var validationResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    validationResult = Page_ClientValidate("")
                }
                if (validationResult == false) {
                    return false
                } 
            }
            $get("<%=this.updtpnlPopUp.ClientID %>").style.visibility = "hidden"
            $get("<%=this.updtpnlPopUp.ClientID %>").style.display = "none"
            var dtActReturnDate = document.getElementById(_clienttxtReturnDate).value
            var cssstyleMain = $get("<%=this.divMain.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            if (oBtnName) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnBack).disabled = true
                __doPostBack(document.getElementById(_clientbtnSave).name, '')
                return true
            }
            else {
                var ocstValRetDate = document.getElementById(_clientCstValRetDate)
                if (ocstValRetDate != null) {
                    ocstValRetDate.innerHTML = ''
                    ocstValRetDate.errormessage = ''
                }
                return false
            } 
        }
        function ConfirmReturn() {
            var bResult = true
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            var sMsg = document.getElementById("<%=hidAreYouSureYouWantToReturnThisBook.ClientID%>").value;
            if (!window.confirm(sMsg)) {
                bResult = false
            }
            HidePopup()
            return bResult
        }

        function IsValidReturnDate(oSrc, args) {
            if ($get("<%=this.updtpnlPopUp.ClientID %>").style.visibility == "visible") {
                var ocstValRetDate = document.getElementById(_clientCstValRetDate)
                var dtStart = document.getElementById(_clienthidStartDate).value
                var dtEnd = document.getElementById(_clienthidEndDate).value
                var dtDuedate = document.getElementById(_clienttxtReturnDate).value
                if (document.getElementById(_clienttxtReturnDate).value == '') {
                    if (ocstValRetDate != null) {
                        ocstValRetDate.innerHTML = document.getElementById("<%=hidDueDateShouldNotBeBlank.ClientID%>").value;
                        ocstValRetDate.errormessage = document.getElementById("<%=hidDueDateShouldNotBeBlank.ClientID%>").value;
                        args.IsValid = false
                        return true
                    } 
                }
            }
            args.IsValid = true
            return false
        }       
    </script>
</asp:Content>
