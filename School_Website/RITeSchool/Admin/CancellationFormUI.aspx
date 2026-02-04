<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="CancellationFormUI.aspx.cs" Inherits="CancellationFormUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td align="right">
                <div style="float: right;" class="LblErrorMsg" id="lblMandatoryMark" runat="server"
                    viewstatemode="Enabled">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" Text="Mandatory Fields"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="Up1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="ValSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="tr1" runat="server">
            <td align="center">
                <table id="tblCancellationDetails" width="80%" runat="server">
                    <tr align="center">
                        <td align="center">
                            <table id="Table1" runat="server" width="98%">
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="Up2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblUpdate" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                                                    CssClass="clsLabel" Font-Bold="true"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr align="center" style="text-align: center; margin: 0px auto;">
                                    <td align="center" style="text-align: center;">
                                        <table align="center">
                                            <tr>
                                                <td class="ClsBorderLight" align="left">
                                                    <span class="ClsLabel">Student Name : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtnSearch" runat="server" Text="Search" class="ClsBtn" OnClick="BtnSearch_Click"
                                                        CausesValidation="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trlink" runat="server">
                                    <td>
                                        <table id="tbllststudent" align="center" width="90%" runat="server">
                                            <tr>
                                                <td align="center" class="width-99-percentage">
                                                    <asp:UpdatePanel ID="Up3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table align="center" width="100%">
                                                                <tr id="trDtPgCount" runat="server" visible="true">
                                                                    <td align="center">
                                                                        <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwSearchStudentDetails"
                                                                            PageSize="5">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1%>" />
                                                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                                        <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                                        <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                                        <br />
                                                                                    </PagerTemplate>
                                                                                </asp:TemplatePagerField>
                                                                            </Fields>
                                                                        </asp:DataPager>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPager" runat="server" widt="100%">
                                                                    <td align="center">
                                                                        <asp:ListView ID="lstvwSearchStudentDetails" runat="server" ViewStateMode="Enabled"
                                                                            DataKeyNames="SchoolWiseStudentId, Id" OnItemCommand="lstvwSearchStudentDetails_ItemCommand"
                                                                            OnDataBound="lstvwSearchStudentDetails_DataBound" OnSorting="lstvwSearchStudentDetails_Sorting">
                                                                            <LayoutTemplate>
                                                                                <table id="lstvwtable1" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                                                    class="GridBorder" width="80%">
                                                                                    <tr id="trheader" runat="server" class="ClsGridHeader">
                                                                                        <th align="center" class="paddingLR" width="80px">
                                                                                            <asp:Label ID="lblEnrolment" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                                                Text="Enrolment No"></asp:Label>
                                                                                        </th>
                                                                                        <th align="center" class="PaddingL" width="70px">
                                                                                            <asp:Label ID="lblRollNo" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                                                Text="Roll No"></asp:Label>
                                                                                        </th>
                                                                                        <th align="center" class="PaddingL" width="120px">
                                                                                            <asp:Label ID="lblClass" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                                                Text="Class Name"></asp:Label>
                                                                                        </th>
                                                                                        <th align="left" class="PaddingL">
                                                                                            <asp:Label ID="lblName" runat="server" Text="Student Name" Style="padding-left: 10px;"></asp:Label>
                                                                                        </th>
                                                                                        <th align="center" style="width: 60px;">
                                                                                            Select
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                    <tr id="trDataPager" class="ClsBorderPager">
                                                                                        <td colspan="10">
                                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwSearchStudentDetails"
                                                                                                PageSize="5">
                                                                                                <Fields>
                                                                                                    <asp:TemplatePagerField>
                                                                                                        <PagerTemplate>
                                                                                                            <table width="100%">
                                                                                                                <tr>
                                                                                                                    <td align="left">
                                                                                                                        <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
                                                                                                                        <asp:DropDownList ID="ddlCnt" ViewStateMode="Enabled" runat="server" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged"
                                                                                                                            AutoPostBack="true">
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
                                                                                <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                                                    <td align="left" class="paddingL">
                                                                                        <asp:Label ID="lblEnrolment1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("EnrolmentNumber") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td id="Td1" align="left" class="PaddingLR" runat="server">
                                                                                        <asp:Label ID="lblRollNo1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("RollNo") %>'>
                                                                                        </asp:Label>
                                                                                        <asp:HiddenField ID="hidData1" runat="server" Value="" />
                                                                                    </td>
                                                                                    <td id="Td2" align="center" class="PaddingL" runat="server">
                                                                                        <asp:Label ID="lblClass1" runat="server" Text='<%# Eval("ClassName") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td id="Td3" align="left" class="PaddingL" runat="server">
                                                                                        <asp:Label ID="lblName1" runat="server" Style="padding-right: 5px;" Text='<%# Eval("StudentName") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:LinkButton ID="LnkBtnSelect" runat="server" Text="Select" CausesValidation="false"
                                                                                            CommandName="SelectDetails"></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <tr id="trItemtemplate" runat="server" class="ClsGridAltRow">
                                                                                    <td align="left" class="paddingL">
                                                                                        <asp:Label ID="lblEnrolment1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("EnrolmentNumber") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td id="Td1" align="left" class="PaddingLR" runat="server">
                                                                                        <asp:Label ID="lblRollNo1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("RollNo") %>'>
                                                                                        </asp:Label>
                                                                                        <asp:HiddenField ID="hidData1" runat="server" Value="" />
                                                                                    </td>
                                                                                    <td id="Td2" align="center" class="PaddingL" runat="server">
                                                                                        <asp:Label ID="lblClass1" runat="server" Text='<%# Eval("ClassName") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td id="Td3" align="left" class="PaddingL" runat="server">
                                                                                        <asp:Label ID="lblName1" runat="server" Style="padding-right: 5px;" Text='<%# Eval("StudentName") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:LinkButton ID="LnkBtnSelect" runat="server" Text="Select" CausesValidation="false"
                                                                                            CommandName="SelectDetails"></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </AlternatingItemTemplate>
                                                                            <EmptyDataTemplate>
                                                                                <tr style="width: 800px">
                                                                                    <td align="center" class="LblNoRecord">
                                                                                        No record Found
                                                                                    </td>
                                                                                </tr>
                                                                            </EmptyDataTemplate>
                                                                        </asp:ListView>
                                                                        <asp:ObjectDataSource TypeName="BusinessLogic.CancellationFormBL" EnablePaging="true"
                                                                            ID="objdsSearchStudentDetails" runat="server" SelectMethod="GetAllSearchStudents"
                                                                            SortParameterName="SortExpression" SelectCountMethod="GetCountSearchStudent"
                                                                            EnableCaching="false">
                                                                            <SelectParameters>
                                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                                    Type="int32" />
                                                                                <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                                                <asp:Parameter Name="SortExpression" Type="String" />
                                                                                <asp:Parameter Name="SortDirection" Type="String" />
                                                                                <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                                                <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center">
            <td align="center">
                <table id="tbl" runat="server" width="98%">
                    <tr id="trControls" runat="server" style="text-align: center; margin: 0px auto;">
                        <td align="center" style="text-align: center;">
                            <asp:UpdatePanel ID="Up4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table align="center">
                                        <tr>
                                            <td class="ClsBorderLight" align="left" style="width: 200px">
                                                <asp:Label ID="lblStudentName" CssClass="ClsLabel" runat="server" Text="Student Name :"></asp:Label>
                                            </td>
                                            <td class="ClsHilightBGB">
                                                <asp:Label ID="lblStudentName1" runat="server" CssClass="ClsLabel"></asp:Label>
                                                <asp:CustomValidator ID="CustStudentName" runat="server" Display="None" CssClass="ClsMdtStar"
                                                    ClientValidationFunction="ValidateStudentName"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight" align="left">
                                                <span class="ClsLabel">Reason :</span>
                                            </td>
                                            <td class="txtNormal" align="left">
                                                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" MaxLength="20" CssClass="ExLrgTxtBox"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="ReqReason" runat="server" ErrorMessage="Reason should not be blank."
                                                    Display="None" ControlToValidate="txtReason"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight" align="left">
                                                <span class="ClsLabel">Refund Cheque In Favour Of :</span>
                                            </td>
                                            <td class="txtNormal" align="left">
                                                <asp:TextBox ID="txtRefundcheque" runat="server" TextMode="MultiLine" MaxLength="20"
                                                    CssClass="ExLrgTxtBox"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="ReqRefundcheque" runat="server" ErrorMessage="Refund Cheque In Favour Of should not be blank."
                                                    Display="None" ControlToValidate="txtRefundcheque"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight" align="left">
                                                <span class="ClsLabel">Cell :</span>
                                            </td>
                                            <td class="txtNormal" align="left">
                                                <asp:TextBox ID="txtCell" runat="server" MaxLength="10" onblur="extractNumber(this,0,false);"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:CustomValidator ID="custCell" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    Visible="true" EnableClientScript="true" ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                        class="ClsBtn" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                                        class="ClsBtn" OnClick="btnCancel_Click" CausesValidation="False" />
                                </td>
                            </tr>--%>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwSearchStudentDetails" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                class="ClsBtn" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                                class="ClsBtn" OnClick="btnCancel_Click" CausesValidation="False" />
                        </td>
                    </tr>
                    <tr id="trStudents" runat="server">
                        <td>
                            <asp:UpdatePanel ID="Up5" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="StudentDetails" align="center" width="90%" runat="server">
                                        <tr>
                                            <td align="center" class="width-99-percentage">
                                                <table align="center" width="100%">
                                                    <tr id="tr2" runat="server" visible="true">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCountStudents" runat="server" PagedControlID="lstvwStudents"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1%>" />
                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                            <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                            <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                            <br />
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trpager1" runat="server" width="100%">
                                            <td align="center">
                                                <asp:ListView ID="lstvwStudents" runat="server" ViewStateMode="Enabled" DataKeyNames="Id,SchoolwiseStudentId,StandardId,DivisionId,StudentId,SubmittedBy"
                                                    OnItemCommand="lstvwStudents_ItemCommand" OnItemDataBound="lstvwStudents_ItemDataBound"
                                                    OnDataBound="lstvwStudents_DataBound" OnSorting="lstvwStudents_Sorting">
                                                    <LayoutTemplate>
                                                        <table id="tbllstvwStudentDetails" runat="server" align="center" cellpadding="0"
                                                            cellspacing="1" class="GridBorder" width="100%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" class="PaddingL" width="80px">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                        Text="Enrolment No."></asp:Label>
                                                                </th>
                                                                <th align="center" class="PaddingL" width="70px">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                        Text="Roll No"></asp:Label>
                                                                </th>
                                                                <th align="center" class="PaddingL" width="120px">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                        Text="Class Name"></asp:Label>
                                                                </th>
                                                                <th align="left" class="paddingL">
                                                                    <asp:Label ID="Label6" runat="server" Text="Student Name" Style="padding-right: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 60px;">
                                                                    Edit
                                                                </th>
                                                                <th align="center" style="width: 60px;">
                                                                    Delete
                                                                </th>
                                                                <th align="center" style="width: 90px;">
                                                                    Apply Fee
                                                                </th>
                                                                <th align="center" style="width: 150px;">
                                                                    Download Report
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr id="trDataPager" class="ClsBorderPager">
                                                                <td colspan="10">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudents" PageSize="20">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
                                                                                                <asp:DropDownList ID="ddlCnt" ViewStateMode="Enabled" runat="server" OnSelectedIndexChanged="cmbPageCnt2_SelectedIndexChanged"
                                                                                                    AutoPostBack="true">
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
                                                        <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                            <td align="center" class="PaddingL">
                                                                <asp:Label ID="lblReceiverName1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("Enrolment_Number") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td1" align="center" class="PaddingLR" runat="server">
                                                                <asp:Label ID="lblInvoiceNo1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("Roll_No") %>'>
                                                                </asp:Label>
                                                                <asp:HiddenField ID="hidData1" runat="server" Value="" />
                                                            </td>
                                                            <td id="Td2" align="center" class="PaddingL" runat="server">
                                                                <asp:Label ID="lblInvoiceDate1" runat="server" Text='<%# Eval("ClassName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td3" align="left" class="PaddingL" runat="server">
                                                                <asp:Label ID="lblTotalAmount1" runat="server" Style="padding-right: 5px;" Text='<%# Eval("StudentName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCancellationFormDetails"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="DeleteCancellationFormDetails"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:LinkButton ID="LnkBtnApplyFee" runat="server" Text="Apply Fee" CommandName="APPLY_FEE" CausesValidation="false"></asp:LinkButton>
                                                            </td>
                                                            <td id="Td7" runat="server" align="center">
                                                                <asp:LinkButton ID="lnkReport" runat="server" CommandName="CancellationFormDetails"
                                                                    CausesValidation="false" Text="Open"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                            <td align="center" class="PaddingL">
                                                                <asp:Label ID="lblReceiverName1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("Enrolment_Number") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td1" align="center" class="PaddingLR" runat="server">
                                                                <asp:Label ID="lblInvoiceNo1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("Roll_No") %>'>
                                                                </asp:Label>
                                                                <asp:HiddenField ID="hidData1" runat="server" Value="" />
                                                            </td>
                                                            <td id="Td2" align="center" class="PaddingL" runat="server">
                                                                <asp:Label ID="lblInvoiceDate1" runat="server" Text='<%# Eval("ClassName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td3" align="left" class="PaddingL" runat="server">
                                                                <asp:Label ID="lblTotalAmount1" runat="server" Style="padding-right: 5px;" Text='<%# Eval("StudentName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCancellationFormDetails"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="DeleteCancellationFormDetails"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:LinkButton ID="LnkBtnApplyFee" runat="server" Text="Apply Fee" CommandName="APPLY_FEE" CausesValidation="false"></asp:LinkButton>
                                                            </td>
                                                            <td id="Td7" runat="server" align="center">
                                                                <asp:LinkButton ID="lnkReport" runat="server" CommandName="CancellationFormDetails"
                                                                    CausesValidation="false" Text="Open"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr id="trNoRecordForSecondListview" runat="server" style="width: 800px">
                                                            <td align="center" class="LblNoRecord">
                                                                No record Found
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.CancellationFormBL" EnablePaging="true"
                                                    ID="ObjectDataSourceStudent" runat="server" SelectMethod="GetAllStudents" SortParameterName="SortExpression"
                                                    SelectCountMethod="GetCountStudents" EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                            Type="int32" />
                                                        <asp:Parameter Name="SortExpression" Type="String" />
                                                        <asp:Parameter Name="SortDirection" Type="String" />
                                                        <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                        <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnl4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSchoolwiseStudentId" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwSearchStudentDetails" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        _clienthidSchoolwiseStudentId = "<%=this.hidSchoolwiseStudentId.ClientID %>";

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function ValidateStudentName(oSrc, args) {
            if (document.getElementById(_clienthidSchoolwiseStudentId).value == "0") {
                oSrc.errormessage = "Student Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        _sClienttxtCellId = "<%=this.txtCell.ClientID %>";
        function MobileNumberValidation(oSrc, args) {

            var sCell = document.getElementById(_sClienttxtCellId).value;

            if (sCell == "") {
                oSrc.errormessage = "Cell No. should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (sCell.length < 10 && sCell.length != 0) {
                oSrc.errormessage = "Cell No. length should be 10 digit.";
                args.IsValid = false;
                return true;
            }
            else if (sCell.substring(0, 1) == '0') {
                oSrc.errormessage = "Cell No. should not start with 0.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function OpenReport(index) {
            var str = $('[id$=' + index + '_hidData1]').val()
            window.open('../Admission/AdmissionFormReport.aspx?' + str, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=500,height=150')
        }

    </script>
</asp:Content>
