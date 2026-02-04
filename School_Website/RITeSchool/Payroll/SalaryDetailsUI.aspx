<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalaryDetailsUI.aspx.cs" Inherits="SalaryDetailsUI" ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr id="trSalaryDetails" runat="server" viewstatemode="enabled">
                <td align="center">
                    <asp:UpdatePanel ID="upnlSalaryDetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%" align="center">
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="valSum" runat="server" ViewStateMode="Enabled" CssClass="LblErrorMsg" ValidationGroup="SaveAll" />
                                        <asp:ValidationSummary ID="valSumShow" runat="server" ViewStateMode="Enabled" CssClass="LblErrorMsg" ValidationGroup="Show" />
                                    </td>
                                </tr>
                                <tr id="trMessages" runat="server" viewstatemode="Enabled">
                                    <td align="center">
                                        <table>
                                            <tr id="trSalaryMessage" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="center" class="ClsHilightBGB">
                                                    <asp:Label ID="lblSalaryMessage" runat="server" Text="Salary of selected month is already saved."
                                                        EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trConfigMessage" runat="server" viewstatemode="Enabled" visible="false">
                                                <td>
                                                    <asp:Label ID="lblConfigMessage" runat="server" ViewStateMode="Enabled" Font-Bold="True" ForeColor="Red"
                                                        Text="Please configure yearwise leaves of respective user to save salary details."
                                                        CssClass="ClsLabel" EnableViewState="False" Width="120%"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trSuccessMessage" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="left">
                                                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" ForeColor="Red" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Blue" Text=""
                                                        CssClass="ClsLabel" EnableViewState="False" Width="120%"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trComboboxes" runat="server">
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <span class="ClsLabel">Staff Group :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStaffGroups" runat="server" ViewStateMode="Enabled" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="right">
                                                    <span class="ClsLabel">Year : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbYear" runat="server" ViewStateMode="Enabled" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="right">
                                                    <span class="ClsLabel">Month : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbMonths" runat="server" ViewStateMode="Enabled" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                </td>
                                                <%--<td align="right" class="ClsGreenBG" valign="middle">
                                                    <asp:LinkButton ID="lnlRetirementNotice" runat="server" Text="Retirement Notice"
                                                        Style="text-align: center;" CssClass="SubTitle" OnClick="lnlRetirementNotice_Click"></asp:LinkButton>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="ClsLabel">Name : </span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtSearch" runat="server" ViewStateMode="Enabled" Width="100%" autocomplete="off"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnShow" runat="server" ViewStateMode="Enabled" Text="Show" CssClass="ClsBtnMid remove-margin-top" ValidationGroup="Show"
                                                                    CausesValidation="true" OnClick="btnShow_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnRefresh" runat="server" ViewStateMode="Enabled" Text="Refresh" CssClass="ClsBtnMid remove-margin-top" ValidationGroup="Show"
                                                                    CausesValidation="true" OnClick="btnRefresh_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trUserLeaves" runat="server" viewstatemode="Enabled">
                                    <td align="center">
                                        <table>
                                            <tr style="height: 5px;">
                                                <td width="5px">
                                                </td>
                                                <td align="center" style="height: 25px" class="ClsGreenBG">
                                                    <asp:LinkButton ID="lnkUserLeaves" runat="server" ViewStateMode="Enabled" Text="Staff Leaves" CssClass="SubTitle"></asp:LinkButton>
                                                </td>
                                                <td width="12px">
                                                </td>
                                                <td align="center" style="height: 25px" class="ClsGreenBG">
                                                    <asp:LinkButton ID="lnkStaffAttendance" runat="server" ViewStateMode="Enabled" Text="Set Full Attendance"
                                                        CssClass="SubTitle"></asp:LinkButton>
                                                </td>
                                                <td width="5px" id="tdStaffLeaveSeparater" runat="server" viewstatemode="Enabled">
                                                </td>
                                                <td align="center" style="height: 25px" class="ClsGreenBG" id="tdStaffLeave" runat="server" viewstatemode="Enabled">
                                                    <asp:LinkButton ID="lnkDaywiseStaffLeave" runat="server" ViewStateMode="Enabled" Text="Datewise Staff Leaves"
                                                        CssClass="SubTitle"></asp:LinkButton>
                                                </td>
                                                <td width="12px">
                                                </td>
                                                <td align="center" style="height: 25px; padding-left:0px;" class="ClsGreenBG">
                                                    <asp:LinkButton ID="lnkPaymentDetails" runat="server" ViewStateMode="Enabled" Text="Salary Payment Details"
                                                        CssClass="SubTitle"></asp:LinkButton>
                                                </td>
                                                <td width="12px">
                                                </td>
                                                <td align="center" style="height: 25px" class="ClsGreenBG">
                                                    <asp:LinkButton ID="lnkExportStaffLEave" runat="server" ViewStateMode="Enabled" Text="Export Staff Leaves"
                                                        CssClass="SubTitle"></asp:LinkButton>
                                                </td>
                                                <td width="12px">
                                                </td>
                                                <td align="center" style="height: 25px" class="ClsGreenBG">
                                                    <asp:LinkButton ID="lnkMonthwiseAttendance" runat="server" ViewStateMode="Enabled" Text="Monthwise Staff Attendance"
                                                        CssClass="SubTitle"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr style="height: 5px;">
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                </td>
                                                <td>
                                                    <div id="tdStaffInOutDetails" runat ="server" visible="true" align="left" style="height: 25px" class="ClsGreenBG">
                                                        <asp:LinkButton ID="lnkStaffInOutDetails" runat="server" ViewStateMode="Enabled" Text="Staff In/Out Details"
                                                            CssClass="SubTitle"></asp:LinkButton>
                                                    </div>
                                                </td>
                                                <td width="12px">
                                                </td>
                                                <td align="left" style="height: 25px" class="ClsGreenBG">
                                                    <asp:LinkButton ID="lnkExcludeFromSalary" runat="server" ViewStateMode="Enabled" Text="Exclude Staff From Salary SMS"
                                                        CssClass="SubTitle"></asp:LinkButton>
                                                </td>
                                                <td width="12px">
                                                </td>
                                                <td align="left" style="height: 25px" class="ClsGreenBG">
                                                    <asp:LinkButton ID="lnlRetirementNotice" runat="server" ViewStateMode="Enabled" Text="Retirement Notice"
                                                        Style="text-align: center;" CssClass="SubTitle" OnClick="lnlRetirementNotice_Click"></asp:LinkButton>
                                                </td>
                                                <td width="12px">
                                                </td>
                                                <td align="left" style="height: 25px" class="ClsGreenBG">
                                                    <asp:LinkButton ID="lnkODDetails" runat="server" ViewStateMode="Enabled" Text="On Duty (O.D) Details"
                                                        CssClass="SubTitle"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                    </td>
                                </tr>
                                <tr id="trLegend" runat="server" viewstatemode="Enabled">
                                    <td align="center">
                                        <table id="tblLegent" viewstatemode="Enabled" runat="server" >
                                            <tr>
                                                <td width="130px">
                                                    <span class="ClsLblLgnd" style="white-space: nowrap;">Please fill basic details of salary.
                                                        &nbsp;</span> <span class="ClsLblLgnd">Legend : </span>
                                                </td>
                                                <td width="20px" align="left">
                                                    <span style="background-color: LightSkyBlue; height: 20px; border-color: Black; border-style: Solid;
                                                        border-width: 1px; width: 20px">
                                                        <img src="../images/spacer.gif" width="20px" height="20px" /></span>
                                                </td>
                                                <td width="50px">
                                                    <span class="ClsLblLgnd">Attendance</span>
                                                </td>
                                                <td width="20px">
                                                    <span style="background-color: LightSalmon; height: 20px; border-color: Black; border-style: Solid;
                                                        border-width: 1px; width: 20px">
                                                        <img src="../images/spacer.gif" width="20px" height="20px" /></span>
                                                </td>
                                                <td width="45px">
                                                    <span class="ClsLblLgnd">Leaves</span>
                                                </td>
                                                <td width="20px">
                                                    <span style="background-color: #E1E1FF; height: 20px; border-color: Black; border-style: Solid;
                                                        border-width: 1px; width: 20px">
                                                        <img src="../images/spacer.gif" width="20px" height="20px" /></span>
                                                </td>
                                                <td width="80px">
                                                    <span class="ClsLblLgnd">Salary Difference</span>
                                                </td>
                                                <td width="20px">
                                                    <span style="background-color: LightPink; height: 20px; border-color: Black; border-style: Solid;
                                                        border-width: 1px; width: 20px">
                                                        <img src="../images/spacer.gif" width="20px" height="20px" /></span>
                                                </td>
                                                <td width="100px">
                                                    <span class="ClsLblLgnd">Earnings and Deductions</span>
                                                </td>
                                                <td align="center" valign="middle" style="border: 1px solid #000000; width: 50px;">
                                                    <span class="ClsLabel" style="color: Red; float: inherit; white-space: nowrap">Deleted
                                                        User</span>
                                                </td>
                                            </tr>
                                            <tr id="trSalaryDifferenceMessage" runat="server" viewstatemode="Enabled" >
                                                <td colspan="10">
                                                    <asp:Label ID="lblSalaryDifferenceMessage" runat="server" ViewStateMode="Enabled" CssClass="ClsLblLgnd" EnableViewState="true"
                                                        Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center">
                                        <table id="tblPageDetails" runat="server" viewstatemode="Enabled" width="180px" cellpadding="0" cellspacing="0"
                                            style="vertical-align: top;" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblStartIndex" Text="1" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                </td>
                                                <td align="center">
                                                    <span class="LblNormal">to</span>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEndIndex" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                </td>
                                                <td align="center">
                                                    <span class="LblNormal">out of</span>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblTotalRecords" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                </td>
                                                <td align="center">
                                                    <span class="LblNormal">records</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" visible="true" runat="server" id="tdGrid">
                                        <div id="divContainer" class="GridBorder" runat="server" viewstatemode="Enabled" visible="false" style="width: 800px;
                                            height: 590px; overflow: scroll">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="grdSalaryDetails" Width="100%" runat="server" ViewStateMode="Enabled" DataKeyNames="UserId,DisplayControls"
                                                            PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                            UseAccessibleHeader="true" CssClass="GridBorder" OnRowDataBound="grdSalaryDetails_RowDataBound"
                                                            OnRowCommand="grdSalaryDetails_RowCommand">
                                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                FirstPageText="First" Mode="NumericFirstLast"></PagerSettings>
                                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Save">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btnSaveSalary" runat="server" ViewStateMode="Enabled" CausesValidation="false" CommandName="SAVE"
                                                                            CommandArgument="<%# Container.DataItemIndex %>" CssClass="ClsBtn" Text="Save"
                                                                            ToolTip="Save" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                            </PagerStyle>
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                            <PagerTemplate>
                                                            </PagerTemplate>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <table id="tblPager" runat="server" viewstatemode="Enabled" width="100%" cellpadding="0" cellspacing="0"
                                                            style="vertical-align: top;">
                                                            <tr>
                                                                <td align="left" class="ClsBorderPager" valign="middle">
                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                                    <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                        OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" ViewStateMode="Enabled">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="100px" align="right" class="ClsBorderPager" valign="middle">
                                                                    <asp:Label ID="lblCurrentPage" runat="server" ViewStateMode="Enabled" CssClass="LblNormal" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:CustomValidator ID="cstAttendance" runat="server" ViewStateMode="Enabled" ClientValidationFunction="CheckAttendance"
                                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="SaveAll"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trNote" runat="server" viewstatemode="Enabled">
                                    <td align="center">
                                        <table id="tblNote" runat="server" viewstatemode="Enabled" align="center" width="800px">
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Leave balance is displayed including the leaves of selected month (i.e. Leave Balance = Used Leaves + Late Mark Leaves)."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label14" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note2 :"
                                                        CssClass="LblNrmlB" Height="16px"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label15" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Please save the salary difference inorder to get the salary difference."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label16" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note3 :"
                                                        CssClass="LblNrmlB" Height="16px"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label17" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Deleted users should be deactivated from the payroll to block them in the salary payment."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label18" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note4 :"
                                                        CssClass="LblNrmlB" Height="16px"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label19" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Click on 'Staff Leaves' link to open popup to add attendance and leaves."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label20" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note5 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label21" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If user consumes a leave attached to the configured holiday or leaves those enclose configured holiday, then the configured percentage of the holiday period will be considered as an unpaid leave(s)."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trUnpublishSalaryNote" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label1" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note6 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="lblUnpublishSalaryNote" runat="server" ViewStateMode="Enabled" BorderWidth="0px" CssClass="LblSmlV"
                                                        Text="Salary of this month can not unpublish since attendance and leaves of next month is already saved."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label22" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note6 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label23" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="To pay salary according to joining / resign date, you will have to click on Save button after setting joining / resign date."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label2" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note7 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label3" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="After opening this page if you have made any changes in earnings and deductions then to reflect same on this page, click on Refresh button."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label5" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note8 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label6" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If this is last month of payroll year and permanent date is not set for permanent users then on unpublish of salary, basic leave of those users will be reset to zero. So please make sure permanent date is set for permanent users."></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                            Text="No record found." Visible="false" EnableViewState="False" Width="70%"></asp:Label><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" ViewStateMode="Enabled" Text="Save All" CssClass="ClsBtn" Visible="false"
                                            disable-page="true" OnClick="btnSave_Click" ValidationGroup="SaveAll" />
                                        <asp:Button ID="btnUnpublish" runat="server" ViewStateMode="Enabled" Text="Unpublish" CssClass="ClsBtn" Visible="false"
                                            ValidationGroup="Show" CausesValidation="true" UseSubmitBehavior="false" OnClick="btnUnpublish_Click" />
                                        <asp:Button ID="btnExport" runat="server" ViewStateMode="Enabled" Text="Export" CssClass="ClsBtn" Visible="true"
                                            ValidationGroup="Show" CausesValidation="true" UseSubmitBehavior="false" OnClick="btnExport_Click" />
                                        <asp:Button ID="btnExportAll" runat="server" ViewStateMode="Enabled" Text="Export All" CssClass="ClsBtn"
                                            Visible="true" UseSubmitBehavior="false" OnClick="btnExportAll_Click" />
                                        <asp:Button ID="btnExportEarnings" runat="server" ViewStateMode="Enabled" 
                                            Text="Export Earnings" CssClass="ClsBtn"
                                            Visible="true" UseSubmitBehavior="false" 
                                            onclick="btnExportEarnings_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidSalaryDiffColumnIndex" runat="server" ViewStateMode="Enabled" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidColumnIndexes" runat="server" ViewStateMode="Enabled" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidTxtValue" runat="server"  ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="HidMonthDays" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidScreenWidth" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidIsSaveClick" runat="server" ViewStateMode="Enabled" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidLeaveTransferMonth" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidMinRecordsStaffGroupId" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidIsStaticOutput" runat="server" ViewStateMode="Enabled" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidIsNextMonthAttendAvail" runat="server" ViewStateMode="Enabled" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidMonthList" runat="server" ViewStateMode="Enabled" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidQueryString" runat="server" ViewStateMode="Enabled" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidSelectedMonth" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSelectedYear" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSelectedStaffGroup" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSelectedPageIndex" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidIsSaveButtonClick" runat="server" ViewStateMode="Enabled" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSalaryFilter" runat="server" ViewStateMode="Enabled" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidSalaryYear" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSalaryMonthId" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSalaryStaffgroup" runat="server" ViewStateMode="Enabled" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidLeaveIntervalMonth" runat="server" ViewStateMode="Enabled" Value="N"></asp:HiddenField>
                                        <asp:HiddenField ID="hidITAmount" runat="server" ViewStateMode="Enabled"></asp:HiddenField>
                                        <asp:HiddenField ID="hidITQueryString" runat="server" ViewStateMode="Enabled" Value=""></asp:HiddenField>
                                        <asp:HiddenField ID="hidActivityQueryString" runat="server" ViewStateMode="Enabled" Value=""></asp:HiddenField>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExport" />
                            <asp:PostBackTrigger ControlID="btnExportAll" />
                            <asp:PostBackTrigger ControlID="btnExportEarnings" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="updtpnl1" runat="server">
                        <ContentTemplate>
                            <div id="divRetirementNotice" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none;
                                position: absolute; z-index: 1000; margin: 0px; padding: 0px; width: 760px; height: 430px;
                                border-width: 1px; left: 5px; top: 150px; line-height: normal; border: solid 2px darkgreen;
                                margin: -110px 0px 0px 00px; background-color: white;">
                                <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                                    background-repeat: repeat-x; color: Black; width: 760px; text-align: right" class="close-img-style">
                                    <div style="font-size: 12px; width: 350px; letter-spacing: 1px; padding-left: 8px;
                                        font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                                        Retirement Notice
                                    </div>
                                    <span style="cursor: hand" onclick="javascript:HidePopup();">
                                        <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif" border="0" />
                                    </span>
                                </div>
                                <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                                    color: #333; overflow: auto; height: 380px; width: 750px; margin-left: 1px" id="Div5">
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td align="left" style="padding-left: 5px; font-weight: bold">
                                                            <asp:Label ID="Label31" runat="server" BorderWidth="0px" CssClass="LblSmlV" EnableViewState="False"
                                                                Style="width: 100%;" Text="List of the staff members having retirement in near future and / or retired recently however still active in Payroll."></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="tr1" runat="server">
                                            <td id="Td1" align="left" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                Text="Legend: " EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td align="center" style="border: 1px solid #000000;" valign="middle">
                                                            <asp:Label ID="Label4" runat="server" BackColor="Pink" BorderStyle="None" BorderWidth="1px"
                                                                CssClass="ClsLblLgnd" EnableViewState="False" Font-Bold="False" ForeColor="Black"
                                                                ReadOnly="True" Text="Retired Recently But Still Active In Payroll" Width="230px"
                                                                Height="15px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ListView ID="lstvwRetirementDetails" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwRetirementDetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table align="center" width="710px" runat="server" id="tblStaffInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th>
                                                                    Sr. No.
                                                                </th>
                                                                <th align="left" class="paddingL">
                                                                    Name (Designation)
                                                                </th>
                                                                <th>
                                                                    Retirement Date
                                                                </th>
                                                                <th>
                                                                    Remaining Days
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trGridRow" runat="server" viewstatemode="Enabled" class="ClsGridRow">
                                                            <td align="center">
                                                                <asp:Label ID="lblSrNo" runat="server" ViewStateMode="Enabled"></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Name") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblRetirementDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("RetirementDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblDays" runat="server" ViewStateMode="Enabled" CssClass="clsLabelC" Text='<%# Eval("RemainingDays") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trGridRow" runat="server" viewstatemode="Enabled" class="ClsGridAltRow">
                                                            <td align="center">
                                                                <asp:Label ID="lblSrNo" runat="server" ViewStateMode="Enabled"></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Name") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblRetirementDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("RetirementDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblDays" runat="server" ViewStateMode="Enabled" CssClass="clsLabelC" Text='<%# Eval("RemainingDays") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <table width="740px" align="center">
                                                            <tr>
                                                                <td class="LblNoRecord" style="text-align: center">
                                                                    <span>No record found.</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="bottom">
                                                <asp:Button ID="btnClosePopUp" runat="server" ViewStateMode="Enabled" Text="Close" CssClass="ClsBtnMid" CausesValidation="false"
                                                    Width="75px" OnClientClick="javascript:HidePopup();return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnlRetirementNotice" EventName="click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div runat="server" viewstatemode="Enabled" id="divErr">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clienthidTxtValue = "<%=this.hidTxtValue.ClientID %>"
        _clientgrdSalaryDetails = "<%=this.grdSalaryDetails.ClientID %>"
        _clientHidMonthDays = "<%=this.HidMonthDays.ClientID %>"
        _clientbtnUnpublish = "<%=this.btnUnpublish.ClientID %>"
        _clientcmbStaffGroups = "<%=this.cmbStaffGroups.ClientID %>"
        _clientcmbYear = "<%=this.cmbYear.ClientID %>"
        _clientcmbMonths = "<%=this.cmbMonths.ClientID %>"
        _clientcstAttendance = "<%=this.cstAttendance.ClientID %>"
        _clienthidIsStaticOutput = "<%=this.hidIsStaticOutput.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clienthidLeaveTransferMonth = "<%=this.hidLeaveTransferMonth.ClientID %>"
        _clientbtnShow = "<%=this.btnShow.ClientID %>"
        _clientbtnRefresh = "<%=this.btnRefresh.ClientID %>"
        _clienthidIsNextMonthAttendAvail = "<%=this.hidIsNextMonthAttendAvail.ClientID %>"
        _clientbtnExport = "<%=this.btnExport.ClientID %>"
        _clientbtnExportAll = "<%=this.btnExportAll.ClientID %>"
        _clientbtnExportEarnings = "<%=this.btnExportEarnings.ClientID %>"
        _clientlnkUserLeaves = "<%=this.lnkUserLeaves.ClientID %>"
        _clientlnkStaffAttendance = "<%=this.lnkStaffAttendance.ClientID %>"
        _clientlnkRetirementNotice = "<%=this.lnlRetirementNotice.ClientID %>"
        _clienthidSelectedMonth = "<%=this.hidSelectedMonth.ClientID %>"
        _clienthidSelectedYear = "<%=this.hidSelectedYear.ClientID %>";
        _clienthidSelectedStaffGroup = "<%=this.hidSelectedStaffGroup.ClientID %>"
        _clienthidLeaveIntervalMonth = "<%=this.hidLeaveIntervalMonth.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
            //DisableControls(false, false)
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientlnkRetirementNotice && $get(_clientlnkRetirementNotice).innerHTML == "Retirement Notice") {

                OpenRetirementNoticePopup();
            }
            AutoSearch();

        }
        function beginRequestHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnShow && $get(_clientbtnShow).innerHTML == "Show") {
                //DisableControls(true, true)
            }
            else {
                //DisableControls(true, false)
            }
        }
        function DisableControls(action, IsShowButton) {
            if (document.getElementById(_clientbtnUnpublish) != null) {
                document.getElementById(_clientbtnUnpublish).disabled = action;
                if (!action && document.getElementById(_clienthidIsNextMonthAttendAvail).value == "Y")
                    document.getElementById(_clientbtnUnpublish).disabled = true;
            }
            var StaticOutput = document.getElementById(_clienthidIsStaticOutput).value
            if (IsShowButton != true && action != false) {
                if (document.getElementById(_clientcmbYear) != null)
                    document.getElementById(_clientcmbYear).disabled = action
                if (document.getElementById(_clientcmbMonths) != null)
                    document.getElementById(_clientcmbMonths).disabled = action
            }
            if (document.getElementById(_clientbtnSave) != null)
                document.getElementById(_clientbtnSave).disabled = action
            if (document.getElementById(_clientbtnShow) != null)
                document.getElementById(_clientbtnShow).disabled = action
            if (document.getElementById(_clientbtnRefresh) != null)
                document.getElementById(_clientbtnRefresh).disabled = action
            if (document.getElementById(_clientbtnExport) != null)
                document.getElementById(_clientbtnExport).disabled = action
            if (document.getElementById(_clientbtnExportAll) != null)
                document.getElementById(_clientbtnExportAll).disabled = action
            if (document.getElementById(_clientbtnExportEarnings) != null)
                document.getElementById(_clientbtnExportEarnings).disabled = action
            if (document.getElementById(_clientlnkUserLeaves) != null)
                document.getElementById(_clientlnkUserLeaves).disabled = action
            if (document.getElementById(_clientlnkStaffAttendance) != null)
                document.getElementById(_clientlnkStaffAttendance).disabled = action
        }
        function GetValue(txt) {
            document.getElementById(_clienthidTxtValue).value = txt.value
        }

        function Validate(textbox, MaxVal, sLeaveBalance) {
            var sMarks = textbox.value
            var OriginalValue = document.getElementById(textbox.id.replace("txt", "hid")).value;
            if (textbox.id.match("AT") || textbox.id.match("LV")) {
                var iMarks = parseInt(sMarks)
                if (textbox.id.match("LV") && sLeaveBalance != "-999" && parseFloat(sLeaveBalance) < sMarks) {
                    if (parseFloat(OriginalValue) + parseFloat(sLeaveBalance) >= iMarks) {
                        if (parseFloat(sLeaveBalance) > OriginalValue) {
                            if (parseFloat(sLeaveBalance) <= MaxVal) {
                                textbox.value = sLeaveBalance
                            }
                            else {
                                textbox.value = MaxVal
                            }
                        }
                    }
                    else
                        textbox.value = OriginalValue;

                }
                else if (sMarks == "" || iMarks > MaxVal) {
                    textbox.value = document.getElementById(_clienthidTxtValue).value

                }
                else {
                    var floatValue = parseFloat(textbox.value)
                    var intValue = parseInt(textbox.value)
                    intValue = parseFloat(intValue)
                    var difference = parseFloat((floatValue * 10) % 10)
                    if (difference != 5 && difference != 0) {
                        if (difference > 5)
                            difference = intValue + 1
                        else
                            difference = intValue + 0.5
                        textbox.value = difference
                    }
                }
            }
            else if (sMarks == "") {
                textbox.value = document.getElementById(_clienthidTxtValue).value
                textbox.focus()
            }
        }


        function CheckGridRow(RowNumber, bDisplayAlert) {
            if (bDisplayAlert == true)
                RowNumber = RowNumber + 1
            else
                RowNumber = RowNumber - 1
            var inputs = []
            var grdViewElement = document.getElementById(_clientgrdSalaryDetails)
            inputs = grdViewElement.rows[RowNumber].getElementsByTagName("input")
            var spans = []
            spans = grdViewElement.rows[RowNumber].getElementsByTagName("span")
            var iMonthDays = document.getElementById(_clientHidMonthDays).value
            var Total = 0

            for (i = 0; i < inputs.length; i++) {
                if ((inputs[i].id.match("AT") || inputs[i].id.match("LV")) && inputs[i].id.match("hid") == null) {
                    Total = Total + parseFloat(inputs[i].value)
                }
            }
            if (Total != 0 && Total != parseInt(iMonthDays)) {
                if (bDisplayAlert) {
                    var sAttendanceMessage = "Total of attendance and leaves should be equal to total days of month(" + iMonthDays + ")."
                    alert(sAttendanceMessage)
                }
                return false
            }
            $get("<%=this.hidIsSaveClick.ClientID %>").value = "Y"
            SetInitStatus();
            return true
        }
        function CheckAttendance(oSrc, args) {
            var grdViewElement = document.getElementById(_clientgrdSalaryDetails)
            var n = grdViewElement.rows.length
            var iMonthDays = document.getElementById(_clientHidMonthDays).value
            var bResult = true
            var sMessage = ""
            var iRowIndex = 2
            var iCounter
            while (iRowIndex < n) {
                if (iRowIndex < 10)
                    iCounter = "_ctl0" + iRowIndex
                else
                    iCounter = "_ctl" + iRowIndex
                if (document.getElementById(_clientgrdSalaryDetails + iCounter + "_btnSaveSalary") != null) {
                    if (!CheckGridRow(iRowIndex, false)) {
                        bResult = false
                        var sName = document.getElementById(_clientgrdSalaryDetails + iCounter + "_lblName").innerHTML
                        sMessage = sMessage + ", " + sName
                    }
                }
                iRowIndex = iRowIndex + 1
            }
            if (!bResult) {
                if (sMessage.length > 0)
                    sMessage = sMessage.substring(1)
                document.getElementById(_clientcstAttendance).errormessage = "Total of attendance and leaves should be equal to total days of month(" + iMonthDays + ") for Staffs : <BR />" + sMessage
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function DisplayConfirmation() {
            var LeaveTransferMonth = document.getElementById(_clienthidLeaveTransferMonth).value
            var MonthId = document.getElementById(_clientcmbMonths).value
            var sMessage = "With this action all the changes made in Earnings and Deduction will be available for this salary month. Are you sure, you want to continue?"

            if ($get(_clienthidLeaveIntervalMonth).value == 'Y')
                sMessage = "With this action all the changes made in Earnings and Deduction will be available for this salary month as well as year-wise leave configuration of next interval will be removed. Are you sure, you want to continue?"

            if (LeaveTransferMonth == MonthId)
                sMessage = "With this action all the changes made in Earnings and Deduction will be available for this salary month as well as year-wise leave configuration of next year will be removed. Are you sure, you want to continue?"
            return confirm(sMessage)
        }

        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>"
        function OpenPopup() {
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('DatewiseStaffLeavesPopup.aspx?' + sEncryptedString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1050,height=700')
            return false;
        }

        function DatewiseLeavesPopup() {
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('StaffAttendancePopup.aspx?' + sEncryptedString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1150,height=700')
            return false;
        }

        function OpenAttendancePopup() {
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('UsersAttendancePopup.aspx?' + sEncryptedString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1150,height=750')
            return false;
        }

        function SetWidth() {
            if (document.getElementById('hidScreenWidth') != null)
                $get('hidScreenWidth').value = "" + window.screen.width
        }
        SetWidth()

        function HideButtons() {
            btnSave = document.getElementById(_clientbtnSave);
            btnUnpublish = document.getElementById(_clientbtnUnpublish);
            btnExport = document.getElementById(_clientbtnExport);
            btnExportAll = document.getElementById(_clientbtnExportAll);
            btnExportEarnings = document.getElementById(_clientbtnExportEarnings);

            selectedMonth = document.getElementById(_clienthidSelectedMonth).value;
            selectedYear = document.getElementById(_clienthidSelectedYear).value;
            selectedStaffGroup = document.getElementById(_clienthidSelectedStaffGroup).value;

            currentMonth = document.getElementById(_clientcmbMonths).value;
            currentYear = document.getElementById(_clientcmbYear).value;
            currentStaffGroup = document.getElementById(_clientcmbStaffGroups).value;

            var disableControl = selectedMonth == currentMonth && selectedYear == currentYear && selectedStaffGroup == currentStaffGroup;

            if (btnSave != null)
                btnSave.disabled = !disableControl;
            if (btnUnpublish != null)
                btnUnpublish.disabled = !disableControl;
            if (btnExport != null)
                btnExport.disabled = !disableControl;
            if (btnExportAll != null)
                btnExportAll.disabled = !disableControl;
            if (btnExportEarnings != null)
                btnExportEarnings.disabled = !disableControl;
        }

        function SetInitStatus() {
            document.getElementById("<%=this.hidIsSaveButtonClick.ClientID %>").value = "Y";
        }

        function OprnFormNo16Report(sEncryptedString) {
            window.open(sEncryptedString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=250,height=250')
        }


        function OpenRetirementNoticePopup(obj) {
            _clientdivTemplates = "<%=this.divRetirementNotice.ClientID %>"
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divRetirementNotice.ClientID %>").style
            var width = 750
            var height = 380
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"

        }

        function HidePopup() {
            $get("<%=this.divRetirementNotice.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divRetirementNotice.ClientID %>").style.display = "none"
            return false
        }

        // This method is used to open form no 16 file for calculating amount.
        function CalculateAmount(txt, queryString) {
            document.getElementById("<%=this.hidITAmount.ClientID %>").value = txt.id.replace("lnk", "txt");
            window.open(queryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=10,height=10')
            return false;
        }

        // This method is used to open form no 16 file for calculating amount.
        function CalculateAmountForAll() {
            var querystring = document.getElementById("<%=this.hidITQueryString.ClientID %>").value;
            window.open(querystring, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=10,height=10')
            return false;
        }

        // This method is used to set amount to respective textbox. this method will be called from popup screen.
        function UpdateAmount(amount, isForSingle) {
            if (isForSingle == 'Y') {
                var Id = document.getElementById("<%=this.hidITAmount.ClientID %>").value;
                var txtAmount = document.getElementById(Id);
                txtAmount.value = amount;
            }
            else {
                SetAmount(amount);
            }
        }

        function SetAmount(amount) {
            var amounts = amount.split(',')
            var iIndex = 0;

            var rowNo = 2;
            var sRowNo = "";

            if (rowNo < 10)
                sRowNo = "0" + rowNo;
            else
                sRowNo = rowNo + "";

            var txt = document.getElementById(_clientgrdSalaryDetails + "_ctl" + sRowNo + "_txt_ED_35_0");

            while (txt != null) {

                txt.value = amounts[iIndex++];

                rowNo++;

                if (rowNo < 10)
                    sRowNo = "0" + rowNo;
                else
                    sRowNo = rowNo + "";

                var txt = document.getElementById(_clientgrdSalaryDetails + "_ctl" + sRowNo + "_txt_ED_35_0");
            }
        }

        function OpenPaymentDetailsPopup() {
            window.open("SalaryPaymentPopup.aspx", '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=750')
        }

        function OpenLeaveExportPopup() {
            window.open("StaffLeaveExportPopup.aspx", '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=650,height=750')
        }

        function OpenMonthwiseAttendancePopup() {
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('MonthwiseStaffAttendancePopup.aspx?' + sEncryptedString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1150,height=630')
            return false;
        }

        function OpenODDetailsPopup() {
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('ODDetailsPopup.aspx?' + sEncryptedString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650')
            return false;
        }

        function OpenInOutDetailsPopup() {
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('StaffInOutDetailsPopup.aspx?' + sEncryptedString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=500')
            return false;
        }

        function OpenAvtivityDetailsScreen() {
            _clienthidQueryString = "<%=this.hidActivityQueryString.ClientID %>"
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('../Admin/ActivityAssignmentUI.aspx?' + sEncryptedString, '_self')
            return false;
        }

    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtSearch.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"

            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, null, 1);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtSearch.ClientID %>");
            bt = document.getElementById("<%=this.btnShow.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
