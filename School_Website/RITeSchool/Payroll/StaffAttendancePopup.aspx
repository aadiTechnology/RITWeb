<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="StaffAttendancePopup.aspx.cs" Inherits="StaffAttendancePopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <%-- <asp:UpdatePanel ID="upnk1" runat="server">
            <ContentTemplate>--%>
        <table width="97%" align="center">
            <tr>
                <td class="ClsGrayMainTitle" align="left" style="width: 99%;">
                    <asp:Label ID="Label3" runat="server" CssClass="MainTitleHead" Text="Datewise Staff Leaves"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                   <table align="right">
                      <tr>
                         <td align="right" style="height: 25px; padding-right:10px;" class="ClsGreenBG">
                             <asp:LinkButton ID="lnkODDetails" runat="server" Text="On Duty (O.D) Details"
                               CssClass="SubTitle"></asp:LinkButton>
                        </td>
                     </tr>
                   </table>                     
               </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                    <asp:CustomValidator ID="cstPartialLeaves" runat="server" ErrorMessage="" Display="None"
                        ClientValidationFunction="ValidatePartialLeaves"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstLeaveBalance" runat="server" ErrorMessage="" Display="None"
                        ClientValidationFunction="ValidateLeaveBalance"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                color: Blue;" EnableViewState="False" Font-Bold="True"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cal_PaymentDate" EventName="SelectionChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lnkPrevious" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="td1" runat="server" align="left">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                color: Red;" EnableViewState="False" Font-Bold="True"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cal_PaymentDate" EventName="SelectionChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lnkPrevious" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="left" width="100px" class="ClsBorderlight">
                                        <asp:Label ID="Label1" runat="server" Text="Date" CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDate" CssClass="SmlTxtBox" runat="server" ReadOnly="true" AutoPostBack="True"
                                            OnTextChanged="txtDate_TextChanged" />
                                        <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Date should not be blank."
                                            AutoPostBack="True" OnSelectionChanged="cal_PaymentDate_SelectionChanged" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="lblStaffGroup" runat="server" CssClass="ClsLabel">Staff Group </asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbStaffGroup" runat="server" Width="150px" Font-Names="Arial"
                                            Font-Size="9pt" AutoPostBack="True" OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="Label2" runat="server" CssClass="ClsLabel">Name </asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClick="btnSearch_Click"
                                            CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cal_PaymentDate" EventName="SelectionChanged" />--%>
                            <asp:AsyncPostBackTrigger ControlID="lnkPrevious" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                            <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlHeader" runat="server" ScrollBars="Vertical" Visible="false">
                                    <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                        cellspacing="1" class="GridBorder">
                                        <tr class="ClsGridHeader">
                                            <th colspan="8" align="center">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" width="50px">
                                                            <asp:LinkButton ID="lnkPrevious" runat="server" OnClick="lnkPrevious_Click" CausesValidation="false"><<</asp:LinkButton>
                                                        </td>
                                                        <td align="center">
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Style="background-color: transparent;
                                                                        float: inherit;"></asp:Label>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="cal_PaymentDate" EventName="SelectionChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="lnkPrevious" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td align="right" width="50px">
                                                            <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click" CausesValidation="false">>></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </th>
                                        </tr>
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="right" style="width: 50px; padding-right: 5px;">
                                                Sr. No.
                                            </th>
                                            <th align="left" style="padding-left: 5px;">
                                                Staff Name
                                            </th>
                                            <th align="left" width="145px" style="padding-left: 5px;">
                                                Designation
                                            </th>
                                            <th align="center" width="120px" id="thLeave">
                                                Leaves
                                                 <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                 <ContentTemplate>
                                                    <asp:DropDownList ID="cmbLeaveHeader" runat="server" CssClass="SmlCombo">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                   <asp:AsyncPostBackTrigger ControlID="cal_PaymentDate" EventName="SelectionChanged" />
                                                   <asp:AsyncPostBackTrigger ControlID="lnkPrevious" EventName="Click" />
                                                   <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </th>
                                            <th align="center" width="90px">
                                                Send SMS?
                                            </th>
                                            <th align="center" width="100px">
                                                Is Half Leave?
                                            </th>
                                            <th align="center" width="100px">
                                                Is Late Mark?
                                            </th>
                                            <th align="center" width="125px">
                                                Partial Leave
                                            </th>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="divContainer" runat="server" ScrollBars="Vertical" Height="370px"
                                            class="GridBorder" Visible="false">
                                            <asp:ListView ID="lstvwUsers" runat="server" DataKeyNames="Id,UserId,LeaveId,MobileNo"
                                                OnItemDataBound="lstvwUsers_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                        cellspacing="1" class="GridBorder">
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="trItem" runat="server" class="ClsGridRow">
                                                        <td align="right" style="width: 50px; padding-right: 5px;">
                                                            <asp:Label ID="lblSrNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("SrNo") %>'></asp:Label>
                                                            <asp:HiddenField ID="hidLeaveDetails" runat="server" Value='<%#Eval("LeaveDetails") %>' />
                                                            <asp:HiddenField ID="hidLeaveBalance" runat="server" Value='<%#Eval("LeaveBalance") %>' />
                                                            <asp:HiddenField ID="hidUserId" runat="server" Value='<%#Eval("UserId") %>' />
                                                        </td>
                                                        <td class="paddingL">
                                                            <%--<asp:LinkButton ID="lblStaffName" runat="server" CssClass="ClsLabel class1" Text='<%#Eval("Name") %>'
                                                                ToolTip="Click here to Show check leave balance."></asp:LinkButton>--%>
                                                            <asp:Label ID="lblStaffName" runat="server" CssClass="ClsLabel class1" Text='<%#Eval("Name") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL" style="width: 150px">
                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLabel" Text='<%#Eval("Designation") %>'></asp:Label>
                                                        </td>
                                                        <td align="center" style="width: 120px">
                                                            <asp:DropDownList ID="cmbLeave" runat="server" CssClass="SmlCombo" EnableViewState="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center" style="width: 90px">
                                                            <asp:CheckBox ID="chkSendSMS" runat="server" />
                                                        </td>
                                                        <td align="center" style="width: 100px">
                                                            <asp:CheckBox ID="chkHalfLeave" runat="server" />
                                                        </td>
                                                        <td align="center" style="width: 100px">
                                                            <asp:CheckBox ID="chkLateMark" runat="server" />
                                                        </td>
                                                        <td align="center" style="width: 125px">
                                                            <asp:DropDownList ID="cmbPartialLeave" runat="server" CssClass="SmlCombo" EnableViewState="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                        <td align="right" style="width: 50px; padding-right: 5px;">
                                                            <asp:Label ID="lblSrNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("SrNo") %>'></asp:Label>
                                                            <asp:HiddenField ID="hidLeaveDetails" runat="server" Value='<%#Eval("LeaveDetails") %>' />
                                                            <asp:HiddenField ID="hidLeaveBalance" runat="server" Value='<%#Eval("LeaveBalance") %>' />
                                                            <asp:HiddenField ID="hidUserId" runat="server" Value='<%#Eval("UserId") %>' />
                                                        </td>
                                                        <td class="paddingL">
                                                            <%--<asp:LinkButton ID="lblStaffName" runat="server" CssClass="ClsLabel class1" Text='<%#Eval("Name") %>'
                                                                ToolTip="Click here to Show check leave balance."></asp:LinkButton>--%>
                                                            <asp:Label ID="lblStaffName" runat="server" CssClass="ClsLabel class1" Text='<%#Eval("Name") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL" style="width: 130px">
                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLabel" Text='<%#Eval("Designation") %>'></asp:Label>
                                                        </td>
                                                        <td align="center" style="width: 120px">
                                                            <asp:DropDownList ID="cmbLeave" runat="server" CssClass="SmlCombo" EnableViewState="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center" style="width: 90px">
                                                            <asp:CheckBox ID="chkSendSMS" runat="server" />
                                                        </td>
                                                        <td align="center" style="width: 100px">
                                                            <asp:CheckBox ID="chkHalfLeave" runat="server" />
                                                        </td>
                                                        <td align="center" style="width: 100px">
                                                            <asp:CheckBox ID="chkLateMark" runat="server" />
                                                        </td>
                                                        <td align="center" style="width: 125px">
                                                            <asp:DropDownList ID="cmbPartialLeave" runat="server" CssClass="SmlCombo" EnableViewState="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:ListView>
                                            <asp:HiddenField ID="hidStaffGroup" runat="server" Value="0" />
                                        </asp:Panel>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cal_PaymentDate" EventName="SelectionChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="lnkPrevious" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table id="tblNote" runat="server" align="center" width="100%">
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                            <asp:Label ID="Label4" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                            <asp:Label ID="Label5" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Please hover mouse on Staff Name or Designation to check available leave balance."></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                            <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note2 :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                            <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="To deduce leave under late mark case please open Staff Leaves screen and save details for respective staff."></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                            <asp:HiddenField ID="hidMonthId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidYear" runat="server" Value="0" />
                            <asp:HiddenField ID="hidLeaveColors" runat="server" Value="" />
                            <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                            <asp:HiddenField ID="hidIsSalaryPublished" runat="server" Value="0" />
                            <asp:HiddenField ID="hidODQueryString" runat="server" Value="0" />
                            <asp:HiddenField ID="hidAllowZeroBalance" runat="server" Value="" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cal_PaymentDate" EventName="SelectionChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lnkPrevious" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkNext" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <br />
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClick="btnClose_Click"
                        CausesValidation="false" />
                </td>
            </tr>
            <%--<tr>
                <td>
                    <asp:HiddenField ID="hidMonthId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidYear" runat="server" Value="0" />
                    <asp:HiddenField ID="hidLeaveColors" runat="server" Value="" />
                    <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                    <asp:HiddenField ID="hidIsSalaryPublished" runat="server" Value="0" />
                </td>
            </tr>--%>
        </table>
        <div id="divLeaveBalance">
            <asp:Label ID="lblLeaveBalance" runat="server" CssClass="ClsLabel" Text=""></asp:Label>
        </div>
        <%--  </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    <script type="text/javascript">
        _clientlstvwUsers = "<%=this.lstvwUsers.ClientID %>"
        _clienthidAllowZeroBalance = "<%=this.hidAllowZeroBalance.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);


        function BeginRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
        }

        function EndRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
            AutoSearch();
        }

        function SetControlState(rowIndex, isMainLeave, isHalfLeave) {
            SetState(rowIndex);
            if (isHalfLeave == 0) {
                if (isMainLeave == 1)
                    SetColor(cmbLeave)
                else
                    SetColor(cmbPartialLeave)
            }
        }

        function ResetMessage() {
            document.getElementById("<%=this.lblMessage.ClientID %>").innerHTML = ''
        }

        function SetLeaves(obj) {
            var rowIndex = 0
            var cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            while (cmbLeave != null) {
                cmbLeave.value = obj.value
                
                SetControlState(rowIndex,1,0)

                rowIndex++;
                cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            }

        }

        function SetStateOfAllControls(rowIndex) {
            SetState(rowIndex);
            if (document.getElementById("<%=this.hidIsSalaryPublished.ClientID %>").value == "1")
                DisableAllFields(true, rowIndex)
            else
                DisableAllFields(false, rowIndex)
        }

        function SetState(rowIndex) {
            cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            chkSendSMS = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkSendSMS")
            chkHalfLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkHalfLeave")
            chkLateMark = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkLateMark")
            cmbPartialLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbPartialLeave")


            if (cmbLeave.value != "0") {
                chkHalfLeave.disabled = false;
                chkSendSMS.disabled = false;


                if (chkHalfLeave.checked) {
                    chkLateMark.disabled = false;
                    cmbPartialLeave.disabled = false;
                }
                else {
                    chkLateMark.checked = false;
                    chkLateMark.disabled = true;
                    cmbPartialLeave.value = "0";
                    cmbPartialLeave.disabled = true;
                }
            }
            else {
                chkLateMark.disabled = false;

                chkSendSMS.checked = false;
                chkSendSMS.disabled = true;

                chkHalfLeave.checked = false;
                chkHalfLeave.disabled = true;

                cmbPartialLeave.value = "0";
                cmbPartialLeave.disabled = true;
            }

            if (parseInt(cmbPartialLeave.value) != 0) {
                chkLateMark.checked = false;
                chkLateMark.disabled = true;
            }
        }

        SetFieldState();
        function SetFieldState() {
            rowIndex = 0
            cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            while (cmbLeave != null) {
                SetStateOfAllControls(rowIndex)
                rowIndex++;
                cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            }
        }

        function DisableAllFields(flag, rowIndex) {
            cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            chkSendSMS = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkSendSMS")
            chkHalfLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkHalfLeave")
            chkLateMark = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkLateMark")
            cmbPartialLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbPartialLeave")

            if (flag) {
                cmbLeave.disabled = flag;
                chkHalfLeave.disabled = flag;
                chkSendSMS.disabled = flag;
                chkLateMark.disabled = flag;
                cmbPartialLeave.disabled = flag;
            }
            else {
                SetState(rowIndex)
            }
        }

        function ShowChangeConfirmation() {
            return window.confirm('With this action you may lose unsaved information (if any). Do you want to continue?');
        }

        function ValidatePartialLeaves(oSrc, args) {
            rowIndex = 0
            srNos = ''
            cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            while (cmbLeave != null) {
                cmbPartialLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbPartialLeave")

                if (parseInt(cmbLeave.value) != 0 && parseInt(cmbLeave.value) == parseInt(cmbPartialLeave.value)) {
                    lblSrNo = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_lblSrNo")
                    srNos = srNos + ", " + lblSrNo.innerHTML
                }

                rowIndex++;
                cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            }

            if (srNos.length > 0) {
                srNos = srNos.substring(1)
                args.IsValid = false;
                oSrc.errormessage = "Leave and Partial Leave should not be same for Sr. No(s) : " + srNos;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateLeaveBalance(oSrc, args) {
            rowIndex = 0
            srNos = ''
            cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            var AllowZero = document.getElementById(_clienthidAllowZeroBalance).value
            var AllowZeroBalance = AllowZero.split(',')

            while (cmbLeave != null) {
                leaveBalance = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_hidLeaveBalance").value
                var leaves = leaveBalance.split(',')
                for (k = 0; k < leaves.length; k++) {
                    var leave = leaves[k].split(':')
                    if (leave.length >= 2) {
                        lblSrNo = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_lblSrNo")
                        isHalfLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkHalfLeave").checked
                        cmbPartialLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbPartialLeave")

                        var LeaveName = ''
                        var AllowZeroValue = 0
                        if (IsMinimumLeaveBalance(parseInt(leave[0]), parseInt(cmbLeave.value), parseInt(leave[1]))) {
                            for (i = 0; i < AllowZeroBalance.length; i++) {
                                var AllowZero = AllowZeroBalance[i]
                                if (AllowZero == parseInt(cmbLeave.value)) {
                                    AllowZeroValue = AllowZeroValue + 1;
                                }
                            }
                            if (AllowZeroValue == 0) {
                                LeaveName = lblSrNo.innerHTML;
                                srNos = srNos + ", " + lblSrNo.innerHTML
                            }
                        }

                        if (IsMinimumLeaveBalance(parseInt(leave[0]), parseInt(cmbPartialLeave.value), parseInt(leave[1]))) {
                            if (LeaveName != lblSrNo.innerHTML)
                                srNos = srNos + ", " + lblSrNo.innerHTML
                        }
                    }
                }

                rowIndex++;
                cmbLeave = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_cmbLeave")
            }

            if (srNos.length > 0) {
                srNos = srNos.substring(1)
                args.IsValid = false;
                oSrc.errormessage = "Leave balance is not sufficient for Sr. No(s) : " + srNos + "."
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function IsMinimumLeaveBalance(leaveId, selectedLeaveId, balance) {
            var srNo = ''
            if (leaveId == selectedLeaveId) {

                var LeaveCount = 1.0
                if (isHalfLeave)
                    LeaveCount = 0.5

                if (balance - LeaveCount < 0) {
                    return true;
                }
            }

            return false;
        }

        function SetColor(obj) {
            var leaveColors = document.getElementById("<%=this.hidLeaveColors.ClientID %>").value;
            var color = 'white'
            var leaves = leaveColors.split(',')
            for (k = 0; k < leaves.length; k++) {
                var leave = leaves[k].split(':')
                if (leave.length >= 0) {
                    if (parseInt(leave[0]) == parseInt(obj.value))
                        obj.style.backgroundColor = leave[1]
                }
            }

        }

        function ClosePopup() {
            queryString = document.getElementById("<%=this.hidQueryString.ClientID %>").value
            window.opener.location = queryString;
            window.close();
            window.opener.focus();
        }

        function OpenPopup(rowIndex) {
        }

        function OpenODDetailsPopup() {
            var sEncryptedString = document.getElementById("<%=this.hidODQueryString.ClientID %>").value           
            window.open('ODDetailsPopup.aspx?' + sEncryptedString, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650')
            return false;
        }        
    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, null, 0);
        }      

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>