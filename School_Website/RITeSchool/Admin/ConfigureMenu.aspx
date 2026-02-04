<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfigureMenu.aspx.cs" Inherits="ConfigureMenu"
    MasterPageFile="../MasterPages/MasterPage.master" ViewStateMode="Disabled" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" style="width: 100%;" height="100%" border="0" cellpadding="0"
        cellspacing="0">
        <tr valign="top">
            <td class="NewInnerTopTL">
                &nbsp;
            </td>
            <td id="tdMainData" colspan="2" align="center" valign="top" class="td-vertical-align-top">
                <!-- Main Data Start-->
                <table width="100%">
                    <tr>
                        <td align="left" style="width: 40%;">
                            <asp:ValidationSummary ID="valSumAdd" runat="server" ValidationGroup="valAdd" CssClass="ClsLabel" />
                            <asp:ValidationSummary ID="valsumEdit" runat="server" ValidationGroup="valEdit" CssClass="ClsLabel" />
                        </td>
                        <td align="center" style="width: 40%;">
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                CssClass="ClsHilightBGB" Width="80%" EnableViewState="false"></asp:Label>
                        </td>
                        <td align="right" style="width: 20%;">
                            <span class="ClsMdtStar">* Mandatory Fields</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" valign="top">
                            <asp:Panel ID="Panel1" runat="server" ViewStateMode="Enabled">
                                <div id="DivViewMenues" runat="server">
                                    <table width="100%" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td align="left" width="10%" rowspan="5" valign="top" class="ClsBorderlight td-vertical-align-top">
                                                <table cellpadding="0" cellspacing="1">
                                                    <tr id="trInternalMenus" runat="server">
                                                        <td class="ConfigHeadBG">
                                                            <span class="LblIB">Internal Menus</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlMenuConfigure" runat="server" ScrollBars="Auto" Width="250px" Height="200px">
                                                                <asp:TreeView runat="Server" ID="Menu_Configure" OnSelectedNodeChanged="Menu_Configure_SelectedNodeChanged"
                                                                    ViewStateMode="Enabled" Font-Names="Verdana" Font-Size="8pt" SelectedNodeStyle-Font-Bold="true"
                                                                    SelectedNodeStyle-ForeColor="darkgreen" SelectedNodeStyle-Font-Underline="true"
                                                                    ForeColor="Black" ShowLines="True">
                                                                    <SelectedNodeStyle Font-Bold="True" ForeColor="DodgerBlue" />
                                                                </asp:TreeView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr id="trMenuSeparator" runat="server">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trExternalMenus" runat="server">
                                                        <td class="ConfigHeadBG">
                                                            <span class="LblIB">External Menus</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlExternal" runat="server" ScrollBars="Auto" Width="250px" Height="200px">
                                                                <asp:TreeView runat="Server" ID="trvExternal" OnSelectedNodeChanged="Menu_Configure_SelectedNodeChanged"
                                                                    ViewStateMode="Enabled" Font-Names="Verdana" Font-Size="8pt" SelectedNodeStyle-Font-Bold="true"
                                                                    SelectedNodeStyle-ForeColor="darkgreen" SelectedNodeStyle-Font-Underline="true"
                                                                    ForeColor="Black" ShowLines="True">
                                                                    <SelectedNodeStyle Font-Bold="True" ForeColor="DodgerBlue" />
                                                                </asp:TreeView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left" valign="top" width="90%">
                                                <table width="100%" cellpadding="0" cellspacing="1">
                                                    <tr>
                                                        <td class="ClsBorderlight" colspan="7" style="vertical-align: middle">
                                                            <table cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td width="15%" style="vertical-align: middle;" class="ClsBorderlight">
                                                                        <span class="ClsLabel">Menu Name :</span>
                                                                    </td>
                                                                    <td class="ClsMdtStar" style="width: 35%">
                                                                        <asp:TextBox ID="txtMenuNameUpdate" CssClass="LrgTxtBox" runat="server" MaxLength="60"
                                                                            Width="170px"></asp:TextBox>
                                                                        *
                                                                    </td>
                                                                    <td class="ClsBorderlight" width="15%">
                                                                        <span class="ClsLabel">Priority :</span>
                                                                    </td>
                                                                    <td class="ClsMdtStar" colspan="2">
                                                                        <asp:TextBox ID="NmcBxPriorityUpdate" CssClass="SmlTxtBox" runat="server" onblur="extractNumber(this,0,false);"
                                                                            onkeyup="extractNumber(this,0,false);" MaxLength="4" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                                        *
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="NmcBxPriorityUpdate"
                                                                            Display="None" ErrorMessage="Please enter priority." ValidationGroup="valEdit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="ClsBorderlight" style="vertical-align: middle;">
                                                                        <span class="ClsLabel" style="width: 100px">Top Menu :</span>
                                                                    </td>
                                                                    <td class="ClsMdtStar">
                                                                        <asp:DropDownList ID="cmbParentMenu" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                                            ViewStateMode="Enabled" OnSelectedIndexChanged="cmbParentMenu_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMenuNameUpdate"
                                                                            Display="None" ErrorMessage="Please enter menu name." ValidationGroup="valEdit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="ClsBorderlight">
                                                                        <asp:CheckBox ID="chkIsDefault" runat="server" CssClass="ClsLabel" Text="Is Default"
                                                                            Width="100px" Visible="False" />
                                                                        <asp:CheckBox ID="chkIsActive" runat="server" CssClass="ClsLabel" Text="Is Active?"
                                                                            Width="100px" />
                                                                    </td>
                                                                    <td class="ClsBorderlight" style="vertical-align: middle;">
                                                                        <asp:CheckBox ID="chkApplicable" runat="server" CssClass="ClsLabel" Text="External"
                                                                            Width="100px" Checked="false" />
                                                                    </td>
                                                                    <td class="ClsBorderlight" style="vertical-align: middle;">
                                                                        <asp:CheckBox ID="chkOnPopUp" runat="server" CssClass="ClsLabel" Text="Is LogOn Message?"
                                                                            AutoPostBack="true" Width="140px" Checked="false" OnCheckedChanged="chkOnPopUp_CheckedChanged" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trShowSubMenu" runat="server" viewstatemode="Enabled" visible="true" >
                                                                    <td class="ClsBorderlight"  style="vertical-align: middle;">
                                                                        <span class="ClsLabel">Show Sub Menu :</span>
                                                                    </td>
                                                                    <td align="left" class="ClsBorderlight" colspan="3">
                                                                       
                                                                        <table id="ShowAllSubmenu" >

                                                                            <tr>
                                                                            <td>
                                                                             <asp:RadioButton ID="optAllUpdate" runat="server" GroupName="ShowSubMenu" Text="All" />
                                                                              <asp:RadioButton ID="optTopUpdate" runat="server" GroupName="ShowSubMenu" Text="Top" />
                                                                            </td>
                                                                                <td id="trshowtopAll" style="display: none;">
                                                                                    <asp:TextBox ID="txtShowTopUpdate" runat="server" MaxLength="4" onblur="extractNumber(this,0,false);"
                                                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="SmlTxtBox"
                                                                                        Width="60px" EnableViewState="false" />
                                                                                    <asp:CheckBox ID="chkAllsubMenu" runat="server" Text="Applicable for all  levles"
                                                                                        Style="white-space: nowrap; display:none;" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="ClsBorderlight">
                                                                        <span class="ClsLabel">Sub Menu :</span>
                                                                    </td>
                                                                    <td class="ClsMdtStar">
                                                                        <asp:DropDownList ID="cmbSubMenu" runat="server" AutoPostBack="false" CssClass="MidCombo"
                                                                            ViewStateMode="Enabled" Enabled="false">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMenuNameUpdate"
                                                                            Display="None" ErrorMessage="Please enter sub menu name." ValidationGroup="valEdit"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="ClsBorderlight">
                                                                        <span class="ClsLabel">End Date :</span>
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="txtDateUpdate" runat="server" CssClass="SmlTxtBox" MaxLength="11"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtDateUpdate" Format="dd MMM yyyy"
                                                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid End date." />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="ClsBorderlight">
                                                                        <asp:Label ID="Label6" runat="server" Text="Applicable to :" CssClass="ClsLblLgnd"
                                                                            Style="padding-left: 100px; white-space: nowrap;" EnableViewState="False"></asp:Label><br />
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:CheckBox ID="chkAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>"
                                                                            Style="white-space: nowrap;" onclick="CheckAllUncheckAlls()" />
                                                                    </td>
                                                                    <td align="left" valign="top" class="ClsBorderlight">
                                                                        <asp:CheckBoxList ID="chkListRoles" runat="server" CellPadding="0" CellSpacing="0"
                                                                            ViewStateMode="Enabled" CssClass="ClsBorderLight" RepeatColumns="2" RepeatDirection="Horizontal"
                                                                            Width="100%">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                    <td width="1%">
                                                                        <span class="ClsMdtStar">*</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CustomValidator ID="cstRoleValidate" runat="server" ClientValidationFunction="CheckBoxListRoles"
                                                                            ErrorMessage="At least one user role should be selected." Display="None" ValidationGroup="valEdit"
                                                                            CssClass="LblErrorMsg"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <CKEditor:CKEditorControl ID="FCKMenuView" Toolbar="Bold|Italic|Underline|Strike|-|Subscript|Superscript NumberedList|BulletedList|NumberedList|-|Indent  /|JustifyLeft|JustifyCenter|JustifyRight|JustifyFull / |Styles|Format|Font|FontSize|TextColor|BGColor /MindTouchDeki_InsertLink|MindTouchDeki_InsertImage|MindTouchDeki_AttachImage|MindTouchDeki_InsertVideo|MindTouchDeki_InsertTemplate|Table /Cut|Copy|Paste|PasteText|PasteWord|-|SpellCheck /|Undo|Redo|-|Find|Replace|-|SelectAll|RemoveFormat| /|Maximize|ShowBlocks|"
                                                                BasePath="../ckeditor/" Width="100%" runat="server" Height="350px" ToolbarCanCollapse="True"
                                                                ReadOnly="false"></CKEditor:CKEditorControl>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="7" style="width: 80%">
                                                            <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="ClsBtn"
                                                                Text="Back" OnClick="ImgBtnBack_Click" />
                                                            <asp:Button ID="imgbtnAdd" runat="server" CausesValidation="False" Text="Add" CssClass="ClsBtn"
                                                                ViewStateMode="Enabled" OnClick="imgbtnAdd_Click" />
                                                            <asp:Button ID="imgbtnUpdate" ViewStateMode="Enabled" ValidationGroup="valEdit" runat="server"
                                                                OnClick="imgbtnUpdate_Click" disable-page="true" Text="Update" CausesValidation="true"
                                                                CssClass="ClsBtn" />
                                                            <asp:Button ID="imgBtnDelete" runat="server" CausesValidation="False" Text="Delete"
                                                                CssClass="ClsBtn" OnClick="ImgBtnDelete_Click" />
                                                            <asp:Button ID="btnPreview" runat="server" CausesValidation="False" Text="Preview"
                                                                CssClass="ClsBtn" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ViewStateMode="Enabled">
                                <div id="DivAddNewMenu" runat="server">
                                    <table width="100%" cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td align="left" style="vertical-align: middle; width: 15%;" class="ClsBorderlight">
                                                <span class="ClsLabel">Menu Name :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="width: 35%">
                                                <asp:TextBox ID="txtMenuName" CssClass="ExLrgTxtBox" runat="server" MaxLength="60"></asp:TextBox>
                                                *
                                                <asp:RequiredFieldValidator ID="RFV_MenuName" runat="server" ControlToValidate="txtMenuName"
                                                    Display="None" ErrorMessage="Please enter menu name." ValidationGroup="valAdd"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="left" class="ClsBorderlight" style="width: 15%">
                                                <span class="ClsLabel">Priority :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="vertical-align: middle; width: 35%;">
                                                <asp:TextBox ID="NmBoxPriority" runat="server" MaxLength="4" onblur="extractNumber(this,0,false);"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="SmlTxtBox"
                                                    EnableViewState="false" />
                                                *<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="NmBoxPriority"
                                                    Display="None" ErrorMessage="Please enter priority." ValidationGroup="valAdd"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="vertical-align: middle" class="ClsBorderlight">
                                                <span class="ClsLabel">Top Menu :</span>
                                            </td>
                                            <td align="left" class="">
                                                <asp:DropDownList ID="cmbParentMenuAdd" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                    ViewStateMode="Enabled" OnSelectedIndexChanged="cmbParentMenuAdd_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:CheckBox ID="chkAddIsActive" runat="server" CssClass="ClsLabel" Text="Is Active"
                                                    Width="100px" Checked="false" />
                                            </td>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:CheckBox ID="chkApplicableAdd" runat="server" CssClass="ClsLabel" Text="Display to Guest"
                                                    EnableViewState="false" Visible="false" Checked="false" />
                                            </td>
                                        </tr>
                                        <tr id="trAddShowSubMenu" runat="server" viewstatemode="Enabled" visible="true">
                                            <td class="ClsBorderlight" style="vertical-align: middle;" >
                                                <span class="ClsLabel">Show Sub Menu :</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight" colspan="3">
                                            <table id="ShowAllSubmenuAdd">
                                              <tr>
                                              <td>

                                                <asp:RadioButton ID="optAllAdd" runat="server" GroupName="ShowSubMenuAdd" Text="All" />
                                                <asp:RadioButton ID="optTopAdd" runat="server" GroupName="ShowSubMenuAdd" Text="Top"  />
                                               </td>                                              
                                                <td id="tdHideControls" style="display: none;">
                                                        <asp:TextBox ID="txtShowTopAdd" runat="server" MaxLength="4" onblur="extractNumber(this,0,false);"
                                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="SmlTxtBox"
                                                            Width="60px" EnableViewState="false" viewstatemode="Enabled" />
                                                        <asp:CheckBox ID="chkAllsubMenuAdd" runat="server" Text="Applicable for all levles"
                                                            Style="white-space: nowrap ; display:none;"  />
                                                   </td>
                                                   </tr>
                                                    </table>
                                                    </td>
                                                   </tr>
                                        
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">Sub Menu :</span>
                                            </td>
                                            <td class="ClsMdtStar">
                                                <asp:DropDownList ID="cmbSubMenuAdd" runat="server" AutoPostBack="false" CssClass="MidCombo"
                                                    ViewStateMode="Enabled">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">End Date :</span>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="SmlTxtBox" MaxLength="11"></asp:TextBox>
                                                <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                    Culture="en" ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid from date."
                                                    ControlFocusOnError="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="Label1" runat="server" Text="Applicable to :" CssClass="ClsLblLgnd"
                                                    Style="padding-left: 150px; white-space: nowrap;" EnableViewState="False"></asp:Label><br />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="chkAddAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>"
                                                    Style="white-space: nowrap;" onclick="CheckAllUncheckAllsForAdd()" />
                                            </td>
                                            <td align="left" valign="top" style="background-color: #fff; border: 1px solid #B7B7FF;
                                                font-size: 9pt; margin: 0; padding: 0; width: 20px">
                                                <asp:CheckBoxList ID="chkAddListRoles" runat="server" CellPadding="0" CellSpacing="0"
                                                    RepeatColumns="2" RepeatDirection="Horizontal" Width="50%">
                                                </asp:CheckBoxList>
                                            </td>
                                            <td width="1%">
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CustomValidator ID="cstRoleValidatetoAdd" runat="server" ClientValidationFunction="CheckBoxListRoles"
                                                    ErrorMessage="At least one user role should be selected." Display="None" ValidationGroup="valAdd"
                                                    CssClass="LblErrorMsg"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="vertical-align: top" class="ClsBorderlight">
                                                <span class="ClsLabel">Contents :</span>
                                            </td>
                                            <td align="left" colspan="3">
                                                <CKEditor:CKEditorControl ID="FCKNewMenu" Toolbar="Bold|Italic|Underline|Strike|-|Subscript|Superscript NumberedList|BulletedList|NumberedList|-|Indent  /|JustifyLeft|JustifyCenter|JustifyRight|JustifyFull|Indent|Outdent| / |Styles|Format|Font|FontSize|TextColor|BGColor /MindTouchDeki_InsertLink|MindTouchDeki_InsertImage|MindTouchDeki_AttachImage|MindTouchDeki_InsertVideo|MindTouchDeki_InsertTemplate|Table /Cut|Copy|Paste|PasteText|PasteWord|SpellCheck|- /|Undo|Redo|-|Find|Replace|-|SelectAll|RemoveFormat| /|Maximize|ShowBlocks|"
                                                    BasePath="../ckeditor/" Width="100%" runat="server" Height="350px" ReadOnly="false"
                                                    ToolbarCanCollapse="True"></CKEditor:CKEditorControl>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:Button ID="imgbtnSave" CausesValidation="true" runat="server" OnClick="imgbtnSave_Click"
                                                    disable-page="true" CssClass="ClsBtn" ValidationGroup="valAdd" Text="Save" />
                                                <asp:Button ID="imgbtnCancel" runat="server" CausesValidation="False" CssClass="ClsBtn"
                                                    Text="Cancel" OnClick="imgbtnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidMenuId" runat="server" Value="0" />
                <asp:HiddenField ID="hidIsExternal" runat="server" Value="0" />
                <asp:HiddenField ID="hidIsChildMenu" runat="server" Value="0" />
                <asp:HiddenField ID="hidParentMenuAddId" runat="server" Value="0" />
                <!-- Main Data Ends-->
            </td>
            <td class="NewInnerTopTR">
            </td>
        </tr>
    </table>
    <script lang="javascript" type="text/javascript">
        _clienttxtDateUpdate = "<%=this.txtDateUpdate.ClientID %>"
        _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"
        _clientchkAll = "<%=this.chkAll.ClientID %>";
        _clientchkListRoles = "<%=this.chkListRoles.ClientID %>";
        _clientchkAddAll = "<%=this.chkAddAll.ClientID %>";
        _clientchkAddListRoles = "<%=this.chkAddListRoles.ClientID %>";
        _clienthidMenuId = "<%=this.hidMenuId.ClientID %>";
        _clientoptTopAdd = "<%=this.optTopAdd.ClientID %>"
        _clientoptTopUpdate = "<%=this.optTopUpdate.ClientID %>"

        function ShowAllCountAdd() {
            if ($get(_clientoptTopAdd).checked) {
                $('#tdHideControls').show();
            }
            else {
                $('#tdHideControls').hide();
            }
        }
        function ShowAllCountUpdate() {
            if ($get(_clientoptTopUpdate).checked) {
                $("#trshowtopAll").show();
            }
            else {
                $("#trshowtopAll").hide();
            }
        }
       
    </script>
    <script src="../Scripts/Admin/ConfigureMenu.js" type="text/javascript"></script>
</asp:Content>
