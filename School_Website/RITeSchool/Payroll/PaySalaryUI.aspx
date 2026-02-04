<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PaySalaryUI.aspx.cs" Inherits="PaySalaryUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="97%" align="center">
                                <tr>
                                    <td>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ValidationGroup="publishSalary" />                                                                
                                                                <asp:ValidationSummary ID="valSumShow" runat="server" CssClass="LblErrorMsg" ValidationGroup="DisplaySalaryDetails" />
                                                            </td>
                                                            <td align="right" valign="top">
                                                                <span class="ClsMdtStar">* Mandatory Fields</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trReport" runat="server">
                                                            <td>
                                                            </td>
                                                            <td align="left" style="height: 25px" class="ClsGreenBG" width="140PX">
                                                                <asp:LinkButton ID="lnkUserReports" runat="server" Text="Report Assignment" CssClass="SubTitle"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 20px;" id="trLeaveConfigMessage" runat="server" visible="false">
                                                            <td align="center">
                                                                <blink>
                                                                    <span id="lblConfigMessage" style="font-weight:bold;text-align:center;" class="LblErrorMsg">Please configure Basic Leaves for next interval if not already done.</span>
                                                                </blink>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="height: 0">
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" ForeColor="Red" EnableViewState="False"></asp:Label>
                                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Blue" Text=""
                                                                    CssClass="ClsLabel" EnableViewState="False" Width="120%"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div runat="server" id="divErr" style="width: 100%" align="center">
                                                        <table align="center" class="LblNoRecord" cellspacing="0" cellpadding="0" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div runat="server" id="div1">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trConfigMessage" runat="server" visible="false">
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight" style="background-color: #E6EEFC">
                                                                <span class="ClsLabel" style="color: Blue; font-weight: bold; font-size: medium">Required
                                                                    salary details are not yet configured.</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trMonthAndYear" runat="server">
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblYearAndMonth" runat="server" CssClass="ClsLabel" Font-Size="Medium"
                                                                    EnableViewState="true" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trChequeNo" runat="server" align="center" visible="false">
                                                <td>
                                                    <table class="ClsBorderLight">
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <table>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight">
                                                                            <asp:Label ID="Label6" runat="server" Text="Payment Type : " CssClass="ClsLabel"></asp:Label>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:RadioButton ID="optCheque" runat="server" CssClass="ClsLabel" 
                                                                                Text="Cheque" GroupName="PaymentType" AutoPostBack="True" 
                                                                                oncheckedchanged="optCheque_CheckedChanged" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:RadioButton ID="optOnline" runat="server" CssClass="ClsLabel" 
                                                                                Text="Online" GroupName="PaymentType" AutoPostBack="True" 
                                                                                oncheckedchanged="optOnline_CheckedChanged" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <table width="100%">
                                                                    <tr id="trCombobox" runat="server">
                                                                        <td class="ClsBorderLight" width="140px">
                                                                            <span class="ClsLabel">Year :</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbYear" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                                                                OnSelectedIndexChanged="cmbMonths_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <span class="ClsMdtStar">*</span>
                                                                        </td>
                                                                        <td class="ClsBorderLight" width="140px">
                                                                            <span class="ClsLabel">Month :</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbMonths" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="cmbMonths_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <span class="ClsMdtStar">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="ClsBorderLight" align="center" width="140px">
                                                                            <span class="ClsLabel">Bank Name :</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbBankName" runat="server" CssClass="LrgCombo"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="cmbBankName_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <span class="ClsMdtStar">*</span>
                                                                        </td>
                                                                        <td class="ClsBorderLight" width="140px">
                                                                            <span class="ClsLabel">Account No. :</span>
                                                                        </td>
                                                                        <td width="230px">
                                                                            <asp:UpdatePanel ID="upnlAccoountNo" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="cmbAccountNo" runat="server" MaxLength="100" CssClass="LrgCombo">
                                                                                    </asp:DropDownList>
                                                                                    <span class="ClsMdtStar">*</span>&nbsp;
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="cmbBankName" EventName="SelectedIndexChanged" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <asp:UpdatePanel ID="unplPaymentType" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                         <table width="100%">
                                                                             <tr>
                                                                                <td class="ClsBorderLight" align="center" width="140px">                                                                                    
                                                                                    <asp:Label ID="lblChequeNo" runat="server" CssClass="ClsLabel" Text="Cheque No.:"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtChequeNo" CssClass="LrgTxtBox" runat="server" MaxLength="25" Style="text-align: right;
                                                                                        padding-right: 2px;" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                                        ondrop="event.returnValue=false" />                                                                                   
                                                                                    <span id="spnStar" runat="server" class="ClsMdtStar">*</span>
                                                                                </td>
                                                                                <td class="ClsBorderLight" width="140px">                                                                                    
                                                                                    <asp:Label ID="lblChequeDate" runat="server" CssClass="ClsLabel" Text="Cheque Date :"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtChequeDate" runat="server" CssClass="LrgTxtBox" MaxLength="11" style="width:195px;"></asp:TextBox>
                                                                                    <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtChequeDate" Format="dd MMM yyyy"
                                                                                        ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid cheque date."
                                                                                        ControlFocusOnError="True" />
                                                                                    <span class="ClsMdtStar">*</span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="ClsBorderLight" align="center">                                                                                    
                                                                                    <asp:Label ID="lblChequeAmount" runat="server" CssClass="ClsLabel" Text="Cheque Amount :"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAmount" CssClass="LrgTxtBox" runat="server" MaxLength="10" Style="text-align: right;
                                                                                        padding-right: 2px;" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                                        ondrop="event.returnValue=false" />
                                                                                    <span class="ClsMdtStar">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkSendSMS" runat="server" CssClass="ClsLabel" Text="Send SMS" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkSendDetailSMS" runat="server" CssClass="ClsLabel" Text="Send detail SMS" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                         <asp:AsyncPostBackTrigger ControlID="optCheque"  EventName="CheckedChanged" />
                                                                         <asp:AsyncPostBackTrigger ControlID="optOnline"  EventName="CheckedChanged" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>                                                               
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trButtons" runat="server" visible="false">
                                                <td align="center">
                                                    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" CausesValidation="false"
                                                        UseSubmitBehavior="false" OnClick="btnExport_Click" />
                                                    <asp:Button ID="btnSalarySlipPreview" runat="server" Text="Preview Salary Slips"
                                                        CssClass="ClsBtn" CausesValidation="false" Style="width: 150px;" UseSubmitBehavior="false"
                                                        OnClick="btnSalarySlipPreview_Click" />
                                                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="ClsBtn" CausesValidation="false"
                                                        UseSubmitBehavior="false" OnClick="btnRefresh_Click" />                                                   
                                                    <asp:CustomValidator ID="cstvalYear" runat="server" ClientValidationFunction="ValidateYear"
                                                        ValidationGroup="DisplaySalaryDetails" Display="None" ErrorMessage="Year should be selected."></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cstvalMonth" runat="server" ClientValidationFunction="ValidateMonth"
                                                        ValidationGroup="DisplaySalaryDetails" Display="None" ErrorMessage="Month should be selected."></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="cmbBankName"
                                                        Display="None" ValidationGroup="publishSalary" InitialValue="0" ErrorMessage="Bank Name should be selected."></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="reqAccountNo" runat="server" ControlToValidate="cmbAccountNo"
                                                        Display="None" ValidationGroup="publishSalary" InitialValue="0" ErrorMessage="Account no should be selected."></asp:RequiredFieldValidator>                                                     
                                                    <asp:CustomValidator ID="cstValChequeNo" runat="server" ClientValidationFunction="ValidateChequeNo"
                                                        ValidationGroup="publishSalary" Display="None"></asp:CustomValidator>                                                    
                                                    <asp:CustomValidator ID="cstValChequeDate" runat="server" ClientValidationFunction="ValidateChequeDate"
                                                        ValidationGroup="publishSalary" Display="None"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cstChequeAmount" runat="server" ClientValidationFunction="ValidateChequeAmount"
                                                        ValidationGroup="publishSalary" Display="None"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr id="trWizard" runat="server">
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Wizard ID="wizSalaryInformation" Width="100%" runat="server" DisplaySideBar="False"
                                                                    ActiveStepIndex="0" OnFinishButtonClick="wizSalaryInformation_PublishPuttonClick"
                                                                    DisplayCancelButton="True" OnCancelButtonClick="wizSalaryInformation_CancelButtonClick"
                                                                    OnNextButtonClick="wizSalaryInformation_NextButtonClick" OnActiveStepChanged="wizSalaryInformation_ActiveStepChanged"
                                                                    Style="margin-right: 0px">
                                                                    <WizardSteps>
                                                                        <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1" StepType="Start">
                                                                            <table width="100%" align="center">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblHEader" runat="server" CssClass="ClsLabel" Text="Please check whether following details are configured."
                                                                                            Font-Bold="true"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="ClsBorderlight">
                                                                                        <asp:CheckBoxList ID="chklstItems" runat="server" CssClass="ClsLabel">
                                                                                            <asp:ListItem Text="Staff Groups - Define staff groups." Value="1"></asp:ListItem>
                                                                                            <asp:ListItem Text="Earnings and Deductions - Define earnings and deductions." Value="2"></asp:ListItem>
                                                                                            <asp:ListItem Text="Staff Leaves - Define leaves." Value="3"></asp:ListItem>
                                                                                            <asp:ListItem Text="Staff Groups - Earnings Deductions Association - Set earnings and deductions to every staff group."
                                                                                                Value="4"></asp:ListItem>
                                                                                            <asp:ListItem Text="User - Staff Groups Association - Assign staff group to every user."
                                                                                                Value="5"></asp:ListItem>
                                                                                            <asp:ListItem Text="User - Earnings and Deductions - Set values of earnings and deductions to respictive user."
                                                                                                Value="6"></asp:ListItem>
                                                                                            <asp:ListItem Text="Staff Attendance and Leaves - Set attendance and leaves of current month."
                                                                                                Value="7"></asp:ListItem>
                                                                                        </asp:CheckBoxList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:WizardStep>
                                                                        <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                                                                            <table align="center">
                                                                                <tr id="trLegend" runat="server">
                                                                                    <td align="left">
                                                                                        <table id="tblLegent" runat="server">
                                                                                            <tr id="trInvalidLeaveMessage" runat="server">
                                                                                                <td align="left">
                                                                                                    <asp:Label ID="lblInvalidLeaveMEssage" runat="server" CssClass="ClsLabelNrml" EnableViewState="true"
                                                                                                        Style="padding-left: 0px;" ForeColor="Red" Text=""></asp:Label>
                                                                                                    <asp:Label ID="hlnkPTChallan" runat="server" CssClass="ClsLabelNrml" Text="Please configure Professional Tax Challan details for selected year and month."
                                                                                                        Visible="false" ForeColor="Red"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr id="trSalaryDifferenceMessage" runat="server">
                                                                                                <td>
                                                                                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLblLgnd" EnableViewState="true"
                                                                                                        Text=""></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblSalaryDifferenceMessage" runat="server" CssClass="ClsLblLgnd" Style="padding-left: 3px;"
                                                                                            Visible="False"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span class="ClsLblLgnd" style="padding-left: 0px;">Legend : </span>
                                                                                                </td>
                                                                                                <td align="center" valign="middle" style="border: 1px solid #000000; width: 100px;">
                                                                                                    <span class="ClsLabel" style="color: Red; float: inherit;">Deleted User</span>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td id="td1" runat="server" align="center">
                                                                                        <table id="tblPageDetails" runat="server" width="220px" style="vertical-align: top;"
                                                                                            align="center">
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="lblStartIndex" Text="1" runat="server" CssClass="LblNrmlB" />
                                                                                                </td>
                                                                                                <td align="center">
                                                                                                    <span class="LblNormal">to</span>
                                                                                                </td>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                                                </td>
                                                                                                <td align="center">
                                                                                                    <span class="LblNormal">out of</span>
                                                                                                </td>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="lblTotalRecords" runat="server" CssClass="LblNrmlB" />
                                                                                                </td>
                                                                                                <td align="center">
                                                                                                    <span class="LblNormal">records</span>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <div id="divContainer" runat="server" align="center" class="GridBorder" style="width: 900px;
                                                                                            height: 492px; overflow: scroll" visible="False">
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <asp:UpdatePanel ID="upnl4" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:GridView ID="grdPaySalary" runat="server" CellPadding="0" CellSpacing="1" CssClass="GridBorder"
                                                                                                                    DataKeyNames="UserId,DisplayControls,StaffGroupId" ForeColor="#333333" GridLines="None"
                                                                                                                    OnRowDataBound="grdPaySalary_RowDataBound" Width="100%">
                                                                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                                                                        NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                                                                                                    <PagerStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                                                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                                                                </asp:GridView>
                                                                                                            </ContentTemplate>
                                                                                                            <Triggers>
                                                                                                                <asp:AsyncPostBackTrigger ControlID="PageDropDownList" EventName="SelectedIndexChanged" />
                                                                                                                <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                                                                                                            </Triggers>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr id="tblPager" runat="server">
                                                                                                    <td align="center" valign="top">
                                                                                                        <table width="100%" cellpadding="0" cellspacing="0" style="vertical-align: top;">
                                                                                                            <tr>
                                                                                                                <td align="left" class="ClsBorderPager" valign="middle">
                                                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                    <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                                                                        OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td width="100px" align="right" class="ClsBorderPager" valign="middle">
                                                                                                                    <asp:Label ID="lblCurrentPage" runat="server" CssClass="LblNormal" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <asp:Label ID="lblNoRecordMessage" runat="server" Text="No record found." CssClass="LblNoRecord"
                                                                                            Visible="False"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <table id="tblNote" runat="server" width="1100px">
                                                                                            <tr>
                                                                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                                                                    <asp:Label ID="Label16" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                                                                        CssClass="LblNrmlB" Height="16px"></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                                                                    <asp:Label ID="Label17" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Deleted users should be deactivated from the payroll to block them in the salary payment."></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                                                                    <asp:Label ID="Label4" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note2 :"
                                                                                                        CssClass="LblNrmlB" Height="16px"></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                                                                    <asp:Label ID="Label5" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="After opening this page if you have made any changes in leaves or earnings deductions then to reflect same on this page, click on Refresh button."></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                                                                    <asp:Label ID="Label3" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note3 :"
                                                                                                        CssClass="LblNrmlB" Height="16px"></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                                                                    <asp:Label ID="Label2" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If this is last month of payroll year and permanent date is not set for permanent users then on publish of salary, basic leave of those users will be reset to zero. So please make sure permanent date is set for permanent users."></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:WizardStep>
                                                                    </WizardSteps>
                                                                    <FinishNavigationTemplate>
                                                                        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                                                            CssClass="ClsBtnMid" Text="Previous" />
                                                                        <asp:Button ID="PublishPutton" runat="server" CommandName="MoveComplete" CssClass="ClsBtnMid"
                                                                            ValidationGroup="publishSalary" Text="Publish" />
                                                                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                            CssClass="ClsBtnMid" Text="Cancel" />
                                                                    </FinishNavigationTemplate>
                                                                    <StartNavigationTemplate>
                                                                        <asp:Button ID="FinishNextButton" runat="server" CommandName="MoveNext" CausesValidation="False"
                                                                            CssClass="ClsBtnMid" Text="Next" />&nbsp;
                                                                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                            CssClass="ClsBtnMid" Text="Cancel" />
                                                                    </StartNavigationTemplate>
                                                                </asp:Wizard>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="width: 697px">
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hidMonthId" runat="server" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidYear" runat="server" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidColumnIndexes" runat="server" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidIsCurrentMonth" runat="server" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidNetSalarySum" runat="server" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSendSMS" runat="server" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidMonthAndYear" runat="server" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidLeaveTransferMonth" runat="server" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidPTDetails" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hidIsMidYear" runat="server" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidMonthList" runat="server" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidSalaryDifferenceColumnIndex" runat="server" Value="-1"></asp:HiddenField>
                                        <asp:HiddenField ID="hidHolidayLeaves" runat="server" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidInvalidLeaves" runat="server" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidMonthName" runat="server" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSelectedPageIndex" runat="server" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidIsLeaveAccumulationInterval" runat="server" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidQueryString" runat="server"></asp:HiddenField>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExport" />
                            <asp:PostBackTrigger ControlID="btnSalarySlipPreview" />                            
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientChkLstchkList = "<%=this.chklstItems.ClientID %>"
        _clienttxtChequeNo = "<%=this.txtChequeNo.ClientID %>"
        _clienthidIsCurrentMonth = "<%=this.hidIsCurrentMonth.ClientID %>"
        _clientchkSendSMS = "<%=this.chkSendSMS.ClientID %>"
        _clienttxtAmount = "<%=this.txtAmount.ClientID %>"
        _clientcstChequeAmount = "<%=this.cstChequeAmount.ClientID %>"
        _clienthidNetSalarySum = "<%=this.hidNetSalarySum.ClientID %>"
        _clientcmbMonths = "<%=this.cmbMonths.ClientID %>"
        _clientcmbYear = "<%=this.cmbYear.ClientID %>"
        _clientbtnSalarySlipPreview = "<%=this.btnSalarySlipPreview.ClientID %>"
        _clientbtnExport = "<%=this.btnExport.ClientID %>"
        _clienthidLeaveTransferMonth = "<%=this.hidLeaveTransferMonth.ClientID %>"
        _clienthidMonthId = "<%=this.hidMonthId.ClientID %>"
        _clientwizSalaryInformation = "<%=this.wizSalaryInformation.ClientID %>"
        _clientbtnRefresh = "<%=this.btnRefresh.ClientID %>"
        _clienthidIsLeaveAccumulationInterval = "<%=this.hidIsLeaveAccumulationInterval.ClientID %>"

        _clientchkSendDetailSMS = "<%=this.chkSendDetailSMS.ClientID %>";
        _clientoptCheque = "<%=this.optCheque.ClientID %>"
        _clientoptOnline = "<%=this.optOnline.ClientID %>"


        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
            DisableControls(false, false)
        }
        function beginRequestHandler(sender, args) {
            DisableControls(true, false)
        }

        function DisableControls(abAction, IsShowButton) {
            if (document.getElementById(_clientwizSalaryInformation + '_StartNavigationTemplateContainerID_FinishNextButton') != null)
                document.getElementById(_clientwizSalaryInformation + '_StartNavigationTemplateContainerID_FinishNextButton').disabled = abAction
            if (document.getElementById(_clientwizSalaryInformation + '_StartNavigationTemplateContainerID_CancelButton') != null)
                document.getElementById(_clientwizSalaryInformation + '_StartNavigationTemplateContainerID_CancelButton').disabled = abAction
            if (document.getElementById(_clientwizSalaryInformation + '_FinishNavigationTemplateContainerID_CancelButton') != null)
                document.getElementById(_clientwizSalaryInformation + '_FinishNavigationTemplateContainerID_CancelButton').disabled = abAction
            if (document.getElementById(_clientwizSalaryInformation + '_FinishNavigationTemplateContainerID_PublishPutton') != null)
                document.getElementById(_clientwizSalaryInformation + '_FinishNavigationTemplateContainerID_PublishPutton').disabled = abAction
            if (document.getElementById(_clientwizSalaryInformation + '_FinishNavigationTemplateContainerID_FinishPreviousButton') != null)
                document.getElementById(_clientwizSalaryInformation + '_FinishNavigationTemplateContainerID_FinishPreviousButton').disabled = abAction
            if (IsShowButton != true && abAction != false) {
                if (document.getElementById(_clientbtnSalarySlipPreview) != null)
                    document.getElementById(_clientbtnSalarySlipPreview).disabled = abAction
                if (document.getElementById(_clientbtnExport) != null)
                    document.getElementById(_clientbtnExport).disabled = abAction
                if (document.getElementById(_clientcmbYear) != null)
                    document.getElementById(_clientcmbYear).disabled = abAction
                if (document.getElementById(_clientcmbMonths) != null)
                    document.getElementById(_clientcmbMonths).disabled = abAction
                if (document.getElementById(_clientbtnRefresh) != null)
                    document.getElementById(_clientbtnRefresh).disabled = abAction
            }
        }

        function DisplayConfirmation(str) {
            var bResult = false
            var returnValue = true
            var SendSMS = false
            if (typeof (Page_ClientValidate) == 'function')
                bResult = Page_ClientValidate()
            if (bResult) {
                var LeaveTransferMonth = document.getElementById(_clienthidLeaveTransferMonth).value
                var leaveIntervalMonth = document.getElementById(_clienthidIsLeaveAccumulationInterval).value
                var MonthId = document.getElementById(_clienthidMonthId).value
                var sMessage = "Are you sure you want to publish the salary of this month?"

                if (leaveIntervalMonth == 'Y')
                    sMessage = "This action will publish the salary of current month as well as reconfigure yearwise leave of the next interval if exists. Are you sure, you want to continue?"

                if (LeaveTransferMonth == MonthId)
                    sMessage = "This action will publish the salary of current month as well as reconfigure yearwise leave of the next year if exists. Are you sure, you want to continue?"
                if (!confirm(sMessage))
                    returnValue = false
                else {
                    if ($get(_clientchkSendSMS).checked == false && $get(_clientchkSendDetailSMS).checked == false) {
                        if (confirm('Do you want to send SMS to the users?')) {
                            if (confirm('Do you want to send detail SMS?'))
                                $get(_clientchkSendDetailSMS).checked = true
                            else
                                $get(_clientchkSendSMS).checked = true
                        }
                    }
                }
                return returnValue
            }
            return false
        }

        function NextEnable(icount, sArgs) {
            var j = 0
            for (i = 0; i < icount; i++) {
                ChBoxL_Id = _clientChkLstchkList + '_' + i
                chk = document.getElementById(ChBoxL_Id)
                if (chk != null) {
                    if (chk.checked == true) {
                        j++
                    }
                }
            }
            if (j == icount) {
                if (document.getElementById(sArgs) != null)
                    document.getElementById(sArgs).disabled = false
            }
            else {
                if (document.getElementById(sArgs) != null)
                    document.getElementById(sArgs).disabled = true
            }
        }
        NextEnable(7, _clientwizSalaryInformation + '_StartNavigationTemplateContainerID_FinishNextButton')
        function ValidateChequeAmount(oSrc, args) {
            var isValid = true
            var isChequePayment = document.getElementById(_clientoptCheque).checked
            var chequeAmount = document.getElementById(_clienttxtAmount).value
            var NetSalarySum = document.getElementById(_clienthidNetSalarySum).value
            if (chequeAmount.trim() == "") {
                if (isChequePayment)
                    $get(_clientcstChequeAmount).errormessage = "Cheque Amount should not be blank."
                else
                    $get(_clientcstChequeAmount).errormessage = "Transaction Amount should not be blank."
                isValid = false
            }
            else if (parseFloat(chequeAmount.trim()) < parseFloat(NetSalarySum)) {
                if (isChequePayment)
                    $get(_clientcstChequeAmount).errormessage = "Cheque Amount should be greater than or equal to total of Net Salary."
                else
                    $get(_clientcstChequeAmount).errormessage = "Transaction Amount should be greater than or equal to total of Net Salary."

                isValid = false
            }
            if (isValid == true) {
                args.IsValid = true
                return false
            }
            else {
                args.IsValid = false
                return true
            }
        }
        function ValidateMonth(oSrc, args) {
            if ($get(_clientcmbMonths) != null) {
                var month = $get(_clientcmbMonths).value
                if (month == 0) {
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }
        function ValidateYear(oSrc, args) {
            if ($get(_clientcmbMonths) != null) {
                var year = $get(_clientcmbYear).value
                if (year == 0) {
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function OpenPopup() {
            var querystring = $get("<%=this.hidQueryString.ClientID %>").value;
            window.open('UserReportAssignmentPopup.aspx?' + querystring, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=650')
            return false;
        }

        function ValidateChequeNo(oSrc, args) {
            var isChequePayment = document.getElementById(_clientoptCheque).checked
            if (isChequePayment) {
                if (document.getElementById("<%=this.txtChequeNo.ClientID %>").value.trim() == "") {
                    oSrc.errormessage = "Cheque No. should not be blank."
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateChequeDate(oSrc, args) {
            var isChequePayment = document.getElementById(_clientoptCheque).checked

            if (document.getElementById("<%=this.txtChequeDate.ClientID %>").value.trim() == "") {
                if (isChequePayment)
                    oSrc.errormessage = "Cheque Date should not be blank."
                else
                    oSrc.errormessage = "Transaction Date should not be blank."
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
