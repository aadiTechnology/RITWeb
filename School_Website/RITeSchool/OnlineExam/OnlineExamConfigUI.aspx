<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OnlineExamConfigUI.aspx.cs" Inherits="OnlineExamConfigUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
<script type="text/javascript" src="https://polyfill.io/v3/polyfill.min.js?features=es6"></script>
<script id="MathJax-script" type="text/javascript" async src="https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js">
</script>
    <table align="center" style="width: 95%">
        <tr>
            <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
               <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <asp:ValidationSummary ID="valSumTaskDetails" CssClass="LblErrorMsg" ShowSummary="true"
                            runat="server" />
                        <asp:RequiredFieldValidator ID="reqStandard" runat="server" ControlToValidate="cmbStandard"
                            InitialValue="0" Display="None" ErrorMessage="Standard Should be Selected."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="ReqSubject" runat="server" ControlToValidate="cmbSubject"
                            InitialValue="0" Display="None" ErrorMessage="Subject Should be selected."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqExam" runat="server" ControlToValidate="cmbExam"
                            InitialValue="0" Display="None" ErrorMessage="Exam should  be selected."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqNoOfQuestions" runat="server" ControlToValidate="txtNoOfQuestions"
                            Display="None" ErrorMessage="Number of questions should not be blank."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="ReqStartDate" runat="server" ControlToValidate="txtStartDate"
                            Display="None" ErrorMessage="Start Date should not be blank."></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cstInvaliStartTime" runat="server" SetFocusOnError="True"
                            Display="None" ErrorMessage="" ClientValidationFunction="IsValidStartTime"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="ReqEndDate" runat="server" ControlToValidate="txtEndDate"
                            Display="None" ErrorMessage="End Date should not be blank."></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cstInvaliEndTime" runat="server" SetFocusOnError="True"
                            Display="None" ErrorMessage="" ClientValidationFunction="IsValidEndTime"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstTimeRangeValidation" runat="server" SetFocusOnError="True"
                            ErrorMessage="End time should be greater than start time." Display="None" ClientValidationFunction="IsValidTimeRange"></asp:CustomValidator>
                       <%-- <asp:CustomValidator ID="cstStartEndDateValidation" runat="server" SetFocusOnError="True"
                            Display="None" ClientValidationFunction="IsStartEndDateValid"></asp:CustomValidator>--%>
                        <asp:CustomValidator ID="CstStaff" runat="server" ClientValidationFunction="CheckAtListOne"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="CheckAtListOne"
                            Display="None" ErrorMessage="" SetFocusOnError="True"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" SetFocusOnError="True"
                            Display="None" ErrorMessage="" ClientValidationFunction="ValidateExam"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" SetFocusOnError="True"
                            Display="None" ErrorMessage="" ClientValidationFunction="ValidateQuestionCount"></asp:CustomValidator>
                        <asp:HiddenField ID="hidExamConfigId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                         <asp:HiddenField ID="hidIsConfigured" runat="server" Value="N" />
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwQuestions" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUnsubmit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td>
               <%-- <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                            ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwQuestions" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUnsubmit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr align="center">
            <td align="center">
                <table style="vertical-align: top;" align="center">
                    <tr class="ClspaddingL">
                        <td class="ClsBorderlight" style="width: 110px;">
                            <span id="Span9" class="paddingLSML">Standard :</span>
                        </td>
                        <td align="left" style="padding-left: 5px;">
                            <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">*</span>
                        </td>
                        <td class="ClsBorderlight" style="width: 110px;">
                            <span id="Span5" class="paddingLSML">Division :</span>
                        </td>
                        <td align="left" style="padding-left: 5px;">
                            <div>
                                <%--<asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <asp:DropDownList ID="cmbClass" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                            OnSelectedIndexChanged="cmbClass_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </div>
                        </td>
                        <td class="ClsBorderlight" style="width: 110px;">
                            <span id="Span3" class="paddingLSML">Subject :</span>
                        </td>
                        <td align="left" style="padding-left: 5px;">
                            <div>
                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <asp:DropDownList ID="cmbSubject" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                            OnSelectedIndexChanged="cmbSubject_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <hr style="border: 1px solid gray;" />
            </td>
        </tr>
        <tr align="center">
            <td align="center">
                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <table style="vertical-align: top;" align="center">
                            <tr class="ClspaddingL">
                                <td class="ClsBorderlight" style="width: 150px;">
                                    <span id="Span7" class="paddingLSML">Exam :</span>
                                </td>
                                <td align="left" style="padding-left: 5px;">
                                    <div>
                                        <asp:DropDownList ID="cmbExam" runat="server" CssClass="LrgCombo" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </div>
                                </td>
                                <td class="ClsBorderlight" style="width: 150px;">
                                    <span id="Span8" class="paddingLSML">No. Of Questions :</span>
                                </td>
                                <td align="left" style="padding-left: 5px; width: 250px;">
                                    <div>
                                        <asp:TextBox ID="txtNoOfQuestions" runat="server" CssClass="MidTxtBox" MaxLength="4" />
                                        <span class="ClsMdtStar">*</span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight">
                                    <span id="Span1" class="paddingLSML">Start Date/Time :</span>
                                </td>
                                <td style="padding-left: 5px;">
                                    <asp:TextBox ID="txtStartDate" CssClass="SmlTxtBox" runat="server" 
                                        ReadOnly="true"></asp:TextBox>
                                    <rjs:PopCalendar ID="CalDtPopup" runat="server" Control="txtStartDate" From-Date=""
                                        Culture="en" ShowErrorMessage="False" From-Today="True" Format="dd mmm yyyy" />
                                    <asp:TextBox ID="txtExamStartTime" CssClass="MidTxtBox" runat="server" 
                                        Width="75px" placeholder="10:00 AM">
                                    </asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                                </td>
                                <td class="ClsBorderlight">
                                    <span id="Span2" class="paddingLSML">End Date/Time :</span>
                                </td>
                                <td style="padding-left: 5px;">
                                    <asp:TextBox ID="txtEndDate" CssClass="SmlTxtBox" runat="server" 
                                        ReadOnly="true"></asp:TextBox>
                                    <rjs:PopCalendar ID="CalEndDtPopup" runat="server" Control="txtEndDate" From-Date=""
                                        Culture="en" ShowErrorMessage="False" From-Today="True" Format="dd mmm yyyy" />
                                    <asp:TextBox ID="txtExamEndTime" CssClass="MidTxtBox" runat="server" 
                                        Width="75px" placeholder="10:00 AM">
                                    </asp:TextBox><span class="ClsMdtStar">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight">
                                    <span id="Span6" runat="server" class="paddingLSML">Shuffle For Count? : </span>
                                </td>
                                <td style="padding-left: 5px;">
                                    <asp:CheckBox ID="chkSuffleForCount" runat="server" Checked="false" onclick="SetSuffleState(this)" />
                                </td>
                                <td class="ClsBorderlight">
                                    <span id="Span4" class="paddingLSML">Shuffle For Sequence? :</span>
                                </td>
                                <td style="padding-left: 5px;">
                                    <asp:CheckBox ID="chkShuffleForSequence" runat="server" Checked="false" />
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" colspan="4">
                                    <asp:ListView ID="lstvwExamQuestionConfiguration" runat="server" OnItemDataBound="lstvwExamQuestionConfiguration_ItemDataBound"
                                        DataKeyNames="Id,QuestionId">
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblTermInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="center" id="chkAll" style="width: 25px" runat="server">
                                                        <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                    </th>
                                                    <th align="left" class="paddingL">
                                                        <asp:Label ID="lblQuestion" runat="server" Text="Question"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trData" runat="server" class="ClsGridRow">
                                                <td align="center">
                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label6" runat="server" Text='<%#Eval("Question")%>'> </asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="trData" runat="server" class="ClsGridAltRow">
                                                <td align="center">
                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label8" runat="server" Text='<%#Eval("Question")%>'> </asp:Label>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="LblNoRecord" style="margin: 10px 0; width: 900px;">
                                                No record found.</div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwQuestions" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td align="center">
                <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <asp:Button CssClass="ClsBtn" ID="btnSave" CausesValidation="true" runat="server"
                            Text="Save" OnClick="btnSave_Click" />
                        <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="False" UseSubmitBehavior="false"
                            runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwQuestions" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td align="center">
               <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <table width="80%">
                            <tr id="trDataPager" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwQuestions"
                                        Visible="true">
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
                            <tr style="width: 90%">
                                <td style="width: 100%">
                                    <asp:ListView ID="lstvwQuestions" runat="server" OnSorting="lstvwQuestions_Sorting"
                                        DataSourceID="ObjDSVehicleStaffDetails" DataKeyNames="Id" OnItemDataBound="lstvwQuestions_ItemDataBound"
                                        OnDataBound="lstvwQuestions_DataBound" OnItemCommand="lstvwQuestions_ItemCommand">
                                        <LayoutTemplate>
                                            <table width="100%" runat="server" id="tblVehicleStaffInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" style="padding-left: 9px;" visible="false">
                                                        <asp:LinkButton ID="lnkBtnSortVehicleName" runat="server" CommandName="Sort" CommandArgument="VehicleNumber"
                                                            CausesValidation="false" ForeColor="Black"> Class </asp:LinkButton>
                                                    </th>
                                                    <th align="left" width="50%" style="padding-left: 9px;">
                                                        <asp:LinkButton ID="lnkBtnStaff" runat="server" CommandName="Sort" CommandArgument="Exam"
                                                            CausesValidation="false" ForeColor="Black"> Exam</asp:LinkButton>
                                                    </th>
                                                    <th align="left" width="20%" style="padding-left: 9px;" visible="false">
                                                        <asp:LinkButton ID="lnkbtnVehicleType" runat="server" CommandName="Sort" CommandArgument="Subject"
                                                            CausesValidation="false" ForeColor="Black"> Subject</asp:LinkButton>
                                                    </th>
                                                    <th align="center" width="300px">
                                                        <asp:LinkButton ID="lnkbtnStartDate" runat="server" CommandName="Sort" CommandArgument="StartDateAndTime"
                                                            CausesValidation="false" ForeColor="Black"> Start Date</asp:LinkButton>
                                                    </th>
                                                    <th align="center" width="300px">
                                                        <asp:LinkButton ID="lnkbtnEndDate" runat="server" CommandName="Sort" CommandArgument="EndDateAndTime"
                                                            CausesValidation="false" ForeColor="Black"> End Date</asp:LinkButton>
                                                    </th>
                                                    <th align="right" width="200px" style="padding-right: 5px;">
                                                        <asp:LinkButton ID="lnkNoOfQue" runat="server" CommandName="Sort" CommandArgument="NoOfQuestions"
                                                            CausesValidation="false" ForeColor="Black"> No. Of Questions</asp:LinkButton>
                                                    </th>
                                                    <th align="center" width="150px">
                                                        Is Submitted?
                                                    </th>
                                                    <th align="center" width="100px">
                                                        Edit
                                                    </th>
                                                    <th align="center" width="100px">
                                                        Delete
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager">
                                                    <td colspan="9">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwQuestions" PageSize="20"
                                                            Visible="true">
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
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td class="paddingL" align="left" visible="false">
                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("class") %>'></asp:Label>
                                                </td>
                                                <td class="paddingL" align="left">
                                                    <asp:Label ID="lblExam" runat="server" Text='<%# Eval("Exam") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidId" runat="server" Value = '<%# Eval("Id") %>' />
                                                </td>
                                                <td align="left" class="paddingL" visible="false">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblStartdate" runat="server" Text='<%# Eval("StartDateAndTime", "{0:dd-MMM-yyyy hh:mm tt}") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("EndDateAndTime" , "{0:dd-MMM-yyyy hh:mm tt}") %>'></asp:Label>
                                                </td>
                                                <td align="right" style="padding-right: 5px;">
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("NoOfQuestions") %>'></asp:Label>
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Image ID="imgSubmitted" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif" Visible= '<%# Eval("IsSubmitted") %>'/>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit %>" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete %>" runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td class="paddingL" align="left" visible="false">
                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("class") %>'></asp:Label>
                                                </td>
                                                <td class="paddingL" align="left">
                                                    <asp:Label ID="lblExam" runat="server" Text='<%# Eval("Exam") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidId" runat="server" Value = '<%# Eval("Id") %>' />
                                                </td>
                                                <td align="left" class="paddingL" visible="false">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblStartdate" runat="server" Text='<%# Eval("StartDateAndTime", "{0:dd-MMM-yyyy hh:mm tt}") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("EndDateAndTime", "{0:dd-MMM-yyyy hh:mm tt}") %>'></asp:Label>
                                                </td>
                                                <td align="right" style="padding-right: 5px;">
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("NoOfQuestions") %>'></asp:Label>
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Image ID="imgSubmitted" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif" Visible= '<%# Eval("IsSubmitted") %>'/>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit %>" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete %>" runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="LblNoRecord" style="text-align: center">
                                                No record found.</div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:ObjectDataSource ID="ObjDSVehicleStaffDetails" runat="server" EnableCaching="False"
                                        EnablePaging="True" SelectCountMethod="CountTotalExamQuestionConfiguration" SelectMethod="GetAllExamQuestionConfiguration"
                                        SortParameterName="sortExpression" TypeName="BusinessLogic.OnlineExamConfigurationBL">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                Type="int32" />
                                            <asp:ControlParameter ControlID="hidSortExpression" Name="sortExpression" Type="String"
                                                PropertyName="Value" />
                                            <asp:ControlParameter ControlID="hidSortDirection" Name="sortDirection" Type="String"
                                                PropertyName="Value" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            <asp:ControlParameter ControlID="cmbStandard" Name="aiStandardId" PropertyName="SelectedValue"
                                                Type="int32" />
                                            <asp:ControlParameter ControlID="cmbClass" Name="aiStandardDivisionId" PropertyName="SelectedValue"
                                                Type="int32" />
                                            <asp:ControlParameter ControlID="cmbSubject" Name="aiSubjectId" PropertyName="SelectedValue"
                                                Type="int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwQuestions" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUnsubmit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
      <%--  <tr style="display: none">
            <td>
                <table width="60%">
                    <tr id="tr2" runat="server" visible="true">
                        <td align="right" class="ClsBorderlight" valign="middle">
                            <span class="LblRht colonPadding"></span>
                            <asp:Label ID="Label2" runat="server" CssClass="LblRht" EnableViewState="False" Text="Applicable to selected Class(es) :"></asp:Label>
                            <br />
                            <asp:CheckBox ID="chkAllDivForVdo" runat="server" onclick="CheckAll1(this);" Style="padding-right: 5px"
                                TabIndex="7" Text="<%$ Resources:LocalizedResources, SelectAll%>" />
                        </td>
                        <td align="left">
                            <asp:ListView ID="lstvwVideoStandardDivision" runat="server" DataKeyNames="StandardId"
                                OnItemDataBound="lstvwVideoStandardDivision_ItemDataBound">
                                <LayoutTemplate>
                                    <table id="tblStaffInfo" runat="server" align="left" cellpadding="0" cellspacing="1"
                                        class="GridBorder" style="color: #333333;" width="auto">
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                        <td align="left" style="padding-left: 5px">
                                            <asp:CheckBox ID="chkVdoStandard" runat="server" Text='<%# Eval("StandardName") %>' />
                                        </td>
                                        <td align="left" style="padding-left: 5px">
                                            <asp:CheckBoxList ID="chkvideoStandardDivLst" runat="server" CssClass="ClsLabel"
                                                RepeatColumns="6" RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow" style="height: 10px">
                                        <td align="left" style="padding-left: 5px">
                                            <asp:CheckBox ID="chkVdoStandard" runat="server" Text='<%# Eval("StandardName") %>' />
                                        </td>
                                        <td align="left" style="padding-left: 5px">
                                            <asp:CheckBoxList ID="chkvideoStandardDivLst" runat="server" CssClass="ClsLabel"
                                                RepeatColumns="6" RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <table width="50%">
                                        <tr>
                                            <td align="center" class="LblNoRecord">
                                                <asp:Label ID="lblNoRecord" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCopy" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                OnClick="btnCopy_Click" Text="Copy" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>--%>
        <tr>
            <td align="center">
                <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                            CausesValidation="False" UseSubmitBehavior="false" />
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" CausesValidation="false"
                            onclick="btnSubmit_Click" />
                        <asp:Button ID="btnUnsubmit" runat="server" Text="Un-Submit" CssClass="ClsBtn" CausesValidation="false"
                            onclick="btnUnsubmit_Click" />
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwQuestions" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUnsubmit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientlblUpdateSucess = "<%=this.lblUpdateMessage.ClientID %>"
        _clientlstvwExamQuestionConfig = "<%=this.lstvwExamQuestionConfiguration.ClientID %>"

        _ClientChkAll = _clientlstvwExamQuestionConfig + "_ChkSelectAll";
        _clientCstStaff = "<%=this.CstStaff.ClientID %>"
        _ClienttxtStartTime = "<%=this.txtExamStartTime.ClientID%>";
        _ClienttxtEndTime = "<%=this.txtExamEndTime.ClientID%>";
        _ClienttxtStartDate = "<%=this.txtStartDate.ClientID%>";
        _ClienttxtEndDate = "<%=this.txtEndDate.ClientID%>";
        
        _ClientcstInvaliEndTime = "<%=this.cstInvaliEndTime.ClientID %>";
        _clientchkShuffleForSequence = '<%=this.chkShuffleForSequence.ClientID %>'
        _clientcmbExam = "<%=this.cmbExam.ClientID %>"
        _clienthidExamConfigId = '<%=this.hidExamConfigId.ClientID %>'
        _clienttxtNoOfQuestions = '<%=this.txtNoOfQuestions.ClientID %>'

        function CheckAllUncheckAlls() {
            if (document.getElementById(_ClientChkAll) != null)
                var checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwExamQuestionConfig + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwExamQuestionConfig + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }


        function CheckAtListOne(oSrc, args) {
            var chk;
            var iRowCount = 0;
            var chkCount = 0;

            chk = document.getElementById(_clientlstvwExamQuestionConfig + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    chkCount = chkCount + 1;
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwExamQuestionConfig + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (chkCount == 0) {
                $get(_clientCstStaff).errormessage = "At least one  question should be selected ."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        function IsValidStartTime(oSrc, args) {
            if (document.getElementById(_ClienttxtStartTime).value == '') {
                oSrc.errormessage = "Start Time should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (document.getElementById(_ClienttxtStartTime).value != '') {
                if (!isTimeValid(_ClienttxtStartTime)) {
                    oSrc.errormessage = "Start Time should be in valid format.";
                    args.IsValid = false;
                    return true;
                }
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
        function IsValidEndTime(oSrc, args) {
            if (document.getElementById(_ClienttxtEndTime).value == '') {            
                oSrc.errormessage = "End Time should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (document.getElementById(_ClienttxtEndTime).value != '') {
                if (!isTimeValid1(_ClienttxtEndTime)) {
                    oSrc.errormessage = 'End Time should be in valid format.'
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }
        function isTimeValid1(result) {

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

//        function IsStartEndDateValid(oSrc, args) {

//            var StartDt = ""; var EndDt = "";

//            var sStrtDate = document.getElementById(_ClienttxtStartDate).value
//            var sEndDate = document.getElementById(_ClienttxtEndDate).value

//            if (sStrtDate != "" && sEndDate != "") {

//                if (sStrtDate > sEndDate) {
//                    oSrc.errormessage = "End date should greater than Start date.";
//                    document.getElementById(_ClientcstStartEndDateValidation).errormessage = "End date should be greater than Start date.";
//                    args.IsValid = false;
//                    return true;
//                }
//            }
//        }

        function GetHours(d) {
            var h = parseInt(d.split(':')[0]);
            if (d.split(':')[1].split(' ')[1] == "PM") {
                h = h + 12;
            }
            return h;
        }

        function GetMinutes(d) {
            return parseInt(d.split(':')[1].split(' ')[0]);
        }

        function IsValidTimeRange(oSrc, args) {
            var StartDt = ""; var EndDt = "";

            var sStrtDate = document.getElementById(_ClienttxtStartDate).value
            var sEndDate = document.getElementById(_ClienttxtEndDate).value


            var sStrtTime = document.getElementById(_ClienttxtStartTime).value
            var sEndTime = document.getElementById(_ClienttxtEndTime).value


            if (sStrtDate != "" && sEndDate != "" && sStrtTime != "" && sEndTime != "") {
                if (document.all) {
                    StartDt = new Date(sStrtDate.replace('-', ' '));
                    EndDt = new Date(sEndDate.replace('-', ' '));
                }
                else {
                    StartDt = new Date(convertdate(sStrtDate));
                    EndDt = new Date(convertdate(sEndDate));
                }
                var startTime = new Date().setHours(GetHours(sStrtTime), GetMinutes(sStrtTime), 0);
                var endTime = new Date(startTime)
                endTime = endTime.setHours(GetHours(sEndTime), GetMinutes(sEndTime), 0);

                if (new Date(convertdate(sStrtDate) + " " + sStrtTime) >= new Date(convertdate(sEndDate) + " " + sEndTime)) {

                    oSrc.errormessage = "End Date/Time should be greater than Start Date/Time.";
                    args.IsValid = false;
                    return true;
                }

            }

            args.IsValid = true;
            return false;
        }


        function SetSuffleState(obj) {        
            if (obj.checked) {
                $get(_clientchkShuffleForSequence).checked = true;
                $get(_clientchkShuffleForSequence).disabled = true;
            }
            else {
                $get(_clientchkShuffleForSequence).disabled = false;
                $get(_clientchkShuffleForSequence).checked = false;
            }
        }


        function ValidateExam(oSrc, args) {
            var data = new Array();
            var found = false;

            var examNew = $('#' + _clientcmbExam + ' option:selected').text().trim()
            var examId = $('#'+ _clienthidExamConfigId).val()

            $('[id$=lblExam]').each(function () {
                var exam = $(this).html().trim()

                var hidId = $('#'+ $(this)[0].id.replace('lblExam', 'hidId')).val()

                if (examNew == exam && examId != hidId) {
                    found = true;
                }

            });

            if (found) {
                oSrc.errormessage = "Exam should not be duplicate."
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateQuestionCount(oSrc, args) {
            var count = $('#' + _clienttxtNoOfQuestions).val()
            if (count != '') {
                if ($('[id$=ChkSelect]:checked').length < parseInt(count)) {
                    oSrc.errormessage = "Selected question's count should be greater than or equal to value set for 'No. of Questions'."
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
