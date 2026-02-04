<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="MailingGroupPopup.aspx.cs" Inherits="MailingGroupPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
<style type="text/css">
    [id$='chkListRoles']
    {
        width: 270px;
    }

    [id$='chkListRoles'] td {
        padding: 0 5px !important;
    }
</style>
    <table style="width: 100%;" cellpadding="0" cellspacing="1">
        <tr>
            <td align="left" colspan="3">
                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="height: 20px">
                            <span style="font-weight: bold; padding-right: 5px;">Contact Group(s) </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trMandatory" runat="server">
            <td align="right" colspan="6">
                <span class="ClsMdtStar">* Mandatory Fields</span>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:UpdatePanel ID="upnlval" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSumError" runat="server" HeaderText="Please correct following errors."
                            ShowMessageBox="false" ShowSummary="true" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct following errors."
                    ShowMessageBox="false" ShowSummary="true" ValidationGroup="OK" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="hidIsCc" runat="server" Value="0" />
                </ContentTemplate>
             </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trError">
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                            ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                        <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="False"
                            Width="100%" Style="text-align: left; padding-left: 20px" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr runat="server" id="trGroups">
            <td id="Td1" align="center" valign="top" runat="server">
                <table>
                    <tr>
                        <td align="left">
                            <table id="tblLegend" runat="server">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblSelectDate" class="ClsLblLgnd" EnableViewState="false" BorderWidth="0px"
                                            Font-Bold="True" runat="server" Text="<%$ Resources:LocalizedResources, Legend %>" />
                                        <span id="Span1" class="ClsLblLgnd colonPadding">:</span>
                                    </td>
                                    <td align="left" style="padding-right: 3px">
                                        <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                            BackColor="Gainsboro" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" Font-Bold="True" PaddingLeft="1px"
                                            Text="<%$ Resources:LocalizedResources, DeactivatedUser %>" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <span class="ClsLblLgnd" style="font-weight: bold">Select Contact Group(s) :</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top" runat="server" id="tdGroup">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ListView ID="lstvwGroup" runat="server" ItemPlaceholderID="trItemPlaceholder"
                                        ClientIDMode="Inherit" OnItemCommand="lstvwGroup_ItemCommand" OnItemDataBound="lstvwGroup_ItemDataBound"
                                        DataKeyNames="GroupId,IsDefault,IsAllDeactivated" OnDataBound="lstvwGroup_DataBound">
                                        <LayoutTemplate>
                                            <table id="tblGroup" style="width: 620px; color: #333333" class="GridBorder" cellpadding="0"
                                                cellspacing="1">
                                                <tr id="trGroupHeader" runat="server" class="ClsGridHeader">
                                                    <th align="center" class="Clspadding" style="width: 8%;">
                                                        <asp:CheckBox runat="server" ID="chkSelect" Onclick="CheckAllGroup();"></asp:CheckBox>
                                                    </th>
                                                    <th class="ClspaddingL" style="width: 46%;">
                                                        Group Name
                                                    </th>
                                                    <th align="center" style="width: 10%;" class="Clspadding" id="thEdit" runat="server">
                                                        Edit
                                                    </th>
                                                    <th align="center" style="width: 12%;" class="Clspadding" id="thDelete" runat="server">
                                                        Delete
                                                    </th>
                                                </tr>
                                                <tr id="trItemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <EmptyDataTemplate>
                                            <table align="center" width="600px">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                <td align="center" class="Clspadding" style="width: 8%;">
                                                    <asp:CheckBox runat="server" ID="chkSelect"></asp:CheckBox>
                                                </td>
                                                <td class="ClspaddingL" style="width: 46%;">
                                                    <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name")%>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hiddenGroupId" Value='<%#Eval("GroupId")%>' />
                                                    <asp:HiddenField runat="server" ID="hidUsers" Value='<%#Eval("Users")%>' />
                                                </td>
                                                <td align="center" style="width: 10%;" class="Clspadding" id="tdEdit" runat="server">
                                                    <asp:ImageButton runat="server" ID="imgEdit" Text="Edit" CommandName="UpdateCommand"
                                                        CommandArgument='<%#Eval("GroupId")%>' CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                        ToolTip="Edit"></asp:ImageButton>
                                                </td>
                                                <td align="center" style="width: 12%;" class="Clspadding" id="tdDelete" runat="server">
                                                    <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="RemoveCommand"
                                                        CommandArgument='<%#Eval("GroupId")%>' CausesValidation="false" ToolTip="Delete"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDelete()) return false;">
                                                    </asp:ImageButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnOk" runat="server" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                                    <asp:PostBackTrigger ControlID="lstGroupUsers" runat="server" />
                                    <asp:PostBackTrigger ControlID="btnCancel" runat="server" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" valign="bottom">
                <asp:UpdatePanel runat="server" ID="upnlbutton" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button Text="Ok" ID="btnOk" runat="server" CssClass="ClsBtnSml" UseSubmitBehavior="false"
                            ValidationGroup="OK" OnClick="btnOk_Click" />
                        <asp:Button Text="Close" ID="btnClose" runat="server" CssClass="ClsBtnSml" UseSubmitBehavior="false"
                            CausesValidation="False" OnClick="btnClose_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:PostBackTrigger ControlID="btnOk" runat="server" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr runat="server" id="trCreateGroup">
            <td align="center">
                <asp:UpdatePanel runat="server" ID="upnlAddGroup" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="ClsBorderlight" style="width: 80%" align="center">
                            <tr>
                                <td align="center" valign="top" style="text-align: center; background-color: #eaeaea"
                                    class="ClsGridHeader">
                                    <asp:Label runat="server" ID="lblTitle" Font-Bold="true" Text="Add/Update Group"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <table style="width: 90%">
                                        <tr>
                                            <td class="ClsBorderlight" width="20%">
                                                <span class="ClsLabel" style="white-space: nowrap">Group Name :</span>
                                            </td>
                                            <td style="white-space: nowrap">
                                                <asp:TextBox ID="txtGroupName" runat="server" CssClass="ClsTxtLarge" MaxLength="50"
                                                    EnableViewState="True"></asp:TextBox>
                                                <span style="color: red" id="spnMandatory" runat="server">*</span>
                                                <asp:CustomValidator ID="cstvalRequired" runat="server" ClientValidationFunction="CheckEmpty"
                                                    CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator runat="server" ID="cstValRole" ClientValidationFunction="ValidateRoles"
                                                    CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstValcheck" runat="server" ClientValidationFunction="CheckValidCheckBoxes"
                                                    CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstCheckDuplicates" runat="server" ClientValidationFunction="CheckDuplicates"
                                                    CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr id="trUserRoles" runat="server">
                                            <td width="20%" class="ClsBorderlight">
                                                <span class="ClsLabel" style="white-space: nowrap">Applicable To :</span>
                                            </td>
                                            <td>
                                                <asp:CheckBoxList ID="chkListRoles" runat="server" CellPadding="0" CellSpacing="0"
                                                    DataTextField="User_Role_Name" DataValueField="User_Role_Id" CssClass="ClsBorderLight"
                                                    RepeatColumns="3" RepeatDirection="Horizontal">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button CssClass="ClsBtn" ID="btnAdd" runat="server" Text="Add" BorderWidth="1px"
                                        CommandName="Add" OnClick="btnAdd_Click"></asp:Button>
                                    <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                        OnClientClick="ClearControls()" Text="Cancel" BorderWidth="1px" OnClick="btnCancel_Click">
                                    </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:PostBackTrigger ControlID="btnOk" runat="server" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                        <asp:PostBackTrigger ControlID="btnCancel" runat="server" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel runat="server" ID="UpdatePanel14" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="GridViewScrollContainer" class="GridBorder" style="max-height: 100pt; overflow: auto;
                            width: 620px" runat="server" visible="false">
                            <asp:ListView ID="lstGroupUsers" runat="server" ItemPlaceholderID="trItemPlaceholder"  OnItemDataBound="lstGroupUsers_ItemDataBound"
                                DataKeyNames="UserId,IsDeactivated" OnItemCommand="lstGroupUsers_ItemCommand">
                                <LayoutTemplate>
                                    <table id="tblUsers" style="width: 600px; color: #333333" class="GridBorder" cellpadding="0"
                                        cellspacing="1">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th class="ClspaddingL">
                                                User Name
                                            </th>
                                            <th align="center">
                                                Delete
                                            </th>
                                        </tr>
                                        <tr id="trItemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <EmptyDataTemplate>
                                    <table style="width: 80%">
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                No record found.
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>' id="trItem" runat="server">
                                        <td align="center" class="ClspaddingL" style="width: 85%;">
                                            <asp:Label runat="server" ID="lblName" Text='<%#Eval("UserName")%>'></asp:Label>
                                        </td>
                                        <td align="center" style="width: 15%;" class="Clspadding">
                                            <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="RemoveCommand"
                                                CommandArgument='<%#Eval("UserId")%>' CausesValidation="false" ToolTip="Delete"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDeleteUser()) return false;">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:PostBackTrigger ControlID="btnOk" runat="server" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlGrpusers" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidGroupId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidSelectedGroupId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidCcSelectedGroupId" runat="server" Value="0" />
                        <asp:HiddenField runat="server" ID="hidIsGroupDeleted" Value="0" />
                        <asp:HiddenField runat="server" ID="hidUserRoles" Value="" />
                        <asp:HiddenField runat="server" ID="hidHasFullAccess" Value="" />
                        <asp:HiddenField runat="server" ID="hidEditedUserCount" Value="0" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:PostBackTrigger ControlID="btnOk" runat="server" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                        <asp:PostBackTrigger ControlID="lstGroupUsers" runat="server" />
                        <asp:PostBackTrigger ControlID="btnCancel" runat="server" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr runat="server" id="trUserRole">
            <td align="center">
                <asp:UpdatePanel ID="upnlRole" runat="server">
                    <ContentTemplate>
                        <table width="30%" align="center">
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <span class="ClsLabel">User Role :</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbRoles" runat="server" CssClass="SmlCombo" OnSelectedIndexChanged="cmbRoles_SelectedIndexChanged"
                                        AutoPostBack="true" onclick="ShowClasses()">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr id="trClass" runat="server" visible="false">
										<td align="left" class="ClsBorderlight">									  
                                      <span class="ClsLabel">Class :</span>
										</td>
										<td align="left">
											<asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Always">
												<ContentTemplate>
													<asp:DropDownList ID="cmbClass" runat="server" ViewStateMode="Enabled" 
                                                        CssClass="SmlCombo" AutoPostBack="true" 
                                                        onselectedindexchanged="cmbClass_SelectedIndexChanged" >
													</asp:DropDownList>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
                            <tr align="center">
                                <td align="left" class="ClsBorderlight">
                                  <asp:Label ID="Label4" runat="server" class="ClsLabel" Text="UserName"></asp:Label>
                                  <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                 <asp:TextBox ID="txtName"  runat="server" MaxLength="50" CssClass="MidTxtBox"  autocomplete="off"></asp:TextBox>&nbsp;
                                  </td>
                               <td align="left">
                               <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>" CssClass="ClsBtnMid remove-margin-top"
                                 CausesValidation="false" OnClick="btnSearch_Click"/>
                              </td>
                            </tr>

                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:PostBackTrigger ControlID="btnOk" runat="server" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                        <asp:PostBackTrigger ControlID="btnCancel" runat="server" />
                        <asp:PostBackTrigger ControlID="cmbRoles" />
                        <asp:PostBackTrigger ControlID="cmbClass" />
                        <asp:PostBackTrigger ControlID="btnSearch" runat="server" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr runat="server" id="trContacts">
            <td align="center" valign="top">
                <table align="center" valign="top">
                    <tr>
                        <td align="center">
                            <span class="ClsLblLgnd" style="font-weight: bold;">Select Users To Add In Selected
                                Group :</span>
                        </td>
                    </tr>
                    <tr id="trItemCount" runat="server">
                        <td align="center" colspan="3">
                            <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwContacts" Visible="true">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                            <br />
                                        </PagerTemplate>
                                    </asp:TemplatePagerField>
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ListView ID="lstvwContacts" runat="server" ItemPlaceholderID="trItemPlaceholder"
                                        OnItemCommand="lstvwContacts_ItemCommand" OnItemDataBound="lstvwContacts_ItemDataBound"
                                        OnDataBound="lstvwContacts_DataBound" OnSorting="lstvwContacts_Sorting"
                                        DataSourceID="lstvwDSobj" DataKeyNames="UserId,IsDeactivated">
                                        <LayoutTemplate>
                                            <table id="tblTachers" style="width: 600px; color: #333333" class="GridBorder" cellpadding="0"
                                                cellspacing="1">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="center" class="Clspadding" style="width: 8%;">
                                                        <asp:CheckBox runat="server" ID="chkSelectAll" Onclick="CheckAllTeacher();"></asp:CheckBox>                                                        
                                                    </th>
                                                    <th class="ClspaddingL">
                                                        <asp:LinkButton ID="lnkItemCode" runat="server" CommandName="Sort" CommandArgument="UserName"
                                                            CausesValidation="False" ForeColor="Black"> User Name</asp:LinkButton>
                                                    </th>
                                                </tr>
                                                <tr id="trItemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="3">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="10" PagedControlID="lstvwContacts">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr align="left">
                                                                                <td>
                                                                                    <span class="LblNrmlB">Select a page:</span>
                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="right" class="LblNormal">
                                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </PagerTemplate>
                                                                </asp:TemplatePagerField>
                                                            </Fields>
                                                        </asp:DataPager>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <EmptyDataTemplate>
                                            <table style="width: 80%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'
                                                id="trItem" runat="server" >
                                                <td align="center" class="Clspadding" style="width: 8%;">
                                                    <asp:CheckBox runat="server" ID="chkSelect" Checked='<%# Convert.ToBoolean(Eval("IsInGroup"))%>'
                                                        Onclick="CheckAll();"></asp:CheckBox>
                                                </td>
                                                <td class="ClspaddingL">
                                                    <asp:Label runat="server" ID="lblName" Text='<%#Eval("UserName")%>'></asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" Value="UserName" />
                                    <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnOk" runat="server" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                                    <asp:PostBackTrigger ControlID="btnCancel" runat="server" />
                                     <asp:PostBackTrigger ControlID="btnSearch" runat="server" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr runat="server" id="trTotalRec" align="center" visible="false">
                        <td>
                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                            <span class="LblNormal">To</span>
                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                            <span class="LblNormal">Out Of </span>
                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                            <span class="LblNormal">Records </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ObjectDataSource TypeName="BusinessLogic.SchoolUserBL" EnablePaging="true" SortParameterName="sortExpression"
                                ID="lstvwDSobj" runat="server" SelectMethod="GetUsersforMailingGroups" SelectCountMethod="GetUserCountForMailingGroups"
                                EnableCaching="false">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                        Type="string" />
                                    <asp:ControlParameter ControlID="cmbRoles" PropertyName="SelectedValue" Name="aiRoleId"
                                        Type="int32" DefaultValue="0" />
                                      <asp:ControlParameter Name="asFilter" ControlID="txtName" Type="String"  PropertyName="Text" />
                                    <asp:ControlParameter ControlID="hidStdDivId" PropertyName="Value" Name="aiStandardDivId" Type="Int32" DefaultValue="0" />
                                    <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection"
                                        PropertyName="Value" />
                                    <asp:Parameter Name="sortExpression" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript" src="../Scripts/jquery-1.7.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-blink.js"></script>
    <script src="../../js/jquery.qtip-1.0.0-rc3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/Validate2.js"></script>
    <script type="text/javascript" src="../Scripts/Validations.js"></script>
    <style type="text/css">
        .class1
        {
            border: 1;
        }
    </style>
    <style type="text/css">
        .class2
        {
            border: 1;
        }
    </style>
    <script type="text/javascript" language="javascript">
        _clientlstvwContacts = "<%=this.lstvwContacts.ClientID %>";
        _clientlstvwGroup = "<%=this.lstvwGroup.ClientID %>";
        _clientcstValcheck = "<%=this.cstValcheck.ClientID %>";
        _clienthidGroupId = "<%=this.hidGroupId.ClientID %>";
        _clienttxtGroupName = "<%=this.txtGroupName.ClientID %>";
        _clientlblUpdateMessage = "<%=this.lblUpdateMessage.ClientID %>";
        _clientbtnAdd = "<%=this.btnAdd.ClientID %>";
        _clienthidSelectedGroupId = "<%=this.hidSelectedGroupId.ClientID %>";
        _clienthidCcSelectedGroupId = "<%=this.hidCcSelectedGroupId.ClientID %>";
        _clienthidIsCc = "<%=this.hidIsCc.ClientID %>";
        _clientchkListRoles = "<%=this.chkListRoles.ClientID %>";
        _clienthidUserRoles = "<%=this.hidUserRoles.ClientID %>";
        _clienthidEditedUserCount = "<%=this.hidEditedUserCount.ClientID %>";
        _clientGridViewScrollContainer = "<%=this.GridViewScrollContainer.ClientID %>";
        _clientlblError = "<%=this.lblError.ClientID %>";
        _clienttrClass = "<%=this.trClass.ClientID%>"
        _clientcmbClass = "<%=this.cmbClass.ClientID%>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);

        function EndReqHandler(sender, args) {
            getGroupIds();
            getGroupIdsCc();
        }
        window.onload = Check();

        function Check() {
            if ($get(_clienthidIsCc).value == '0')
                getGroupIds();
            else
                getGroupIdsCc();
        }

        function getGroupIds() {
            $get(_clienthidSelectedGroupId).value = window.opener.getGroupIds();
            var iRowCount = 0;
            var select = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_chkSelect");
            while (select != null) {
                var GroupId = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_hiddenGroupId");
                var arrIds = new Array();
                arrIds = $get(_clienthidSelectedGroupId).value.split(',');
                if (GroupId != null) {
                    var len = arrIds.length;
                    var i = 0;
                    for (; i < len; i++) {
                        if (arrIds[i] == GroupId.value)
                            select.checked = true;
                    }
                }

                iRowCount = iRowCount + 1;
                select = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_chkSelect");
            }
        }

        function getGroupIdsCc() {
            $get(_clienthidCcSelectedGroupId).value = window.opener.getGroupIdsCc();
            var iRowCount = 0;
            var select = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_chkSelect");
            while (select != null) {
                var GroupId = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_hiddenGroupId");
                var arrIds = new Array();
                arrIds = $get(_clienthidCcSelectedGroupId).value.split(',');
                if (GroupId != null) {
                    var len = arrIds.length;
                    var i = 0;
                    for (; i < len; i++) {
                        if (arrIds[i] == GroupId.value)
                            select.checked = true;
                    }
                }

                iRowCount = iRowCount + 1;
                select = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_chkSelect");
            }
        }

        function CheckAllTeacher() {
            var checkAll;
            var _ClientChkAll = _clientlstvwContacts + "_chkSelectAll";
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked

            var iRowCount = 0
            var chk = document.getElementById(_clientlstvwContacts + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (chk.disabled == false)
                    chk.checked = checkAll
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwContacts + "_ctrl" + iRowCount + "_chkSelect");
            }
        }

        function CheckAllGroup() {
            var i;
            var oListView = document.getElementById(_clientlstvwGroup);
            var oHdrChk = document.getElementById(_clientlstvwGroup + "_" + 'chkSelect');
            for (i = 0; i < $get('tblGroup').rows.length - 1; i++) {
                var chk = _clientlstvwGroup + "_ctrl" + i + "_" + 'chkSelect';
                if (document.getElementById(chk).disabled == false)
                    document.getElementById(chk).checked = oHdrChk.checked;
            }
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to remove this Group?')) {
                bResult = false
            }
            return bResult
        }

        function ConfirmDeleteUser() {
            var bResult = true
            if (!window.confirm('Are you sure you want to remove this user from selected Group?')) {
                bResult = false
            }
            return bResult
        }

        function CheckEmpty(oSrc, args) {
            if ($get(_clienttxtGroupName) != null && ($get(_clienttxtGroupName).value).trim() == "") {
                $get(_clientlblUpdateMessage).innerHTML = "";
                if ($get(_clientlblError) != null)
                    $get(_clientlblError).innerHTML = "";
                oSrc.errormessage = "Group Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return true;
        }

        function CheckValidCheckBoxes(oSrc, args) {
            var UserCount = $get(_clienthidEditedUserCount).value;
            var hidGroupId = $get(_clienthidGroupId).value;
            var Mode = "Add";
            var ValidateEdit = false;
            if (hidGroupId != "0" && hidGroupId != "")
                Mode = "Edit"
            if (hidGroupId != "0" && hidGroupId != "" && UserCount != "" && parseInt(UserCount) > 0)
                ValidateEdit = true;

            if ($get('tblTachers') != null && (Mode == "Add" || !ValidateEdit)) {
                if (!CheckAtleastOneCheckBox(_clientlstvwContacts, 'chkSelect', $get('tblTachers').rows.length)) {
                    $get(_clientlblUpdateMessage).innerHTML = "";
                    if ($get(_clientlblError) != null)
                        $get(_clientlblError).innerHTML = "";
                    oSrc.errormessage = "At least one user should be selected for the Group.";
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return true;
            }
        }

        function CheckAll() {
            var isUnchecked = false;
            var rowIndex = 0;
            var chkSelect = document.getElementById(_clientlstvwContacts + "_ctrl" + rowIndex + "_chkSelect");
            while (chkSelect != null) {
                if (!chkSelect.checked) {
                    isUnchecked = true;
                    break;
                }
                rowIndex++;
                chkSelect = document.getElementById(_clientlstvwContacts + "_ctrl" + rowIndex + "_chkSelect");
            }

            var chkAll = document.getElementById(_clientlstvwContacts + "_chkSelectAll");
            if (isUnchecked)
                chkAll.checked = false;
            else
                chkAll.checked = true;
        }

        function ValidateRoles(oSrc, args) {
            var iRowCnt = 0;
            var IsSelected = false;
            var select = $get(_clientchkListRoles + "_" + iRowCnt);
            while (select != null) {
                if (select.checked) {
                    IsSelected = true;
                    break;
                }
                iRowCnt = iRowCnt + 1;
                select = $get(_clientchkListRoles + "_" + iRowCnt);
            }

            if (!IsSelected) {
                $get(_clientlblUpdateMessage).innerHTML = "";
                if ($get(_clientlblError) != null)
                    $get(_clientlblError).innerHTML = "";
                oSrc.errormessage = "At least one applicable role should be selected.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return true;
        }

        function CheckValidOK() {
            var Selected = false;
            var iRowCount = 0;
            var bResult = true
            var select = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_chkSelect");
            while (select != null) {
                if (select.checked) {
                    Selected = true;
                    break;
                }
                iRowCount = iRowCount + 1;
                select = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_chkSelect");
            }

            if (!Selected) {
                bResult = window.confirm("No group is selected. Are you sure you want to continue?");
            }
            return bResult;
        }

        function CheckDuplicates(oSrc, args) {
            var IsDuplicate = false;
            var iRowCount = 0;
            var lblName = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_lblName");
            while (lblName != null) {
                var GroupName = $get(_clienttxtGroupName).value;
                var hiddenGroupId = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_hiddenGroupId");
                var hidGroupId = $get(_clienthidGroupId).value;

                if (hidGroupId != "0" && hidGroupId != "" && hidGroupId != hiddenGroupId.value && GroupName != "" && GroupName.toLowerCase() == lblName.innerHTML.toLowerCase())
                    IsDuplicate = true;
                else if ((hidGroupId == "0" || hidGroupId == "") && GroupName != "" && GroupName.toLowerCase() == lblName.innerHTML.toLowerCase())
                    IsDuplicate = true;

                if (IsDuplicate) {
                    $get(_clientlblUpdateMessage).innerHTML = "";
                    if ($get(_clientlblError) != null)
                        $get(_clientlblError).innerHTML = "";
                    oSrc.errormessage = "Group Name already exists.";
                    args.IsValid = false;
                    return true;
                }

                iRowCount = iRowCount + 1;
                lblName = $get(_clientlstvwGroup + "_ctrl" + iRowCount + "_lblName");
            }
            args.IsValid = true;
            return true;
        }

        function ClearControls() {

            var iRowCount = 0;
            var select = $get(_clientlstvwContacts + "_ctrl" + iRowCount + "_chkSelect");
            while (select != null) {
                select.checked = false;
                iRowCount = iRowCount + 1;
                select = $get(_clientlstvwContacts + "_ctrl" + iRowCount + "_chkSelect");
            }

            iRowCount = 0;
            $get(_clienthidUserRoles).value = "";
            select = $get(_clientchkListRoles + "_" + iRowCount);
            while (select != null) {
                select.checked = false;
                iRowCount = iRowCount + 1;
                select = $get(_clientchkListRoles + "_" + iRowCount);
            }

            CheckAll();
        }
        
    </script>
    <script type="text/javascript">

        function showtooltip() {
            $('.class1').qtip({
                content: {
                    text: false // Use each elements title attribute
                },
                style: {
                    name: 'cream',
                    color: 'black',  //'cream', // Give it some style
                    border: {
                        width: 3,
                        radius: 5
                    },
                    tip: 'topLeft',
                    width: 330
                },

                position: { adjust: { x: -10, y: 0} }
            });
        }
        showtooltip();
        //ShowNote();
        ShowClasses();
        function ShowClasses() {
            var cClass = $get(_clientcmbClass).value;
            var trClasses = document.getElementById(_clienttrClass);

            var isfound = false;
            for (i = 0; i < cClass.length; i++) {
                if (cClass == "Student") {
                                isfound = true;
                                break;
                            }
                        }
                        

//            if (document.getElementById(_clientcmbClass + "_2").selected) {
//                trClasses.style.display = "table-row";
//            }
//            else {
//                trClasses.style.display = "none";
//            }


//            var cClass = $get(_clientcmbClass).value;
//            var vhidHealthComponentIdIsFitnessComponent = $get(_clienthidhidHealthComponentIdIsFitnessComponent);
//            var componentIds = vhidHealthComponentIdIsFitnessComponent.value.split(',');
//            var i;
//            var isfound = false;
//            for (i = 0; i < cClass.length; i++) {
//                if (componentIds[i] != "" && parseInt(cComponentId) == parseInt(componentIds[i])) {
//                    isfound = true;
//                    break;
//                }
//            }
//            if (isfound) {
//                $('#trTest').fadeIn(200);
//                $('#trMeasure').fadeIn(200);
//            }
//            else {
//                $('#trTest').fadeOut(200);
//                $('#trMeasure').fadeOut(200);
//            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
