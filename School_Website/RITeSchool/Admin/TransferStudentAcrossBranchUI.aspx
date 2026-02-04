<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransferStudentAcrossBranchUI.aspx.cs" Inherits="TransferStudentAcrossBranchUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td align="right">
                    <span class="ClsMdtStar">* Mandatory fields.</span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSum" runat="server" ValidationGroup="Source"></asp:ValidationSummary>
                            <asp:ValidationSummary ID="valSumTarget" runat="server" ValidationGroup="Target">
                            </asp:ValidationSummary>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="Source"
                                ErrorMessage="Source Standard should be selected." ControlToValidate="ddlStandard"
                                ValueToCompare="0" Display="None" Operator="NotEqual"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="Source"
                                ErrorMessage="Source Division should be selected." ControlToValidate="ddldivision"
                                ValueToCompare="0" Display="None" Operator="NotEqual"></asp:CompareValidator>
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ValidationGroup="Target"
                                ErrorMessage="Target branch should be selected." ControlToValidate="ddlBranch"
                                ValueToCompare="0" Display="None" Operator="NotEqual"></asp:CompareValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="At least one student should be select to transfer."
                                Display="None" ClientValidationFunction="ValidateStudents" ValidationGroup="Target"></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnTransfer" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" ForeColor="Blue"
                                            Font-Bold="true"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnTransfer" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight" style="width: 150px">
                                <asp:Label ID="lblStandard" runat="server" CssClass="clsLabel" Text="Standard : "></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStandard" runat="server" Style="width: 150px; height: 20px;"
                                    AutoPostBack="true" CssClass="MidCombo" OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged"
                                    ControlToValidate="ddlStandard">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                            </td>
                            <td class="ClsBorderlight" style="width: 150px">
                                <asp:Label ID="Label16" runat="server" CssClass="clsLabel" EnableViewState="False"
                                    Text="Division : "></asp:Label>
                            </td>
                            <td id="tdTargetStdCombo" runat="server">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddldivision" runat="server" Style="width: 150px;" CssClass="MidCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="ClsBorderlight">
                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel" Text="Name / Reg. No. : "
                                    EnableViewState="False"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="MidTxtBox" Style="width: 100%"
                                    autocomplete="off"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" Text="Show" CssClass="ClsBtn" ValidationGroup="Source"
                                    OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListView ID="lstvwStudentTransfer" runat="server" DataKeyNames="Student_Id">
                                <LayoutTemplate>
                                    <table cellpadding="0" cellspacing="1" class="GridBorder" style="color: #333333">
                                        <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="center" id="thChkSelectAll" runat="server" style="width: 40px; font-size: 9pt;">
                                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="SelectAll(this);" />
                                            </th>
                                            <th align="left" class="paddingL" style="font-size: 9pt; width: 200px;">
                                                Reg.No.
                                            </th>
                                            <th align="left" class="paddingL" style="font-size: 9pt; width: 100px;">
                                                Roll.No.
                                            </th>
                                            <th align="left" class="paddingL" style="font-size: 9pt; width: 300px;">
                                                Student Name.
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trGridRow" runat="server" class="ClsGridRow">
                                        <td align="center">
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Enrolment_Number") %>' />
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Roll_No") %>' />
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("StudentName") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                <asp:Label ID="lblNoRecordFound" runat="server" Text="<%$ Resources:LocalizedResources,NoRecordFound%>"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <asp:HiddenField ID="hidPaidFeesStudentIds" runat="server" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnTransfer" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="50%;">
                        <tr>
                            <td align="left" class="ClsBorderLight">
                                <span class="clsLabel" style="text-align: justify; padding: 5px;"><b>Note -</b> On click
                                    of Transfer button, student basic details will be transferred to selected branch.
                                    But it will be in deactivated state. To activate that student user need to login
                                    to website of that branch and need to activate that student from 'Transferred Student
                                    Details' screen by selecting division.</span>
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
                    <table cellpadding="0" cellspacing="1">
                        <tr>
                            <td align="left" class="ClsBorderlight" colspan="1" style="width: 150px">
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabel" Text="Transfer to Branch : "></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="MidCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnTransfer" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td align="left">
                                <asp:Button ID="btnTransfer" runat="server" Text="Transfer" CssClass="ClsBtn" Height="26px"
                                    ValidationGroup="Target" Width="110px" OnClick="btnTransfer_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">

            var _clientlstvwStudentTransfer = '<%= this.lstvwStudentTransfer.ClientID %>';

            $(document).ready(function () {
                AutoSearch();
            });

            function AutoSearch() {


                var _chkSelect = '_chkSelect';
                var _ctrl = '_ctrl';

                _clienttxtUserName = '#<%=txtSearch.ClientID%>';
                var SchoolId = "<%=miSchoolId %>";
                var AcademicYearId = "<%=miAcademicYearId %>"
                var _clientddlStandard = '<%=ddlStandard.ClientID%>';
                var _clientddlDivision = '<%=ddldivision.ClientID%>';

                BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtUserName, _clientddlStandard, _clientddlDivision, null, 0);
            }

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(EndRequestHandler);

            // This function is used to enabled controls once a postback is complete.
            function EndRequestHandler() {
                AutoSearch();
            }

            function SearchSelectedValue(val) {
                txt = document.getElementById("<%=this.txtSearch.ClientID %>");
                bt = document.getElementById("<%=this.btnSearch.ClientID %>");
                SearchResult(txt, val, bt);
            }



            // This function is used to Check Uncheck all checkboxes in the ListView

            function SelectAll(chk) {
                $('[id$=chkSelect]').prop('checked', chk.checked);
                $('[id$=chkSelect]').click(function () {
                    if ($('[id$=chkSelect]').length == $('[id$=chkSelect]:checked').length) {
                        $('[id$=chkSelectAll]').prop('checked', true)
                    }
                    else {
                        $('[id$=chkSelectAll]').prop('checked', false)
                    }
                });
            }

            function ValidateStudents(src, args) {
                var isValid = true;
                if ($('[id$=chkSelect]:checked').length == 0)
                    isValid = false;

                args.IsValid = isValid;
                return !isValid;
            }

            function ResetMessage() {
                $('[id$=lblMessage]').html('')
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
