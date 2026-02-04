<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PreprimaryProgressReportRemarkConfigListUI.aspx.cs"
    Inherits="PreprimaryRemarksUI" ValidateRequest="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
         <tr align="center" id="trValSummary" runat="server">
                <td align="center">
                    <asp:ValidationSummary ID="valSummRemarks" CssClass="LblErrorMsg" ShowSummary="true"
                        runat="server" />
                </td>
            </tr>
            <tr align="left" id="trErrorMessage" runat="server" visible="false">
                <td align="left">
                    <table align="left">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                    EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr align="center">
                <td align="center">
                    <div style="width: 50%; overflow: auto;">
                        <div id="divSortOrder" runat="server" align="center" class="ToprLinkHlilight" style="width: 120px;
                            height: 18px; float: right" visible="false">
                            <asp:HyperLink ID="hlnkSortOrder" runat="server" CssClass="ClsHilightTextB" NavigateUrl="SortRemarkPopup.aspx"
                                Target="_blank">Sort Order</asp:HyperLink>
                        </div>
                    </div>
                </td>
            </tr>
              <tr align="center">
                <td align="center">
                    <div id="Div1" style="width: 45%; overflow: auto;">
                        <asp:ListView ID="lstvwPrePrimaryProgressRemark" runat="server" 
                            
                            DataKeyNames="SchoolId,AcademicYearId,PrePrimaryProgressReportRemarkId,OriginalPrePrimaryProgressReportRemarkId,SortOrder" 
                            onitemdatabound="lstvwPrePrimaryProgressRemark_ItemDataBound" 
                              >
                            <LayoutTemplate>
                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                    cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="center" width="30px">
                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAllsStaffGroups()" />
                                        </th>
                                        <th align="left" style="padding-left: 10px;">
                                            Remark Name
                                        </th>
                                       <th align="center" >
                                            Sort Order
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="trItem" runat="server" class="ClsGridRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td class="paddingL">
                                        <asp:TextBox ID="txtRemark" runat="server" MaxLength="50" Text='<%#Eval("PrePrimaryProgressReportRemarkName") %>'></asp:TextBox>
                                    </td>
                                   <td align="center" >
                                        <asp:DropDownList ID="cmbSortOrder" runat="server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td class="paddingL">
                                        <asp:TextBox ID="txtRemark" runat="server" MaxLength="50" Text='<%#Eval("PrePrimaryProgressReportRemarkName") %>'></asp:TextBox>
                                    </td>
                                  <td align="center" >
                                        <asp:DropDownList ID="cmbSortOrder" runat="server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                        <asp:CustomValidator ID="cstvalRemark" runat="server" ClientValidationFunction="ValidateRemarkName"
                            SetFocusOnError="True" Display="None" ErrorMessage="Selected remark name should not be blank."></asp:CustomValidator>
                        <asp:CustomValidator ID="cstvalDuplicateValue" runat="server" ClientValidationFunction="DuplicateValue"
                            SetFocusOnError="True" Display="None" ErrorMessage="You have entered duplicate value for selected remark name."></asp:CustomValidator>
                            <asp:CustomValidator ID="cstDuplicateSortOrder" runat="server" ClientValidationFunction="ValidateDuplicateSortOrder"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        <asp:CustomValidator ID="cstSortOrder" runat="server" ClientValidationFunction="ValidateSortOrder"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                    &nbsp;
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" 
                        BorderWidth="1px" onclick="BtnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="false" UseSubmitBehavior="false" />
                </td>
            </tr>
        
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientSaveId = "<%=this.BtnSave.ClientID %>"
        _clientcstSortOrder = "<%=this.cstSortOrder.ClientID %>";
         _clientbtnCancelId = "<%=this.btnCancel.ClientID %>"
         _clientlstvwPrePrimaryProgressRemark = "<%=this.lstvwPrePrimaryProgressRemark.ClientID %>"
         _ClientcstvalRemark = "<%=this.cstvalRemark.ClientID %>"
         _ClientvalSummRemarks = "<%=this.valSummRemarks.ClientID %>"
         _ClientcstvalDuplicateValue = "<%=this.cstvalDuplicateValue.ClientID %>"
         _clienttrErrorMessage = "<%=this.trErrorMessage.ClientID %>"
         _clientcstDuplicateSortOrder = "<%=this.cstDuplicateSortOrder.ClientID %>";

         function CheckAllUncheckAllsStaffGroups() {
             var checkAll = document.getElementById("ctl00_MainBody_lstvwPrePrimaryProgressRemark_ChkSelectAll").checked
             var chk
             var iRowCount = 0
             if (iRowCount < 10)
                 chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_ChkSelect")
             else
                 chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_ChkSelect")
             while (chk != null) {
                 chk.checked = checkAll
                 iRowCount = iRowCount + 1
                 if (iRowCount < 10)
                     chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_ChkSelect")
                 else
                     chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_ChkSelect")
             }
         }
         function DuplicateValue(oSrc, args) {
             if (DuplicateText(document, _clientlstvwPrePrimaryProgressRemark, "_ChkSelect", "_txtRemark")) {
                 args.IsValid = false
                 return true
             }
             else {
                 args.IsValid = true
                 return false
             }
         }

         function CheckSelectedRemark(objBtn) {
             var errMsg = document.getElementById(_clienttrErrorMessage)
             if (errMsg != null)
                 errMsg.style.display = "none"
             var bResult = true
             if (CheckSelection(_clientlstvwPrePrimaryProgressRemark, 'ChkSelect')) {
                 bResult = true
             }
             else {
                 $get(_ClientvalSummRemarks).style.display = "none"
                 alert("At least one remark should be selected.")
                 bResult = false
             }
             return bResult
         }

         function ValidateRemarkName(oSrc, args) {
             var chk
             var sMessage = false
             var sSortMessage=false
             var iRowCount = 0
             chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_ChkSelect")
             while (chk != null) {
                 if (chk.checked == true) {
                     txtRemarks = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_txtRemark")
                     if (txtRemarks.value.trim() == "")
                         sMessage = true
                 }
                 iRowCount = iRowCount + 1
                 chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_ChkSelect")
             }
             if (sMessage == true) {
                 $get(_ClientcstvalRemark).errormessage = "Selected remark name should not be blank."
                 args.IsValid = false
                 return true
             }
             args.IsValid = true
             return false
         }

         function ValidateDuplicateSortOrder(oSrc, args) {
             var iRowCount = 0;
             var sortOrders = "";
             var isDuplicate = false;

             var sCnt = "";
             chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_ChkSelect");
             cmb = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_cmbSortOrder");

             while (chk != null) {
                 if (chk.checked == true) {
                     if (cmb.value != 0) {
                         if (sortOrders.match("," + cmb.value + ",") != null) {
                             isDuplicate = true;
                             if (sCnt != "")
                                 sCnt = sCnt + ", " + (iRowCount + 1);
                             else
                                 sCnt = (iRowCount + 1);
                         }
                         else {
                             if (cmb.value != "9999")
                                 sortOrders = sortOrders + "," + cmb.value + ",";
                         }
                     }
                 }

                 iRowCount = iRowCount + 1;
                 chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + (iRowCount) + "_ChkSelect")
                 cmb = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + (iRowCount) + "_cmbSortOrder");
             }
             if (isDuplicate) {
                 document.getElementById(_clientcstDuplicateSortOrder).errormessage = "Remark sort order should not be duplicate for row(s) : " + (sCnt) + ".";
                 document.getElementById(_clientcstDuplicateSortOrder).innerHTML = "Remark sort order should not be duplicate for row(s) : " + (sCnt) + ".";
                 args.IsValid = false;
             }
             if (args.IsValid == false)
                 return true;

             if (args.IsValid == true)
                 return false;
         }

         function ValidateSortOrder(oSrc, args) {
             var iRowCount = 0;
             var sortOrders = "";
             var notSelected = true;
             var isDuplicate = false;
             var sCount = "";
             var sCnt = "";
             chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_ChkSelect");
             cmb = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + iRowCount + "_cmbSortOrder");
             document.getElementById(_clientcstSortOrder).errormessage = "";
             while (chk != null) {
                 if (chk.checked == true) {
                     if (cmb.value == "0") {
                         notSelected = false;
                         if (sCount != "")
                             sCount = sCount + ", " + (iRowCount + 1);
                         else
                             sCount = (iRowCount + 1);
                     }
                     else {
                         if (sortOrders.match("," + cmb.value + ",") != null) {
                             isDuplicate = true;
                             if (sCnt != "")
                                 sCnt = sCnt + ", " + (iRowCount + 1);
                             else
                                 sCnt = (iRowCount + 1);
                         }
                         else {
                             if (cmb.value != "9999")
                                 sortOrders = sortOrders + "," + cmb.value + ",";
                         }
                     }
                 }
                 else {
                     cmb.value = "0";
                 }
                 iRowCount = iRowCount + 1;
                 chk = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + (iRowCount) + "_ChkSelect")
                 cmb = document.getElementById(_clientlstvwPrePrimaryProgressRemark + "_ctrl" + (iRowCount) + "_cmbSortOrder");
             }
             if (!notSelected) {
                 document.getElementById(_clientcstSortOrder).errormessage = "Remark sort order should be selected for row(s) : " + (sCount) + ".";
                 document.getElementById(_clientcstSortOrder).innerHTML = "Remark sort order should be selected for row(s) : " + (sCount) + ".";
                 args.IsValid = false;

             }
             if (args.IsValid == false)
                 return true;

             if (args.IsValid == true)
                 return false;
         }
    </script>
</asp:Content>
