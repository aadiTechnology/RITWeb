<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PendingFeeStudentList.aspx.cs" Inherits="PendingFeeStudentList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="98%">
            <tr>
                <td class="ClsMdtStar" align="left">
                    &nbsp;<asp:ValidationSummary ID="valSumErrorMsg" runat="server" ShowMessageBox="False"
                        ShowSummary="True" CssClass="ClsLabel" ValidationGroup="show" />
                    <asp:CustomValidator ID="cstDueDate" runat="server" CssClass="ClsMdtStar" Display="None"
                        EnableClientScript="true" Visible="true" ValidationGroup="show" ClientValidationFunction="ValidateDueDate"
                        ErrorMessage="Error msg"></asp:CustomValidator>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                    <div style="float: right" class="LblErrorMsg" id="lblMandatoryMark" runat="server">
                        <span>*</span>
                        <asp:Label ID="lblMandatoryFields" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnExport" />
                            <asp:AsyncPostBackTrigger ControlID="grdFeesToBePaid" EventName="DataBound" />
                            <asp:AsyncPostBackTrigger ControlID="btnSendSMS" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnResolveConflict" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>      
            <tr align="center">
                <td align="center">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Always">
                        <ContentTemplate>
                            <table runat="server" id="tblInputFields">
                                <tr runat="Server" id="trStandard" align="center">
                                    <td align="center">
                                        <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr align="center">
                                                        <td align="left" class="ClsBorderlight" style="width: 211px">
                                                            <asp:Label ID="lblDueDate" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, DueDate%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" class="ClsTextNormal" style="padding-right: 10px; height: 19px;"
                                                            colspan="1">
                                                            <asp:TextBox ID="txtDueDate" CssClass="MidTxtBox" runat="server" AutoPostBack="True"></asp:TextBox>
                                                            <rjs:PopCalendar ID="cal_DueDate" runat="server" Control="txtDueDate" Format="dd MMM yyyy"
                                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources,DueDateShouldNotBeBlank%>"
                                                                Culture="en" />
                                                            <asp:Label ID="Label5" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>&nbsp;&nbsp;
                                                        </td>
                                                        <td class="ClsBorderlight" style="width: 211px">
                                                            <asp:Label ID="lblSelectStandard" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, SelectStandard%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlStandard" AutoPostBack="true" runat="server" CssClass="SmlTxtBox"
                                                                OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged" AppendDataBoundItems="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="ClsBorderlight" align="left">
                                                            <asp:Label ID="lblStudNameRegNo" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, StudentNameRegNo%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRegNumber" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td class="ClsBorderlight">
                                                            <asp:Label ID="lblSelectDivision" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, SelectDivision%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="SmlTxtBox">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight" align="left">
                                                            <asp:Label ID="lblPendingFee" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, PendingFee%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="cmbOperator" runat="server" Width="80px" CssClass="SmlTxtBox">
                                                                <asp:ListItem Selected="True" Value="0" Text="<%$ Resources:LocalizedResources, Select%>"></asp:ListItem>
                                                                <asp:ListItem Value="3">=</asp:ListItem>
                                                                <asp:ListItem Value="1">&lt;</asp:ListItem>
                                                                <asp:ListItem Value="2">&lt;=</asp:ListItem>
                                                                <asp:ListItem Value="5">&gt;</asp:ListItem>
                                                                <asp:ListItem Value="4">&gt;=</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="SmlTxtBox" MaxLength="7" Width="60px"
                                                                onblur="extractNumber(this,0,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                onkeyup="extractNumber(this,0,false); ValidateAmount(event);" onpaste="event.returnValue=false"
                                                                autocomplete="off"></asp:TextBox>
                                                            <asp:DropDownList ID="ddlFilter" runat="server" Width="50px" onchange="SetDefault();">
                                                                <asp:ListItem Text="Rs." Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="%" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <span id="spnAmountMandetory" runat="server" class="ClsMdtStar" style="color: Red;">
                                                                *</span>
                                                            <%--<asp:Label ID="lblAmountMandetory" runat="server" Visible="false" CssClass="ClsMdtStar"
																ForeColor="Red" Text="*"></asp:Label>--%>
                                                        </td>
                                                        <td class="ClsBorderlight">
                                                            <asp:Label ID="lblFeeType" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, FeeType%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="cmbFeeType" OnSelectedIndexChanged="cmbFeeType_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="SmlTxtBox">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="ClsBorderlight" align="left" style="width: 150px">
                                                            <asp:Label ID="lblIgnorLeftStudent" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, IgnoreLeftStudent%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" style="width: 250px">
                                                            <asp:CheckBox ID="chkleftStu" runat="server" />
                                                        </td>
                                                        <td class="ClsBorderlight">
                                                            <asp:Label ID="Label1" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, PayableFor%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="cmbPayableFor" AutoPostBack="true" runat="server" CssClass="SmlCombo" Width="150px">                                                               
                                                            </asp:DropDownList>
                                                        </td>
                                                           
                                                    </tr>
                                                    <tr>
                                                         <td class="ClsBorderlight" align="left" style="width: 150px">
                                                           <asp:Label ID="lblIgnorPDCStudent" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, IgnorePDCStudent%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                         </td>
                                                         <td align="left">
                                                            <asp:CheckBox ID="chkPDCStud" runat="server" />
                                                         </td>
                                                    </tr>
                                                    <tr id="Tr2">
                                                        <td align="left">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                        </td>
                                                        <td width="100px">
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr3">
                                                        <td align="center" colspan="4">
                                                            <table cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td align="center">
                                                                        &nbsp;<asp:Button ID="btnShow" runat="server" CausesValidation="true" CssClass="ClsBtnMid"
                                                                            Height="26px" OnClick="btnShow_Click" Text="<%$ Resources:LocalizedResources, Show%>"
                                                                            ValidationGroup="show" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnResolveConflict" runat="server" CausesValidation="false" CssClass="ClsBtnMid"
                                                                            Height="26px" OnClick="btnResolveConflict_Click" Text="Resolve Conflict" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CustomValidator ID="reqValAmount" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, AmountPercentageShouldNotBeBlank%>"
                                                                            CssClass="LblNormal" Display="none" EnableClientScript="true" ValidateEmptyText="true"
                                                                            ValidationGroup="show" ControlToValidate="txtAmount" ClientValidationFunction="AmountRequiredValidation"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="4">
                                                            <asp:HiddenField ID="hidServerDate" runat="server" />
                                                            <asp:HiddenField ID="hidIsSMSAccessEnabled" runat="server" Value="" />
                                                            <asp:HiddenField ID="hidStandardId" runat="server" />
                                                            <asp:HiddenField ID="hidIsMsgAccessEnabled" runat="server" Value="" />
                                                            <asp:HiddenField ID="hidYearEndDate" runat="server" />
                                                            <asp:HiddenField ID="hidYearStartDate" runat="server" />
                                                            <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                                            <asp:HiddenField ID="hidDueDateShouldNotBeBlank" runat="server" />
                                                            <asp:HiddenField ID="hidShow" runat="server" />
                                                            <asp:HiddenField ID="hidSMSText" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />                                                
                                                <asp:PostBackTrigger ControlID="btnExport" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlFeesToBePaidGrid" runat="server">
                                            <table id="Table1" width="100%">
                                                <tr runat="server" id="trTotalRec" align="center" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources, To%>"></asp:Label>
                                                        <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf%>"></asp:Label>
                                                        <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources, Records%>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="top">
                                                        <asp:UpdatePanel runat="server" ID="upnlGrid" ChildrenAsTriggers="true">
                                                            <ContentTemplate>
                                                                <asp:GridView CssClass="GridBorder" ID="grdFeesToBePaid" runat="server" AllowPaging="True"
                                                                    AutoGenerateColumns="False" AllowSorting="True" EmptyDataText="<%$ Resources:LocalizedResources, NoRecordFound%>"
                                                                    Width="100%" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                                    GridLines="None" DataKeyNames="Student_Id,User_Id,SMSName,Mobile_Number,TotalAmount,SchoolLeft_Date"
                                                                    OnSorting="grdFeesToBePaid_Sorting" OnPageIndexChanging="grdFeesToBePaid_PageIndexChanging"
                                                                    OnRowCreated="grdFeesToBePaid_RowCreated" OnRowDataBound="grdFeesToBePaid_RowDataBound">
                                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                    </PagerStyle>
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, RegNo%>" DataField="Enrolment_Number"
                                                                            SortExpression="Enrolment_Number">
                                                                            <ItemStyle HorizontalAlign="Left" CssClass="paddingLR" VerticalAlign="Middle" Wrap="False" />
                                                                            <HeaderStyle HorizontalAlign="Left" CssClass="paddingLR" VerticalAlign="Middle" Wrap="False"
                                                                                Width="5%" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Class%>" DataField="Class"
                                                                            SortExpression="Std_Div_ID">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLR" Wrap="False" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLR" Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, RollNo%>" DataField="Roll_No"
                                                                            SortExpression="Roll_No">
                                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" CssClass="paddingLR" Wrap="False"
                                                                                Width="5%" />
                                                                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" CssClass="paddingLR" Width="5%"
                                                                                Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, StudentName%>" DataField="StudentName"
                                                                            SortExpression="First_Name">
                                                                            <ItemStyle HorizontalAlign="Left" CssClass="paddingLR" VerticalAlign="Middle" Wrap="False" />
                                                                            <HeaderStyle HorizontalAlign="Left" CssClass="paddingLR" VerticalAlign="Middle" Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, PendingAmount%>" DataField="Amount"
                                                                            FooterText="1" SortExpression="Amount">
                                                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
                                                                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR"
                                                                                Width="5%" />
                                                                            <FooterStyle HorizontalAlign="left" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, LateFee%>" DataField="Late_Fee_Amt"
                                                                            FooterText="2" NullDisplayText="-" SortExpression="Late_Fee_Amt">
                                                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR"
                                                                                Width="5%" />
                                                                            <FooterStyle HorizontalAlign="left" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, PayableFor%>" DataField="PaybleFor"
                                                                            FooterText="1">
                                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Wrap="True" CssClass="paddingLR" />
                                                                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
                                                                            <FooterStyle HorizontalAlign="left" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                    <PagerTemplate>
                                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage%>"
                                                                                        runat="server" CssClass="LblNrmlB" />
                                                                                    <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                                        OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </PagerTemplate>
                                                                </asp:GridView>
                                                                <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdDSobj"
                                                                    runat="server" SelectMethod="GetPendingFeeStudentList" SortParameterName="sortExpression"
                                                                    SelectCountMethod="GetCountPendingFeeStudentList" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                            Type="int32" />
                                                                        <asp:ControlParameter Name="aiStandardId" Type="int32" ControlID="ddlStandard" PropertyName="SelectedValue" />
                                                                        <asp:ControlParameter Name="aiDivisionId" Type="int32" ControlID="ddlDivision" PropertyName="SelectedValue" />
                                                                        <asp:ControlParameter Name="asRegNo" ControlID="txtRegNumber" PropertyName="Text"
                                                                            DefaultValue="" />
                                                                        <asp:ControlParameter Name="odtStartDate" ControlID="hidServerDate" PropertyName="Value" />
                                                                        <asp:ControlParameter Name="abLeftStudent" ControlID="chkleftStu" PropertyName="Checked" />
                                                                        <asp:ControlParameter Name="abPDCStudent" ControlID="chkPDCStud" PropertyName="Checked" />
                                                                        <asp:ControlParameter Name="aiFeeTypeId" Type="int32" ControlID="cmbFeeType" PropertyName="SelectedValue"
                                                                            DefaultValue="0" />
                                                                        <asp:ControlParameter Name="asPayableFor" Type="String" ControlID="cmbPayableFor" PropertyName="SelectedValue" />
                                                                        <asp:ControlParameter Name="asOperator" Type="string" ControlID="cmbOperator" PropertyName="SelectedItem.Text" />
                                                                        <asp:ControlParameter Name="aiAmount" ControlID="txtAmount" PropertyName="Text" DefaultValue="0" />
                                                                        <asp:ControlParameter Name="asPercentFilter" ControlID="ddlFilter" PropertyName="SelectedValue" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                                <asp:PostBackTrigger ControlID="btnExport" />
                                                                <asp:AsyncPostBackTrigger ControlID="grdFeesToBePaid" EventName="DataBound" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trTotalAmount" runat="server" class="ClsBorderlight">
                                    <td>
                                        <table align="center" id="tblTotalAmount" runat="server" visible="false">
                                            <tr>
                                                <td style="background-color: #e4efc4;" align="left">
                                                    <asp:Label ID="lblTotalPendingAmount" runat="server" class="LblNrmlB" Style="width: 205px"
                                                        Text="<%$ Resources:LocalizedResources, TotalPendingAmount%>"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" style="background-color: #eaeaea">
                                                    <asp:Label ID="lblTotalAmount" Width=" 90px" runat="server" CssClass="ClsHilightFeeL" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <asp:Button ID="btnSendSMS" runat="server" Text="Send SMS With Amount" CssClass="ClsBtnLrg"
                                            Height="24px" CausesValidation="true" OnClick="btnSendSMS_Click" />
                                        <asp:Button ID="btnSMS" runat="server" Text="<%$ Resources:LocalizedResources, sndSMS%>"
                                            CssClass="ClsBtnMid" Height="24px" CausesValidation="true" OnClick="btnSMS_Click" />
                                        <asp:Button ID="btnMessage" runat="server" Text="<%$ Resources:LocalizedResources, SendMessage%>"
                                            CssClass="ClsBtnMid" Height="24px" CausesValidation="true" OnClick="btnMessage_Click" />
                                        <asp:Button ID="btnExport" Visible="false" Text="<%$ Resources:LocalizedResources, Export%>"
                                            CssClass="ClsBtn" runat="server" Height="24px" OnClick="btnExport_Click" />
                                        <%} %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table align="center">
                                            <tr>
                                                <td width="2%">
                                                </td>
                                                <td width="98%" align="center">
                                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>"
                                                        CssClass="ClsBtnMid" Height="24px" UseSubmitBehavior="false" CausesValidation="false"
                                                       PostBackUrl="~/RITeSchool/Accountant/StudentPayFeeUI.aspx" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnExport" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _sClientGridId = "<%=this.grdFeesToBePaid.ClientID %>"
        _clienttxtDueDate = "<%=this.txtDueDate.ClientID%>"
        _clientcstDueDate = "<%=this.cstDueDate.ClientID%>"
        _clientbtnBack = "<%=this.btnBack.ClientID %>"
        _clientbtnSMS = "<%=this.btnSMS.ClientID %>"
        _clientbtnMessage = "<%=this.btnMessage.ClientID %>"
        _clientbtnShow = "<%=this.btnShow.ClientID %>"
        _clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>"
        _clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>"
        _clientddlFilter = "<%=this.ddlFilter.ClientID %>"
        _clienttxtAmount = "<%=this.txtAmount.ClientID %>"
        _clientcmbOperator = "<%=this.cmbOperator.ClientID %>"
        _clientreqValAmount = "<%=this.reqValAmount.ClientID %>"
        _clientspnAmountMandetory = "<%=this.spnAmountMandetory.ClientID %>"
        _clientddlStandard = "<%=this.ddlStandard.ClientID %>"
        _clienthidSMSText = "<%=this.hidSMSText.ClientID %>"

        $(document).ready(function () {
            $('#' + _clientddlFilter).live('change', function () {
                $('#' + _clienttxtAmount).attr('maxlength', this.value == 2 ? 3 : 7);
            })

            $('#' + _clientcmbOperator).live('change', function () {
                if ($('#' + _clientcmbOperator).val() == 0) {
                    $("#" + _clientspnAmountMandetory).hide();
                    $('#' + _clienttxtAmount).val('');
                    $('#' + _clienttxtAmount).attr('disabled', true);
                    $('#' + _clientreqValAmount).attr('disabled', true);

                }
                else {
                    $('#' + _clienttxtAmount).attr('disabled', false);
                    $('#' + _clientreqValAmount).attr('disabled', false);

                    $("#" + _clientspnAmountMandetory).show();
                }

            })
        });

        function ValidateAmount(e) {
            var evt = e || window.event;
            var abc = document.getElementById(_clienttxtAmount).value;
            if (parseInt(abc) == 0) {
                document.getElementById(_clienttxtAmount).value = "";
                return false;
            }

            var selectedIndex = document.getElementById(_clientddlFilter).value;
            if (selectedIndex == 2) {
                if (abc > 100) {
                    document.getElementById(_clienttxtAmount).value = abc.substring(0, abc.length - 1);
                    return false
                }
            }
        }

        function SetDefault() {
            document.getElementById(_clienttxtAmount).value = "";
        }

        function ValidateDueDate(aSrc, args) {
            var dtEndDate, dtStartDate
            var strStartDate = document.getElementById(_clienttxtDueDate).value
            if (document.getElementById(_clienttxtDueDate).value == "") {
                document.getElementById(_clientcstDueDate).errormessage = document.getElementById("<%=hidDueDateShouldNotBeBlank.ClientID%>").value;
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function AmountRequiredValidation(aSrc, args) {
            if ($('#' + _clientcmbOperator).val() != 0) {
                if ($('#' + _clienttxtAmount).val() == '') {
                    args.IsValid = false
                    return true;
                }
            }
        }


        function CheckIfDateInAcademicYear(dtObj) {
            var bReturn
            var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value)
            var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value)
            if ((dtObj < dtYearStartDate) || (dtObj > dtYearEndDate)) {
                bReturn = false
            }
            else {
                bReturn = true
            }
            return bReturn
        }

        function ConfirmSMS() {
            var SMS = document.getElementById(_clienthidSMSText).value
            if (confirm("SMS text will be as follows - \n\n" + SMS + " \n\n Do you want to continue?"))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>
