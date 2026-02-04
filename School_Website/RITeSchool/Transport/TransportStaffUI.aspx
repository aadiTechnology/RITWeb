<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransportStaffUI.aspx.cs" Inherits="TransportStaffUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>               
                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                    vertical-align: top">
                    <tr>
                        <td id="MainDataTable" align="center">
                            <!-- Data Insert Here -->
                            <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 77%">
                                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                            Height="20px" Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                                                        ValidationGroup="Save" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1" class="ClsTextNormal" align="center">
                                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                        <!-- User InfoTable starts here -->
                                        <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                            style="width: 55%; margin-left: 19px;">
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 50%">                                                   
                                                    <span class="ClsLabel">Name :</span>                                                   
                                                    <span class="LblSmlGray floatR">(First Name)</span>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 50%; margin-left: 40px;">
                                                    &nbsp;<asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" Width="50px">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="MidTxtBox" onblur="formatName(this)" Width="186px"></asp:TextBox>
                                                    *&nbsp;
                                                    <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtFirstName"
                                                        Display="None" ErrorMessage="First Name should not be blank." ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 46%">                                                    
                                                    <span class="LblSmlGray floatR">(Middle Initial)</span>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                    <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="50" onblur="formatName(this)"
                                                        Width="186px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 46%">                                                   
                                                    <span class="LblSmlGray floatR">(Last Name)</span>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="MidTxtBox" onblur="formatName(this)"></asp:TextBox>
                                                    *<asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtLastName"
                                                        Display="None" ErrorMessage="Last Name should not be blank." ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                             <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <span class="ClsLabel">Date Of Birth :</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtCalDobPopup" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"></asp:TextBox>
                                                <rjs:PopCalendar ID="CalDobPopup" runat="server" Control="txtCalDobPopup" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                    To-Today="true"/>
                                                <span style="color: #ff0000">&nbsp; *</span>                                                
                                            </td>
                                        </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 46%">                                                   
                                                    <span class="ClsLabel">Address :</span>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                    <asp:TextBox ID="txtAddress" CssClass="MidTxtBox" runat="server" MaxLength="200"
                                                        TextMode="MultiLine" Height="54px" />
                                                        <span style="color: red">* </span>
                                                    <asp:CustomValidator ID="cstValAddress" runat="server" 
                                                    ClientValidationFunction="validateAddress" CssClass="ClsMdtStar" 
                                                    Display="None" EnableClientScript="true" ErrorMessage="Error msg" 
                                                    ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 46%">                                                   
                                                    <span class="ClsLabel">Mobile No.:</span>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                    <asp:TextBox ID="txtMobileNo" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    *<asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        Visible="true" ErrorMessage="Mobile No. should be of 10 digits." ValidationGroup="Save" EnableClientScript="true"
                                                        ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 46%">
                                        <span
                                            class="ClsLabel">Emergency Contact : </span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:TextBox ID="txtEmergencyNo" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" />&nbsp;*
                                       <asp:RequiredFieldValidator ID="reqEmergencyNo" runat="server" ControlToValidate="txtEmergencyNo"
                                            Display="None" ErrorMessage="Emergency contact number should not be blank." ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        
                                    </td>
                                </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 46%;">                                                    
                                                    <span class="ClsLabel">Designation :</span>
                                                </td>
                                                <td align="left" style="width: 31%">
                                                    <asp:DropDownList ID="cmbDesignation" runat="server" CssClass="LrgCombo">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*
                                                        <asp:CompareValidator ID="cmpDesignation" runat="server" ControlToValidate="cmbDesignation"
                                                            Display="None" ErrorMessage="Designation should be selected." ValidationGroup="Save" Operator="NotEqual"
                                                            Type="Integer" ValueToCompare="0"></asp:CompareValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" style="width: 46%;" class="ClsBorderlight">
                                                    <span class="ClsLabel" id="Span8">Photo :</span>
                                                </td>
                                                <td>
                                                    <div class="ClsBorderlight" style="width: 112px; vertical-align: middle">
                                                        <asp:Image ID="imgPhoto" ImageUrl="~/RITeSchool/images/Student_BlankPh.jpg" runat="server"
                                                            Height="151px" Width="119px" /></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" style="width: 46%">
                                                    <span class="ClsLabel">Upload Photo :</span>
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="UploadPhoto" runat="server" />
                                                    <asp:CustomValidator ID="CustPhoto" Display="None" runat="server" ClientValidationFunction="ValidatePhoto"
                                                        ErrorMessage="Invalid file format." ControlToValidate="UploadPhoto" ValidationGroup="Save"
                                                        CssClass="LblErrorMsg"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                    <td align="left" class="ClsBorderlight" colspan="2">                                       
                                        <span class="LblSmlGray">Upload an image file for Transport Staff's photo
                                            <br />
                                            (Max Height: 151px and Max Width: 112px).<br />
                                            (Image size should not exceed 80 kb. Supported file formats are JPG, JPEG)</span>
                                    </td>
                                </tr>
                                            <tr>
                                                <td style="width: 50%" align="right">
                                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                                        CausesValidation="true" OnClick="btnSave_Click" ValidationGroup="Save" />
                                                </td>
                                                <td align="left">                                                    
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- User InfoTable ListView -->
                    <tr id="trPagerTransportStaff" runat="server">
                        <td align="center">
                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwTransportStaff">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                            <br />
                                        </PagerTemplate>
                                    </asp:TemplatePagerField>
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table align="center" width="60%">
                                <tr>
                                    <td align="center" style="width: 100%">
                                        <asp:ListView ID="lstvwTransportStaff" runat="server" OnDataBound="lstvwTransportStaff_DataBound"
                                            DataKeyNames="miTransportStaffId,miUserId" OnItemDataBound="lstvwTransportStaff_ItemDataBound"
                                            OnItemCommand="lstvwTransportStaff_ItemCommand" OnSorting="lstvwTransportStaff_Sorting"
                                            DataSourceID="ObjDSTransportStaff">
                                            <LayoutTemplate>
                                                <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" width="25%" style="padding-left: 7px;">
                                                            <asp:LinkButton ID="lnkBtnSortName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                CausesValidation="false" ForeColor="Black"> Name </asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="18%" style="padding-left: 7px;">
                                                            <asp:LinkButton ID="lnkBtnDesignation" runat="server" CommandName="Sort" CommandArgument="DesignationId"
                                                                CausesValidation="false" ForeColor="Black"> Designation</asp:LinkButton>
                                                        </th>
                                                         <th align="left" width="18%" style="padding-left: 7px;">
                                                            <asp:LinkButton ID="lnkbtnDOB" runat="server" CommandName="Sort" CommandArgument="DOB"
                                                                CausesValidation="false" ForeColor="Black"> DOB</asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="18%" style="padding-left: 7px;">
                                                            <asp:LinkButton ID="lnkBtnMobileNo" runat="server" CommandName="Sort" CommandArgument="MobileNo"
                                                                CausesValidation="false" ForeColor="Black"> Mobile No</asp:LinkButton>
                                                        </th>
                                                        <th class="paddingLR" align="center">
                                                            Photo
                                                        </th>
                                                        <th align="center" width="135px">
                                                            Edit
                                                        </th>
                                                        <th align="center" width="135px">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                        <td colspan="7">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwTransportStaff"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td align="right" class="LblNormal">
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("msName") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("msDesignation") %>'></asp:Label>
                                                    </td>
                                                     <td align="left" class="paddingL">
                                                        <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("mdtDOB") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("msMobileNo") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingLR" align="center" width="5%">
                                                        <asp:Image ID="imgPhotoUpload" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
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
                                                    <td class="paddingL" align="left">
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("msName") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingL" align="left">
                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("msDesignation") %>'></asp:Label>
                                                    </td>
                                                     <td align="left" class="paddingL">
                                                        <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("mdtDOB") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("msMobileNo") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingLR" align="center" width="5%">
                                                        <asp:Image ID="imgPhotoUpload" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
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
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        &nbsp;
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                                            CausesValidation="False" UseSubmitBehavior="false" />
                                             <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:LocalizedResources, Add%>" CssClass="ClsBtn" BorderWidth="1px" CausesValidation="false" UseSubmitBehavior="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ObjectDataSource TypeName="BusinessLogic.TransportStaffBL" EnablePaging="True"
                                ID="ObjDSTransportStaff" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                SelectCountMethod="CountTotalTransportStaff" EnableCaching="False">
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
                            <asp:HiddenField ID="hidMode" runat="server" />
                            <asp:HiddenField ID="hidTransportStaffID" runat="server" Value="0" />
                            <asp:HiddenField ID="hidFilePath" runat="server" />
                            <asp:HiddenField ID="hidUserId" runat="server" />
                              <asp:HiddenField ID= "hidUserRoleid" runat="server" />
                               <asp:HiddenField ID="hidQueryString" runat="server"/>
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientcstbtnSave = "<%=this.btnSave.ClientID%>"
        _clientcstbtnCancel = "<%=this.btnCancel.ClientID%>"
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID%>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientUploadPhoto = "<%=this.UploadPhoto.ClientID%>"
        _ClientCustPhoto = "<%=this.CustPhoto.ClientID %>"
        _ClienttxtAddress = "<%=this.txtAddress.ClientID %>"
        _clientcstValAddress = "<%=this.cstValAddress.ClientID %>"
        
        function validateAddress(source, args) {
            var txtAddress= document.getElementById(_ClienttxtAddress).value;
            var bIsValid = true;

            if (txtAddress.trim() != "") 
            {
                if (txtAddress.length > 150) 
                {
                    bIsValid = false;
                    document.getElementById(_clientcstValAddress).errormessage =
                  "Length of address should not exceed 150 characters.";
                }

            }
            else {

                bIsValid = false;
                document.getElementById(_clientcstValAddress).errormessage =
                  "Address should not be blank.";
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }


        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobileNo.ClientID %>"
        function MobileNumberValidation(oSrc, args) {
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)
            document.getElementById(_clientcst_MobileNumber).errormessage = ""
            if (sMobileNumber.length == 0) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile No. should not be blank."
                args.IsValid = false
                return true
            }
            if (sMobileNumber.length < 10) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile No. should be of 10 digits."
                args.IsValid = false
                return true
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile No. should not start with zero."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            }
            if (document.getElementById(_clientlblErrorMsg) != null) {
                document.getElementById(_clientlblErrorMsg).style.display = "none"
                document.getElementById(_clientlblErrorMsg).innerHTML = ""
            }

        }
        function ValidatePhoto(aSrc, args) {
            var sImage = new Image();
            aSrc.errormessage = "";
            sImage.src = document.getElementById(_clientUploadPhoto).value;
            var iWidth = sImage.width
            var iHeight = sImage.height
            if (sImage.src != "") {
                if (!CheckFileType(sImage.src)) {
                    aSrc.errormessage = "Invalid file format.";
                    document.getElementById(_ClientCustPhoto).errormessage = "Invalid file format.";
                }
            }
            if (aSrc.errormessage == "") {
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }
        //This function is used to check file type.
        function CheckFileType(sFileName) {
            var bIsValid;
            var sFileType = sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase();
            if (sFileType == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG")
                bIsValid = true;
            else
                bIsValid = false;
            return bIsValid
        }
    </script>

</asp:Content>
