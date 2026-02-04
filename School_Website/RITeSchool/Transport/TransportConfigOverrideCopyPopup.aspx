<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="TransportConfigOverrideCopyPopup.aspx.cs" Inherits="TransportConfigOverrideCopyPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td class="ClsGrayMainTitle" align="left">
                <span class="MainTitleHead">Copy Transport Override Configuration</span>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:ValidationSummary ID="valSum" runat="server" />
                <asp:RequiredFieldValidator ID="reqValDisplayName" runat="server" ErrorMessage="Display Name should not be blank." ControlToValidate="txtDisplayName" Display="None"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                    ClientValidationFunction="VaidateVehicles"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%" id="trCopyConfig" runat="server">
                    <tr>
                        <td align="right">
                            <span class="ClsMdtStar">* Mandatory fields</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td>
                                        <span class="ClsLblLgnd">Legend : </span>
                                    </td>
                                    <td style="border: 1px solid black; padding-left:5px;padding-right:5px;">
                                        <span style="color: Navy;">Config Already Exist</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td style="width:100px" class="clsBorderLight">
                                        <span class="clsLabel">Display Name : </span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                        
                    <tr>
                        <td align="center">
                            <asp:ListView ID="lstVehicles" runat="server" DataKeyNames="VehicleId,IsAlreadyExist,TransportShiftId,RouteId"
                                OnItemDataBound="lstVehicles_ItemDataBound">
                                <LayoutTemplate>
                                    <table align="center" runat="server" id="tblStopInfo" style="color: #333333" cellpadding="0"
                                        cellspacing="1" class="GridBorder" width="100%">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="center" width="100px">
                                                <input type="checkbox" id="chkAll" onclick="CheckUnCheckAllCopy(this);" />
                                            </th>
                                            <th align="left">
                                                <asp:Label ID="Label3" runat="server" CausesValidation="false" CssClass="clsLabel"
                                                    EnableViewState="false">Route No.</asp:Label>
                                            </th>
                                            <th align="left" width="100px">
                                                <asp:Label ID="LinkButton1" runat="server" CausesValidation="false" CssClass="clsLabel"
                                                    EnableViewState="false">Vehicle Number</asp:Label>
                                            </th>
                                            <th align="left" width="100px">
                                                <asp:Label ID="Label1" runat="server" CausesValidation="false" CssClass="clsLabel"
                                                    EnableViewState="false">Journey</asp:Label>
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <td align="center">
                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="CheckHeader();" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label4" runat="server" CssClass="clsLabel" Text='<%# Eval("RouteName") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblVehicleNo" runat="server" CssClass="clsLabel" Text='<%# Eval("VehicleNumber") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label2" runat="server" CssClass="clsLabel" Text='<%# Eval("TransportShiftName") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <td align="center">
                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="CheckHeader();" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label4" runat="server" CssClass="clsLabel" Text='<%# Eval("RouteName") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblVehicleNo" runat="server" CssClass="clsLabel" Text='<%# Eval("VehicleNumber") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label2" runat="server" CssClass="clsLabel" Text='<%# Eval("TransportShiftName") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="ClsBtn" OnClick="btnCopy_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="window.close();"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        function CheckUnCheckAllCopy(obj) {
            if (obj.checked)
                $('[id*=_chkSelect]').attr('checked', 'checked')
            else
                $('[id*=_chkSelect]').removeAttr('checked')
        }

        function CheckHeader() {
            if ($('[id$=_chkSelect]').length == $('[id$=_chkSelect]:checked').length)
                $('[id=chkAll]').attr('checked', 'checked')
            else
                $('[id=chkAll]').removeAttr('checked')
        }

        function VaidateVehicles(src, args) {
            if ($('[id$=_chkSelect]:checked').length > 0) {
                args.IsValid = true;
                return false;
            }
            else {
                src.errormessage = 'At least one vehicle number should be selected to copy configuration.'
                args.IsValid = false;
                return true;
            }
        }

    </script>
</asp:Content>
