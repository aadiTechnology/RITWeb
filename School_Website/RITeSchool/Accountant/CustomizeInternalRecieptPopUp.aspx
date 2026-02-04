<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    CodeFile="CustomizeInternalRecieptPopUp.aspx.cs" Inherits="CustomizeInternalRecieptPopUp" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True" Text="<%$ Resources:LocalizedResources, CustomizeInternalFeeReceipt %>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333" valign="top">
                <asp:Label ID="lblmandatoryStar" runat="server" CssClass="ClsMdtStar" Text="* "
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError %>"
                         runat="server" />
                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlFields" runat="server" Width="100%">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <table cellpadding="0" cellspacing="0" runat="server" id="tblHeading" visible="True">
                                        <tr>
                                            <td align="left">
                                                <table cellpadding="0" cellspacing="2" runat="server" id="Table1" visible="True">
                                                    <tr>
                                                        <td class="ClsBorderlight" valign="middle" style="padding-left: 5px">
                                                            <asp:Label ID="lblStudent" runat="server" CssClass="TextNormal" Text="<%$ Resources:LocalizedResources, StudentName %>"
                                                                EnableViewState="False"></asp:Label>
                                                                <span> :</span>
                                                        </td>
                                                        <td class="ClsHilightBGB">
                                                            <asp:Label ID="lblStudentHeading" runat="server" EnableViewState="True"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:Label ID="lblDate" runat="server" CssClass="TextNormal" Text="<%$ Resources:LocalizedResources, PaymentDate %>"
                                                                EnableViewState="False"></asp:Label>
                                                                 <span> :</span>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtDate" CssClass="SmlTxtBox" runat="server"></asp:TextBox>
                                                            <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtDate" Format="dd MMM yyyy" Culture="en"
                                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, PaymentDateShouldNotBlank %>" />
                                                            <asp:Label ID="Label17" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>
                                                            <asp:CustomValidator ID="cstDate" runat="server" Display="none" EnableClientScript="true"
                                                                 ClientValidationFunction="ValidateDate" ErrorMessage="<%$ Resources:LocalizedResources, PaymentDateShouldNotFutureDate %>"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:Label ID="Label3" runat="server" CssClass="TextNormal" Text="<%$ Resources:LocalizedResources, PayableFor %>"
                                                                EnableViewState="False"></asp:Label><span> :</span><br />
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" onclick="CheckUncheckAllCheckBoxes(this.checked);"/>
                                                        </td>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:CheckBoxList ID="chklstPayableFor" runat="server" RepeatDirection="Horizontal"
                                                                RepeatColumns="3">
                                                            </asp:CheckBoxList>
                                                            <asp:CustomValidator ID="cstvalAtLeast" runat="server" Display="none" EnableClientScript="true" 
                                                                 ClientValidationFunction="CheckAtLeastOnePayableForIsSeleted" ErrorMessage="<%$ Resources:LocalizedResources, AtLeastPayableForShouldBeSelected %>"></asp:CustomValidator>
                                                        </td>
                                                        <td><asp:Label ID="Label5" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px;">
                                                            <asp:Label ID="Label4" runat="server" CssClass="TextNormal" Text="<%$ Resources:LocalizedResources, AmountPayable %>"
                                                                EnableViewState="False"></asp:Label><span> :</span>
                                                        </td>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:Label ID="lblPaybleAmount" runat="server" CssClass="TextNormalB"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:Label ID="lblRemark" runat="server" CssClass="TextNormal " Text="<%$ Resources:LocalizedResources, Remarks %>"
                                                                EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="LrgTxtBox" MaxLength="100" Width="346px"
                                                                Rows="3"></asp:TextBox>
                                                            <asp:Label ID="Label2" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>
                                                        </td>
                                                        <td align="left" valign="middle">
                                                            <asp:RequiredFieldValidator ID="reqValDate" runat="server" ControlToValidate="txtDate"
                                                                ErrorMessage="<%$ Resources:LocalizedResources, PaymentDateShouldNotBlank %>" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="reqValRemark" runat="server" ControlToValidate="txtRemark"
                                                                 ErrorMessage="<%$ Resources:LocalizedResources, RemarksShouldNotBeBlank %>" SetFocusOnError="True"
                                                                Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regvalTxtReason" runat="server" Display="None"
                                                                ControlToValidate="txtRemark" ErrorMessage="" ValidationExpression="^[\s\S]{0,140}$"> </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px; width: 140px;
                                                            padding-right: 5px;">
                                                            <asp:Label ID="Label1" runat="server" CssClass="TextNormal " Text="<%$ Resources:LocalizedResources, ConsolidatePartialPaymentsOnReceipt %>"
                                                                EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:CheckBox ID="chkConslidatedPayableFor" runat="server" CssClass="LblSmlGray"
                                                                Style="vertical-align: middle;" Text="<%$ Resources:LocalizedResources, MsgCustomizeInternalRecieptPopUp %>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="" valign="top">
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:Button ID="btnPrint" runat="server" Text="<%$ Resources:LocalizedResources, Print %>" CssClass="ClsBtn" UseSubmitBehavior="false"
                                                                Width="78px" OnClick="btnPrint_OnClick" />
                                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtn" CausesValidation="False"
                                                                UseSubmitBehavior="false" OnClick="btnCancel_OnClick" />
                                                        </td>
                                                        <td align="left" valign="top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidInternalFeeDetailsId" runat="server" />
        <asp:HiddenField ID="hidStudentId" runat="server" />
        <asp:HiddenField ID="hidStudentName" runat="server" />
        <asp:HiddenField ID="hidServerDate" runat="server" />
        <asp:HiddenField ID="hidNextAcademicYearId" runat="server" />
        <asp:HiddenField ID="hidRegNo" runat="server" />
        <asp:HiddenField ID="hidFromDate" runat="server" />
        <asp:HiddenField ID="hidToDate" runat="server" />
        <asp:HiddenField ID="hidIncludePaid" runat="server" />
        <asp:HiddenField ID="hidPayForNextYear" runat="server" />
        <asp:HiddenField ID="hidIsRegNoFilter" runat="server" />
        <asp:HiddenField ID="hidStandardID" runat="server" />
        <asp:HiddenField ID="hidDivisionID" runat="server" />
        <asp:HiddenField ID="hidFeeTypeID" runat="server" />
        <asp:HiddenField ID="hidPageIndex" runat="server" />
        <asp:HiddenField ID="hidPaymentDateShouldNotFutureDate" runat="server" />
        <asp:HiddenField ID="hidAtLeastPayableForShouldBeSelected" runat="server" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
    </div>
    <script language="javascript" type="text/javascript">

        _clientcstDate = "<%=this.cstDate.ClientID%>";
        _clientcstvalAtLeast = "<%=this.cstvalAtLeast.ClientID%>";
        _clienttxtDate = "<%=this.txtDate.ClientID %>"
        _clientServerDate = "<%=this.hidServerDate.ClientID %>";
        _clientlblPaybleAmount =    "<%=this.lblPaybleAmount.ClientID %>";

        function ValidateDate(source, args) {
            var bIsValid = true;

            if (document.getElementById(_clienttxtDate).value != "") {
                var serverDate = document.getElementById(_clientServerDate).value;
                dtStartDate = new Date(convertdate(document.getElementById(_clienttxtDate).value));
                var today = new Date(serverDate);
                if (today < dtStartDate) {
                    document.getElementById(_clientcstDate).errormessage =
                                    document.getElementById("<%=hidPaymentDateShouldNotFutureDate.ClientID%>").value;
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CheckAtLeastOnePayableForIsSeleted(source, args) {
            var bIsValid = true;
            var len = $("input:checkbox[id*=_chklstPayableFor_]:checked").length;
            if (len == 0) {
                document.getElementById(_clientcstvalAtLeast).errormessage = document.getElementById("<%=hidAtLeastPayableForShouldBeSelected.ClientID%>").value;
                bIsValid = false;
            }
            
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CheckUncheckAllCheckBoxes(checked) {
            var PaybleAmount = parseInt(document.getElementById(_clientlblPaybleAmount).innerHTML);
            document.getElementById(_clientlblPaybleAmount).innerHTML = "0";
            $("input:checkbox[id*=_chklstPayableFor_]").each(
                function () {
                    var chkText = $('label[for=' + this.id + ']').text();
                    var Amount = chkText.substring(chkText.indexOf("Rs.") + 3, chkText.length - 1);
                    this.checked = checked;
                    if (checked)
                        SetTotal(checked, parseInt(Amount));
                }
            );
        }

        function SetTotal(checked, amount) {
            $("input:checkbox[id*=_chkSelectAll]").attr("checked", $("input:checkbox[id*=_chklstPayableFor_]:checked").length == $("input:checkbox[id*=_chklstPayableFor_]").length);
            var PaybleAmount = parseInt(document.getElementById(_clientlblPaybleAmount).innerHTML);
            PaybleAmount = (checked) ? PaybleAmount + amount : PaybleAmount - amount;
            document.getElementById(_clientlblPaybleAmount).innerHTML = PaybleAmount;
        }

    </script>
</asp:Content>
