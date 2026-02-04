<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DynamicFieldDetailsUI.aspx.cs" Inherits="DynamicFieldDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td id="MainDataTable" align="center">
                <table border="0" cellpadding="0" cellspacing="0" width="50%" style="height: 100%;">
                    <tr id="trerror"  runat="server">
                        <td align="center" >
                             <asp:ValidationSummary ID="valSumErrorMsg" runat="server"  ShowMessageBox="False"
                                ShowSummary="true" />
                            <asp:CustomValidator ID="cstStudentDetails" runat="server" ClientValidationFunction="CheckAtListOne"  ErrorMessage="Select atleast on field to export from Student Details."
                                            SetFocusOnError="True" Display="None" CssClass="LblErrorMsg"></asp:CustomValidator>
                                            
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                               
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                        CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 100%;">
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr align="center" valign="top">
                                    <td align="center">
                                        <div style="height: 100%; overflow: auto">
                                            <table width="100%">
                                                <tr id="Tr1" runat="server">
                                                    <td colspan="2">
                                                        <div style="width: 100%;">
                                                            <asp:UpdatePanel ID="upnlUpdate" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="85%">
                                                                        <tr>
                                                                            <td align="left" style="width: 25%">
                                                                                <asp:Label ID="lblStandards" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                                    Font-Bold="True" Text="Select Standard :"></asp:Label>
                                                                            </td>
                                                                            <td class="ClsBorderlight"  style="width: 25%" align="left">
                                                                                <asp:DropDownList ID="cmbStandards" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                                                    OnSelectedIndexChanged="cmbStandards_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td style="width: 25%" align="left">
                                                                                <asp:Label ID="Label1" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                                    Text="Select Division :"></asp:Label>
                                                                            </td>
                                                                            <td class="ClsBorderlight"  style="width: 25%" align="left">
                                                                                <asp:DropDownList ID="cmbDivisions" runat="server" CssClass="LrgCombo">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 0.5px;">
                                                                            <td colspan="4">&nbsp;</td>
                                                                        </tr>
                                                                         <tr>
                                                                            <td align="left" colspan="1" style="width: 25%">
                                                                                <asp:Label ID="Label2" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                                    Font-Bold="True" Text="Include With Left :"></asp:Label>
                                                                            </td>
                                                                             <td align="left" colspan="3" style="width: 25%">
                                                                                <asp:CheckBox ID="chkIncludeWithLeft" runat="server"/>
                                                                            </td>
                                                                         </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandards" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="TrLable" runat="server">
                                                    <td align="center" valign="top" class="ClsHilightBG" style="width: 50%">
                                                        <asp:Label ID="lblStudentDetails" runat="server" Font-Bold="True" CssClass="ClsHilightText"
                                                            Text="Student Details"></asp:Label>
                                                    </td>
                                                    <td runat="server" id="tdAddtionalDetails" visible="false" align="center" class="ClsHilightBG"
                                                        style="width: 50%">
                                                        <asp:Label ID="lblStidentdetailsmenus" runat="server" Font-Bold="True" Text="Additional Details"
                                                            CssClass="ClsHilightText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" runat ="server" id="tdStudentDetails">
                                                        <asp:ListView ID="lstvwStudentDetails" runat="server">
                                                            <LayoutTemplate>
                                                                <table id="tblStudentDetails" width="100%" style="color: #333" cellpadding="3" cellspacing="1"
                                                                    class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th id="thSelectAll" runat="server" align="center" style="padding: 0;">
                                                                            <asp:CheckBox ID="chkSelectAll" Text="Select All" runat="server" Style="font-weight: bold;"
                                                                                onclick="CheckAllUncheckAllStudentDetails()" CssClass="vertical-align-top all-checkbox" />
                                                                        </th>
                                                                        <th align="left">
                                                                            <asp:Label ID="lblStandardName" runat="server" Text="Fields To Select" CausesValidation="false"
                                                                                ForeColor="Black"> </asp:Label>
                                                                        </th>
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr class="ClsGridRow">
                                                                    <td align="center" id="tdchkPay" runat="server">
                                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsSelected") %>' />
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hidDnyamicReportFieldMasterIdForStudentInfo" runat="server"
                                                                            Value='<%# Eval("DynamicReportFieldMasterId") %>' />
                                                                        <asp:Label ID="lblFieldName" runat="server" Text='<%# Eval("FieldText") %>' />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr class="ClsGridAltRow">
                                                                    <td align="center" id="tdchkPay" runat="server">
                                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsSelected") %>' />
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hidDnyamicReportFieldMasterIdForStudentInfo" runat="server"
                                                                            Value='<%# Eval("DynamicReportFieldMasterId") %>' />
                                                                        <asp:Label ID="lblFieldName" runat="server" Text='<%# Eval("FieldText") %>' />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center" colspan="4" style="width: 100%; float: left">
                                                                        No record found.
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                    </td>
                                                    <td runat="server" align="center" valign="top" style="width: 50%; vertical-align: top;">
                                                        <% if (SchoolBase.Settings.IsAdditionalFieldsApplicable)
                                                           {%>
                                                        <asp:ListView ID="lstViewAdditionalFields" runat="server">
                                                            <LayoutTemplate>
                                                                <table id="lstvwPayFee" width="100%" style="color: #333" cellpadding="3" cellspacing="1"
                                                                    class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th id="thSelectAll" runat="server" align="center" style="padding: 0;">
                                                                            <asp:CheckBox ID="chkSelectAll" Text="Select All" runat="server" Style="font-weight: bold;"
                                                                                onclick="CheckAllUncheckAllAdditionalDetails()" CssClass="vertical-align-top all-checkbox" />
                                                                        </th>
                                                                        <th align="left">
                                                                            <asp:Label ID="lblStandardName" runat="server" Text="Fields To Select" CausesValidation="false"
                                                                                ForeColor="Black"> </asp:Label>
                                                                        </th>
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr class="ClsGridRow">
                                                                    <td align="center" id="tdchkPay" runat="server">
                                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsSelected") %>' />
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hidDnyamicReportFieldMasterIdForStudentAddiInfo" runat="server"
                                                                            Value='<%# Eval("DynamicReportFieldMasterId") %>' />
                                                                        <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("FieldText") %>' />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr class="ClsGridAltRow">
                                                                    <td align="center" id="tdchkPay" runat="server">
                                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsSelected") %>' />
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hidDnyamicReportFieldMasterIdForStudentAddiInfo" runat="server"
                                                                            Value='<%# Eval("DynamicReportFieldMasterId") %>' />
                                                                        <asp:Label ID="lblStandardName" runat="server" Text='<%# Eval("FieldText") %>' />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center" colspan="4" style="width: 100%; float: left">
                                                                        No record found.
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                        <%} %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td >
                    <table width="100%" align="center">
                        <tr>
                            <td align="left" class="ClsBorderlight " style="width: 80px; background-color: #ffffc4;">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text= "<%$ Resources:LocalizedResources, Note%>"
                                    CssClass="LblNrmlB"></asp:Label>
                                     <span class="colonPadding">:</span>
                            </td>
                            <td align="left" class="ClsBorderlight" >
                                <asp:Label ID="lblNote" runat="server"  BorderWidth="0px" Width="100%" CssClass="LblSmlV" Text= "This action will export only saved details. So make sure there are no export action on unsaved details."  ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1" style="padding-top: 5px">
                                        <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                          OnClientClick="ClearMessage()"  BorderWidth="1px" UseSubmitBehavior="false" OnClick="btnSave_Click"></asp:Button>
                                        <asp:Button CssClass="ClsBtn" ID="btnExport" runat="server" Text="<%$ Resources:LocalizedResources, Export %>"
                                            BorderWidth="1px" UseSubmitBehavior="false" OnClick="btnExport_Click"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        _clientlstvwStudentDetails = "<%=this.lstvwStudentDetails.ClientID %>"
        _clientlstViewAdditionalFields = "<%=this.lstViewAdditionalFields.ClientID %>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"

        //This function is used to check uncheck all check boxes.
        function CheckAllUncheckAllStudentDetails() {
            var checkAll = document.getElementById("ctl00_MainBody_lstvwStudentDetails_chkSelectAll").checked
            var chk
            var iRowCount = 0
            if (iRowCount < 10)
                chk = document.getElementById(_clientlstvwStudentDetails + "_ctrl" + iRowCount + "_chkSelect")
            else
                chk = document.getElementById(_clientlstvwStudentDetails + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                if (iRowCount < 10)
                    chk = document.getElementById(_clientlstvwStudentDetails + "_ctrl" + iRowCount + "_chkSelect")
                else
                    chk = document.getElementById(_clientlstvwStudentDetails + "_ctrl" + iRowCount + "_chkSelect")
            }
        }

        //This function is used to check uncheck all check boxes for additional details.
        function CheckAllUncheckAllAdditionalDetails() {
            var checkAll = document.getElementById("ctl00_MainBody_lstViewAdditionalFields_chkSelectAll").checked
            var chk
            var iRowCount = 0
            if (iRowCount < 10)
                chk = document.getElementById(_clientlstViewAdditionalFields + "_ctrl" + iRowCount + "_chkSelect")
            else
                chk = document.getElementById(_clientlstViewAdditionalFields + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                if (iRowCount < 10)
                    chk = document.getElementById(_clientlstViewAdditionalFields + "_ctrl" + iRowCount + "_chkSelect")
                else
                    chk = document.getElementById(_clientlstViewAdditionalFields + "_ctrl" + iRowCount + "_chkSelect")
            }
        }

        //This function is used to clear success message lable.
        function ClearMessage() {
            document.getElementById(_clientlblUpdateSucess).innerHTML = ''
        }

        //This method is used validate user has been selected at least one check box for student details.
        function CheckAtListOne(oSrc, args) {
                        var iRowCount = 0;
                        var isFound = false;
                        var isValid = false;
                        var chk = document.getElementById(_clientlstvwStudentDetails + "_ctrl" + iRowCount + "_chkSelect")
                        while (chk != null) {
                            if (chk.checked) {
                                isValid = true;
                                break;
                            }
                            iRowCount = iRowCount + 1;
                            chk = document.getElementById(_clientlstvwStudentDetails + "_ctrl" + iRowCount + "_chkSelect")
                        }
                        args.IsValid = isValid;

                    }

                    
        
    </script>
</asp:Content>
