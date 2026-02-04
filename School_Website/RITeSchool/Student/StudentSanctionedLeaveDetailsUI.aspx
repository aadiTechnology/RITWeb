<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="~/RITeSchool/Student/StudentSanctionedLeaveDetailsUI.aspx.cs"
    Inherits="StudentSanctionedLeaveDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" align="center">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
            vertical-align: top">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td style="width: 77%">
                                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                    <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px;">
                                        <span class="ClsMdtStar">*
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label></span></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                            CssClass="NewClsLabel" ShowSummary="true" />
                                        <asp:CustomValidator ID="cst_StartAndEndDate" runat="server" ClientValidationFunction="cstStartAndEndDate"
                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cst_StartDateValidation" runat="server" ClientValidationFunction="cstStartDateValidation"
                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cst_EndDateValidation" runat="server" ClientValidationFunction="cstEndDateValidation"
                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cst_ValidateYear" runat="server" ClientValidationFunction="cstValidateYear"
                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cst_ValidateEndDateForUsedLeaves" runat="server" ClientValidationFunction="cstValidateEndDateForUsedLeaves"
                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                            EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                        <table width="80%">
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="clsLabel">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, SelectStandard %>"></asp:Label>
                                                        <span id="Span1" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlStandard" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="ClsBorderlight">
                                                    <span class="clsLabel" style="width: 125px">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, SelectDivision %>"></asp:Label>
                                                        <span id="Span2" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlDivision" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" align="right">
                                                    <span class="clsLabel">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, StudentNameRegNo %>"></asp:Label>
                                                        <span id="Span3" class="colonPadding">:</span> </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" CssClass="LrgTxtBox" AutoPostBack="False"
                                                        autocomplete="off"></asp:TextBox>
                                                </td>
                                                <td class="ClsBorderlight" align="right">
                                                    <span class="clsLabel">
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, ShowUsedLeaves %>"></asp:Label>
                                                        <span id="Span4" class="colonPadding">:</span></span>
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkShowCanceledRecords" runat="server" Enabled="true" AutoPostBack="true"
                                                        OnCheckedChanged="chkShowCanceledRecords_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>"
                                                        CssClass="ClsBtn" CausesValidation="true" OnClick="btnSearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl2" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr id="trPagerStudentSanctionedLeaves" runat="server">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwStudentSanctionedLeave">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, To %>" />
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf %>" />
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records %>" />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr id="trLegend" runat="server" align="center">
                                    <td align="left">
                                        <table id="LegendTable" runat="server" align="left" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td align="left" width="60px">
                                                    <asp:Label ID="lblLegend" runat="server" class="ClsLblLgnd" Style="border-width: 0px;
                                                        font-weight: bold" Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblLegendImage" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                        BackColor="#FFCCCC" Height="20px" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                </td>
                                                <td align="center" id="tblSanctionedLeave" style="padding-left: 10px;" runat="server">
                                                    <%--<asp:Label ID="lblLongLeaveExceed" runat="server" Text="<%$ Resources:LocalizedResources,LongLeaveExceeded%>".replace("%maxdays%",<%=Session["MaxLeaveDays"]%>)></asp:Label>--%>
                                                    <span class="ClsTextNormal" style="font-weight: bold">Long leave exceeded more than
                                                        <%= SchoolBase.Settings.MaxLeaveDays %>
                                                        days</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%-- <div id="div1" runat="server" style="width: 635pt; height: 100%; ">--%>
                                        <table align="center" width="100%">
                                            <tr align="center" style="width: 100%">
                                                <td align="center" style="width: 100%">
                                                    <asp:ListView ID="lstvwStudentSanctionedLeave" DataKeyNames="SanctionedLeaveDetailsId,StudentId,UserId"
                                                        runat="server" DataSourceID="ObjDSStudentSanctionedLeaves" OnDataBound="lstvwStudentSanctionedLeave_DataBound"
                                                        OnItemDataBound="lstvwStudentSanctionedLeave_ItemDataBound" OnItemCommand="lstvwStudentSanctionedLeave_ItemCommand">
                                                        <LayoutTemplate>
                                                            <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" width="8%" style="padding-left: 8px;">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, RegNo %>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" width="18%" style="padding-left: 8px;">
                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, StudentName %>"></asp:Label>
                                                                    </th>
                                                                    <th align="center" width="9%">
                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, Class %>"></asp:Label>
                                                                    </th>
                                                                    <th align="center" width="12%">
                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources,  StartDate %>"></asp:Label>
                                                                    </th>
                                                                    <th align="center" width="12%">
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, EndDate %>"></asp:Label>
                                                                    </th>
                                                                     <th align="center" width="12%" style="padding-left: 8px;">
                                                                        <asp:Label ID="Label13" runat="server" Text="Remark"></asp:Label>
                                                                    </th>
                                                                    <th align="center" width="15%">
                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, MobileNumbers %>"></asp:Label>

                                                                    </th>
                                                                   
                                                                    <th align="center" width="6%">
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, IsUsed %>"></asp:Label>
                                                                    </th>
                                                                    
                                                                    <th align="center" width="15%">
                                                                        <asp:Label ID="Label12" runat="server" Text="Show On Absent Student Popup?"></asp:Label>
                                                                    </th>
                                                                    
                                                                    <th align="center" width="7%">
                                                                        <asp:Label ID="lblDelete" runat="server"  Text="<%$ Resources:LocalizedResources, Delete %>"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                    <td colspan="8">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentSanctionedLeave"
                                                                            PageSize="20">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectAPage %>"
                                                                                                        runat="server" CssClass="LblNrmlB" />
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
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%# Eval("RegistrationNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblClass" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtStartDate" runat="server" MaxLength="50" CssClass="SmlTxtBox"
                                                                        AutoPostBack="false" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, StartDateShouldNotBeBlank %>" />
                                                                    <asp:HiddenField ID="hidStartDt" runat="server" Value='<%# Eval("AStartDate","{0:dd-MMM-yyyy}") %>' />
                                                                    <asp:HiddenField ID="hidEndDt" runat="server" Value='<%# Eval("AEndDate","{0:dd-MMM-yyyy}") %>' />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtEndDate" runat="server" MaxLength="50" CssClass="SmlTxtBox" AutoPostBack="False"
                                                                        Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, EndDateShouldNotBeBlank %>" />
                                                                </td>
                                                                 <td align="center">
                                                                   <asp:TextBox ID="txtRemark" runat="server" MaxLength="100" CssClass="LrgTxtBox"
                                                                        AutoPostBack="false" Text='<%# Eval("Remark") %>'></asp:TextBox>
                                                                       
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                                </td>                                                               
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkIsCanceled" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkShowOnAbsectStudentPopUp" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                   <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ImageUrl="../images/IconGrid_Delete.gif" />
                                                                </td>
                                                               
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%# Eval("RegistrationNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblClass" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtStartDate" runat="server" MaxLength="50" CssClass="SmlTxtBox"
                                                                        AutoPostBack="false" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, StartDateShouldNotBeBlank %>" />
                                                                    <asp:HiddenField ID="hidStartDt" runat="server" Value='<%# Eval("AStartDate","{0:dd-MMM-yyyy}") %>' />
                                                                    <asp:HiddenField ID="hidEndDt" runat="server" Value='<%# Eval("AEndDate","{0:dd-MMM-yyyy}") %>' />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtEndDate" runat="server" MaxLength="50" CssClass="SmlTxtBox" AutoPostBack="False"
                                                                        Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                    <rjs:PopCalendar ID="calEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, EndDateShouldNotBeBlank %>" />
                                                                </td>
                                                                 <td align="center">
                                                                   <asp:TextBox ID="txtRemark" runat="server" MaxLength="100" CssClass="LrgTxtBox"
                                                                        AutoPostBack="false" Text='<%# Eval("Remark") %>'></asp:TextBox>
                                                                        
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                                </td>                                                                
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkIsCanceled" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkShowOnAbsectStudentPopUp" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ImageUrl="../images/IconGrid_Delete.gif" />
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                            <asp:ObjectDataSource TypeName="BusinessLogic.StudentSanctionedLeavesBL" EnablePaging="True"
                                                ID="ObjDSStudentSanctionedLeaves" runat="server" SelectMethod="GetStudentSanctionedLeaveDetails"
                                                SortParameterName="sortExpression" SelectCountMethod="CountTotalSanctionedLeaves"
                                                EnableCaching="False">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                        Type="int32" />
                                                    <asp:ControlParameter ControlID="hidStandardId" Type="Int32" PropertyName="Value"
                                                        DefaultValue="0" Name="aiStandardId" />
                                                    <asp:ControlParameter ControlID="hidDivisionId" Type="Int32" PropertyName="Value"
                                                        DefaultValue="0" Name="aiDivisionId" />
                                                    <asp:ControlParameter ControlID="txtSearch" Type="String" PropertyName="Text" DefaultValue=""
                                                        Name="asName" />
                                                    <asp:ControlParameter ControlID="hidShowCanceledRecords" Type="Boolean" PropertyName="Value"
                                                        DefaultValue="" Name="abShowCanceledRecords" />
                                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                            <asp:HiddenField ID="hidMode" runat="server" />
                                            <asp:HiddenField ID="hidServerDate" runat="server" />
                                            <asp:HiddenField ID="hidSanctionedLeaveId" runat="server" />
                                            <asp:HiddenField ID="hidStandardId" runat="server" />
                                            <asp:HiddenField ID="hidDivisionId" runat="server" />
                                            <asp:HiddenField ID="hidShowCanceledRecords" runat="server" />
                                            <asp:HiddenField ID="hidPageNo" runat="server" Value="1" />
                                            <asp:HiddenField ID="HidSMSTemplateName" runat="server" />
                                            <asp:HiddenField ID="hidSmsTemplate" runat="server" />
                                            <asp:HiddenField ID="hidEndDateShouldBeGreaterThanStartDateForRow" runat="server" />
                                            <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                            <asp:HiddenField ID = "hidAreYouSureYouWantToDeleteThisRecords" runat = "server" />
                                            <asp:HiddenField ID="hidStartDateAndEndDateShouldBeWithinCurrentAcademicYearAtRow"
                                                runat="server" />
                                            <asp:HiddenField ID="hidEndDateShouldNotBeBlankForRow" runat="server" />
                                            <asp:HiddenField ID="hidIfYouChangeThePageThenEnteredDatesOnCurrentPageWillGetLost"
                                                runat="server" />
                                            <asp:HiddenField ID="hidStartDateShouldNotBeBlankForRow" runat="server" />
                                            <asp:HiddenField ID="hidEndDateSHouldNotBeFuture" runat ="server" />
                                        </table>
                                        <%--</div>--%>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="chkShowCanceledRecords" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td colspan="4" align="center">
                                <asp:UpdatePanel ID="upnl" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                            CssClass="ClsBtn" Height="24px" disable-page="true" ValidationGroup="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>" 
                                            CssClass="ClsBtn" Height="24px" OnClick="btnBack_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkShowCanceledRecords" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientlstvwStudentSanctionedLeave = "<%=this.lstvwStudentSanctionedLeave.ClientID %>"
        _clientcst_StartAndEndDate = "<%=this.cst_StartAndEndDate.ClientID %>";
        _clientlbl_UpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
        _clientcst_StartDateValidation = "<%=this.cst_StartDateValidation.ClientID %>";
        _clientcst_EndDateValidation = "<%=this.cst_EndDateValidation.ClientID %>";

        _clientLblErrorMessage = "<%=this.lblErrorMsg.ClientID %>";
        _clientcst_ValidateYear = "<%=this.cst_ValidateYear.ClientID %>";
        _clientddlStandard = "<%=this.ddlStandard.ClientID %>";
        _clientcst_ValidateEndDateForUsedLeaves = "<%=this.cst_ValidateEndDateForUsedLeaves.ClientID%>";

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

        function cstStartAndEndDate(oSrc, args) {

            var dtStartDate;
            var dtEndDate;
            var sMsg = "";
            var isValid = true;
            var chk
            var i = 1;
            var iRow = 0;
            var iPercent = "";
            var sHolidayName = "";
            var maxRows;
            var EndDate;
            var Startdate
            if ((document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) > 20)
                maxRows = 20;
            else
                maxRows = (document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) - 1;
            while (i <= maxRows) {
                var HolidyStartDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtStartDate").value;
                var HolidyEndDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtEndDate").value;
                if (HolidyStartDate != null && HolidyStartDate != "" && HolidyEndDate != null && HolidyEndDate != "") {
                    if (document.all) {
                        EndDate = new Date(HolidyEndDate.replace('-', ' '));
                        Startdate = new Date(HolidyStartDate.replace('-', ' '));
                    }
                    else {
                        EndDate = new Date(convertdate(HolidyEndDate));
                        Startdate = new Date(convertdate(HolidyStartDate));
                    }
                    if (!(Startdate <= EndDate))
                        sMsg = sMsg + i + ", ";
                }
                i = i + 1;
                iRow = iRow + 1;
            }
            if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                document.getElementById(_clientcst_StartAndEndDate).errormessage = document.getElementById("<%=this.hidEndDateShouldBeGreaterThanStartDateForRow.ClientID %>").value + " " + sMsg;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
                lbl1.innerHTML = "";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function cstEndDateValidation(oSrc, args) {
            var dtStartDate;
            var dtEndDate;
            var sMsg = "";
            var isValid = true;
            var chk
            var i = 1;
            var iRow = 0;
            var iPercent = "";
            var sHolidayName = "";
            var maxRows;
            if ((document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) > 20)
                maxRows = 20;
            else
                maxRows = (document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) - 1;
            while (i <= maxRows) {
                var HolidyStartDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtStartDate").value.trim();
                var HolidyEndDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtEndDate").value.trim();
                if (HolidyStartDate != null && HolidyStartDate != "" && (HolidyEndDate == null || HolidyEndDate == "")) {
                    sMsg = sMsg + i + ", ";
                }
                i = i + 1;
                iRow = iRow + 1;
            }
            if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                document.getElementById(_clientcst_EndDateValidation).errormessage = document.getElementById(_clientcst_EndDateValidation).errormessage = document.getElementById("<%=this.hidEndDateShouldNotBeBlankForRow.ClientID %>").value + sMsg;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
                lbl1.innerHTML = "";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;

            return false;
        }

        function cstValidateYear(oSrc, args) {

            var EndDate;
            var Startdate
            var sMsg = "";
            var isValid = true;
            var chk
            var i = 1;
            var iRow = 0;
            var iPercent = "";
            var sHolidayName = "";
            var maxRows;
            var SelectedStd = document.getElementById(_clientddlStandard).value;
            if ((document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) > 20)
                maxRows = 20;
            else
                maxRows = (document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) - 1;
            while (i <= maxRows) {
                var HolidyStartDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtStartDate").value.trim();
                var HolidyEndDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtEndDate").value.trim();
                var dtStartDate1 = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_hidStartDt").value.trim();
                var dtEndDate1 = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_hidEndDt").value.trim();
                dtStartDate = new Date(convertdate(dtStartDate1));
                dtEndDate = new Date(convertdate(dtEndDate1));
                //dtEndDate = new Date(dtEndDate1.replace('-', '/'));
                //dtStartDate = dtStartDate1;
                //dtEndDate = dtEndDate1;
                if (HolidyStartDate != null && HolidyStartDate != "" && (HolidyEndDate != null || HolidyEndDate != "")) {

                    //EndDate = new Date(HolidyEndDate.replace('-', '/'));
                    //Startdate = new Date(HolidyStartDate.replace('-', '/'));
                    EndDate = new Date(convertdate(HolidyEndDate));
                    Startdate = new Date(convertdate(HolidyStartDate));
                    if (Startdate < dtStartDate || EndDate > dtEndDate)
                        if (SelectedStd != 0) {
                            sMsg = sMsg + i + ", ";
                        }
                        else {
                            sMsg = sMsg + i + '(' + dtStartDate1 + ' To ' + dtEndDate1 + ')' + ", ";
                        }

                }
                i = i + 1;
                iRow = iRow + 1;
            }

            if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                if (SelectedStd == 0)
                    document.getElementById(_clientcst_ValidateYear).errormessage = document.getElementById("<%=this.hidStartDateAndEndDateShouldBeWithinCurrentAcademicYearAtRow.ClientID %>").value + sMsg;
                else
                    document.getElementById(_clientcst_ValidateYear).errormessage = document.getElementById("<%=this.hidStartDateAndEndDateShouldBeWithinCurrentAcademicYearAtRow.ClientID %>").value + '(' + dtStartDate1 + ' To ' + dtEndDate1 + ')' + " at row(s): " + sMsg;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
                lbl1.innerHTML = "";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;

        }

        function cstStartDateValidation(oSrc, args) {
            var dtStartDate;
            var dtEndDate;
            var sMsg = "";
            var isValid = true;
            var chk
            var i = 1;
            var iRow = 0;
            var iPercent = "";
            var sHolidayName = "";
            var maxRows;
            if ((document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) > 20)
                maxRows = 20;
            else
                maxRows = (document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) - 1;
            while (i <= maxRows) {
                var HolidyStartDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtStartDate").value.trim();
                var HolidyEndDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtEndDate").value.trim();
                if ((HolidyStartDate == null || HolidyStartDate == "") && HolidyEndDate != null && HolidyEndDate != "") {
                    sMsg = sMsg + i + ", ";
                }
                i = i + 1;
                iRow = iRow + 1;
            }
            if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                document.getElementById(_clientcst_StartDateValidation).errormessage = document.getElementById("<%=this.hidStartDateShouldNotBeBlankForRow.ClientID %>").value + sMsg;
                document.getElementById(_clientLblErrorMessage).style.display = 'block';
                var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
                lbl1.innerHTML = "";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function cstValidateEndDateForUsedLeaves(oSrc, args) {
            var today = new Date();
            var dtStartDate;
            var dtEndDate;
            var sMsg = "";
            var isValid = true;
            var chk
            var i = 1;
            var iRow = 0;
            var iPercent = "";
            var sHolidayName = "";
            var maxRows;
             if ((document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) > 20)
                maxRows = 20;
             else
                 maxRows = (document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) - 1;
             while (i <= maxRows) {
                 var HolidyEndDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtEndDate").value.trim();
                 var isUsedLeave = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_chkIsCanceled").checked;
                 EndDate = new Date(convertdate(HolidyEndDate));

                 if (isUsedLeave && EndDate > today) {
                     sMsg = sMsg + i + ", ";
                 }
                 i = i + 1;
                 iRow = iRow + 1;
             }
             if (sMsg != "") {
                 sMsg = sMsg.substring(0, sMsg.length - 2);
                 document.getElementById(_clientcst_ValidateEndDateForUsedLeaves).errormessage = document.getElementById("<%=this.hidEndDateSHouldNotBeFuture.ClientID %>").value + sMsg;
                 document.getElementById(_clientLblErrorMessage).style.display = 'block';
                 var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
                 lbl1.innerHTML = "";
                 args.IsValid = false;
                 return true;
             }

             args.IsValid = true;
             return false;
        }

        function btnsaveonclick(varname) {
            var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
            lbl1.innerHTML = "";
            var lbl1 = document.getElementById(_clientLblErrorMessage);
            lbl1.innerHTML = "";
        }

        function MessageAboutDate(oCmb) {
            var bIsValid
            if (window.confirm(document.getElementById("<%=this.hidIfYouChangeThePageThenEnteredDatesOnCurrentPageWillGetLost.ClientID %>").value))
                bIsValid = true
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false
            }
            return bIsValid
        }
    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _clienttxtRegNumber = '#<%=txtSearch.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            var ddlDivision = '<%=ddlDivision.ClientID%>';
            _clientddlStandard = '<%=ddlStandard.ClientID %>';

            BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, _clientddlStandard, ddlDivision, null, 1);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtSearch.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

        //This Function is used to confirmed delete sanctioned leave.
        function ConfirmSanctionLeaveDelete(isUsedLeave) {
            var bResult = true
            if (isUsedLeave == "True") {
                alert("You can not delete this leave, as it is an used leave.");
                return false;
            }
            else {
                if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantToDeleteThisRecords.ClientID %>").value)) {
                    alert('Cancel');
                    bResult = false
              }
            }
            return bResult
        }

    </script>
</asp:Content>
