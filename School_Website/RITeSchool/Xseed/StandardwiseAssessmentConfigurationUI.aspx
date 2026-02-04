<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StandardwiseAssessmentConfigurationUI.aspx.cs"
    Inherits="StandardwiseAssessmentConfigurationUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 20px">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="right" style="color: #ff3333" valign="top">
                                        <span class="ClsMdtStar">* Mandatory Fields </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:ValidationSummary ID="valSumErrorMsg" CssClass="LblErrorMsg" runat="server"
                                            ShowMessageBox="False" ShowSummary="True" ValidationGroup="Save" />
                                            <asp:CustomValidator ID="cstValidDate" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="End Date should be greater than Start Date." SetFocusOnError="True"
                                            ClientValidationFunction="StartAndEndDateValidation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstConfirmAction" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                            ClientValidationFunction="ConfirmAction"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstDate" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                            ClientValidationFunction="DateValidations"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstDateRange" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                            ClientValidationFunction="DateRangeValidations"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstEndDate" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                            ClientValidationFunction="EndDateValidations"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstvalAssessmentSortOrder" runat="server" ValidationGroup="Save"
                                            ClientValidationFunction="ValidateAssessmentSortOrder" SetFocusOnError="True"
                                            Display="None" ErrorMessage="Selected Assessment sort order should not be blank."></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstvalDuplicateSortOrder" runat="server" ClientValidationFunction="DuplicateSortOrder"
                                            SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="You have entered duplicate value for selected Assessment sort order."></asp:CustomValidator> 
                                            <asp:CustomValidator ID="cstStartDate" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="Start Date should not be blank." SetFocusOnError="True"
                                            ClientValidationFunction="EmptyStartDate"></asp:CustomValidator> 
                                            <asp:CustomValidator ID="cstEndDate1" Display="None" runat="server" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="End Date should not be blank." SetFocusOnError="True"
                                            ClientValidationFunction="EmptyEndDate"></asp:CustomValidator>                                           
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1" class="ClsTextNormal" align="center">
                                        <asp:Label ID="lblUpdateSuccess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tblMainAssessmentTable" border="0" cellpadding="1" cellspacing="1" runat="server"
                        width="80%">
                        <tr align="center">
                            <td align="right" style="width: 50%">
                                <span class="LblNormal">Standard :</span>
                            </td>
                            <td align="left" style="width: 50%">
                                <asp:DropDownList ID="cmbStandard" runat="server" CssClass="SmlCombo" AutoPostBack="true"
                                    OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:UpdatePanel runat="server" ID="uPnl">
                                    <ContentTemplate>
                                        <table id="tbl" align="center" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <div id="divContainer" class="GridBorder" visible="false" runat="server" style="height: 400px;
                                                        overflow: auto">
                                                        <asp:ListView ID="lstvwStandardwiseAssessmentConfig" runat="server" DataKeyNames="StandardwiseAssessmentId, AssessmentId, IsDeleted, SortOrder"
                                                            OnItemDataBound="lstvwStandardwiseAssessmentConfig_ItemDataBound" OnDataBound="lstvwStandardwiseAssessmentConfig_DataBound">
                                                            <LayoutTemplate>
                                                                <table align="center" width="100%" runat="server" id="tblAssessmentList" style="color: #333333"
                                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th align="center" style="width: 8%">
                                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                                        </th>
                                                                        <th align="center" style="width: 8%">
                                                                            Sr. No.
                                                                        </th>
                                                                        <th align="left" style="width: 30%; padding-left: 10px">
                                                                            Assessment Name
                                                                        </th>
                                                                        <th align="center" style="width: 14%">
                                                                            Start Date
                                                                        </th>
                                                                        <th align="center" style="width: 14%">
                                                                            End Date
                                                                        </th>
                                                                        <th align="center" style="width: 12%">
                                                                            Is Final?
                                                                        </th>
                                                                        <th align="center" style="width: 30%">
                                                                            Sort Order
                                                                        </th>
                                                                    </tr>
                                                                    <tr runat="server" id="itemPlaceholder">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblRowNo" runat="server" />
                                                                    </td>
                                                                    <td align="left" style="padding-left: 8px">
                                                                        <asp:Label ID="lblAssessmentName" runat="server" Text='<%#Eval("AssessmentName")%>'
                                                                            CssClass="LblNormal"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                            <asp:TextBox ID="txtcalstartDate" CssClass="SmlCombo" runat="server" Text='<%#Eval("StartDate","{0:dd-MMM-yyyy}")%>' AutoPostBack="True"></asp:TextBox>
                                                                              <rjs:PopCalendar ID="calStartDate" runat="server" Control="txtcalstartDate" ShowErrorMessage="false" InvalidDateMessage="Please select valid start date."
                                                                               Format="dd MMM yyyy" ShowWeekend="True" />
                                                                    </td>
                                                                    <td align="center">
                                                                    <asp:TextBox ID="txtcalEndDate" CssClass="SmlCombo" runat="server" Text='<%#Eval("EndDate","{0:dd-MMM-yyyy}")%>' AutoPostBack="True"></asp:TextBox>
                                                                              <rjs:PopCalendar ID="calEndDate" runat="server" Control="txtcalEndDate" ShowErrorMessage="false" InvalidDateMessage="Please select valid end date."
                                                                               Format="dd MMM yyyy" ShowWeekend="True" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <span id="spnOptIsFinal" runat="server">
                                                                            <asp:RadioButton ID="optIsFinal" runat="server" /></span>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:DropDownList ID="cmbSortOrder" runat="server" Width="87%">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblRowNo" runat="server" />
                                                                    </td>
                                                                    <td align="left" style="padding-left: 8px">
                                                                        <asp:Label ID="lblAssessmentName" runat="server" Text='<%#Eval("AssessmentName")%>'
                                                                            CssClass="LblNormal"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:TextBox ID="txtcalstartDate" CssClass="SmlCombo" runat="server" Text='<%#Eval("StartDate","{0:dd-MMM-yyyy}")%>' AutoPostBack="True"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="calStartDate" runat="server" Control="txtcalstartDate" ShowErrorMessage="false" InvalidDateMessage="Please select valid start date."
                                                                        Format="dd MMM yyyy" ShowWeekend="True" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:TextBox ID="txtcalEndDate" CssClass="SmlCombo" runat="server" Text='<%#Eval("EndDate","{0:dd-MMM-yyyy}")%>' AutoPostBack="True"></asp:TextBox>
                                                                              <rjs:PopCalendar ID="calEndDate" runat="server" Control="txtcalEndDate" ShowErrorMessage="false" InvalidDateMessage="Please select valid end date."
                                                                               Format="dd MMM yyyy" ShowWeekend="True" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <div id="divRadio" runat="server">
                                                                        </div>
                                                                        <span id="spnOptIsFinal" runat="server">
                                                                            <asp:RadioButton ID="optIsFinal" runat="server" /></span>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:DropDownList ID="cmbSortOrder" runat="server" Width="87%">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>                                                            
                                                        </asp:ListView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hidRowCount" runat="server" />
                                        <asp:HiddenField ID="hidStdStartDate" runat="server" />
                                        <asp:HiddenField ID="hidStdEndDate" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trNoRecordMsg" runat="server" visible="false">
                            <td style="height: 10px;" align="center" colspan="2">
                                <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                    Text="No Record Found." EnableViewState="False" Width="70%"></asp:Label>
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
                <td align="center" colspan="2">
                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" Visible="false" disable-page="true"
                        ValidationGroup="Save" BorderWidth="1px" CausesValidation="true" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="False" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientListViewId = "<%=this.lstvwStandardwiseAssessmentConfig.ClientID %>"
        _clientcstDate = "<%=this.cstDate.ClientID %>"
        _clientcstConfirmAction = "<%=this.cstConfirmAction.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _ClientChkAll = _clientListViewId + "_ChkSelectAll";
        _ClientChkSelect = _clientListViewId + "_ChkSelect";
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _clienthidStdStartDate = "<%=this.hidStdStartDate.ClientID %>"
        _clienthidStdEndDate = "<%=this.hidStdEndDate.ClientID %>"
        _clientlblUpdateSuccess = "<%=this.lblUpdateSuccess.ClientID %>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientoptIsFinal = _clientListViewId + "_optIsFinal"
        _clientcstvalAssessmentSortOrder = "<%=this.cstvalAssessmentSortOrder.ClientID %>"
        _clientcstvalDuplicateSortOrder = "<%=this.cstvalDuplicateSortOrder.ClientID %>"
        _clientcstValidDate = "<%=this.cstValidDate.ClientID %>"
        _clientcstStartDate = "<%=this.cstStartDate.ClientID %>"
        _clientcstEndDate = "<%=this.cstEndDate1.ClientID %>"
      
        function CheckAllUncheckAlls() {

            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var enble
            var iRowCount = 0
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_optIsFinal").disabled = !checkAll;
                document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_cmbSortOrder").disabled = !checkAll;
                document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtcalstartDate").disabled = !checkAll;
                document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtcalEndDate").disabled = !checkAll;               
                if (checkAll == false) {
                    document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_cmbSortOrder").value = 0;
                    document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_optIsFinal").checked = false;
                    document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtcalstartDate").value = "";
                    document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtcalEndDate").value = "";
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            }
        }

        function SetControlEnability(obj, iRowNo) {

            var iRowCount = document.getElementById(_clienthidRowCount).value
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_ChkSelect");
            var SortOrder = document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_cmbSortOrder");
            var ddlSortOrder = document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_cmbSortOrder")
            var txtStartDate = document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_txtcalstartDate")
            var txtEndDate = document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_txtcalEndDate")
            if (chk.checked == false) {
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_optIsFinal").checked = false;
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_optIsFinal").disabled = true;               
                ddlSortOrder.value = 0;
                ddlSortOrder.disabled = true;
                txtStartDate.value = "";
                txtStartDate.disabled = true;
                txtEndDate.value = "";
                txtEndDate.disabled = true;
                $("#calStartDate").datepicker("option", "disabled", true);
            }
            else {
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_cmbSortOrder").disabled = false;
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_optIsFinal").disabled = false;
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_txtcalstartDate").disabled = false;
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_txtcalEndDate").disabled = false;
            }
        }
        function DateValidations(oSrc, args) {

            var iRowCount = document.getElementById(_clienthidRowCount).value           
            var sMsg = "";
            var iRowNo = "";
            var chk = "";

            var StandardStDt = "";
            var StandardEndDt = "";
            var AssessmentNm = "";
            var AssessmentName = "";
            
            ResetUpdateLbl();

            for (var RowNumber = 0; RowNumber < iRowCount; RowNumber++) {
                chk = document.getElementById(_clientListViewId + "_ctrl" + RowNumber + "_ChkSelect");
                if (chk.checked == true) {
                    chk = document.getElementById(_clientListViewId + "_ctrl" + RowNumber + "_ChkSelect");
                    var AssName = _clientListViewId + "_ctrl" + RowNumber + "_lblAssessmentName"
                    var StartDt = _clientListViewId + "_ctrl" + RowNumber + "_txtcalstartDate"
                    var EndDt = _clientListViewId + "_ctrl" + RowNumber + "_txtcalEndDate"

                    var StDt = new Date(convertvaliddate(document.getElementById(StartDt).value))
                    var EndDt = new Date(convertvaliddate(document.getElementById(EndDt).value))
                    var StdStDt = new Date(convertvaliddate(document.getElementById(_clienthidStdStartDate).value))
                    var StdEndDt = new Date(convertvaliddate(document.getElementById(_clienthidStdEndDate).value))

                    if (StDt < StdStDt && EndDt > StdEndDt) {
                        AssessmentNm = document.getElementById(AssName).innerHTML

                        sMsg = "1";
                        iRowNo += RowNumber.toString() + ", "
                        if (AssessmentName == "")
                            AssessmentName = AssessmentNm;
                        else
                            AssessmentName += ", " + AssessmentNm;
                    }
                }
            }
            if (iRowNo != "") {
                oSrc.errormessage = "Assessment Start Date and End Date should be in between standardwise academic year start date (" + StdStDt + ") and end date ( " + StdEndDt + ") for assessment(s): " + AssessmentName + ".";
                document.getElementById(_clientcstDate).innerText = "Assessment Start Date and End Date should be in between standardwise academic year start date (" + StdStDt + ") and end date ( " + StdEndDt + ") for assessment(s): " + AssessmentName + ".";
                args.IsValid = false
                return true
            }
            if (sMsg != "") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }

        }
        function DateRangeValidations(oSrc, args) {

            var iRowCount = document.getElementById(_clienthidRowCount).value            
            var sMsg = "";
            var iRowNo = "";
            var chk = "";
            var chk1 = "";

            var StandardStDt = "";
            var StandardEndDt = "";
            var AssessmentNm = "";
            var AssessmentName = "";
            var AsesmentName = "";            
            ResetUpdateLbl();

            for (var RowNumber = 0; RowNumber < iRowCount; RowNumber++) {
//                chk = document.getElementById(_clientListViewId + "_ctrl" + RowNumber + "_ChkSelect");
//                if (chk.checked == true) {
                    var AssName1 = _clientListViewId + "_ctrl" + RowNumber + "_lblAssessmentName"
                    var StartDate1 = _clientListViewId + "_ctrl" + RowNumber + "_txtcalstartDate"
                    var EndDate1 = _clientListViewId + "_ctrl" + RowNumber + "_txtcalEndDate"

                    var AssessName = document.getElementById(AssName1).innerHTML
                    var StDt1 = new Date(convertdate(document.getElementById(StartDate1).value))
                    var EndDt1 = new Date(convertdate(document.getElementById(EndDate1).value))
                    AssessmentName = "";
                    for (i = RowNumber + 1; i < iRowCount; i++) {
                        chk1 = document.getElementById(_clientListViewId + "_ctrl" + i + "_ChkSelect");
                        if (chk1.checked == true) {
                            var AssName = _clientListViewId + "_ctrl" + i + "_lblAssessmentName";
                            var StartDt = _clientListViewId + "_ctrl" + i + "_txtcalstartDate";
                            var EndDt = _clientListViewId + "_ctrl" + i + "_txtcalEndDate";

                            var StDt = new Date(convertdate(document.getElementById(StartDt).value));
                            var EndDt = new Date(convertdate(document.getElementById(EndDt).value));

                            if ((StDt1 >= StDt && StDt1 <= EndDt) || (EndDt1 >= StDt && EndDt1 <= EndDt)) {
                                AssessmentNm = document.getElementById(AssName).innerHTML

                                sMsg = "1";
                                iRowNo += RowNumber.toString() + ", "
                                if (AssessmentName == "")
                                    AssessmentName = AssessmentNm;
                                else
                                    AssessmentName += ", " + AssessmentNm;
                            }
                        }
                    }                   
                    if (AssessmentName != "") {
                        AsesmentName += ', ' + AssessName + '  ' + ' -> (' + AssessmentName + ')';
                    }
               // }
            }
            if (iRowNo != "") {
                oSrc.errormessage = "Assessment Dates should not be overlap : " + AsesmentName.substr(1, AsesmentName.length); +".";
                document.getElementById(_clientcstDate).innerText = "Assessment Dates should not be overlap : " + AsesmentName.substr(1, AsesmentName.length); +".";
                args.IsValid = false
                return true
            }
            if (sMsg != "") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function EndDateValidations(oSrc, args) {
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = "";
            var iRowNo = "";
            var chk = "";
            var chkOpt = "";

            var StandardStDt = "";
            var StandardEndDt = "";
            var AssessmentNm = "";
            var AssessmentName = "";
            var AsesmentName = "";
            var asAss = "";
            var iNo = 0;           
            ResetUpdateLbl();

            var StartDate1 = "";
            var EndDate1 = "";
            var AssessName = "";
            var iCnt = 0;
            for (var RowNumber = 0; RowNumber < iRowCount; RowNumber++) {
                chkOpt = document.getElementById(_clientListViewId + "_ctrl" + RowNumber + "_optIsFinal");
                if (chkOpt.checked == true) {
                    iCnt = iCnt + 1;
                    var AssName1 = _clientListViewId + "_ctrl" + RowNumber + "_lblAssessmentName"
                    var StDt1 = _clientListViewId + "_ctrl" + RowNumber + "_txtcalstartDate"
                    var EndDt1 = _clientListViewId + "_ctrl" + RowNumber + "_txtcalEndDate"

                    AssessName = document.getElementById(AssName1).innerHTML
                    StartDate1 = new Date(convertdate(document.getElementById(StDt1).value))
                    EndDate1 = new Date(convertdate(document.getElementById(EndDt1).value))
                }
            }
            if (iCnt > 0) {
                for (i = 0; i < iRowCount; i++) {
                    chk = document.getElementById(_clientListViewId + "_ctrl" + i + "_ChkSelect");
                    if (chk.checked == true) {
                        var AssName = _clientListViewId + "_ctrl" + i + "_lblAssessmentName";
                        var StartDt = _clientListViewId + "_ctrl" + i + "_txtcalstartDate";
                        var EndDt = _clientListViewId + "_ctrl" + i + "_txtcalEndDate";
                        var StDt = new Date(convertdate(document.getElementById(StartDt).value))
                        var EndDt = new Date(convertdate(document.getElementById(EndDt).value))
                        if (EndDt > StartDate1 && EndDate1.getTime() != EndDt.getTime()) {
                            AssessmentNm = document.getElementById(AssName).innerHTML

                            sMsg = "1";
                            iRowNo += RowNumber.toString() + ", "
                            if (AssessmentName == "")
                                AssessmentName = AssessmentNm;
                            else
                                AssessmentName += ", " + AssessmentNm;
                        }

                    }

                }
                if (AssessmentName != "") {
                    asAss += '  ' + AssessmentName;
                }
                if (iRowNo != "") {
                    oSrc.errormessage = "Start Date of Final Assessment should be greater than end date of assessment(s) : " + asAss + ".";
                    document.getElementById(_clientcstDate).innerText = "End Date of Final Assessment should be greater than end date of assessment(s) : " + asAss + ".";
                    args.IsValid = false
                    return true
                }
                if (sMsg != "") {
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }
            else {
                oSrc.errormessage = "Any one of the selected assessment should be selected as final.";
                document.getElementById(_clientcstDate).innerText = "Any one of the selected assessment should be selected as final.";
                args.IsValid = false
                return true
            }


        }

        function CheckUncheckRadioBtn(optbtn, iRowNo) {
            var chk
            var chkOpt
            var iCount = 0           
            var chk1 = document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_ChkSelect");
            if (chk1.checked) {
                var aa = document.getElementById(_clientListViewId + "_tblAssessmentList")
                var iRowCount = document.getElementById(_clienthidRowCount).value
                for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                    chkOpt = document.getElementById(_clientListViewId + "_ctrl" + iRowNumber + "_optIsFinal");
                    chk = document.getElementById(_clientListViewId + "_ctrl" + iRowNumber + "_ChkSelect");
                    if (chk.checked == true) {
                        if (chkOpt.id != optbtn.id)
                            chkOpt.checked = false;
                    }                   
                }
            }
            else {
                optbtn.checked = false;
            }
        }
        function ConfirmAction(aSrc, args) {            
            var iCount = 0;
            var sMsg = "";
            var iRowCount = document.getElementById(_clienthidRowCount).value
            for (var RowNumber = 0; RowNumber < iRowCount; RowNumber++) {
                chk = document.getElementById(_clientListViewId + "_ctrl" + RowNumber + "_ChkSelect");
                if (chk.checked == true) {
                    iCount = iCount + 1;
                    sMsg = "1";
                }
            }
            if (iCount == 0) {
                aSrc.errormessage = "At least one Assessment should be selected for saving."
                document.getElementById(_clientcstConfirmAction).errormessage = "At least one Assessment should be selected for saving."
                args.IsValid = false
                return true
            }
            if (sMsg == "") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function ValidateAssessmentSortOrder(aSrc, args) {       
            var chk
            var sMessage = false
            var iRowCount = 0
            var sMsg = ""
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            while (chk != null) {
                if (chk.checked == true) {
                    cmbSortOrder = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_cmbSortOrder")
                    if (cmbSortOrder[0].selected == true) {
                        sMessage = true
                        sMsg = sMsg + (iRowCount + 1) + ", "
                    }
                }

                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            }
            if (sMessage == true) {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                $get(_clientcstvalAssessmentSortOrder).errormessage = "Assessment sort order should be selected for row(s): " + sMsg
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function DuplicateSortOrder(oSrc, args) {
            var iRowCount = 0;
            var sortOrders = "";
            var isDuplicate = false;

            var sCnt = "";
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            cmb = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_cmbSortOrder");

            while (chk != null) {
                if (chk.checked == true) {
                    if (cmb.value != 0) {
                        if (sortOrders.match("," + cmb.value + ",") != null) {
                            isDuplicate = true;
                            if (sCnt != "")
                                sCnt = sCnt + ", " + (iRowCount + 1);
                            else
                                sCnt = (iRowCount + 1);
                        }
                        else {
                            if (cmb.value != "9999")
                                sortOrders = sortOrders + "," + cmb.value + ",";
                        }
                    }
                }

                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + (iRowCount) + "_ChkSelect")
                cmb = document.getElementById(_clientListViewId + "_ctrl" + (iRowCount) + "_cmbSortOrder");
            }
            if (isDuplicate) {
                document.getElementById(_clientcstvalDuplicateSortOrder).errormessage = "Sort order should not be duplicate for row(s) : " + (sCnt) + ".";
                document.getElementById(_clientcstvalDuplicateSortOrder).innerHTML = "Sort order should not be duplicate for row(s) : " + (sCnt) + ".";
                args.IsValid = false;
            }
            if (args.IsValid == false)
                return true;

            if (args.IsValid == true)
                return false;
        }
        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSuccess) != undefined)
                document.getElementById(_clientlblUpdateSuccess).innerHTML = ""
            if (document.getElementById(_clientlblErrorMsg) != undefined)
                document.getElementById(_clientlblErrorMsg).innerHTML = ""            
        }

        function StartAndEndDateValidation(oSrc, args) {
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = "";
            var iRowNo = ""
            var RowNumber;
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var StartDate = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtcalstartDate"
                var EndDate = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtcalEndDate"                
                var TermStartDate = new Date(convertdate(document.getElementById(StartDate).value));
                var TermEndDate = new Date(convertdate(document.getElementById(EndDate).value));
                if (TermStartDate != "" && TermStartDate != "") {
                    var dtStartDate;
                    var dtEndDate;
                   
                    if (document.all) {
                        dtStartDate = new Date(TermStartDate.replace('-', ' '));
                        dtEndDate = new Date(TermStartDate.replace('-', ' '));                        
                    }
                    else {
                        dtStartDate = new Date(convertdate(document.getElementById(StartDate).value));
                        dtEndDate = new Date(convertdate(document.getElementById(EndDate).value));
                    }
                    if (dtStartDate > dtEndDate) {                        
                        sMsg = "1";
                        iRowNo += i.toString() + ", "                        
                    }
                }
            }
            if (iRowNo != "") {
                oSrc.errormessage = "End Date should be greater than Start Date.";
                document.getElementById(_clientcstValidDate).innerHTML = "End Date should be greater than Start Date.";
                args.IsValid = false
                return true
            }
            if (sMsg != "") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function EmptyStartDate(oSrc, args) {            
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = "";
            var iRowNo = ""
            var RowNumber;
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var StartDate = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtcalstartDate"
                var chk = document.getElementById(_clientListViewId + "_ctrl" + (RowNumber) + "_ChkSelect")
                var TermStartDate = (document.getElementById(StartDate).value);
                if (chk.checked && TermStartDate == "") {                    
                    sMsg = "1";
                    iRowNo += i.toString() + ", "                    
                }
            }
            if (iRowNo != "") {               
                oSrc.errormessage = "Start Date should not be Empty.";
                document.getElementById(_clientcstStartDate).innerHTML = "Start Date should not be Empty.";
                args.IsValid = false
                return true
            }
            if (sMsg != "") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function EmptyEndDate(oSrc, args) {
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = "";
            var iRowNo = ""
            var RowNumber;
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var EndDate = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtcalEndDate"
                var chk = document.getElementById(_clientListViewId + "_ctrl" + (RowNumber) + "_ChkSelect")
                var TermEndDate = (document.getElementById(EndDate).value);
                if (chk.checked && TermEndDate == "") {
                    sMsg = "1";
                    iRowNo += i.toString() + ", "
                }
            }
            if (iRowNo != "") {               
                oSrc.errormessage = "End Date should not be Empty.";
                document.getElementById(_clientcstEndDate).innerHTML = "End Date should not be Empty.";
                args.IsValid = false
                return true
            }
            if (sMsg != "") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
    </script>
</asp:Content>
