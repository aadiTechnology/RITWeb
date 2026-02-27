<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BulkDocumentUploadDetailsUI.aspx.cs" Inherits="BulkDocumentUploadDetailsUI" %>

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
                    <asp:Label ID="lblMandatoryFields" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                            <asp:ValidationSummary ID="valSumFilter" runat="server" CssClass="ClsLabel" ShowSummary="true"
                                ValidationGroup="FILTER" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Document should be selected."
                                ControlToValidate="ddlDocuments" InitialValue="0" Display="None" ValidationGroup="FILTER"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Document should be selected."
                                ControlToValidate="ddlDocuments" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="ValidateSelection"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator7" runat="server" ErrorMessage="" ClientValidationFunction="ValidateAction"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" ClientValidationFunction="ValidateEmptyTitle"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" ClientValidationFunction="ValidateEmptyStartDate"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="custValEndDate" runat="server" ErrorMessage="" ClientValidationFunction="ValidateEmptyEndDate"
                                Display="None"></asp:CustomValidator>

                            <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="" ClientValidationFunction="ValidateStartEndDate"
                                Display="None"></asp:CustomValidator>

                            <asp:CustomValidator ID="custValAmount" runat="server" ErrorMessage="" ClientValidationFunction="ValidateEmptyAmount"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="custValPolicyNo" runat="server" ErrorMessage="" ClientValidationFunction="ValidateEmptyPolicyNumber"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator9" runat="server" ErrorMessage="" ClientValidationFunction="ValidateDuplicatePolicyNumber"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="" ClientValidationFunction="ValidateDescription"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="" ClientValidationFunction="ValidateUploadedFiles"
                                Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator8" runat="server" ErrorMessage="" OnServerValidate="DateOverlapping_Validate"
                                Display="None"></asp:CustomValidator>
                            <%--<asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="DocumentDate_Validate"
                                CssClass="ClsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" OnServerValidate="DocumentTitle_Validate"
                                CssClass="ClsLabel" Display="None" ErrorMessage="Title should not be duplicate for selected document."></asp:CustomValidator>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblUpdateSuccess" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                    CssClass="ClsLabel" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr align="center" style="text-align: center; margin: 0px auto;">
            <td align="center" style="text-align: center;">
                <table align="center">
                  <tr>
                        <td class="ClsBorderLight" align="left">
                            <asp:Label ID="lblDocuments" runat="server" Text="Document: " CssClass="ClsLabel"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDocuments" runat="server" CssClass="MidCombo">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">*</span>
                        </td>
                        <td class="ClsBorderLight" align="left">
                            <asp:Label ID="Label1" runat="server" Text="Vehicle Number: " CssClass="ClsLabel"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="LrgTxtBox" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkShowAll" runat="server" Text="Show All?" />
                        </td>
                        <td style="width: 100px;">
                            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" ValidationGroup="FILTER"
                                OnClick="btnShow_Click" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="tr1" runat="server">
            <td align="center">
                <table id="tblDetails" width="100%" runat="server">
                    <tr align="center">
                        <td align="center">
                            <table width="98%">
                             <tr>
                               <td align="left">
                                        <table id="LegendTable" runat="server">
                                            <tr>
                                                <td align="left" width="55px" valign="middle">
                                                    <span class="ClsLblLgnd">Legend : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" style="padding-right: 5px;padding-left:5px;"
                                                        TabIndex="3" ForeColor="Navy" Font-Bold="true" ReadOnly="True" Text="EXPIRING SOON"></asp:Label>
                                                </td>  
                                                <td align="left">
                                                    <asp:Label ID="Label2" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" style="padding-right: 5px;padding-left:5px;"
                                                        TabIndex="3" ForeColor="Red" Font-Bold="true" ReadOnly="True" Text="EXPIRED"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                              </tr>
                                <tr id="Tr2" runat="server">
                                    <td align="center">
                            
                                        <asp:ListView ID="lstvwBulkDocumentDetails" runat="server" ViewStateMode="Enabled"
                                            DataKeyNames="Id, VehicleId, FileName" OnItemDataBound="lstvwBulkDocumentDetails_ItemDataBound"
                                            OnDataBound="lstvwBulkDocumentDetails_DataBound" OnItemCommand="lstvwBulkDocumentDetails_ItemCommand">
                                            <LayoutTemplate>
                                                <table id="tbldocdetails" runat="server" align="center" style="color: #333333" cellpadding="0"
                                                    cellspacing="1" class="GridBorder" width="100%">
                                                    <tr id="TrHeader" runat="server" class="ClsGridHeader">
                                                        <th id="thchk" runat="server" align="center" width="5%">
                                                        </th>
                                                        <th style="width:50px;" align="center">
                                                            <asp:Label ID="Label3" runat="server" Text="Sr. No." style="float:inherit"></asp:Label>
                                                        </th>
                                                        <th class="paddingLR" align="left" style="width: 150px;">
                                                            <asp:Label ID="lblVehicleNo" runat="server" Text="Vehicle Number"></asp:Label>
                                                        </th>
                                                        <th class="paddingLR" align="left" style="width: 100px;">
                                                            <asp:Label ID="Label2" runat="server" Text="Action"></asp:Label>
                                                        </th>                                                        
                                                        <th class="paddingLR" align="left">
                                                            <asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label>
                                                        </th>
                                                        <th class="paddingLR" align="left" style="width: 125px">
                                                            <asp:Label ID="lblStartDate" runat="server" Text="Start Date"></asp:Label>
                                                        </th>
                                                        <th class="paddingLR" align="left" style="width: 125px" id="thEndDate" runat="server">
                                                            <asp:Label ID="lblEndDate" runat="server" Text="End date"></asp:Label>
                                                        </th>
                                                        <th class="paddingLR" align="left" style="width: 65px" id="thAmount" runat="server"
                                                            visible="false">
                                                            <asp:Label ID="lblAmount" runat="server" Text="Amount"></asp:Label>
                                                        </th>
                                                        <th class="paddingLR" align="left" id="thPolicyNo" runat="server" visible="false">
                                                            <asp:Label ID="lblPolicyNo" runat="server" Text="Policy No"></asp:Label>
                                                        </th>
                                                        <th class="paddingLR" align="left">
                                                            <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                                        </th>
                                                        <th class="paddingLR" align="left" width="50px">
                                                            Upload Document
                                                        </th>
                                                        <th align="center" width="50px">
                                                            View
                                                        </th>
                                                        <th align="center" width="50px">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr class="ClsGridHeader">
                                                        <th>
                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckUncheckAll();" />
                                                        </th>
                                                        <th>
                                                        </th>
                                                        <th>
                                                        </th>
                                                        <th>
                                                            <asp:DropDownList ID="cmbActionAll" runat="server" CssClass="SmlCombo" onchange="SetAction(this);">
                                                            <asp:ListItem Text="-- Select --" Value = "0"></asp:ListItem>
                                                            <asp:ListItem Text="New Record" Value = "1"></asp:ListItem>
                                                            <asp:ListItem Text="Update" Value = "2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </th>                                                        
                                                        <th>
                                                            <asp:TextBox ID="txtTitleAll" runat="server" Width="98%" onchange="SetTitleField(this,'txtTitle'); return false;"></asp:TextBox>
                                                        </th>
                                                        <th>
                                                            <asp:TextBox ID="txtStartDateAll" runat="server" Width="75%" Text='<%#Eval("StartDate") %>'
                                                                onchange="SetTitleField(this,'txtStartDate'); return false;"></asp:TextBox>
                                                            <rjs:PopCalendar ID="CalStartDate" runat="server" Control="txtStartDateAll" Format="dd MMM yyyy"
                                                                ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid Start date." />
                                                        </th>
                                                        <th id="thEndDateRow" runat="server" visible="false">
                                                            <asp:TextBox ID="txtEndDateAll" runat="server" Width="75%" Text='<%#Eval("EndDate") %>'
                                                                onchange="SetTitleField(this,'txtEndDate'); return false;"></asp:TextBox>
                                                            <rjs:PopCalendar ID="CalEndDate" runat="server" Control="txtEndDateAll" Format="dd MMM yyyy"
                                                                ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid End date." />
                                                        </th>
                                                        <th>
                                                        </th>
                                                        <th>
                                                        </th>
                                                        <th>
                                                        </th>
                                                        <th>
                                                        </th>
                                                        <th>
                                                        </th>
                                                        <th>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trItemtemplates" runat="server" class="ClsGridRow">
                                                    <td id="tdchk" runat="server" align="center">
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRowNo" runat="server" Text="" style="float:inherit;" CssClass="clsLabel"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVehicleNumber" runat="server" Text='<%#Eval("VehicleNumber") %>'
                                                            CssClass="ClsLabel" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbAction" runat="server" CssClass="SmlCombo">
                                                        <asp:ListItem Text="-- Select --" Value = "0"></asp:ListItem>
                                                        <asp:ListItem Text="New Record" Value = "1"></asp:ListItem>
                                                        <asp:ListItem Text="Update" Value = "2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>                                                    
                                                    <td align="center">
                                                        <asp:TextBox ID="txtTitle" runat="server" Width="98%" Text='<%#Eval("Title") %>'></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtStartDate" runat="server" Width="75%" Text='<%#Eval("StartDate") %>'></asp:TextBox>
                                                        <rjs:PopCalendar ID="CalStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                            ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid Start date." />
                                                    </td>
                                                    <td align="center" id="tdEndDate" runat="server">
                                                        <asp:TextBox ID="txtEndDate" runat="server" Width="75%" Text='<%#Eval("EndDate") %>'></asp:TextBox>
                                                        <rjs:PopCalendar ID="CalEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                            ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid End date." />
                                                    </td>
                                                    <td align="center" id="tdAmount" runat="server" visible="false">
                                                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" Width="80%" Text='<%#Eval("Amount") %>'>                                                                
                                                        </asp:TextBox>
                                                    </td>
                                                    <td id="tdPolicyNo" runat="server" visible="false">
                                                        <asp:TextBox ID="txtPolicyNo" runat="server" Width="80%" MaxLength="20" Text='<%#Eval("PolicyNo") %>'>                                                                
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="300" Width="98%" TextMode="MultiLine"
                                                            Text='<%# Eval("Description") %>'>                                                                
                                                        </asp:TextBox>
                                                    </td>
                                                    <td class="paddingLR" align="left">
                                                        <asp:FileUpload ID="flDocument" runat="server" />
                                                        <asp:HiddenField ID="hidDocFile" runat="server" Value="" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgbtnView" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ToolTip="View" CommandName="DOWNLOAD" CausesValidation="false" Visible="false"
                                                            ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="DeleteDocumentDetails"
                                                            ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.gif" Visible="true" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <asp:HiddenField ID="hidDocumentId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidVehicleNo" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidShowAll" runat="server" Value="N" />
                                    </td>
                                </tr>
                                <tr id="trNote" runat="server" visible="false">
                                    <td align="left">
                                        <span class="LblSmlGray">(Attachment supports files of types - .BMP, .JPG, .JPEG, .PDF,
                                            .PNG upto total size less than 10 MB.)</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                    class="ClsBtn" OnClick="btnSave_Click" Visible="False" />
                <asp:Button ID="btnExport" runat="server" Text="Export" class="ClsBtn" Visible="False"
                    CausesValidation="False" OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        _clientlstvwBulkDocumentDetails = "<%=this.lstvwBulkDocumentDetails.ClientID %>"

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function OpenDocument(FilePath) {
            window.open(FilePath, '_new');
            return false;
        }

        function CheckFileType(sFileName) {
            var bIsValid;
            if (sFileName != "") {
                var extension = sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase();
                if (extension == ".PDF" || extension == ".JPEG" || extension == ".JPG" || extension == ".PNG" || extension == ".BMP")
                    bIsValid = true;
                else
                    bIsValid = false;
            }
            else
                bIsValid = true;

            return bIsValid;
        }

        function ResetMessage() {
            if ($get("<%=this.lblUpdateSuccess.ClientID %>") != null)
                $get("<%=this.lblUpdateSuccess.ClientID %>").innerHTML = "";
        }

        function CheckUncheckAll() {
            var checkAll = document.getElementById(_clientlstvwBulkDocumentDetails + "_ChkSelectAll").checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwBulkDocumentDetails + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwBulkDocumentDetails + "_ctrl" + iRowCount + "_chkSelect")
            }
        }

        function ValidateSelection(oSrc, args) {
            if ($('[id$=_chkSelect]:checked').length == 0) {
                oSrc.errormessage = 'At least one vehicle should be selected to save details.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateEmptyTitle(oSrc, args) {
            var invalidRowNo = ValidateFields('_txtTitle')
            if (invalidRowNo.length > 0) {
                oSrc.errormessage = 'Title should not be blank for serial number(s) - ' + invalidRowNo + '.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateEmptyStartDate(oSrc, args) {
            var invalidRowNo = ValidateFields('_txtStartDate')
            if (invalidRowNo.length > 0) {
                oSrc.errormessage = 'Start Date should not be blank for serial number(s) - ' + invalidRowNo+'.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateEmptyEndDate(oSrc, args) {
            var invalidRowNo = ValidateFields('_txtEndDate')
            if (invalidRowNo.length) {
                oSrc.errormessage = 'End Date should not be blank for serial number(s) - ' + invalidRowNo+'.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateStartEndDate(oSrc, args) {
            var isFound = false;
            var invalidRowNo = ''
            $('[id$=_chkSelect]:checked').each(function () {
                var id = this.id;

                var startDate = $('#' + this.id.replace('_chkSelect', '_txtStartDate')).val()
                var endDate = $('#' + this.id.replace('_chkSelect', '_txtEndDate')).val()
                var rowNo = $('#' + this.id.replace('_chkSelect', '_lblRowNo')).html()

                if (startDate != '' && endDate != '' && endDate != undefined) {
                    var stDate;
                    if (document.all)
                        stDate = new Date(startDate.replace('-', ' '));
                    else
                        stDate = new Date(convertdate(startDate));

                    var edDate;
                    if (document.all)
                        edDate = new Date(endDate.replace('-', ' '));
                    else
                        edDate = new Date(convertdate(endDate));

                    if (edDate <= stDate) {                    
                        isFound = true;
                        invalidRowNo = invalidRowNo + ',' + rowNo;
                    }
                }
            })

            if (isFound) {
                invalidRowNo = invalidRowNo.substring(1);
                oSrc.errormessage = 'End date should be greater than Start Date for serial number(s) - ' + invalidRowNo+'.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }
        
        function ValidateEmptyAmount(oSrc, args) {
         var inValidRowNo = ValidateNumericField('_txtAmount')
         if (inValidRowNo.length > 0) {
             inValidRowNo = inValidRowNo.substring(1);
             oSrc.errormessage = 'Amount should not be blank or zero for serial number(s) - ' + inValidRowNo+'.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateEmptyPolicyNumber(oSrc, args) {
        var invalidRowNo = ValidateNumericField('_txtPolicyNo')
        if (invalidRowNo.length > 0) {
            invalidRowNo = invalidRowNo.substring(1);
            oSrc.errormessage = 'Policy Number should not be blank or zero for serial number(s) - ' + invalidRowNo+'.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateDuplicatePolicyNumber(src, args) {
            var invalidRowNo = ''
            $('[id$=_txtPolicyNo]').each(function () {
                var chkId = this.id.replace('_txtPolicyNo', '_chkSelect')
                var rowNo = $('#' + this.id.replace('_txtPolicyNo', '_lblRowNo')).html()
                var policyNo = $(this).val()

                if ($('#' + chkId).prop('checked')) {
                    if ($('[ID$=_txtPolicyNo]').filter(function (index) { return $(this).val() == policyNo }).length > 1) {
                        invalidRowNo = invalidRowNo + ',' + rowNo;
                    }
                }
            })

            if (invalidRowNo.length > 0) {
                invalidRowNo = invalidRowNo.substring(1)
                src.errormessage = 'Policy Number should not be duplicate for serial number(s) - ' + invalidRowNo + '.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }
        
        function ValidateDescription(src, args) {
            var isFound = false;
            var invalidRowNo = ''
            $('[id$=_chkSelect]:checked').each(function () {
                var id = this.id;
                var newId = this.id.replace('_chkSelect', '_txtDescription')
                var rowNo = $('#' + this.id.replace('_chkSelect', '_lblRowNo')).html()

                if ($('#' + newId).val().length > 500) {
                    isFound = true;
                    invalidRowNo = invalidRowNo + ',' + rowNo;
                }
            })

            if (isFound) {
                invalidRowNo = invalidRowNo.substring(1)
                src.errormessage = 'Description length should not be more than 500 for serial number(s) - ' + invalidRowNo+'.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateUploadedFiles(src, args) {
            var isFound = false;
            isInvalidExtensionFound = false;
            totalFileSize = 0;

            var blankRowNos = ''
            var invalidRowNos = ''

            $('[id$=_chkSelect]:checked').each(function () {
                var id = this.id;
                var newId = this.id.replace('_chkSelect', '_flDocument')
                var extFile = this.id.replace('_chkSelect', '_hidDocFile')
                var action = this.id.replace('_chkSelect', '_cmbAction')
                var rowNo = $('#'+this.id.replace('_chkSelect', '_lblRowNo')).html()

                if ($('#' + newId).val() == '' && ($('#' + extFile).val() == '' || $('#' + action).val() == '1')) {
                    blankRowNos = blankRowNos + ',' + rowNo
                    isFound = true;
                }
                else {
                    var isValid = CheckFileType($('#' + newId).val())
                    if (isValid == false) {                        
                        invalidRowNos = invalidRowNos + ',' + rowNo;
                        isInvalidExtensionFound = true;
                    }
                    else {
                        if ($('#' + newId)[0].files.length > 0)
                            totalFileSize += $('#' + newId)[0].files[0].size;
                    }
                }
            })

            if (isFound) {
                blankRowNos = blankRowNos.substring(1)
                src.errormessage = 'File should be uploaded for serial number(s)-' + blankRowNos+'.'
                args.IsValid = false;
                return true;
            }
            else if (isInvalidExtensionFound) {
                invalidRowNos = invalidRowNos.substring(1)
                src.errormessage = 'File extension should be only from only suggested types for serial number(s) - ' + invalidRowNos+'.'
                args.IsValid = false;
                return true;
            }
            else if (totalFileSize > 10485760) {
                src.errormessage = 'Total size of uploaded file should not exceed 10 MB for selected vehicles.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateNumericField(txt) {
            var isFound = false;
            var invalidRowNo = ''
            $('[id$=_chkSelect]:checked').each(function () {
                var id = this.id;
                var newId = this.id.replace('_chkSelect', txt)
                var rowNo = $('#' + this.id.replace('_chkSelect', '_lblRowNo')).html()

                if ($('#' + newId).val() == '' || $('#' + newId).val() == '0') {
                    isFound = true;
                    invalidRowNo = invalidRowNo + ',' + rowNo;
                }
            })
            return invalidRowNo;
        }


        function ValidateAction(src, args) {
            var isFound = false;
            var invalidRowNo = ''
            $('[id$=_chkSelect]:checked').each(function () {
                var id = this.id;
                var newId = this.id.replace('_chkSelect', '_cmbAction')
                var rowNo = $('#' + this.id.replace('_chkSelect', '_lblRowNo')).html()

                if ($('#' + newId).val() == '0') {
                    isFound = true;
                    invalidRowNo = invalidRowNo + ',' + rowNo;
                }
            })

            if (isFound) {
                invalidRowNo = invalidRowNo.substring(1);
                src.errormessage = 'Action should be selecteds for serial number(s) - ' + invalidRowNo+'.'
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateFields(txt) {
            var isFound = false;
            var invalidRowNo = ''
            $('[id$=_chkSelect]:checked').each(function () {
                var id = this.id;
                var newId = this.id.replace('_chkSelect', txt)
                var rowNo = $('#' + this.id.replace('_chkSelect', '_lblRowNo')).html()
                if ($('#' + newId).val() == '') {
                    isFound = true;
                    invalidRowNo = invalidRowNo + ',' + rowNo;
                }
            })


            if (invalidRowNo.length > 0) {
                invalidRowNo = invalidRowNo.substring(1);
            }

            return invalidRowNo;
        }

        function SetTitleField(obj, field) {
            var headerVal = $(obj).val()
            $('[id$=_chkSelect]:checked').each(function () {
                var id = this.id;
                var newId = this.id.replace('_chkSelect', '_' + field)
                $('#' + newId).val(headerVal)
            })
            return false;
        }

        function SetAction(obj) {
            var headerVal = $(obj).val()
            $('[id$=_chkSelect]:checked').each(function () {
                var id = this.id;
                var newId = this.id.replace('_chkSelect', '_' + 'cmbAction')
                $('#' + newId).val(headerVal)
            })
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
