<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserShiftAssociation.aspx.cs" Inherits="UserShiftAssociation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="server">
    <div class="MainBodyDiv" runat="server">
        <table align="center" width="100%">
            <tr>
                <td>
                    <table align="center" width="100%">
                        <tr>
                            <td align="right" colspan="2" style="color: #ff3333" valign="top">
                                <span class="ClsMdtStar">*
                                    <asp:Label ID="lblMandatoryField" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 100%;">
                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" EnableViewState="False"
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trListview">
                <td align="center">
                    <table id="tblShiftAssociation" border="0" cellpadding="1" cellspacing="1" runat="server"
                        width="50%">
                        <tr align="center">
                            <td align="center" class="ClsTextNormal" colspan="2">
                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <asp:Label ID="lblUpdateSucess" runat="server" EnableViewState="False" Font-Bold="True"
                                            ForeColor="Blue" Height="20px"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbShifts" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSaveShifts" EventName="Click" />                                            
                                        </Triggers>
                                 </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr align="center" style="text-align:center; margin:0px auto;">
                            <td align="center" style="text-align:center; margin:0px auto;">
                                <asp:UpdatePanel ID="upnlShiftAsso" runat="server">
                                    <ContentTemplate>
                                        <table align="center" width="50%" style="text-align:center;">
                                            <tr align="center" style="text-align:center;">
                                                <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                                    colspan="1">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="lblShift" runat="server" Text="Shift"></asp:Label>
                                                        <span id="Span3" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbShifts" runat="server" CausesValidation="true" CssClass="LrgCombo"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:CustomValidator ID="cvCmbShifts" Display="None" ClientValidationFunction="ValidateShiftSelection"
                                                        Visible="true" runat="server"></asp:CustomValidator>
                                                    <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                                    colspan="1">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="lblStaffGroup" runat="server" Text="Staff Group"></asp:Label>
                                                        <span id="Span1" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStaffGroup" runat="server" CausesValidation="false" CssClass="LrgCombo"
                                                        AutoPostBack="false">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                                    colspan="1">
                                                    <span class="ClsLabel">Name :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox" Width="200px" AutoPostBack="true"
                                                        autocomplete="off"></asp:TextBox>
                                                </td>
                                                <%--  <td align="left">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" CausesValidation="false" OnClick="btnSearch_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click"/>&nbsp;
                                                </td>--%>
                                            </tr>
                                            <tr align="center">
                                                <td align="center" colspan="3">
                                                    <asp:Button ID="btnshow" Text="Show" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                                        CausesValidation="true" disable-page="true" OnClick="btnshow_Click" />
                                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                                        CssClass="ClsBtn" CausesValidation="false" UseSubmitBehavior="false" />
                                                </td>
                                            </tr>
                                        </table>
                                        <tabel width="50%" slign="center">
                                            <tr id="tr1" runat="server" align="center">
                                                <td align="center" colspan="3">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwStaffGroupUsers">
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
                                            <tr align="center" class="padding-10">
                                                <td align="center" colspan="3">
                                                    <div id="Div1" style="overflow: auto;">
                                                        <asp:ListView ID="lstvwStaffGroupUsers" runat="server" DataKeyNames="UserId,ShiftId,UserName"
                                                            AllowPaging="True" AllowSorting="True" PageSize="20" OnPageIndexChanging="lstvwStaffGroupUsers_PageIndexChanging"
                                                            OnDataBound="lstvwStaffGroupUsers_DataBound" 
                                                            onitemdatabound="lstvwStaffGroupUsers_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table width="70%" align="center" runat="server" id="tblContacts" style="color: #333333; text-align:center;" cellpadding="0"
                                                                    cellspacing="1" class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th align="center">
                                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckUncheckAll(this);" />
                                                                        </th>
                                                                        <th align="left" style="padding-left: 10px;">
                                                                            <asp:LinkButton ID="lnkBtnSortName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                                CausesValidation="false" ForeColor="Black"> User Name </asp:LinkButton>
                                                                        </th>
                                                                    </tr>
                                                                    <tr runat="server" id="itemPlaceholder">
                                                                    </tr>
                                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                                        <td colspan="3">
                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStaffGroupUsers"
                                                                                PageSize="20">
                                                                                <Fields>
                                                                                    <asp:TemplatePagerField>
                                                                                        <PagerTemplate>
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td style="text-align:left;" align="left">
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
                                                                <tr id="trItem" runat="server" class="ClsGridRow">
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="ChkSelect" runat="server" name="ChkSelectUser" onclick="UncheckCheck(this);" />
                                                                    </td>
                                                                    <td class="paddingL" align="left">
                                                                        <asp:Label ID="txtStaffGroup" runat="server" MaxLength="50" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                                                    <td align="center">
                                                                        <asp:CheckBox ID="ChkSelect" runat="server" onclick="UncheckCheck(this);" />
                                                                    </td>
                                                                    <td class="paddingL" align="left">
                                                                        <asp:Label ID="txtStaffGroup" runat="server" MaxLength="50" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="LblNoRecord" align="center">
                                                                            No record found.
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                        <asp:ObjectDataSource TypeName="BusinessLogic.UserShiftAssociationBL" EnablePaging="true"
                                                            ID="GrdDSobj" runat="server" SelectMethod="GetUserDetails" SortParameterName="sortExpression"
                                                            SelectCountMethod="CountTotalUserRecords" EnableCaching="false">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="int32" />
                                                                <asp:ControlParameter Name="aiShiftId" Type="int32" ControlID="cmbShifts" PropertyName="SelectedValue" />
                                                                <asp:ControlParameter Name="aistaffGroupId" Type="int32" ControlID="cmbStaffGroup" PropertyName="SelectedValue" />                                                                
                                                                <asp:ControlParameter Name="sortExpression" Type="string" ControlID="hidSortExpression" PropertyName="Value" />
                                                                <asp:ControlParameter Name = "asSearchText" ControlID="txtSearch" Type="string" PropertyName="Text" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:Button runat="server" ID="btnSaveShifts" Text="Associate Shift" class="ClsBtn"
                                                            Visible="false" OnClick="btnSaveShifts_Click" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidPageNo" runat="server" Value="1" />
                                    </ContentTemplate>
                                    <%--<Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbShifts" 
                                                EventName="SelectedIndexChanged" />
                                        </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        _slienttxtSearch = '#<%=txtSearch.ClientID%>';
        _ClientcmbShifts = "<%=this.cmbShifts.ClientID %>";
        _clientlblError = '#<%=lblErrorMessage.ClientID%>';
        _clientlstvwStaffGroupsShifts = "<%=this.lstvwStaffGroupUsers.ClientID %>"
        _clientChkAllSelecter = '#<%=lstvwStaffGroupUsers.ClientID%> input[id*="ChkSelectAll"]:checkbox';
        _clientChkSelecter = '#<%=lstvwStaffGroupUsers.ClientID%> input[id*="ChkSelect"]:checkbox';
        _clientcvCmbShifts = "<%=this.cvCmbShifts.ClientID %>";
        var SchoolId = "<%=miSchoolId %>";
        var AcademicYearId = "<%=miAcademicYearId %>"
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"

        function AutoSearch() {
            _slienttxtSearch = '#<%=txtSearch.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"

            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtSearch, null, 1);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtSearch.ClientID %>");
            bt = document.getElementById("<%=this.btnshow.ClientID %>");
            SearchResult(txt, val, bt);
        }

        function CheckUncheckAll(src) {
            if (src == null)
                src = $get(_clientlstvwStaffGroupsShifts + '_ChkSelectAll');

            var iRowCount = 0;
            var chk = $get(_clientlstvwStaffGroupsShifts + '_ctrl' + iRowCount + '_ChkSelect');
            while (chk != null) {
                chk.checked = src.checked;

                iRowCount++;
                chk = $get(_clientlstvwStaffGroupsShifts + '_ctrl' + iRowCount + '_ChkSelect');
            }
        }

        function UncheckCheck(src) {
            if (src == null)
                src = $get(_clientlstvwStaffGroupsShifts + '_ChkSelect');
            src1 = $get(_clientlstvwStaffGroupsShifts + '_ChkSelectAll');
            var iRowCount = 0;
            var icheckcount = 0;
            var chk = $get(_clientlstvwStaffGroupsShifts + '_ctrl' + iRowCount + '_ChkSelect');
            while (chk != null) {
                if (chk.checked == true)
                    icheckcount++
                iRowCount++;
                chk = $get(_clientlstvwStaffGroupsShifts + '_ctrl' + iRowCount + '_ChkSelect');
            }
            if (iRowCount == icheckcount) {
                src1.checked = true;
            }
            else {
                src1.checked = false;
            }
        }

        //This function is used to display message when page index will be changed.
        function MessageAboutDate(oCmb) {
            var bIsValid
            if (window.confirm("If you change the page then entered details on current page will get lost. Do you want to continue?"))
                bIsValid = true
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false
            }
            return bIsValid
        }

        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblError) != null) {
                document.getElementById(_clientlblError).style.display = "none";
            }
        }

        function ValidateShiftSelection(oSrc, args) {
            var shiftList = document.getElementById(_ClientcmbShifts).value;
            if (shiftList != 0) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
                document.getElementById(_clientcvCmbShifts).errormessage = "Please select Shift.";
                return true;
            }
        }
     
    </script>
    <script type="text/javascript" src="../Scripts/Payroll/UserShiftAssociation.js"></script>
</asp:Content>
