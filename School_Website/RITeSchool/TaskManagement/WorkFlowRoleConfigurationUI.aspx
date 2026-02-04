<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="WorkFlowRoleConfigurationUI.aspx.cs" Inherits="WorkFlowRoleConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="500px">
            <tr>
                <td>
                    <asp:CustomValidator ID="CstWorkFolwConfig" runat="server" ClientValidationFunction="CheckAtListOne"
                        SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                        CssClass="LblErrorMsg"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" EnableViewState="False"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="1" class="ClsTextNormal" align="center">
                </td>
            </tr>
        </table>
        <table width="50%" runat="server" id="tbldivErr" Visible="false">
            <tr align="left" width="100%" >
                <td align="left" width="100%" class="LblNoRecord">
                    <div runat="server" id="divErr" style="height:20px"  class="ClsConfigText"></div>
                    <asp:LinkButton ID="lnkTeacher" Height="20px" Font-Size="13px" runat="server" PostBackUrl="~/RITeSchool/Admin/TeacherUI.aspx">Teacher</asp:LinkButton><br />
                    <asp:LinkButton ID="lnkAdminStaff" Height="20px" Font-Size="13px" runat="server" PostBackUrl="~/RITeSchool/Admin/SupervisorDetailsUI.aspx">Admin Staff</asp:LinkButton><br />
                    <asp:LinkButton ID="lnkOtherStaff" Height="20px" Font-Size="13px" runat="server" PostBackUrl="~/RITeSchool/Payroll/OtherStaffUI.aspx">Other Staff</asp:LinkButton><br />                    
                                     
                </td>
            </tr>
        </table>        
        <table id="tblWorkFlowConfiguration">
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblUpdateSucess" runat="server" CssClass="ClsLabel" EnableViewState="False"
                        Font-Bold="True" ForeColor="Blue" Height="20px" Visible="False" Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table align="center">
                        <tr align="center" id="trAssignTaskBy" runat="server">
                            <td align="center" class="ClsBorderlight" colspan="1" style="padding-left: 5px;">
                                <span class="ClsLabel">Task Assigner:</span> &nbsp;
                            </td>
                            <td align="left" style="width: 100px" colspan="1">
                                <asp:DropDownList ID="cmbAssignTaskBy" CssClass="MidCombo" AutoPostBack="true" runat="server"
                                    Width="222px" OnSelectedIndexChanged="cmbAssignTaskBy_SelectedIndexChanged" Height="22px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 50px" id="trLbl" runat="server">
                    <span class="ClsLabel">Can Assign Task To :</span>
                </td>
            </tr>
            <tr id="trListView" runat="server">
                <td align="center" colspan="1">
                    <asp:ListView ID="lstvwWorkFlowConfiguration" runat="server" OnItemDataBound="lstvwWorkFlowConfiguration_ItemDataBound"
                        DataKeyNames="WorkFlowLevelId, AssignedByDesignationId,AssignedToDesignationId,Is_Deleted">
                        <LayoutTemplate>
                            <div id="Div1" class="GridBorder" runat="server" style="height: 337px; overflow: auto;">
                                <table align="center" runat="server" id="tblWorkFlowConfigInfo" width="480px" style="color: #333333"
                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                    <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="center" id="chkAll" runat="server" style="width: 50px">
                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                        </th>
                                        <th align="left" class="paddingL" style="width: 300px">
                                            Role
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr id="trData" runat="server" class="ClsGridRow">
                                <td align="center" style="width: 50px;">
                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                </td>
                                <td align="left" class="paddingL">
                                    <asp:Label ID="lblDesg" runat="server" Text='<%#Eval("Designation")%>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr id="trData" runat="server" class="ClsGridAltRow">
                                <td align="center" style="width: 50px;">
                                    <asp:CheckBox ID="ChkSelect" runat="server" onclick="CheckAtListOne()" />
                                </td>
                                <td align="left" class="paddingL">
                                    <asp:Label ID="lblDesg" runat="server" Text='<%#Eval("Designation")%>'></asp:Label>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" disable-page="true" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="False"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientListViewId = "<%=this.lstvwWorkFlowConfiguration.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"

        _ClientChkAll = _clientListViewId + "_ChkSelectAll";
        _clientCstWorkFolwConfig = "<%=this.CstWorkFolwConfig.ClientID %>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"

        function CheckAllUncheckAlls() {

            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var enble
            var iRowCount = 0
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll

                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            }
        }

        var Page_IsValid = true;
        function CheckAtListOne() {
        	Page_IsValid = true;	
            var chk;
            var iRowCount = 0;
            var chkCount = 0;

            if (document.getElementById(_clientlblUpdateSucess) != undefined) {
                document.getElementById(_clientlblUpdateSucess).innerHTML = ""
            }

            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    chkCount = chkCount + 1;
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (chkCount == 0) {
            	alert("At least one designation should be selected for task assignment.");
            	Page_IsValid = false;
                return false
            }
            return true
        }

        function ResetUpdateLbl() {

            if (document.getElementById(_clientlblUpdateSucess) != null)
                document.getElementById(_clientlblUpdateSucess).style.display = "none"

        }
        
    </script>

</asp:Content>
