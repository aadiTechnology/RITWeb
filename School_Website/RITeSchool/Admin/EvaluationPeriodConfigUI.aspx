<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EvaluationPeriodConfigUI.aspx.cs"
    MasterPageFile="../MasterPages/MasterPage.master" Inherits="EvaluationPeriodConfigUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
                    CssClass="ClsLabel" ShowSummary="true" />
            </td>
        </tr>
        <tr>
            <td id="tdMessage" runat="server" align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Blue"
                            EnableViewState="false"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbTests" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCopy" EventName="Click" />                        
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td align="left" colspan="1" class="ClsBorderlight">
                            <span class="ClsLblLgnd" style="font-weight: bold">Select Exam :</span>&nbsp;
                        </td>
                        <td align="left" colspan="1">
                            <asp:DropDownList ID="cmbTests" runat="server" CssClass="LrgCombo" OnSelectedIndexChanged="cmbTest_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblTermList" align="center" width="60%">
                    <tr align="center" style="width: 100%">
                        <td align="center" style="width: 300px">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ListView ID="lstvwTestConfiguration" runat="server" DataKeyNames="TestId,StandardId"
                                         OnItemDataBound="lstvwTestConfiguration_ItemDataBound">
                                        <LayoutTemplate>
                                            <table align="center" width="60%" runat="server" id="tblTermInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" class="paddingL" style="width: 20%; padding-right: 10px">
                                                        <asp:Label ID="lblStandardName" runat="server" Text="Standard"></asp:Label>
                                                    </th>
                                                    <th align="Center" class="paddingL" style="width: 20%">
                                                        <asp:Label ID="lblStartDate" runat="server" Text=" Start Date"></asp:Label>
                                                    </th>
                                                    <th align="Center" class="paddingL" style="width: 20%">
                                                        <asp:Label ID="lblEndDate" runat="server" Text=" End Date"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                <td align="left" class="paddingL" style="padding-right: 10px">
                                                    <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidStartDate" runat="server" />
                                                    <asp:HiddenField ID="hidEndDate" runat="server" />
                                                </td>
                                                <td align="Center" class="paddingL">
                                                    <asp:TextBox ID="txtStartDate" runat="server" MaxLength="11" Text='<%# Eval("TestStartDate") %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm1StartDate" runat="server" Control="txtStartDate" Culture="en"
                                                        Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                    <asp:HiddenField ID="hidTestId" Value='<%# Eval("TestId")%>' runat="server" />
                                                </td>
                                                <td align="Center" class="paddingL">
                                                    <asp:TextBox ID="txtEndDate" runat="server" MaxLength="11" Text='<%# Eval("TestEndDate") %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm1EndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                        Culture="en" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td align="left" class="paddingL" style="padding-right: 10px">
                                                    <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidStartDate" runat="server" />
                                                    <asp:HiddenField ID="hidEndDate" runat="server" />
                                                </td>
                                                <td align="Center" class="paddingL">
                                                    <asp:TextBox ID="txtStartDate" runat="server" MaxLength="11" Text='<%# Eval("TestStartDate") %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm1StartDate" runat="server" Control="txtStartDate" Culture="en"
                                                        Format="dd MMM yyyy" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                    <asp:HiddenField ID="hidTestId" Value='<%# Eval("TestId")%>' runat="server" />
                                                </td>
                                                <td align="Center" class="paddingL">
                                                    <asp:TextBox ID="txtEndDate" runat="server" MaxLength="11" Text='<%# Eval("TestEndDate") %>'
                                                        CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calTerm1EndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                        Culture="en" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        <asp:Label ID="lblNoRecordFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbTests" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div runat="server" id="divErr">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text=" Save" CausesValidation="true"
                                OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                <asp:HiddenField ID="hidRowCount" runat="server" />
                <asp:HiddenField ID="hidTestId" runat="server" Value="0" />
                <asp:HiddenField ID="hidStartDate" runat="server" />
                <asp:HiddenField ID="hidEndDate" runat="server" />
            </td>
        </tr>
        <tr style="height: 30px;">
            <td align="center">
                <hr style="width: 70%; border-width: 5px;" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td>
                            Exam List Details
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:CheckBoxList ID="ChkExamList" runat="server" Width="432px" RepeatColumns="2">
                                    </asp:CheckBoxList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cmbTests" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnCopy" Text="Copy to Other Exam" runat="server" CssClass="ClsBtn"
                                ValidationGroup="Save" BorderWidth="1px" CausesValidation="true" OnClick="btnCopy_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script>
        function ConfirmCopy() {
        
            if ($('[id*=ChkExamList]:checked').length == 0) {
                alert('At least one exam should be selected.')
                return false;
            }
            else
                return confirm('This action will overwrite existing configuration(if exist). Do you want to continue?')
        }
    </script>
</asp:Content>
