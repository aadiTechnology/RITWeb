<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="StudentRecordCommentPopup.aspx.cs" Inherits="StudentRecordCommentPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr>
                <td align="left" class="ClsGrayMainTitle" valign="middle">
                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; width: 100%;">
                        <tr>
                            <td align="left" class="MainTitleHead" style="height: 20px">
                                <span style="font-weight: bold">Student Record Comment</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="ValSum" runat="server" ValidationGroup="Save"/>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                        ClientValidationFunction="ValidateComment" ValidationGroup="Save"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstTime" runat="server" ErrorMessage="" ClientValidationFunction="ValidateTime"
                            Display="None" ValidationGroup="Save"></asp:CustomValidator>          
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None"
                        ClientValidationFunction="ValidateLectureName" ValidationGroup="Save"></asp:CustomValidator>          
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="98%">
                        <tr>
                            <td align="left" width="15%" class="ClsBorderlight">
                                <span class="ClsLabel">Date : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDate" runat="server" CssClass="SmlTxtBox" MaxLength="11" ReadOnly="true"></asp:TextBox>
                                <rjs:PopCalendar ID="cDate" runat="server" Control="txtDate" Culture="en" Format="dd MMM yyyy"
                                    ShowErrorMessage="false" To-Today="true" ShowWeekend="True" InvalidDateMessage="Invalid date format."
                                    ValidationGroup="Save" />
                                <asp:TextBox ID="txtTime" CssClass="SmlTxtBox" runat="server" TabIndex="4" 
                                        Width="60px"></asp:TextBox>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>                            
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Comment : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtComment" runat="server" CssClass="ExLrgTxtBox" TextMode="MultiLine"
                                    Height="100" Width="95%"></asp:TextBox>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Lecture Name : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtLectureName" runat="server" CssClass="ExLrgTxtBox" TextMode="MultiLine"
                                    Height="50px" Width="95%"></asp:TextBox>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>                        
                        <tr class="height20">
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" 
                                    onclick="btnSave_Click" ValidationGroup="Save" />
                                <asp:Button ID="btnSaveAndSubmit" runat="server" Text="Save and Submit" 
                                    CssClass="ClsBtn" onclick="btnSaveAndSubmit_Click" ValidationGroup="Save" />
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn"  Enabled="false" Visible="false"
                                    CausesValidation="false" onclick="btnSubmit_Click" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="ClsBtn" 
                                    CausesValidation="false" onclick="btnDelete_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false" OnClientClick="window.close()" />
                                <asp:HiddenField ID="hidCommentId" runat="server" Value="0" />
                                <asp:HiddenField ID="hidCurrentTime" runat="server" Value="0" />
                                <asp:HiddenField ID="hidSelectedDateTime" runat="server" Value="0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clienttxtComment = "<%=this.txtComment.ClientID %>"
        _clienttxtTime = "<%=this.txtTime.ClientID %>"
        _clienthidCurrentTime = "<%=this.hidCurrentTime.ClientID %>"
        _clienthidSelectedDateTime = "<%=this.hidSelectedDateTime.ClientID %>"

        function ValidateComment(oSrc, args) {            
            var comment = $('#' + "<%=this.txtComment.ClientID %>").val()
            
            if (comment.trim() == "") {
                oSrc.errormessage = "Comment should not be blank."
                args.IsValid = false
                return true;
            }
            else if (comment.trim().length > 500) {
                oSrc.errormessage = "Comment length should not exceed 500 characters."
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false
        }

        function ValidateLectureName(oSrc, args) {
            var LectureName = $('#' + "<%=this.txtLectureName.ClientID %>").val()

            if (LectureName.trim() == "") {
                oSrc.errormessage = "Lecture Name should not be blank."
                args.IsValid = false
                return true;
            }

            else if (LectureName.trim().length > 300) {
                oSrc.errormessage = "Lecture Name length should not exceed 300 characters."
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function ValidateTime(oSrc, args) {            
            var Time = $('#' + _clienttxtTime).val()
            if (Time.trim() != "") {
                if (!isTimeValid(_clienttxtTime)) {
                    oSrc.errormessage = "Time should be in HH:MM AM/PM format (e.g 10:00 AM)."
                    args.IsValid = false
                    return true
                }
                else if(!ValidateFutureTiming()) {
                    oSrc.errormessage = "Time should not be future time."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateFutureTiming() {            
            var txtTiming = $('#' + _clienthidSelectedDateTime).val()
            var CurrentTiming = $('#' + _clienthidCurrentTime).val()
            if (txtTiming != "" && CurrentTiming != "") {
                var selectedTime;
                var CurrentTime
                if (document.all) {
                    selectedTime = new Date(document.getElementById(_clienthidSelectedDateTime).value.replace('-', ' '));              
                    CurrentTime = new Date(document.getElementById(_clienthidCurrentTime).value.replace('-', ' '));
                }
                else {
                    selectedTime = new Date(txtTiming);
                    CurrentTime = new Date(CurrentTiming);
                }                
                if (selectedTime > CurrentTime) {                    
                    return false;
                }
                
                return true;
            }
        }

        function isTimeValid(txtTimeId) {
            var timeStr = trimAll(document.getElementById(txtTimeId).value.toUpperCase());
            if (trimAll(timeStr) == '')
                return false;

            // Checks if time is in HH:MM 12 hour format.
            // The seconds are optional.
            var timePat = /^(\d{1,2}):(\d{1,2})?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            if (timeStr.length < 6)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            ampm = matchArray[4];

            if (ampm == "") {
                return false;
            }

            if (hour <= 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + minute + '0';
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(txtTimeId).value = str;
            return true;
        }

    </script>
</asp:Content>