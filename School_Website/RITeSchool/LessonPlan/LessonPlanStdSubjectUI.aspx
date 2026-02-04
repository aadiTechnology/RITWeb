<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LessonPlanStdSubjectUI.aspx.cs" Inherits="LessonPlanStdSubjectUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
<style>
.clsLabel, .ClsLabel 
{
    font-family: open sans;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        .ClsLabel
        {
            font-family:Open Sans;
        }
        
    </style>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                                <asp:CustomValidator ID="cstVal" runat="server" ClientValidationFunction="ValidateSubjects"
                                    Display="None" ErrorMessage="At least one subject should be selected."></asp:CustomValidator>
                            </td>
                            <td align="right">
                                <span class="ClsMdtStar">*</span>
                                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="50%">
                                <tr>
                                    <td align="center" id="tdMessage" runat="server">
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left" class="ClsBorderlight" width="100px">
                                <span class="ClsLabel">Standard : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="50%">
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwSubjects" runat="server" OnItemDataBound="lstvwSubjects_ItemDataBound"
                                            DataKeyNames="SubjectId">
                                            <LayoutTemplate>
                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                    cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" width="30px">
                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckUncheckAll()" />
                                                        </th>
                                                        <th align="left" style="padding-left: 10px;">
                                                            Subject
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trItem" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </td>
                                                    <td class="paddingL">
                                                        <asp:Label ID="lblSubhect" runat="server" Text='<%#Eval("SubjectName") %>' CssClass="ClsLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </td>
                                                    <td class="paddingL">
                                                        <asp:Label ID="lblSubhect" runat="server" Text='<%#Eval("SubjectName") %>' CssClass="ClsLabel"></asp:Label>
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" Enabled="False"
                                OnClick="btnSave_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientlstvwSubjects = "<%=this.lstvwSubjects.ClientID %>"
        function CheckUncheckAll() {
            var checkAll = document.getElementById(_clientlstvwSubjects + "_ChkSelectAll").checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwSubjects + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwSubjects + "_ctrl" + iRowCount + "_chkSelect")
            }
        }

        function ValidateSubjects(oSrc, args) {
            var isFound = false;
            var iRowCount = 0
            var chk = document.getElementById(_clientlstvwSubjects + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.checked) {
                    isFound = true;
                    break;
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwSubjects + "_ctrl" + iRowCount + "_chkSelect")
            }

            args.IsValid = isFound;
            return !isFound;
        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
