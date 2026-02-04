<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserAppointmentDetailsUI.aspx.cs" Inherits="UserAppointmentDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="left" width="50%">
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                        <asp:RequiredFieldValidator ID="reqValName" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, ValBlankName%>"
                                            Display="None" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator8" runat="server" ClientValidationFunction="DuplicateNameValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValDuplicateName%>"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateAddress"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, valDesignation%>"
                                            ControlToValidate="cmbDesignation" Display="None" Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateJoiningDate"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="ValidatePaymentStartDate"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, valJobTypeSelection%>"
                                            ControlToValidate="cmbJobType" Display="None" Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ClientValidationFunction="ValidateAgreementDate"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValParameter" runat="server" ClientValidationFunction="ValidateParameterValue"
                                            SetFocusOnError="True" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, valAmountForEarnDeduct%>"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator5" runat="server" ClientValidationFunction="CompairePaymentStartDate"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator6" runat="server" ClientValidationFunction="CompaireJoiningDate"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator7" runat="server" ClientValidationFunction="CompaireJoiningDate1"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                    </td>
                                    <td width="50%" valign="top">
                                        <div style="float: right;">
                                            <span class="ClsMdtStar">* Mandatory Fields </span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAppointentDetails" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <table>
                        <tr>
                            <td align="right" style="height: 25px" class="ClsGreenBG">
                                <asp:HyperLink ID="lnkPaymentGroups" runat="server" Text="Payment Groups" CssClass="SubTitle"
                                    NavigateUrl="~/RITeSchool/Payroll/PaymentGroupUI.aspx"></asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="center" id="tdMessage" runat="server" colspan="2">
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150px" class="ClsBorderlight">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Name%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="SmlCombo">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="LrgTxtBox" MaxLength="150"></asp:TextBox>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Address%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="LrgTxtBox" TextMode="MultiLine"></asp:TextBox>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Designation%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbDesignation" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="lblEmpNo" runat="server" Text="Letter No"
                                         CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td class="TxtNormal">
                                        <asp:TextBox ID="txtEmpNoPrefix" CssClass="MidTxtBox" runat="server" MaxLength="20"
                                             Width="100px" ReadOnly="true">
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtEmpNo" runat="server" CssClass="SmlTxtBox" MaxLength="10"
                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, JoiningDate%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtJoiningDate" CssClass="MidTxtBox" runat="server" />
                                        <rjs:PopCalendar ID="cal_JoiningDate" runat="server" Control="txtJoiningDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" AutoPostBack="False" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" width="200px">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, SalaryStartDate%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPaymentStartDate" CssClass="MidTxtBox" runat="server" />
                                        <rjs:PopCalendar ID="cal_PaymentStartDate" runat="server" Control="txtPaymentStartDate"
                                            Format="dd MMM yyyy" Culture="en" ShowWeekend="True" ShowErrorMessage="false"
                                            AutoPostBack="False" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, JobType%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbJobType" runat="server" CssClass="MidCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, AgreementDate%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgreementDate" CssClass="MidTxtBox" runat="server" />
                                        <rjs:PopCalendar ID="cal_AgreementDate" runat="server" Control="txtAgreementDate"
                                            Format="dd MMM yyyy" Culture="en" ShowWeekend="True" ShowErrorMessage="false"
                                            AutoPostBack="False" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, PaymentGroup%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbPaymentGroup" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                            OnSelectedIndexChanged="cmbPaymentGroup_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, EarningDeduction%>"
                                            CssClass="ClsLabel"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ListView ID="lstvwParameters" runat="server" DataKeyNames="EarningDeductionId,IsEarning"
                                                                OnItemDataBound="lstvwParameters_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                        <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                            <th align="left" style="padding-left: 5px">
                                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, EarningDeductionName%>"
                                                                                 CssClass="ClsLabelR"></asp:Label>
                                                                            </th>
                                                                            <th align="right" style="padding-right: 5px;" width="150px">
                                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:LocalizedResources, Amount%>"
                                                                                    CssClass="ClsLabelR"></asp:Label>
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                        <td align="center">
                                                                            <asp:Label ID="lblEDName" runat="server" CssClass="ClsLabel" Text='<%#Eval("ShortName") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hidIsEarning" runat="server" Value='<%#Eval("IsEarning") %>' />
                                                                        </td>
                                                                        <td align="right" style="padding-right: 5px;">
                                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="MidTxtBox" Text='<%#Eval("Amount") %>'
                                                                                Style="text-align: right; padding-right: 2px;" onblur="extractNumber(this,1,true);"
                                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, true);"
                                                                                onkeyup="extractNumber(this,1,true);" onpaste="event.returnValue=false" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                        <td align="center">
                                                                            <asp:Label ID="lblEDName" runat="server" CssClass="ClsLabel" Text='<%#Eval("ShortName") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hidIsEarning" runat="server" Value='<%#Eval("IsEarning") %>' />
                                                                        </td>
                                                                        <td align="right" style="padding-right: 5px;">
                                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="MidTxtBox" Text='<%#Eval("Amount") %>'
                                                                                Style="text-align: right; padding-right: 2px;" onblur="extractNumber(this,1,true);"
                                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, true);"
                                                                                onkeyup="extractNumber(this,1,true);" onpaste="event.returnValue=false" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    <tr>
                                                                        <td class="LblNoRecord" align="center">
                                                                            No record found.
                                                                        </td>
                                                                    </tr>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="right" class="ClsBorderlight" style="float: right;">
                                                                        <asp:Label ID="lblGrossSalaryHeader" runat="server" Font-Bold="true" CssClass="ClsLabel"
                                                                            Text="<%$ Resources:LocalizedResources, GrossSalary%>"></asp:Label>
                                                                    </td>
                                                                    <td class="ClsBorderlight" align="right" width="150px">
                                                                        <asp:Label ID="lblGrossSalary" runat="server" Font-Bold="true" CssClass="ClsLabel"
                                                                            Style="float: right; padding-right: 10px;" Text="0"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwAppointentDetails" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="cmbPaymentGroup" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAppointentDetails" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                CssClass="ClsBtn" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                CssClass="ClsBtn" CausesValidation="false" OnClick="btnCancel_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAppointentDetails" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="80%">
                                <tr runat="server" id="trTotalRec" align="center">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwAppointentDetails">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwAppointentDetails" runat="server" DataKeyNames="Id,Designation,Status"
                                            OnItemCommand="lstvwAppointentDetails_ItemCommand" OnItemDataBound="lstvwAppointentDetails_ItemDataBound"
                                            OnSorting="lstvwAppointentDetails_Sorting" OnDataBound="lstvwAppointentDetails_DataBound">
                                            <LayoutTemplate>
                                                <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" style="padding-left: 5px">
                                                            <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, Name%>"></asp:LinkButton>
                                                        </th>
                                                        <th align="left" style="padding-left: 5px" width="150px">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="SortOrder"
                                                                CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, Designation%>"></asp:LinkButton>
                                                        </th>
                                                        <th align="left">
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="StatusName"
                                                                CssClass="ClsLabel" CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, ServiceType%>"></asp:LinkButton>
                                                        </th>
                                                        <th align="center" style="padding-left: 5px">
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="JoiningDate"
                                                                CssClass="clsLabelgrd" CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, JoiningDate%>"></asp:LinkButton>
                                                        </th>
                                                        <th align="center" style="padding-left: 5px">
                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="AgreementDate"
                                                                CssClass="clsLabelgrd" CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, AgreementDate%>"></asp:LinkButton>
                                                        </th>
                                                        <th align="center">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, AppointmentLetter%>"
                                                                CssClass="clsLabelgrd" Style="text-align: center;"></asp:Label>
                                                        </th>
                                                        <th class="clsLabelgrd">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, ServiceContract%>"
                                                                CssClass="clsLabelgrd" Style="text-align: center;"></asp:Label>
                                                        </th>
                                                        <th width="30px" align="center" class="clsLabelgrd">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"
                                                                CssClass="clsLabelgrd"></asp:Label>
                                                        </th>
                                                        <th width="50px" align="center" class="clsLabelgrd">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"
                                                                CssClass="clsLabelgrd"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                        <td colspan="9">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwAppointentDetails"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
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
                                                        <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidAppId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLabel" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblServiceType" runat="server" CssClass="ClsLabel" Text='<%#Eval("Status.StatusName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblJoiningdate" runat="server" CssClass="clsLabelgrd" Text='<%#Eval("JoiningDate") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAgreementDate" runat="server" CssClass="clsLabelgrd" Text='<%#Eval("AgreementDate") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnAppointmentLetter" runat="server" CausesValidation="false"
                                                            CommandName="AppointmentLetter" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ImageUrl="../images/iconGridSml_ViewGE.gif" ToolTip="<%$ Resources:LocalizedResources, AppointmentLetter%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnServiceContract" runat="server" CausesValidation="false"
                                                            CommandName="ServiceContract" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ImageUrl="../images/iconGridSml_ViewGE.gif" ToolTip="<%$ Resources:LocalizedResources, ServiceContract%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidAppId" runat="server" Value='<%#Eval("Id") %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLabel" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblServiceType" runat="server" CssClass="ClsLabel" Text='<%#Eval("Status.StatusName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblJoiningdate" runat="server" CssClass="clsLabelgrd" Text='<%#Eval("JoiningDate") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblAgreementDate" runat="server" CssClass="clsLabelgrd" Text='<%#Eval("AgreementDate") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnAppointmentLetter" runat="server" CausesValidation="false"
                                                            CommandName="AppointmentLetter" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ImageUrl="../images/iconGridSml_ViewGE.gif" ToolTip="<%$ Resources:LocalizedResources, AppointmentLetter%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnServiceContract" runat="server" CausesValidation="false"
                                                            CommandName="ServiceContract" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ImageUrl="../images/iconGridSml_ViewGE.gif" ToolTip="<%$ Resources:LocalizedResources, ServiceContract%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.UserAppointmentDetailsBL" EnablePaging="True"
                                            SortParameterName="sortExpression" ID="objdsAppointments" runat="server" SelectMethod="GetAll"
                                            SelectCountMethod="Count" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:ControlParameter Name="sortExpression" Type="String" ControlID="hidSortExpression"
                                                    PropertyName="Value" />
                                                <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection"
                                                    PropertyName="Value" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAppointentDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAppointentDetails" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                            <asp:HiddenField ID="hidAppointmentId" runat="server" Value="0" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAppointentDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAppointentDetails" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:HiddenField ID="hidValBlankAddress" runat="server" Value="" />
                    <asp:HiddenField ID="hidValAddressLength" runat="server" Value="" />
                    <asp:HiddenField ID="hidmsgConfirmDelete" runat="server" Value="" />
                    <asp:HiddenField ID="hidValBlankJoiningDate" runat="server" Value="" />
                    <asp:HiddenField ID="hidValJoiningDateFormat" runat="server" Value="" />
                    <asp:HiddenField ID="hidvalBlankPaymentStartDate" runat="server" Value="" />
                    <asp:HiddenField ID="hidvalPaymentStartDateFormat" runat="server" Value="" />
                    <asp:HiddenField ID="hidvalBlankAgreementdate" runat="server" Value="" />
                    <asp:HiddenField ID="hidvalAgreementDateFormat" runat="server" Value="" />
                    <asp:HiddenField ID="hidPaymentGroupMsg" runat="server" Value="" />
                    <asp:HiddenField ID="hidJoiningDateValAD" runat="server" Value="" />
                    <asp:HiddenField ID="hidJoiningDateValPSD" runat="server" Value="" />
                    <asp:HiddenField ID="hidPaymentStartDateAD" runat="server" Value="" />
                    <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                </td>
            </tr>
        </table>
        <div id="divPopup" style="display: none;">
            <table align="center">
                <tr>
                    <td align="left" class="ClsBorderlight">
                        <asp:Label ID="Label14" runat="server" Text="Export To : " CssClass="ClsLabel"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="cmbFileType" runat="server" CssClass="MidCombo">                            
                            <asp:ListItem Text="PDF" Value="5"></asp:ListItem>
                            <asp:ListItem Text="MS Word" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" CausesValidation="false" OnClientClick="ShowAppointmentPopup(); return false;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript" language="javascript">

        _clientlstvwParameters = "<%=this.lstvwParameters.ClientID %>";
        _clienttxtAddress = "<%=this.txtAddress.ClientID %>";
        _clienttxtJoiningDate = "<%=this.txtJoiningDate.ClientID %>"
        _clienttxtPaymentStartDate = "<%=this.txtPaymentStartDate.ClientID %>";
        _clienttxtAgreementDate = "<%=this.txtAgreementDate.ClientID %>";
        _clientLstvwAppointentDetails = "<%=this.lstvwAppointentDetails.ClientID %>";
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>"
        _clientcmbFileType = "<%=this.cmbFileType.ClientID %>"
        _clientlblMessage = "<%=this.lblMessage.ClientID %>"

        function ShowConfirmation() {
            return confirm($get("<%=this.hidmsgConfirmDelete.ClientID %>").value);
        }

        function ValidateAddress(oSrc, args) {
            var address = $get(_clienttxtAddress).value.trim();
            if (address.trim() == "") {
                oSrc.errormessage = $get("<%=this.hidValBlankAddress.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (address.length > 300) {
                oSrc.errormessage = $get("<%=this.hidValAddressLength.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateJoiningDate(oSrc, args) {
            var bIsValid = true;
            var dtJoiningDate = $get(_clienttxtJoiningDate);
            dtJoiningDate.value = dtJoiningDate.value.trim();

            var dtAgreementDate = $get(_clienttxtAgreementDate);
            dtAgreementDate.value = dtAgreementDate.value.trim();

            var dtPaymentStartDate = $get(_clienttxtPaymentStartDate);
            dtPaymentStartDate.value = dtPaymentStartDate.value.trim();

            if (dtJoiningDate.value == "") {
                oSrc.errormessage = $get("<%=this.hidValBlankJoiningDate.ClientID %>").value;
                bIsValid = false;
            }
            else if (dtJoiningDate.value != "") {
                jDate = new Date(convertvaliddate2(dtJoiningDate.value));
                amDate = new Date(convertvaliddate2(dtAgreementDate.value));
                if (jDate == "NaN") {
                    oSrc.errormessage = $get("<%=this.hidValJoiningDateFormat.ClientID %>").value;
                    bIsValid = false;
                }
            }

            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CompaireJoiningDate(oSrc, args) {
            var bIsValid = true;
            var dtJoiningDate = $get(_clienttxtJoiningDate);
            dtJoiningDate.value = dtJoiningDate.value.trim();

            var dtAgreementDate = $get(_clienttxtAgreementDate);
            dtAgreementDate.value = dtAgreementDate.value.trim();

            var dtPaymentStartDate = $get(_clienttxtPaymentStartDate);
            dtPaymentStartDate.value = dtPaymentStartDate.value.trim();

            if (dtJoiningDate.value != "") {
                jDate = new Date(convertvaliddate2(dtJoiningDate.value));
                amDate = new Date(convertvaliddate2(dtAgreementDate.value));
                psDate = new Date(convertvaliddate2(dtPaymentStartDate.value));
                if (jDate != "NaN") {
                    if (jDate > psDate) {
                        oSrc.errormessage = oSrc.errormessage = $get("<%=this.hidJoiningDateValPSD.ClientID %>").value;
                        bIsValid = false;
                    }
                }
            }

            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CompaireJoiningDate1(oSrc, args) {
            var bIsValid = true;
            var dtJoiningDate = $get(_clienttxtJoiningDate);
            dtJoiningDate.value = dtJoiningDate.value.trim();

            var dtAgreementDate = $get(_clienttxtAgreementDate);
            dtAgreementDate.value = dtAgreementDate.value.trim();

            var dtPaymentStartDate = $get(_clienttxtPaymentStartDate);
            dtPaymentStartDate.value = dtPaymentStartDate.value.trim();

            if (dtJoiningDate.value != "") {
                jDate = new Date(convertvaliddate2(dtJoiningDate.value));
                amDate = new Date(convertvaliddate2(dtAgreementDate.value));
                psDate = new Date(convertvaliddate2(dtPaymentStartDate.value));

                if (jDate != "NaN") {
                    if (jDate < amDate) {
                        oSrc.errormessage = $get("<%=this.hidJoiningDateValAD.ClientID %>").value;
                        bIsValid = false;
                    }
                }
            }

            args.IsValid = bIsValid;
            return !bIsValid;
        }


        function CompairePaymentStartDate(oSrc, args) {
            var bIsValid = true;

            var dtAgreementDate = $get(_clienttxtAgreementDate);
            dtAgreementDate.value = dtAgreementDate.value.trim();

            var dtPaymentStartDate = $get(_clienttxtPaymentStartDate);
            dtPaymentStartDate.value = dtPaymentStartDate.value.trim();

            if (dtPaymentStartDate.value != "" && dtPaymentStartDate.value != "") {
                amDate = new Date(convertvaliddate2(dtAgreementDate.value));
                psDate = new Date(convertvaliddate2(dtPaymentStartDate.value));
                if (amDate > psDate) {
                    oSrc.errormessage = $get("<%=this.hidPaymentStartDateAD.ClientID %>").value;  //"Payment Start Date should not be less than Agreement Date.";
                    bIsValid = false;
                }
            }

            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidatePaymentStartDate(oSrc, args) {
            var bIsValid = true;
            var dtPaymentStartDate = $get(_clienttxtPaymentStartDate);
            dtPaymentStartDate.value = dtPaymentStartDate.value.trim();
            if (dtPaymentStartDate.value == "") {
                oSrc.errormessage = $get("<%=this.hidvalBlankPaymentStartDate.ClientID %>").value;
                bIsValid = false;
            }
            else if (dtPaymentStartDate.value != "") {
                jDate = new Date(convertvaliddate2(dtPaymentStartDate.value));
                if (jDate == "NaN") {
                    oSrc.errormessage = $get("<%=this.hidvalPaymentStartDateFormat.ClientID %>").value;
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateAgreementDate(oSrc, args) {
            var bIsValid = true;
            var dtAgreementDate = $get(_clienttxtAgreementDate);
            dtAgreementDate.value = dtAgreementDate.value.trim();
            if (dtAgreementDate.value == "") {
                oSrc.errormessage = $get("<%=this.hidvalBlankAgreementdate.ClientID %>").value;
                bIsValid = false;
            }
            else if (dtAgreementDate.value != "") {
                jDate = new Date(convertvaliddate2(dtAgreementDate.value));
                if (jDate == "NaN") {
                    oSrc.errormessage = $get("<%=this.hidvalAgreementDateFormat.ClientID %>").value;
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateParameterValue(oSrc, args) {
            var isFound = false;
            var rowNumber = 0;
            var txt = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_txtAmount")

            while (txt != null) {

                if (txt.value.trim() != "" && parseInt(txt.value.trim()) > 0) {
                    isFound = true;
                    break;
                }

                rowNumber++;
                txt = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_txtAmount")
            }

            args.IsValid = isFound;
            return !isFound;
        }

        function UpdateGrossSalary() {
            var grossAmount = 0;
            var isFound = false;
            var rowNumber = 0;
            var txt = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_txtAmount")

            while (txt != null) {
                txt.value = txt.value.trim();
                if (txt.value != "" && parseInt(txt.value) > 0) {
                    var hid = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_hidIsEarning")
                    if (hid.value == "True")
                        grossAmount = grossAmount + parseInt(txt.value)
                    else
                        grossAmount = grossAmount - parseInt(txt.value)
                }

                rowNumber++;
                txt = document.getElementById(_clientlstvwParameters + "_ctrl" + rowNumber + "_txtAmount")
            }

            $get("<%=this.lblGrossSalary.ClientID %>").innerHTML = grossAmount;

        }

        function ConfirmChange() {
            return window.confirm($get("<%=this.hidPaymentGroupMsg.ClientID %>").value);
        }

        function OpeReportPopup(querystring) {
            window.open(querystring, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=10,height=10')
            return false;
        }

        function DuplicateNameValidation(oSrc, args) {
            var isFound = false;
            var rowNumber = 0;
            var newName = document.getElementById("<%=this.txtName.ClientID %>").value;
            newName = newName.trim();
            if (newName != "") {
                var objSalutation = document.getElementById("<%=this.cmbSalutation.ClientID %>");
                var salutation = objSalutation.options[objSalutation.selectedIndex].text;

                var appointmentId = document.getElementById("<%=this.hidAppointmentId.ClientID %>").value;

                newName = salutation + " " + newName;

                var Name = document.getElementById(_clientLstvwAppointentDetails + "_ctrl" + rowNumber + "_lblName")

                while (Name != null) {
                    var appId = document.getElementById(_clientLstvwAppointentDetails + "_ctrl" + rowNumber + "_hidAppId").value;
                    if (appointmentId != appId && Name.innerHTML.trim().toLowerCase() == newName.trim().toLowerCase()) {
                        isFound = true;
                        break;
                    }

                    rowNumber++;
                    Name = document.getElementById(_clientLstvwAppointentDetails + "_ctrl" + rowNumber + "_lblName")
                }
            }

            args.IsValid = !isFound;
            return isFound;
        }

        eval(function (p, a, c, k, e, d) { e = function (c) { return c }; if (!''.replace(/^/, String)) { while (c--) { d[c] = k[c] || c } k = [function (e) { return d[e] } ]; e = function () { return '\\w+' }; c = 1 }; while (c--) { if (k[c]) { p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]) } } return p } ('0 1(){$2(3).4=\'\'}', 5, 5, 'function|ResetMessage|get|_clientlblMessage|innerHTML'.split('|'), 0, {}))

        function OpenPopup(queryString) {
            $get(_clienthidQueryString).value = queryString
            $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Format Selection", visible: false, modal: true, resizable: false, width: '300px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
                }

        function ShowAppointmentPopup() {
            var queryString = $get(_clienthidQueryString).value
            queryString = queryString + '&FileType=' + $get(_clientcmbFileType).value
            $.ajax({ type: "POST", data: '{"asQueryString":"' + queryString + '"}', url: "UserAppointmentDetailsUI.aspx/GetQueryString", contentType: "application/json; charset=utf-8", dataType: "json", success: function (msg) {
                var data = msg.d
                $("#divPopup").data("kendoWindow").close(); OpeReportPopup(data)
            }, error: function (msg) { } 
            });
    }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
