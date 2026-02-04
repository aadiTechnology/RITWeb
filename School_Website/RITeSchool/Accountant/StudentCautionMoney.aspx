<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentCautionMoney.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" Inherits="StudentCautionMoney" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
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
                                                <td>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" />
                                                    <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        Visible="true" ErrorMessage="<%$ Resources:LocalizedResources, ChequeNumberShouldNotBeBlank%>"
                                                        ClientValidationFunction="ValidateControls"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td align="right" style="width: 144px; height: 25px; padding-right: 15px; padding-left: 0px;"
                                                                class="ClsGreenBG" runat="server" id="tdBank">
                                                                <asp:HyperLink ID="hlnkBankDetails" runat="server" Text="<%$ Resources:LocalizedResources, AddBankName%>"
                                                                    NavigateUrl="BankDetailsPopup.aspx" CssClass="SubTitle " />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" colspan="2">
                                                    <span class="ClsLblLgnd" style="height: 20px">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, SelectFilter%>"></asp:Label>
                                                        <span class="colonPadding">:</span></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <table width="100%" id="tblInput" runat="server">
                                                        <tr>
                                                            <td class="ClsBorderlight" valign="top" width="5%">
                                                                <asp:RadioButton ID="optChequeNumber" runat="server" GroupName="Filter" AutoPostBack="true"
                                                                    Checked="true" OnCheckedChanged="optChequeNumber_CheckedChanged" TabIndex="1" />
                                                            </td>
                                                            <td valign="top" class="ClsBorderlight" width="30%">
                                                                <span class="ClsLabel">
                                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, ChequeNumber%>"></asp:Label>
                                                                    <span id="Span2" class="colonPadding">:</span></span>
                                                            </td>
                                                            <td valign="top" align="left" width="70%">
                                                                <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="LrgTxtBox" MaxLength="6"
                                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                                    ondrop="event.returnValue=false;" TabIndex="2"></asp:TextBox>
                                                                <asp:Label ID="lblChequeNumberMandMark" runat="server" CssClass="ClsMdtStar" Text="*"
                                                                    Height="14px" Width="14px" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="HilightBGGray" colspan="3">
                                                                <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB"><asp:Label
                                                                    ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, OR%>"></asp:Label>
                                                                </span>
                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ClsBorderlight" valign="top" width="5%">
                                                                <asp:RadioButton ID="optRegNo" runat="server" AutoPostBack="true" GroupName="Filter"
                                                                    OnCheckedChanged="optRegNo_CheckedChanged" />
                                                            </td>
                                                            <td class="ClsBorderlight" valign="top">
                                                                <span class="ClsLabel">
                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, SelectstudentNameRegNo%>"></asp:Label>
                                                                    <span id="Span1" class="colonPadding">:</span></span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtRegNo" runat="server" CssClass="LrgTxtBox" MaxLength="50" TabIndex="3"></asp:TextBox>
                                                                <asp:Label ID="lblRegNoMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                    Text="*" Width="14px" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="HilightBGGray" colspan="3">
                                                                <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB"><asp:Label
                                                                    ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, OR%>"></asp:Label>
                                                                </span>
                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="1" valign="top" class="ClsBorderlight">
                                                                <asp:RadioButton ID="optDate" runat="server" AutoPostBack="true" GroupName="Filter"
                                                                    OnCheckedChanged="optDate_CheckedChanged" />
                                                            </td>
                                                            <td valign="top" colspan="2">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="ClsBorderlight" style="width:250px">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, PaymentStartDate%>"></asp:Label>
                                                                                <span id="Span3" class="colonPadding">:</span> </span>
                                                                        </td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                                TabIndex="4"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtFromDate" Format="dd MMM yyyy"
                                                                                Culture="en" ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, PleaseSelectValidFromDate%>"
                                                                                ControlFocusOnError="True" />
                                                                            <asp:Label ID="lblFromDateMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                                Text="*" Width="14px" Visible="False"></asp:Label>
                                                                        </td>
                                                                        <td class="ClsBorderlight" style="width:250px">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, Dew_Date%>"></asp:Label>
                                                                                <span id="Span4" class="colonPadding">:</span></span>
                                                                        </td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="SmlTxtBox" MaxLength="11" TabIndex="5"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="cToDate" runat="server" Control="txtToDate" Format="dd MMM yyyy"
                                                                                Culture="en" ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, PleaseSelectValidFromDate%>" />
                                                                            <asp:Label ID="lblToDateMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                                Text="*" Width="14px" Visible="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="center" class="HilightBGGray" colspan="3">
                                                                <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB"><asp:Label
                                                                    ID="Label16" runat="server" Text="<%$ Resources:LocalizedResources, OR%>"></asp:Label>
                                                                </span>
                                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="1" valign="top" class="ClsBorderlight">
                                                                <asp:RadioButton ID="optReturnDate" runat="server" AutoPostBack="true" 
                                                                    GroupName="Filter" oncheckedchanged="optReturnDate_CheckedChanged" />
                                                            </td>
                                                            <td valign="top" colspan="2">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="ClsBorderlight" style="width:250px">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label12" runat="server" Text="Return Start Date"></asp:Label>
                                                                                <span id="Span5" class="colonPadding">:</span> </span>
                                                                        </td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox ID="txtReturnStartDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                                                TabIndex="4"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="calReturn1" runat="server" Control="txtReturnStartDate" Format="dd MMM yyyy"
                                                                                Culture="en" ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid Return Start Date."
                                                                                ControlFocusOnError="True" />
                                                                            <asp:Label ID="Label13" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                                Text="*" Width="14px" Visible="False"></asp:Label>
                                                                        </td>
                                                                        <td class="ClsBorderlight" style="width:250px">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label14" runat="server" Text="End Date"></asp:Label>
                                                                                <span id="Span6" class="colonPadding">:</span></span>
                                                                        </td>
                                                                        <td align="left" valign="top">
                                                                            <asp:TextBox ID="txtReturnEndDate" runat="server" CssClass="SmlTxtBox" MaxLength="11" TabIndex="5"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="calReturn2" runat="server" Control="txtReturnEndDate" Format="dd MMM yyyy"
                                                                                Culture="en" ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid Return End Date." />
                                                                            <asp:Label ID="Label15" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                                                Text="*" Width="14px" Visible="False"></asp:Label>
                                                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Return Start Date should be less than or equal to End Date." Display="None" ClientValidationFunction="ValidatereteurnDates"></asp:CustomValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="HilightBGGray" colspan="2">
                                                    <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB"><asp:Label
                                                        ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, AND1%>"></asp:Label>
                                                    </span>
                                                    <img src="../images/ArrowBlueDblNw.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="ClsBorderlight">
                                                                <asp:RadioButton ID="optCMNotPaid" runat="server" GroupName="grpOption" 
                                                                    Text="<%$ Resources:LocalizedResources, StudentWhoHavenotPaidACautionMoney%>" 
                                                                    AutoPostBack="True" oncheckedchanged="optCMNotPaid_CheckedChanged" />
                                                            </td>
                                                            <td class="ClsBorderlight">
                                                                <asp:RadioButton ID="optCMPaid" runat="server" GroupName="grpOption" 
                                                                    Text="<%$ Resources:LocalizedResources, StudentWhoHavePaidACautionMoney%>" 
                                                                    AutoPostBack="True" oncheckedchanged="optCMPaid_CheckedChanged" />
                                                            </td>
                                                            <td class="ClsBorderlight">
                                                                <asp:RadioButton ID="optCMReturned" runat="server" GroupName="grpOption" AutoPostBack="true"
                                                                    Text="<%$ Resources:LocalizedResources, StudentsToWhomCautionMoneyIsReturned%>" 
                                                                    oncheckedchanged="optCMReturned_CheckedChanged" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" colspan="2">
                                                    <asp:Button ID="btnShow" runat="server" Text="<%$ Resources:LocalizedResources, Show%>"
                                                        CssClass="ClsBtn" TabIndex="5" OnClick="btnShow_Click" UseSubmitBehavior="false"
                                                        Width="100px" />
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trTotalRec" align="center">
                                                <td align="center" colspan="2">
                                                    <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                    <span class="LblNrmlB">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, To%>"></asp:Label></span>
                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                    <span class="LblNrmlB">
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, OutOf%>"></asp:Label></span>
                                                    <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                    <span class="LblNrmlB">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, Records%>"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" colspan="2">
                                                    <asp:GridView ID="grdStudents" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CellPadding="0" CellSpacing="1" OnRowDataBound="grdStudents_RowDataBound" ForeColor="#333333"
                                                        GridLines="None" BackColor="White" AllowSorting="True" DataKeyNames="Schoolwise_Student_Id,Return_Cheque_Id,Payment_Cheque_Id,Paid_By_Student,Returned_By_School,SchoolLeft_Date,Admission_Date,Payment_Date,Is_RTE_Student,ElectronicPaymentId,NetBankingPaymentTransactionID,ReceiptNumber"
                                                        CssClass="GridBorder" AllowPaging="True" OnPageIndexChanging="grdStudents_PageIndexChanging"
                                                        OnSorting="grdStudents_Sorting" OnRowCreated="grdStudents_RowCreated" 
                                                        onrowcommand="grdStudents_RowCommand">
                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                        </PagerStyle>
                                                        <Columns>
                                                            <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, RegNumber%>" DataField="Enrolment_Number"
                                                                SortExpression="Enrolment_Number">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="11%" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingL" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Form Number" DataField="FormNumber"  Visible="false">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="11%" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingL" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, StudentName%>" DataField="StudentName"
                                                                SortExpression="StudentName">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="30%" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingL" />
                                                            </asp:BoundField>
                                                             <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, DateofBirth%>" DataField="DOB"
                                                                SortExpression="DOB" HtmlEncode="False" DataFormatString="{0:dd MMM yyyy}">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="12%" />
                                                            </asp:BoundField>
                                                             <asp:BoundField HeaderText="School Left Date" DataField="SchoolLeft_Date"
                                                            NullDisplayText="-" DataFormatString="{0:dd MMM yyyy}" >
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="12%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, AmountRs%>" DataField="Amount">
                                                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" CssClass="ClspaddingR"
                                                                    Width="12%" />
                                                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="false" Width="12%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, CautionMoneyPaidDate%>" DataField="Payment_Date"
                                                                NullDisplayText="-" SortExpression="Payment_Date" HtmlEncode="False" DataFormatString="{0:dd MMM yyyy}">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Wrap="false" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="8%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, ReturnedDate%>" DataField="Return_Date"
                                                                NullDisplayText="-" SortExpression="Return_Date" HtmlEncode="False" DataFormatString="{0:dd MMM yyyy}">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="8%" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField ButtonType="Image" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                HeaderText="<%$ Resources:LocalizedResources, Pay%>">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField ButtonType="Image" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                HeaderText="<%$ Resources:LocalizedResources, Return%>">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                                            </asp:ButtonField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Reciept%>">
                                                                <ItemTemplate>                                                                    
                                                                    <asp:LinkButton ID="lnkReciept" runat="server" CommandArgument="<%# Container.DataItemIndex %>" CommandName="RECEIPT">Receipt</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Delete%>">
                                                                <ItemTemplate>                                                                    
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="REMOVE"
                                                                    ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"/>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                            </asp:TemplateField>                                                             
                                                        </Columns>
                                                        <RowStyle CssClass="ClsGridRow" />
                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center"
                                                            VerticalAlign="Middle" />
                                                        <PagerTemplate>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                        <span class="LblNrmlB">
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, SelectAPage%>"></asp:Label></span>
                                                                        <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                            OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </PagerTemplate>
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hidChequeId" runat="server" />
                                                    <asp:HiddenField ID="hidCautionMode" runat="server" />
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    <asp:HiddenField ID="hidQueryString" runat="server" />
                                                    <asp:HiddenField ID="hidStudentId" runat="server" Value = "0" />
                                                    <asp:HiddenField ID="hidPaidDate" runat="server" />
                                                </td>                                                
                                            </tr>
                                        </table>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.StudentCautionMoneyDetailsCollectionBL"
                                            EnablePaging="true" ID="GrdDSobj" runat="server" SelectMethod="GetStudentCautionMoneyDetails"
                                            SortParameterName="sortExpression" SelectCountMethod="CountStudentsForCautionMoney"
                                            EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:ControlParameter ControlID="txtRegNo" PropertyName="Text" Name="asRegNo" />
                                                <asp:ControlParameter ControlID="optCMPaid" PropertyName="Checked" Name="abIncludePaid" />
                                                <asp:ControlParameter ControlID="optCMReturned" PropertyName="Checked" Name="abIncludeReturned" />
                                                <asp:ControlParameter ControlID="txtChequeNumber" PropertyName="Text" Name="aiChequeNumber" />
                                                <asp:ControlParameter ControlID="txtFromDate" PropertyName="Text" Name="adtFromDate" />
                                                <asp:ControlParameter ControlID="txtToDate" PropertyName="Text" Name="adtToDate" />
                                                <asp:ControlParameter ControlID="txtReturnStartDate" PropertyName="Text" Name="adtReturnStartDate" />
                                                <asp:ControlParameter ControlID="txtReturnEndDate" PropertyName="Text" Name="adtReturnEndDate" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="string" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdStudents" />

                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="45%">
                                <asp:Button ID="btnExport" Text="<%$ Resources:LocalizedResources, Export%>" CssClass="ClsBtn"
                                    runat="server" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidValEndDateBlank" runat="server" />
                    <asp:HiddenField ID="hidValStartDateBlank" runat="server" />
                    <asp:HiddenField ID="hidEndDateShouldBeGreaterThanStartDate" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                    <asp:HiddenField ID="HidLabel" runat="server" />
                    <asp:HiddenField ID="hidNoRecordFound" runat="server"  />
                    <asp:HiddenField ID="hidPage" runat="server"  />
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _clientbtnExport = "<%=this.btnExport.ClientID %>"
        _clientGrdId = "<%=this.grdStudents.ClientID %>"
        _clientbtnSave = "<%=this.btnShow.ClientID %>"
        _clientoptChequeNumberId = "<%=this.optChequeNumber.ClientID %>"
        _clientoptRegNoId = "<%=this.optRegNo.ClientID %>"
        _clientoptDateId = "<%=this.optDate.ClientID %>"
        _clienttxtChequeNumberId = "<%=this.txtChequeNumber.ClientID %>"
        _clienttxtRegNoId = "<%=this.txtRegNo.ClientID %>"
        _clienttxtFromDateId = "<%=this.txtFromDate.ClientID %>"
        _clientcFromDateId = "<%=this.cFromDate.ClientID %>"
        _clienttxtToDateId = "<%=this.txtToDate.ClientID %>"
        _clienttcToDateId = "<%=this.cToDate.ClientID %>"
        _clientcstFormId = "<%=this.cstForm.ClientID %>"

        _clientoptReturnDate = "<%=this.optReturnDate.ClientID %>"
        _clienttxtReturnStartDate = "<%=this.txtReturnStartDate.ClientID %>"
        _clienttxtReturnEndDate = "<%=this.txtReturnEndDate.ClientID %>"


        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement != null && postBackElement.id == _clientbtnSave) {
                if (document.getElementById(_clientbtnExport).style.visibility == "hidden") {
                    if (document.getElementById(_clientGrdId) != undefined && document.getElementById(_clientGrdId) != null) {
                        var iCount = document.getElementById(_clientGrdId).rows.length - 1
                        if (iCount > 0)
                            document.getElementById(_clientbtnExport).style.visibility = "inherit"
                        else
                            document.getElementById(_clientbtnExport).style.visibility = "hidden"
                    }
                    else
                        document.getElementById(_clientbtnExport).style.visibility = "hidden"
                }
                else
                    document.getElementById(_clientbtnExport).style.visibility = "hidden"
            }
        }


        function ValidatereteurnDates(oSrc, args) {
            var stDate = document.getElementById(_clienttxtReturnStartDate).value
            var edDate = document.getElementById(_clienttxtReturnEndDate).value
            var fromDate, toDate

            if ($get(_clientoptReturnDate).checked == true && stDate.trim() != "" && edDate.trim() !="") {
                if (document.all) {
                    fromDate = new Date(dstDate.replace('-', ' '))
                    toDate = new Date(edDate.replace('-', ' '))
                }
                else {
                    fromDate = new Date(convertdate(stDate))
                    toDate = new Date(convertdate(edDate))
                }

                if (fromDate > toDate) {
                    args.IsValid = false
                    return true
                }
            }

            args.IsValid = true
            return false
        }


        function ClearErrorLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            if (isPageValid) { }
        }
        function NoAction() {
            return false
        }
        function clickButton(e, buttonid) {
            var evt = e ? e : window.event
            var bt = document.getElementById(buttonid)
            if (bt) {
                if (evt.keyCode == 13) {
                    bt.click()
                    return false
                }
            }
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
        function ValidateControls(oSrc, args) {
            if (document.getElementById(_clientoptDateId).checked && !document.getElementById(_clientoptDateId).disabled) {
                if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtToDateId).value) != "" &&
stripLeadingTrailingBlanks(document.getElementById(_clienttxtFromDateId).value) == "") {
                    oSrc.errormessage = document.getElementById("<%=this.hidValStartDateBlank.ClientID %>").value
                    args.IsValid = false
                    return true
                }
                else if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtFromDateId).value) != "" &&
stripLeadingTrailingBlanks(document.getElementById(_clienttxtToDateId).value) == "") {
                    oSrc.errormessage = document.getElementById("<%=this.hidValEndDateBlank.ClientID %>").value
                    args.IsValid = false
                    return true
                }
                else if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtFromDateId).value) != "" &&
stripLeadingTrailingBlanks(document.getElementById(_clienttxtToDateId).value)) {
                    var fromDate
                    var toDate
                    if (document.all) {
                        fromDate = new Date((document.getElementById(_clienttxtFromDateId).value).replace('-', ' '))
                        toDate = new Date((document.getElementById(_clienttxtToDateId).value).replace('-', ' '))
                    }
                    else {
                        fromDate = new Date(convertdate(document.getElementById(_clienttxtFromDateId).value))
                        toDate = new Date(convertdate(document.getElementById(_clienttxtToDateId).value))
                    }
                    if (fromDate > toDate) {
                        oSrc.errormessage = document.getElementById("<%=this.hidEndDateShouldBeGreaterThanStartDate.ClientID %>").value

                        args.IsValid = false
                        return true
                    }
                }
            }
            args.IsValid = true
            return false
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }
    </script>
</asp:Content>
