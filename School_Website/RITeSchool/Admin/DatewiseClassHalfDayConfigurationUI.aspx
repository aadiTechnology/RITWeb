<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DatewiseClassHalfDayConfigurationUI.aspx.cs"
    Inherits="DatewiseClassHalfDayConfigurationUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" style="width: 80%;">
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table align="center">
                        <tr>
                            <td id="tdMessage" runat="server" align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstViewHalfDayStandardDivDetails" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table align="center">
                        <tr>
                            <td class="ClsBorderlight" style="width: 120px;" align="center">
                                <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="Date"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtHalfDayDate" CssClass="SmlCombo" runat="server"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_FormOpenDate" runat="server" Control="txtHalfDayDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" To-Today="true" AutoPostBack="False" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstViewHalfDayStandardDivDetails" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>"
                                CssClass="SubTitle" onclick="CheckAllCheckBox(this)" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="50%">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ListView ID="lstViewStdDivDetails" runat="server" DataKeyNames="StandardName, StandardId"
                                            OnItemDataBound="lstViewStdDivDetails_ItemDataBound">
                                            <LayoutTemplate>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstViewHalfDayStandardDivDetails" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                OnClick="btnSave_Click" UseSubmitBehavior="False" />
                            <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                CausesValidation="False" OnClick="btnCancel_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstViewHalfDayStandardDivDetails" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="90%">
                                <tr id="trItemCount" runat="server">
                                    <td align="center" style="width: 100%;">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstViewHalfDayStandardDivDetails"
                                            Visible="true">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                            Text="<%# Container.StartRowIndex + 1%>" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                            Text=" To " />
                                                        <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                            Text=" Out Of " />
                                                        <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                            Text="Records " />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListView ID="lstViewHalfDayStandardDivDetails" runat="server" DataKeyNames="HalfDayDetailsId"
                                            OnItemCommand="lstViewHalfDayStandardDivDetails_ItemCommand" OnItemDeleting="lstViewHalfDayStandardDivDetails_ItemDeleting"
                                            OnItemEditing="lstViewHalfDayStandardDivDetails_ItemEditing" OnItemDataBound="lstViewHalfDayStandardDivDetails_ItemDataBound"
                                            OnDataBound="lstViewHalfDayStandardDivDetails_DataBound" 
                                            onsorting="lstViewHalfDayStandardDivDetails_Sorting">
                                            <LayoutTemplate>
                                                <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="center" width="100px" class="clsLabelgrd">
                                                            <asp:LinkButton ID="lnkDate" runat="server" CausesValidation="false" ForeColor="Black"
                                                                CommandArgument="HalfDayDate" CommandName="SortRow">Date</asp:LinkButton>
                                                        </th>
                                                        <th align="left" class="clsLabelgrd">
                                                            <span><b>Classes</b></span>
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
                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                        <td colspan="4" align="left">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstViewHalfDayStandardDivDetails">
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
                                                    <td align="center">
                                                        <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text='<%#Eval("HalfDayDate") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                            Text='<%#Eval("ClassName") %>'></asp:Label>
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
                                                        <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text='<%#Eval("HalfDayDate") %>'></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                            Text='<%#Eval("ClassName") %>'></asp:Label>
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
                                        <asp:HiddenField ID="hidDivisionIds" runat="server" Value="" />
                                        <asp:HiddenField ID="hidFirstFxFollowingErrors" runat="server" Value="" />
                                        <asp:HiddenField ID="hidHalfDayDate" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                        <asp:ObjectDataSource TypeName="BusinessLogic.SchoolWorkinDetailsBL" EnablePaging="true"
                                            ID="lstvwDSobj" runat="server" SelectMethod="Get" SelectCountMethod="CountTotalConfiguration" SortParameterName="asSortExpression"
                                            EnableCaching="false">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="int32" />
                                                <asp:ControlParameter ControlID="hidSortExpression" PropertyName="Value" Name="asSortExpression" />
                                                <asp:ControlParameter ControlID="hidSortDirection" PropertyName="Value" Name="asSortDirection" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                            </td>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstViewHalfDayStandardDivDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                        PostBackUrl="~/RITeSchool/Admin/schoolconfigurationcontrolpanel.aspx" />
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">
            _ClientListValue = "<%=this.lstViewStdDivDetails.ClientID %>"
            _ClienthidDivisionIds = "<%=this.hidDivisionIds.ClientID  %>"
            _clientchkAllId = "<%=this.chkAll.ClientID %>"

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?');
            }

            function CheckSaveCheckbox() {
                var bRetRow = CheckAllRowCheckBox();
                var msgHeader = document.getElementById("<%=hidFirstFxFollowingErrors.ClientID %>").value

                if (bRetRow == false) {
                    alert(msgHeader + "\n" + "Please select at least one class.");
                    return false;
                }
                return true;
            }

            function CheckAllRowCheckBox() {
                var lbl
                var iRowCount = 0
                lbl = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_lblStandard")
                var sDivisionIds = document.getElementById(_ClienthidDivisionIds).value;
                var DivArr = [];
                DivArr = sDivisionIds.split(',')
                var sValue = false;
                while (lbl != null) {
                    for (var i = 0; i < DivArr.length; i++) {
                        var chk = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_Chk_" + DivArr[i])
                        if (chk != null && chk.checked) {
                            sValue = true;
                            break;
                        }
                    }
                    if (sValue == true) {
                        return true;
                        break;
                    }
                    iRowCount++;
                    lbl = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_lblStandard")
                }
                return false;
            }

            function CheckHeaderCheckbox(obj, DivId) {
                var iRowCount = 0
                var chk = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_Chk_" + DivId)
                while (chk != null) {
                    if (obj.checked)
                        chk.checked = true;
                    else
                        chk.checked = false;

                    iRowCount++;
                    var chk = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_Chk_" + DivId)
                }
            }

            function CheckStdHeaderCheckbox(obj, iRowIndex) {
                var sDivisionIds = document.getElementById(_ClienthidDivisionIds).value;
                var DivArr = [];
                DivArr = sDivisionIds.split(',')
                for (var i = 0; i < DivArr.length; i++) {
                    var chk = document.getElementById(_ClientListValue + "_ctrl" + iRowIndex + "_Chk_" + DivArr[i])
                    if (obj.checked)
                        chk.checked = true;
                    else
                        chk.checked = false;
                }
            }

            function CheckAllCheckBox(obj) {
                var lbl
                var sDivisionIds = document.getElementById(_ClienthidDivisionIds).value;
                var DivArr = [];
                DivArr = sDivisionIds.split(',')
                var iRowCount = 0;
                lbl = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_lblStandard")
                while (lbl != null) {
                    var DivArr = [];
                    DivArr = sDivisionIds.split(',')
                    for (var i = 0; i < DivArr.length; i++) {
                        var chk = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_Chk_" + DivArr[i])
                        if (chk != null) {
                            if (obj.checked)
                                chk.checked = true;
                            else
                                chk.checked = false;
                        }
                    }
                    iRowCount++;
                    lbl = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_lblStandard")
                }
            }
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
