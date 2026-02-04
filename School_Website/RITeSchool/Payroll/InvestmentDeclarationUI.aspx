<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="InvestmentDeclarationUI.aspx.cs" Inherits="InvestmentDeclarationUI" %>

<%@ Register Src="~/UserControls/InvestmentDeclarationsUC.ascx" TagPrefix="UCL"
    TagName="InvestmentDeclarations" %>
<%@ Register Src="~/UserControls/IncomeDeclarationUC.ascx" TagPrefix="UCL"
    TagName="IncomeDeclarations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr id="trInvestmentDetails" runat="server">
                <td>
                    <table width="97%">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                        </td>
                                        <td align="right" width="150px">
                                            <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trInvMethod" runat="server">
                            <td align="right">
                                <table width="80%">
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="left" class="ClsGreenBG" width="190px" style="white-space:nowrap">
                                            <asp:HyperLink ID="lnkInvestmentMethods" runat="server" Text="Investment / Income Methods"
                                                NavigateUrl="~/RITeSchool/Payroll/InvestmentMethodUI.aspx" CssClass="SubTitle"
                                                Style="text-align: left;"></asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trPublishMessage" runat="server" visible="false">
                            <td align="center" width="100%" class="ClsHilightBGB">
                               <span class="LblNrmlB" style="border-width:0px;font-weight:bold;">Income tax details of this financial year has been published.</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="tdMessage" runat="server">
                                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                    Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table style="white-space:nowrap" width="400px">
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="optInvestment" runat="server" Text="Investment Declarations"
                                                CssClass="ClsLabel" AutoPostBack="true" GroupName="Declarations" OnCheckedChanged="optInvestment_CheckedChanged" />
                                        </td>
                                        <td align="left">
                                            <asp:RadioButton ID="optIncome" runat="server" Text="Income Declarations" CssClass="ClsLabel"
                                                AutoPostBack="true" GroupName="Declarations" OnCheckedChanged="optIncome_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table  style="white-space:nowrap">
                                    <tr>
                                        <td align="left" class="ClsBorderlight" width="100px" id="tdStaffGroup" runat="server" style="white-space:nowrap">
                                            <span class="ClsLabel">Staff Group :</span>
                                        </td>
                                        <td width="160px" id="tdStaffGroupCombo" runat="server" style="white-space:nowrap">
                                            <asp:DropDownList ID="cmbStaffGroup" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" class="ClsBorderlight" width="100px" id="tdUser" runat="server" style="white-space:nowrap">
                                            <span class="ClsLabel">User :</span>
                                        </td>
                                        <td id="tdCmbUser" runat="server" width="220px" style="white-space:nowrap">
                                            <asp:DropDownList ID="cmbUser" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                                OnSelectedIndexChanged="cmbUser_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                            <asp:RequiredFieldValidator ID="reqUser" runat="server" Display="None" ControlToValidate="cmbUser"
                                                CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="User should be selected."></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left" class="ClsBorderlight" width="100px" style="white-space:nowrap">
                                            <span class="ClsLabel">Section :</span>
                                        </td>
                                        <td style="white-space:nowrap">
                                            <asp:DropDownList ID="cmbSection" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                                Width="200px" OnSelectedIndexChanged="cmbSection_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr  style="border: thin solid Gray;" />
                            </td>
                        </tr>
                        <tr id="trRegim" runat="server" visible="false">
                            <td align="center">
                                <table>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            <span class="ClsLabel">Regime :</span>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlRegime" runat="server" AutoPostBack="false" CssClass="MidCombo">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="ReqRegime" runat="server" ControlToValidate="ddlRegime" Enabled="false"
                                                InitialValue="0" Display="None" ViewStateMode="Enabled" ErrorMessage="Regime should be selected.">
                                            </asp:RequiredFieldValidator>
                                            <span id="starspan" class="ClsMdtStar" runat="server" visible="false">*</span>
                                        </td>                                       
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trInvestmentDeclarations" runat="server" visible="false">
                            <td>
                                <UCL:InvestmentDeclarations ID="ucInvestmentDeclarations" runat="server"></UCL:InvestmentDeclarations>
                            </td>
                        </tr>
                        <tr id="trIncomeDeclarations" runat="server" visible="false">
                            <td>
                                <UCL:IncomeDeclarations ID="ucIncomeDeclarations" runat="server"></UCL:IncomeDeclarations>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button CssClass="ClsBtn" ID="btnBack" CausesValidation="false" runat="server"
                                    Text="Back" onclick="btnBack_Click" />
                                <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                            </td>
                        </tr>
                        <asp:HiddenField ID="hidHasFullAccess" runat="server" Value="N" />
                        <asp:HiddenField ID="hidItemCount" runat="server" Value="" OnValueChanged="HidItemCount_ValueChanged" /> 
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clienthidItemCount = "<%=this.hidItemCount.ClientID %>";

        function ResetFields() {
            $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
        }

        function UpdateFileUploadCount(ItemCount) {            
            document.getElementById(_clienthidItemCount).value = ItemCount;
            __doPostBack(document.getElementById(_clienthidItemCount).name, '')
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
