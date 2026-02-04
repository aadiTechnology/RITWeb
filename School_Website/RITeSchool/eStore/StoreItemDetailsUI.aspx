<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StoreItemDetailsUI.aspx.cs" Inherits="StoreItemDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="98%">
        <tr>
            <td align="right">
                <div style="float: right;" class="LblErrorMsg" id="lblMandatoryMark" runat="server"
                    viewstatemode="Enabled">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" Text="Mandatory Fields"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:ValidationSummary ID="valErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:ValidationSummary ID="ValErrorMsgGenerate" runat="server" CssClass="ClsLabel LblErrorMsg"
                    ValidationGroup="Generate" />
            </td>
        </tr>
        <tr>
            <td id="tdMessage" runat="server" align="center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true" Height="20px"
                    Width="100%" Text="" EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td class="txtNormal" colspan="4">
                            <asp:RequiredFieldValidator ID="ReqTitle" runat="server" ErrorMessage="Title should not be blank."
                                Display="None" ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Item Code should not be duplicate." Display="None" OnServerValidate="Validate_Duplication"></asp:CustomValidator>                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Item Code should not be blank."
                                Display="None" ControlToValidate="txtItemCode"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="custValItemCode" runat="server" ErrorMessage="Item Code should not be duplicate." Display="None" OnServerValidate="Validate_Duplication"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="UOM should be selected."
                                Display="None" ControlToValidate="cmbUOM" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="MRP should not be blank."
                                Display="None" ControlToValidate="txtMRP"></asp:RequiredFieldValidator>     
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="GST should be selected."
                                Display="None" ControlToValidate="cmbGST" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqPrice" runat="server" ErrorMessage="Sale Rate should not be blank."
                                Display="None" ControlToValidate="txtPrice"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqQuantity" runat="server" ErrorMessage="Quantity should not be blank."
                                Display="None" ControlToValidate="txtQuantity"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqReorderQuantity" runat="server" ErrorMessage="ReOrder Quantity should not be blank."
                                Display="None" ControlToValidate="txtReorderQty"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularDesc" runat="server" ControlToValidate="txtDescription"
                                Display="None" ValidationExpression="^.{0,500}$" ErrorMessage="Description allowed maximum 500 characters." />
                            <asp:CustomValidator ID="CustFileUpload" runat="server" ClientValidationFunction="ValidateFileType"
                                CssClass="clsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                            <asp:CustomValidator ID="CustFileSize" runat="server" ClientValidationFunction="ValidateFileSize"
                                CssClass="clsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                            <asp:CustomValidator ID="CustClass" runat="server" ClientValidationFunction="ValidateClasses"
                                CssClass="clsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center; margin: 0px auto;">
                        <td align="center" style="text-align: center;">
                            <table align="center">
                                <tr>
                                    <td class="clsBorderLight" align="left" width="200px">
                                        <span class="ClsLabel">Store Category :</span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblStoreCategoryName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trGender" runat="server" visible="false">
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">For :</span>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdGender" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                            Width="150px">
                                            <asp:ListItem Selected="True" Text="Boy" Value="M"></asp:ListItem>
                                            <asp:ListItem Text="Girl" Value="F"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Title :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtTitle" runat="server" CssClass="ExLrgTxtBox" ViewStateMode="Enabled"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Item Code :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtItemCode" runat="server" CssClass="MidTxtBox" ViewStateMode="Enabled" MaxLength="10"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>                                        
                                        <asp:Image id="img" runat="server" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">UOM :</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbUOM" runat="server" CssClass="MidCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Description :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="ExLrgTxtBox" ViewStateMode="Enabled"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Associated Standards :</span>
                                        <asp:CheckBox ID="ChkSelectAllStd" runat="server" onclick="CheckAll(this)" />
                                    </td>
                                    <td align="left">
                                        <asp:CheckBoxList ID="ChkStandards" runat="server" RepeatDirection="Horizontal" CssClass="ClsLabel"
                                            RepeatColumns="3" onclick="CheckMain();">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Set Availability Setting :</span>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ChkAvailability" runat="server" onclick="HideFields()" />
                                    </td>
                                </tr>
                                <tr id="trStartDate" style="display: none;">
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Start Date :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="MidTxtBox" ReadOnly="true"></asp:TextBox>
                                        <rjs:PopCalendar ID="cStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                            ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid start date." />
                                    </td>
                                </tr>
                                <tr id="trEndDate" style="display: none;">
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">End Date :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="MidTxtBox" ReadOnly="true"></asp:TextBox>
                                        <rjs:PopCalendar ID="cEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                            ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid end date." />
                                    </td>
                                </tr>
                                <tr id="trVariation" runat="server" visible = "false">
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Is Variation Applicable?:</span>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="ChkVariation" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">MRP :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtMRP" runat="server" CssClass="SmlTxtBox" MaxLength="8" onblur="extractNumber(this,2,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" style="text-align:right;padding-right:5px;"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Discount in % :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="SmlTxtBox" MaxLength="6" onblur="extractNumber(this,2,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" style="text-align:right;padding-right:5px;"></asp:TextBox>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">GST :</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbGST" runat="server" CssClass="SmlCombo" >
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Sale Rate :</span>
                                    </td>
                                    <td align="left">                                       
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="SmlTxtBox" MaxLength="10" style="text-align:right;padding-right:5px;" onkeydown="return false" onpaste="return false"/>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">HSN Code :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtHSNCode" runat="server" CssClass="SmlTxtBox" MaxLength="50" />                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Available Quantity :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Re-order Quantity :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtReorderQty" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Images :</span>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="flImage" runat="server" multiple="true" ViewStateMode="Enabled"
                                            accept=".JPG, .JPEG, .BMP, .PNG" />
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <span class="LblSmlGray">(Attachment supports files of types - .JPG, .JPEG, .BMP, .PNG). Total file size should not exceed 10 MB.</span>
                                    </td>
                                </tr>
                                <tr id="trAttachments" runat="server">
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Attachments :</span>
                                    </td>
                                    <td align="left">
                                        <asp:Panel ID="AttachmentPanel" runat="server" Style="height: auto">
                                            <table id="tblAttachments" runat="server">
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center" width="90%" runat="server">
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnSave" runat="server" class="ClsBtn" Text="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnClear" runat="server" class="ClsBtn" Text="Clear" CausesValidation="false"
                                            OnClick="btnClear_Click" />
                                        <asp:Button ID="btnCancel" runat="server" class="ClsBtn" Text="Cancel" CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidFileUpload" runat="server" />
                                        <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                        <asp:HiddenField ID="HidBackUrl" runat="server" />
                                        <asp:HiddenField ID="hidDeleteedIds" runat="server" Value="" />
                                        <asp:HiddenField ID="hidAttachmentCount" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidStoreCategoryId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidAreVariotionExists" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidGSTData" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        _clientFileUploadClientId = "<%=this.flImage.ClientID%>"
        var _clienthidAttachmentCount = "<%=this.hidAttachmentCount.ClientID %>"
        _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
        _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"
        _clientChkAvailability = "<%=this.ChkAvailability.ClientID %>"
        _clientChkVariation = '<%=this.ChkVariation.ClientID %>'
        _clienthidAreVariotionExists = '<%=this.hidAreVariotionExists.ClientID %>'

        function ValidateFileType(oSrc, args) {
            var isFound = false
            var files = $('[id$=flImage]')[0].value;
            var attachentCnt = $('#' + _clienthidAttachmentCount).val();

            if (files.trim() == '' && attachentCnt == 0) {
                oSrc.errormessage = "Please select image(s) to upload.";
                args.IsValid = false;
                return true;
            }
            else if (files.trim() != '') {
                var fileList = files.split(',')
                for (var k = 0; k < fileList.length; k++) {
                    var file = fileList[k].trim()

                    var extension = file.substr(file.lastIndexOf('.')).toUpperCase()
                    if (extension != ".BMP" && extension != ".JPG" && extension != ".JPEG" && extension != ".PNG") {
                        isFound = true
                        break;
                    }
                }
            }

            if (isFound) {
                oSrc.errormessage = "Image type should be in only BMP, .JPG, .JPEG and .PNG format.";
                args.IsValid = false;
                return true;
            }


            args.IsValid = true;
            return false;
        }

        function ValidateFileSize(oSrc, args) {
            var obj = document.getElementById('<%=flImage.ClientID %>')
            var fileSize = GetFileSize(obj)

            if (fileSize >= 10485760) {
                oSrc.errormessage = "Image's total file size should be less than 10 MB."
                args.IsValid = false
                return true
            }

            args.IsValid = true;
            return false;
        }

        function GetFileSize(obj) {
            var size = 0;
            for (var k = 0; k < obj.files.length; k++) {
                size += obj.files[k].size;
            }
            return size;
        }

        function ValidateClasses(oSrc, args) {
            var isFound = false
            if ($('[id*=_ChkStandards_]:checked').length == 0) {
                oSrc.errormessage = "At least one class should be selected.";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }


        function CheckAll(obj) {
            if (obj.checked) {
                $('[id*=_ChkStandards_]').attr('checked', 'checked')
            }
            else {
                $('[id*=_ChkStandards_]').removeAttr('checked')
            }
        }

        function CheckMain() {
            if ($('[id*=_ChkStandards_]').length == $('[id*=_ChkStandards_]:checked').length)
                $('[id$=ChkSelectAllStd]').attr('checked', 'checked')
            else
                $('[id$=ChkSelectAllStd]').removeAttr('checked')
        }

        function HideAttachment(index) {
            $('[id$=hyper_' + index + ']').hide();
            $('[id$=img_' + index + ']').hide();

            var sData = $('[id$=hidDeleteedIds]').val()
            if (sData == '')
                $('[id$=hidDeleteedIds]').val(index)
            else
                $('[id$=hidDeleteedIds]').val(sData + ',' + index)

            var cnt = $('#' + _clienthidAttachmentCount).val();
            cnt = cnt - 1;
            $('#' + _clienthidAttachmentCount).val(cnt)
        }

        function HideFields() {
            var isChecked = $get(_clientChkAvailability).checked
            if (isChecked) {
                $('#trStartDate').show();
                $('#trEndDate').show();
            }
            else {
                $('#' + _clienttxtStartDate).val('')
                $('#' + _clienttxtEndDate).val('')
                $('#trStartDate').hide()
                $('#trEndDate').hide()
            }
        }

        $(document).ready(function () {
            HideFields();
        })

        function ConfirmVaritionDelete() {
            var validationResult = true;
            if (typeof (Page_ClientValidate) == 'function')
                validationResult = Page_ClientValidate("");

            if (validationResult) {
                if ($('#' + _clienthidAreVariotionExists).val() == 'Y' && $get(_clientChkVariation)!= null && $get(_clientChkVariation).checked == false) {
                    return confirm('Variation option is un-checked. This action will delete all available variations(if any). Do you want to continue?')
                }
            }
            return true;
        }

        function SetAmount() {
            var amt = $('#' + '<%=this.txtMRP.ClientID %>').val()
            var discount = $('#' + '<%=this.txtDiscount.ClientID %>').val()
            var gst = $('#' + '<%=this.cmbGST.ClientID %>').val()
            var gstPercentage = 0;

            if (gst != 0) {
                var gstRules = $('[id$=hidGSTData]').val()
                var rules = eval('[' + gstRules + ']')[0]

                var gstRule = rules.filter(function (dt) {
                    return dt.Id == gst
                })
                gstPercentage = gstRule[0].Percentage
            }
            
            if (amt == '')
                amt = '0';

            var price = 0;

            if (discount == '')
                discount = 0;

            if (gst == '' || gst == '0')
                gst = 0;

            price = (parseFloat(amt) * parseFloat(discount)) / 100
            amt = amt - price
            amt = amt + Math.round((amt * gstPercentage) / 100, 0)
            
            $('#' + '<%=this.txtPrice.ClientID %>').val(amt);
        }

    </script>
</asp:Content>
