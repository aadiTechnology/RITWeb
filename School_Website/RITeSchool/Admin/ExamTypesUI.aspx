<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ExamTypesUI.aspx.cs" Inherits="ExamTypesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                    vertical-align: top">
                    <tr>
                        <td id="MainDataTable" align="center">
                            <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 77%">
                                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                            Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label></asp:Panel>
                                                </td>
                                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 77%">
                                                    <asp:Panel ID="pnlCheckdepandency" runat="server" Width="96%">
                                                        <asp:Label ID="lblCheckDependency" Style="text-align: left" runat="server" ForeColor="Red"
                                                            Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                                </td>
                                            </tr>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="1" id="tdMessage" runat="server" class="ClsTextNormal" align="center">
                <asp:Label ID="lblUpateMessage" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="tblDesignationName" runat="server" border="0" cellpadding="1" cellspacing="2" width="25%" align="center">
        <tr align="center">
            <td align="left" class="ClsBorderLight" style="width: 49%">
                <span class="ClsLabel">Subject :</span>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSubjects" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged" />
                                        <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
            </td>
        </tr>
    </table>
   <table width ="100%">
       <tr>
            <td align="right">
                <asp:LinkButton ID="lnlRetirementNotice" runat="server" Text="Add Exam Type" Style="padding-right: 10px; top: 20px; height: 19px;"
                    CssClass="SubTitle" OnClientClick="OpenAttendancePopup()"></asp:LinkButton>
            </td>
        </tr>
    <tr>
        <td align="center">
           
          <table width="50%">
                    <tr>
                        <td align="center">
                            <asp:ListView ID="lstvwExamTypes" runat="server" DataKeyNames="TestTypeId,Flag" OnItemDataBound="lstvwExamTypes_ItemDataBound">
                                <LayoutTemplate>
                                    <table cellpadding="0" cellspacing="1" class="GridBorder" width="100%" style="color: #333333">
                                        <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="center" id="thChkSelectAll" runat="server" style="width: 40px; font-size: 9pt;">
                                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckUncheckAll(this);" />
                                            </th>
                                            <th align="left" class="paddingL" style="font-size: 9pt;">
                                                Exam Type
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
                                            <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("TestTypeName") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td class="LblNoRecord" align="center">
                                            No record found.
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr>
            <td align="center">
                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                        CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" UseSubmitBehavior="false" />
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" disable-page="true"
                                        OnClientClick="ClearMessages();" Style="margin-top: 10px;" TabIndex="3" OnClick="btnSave_Click" />
            </td>
        </tr>
                </table>
     
        </td>
    </tr>
    
    </table>
    <script type="text/javascript">

        var _clientlstvwExamTypes = '<%= this.lstvwExamTypes.ClientID %>';
        var _clientlblUpdateMessage = '<%= this.lblUpateMessage.ClientID %>';
        var _chkSelect = '_chkSelect';
        var _ctrl = '_ctrl';


        function OpenAttendancePopup() {
            //var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('ExamTypePopup.aspx', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500')
            return false;
        }
        // This function is used to Check Uncheck all checkboxes in the ListView
        function CheckUncheckAll(src) {
            if (src == null)
                src = $get(_clientlstvwExamTypes + '_chkSelectAll');

            var iRowCount = 0;
            var chk = $get(_clientlstvwExamTypes + _ctrl + iRowCount + _chkSelect);
            while (chk != null) {
                chk.checked = src.checked;

                iRowCount++;
                chk = $get(_clientlstvwExamTypes + _ctrl + iRowCount + _chkSelect);
            }
        }

        // This function is used to clear Update Message.
        function ClearMessages() {
            var lblUpdateMsg = $get(_clientlblUpdateMessage);
            if (lblUpdateMsg)
                lblUpdateMsg.innerHTML = empty;
        }
    </script>
    </table>
</asp:Content>
