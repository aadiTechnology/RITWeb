<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LessonPlanUI.aspx.cs" Inherits="LessonPlanUI"
    ViewStateMode="Enabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        .ClsLabel
        {
            font-family: Open Sans;
        }
        
        .ClsRemarkLabel
        {
            font-size: 14px;
            font-family: Arial;
        }
    </style>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr>
                <td align="right">
                    <span class="ClsMdtStar">* Mandatory Fields</span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="80%">
                                <tr>
                                    <td align="center" id="tdMessage" runat="server" viewstatemode="Enabled">
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:AsyncPostBackTrigger ControlID="lstvwLessonPlan" EventName="ItemCommand" />                            
                            <asp:AsyncPostBackTrigger ControlID="cmbTeacher" EventName="SelectedIndexChanged" />--%>
                            <asp:AsyncPostBackTrigger ControlID="calEndDate" EventName="SelectionChanged" />
                            <asp:AsyncPostBackTrigger ControlID="calStartDate" EventName="SelectionChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left" class="ClsBorderlight" id="tdTeacherHeader" runat="server" viewstatemode="Enabled">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Teacher : "></asp:Label>
                            </td>
                            <td align="left" id="tdTeacher" runat="server" viewstatemode="Enabled">
                                <asp:DropDownList ID="cmbTeacher" runat="server" ViewStateMode="Enabled" TabIndex="1"
                                    CssClass="LrgCombo" OnSelectedIndexChanged="cmbTeacher_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table style="margin-top: 10px !important">
                        <tr>
                            <td align="left" class="ClsBorderlight" id="tdStartDateHeader" runat="server" viewstatemode="Enabled">
                                <asp:Label ID="Label8" runat="server" CssClass="ClsLabel" Text="Start Date : "></asp:Label>
                            </td>
                            <td align="left" id="tdStartDate" runat="server" viewstatemode="Enabled">
                                <asp:TextBox ID="txtStartDate" TabIndex="2" runat="server" ViewStateMode="Enabled"
                                    MaxLength="12" CssClass="SmlTxtBox"></asp:TextBox>
                                <rjs:PopCalendar ID="calStartDate" runat="server" ViewStateMode="Enabled" Control="txtStartDate"
                                    AutoPostBack="true" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false"
                                    OnSelectionChanged="calDates_SelectionChanged" />
                            </td>
                            <td style="width: 280px">
                            </td>
                            <td align="left" class="ClsBorderlight" id="tdEndDateHeader" runat="server" viewstatemode="Enabled">
                                <asp:Label ID="Label9" runat="server" CssClass="ClsLabel" Text="End Date : "></asp:Label>
                            </td>
                            <td align="left" id="tdEndDate" runat="server" viewstatemode="Enabled">
                                <asp:TextBox ID="txtEndDate" TabIndex="3" runat="server" ViewStateMode="Enabled"
                                    MaxLength="12" CssClass="SmlTxtBox"></asp:TextBox>
                                <%--<asp:CompareValidator ID = "cmpEndDate" runat = "server" ControlToValidate = "txtEndDate" ControlToCompare = "txtStartDate" Operator = "GreaterThanEqual" Display = "None" ErrorMessage = "End Date should be greater than Start date."></asp:CompareValidator>--%>
                                <rjs:PopCalendar ID="calEndDate" runat="server" ViewStateMode="Enabled" Control="txtEndDate"
                                    AutoPostBack="true" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false"
                                    OnSelectionChanged="calDates_SelectionChanged" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="Height10">
                <td align="center">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnlLessons" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="80%">
                                <tr>
                                    <td align="center">
                                        <table id="LegendTable" runat="server" align="center">
                                            <tr>
                                                <td align="center" rowspan="3" style="padding-right: 5px">
                                                    <asp:Label ID="lblLegend" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                        EnableViewState="false" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                                    <span class="ClsLblLgnd">:</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    &nbsp;
                                                    <asp:Image ID="lblSubmitImg" CssClass="img-align-unset" runat="server" EnableViewState="False"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblSubmit" runat="server" CssClass="ClsTextNormal" EnableViewState="False"
                                                        Font-Bold="True" Text="Submited"></asp:Label>
                                                </td>
                                                <td style="width: 5px;">
                                                </td>
                                                <td align="left">
                                                    <asp:Image ID="lblDeleteImg" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblNonSumbited" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                        Font-Bold="True" Text="Non Submited"> </asp:Label>
                                                </td>
                                                <td style="width: 5px;">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblNotApplicabelImg" Font-Size="16px" ForeColor="GrayText" runat="server"
                                                        CssClass="ClsTextNormal" EnableViewState="false" Text="-"> </asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblNotApplicabel" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                        Font-Bold="True" Text="Not Applicable"> </asp:Label>
                                                </td>
                                                <td style="width: 5px;">
                                                </td>
                                                <td align="left" class="ClsBorderlight" id="tdSuggestion" runat="server" viewstatemode="Enabled">
                                                    <asp:Label ID="lblSuggistionAdded" runat="server" CssClass="ClsLabel" ForeColor ="Blue" Font-Bold="true" Text="Suggestion Added"></asp:Label>
                                                </td>
                                                <td align="left">
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataPager ID="DtPgCount" runat="server" ViewStateMode="Enabled" PageSize="20"
                                            PagedControlID="lstvwLessonPlan">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ViewStateMode="Enabled" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" ViewStateMode="Enabled" CssClass="LblNormal"
                                                            Text=" To " />
                                                        <asp:Label runat="server" ViewStateMode="Enabled" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" ViewStateMode="Enabled" CssClass="LblNormal"
                                                            Text=" Out Of " />
                                                        <asp:Label runat="server" ViewStateMode="Enabled" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" ViewStateMode="Enabled" CssClass="LblNormal"
                                                            Text="Records " />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                        <asp:ListView ID="lstvwLessonPlan" runat="server" ViewStateMode="Enabled" OnDataBound="lstvwLessonPlan_DataBound"
                                            OnItemDataBound="lstvwLessonPlan_ItemDataBound" OnItemCommand="lstvwLessonPlan_ItemCommand">
                                            <LayoutTemplate>
                                                <table width="80%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="center" style="width: 20%">
                                                            <asp:Label ID="Label4" runat="server" Text="Start Date" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                        </th>
                                                        <th align="center" style="width: 20%">
                                                            <asp:Label ID="Label5" runat="server" Text="End Date" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                        </th>
                                                        <th align="center" style="width: 15%">
                                                            <asp:Label ID="Label3" runat="server" Text="View Remark" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                        </th>
                                                        <th width="50px" align="center" style="width: 50px" id="thEdit" runat="server">
                                                            <asp:Label ID="lblEdit" runat="server" ViewStateMode="Enabled" Text="Edit"> </asp:Label>
                                                        </th>
                                                        <th width="50px" style="width: 50px" id="thDelete" runat="server" viewstatemode="Enabled">
                                                            <asp:Label ID="lblDelete" runat="server" ViewStateMode="Enabled" Text="Delete"> </asp:Label>
                                                        </th>
                                                        <th width="50px" style="width: 50px" id="thView" runat="server" viewstatemode="Enabled">
                                                            <asp:Label ID="Label6" runat="server" Text="View"> </asp:Label>
                                                        </th>
                                                        <th width="50px" style="width: 20px">
                                                            <asp:Label ID="Label2" runat="server" Text="Export"> </asp:Label>
                                                        </th>
                                                        <th align="left" class="PaddingL-10" width="80px" id="thStatus" runat="server">
                                                            <asp:Label ID="Label7" runat="server" Text="Submit Status"> </asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                        <td colspan="8">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" ViewStateMode="Enabled" PagedControlID="lstvwLessonPlan"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" ViewStateMode="Enabled"
                                                                                            CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" ViewStateMode="Enabled" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td align="right" class="LblNormal">
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" ViewStateMode="Enabled" CssClass="LblNormal" />
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
                                                        <asp:Label ID="lblStartDate" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel"
                                                            Text='<%#Eval("StartDate") %>' Style="float: inherit"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblEndDate" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel"
                                                            Style="float: inherit" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnViewRemarks" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            CommandName="VIEWRemark" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif"
                                                            ToolTip="View Remark" Visible="false" />
                                                        <asp:Label ID="lblRemarks" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel"
                                                            Text="-" Style="text-align: center; width: 100%;"></asp:Label>
                                                    </td>
                                                    <td align="center" id="tdEdit" runat="server" viewstatemode="Enabled">
                                                        <asp:ImageButton ID="btnEdit" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            CommandName="UpdateCommand" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center" id="tdDelete" runat="server" viewstatemode="Enabled">
                                                        <asp:ImageButton ID="btnDelete" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            CommandName="RemoveCommand" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                    <td align="center" id="tdView" runat="server" viewstatemode="Enabled">
                                                        <asp:ImageButton ID="btnView" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            CommandName="VIEW" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif"
                                                            ToolTip="View" />
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Button ID="btnExport" runat="server" Text=".." style="width:15px" CssClass="ClsBtn" CommandName="EXPORT" />--%>
                                                        <asp:LinkButton ID="lbtnExport" runat="server" ViewStateMode="Enabled" Text="Export"
                                                            CommandName="EXPORT"></asp:LinkButton>
                                                    </td>
                                                    <td align="left" id="tdStatus" runat="server" viewstatemode="Enabled" class="PaddingL-10">
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:Label ID="lblStartDate" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel"
                                                            Text='<%#Eval("StartDate") %>' Style="float: inherit"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblEndDate" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel"
                                                            Style="float: inherit" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnViewRemarks" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            CommandName="VIEWRemark" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif"
                                                            ToolTip="View Remark" Visible="false" />
                                                        <asp:Label ID="lblRemarks" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel"
                                                            Text="-" Style="text-align: center; width: 100%;"></asp:Label>
                                                    </td>
                                                    <td align="center" id="tdEdit" runat="server" viewstatemode="Enabled">
                                                        <asp:ImageButton ID="btnEdit" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            CommandName="UpdateCommand" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center" id="tdDelete" runat="server" viewstatemode="Enabled">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                    <td align="center" id="tdView" runat="server" viewstatemode="Enabled">
                                                        <asp:ImageButton ID="btnView" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            CommandName="VIEW" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif"
                                                            ToolTip="View" />
                                                    </td>
                                                    <td align="center">
                                                        <%--<asp:Button ID="btnExport" runat="server" Text=".." style="width:15px" CssClass="ClsBtn" CommandName="EXPORT" />--%>
                                                        <asp:LinkButton ID="lbtnExport" runat="server" ViewStateMode="Enabled" Text="Export"
                                                            CommandName="EXPORT"></asp:LinkButton>
                                                    </td>
                                                    <td align="left" id="tdStatus" runat="server" viewstatemode="Enabled" class="PaddingL-10">
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center" colspan="8">
                                                        <asp:Label ID="lblNoRecFound" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.LessonPlanDetailsBL" EnablePaging="True"
                                            ID="objdsLessonPlan" runat="server" ViewStateMode="Enabled" SelectMethod="GetAllConfigs"
                                            SortParameterName="asSortExpression" SelectCountMethod="GetAllConfigsCount" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="int32" />
                                                <asp:SessionParameter Name="aiReportingUserId" SessionField="I_USER_ID" Type="int32" />
                                                <asp:ControlParameter ControlID="cmbTeacher" Type="Int32" Name="aiUserId" PropertyName="SelectedValue" />
                                                <asp:Parameter Name="asSortExpression" Type="String" />
                                                <asp:Parameter Name="asSortDirection" Type="String" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                <asp:ControlParameter ControlID="txtStartDate" Name="StartDate" Type="string" PropertyName="Text" />
                                                <asp:ControlParameter ControlID="txtEndDate" Name="EndDate" Type="string" PropertyName="Text" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidPageNo" runat="server" ViewStateMode="Enabled" Value="0" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="calEndDate" EventName="SelectionChanged" />
                            <asp:AsyncPostBackTrigger ControlID="calStartDate" EventName="SelectionChanged" />
                            <asp:PostBackTrigger ControlID="lstvwLessonPlan" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTeacher" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnExport" CssClass="ClsBtn" runat="server" Text="Export All" OnClick="btnExport_Click"
                                CausesValidation="false" />
                        </ContentTemplate>
                        <Triggers>                          
                            <asp:AsyncPostBackTrigger ControlID="calEndDate" EventName="SelectionChanged" />
                            <asp:AsyncPostBackTrigger ControlID="calStartDate" EventName="SelectionChanged" />
                            <asp:PostBackTrigger ControlID ="btnExport" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTeacher" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnAdd" runat="server" ViewStateMode="Enabled" Text="Add" CssClass="ClsBtn"
                                OnClick="btnAdd_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbTeacher" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>

    <div id="divTemplates" runat="server" style="visibility: hidden; display: none; position: fixed;
        margin: 0px; padding: 0px; width: 440px; height: auto; border-width: 1px; left: 5px;
        top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
        background-color: white;" viewstatemode="Enabled">
        <div class="StudentWiseRemarkMasterPop">
            <div style="font-size: 15px; width: 350px; letter-spacing: 1px; padding-left: 8px;
                font-weight: bolder; color: darkgreen; float: left; height: 10px" align="left">
                View Remarks :-
            </div>
            <span style="cursor: hand; float: right;" onclick="javascript:HidePopup();">
                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif" border="0" />
            </span>
        </div>
        <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
            color: #333; overflow: auto; height: auto; width: 435px; margin-left: 1px" id="Div5">
            <asp:Label ID="lblAllRemarks" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel"
                Style="float: left; font-family: Verdana; font-size: 13px; padding-top: 15px;
                padding-bottom: 15px;" Text="Test Message"></asp:Label>
        </div>
    </div>
    <script lang="javascript" type="text/javascript">
        _cltxtStartDate = "<%=this.txtStartDate.ClientID %>";
        _cltxtEndDate = "<%=this.txtEndDate.ClientID %>";

        function ConfirmDelete() {
            return window.confirm('Are you sure you want to delete this record?')
        }

        function CheckForSuggisition(msg, IsSuggisitionAdded, IsSuggisitionRead, TeacherId, dtStartDate, dtEndDate, miSchoolId, miAcademicYearId, miUpdatedById) {
            if (IsSuggisitionAdded == "True" && IsSuggisitionRead == "False") {
                $.ajax({
                    type: "POST",
                    data: '{"aiTeacherId":"' + TeacherId + '","adtStartDate":"' + dtStartDate + '","adtEndDate":"' + dtEndDate + '","aiSchoolId":"' + miSchoolId + '","aiAcademicYearId":"' + miAcademicYearId + '","aiUpdatedById":"' + miUpdatedById + '" }',
                    url: "LessonPlanUI.aspx/UpdateStatus",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                    });
            }
            OpenPopup(msg);
            return false;           
         }

         function OpenPopup(msg) {          
            _clientdivTemplates = "<%=this.divTemplates.ClientID %>"
            var x, y
            var cssstyle = $get("<%=this.divTemplates.ClientID %>").style
            var width = 350
            var height = 150
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"

            $get("<%=this.lblAllRemarks.ClientID %>").innerHTML = msg

            setTotal();
        }

        function HidePopup() {
            $get("<%=this.divTemplates.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divTemplates.ClientID %>").style.display = "none"
            return false
        }

    </script>
    <script lang="javascript" type="text/javascript">

        _cltdivTemplates = "<%=this.divTemplates.ClientID %>"

        var _totalWinHeight;
        var _adjWinHeight;
        var _rightFooterPos;
        var _bottomFooterPos;
        var _adjWinWidth;
        var _rightPosition;

        window.onresize = setTotal;
        window.onscroll = setTotal;
        window.onload = setTotal;

        function setTotal() {
            _totalWinHeight = document.body.scrollHeight;
            _adjWinHeight = _totalWinHeight; //-608;
            _adjWinWidth = document.body.scrollWidth;

            if (document.getElementById(_cltdivTemplates) != null) {
                _rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltdivTemplates).style.height);
                document.getElementById(_cltdivTemplates).style.top = _rightFooterPos;
            }

            if (document.getElementById(_cltdivTemplates) != null) {
                _rightPosition = parseInt(screen.width / 2) - parseInt(parseInt(document.getElementById(_cltdivTemplates).style.width) / 2);
                document.getElementById(_cltdivTemplates).style.left = _rightPosition;
            }
            window_onscroll();
        }

        function window_onscroll() {
            if (document.body.scrollTop <= _adjWinHeight) {
                if (document.getElementById(_cltdivTemplates) != null) {
                    document.getElementById(_cltdivTemplates).style.top = document.body.scrollTop + _rightFooterPos;
                }
            }

            if (document.body.scrollLeft <= _adjWinWidth) {
                if (document.getElementById(_cltdivTemplates) != null) {
                    document.getElementById(_cltdivTemplates).style.left = document.body.scrollLeft + _rightPosition;
                }
            }
        }      
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
