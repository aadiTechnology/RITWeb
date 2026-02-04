<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="GenerateChallanPopUp.aspx.cs" Inherits="GenerateChallanPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
            vertical-align: top">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td style="height: 19px" align="left" colspan="6" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                    <tr>
                                        <td class="ClsGrayMainTitle" style="height: 20px; width: 99%;">
                                            <span class="MainTitleHead" style="font-weight: bold">Generate Challan</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <span class="ClsMdtStar">* Mandatory Fields</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;">
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <table style="width: 70%; text-align: center; margin: 0px auto;" align="center">
                        <tr>
                            <td align="left" style="text-align:left; font-size:11px;" colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" />
                                        <asp:RequiredFieldValidator ID="reqAcademicYear" runat="server" Display="None" ErrorMessage="Academic Year should be selected."
                                            ControlToValidate="cmbAcademicYear" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqFeeType" runat="server" Display="None" ErrorMessage="Fee type should be selected."
                                            ControlToValidate="cmbFeeType" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqPayableFor" runat="server" Display="None" ErrorMessage="Payable for should be selected."
                                            ControlToValidate="cmbPayableFor" InitialValue="0"></asp:RequiredFieldValidator>
                                    </ContentTemplate>                                  
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;" class="ClsBorderlight">
                                <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="Academic Year"
                                    Height="16px"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <asp:DropDownList ID="cmbAcademicYear" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                    OnSelectedIndexChanged="cmbAcademicYear_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">* </span>
                                <%-- </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnDisplayChallan" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;" class="ClsBorderlight">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Fee Type" Height="16px"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <asp:DropDownList ID="cmbFeeType" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                    OnSelectedIndexChanged="cmbFeeType_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="-- Select --"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="ClsMdtStar">* </span>
                                <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbAcademicYear" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnDisplayChallan" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;" class="ClsBorderlight">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Payable For" Height="16px"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <asp:DropDownList ID="cmbPayableFor" runat="server" CssClass="MidCombo">
                                    <asp:ListItem Value="0" Text="-- Select --"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="ClsMdtStar">* </span>
                                <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbFeeType" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnDisplayChallan" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="text-align: center;">
                                <asp:Button ID="btnDisplayChallan" CssClass="ClsBtn" runat="server" Text="Generate"
                                    OnClick="btnDisplayChallan_Click" />
                                <asp:Button ID="btnClose" CssClass="ClsBtn" runat="server" Text="Close" ViewStateMode="Enabled"
                                    OnClientClick="ClosePopup(); return false;" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:HiddenField ID="hidStudentId" Value="0" runat="server" />
                                <asp:HiddenField ID="hidStandardId" Value="0" runat="server" />
                                <asp:HiddenField ID="hidSchoolwiseStudentId" Value="0" runat="server" />
                                <asp:HiddenField ID="hidStandardDivisionId" Value="0" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        function ClosePopup() {
            window.close();
        }

    </script>
</asp:Content>
