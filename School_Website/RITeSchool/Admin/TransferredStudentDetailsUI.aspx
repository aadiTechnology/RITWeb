<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransferredStudentDetailsUI.aspx.cs" Inherits="TransferredStudentDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        .Header
        {
            font-size: 9pt;
            font-family: Arial;
            text-align: left;
            padding-left: 5px;
        }
    </style>
    <table width="100%">
        <tr>
            <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSum" runat="server" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Target class should be selected for each selected row."
                            Display="None" ClientValidationFunction="ValidateTargetClass"></asp:CustomValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                                        ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table border="0" cellpadding="1" cellspacing="2" style="margin-left: 19px;">
                                <tr>
                                    <td class="ClsBorderLight">
                                        <asp:Label ID="Label5" runat="server" class="ClsLabel" Text="Type"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="optFrom" runat="server" Text="From" GroupName="Type" Width="75px" />
                                        <asp:RadioButton ID="optTo" runat="server" Text="To" GroupName="Type" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 200px" class="ClsBorderlight">
                                        <asp:Label ID="branchId" runat="server" class="ClsLabel" Text="Branch"> </asp:Label>
                                        <span class="colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBranch" runat="server" Style="width: 240px; height: 20px;"
                                            CssClass="MidCombo">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trActivatedFilter">
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="Label2" runat="server" class="ClsLabel" Text="Show Only Non Activated?"> </asp:Label>
                                        <span class="colonPadding">:</span>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkOnlyNonActivated" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel" Text="Name / Reg. No. : "
                                            EnableViewState="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="MidTxtBox" Style="width: 100%"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" CausesValidation="False"
                                            OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ListView ID="lstvwBranch" DataKeyNames="Student_Id" runat="server" OnItemDataBound="lstvwBranch_ItemDataBound">
                                        <LayoutTemplate>
                                            <table align="center" width="70%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="center" id="thChkSelectAll" runat="server" style="width: 40px; font-size: 9pt;">
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="SelectAll(this);" />
                                                    </th>
                                                    <th align="left" style="width: 80px;">
                                                        <asp:Label ID="Label4" runat="server" class="Header" Text="Branch Name"></asp:Label>
                                                    </th>
                                                    <th align="left" style="width: 80px;">
                                                        <asp:Label ID="lblSortorder1" runat="server" class="Header" Text="Enrolment No."></asp:Label>
                                                    </th>
                                                    <th align="left" style="width: 220px;">
                                                        <asp:Label ID="Label6" runat="server" class="Header" Text="Student Name"> </asp:Label>
                                                    </th>
                                                    <th align="left" style="width: 80px;">
                                                        <asp:Label ID="Label7" runat="server" class="Header" Text="Source Class"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 80px;" id="thTargetClass" runat="server">
                                                        <asp:Label ID="Label10" runat="server" Style="text-align: center; font-size: 9pt;
                                                            font-family: Arial;" Text="Target Class"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="center" id="tdSelect" runat="server">
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblBranchName" runat="server" class="clsLabel" Text='' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblStudentName" runat="server" class="clsLabel" Text='<%# Eval("Enrolment_Number") %>' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label3" runat="server" class="clsLabel" Text='<%# Eval("FullName") %>' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" class="clsLabel" Text='<%# Eval("SourceClass") %>' />
                                                </td>
                                                <td align="center" id="tdTargetDivision" runat="server">
                                                    <asp:DropDownList ID="ddlTargetDivision" runat="server" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                <td align="center" id="tdSelect" runat="server">
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblBranchName" runat="server" class="clsLabel" Text='' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblStudentName" runat="server" class="clsLabel" Text='<%# Eval("Enrolment_Number") %>' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label3" runat="server" class="clsLabel" Text='<%# Eval("FullName") %>' />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" class="clsLabel" Text='<%# Eval("SourceClass") %>' />
                                                </td>
                                                <td align="center" id="tdTargetDivision" runat="server">
                                                    <asp:DropDownList ID="ddlTargetDivision" runat="server" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label runat="server" ID="lblNoRecord" CssClass="LblNoRecord" Text="<%$ Resources:LocalizedResources, NoRecordFound%>"
                                                        Width="50%" />
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="BtnAdd" runat="server" CssClass="ClsBtn" Text="Activate" OnClick="BtnAdd_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnAdd" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        //  var _clientlstvwStudentTransfer = '<= this.lstvwStudentTransfer.ClientID >';

        //        $(document).ready(function () {
        //            AutoSearch();
        //        });

        //        function AutoSearch() {


        //            var _chkSelect = '_chkSelect';
        //            var _ctrl = '_ctrl';

        //            _clienttxtUserName = '#<%=txtSearch.ClientID%>';
        //            var SchoolId = "<%=miSchoolId %>";
        //            var AcademicYearId = "<%=miAcademicYearId %>"

        //            BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtUserName, _clientddlStandard, _clientddlDivision, null, 0);
        //        }

        //        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //        prm.add_endRequest(EndRequestHandler);

        //        // This function is used to enabled controls once a postback is complete.
        //        function EndRequestHandler() {
        //            AutoSearch();
        //        }

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

            SetAllFields();
        }

        function SetField(index) {
            var isChecked = $('[id$=_ctrl' + index + '_chkSelect]').prop('checked');
            if (isChecked) {
                $('[id$=_ctrl' + index + '_ddlTargetDivision]').removeAttr('disabled');
            }
            else {
                var div = $('[id$=_ctrl' + index + '_ddlTargetDivision]')
                div.val('0');
                div.attr('disabled', true);
            }
        }

        SetAllFields();
        function SetAllFields() {
            var index = 0
            $('[id$=_chkSelect]').each(function () {
                SetField(index)
                index++
            })
        }

        function ValidateTargetClass(src, args) {
            var isValid = true;
            var index = 0
            var isFound = false;
            $('[id$=_chkSelect]').each(function () {
                if ($(this).prop('checked') && $(this).prop('disabled') == false) {
                    isFound = true
                    if ($('[id$=_ctrl' + index + '_ddlTargetDivision]').val() == '0') {
                        src.errormessage = 'Target class should be selected for each selected record.'
                        isValid = false;
                    }
                }

                index++;
            })

            if (!isFound) {
                src.errormessage = 'At least one record should be selected for activation.'
                isValid = false;
            }

            args.IsValid = isValid;
            return !isValid;
        }

        function ResetMessage() {
            $('[id$=lblUpdateMessage]').html('')
        }

        function HideFields(val) {
            if (val == 0)
                $('#trActivatedFilter').show();
            else
                $('#trActivatedFilter').hide();
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
