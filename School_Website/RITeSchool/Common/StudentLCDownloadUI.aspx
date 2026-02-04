<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentLCDownloadUI.aspx.cs" Inherits="StudentLCDownloadUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td align="right" style="padding-right: 30px" valign="bottom">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                    Text="Mandatory Fields"></asp:Label>
            </td>
        </tr>
        <tr align="center">
            <td runat="server" id="td2" align="center" style="text-align: center; margin: 0px auto;">
                <table align="center" width="20%" style="text-align: center; margin: 0px auto;">
                    <tr align="center">
                        <td style="width: 150px;" class="ClsBorderlight">
                            <span class="ClsLabel" style="font-weight: bold">Standard :</span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                ViewStateMode="Enabled" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight">
                            <span class="ClsLabel" style="font-weight: bold">Division :</span>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="cmbDivision" CssClass="MidCombo" runat="server" AutoPostBack="true"
                                        EnableViewState="true" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight">
                            <span class="ClsLabel" style="font-weight: bold">Name :</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox" Width="200px" autocomplete="off"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" CausesValidation="false"
                                OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr id="trItemCount" runat="server">
                                <td align="center" style="width: 100%;">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentLCDetails"
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
                            <tr align="center" style="text-align: center; margin: 0px auto;">
                                <td align="center" style="text-align: center;">
                                    <asp:ListView ID="lstvwStudentLCDetails" runat="server" DataKeyNames="StudentId, LCNo"
                                        OnDataBound="lstvwStudentLCDetails_DataBound" OnItemCommand="lstvwStudentLCDetails_ItemCommand"
                                        OnItemDataBound="lstvwStudentLCDetails_ItemDataBound" OnSorting="lstvwStudentLCDetails_Sorting">
                                        <LayoutTemplate>
                                            <table align="center" style="text-align: center;" width="60%" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="center" class="paddingL" style="width: 70px; font-size: 10pt;">
                                                        <asp:Label ID="lblSrNo" runat="server" Text="Sr. No."> </asp:Label>
                                                    </th>
                                                    <th align="center" class="paddingL" style="width: 70px; font-size: 10pt;">
                                                        <asp:Label ID="lblEnrolmentNo" runat="server" Text="Reg. No."> </asp:Label>
                                                    </th>
                                                    <th align="center" class="paddingL" style="width: 130px; font-size: 10pt;">
                                                        <asp:LinkButton ID="lnlClassName" runat="server" CausesValidation="false" ForeColor="Black"
                                                            CommandArgument="ClassName" CommandName="SortRow">Class Name</asp:LinkButton>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd" width="250px" style="font-size: 10pt;">
                                                        <asp:Label ID="lblStudentName" runat="server" Text="Student Name"> </asp:Label>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd" style="font-size: 10pt;">
                                                        <asp:Label ID="lblFileName" runat="server" Text="File Name"> </asp:Label>
                                                    </th>
                                                    <th width="50px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                        <asp:Label ID="lblDownload" runat="server" Text="Download" ToolTip="Download"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="7" align="left">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwStudentLCDetails">
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
                                                    <asp:Label ID="lblSrNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("SrNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEnrolmentNo" runat="server" CssClass="clsLabelC" Text='<%#Eval("EnrollmentNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblClassName" runat="server" CssClass="clsLabelC" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblFileName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("LCFilePath") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidlc" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" CommandName="DownloadCommand"
                                                        ToolTip="Download" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td align="center">
                                                    <asp:Label ID="lblSrNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("SrNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEnrolmentNo" runat="server" CssClass="clsLabelC" Text='<%#Eval("EnrollmentNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblClassName" runat="server" CssClass="clsLabelC" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblFileName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("LCFilePath") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidlc" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" CommandName="DownloadCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table align="center" style="text-align: center;" width="50%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentLCUploadBL" EnablePaging="true"
                                        ID="lstvwDSobj" runat="server" SelectMethod="GetStudentLCDownload" SelectCountMethod="CountLCDownload"
                                        EnableCaching="false">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                Type="int32" />
                                            <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" PropertyName="Value" />
                                            <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" PropertyName="Value" />
                                            <asp:ControlParameter ControlID="cmbStandard" Type="String" PropertyName="SelectedValue"
                                                DefaultValue="0" Name="aiStandardId" />
                                            <asp:ControlParameter ControlID="cmbDivision" Type="String" PropertyName="SelectedValue"
                                                DefaultValue="0" Name="aiDivisionId" />
                                            <asp:ControlParameter Name="asFilter" ControlID="txtSearch" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwStudentLCDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        _clientListViewID = "<%=this.lstvwStudentLCDetails.ClientID %>";      

        function openfile(index) {
            var Path = document.getElementById(_clientListViewID + '_ctrl' + index + '_hidlc').value;
            window.open(Path);
        }        
    </script>
</asp:Content>
