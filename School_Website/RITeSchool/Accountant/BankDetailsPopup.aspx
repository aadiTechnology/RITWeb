<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankDetailsPopup.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" Inherits="BankDetailsPopup" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            <tr>
                <td align="left" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                               <asp:Label ID="lblAddBankName" runat="server" class="MainTitleHead" Text="<%$ Resources:LocalizedResources, AddBankName%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label id="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" runat="server" />
                    <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtBankName"
                        ErrorMessage="<%$ Resources:LocalizedResources, BankNameShouldNotBeBlank%>" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdBanks" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" Text="" EnableViewState="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdBanks" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="80%" align="center">
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" cellpadding="1" cellspacing="2">
                                            <tr>
                                                <td valign="middle" class="ClsBorderlight">
                                                    <asp:Label ID="lblBankName" runat="server" class="ClsLabel" style="height: 16px" Text="<%$ Resources:LocalizedResources, BankName%>"></asp:Label>
                                                    <span class="colonPadding ClsLabel">:</span>
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="LrgTxtBox" MaxLength="50"
                                                        TabIndex="1" Width="300px"></asp:TextBox>&nbsp;
                                                    <asp:HiddenField ID="hidBankId" runat="server" />
                                                    <span class="ClsMdtStar">*</span>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn" TabIndex="2"
                                                        OnClick="btnSave_Click"  disable-page="true" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" TabIndex="3"
                                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="grdBanks" runat="server" Width="100%" AutoGenerateColumns="False"
                                            PageSize="20" OnRowCommand="grdBanks_RowCommand" CellPadding="0" CellSpacing="1"
                                            OnRowDataBound="grdBanks_RowDataBound" ForeColor="#333333" GridLines="None" BackColor="White"
                                            DataKeyNames="Schoolwise_Bank_Id,Count" CssClass="GridBorder" TabIndex="4">
                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                            </PagerStyle>
                                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>" PreviousPageText="<%$ Resources:LocalizedResources, Previous%>"
                                                FirstPageText="<%$ Resources:LocalizedResources, First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                            <Columns>
                                                <asp:BoundField HeaderText="<%$ Resources:LocalizedResources, BankName%>" DataField="Bank_Name">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingL"/>
                                                </asp:BoundField>
                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                    HeaderText="<%$ Resources:LocalizedResources, Edit%>" Text="Edit" CommandName="EDIT_BANK">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                </asp:ButtonField>
                                                <asp:ButtonField ButtonType="Image" CommandName="DELETE_BANK" HeaderText="<%$ Resources:LocalizedResources, Delete%>"
                                                    Text="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                </asp:ButtonField>
                                            </Columns>
                                            <RowStyle CssClass="ClsGridRow" />
                                            <HeaderStyle CssClass="ClsGridHeader" />
                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="grdBanks" EventName="RowCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnClose" Text="<%$ Resources:LocalizedResources, Close%>" CssClass="ClsBtn" runat="server" CausesValidation="false"
                                    OnClick="btnClose_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hidSave" runat="server" />
    <asp:HiddenField ID="hidCultureInfo" runat = "server" />
    <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisBank" runat="server" />
    <script language="javascript" type="text/javascript">

        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clientlblErrorMsgId = "<%=this.lblErrorMsg.ClientID %>"
        _clientvalSumErrorMsgId = "<%=this.valSumErrorMsg.ClientID %>"
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=hidAreYouSureYouWantToDeleteThisBank.ClientID%>").value)) {
                bResult = false
            }
            return bResult
        }
        function ClearErrorLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            if (isPageValid) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            }
        }
        function ClearErrorLabel() {
            if (document.getElementById(_clientbtnSave) != null)
                document.getElementById(_clientlblErrorMsgId).style.display = "none"
            if (document.getElementById(_clientvalSumErrorMsgId) != null)
                document.getElementById(_clientvalSumErrorMsgId).style.display = "none"
            return true
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }

    </script>
</asp:Content>
