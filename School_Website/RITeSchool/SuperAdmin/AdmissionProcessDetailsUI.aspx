<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="AdmissionProcessDetailsUI.aspx.cs" Inherits="AdmissionProcessDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" style="width: 100%;">
            <tr>
                <td align="right" style="padding-right: 30px" valign="bottom">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                        Text="Mandatory Fields"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ShowSummary="true" />
                            <asp:RequiredFieldValidator ID="reqSelectStanderd" runat="server" ErrorMessage="Standard should be selected."
                                ControlToValidate="cmbStanderds" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstValFormCount" runat="server" ErrorMessage="" ClientValidationFunction="ValidateTotalForms"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstFormDate" runat="server" ErrorMessage="" ClientValidationFunction="ValidateAdmissionDate"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstAdmissionConfirmDate" runat="server" ErrorMessage=""
                                ClientValidationFunction="ValidateAdmissionConfirmDate" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstLotteryAdmissionDate" runat="server" ErrorMessage=""
                                ClientValidationFunction="ValidateLotteryAdmissionDate" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstDOBDate" runat="server" ErrorMessage="" ClientValidationFunction="ValidateDOBDate"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" ClientValidationFunction="ValidateMinDate"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" ClientValidationFunction="ValidateStartTime"
                                Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="" ClientValidationFunction="ValidateEndTime"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="ValidateMaxDate"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvUrl" runat="server"  ClientValidationFunction="validateUrl" 
                                  ErrorMessage="" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvExternalSiteMessage" runat="server" ErrorMessage="" ClientValidationFunction="ValidateExternalSiteMessage"
                                Display="None"></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwAdmissionProcessDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwAdmissionProcessDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="1" style="text-align: center">
                                <tr style="height: 10px;">
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 225px;" class="ClsBorderlight">
                                        <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="Standard" Height="16px"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbStanderds" runat="server" CssClass="MidCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblTotalForm" runat="server" CssClass="ClsLabel" Text="Total Forms"
                                            Height="16px"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtTotalForm" runat="server" MaxLength="5" CssClass="MidTxtBox"
                                            Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" TabIndex="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblTotalOnlineForm" runat="server" CssClass="ClsLabel" Text="Total Online Forms"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtTotalOnlineForm" runat="server" MaxLength="5" CssClass="MidTxtBox"
                                            Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" TabIndex="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Text="Form Open Date"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:TextBox ID="txtFormOpenDate" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_FormOpenDate" runat="server" Control="txtFormOpenDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblStartTime" runat="server" CssClass="ClsLabel" Text="Start Time"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:TextBox ID="txtStartTime" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblFormCloseDate" runat="server" CssClass="ClsLabel" Text="Form Close Date"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:TextBox ID="txtFormCloseDate" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_FormCloseDate" runat="server" Control="txtFormCloseDate"
                                            Format="dd MMM yyyy" Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblEndTime" runat="server" CssClass="ClsLabel" Text="End Time"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:TextBox ID="txtEndTime" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblLotteryDate" runat="server" CssClass="ClsLabel" Text="Lottery Date"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:TextBox ID="txtLotteryDate" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_LotteryDate" runat="server" Control="txtLotteryDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblAdmissionConfirmDate" runat="server" CssClass="ClsLabel" Text="Admission Confirmation  Last Date"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:TextBox ID="txtAdmissionConfirmDate" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_AdmissionConfirmDate" runat="server" Control="txtAdmissionConfirmDate"
                                            Format="dd MMM yyyy" Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblIsLotteryConfirmDate" runat="server" CssClass="ClsLabel" Text="Is Lottery Confirmed?"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkIsLotteryConfirm" runat="server" CssClass="LblSmlRslt" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblCanConfirmDirectly" runat="server" CssClass="ClsLabel" Text="Can Confirm Directly?"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkCanConfirmDirectly" runat="server" CssClass="LblSmlRslt" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabel" Text="Amount"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="MidTxtBox" MaxLength="6" Style="text-align: right;
                                            padding-right: 5px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                            onkeypress="return blockNonNumbers(this, event, false, false);" onkeyup="extractNumber(this,2,false);"
                                            onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblDOBMin" runat="server" CssClass="ClsLabel" Text="DOB Min. Limit"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDOBMin" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_DOBMin" runat="server" Control="txtDOBMin" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" AutoPostBack="False" To- />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblDOBMax" runat="server" CssClass="ClsLabel" Text="DOB Max. Limit"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDOBMax" CssClass="MidTxtBox" runat="server"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_DOBMax" runat="server" Control="txtDOBMax" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblEnableAdmissionFormFee" runat="server" CssClass="ClsLabel" Text="Enable Admission Form Fee?"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkEnableAdmissionFormFee" runat="server" CssClass="LblSmlRslt" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblIsInternalAdmission" runat="server" CssClass="ClsLabel" Text="Is Internal Admission?"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkIsInternalAdmission" runat="server" CssClass="LblSmlRslt" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblEnableWaitingList" runat="server" CssClass="ClsLabel" Text="Enable Wating List"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkEnableWaitingList" runat="server" CssClass="LblSmlRslt" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Wating List(URL)"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                       <asp:TextBox ID="txtWaitingListURL" runat="server" MaxLength="100" CssClass="LrgTxtBox" Width="153%" TextMode="SingleLine"></asp:TextBox>
                                    </td>
                                  
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblEnableInternalLink" runat="server" CssClass="ClsLabel" Text="Enable Internal Link?"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkEnableInternalLink" runat="server" CssClass="LblSmlRslt" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblExternalSiteMessage" runat="server" CssClass="ClsLabel" Text="Display Text on External Site"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtExternalSiteMessage" runat="server" MaxLength="100" CssClass="LrgTxtBox" Width="100%" TextMode="SingleLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height: 10px">
                                    <td colspan="2">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwAdmissionProcessDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                CausesValidation="False" OnClick="btnCancel_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwAdmissionProcessDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="Height10">
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListView ID="lstvwAdmissionProcessDetails" runat="server" DataKeyNames="AdmissionProcessId"
                                OnItemCommand="lstvwAdmissionProcessDetails_ItemCommand" OnItemDeleting="lstvwAdmissionProcessDetails_ItemDeleting"
                                OnItemEditing="lstvwAdmissionProcessDetails_ItemEditing" OnSelectedIndexChanged="lstvwAdmissionProcessDetails_SelectedIndexChanged"
                                OnItemDataBound="lstvwAdmissionProcessDetails_ItemDataBound">
                                <LayoutTemplate>
                                    <table width="80%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr class="ClsGridHeader" id="trHeader" runat="server">
                                            <th align="left" class="clsLabelgrd">
                                                <span><b>Standard</b></span>
                                            </th>
                                            <th align="center" width="150px" class="clsLabelgrd">
                                                <span><b>Form Open Date</b></span>
                                            </th>
                                            <th align="center" class="clsLabelgrd" width="150px">
                                                <span><b>Form Close Date</b></span>
                                            </th>
                                            <th align="right" class="clsLabelgrd" width="100px" style="padding-right: 5px;">
                                                <span><b>Amount</b></span>
                                            </th>
                                            <th align="center" class="clsLabelgrd" width="150px">
                                                <span><b>DOB Min. Limit</b></span>
                                            </th>
                                            <th align="center" class="clsLabelgrd" width="150px">
                                                <span><b>DOB Max. Limit</b></span>
                                            </th>
                                            <th width="40px" align="center" class="clsLabelgrd">
                                                <asp:Label ID="lblEdit" runat="server" Text="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                            </th>
                                            <th width="40px" align="center" class="clsLabelgrd">
                                                <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <td align="center">
                                            <asp:Label ID="lblStanderd" runat="server" CssClass="ClsLabel" Text='<%#Eval("StandardName") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("FormOpenDate") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblFormCloseDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("FormCloseDate") %>'></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                padding-right: 5px;" Text='<%#Eval("Amount") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblDOBMin" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("DOBMin") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblDOBMax" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("DOBMax") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <td align="center">
                                            <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("StandardName") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("FormOpenDate") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblFormCloseDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("FormCloseDate") %>'></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                padding-right: 5px;" Text='<%#Eval("Amount") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblDOBMin" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("DOBMin") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblDOBMax" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("DOBMax") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                            <asp:HiddenField ID="hidAdmissionId" runat="server" Value="0" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwAdmissionProcessDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">
            _clientTxtDOBMaxDate = "<%=this.txtDOBMax.ClientID %>";
            _clientTxtDOBMinDate = "<%=this.txtDOBMin.ClientID %>";
            _clientTxtFormOpenDate = "<%=this.txtFormOpenDate.ClientID %>";
            _clientTxtFormCloseDate = "<%=this.txtFormCloseDate.ClientID %>";
            _clientTxtLotteryDate = "<%=this.txtLotteryDate.ClientID %>";
            _clientTxtAdmissionConfirmLastDate = "<%=this.txtAdmissionConfirmDate.ClientID %>";
            _clientlblMessage = "<%=this.lblMessage.ClientID %>"
            _clienttxtTotalOnlineForm = "<%=this.txtTotalOnlineForm.ClientID %>"
            _clienttxtTotalForm = "<%=this.txtTotalForm.ClientID %>"
            _clienttxtStartTime = "<%=this.txtStartTime.ClientID %>"
            _clienttxtEndTime = "<%=this.txtEndTime.ClientID %>"
            _clientchkEnableInternalLink = "<%=this.chkEnableInternalLink.ClientID %>"
            _clienttxtExternalSiteMessage = "<%=this.txtExternalSiteMessage.ClientID %>"


            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?');
            }

            function ClearMessage() {
                document.getElementById(_clientlblMessage).innerHTML = "";
                $get(_clientlblMessage).innerHTML = "";
                $("#" + _clientlblMessage).html("");
            }

            function ValidateStartTime(oSrc, args) {
                var StartTime = $('#' + _clienttxtStartTime).val()
                if (StartTime.trim() != "") {
                    if (!isTimeValid(_clienttxtStartTime)) {
                        oSrc.errormessage = "Start Time should be in HH:MM AM/PM (e.g 10:00 AM)."
                        args.IsValid = false
                        return true
                    }
                }
                args.IsValid = true
                return false
            }



            function ValidateEndTime(oSrc, args) {
                var EndTime = $('#' + _clienttxtEndTime).val()
                if (EndTime.trim() != "") {
                    if (!isTimeValid(_clienttxtEndTime)) {
                        oSrc.errormessage = "End Time should be in HH:MM AM/PM (e.g 10:00 AM)."
                        args.IsValid = false
                        return true
                    }
                }
                args.IsValid = true
                return false
            }

            function isTimeValid(txtTimeId) {

                var timeStr = trimAll(document.getElementById(txtTimeId).value.toUpperCase());
                if (trimAll(timeStr) == '')
                    return false;

                // Checks if time is in HH:MM 12 hour format.
                // The seconds are optional.
                var timePat = /^(\d{1,2}):(\d{1,2})?(\s)(AM|am|PM|pm)?$/;
                var matchArray = timeStr.match(timePat);

                if (matchArray == null)
                    return false;

                if (timeStr.length < 6)
                    return false;

                hour = matchArray[1];
                minute = matchArray[2];
                ampm = matchArray[4];

                if (ampm == "") {
                    return false;
                }

                if (hour <= 0 || hour > 12)
                    return false;

                if (minute < 0 || minute > 59)
                    return false;

                var str;
                if (hour.length == 1)
                    str = '0' + hour;
                else
                    str = hour;
                if (minute.length == 1)
                    str = str + ':' + minute + '0';
                else
                    str = str + ':' + minute;

                str = str + ' ' + ampm.toUpperCase();

                document.getElementById(txtTimeId).value = str;
                return true;
            }

            function ValidateDOBDate(oSrc, args) {
                var DOBMax = $get(_clientTxtDOBMaxDate).value;
                var DOBMin = $get(_clientTxtDOBMinDate).value;

                if (DOBMax.trim() != "" && DOBMin.trim() != "") {
                    var dtDOBMax;
                    if (document.all)
                        dtDOBMax = new Date(DOBMax.replace('-', ' '));
                    else
                        dtDOBMax = new Date(convertdate(DOBMax));

                    var dtDOBMin;
                    if (document.all)
                        dtDOBMin = new Date(DOBMin.replace('-', ' '));
                    else
                        dtDOBMin = new Date(convertdate(DOBMin));


                    if (dtDOBMax < dtDOBMin) {
                        oSrc.errormessage = "DOB Maximum Limit should not be less than Minimum Limit."
                        args.IsValid = false
                        return true
                    }
                }

                args.IsValid = true
                return false
            }

            function ValidateAdmissionDate(oSrc, args) {
                var FormOpenDate = $get(_clientTxtFormOpenDate).value;
                var FormCloseDate = $get(_clientTxtFormCloseDate).value;

                if (FormOpenDate.trim() != "" && FormCloseDate.trim() != "") {
                    var dtFormOpenDate;
                    if (document.all)
                        dtFormOpenDate = new Date(FormOpenDate.replace('-', ' '));
                    else
                        dtFormOpenDate = new Date(convertdate(FormOpenDate));

                    var dtFormCloseDate;
                    if (document.all)
                        dtFormCloseDate = new Date(FormCloseDate.replace('-', ' '));
                    else
                        dtFormCloseDate = new Date(convertdate(FormCloseDate));


                    if (dtFormCloseDate < dtFormOpenDate) {
                        oSrc.errormessage = "Admission Form Close Date should not be less than Form Open Date."
                        args.IsValid = false
                        return true
                    }
                }

                args.IsValid = true
                return false
            }
            function ValidateAdmissionConfirmDate(oSrc, args) {
                var FormCloseDate = $get(_clientTxtFormCloseDate).value;
                var AdmissionConfirmLastDate = $get(_clientTxtAdmissionConfirmLastDate).value;

                if (FormCloseDate.trim() != "" && AdmissionConfirmLastDate.trim() != "") {
                    var dtFormCloseDate;
                    if (document.all)
                        dtFormCloseDate = new Date(FormCloseDate.replace('-', ' '));
                    else
                        dtFormCloseDate = new Date(convertdate(FormCloseDate));

                    var dtAdmissionConfirmLastDate;
                    if (document.all)
                        dtAdmissionConfirmLastDate = new Date(AdmissionConfirmLastDate.replace('-', ' '));
                    else
                        dtAdmissionConfirmLastDate = new Date(convertdate(AdmissionConfirmLastDate));


                    if (dtAdmissionConfirmLastDate < dtFormCloseDate) {
                        oSrc.errormessage = "Admission Confirm Last Date should not be less than Form Close Date."
                        args.IsValid = false
                        return true
                    }
                }

                args.IsValid = true
                return false
            }

            function ValidateLotteryAdmissionDate(oSrc, args) {
                var LotteryDate = $get(_clientTxtLotteryDate).value;
                var AdmissionConfirmLastDate = $get(_clientTxtAdmissionConfirmLastDate).value;


                if (LotteryDate.trim() != "" && AdmissionConfirmLastDate.trim() != "") {
                    var dtLotteryDate;
                    if (document.all)
                        dtLotteryDate = new Date(LotteryDate.replace('-', ' '));
                    else
                        dtLotteryDate = new Date(convertdate(LotteryDate));

                    var dtAdmissionConfirmLastDate;
                    if (document.all)
                        dtAdmissionConfirmLastDate = new Date(AdmissionConfirmLastDate.replace('-', ' '));
                    else
                        dtAdmissionConfirmLastDate = new Date(convertdate(AdmissionConfirmLastDate));


                    if (dtAdmissionConfirmLastDate < dtLotteryDate) {
                        oSrc.errormessage = "Admission Confirm Last Date should not be less than Lottery Date."
                        args.IsValid = false
                        return true
                    }
                }
                args.IsValid = true
                return false
            }


            function ValidateTotalForms(oSrc, args) {
                var totalForms = $('#' + _clienttxtTotalForm).val();
                var totalOnlineForms = $('#' + _clienttxtTotalOnlineForm).val();

                if (totalOnlineForms != "" && parseInt(totalOnlineForms) != 0 && (totalForms == "" || parseInt(totalForms) == 0)) {
                    oSrc.errormessage = "If value for Total Online Forms is available then Total Forms should not be blank or zero.";
                    args.IsValid = false;
                    return true;
                }
                else if (totalForms != "" && parseInt(totalForms) != 0 && totalOnlineForms != "" && parseInt(totalOnlineForms) != 0) {
                    if (parseInt(totalForms) < parseInt(totalOnlineForms)) {
                        oSrc.errormessage = "Total Online Forms should not be greater than Total Forms.";
                        args.IsValid = false;
                        return true;
                    }
                }

                args.IsValid = true
                return false
            }

            function ValidateMinDate(oSrc, args) {
                var FormCloseDate = $('#' + _clientTxtFormCloseDate).val()
                var formOpenDate = $('#' + _clientTxtFormOpenDate).val()
                var dobMin = $('#' + _clientTxtDOBMinDate).val()


                if (dobMin.trim() != "" && (FormCloseDate.trim() != "" || formOpenDate.trim() != "")) {

                    var dtdobMin;
                    if (document.all)
                        dtdobMin = new Date(dobMin.replace('-', ' '));
                    else
                        dtdobMin = new Date(convertdate(dobMin));

                    if (formOpenDate.trim() != "") {

                        var dtformOpenDate
                        if (document.all)
                            dtformOpenDate = new Date(formOpenDate.replace('-', ' '));
                        else
                            dtformOpenDate = new Date(convertdate(formOpenDate));

                        if (dtdobMin > dtformOpenDate) {
                            oSrc.errormessage = "DOB Min. Limit should not be greater than Form Open Date."
                            args.IsValid = false
                            return true
                        }
                    }
                    else {
                        var dtFormCloseDate
                        if (document.all)
                            dtFormCloseDate = new Date(FormCloseDate.replace('-', ' '));
                        else
                            dtFormCloseDate = new Date(convertdate(FormCloseDate));

                        if (dtdobMin > dtFormCloseDate) {
                            oSrc.errormessage = "DOB Min. Limit should not be greater than Form Close Date."
                            args.IsValid = false
                            return true
                        }
                    }
                }

                args.IsValid = true
                return false
            }

            function ValidateMaxDate(oSrc, args) {
                var FormCloseDate = $('#' + _clientTxtFormCloseDate).val()
                var formOpenDate = $('#' + _clientTxtFormOpenDate).val()
                var dobMax = $('#' + _clientTxtDOBMaxDate).val()


                if (dobMax.trim() != "" && (FormCloseDate.trim() != "" || formOpenDate.trim() != "")) {

                    var dtdobMax;
                    if (document.all)
                        dtdobMax = new Date(dobMax.replace('-', ' '));
                    else
                        dtdobMax = new Date(convertdate(dobMax));

                    if (formOpenDate.trim() != "") {

                        var dtformOpenDate
                        if (document.all)
                            dtformOpenDate = new Date(formOpenDate.replace('-', ' '));
                        else
                            dtformOpenDate = new Date(convertdate(formOpenDate));

                        if (dtdobMax > dtformOpenDate) {
                            oSrc.errormessage = "DOB Max. Limit should not be greater than Form Open Date."
                            args.IsValid = false
                            return true
                        }
                    }
                    else {
                        var dtFormCloseDate
                        if (document.all)
                            dtFormCloseDate = new Date(FormCloseDate.replace('-', ' '));
                        else
                            dtFormCloseDate = new Date(convertdate(FormCloseDate));

                        if (dtdobMax > dtFormCloseDate) {
                            oSrc.errormessage = "DOB Max. Limit should not be greater than Form Close Date."
                            args.IsValid = false
                            return true
                        }
                    }
                }

                args.IsValid = true
                return false
            }

            function validateUrl(sender, args) {
                var chk = document.getElementById('<%= chkEnableWaitingList.ClientID %>');
                var url = document.getElementById('<%= txtWaitingListURL.ClientID %>').value.trim();

                var urlPattern = /^(https?:\/\/)([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(\/.*)?$/;

                if (chk.checked) {
                    if (url === "") {
                        args.IsValid = false;
                        sender.errormessage = "URL is required when Enable Waiting List is checked.";
                        return;
                    }

                    if (!urlPattern.test(url)) {
                        args.IsValid = false;
                        sender.errormessage = "Please enter a valid URL starting with http:// or https://";
                        return;
                    }
                }
               args.IsValid = true;
            }

            // Validation function for External Site Message - required when Enable Internal Link is checked
            function ValidateExternalSiteMessage(oSrc, args) {
                var chkEnableInternalLink = document.getElementById(_clientchkEnableInternalLink);
                var txtExternalSiteMessage = document.getElementById(_clienttxtExternalSiteMessage);
                var chkEnableWaitingList = document.getElementById('<%= chkEnableWaitingList.ClientID %>');
                
                if (chkEnableInternalLink && chkEnableInternalLink.checked) {
                    if (txtExternalSiteMessage && txtExternalSiteMessage.value.trim() === "") {
                        oSrc.errormessage = "'Display Text on External Site' should not be empty when 'Enable Internal Link' is checked.";
                        args.IsValid = false;
                        return;
                    }
                    else if (chkEnableWaitingList.checked) {
                        oSrc.errormessage = "You can not enable 'Waiting List' and 'Internal Link' for same standard.";
                        args.IsValid = false;
                        return;
                    }
                }
                args.IsValid = true;
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
