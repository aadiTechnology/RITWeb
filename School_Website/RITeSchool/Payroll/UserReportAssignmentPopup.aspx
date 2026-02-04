<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="UserReportAssignmentPopup.aspx.cs" Inherits="UserReportAssignmentPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align:top;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="left" valign="top" style="height: 20px">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="ClsGrayMainTitle" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="center" class="MainTitleHead">
                                            <span style="font-weight: bold">Report Assignment</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" style="height: 10px">
                    <table width="100%">
                        <tr>
                            <td align="left" width="50%">
                                <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true"
                                    ValidationGroup="Search" />
                                <asp:ValidationSummary ID="valSumSave" runat="server" CssClass="LblErrorMsg" ShowSummary="true"
                                    ValidationGroup="Save" />
                            </td>
                            <td width="50%">
                                <div style="float: right;">
                                    <span class="ClsMdtStar">* Mandatory Fields </span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <table width="50%">
                        <tr style="height:10px;">
                            <td align="center" colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMassage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwUsers" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="50%" class="ClsBorderlight">
                                <span class="ClsLabel">Report : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbReport" runat="server" CssClass="LrgCombo">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                                <asp:RequiredFieldValidator ID="reqReport" runat="server" Display="None" ControlToValidate="cmbReport"
                                    ValidationGroup="Search" InitialValue="0" ErrorMessage="Report should be selected."></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">User Role : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="LrgCombo">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                                <asp:RequiredFieldValidator ID="reqUserRole" runat="server" Display="None" ControlToValidate="cmbUserRole"
                                    ValidationGroup="Search" InitialValue="0" ErrorMessage="User Role should be selected."></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight">
                                <span class="ClsLabel">Name : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFilter" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClick="btnSearch_Click"
                                    ValidationGroup="Search" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 5px;">
                <td>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr runat="server" id="trTotalRec" align="center">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwUsers">
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
                                    <td valign="top" align="center">
                                        <asp:ListView ID="lstvwUsers" runat="server" OnDataBound="lstvwUsers_DataBound" OnItemDataBound="lstvwUsers_ItemDataBound"
                                            DataKeyNames="UserId,ReportUserDetailId" OnSorting="lstvwUsers_Sorting">
                                            <LayoutTemplate>
                                                <table width="80%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                    cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" width="30px">
                                                            <asp:CheckBox ID="chkAll" runat="server" onclick="SelectAll(this)" />
                                                        </th>
                                                        <th align="left" style="padding-left: 15px;">
                                                            <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="UserName"
                                                                CausesValidation="false" ForeColor="Black"> Name </asp:LinkButton>
                                                        </th>
                                                        <th align="center" class="clsLabelgrd" width="130px">
                                                            Has Full Access?
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                        <td colspan="3" align="left">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwUsers" PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged"
                                                                                            onchange="CheckPageIndexChange(this)">
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
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </td>
                                                    <td align="left" style="padding-left: 10px;">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkHasFullAccess" runat="server" />
                                                        <asp:HiddenField ID="hidIsViewApplicable" runat="server" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </td>
                                                    <td align="left" style="padding-left: 10px;">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkHasFullAccess" runat="server" />
                                                        <asp:HiddenField ID="hidIsViewApplicable" runat="server" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.ReportsBL" EnablePaging="True" ID="objdsUsers"
                                            runat="server" SelectMethod="GetUserReportDetails" SelectCountMethod="GetUserReportCount"
                                            SortParameterName="sortExpression" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="int32" />
                                                <asp:ControlParameter ControlID="cmbReport" PropertyName="SelectedValue" Name="aiReportId"
                                                    Type="Int32" />
                                                <asp:ControlParameter ControlID="cmbUserRole" PropertyName="SelectedValue" Name="aiUserRoleId"
                                                    Type="Int32" />
                                                <asp:ControlParameter Name="asFilter" Type="String" ControlID="txtFilter" PropertyName="Text" />
                                                <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection"
                                                    PropertyName="Value" />
                                                <asp:Parameter Name="sortExpression" Type="String" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                        <asp:HiddenField ID="hidPageNo" runat="server" Value="" />                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" disable-page="true"
                                                        ValidationGroup="Save" Visible="false" OnClick="btnSave_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" CssClass="ClsBtn" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clientlstvwUsers = "<%=this.lstvwUsers.ClientID %>";

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
        }

        function beginRequestHandler(sender, args) {
            if ($get("<%=this.lblMassage.ClientID %>") != null)
                $get("<%=this.lblMassage.ClientID %>").innerHTML = "";
        }

        function SelectAll(obj) {
            var rowIndex = 0;
            var chkSelect = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkSelect");
            while (chkSelect != null) {
                chkSelect.checked = obj.checked;
                SetState(chkSelect, rowIndex, 1);

                rowIndex++;
                chkSelect = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkSelect");
            }
        }

        //This function is used to display message when page index will be changed.
        function CheckPageIndexChange(obj) {
            var bIsValid
            if (window.confirm("This action will lose unsaved details (if any) of this page. Do you want to continue?"))
                bIsValid = true
            else {
                obj.value = document.getElementById("<%=this.hidPageNo.ClientID %>").value
                bIsValid = false
            }
            return bIsValid
        }

        function SetState(obj, rowIndex, isSelectAll) {
            var hidIsViewApplicable = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_hidIsViewApplicable");
            var chkHasFullAccess = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkHasFullAccess");
            if (obj.checked) {
                if (hidIsViewApplicable.value == "1")
                    chkHasFullAccess.disabled = false;
                else
                    chkHasFullAccess.disabled = true;
            }
            else {
                chkHasFullAccess.disabled = true;
                chkHasFullAccess.checked = false;
            }

            if (isSelectAll != 1)
                CheckAll();
        }

        function CheckAll() {
            var isUnchecked = false;
            var rowIndex = 0;
            var chkSelect = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkSelect");
            while (chkSelect != null) {
                if (!chkSelect.checked) {
                    isUnchecked = true;
                    break;
                }
                rowIndex++;
                chkSelect = document.getElementById(_clientlstvwUsers + "_ctrl" + rowIndex + "_chkSelect");
            }

            var chkAll = document.getElementById(_clientlstvwUsers + "_chkAll");
            if (isUnchecked)
                chkAll.checked = false;
            else
                chkAll.checked = true;
        }

        function SetMesageState() {
            if ($get("<%=this.lblMassage.ClientID %>") != null)
                $get("<%=this.lblMassage.ClientID %>").innerHTML = "";
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
