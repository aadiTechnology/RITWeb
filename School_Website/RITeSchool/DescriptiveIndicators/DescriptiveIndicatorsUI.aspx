<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DescriptiveIndicatorsUI.aspx.cs" Inherits="DescriptiveIndicatorsUI"
    ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" width="100%" cellpadding="0">
        <tr style="margin: 0px auto;">
            <td>
                <table id="tblLearningOutcome" runat="server" style="width: 100%;">
                    <tr>
                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table id="LegendTable" runat="server" visible="false">                                        
                                        <tr>
                                            <td style="height: 10px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 70px;">
                                                <asp:Label ID="lblLegend" runat="server" CssClass="ClsLblLgnd" EnableViewState="false"
                                                    Font-Bold="True" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                                <span class="ClsLblLgnd colonPadding">:</span>
                                            </td>
                                            <td align="right">
                                                &nbsp;<asp:Image ID="Image1" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/icoGrid_MarkEntryNotStart.gif" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="Label4" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                    Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryNotStarted %>"></asp:Label>
                                            </td>
                                            <td align="left">
                                                &nbsp;<asp:Image ID="Image2" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/icoGrid_MarkEntryPartDone.gif" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="Label8" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                    Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryPartiallyDone %>"></asp:Label>
                                            </td>
                                            <td align="left">
                                                &nbsp;
                                                <asp:Image ID="Image3" runat="server" CssClass="img-align-unset" EnableViewState="False"
                                                    ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="Label9" runat="server" CssClass="ClsTextNormal" EnableViewState="False"
                                                    Font-Bold="True" Text="<%$ Resources:LocalizedResources, MarksEntryCompleted %>"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbClassTeachers" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="CmbTerm" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center" style="height: 50px;">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <table style="margin: 0px auto;" width="100%">
                                                    <tr>
                                                        <td id="tdClassTeacherLable" runat="server" align="right" class="ClsBorderlight">
                                                            <asp:Label ID="lblTeacher" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                Font-Bold="True" Text="<%$ Resources:LocalizedResources, SelectClassTeacher %>"></asp:Label>
                                                            <span class="ClsLblLgnd colonPadding">:</span>
                                                        </td>
                                                        <td id="tdcmbTeachers" runat="server" align="left">
                                                            <asp:DropDownList ID="cmbClassTeachers" AutoPostBack="true" runat="server" CssClass="ExLrgCombo"
                                                                OnSelectedIndexChanged="cmbClassTeachers_SelectedIndexChanged" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 20px;">
                                                        </td>
                                                        <td id="tdTermLable" runat="server" align="left" class="ClsBorderlight">
                                                            <asp:Label ID="lblTerm" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                Text="Select Term"></asp:Label>
                                                            <span class="ClsLblLgnd colonPadding">:</span>
                                                        </td>
                                                        <td id="tdCmbTerm" runat="server" align="left">
                                                            <asp:DropDownList ID="CmbTerm" AutoPostBack="true" runat="server" CssClass="SmlCombo"
                                                                OnSelectedIndexChanged="CmbTerm_SelectedIndexChanged" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbClassTeachers" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="CmbTerm" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <table align="center">
                                <tr id="trStudentDetails" runat="server">
                                    <td>
                                        <asp:UpdatePanel ID="upnl1" runat="server">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr id="trPager" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentDetails">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <asp:Label EnableViewState="false" runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblTo" runat="server" EnableViewState="false" CssClass="LblNormal"
                                                                                Text="<%$ Resources:LocalizedResources, To %>" />
                                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                                Text="<%$ Resources:LocalizedResources, OutOf %>" />
                                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblRecords" runat="server" EnableViewState="false" CssClass="LblNormal"
                                                                                Text="<%$ Resources:LocalizedResources, Records %>" />
                                                                            <br />
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%;">
                                                            <asp:ListView runat="server" ID="lstvwStudentDetails" DataKeyNames="YearwiseStudentId,EditStatus,IsPublished, StandardId"
                                                                OnDataBound="lstvwStudentDetails_DataBound" OnItemCommand="lstvwStudentDetails_ItemCommand"
                                                                OnItemDataBound="lstvwStudentDetails_ItemDataBound" OnSorting="lstvwStudentDetails_Sorting"
                                                                ViewStateMode="Enabled">
                                                                <LayoutTemplate>
                                                                    <table align="center" width="100%" runat="server" id="tblStudentDetails" style="color: #333333"
                                                                        cellpadding="0" cellspacing="1" class="GridBorder">
                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                            <th align="left" style="padding-left: 7px; width: 100px;">
                                                                                <asp:LinkButton ID="lnkBtnRollNo" runat="server" CommandName="SortRow" CommandArgument="Roll_No"
                                                                                    CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, RollNo %>"></asp:LinkButton>
                                                                            </th>
                                                                            <th align="left" style="padding-left: 7px; width: 500px;">
                                                                                <asp:LinkButton ID="lnkBtnStudentName" runat="server" CommandName="SortRow" CommandArgument="Name"
                                                                                    CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, StudentName %>"></asp:LinkButton>
                                                                            </th>
                                                                            <th align="center" style="padding-left: 3px; width: 100px">
                                                                                <asp:Label runat="server" ID="lblEditText" Text="<%$ Resources:LocalizedResources, Edit %>"></asp:Label>
                                                                            </th>
                                                                            <th id="thPublish" runat="server" align="center" style="padding-left: 3px; width: 100px">
                                                                                <asp:Label runat="server" ID="Label1" Text="Publish"></asp:Label>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                        <tr class="ClsBorderPager" id="trDataPager">
                                                                            <td colspan="4">
                                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwStudentDetails">
                                                                                    <Fields>
                                                                                        <asp:TemplatePagerField>
                                                                                            <PagerTemplate>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                            <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
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
                                                                            <asp:Label ID="lblMenuName" EnableViewState="false" runat="server" Text='<%# Eval("RollNo") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" class="paddingL">
                                                                            <asp:Label ID="lblLinkName" runat="server" EnableViewState="false" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:HyperLink ID="hyplnkEdit" runat="server" NavigateUrl="#" Text="Assign Marks"></asp:HyperLink>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnPublish" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                                                                CommandName="Publish" ToolTip="Publish" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                Text="Publish" />
                                                                            <asp:Label ID="lblPublishStatus" runat="server" Text="-" Visible="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" class="paddingL">
                                                                            <asp:Label ID="lblMenuName" runat="server" EnableViewState="false" Text='<%# Eval("RollNo") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" class="paddingL">
                                                                            <asp:Label ID="lblLinkName" runat="server" EnableViewState="false" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:HyperLink ID="hyplnkEdit" runat="server" NavigateUrl="#" Text="Assign Marks"></asp:HyperLink>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnPublish" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                                                                CommandName="Publish" ToolTip="Publish" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                Text="Publish" />
                                                                            <asp:Label ID="lblPublishStatus" runat="server" Text="-" Visible="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    <tr align="center">
                                                                        <td class="LblNoRecord" align="center" style="width: 800px;">
                                                                            <asp:Label ID="lblNoRecordFound" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>"
                                                                                runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                            <asp:HiddenField ID="hidTeacherId" runat="server" />
                                                            <asp:HiddenField ID="hidTermId" runat="server" />
                                                            <asp:HiddenField ID="hidEdited" runat="server" />
                                                            <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                                            <asp:ObjectDataSource TypeName="BusinessLogic.DescriptiveIndicatorBL" EnablePaging="True"
                                                                ID="ObjDSStudentDetails" runat="server" SelectMethod="GetAllStudentDetails" SelectCountMethod="GetCount"
                                                                EnableCaching="False">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                        Type="int32" />
                                                                    <asp:ControlParameter Name="aiStdDivId" ControlID="hidTeacherId" Type="Int32" PropertyName="Value" />
                                                                    <asp:ControlParameter Name="aiTermId" ControlID="hidTermId" Type="Int32" PropertyName="Value" />
                                                                    <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" Type="String"
                                                                        PropertyName="Value" />
                                                                    <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" Type="String"
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
                                                <asp:AsyncPostBackTrigger ControlID="cmbClassTeachers" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwStudentDetails" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="CmbTerm" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>                                    
                                    <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtn" CausesValidation="false"
                                        OnClick="btnPublish_Click" ViewStateMode="Enabled" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbClassTeachers" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="CmbTerm" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
