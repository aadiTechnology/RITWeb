<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentsHouseAssignmentUI.aspx.cs" Inherits="StudentsHouseAssignmentUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td colspan="4">
                                <table width="100%">
                                    <tr>
                                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                            <span class="ClsMdtStar">*
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsMdtStar" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveUp" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" id="tblMassage" runat="server" visible="true" style="color: Blue;
                                            font-weight: bold;">
                                            <tr>
                                                <td id="tdMessage" runat="server" class="ClsTextNormal" align="center">
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                        EnableViewState="false" Text=""></asp:Label>
                                                    <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" Height="20px"
                                                        Width="100%" EnableViewState="False" CssClass="LblErrorMsg"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveUp" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="tblSearch" runat="server" colspan="4">
                                <table cellpadding="0" cellspacing="2">
                                    <tr id="trCombo">
                                        <td align="center" class="ClsBorderlight" colspan="1">
                                            <span class="ClsLabel" style="width: 70px;">
                                                <asp:Label ID="lblStandard" runat="server" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                                <span id="Span2" class="colonPadding">:</span></span>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:DropDownList ID="cmbStandard" Width="95px" AutoPostBack="true" runat="server"
                                                CssClass="SmlTxtBox" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                            <asp:RequiredFieldValidator ID="reqStandard" runat="server" Display="None" InitialValue="0"
                                                ControlToValidate="cmbStandard" ErrorMessage="Standard should be selected."></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="center" class="ClsBorderlight" colspan="1">
                                            <span class="ClsLabel" style="width: 60px;">
                                                <asp:Label ID="lblDivision" runat="server" Text="<%$ Resources:LocalizedResources, Division %>"></asp:Label>
                                                <span id="Span1" class="colonPadding">:</span></span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                ID="uPnl">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbDivision" runat="server" CssClass="SmlTxtBox" Width="95px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="reqcmbDivision" runat="server" Display="None" InitialValue="0"
                                                        ControlToValidate="cmbDivision" ErrorMessage="Division should be selected."></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Show Already Configured Students: </span>
                                        </td>
                                        <td class="ClsLabel" align="left" colspan="3">
                                            <asp:CheckBox ID="chkCofiguredStudents" runat="server" Checked="false" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblShow" runat="server" border="0" cellpadding="1" cellspacing="2" style="width: 300px;"
                                    align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnShow" Text="Show" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                                OnClick="btnShow_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4" id="td1" runat="server">
                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel5">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveUp" CausesValidation="false" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                        CssClass="ClsBtn" disable-page="true" Visible="False" OnClick="btnSave_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBackUp" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                                        CssClass="ClsBtn" CausesValidation="false" Visible="False" OnClick="btnBackUp_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveUp" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="right" valign="top">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:ListView ID="lstStudentsHouseInformation" runat="server" DataKeyNames="SchoolwiseStudentId,HouseId"
                                                                    OnItemDataBound="lstStudentsHouseInformation_ItemDataBound" OnDataBound="lstStudentsHouseInformation_DataBound">
                                                                    <LayoutTemplate>
                                                                        <table id="tblContacts" runat="server" style="width: 650px; height: 100%; color: #333333"
                                                                            class="GridBorder" cellpadding="0" cellspacing="1">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="ClspaddingL" width="100px">
                                                                                    <asp:Label ID="lblRegNo" runat="server" Text="<%$ Resources:LocalizedResources, RegNo %>"></asp:Label>
                                                                                </th>
                                                                                <th align="left" class="ClspaddingL" width="80px">
                                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, RollNo %>"></asp:Label>
                                                                                </th>
                                                                                <th style="display: inline" align="left" class="ClspaddingL" width="450px">
                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, StudentName %>"></asp:Label>
                                                                                </th>
                                                                                <th align="left" class="ClspaddingL" width="150px">
                                                                                    <asp:Label ID="lblAllHouse" runat="server" Text="House"></asp:Label>
                                                                                </th>                                                                                
                                                                            </tr>
                                                                            <tr id="trHeaderControls" runat="server" class="ClsGridHeader">
                                                                                <th>
                                                                                </th>
                                                                                <th>
                                                                                </th>
                                                                                <th>
                                                                                </th>
                                                                                <th align="left" class="ClspaddingL">
                                                                                    <asp:DropDownList ID="cmbAllHouse" AppendDataBoundItems="true" Width="150px" onchange="SelectAllControls(this)"
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </th>
                                                                            </tr>
                                                                            <tr runat="server" id="itemPlaceholder">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="trStudentRow" runat="server" class="ClsGridRow">
                                                                            <td align="left" class="ClspaddingL" width="100px">
                                                                                <asp:Label ID="lblEnrollNo" runat="server" Text='<%#Eval("RegNo")%>'></asp:Label>
                                                                            </td>
                                                                            <td class="ClspaddingL">
                                                                                <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("StudentName")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:DropDownList ID="cmbHouse" AppendDataBoundItems="true" Width="150px" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>                                                                            
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="trStudentRow" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="ClspaddingL" width="10px">
                                                                                <asp:Label ID="lblEnrollNo" runat="server" Text='<%#Eval("RegNo")%>'></asp:Label>
                                                                            </td>
                                                                            <td class="ClspaddingL">
                                                                                <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("StudentName")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:DropDownList ID="cmbHouse" Width="150px" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <tr>
                                                                            <td id="tdNo" runat="server" class="LblNoRecord" align="center">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" valign="top" style="vertical-align: top;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:ListView ID="lstvwHouseCount" runat="server" OnItemDataBound="lstvwHouseCount_ItemDataBound">
                                                                    <LayoutTemplate>
                                                                        <table id="tblContacts" runat="server" style="width: 500px; height: 50%; color: #333333"
                                                                            class="GridBorder" cellpadding="0" cellspacing="1">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="ClspaddingL" width="100px">
                                                                                    <asp:Label ID="lblHouseName" runat="server" Text="House Name"></asp:Label>
                                                                                </th>
                                                                                <th align="center" width="100px">
                                                                                    <asp:Label ID="Label1" runat="server" Text="Color"></asp:Label>
                                                                                </th>
                                                                                <th align="Center" class="ClspaddingL" width="10px" style="text-align: center">
                                                                                    <asp:Label ID="lblStudentcount" runat="server" Text="Student Count"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                            <tr runat="server" id="itemPlaceholder">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="trStudentRow" runat="server" class="ClsGridRow">
                                                                            <td align="left" class="ClspaddingL" width="80px">
                                                                                <asp:Label ID="lblHouseName" runat="server" Text='<%#Eval("HouseName")%>'></asp:Label>
                                                                            </td>
                                                                            <td id="tdColor" runat="server">
                                                                            </td>
                                                                            <td class="ClspaddingL" width="5px" style="text-align: center">
                                                                                <asp:Label ID="lblStudentCount" runat="server" Text='<%#Eval("StudentCount")%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="trStudentRow" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="ClspaddingL" width="100px">
                                                                                <asp:Label ID="lblHouseName" runat="server" Text='<%#Eval("HouseName")%>'></asp:Label>
                                                                            </td>
                                                                            <td id="tdColor" runat="server">
                                                                            </td>
                                                                            <td class="ClspaddingL" width="80px" style="text-align: center;">
                                                                                <asp:Label ID="lblStudentCount" runat="server" Text='<%#Eval("StudentCount")%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <tr>
                                                                            <td id="tdNo" runat="server" class="LblNoRecord" align="center">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveUp" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4" id="tdBack" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                                ID="UpdatePanel3">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" CausesValidation="false" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                        CssClass="ClsBtn" disable-page="true" Visible="False" OnClick="btnSave_Click" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSaveUp" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                                CssClass="ClsBtn" CausesValidation="false" OnClick="btnBackUp_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                    <asp:HiddenField ID="hidHouseConId" runat="server" Value="0" ViewStateMode="Enabled" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
            ID="UpdatePanel1">
            <ContentTemplate>
                <asp:HiddenField ID="hidSortDirection" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidCanEdit" runat="server" Value="Y" />
                <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">
        _clientHouseListview = "<%=this.lstStudentsHouseInformation.ClientID %>"
        _clientAllHouse = _clientHouseListview + "_cmbAllHouse"
        function SelectAllControls(cmbAllHouse) {
            var rowindex = 0
            var cmbHouse = document.getElementById(_clientHouseListview + "_ctrl" + rowindex + "_cmbHouse");
            while (cmbHouse != null) {
                cmbHouse.value = cmbAllHouse.value
                ChangeColor(cmbHouse);
                rowindex += 1
                var cmbHouse = document.getElementById(_clientHouseListview + "_ctrl" + rowindex + "_cmbHouse");
            }
        }
        
        function ChangeColor(cmbHouse) {
            cmbHouse.style.backgroundColor = cmbHouse.options[cmbHouse.selectedIndex].style.backgroundColor;
        }

    </script>
</asp:Content>
