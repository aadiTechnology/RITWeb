<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="PayInternalFeeOnlinePopup.aspx.cs" Inherits="PayInternalFeeOnlinePopup" %>

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
                                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True" Text="Pay Internal Fees Online"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
            
            <tr>
                <td align="left" width="200px" class="ClsBorderlight">
                   <asp:Label ID="lblAcademicYear" runat="server" CssClass="clsLabel" 
                        Text = "<%$ Resources:LocalizedResources, AcademicYear %>"></asp:Label>
                 </td>
                 <td align="left">
                   <asp:DropDownList ID="cmbAcademicYrId" runat="server" ViewStateMode="Enabled" 
                         AutoPostBack="true" CssClass="MidCombo" 
                         onselectedindexchanged="cmbAcademicYrId_SelectedIndexChanged"></asp:DropDownList>
                 </td>
                
             </tr>
             <tr>
             <td align="left" class="ClsBorderlight">
                   <asp:Label ID="Label1" runat="server" CssClass="clsLabel" 
                        Text = "Pending Academic Year(s) : "></asp:Label>
                 </td>
                <td>
                     <asp:Label ID="lblPendingFeeAcademicYear" runat="server" CssClass="clsLabel" Font-Bold="true"></asp:Label>
                </td>
             </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333; height:20px;" valign="top">                
                </td>
            </tr>               
            <tr>
                <td align="center" colspan="2">
                    <asp:UpdatePanel ID="uFeepnl" runat="server" >
						<ContentTemplate>
                            <asp:Panel ID="pnlFields" runat="server" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td align="center">
                                            <table cellpadding="0" cellspacing="0" runat="server" id="tblHeading" visible="True" width="800px"> 
                                                <tr>
                                                    <td>
                                                        <asp:ListView ID="lstvwInternalFee" runat="server" OnItemDataBound="lstvwInternalFee_ItemDataBound"
                                                    DataKeyNames="InternalFeeDetailsId,SerialNumber,FeeDetailsId,DebitCredit,ReceiptNo,InternalFeeMasterId,IsLastCredit,SchoolwiseStudentId,Amount">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" id="tblStudentInfo" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsMarksGridHeader">
                                                                <th id="thchk" runat="server" align="center" width="4%">
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this);" />
                                                                </th>
                                                                <th id="thFeeType" runat="server" align="left" width="15%" style="padding-left: 5px">
                                                                    <asp:Label ID="lblStudent" runat="server" Text="<%$ Resources:LocalizedResources, FeeType %>" EnableViewState="False"></asp:Label>
                                                                </th>
                                                                <th id="thPaybleFor" runat="server" align="left" width="15%" style="padding-left: 5px">
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, PaybleFor %>" EnableViewState="False"></asp:Label>
                                                                </th>
                                                                <th id="thAmount" runat="server" align="right" width="6%" style="padding-right: 5px">
                                                                   <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Amount %>" EnableViewState="False"></asp:Label>
                                                                </th>                                                               
                                                                <th id="thDueDate" runat="server" align="center" width="8%">
                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, DueDate %>" EnableViewState="False"></asp:Label>
                                                                </th>                                                              
                                                                <th id="thPrint" runat="server" align="center" width="6%">
                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, Print %>" EnableViewState="False"></asp:Label>                                                                  
                                                                </th>                                                              
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trlstvwRow" runat="server" class="ClsMarksGridAltRowN">
                                                            <td id="tdchk" runat="server" align="center">
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                <asp:HiddenField ID="hidDueDateKey" runat="server" Value='<%# Eval("PaidDate","{0:yyyyMMdd}") %>' />
                                                            </td>
                                                            <td id="tdFeeType" runat="server" align="left" style="padding-left: 5px">
                                                                <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("FeeType") %>' />
                                                            </td>
                                                            <td id="tdPaybleFor" runat="server" align="left" style="padding-left: 5px">
                                                                <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("PayableFor") %>' />
                                                            </td>
                                                            <td id="tdAmount" runat="server" align="right" style="padding-right: 5px">
                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' />
                                                            </td>                                                          
                                                            <td id="tdDueDate" runat="server" align="center">
                                                                <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate","{0:dd-MMM-yyyy}")%>' />
                                                            </td>                                                           
                                                            <td id="tdPrint" runat="server" align="center">
                                                                <asp:HyperLink ID="hlnkReceipt" runat="server" Text="<%$ Resources:LocalizedResources, Receipt %>" Visible="true" NavigateUrl="InternalFeePaymentReceipt.aspx"> </asp:HyperLink>
                                                                  <asp:HiddenField ID="hidRemark" runat="server" Value='<%# Eval("Remarks") %>' />
                                                            </td>                                                           
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>" EnableViewState="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 11px">
                                                    </td>
                                                </tr>
                                                <tr>
                                            <td align="center">
                                                <table cellpadding="0" cellspacing="2" runat="server" id="Table1" visible="True">
                                                    <tr>
                                                        <td class="" valign="top">
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Pay %>" CssClass="ClsBtn" TabIndex="3"
                                                                UseSubmitBehavior="false" OnClick="btnSave_Click" />
                                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtn" TabIndex="5"
                                                                CausesValidation="False" UseSubmitBehavior="false" />
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
                                <asp:HiddenField ID="hidQueryString" runat="server" />
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidRemark" runat="server" Value="1"/>
        <asp:HiddenField ID="hidStudentId" runat="server" />
        <asp:HiddenField ID="hidNextAcademicYearId" runat="server" />                        
        <asp:HiddenField ID="hidRegNo" runat="server" />                       
        <asp:HiddenField ID="hidIsNextYearFeePayment" runat="server" Value="0" /> 
        <asp:HiddenField ID="hidIsOnlinePayment" runat="server" Value="0" />  
              
        <asp:HiddenField ID="hidSNSSchoolId" runat="server" Value="N" />                  
        </div>

        <script type="text/javascript" src="../Scripts/Validations.js"></script>
         <script language="javascript" type="text/javascript">

             _clienthidRemark = "<%=this.hidRemark.ClientID %>";                                                                           
             _clientlstvwInternalFee = "<%=this.lstvwInternalFee.ClientID %>";                          
             _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";
             _clienthidSNSSchoolId = "<%=this.hidSNSSchoolId.ClientID %>"
             _clientbtnSave = "<%=this.btnSave.ClientID %>"

             var prm = Sys.WebForms.PageRequestManager.getInstance();
             prm.add_endRequest(EndReqHandler);
             prm.add_beginRequest(BeginRequestHandler);

             function EndReqHandler(sender, args) {                 
                 var sEncrypt = document.getElementById(_clienthidQueryString).value;                 

                 window.open("PayFeeOnline.aspx?" + sEncrypt, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=600').focus();
                 return true;                                 
             }

             function BeginRequestHandler(sender, args) {
                 if ($get(_clientbtnSave) != null)
                     $get(_clientbtnSave).disabled = true;
             }                        

             function OpenRecieptPopup(sQueryString) {

                 window.open('InternalFeePaymentReceipt.aspx?' +
                    sQueryString, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=670,height=450');

                 return false;
             }

             function CheckAll(Src) {
              
                 var first = true;
                 var i = 0
                 var chk = $get(_clientlstvwInternalFee + "_ctrl" + i + "_chkSelect");
                 var feeType = $get(_clientlstvwInternalFee + "_ctrl" + i + "_lblFeeType");
                 while (feeType != null) {
                     
                     if (chk != null) {
                         chk.checked = Src.checked;
                         if (first) {                            
                             first = false;
                         }
                         CheckSelected(chk, i);
                     }

                     i++;
                     chk = $get(_clientlstvwInternalFee + "_ctrl" + i + "_chkSelect");
                     feeType = $get(_clientlstvwInternalFee + "_ctrl" + i + "_lblFeeType");
                 }
             }

             function CheckSelected(obj, iRowCount) {
                 var PreviousTotal;
                 var PreviousPayble;
                 var chk = $get(_clientlstvwInternalFee + "_ctrl" + iRowCount + "_chkSelect");
                 if (chk != null) {

                     var lblAmount = $get(_clientlstvwInternalFee + "_ctrl" + iRowCount + "_lblAmount");                     
                 }

                 if ($('#' + _clienthidRemark).val() == "1") {
                     var rmk = ''
                     $("[id$=chkSelect]").each(function () {

                         if ($(this).prop('checked')) {
                             var hd = this.id.replace('chkSelect', 'hidRemark')
                             if ($('#' + hd).val().trim() != "")
                                 rmk = rmk + ', ' + $('#' + hd).val()
                         }
                     })                     
                 }
             }             

             function ConfirmActionForStudent(iPageCount, sActionName) {                 
                 var validationResult = true;                 
                 if (typeof (Page_ClientValidate) == 'function')
                     validationResult = Page_ClientValidate("");
                 if (validationResult == false)
                     return false;

                 if ($get(_clienthidSNSSchoolId).value == "Y") {
                     return CheckIfAtleastOneRadioButtonInGridIsSelected(document, _clientlstvwInternalFee, 'rdoPayFee', sActionName, 'false', iPageCount, 'true');
                 }
                 else
                     return CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientlstvwInternalFee, 'chkSelect', sActionName, 'false', iPageCount, 'true');
             }

             function CloseWindow() {                 
                 window.close();
             }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
