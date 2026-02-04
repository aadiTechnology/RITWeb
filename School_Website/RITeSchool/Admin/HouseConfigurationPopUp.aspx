<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="HouseConfigurationPopUp.aspx.cs" Inherits="HouseConfigurationPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" style="width: 100%;">
            <tr>
                <td align="left" rowspan="1" style="height: 20px">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <span class="MainTitleHead">House Standard Assignment.</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ShowSummary="true" />
                            <asp:CustomValidator ID="cstStandardConfig" runat="server" ErrorMessage="" ClientValidationFunction="ValidateHouseCheck"
                                Display="None"></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server" align="center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListView ID="lstvwConfigureStandards" runat="server" DataKeyNames="StandardId">
                                <LayoutTemplate>
                                    <table id="lstvwPayFee" width="60%" style="color: #333" cellpadding="3" cellspacing="1"
                                        class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="left" style="padding-left: 10px;" width="150px">
                                                <asp:Label ID="lblStandardName" runat="server" Text="Standard" CausesValidation="false"
                                                    ForeColor="Black"> </asp:Label>
                                            </th>
                                            <th id="thSelectAll" runat="server" align="center" style="padding: 0;">
                                                <asp:CheckBox ID="chkStandardHouse" Text="Is House Config. Applicable?" runat="server"
                                                    Style="font-weight: bold;" onclick="CheckAllUncheckAlls()" />
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr class="ClsGridRow">
                                        <td align="left">
                                            <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>'
                                                CssClass="ClspaddingL" />
                                        </td>
                                        <td align="center" id="tdchkPay" runat="server">
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("AllowHouseConfiguration") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="ClsGridAltRow">
                                        <td align="left">
                                            <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("StandardName") %>'
                                                CssClass="ClspaddingL" />
                                        </td>
                                        <td align="center" id="tdchkPay" runat="server">
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("AllowHouseConfiguration") %>' />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                OnClick="btnSave_Click" />
                            <asp:Button ID="btnClose" CssClass="ClsBtn" runat="server" Text="Close" CausesValidation="False"
                                OnClientClick="ClosePopup(); return false;" /><br />
                            <asp:HiddenField ID="hidHouseConfigureId" runat="server" Value="0" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        function CheckAllUncheckAlls() {
            var checkAll = $("[id$=chkStandardHouse]").attr('checked');
            if (checkAll)
                $("[id$=chkSelect]").attr('checked', checkAll);
            else
                $("[id$=chkSelect]").removeAttr('checked');
        }
        $(function () {
            $("[id$=chkSelect]").click(function () {
                if ($("[id$=chkSelect]").length == $("[id$=chkSelect]:checked").length)
                    $("[id$=chkStandardHouse]").attr('checked', "checked");
                else $("[id$=chkStandardHouse]").removeAttr("checked");
            });

            CheckHeaderCheckboxAtPageLoad();
        });

        function CheckHeaderCheckboxAtPageLoad() {
            if ($("[id$=chkSelect]").length == $("[id$=chkSelect]:checked").length)
                $("[id$=chkStandardHouse]").attr('checked', "checked");
            else $("[id$=chkStandardHouse]").removeAttr("checked");
        }

        function ValidateHouseCheck(oSrc, args) {
            var cnt = $("[id$=chkSelect]:checked").length;
            if (cnt == 0) {
                oSrc.errormessage = "At least one standard should be selected.";
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }
        function ClosePopup() {
            window.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
