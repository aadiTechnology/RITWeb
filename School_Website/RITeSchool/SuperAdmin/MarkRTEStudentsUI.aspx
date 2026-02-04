<%@ Page Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="MarkRTEStudentsUI.aspx.cs" Inherits="MarkRTEStudentsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table cellpadding="0" cellspacing="1" align="center" style="text-align: center;
        width: 100%;">
        <tr style="height: 10px;">
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:ValidationSummary ID="valSumErrorMsg" CssClass="ClsLabel" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>"
                    runat="server" ValidationGroup="SaveRTE" />
                <asp:CustomValidator ID="cstCustomValidator" runat="server" Visible="true" ClientValidationFunction="cstValidate"
                    ErrorMessage="Please select atleast one student" ValidationGroup="SaveRTE" Display="None"></asp:CustomValidator>
            </td>
        </tr>
        <tr align="center">
            <td id="tdMessage" runat="server" colspan="3">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                            EnableViewState="false" CssClass="ClsLabelNrml" Font-Bold="True"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <table align="center" style="width: 30%;">
                    <tr align="center">
                        <td class="ClsBorderlight">
                            <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="Standard" Height="16px"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbStandard" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="center">
                        <td class="ClsBorderlight">
                            <asp:Label ID="lblDivision" runat="server" CssClass="ClsLabel" Text="Division" Height="16px"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="cmbDivision" runat="server" CssClass="MidCombo">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr align="center">
                        <td class="ClsBorderlight">
                            <span class="clsLabel">
                                <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalizedResources, StudentNameRegNo %>"></asp:Label>
                                <span id="Span3" class="colonPadding">:</span> </span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" CssClass="LrgTxtBox" AutoPostBack="False"
                                autocomplete="off"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderLight">
                            <span class="ClsLabel">Category :</span>
                        </td>
                        <td align="left">
                            <asp:RadioButton ID="optNONRTE" runat="server" GroupName="RTECategory" Text="Non RTE students"
                                CssClass="ClsLabel" />
                            <asp:RadioButton ID="optRTE" runat="server" GroupName="RTECategory" Text="RTE students"
                                CssClass="ClsLabel" />
                        </td>
                    </tr>
                </table>
                <table align="center" style="width: 30%;">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>"
                                Width="150px" CssClass="ClsBtn" CausesValidation="true" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
                <table align="center" style="width: 60%;">
                    <tr align="center">
                        <td align="center">
                            <table align="center" width="100%">
                                <tr id="trPagerlstvwStudentRTE1" runat="server" align="center">
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentRTE1"
                                                    Visible="true">
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
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="width: 100%">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ListView ID="lstvwStudentRTE1" runat="server" DataKeyNames="StudentId" OnDataBound="lstvwStudentRTE1_DataBound">
                                                    <LayoutTemplate>
                                                        <table align="center" width="70%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" style="padding-left: 5px; width: 70px">
                                                                    <asp:CheckBox ID="CheckBoxSelect" runat="server" Text="Is RTE?" onclick="CheckAllUncheckAlls(_clientGridId)" />
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 135px;">
                                                                    <asp:Label ID="lblEnrolmentNumber" runat="server" Text="Registration Number" />
                                                                </th>
                                                                <th align="left" style="padding-left: 5px; width: 100px">
                                                                    <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalizedResources, StudentName %>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder" colspan="2">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="8">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentRTE1"
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
                                                            <td align="center" class="paddingL">
                                                                <asp:CheckBox runat="server" ID="chkSelect" />
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblEnrolmentNumber" runat="server" Text='<%#Eval("EnrolmentNumber") %>' />
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName" ) %>'> </asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="center" class="paddingL">
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblEnrolmentNumber" runat="server" Text='<%#Eval("EnrolmentNumber") %>' />
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr>
                                                            <td class="LblNoRecord" align="center">
                                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwStudentRTE1" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="upnl5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="Submit" ValidationGroup="SaveRTE"
                                                    OnClick="btnSave_Click" />
                                                <asp:Button ID="btnBack" CssClass="ClsBtn" runat="server" Text="Back" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ObjectDataSource ID="ObjDSlstvwStudentRTE1" runat="server" EnableCaching="False"
                                EnablePaging="True" TypeName="BusinessLogic.SuperAdminDetailsBL" SelectMethod="GetAllStudent"
                                SelectCountMethod="Count">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                        Type="int32" />
                                    <asp:ControlParameter Name="aiStandardId" ControlID="cmbStandard" PropertyName="SelectedValue" />
                                    <asp:ControlParameter Name="aiDivisionId" ControlID="cmbDivision" PropertyName="SelectedValue" />
                                    <asp:ControlParameter Name="abIsRTEStudent" ControlID="optRTE" PropertyName="Checked" />
                                    <asp:ControlParameter Name="asSearchText" ControlID="txtSearch" PropertyName="Text" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script lang="javascript" type="text/javascript">
        _clientlblSuccessMsg = "<%=this.lblMessage.ClientID %>"
        var _clientGridId = '<%=this.lstvwStudentRTE1.ClientID%>';
        var _iRowCount = '<%=this.lstvwStudentRTE1.Items.Count%>';
        function CheckAllUncheckAlls() {
            var checkAll = $("[id$=CheckBoxSelect]").attr('checked');
            if (checkAll)
                $("[id$=chkSelect]").attr('checked', checkAll);
            else
                $("[id$=chkSelect]").removeAttr('checked');
        }
        $(function () {
            $("[id$=chkSelect]").click(function () {
                if ($("[id$=chkSelect]").length == $("[id$=chkSelect]:checked").length)
                    $("[id$=CheckBoxSelect]").attr('checked', "checked");
                else $("[id$=CheckBoxSelect]").removeAttr("checked");
            });

            CheckHeaderCheckboxAtPageLoad();
        });
        function CheckHeaderCheckboxAtPageLoad() {
            if ($("[id$=chkSelect]").length == $("[id$=chkSelect]:checked").length)
                $("[id$=CheckBoxSelect]").attr('checked', "checked");
            else $("[id$=CheckBoxSelect]").removeAttr("checked");
        }
        //This function is used for Saving validation.
        function cstValidate(src, args) {
            if (CheckAtleastOneCheckBox(_clientGridId, 'chkSelect', _iRowCount)) {
                args.IsValid = true;
                return false;
            }
            args.IsValid = false;
            document.getElementById(_clientlblSuccessMsg).innerHTML = ""
            return true;
        }
    </script>
</asp:Content>
