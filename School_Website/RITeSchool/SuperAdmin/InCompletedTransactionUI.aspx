<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="InCompletedTransactionUI.aspx.cs" Inherits="InCompletedTransactionUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <%--<asp:LinkButton ID ="lnkbtnGatewayLinkss"  runat="server" Visible="false">Gateway Links</asp:LinkButton>--%>
    <div id="divPaymentGatewayLoginURL" runat="server">
        <asp:HyperLink ID="hlnkGatewayLinks" runat="server" style="text-decoration:underline"></asp:HyperLink>
    </div>
    <div class="MainBodyDiv">
        <asp:UpdatePanel ID="UpdtpnlMain" runat="server">
            <ContentTemplate>
                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                    <tr>
                        <td style="background-color: white;" id="MainDataTable" align="center">
                            <!-- Data Insert Here -->
                            <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:ValidationSummary ID="valErrorMsg" runat="server" CssClass="ClsLabel" />
                                        <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                                            Visible="true" ErrorMessage="Cheque number should not be blank." ClientValidationFunction="ValidateControls"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstPaymentDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                            Visible="true" ErrorMessage="" ClientValidationFunction="ValidatePaymentDate"></asp:CustomValidator>
                                        <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" 
                                            EnableViewState="False" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table align="center" cellpadding="1" cellspacing="2" width="100%">
                                            <tr >
                                                <td class="ClsBorderlight" valign="top">                                                   
                                                    <asp:RadioButton ID="optRegNo" runat="server" Checked="true"  AutoPostBack="true" GroupName="Filter"  
                                                    Text="For Student Fee" OnCheckedChanged="optRegNo_CheckedChanged" />    

                                                    <asp:RadioButton ID="optCautionMoney" runat="server" Visible="false"
                                                        AutoPostBack="true" GroupName="Filter"
                                                    Text="For Caution Money" oncheckedchanged="optCautionMoney_CheckedChanged" />  
                                                    
                                                    <asp:RadioButton ID="optInternalFee" runat="server" Visible="false" AutoPostBack="true" GroupName="Filter"
                                                    Text="For Internal Fee" OnCheckedChanged="optInternalFee_CheckedChanged"/>                                                                                                              
                                                </td>
                                                <td class="ClsBorderlight" valign="top">
                                                        <span class="ClsLabel" id="lblRegNo">Student Name / Reg. No. / Transaction Id :</span>
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtRegNo" runat="server" CssClass="MidTxtBox" MaxLength="50" TabIndex="3"></asp:TextBox>
                                                    <asp:Label ID="lblRegNoMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                        Text="*" Width="14px" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="Tr2" runat="Server">
                                                <td align="center" class="HilightBGGray" colspan="5">
                                                    <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
                                                    <img src="../images/ArrowBlueDblNw.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" valign="top" width="20%">
                                                    <asp:RadioButton ID="optAdmission" runat="server" GroupName="Filter" AutoPostBack="true"
                                                        TabIndex="1" Text="Admission Process" OnCheckedChanged="optAdmission_CheckedChanged" />
                                                </td>
                                                <td valign="top" class="ClsBorderlight" width="25%">
                                                        <span class="ClsLabel" id="lblMobileNo">Mobile Number / Transaction Id / Form No. :</span>
                                                </td>
                                                <td valign="top" align="left" width="60%">
                                                    <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="MidTxtBox" MaxLength="10" TabIndex="2"></asp:TextBox>
                                                    <asp:Label ID="lblMobileMandMark" runat="server" CssClass="ClsMdtStar" Text="*" Height="14px"
                                                        Width="14px"></asp:Label>
                                                </td>
                                            </tr>
                                          
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" valign="top" width="20%">
                                                 <asp:RadioButton ID="optTransactionDate" runat="server" GroupName="Filter" AutoPostBack="true" TabIndex="10" Text="Transaction Date" 
                                                        oncheckedchanged="optTransactionDate_CheckedChanged" />                                                      
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                        TabIndex="4"></asp:TextBox>
                                                    <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtFromDate" Format="dd MMM yyyy"
                                                        ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid from date."
                                                        ControlFocusOnError="True" />
                                                    <asp:Label ID="lblDateMandatory" runat="server" CssClass="ClsMdtStar" Text="*" Height="14px"
                                                        Width="14px"></asp:Label>
                                                </td>
                                            </tr>
                                             <tr>
                                             <td valign="top" class="ClsBorderlight" width="25%">
                                                        <span class="ClsLabel" id="Span3">Type:</span>
                                                </td>
                                                <td class="ClsBorderlight" valign="top" width="20%">
                                                    <asp:RadioButton ID="optIncomplte" runat="server" GroupName="TypeFilter" 
                                                        Checked="true" TabIndex="1" Text="Incomplete" />
                                                        <asp:RadioButton ID="optFail" runat="server" GroupName="TypeFilter" 
                                                         TabIndex="1" Text="Failed"/>
                                                    <asp:RadioButton ID="optSuccessful" runat="server" GroupName="TypeFilter" 
                                                         TabIndex="1" Text="Successful"/>
                                            </tr>
                                           <tr>
                                                <td align="center" valign="top" colspan="3">
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" TabIndex="7"
                                                        Width="100px" OnClick="btnShow_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                <tr id="trLsttran" runat="server" visible="true">
                                    <td>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwTransaction">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                                                    <br />
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <div>
                                                        <asp:ListView ID="lstvwTransaction" runat="server" OnDataBound="lstvwTransaction_DataBound"
                                                            OnItemDataBound="lstvwTransaction_ItemDataBound" OnItemCommand="lstvwTransaction_ItemCommand"
                                                            OnSorting="lstvwTransaction_Sorting" DataKeyNames="NetBankingPaymentTransactionID,AdmissionID,AcedemicYearId,StudentId,FeeAmount,
                                                            TransactionAMT,UserId,GatewayId,BankCode,FullName"> 
                                                           
                                                            <LayoutTemplate>
                                                                <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1" class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th id="th1" runat="server" align="left" class="ClspaddingL" style="width: 16%">
                                                                            Transaction ID
                                                                        </th>
                                                                        <th id="thFormNo" runat="server" align="left" class="ClspaddingL" style="width: 12%">
                                                                            Form No.
                                                                        </th>
                                                                        <th id="thReg" runat="server" align="left" class="ClspaddingL" style="width: 15%">
                                                                            <asp:LinkButton ID="lnkSortRegNo" runat="server" CommandName="Sort" CommandArgument="Enrolment_Number"
                                                                                ForeColor="Black"> Reg. No.</asp:LinkButton>
                                                                        </th>
                                                                        <th align="left" class="ClspaddingL" style="width: 33%">
                                                                            <asp:LinkButton ID="lnkSortName" runat="server" CommandName="Sort" CommandArgument="FullName"
                                                                                ForeColor="Black"> Name</asp:LinkButton>
                                                                        </th>
                                                                        <th id="thMob" runat="server" align="left" class="ClspaddingL" style="width: 13%">
                                                                            <asp:LinkButton ID="lnkMobNo" runat="server" CommandName="Sort" CommandArgument="MobileNumber"
                                                                                ForeColor="Black"> Mob. No.</asp:LinkButton>
                                                                        </th>
                                                                        <th class="ClspaddingL" style="width: 12%" align="right">
                                                                            <asp:LinkButton ID="lnkAmount" runat="server" CommandName="Sort" CommandArgument="TransactionAMT"
                                                                                ForeColor="Black"> Amount</asp:LinkButton>
                                                                        </th>
                                                                        <th  style="width: 12%" align="center">
                                                                            <asp:LinkButton ID="lnkTransactionDate" runat="server" CommandName="Sort" CommandArgument="TransactionDateTime"
                                                                                ForeColor="Black"> Date</asp:LinkButton>
                                                                        </th>
                                                                        <th class="ClspaddingL" style="width: 30%">
                                                                            <asp:LinkButton ID="lnkSortBank" runat="server" CommandName="Sort" CommandArgument="RegisterdBankName"
                                                                                ForeColor="Black"> Bank</asp:LinkButton>
                                                                        </th>
                                                                        <th class="ClspaddingL" style="width: 30%" align="left">
                                                                            <asp:LinkButton ID="lnkGetwayTxnId" runat="server" CommandName="Sort" CommandArgument="TPSLTransactionId"
                                                                                ForeColor="Black"> Gateway Txn Id</asp:LinkButton>
                                                                        </th>
                                                                        <th id="thComplete" runat="Server" >
                                                                            Complete
                                                                        </th>
                                                                         <th id="thIncomplete" runat="Server" >
                                                                            InComplete
                                                                        </th>
                                                                        <th id="thFail" runat="Server">
                                                                            Fail
                                                                        </th>
                                                                        <th id="thDelete" runat="Server">
                                                                            Delete
                                                                        </th>
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                        <td colspan="10">
                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwTransaction">
                                                                                <Fields>
                                                                                    <asp:TemplatePagerField>
                                                                                        <PagerTemplate>
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td align="left">
                                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
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
                                                                <tr class="ClsGridRow" id="trData" runat="server">
                                                                <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblTransactionId" runat="server" Text='<%# Eval("NetBankingPaymentTransactionID") %>' />
                                                                    </td>
                                                                    <td id="tdFormNo" runat="server" align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblFormNo" runat="server" Text='' />
                                                                    </td>
                                                                    <td id="td4" runat="server" align="left" class="ClspaddingL" visible='<%# !Convert.ToBoolean(Eval("IsAdmission")) %>'>
                                                                        <asp:Label ID="lblRegNo" runat="server" Text='<%# Eval("Enrolment_Number") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("FullName") %>' />
                                                                    </td>
                                                                    <td id="td1" runat="server" align="left" class="ClspaddingL" visible='<%# Convert.ToBoolean(Eval("IsAdmission")) %>'>
                                                                        <asp:Label ID="lblMobNo" runat="server" Text='<%# Eval("MobileNumber") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("TransactionAMT") %>' />
                                                                    </td>
                                                                    <td align="center" class="Clspadding">
                                                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("TransactionDateTime","{0:dd-MMM-yyyy}")%>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("RegisterdBankName") %>' />
                                                                    </td>
                                                                     <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("TPSLTransactionId") %>' />
                                                                    </td>
                                                                    <td id="tdbtnComplete" runat="server" visible="false">
                                                                        <asp:Button ID="btnComplete" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            CssClass="ClsBtnSml" Text="Complete" Visible="True" Width="80px" CommandArgument='<%# Eval("NetBankingPaymentTransactionID") %>'
                                                                            ToolTip="Complete" CommandName="Complete" />
                                                                    </td>
                                                                     <td id="tdbtnInComplete" runat="server" visible="false">
                                                                        <asp:Button ID="btnInComplete" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            CssClass="ClsBtnSml" Text="InComplete" Visible="True" Width="80px" 
                                                                            ToolTip="InComplete" CommandName="InComplete" />
                                                                    </td>
                                                                     <td id="tdbtnFail" runat="server" visible="false"> 
                                                                        <asp:Button ID="btnFail" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            CssClass="ClsBtnSml" Text="Fail" Visible="True" Width="80px" ToolTip="Fail" CommandName="Fail" />
                                                                    </td> 
                                                                    <td align="center" id="tdDelete" runat="server" visible="false">
                                                                        <asp:ImageButton ID="imgbtnDeleteTran" CommandArgument='<%# Eval("NetBankingPaymentTransactionID") %>'
                                                                            runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove"
                                                                            ToolTip="Delete" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr class="ClsGridAltRow"  id="trData" runat="server">
                                                                 <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblTransactionId" runat="server" Text='<%# Eval("NetBankingPaymentTransactionID") %>' />
                                                                    </td>
                                                                    <td id="tdFormNo" runat="server" align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblFormNo" runat="server" Text='' />
                                                                    </td>
                                                                    <td id="td4" runat="server" align="left" class="ClspaddingL" visible='<%# !Convert.ToBoolean(Eval("IsAdmission")) %>'>
                                                                        <asp:Label ID="lblRegNo" runat="server" Text='<%# Eval("Enrolment_Number") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("FullName") %>' />
                                                                    </td>
                                                                    <td id="td2" runat="server" align="left" class="ClspaddingL" visible='<%# Convert.ToBoolean(Eval("IsAdmission")) %>'>
                                                                        <asp:Label ID="lblMobNo" runat="server" Text='<%# Eval("MobileNumber") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("TransactionAMT") %>' />
                                                                    </td>
                                                                    <td align="center" class="Clspadding">
                                                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("TransactionDateTime","{0:dd-MMM-yyyy}")%>' />
                                                                    </td>
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("RegisterdBankName") %>' />
                                                                    </td>
                                                                     <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("TPSLTransactionId") %>' />
                                                                    </td>
                                                                    <td id="tdbtnComplete" runat="server" visible="false"> 
                                                                        <asp:Button ID="btnComplete" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            CssClass="ClsBtnSml" Text="Complete" Visible="True" Width="80px" CommandArgument='<%# Eval("NetBankingPaymentTransactionID") %>'
                                                                            ToolTip="Complete" CommandName="Complete" />
                                                                    </td>
                                                                 <td  id="tdbtnInComplete" runat="server" visible="false">
                                                                 <asp:Button ID="btnInComplete" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            CssClass="ClsBtnSml" Text="InComplete" Visible="True" Width="80px" ToolTip="InComplete"
                                                                             CommandName="InComplete" />
                                                                    </td>
                                                                    <td id="tdbtnFail" runat="server" visible="false"> 
                                                                        <asp:Button ID="btnFail" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                            CssClass="ClsBtnSml" Text="Fail" Visible="True" Width="80px" ToolTip="Fail" CommandName="Fail" />
                                                                    </td>
                                                                    <td align="center" id="tdDelete" runat="server" visible="false">
                                                                        <asp:ImageButton ID="imgbtnDeleteTran" CommandArgument='<%# Eval("NetBankingPaymentTransactionID") %>'
                                                                            runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove"
                                                                            ToolTip="Delete" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="LblNoRecord" align="center">
                                                                            No record found.
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentFeeDetailsBL" EnablePaging="true"
                                                        ID="lstDSobj" runat="server" SelectMethod="GetInCompleteTransaction" SortParameterName="sortExpression"
                                                        SelectCountMethod="CountRowsOfInCompleteTransaction" EnableCaching="false">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="int32" />
                                                            <asp:ControlParameter ControlID="txtRegNo" PropertyName="Text" Name="asRegNo" />
                                                            <asp:ControlParameter ControlID="txtFromDate" PropertyName="Text" Name="asTransactionDate" />                                                            
                                                            <asp:ControlParameter ControlID="hidPaymentCategoryFeeId" PropertyName="Value" Name="asPaymentCategoryFeeId" Type="string" />
                                                            <asp:ControlParameter ControlID="optIncomplte" PropertyName="checked" Name="IsIncomplete" Type="Boolean" /> 
                                                            <asp:ControlParameter ControlID="optSuccessful" PropertyName="checked" Name="IsSuccessful" Type="Boolean" /> 
                                                       </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentFeeDetailsBL" EnablePaging="true"
                                                        ID="objAdmission" runat="server" SelectMethod="GetInCompleteAdmissionTransaction"
                                                        SortParameterName="sortExpression" SelectCountMethod="CountRowsOfInCompleteAdmissionTransaction"
                                                        EnableCaching="false">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="int32" />
                                                            <asp:ControlParameter ControlID="txtMobileNumber" PropertyName="Text" Name="asMobileNumber" />
                                                            <asp:ControlParameter ControlID="txtFromDate" PropertyName="Text" Name="asTransactionDate" />
                                                            <asp:ControlParameter ControlID="optIncomplte" PropertyName="checked" Name="IsIncomplete"  Type="Boolean"/> 
                                                            <asp:ControlParameter ControlID="optSuccessful" PropertyName="checked" Name="IsSuccessful" Type="Boolean" /> 
                                                    </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidSendSMS" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidTranscationId" runat="server" />
                                        <asp:HiddenField ID="hidAdmissionId" runat="server" />
                                        <asp:HiddenField ID="hidStudentId" runat="server" />
                                        <asp:HiddenField ID="hidAmount" runat="server" />
                                        <asp:HiddenField ID="hidUserId" runat="server" />
                                        <asp:HiddenField ID="hidAcdYrId" runat="server" />
                                        <asp:HiddenField ID="hidGatewayId" runat="server" />
                                        <asp:HiddenField ID="hidBankCode" runat="server" />
                                        <asp:HiddenField ID="hidAmountInDecimal" runat="server" />
                                        <asp:HiddenField ID="hidPaymentCategoryFeeId" runat="server" />                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divMain" runat="server" class="overlay" style="visibility: hidden; display: none;">
                            </div>
                            <div id="updtpnlPopUp" runat="server" style="visibility: hidden; display: none; position: absolute;
                                margin: 0px; padding: 0px; width: 400px; height: 210px; border-width: 0px; left: 0px;
                                top: 0px; line-height: normal; width: auto; border: solid 1px black; margin: 0px 0px 0px 5px;
                                background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
                                <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                                    background-repeat: repeat-x; padding: 4px; color: Black; text-align: right;">
                                    <div style="padding: 1px; font-size: 12px; font-weight: bold; color: Black; float: left;">
                                        Transaction Details</div>
                                    <span style="cursor: hand" onclick="javascript:HidePopup(false);">
                                        <img alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                    </span>
                                </div>
                                <div style="padding: 10px; text-align: left;" class="ClsLabel">
                                    <table width="400px">
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ID="valSave" runat="server" ShowMessageBox="true" ShowSummary="false"
                                                    CssClass="ClsLabel" ValidationGroup="Complete" />
                                            </td>
                                        </tr>
                                          <tr>
                                            <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="Span2" class="LblNormal">Transaction Number :</span>
                                            </td>
                                            <td class="ClsHilightBGB">
                                               <span id="spnTransactionNumber"  runat = "server" />                                             
                                            </td>
                                        </tr> 
                                          <tr>
                                            <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="Span4" class="LblNormal" style="height:20px;">Name :</span>
                                            </td>
                                            <td class="ClsHilightBGB">
                                               <span id="spnName" class="LblNormal" runat ="server" />                                           
                                            </td>
                                        </tr>                                      
                                        <tr>
                                            <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="lblBankName" class="LblNormal">Bank/Card Name :</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbBankName" runat="server" CssClass="LrgCombo" Width="200"  AutoPostBack="false" CausesValidation="true"></asp:DropDownList>
                                                <asp:CustomValidator ID="cstValidateBank" runat="server" Display="none" EnableClientScript="true"
                                                    ClientValidationFunction="ValidateBank" ErrorMessage="" ValidationGroup="Complete"></asp:CustomValidator>
                                                <span class="ClsMdtStar">* </span>                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="lblAmount" class="LblNormal">Txn. Amount In Rs. :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAmount" CssClass="SmlCombo"   onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);"
                                                    onkeypress="return blockNonNumbers (this, event, true, false);" CausesValidation="true"
                                                     runat="server" MaxLength="10"></asp:TextBox>
                                                <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstValidateAmount" runat="server" Display="none" EnableClientScript="true"
                                                    ClientValidationFunction="ValidateAmount" ErrorMessage="" ValidationGroup="Complete"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight" style="white-space:nowrap">                                                 
                                                <span id="lblTxnId" class="LblNormal">Gateway Transaction ID :</span>
                                            </td>
                                            <td>                                                
                                                <asp:TextBox ID="txtTPSLTransactionID" CssClass="SmlCombo" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                                <span class="ClsMdtStar">* </span>
                                                <asp:RequiredFieldValidator ID="reqTPSLTransactionID" Display="None" runat="server"
                                                    CssClass="ClsMdtStar" ErrorMessage="Transaction ID should not be blank." Visible="true"
                                                    ControlToValidate="txtTPSLTransactionID" ValidationGroup="Complete"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnOk" runat="server" Text="OK" CssClass="ClsBtn" ValidationGroup="Complete" 
                                                    OnClick="btnOk_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                    OnClientClick="javascript:HidePopup(false);return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        _clientoptRegNo = "<%=this.optRegNo.ClientID %>"
        _clienttxtMobileNumber = "<%=this.txtMobileNumber.ClientID %>"
        _clienttxtRegNoId = "<%=this.txtRegNo.ClientID %>"
        _clienthidTranscationId = "<%=this.hidTranscationId.ClientID %>"
        _clienthidAdmissionId = "<%=this.hidAdmissionId.ClientID %>"
        _clienthidStudentId = "<%=this.hidStudentId.ClientID %>"
        _clienthidAmount = "<%=this.hidAmount.ClientID %>"
        _clienthidUserId = "<%=this.hidUserId.ClientID %>"
        _clienthidAcdYrId = "<%=this.hidAcdYrId.ClientID %>"
        _clienthidSendSMS = "<%=hidSendSMS.ClientID %>"
        _clienthidGatewayId = "<%=this.hidGatewayId.ClientID %>"
        _clientcmbBankName = "<%=this.cmbBankName.ClientID %>"
        _clienttxtAmount = "<%=this.txtAmount.ClientID %>";
        _clienthidAmountInDecimal = "<%=this.hidAmountInDecimal.ClientID %>"
        _clientoptCautionMoney = "<%=this.optCautionMoney.ClientID %>"
        _clientoptInternalFee = "<%=this.optInternalFee.ClientID %>"        
        _clienttxtFromDate = "<%=this.txtFromDate.ClientID %>"
        _clientoptTransactionDate = "<%=this.optTransactionDate.ClientID %>"
        _clientoptAdmission = "<%=this.optAdmission.ClientID %>"

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this transaction?')) {
                bResult = false
            }
            return bResult
        }

        function ConfirmCompelte(iTranscationId, iAdmissionID, iAcedemicYearId, iStudentId, iAmount, iUserId,iGatewayId) {
            var bResult = true
            if (window.confirm('Are you sure you want to complete this transaction?')) {
                document.getElementById(_clienthidTranscationId).value = iTranscationId;
                document.getElementById(_clienthidAdmissionId).value = iAdmissionID;
                document.getElementById(_clienthidAcdYrId).value = iAcedemicYearId;
                document.getElementById(_clienthidStudentId).value = iStudentId;
                document.getElementById(_clienthidAmount).value = iAmount;
                document.getElementById(_clienthidUserId).value = iUserId;
                $get(_clienthidGatewayId).value = iGatewayId;
                if (window.confirm('Do you want to send SMS to the student?'))
                    document.getElementById(_clienthidSendSMS).value = "Y";
                else
                    document.getElementById(_clienthidSendSMS).value = "N";
               
                return bResult;
            }
            return false
        }

        function ConfirmFail() {
            return confirm('Are you sure you want to fail this transaction?');
        }
        function ConfirmInCompelte() {
            return confirm('Are you sure you want to mark this transaction as incomplete?');
        }

        function HidePopup(oBtnName) {
            if (oBtnName) {
                var validationResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    validationResult = Page_ClientValidate("")
                }
                if (validationResult == false) {
                    return false
                } 
            }
            $get("<%=this.updtpnlPopUp.ClientID %>").style.visibility = "hidden"
            $get("<%=this.updtpnlPopUp.ClientID %>").style.display = "none"
            var cssstyleMain = $get("<%=this.divMain.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            if (oBtnName) {
                return true
            }
            else {
                return false
            } 
        }
        function ShowPopup() {
            var x, y, tt_ovr_
            var cssstyleMain = $get("<%=this.divMain.ClientID %>").style
            cssstyleMain.visibility = "visible"
            cssstyleMain.display = "block"
            var cssstyle = $get("<%=this.updtpnlPopUp.ClientID %>").style
            var btnReturn = $get("<%=this.btnOk.ClientID %>")
            var now = new Date()
            var width = 250
            var height = 110
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010)
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
        }
        function ValidateControls(oSrc, args) {            
            if ((document.getElementById(_clientoptRegNo) != null && document.getElementById(_clientoptRegNo).checked) || (document.getElementById(_clientoptCautionMoney) != null && document.getElementById(_clientoptCautionMoney).checked) || (document.getElementById(_clientoptInternalFee) != null && document.getElementById(_clientoptInternalFee).checked)) {
                if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtRegNoId).value) == "") {
                    oSrc.errormessage = "Reg. No. or name or Transaction Id should not be blank."
                    args.IsValid = false
                    return true
                }
            }
            else if ((document.getElementById(_clientoptAdmission) != null && document.getElementById(_clientoptAdmission).checked)) {                
               if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtMobileNumber).value) == "") {
                   oSrc.errormessage = "Mobile number or Transaction Id or Form No. should not be blank."
                   args.IsValid = false
                   return true
               }                
            }
        }

        function ValidateBank(src, args) {
            args.IsValid = true;
            var cmbBanks = $get(_clientcmbBankName);
            if (cmbBanks != null && cmbBanks.value == "0") {
                args.IsValid = false;
                src.errormessage = "Bank should be selected.";
            }
            return !args.IsValid;
        }

        function ValidateAmount(src, args) {            
            args.IsValid = true;
            var txtAmount = $get(_clienttxtAmount);
            var AmountInDecimal = $get(_clienthidAmountInDecimal);

            if (txtAmount != null && txtAmount.value.trim() == "") {
                args.IsValid = false;
                src.errormessage = "Txn. Amount In Rs. should not be blank.";
            }
            else if (txtAmount != null && AmountInDecimal != null && parseFloat(txtAmount.value) < parseFloat(AmountInDecimal.value)) {
                args.IsValid = false;
                src.errormessage = "Txn. Amount In Rs should be greater than paid amount.";
            }
            return !args.IsValid;
        }

        function VerifyClose() {
            if (!Page_ClientValidate(""))
                HidePopup(true);
        }
        function OpenGatewayPopup() {
            $("#divPaymentGatewayLoginURL").show();
            $("#divPaymentGatewayLoginURL").kendoWindow({ title: "Gateway Links", visible: false, modal: true, resizable: false, width: '350px' }).data("kendoWindow");
            //var a = $("#divPaymentGatewayLoginURL").show(); ContentWindow = $("#divPaymentGatewayLoginURL").kendoWindow({ title: "Gateway Links", visible: false, modal: true, resizable: false, width: '350px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }

        function ValidatePaymentDate(src, args) {                        
            var txtPaymentDate = $get(_clienttxtFromDate);
            if (txtPaymentDate.value.trim() == "" && (document.getElementById(_clientoptTransactionDate) != null && document.getElementById(_clientoptTransactionDate).checked)) {
                src.errormessage = "Transaction Date should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
        }

    </script>
</asp:Content>
