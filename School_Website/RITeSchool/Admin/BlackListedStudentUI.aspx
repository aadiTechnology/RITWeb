<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BlackListedStudentUI.aspx.cs" Inherits="BlackListedStudentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div>
            <table width="100%">
                <tr>
                    <td align="center">
                        <table width="80%">
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="Upnl" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblUpdate" runat="server" ForeColor="Blue" EnableViewState="false"
                                                Font-Bold="true"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnSearch" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lstvwBlackListedStudents" EventName="ItemCommand" />
                                            <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="lstvwBlackListedStudents" EventName="Sorting" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <table>
                                        <tr>
                                            <td align="center" width="155px;" class="ClsBorderlight">
                                                <asp:Label ID="lblStudentName" runat="server" Text="Reg. No. / Name" CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" MaxLength="100" autocomplete="off"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" width="155px;" class="ClsBorderlight">
                                                <asp:Label ID="Label1" runat="server" Text="Show all left students" CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="BtnSearch" runat="server" CssClass="ClsBtn" Text="Search" CausesValidation="false"
                                                    OnClick="BtnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table align="center" width="95%">
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table align="center" width="90%">
                                                <tr runat="server" id="trTotalRec" align="center">
                                                    <td align="center">
                                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwBlackListedStudents">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1 %>" />
                                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                        <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize %>" />
                                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                        <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount %>" />
                                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                        <br />
                                                                    </PagerTemplate>
                                                                </asp:TemplatePagerField>
                                                            </Fields>
                                                        </asp:DataPager>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListView ID="lstvwBlackListedStudents" runat="server" DataKeyNames="SchoolwiseStudentId,Id"
                                                            OnItemCommand="lstvwBlackListedStudents_ItemCommand" OnSorting="lstvwBlackListedStudents_Sorting"
                                                            OnDataBound="lstvwBlackListedStudents_DataBound" OnItemDataBound="lstvwBlackListedStudents_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table id="lstvwtable" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                                    class="GridBorder" width="100%">
                                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                        <th align="left" width="150px">
                                                                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="ClsLabel" CommandName="Sort"
                                                                                CommandArgument="Enrolment_Number" CausesValidation="false" ForeColor="Black"
                                                                                Text="Enrolment No."></asp:LinkButton>
                                                                        </th>
                                                                        <th align="left" width="250px">
                                                                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="ClsLabel" CommandName="Sort"
                                                                                CommandArgument="StudentName" CausesValidation="false" ForeColor="Black" Text="Student Name"></asp:LinkButton>
                                                                        </th>
                                                                        <th align="center" width="100px">
                                                                            <asp:LinkButton ID="lnkLeftDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                CommandName="Sort" CommandArgument="SchoolLeft_Date" CausesValidation="false"
                                                                                ForeColor="Black" Text="Left Date"></asp:LinkButton>
                                                                        </th>
                                                                        <th align="center">
                                                                            <asp:Label ID="lblComment" runat="server" CssClass="clsLabel" Text="Comment"></asp:Label>
                                                                        </th>
                                                                        <th align="center" style="width: 100px;">
                                                                            Update
                                                                        </th>
                                                                        <th align="center" style="width: 100px;">
                                                                            Add
                                                                        </th>
                                                                        <th align="center" style="width: 100px;">
                                                                            Remove
                                                                        </th>
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                    <tr id="trDataPager" class="ClsBorderPager">
                                                                        <td colspan="7">
                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwBlackListedStudents"
                                                                                PageSize="20">
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
                                                                <tr id="trItemtemplate" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblEnrolment1" runat="server" CssClass="clsLabel" Text='<%# Eval("EnrolmentNumber") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblStudentName1" runat="server" CssClass="clsLabel" Text='<%# Eval("StudentName") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblLeftDate1" runat="server" CssClass="clsLabel" Style="float: inherit"
                                                                            Text='<%# Eval("SchoolLeftDate") %>'>
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="LrgTxtBox" MaxLength="500" Width="98%"
                                                                            Style="margin-left: 5px;" Text='<%# Eval("Comment") %>'></asp:TextBox>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CssClass="clsLabel" CommandName="UPDATESTUDENT"
                                                                            Style="float: inherit">Update</asp:LinkButton>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:LinkButton ID="lnkAdd" runat="server" CssClass="clsLabel" CommandName="ADD"
                                                                            Style="float: inherit">Add</asp:LinkButton>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:LinkButton ID="lnkRemove" runat="server" CssClass="clsLabel" CommandName="REMOVE"
                                                                            Style="float: inherit">Remove</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                        <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                        <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="objdsStudentList"
                                                            runat="server" SelectMethod="GetAllBlackListedStudents" SortParameterName="SortExpression"
                                                            SelectCountMethod="GetBlackListedStudentsCount" EnableCaching="false">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                <asp:ControlParameter ControlID="txtName" Name="asFilter" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="chkAll" Name="abShowAllLeft" PropertyName="Checked" />
                                                                <asp:ControlParameter ControlID="hidSortExpression" Name="SortExpression" Type="String"
                                                                    PropertyName="Value" />
                                                                <asp:ControlParameter ControlID="hidSortDirection" Name="SortDirection" Type="String"
                                                                    PropertyName="Value" />
                                                                <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                                <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hidSchoolwiseStudentId" runat="server" />
                                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnSearch" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lstvwBlackListedStudents" EventName="ItemCommand" />
                                            <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="lstvwBlackListedStudents" EventName="Sorting" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="BtnBack" runat="server" CssClass="ClsBtn" Text="Back" CausesValidation="false" Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
