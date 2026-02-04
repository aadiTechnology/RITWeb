<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="AdmissionStatusPopup.aspx.cs" Inherits="AdmissionStatusPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .ClsReceiverHeader
        {
            font-weight: 700;
            font-size: 9pt;
            color: #006;
            text-decoration: none;
            padding-right: 5px;
            height: 20px;
            background-color: #D9E8AA;
            border-bottom-style: solid;
            border-bottom-width: 1px;
        }
        
        .ClsReceiverCell
        {
            background-color: #E4EFC4;
            font-family: Arial;
            font-size: 9pt;
            padding-right: 5px;
        }
    </style>
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td class="ClsGrayMainTitle" align="left">
                    <span class="MainTitleHead">Admission Status Popup</span>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSum" runat="server" />
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Status should be selected."
                                            ControlToValidate="cmbStatus" Display="None" Operator="NotEqual" Type="Integer"
                                            ValueToCompare="0"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateComment"
                                            Display="None"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Follow-up Date should not be blank."
                                            ControlToValidate="txtFollowupDate" Display="None"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                     <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right" valign="top">
                                <span class="ClsMdtStar">&nbsp * Mandatory Fields</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="False" 
                                            Font-Bold="True" ForeColor="Blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="150px">
                                        <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Form No. :"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblFormNo" runat="server" Text="lblFormNo" CssClass="ClsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="150px">
                                        <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Student Name :"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblStudentName" runat="server" Text="lblStudentName" CssClass="ClsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="150px">
                                        <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text="Current Status : "></asp:Label>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblCurrentStatus" runat="server" Text="lblCurrentStatus" CssClass="ClsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="150px">
                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Date :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDate" runat="server" ReadOnly="true" CssClass="MidTxtBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="150px">
                                        <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Status :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbStatus" runat="server" CssClass="MidCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="150px">
                                        <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" Text="Comment :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtComment" runat="server" CssClass="ExLrgTxtBox" Style="width: 350px;
                                            height: 100px;" TextMode="MultiLine"></asp:TextBox>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="150px">
                                        <asp:Label ID="Label7" runat="server" CssClass="ClsLabel" Text="Follow-up Date :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFollowupDate" runat="server" CssClass="MidTxtBox" ReadOnly="True"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_FollowuDate" runat="server" Control="txtFollowupDate" Format="dd MMM yyyy"
                                            From-Today="true" Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Date should not be blank."
                                            AutoPostBack="False" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" OnClick="btnCancel_Click"
                                            CausesValidation="false" UseSubmitBehavior="false" />
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="hidStudentAdmissionId" runat="server" />
                            <asp:HiddenField ID="hidAdmissionStatusDetailsId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidLastCommentId" runat="server" Value="0" />
                        </ContentTemplate>                       
                    </asp:UpdatePanel>
                </td>
            </tr>
            <%--<tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="90%">
                                <tr>
                                    <td>
                                    </td>
                                    <td align="center" width="30px">
                                        <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../images/IconGrid_Edit.gif"
                                            ToolTip="Edit last comment." OnClick="imgBtnEdit_Click" />
                                    </td>
                                    <td align="center" width="30px">
                                        <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="../images/IconGrid_Delete.gif"
                                            ToolTip="Delete last comment." OnClick="imgBtnDelete_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="imgBtnDelete" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>--%>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblComments" runat="server" width="90%" enableviewstate="true">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblPreviousComments" runat="server" Text="Previous Comment(s) : "
                                            Style="font-weight: bold;"></asp:Label>
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
            <tr>
                <td align="center">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="CloseWindow()"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            _clienthidStudentAdmissionId = "<%=this.hidStudentAdmissionId.ClientID %>"
            _clienttxtComment = "<%=this.txtComment.ClientID %>"
            _clientlblMessage = "<%=this.lblMessage.ClientID %>"

            function CloseWindow() {
                var studentAdmissionId = $get(_clienthidStudentAdmissionId).value;
                window.opener.UpdateStaus(studentAdmissionId);
                window.close();
                window.opener.focus();
            }

            function ValidateComment(oSrc, args) {
                var comment = $get(_clienttxtComment).value
                comment = trimAll(comment)
                if (comment.length == 0) {
                    oSrc.errormessage = "Comment should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (comment.length > 500) {
                    oSrc.errormessage = "Comment length should not be greater than 500.";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function ClearMessage() {
                if ($get(_clientlblMessage) != null)
                    $get(_clientlblMessage).innerHTML = "";
            }

        </script>
    </div>
</asp:Content>
