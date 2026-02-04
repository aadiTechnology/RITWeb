<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HolidayConfigurationPopup.aspx.cs"
    MasterPageFile="../MasterPages/PopupMasterSml.master" Inherits="HolidayConfigurationPopup" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            <tr>
                <td align="left" colspan="2" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True"
                                    EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333" valign="top">
                   <span class="ClsMdtStar">*</span>
                                            <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:ValidationSummary ID="valSumErrorMsg" CssClass="ClsLabel" runat="server" />
                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="570px" align="center">
                        <%--<asp:Panel ID="pnlFields" runat="server">--%>
                        <tr>
                            <td colspan="2">
                                <table width="560px" cellpadding="1" cellspacing="2">
                                    <tr>
                                        <td valign="top" class="ClsBorderlight" style="width: 40%" align="right">
                                      <span class="LblRht colonPadding">:</span>  
                                            <asp:Label ID="lblStartDate" runat="server" CssClass="LblRht" Text= "<%$ Resources:LocalizedResources, Start_Date%>"
                                                EnableViewState="false"></asp:Label>
                                                
                                        </td>
                                        <td valign="top" align="left" style="width: 60%" class="ClsBorderlight">
                                            <asp:TextBox ID="txtStartDate" CssClass="SmlCombo" onchange="CountHolidayDays()" runat="server" AutoPostBack="True"
                                                TabIndex="1"></asp:TextBox>
                                            <rjs:PopCalendar ID="cStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy" Culture = "en"
                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage= "<%$ Resources:LocalizedResources, CalMsgStartDate%>" />
                                            <span class="ClsMdtStar" style="color: Red">* </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ClsBorderlight" align="right">     
                                         <span class="LblRht colonPadding">:</span>                                   
                                            <asp:Label ID="lblEndDate" runat="server" CssClass="LblRht" Text= "<%$ Resources:LocalizedResources, End_Date%>" EnableViewState="false"></asp:Label>
                                           
                                        </td>
                                        <td valign="top" align="left" class="ClsBorderlight">
                                            <asp:TextBox ID="txtEndDate" CssClass="SmlCombo" runat="server" onchange="CountHolidayDays()" AutoPostBack="True"
                                                TabIndex="2"></asp:TextBox>
                                            <rjs:PopCalendar ID="cEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy" Culture = "en"
                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage= "<%$ Resources:LocalizedResources, CalMsgEndDate%>" />
                                            <span class="ClsMdtStar" style="color: Red">* </span>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="ClsBorderlight">
                                          <span class="LblRht colonPadding">:</span>
                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" Text= "<%$ Resources:LocalizedResources, TotalDays%>" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" valign="middle" class="ClsBorderlight">
                                            <asp:TextBox ID="lblTotaldays" ReadOnly="true" CssClass="ClsHilightBGB" Style="width: 38px;
                                                height: 16px" runat="server" TabIndex="-10"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ClsBorderlight">
                                        <span class="LblRht colonPadding">:</span>
                                            <asp:Label ID="lblHolidayName" runat="server" CssClass="LblRht" Text= "<%$ Resources:LocalizedResources, Name%>" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td valign="top" align="left" class="ClsBorderlight">
                                            <asp:TextBox ID="txtNameofHoliday" runat="server" CssClass="LrgTxtBox" MaxLength="50"
                                                TabIndex="3"></asp:TextBox>
                                            <span class="ClsMdtStar" style="color: Red">* </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ClsBorderlight">
                                           <span class="LblRht colonPadding">:</span>
                                            <asp:Label ID="lblRemarks" runat="server" CssClass="LblRht" Text= "<%$ Resources:LocalizedResources, Remarks%>" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" valign="top" class="ClsBorderlight">
                                            <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" CssClass="LrgTxtBox"
                                                MaxLength="200" Height="46px" TabIndex="4" Width="233px"></asp:TextBox>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                    <td align="right" class="ClsBorderlight" valign="middle">
                                      <span class="LblRht colonPadding">:</span>
                                    <asp:Label ID="Label6" runat="server" Text= "<%$ Resources:LocalizedResources, AssociatedClass%>" CssClass="LblRht" 
                                        EnableViewState="False"></asp:Label><br /> <br />        
                                    <asp:CheckBox ID="chkAll" runat="server" Text= "<%$ Resources:LocalizedResources, SelectAll%>" onclick="CheckAll1(this);" TabIndex="7" style="padding-right:5px" />                                                              
                                    </td>
                                        <td colspan="2" align="left">
                                          <asp:ListView ID="lstvwStandardDivisions" runat="server" DataKeyNames="StandardId" 
                                                OnItemDataBound="lstvwStandardDivisions_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table align="right" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                        cellpadding="0" cellspacing="1" class="GridBorder">                                                        
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                        <td align="left" style="padding-left: 5px">
                                                            <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>'/>                                                            
                                                        </td>
                                                        <td align="left" style="padding-left: 5px">                                                        
                                                            <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                CssClass="ClsLabel" RepeatColumns="6">
                                                            </asp:CheckBoxList>
                                                        </td>                                
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow" style="height:10px">
                                                        <td align="left" style="padding-left: 5px">
                                                            <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>'   />                                                           
                                                        </td>
                                                        <td align="left" style="padding-left: 5px">                                                            
                                                            <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                CssClass="ClsLabel" RepeatColumns="6">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <EmptyDataTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="LblNoRecord" align="center">
                                                            <asp:Label ID="lblNoRecord" runat="server" Text= "<%$ Resources:LocalizedResources, NoRecordsFound%>" 
                                        EnableViewState="False"></asp:Label>       
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                        </td>
                                        <td>
                                            <span class="ClsMdtStar" style="color: Red">*
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn" 
                                                CausesValidation="true" OnClick="btnSave_Click"  UseSubmitBehavior="false" />
                                            <asp:Button ID="btnCancel" runat="server" Text= "<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" 
                                                CausesValidation="False" UseSubmitBehavior="false" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtStartDate"
                                    ErrorMessage= "<%$ Resources:LocalizedResources, ValStartDateBlank1%>" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEndDate"
                                    ErrorMessage= "<%$ Resources:LocalizedResources, ValEndDateBlank1%>" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNameofHoliday"
                                    ErrorMessage= "<%$ Resources:LocalizedResources, ValHolidayName%>" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="cst_Remark" runat="server" Display="None" ControlToValidate="txtRemarks"
                                    ErrorMessage= "<%$ Resources:LocalizedResources, ValRemarkLength%>" ValidationExpression="^[\s\S]{0,200}$">
                                </asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="cstvalStandards" runat="server" Display="None" ClientValidationFunction="ValidateStandards"
                                    SetFocusOnError="True" ErrorMessage= "<%$ Resources:LocalizedResources, valClassSelected%>">
                                </asp:CustomValidator>
                                <asp:CustomValidator ID="cst_StartDate" runat="server" ClientValidationFunction="cstStartDate"
                                    Display="None" SetFocusOnError="True" ErrorMessage=""></asp:CustomValidator>
                                <asp:CustomValidator ID="cst_EndDate" runat="server" Display="None" ClientValidationFunction="cstEndDate"
                                    SetFocusOnError="True" ErrorMessage="">                             
                                </asp:CustomValidator>
                            </td>
                        </tr>
                        <%--</asp:Panel>--%>
                        <tr>
                            <td>
                                <div runat="server" id="divErr">
                                </div>
                                 <asp:HiddenField ID="hidEndDate" runat="server" />
                                <asp:HiddenField ID="hidYearEndDate" runat="server" />
                                <asp:HiddenField ID="hidYearStartDate" runat="server" />
                                <asp:HiddenField ID="hidHolidayId" runat="server" />
                                <asp:HiddenField ID="hidActionFlag" runat="server" />
                                <asp:HiddenField ID="hidIsConfig" runat="server" />
                                <asp:HiddenField ID="hidTotalDays" runat="server" />
                                <asp:HiddenField ID="hidHolidayName" runat="server" />
                                <asp:HiddenField ID="hidChkLstCnt" runat="server" />
                                <asp:HiddenField ID = "hidValStartEndDate" runat = "server" />
                                <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                <asp:HiddenField ID = "hidvalHolidayStartDate" runat = "server" />
                                <asp:HiddenField ID = "hidvalHolidayEndDate" runat = "server" />
                                <asp:HiddenField ID = "hidand" runat ="server" />
                                <asp:HiddenField ID = "hidbetween" runat = "server" />
                                <asp:HiddenField ID = "hidHolidayBetween" runat = "server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>"
        _clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>"
        _clientCstStartDate = "<%=this.cst_StartDate.ClientID %>"
        _clientCstEndDate = "<%=this.cst_EndDate.ClientID %>"
        _clientErrLabel = "<%=this.lblErrorMsg.ClientID %>"
        _clienttxtStartDateID = "<%=this.txtStartDate.ClientID %>"
        _clienttxtEndDateID = "<%=this.txtEndDate.ClientID %>"
        _clientlblTotaldaysID = "<%=this.lblTotaldays.ClientID %>"
        _clienthidEndDateID = "<%=this.hidEndDate.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clientvalSumErrorMsgId = "<%=this.valSumErrorMsg.ClientID %>"
        _clienthidTotalDaysID = "<%=this.hidTotalDays.ClientID %>"        
        _clienthidChkLstCnt = "<%=this.hidChkLstCnt.ClientID %>"        
        _clientcstvalStandards = "<%=this.cstvalStandards.ClientID %>"
        _clientlstvwStandardDivisions = "<%=this.lstvwStandardDivisions.ClientID %>"
        _clientchkAll = "<%=this.chkAll.ClientID %>"

        function CountHolidayDays() {
            var dtEndDate, dtStartDate
            dtEndDate = new Date(convertdate(document.getElementById(_clienttxtEndDateID).value))
            dtStartDate = new Date(convertdate(document.getElementById(_clienttxtStartDateID).value))
            if (dtEndDate >= dtStartDate) {
                document.getElementById(_clientlblTotaldaysID).value = ((dtEndDate - dtStartDate) / (3600 * 24000)) + 1
            }
            else
                document.getElementById(_clientlblTotaldaysID).value = "0"
        }
        function closewindow() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnCancel).disabled = true
            window.close()
        }

        function CheckDate() {
            ClearErrorMsg()
            var sDate
            if (document.all)
                sDate = new Date(document.getElementById(_clienttxtStartDateID).value.replace('-', ' '))
            else
                sDate = new Date(convertdate(document.getElementById(_clienttxtStartDateID).value))
            document.getElementById(_clienttxtEndDateID).value = document.getElementById(_clienttxtStartDateID).value
            var strStartDate = document.getElementById(_clienttxtStartDateID).value
            var strEndDate = document.getElementById(_clienttxtEndDateID).value
            if (strStartDate == "" || strEndDate == "") {
                document.getElementById(_clientlblTotaldaysID).value = "0"
            }
            else {
                document.getElementById(_clientlblTotaldaysID).innerText = "1"
            }
            document.getElementById(_clienthidTotalDaysID).value = document.getElementById(_clientlblTotaldaysID).value
        }

        function ClearErrorLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            if (isPageValid) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            }
            ClearErrorMsg()
        }

        function getDateString(obj) {
            var strDate = obj.getDate() + "-"
            var strMonth = parseInt(obj.getMonth())
            strMonth = months[strMonth]
            strDate = strDate + strMonth + "-"
            strDate = strDate + obj.getFullYear()
            return strDate
        }
        function getMonthName(month) {
            switch (month) {
                case 1:
                    return "Jan"
                    break
                case 2:
                    return "Feb"
                    break
                case 3:
                    return "March"
                    break
                case 4:
                    return "April"
                    break
                case 5:
                    return "May"
                    break
                case 6:
                    return "June"
                    break
                case 7:
                    return "July"
                    break
                case 8:
                    return "Aug"
                    break
                case 9:
                    return "Sep"
                    break
                case 10:
                    return "Oct"
                    break
                case 11:
                    return "Nov"
                    break
                case 12:
                    return "Dec"
                    break
            }
        }
        function cstStartDate(aSrc, args) {
            ClearErrorMsg()
            var sMessage = "";
            var dtEndDate, dtStartDate
            if (document.all) {
                dtEndDate = new Date(document.getElementById(_clienttxtEndDateID).value.replace('-', ' '))
                dtStartDate = new Date(document.getElementById(_clienttxtStartDateID).value.replace('-', ' '))
            }
            else {
                dtEndDate = new Date(convertdate(document.getElementById(_clienttxtEndDateID).value))
                dtStartDate = new Date(convertdate(document.getElementById(_clienttxtStartDateID).value))
            }
            document.getElementById(_clienthidEndDateID).value = document.getElementById(_clienttxtEndDateID).value
            var strStartDate = document.getElementById(_clienttxtStartDateID).value
            var strEndDate = document.getElementById(_clienttxtEndDateID).value

            if (dtEndDate >= dtStartDate) {
                document.getElementById(_clientlblTotaldaysID).value = ((dtEndDate - dtStartDate) / (3600 * 24000)) + 1
            }
            else
                document.getElementById(_clientlblTotaldaysID).value = "0"
            document.getElementById(_clienthidTotalDaysID).value = document.getElementById(_clientlblTotaldaysID).value
            if (!(CheckIfDateInAcademicYear(dtStartDate))) {
                var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value)
                var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value)
                var strStartYear = getDateString(dtYearStartDate)
                var strEndYear = getDateString(dtYearEndDate)
                document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=this.hidvalHolidayStartDate.ClientID %>").value + " (i.e " + document.getElementById("<%=this.hidbetween.ClientID %>").value + strStartYear + document.getElementById("<%=this.hidand.ClientID %>").value + strEndYear + document.getElementById("<%=this.hidHolidayBetween.ClientID %>").value + ")."
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function cstEndDate(aSrc, args) {
            var sMessage = "";
            ClearErrorMsg()
            var dtEndDate, dtStartDate
            if (document.all) {
                dtEndDate = new Date(document.getElementById(_clienttxtEndDateID).value.replace('-', ' '))
                dtStartDate = new Date(document.getElementById(_clienttxtStartDateID).value.replace('-', ' '))
            }
            else {
                dtEndDate = new Date(convertdate(document.getElementById(_clienttxtEndDateID).value))
                dtStartDate = new Date(convertdate(document.getElementById(_clienttxtStartDateID).value))
            }
            document.getElementById(_clienthidEndDateID).value = document.getElementById(_clienttxtEndDateID).value
            var strStartDate = document.getElementById(_clienttxtStartDateID).value
            var strEndDate = document.getElementById(_clienttxtEndDateID).value

            if (dtEndDate >= dtStartDate) {
                document.getElementById(_clientlblTotaldaysID).value = ((dtEndDate - dtStartDate) / (3600 * 24000)) + 1
            }
            else
                document.getElementById(_clientlblTotaldaysID).value = "0"
            document.getElementById(_clienthidTotalDaysID).value = document.getElementById(_clientlblTotaldaysID).value
            if (dtStartDate > dtEndDate) {
                document.getElementById(_clientCstEndDate).errormessage = document.getElementById("<%=this.hidValStartEndDate.ClientID %>").value;
                args.IsValid = false
                return true
            }
            else if (!(CheckIfDateInAcademicYear(dtEndDate))) {
                var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value)
                var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value)
                var strStartYear = getDateString(dtYearStartDate)
                var strEndYear = getDateString(dtYearEndDate)
                document.getElementById(_clientCstEndDate).errormessage = document.getElementById("<%=this.hidvalHolidayEndDate.ClientID %>").value + "(i.e" + document.getElementById("<%=this.hidbetween.ClientID %>").value + strStartYear + document.getElementById("<%=this.hidand.ClientID %>").value + strEndYear + document.getElementById("<%=this.hidHolidayBetween.ClientID %>").value + ")."
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function CheckIfDateInAcademicYear(dtObj) {
            var bReturn
            var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value)
            var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value)
            if ((dtObj < dtYearStartDate) || (dtObj > dtYearEndDate)) {
                bReturn = false
            }
            else {
                bReturn = true
            }
            return bReturn
        }
        function ClearErrorMsg() {

            if (document.getElementById(_clientErrLabel) != null) {
                document.getElementById(_clientErrLabel).style.display = "none"
            }
            if (document.getElementById(_clientvalSumErrorMsgId) != null) {
                document.getElementById(_clientvalSumErrorMsgId).style.display = "none"
            }
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }

        function CheckOrUncheckAllCheckBox() {
            var iCount = document.getElementById(_clienthidChkLstCnt).value
            var chkAll = document.getElementById(_clientchkAll).checked;
            for (i = 0; i < iCount; i++) {
                document.getElementById(_clientchkStandardLst + "_" + i).checked = chkAll
            }
        }
        
        function CheckAll(obj,iRowCount) {           
            var chk
            var iRowCnt = 0
            chk = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandardDivLst_" + iRowCnt);
            while (chk != null) {
                chk.checked = obj.checked;
                iRowCnt = iRowCnt + 1
                chk = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandardDivLst_" + iRowCnt);
            }
            CheckAllDependancy();
        }

        function CheckAllCheck(obj, iRowCount) {
            var chk
            var isChecked=0,isUnchecked=0
            var iRowCnt = 0
            obj = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandard");
            chk = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandardDivLst_" + iRowCnt);
            while (chk != null) {
                if (chk.checked)
                    isChecked = 1;
                else
                    isUnchecked = 1;
                iRowCnt = iRowCnt + 1

                chk = document.getElementById(_clientlstvwStandardDivisions + "_ctrl" + iRowCount + "_chkStandardDivLst_" + iRowCnt);
            }
            if ((isChecked == 1 && isUnchecked == 1) || isUnchecked == 1) 
                obj.checked = false;                            
            else if (isChecked == 1)
                obj.checked = true;
            
            CheckAllDependancy();            
        }

        function CheckAllDependancy() {
            var CheckAll = document.getElementById(_clientchkAll).value;
            var v1 = 0;            
            
            var listView = document.getElementById('<%= lstvwStandardDivisions.FindControl("tblStaffInfo").ClientID %>');
            
            for (var i = 0; i < listView.rows.length; i++) {
                var inputs = listView.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].type == "checkbox") {
                        if (!inputs[j].checked) {
                            v1 = 1;
                        }
                    }
                }
            }
            if (v1 == 1)
                document.getElementById(_clientchkAll).checked = false;
            else
                document.getElementById(_clientchkAll).checked = true;
        }

        function CheckAll1(Src) {            
              var listView = document.getElementById('<%= lstvwStandardDivisions.FindControl("tblStaffInfo").ClientID %>');
            for (var i = 0; i < listView.rows.length; i++) {
                var inputs = listView.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].type == "checkbox")
                        inputs[j].checked = Src.checked;
                }
            }

        }
        
        function ValidateStandards(aSrc, args) {                        
            var j = 0;
            var checks = document.forms[0].elements;
            var boxLength = checks.length;

            for (i = 0; i < boxLength; i++) {
                if (checks[i].type == 'checkbox') {
                    if (checks[i].checked == true) {
                        j++;
                    }
                }
            }

            if (j > 0) {
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }
               
    </script>
   
</asp:Content>
