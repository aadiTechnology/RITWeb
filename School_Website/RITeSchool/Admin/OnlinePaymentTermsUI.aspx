<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="OnlinePaymentTermsUI.aspx.cs" Inherits="OnlinePaymentTermsUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
        .notice-popup-wrapper
        {
            position: absolute;
            left: 50%;
            top: 50%;
            border: solid 2px darkgreen;
            background-color: lightyellow;
            font-family: Tahoma;
        }
        .notice-popup-title-text
        {
            margin: 0;
            text-align: left;
            font-size: 14px;
        }
        
        .notice-popup-title-closebtn
        {
            float: right;
            cursor: pointer;
        }
        
        .notice-popup-content
        {
            padding: 15px;
            text-align: left;
            vertical-align: top;
            overflow: auto;
        }
        .web_dialog_overlay
        {
            position: absolute;
            height: 100%;
            width: 100%;
            background: transparent;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 1001;
            display: none;
        }
        .style1
        {
            height: 26px;
        }
        #tblControls
        {
            width: 990px;
        }
        .btnDwnload
        {
            font-size: 9pt;
            font-weight: 8px;
            font-style: normal;
            font-family: Arial;
        }
    </style>
    <div id="overlay" class="web_dialog_overlay">
    </div>
    <div class="MainBodyDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsMdtStar" />
                            <asp:CustomValidator ID="cstvalParameter" runat="server" ClientValidationFunction="ValidateParameter"
                                SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="100%" class="LblNormal ClsMdtStar">
                            * Mandatory Fields
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="700px">
                                <tr>
                                    <td align="center">
                                        <table id="tblControls" runat="server">
                                            <tr>
                                                <td colspan="2" id="tdMessage" runat="server" class="ClsTextNormal" align="center">
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight paddingL" style="width: 205px;">
                                        Select Category :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbCatagory" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                            Width="200px" OnSelectedIndexChanged="cmbCatagory_SelectedIndexChanged" />
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr id="trDescription" runat="server">
                                    <td class="ClsBorderlight paddingL" style="width: 205px;">
                                        Description :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="LrgTxtBox" Width="550px"
                                            Height="50px" MaxLength="1500" TextMode="MultiLine"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                         <asp:Button ID="btnCancel" runat="server"  Text="Cancel" CssClass="ClsBtn" CausesValidation ="false" onclick="btnCancel_Click"
                                 />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="1000px">
                                <tr id="trLstview" runat="server">
                                    <td>
                                        <asp:ListView ID="lstvwHomeworkTeacher" runat="server" DataKeyNames="Id,TermsCatagoryId"
                                            OnItemCommand="lstvwHomeworkTeacher_ItemCommand" OnItemDataBound="lstvwHomeworkTeacher_ItemDataBound">
                                            <LayoutTemplate>
                                                <table id="tblhomework" align="center" width="100%" runat="server" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" class="paddingL">
                                                            <asp:Label ID="lblSubject" runat="server" Text="Description"></asp:Label>
                                                        </th>
                                                        <th align="center" width="50px">
                                                            <asp:Label ID="Label7" runat="server" Text="Edit"></asp:Label>
                                                        </th>
                                                        <th align="center" width="50px">
                                                            <asp:Label ID="lblAdd" runat="server" Text="Delete"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left">
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Discription") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="Delete" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                    <td align="left">
                                                        <asp:HiddenField ID="hidId" runat="server" Value='<%#Eval("Id") %>' />
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Discription") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="Delete" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td width="550px" align="center" class="LblNoRecord">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:HiddenField ID="hidDescriptionId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBackEnd" runat="server" CausesValidation="false" Text="Back" CssClass="ClsBtn"
                                PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx"  />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">
        _clientTxtDescription = "<%=this.txtDescription.ClientID %>";
        _clienthidParameterId = "<%=this.hidDescriptionId.ClientID %>";
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        _clientlstvwParameter = "<%=this.lstvwHomeworkTeacher.ClientID %>"

        //this function is used to clear fields.
        function ClearFields() {
            $get(_clientTxtDescription).value = "";
            $get(_clienthidParameterId).value = 0;
        }

        //this function will call when user want to delete record.
        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

    
        function ValidateParameter(oSrc, args) {
            var templateId = $get(_clienthidParameterId).value;
            var duplicate = false;
            var sName = $get(_clientTxtDescription).value;
            sName = sName.trim();

            if (sName.trim() == "") {
                oSrc.errormessage = "Description should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (sName.length > 500) {
                oSrc.errormessage = "Description length should not be greater than 500 characters.";
                args.IsValid = false;
                return true;
            }
            else {
                var iRowCount = 0;
                var Id = $get(_clientlstvwParameter + "_ctrl" + iRowCount + "_hidId");
                var Name = $get(_clientlstvwParameter + "_ctrl" + iRowCount + "_lblDescription");
                while (Name != null && Id != null) {
                    if (Name.innerHTML == sName && Id.value != templateId) {
                        duplicate = true;
                        break;
                    }
                    iRowCount = iRowCount + 1;
                    Name = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_lblDescription");
                    Id = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_hidId");
                }

                if (duplicate) {
                    oSrc.errormessage = "Description should not be duplicated.";
                    $get(_clientlblMessage).innerHTML = "";
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return true;
        }

    </script>
</asp:Content>
