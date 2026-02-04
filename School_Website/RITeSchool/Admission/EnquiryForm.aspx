<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="EnquiryForm.aspx.cs" Inherits="EnquiryForm" 
    Title="Admission process" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">

    <script src="../PopCalendar2008/PopCalendarAjaxNet.js" type="text/javascript"></script>

    <script src="../PopCalendar2008/PopCalendarFunctionsAjaxNet.js" type="text/javascript"></script>

    <script type="text/javascript">
        grecaptcha.ready(function () {
            var dt = document.getElementById('<%=this.hidCaptData.ClientID %>').value
            if (dt != null && dt != '') {
                grecaptcha.execute(dt, { action: 'submit' }).then(function (token) {
                    document.getElementById('g-recaptcha-token').value = token;
                });
            }
        });
    </script>

    <div style="width: 97%" align="center" >
        <div id="nifty" align="center">
            <b class="rtop"></b>
            <input type="hidden" id="g-recaptcha-token" name="g-recaptcha-token" />
            <asp:HiddenField ID="hidCaptData" runat="server" Value="" />
            <table align="center" class="paddingLR" cellspacing="2" cellpadding="2" border="0" 
                width="100%">
                <tbody >  
                    <tr id="trDPISBranch" runat="server" visible="false">
                        <td colspan="4" align="center">
                            <div style="background-color:#gray;box-shadow: 2px 2px silver;width:350px;padding:5px;">
                                <asp:Label ID="lblBranchName" runat="server" Text="" style="font-size:20px;font-weight:bold;"></asp:Label>
                            </div>
                        </td>
                    </tr> 
                    <tr>
                        <td colspan="2" class="HeadTxtBWOPadding borderBtm" align="left">
                            <asp:Label ID="lblEnquiryHeader" runat="server" Text="Enquiry Form"></asp:Label>                            
                        </td>
                        <td class="borderBtm ErrMsg" align="right" colspan="2">
                            NOTE: Fields with yellow background are mandatory.
                        </td>
                    </tr>                
                    <tr class="">
                        <td align="left" colspan="4">                            
                            <asp:ValidationSummary ID="valSum" runat="server" DisplayMode="BulletList" ShowMessageBox="false" CssClass="ClsLabel"
                                ShowSummary="true" />
                        </td>                        
                    </tr>
                    <tr>
                        <td colspan="4" id="tdErrorMessage" runat="server" visible="false" rowspan=1>
                        Enquiry No already exists. Please change the Enquiry No.<asp:Label ID="lblError1" runat="server" Visible="false" ></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td align="left" class="TxtNormal" style="width: 195px">
                            <asp:Image ID="Image1" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="160px" />
                        </td>
                        <td align="left">
                        </td>
                        <td align="left" class="TxtNormal">
                            <asp:Image ID="Image3" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="120px" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr id="trLocation" runat="server" visible="false">
                        <td class="TxtNormal" align="left">
                            <span>School Location :</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbSchoolLocation" runat="server" ViewStateMode="Enabled" 
                                CssClass="MidCombo" BackColor="#ffffa0"></asp:DropDownList>
							<asp:RequiredFieldValidator ID="reqcmbLocation" runat="server" ErrorMessage="School Location should be Selected." Display="None" Enabled="false" ControlToValidate="cmbSchoolLocation"  InitialValue="0"></asp:RequiredFieldValidator>
                       </td>
                    </tr>
                    <tr id="trSPSEnquiry" runat="server">
                        <td align="left" style="width: 195px" >
                            <asp:Label ID="lblEnquiryName" runat="server" Text="Enquiry No: "></asp:Label>                                               
                        </td>
                        <td  align="left" colspan="3">
                             <asp:TextBox ID="txtEnqNo" runat="server" ReadOnly="true" ViewStateMode="Enabled"></asp:TextBox>
                        </td>
                          
                          <%--  <td class="TxtNormal" align="left" style="font-size: 10pt">
                             Date :
                            </td>                        
                            <td align="left"> 
                                <asp:TextBox ID="txtdate" runat="server" CssClass="SmlTxtBox" />
                                 <rjs:PopCalendar ID="PopCalendar6" runat="server"
                                 Control="txtdate" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false"
                                 InvalidDateMessage="Please select valid date ." To-Today="true" /> 
                               
                            </td>--%>
                            </tr>
                    <tr id="trSPSAdmissionFor" runat="server" visible="false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            Admission for:&nbsp;
                        </td>
                       <td  align="left" colspan="3">
                           <asp:DropDownList ID="cmbAdmissionFor" runat="server" CssClass="TxtBox"  
                               AutoPostBack="true"  ViewStateMode="Enabled"
                                BackColor="#ffffa0" 
                               onselectedindexchanged="cmbAdmissionFor_SelectedIndexChanged">
                            </asp:DropDownList>                        
                        </td>
                    </tr>
                     <tr>
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                          <asp:Label ID="Label5" runat="server" Text="Grade/Std. Applying for"></asp:Label>    
                          <%--  Grade/Std. Applying for:&nbsp;--%>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="cmbStd" runat="server" CssClass="TxtBox"  AutoPostBack="false"  ViewStateMode="Enabled"
                                        BackColor="#ffffa0" >
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblStdName" runat="server" Font-Bold="true"></asp:Label>
                                    <asp:CompareValidator ID="cmp_valStdr" runat="server" ControlToValidate="cmbStd"
                                        Display="None" ErrorMessage="Admission sought for standard should be selected."
                                        Operator="NotEqual" ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                             </ContentTemplate>
                             <Triggers>                           
                                 <asp:AsyncPostBackTrigger ControlID="cmbAdmissionFor" EventName="SelectedIndexChanged" />                            
                             </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            Admission Year (Academic Year):
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbYear" runat="server" CssClass="TxtBox" AutoPostBack="true" ViewStateMode="Enabled"
                                OnSelectedIndexChanged="cmbYear_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="cmp_valYear" runat="server" ControlToValidate="cmbYear"
                                Display="None" ErrorMessage="Academic Year should be selected." Operator="NotEqual"
                                ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TxtNormal" valign="top" 
                            style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="lblStudentName" runat="server" Text="Name of Child:"></asp:Label>                            
                        </td>
                        <td align="left" colspan="1">
                            <table align="left" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td class="TxtNormal" id="tdLastName" runat="server" visible = "false" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtSLastName" MaxLength="50" onblur="formatName(this)"/>    
                                         <asp:CustomValidator ID="cstSLastName" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidatesLastName"></asp:CustomValidator>                                     
                                    </td>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBoxMand" ID="txtSName" MaxLength="50" BackColor="#ffffa0" onblur="formatName(this)"/>
                                        <asp:RequiredFieldValidator ID="reqSName" runat="server" ErrorMessage="Student First Name should not be blank."
                                            Display="None" ControlToValidate="txtSName"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtFahterName" MaxLength="50"  onblur="formatName(this)"/>
                                        <asp:CustomValidator ID="cstStudentMiddleName" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateMiddleName"></asp:CustomValidator>
                                    </td>
                                     <td id="tdSPSLastName" runat="server" visible = "false" class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtSPSLastName" MaxLength="50" BackColor="#ffffa0" onblur="formatName(this)"/>
                                        <asp:CustomValidator ID="cstStudentFirstName" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateFirstName"></asp:CustomValidator>                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdlblLastName" runat="server" visible = "false" class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Last Name)
                                    </td>
                                    <td class="TxtNormal" align="center" style="font-size: 10pt">
                                        (First Name)
                                    </td>
                                    <td class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Middle Name)
                                    </td>
                                    <td id="tdSPSlblLastName" runat="server" visible = "false" class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Last Name)
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            Gender:&nbsp;
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            <asp:RadioButton ID="rdoMale" Text="Male" runat="server" GroupName="rdoGroupSex"
                                CssClass="ClsLabel" Checked="True"></asp:RadioButton>
                            <asp:RadioButton ID="rdoFemale" Text="Female" runat="server" GroupName="rdoGroupSex"
                                CssClass="ClsLabel"></asp:RadioButton>
                        </td>
                    </tr>
                     <tr>
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            Date of Birth:&nbsp;
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            <asp:TextBox ID="txtCalDobPopup" CssClass="TxtBoxMand" runat="server" 
                                BackColor="#ffffa0" ></asp:TextBox><rjs:PopCalendar ID="CalDobPopup" runat="server"
                                    Control="txtCalDobPopup" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" To-Today="true"
                                    InvalidDateMessage="Please select valid date of birth." />
                            <asp:RequiredFieldValidator ID="reqDOB" runat="server" ErrorMessage="Date of Birth should not be blank."
                                Display="None" ControlToValidate="txtCalDobPopup"> </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstDOB" Display="None" runat="server" CssClass="ClsMdtStar"
                                ControlToValidate="txtCalDobPopup" Visible="true" EnableClientScript="true" ClientValidationFunction="checkDOB"></asp:CustomValidator>
                                 <asp:HiddenField ID="hidMinBdate" runat="server" />
							  <asp:HiddenField ID="hidMaxBdate" runat="server" />
                        </td>                        
                    </tr>
                    <tr id="trCategoty" runat="server" visible="false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            Category :
                        </td>
                        <td colspan="3" class="TxtNormal" align="left" style="font-size: 10pt">
                            <asp:DropDownList ID="cmbCategory" runat="server" CssClass="TxtBox"  AutoPostBack="false"  ViewStateMode="Enabled"
                            BackColor="#ffffa0" >
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqcmbCategory" runat="server" ErrorMessage="Category should be selected.."
                                Display="None" ControlToValidate="cmbCategory" InitialValue="0"> </asp:RequiredFieldValidator>
                        </td>                        
                    </tr>
                    <tr id="trSPSEnquirtNationality" runat="server" visible="false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            Nationality :
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            <asp:TextBox ID="txtNationality" CssClass="TxtBox" runat="server" AutoPostBack="false"></asp:TextBox>
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            Passport No. :
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            <asp:TextBox ID="txtPassportNo" CssClass="TxtBox" runat="server" AutoPostBack="false"></asp:TextBox>                           
                        </td>
                    </tr>
                    <tr id="trSPSEnquiryPhoneNo" runat="server" visible="false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            Residence Phone No. :
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            <asp:TextBox ID="txtResidencePhone" CssClass="TxtBox" runat="server" MaxLength="12" AutoPostBack="false" 
                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);" 
                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false" ondrop="event.returnValue=false">
                            </asp:TextBox>
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                           Office Phone No. :
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                            <asp:TextBox ID="txtOfficePhone" CssClass="TxtBox" runat="server" MaxLength="12" AutoPostBack="false" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false">                            
                            </asp:TextBox>                           
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TxtNormal" valign="top" 
                            style="font-size: 10pt; width: 195px;">
                            Father's Name:
                        </td>
                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                            <table cellpadding="0" cellspacing="1">
                                <tr>
                                    <td id="tdFlastName" runat="server" visible="false" class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtFSurname" MaxLength="50"  onblur="formatName(this)"/>                                        
                                    </td>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                      
                                          <asp:TextBox runat="server" CssClass="TxtBoxMand" ID="txtFName" MaxLength="50" BackColor="#ffffa0"  onblur="formatName(this)"/><asp:RequiredFieldValidator ID="reqFName" runat="server" ErrorMessage="Father's First Name should not be blank."
                                            Display="None" ControlToValidate="txtFName"> </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtFFatherName" MaxLength="50"  onblur="formatName(this)"/>
                                        <asp:CustomValidator ID="cstFatherMiddleName" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateFatherMiddleName"></asp:CustomValidator>
                                    </td>
                                    <td id="tdSPSFlastName" runat="server" visible="false" class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtSPSFLastName" MaxLength="50" BackColor="#ffffa0"  onblur="formatName(this)"/>
                                        <asp:CustomValidator ID="cstFatherSurName" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateFatherSurName"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdlblFlastName" runat="server" visible="false" class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Last Name)
                                    </td>
                                    <td class="TxtNormal" align="center" style="font-size: 10pt">
                                        (First Name)
                                    </td>
                                    <td class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Middle Name)
                                    </td>
                                    <td id="tdSPSlblFlastName" runat="server" visible="false" class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Last Name)
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" class="TxtNormal" valign="top" style="font-size: 10pt">
                            Mother's Name:
                        </td>
                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                            <table cellpadding="0" cellspacing="1">
                                <tr>
                                    <td id="tdMLastName" runat="server" visible="false" class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtMSurname" MaxLength="50"  onblur="formatName(this)"/>                                        
                                    </td>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBoxMand" ID="txtMName" MaxLength="50" onblur="formatName(this)"/>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Mother's First Name should not be blank." Display="None" ClientValidationFunction="ValiadteMotherName"></asp:CustomValidator>
                                    </td>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtMHName" MaxLength="50" onblur="formatName(this)"/>
                                        <asp:CustomValidator ID="cstMotherHusbandName" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateMotherMiddleName"></asp:CustomValidator>
                                    </td>
                                     <td id="tdSPSMLastName" runat="server" visible="false" class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtSPSMLastName" MaxLength="50" BackColor="#ffffa0"  onblur="formatName(this)"/>
                                        <asp:CustomValidator ID="cstMotherSurName" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateMotherSurName"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdlblMLastName" runat="server" visible="false" class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Last Name)
                                    </td>
                                    <td class="TxtNormal" align="center" style="font-size: 10pt">
                                        (First Name)
                                    </td>
                                    <td class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Middle Name)
                                    </td>
                                    <td id="tdSPSlblMLastName" runat="server" visible="false" class="TxtNormal" align="center" style="font-size: 10pt">
                                        (Last Name)
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr1" runat="server" visible="false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                             Father's Qualification :
                        </td>
                        <td align="left">
                            <%--<asp:DropDownList ID="ddlFatherQualification" runat="server" CssClass="MidCombo"   ViewStateMode="Enabled">
                              
                            </asp:DropDownList>--%>
                              <asp:TextBox runat="server" CssClass="TxtBoxMand" ID="txtFQualification" MaxLength="50"   onblur="formatName(this)"/>                       
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                             Mother's Qualification :
                        </td>
                        <td align="left">
                          <%--  <asp:DropDownList ID="ddlMotherQualification" runat="server" CssClass="MidCombo" AutoPostBack="false"  ViewStateMode="Enabled">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>  --%>
                                 <asp:TextBox runat="server" CssClass="TxtBoxMand" ID="txtMQualification" MaxLength="50"   onblur="formatName(this)"/>              
                        </td>
                    </tr>
                    <tr id="trSPSOccupation" runat="server" visible = "false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                             Father's Occupation :
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbFatherOccupation" runat="server" CssClass="MidCombo"  AutoPostBack="false"  ViewStateMode="Enabled">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cstFatherOccupation" runat="server" ErrorMessage="" ControlToValidate="cmbFatherOccupation" 
                            ClientValidationFunction="ValidateOccupation"></asp:CustomValidator>                            
                        </td>
                        <td class="TxtNormal" align="left" style="font-size: 10pt">
                             Mother's Occupation :
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbMotherOccupation" runat="server" CssClass="MidCombo" AutoPostBack="false"  ViewStateMode="Enabled">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>  
                            <asp:CustomValidator ID="cstMotherOccupation" runat="server" ErrorMessage="" ControlToValidate="cmbMotherOccupation" 
                            ClientValidationFunction="ValidateOccupation"></asp:CustomValidator>                      
                        </td>
                    </tr>
                       <tr id="trWhatsup" runat="server" visible="false">
                         <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                             <asp:Label ID="Label3" runat="server" Text="Father WhatsApp No. :"></asp:Label>                             
                        </td>
                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                            <asp:TextBox ID="txtFoWhatsup" runat="server" CssClass="TxtBoxMand" MaxLength="10" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" BackColor="#ffffa0" />
                           <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None" ErrorMessage="Father's WhatsUp number should not be blank."
                                CssClass="ClsMdtStar" ControlToValidate="txtFoWhatsup"></asp:RequiredFieldValidator>--%>
                                <asp:CustomValidator ID="CustomValidator2" Display="None" runat="server" CssClass="ClsMdtStar" Visible="true"
                                    ErrorMessage="" EnableClientScript="true"
                                    ClientValidationFunction="MobileNumberValidator"></asp:CustomValidator>
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                            <asp:Label ID="Label4" runat="server" Text="Mother WhatsApp No. :"></asp:Label>                             
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                            <asp:TextBox ID="txtMoWhatsup" runat="server" CssClass="TxtBox" MaxLength="10" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                         
                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None" ErrorMessage="Mother's WhatsUp number should not be blank."
                                CssClass="ClsMdtStar" ControlToValidate="txtMoWhatsup"></asp:RequiredFieldValidator>--%>
                                <asp:CustomValidator ID="CustomValidator3" Display="None" runat="server" CssClass="ClsMdtStar" Visible="true"
                                    ErrorMessage="" EnableClientScript="true"
                                    ClientValidationFunction="MotherMobileNumberValidator"></asp:CustomValidator>
                        </td>
                        
                    </tr>
                      <tr>
                         <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                             <asp:Label ID="lblFatherMobileNo" runat="server" Text="Mobile No. Father:"></asp:Label>                             
                        </td>
                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                            <asp:TextBox ID="txtFatherMob1" runat="server" CssClass="TxtBoxMand" MaxLength="10" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" BackColor="#ffffa0" />
                           <%-- <asp:RequiredFieldValidator ID="reqMobileNo" runat="server" Display="None" ErrorMessage="Father's Mobile number should not be blank."
                                CssClass="ClsMdtStar" ControlToValidate="txtFatherMob1"></asp:RequiredFieldValidator>--%>
                                <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar" Visible="true"
                                    ErrorMessage="" EnableClientScript="true"
                                    ClientValidationFunction="FatherMobileNumberValidation"></asp:CustomValidator>
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                            <asp:Label ID="lblMotherMobileNo" runat="server" Text="Mobile No. Mother:"></asp:Label>                             
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                            <asp:TextBox ID="txtMotherMob1" runat="server" CssClass="TxtBox" MaxLength="10" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="None" ErrorMessage="Mother's Mobile number should not be blank."
                                CssClass="ClsMdtStar" ControlToValidate="txtMotherMob1"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstMotherMobileNoEmpty" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage=""
                             ClientValidationFunction="ValidateMotherMobileNo"></asp:CustomValidator>
                               
                        </td>
                        
                    </tr>
                    <tr id="trmobile2" runat="server" >
                        <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                        Mobile No.2 Father: 
                        </td>
                        <td align="left" class="TxtNormal" style="font-size: 10pt"> 
                            <asp:TextBox ID="txtFatherMob2" runat="server" CssClass="TxtBox" MaxLength="10"
                                onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false"/>
                            <asp:CustomValidator ID="cst_MobileNumber2" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ErrorMessage="" EnableClientScript="true"
                                ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                        Mobile No.2 Mother:
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                            <asp:TextBox ID="txtMotherMob2" runat="server" CssClass="TxtBox" MaxLength="10" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                        </td>
                    </tr>
                    <tr id="trMotherEmail" runat="server">
                     <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="lblFatherEmail" runat="server" Text="E-Mail ID:"></asp:Label>                            
                        </td>
                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="TxtBoxMandMid" MaxLength="100" Width="470px" />
                           <%-- <asp:RequiredFieldValidator ID="reqFatherEmail" runat="server" Display="None" ErrorMessage="Father Email ID should not be blank." ControlToValidate="txtEmail"></asp:RequiredFieldValidator>--%>
                            <asp:CustomValidator ID="cstValEmail" runat="server" ControlToValidate="txtEmail"
                                ClientValidationFunction="EmailValidator" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>
                       
                        <%--  <tr id="trMotherEmail" runat="server" visible="false">--%>
                        <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;" id="tdMoemail" runat="server">
                            <asp:Label ID="Label2" runat="server" Text="Mother E-Mail ID :"></asp:Label>                            
                        </td>
                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt" id="tdMoemail1" runat="server">
                            <asp:TextBox ID="txtMotherEmail" runat="server" CssClass="TxtBoxMandMid" MaxLength="100" Width="470px" />
                            <%--<asp:CustomValidator ID="cstValMotherEmail" runat="server" ControlToValidate="txtMotherEmail"
                                ClientValidationFunction="EmailValidation1" Display="None" ValidateEmptyText="True"></asp:CustomValidator>--%>
                          <%--  <asp:RequiredFieldValidator ID="reqMotherEmail" runat="server" Display="None" ErrorMessage="Mother Email ID should not be blank." ControlToValidate="txtMotherEmail">
                            </asp:RequiredFieldValidator>--%>
                        </td>                        
                   <%-- </tr>--%>
                 <%--   </td>--%>
                    </tr>


                     <tr>
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="lblAddressField" runat="server" Text="Address:"></asp:Label>
                        </td>
                        <td class="TxtNormal" align="left"  style="font-size: 10pt">
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="TxtBoxMand" MaxLength="300"
                                TextMode="MultiLine" Columns="21" Rows="4" Width="270px" BackColor="#ffffa0" />
                            <asp:RequiredFieldValidator ID="reqAddress" runat="server" ErrorMessage="Address should not be blank."
                                Display="None" ControlToValidate="txtAddress"> </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regAddress" runat="server" ControlToValidate="txtAddress"
                                Display="None" ErrorMessage="Address should not exceed than 300 characters."
                                ValidationExpression="^[\s\S]{0,300}$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="trPermanentAddressSameAsPresent" runat="server" visible = "false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="Label1" runat="server" Text="Permanent Address Same as Present Address"></asp:Label>
                        </td>
                        <td class="TxtNormal" align="left" colspan="2"  style="font-size: 10pt">
                            <asp:CheckBox ID="chkAddress" runat="server" />                           
                        </td>
                    </tr>
                    <tr id="trSPSPermanentAddress" runat="server" visible = "false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                             Permanent Address with Pincode : 
                        </td>
                        <td class="TxtNormal" align="left"  style="font-size: 10pt">
                            <asp:TextBox ID="txtPermanentAddress" runat="server" CssClass="TxtBox" MaxLength="300"
                                TextMode="MultiLine" Columns="21" Rows="4" Width="270px"/>                           
                        </td>
                    </tr>
                    <tr id="trDisplayArea" runat="server">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            Area:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbArea" runat="server" CssClass="TxtBox" AutoPostBack="false" Width="470px" ViewStateMode="Enabled">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cmbYear"
                                Display="None" ErrorMessage="Academic Year should be selected." Operator="NotEqual"
                                ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                        </td>
                       <%--  <td class="TxtNormal" align="left" style="font-size: 10pt">
                           Previous Standard :&nbsp;
                        </td>
                        <td align="left">
                          <asp:DropDownList ID="ddlPreStandard" runat="server" CssClass="TxtBox"  AutoPostBack="false"  ViewStateMode="Enabled"
                                        BackColor="#ffffa0" >
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>       
                         </td>  --%>
                    </tr>
                      <tr id="trlandmark" runat="server" visible="false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            Landmark :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLandmarks" runat="server" CssClass="TxtBox" MaxLength="300" Width="450px"></asp:TextBox>
                             
                        </td>
                    
                    </tr>

                     <tr>
                       <td class="TxtNormal" align="left" style="font-size: 10pt" id="tdstd" runat="server">
                           Previous Standard :&nbsp;
                        </td>
                        <td align="left" id="tdstd1" runat="server">
                          <asp:DropDownList ID="ddlPreStandard" runat="server" CssClass="TxtBox"  AutoPostBack="false"  ViewStateMode="Enabled"
                                        BackColor="#ffffa0" >
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>

                                     <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlPreStandard"
                                        Display="None" ErrorMessage="Admission sought for Previous standard should be selected."
                                        Operator="NotEqual" ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                    <%--      <asp:CustomValidator ID="cstddlPreStandard" runat="server" ErrorMessage="" ControlToValidate="ddlPreStandard" 
                            ClientValidationFunction="ValidatePreviousStandard"></asp:CustomValidator>       --%>
                           
                         </td>  
                       <%-- <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="lblFatherEmail" runat="server" Text="Father E-Mail ID:"></asp:Label>                            
                        </td>
                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="TxtBoxMandMid" MaxLength="100" Width="470px" />
                            <asp:RequiredFieldValidator ID="reqFatherEmail" runat="server" Display="None" ErrorMessage="Father Email ID should not be blank." ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstValEmail" runat="server" ControlToValidate="txtEmail"
                                ClientValidationFunction="EmailValidation" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                        </td>--%>
                        <td class="TxtNormal" align="left" style="font-size: 10pt" >
                            <span id="spCurrentSchool" runat="server">Current School:&nbsp;</span>                            
                        </td>
                        <td class="TxtNormal" align="left" colspan="1" style="font-size: 10pt" >
                            <asp:TextBox ID="txtSchoolName" runat="server" CssClass="TxtBox" MaxLength="200"
                                Width="470px" />
                            <asp:CustomValidator ID="cstCurrentSchoolName" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateCurrentSchool"></asp:CustomValidator>
                        </td>
                    </tr>
                  <%--  <tr id="trMotherEmail" runat="server" visible="false">
                        <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="Label2" runat="server" Text="Mother E-Mail ID :"></asp:Label>                            
                        </td>
                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                            <asp:TextBox ID="txtMotherEmail" runat="server" CssClass="TxtBoxMandMid" MaxLength="100" Width="470px" />
                            <asp:CustomValidator ID="cstValMotherEmail" runat="server" ControlToValidate="txtMotherEmail"
                                ClientValidationFunction="EmailValidation" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="reqMotherEmail" runat="server" Display="None" ErrorMessage="Mother Email ID should not be blank." ControlToValidate="txtMotherEmail">
                            </asp:RequiredFieldValidator>
                        </td>                        
                    </tr>--%>
                    <tr id="trLastSchoolAddress" runat="server" visible = "false">
                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 195px;">
                            Last School Address : 
                        </td>
                        <td class="TxtNormal" align="left"  style="font-size: 10pt">
                            <asp:TextBox ID="txtLastSchoolAddress" runat="server" CssClass="TxtBox" MaxLength="300"
                                TextMode="MultiLine" Columns="21" Rows="4" Width="270px"/>                            
                        </td>
                    </tr>


                  <%--  <tr>
                         <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                             <asp:Label ID="lblFatherMobileNo" runat="server" Text="Mobile No. Father:"></asp:Label>                             
                        </td>
                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                            <asp:TextBox ID="txtFatherMob1" runat="server" CssClass="TxtBoxMand" MaxLength="10" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" BackColor="#ffffa0" />
                            <asp:RequiredFieldValidator ID="reqMobileNo" runat="server" Display="None" ErrorMessage="Father's Mobile number should not be blank."
                                CssClass="ClsMdtStar" ControlToValidate="txtFatherMob1"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar" Visible="true"
                                    ErrorMessage="" EnableClientScript="true"
                                    ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                            <asp:Label ID="lblMotherMobileNo" runat="server" Text="Mobile No. Mother:"></asp:Label>                             
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                            <asp:TextBox ID="txtMotherMob1" runat="server" CssClass="TxtBox" MaxLength="10" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                            <asp:CustomValidator ID="cstMotherMobileNoEmpty" CssClass="ClsMdtStar" runat="server" Display="None" ErrorMessage="" ClientValidationFunction="ValidateMotherMobileNo"></asp:CustomValidator>
                               
                        </td>
                        
                    </tr>
                    <tr>
                        <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                        Mobile No.2 Father: 
                        </td>
                        <td align="left" class="TxtNormal" style="font-size: 10pt"> 
                            <asp:TextBox ID="txtFatherMob2" runat="server" CssClass="TxtBox" MaxLength="10"
                                onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false"/>
                            <asp:CustomValidator ID="cst_MobileNumber2" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ErrorMessage="" EnableClientScript="true"
                                ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                        Mobile No.2 Mother:
                        </td>
                        <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                            <asp:TextBox ID="txtMotherMob2" runat="server" CssClass="TxtBox" MaxLength="10" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                        </td>
                    </tr>--%>
                    <tr id="trEnquiryOther" runat="server">
                        <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="lblSiblingSchool" runat="server" Text=""></asp:Label>                                                  
                        </td>
                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                             <asp:TextBox ID="TxtSibling" runat="server" CssClass="TxtBox" MaxLength="100"  Width="470px"/>
                        </td>
                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                            Friends/Colleague:
                        </td>
                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                             <asp:TextBox ID="txtFrnd" runat="server" CssClass="TxtBox" MaxLength="100"  Width="470px" />
                        </td>
                    </tr>
                  <%--  <tr id="trsource" runat="server">
                     <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                     Source
                     </td>
                      <td align="left" class="TxtNormal" style="font-size: 10pt">
                          <asp:CheckBoxList ID="CheckBoxList1" runat="server" 
                              RepeatDirection="Horizontal" RepeatColumns="4">
                                  <asp:ListItem Text="Facebook" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Instagram" Value="0"></asp:ListItem>
                                      <asp:ListItem Text="WhatsApp" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Youtube" Value="0"></asp:ListItem>
                                           <asp:ListItem Text="Newspaper" Value="0"></asp:ListItem>
                                          <asp:ListItem Text="Walk-In" Value="0"></asp:ListItem>
                                          <asp:ListItem Text="Brochure" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Friends & Relatives" Value="0"></asp:ListItem>
                                           <asp:ListItem Text="AWS School Parent" Value="0"></asp:ListItem>
                                          <asp:ListItem Text="Other " Value="0"></asp:ListItem>
                          </asp:CheckBoxList>
                      </td>
                    </tr>--%>
                    <tr id="trHeardOfSchool" runat="server">
                        <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="lblHeardOfSchool" runat="server" Text=""></asp:Label>                                                         
                        </td>
                        <td  align="left" style="font-size: 10pt">
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td align="left">
                                    <asp:CheckBoxList ID="chklstReferences" runat="server" CssClass="TxtNormal" RepeatDirection="Horizontal" RepeatColumns="4">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                        </td>
                        <td align="left" class="TxtNormal">
                            &nbsp;
                        </td>
                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="Label6" runat="server" Text="Aadhar Card Number : "></asp:Label>                                                         
                        </td>
                        <td  align="left" style="font-size: 10pt">
                            <asp:TextBox ID="txtAadharCardNumber" runat="server" MaxLength="12" CssClass="LrgTxtBox" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);" 
                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="You have left this school on 01-Jan-2024." Display="None" ClientValidationFunction="ValidateAadharCardNumber" OnServerValidate="BlackListStudent_Validate"></asp:CustomValidator>
                        </td>
                        <td align="left" class="TxtNormal">
                            &nbsp;
                        </td>
                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" class="TxtNormal" style="font-size: 10pt">
                            <asp:HiddenField ID="hidServerDate" runat="server" />
                             <asp:HiddenField ID="hidDOB" runat="server" />
                            <asp:HiddenField ID="hidSchoolId" runat="server" Value="0" />
                                <asp:HiddenField ID="hidNextAcademiYearId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidIsMotherNameMandatory" runat="server" Value="0" />
                            <asp:HiddenField ID="hidSNSSchoolId" runat="Server" ViewStateMode="Enabled" Value="0"/>                            
                            <asp:HiddenField ID="hidSVPSchoolId" runat="Server" ViewStateMode="Enabled" Value="0"/>
                            <asp:HiddenField ID="hidSVNPSchoolId" runat="Server" ViewStateMode="Enabled" Value="0"/>
                            <asp:HiddenField ID="hidEnquiryId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidDBEnquiryId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidSPSSchoolId" runat="server" Value="132" />
                            <asp:HiddenField ID="hidDPISSchoolId" runat="server" Value="0" />
                             <asp:HiddenField ID="hidAaryanSchool" runat="server" Value="0" />
                             <asp:HiddenField ID="hidStatusId" runat="server" Value="0" />
                             <asp:HiddenField ID="hidValidateAadharCard" runat="server" Value="N" />
                            <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="ClsBtn"
                                OnClick="btnSubmit_Click" />
                            <asp:Button runat="server" ID="btnBack" Text="Back" CausesValidation="false" CssClass="ClsBtn"
                              />
                        </td>
                    </tr>
                </tbody>
            </table>
            <b class="rbottom"></b>
        </div>
        <br />
    </div>
    
<script language="javascript" type="text/javascript">
    _clienthidDOB = "<%=this.hidDOB.ClientID %>"
    _clienttxtMobile2 = "<%=this.txtMotherMob1.ClientID %>"
    _clienttxtMobile = "<%=this.txtFatherMob1.ClientID %>"

    _clienttxtEmailId = "<%=this.txtEmail.ClientID %>"
    _clienttxtMotherEmail = "<%=this.txtMotherEmail.ClientID %>"
    _clienttxtCalDobPopup = "<%=this.txtCalDobPopup.ClientID %>"

    _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>"
    _clientcst_MobileNumber2 = "<%=this.cst_MobileNumber2.ClientID %>"

    _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>"
  //  _clientcstValMotherEmail = "<=this.cstValMotherEmail.ClientID %>"

   // _clientcstValMotherEmail = "<=this.cstValMotherEmail.ClientID %>"
    _clientcstDOB = "<%=this.cstDOB.ClientID %>"
    _clienthidServerDate = "<%=this.hidServerDate.ClientID %>"
    _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>"
    _clienthidSNSSchoolId = "<%=this.hidSNSSchoolId.ClientID %>"
    _clienthidMaxBdate = "<%=this.hidMaxBdate.ClientID %>"
    _clienthidMinBdate = "<%=this.hidMinBdate.ClientID %>"
    _clienttxtMName = "<%=this.txtMName.ClientID %>"
    _clienthidIsMotherNameMandatory = "<%=this.hidIsMotherNameMandatory.ClientID %>"
    _clienttxtAddress = "<%=this.txtAddress.ClientID %>"
    _clientchkAddress = "<%=this.chkAddress.ClientID %>"
    _clienttxtPermanentAddress = "<%=this.txtPermanentAddress.ClientID %>"

    _clienthidSPSSchoolId = "<%=this.hidSPSSchoolId.ClientID %>"
    _clienthidSVPSchoolId = "<%=this.hidSVPSchoolId.ClientID %>"
    _clienthidSVNPSchoolId = "<%=this.hidSVNPSchoolId.ClientID %>"

    _clienttxtSLastName = "<%=this.txtSLastName.ClientID %>"
    _clientcstStudentFirstName = "<%=this.cstStudentFirstName.ClientID %>"

    _clienttxtFahterName = "<%=this.txtFahterName.ClientID %>"
    _clientcstStudentMiddleName = "<%=this.cstStudentMiddleName.ClientID %>"

    _clienttxtFSurname = "<%=this.txtSPSFLastName.ClientID %>"
    _clientcstFatherSurName = "<%=this.cstFatherSurName.ClientID %>"

    _clienttxtFFatherName = "<%=this.txtFFatherName.ClientID %>"
    _clientcstFatherMiddleName = "<%=this.cstFatherMiddleName.ClientID %>"

    _clienttxtMSurname = "<%=this.txtSPSMLastName.ClientID %>"
    _clientcstMotherSurName = "<%=this.cstMotherSurName.ClientID %>"

    _clienttxtMHName = "<%=this.txtMHName.ClientID %>"
    _clientcstMotherHusbandName = "<%=this.cstMotherHusbandName.ClientID %>"

    _clienttxtSchoolName = "<%=this.txtSchoolName.ClientID %>"
    _clientcstCurrentSchoolName = "<%=this.cstCurrentSchoolName.ClientID %>"

    _clienttxtMotherMob1 = "<%=this.txtMotherMob1.ClientID %>"
    _clientcstMotherMobileNoEmpty = "<%=this.cstMotherMobileNoEmpty.ClientID %>"

    _clientcmbFatherOccupation = "<%=this.cmbFatherOccupation.ClientID %>"
    _clientcstFatherOccupation = "<%=this.cstFatherOccupation.ClientID %>"
    _clientcmbMotherOccupation = "<%=this.cmbMotherOccupation.ClientID %>"
    _clientcstMotherOccupation = "<%=this.cstMotherOccupation.ClientID %>"
    _clienthidDPISSchoolId = "<%=this.hidDPISSchoolId.ClientID %>"

    _clienthidAaryanSchool = "<%=this.hidAaryanSchool.ClientID %>"
//    _clientcstddlPreStandard = "<=this.cstddlPreStandard.ClientID %>"
//    _clientddlPreStandard = "<=this.ddlPreStandard.ClientID %>"
    _clientcstSLastName = "<%=this.cstSLastName.ClientID %>"

    _clienttxtAadharCardNumber = '<%=this.txtAadharCardNumber.ClientID %>'
    _clienthidValidateAadharCard = '<%=this.hidValidateAadharCard.ClientID %>'
    
    function CopyPresentToPermanent() {
        var chkStatus = document.getElementById(_clientchkAddress);
        var PresentAddress = document.getElementById(_clienttxtAddress).value;
        var PermanentAddress = document.getElementById(_clienttxtPermanentAddress).value;

        if (chkStatus.checked) {
            document.getElementById('<%=this.txtPermanentAddress.ClientID %>').value = PresentAddress;
        }
        else {
            document.getElementById('<%=this.txtPermanentAddress.ClientID %>').value = "";
        }
    }


    function ValidatesLastName(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var AaryanSchoolId = document.getElementById(_clienthidAaryanSchool).value;
        var LastName = document.getElementById(_clienttxtSLastName).value;

        if (AaryanSchoolId=="True") {
            if (LastName == "") {
                document.getElementById(_clientcstSLastName).errormessage = "Student Last Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        args.IsValid = true;
        return false;
     }



     function ValidateFirstName(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var SPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value;
        var SVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value;
        var SVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value;
     //   var AaryanSchoolId = document.getElementById(_clienthidAaryanSchool).value;
        var LastName = document.getElementById(_clienttxtSLastName).value;

        if (SchoolId == SPSSchoolId || SchoolId == SVPSchoolId || SchoolId == SVNPSchoolId) {
            if (LastName == "") {
                document.getElementById(_clientcstStudentFirstName).errormessage = "Student Last Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        args.IsValid = true;
        return false;
    }

    function ValidateMiddleName(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var SPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value;
        var SVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value;
        var SVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value;
       
        var AaryanSchoolId = document.getElementById(_clienthidAaryanSchool).value;
        if (SchoolId == SPSSchoolId || SchoolId == SVPSchoolId || SchoolId == SVNPSchoolId) {
            var FatherName = document.getElementById(_clienttxtSLastName).value;
            if (FatherName == "") {
                document.getElementById(_clientcstStudentMiddleName).errormessage = "Student Middle Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        else if (AaryanSchoolId == "True") {
            var FatherName = document.getElementById(_clienttxtFahterName).value;
            if (FatherName == "") {
                document.getElementById(_clientcstStudentMiddleName).errormessage = "Student Middle Name should not be blank.";
                args.IsValid = false;
                return true;
            }
        }
        args.IsValid = true;
        return false;
    }

    function ValidateFatherSurName(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var SPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value;
        var SVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value;
        var SVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value;
        var FatherLastName = document.getElementById(_clienttxtFSurname).value;

        if (SchoolId == SPSSchoolId || SchoolId == SVPSchoolId || SchoolId == SVNPSchoolId) {
            if (FatherLastName == "") {
                document.getElementById(_clientcstFatherSurName).errormessage = "Father's Last Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        args.IsValid = true;
        return false;
    }

    function ValidateFatherMiddleName(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var SPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value;
        var SVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value;
        var SVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value;
        var FatherMiddleName = document.getElementById(_clienttxtFFatherName).value;

        if (SchoolId == SPSSchoolId || SchoolId == SVPSchoolId || SchoolId == SVNPSchoolId) {
            if (FatherMiddleName == "") {
                document.getElementById(_clientcstFatherMiddleName).errormessage = "Father's Middle Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        args.IsValid = true;
        return false;
    }

    function ValidateMotherSurName(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var SPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value;
        var SVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value;
        var SVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value;
        var MOtherSurName = document.getElementById(_clienttxtMSurname).value;

        if (SchoolId == SPSSchoolId || SchoolId == SVPSchoolId || SchoolId == SVNPSchoolId) {
            if (MOtherSurName == "") {
                document.getElementById(_clientcstMotherSurName).errormessage = "Mother's Last Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        args.IsValid = true;
        return false;
    }

    function ValidateMotherMiddleName(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var SPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value;
        var SVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value;
        var SVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value;
        var MOtherMiddleName = document.getElementById(_clienttxtMHName).value;

        if (SchoolId == SPSSchoolId || SchoolId == SVPSchoolId || SchoolId == SVNPSchoolId ) {
            if (MOtherMiddleName == "") {
                document.getElementById(_clientcstMotherHusbandName).errormessage = "Mother's Middle Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        args.IsValid = true;
        return false;
    }

    function ValidateCurrentSchool(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var SPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value;
        var SVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value;
        var SVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value;
        var LastSchoolName = document.getElementById(_clienttxtSchoolName).value;

        if (SchoolId == SPSSchoolId || SchoolId == SVPSchoolId || SchoolId == SVNPSchoolId) {
            if (LastSchoolName == "") {
                document.getElementById(_clientcstCurrentSchoolName).errormessage = "Last School Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        args.IsValid = true;
        return false;
    }


    function ValidateMotherMobileNo(oSrc, args) {    
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var SPSSchoolId = document.getElementById(_clienthidSPSSchoolId).value;
        var SVPSchoolId = document.getElementById(_clienthidSVPSchoolId).value;
        var SVNPSchoolId = document.getElementById(_clienthidSVNPSchoolId).value;
        var MotherMobileNo = document.getElementById(_clienttxtMotherMob1).value;
        var AaryanSchoolId = document.getElementById(_clienthidAaryanSchool).value;
        if (SchoolId == SPSSchoolId || SchoolId == SVPSchoolId || SchoolId == SVNPSchoolId || AaryanSchoolId == "True") {
            if (MotherMobileNo == "") {
                document.getElementById(_clientcstMotherMobileNoEmpty).errormessage = "Mother's Mobile No. should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
        args.IsValid = true;
        return false;
    }

    function ValidateOccupation(oSrc, args) {
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var DPISSchool = document.getElementById(_clienthidDPISSchoolId).value;
        var FatherOccu = document.getElementById(_clientcmbFatherOccupation).value;
        var MotherOccu = document.getElementById(_clientcmbMotherOccupation).value;

        if (SchoolId == DPISSchool) {
            if (FatherOccu == 0) {
                oSrc.errormessage = "Father's Occupation should be selected.";
                args.IsValid = false;
                return true;
            }

            if (MotherOccu == 0) {
                oSrc.errormessage = "Mother's occupation should be selected.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
    }

    function ValidatePreviousStandard(oSrc, args) {
    
        var SchoolId = document.getElementById(_clienthidSchoolId).value;
        var AaryanSchoolId = document.getElementById(_clienthidAaryanSchool).value;
        var PreStandard = document.getElementById(_clientddlPreStandard).value;
        if (SchoolId == AaryanSchoolId) {
            
            if (PreStandard.Selected == "Play Group" || PreStandard == "Nursery") {
               
                oSrc.errormessage = "Previous School Name should not required.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
            
            }
            args.IsValid = true;
            return false;
        }

//        function EmailValidation1(oSrc, args) {
//    var sEmailMother = document.getElementById(_clienttxtMotherEmail).value;
//    var AaryanSchoolId = document.getElementById(_clienthidAaryanSchool).value;
//  
//    sEmailMother = stripLeadingTrailingBlanks(sEmailMother)
//    if (AaryanSchoolId!=="True") {
//        if (isEmpty(sEmailMother)){
//             document.getElementById(_clientcstValMotherEmail).errormessage = "Mother Email Address should not be blank.";
//            args.IsValid = false
//            return true
//        }
//        else {
//            if (!isEmail(sEmailMother)) {
//                document.getElementById(_clientcstValMotherEmail).errormessage = "Mother Email Address should be in valid format(For Example :\" john.smith@yahoo.com \").";
//                args.IsValid = false
//                return true
//            }
//        }
//   }
//    else {
//        if (!isEmpty(sEmailMother)) {
//            if (!isEmpty(sEmailMother)){
//                  document.getElementById(_clientcstValMotherEmail).errormessage = "Mother Email Address should be in valid format(For Example :\" john.smith@yahoo.com \").";
//                args.IsValid = false
//                return true
//            }
//        }
//    }
//    args.IsValid = true
//    return false
        //}

        function MobileNumberValidator(oSrc, args) {
            var fatherMobileNo = document.getElementById("<%=this.txtFoWhatsup.ClientID %>").value;
            if (fatherMobileNo == "") {
                oSrc.errormessage = "Father WhatsApp No. should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (fatherMobileNo.length != 10) {
                oSrc.errormessage = "Father WhatsApp No. should be of 10 digits.";
                args.IsValid = false;
                return true;
            }
            else if (fatherMobileNo.substring(0,1)=="0") {
                oSrc.errormessage = "Father WhatsApp No. should not start with zero.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function FatherMobileNumberValidation(oSrc, args) {
            var fatherMobileNo = document.getElementById(_clienttxtMobile).value;
            if (fatherMobileNo == "") {
                oSrc.errormessage = "Father's Mobile No. should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (fatherMobileNo.length != 10) {
                oSrc.errormessage = "Father's Mobile No. should be of 10 digits.";
                args.IsValid = false;
                return true;
            }
            else if (fatherMobileNo.substring(0, 1) == "0") {
                oSrc.errormessage = "Father's Mobile No. should not start with zero.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        

        function MotherMobileNumberValidator(oSrc, args) {
            var motherMobileNo = document.getElementById("<%=this.txtMoWhatsup.ClientID %>").value;
            if (motherMobileNo == "") {
                oSrc.errormessage = "Mother WhatsApp No. should not be blank.";
                args.IsValid = false;
                return true;
            }
            else if (motherMobileNo.length != 10) {
                oSrc.errormessage = "Mother WhatsApp No. should be of 10 digits.";
                args.IsValid = false;
                return true;
            }
            else if (motherMobileNo.substring(0, 1) == "0") {
                oSrc.errormessage = "Mother WhatsApp No. should not start with zero.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function EmailValidator(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtEmailId).value
            sEmail = stripLeadingTrailingBlanks(sEmail)
       
                if (isEmpty(sEmail)) {
                    document.getElementById(_clientcstValEmailId).errormessage = "Father Email Address should not be blank."
                    args.IsValid = false
                    return true
                }
                else {
                    if (!isEmail(sEmail)) {
                        document.getElementById(_clientcstValEmailId).errormessage = "Father Email Address should be in valid format."
                        args.IsValid = false
                        return true
                    }
                }
            
            args.IsValid = true
            return false
        }

        function ValidateAadharCardNumber(oSrc, args) {
            var aadharNo = document.getElementById(_clienttxtAadharCardNumber).value
            var validate = document.getElementById(_clienthidValidateAadharCard).value

            if (aadharNo == '' && validate == 'Y') {
                oSrc.errormessage = 'Aadhar Card Number should not be blank.'
                args.IsValid = false;
                return true;
            }
                        
            if (aadharNo != '') {
                if (aadharNo.length != 12) {
                    oSrc.errormessage = 'Aadhar Card Number length should be 12 digit long.'
                    args.IsValid = false;
                    return true;
                }
                else if (parseInt(aadharNo) == 0) {
                    oSrc.errormessage = 'Aadhar Card Number should not be 0.'
                    args.IsValid = false;
                    return true;
                }
            }
            
            args.IsValid = true;
            return false;
        }

</script> 

    <script type="text/javascript" src="../Scripts/Admission/Enquiry.js?version=1.7"></script>
</asp:Content>
