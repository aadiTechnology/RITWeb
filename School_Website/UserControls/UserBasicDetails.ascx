<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserBasicDetails.ascx.cs"
    Inherits="UserBasicDetailsUC" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<table border="0" cellpadding="1" cellspacing="2" width="100%">
    <tr>
        <td class="ClsBorderlight" id="tdUC" runat="server" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="lblpanNo" runat="server" Text="<%$ Resources:LocalizedResources, PanNo %>"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
            <asp:TextBox ID="txtPanNo" runat="server" onkeypress="return PreventSpecialChars(event);" TabIndex="101" CssClass="MidTxtBox" MaxLength="20"></asp:TextBox>
            <asp:FileUpload ID="UploadPAN" runat="server" Style="white-space: nowrap" />
             <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ClientValidationFunction="ValidateFile" ValidationGroup="Save"
                                        ErrorMessage="Invalid file format. Only bitmap(*.bmp) is allowed." CssClass="TxtNormal"></asp:CustomValidator>
            <asp:ImageButton ID="btnDownload" runat="server" CausesValidation="false" ToolTip="View Attachment"
                                    CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
             <img src="~/RITeSchool/images/IconGrid_Delete.gif" alt="" id="imgBtnDelete" class="img-align-unset" runat="server" onclick="WarnOnDelete()"
                                                    title="DeleteAttachment" />                                     
        </td>
    </tr>
    <tr runat="server" id="trUploadNote" >
                                <td colspan="2" align="center" style="white-space:nowrap">
                                    <span class="LblSmlGray">(Supports files of types - .BMP,.DOC,.DOCX,.JPG,.JPEG,.PDF,.XLS,.XLSX
                                        upto 2 MB)</span>
                                </td>
                            </tr>
    <tr>
        <td class="ClsBorderlight" id="td2" runat="server" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="lblAadharNo" runat="server" Text="Aadhar Card Number"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
            <asp:TextBox ID="txtAadharNo" runat="server" onkeypress="return blockNonNumbers (this, event, false, false);" TabIndex="101" CssClass="MidTxtBox" MaxLength="12"></asp:TextBox>
            <asp:FileUpload ID="UploadAadhar" runat="server" Style="white-space: nowrap" />
             <asp:CustomValidator ID="cstValidateAadharLogo" Display="None" runat="server" ClientValidationFunction="ValidateAadharFile" ValidationGroup="Save"
                                        ErrorMessage="Invalid file format. Only bitmap(*.bmp) is allowed." CssClass="TxtNormal"></asp:CustomValidator>
            <asp:ImageButton ID="imgDownloadAadhar" runat="server" CausesValidation="false" Visible = "false" ToolTip="View Attachment"
                                    CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
             <img src="~/RITeSchool/images/IconGrid_Delete.gif" alt="" id="imgBtnDeleteAadhar" Visible = "false" class="img-align-unset" runat="server" onclick="WarnOnDeleteAadhar()"
                                                    title="DeleteAttachment" />                                     
        </td>
    </tr>
    <tr runat="server" id="trUploadAadharNote" >
          <td colspan="2" align="center" style="white-space:nowrap">
              <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed
                              3MB.)</span>
          </td>
    </tr>
      <tr>
        <td class="ClsBorderlight" id="td1" runat="server" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="lblEmpNo" runat="server" Text="<%$ Resources:LocalizedResources, EmployeeNumber %>"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
            <asp:TextBox ID="txtEmpNo" runat="server" TabIndex="101" CssClass="MidTxtBox" MaxLength="50" Width="150px"></asp:TextBox>
        </td>
    </tr>
    <tr id="trDate" runat="server">
        <td class="ClsBorderlight" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, JoiningDate %>"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
            <asp:TextBox ID="txtJoiningDate" CssClass="SmlCombo" runat="server" Style="vertical-align: bottom"
                ValidationGroup="Save" CausesValidation="true" MaxLength="11" onpaste="event.returnValue=false" TabIndex="102" 
                ondrop="event.returnValue=false"></asp:TextBox>
            <rjs:PopCalendar ID="calJoiningDate" runat="server"  Culture="en" Control="txtJoiningDate" Format="dd MMM yyyy" 
                ControlFocusOnError="True" ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>"/>           
            <asp:CustomValidator ID="csttxtDateofJoining" runat="server" ClientValidationFunction="validateJoiningDate" ControlToValidate="txtJoiningDate"
                CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                Visible="true"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td class="ClsBorderlight" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, PermanentDate %>"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
            <asp:TextBox ID="txtPermanentDate" CssClass="SmlCombo" runat="server" Style="vertical-align: bottom" TabIndex="103" ValidationGroup="Save"
                MaxLength="11" onpaste="event.returnValue=false" ondrop="event.returnValue=false" CausesValidation="true"></asp:TextBox>
            <rjs:PopCalendar ID="PopCalendar1" runat="server" Culture="en" Control="txtPermanentDate" Format="dd MMM yyyy"
                ControlFocusOnError="True" ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>" />
            <asp:CustomValidator ID="cstPermanentDate" runat="server" ClientValidationFunction="validateDates" ControlToValidate="txtPermanentDate"
                CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                Visible="true"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td class="ClsBorderlight" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, ResignationDate %>"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
            <asp:TextBox ID="txtResignationDate" CssClass="SmlCombo" runat="server" 
                Style="vertical-align: bottom" TabIndex="104"
                CausesValidation="true" MaxLength="11" onpaste="event.returnValue=false" 
                ondrop="event.returnValue=false"></asp:TextBox>
            <rjs:PopCalendar ID="PopCalendar2" runat="server" Culture="en" Control="txtResignationDate" Format="dd MMM yyyy" 
                ControlFocusOnError="True" ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>" />
            <asp:CustomValidator ID="cstvalResignDate" runat="server" ClientValidationFunction="validateResignDate" ControlToValidate="txtResignationDate"
                CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                Visible="true"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td class="ClsBorderlight" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="Label6" runat="server" Text="Transfer Date"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
            <asp:TextBox ID="txtTransferDate" CssClass="SmlCombo" runat="server" 
                Style="vertical-align: bottom" TabIndex="104"
                CausesValidation="true" MaxLength="11" onpaste="event.returnValue=false" 
                ondrop="event.returnValue=false"></asp:TextBox>
            <rjs:PopCalendar ID="PopCalendar3" runat="server" Culture="en" Control="txtTransferDate" Format="dd MMM yyyy" 
                ControlFocusOnError="True" ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateTransferDate"
                CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                Visible="true"></asp:CustomValidator>
            <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateTransferDate1"
                CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                Visible="true"></asp:CustomValidator>
        </td>
    </tr>
    <tr id="trGrade" runat="server" visible="false">
        <td class="ClsBorderlight" style="white-space:nowrap">            
            <span class="ClsLabel" ><asp:Label ID="Label19" runat="server" Text="Grade Pay (Rs.)"></asp:Label><span class="colonPadding">:</span></span>
        </td>
        <td align="left" style="white-space:nowrap;height:30px;">
            <div style="float:left;width:160px;height:100%;padding-top:5px;" class="ClsBorderlight">
                <span class="ClsLabel"><asp:Label ID="lblGradepay" runat="server"  Text=""></asp:Label><span class="colonPadding"></span></span>
            </div>
        </td>
    </tr>


     <tr>
        <td class="ClsBorderlight" style="white-space:nowrap">            
            <span class="ClsLabel"><asp:Label ID="Label4" runat="server" Text="Job Type"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
           <asp:DropDownList ID="cmbStaffStatusType" runat="server" CssClass="MidCombo">
           </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="ClsBorderlight" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="Label5" runat="server" Text="Status"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
           <asp:DropDownList ID="cmbStaffWorkingStatus" runat="server" CssClass="MidCombo">
           </asp:DropDownList>
        </td>
    </tr>
     <tr>
        <td class="ClsBorderlight" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="Label8" runat="server" Text="Blood Group"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
           <asp:DropDownList ID="cmbBloodGroup" runat="server" CssClass="MidCombo">
           </asp:DropDownList>
        </td>
    </tr>
    <tr id="trIsOnClockHoursBasis" runat="server" visible = "false">
        <td class="ClsBorderlight" style="white-space:nowrap">
            <span class="ClsLabel"><asp:Label ID="Label7" runat="server" Text="Is On Clock Hours Basis?"></asp:Label><span class="colonPadding"> :</span></span>
        </td>
        <td align="left" style="white-space:nowrap">
            <asp:CheckBox ID="chkIsOnCHB" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
           <%-- <asp:HiddenField ID="hidUserIdUC" runat="server" Value="" />--%>
           <asp:HiddenField runat="server" ID="hidJoiningDateValidation" />
           <asp:HiddenField runat="server" ID="hidPermanentDateValidation" />
           <asp:HiddenField runat="server" ID="hidPermanentJoiningDateValidation" />
           <asp:HiddenField runat="server" ID="hidJoiningDateValidation1"/>
           <asp:HiddenField runat="server" ID="hidResignationDateValidation" />
           <asp:HiddenField runat="server" ID="hidResignationDateValidation1" />
           <asp:HiddenField runat="server" ID="hidResignationDateValidation2" />
           <asp:HiddenField runat="server" ID="hidFilePath" />
           <asp:HiddenField runat="server" ID="hidAadharFilePath" />
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
    _clienttxtJoiningDate = "<%=this.txtJoiningDate.ClientID %>";
    _clientcsttxtDateofJoining = "<%=this.csttxtDateofJoining.ClientID %>";
    _clienttxtPermanentDate = "<%=this.txtPermanentDate.ClientID %>";
    _clienttxtResignDate = "<%=this.txtResignationDate.ClientID %>";
    _clientcstvalResignDate = "<%=this.cstvalResignDate.ClientID %>";
    _clienttxtPanNo = "<%=this.txtPanNo.ClientID %>";
    _clientcstPermanentDate = "<%=this.cstPermanentDate.ClientID %>";
    _clientUploadFile = "<%=this.UploadPAN.ClientID %>";
    _clientUploadAadhar = "<%=this.UploadAadhar.ClientID %>"
    _clientcstValidateLogo = '<%=this.cstValidateLogo.ClientID %>';
    _clientcstValidateAadharLogo = '<%=this.cstValidateAadharLogo.ClientID %>';
    _clienthidFilePath = "<%=this.hidFilePath.ClientID %>";
    _clienthidAadharFilePath = "<%=this.hidAadharFilePath.ClientID %>";
    _clienttxtTransferDate = "<%=this.txtTransferDate.ClientID %>";
    _clientlblGradepay = "<%=this.lblGradepay.ClientID %>";

    function validateJoiningDate(source, args) {
        var txtDOJ = trimAll(document.getElementById(_clienttxtJoiningDate).value);
        var txtDOP = trimAll(document.getElementById(_clienttxtPermanentDate).value);

        var txtStartDate = $get(_clienttxtJoiningDate);
        var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));

        var txtPermanentDate = $get(_clienttxtPermanentDate);
        var dtPermanentDate = new Date(txtPermanentDate.value.replace(/-/g, ' '));

        var bIsValid = true;
        if (txtDOJ.trim() != "") {
            if (!IsValidDate(dtStartDate)) {
                bIsValid = false;
                document.getElementById(_clientcsttxtDateofJoining).errormessage = document.getElementById("<%=hidJoiningDateValidation.ClientID%>").value; 
            }
            else if (txtDOP.trim() != "" && txtDOJ.trim() != "") {

                if (!IsValidDate(dtPermanentDate)) {
                    bIsValid = false;
                    document.getElementById(_clientcsttxtDateofJoining).errormessage = document.getElementById("<%=hidPermanentDateValidation.ClientID%>").value; 
                }
                else if (IsValidDate(dtPermanentDate) && (dtStartDate > dtPermanentDate)) {
                    bIsValid = false;
                    document.getElementById(_clientcsttxtDateofJoining).errormessage = document.getElementById("<%=hidPermanentJoiningDateValidation.ClientID%>").value; 
                }
            }           
        }
        args.IsValid = bIsValid;
        return !bIsValid;
    }

    function WarnOnDelete() {
        if(window.confirm("Are you sure you want to delete this attachment?\nAttachment will get removed only after staff record is saved."))
        {
            $get(_clienthidFilePath).value = "";
        }
    }

    function WarnOnDeleteAadhar() {
        if (window.confirm("Are you sure you want to delete this attachment?\nAttachment will get removed only after staff record is saved.")) {
            $get(_clienthidAadharFilePath).value = "";
        }
    }

    function ValidateFile(aSrc, args) {
        var oFileName = document.getElementById(_clientUploadFile).value;

        if (oFileName != "" && !oFileName.toUpperCase().endsWith(".DOC") && !oFileName.toUpperCase().endsWith(".DOCX") && !oFileName.toUpperCase().endsWith(".PDF") && !oFileName.toUpperCase().endsWith(".BMP") && !oFileName.toUpperCase().endsWith(".XLS")
             && !oFileName.toUpperCase().endsWith(".JPG") && !oFileName.toUpperCase().endsWith(".JPEG") && !oFileName.toUpperCase().endsWith(".XLSX")) {
                aSrc.errormessage = "Invalid file format of Pan card.";
                args.IsValid = false;
                return true;
            }        
        args.IsValid = true;
        return false;
    }

    function ValidateAadharFile(aSrc, args) {
        var oFileName = document.getElementById(_clientUploadAadhar).value;

        if (oFileName != "" && !oFileName.toUpperCase().endsWith(".PDF") && !oFileName.toUpperCase().endsWith(".JPG") && !oFileName.toUpperCase().endsWith(".PNG") && !oFileName.toUpperCase().endsWith(".BMP") && !oFileName.toUpperCase().endsWith(".JPEG")) {
            aSrc.errormessage = "Invalid file format of Aadhar card.";
            args.IsValid = false;
            return true;
        }
        args.IsValid = true;
        return false;
    }

    function validateDates(source, args) {
        var txtDOJ = trimAll(document.getElementById(_clienttxtJoiningDate).value);        
        var txtDOP = trimAll(document.getElementById(_clienttxtPermanentDate).value);
        var txtDOR = trimAll(document.getElementById(_clienttxtResignDate).value);

        var txtStartDate = $get(_clienttxtJoiningDate);
        var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));
        var txtPermanentDate = $get(_clienttxtPermanentDate);
        var dtPermanentDate = new Date(txtPermanentDate.value.replace(/-/g, ' '));
        var txtResignDate = $get(_clienttxtResignDate);
        var dtResignDate = new Date(txtResignDate.value.replace(/-/g, ' '));

        var bIsValid = true;
        if (txtDOP.trim() != "" && txtDOR.trim() == "") {

            if (IsValidDate(dtPermanentDate)) {               
                if (txtDOJ.trim() == "") {
                    bIsValid = false;
                    document.getElementById(_clientcstPermanentDate).errormessage = document.getElementById("<%=hidJoiningDateValidation1.ClientID%>").value; 
                }
            }

        }
        args.IsValid = bIsValid;
        return !bIsValid;

    }

    function IsValidDate(date) {
        if (typeof (date) == 'string') 
            date = new Date(date);
        return !(date == 'Invalid Date' || date == 'NaN' || date.getFullYear() < 1900);
        
    }

    function validateResignDate(source, args) {
        var txtDOJ = trimAll(document.getElementById(_clienttxtJoiningDate).value);
        var txtDOR = trimAll(document.getElementById(_clienttxtResignDate).value);
        var txtDOP = trimAll(document.getElementById(_clienttxtPermanentDate).value);

        var txtStartDate = $get(_clienttxtJoiningDate);
        var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));

        var txtResignDate = $get(_clienttxtResignDate);
        var dtResignDate = new Date(txtResignDate.value.replace(/-/g, ' '));

        var txtPermanentDate = $get(_clienttxtPermanentDate);
        var dtPermanentDate = new Date(txtPermanentDate.value.replace(/-/g, ' '));

        var bIsValid = true;
        if (txtDOR.trim() != "") {            
            if (txtDOJ.trim() != "") {
                if (!IsValidDate(dtResignDate)) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalResignDate).errormessage = document.getElementById("<%=hidResignationDateValidation.ClientID%>").value;
                }
                else if (dtStartDate >= dtResignDate) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalResignDate).errormessage = document.getElementById("<%=hidResignationDateValidation1.ClientID%>").value;
                }
                if (txtDOP.trim() != "" && dtPermanentDate >= dtResignDate) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalResignDate).errormessage = document.getElementById("<%=hidResignationDateValidation2.ClientID%>").value; 
                }
            }

            else {
                bIsValid = false;
                if(txtDOP=="")
                    document.getElementById(_clientcstvalResignDate).errormessage = document.getElementById("<%=hidJoiningDateValidation1.ClientID%>").value;
            }
        }
        else if (txtDOJ.trim() != "") {
            if (txtDOP.trim() != "" && !IsValidDate(dtPermanentDate)) {
                bIsValid = false;
                document.getElementById(_clientcstvalResignDate).errormessage = document.getElementById("<%=hidPermanentDateValidation.ClientID%>").value; 
            }
        }
        args.IsValid = bIsValid;
        return !bIsValid;

    }


    function ValidateTransferDate(source, args) {
        var txtDOJ = trimAll(document.getElementById(_clienttxtJoiningDate).value);
        var txtDOT = trimAll(document.getElementById(_clienttxtTransferDate).value);

        var txtStartDate = $get(_clienttxtJoiningDate);
        var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));

        var txtTransferDate = $get(_clienttxtTransferDate);
        var dtTransferDate = new Date(txtTransferDate.value.replace(/-/g, ' '));


        if (txtDOJ == "" && txtDOT != "") {
            source.errormessage = "Joining Date should not be blank if need to set Transfer Date.";
            args.IsValid = false;
            return true;
        }
        else if (txtDOJ != "" && txtDOT != "" && dtTransferDate <= dtStartDate) {
            source.errormessage = "Transfer Date should be greater than Joining Date.";
            args.IsValid = false;
            return true;
        }
        

        args.IsValid = true;
        return false;

    }

    function ValidateTransferDate1(source, args) {
        var txtDOP = trimAll(document.getElementById(_clienttxtPermanentDate).value);
        var txtDOT = trimAll(document.getElementById(_clienttxtTransferDate).value);

        if (txtDOP != "" && txtDOT != "") {
            var txtPermanentDate = $get(_clienttxtPermanentDate);
            var dtPermanentDate = new Date(txtPermanentDate.value.replace(/-/g, ' '));

            var txtTransferDate = $get(_clienttxtTransferDate);
            var dtTransferDate = new Date(txtTransferDate.value.replace(/-/g, ' '));


           if (dtTransferDate <= dtPermanentDate) {
                source.errormessage = "Transfer Date should be greater than Permanent Date.";
                args.IsValid = false;
                return true;
            }
        }

        args.IsValid = true;
        return false;

    }


    function PreventSpecialChars(e) {
        var k;
        document.all ? k = e.keyCode : k = e.which;
        return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57) || k == 0 || k == 9);
    }
    
</script>
