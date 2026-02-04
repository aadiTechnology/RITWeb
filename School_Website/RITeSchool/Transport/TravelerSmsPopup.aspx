<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="TravelerSmsPopup.aspx.cs" Inherits="TravelerSmsPopup"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table>
        <tr>
            <td class="ClsGrayMainTitle" align="left">               
                <span class="MainTitleHead">Send Sms</span>
            </td>
        </tr>
        <tr align="left">
            <td style="width: 100%" align="left">
                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" />
                <asp:Label ID="lblError" runat="server" Visible="false" CssClass="ClsMdtStar"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
        <tr align="center">
            <td colspan="2" align="center" valign="top">
                <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                    Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
    <table width="70%">
        <tr id="trNoRecordMsg" runat="server">
            <td style="height: 10px;" align="center">
                <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                    Text="No Record Found." EnableViewState="False" Width="70%"></asp:Label>
            </td>
        </tr>
        <tr align="left" id="trlstvwTransport" runat="server" width="100%">
            <td colspan="3">
                <table id="tblTransportDetails" runat="server" align="center" width="100%">
                    <tr align="center" style="width: 100%">
                        <td align="center" style="width: 100%">
                            <div id="divContainer" class="GridBorder" runat="server" visible="true" style="width: 100%;
                                height: 300px; overflow: scroll">
                                <asp:ListView ID="lstvwTravelersDetails" runat="server" 
                                    onitemdatabound="lstvwTravelersDetails_ItemDataBound">
                                    <LayoutTemplate>
                                        <table align="center" width="100%" runat="server" id="tblTravlerInfo" style="color: #333333"
                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th align="left" width="25%" style="padding-left: 9px;">
                                                    <asp:Label ID="lnkSortName" runat="server" CausesValidation="false" ForeColor="Black">Travelers Name </asp:Label>
                                                </th>
                                                <th align="center" width="25%" id="thMobileNo" runat="server">
                                                    <asp:Label ID="lblMobileNo" runat="server" CausesValidation="false" ForeColor="Black">Mobile No. </asp:Label>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td align="left" class="paddingL">
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                            </td>
                                            <td align="center" id="tdMobileNo" runat="server">
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                            <td align="left" class="paddingL">
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                            </td>
                                            <td align="center" id="tdMobileNo" runat="server">
                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                No record found.
                                            </td>
                                        </tr>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="middle" class="ClsBorderlight">
                <asp:Label ID="Label3" runat="server" CssClass="clsLabel" Text="Type :"
                    EnableViewState="False"></asp:Label>
            </td>
            <td colspan="2" class="ClsBorderlight" >
                <asp:RadioButton ID="rbSMS" runat="server" Text="SMS" GroupName="SmsMessage" Checked="true" class="ClsLabel"  ViewStateMode="Enabled" onclick="SetLimit(1);"/>
                <asp:RadioButton ID="rbMessage" runat="server" Text="Message" GroupName="SmsMessage" class="ClsLabel" ViewStateMode="Enabled" onclick="SetLimit(2);"/>
            </td>
        </tr>
       <tr id="trSMS" runat="server">
            <td valign="middle" class="ClsBorderlight">
                <asp:Label ID="lblReason" runat="server" CssClass="clsLabel" Text="SMS Text :"
                    EnableViewState="False"></asp:Label>
            </td>
            <td valign="top" align="left" colspan="2">
                <asp:TextBox ID="txtReason" runat="server" CssClass="LrgTxtBox" MaxLength="500" TabIndex="2"
                    Rows="3" TextMode="MultiLine" Width="90%"></asp:TextBox>&nbsp;
                <asp:Label ID="lblStar" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"
                    EnableViewState="false"></asp:Label>&nbsp;
                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateText"></asp:CustomValidator>
            </td>            
        </tr>
        <tr id="trSave" runat="server">
            <td>
            </td>
            <td align="left" colspan="2">
                <asp:Button ID="btnSendSms" Text="Send" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="true" OnClick="btnSendSms_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
            </td>
        </tr>
        <tr id="tr1" runat="server">
            <td align="center" colspan="3" style="height: 40px">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" UseSubmitBehavior="false" OnClick="btnBack_Click" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hidUserRoleId" runat="server" Value="0" />
                <asp:HiddenField ID="hidRouteId" runat="server" Value="0" />
                <asp:HiddenField ID="hidStopId" runat="server" Value="0" />
                <asp:HiddenField ID="hidShiftId" runat="server" Value="0" />
                <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                <asp:HiddenField ID="hidDivisionId" runat="server" Value="0" />
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">


        _clientlblUpdateSucess = "<%=this.lblSuccessMsg.ClientID %>"
        _clienttxtReason = '<%=this.txtReason.ClientID %>'
        _clientrbSMS = '<%=this.rbSMS.ClientID %>'

        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).innerHTML = "";
                document.getElementById(_clientlblUpdateSucess).style.display = "none";

            }
        }

        function ValidateText(src, args) {            
            var limit = 0;
            var message = '';
            var blankMessage = ''
            var isSMS = document.getElementById(_clientrbSMS).checked
            var data = document.getElementById(_clienttxtReason).value

            if (isSMS) {
                limit = 100
                message = 'SMS length should not be greater than 100 characters.';
                blankMessage = 'SMS text should not be blank.'
            }
            else {
                limit = 500;
                message = 'Message length should not be greater than 500 characters.';
                blankMessage = 'Message text should not be blank.'
            }

            if (data.trim() == '') {
                src.errormessage = blankMessage;
                args.IsValid = false
                return true;
            }
            else if (data.trim().length > limit) {
                src.errormessage = message;
                args.IsValid = false
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function SetLimit(val) {
            if (val == 2) {
                $('#' + '<%=this.lblReason.ClientID %>').html('Message Text');
            }
            else {
                $('#' + '<%=this.lblReason.ClientID %>').html('SMS Text');
            }
        }
    </script>

</asp:Content>
