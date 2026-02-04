<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ItemDetailsUI.aspx.cs" Inherits="ItemDetailsUI"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%" align="center">
        <tr>
            <td>
                <asp:UpdatePanel ID="UPanelItemSearch" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="2" align="center" width="100%">
                            <tr>
                                <td align="left" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" valign="top">
                                                <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                                                    ForeColor="Red" EnableViewState="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="80%" align="center">
                                        <tr>
                                            <td align="left" style="background-color: white;" valign="top">
                                                <asp:Label ID="lblErrorMsg" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                    ForeColor="Red" Height="20px" Style="text-align: left" Width="100%"></asp:Label>
                                                <asp:ValidationSummary ID="valsumItems" runat="server" CssClass="ClsLabel" ValidationGroup="Items" />
                                                <asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ClsLabel"
                                                    ForeColor="Red" meta:resourcekey="lblErrResource1"></asp:Label>
                                                 <asp:Label ID="lblError1" runat="server" ForeColor="Red" Width="100%" EnableViewState="False"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <asp:Label ID="lblError2" runat="server" ForeColor="Red" Width="100%" EnableViewState="False"
                                                    CssClass="ClsLabel"></asp:Label>
                                                <asp:Label ID="lblError3" runat="server" ForeColor="Red" Width="100%" EnableViewState="False"
                                                    CssClass="ClsLabel"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="tdMessage" runat="server" colspan="4">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                        Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Blue" Width="100%" EnableViewState="False"
                                        CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="80%" align="center">
                                        <tr>
                                            <td align="left" class="ClsBtmBorderGray" style="height: 21px">
                                                <span class="ClsLblLgnd" style="font-weight: bold">Item Details :</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="80%" align="center">
                                        <tr>
                                            <td class="ClsBorderLight" width="200px">
                                                <span class="ClsLabel" style="width: 130px">Item Name :</span>
                                            </td>
                                            <td valign="middle" width="200px">
                                                <asp:TextBox ID="txtItemName" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"
                                                    TabIndex="1"></asp:TextBox><span class="ClsMdtStar"> *</span>
                                                <asp:RequiredFieldValidator ID="reqItemName" runat="server" ControlToValidate="txtItemName"
                                                    CssClass="ClsLabel" Display="None" ErrorMessage="Item Name should not be blank."
                                                    ValidationGroup="Items"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="ClsBorderLight" width="200px">
                                                <span class="ClsLabel" style="width: 130px">Item Code :</span>
                                            </td>
                                            <td valign="middle" width="200px">
                                                <asp:TextBox ID="txtItemCode" runat="server" CssClass="ExLrgTxtBox" MaxLength="15"
                                                    TabIndex="2"></asp:TextBox><span class="ClsMdtStar"> *</span>
                                                <asp:RequiredFieldValidator ID="reqItemCode" runat="server" ControlToValidate="txtItemCode"
                                                    CssClass="ClsLabel" Display="None" ErrorMessage="Item Code should not be blank."
                                                    ValidationGroup="Items"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Unit Price :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtItemPrice" runat="server" CssClass="MidTxtBox" onblur="extractNumber(this,2,true);"
                                                    onkeyup="extractNumber(this,2,true);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="7"
                                                    Style="text-align: right; padding-right: 5px" TabIndex="3"></asp:TextBox>
                                            </td>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Unit Of Measurement :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="ddlUOM" runat="server" CssClass="ExLrgTxtBox" AutoPostBack="True"
                                                    TabIndex="4" OnSelectedIndexChanged="ddlUOM_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:CustomValidator ID="cstvalUOM" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    Visible="true" ErrorMessage="Unit Of Measurement should be selected." EnableClientScript="true"
                                                    ClientValidationFunction="ValidateItemUOM" ValidationGroup="Items"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Item Category :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="ExLrgTxtBox"
                                                    TabIndex="5">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:CustomValidator ID="cstvalCategory" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    Visible="true" ErrorMessage="Item Category should be selected." EnableClientScript="true"
                                                    ClientValidationFunction="ValidateItemCategory" ValidationGroup="Items"></asp:CustomValidator>
                                            </td>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Item Quantity in UOM :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtItemQuantity" runat="server" CssClass="ExLrgTxtBox" MaxLength="7"
                                                    TabIndex="6" onblur="extractNumber(this,2,true);" onkeyup="extractNumber(this,2,true);"
                                                    onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <asp:DropDownList ID="cmbSelectedUnitsUOMQty" runat="server"
                                                    TabIndex="7" />
                                                <span class="ClsMdtStar">*</span>
                                                <asp:CustomValidator ID="cstvalItemQuantity" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    Visible="true" ErrorMessage="Item Quantity should not be blank." EnableClientScript="true"
                                                    ClientValidationFunction="ValidateItemQuantity" ValidationGroup="Items"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Reorder Level :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtReorderLevel" runat="server" CssClass="MidTxtBox" MaxLength="7"
                                                    TabIndex="7" onblur="extractNumber(this,2,true);" onkeyup="extractNumber(this,2,true);"
                                                    onkeypress="return blockNonNumbers (this, event, true, true);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <asp:DropDownList ID="cmbSelectedUnitsReorderQty" AutoPostBack="true" runat="server"
                                                    TabIndex="9" />
                                                <span class="ClsMdtStar">*</span>
                                                <asp:CustomValidator ID="cstvalReorderLevel" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    ErrorMessage="Reorder Level should not be blank." EnableClientScript="true" ValidationGroup="Items"
                                                    ClientValidationFunction="ValidateReorderLevel" Visible="true"></asp:CustomValidator>
                                            </td>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 160px">Consider For Detail Level? :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:CheckBox ID="chkIsConsiderForDetailLevel" runat="server" TabIndex="10" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Make :</span>
                                            </td>
                                            <td colspan="1">
                                                <asp:TextBox ID="txtMake" runat="server" Width="686px" CssClass="LrgTxtBox" MaxLength="100"
                                                    TabIndex="11"></asp:TextBox>
                                            </td>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">GST Category :</span>
                                            </td>
                                            <td colspan="1">
                                                <asp:DropDownList ID="cmbGST" runat="server" CssClass="LrgCombo">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                       
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <span class="ClsLabel">Item Image 1 :</span>
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="fileUploadItems" runat="server" TabIndex="12" />
                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" ToolTip="Delete"
                                                    ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                    OnClick="btnDelete_Click" />
                                                <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" ToolTip="View"
                                                    ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                            </td>
                                            <td align="left" class="ClsBorderlight">
                                                <span class="ClsLabel">Item Image 2 :</span>
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="fileUploadItems1" runat="server" TabIndex="13" />
                                                <asp:ImageButton ID="btnDelete1" runat="server" CausesValidation="false" ToolTip="Delete"
                                                    ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                    OnClick="btnDelete1_Click" />
                                                <asp:ImageButton ID="btnView1" runat="server" CausesValidation="false" ToolTip="View"
                                                    ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span class="LblSmlGray">(Supports files of types - .BMP, .JPG, .JPEG, .PNG upto 1 mb.)</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <span class="LblSmlGray">(Supports files of types - .BMP, .JPG, .JPEG, .PNG upto 1 mb.)</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <span class="ClsLabel">Item Image 3 :</span>
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="fileUploadItems2" runat="server" TabIndex="14" />
                                                <asp:ImageButton ID="btnDelete2" runat="server" CausesValidation="false" ToolTip="Delete"
                                                    ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                    OnClick="btnDelete2_Click" />
                                                <asp:ImageButton ID="btnView2" runat="server" CausesValidation="false" ToolTip="View"
                                                    ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span class="LblSmlGray">(Supports files of types - .BMP, .JPG, .JPEG, .PNG upto 1 mb.)</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                                                    ValidationGroup="Items" ClientValidationFunction="IsFileValid"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None"
                                                    ValidationGroup="Items" ClientValidationFunction="IsFileValid1"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" Display="None"
                                                    ValidationGroup="Items" ClientValidationFunction="IsFileValid2"></asp:CustomValidator>
                                            </td>
                                        </tr>

                                         <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Hall :</span>
                                            </td>
                                            <td colspan="1">
                                                <asp:TextBox ID="txtHall" runat="server" Width="130px" CssClass="LrgTxtBox" MaxLength="100"
                                                    TabIndex="11"></asp:TextBox>
                                            </td>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Rack No:</span>
                                            </td>
                                            <td colspan="1">
                                               <asp:TextBox ID="txtRack" runat="server" Width="130px" CssClass="LrgTxtBox" MaxLength="100"
                                                    TabIndex="11"></asp:TextBox>
                                            </td>
                                           
                                        </tr>
                                        <tr>
                                         <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Shelf No:</span>
                                            </td>
                                            <td colspan="1">
                                               <asp:TextBox ID="txtShelf" runat="server" Width="130px" CssClass="LrgTxtBox" MaxLength="100"
                                                    TabIndex="11"></asp:TextBox>
                                            </td>
                                             <td class="ClsBorderLight" >
                                                <span class="ClsLabel" style="width: 130px">Vendor Name:</span>
                                            </td>
                                            <td colspan="1">
                                                <asp:DropDownList ID="cmbVendor" runat="server" CssClass="ExLrgTxtBox"
                                                    TabIndex="5">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                         <tr>
                                         <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Invoice No:</span>
                                            </td>
                                             <td colspan="1">
                                               <asp:TextBox ID="txtInvoiceNo" runat="server" Width="130px" CssClass="LrgTxtBox" MaxLength="100"
                                                    TabIndex="11"></asp:TextBox>
                                            </td>
                                            </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnAddAndContinue" runat="server" BorderStyle="Solid" CssClass="ClsBtnLrg"
                                                    TabIndex="15" Text="Add and Continue" UseSubmitBehavior="False" ValidationGroup="Items"
                                                    OnClick="btnAddAndContinue_Click" />
                                                <asp:Button ID="btnItemSave" runat="server" BorderStyle="Solid" CssClass="ClsBtnMid"
                                                    TabIndex="16" Text="Add" ValidationGroup="Items" UseSubmitBehavior="False" OnClick="btnItemSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false"
                                                    TabIndex="17" CssClass="ClsBtnMid" Text="Cancel" OnClick="btnCancel_Click" />
                                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn"
                                                    CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:HiddenField ID="hidIsNewItem" runat="server" />
                                                <asp:HiddenField ID="hidItemID" runat="server" />
                                                <asp:HiddenField ID="hidModeType" runat="server" />
                                                <asp:HiddenField ID="hidFilePath" runat="server" />
                                                <asp:HiddenField ID="hidItemName" runat="server" />
                                                <asp:HiddenField ID="hidITemCode" runat="server" />
                                                <asp:HiddenField ID="hidItemCAtegory" runat="server" />
                                                <asp:HiddenField ID="hidConsiderUnitQuantity" runat="server" />
                                                <asp:HiddenField ID="hidConsiderUnitReorderLevel" runat="server" />
                                                <asp:HiddenField ID="hidIsFromRequisitionScreen" runat="server" Value="N" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAddAndContinue" />
                        <asp:PostBackTrigger ControlID="btnItemSave" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete1" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete2" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlUOM" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientddlItemCategoryId = "<%=this.ddlCategory.ClientID %>"
        _clientddlItemUOMId = "<%=this.ddlUOM.ClientID %>"
        _clienttxtItemQuantity = "<%=this.txtItemQuantity.ClientID %>"
        _clienttxtReorderLevel = "<%=this.txtReorderLevel.ClientID %>"
        _clientcstvalItemQuantity = "<%=this.cstvalItemQuantity.ClientID %>"
        _clientcstvalReorderLevel = "<%=this.cstvalReorderLevel.ClientID %>"
        _ClientfileUploadItems = "<%=this.fileUploadItems.ClientID %>"; //upload    hidFilePath
        _ClientfileUploadItems1 = "<%=this.fileUploadItems1.ClientID %>";
        _ClientfileUploadItems2 = "<%=this.fileUploadItems2.ClientID %>";
        _ClientlblSuccess = "<%=this.lblSuccess.ClientID %>"; //upload
        _ClientlblError1 = "<%=this.lblError1.ClientID %>"; //upload
        _ClientlblError2 = "<%=this.lblError2.ClientID %>"; //upload
        _ClientlblError3 = "<%=this.lblError3.ClientID %>"; //upload
        _clientlblErr = "<%=this.lblErr.ClientID %>"
        function ValidateItemCategory(oSrc, args) {
            ddlItemCategory = document.getElementById(_clientddlItemCategoryId)
            if (ddlItemCategory.value == "0") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function ValidateItemUOM(oSrc, args) {
            ddlUOM = document.getElementById(_clientddlItemUOMId)
            if (ddlUOM.value == "0") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function ValidateItemQuantity(oSrc, args) {
            txtItemQuantity = document.getElementById(_clienttxtItemQuantity)
            var sItemQuantity = trimAll(txtItemQuantity.value)
            if (sItemQuantity == '') {
                document.getElementById(_clientcstvalReorderLevel).innerHTML = "Item Quantity should not be blank."
                document.getElementById(_clientcstvalItemQuantity).ErrorMessage = "Item Quantity should not be blank."
                args.IsValid = false
                return true
            }
            else if (sItemQuantity == '.') {
                document.getElementById(_clientcstvalReorderLevel).innerHTML = "Insert valid Reorder level."
                document.getElementById(_clientcstvalItemQuantity).ErrorMessage = "Insert valid Item Quantity."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ValidateReorderLevel(oSrc, args) {
            txtReorderLevel = document.getElementById(_clienttxtReorderLevel)
            var sReorderLevel = trimAll(txtReorderLevel.value)
            if (sReorderLevel == '') {
                document.getElementById(_clientcstvalReorderLevel).innerHTML = "Reorder level should not be blank."
                document.getElementById(_clientcstvalReorderLevel).ErrorMessage = "Reorder level should not be blank."
                args.IsValid = false
                return true
            }
            else if (sReorderLevel == '.') {
                document.getElementById(_clientcstvalReorderLevel).innerHTML = "Insert valid Reorder level."
                document.getElementById(_clientcstvalReorderLevel).ErrorMessage = "Insert valid Reorder level."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ClearErrorLabels() {

            if ($get("<%=this.lblMessage.ClientID %>") != null)
                $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
        }

        //This function is used to validate is file uploaded by user or not

        function IsFileValid(oSrc, args) {
            var myImage = document.getElementById(_ClientfileUploadItems).value;

            if (myImage == "" || myImage == null) {
                args.IsValid = true
                return false;
            }
            else {
                var ext = myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase()
                if (ext == ".PNG" || ext == ".JPG" || ext == ".JPEG" || ext == ".BMP") {
                    args.IsValid = true
                    return false;
                }
                else {
                    oSrc.errormessage = 'Invalid file type for Item Image 1.'
                    args.IsValid = false
                    return true;
                }
            }
        }

        function ClearMessages() {
            if (document.getElementById(_ClientlblSuccess)) {
                document.getElementById(_ClientlblSuccess).innerHTML = "";
                document.getElementById(_ClientlblSuccess).innerText = "";
            }

            if (document.getElementById(_ClientlblError1)) {
                document.getElementById(_ClientlblError1).innerHTML = "";
                document.getElementById(_ClientlblError1).innerText = "";
            }

            if (document.getElementById(_ClientlblError2)) {
                document.getElementById(_ClientlblError2).innerHTML = "";
                document.getElementById(_ClientlblError2).innerText = "";
            }

            if (document.getElementById(_ClientlblError3)) {
                document.getElementById(_ClientlblError3).innerHTML = "";
                document.getElementById(_ClientlblError3).innerText = "";
            }

            if (document.getElementById(_clientlblErr)) {
                document.getElementById(_clientlblErr).innerHTML = "";
                document.getElementById(_clientlblErr).innerText = "";
            }
        }

        function IsFileValid1(oSrc, args) {
            var myImage = document.getElementById(_ClientfileUploadItems1).value;

            if (myImage == "" || myImage == null) {
                args.IsValid = true
                return false;
            }
            else {
                var ext = myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase()
                if (ext == ".PNG" || ext == ".JPG" || ext == ".JPEG" || ext == ".BMP") {
                    args.IsValid = true
                    return false;
                }
                else {
                    oSrc.errormessage = 'Invalid file type for Item Image 2.'
                    args.IsValid = false
                    return true;
                }
            }
        }

        function IsFileValid2(oSrc, args) {
            var myImage = document.getElementById(_ClientfileUploadItems2).value;

            if (myImage == "" || myImage == null) {
                args.IsValid = true
                return false;
            }
            else {
                var ext = myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase()
                if (ext == ".PNG" || ext == ".JPG" || ext == ".JPEG" || ext == ".BMP") {
                    args.IsValid = true
                    return false;
                }
                else {
                    oSrc.errormessage = 'Invalid file type for Item Image 3.'
                    args.IsValid = false
                    return true;
                }
            }
        }

        function OpenWindow(sfilepath) {
            window.open(sfilepath, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
            return false;
        }

        function ConfirmDelete() {
            return window.confirm('Are you sure you want to delete current Image file?')
        }

        function ResetLabel() {
            if (document.getElementById(_clientlblErr) != null)
                document.getElementById(_clientlblErr).innerHTML = "";
        }

        function ClosePopup() {            
            _clienthidIsFromRequisitionScreen = "<%=this.hidIsFromRequisitionScreen.ClientID %>";
            var IsFromRequisitionScreen = document.getElementById(_clienthidIsFromRequisitionScreen).value;
            if (IsFromRequisitionScreen == "Y") {
                window.close();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
