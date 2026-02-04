<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LeaveDeatilsUI.aspx.cs" Inherits="LeaveDeatilsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td align="center">
                <table align="center">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Blue" CssClass="ClsLabelNrml"
                                        EnableViewState="false"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbReportingRole" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="chkNonLeave" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwConfiguration" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight" style="width: 200px;">
                            <asp:Label ID="lblAcademicYear" runat="server" CssClass="ClsLabel" Text="Academic Year"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbAcademicYear" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                OnSelectedIndexChanged="cmbAcademicYear_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight" style="width: 200px;">
                            <asp:Label ID="lblReportingRole" runat="server" CssClass="ClsLabel" Text="Category"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbReportingRole" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                OnSelectedIndexChanged="cmbReportingRole_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">*</span>
                            <asp:RequiredFieldValidator ID="reqvalcmbReportingRole" runat="server" Display="None"
                                ControlToValidate="cmbReportingRole" CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="<%$Resources:LocalizedResources, valReportingUserRole%>"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                 
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="trShowUpdated" runat="server" visible="false">
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 205px;">
                                                <asp:Label ID="Label8" runat="server" CssClass="ClsLabel" Text="Show Only Non-updated Records?"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkNonLeave" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="chkNonLeave_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>   
                                        <td class="ClsBorderlight">
                                               <asp:Label ID="Label9" runat="server" CssClass="ClsLabel" Text="Show Old Non-updated Records?"></asp:Label>
                                                 <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td>
                                               <asp:CheckBox ID="chknonupdatedrecords" runat="server" Checked="false" AutoPostBack="True" OnCheckedChanged="chknonupdatedrecords_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbReportingRole" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" runat="server" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="90%">
                            <tr>
                                <td align="left">
                                    <table id="trLegend" runat="server" visible="false">
                                        <tr>
                                            <td class="ClsLblLgnd" style="padding-right: 5px;">
                                                Legend :
                                            </td>
                                            <td style="border: 1px solid navy; padding-left: 5px; padding-right: 5px;">
                                                <span class="clsLabel"><b>Leave not updated in Payroll</b></span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server" id="trTotalRec" align="center">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwConfiguration">
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
                                <td align="center" valign="top" runat="server">
                                    <asp:ListView ID="lstvwConfiguration" runat="server" ItemPlaceholderID="trItemPlaceholder"
                                        DataKeyNames="Id,StatusId,UserId,IsLeaveUpdatedInPayroll,IsApprovedByApprover" OnSorting="lstvwConfiguration_Sorting"
                                        OnItemDataBound="lstvwConfiguration_ItemDataBound" OnDataBound="lstvwConfiguration_DataBound"
                                        OnItemCommand="lstvwConfiguration_ItemCommand">
                                        <LayoutTemplate>
                                            <table id="tblDetails" style="width: 100%; color: #333333" class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th class="ClspaddingL" width="230px">
                                                        <asp:Label ID="lblName" runat="server" Text="Sender Name"></asp:Label>
                                                    </th>
                                                    <th style="width: 100px; text-align: center;">
                                                        <asp:Label ID="lblhidIsFinalApprover" runat="server" Text="Start Date"></asp:Label>
                                                    </th>
                                                    <th style="width: 100px;">
                                                        <asp:Label ID="lblhidIsSupervisor" runat="server" Text="End Date"></asp:Label>
                                                    </th>
                                                    <th align="left" class="paddingLSML">
                                                        <asp:Label ID="Label1" runat="server" Text="Description"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 70px;">
                                                        <asp:Label ID="Label5" runat="server" Text="Total Days"></asp:Label>
                                                    </th>
                                                    <th align="left" style="width: 80px;" class="paddingLSML">
                                                        <asp:Label ID="Label2" runat="server" Text="Status"></asp:Label>
                                                    </th>
                                                    <th align="left" style="width: 100px;" class="paddingLSML">
                                                        <asp:Label ID="Label6" runat="server" Text="Leave Type"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 110px;">
                                                        <asp:Label ID="Label7" runat="server" Text="Leave Balance"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 50px; padding-right: 5px">
                                                        <asp:Label ID="Label3" runat="server" Text="View"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 50px; padding-right: 5px" id="thDelete" runat="server">
                                                        <asp:Label ID="Label4" runat="server" Text="Delete"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="trItemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="10">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwConfiguration"
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
                                            <tr class="ClsGridRow" id="Tr2" runat="server">
                                                <td align="left">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblUserName" Text='<%#Eval("UserName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblStartDate" Text='<%#Eval("StartDate") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblEndDate" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblDescription" Text='<%#Eval("Description") %>'></asp:Label>
                                                </td>
                                                <td align="center" style="text-align: center;">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblTotalDays" Text='<%#Eval("TotalDays") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblStatus" Text='<%#Eval("Status") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblLeaveType" Text='<%#Eval("LeaveName") %>'></asp:Label>
                                                </td>
                                                <td align="center" style="text-align: center;">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblLeaveBalance" Text='<%#Eval("LeaveBalance") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton runat="server" ID="BtnView" CommandName="UpdateCommand" AlternateText="<%$ Resources:LocalizedResources, View%>"
                                                        ToolTip="<%$ Resources:LocalizedResources, View%>" CausesValidation="false" ImageUrl="../images/iconGridSml_ViewGE.gif">
                                                    </asp:ImageButton>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton runat="server" ID="imgDelete" CommandName="RemoveCommand" AlternateText="<%$ Resources:LocalizedResources, Delete%>"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CausesValidation="false"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="ClsGridAltRow" id="Tr2" runat="server">
                                                <td align="left">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblUserName" Text='<%#Eval("UserName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblStartDate" Text='<%#Eval("StartDate") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblEndDate" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblDescription" Text='<%#Eval("Description") %>'></asp:Label>
                                                </td>
                                                <td align="center" style="text-align: center;">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblTotalDays" Text='<%#Eval("TotalDays") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblStatus" Text='<%#Eval("Status") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblLeaveType" Text='<%#Eval("LeaveName") %>'></asp:Label>
                                                </td>
                                                <td align="center" style="text-align: center;">
                                                    <asp:Label runat="server" CssClass="ClsLabel" ID="lblLeaveBalance" Text='<%#Eval("LeaveBalance") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton runat="server" ID="BtnView" CommandName="UpdateCommand" AlternateText="<%$ Resources:LocalizedResources, View%>"
                                                        ToolTip="<%$ Resources:LocalizedResources, View%>" CausesValidation="false" ImageUrl="../images/iconGridSml_ViewGE.gif">
                                                    </asp:ImageButton>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton runat="server" ID="imgDelete" CommandName="RemoveCommand" AlternateText="<%$ Resources:LocalizedResources, Delete%>"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CausesValidation="false"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"></asp:ImageButton>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td class="LblNoRecord" align="center">
                                                    <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:ObjectDataSource TypeName="BusinessLogic.PayrollBL.UserApplyLeaveDetailsBL"
                                        EnablePaging="True" ID="objdsPayments" runat="server" SelectCountMethod="Count"
                                        SelectMethod="GetAll" SortParameterName="sortExpression" EnableCaching="False">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:SessionParameter Name="aiUserId" SessionField="I_USER_ID" Type="int32" />
                                            <asp:ControlParameter ControlID="cmbReportingRole" Name="aiCategoryId" Type="String"
                                                PropertyName="SelectedValue" />
                                            <asp:ControlParameter ControlID="cmbAcademicYear" Name="aiAcademicYearId" Type="int32" PropertyName="SelectedValue" />
                                            <asp:ControlParameter ControlID="chkNonLeave" Name="abShowOnlyNonUpdated" Type="Boolean"
                                                PropertyName="Checked" />
                                            <asp:ControlParameter ControlID="chknonupdatedrecords" Name="abShowOldNonUpdated" Type="Boolean"
                                                PropertyName="Checked" />
                                            <asp:Parameter Name="sortExpression" Type="String" />
                                            <asp:Parameter Name="sortDirection" Type="String" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbReportingRole" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbAcademicYear" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="chkNonLeave" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfiguration" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnAdd" CssClass="ClsBtn" runat="server" Text="Add Leave" UseSubmitBehavior="false"
                            CausesValidation="false" OnClick="btnAdd_Click" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                        <asp:HiddenField ID="hidmsgConfirmDelete" runat="server" />
                        <asp:HiddenField ID="hidPageNo" runat="server" />
                        <asp:HiddenField ID="hidHasFullAccess" runat="server" Value="N" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbReportingRole" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="chkNonLeave" EventName="CheckedChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <script language="javascript" type="text/javascript">
            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?');
            }
        </script>
    </table>
</asp:Content>
