<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SchoolDesignationUI.aspx.cs" Inherits="SchoolDesignationUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
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
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="1" id="tdMessage" runat="server" class="ClsTextNormal" align="center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="tblDesignationName" runat="server" border="0" cellpadding="1" cellspacing="2"
        width="30%" align="center">
        <tr>
            <td colspan="2" align="center">
                <asp:RadioButton ID="rdoSchoolDesig" runat="server" CssClass="clsLabel" AutoPostBack="true"
                    GroupName="Designation" Text="School Designation" OnCheckedChanged="rdoSchoolDesig_CheckedChanged" />
                <asp:RadioButton ID="rdoPTADesig" runat="server" CssClass="clsLabel" AutoPostBack="true"
                    GroupName="Designation" Text="PTA Designation" OnCheckedChanged="rdoPTADesig_CheckedChanged" />
            </td>
        </tr>
        <tr style="height: 5px;">
            <td>
            </td>
        </tr>
        <tr align="center">
            <td align="left" class="ClsBorderLight" style="width: 49%">
                <span class="ClsLabel">User Role :</span>
            </td>
            <td align="left">
                <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="MidCombo" AutoPostBack="true"
                    CausesValidation="false" OnSelectedIndexChanged="cmbUserRole_SelectedIndexChanged">
                </asp:DropDownList>
                <span class="ClsMdtStar">*</span>
                <asp:RequiredFieldValidator ID="reqCmbCategory" runat="server" Display="None" ControlToValidate="cmbUserRole"
                    CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="User Role should be selected."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr align="center">
            <td align="left" class="ClsBorderLight" style="width: 49%">
                <span class="ClsLabel">Designation :</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDesignationName" runat="server" MaxLength="50" CssClass="MidTxtBox"></asp:TextBox>
                <span class="ClsMdtStar">*</span>
                <asp:RequiredFieldValidator ID="reqDesignationName" runat="server" ControlToValidate="txtDesignationName"
                    Display="None" ErrorMessage="Designation Name should not be blank."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr align="center">
            <td align="left" class="ClsBorderLight" style="width: 49%">
                <span class="ClsLabel">Sort Order :</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSortOrder" runat="server" MaxLength="5" CssClass="SmlTxtBox"
                    onblur="extractNumber(this,1,true);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, true);"
                    onkeyup="extractNumber(this,1,true);" onpaste="event.returnValue=false"></asp:TextBox>
                <span class="ClsMdtStar">*</span>
                <asp:RequiredFieldValidator ID="reqBlankSortOrder" runat="server" ControlToValidate="txtSortOrder"
                    Display="None" ErrorMessage="Sort Order should not be blank."></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="reqZeroSortOrder" InitialValue="0" runat="server"
                    ControlToValidate="txtSortOrder" Display="None" ErrorMessage="Sort Order should not be zero."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trIsAccountAvailable" runat="server">
            <td align="left" class="ClsBorderlight">
                <span class="ClsLabel">Is Accounting Screen Available? : </span>
            </td>
            <td class="ClsLabel" align="left">
                <asp:CheckBox ID="chkIsAccountingScreenAvailable" runat="server" Enabled="false"
                    Checked="false" />
            </td>
        </tr>
    </table>
    <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
        style="width: 300px;" align="center">
        <tr>
            <td align="center">
                <asp:Button ID="btnAdd" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                    disable-page="true" OnClick="btnAdd_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" OnClick="btnCancel_Click" />&nbsp;
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td align="center">
                            <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwConfigureDesignation"
                                PageSize="20">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1%>" />
                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                            <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                            <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                            <br />
                                        </PagerTemplate>
                                    </asp:TemplatePagerField>
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                </table>
                <table id="tblDesignationNameList" align="center" width="100%">
                    <tr align="center">
                        <td align="center">
                            <asp:ListView ID="lstvwConfigureDesignation" runat="server" DataKeyNames="DesignationId,UserRoleId"
                                OnSorting="lstvwConfigureDesignation_Sorting" DataSourceID="ObjDSConfigureDesignation"
                                OnItemDataBound="lstvwConfigureDesignation_ItemDataBound" OnDataBound="lstvwConfigureDesignation_DataBound"
                                OnItemCommand="lstvwConfigureDesignation_ItemCommand" OnItemEditing="lstvwConfigureDesignation_ItemEditing">
                                <LayoutTemplate>
                                    <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                        cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="left" width="100px" style="padding-left: 9px;">
                                                User Role
                                            </th>
                                            <th align="left" style="padding-left: 9px;">
                                                <asp:LinkButton ID="lnkBtnDesignationName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                    CausesValidation="false" ForeColor="Black">Designation </asp:LinkButton>
                                            </th>
                                            <th align="right" width="100px" style="text-align:right; padding-right: 5px;">
                                                <asp:LinkButton ID="lnkbtnSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder"
                                                    CausesValidation="false" ForeColor="Black">Sort Order</asp:LinkButton>                                            
                                            </th>
                                            <th align="center" width="250px" id="thIsAccountScreen">
                                                Is Accounting Screen Available?
                                            </th>
                                            <th align="center" width="50px">
                                                Edit
                                            </th>
                                            <th align="center" width="50px">
                                                Delete
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                        <tr class="ClsBorderPager" id="trDataPager">
                                            <td colspan="6">
                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwConfigureDesignation"
                                                    PageSize="20">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                            <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
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
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserRoleName") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                        </td>
                                        <td align="right" style="padding-right: 5px;">
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label>
                                        </td>
                                        <td align="center" id="tIsAccountingScreen">
                                            <asp:Image ID="imgTickmark" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ImageUrl="../images/IconGrid_Edit.GIF" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                ImageUrl="../images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserRoleName") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                        </td>
                                        <td align="right" style="padding-right: 5px;">
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label>
                                        </td>
                                        <td align="center" id="tIsAccountingScreen">
                                            <asp:Image ID="imgTickmark" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ImageUrl="../images/IconGrid_Edit.GIF" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                </table>
                <asp:ObjectDataSource TypeName="BusinessLogic.DesignationMasterBL" EnablePaging="True"
                    ID="ObjDSConfigureDesignation" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                    EnableCaching="False" SelectCountMethod="Count">
                    <SelectParameters>
                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                            Type="int32" />
                        <asp:Parameter Name="sortExpression" Type="String" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:ControlParameter Name="abIsPTADesignation" ControlID="rdoPTADesig" Type="Boolean"
                            PropertyName="Checked" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="hidSortDirection" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidMode" runat="server" />
                <asp:HiddenField ID="hidServerDate" runat="server" />
                <asp:HiddenField ID="hidDesignationId" runat="server" />
                <asp:HiddenField ID="hidDesignationName" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";

        function ClearSuccessfulMessage() {
            document.getElementById(_clientlblMessage).innerHTML = "";
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }       
           
    </script>
</asp:Content>
