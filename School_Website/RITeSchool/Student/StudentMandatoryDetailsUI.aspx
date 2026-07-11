<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentMandatoryDetailsUI.aspx.cs" Inherits="StudentMandatoryDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <%-- Mandatory marker and message area --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="0" cellspacing="1">
                <tr id="trMandatory" runat="server">
                    <td align="right" colspan="6">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" runat="server" CssClass="ClsMdtStar" Text="All fields are mandatory."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div style="margin:0px auto;font-size:15px;width:80%" class="ClsHilightBGB">
                            <span class="">Please submit the required details below to access the other screens.</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:ValidationSummary ID="valSumError" runat="server" HeaderText="Please correct following errors."
                            CssClass="ClsMdtStar" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td align="center" id="tdMessage" runat="server" colspan="2">
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                        <asp:Label ID="lblError" runat="server" EnableViewState="false" ForeColor="Red" CssClass="ClsTextNormal"
                            Style="display: block; margin: 5px 0;"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="width: 50%; margin: auto; vertical-align: top;">
        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="MainBodyDiv">
                    <table id="tblUserInfo" border="0" cellpadding="1" cellspacing="2" align="center"
                        width="100%">
                        <%-- Student Basic Information section --%>
                        <tr>
                            <td align="center" colspan="2">
                                <h3>
                                    <u>Student Basic Information</u></h3>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <div style="width: 80%; border: 1px solid gray; padding: 20px 100px; border-radius: 10px;
                                    box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.4);">
                                    <table style="margin: 5px auto;">
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 150px">
                                                <asp:Label ID="lblFatherMobileNumber" runat="server" CssClass="ClsLabel" Text="Father Mobile Number :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtFatherMobileNumber" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                    onblur="extractNumber(this,1,false);" onkeyup="extractNumber(this,1,false);"
                                                    onkeypress="return blockNonNumbers(this,event,true,false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvFatherMobileNumber" runat="server" Display="None"
                                                    ControlToValidate="txtFatherMobileNumber" CssClass="ClsMdtStar" ErrorMessage="Father Mobile Number should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="lblMotherMobileNumber" runat="server" CssClass="ClsLabel" Text="Mother Mobile Number :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtMotherMobileNumber" runat="server" CssClass="SmlTxtBox" MaxLength="11"
                                                    onblur="extractNumber(this,1,false);" onkeyup="extractNumber(this,1,false);"
                                                    onkeypress="return blockNonNumbers(this,event,true,false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvMotherMobileNumber" runat="server" Display="None"
                                                    ControlToValidate="txtMotherMobileNumber" CssClass="ClsMdtStar" ErrorMessage="Mother Mobile Number should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="lblEmergencyContact" runat="server" CssClass="ClsLabel" Text="Emergency Contact :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtEmergencyContact" runat="server" CssClass="SmlTxtBox" MaxLength="15"
                                                    onblur="extractNumber(this,1,false);" onkeyup="extractNumber(this,1,false);"
                                                    onkeypress="return blockNonNumbers(this,event,true,false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvEmergencyContact" runat="server" Display="None"
                                                    ControlToValidate="txtEmergencyContact" CssClass="ClsMdtStar" ErrorMessage="Emergency Contact should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="lblBloodGroup" runat="server" CssClass="ClsLabel" Text="Blood Group :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="SmlCombo">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvBloodGroup" runat="server" Display="None" InitialValue="0"
                                                    ControlToValidate="ddlBloodGroup" CssClass="ClsMdtStar" ErrorMessage="Blood Group should be selected."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <%-- Student Transport Information section --%>
                        <tr>
                            <td align="center" colspan="2">
                                <h3>
                                    <u>Student Transport Information</u></h3>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <div style="width: 80%; border: 1px solid gray; padding: 20px 50px; border-radius: 10px;
                                    box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.4);">
                                    <table style="margin: 10px auto;">
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 150px;">
                                                <asp:Label ID="lblTransportMode" runat="server" CssClass="ClsLabel" Text="Transport Mode :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTransportMode" runat="server" CssClass="midCombo" AutoPostBack="false"
                                                    OnSelectedIndexChanged="ddlTransportMode_SelectedIndexChanged" onchange="ToggleTransportFields();">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvTransportMode" runat="server" Display="None" InitialValue="0"
                                                    ControlToValidate="ddlTransportMode" CssClass="ClsMdtStar" ErrorMessage="Transport Mode should be selected."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr id="trRouteStop" runat="server" style="display: none;">
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="lblRouteNo" runat="server" CssClass="ClsLabel" Text="Route No :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtRouteNo" runat="server" CssClass="SmlTxtBox" MaxLength="15"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvRouteNo" runat="server" Display="None" ControlToValidate="txtRouteNo"
                                                    CssClass="ClsMdtStar" ErrorMessage="Route No should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr id="trStopName" runat="server" style="display: none;">
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="lblStopName" runat="server" CssClass="ClsLabel" Text="Stop Name :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtStopName" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvStopName" runat="server" Display="None" ControlToValidate="txtStopName"
                                                    CssClass="ClsMdtStar" ErrorMessage="Stop Name should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr id="trContractor" runat="server" style="display: none;">
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="lblContractorName" runat="server" CssClass="ClsLabel" Text="Contractor Name :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtContractorName" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvContractorName" runat="server" Display="None"
                                                    ControlToValidate="txtContractorName" CssClass="ClsMdtStar" ErrorMessage="Contractor Name should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr id="trContractorNo" runat="server" style="display: none;">
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="lblContractorContactNo" runat="server" CssClass="ClsLabel" Text="Contractor Contact No. :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtContractorContactNo" runat="server" CssClass="SmlTxtBox" MaxLength="15"
                                                    onblur="extractNumber(this,1,false);" onkeyup="extractNumber(this,1,false);"
                                                    onkeypress="return blockNonNumbers(this,event,true,false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="rfvContractorContactNo" runat="server" Display="None"
                                                    ControlToValidate="txtContractorContactNo" CssClass="ClsMdtStar" ErrorMessage="Contractor Contact No. should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:CustomValidator ID="cvTransportValidation" runat="server" Display="None" ClientValidationFunction="ValidateTransportFields"
                                                    EnableClientScript="true" OnServerValidate="cvTransportValidation_ServerValidate"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 30px;">
                            <td>
                            </td>
                        </tr>
                        <%-- Action buttons --%>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" OnClick="btnSave_Click" />
                                <asp:Button ID="btnSubmit" runat="server" CssClass="ClsBtn" Text="Submit" OnClick="btnSubmit_Click"
                                    CausesValidation="false" />
                            </td>
                        </tr>
                        <tr style="height: 30px;">
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript" language="javascript">
        // Client ids for transport mode controls.
        var _clientddlTransportMode = "<%=this.ddlTransportMode.ClientID %>";
        var _clienttxtRouteNo = "<%=this.txtRouteNo.ClientID %>";
        var _clienttxtStopName = "<%=this.txtStopName.ClientID %>";
        var _clienttxtContractorName = "<%=this.txtContractorName.ClientID %>";
        var _clienttxtContractorContactNo = "<%=this.txtContractorContactNo.ClientID %>";
        var _clienttrRouteStop = "<%=this.trRouteStop.ClientID %>";
        var _clienttrStopName = "<%=this.trStopName.ClientID %>";
        var _clienttrContractor = "<%=this.trContractor.ClientID %>";
        var _clienttrContractorNo = "<%=this.trContractorNo.ClientID %>";

        // Toggles route/contractor fields as per selected transport mode.
        function ToggleTransportFields() {
            var mode = document.getElementById(_clientddlTransportMode).value;

            var trRouteStop = document.getElementById(_clienttrRouteStop);
            var trStopName = document.getElementById(_clienttrStopName);
            var trContractor = document.getElementById(_clienttrContractor);
            var trContractorNo = document.getElementById(_clienttrContractorNo);

            if (mode == "1") { // School Transport
                trRouteStop.style.display = "";
                trStopName.style.display = "";
                trContractor.style.display = "none";
                trContractorNo.style.display = "none";
            }
            else if (mode == "2") { // Private Transport
                trRouteStop.style.display = "none";
                trStopName.style.display = "none";
                trContractor.style.display = "";
                trContractorNo.style.display = "";
            }
            else { // Default (0 or Select)
                trRouteStop.style.display = "none";
                trStopName.style.display = "none";
                trContractor.style.display = "none";
                trContractorNo.style.display = "none";
            }
        }

        // Validates transport fields based on selected transport mode.
        function ValidateTransportFields(source, args) {
            var mode = document.getElementById(_clientddlTransportMode).value;

            if (mode == "1") {
                var routeNo = document.getElementById(_clienttxtRouteNo).value;
                var stopName = document.getElementById(_clienttxtStopName).value;

                if (routeNo.replace(/^\s+|\s+$/g, '') == "" || stopName.replace(/^\s+|\s+$/g, '') == "") {
                    source.errormessage = "Route No and Stop Name should not be blank for selected Transport Mode.";
                    args.IsValid = false;
                    return;
                }
            }
            else if (mode == "2") {
                var contractorName = document.getElementById(_clienttxtContractorName).value;
                var contractorContactNo = document.getElementById(_clienttxtContractorContactNo).value;

                if (contractorName.replace(/^\s+|\s+$/g, '') == "" || contractorContactNo.replace(/^\s+|\s+$/g, '') == "") {
                    source.errormessage = "Contractor Name and Contractor Contact No. should not be blank for selected Transport Mode.";
                    args.IsValid = false;
                    return;
                }
            }

            args.IsValid = true;
        }

        // Re-applies visibility after page lifecycle updates.
        function pageLoad() {
            ToggleTransportFields();
        }

        function Resetmessage() {
            $('#' + '<%=this.lblMessage.ClientID %>').html('');
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
