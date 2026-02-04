<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SMSUI.aspx.cs" Inherits="SMSUI" ValidateRequest="false" ViewStateMode="Disabled"%>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        td[valign="top"] {
            vertical-align: top;
        }
    </style>
    <table align="center" width="100%" style="height: 100%" border="0" cellspacing="0"
        cellpadding="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="center">
                            <!--MainDataTable Starts Here -->
                            <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                                <tr>
                                    <td id="MainDataTable" align="center">
                                        <!-- Data Insert Here -->
                                        <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                            <tr id="trMandatoryMark" runat="server" viewstatemode="Enabled">
                                                <td align="right" colspan="1">
                                                    <div style="float: right" class="LblErrorMsg">
                                                        * Mandatory Fields</div>
                                                </td>
                                            </tr>
                                            <asp:Panel ID="pnlErrorMsg" Visible="False" runat="server" ViewStateMode="Enabled" Width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="blue"
                                                            Width="100%" CssClass="MainTitleHead" ViewStateMode="Enabled"></asp:Label>
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="2" style="width: 100%">
                                                        <tr id="trPlaneDisplay" runat="server" visible="false" viewstatemode="Enabled">
                                                            <td style="width: 65%" valign="top" align="center">
                                                                <table cellpadding="0" cellspacing="2" width="100%">
                                                                    <tr>
                                                                        <td align="left" class="TdDataEntryHeader ClsBorderlight" style="width: 20%" valign="top">
                                                                            <asp:Label ID="lblFrom" runat="server" CssClass="ClsLabel" Text="From :" ViewStateMode="Enabled"></asp:Label>
                                                                        </td>
                                                                        <td align="left" class="TdDataEntryControl">
                                                                            <asp:TextBox ID="txtFrom" runat="server" CssClass="ExLrgTxtBox" MaxLength="11" ViewStateMode="Enabled"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trReceivedDate" runat="server" visible="false" viewstatemode="Enabled">
                                                                        <td align="left" class=" ClsBorderlight">
                                                                            <asp:Label ID="lblReceivedDate" runat="server" CssClass="ClsLabel" Text="Sent Date  :"
                                                                                ViewStateMode="Enabled"></asp:Label>
                                                                        </td>
                                                                        <td align="left" colspan="1">
                                                                            <asp:TextBox ID="txtReceivedDate" runat="server" CssClass="ExLrgTxtBox" BorderStyle="Solid"
                                                                                ReadOnly="True" BorderColor="Gray" BorderWidth="1px" ViewStateMode="Enabled"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class=" ClsBorderlight">
                                                                            <span class="ClsLabel">To :</span>&nbsp;
                                                                        </td>
                                                                        <td align="left" class="TdDataEntryControl ">
                                                                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                                                ID="UpdatePanel2">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox ID="txtUser" runat="server" CssClass="ClsReadOnly" Height="30px" MaxLength="200"
                                                                                        ReadOnly="True" TextMode="MultiLine" Width="95%" ViewStateMode="Enabled">
                                                                                    </asp:TextBox>
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnSendSMS" EventName="Click" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trShowSMSTxt" runat="server">
                                                                        <td align="left" class=" ClsBorderlight" valign="top">
                                                                            <span class="ClsLabel">SMS Text :</span>&nbsp;
                                                                        </td>
                                                                        <td align="left" class="TdDataEntryHeader ">
                                                                            <asp:TextBox ID="txtShowSMS" runat="server" CssClass="ExLrgTxtBox" Height="140px"
                                                                                TextMode="MultiLine" Width="95%" ReadOnly="True" ViewStateMode="Enabled"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="TdDataEntryHeader" valign="top">
                                                                        </td>
                                                                        <td align="left" class="TdDataEntryHeader ">
                                                                            <div style="float: left">
                                                                                <asp:Button ID="btnBack" Text="Back" runat="server" ValidationGroup="valGroupSend"
                                                                                    CssClass="ClsBtnMid" CausesValidation="False" ViewStateMode="Enabled"/>
                                                                            </div>
                                                                            <div style="float: right">
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="TdDataEntryHeader">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td align="left" class="TdDataEntryHeader">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                        </td>
                                                                        <td runat="server" id="TDtxtCount" align="left" viewstatemode="Enabled">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                        </td>
                                                                        <td align="left">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="2" style="width: 100%">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:ValidationSummary CssClass="ClsLabel" DisplayMode="BulletList" ID="valSum_SendMessage"
                                                                                runat="server" ValidationGroup="valGroupSend" Width="100%" ViewStateMode="Enabled"/>
                                                                            <div class="ClsHilightBGB" id="MsgLbl" visible="false" runat="server" style="width: 40%;
                                                                                text-align: center" viewstatemode="Enabled">
                                                                                <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                                                    ID="UpdatePanel1">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                                                            ViewStateMode="Enabled"></asp:Label>
                                                                                    </ContentTemplate>
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="btnSendSMS" EventName="Click" />
                                                                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                                                                    </Triggers>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                            <asp:UpdatePanel ChildrenAsTriggers="false" runat="server" UpdateMode="Conditional"
                                                                                ID="uperror">
                                                                                <ContentTemplate>
                                                                                    <asp:Label ID="Label1" ViewStateMode="Enabled" runat="server" CssClass="LblErrorMsg" /><br />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnSendSMS" EventName="Click" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trMobileDisplay" runat="server" visible="true" viewstatemode="Enabled">
                                                            <td style="width: 35%" valign="top" id="trMobileView" runat="server" viewstatemode="Enabled">
                                                                <table cellpadding="0" cellspacing="2" width="100%">
                                                                    <tr>
                                                                        <td align="left" >
                                                                        </td>
                                                                        <td align="left" class="ClsLabel">
                                                                            <asp:CheckBox ID="chkScheduleSMS" Text="Schedule SMS" runat="server"
                                                                                Visible="true" ViewStateMode="Enabled"/>
                                                                            <asp:Image runat="server" ID="imgNew" ImageUrl="~/images/newLink.gif" style="padding-left:10px"/>
                                                                        </td>                                                                                                                                                
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="white-space:nowrap;padding-left:2px" >
                                                                         <span id="Span1" class="ClsLabel ClsBorderlight" style="padding-right:18px;padding-left:4px">Date & Time :</span>
                                                                        </td>
                                                                        <td align="left" colspan="3">
                                                                          <asp:TextBox ID="txtPaymentDate" CssClass="SmlTxtBox" runat="server" TabIndex="1" Width="90px" ViewStateMode="Enabled">
                                                                          </asp:TextBox>
                                                                            <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtPaymentDate" Format="dd MMM yyyy" 
                                                                                Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Payment date should not be blank." /> 
                                                                           <asp:TextBox ID="txtStartTime" runat="server" CssClass="MidTxtBox" Width="70px" MaxLength="8" Enabled="false" ViewStateMode="Enabled">1:00 AM</asp:TextBox><span
                                                                             class="LblSmlGray">&nbsp;e.g. 10:00 AM</span>           
                                                                            <span class="LblSmlGray" style="padding-left:5px">SMS Schedule should be set after 1 hour and within 7 days range from now.</span>                                                                  
                                                                             <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="IsValidTimeRange"
                                                                                CssClass="LblErrorMsg" Display="None" ErrorMessage=""
                                                                                ValidationGroup="valGroupSend"></asp:CustomValidator>
                                                                            <asp:CustomValidator ID="cstInvalidStartTime" CssClass="LblErrorMsg" runat="server" ValidationGroup="valGroupSend"
                                                                                SetFocusOnError="True" Display="None" ErrorMessage="Please enter valid time e.g. 10:00 AM."
                                                                                ClientValidationFunction="IsValidStartTime"> </asp:CustomValidator>
                                                                        </td>                                                  
                                                                                                                                         
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="TdDataEntryHeader" valign="top">
                                                                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                                                ID="uPnl">
                                                                                <ContentTemplate>
                                                                                    <table cellpadding="0" cellspacing="2">
                                                                                        <tr>
                                                                                            <td align="left" class=" ClsBorderlight">
                                                                                                <span id="lblFreeSMS" class="ClsLabel">Free SMS :</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="ClsBorder1PBg">
                                                                                                <asp:Label ID="lblFreeSMSVal" runat="server" ViewStateMode="Enabled" CssClass="LblUsrNameSml"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class=" ClsBorderlight">
                                                                                                <span id="lblSentSMS" class="ClsLabel">Sent SMS :</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="ClsBorder1PBg">
                                                                                                <asp:Label ID="lblSentSMSVal" runat="server" ViewStateMode="Enabled" CssClass="LblUsrNameSml"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class=" ClsBorderlight" id="tdExeced" runat="server">
                                                                                                <span id="lblExceededSms" class="ClsLabel" style="white-space: nowrap;">Exceeded SMS
                                                                                                    :</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="ClsBorder1PBg">
                                                                                                <asp:Label ID="lblExceededSmsVal" runat="server" ViewStateMode="Enabled" CssClass="LblUsrNameSml"
                                                                                                    Text="0"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                         <tr>
                                                                                            <td align="left" class=" ClsBorderlight" id="td1" runat="server">
                                                                                                <span id="Span2" class="ClsLabel" style="white-space: nowrap;">SMS Balance
                                                                                                    :</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" class="ClsBorder1PBg">
                                                                                                <asp:Label ID="lblSMSBalance" runat="server" ViewStateMode="Enabled" CssClass="LblUsrNameSml"
                                                                                                    Text="0"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="SMSLblSMlBlue" style="vertical-align: bottom;padding-left: 10px"
                                                                                                    CausesValidation="False" OnClientClick="SMSTemplate(); return false;">Use Template</asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnSendSMS" EventName="Click"/>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"/>
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                            <img src="../images/spacer.gif" width="100" height="10" />&nbsp;
                                                                            <asp:CustomValidator ID="cstSMSMsg" runat="server" CssClass="LblErrorMsg" ValidationGroup="valGroupSend"
                                                                                Display="None" ErrorMessage="SMS Text should not be blank." ClientValidationFunction="CheckReqSMSText"></asp:CustomValidator>
                                                                            <asp:RequiredFieldValidator ID="reqSender" runat="server" Display="None" ValidationGroup="valGroupSend"
                                                                                ErrorMessage="From(Sender) should not be blank." ControlToValidate="txtFromMb"
                                                                                CssClass="LblErrorMsg"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td align="left" class="TdDataEntryHeader " style="width: 20%" colspan="2" valign="top">
                                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td align="left" class="TdDataEntryHeader " colspan="1" valign="top">
                                                                                        <!-- Mobile BG Table Starts -->
                                                                                        <table cellpadding="0" cellspacing="0" style="height: 310px" width="230px">
                                                                                            <tr>
                                                                                                <td class="SMSDisplay" style="width: 100%" valign="top" align="center">
                                                                                                    <!-- SMS Display Table Starts -->
                                                                                                    <table align="center" style="width: 98%" cellspacing="1" cellpadding="0">
                                                                                                        <tbody>
                                                                                                            <tr>
                                                                                                                <td align="center" valign="bottom" style="height: 51px;">
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right" valign="top" style="padding-right: 15px">
                                                                                                                    <input style="width: 175px; font-size: 8pt; color: #ffffff; border-top-style: none;
                                                                                                                        font-family: arial; border-right-style: none; border-left-style: none; background-color: #0c53fc;
                                                                                                                        border-bottom-style: none;" id="txt_count" onfocus="this.blur();" value="0 characters, 1 SMS message(s)" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="center" style="padding-top: 4px; padding-bottom: 2px;" valign="bottom">
                                                                                                                    <table cellpadding="0" cellspacing="0" style="width: 80%">
                                                                                                                        <tr>
                                                                                                                            <td style="width: 17%">
                                                                                                                                <span class="SMSLblSMlBlk">From :</span>
                                                                                                                            </td>
                                                                                                                            <td style="width: 50%">
                                                                                                                                <asp:TextBox ID="txtFromMb" runat="server" Width="99%" CssClass="SMSFromTxtBox" ViewStateMode="Enabled"
                                                                                                                                    MaxLength="8"></asp:TextBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="center" valign="bottom" style="padding-top: 2px; padding-bottom: 3px;">
                                                                                                                    <table cellpadding="0" cellspacing="0" style="width: 80%">
                                                                                                                        <tr>
                                                                                                                            <td style="width: 17%">
                                                                                                                                <span class="SMSLblSMlBlk">To :</span>
                                                                                                                            </td>
                                                                                                                            <td style="width: 50%">
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td style="width: 98%; padding-right: 2px; padding-top: 1px;" align="center" valign="bottom">
                                                                                                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                                                                                        ID="UpdatePanel3">
                                                                                                                        <ContentTemplate>
                                                                                                                            <asp:TextBox ID="txtMessage" runat="server" CssClass="ExLrgTxtBox" Width="187px"
                                                                                                                                Height="94pt" TextMode="MultiLine" Style="overflow: hidden" ViewStateMode="Enabled">Type your SMS here...</asp:TextBox>
                                                                                                                        </ContentTemplate>
                                                                                                                        <Triggers>
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="btnSendSMS" EventName="Click"/>
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"/>
                                                                                                                        </Triggers>
                                                                                                                    </asp:UpdatePanel>
                                                                                                                </td>
                                                                                                            </tr>                                                                                                             
                                                                                                            <tr>
                                                                                                                <td style="width: 100%;" align="center">
                                                                                                                    <table cellpadding="0" cellspacing="0" style="width: 82%;">
                                                                                                                        <tr>
                                                                                                                            <td align="left" style="width: 50%">
                                                                                                                                <asp:Button ID="btnSendSMS" OnClick="imgBtnSendMessage_Click" runat="server" CssClass="btnSMSsend"
                                                                                                                                    disable-page="true" ValidationGroup="valGroupSend" Text="Send SMS" OnClientClick="if(!setUserNames()) return false;"
                                                                                                                                    ViewStateMode="Enabled">
                                                                                                                                </asp:Button>
                                                                                                                            </td>
                                                                                                                            <td align="center">
                                                                                                                            </td>
                                                                                                                            <td align="right" style="width: 50%">
                                                                                                                                <asp:Button ID="btnClear" Text="Clear" runat="server" ValidationGroup="valGroupSend"
                                                                                                                                    CssClass="BtnHLightSml" CausesValidation="False" ViewStateMode="Enabled"/>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>                                                                                                           
                                                                                                        </tbody>
                                                                                                    </table>
                                                                                                    <!-- SMS Display Table End -->
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 100%;" align="center">
                                                                                                <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server" ID="UpdatePanel7">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:TextBox ID="txtTemplateId" runat="server" Width="100%" placeholder = "Template Id"></asp:TextBox>
                                                                                                        <asp:RequiredFieldValidator ID="reqtemplateid" runat="server" ErrorMessage="Template Id should not be blank."  CssClass="LblErrorMsg" ControlToValidate = "txtTemplateId"></asp:RequiredFieldValidator>
                                                                                                        <span class="ClsMdtStar">*</span>                                                                                                
                                                                                                    </ContentTemplate>
                                                                                                    <Triggers>
                                                                                                        <asp:AsyncPostBackTrigger ControlID="btnSendSMS" EventName="Click"/>
                                                                                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"/>
                                                                                                    </Triggers>
                                                                                                </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <!-- Mobile BG Table End -->
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td align="left" class="TdDataEntryHeader" colspan="1" valign="top" style="width: 100%">
                                                                            <table cellpadding="0" cellspacing="2" style="width: 100%; padding-top: 3px;">
                                                                                <tr>
                                                                                    <td align="left">
                                                                                    </td>
                                                                                    <td align="left" style="width: 80%;" valign="baseline" class="ClsLabel">
                                                                                        <asp:CheckBox ID="chkManualNumber" runat="server" Text="Add Mobile Numbers Manually"
                                                                                            AutoPostBack="True" OnCheckedChanged="chkManualNumber_CheckedChanged" ViewStateMode="Enabled"/>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                        <asp:LinkButton ID="hlnkPersonalAddresses" runat="server" CssClass="SMSLblSMlBlue"
                                                                                            CausesValidation="False" OnClientClick="PersonalAddressBook();return false;" ViewStateMode="Enabled">Personal Address book</asp:LinkButton>
                                                                                        <asp:CustomValidator ID="cstManualNos" runat="server" ClientValidationFunction="CheckValidMobileNos"
                                                                                            CssClass="LblErrorMsg" Display="None" ErrorMessage="Enter 10 digit multiple mobile numbers seperated by comma. Max 10 numbers are allowed."
                                                                                            ValidationGroup="valGroupSend"></asp:CustomValidator>
                                                                                        <asp:HiddenField ID="hidManualSMSCount" runat="server" />
                                                                                    </td>
                                                                                    <td align="left" style="width: 20px;" valign="top">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                    </td>
                                                                                    <td colspan="2">
                                                                                        <asp:UpdatePanel ID="updtpnlNumbers" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 99%">
                                                                                                            <asp:TextBox ID="txtManualNumbers" runat="server" CssClass="ClsReadOnly" Height="44px"
                                                                                                                MaxLength="120" Rows="3" TextMode="MultiLine" Width="100%" ViewStateMode="Enabled"></asp:TextBox><br />
                                                                                                            <span class="ClsLabel">(Enter multiple mobile numbers seperated by comma. Max 10 digit
                                                                                                                number.)</span>
                                                                                                        </td>
                                                                                                        <td style="width: 3%">
                                                                                                            &nbsp; <span class="ClsMdtStar" runat="server" id="spnMandManualNos" visible="true"
                                                                                                                style="visibility: hidden;" viewstatemode="Enabled">*</span>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:AsyncPostBackTrigger ControlID="chkManualNumber" EventName="CheckedChanged"/>
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" rowspan="4" valign="top" style="width: 6%;" id="6">
                                                                                        <img src="../images/ArrowBlueDblNw.gif" />
                                                                                        <img src="../images/ArrowBlueDblNw.gif" />
                                                                                    </td>
                                                                                    <td colspan="2" style="width: 74%;">
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width: 99%; padding-left: 2px; padding-bottom: 2px; padding-top: 2px;
                                                                                                    margin-left: 5px;" class="ClsBorderlight">
                                                                                                    <span class="ClsHilightTextB" style="font-family: Verdana">Step 1 : Please select category
                                                                                                        first :</span>
                                                                                                </td>
                                                                                                <td align="left" valign="top">
                                                                                                    <span class="ClsMdtStar">*</span>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" id="tdchkadmin" runat="server" visible="true" colspan="2">
                                                                                        <asp:UpdatePanel UpdateMode="Always" runat="server" ID="UpdatePanel6">
                                                                                            <ContentTemplate>
                                                                                                <asp:CheckBox ID="chkAdmin" Text="Include Admin" runat="server" class="ClsLabel"
                                                                                                    Visible="true" ViewStateMode="Enabled"/>
                                                                                                <asp:CheckBox ID="chkPrincipal" Text="Include Principal" runat="server" class="ClsLabel"
                                                                                                    Visible="true" ViewStateMode="Enabled"/>
                                                                                                <asp:LinkButton ID="lnkTeacherGroups" runat="server" CssClass="SMSLblSMlBlue" style="vertical-align: bottom;padding-left: 10px"
                                                                                                    CausesValidation="False" OnClientClick="TeacherGroup(); return false;" Visible="True" ViewStateMode="Enabled">Contact Group(s)</asp:LinkButton>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" colspan="2">
                                                                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel4">
                                                                                            <ContentTemplate>
                                                                                                <asp:RadioButton ID="optTeachers" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                                                                    GroupName="UserType" Text="Teachers" AutoPostBack="true" OnCheckedChanged="opt_CheckedChanged"
                                                                                                    onchange="Page_BlockSubmit = false;" ViewStateMode="Enabled"/>
                                                                                                <asp:RadioButton ID="optStudents" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                                                                    GroupName="UserType" Text="Students" AutoPostBack="true" OnCheckedChanged="opt_CheckedChanged"
                                                                                                    onchange="Page_BlockSubmit = false;" ViewStateMode="Enabled"/>
                                                                                                <asp:RadioButton ID="optSupervisor" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                                                                    GroupName="UserType" Text="Supervisor" AutoPostBack="true" Width="90px" OnCheckedChanged="opt_CheckedChanged"
                                                                                                    onchange="Page_BlockSubmit = false;" ViewStateMode="Enabled"/>
                                                                                                <asp:RadioButton ID="optOtherStaff" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                                                                    GroupName="UserType" Text="Other Staff" AutoPostBack="true" Width="90px" OnCheckedChanged="opt_CheckedChanged"
                                                                                                    onchange="Page_BlockSubmit = false;" ViewStateMode="Enabled"/>
                                                                                                <asp:RadioButton ID="optParentTeacherAssociation" runat="server" CssClass="ClsLabel"
                                                                                                    Font-Bold="False" GroupName="UserType" Text="Parent Teacher Association" AutoPostBack="true"
                                                                                                    OnCheckedChanged="opt_CheckedChanged" onchange="Page_BlockSubmit = false;" ViewStateMode="Enabled"/>
                                                                                                <asp:RadioButton ID="optEntireSchool" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                                                                    GroupName="UserType" Text="Entire School" AutoPostBack="true" OnCheckedChanged="opt_CheckedChanged"
                                                                                                    onchange="Page_BlockSubmit = false;" ViewStateMode="Enabled"/>
                                                                                                <asp:RadioButton ID="optLeftStudents" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                                                                    GroupName="UserType" Text="Left Students" AutoPostBack="true" OnCheckedChanged="opt_CheckedChanged"
                                                                                                    onchange="Page_BlockSubmit = false;" ViewStateMode="Enabled"/>
                                                                                                <asp:HiddenField ID="hidQry" runat="server" ViewStateMode="Enabled"/>
                                                                                                <asp:HiddenField ID="hidGroupQuery" runat="server"/>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" colspan="2">
                                                                                        <table cellpadding="0" cellspacing="2" width="100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width: 99%; padding-left: 2px; padding-bottom: 2px; padding-top: 2px;
                                                                                                    margin-left: 5px;" class=" ClsBorderlight">
                                                                                                    <span class="ClsHilightTextB" style="font-family: Verdana">Step 2 : Now click on address
                                                                                                        book link for SMS Recipient(s) :</span>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" style="padding-right: 3px; padding-left: 5px; padding-bottom: 3px;
                                                                                                    padding-top: 3px">
                                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td cssclass="ClsLabel">
                                                                                                                <a href="JavaScript:ToUserId()" runat="server" id="HlnkSelectUser" class="SMSLblSMlBlue" 
                                                                                                                    viewstatemode="Enabled">
                                                                                                                    <u>Address book Click here</u> </a>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <span class="ClsLabel">Selected Recipient(s)</span>
                                                                                                            </td>
                                                                                                            <td valign="bottom">
                                                                                                                <img src="../images/down.gif" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left">
                                                                                                    <asp:TextBox ID="txtToUserId" runat="server" CssClass="ClsReadOnly" MaxLength="200"
                                                                                                        TextMode="MultiLine" Width="100%" Height="44px" ReadOnly="True" 
                                                                                                        ViewStateMode="Enabled"></asp:TextBox>
                                                                                                    <asp:CustomValidator ID="reqToUserId" runat="server" CssClass="LblErrorMsg" ValidationGroup="valGroupSend"
                                                                                                        Display="None" ErrorMessage="At least one SMS recipient should be selected."
                                                                                                        ClientValidationFunction="CheckReqToUserId"> </asp:CustomValidator>
                                                                                                </td>
                                                                                                <td align="left" valign="top">
                                                                                                    <span class="ClsMdtStar">*</span>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <img src="../images/ArrowBlueDblRev.gif" />
                                                                                        <img src="../images/ArrowBlueDblRev.gif" />
                                                                                    </td>
                                                                                    <td align="left" class=" ClsBorderlight" style="width: 99%">
                                                                                        <span class="ClsHilightTextB" style="font-family: Verdana">Step 3 : Please type your
                                                                                            SMS & click Send SMS :</span>
                                                                                    </td>
                                                                                    <td align="left" valign="top">
                                                                                        <span class="ClsMdtStar">*</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" style="width: 100px;">
                                                                                    </td>
                                                                                    <td align="left">
                                                                                    </td>
                                                                                    <td align="left">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" style="width: 15px">
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:CheckBox ID="chkSendMessage" Text="Send Message" runat="server" class="ClsLabel"
                                                                                            Visible="true" ViewStateMode="Enabled"/>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" style="width: 15px">
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:Button ID="btnSent" Text="Sent Items" runat="server" ValidationGroup="valGroupSend"
                                                                                            CssClass="ClsBtnSml" OnClick="imgBtnSentItems_Click" CausesValidation="False"
                                                                                            Width="73px" ViewStateMode="Enabled"/>
                                                                                        <asp:Button ID="btnScheduledSMS" Text="Scheduled SMS" runat="server" ValidationGroup="valGroupSend"
                                                                                            CssClass="ClsBtnSml" CausesValidation="False"
                                                                                            Width="100px" onclick="btnScheduledSMS_Click" ViewStateMode="Enabled"/>
                                                                                        <asp:Button ID="btnRecievedSMS" Text="Received SMS" runat="server" ValidationGroup="valGroupSend"
                                                                                            CssClass="ClsBtnSml" CausesValidation="False" OnClick="btnRecieved_Click" Width="90px" ViewStateMode="Enabled"/>
                                                                                        <asp:Button ID="btnAllSentItems" Text="All Sent Items" runat="server" ValidationGroup="valGroupSend"
                                                                                            CssClass="ClsBtnSml" CausesValidation="False" Width="90px" onclick="btnAllSentItems_Click" ViewStateMode="Enabled"/>
                                                                                    </td>
                                                                                    <td align="left" style="height: 26px">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblNoteData" runat="server" viewstatemode="Enabled">
                                                        <tr>
                                                            <td style="width: 103px">
                                                            </td>
                                                            <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                                <span class="LblNrmlB" style="font-weight: bold">Note :</span>
                                                            </td>
                                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px">
                                                                <span class="LblSmlV" style="color: Red;">Do not use any website URL or mobile number
                                                                    in SMS text. Such SMS will not get delivered to selected recipient(s).</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:UpdatePanel UpdateMode="Always" runat="server" ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="HidUserNames" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidReplyUserID" runat="server" />
                                                            <asp:HiddenField ID="hidUserId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAdminUserID" runat="server" Value="0" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAdminUserName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidPrincipalUserID" runat="server" Value="0" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidPrincipleName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidAdminMbNo" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidPrincipalMbNo" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidBackUrl" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidTeacherId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidStdDivId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidStudentId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidTeacherName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidStdDivName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidStudentName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidSupervisorId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidSupervisorName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidOtherStaffId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidOtherStaffName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidParentTeacherAssociationId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidParentTeacherAssociationName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidUserType" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidCanEdit" runat="server" Value="Y" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidSMSCount" runat="server" Value="1" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidSMSId" runat="server" Value="0" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidState" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidGroupId" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidGroupName" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidEditedSMSId" Value="0" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidResendUserName" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidSMSCountVal" Value = "0" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidIsUnicodeSMS" Value = "0" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidIncreasedSMSLength" Value = "0" ViewStateMode="Enabled"/>
                                                          <%--  <asp:HiddenField runat="server" ID="hidShowAllSentSMS" Value="0"/>            --%>                                                
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- Data Insert End Here -->
                                    </td>
                                </tr>
                            </table>
                            <!--MainDataTable End Here -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        var sTeacherType= 'Teacher'
        var sAdminType = 'Admin';
        var sSuperviserType = 'Supervisor';
        var sPrincipalType = 'Principal';
        _clienttxtToUserId = "<%=this.txtToUserId.ClientID %>";
        _clienttxtMessage = "<%=this.txtMessage.ClientID %>";
        _clientHidReplyUserID = "<%=this.HidReplyUserID.ClientID %>";
        _clienthidUserId = "<%=this.hidUserId.ClientID %>";
        _clientHidUserNames = "<%=this.HidUserNames.ClientID %>";
        _clientHlnkSelectUser = "<%=this.HlnkSelectUser.ClientID %>";
        _clientoptTeachers = "<%=this.optTeachers.ClientID %>";
        _clientoptStudents = "<%=this.optStudents.ClientID %>";
        _clientoptSupervisor = "<%=this.optSupervisor.ClientID %>";
        _clientoptOtherStaff = "<%=this.optOtherStaff.ClientID %>";
        _clientoptParentTeacherAssociation = "<%=this.optParentTeacherAssociation.ClientID %>";
        _clientoptEntireSchool = "<%=this.optEntireSchool.ClientID %>";
        _clientlblMessage = "<%=this.MsgLbl.ClientID %>";
        _clienttxtMessage = "<%=this.txtMessage.ClientID %>";
        _clienthidQry = "<%=this.hidQry.ClientID %>";
        _clientHidStdDivId = "<%=this.HidStdDivId.ClientID %>";
        _clientHidTeacherId = "<%=this.HidTeacherId.ClientID %>";
        _clientHidStudentId = "<%=this.HidStudentId.ClientID %>";
        _clientHidSupervisorId = "<%=this.HidSupervisorId.ClientID %>";
        _clientHidOtherStaffId = "<%=this.HidOtherStaffId.ClientID %>";
        _clientHidOtherStaffName = "<%=this.HidOtherStaffName.ClientID %>";
        _clientHidParentTeacherAssociationId = "<%=this.HidParentTeacherAssociationId.ClientID %>";
        _clientHidParentTeacherAssociationName = "<%=this.HidParentTeacherAssociationName.ClientID %>";
        _clientHidStdDivName = "<%=this.HidStdDivName.ClientID %>";
        _clientHidTeacherName = "<%=this.HidTeacherName.ClientID %>";
        _clientHidStudentName = "<%=this.HidStudentName.ClientID %>";
        _clientHidSupervisorName = "<%=this.HidSupervisorName.ClientID %>";
        _clientHidUserType = "<%=this.HidUserType.ClientID %>";
        _clienttxtManualNumbers = "<%=this.txtManualNumbers.ClientID %>";
        _clientchkManualNumber = "<%=this.chkManualNumber.ClientID %>";
        _clientcstManualNos = "<%=this.cstManualNos.ClientID %>";
        _clientChkAdmin = "<%=this.chkAdmin.ClientID %>";
        _clientchkPrincipal = "<%=this.chkPrincipal.ClientID %>";
        _clientHidAdminUserID = "<%=this.HidAdminUserID.ClientID %>";
        _clientHidAdminUserName = "<%=this.HidAdminUserName.ClientID %>";
        _clientHidPrincipleUserID = "<%=this.HidPrincipalUserID.ClientID %>";
        _clientHidPrincipleName = "<%=this.HidPrincipleName.ClientID %>";
        _clientbtnSendSMS = "<%=this.btnSendSMS.ClientID %>";
        _clientbtnClear = "<%=this.btnClear.ClientID %>";
        _clienthidSMSCount = "<%=this.hidSMSCount.ClientID %>";
        _clientchkSendMsg = "<%=this.chkSendMessage.ClientID %>";
        _clientGroupId = "<%=this.hidGroupId.ClientID %>";
        _clienthidGroupName = "<%=this.hidGroupName.ClientID %>";
        _clienthidGroupQuery = "<%=this.hidGroupQuery.ClientID %>";
        _clientchkScheduleSMS = "<%=this.chkScheduleSMS.ClientID %>";
        _clienttxtPaymentDate = "<%=this.txtPaymentDate.ClientID %>";
        _clienttxtStartTime = "<%=this.txtStartTime.ClientID %>";
        _clienthidIsUnicodeSMS = "<%=this.hidIsUnicodeSMS.ClientID %>"
        _clienthidManualSMSCount = "<%=this.hidManualSMSCount.ClientID %>"
        _clientoptLeftStudents = "<%=this.optLeftStudents.ClientID %>"
        _clientTemplateRegistrationId = "<%= this.txtTemplateId.ClientID %>"
        var smsLength = 306;        
        if (parseInt($('[id$=hidIncreasedSMSLength]').val()) > 1)
            smsLength = parseInt($('[id$=hidIncreasedSMSLength]').val())
                    
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);       

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;

            if (postBackElement.id == _clientoptTeachers || postBackElement.id == _clientoptStudents || postBackElement.id == _clientoptSupervisor || postBackElement.id == _clientoptOtherStaff || postBackElement.id == _clientoptParentTeacherAssociation || postBackElement.id == _clientoptLeftStudents)
                ToUserId();
        }

        var Page_IsValid = true;
        var sEntireSchool = 'Entire School';
        var sAdmin = document.getElementById(_clientHidAdminUserName).value;
        var sPricipal = document.getElementById(_clientHidPrincipleName).value;

        function IsValidTimeRange(src, args) {
            var bIsValid = true;
            if ($get(_clientchkScheduleSMS).checked) {
                var bIsValid = true;
                var StartDt = "";
                var sStrtDate = document.getElementById(_clienttxtPaymentDate).value
                var sStrtTime = document.getElementById(_clienttxtStartTime).value
                if (sStrtTime == "")
                    sStrtTime = "00:00 AM";

                var currentdate = new Date();
                var hours;              
                hours = (currentdate.getHours() + 1);
                
                var datetime = new Date((currentdate.getMonth() + 1) + "/" + currentdate.getDate() 
                                    + "/" + currentdate.getFullYear() + " "+
                                    + hours + ":"
                                    + currentdate.getMinutes() + ":" + currentdate.getSeconds());

                var date = new Date();
                var result = date.setTime(date.getTime() + (7 * 24 * 60 * 60 * 1000));
                                
                if (sStrtDate == "") {
                    src.errormessage = "Schedule Date should not be blank.";
                    bIsValid = false;
                }
                else if (sStrtDate != "" && !validateDate(sStrtDate)) {
                    src.errormessage = "Schedule Date should be in valid format.";
                    bIsValid = false;
                }
                else if (GetConvertedDate(sStrtDate, sStrtTime) <= datetime) {
                    src.errormessage = "SMS Schedule Date & Time should be set after 1 hour from now.";
                    bIsValid = false;
                }
                else if (GetConvertedDate(sStrtDate, sStrtTime) >= new Date(result)) {
                    src.errormessage = "Schedule Date & Time should be within the 7 days from now.";
                    bIsValid = false;
                }

            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function GetConvertedDate(sStrtDate,sStartTime) {
            StartDt = new Date(sStrtDate.replace('-', ' ').replace(/-/g, ' ') + " " + sStartTime);
            return StartDt;
        }

        function validateDate(txtDueDate) {
            var isValid = true;
            if (document.all) {
                if (isNaN(new Date(convertdate(txtDueDate).replace(/-/g, ' '))))
                    isValid = false;
            }
            else {
                if (isNaN(new Date(convertdate(txtDueDate).replace('-', ' '))))
                    isValid = false;
            }
            return isValid;
        }

        function getDate(obj) {
            var strDate = obj.replace('-', ' ').replace('-', ' ');
            return new Date(strDate);
        }

        function IsValidStartTime(oSrc, args) {
            if ($get(_clientchkScheduleSMS).checked) {
                if (document.getElementById(_clienttxtStartTime)) {
                    if (document.getElementById(_clienttxtStartTime).value != '') {
                        if (!isTimeValid(_clienttxtStartTime)) {
                            args.IsValid = false;
                            return true;
                        }
                        else if (isTimeValid(_clienttxtStartTime)) {
                            var time = $get(_clienttxtStartTime).value.trim();
                            if (time.toLowerCase() == "00:00 pm") {
                                args.IsValid = false;
                                return true;
                            }
                            else if (time.toLowerCase() == "00:00 am") {
                                args.IsValid = false;
                                return true;
                            }
                        }

                        args.IsValid = true;
                        return false;
                    }
                    else if (document.getElementById(_clienttxtStartTime).value == '') {
                        args.IsValid = false;
                        return true;
                    }
                }
            }
            args.IsValid = true;
            return false;
        }

        function isTimeValid(result) {

            var timeStr = document.getElementById(result).value;
            timeStr = timeStr.toUpperCase();
            if (trimAll(timeStr) == '')
                return false;

            var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            second = matchArray[4];
            ampm = matchArray[6];

            if (second == "") { second = null; }
            if (ampm == "") { ampm = null; }

            if (hour < 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            if (second != null && (second < 0 || second > 59))
                return false;

            if (ampm == null)
                return false;
            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + '0' + minute;
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(result).value = str;
            return true;
        }

        function SetControlsForAdminDetails(UserType) {            
            var sUsers = document.getElementById(_clienttxtToUserId).value;
            var reUsers = sUsers.replace(/\s+/g, 'T');
            var reAdmin = sAdmin.replace(/\s+/g, 'T');
            var rePricipal = sPricipal.replace(/\s+/g, 'T');
            var iIndex = sUsers.search('(' + sAdminType + ')');
            var ipIndex = sUsers.search('(' + sPrincipalType + ')');
            var sIds = document.getElementById(_clienthidUserId).value;
            var AdminId = document.getElementById(_clientHidAdminUserID).value;
            var PrincipleId = document.getElementById(_clientHidPrincipleUserID).value;
            if (sUsers != '') {
                sUsers = sUsers + ', ';
            }
            if (sIds != '') {
                sIds = sIds + '; ';
            }

            if (UserType == sAdminType) {
                if (iIndex == -1) {                    
                    if ((document.getElementById(_clientChkAdmin) != null) && (document.getElementById(_clientChkAdmin).checked)) {
                        document.getElementById(_clienttxtToUserId).value = sUsers + sAdmin;

                        if (document.getElementById(_clientoptParentTeacherAssociation).checked == true) {
                            if (document.getElementById(_clientHidParentTeacherAssociationId).value != '') {
                                document.getElementById(_clientHidParentTeacherAssociationId).value = document.getElementById(_clientHidParentTeacherAssociationId).value + ";" + document.getElementById(_clientHidAdminUserID).value;
                            }
                            else {
                                document.getElementById(_clientHidParentTeacherAssociationId).value = document.getElementById(_clientHidAdminUserID).value;
                            }
                            document.getElementById(_clientHidParentTeacherAssociationName).value = sUsers + sAdmin;
                        }
                    }

                }
                else {

                    if (!document.getElementById(_clientChkAdmin).checked) {
                        document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(', ' + sAdmin, '');
                        document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sAdmin + ', ', '');
                        document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sAdmin, '');
                        var AdminId = document.getElementById(_clientHidAdminUserID).value;

                        if (document.getElementById(_clientoptParentTeacherAssociation).checked == true) {
                            document.getElementById(_clientHidParentTeacherAssociationId).value = document.getElementById(_clientHidParentTeacherAssociationId).value.replace('; ' + AdminId, '');
                            document.getElementById(_clientHidParentTeacherAssociationId).value = document.getElementById(_clientHidParentTeacherAssociationId).value.replace(AdminId + '; ', '');
                            document.getElementById(_clientHidParentTeacherAssociationId).value = document.getElementById(_clientHidParentTeacherAssociationId).value.replace(AdminId, '');
                            document.getElementById(_clientHidParentTeacherAssociationName).value = document.getElementById(_clientHidParentTeacherAssociationName).value.replace(sAdmin, '');
                        }
                    }

                }
                document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value;

            }
            if (UserType == sPrincipalType) {
                if (ipIndex == -1) {
                    if ((document.getElementById(_clientchkPrincipal) != null) && (document.getElementById(_clientchkPrincipal).checked)) {
                        document.getElementById(_clienttxtToUserId).value = sUsers + sPricipal
                        if (document.getElementById(_clientHidTeacherId).value != '') {
                            document.getElementById(_clientHidTeacherId).value = document.getElementById(_clientHidTeacherId).value + ";" + document.getElementById(_clientHidPrincipleUserID).value
                            document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value + sPricipal
                            sUsers += document.getElementById(_clientHidTeacherName).value
                        }
                        else {
                            document.getElementById(_clientHidTeacherId).value = document.getElementById(_clientHidPrincipleUserID).value
                            document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value + sPricipal
                            sUsers += document.getElementById(_clientHidTeacherName).value
                        }
                    }
                }
                else {
                    if (!document.getElementById(_clientchkPrincipal).checked) {
                        document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(', ' + sPricipal, '')
                        document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sPricipal + ', ', '')
                        document.getElementById(_clienttxtToUserId).value = document.getElementById(_clienttxtToUserId).value.replace(sPricipal, '')
                        var PrincipleId = document.getElementById(_clientHidPrincipleUserID).value
                        document.getElementById(_clientHidTeacherId).value = document.getElementById(_clientHidTeacherId).value.replace(PrincipleId, '')
                        document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sPricipal, '')
                    }
                }
                document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value
            }
            else if (UserType == 'EntireSchool') {
                document.getElementById(_clienttxtToUserId).value = sEntireSchool;
                document.getElementById(_clientHidUserNames).value = sEntireSchool;
                ClearAllUsers();
            }
            else {
                if (document.getElementById(_clienttxtToUserId).value == sEntireSchool) {
                    document.getElementById(_clienttxtToUserId).value = "";
                    document.getElementById(_clientHidUserNames).value = "";

                }

            }
            if (UserType == "Teacher" && document.getElementById(_clientHidParentTeacherAssociationId).value != '') {
                if (document.getElementById(_clientHidTeacherId).value != '')
                    document.getElementById(_clientHidTeacherId).value += "; " + document.getElementById(_clientHidParentTeacherAssociationId).value;
                else
                    document.getElementById(_clientHidTeacherId).value += document.getElementById(_clientHidParentTeacherAssociationId).value;

            }
            if (UserType == "Supervisor" && document.getElementById(_clientHidParentTeacherAssociationId).value != '') {
                if (document.getElementById(_clientHidSupervisorId).value != '')
                    document.getElementById(_clientHidSupervisorId).value += "; " + document.getElementById(_clientHidParentTeacherAssociationId).value;
                else
                    document.getElementById(_clientHidSupervisorId).value += document.getElementById(_clientHidParentTeacherAssociationId).value;

            }

            if (UserType == "ParentTeacherAssociation" && document.getElementById(_clientHidTeacherId).value != '') {
                if (document.getElementById(_clientHidParentTeacherAssociationId).value != '')
                    document.getElementById(_clientHidParentTeacherAssociationId).value += "; " + document.getElementById(_clientHidTeacherId).value;
                else
                    document.getElementById(_clientHidParentTeacherAssociationId).value += document.getElementById(_clientHidTeacherId).value;
                if ((document.getElementById(_clientChkAdmin) != null) && (document.getElementById(_clientChkAdmin).checked))
                    document.getElementById(_clientHidParentTeacherAssociationId).value += "; " + document.getElementById(_clientHidAdminUserID).value;
            }

            if (UserType == "Student" && document.getElementById(_clientHidParentTeacherAssociationId).value != '') {
                if (document.getElementById(_clientHidStudentId).value != '')
                    document.getElementById(_clientHidStudentId).value += "; " + document.getElementById(_clientHidParentTeacherAssociationId).value;
                else
                    document.getElementById(_clientHidStudentId).value += document.getElementById(_clientHidParentTeacherAssociationId).value;
            }
        }

        function setUserNames() {
            Page_IsValid = true
            var isPageValid = true;
            if (typeof (Page_ClientValidate) == 'function')
                isPageValid = Page_ClientValidate();
            if (isPageValid) {
                if (parseInt(document.getElementById(_clienthidSMSCount).value) > 1) {
                    if (!window.confirm("SMS will be send in " + document.getElementById(_clienthidSMSCount).value + " parts for each selected user. Are you sure to continue?")) {
                        Page_IsValid = false;
                        return false;
                    }
                }
            }
            document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value;
            return true;
        }
        function ClearAllUsers() {
            document.getElementById(_clientHidTeacherId).value = "";
            document.getElementById(_clientHidTeacherName).value = "";
            document.getElementById(_clientHidStudentId).value = "";
            document.getElementById(_clientHidStudentName).value = "";
            document.getElementById(_clientHidStdDivId).value = "";
            document.getElementById(_clientHidStdDivName).value = "";
            document.getElementById(_clientHidSupervisorId).value = "";
            document.getElementById(_clientHidSupervisorName).value = "";
            document.getElementById(_clientHidOtherStaffId).value = "";
            document.getElementById(_clientHidOtherStaffName).value = "";
            document.getElementById(_clientHidParentTeacherAssociationId).value = "";
            document.getElementById(_clientHidParentTeacherAssociationName).value = "";
            $get(_clientGroupId).value = "";
            $get(_clienthidGroupName).value = "";
        }

        var sBuyersList = null;
        var sSuppliersList = null;
        var sSupplierIdList;
        var sUserIdList;

        function SetToUserId(UserName, UserId, isIndivisualUser) {            
            sBuyersList = UserName;
            sUserIdList = UserId;
            var sExistingUserNames = document.getElementById(_clienttxtToUserId).value;
            var iUserType = document.getElementById(_clientHidUserType).value;

            //if teachers
            if (document.getElementById(_clientoptTeachers).checked == true && isIndivisualUser!="G") {
                document.getElementById(_clientHidTeacherId).value = sUserIdList;
                document.getElementById(_clientHidTeacherName).value = sBuyersList;
                var TeacherIds = document.getElementById(_clientHidTeacherId).value;
                var PrincipleId = document.getElementById(_clientHidPrincipleUserID).value;
                if (TeacherIds.match(PrincipleId) != null && TeacherIds.match(PrincipleId).index >= 0) {
                    if (document.getElementById(_clientchkPrincipal) != null)
                        $get(_clientchkPrincipal).checked = true;
                }
                else {
                    if (document.getElementById(_clientchkPrincipal) != null)
                        $get(_clientchkPrincipal).checked = false;
                }
            }
            //if Supervisor
            else if (document.getElementById(_clientoptSupervisor).checked == true && isIndivisualUser != "G") {
                document.getElementById(_clientHidSupervisorId).value = sUserIdList;
                document.getElementById(_clientHidSupervisorName).value = sBuyersList;
            }

            else if (document.getElementById(_clientoptOtherStaff).checked == true && isIndivisualUser != "G") {
                document.getElementById(_clientHidOtherStaffId).value = sUserIdList;
                document.getElementById(_clientHidOtherStaffName).value = sBuyersList;
            }
            //if parent
            else if (document.getElementById(_clientoptParentTeacherAssociation).checked == true && isIndivisualUser != "G") {
                document.getElementById(_clientHidParentTeacherAssociationId).value = sUserIdList;
                document.getElementById(_clientHidParentTeacherAssociationName).value = sBuyersList;
                var PrincipleId = document.getElementById(_clientHidPrincipleUserID).value;
                var AdminId = document.getElementById(_clientHidAdminUserID).value;



                var ParentTeacherIds = document.getElementById(_clientHidParentTeacherAssociationId).value;
                document.getElementById(_clientHidParentTeacherAssociationId).value = ParentTeacherIds
                var ParentTeacherNames = document.getElementById(_clientHidParentTeacherAssociationName).value
                document.getElementById(_clientHidParentTeacherAssociationName).value = ParentTeacherNames
                if (ParentTeacherIds != "") {
                    var arr = ParentTeacherIds.split(";")
                    var iIndex;
                    var iValue = false;
                    for (iIndex = 0; iIndex < arr.length; iIndex++) {
                        if (arr[iIndex] == PrincipleId)
                            iValue = true;
                    }
                    if (iValue == true) {
                        if ((ParentTeacherIds.match(PrincipleId) != null && ParentTeacherIds.match(PrincipleId).index >= 0)) {
                            if (document.getElementById(_clientchkPrincipal) != null)
                                $get(_clientchkPrincipal).checked = true;
                        }
                    }
                    else {
                        if (document.getElementById(_clientchkPrincipal) != null)
                            $get(_clientchkPrincipal).checked = false;
                    }
                    if (ParentTeacherIds.match(AdminId) != null && ParentTeacherIds.match(AdminId).index >= 0)
                        $get(_clientChkAdmin).checked = true;
                    else
                        $get(_clientChkAdmin).checked = false;
                }
            }            
            //if students
            else if (((document.getElementById(_clientoptStudents) != null) && (document.getElementById(_clientoptStudents).checked == true) || (document.getElementById(_clientoptLeftStudents) != null) && (document.getElementById(_clientoptLeftStudents).checked == true)) && isIndivisualUser != "G") {            
                if (isIndivisualUser == 'N') {
                    document.getElementById(_clientHidStdDivId).value = sUserIdList;
                    document.getElementById(_clientHidStdDivName).value = sBuyersList;
                }
                else {                    
                    document.getElementById(_clientHidStudentId).value = sUserIdList;
                    document.getElementById(_clientHidStudentName).value = sBuyersList;
                }
            }

            //if Groups
            else if (isIndivisualUser == "G") {
                $get(_clienthidGroupName).value = UserName;
                $get(_clientGroupId).value = UserId;
            }
            document.getElementById(_clienttxtToUserId).value = ForatNames();

        }

        function SetTemplate(sTemplate, tempregid) {
            $get(_clienttxtMessage).value = sTemplate;
            $get(_clientTemplateRegistrationId).value = tempregid; 
        }

        function ForatNames() {
            var ss = document.getElementById(_clientHidTeacherName).value;            
            var sFileExtension = ss.substring(0, ss.indexOf('('));
            var sUserNameList = '';
            var TeacherIds = document.getElementById(_clientHidTeacherId).value;
            var ParentTeacherIds = document.getElementById(_clientHidParentTeacherAssociationId).value;
            var PrincipleId = document.getElementById(_clientHidPrincipleUserID).value;
            var AdminId = document.getElementById(_clientHidAdminUserID).value;

            if ((document.getElementById(_clientChkAdmin) != null) && document.getElementById(_clientChkAdmin).checked) {
                if (sUserNameList != '')
                    sUserNameList = sUserNameList + ', ' + sAdmin;
                else
                    sUserNameList = sUserNameList + sAdmin;
            }
            document.getElementById(_clientoptParentTeacherAssociation);
            if ((document.getElementById(_clientchkPrincipal) != null) && document.getElementById(_clientchkPrincipal).checked && !document.getElementById(_clientoptTeachers).checked) {
                if (sUserNameList != '')
                    sUserNameList = sUserNameList + ', ' + sPricipal;
                else
                    sUserNameList = sUserNameList + sPricipal;
            }
            else if (document.getElementById(_clientoptTeachers).checked && (document.getElementById(_clientchkPrincipal) != null) && $get(_clientchkPrincipal).checked == true && (TeacherIds.indexOf(PrincipleId) == -1 || document.getElementById(_clientHidParentTeacherAssociationId).value != '')) {
                if (sUserNameList != '')
                    sUserNameList = sUserNameList + ', ' + sPricipal;
                else
                    sUserNameList = sUserNameList + sPricipal;
            }
            if (document.getElementById(_clientHidParentTeacherAssociationName).value == '') {
                if ((document.getElementById(_clientchkPrincipal) != null) && document.getElementById(_clientchkPrincipal).checked && sUserNameList.indexOf(sPricipal) == -1) {
                    if (TeacherIds.match(PrincipleId) != null && TeacherIds.match(PrincipleId).index >= 0) {
                        if (sUserNameList == '') {
                            sUserNameList = sUserNameList + sPricipal;
                        } else
                            sUserNameList = sUserNameList + ', ' + sPricipal;
                    }
                    else {
                        if (document.getElementById(_clientchkPrincipal) != null)
                            $get(_clientchkPrincipal).checked = false;
                    }
                }
            }

            if (document.getElementById(_clientHidTeacherName).value != '') {
                if (sUserNameList == '') {
                    sUserNameList = document.getElementById(_clientHidTeacherName).value;
                }
                else {
                    if ((document.getElementById(_clientchkPrincipal) != null) && document.getElementById(_clientchkPrincipal).checked) {
                        document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sPricipal + ', ', '');
                        document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sPricipal, '');
                    }
                    if ((document.getElementById(_clientChkAdmin) != null) && document.getElementById(_clientChkAdmin).checked) {
                        document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sAdmin + ', ', '');
                        document.getElementById(_clientHidTeacherName).value = document.getElementById(_clientHidTeacherName).value.replace(sAdmin, '');
                    }
                    if (document.getElementById(_clientHidTeacherName).value != '') {
                        var Teachers = document.getElementById(_clientHidTeacherName).value.split(',');
                        var i = 0;
                        for (i = 0; i < Teachers.length; i++) {
                            if (sUserNameList.indexOf(Teachers[i]) == -1)
                                sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidTeacherName).value;
                        }

                    }
                }
            }

            if (document.getElementById(_clientHidStdDivName).value != '') {
                
                if (sUserNameList == '') {
                    sUserNameList = document.getElementById(_clientHidStdDivName).value;
                }
                else {
                    sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidStdDivName).value;
                }
            }
             
            if ($get(_clienthidGroupName).value != '') {
                if (sUserNameList == '') {
                    sUserNameList = $get(_clienthidGroupName).value;
                }
                else {
                    sUserNameList = sUserNameList + ', ' + $get(_clienthidGroupName).value;
                }
            }

            if (document.getElementById(_clientHidStudentName).value != '') {
                if (sUserNameList == '') {
                    sUserNameList = document.getElementById(_clientHidStudentName).value;
                }
                else {
                    sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidStudentName).value;
                }
            }
            if (document.getElementById(_clientHidSupervisorName).value != '') {
                if (sUserNameList == '') {
                    sUserNameList = document.getElementById(_clientHidSupervisorName).value;
                }
                else {
                    sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidSupervisorName).value;
                }
            }

            if (document.getElementById(_clientHidOtherStaffName).value != '') {
                if (sUserNameList == '') {
                    sUserNameList = document.getElementById(_clientHidOtherStaffName).value;
                }
                else {
                    sUserNameList = sUserNameList + ', ' + document.getElementById(_clientHidOtherStaffName).value;
                }
            }
            if (document.getElementById(_clientHidParentTeacherAssociationName).value != '') {

                if ((document.getElementById(_clientchkPrincipal) != null) && document.getElementById(_clientchkPrincipal).checked) {
                    if (ParentTeacherIds.match(PrincipleId) != null && ParentTeacherIds.match(PrincipleId).index >= 0) {
                        if (ParentTeacherIds.indexOf(PrincipleId) == -1) {
                            if (sUserNameList == '') {
                                sUserNameList = sUserNameList + sPricipal;
                            } else
                                sUserNameList = sUserNameList + ', ' + sPricipal;
                        }
                    }
                    else if (document.getElementById(_clientchkPrincipal) != null)
                        $get(_clientchkPrincipal).checked = false;
                }

                if ((document.getElementById(_clientChkAdmin) != null) && document.getElementById(_clientChkAdmin).checked) {
                    if (ParentTeacherIds.match(AdminId) != null && ParentTeacherIds.match(AdminId).index >= 0) {
                        if (ParentTeacherIds.indexOf(AdminId) == -1) {
                            if (sUserNameList == '') {
                                sUserNameList = sUserNameList + sAdmin;
                            } else
                                sUserNameList = sUserNameList + ', ' + sAdmin;
                        }
                    }
                    else
                        $get(_clientChkAdmin).checked = false;
                }

                if (sUserNameList == '') {
                    sUserNameList = document.getElementById(_clientHidParentTeacherAssociationName).value;
                }
                else {
                    if ((document.getElementById(_clientchkPrincipal) != null) && document.getElementById(_clientchkPrincipal).checked) {
                        document.getElementById(_clientHidParentTeacherAssociationName).value = document.getElementById(_clientHidParentTeacherAssociationName).value.replace(sPricipal + ', ', '');
                        document.getElementById(_clientHidParentTeacherAssociationName).value = document.getElementById(_clientHidParentTeacherAssociationName).value.replace(sPricipal, '');
                    }

                    if ((document.getElementById(_clientChkAdmin) != null) && document.getElementById(_clientChkAdmin).checked) {
                        document.getElementById(_clientHidParentTeacherAssociationName).value = document.getElementById(_clientHidParentTeacherAssociationName).value.replace(sAdmin + ', ', '');
                        document.getElementById(_clientHidParentTeacherAssociationName).value = document.getElementById(_clientHidParentTeacherAssociationName).value.replace(sAdmin, '');
                    }
                    if (document.getElementById(_clientHidParentTeacherAssociationName).value != '') {
                        sUserNameList = GetUserNameList(sUserNameList, document.getElementById(_clientHidParentTeacherAssociationName).value);
                    }
                }

            }
            return sUserNameList;
        }

        function GetUserNameList(asUserList, asAppendUsers) {
            var Users = asUserList.split(', ');
            var iCount = Users.length;
            var i;
            for (i = 0; i < Users.length && asAppendUsers != ''; i++) {
                var IndexOfUserName = (Users[i].indexOf(' (') != -1) ? asAppendUsers.indexOf(Users[i].substring(0, Users[i].indexOf(' ('))) : -1;
                if (IndexOfUserName != -1) {
                    if (asAppendUsers.substring(IndexOfUserName).substring(0, asAppendUsers.substring(IndexOfUserName).indexOf('), ')) != '')
                        asAppendUsers = asAppendUsers.replace(asAppendUsers.substring(IndexOfUserName).substring(0, asAppendUsers.substring(IndexOfUserName).indexOf('), ') + 3), "");
                    else
                        asAppendUsers = asAppendUsers.replace(asAppendUsers.substring(IndexOfUserName), "");
                    IndexOfUserName = asAppendUsers.indexOf(Users[i].substring(0, Users[i].indexOf(' (')));
                    if (IndexOfUserName != -1)
                        asAppendUsers = asAppendUsers.replace(asAppendUsers.substring(IndexOfUserName).substring(0, asAppendUsers.substring(IndexOfUserName).indexOf(')') + 1), "");
                }
            }
            if (asAppendUsers != '')
                return asUserList + ', ' + asAppendUsers;
            else
                return asUserList;
        }

        function GetUserIdList(asUserIdList, asAppendUsersId) {
            var Users = asUserList.split(', ');
            var iCount = Users.length;
            var i;
            for (i = 0; i < Users.length && asAppendUsers != ''; i++) {
                var IndexOfUserName = asAppendUsers.indexOf(Users[i].substring(0, Users[i].indexOf(' (')));
                if (IndexOfUserName != -1) {
                    if (asAppendUsers.substring(IndexOfUserName).substring(0, asAppendUsers.substring(IndexOfUserName).indexOf('), ')) != '')
                        asAppendUsers = asAppendUsers.replace(asAppendUsers.substring(IndexOfUserName).substring(0, asAppendUsers.substring(IndexOfUserName).indexOf('), ') + 3), "");
                    else
                        asAppendUsers = asAppendUsers.replace(asAppendUsers.substring(IndexOfUserName), "");
                    IndexOfUserName = asAppendUsers.indexOf(Users[i].substring(0, Users[i].indexOf(' (')));
                    if (IndexOfUserName != -1)
                        asAppendUsers = asAppendUsers.replace(asAppendUsers.substring(IndexOfUserName).substring(0, asAppendUsers.substring(IndexOfUserName).indexOf(')') + 1), "");
                }
            }
            if (asAppendUsers != '')
                return asUserList + ', ' + asAppendUsers;
            else
                return asUserList;
        }

        function GetUserNames(isIndivisualUser) {
           
            var sUserNameList = '';
            var iUserType = document.getElementById(_clientHidUserType).value;
            if (document.getElementById(_clientoptTeachers).checked == true) {
                sUserNameList = document.getElementById(_clientHidTeacherName).value;
            }
            else if (document.getElementById(_clientoptStudents).checked == true || document.getElementById(_clientoptLeftStudents).checked == true) {            
                if ((iUserType == sAdminType || iUserType == sSuperviserType) && isIndivisualUser == 'N')
                    sUserNameList = document.getElementById(_clientHidStdDivName).value;
                else
                    sUserNameList = document.getElementById(_clientHidStudentName).value;
            }
            else if (document.getElementById(_clientoptSupervisor).checked == true) {
                sUserNameList = document.getElementById(_clientHidSupervisorName).value;
            }

            else if (document.getElementById(_clientoptOtherStaff).checked == true) {
                sUserNameList = document.getElementById(_clientHidOtherStaffName).value;
            }
            else if (document.getElementById(_clientoptParentTeacherAssociation).checked == true) {
                sUserNameList = document.getElementById(_clientHidParentTeacherAssociationName).value;
            }            
            return sUserNameList;
        }

        function ToUserId() {            
            var UserRole;
            var SelectedQry = document.getElementById(_clienthidQry).value;
            var AlreadySelectedUserId = document.getElementById(_clientHidReplyUserID).value;

            if (document.getElementById(_clientoptTeachers).checked == true) {
                UsersList = "Teacher";
            }
            else if (document.getElementById(_clientoptStudents).checked == true) {
                UsersList = "Student";
            }
            else if (document.getElementById(_clientoptSupervisor).checked == true) {
                UsersList = "Supervisor";
            }

            else if (document.getElementById(_clientoptOtherStaff).checked == true) {
                UsersList = "OtherStatff";
            }
            else if (document.getElementById(_clientoptParentTeacherAssociation).checked == true) {
                UsersList = "ParentTeacherAssociation";
            }
            else if (document.getElementById(_clientoptEntireSchool).checked == true) {
                UsersList = "EntireSchool";
                return;
            }
            else if(document.getElementById(_clientoptLeftStudents).checked == true){
                UsersList = "LeftStudents";
            }
            else if (document.getElementById(_clientChkAdmin).checked == true) {
                alert('Admin is already added in the To-list.');
                return;
            }
            else if ((document.getElementById(_clientchkPrincipal) != null) && document.getElementById(_clientchkPrincipal).checked == true) {
                alert('Principal is already added in the To-list.');
                return;
            }
            else {
                alert('Select option to whom you want to send mail.');
                return;
            }
            window.open("../Common/SelectUserName.aspx?" + SelectedQry, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=520').focus();
        }

        function PersonalAddressBook() {
            var SelectedQry = document.getElementById(_clienthidQry).value;
            window.open("../Common/PesonalAddressBookUI.Aspx?" + SelectedQry, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=520').focus();
        }

        function TeacherGroup() { 
            var SelectedQry = document.getElementById(_clienthidQry).value;
            window.open("../Common/MailingGroupPopup.Aspx?" + SelectedQry, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=750').focus();
        }

        function SMSTemplate() {
            var SelectedQry = document.getElementById(_clienthidQry).value;
            window.open("../Admin/SmsTemplateUI.aspx?" + SelectedQry, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=650').focus();
        }
        
        function getGroupIds() {
            return  $get(_clientGroupId).value;
        }

        function xyz() {
            return true;
            window.event.returnValue = false;
            window.clipboardData.effectAllowed = true
            window.clipboardData.clearData();
        }

        function GetUserIds(isIndivisualUser) {            
            var sUserIds = '';
            var iUserType = document.getElementById(_clientHidUserType).value;

            if (document.getElementById(_clientoptTeachers).checked == true) {
                sUserIds = document.getElementById(_clientHidTeacherId).value;
            }
            else if (((document.getElementById(_clientoptStudents) != null) && (document.getElementById(_clientoptStudents).checked == true)) || ((document.getElementById(_clientoptLeftStudents) != null) && (document.getElementById(_clientoptLeftStudents).checked == true))) {
                if ((iUserType == sAdminType || iUserType == sSuperviserType || iUserType == sTeacherType) && isIndivisualUser == 'N')
                    sUserIds = document.getElementById(_clientHidStdDivId).value;
                else
                    sUserIds = document.getElementById(_clientHidStudentId).value;                
            }
            else if (document.getElementById(_clientoptSupervisor).checked == true) {
                sUserIds = document.getElementById(_clientHidSupervisorId).value;
            }
            else if (document.getElementById(_clientoptParentTeacherAssociation).checked == true) {
                sUserIds = document.getElementById(_clientHidParentTeacherAssociationId).value;
            }
            else if (document.getElementById(_clientoptOtherStaff).checked == true) {
                sUserIds = document.getElementById(_clientHidOtherStaffId).value;
            }
            return sUserIds;

        }

        function getManualNumbers() {
            var strmno = '';
            if (document.getElementById(_clienttxtManualNumbers) != null) {
                strmno = document.getElementById(_clienttxtManualNumbers).value;
            }
            return strmno;
        }
        
        function setManualNumbers(sMobileNumbers) {
            if (document.getElementById(_clienttxtManualNumbers) != null) {
                sMobileNumbers = trimAll(sMobileNumbers);
                if (sMobileNumbers.length == 0) {
                    document.getElementById(_clientchkManualNumber).checked = false;
                    document.getElementById(_clienttxtManualNumbers).className = "ClsReadOnly";
                    document.getElementById(_clienttxtManualNumbers).disabled = true;
                    $get('<%=spnMandManualNos.ClientID %>').style.visibility = 'hidden';
                } else {
                    document.getElementById(_clientchkManualNumber).checked = true;
                    document.getElementById(_clienttxtManualNumbers).className = "LrgMobileTxtBox";
                    document.getElementById(_clienttxtManualNumbers).disabled = false;
                    $get('<%=spnMandManualNos.ClientID %>').style.visibility = '';
                }
                document.getElementById(_clienttxtManualNumbers).value = sMobileNumbers;
            }
        }
        var iTotalChars = 0;
        var iMsgCount = 0;

        function alertMsgLength(e) {            
            if (iTotalChars > smsLength) {
                document.getElementById(_clienttxtMessage).value = sMsg.substring(0, smsLength);
                document.getElementById(_clienttxtMessage).focus();
                return false;
            }
            if (document.getElementById('txt_count') != null) {
                updateTextBoxCounter();
            }
        }


        function GetCharCount(e, sMsg) {
            var iExtraChars = 1;
            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }
            if (isNaN(key)) return true;
            if (key == 8) {
                keychar = sMsg.charAt(sMsg.length - 1);
            } else
                keychar = String.fromCharCode(key);
            // check for backspace or delete, or if Ctrl was pressed

            if (keychar == '^') {
                iExtraChars++;
            }
            else if (keychar == '{') {
                iExtraChars++;
            }
            else if (keychar == '}') {
                iExtraChars++;
            }
            else if (keychar == '\\') {
                iExtraChars++;
            }
            else if (keychar == '[') {
                iExtraChars++;
            }
            else if (keychar == '~') {
                iExtraChars++;
            }
            else if (keychar == ']') {
                iExtraChars++;
            }
            else if (keychar == '|') {
                iExtraChars++;
            }
            else if (key == 0x20AC) {
                iExtraChars++;
            }
            if (key == 8)
                return -(iExtraChars);
            else
                return iExtraChars;
        }

        function updateTextBoxCounter() {

            var unicodeFlag = 0;
            var extraChars = 0;
            var msgCount = 0;
            var sMsgTxt = document.getElementById(_clienttxtMessage).value;
            var TotalCount = 0;
            var i = 0;

            $('#' + _clienthidIsUnicodeSMS).val("0")

            for (; (i < sMsgTxt.length); i++) {
                if ((sMsgTxt.charAt(i) >= '0') && (sMsgTxt.charAt(i) <= '9')) {
                }
                else if ((sMsgTxt.charAt(i) >= 'A') && (sMsgTxt.charAt(i) <= 'Z')) {
                }
                else if ((sMsgTxt.charAt(i) >= 'a') && (sMsgTxt.charAt(i) <= 'z')) {
                }
                else if (sMsgTxt.charAt(i) == '@') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA3) {
                }
                else if (sMsgTxt.charAt(i) == '$') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xEC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF2) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC7) {
                }
                else if (sMsgTxt.charAt(i) == '\r') {
                }
                else if (sMsgTxt.charAt(i) == '\n') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x394) {
                }
                else if (sMsgTxt.charAt(i) == '_') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x393) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39B) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A0) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A3) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x398) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39E) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xDF) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC9) {
                }
                else if (sMsgTxt.charAt(i) == ' ') {
                }
                else if (sMsgTxt.charAt(i) == '!') {
                }
                else if (sMsgTxt.charAt(i) == '\"') {
                }
                else if (sMsgTxt.charAt(i) == '#') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA4) {
                }
                else if (sMsgTxt.charAt(i) == '%') {
                }
                else if (sMsgTxt.charAt(i) == '&') {
                }
                else if (sMsgTxt.charAt(i) == '\'') {
                }
                else if (sMsgTxt.charAt(i) == '(') {
                }
                else if (sMsgTxt.charAt(i) == ')') {
                }
                else if (sMsgTxt.charAt(i) == '*') {
                }
                else if (sMsgTxt.charAt(i) == '+') {
                }
                else if (sMsgTxt.charAt(i) == ',') {
                }
                else if (sMsgTxt.charAt(i) == '-') {
                }
                else if (sMsgTxt.charAt(i) == '.') {
                }
                else if (sMsgTxt.charAt(i) == '/') {
                }
                else if (sMsgTxt.charAt(i) == ':') {
                }
                else if (sMsgTxt.charAt(i) == ';') {
                }
                else if (sMsgTxt.charAt(i) == '<') {
                }
                else if (sMsgTxt.charAt(i) == '=') {
                }
                else if (sMsgTxt.charAt(i) == '>') {
                }
                else if (sMsgTxt.charAt(i) == '?') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xDC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA7) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xBF) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xFC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE0) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x391) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x392) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x395) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x396) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x397) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x399) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39A) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39C) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39D) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39F) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A7) {
                }
                else if (sMsgTxt.charAt(i) == '^') {
                    extraChars++;
                }
                else if (sMsgTxt.charAt(i) == '{') {
                    extraChars++;
                }
                else if (sMsgTxt.charAt(i) == '}') {
                    extraChars++;
                }
                else if (sMsgTxt.charAt(i) == '\\') {
                    extraChars++;
                }
                else if (sMsgTxt.charAt(i) == '[') {
                    extraChars++;
                }
                else if (sMsgTxt.charAt(i) == '~') {
                    extraChars++;
                }
                else if (sMsgTxt.charAt(i) == ']') {
                    extraChars++;
                }
                else if (sMsgTxt.charAt(i) == '|') {
                    extraChars++;
                }
                else if (sMsgTxt.charCodeAt(i) == 0x20AC) {
                    extraChars++;
                }
                else {
                    unicodeFlag = 1;
                    $('#' + _clienthidIsUnicodeSMS).val("1")
                }
                TotalCount = parseInt(i + extraChars);
                if (TotalCount >= smsLength) {
                    sMsgTxt = sMsgTxt.substring(0, i);
                    break;
                }
            }
            if (TotalCount >= smsLength)
                document.getElementById(_clienttxtMessage).value = sMsgTxt;
            if (unicodeFlag) {
                msgCount = sMsgTxt.length;
                if (msgCount <= 70) {
                    msgCount = 1;
                }
                else {
                    msgCount += (67 - 1);
                    msgCount -= (msgCount % 67);
                    msgCount /= 67;
                }
                document.getElementById('txt_count').value = "" + sMsgTxt.length + " unicode characters, " + msgCount + " SMS message(s)";
            }
            else {
                msgCount = sMsgTxt.length + extraChars;
                if (msgCount <= 160) {
                    msgCount = 1;
                }
                else {
                    msgCount += (153 - 1);
                    msgCount -= (msgCount % 153);
                    msgCount /= 153;
                }
                document.getElementById(_clienthidSMSCount).value = msgCount;
                document.getElementById('txt_count').value = "" + (sMsgTxt.length + extraChars) + " characters, " + msgCount + " SMS message(s)";
            }
        }

        function DisableToLink(val) {
            document.getElementById(_clientHlnkSelectUser).disabled = val;
        }

        function CheckReqToUserId(oSrc, args) {
            var strmno = document.getElementById(_clienttxtToUserId).value;
            strmno = trimAll(strmno);

            if (document.getElementById(_clientoptEntireSchool).checked != true && document.getElementById(_clientchkManualNumber).checked != true) {
                if (strmno.length <= 0) {
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }



        function trimAll(sString) {
            while (sString.substring(0, 1) == ' ') {
                sString = sString.substring(1, sString.length);
            }
            while (sString.substring(sString.length - 1, sString.length) == ' ') {
                sString = sString.substring(0, sString.length - 1);
            }
            while (sString.charCodeAt(sString.length - 1) == 10 || sString.charCodeAt(sString.length - 1) == 13) {
                sString = sString.substring(0, sString.length - 1);
            }
            return sString;
        }


        function ClearTextFields() {

            if (document.getElementById(_clientlblMessage) != null)
                document.getElementById(_clientlblMessage).style.display = "none";
            document.getElementById(_clienttxtMessage).value = "Type your SMS here...";
            document.getElementById('txt_count').value = '0 characters, 1 SMS message(s)';
            return false;
        }
        
        function ConfirmSendMessage() {
            Page_IsValid = true;
            var bResult = true
            var isPageValid = true;
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate();
            }
            if (isPageValid) {

                var chkSendMsg = document.getElementById(_clientchkSendMsg)
                if (chkSendMsg.checked == true) {
                    if (!window.confirm('Sms will be sent to selected user(s) and you will be redirected to Message center. Do you want to continue?')) {
                        Page_IsValid = false;
                        bResult = false;
                    }
                }
                return bResult;
            }
        }

        function CheckReqSMSText(oSrc, args) {

            if (document.getElementById(_clientlblMessage) != null)
                document.getElementById(_clientlblMessage).style.display = "none";
            var strmno = document.getElementById(_clienttxtMessage).value;
            strmno = trimAll(strmno);
            if (strmno == "Type your SMS here...") {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function poof3(x, y) {
            xName = x.name;
            x.value = trimAll(x.value);
            xValue = x.value;

            if (xValue == y) {
                x.value = "";
            } else if (xValue == "") {
                x.value = y;
            }
        }


        function CheckValidMobileNos(oSrc, args) {
            if (document.getElementById(_clientchkManualNumber).checked == true) {
                if (document.getElementById(_clienttxtManualNumbers) != null) {
                    var strmno = document.getElementById(_clienttxtManualNumbers).value;
                    if (trimAll(strmno).length == 0) {
                        document.getElementById(_clientcstManualNos).errormessage = "Mobile numbers should not be blank.";
                        args.IsValid = false; return true;
                    }
                    var arr = strmno.replace(/\s/g, "").replace(/,,+/g, ",").split(","); var iTextTypeCnt; var bvalid = true; var bZero = true;
                    var formatedNos = "";
                    for (iTextTypeCnt = 0; iTextTypeCnt < arr.length; iTextTypeCnt++) {
                        strmno = trimAll(arr[iTextTypeCnt]); if (strmno.length == 0) { }
                        else if (strmno.length != 10 || !strmno.match(/^\d{10}$/))
                        { bvalid = false; }
                        else if (strmno.substring(0, 1) == '0')
                        { bZero = false; }
                        else { formatedNos = formatedNos + strmno + ", "; }
                    }
                    
                    var maxCount = parseInt(document.getElementById(_clienthidManualSMSCount).value)
                    if (arr.length > maxCount && trimAll(arr[maxCount]).length > 0) {
                        document.getElementById(_clientcstManualNos).errormessage = "Max " + maxCount + " mobile numbers are allowed.";
                        args.IsValid = false; return true;
                    }
                    else if (!bvalid) {
                        document.getElementById(_clientcstManualNos).errormessage = "Enter 10 digit mobile numbers seperated by comma.";
                        args.IsValid = false; return true;
                    }
                    else if (!bZero) {
                        document.getElementById(_clientcstManualNos).errormessage = "Mobile number should not start with zero.";
                        args.IsValid = false; return true;
                    }
                    formatedNos = formatedNos.substring(0, formatedNos.length - 2);
                    document.getElementById(_clienttxtManualNumbers).value = formatedNos;
                }
            }
            args.IsValid = true;
            return false;
        }

        window.onload = ScheduleSMS;

        ScheduleSMS();

        function ScheduleSMS() {
            var chkSchedule = $get(_clientchkScheduleSMS);
            var chkSendMsg = $get(_clientchkSendMsg);
            if (chkSchedule != null && chkSchedule.checked) {
                $get(_clienttxtPaymentDate).disabled = false;
                $get(_clienttxtStartTime).disabled = false;
                if (chkSendMsg != null) {
                    if (chkSendMsg.checked)
                        chkSendMsg.checked = false;
                    chkSendMsg.disabled = true;
                }
                $get(_clientbtnSendSMS).value = "Schedule SMS";
            }
            else {
                if ($get(_clienttxtPaymentDate) != null) {
                    $get(_clienttxtPaymentDate).disabled = true;
                    $get(_clienttxtPaymentDate).value = '';
                }

                if ($get(_clienttxtStartTime) != null) {
                    $get(_clienttxtStartTime).disabled = true;
                    $get(_clienttxtStartTime).value = "12:00 AM";
                }

                if (chkSendMsg != null)
                    chkSendMsg.disabled = false;
                $get(_clientbtnSendSMS).value = "Send SMS";
            }
        }
        
        /*This code is used to enable radio button after click on the */
        $("input[type=radio][name$=UserType][value!='optEntireSchool']").change(function () {
            $("input[type=radio][name$=UserType][value!='optEntireSchool']").attr('disabled', true);
        });

    </script>
</asp:Content>
