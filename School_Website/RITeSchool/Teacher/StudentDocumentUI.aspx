<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="StudentDocumentUI.aspx.cs"
    Inherits="StudentDocumentUI" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table id="tblTop" runat="server" style="width: 100%;" cellspacing="1" cellpadding="0"
        border="0">
        <tbody>
            <tr>
                <td align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table style="width: 95%;" border="0" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" Text="* Mandatory Fields" CssClass="ClsMdtStar" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:UpdatePanel ID="upnl11" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbClassTeacher" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width: 100%" valign="top">
                                    <table id="Table1" runat="server" visible="true" align="center">
                                        <tr id="trStudentDetails" runat="server">
                                            <td width="110px">
                                                <span class="text-dark font-weight-bold">
                                                    <asp:Label ID="Label3" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, ClassTeacher %>"
                                                        CssClass="text-dark font-weight-bold"></asp:Label>
                                                    :</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbClassTeacher" runat="server" ViewStateMode="Enabled" CssClass="ExLrgCombo"
                                                    AutoPostBack="true" OnSelectedIndexChanged="cmbClassTeacher_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                            <td style="width: 50px;">
                                            </td>
                                            <td width="70px">
                                                <asp:Label ID="Label4" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Student %>"></asp:Label>
                                                <span>:</span>
                                            </td>
                                            <td id="tdCmbStudents" runat="server">
                                                <asp:UpdatePanel ID="upnl1" runat="server" ViewStateMode="Enabled" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="cmbStudents" runat="server" CssClass="ExLrgCombo" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbStudents_SelectedIndexChanged">
                                                            <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cmbClassTeacher" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="80%">
                                        <tr align="left">
                                            <td style="text-align: left;">
                                                <table id="LegendTable" runat="server" visible="false" align="left">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label CssClass="ClsLblLgnd" ID="lblLegend" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                BackColor="#ffffcc" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label CssClass="ClsTextNormal" ID="lblDeactivatedUser" Font-Bold="true" runat="server"
                                                                EnableViewState="False" Text="Mandatory Documents."></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ViewStateMode="Enabled" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:ListView ID="lstvwConfiguredDocument" runat="server" DataKeyNames="StudentDocumentId, StandardwiseDocumentId,IsSubmitted,IsApplicable,IsSubmissionMandatory"
                                                            OnItemDataBound="lstvwConfiguredDocument_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table align="center" width="100%" runat="server" id="tblTermInfo" style="color: #333333"
                                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th align="center" style="padding-left: 10px; width: 126px">
                                                                            <asp:CheckBox ID="chkIsApplicableAll" runat="server" Text="<%$ Resources:LocalizedResources, IsApplicable%>"
                                                                                onclick="CheckAllUncheckAllsIsApplicableCheckBox()" CssClass="vertical-align-top all-checkbox" />
                                                                        </th>
                                                                        <th align="center" style="padding-left: 10px; width: 126px">
                                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" Text="<%$ Resources:LocalizedResources, IsSubmitted%>"
                                                                                onclick="CheckAllUncheckAlls()" CssClass="vertical-align-top all-checkbox" />
                                                                        </th>
                                                                        <th align="left" style="padding-left: 12px">
                                                                            <asp:Label ID="lblDocument" runat="server" Text="<%$ Resources:LocalizedResources, DocumentName %>" />
                                                                        </th>
                                                                        <th align="center" style="padding-left: 10px; width: 150px">
                                                                            <asp:Label ID="lblAttachment" runat="server" Text="<%$ Resources:LocalizedResources, AttachmentCount %>" />
                                                                        </th>
                                                                    </tr>
                                                                    <tr runat="server" id="itemPlaceholder">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr id="trData" runat="server" class="ClsGridRow">
                                                                    <td align="center" id="tdIsApplicable" runat="server" style="padding-left: 8px">
                                                                        <asp:CheckBox ID="chkIsApplicable" runat="server" />
                                                                        <asp:HiddenField ID="hidIsDocMandatory" runat="server" Value="0" />
                                                                    </td>
                                                                    <td align="center" id="tdSelect" runat="server" style="padding-left: 8px">
                                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                    </td>
                                                                    <td align="left" id="tdDocumentName" runat="server" style="padding-left: 8px">
                                                                        <asp:Label ID="lblDocumentName" CssClass="LblNormal" runat="server" Text='<%#Eval("DocumentName")%>'></asp:Label>
                                                                    </td>
                                                                    <td id="tdlnkAttachment" runat="server" align="center">
                                                                        <asp:LinkButton ID="lnkAttachment" runat="server" Text='<%#Eval("DocumentCount") %>'
                                                                            CausesValidation="false" ToolTip="Click to upload / delete attachment."></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="trData" runat="server" class="ClsGridAltRow">
                                                                    <td align="center" id="tdIsApplicable" runat="server" style="padding-left: 8px">
                                                                        <asp:CheckBox ID="chkIsApplicable" runat="server" />
                                                                        <asp:HiddenField ID="hidIsDocMandatory" runat="server" Value="0" />
                                                                    </td>
                                                                    <td align="center" id="tdSelect" runat="server" style="padding-left: 8px">
                                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                    </td>
                                                                    <td align="left" id="tdDocumentName" runat="server" style="padding-left: 8px">
                                                                        <asp:Label ID="lblDocumentName" CssClass="LblNormal" runat="server" Text='<%#Eval("DocumentName")%>'></asp:Label>
                                                                    </td>
                                                                    <td id="tdlnkAttachment" runat="server" align="center">
                                                                        <asp:LinkButton ID="lnkAttachment" runat="server" Text='<%#Eval("DocumentCount") %>'
                                                                            CausesValidation="false" ToolTip="Click to upload / delete attachment."></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td class="LblNoRecord" align="center">
                                                                            <asp:Label ID="lblNoDocument" runat="server" Text="No Record Found."></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cmbClassTeacher" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upnl10" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" OnClick="btnSave_Click"
                                                Visible="false" Text="<%$ Resources:LocalizedResources, Save%>" CausesValidation="true" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbClassTeacher" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <script type="text/javascript">
        _clientListViewId = "<%=this.lstvwConfiguredDocument.ClientID %>"
        _ClientChkAll = _clientListViewId + "_ChkSelectAll";
        _ClientIsApplicableAll = _clientListViewId + "_chkIsApplicableAll";
        function OpenPopup(querystring) {
            window.open('../Payroll/InvestmentDocumentPopup.aspx?' + querystring, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500').focus();
            return false;
        }
        function SetIsApplicableSatus(obj, iRowNo) {
            if (!obj.checked) {
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_ChkSelect").checked = false;
            }
        }
        function SetIsSubmittedSatus(obj, iRowNo) {
            if (obj.checked)
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_chkIsApplicable").checked = true;
        }
        function CheckAllUncheckAllsIsApplicableCheckBox() {
            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientIsApplicableAll).checked
            if (!checkAll) {
                $("#<%=lstvwConfiguredDocument.ClientID %>_tblTermInfo input[type=checkbox]").attr('checked', checkAll);
            }
            else {
                $("#<%=lstvwConfiguredDocument.ClientID %>_tblTermInfo input[type=checkbox][id$=chkIsApplicable]").attr('checked', checkAll);

            }

        }
        setTimeout(function () {
            $('#ctl00_MainBody_lblMessage').fadeOut('fast');
        }, 5000);
    </script>
</asp:Content>
