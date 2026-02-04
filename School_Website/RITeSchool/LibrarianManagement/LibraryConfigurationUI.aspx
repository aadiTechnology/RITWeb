<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="LibraryConfigurationUI.aspx.cs" Inherits="LibraryConfigurationUI" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="97%" align="center">
        <tr>
            <td>
                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" />
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:UpdatePanel ID="UPanelErrorlbl" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" Visible="False"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2" class="ClsTextNormal">
                <span class="ClsMdtStar">* Mandatory Fields</span>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:UpdatePanel ID="UPanelLibrarySettings" runat="server" ChildrenAsTriggers="False"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center" width="90%">
                            <tr>
                                <td class="ClsBorderlight" style="width: 25%;">
                                    <%--<asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="User Role :" 
                                        EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">User Role :</span>
                                </td>
                                <td style="width: 25%;">
                                    <asp:DropDownList ID="cmbLibraryConfig" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="cmbLibraryConfig_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="cmpUserRole" runat="server" ControlToValidate="cmbLibraryConfig"
                                        CssClass="ClsLabel" Display="None" ErrorMessage="User role should be selected."
                                        Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator><span class="ClsMdtStar">*</span>
                                </td>
                                <td style="width: 2%" rowspan="4">
                                </td>
                                <td class="ClsBorderlight" style="width: 28%;">
                                    <%--<asp:Label ID="lblReturnDay" runat="server" Text="Issue Period (In days) :" 
                                        CssClass="ClsLabel" EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">Issue Period (In days) :</span>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtReturnDay" runat="server" MaxLength="3" onblur="extractNumber(this,2,false);"
                                        onkeyup="extractNumber(this,2,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="MidTxtBox"></asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                                    <asp:CompareValidator ID="cmpReturnDay" runat="server" CssClass="ClsLabel" Display="None"
                                        ValueToCompare="0" ControlToValidate="txtReturnDay" ErrorMessage="Issue period should not be 0."
                                        Operator="GreaterThan" Type="Integer"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="reqReturnDay" runat="server" ControlToValidate="txtReturnDay"
                                        Display="None" ErrorMessage="Issue period (In days) should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight">
                                    <%--<asp:Label ID="lblAttemptToRenew" runat="server" Text="Renew Attempts :" 
                                        CssClass="ClsLabel" EnableViewState="False"></asp:Label>--%>
                                         <span class="ClsLabel">Renew Attempts :</span>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAttemptToRenew" runat="server" MaxLength="1" onblur="extractNumber(this,2,true);"
                                        onkeyup="extractNumber(this,2,true);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="MidTxtBox"></asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                                    <asp:RequiredFieldValidator ID="reqRenewAttempt" runat="server" ControlToValidate="txtAttemptToRenew"
                                        Display="None" ErrorMessage="Renew attempts should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                </td>
                                <td class="ClsBorderlight">
                                    <%--<asp:Label ID="lblBookPerPerson" runat="server" Text="Books Per Person :" 
                                        CssClass="ClsLabel" EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">Books Per Person :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBookPerPerson" runat="server" MaxLength="2" onblur="extractNumber(this,2,true);"
                                        onkeyup="extractNumber(this,2,true);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="MidTxtBox"></asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                                    <asp:RequiredFieldValidator ID="reqBookPerPerson" runat="server" ControlToValidate="txtBookPerPerson"
                                        Display="None" ErrorMessage="Book per person should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cmpBookPerPerson" runat="server" Display="None" CssClass="ClsLabel"
                                        ValueToCompare="0" ControlToValidate="txtBookPerPerson" ErrorMessage="Book per person should not be 0."
                                        Operator="GreaterThan"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight">
                                    <%--<asp:Label ID="lblLateFee" runat="server" Text="Late Fee Per Day (In Rs.) :" 
                                        CssClass="ClsLabel" EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">Late Fee Per Day (In Rs.) :</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLateFee" runat="server" MaxLength="4" onblur="extractNumber(this,2,true);"
                                        onkeyup="extractNumber(this,2,true);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="MidTxtBox"></asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                                    <asp:RequiredFieldValidator ID="reqLateFee" runat="server" ControlToValidate="txtLateFee"
                                        Display="None" ErrorMessage="Late fee per day (In Rs.) should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                </td>
                                <td class="ClsBorderlight">
                                    <%--<asp:Label ID="lblEffectiveLateFee" runat="server" Text="Late Fee Effective From (Day No.) :"
                                        CssClass="ClsLabel" EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">Late Fee Effective From (Day No.) :</span>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEffectiveLateFee" runat="server" MaxLength="3" onblur="extractNumber(this,2,true);"
                                        onkeyup="extractNumber(this,2,true);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="MidTxtBox"></asp:TextBox>
                                    <span class="ClsMdtStar">*</span>
                                    <asp:RequiredFieldValidator ID="reqEffectiveDays" runat="server" ControlToValidate="txtEffectiveLateFee"
                                        Display="None" ErrorMessage="Late fee effective from (day no.) should not be blank."
                                        CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight">
                                   <span class="ClsLabel">No of Books Allowed to Claim :</span></td>
                                <td>
                                    <asp:TextBox ID="txtReserveBooks" runat="server" CssClass="MidTxtBox" MaxLength="4" 
                                        onblur="extractNumber(this,2,true);" ondrop="event.returnValue=false" 
                                        onkeypress="return blockNonNumbers (this, event, false, false);" 
                                        onkeyup="extractNumber(this,2,true);" onpaste="event.returnValue=false"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    <asp:RequiredFieldValidator ID="reqReserveBooks" runat="server" 
                                        ControlToValidate="txtReserveBooks" CssClass="ClsLabel" Display="None" 
                                        ErrorMessage="No. of books allowed to claim per person should not be blank."></asp:RequiredFieldValidator>
                                </td>
                                <td >
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" style="height: 5px;">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="5" valign="bottom">
                                    <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" OnClick="btnSave_Click" disable-page="true"
                                        Text="Save" />
                                    <asp:Button ID="btnNew" runat="server" CausesValidation="False" CssClass="ClsBtn"
                                        OnClick="btnNew_Click" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="grdvwLibraryConfig" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbLibraryConfig" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:UpdatePanel ID="UPanelGrid" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdvwLibraryConfig" Style="overflow: auto;" runat="server" AutoGenerateColumns="False"
                            Height="100%" PageSize="20" GridLines="None" CellPadding="2" CellSpacing="1"
                            ForeColor="#333333" Width="930px" BackColor="White" EmptyDataText="No Record Found"
                            CssClass="GridBorder" DataKeyNames="Lib_Config_Id" OnRowDataBound="grdvwLibraryConfig_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="User_Role_Name" HeaderText="User Role" HtmlEncode="False">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="30px" />
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Issue Period (Days)" DataField="Return_Days" HtmlEncode="False">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Renew Attempts" DataField="NoOf_Attempt_Renew" HtmlEncode="False">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="40px" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Books Per Person" DataField="No_Of_Book_Per_Person" HtmlEncode="False">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="70px" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Late_Fee_Per_Day" HeaderText="Late Fee Per Day (Rs.)" HtmlEncode="False">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" VerticalAlign="Middle"  />
                                </asp:BoundField>
                                <asp:BoundField DataField="Late_Fee_Effective_From" HeaderText="Late Fee Effective From (Day)"
                                    HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" VerticalAlign="Middle" Width="200px" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Reserve_Books_Per_Person" HeaderText="No of Books Allowed to Claim" HtmlEncode="false" >
                                <HeaderStyle HorizontalAlign="Center" Wrap="false" VerticalAlign="Middle" Width="130px" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <%--<asp:ButtonField ButtonType="Image" CommandName="EDIT_COMMAND" HeaderText="Edit"
                                    ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                </asp:ButtonField>--%>
                            </Columns>
                            <RowStyle CssClass="ClsGridAltRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" BackColor="#E6EEFC" HorizontalAlign="Center" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdvwLibraryConfig" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="height: 35px;" valign="bottom">
                <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="ClsBtn"
                    OnClick="btnBack_Click" Text="Back" ValidationGroup="valGroupSend" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UPnlHidden" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidNewConfig" runat="server" />
                        <asp:HiddenField ID="hidIsConfig" runat="server" />
                        <asp:HiddenField ID="hidLibConfigId" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        _clientLableError = "<%=this.lblErrorMsg.ClientID %>"
        _clientValSum = "<%=this.valSumErrorMsg.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnNew = "<%=this.btnNew.ClientID %>"
        _clientbtnBack = "<%=this.btnBack.ClientID %>"
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_beginRequest(BeginReqHandler)
        prm.add_endRequest(EndReqHandler)
        function ClearText() {
            if (document.getElementById(_clientLableError) != null)
                document.getElementById(_clientLableError).style.display = "none"
            if (document.getElementById(_clientValSum) != null)
                document.getElementById(_clientValSum).style.display = "none"
            return true
        }
        function BeginReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnSave)
                DisableButtons(true)
        }
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnSave)
                DisableButtons(false)
        }
        function DisableButtons(action) {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function' && action)
                isPageValid = Page_ClientValidate()
            if (isPageValid) {
                if (document.getElementById(_clientbtnSave) != null)
                    document.getElementById(_clientbtnSave).disable = action
                if (document.getElementById(_clientbtnNew) != null)
                    document.getElementById(_clientbtnNew).disable = action
                if (document.getElementById(_clientbtnBack) != null)
                    document.getElementById(_clientbtnBack).disable = action
                ClearText()
            } 
        }
    </script>
</asp:Content>
