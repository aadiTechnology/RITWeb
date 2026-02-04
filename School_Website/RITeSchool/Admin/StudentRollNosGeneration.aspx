<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    EnableEventValidation="false" CodeFile="StudentRollNosGeneration.aspx.cs" Inherits="StudentRollNosGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                            <span class="ClsMdtStar">* Mandatory Fields</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Always">
                                    <ContentTemplate>
                                        <table width="100%" id="tblMassage" runat="server" visible="true" style="color: Blue;
                                            font-weight: bold;">
                                            <tr>
                                                <td align="center" valign="top">
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                        EnableViewState="false" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" 
                                     Height="20px" Width="100%" EnableViewState="False" CssClass="LblErrorMsg" ></asp:Label>
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="tblSearch" runat="server" colspan="3">
                                <table cellpadding="0" cellspacing="2">
                                    <tr id="trCombo">
                                        <td align="left" class="ClsBorderlight" colspan="1">
                                            <span class="ClsLabel">Standard :</span>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:DropDownList ID="cmbStandard" Width="95px" AutoPostBack="true" OnSelectedIndexChanged="cmbStd_SelectedIndexChanged"
                                                runat="server" CssClass="SmlTxtBox">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>&nbsp;&nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="reqdvalStandard" runat="server" ControlToValidate="cmbStandard"
                                                Display="None" ErrorMessage="Please select Standard." InitialValue="0"></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="left" class="ClsBorderlight" colspan="1">
                                            <span class="ClsLabel">Division :</span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                ID="uPnl">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbDivision" runat="server" CssClass="SmlTxtBox" Width="95px"
                                                        OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged" CausesValidation="True">
                                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>&nbsp;&nbsp;&nbsp;
                                                    <asp:RequiredFieldValidator ID="reqdvalDivision" runat="server" ControlToValidate="cmbDivision"
                                                        Display="None" ErrorMessage="Please select Division." InitialValue="0"></asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" OnClick="btnSearch_Click"
                                                Text="Show" />
                                        </td>
                                    </tr>
                                </table>
                            </td>                            
                        </tr>
                        <tr>
                            <td align="center" colspan="1" id="td1" runat="server">
                                <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                    ID="UpdatePanel5">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveUp" CausesValidation="false" runat="server" Text="Save" CssClass="ClsBtn"
                                                        OnClick="btnSave_Click" Visible="False" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBackUp" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                                                        OnClick="btnBack_Click" Visible="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center" valign="top">
                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <table style="width:100%;height:100%;" cellpadding="0" cellspacing="2">
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left" width="50px" id="tdStandardDivisionLabel" runat="server">
                                                                <asp:Label ID="lblDivision0" runat="server" Style="color: #006666; float: left; font-size: 9pt;
                                                                    font-weight: bold;" EnableViewState="false" Text="Class : "></asp:Label>
                                                            </td>
                                                            <td align="center" class="ClsHilightBG" width="100px" id="tdStandardDivisionValue"
                                                                runat="server">
                                                                <asp:Label ID="lblStandardDivisionValue" runat="server" Font-Bold="True" Width="100px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="top">
                                                    <asp:ListView ID="lstvwStudentList" ItemPlaceholderID="ContactRowContainer" runat="server"
                                                        DataKeyNames="YearWise_Student_Id,SchoolLeft_Date" OnItemDataBound="lstvwStudentList_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table style="width:100%;height:100%;color: #333333" runat="server" id="tblContacts" class="GridBorder"
                                                                cellpadding="0" cellspacing="1">
                                                                <tr class="ClsGridHeader">
                                                                    <th>
                                                                        Reg. No.
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        Student Name
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        Current Roll No.
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL">
                                                                        New Roll No.
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="ContactRowContainer" />
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trStudentRow" runat="server" class="ClsGridRow">
                                                                <td align="left" class="ClspaddingL" width="10%">
                                                                    <asp:Label ID="lblEnrollNo" runat="server" Text='<%#Eval("Enrolment_Number")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblCurrentRoll_No" runat="server" Text='<%#Eval("Roll_No")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblNewRoll_No" runat="server" Text='<%#Eval("Roll_No")%>' Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtNewRoll_No" CssClass="ExSmlTxtBoxP" MaxLength="3" Width="35px"
                                                                        runat="server" Text='<%#Eval("Roll_No")%>'></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="trStudentRow" runat="server" class="ClsGridAltRow">
                                                                <td align="left" class="ClspaddingL" width="10%">
                                                                    <asp:Label ID="lblEnrollNo" runat="server" Text='<%#Eval("Enrolment_Number")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblCurrentRoll_No" runat="server" Text='<%#Eval("Roll_No")%>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblNewRoll_No" runat="server" Text='<%#Eval("Roll_No")%>' Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtNewRoll_No" CssClass="ExSmlTxtBoxP" MaxLength="3" Width="35px"
                                                                        runat="server" Text='<%#Eval("Roll_No")%>'></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="1" id="tdBack" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                                ID="UpdatePanel3">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" CausesValidation="false" runat="server" Text="Save" CssClass="ClsBtn"
                                                        OnClick="btnSave_Click" Visible="False" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                                                OnClick="btnBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
            ID="UpdatePanel1">
            <ContentTemplate>
                <asp:HiddenField ID="hidSortDirection" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidStandardId" runat="server" />
                <asp:HiddenField ID="hidDivisionId" runat="server" />
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
        function ValidatePage(SCheckBoxname, SLabelname, SRegLabelname, grdid, numSelects) {

            var lblmsg = '<%= lblMessage.ClientID %>';
            $get(lblmsg).style.visibility = "hidden";
            return ValidateRollNumbersInListView(SCheckBoxname, SLabelname, SRegLabelname, grdid, numSelects)
        }
    </script>
</asp:Content>
