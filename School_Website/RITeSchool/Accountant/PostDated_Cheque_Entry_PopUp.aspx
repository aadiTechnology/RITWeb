<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostDated_Cheque_Entry_PopUp.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master" Inherits=" PostDated_Cheque_Entry_PopUp" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        vertical-align: top">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%; height: 100%">
                    <tr>
                        <td style="height: 19px" align="left" colspan="6" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td class="ClsGrayMainTitle" style="height: 20px; width: 99%;">
                                        <%--<asp:Label ID="lblHeader" Text="Post dated Cheque(s) Management" runat="server" CssClass="MainTitleHead"
                                            Font-Bold="True" EnableViewState="false"></asp:Label>--%>
                                        <span class="MainTitleHead" style="font-weight: bold">
                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, PostDatedChequeManagement%>"></asp:Label></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" style="height: 19px;">
                            &nbsp;<asp:ValidationSummary runat="server" ValidationGroup="Save" ID="valChequeData"
                                ShowMessageBox="false" ShowSummary="true" />
                        </td>
                        <td>
                            <asp:CustomValidator ValidationGroup="Save" ID="cstChequeNo" runat="server" CssClass="ClsMdtStar"
                                Display="None" EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateChequeNumber"
                                ErrorMessage="Error msg"></asp:CustomValidator>
                            <asp:CustomValidator ID="cst_ChequeDate" runat="server" ClientValidationFunction="cstStartDate"
                                ValidationGroup="Save" Display="None" Visible="true" SetFocusOnError="True" ErrorMessage="Cheque date."></asp:CustomValidator>
                            <asp:CustomValidator ValidationGroup="Save" ID="cstChequeAmt" runat="server" CssClass="ClsMdtStar"
                                Display="None" EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateChequeAmt"
                                ErrorMessage="Error msg"></asp:CustomValidator>
                            <asp:CustomValidator ValidationGroup="Save" ID="cstBankName" runat="server" CssClass="ClsMdtStar"
                                Display="None" EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateBankName"
                                ErrorMessage="Error msg"></asp:CustomValidator>
                            <%--<asp:CustomValidator ValidationGroup="Save" ID="cstRemarks" runat="server" CssClass="ClsMdtStar"
                                Display="None" EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateRemarks"
                                ErrorMessage="Error msg"></asp:CustomValidator>--%>
                        </td>
                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                            <span class="ClsMdtStar">* <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel runat="server" ID="pnlupdate">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblErrMsg" runat="server" Visible="False" CssClass="LblErrorMsg"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trChequeEntry" runat="server">
                                <td style="background-color: white" id="Td1" align="center" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                        <tr>
                                            <td align="right" valign="top" class="ClsBorderlight">
                                                <%--<asp:Label ID="Label1" runat="server" Text="Cheque Number :" CssClass="ClsLabel" EnableViewState="false"></asp:Label>--%>
                                                <span class="ClsLabel">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, ChequeNumber%>"></asp:Label>
                                                    <span id="Span2" class="colonPadding">:</span></span>
                                            </td>
                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                                <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="SmlTxtBox" MaxLength="6"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;" TabIndex="1"></asp:TextBox>&nbsp;
                                                <%--<asp:Label ID="Label9" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>--%>
                                                <span class="ClsMdtStar">*</span>&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top" class="ClsBorderlight">
                                                <%--<asp:Label ID="Label8" runat="server" Text="Cheque Date :" CssClass="ClsLabel" EnableViewState="false"></asp:Label>--%>
                                                <span class="ClsLabel">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, ChequeDate%>"></asp:Label>
                                                    <span id="Span1" class="colonPadding">:</span></span>
                                            </td>
                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                                <asp:TextBox ID="txtChequeDate" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"
                                                    TabIndex="2"></asp:TextBox>
                                                <rjs:PopCalendar ID="cal_ChequeDate" runat="server" Control="txtChequeDate" Format="dd MMM yyyy"
                                                    Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, ChequeDateShouldNotBeBlank%>" />
                                                <%--<asp:Label ID="Label4" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>--%>
                                                <span class="ClsMdtStar">*</span>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top" class="ClsBorderlight">
                                                <%--<asp:Label ID="Label14" runat="server" Text="Cheque Amount :" CssClass="ClsLabel" EnableViewState="false"></asp:Label>--%>
                                                <span class="ClsLabel">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, ChequeAmount%>"></asp:Label>
                                                    <span id="Span3" class="colonPadding">:</span></span>
                                            </td>
                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px">
                                                <asp:TextBox ID="txtChequeAmt" runat="server" CssClass="SmlTxtBox" MaxLength="6"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" TabIndex="3"></asp:TextBox>&nbsp;
                                                <%--<asp:Label ID="Label15" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*" EnableViewState="false"></asp:Label>--%>
                                                <span class="ClsMdtStar">*</span>&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 19px" align="right" valign="top" class="ClsBorderlight">
                                                <%--<asp:Label ID="Label2" runat="server" Text="Bank Name :" CssClass="ClsLabel" EnableViewState="false"></asp:Label>--%>
                                                <span class="ClsLabel">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, BankName%>"></asp:Label>
                                                    <span id="Span4" class="colonPadding">:</span></span>
                                            </td>
                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px">
                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="LrgCombo" TabIndex="4">
                                                </asp:DropDownList>
                                                <%--<asp:TextBox ID="txtBankName" runat="server" CssClass="SmlTxtBox" MaxLength="50"
                                                    TabIndex="4" Width="400px"></asp:TextBox>--%>
                                                &nbsp;<%--<asp:Label ID="Label10" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                                    Text="*" EnableViewState="false"></asp:Label>--%><span class="ClsMdtStar">*</span>&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 19px" align="right" valign="top" class="ClsBorderlight">
                                                <%--<asp:Label ID="Label11" runat="server" Text="Remarks :" CssClass="ClsLabel" EnableViewState="false"></asp:Label>--%>
                                                <span class="ClsLabel">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, Remarks%>"></asp:Label>
                                                    <span id="Span5" class="colonPadding">:</span> </span>
                                            </td>
                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px">
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="SmlTxtBox" MaxLength="50" TabIndex="5"
                                                    Width="400px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:HiddenField ID="hidYearEndDate" runat="server" />
                                                <asp:HiddenField ID="hidYearStartDate" runat="server" />
                                                <asp:HiddenField ID="hidStudentId" runat="server" />
                                                <asp:HiddenField ID="hidMode" runat="server" Value="New" />
                                                <asp:HiddenField ID="hidPostdatedChequeId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                <asp:HiddenField ID="hidServerDate" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                    CssClass="ClsBtnMid" OnClick="btnSave_Click" ValidationGroup="Save" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                                                    CausesValidation="False" CssClass="ClsBtnMid" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trGrdCheque" runat="server">
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="grdPostDatedCheque" CssClass="GridBorder" runat="server" AutoGenerateColumns="False"
                                                    AllowSorting="true" Height="100%" PageSize="110" CellPadding="0" CellSpacing="1"
                                                    ForeColor="#333333" GridLines="None" DataKeyNames="Postdated_Cheque_Id,Status,Is_Cheque_Bounce"
                                                    Width="99%" BackColor="White" OnRowDataBound="grdPostDatedCheque_RowDataBound"
                                                    OnRowCommand="grdPostDatedCheque_RowCommand" OnRowCreated="grdPostDatedCheque_RowCreated"
                                                    OnSorting="grdPostDatedCheque_Sorting" EmptyDataText="<%$ Resources:LocalizedResources, ChequesAreNotAvailable%>">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, ChequeNo%>" DataField="Cheque_Number"
                                                            SortExpression="Cheque_Number">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, ChequeDate%>" SortExpression="Cheque_Date"
                                                            DataField="Cheque_Date" DataFormatString="{0:dd MMM yyyy}">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, ChequeAmount%>" SortExpression="Cheque_Amount"
                                                            DataField="Cheque_Amount">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, BankName%>" SortExpression="Bank_Name"
                                                            DataField="Bank_Name">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Status%>" SortExpression="Status"
                                                            DataField="Status">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Edit_Cheque" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            HeaderText="<%$ Resources:LocalizedResources, Edit%>" Text="<%$ Resources:LocalizedResources, Edit%>">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField ButtonType="Image"  CommandName="Delete_Cheque" HeaderText="<%$ Resources:LocalizedResources, Delete%>"
                                                            Text="<%$ Resources:LocalizedResources, Delete%>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                        </asp:ButtonField>
                                                    </Columns>
                                                    <RowStyle CssClass="ClsGridAltRow" />
                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                    <AlternatingRowStyle CssClass="ClsGridRow" />
                                                    <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hidCultureInfo" runat="server" />
                        <asp:HiddenField ID="hidChequeAmountShouldNotBeBlank" runat="server" />
                        <asp:HiddenField ID="hidChequeAmountShouldBeGreaterThanZero" runat="server"  />
                        <asp:HiddenField ID="hidChequeNumberShouldNotBeBlank" runat="server"  />
                        <asp:HiddenField ID="hidBankNameShouldBeSelected" runat="server"  />
                        <asp:HiddenField ID="hidChequeDateShouldNotBeBlank" runat="server"  />
                        <asp:HiddenField ID="hidValChequeDate" runat="server"  />
                        <asp:HiddenField ID="hidAnd" runat="server"  />
                        <asp:HiddenField ID="hidValDeleteChequeDetails" runat="server"  />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnClose" Text="<%$ Resources:LocalizedResources, Close%>" CssClass="ClsBtnMid"
                    runat="server" CausesValidation="false" OnClick="btnClose_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;<br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>"
        _clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>"
        _clientCstStartDate = "<%=this.cst_ChequeDate.ClientID %>"
        _clientcalStartDateID = "<%=this.txtChequeDate.ClientID %>"
        _clienttxtBankNameID = "<%=this.ddlBankName.ClientID %>"
        _clienttxtChequeNumberID = "<%=this.txtChequeNumber.ClientID %>"
        _clienttxtChequeAmtID = "<%=this.txtChequeAmt.ClientID %>"
        _clienttxtRemarksID = "<%=this.txtRemarks.ClientID %>"
        _clientcstChequeAmtID = "<%=this.cstChequeAmt.ClientID %>"
        _clientcstBankNameID = "<%=this.cstBankName.ClientID %>"
        _clientcstChequeNoID = "<%=this.cstChequeNo.ClientID %>"
        _clientServerDate = "<%=this.hidServerDate.ClientID %>"
        _clientValSummary = "<%=this.valChequeData.ClientID %>"
        function ValidateChequeAmt(aSrc, args) {
            if (document.getElementById(_clienttxtChequeAmtID).value == "") {
                document.getElementById(_clientcstChequeAmtID).errormessage = document.getElementById("<%=this.hidChequeAmountShouldNotBeBlank.ClientID %>").value
                args.IsValid = false
                return true
            }
            else if (document.getElementById(_clienttxtChequeAmtID).value == parseInt(0)) {
                document.getElementById(_clientcstChequeAmtID).errormessage = document.getElementById("<%=this.hidChequeAmountShouldBeGreaterThanZero.ClientID %>").value
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function ValidateChequeNumber(aSrc, args) {
            if (document.getElementById(_clienttxtChequeNumberID).value == "") {
                document.getElementById(_clientcstChequeNoID).errormessage = document.getElementById("<%=this.hidChequeNumberShouldNotBeBlank.ClientID %>").value
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function HideValSummary() {
            valSum = document.getElementById(_clientValSummary)
            if (valSum != null)
                document.getElementById(_clientValSummary).style.display = 'none'
        }
        function ValidateBankName(aSrc, args) {
            if (document.getElementById(_clienttxtBankNameID).value == "0") {
                document.getElementById(_clientcstBankNameID).errormessage = document.getElementById("<%=this.hidBankNameShouldBeSelected.ClientID %>").value
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function cstStartDate(aSrc, args) {
            var dtEndDate, dtStartDate
            var strStartDate = document.getElementById(_clientcalStartDateID).value
            if (document.getElementById(_clientcalStartDateID).value == "") {
                document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=this.hidChequeDateShouldNotBeBlank.ClientID %>").value
                args.IsValid = false
                return true
            }
            else if (!(CheckIfDateInAcademicYear(dtStartDate))) {
                var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value)
                var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value)
                var strStartYear = getDateString(dtYearStartDate)
                var strEndYear = getDateString(dtYearEndDate)
                document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=this.hidValChequeDate.ClientID %>").value + strStartYear + " " + document.getElementById("<%=this.hidAnd.ClientID %>").value + " " + strEndYear + ")."
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
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidValDeleteChequeDetails.ClientID %>").value)) {
                bResult = false
            }
            return bResult
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

    </script>
</asp:Content>
