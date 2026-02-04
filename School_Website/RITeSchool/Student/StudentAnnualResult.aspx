<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="StudentAnnualResult.aspx.cs" Inherits="StudentProgressSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" style="width: 100%;">
        <table width="97%">
            <tr>
                <td>
                    <asp:ValidationSummary ID="valSum" runat="server" ShowMessageBox="false" ShowSummary="true"
                        CssClass="LblErrorMsg" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlFilter" runat="server">
                        <table width="100%" cellpadding="0" cellspacing="1">
                            <tr>
                                <td runat="server" id="tdlblTeacher" class="ClsBorderlight">                                   
                                        <span class="ClsLabel" id="lblTeacher"> Class Teacher :</span></td>
                                <td runat="server" id="tdcmbTeachers" class="ClsBorderlight">
                                    <asp:DropDownList ID="cmbTeachers" runat="server" AutoPostBack="true" CssClass="ExLrgCombo"
                                        OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="cmp_TeacherName" runat="server" ControlToValidate="cmbTeachers"
                                        Display="None" ErrorMessage="Class Teacher should be selected." Operator="NotEqual"
                                        ValueToCompare='0'></asp:CompareValidator>
                                    <span style="color: #ff0000" runat="server" id="spnMandatory">*</span></td>
                                <td runat="server" id="tdlblStudent" class="ClsBorderlight" style="width: 56px">                                    
                                    <span class="ClsLabel" id="lblStudent"> Student :</span></td>
                                <td runat="server" id="tdUPanelStudent" class="ClsBorderlight">
                                    <asp:UpdatePanel ID="UPanelStudent" runat="server">
                                        <ContentTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="1">
                                                <tr>
                                                    <td  id="td1">
                                                        <asp:DropDownList ID="cmbStudents" runat="server" CssClass="ExLrgCombo" OnSelectedIndexChanged="cmbStudents_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hidQery" runat="server" />
                                                    </td>
                                                    <td runat="server" id="tdbtnShow">
                                                        <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtnSml" OnClick="btnShow_Click"
                                                            UseSubmitBehavior="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td runat="server" id="tdbtnPrint" class="ClsBorderlight" align="right">
                                    <asp:Button ID="btnPrint" runat="server" Text="Print Preview" CausesValidation="true"
                                        CssClass="ClsBtnMid" />
                                </td>
                                <td align="left" colspan="1" runat="Server" id="tdhlnkToppers">
                                    <asp:HyperLink Height="20px" ID="hlnkToppers" CssClass="ToprLinkHlilight ClsPaddingGen LblNrmlB " Enabled="False"
                                        NavigateUrl="../Student/ExamToppersUI.aspx" Target="_blank" runat="server" Text="Toppers"></asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel runat="server" ID="uPnl" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnCancelUp" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="1px"
                                            CausesValidation="false" CssClass="ClsBtnSml" OnClick="btnCancel_Click" Text="Back" /></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="GridViewScrollContainer" runat="server" Visible="true">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="100%" EnableViewState="false">
                                            <table width="100%">
                                                <tr id="trErr" runat="server" visible="false" enableviewstate="false">
                                                    <td align="center" class="LblNoRecord" style="width: 100%">
                                                        <asp:Label ID="lblErrorMsg" runat="server" Visible="False" CssClass="LblNoRecord"
                                                            EnableViewState="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/RITeSchool/Admin/displayassignedclassteacherui.aspx"
                                                            Visible="false" CssClass="ClsConfigLink">Class Teacher Assignment</asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="LblErrorMsg">
                                                    <asp:UpdatePanel ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server"
                                                        ID="UpdatePanel1">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblErrorsMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                Visible="False" CssClass="ClsConfigHead" EnableViewState="False"></asp:Label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        &nbsp;<asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            CausesValidation="false" CssClass="ClsBtnSml" OnClick="btnCancel_Click" Text="Back"
                                            Visible="True" /></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hidStudId" runat="server" />
    <asp:HiddenField ID="hidUserHasFullAccess" runat="server" />

    <script language="javascript" type="text/javascript">
        _ClientbtnShow = "<%=this.btnShow.ClientID %>"
        function GeneratePrint() {
            _sClientcmbTeachers = "<%=this.cmbTeachers.ClientID %>"
            _sClientcmbStudents = "<%=this.cmbStudents.ClientID %>"
            _sClienthidQery = "<%=this.hidQery.ClientID %>"
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            window.open("../Student/StudentAnnualResultPrint.aspx?" + document.getElementById(_sClienthidQery).value, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=' + screen.width + ' ,height=600')
        }
        function ShowToppers(sQryStr) {
            _sClienthlnkToppers = "<%=this.hlnkToppers.ClientID %>"
            if ((document.getElementById(_sClienthlnkToppers) == null) || (document.getElementById(_sClienthlnkToppers) == "") || (document.getElementById(_sClienthlnkToppers).disabled))
                return false
            window.open(sQryStr, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=900,height=600')
            return false
        }
        function DisableButtons() {
            var isPageValid = false
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            if (isPageValid) {
                document.getElementById(_ClientbtnShow).disabled = true
            } 
        }
    </script>
</asp:Content>
