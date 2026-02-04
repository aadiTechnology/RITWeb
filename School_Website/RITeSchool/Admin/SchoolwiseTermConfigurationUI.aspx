<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SchoolwiseTermConfigurationUI.aspx.cs" Inherits="SchoolwiseTermConfigurationUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                            vertical-align: top">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 77%">
                                                <asp:CustomValidator ID="CustomValidator1" Display="None" runat="server"
                                                    ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                                    SetFocusOnError="True" ClientValidationFunction="ValidateDates"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstDate" Display="None" runat="server" ValidationGroup="Save"
                                                    CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                                    ClientValidationFunction="DateValidations"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstDateTermII" Display="None" runat="server" ValidationGroup="Save"
                                                    CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                                    ClientValidationFunction="DateValidationsTermII"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstTermDate" Display="None" runat="server" ValidationGroup="Save"
                                                    CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                                    ClientValidationFunction="TermDateValidations"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstT1StDtAcademicYrValidation" Display="None" runat="server"
                                                    ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                                    SetFocusOnError="True" ClientValidationFunction="Term1StDtValidationAsPerAcademicYrDates"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstT1EndDtAcademicYrValidation" Display="None" runat="server"
                                                    ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                                    SetFocusOnError="True" ClientValidationFunction="Term1EndDtValidationAsPerAcademicYrDates"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstT2StDtAcademicYrValidation" Display="None" runat="server"
                                                    ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                                    SetFocusOnError="True" ClientValidationFunction="Term2StDtValidationAsPerAcademicYrDates"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstT2EndDtAcademicYrValidation" Display="None" runat="server"
                                                    ValidationGroup="Save" CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage=""
                                                    SetFocusOnError="True" ClientValidationFunction="Term2EndDtValidationAsPerAcademicYrDates"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                                    CssClass="ClsLabel" ShowSummary="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" class="ClsTextNormal" align="center" id="tdMessage" runat="server">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"
                                        Visible="False" EnableViewState="False" CssClass="ClsTextNormal" Font-Bold="True" style="text-align:center;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpnlListView" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr id="tr1" runat="server">
                                <td align="center">
                                </td>
                            </tr>
                        </table>
                        <table id="tblTermList" align="center" width="80%">
                            <tr align="center" style="width: 100%">
                                <td align="center" style="width: 950px">
                                    <asp:ListView ID="lstvwTermConfiguration" runat="server" DataKeyNames="StandardId"
                                        OnItemDataBound="lstvwTermConfiguration_ItemDataBound">
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblTermInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="center" class="paddingL" style="width: 18%; padding-right: 10px"> 
                                                        <asp:Label ID="lblStandardName" runat="server" Text="<%$ Resources:LocalizedResources, Standard%>"></asp:Label>
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 220px">
                                                       <asp:Label ID="lblStartTermI" runat="server" Text="<%$ Resources:LocalizedResources, StartDateTermI%>"></asp:Label>
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 220px">
                                                       <asp:Label ID="lblEndDateTermI" runat="server" Text="<%$ Resources:LocalizedResources, EndDateTermI%>"></asp:Label>
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 220px">
                                                        <asp:Label ID="lblStartTermII" runat="server" Text="<%$ Resources:LocalizedResources, StartDateTermII%>"></asp:Label>
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 220px">
                                                      <asp:Label ID="lblEndDateTermII" runat="server" Text="<%$ Resources:LocalizedResources, EndDateTermII%>"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                <td align="center" class="paddingL" style="padding-right: 10px">
                                                    <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidStartDate" runat="server" />
                                                    <asp:HiddenField ID="hidEndDate" runat="server" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:TextBox ID="txtTerm1StartDate" runat="server" MaxLength="11" Text='<%# Convert.ToDateTime(Eval("TermIInfo.TermStartDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm1StartDate" runat="server" Control="txtTerm1StartDate" culture="en"
                                                        Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                    <asp:HiddenField ID="hidTrmIId" Value='<%# Eval("TermIInfo.TermId")%>' runat="server" />
                                                    <asp:HiddenField ID="hidSchoolwiseTermIId" Value='<%# Eval("TermIInfo.SchoolwiseTermId")%>'
                                                        runat="server" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:TextBox ID="txtTerm1EndDate" runat="server" MaxLength="11" Text='<%# Convert.ToDateTime(Eval("TermIInfo.TermEndDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm1EndDate" runat="server" Control="txtTerm1EndDate" Format= "dd MMM yyyy" Culture="en"
                                                        ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:TextBox ID="txtTerm2StartDate" runat="server" MaxLength="11" Text='<%# Convert.ToDateTime(Eval("TermIIInfo.TermStartDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm2StartDate" runat="server" Control="txtTerm2StartDate" Culture="en"
                                                        Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                    <asp:HiddenField ID="hidTrmIIId" Value='<%# Eval("TermIIInfo.TermId")%>' runat="server" />
                                                    <asp:HiddenField ID="hidSchoolwiseTermIIId" Value='<%# Eval("TermIIInfo.SchoolwiseTermId")%>'
                                                        runat="server" />
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:TextBox ID="txtTerm2EndDate" runat="server" MaxLength="11" Text='<%# Convert.ToDateTime(Eval("TermIIInfo.TermEndDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm2EndDate" runat="server" Control="txtTerm2EndDate" Format="dd MMM yyyy" Culture="en"
                                                        ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td align="center" class="paddingL" style="padding-right: 10px">
                                                    <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidStartDate" runat="server" />
                                                    <asp:HiddenField ID="hidEndDate" runat="server" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:TextBox ID="txtTerm1StartDate" runat="server" MaxLength="11" Text='<%# Convert.ToDateTime(Eval("TermIInfo.TermStartDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                        CssClass="SmlTxtBox" ></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm1StartDate" runat="server" Control="txtTerm1StartDate" Culture="en"  
                                                        Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                    <asp:HiddenField ID="hidTrmIId" Value='<%# Eval("TermIInfo.TermId")%>' runat="server" />
                                                    <asp:HiddenField ID="hidSchoolwiseTermIId" Value='<%# Eval("TermIInfo.SchoolwiseTermId")%>'
                                                        runat="server" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:TextBox ID="txtTerm1EndDate" runat="server" MaxLength="11"  Text='<%# Convert.ToDateTime(Eval("TermIInfo.TermEndDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm1EndDate" runat="server" Control="txtTerm1EndDate" Format="dd MMM yyyy" Culture="en"
                                                        ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:TextBox ID="txtTerm2StartDate" runat="server" MaxLength="11" Text='<%# Convert.ToDateTime(Eval("TermIIInfo.TermStartDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm2StartDate" runat="server" Control="txtTerm2StartDate" Culture="en"
                                                        Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                    <asp:HiddenField ID="hidTrmIIId" Value='<%# Eval("TermIIInfo.TermId")%>' runat="server" />
                                                    <asp:HiddenField ID="hidSchoolwiseTermIIId" Value='<%# Eval("TermIIInfo.SchoolwiseTermId")%>'
                                                        runat="server" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:TextBox ID="txtTerm2EndDate" runat="server" MaxLength="11" Text='<%# Convert.ToDateTime(Eval("TermIIInfo.TermEndDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm2EndDate" runat="server" Control="txtTerm2EndDate" Format="dd MMM yyyy" Culture="en"
                                                        ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                       <asp:Label ID="lblNoRecordFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
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
                                    <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save%>" runat="server" CssClass="ClsBtn" ValidationGroup="Save" disable-page="true"
                                        BorderWidth="1px" CausesValidation="true" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidRowCount" runat="server" />
    <asp:HiddenField ID="hidCurrentDate" runat="server" />
    <asp:HiddenField ID="hidTermId" runat="server" />
    <asp:HiddenField ID="hidEndDate" runat="server" />
    <asp:HiddenField ID="hidValTermIEndDateShouldBeGreaterThanTermIStartDate" runat="server" />
    <asp:HiddenField ID="hidValTermIIEndDateShouldBeGreaterThanTermIIStartDate" runat="server" />
    <asp:HiddenField ID="hidValTermIIStartDateShouldBeGreaterThanTermIEndDate" runat="server" />
    <asp:HiddenField ID="hidValTermIStartDateShouldBeInBetween" runat="server" />
    <asp:HiddenField ID="hidAnd" runat="server" />
    <asp:HiddenField ID="hidForStandard" runat="server" />
    <asp:HiddenField ID="hidCultureInfo" runat="server" />
    <asp:HiddenField ID="hidTermIEndDateShouldBeInBetween" runat="server" />
    <asp:HiddenField ID="hidTermIIStartDateShouldBeInBetween" runat="server" />
    <asp:HiddenField ID="hidTermIIEndDateShouldBeInBetween" runat="server" />
    <table id="tblSaveTermName" runat="server" border="0" cellpadding="1" cellspacing="2"
        style="width: 46%;" align="center">
    </table>

    <script language="javascript" type="text/javascript">

        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientcstDate = "<%=this.cstDate.ClientID %>"
        _clientcstDateTermII = "<%=this.cstDateTermII.ClientID %>"
        _clientcstTermDate = "<%=this.cstTermDate.ClientID %>"
        _clientcstT1StDtAcademicYrValidation = "<%=this.cstT1StDtAcademicYrValidation.ClientID %>"
        _clientcstT1EndDtAcademicYrValidation = "<%=this.cstT1EndDtAcademicYrValidation.ClientID %>"
        _clientcstT2StDtAcademicYrValidation = "<%=this.cstT2StDtAcademicYrValidation.ClientID %>"
        _clientcstT2EndDtAcademicYrValidation = "<%=this.cstT2EndDtAcademicYrValidation.ClientID %>"
        _clientTermViewId = "<%=this.lstvwTermConfiguration.ClientID %>"
        _clienthidCurrentDate = "<%=this.hidCurrentDate.ClientID %>"
        _clientlblMessage = "<%=this.lblMessage.ClientID %>"
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"

        function ClearMessage() {
            if (document.getElementById(_clientlblMessage) != null) {
                document.getElementById(_clientlblMessage).innerHTML = ""
            }
        }

        function ValidateDates(oSrc, args) {
            var rowINdex = 0
            var lblStandardName = document.getElementById(_clientTermViewId + "_ctrl" + rowINdex + "_" + "lblStandardName")
            var standards = ""

            while (lblStandardName != null) {
                var txtTerm1StartDate = document.getElementById(_clientTermViewId + "_ctrl" + rowINdex + "_" + "txtTerm1StartDate")
                var txtTerm1EndDate = document.getElementById(_clientTermViewId + "_ctrl" + rowINdex + "_" + "txtTerm1EndDate")
                var txtTerm2StartDate = document.getElementById(_clientTermViewId + "_ctrl" + rowINdex + "_" + "txtTerm2StartDate")
                var txtTerm2EndDate = document.getElementById(_clientTermViewId + "_ctrl" + rowINdex + "_" + "txtTerm2EndDate")

                if (txtTerm1StartDate.value.trim() == "" || txtTerm1EndDate.value.trim() == "" || txtTerm2StartDate.value.trim() == "" || txtTerm2EndDate.value.trim() == "")
                    standards = standards + "," + lblStandardName.innerHTML;

                rowINdex = rowINdex + 1
                lblStandardName = document.getElementById(_clientTermViewId + "_ctrl" + rowINdex + "_" + "lblStandardName")
            }

            if (standards.length > 0) {
                standards = standards.substring(1)
                oSrc.errormessage = "Term Date(s) should not be blank for standard(s) '" + standards + "'.";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function DateValidations(oSrc, args) {
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = ""
            var iRowNo = ""
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            
            var TermStandard = "";
            var TermStd = "";
            var TodayDate = document.getElementById(_clienthidCurrentDate).value
            var dtToday;
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var TermIStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm1StartDate"
                var TermIEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm1EndDate"
                var Std = _clientTermViewId + "_ctrl" + RowNumber + "_" + "lblStandardName"

                var TermIStDt1 = document.getElementById(TermIStDt).value
                var TermIEndDt1 = document.getElementById(TermIEndDt).value

                if (TermIStDt1 != "" && TermIEndDt1 != "") {
                    var dtStartDate;
                    var dtEndDate;
                    if (document.all) {

                        dtStartDate = new Date(TermIStDt1.replace('-', ' '));
                        dtEndDate = new Date(TermIEndDt1.replace('-', ' '));
                        dtToday = new Date(TodayDate.replace('-', ' '));
                    }
                    else {
                        dtStartDate = new Date(convertdate(document.getElementById(TermIStDt).value));
                        dtEndDate = new Date(convertdate(document.getElementById(TermIEndDt).value));
                        dtToday = new Date(convertdate(TodayDate));

                    }
                    if (dtStartDate > dtEndDate) {
                        TermStandard = document.getElementById(Std).innerHTML

                        sMsg = "1";
                        iRowNo += i.toString() + ", "
                        if (TermStd == "")
                            TermStd = TermStandard;
                        else
                            TermStd += ", " + TermStandard;
                    }
                }
            }
            if (iRowNo != "") {
                oSrc.errormessage = document.getElementById("<%=hidValTermIEndDateShouldBeGreaterThanTermIStartDate.ClientID%>").value + " : " + TermStd + ".";
                document.getElementById(_clientcstDate).innerText = document.getElementById("<%=hidValTermIEndDateShouldBeGreaterThanTermIStartDate.ClientID%>").value + " : " + TermStd + ".";
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

        function DateValidationsTermII(oSrc, args) {

            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = ""
            var iRowNo = ""
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            
            var TermStandard = "";
            var TermStd = "";
            var TodayDate = document.getElementById(_clienthidCurrentDate).value
            var dtToday;
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var TermIIStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm2StartDate"
                var TermIIEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm2EndDate"
                var Std = _clientTermViewId + "_ctrl" + RowNumber + "_" + "lblStandardName"

                var TermIIStDt2 = document.getElementById(TermIIStDt).value
                var TermIIEndDt2 = document.getElementById(TermIIEndDt).value

                if (TermIIStDt2 != "" && TermIIEndDt2 != "") {
                    var dtStartDate;
                    var dtEndDate;
                    if (document.all) {
                        dtStartDate = new Date(TermIIStDt2.replace('-', ' '));
                        dtEndDate = new Date(TermIIEndDt2.replace('-', ' '));
                        dtToday = new Date(TodayDate.replace('-', ' '));
                    }
                    else {
                        dtStartDate = new Date(convertdate(document.getElementById(TermIIStDt).value));
                        dtEndDate = new Date(convertdate(document.getElementById(TermIIEndDt).value));
                        dtToday = new Date(convertdate(TodayDate));
                    }
                    if (dtStartDate > dtEndDate) {
                        TermStandard = document.getElementById(Std).innerHTML
                        sMsg = "1";
                        iRowNo += i.toString() + ", "
                        if (TermStd == "")
                            TermStd = TermStandard;
                        else
                            TermStd += ", " + TermStandard;
                    }
                }
            }
            if (iRowNo != "") {
                oSrc.errormessage = document.getElementById("<%=hidValTermIIEndDateShouldBeGreaterThanTermIIStartDate.ClientID %>").value + " : " + TermStd + ".";
                document.getElementById(_clientcstDateTermII).innerHTML = document.getElementById("<%=hidValTermIIEndDateShouldBeGreaterThanTermIIStartDate.ClientID %>").value + " : " + TermStd + ".";
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
        /////////////////////////////////////////////////
        function TermDateValidations(oSrc, args) {


            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = ""
            var iRowNo = ""
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            
            var TermStandard = "";
            var TermStd = "";
            var TodayDate = document.getElementById(_clienthidCurrentDate).value
            var dtToday;
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var TermIIStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm2StartDate"
                var TermIEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm1EndDate"
                var Std = _clientTermViewId + "_ctrl" + RowNumber + "_" + "lblStandardName"

                var TermIIStDt2 = document.getElementById(TermIIStDt).value
                var TermIEndDt1 = document.getElementById(TermIEndDt).value

                if (TermIIStDt2 != "" && TermIEndDt1 != "") {
                    var dtStartDate;
                    var dtEndDate;
                    if (document.all) {
                        dtStartDate = new Date(TermIIStDt2.replace('-', ' '));
                        dtEndDate = new Date(TermIEndDt1.replace('-', ' '));
                        dtToday = new Date(TodayDate.replace('-', ' '));
                    }
                    else {
                        dtStartDate = new Date(convertdate(document.getElementById(TermIIStDt).value));
                        dtEndDate = new Date(convertdate(document.getElementById(TermIEndDt).value));
                        dtToday = new Date(convertdate(TodayDate));
                    }
                    if (dtStartDate <= dtEndDate) {
                        TermStandard = document.getElementById(Std).innerHTML
                        sMsg = "1";
                        iRowNo += i.toString() + ", "
                        if (TermStd == "")
                            TermStd = TermStandard;
                        else
                            TermStd += ", " + TermStandard;
                    }
                }
            }
            if (iRowNo != "") {
                oSrc.errormessage = document.getElementById("<%=hidValTermIIStartDateShouldBeGreaterThanTermIEndDate.ClientID%>").value + " : " + TermStd + ".";
                document.getElementById(_clientcstTermDate).innerHTML = document.getElementById("<%=hidValTermIIStartDateShouldBeGreaterThanTermIEndDate.ClientID%>").value + " : " + TermStd + ".";
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
        ///////////////////////////////////////////////////////////////////////////////
        function Term1StDtValidationAsPerAcademicYrDates(oSrc, args) {
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = ""
            var iRowNo = ""
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            
            var TermStandard = "";
            var TermStd = "";
            var dtStartDate;
            var dtAcYrStartDate; var dtAcYrEndDate;
            var hidStDt;
            var hidEndDt;
            oSrc.errormessage = "";
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var TermIStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm1StartDate"
                hidStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "hidStartDate"
                hidEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "hidEndDate"
                var Std = _clientTermViewId + "_ctrl" + RowNumber + "_" + "lblStandardName"
                var TermIStDt1 = document.getElementById(TermIStDt).value

                hidStDt = document.getElementById(hidStDt).value
                hidEndDt = document.getElementById(hidEndDt).value

                if (document.all) {
                    hidStDt = new Date(hidStDt.replace('-', ' ').replace('-', ' '));
                    hidEndDt = new Date(hidEndDt.replace('-', ' ').replace('-', ' '));
                }
                else 
                {
                    hidStDt = new Date(convertdate(hidStDt));
                    hidEndDt = new Date(convertdate(hidEndDt));
                }

                var dthidStDt = new Date(hidStDt)
                var dthidEndDt = new Date(hidEndDt)
                var dthidStDate = getDateString1(dthidStDt);
                var dthidEndDate = getDateString1(dthidEndDt);
                if (TermIStDt1 != "") {
                    var dtStartDate;
                    if (document.all) {
                        dtStartDate = new Date(TermIStDt1.replace('-', ' ').replace('-', ' '));
                        dtAcYrStartDate = new Date(hidStDt);
                        dtAcYrEndDate = new Date(hidEndDt);
                    }
                    else {
                        dtStartDate = new Date(convertdate(document.getElementById(TermIStDt).value))
                        dtAcYrStartDate = new Date(convertdate(dthidStDate));
                        dtAcYrEndDate = new Date(convertdate(dthidEndDate));
                    }

                    if (dtStartDate < new Date(dtAcYrStartDate) || dtStartDate > new Date(dtAcYrEndDate)) {
                        TermStandard = document.getElementById(Std).innerHTML
                        sMsg = "1";
                        iRowNo += i.toString() + ", "
                        if (oSrc.errormessage == "" || oSrc.errormessage == undefined)
                            oSrc.errormessage = document.getElementById("<%=hidValTermIStartDateShouldBeInBetween.ClientID %>").value.replace("%dthidStDate%", dthidStDate).replace("%dthidEndDate%", dthidEndDate) + " " + document.getElementById("<%=hidForStandard.ClientID%>").value + TermStandard + ".";
                        else
                            oSrc.errormessage += " <br>" + document.getElementById("<%=hidValTermIStartDateShouldBeInBetween.ClientID %>").value.replace("%dthidStDate%", dthidStDate).replace("%dthidEndDate%", dthidEndDate) + " " + document.getElementById("<%=hidForStandard.ClientID%>").value + TermStandard + ".";                      
                    }
                }
            }
            if (iRowNo != "") {             
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
        ///Term1 End Date
        function Term1EndDtValidationAsPerAcademicYrDates(oSrc, args) {
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = ""
            var iRowNo = ""
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            
            var TermStandard = "";
            var TermStd = "";
            var dtEndDate;
            var dtAcYrEndDate; var dtAcYrStartDate;
            var hidEndDt; var hidStDt;
            oSrc.errormessage = "";
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var TermIEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm1EndDate"
                hidStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "hidStartDate"
                hidEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "hidEndDate"
                var Std = _clientTermViewId + "_ctrl" + RowNumber + "_" + "lblStandardName"

                var TermIEndDt1 = document.getElementById(TermIEndDt).value
                hidStDt = document.getElementById(hidStDt).value
                hidEndDt = document.getElementById(hidEndDt).value

                if (document.all) {
                    hidStDt = new Date(hidStDt.replace('-', ' ').replace('-', ' '));
                    hidEndDt = new Date(hidEndDt.replace('-', ' ').replace('-', ' '));
                }
                else {
                    hidStDt = new Date(convertdate(hidStDt));
                    hidEndDt = new Date(convertdate(hidEndDt));
                }

                var dthidStDt = new Date(hidStDt)
                var dthidEndDt = new Date(hidEndDt)

                var dthidStDate = getDateString1(dthidStDt);
                var dthidEndDate = getDateString1(dthidEndDt);
                if (TermIEndDt1 != "") {
                    if (document.all) {
                        dtEndDate = new Date(TermIEndDt1.replace('-', ' ').replace('-', ' '));
                        dtAcYrStartDate = new Date(hidStDt);
                        dtAcYrEndDate = new Date(hidEndDt);
                    }
                    else {
                        dtEndDate = new Date(convertdate(document.getElementById(TermIEndDt).value));
                        dtAcYrStartDate = new Date(convertdate(dthidStDate));
                        dtAcYrEndDate = new Date(convertdate(dthidEndDate));
                    }
                    if (dtEndDate > dtAcYrEndDate || dtEndDate < dtAcYrStartDate) {
                        TermStandard = document.getElementById(Std).innerHTML
                        sMsg = "1";
                        iRowNo += i.toString() + ", "
                        if (oSrc.errormessage == "" || oSrc.errormessage == undefined)
                            oSrc.errormessage = document.getElementById("<%=hidTermIEndDateShouldBeInBetween.ClientID%>").value.replace("%dthidStDate%", dthidStDate).replace("%dthidEndDate%", dthidEndDate) + " " + document.getElementById("<%=hidForStandard.ClientID%>").value + TermStandard + ".";
                        else
                            oSrc.errormessage += "<br>" + document.getElementById("<%=hidTermIEndDateShouldBeInBetween.ClientID%>").value.replace("%dthidStDate%", dthidStDate).replace("%dthidEndDate%", dthidEndDate) + " " + document.getElementById("<%=hidForStandard.ClientID%>").value + TermStandard + ".";
                    }
                }
            }
            if (iRowNo != "") {
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
        ////Term2 Start Date
        function Term2StDtValidationAsPerAcademicYrDates(oSrc, args) {

            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = ""
            var iRowNo = ""
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            
            var TermStandard = "";
            var TermStd = "";
            var dtStartDate2;
            var dtAcYrStartDate; var dtAcYrEndDate;
            var hidStDt; var hidEndDt;
            oSrc.errormessage = "";
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var TermIIStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm2StartDate"
                
                hidStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "hidStartDate"
                hidEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "hidEndDate"
                var Std = _clientTermViewId + "_ctrl" + RowNumber + "_" + "lblStandardName"

                var TermIIStDt2 = document.getElementById(TermIIStDt).value
                hidStDt = document.getElementById(hidStDt).value
                hidEndDt = document.getElementById(hidEndDt).value
               
                if (document.all) 
                {
                    hidStDt = new Date(hidStDt.replace('-', ' ').replace('-',' '));
                    hidEndDt = new Date(hidEndDt.replace('-', ' ').replace('-',' '));
                }
                else
                 {
                    hidStDt = new Date(convertdate(hidStDt));
                    hidEndDt = new Date(convertdate(hidEndDt));
                }
                var dthidStDt = new Date(hidStDt)
                var dthidEndDt = new Date(hidEndDt)
                var dthidStDate = getDateString1(dthidStDt);
                var dthidEndDate = getDateString1(dthidEndDt);
                if (TermIIStDt2 != "") 
                {
                    var dtStartDate;
                    if (document.all) {
                        dtStartDate2 = new Date(TermIIStDt2.replace('-', ' ').replace('-', ' '));
                        dtAcYrStartDate = new Date(hidStDt);
                        dtAcYrEndDate = new Date(hidEndDt);
                    }
                    else 
                    {
                        dtStartDate2 = new Date(convertdate(document.getElementById(TermIIStDt).value));
                        dtAcYrStartDate = new Date(convertdate(dthidStDate));
                        dtAcYrEndDate = new Date(convertdate(dthidEndDate));
                    }
                    if (dtStartDate2 < dtAcYrStartDate || dtStartDate2 > dtAcYrEndDate) {
                        TermStandard = document.getElementById(Std).innerHTML
                        sMsg = "1";
                        iRowNo += i.toString() + ", "
                        if (oSrc.errormessage == "" || oSrc.errormessage == undefined)

                            oSrc.errormessage = document.getElementById("<%=hidTermIIStartDateShouldBeInBetween.ClientID%>").value.replace("%dthidStDate%", dthidStDate).replace("%dthidEndDate%", dthidEndDate) + " " + document.getElementById("<%=hidForStandard.ClientID%>").value + TermStandard + ".";
                        else
                            oSrc.errormessage += "<br>" + document.getElementById("<%=hidTermIIStartDateShouldBeInBetween.ClientID%>").value.replace("%dthidStDate%", dthidStDate).replace("%dthidEndDate%", dthidEndDate) + " " + document.getElementById("<%=hidForStandard.ClientID%>").value + TermStandard + ".";
                    }
                }
            }
            if (iRowNo != "") {
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
        ///Ter2 End Date
        function Term2EndDtValidationAsPerAcademicYrDates(oSrc, args) {

            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = ""
            var iRowNo = ""
            document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
            
            var TermStandard = "";
            var TermStd = "";
            var dtEndDate2;
            var dtAcYrEndDate; var dtAcYrStartDate;
            var hidEndDt; var hidStDt;
            oSrc.errormessage = "";
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var TermIIEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "txtTerm2EndDate"
                hidStDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "hidStartDate"
                hidEndDt = _clientTermViewId + "_ctrl" + RowNumber + "_" + "hidEndDate"
                var Std = _clientTermViewId + "_ctrl" + RowNumber + "_" + "lblStandardName"

                var TermIIEndDt2 = document.getElementById(TermIIEndDt).value
                hidStDt = document.getElementById(hidStDt).value
                hidEndDt = document.getElementById(hidEndDt).value

                if (document.all) {
                    hidStDt = new Date(hidStDt.replace('-', ' ').replace('-', ' '));
                    hidEndDt = new Date(hidEndDt.replace('-', ' ').replace('-', ' '));
                }
                else {
                    hidStDt = new Date(convertdate(hidStDt));
                    hidEndDt = new Date(convertdate(hidEndDt));
                }
               
                var dthidStDt = new Date(hidStDt)
                var dthidEndDt = new Date(hidEndDt)
                var dthidStDate = getDateString1(dthidStDt);
                var dthidEndDate = getDateString1(dthidEndDt);
                //dthidStDate = "10-Jul-2010"
                if (TermIIEndDt2 != "") {
                    if (document.all) {
                        dtEndDate2 = new Date(TermIIEndDt2.replace('-', ' ').replace('-', ' '));
                        dtAcYrStartDate = new Date(hidStDt);
                        dtAcYrEndDate = new Date(hidEndDt);
                    }
                    else {
                        dtEndDate2 = new Date(convertdate(document.getElementById(TermIIEndDt).value));
                        dtAcYrStartDate = new Date(convertdate(dthidStDate));
                        dtAcYrEndDate = new Date(convertdate(dthidEndDate));
                    }
                    if (dtEndDate2 > dtAcYrEndDate || dtEndDate2 < dtAcYrStartDate) {
                        TermStandard = document.getElementById(Std).innerHTML
                        sMsg = "1";
                        iRowNo += i.toString() + ", "
                        
                        if (oSrc.errormessage == "" || oSrc.errormessage == undefined)
                            oSrc.errormessage = document.getElementById("<%=hidTermIIEndDateShouldBeInBetween.ClientID%>").value.replace("%dthidStDate%", dthidStDate).replace("%dthidEndDate%", dthidEndDate) + " " + document.getElementById("<%=hidForStandard.ClientID%>").value + TermStandard + ".";
                        else
                            oSrc.errormessage += "<br>" + document.getElementById("<%=hidTermIIEndDateShouldBeInBetween.ClientID%>").value.replace("%dthidStDate%", dthidStDate).replace("%dthidEndDate%", dthidEndDate) + " " + document.getElementById("<%=hidForStandard.ClientID%>").value + TermStandard + ".";
                      
                    }
                }
            }
            if (iRowNo != "") {
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

        function getDateString1(oDtobj) {
            
            var obj = new Date(oDtobj);
            var strDate = obj.getDate() + "-";
            var strMonth = parseInt(obj.getMonth()) + 1;
            strMonth = getMonthName1(strMonth);
            strDate = strDate + strMonth + "-";
            strDate = strDate + obj.getFullYear();
            return strDate;
        }

        function getMonthName1(month) {
            switch (month) {
                case 1:
                    return "Jan";
                    break;

                case 2:
                    return "Feb";
                    break;

                case 3:
                    return "Mar";
                    break;

                case 4:
                    return "Apr";
                    break;

                case 5:
                    return "May";
                    break;

                case 6:
                    return "Jun";
                    break;

                case 7:
                    return "Jul";
                    break;

                case 8:
                    return "Aug";
                    break;

                case 9:
                    return "Sep";
                    break;

                case 10:
                    return "Oct";
                    break;

                case 11:
                    return "Nov";
                    break;

                case 12:
                    return "Dec";
                    break;
            }
        }
        


    </script>
</asp:Content>
