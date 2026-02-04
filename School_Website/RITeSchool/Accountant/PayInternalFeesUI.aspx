<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PayInternalFeesUI.aspx.cs" Inherits="PayInternalFeesUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="center">
                    <table width="98%" align="center">
                        <tr>
                            <td colspan="2" width="100%">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" cellpadding="1" cellspacing="2" width="100%">
                                            <tr>
                                                <td align="right">
                                                    <span class="ClsMdtStar">*</span>
                                                   <asp:Label ID="lblmandatoryField" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" />
                                                   
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <table width="100%" id="tblInput" runat="server">
                                                      
                                                        <tr>
                                                            
                                                            <td class="ClsBorderlight" valign="middle" style="width: 150px">
                                                                <asp:Label ID="lblStudNameRegNo" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,StudentNameRegNo %>"></asp:Label>
                                                                 <span class="ClsLabel colonPadding">:</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtRegNo" runat="server" CssClass="LrgTxtBox" Width="290px" MaxLength="50" TabIndex="2" autocomplete="off"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqRegName" Display="None" runat="server" ErrorMessage="<%$ Resources:LocalizedResources,StudentNameRegNoShouldNotBeBlank%>"
															ControlToValidate="txtRegNo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                           <asp:Button ID="btnShow" runat="server" Text="<%$ Resources:LocalizedResources,Show%>"
                                                                    CssClass="ClsBtn" TabIndex="9" Width="100px" OnClick="btnShow_Click" />
                                                            </td>
                                                        </tr>                                                        
                                                    </table>
                                                </td>
                                            </tr>
                                            
                                            <tr runat="server" id="trTotalRec" align="center">
                                                <td align="center" colspan="2">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudent">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources,To%>" />
                                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources,OutOf%>" />
                                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources,Records%>"/>
                                                                    <br />
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" colspan="2" width="1320px">
                                                    <asp:ListView ID="lstvwStudent" runat="server" DataSourcID="objDSStudentList" OnDataBound="lstvwStudent_DataBound"
                                                        OnItemDataBound="lstvwStudent_ItemDataBound" 
                                                        DataKeyNames="SchoolWise_Student_Id,StudentName,TotalAmount,PendingAmount">
                                                        <LayoutTemplate>
                                                            <table width="1300px" runat="server" id="tblStudentInfo" style="color: #333333" cellpadding="0"
                                                                cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" width="12%">
                                                                         <asp:Label ID="lblRegNo" runat="server" Text="<%$ Resources:LocalizedResources, RegNo%>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" width="12%" style="padding-left: 9px;">
                                                                        <asp:Label ID="lblClass" runat="server" Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                    </th>
                                                                   <%-- <th align="left" width="10%" style="padding-left: 9px;">
                                                                    
                                                                            Roll No.
                                                                    </th>--%>
                                                                    <th align="left" width="20%" style="padding-left: 9px;">
                                                                            <asp:Label ID="lblStudentname" runat="server" Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                    </th>
                                                                     <th id="thPaidDate" runat="server" align="right" style="padding-right: 5px;" width="13%">
                                                                            <asp:Label ID="lblPendingFee" runat="server" Text="<%$ Resources:LocalizedResources,PendingFeeRs%>"></asp:Label>
                                                                    </th>
                                                                    <th align="right" width="13%" style="padding-right: 5px;">
                                                                      <asp:Label ID="lblTotalPaybales" runat="server" Text="<%$ Resources:LocalizedResources,TotalPayblesRs%>"></asp:Label>
                                                                    </th>
                                                                    <td id="thPay" runat="server" align="center" width="4%">
                                                                       <asp:Label ID="lblPay" runat="server" Text="<%$ Resources:LocalizedResources,Pay%>"></asp:Label>
                                                                    </td>                                                                
                                                                    <td id="thCustomReceipt" runat="server" align="center" width="13%">
                                                                        <asp:Label ID="lblCustomreceipt" runat="server" Text="<%$ Resources:LocalizedResources,CustomReceipt%>"></asp:Label>
                                                                    </td>
                                                                
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                    <td colspan="7">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudent" PageSize="20">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources,SelectPage%>" runat="server" CssClass="LblNrmlB" />
                                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td align="right" class="LblNormal">
                                                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </PagerTemplate>
                                                                                </asp:TemplatePagerField>
                                                                            </Fields>
                                                                        </asp:DataPager>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblRegNo" runat="server" Text='<%#Eval("Enrolment_Number") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblClass" runat="server" Text='<%#Eval("Class") %>'></asp:Label>
                                                                </td>
                                                               <%-- <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                </td>--%>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblStudentname" runat="server" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                 <td id="tdPaidDate" runat="server" align="right" style="padding-right: 5px;">
                                                                    <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PendingAmount") %>'></asp:Label>
                                                                </td>
                                                                <td align="right" style="padding-right: 5px;">
                                                                    <asp:Label ID="lblAmtPaid" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
                                                                </td>
                                                                
                                                               
                                                                <td id="tdPay" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Pay"
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                
                                                                <td id="tdCustomReceipt" runat="server" align="center">
                                                                    <asp:HyperLink ID="hlnkCustomReceipt" runat="server" Text="<%$ Resources:LocalizedResources,CustomReceipt%>" Visible="true" NavigateUrl="CustomizeInternalRecieptPopUp.aspx"> </asp:HyperLink>
                                                                </td>
                                                                
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblRegNo" runat="server" Text='<%#Eval("Enrolment_Number") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblClass" runat="server" Text='<%#Eval("Class") %>'></asp:Label>
                                                                </td>
                                                                <%-- <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                </td>--%>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblStudentname" runat="server" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td id="tdPaidDate" runat="server" align="right" style="padding-right: 5px;">
                                                                    <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PendingAmount") %>'></asp:Label>
                                                                </td>
                                                                <td align="right" style="padding-right: 5px;">
                                                                    <asp:Label ID="lblAmtPaid" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
                                                                </td>                                                             
                                                                
                                                                <td id="tdPay" runat="server" align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Pay"
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                              
                                                                <td id="tdCustomReceipt" runat="server" align="center">
                                                                    <asp:HyperLink ID="hlnkCustomReceipt" runat="server" Text="<%$ Resources:LocalizedResources,CustomReceipt%>" Visible="true" NavigateUrl="CustomizeInternalRecieptPopUp.aspx"> </asp:HyperLink>
                                                                </td>
                                                              
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                       <asp:Label ID="lblNoRecordFound" runat="server" Text="<%$ Resources:LocalizedResources,NoRecordFound%>"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    <asp:HiddenField ID="hidStandardId" runat="server" />
                                                    <asp:HiddenField ID="hidFeeTypeID" runat="server" />
                                                    <asp:HiddenField ID="hidPageIndex" runat="server" />
                                                    <asp:HiddenField ID="hidbaseUrl" runat="server" />
                                                    <asp:HiddenField ID="hidCultureInfo" runat="server"/>  
                                                    <asp:HiddenField ID="hidShow" runat="server" />
                                                    <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisRecord" runat="server" />                                                  
                                                </td>
                                            </tr>
                                         
                                        </table>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.InternalFeeDetailsBL" EnablePaging="true"
                                            ID="objDSStudentList" runat="server" SelectMethod="GetStudentInternalFeeDetails"
                                            SortParameterName="sortExpression" SelectCountMethod="CountStudents" EnableCaching="false">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="int32" />
                                                <asp:ControlParameter ControlID="txtRegNo" PropertyName="Text" Name="asRegNo" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td width="45%" align="left">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSendSms" Text="<%$ Resources:LocalizedResources,SendSMS%>" CssClass="ClsBtn" runat="server" CausesValidation="false"
                                            TabIndex="10" OnClick="btnSendSms_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>                            
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">


        _clientbtnSave = "<%=this.btnShow.ClientID %>"
        _clienttxtRegNoId = "<%=this.txtRegNo.ClientID %>"        
        
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=hidAreYouSureYouWantToDeleteThisRecord.ClientID%>").value)) {
                bResult = false
            }
            return bResult
        }

        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }

        function OpenPopup(sQueryString) {

            window.open(sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=500');

            return false;
        }

        function OpenRecieptPopup(sQueryString) {

            window.open('InternalFeePaymentReceipt.aspx?' +
                    sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=670,height=450');

            return false;
        }
        function ValidateFeeType(aSrc, args) {
            if (document.getElementById(_clientddlInternalFeeType) != null) {
                if (document.getElementById(_clientddlInternalFeeType).value == 0) {
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }
            else {
                args.IsValid = true
                return false
            }
        }
    
    </script>
	
	<script language="javascript" type="text/javascript">	    
	    _clienttxtRegNumber = '#<%=txtRegNo.ClientID%>';
	    var SchoolId = "<%=miSchoolId %>";
	    var AcademicYearId = "<%=miAcademicYearId %>"
	    
	    $(document).ready(function () {
	        BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, null, 1);
	    });

	    var prm = Sys.WebForms.PageRequestManager.getInstance();
	    prm.add_endRequest(EndReqHandler);

	    function EndReqHandler(sender, args) {
	        BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, null, 1);
	    }

	    function clickButton(e, buttonid) {
	        var evt = e ? e : window.event;
	        var bt = $get(buttonid);
	        if (bt) {
	            if (evt.keyCode == 13) {
	                $('ul').hide();
	            }
	        }
	    }

	    function SearchSelectedValue(val) {
	        txt = document.getElementById("<%=this.txtRegNo.ClientID %>");
	        bt = document.getElementById("<%=this.btnShow.ClientID %>");
	        SearchResult(txt, val, bt);
	    }

    </script>

</asp:Content>
