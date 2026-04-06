<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmissionNew.master"
    AutoEventWireup="true" CodeFile="StudentRegistrationDetails.aspx.cs" Inherits="StudentRegistrationDetails" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <style>
            .trHeight {
                height:30px;
            }
         </style>
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="90%">
            <tr align="center" style="text-align: center; margin: 0px auto;">
                <td id="tdMessage" runat="server" align="center" colspan="6">
                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="left" colspan="6">
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                        ShowSummary="true" ValidationGroup="Save" />
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 30px" valign="bottom" colspan="6">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                        Text="Mandatory Fields"></asp:Label>
                </td>
            </tr>
             <tr id="trEnquiry" runat="server" visible="false">
               <td align="left" style="width: 195px" >
                 <asp:Label ID="lblEnquiryName" runat="server" Text="Enquiry No: "></asp:Label>                                               
               </td>
                <td  align="left" colspan="3">
                    <asp:TextBox ID="txtEnqNo" runat="server" ReadOnly="true" ViewStateMode="Enabled"></asp:TextBox>
               </td>
             </tr>
            <tr class="trHeight">
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    For Academic Year:
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbYear" runat="server" CssClass="MidTxtNormalAdmission" AutoPostBack="true" Enabled="false"
                        ViewStateMode="Enabled">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="cmp_valYear" runat="server" ControlToValidate="cmbYear"
                        Display="None" ErrorMessage="Academic Year should be selected." Operator="NotEqual"
                        ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    Admission sought for:&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbStd" runat="server" CssClass="MidTxtNormalAdmission" AutoPostBack="True"
                        ViewStateMode="Enabled" 
                        onselectedindexchanged="cmbStd_SelectedIndexChanged">
                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblStdName" runat="server" Font-Bold="true"></asp:Label>
                    <asp:CompareValidator ID="cmp_valStdr" runat="server" ControlToValidate="cmbStd"
                        Display="None" ErrorMessage="Admission sought for standard should be selected."
                        Operator="NotEqual" ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                    <asp:HiddenField ID="hidMinBdate" runat="server" />
                    <asp:HiddenField ID="hidMaxBdate" runat="server" />
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt" id="tdReceiptNo" runat="server">
                    <span class="ClsLabel">Receipt No:</span>
                </td>
                <td align="left" id="tdReceiptNo1" runat="server" >
                    <asp:TextBox ID="txtManualReceiptNo"  runat="server" CssClass="MidTxtNormalAdmission" MaxLength="6" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);" 
                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>
                    
                </td>
            </tr>
            <tr class="trHeight">
                <td colspan="1" align="left" class="TxtNormal" valign="top" style="font-size: 10pt">
                    Student's Name:
                </td>
                <td colspan="5">
                     <table align="left" cellpadding="0" cellspacing="1">
                         <tr>
                             <td class="TxtNormal" align="left" style="font-size: 10pt;width:100px;">
                                 First Name
                             </td>
                             <td class="TxtNormal" style="font-size: 10pt">
                                <asp:TextBox runat="server" CssClass="MidTxtNormalAdmission" ID="txtFirstName" MaxLength="50" onblur="formatName(this)"   BackColor="#ffffa0"/>
                                <asp:RequiredFieldValidator ID="reqSName" runat="server" ErrorMessage="Student's First Name should not be blank."
                                    Display="None" ControlToValidate="txtFirstName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>                             
                             <td class="TxtNormal" align="left" style="font-size: 10pt;width:130px;padding-left:20px;">
                                 Middle Name
                             </td>
                              <td class="TxtNormal" style="font-size: 10pt">
                                     <asp:TextBox runat="server" CssClass="MidTxtNormalAdmission" ID="txtMiddleName" MaxLength="50"
                                         onblur="formatName(this)"  BackColor="#ffffa0"/>
                                     <asp:RequiredFieldValidator ID="reqMiddleName" runat="server" ErrorMessage="Student's Middle Name should not be blank."
                                         Display="None" ControlToValidate="txtMiddleName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                     <span class="ClsMdtStar">* </span>
                             </td>
                             <td class="TxtNormal" align="left" style="font-size: 10pt;width:130px;padding-left:20px;">
                                 Last Name
                             </td>
                             <td class="TxtNormal" style="font-size: 10pt">
                                 <asp:TextBox runat="server" CssClass="MidTxtNormalAdmission" ID="txtLastName" MaxLength="50" onblur="formatName(this)"  BackColor="#ffffa0" />
                                 <asp:RequiredFieldValidator ID="reqLastName" runat="server" ErrorMessage="Student's Last Name should not be blank."
                                     Display="None" ControlToValidate="txtLastName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                 <span class="ClsMdtStar">* </span>
                             </td>
                         </tr>
                     </table>
                </td>
            </tr>            
            <tr class="trHeight">
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    Gender :
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt;padding-left:0px;">
                    <asp:RadioButton ID="rdoMale" Text="Male" runat="server" GroupName="rdoGroupSex"
                        CssClass="ClsLabel" Checked="True" ViewStateMode="Enabled"></asp:RadioButton>
                    <asp:RadioButton ID="rdoFemale" Text="Female" runat="server" GroupName="rdoGroupSex"
                        CssClass="ClsLabel" ViewStateMode="Enabled"></asp:RadioButton>
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    Date of Birth :
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt" colspan="3">
                    <asp:TextBox ID="txtCalDobPopup" CssClass="MidTxtNormalAdmission" runat="server" AutoPostBack="True"  BackColor="#ffffa0">
                    </asp:TextBox><rjs:PopCalendar ID="CalDobPopup" runat="server" Control="txtCalDobPopup"
                        Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth." />
                    <asp:Label ID="lblAge" runat="server" CssClass="LblI" Style="font-size: 14px; font-family: Cambria;
                        font-weight: bold;"></asp:Label>
                    <asp:RequiredFieldValidator ID="reqDOB" runat="server" ErrorMessage="Date of Birth should not be blank."
                        Display="None" ControlToValidate="txtCalDobPopup" ValidationGroup="Save"> </asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cstDOB" Display="None" runat="server" CssClass="ClsMdtStar"
                        ControlToValidate="txtCalDobPopup" Visible="true" EnableClientScript="true" ClientValidationFunction="checkDOB" ValidationGroup="Save"></asp:CustomValidator>

                     <span class="ClsMdtStar">* </span>
                </td>
            </tr>
            <tr class="trHeight">
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    Place of Birth :
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    <asp:TextBox runat="server" CssClass="MidTxtNormalAdmission" ID="txtBirthPlace" MaxLength="50"  BackColor="#ffffa0" />
                    <asp:RequiredFieldValidator ID="reqBirthPlace" runat="server" ErrorMessage="Place Of Birth should not be blank."
                        Display="None" ControlToValidate="txtBirthPlace" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    Birth Taluka :
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    <asp:TextBox runat="server" CssClass="MidTxtNormalAdmission" ID="txtBirthTaluka" MaxLength="50"  BackColor="#ffffa0" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Birth Taluka should not be blank."
                        Display="None" ControlToValidate="txtBirthTaluka" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    Birth District :
                </td>
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    <asp:TextBox runat="server" CssClass="MidTxtNormalAdmission" ID="txtBirthDistrict" MaxLength="50"   BackColor="#ffffa0"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Birth Distict should not be blank."
                        Display="None" ControlToValidate="txtBirthDistrict" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>
                </td>
            </tr>
            <tr class="trHeight">
                <td class="TxtNormal" align="left" style="font-size: 10pt">
                    Last School Name :
                </td>
                <td class="TxtNormal" align="left" colspan="3" style="font-size: 10pt">
                    <asp:TextBox ID="txtSchoolName" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="200"
                        Width="569px"  BackColor="#ffffa0" />
                    <asp:RequiredFieldValidator ID="reqLastSchoolName" runat="server" ErrorMessage="Last School Name should not be blank."
                        Display="None" ControlToValidate="txtSchoolName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span visible="false" id="spnMdtLastSchoolName" runat="server" class="ClsMdtStar">* </span>
                </td>
            </tr>
            <tr class="trHeight">
                        <td align="left" class="TxtNormal" style="font-size: 10pt; width: 195px;">
                            <asp:Label ID="Label29" runat="server" Text="Aadhar Card Number : "></asp:Label>                                                         
                        </td>
                        <td  align="left" style="font-size: 10pt">
                            <asp:TextBox ID="txtAadharCardNumber" runat="server" MaxLength="12" CssClass="MidTxtNormalAdmission" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);" 
                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="You have left this school on 01-Jan-2024." ValidationGroup="Save" Display="None" ClientValidationFunction="ValidateAadharCardNumber" OnServerValidate="BlackListStudent_Validate"></asp:CustomValidator>
                        </td>                        
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td class="TxtNormal" align="left" style="font-size: 10pt" colspan="5">
                    <b>CORRESPONDANCE & BUS PICK UP :</b>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr class="trHeight">
                <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt; width: 220px;
                    height: 20px;">
                    House Name/Plot no :
                </td>
                <td align="left" class="TxtNormal" style="font-size: 10pt; height: 20px;" colspan="1">
                    <asp:TextBox ID="txtHouseNo" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" BackColor="#ffffa0" />
                    <span class="ClsMdtStar">* </span>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="House Name/Plot no should not be blank."
                        Display="None" ControlToValidate="txtHouseNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left" class="TxtNormal" style="font-size: 10pt; height: 20px;" colspan="1">
                    Land Mark :
                </td>
                <td align="left" class="TxtNormal" style="font-size: 10pt; height: 20px;">
                    <asp:TextBox ID="txtLandmark" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="50" BackColor="#ffffa0" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Land Mark should not be blank."
                        Display="None" ControlToValidate="txtLandmark" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>
                </td>
            </tr>
            <tr class="trHeight">
                <td align="left" class="TxtNormal" style="font-size: 10pt">
                    Main Area/Lane :
                </td>
                <td align="left" class="TxtNormal" style="font-size: 10pt">
                    <asp:TextBox ID="txtMainArea" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" BackColor="#ffffa0"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Main Area/Lane should not be blank."
                        Display="None" ControlToValidate="txtMainArea" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>
                </td>
            </tr>
            <tr class="trHeight">
                <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                    City :
                </td>
                <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                    <asp:TextBox ID="txtCity" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" BackColor="#ffffa0" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="City should not be blank."
                        Display="None" ControlToValidate="txtCity" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>
                </td>
                <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                    Taluka :
                </td>
                <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                    <asp:TextBox ID="txttaluka" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" BackColor="#ffffa0" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Taluka should not be blank."
                        Display="None" ControlToValidate="txttaluka" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>
                </td>
                <td align="left" class="TxtNormal" style="font-size: 10pt">
                    District :
                </td>
                <td align="left" class="TxtNormal" style="font-size: 10pt">
                    <asp:TextBox ID="txtDistrict" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100"  BackColor="#ffffa0"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="District should not be blank."
                        Display="None" ControlToValidate="txtDistrict" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>
                </td>
            </tr>
            <tr class="trHeight">
                <td class="TxtNormal" align="left" style="font-size: 10pt" colspan="1">
                    PERMANENT ADDRESS :
                </td>
                <td align="left" class="TxtNormal" colspan="5" style="font-size: 10pt">                  
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="200" TextMode="MultiLine"
                        Columns="20" Rows="4" Width="300px" BackColor="#ffffa0" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Permanent Address should not be blank."
                        Display="None" ControlToValidate="txtAddress" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    <span class="ClsMdtStar">* </span>                          
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td class="TxtNormal" align="left" style="font-size: 10pt" colspan="5">
                    <b>FATHER / MOTHER DETAILS :</b>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr class="trHeight">
                <td colspan="6">
                    <table style="width: 100%;">
                        <tr>
                            <td style="font-size: 10pt; width: 250px;">
                            </td>
                            <td align="left">
                                <strong>FATHER</strong>
                            </td>
                            <td align="left">
                                <strong>MOTHER</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td class="TxtNormal" style="font-size: 10pt; width: 250px;">
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="First Name" Width="110px" class="TxtNormal"
                                    align="left" Style="font-size: 10pt"></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text="Middle Name" Width="110px" class="TxtNormal"
                                    align="left" Style="font-size: 10pt"></asp:Label>
                                <asp:Label ID="Label3" runat="server" Text="Last Name" Width="110px" class="TxtNormal"
                                    align="left" Style="font-size: 10pt"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="First Name" Width="110px" class="TxtNormal"
                                    align="left" Style="font-size: 10pt"></asp:Label>
                                <asp:Label ID="Label5" runat="server" Text="Middle Name" Width="110px" class="TxtNormal"
                                    align="left" Style="font-size: 10pt"></asp:Label>
                                <asp:Label ID="Label6" runat="server" Text="Last Name" Width="110px" class="TxtNormal"
                                    align="left" Style="font-size: 10pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TxtNormal" style="font-size: 10pt; width: 250px;">
                                Name :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFFirstName" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100"
                                    Width="110px" />
                                 <asp:RequiredFieldValidator ID="reqFFirstName" runat="server" ErrorMessage="Student's Father Name should not be blank."
                                    Display="None" ControlToValidate="txtFFirstName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                               <span id="spnFFirstName" runat="server" class="ClsMdtStar">*</span>
                                <asp:TextBox ID="txtFMiddleName" runat="server" CssClass="MidTxtNormalAdmission"
                                    MaxLength="100" Width="110px" />
                                 <asp:RequiredFieldValidator ID="reqFMiddleName" runat="server" ErrorMessage="Student's father's Middle Name should not be blank."
                                    Display="None" ControlToValidate="txtFMiddleName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                               <span id="spnFMiddleName" runat="server" class="ClsMdtStar">*</span>
                                <asp:TextBox ID="txtFLastName" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100"
                                    Width="110px" />
                                 <asp:RequiredFieldValidator ID="reqFLastName" runat="server" ErrorMessage="Student's father's Last Name should not be blank."
                                    Display="None" ControlToValidate="txtFLastName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span id="spnFLastName" runat="server" class="ClsMdtStar">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMFirstName" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100"
                                    Width="110px" />
                                <asp:RequiredFieldValidator ID="reqMFirstName" runat="server" ErrorMessage="Student's Mother Name should not be blank."
                                    Display="None" ControlToValidate="txtMFirstName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                               <span id="spnMFirstName" runat="server" class="ClsMdtStar">*</span>
                                <asp:TextBox ID="txtMMiddleName" runat="server" CssClass="MidTxtNormalAdmission"
                                    MaxLength="100" Width="110px" />
                                 <asp:RequiredFieldValidator ID="reqMMiddleName" runat="server" ErrorMessage="Student's Mother's Middle Name should not be blank."
                                    Display="None" ControlToValidate="txtMMiddleName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                  <span id="spnMMiddleName" runat="server" class="ClsMdtStar">*</span>
                                <asp:TextBox ID="txtMLastName" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100"
                                    Width="110px" />
                                 <asp:RequiredFieldValidator ID="reqMLastName" runat="server" ErrorMessage="Student's Mother's Last Name should not be blank."
                                    Display="None" ControlToValidate="txtMLastName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                   <span id="spnMLastName" runat="server" class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td class="TxtNormal" style="font-size: 10pt">
                                Educational Qualification :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFQuali" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100"
                                    Width="200px" BackColor="#ffffa0" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Father Educational Qualification should not be blank."
                                    Display="None" ControlToValidate="txtFQuali" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMQuali" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100"
                                    Width="200px" BackColor="#ffffa0"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Mother Educational Qualification should not be blank."
                                    Display="None" ControlToValidate="txtMQuali" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="TxtNormal" style="font-size: 10pt">
                                Occupation :
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbFOccupation" runat="server" CssClass="TxtNormalAdmissionMandAdmission"
                                    ViewStateMode="Enabled" Width="100px" BackColor="#ffffa0">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="cmp_valFOcc" runat="server" ControlToValidate="cmbFOccupation"
                                    Display="None" ErrorMessage="Father Occupation should be selected." Operator="NotEqual"
                                    ValueToCompare="0" CssClass="ClsLabel" ValidationGroup="Save"></asp:CompareValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbMOccupation" runat="server" CssClass="TxtNormalAdmission" onselectedindexchanged="cmbMOccupation_SelectedIndexChanged" AutoPostBack="True"
                                    ViewStateMode="Enabled" Width="100px" BackColor="#ffffa0">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cmbMOccupation"
                                    Display="None" ErrorMessage="Mother Occupation should be selected." Operator="NotEqual"
                                    ValueToCompare="0" CssClass="ClsLabel" ValidationGroup="Save"></asp:CompareValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="TxtNormal" style="font-size: 10pt">
                                Company/Organisation Address :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFOrgAddress" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"
                                    ViewStateMode="Enabled" TextMode="MultiLine" Columns="20" Rows="4" Width="300px" BackColor="#ffffa0" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Father Company/Organisation Address should not be blank."
                                    Display="None" ControlToValidate="txtFOrgAddress" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMOrgAddress" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"
                                    TextMode="MultiLine" Columns="20" Rows="4" Width="300px" />  
                                 <asp:RequiredFieldValidator ID="reqMOrgAddress" runat="server"  ControlToValidate="txtMOrgAddress"  ErrorMessage="Mother Organization Address should not be blank."
                                 Display="None"  ValidationGroup="Save"> </asp:RequiredFieldValidator>  
                                <span id="spnMOrgAddress" runat="server" class="ClsMdtStar">*</span>                          
                            </td>
                        </tr>
                        <tr>
                            <td class="TxtNormal" style="font-size: 10pt">
                                Office Telephone No :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFOffPhone" runat="server" CssClass="TxtNormalAdmission" MaxLength="20"
                                    onblur="extractNumber(this,0,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                    onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" BackColor="#ffffa0"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="FatherOffice Telephone No. should not be blank."
                                    Display="None" ControlToValidate="txtFOffPhone" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span  class="ClsMdtStar">*</span>     
                            </td>
                            <td>
                                <asp:TextBox ID="txtMOffPhone" runat="server" CssClass="TxtNormalAdmission" MaxLength="20"
                                    onblur="extractNumber(this,0,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                    onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" />  
                                <asp:RequiredFieldValidator ID="reqMOffPhone" runat="server"  ControlToValidate="txtMOffPhone"
                                      ErrorMessage="Mother Office Phone should not be blank." Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator> 
                                 <span id="spnMOffPhone" runat="server" class="ClsMdtStar">*</span>                            
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1" style="font-size: 10pt">
                                Mobile Number :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFMobNo" runat="server" MaxLength="10" CssClass="TxtNormalAdmission"
                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                    onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                    ondrop="event.returnValue=false" BackColor="#ffffa0"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Father Mobile Number should not be blank."
                                    Display="None" ControlToValidate="txtFMobNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMMobNo" runat="server" MaxLength="10" CssClass="TxtNormalAdmission"
                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                    onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                    ondrop="event.returnValue=false" BackColor="#ffffa0"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Mother Mobile Number should not be blank."
                                    Display="None" ControlToValidate="txtMMobNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="TxtNormal" style="font-size: 10pt">
                                E-mail Address :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFEmail" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"
                                    TextMode="SingleLine" Columns="20" Rows="4" Width="200px" BackColor="#ffffa0" />
                                <asp:RegularExpressionValidator ID="regvalFEmail" runat="server" ControlToValidate="txtFEmail"
                                    Display="None" ErrorMessage="Father Email Address should be in valid format(For Example :\&quot; john.smith@yahoo.com \&quot;)."
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Father E-mail Address should not be blank."
                                    Display="None" ControlToValidate="txtFEmail" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <span class="ClsMdtStar">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMEmail" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"
                                    TextMode="SingleLine" Columns="20" Rows="4" Width="200px" />
                                <asp:RegularExpressionValidator ID="regvalMEmail" runat="server" ControlToValidate="txtMEmail"
                                    Display="None" ErrorMessage="Mother Email Address should be in valid format(For Example :\&quot; john.smith@yahoo.com \&quot;)."
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="reqMEmail" runat="server" ControlToValidate="txtMEmail" ErrorMessage="Mother Email should not be blank." Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>    
                                <span id="spnMEmail" runat="server" class="ClsMdtStar">*</span>                           
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;">
                </td>
            </tr>
            <tr id="trSNSBrotherDetails" runat="server" visible="true">
                <td class="TxtNormal" style="font-size: 10pt" colspan="5">
                    <b>Details of Brothers and Sisters of the student :</b>
                </td>
            </tr>
            <tr class="trHeight">
                <td colspan="4">
                    <table>
                        <tr>
                            <td style="width: 250px; font-size: 10pt" class="TxtNormal">
                                Name
                            </td>
                            <td style="width: 50px; font-size: 10pt" class="TxtNormal">
                                Age
                            </td>
                            <td style="width: 250px; font-size: 10pt" class="TxtNormal">
                                Name of the Institution
                            </td>
                            <td style="width: 100px; font-size: 10pt" class="TxtNormal">
                                Standard
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtBName1" runat="server" CssClass="MidTxtNormalAdmission" Width="250px" />
                            </td>
                            <td style="width: 50px;">
                                <asp:TextBox ID="txtBAge1" runat="server" CssClass="MidTxtNormalAdmission" Width="50px"
                                    MaxLength="2" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                    ondrop="event.returnValue=false" />
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtBInstitution1" runat="server" CssClass="MidTxtNormalAdmission"
                                    Width="250px" />
                            </td>
                            <td style="width: 100px;">
                                <asp:TextBox ID="txtBStandard1" runat="server" CssClass="MidTxtNormalAdmission" Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtBName2" runat="server" CssClass="MidTxtNormalAdmission" Width="250px" />
                            </td>
                            <td style="width: 50px;">
                                <asp:TextBox ID="txtBAge2" runat="server" CssClass="MidTxtNormalAdmission" Width="50px"
                                    MaxLength="2" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                    ondrop="event.returnValue=false" />
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtBInstitution2" runat="server" CssClass="MidTxtNormalAdmission"
                                    Width="250px" />
                            </td>
                            <td style="width: 100px;">
                                <asp:TextBox ID="txtBStandard2" runat="server" CssClass="MidTxtNormalAdmission" Width="100px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;">
                </td>
            </tr>
            <tr id="trSNSStrimwiseSubjects" runat="server" visible="false" class="trHeight">
                                    <td colspan="6">
                                        <table style="width:100%; text-align:left; margin:0px auto;">
                                            <tr>
                                                <td style="height:10pt;"></td>
                                            </tr>
                                            <tr>
                                               <td class="TxtNormal" style="font-size: 10pt" colspan="2">
                                                    <b>Stream Wise Subject Details.</b>
                                               </td>  
                                            </tr>
                                            <tr>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:Label runat = "server" ID = "Label7">Select Stream :</asp:Label> 
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbStream" runat="server" CssClass="TxtNormalAdmission" 
                                                       ViewStateMode="Enabled" OnSelectedIndexChanged="cmbStream_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="SCIENCE" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="COMMERCE" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="ARTS" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="ABROAD EDUCATION EXAMINATION" Value="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:CustomValidator ID="cstStream" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="ValidateStream" ErrorMessage="" ValidationGroup="Save"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cstStreamDetails" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="validateSubjectDetails" ErrorMessage="" ValidationGroup="Save"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height:20px;"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table style="text-align:center; margin:0px auto;" align="center">                                                        
                                                        <tr id="trScienceStream" style="display:none;" runat="server">
                                                            <td>   
                                                                <table border="1" cellpadding="1" cellspacing="1">
                                                                    <tr>                                            
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:133pt;">
                                                                            <asp:Label runat = "server" ID = "Label8"><b>GROUP</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:280pt;">
                                                                            <asp:Label runat = "server" ID = "Label9"><b>COMPULSORY SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                            <asp:Label runat = "server" ID = "Label10"><b>OPTIONAL SUBJECTS (Select any one)</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:200pt;">
                                                                            <asp:Label runat = "server" ID = "Label11"><b>COMPETITIVE EXAMS COACHING</b></asp:Label> 
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria;">
                                                                            <asp:RadioButton ID="rdoStream_SciGroupOne" Text="1" runat="server" GroupName="Science" onclick="ResetSubjects(1)"></asp:RadioButton>                                                                            
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 10pt;">
                                                                            English, Physics, Chemistry, Maths
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">
                                                                            <asp:RadioButton ID="rdoStream_SciGr1PhyEdu" Text="Physical education" runat="server" Font-Size="12" GroupName="GroupOne"></asp:RadioButton> <br /><br />
                                                                            <asp:RadioButton ID="rdoStream_SciGr1CompSci" Text="Computer Science" runat="server" GroupName="GroupOne"></asp:RadioButton>
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">  
                                                                            <asp:CheckBox ID="chkStream_SciGr1JEE" runat="server" CssClass="LblSml" Text="JEE" />  
                                                                            <asp:CheckBox ID="chkStream_SciGr1ExtraCo" runat="server" CssClass="LblSml" Text="EXTRACOACHING" />                                                                                                                                                                                                                                 
                                                                         </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria;">
                                                                            <asp:RadioButton ID="rdoStream_SciGroupTwo" Text="2" runat="server" GroupName="Science" onclick="ResetSubjects(2)"></asp:RadioButton>
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            English, Physics, Chemistry, Biology
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">
                                                                            <asp:RadioButton ID="rdoStream_SciGr2PhyEdu" Text="Physical education" runat="server" Font-Size="12" GroupName="GroupOne"></asp:RadioButton> <br /><br />
                                                                            <asp:RadioButton ID="rdoStream_SciGr2CompSci" Text="Computer Science" runat="server" GroupName="GroupOne"></asp:RadioButton>
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">  
                                                                            <asp:CheckBox ID="chkStream_SciGr2Neet" runat="server" CssClass="LblSml" Text="NEET" />  
                                                                            <asp:CheckBox ID="chkStream_SciGr2ExtraCO" runat="server" CssClass="LblSml" Text="EXTRACOACHING" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>   
                                                        <tr id="trCommerceStream" style="display:none;" runat="server" >
                                                            <td>   
                                                                <table border="1" cellpadding="0" cellspacing="0">
                                                                    <tr>                                            
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:133pt;">
                                                                            <asp:Label runat = "server" ID = "Label12"><b>GROUP</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                            <asp:Label runat = "server" ID = "Label13"><b>COMPULSORY SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                            <asp:Label runat = "server" ID = "Label14"><b>OPTIONAL SUBJECTS (Select any one)</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            <asp:Label runat = "server" ID = "Label15"><b>COMPETITIVE EXAMS COACHING</b></asp:Label> 
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria;">
                                                                            1
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            English, Business Studies, Accounts, Economics
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">
                                                                            <asp:RadioButton ID="rdoStream_ComMaths" Text="Maths" runat="server" Font-Size="12" GroupName="Commerce"></asp:RadioButton> <br /><br />
                                                                            <asp:RadioButton ID="rdoStream_ComPhyEdu" Text="Physical education" runat="server" GroupName="Commerce"></asp:RadioButton> <br /><br />
                                                                            <asp:RadioButton ID="rdoStream_ComLeagalStudies" Text="Legal Studies" runat="server" GroupName="Commerce"></asp:RadioButton>
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">    
                                                                            <asp:CheckBox ID="chkStream_ComCA" runat="server" CssClass="LblSml" Text="CA" />
                                                                            <asp:CheckBox ID="chkStream_ComExtraCo" runat="server" CssClass="LblSml" Text="EXTRACOACHING" />                                                                                                                                                                                                                                
                                                                         </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trArtsStream" style="display:none;" runat="server">
                                                            <td>   
                                                                <table border="1" cellpadding="0" cellspacing="0">
                                                                    <tr>                                            
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:133pt;">
                                                                            <asp:Label runat = "server" ID = "Label16"><b>GROUP</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                            <asp:Label runat = "server" ID = "Label17"><b>COMPULSORY SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                            <asp:Label runat = "server" ID = "Label18"><b>OPTIONAL SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            <asp:Label runat = "server" ID = "Label19"><b>COMPETITIVE EXAMS COACHING</b></asp:Label> 
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; width:130pt;">
                                                                            1
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                            English, History, Psychology
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; width:250pt; text-align:left;">
                                                                            <asp:RadioButton ID="rdoStream_ArtLegalSci" Text="Legal Studies <b>OR</b>" runat="server" Font-Size="12" GroupName="ArtLegalStud"></asp:RadioButton><br />
                                                                            <asp:RadioButton ID="rdoStream_ArtPhyEdu" Text="PhysicalEducation" runat="server" GroupName="ArtLegalStud"></asp:RadioButton>
                                                                            <br /><br />
                                                                            <asp:RadioButton ID="rdoStream_ArtGerman" Text="German <b>OR</b>" runat="server" Font-Size="12" GroupName="ArtGerman"></asp:RadioButton><br />
                                                                            <asp:RadioButton ID="rdoStream_ArtEconomics" Text="Economics" runat="server" GroupName="ArtGerman"></asp:RadioButton>
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">     
                                                                            <asp:CheckBox ID="chkStream_ArtClat" runat="server" CssClass="LblSml" Text="CLAT" />
                                                                            <asp:CheckBox ID="chkStream_ArtExtraCo" runat="server" CssClass="LblSml" Text="EXTRACOACHING" />                                                                            
                                                                         </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trAbroadEducation" style="display:none;" runat="server">
                                                            <td>   
                                                                <table border="1" cellpadding="0" cellspacing="0">
                                                                    <tr>                                            
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:133pt;">
                                                                            <asp:Label runat = "server" ID = "Label20"><b>GROUP</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                            <asp:Label runat = "server" ID = "Label21"><b>COMPULSORY SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                            <asp:Label runat = "server" ID = "Label22"><b>OPTIONAL SUBJECTS (Select any one)</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            <asp:Label runat = "server" ID = "Label23"><b>COMPETITIVE EXAMS COACHING</b></asp:Label> 
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; width:130pt;">
                                                                            1
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                            SAT
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">                                                                           
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">
                                                                            <asp:RadioButton ID="rdoStream_AbrodEduNo" Text="No" runat="server" GroupName="Financially"></asp:RadioButton>
                                                                            <asp:RadioButton ID="rdoStream_AbrodEduYes" Text="Yes" runat="server" CssClass="ClsLabel" GroupName="Financially"></asp:RadioButton>
                                                                         </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>                                                    
                                            <tr>
                                                <td style="height:5px;"></td>
                                            </tr>
                                            </table>
                                    </td>
                                </tr>
                                <tr id="tr9thSubjectCombination" runat="server" visible = "false">
                                    <td colspan="6">
                                         <table style="width:100%; text-align:left; margin:0px auto;">
                                            <tr>
                                                <td style="height:10pt;"></td>
                                            </tr>
                                            <tr>
                                               <td class="TxtNormal" style="font-size: 10pt" colspan="2">
                                                    <b>GRADE IX SUBJECT COMBINATION</b>
                                               </td>  
                                            </tr>
                                            <tr>
                                                <td colspan="2">   
                                                    <table border="1" style="text-align:center; width:70%; margin:0px auto;" cellpadding="0" cellspacing="0">
                                                        <tr>                                            
                                                            <td class="TxtNormal" style="font-size: 10pt; width:100pt;">
                                                                <asp:Label runat = "server" ID = "Label24"></asp:Label> 
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt; width:50pt;">
                                                                <asp:Label runat = "server" ID = "Label25"></asp:Label> 
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                <asp:Label runat = "server" ID = "Label26"><b>Compulsory Subjects</b></asp:Label> 
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                <asp:Label runat = "server" ID = "Label27"><b>Optional Subjects (Select any one)</b></asp:Label> 
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                <asp:Label runat = "server" ID = "Label28"><b>Optional Subjects</b></asp:Label> 
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:center;">
                                                                GRADE IX
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:center;">
                                                                1
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt;">
                                                                English, Science, SST
                                                             </td>
                                                             <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">   
                                                                <asp:RadioButton ID="rdo9th_Hindi" Text="Hindi" runat="server" Font-Size="12" GroupName="9thFirstGroup"></asp:RadioButton><br />
                                                                <asp:RadioButton ID="rdo9th_Marathi" Text="Marathi" runat="server" GroupName="9thFirstGroup"></asp:RadioButton><br />
                                                                <asp:RadioButton ID="rdo9th_Sanskrit" Text="Sanskrit" runat="server" GroupName="9thFirstGroup"></asp:RadioButton>                                                                        
                                                             </td>
                                                             <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">   
                                                                <asp:RadioButton ID="rdo9th_MathsStd" Text="Mathematics standard" runat="server" Font-Size="12" GroupName="9thSecondGroup"></asp:RadioButton><br />
                                                                <asp:RadioButton ID="rdo9th_MathsBasic" Text="Mathematics Basic" runat="server" GroupName="9thSecondGroup"></asp:RadioButton><br />                                                                
                                                             </td>                                                             
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                <td style="height: 20px;">
                </td>
            </tr>
        </table>
        <table style="text-align: center; margin: 0px auto; border: 1px auto;" width="100px">
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="Save" TabIndex="9"
                        OnClick="btnSave_Click" ValidationGroup="Save" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnBack" Text="Back" CausesValidation="false" CssClass="ClsBtn" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidStudentAdmisssionID" runat="server" Value="0" />
                    <asp:HiddenField ID="hidAcademicYearId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidEnquieryId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidNextAcademiYearId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidStatusId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidMinMaxDOBMap" runat="Server" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hidStandardName" runat="server" Value="" />
                     <asp:HiddenField ID="hidSchoolId" runat="server" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hidIsEnquiry" runat="server" ViewStateMode="Enabled"/>
                       <asp:HiddenField ID="hidServerDt" runat="server" ViewStateMode="Enabled" />
                    
                            <asp:HiddenField ID="hidSNSSchoolId" runat="server" ViewStateMode="Enabled"/>
                             <asp:HiddenField ID="hidIsSubjectSectionApplicable" runat="server" ViewStateMode="Enabled" Value="N"/>
                    <asp:HiddenField ID="hidValidateAadharCard" runat="server" Value="N" />                    
                </td>
            </tr>
        </table>        
    </div>
        <script language="javascript" type="text/javascript">
            _clienthidMaxBdate = "<%=this.hidMaxBdate.ClientID %>"
            _clienthidMinBdate = "<%=this.hidMinBdate.ClientID %>"
            _clientcmbStream = "<%=this.cmbStream.ClientID %>"
            _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>"
            _clienthidSNSSchoolId = "<%=this.hidSNSSchoolId.ClientID %>"
            _clienthidIsSubjectSectionApplicable = "<%=this.hidIsSubjectSectionApplicable.ClientID %>"
            var _clienttxtCalDobPopup = '<%= txtCalDobPopup.ClientID %>';
            _clienthidServerDt = "<%=this.hidServerDt.ClientID %>"
            var _clientcstDOB = '<%= cstDOB.ClientID %>';
            _clienttxtAadharCardNumber = '<%=this.txtAadharCardNumber.ClientID %>'
            _clienthidValidateAadharCard = '<%=this.hidValidateAadharCard.ClientID %>'
            
            function ValidateStream(oSrc, args) {

                var StreamId = document.getElementById(_clientcmbStream).value;
                var SchoolId = document.getElementById(_clienthidSchoolId).value;
                var SNSSchoolId = document.getElementById(_clienthidSNSSchoolId).value;

                if (SchoolId == SNSSchoolId && StreamId == "0") {
                    oSrc.errormessage = "Stream should be selected.";
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            function validateSubjectDetails(oSrc, args) {
                var ShowValidation = document.getElementById(_clienthidIsSubjectSectionApplicable).value;
                _clientrdoStream_SciGroupOne = "<%=this.rdoStream_SciGroupOne.ClientID %>"
                _clientrdoStream_SciGroupTwo = "<%=this.rdoStream_SciGroupTwo.ClientID %>"
                var StreamId = document.getElementById(_clientcmbStream).value;
                var GroupOneChecked = $("#<%= rdoStream_SciGroupOne.ClientID %>").is(":checked");
                var GroupTwoChecked = $("#<%= rdoStream_SciGroupTwo.ClientID %>").is(":checked");

                if (StreamId == 1) {
                    var OptionalSubject1;
                    var OptionalSubject2;
                    if (GroupOneChecked == false && GroupTwoChecked == false) {
                        oSrc.errormessage = "One group for science stream should be selected.";
                        args.IsValid = false;
                        return true;
                    }
                    else {
                        if (GroupOneChecked == true) {
                            OptionalSubject1 = $("#<%= rdoStream_SciGr1PhyEdu.ClientID %>").is(":checked");
                            OptionalSubject2 = $("#<%= rdoStream_SciGr1CompSci.ClientID %>").is(":checked");

                            if (OptionalSubject1 == false && OptionalSubject2 == false) {
                                oSrc.errormessage = "Optional Subject should be selected.";
                                args.IsValid = false;
                                return true;
                            }
                        }
                        else if (GroupTwoChecked == true) {
                            OptionalSubject1 = $("#<%= rdoStream_SciGr2PhyEdu.ClientID %>").is(":checked");
                            OptionalSubject2 = $("#<%= rdoStream_SciGr2CompSci.ClientID %>").is(":checked");

                            if (OptionalSubject1 == false && OptionalSubject2 == false) {
                                oSrc.errormessage = "Optional Subject should be selected.";
                                args.IsValid = false;
                                return true;
                            }
                        }
                    }
                }
                else if (StreamId == 2) {
                    OptionalSubject1 = $("#<%= rdoStream_ComMaths.ClientID %>").is(":checked");
                    OptionalSubject2 = $("#<%= rdoStream_ComPhyEdu.ClientID %>").is(":checked");
                    OptionalSubject3 = $("#<%= rdoStream_ComLeagalStudies.ClientID %>").is(":checked");

                    if (OptionalSubject1 == false && OptionalSubject2 == false && OptionalSubject3 == false) {
                        oSrc.errormessage = "Optional Subject should be selected.";
                        args.IsValid = false;
                        return true;
                    }
                }
                else if (StreamId == 3) {
                    var OptionalSubject3;
                    var OptionalSubject4;

                    OptionalSubject1 = $("#<%= rdoStream_ArtLegalSci.ClientID %>").is(":checked");
                    OptionalSubject2 = $("#<%= rdoStream_ArtPhyEdu.ClientID %>").is(":checked");
                    OptionalSubject3 = $("#<%= rdoStream_ArtGerman.ClientID %>").is(":checked");
                    OptionalSubject4 = $("#<%= rdoStream_ArtEconomics.ClientID %>").is(":checked");

                    if (OptionalSubject1 == false && OptionalSubject2 == false && OptionalSubject3 == false && OptionalSubject4 == false) {
                        oSrc.errormessage = "Optional Subject should be selected.";
                        args.IsValid = false;
                        return true;
                    }
                }
                else if (StreamId == 4) {
                    var True = $("#<%= rdoStream_AbrodEduNo.ClientID %>").is(":checked");
                    var False = $("#<%= rdoStream_AbrodEduYes.ClientID %>").is(":checked");

                    if (True == false && False == false) {
                        oSrc.errormessage = "Competitive exam coaching should be selected.";
                        args.IsValid = false;
                        return true;
                    }
                }
            }
            function ChangeStreamDetails(value) {
                var StreamId = document.getElementById(_clientcmbStream).value;
                //var StreamId = $(value).val();
                //var StreamId = $(value);
               
                $('[id*=rdoStream_]').prop('checked', false);
                $('[id*=chkStream_]').prop('checked', false);

                if (StreamId == "1") {
                    //document.getElementById('trScienceStream').style.display = '';
                    trScienceStream.Attributes.Add("style", "display:block");
                    $('#trScienceStream').show();
                    $('#trCommerceStream').hide();
                    $('#trArtsStream').hide();
                    $('#trAbroadEducation').hide();

                }
                else if (StreamId == "2") {
                    $('#trCommerceStream').show();
                    $('#trScienceStream').hide();
                    $('#trArtsStream').hide();
                    $('#trAbroadEducation').hide();
                }
                else if (StreamId == "3") {
                   
                    $('#trArtsStream').show();
                    $('#trScienceStream').hide();
                    $('#trCommerceStream').hide();
                    $('#trAbroadEducation').hide();
                }
                else if (StreamId == "4") {
                    $('#trAbroadEducation').show();
                    $('#trScienceStream').hide();
                    $('#trCommerceStream').hide();
                    $('#trArtsStream').hide();
                }
                else if (StreamId == "0") {
                    $('#trAbroadEducation').hide();
                    $('#trScienceStream').hide();
                    $('#trCommerceStream').hide();
                    $('#trArtsStream').hide();
                }
            }

            function ResetSubjects(id) {            
                if (id == 1)
                    ResetSubject(false)
                else
                    ResetSubject(true)
            }
            
            _clientrdoStream_SciGr1PhyEdu = "<%=this.rdoStream_SciGr1PhyEdu.ClientID %>"
            _clientrdoStream_SciGr1CompSci = "<%=this.rdoStream_SciGr1CompSci.ClientID %>"
            _clientchkStream_SciGr1JEE = "<%=this.chkStream_SciGr1JEE.ClientID %>"
            _clientchkStream_SciGr1ExtraCo = "<%=this.chkStream_SciGr1ExtraCo.ClientID %>"

            _clientrdoStream_SciGr2PhyEdu = "<%=this.rdoStream_SciGr2PhyEdu.ClientID %>"
            _clientrdoStream_SciGr2CompSci = "<%=this.rdoStream_SciGr2CompSci.ClientID %>"
            _clientchkStream_SciGr2Neet = "<%=this.chkStream_SciGr2Neet.ClientID %>"
            _clientchkStream_SciGr2ExtraCO = "<%=this.chkStream_SciGr2ExtraCO.ClientID %>"

            function ResetSubject(val) {                
                document.getElementById(_clientrdoStream_SciGr1PhyEdu).checked = false
                document.getElementById(_clientrdoStream_SciGr1CompSci).checked = false
                document.getElementById(_clientchkStream_SciGr1JEE).checked = false
                document.getElementById(_clientchkStream_SciGr1ExtraCo).checked = false

                document.getElementById(_clientrdoStream_SciGr1PhyEdu).disabled = val
                document.getElementById(_clientrdoStream_SciGr1CompSci).disabled = val
                document.getElementById(_clientchkStream_SciGr1JEE).disabled = val
                document.getElementById(_clientchkStream_SciGr1ExtraCo).disabled = val


                document.getElementById(_clientrdoStream_SciGr2PhyEdu).checked = false
                document.getElementById(_clientrdoStream_SciGr2CompSci).checked = false
                document.getElementById(_clientchkStream_SciGr2Neet).checked = false
                document.getElementById(_clientchkStream_SciGr2ExtraCO).checked = false

                document.getElementById(_clientrdoStream_SciGr2PhyEdu).disabled = !val
                document.getElementById(_clientrdoStream_SciGr2CompSci).disabled = !val
                document.getElementById(_clientchkStream_SciGr2Neet).disabled = !val
                document.getElementById(_clientchkStream_SciGr2ExtraCO).disabled = !val

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
        <script type="text/javascript" src="../Scripts/Admission/AdmissionFormStudentDetails.js?version=1.7"></script>

</asp:Content>