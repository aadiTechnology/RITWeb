<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="TaskManagementPopup.aspx.cs" Inherits="TaskManagementPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top; height: 450px">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; vertical-align: top;">
            <tr>
                <td>
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                    <table>
                        <tr>
                            <td align="left" style="vertical-align: top; width: 800px;">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="ClsGrayMainTitle" align="left" style="padding-left: 10px">
                                                                <span class="MainTitleHead">Add New Task</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="width: 100%; padding-right: 30px; height: 21px;" class="ClsBtmBorderGray"
                                                                valign="top">
                                                                <span class="ClsMdtStar">* Mandatory Fields</span>
                                                            </td>
                                                        </tr>
                                                        <tr align="left" id="trValSummary" runat="server">
                                                            <td align="center">
                                                                <asp:ValidationSummary ID="valSumTaskDetails" CssClass="LblErrorMsg" ShowSummary="true"
                                                                    runat="server" ValidationGroup="TaskStartStep" />
                                                                <asp:ValidationSummary ID="valsumUserDetails" CssClass="LblErrorMsg" ShowSummary="true"
                                                                    runat="server" ValidationGroup="TaskNavigateStep" />
                                                                <asp:ValidationSummary ID="valsumStatusDetails" CssClass="LblErrorMsg" ShowSummary="true"
                                                                    runat="server" ValidationGroup="TaskFinishStep" />
                                                                <%--<asp:ValidationSummary ID="valSumChildDetails" CssClass="LblErrorMsg" ShowSummary="true"
                                                runat="server" ValidationGroup="SaveChild" />--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--<asp:CustomValidator ID="cstCheckAtLeastOne" Display="None" runat="server" ValidationGroup="Save"
                                                                            CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                                                            ClientValidationFunction="CheckAtLeastOne"></asp:CustomValidator>--%>
                                                                <asp:CustomValidator ID="cstCheckAtLeastOne" Display="None" runat="server" ValidationGroup="TaskNavigateStep"
                                                                    CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"></asp:CustomValidator>
                                                                <%-- ErrorMessage="Please enter valid start time e.g. 10:00 AM." 
                                                                        <asp:CustomValidator ID="cstStartTime" runat="server" Display="none" ErrorMessage="Please enter valid time."
                                                            SetFocusOnError="True" ValidationGroup="Save"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cstEndTime" runat="server" Display="none" ErrorMessage="Please enter valid time."
                                                            SetFocusOnError="True" ValidationGroup="Save"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cstBufferTime" runat="server" Display="none" ErrorMessage="Please enter valid time."
                                                            SetFocusOnError="True" ValidationGroup="Save"></asp:CustomValidator>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="height20" style="width: 100px">
                                <asp:Label ID="lblMessage" runat="server" CssClass="LblErrorMsg"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<asp:UpdatePanel runat="server" ID="UpdatePanel11">
                                            <ContentTemplate>--%>
                                <asp:Wizard ID="wizard_TaskDetails" runat="server" DisplaySideBar="False" ActiveStepIndex="0"
                                    DisplayCancelButton="True" Width="100%" OnActiveStepChanged="wizard_TaskDetails_ActiveStepChanged"
                                    OnFinishButtonClick="wizard_TaskDetails_FinishButtonClick" OnNextButtonClick="wizard_TaskDetails_NextButtonClick"
                                    TabIndex="10">
                                    <WizardSteps>
                                        <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1" StepType="Start">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" class="ClsBtmBorderGray" style="height: 21px">
                                                                    <span class="ClsLblLgnd" style="font-weight: bold">Task Details :</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%" align="center" style="padding-left: 5px; height: 172px">
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" style="width: 125px">
                                                                    <span class="ClsLabel">Task :</span>
                                                                </td>
                                                                <td align="left" colspan="3" style="padding-left: 5px">
                                                                    <asp:TextBox ID="txtTaskName" CssClass="LrgTxtBox" runat="server" Width="535px" MaxLength="100"></asp:TextBox><span
                                                                        class="ClsMdtStar">*</span>
                                                                    <asp:RequiredFieldValidator ID="reqTaskName" runat="server" ControlToValidate="txtTaskName"
                                                                        Display="None" ValidationGroup="TaskStartStep" ErrorMessage="Task Name should not be blank."
                                                                        CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" style="width: 125px">
                                                                    <span class="ClsLabel">Task Details :</span>
                                                                </td>
                                                                <td align="left" colspan="3" style="padding-left: 5px">
                                                                    <asp:TextBox ID="txtTaskDetails" CssClass="LrgTxtBox" runat="server" Width="535px"
                                                                        TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" style="width: 125px">
                                                                    <span class="ClsLabel">Start Date and Time :</span>
                                                                </td>
                                                                <td style="width: 190px" colspan="1">
                                                                    <table style="width: 210px">
                                                                        <tr>
                                                                            <td colspan="1" style="padding-left: 2px">
                                                                                <asp:TextBox ID="txtStartDate" runat="server" MaxLength="11" CssClass="SmlTxtBox"
                                                                                    AutoPostBack="false" Width="90px" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                                <rjs:PopCalendar ID="calStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                                                    ShowWeekend="True" ShowErrorMessage="false" /><span class="ClsMdtStar">*</span>
                                                                                <asp:RequiredFieldValidator ID="repStartDate" runat="server" ControlToValidate="txtStartDate"
                                                                                    Display="None" ValidationGroup="TaskStartStep" ErrorMessage="Start Date should not be blank."
                                                                                    CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtStartTime" CssClass="SmlTxtBox" runat="server" MaxLength="8"
                                                                                    Width="50px"></asp:TextBox><span class="ClsMdtStar">*</span>
                                                                                <asp:RequiredFieldValidator ID="reqStartTime" runat="server" ControlToValidate="txtStartTime"
                                                                                    Display="None" ValidationGroup="TaskStartStep" ErrorMessage="Start Time should not be blank."
                                                                                    CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <%--<tr>
                                                                            <td style="width: 130px">
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:CustomValidator ID="cstInvalidStartTime" runat="server" SetFocusOnError="True"
                                                                                    Display="None" ErrorMessage="Please enter valid start time e.g. 10:00 AM." ClientValidationFunction="IsValidStartTime"
                                                                                    ValidationGroup="TaskStartStep" ControlToValidate="txtStartTime"></asp:CustomValidator>
                                                                                <span>e.g. 10:00 AM</span>
                                                                            </td>
                                                                        </tr>--%>
                                                                    </table>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" style="width: 131px">
                                                                    <span class="ClsLabel">End Date and Time :</span>
                                                                </td>
                                                                <td style="width: 190px" colspan="1">
                                                                    <table style="width: 235px">
                                                                        <tr>
                                                                            <td colspan="1" style="width: 135px">
                                                                                <asp:TextBox ID="txtEndDate" runat="server" Width="90px" MaxLength="11" CssClass="SmlTxtBox" Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox><rjs:PopCalendar ID="calEndDate" Width="80px" runat="server" Control="txtEndDate"
                                                                                    Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" /><span class="ClsMdtStar">*</span>
                                                                                <asp:RequiredFieldValidator ID="reqEndDate" runat="server" ControlToValidate="txtEndDate"
                                                                                    Display="None" ValidationGroup="TaskStartStep" ErrorMessage="End Date should not be blank."
                                                                                    CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtEndTime" CssClass="SmlTxtBox" runat="server" MaxLength="8" Width="50px"></asp:TextBox><span
                                                                                    class="ClsMdtStar">*</span>
                                                                                <asp:RequiredFieldValidator ID="reqEndTime" runat="server" ControlToValidate="txtEndTime"
                                                                                    Display="None" ValidationGroup="TaskStartStep" ErrorMessage="End Time should not be blank."
                                                                                    CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <%-- <tr>
                                                                            <td style="width: 130px">
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:CustomValidator ID="CustomValidator1" runat="server" SetFocusOnError="True"
                                                                                    Display="None" ErrorMessage="Please enter valid start time e.g. 10:00 AM." ClientValidationFunction="IsValidStartTime"
                                                                                    ValidationGroup="TaskStartStep" ControlToValidate="txtStartTime"></asp:CustomValidator>
                                                                                <span>e.g. 10:00 AM</span>
                                                                            </td>
                                                                        </tr>--%>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 125px">
                                                                </td>
                                                                <td align="right" style="padding-right: 30px; width: 189px;">
                                                                    <asp:CustomValidator ID="cstInvaliStartTime" runat="server" SetFocusOnError="True"
                                                                        Display="None" ErrorMessage="Please enter valid start time e.g. 10:00 AM." ClientValidationFunction="IsValidStartTime"
                                                                        ValidationGroup="TaskStartStep" ControlToValidate="txtStartTime"></asp:CustomValidator>
                                                                    <span>e.g. 10:00 AM</span>
                                                                </td>
                                                                <td align="left" style="width: 125px">
                                                                </td>
                                                                <td align="right" style="padding-right: 10px">
                                                                    <asp:CustomValidator ID="cstInvaliEndTime" runat="server" SetFocusOnError="True"
                                                                        Display="None" ErrorMessage="Please enter valid end time e.g. 10:00 AM." ClientValidationFunction="IsValidEndTime"
                                                                        ValidationGroup="TaskStartStep" ControlToValidate="txtEndTime"></asp:CustomValidator>
                                                                    <span>e.g. 10:00 AM</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" style="width: 185px">
                                                                    <span class="ClsLabel">Buffer Date and Time :</span>
                                                                </td>
                                                                <td style="width: 170px; padding-left: 2px;" colspan="1">
                                                                    <table style="width: 220px">
                                                                        <tr>
                                                                            <td colspan="1" align="left" style="width: 130px">
                                                                                <asp:TextBox ID="txtBufferDate" runat="server" MaxLength="11" CssClass="SmlTxtBox"
                                                                                    AutoPostBack="false" Width="90px" Text='<%# Eval("BufferDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                                <rjs:PopCalendar ID="calBufferDate" runat="server" Control="txtBufferDate" Format="dd MMM yyyy"
                                                                                    ShowWeekend="True" ShowErrorMessage="false" />
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtBufferTime" CssClass="SmlTxtBox" runat="server" MaxLength="8"
                                                                                    Width="50px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <%--<tr>
                                                                             <td style="width: 130px">
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:CustomValidator ID="CustomValidator2" runat="server" SetFocusOnError="True"
                                                                                    Display="None" ErrorMessage="Please enter valid start time e.g. 10:00 AM." ClientValidationFunction="IsValidStartTime"
                                                                                    ValidationGroup="TaskStartStep" ControlToValidate="txtStartTime"></asp:CustomValidator>
                                                                                <span>e.g. 10:00 AM</span>
                                                                            </td>
                                                                        </tr>--%>
                                                                    </table>
                                                                </td>
                                                                <td colspan="2" align="left">
                                                                    <table>
                                                                        <tr>
                                                                            <td align="left" valign="top">
                                                                                <asp:RadioButton ID="optDailyTask" runat="server" GroupName="Filter" Checked="true"
                                                                                    Text="Daily Task" onclick="CheckUncheckRadioBtn()" TabIndex="1" CssClass="LblNormal"
                                                                                    AutoPostBack="true" OnCheckedChanged="optDailyTask_OnCheckedChanged" />
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:RadioButton ID="optNormalTask" runat="server" AutoPostBack="true" GroupName="Filter"
                                                                                    onclick="CheckUncheckRadioBtn()" TabIndex="1" Text="Normal Task" CssClass="LblNormal"
                                                                                    OnCheckedChanged="optNormalTask_OnCheckedChanged" />
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:RadioButton ID="optGeneralTask" runat="server" AutoPostBack="true" GroupName="Filter"
                                                                                    onclick="CheckUncheckRadioBtn()" TabIndex="1" Text="General Task" CssClass="LblNormal"
                                                                                    OnCheckedChanged="optGeneralTask_OnCheckedChanged" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 125px">
                                                                </td>
                                                                <td align="right" style="padding-right: 30px; width: 189px;">
                                                                    <asp:CustomValidator ID="cstInvaliBufferTime" runat="server" SetFocusOnError="True"
                                                                        Display="None" ErrorMessage="Please enter valid buffer time e.g. 10:00 AM." ClientValidationFunction="IsValidBufferTime"
                                                                        ValidationGroup="TaskStartStep" ControlToValidate="txtBufferTime"></asp:CustomValidator>
                                                                    <span>e.g. 10:00 AM</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</td> </tr> </table>--%>
                                        </asp:WizardStep>
                                        <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 50px">
                                                    </td>
                                                    <td align="right" class="ClsBorderlight" style="width: 150px">
                                                        <span class="ClsLabel">Assigned To :</span>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px; width: 250px">
                                                        <asp:DropDownList ID="cmbDesignation" CssClass="LrgCombo" runat="server" OnSelectedIndexChanged="cmbDesignation_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList><span class="ClsMdtStar">*</span>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px; width: 100px">
                                                        <asp:CheckBox ID="chkIncludeMe" runat="server" Text="Include Me" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="4">
                                                        <table align="center" width="100%">
                                                            <tr>
                                                                <td align="center" valign="top">
                                                                    <div id="divUserListView" runat="server" visible="true" class="GridBorder" style="width: 600px;
                                                                        height: 300px; overflow: scroll;">
                                                                        <asp:ListView ID="lstvwUserDetails" runat="server" DataKeyNames="UserId,DesignationId,IsSelected"
                                                                            OnItemDataBound="lstvwUserDetails_ItemDataBound" OnItemCommand="lstvwUserDetails_ItemCommand"
                                                                            OnDataBound="lstvwUserDetails_DataBound">
                                                                            <LayoutTemplate>
                                                                                <table width="100%" runat="server" id="tblUser" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1" class="GridBorder">
                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                        <th align="center" style="width: 10%">
                                                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                                                        </th>
                                                                                        <th align="left" style="width:40%; padding-left: 11px">
                                                                                            Resource Name
                                                                                        </th>
                                                                                        <th align="left" style="padding-left: 11px">
                                                                                            Designation
                                                                                        </th>
                                                                                        <th align="center" style="padding-left: 11px">
                                                                                            Task Details
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr id="trUserDetails" runat="server" class="ClsGridRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                                    </td>
                                                                                    <td align="left" style="padding-left: 8px">
                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName")%>' CssClass="LblNormal"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="padding-left: 8px">
                                                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%#Eval("DesignationName")%>'
                                                                                            CssClass="LblNormal"></asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:ImageButton ID="imgDetails" runat="server" CommandName="DETAIL" ToolTip="Task Details"
                                                                                            ImageUrl="~/RITeSchool/images/Selection5.gif" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trUserTaskDetails" runat="server" visible="false" align="center">
                                                                                    <td id="tdUserTaskDetails" runat="server" colspan="4" align="center">
                                                                                        <table align="center" width="95%">
                                                                                            <tr>
                                                                                                <td align="center" valign="top">
                                                                                                    <div id="divUserTaskDetailContainor" runat="server" visible="true" class="GridBorder"
                                                                                                        style="width: 500px; height: 150px; overflow: scroll;">
                                                                                                        <asp:ListView ID="lstvwUserTaskDetails" runat="server" OnItemDataBound="lstvwUserTaskDetails_ItemDataBound">
                                                                                                            <%--DataKeyNames="TaskDetailsId,TaskId,TaskAssignerUserId">--%>
                                                                                                            <LayoutTemplate>
                                                                                                                <table width="800px" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                                    cellspacing="1" class="GridBorder">
                                                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                                        <th align="left" style="width: 200px; padding-left: 12px">
                                                                                                                            Task Name
                                                                                                                        </th>
                                                                                                                        <th align="left" style="width: 200px; padding-left: 12px">
                                                                                                                            Assigner Name
                                                                                                                        </th>
                                                                                                                        <th align="left" style="width: 150px; padding-left: 12px">
                                                                                                                            Start Date and Time
                                                                                                                        </th>
                                                                                                                        <th align="left" style="width: 150px; padding-left: 12px">
                                                                                                                            End Date and Time
                                                                                                                        </th>
                                                                                                                    </tr>
                                                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </LayoutTemplate>
                                                                                                            <ItemTemplate>
                                                                                                                <tr id="trUTaskDetails" runat="server" class="ClsGridRow">
                                                                                                                    <td align="left" style="width: 250px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblTaskName" runat="server" Text='<%#Eval("TaskName")%>' CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 250px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("TaskAssignerName")%>' CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 150px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblStartDateTime" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 150px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblEndDateTime" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </ItemTemplate>
                                                                                                            <AlternatingItemTemplate>
                                                                                                                <tr id="trUTaskDetails" runat="server" class="ClsGridRow">
                                                                                                                    <td align="left" style="width: 200px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblTaskName" runat="server" Text='<%#Eval("TaskName")%>' CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 200px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("TaskAssignerName")%>' CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 150px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblStartDateTime" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 150px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblEndDateTime" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </AlternatingItemTemplate>
                                                                                                            <EmptyDataTemplate>
                                                                                                                <table width="100%">
                                                                                                                    <tr>
                                                                                                                        <td class="LblNoRecord" align="center">
                                                                                                                            No Records Found.
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </EmptyDataTemplate>
                                                                                                        </asp:ListView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                                                                                        Text="Cancel"  OnClick="BtnCancelTask_Click" BorderWidth="1px"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <tr id="trUserDetails" runat="server" class="ClsGridAltRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                                    </td>
                                                                                    <td align="left" style="padding-left: 8px">
                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName")%>' CssClass="LblNormal"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="padding-left: 8px">
                                                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%#Eval("DesignationName")%>'
                                                                                            CssClass="LblNormal"></asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:ImageButton ID="imgDetails" runat="server" CommandName="DETAIL" ToolTip="Task Details"
                                                                                            ImageUrl="~/RITeSchool/images/Selection5.gif" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="trUserTaskDetails" runat="server" visible="false" align="center">
                                                                                    <td id="tdUserTaskDetails" runat="server" colspan="4" align="center">
                                                                                        <table align="center" width="95%">
                                                                                            <tr>
                                                                                                <td align="center" valign="top">
                                                                                                    <div id="divUserTaskDetailContainor" runat="server" visible="true" class="GridBorder"
                                                                                                        style="width: 100%; height: 150px; overflow: scroll;">
                                                                                                        <asp:ListView ID="lstvwUserTaskDetails" runat="server" OnItemDataBound="lstvwUserTaskDetails_ItemDataBound">
                                                                                                            <%-- DataKeyNames="TaskDetailsId,TaskId,TaskAssignerUserId">--%>
                                                                                                            <LayoutTemplate>
                                                                                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                                    cellspacing="1" class="GridBorder">
                                                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                                        <th align="left" style="width: 200px; padding-left: 12px">
                                                                                                                            Task Name
                                                                                                                        </th>
                                                                                                                        <th align="left" style="width: 200px; padding-left: 12px">
                                                                                                                            Assigner Name
                                                                                                                        </th>
                                                                                                                        <th align="left" style="width: 170px; padding-left: 12px">
                                                                                                                            Start Date and Time
                                                                                                                        </th>
                                                                                                                        <th align="left" style="width: 170px; padding-left: 12px">
                                                                                                                            End Date and Time
                                                                                                                        </th>
                                                                                                                    </tr>
                                                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </LayoutTemplate>
                                                                                                            <ItemTemplate>
                                                                                                                <tr id="trUTaskDetails" runat="server" class="ClsGridRow">
                                                                                                                    <td align="left" style="width: 200px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblTaskName" runat="server" Text='<%#Eval("TaskName")%>' CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 200px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("TaskAssignerName")%>' CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 250px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblStartDateTime" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 250px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblEndDateTime" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </ItemTemplate>
                                                                                                            <AlternatingItemTemplate>
                                                                                                                <tr id="trUTaskDetails" runat="server" class="ClsGridRow">
                                                                                                                    <td align="left" style="width: 200px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblTaskName" runat="server" Text='<%#Eval("TaskName")%>' CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 200px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("TaskAssignerName")%>' CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 250px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblStartDateTime" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 250px; padding-left: 8px">
                                                                                                                        <asp:Label ID="lblEndDateTime" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </AlternatingItemTemplate>
                                                                                                            <EmptyDataTemplate>
                                                                                                                <table width="100%">
                                                                                                                    <tr>
                                                                                                                        <td class="LblNoRecord" align="center">
                                                                                                                            No Records Found.
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </EmptyDataTemplate>
                                                                                                        </asp:ListView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                                                                                        Text="Cancel"  OnClick="BtnCancelTask_Click" BorderWidth="1px"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </AlternatingItemTemplate>
                                                                        </asp:ListView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trNoRecordMsg" runat="server" visible="false">
                                                    <td style="height: 10px;" align="center" colspan="4">
                                                        <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                                            Text="No Record Found." EnableViewState="False" Width="70%"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:WizardStep>
                                        <asp:WizardStep ID="WizardStep3" runat="server" StepType="Finish" Title="Step 3">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" class="ClsBorderlight" style="width: 125px">
                                                        <span class="ClsLabel">Status :</span>
                                                    </td>
                                                    <td align="left" colspan="6" style="padding-left: 5px">
                                                        <asp:DropDownList ID="cmbStatus" CssClass="MidCombo" runat="server">
                                                        </asp:DropDownList><span class="ClsMdtStar">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight" style="width: 125px">
                                                        <span class="ClsLabel">Comment :</span>
                                                    </td>
                                                    <td align="left" colspan="6" style="padding-left: 5px">
                                                        <asp:TextBox ID="txtComment" CssClass="LrgTxtBox" runat="server" Width="520px" TextMode="MultiLine"
                                                            MaxLength="500"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight" style="width: 125px">
                                                        <span class="ClsLabel">Comment History :</span>
                                                    </td>
                                                    <td align="left" colspan="6" style="padding-left: 5px">
                                                        <asp:TextBox ID="txtCommentDetails" ReadOnly="true" Height="100px" CssClass="LrgTxtBox"
                                                            runat="server" Width="520px" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:WizardStep>
                                    </WizardSteps>
                                    <StartNavigationTemplate>
                                        <asp:Button ID="StartNextButton" runat="server" ValidationGroup="TaskStartStep" CausesValidation="True"
                                            CommandName="MoveNext" CssClass="ClsBtnMid" Text="Next" />&nbsp;
                                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="ClsBtnMid" Text="Cancel" />
                                    </StartNavigationTemplate>
                                    <StepNavigationTemplate>
                                        <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                            CssClass="ClsBtnMid" Text="Previous" />
                                        <asp:Button ID="StepNextButton" runat="server" ValidationGroup="TaskNavigateStep"
                                            CausesValidation="True" CommandName="MoveNext" CssClass="ClsBtnMid" Text="Next" />
                                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="ClsBtnMid" Text="Cancel" />
                                    </StepNavigationTemplate>
                                    <FinishNavigationTemplate>
                                        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                            CssClass="ClsBtnMid" Text="Previous" />
                                        <asp:Button ID="FinishButton" runat="server" ValidationGroup="TaskFinishStep" CommandName="MoveComplete"
                                            CssClass="ClsBtnMid" Text="Finish" CausesValidation="True" />
                                        <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="ClsBtnMid" Text="Cancel" />
                                    </FinishNavigationTemplate>
                                    <StepStyle ForeColor="#333333" />
                                    <SideBarStyle BackColor="#507CD1" VerticalAlign="Top" />
                                    <NavigationButtonStyle CssClass="ClsBtnMid" />
                                    <SideBarButtonStyle BackColor="#507CD1" Font-Names="Verdana" ForeColor="White" />
                                    <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                                        Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                </asp:Wizard>
                                <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="height20" style="width: 500px; height: 16px;">
                                <asp:CustomValidator ID="cstStartEndDateValidation" runat="server" SetFocusOnError="True"
                                    Display="None" ClientValidationFunction="IsStartEndDateValid" ValidationGroup="TaskStartStep"></asp:CustomValidator>
                                <asp:CustomValidator ID="cstTimeRangeValidation" runat="server" SetFocusOnError="True"
                                    Display="None" ClientValidationFunction="IsValidTimeRange" ValidationGroup="TaskStartStep"></asp:CustomValidator>
                                <asp:CustomValidator ID="cstValidateBufferDate" runat="server" SetFocusOnError="True"
                                    Display="None" ClientValidationFunction="ValidateBufferDate" ValidationGroup="TaskStartStep"></asp:CustomValidator>
                                <asp:CustomValidator ID="cstBufferDate" runat="server" SetFocusOnError="True" Display="None"
                                    ClientValidationFunction="BufferDateValidation" ValidationGroup="TaskStartStep"></asp:CustomValidator>
                                <asp:CustomValidator ID="cstBufferTime" runat="server" SetFocusOnError="True" Display="None"
                                    ClientValidationFunction="ValidateBufferTime" ValidationGroup="TaskStartStep"></asp:CustomValidator>
                                <asp:CustomValidator ID="cstValidateStatus" runat="server" SetFocusOnError="True"
                                    Display="None" ClientValidationFunction="ValidateStatus" ValidationGroup="TaskFinishStep"></asp:CustomValidator>
                                <asp:CustomValidator ID="cstEditedStartEndDate" runat="server" SetFocusOnError="True"
                                    Display="None" ClientValidationFunction="ValidateEditedStartEndDate" ValidationGroup="TaskStartStep"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidTaskCompletedDate" runat="server" />
                    <asp:HiddenField ID="hidTaskCompletedCount" runat="server" Value="0" />
                    <asp:HiddenField ID="hidTaskCompletedStTime" runat="server" />
                    <asp:HiddenField ID="hidTaskCompletedEndTime" runat="server" />
                    <asp:HiddenField ID="hidStartDate" runat="server" />
                    <asp:HiddenField ID="hidEndDate" runat="server" />
                    <asp:HiddenField ID="hidStTime" runat="server" />
                    <asp:HiddenField ID="hidEndTime" runat="server" />
                    <asp:HiddenField ID="hidFlag" runat="server" Value="1"/>                    
                    <asp:HiddenField ID="hidTaskDetailId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidTaskId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidTaskName" runat="server" Value="" />
                    <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidRowCount" runat="server" Value="0" />
                    <asp:HiddenField ID="hidTaskAssignerUserId" runat="server" Value="0" />
                    <%--<asp:HiddenField ID="hidAssignedToUserId" runat="server" Value="0" />--%>
                    <asp:HiddenField ID="hidComment" runat="server" Value="" />
                    <asp:HiddenField ID="hidCommentDetails" runat="server" Value="" />
                    <asp:HiddenField ID="hidStatusId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidTaskTypeId" runat="server" Value="0" />                    
                    
                    <%-- </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="FinishButton" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
    </div>

    <script language="JavaScript" type="text/javascript">
        function CloseWindow() {
            window.close()
            window.opener.location.reload(true)
            window.opener.focus()
        }
        _clientListViewId = "<%=this.lstvwUserDetails.ClientID %>"
        _ClientChkAll = _clientListViewId + "_ChkSelectAll";
        _ClientChkSelect = _clientListViewId + "_ChkSelect";

        _ClienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _ClienthidTaskTypeId = "<%=this.hidTaskTypeId.ClientID %>"
        _ClienthidStartDate = "<%=this.hidStartDate.ClientID %>"
        _ClienthidEndDate = "<%=this.hidEndDate.ClientID %>"


        _ClienthidTaskCompletedDate = "<%=this.hidTaskCompletedDate.ClientID %>"
        _ClienthidTaskCompletedStTime = "<%=this.hidTaskCompletedStTime.ClientID %>"
        _ClienthidTaskCompletedEndTime = "<%=this.hidTaskCompletedEndTime.ClientID %>"
        _ClienthidTaskCompletedCount = "<%=this.hidTaskCompletedCount.ClientID %>"
        _ClienthidStartDate = "<%=this.hidStartDate.ClientID %>"
        _ClienthidEndDate = "<%=this.hidEndDate.ClientID %>"
        _ClienthidStartTime = "<%=this.hidStTime.ClientID %>"
        _ClienthidEndTime = "<%=this.hidEndTime.ClientID %>"



        _ClientcmbStatus = "<%=this.cmbStatus.ClientID%>";
        _ClienttxtStartDate = "<%=this.txtStartDate.ClientID%>";
        _ClienttxtEndDate = "<%=this.txtEndDate.ClientID%>";
        _ClienttxtBufferDate = "<%=this.txtBufferDate.ClientID%>";

        _ClienttxtStartTime = "<%=this.txtStartTime.ClientID%>";
        _ClienttxtEndTime = "<%=this.txtEndTime.ClientID%>";
        _ClienttxtBufferTime = "<%=this.txtBufferTime.ClientID%>";

        _ClientoptDailyTask = "<%=this.optDailyTask.ClientID%>";
        _ClientoptNormalTask = "<%=this.optNormalTask.ClientID%>";
        _ClientoptGeneralTask = "<%=this.optGeneralTask.ClientID%>";

        _ClientcstStartEndDateValidation = "<%=this.cstStartEndDateValidation.ClientID%>";
        _ClientcstTimeRangeValidation = "<%=this.cstTimeRangeValidation.ClientID%>";
        _ClientcstValidateBufferDate = "<%=this.cstValidateBufferDate.ClientID%>";
        _CilentcstCheckAtLeastOne = "<%=this.cstCheckAtLeastOne.ClientID%>";
        _ClientcstBufferDate = "<%=this.cstBufferDate.ClientID%>";
        _ClientcstBufferTime = "<%=this.cstBufferTime.ClientID%>";
        _ClientcstEditedStartEndDate = "<%=this.cstEditedStartEndDate.ClientID%>";

        _ClientchkIncludeMe = "<%=this.chkIncludeMe.ClientID%>";


        function CheckUncheckRadioBtn() {
            //var chkOpt1 = document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_optIsFinal");
            var OptDaily = document.getElementById(_ClientoptDailyTask);
            var OptNormal = document.getElementById(_ClientoptNormalTask);
            var optGeneral = document.getElementById(_ClientoptGeneralTask);
            if (OptDaily.checked) {
                OptNormal.checked = false;
                optGeneral.checked = false;
            }
            else if (OptNormal.checked) {
                OptDaily.checked = false;
                optGeneral.checked = false;
            }
            else
                optGeneral.checked = true;
        }
        //window.opener.location = window.opener.location.pathname + queystring;
        //            window.close();
        //            window.opener.focus();
        //            return false;
        function CheckAllUncheckAlls() {


            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var enble
            var iRowCount = 0
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            }

        }
        function CheckAtLeastOneStatus() {
            if (document.getElementById(_ClientcmbStatus).value == 0) {
                alert("Status should be selected.");
                return false;
            }
            return true;
        }
        function CheckAtLeastOne() {            

            var iCount = 0;
            var sMsg = "";
            var iRowCount = 0;
            var sChk = document.getElementById(_ClientchkIncludeMe).checked;
            iRowCount = document.getElementById(_ClienthidRowCount).value;
            if (sChk) {
                iCount = iCount + 1;
            }
            for (var RowNumber = 0; RowNumber < iRowCount; RowNumber++) {
                chk = document.getElementById(_clientListViewId + "_ctrl" + RowNumber + "_ChkSelect");
                if (chk.checked == true) {
                    iCount = iCount + 1;
                }

            }
            if (iCount == 0) {
                alert("At least one user should be selected for assigning task.");
                return false;
            }
            return true;
        }

        function ValidateStatus(oSrc, args) {

            var iCount = 0;
            var sMsg = "";
            if (document.getElementById(_ClientcmbStatus).value == 0) {
                //                oSrc.errormessage = "Status should be selected.";
                //                document.getElementById(_CilentcstCheckAtLeastOne).errormessage = "Status should be selected.";
                //                args.IsValid = false;
                //                return true;
            }
            args.IsValid = true;
            return false;
        }

        function IsValidStartTime(oSrc, args) {

            if (document.getElementById(_ClienttxtStartTime).value != '') {
                if (!isTimeValid(_ClienttxtStartTime)) {
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }
        function IsValidEndTime(oSrc, args) {

            if (document.getElementById(_ClienttxtEndTime).value != '') {
                if (!isTimeValid(_ClienttxtEndTime)) {
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }
        function IsValidBufferTime(oSrc, args) {

            if (document.getElementById(_ClienttxtBufferTime).value != '') {
                if (!isTimeValid(_ClienttxtBufferTime)) {
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }
        function isTimeValid(result) {

            var timeStr = document.getElementById(result).value;
            if (trimAll(timeStr) == '')
                return false;

            var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            second = matchArray[4];
            ampm = matchArray[6];

            if (second == "") { second = null; }
            if (ampm == "") { ampm = null; }

            if (hour < 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            if (second != null && (second < 0 || second > 59))
                return false;

            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + '0' + minute;
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(result).value = str;
            return true;
        }
        function IsStartEndDateValid(oSrc, args) {
            var StartDt = ""; var EndDt = ""; var BufferDt = ""

            var sStrtDate = document.getElementById(_ClienttxtStartDate).value
            var sEndDate = document.getElementById(_ClienttxtEndDate).value
            var sBufferDate = document.getElementById(_ClienttxtBufferDate).value
            if (sStrtDate != "" && sEndDate != "") {
                if (convertvaliddate(sStrtDate) > convertvaliddate(sEndDate)) {

                    oSrc.errormessage = "End date should greater than Start date.";
                    document.getElementById(_ClientcstStartEndDateValidation).errormessage = "End date should greater than Start date.";
                    args.IsValid = false;
                    return true;
                }
            }
        }
        function ValidateBufferDate(oSrc, args) {
            var sStrtDate = document.getElementById(_ClienttxtStartDate).value
            var sEndDate = document.getElementById(_ClienttxtEndDate).value
            var sBufferDate = document.getElementById(_ClienttxtBufferDate).value
            if (sStrtDate != "" && sEndDate != "" && sBufferDate != "") {
                //                if (convertvaliddate(sStrtDate) > convertvaliddate(sStrtDate)) {
                //                    oSrc.errormessage = "Buffer Date should be greater than Start Date.";
                //                    document.getElementById(_ClientcstValidateBufferDate).errormessage = "Buffer Date suould be greater than Start Time and End Date.";
                //                    args.IsValid = false;
                //                    return true;
                //                }

                if (convertvaliddate(sEndDate) > convertvaliddate(sBufferDate)) {
                    oSrc.errormessage = "Buffer Date should be greater than End Date.";
                    document.getElementById(_ClientcstValidateBufferDate).errormessage = "Buffer Date should be greater than End Date.";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }
        function ValidateBufferTime(oSrc, args) {
            var sStrtDate = document.getElementById(_ClienttxtStartDate).value
            var sEndDate = document.getElementById(_ClienttxtEndDate).value
            var sBufferDate = document.getElementById(_ClienttxtBufferDate).value

            var sStrtTime = document.getElementById(_ClienttxtStartTime).value
            var sEndTime = document.getElementById(_ClienttxtEndTime).value
            var sBufferTime = document.getElementById(_ClienttxtBufferTime).value
            if (sStrtDate != "" && sEndDate != "" && sBufferDate != "") {

                if (convertvaliddate(sEndDate) == convertvaliddate(sBufferDate)) {
                    if (new Date(convertdate(sEndDate + " " + sEndTime)) > new Date(convertdate(sBufferDate + " " + sBufferTime))) {
                        oSrc.errormessage = "Buffer Time should be greater than End Time.";
                        document.getElementById(_ClientcstBufferTime).errormessage = "Buffer Time should be greater than End Time.";
                        args.IsValid = false;
                        return true;
                    }
                }
            }

            args.IsValid = true;
            return false;
        }
        function IsValidTimeRange(oSrc, args) {

            var StartDt = ""; var EndDt = ""; var BufferDt = "";

            var sStrtDate = document.getElementById(_ClienttxtStartDate).value
            var sEndDate = document.getElementById(_ClienttxtEndDate).value
            var sBufferDate = document.getElementById(_ClienttxtBufferDate).value

            var sStrtTime = document.getElementById(_ClienttxtStartTime).value
            var sEndTime = document.getElementById(_ClienttxtEndTime).value
            var sBufferTime = document.getElementById(_ClienttxtBufferTime).value

            if (sStrtDate != "" && sEndDate != "") {
                if (document.all) {
                    StartDt = new Date(sStrtDate.replace('-', ' '));
                    EndDt = new Date(sEndDate.replace('-', ' '));
                }
                else {
                    StartDt = new Date(convertdate(sStrtDate));
                    EndDt = new Date(convertdate(sEndDate));
                }

                if (convertvaliddate(sStrtDate) == convertvaliddate(sEndDate)) {
                    if (new Date(convertdate(sStrtDate + " " + sStrtTime)) > new Date(convertdate(sEndDate + " " + sEndTime))) {
                        oSrc.errormessage = "End Time should be greater than Start Time.";
                        document.getElementById(_ClientcstTimeRangeValidation).errormessage = "End Time should be greater than Start Time.";
                        args.IsValid = false;
                        return true;
                    }
                }

            }
            if (document.getElementById(_ClienttxtStartTime).value != '') {
                if (!isTimeValid(_ClienttxtStartTime)) {
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }
        function BufferDateValidation(oSrc, args) {
            if (document.getElementById(_ClienttxtBufferTime).value != '') {
                if (document.getElementById(_ClienttxtBufferDate).value == '') {
                    oSrc.errormessage = 'Buffer Date should not be blank.';
                    document.getElementById(_ClientcstBufferDate).errormessage = 'Buffer Date should not be blank.';
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateEditedStartEndDate(oSrc, args) {            

            var hidTaskCompletedDate = document.getElementById(_ClienthidTaskCompletedDate).value;
            var hidTaskCompletedStTime = document.getElementById(_ClienthidTaskCompletedStTime).value;
            var hidTaskCompletedEndTime = document.getElementById(_ClienthidTaskCompletedEndTime).value;


            var hidTaskCompletedCount = document.getElementById(_ClienthidTaskCompletedCount).value;
            var txtStartDt = document.getElementById(_ClienttxtStartDate).value;
            var txtEndDt = document.getElementById(_ClienttxtEndDate).value;
            var txtStartTm = document.getElementById(_ClienttxtStartTime).value;
            var txtEndTm = document.getElementById(_ClienttxtEndTime).value;
            var hidStDate = document.getElementById(_ClienthidStartDate).value;
            var hidEndDate = document.getElementById(_ClienthidEndDate).value;
            var hidStTime = document.getElementById(_ClienthidStartTime).value;
            var hidEndTime = document.getElementById(_ClienthidEndTime).value;

            /////DAILY Task
            if (document.getElementById(_ClienthidTaskTypeId).value == '3') {
                ////This task already completed by some user.
                if (hidTaskCompletedCount != '0') {
                    ////If task date and edited start date is not same
                    //if (new Date(convertdate(hidStDate + " " + hidStTime)) != new Date(convertdate(txtStartDt + " " + txtStartTm))) {
                    //if ((convertdate(hidStDate) != convertdate(txtStartDt)) 
                    if (convertdate(hidStDate + " " + hidStTime) != convertdate(txtStartDt + " " + txtStartTm)
                    || convertdate(hidEndDate + " " + hidEndTime) != convertdate(txtEndDt + " " + txtEndTm)) {
                        //|| (convertdate(hidEndDate) != convertdate(txtEndDt))) {
                        oSrc.errormessage = 'Start Date and End Date can not be changed. Since, this task is already done by user(s).';
                        document.getElementById(_ClientcstEditedStartEndDate).errormessage = 'Start Date and End Date can not be changed. Since, this task is already done by user(s).';
                        args.IsValid = false;
                        return true

                    }

                }
            }
            else if (document.getElementById(_ClienthidTaskTypeId).value == '1') {

                ////This task already completed by some user.
                if (hidTaskCompletedCount != '0') {
                    ////If task date and edited start date is not same
                    if (convertdate(hidStDate + " " + hidStTime) != convertdate(txtStartDt + " " + txtStartTm)) {
                        //if (convertdate(hidStDate) != convertdate(txtStartDt)) {
                        oSrc.errormessage = 'Start Date can not be changed. Since, this task is already done by user(s).';
                        document.getElementById(_ClientcstEditedStartEndDate).errormessage = 'Start Date can not be changed. Since, this task is already done by user(s).';
                        args.IsValid = false;
                        return true
                    }
                    ////If task start date and edited end date is not same
                    //                    if (convertdate(hidEndDate) != convertdate(txtEndDt)) {
                    //                        if (convertdate(hidEndDate) > convertdate(txtEndDt)) {
                    if (convertdate(hidEndDate + " " + hidEndTime) != convertdate(txtStartDt + " " + txtEndTm)) {
                        if (convertdate(hidEndDate + " " + hidEndTime) > convertdate(txtStartDt + " " + txtEndTm)) {
                            ////This task already completed by some user.
                            if (hidTaskCompletedCount != '0') {
                                var dtTaskCompletedDate = getDateString1(convertdate(hidTaskCompletedDate));
                                ////iF Existind task completed ebd date is greater than edited end date
                                // if (convertdate(hidTaskCompletedDate) > convertdate(txtEndDt)) {
                                if (convertdate(hidTaskCompletedDate + " " + hidTaskCompletedEndTime) > convertdate(txtEndDt + " " + txtEndTm)) {
                                    oSrc.errormessage = "End Date should be greater than " + dtTaskCompletedDate + ".";
                                    document.getElementById(_ClientcstEditedStartEndDate).errormessage = "End Date should be greater than " + dtTaskCompletedDate + ".";
                                    args.IsValid = false;
                                    return true
                                }
                            }
                        }
                    }
                }
            }
            else {
                args.IsValid = true;
                return false;
            }

        }


        function getDateString1(oDtobj) {
            var obj = new Date(oDtobj);
            var strDate = obj.getDate() + "-";
            var strMonth = parseInt(obj.getMonth()) + 1;
            strMonth = getMonthName1(strMonth);
            strDate = strDate + strMonth + "-";
            strDate = strDate + obj.getFullYear();
            return strDate;
        }

        function getMonthName1(month) {
            switch (month) {
                case 1:
                    return "Jan";
                    break;

                case 2:
                    return "Feb";
                    break;

                case 3:
                    return "Mar";
                    break;

                case 4:
                    return "Apr";
                    break;

                case 5:
                    return "May";
                    break;

                case 6:
                    return "Jun";
                    break;

                case 7:
                    return "Jul";
                    break;

                case 8:
                    return "Aug";
                    break;

                case 9:
                    return "Sep";
                    break;

                case 10:
                    return "Oct";
                    break;

                case 11:
                    return "Nov";
                    break;

                case 12:
                    return "Dec";
                    break;

            }
        }
        function CloseWindow(queystring) {
            window.opener.location = window.opener.location.pathname + queystring;
            window.close();
            window.opener.focus();
            return false;
        }
    </script>

</asp:Content>
