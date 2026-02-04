<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ITCommissionerInfoPopup.aspx.cs"
    MasterPageFile="../MasterPages/PopupMasterSml.master" Inherits="ITCommissionerInfoPopup" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td>
                                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True"> Income Tax Configuration </asp:Label>
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
				<asp:UpdatePanel runat="server" id="upnl1">
				<ContentTemplate>

                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="Please fix following error(s)" ValidationGroup="CITConfiguration"
                        runat="server" />
						</ContentTemplate>
						</asp:UpdatePanel>
                </td>
            </tr>
			 <tr>
                <td align="left" colspan="2">
				<asp:UpdatePanel runat="server" id="upnl2">
				<ContentTemplate>
                    <asp:ValidationSummary ID="ValidationSummary2" HeaderText="Please fix following error(s)" ValidationGroup="Quarters"
                        runat="server" />
						</ContentTemplate>
						</asp:UpdatePanel>
                </td>
            </tr>
             <tr>
                <td align="left" colspan="2">
				<asp:UpdatePanel runat="server" id="upnl3">
				<ContentTemplate>
                    <asp:ValidationSummary ID="ValidationSummary3" HeaderText="Please fix following error(s)" ValidationGroup="Deductor"
                        runat="server" />
						</ContentTemplate>
						</asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trPublishMessage" runat="server" visible="false">
                <td align="center" width="100%" class="ClsHilightBGB">
                    <span class="LblNrmlB" style="border-width: 0px; font-weight: bold;">Income tax details
                        of this financial year has been published.</span>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdMessage" runat="server" width="100%">
				<asp:UpdatePanel runat="server" ID="upnlLabelMessage">
				<ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                        Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
				</ContentTemplate>
				</asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
				<asp:UpdatePanel runat="server" ID="upnlCITConfiguration" UpdateMode="Conditional">
				<ContentTemplate>
                    <cc1:CollapsablePanel ID="colpnlSubjectSchedule" runat="server" TitleText="CIT Configuration"
                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" 
                        TitleStyle-Height="25px" Collapsed="false" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                        <asp:Panel ID="pnlFields" runat="server" Width="100%">
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <table cellpadding="1" cellspacing="1" runat="server" id="tblHeading" visible="True">
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="100px">
                                                    <span class="ClsLabel">Address :</span>
                                                </td>
                                                <td class="ClsMdtStar" align="left" style="white-space: nowrap">
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
                                                        MaxLength="100" TabIndex="1" Height="60px" Width="250px"></asp:TextBox>
                                                    <span style="color: red">* </span>
                                                    <asp:RequiredFieldValidator ID="reqAddress" runat="server" CssClass="ClsMdtStar"
                                                        ErrorMessage="Address should not be blank." Display="None" ControlToValidate="txtAddress" ValidationGroup="CITConfiguration"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator Display="None" CssClass="ClsMdtStar" ErrorMessage="" ID="cstAddress"
                                                        runat="server" ControlToValidate="txtAddress" ClientValidationFunction="ValidateAddress" ValidationGroup="CITConfiguration"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">City :</span>
                                                </td>
                                                <td class="ClsMdtStar" align="left">
                                                    <asp:TextBox ID="txtCity" runat="server" TabIndex="2" MaxLength="30" CssClass="LrgTxtBox"></asp:TextBox>
                                                    <span style="color: red">* </span>
                                                    <asp:RequiredFieldValidator ID="reqCity" runat="server" CssClass="ClsMdtStar" ErrorMessage="City should not be blank."
                                                        Display="None" ControlToValidate="txtCity" ValidationGroup="CITConfiguration"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Pincode :</span>
                                                </td>
                                                <td class="ClsMdtStar" align="left">
                                                    <asp:TextBox ID="txtPincode" CssClass="LrgTxtBox" runat="server" MaxLength="6" onblur="extractNumber(this,0,false);"
                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" TabIndex="3" />
                                                    <span style="color: red">* </span>
                                                    <asp:RequiredFieldValidator ID="reqPincode" runat="server" CssClass="ClsMdtStar"
                                                        ErrorMessage="Pincode should not be blank." Display="None" ValidationGroup="CITConfiguration" ControlToValidate="txtPincode"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator Display="None" CssClass="ClsMdtStar" ErrorMessage="" ID="cstPincode"
                                                        runat="server" ControlToValidate="txtPincode" ClientValidationFunction="ValidatePincode" ValidationGroup="CITConfiguration"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="Save" BorderWidth="1px" ValidationGroup="CITConfiguration"
                                            UseSubmitBehavior="false" TabIndex="4" OnClick="btnSave_Click" OnClientClick="ClearLabel()" />
                                        &nbsp;<asp:Button ID="btnClear" CssClass="ClsBtn" runat="server" CausesValidation="False" OnClientClick="ClearCITFields()"
                                            Text="Clear" BorderWidth="1px" UseSubmitBehavior="false" TabIndex="5" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </cc1:CollapsablePanel>
					</ContentTemplate>
					<Triggers>                        
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click"/>
                    </Triggers>
					</asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
				<asp:UpdatePanel runat="server" ID="upnlQuarterDetails" UpdateMode="Always">
				<ContentTemplate>
                    <cc1:CollapsablePanel ID="CollapsablePanel1" runat="server" TitleText="Quarter Details" 
                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" 
                        TitleStyle-Height="25px" Collapsed="True" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                        <asp:Panel ID="Panel1" runat="server" Width="100%">
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwQuarters" runat="server" DataKeyNames="Id">
                                            <LayoutTemplate>
                                                <table align="center" width="380px" runat="server" id="tblQuarters" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" style="width: 100px; padding-left: 5px;">
                                                            Quarter
                                                        </th>
                                                        <th align="left" style="padding-left: 5px;">
                                                            Receipt Number
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                    <td align="left" style="padding-left: 5px">
                                                        <asp:Label ID="lblQuarter" runat="server" Text='<%# Eval("Name") %>' />
                                                    </td>
                                                    <td align="left" style="padding-left: 5px">
                                                        <asp:TextBox ID="txtReceiptNumber" runat="server" CssClass="LrgTxtBox" Width="270px" TabIndex="6"
                                                            MaxLength="30" onkeypress="return PreventSpecialChars(event);" Text='<%# Eval("ReceiptNumber") %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label runat="server" ID="lblNoRecord" CssClass="LblNoRecord" Text="No Record found"
                                                            Width="50%" />
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                    <td align="left" style="padding-left: 5px">
                                                        <asp:Label ID="lblQuarter" runat="server" Text='<%# Eval("Name") %>' />
                                                    </td>
                                                    <td align="left" style="padding-left: 5px">
                                                        <asp:TextBox ID="txtReceiptNumber" runat="server" CssClass="LrgTxtBox" Width="270px" TabIndex="6"
                                                            MaxLength="30" onkeypress="return PreventSpecialChars(event);" Text='<%# Eval("ReceiptNumber") %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                        <asp:HiddenField ID="hidIsPublished" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSaveQurters" CssClass="ClsBtn" runat="server" Text="Save" BorderWidth="1px" ValidationGroup="Quarters"
                                            UseSubmitBehavior="false" TabIndex="7" OnClick="btnSaveQurters_Click"  />
											<asp:CustomValidator Display="None" CssClass="ClsMdtStar" ErrorMessage="Receipt number for atleast one quarter should be enter." ID="cstQuarters"
                                                        runat="server"  ClientValidationFunction="ValidateReceiptNumber" ValidationGroup="Quarters"></asp:CustomValidator>
                                    </td>                                   
                                </tr>
                            </table>
                        </asp:Panel>
                    </cc1:CollapsablePanel>
					</ContentTemplate>
					<Triggers>                        
                        <asp:AsyncPostBackTrigger ControlID="btnSaveQurters" EventName="Click" />
                    </Triggers>
					</asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
				<asp:UpdatePanel runat="server" ID="upnlDeductorPersonDetails" >
				<ContentTemplate>
                    <cc1:CollapsablePanel ID="CollapsablePanel2" runat="server" TitleText="Deductor Person Details"
                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left"
                        TitleStyle-Height="25px" Collapsed="True" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                        <asp:Panel ID="Panel2" runat="server" Width="100%">
                            <table width="60%" align="center">
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <span class="ClsLabel">Deductor Name :</span>
                                    </td>
                                    <td class="ClsMdtStar" align="left" style="white-space: nowrap">
                                        <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" Width="50px"
                                            TabIndex="8">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" Width="186px" onblur="formatName(this)"
                                            CssClass="MidTxtBox" TabIndex="9"></asp:TextBox>
                                        *&nbsp;
                                        <asp:RequiredFieldValidator ID="rqdFirstName" runat="server" ControlToValidate="txtFirstName" ValidationGroup="Deductor"
                                            Display="None" ErrorMessage="Deductor Name should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <span class="ClsLabel">Deductor Father Name :</span>
                                    </td>
                                    <td class="ClsMdtStar" align="left" style="white-space: nowrap">
                                        <asp:TextBox ID="txtFatherName" runat="server" MaxLength="50" TabIndex="10" Width="235px" onblur="formatName(this)"
                                            CssClass="MidTxtBox"></asp:TextBox>
                                        *&nbsp;
                                        <asp:RequiredFieldValidator ID="reqFatherName" runat="server" ControlToValidate="txtFatherName" ValidationGroup="Deductor"
                                            Display="None" ErrorMessage="Deductor Father Name should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>                                    
                                    <td align="left" class="ClsBorderlight">
                                        <span class="ClsLabel">Designation :</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbDesignations" runat="server" CssClass="MidCombo" Style="width: 190px;"
                                            TabIndex="11">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:RequiredFieldValidator ID="reqDesignations" runat="server" Display="None" ErrorMessage="Designation should be selected." ValidationGroup="Deductor"
                                            ControlToValidate="cmbDesignations" InitialValue="0" CssClass="ClsMdtStar"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <table align="center">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSaveDeductor" runat="server" BorderWidth="1px" CssClass="ClsBtn" ValidationGroup="Deductor" 
                                            TabIndex="12" Text="Save" UseSubmitBehavior="false" OnClick="btnSaveDeductor_Click" />
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnClearDeductor" runat="server" BorderWidth="1px" CssClass="ClsBtn" OnClientClick="ClearFields()" CausesValidation="False"
                                            TabIndex="13" Text="Clear" UseSubmitBehavior="false"/>
                                    </td>
                                    <asp:HiddenField ID="hidDeductorId" runat="server" Value="0" />
                                </tr>
                            </table>
                        </asp:Panel>
                    </cc1:CollapsablePanel>
					</ContentTemplate>
					<Triggers>                        
                        <asp:AsyncPostBackTrigger ControlID="btnSaveDeductor" EventName="Click"/>
                    </Triggers>
					</asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" CausesValidation="False"
                        OnClientClick="window.close();" Text="Close" BorderWidth="1px" UseSubmitBehavior="false"
                        TabIndex="15" />
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _clienttxtPincode = "<%=this.txtPincode.ClientID %>";
        _clienttxtAddress = "<%=this.txtAddress.ClientID %>";
        _clienttxtCity = "<%=this.txtCity.ClientID %>";
        _clientcmbSalutation = "<%=this.cmbSalutation.ClientID %>";
        _clienttxtFirstName = "<%=this.txtFirstName.ClientID %>";
        _clienttxtFatherName = "<%=this.txtFatherName.ClientID %>";
        _clientcmbDesignations = "<%=this.cmbDesignations.ClientID %>";
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        _clientlstvwQuarters = "<%=this.lstvwQuarters.ClientID %>";

        function ClearLabel() {
            $get(_clientlblMessage).innerHTML = "";
           }

           function ValidateReceiptNumber(oSrc, args) {
           	var iRowCount = 0;
           	var Flag = 0;
           	var Quarter;
           	Quarter = document.getElementById(_clientlstvwQuarters + "_ctrl" + iRowCount + "_txtReceiptNumber")
           	while (Quarter != null) 
			{
				if (Quarter.value.trim() != "") 
				{
					Flag = 1;
					break;
				}
				iRowCount = iRowCount + 1
				Quarter = document.getElementById(_clientlstvwQuarters + "_ctrl" + iRowCount + "_txtReceiptNumber")
			}

			if (Flag == 0) 
			{
				oSrc.errormessage = "Receipt number for atleast one quarter should be enter.";
				args.IsValid = false;
				return true;
			}

			args.IsValid = true;
			return false;
           }

        
        function ValidatePincode(oSrc, args) {
            var sPIN = $get(_clienttxtPincode).value;

            if (sPIN.length != 6) {
                oSrc.errormessage = "Pincode should be of 6 digits.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ValidateAddress(oSrc, args) {
            var sAddress = $get(_clienttxtAddress).value;

            if (sAddress.length > 100) {
                oSrc.errormessage = "Length of Address should not exceed 100 characters.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function PreventSpecialChars(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57) || k == 0 || k == 9);
        }

        function ClearCITFields() {
            $get(_clientlblMessage).innerHTML = "";
            $get(_clienttxtPincode).value = "";
            $get(_clienttxtAddress).value = "";
            $get(_clienttxtCity).value = "";
        }

        function ClearFields() {
            $get(_clientlblMessage).innerHTML = "";
            $get(_clientcmbSalutation).value = 0;
            $get(_clientcmbDesignations).value = 0;
            $get(_clienttxtFirstName).value = "";
            $get(_clienttxtFatherName).value = "";
        }
        
    </script>
</asp:Content>
