<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    CodeFile="ResetFeeReceiptPopUp.aspx.cs" Inherits="ResetFeeReceiptPopUp" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            <tr>
                <td align="left" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblAddBankName" runat="server" class="MainTitleHead" Text="Reset Receipt Number"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server" align="left">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" />
                            <asp:RequiredFieldValidator ID="reqDate" runat="server" ControlToValidate="txtStartDate"
                                ErrorMessage="From Date should not be blank." SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstValidateFeeTypes" runat="server" ClientValidationFunction="ValidateFeeTypes"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="regAccountHeader" ControlToValidate="cmbAccountHeader"
                                runat="server" ErrorMessage="Account Header should be selected." Display="None"
                                InitialValue="0"></asp:RequiredFieldValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbFeeType" EventName="SelectedIndexChanged" /> 
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage" Text="" EnableViewState="false" ForeColor="Blue" Font-Bold=true></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbFeeType" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trControls" runat="server">
                <td align="center">
                    <table width="75%">                       
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr id="trFromDate" runat="server">
                                                <td class="ClsBorderlight" width="150px">
                                                    <span class="ClsLabel">From Date : </span>
                                                </td>
                                                <td align="left" class="ClsTextNormal" style="padding-right: 10px; height: 19px;"
                                                    colspan="1">
                                                    <asp:TextBox ID="txtStartDate" CssClass="MidTxtBox" runat="server" AutoPostBack="True"></asp:TextBox>
                                                    <rjs:PopCalendar ID="cal_FormOpenDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                        Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                                    <asp:Label ID="Label5" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                             <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Reset For : </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbResetFor" AutoPostBack="true" runat="server" 
                                                        CssClass="MidCombo" Width="300px" 
                                                        onselectedindexchanged="cmbResetFor_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="School Fee"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Internal Fee"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="External Fee"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span style="font-size: 9pt;" class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr id="trFeeType" runat="server">
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Fee Type&nbsp; : </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbFeeType" OnSelectedIndexChanged="cmbFeeType_SelectedIndexChanged"
                                                        AutoPostBack="true" runat="server" CssClass="MidCombo" Width="300px">
                                                    </asp:DropDownList>
                                                     <span id="spMandatory" runat="server" class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr id="trAccountHeader" runat="server">
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">&nbsp;Account Header : </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbAccountHeader" runat="server" CssClass="MidCombo" Width="300px"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr id="trOrderBy" runat="server">
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Order By : </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbOrderBy" runat="server" CssClass="MidCombo" Width="300px"
                                                        AutoPostBack="True">
                                                        <asp:ListItem Text="Receipt No." Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Paid Date" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="ClsBtn" OnClick="btnReset_Click" />
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="window.close()" />
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    
    <script language="javascript" type="text/javascript">

        _clientlblMessage = "<%=this.lblMessage.ClientID%>"
        _clientcmbResetFor = "<%=this.cmbResetFor.ClientID %>"
        _clientcmbFeeType = "<%=this.cmbFeeType.ClientID %>"

        function ClearMessage() {
            $get(_clientlblMessage).innerHTML = "";
        }

        function ValidateFeeTypes(oSrc, args) {            
            var ResetFor = $get(_clientcmbResetFor).value;
            var FeeTypeId = $get(_clientcmbFeeType).value;

            if (ResetFor == 0 && FeeTypeId == 0) {
                oSrc.errormessage = "Fee Type should be selected.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
        }
    </script>
</asp:Content>
