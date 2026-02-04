<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="MissingAttendancePopup.aspx.cs" Inherits="MissingAttendancePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="left">
                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                    <tr>
                        <td style="height: 20px">
                            <asp:Label ID="lblSendSMS" runat="server" class="MainTitleHead"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right" style="color: #ff3333" valign="middle">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table align="center" width="80%">
                    <tr>
                        <td align="center" id="tdMessage" runat="server" colspan="2">
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="100%" align="center">
                                <tr>
                                    <td valign="middle" class="ClsBorderlight" style="width: 120px">
                                        <asp:Label ID="lblClassName" runat="server" class="ClsLabel" Style="height: 16px;"
                                            Text="Class Name"></asp:Label>
                                        <span class="colonPadding">:</span>
                                    </td>
                                    <td valign="middle" style="width: 120px" class="ClsHilightBGB ">
                                        <asp:Label ID="lblClassNameData" runat="server" class="ClsLabel" Style="height: 16px;"></asp:Label>
                                    </td>
                                    <td valign="middle" class="ClsBorderlight" style="width: 120px">
                                        <asp:Label ID="lblDate" runat="server" class="ClsLabel" Style="height: 16px;" Text="Date"></asp:Label>
                                        <span class="colonPadding">:</span>
                                    </td>
                                    <td valign="middle" style="width: 120px" class="ClsHilightBGB ">
                                        <asp:Label ID="lblDateData" runat="server" class="ClsLabel" Style="height: 16px;"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                   
                </table>
            </td>
        </tr>
        <tr>
            <td style="height:5px;"></td>
        </tr>
         <tr id="trAbsentStudents" runat="server" visible="false" align="center" style="text-align:center; height:20px; width:100%;">
             <td align="center" style="text-align:center" class="ClsHilightBGB">
                 <asp:Label ID="lblAbsentStudents" runat="server" Text=""></asp:Label>
             </td>
        </tr>        
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:ListView ID="lstvwAbsentStudent" runat="server" 
                                DataKeyNames="Mobile_Number,Mobile_Number2,User_Id,FromAbsentDate" 
                                onitemdatabound="lstvwAbsentStudent_ItemDataBound">
                                <LayoutTemplate>
                                <table cellpadding="0" cellspacing="0" width="500px">
                                                <tr>
                                                    <td style="height: 40px" id="trLbl" runat="server" align="left">                                                        
                                                        <asp:Label ID="lblAbsentLedgend" class="ClsLblLgnd" runat="server" Text="Absent Student(s) :"></asp:Label>                                               
                                                    </td>
                                                </tr>                                                
                                 </table>     
                                    <table id="tblNoticeDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                        class="GridBorder" width="80%">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="center" id="thChkSelectAll" runat="server" width="10%;">
                                                <asp:CheckBox ID="chkSelectAll" runat="server" ToolTip="Select student to send SMS."
                                                    onclick="CheckUncheckAll(this);" Checked="true"></asp:CheckBox>
                                            </th>
                                            <th align="left" class="paddingL" width="90%">
                                                Name
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                        <td id="tdChkSelect" runat = "server" align="center">
                                            <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select student to send SMS."
                                                CssClass="check-box" onclick="CheckUncheck(this);" Checked="true"></asp:CheckBox>
                                        </td>
                                        <td align="Left" class="paddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'>
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                        <td id="tdChkSelect" runat = "server" align="center">
                                            <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select student to send SMS."
                                                CssClass="check-box" onclick="CheckUncheck(this);" Checked="true"></asp:CheckBox>
                                        </td>
                                        <td align="Left" class="paddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'>
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <tr style="width: 800px">
                                        <td align="center" class="LblNoRecord">
                                            No record found.
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center">
            <td align="center" colspan="3">
                <asp:Button ID="btnSendSMS" Text="Send SMS" CssClass="ClsBtn" runat="server" CausesValidation="false"
                    TabIndex="1" OnClick="btnSendSMS_Click" OnClientClick="if(!ErrorMessage()) return false;" />
                <asp:HiddenField ID="hidStudentIds" runat="server" Value="" />
                <asp:HiddenField ID="hidStdDivId" runat="server" Value="" />
                <asp:HiddenField ID="hidHalfDayPresentStudentId" runat="server" Value="" />
            </td>
        </tr>
        <tr>
            <td>
                <hr style="color:Green;" />
            </td>
        </tr>
        <tr>
            <td>
                <table id = "tblHalfDayStudents" runat = "server" width="100%">
                    <tr>
                        <td>
                            <asp:ListView ID="lstvwHalfDayAbsentStudentDetails" runat="server" DataKeyNames="Mobile_Number,Mobile_Number2,User_Id,FromAbsentDate">
                                <LayoutTemplate>
                                <table cellpadding="0" cellspacing="0" width="500px">
                                                <tr>
                                                    <td style="height: 40px" id="trLbl" runat="server" align="left">
                                                        <span class="ClsLblLgnd">1/2 Day Absent Student(s) :</span>
                                                    </td>
                                                </tr>
                                            </table>
                                    <table id="tblNoticeDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                        class="GridBorder" width="80%">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="center" width="10%;">
                                                <asp:CheckBox ID="chkSelectAllHalfDayStudent" runat="server" ToolTip="Select student to send SMS."
                                                    onclick="CheckUncheckAllHalfDayStudentListview(this);" Checked="true"></asp:CheckBox>
                                            </th>
                                            <th align="left" class="paddingL" width="90%">
                                                Name
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                        <td align="center">
                                            <asp:CheckBox ID="chkSelectHalfDayStudent" runat="server" ToolTip="Select student to send SMS."
                                                CssClass="check-box" onclick="CheckUncheckHalfDayStudent(this);" Checked="true"></asp:CheckBox>
                                        </td>
                                        <td align="Left" class="paddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'>
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                        <td align="center">
                                            <asp:CheckBox ID="chkSelectHalfDayStudent" runat="server" ToolTip="Select student to send SMS."
                                                CssClass="check-box" onclick="CheckUncheckHalfDayStudent(this);" Checked="true"></asp:CheckBox>
                                        </td>
                                        <td align="Left" class="paddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'>
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <tr style="width: 800px">
                                        <td align="center" class="LblNoRecord">
                                            No record found.
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center">
            <td align="center" colspan="3">
                <asp:Button ID="btnSendSMSToHalfDayAbsentStudent" Text="Send SMS" 
                    CssClass="ClsBtn" runat="server" CausesValidation="false"
                    TabIndex="1" 
                    OnClientClick="if(!ErrorMessageForSecondListView()) return false;" 
                    onclick="btnSendSMSToHalfDayAbsentStudent_Click" />
                    <asp:Button ID="btnClose" Text="Close" CssClass="ClsBtn" runat="server" CausesValidation="false"
                    TabIndex="2" OnClick="btnClose_Click" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        var _clientlstvwExamTypes = '<%= this.lstvwAbsentStudent.ClientID %>';
        var _chkSelect = '_chkSelect';
        var _ctrl = '_ctrl';
        var checkBoxSelector = '#<%= this.lstvwAbsentStudent.ClientID%> input[id*="chkSelect"]:checkbox';
        var _ClientlstvwHalfDayAbsentStudentDetails = '<%= this.lstvwHalfDayAbsentStudentDetails.ClientID %>';
        function CheckUncheckAll(src) {
            if (src == null)
                src = $get(_clientlstvwExamTypes + '_chkSelectAll');

            var iRowCount = 0;
            var chk = $get(_clientlstvwExamTypes + _ctrl + iRowCount + _chkSelect);
            while (chk != null) {
                chk.checked = src.checked;

                iRowCount++;
                chk = $get(_clientlstvwExamTypes + _ctrl + iRowCount + _chkSelect);
            }
        }
        //for 2nd list view
        function CheckUncheckAllHalfDayStudentListview(src) {
            if (src == null)
                src = $get(_ClientlstvwHalfDayAbsentStudentDetails + '_chkSelectAllHalfDayStudent'); 

            var iRowCount = 0;
            var chk = $get(_ClientlstvwHalfDayAbsentStudentDetails + _ctrl + iRowCount + "_chkSelectHalfDayStudent");
            while (chk != null) {
                chk.checked = src.checked;

                iRowCount++;
                chk = $get(_ClientlstvwHalfDayAbsentStudentDetails + _ctrl + iRowCount + "_chkSelectHalfDayStudent");
            }
        }

        function CheckUncheck(src) {
            if (src == null)
                src = $get(_clientlstvwExamTypes + '_chkSelect');
            src1 = $get(_clientlstvwExamTypes + '_chkSelectAll');
            var iRowCount = 0;
            var icheckcount = 0;
            var chk = $get(_clientlstvwExamTypes + _ctrl + iRowCount + _chkSelect);
            while (chk != null) {
                if (chk.checked == true)
                    icheckcount++
                iRowCount++;
                chk = $get(_clientlstvwExamTypes + _ctrl + iRowCount + _chkSelect);
            }
            if (iRowCount == icheckcount) {
                src1.checked = true;
            }
            else {
                src1.checked = false;
            }
        }

        //for 2nd listview
        function CheckUncheckHalfDayStudent(src) {
            if (src == null)
                src = $get(_ClientlstvwHalfDayAbsentStudentDetails + '_chkSelectHalfDayStudent');
            src1 = $get(_ClientlstvwHalfDayAbsentStudentDetails + '_chkSelectAllHalfDayStudent');
            var iRowCount = 0;
            var icheckcount = 0;
            var chk = $get(_ClientlstvwHalfDayAbsentStudentDetails + _ctrl + iRowCount + "_chkSelectHalfDayStudent");
            while (chk != null) {
                if (chk.checked == true)
                    icheckcount++
                iRowCount++;
                chk = $get(_ClientlstvwHalfDayAbsentStudentDetails + _ctrl + iRowCount + "_chkSelectHalfDayStudent");
            }
            if (iRowCount == icheckcount) {
                src1.checked = true;
            }
            else {
                src1.checked = false;
            }
        }

        function ErrorMessage() {
            var iRowCount = 0;
            var icheckcount = 0;
            var chk = $get(_clientlstvwExamTypes + _ctrl + iRowCount + _chkSelect);
            while (chk != null) {
                if (chk.checked == true)
                    icheckcount++
                iRowCount++;
                chk = $get(_clientlstvwExamTypes + _ctrl + iRowCount + _chkSelect);
            }
            if (icheckcount == 0) {
                alert("At lease one student should be selected.");
                return false;
            }
            else
                return true;
        }
        //for 2nd listview
        function ErrorMessageForSecondListView() {
            var iRowCount = 0;
            var icheckcount = 0;
            var chk = $get(_ClientlstvwHalfDayAbsentStudentDetails + _ctrl + iRowCount + "_chkSelectHalfDayStudent");
            while (chk != null) {
                if (chk.checked == true)
				{
                    icheckcount++
					break;
				}
                iRowCount++;
                chk = $get(_ClientlstvwHalfDayAbsentStudentDetails + _ctrl + iRowCount + "_chkSelectHalfDayStudent");
            }
            if (icheckcount == 0) {
                alert("At lease one student should be selected.");
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
