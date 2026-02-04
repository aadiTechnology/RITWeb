<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="BasicLeaveConfigPopup.aspx.cs" Inherits="BasicLeaveConfigPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="left">
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="center" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">Basic Leave Configuration</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                </td>
            </tr>
            <tr id="trDetails" runat="server">
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="upnl1" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="1" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                                </td>
                                                <td width="50%">
                                                    <div style="float: right;">
                                                        <span class="ClsMdtStar">* Mandatory Fields </span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="95%">
                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                                        Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Staff Group :</span>
                                                </td>
                                                <td align="left" width="180px">
                                                    <asp:DropDownList ID="cmbStaffGroup" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqStaffGroup" runat="server" Display="None" ControlToValidate="cmbStaffGroup"
                                                        InitialValue="0" ErrorMessage="Please select Staff Group."></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" width="80px" class="ClsBorderlight">
                                                    <span class="ClsLabel">Month :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbMonth" runat="server" CssClass="MidCombo">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkAccumulateLeave" runat="server" AutoPostBack="true" Text="Accumulate Leaves?"
                                                        OnCheckedChanged="chkAccumulateLeave_CheckedChanged" />
                                                </td>                                                
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hidConfigId" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwConfiguredLeaves" runat="server" DataKeyNames="LeaveId,Id"
                                            OnItemDataBound="lstvwConfiguredLeaves_ItemDataBound">
                                            <LayoutTemplate>
                                                <table width="95%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader">
                                                        <th align="left" width="50%" class="ClsLabelL">
                                                            Leave Name
                                                        </th>
                                                        <th align="right" class="clsLabelgrd">
                                                            Basic Leaves
                                                        </th>
                                                        <th align="right" class="ClsLabelR">
                                                            Accumulate Leaves
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:Label ID="lblLeaveName" runat="server" CssClass="ClsLabel" Text='<%#Eval("LeaveName") %>'></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txtBasicLeave" runat="server" Text='<%#Eval("BasicLeaves") %>' CssClass="SmlTxtBox"
                                                            Style="text-align: right; padding-right: 2px;" onblur="extractNumber(this,1,false);"
                                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                            onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                        <asp:HiddenField ID="hidBasicLeave" runat="server" Value='<%#Eval("BasicLeaves") %>' />
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txtAccLeave" runat="server" Text='<%#Eval("AccumulateLeaves") %>'
                                                            CssClass="SmlTxtBox" Style="text-align: right; padding-right: 2px;" onblur="extractNumber(this,1,false);"
                                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                            onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                        <asp:HiddenField ID="hidAccLeave" runat="server" Value='<%#Eval("AccumulateLeaves") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:Label ID="lblLeaveName" runat="server" CssClass="ClsLabel" Text='<%#Eval("LeaveName") %>'></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txtBasicLeave" runat="server" Text='<%#Eval("BasicLeaves") %>' CssClass="SmlTxtBox"
                                                            Style="text-align: right; padding-right: 2px;" onblur="extractNumber(this,1,false);"
                                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                            onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                        <asp:HiddenField ID="hidBasicLeave" runat="server" Value='<%#Eval("BasicLeaves") %>' />
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txtAccLeave" runat="server" Text='<%#Eval("AccumulateLeaves") %>'
                                                            CssClass="SmlTxtBox" Style="text-align: right; padding-right: 2px;" onblur="extractNumber(this,1,false);"
                                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                            onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                        <asp:HiddenField ID="hidAccLeave" runat="server" Value='<%#Eval("AccumulateLeaves") %>' />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="30%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button CssClass="ClsBtn" ID="BtnSave" runat="server" Text="Save" UseSubmitBehavior="false"
                                                        OnClick="BtnSave_Click" />
                                                </td>
                                                <td align="left">
                                                    <asp:Button CssClass="ClsBtn" ID="BtnCancel" CausesValidation="false" runat="server"
                                                        Text="Cancel" OnClick="BtnCancel_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr style="border: thin solid #C0C0C0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="100px">
                                                    <span class="ClsLabel">Staff Group :</span>
                                                </td>
                                                <td align="left" width="180px">
                                                    <asp:DropDownList ID="cmbConfigStaffGroups" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbConfigStaffGroups_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>                                                
                                                <td>
                                                    <asp:Button CssClass="ClsBtn" Style="width: 200px;" ID="btnApplyToAll" runat="server"
                                                        CausesValidation="false" Visible="false" Text="Apply to all users of Staff Group"
                                                        UseSubmitBehavior="false" OnClick="btnApplyToAll_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwBasicLeaveDetails" runat="server" DataKeyNames="Id" OnItemCommand="lstvwBasicLeaveDetails_ItemCommand"
                                            OnItemEditing="lstvwBasicLeaveDetails_ItemEditing" OnItemDataBound="lstvwBasicLeaveDetails_ItemDataBound"
                                            OnItemDeleting="lstvwBasicLeaveDetails_ItemDeleting">
                                            <LayoutTemplate>
                                                <table width="95%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader">
                                                        <th align="left" width="100px" class="ClsLabelL">
                                                            Staff Group
                                                        </th>
                                                        <th align="left" width="100px" class="clsLabelgrd">
                                                            Month
                                                        </th>
                                                        <th align="center" width="150px">
                                                            Accumulate Leaves?
                                                        </th>
                                                        <th width="50px" align="center">
                                                            Edit
                                                        </th>
                                                        <th width="50px">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:Label ID="lblStaffGroups" runat="server" CssClass="ClsLabel" Text='<%#Eval("StaffGroupsName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblMonth" runat="server" CssClass="ClsLabel" Text='<%#Eval("Month") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                            Visible='<%#Convert.ToBoolean(Eval("IsAccumulationMonth")) %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="EDIT"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="DELETE"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="Delete" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:Label ID="lblStaffGroups" runat="server" CssClass="ClsLabel" Text='<%#Eval("StaffGroupsName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblMonth" runat="server" CssClass="ClsLabel" Text='<%#Eval("Month") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                            Visible='<%#Convert.ToBoolean(Eval("IsAccumulationMonth")) %>' />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="EDIT"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="DELETE"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="Delete" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr id="tr1" runat="server">
                                    <td style="height: 10px;">
                                        <table align="center" width="95%" >
                                            <tr >
                                                <td align="left" class="ClsBorderlight " width="50px" style="background-color: #ffffc4;">
                                                    <span class="LblNrmlB" style="font-weight: bold;">Note :</span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                    <span class="LblSmlV">Leaves will be displayed on Year wise Leaves Configuration screen only when configuration is applied to all users.</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divErr" runat="server">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="30%">
                        <tr>
                            <td align="center">
                                <asp:Button CssClass="ClsBtn" ID="btnClose" CausesValidation="false" runat="server"
                                    OnClientClick="ClosePopup()" Text="Close" />
                                <asp:HiddenField ID="hidUpdateExisting" runat="server" Value="N" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clientlstvwConfiguredLeaves = "<%=this.lstvwConfiguredLeaves.ClientID %>"
        _clietnhidUpdateExisting = "<%=this.hidUpdateExisting.ClientID %>"
        _clientchkAccumulateLeave = "<%=this.chkAccumulateLeave.ClientID %>"
        _clientcmbStaffGroup = "<%=this.cmbStaffGroup.ClientID %>"

        function ClosePopup() {
            window.close();
        }

        function CheckValue(obj, iRowIndex) {
            var hidField = document.getElementById(_clientlstvwConfiguredLeaves + "_ctrl" + iRowIndex + "_hidBasicLeave")
            if (obj.value.trim() == "")
                obj.value = hidField.value;
            else {
                hidField.value = obj.value;
                var floatValue = parseFloat(obj.value)
                var intValue = parseInt(obj.value)

                if (floatValue < 1)
                    intValue = 0

                intValue = parseFloat(intValue)
                var difference = parseFloat((floatValue * 10) % 10)
                if (difference != 5 && difference != 0) {
                    if (difference > 5)
                        difference = intValue + 1
                    else
                        difference = intValue + 0.5

                    obj.value = difference
                    hidField.value = obj.value;
                }
            }
        }

        function ConfirmUpdateExisting() {

            if (!confirm('While applying the basic leave configuration to all of the users, please note the following:\n\n' +
                '1. The configuration will be applied to the staff member as per the configured count for each leave of the staff group. Please check the count is accurate before proceeding.\n\n' +
                '2. The basic leaves are calculated for user as per the permanent date. If permanent date is not available, staff member is considered as temporary and basic leaves will be configured as zero. Please make sure the permanent date for each user is correct.\n\n' +
                'Are you sure you want to continue?'))
                return false;

            if (confirm("Do you want to overwrite existing configuration of staff?"))
                $get(_clietnhidUpdateExisting).value = "Y"
            else
                $get(_clietnhidUpdateExisting).value = "N"
            return true;
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this configuration?');
        }

        function ResetFields() {
            $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
        }

    </script>
</asp:Content>
