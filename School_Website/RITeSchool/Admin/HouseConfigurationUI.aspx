<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="HouseConfigurationUI.aspx.cs" Inherits="HouseConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                    vertical-align: top">
                    <tr>
                        <td>
                        </td>
                        <td align="right" style="height: 25px; width: 200px; padding-right:10px;" class="ClsGreenBG">
                            <asp:LinkButton ID="lnkHouseConfig" runat="server" Text="House Standard Assignment"
                                CssClass="SubTitle" CausesValidation="False"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                            <span class="ClsMdtStar">* Mandatory Fields</span>
                        </td>
                    </tr>
                    <tr>
                        
                        <td align="left">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="1" id="tdMessage" runat="server" class="ClsTextNormal" align="center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="1" cellspacing="2" align="center" style="width: 36%">
        <tr align="center">
            <td align="left" class="ClsBorderLight" style="width: 49%">
                <span class="ClsLabel">Name :</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtHouseName" runat="server" MaxLength="50" CssClass="LrgTxtBox"></asp:TextBox>
                <span class="ClsMdtStar">*</span>
                <asp:RequiredFieldValidator ID="reqHouseName" runat="server" ControlToValidate="txtHouseName"
                    Display="None" ErrorMessage="House Name should not be blank."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr align="center">
            <td align="left" class="ClsBorderLight" style="width: 49%">
                <span class="ClsLabel">Color :</span>
            </td>
            <td align="left">
                <asp:DropDownList ID="cmbHouseColor" runat="server" CssClass="LrgCombo" CausesValidation="true">
                </asp:DropDownList>
                <span class="ClsMdtStar">*</span>
                <asp:CustomValidator ID="cstValHouseColor" runat="server" ClientValidationFunction="ValidateColor"
                    SetFocusOnError="True" Display="None" ErrorMessage="House Color should be selected."></asp:CustomValidator>
            </td>
        </tr>
        <tr align="center">
            <td align="left" class="ClsBorderLight" style="width: 49%">
                <span class="ClsLabel">Motto :</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtMotto" runat="server" TextMode="MultiLine" CssClass="ExLrgTxtBox"></asp:TextBox>
                <asp:CustomValidator ID="cstvalMoto" runat="server" ClientValidationFunction="ValidateMoto"
                    SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="1" cellspacing="2" style="width: 300px;" align="center">
        <tr>
            <td align="center">
                <asp:Button ID="btnAdd" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                    disable-page="true" OnClick="btnAdd_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" OnClick="btnCancel_Click" />&nbsp;
            </td>
        </tr>
    </table>
    <table align="center" width="80%">
        <tr>
            <td align="center">
                <table align="center" width="80%">
                    <tr align="center">
                        <td align="center">
                            <asp:ListView ID="lstvwConfigureHouse" runat="server" DataKeyNames="Id" DataSourceID="ObjDSConfigureHouse"
                                OnItemDataBound="lstvwConfigureHouse_ItemDataBound" OnItemCommand="lstvwConfigureHouse_ItemCommand"
                                OnSorting="lstvwConfigureHouse_Sorting">
                                <LayoutTemplate>
                                    <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                        cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="center" width="10%" style="white-space: nowrap; padding-left: 10px;">
                                                Sr. No.
                                            </th>
                                            <th align="left" width="250px" style="padding-left: 10px;">
                                                <asp:LinkButton ID="lnkBtnDesignationName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                    CausesValidation="false" ForeColor="Black">Name </asp:LinkButton>
                                            </th>
                                            <th align="left" width="250px" style="padding-left: 10px;">
                                                Color
                                            </th>
                                            <th align="left" width="250px" style="padding-left: 10px;">
                                                Motto
                                            </th>
                                            <th align="center" width="50px" style="padding-left: 10px;">
                                                Edit
                                            </th>
                                            <th align="center" width="50px" style="padding-left: 10px;">
                                                Delete
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                        <tr class="ClsBorderPager" id="trDataPager">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <td align="center">
                                            <asp:Label ID="lblSrNo" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </td>
                                        <td id="tdColor" runat="server" align="left" class="paddingL">
                                            <asp:Label ID="lblColor" runat="server" Text='<%# Eval("Color") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblMotto" runat="server" Text='<%# Eval("Motto") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ImageUrl="../images/IconGrid_Edit.GIF" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                ImageUrl="../images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <td align="center">
                                            <asp:Label ID="lblSrNo" runat="server"></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </td>
                                        <td id="tdColor1" runat="server" align="left" class="paddingL">
                                            <asp:Label ID="lblColor" runat="server" Text='<%# Eval("Color") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblMotto" runat="server" Text='<%# Eval("Motto") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ImageUrl="../images/IconGrid_Edit.GIF" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <tr>
                                        <td id="NoRecordFound" runat="server" class="LblNoRecord" align="center">
                                            No record found.
                                        </td>
                                    </tr>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                </table>
                <asp:ObjectDataSource TypeName="BusinessLogic.HouseCofigurationBL" EnablePaging="True"
                    ID="ObjDSConfigureHouse" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                    EnableCaching="False" SelectCountMethod="Count">
                    <SelectParameters>
                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                            Type="int32" />
                        <asp:Parameter Name="sortExpression" Type="String" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="hidSortDirection" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidHouseConfigurationId" runat="server" />
                <asp:HiddenField ID="hidHouseName" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="False"
                    OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        _clientcmbHouseColor = "<%=this.cmbHouseColor.ClientID %>";
        _clienttxtMotto = "<%=this.txtMotto.ClientID %>";

        function ClearSuccessfulMessage() {
            document.getElementById(_clientlblMessage).innerHTML = "";
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        function ValidateColor(oSrc, args) {
            var color = $get(_clientcmbHouseColor).value;
            if (color == "0") {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ValidateMoto(oSrc, args) {
            var parameter = $get(_clienttxtMotto).value;
            if (parameter.length > 300) {
                oSrc.errormessage = "Motto length should not be greater than 300 characters.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ChangeColor(cmbHouse) {
            cmbHouse.style.backgroundColor = cmbHouse.options[cmbHouse.selectedIndex].style.backgroundColor;
        }

        function OpenPopup() {
            window.open("HouseConfigurationPopUp.aspx", '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=660').focus();
        }

    </script>
</asp:Content>
