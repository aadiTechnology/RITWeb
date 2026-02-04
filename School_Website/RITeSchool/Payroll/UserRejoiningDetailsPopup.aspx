<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="UserRejoiningDetailsPopup.aspx.cs" Inherits="UserRejoiningDetailsPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%">
        <tr>
            <td valign="top">
                <table width="100%">
                    <tr>
                        <td align="left" style="height: 20px; width: 99%;" class="ClsGrayMainTitle">
                            <span style="font-weight: bold">User Joining Details.</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right: 30px" valign="bottom">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                    Text="Mandatory Fields"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" />
                        <asp:RequiredFieldValidator ID="reqCmbStaffGroup" runat="server" Display="None" ErrorMessage="Staff group should be selected."
                            ControlToValidate="cmbStaffGroup" InitialValue="0"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqStaffMember" runat="server" Display="None" ErrorMessage="Name should be selected."
                            ControlToValidate="cmbUserName" InitialValue="0"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="CompareDates"
                            Display="None"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="reqJoiningDate" runat="server" Display="None" ErrorMessage="Joining date should not be blank."
                            ControlToValidate="txtJoiningDate"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqResignationDate" runat="server" Display="None" ErrorMessage="Resignation date should not be blank."
                            ControlToValidate="txtResignationDate"></asp:RequiredFieldValidator>
                         <asp:CustomValidator ID="cstValidateDate" runat="server" ErrorMessage="" ClientValidationFunction="ValidateJoiningDate"
                            Display="None"></asp:CustomValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbUserName" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwUserDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr align="center">
            <td id="tdMessage" runat="server" align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" Style="text-align: center;" runat="server" Text="" EnableViewState="false"
                            CssClass="LblNormal"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbUserName" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwUserDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td class="ClsBorderlight" style="width: 140px;">
                                    <asp:Label ID="lblStaffGroup" runat="server" CssClass="ClsLabel" Text="Staff Group"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbStaffGroup" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                        TabIndex="1" Width="218px" OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td>
                                </td>
                                <td class="ClsBorderlight" style="width: 110px;">
                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text="Name"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbUserName" runat="server" CssClass="LrgCombo" TabIndex="2"
                                                Width="219px" AutoPostBack="true" OnSelectedIndexChanged="cmbUserName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">* </span>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Employee No."></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtEmployeeNo" CssClass="LrgTxtBox" runat="server" TabIndex="3"></asp:TextBox>                                    
                                </td>
                                <td>
                                </td>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Account No."></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAccountNo" CssClass="LrgTxtBox" runat="server" TabIndex="4"></asp:TextBox>                                    
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="P.F. No."></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPFNo" CssClass="LrgTxtBox" runat="server" TabIndex="5"></asp:TextBox>                                    
                                </td>
                                <td>
                                </td>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text="UAN"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtUAN" CssClass="LrgTxtBox" runat="server" TabIndex="6"></asp:TextBox>                                    
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text="PAN No."></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPANNo" CssClass="LrgTxtBox" runat="server" TabIndex="7"></asp:TextBox>                                    
                                </td>
                                <td>
                                </td>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Joining Date"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtJoiningDate" CssClass="SmlTxtBox" runat="server" TabIndex="8"></asp:TextBox>
                                    <rjs:PopCalendar ID="calJoiningDate" runat="server" Control="txtJoiningDate" Format="dd MMM yyyy"
                                        Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" Text="Last Resignation Date"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtResignationDate" CssClass="SmlTxtBox" runat="server" TabIndex="9"></asp:TextBox>
                                    <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtResignationDate" Format="dd MMM yyyy"
                                        Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px;" colspan="5">                                    
                                    <asp:HiddenField ID="hidUserReJoiningId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidUserStaffGroupId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidOldJoiningDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="5">
                                    <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="Save" TabIndex="10"
                                        OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="Cancel" CausesValidation="False"
                                        TabIndex="11" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwUserDetails" EventName="ItemCommand" />
                         <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="cmbUserName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <hr style="color: #C0C0C0" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" align="center">
                            <table width="60%">
                                <tr>
                                    <td class="ClsBorderlight" align="center" style="width: 150px;">
                                        <asp:Label ID="lblNameSearch" runat="server" CssClass="ClsLabel" Text="Name"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtSearch" CssClass="ExLrgTxtBox" runat="server" TabIndex="11"></asp:TextBox>
                                        <asp:Button ID="btnSearch" CssClass="ClsBtn" runat="server" Text="Search" TabIndex="12"
                                            CausesValidation="false" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="80%">
                                        <tr id="trItemCount" runat="server">
                                            <td align="center" style="width: 80%;">
                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwUserDetails"
                                                    Visible="true">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                                    Text="<%# Container.StartRowIndex + 1%>" />
                                                                <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                    Text=" To " />
                                                                <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                    Text=" Out Of " />
                                                                <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                    Text="Records " />
                                                                <br />
                                                            </PagerTemplate>
                                                        </asp:TemplatePagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstvwUserDetails" runat="server" DataKeyNames="UserRejoinId,UserId"
                                                    OnItemCommand="lstvwUserDetails_ItemCommand" OnItemDataBound="lstvwUserDetails_ItemDataBound"
                                                    OnDataBound="lstvwUserDetails_DataBound">
                                                    <LayoutTemplate>
                                                        <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                <th align="left" class="paddingL" style="width: 160px; font-size: 10pt;">
                                                                    Name
                                                                </th>
                                                                <th align="center" class="paddingL" style="width: 130px; font-size: 10pt;">
                                                                    Joinig Date
                                                                </th>
                                                                <th align="center" class="paddingL" style="width: 130px; font-size: 10pt;">
                                                                    Last Resignation Date
                                                                </th>
                                                                <th width="40px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                                    <asp:Label ID="lblEdit" runat="server" Text="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                                </th>
                                                                <th width="40px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                                    <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                <td colspan="7" align="left">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwUserDetails">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
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
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left">
                                                                <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                    Text='<%#Eval("UserName") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblJoiningDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("JoiningDate") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblResignationDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("ResignationDate") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                    ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                    ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="left">
                                                                <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                    Text='<%#Eval("UserName") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblJoiningDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("JoiningDate") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblResignationDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("ResignationDate") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                    ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                    ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    No record found.
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.UserRejoiningDetailsBL" EnablePaging="true"
                                                    ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                                    EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:ControlParameter Name="asFilter" ControlID="txtSearch" PropertyName="Text" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="lstvwUserDetails" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" align="center">
                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Close %>"
                                CssClass="ClsBtn" OnClientClick="ClosePopup(); return false;" CausesValidation="false"
                                TabIndex="13" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clienttxtJoiningDate = "<%=this.txtJoiningDate.ClientID %>";
        _clienttxtResignationDate = "<%=this.txtResignationDate.ClientID %>";
        _clienthidOldJoiningDate = "<%=this.hidOldJoiningDate.ClientID %>"

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function ClosePopup() {
            window.close();
        }

        function CompareDates(oSrc, args) {
            var JoiningDate = $('#' + _clienttxtJoiningDate).val()
            var ResignationDate = $('#' + _clienttxtResignationDate).val()

            var Joining;
            if (document.all)
                Joining = new Date(JoiningDate.replace('-', ' '));
            else
                Joining = new Date(convertdate(JoiningDate));

            var Resignation;
            if (document.all)
                Resignation = new Date(ResignationDate.replace('-', ' '));
            else
                Resignation = new Date(convertdate(ResignationDate));

            if (Resignation >= Joining) {
                oSrc.errormessage = "Joining date should be greater than Last Resignation Date.";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateJoiningDate(oSrc, args) {
            var JoiningDate = $('#' + _clienttxtJoiningDate).val()
            var OldJoiningDate = $('#' + _clienthidOldJoiningDate).val()
            var ResignationDate = $('#' + _clienttxtResignationDate).val()

            if (OldJoiningDate != null) {

                var Joining;
                if (document.all)
                    Joining = new Date(JoiningDate.replace('-', ' '));
                else
                    Joining = new Date(convertdate(JoiningDate));

                var Resignation;
                if (document.all)
                    Resignation = new Date(ResignationDate.replace('-', ' '));
                else
                    Resignation = new Date(convertdate(ResignationDate));

                var OldJoining
                if(document.all)
                    OldJoining = new Date(OldJoiningDate.replace('-', ' '));
                else
                    OldJoining = new Date(convertdate(OldJoiningDate));

                if (OldJoining >= Resignation) {
                    oSrc.errormessage = "Joining date should be greater than Last Joining Date. Last Joining date is - " + OldJoiningDate;
                    args.IsValid = false
                    return true
                }
                else {
                    if (OldJoining >= Joining) {
                        oSrc.errormessage = "Resignation date should be greater than Last Joining Date. Last Joining date is - " + OldJoiningDate;
                        args.IsValid = false
                        return true
                    }
                    args.IsValid = true
                    return false                     
                }                
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
