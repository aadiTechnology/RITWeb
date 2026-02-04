<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChequeClearancePopup.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master" Inherits="ChequeClearancePopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top">
        <table width="100%" align="center">
            <tr>
                <td align="left" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <%--<asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Text="Cheque Clearance Details"
                                    Font-Bold="True" EnableViewState="False"></asp:Label>--%>
                                    <span class="MainTitleHead" style="font-weight:bold">Cheque Clearance Details</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" style="color: #ff3333" valign="top">
                    <%--<asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                        ForeColor="Red" EnableViewState="false"></asp:Label>--%>
                        <span class="ClsMdtStar">* Mandatory Fields </span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="Please fix following error(s)"
                        runat="server" />
                    <asp:CustomValidator ID="cstClearanceDate" runat="server" Display="none" EnableClientScript="true"
                        ClientValidationFunction="ValidateClearanceDate" ErrorMessage="Cheque clearance date should not be blank."></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <%--<asp:Label ID="lblStudentDetails" runat="server" BorderWidth="0px" Font-Bold="True"
                        Text="Student Details" CssClass="ClsLblLgnd" Width="200px" EnableViewState="false"></asp:Label>--%>
                        <span class="ClsLblLgnd" style="font-weight:bold;width:200px">Student Details</span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
                        <tr>
                            <td align="left" style="width: 20%; height: 1px" class="ClsBorderlight">
                                <%--<asp:Label ID="Label" runat="server" CssClass="ClsLabel" 
                                    Text="Registration Number :" EnableViewState="False" />--%>
                                    <span class="ClsLabel" > Registration Number :</span>
                            </td>
                            <td align="left" style="width: 20%; height: 1px" class="ClsBorderlight">
                                <asp:Label ID="lblRegNo" runat="server" CssClass="ClsLblRslt" EnableViewState="true"></asp:Label>
                            </td>
                            <td align="left" class="ClsBorderlight" style="width: 20%; height: 1px">
                                <%--<asp:Label ID="Label6" runat="server" CssClass="ClsLabel" Text="Student Class :"
                                    EnableViewState="False"></asp:Label>--%>
                                    <span class="ClsLabel" > Student Class :</span>
                            </td>
                            <td class="ClsBorderlight" style="width: 20%; height: 1px">
                                <asp:Label ID="lblStudentClass" runat="server" CssClass="ClsLblRslt" EnableViewState="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <%--<asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Student Name :" 
                                    EnableViewState="False" />--%>
                                    <span class="ClsLabel" > Student Name :</span>
                            </td>
                            <td class="ClsBorderlight" colspan="2">
                                <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLblRslt" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <%--<asp:Label ID="Label13" runat="server" BorderWidth="0px" Font-Bold="True" Text="Cheque Details"
                        CssClass="ClsLblLgnd" Width="200px" EnableViewState="false"></asp:Label>--%>
                        <span class="ClsLblLgnd" style="font-weight:bold;width:200px">Cheque Details</span>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
                        <tr>
                            <td align="left" style="width: 20%; height: 1px" class="ClsBorderlight">
                                <%--<asp:Label ID="label11" runat="server" CssClass="ClsLabel" 
                                    Text="Cheque Number :" EnableViewState="False" />--%>
                                    <span class="ClsLabel" > Cheque Number :</span>
                            </td>
                            <td align="left" style="width: 20%; height: 1px" class="ClsBorderlight">
                                <asp:Label ID="lblChequeNumber" runat="server" CssClass="ClsLblRslt" EnableViewState="true"></asp:Label>
                            </td>
                            <td align="left" class="ClsBorderlight" style="width: 20%; height: 1px">
                                <%--<asp:Label ID="Label10" runat="server" CssClass="ClsLabel" Text="Cheque Date :" EnableViewState="False"></asp:Label>--%>
                                <span class="ClsLabel" > Cheque Date :</span>
                            </td>
                            <td align="left" class="ClsBorderlight" style="width: 20%; height: 1px">
                                <asp:Label ID="lblChequeDate" runat="server" CssClass="ClsLblRslt" EnableViewState="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <%--<asp:Label ID="Label12" runat="server" CssClass="ClsLabel" Text="Bank Name :" 
                                    EnableViewState="False" />--%>
                                    <span class="ClsLabel" > Bank Name :</span>
                            </td>
                            <td class="ClsBorderlight" colspan="2">
                                <asp:Label ID="lblBankName" runat="server" CssClass="ClsLblRslt" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%-- <tr>
                <td align="left">
                    <table>
                        <tr>
                            <td runat="server" class="ClsHilightText">
                                <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Font-Bold="True" Text="Cheque Number :"
                                    EnableViewState="False"></asp:Label></td>
                            <td align="left" class="ClsHilightBGB" style="padding-right: 10px">
                                <asp:Label ID="lblChequeNumber" runat="server" CssClass="ClsLabel" EnableViewState="true"></asp:Label></td>
                            <td class="ClsHilightText">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Font-Bold="True" Text="Cheque Date :"
                                    EnableViewState="false"></asp:Label></td>
                            <td align="left" class="ClsHilightBGB" style="padding-right: 10px">
                                <asp:Label ID="lblChequeDate" runat="server" CssClass="ClsLabel" EnableViewState="true"></asp:Label></td>
                            <td runat="server" class="ClsHilightText">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Font-Bold="True" Text="Bank Name :"
                                    EnableViewState="False"></asp:Label></td>
                            <td align="left" class="ClsHilightBGB">
                                <asp:Label ID="lblBankName" runat="server" CssClass="ClsLabel" EnableViewState="true"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <tr>
                <td align="center">
                    <table width="100%" align="center">
                        <tr>
                            <td colspan="2">
                                <table align="center" cellpadding="1" cellspacing="2">
                                    <tr>
                                        <td valign="top" class="ClsBorderlight">
                                            <%--<asp:Label ID="lbl1" runat="server" CssClass="ClsLabel" Text="Cheque Clearance Date :"
                                                EnableViewState="False"></asp:Label>--%>
                                                <span class="ClsLabel" > Cheque Clearance Date :</span>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:TextBox ID="txtClearanceDate" runat="server" CssClass="SmlTxtBox" MaxLength="50"
                                                TabIndex="1" EnableViewState="true"></asp:TextBox>
                                            <rjs:PopCalendar ID="calClearanceDate" runat="server" Control="txtClearanceDate"
                                                Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Cheque date should not be blank." />
                                            <asp:Label ID="Label17" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="right">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" TabIndex="2"
                                                OnClick="btnSave_Click" UseSubmitBehavior="false" />
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Button ID="btnClose" Text="close" CssClass="ClsBtn" runat="server" CausesValidation="false"
                                                OnClick="btnClose_Click" TabIndex="3" />
                                            <asp:Button ID="btnRemove" runat="server" Text="Remove Date" CssClass="ClsBtn" TabIndex="5"
                                                UseSubmitBehavior="false" OnClick="btnRemove_Click" 
                                                CausesValidation="false" Width="100px" /> 
                                        </td>
                                        <td valign="top" align="left">
                                            <%--<asp:Button ID="btnRemove" runat="server" Text="Remove Date" CssClass="ClsBtn" TabIndex="1"
                                                UseSubmitBehavior="false" onclick="btnRemove_Click" CausesValidation="false"/>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:HiddenField ID="hidPDCId" runat="server" />
                                <asp:HiddenField ID="hidStudentId" runat="server" />
                                <asp:HiddenField ID="hidServerDate" runat="server" />
                                <asp:HiddenField ID="hidCategoryName" runat="server" />
                            </td>
                            <td>
                                <asp:HiddenField ID="hidCategoryValue" runat="server" />
                                <asp:HiddenField ID="hidIncludeChequeFlag" runat="server" />
                                <asp:HiddenField ID="hidPageIndex" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientvalSumErrorMsgId = "<%=this.valSumErrorMsg.ClientID %>"
        _clienttxtClearanceDateId = "<%=this.txtClearanceDate.ClientID %>"
        _clientcstClearanceDate = "<%=this.cstClearanceDate.ClientID %>"
        _clientServerDate = "<%=this.hidServerDate.ClientID %>"
        _clientlblChequeDate = "<%=this.lblChequeDate.ClientID %>"
        _clientbtnRemove = "<%=this.btnRemove.ClientID %>"
        function ValidateClearanceDate(source, args) {
            var bIsValid = true
            if (document.getElementById(_clienttxtClearanceDateId).value == "") {
                document.getElementById(_clientcstClearanceDate).errormessage =
"Cheque clearance date should not be blank."
                bIsValid = false
            }
            else if (document.getElementById(_clienttxtClearanceDateId).value != "") {
                var serverDate
                var chequeDate
                var clearanceDate
                if (document.all) {
                    serverDate = new Date((document.getElementById(_clientServerDate).value).replace('-', ' '))
                    chequeDate = new Date((document.getElementById(_clientlblChequeDate).innerHTML).replace('-', ' '))
                    clearanceDate = new Date((document.getElementById(_clienttxtClearanceDateId).value).replace('-', ' '))
                }
                else {
                    serverDate = new Date((document.getElementById(_clientServerDate).value))
                    chequeDate = new Date(convertdate(document.getElementById(_clientlblChequeDate).innerHTML))
                    clearanceDate = new Date(convertdate(document.getElementById(_clienttxtClearanceDateId).value))
                }
                var today = new Date(serverDate)
                if (today < clearanceDate) {
                    document.getElementById(_clientcstClearanceDate).errormessage =
"Cheque clearance date should not be future date."
                    bIsValid = false
                }
                else if (chequeDate > clearanceDate) {
                    document.getElementById(_clientcstClearanceDate).errormessage =
"Cheque clearance date should be greater than cheque date."
                    bIsValid = false
                } 
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        function ClearErrorLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            if (isPageValid) {
                document.getElementById(_clientbtnSave).disabled = true
            } 
        }
        function ClearErrorLabel() {
            if (document.getElementById(_clientvalSumErrorMsgId) != null)
                document.getElementById(_clientvalSumErrorMsgId).style.display = "none"
            return true
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
        function ConfirmDelete() {
            var bResult = true
            var msg = ""
            msg = 'Are you sure you want to remove cheque clearance date?'
            if (!window.confirm(msg)) {
                bResult = false
            }
            return bResult
        }
    </script>
</asp:Content>
