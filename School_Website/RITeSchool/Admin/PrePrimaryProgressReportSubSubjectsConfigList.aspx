<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PrePrimaryProgressReportSubSubjectsConfigList.aspx.cs"
    Inherits="PrePrimaryProgressReportSubSubjectsConfigList" ValidateRequest="true" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:UpdatePanel UpdateMode="always" runat="server" ID="UpdatePanel9">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValSummaryErrMsg" CssClass="LblErrorMsg" runat="server"
                                            ShowMessageBox="False" ShowSummary="True" ValidationGroup="Save" />
                                        <asp:ValidationSummary ID="ValSummaryCopy" CssClass="LblErrorMsg" runat="server"
                                            ShowMessageBox="False" ShowSummary="True" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333" valign="top">
                    <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:UpdatePanel UpdateMode="always" runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel UpdateMode="always" runat="server" ID="UpdatePanel10">
                       <ContentTemplate>
                          <asp:Label ID="lblSuccess" CssClass="ClsLabelUpdate" Font-Bold="True" ForeColor="Blue"
				          Visible="true" runat="server" ></asp:Label>
                       </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                
            </tr>
            <tr>
                <td align="center" style="width: 90%;" colspan="2">
                    <asp:UpdatePanel UpdateMode="always" runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <table border="0" cellpadding="1" cellspacing="1">
                                <tr align="center">
                                    <td align="center" colspan="1" class="ClsOnlyBorderlght">
                                        <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="Standard : "
                                            EnableViewState="false"></asp:Label>
                                        <span class="ClsMdtStar" style="color: #ff0000"></span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbStandard" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged"
                                            CssClass="SmlCombo" TabIndex="1">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                        <asp:CompareValidator runat="server" ID="cmpStandard" Display="None" ControlToValidate="cmbStandard"
                                            ValidationGroup="Save" Operator="NotEqual" ValueToCompare="0" ErrorMessage="Standard should be selected."></asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                        <ContentTemplate>
                            <table border="0" cellpadding="1" cellspacing="1" id="tblSave" runat="server" width="95%">
                                <tr align="left">
                                    <td align="left" class="ClsOnlyBorderlght">
                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Module Name : " EnableViewState="false"
                                            Width="150px"></asp:Label>
                                        <span class="ClsMdtStar" style="color: #ff0000"></span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbModuleName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbModuleName_SelectedIndexChanged"
                                            CssClass="LrgCombo" TabIndex="2">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                        <asp:CompareValidator runat="server" ID="cmpModule" Display="None" ControlToValidate="cmbModuleName"
                                            ValidationGroup="Save" Operator="NotEqual" ValueToCompare="0" ErrorMessage="Module Name should be selected."></asp:CompareValidator>
                                    </td>
                                    <td align="left" class="ClsOnlyBorderlght">
                                        <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Pre-Primary Subject Name : "
                                            EnableViewState="False" Width="161px"></asp:Label>
                                        <span class="ClsMdtStar" style="color: #ff0000"></span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbSubjectName" runat="server" CssClass="LrgCombo" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblSubjectMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                            Text="*" Width="14px" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr align="left" valign="middle">
                                    <%-- <td>
                                    </td>--%>
                                    <td align="left" class="ClsOnlyBorderlght">
                                        <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Skills / Behaviour Name : "
                                            EnableViewState="false"></asp:Label>
                                        <span class="ClsMdtStar" style="color: #ff0000"></span>
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:TextBox ID="txtSubjectName" runat="server" MaxLength="50" CssClass="MidTxtBox"
                                            TabIndex="3" Width="195px"></asp:TextBox>
                                        <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                        <asp:RequiredFieldValidator ID="reqSubjectName" Display="None" runat="server" ErrorMessage="Skills / Behaviour name should not be blank."
                                            ControlToValidate="txtSubjectName" ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" style="height: 20px" class="ClspaddingT" colspan="4">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" BorderWidth="1px"
                                            Enabled="true" OnClick="btnSave_Click" ValidationGroup="Save" TabIndex="4" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                            TabIndex="4" CausesValidation="False" OnClick="btnCancel_Click" />
                                        <div id="divSortOrder" runat="server" align="center" class="ToprLinkHlilight" style="width: 120px;
                                            height: 18px; float: right" visible="false">
                                            <asp:HyperLink ID="hlnkSortOrder" runat="server" CssClass="ClsHilightTextB" NavigateUrl="SortSubSubjectPopup.aspx"
                                                Target="_blank">Sort Order</asp:HyperLink>
                                            <asp:CustomValidator ID="cstSubSubject" runat="server" Display="none" ValidationGroup="Save"
                                                EnableClientScript="true" ClientValidationFunction="ValidateSaveSubSubject" ErrorMessage="Subject Name should be selected."></asp:CustomValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbModuleName" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwSubject" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                        <ContentTemplate>
                            <table border="0" cellpadding="1" cellspacing="1" width="95%" id="tbllstSub" runat="server">
                                <tr id="trDtPgr" runat="server">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwSubject">
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
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel8">
                                            <ContentTemplate>
                                                <asp:ListView ID="lstvwSubject" runat="server" DataKeyNames="SubSubjectID,SubjectID,ModuleID,SubSubjectName"
                                                    OnDataBound="lstvwSubject_DataBound" OnItemCommand="lstvwSubject_ItemCommand"
                                                    OnItemDataBound="lstvwSubject_ItemDataBound" OnSorting="lstvwSubject_Sorting">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="ClspaddingL">
                                                                    <asp:LinkButton ID="lnkModuleName" runat="server" CommandName="Sort" CommandArgument="ModuleName"
                                                                        ForeColor="Black">Module Name</asp:LinkButton>
                                                                </th>
                                                                <th align="left" class="ClspaddingL">
                                                                    <asp:LinkButton ID="lnkSubjectName" runat="server" CommandName="Sort" CommandArgument="SubjectName"
                                                                        ForeColor="Black">Subject Name</asp:LinkButton>
                                                                </th>
                                                                <th align="left" class="ClspaddingL">
                                                                    <asp:LinkButton ID="lnkSubSubjectName" runat="server" CommandName="Sort" CommandArgument="SubSubjectName"
                                                                        ForeColor="Black">Skills / Behaviour Name</asp:LinkButton>
                                                                </th>
                                                                <th align="center">
                                                                    Edit
                                                                </th>
                                                                <th align="center">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                <td colspan="5">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwSubject">
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
                                                        <tr class="ClsGridRow">
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("ModuleName") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("SubjectName") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("SubSubjectName") %>' />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnEditReq" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                    ToolTip="Edit" CommandArgument='<%# Eval("SubSubjectID") %>' CommandName="Modify" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnDeleteReq" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                    CommandName="Remove" CommandArgument='<%# Eval("SubSubjectID") %>' ToolTip="Delete" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("ModuleName") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("SubjectName") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("SubSubjectName") %>' />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnEditReq" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                    ToolTip="Edit" CommandArgument='<%# Eval("SubSubjectID") %>' CommandName="Modify" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnDeleteReq" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                    CommandName="Remove" CommandArgument='<%# Eval("SubSubjectID") %>' ToolTip="Delete" />
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
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                            <ContentTemplate>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.PrePrimaryProgressSheetConfigBL" EnablePaging="true"
                                                    ID="lstDSobj" runat="server" SelectMethod="GetAllConfiguredPrePrimarySubSubjects"
                                                    SortParameterName="sortExpression" SelectCountMethod="CountAllConfiguredPrePrimarySubSubjects"
                                                    EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:SessionParameter Name="aiAcademicYrID" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                            Type="int32" />
                                                        <asp:ControlParameter ControlID="cmbStandard" PropertyName="SelectedValue" Name="aiStandardId"
                                                            Type="int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                            <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 100%;">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                        <ContentTemplate>
                            <table border="0" cellpadding="1" cellspacing="1" id="tblCopy" runat="server">
                                <tr align="center">
                                    <td align="center" colspan="1" class="ClsOnlyBorderlght">
                                        <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Copy Configuration To : "
                                            EnableViewState="False"></asp:Label>
                                        <span class="ClsMdtStar" style="color: #ff0000"></span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbCopyStandard" runat="server" CssClass="SmlCombo" TabIndex="5">
                                        </asp:DropDownList>
                                        <%--<span class="ClsMdtStar" style="color: #ff0000">* </span>--%>
                                        <%--<asp:CompareValidator runat="server" ID="CompareValidator1" Display="None" ControlToValidate="cmbCopyStandard"
                                            Operator="NotEqual" ValueToCompare="0" ErrorMessage="Please select standard to copy the configuration."
                                            ValidationGroup="Copy"></asp:CompareValidator>--%>
                                        <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="ClsBtn" BorderWidth="1px"
                                            TabIndex="5" OnClick="btnCopy_Click" CausesValidation="True" ValidationGroup="Copy" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 100%;">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                        TabIndex="6" CausesValidation="false" UseSubmitBehavior="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hidSubSubjectId" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hidIsSubjectApplicable" runat="server"></asp:HiddenField>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbModuleName" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwSubject" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientlblErrorMsgId = "<%=this.lblErrorMsg.ClientID %>"
        _clientcstSubSubject = "<%=this.cstSubSubject.ClientID %>"
        _clientcmbSubjectName = "<%=this.cmbSubjectName.ClientID %>"
        _clientcmbCopyStandard = "<%=this.cmbCopyStandard.ClientID %>"
        _clienthidIsSubjectApplicable = "<%=this.hidIsSubjectApplicable.ClientID %>"
        function ConfirmDelete() {
            var bResult = true
            if ($get("<%= this.lblSuccess.ClientID %>") != null)
                $get("<%= this.lblSuccess.ClientID %>").style.display = "none";
            if (!window.confirm('Are you sure you want to delete this Skills / Behaviour?')) 
                bResult = false;
            return bResult;
        }

        function ConfirmCopyAction() {
            var sActionOverride = "This action will overwrite all predefined configuration for selected standard. Are you sure you want to continue?"
            var sCheckBoxMessage = 'Atleast one standard should be selected. '
            document.getElementById(_clientlblErrorMsgId).style.visibility = "hidden"
            var bResult = false
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == true) {
                if (CheckForIsStandardSelected())
                    bResult = confirm(sActionOverride)
                else {
                    alert(sCheckBoxMessage)
                    bResult = false;
                }
            }
            return bResult
        }


        function CheckForIsStandardSelected() {
            if (document.getElementById(_clientcmbCopyStandard).value == 0)
                return false
            else
                return true
        }

        function ValidateSaveSubSubject(source, args) {
            var iSubjectIndex = document.getElementById(_clientcmbSubjectName).selectedIndex
            var iModuleFlag = document.getElementById(_clienthidIsSubjectApplicable).value
            if ($get("<%= this.lblSuccess.ClientID %>") != null)
                $get("<%= this.lblSuccess.ClientID %>").style.display = "none";
            var bIsValid = true
            if (iModuleFlag != 0 && iSubjectIndex == 0) {
                document.getElementById(_clientcstSubSubject).errormessage = "Subject Name should be selected."
                bIsValid = false
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
    </script>

</asp:Content>
