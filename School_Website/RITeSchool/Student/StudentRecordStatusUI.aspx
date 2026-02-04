<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentRecordStatusUI.aspx.cs" Inherits="StudentRecordStatusUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel">Class(s) : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbClasses" runat="server" CssClass="LrgCombo" Style="width: 260px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel">Registration Number / Name : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="ClsLabel">Show only Rise and Shine Students : </span>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkIncludeRiseAndShine" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr class="height20">
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
           <%-- <tr runat="server" id="trTotalRec" align="center">
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudents">
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>--%>
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr runat="server" id="trTotalRec" align="center">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudents">
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
                                                <td align="center">
                                                    <asp:ListView ID="lstvwStudents" runat="server" OnSorting="lstvwStudents_Sorting"
                                                        OnDataBound="lstvwStudents_DataBound" OnItemDataBound="lstvwStudents_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th style="width: 150px">
                                                                        <asp:LinkButton ID="lnkEnrlNo" runat="server" CommandName="Sort" CommandArgument="Enrolment_Number"
                                                                            CssClass="clsLabel" CausesValidation="false" ForeColor="Black"> Registration Number</asp:LinkButton>
                                                                    </th>
                                                                    <th style="width: 100px;">
                                                                        <asp:LinkButton ID="lnkRollNo" runat="server" CommandName="Sort" CommandArgument="Roll_No"
                                                                            Style="float: right" CssClass="ClsLabelR" CausesValidation="false" ForeColor="Black"> Roll No.</asp:LinkButton>
                                                                    </th>
                                                                    <th style="width: 150px">
                                                                        <asp:LinkButton ID="lnkClass" runat="server" CommandName="Sort" CommandArgument="className"
                                                                            CssClass="clsLabel" CausesValidation="false" ForeColor="Black"> Class</asp:LinkButton>
                                                                    </th>
                                                                    <th>
                                                                        <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="StudentName"
                                                                            CssClass="clsLabel" CausesValidation="false" ForeColor="Black"> Name</asp:LinkButton>
                                                                    </th>
                                                                    <th style="width: 250px" align="center">
                                                                        <span class="ClsLabel" style="float: inherit">Action For Me</span>
                                                                    </th>
                                                                    <%--<th style="width: 150px" align="center">
                                                                        <span class="ClsLabel" style="float: inherit">Read By Counsellor?</span>
                                                                    </th>--%>
                                                                    <th width="50px" align="center" class="clsLabelgrd">
                                                                        <asp:Label ID="lblEdit" runat="server" Text="Action"> </asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                    <td colspan="6">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudents" PageSize="20">
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
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td>
                                                                    <asp:Label ID="lblRegNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("RegNo") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRollNo" runat="server" CssClass="ClsLabel" Style="float: right;
                                                                        padding-right: 5px;" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblClass" runat="server" CssClass="ClsLabel" Text='<%#Eval("Class") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <%--<asp:Image ID="imgPrincipalStatus" runat="server" />--%>
                                                                    <asp:Label ID="lblAction" runat="server" Text="" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                                </td>
                                                                <%--<td align="center">
                                                                    <asp:Image ID="imgCounsellorStatus" runat="server" />
                                                                    <asp:Label ID="lblCounsellorStatus" runat="server" Text="" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                                </td>--%>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlnkEdit" runat="server" ImageUrl=""></asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td>
                                                                    <asp:Label ID="lblRegNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("RegNo") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRollNo" runat="server" CssClass="ClsLabel" Style="float: right;
                                                                        padding-right: 5px;" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblClass" runat="server" CssClass="ClsLabel" Text='<%#Eval("Class") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                                </td>
                                                               <td align="center">
                                                                    <%--<asp:Image ID="imgPrincipalStatus" runat="server" />--%>
                                                                    <asp:Label ID="lblAction" runat="server" Text="" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                                </td>
                                                               <%-- <td align="center">
                                                                    <asp:Image ID="imgCounsellorStatus" runat="server" />
                                                                    <asp:Label ID="lblCounsellorStatus" runat="server" Text="" CssClass="ClsLabel" style="float:inherit"></asp:Label>
                                                                </td>--%>
                                                                <td align="center">
                                                                    <asp:HyperLink ID="hlnkEdit" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"></asp:HyperLink>
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
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.StudentRecordBL" EnablePaging="True"
                                            ID="objdsStudents" runat="server" SelectMethod="GetAllStudentStatus" SortParameterName="asSortExpression"
                                            SelectCountMethod="GetAllStudentCount" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="string" />
                                                <asp:SessionParameter Name="aiUserId" SessionField="I_USER_ID" Type="int32" />
                                                <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                <asp:ControlParameter ControlID="cmbClasses" Name="aiStdDivId" Type="Int32" PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="hidIncludeRiseAndShine" Name="asIncludeRiseAndShinde" Type="String" PropertyName="Value" />
                                                <asp:ControlParameter ControlID="hidShowOnlysavedRecords" Name="asShowSaved" Type="String"
                                                    PropertyName="Value" />
                                                <asp:ControlParameter ControlID="hidUserHasEditAccess" Name="asHasEditAccess" Type="String"
                                                    PropertyName="Value" />
                                                <asp:ControlParameter ControlID="hidSortExpression" Name="asSortExpression" Type="String"
                                                    PropertyName="Value" />
                                                <asp:ControlParameter ControlID="hidSortDirection" Name="asSortDirection" Type="String"
                                                    PropertyName="Value" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                        <asp:HiddenField ID="hidUserHasEditAccess" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidPageNo" runat="server" />
                                        <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                                        <asp:HiddenField ID="hidIncludeRiseAndShine" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidShowOnlysavedRecords" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidAssociatedClassId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsPrincipal" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsCounsellor" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsSubjectTeacher" runat="server" Value="0" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwStudents" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                            <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 1 :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                            <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Principal and Counsellor can see those students to whom details are submitted by class teacher(s)."></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                            <asp:Label ID="Label1" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 2 :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                            <asp:Label ID="Label2" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If Principal or Counsellor is a class teacher of any class then on selection of same class, he / she can see all students to whom details of selected class."></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                            <asp:Label ID="Label3" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 3 :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                            <asp:Label ID="Label4" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Status column will show unread, unsubmitted student records and comments."></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
