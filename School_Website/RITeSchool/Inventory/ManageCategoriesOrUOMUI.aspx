<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="ManageCategoriesOrUOMUI.aspx.cs" Inherits="ManageCategoriesOrUOMUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%" style="vertical-align: top">
        <tr valign="top">
            <td class="ClsGrayMainTitle" valign="top" align="left">
                <span class="MainTitleHead">Manage Category/Unit of Measurement</span>
            </td>
            <td style="height: 10px;">
            </td>
        </tr>
    </table>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr align="center">
                <td align="center">
                    <asp:ValidationSummary ID="valSummary" CssClass="LblErrorMsg" runat="server" />
                    <asp:Label ID="lblMand" runat="server" ForeColor="Red" CssClass="floatR">* Mandatory Fields</asp:Label>
                </td>
            </tr>
            <tr id="trMessage" runat="server" align="center">
                <td align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="False" Text="" CssClass="LblErrorMsg"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Blue" Text="" CssClass="LblNormalImg"
                     EnableViewState="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 100%;">
                    <table width="75%" align="center">
                        <tr>
                            <td align="center" class="ClsBorderlight">
                                <asp:RadioButton ID="optItemCategory" runat="server" Text="Item Category" Style="font-family: Arial;
                                    font-size: 9pt" AutoPostBack="true" OnCheckedChanged="optItemCategory_CheckedChanged"
                                    GroupName="ItemCategoryOrUOM" />
                            </td>
                            <td align="center" class="ClsBorderlight">
                                <asp:RadioButton ID="optUOM" runat="server" Text="Unit of Measurement" Style="font-family: Arial;
                                    font-size: 9pt" AutoPostBack="true" OnCheckedChanged="optUOM_CheckedChanged"
                                    GroupName="ItemCategoryOrUOM" TabIndex="1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 936px">
                    &nbsp;
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <table align="center" id="tblName" border="0" style="width: 100%;">
                        <tr>
                            <td colspan="2">
                                <table align="center">
                                    <tr>
                                        <td align="left" class="ClsBorderlight" style="width: 150px;">
                                            <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text="" EnableViewState="False"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtName" runat="server" CssClass="MidTxtBox" MaxLength="20" 
                                                TabIndex="2"></asp:TextBox>
                                            <span class="ClsMdtStar" runat="server" id="lblStar">* </span>
                                            <asp:CustomValidator ID="cstCategoriesOrUOM" runat="server" ClientValidationFunction="CheckIsEmpty"
                                                SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr id="DisplayCount" runat="server">
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblPieceCount" runat="server" CssClass="ClsLabel" Text="Unit Count"
                                                EnableViewState="False"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtPieceCount" runat="server" CssClass="MidTxtBox" MaxLength="4"
                                                onkeypress="return blockNonNumbers(this, event, false, false);" 
                                                TabIndex="3"></asp:TextBox>
                                            <span class="ClsMdtStar" runat="server" id="Span1">* </span>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateUnitCount"
                                                SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="center">
                    <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="Save" BorderWidth="1px"
                        disable-page="true" Height="24px" OnClick="btnSave_Click" TabIndex="4"></asp:Button>
                    <asp:Button CssClass="ClsBtn" ID="btnCancel" runat="server" Text="Cancel" BorderWidth="1px"
                        Height="24px" OnClick="btnCancel_Click" CausesValidation="False" 
                        TabIndex="5"></asp:Button>
                </td>
            </tr>
        </table>
        </td> </tr>
        <tr>
            <td align="center" style="width: 100%;">
                <table width="65%" align="center">
                    <tr id="trListView">
                        <td id="tdListView" align="center">
                            <asp:ListView ID="lstvwCategoriesOrUOM" runat="server" DataKeyNames="UOMID,ItemCategoryID,IsUsed"
                                OnItemCommand="lstvwCategoriesOrUOM_ItemCommand" OnItemDataBound="lstvwCategoriesOrUOM_ItemDataBound">
                                <LayoutTemplate>
                                    <table width="100%" runat="server" id="tblCategoriesOrUOM" style="color: #333333"
                                        cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="left" style="padding-left: 9px;">
                                                <asp:Label ID="lblCategoriesOrUOMName" runat="server" CausesValidation="false" ForeColor="Black"> Name </asp:Label>
                                            </th>
                                            <th align="right" style="padding-right: 9px; width: 30%;" id="thPieceCount">
                                                <asp:Label ID="lblPieceCount" runat="server" CausesValidation="false" ForeColor="Black"> Unit Count </asp:Label>
                                            </th>
                                            <th align="center" style="width: 30px">
                                                Edit
                                            </th>
                                            <th align="center" style="width: 30px">
                                                Delete
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </td>
                                        <td align="right" class="paddingLR" id="tdPieceCount">
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("PieceCount") %>'></asp:Label>
                                        </td>
                                        <td align="center" style="width: 30px">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATE_CATEGORIES_UOM"
                                                ImageUrl="../images/IconGrid_Edit.GIF" />
                                        </td>
                                        <td align="center" style="width: 30px">
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="REMOVE"
                                                ImageUrl="../images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <td class="paddingL" align="left">
                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </td>
                                        <td class="paddingLR" align="right" id="tdPieceCount">
                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("PieceCount") %>'></asp:Label>
                                        </td>
                                        <td align="center" style="width: 30px">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATE_CATEGORIES_UOM"
                                                ImageUrl="../images/IconGrid_Edit.GIF" />
                                        </td>
                                        <td align="center" style="width: 30px">
                                            <asp:ImageButton ID="imgBtnDelete" CommandName="REMOVE" CausesValidation="false"
                                                runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                            <asp:HiddenField ID="hidMode" runat="server" Value="New" />
                            <asp:HiddenField ID="hidUOMId" runat="server" />
                            <asp:HiddenField ID="hidCategoryId" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button CssClass="ClsBtn" ID="btnClose" runat="server" Text="Close" BorderWidth="1px"
                                Height="24px" CausesValidation="False" TabIndex="6"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientlstvwCategoriesOrUOM = "<%=this.lstvwCategoriesOrUOM.ClientID %>"
        _clientcstCategoriesOrUOM = "<%=this.cstCategoriesOrUOM.ClientID %>"
        _clienttxtName = "<%=this.txtName.ClientID %>"
        _clientlblName = "<%=this.lblName.ClientID %>"
        _clienttrErrorMessage = "<%=this.trMessage.ClientID %>"
        _clientlblUpdateSucess = "<%=this.lblMessage.ClientID %>"
        _clienttxtPieceCount = "<%=this.txtPieceCount.ClientID %>"


        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }
        function CheckIsEmpty(oSrc, args) {
            var errMsg = document.getElementById(_clienttrErrorMessage)
            if (errMsg != null)
                errMsg.style.display = "none"
            var sMessage = ""
            var txtName = document.getElementById(_clienttxtName)
            var sCategoryOrUOM = document.getElementById(_clientlblName).innerHTML
            if (txtName.value.trim() == "") {
                sMessage = sCategoryOrUOM.substring(0, sCategoryOrUOM.lastIndexOf(":"));
            }
            if (sMessage != "") {
                $get(_clientcstCategoriesOrUOM).errormessage = sMessage + " should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).innerHTML = "";
                document.getElementById(_clientlblUpdateSucess).style.display = "none";

            }
        }

        function ValidateUnitCount(oSrc, args) {
            var pieceCount = $('#' + _clienttxtPieceCount).val()
            if (pieceCount == "" || parseInt(pieceCount) == 0) {
                oSrc.errormessage = "Unit Count should not be blank or zero.";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

    </script>
</asp:Content>
