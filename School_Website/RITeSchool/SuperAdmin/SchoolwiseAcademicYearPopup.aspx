<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchoolwiseAcademicYearPopup.aspx.cs"
    Inherits="SchoolwiseAcademicYearPopup" MasterPageFile="../MasterPages/PopupMasterSml.master" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div align="center" class="MainBodyDiv">
            <table width="97%" cellpadding="0" cellspacing="2" style="vertical-align: top;">
            <tr>
                <td colspan="2">
                    <table width="100%" align="center">
                        <tr>
                            <td class="ClsGrayMainTitle" style="height: 20px;">
                                <asp:Label ID="lblAddAcademicYear" runat="server" CssClass="ClsMainTitleHead" Font-Bold="True"
                                    Text="<%$ Resources:LocalizedResources, AddAcademicYear %>" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <div style="text-align: right; float: right">
                        <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* " ForeColor="Red"
                            EnableViewState="false">
                        </asp:Label>
                        <asp:Label ID="Label1" runat="server" CssClass="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                    </div>
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="LblErrorMsg"
                        ValidationGroup="Save" ShowMessageBox="false" ShowSummary="true" />
                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblNorecord" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                        Visible="False" EnableViewState="false" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table style="width: 100%">
                        <tr>
                            <td class="ClsBorderlight" colspan="2" style= "width:100%">
                                <asp:GridView ID="grdvwStandard" runat="server" Width="100%" AutoGenerateColumns="False"
                                    AllowSorting="false" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                    BackColor="White" CssClass="GridBorder" AllowPaging="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataText="No Record Found" DataKeyNames="StandardwiseAcademicYearId,StandardId">
                                    <Columns>
                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Standard %>" DataField="StandardName">
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingR" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, StartDate %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                   Text='<%# Convert.ToDateTime(Eval("StartDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'   
                                                   TabIndex="9" ></asp:TextBox> 
                                                <rjs:PopCalendar ID="cStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                  Culture="en"  ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, EndDate%>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="SmlTxtBox" MaxLength="11" TabIndex="9"
                                                    Text='<%# Convert.ToDateTime(Eval("EndDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'>
                                                    </asp:TextBox>
                                                <rjs:PopCalendar ID="cEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy" 
                                                   Culture="en"  ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, SchoolReopeningDate %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReopeningDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                    TabIndex="9" Text='<%# Convert.ToDateTime(Eval("SchoolReopeningDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'>                                                   ></asp:TextBox>
                                                <rjs:PopCalendar ID="cReopningDate" runat="server" Control="txtReopeningDate" Format="dd MMM yyyy"
                                                   Culture="en"  ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle Width="28%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="ClsGridRow" />
                                    <HeaderStyle CssClass="ClsGridHeader" />
                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="ClsBorderlight" colspan="2">
                                            <tr>
                                                <td class="ClsBorderlight" style="width: 144px">
                                                    <span id="lblIsCurrentYear" class="ClsLabel">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, CurrentAcademicYear %>"></asp:Label>
                                                        <span class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkIsCurrentYear" runat="server" Text="" TabIndex="3" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" style="width: 144px">
                                                    <span id="lblIsClosedYear" class="ClsLabel">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, CloseAcademicYear %>"></asp:Label>
                                                        <span class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkIsClosedYear" runat="server" Text="" TabIndex="4" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" style="width: 144px">
                                                    <span id="lblNewAcaYear" class="ClsLabel">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, NewAcademicYear %>"></asp:Label>
                                                        <span class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkIsNewYear" runat="server" Text="" TabIndex="5" Enabled="False" />
                                                </td>
                                            </tr>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="ClsBtn"
                                    ValidationGroup="Save" Text="<%$ Resources:LocalizedResources, Save %>" OnClick="btnSave_Click"
                                    TabIndex="6" UseSubmitBehavior="false" />
                                <asp:Button ID="btnBack" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                    Text="<%$ Resources:LocalizedResources, Close %>" TabIndex="7" UseSubmitBehavior="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnPreviewStudentList" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                    Text="<%$ Resources:LocalizedResources, PreviewOutofAcademicYearStudents %>"
                                    TabIndex="8" Width="270px" OnClick="btnPreviewStudentList_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:HiddenField ID="hidActionFlag" runat="server" />
                                <asp:HiddenField ID="hidAcademicYearId" runat="server" />
                                <asp:CustomValidator ID="cstStartDate" runat="server" ClientValidationFunction="ValidateStartDateEndDate"
                                    ValidationGroup="Save" SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                <asp:CustomValidator ID="cstEndDate" runat="server" ClientValidationFunction="ValidateStartDateEndDate"
                                    ValidationGroup="Save" SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                <asp:CustomValidator ID="cstReopeningDate" runat="server" ClientValidationFunction="ValidateReopeningDate"
                                    ValidationGroup="Save" SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                    <asp:HiddenField ID="hidValAttendenceDeleted" runat="server" />
                    <asp:HiddenField ID="hidStartDateShouldNotBeBlankForRowNumber" runat="server"  />
                    <asp:HiddenField ID="hidStartDateShouldBeLessThanEndDateForRowNumber" runat="server"  />
                    <asp:HiddenField ID="hidSchoolReopeningDateShouldNotBeBlankForRowNumber" runat="server"  />
                    <asp:HiddenField ID="hidEndDateShouldNotBeBlankForRowNumber" runat="server"  />
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _clientoptchkIsCurrentYear = "<%=this.chkIsCurrentYear.ClientID %>"
        _clientoptchkIsClosedYear = "<%=this.chkIsClosedYear.ClientID %>"
        _clientoptchkIsNewYear = "<%=this.chkIsNewYear.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnBack = "<%=this.btnBack.ClientID %>"
        _clientgrdvwStandard = "<%=this.grdvwStandard.ClientID %>"
        _ClientcstStartDate = "<%=this.cstStartDate.ClientID %>"
        _ClientcstEndDate = "<%=this.cstEndDate.ClientID %>"
        _ClientcstReopeningDate = "<%=this.cstReopeningDate.ClientID %>"
        _ClientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _ClientlblNoRecord = "<%=this.lblNorecord.ClientID %>"

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
        function closewindow() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnBack).disabled = true
            window.close()
        }
        function ClearMessage() {
            if (document.getElementById(_ClientlblNoRecord) != null)
                document.getElementById(_ClientlblNoRecord).innerHTML = "";
        }
        function ValidateStartDateEndDate(oSrc, args) {

            if (document.getElementById(_ClientlblNoRecord) != null)
                document.getElementById(_ClientlblNoRecord).innerHTML = "";
            document.getElementById(_ClientlblErrorMsg).innerHTML = "";
            var iRowCount = 2;
            var iRowId = 2
            var sStartDateBlankMsg = "";
            var sEndDateBlankMsg = "";
            var txtStartDate
            var txtEndDate

            if (iRowCount < 10)
                txtStartDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowCount + "_txtStartDate")
            else
                txtStartDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowCount + "_txtStartDate")

            if (iRowId < 10)
                txtEndDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowId + "_txtEndDate")
            else
                txtEndDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowId + "_txtEndDate")



            while (txtStartDate != null) {
                if (txtStartDate.value == "")
                    sStartDateBlankMsg = sStartDateBlankMsg + ", " + (iRowCount - 1);
                iRowCount = iRowCount + 1

                if (iRowCount < 10)
                    txtStartDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowCount + "_txtStartDate")
                else
                    txtStartDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowCount + "_txtStartDate")
            }

            while (txtEndDate != null) {
                if (txtEndDate.value == "")
                    sEndDateBlankMsg = sEndDateBlankMsg + ", " + (iRowId - 1);
                iRowId = iRowId + 1
                if (iRowId < 10)

                    txtEndDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowId + "_txtEndDate")
                else
                    txtEndDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowId + "_txtEndDate")
            }

            if (sStartDateBlankMsg != "" || sEndDateBlankMsg != "") {

                if (sStartDateBlankMsg != "") {
                    sStartDateBlankMsg = sStartDateBlankMsg.substring(1)
                    $get(_ClientcstStartDate).errormessage =document.getElementById("<%=this.hidStartDateShouldNotBeBlankForRowNumber.ClientID %>").value + sStartDateBlankMsg + "."
                }
                if (sEndDateBlankMsg != "") {
                    sEndDateBlankMsg = sEndDateBlankMsg.substring(1)
                    $get(_ClientcstEndDate).errormessage = document.getElementById("<%=this.hidEndDateShouldNotBeBlankForRowNumber.ClientID %>").value + sEndDateBlankMsg + "."
                }
                args.IsValid = false
                return true
            }
            else {
                iRowCount = 2
                var ErrorMessage = ""
                if (iRowCount < 10)
                    txtStartDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowCount + "_txtStartDate")
                else
                    txtStartDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowCount + "_txtStartDate")

                if (iRowCount < 10)
                    txtEndDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowCount + "_txtEndDate")
                else
                    txtEndDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowCount + "_txtEndDate")

                var EndDate;
                var Startdate;



                while (txtStartDate != null && txtEndDate != null) {

                    if (document.all) {
                        EndDate = new Date(txtEndDate.value.replace('-', ' '));
                        Startdate = new Date(txtStartDate.value.replace('-', ' '));
                    }
                    else {
                        EndDate = new Date(convertdate(txtEndDate.value));
                        Startdate = new Date(convertdate(txtStartDate.value));
                    }
                    if (EndDate < Startdate)
                        ErrorMessage = ErrorMessage + ", " + (iRowCount - 1)
                    iRowCount = iRowCount + 1

                    if (iRowCount < 10)
                        txtStartDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowCount + "_txtStartDate")
                    else
                        txtStartDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowCount + "_txtStartDate")

                    if (iRowCount < 10)
                        txtEndDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowCount + "_txtEndDate")
                    else
                        txtEndDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowCount + "_txtEndDate")
                }

                if (ErrorMessage != "") {
                    ErrorMessage = ErrorMessage.substring(1)
                    $get(_ClientcstStartDate).errormessage = document.getElementById("<%=this.hidStartDateShouldBeLessThanEndDateForRowNumber.ClientID %>").value + ErrorMessage + "."
                    args.IsValid = false
                    return true
                }

            }

            args.IsValid = true
            return false
        }


        function ValidateReopeningDate(oSrc, args) {
            if (document.getElementById(_ClientlblNoRecord) != null)
                document.getElementById(_ClientlblNoRecord).innerHTML = "";
            document.getElementById(_ClientlblErrorMsg).innerHTML = "";
            var iRowCount = 2;

            var sStartDateBlankMsg = "";

            var txtReopningDate



            if (iRowCount < 10)
                txtReopningDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowCount + "_txtReopeningDate")
            else
                txtReopningDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowCount + "_txtReopeningDate")



            while (txtReopningDate != null) {
                if (txtReopningDate.value == "")
                    sStartDateBlankMsg = sStartDateBlankMsg + ", " + (iRowCount - 1);
                iRowCount = iRowCount + 1

                if (iRowCount < 10)
                    txtReopningDate = document.getElementById(_clientgrdvwStandard + "_ctl0" + iRowCount + "_txtReopeningDate")
                else
                    txtReopningDate = document.getElementById(_clientgrdvwStandard + "_ctl" + iRowCount + "_txtReopeningDate")
            }



            if (sStartDateBlankMsg != "") {

                if (sStartDateBlankMsg != "") {
                    sStartDateBlankMsg = sStartDateBlankMsg.substring(1)
                    $get(_ClientcstReopeningDate).errormessage = document.getElementById("<%=this.hidSchoolReopeningDateShouldNotBeBlankForRowNumber.ClientID %>").value + sStartDateBlankMsg + "."
                }

                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }


        function ConfirmAction() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function')
                isPageValid = Page_ClientValidate()
            if (isPageValid) {

                if (window.confirm(document.getElementById("<%=this.hidValAttendenceDeleted.ClientID %>").value)) {
                    DisableButtons();
                    return true;
                }
                else return false;
            }
        }



        function DisableButtons() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnBack).disabled = true
        }
        function DisableCurrentYearIfCloseYearChecked() {
            if (document.getElementById(_clientoptchkIsClosedYear).checked == true) {
                document.getElementById(_clientoptchkIsCurrentYear).checked = false
                document.getElementById(_clientoptchkIsCurrentYear).disabled = true
                document.getElementById(_clientoptchkIsNewYear).checked = false
                document.getElementById(_clientoptchkIsNewYear).disabled = true
            }
            else
                document.getElementById(_clientoptchkIsCurrentYear).disabled = false
        }
        function DisableCloseYearIfCurrentYearChecked() {
            if (document.getElementById(_clientoptchkIsCurrentYear).checked == true) {
                document.getElementById(_clientoptchkIsClosedYear).checked = false
                document.getElementById(_clientoptchkIsClosedYear).disabled = true
                document.getElementById(_clientoptchkIsNewYear).checked = false
                document.getElementById(_clientoptchkIsNewYear).disabled = true
            }
            else
                document.getElementById(_clientoptchkIsClosedYear).disabled = false
        }
    </script>
</asp:Content>
