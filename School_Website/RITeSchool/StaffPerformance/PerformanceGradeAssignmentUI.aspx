<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PerformanceGradeAssignmentUI.aspx.cs" Inherits="PerformanceGradeAssignmentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        #tdCombo > select {
            width: 105px !important;
        }

    </style>

    <asp:UpdatePanel ID="updtpnl1" runat="server">
        <ContentTemplate>
            <div>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="valSummary" runat="server" CssClass="lblNormal" ShowSummary="true"
                                    ValidationGroup="Block" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 500px; height: 20px">
                    <asp:Label ID="lblErrorMesage" runat="server"></asp:Label>
                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                        Visible="true" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                </div>
                <div style="margin-top: 10px">
                    <table>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="left" class="ClsBorderlight" width="80px">
                                            <asp:Label ID="lblYear" CssClass="clsLabel" runat="server" Text="<%$ Resources:LocalizedResources,Year%>" />
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td id="tdCombo" style="white-space: nowrap">
                                            <asp:DropDownList ID="cmbYear" runat="server"  CssClass="SmlCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="cmbYear_SelectedIndexChanged">
                                            </asp:DropDownList>                                           
                                            <asp:RequiredFieldValidator ID="reqCmbYear" runat="server" Display="None" ControlToValidate="cmbYear"
                                                CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="Year should be selected."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight" width="80px">
                                            <asp:Label ID="Label1" CssClass="clsLabel" runat="server" Text="Status" />
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td id="td1" style="white-space: nowrap">
                                            <asp:RadioButton ID="optSubmitted" runat="server" Text="Submitted" 
                                                GroupName="Status" CssClass="ClsLabel" AutoPostBack="True" 
                                                oncheckedchanged="optSubmitted_CheckedChanged" />
                                            <asp:RadioButton ID="optPending" runat="server" Text="Pending" 
                                                GroupName="Status" CssClass="ClsLabel" AutoPostBack="True" 
                                                oncheckedchanged="optPending_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 48px; visibility: hidden; display: none;">
                    <div style="float: left; padding-left: 340px">
                        <table>
                            <tr>
                                <td>
                                    <span class="ClsLblLgnd" style="font: Bold; width: 50px">Legend :</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblPendingFee" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" CssClass="PendingFees" EnableViewState="False" Height="20px"
                                        Text=" " Width="20px"> <img height="20px" src="../images/spacer.gif" width="20px" /></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblLegend" runat="server" CssClass="ClsTextNormal" Style="font-weight: bold;"
                                        Text="Deactivated Users" />                                    
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div>
                    <div style="width: 750px;">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:ListView ID="lstvwPerformanceGradeAssignment" runat="server" DataKeyNames="UserId,IsSupervisor"
                                        OnItemCommand="lstvwPerformanceGradeAssignment_ItemCommand" OnItemDataBound="lstvwPerformanceGradeAssignment_ItemDataBound">
                                        <LayoutTemplate>
                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder"
                                                id="tblStudent" runat="server">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th class="paddingL" style="width: 400px;" align="left">
                                                        <asp:Label ID="lblStaffname" runat="server" Text="<%$ Resources:LocalizedResources, StaffName1%>"
                                                            CausesValidation="false" ForeColor="Black"> </asp:Label>
                                                    </th>
                                                    <th style="display:none;">
                                                        <asp:Label ID="lblInvitee" runat="server" Text="<%$ Resources:LocalizedResources, Invitee%>" CausesValidation="false"
                                                            ForeColor="Black"> </asp:Label>
                                                    </th>
                                                    <th>
                                                        <asp:Label ID="lblPerformanceEval" runat="server" Text="<%$ Resources:LocalizedResources, PerformanceEvaluation%>"
                                                            CausesValidation="false" ForeColor="Black"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="tr2" runat="server" class="ClsGridRow">
                                                <td class="paddingL" style="width: 400px;">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>' />
                                                </td>
                                                <td class="paddingL" style="width: 200px;display:none;" align="center">
                                                    <asp:ImageButton ID="imgBtnInvitee" runat="server" CausesValidation="false" CommandName="Invitee"
                                                        ImageUrl="../images/selection5.gif" ToolTip="View Invitee Member(s)" />                                                    
                                                </td>
                                                <td class="paddingL" style="width: 200px;" align="center">
                                                    <asp:ImageButton ID="imgBtnSelect" runat="server" CausesValidation="false" CommandName="SelectCommand"
                                                        ImageUrl="../images/selection5.gif" ToolTip="Performance Evaluation"  />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="tr3" runat="server" class="ClsGridAltRow">
                                                <td class="paddingL" style="width: 400px;">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>' />
                                                </td>
                                                <td class="paddingL" style="width: 200px;display:none" align="center">
                                                    <asp:ImageButton ID="imgBtnInvitee" runat="server" CausesValidation="false" CommandName="Invitee"
                                                        ImageUrl="../images/selection5.gif" ToolTip="View Invitee Member(s)" />                                                    
                                                </td>
                                                <td class="paddingL" style="width: 200px;" align="center">
                                                    <asp:ImageButton ID="imgBtnSelect" runat="server" CausesValidation="false" CommandName="SelectCommand"
                                                        ImageUrl="../images/selection5.gif"  ToolTip="Performance Evaluation"/>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td class="LblNoRecord" align="center" colspan="4" style="width: 750px; float: left">
                                                    No record found.
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div>
                    <asp:HiddenField ID="hidReportingUserId" runat="server" />
                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                </div>
            </div>
            <div id="divInviteeList" runat="server" style="visibility: hidden; display: none;
                position: absolute; z-index: 1000; margin: 0px; padding: 0px; width: 500px; height: 300px;
                border-width: 1px; left: 5px; top: 150px; line-height: normal; border: solid 2px darkgreen;
                margin: -110px 0px 0px 00px; background-color: white;">
                <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                    background-repeat: repeat-x; color: Black; width: 500px; text-align: right">
                    <div style="font-size: 12px; width: 350px; letter-spacing: 1px; padding-left: 8px;
                        font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                        <asp:Label ID="lblInvitee" runat="server" Text="<%$ Resources:LocalizedResources, Invitee%>" CausesValidation="false"
                                                            ForeColor="Black"> </asp:Label>
                    </div>
                    <span style="cursor: hand" onclick="javascript:HidePopup();">
                        <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                            border="0" />
                    </span>
                </div>
                <div style="padding: 2px; background-color: white; text-align: center; vertical-align: top;
                    color: #333; overflow: auto; height: 200px; width: 400px; margin-left: 1px" id="Div5">
                    <table>
                        <tr>
                            <td colspan="2">                                
                                <asp:ListView ID="lstvwInvitee" runat="server" DataKeyNames="UserId">
                                    <LayoutTemplate>
                                        <table id="lstvwPayFee" width="350px" style="color: #333" cellpadding="3" cellspacing="1"
                                            class="GridBorder">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th id="thchkPay" runat="server" align="center" style="padding: 0;">
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                </th>
                                                <th align="left" style="padding: 0 0 0 15px;">
                                                    <asp:Label ID="lblStaffname" runat="server" Text="<%$ Resources:LocalizedResources, StaffName1%>"
                                                            CausesValidation="false" ForeColor="Black"> </asp:Label>
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class="ClsGridRow">
                                            <td align="center" id="tdchkPay" runat="server">
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsSubmitted") %>' />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' CssClass="ClspaddingL" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ClsGridRow">
                                            <td align="center" id="tdchkPay" runat="server">
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsSubmitted") %>' />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' CssClass="ClspaddingL" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <tr>
                                            <td class="LblNoRecord" align="center" colspan="4" style="width: 350px; float: left">
                                                No record found.
                                            </td>
                                        </tr>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="bottom">
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtnMid" CausesValidation="false"
                                    Width="75px" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClosePopUp" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtnMid" CausesValidation="false"
                                    Width="75px" OnClientClick="javascript:HidePopup();return false;" />
                            </td>
                        </tr>
                    </table>
                </div>               
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">

        _clientListViewId = "<%=this.lstvwInvitee.ClientID %>"
        _ClientChkAll = _clientListViewId + "_chkSelectAll";

        function OpenPopup(obj) {
            _clientdivTemplates = "<%=this.divInviteeList.ClientID %>"
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divInviteeList.ClientID %>").style
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
            $get("<%=this.divInviteeList.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divInviteeList.ClientID %>").style.display = "none"
            return false
        }
        
        function CheckAllUncheckAlls() {
            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked

            var iRowCount = 0
            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_chkSelect");
            }
        }
        
    </script>
</asp:Content>