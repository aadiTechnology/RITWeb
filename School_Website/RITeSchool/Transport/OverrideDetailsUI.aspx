<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OverrideDetailsUI.aspx.cs" Inherits="OverrideDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <span class="ClsMdtStar">*</span>
                            <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="valSum" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name should not be blank." Display="None" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="End Date should be same or greater than Start Date." Display="None" ClientValidationFunction="ValidateEndDates"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Start should not be blank." Display="None" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="End Date should not be blank." Display="None" ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="New Route should not be blank." Display="None" ControlToValidate="cmbTargetRoute" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="New Vehicle should not be blank." Display="None" ControlToValidate="cmbTargetVehicle" InitialValue="0"></asp:RequiredFieldValidator>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Old Route should not be blank." Display="None" ControlToValidate="cmbSourceRoute" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Old Vehicle should not be blank." Display="None" ControlToValidate="cmbSourceVehicle" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateRouteVehicleJourney"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="At least one student should be selected." Display="None" ClientValidationFunction="ValidateStudents"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="Selected New and Old  Journey should be of same category(Pickup / Drop)." Display="None" ClientValidationFunction="ValidateJourneys"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None" OnServerValidate="Date_Validate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table>
                                <tr>
                                    <td class="ClsBorderlight" style="height: 30px;" colspan="4">
                                        <span class="ClsLabel" style="font-weight: bold;">Basic Details : </span>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" style="width: 150px">
                                        <span class="ClsLabel">Name : </span>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" Style="width: 98%"
                                            MaxLength="100"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">Start Date : </span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="MidTxtBox" ReadOnly="true"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Start date should not be blank."
                                            AutoPostBack="False" From-Today="true" />
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                    <td class="ClsBorderlight" style="width: 150px">
                                        <span class="ClsLabel">End Date : </span>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="MidTxtBox" ReadOnly="true"></asp:TextBox>
                                        <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="End date should not be blank."
                                            AutoPostBack="False" From-Today="true" />
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr style="height: 15px;">
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" style="height: 30px;" colspan="4">
                                        <span class="ClsLabel" style="font-weight: bold;">New Vehicle / Journey Details :
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">New Route : </span>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:DropDownList ID="cmbTargetRoute" runat="server" CssClass="ExLrgCombo" Style="width: 98%"
                                            OnSelectedIndexChanged="cmbTargetRoute_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">New Vehicle : </span>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="cmbTargetVehicle" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cmbTargetVehicle_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbTargetRoute" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">New Journey : </span>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="cmbTargetJourney" runat="server" CssClass="MidCombo">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbTargetVehicle" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="cmbTargetRoute" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr style="height: 15px;">
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" style="height: 30px;" colspan="4">
                                        <span class="ClsLabel" style="font-weight: bold;">Old Vehicle / Journey Details :
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">Old Route : </span>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:DropDownList ID="cmbSourceRoute" runat="server" CssClass="ExLrgCombo" Style="width: 98%"
                                            AutoPostBack="True" OnSelectedIndexChanged="cmbSourceRoute_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">Old Vehicle : </span>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="cmbSourceVehicle" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cmbSourceVehicle_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbSourceRoute" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td class="ClsBorderlight">
                                        <span class="ClsLabel">Old Journey : </span>
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="cmbSourceJourney" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbSourceJourney_SelectedIndexChanged"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbSourceVehicle" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="cmbSourceRoute" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="ClsBorderLight">
                                                            <span class="ClsLabel">Student Details : </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ListView ID="lstvwStudentList" runat="server" DataKeyNames="UserId">
                                                                <LayoutTemplate>
                                                                    <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                        <tr class="ClsGridHeader">
                                                                            <th id="thSelect" runat="server">
                                                                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckUncheckAll(this)" />
                                                                            </th>
                                                                            <th align="left" width="100px">
                                                                                <span class="ClsLabel">Class</span>
                                                                            </th>
                                                                            <th align="left" width="100px">
                                                                                <span class="ClsLabel">Enrolment No</span>
                                                                            </th>
                                                                            <th align="left">
                                                                                <span class="ClsLabel">Student Name</span>
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceHolder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                        <td align="center">
                                                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="SetHeaderCheckbox()" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblClass" runat="server" CssClass="ClsLabel" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("RegistraionNo") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    <tr>
                                                                        <td class="LblNoRecord" align="center">
                                                                            <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbSourceJourney" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="cmbSourceVehicle" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="cmbSourceRoute" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 15px;">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" 
                                            CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientcmbSourceJourney = "<%=cmbSourceJourney.ClientID %>"
        _clientcmbSourceRoute = "<%=this.cmbSourceRoute.ClientID %>"
        _clientcmbSourceVehicle = "<%=this.cmbSourceVehicle.ClientID %>"

        _clientcmbTargetRoute = "<%=this.cmbTargetRoute.ClientID %>"
        _clientcmbTargetVehicle = "<%=this.cmbTargetVehicle.ClientID %>"
        _clientcmbTargetJourney = "<%=this.cmbTargetJourney.ClientID %>"

        _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
        _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"

        function CheckUncheckAll(obj) {
            $('[id$=chkSelect]').prop('checked', obj.checked)
        }

        function SetHeaderCheckbox() {
            if ($('[id$=chkSelect]').length == $('[id$=chkSelect]:checked').length)
                $('[id$=chkSelectAll]').prop('checked', true)
            else
                $('[id$=chkSelectAll]').prop('checked', false)
        }

        function ValidateStudents(src, args) {            
            if ($('#' + _clientcmbSourceJourney).val() != 0 && $('[id$=chkSelect]:checked').length == 0) {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateRouteVehicleJourney(src, args) {
            if ($('#' + _clientcmbSourceRoute).val() != 0 && $('#' + _clientcmbTargetRoute).val() != 0 && $('#' + _clientcmbSourceVehicle).val() != 0 && $('#' + _clientcmbTargetVehicle).val() !=0 && $('#' + _clientcmbSourceRoute).val() == $('#' + _clientcmbTargetRoute).val() && $('#' + _clientcmbSourceVehicle).val() == $('#' + _clientcmbTargetVehicle).val() && $('#' + _clientcmbSourceJourney).val() == $('#' + _clientcmbTargetJourney).val()) {
                src.errormessage = 'Old and New Route/Vehicle/Journey combination should not be same.'
                args.IsValid = false;
                return true;
            }
            else if ($('#' + _clientcmbSourceJourney).val() == 0 && $('#' + _clientcmbTargetJourney).val() != 0) {
                src.errormessage = 'Old journey should be selected.'
                args.IsValid = false;
                return true;
            }
            else if ($('#' + _clientcmbSourceJourney).val() != 0 && $('#' + _clientcmbTargetJourney).val() == 0) {
                src.errormessage = 'New journey should be selected.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateEndDates(src, args) {
            var dtStartDate = document.getElementById(_clienttxtStartDate).value;
            var startDate;
            if (document.all)
                startDate = new Date(dtStartDate.replace('-', ' '));
            else
                startDate = new Date(convertdate(dtStartDate));

            var dtEndDate = document.getElementById(_clienttxtEndDate).value;
            var endDate;
            if (document.all)
                endDate = new Date(dtEndDate.replace('-', ' '));
            else
                endDate = new Date(convertdate(dtEndDate));

            if (startDate > endDate) {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateJourneys(src, args) {
            var source = $("#"+_clientcmbSourceJourney+" option:selected").text();
            var target = $("#" + _clientcmbTargetJourney + " option:selected").text();

            if ((source.match("PICKUP") != null && target.match("PICKUP") == null) || (source.match("DROP") != null && target.match("DROP") == null) ||
            (source.match("PICKUP") == null && target.match("PICKUP") != null) || (source.match("DROP") == null && target.match("DROP") != null)) {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
