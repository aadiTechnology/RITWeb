<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LibraryRecordsUI.aspx.cs" Inherits="LibraryRecordsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 98%;">
            <tr>
                <td align="right">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Standard should be selected." ValidationGroup="Show"
                                ControlToValidate="cmbStandards" Display="None" Type="Integer" ValueToCompare="0"
                                Operator="NotEqual"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Division should be selected." ValidationGroup="Show"
                                ControlToValidate="cmbDivisions" Display="None" Type="Integer" ValueToCompare="0"
                                Operator="NotEqual"></asp:CompareValidator>
                            <asp:ValidationSummary ID="ValSum" runat="server" CssClass="clsLabel" ValidationGroup="Show" />
                            <asp:ValidationSummary ID="ValSumReturn" runat="server" CssClass="clsLabel" ValidationGroup="Return" />
                            <asp:ValidationSummary ID="ValSumIssue" runat="server" CssClass="clsLabel" ValidationGroup="Issue" />
                            <asp:CustomValidator ID="cstEndTime" runat="server" ErrorMessage="" ClientValidationFunction="ValidateTime" ValidationGroup="Return"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" ClientValidationFunction="ValidateIssueTime" ValidationGroup="Issue"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" ClientValidationFunction="ValidateAccessionNo" ValidationGroup="Issue"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstCompairDateTime" runat="server" ErrorMessage="" ClientValidationFunction="CompairIssueReturnDateTime" Display="None" ValidationGroup="Return"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="ValidateDuplicateAccessionNo" ValidationGroup="Issue"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="" ClientValidationFunction="ValidateIssueRecord" ValidationGroup="Issue"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="" ClientValidationFunction="ValidateReturnRecord" ValidationGroup="Return"
                                Display="None"></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnIssue" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReturn" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center" style="text-align: center; margin: 0px auto;">
                <td align="center" style="text-align: center; margin: 0px solid;">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="text-align: center; margin: 0px solid;" align="center">
                                <tr>
                                    <td id="tdMessage" runat="server" align="center">
                                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnIssue" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReturn" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red" EnableViewState="false"
                                Width="100%" CssClass="ClsMdtStar"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnIssue" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReturn" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left" class="clsBorderLight" style="width: 100px;">
                                <span class="clsLabel">Standard : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbStandards" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                    OnSelectedIndexChanged="cmbStandards_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="clsBorderLight">
                                <span class="clsLabel">Division : </span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbDivisions" runat="server" CssClass="MidCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandards" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr align="center" style="text-align: center;">
                            <td align="left" class="ClsBorderLight">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Date"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtShowDate" CssClass="MidCombo" runat="server"></asp:TextBox>
                                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtShowDate" Format="dd MMM yyyy"
                                    Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                <div><span class="LblSmlGray">(This field is to find record for selected date, if books are not returned.)</span></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnShow" runat="server" ValidationGroup="Show" Text="Show" CssClass="ClsBtn" OnClick="btnShow_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center" style="text-align: center; margin: 0px auto;">
                <td align="center" style="text-align: center; margin: 0px solid;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="trBookIssueReturnDate" runat="server" visible="false" style="width: 25%;
                                text-align: center;" align="center">
                                <tr align="center" style="text-align: center;">
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Text="Issue/Return Date"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtIssueReturnDate" CssClass="SmlTxtBox" runat="server" Width="100px"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_FormOpenDate" runat="server" Control="txtIssueReturnDate" To-Today="true"
                                            Format="dd MMM yyyy" Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                        <asp:TextBox ID="txtIssueReturnTime" CssClass="SmlTxtBox" runat="server" TabIndex="4"
                                            Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 70%">
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwStudents" runat="server" DataKeyNames="UserId,IsAbsent,Id" OnItemDataBound="lstvwStudents_ItemDataBound">
                                            <LayoutTemplate>
                                                <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="center" style="width: 50px">
                                                            <input type="checkbox" id="chkAll" onclick="SetFields()" />
                                                        </th>
                                                        <th align="center" style="width: 100px">
                                                            <span class="clsLabel" style="float: inherit">Is Absent?</span>
                                                        </th>
                                                         <th align="center" style="width: 80px;">
                                                            <span class="clsLabel" style="float: inherit">GR. No</span>
                                                        </th>
                                                        <th align="right" style="width: 60px; padding-right: 5px;">
                                                            <span class="clsLabel" style="float: inherit">Roll No</span>
                                                        </th>
                                                        <th align="left">
                                                            <span class="clsLabel">Student Name</span>
                                                        </th>
                                                        <th align="center" style="width: 150px">
                                                            <span class="clsLabel" style="float: inherit">Accession No.</span>
                                                        </th>
                                                        <th align="center" style="width: 200px">
                                                            <span class="clsLabel" style="float: inherit">Comment</span>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsAbsent" runat="server" />
                                                    </td>
                                                     <td align="center">
                                                        <asp:Label ID="lblGrNo" runat="server" Text='<%#Eval("GrNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="padding-right: 5px;">
                                                        <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabel" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtAccessionNo" runat="server" CssClass="LrgTxtBox" MaxLength="20"
                                                            Text='<%#Eval("BookNo") %>'></asp:TextBox>
                                                        <asp:HiddenField ID="hidBookDetailsId" Value='<%#Eval("Id") %>' runat="server" />
                                                        <asp:HiddenField ID="hidIsAbsent" Value="0" runat="server" />
                                                        <asp:HiddenField ID="hidBookIssueTiming" Value='<%#Eval("IssueTiming") %>' runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="LrgTxtBox" MaxLength="100" Text='<%#Eval("Comment") %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkIsAbsent" runat="server" />
                                                    </td>
                                                      <td align="center">
                                                        <asp:Label ID="lblGrNo" runat="server" Text='<%#Eval("GrNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="padding-right: 5px;">
                                                        <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabel" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtAccessionNo" runat="server" CssClass="LrgTxtBox" MaxLength="20"
                                                            Text='<%#Eval("BookNo") %>'></asp:TextBox>
                                                        <asp:HiddenField ID="hidBookDetailsId" Value='<%#Eval("Id") %>' runat="server" />
                                                        <asp:HiddenField ID="hidIsAbsent" Value="0" runat="server" />
                                                        <asp:HiddenField ID="hidBookIssueTiming" Value='<%#Eval("IssueTiming") %>' runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="LrgTxtBox" MaxLength="100" Text='<%#Eval("Comment") %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />      
                            <asp:AsyncPostBackTrigger ControlID="btnReturn" EventName="Click" />                      
                            <asp:AsyncPostBackTrigger ControlID="btnIssue" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="height10">
                <td align="center">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnIssue" runat="server" Text="Issue" CssClass="ClsBtn" Visible="false" ValidationGroup="Issue"
                                OnClick="btnIssue_Click" />
                            <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="ClsBtn" Visible="false" ValidationGroup="Return"
                                OnClick="btnReturn_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnIssue" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReturn" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script language="ecmascript" type="text/javascript">
        _clientlstvwStudents = "<%=this.lstvwStudents.ClientID %>"
        _clientbtnShow = "<%=this.btnShow.ClientID %>"
        _clienttxtIssueReturnDate = "<%=this.txtIssueReturnDate.ClientID %>"
        _clienttxtIssueReturnTime = "<%=this.txtIssueReturnTime.ClientID %>"
        _clientlstvwStudents = "<%=this.lstvwStudents.ClientID %>"
        _clientbtnIssue = "<%=this.btnIssue.ClientID %>"
        _clientbtnReturn = "<%=this.btnReturn.ClientID %>"


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);


        function EndRequestHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientbtnShow || postBackElement.id == _clientbtnIssue || postBackElement.id == _clientbtnReturn) {
                SetFieldState()
            }
        }

        function EnableDisableRow(rowIndex) {
            var isChecked = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect').checked
            var chkIsAbsent = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkIsAbsent');
            var isAbsent = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkIsAbsent').checked;
            var txtAccessionNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_txtAccessionNo');
            var txtRemark = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_txtRemark');
            var BookDetailsId = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_hidBookDetailsId');
            var IsAbsentVal = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_hidIsAbsent').value;
            
            if (isChecked) {
                if (BookDetailsId.value == 0) {
                    chkIsAbsent.disabled = false
                    txtAccessionNo.disabled = false
                    txtRemark.disabled = false
                }
                else {
                    if (isAbsent) {
                        if (IsAbsentVal) {
                            chkIsAbsent.disabled = true
                            txtAccessionNo.disabled = true
                            txtRemark.disabled = true
                        }
                        else {
                            chkIsAbsent.disabled = false
                            txtAccessionNo.disabled = true
                            txtRemark.disabled = false
                        }
                    }
                    else {
                        chkIsAbsent.disabled = true
                        txtAccessionNo.disabled = true
                        txtRemark.disabled = true
                    }
                }
            }
            else {
                if (isAbsent && BookDetailsId.value != 0) {
                    chkIsAbsent.disabled = true
                    txtAccessionNo.disabled = true
                    txtRemark.disabled = true
                }
                else {
                    chkIsAbsent.checked = false
                    chkIsAbsent.disabled = true
                    txtAccessionNo.disabled = true
                    txtRemark.disabled = true
                }

                if (BookDetailsId.value == 0) {
                    chkIsAbsent.checked = false
                    txtAccessionNo.value = ""
                    txtRemark.value = ""
                }
            }

            if ($("[id$=chkSelect]").length == $("[id$=chkSelect]:checked").length)
                $('#chkAll').prop('checked', true)
            else
                $('#chkAll').removeAttr('checked')
        }

        function SetFieldState() {
            var rowIndex = 0

            var chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            while (chkSelect != null) {
                EnableDisableRow(rowIndex);
                rowIndex++;
                chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            }
        }


        function SetFields() {
            var chked = $('#chkAll').prop('checked')
            $("[id$=chkSelect]").prop('checked', chked)
            SetFieldState()
        }

        function SetAccessionNoState(rowIndex) {
            var chkIsAbsent = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkIsAbsent').checked;
            var txtAccessionNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_txtAccessionNo');
            var txtRemark = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_txtRemark');

            if (chkIsAbsent) {
                txtAccessionNo.disabled = true
                txtRemark.disabled = false
                txtAccessionNo.value=""
            }
            else {
                txtAccessionNo.disabled = false
                txtRemark.disabled = false
            }
        }

        function ValidateTime(oSrc, args) {
            var IssuReturnTime = $('#' + _clienttxtIssueReturnTime).val()
            if (IssuReturnTime.trim() != "") {
                if (!isTimeValid(_clienttxtIssueReturnTime)) {
                    oSrc.errormessage = "Return Time should be in HH:MM AM/PM format (e.g 10:00 AM)."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateIssueTime(oSrc, args) {
            var IssuReturnTime = $('#' + _clienttxtIssueReturnTime).val()
            if (IssuReturnTime.trim() != "") {
                if (!isTimeValid(_clienttxtIssueReturnTime)) {
                    oSrc.errormessage = "Issue Time should be in HH:MM AM/PM format (e.g 10:00 AM)."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        ValidateIssueTime


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

        function CompairIssueReturnDateTime(oSrc, args) {
            var ReturnDate = $('#' + _clienttxtIssueReturnDate).val() + " " + $('#' + _clienttxtIssueReturnTime).val()
            var iRowCount = 0;            
            var sRowNo = "";
            var IssueDt = "";
            var ReturnDt = "";
            var ReturnDtTm = document.getElementById(_clientlstvwStudents + "_ctrl" + iRowCount + "_hidBookIssueTiming")
            while (ReturnDtTm != null) {
                var isChecked = $get(_clientlstvwStudents + '_ctrl' + iRowCount + '_chkSelect').checked
                if (isChecked == true) {
                  
                    ReturnDt= new Date(ReturnDate.replace('-', ' '));
                    IssueDt = new Date(ReturnDtTm.value.replace('-', ' '));

                    if (ReturnDt < IssueDt) {
                        var rollNo = $get(_clientlstvwStudents + '_ctrl' + iRowCount + '_lblRollNo').innerHTML
                        sRowNo = sRowNo + "," + rollNo;
                    }
                }
                iRowCount++;
                ReturnDtTm = document.getElementById(_clientlstvwStudents + "_ctrl" + iRowCount + "_hidBookIssueTiming")
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substr(1);
                oSrc.errormessage = "Return date/time should be greater than issue date/time for Roll No(s) - " + sRowNo;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateAccessionNo(oSrc, args) {
            var rowIndex = 0
            var finalMsg = "";
            var chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            while (chkSelect != null) {
                var chkIsAbsent = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkIsAbsent').checked;
                var txtAccessionNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_txtAccessionNo');
                var rollNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_lblRollNo').innerHTML

                if (chkSelect.checked && !chkIsAbsent && txtAccessionNo.value.trim() == "") {
                    finalMsg = finalMsg + ", " + rollNo;
                }

                rowIndex++;
                chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            }

            if (finalMsg.length > 0) {
                finalMsg = finalMsg.substring(2)
                oSrc.errormessage = "Accession number should not be blank for Roll No(s) : " + finalMsg + ".";
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false
        }

        function ValidateIssueRecord(oSrc, args) {
            var finalMsg = IsBookIssued(true);
            if (finalMsg.length > 0) {
                oSrc.errormessage = "Book is already either issued or marked as Absent for Roll No(s) : " + finalMsg + ".";
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false
        }

        function ValidateReturnRecord(oSrc, args) {
            var finalMsg = IsBookIssued(false);
            if (finalMsg.length > 0) {
                oSrc.errormessage = "No any book is issued for Roll No(s) : " + finalMsg + ".";
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false
        }
        
        function IsBookIssued(isIssue) {
            var rowIndex = 0
            var finalMsg = "";
            var chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            while (chkSelect != null) {
                var txtAccessionNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_txtAccessionNo');
                var rollNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_lblRollNo').innerHTML
                var BookDetailsId = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_hidBookDetailsId').value

                if (chkSelect.checked) {
                    if (isIssue) {
                        if (BookDetailsId.trim() != "" && BookDetailsId.trim() != "0") {
                            finalMsg = finalMsg + ", " + rollNo;
                        }
                    }
                    else {
                        if (BookDetailsId.trim() == "" || BookDetailsId.trim() == "0") {
                            finalMsg = finalMsg + ", " + rollNo;
                        }
                    }
                }

                rowIndex++;
                chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            }

            if (finalMsg.length > 0)
                finalMsg = finalMsg.substring(2)
            return finalMsg;
        }

        var msges = "";
        function ValidateDuplicateAccessionNo(oSrc, args) {
            var rowIndex = 0
            var finalMsg = "";
            var msg = "";
            msges = "";
            var chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            while (chkSelect != null) {
                var chkIsAbsent = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkIsAbsent').checked;
                var txtAccessionNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_txtAccessionNo');
                var rollNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_lblRollNo').innerHTML

                if (chkSelect.checked && !chkIsAbsent && msges.match("," + rowIndex + ",") == null && txtAccessionNo.value.trim() !="") {
                    msg = IsDuplicateAccNo(txtAccessionNo.value.trim(), rowIndex)
                    if (msg != "")
                        finalMsg = finalMsg + ", " + rollNo + "(" + msg + ")";
                }

                rowIndex++;
                chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            }

            if (finalMsg.length > 0) {
                finalMsg = finalMsg.substring(2)
                oSrc.errormessage = "Accession number should not be duplicate for Roll No(s) : " + finalMsg + ".";
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false
        }

        function IsDuplicateAccNo(accNo, rowIndex) {
            rowIndex = rowIndex + 1;
            var duplicate = ""
            var chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            while (chkSelect != null) {
                var chkIsAbsent = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkIsAbsent').checked;
                var txtAccessionNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_txtAccessionNo');
                var rollNo = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_lblRollNo').innerHTML
                if (chkSelect.checked && !chkIsAbsent) {
                    if (accNo == txtAccessionNo.value.trim()) {
                        duplicate = duplicate + ", " + rollNo;
                        msges = msges+","+rowIndex
                    }
                }

                rowIndex++;
                chkSelect = $get(_clientlstvwStudents + '_ctrl' + rowIndex + '_chkSelect')
            }

            if (duplicate.length > 0)
                duplicate = duplicate.substring(2);
            return duplicate;
        }

        function ClearMessage() {
            $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
            $get("<%=this.lblErrorMsg.ClientID %>").innerHTML = "";            
        }

        $(document).ready(function () {
            $("[id$=chkIsAbsent]").click(function () {
                
            })
        })

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
