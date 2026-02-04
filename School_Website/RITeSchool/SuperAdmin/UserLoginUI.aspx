<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    CodeFile="UserLoginUI.aspx.cs" Inherits="UserLoginUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        &nbsp;<table width="97%">
            <tr id="trPrecondition" runat="server" visible="false">
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>           
            <tr runat="server" id="trCombo">
                <td align="left">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="width: 30%">
                                <table width="100%">
                                    <tr id="trUserRole" runat="server">
                                        <td class="ClsBorderlight" colspan="1" style="width: 15%;">
                                            <asp:Label ID="lblUserRole" runat="server" Text="User Role : " CssClass="ClsLabel"
                                                EnableViewState="False"></asp:Label></td>
                                        <td colspan="1" style="width: 20%;">
                                            <asp:DropDownList ID="ddlUserRole" runat="server" AutoPostBack="true" Width="132px"
                                                OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%">
                               <%-- <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                    ID="uPnl">
                                    <ContentTemplate>--%>
                                        <asp:Panel ID="pnlForStudent" runat="server" Visible="false" Width="100%">
                                            <table id="Table1" runat="server" width="100%">
                                                <tr>
                                                    <td align="center" class="ClsBorderlight">
                                                        <asp:Label ID="lblStandard" runat="server" Text="Standard : " CssClass="ClsLabel"></asp:Label></td>
                                                    <td align="left" >
                                                        <asp:DropDownList ID="ddlStandard" runat="server" Width="132px" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" class="ClsBorderlight">
                                                        <asp:Label ID="lblDivision" runat="server" Text="Division : " CssClass="ClsLabel"></asp:Label></td>
                                                    <td align="left">
                                                        <%--<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                            ID="UpdatePanel2">
                                                            <ContentTemplate>--%>
                                                                <asp:DropDownList ID="ddlDivision" runat="server" Width="122px" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            <%--</ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlUserRole" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" ChildrenAsTriggers="True" UpdateMode="Conditional"
                        runat="server">
                        <ContentTemplate>--%>
                            <asp:Panel ID="pnlUserGrid" runat="server">
                                <table runat="server" width="100%">
                                    <tr runat="server" id="trTotalRec" align="center">
                                        <td>
                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                            <asp:Label ID="lblTo" runat="server" Text=" To " CssClass="LblNormal" />
                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                            <asp:Label ID="lblOutOf" runat="server" Text=" Out Of " CssClass="LblNormal" />
                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                            <asp:Label ID="lblRecords" runat="server" Text="Records " CssClass="LblNormal" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top">
                                            <asp:GridView CssClass="GridBorder" ID="grdUsers" runat="server" AllowPaging="True"
                                                AutoGenerateColumns="False" OnRowCommand="grdUsers_RowCommand" AllowSorting="True"
                                                OnRowCreated="grdUsers_RowCreated" OnRowDataBound="grdUsers_RowDatabound" Width="100%"
                                                PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                DataKeyNames="User_Id,Is_Locked" OnSorting="grdUsers_Sorting" 
                                                OnPageIndexChanging="grdUsers_PageIndexChanging">
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                </PagerStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="Roll_No" HeaderText="Roll No." SortExpression="Roll_No">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="User_Login" HeaderText="User Name" SortExpression="User_Login">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField ButtonType="Button" HeaderText="Login" Text="Login"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" CommandName="LOGIN">
                                                        <ControlStyle CssClass="ClsBtnSml" />
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="True" />
                                                    </asp:ButtonField>
                                                   <%-- <asp:ButtonField ButtonType="Button" HeaderText="Login" Text="Login">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True" />
                                                    </asp:ButtonField>--%>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <RowStyle CssClass="ClsGridRow" />
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                <PagerTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                    OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </PagerTemplate>
                                            </asp:GridView>
                                            <asp:ObjectDataSource TypeName="BusinessLogic.SchoolUserCollectionBL" EnablePaging="true"
                                                ID="GrdDSobj" runat="server" SelectMethod="GetUserDetails" SortParameterName="sortExpression"
                                                SelectCountMethod="GetCountUsers" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                    <asp:ControlParameter Name="aiUserRoleId" Type="int32" ControlID="ddlUserRole" PropertyName="SelectedValue" />
                                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                        Type="int32" />
                                                    <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection"
                                                        PropertyName="Value" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdODStudent"
                                                runat="server" SelectMethod="GetAllCurrentStudents" SortParameterName="sortExpression"
                                                SelectCountMethod="CountCurrentStudentRows" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                        Type="string" />
                                                    <asp:ControlParameter ControlID="ddlStandard" Type="Int32" PropertyName="SelectedValue"
                                                        Name="aiStandardId" />
                                                    <asp:ControlParameter ControlID="ddlDivision" Type="Int32" PropertyName="SelectedValue"
                                                        Name="aiDivisionId" />
                                                    <asp:Parameter Name="asName" DefaultValue="" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <%--</ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlUserRole" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 20px">
                    <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="ClsBtn" OnClick="btnCancel_Click"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientUserDetailGridId = "<%=this.grdUsers.ClientID %>"
        function ConfirmLocking(str) {
            return window.confirm(str)
        }
    </script>
</asp:Content>
