<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="PesonalAddressBookUI.aspx.cs" Inherits="RITeSchool_Common_PesonalAddressBookUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table style="width: 100%;" cellpadding="0" cellspacing="1">
        <tr>
            <td align="left" colspan="3">
                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 100%;
                    padding-right: 5px;">
                    <tr>
                        <td style="height: 20px">
                            <span style="font-weight:bold">Select User To Send Message </span>
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
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 80%;" cellpadding="0" cellspacing="1" align="center">
                            <tr>
                                <td align="left" valign="bottom" width="50%" class="ClsBorderlight">
                                    <asp:RadioButton ID="optIndividual" runat="server" Text="Individual Details" AutoPostBack="true"
                                        GroupName="check" OnCheckedChanged="optIndividual_CheckedChanged" />
                                </td>
                                <td align="left" valign="bottom" width="50%" class="ClsBorderlight">
                                    <asp:RadioButton ID="optGroup" runat="server" Text="Group Details" AutoPostBack="true"
                                        OnCheckedChanged="optGroup_CheckedChanged" GroupName="check" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="optGroup" EventName="checkedchanged" />
                        <asp:AsyncPostBackTrigger ControlID="optIndividual" EventName="checkedchanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center" valign="bottom">
                <table style="width: 80%;" cellpadding="0" cellspacing="1">
                    <tr>
                        <td align="right" valign="bottom">
                            <asp:Button Text="Ok" ID="imgBtnOKUp" runat="server" CssClass="ClsBtnSml" OnClick="imgBtnOk_Click"
                                UseSubmitBehavior="false" ValidationGroup="SelectOK" />
                            <asp:Button Text="Close" ID="btnCloseUp" runat="server" CssClass="ClsBtnSml" UseSubmitBehavior="false"
                                CausesValidation="False" OnClientClick="window.close(); return false;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:UpdatePanel runat="server" ID="updtpnlAddressList" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ListView ID="lstvwAddressBook" runat="server" DataSourceID="ObjDSPersonalAddBook"
                            ItemPlaceholderID="trItemPlaceholder" DataKeyNames="PersonalAddressBookId" 
                            OnItemCommand="lstvwAddressBook_ItemCommand" 
                            ondatabound="lstvwAddressBook_DataBound">
                            <LayoutTemplate>
                                <table id="tbllstvwAddressBook" style="width: 85%; color: #333333" class="GridBorder"
                                    cellpadding="0" cellspacing="1">
                                    <tr id="trMainHeader" runat="server" class="ClsGridHeader">
                                        <td align="center" class="Clspadding" style="width: 8%;">
                                            <asp:CheckBox runat="server" ID="chkSelect" Onclick="CheckAll();"></asp:CheckBox>
                                        </td>
                                        <td class="ClspaddingL" style="width: 46%;">
                                            <asp:Label runat="server" ID="lblNameHdr" Text="Name"> </asp:Label>
                                        </td>
                                        <td class="ClspaddingL" style="width: 24%;">
                                            <asp:Label runat="server" ID="lblMobileNoHdr" Text="Mobile Number"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%;" class="Clspadding">
                                            <asp:Label runat="server" ID="Label1" Text="Edit"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 12%;" class="Clspadding">
                                            <asp:Label runat="server" ID="Label2" Text="Delete"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trItemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <EmptyDataTemplate>
                                <table style="width: 100%">
                                    <tr>
                                        <td class="LblNoRecord" align="center">
                                            No record found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr class="ClsGridRow">
                                    <td align="center" class="Clspadding" style="width: 8%;">
                                        <asp:CheckBox runat="server" ID="chkSelect"></asp:CheckBox>
                                    </td>
                                    <td class="ClspaddingL" style="width: 46%;">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name")%>'></asp:Label>
                                    </td>
                                    <td class="ClspaddingL" style="width: 24%;">
                                        <asp:Label runat="server" ID="lblMobileNo" Text='<%#Eval("Mobile_No")%>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 10%;" class="Clspadding">
                                        <asp:ImageButton runat="server" ID="imgUpdate" Text="Edit" CommandName="EditAddress"
                                            CommandArgument='<%#Eval("PersonalAddressBookId")%>' CausesValidation="false"
                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit"></asp:ImageButton>
                                    </td>
                                    <td align="center" style="width: 12%;" class="Clspadding">
                                        <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="DeleteAddress"
                                            CommandArgument='<%#Eval("PersonalAddressBookId")%>' CausesValidation="false"
                                            ToolTip="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDelete()) return false;">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="ClsGridAltRow">
                                    <td align="center" class="Clspadding" style="width: 8%;">
                                        <asp:CheckBox runat="server" ID="chkSelect"></asp:CheckBox>
                                    </td>
                                    <td class="ClspaddingL" style="width: 46%;">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name")%>'></asp:Label>
                                    </td>
                                    <td class="ClspaddingL" style="width: 24%;">
                                        <asp:Label runat="server" ID="lblMobileNo" Text='<%#Eval("Mobile_No")%>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 10%;" class="Clspadding">
                                        <asp:ImageButton runat="server" ID="imgUpdate" Text="Edit" CommandName="EditAddress"
                                            CommandArgument='<%#Eval("PersonalAddressBookId")%>' CausesValidation="false"
                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit"></asp:ImageButton>
                                    </td>
                                    <td align="center" style="width: 12%;" class="Clspadding">
                                        <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="DeleteAddress"
                                            CommandArgument='<%#Eval("PersonalAddressBookId")%>' CausesValidation="false"
                                            ToolTip="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDelete()) return false;">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="optGroup" EventName="checkedchanged" />
                        <asp:AsyncPostBackTrigger ControlID="optIndividual" EventName="checkedchanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ListView ID="lstvwGroup" runat="server" ItemPlaceholderID="trItemPlaceholder"
                            DataKeyNames="PersonalAddressBookGroupId" Visible="false" 
                            OnItemCommand="lstvwGroup_ItemCommand" ondatabound="lstvwGroup_DataBound">
                            <LayoutTemplate>
                                <table id="tbllstvwAddressBookGroup" style="width: 80%; color: #333333" class="GridBorder"
                                    cellpadding="0" cellspacing="1">
                                    <tr id="trGroupHeader" runat="server" class="ClsGridHeader">
                                        <td align="center" class="Clspadding" style="width: 8%;">
                                            <asp:CheckBox runat="server" ID="chkSelect" Onclick="CheckAllGroup();"></asp:CheckBox>
                                        </td>
                                        <td class="ClspaddingL" style="width: 46%;">
                                            <asp:Label runat="server" ID="lblNameHdr" Text="Group Name"> </asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%;" class="Clspadding">
                                            <asp:Label runat="server" ID="Label1" Text="Edit"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 12%;" class="Clspadding">
                                            <asp:Label runat="server" ID="Label2" Text="Delete"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trItemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <EmptyDataTemplate>
                                <table style="width: 100%">
                                    <tr>
                                        <td class="LblNoRecord" align="center">
                                            No record found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr class="ClsGridRow">
                                    <td align="center" class="Clspadding" style="width: 8%;">
                                        <asp:CheckBox runat="server" ID="chkSelect" Checked='<%# Convert.ToBoolean(Eval("Ischeck"))%>'></asp:CheckBox>
                                    </td>
                                    <td class="ClspaddingL" style="width: 46%;">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name")%>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 10%;" class="Clspadding">
                                        <asp:ImageButton runat="server" ID="imgUpdate" Text="Edit" CommandName="EditAddress"
                                            CommandArgument='<%#Eval("PersonalAddressBookGroupId")%>' CausesValidation="false"
                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit"></asp:ImageButton>
                                    </td>
                                    <td align="center" style="width: 12%;" class="Clspadding">
                                        <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="DeleteAddress"
                                            CommandArgument='<%#Eval("PersonalAddressBookGroupId")%>' CausesValidation="false"
                                            ToolTip="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDeleteGroup()) return false;">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="ClsGridAltRow">
                                    <td align="center" class="Clspadding" style="width: 8%;">
                                        <asp:CheckBox runat="server" ID="chkSelect" Checked='<%# Convert.ToBoolean(Eval("Ischeck"))%>'></asp:CheckBox>
                                    </td>
                                    <td class="ClspaddingL" style="width: 46%;">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name")%>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 10%;" class="Clspadding">
                                        <asp:ImageButton runat="server" ID="imgUpdate" Text="Edit" CommandName="EditAddress"
                                            CommandArgument='<%#Eval("PersonalAddressBookGroupId")%>' CausesValidation="false"
                                            ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit"></asp:ImageButton>
                                    </td>
                                    <td align="center" style="width: 12%;" class="Clspadding">
                                        <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="DeleteAddress"
                                            CommandArgument='<%#Eval("PersonalAddressBookGroupId")%>' CausesValidation="false"
                                            ToolTip="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDeleteGroup()) return false;">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="optGroup" EventName="checkedchanged" />
                        <asp:AsyncPostBackTrigger ControlID="optIndividual" EventName="checkedchanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" valign="bottom">
                <table style="width: 80%;" cellpadding="0" cellspacing="1">
                    <tr>
                        <td align="right" valign="bottom">
                            <asp:Button Text="Ok" ID="imgBtnOKBottom" runat="server" CssClass="ClsBtnSml" OnClick="imgBtnOk_Click"
                                UseSubmitBehavior="false" ValidationGroup="SelectOK" />
                            <asp:Button Text="Close" ID="btnCloseBottom" runat="server" CssClass="ClsBtnSml"
                                UseSubmitBehavior="false" CausesValidation="False" OnClientClick="window.close(); return false;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="updtpnlAddAddress" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:ValidationSummary ID="valSumAddressBook" runat="server" HeaderText="Please currect follwing errors."
                                        ShowMessageBox="true" ShowSummary="false" />
                                </td>
                            </tr>
                            <tr id="trError">
                                <td align="center">
                                    <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="False"
                                        Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <table class="ClsBorderlight" style="width: 70%">
                                        <tr>
                                            <td align="center" valign="top" style="text-align: center; background-color: #eaeaea"
                                                class="ClsGridHeader">
                                                <asp:Label runat="server" ID="lblTitle" Font-Bold="true" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top">
                                                <table style="width: 70%">
                                                    <tr>
                                                        <td class="ClsBorderlight">
                                                            <span class="ClsLabel">Name :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtUserName" CssClass="MidTxtBox" MaxLength="100"></asp:TextBox>
                                                            <span style="color: red" id="spnMandatory" runat="server">*</span>
                                                            <asp:RequiredFieldValidator ID="reqdValUserName" runat="server" ControlToValidate="txtUserName"
                                                                Display="None" ErrorMessage="Name should not be blank."></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cstValcheck" runat="server" ClientValidationFunction="CheckValidCheckBoxes"
                                                                CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr id="trMobileNumber" runat="server">
                                                        <td class="ClsBorderlight">
                                                             <span class="ClsLabel">Mobile Number :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtUserMobileNo" CssClass="MidTxtBox" MaxLength="10"
                                                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                ondrop="event.returnValue=false"></asp:TextBox>
                                                            <span style="color: red" id="Span1" >*</span>
                                                            <asp:RequiredFieldValidator ID="reqdValMobileNo" runat="server" ControlToValidate="txtUserMobileNo"
                                                                Display="None" ErrorMessage="Mobile Number should not be blank."></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regValMobileNo" runat="server" ValidationExpression="[\s\S]{10,10}"
                                                                ControlToValidate="txtUserMobileNo" ErrorMessage="Mobile Number should be a 10 digit number."
                                                                Display="None"></asp:RegularExpressionValidator>
                                                            <asp:CustomValidator ID="cstValMobNos" runat="server" ClientValidationFunction="CheckValidMobileNos"
                                                                CssClass="LblErrorMsg" Display="None" ErrorMessage="Mobile number should not start with zero."></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button CssClass="ClsBtn" ID="btnAdd" runat="server" Text="Add" BorderWidth="1px"
                                                    CommandName="Add" OnClick="btnAdd_Click" CommandArgument="0" OnClientClick="MakeErrorInvisible();">
                                                </asp:Button>
                                                <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                                    Text="Cancel" BorderWidth="1px" OnClick="btnCancel_Click"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hidPersonalBookGroupId" runat="server" />
                                                <asp:HiddenField ID="hidGroupID" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidMobileNos" runat="server" />
                                                <asp:HiddenField ID="hidGroupMob" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwAddressBook" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="optGroup" EventName="checkedchanged" />
                        <asp:AsyncPostBackTrigger ControlID="optIndividual" EventName="checkedchanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ListView ID="lstvwGroupDetails" runat="server" ItemPlaceholderID="trItemPlaceholder"
                            Visible="false" DataKeyNames="PersonalAddressBookId" 
                            ondatabound="lstvwGroupDetails_DataBound">
                            <LayoutTemplate>
                                <table id="tbllstvwAddressBookGroupDetails" style="width: 80%; color: #333333" class="GridBorder"
                                    cellpadding="0" cellspacing="1">
                                    <tr id="trDetailsHeader" runat="server" class="ClsGridHeader">
                                        <td align="center" class="Clspadding" style="width: 8%;">
                                            <asp:CheckBox runat="server" ID="chkSelect" Onclick="CheckAllGroupDetails();"></asp:CheckBox>
                                        </td>
                                        <td class="ClspaddingL" style="width: 46%;">
                                            <asp:Label runat="server" ID="lblNameHdr" Text="Name"> </asp:Label>
                                        </td>
                                        <td class="ClspaddingL" style="width: 24%;">
                                            <asp:Label runat="server" ID="lblMobileNoHdr" Text="Mobile Number"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trItemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <EmptyDataTemplate>
                                <table style="width: 100%">
                                    <tr>
                                        <td class="LblNoRecord" align="center">
                                            No record found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr class="ClsGridRow">
                                    <td align="center" class="Clspadding" style="width: 8%;">
                                        <asp:CheckBox runat="server" ID="chkSelect" Checked='<%# Convert.ToBoolean(Eval("IsInGroup"))%>'>
                                        </asp:CheckBox>
                                    </td>
                                    <td class="ClspaddingL" style="width: 46%;">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name")%>'></asp:Label>
                                    </td>
                                    <td class="ClspaddingL" style="width: 24%;">
                                        <asp:Label runat="server" ID="lblMobileNo" Text='<%#Eval("Mobile_No")%>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="ClsGridAltRow">
                                    <td align="center" class="Clspadding" style="width: 8%;">
                                        <asp:CheckBox runat="server" ID="chkSelect" Checked='<%# Convert.ToBoolean(Eval("IsInGroup"))%>'>
                                        </asp:CheckBox>
                                    </td>
                                    <td class="ClspaddingL" style="width: 46%;">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name")%>'></asp:Label>
                                    </td>
                                    <td class="ClspaddingL" style="width: 24%;">
                                        <asp:Label runat="server" ID="lblMobileNo" Text='<%#Eval("Mobile_No")%>'></asp:Label>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="optGroup" EventName="checkedchanged" />
                        <asp:AsyncPostBackTrigger ControlID="optIndividual" EventName="checkedchanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwGroup" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>    
    <asp:ObjectDataSource ID="ObjDSPersonalAddBook" runat="server" SelectMethod="GetAddressBookList"
        TypeName="BusinessLogic.PersonalAddressBookBL">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="0" Name="aiUserId" SessionField="I_USER_ID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjDSGroup" runat="server" SelectMethod="GetAddressBookGroupList"
        TypeName="BusinessLogic.PersonalAddressBookBL">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="0" Name="aiUserId" SessionField="I_USER_ID" Type="Int32" />
            <asp:ControlParameter DefaultValue="0" PropertyName="Value" ControlID="hidGroupMob"
                Name="asGroupMob" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjDSGroupDetails" runat="server" SelectMethod="GetAddressBookGroupDetails"
        TypeName="BusinessLogic.PersonalAddressBookBL">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="0" Name="aiUserId" SessionField="I_USER_ID" Type="Int32" />
            <asp:ControlParameter DefaultValue="0" PropertyName="Value" ControlID="hidGroupID"
                Name="aiGroupID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <script type="text/javascript" language="javascript">
        _clientcstValMobNos = "<%=this.cstValMobNos.ClientID %>"
        _clienttxtUserMobileNo = "<%=this.txtUserMobileNo.ClientID %>"
        _clientlstvwAddressBook = '<%= lstvwAddressBook.ClientID %>'
        _clientlstvwGroup = '<%= lstvwGroup.ClientID %>'
        _clientlstvwGroupDetails = '<%= lstvwGroupDetails.ClientID %>'
        _clientoptIndividual = '<%= optIndividual.ClientID %>'
        _clientoptGroup = '<%= optGroup.ClientID %>'
        _clientcstValcheck = '<%= cstValcheck.ClientID %>'
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement != null && postBackElement.id == _clientoptIndividual) {
                GetAlreadyAddedManualNumbers()
            } 
        }
        function ConfirmDelete() {
            var bFlag = false
            bFlag = window.confirm('Are you sure you want to delete selected phone contact?')
            return bFlag
        }
        function ConfirmDeleteGroup() {
            var bFlag = false
            bFlag = window.confirm('Are you sure you want to delete selected group?')
            return bFlag
        }
        function GetAlreadyAddedManualNumbers() {
            var sMobileNos = window.opener.getManualNumbers()
            $get('<%= hidGroupMob.ClientID %>').value = sMobileNos
            var sArrMobileNos = sMobileNos.split(',')
            var bFlag = false
            var i
            var SelectedCount = 0
            var _clientListViewId = _clientlstvwAddressBook
            if ($get('tbllstvwAddressBook') != null) {
                var iRowCount = $get('tbllstvwAddressBook').rows.length
                var oListView = document.getElementById(_clientListViewId)
                for (i = 0; i < iRowCount - 1; i++) {
                    var iMobileCnt = 0
                    for (iMobileCnt = 0; iMobileCnt < sArrMobileNos.length; iMobileCnt++) {
                        var chkSelect = _clientListViewId + "_ctrl" + i + "_" + "chkSelect"
                        var lblMobileNo = _clientListViewId + "_ctrl" + i + "_" + "lblMobileNo"
                        if (document.getElementById(lblMobileNo) != null) {
                            var MNo = document.getElementById(lblMobileNo).innerHTML
                            sArrMobileNos[iMobileCnt] = trimAll(sArrMobileNos[iMobileCnt])
                            if (trimAll(MNo) == sArrMobileNos[iMobileCnt]) {
                                document.getElementById(chkSelect).checked = true
                                sArrMobileNos.splice(iMobileCnt, 1)
                                SelectedCount++
                            } 
                        } 
                    } 
                }
                if (SelectedCount == iRowCount - 1)
                    $get(_clientListViewId + "_" + "chkSelect").checked = true
                $get('<%= hidMobileNos.ClientID %>').value = sArrMobileNos.join(", ")
            } 
        }
        GetAlreadyAddedManualNumbers()
        function MakeErrorInvisible() {
            if ($get('trError') != null)
                $get('trError').style.visibility = 'hidden'
        }
        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true
            if (document.getElementById(_clientoptIndividual).checked) {
                if ($get('tbllstvwAddressBook') != null) {
                    if (CheckAtleastOneCheckBox(_clientlstvwAddressBook, 'chkSelect', $get('tbllstvwAddressBook').rows.length)) {
                        bResult = true
                    }
                    else {
                        bResult = window.confirm(sActionName)
                    } 
                } 
            }
            else {
                if ($get('tbllstvwAddressBookGroup') != null) {
                    if (CheckAtleastOneCheckBox(_clientlstvwGroup, 'chkSelect', $get('tbllstvwAddressBookGroup').rows.length)) {
                        bResult = true
                    }
                    else {
                        bResult = window.confirm(sActionName)
                    } 
                } 
            }
            return bResult
        }
        function CheckAll() {
            CheckOrUnCheckAllCheckBox(_clientlstvwAddressBook, 'chkSelect', $get('tbllstvwAddressBook').rows.length)
        }
        function CheckAllGroup() {
            CheckOrUnCheckAllCheckBox(_clientlstvwGroup, 'chkSelect', $get('tbllstvwAddressBookGroup').rows.length)
        }
        function CheckAllGroupDetails() {
            CheckOrUnCheckAllCheckBox(_clientlstvwGroupDetails, 'chkSelect', $get('tbllstvwAddressBookGroupDetails').rows.length)
        }
        function CheckValidMobileNos(oSrc, args) {
            var strmno = document.getElementById(_clienttxtUserMobileNo).value
            if (strmno != '') {
                if (strmno.length == 10) {
                    if (strmno.substring(0, 1) == '0') {
                        document.getElementById(_clientcstValMobNos).errormessage = "Mobile numbers should not start with zero."
                        args.IsValid = false
                        return true
                    } 
                } 
            }
            args.IsValid = true
            return false
        }
        function CheckValidCheckBoxes(oSrc, args) {
            if (document.getElementById(_clientoptGroup).checked) {
                if ($get('tbllstvwAddressBookGroupDetails') != null) {
                    if (CheckAtleastOneCheckBox(_clientlstvwGroupDetails, 'chkSelect', $get('tbllstvwAddressBookGroupDetails').rows.length)) {
                        args.IsValid = true
                        return false
                    }
                    else {
                        document.getElementById(_clientcstValcheck).errormessage = "At least one mobile number should be selected for group."
                        args.IsValid = false
                        return true
                    } 
                }
                else {
                    document.getElementById(_clientcstValcheck).errormessage = "At least one mobile number should be selected for group."
                    args.IsValid = false
                    return true
                } 
            }
            args.IsValid = true
            return false
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
