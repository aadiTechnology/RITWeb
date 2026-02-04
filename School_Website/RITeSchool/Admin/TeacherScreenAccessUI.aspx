<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="TeacherScreenAccessUI.aspx.cs" Inherits="TeacherScreenAccessUI" ValidateRequest="false" ViewStateMode="Disabled" %>

<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        td[valign="top"] {
            vertical-align: top;
        }
        
        label[for] {
            font-family: open sans;
        }
        
        table[id*=lstvw] tr td > table tr:first-child > td {
            border: 1px solid white;
            border-spacing: 0 !important;
        }
    </style>
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td id="MainDataTable" align="center">
                <!-- Data Insert Here -->
                <table border="0" cellpadding="0" cellspacing="2" width="100%" style="height: 100%;">
                    <tr id="trerror" visible="false" runat="server">
                        <td align="left" colspan="1">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" ShowMessageBox="False"
                                    ShowSummary="true" />
                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        <asp:UpdatePanel ID="upnlUpdate" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblUpdateSucess" runat="server" ViewStateMode="Enabled" ForeColor="Blue" Height="20px" Width="100%"
                                CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 100%;">
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left">
                                        <table id="tblTeacherCombo" runat="server" viewstatemode="Enabled" >
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <asp:Label ID="lblTeacher" runat="server" ViewStateMode="Enabled" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                        Font-Bold="True" Text="<%$ Resources:LocalizedResources, SearchTeacher %>"></asp:Label>
                                                        <span class="ClsLblLgnd colonPadding" > : </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo"
                                                        OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                   <td align="left">
                                                       <asp:TextBox ID="txtName" TabIndex="1" runat="server" MaxLength="50" CssClass="MidTxtBox" autocomplete="off" ViewStateMode="Enabled"></asp:TextBox>&nbsp;
                                                   </td>
                                                <td align="left">
                                                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>" TabIndex="2" CssClass="ClsBtnMid remove-margin-top"
                                                        OnClick="btnSearch_Click" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAcademicApplicable" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, AcademicYearChangeAppl %>" />
															<asp:CheckBox ID="chkFinancialYearApplicable" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, FinancialYearChangeAppl %>"  Visible="false" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="center" valign="top">
                                    <td align="center">
                                        <asp:UpdatePanel ID="updtpnlscreenList" runat="server" ChildrenAsTriggers="False"
                                            UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div style="height: 100%; overflow: auto">
                                                    <table width="100%">
                                                        <tr id="TrLable" runat="server" viewstatemode="Enabled">
                                                            <td align="center"  valign="top" class="ClsHilightBG" style="width: 50%">
                                                                <asp:Label ID="lblSchoolConfig" runat="server" ViewStateMode="Enabled" Font-Bold="True" CssClass="ClsHilightText"
                                                                    Text="<%$ Resources:LocalizedResources, SchoolMenus %>"></asp:Label>
                                                            </td>
                                                            <td align="center" class="ClsHilightBG" style="width: 50%">
                                                                <asp:Label ID="lblOtherMenus" runat="server" ViewStateMode="Enabled" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Reports %>" CssClass="ClsHilightText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <div>
                                                                    <asp:ListView ID="lstvwScreenAccess" GroupItemCount="3" GroupPlaceholderID="ContactRowContainer"
                                                                        ItemPlaceholderID="ContactItemContainer" runat="server" ViewStateMode="Enabled">
                                                                        <LayoutTemplate>
                                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" class="GridBorder"
                                                                                cellpadding="0" cellspacing="1">
                                                                                <tr id="Tr1" runat="server" class="ClsGridHeader">
                                                                                    <th id="Th1" runat="server">
                                                                                    </th>
                                                                                    <th id="Th2" runat="server">
                                                                                        <asp:Label ID="lblScreenNameText" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, ScreenName %>"></asp:Label>
                                                                                    </th>
                                                                                    <th id="Th3" runat="server">
                                                                                         <asp:Label ID="lblCanEditText" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, CanEdit %>"></asp:Label>
                                                                                    </th>
                                                                                </tr>
                                                                                <tr runat="server" id="ContactRowContainer" />
                                                                            </table>
                                                                        </LayoutTemplate>
                                                                        <GroupTemplate>
                                                                            <tr runat="server" id="ContactRow">
                                                                                <td runat="server" id="ContactItemContainer" />
                                                                            </tr>
                                                                        </GroupTemplate>
                                                                        <ItemTemplate>
                                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                <td align="left" class="ClspaddingL" width="10%">
                                                                                    <asp:Label ID="lblConfigChk" runat="server" ViewStateMode="Enabled"><%#Eval("ConfigChk")%></asp:Label>
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblConfigure_Name" runat="server" ViewStateMode="Enabled"><%#Eval("Configure_Name")%></asp:Label>
                                                                                </td>
                                                                                <td class="ClspaddingL">
                                                                                    <asp:Label ID="lblCanEdit" runat="server" ViewStateMode="Enabled"><%#Eval("CanEdit")%></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                                <td align="left" class="ClspaddingL" width="10%">
                                                                                    <asp:Label ID="lblConfigChk" runat="server" ViewStateMode="Enabled"><%#Eval("ConfigChk")%></asp:Label>
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblConfigure_Name" runat="server" ViewStateMode="Enabled"><%#Eval("Configure_Name")%></asp:Label>
                                                                                </td>
                                                                                <td class="ClspaddingL">
                                                                                    <asp:Label ID="lblCanEdit" runat="server" ViewStateMode="Enabled"><%#Eval("CanEdit")%></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                    </asp:ListView>
                                                                </div>
                                                            </td>
                                                            <td align="left" valign="top" style="width: 50%;">
                                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1">                                                                   
                                                                    <tr>
                                                                        <td>
                                                                            <div id="Div1" style="width: 100%; overflow: auto;">
                                                                                <asp:ListView ID="lstvwReportFolders" runat="server" ViewStateMode="Enabled" DataKeyNames="Report_Folder_Id , HasAccess"
                                                                                    OnItemDataBound="lstvwReportFolders_ItemDataBound">
                                                                                    <LayoutTemplate>
                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="center" width="30px">
                                                                                                </th>
                                                                                                <th align="left" style="padding-left: 10px;">
                                                                                                   <asp:Label ID="lblReportNameText" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, ReportName %>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="right" style="padding-left: 10px;">
                                                                                                    <asp:Label ID="lblHasFullAccessText" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, HasFullAccess %>"></asp:Label>
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="trItem" runat="server" class="ClsGridRow">
                                                                                            <td align="center">
                                                                                                <asp:CheckBox ID="ChkSelect" runat="server" ViewStateMode="Enabled" />
                                                                                            </td>
                                                                                            <td class="ClspaddingL" colspan="2">
                                                                                                <asp:Label ID="lblReportFolder" runat="server" ViewStateMode="Enabled" Text='<%#Eval("Report_Folder_Name") %>'
                                                                                                    Font-Bold="true"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trReports" runat="server" viewstatemode="Enabled" class="ClsGridRow">
                                                                                            <td>
                                                                                            </td>
                                                                                            <td id="tdReports" runat="server" viewstatemode="Enabled" colspan="2">
                                                                                                <asp:ListView ID="lstvwReports" runat="server" ViewStateMode="Enabled" DataKeyNames="Report_Id,HasAccess,HasFullAccess,IsViewAvailable"
                                                                                                    OnItemDataBound="lstvwReports_ItemDataBound">
                                                                                                    <LayoutTemplate>
                                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                            cellspacing="1">
                                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </LayoutTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                                                            <td class="ClspaddingL">
                                                                                                                <asp:CheckBox ID="chkReportName" runat="server" ViewStateMode="Enabled" Text='<%#Eval("Report_Display_Name") %>'
                                                                                                                    />
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 100px;">
                                                                                                                <asp:CheckBox ID="chkHasFullAccess" runat="server" ViewStateMode="Enabled" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </ItemTemplate>
                                                                                                    <AlternatingItemTemplate>
                                                                                                        <tr id="trAltItem" runat="server" class="ClsGridRow">
                                                                                                            <td class="ClspaddingL">
                                                                                                                <asp:CheckBox ID="chkReportName" runat="server" ViewStateMode="Enabled" Text='<%#Eval("Report_Display_Name") %>'
                                                                                                                   />
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 100px;">
                                                                                                                <asp:CheckBox ID="chkHasFullAccess" runat="server" ViewStateMode="Enabled" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </AlternatingItemTemplate>
                                                                                                </asp:ListView>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="trAltItem" runat="server" class="ClsGridRow">
                                                                                            <td align="center">
                                                                                                <asp:CheckBox ID="ChkSelect" runat="server" ViewStateMode="Enabled" />
                                                                                            </td>
                                                                                            <td class="ClspaddingL" colspan="2">
                                                                                                <asp:Label ID="lblReportFolder" runat="server" ViewStateMode="Enabled" Text='<%#Eval("Report_Folder_Name") %>'
                                                                                                    Font-Bold="true"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trReports" runat="server" viewstatemode="Enabled" class="ClsGridRow">
                                                                                            <td>
                                                                                            </td>
                                                                                            <td id="tdReports" runat="server" viewstatemode="Enabled" colspan="2">
                                                                                                <asp:ListView ID="lstvwReports" runat="server" ViewStateMode="Enabled" DataKeyNames="Report_Id,HasAccess,HasFullAccess,IsViewAvailable"
                                                                                                    OnItemDataBound="lstvwReports_ItemDataBound">
                                                                                                    <LayoutTemplate>
                                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                            cellspacing="1">
                                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </LayoutTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                                                            <td class="ClspaddingL">
                                                                                                                <asp:CheckBox ID="chkReportName" runat="server" ViewStateMode="Enabled" Text='<%#Eval("Report_Display_Name") %>'
                                                                                                                    />
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 100px;">
                                                                                                                <asp:CheckBox ID="chkHasFullAccess" runat="server" ViewStateMode="Enabled" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </ItemTemplate>
                                                                                                    <AlternatingItemTemplate>
                                                                                                        <tr id="trAltItem" runat="server" class="ClsGridRow">
                                                                                                            <td class="ClspaddingL">
                                                                                                                <asp:CheckBox ID="chkReportName" runat="server" ViewStateMode="Enabled" Text='<%#Eval("Report_Display_Name") %>'/>
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 100px;">
                                                                                                                <asp:CheckBox ID="chkHasFullAccess" runat="server" ViewStateMode="Enabled" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </AlternatingItemTemplate>
                                                                                                </asp:ListView>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                </asp:ListView>
                                                                            </div>                                                                           
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                <td align="center">
                                    <div runat="server" viewstatemode="Enabled" id="divErr">
                                    </div>
                                </td>
                            </tr>
                                <tr>
                                    <td align="center" colspan="1" style="padding-top: 5px">
                                        <asp:Button CssClass="ClsBtn" ID="btnBack" CausesValidation="false" runat="server" ViewStateMode="Enabled" 
                                            Text="<%$ Resources:LocalizedResources, Back %>" BorderWidth="1px" OnClick="btnBack_Click"></asp:Button>
                                        <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Save %>" BorderWidth="1px"
                                            OnClick="btnSave_Click" UseSubmitBehavior="false"></asp:Button>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidScreenAccess" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidCountSchoolConfig" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidCultureInfo" runat="server" ViewStateMode="Enabled" />
                 <asp:HiddenField ID="hidUserRoleId" runat="server" />
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clientcstbtnSave = "<%=this.btnSave.ClientID%>";
        _clientcstBtnBack = "<%=this.btnBack.ClientID%>";
        _clienthidCountSchoolConfigId = "<%=this.hidCountSchoolConfig.ClientID%>";
        _clientlstvwReportFolders = "<%=this.lstvwReportFolders.ClientID %>";

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginReqHandler);
        prm.add_endRequest(EndReqHandler);

        function BeginReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientcstbtnSave)
                DisableButtons(true);
        }

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientcstbtnSave)
                DisableButtons(false);
        }

        // Disable save and back button if page is valid.
        function DisableButtons(action) {
            var isPageValid = true;
            if (typeof (Page_ClientValidate) == 'function' && action)
                isPageValid = Page_ClientValidate();
            if (isPageValid) {
                if (document.getElementById(_clientcstbtnSave) != null)
                    document.getElementById(_clientcstbtnSave).disabled = action;
                if (document.getElementById(_clientcstBtnBack) != null)
                    document.getElementById(_clientcstBtnBack).disabled = action;
            }
        }

        function CountCheckedNode(iNodeLength, stvwName) {
            var iIsCheked = 0;
            var tvNodeId; ;
            for (iChildNode = 0; iChildNode < iNodeLength; iChildNode++) {
                if (iChildNode < 10)
                    tvNodeId = stvwName + iChildNode + "CheckBox";
                else if (iChildNode >= 10)
                    tvNodeId = stvwName + iChildNode + "CheckBox";

                if (document.getElementById(tvNodeId) != null) {
                    if (document.getElementById(tvNodeId).checked == true)
                        iIsCheked++;
                }
            }
            return iIsCheked;
        }

        function clickButton(e) {

            var evt = e ? e : window.event;
            if (evt.keyCode == 13)
                return false;
        }

        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                //check if nxt sibling is not null & is an element node
                if (nxtSibling && nxtSibling.nodeType == 1) {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }

        //This function is used to check child node if parent node is checked.
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                if (!childChkBoxes[i].disabled)
                    childChkBoxes[i].checked = check;
            }
        }

        //This function is used to check parent node if all child nodes are checked.
        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;
            if (parentNodeTable) {
                var checkUncheckSwitch;
                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }
                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }

        }
        //This function is used to check whether all child nodes are checked or not.
        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) {
                    //check if the child node is an element node
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            //return true;
            return false;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

        // This method is used to check/uncheck all the child checkboxes on change of parent checkbox state.
        function CheckAllChild(parent, evt, chkName) {
            var check = false;
            if (parent.checked)
                check = true;
            var childChkBoxes = document.getElementsByName(chkName);
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
                if (childChkBoxes[i].id.match("EchkAccess") != null) {
                    childChkBoxes[i].disabled = !check;
                    childChkBoxes[i].checked = false;
                }
            }
        }

        function checkUnCheckEdit(parent, chkName) {
            document.getElementById(chkName).checked = parent.checked;
            document.getElementById(chkName).disabled = !parent.checked;
            CheckUnCheckParentCheckBox(chkName);
        }

        // This method is used to check/uncheck parent checkbox by checking child checkboxes.
        function CheckUnCheckParentCheckBox(chkName) {
            var chkMainClientId = document.getElementById(chkName).name;
            var iCount = 0;
            var childChkBoxes = document.getElementsByName(chkMainClientId);
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                if (childChkBoxes[i].checked == true && childChkBoxes[i].disabled == false) {
                    iCount++;
                }
            }
            chkMainClientId = chkMainClientId.replace('chk_', 'chkMain_');
            if (iCount > 0)
                document.getElementById(chkMainClientId).checked = true;
            else
                document.getElementById(chkMainClientId).checked = false;
        }

        // This method is used to read checked screens.
        function CalculateAccess() {
            var sXml = "<ScreenAccess>"
            var sIds = ''
            var sAbsentIds = ''
            var checks = document.forms[0].elements;
            var boxLength = checks.length;
            var allChecked = false;
            var totalChecked = 0;
            for (j = 0; j < boxLength; j++) {
                if (checks[j].type == 'checkbox' && checks[j].id.match("chkAccess_") != null && checks[j].id.charAt(0) != "E") {
                	var EditAccessChk = document.getElementById("E" + checks[j].id);
                	sXml = sXml + '<Screen id="' + checks[j].id.split('_')[1] + '" IsDeleted="' + (checks[j].checked ? 'N' : 'Y') + '" CanEdit="' + (EditAccessChk.checked ? 'Y' : 'N') + '"></Screen>';
                }
            }
            sXml = sXml + "</ScreenAccess>";
            $get("<%=hidScreenAccess.ClientID %>").value = sXml;
            return false;

        }

        function SelectUnSelectChilds(row, chkSelect) {
            var iRowIndex = 0;
            var isChecked = chkSelect.checked;
            var reportName = document.getElementById(_clientlstvwReportFolders + "_ctrl" + row + "_lstvwReports_ctrl" + iRowIndex + "_chkReportName")
            while (reportName != null) {
                reportName.checked = isChecked;
                iRowIndex++;
                reportName = document.getElementById(_clientlstvwReportFolders + "_ctrl" + row + "_lstvwReports_ctrl" + iRowIndex + "_chkReportName")
            }
        }
    </script>
       
        <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            var UserRole = '<%=hidUserRoleId.ClientID%>';
            $get(UserRole).value = 2;

            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, UserRole, 0);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

</script>

</asp:Content>
