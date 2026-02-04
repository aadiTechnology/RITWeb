<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RemarksLengthConfigurationUI.aspx.cs" Inherits="RemarksLengthConfigurationUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="upnlError" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="right" class="TxtNormal" style="padding-right: 10px; top: 20px">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                            Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="valsumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                            HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                            ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                        <asp:Label ID="lblErrorMsg" Style="text-align: center" runat="server" ForeColor="Red"
                            Width="100%" CssClass="ClsMdtStar"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%" align="center">
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="upnlMaster">
                    <ContentTemplate>
                        <table width="70%" align="center" cellpadding="0" cellspacing="1" border="0">
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upnlData" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td class="ClsBorderlight" width="160px" runat="server" id="td1">
                                                        <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, Standard%>"
                                                            EnableViewState="false"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbStandard" Width="200px" runat="server" CssClass="LrgCombo"
                                                            AutoPostBack="true" TabIndex="1">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                        <asp:RequiredFieldValidator ID="reqcmbReportingParameter" runat="server" Display="None"
                                                            InitialValue="0" ControlToValidate="cmbStandard" ErrorMessage="<%$ Resources:LocalizedResources, StandardShouldSelected%>"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" width="160px" runat="server" id="tdRole">
                                                        <asp:Label ID="lblTerm" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, Term%>"
                                                            EnableViewState="false"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbTerm" Width="200px" runat="server" CssClass="LrgCombo" AutoPostBack="false"
                                                            TabIndex="1">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">*</span>
                                                        <asp:RequiredFieldValidator ID="reqcmbRole" runat="server" Display="None" InitialValue="0"
                                                            ControlToValidate="cmbTerm" ErrorMessage="<%$ Resources:LocalizedResources, TermShouldBeSelected%>"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ClsBorderlight" width="160px" runat="server" id="tdUser">
                                                        <asp:Label ID="lblRemarkLength" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, ValProgressRemarkLength%>"
                                                            EnableViewState="false"></asp:Label>
                                                        <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:TextBox ID="txtRemarkLength" CssClass="SmlTxtBox" runat="server" MaxLength="3"
                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                            ondrop="event.returnValue=false" />
                                                        <span style="color: #ff0000">*</span>
                                                        <%--<asp:RequiredFieldValidator ID="reqRemarkLength" runat="server" Display="None" ControlToValidate="txtRemarkLength"
                                                            ErrorMessage="<%$ Resources:LocalizedResources, RemarkLengthCondition%>"></asp:RequiredFieldValidator>--%>
                                                        <asp:CustomValidator ID="cstvalRemarkLength" runat="server" ClientValidationFunction="ValidateLength"
                                                            Display="None" ErrorMessage="Select" SetFocusOnError="True">
                                                        </asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbTerm" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnls" runat="server">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Save%>"
                                                            disable-page="true" TabIndex="4" OnClick="btnSave_Click" CausesValidation="true" />
                                                        <asp:CustomValidator ID="cstvalRole" runat="server" ClientValidationFunction="CheckDuplication"
                                                            Display="None" ErrorMessage="Select" SetFocusOnError="True">
                                                        </asp:CustomValidator>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                                            CausesValidation="false" UseSubmitBehavior="true" TabIndex="5" OnClick="btnCancel_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbTerm" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="90%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlListView" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ListView ID="lstvwRemarkLengthConfiguration" runat="server" DataKeyNames="StandardwiseRemarkLengthId,StandardId,TermId"
                                                            OnItemCommand="lstvwRemarkLengthConfiguration_ItemCommand" OnItemDataBound="lstvwRemarkLengthConfiguration_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table align="center" width="100%" runat="server" id="tblUserInfo" style="color: #333333"
                                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th align="left" style="width: 500px; padding-left: 5px">
                                                                            <asp:Label ID="lblUserName" runat="server" Text="<%$ Resources:LocalizedResources, Standard%>" />
                                                                        </th>
                                                                        <th align="left" style="width: 500px; padding-left: 5px">
                                                                            <asp:Label ID="lblReportingParameter" runat="server" Text="<%$ Resources:LocalizedResources, Term%>" />
                                                                        </th>
                                                                        <th align="right" style="width: 500px; padding-right: 5px">
                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, ValProgressRemarkLength%>" />
                                                                        </th>
                                                                        <th align="center" style="width: 150px">
                                                                            <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit %>" />
                                                                        </th>
                                                                        <th align="center" style="width: 150px">
                                                                            <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete %>" />
                                                                        </th>
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                    <td align="left" style="padding-left: 5px">
                                                                        <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>' />
                                                                        <asp:HiddenField ID="HidStandardId" runat="server" Value='<%# Eval("StandardId") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClsLabelL">
                                                                        <asp:Label ID="lblTermName" runat="server" Text='<%# Eval("Term") %>' />
                                                                        <asp:HiddenField ID="hidTermId" runat="server" Value='<%# Eval("TermId") %>' />
                                                                    </td>
                                                                    <td style="padding-right:5px" align="right">
                                                                        <asp:Label ID="lblLength" runat="server" Text='<%# Eval("MaxRemarkLength") %>' />
                                                                        <asp:HiddenField ID="HidStandardwiseRemarkLengthId" runat="server" Value='<%# Eval("StandardwiseRemarkLengthId") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="UpdateCommand" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                                    <td align="left" style="padding-left: 5px">
                                                                        <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>' />
                                                                        <asp:HiddenField ID="HidStandardId" runat="server" Value='<%# Eval("StandardId") %>' />
                                                                    </td>
                                                                    <td align="left" class="ClsLabelL">
                                                                        <asp:Label ID="lblTermName" runat="server" Text='<%# Eval("Term") %>' />
                                                                        <asp:HiddenField ID="hidTermId" runat="server" Value='<%# Eval("TermId") %>' />
                                                                    </td>
                                                                    <td style="padding-right:5px" align="right">
                                                                        <asp:Label ID="lblLength" runat="server" Text='<%# Eval("MaxRemarkLength") %>' />
                                                                        <asp:HiddenField ID="HidStandardwiseRemarkLengthId" runat="server" Value='<%# Eval("StandardwiseRemarkLengthId") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="UpdateCommand" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete %>"
                                                                            TabIndex="6" CausesValidation="false" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                    </td>
                                                                </tr>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="LblNoRecord" align="center">
                                                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"
                                                                                EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnBack" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Back%>"
                                        runat="server" TabIndex="20" CausesValidation="false" />
                                    <asp:HiddenField ID="hidConfigId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                    <asp:HiddenField ID="hidValTimeSpan" runat="server" />
                                    <asp:HiddenField ID="hidValBlankTimeSpan" runat="server" />
                                    <asp:HiddenField ID="hidAlertDeleteUser" runat="server" />
                                    <asp:HiddenField ID="hidMaxRemarkLength" runat="server" />
                                    <asp:HiddenField ID="hidProgressRemarkLengthAlertMessage" runat="server" />
                                    <asp:HiddenField ID="hidRemarkExistErrorMessage" runat="server" />
                                    <asp:HiddenField ID="hidRemarkLength" runat="server" />
                                    <asp:HiddenField ID="hidRemarkLengthCondition" runat="server" />
                                </td>
                            </tr>
                            <tr id="trPrecondition" runat="server" visible="false">
                                <td align="left">
                                    <div runat="server" id="divErr">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        _clientcmbTerm = "<%=this.cmbTerm.ClientID %>";
        _clientcmbStandard = "<%=this.cmbStandard.ClientID %>";
        _clientListViewId = "<%=this.lstvwRemarkLengthConfiguration.ClientID %>";
        _clienthidConfigId = "<%=this.hidConfigId.ClientID %>";
        _clientlblMessage = "<%=this.lblUpdateMessage.ClientID %>";

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidAlertDeleteUser.ClientID %>").value)) {
                bResult = false
            }
            return bResult
        }

        function ValidateLength(oSrc, args) {
            var Length = $get("<%= this.txtRemarkLength.ClientID %>");
            if (Length != null && Length.value == "") {
                oSrc.errormessage = $get("<%=this.hidRemarkLengthCondition.ClientID %>").value;
                args.IsValid = false;
                return false;
            }
            else if (Length != null && parseInt(Length.value.trim()) == 0) {
                oSrc.errormessage = $get("<%=this.hidRemarkLength.ClientID %>").value;
                args.IsValid = false;
                return false;
            }
            args.IsValid = true;
            return true;
        }

        function CheckDuplication(oSrc, args) {
            var iRowCount = 0;
            var IsFound = false;
            var SelectStandard = document.getElementById(_clientcmbStandard)
            var Term = document.getElementById(_clientcmbTerm)
            var EditedId = document.getElementById(_clienthidConfigId)          
            var IsFound = false;
            var StandardInList = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_HidStandardId")
            var ConfigId = $get(_clientListViewId + "_ctrl" + iRowCount + "_HidStandardwiseRemarkLengthId");
            while (StandardInList != null) {
                if (StandardInList.value == SelectStandard.value) {
                    var TermInList = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_hidTermId")
                    if (EditedId.value != ConfigId.value) {
                        if (TermInList.value == Term.value) {
                            IsFound = true;
                            break;
                        }
                    }
                }
                iRowCount = iRowCount + 1;
                StandardInList = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_HidStandardId")
                ConfigId = $get(_clientListViewId + "_ctrl" + iRowCount + "_HidStandardwiseRemarkLengthId");
            }

            if (IsFound) {

                oSrc.errormessage = $get("<%= this.hidRemarkExistErrorMessage.ClientID %>").value;
                args.IsValid = false;
                return false;
            }
            args.IsValid = true;
            return true
        }

        function ValidRemarkLength() {
            document.getElementById(_clientlblMessage).innerHTML = "";
            var iRemarkLength = $get("<%= this.txtRemarkLength.ClientID %>");
            var iMaxRemarkLength = $get("<%= this.hidMaxRemarkLength.ClientID %>");
            var bResult = true           
            if (parseInt(iMaxRemarkLength.value) > parseInt(iRemarkLength.value)) {
                if ($get(_clienthidConfigId).value > 0) {
                    var sAlertMessage = ($get("ctl00_MainBody_hidProgressRemarkLengthAlertMessage").value).replace("MaxRemarkLength", iMaxRemarkLength.value)
                    sAlertMessage = sAlertMessage.replace("MaxRemarkLength", iMaxRemarkLength.value);

                    if (!window.confirm(sAlertMessage)) {
                        bResult = false
                        Page_IsValid = false;
                    }
                }

            } return bResult
        }       
        
    </script>
</asp:Content>
